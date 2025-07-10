//*******************************************************************************
//Modification History
//*******************************************************************************
//Author			             Date                 Description
//*******************************************************************************
//Nikunj Patel	                 2009.03.20           Moving GetEmailConfigurationDetails from MetaConfigMaintenanceDAClass to MailDAClass
//
//Neeraj Singh                   06/jun/2010          Enhancement for .net 4.0
//Neeraj Singh                   07/jun/2010          review changes done
//Prashant                       12/30/2013           BT-2354: Logging emails generated from YRS application
//Shashank Patel                 02/23/2015           BT-2359: Sending emails based on pre-defined email templates in YRS
//Manthan Rajguru                2015.09.16           YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru                2018.10.11           YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//*******************************************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using YMCAObjects;
using System.Transactions;
using System.Collections.Generic; // SR | 2018.10.10 | adding reference for send Email & exception logging.
namespace YMCARET.YmcaDataAccessObject
{

	public class MailDAClass
	{
		public MailDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static string GetMailLastParagraph()
		{
			string strMailLastParagraphMsg;
			Database db= null;
			DbCommand GetCommandWrapper = null;
			 
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				GetCommandWrapper=db.GetStoredProcCommand("dbo.yrs_usp_Mail_GetFooter");
				GetCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (GetCommandWrapper == null) return null;
				
				strMailLastParagraphMsg = db.ExecuteScalar(GetCommandWrapper).ToString();
				
				return strMailLastParagraphMsg;
			}
			catch
			{
				throw;
			}
		}

