'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	UnfundedUEINs.aspx.vb
' Author Name		:	Bala
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	01/27/2016
' Program Specification Name	: 
' Unit Test Plan Name			:	
' Description					: Displays unfunded UEIN with selection option. Sends the selected UEIN to their respective LPA's
' Cache-Session                 : 
'************************************************************************************
'Modficiation History
'************************************************************************************
'Modified By		    Date	        Description
'************************************************************************************
'Pramod Prakash Pokale  2016.01.29      YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
'Anudeep A              2016.03.21      YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
'Pramod Prakash Pokale  2016.04.29      YRS-AT-2954 -  YRS enh: send copy of email to IDM in HTML format (,html) rather than Text (.txt) 
'Manthan Milan Rajguru  2016.05.05      YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
'Anudeep A              2016.06.27      YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
'Shilpa N               2019.03.11      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'**************************************************************************************************************************************************

Option Explicit On
Imports System
Imports System.Data
Imports YMCAUI.SessionManager
Imports YMCARET.YmcaBusinessObject
Public Class UnfundedUEINs
    Inherits System.Web.UI.Page
    'Start: AA:03.21.2016 YRS_AT_2594 Added below constants to access the gridview columns
    Private m_int_const_UEINTransmittalsID As Int16 = 0
    Private m_int_const_YMCAName As Int16 = 1
    Private m_int_const_TransmittalNo As Int16 = 2
    Private m_int_const_TransmittalDate As Int16 = 3
    Private m_int_const_AmountDue As Int16 = 4
    Private m_int_const_LPA_Name As Int16 = 5
    Private m_int_const_LPA_EmailId As Int16 = 6
    Private m_int_const_LPA_YMCAId As Int16 = 7
    'End AA:03.21.2016 YRS_AT_2594 Added below constants to access the gridview columns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CheckReadOnlyMode()
            If Not IsPostBack Then
                LoadUnfundedUEIN()
                divConfirmationMessage.InnerText = MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UEIN_CONFIRMATION).DisplayText
                hdnValidationError.Value = MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UEIN_CHECKBOX_VALIDATION_ERROR).DisplayText
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Unfunded UEINs --> Page Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub LoadUnfundedUEIN()
        Dim dsUnfundedUEIN As DataSet = New UEINBOClass().GetUnfundedUEIN()
        Dim dtUnfundedUEIN As DataTable = dsUnfundedUEIN.Tables("UnfundedUEIN")

        Session("dtUnfundedUEIN") = dtUnfundedUEIN
        gvUnfundedUEIN.DataSource = dtUnfundedUEIN
        gvUnfundedUEIN.DataBind()
    End Sub

    Protected Sub chkboxSelectAllUnfunded_CheckedChanged(sender As Object, e As EventArgs)
        Dim ChkBoxHeader As CheckBox = CType(gvUnfundedUEIN.HeaderRow.FindControl("checkboxSelectAllUnfunded"), CheckBox)
        For Each row As GridViewRow In gvUnfundedUEIN.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("checkboxUnfunded"), CheckBox)
            ChkBoxRows.Checked = ChkBoxHeader.Checked
        Next
    End Sub

    Protected Sub gvUnfundedUEIN_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            'Retrieve the table from the session object.
            Dim dt = TryCast(Session("dtUnfundedUEIN"), DataTable)

            If dt IsNot Nothing Then

                'Sort the data.
                dt.DefaultView.Sort = e.SortExpression & " " & GetSortDirection(e.SortExpression)
                gvUnfundedUEIN.DataSource = Session("dtUnfundedUEIN")
                gvUnfundedUEIN.DataBind()

            End If
        Catch ex As Exception
            HelperFunctions.LogException("Unfunded UEINs --> gvUnfundedUEIN_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Function GetSortDirection(ByVal column As String) As String

        ' By default, set the sort direction to ascending.
        Dim sortDirection = "ASC"

        ' Retrieve the last column that was sorted.
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)

        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
                  AndAlso lastDirection = "ASC" Then

                    sortDirection = "DESC"

                End If
            End If
        End If

        ' Save new values in ViewState.
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column

        Return sortDirection

    End Function

    Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String
        Dim str As New System.IO.MemoryStream()
        dt.WriteXml(str, True)
        str.Seek(0, System.IO.SeekOrigin.Begin)
        Dim sr As New System.IO.StreamReader(str)
        Dim xmlstr As String
        xmlstr = sr.ReadToEnd()
        Return (xmlstr)
    End Function

    Protected Sub btnProceed_Click(sender As Object, e As EventArgs) Handles btnProceed.Click
        Dim dtUnfundedUEIN As DataTable
        Dim dc As DataColumn
        Dim lsTransmitalID As List(Of Integer)
        Dim iTransmittalNo As Integer
        Dim chkRow As CheckBox
        Dim hdnUeinTransmittalNo As HiddenField
        Dim strXmlContent As String
        Dim strSelectionColumnName As String
        'Start:AA:03.21.2016 YRS_AT_2594 
        Dim dtSelectedYmcas As DataTable
        Dim dictParameters As Dictionary(Of String, String)
        Dim objMailUtil As MailUtil
        Dim strTableRows As StringBuilder
        Dim drUEINSForSelectedYMCARows As DataRow()
        Dim strXml As String
        Dim objYRSDMServiceRet As YRSDMService.WebServiceReturn
        Dim objYRSDMService As YRSDMService.YRSDMService
        Dim strBatchId As String
        Dim client As New Net.WebClient()
        Dim intMailCopyToIDM As Integer = 0 'Manthan | 2016.05.05 | YRS-AT-2594 | Initializing declared variable
        'End:AA:03.21.2016 YRS_AT_2594 
        Try
            client = New Net.WebClient()
            objYRSDMService = New YRSDMService.YRSDMService()
            objYRSDMService.Credentials = System.Net.CredentialCache.DefaultCredentials 'AA:06.27.2016 YRS-AT-2954 Added to pass login windows credentials to the DMS Webservice
            Dim request = DirectCast(Net.WebRequest.Create(objYRSDMService.Url), Net.HttpWebRequest)
            Try
                request.Credentials = objYRSDMService.Credentials 'AA:06.27.2016 YRS-AT-2954 Added to pass login windows credentials to the DMS Webservice
                Dim response = DirectCast(request.GetResponse(), Net.HttpWebResponse)
                ' Web service up and running  
                If response.StatusCode <> Net.HttpStatusCode.OK Then
                    HelperFunctions.ShowMessageToUser("DM Web Service is not available therefore cannot proceed further, please contact IT department")
                    Exit Sub
                End If
            Catch ex As Exception
                HelperFunctions.ShowMessageToUser("DM Web Service is not available due to the following error therefore cannot proceed further, please contact IT department" + Environment.NewLine + "Error: " + ex.Message + "", EnumMessageTypes.Error)
                HelperFunctions.LogException("UnfundedUEINs_btnProceed_Click", ex)
                Exit Sub
            End Try
            strSelectionColumnName = "Selection"
            dtUnfundedUEIN = TryCast(Session("dtUnfundedUEIN"), DataTable)
            If (Not dtUnfundedUEIN Is Nothing) Then
                If (dtUnfundedUEIN.Columns.Contains(strSelectionColumnName)) Then
                    dtUnfundedUEIN.Columns.Remove(strSelectionColumnName)
                End If

                dc = New DataColumn(strSelectionColumnName, System.Type.GetType("System.Boolean"))
                dtUnfundedUEIN.Columns.Add(dc)

                lsTransmitalID = New List(Of Integer)()
                'dim rows as new list(of gridviewrow)()
                For Each row As GridViewRow In gvUnfundedUEIN.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        chkRow = CType(row.Cells(0).FindControl("checkboxUnfunded"), CheckBox)
                        If Not chkRow Is Nothing AndAlso (chkRow.Checked) Then
                            'rows.add(row)
                            hdnUeinTransmittalNo = CType(row.Cells(0).FindControl("hdnUEINTransmittalID"), HiddenField)
                            If Not hdnUeinTransmittalNo Is Nothing Then
                                lsTransmitalID.Add(Convert.ToInt32(hdnUeinTransmittalNo.Value))
                            End If
                        End If
                    End If
                Next

                For Each row As DataRow In dtUnfundedUEIN.Rows
                    row(strSelectionColumnName) = False
                    iTransmittalNo = Convert.ToInt32(row("UEINTransmittalsID"))
                    If lsTransmitalID.Exists(Function(m As Integer) m = iTransmittalNo) Then
                        row(strSelectionColumnName) = True
                    End If
                Next

                'Start:AA:03.21.2016 YRS_AT_2594 Added below code to call yrs mail calss and DMS webservice 
                'strXmlContent = ConvertDatatableToXML(dtUnfundedUEIN)
                strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.MRDBO.GetNextBatchId(Date.Now.ToString("MM/dd/yyyy")).Tables(0).Rows(0)(0))
                YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, "UnfundedUEINsEmailGeneration", dtUnfundedUEIN.DataSet)
                dtSelectedYmcas = dtUnfundedUEIN.Select("Selection = True").CopyToDataTable().DefaultView.ToTable(True, "YMCAName") 'Getting the selected ymca names 
                objMailUtil = New MailUtil
                strXml = String.Empty

                For Each dtSelectedrow As DataRow In dtSelectedYmcas.Rows
                    drUEINSForSelectedYMCARows = dtUnfundedUEIN.Select("Selection = True AND YMCAName = '" + dtSelectedrow("YMCAName").ToString().Replace("'", "''") + "'") 'Getting the details of ymca using the ymcaname and on selection 
                    If drUEINSForSelectedYMCARows.Length > 0 Then
                        strTableRows = New StringBuilder()
                        For Each row As DataRow In drUEINSForSelectedYMCARows
                            strTableRows.Append(String.Format("<tr><td>{0}</td><td>${1}</td></tr>", row("TransmittalNo").ToString(), row("AmountDue").ToString()))
                        Next
                        dictParameters = New Dictionary(Of String, String)
                        dictParameters.Add("LPAName", drUEINSForSelectedYMCARows(0)("LPAName"))
                        dictParameters.Add("UEINTransmittalRows", strTableRows.ToString())
                        If objMailUtil IsNot Nothing Then
                            'Start - Manthan | 2016.05.05 | YRS-AT-2594 | Logging exception error during email sending failure
                            Try
                                objMailUtil.SendMailMessage(EnumEmailTemplateTypes.UNFUNDED_UEIN_INTIMATION, "", drUEINSForSelectedYMCARows(0)("Email"), "", "", dictParameters, "", Nothing, Mail.MailFormat.Html) ' Calling email sending method
                            Catch ex As Exception
                                HelperFunctions.LogException("UnfundedUEINs --> btnProceed_SendingMail", ex)
                                HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_UEIN_MAIL_SENT_ERROR)
                                Exit Sub
                            End Try
                            'End - Manthan | 2016.05.05 | YRS-AT-2594 | Logging exception error during email sending failure
                            If Not String.IsNullOrEmpty(objMailUtil.MailMessage) Then
                                'PPP | 04/29/2016 | YRS-AT-2954 | Changing <Extension>.txt</Extension> to <Extension>.html</Extension>
                                'Preparing XML to call webservice
                                strXml = String.Format("<root><parameters><ConectionString>{0}</ConectionString>" +
                                    "<DocCode>{1}</DocCode><RefID>{2}</RefID><EntityID>{3}</EntityID><FileDetails><Extension>.html</Extension><BinaryData>{4}</BinaryData></FileDetails></parameters></root>",
                                    ConfigurationManager.ConnectionStrings("YRS").ConnectionString,
                                    YMCAObjects.IDMDocumentCodes.Unfunded_Interest_Summary,
                                    drUEINSForSelectedYMCARows(0)("YMCAID").ToString(),
                                    drUEINSForSelectedYMCARows(0)("YMCAID").ToString(),
                                    Convert.ToBase64String(HelperFunctions.GetBytes(objMailUtil.MailMessage)))
                                'Start - Manthan | 2016.05.05 | YRS-AT-2594 | Logging exception error when mail copy not sent to IDM and counter to check number of emails copied to IDM
                                Try
                                    objYRSDMServiceRet = objYRSDMService.YRSMethod("PushFile", strXml) 'Webservice call
                                    If objYRSDMServiceRet Is Nothing Then
                                        intMailCopyToIDM = intMailCopyToIDM + 1
                                    ElseIf objYRSDMServiceRet.ReturnStatus = YRSDMService.Status.Failure Then
                                        intMailCopyToIDM = intMailCopyToIDM + 1
                                    End If
                                Catch ex As Exception
                                    intMailCopyToIDM = intMailCopyToIDM + 1
                                    HelperFunctions.LogException("UnfundedUEINs --> btnProceed_Sending Mail copy into IDM", ex)
                                End Try
                                'End - Manthan | 2016.05.05 | YRS-AT-2594 | Logging exception error when mail copy not sent to IDM and counter to check number of emails copied to IDM
                            Else
                                'Start - Manthan | 2016.05.05 | YRS-AT-2594 | Showing error message when mail not sent
                                HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_UEIN_MAIL_SENT_ERROR)
                                Exit Sub
                                'End - Manthan | 2016.05.05 | YRS-AT-2594 | Showing error message when mail not sent
                            End If
                        End If
                    End If
                Next

                'If (New UEINBOClass().SendEmailOfUnfundedUEINs(strXmlContent)) Then
                If intMailCopyToIDM = 0 Then 'Manthan | 2016.05.05 | YRS-AT-2594 | Added condition to check if counter is zero to show success message
                    'End:AA:03.21.2016 YRS_AT_2594 Added below code to call yrs mail calss and DMS webservice 
                    HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_UEIN_MAIL_SENT_SUCCESS)
                Else
                    HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_UEIN_MAIL_COPY_NOT_SENT_ERROR) 'Manthan | 2016.05.05 | YRS-AT-2594 | Showing error message when mail copy not sent to IDM
                End If
            Else
                HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_UEIN_MAIL_SENT_ERROR)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Unfunded UEINs --> btnProceed_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            strXmlContent = Nothing
            hdnUeinTransmittalNo = Nothing
            chkRow = Nothing
            lsTransmitalID = Nothing
            dc = Nothing
            dtUnfundedUEIN = Nothing
        End Try
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs)
        Session("dtUnfundedUEIN") = Nothing
        Response.Redirect("MainWebForm.aspx")
    End Sub
    'Start: AA:03.21.2016 YRS_AT_2594 
    'Added below code to hide lpa name, lpa emailid , ymcaid columns in gridview
    Private Sub gvUnfundedUEIN_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUnfundedUEIN.RowDataBound
        Dim chk As CheckBox
        Try
            e.Row.Cells(m_int_const_LPA_Name).Visible = False
            e.Row.Cells(m_int_const_LPA_EmailId).Visible = False
            e.Row.Cells(m_int_const_LPA_YMCAId).Visible = False
            If e.Row.RowType = DataControlRowType.DataRow Then
                If String.IsNullOrEmpty(e.Row.Cells(m_int_const_LPA_EmailId).Text) OrElse IsDBNull(e.Row.Cells(m_int_const_LPA_EmailId).Text) OrElse e.Row.Cells(m_int_const_LPA_EmailId).Text = "&nbsp;" Then
                    e.Row.CssClass = "NoEmailexists"
                    chk = e.Row.Cells(0).FindControl("checkboxUnfunded")
                    If chk IsNot Nothing Then
                        chk.Checked = False
                        chk.Enabled = False
                        chk.Attributes.Add("disabled", "disabled")
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Unfunded UEINs --> gvUnfundedUEIN_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'End: AA:03.21.2016 YRS_AT_2594 

    'START: Shilpa N | 03/11/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            hdnGenerateEmail.Value = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
        'END: Shilpa N | 03/11/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    End Sub
End Class