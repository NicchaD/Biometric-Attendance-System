//*******************************************************************************
//Modification History
//*******************************************************************************
//	Date		Author			Description
//*******************************************************************************
//	2009.03.20	Nikunj Patel	Moving GetEmailConfigurationDetails from MetaConfigMaintenanceBOClass to MailBOClass
// 15-Oct-2009	Priya			made changes to send mail in void-reissue
// 10-Jan-2013  Dinesh Kanojia  BT:1135:YRS 5.0-1659:New Configuration value for hardship withdrawal email sender
//14-Apr-2013   Anudeep         Bt-1131:YRS 5.0-1654 : Email to finance dept when withdrawal to foreign address 
// 12-30-2013   Prashant        BT-2354: Logging emails generated from YRS application 
// 23-Feb-2015  Shashank Patel  BT-2359: Sending emails based on pre-defined email templates in YRS
// 2015.09.16   Manthan Rajguru YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
// 2015.12.17   Bala            YRS-AT-2642: Recommended Html styles change.
//*******************************************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.IO;
using System.Collections.Generic;
using YMCAObjects;
using System.Net.Mail;
using System.Text;

namespace YMCARET.YmcaBusinessObject
{
    public class MailBOClass
    {
        public struct MailServiceStatus
        {
            public MailStatuses ServiceStatus;
            public MailStatuses DefaultStatus;
        }
        public enum MailStatuses
        {
            UNKNOWN = 0,
            ENABLED = 1,
            DISABLED = 2,
            USE_DEFAULT_ADDRESSES = 3,
            USE_ACTUAL_ADDRESSES = 4
        }

        public class MailAddress
        {
            public string To;
            public string From;
            public string Cc;
            public string Bcc;
        }

        public MailBOClass()
        {
            //TODO: Add constructor logic here//
        }

