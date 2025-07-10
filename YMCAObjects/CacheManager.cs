/*
* Copyright YMCA Retirement Fund All Rights Reserved. 
 *
 * Project Name		:	YMCA-YRS
 * FileName			:	CacheManager.cs 
 * Author Name		:	Shashank Patel
 * Description      :   This class is used to handle caching in YRS application
 * Employee ID		:	55381
 * Creation Date	:	28-August-2014
 *************************************************************
 * Modified By          Date         Descritption
 * ***********************************************************
 * 
 * 
 * 
 * ***********************************************************
 * */
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
namespace YMCAObjects
{
   
    public class CacheManager
    {
        public sealed class CachePeriod
        {
            double dCachePeriod;
            private CachePeriod() { }
            private CachePeriod(double cachePeriod)
            {
                double dShortestCachePeriodInSeconds = 3600;
                string stShortestCachePeriodInSeconds;

                stShortestCachePeriodInSeconds = Convert.ToString(ConfigurationManager.AppSettings["ShortestCachePeriodInSeconds"]);
                if (!string.IsNullOrEmpty(stShortestCachePeriodInSeconds) && !string.IsNullOrWhiteSpace(stShortestCachePeriodInSeconds))
                    double.TryParse(ConfigurationManager.AppSettings["ShortestCachePeriodInSeconds"], out dShortestCachePeriodInSeconds);

                dCachePeriod = dShortestCachePeriodInSeconds * cachePeriod;
            }

            /// <summary>
            /// Returns the total cache period in seconds
            /// </summary>
            /// <returns>Total Cache Period</returns>
            public double ToSeconds()
            {
                return dCachePeriod;
            }
            /// <summary>
            /// Cache will be invalidated after a Shortest Cache Period specified in configuration elapse, default is 1 hour.
            /// </summary>
            public static readonly CachePeriod Short = new CachePeriod(1);//Default 3600 i.e. 1 Hour
            /// <summary>
            /// Cache will be invalidated after a Medium Cache Period elapse, default is (Shortest Cache Period specified in configuration)*3 hours.
            /// </summary>
            public static readonly CachePeriod Medium = new CachePeriod(3);//Default 10800 i.e. 3 Hours
            /// <summary>
            /// Cache will be invalidated after a Long Cache Period elapse, default is (Shortest Cache Period specified in configuration)*6 hours.
            /// </summary>
            public static readonly CachePeriod Long = new CachePeriod(6);//Default 21600 i.e. 6 Hours
            /// <summary>
            /// Cache will be invalidated after a ExtraLong Cache Period elapse, default is (Shortest Cache Period specified in configuration)*24 hours.
            /// </summary>
            public static readonly CachePeriod ExtraLong = new CachePeriod(24);//Default 86400 i.e. 24 Hours
        }

        private static ICacheManager iCacheManager;

        public CacheManager()
        {
          
        }

        
        private static ICacheManager CurrentCacheInstance
        {
            get
            {
                if (iCacheManager == null)
                {
                   iCacheManager = CacheFactory.GetCacheManager();
                    return iCacheManager;
                }
                else
                {
                    return iCacheManager;
                }
            }
        }

        public static void AddCache(string strKey, object value)
        {

            CurrentCacheInstance.Add(strKey, value);
        }

        public static void AddCache(string strKey, object value,CachePeriod cachePeriod)
        {
            CurrentCacheInstance.Add(strKey, value, CacheItemPriority.High, null, new AbsoluteTime(TimeSpan.FromSeconds(cachePeriod.ToSeconds())));
        }

        public static void AddCache(string strKey, object value, CachePeriod cachePeriod, ICacheItemRefreshAction refreshAction)
        {
            try
            {
                CurrentCacheInstance.Add(strKey, value, CacheItemPriority.High, refreshAction == null ? new CacheRefreshAction() : refreshAction, new AbsoluteTime(TimeSpan.FromSeconds(cachePeriod.ToSeconds())));
            }
            catch ( Exception ex )
            {
                //Log error 
                ExceptionPolicy.HandleException(ex, "Exception Policy");
            }
        }

       
       public static void RemoveCache(string strKey)
        {

                CurrentCacheInstance.Remove(strKey);
            
        }

        public static void ClearAllCache()
        {

            CurrentCacheInstance.Flush();        
        }

        public static bool KeyExists(string strKey)
        {

            return CurrentCacheInstance.Contains(strKey);
        }

        public static object GetDataFromCache(string strKey)
        {

            return CurrentCacheInstance.GetData(strKey);
        }

        [Serializable]
        public class CacheRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
                // Item has been removed from cache. Perform desired actions here, based on
                
                string strExpiredExpression =string.Format( "Cached item {0} was expired in the cache with the reason '{1}'", key, removalReason);
                string strExpiredItem = string.Format(string.Format("Item values were: Key = {0}, Values = '{1}'", key, expiredValue));
                 Logger.Write(strExpiredExpression + "    " + strExpiredItem, "Application", 0, 0, TraceEventType.Information);

                
                // Refresh the cache if it expired, but not if it was explicitly removed
                if (removalReason == CacheItemRemovedReason.Expired)
                {
                    //CacheManager defaultCache = EnterpriseLibraryContainer.Current.GetInstance
                    //                            <CacheManager>("InMemoryCacheManager");
                    //defaultCache.Add(key, new Product(10, "Exciting Thing",
                    //                 "Useful for everything"), CacheItemPriority.Low,
                    //                 new MyCacheRefreshAction(),
                    //                 new SlidingTime(new TimeSpan(0, 0, 10)));
                    //Logger.Write(("Refreshed the item by adding it to the cache again.", "Application", 0, 0, TraceEventType.Information);
                }
            }
        }

    }
}