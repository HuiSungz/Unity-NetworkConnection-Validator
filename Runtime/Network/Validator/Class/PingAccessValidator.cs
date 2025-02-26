
using System;
using System.Threading;
using System.Threading.Tasks;
using NetworkConnector.Utils;
using UnityEngine;

namespace NetworkConnector.Validator
{
    internal class PingAccessValidator : INetworkAccessValidator
    {
        private readonly string[] _pingHosts;
        private readonly int _timeout;
        private readonly NetworkConfigSO _config;
        
        public PingAccessValidator(string[] pingHosts, int timeout)
        {
            _pingHosts = pingHosts;
            _timeout = timeout;
            _config = NetworkConfigSO.Instance;
        }
        
        public async Task<bool> ValidateAccessAsync(CancellationToken cancellationToken = default)
        {
            async Task<bool> SingleHostAttempt(string host, CancellationToken token)
            {
                try
                {
                    var ping = new Ping(host);
                    var pingStartTime = Time.time;

                    while (!ping.isDone)
                    {
                        if (token.IsCancellationRequested || 
                            (Time.time - pingStartTime) * 1000 > _timeout)
                        {
                            return false;
                        }
                        await Task.Delay(50, token);
                    }

                    return ping.isDone && ping.time >= 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return await RetryHelper.RetryWithBackoff(TryAllHosts, _timeout / 1000f, _config, cancellationToken);

            async Task<bool> TryAllHosts(CancellationToken token)
            {
                foreach (var host in _pingHosts)
                {
                    if (await SingleHostAttempt(host, token))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}