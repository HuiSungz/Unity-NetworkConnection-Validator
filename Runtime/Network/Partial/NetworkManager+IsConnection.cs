
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    internal static partial class NetworkManager
    {
        #region Public Methods - Connection

        /// <summary>
        /// 네트워크 연결 상태를 비동기적으로 확인하고 결과를 콜백으로 전달합니다.
        /// </summary>
        /// <param name="onSucceeded">연결 성공 시 호출될 콜백</param>
        /// <param name="onFailed">연결 실패 시 호출될 콜백</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        public static void RequestIsConnected(Action onSucceeded, Action onFailed,
            NetworkValidationType type = NetworkValidationType.Default)
        {
            _ = RequestIsConnectedInternal(type, onSucceeded, onFailed, CurrentToken);
        }
        
        /// <summary>
        /// 네트워크 연결 상태를 비동기적으로 확인합니다.
        /// </summary>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        /// <returns>연결 성공 여부</returns>
        public static async Task<bool> RequestIsConnectedAsync(
            NetworkValidationType type = NetworkValidationType.Default)
        {
            return await RequestIsConnectedInternal(type, cancellationToken: CurrentToken);
        }
        
        /// <summary>
        /// 지정된 시간 동안 네트워크 연결을 시도하고 결과를 콜백으로 전달합니다.
        /// </summary>
        /// <param name="duration">최대 시도 시간(초)</param>
        /// <param name="onSucceeded">연결 성공 시 호출될 콜백</param>
        /// <param name="onFailed">연결 실패 시 호출될 콜백</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        public static void RequestTryConnect(float duration, Action onSucceeded, Action onFailed,
            NetworkValidationType type = NetworkValidationType.Default)
        {
            ResetCancellationToken();
            
            _ = RequestTryConnectInternal(duration, type, CurrentToken)
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {
                        if (Config.IsVerboseLog)
                        {
                            Debug.Log("[NetworkManager] Connection successful (callback)");
                        }
                        NetworkEvent.RaiseConnectionSuccess();
                        onSucceeded?.Invoke();
                    }
                    else
                    {
                        if (Config.IsVerboseLog)
                        {
                            Debug.Log("[NetworkManager] Connection attempts failed within duration (callback)");
                        }
                        NetworkEvent.RaiseConnectionFailed();
                        onFailed?.Invoke();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 지정된 시간 동안 네트워크 연결을 시도합니다.
        /// </summary>
        /// <param name="duration">최대 시도 시간(초)</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        /// <returns>연결 성공 여부</returns>
        public static async Task<bool> RequestTryConnect(
            float duration,
            NetworkValidationType type = NetworkValidationType.Default)
        {
            ResetCancellationToken();
            return await RequestTryConnectInternal(duration, type, CurrentToken);
        }

        #endregion
        
        /// <summary>
        /// 네트워크 연결 상태를 내부적으로 확인하는 메서드입니다.
        /// </summary>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        /// <param name="onSucceeded">연결 성공 시 호출될 콜백</param>
        /// <param name="onFailed">연결 실패 시 호출될 콜백</param>
        /// <param name="cancellationToken">작업 취소 토큰</param>
        /// <returns>연결 성공 여부</returns>
        private static async Task<bool> RequestIsConnectedInternal(
            NetworkValidationType type, 
            Action onSucceeded = null, 
            Action onFailed = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (type.HasFlag(NetworkValidationType.UnityNetwork)
                    && !await Validators[NetworkValidationType.UnityNetwork].ValidateAccessAsync(cancellationToken))
                {
                    onFailed?.Invoke();
                    NetworkConnectionManager.UpdateConnectionStatus(false, false);
                    return false;
                }

                var tasks = new List<Task<bool>>();

                if (type.HasFlag(NetworkValidationType.Http))
                {
                    tasks.Add(Validators[NetworkValidationType.Http].ValidateAccessAsync(cancellationToken));
                }
                if (type.HasFlag(NetworkValidationType.Ping))
                {
                    tasks.Add(Validators[NetworkValidationType.Ping].ValidateAccessAsync(cancellationToken));
                }

                if (tasks.Count == 0)
                {
                    onSucceeded?.Invoke();
                    NetworkConnectionManager.UpdateConnectionStatus(true, true);
                    return true;
                }

                var results = await Task.WhenAll(tasks);
                var isConnected = results.All(result => result);
                
                NetworkConnectionManager.UpdateConnectionStatus(isConnected, isConnected);
                
                if (isConnected)
                {
                    onSucceeded?.Invoke();
                }
                else
                {
                    onFailed?.Invoke();
                }

                return isConnected;
            }
            catch (OperationCanceledException)
            {
                onFailed?.Invoke();
                NetworkConnectionManager.UpdateConnectionStatus(false, false);
                throw;
            }
        }

        /// <summary>
        /// 지정된 시간 동안 네트워크 연결을 내부적으로 시도하는 메서드입니다.
        /// 지수 백오프를 사용하여 재시도 간격을 조절합니다.
        /// </summary>
        /// <param name="duration">최대 시도 시간(초)</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        /// <param name="cancellationToken">작업 취소 토큰</param>
        /// <returns>연결 성공 여부</returns>
        private static async Task<bool> RequestTryConnectInternal(
            float duration,
            NetworkValidationType type,
            CancellationToken cancellationToken)
        {
            var startTime = Time.time;
            var currentDelay = Config.BaseRetryInterval;

            try
            {
                while (Time.time - startTime < duration && !cancellationToken.IsCancellationRequested)
                {
                    var isConnected = await RequestIsConnectedAsync(type);
                    
                    if (isConnected)
                    {
                        NetworkConnectionManager.UpdateConnectionStatus(true, true);
                        return true;
                    }

                    currentDelay = Mathf.Min(
                        Mathf.RoundToInt(currentDelay * Config.RetryBackoffMultiplier),
                        Config.MaxRetryInterval
                    );
                    
                    await Task.Delay(currentDelay, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                NetworkConnectionManager.UpdateConnectionStatus(false, false);
                throw;
            }

            NetworkConnectionManager.UpdateConnectionStatus(false, false);
            return false;
        }
    }
}