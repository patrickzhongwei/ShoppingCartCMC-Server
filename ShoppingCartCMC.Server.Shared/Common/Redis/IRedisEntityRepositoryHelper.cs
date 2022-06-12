using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public interface IRedisEntityRepositoryHelper
    {
        T GetFromRedis<T>(string id);

        IList<T> GetBatchFromRedis<T>(IEnumerable<string> ids);

        IList<T> GetAllFromRedis<T>();

        void SetExpiredSoonInRedis<T>(string id);

        void SetBatchExpiredSoonInRedis<T>(IEnumerable<string> ids);

        void StoreInRedis<T>(T t);

        void StoreInRedis<T>(T t, TimeSpan span);

        void StoreBatchInRedis<T>(IEnumerable<T> batch);

        void StoreBatchInRedis<T>(IEnumerable<T> batch, TimeSpan span);

        void DeleteBatchInRedis<T>(IEnumerable<string> ids);

        void DeleteInRedis<T>(string id);

        void DeleteAllInRedis<T>();


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++       
        List<string> ScanKeysInRedis(string pattern);
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

        void RemoveKeyInRedis(string key);
        
        void RemoveAllKeysInRedis(IEnumerable<string> keys);

        long GetNextSequence<T>();

        void SetSequence<T>(int seqId);

       
        void PublishMessage(string channelName, string message);

        void SubscribeToChannels(string[] channelNames, Action<string, string> messageHandler);
    }
}
