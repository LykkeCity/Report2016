using Reports2016.Domains;
using Microsoft.Extensions.DependencyInjection;
using Common.Log;
using AzureStorage.Tables;

namespace Report2016.AzureRepositories
{
    public static class AzureStorageBinder
    {

        public static void BindAzureRepositories(this IServiceCollection serviceCollection, string connectionString, ILog log)
        {

            var votesRepository = new VotesRepository(
                new AzureTableStorage<VoteEntity>(connectionString, "Votes", log));

            serviceCollection.AddSingleton<IVotesRepository>(votesRepository);

            var voteTokensRepository = new VoteTokensRepository(
                new AzureTableStorage<VoteTokenEntity>(connectionString, "VoteTokens", log));

            serviceCollection.AddSingleton<IVoteTokensRepository>(voteTokensRepository);

        }

    }
}
