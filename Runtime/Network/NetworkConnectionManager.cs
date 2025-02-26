
using UnityEngine;

namespace NetworkConnector
{
    internal static class NetworkConnectionManager
    {
        #region Fields

        private static bool _currentConnectionStatus;
        private static readonly object StatusLock = new();

        #endregion

        #region Properties

        public static bool IsConnected
        {
            get
            {
                lock (StatusLock)
                {
                    return _currentConnectionStatus;
                }
            }
            private set
            {
                lock (StatusLock)
                {
                    if (_currentConnectionStatus == value)
                    {
                        return;
                    }
                    
                    _currentConnectionStatus = value;
                    RaiseConnectionStatusChanged(value);
                }
            }
        }

        #endregion

        #region Internal Methods

        internal static void UpdateConnectionStatus(bool newStatus, bool isSuccess)
        {
            IsConnected = newStatus;
            
            if (isSuccess)
            {
                NetworkEvent.RaiseConnectionSuccess();
                if (NetworkManager.Config.IsVerboseLog)
                {
                    Debug.Log("[NetworkConnectionManager] Connection successful");
                }
            }
            else if (!newStatus)
            {
                NetworkEvent.RaiseConnectionFailed();
                if (NetworkManager.Config.IsVerboseLog)
                {
                    Debug.Log("[NetworkConnectionManager] Connection failed");
                }
            }
        }

        #endregion

        #region Private Methods

        private static void RaiseConnectionStatusChanged(bool status)
        {
            NetworkEvent.RaiseConnectionStatusChanged(status);
            if (NetworkManager.Config.IsVerboseLog)
            {
                Debug.Log($"[NetworkConnectionManager] Connection status changed: {status}");
            }
        }

        #endregion
    }
}