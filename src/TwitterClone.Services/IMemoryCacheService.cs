using System.Threading.Tasks;

namespace TwitterClone.Services
{
    public interface IMemoryCacheService
    {
        Task<long> GetTweetCountAsync();
        Task<long>IncrementTweetCountAsync();
    }
}