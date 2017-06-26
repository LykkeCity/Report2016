using System;
using System.Threading.Tasks;

namespace Reports2016.Domains
{

    public interface IVoteToken{
        string Token { get; set; }
        string Email { get; set; }
    }

    public interface IVoteTokensRepository
    {

        Task<IVoteToken> FindTokenAsync(string token);

    }

}
