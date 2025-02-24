
using System.Collections.Generic;
using System.Threading;
using Network.Validator;

namespace Network
{
    internal static partial class NetworkManager
    {
        #region Fields - Base

        private static readonly Dictionary<NetworkValidationType, INetworkAccessValidator> Validators;
        private static CancellationTokenSource _connectionTrackingCts;
        
        public static NetworkConfigSO Config { get; }

        static NetworkManager()
        {
            Config = NetworkConfigSO.Instance;
            Validators = new Dictionary<NetworkValidationType, INetworkAccessValidator>
            {
                {NetworkValidationType.UnityNetwork, new UnityAccessValidator()},
                {NetworkValidationType.Http, new HttpAccessValidator(Config.HttpUrls, Config.HttpTimeout)},
                {NetworkValidationType.Ping, new PingAccessValidator(Config.PingHosts, Config.PingTimeout)}
            };
        }

        #endregion

        #region Fields - Connection

        private static readonly object ConnectionLock = new();
        
        private static bool _isTracking;
        private static bool _lastConnectionStatus;

        #endregion
        
        private static void ResetCancellationToken()
        {
            lock (ConnectionLock)
            {
                _connectionTrackingCts?.Cancel();
                _connectionTrackingCts?.Dispose();
                _connectionTrackingCts = new CancellationTokenSource();
            }
        }

        private static CancellationToken CurrentToken
        {
            get
            {
                lock (ConnectionLock)
                {
                    if (_connectionTrackingCts == null || _connectionTrackingCts.IsCancellationRequested)
                    {
                        _connectionTrackingCts = new CancellationTokenSource();
                    }
                    return _connectionTrackingCts.Token;
                }
            }
        }
    }
}