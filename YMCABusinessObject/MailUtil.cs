/************************************************************************************************************************/
// Author: Sanjay GS Rawat
// Created on: 10/10/2018
// Summary of Functionality: Send Mail utility
// Declared in Version: 20.6.0 | YRS-AT-3101 -YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name                   | Date          | Version No        | Ticket
// ------------------------------------------------------------------------------------------------------
// Sanjay GS Rawat                  | 	2018.10.10  | 20.6.0            | YRS-AT-3101 -YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
// Vinayan C                        | 	2018.11.09  | 20.6.0            | YRS-AT-3101 -YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
// Megha Lad                        | 	2019.01.19  | 20.6.2            | YRS-AT-3157 -YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
// Pooja Kumkar                     |   2019.10.16  | 20.7.0            | YRS-AT-2670 - YRS enh:Hacienda withholding message - Puerto Rico based YMCA orgs
// ------------------------------------------------------------------------------------------------------
/************************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
//using System.Web.Mail;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using YMCAObjects;
using YMCARET.YmcaDataAccessObject; // SR | 2018.05.25 | YRS-AT-3101 | Added reference to get access to DA layer.

namespace YMCARET.YmcaBusinessObject
{

    public class MailUtil
    {

        private string strFromMail;

        private string strToMail;

        private string strMailMessage = String.Empty;

        private string strSubject = String.Empty;

        private string strCc;

        private List<string> colAttachments = new List<string>();

        private string strMailCategory;

        //private MailFormat mailFormat;
        private bool isHTML;

        // Added by Paramesh K.
        public MailUtil()
        {
        }

        public string FromMail
        {
            get
            {
                return strFromMail;
            }
            set
            {
                strFromMail = value;
            }
        }

        public string ToMail
        {
            get
            {
                return strToMail;
            }
            set
            {
                strToMail = value;
            }
        }

        public string Subject
        {
            get
            {
                return strSubject;
            }
            set
            {
                strSubject = value;
            }
        }

        public string MailMessage
        {
            get
            {
                return strMailMessage;
            }
            set
            {
                strMailMessage = value;
            }
        }

        public bool IsHTML
        {
            get
            {
                return isHTML;
            }
            set
            {
                isHTML = value;
            }
        }

        public string SendCc
        {
            get
            {
                return strCc;
            }
            set
            {
                strCc = value;
            }
        }

        public List<string> MailAttachments
        {
            get
            {
                return colAttachments;
            }
        }

        public string MailCategory
        {
            get
            {
                return strMailCategory;
            }
            set
            {
                strMailCategory = value;
                Logger.Write("Mailutil - Mailcategory --> Retreiving From,to and cc process started ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                YMCARET.YmcaBusinessObject.MailBOClass.MailAddress addresses;
                addresses = YMCARET.YmcaBusinessObject.MailBOClass.GetEmailAddressByCategory(strMailCategory);
                if (!(addresses == null))
                {
                    FromMail = addresses.From;
                    ToMail = addresses.To;
                    SendCc = addresses.Cc;
                }

                Logger.Write("Mailutil - Mailcategory --> Retreiving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                // GetEmailConfigurationDetails(Value)
            }
        }

        private static void SendMail(string strCategory, string strFrom, string strTo, string strCCMail, string strMessage, string strSubject, bool isHtml, List<string> mailAttachments)
        {
            SmtpClient smtpClient;
            YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus mailStatus;
            try
            {
                // Start: Added By Prashant 2013.12.30 : BT-2354
                long longEmailDetailID;
                longEmailDetailID = YMCARET.YmcaBusinessObject.MailBOClass.InsertEmailDetails(0, strFrom, strTo, strCCMail, null, strMessage, strSubject);
                // End: Added By Prashant 2013.12.30 : BT-2354
                mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus();
                Logger.Write("Mailutil - SendMail --> Mail process started ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                if ((mailStatus.ServiceStatus != YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED))
                {
                    return;
                }

                MailMessage msg; // = new MailMessage();
                if (((strCategory == null)
                            || (strCategory == String.Empty)))
                {
                    Logger.Write("Mailutil - SendMail --> Retreiving From,to and cc process started ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    YMCARET.YmcaBusinessObject.MailBOClass.MailAddress addresses;
                    addresses = YMCARET.YmcaBusinessObject.MailBOClass.GetEmailAddressByCategory(strCategory);
                    if (!(addresses == null))
                    {
                        msg = new MailMessage(addresses.From, addresses.To);

                        msg.CC.Add(addresses.Cc);
                    }

                    Logger.Write("Mailutil - SendMail --> Retreiving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                }
                msg = new MailMessage();
                if (!(strFrom == null))
                {
                    //msg.From = strFrom.Trim();
                    msg.From = new MailAddress(strFrom);
                }

                if (!(strTo == null))
                {
                    //msg.To = strTo.Trim();
                    msg.To.Add(strTo);
                }

                if (!(strCCMail == null))
                {
                    //msg.Cc = strCCMail.Trim();
                    msg.CC.Add(strCCMail);
                }

                if (!(strMessage == null))
                {
                    msg.Body = strMessage.Trim();
                }

                if (!(strSubject == null))
                {
                    msg.Subject = strSubject.Trim();
                }

                //msg.BodyFormat = mailFormatType;
                msg.IsBodyHtml = isHtml;
                if ((!(msg.Attachments == null)
                            && !(mailAttachments == null)))
                {
                    //IEnumerator ienum = mailAttachments.GetEnumerator();
                    //while (ienum.MoveNext())
                    //{
                    //    //msg.Attachments.Add(new MailAttachment(Convert.ToString(ienum.Current)));
                    //}
                    foreach (string attachment in mailAttachments)
                    {
                        msg.Attachments.Add(new Attachment(attachment));
                    }
                }

                //string strSmtpServer = ConfigurationSettings.AppSettings["smtpServer"];
                string strSmtpServer = ConfigurationManager.AppSettings["smtpServer"];
                if ((!(strSmtpServer == null)
                            && (strSmtpServer != String.Empty)))
                {
                    //SmtpMail.SmtpServer = strSmtpServer;
                    //Todo handle error
                    smtpClient = new SmtpClient(strSmtpServer);

                    Logger.Write("Mailutil - SendMail --> checking default address", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    if ((mailStatus.DefaultStatus == YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES))
                    {
                        DataSet dsAdminDefaultEmailId;
                        string l_string_Admin_DefaultEmailId = String.Empty;
                        dsAdminDefaultEmailId = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_DEFAULT_TO_EMAILID");
                        if (YMCAObjects.CommonClass.isNonEmpty(dsAdminDefaultEmailId))
                        {
                            l_string_Admin_DefaultEmailId = Convert.ToString(dsAdminDefaultEmailId.Tables[0].Rows[0]["Value"]);
                        }

                        //msg.To = l_string_Admin_DefaultEmailId;
                        msg.To.Add(l_string_Admin_DefaultEmailId);

                        //msg.Cc = "";
                        msg.CC.Add("");
                    }

                    Logger.Write("Mailutil - SendMail --> mail sending started", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    //SmtpMail.Send(msg);
                    smtpClient.Send(msg);
                    // Start: Added By Prashant 2013.12.30 : BT-2354
                    YMCARET.YmcaBusinessObject.MailBOClass.UpdateMailSentStatus(longEmailDetailID);
                    // End: Added By Prashant 2013.12.30 : BT-2354
                    Logger.Write("Mailutil - SendMail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                }
                else
                {
                    CommonClass.LogException("MailUtil --> SendMailMessage", new Exception("smtpServer address is not defined"));
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void Send()
        {
            try
            {
                MailUtil.SendMail("", FromMail, ToMail, SendCc, MailMessage, Subject, IsHTML, MailAttachments);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static void Send(string mailCategory, string strFrom, string strTo, string strCCMail, string strMessage, string strSubject, bool isHtml, List<string> colAttachments)
        {
            try
            {
                MailUtil m = new MailUtil();
                m.MailCategory = mailCategory;
                m.ToMail = strTo;
                m.FromMail = strFrom;
                m.SendCc = strCCMail;
                m.MailMessage = strMessage;
                m.Subject = strSubject;
                //m.MailFormatType = mailFormatType;
                m.IsHTML = isHtml;
                m.colAttachments = colAttachments;
                m.Send();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool UseDefault()
        {
            YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus mailStatus;
            mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus();
            if ((mailStatus.DefaultStatus == YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool MailService()
        {
            YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus mailStatus;
            mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus();
            if ((mailStatus.ServiceStatus == YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void SendMail(EnumEmailTemplateTypes enumTemplateName, string strFromMail, string strToMail, string strCcMail, string strBccMail, Dictionary<string, string> dicParameters, string strMailAttachments, DataSet dsMultipleParameter, bool IsHtml)
        {
            EmailTemplate emailtemplate;
            MailMessage message;
            SmtpClient smtpClient;
            DataSet defaultAdminEmailData;

            string[] fileAttachments;
            string smtpServer;
            string defaultAdminEmailID;
            long longEmailDetailID;
            try
            {
                emailtemplate = YMCARET.YmcaBusinessObject.MailBOClass.Compose(enumTemplateName.ToString(), strFromMail, strToMail, strCcMail, strBccMail, dicParameters, strMailAttachments, dsMultipleParameter);
                // 'Logging EmailDetail'
                longEmailDetailID = YMCARET.YmcaBusinessObject.MailBOClass.InsertEmailDetails(emailtemplate);
                YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus mailStatus;
                // Dim longEmailDetailID As Long
                mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus();
                if ((mailStatus.ServiceStatus != YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED))
                {
                    Logger.Write("Mailutil - SendMail --> Mail Service Status is disabled.", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    return;
                }

                message = new MailMessage();
                if (!(emailtemplate == null))
                {
                    message.From = new MailAddress(emailtemplate.FromMail);
                    message.To.Add(emailtemplate.ToEmail);
                    message.CC.Add(emailtemplate.CcEmail);
                    message.Subject = emailtemplate.Subject;
                    message.Body = (emailtemplate.Body + Convert.ToString(emailtemplate.Footer));
                    message.IsBodyHtml = IsHtml;
                }

                Logger.Write("Mailutil - SendMail --> Retrieving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                if ((!(emailtemplate.ListEmailAttachment == null)
                            && (emailtemplate.ListEmailAttachment.Count > 0)))
                {
                    if (emailtemplate.DynamicFileAttachment)
                    {
                        fileAttachments = strMailAttachments.Split(';');
                        AddAttachment(fileAttachments, ref message);
                    }

                    if (!string.IsNullOrEmpty(emailtemplate.StaticFileAttachment))
                    {
                        fileAttachments = emailtemplate.StaticFileAttachment.Split(';');
                        AddAttachment(fileAttachments, ref message);
                    }

                }

                smtpServer = ConfigurationManager.AppSettings["smtpServer"];
                if (!string.IsNullOrEmpty(smtpServer))
                {
                    // SmtpMail.SmtpServer = strSmtpServer;
                    smtpClient = new SmtpClient(smtpServer);

                    Logger.Write("Mailutil - SendMail --> checking default address", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    if (mailStatus.DefaultStatus == YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES)
                    {
                        defaultAdminEmailID = String.Empty;
                        defaultAdminEmailData = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_DEFAULT_TO_EMAILID");
                        if (CommonClass.isNonEmpty(defaultAdminEmailData))
                        {
                            defaultAdminEmailID = Convert.ToString(defaultAdminEmailData.Tables[0].Rows[0]["Value"]);
                        }
                        message.To.Add(defaultAdminEmailID);
                        message.CC.Add("");
                        message.Bcc.Add("");
                    }
                    Logger.Write("Mailutil - SendMail --> mail sending started", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    smtpClient.Send(message);

                    // Update the mailsent status
                    YMCARET.YmcaBusinessObject.MailBOClass.UpdateMailSentStatus(longEmailDetailID);
                    Logger.Write("Mailutil - SendMail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                }
                else
                {
                    CommonClass.LogException("MailUtil --> SendMailMessage", new Exception("smtpServer address is not defined"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                defaultAdminEmailID = null;
                smtpServer = null;
                fileAttachments = null;
                defaultAdminEmailData = null;
                smtpClient = null;
                message = null;
                emailtemplate = null;
            }
        }

        // This method can be called only when mail util object is intilized for the usae of retrieving mailutil properties
        public void SendMailMessage(EnumEmailTemplateTypes enumTemplateName, string strFromMail, string strToMail, string strCcMail, string strBccMail, Dictionary<string, string> dicParameters, string strMailAttachments, DataSet dsMultipleParameter, bool IsHtml)
        {
            try
            {
                string[] strFiles;
                EmailTemplate emailtemplate;
                SmtpClient smtpClient;
                emailtemplate = YMCARET.YmcaBusinessObject.MailBOClass.Compose(enumTemplateName.ToString(), strFromMail, strToMail, strCcMail, strBccMail, dicParameters, strMailAttachments, dsMultipleParameter);
                // 'Logging EmailDetail'
                long longEmailDetailID;
                longEmailDetailID = YMCARET.YmcaBusinessObject.MailBOClass.InsertEmailDetails(emailtemplate);
                YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus mailStatus;
                // Dim longEmailDetailID As Long
                mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus();
                if ((mailStatus.ServiceStatus != YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED))
                {
                    Logger.Write("Mailutil - SendMail --> Mail Service Status is disabled.", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    return;
                }

                MailMessage msg = new MailMessage();
                if (!(emailtemplate == null))
                {
                    //msg.From = emailtemplate.FromMail;
                    //msg.To = emailtemplate.ToEmail;
                    //msg.Cc = emailtemplate.CcEmail;
                    //msg.Subject = emailtemplate.Subject;
                    //msg.Body = (emailtemplate.Body + Convert.ToString(emailtemplate.Footer));
                    //msg.BodyFormat = mailFormatType;
                    msg = new MailMessage(emailtemplate.FromMail, emailtemplate.ToEmail, emailtemplate.Subject, emailtemplate.Body + Convert.ToString(emailtemplate.Footer));
                    msg.IsBodyHtml = IsHtml;
                    if (!string.IsNullOrEmpty(emailtemplate.CcEmail))
                    {
                        msg.CC.Add(emailtemplate.CcEmail);
                    }
                }

                Logger.Write("Mailutil - SendMail --> Retrieving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                if ((!(emailtemplate.ListEmailAttachment == null)
                            && (emailtemplate.ListEmailAttachment.Count > 0)))
                {
                    if (emailtemplate.DynamicFileAttachment)
                    {
                        strFiles = strMailAttachments.Split(';');
                        AddAttachment(strFiles, ref msg);
                    }

                    if (!string.IsNullOrEmpty(emailtemplate.StaticFileAttachment))
                    {
                        strFiles = emailtemplate.StaticFileAttachment.Split(';');
                        AddAttachment(strFiles, ref msg);
                    }

                }

                //string strSmtpServer = ConfigurationSettings.AppSettings["smtpServer"];
                string strSmtpServer = ConfigurationManager.AppSettings["smtpServer"];
                if ((!(strSmtpServer == null)
                            && (strSmtpServer != String.Empty)))
                {
                    //SmtpMail.SmtpServer = strSmtpServer;
                    smtpClient = new SmtpClient(strSmtpServer);
                    Logger.Write("Mailutil - SendMail --> checking default address", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    if ((mailStatus.DefaultStatus == YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES))
                    {
                        DataSet dsAdminDefaultEmailId;
                        string l_string_Admin_DefaultEmailId = String.Empty;
                        dsAdminDefaultEmailId = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_DEFAULT_TO_EMAILID");
                        if (CommonClass.isNonEmpty(dsAdminDefaultEmailId))
                        {
                            l_string_Admin_DefaultEmailId = Convert.ToString(dsAdminDefaultEmailId.Tables[0].Rows[0]["Value"]);
                        }

                        //msg.To = l_string_Admin_DefaultEmailId;
                        //msg.Cc = "";
                        //msg.Bcc = "";
                        msg.To.Clear(); //|PK|10/16/2019 |YRS-AT-2670|while working on ticket YRS-AT-2670, it seems that when USE_DEFAULT_ADDRESSES =true then the mail is sent to the original email as well as a default email address. But it should only send to a default email id.
                        msg.To.Add(l_string_Admin_DefaultEmailId);
                        msg.CC.Clear();
                        msg.Bcc.Clear();
                    }

                    Logger.Write("Mailutil - SendMail --> mail sending started", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                    try
                    {
                        //SmtpMail.Send(msg);
                        smtpClient.Send(msg);
                        MailMessage = msg.Body;
                        // Update the mailsent status
                        YMCARET.YmcaBusinessObject.MailBOClass.UpdateMailSentStatus(longEmailDetailID);
                    }
                    catch (Exception ex)
                    {
                        CommonClass.LogException("MailUtil --> SendMailMessage", ex);
                        throw ex;
                    }

                    Logger.Write("Mailutil - SendMail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                }
                else
                {
                    throw new Exception("smtpServer address is not defined");
                }
            }
            catch (Exception ex)
            {
                CommonClass.LogException("MailUtil --> SendMailMessage", ex);
                throw ex;
            }
        }

        // End:AA:03.21.2016 YRS-AT-2594 Added below code to send an email and mailutil objects properties can also be accessed like body of email
        private static void AddAttachment(string[] strFiles, ref MailMessage msg)
        {
            if ((strFiles.Length > 0))
            {
                for (int iindex = 0; (iindex
                            <= (strFiles.Length - 1)); iindex++)
                {
                    //msg.Attachments.Add(new System.Web.Mail.MailAttachment(strFiles[iindex]));
                    msg.Attachments.Add(new Attachment(strFiles[iindex]));
                }
            }
        }

        public ReturnObject<bool> SendLoanEmail(string loanStatus, string loanNumber, string paymentMethod, string attachment)
        {
            LoanOperation participant, ymca;
            Dictionary<string, string> templateParameters;
            string persID, ymcaID, errorNo;

            ReturnObject<bool> result, participantEmailResult, ymcaEmailResult;
            try
            {
                Logger.Write("Mailutil - SendLoanEmail --> mail sending start", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                result = new ReturnObject<bool>();
                result.Value = true;
                result.MessageList = new List<string>();
                errorNo = string.Empty;

                participant = new LoanOperation();
                ymca = new LoanOperation();
                GetConfiguration(loanStatus, paymentMethod, participant, ymca);

                if (participant.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE || ymca.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                {
                    participantEmailResult = new ReturnObject<bool>(); 
                    if (participant.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                    {
                        templateParameters = MailDAClass.GetLoanEmailParameters(loanNumber, participant.EmailTemplate.ToString());
                        persID = templateParameters["PersID"];
                        participant.EmailID = templateParameters["PersEmailID"];
                        participantEmailResult = SendLoanEmail(persID, EntityTypes.PERSON, participant, null, templateParameters);
                    }

                    ymcaEmailResult = new ReturnObject<bool>();
                    if (ymca.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                    {
                        templateParameters = MailDAClass.GetLoanEmailParameters(loanNumber, ymca.EmailTemplate.ToString());
                        ymcaID = templateParameters["YmcaID"];
                        ymca.EmailID = templateParameters["LPAEmailID"];
                        ymcaEmailResult = SendLoanEmail(ymcaID, EntityTypes.YMCA, ymca, null, templateParameters);
                    }

                    if (participant.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE && ymca.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                    {
                        if (participantEmailResult.Value && ymcaEmailResult.Value)
                        {
                            result.Value = true;
                            //Add common success message
                            result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_SENT.ToString());
                        }
                        else if (!participantEmailResult.Value && !ymcaEmailResult.Value)
                        {
                            result.Value = false;
                            //Add common error message
                            result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_ERROR.ToString());
                        }
                        else if (!participantEmailResult.Value)
                        {
                            result = participantEmailResult;
                        }
                        else if (!ymcaEmailResult.Value)
                        {
                            result = ymcaEmailResult;
                        }
                    }
                    else if (participant.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                    {
                        result = participantEmailResult;
 
                    }
                    else if (ymca.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                    {
                        result = ymcaEmailResult;
                    }

                }

                Logger.Write("Mailutil - SendLoanEmail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                persID = null;
                ymcaID = null;
                templateParameters = null;
                participant = null;
                ymca = null;
            }
        }

        private ReturnObject<bool> SendLoanEmail(string entityID, EntityTypes entityType, LoanOperation entityDetails, string attachment, Dictionary<string, string> templateParameters)
        {
            ReturnObject<bool> result;
            ReturnObject<bool> resultOfIDM;
            //START: VC | 2018.11.09 | YRS-AT-3101 | Declared variables
            StringBuilder emailDetailsTable;
            EmailTemplate emailtemplate;
            //END: VC | 2018.11.09 | YRS-AT-3101 | Declared variables
            try
            {
                result = new ReturnObject<bool>();
                if (entityDetails.EmailTemplate != EnumEmailTemplateTypes.NO_TEMPLATE)
                {
                    if (string.IsNullOrEmpty(entityDetails.EmailID))
                    {
                        result.MessageList = new List<string>();
                        if (entityType == EntityTypes.PERSON)
                            result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_MISSING_PARTICIPANT.ToString());
                        else if (entityType == EntityTypes.YMCA)
                            result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_MISSING_YMCA.ToString());

                        result.Value = false;
                    }
                    else
                    {
                        try
                        {
                            SendMailMessage(entityDetails.EmailTemplate, "", entityDetails.EmailID, "", "", templateParameters, attachment, null, true);
                            entityDetails.MailMessage = this.MailMessage;
                            result.Value = true;
                            if (entityType == EntityTypes.PERSON)
                                result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_SENT_PARTICIPANT.ToString());
                            else if (entityType == EntityTypes.YMCA)
                                result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_SENT_YMCA.ToString());

                            if (entityDetails.IsIDMCopyRequired && !string.IsNullOrEmpty(entityDetails.MailMessage))
                            {
                                //START: VC | 2018.11.09 | YRS-AT-3101 | Added code to append To,From,CC,BCC Address and mail subject with email html
                                emailtemplate = YMCARET.YmcaBusinessObject.MailBOClass.Compose(entityDetails.EmailTemplate.ToString(), "", entityDetails.EmailID, "", "", templateParameters, attachment, null);
                                emailDetailsTable = new StringBuilder();
                                emailDetailsTable.Append(string.Format("<body><table><tr><td>From:</td><td>{0}</td></tr><tr><td>To:</td><td>{1}</td></tr>", emailtemplate.FromMail, emailtemplate.ToEmail));
                                if (!string.IsNullOrEmpty(emailtemplate.CcEmail))
                                    emailDetailsTable.Append(string.Format("<tr><td>Cc:</td><td>{0}</td></tr>", emailtemplate.CcEmail));
                                if (!string.IsNullOrEmpty(emailtemplate.BccEmail))
                                    emailDetailsTable.Append(string.Format("<tr><td>Bcc:</td><td>{0}</td></tr>", emailtemplate.BccEmail));
                                emailDetailsTable.Append(string.Format("<tr><td>Subject:</td><td>{0}</td></tr>", emailtemplate.Subject));
                                emailDetailsTable.Append("</table>");
                                entityDetails.MailMessage = System.Text.RegularExpressions.Regex.Replace(entityDetails.MailMessage, "<body>", emailDetailsTable.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                //END: VC | 2018.11.09 | YRS-AT-3101 | Added code to append To,From,CC,BCC Address and mail subject with email html
                                resultOfIDM = CommonBOClass.SendEmailCopyToIDM(entityID, entityDetails.MailMessage, entityDetails.DocTypeCode);
                                if (!resultOfIDM.Value)
                                {
                                    result.Value = false;
                                    if (result.MessageList != null && result.MessageList.Count > 0)
                                    {
                                        result.MessageList.AddRange(resultOfIDM.MessageList);
                                    }
                                    else
                                    {
                                        result.MessageList = resultOfIDM.MessageList;
                                    }
                                }
                            }
                            else if (entityDetails.IsIDMCopyRequired && string.IsNullOrEmpty(entityDetails.MailMessage))
                            {
                                result.Value = false;
                                if (entityType == EntityTypes.PERSON)
                                    result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_ERROR_PARTICIPANT.ToString());
                                else if (entityType == EntityTypes.YMCA)
                                    result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_ERROR_YMCA.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonClass.LogException("SendLoanEmail", ex);
                            ex = null;

                            result.Value = false;
                            result.MessageList = new List<string>();
                            if (entityType == EntityTypes.PERSON)
                                result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_ERROR_PARTICIPANT.ToString());
                            else if (entityType == EntityTypes.YMCA)
                                result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_EMAIL_ERROR_YMCA.ToString());
                        }
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                resultOfIDM = null;
                result = null;
            }
        }

        private static void GetConfiguration(string loanStatus, string paymentMethod, LoanOperation participant, LoanOperation ymca)
        {
            participant.EmailTemplate = EnumEmailTemplateTypes.NO_TEMPLATE;
            ymca.EmailTemplate = EnumEmailTemplateTypes.NO_TEMPLATE;

            if (loanStatus == YMCAObjects.LoanStatus.ACCEPTED)
            {

            }
            else if (loanStatus == YMCAObjects.LoanStatus.APPROVED)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Approve_Email;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.CANCEL)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Cancel_ParticipantEmail;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.DECLINED)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Decline_Email;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.DISB)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.LoanProcessing_ParticipantEmail;
                ymca.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_PAID_EFT_YMCA;
                ymca.IsIDMCopyRequired = true;
                ymca.DocTypeCode = YMCAObjects.IDMDocumentCodes.LoanProcessing_LPAEmail;                  
            }
            else if (loanStatus == YMCAObjects.LoanStatus.EXP)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Expired_ParticipantEmail;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.PAID)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Paid_ParticipantEmail;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.PEND)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_STATUS_UPDATE_PERSONS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Paid_ParticipantEmail;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.REJECTED)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_EFT_PAYMENT_FAILURE;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_EFT_Rejected_ParticipantEmail;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.WITHDRAWN)
            {

            }
            //START: ML | 2019.01.19 | YRS-AT-3157 | Setting parameters for freeze and unfreeze
            else if (loanStatus == YMCAObjects.LoanStatus.FREEZE)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_FREEZE_PROCESS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Freeze;
            }
            else if (loanStatus == YMCAObjects.LoanStatus.UNFREEZE)
            {
                participant.EmailTemplate = YMCAObjects.EnumEmailTemplateTypes.LOAN_UNFREEZE_PROCESS;
                participant.IsIDMCopyRequired = true;
                participant.DocTypeCode = YMCAObjects.IDMDocumentCodes.Loan_Unfreeze;
            }
            //END: ML | 2019.01.19 | YRS-AT-3157 | Setting parameters for freeze and unfreeze
        }
    }
}