'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCa-YRS
' FileName			:	ACHDebitReceiptApplicationForm.aspx.vb
' Author Name		:	Ashish Srivastava
' Employee ID		:	51821
' Email				:	ashish.srivastava@3i-infotech.com
' Contact No		:	8609
' Creation Time		:	04/24/2008 5:03:26 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Desription
'********************************************************************************************************************************
'Ashish Srivastava  02-Dec-2008     Funded date should be within current business month
'Ashish Srivastava  12-Jan-2009     Remove comma seprated YMca's no logic and optimized code
'Ashish Srivastava  28-Jan-2009     Added Sendmail for loan defaulted persons
'Nikunj Patel       2009.04.16      Mail Util changes
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Ashish Srivastava   2010.07.05     Enhancements08 changes 
'Anudeep            2012.12.24      YRS 5.0-1717:Issue email when loan paid off 
'Anudeep            2013-12-16      BT:2311-13.3.0 Observations
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Bala               2015.12.14      YRS-AT-2642: YRS enh: Remove ssno from sending loan defalulted email.
'Anudeep A          2016.04.12      YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Pramod P. Pokale   2016.07.27      YRS-AT-3058 - YRS bug: timing issue Loan Utility - defaulting a Loan and then transmittal w/payment, Auto closed as successfully paid off? (trackIt 26945) 
'********************************************************************************************************************************
#Region "Import Namespaces"
Imports System
Imports System.Text
Imports YMCARET.YmcaBusinessObject
Imports System.Drawing

#End Region


Public Class ACHDebitReceiptApplicationForm
    Inherits System.Web.UI.Page

    Dim strFormName As String = New String("ACHDebitReceiptApplicationForm.aspx")

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonBack As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHFundingDate As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonPostAndApply As System.Web.UI.WebControls.Button
    Protected WithEvents DatagridACHDebitMatchReceipt As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DateusercontrolFundedDate As YMCAUI.DateUserControl
    Protected WithEvents PlaceHolderPostAndApply As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Try
            If Not IsPostBack Then

                If Not Session("l_dsAchDebitMatchTransmittal") Is Nothing Then
                    Session("l_dsAchDebitMatchTransmittal") = Nothing
                End If
                If Not Session("l_dataset_ACHInsert") Is Nothing Then
                    LoadACHDebitMatchedTransmittals()
                End If
            End If
            If IsPostBack Then
                'AA:16.12.2013 - BT:2311 Commented below code and kept in buton yes click event
                'If (Session("IsDone") = True) Then
                '    Session("IsDone") = False
                '    Response.Redirect("MainWebForm.aspx", False)
                'End If

                'If Not Session("PostAndApplyOk") = Nothing Then
                '    If Request.Form("YES") = "Yes" And Session("PostAndApplyOk") = True Then
                '        Session("PostAndApplyOk") = Nothing
                '        AchDebitPostAndApplyReceipts()
                '    End If
                '    '***********

                'End If
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
        'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        'Me.Menu1.DataBind()






    End Sub

