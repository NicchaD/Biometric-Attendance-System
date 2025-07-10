'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	MailUtil.aspx.vb
' Author Name		:	Ashutosh G
' Employee ID		:	
' Email				:	 
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : 
'*******************************************************************************
'Changed By:        On:                 IssueId
'Aparna Samala      20/04/2007          To send email -while error occurs copying the files
'*******************************************************************************
'Modified by Shubhrata Tripathi Dec 22nd 06 YREN 3006 To maintain transactions
'****************************************************************************************************************************************
'Modification History
'****************************************************************************************************************************************
'Modified By                    Date                                    Desription
'****************************************************************************************************************************************
'Ashutosh Patil                 21-May-2007                             GetEmailConfigurationDetails Function for Common Email
'                                                                       common email functionality.
'Ashutosh Patil                 12-Jun-2007                             YREN-3592 and Change in Function GetEmailConfigurationDetails
'Aparna Samala                  18/07/07                                Included changes made by Anita-Added Tostring.Trim for all strings
'Parmaesh K.                    July 31st 2007                          Added property MailFormatType, for the mail body format 
'Priya Jawale                   11/03/08                                Made changes into GetEmailConfigurationDetails() function to get mailids values values of reverse refund
'Ashish Srivastava              29-Jan-2009                             Added Email for Defualted TD Loan
'Nikunj Patel                   2009.03.19								Refactoring code in class, Generalizing the class
'Nikunj Patel                   2009.04.20								Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Prashant Pandey                2013.12.30								BT-2354 Logging emails generated from YRS application
'Manthan Rajguru                2015.09.16                              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Anudeep A                      2016.03.21                              YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
'****************************************************************************************************************************************
Option Strict On
Option Explicit On

Imports System
Imports System.Web.Mail
Imports System.Configuration
Imports System.Diagnostics
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports YMCAObjects

Public Class MailUtil

    Private strFromMail As String
    Private strToMail As String
    Private strMailMessage As String = String.Empty
    Private strSubject As String = String.Empty
    Private strCc As String
    Private colAttachments As New Collection
    Private strMailCategory As String
    Private mailFormat As Mail.MailFormat 'Added by Paramesh K.

    Public Sub New()
        MyBase.New()
    End Sub