        public static string GetMailLastParagraph()
        {
            try
            {
                string strMailLastParagraphMsg = YMCARET.YmcaDataAccessObject.MailDAClass.GetMailLastParagraph().Trim();
                if (strMailLastParagraphMsg == "" || strMailLastParagraphMsg == System.DBNull.Value.ToString())
                {
                    throw new Exception("Description is not specified in av_rpt_LastParagraph.");
                }
                else
                {
                    return strMailLastParagraphMsg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetMailConfigurationDetails(string paramStrConfigCategoryCode)
        {
            try
            {
                int No_Mail_Service, Use_Default;
                return (YMCARET.YmcaDataAccessObject.MailDAClass.GetMailConfigurationDetails(paramStrConfigCategoryCode, out No_Mail_Service, out Use_Default));
            }
            catch
            {
                throw;
            }
        }

        public static MailServiceStatus GetMailServiceStatus()
        {
            int No_Mail_Service, Use_Default;
            MailServiceStatus returnValue = new MailServiceStatus();
            returnValue.ServiceStatus = MailStatuses.UNKNOWN;
            returnValue.DefaultStatus = MailStatuses.UNKNOWN;
            try
            {
                YMCARET.YmcaDataAccessObject.MailDAClass.GetMailConfigurationDetails("THIS_DOES_NOT_EXIST_IN_SYSTEM", out No_Mail_Service, out Use_Default);
                returnValue.DefaultStatus = Use_Default == 1 ? MailStatuses.USE_DEFAULT_ADDRESSES : MailStatuses.USE_ACTUAL_ADDRESSES;
                returnValue.ServiceStatus = No_Mail_Service == 1 ? MailStatuses.ENABLED : MailStatuses.DISABLED;
            }
            catch
            {
                throw;
            }
            return returnValue;
        }
        public static MailAddress GetEmailAddressByCategory(string paramStrConfigCategoryCode)
        {
            //Created By Ashutosh Patil as on 21-May-2007
            //Common function GetEmailConfigurationDetails for common email.

            DataSet l_Mail_dataset;
            DataTable l_DataTable;

            //Added By Ashish 28-Jan-2009,Start
            string l_paramstrConfigCategoryCode;
            string fromEmail, toEmail, ccEmail;
            fromEmail = string.Empty; toEmail = string.Empty; ccEmail = string.Empty;
            //Added By Ashish 28-Jan-2009,End
            try
            {
                //Added By Ashish 28-Jan-2009,Start
                l_paramstrConfigCategoryCode = paramStrConfigCategoryCode;
                if (paramStrConfigCategoryCode.Trim().ToUpper() == "TDLOAN_DEFAULTED")
                {
                    l_paramstrConfigCategoryCode = "TDLoan";
                }
                //Anudeep:14.04.2013 : YRS 5.0-1654 : Email to finance dept when withdrawal to foreign address 
                if (paramStrConfigCategoryCode.Trim().ToUpper() == "REFUNDEMAIL")
                {
                    l_paramstrConfigCategoryCode = "REFUND";
                }
                l_Mail_dataset = GetMailConfigurationDetails(l_paramstrConfigCategoryCode);

                if (isEmpty(l_Mail_dataset) && isEmpty(l_Mail_dataset.Tables["EmailDetails"])) return null;

                l_DataTable = l_Mail_dataset.Tables[0];
                if (isEmpty(l_DataTable)) return null;

                switch (paramStrConfigCategoryCode)
                {
                    case "TDLoan":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "TDLOANS_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "TDLOANS_DEFAULT_TO_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    //Default code moved to the SendMail Process
                    case "TDLoan_Defaulted":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "TDLOANS_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "TDLOANS_DEFAULT_TO_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "TDLOANS_DEFAULTED_CC_EMAILID")
                            {
                                ccEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    case "DELINQ":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "DELINQ_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "DELINQ_DEFAULT_TO_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "DELINQ_CC_EMAILID")
                            {
                                ccEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    // 10-Jan-2013  Dinesh Kanojia  BT:1135:YRS 5.0-1659:New Configuration value for hardship withdrawal email sender
                    case "HARD":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "HARDSHIP_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "HARDSHIP_TO_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    case "ADMIN":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "ADMIN_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "ADMIN_TO_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    //Priya 11/03/08 Made changes into GetEmailConfigurationDetails() function to get mailids values values of reverse refund
                    case "REFUND":
                        DataSet ds;
                        ds = YMCACommonBOClass.getConfigurationValue("ADMIN_FROM_EMAILID");
                        if (isNonEmpty(ds))
                        {
                            fromEmail = ds.Tables[0].Rows[0]["Value"].ToString();
                        }
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "BACKDATED_REFUND_REVERSE_MAILIDS")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    //Priya 15-October-2009 made changes to send mail in void-reissue
                    case "VOID":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "VOID_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "VOID_TO_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    //Added By SG: 2012.09.05: BT-960
                    case "CASOUT":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "CASHOUT_FROM_EMAILID")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "CASHOUT_TO_FINDEPT_EMAILID")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    //Start:Anudeep:14.04.2013 : YRS 5.0-1654 : Email to finance dept when withdrawal to foreign address 
                    case "REFUNDEMAIL":
                        foreach (DataRow l_DataRow in l_DataTable.Rows)
                        {
                            if (((string)l_DataRow["chvKey"]).Trim() == "REFUND_NON_US_CA_FROM_EMAIL")
                            {
                                fromEmail = l_DataRow["chvValue"].ToString();
                            }
                            if (((string)l_DataRow["chvKey"]).Trim() == "REFUND_NON_US_CA_TO_EMAIL")
                            {
                                toEmail = l_DataRow["chvValue"].ToString();
                            }
                        }
                        break;
                    //End:Anudeep:14.04.2013 : YRS 5.0-1654 : Email to finance dept when withdrawal to foreign address 
                    default:
                        //throw new Exception("Unhandled parameter to obtain email addresses"); 
                        return null;
                    //break;
                }
                MailAddress m = new MailAddress();
                m.To = toEmail; m.From = fromEmail; m.Cc = ccEmail;
                return m;
            }
            catch
            {
                throw;
            }

        }
        #region "General Utility Functions"
        private static bool isNonEmpty(DataSet ds)
        {
            if (ds == null) return false;
            if (ds.Tables.Count == 0) return false;
            if (ds.Tables[0].Rows.Count == 0) return false;
            return true;
        }

        private static bool isNonEmpty(DataView dv)
        {
            if (dv == null) return false;
            if (dv.Count == 0) return false;
            return true;
        }
        private static bool isNonEmpty(DataTable dt)
        {
            if (dt == null) return false;
            if (dt.Rows.Count == 0) return false;
            return true;
        }
        private static bool isEmpty(DataSet ds)
        {
            return !isNonEmpty(ds);
        }
        private static bool isEmpty(DataView dv)
        {
            return !isNonEmpty(dv);
        }
        private static bool isEmpty(DataTable dt)
        {
            return !isNonEmpty(dt);
        }
        #endregion