#Region "Private Methods"


    Private Sub LoadACHDebitMatchedTransmittals()
        Dim strYmcaNos As New StringBuilder
        Dim strBatchId As String = String.Empty
        Dim achDebitImportBOClass As ACHDebitImportBOClass
        Dim l_dsAchDebitMatchTransmittal As DataSet

        Try
            Dim dsAchDebitImport As DataSet
            dsAchDebitImport = CType(Session("l_dataset_ACHInsert"), DataSet)
            If dsAchDebitImport.Tables(0).Rows.Count > 0 Then
                strBatchId = dsAchDebitImport.Tables(0).Rows(0)("REFNO")
                LabelRefNo.Text = strBatchId

                'Added by Ashish on 02-Dec-2008 ,funded date should be within the current month, Start
                Dim l_string_AcctDate As String
                Dim l_ds_AcctDate As DataSet
                Dim l_DateTime_AcctDate As DateTime
                Dim l_DateTime_CurrentDate As DateTime

                'Set Current Date
                l_DateTime_CurrentDate = System.DateTime.Now.Date
                'Get the accounting date 
                l_ds_AcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
                l_string_AcctDate = l_ds_AcctDate.Tables(0).Rows(0).Item(0).ToString()
                If Not l_string_AcctDate.Equals(String.Empty) Then
                    l_DateTime_AcctDate = Convert.ToDateTime(l_string_AcctDate).Date
                    ViewState("CurrentAccountingDate") = l_string_AcctDate
                Else
                    l_DateTime_AcctDate = System.DateTime.Now.Date
                    ViewState("CurrentAccountingDate") = System.DateTime.Now.Date.ToString()
                End If

                If (l_DateTime_AcctDate.Year = l_DateTime_CurrentDate.Year And l_DateTime_AcctDate.Month < l_DateTime_CurrentDate.Month) Or l_DateTime_AcctDate.Year < l_DateTime_CurrentDate.Year Then

                    Me.DateusercontrolFundedDate.Text = l_DateTime_AcctDate.Date.ToString("MM/dd/yyyy")
                Else

                    Me.DateusercontrolFundedDate.Text = l_DateTime_CurrentDate.Date.ToString("MM/dd/yyyy")

                End If
                'Added by Ashish on 02-Dec-2008 ,funded date should be within the current month, End
                'Commented by Ashish on 02-Dec-2008
                'Me.DateusercontrolFundedDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy")

                'DateusercontrolFundedDate.RangeValidatorMinValue = System.DateTime.Now.Date.AddDays(-(System.DateTime.Now.Date.Day - 1)).ToString("MM/dd/yyyy")

                'DateusercontrolFundedDate.RangeValidatorMaxValue = System.DateTime.Now.Date.ToString("MM/dd/yyyy")
                'DateusercontrolFundedDate.RangeValidatorErrorMassege = "Funded Date should be between first day of current month to today date."
                'DateusercontrolFundedDate.EnabledDateRangeValidator = True
                DateusercontrolFundedDate.RequiredDate = True
                DateusercontrolFundedDate.RequiredValidatorErrorMessage = "Please enter funded date."
                'commented by Ashish on 12-Jan-2009,Start
                'For Each dtRow As DataRow In dsAchDebitImport.Tables(0).Rows
                '    If dtRow("Selected").ToString().Trim() = "1" Then
                '        strYmcaNos.Append(dtRow("YMCANO").ToString().Trim().PadLeft(6, "0") & ",")
                '    End If
                'Next
                'If strYmcaNos.Length > 0 And strBatchId <> String.Empty Then
                '    achDebitImportBOClass = New ACHDebitImportBOClass
                '    l_dsAchDebitMatchTransmittal = achDebitImportBOClass.GetAchDebitMatchedTransmittals(strYmcaNos.ToString(), strBatchId)

                'End If
                'commented by Ashish on 12-Jan-2009,End

                If strBatchId <> String.Empty Then
                    achDebitImportBOClass = New ACHDebitImportBOClass
                    l_dsAchDebitMatchTransmittal = achDebitImportBOClass.GetAchDebitMatchedTransmittals(strBatchId)

                End If
                RemoveUnSelectedYmcaTransmittals(dsAchDebitImport, l_dsAchDebitMatchTransmittal)

                If Not l_dsAchDebitMatchTransmittal Is Nothing Then
                    Session("l_dsAchDebitMatchTransmittal") = l_dsAchDebitMatchTransmittal
                    Me.DatagridACHDebitMatchReceipt.DataSource = l_dsAchDebitMatchTransmittal
                    Me.DatagridACHDebitMatchReceipt.DataBind()

                End If


            End If



        Catch ex As Exception
            Throw ex
        Finally
            achDebitImportBOClass = Nothing

        End Try
    End Sub
    Private Function ValidateCheckBoxSelection(ByRef intTotalRecord As Int32, ByRef intRecorsSelected As Int32) As Boolean
        Try
            Dim boolSelected As Boolean = False
            Dim ctr As Integer
            Dim ctr1 As Integer
            Dim totalRec As Integer
            ctr = 0
            ctr1 = 0
            Session("PostAndApplyOk") = Nothing

            For Each item1 As DataGridItem In DatagridACHDebitMatchReceipt.Items
                If item1.Cells(11).Text.Equals("1") Then
                    ctr1 += 1
                End If
            Next
            totalRec = ctr1
            For Each item As DataGridItem In DatagridACHDebitMatchReceipt.Items
                Dim chkBox As CheckBox
                chkBox = CType(item.FindControl("CheckBoxSelect"), CheckBox)
                If chkBox.Checked = True And item.Cells(11).Text.Equals("1") Then
                    boolSelected = True
                    ctr += 1
                End If
            Next


            intTotalRecord = totalRec
            intRecorsSelected = ctr
            Return boolSelected
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub AchDebitPostAndApplyReceipts()
        Dim achDebitImportBOClass As ACHDebitImportBOClass = Nothing
        Dim l_dsLoanPersonalDetails As DataSet
        Dim l_bool_ServiceUpdate As Boolean
        Dim l_LoggedBatchID As Int64
        Try

            achDebitImportBOClass = New ACHDebitImportBOClass
            If (Not Session("l_dataset_ACHInsert") Is Nothing And Not Session("l_dsAchDebitMatchTransmittal") Is Nothing) Then

                l_dsLoanPersonalDetails = achDebitImportBOClass.SavePostAndApplyReceipts(CType(Session("l_dataset_ACHInsert"), DataSet), CType(Session("l_dsAchDebitMatchTransmittal"), DataSet), CType(DateusercontrolFundedDate.Text, DateTime), l_bool_ServiceUpdate, l_LoggedBatchID)

            End If
            ' this part will do email sending. 
            ' email will be sent if the last remaining installment of the loan if paid i.e loan is re-paid in full
            'Commented by Ashish 29-Jan-2009,Start
            'If Not dtPersonalDetails Is Nothing Then


            '    If dtPersonalDetails.Rows.Count > 0 Then

            '        Try
            '            SendMail(dtPersonalDetails)
            '        Catch
            '            Dim AlertScript As String = "<script language='javascript'>" & _
            '                     "alert('There was a problem encountered while sending mail');</script>"

            '            ' Add the JavaScript code to the page.
            '            Page.RegisterStartupScript("AlertScript", AlertScript)
            '        End Try

            '    End If
            'End If
            'Commented by Ashish 29-Jan-2009,Start
            'Added by Ashish on 29-Jan-2009 ,Start
            If Not l_dsLoanPersonalDetails Is Nothing Then
                'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                'Dim dtIDMFTList As New DataTable 'AA:04.27.2016 YRS-AT-2830:Added to copy the files in IDM
                'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                If l_dsLoanPersonalDetails.Tables.Count > 0 Then
                    ' email will be sent if the last remaining installment of the loan if paid i.e loan is re-paid in full
                    If l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails").Rows.Count > 0 Then
                        Try
                            'start:AA:04.12.2016 YRS-AT-2830:Changed to call from common function where it will close loan and send email 
                            'SendMail(l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"))

                            'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                            'Call New LoanClass().SendLoanPaidClosedMail(l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"), dtIDMFTList)
                            Call New LoanClass().SendLoanPaidClosedMail(l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"))
                            'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                            'end:AA:04.12.2016 YRS-AT-2830:Changed to call from common function where it will close loan and send email 
                        Catch
                            Dim AlertScript As String = "<script language='javascript'>" & _
                                     "alert('There was a problem encountered while sending mail');</script>"

                            ' Add the JavaScript code to the page.
                            'Page.RegisterStartupScript("AlertScript1", AlertScript)
                            'AA:16.12.2013 - BT:2311 Added to open the copy to server pop-up in ajax postback page
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "AlertScript1", AlertScript, False)
                        End Try

                    End If
                    'Email sent for defaulted loan
                    If l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails").Rows.Count > 0 Then
                        Try
                            'Start: Bala: YRS-AT-2642: Remove ssno from sending email.
                            'SendLoanDefaultedMail(l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"))
                            'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                            'Call New LoanClass().SendLoanDefaultedClosedMail(l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"), dtIDMFTList) 'AA:04.12.2016 YRS-AT-2830:Changed to copy idm the file
                            Call New LoanClass().SendLoanDefaultedClosedMail(l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"))
                            'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                            'End: Bala: YRS-AT-2642: Remove ssno from sending email.
                        Catch
                            Dim AlertScript As String = "<script language='javascript'>" & _
                                     "alert('There was a problem encountered while sending mail');</script>"

                            ' Add the JavaScript code to the page.
                            'Page.RegisterStartupScript("AlertScript2", AlertScript)
                            'AA:16.12.2013 - BT:2311 Added to open the copy to server pop-up in ajax postback page
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "AlertScript2", AlertScript, False)
                        End Try

                    End If
                    'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                    ''start:AA:04.27.2016 YRS-AT-2830:Added below lines to copy the files in the IDM 
                    'If HelperFunctions.isNonEmpty(dtIDMFTList) Then
                    '    Session("FTFileList") = dtIDMFTList

                    '    'Call the ASPX to copy the file.
                    '    Dim popupScript As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp_5', " & _
                    '    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"

                    '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript5", popupScript, True)

                    'End If
                    ''End:AA:04.27.2016 YRS-AT-2830:Added below lines to copy the files in the IDM 
                    'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                End If
            End If
            'Added by Ashish on 29-Jan-2009 ,End
            'Email sent when service update failed
            If Not l_bool_ServiceUpdate Then
                Try
                    SendServiceFailedMail(l_LoggedBatchID)
                Catch
                    Dim AlertScript As String = "<script language='javascript'>" & _
                             "alert('There was a problem encountered while sending service update failed mail');</script>"

                    ' Add the JavaScript code to the page.
                    'Page.RegisterStartupScript("AlertServiceMail", AlertScript)
                    'AA:16.12.2013 - BT:2311 Added to open the copy to server pop-up in ajax postback page
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "AlertServiceMail", AlertScript, False)
                End Try

            End If
            Session("l_dataset_ACHInsert") = Nothing
            Session("l_dsAchDebitMatchTransmittal") = Nothing
            'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
            'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "Processed Successfully", MessageBoxButtons.OK)
            lblMessage.Text = "Processed Successfully"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','OK');", True)
            Session("IsDone") = True
            Me.ButtonBack.Enabled = False
            Me.ButtonPostAndApply.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub SendMail(ByVal l_datatable_persondetails As DataTable)

        ' This method will send email if the last remaining installment of the loan if paid i.e loan is re-paid in full
        Dim obj As New MailUtil
        Dim l_string_details As StringBuilder
        Dim drow As DataRow
        Try
            l_string_details = New StringBuilder
            'Start: Anudeep:24.12.2012 YRS 5.0-1717:Issue email when loan paid off 
            l_string_details.Append("You may now close the loan for following participants as the final payment of their TD Loan has been funded.")
            l_string_details.Append("<table width='50%' border='1' >")
            l_string_details.Append("<tr><td align='center'><B>Fund Id Number</B></td><td align='center'><B>First Name</B></td><td align='center'><B>Last Name</B></td></tr>")
            For Each drow In l_datatable_persondetails.Rows
                l_string_details.Append("<tr>")
                l_string_details.Append("<td>" & drow("intFundIdNo").ToString() & "</td>")
                l_string_details.Append("<td>" & drow("chvFirstName").ToString() & "</td>")
                l_string_details.Append("<td>" & drow("chvLastName").ToString() & "</td>")
                l_string_details.Append("</tr>")
            Next
            l_string_details.Append("</table>")
            'For Each drow In l_datatable_persondetails.Rows
            '    l_string_details.Append("Fund Id Number : " + drow("intFundIdNo").ToString() + ControlChars.CrLf + "First Name : " + drow("chvFirstName").ToString() + ControlChars.CrLf + "Last Name : " + drow("chvLastName").ToString() + ControlChars.CrLf + ControlChars.CrLf)
            'Next
            'End: Anudeep:24.12.2012 YRS 5.0-1717:Issue email when loan paid off 

            obj.MailCategory = "TDLoan"
            If obj.MailService = False Then Exit Sub
            obj.MailFormatType = Mail.MailFormat.Html

            obj.MailMessage = l_string_details.ToString()  'added by Hafiz on 26/07/2007
            obj.Subject = "The Tax Deferred Loan for the following participant has been repaid in full."
            obj.Send()
        Catch
            Throw
        End Try
    End Sub
    'Start: Bala: YRS-AT-2642: Following function is not required. Moved it to LoanClass()
    ''Added By Ashish 29-Jan-2009 ,Start
    'Private Sub SendLoanDefaultedMail(ByVal l_datatable_persondetails As DataTable)
    '    ' This method will be sent email if person defaluted for 
    '    Dim obj As New MailUtil
    '    Dim l_string_LoanDefaultedDetails As StringBuilder
    '    Dim drow As DataRow
    '    Try

    '        l_string_LoanDefaultedDetails = New StringBuilder
    '        For Each drow In l_datatable_persondetails.Rows
    '            l_string_LoanDefaultedDetails.Append("First Name : " + drow("chvFirstName").ToString() + ControlChars.CrLf + "Last Name : " + drow("chvLastName").ToString() + ControlChars.CrLf + "Ymca Name : " + drow("chvYmcaName").ToString() + ControlChars.CrLf + "Ymca Number : " + drow("chrYmcano").ToString() + ControlChars.CrLf + "Social Security No : " + drow("chrSSNo").ToString() + ControlChars.CrLf + "Fund Id Number : " + drow("intFundIdNo").ToString() + ControlChars.CrLf + ControlChars.CrLf)
    '        Next

    '        obj.MailCategory = "TDLoan_Defaulted"
    '        If obj.MailService = False Then Exit Sub

    '        obj.MailMessage = l_string_LoanDefaultedDetails.ToString()
    '        obj.Subject = "Tax Deferred Loan Defaulted."
    '        obj.Send()
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'End: Bala: YRS-AT-2642: Following function is not required. Moved it to LoanClass()
    'Added By Ashish 29-Jan-2009 ,End
    'Added By Ashish 17-July-2008 ,Start
    Private Sub SendServiceFailedMail(ByVal parameterLoggedId As Int64)
        Dim obj As MailUtil
        Dim l_str_msg As String
        Try
            If parameterLoggedId > 0 Then
                obj = New MailUtil
                obj.MailCategory = "ADMIN"
                If obj.MailService = False Then Exit Sub

                obj.MailMessage = "ServiceTime and Vesting update failed for BatchID :" + Convert.ToString(parameterLoggedId)

                obj.Subject = "ServiceTime and Vesting update failed."

                obj.Send()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Added By Ashish 17-July-2008 ,End
    'Added by Ashish 13-Jan-2009
    Private Sub RemoveUnSelectedYmcaTransmittals(ByVal paraDataSetAchImportData As DataSet, ByRef paraDataSetMatchedTransmittal As DataSet)
        Try
            If paraDataSetAchImportData.Tables(0).Rows.Count > 0 And paraDataSetMatchedTransmittal.Tables(0).Rows.Count > 0 Then
                For Each dtRowAchImportData As DataRow In paraDataSetAchImportData.Tables(0).Rows
                    Dim dtRowFindMatchedTransmittals As DataRow()
                    If dtRowAchImportData("Selected") = 1 Then

                        dtRowFindMatchedTransmittals = paraDataSetMatchedTransmittal.Tables(0).Select("YMCANo=" & dtRowAchImportData("YMCANo") & " and AchPaymentDate='" & Convert.ToDateTime(dtRowAchImportData("PaymentDate")).Date.ToString("MM/dd/yyyy") & "'")
                        If dtRowFindMatchedTransmittals.Length > 0 Then
                            Dim i As Int16
                            For i = 0 To dtRowFindMatchedTransmittals.Length - 1
                                dtRowFindMatchedTransmittals(i)("ValidRecord") = 1
                            Next


                        End If
                    Else
                        dtRowFindMatchedTransmittals = paraDataSetMatchedTransmittal.Tables(0).Select("YMCANo=" & dtRowAchImportData("YMCANo") & " and AchPaymentDate='" & Convert.ToDateTime(dtRowAchImportData("PaymentDate")).Date.ToString("MM/dd/yyyy") & "'")
                        If dtRowFindMatchedTransmittals.Length > 0 Then
                            Dim i As Int16
                            For i = dtRowFindMatchedTransmittals.Length - 1 To 0 Step -1
                                paraDataSetMatchedTransmittal.Tables(0).Rows.Remove(dtRowFindMatchedTransmittals(i))
                            Next

                        End If


                    End If
                    paraDataSetMatchedTransmittal.Tables(0).AcceptChanges()

                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "Events"
    Public Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim chkFlag As CheckBox = CType(sender, CheckBox)
            'Dim dbl_Total As Double
            Dim dt As DataTable
            Dim ds As DataSet
            ds = CType(Session("l_dsAchDebitMatchTransmittal"), DataSet)
            Dim dgItem As DataGridItem = CType(chkFlag.NamingContainer, DataGridItem)
            Dim i As Integer = dgItem.ItemIndex
            If chkFlag.Checked = True Then
                ds.Tables(0).Rows(i).Item("Selected") = 1

            Else
                ds.Tables(0).Rows(i).Item("Selected") = 0

            End If
            'Session("l_dsAchDebitMatchTransmittal") = ds


        Catch ex As Exception

            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonBack.Click
        Try
            Session("MatchReceipts") = True
            Session("BackMatchReceipts") = True
            Response.Redirect("ACHDebitImportForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Sub ButtonPostAndApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPostAndApply.Click
        Try

            Dim intTotalRecord As Int32
            Dim intRecordsSelected As Int32
            Dim minFundedDateRange As DateTime
            Dim maxFundedDateRange As DateTime
            Dim l_DateTimeFundedDate As DateTime
            'Added by Ashish on 02-Dec-2008 ,put validation funded date within business month ,Start
            Dim l_string_AcctDate As String
            Dim l_DateTime_AcctDate As DateTime
            Dim l_DateTime_CurrentDate As DateTime

            'Set Current Date
            l_DateTime_CurrentDate = System.DateTime.Now.Date
            'Get the accounting date 
            'l_ds_AcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
            'l_string_AcctDate = l_ds_AcctDate.Tables(0).Rows(0).Item(0).ToString()
            l_string_AcctDate = ViewState("CurrentAccountingDate")
            If Not l_string_AcctDate.Equals(String.Empty) Then
                l_DateTime_AcctDate = Convert.ToDateTime(l_string_AcctDate).Date
            Else
                l_DateTime_AcctDate = System.DateTime.Now.Date
            End If

            If (l_DateTime_AcctDate.Year = l_DateTime_CurrentDate.Year And l_DateTime_AcctDate.Month < l_DateTime_CurrentDate.Month) Or l_DateTime_AcctDate.Year < l_DateTime_CurrentDate.Year Then
                minFundedDateRange = l_DateTime_AcctDate.AddDays(-(l_DateTime_AcctDate.Day - 1))
                maxFundedDateRange = l_DateTime_AcctDate
            Else
                minFundedDateRange = l_DateTime_CurrentDate.AddDays(-(l_DateTime_CurrentDate.Day - 1))
                maxFundedDateRange = l_DateTime_CurrentDate

            End If

            ' minFundedDateRange = System.DateTime.Now.Date.AddDays(-(System.DateTime.Now.Date.Day - 1))
            'maxFundedDateRange = System.DateTime.Now.Date
            'Added by Ashish on 02-Dec-2008 ,put validation funded date within business month ,End
            If Not DateusercontrolFundedDate.Text.Trim() = "" Then
                l_DateTimeFundedDate = CType(DateusercontrolFundedDate.Text, DateTime)
                If l_DateTimeFundedDate < minFundedDateRange Or l_DateTimeFundedDate > maxFundedDateRange Then

                    'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "Invalid Funded date..Funded Date should be between first day of current month to today date.", MessageBoxButtons.Stop)
                    If minFundedDateRange.Month = l_DateTime_CurrentDate.Month Then
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "Invalid Funded date..Funded Date should be between first day of current business month to today's date.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Invalid Funded date..Funded Date should be between first day of current business month to today's date.", EnumMessageTypes.Error)
                    Else
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "Invalid Funded date..Funded Date should be within the current business month.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Invalid Funded date..Funded Date should be within the current business month.", EnumMessageTypes.Error)
                    End If
                    Session("PostAndApplyOk") = False
                    Session("IsDone") = False
                    Exit Sub
                End If
            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "Funded Date can not be blank.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Funded Date can not be blank.", EnumMessageTypes.Error)
                Session("PostAndApplyOk") = False
                Session("IsDone") = False
                Exit Sub
            End If

            'if 
            'If Not Session("MatchReceipts") = Nothing Then

            '    Session("MatchReceipts") = Nothing
            'End If
            If ValidateCheckBoxSelection(intTotalRecord, intRecordsSelected) Then
                'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
                'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "You have selected " & intRecordsSelected & "/" & intTotalRecord & " records.Do you wish to proceed?", MessageBoxButtons.YesNo)
                lblMessage.Text = "You have selected " & intRecordsSelected & "/" & intTotalRecord & " records.Do you wish to proceed?"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YESNO');", True)
                Session("PostAndApplyOk") = True
            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolderPostAndApply, "YMCA-YRS", "Select at least one record.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Select at least one record.", EnumMessageTypes.Error)
                Session("PostAndApplyOk") = False
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#End Region



    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try

            Session("strBatchId") = Nothing
            Session("BackMatchReceipts") = Nothing
            Session("MatchReceipts") = Nothing
            Session("l_dataset_ACHInsert") = Nothing
            Session("l_dsAchDebitMatchTransmittal") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception

            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DatagridACHDebitMatchReceipt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridACHDebitMatchReceipt.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                If e.Item.Cells(11).Text.Equals("0") Then
                    'e.Item.Visible = False
                    e.Item.ForeColor = Color.Red
                    e.Item.Style.Add("FontItemStype", "Bold")
                    Dim chkBox As CheckBox
                    chkBox = CType(e.Item.FindControl("CheckBoxSelect"), CheckBox)
                    If Not chkBox Is Nothing Then
                        chkBox.Enabled = False
                        chkBox.Checked = False
                    End If


                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'AA:16.12.2013 - BT:2311 Added Button yes and no button click event functionality.
    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)

            If Not Session("PostAndApplyOk") = Nothing Then
                If Session("PostAndApplyOk") = True Then
                    Session("PostAndApplyOk") = Nothing
                    AchDebitPostAndApplyReceipts()
                End If
            End If
            If (Session("IsDone") = True) Then
                Session("IsDone") = False
                'Response.Redirect("MainWebForm.aspx", False) 'AA:05.03.2016 YRS-AT-2830 Commented this line to start the copy initiate process of IDM 
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
