//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	StatusDAClass.cs
// Author Name		:	Shashi Shekhar Singh
// Employee ID		:	51426
// Email			:	shashi.singh@3i-infotech.com
// Contact No		:	
// Creation Time	:	18th Aug 2010
// Program Specification Name	:		
// Unit Test Plan Name			:	
// Description					:	DataAcess class for Status page form
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
//Manthan Rajguru           2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System.Data;
using System.Collections;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
   public sealed class StatusDAClass
    {



       /// <summary>
        /// To get Email Status from AtsMetaConfiguration table
       /// </summary>
       /// <param name="parameterKeyValues">Key values</param>
       /// <returns>Dataset</returns>
       public static DataSet GetMailStatus(string parameterKeyValues)
       {
           DataSet dsGetEmailStatus = null;
           Database db = null;
           DbCommand commandGetEmailStatus = null;

           try
           {
               db = DatabaseFactory.CreateDatabase("YRS");

               if (db == null) return null;

               commandGetEmailStatus = db.GetStoredProcCommand("yrs_usp_GetStatusPageData");

               if (commandGetEmailStatus == null) return null;
               commandGetEmailStatus.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
               dsGetEmailStatus = new DataSet();
               db.AddInParameter(commandGetEmailStatus, "@varchar_keyValues", DbType.String, parameterKeyValues);

               db.LoadDataSet(commandGetEmailStatus, dsGetEmailStatus, "EmailStatus");
               return dsGetEmailStatus;
           }
           catch
           {
               throw;
           }

       }


       /// <summary>
       /// To get the Email Status dataset
       /// </summary>
       /// <param name="alKeyValues">Key values</param>
       /// <returns>Dataset</returns>
       
       public static DataSet GetEmailStatus(ArrayList alKeyValues)
       {
           DataSet l_DataSet = new DataSet();
           StringBuilder builder = new StringBuilder();

           try
           {
               if (alKeyValues != null)
               {
                   foreach (string val in alKeyValues)
                   {
                       if (builder.Length < 1900)
                       {
                           builder.Append(val + ",");
                       }
                       else
                       {
                           builder.Append(val + ",");
                           builder = builder.Remove(builder.Length - 1, 1);
                           l_DataSet = GetMailStatus(builder.ToString());
                           builder.Length = 0;
                       }
                   }

                   if (builder != null)
                   {
                       builder = builder.Remove(builder.Length - 1, 1);
                       l_DataSet = GetMailStatus(builder.ToString());
                       builder.Length = 0;
                   }
                   return l_DataSet;
               }
               return null;
           }
           catch
           {
               throw;
           }
           finally
           {
               builder = null;
               l_DataSet = null;
               l_DataSet = null;
           }
       } 







    }
}