		//function returning dataset for the search against Category Code
		//The DataSet contains the details like Mail Service, Use Default, Email Ids 
		public static DataSet GetMailConfigurationDetails(string paramStrConfigCategoryCode,out Int32 No_Mail_Service,out Int32 Use_Default)
		{
			DataSet dsGetEmailConfiguration = null;
			Database db = null;
			string [] l_TableNames;
			No_Mail_Service=0;
			Use_Default=0;
			DbCommand GetEmailConfigCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				GetEmailConfigCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetEmailConfigurationDetails");
				
				if (GetEmailConfigCommandWrapper == null) return null;

				db.AddInParameter(GetEmailConfigCommandWrapper,"@chvConfigCategoryCode",DbType.String,paramStrConfigCategoryCode);
                db.AddOutParameter(GetEmailConfigCommandWrapper, "@NO_MAIL_SERVICE", DbType.Int32, No_Mail_Service);
                db.AddOutParameter(GetEmailConfigCommandWrapper, "@USE_DEFAULT", DbType.Int32, Use_Default);
							
				dsGetEmailConfiguration = new DataSet();
				l_TableNames = new string [] {"EmailDetails"};
				db.LoadDataSet(GetEmailConfigCommandWrapper, dsGetEmailConfiguration,l_TableNames);
				No_Mail_Service=Convert.ToInt32(db.GetParameterValue(GetEmailConfigCommandWrapper,"@NO_MAIL_SERVICE")); 
				Use_Default=Convert.ToInt32(db.GetParameterValue(GetEmailConfigCommandWrapper,"@USE_DEFAULT")); 
				return dsGetEmailConfiguration;
			}
			catch 
			{
				throw;
			}
		}

		//Start : Added By Prashant 12/30/2013 : BT-2354
		/// <summary>
		/// Insert Email Details 
		/// </summary>
		/// <param name="TemplateID">Template ID </param>
		/// <param name="FromMail">From Mail</param>
		/// <param name="ToMail">To Mail</param>
		/// <param name="CcMail">Cc Mail</param>
		/// <param name="bccMail">Bcc Email</param>
		/// <param name="MailMsg">Describe the message</param>
		/// <param name="Subject">Subject of Mail</param>
		public static long InsertEmailDetails(int templateID, string fromMail, string toMail, string ccMail, string bccMail, string mailMsg, string mailSubject)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			long l_long_Output;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return -1;

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_Email_InsertEmailDetails");
				if (insertCommandWrapper == null) return -1;

				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper, "@intTemplateID", DbType.Int16, null);
				db.AddInParameter(insertCommandWrapper, "@chvFromMail", DbType.String, fromMail);
				db.AddInParameter(insertCommandWrapper, "@chvToMail", DbType.String, toMail);
				db.AddInParameter(insertCommandWrapper, "@chvCcMail", DbType.String, ccMail);
				db.AddInParameter(insertCommandWrapper, "@chvBccMail", DbType.String, bccMail);
				db.AddInParameter(insertCommandWrapper, "@chvMailMesssage", DbType.String, mailMsg);
				db.AddInParameter(insertCommandWrapper, "@chvSubject", DbType.String, mailSubject);

				db.AddOutParameter(insertCommandWrapper, "@bintOutput", DbType.Int64, int.MaxValue);

				db.ExecuteNonQuery(insertCommandWrapper);
				l_long_Output = Convert.ToInt64(db.GetParameterValue(insertCommandWrapper, "@bintOutput"));

				return l_long_Output;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Update the EMail detail record(s)
		/// </summary>
		/// <param name="emailDetailId">Email detailed Id</param>
		/// <returns>Long</returns>
		public static long UpdateMailSentStatus(long emailDetailId)
		{
			Database db = null;
			DbCommand UpdateCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return -1;
				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_Email_UpdateMailSentStatus");
				if (UpdateCommandWrapper == null) return -1;
				db.AddInParameter(UpdateCommandWrapper, "@bintEmailDetailID", DbType.Int64, emailDetailId);
				db.ExecuteNonQuery(UpdateCommandWrapper);
				return -1;
			}
			catch
			{
				throw;
			}
		}
		// End : Added By Prashant 12/30/2013 : BT-2354

        //SP 2015.02.23 BT-2359 -Start
        public static EmailTemplate GetEmailTemplateByName(string parameterStringTemplateName)
        {
            EmailTemplate template = null;
            Database database = null;
            DbCommand storedProcCommand = null;
            EmailTemplate template2;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null)
                {
                    return null;
                }
                storedProcCommand = database.GetStoredProcCommand("yrs_usp_Email_GetEmailTemplateByName");
                if (storedProcCommand == null)
                {
                    return null;
                }
                database.AddInParameter(storedProcCommand, "@chvName", DbType.String, parameterStringTemplateName);
                template = new EmailTemplate();
                using (IDataReader reader = database.ExecuteReader(storedProcCommand))
                {
                    while (reader.Read())
                    {
                        template.TemplateID=Convert.ToInt32(reader["intTemplateID"]);
                        template.Name=Convert.ToString(reader["chvName"]);
                        template.FromMail=Convert.ToString(reader["chvFromEmailID"]);
                        template.ToEmail=Convert.ToString(reader["chvToEmailID"]);
                        template.CcEmail=Convert.ToString(reader["chvCcEmailID"]);
                        template.BccEmail=Convert.ToString(reader["chvBccEmailID"]);
                        template.Subject=Convert.ToString(reader["chvSubject"]);
                        template.Body=Convert.ToString(reader["chvBody"]);
                        template.Footer=Convert.ToString(reader["chvFooter"]);
                        template.IsActive=Convert.ToBoolean(reader["bitActive"]);
                        template.StaticFileAttachment=Convert.ToString(reader["chvStaticFileAttachmentPath"]);
                        template.DynamicFileAttachment=Convert.ToBoolean(reader["bitDynamicAttachment"]);
                    }
                }
                template2 = template;
            }
            catch
            {
                throw;
            }
            return template2;
        }

        private static long InsertEmailAttachment(EmailAttachment emailattachment)
        {
            Database database = null;
            DbCommand storedProcCommand = null;
            long num2;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null)
                {
                    return -1L;
                }
                storedProcCommand = database.GetStoredProcCommand("yrs_usp_Email_InsertEmailAttachement");
                if (storedProcCommand == null)
                {
                    return -1L;
                }
                database.AddInParameter(storedProcCommand, "@bintEmailDetailID", DbType.Int64, emailattachment.EmailDetailID);
                database.AddInParameter(storedProcCommand, "@vbinFileAttachment", DbType.Binary, emailattachment.Attachment);
                database.AddInParameter(storedProcCommand, "@chvAttachmentFileNameWithExtension", DbType.String, emailattachment.AttachmentFileNameWithExtension);
                database.AddInParameter(storedProcCommand, "@chvAttachmentFileNameWithFullPath", DbType.String, emailattachment.AttachmentFileNameWithFullPath);
                database.AddOutParameter(storedProcCommand, "@bintOutput", DbType.Int64, 0x7fffffff);
                database.ExecuteNonQuery(storedProcCommand);
                num2 = Convert.ToInt64(database.GetParameterValue(storedProcCommand, "@bintOutput"));
            }
            catch
            {
                throw;
            }
            return num2;
        }
        public static long InsertEmailDetails(EmailTemplate objemailtemplate)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                long num;
                try
                {
                    num = SaveEmailDetail(objemailtemplate);
                    if (objemailtemplate.ListEmailAttachment != null)
                    {
                        foreach (EmailAttachment attachment in objemailtemplate.ListEmailAttachment)
                        {
                            attachment.EmailDetailID = num;
                            InsertEmailAttachment(attachment);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                scope.Complete();
                return num;
            }
        }

        private static long SaveEmailDetail(EmailTemplate emailtemplate)
        {
            Database database = null;
            DbCommand storedProcCommand = null;
            long num2;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null)
                {
                    return -1L;
                }
                storedProcCommand = database.GetStoredProcCommand("yrs_usp_Email_InsertEmailDetails");
                if (storedProcCommand == null)
                {
                    return -1L;
                }
                database.AddInParameter(storedProcCommand, "@intTemplateID", DbType.Int16, emailtemplate.TemplateID);
                database.AddInParameter(storedProcCommand, "@chvFromMail", DbType.String, emailtemplate.FromMail);
                database.AddInParameter(storedProcCommand, "@chvToMail", DbType.String, emailtemplate.ToEmail);
                database.AddInParameter(storedProcCommand, "@chvCcMail", DbType.String, emailtemplate.CcEmail);
                database.AddInParameter(storedProcCommand, "@chvBccMail", DbType.String, emailtemplate.BccEmail);
                database.AddInParameter(storedProcCommand, "@chvMailMesssage", DbType.String, emailtemplate.Body + emailtemplate.Footer);
                database.AddInParameter(storedProcCommand, "@chvSubject", DbType.String, emailtemplate.Subject);
                database.AddOutParameter(storedProcCommand, "@bintOutput", DbType.Int64,int.MaxValue);
                database.ExecuteNonQuery(storedProcCommand);
                num2 = Convert.ToInt64(database.GetParameterValue(storedProcCommand, "@bintOutput"));
            }
            catch
            {
                throw;
            }
            return num2;
        }

        //SP 2015.02.23 BT-2359 -End

        // START : MMR | 2018.10.10 | YRS-AT-3101 - Create common function to get Loan related template parameter.
        public static Dictionary<string, string> GetLoanEmailParameters(string loanNumber, string emailTemplate)
        {
            Dictionary<string, string> parametersForMailTemplate;
            DataSet dsEmailParameters = null;
            Database db = null;
            DbCommand getCommandWrapper = null;
            string key, value;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_LOAN_LoanEmailParameters");

                getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(getCommandWrapper, "@INT_LoanNumber", DbType.Int32, loanNumber);
                db.AddInParameter(getCommandWrapper, "@VARCHAR_MailTemplate", DbType.String, emailTemplate);
                if (getCommandWrapper == null) return null;

                dsEmailParameters = new DataSet();
                db.LoadDataSet(getCommandWrapper, dsEmailParameters, "EmailParameters");

                parametersForMailTemplate = new Dictionary<string, string>();
                if (HelperFunctions.isNonEmpty(dsEmailParameters))
                {
                    foreach (DataRow row in dsEmailParameters.Tables[0].Rows)
                    {
                        key = Convert.ToString(Convert.IsDBNull(row["KEY"]) ? "" : row["KEY"]);
                        value = Convert.ToString(Convert.IsDBNull(row["VALUE"]) ? "" : row["VALUE"]);
                        parametersForMailTemplate.Add(key, value);
                    }
                }
                return parametersForMailTemplate;
            }
            catch
            {
                throw;
            }
            finally
            {
                parametersForMailTemplate = null;
                dsEmailParameters = null;
                db = null;
                getCommandWrapper = null;
            }
            // END : MMR | 2018.10.10 | YRS-AT-3101 - Create common function to get Loan related mail template parameter.
        }
	}
}
