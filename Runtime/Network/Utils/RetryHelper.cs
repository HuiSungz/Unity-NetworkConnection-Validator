
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NetworkConnector.Utils
{
    internal static class RetryHelper
    {
        public static async Task<T> RetryWithBackoff<T>(
            Func<CancellationToken, Task<T>> operation,
            float timeoutSeconds,
            NetworkConfigSO config,
            CancellationToken cancellationToken = default)
        {
            var startTime = Time.time;
            var currentDelay = config.BaseRetryInterval;
            
            while (Time.time - startTime < timeoutSeconds && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await operation(cancellationToken);
                    if (result != null && !result.Equals(default(T)))
                    {
                        return result;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    if (config.IsVerboseLog)
                    {
                        Debug.LogWarning($"[RetryHelper] Operation failed: {exception.Message}");
                    }
                }

                try
                {
                    await Task.Delay(currentDelay, cancellationToken);
                    
                    currentDelay = Mathf.Min(
                        Mathf.RoundToInt(currentDelay * config.RetryBackoffMultiplier),
                        config.MaxRetryInterval
                    );
                }
                catch (OperationCanceledException)
                {
                    // Ignored
                }
            }

            return default;
        }
    }
}