        // Start : Added By Prashant 12-30-2013 : BT-2354
        /// <summary>
        /// Insert Email Details 
        /// </summary>
        /// <param name="templateID">Template ID </param>
        /// <param name="fromMail">From Mail</param>
        /// <param name="toMail">To Mail</param>
        /// <param name="ccMail">Cc Mail</param>
        /// <param name="bccMail">Bcc Email</param>
        /// <param name="mailMsg">Describe the message</param>
        /// <param name="mailSubject">Subject of Mail</param>
        /// <returns></returns>
        public static long InsertEmailDetails(int templateID, string fromMail, string toMail, string ccMail, string bccMail, string mailMsg, string mailSubject)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.MailDAClass.InsertEmailDetails(templateID, fromMail, toMail, ccMail, bccMail, mailMsg, mailSubject);
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
            try
            {
                return YMCARET.YmcaDataAccessObject.MailDAClass.UpdateMailSentStatus(emailDetailId);
            }
            catch
            {
                throw;
            }
        }
        // End : Added By Prashant 12-30-2013 : BT-2354

        //SP 2015.02.23 BT-2359 -start
        public static EmailTemplate Compose(string enumTemplateName, string strFromMail, string strToMail, string strCcMail, string strBccMail, Dictionary<string, string> dicParameters, string strMailAttachments, DataSet dsMultipleParameter)
        {
            string fileName = null;
            string[] strArray = null;
            byte[] buffer = null;
            string[] strArray2 = null;
            EmailTemplate emailTemplateByName = null;
            SmtpClient client = new SmtpClient();
            emailTemplateByName = GetEmailTemplateByName(enumTemplateName);
            List<EmailAttachment> list = null;
            if (emailTemplateByName == null)
            {
                throw new Exception("EmailTemplate Not found");
            }
            if (dsMultipleParameter != null)
            {
                Dictionary<string, string> messageDetails = GetMessageDetails(dsMultipleParameter);
                foreach (KeyValuePair<string, string> pair in messageDetails)
                {
                    string oldValue = "$$$" + pair.Key + "$$$";
                    emailTemplateByName.Body = emailTemplateByName.Body.Replace(oldValue, pair.Value);
                }
            }
            if (dicParameters != null)
            {
                foreach (KeyValuePair<string, string> pair in dicParameters)
                {
                    string introduced16 = "$$" + pair.Key + "$$";
                    emailTemplateByName.Body = emailTemplateByName.Body.Replace(introduced16, pair.Value);
                    string introduced17 = "$$" + pair.Key + "$$";
                    emailTemplateByName.Subject = emailTemplateByName.Subject.Replace(introduced17, pair.Value);
                }
            }
            if (emailTemplateByName.Subject.StartsWith("$$"))
            {
                throw new Exception("Parameter is missing in Subject");
            }
            if (emailTemplateByName.Body.StartsWith("$$"))
            {
                throw new Exception("Parameter is missing in Body");
            }
            if (emailTemplateByName.Body.StartsWith("$$$"))
            {
                throw new Exception("Multiple Parameter is missing in Body");
            }
            if (!string.IsNullOrEmpty(strFromMail))
            {
                emailTemplateByName.FromMail = strFromMail.Trim();
            }
            if (!string.IsNullOrEmpty(strToMail))
            {
                emailTemplateByName.ToEmail = strToMail.Trim();
            }
            if (!string.IsNullOrEmpty(strCcMail))
            {
                emailTemplateByName.CcEmail = strCcMail.Trim();
            }
            if (!string.IsNullOrEmpty(strBccMail))
            {
                emailTemplateByName.BccEmail = strBccMail.Trim();
            }
            if (emailTemplateByName.DynamicFileAttachment)
            {
                if (string.IsNullOrEmpty(strMailAttachments))
                {
                    throw new Exception("File path is missing.");
                }
                list = new List<EmailAttachment>();
                strArray = strMailAttachments.Split(new char[] { Convert.ToChar(";") });
                if (strArray.Length > 0)
                {
                    for (int i = 0; i <= (strArray.Length - 1); i++)
                    {
                        fileName = Path.GetFileName(strArray[i]);
                        buffer = File.ReadAllBytes(strArray[i]);
                        list.Add(new EmailAttachment(buffer, fileName));
                    }
                }
                emailTemplateByName.ListEmailAttachment = list;
            }
            if (!string.IsNullOrEmpty(emailTemplateByName.StaticFileAttachment))
            {
                if (list == null)
                {
                    list = new List<EmailAttachment>();
                }
                strArray2 = emailTemplateByName.StaticFileAttachment.Split(new char[] { Convert.ToChar(";") });
                if (strArray2.Length > 0)
                {
                    for (int j = 0; j <= (strArray2.Length - 1); j++)
                    {
                        fileName = Path.GetFileName(strArray2[j]);
                        list.Add(new EmailAttachment(fileName, strArray2[j]));
                    }
                }
                emailTemplateByName.ListEmailAttachment = list;
            }
            return emailTemplateByName;
        }
        public static EmailTemplate GetEmailTemplateByName(string parameterStringTemplateName)
        {
            EmailTemplate emailTemplateByName;
            try
            {
                emailTemplateByName = MailDAClass.GetEmailTemplateByName(parameterStringTemplateName);
            }
            catch
            {
                throw;
            }
            return emailTemplateByName;
        }


