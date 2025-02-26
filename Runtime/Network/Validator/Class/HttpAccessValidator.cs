
using System;
using System.Threading;
using System.Threading.Tasks;
using NetworkConnector.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkConnector.Validator
{
    internal class HttpAccessValidator : INetworkAccessValidator
    {
        private readonly string[] _httpUrls;
        private readonly float _timeout;
        private readonly NetworkConfigSO _config;
        
        public HttpAccessValidator(string[] httpUrls, float timeout)
        {
            _httpUrls = httpUrls;
            _timeout = timeout;
            _config = NetworkConfigSO.Instance;
        }
        
        public async Task<bool> ValidateAccessAsync(CancellationToken cancellationToken = default)
        {
            async Task<bool> SingleUrlAttempt(string url, CancellationToken token)
            {
                try
                {
                    using var request = UnityWebRequest.Head(url);
                    request.timeout = Mathf.CeilToInt(_timeout);
                    
                    var operation = request.SendWebRequest();
                    while (!operation.isDone)
                    {
                        if (token.IsCancellationRequested)
                        {
                            request.Abort();
                            return false;
                        }
                        await Task.Delay(50, token);
                    }

#if UNITY_2020_1_OR_NEWER
                    return request.result == UnityWebRequest.Result.Success;
#else
                    return !request.isNetworkError && !request.isHttpError;
#endif
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return await RetryHelper.RetryWithBackoff(TryAllUrls, _timeout, _config, cancellationToken);

            async Task<bool> TryAllUrls(CancellationToken token)
            {
                foreach (var url in _httpUrls)
                {
                    if (await SingleUrlAttempt(url, token))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}