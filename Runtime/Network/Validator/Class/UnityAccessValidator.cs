
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NetworkConnector.Validator
{
    internal class UnityAccessValidator : INetworkAccessValidator
    {
        public Task<bool> ValidateAccessAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromResult(false);
            }
            
            var hasAccess = Application.internetReachability != NetworkReachability.NotReachable;
            return Task.FromResult(hasAccess);
        }
    }
}