
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NetworkConnector
{
    internal static partial class NetworkManager
    {
        #region Public Methods - Connection Tracking 

        /// <summary>
        /// 네트워크 연결 상태 추적을 시작합니다.
        /// 이미 추적 중인 경우 현재 사용 중인 CancellationTokenSource를 반환합니다.
        /// </summary>
        /// <param name="type">추적할 네트워크 연결 유형</param>
        /// <returns>추적 작업을 제어하기 위한 CancellationTokenSource</returns>
        public static CancellationTokenSource StartConnectionTracking(NetworkValidationType type = NetworkValidationType.Default)
        {
            lock (ConnectionLock)
            {
                if (_isTracking)
                {
                    return _connectionTrackingCts;
                }

                ResetCancellationToken();
                _isTracking = true;
                _ = TrackConnectionInternal(CurrentToken, type);
                return _connectionTrackingCts;
            }
        }
        
        /// <summary>
        /// 현재 진행 중인 네트워크 연결 상태 추적을 중지합니다.
        /// </summary>
        public static void StopConnectionTracking()
        {
            lock (ConnectionLock)
            {
                _isTracking = false;
                _connectionTrackingCts?.Cancel();
            }
        }

        #endregion
        
        private static async Task TrackConnectionInternal(CancellationToken cancellationToken, NetworkValidationType type)
        {
            try
            {
                var currentDelay = Config.BaseRetryInterval;
                
                while (_isTracking && !cancellationToken.IsCancellationRequested)
                {
                    var currentStatus = await RequestIsConnectedAsync(type);
                    NetworkConnectionManager.UpdateConnectionStatus(currentStatus, currentStatus);
                    
                    if (currentStatus)
                    {
                        currentDelay = Config.BaseRetryInterval;
                    }
                    else
                    {
                        currentDelay = Mathf.Min(
                            Mathf.RoundToInt(currentDelay * Config.RetryBackoffMultiplier),
                            Config.MaxRetryInterval
                        );
                    }

                    await Task.Delay(currentDelay, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                NetworkConnectionManager.UpdateConnectionStatus(false, false);
            }
            catch (Exception exception)
            {
                NetworkEvent.RaiseNetworkError(exception);
                NetworkConnectionManager.UpdateConnectionStatus(false, false);
            }
            finally
            {
                _isTracking = false;
            }
        }
    }
}