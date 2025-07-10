'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ServiceManager.vb
' Author Name		:	Sanjay R.    
' Employee ID		:	51193
' Email				:	sanjay.singh@3i-infotech.com
' Contact No		:	8637
' Creation Time		:	06-Aug-2013
'*******************************************************************************

'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Sanjay R           2013.08.06      Added method to get Person status from Tom's web service
'Anudeep            2013.08.23      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep            2013.09.30      BT-1501 :YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            2015.05.05      BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Pramod P. Pokale   2017.01.12      YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing 
'                                   YRS-AT-3265 -  YRS enh:improve usability of QDRO split screens (TrackIT 28050) 
'Santosh B          2018.09.05      YRS-AT-4081 -  YRS enh: YRS enhancement-generate email to LPA when loan is closed.Track it 34779 
'*********************************************************************************************************************
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Xml

Namespace ServiceManager
    Public Class AdminConsoleBeneficiaryTracking
        Private Sub New()
        End Sub
        Public Shared Function IsValidPerson(ByVal strPersID As String) As String
            Dim result As String
            Dim objService As AdminProxyService
            Dim strxml As StringBuilder
            Dim xmlDoc As XmlDocument
            Dim ndRoot As XmlNode
            Dim ndMsgPending As XmlNode
            Try
                'START: PPP | 01/12/2017 | YRS-AT-3145 & 3265 | Tracing
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("IsValidPerson", "Start")
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("IsValidPerson", String.Format("PersID : {0}", strPersID))
                'END: PPP | 01/12/2017 | YRS-AT-3145 & 3265 | Tracing
                'Start:Anudeep:23.08.2013 -YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS) 
                'Code addedby Anudeep:23.08.2013 to check whether the key exists in web.config 
                If System.Configuration.ConfigurationSettings.AppSettings("Admin_WS_Path") Is Nothing Then
                    Return "Admin_WS_Path key does not exists in configuration file"
                End If
                If System.Configuration.ConfigurationSettings.AppSettings("Admin_WS_Path").ToString.Trim() = "" Then
                    Return "Admin_WS_Path key value is empty in configuration file"
                End If
                'Code addedby Anudeep:23.08.2013 to check whether the key value in web.config has been set to False
                If System.Configuration.ConfigurationSettings.AppSettings("Admin_WS_Path").ToString().ToUpper() = "FALSE" Then
                    Return "NoPending"
                End If

                objService = New AdminProxyService
                strxml = New StringBuilder
                xmlDoc = New XmlDocument
                'End:Anudeep:23.08.2013 -YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS) 
                strxml.Append("<root><userId>" + strPersID + "</userId></root>")
                'strxml.Append("<root><userId>7da455ab-3c78-4183-99fa-0003dce891cd</userId></root>")
                'strxml.Append("<root><userId>b991b38b-0be9-4d73-a455-50b717b59d00</userId></root>")
                'result = objService.PendUserBeneficiaries(strxml.ToString())
                result = objService.CheckUserPendingBeneficiaries(strxml.ToString())
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("IsValidPerson", result) ' PPP | 01/12/2017 | YRS-AT-3145 & 3265 | Tracing result of service

                xmlDoc.LoadXml(result)
                ndRoot = xmlDoc.SelectSingleNode("root")
                ndMsgPending = xmlDoc.SelectSingleNode("ServiceData/InputInformation/msgPending")

                If ndRoot Is Nothing AndAlso ndMsgPending Is Nothing Then
                    'Anudeep:30.09.2013:BT-1501: Changed the below message to specify which service is unavailable
                    'Return "Service unavailable. Please contact Administrator"
                    Return "Admin console web service is unavailable, please contact IT department"
                    Exit Function
                ElseIf Not (ndMsgPending Is Nothing) Then
                    Return ndMsgPending.InnerText.ToString()
                ElseIf Not (ndRoot Is Nothing) Then
                    If ndRoot.Attributes("status").Value = "NoPending" Then
                        Return "NoPending"
                    Else
                        Return ndRoot.Attributes("status").Value.ToString()
                    End If
                Else
                    Return "Invalid status"

                End If
            Catch ex As Exception
                HelperFunctions.LogException("IsValidPerson", ex) 'PPP | 01/12/2017 | YRS-AT-3145 | Logging exception message

                'Anudeep:30.09.2013:BT-1501: Changed the below message to specify which service is unavailable
                'Return "Service unavailable. Please contact Administrator"
                Return "Admin console web service is unavailable, please contact IT department"
            Finally
                'START: PPP | 01/12/2017 | YRS-AT-3145 & 3265 | Tracing 
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("IsValidPerson", "End")
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                'END: PPP | 01/12/2017 | YRS-AT-3145 & 3265 | Tracing 
            End Try
        End Function
    End Class

    'Start: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added to call the web account info service
    Public Class AdminWebAccountInfo

        Function LockAccount(strUserId As String, strUpdater As String) As String
            Try
                Dim objService As New AdminWebAccountInfoProxyService
                Return objService.NS_setLockAccountAC(strUserId, strUpdater)
            Catch ex As Exception
                HelperFunctions.LogException("ServiceManager_AdminWebAccountInfo_LockAccount", ex)
                Return "error|" + ex.Message
            End Try
        End Function

        Function SendTempPasswordEmail(strUserId As String, strUpdater As String) As String
            Try
                Dim objService As New AdminWebAccountInfoProxyService
                Return objService.NS_ForgotPassSendEmail(strUserId, strUpdater)
            Catch ex As Exception
                HelperFunctions.LogException("ServiceManager_AdminWebAccountInfo_SendTempPasswordEmail", ex)
                Return "error|" + ex.Message
            End Try
        End Function

        Function UnLockAccount(strUserId As String, strUpdater As String) As String
            Try
                Dim objService As New AdminWebAccountInfoProxyService
                Return objService.NS_setUnLockAccountAC(strUserId, strUpdater)
            Catch ex As Exception
                HelperFunctions.LogException("ServiceManager_AdminWebAccountInfo_UnLockAccount", ex)
                Return "error|" + ex.Message
            End Try
        End Function

        Function GetUserDetails(strUserID As String) As String
            Try
                Dim objService As New AdminWebAccountInfoProxyService
                Return objService.NS_getAdminData(strUserID)
            Catch ex As Exception
                HelperFunctions.LogException("ServiceManager_AdminWebAccountInfo_GetUserDetails", ex)
                Return "error|" + ex.Message
            End Try
        End Function

        Function GetAdminActivityLog(strUserID As String, ByRef strErrorMessage As String) As DataSet
            Try
                Dim objService As New AdminWebAccountInfoProxyService
                Return objService.NS_getAdminAcctivity(strUserID)
            Catch ex As Exception
                HelperFunctions.LogException("ServiceManager_AdminWebAccountInfo_GetAdminActivityLog", ex)
                strErrorMessage = "error|" + ex.Message
                Return Nothing
            End Try
        End Function
    End Class
    'End: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added to call the web account info service

    'START: SB | 09/05/2018 | YRS-AT-4081 | Following class will help to access DMS web service
    Public Class DMSService

        ' Following method will help to push HTML type of documents into IDM
        Public Shared Function SendEmailCopyToIDM(ByVal entityID As String, ByVal MailBody As String) As YMCAObjects.ReturnObject(Of Boolean)
            Dim client As New Net.WebClient()
            Dim serviceClient As YRSDMService.WebServiceReturn
            Dim serviceOutput As YRSDMService.YRSDMService
            Dim xml As String
            Dim result As YMCAObjects.ReturnObject(Of Boolean)
            Try
                result = New YMCAObjects.ReturnObject(Of Boolean)()
                result.Value = False

                client = New Net.WebClient()
                serviceOutput = New YRSDMService.YRSDMService()
                serviceOutput.Credentials = System.Net.CredentialCache.DefaultCredentials
                Dim request = DirectCast(Net.WebRequest.Create(serviceOutput.Url), Net.HttpWebRequest)

                'Added to pass login windows credentials to the DMS Webservice
                request.Credentials = serviceOutput.Credentials
                Dim response = DirectCast(request.GetResponse(), Net.HttpWebResponse)
                ' Web service up and running  
                If response.StatusCode <> Net.HttpStatusCode.OK Then
                    result.MessageList.Add("DM Web Service is not available therefore cannot proceed further, please contact IT department")
                Else
                    xml = String.Format("<root><parameters><ConectionString>{0}</ConectionString>" +
                                            "<DocCode>{1}</DocCode><RefID>{2}</RefID><EntityID>{3}</EntityID><FileDetails><Extension>.html</Extension><BinaryData>{4}</BinaryData></FileDetails></parameters></root>",
                                            ConfigurationManager.ConnectionStrings("YRS").ConnectionString,
                                            YMCAObjects.IDMDocumentCodes.Loan_PayedOff_LPA_Email,
                                            entityID,
                                            entityID,
                                            Convert.ToBase64String(HelperFunctions.GetBytes(MailBody)))
                    serviceClient = serviceOutput.YRSMethod("PushFile", xml)
                    If serviceClient.ReturnStatus = YRSDMService.Status.Success Then
                        result.Value = True
                    End If
                End If
                Return result
            Catch ex As Exception
                HelperFunctions.LogException("SendEmailCopyToIDM", ex)

                'To do  - How to handle
                result = New YMCAObjects.ReturnObject(Of Boolean)()
                result.Value = False
                result.MessageList.Add(String.Format("DM Web Service is not available due to the following error therefore cannot proceed further, please contact IT department. Error: {0}", ex.Message))
                Return result
            Finally

            End Try
        End Function

    End Class
    'END: SB | 09/05/2018 | YRS-AT-4081 | Following class will help to access DMS web service
End Namespace