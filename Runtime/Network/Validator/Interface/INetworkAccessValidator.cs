
using System.Threading;
using System.Threading.Tasks;

namespace Network.Validator
{
    internal interface INetworkAccessValidator
    {
        /// <summary>
        /// 네트워크 접근 가능 여부를 검증합니다.
        /// </summary>
        /// <param name="cancellationToken">작업 취소를 위한 토큰</param>
        /// <returns>접근 가능 여부</returns>
        Task<bool> ValidateAccessAsync(CancellationToken cancellationToken = default);
    }
}