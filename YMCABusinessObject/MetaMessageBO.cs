//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		    :	YMCA YRS
// FileName			    :	MetaMessageDA.cs
// Author Name		    :	Shashank Patel
// Creation Time		:	07/31/2014
// Desctiption          :   This class is used for get the records in metamessage object from DA layer
//                          YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Pramod P. Pokale     2018.04.27    YRS-AT-3101: YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//*****************************************************
using YMCARET.YmcaDataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMCAObjects;

namespace YMCARET.YmcaBusinessObject
{
    public class MetaMessageBO
    {
        private const string Meta_Messages = "META_MESSAGES";
        
        public static MetaMessage GetMessageByMessageNo(int messageNo,Dictionary<string,string> dictParmaeters = null )
        {

          MetaMessage metaMessage;

            try
            {
              
                metaMessage = GetMessageByMessageNo(messageNo);               
            
                //check if message number exists in the list
                if (metaMessage != null)
                {

                    if (dictParmaeters != null)
                    {
                        foreach (KeyValuePair<string, string> keyPair in dictParmaeters)
                        {
                            metaMessage.DisplayText = metaMessage.DisplayText.Replace("$$" + keyPair.Key + "$$", keyPair.Value);
                        }
                    }
                }
                else
                {
                    throw new Exception("Message not found for message number " + messageNo.ToString());
                }
                
            }
            catch (Exception)
            {
                throw;
            }
           
            return metaMessage;
        }

        public static string GetMessageByTextMessageNo(int messageNo, Dictionary<string, string> dictParmaeters = null)
        {
            MetaMessage metaMessage;
            try
            {
                metaMessage = GetMessageByMessageNo(messageNo, dictParmaeters);
            }
            catch (Exception)
            {

                throw;
            }

            return metaMessage.DisplayText;

            
        }

        public static List<MetaMessage> GetMessageList()
        {
            List<MetaMessage> lstMetaMessage;
            try
            {
                lstMetaMessage = MetaMessageDA.GetMessages();
            }
            catch 
            {                
                throw;
            }
            return lstMetaMessage;
        }

        public static int UpdateMessage(MetaMessage metaMessage)
        {
            try
            {
                return MetaMessageDA.UpdateMessage(metaMessage);
            }
            catch
            {
                throw;
            }
        }

       public static void ClearMessagesCache()
        {

            try
            {
                CacheManager.RemoveCache(Meta_Messages);
            }
            catch
            {
                throw;
            }

        }

        private static MetaMessage GetMessageByMessageNo(int messageNo)
        {
            object cacheData;            
            List<MetaMessage> messageCacheList;
            MetaMessage metaMessage;
            try
            {
                messageCacheList = new List<MetaMessage>();
                metaMessage = new MetaMessage();
                cacheData = CacheManager.GetDataFromCache(Meta_Messages);

                if (cacheData == null)
                {         
                    messageCacheList.AddRange(MetaMessageDA.GetMessages());
                    CacheManager.AddCache(Meta_Messages, messageCacheList, CacheManager.CachePeriod.ExtraLong);
                    cacheData = CacheManager.GetDataFromCache(Meta_Messages);
                }

                messageCacheList = (List<MetaMessage>)cacheData;                  
                //check if message exists then return message from cache
                if (messageCacheList.Exists(p => p.MessageNo == messageNo))
                {
                    metaMessage = messageCacheList.SingleOrDefault(p => p.MessageNo == messageNo);
                    return (MetaMessage) metaMessage.Clone();
                }
                else
                {
                    throw new Exception("No Message defined for Message No: " + messageNo.ToString());
                }

            }
            catch
            {
                throw;
            }

        }

        //Note: the below commented code wil be used in future purpose, please don't remove
        //private static MetaMessage GetMessageByMessageNo(int messageNo)
        //{ 
        // object cacheData;
        // List<MetaMessage> lstMetaMessage;
        // List<MetaMessage> messageCacheList;

        //try
        // {
        //     messageCacheList = new List<MetaMessage>();
              
        //     cacheData = CacheManager.GetDataFromCache(Meta_Messages);

