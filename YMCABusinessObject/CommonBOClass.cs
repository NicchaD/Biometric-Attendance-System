/************************************************************************************************************************/
// Author: Sanjay GS Rawat
// Created on: 10/15/2018
// Summary of Functionality: Common class to define generic / common functions required in the YRS application, It is coupled with CommonDAClass.cs.
// Declared in Version: 20.6.0 | YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name               | Date        | Version No    | Ticket
// ------------------------------------------------------------------------------------------------------
// Pramod Prakash Pokale        | 11/13/2018  | 20.6.0        | YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
/************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMCAObjects;
using System.Configuration; 

namespace YMCARET.YmcaBusinessObject
{
    public sealed class CommonBOClass
    {
        public static ReturnObject<Boolean> SendEmailCopyToIDM(string entityID, string mailBody, string docCode)
        {
            System.Net.WebClient client;
            YRSDMService.YRSDMService serviceClient;
            YRSDMService.WebServiceReturn serviceOutput;
            string xml;
            ReturnObject<Boolean> result;
            try
            {
                result = new ReturnObject<bool>();

                client = new System.Net.WebClient();
                serviceClient = new YRSDMService.YRSDMService();
                serviceClient.Credentials = System.Net.CredentialCache.DefaultCredentials;
                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(serviceClient.Url);

                // Added to pass login windows credentials to the DMS Webservice
                request.Credentials = serviceClient.Credentials;
                var response = (System.Net.HttpWebResponse)request.GetResponse();
                // Web service up and running  
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.Value = false;
                    result.MessageList.Add(MetaMessageList.MESSAGE_LOAN_DMWEBSERVICE_OFFLINE.ToString());
                }
                else
                {
                    xml = CreateIDMServiceXML(entityID, mailBody, docCode);

                    serviceOutput = serviceClient.YRSMethod("PushFile", xml);
                    if (serviceOutput.ReturnStatus == YRSDMService.Status.Success)
                    {
                        result.Value = true;
                    }
                    else
                    {
                        result.Value = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add(serviceOutput.Message);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                CommonClass.LogException("SendEmailCopyToIDM", ex);
                
                result = new ReturnObject<bool>();
                result.Value = false;
                result.MessageList = new List<string>();
                result.MessageList.Add(MetaMessageList.MESSAGE_LOAN_EMAIL_COPY_NOT_SENT_ERROR.ToString());
                return result;
            }
            finally
            {
                result = null;
                xml = null;
                serviceOutput = null;
                serviceClient = null;
                client = null;
            }
        }

        public static string CreateIDMServiceXML(string entityID, string mailBody, string docCode)
        {
            System.Xml.XmlDocument doc;
            StringBuilder sb;
            string encryptedMailBody;
            try
            {
                //START: PPP | 11/13/2018 | YRS-AT-3101 | Bytes array returned from GetBytes() function is causing insertion of "space" like special chracter after every alphabet or character in saved file by IDM
                //                                        So replacing it with Encoding.ASCII.GetBytes(<StringContent>) which is not causing such problem.
                //                                        In future for PDF or other files we can use System.IO.File.ReadAllBytes(<FilePath>)
                //encryptedMailBody = Convert.ToBase64String(GetBytes(mailBody));
                encryptedMailBody = Convert.ToBase64String(Encoding.ASCII.GetBytes(mailBody));
                //END: PPP | 11/13/2018 | YRS-AT-3101 | Bytes array returned from GetBytes() function is causing insertion of "space" like special chracter after every alphabet or character in saved file by IDM
                sb = new StringBuilder();
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb))
                {
                    writer.WriteStartElement("root");
                    //START: parameters
                    writer.WriteStartElement("parameters");
                    writer.WriteElementString("ConectionString", ConfigurationManager.ConnectionStrings["YRS"].ConnectionString);
                    writer.WriteElementString("DocCode", docCode);
                    writer.WriteElementString("RefID", entityID);
                    writer.WriteElementString("EntityID", entityID);
                    //START: FileDetails
                    writer.WriteStartElement("FileDetails");
                    writer.WriteElementString("Extension", ".html");
                    writer.WriteElementString("BinaryData", encryptedMailBody);
                    writer.WriteEndElement();
                    //END: FileDetails
                    writer.WriteEndElement();
                    //END: parameters
                    writer.WriteEndElement();
                    writer.Flush();
                }

                doc = new System.Xml.XmlDocument();
                doc.LoadXml(sb.ToString());
                return string.Format("<root>{0}</root>", doc.DocumentElement.InnerXml);
            }
            catch
            {
                throw;
            }
            finally
            {
                encryptedMailBody = null;
                sb = null;
                doc = null;
            }
        }

        //START: PPP | 11/13/2018 | YRS-AT-3101 | Following function is not required
        //public static byte[] GetBytes(string str)
        //{
        //    byte[] bytes = new byte[str.Length * 2 - 1];
        //    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    return bytes;
        //}
        //END: PPP | 11/13/2018 | YRS-AT-3101 | Following function is not required

        // START : VC | 2018.11.21 | YRS-AT-4018 -  Method used to check whether the participant loan is in progress or not. 
        public static int ValidatePIIRestrictions(string persId)
        {
            return YMCARET.YmcaDataAccessObject.CommonDAClass.ValidatePIIRestrictions(persId);

        }
        // END : VC | 2018.11.21 | YRS-AT-4018 -  Method used to check whether the participant loan is in progress or not.
    }
}
