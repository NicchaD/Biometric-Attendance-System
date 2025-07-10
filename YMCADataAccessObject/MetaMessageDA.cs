//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		    :	YMCA YRS
// FileName			    :	MetaMessageDA.cs
// Author Name		    :	Shashank Patel
// Creation Time		:	07/31/2014
// Desctiption          :   This class is used for get the records from atsmetamessage table.
//                          YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
//*******************************************************************************
//Modification History  :
//*******************************************************************************
//Modified Date		    Modified By	   Description
//*******************************************************************************
//Anudeep A             2015.08.12     YRS 5.0-2441-Modifications for 403b Loans
//Manthan Rajguru       2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using YMCAObjects;

namespace YMCARET.YmcaDataAccessObject
{
   public class MetaMessageDA
    {

       public static List<MetaMessage> GetMessages()
        {
            DbCommand dbCommand = null;
            Database db;
            List<MetaMessage> lstMetaMessages;
           try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                lstMetaMessages = new List<MetaMessage>();

                if (db == null) return null;

                dbCommand = db.GetStoredProcCommand("yrs_usp_Msg_GetMetaMessages");

                if (dbCommand == null) return null;

               lstMetaMessages = GetAllMessages(dbCommand, db);

               return lstMetaMessages;
            }
            catch
            {
                throw;
            }
        }

       private static List<MetaMessage> GetAllMessages(DbCommand dbCommand, Database db)
       {
           MetaMessage metaMessage;
           List<MetaMessage> lstMetaMessages;
           try
           {
               lstMetaMessages = new List<MetaMessage>();
               using (IDataReader reader = db.ExecuteReader(dbCommand))
               {
                   while (reader.Read())
                   {
                       metaMessage = new MetaMessage();

                       metaMessage.MessageNo = Convert.ToInt32(reader["intMessageNo"]);
                       metaMessage.DisplayText = Convert.ToString(reader["chvDisplayText"]);
                       metaMessage.MessageDescription = Convert.ToString(reader["chvDescription"]);
                       //AA:2015.08.12 YRS 5.0-2441 Added Display code to show code in loan utility screen
                       metaMessage.DisplayCode = Convert.ToString(reader["chvDisplayCode"]);
                       string stMessageType = Convert.ToString(reader["chrType"]).ToUpper();

                       if (stMessageType == "S")
                       {
                           metaMessage.MessageType = EnumMessageTypes.Success;
                       }
                       else if (stMessageType == "E")
                       {
                           metaMessage.MessageType = EnumMessageTypes.Error;
                       }
                       else if (stMessageType == "W")
                       {
                           metaMessage.MessageType = EnumMessageTypes.Warning;
                       }
                       else if (stMessageType == "I")
                       {
                           metaMessage.MessageType = EnumMessageTypes.Information;
                       }

                       metaMessage.ModuleName = Convert.ToString(reader["chvModule"]);
                       metaMessage.CreatedBy = Convert.ToString(reader["chvCreator"]);
                       metaMessage.CreatedOn = Convert.ToDateTime(reader["dtmCreated"]);

                       lstMetaMessages.Add(metaMessage);
                   }

               }

           }
           catch
           {
               throw;
           }
           return lstMetaMessages;
       }

        //Note: the below commented code wil be used in future purpose, please don't remove
       //public static List<MetaMessage> GetMetaMessagesModuleWiseByMessageNo(int messageNo)
       //{
       //    DbCommand dbCommand = null;
       //    Database db;
       //    List<MetaMessage> lstMetaMessages;
       //    try
       //    {
       //        db = DatabaseFactory.CreateDatabase("YRS");
       //        lstMetaMessages = new List<MetaMessage>();

       //        if (db == null) return null;

       //        dbCommand = db.GetStoredProcCommand("yrs_usp_Msg_GetMetaMessagesModuleWiseByMessageNo");

       //        if (dbCommand == null) return null;
               
       //        db.AddInParameter(dbCommand, "@int_MessageNo", DbType.Int32, messageNo);

       //        lstMetaMessages = GetAllMessages(dbCommand, db);

       //        return lstMetaMessages;
       //    }
       //    catch
       //    {
       //        throw;
       //    }
       //}

       

       public static int UpdateMessage(MetaMessage metaMessage)
       {
           DbCommand dbCommand = null;
           Database db;
           int result;
           string strMessageType;
           try
           {
               db = DatabaseFactory.CreateDatabase("YRS");
               strMessageType = string.Empty;
               if (db == null) throw new Exception("Database object is not initialized.");
               if (metaMessage == null) throw new Exception("MetaMessage object cannot be null for update.");
             
               switch (metaMessage.MessageType)
               {
                   case EnumMessageTypes.Error:
                       {
                           strMessageType = "E"; 
                           break;
                       }
                   case EnumMessageTypes.Information:
                       {
                           strMessageType = "I";
                           break;
                       }
                   case EnumMessageTypes.Success:
                       {
                           strMessageType = "S"; 
                           break;
                       }
                   case EnumMessageTypes.Warning:
                       {
                           strMessageType = "W"; 
                           break;
                       }
                   default:
                       {
                           throw new Exception("No Message type defined in while updating Message for Message No " + metaMessage.MessageNo.ToString());                           
                       }
               }

               dbCommand = db.GetStoredProcCommand("yrs_usp_Msg_UpdateMetaMessages");

               db.AddInParameter(dbCommand, "@int_MessageNumber", DbType.Int32, metaMessage.MessageNo);
               db.AddInParameter(dbCommand, "@varchar_DisplayText", DbType.String, metaMessage.DisplayText);
               db.AddInParameter(dbCommand, "@varchar_MessageDescription", DbType.String, metaMessage.MessageDescription);              

               if (dbCommand == null) throw new Exception("Database command object is not initialized.");

               result = db.ExecuteNonQuery(dbCommand);
           }
           catch
           {
               throw;
           }
           return result;

       }
    }
}