        private static Dictionary<string, string> GetMessageDetails(DataSet dsparamater)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            StringBuilder builder = new StringBuilder();
            foreach (DataTable table in dsparamater.Tables)
            {
                builder.Clear();
                if (table.TableName == null)
                {
                    throw new Exception("Datatable is not found in dataset");
                }
                if (table.Rows.Count < 1)
                {
                    throw new Exception("No row found in datatable");
                }
                //Start: Bala: 17-Dec-2015: Recommended Html styles change.
                //builder.Append("<table><tr>");
                builder.Append("<table style='border-color: #ffd7a7; border-width: thin; border-style: solid;width:70%;'><thead><tr style='font-weight: bold; FONT-SIZE: 12px; COLOR: #000066; FONT-FAMILY: tahoma; HEIGHT: 22px; BACKGROUND-COLOR: #c9dbed; TEXT-ALIGN: left;'>");
                //End: Bala: 17-Dec-2015: Recommended Html styles change.
                foreach (DataColumn column in table.Columns)
                {
                    builder.Append("<td>");
                    builder.Append(column.ColumnName);
                    builder.Append("</td>");
                }
                //Start: Bala: 17-Dec-2015: Recommended Html styles change.
                //builder.Append("</tr>");
                builder.Append("</tr></thead><tbody>");
                string oddRow = "style='FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #000033; FONT-FAMILY: tahoma; BACKGROUND-COLOR: #eeeeee; text-align: left; height: 16px; vertical-align: top;'",
                    evenRow = "style='FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #000033; FONT-FAMILY: tahoma; BACKGROUND-COLOR: #ffffff; text-align: left; height: 16px; vertical-align: top;'";
                //End: Bala: 17-Dec-2015: Recommended Html styles change.
                foreach (DataRow row in table.Rows)
                {
                    //Start: Bala: 17-Dec-2015: Recommended Html styles change.
                    //builder.Append("<tr>");
                    if ((table.Rows.IndexOf(row) + 1) % 2 == 0)
                        builder.Append(string.Format("<tr {0}>", evenRow));
                    else
                        builder.Append(string.Format("<tr {0}>", oddRow));
                    //End: Bala: 17-Dec-2015: Recommended Html styles change.
                    foreach (DataColumn column in table.Columns)
                    {
                        builder.Append("<td>");
                        builder.Append(row[column.ColumnName].ToString());
                        builder.Append("</td>");
                    }
                    builder.Append("</tr>");
                }
                //Start: Bala: 17-Dec-2015: Recommended Html styles change.
                //builder.Append("</table>");
                builder.Append("</tbody></table>");
                //End: Bala: 17-Dec-2015: Recommended Html styles change.
                dictionary.Add(table.TableName, builder.ToString());
            }
            return dictionary;
        }

        public static long InsertEmailDetails(EmailTemplate emailtemplate)
        {
            long num;
            try
            {
                num = MailDAClass.InsertEmailDetails(emailtemplate);
            }
            catch
            {
                throw;
            }
            return num;
        }
        //SP 2015.02.23 BT-2359 -End
    }
}
