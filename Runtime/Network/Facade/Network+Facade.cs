
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Network
{
    public static class NConnector
    {
        #region Properties

        /// <summary>
        /// 현재 네트워크 연결 상태를 반환합니다.
        /// </summary>
        public static bool IsConnected => NetworkConnectionManager.IsConnected;

        #endregion

        #region Public Methods - Connection Check

        /// <summary>
        /// 네트워크 연결 상태를 확인합니다.
        /// </summary>
        /// <param name="onSucceeded">연결 성공 시 호출될 콜백</param>
        /// <param name="onFailed">연결 실패 시 호출될 콜백</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        public static void ValidationConnection(
            Action onSucceeded = null,
            Action onFailed = null,
            NetworkValidationType type = NetworkValidationType.Default)
        {
            NetworkManager.RequestIsConnected(onSucceeded, onFailed, type);
        }

        /// <summary>
        /// 네트워크 연결 상태를 비동기적으로 확인합니다.
        /// </summary>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        /// <returns>연결 성공 여부</returns>
        public static async Task<bool> ValidationConnectionAsync(
            NetworkValidationType type = NetworkValidationType.Default)
        {
            return await NetworkManager.RequestIsConnectedAsync(type);
        }

        #endregion

        #region Public Methods - Connection Attempt

        /// <summary>
        /// 지정된 시간 동안 네트워크 연결을 시도합니다.
        /// </summary>
        /// <param name="duration">최대 시도 시간(초)</param>
        /// <param name="onSucceeded">연결 성공 시 호출될 콜백</param>
        /// <param name="onFailed">연결 실패 시 호출될 콜백</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        public static void TryConnect(
            float duration,
            Action onSucceeded = null,
            Action onFailed = null,
            NetworkValidationType type = NetworkValidationType.Default)
        {
            NetworkManager.RequestTryConnect(duration, onSucceeded, onFailed, type);
        }

        /// <summary>
        /// 지정된 시간 동안 네트워크 연결을 비동기적으로 시도합니다.
        /// </summary>
        /// <param name="duration">최대 시도 시간(초)</param>
        /// <param name="type">확인할 네트워크 연결 유형</param>
        /// <returns>연결 성공 여부</returns>
        public static async Task<bool> TryConnectAsync(
            float duration,
            NetworkValidationType type = NetworkValidationType.Default)
        {
            return await NetworkManager.RequestTryConnect(duration, type);
        }

        #endregion

        #region Public Methods - Connection Tracking

        /// <summary>
        /// 네트워크 연결 상태 추적을 시작합니다.
        /// </summary>
        /// <param name="type">추적할 네트워크 연결 유형</param>
        /// <returns>추적 작업을 제어하기 위한 CancellationTokenSource</returns>
        public static CancellationTokenSource StartTracking(
            NetworkValidationType type = NetworkValidationType.Default)
        {
            return NetworkManager.StartConnectionTracking(type);
        }

        /// <summary>
        /// 네트워크 연결 상태 추적을 중지합니다.
        /// </summary>
        public static void StopTracking()
        {
            NetworkManager.StopConnectionTracking();
        }

        #endregion

        #region Public Methods - Events

        /// <summary>
        /// 네트워크 상태 변경 이벤트에 구독합니다.
        /// </summary>
        public static void SubscribeStatusChanged(Action<bool> callback)
        {
            NetworkEvent.OnConnectionStatusChanged += callback;
        }

        /// <summary>
        /// 네트워크 상태 변경 이벤트 구독을 해제합니다.
        /// </summary>
        public static void UnsubscribeStatusChanged(Action<bool> callback)
        {
            NetworkEvent.OnConnectionStatusChanged -= callback;
        }

        /// <summary>
        /// 네트워크 연결 성공 이벤트에 구독합니다.
        /// </summary>
        public static void SubscribeConnected(Action callback)
        {
            NetworkEvent.OnConnectionSuccess += callback;
        }

        /// <summary>
        /// 네트워크 연결 성공 이벤트 구독을 해제합니다.
        /// </summary>
        public static void UnsubscribeConnected(Action callback)
        {
            NetworkEvent.OnConnectionSuccess -= callback;
        }

        /// <summary>
        /// 네트워크 연결 실패 이벤트에 구독합니다.
        /// </summary>
        public static void SubscribeDisconnected(Action callback)
        {
            NetworkEvent.OnConnectionFailed += callback;
        }

        /// <summary>
        /// 네트워크 연결 실패 이벤트 구독을 해제합니다.
        /// </summary>
        public static void UnsubscribeDisconnected(Action callback)
        {
            NetworkEvent.OnConnectionFailed -= callback;
        }

        /// <summary>
        /// 네트워크 오류 이벤트에 구독합니다.
        /// </summary>
        public static void SubscribeError(Action<Exception> callback)
        {
            NetworkEvent.OnNetworkError += callback;
        }

        /// <summary>
        /// 네트워크 오류 이벤트 구독을 해제합니다.
        /// </summary>
        public static void UnsubscribeError(Action<Exception> callback)
        {
            NetworkEvent.OnNetworkError -= callback;
        }

        #endregion
    }
}
