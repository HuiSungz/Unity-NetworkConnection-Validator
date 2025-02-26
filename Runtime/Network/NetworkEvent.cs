
using System;

namespace NetworkConnector
{
    /// <summary>
    /// 네트워크 연결 상태 변경 및 오류에 대한 이벤트를 관리합니다.
    /// </summary>
    internal static class NetworkEvent
    {
        /// <summary>
        /// 네트워크 연결 상태가 변경될 때 발생하는 이벤트입니다.
        /// 연결 상태가 이전과 다를 때만 발생합니다.
        /// </summary>
        internal static event Action<bool> OnConnectionStatusChanged;

        /// <summary>
        /// 네트워크 작업 중 예외가 발생했을 때 발생하는 이벤트입니다.
        /// 재시도 가능한 일반적인 네트워크 오류는 제외됩니다.
        /// </summary>
        internal static event Action<Exception> OnNetworkError;

        /// <summary>
        /// 네트워크 연결 시도가 성공했을 때 발생하는 이벤트입니다.
        /// RequestTryConnect 메서드에서 성공 시 발생합니다.
        /// </summary>
        internal static event Action OnConnectionSuccess;

        /// <summary>
        /// 네트워크 연결 시도가 실패했을 때 발생하는 이벤트입니다.
        /// RequestTryConnect 메서드에서 타임아웃이나 취소 시 발생합니다.
        /// </summary>
        internal static event Action OnConnectionFailed;

        internal static void RaiseConnectionStatusChanged(bool isConnected)
        {
            OnConnectionStatusChanged?.Invoke(isConnected);
        }

        internal static void RaiseNetworkError(Exception exception)
        {
            OnNetworkError?.Invoke(exception);
        }

        internal static void RaiseConnectionSuccess()
        {
            OnConnectionSuccess?.Invoke();
        }

        internal static void RaiseConnectionFailed()
        {
            OnConnectionFailed?.Invoke();
        }
    }
}