using ShoppingCartCMC.Server.Shared;
using ShoppingCartCMC.Shared;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using ServiceStack;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ShoppingCartCMC.Server.Shared.Common;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public class RedisEntityRepositoryHelper : IRedisEntityRepositoryHelper
    {
        private IRedisClientsManager _redisManager;
        private readonly ILogger<RedisEntityRepositoryHelper> _logger;

        public RedisEntityRepositoryHelper(IRedisClientsManager redisManager, ILoggerFactory loggerFactory)
        {
            _redisManager   = redisManager;
            _logger         = loggerFactory.CreateLogger<RedisEntityRepositoryHelper>();
        }


        //PW: generic method to get Redis by id
        public T GetFromRedis<T>(string id)
        {
            using (var redis = this._redisManager.GetClient())
            {
                //PW: get from Redis
                var entity = redis.As<T>();
                T cache = entity.GetById(id);               

                return cache;
            }
        }


        //PW: generic method to get batch Redis by ids
        public IList<T> GetBatchFromRedis<T>(IEnumerable<string> ids)
        {
            using (var redis = this._redisManager.GetClient())
            {
                //PW: get from Redis
                var entity = redis.As<T>();
                IList<T> listCache = entity.GetByIds(ids);

                return listCache;
            }
        }



        public IList<T> GetAllFromRedis<T>()
        {
            using (var redis = this._redisManager.GetClient())
            {
                //PW: get from Redis
                var entity = redis.As<T>();
                IList<T> listCache = entity.GetAll();

                return listCache;
            }
        }



        public void SetExpiredSoonInRedis<T>(string id)
        {
            try
            {
                using (var redis = this._redisManager.GetClient())
                {
                    //PW: get from Redis
                    var entity = redis.As<T>();
                    T cache = entity.GetById(id);

                    if (cache == null || cache.GetId() == null) 
                    {
                        string k = redis.UrnKey<T>(id);
                        IRedis t = (IRedis)Activator.CreateInstance(typeof(T));
                        t.Dirty = true; //PW: !!

                        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++                       
                        redis.SetValue(k, JsonConvert.SerializeObject(t), Setting.StaleRedisCacheTolerance);   
                        //-------------------------------------------------------------------------------------------------------------------------------
                    }
                    else
                        entity.Store(cache, Setting.StaleRedisCacheTolerance);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _logger.LogError(ex.ToString());
            }
        }


        public void SetBatchExpiredSoonInRedis<T>(IEnumerable<string> ids)
        {
            foreach (string id in ids)
            {
                SetExpiredSoonInRedis<T>(id);
            }
        }



        public void StoreInRedis<T>(T t)
        {
            try
            {
                using (var redis = this._redisManager.GetClient())
                {
                    if (t == null) return;

                    var entity = redis.As<T>();
                    entity.Store(t, Setting.DefaultRedisCacheLifeTime);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _logger.LogError(ex.ToString());
            }
        }


        public void StoreInRedis<T>(T t, TimeSpan span)
        {
            try
            {
                using (var redis = this._redisManager.GetClient())
                {
                    if (t == null) return;

                    var entity = redis.As<T>();
                    entity.Store(t, span);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _logger.LogError(ex.ToString());
            }
        }


        public void StoreBatchInRedis<T>(IEnumerable<T> batch)
        {
            foreach (T t in batch)
            {
                StoreInRedis<T>(t);
            }
        }


        public void StoreBatchInRedis<T>(IEnumerable<T> batch, TimeSpan span)
        {
            foreach (T t in batch)
            {
                StoreInRedis<T>(t, span);
            }
        }


        public void DeleteInRedis<T>(string id)
        {
            try
            {
                using (var redis = this._redisManager.GetClient())
                {
                    if (id == null || id == "") return;

                    var entity = redis.As<T>();
                    entity.DeleteById(id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _logger.LogError(ex.ToString());
            }
        }



        public void DeleteBatchInRedis<T>(IEnumerable<string> ids)
        {
            using (var redis = this._redisManager.GetClient())
            {
                if (ids == null || ids.Count() <= 0) return;

                var entity = redis.As<T>();
                entity.DeleteByIds(ids);
            }
        }



        public void DeleteAllInRedis<T>()
        {
            using (var redis = this._redisManager.GetClient())
            {
                var entity = redis.As<T>();
                entity.DeleteAll();
            }
        }



        
        public List<string> ScanKeysInRedis(string pattern)
        {
            using (var redis = this._redisManager.GetClient())
            {
                return redis.ScanAllKeys(pattern).ToList();
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public void RemoveKeyInRedis(string key)
        {
            using (var redis = this._redisManager.GetClient())
            {
                redis.Remove(key);
            }
        }


        public void RemoveAllKeysInRedis(IEnumerable<string> keys)
        {
            using (var redis = this._redisManager.GetClient())
            {
                redis.RemoveAll(keys);
            }
        }



        public long GetNextSequence<T>()
        {
            using (var redis = this._redisManager.GetClient())
            {
                var entity = redis.As<T>();
                return entity.GetNextSequence();                
            }
        }

        public void SetSequence<T>(int seqId)
        {
            using (var redis = this._redisManager.GetClient())
            {
                var entity = redis.As<T>();
                entity.SetSequence(seqId);
            }
        }
        


        //PW: publish/subscribe
        public void PublishMessage(string channelName, string message)
        {
            try
            {
                using (var redis = this._redisManager.GetClient())
                {
                    redis.PublishMessage(channelName, message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _logger.LogError(ex.ToString());
            }

        }


        public void SubscribeToChannels(string[] channelNames, Action<string, string> messageHandler)
        {
            try
            {
                var redisPubSub = new RedisPubSubServer(this._redisManager, channelNames)
                {
                    OnMessage = (channel, msg) =>
                    {
                        messageHandler(channel, msg);
                    }
                }.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _logger.LogError(ex.ToString());
            }
        }
    }
}