#Region "Public Properties"
    Public Property FromMail() As String
        Get
            Return strFromMail
        End Get
        Set(ByVal Value As String)
            strFromMail = Value
        End Set
    End Property
    Public Property ToMail() As String
        Get
            Return strToMail
        End Get
        Set(ByVal Value As String)
            strToMail = Value
        End Set
    End Property
    Public Property Subject() As String
        Get
            Return strSubject
        End Get
        Set(ByVal Value As String)
            strSubject = Value
        End Set
    End Property
    Public Property MailMessage() As String
        Get
            Return strMailMessage
        End Get
        Set(ByVal Value As String)
            strMailMessage = Value
        End Set
    End Property
    'Added by Paramesh K. on July 31st 2008
    Public Property MailFormatType() As MailFormat
        Get
            Return mailFormat
        End Get
        Set(ByVal Value As MailFormat)
            mailFormat = Value
        End Set
    End Property
    Public Property SendCc() As String
        Get
            Return strCc
        End Get
        Set(ByVal Value As String)

            strCc = Value

        End Set
    End Property
    Public ReadOnly Property MailAttachments() As Collection
        Get
            Return colAttachments
        End Get
    End Property
    Public Property MailCategory() As String
        Get
            Return strMailCategory
        End Get
        Set(ByVal Value As String)
            strMailCategory = Value
            Logger.Write("Mailutil - Mailcategory --> Retreiving From,to and cc process started ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            Dim addresses As YMCARET.YmcaBusinessObject.MailBOClass.MailAddress
            addresses = YMCARET.YmcaBusinessObject.MailBOClass.GetEmailAddressByCategory(strMailCategory)
            If Not addresses Is Nothing Then
                FromMail = addresses.From
                ToMail = addresses.To
                SendCc = addresses.Cc
            End If
            Logger.Write("Mailutil - Mailcategory --> Retreiving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            'GetEmailConfigurationDetails(Value)
        End Set
    End Property
#End Region

    Private Overloads Shared Sub SendMail(ByVal strCategory As String, ByVal strFrom As String, ByVal strTo As String, ByVal strCCMail As String, ByVal strMessage As String, ByVal strSubject As String, ByVal mailFormatType As Mail.MailFormat, ByVal mailAttachments As Collection)
        Try
            Dim mailStatus As YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus

            'Start: Added By Prashant 2013.12.30 : BT-2354
            Dim longEmailDetailID As Long
            longEmailDetailID = YMCARET.YmcaBusinessObject.MailBOClass.InsertEmailDetails(0, strFrom, strTo, strCCMail, Nothing, strMessage, strSubject)
            'End: Added By Prashant 2013.12.30 : BT-2354

            mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus()
            Logger.Write("Mailutil - SendMail --> Mail process started ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            If mailStatus.ServiceStatus <> YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED Then
                Return
            End If
            Dim msg As MailMessage = New MailMessage
            If strCategory Is Nothing OrElse strCategory = String.Empty Then
                Logger.Write("Mailutil - SendMail --> Retreiving From,to and cc process started ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
                Dim addresses As YMCARET.YmcaBusinessObject.MailBOClass.MailAddress
                addresses = YMCARET.YmcaBusinessObject.MailBOClass.GetEmailAddressByCategory(strCategory)
                If Not addresses Is Nothing Then
                    msg.From = addresses.From
                    msg.To = addresses.To
                    msg.Cc = addresses.Cc
                End If
                Logger.Write("Mailutil - SendMail --> Retreiving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            End If

            If Not strFrom Is Nothing Then msg.From = strFrom.Trim()
            If Not strTo Is Nothing Then msg.To = strTo.Trim()
            If Not strCCMail Is Nothing Then msg.Cc = strCCMail.Trim()
            If Not strMessage Is Nothing Then msg.Body = strMessage.Trim()
            If Not strSubject Is Nothing Then msg.Subject = strSubject.Trim()
            msg.BodyFormat = mailFormatType

            If Not msg.Attachments Is Nothing AndAlso Not mailAttachments Is Nothing Then
                Dim ienum As IEnumerator = mailAttachments.GetEnumerator()
                While ienum.MoveNext()
                    msg.Attachments.Add(New MailAttachment(DirectCast(ienum.Current, String)))
                End While
            End If
            Dim strSmtpServer As String = ConfigurationSettings.AppSettings("smtpServer")
            If (Not strSmtpServer Is Nothing AndAlso strSmtpServer <> String.Empty) Then
                SmtpMail.SmtpServer = strSmtpServer
            End If

            Logger.Write("Mailutil - SendMail --> checking default address", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            If mailStatus.DefaultStatus = YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES Then
                Dim dsAdminDefaultEmailId As DataSet
                Dim l_string_Admin_DefaultEmailId As String = String.Empty
                dsAdminDefaultEmailId = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_DEFAULT_TO_EMAILID")
                If HelperFunctions.isNonEmpty(dsAdminDefaultEmailId) Then
                    l_string_Admin_DefaultEmailId = Convert.ToString(dsAdminDefaultEmailId.Tables(0).Rows(0)("Value"))
                End If
                msg.To = l_string_Admin_DefaultEmailId
                msg.Cc = ""
            End If
            Logger.Write("Mailutil - SendMail --> mail sending started", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            SmtpMail.Send(msg)

            'Start: Added By Prashant 2013.12.30 : BT-2354
            YMCARET.YmcaBusinessObject.MailBOClass.UpdateMailSentStatus(longEmailDetailID)
            'End: Added By Prashant 2013.12.30 : BT-2354

            Logger.Write("Mailutil - SendMail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Overloads Sub Send()
        Try
            SendMail("", FromMail, ToMail, SendCc, MailMessage, Subject, MailFormatType, MailAttachments)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Overloads Shared Sub Send(ByVal mailCategory As String, ByVal strFrom As String, ByVal strTo As String, ByVal strCCMail As String, _
                ByVal strMessage As String, ByVal strSubject As String, ByVal mailFormatType As Mail.MailFormat, ByVal colAttachments As Collection)
        Try
            Dim m As New MailUtil
            m.MailCategory = mailCategory
            m.ToMail = strTo
            m.FromMail = strFrom
            m.SendCc = strCCMail
            m.MailMessage = strMessage
            m.Subject = strSubject
            m.MailFormatType = mailFormatType
            m.colAttachments = colAttachments
            m.Send()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub GetEmailConfigurationDetails(ByVal paramstrConfigCategoryCode As String)
    '    'Created By Ashutosh Patil as on 21-May-2007
    '    'Common function GetEmailConfigurationDetails for common email.

    '    Dim l_Mail_dataset As DataSet
    '    Dim l_DataTable As DataTable
    '    Dim l_DataRow As DataRow

    '    'Added By Ashish 28-Jan-2009,Start
    '    Dim l_paramstrConfigCategoryCode As String
    '    'Added By Ashish 28-Jan-2009,End
    '    Try
    '        'Added By Ashish 28-Jan-2009,Start
    '        l_paramstrConfigCategoryCode = paramstrConfigCategoryCode
    '        If paramstrConfigCategoryCode.Trim().ToUpper() = "TDLOAN_DEFAULTED" Then
    '            l_paramstrConfigCategoryCode = "TDLoan"
    '        End If
    '        l_Mail_dataset = YMCARET.YmcaBusinessObject.MailBOClass.GetMailConfigurationDetails(l_paramstrConfigCategoryCode)
    '        'Added By Ashish 28-Jan-2009,End
    '        'Commented by Ashish on 28-Jan-2009
    '        'l_Mail_dataset = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetMailConfigurationDetails(paramstrConfigCategoryCode, l_int_NoMailService, l_int_UseDefault)

    '        'If mail service is disabled return the constant string
    '        If MailService() = False Then
    '            Exit Sub
    '        End If

    '        If isNonEmpty(l_Mail_dataset) AndAlso isNonEmpty(l_Mail_dataset.Tables("EmailDetails")) Then

    '            l_DataTable = l_Mail_dataset.Tables(0)
    '            If isNonEmpty(l_DataTable) Then Exit Sub

    '            Select Case paramstrConfigCategoryCode
    '                Case "TDLoan"
    '                    For Each l_DataRow In l_DataTable.Rows
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "TDLOANS_FROM_EMAILID") Then
    '                            Me.FromMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "TDLOANS_DEFAULT_TO_EMAILID") Then
    '                            Me.ToMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                    Next
    '                    'Default code moved to the SendMail Process
    '                Case "TDLoan_Defaulted"
    '                    For Each l_DataRow In l_DataTable.Rows
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "TDLOANS_FROM_EMAILID") Then
    '                            Me.FromMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "TDLOANS_DEFAULT_TO_EMAILID") Then
    '                            Me.ToMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "TDLOANS_DEFAULTED_CC_EMAILID") Then
    '                            Me.SendCc = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                    Next
    '                    'Default code moved to the SendMail Process
    '                Case "DELINQ"
    '                    For Each l_DataRow In l_DataTable.Rows
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "DELINQ_FROM_EMAILID") Then
    '                            Me.FromMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "DELINQ_DEFAULT_TO_EMAILID") Then
    '                            Me.ToMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "DELINQ_CC_EMAILID") Then
    '                            Me.SendCc = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                    Next
    '                Case "ADMIN"
    '                    For Each l_DataRow In l_DataTable.Rows
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "ADMIN_FROM_EMAILID") Then
    '                            Me.FromMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "ADMIN_TO_EMAILID") Then
    '                            Me.ToMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                    Next
    '                    'Default code moved to the SendMail Process

    '                    'Priya 11/03/08 Made changes into GetEmailConfigurationDetails() function to get mailids values values of reverse refund
    '                Case "REFUND"

    '                    Dim ds As DataSet
    '                    ds = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_FROM_EMAILID")
    '                    If isNonEmpty(ds) Then
    '                        Me.FromMail = ds.Tables(0).Rows(0)("Value").ToString()
    '                    End If

    '                    For Each l_DataRow In l_DataTable.Rows
    '                        If (CType(l_DataRow("chvKey"), String).Trim = "BACKDATED_REFUND_REVERSE_MAILIDS") Then
    '                            Me.ToMail = CType(l_DataRow("chvValue"), String)
    '                        End If
    '                    Next

    '            End Select
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Public Function UseDefault() As Boolean
        Dim mailStatus As YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus
        mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus()
        If mailStatus.DefaultStatus = YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function MailService() As Boolean
        Dim mailStatus As YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus
        mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus()
        If mailStatus.ServiceStatus = YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Overloads Shared Sub SendMail(ByVal enumTemplateName As EnumEmailTemplateTypes, ByVal strFromMail As String, ByVal strToMail As String, ByVal strCcMail As String, ByVal strBccMail As String, ByVal dicParameters As Dictionary(Of String, String), ByVal strMailAttachments As String, ByVal dsMultipleParameter As DataSet, ByVal mailFormatType As Mail.MailFormat)
        Try

            Dim strFiles As String()
            Dim emailtemplate As EmailTemplate
            emailtemplate = YMCARET.YmcaBusinessObject.MailBOClass.Compose(enumTemplateName.ToString(), strFromMail, strToMail, strCcMail, strBccMail, dicParameters, strMailAttachments, dsMultipleParameter)


            ''Logging EmailDetail'
            Dim longEmailDetailID As Long
            longEmailDetailID = YMCARET.YmcaBusinessObject.MailBOClass.InsertEmailDetails(emailtemplate)


            Dim mailStatus As YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus
            'Dim longEmailDetailID As Long
            mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus()
            If mailStatus.ServiceStatus <> YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED Then
                Logger.Write("Mailutil - SendMail --> Mail Service Status is disabled.", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
                Return
            End If
            Dim msg As MailMessage = New MailMessage()

            If Not emailtemplate Is Nothing Then
                msg.From = emailtemplate.FromMail
                msg.To = emailtemplate.ToEmail
                msg.Cc = emailtemplate.CcEmail
                msg.Subject = emailtemplate.Subject
                msg.Body = emailtemplate.Body + Convert.ToString(emailtemplate.Footer)
                msg.BodyFormat = mailFormatType
            End If
            Logger.Write("Mailutil - SendMail --> Retrieving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)

            If Not emailtemplate.ListEmailAttachment Is Nothing AndAlso emailtemplate.ListEmailAttachment.Count > 0 Then
                If emailtemplate.DynamicFileAttachment Then
                    strFiles = strMailAttachments.Split(CChar(";"))
                    AddAttachment(strFiles, msg)
                End If

                If Not String.IsNullOrEmpty(emailtemplate.StaticFileAttachment) Then
                    strFiles = emailtemplate.StaticFileAttachment.Split(CChar(";"))
                    AddAttachment(strFiles, msg)
                End If

            End If
            Dim strSmtpServer As String = ConfigurationManager.AppSettings("smtpServer")
            If (Not strSmtpServer Is Nothing AndAlso strSmtpServer <> String.Empty) Then
                SmtpMail.SmtpServer = strSmtpServer
            End If

            Logger.Write("Mailutil - SendMail --> checking default address", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            If mailStatus.DefaultStatus = YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES Then
                Dim dsAdminDefaultEmailId As DataSet
                Dim l_string_Admin_DefaultEmailId As String = String.Empty
                dsAdminDefaultEmailId = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_DEFAULT_TO_EMAILID")
                If HelperFunctions.isNonEmpty(dsAdminDefaultEmailId) Then
                    l_string_Admin_DefaultEmailId = Convert.ToString(dsAdminDefaultEmailId.Tables(0).Rows(0)("Value"))
                End If
                msg.To = l_string_Admin_DefaultEmailId
                msg.Cc = ""
                msg.Bcc = ""
            End If
            Logger.Write("Mailutil - SendMail --> mail sending started", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)


            SmtpMail.Send(msg)
            'Update the mailsent status
            YMCARET.YmcaBusinessObject.MailBOClass.UpdateMailSentStatus(longEmailDetailID)
            Logger.Write("Mailutil - SendMail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
        Catch ex As Exception
            Throw
        End Try

    End Sub
    'Start:AA:03.21.2016 YRS-AT-2594 Added below code to send an email and mailutil objects properties can also be accessed like body of email
    'This method can be called only when mail util object is intilized for the usae of retrieving mailutil properties
    Public Overloads Sub SendMailMessage(ByVal enumTemplateName As EnumEmailTemplateTypes, ByVal strFromMail As String, ByVal strToMail As String, ByVal strCcMail As String, ByVal strBccMail As String, ByVal dicParameters As Dictionary(Of String, String), ByVal strMailAttachments As String, ByVal dsMultipleParameter As DataSet, ByVal mailFormatType As Mail.MailFormat)
        Try

            Dim strFiles As String()
            Dim emailtemplate As EmailTemplate
            emailtemplate = YMCARET.YmcaBusinessObject.MailBOClass.Compose(enumTemplateName.ToString(), strFromMail, strToMail, strCcMail, strBccMail, dicParameters, strMailAttachments, dsMultipleParameter)


            ''Logging EmailDetail'
            Dim longEmailDetailID As Long
            longEmailDetailID = YMCARET.YmcaBusinessObject.MailBOClass.InsertEmailDetails(emailtemplate)


            Dim mailStatus As YMCARET.YmcaBusinessObject.MailBOClass.MailServiceStatus
            'Dim longEmailDetailID As Long
            mailStatus = YMCARET.YmcaBusinessObject.MailBOClass.GetMailServiceStatus()
            If mailStatus.ServiceStatus <> YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.ENABLED Then
                Logger.Write("Mailutil - SendMail --> Mail Service Status is disabled.", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
                Return
            End If
            Dim msg As MailMessage = New MailMessage()

            If Not emailtemplate Is Nothing Then
                msg.From = emailtemplate.FromMail
                msg.To = emailtemplate.ToEmail
                msg.Cc = emailtemplate.CcEmail
                msg.Subject = emailtemplate.Subject
                msg.Body = emailtemplate.Body + Convert.ToString(emailtemplate.Footer)
                msg.BodyFormat = mailFormatType
            End If
            Logger.Write("Mailutil - SendMail --> Retrieving From,to and cc process completed ", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)

            If Not emailtemplate.ListEmailAttachment Is Nothing AndAlso emailtemplate.ListEmailAttachment.Count > 0 Then
                If emailtemplate.DynamicFileAttachment Then
                    strFiles = strMailAttachments.Split(CChar(";"))
                    AddAttachment(strFiles, msg)
                End If

                If Not String.IsNullOrEmpty(emailtemplate.StaticFileAttachment) Then
                    strFiles = emailtemplate.StaticFileAttachment.Split(CChar(";"))
                    AddAttachment(strFiles, msg)
                End If

            End If
            Dim strSmtpServer As String = ConfigurationManager.AppSettings("smtpServer")
            If (Not strSmtpServer Is Nothing AndAlso strSmtpServer <> String.Empty) Then
                SmtpMail.SmtpServer = strSmtpServer
            End If

            Logger.Write("Mailutil - SendMail --> checking default address", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            If mailStatus.DefaultStatus = YMCARET.YmcaBusinessObject.MailBOClass.MailStatuses.USE_DEFAULT_ADDRESSES Then
                Dim dsAdminDefaultEmailId As DataSet
                Dim l_string_Admin_DefaultEmailId As String = String.Empty
                dsAdminDefaultEmailId = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("ADMIN_DEFAULT_TO_EMAILID")
                If HelperFunctions.isNonEmpty(dsAdminDefaultEmailId) Then
                    l_string_Admin_DefaultEmailId = Convert.ToString(dsAdminDefaultEmailId.Tables(0).Rows(0)("Value"))
                End If
                msg.To = l_string_Admin_DefaultEmailId
                msg.Cc = ""
                msg.Bcc = ""
            End If
            Logger.Write("Mailutil - SendMail --> mail sending started", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)

            Try
                SmtpMail.Send(msg)
                MailMessage = msg.Body
                'Update the mailsent status
                YMCARET.YmcaBusinessObject.MailBOClass.UpdateMailSentStatus(longEmailDetailID)
            Catch ex As Exception
                HelperFunctions.LogException("MailUtil --> SendMailMessage", ex)
            End Try
            
            Logger.Write("Mailutil - SendMail --> mail sending ended", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'End:AA:03.21.2016 YRS-AT-2594 Added below code to send an email and mailutil objects properties can also be accessed like body of email
    Private Shared Sub AddAttachment(ByVal strFiles() As String, ByRef msg As MailMessage)
        If strFiles.Length > 0 Then
            For iindex As Integer = 0 To strFiles.Length - 1
                msg.Attachments.Add(New MailAttachment(strFiles(iindex)))
            Next
        End If
    End Sub

End Class