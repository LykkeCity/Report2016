using System;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using Reports2016.Domains;
using Common;

namespace Report2016.AzureRepositories
{
    public class VoteEntity : TableEntity, IVote
    {
        public static string GeneratePartitionKey()
        {
            return "v";
        }

        public static string GenerateRowKey(string userId)
        {
            return userId;
        }

        public string UserId => PartitionKey;

        public string Email { get; set; }


        public string Option { get; set; }

        public void SetOption(VoteOption src)
        {
            Option = src.ToString();
        }

        VoteOption IVote.Option => Option.ParseEnum<VoteOption>(VoteOption.NotSure);

        public static VoteEntity Create(IVote src)
        {
            var result = new VoteEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.UserId),
                Email = src.Email,
            };

            result.SetOption(src.Option);

            return result;

        }

    }


    public class VotesRepository : IVotesRepository
    {

        INoSQLTableStorage<VoteEntity> _tableStorage;

        public VotesRepository(INoSQLTableStorage<VoteEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IVote> GetAsync(string userId)
        {
            var partitionKey = VoteEntity.GeneratePartitionKey();
            var rowKey = VoteEntity.GenerateRowKey(userId);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<VoteResult> VoteAsync(IVote vote)
        {
            try
            {
                var entity = VoteEntity.Create(vote);
                await _tableStorage.InsertAsync(entity);
                return VoteResult.Accepted;
            }
            catch (Exception)
            {
                return VoteResult.VoteIsAlreadyMade;
            }
        }
    }
}