        //     if (cacheData != null)
        //     {
        //         messageCacheList = (List<MetaMessage>)cacheData;

        //         //check if message exists then return message from cache
        //         if (messageCacheList.Exists(p => p.MessageNo == messageNo))
        //         {
        //             return messageCacheList.SingleOrDefault(p => p.MessageNo == messageNo);
        //         }               

        //     }
             
        //     lstMetaMessage = MetaMessageDA.GetMetaMessagesModuleWiseByMessageNo(messageNo);


        //     if (messageCacheList == null)
        //     {
        //         messageCacheList = new List<MetaMessage>();
        //     }
        //        messageCacheList.AddRange(lstMetaMessage);
        //        CacheManager.AddCache(Meta_Messages, messageCacheList,CacheManager.CachePeriod.ExtraLong);
                
        //        //check if message exists then return message from cache
        //        if (messageCacheList.Exists(p => p.MessageNo == messageNo))
        //        {
        //            return messageCacheList.SingleOrDefault(p => p.MessageNo == messageNo);
        //        }
        //        else
        //        {
        //            return null;
        //        }

        // }
        // catch
        // {
        //     throw;
        // }
        
        //}

        //private static List<MetaMessage> GetMetaMessagesModuleWiseByMessageNo(int messageNo)
        //{
        //    List<MetaMessage> lstMetaMessage;
           
        //    try
        //    {
               
        //            lstMetaMessage = MetaMessageDA.GetMetaMessagesModuleWiseByMessageNo(messageNo);                 

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return lstMetaMessage;
        //}

        //START: PPP | 04/27/2018 | YRS-AT-3101 | Similar to GetMessageByMessageNo(int, Dictionary<string, string>), this function id searching messages based on message code
        /// <summary>
        /// Provides message based on given message code.
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="dictParmaeters">Dynamic values needs to be updated in message</param>
        /// <returns>Message</returns>
        public static MetaMessage GetMessageByCode(string messageCode, Dictionary<string, string> dictParmaeters = null)
        {
            MetaMessage metaMessage;
            try
            {

                metaMessage = GetMessageByCode(messageCode);

                //check if message number exists in the list
                if (metaMessage != null)
                {

                    if (dictParmaeters != null)
                    {
                        foreach (KeyValuePair<string, string> keyPair in dictParmaeters)
                        {
                            metaMessage.DisplayText = metaMessage.DisplayText.Replace("$$" + keyPair.Key + "$$", keyPair.Value);
                        }
                    }
                    return metaMessage;
                }
                else
                {
                    throw new Exception("Message not found for message number " + messageCode.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                metaMessage = null;
            }
        }

        /// <summary>
        /// Provides message based on given message code.
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <returns>Message</returns>
        private static MetaMessage GetMessageByCode(string messageCode)
        {
            object cacheData;
            List<MetaMessage> messageCacheList;
            MetaMessage metaMessage;
            try
            {
                messageCacheList = new List<MetaMessage>();
                metaMessage = new MetaMessage();
                cacheData = CacheManager.GetDataFromCache(Meta_Messages);

                if (cacheData == null)
                {
                    messageCacheList.AddRange(MetaMessageDA.GetMessages());
                    CacheManager.AddCache(Meta_Messages, messageCacheList, CacheManager.CachePeriod.ExtraLong);
                    cacheData = CacheManager.GetDataFromCache(Meta_Messages);
                }

                messageCacheList = (List<MetaMessage>)cacheData;
                //check if message exists then return message from cache
                if (messageCacheList.Exists(p => p.DisplayCode == messageCode))
                {
                    metaMessage = messageCacheList.SingleOrDefault(p => p.DisplayCode == messageCode);
                    return (MetaMessage)metaMessage.Clone();
                }
                else
                {
                    throw new Exception("No Message defined for Message No: " + messageCode.ToString());
                }
            }
            catch
            {
                throw;
            }
            finally 
            {
                metaMessage = null;
                messageCacheList = null;
                cacheData = null;
            }
        }
        //END: PPP | 04/27/2018 | YRS-AT-3101 | Similar to GetMessageByMessageNo(int, Dictionary<string, string>), this function id searching messages based on message code
    }
}
