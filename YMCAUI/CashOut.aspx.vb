'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		            :	YMCA_YRS
' FileName			            :	CashOut.aspx.vb
' Author Name		            :	Aparna Samala
' Employee ID		            :	34773
' Email				            :	aparna.samala@icici-infotech.com    
' Contact No		            :	8609
' Creation Time		            :	21/11/2006 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session   
' Modified and Added by Shubhrata Tripathi Nov 28th 2006 
'*******************************************************************************
'****************************************************
'Modification History
'****************************************************
'Modified by            Date                Description
'****************************************************
'Aparna Samala          07/02/2007          YREN-3061 
'AParna Samala          13/12/2007          YREN-4181
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Sanjeev Gupta(SG)      2012.06.05          BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
'Sanjeev Gupta(SG)      2012.08.23          BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
'Priya Patil			23.10.2012			YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
'Priya Patil			08.11.2012			YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
'Sanjeev Gupta(SG)      2012.11.21          BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
'Priya Patil			23.11.2012			YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
'Priya					11/24/2012			YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
'Sanjeev Gupta(SG)      2012.11.26          BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
'Sanjeev Gupta(SG)      2012.12.31          BT-1511: Changes requested while perfoming demo to the client for BT-960: YRS 5.0-1489
'Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
'Sanjay Rawat           2014.04.02          BT 2298/YRS 5.0-2259 - Line of code commented to block summary report popup before creating batch.
'Dinesh Kanojia         2014.03.31          BT-2324: YRS 5.0-2267 - Changes to Cashout master page
'Dinesh Kanojia         2014.04.09          Reopen - BT-2324: YRS 5.0-2267 - Changes to Cashout master page
'Shashank Patel         2014.04.08          BT 2298/YRS 5.0-2259-Optimizing the Cash out process to improve its performance.
'Shashnk Patel          2014.10.07          BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS 
'Sanjay S.              2014.11.28          BT 2728: Cash out payment register mail shows wrong information
'Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
'Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
'Dinesh Kanojia         2015.06.15          BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Anudeep A              2015.10.21          YRS-AT-2463 - Cashout utility for participants with two plans. One release blank rather than two per participant (TrackIT 21783)
'Anudeep A              2016.03.07          YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
'Shilpa N               02/22/2019          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'********************************************************************************
Imports System.Web
Imports System.Data.SqlClient
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports System.IO
Imports System.Collections
Imports System.ComponentModel
Imports System.Threading
Imports System.Reflection
Imports System.Security
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Net
Imports System.Linq
Imports YMCAObjects
Imports YMCARET.CommonUtilities


Public Class CashOut
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("CashOut.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Commented by Dinesh Kanojia to incorporate master file
    'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonProcess As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonReport As System.Web.UI.WebControls.Button
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownAmount As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGridCashOut As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonGetData As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCashouts As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYmcaList As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCshoutsSelected As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTotalAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalAmtSelected As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonUpdateCounter As System.Web.UI.WebControls.Button
    Protected WithEvents LabelSelectedCashouts As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCashoutList As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSelectedAmt As System.Web.UI.WebControls.Label

    Protected WithEvents CashoutTabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents CashoutMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents GridViewCashoutBatch As System.Web.UI.WebControls.GridView
    Protected WithEvents LabelNoRecords As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNoBatch As System.Web.UI.WebControls.Label
    Protected WithEvents LinkEligibleList As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LinkNotEligibleList As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LinkProcessBatch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LinkUnprocessBatch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ButtonCreateBatch As System.Web.UI.WebControls.Button
    Protected WithEvents tdBatchPanel As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdParticipantPanel As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents ButtonOkBatch As System.Web.UI.WebControls.Button
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Protected WithEvents trColorMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tbUpdateCounter As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trBatchHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trListHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents linkPlain As System.Web.UI.WebControls.LinkButton
    Protected WithEvents linkHighlighted As System.Web.UI.WebControls.LinkButton
    Protected WithEvents linkAll As System.Web.UI.WebControls.LinkButton
    Protected WithEvents buttonFilter As System.Web.UI.WebControls.Image
    Protected WithEvents labelFilter As System.Web.UI.WebControls.Label
    Protected WithEvents btnOk As System.Web.UI.WebControls.Button

    Protected WithEvents lblConfirmMessage As System.Web.UI.WebControls.Label
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button

    'Commented by Dinesh Kanojia to incorporate master file
    'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
    'Protected WithEvents labelBatchID As System.Web.UI.WebControls.Label

    'Protected WithEvents chkSelectAll As System.Web.UI.WebControls.CheckBox


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Global Declaration"
    'Added by Shubhrata Nov 28th 2006
    Dim objCashOutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass
    Dim g_String_Exception_Message As String
    Dim g_dataset_Cashouts As New DataSet
    Dim dtFileList As New DataTable

#End Region

#Region "Properties"
    Private Property Session_datatable_DropDownData() As DataTable
        Get
            If Not (Session("g_datatable_DropDownData")) Is Nothing Then

                Return (CType(Session("g_datatable_DropDownData"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DropDownData") = Value
        End Set
    End Property


    Private Property Session_DataSet_Cashouts() As DataSet
        Get
            If Not (Session("g_DataSet_Cashouts")) Is Nothing Then

                Return (CType(Session("g_DataSet_Cashouts"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("g_DataSet_Cashouts") = Value
        End Set
    End Property
    'aparna yren-3016
    Private Property TotalAmount() As Decimal
        Get
            If Not Session("TotalAmount") Is Nothing Then
                Return (CType(Session("TotalAmount"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAmount") = Value
        End Set
    End Property
    Private Property TotalAmountSelected() As Decimal
        Get
            If Not Session("TotalAmountSelected") Is Nothing Then
                Return (CType(Session("TotalAmountSelected"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAmountSelected") = Value
        End Set
    End Property

    Private Property SelectedCashouts() As Integer
        Get
            If Not Session("SelectedCashouts") Is Nothing Then
                Return (CType(Session("SelectedCashouts"), String))
            Else
                Return 0
            End If

        End Get
        Set(ByVal Value As Integer)
            Session("SelectedCashouts") = Value
        End Set
    End Property

    Private Property TotalNoOfCashouts() As Integer
        Get
            If Not Session("TotalNoOfCashouts") Is Nothing Then
                Return (CType(Session("TotalNoOfCashouts"), String))
            Else
                Return 0
            End If

        End Get
        Set(ByVal Value As Integer)
            Session("TotalNoOfCashouts") = Value
        End Set
    End Property
    'aparna yren-3016


    Private Property SSNO() As Decimal
        Get
            If Not Session("SSNO") Is Nothing Then
                Return (CType(Session("SSNO"), String))
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SSNO") = Value
        End Set
    End Property

    Private Property DropdownValue() As String
        Get
            If Not Session("DropdownValue") Is Nothing Then
                Return (CType(Session("DropdownValue"), String))
            Else
                Return ""
            End If
        End Get

        Set(ByVal Value As String)
            Session("DropdownValue") = Value
        End Set
    End Property

    'Added By SG: 2012.06.05: BT-960
    Enum ProcessState
        Request
        Process
    End Enum

    'Added By SG: 2012.09.24: BT-960
    Enum CashoutRange
        Range1
        Range2
    End Enum

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("CashOut Page load", "Page Load Call.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            HttpContext.Current.Session("iProcessCount") = "Initiate"
            If Not IsPostBack Then
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                Else
                    'Get the UserId and Ipaddress for the cashoutlog table
                    Session("CashOutRangeDesc") = Nothing
                    'Session("CashoutBatchId") = Nothing
                    Session("CashOutReqType") = Nothing
                    'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                    Session("PrintBatchID") = Nothing
                    'Session("printBatchDdata") = Nothing
                    'END Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 

                    'Added By SG: 2012.11.21: BT-960
                    Session("DiscardBatch") = Nothing
                    'Added By SG: 2012.11.21: BT-960

                    ' Session("UserId") = Session("LoggedUserKey")
                    Session("IPAddress") = Dns.GetHostName()
                    'Binding the Dropdown values
                    BindDropDown()

                    'Session("dtSelectedBatchRecords") = Nothing



                    If Not Session("Cashoutprocess") Is Nothing Then
                        If Convert.ToBoolean(Session("Cashoutprocess")) Then
                            DropDownAmount.SelectedValue = CashoutRange.Range2.ToString()
                            Me.CashoutMultiPage.SelectedIndex = 0
                            Me.CashoutTabStrip.SelectedIndex = 0
                            DropDownAmount_SelectedIndexChanged(sender, e)
                            If Not Request.QueryString("batchId") Is Nothing Then
                                HelperFunctions.ShowMessageToUser("CashOut Batch ID: " + Request.QueryString("batchId") + " processed successfully.", EnumMessageTypes.Success)
                            End If
                            'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                            'Session("CashoutBatchId") = Nothing
                        End If
                        Session("CashoutBatchId") = Request.QueryString("batchId")
                        If Not Session("Under$50CashoutStandardLetter") Is Nothing Then
                            If Session("Under$50CashoutStandardLetter") = "Under$50CashoutStandardLetter" Then
                                If Not Session("FTFileList") Is Nothing Then
                                    DropDownAmount.SelectedValue = CashoutRange.Range1.ToString()
                                    'Session("CashoutBatchId") = Request.QueryString("batchId")
                                    BindRangeDetails()
                                    Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                                                    "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                                                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                                                    "</script>"
                                    If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                                        Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                                    End If

                                    Session("strReportName_1") = "Cash Out"
                                    Dim popupScript3 As String = "<script language='javascript'>" & _
                                                    "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp22', " & _
                                                    "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                                                    "</script>"
                                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                                        Page.RegisterStartupScript("PopupScript22", popupScript3)
                                    End If
                                    'Me.CashoutMultiPage.SelectedIndex = 0
                                End If
                            End If
                            Session("FTFileList") = Nothing
                            Session("Under$50CashoutStandardLetter") = Nothing
                        End If

                        If Not Session("50.1to5000") Is Nothing Then
                            If Session("50.1to5000") = "50.1to5000" Then
                                Session("strReportName_1") = "Cash Out"
                                Dim popupScript3 As String = "<script language='javascript'>" & _
                                                "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp22', " & _
                                                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                                                "</script>"
                                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                                    Page.RegisterStartupScript("PopupScript22", popupScript3)
                                End If
                            End If
                            Session("50.1to5000") = Nothing
                        End If

                        Session("Cashoutprocess") = Nothing
                        Session("dtSelectedBatchRecords") = Nothing
                        'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                    Else

                        'ButtonProcess.Attributes.Add("onclick", "javascript:return Disable();")
                        Me.ButtonReport.Enabled = False
                        'aparna yren -3016
                        Me.SelectedCashouts = 0
                        Me.TotalNoOfCashouts = 0
                        Me.TotalAmountSelected = 0.0
                        Me.TotalAmount = 0.0
                        Me.LabelCashouts.Visible = False
                        Me.LabelSelectedCashouts.Visible = False
                        Me.LabelTotal.Visible = False
                        Me.LabelSelectedAmt.Visible = False
                        Me.TextBoxCashoutList.Visible = False
                        Me.TextBoxCshoutsSelected.Visible = False
                        Me.TextboxTotalAmt.Visible = False
                        Me.TextboxTotalAmtSelected.Visible = False
                        'aparna yren -3016

                        'Commented by Dinesh Kanojia to incorporate master file
                        'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                        'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                        'Me.Menu1.DataBind()

                        'CashoutTabStrip.Items.Item(1).Enabled = False
                        Me.CashoutMultiPage.SelectedIndex = 1

                        'Added By SG: 2012.11.26: To disable buttonprocess at the time of creating and processing batch.
                        'ButtonProcess.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(ButtonProcess, "").ToString())
                        ButtonCreateBatch.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(ButtonCreateBatch, "").ToString())
                        ButtonGetData.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(ButtonGetData, "").ToString())

                        'Start : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                        'Below Line will get execute once all batch request will get generated sucessfully from modal popup window.
                        If Not Session("Process") Is Nothing Then
                            If Session("Process") = "complete" Then
                                Session("strReportName_1") = "Cash Out"
                                Session("PrintBatchID") = Session("strBatchId")
                                HelperFunctions.ShowMessageToUser("CashOut Batch ID: " + CType(Session("strBatchId"), String).Trim() + " created successfully.", EnumMessageTypes.Success)

                                Dim popupScript3 As String = "<script language='javascript'>" & _
                               "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp1', " & _
                               "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                               "</script>"
                                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                                    Page.RegisterStartupScript("PopupScript2", popupScript3)
                                End If


                                Dim popupScript4 As String = "<script language='javascript'>" & _
                               "window.open('FT\\ReportViewer.aspx', 'ReportPopUp2', " & _
                               "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                               "</script>"
                                If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
                                    Page.RegisterStartupScript("PopupScript3", popupScript4)
                                End If

                                Me.LinkEligibleList.Visible = False
                                Me.LinkNotEligibleList.Visible = False
                                'Commented by Dinesh Kanojia to incorporate master file
                                'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                                'labelBatchID.Text = ""
                                Me.trBatchHeader.Visible = False
                                ButtonProcess.Enabled = False
                                labelFilter.Text = ""
                                Session("SegregateDatatable") = Nothing
                                Me.ClearData()
                                Me.DropDownAmount.SelectedValue = Session("ReqType").ToString()
                                If Me.DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then
                                    BindBatchGrid()
                                End If
                                BindRangeDetails()
                                Session("g_bool_ProcessOK") = Nothing
                                Me.LabelNoRecords.Text = "No Records Found"
                                Me.LabelNoRecords.Visible = False
                                Session("strReportName_1") = "Cash Out"
                                Session("Process") = Nothing
                                Session("PrintBatchID") = Session("strBatchId")
                            End If
                        End If
                    End If
                    'End: Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                End If
            End If

            If IsPostBack Then
                If Request.Form("Ok") = "OK" Then
                    If Not Session("g_bool_ProcessOK") Is Nothing Then
                        If Session("g_bool_ProcessOK") = True Then
                            Me.ClearData()
                            'Added By SG: 2012.12.18: BT-1511:
                            If Me.DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then
                                BindBatchGrid()
                            End If

                            BindRangeDetails()
                            'Me.CashoutTabStrip.Visible = False

                            Session("g_bool_ProcessOK") = Nothing
                            'START: Added By SG: BT-960
                            Me.LabelNoRecords.Text = "No Records Found"
                            Me.LabelNoRecords.Visible = False
                            'END: Added By SG: BT-960
                        End If
                    End If
                End If
            End If


        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub

    '<System.Web.Services.WebMethod()> _
    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        Try
            SelectAll()

        Catch
            Throw
            'GetCatch(ex)
        End Try
    End Sub

    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
    '    If Not Session("AllFilesCreated") Is Nothing Then
    '        If Convert.ToBoolean(Session("AllFilesCreated")) Then
    '            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('modalcashout')", True)
    '            Me.LinkEligibleList.Visible = False
    '            Me.LinkNotEligibleList.Visible = False
    '            'Commented by Dinesh Kanojia to incorporate master file
    '            'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
    '            'labelBatchID.Text = ""
    '            Me.trBatchHeader.Visible = False
    '            'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
    '            'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cash Out Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + "<BR> Processed Successfully.", MessageBoxButtons.OK)
    '            HelperFunctions.ShowMessageToUser("CashOut Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + " processed successfully.", EnumMessageTypes.Success)
    '            Session("g_bool_ProcessOK") = True
    '            ButtonProcess.Enabled = False
    '            labelFilter.Text = ""
    '            Session("SegregateDatatable") = Nothing
    '            Session("strReportName_1") = "Cash Out" 'SP 2014.04.08 BT 2298/YRS 5.0-2259-
    '        End If
    '    End If
    'End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog()", True)
        Me.LinkEligibleList.Visible = False
        Me.LinkNotEligibleList.Visible = False
        'Commented by Dinesh Kanojia to incorporate master file
        'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
        'labelBatchID.Text = ""
        Me.trBatchHeader.Visible = False
        'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
        'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cash Out Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + "<BR> Processed Successfully.", MessageBoxButtons.OK)
        ' HelperFunctions.ShowMessageToUser("CashOut Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + " created successfully.", EnumMessageTypes.Success)

        HelperFunctions.ShowMessageToUser("CashOut Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + " created successfully.", EnumMessageTypes.Success)
        Session("g_bool_ProcessOK") = True
        ButtonProcess.Enabled = False
        labelFilter.Text = ""
        Session("SegregateDatatable") = Nothing
        'Session("Process") = "complete"
        Session("strReportName_1") = "Cash Out" 'SP 2014.04.08 BT 2298/YRS 5.0-2259-

        Dim BatchID As String = String.Empty
        BatchID = Session("strBatchId") 'This code is aded by dines for YRS 5.0-2315
        Session("PrintBatchID") = BatchID
        Session("CashoutBatchId") = BatchID
        If (Not Session("PrintBatchID") Is Nothing AndAlso Session("PrintBatchID").ToString().Length > 1) Then
            'Start:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
            'Session("strReportName") = "Withdrawals_New"
            Session("strReportName") = "New Cashout Letter"
            'End:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
            Session("strModuleName") = "CashOut"
            ' Me.PrintReport()


            ' Dim popupScript3 As String = "<script language='javascript'>" & _
            '                        "window.open('FT\\ReportViewer_1.aspx', 'popupScript3', " & _
            '                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            '                        "</script>"
            ' If (Not Me.IsStartupScriptRegistered("popupScript3")) Then
            '     Page.RegisterStartupScript("popupScript3", popupScript3)
            ' End If


            ' Dim popupScript4 As String = "<script language='javascript'>" & _
            '"window.open('FT\\ReportViewer.aspx', 'popupScript4', " & _
            '"'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            '"</script>"
            ' If (Not Me.IsStartupScriptRegistered("popupScript4")) Then
            '     Page.RegisterStartupScript("popupScript4", popupScript4)
            ' End If

            Me.LinkEligibleList.Visible = False
            Me.LinkNotEligibleList.Visible = False
            'Commented by Dinesh Kanojia to incorporate master file
            'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
            'labelBatchID.Text = ""
            Me.trBatchHeader.Visible = False
            ButtonProcess.Enabled = False
            labelFilter.Text = ""
            Session("SegregateDatatable") = Nothing
            Me.ClearData()
            Me.DropDownAmount.SelectedValue = Session("ReqType").ToString()
            If Me.DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then
                BindBatchGrid()
            End If
            BindRangeDetails()
            Session("g_bool_ProcessOK") = Nothing
            Me.LabelNoRecords.Text = "No Records Found"
            Me.LabelNoRecords.Visible = False
            Session("strReportName_1") = "Cash Out"
            Session("Process") = "complete"

        End If
    End Sub

    Private Sub ButtonReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReport.Click
        Try
            ' MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cashouts Report Pending", MessageBoxButtons.Stop)
            ' Exit Sub
            'aparna yren-3016

            'Added By SG: 2012.06.14: BT-960
            'CashoutLogDataFromGrid(False)
            If Not Session("CashOutReqType") Is Nothing AndAlso Session("CashOutReqType").ToString() = ProcessState.Process.ToString() Then
                CashoutLogDataFromGrid(True, Session("CashOutReqType").ToString())
            Else
                CashoutLogDataFromGrid(False, Nothing)
            End If


            'Opening Report
            OpenReportViewer()

            Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                                  "ShowHideButton('none')</" & "script" & ">"
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript21", popupScript3)
            End If

        Catch
            Throw
            ' GetCatch(ex)
        End Try




    End Sub
    <System.Web.Services.WebMethod()> _
    Public Shared Function CashOutBatchCreationProcess(ByVal strBatchId As String, ByVal strReportType As String, ByVal strCashOutType As String, ByVal iCount As Integer, ByVal strProcessName As String, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim strretval As String = String.Empty
        Dim dsTemp As DataSet
        Dim objReturnStatusValues = New ReturnStatusValues()
        Dim objCashOut As New CashOut

        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Cashout batch creation process")
        'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        If HttpContext.Current.Cache(strBatchId) Is Nothing Then
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Pull data from batch creation log table")
            dsTemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Finish: Pull data from batch creation log table")
        Else
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Pull data from cache to process batch creation request")
            dsTemp = HttpContext.Current.Cache(strBatchId)
        End If

        'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        objReturnStatusValues = objCashOut.Process(strBatchId, strReportType, strCashOutType, iCount, strProcessName, dsTemp, iIDXCreated, iPDFCreated, strModule)
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Pull data from cache")
        YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)
        'Start:Dinesh Kanojia        2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        HttpContext.Current.Cache(strBatchId) = dsTemp
        If HelperFunctions.isNonEmpty(dsTemp.Tables("dtFileList")) Then
            HttpContext.Current.Session("FTFileList") = dsTemp.Tables("dtFileList")
        End If
        'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Finish: Cashout batch creation process")
        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Return objReturnStatusValues

    End Function
    'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
    ''' <summary>
    ''' GetBatchSize will get the batch size for batch creation process.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBatchSize() As Integer
        Dim dsbatchSize As DataSet
        Dim intBatchsize As Integer
        Try
            dsbatchSize = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("BATCH_SIZE")
            If HelperFunctions.isNonEmpty(dsbatchSize) Then
                intBatchsize = dsbatchSize.Tables(0).Rows(0)("Value")
            Else
                intBatchsize = 2
            End If
            Return intBatchsize
        Catch
            Throw
        End Try
    End Function
    'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
    Private Function Process(ByVal strBatchId As String, ByVal strReportType As String, ByVal strCashOutType As String, ByVal iCount As Integer, ByVal strProcessName As String, dsTemp As DataSet, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim blnSuccess As Boolean = True
        Dim strProgressStatus As String = String.Empty
        Dim dtFileListSource As New DataTable
        Dim strVal As String = String.Empty
        Dim i As Integer = 0
        Dim strReturn As String = String.Empty
        Dim drtemp As DataRow
        Dim objReturnStatusValues As New ReturnStatusValues
        Dim dtSelectedBatchRecords As New DataTable
        Dim objBatchProcess As New BatchRequestCreation
        Dim ArrErrorDataList = New List(Of ExceptionLog)
        Dim dtFileList As New DataTable
        Dim strBatchError As String
        Dim l_bool_status As Boolean = False
        Dim dttemp As New DataTable
        'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        'get the batch size to create batch request, default will be 2.
        Dim BatchSize As Integer = GetBatchSize()
        'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        Dim iProcessCount As Integer = 0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Cashout batch creation process")
            objReturnStatusValues.strBatchId = strBatchId
            objReturnStatusValues.strReportType = strReportType
            objReturnStatusValues.iProcessCount = iCount
            objReturnStatusValues.strretValue = "pending"
            objReturnStatusValues.iIdxCreated = iIDXCreated
            objReturnStatusValues.iPdfCreated = iPDFCreated

            dtSelectedBatchRecords = dsTemp.Tables("SelectedBatchRecords")


            Dim dtArrErrorDataList As DataTable = dsTemp.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                For Each dr As DataRow In dtArrErrorDataList.Rows
                    ArrErrorDataList.Add(New ExceptionLog(dr("FundNo"), dr("Errors"), dr("Description")))
                Next
            End If

            If Not dtSelectedBatchRecords.Columns.Contains("IsReportPrinted") Then
                dtSelectedBatchRecords.Columns.Add("IsReportPrinted")
            End If


            'Get no. of records to create refund request and further for IDX and PDF files generation as per batch size. 
            dttemp = dtSelectedBatchRecords.Clone
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Start: Add participant in temp table.")
            For iProcessCount = iCount To dtSelectedBatchRecords.Rows.Count - 1
                If iProcessCount - iCount >= BatchSize Then
                    Exit For
                End If
                drtemp = dttemp.NewRow
                drtemp("PersonId") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonId").ToString
                drtemp("SSNO") = dtSelectedBatchRecords.Rows(iProcessCount)("SSNO").ToString
                drtemp("FUNDNo") = dtSelectedBatchRecords.Rows(iProcessCount)("FUNDNo").ToString
                drtemp("FirstName") = dtSelectedBatchRecords.Rows(iProcessCount)("FirstName").ToString
                drtemp("LastName") = dtSelectedBatchRecords.Rows(iProcessCount)("LastName").ToString
                drtemp("MiddleName") = dtSelectedBatchRecords.Rows(iProcessCount)("MiddleName").ToString
                drtemp("Name") = dtSelectedBatchRecords.Rows(iProcessCount)("Name").ToString
                drtemp("PersonAgeDOB") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonAgeDOB").ToString
                drtemp("MaxTermDate") = dtSelectedBatchRecords.Rows(iProcessCount)("MaxTermDate").ToString
                drtemp("FundEventId") = dtSelectedBatchRecords.Rows(iProcessCount)("FundEventId").ToString
                drtemp("IsTerminated") = dtSelectedBatchRecords.Rows(iProcessCount)("IsTerminated").ToString
                drtemp("IsVested") = dtSelectedBatchRecords.Rows(iProcessCount)("IsVested").ToString
                drtemp("IntAddressId") = dtSelectedBatchRecords.Rows(iProcessCount)("IntAddressId").ToString
                drtemp("EligibleBalance") = dtSelectedBatchRecords.Rows(iProcessCount)("EligibleBalance").ToString
                drtemp("TaxableAmount") = dtSelectedBatchRecords.Rows(iProcessCount)("TaxableAmount").ToString
                drtemp("StatusType") = dtSelectedBatchRecords.Rows(iProcessCount)("StatusType").ToString
                drtemp("PlansType") = dtSelectedBatchRecords.Rows(iProcessCount)("PlansType").ToString
                drtemp("BatchId") = dtSelectedBatchRecords.Rows(iProcessCount)("BatchId").ToString
                drtemp("Selected") = dtSelectedBatchRecords.Rows(iProcessCount)("Selected").ToString
                drtemp("LastContributionDate") = dtSelectedBatchRecords.Rows(iProcessCount)("LastContributionDate").ToString
                drtemp("IsHighlighted") = dtSelectedBatchRecords.Rows(iProcessCount)("IsHighlighted").ToString
                drtemp("RefRequestID") = dtSelectedBatchRecords.Rows(iProcessCount)("RefRequestID").ToString
                drtemp("IsRMDEligible") = dtSelectedBatchRecords.Rows(iProcessCount)("IsRMDEligible").ToString
                drtemp("chvShortDescription") = dtSelectedBatchRecords.Rows(iProcessCount)("chvShortDescription").ToString
                drtemp("Remarks") = dtSelectedBatchRecords.Rows(iProcessCount)("Remarks").ToString
                drtemp("mnyEstimatedBalance") = dtSelectedBatchRecords.Rows(iProcessCount)("mnyEstimatedBalance").ToString
                drtemp("IsReportPrinted") = "1"
                dtSelectedBatchRecords.Rows(iProcessCount)("IsReportPrinted") = "1"
                dttemp.Rows.Add(drtemp)
            Next
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Finish: Add participant in temp table.")
            If Not dttemp Is Nothing Then
                'Create refund request for selected records.
                objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Start: Creation of Refund request")
                objCashOutBOClass.CreateAndProcessRequest(dttemp, l_bool_status, strCashOutType)
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "End: Creation of Refund request")
            End If

            If HelperFunctions.isNonEmpty(dttemp) Then
                Dim l_stringDocType As String = String.Empty
                Dim l_StringReportName As String = String.Empty
                Dim l_string_OutputFileType As String = String.Empty

                l_stringDocType = "REFREQST"
                'Start:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                'l_StringReportName = "Withdrawals_New.rpt"
                l_StringReportName = "New Cashout Letter.rpt"
                'End:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                l_string_OutputFileType = "Withdrawal_REFREQST_" + l_stringDocType

                'Proceed records for IDX and PDF files creation.
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Start: Reuqest to create IDX and PDF files generation")
                objBatchProcess.InvokeBatchRequestCreation(0, dttemp, l_stringDocType, l_StringReportName, l_string_OutputFileType, strReportType, ArrErrorDataList, dtFileList, iIDXCreated, iPDFCreated)
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "End: Reuqest to create IDX and PDF files generation")
            End If

            If dtSelectedBatchRecords.Rows.Count > objReturnStatusValues.iProcessCount + BatchSize Then
                objReturnStatusValues.iProcessCount += BatchSize

                If HelperFunctions.isNonEmpty(dtFileList) Then
                    If Not dsTemp.Tables.Contains("dtFileList") Then
                        dtFileList.TableName = "dtFileList"
                        dsTemp.Tables.Add(dtFileList)
                    End If
                    dsTemp.Tables("dtFileList").Merge(dtFileList)
                End If
                Return objReturnStatusValues
            End If

            'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
            If objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count Then
                objReturnStatusValues.strretValue = "success"
                HttpContext.Current.Cache.Remove(strBatchId)
            Else
                objReturnStatusValues.strretValue = "pending"
            End If

            objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count
            'objReturnStatusValues.strretValue = "success"
            'End:Dinesh Kanojia         2014.12.09         BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)

            If HelperFunctions.isNonEmpty(dtFileList) Then
                If Not dsTemp.Tables.Contains("dtFileList") Then
                    dtFileList.TableName = "dtFileList"
                    dsTemp.Tables.Add(dtFileList)
                End If
                dsTemp.Tables("dtFileList").Merge(dtFileList)
            End If


        Catch ex As Exception
            HelperFunctions.LogException("Process Cashout Batch Creation.", ex)
            objReturnStatusValues.strretValue = "error"
            ArrErrorDataList.Add(New ExceptionLog("Process Exception", "Process Cashout Batch Creation.", ex.Message))
            Return objReturnStatusValues
            Throw ex
        Finally
            Dim dtArrErrorDataList As New DataTable("ArrErrorDataList")
            dtArrErrorDataList.Columns.Add("FundNo")
            dtArrErrorDataList.Columns.Add("Errors")
            dtArrErrorDataList.Columns.Add("Description")
            Dim dr As DataRow
            For Each exlog As ExceptionLog In ArrErrorDataList
                dr = dtArrErrorDataList.NewRow()
                dr("FundNo") = exlog.FundNo
                dr("Errors") = exlog.Errors
                dr("Description") = exlog.Decription
                dtArrErrorDataList.Rows.Add(dr)
            Next
            If dsTemp.Tables.Contains("ArrErrorDataList") Then
                dsTemp.Tables.Remove("ArrErrorDataList")
            End If
            dtArrErrorDataList.TableName = "ArrErrorDataList"
            dsTemp.Tables.Add(dtArrErrorDataList)

            'If HelperFunctions.isNonEmpty(dtFileList) Then
            '    If Not dsTemp.Tables.Contains("dtFileList") Then
            '        dtFileList.TableName = "dtFileList"
            '        dsTemp.Tables.Add(dtFileList)
            '    End If
            '    dsTemp.Tables("dtFileList").Merge(dtFileList)
            'End If

            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                Dim dv As New DataView(dtSelectedBatchRecords)
                dv.RowFilter = "IsReportPrinted = 1"
                objReturnStatusValues.iTotalCount = dtSelectedBatchRecords.Rows.Count
                objReturnStatusValues.iIdxCreated = iIDXCreated
                objReturnStatusValues.iPdfCreated = iPDFCreated
                objReturnStatusValues.iTotalIDXPDFCount = dv.Count
            End If
        End Try
        Return objReturnStatusValues
    End Function


    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Dim strBatchId As String = String.Empty
        Dim strReportProcessName As String
        Dim dsCashOutTemp As New DataSet
        Dim dt As New DataTable
        Dim strCashOutType As String
        Dim XmlSelectedPersDetails As String  'SP 2014.10.07 BT-2633\YRS 5.0-2403
        Try
            If Not Session("dtSelectedBatchRecords") Is Nothing Then

                Me.LinkEligibleList.Visible = False
                Me.LinkNotEligibleList.Visible = False
                Me.trBatchHeader.Visible = False
                Session("g_bool_ProcessOK") = True
                ButtonProcess.Enabled = False
                If Me.DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then
                    BindBatchGrid()
                End If

                BindRangeDetails()
                labelFilter.Text = ""
                Session("SegregateDatatable") = Nothing

                'SP 2014.10.07 BT-2633\YRS 5.0-2403 -Start
                XmlSelectedPersDetails = GetXmlPerDetailsFromDataSet(Session("dtSelectedBatchRecords"))
                If Not String.IsNullOrEmpty(XmlSelectedPersDetails) Then
                    ExpiredRefRequestsForSelectedPerson(XmlSelectedPersDetails)
                End If
                'SP 2014.10.07 BT-2633\YRS 5.0-2403 -End

                Dim strModule As String = "CashOutBatchCreation"
                strBatchId = CType(Session("dtSelectedBatchRecords"), DataTable).Rows(0)("BatchId")
                dt = CType(Session("dtSelectedBatchRecords"), DataTable)
                dt.TableName = "SelectedBatchRecords"
                dsCashOutTemp.Tables.Add(dt)
                strReportProcessName = "BIE"
                strCashOutType = Session("CashOutReqType").ToString()
                YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsCashOutTemp)
                Session("strBatchId") = strBatchId
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "CashOutBatchCreation", "CallProcess('" + strBatchId + "', '" + strReportProcessName + "','" + strModule + "','" + strCashOutType + "');", True)
            Else
                Throw New Exception("Records not selected for batch creation.")
                HelperFunctions.ShowMessageToUser("Please select the person(s) to create a batch.", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnYes_Click", ex)
        End Try
    End Sub

    Private Sub ButtonProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonProcess.Click
        Dim g_dataset_Report As New DataSet
        Dim dtSelectedBatchRecords As New DataTable
        Dim l_string_Message As String
        Dim l_IDMDatable As New DataTable
        'Shubhrata
        Dim l_bool_status As Boolean = False
        Dim XmlSelectedPersDetails 'SP 2014.10.07 BT-2633\YRS 5.0-2403
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(sender.ID.ToString(), Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'START: Added By SG: BT-960
            If Not Session("DiscardBatch") Is Nothing AndAlso Session("DiscardBatch").ToString() = "True" AndAlso Not Session("CashoutBatchId") Is Nothing Then
                'Call to BO layer for processing the requests.
                objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass

                objCashOutBOClass.CancelPendingCashoutRequest(Session("CashoutBatchId"), "CASOUT")

                Session("DiscardBatch") = Nothing
                Me.LinkEligibleList.Visible = False
                Me.LinkNotEligibleList.Visible = False

                'Me.tdParticipantPanel.InnerText = ""
                'Commented by Dinesh Kanojia to incorporate master file
                'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                'labelBatchID.Text = ""

                Me.trBatchHeader.Visible = False
                'Me.trListHeader.Visible = False
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cash Out Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + "<BR> Processed Successfully.", MessageBoxButtons.OK)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser("CashOut Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + " processed successfully.", EnumMessageTypes.Success)
                Session("g_bool_ProcessOK") = True
                ButtonProcess.Enabled = False
                Exit Sub 'END: Added By SG: BT-960
            ElseIf DataGridCashOut Is Nothing Or DataGridCashOut.Items.Count = 0 Then
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "No Item is present to Proceed", MessageBoxButtons.OK)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser("No item(s) present to proceed.", EnumMessageTypes.Information)
                Exit Sub
            End If

            'START: Added By SG: 2012.06.05: BT-960
            Dim l_string_CashOutReqType As String

            If Not Session("CashOutReqType") Is Nothing Then
                l_string_CashOutReqType = Session("CashOutReqType").ToString()
                Session("ReqType") = CashoutRange.Range2.ToString()
            End If

            CashoutLogDataFromGrid(True, l_string_CashOutReqType)
            dtSelectedBatchRecords = Session("dtSelectedBatchRecords")
            'END: Added By SG: 2012.06.05: BT-960

            'Looping through Grid and fetching selected rows
            If dtSelectedBatchRecords.Rows.Count > 0 Then
                'aparna 22/01/2007
                'Calling BO Class to Process the selected records
                Try
                    Dim l_Str_SelectedCashouts As Integer
                    Dim l_dgitem As DataGridItem
                    Dim l_checkbox As New CheckBox

                    Const EligibleBalance As Integer = 10
                    Me.TotalAmountSelected = 0.0
                    Me.SelectedCashouts = 0
                    For Each l_dgitem In Me.DataGridCashOut.Items
                        l_checkbox = l_dgitem.FindControl("CheckBoxSelect")
                        If l_checkbox.Checked Then
                            Me.TotalAmountSelected += CType(l_dgitem.Cells(EligibleBalance).Text, Decimal)
                            Me.SelectedCashouts += 1
                        End If
                    Next
                    If Me.TextBoxCshoutsSelected.Text <> Me.SelectedCashouts.ToString() Then
                        'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "There is a mismatch on the cash out selected in list and in number of cash out selected, Please verify. Click update counter button to avoid this mismatch before the Process.", MessageBoxButtons.Stop)
                        'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                        HelperFunctions.ShowMessageToUser("There is a mismatch between cashout(s) selected and updated counter, please click on the Update Counter to proceed further.", EnumMessageTypes.Error)
                        Exit Sub
                    End If
                    'Added By SG: BT-960
                    'objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
                    'objCashOutBOClass.CreateAndProcessRequest(dtSelectedBatchRecords, l_bool_status)

                    If objCashOutBOClass Is Nothing Then
                        'Call to BO layer for processing the requests.
                        objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
                    End If

                    If DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then

                        'Dim l_Str_SelectedCashouts As Integer
                        'Dim l_dgitem As DataGridItem
                        'Dim l_checkbox As New CheckBox

                        'Const EligibleBalance As Integer = 10
                        'Me.TotalAmountSelected = 0.0
                        'Me.SelectedCashouts = 0
                        'For Each l_dgitem In Me.DataGridCashOut.Items
                        '	l_checkbox = l_dgitem.FindControl("CheckBoxSelect")
                        '	If l_checkbox.Checked Then
                        '		Me.TotalAmountSelected += CType(l_dgitem.Cells(EligibleBalance).Text, Decimal)
                        '		Me.SelectedCashouts += 1
                        '	End If
                        'Next
                        'If Me.TextBoxCshoutsSelected.Text <> Me.SelectedCashouts.ToString() Then
                        '	MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "There is a mismach on the cash out selected in list and in number of cash out selected, Please verify. Click update counter button to avoid this mismatch before the Process.", MessageBoxButtons.Stop)
                        '	Exit Sub
                        'End If
                        'Me.TextboxTotalAmtSelected.Text = "$" + Me.TotalAmountSelected.ToString()
                        'Me.TextBoxCshoutsSelected.Text = Me.SelectedCashouts.ToString()
                        'Me.TextboxTotalAmt.Text = "$" + Me.TotalAmount.ToString()
                        'Me.TextBoxCashoutList.Text = Me.TotalNoOfCashouts.ToString()

                        'Start : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                        'objCashOutBOClass.CreateAndProcessRequest(dtSelectedBatchRecords, l_bool_status, l_string_CashOutReqType)
                        'All Selected data from gridview is been stored in below session which can be used for batch creation in pop up window
                        Session("dtSelectedBatchRecords") = dtSelectedBatchRecords

                        Session("CashoutBatchId") = CType(Session("dtSelectedBatchRecords"), DataTable).Rows(0)("BatchId")
                        'End : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                        'Added By SG: 2012.09.03: BT-960
                        l_bool_status = True
                        If l_string_CashOutReqType = ProcessState.Request.ToString() AndAlso l_bool_status = True Then
                            'Start : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                            'Below line will open modal pop window to generate batch request creation.

                            'New Batch Process

                            'If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "CallPopup", "showPanel('modalcashout');", True)
                            'End If

                            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                                If dtSelectedBatchRecords.Rows.Count > 0 Then
                                    lblConfirmMessage.Text = "Are you sure you want to create a batch for selected " + dtSelectedBatchRecords.Rows.Count.ToString() + " person(s)"
                                    'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showConfirmdialog();", True)
                                    ScriptManager.RegisterStartupScript(Me, GetType(Page), "Open", "showConfirmdialog();", True)
                                Else
                                    HelperFunctions.ShowMessageToUser("Please select the person(s) to create a batch.", EnumMessageTypes.Error)
                                End If
                            Else
                                HelperFunctions.ShowMessageToUser("Please select the person(s) to create a batch.", EnumMessageTypes.Error)
                            End If

                            Exit Sub

                            ''Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                            'Dim BatchID As String = String.Empty
                            'BatchID = CType(Session("dtSelectedBatchRecords"), DataTable).Rows(0)("BatchId") 'GridViewCashoutBatch.DataKeys(intIndex).Value.ToString()
                            'Session("PrintBatchID") = BatchID
                            'If (Not Session("PrintBatchID") Is Nothing AndAlso Session("PrintBatchID").ToString().Length > 1) Then
                            '    'Dim l_printBatchDdata As DataSet
                            '    'l_printBatchDdata = objCashOutBOClass.GetBatchDetails(BatchID)

                            '    ' Session("printBatchDdata") = l_printBatchDdata.Tables(0)

                            '    'Added By SG: 2012.12.05: BT-960
                            '    ' Session("strReportName") = "Withdrawals" 'rdFormToPrint.SelectedValue
                            '    Session("strReportName") = "Withdrawals_New"
                            '    Session("strModuleName") = "CashOut"
                            '    Me.PrintReport()
                            '    'END Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 

                            'End If
                            'End : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                        ElseIf l_string_CashOutReqType = ProcessState.Process.ToString() AndAlso l_bool_status = True Then
                            'Start : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                            'Below line code is added for request process.
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Start: Request Process started.")
                            objCashOutBOClass.CreateAndProcessRequest(dtSelectedBatchRecords, l_bool_status, l_string_CashOutReqType)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Finish: Request Process started.")
                            'End : Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
                            If Not dtSelectedBatchRecords Is Nothing Then
                                Dim l_decimal_RmdAmount As Decimal = 0
                                Dim l_decimal_MilleAmount As Decimal = 0
                                Dim l_int_RmdCount As Integer = 0
                                Dim l_int_MilleCount As Integer = 0
                                'Priya 22-Nov-2012  BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Coding to get Processed value
                                Dim BatchID As String = String.Empty
                                BatchID = dtSelectedBatchRecords.Rows(0)("BatchId") 'GridViewCashoutBatch.DataKeys(intIndex).Value.ToString()
                                Dim l_BatchDdata As DataSet
                                l_BatchDdata = objCashOutBOClass.GetBatchDetails(BatchID)
                                'Start/2014.11.28/SR/BT 2728: Cash out payment register mail shows wrong information
                                Dim TobeDistinct As String() = {"FundEventId", "IsRMDEligible"}
                                Dim dtDistinct As DataTable = GetDistinctRecords(dtSelectedBatchRecords, TobeDistinct)
                                'End/2014.11.28/SR/BT 2728: Cash out payment register mail shows wrong information
                                If HelperFunctions.isNonEmpty(l_BatchDdata) Then

                                    'For i As Integer = 0 To dtSelectedBatchRecords.Rows.Count - 1
                                    For i As Integer = 0 To dtDistinct.Rows.Count - 1  '2014.11.28/SR/BT 2728: Replaced dtSelectedBatchRecords with dtDistinct - TO get uniquefundevent
                                        For j As Integer = 0 To l_BatchDdata.Tables(0).Rows.Count - 1
                                            'If l_BatchDdata.Tables(0).Rows(j)("guiFundEventId").ToString() = dtSelectedBatchRecords.Rows(i)("FundEventId").ToString() Then
                                            '    If dtSelectedBatchRecords.Rows(i)("IsRMDEligible").ToString() = "True" Then
                                            If l_BatchDdata.Tables(0).Rows(j)("guiFundEventId").ToString() = dtDistinct.Rows(i)("FundEventId").ToString() Then  '2014.11.28/SR/BT 2728: Replaced dtSelectedBatchRecords with dtDistinct - TO get unique guifundevent record
                                                If dtDistinct.Rows(i)("IsRMDEligible").ToString() = "True" Then  '2014.11.28/SR/BT 2728: Replaced dtSelectedBatchRecords with dtDistinct - TO get unique guifundeventid record
                                                    l_int_RmdCount = l_int_RmdCount + 1
                                                    If l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt").ToString().Trim.Length > 0 Then
                                                        l_decimal_RmdAmount = l_decimal_RmdAmount + Convert.ToDecimal(l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt").ToString())
                                                    End If

                                                Else
                                                    l_int_MilleCount = l_int_MilleCount + 1
                                                    'If l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt").ToString() <> "" OrElse Not IsNothing(l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt")) Then
                                                    If l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt").ToString().Trim.Length > 0 Then
                                                        l_decimal_MilleAmount = l_decimal_MilleAmount + Convert.ToDecimal(l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt").ToString())
                                                    End If
                                                    'l_decimal_MilleAmount = l_decimal_MilleAmount + Convert.ToDecimal(l_BatchDdata.Tables(0).Rows(j)("mnyProcessedAmt").ToString())
                                                End If
                                            End If
                                        Next
                                    Next
                                End If

                                Session("50.1to5000") = "50.1to5000"
                                'For i As Integer = 0 To dtSelectedBatchRecords.Rows.Count - 1



                                '	If dtSelectedBatchRecords.Rows(i)("IsRMDEligible").ToString() = "True" Then
                                '		l_int_RmdCount = l_int_RmdCount + 1
                                '		l_decimal_RmdAmount = l_decimal_RmdAmount + Convert.ToDecimal(dtSelectedBatchRecords.Rows(i)("EligibleBalance").ToString())
                                '	Else
                                '		l_int_MilleCount = l_int_MilleCount + 1
                                '		l_decimal_MilleAmount = l_decimal_MilleAmount + Convert.ToDecimal(dtSelectedBatchRecords.Rows(i)("EligibleBalance").ToString())
                                '	End If
                                'Next
                                'END Priya 22-Nov-2012  BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Coding to get Processed value
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Start: Sending Mail.")
                                SendMail(l_int_MilleCount, l_int_RmdCount, l_decimal_MilleAmount, l_decimal_RmdAmount)
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Finish: Mail Send.")

                                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()

                            End If
                        End If
                        'BindBatchGrid()
                    ElseIf DropDownAmount.SelectedValue = CashoutRange.Range1.ToString() Then 'SP 2014.04.08 BT 2298/YRS 5.0-2259- adding if to check specific

                        'SP 2014.10.07 BT-2633\YRS 5.0-2403 -Start
                        XmlSelectedPersDetails = GetXmlPerDetailsFromDataSet(dtSelectedBatchRecords)
                        If Not String.IsNullOrEmpty(XmlSelectedPersDetails) Then
                            ExpiredRefRequestsForSelectedPerson(XmlSelectedPersDetails)
                        End If
                        'SP 2014.10.07 BT-2633\YRS 5.0-2403 -End

                        objCashOutBOClass.CreateAndProcessRequest(dtSelectedBatchRecords, l_bool_status)
                        If l_bool_status = True Then


                            'Added By SG: 2012.12.18: BT-1511:
                            If Me.DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then
                                BindBatchGrid()
                            End If

                            BindRangeDetails()
                            'Me.CashoutTabStrip.Visible = False

                            'Session("g_bool_ProcessOK") = Nothing
                            'START: Added By SG: BT-960
                            Me.LabelNoRecords.Text = "No Records Found"
                            Me.LabelNoRecords.Visible = False


                            Session("dtSelectedBatchRecords") = dtSelectedBatchRecords
                            CallReportsForUnder50()
                            'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                            'SP:2014.04.08 -BT 2298/YRS 5.0-2259 - Add code to open summary report for 0-50 range cashout -Start
                            'Session("strReportName_1") = "Cash Out"
                            'Dim popupScript3 As String = "<script language='javascript'>" & _
                            '                              "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp1', " & _
                            '                              "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                            '                              "</script>"
                            'If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                            '    Page.RegisterStartupScript("PopupScript2", popupScript3)
                            'End If
                            'SP:2014.04.08 -BT 2298/YRS 5.0-2259 - Add code to open summary report for 0-50 range cashout -End
                            'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                        End If
                    End If

                    'Get the dataset containing the processed records.
                    'g_dataset_Report = objCashOutBOClass.SelectReportData(Session("IPAddress"), Session("UserId"), Session("CashoutBatchId"))
                    'Calling the report


                    'Call ReportViewer.aspx 
                    'Start, SR:2014.04.02 -BT 2298/YRS 5.0-2259 - Below line of code commented to block summary report popup before creating batch                    
                    'Dim popupScript3 As String = "<script language='javascript'>" & _
                    '"window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp1', " & _
                    '"'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                    '"</script>"
                    'If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    '    Page.RegisterStartupScript("PopupScript2", popupScript3)
                    'End If
                    'End, SR:2014.04.02 -BT 2298/YRS 5.0-2259 - Below line of code commented to block summary report popup before creating batch
                Catch
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Start: Some Exception Occured.")
                    Throw
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Finish: Some Exception Occured.")
                End Try

                'If g_dataset_Report.Tables(0).Rows.Count > 0 Then
                '    'To Generate Letters
                '    'Call to IDM
                '    l_IDMDatable = .CashoutReports(g_dataset_Report)
                'Else
                '    '  MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "No Reports to be generated", MessageBoxButtons.OK)
                '    '  Exit Sub
                'End If
            Else
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Please Select an Item to Proceed", MessageBoxButtons.OK)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser("Please select at least one cashout to proceed.", EnumMessageTypes.Error)
                ButtonProcess.Enabled = True
                Exit Sub
            End If

            'If l_IDMDatable.Rows.Count > 0 Then
            '    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error While Copying to File", MessageBoxButtons.OK)
            '    Session("g_bool_ProcessOK") = True
            '    Exit Sub
            'Else
            'Shubhrata Nov 29th 2006
            If l_bool_status = True Then
                'START: Added By SG: BT-960
                Me.LinkEligibleList.Visible = False
                Me.LinkNotEligibleList.Visible = False

                'Me.tdParticipantPanel.InnerText = ""
                'Commented by Dinesh Kanojia to incorporate master file
                'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                'labelBatchID.Text = ""

                Me.trBatchHeader.Visible = False
                'Me.trListHeader.Visible = False
                'END: Added By SG: BT-960
                ''SR:2012.11.16- YRS 5.0-1489- Message content change as per observation from tester
                If l_string_CashOutReqType = ProcessState.Request.ToString() Then

                    ' MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cash Out Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + "<BR> Created Successfully.", MessageBoxButtons.OK)
                Else
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cash Out Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + "<BR> Processed Successfully.", MessageBoxButtons.OK)
                    'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                    HelperFunctions.ShowMessageToUser("Cash Out Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + " processed successfully.", EnumMessageTypes.Success)
                    Session("Cashoutprocess") = True
                    Response.Redirect("CashOut.aspx?batchId=" + CType(Session("CashoutBatchId"), String).Trim(), False)
                End If
                Session("g_bool_ProcessOK") = True
                ButtonProcess.Enabled = False
                'Added By SG: 2012.12.18: BT-1511:
                If Me.DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() Then
                    BindBatchGrid()
                End If

                BindRangeDetails()

                'Added By SG: 2012.01.04: BT-1511
                labelFilter.Text = ""
                Session("SegregateDatatable") = Nothing

                ' Response.Redirect("CashOut.aspx", False)
                Exit Sub
            Else
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error while processing CashOut Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + ".", MessageBoxButtons.Stop)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser("There was an error while processing CashOut Batch ID: " + CType(Session("CashoutBatchId"), String).Trim() + ".", EnumMessageTypes.Error)
                Session("g_bool_ProcessOK") = True
                Exit Sub
            End If
            ' End If
        Catch
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Main Exception block executed.")
            Throw
            'GetCatch(ex)
        Finally 'Added By SG: BT-960
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Button Process Clicked", "Finally block executed.")
            objCashOutBOClass = Nothing
        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click, ButtonOkBatch.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch
            Throw
            'GetCatch(ex)
        End Try
    End Sub

    'aparna -yren -3016
    Private Sub ButtonUpdateCounter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdateCounter.Click
        Dim l_Str_SelectedCashouts As Integer
        Dim l_dgitem As DataGridItem
        Dim l_checkbox As New CheckBox
        Try
            Const EligibleBalance As Integer = 10
            Me.TotalAmountSelected = 0.0
            Me.SelectedCashouts = 0
            For Each l_dgitem In Me.DataGridCashOut.Items
                l_checkbox = l_dgitem.FindControl("CheckBoxSelect")
                If l_checkbox.Checked Then
                    Me.TotalAmountSelected += CType(l_dgitem.Cells(EligibleBalance).Text, Decimal)
                    Me.SelectedCashouts += 1
                End If
            Next

            Me.TextboxTotalAmtSelected.Text = "$" + Me.TotalAmountSelected.ToString()
            Me.TextBoxCshoutsSelected.Text = Me.SelectedCashouts.ToString()
            Me.TextboxTotalAmt.Text = "$" + Me.TotalAmount.ToString()
            Me.TextBoxCashoutList.Text = Me.TotalNoOfCashouts.ToString()

            'Dim popupScript3 As String = "<" & "script language='javascript'>" & _
            '                      "ShowHideButton('block')</" & "script" & ">"
            'If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
            '    Page.RegisterStartupScript("PopupScript21", popupScript3)
            'End If

        Catch
            Throw
            ' GetCatch(ex)
        End Try
    End Sub
    'aparna -yren -3016
    Private Sub ButtonGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGetData.Click
        Try
            'Added by SG: 2012.11.27: BT-960
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(sender.ID.ToString(), Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)   'Shilpa N | 02/22/2019 | YRS-AT-4248 | Changed RegisterClientScriptBlock to RegisterStartupScript. It was not loading the Dialogue box. 
                Exit Sub
            End If

            'If Not checkSecurity.Equals("True") Then
            '    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'End : BT-960

            Me.trListHeader.Visible = True

            FetchCashoutData()

            'Added By SG: 2012.01.04: BT-1511
            labelFilter.Text = ""
            Session("SegregateDatatable") = Nothing

            Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                                   "ShowHideButton('block')</" & "script" & ">"
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript21", popupScript3)
            End If

        Catch
            Throw
        End Try
    End Sub

    Private Sub DataGridCashOut_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridCashOut.ItemCreated
        Try
            If LinkEligibleList.Visible = True Then
                e.Item.Cells(0).Visible = False
                'Added By SG: 2012.12.12: BT-1511
                'e.Item.Cells(22).Visible = True
                e.Item.Cells(20).Visible = True
            Else
                e.Item.Cells(0).Visible = True
                'Added By SG: 2012.12.12: BT-1511
                'e.Item.Cells(22).Visible = False
                e.Item.Cells(20).Visible = False
            End If

            'Added By SG: 2012.12.21: BT-1511
            If DropDownAmount.SelectedValue = CashoutRange.Range2.ToString() _
                AndAlso Not Session("CashOutReqType") Is Nothing AndAlso Session("CashOutReqType") = ProcessState.Request.ToString() Then
                e.Item.Cells(21).Visible = True
            Else
                e.Item.Cells(21).Visible = False
            End If

        Catch
            Throw
            'GetCatch(ex)
        End Try
    End Sub

    Private Sub DataGridCashOut_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridCashOut.ItemDataBound
        Try
            Const EligibleBalance As Integer = 10
            Const intIsVested As Integer = 7
            'Commented By SG: 2012.12.12: BT-1511
            'Const intCheckCashed As Integer = 20
            Const intIsRMDEligible As Integer = 19
            Const intIsHighlighted As Integer = 16
            Const intMaxTermDate As Integer = 8
            Const intLastContributionDate As Integer = 9

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Me.TotalAmount += e.Item.Cells(EligibleBalance).Text


                If e.Item.Cells(intIsRMDEligible).Text = "True" Then
                    e.Item.BackColor = System.Drawing.Color.GreenYellow
                    If e.Item.Cells(intIsHighlighted).Text = "1" Then
                        e.Item.Cells(intMaxTermDate).ForeColor = System.Drawing.Color.LightCoral
                        e.Item.Cells(intLastContributionDate).ForeColor = System.Drawing.Color.LightCoral
                        e.Item.Cells(intMaxTermDate).Font.Bold = True
                        e.Item.Cells(intLastContributionDate).Font.Bold = True
                    End If
                ElseIf e.Item.Cells(intIsHighlighted).Text = "1" Then
                    e.Item.BackColor = System.Drawing.Color.LightCoral
                End If

                If e.Item.Cells(intIsVested).Text = "1" Then
                    e.Item.Cells(intIsVested).Text = "Yes"
                Else
                    e.Item.Cells(intIsVested).Text = "No"
                End If

                'Commented By SG: 2012.12.12: BT-1511
                'If e.Item.Cells(intCheckCashed).Text = "1" Then
                '    e.Item.Cells(intCheckCashed).Text = "Yes"
                'ElseIf e.Item.Cells(intCheckCashed).Text = "0" Then
                '    e.Item.Cells(intCheckCashed).Text = "No"
                'End If
            End If
        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub

    Private Sub DropDownAmount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownAmount.SelectedIndexChanged
        'Added By SG: 2012.12.18: BT-1511:
        BindRangeDetails()
    End Sub

    'Added By SG: 2012.12.18: BT-1511:
    Private Sub BindRangeDetails()
        Dim dtBatch As DataTable
        Dim drFilterBatch As DataRow()
        Try
            Me.DropdownValue = Me.DropDownAmount.SelectedValue

            If Me.DropdownValue = CashoutRange.Range1.ToString() Then
                Session("CashOutReqType") = Nothing
                Me.ButtonGetData.Enabled = True
                'START: Added By SG: BT-960
                'Me.CashoutTabStrip.Items.Item(1).Enabled = False
                'Me.CashoutTabStrip.Items.Item(0).Enabled = True
                Me.CashoutMultiPage.SelectedIndex = 1
                Me.LinkNotEligibleList.Visible = False
                Me.LinkEligibleList.Visible = False

                'tdParticipantPanel.InnerText = ""
                'Commented by Dinesh Kanojia to incorporate master file
                'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                'labelBatchID.Text = ""

                ButtonProcess.Text = "Process Cashout"
                ClearDataNControls()
                Me.CashoutTabStrip.Visible = False
                Me.trBatchHeader.Visible = False
                Me.trListHeader.Visible = False
            ElseIf Me.DropdownValue = CashoutRange.Range2.ToString() Then
                BindBatchGrid()
                Me.trBatchHeader.Visible = True
                Me.trListHeader.Visible = True
                Me.CashoutTabStrip.Visible = True
                'Me.CashoutTabStrip.Items.Item(1).Enabled = True
                'Me.CashoutTabStrip.Items.Item(0).Enabled = False
                Me.CashoutTabStrip.SelectedIndex = 0
                Me.CashoutMultiPage.SelectedIndex = 0

                Me.LinkProcessBatch.Visible = True
                Me.LinkUnprocessBatch.Visible = False
                Me.tdBatchPanel.InnerText = "List of Unprocessed Batches"

                'Session("CashOutReqType") = ProcessState.Request.ToString()
                If Not Session("CashoutBatches") Is Nothing Then
                    dtBatch = CType(Session("CashoutBatches"), DataTable)
                    If Not dtBatch Is Nothing AndAlso dtBatch.Rows.Count > 0 Then
                        'drFilterBatch = dtBatch.Select("ProcessedDate IS NULL")
                        'Start:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                        'drFilterBatch = dtBatch.Select("ProcessedDate IS NULL")
                        drFilterBatch = dtBatch.Select("ProcessedDate IS NULL AND chvCashOutRangeDesc <> 'SPECIAL CASHOUT'")
                        'End:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                        If Not drFilterBatch Is Nothing AndAlso drFilterBatch.Length > 0 Then
                            Session("CashoutFilterBatches") = drFilterBatch.CopyToDataTable()
                            Me.GridViewCashoutBatch.AllowPaging = False
                            Me.GridViewCashoutBatch.DataSource = drFilterBatch.CopyToDataTable()
                            Me.GridViewCashoutBatch.DataBind()
                            Me.LabelNoBatch.Visible = False

                        Else
                            Me.LabelNoBatch.Visible = True
                        End If
                    Else
                        Me.LabelNoBatch.Visible = True
                    End If
                Else
                    Me.LabelNoBatch.Visible = True
                End If

                If Me.LabelNoRecords.Visible = True Then
                    Me.GridViewCashoutBatch.DataSource = Nothing
                    Me.GridViewCashoutBatch.DataBind()
                End If

                ButtonProcess.Text = "Save Batch"
                ClearDataNControls()
            Else
                Me.trBatchHeader.Visible = False
                Me.trListHeader.Visible = False
                Me.CashoutTabStrip.Visible = False
                Me.CashoutMultiPage.SelectedIndex = 1
                Session("CashOutReqType") = Nothing
                Me.ButtonGetData.Enabled = False
                Me.LinkNotEligibleList.Visible = False
                Me.LinkEligibleList.Visible = False

                'tdParticipantPanel.InnerText = ""
                'Commented by Dinesh Kanojia to incorporate master file
                'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                'labelBatchID.Text = ""

                ClearDataNControls()
                'END: Added By SG: BT-960
            End If
        Catch
            Throw
            ' GetCatch(ex)
        Finally
            dtBatch = Nothing
            drFilterBatch = Nothing
        End Try
    End Sub

#Region "Private Methods"

    Private Function ClearData()
        Try
            'Commented By SG: 2012.12.18: BT-1511:
            'Me.DropDownAmount.SelectedIndex = 0

            'Me.DataGridCashOut.Visible = False
            Me.DataGridCashOut.DataSource = Nothing
            Me.DataGridCashOut.DataBind()
            Me.Session_DataSet_Cashouts = Nothing
            Me.ButtonGetData.Enabled = False
            Me.ButtonProcess.Enabled = False
            Me.ButtonReport.Enabled = False
            Me.ButtonSelectAll.Enabled = False
            'aparna yren-3016 
            Me.SelectedCashouts = 0
            Me.TotalNoOfCashouts = 0
            Me.TotalAmountSelected = 0.0
            Me.TotalAmount = 0.0
            Me.LabelCashouts.Visible = False
            Me.LabelSelectedCashouts.Visible = False
            Me.LabelTotal.Visible = False
            Me.LabelSelectedAmt.Visible = False
            Me.TextBoxCashoutList.Visible = False
            Me.TextBoxCshoutsSelected.Visible = False
            Me.TextboxTotalAmt.Visible = False
            Me.TextboxTotalAmtSelected.Visible = False
            Me.ButtonUpdateCounter.Enabled = False
            'aparna yren -3016

            'Added By SG: 2013.01.02
            Session("FilterDatatable") = Nothing
        Catch ex As Exception
            GetCatch(ex)
        End Try

    End Function
    'Binds the datagrid
    Private Function BindDataintheGrid(ByVal stTableName As String)
        Dim l_String_Search As String
        Dim l_CheckBox As CheckBox
        Dim dtSelectedBatchRecords As New DataTable
        Dim g_dataset_Cashouts As New DataSet
        Dim l_string_BatchId As String
        Dim l_drFilter As DataRow()
        Try
            Me.TotalNoOfCashouts = 0
            Me.SelectedCashouts = 0
            Me.TotalAmount = 0.0
            Me.TotalAmountSelected = 0.0

            Session("DiscardBatch") = Nothing

            If Not Me.Session_DataSet_Cashouts Is Nothing Then
                g_dataset_Cashouts = Me.Session_DataSet_Cashouts
                'Get Batchid from the CashoutLog table
                If g_dataset_Cashouts.Tables(stTableName).Rows.Count > 0 Then
                    l_string_BatchId = g_dataset_Cashouts.Tables(stTableName).Rows(0)("BatchId").ToString().Trim()
                    Session("CashoutBatchId") = l_string_BatchId
                End If

                If g_dataset_Cashouts.Tables(stTableName).Rows.Count > 0 Then
                    If Not Session("CashOutReqType") Is Nothing AndAlso Session("CashOutReqType").ToString() = ProcessState.Process.ToString() Then
                        'Priya 11/24/2012 Added code to to make sorting for eligible and not eligible participant
                        If LinkEligibleList.Visible = False Then
                            'BindUpdatePanelGrid(LinkEligibleList, e)
                            l_drFilter = g_dataset_Cashouts.Tables(stTableName).Select("Remarks = ''")
                        Else
                            'BindUpdatePanelGrid(LinkEligibleList, e)
                            l_drFilter = g_dataset_Cashouts.Tables(stTableName).Select("Remarks <> ''")
                        End If
                        'END Priya 11/24/2012 Added code to to make sorting for eligible nad not eligible participant
                        'l_drFilter = g_dataset_Cashouts.Tables(stTableName).Select("Remarks = ''")
                        If Not l_drFilter Is Nothing AndAlso l_drFilter.Length > 0 Then
                            dtSelectedBatchRecords = l_drFilter.CopyToDataTable()
                            dtSelectedBatchRecords.DefaultView.Sort = CType(Session("CashoutListSort"), String)
                            LabelNoRecords.Visible = False
                        Else
                            LabelNoRecords.Visible = True
                            If LabelNoRecords.Text = "No Records Found" Then
                                LabelNoRecords.Text = LabelNoRecords.Text + ", <BR>To cancel all pending requests from this batch, please process the batch."
                            End If

                            'LabelNoRecords.Text = LabelNoRecords.Text + ", <BR>To cancel all pending requests from this batch, please process the batch."
                            Session("DiscardBatch") = "True"
                        End If
                    Else
                        dtSelectedBatchRecords = g_dataset_Cashouts.Tables(stTableName)
                        dtSelectedBatchRecords.DefaultView.Sort = CType(Session("CashoutListSort"), String)
                    End If

                    If Not dtSelectedBatchRecords Is Nothing AndAlso dtSelectedBatchRecords.Rows.Count > 0 Then
                        Me.DataGridCashOut.DataSource = dtSelectedBatchRecords.DefaultView
                        Me.DataGridCashOut.DataBind()
                        Me.tbUpdateCounter.Visible = True

                        'Added By SG: 2013.01.02
                        Session("FilterDatatable") = dtSelectedBatchRecords
                    Else
                        Me.DataGridCashOut.DataSource = Nothing
                        Me.DataGridCashOut.DataBind()

                        'Added By SG: 2013.01.02
                        Session("FilterDatatable") = Nothing
                    End If

                    Me.ButtonSelectAll.Enabled = True
                    'aparna -yren-3016
                    'by Aparna Samala 07/02/2007

                    Me.LabelCashouts.Visible = True
                    Me.LabelSelectedCashouts.Visible = True
                    Me.LabelTotal.Visible = True
                    Me.LabelSelectedAmt.Visible = True
                    Me.TextBoxCashoutList.Visible = True
                    Me.TextBoxCshoutsSelected.Visible = True
                    Me.TextboxTotalAmt.Visible = True
                    Me.TextboxTotalAmtSelected.Visible = True

                    'by Aparna Samala 07/02/2007
                    Me.TextboxTotalAmtSelected.Visible = True
                    Me.TotalNoOfCashouts = dtSelectedBatchRecords.Rows.Count
                    Me.ButtonUpdateCounter.Enabled = True
                    Me.ButtonProcess.Enabled = True
                    Me.ButtonReport.Enabled = True
                    Me.TextboxTotalAmtSelected.Text = "$" + Me.TotalAmountSelected.ToString()
                    Me.TextBoxCshoutsSelected.Text = Me.SelectedCashouts.ToString()
                    Me.TextboxTotalAmt.Text = "$" + Me.TotalAmount.ToString()
                    Me.TextBoxCashoutList.Text = Me.TotalNoOfCashouts.ToString()
                    'aparna -yren-3016
                Else
                    Me.DataGridCashOut.DataSource = Nothing
                    Me.DataGridCashOut.DataBind()
                    LabelNoRecords.Visible = True
                    LabelNoRecords.Text = "No Records Found"
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "No Records Found", MessageBoxButtons.Stop)

                    'Added By SG: 2013.01.02
                    Session("FilterDatatable") = Nothing
                End If
            End If

        Catch
            Throw
        Finally

        End Try

    End Function

    'Insert or Delete from  the Log table
    Private Function CashoutLogDataFromGrid(ByVal boolForRequestProcess As Boolean, ByVal l_string_CashOutReqType As String)
        Dim g_datatable_Cashouts As New DataTable
        Dim l_string_Search As String
        Dim l_CheckBox As New CheckBox
        Dim l_Find_Datarow As DataRow()
        Dim l_Datagriditem As DataGridItem
        Dim g_dataset_Report As New DataSet
        Dim dtSelectedBatchRecords As New DataTable
        'Incase the Item order in the datagrid is changed the below values should be changed accordingly
        Const intSSNogridcolIndex As Integer = 1
        Const intLNamegridcolIndex As Integer = 3
        Const intFNamegridcolIndex As Integer = 4
        Const intMNamegridcolIndex As Integer = 13
        Const intAgegridcolIndex As Integer = 14
        Const intEligiblebalIndex As Integer = 10
        Const intFundEventIdgridcolIndex As Integer = 15
        Const intFundIdNogridcolIndex As Integer = 2
        'aparna yren -3016
        Const intTaxableAmtIndex As Integer = 12
        'YRPS -4181
        Const intPlantypegridcolIndex As Integer = 11
        'Added By SG: BT-960
        Const intRefRequestIDIndex As Integer = 18
        Const intIsRMDEligible As Integer = 19
        Const intEstimatedBalance As Integer = 21
        Dim l_dataset_CashoutLog As New DataSet
        Dim dtSelectedBatchRecordsLog As New DataTable
        Dim l_dataset_BatchId As New DataSet
        Dim l_string_newBatchId As String

        Try
            'START: Added By SG: BT-960
            Dim stTableName As String

            If l_string_CashOutReqType = ProcessState.Process.ToString() Then
                stTableName = "RequestedParticipants"
            Else
                stTableName = "EligibleParticipants"
            End If
            'END: Added By SG: BT-960

            l_string_newBatchId = Nothing

            'get the Schema of the yrs_AtsCashoutsLog table
            objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
            l_dataset_CashoutLog = objCashOutBOClass.GetCashoutLogSchema()

            'START: Added By SG: BT-960
            Dim l_dcol As DataColumn = New DataColumn("bitIsRMDEligible")
            l_dcol.DataType = System.Type.GetType("System.Boolean")
            l_dataset_CashoutLog.Tables("CashoutLogSchema").Columns.Add(l_dcol)
            'END: Added By SG: BT-960

            'get New BatchId for the yrs_AtsCashoutsLog table
            'l_dataset_BatchId = objCashOutBOClass.GetNextBatchId()
            'If l_dataset_BatchId.Tables(0).Rows.Count > 0 Then
            '    l_string_newBatchId = l_dataset_BatchId.Tables(0).Rows(0)(0)
            '    Session("CashoutBatchId") = l_string_newBatchId

            'End If

            If Not Me.Session_DataSet_Cashouts Is Nothing Then
                g_datatable_Cashouts = Me.Session_DataSet_Cashouts.Tables(stTableName)
                dtSelectedBatchRecords = g_datatable_Cashouts.Clone
            End If
            l_dataset_CashoutLog.Tables("CashoutLogSchema").Columns.Add("PersId")
            'Looping through the datagrid and finding out the selected items

            For Each l_Datagriditem In Me.DataGridCashOut.Items
                'This is for cashout log
                Dim l_drow As DataRow

                l_drow = l_dataset_CashoutLog.Tables("CashoutLogSchema").NewRow()
                l_drow("chvBatchId") = Session("CashoutBatchId")
                l_drow("chrSSNo") = l_Datagriditem.Cells(intSSNogridcolIndex).Text.Trim()
                If l_Datagriditem.Cells(intLNamegridcolIndex).Text.Trim() = "&nbsp;" Then
                    l_drow("chvLastName") = ""
                Else
                    l_drow("chvLastName") = l_Datagriditem.Cells(intLNamegridcolIndex).Text.Trim()
                End If
                If l_Datagriditem.Cells(intFNamegridcolIndex).Text.Trim() = "&nbsp;" Then
                    l_drow("chvFirstName") = ""
                Else
                    l_drow("chvFirstName") = l_Datagriditem.Cells(intFNamegridcolIndex).Text.Trim()
                End If
                If l_Datagriditem.Cells(intMNamegridcolIndex).Text.Trim() = "&nbsp;" Then
                    l_drow("chvMiddleName") = ""
                Else
                    l_drow("chvMiddleName") = l_Datagriditem.Cells(intMNamegridcolIndex).Text.Trim()
                End If
                ' l_drow("chvMiddleName") = l_Datagriditem.Cells(intMNamegridcolIndex).Text.Trim()

                'Priya Patil 08.11.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changesmade for erro come if age is "&nbsp;"
                'l_drow("numPersonAge") = l_Datagriditem.Cells(intAgegridcolIndex).Text.Trim()
                If l_Datagriditem.Cells(intAgegridcolIndex).Text.Trim() = "&nbsp;" Then
                    l_drow("numPersonAge") = System.DBNull.Value
                Else
                    l_drow("numPersonAge") = l_Datagriditem.Cells(intAgegridcolIndex).Text.Trim()
                End If
                'END Priya Patil 08.11.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
                l_drow("mnyGrossRefundAmount") = CType(l_Datagriditem.Cells(intEligiblebalIndex).Text, Decimal)

                'Added By SG: 2012.12.31: BT-1511
                If l_string_CashOutReqType = ProcessState.Request.ToString() Then
                    l_drow("mnyEstimatedBalance") = CType(l_Datagriditem.Cells(intEstimatedBalance).Text, Decimal)
                End If

                'mnyTaxWithHeld
                'aparna -yren -3016
                If l_string_CashOutReqType Is Nothing Then
                    l_drow("mnyTaxWithHeld") = 0.2 * CType(l_Datagriditem.Cells(intTaxableAmtIndex).Text.Trim(), Decimal)
                Else
                    l_drow("mnyTaxWithHeld") = 0
                End If

                'aparna -yren -3016
                l_drow("guiFundEventId") = l_Datagriditem.Cells(intFundEventIdgridcolIndex).Text.Trim()
                l_drow("intFundIdNo") = l_Datagriditem.Cells(intFundIdNogridcolIndex).Text.Trim()
                'aparna -YRPS -4181
                l_drow("chvPlanType") = l_Datagriditem.Cells(intPlantypegridcolIndex).Text.Trim()
                'aparna -YRPS -4181
                If boolForRequestProcess = True Then
                    l_drow("bitLookUp") = 0
                Else
                    l_drow("bitLookUp") = 1
                End If

                l_drow("intStdReportCount") = 0
                l_drow("intActualReportCount") = 0
                l_drow("chvCashOutRangeDesc") = Session("CashOutRangeDesc")
                l_drow("chvUserId") = Session("LoginId")
                l_drow("chvIPAddress") = Session("IPAddress")

                'START: Added By SG: BT-960
                If l_string_CashOutReqType = ProcessState.Process.ToString() Then
                    l_drow("guiRefRequestId") = l_Datagriditem.Cells(intRefRequestIDIndex).Text.Trim()
                End If
                'END: Added By SG: BT-960

                l_CheckBox = l_Datagriditem.FindControl("CheckBoxSelect")
                If l_CheckBox.Checked = True Then
                    If boolForRequestProcess = True Then
                        'Added By SG: 2012.08.23: BT-960
                        'l_string_Search = "FundEventId = '" + l_Datagriditem.Cells(intFundEventIdgridcolIndex).Text + "'"
                        l_string_Search = "FundEventId = '" + l_Datagriditem.Cells(intFundEventIdgridcolIndex).Text + "' AND PlansType = '" + l_Datagriditem.Cells(intPlantypegridcolIndex).Text + "'"
                        l_Find_Datarow = g_datatable_Cashouts.Select(l_string_Search)
                        If Not l_Find_Datarow(0) Is Nothing Then
                            Dim l_datarow As DataRow
                            'This takes only those rows which are selected in new datatable
                            l_Find_Datarow(0)("Selected") = 1
                            l_datarow = l_Find_Datarow(0)
                            dtSelectedBatchRecords.ImportRow(l_datarow)
                        End If

                        l_drow("bitSelected") = 1
                        'l_dataset_CashoutLog.Tables("CashoutLogSchema").Rows.Add(l_drow)
                        'Else
                        '    l_drow("bitSelected") = 0
                        'l_dataset_CashoutLog.Tables("CashoutLogSchema").Rows.Add(l_drow)
                    End If
                    'aparna yren-3016
                    l_drow("bitSelected") = 1
                Else
                    'If boolForRequestProcess = False Then
                    l_drow("bitSelected") = 0
                    '  l_dataset_CashoutLog.Tables("CashoutLogSchema").Rows.Add(l_drow)
                    ' End If

                End If

                'Added By SG: BT-960
                l_drow("bitIsRMDEligible") = l_Datagriditem.Cells(intIsRMDEligible).Text.Trim()

                l_drow("PersId") = l_Datagriditem.Cells(22).Text.Trim()

                l_dataset_CashoutLog.Tables("CashoutLogSchema").Rows.Add(l_drow)

                '   l_dataset_CashoutLog.Tables(0).ImportRow(l_drow)
            Next

            'Deleting the data from Log table for the particular user
            'Added By SG: 2012.19.11: BT-960
            'objCashOutBOClass.DeleteReportData(Session("LoginId"), Session("IPAddress"))
            objCashOutBOClass.DeleteReportData(Session("LoginId"), Session("IPAddress"), Session("CashoutBatchId"))

            'Added By SG: BT-960
            If l_string_CashOutReqType = ProcessState.Process.ToString() AndAlso boolForRequestProcess = True Then
                'dtSelectedBatchRecords.AcceptChanges()
                Session("dtSelectedBatchRecords") = dtSelectedBatchRecords
            Else
                'This datatable is for the request processing
                If boolForRequestProcess = True Then
                    dtSelectedBatchRecords.AcceptChanges()
                    Session("dtSelectedBatchRecords") = dtSelectedBatchRecords
                    'by Aparna - 07/02/2007 -YREN-3061
                    'To ensure that the records are not sent to cashoutlog table if they are not selected in grid
                    'Inserting the data into Log table for the particular user
                    If dtSelectedBatchRecords.Rows.Count > 0 Then
                        objCashOutBOClass.InsertReportData(l_dataset_CashoutLog)
                        objCashOutBOClass = Nothing
                    End If
                Else
                    'Inserting the data into Log table for the particular user
                    objCashOutBOClass.InsertReportData(l_dataset_CashoutLog)
                    objCashOutBOClass = Nothing
                    'by Aparna - 07/02/2007 -YREN-3061
                End If
            End If

            'Commented by Aparna - 07/02/2007 -YREN-3061

            ''Inserting the data into Log table for the particular user
            'objCashOutBOClass.InsertReportData(l_dataset_CashoutLog)
            'objCashOutBOClass = Nothing

            ' YMCARET.YmcaBusinessObject.CashoutBOClass.InsertReportData(l_dataset_CashoutLog)

        Catch ex As Exception
            GetCatch(ex)
        End Try

    End Function
    'To open the Report
    Private Sub OpenReportViewer()
        Try
            Session("strReportName") = "Cash Out"
            'Call ReportViewer.aspx 
            Dim popupScript As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If

        Catch
            Throw
        End Try

    End Sub

    'To check or unCheck the checkboxes in the datagrid
    '<System.Web.Services.WebMethod()> _
    Private Sub SelectAll()
        Try
            Dim l_CheckBox As System.Web.UI.WebControls.CheckBox
            Dim l_DatagridItem As DataGridItem
            Const EligibleBalance As Integer = 10

            Me.SelectedCashouts = 0
            Me.TotalAmountSelected = 0.0
            If DataGridCashOut.Items.Count > 0 Then
                Me.SelectedCashouts = DataGridCashOut.Items.Count
                If ButtonSelectAll.Text = "Select All" Then
                    For Each l_DatagridItem In DataGridCashOut.Items
                        l_CheckBox = l_DatagridItem.FindControl("CheckBoxSelect")
                        Me.TotalAmountSelected += CType(l_DatagridItem.Cells(EligibleBalance).Text, Decimal)
                        If l_CheckBox.Enabled Then
                            l_CheckBox.Checked = True
                        End If
                    Next
                    ButtonSelectAll.Text = "Select None"
                Else
                    For Each l_DatagridItem In DataGridCashOut.Items
                        l_CheckBox = l_DatagridItem.FindControl("CheckBoxSelect")
                        If l_CheckBox.Enabled Then
                            l_CheckBox.Checked = False
                        End If
                    Next
                    Me.SelectedCashouts = 0
                    Me.TotalAmountSelected = 0.0
                    ButtonSelectAll.Text = "Select All"
                End If

            End If

            'aparna -yren -3016
            Me.TextboxTotalAmtSelected.Text = "$" + Me.TotalAmountSelected.ToString()
            Me.TextBoxCshoutsSelected.Text = Me.SelectedCashouts.ToString()
            Me.TextboxTotalAmt.Text = "$" + Me.TotalAmount.ToString()
            Me.TextBoxCashoutList.Text = Me.TotalNoOfCashouts.ToString()
            'aparna -yren -3016
        Catch
            Throw
        End Try

    End Sub

    'To Bind DropDown
    Public Sub BindDropDown()
        Dim l_dataset As New DataSet

        Try
            'get the dataset from the database 
            objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
            l_dataset = objCashOutBOClass.GetAmountRange()

            If Not l_dataset Is Nothing Then
                Me.Session_datatable_DropDownData = l_dataset.Tables(0)

                If Not l_dataset.Tables(0) Is Nothing AndAlso l_dataset.Tables(0).Rows.Count > 0 Then
                    DropDownAmount.DataSource = l_dataset.Tables(0)
                    DropDownAmount.DataTextField = "Range"
                    DropDownAmount.DataValueField = "CashoutRefundType"
                    DropDownAmount.DataBind()
                    Me.DropDownAmount.Items.Insert(0, "-Select Range-")
                    Me.DropDownAmount.Items(0).Value = "0"
                End If

                'If Not l_dataset.Tables(1) Is Nothing AndAlso l_dataset.Tables(1).Rows.Count > 0 Then
                '    Session("CashoutBatches") = l_dataset.Tables(1)
                'End If
            End If
        Catch
            Throw
        Finally
            objCashOutBOClass = Nothing
        End Try
    End Sub

    Public Sub BindBatchGrid()
        Dim dtBatchRecords As DataTable
        Dim objCashoutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass
        Dim dv As DataView
        Dim strCashoutRange As String
        Dim dtDropdowndata As DataTable
        Try
            objCashoutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
            'Start:Anudeep A:2015.10.21 YRS-AT-2463 Added a to filter as per the cashout range
            dtDropdowndata = Session_datatable_DropDownData
            If HelperFunctions.isNonEmpty(dtDropdowndata) Then
                strCashoutRange = dtDropdowndata.Select("CashoutRefundType = '" + DropdownValue + "'")(0)("chvCashOutRptDesc").ToString()
            End If
            dtBatchRecords = objCashoutBOClass.GetDataTableCashoutBatchRecords(strCashoutRange)
            'End:Anudeep A:2015.10.21 YRS-AT-2463 Added a to filter as per the cashout range

            If Not dtBatchRecords Is Nothing AndAlso dtBatchRecords.Rows.Count > 0 Then
                dv = dtBatchRecords.DefaultView
                dv.Sort = "chvBatchid DESC"
                Session("CashoutBatchId") = dv.Item(0)("chvBatchid").ToString()
                Session("CashoutBatches") = dtBatchRecords
            End If
        Catch
            Throw
        Finally
            dtBatchRecords = Nothing
            objCashoutBOClass = Nothing
        End Try
    End Sub

    'Error Message - redirected to Errorpage
    Public Sub GetCatch(ByVal ex As Exception)
        Dim l_String_Exception_Message As String
        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    End Sub

    'SP 2014.10.07 BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS -Start
    Private Sub ExpiredRefRequestsForSelectedPerson(ByVal XmlSelectedPersDetails As String)
        Dim objCashOutDaClass As YMCARET.YmcaBusinessObject.CashOutBOClass
        Try
            objCashOutDaClass = New YMCARET.YmcaBusinessObject.CashOutBOClass()
            objCashOutDaClass.ExpiredRefRequestsForSelectedPerson(XmlSelectedPersDetails)
        Catch
            Throw
        End Try
    End Sub
    Private Function GetXmlPerDetailsFromDataSet(ByVal dtSelectedBatchRecords As DataTable) As String
        Dim sbOutput As StringBuilder
        Try
            sbOutput = New StringBuilder()
            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                sbOutput.Append("<PersonDetails>")
                For Each dr As DataRow In dtSelectedBatchRecords.Rows
                    sbOutput.Append("<Pers>")
                    sbOutput.Append("<FundEventId>" + dr("FundEventId").ToString + "</FundEventId>")
                    sbOutput.Append("<PlanType>" + dr("PlansType").ToString + "</PlanType>")
                    sbOutput.Append("</Pers>")
                Next
                sbOutput.Append("</PersonDetails>")
            End If
        Catch ex As Exception

        End Try
        Return sbOutput.ToString()
    End Function
    'SP 2014.10.07 BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS -End

#End Region

    'START: Added By SG: BT-960
    Private Function ClearDataNControls()
        'Me.DataGridCashOut.Visible = False
        Me.DataGridCashOut.DataSource = Nothing
        Me.DataGridCashOut.DataBind()
        Me.ButtonSelectAll.Enabled = False
        Me.SelectedCashouts = 0
        Me.TotalNoOfCashouts = 0
        Me.TotalAmountSelected = 0.0
        Me.TotalAmount = 0.0
        Me.LabelCashouts.Visible = False
        Me.LabelSelectedCashouts.Visible = False
        Me.LabelTotal.Visible = False
        Me.LabelSelectedAmt.Visible = False
        Me.TextBoxCashoutList.Visible = False
        Me.TextBoxCshoutsSelected.Visible = False
        Me.TextboxTotalAmt.Visible = False
        Me.TextboxTotalAmtSelected.Visible = False
        Me.ButtonUpdateCounter.Enabled = False
        Me.ButtonProcess.Enabled = False
        Me.ButtonReport.Enabled = False
        Me.LabelNoRecords.Visible = False

        'Added By SG: 2013.01.02
        Session("FilterDatatable") = Nothing
    End Function

    'END: Added By SG: BT-960

    'START: Added By SG: BT-960: 2012.08.23
    Private Sub DataGridCashOut_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridCashOut.SortCommand

        Try

            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            Dim l_dtFilter As DataTable
            'Dim stTableName As String

            'stTableName = GetTableName()

            If Not Session("SegregateDatatable") Is Nothing Then
                l_dtFilter = TryCast(Session("SegregateDatatable"), DataTable)
            ElseIf Not Session("FilterDatatable") Is Nothing Then
                l_dtFilter = TryCast(Session("FilterDatatable"), DataTable)
            End If

            If Not l_dtFilter Is Nothing AndAlso l_dtFilter.Rows.Count > 0 Then
                'If Not Me.Session_DataSet_Cashouts Is Nothing Then
                'g_dataset_Cashouts = Me.Session_DataSet_Cashouts

                'dv = Me.g_dataset_Cashouts.Tables(stTableName).DefaultView
                dv = l_dtFilter.DefaultView
                'Priya Patil 08.11.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
                'dv.Sort = SortExpression
                dv.Sort = SortExpression + " ASC"

                'END Priya Patil 08.11.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
            End If

            If Not Session("CashoutListSort") Is Nothing Then
                If Session("CashoutListSort").ToString.Trim.EndsWith(" ASC") Then
                    ' If SortExpression + " ASC" = Session("CashoutListSort").ToString.Trim Then
                    dv.Sort = SortExpression + " DESC"
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
            Else
                'Priya Patil 08.11.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
                'dv.Sort = SortExpression + " ASC"
                If SortExpression.ToString.Trim.EndsWith(" ASC") Then
                    dv.Sort = SortExpression + " DESC"
                    'Priya 23-Nov-2012  BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Coding to sort
                ElseIf dv.Sort = SortExpression + " ASC" Then
                    dv.Sort = SortExpression + " DESC"
                    'END Priya 23-Nov-2012  BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Coding to sort
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                'END Priya Patil 08.11.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
            End If
            Session("CashoutListSort") = dv.Sort
            'BindDataintheGrid(stTableName)

            If Not l_dtFilter Is Nothing AndAlso l_dtFilter.Rows.Count > 0 Then
                DataGridCashOut.DataSource = dv
                DataGridCashOut.DataBind()
                LabelNoRecords.Visible = False
            Else
                LabelNoRecords.Visible = True
            End If

            'Added By SG: 2012.01.04: BT-1511
            Me.TotalAmountSelected = 0
            Me.SelectedCashouts = 0

            TextboxTotalAmtSelected.Text = "0"
            TextBoxCshoutsSelected.Text = "0"
        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub




    Private Function GetTableName() As String
        Dim stTableName As String
        If Me.DropdownValue = CashoutRange.Range2.ToString() AndAlso Not Session("CashOutReqType") Is Nothing AndAlso Session("CashOutReqType") = ProcessState.Process.ToString() Then
            stTableName = "RequestedParticipants"
        Else
            stTableName = "EligibleParticipants"
        End If

        Return stTableName
    End Function

    'Added By SG: 2012.09.03: BT-960
    Dim IDM As IDMforAll

    Public Function CallReportsForUnder50() As String
        Dim l_Dataset As DataSet
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_double_totalamtforreleaseblnk As Double = 0.0
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String
        Dim dtSelectedBatchRecords As DataTable

        Try
            IDM = New IDMforAll

            'create the Datatable -Filelist
            If IDM.DatatableFileList(False) Then
                Session("FTFileList") = IDM.SetdtFileList
            Else
                Throw New Exception("Unable to generate Release Blanks, Could not create dependent table")
            End If

            'Generate Release Blank Report.
            'get Which report should be called-base on the total Amount

            'Total Amount = Funded + Unfunded
            'RefRequestIds = CType(Session("RefundRequestIDs"), ArrayList)

            If Not Session("dtSelectedBatchRecords") Is Nothing Then
                dtSelectedBatchRecords = Session("dtSelectedBatchRecords")

                For Each row In dtSelectedBatchRecords.Rows
                    Try
                        'Priya Patil 23.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Added if requestid is not null
                        'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                        'If Not IsNull(row("RefRequestID")) Then
                        If Not String.IsNullOrEmpty(row("RefRequestID")) Then
                            'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                            l_Dataset = YMCARET.YmcaBusinessObject.RefundRequest.GetWithdrawalReportData(row("RefRequestID").ToString().Trim())

                            If Not l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
                                Session("PersonID") = l_Dataset.Tables("atsRefundRequest").Rows(0)("PersID").ToString()
                            End If

                            If Not l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
                                l_double_totalamtforreleaseblnk = l_double_totalamtforreleaseblnk + Convert.ToDouble(l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString())
                            End If

                            l_StringReportName = "Under$50CashoutStandardLetter.rpt"
                            l_stringDocType = "REFLTTRS"

                            l_ArrListParamValues.Add(row("RefRequestID").ToString().Trim())

                            l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType

                            l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, row("RefRequestID").ToString().Trim())

                            l_ArrListParamValues.Clear()
                        End If
                    Catch
                        Throw
                    Finally
                        If Not l_Dataset Is Nothing Then
                            l_Dataset.Dispose()
                        End If

                        l_double_totalamtforreleaseblnk = Nothing
                    End Try
                Next
                'Start:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                Session("Under$50CashoutStandardLetter") = "Under$50CashoutStandardLetter"
                'If Not Session("FTFileList") Is Nothing Then
                '    Try
                '        ' Call the calling of the ASPX to copy the file.
                '        Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                '        "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                '        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                '        "</script>"
                '        If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                '            Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                '        End If

                '    Catch ex As Exception
                '        'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
                '        'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                '        HelperFunctions.ShowMessageToUser("There was an error while copying documents to IDM.", EnumMessageTypes.Information)
                '        Exit Function
                '    End Try
                'End If
                'End:Dinesh Kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
            End If

            CallReportsForUnder50 = l_StringErrorMessage

        Catch ex As Exception
            Dim l_string_message As String = "Error Occured while generating reports"
            Session("GenerateErrors") = "Error: " + l_string_message
            CallReportsForUnder50 = l_string_message
        Finally
            IDM = Nothing
        End Try
    End Function

    Public Function CallReports() As String
        Dim l_Dataset As DataSet
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_double_totalamtforreleaseblnk As Double = 0.0
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String
        Dim dtSelectedBatchRecords As DataTable

        Try
            IDM = New IDMforAll

            'create the Datatable -Filelist
            If IDM.DatatableFileList(False) Then
                Session("FTFileList") = IDM.SetdtFileList
            Else
                Throw New Exception("Unable to generate Release Blanks, Could not create dependent table")
            End If

            If Not Session("dtSelectedBatchRecords") Is Nothing Then
                dtSelectedBatchRecords = Session("dtSelectedBatchRecords")
                dtSelectedBatchRecords.Columns.Add("DocType")
                dtSelectedBatchRecords.Columns.Add("ReportName")
                dtSelectedBatchRecords.Columns.Add("Param1")
                dtSelectedBatchRecords.Columns.Add("OutputFileType")
                dtSelectedBatchRecords.Columns.Add("AppType")
                dtSelectedBatchRecords.Columns.Add("YMCANO")
                dtSelectedBatchRecords.Columns.Add("YMCAName")
                dtSelectedBatchRecords.Columns.Add("YCMAState")
                dtSelectedBatchRecords.Columns.Add("YMCACity")

                For Each row In dtSelectedBatchRecords.Rows
                    row("DocType") = "REFREQST"
                    'Start:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                    'row("ReportName") = "Withdrawals_New.rpt"
                    row("ReportName") = "New Cashout Letter"
                    'End:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                    row("Param1") = "BIE"
                    row("OutputFileType") = "Withdrawal_REFREQST"
                    row("AppType") = "P"
                    row("YMCANO") = ""
                    row("YMCAName") = ""
                    row("YCMAState") = ""
                    row("YMCACity") = ""
                Next

                'For Each row In dtSelectedBatchRecords.Rows
                '    Try
                '        'Priya Patil 23.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Added if requestid is not null
                '        If row("RefRequestID").ToString <> String.Empty OrElse row("RefRequestID").ToString <> "" Then
                '            l_Dataset = YMCARET.YmcaBusinessObject.RefundRequest.GetWithdrawalReportData(row("RefRequestID").ToString().Trim())
                '            If Not l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
                '                Session("PersonID") = l_Dataset.Tables("atsRefundRequest").Rows(0)("PersID").ToString()
                '            End If

                '            'l_stringDocType = "REFREQST"
                '            'l_StringReportName = "Withdrawals_New.rpt"
                '            'l_ArrListParamValues.Add(row("RefRequestID").ToString().Trim())
                '            'l_ArrListParamValues.Add("BIE")
                '            'l_string_OutputFileType = "Withdrawal_" & l_stringDocType
                '            row("DocType") = "REFREQST"
                '            row("ReportName") = "Withdrawals_New.rpt"
                '            row("Param1") = "BIE"
                '            row("OutputFileType") = "Withdrawal_REFREQST"
                '            row("AppType") = "P"
                '            row("YMCANO") = ""
                '            row("YMCAName") = ""
                '            row("YCMAState") = ""
                '            row("YMCACity") = ""

                '            'l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, row("RefRequestID").ToString().Trim())
                '            ''l_ArrListParamValues.Clear()
                '        End If
                '    Catch
                '        Throw
                '    Finally
                '        If Not l_Dataset Is Nothing Then
                '            l_Dataset.Dispose()
                '        End If

                '        l_double_totalamtforreleaseblnk = Nothing
                '    End Try

                'Next

                Try
                    If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                        If dtSelectedBatchRecords.Rows.Count > 0 Then
                            l_StringErrorMessage = Me.SetPropertiesForIDM(dtSelectedBatchRecords)
                        End If
                    End If
                Catch
                    Throw
                Finally
                    If Not l_Dataset Is Nothing Then
                        l_Dataset.Dispose()
                    End If
                    l_double_totalamtforreleaseblnk = Nothing
                End Try

                If Not Session("FTFileList") Is Nothing Then
                    Try
                        ' Call the calling of the ASPX to copy the file.
                        Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                        "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                        "</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                            Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                        End If

                    Catch ex As Exception
                        'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
                        'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                        HelperFunctions.ShowMessageToUser("There was an error while copying documents to IDM.", EnumMessageTypes.Information)
                        Exit Function
                    End Try
                End If
            End If

            CallReports = l_StringErrorMessage

        Catch ex As Exception
            Dim l_string_message As String = "Error Occured while generating reports"
            Session("GenerateErrors") = "Error: " + l_string_message
            CallReports = l_string_message
        Finally
            IDM = Nothing
        End Try
    End Function

    'Added By SG: 2012.09.03: BT-960
    Private Function SetPropertiesForIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String, ByVal l_RefRequestsID As String) As String
        Dim l_StringErrorMessage As String = String.Empty

        Try
            If IDM Is Nothing Then
                IDM = New IDMforAll
            End If

            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"

            If Not Session("PersonID") Is Nothing Then
                IDM.PersId = DirectCast(Session("PersonID"), String)
            End If

            If Not Session("FTFileList") Is Nothing Then
                IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If

            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName
            IDM.ReportParameters = l_ArrListParamValues
            IDM.RefRequestsID = l_RefRequestsID

            l_StringErrorMessage = IDM.ExportToPDF()

            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList

        Catch
            Throw
        End Try
    End Function

    'Added By SG: 2012.09.03: BT-960
    Private Function SetPropertiesForIDM(ByVal dtGenerateCashOutPdf As DataTable) As String
        Dim l_StringErrorMessage As String = String.Empty
        Try
            If IDM Is Nothing Then
                IDM = New IDMforAll
            End If

            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"

            'If Not Session("PersonID") Is Nothing Then
            '    IDM.PersId = DirectCast(Session("PersonID"), String)
            'End If

            'If Not Session("FTFileList") Is Nothing Then
            '    IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            'End If

            'IDM.DocTypeCode = l_StringDocType
            'IDM.OutputFileType = l_string_OutputFileType
            'IDM.ReportName = l_StringReportName
            'IDM.ReportParameters = l_ArrListParamValues
            'IDM.RefRequestsID = l_RefRequestsID

            Dim tParam As New Threading.ParameterizedThreadStart(AddressOf IDM.ExportToPDF)
            'Dim objArray As New ArrayList()
            'objArray.Add(dtGenerateCashOutPdf)
            'objArray.Add(IDM)
            'Dim t As New Threading.Thread(AddressOf Sample)

            't.Start()

            l_StringErrorMessage = IDM.ExportToPDF()

            ' Page.RegisterStartupScript("CALLAJAX", "<script> CalAjax(); </" + "script>")
            ' l_StringErrorMessage = IDM.ExportPdf
            'l_ArrListParamValues.Clear()

            'Session("FTFileList") = IDM.SetdtFileList

        Catch
            Throw
        End Try
    End Function

    'Public Sub Sample(ByVal obj As ArrayList)
    '    'If IDM Is Nothing Then
    '    '    IDM = New IDMforAll
    '    'End If
    '    Dim objIDM As IDMforAll
    '    objIDM = obj.Item(1)
    '    Session("iProcessCount") = "2321332213" + Session("iProcessCount")
    '    Thread.Sleep(5000)
    '    objIDM.ExportToPDF(obj.Item(0), Session)
    'End Sub


    <System.Web.Services.WebMethod(True)> _
    Public Shared Function getpageStatus() As String
        Return Convert.ToString(HttpContext.Current.Session("iProcessCount"))
    End Function

    'Added By SG: 2012.09.03: BT-960
    Private Sub SendMail(ByVal strBalance As Integer, ByVal strRMDPayment As Integer, ByVal stramount1 As Decimal, ByVal stramount2 As Decimal)
        Dim objMail As MailUtil
        Dim strbMessage As StringBuilder

        Try
            objMail = New MailUtil

            objMail.MailCategory = "CASOUT"
            If objMail.MailService = False Then Exit Sub

            strbMessage = New StringBuilder()

            If strBalance > 0 Or stramount1 > 0 Then
                strbMessage.Append(GetMessageFromResource("EMAIL_CONTENT_MILLE_TEXT").Replace("@MilleCountTransfer", strBalance.ToString()).Replace("@MilleAmountTransfer", stramount1.ToString("#,0.00")))
                'strbMessage.Append(strBalance.ToString() & " balances transferred to Millenium Bank for a total of " & stramount1.ToString() & ".")
                strbMessage.Append(ControlChars.CrLf & ControlChars.CrLf)
            End If

            If strRMDPayment > 0 Or stramount2 > 0 Then
                strbMessage.Append(GetMessageFromResource("EMAIL_CONTENT_RMD_TEXT").Replace("@RmdCountTransfer", strRMDPayment.ToString()).Replace("@RmdAmountTransfer", stramount2.ToString("#,0.00")))
                'strbMessage.Append(strRMDPayment.ToString() & " RMD payments generated totaling " & stramount2.ToString() & ".")
                strbMessage.Append(ControlChars.CrLf & ControlChars.CrLf)
            End If

            objMail.MailMessage = strbMessage.ToString()

            objMail.Subject = "Cash out payment register for " & String.Format("{0:MM/dd/yyyy}", DateTime.Now) & "."

            objMail.Send()

        Catch
            Throw
        Finally
            objMail = Nothing
            strbMessage = Nothing
        End Try
    End Sub

    Private Function GetMessageFromResource(ByVal l_string_MessageCode) As String

        Dim l_string_Message As String = String.Empty

        If l_string_MessageCode = "EMAIL_CONTENT_MILLE_TEXT" Then
            l_string_Message = Resources.CashoutRefundMessage.EMAIL_CONTENT_MILLE_TEXT
        ElseIf l_string_MessageCode = "EMAIL_CONTENT_RMD_TEXT" Then
            l_string_Message = Resources.CashoutRefundMessage.EMAIL_CONTENT_RMD_TEXT
        End If

        Return l_string_Message

    End Function

    'Added By SG: 2012.09.21: BT-960
    Private Sub BindUpdatePanelGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkEligibleList.Click, LinkNotEligibleList.Click
        Dim dtCashout As New DataTable
        Dim drFilterCashout As DataRow()
        'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
        'added new drFilterNotEligibleCashout to get not eligibleparticipant count to view link 
        Dim drFilterNotEligibleCashout As DataRow()
        'end Priya 23.10.2012: YRS 5.0-1489
        Dim stQuery As String
        Dim lnkTempSender As LinkButton = sender

        Try
            If lnkTempSender.ID = LinkEligibleList.ID Then
                stQuery = "Remarks = ''"
                LinkEligibleList.Visible = False
                LinkNotEligibleList.Visible = True
                'tdParticipantPanel.InnerText = "List of Eligible Participants"
                tbUpdateCounter.Visible = True
                Me.TotalAmount = 0
            ElseIf lnkTempSender.ID = LinkNotEligibleList.ID Then
                stQuery = "Remarks <> ''"
                LinkEligibleList.Visible = True
                LinkNotEligibleList.Visible = False
                'tdParticipantPanel.InnerText = "List of Non Eligible Participants"
                tbUpdateCounter.Visible = False
            End If

            If Not Me.Session_DataSet_Cashouts Is Nothing Then
                dtCashout = Me.Session_DataSet_Cashouts.Tables(GetTableName())
            Else
                LabelNoRecords.Visible = True
            End If

            If Not dtCashout Is Nothing AndAlso dtCashout.Rows.Count > 0 Then
                drFilterCashout = dtCashout.Select(stQuery)

                If Not drFilterCashout Is Nothing AndAlso drFilterCashout.Length > 0 Then
                    'Added By SG: 2013.01.02
                    Session("FilterDatatable") = drFilterCashout.CopyToDataTable()

                    DataGridCashOut.DataSource = drFilterCashout.CopyToDataTable()
                    DataGridCashOut.DataBind()
                    LabelNoRecords.Visible = False

                    If lnkTempSender.ID = LinkEligibleList.ID Then
                        Me.TotalNoOfCashouts = drFilterCashout.CopyToDataTable().Rows.Count

                        Dim nAmount As Object
                        nAmount = drFilterCashout.CopyToDataTable().Compute("SUM(EligibleBalance)", "")
                        Me.TotalAmount = CType(nAmount, Decimal)

                        TextBoxCashoutList.Text = Me.TotalNoOfCashouts.ToString()
                        TextboxTotalAmt.Text = "$" + nAmount.ToString()
                    End If
                Else
                    'Added By SG: 2013.01.02
                    Session("FilterDatatable") = Nothing

                    LabelNoRecords.Visible = True
                End If

                'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                'added new drFilterNotEligibleCashout to get not eligibleparticipant count to view link 
                If bool_Calling_Command = True Then
                    drFilterNotEligibleCashout = dtCashout.Select("Remarks <> ''")
                End If
                'end Priya 23.10.2012: YRS 5.0-1489
            Else
                LabelNoRecords.Visible = True
            End If

            If bool_Calling_Command = True Then
                'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                'added new drFilterNotEligibleCashout to get not eligibleparticipant count to view link 
                If Not drFilterNotEligibleCashout Is Nothing AndAlso drFilterNotEligibleCashout.Length > 0 Then
                    bool_Calling_Command = False
                    LinkNotEligibleList.Visible = True
                    LinkEligibleList.Visible = False
                    'LinkNotEligibleList.Enabled = False
                    'LinkNotEligibleList.Text = "Not Eligible Participants"
                Else
                    LinkNotEligibleList.Visible = False
                End If
            End If
            'end Priya 23.10.2012: YRS 5.0-1489

            If LabelNoRecords.Visible = True Then
                DataGridCashOut.DataSource = Nothing
                DataGridCashOut.DataBind()

                'Added By SG: 2013.01.02
                Session("FilterDatatable") = Nothing
                Me.TotalNoOfCashouts = 0
                Me.TotalAmount = 0
                TextboxTotalAmt.Text = "0"
                TextBoxCashoutList.Text = "0"
            End If

            'Added By SG: 2012.01.04: BT-1511
            labelFilter.Text = ""
            Session("SegregateDatatable") = Nothing
        Catch
            Throw
            'GetCatch(ex)
        Finally
            dtCashout = Nothing
            drFilterCashout = Nothing
        End Try
    End Sub

    Private Sub BindBatchGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkUnprocessBatch.Click, LinkProcessBatch.Click
        Dim dtBatch As New DataTable
        Dim drFilterBatch As DataRow()
        Dim stQuery As String
        Dim lnkTempSender As LinkButton = sender
        Dim dv As DataView

        Try
            If lnkTempSender.ID = LinkProcessBatch.ID Then
                LinkProcessBatch.Visible = False
                LinkUnprocessBatch.Visible = True
                stQuery = "ProcessedDate IS NOT NULL"
                tdBatchPanel.InnerText = "List of Processed Batches"
                trColorMessage.Visible = False

                Me.GridViewCashoutBatch.AllowPaging = True
            Else
                LinkProcessBatch.Visible = True
                LinkUnprocessBatch.Visible = False
                stQuery = "ProcessedDate IS NULL"
                tdBatchPanel.InnerText = "List of Unprocessed Batches"
                trColorMessage.Visible = True

                Me.GridViewCashoutBatch.AllowPaging = False
            End If

            If Not Session("CashoutBatches") Is Nothing Then
                dtBatch = CType(Session("CashoutBatches"), DataTable)
                If Not dtBatch Is Nothing AndAlso dtBatch.Rows.Count > 0 Then
                    drFilterBatch = dtBatch.Select(stQuery)
                    If Not drFilterBatch Is Nothing AndAlso drFilterBatch.Length > 0 Then
                        Dim strSortExpression As String
                        strSortExpression = "CreatedDate DESC"

                        Session("CashoutFilterBatches") = drFilterBatch.CopyToDataTable()
                        Session("CashoutBatchSort") = strSortExpression

                        dv = drFilterBatch.CopyToDataTable().DefaultView
                        dv.Sort = strSortExpression

                        Me.GridViewCashoutBatch.DataSource = dv
                        Me.GridViewCashoutBatch.DataBind()

                        'If lnkTempSender.ID = LinkProcessBatch.ID Then
                        '    Me.SessionPageCount = Me.GridViewCashoutBatch.PageCount
                        'End If

                        LabelNoBatch.Visible = False
                    Else
                        LabelNoBatch.Visible = True
                    End If
                Else
                    LabelNoBatch.Visible = True
                End If
            Else
                LabelNoBatch.Visible = True
            End If

            If LabelNoBatch.Visible = True Then
                Session("CashoutFilterBatches") = Nothing
                Me.GridViewCashoutBatch.DataSource = Nothing
                Me.GridViewCashoutBatch.DataBind()
            End If
        Catch
            Throw
        Finally
            dv = Nothing
            dtBatch = Nothing
            drFilterBatch = Nothing
            lnkTempSender = Nothing
        End Try
    End Sub

    Private Sub CreateBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCreateBatch.Click
        Try
            'Added by SG: 2012.11.27: BT-960
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(sender.ID.ToString(), Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : BT-960

            CashoutTabStrip.Items.Item(0).Enabled = True
            CashoutTabStrip.SelectedIndex = 1
            CashoutMultiPage.SelectedIndex = 1
            DataGridCashOut.DataSource = Nothing
            DataGridCashOut.DataBind()
            LinkEligibleList.Visible = False
            LinkNotEligibleList.Visible = False
            Session("CashOutReqType") = ProcessState.Request.ToString()
            ButtonGetData.Enabled = True

            'Added By SG: 2013.01.02
            Session("FilterDatatable") = Nothing

            'tdParticipantPanel.InnerText = ""
            'Commented by Dinesh Kanojia to incorporate master file
            'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
            'labelBatchID.Text = ""

            ButtonProcess.Text = "Save Batch"
            tbUpdateCounter.Visible = True

            'ButtonGetData_Click(sender, e)

            ClearDataNControls()

            FetchCashoutData()

            'Added By SG: 2012.01.04: BT-1511
            labelFilter.Text = ""
            Session("SegregateDatatable") = Nothing

            'Need to show counter button for record selection in a gridview.
            Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                                   "ShowHideButton('block')</" & "script" & ">"
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript21", popupScript3)
            End If

        Catch
            ' GetCatch(ex)
            Throw
        End Try
    End Sub

    Private Function FetchCashoutData()
        Dim l_string_Selectedvalue As String
        Dim l_DataTable_DropdownData As New DataTable
        Dim l_String_Search As String
        Dim l_Find_Datarow As DataRow()
        Dim l_Double_LowerLimit As Double
        Dim l_Double_UpperLimit As Double
        Dim l_String_CashOutRptDesc As String
        Dim l_string_UserId As String
        Dim l_string_IPAddress As String

        Try

            Me.TotalNoOfCashouts = 0
            Me.SelectedCashouts = 0
            Me.TotalAmount = 0.0
            Me.TotalAmountSelected = 0.0
            'Commented by Aparna -Wrong Placing of code - 07/02/2007
            'aparna yren -3016
            'Me.LabelCashouts.Visible = True
            'Me.LabelSelectedCashouts.Visible = True
            'Me.LabelTotal.Visible = True
            'Me.LabelSelectedAmt.Visible = True
            'Me.TextBoxCashoutList.Visible = True
            'Me.TextBoxCshoutsSelected.Visible = True
            'Me.TextboxTotalAmt.Visible = True
            'Me.TextboxTotalAmtSelected.Visible = True
            'aparna yren -3016
            'Commented by Aparna -Wrong Placing of code - 07/02/2007


            'Deleting the data from Log table for the particular user
            'objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
            'objCashOutBOClass.DeleteReportData(Session("UserId"), Session("IPAddress"))
            'objCashOutBOClass = Nothing
            l_string_UserId = Session("LoginId")
            l_string_IPAddress = Session("IPAddress")

            l_DataTable_DropdownData = Me.Session_datatable_DropDownData
            l_string_Selectedvalue = Me.DropdownValue
            'Search for the lower and upper limit of the Selected item in drop down to be sent to BO layer
            If Not Me.DropdownValue.ToString = "0" Then
                l_String_Search = "CashoutRefundType = '" + l_string_Selectedvalue + "'"
                l_Find_Datarow = l_DataTable_DropdownData.Select(l_String_Search)


                If l_Find_Datarow.Length > 0 Then
                    l_Double_LowerLimit = l_Find_Datarow(0)("numLowerLimit")
                    l_Double_UpperLimit = l_Find_Datarow(0)("numUpperLimit")
                    l_String_CashOutRptDesc = l_Find_Datarow(0)("chvCashOutRptDesc")
                    'Setting up teh session value for reports
                    Session("CashOutRangeDesc") = l_String_CashOutRptDesc
                    objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass

                    Me.Session_DataSet_Cashouts = objCashOutBOClass.GetEligibleParticipants(l_Double_LowerLimit, l_Double_UpperLimit, l_String_CashOutRptDesc, l_string_IPAddress, l_string_UserId)

                    objCashOutBOClass = Nothing
                    'Binds The datagrid
                    BindDataintheGrid(GetTableName())
                Else
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please select a valid range to proceed", MessageBoxButtons.Stop)
                    'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                    HelperFunctions.ShowMessageToUser("Please select a valid range to proceed.", EnumMessageTypes.Error)
                End If
                'get the required data for filling up the datagrid
            Else
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please select a range to proceed", MessageBoxButtons.Stop)
                'Reopen - 09/04/2014 : BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                HelperFunctions.ShowMessageToUser("Please select a range to proceed.", EnumMessageTypes.Error)
            End If

        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Function

    Protected Sub CashoutTabStrip_SelectedIndexChange(ByVal sender As Object, ByVal e As EventArgs) Handles CashoutTabStrip.SelectedIndexChange
        Dim dtBatch As DataTable
        Dim dtFilterBatch As DataTable
        Dim drFilterBatch() As DataRow 'Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
        Try
            Me.CashoutMultiPage.SelectedIndex = Me.CashoutTabStrip.SelectedIndex
            If CashoutTabStrip.Items(CashoutTabStrip.SelectedIndex).Text = "Manage Batch" Then
                If Not Session("CashoutBatches") Is Nothing Then
                    dtBatch = Session("CashoutBatches")
                    'Start: Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
                    'If Not dtBatch Is Nothing AndAlso dtBatch.Rows.Count > 0 Then
                    If HelperFunctions.isNonEmpty(dtBatch) Then
                        drFilterBatch = dtBatch.Select("ProcessedDate IS NULL")
                        If drFilterBatch.Length > 0 Then
                            dtFilterBatch = dtBatch.Select("ProcessedDate IS NULL").CopyToDataTable()
                            'If Not dtFilterBatch Is Nothing AndAlso dtFilterBatch.Rows.Count > 0 Then
                            If HelperFunctions.isNonEmpty(dtFilterBatch) Then
                                'End:  Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
                                'Priya Patil 30.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                                LinkUnprocessBatch.Visible = False
                                LinkProcessBatch.Visible = True
                                'END Priya Patil 30.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                                Session("CashoutFilterBatches") = dtFilterBatch

                                'Added By SG: 2012.12.18: BT-1511:
                                Me.GridViewCashoutBatch.AllowPaging = False

                                Me.GridViewCashoutBatch.DataSource = dtFilterBatch
                                Me.GridViewCashoutBatch.DataBind()
                                trColorMessage.Visible = True
                                LabelNoBatch.Visible = False
                                tdBatchPanel.InnerText = "List of Unprocessed Batches"
                                'Priya Patil 23.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                                'To show selected row after select batch
                                'Dim ImageSelect As Image
                                'If HelperFunctions.isNonEmpty(Session_DataSet_Cashouts) Then
                                '	Dim strBatchID As String = Session_DataSet_Cashouts.Tables(0).Rows(0)("BatchID").ToString()
                                '	'ImageSelected = GridViewCashoutBatch.Rows(e.CommandArgument).FindControl("ImageSelected")
                                '	If e.Row.Cells(1).Text = strBatchID Then 'Cells(1) = batch ID
                                '		ImageSelect = e.Row.FindControl("ImageSelect")
                                '		If Not ImageSelect Is Nothing Then
                                '			ImageSelect.ImageUrl = "images\selected.gif"
                                '			ImageSelect.ToolTip = "Selected"
                                '			ImageSelect.AlternateText = "Selected"
                                '		End If
                                '	End If
                                'End If
                                'END Priya Patil 23.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                            Else
                                LabelNoBatch.Visible = True
                            End If
                        End If
                    Else
                        LabelNoBatch.Visible = True
                    End If
                Else
                    LabelNoBatch.Visible = True
                End If

                If LabelNoBatch.Visible = True Then
                    GridViewCashoutBatch.DataSource = Nothing
                    GridViewCashoutBatch.DataBind()
                End If
                'Priya Patil 30.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000  
                'commented line of code and put aboe in if condition
                'LinkUnprocessBatch.Visible = False
                'LinkProcessBatch.Visible = True
                'END Priya Patil 30.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
            Else

            End If
        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Dim bool_Calling_Command As Boolean
    Private Sub DataGridCashoutBatch_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewCashoutBatch.RowCommand
        Dim objCashOutsBO As New YMCARET.YmcaBusinessObject.CashOutBOClass()
        Try
            Dim BatchID As String
            Dim intIndex As Integer

            If e.CommandName = "Process" Then
                intIndex = Convert.ToInt32(e.CommandArgument)
                BatchID = GridViewCashoutBatch.DataKeys(intIndex).Value.ToString()

                CashoutTabStrip.Items.Item(0).Enabled = True
                CashoutTabStrip.SelectedIndex = 1
                CashoutMultiPage.SelectedIndex = 1
                Session("CashOutReqType") = ProcessState.Process.ToString()
                ButtonGetData.Enabled = False
                Session("CashoutBatchId") = BatchID
                Me.Session_DataSet_Cashouts = objCashOutsBO.GetRequestedParticipantForProcessing(BatchID)
                ClearDataNControls()
                'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                'added new drFilterNotEligibleCashout to get not eligibleparticipant count to view link 
                'LinkNotEligibleList.Visible = True
                'LinkEligibleList.Visible = False
                bool_Calling_Command = True
                BindUpdatePanelGrid(LinkEligibleList, e)

                'END Priya 23.10.2012: YRS 5.0-1489
                tbUpdateCounter.Visible = True
                'tdParticipantPanel.InnerText = "List of Eligible Participants"

                'tdParticipantPanel.InnerText = "Batch Details: (Batch ID - " + BatchID + ")"
                'Commented by Dinesh Kanojia to incorporate master file
                'BT-2324: YRS 5.0-2267 - Changes to Cashout master page
                'labelBatchID.Text = "> Batch ID - " + BatchID

                ButtonProcess.Text = "Process Batch"
                BindDataintheGrid(GetTableName())
                'Added By SG: 2012.11.26: BT-960
                ButtonSelectAll.Text = "Select All"

                'Added By SG: 2012.01.04: BT-1511
                labelFilter.Text = ""
                Session("SegregateDatatable") = Nothing



            ElseIf e.CommandName = "ViewReport" Then
                'intIndex = Convert.ToInt32(e.CommandArgument)
                'BatchID = GridViewCashoutBatch.DataKeys(intIndex).Value.ToString()
                'Added By SG: 2012.12.17: BT-1511
                BatchID = e.CommandArgument

                Session("CashoutBatchId") = BatchID
                OpenReportViewer()
                'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
            ElseIf e.CommandName = "Print" Then
                'Dim l_printBatchDdata As DataSet
                'intIndex = Convert.ToInt32(e.CommandArgument)
                'BatchID = GridViewCashoutBatch.DataKeys(intIndex).Value.ToString()
                'Added By SG: 2012.12.17: BT-1511
                BatchID = e.CommandArgument
                Session("PrintBatchID") = BatchID

                'l_printBatchDdata = objCashOutsBO.GetBatchDetails(BatchID)
                'Session("printBatchDdata") = l_printBatchDdata.Tables(0)
                If (Not Session("PrintBatchID") Is Nothing AndAlso Session("PrintBatchID").ToString().Length > 1) Then
                    'If (rdFormToPrint.SelectedValue <> String.Empty) Then

                    'Added By SG: 2012.12.05: BT-960
                    ' Session("strReportName") = "Withdrawals" 'rdFormToPrint.SelectedValue
                    'Start:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                    'Session("strReportName") = "Withdrawals_New"
                    Session("strReportName") = "New Cashout Letter"
                    'End:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                    Session("strModuleName") = "CashOut"
                    Me.PrintReport()
                    'End If
                    'END Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                End If
                'START: Added By SG: 2012.12.17: BT-1511
            ElseIf e.CommandName = "View" Then
                BatchID = e.CommandArgument
                Session("PrintBatchID") = BatchID

                If (Not Session("PrintBatchID") Is Nothing AndAlso Session("PrintBatchID").ToString().Length > 1) Then
                    'Start:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                    'Session("strReportName") = "Withdrawals_New"
                    Session("strReportName") = "New Cashout Letter"
                    'End:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                    Session("strModuleName") = "CashOut"
                    'Me.PrintReport()
                    'Me.OpenReportViewer()
                    Dim popupScript As String = "<script language='javascript'>" & _
                        "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                        "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                        Page.RegisterStartupScript("PopupScript1", popupScript)
                    End If
                End If
            End If
            'END: Added By SG: 2012.12.17: BT-1511
            Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                                 "ShowHideButton('none')</" & "script" & ">"
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript21", popupScript3)
            End If
        Catch
            'GetCatch(ex)
            Throw
        Finally
            objCashOutsBO = Nothing
        End Try
    End Sub

    'END: Added By SG: BT-960: 2012.08.23

    Private Sub DataGridCashoutBatch_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewCashoutBatch.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                If LinkProcessBatch.Visible = True Then
                    Dim l_int_CreatedDiffDays As Integer
                    l_int_CreatedDiffDays = Convert.ToInt32(e.Row.Cells(9).Text) 'DaysCreatedDiff

                    If l_int_CreatedDiffDays >= 60 AndAlso l_int_CreatedDiffDays <= 70 Then
                        e.Row.BackColor = System.Drawing.Color.LightGreen
                    ElseIf l_int_CreatedDiffDays >= 71 AndAlso l_int_CreatedDiffDays <= 80 Then
                        e.Row.BackColor = System.Drawing.Color.Yellow
                    ElseIf l_int_CreatedDiffDays >= 81 AndAlso l_int_CreatedDiffDays <= 90 Then
                        e.Row.BackColor = System.Drawing.Color.Red
                    ElseIf l_int_CreatedDiffDays > 90 Then
                        e.Row.Cells(1).Text = e.Row.Cells(1).Text + " PAST DUE"
                    End If

                    'Priya Patil 23.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                    'To show selected row after select batch
                    Dim ImageSelect As Image
                    If HelperFunctions.isNonEmpty(Session_DataSet_Cashouts) Then
                        Dim strBatchID As String = Session_DataSet_Cashouts.Tables(0).Rows(0)("BatchID").ToString()
                        'ImageSelected = GridViewCashoutBatch.Rows(e.CommandArgument).FindControl("ImageSelected")
                        If e.Row.Cells(1).Text = strBatchID Then 'Cells(1) = batch ID
                            'Priya Patil 08.11.2012			YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Changes made for sorting
                            e.Row.Font.Bold = True
                            'ImageSelect = e.Row.FindControl("ImageSelect")
                            'If Not ImageSelect Is Nothing Then
                            '	ImageSelect.ImageUrl = "images\selected.gif"
                            '	ImageSelect.ToolTip = "Selected"
                            '	ImageSelect.AlternateText = "Selected"
                            'End If
                        End If
                    End If
                    'END Priya Patil 23.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 

                End If

            End If

            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                If LinkProcessBatch.Visible = True Then
                    e.Row.Cells(7).Visible = False 'ProcessedDate
                    e.Row.Cells(8).Visible = True 'Initiate Process
                    e.Row.Cells(10).Visible = False '  ViewReport
                    e.Row.Cells(9).Visible = False 'DaysCreatedDiff
                    'Me.tdBatchPanel.InnerText = "List of Unprocessed Batches"
                    'e.Row.Cells(4).Visible = True 'BatchCount
                    'e.Row.Cells(5).Visible = False 'BatchCountLookup
                    e.Row.Cells(4).Visible = False 'BatchCount
                    e.Row.Cells(5).Visible = True 'BatchCountLookup
                    e.Row.Cells(11).Visible = True  'Print
                    e.Row.Cells(6).Visible = False 'ProcessedCount
                    e.Row.Cells(12).Visible = True    'View
                Else
                    e.Row.Cells(8).Visible = False
                    e.Row.Cells(7).Visible = True
                    e.Row.Cells(10).Visible = True
                    e.Row.Cells(9).Visible = False
                    e.Row.Cells(4).Visible = False 'BatchCount
                    e.Row.Cells(5).Visible = True 'BatchCountLookup
                    e.Row.Cells(11).Visible = False 'Print
                    e.Row.Cells(6).Visible = True 'ProcessedCount
                    e.Row.Cells(12).Visible = False 'View
                End If
            End If
        Catch
            'GetCatch(ex)
            Throw
        End Try
    End Sub

    Protected Sub DataGridCashoutBatch1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridViewCashoutBatch.Sorting
        Try
            Dim dv As New DataView
            Dim dt As New DataTable
            Dim SortExpression As String
            SortExpression = e.SortExpression

            If Not Session("CashoutFilterBatches") Is Nothing Then
                dt = Session("CashoutFilterBatches")
                dv = dt.DefaultView
                dv.Sort = SortExpression
            End If

            If Not Session("CashoutBatchSort") Is Nothing Then
                'If SortExpression + " ASC" = Session("CashoutBatchSort").ToString.Trim Then
                If Session("CashoutBatchSort").ToString.Trim.EndsWith(" ASC") Then
                    '  If ViewState("Sort").ToString.Trim.EndsWith(" DESC") Then
                    dv.Sort = SortExpression + " DESC"
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
            Else
                dv.Sort = SortExpression + " ASC"
            End If

            Session("CashoutBatchSort") = dv.Sort

            GridViewCashoutBatch.DataSource = dv
            GridViewCashoutBatch.DataBind()
        Catch
            Throw
            ' GetCatch(ex)
        End Try
    End Sub

    Protected Sub DataGridCashoutBatch1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewCashoutBatch.PageIndexChanging
        Dim dv As New DataView
        Dim dt As New DataTable
        Dim SortExpression As String

        Try
            If Not Session("CashoutFilterBatches") Is Nothing Then
                dt = Session("CashoutFilterBatches")
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    dv = dt.DefaultView
                    If Not String.IsNullOrEmpty(Session("CashoutBatchSort")) Then
                        SortExpression = Session("CashoutBatchSort").ToString().Trim()
                        dv.Sort = SortExpression
                    End If

                    GridViewCashoutBatch.DataSource = dv
                    GridViewCashoutBatch.PageIndex = e.NewPageIndex
                    GridViewCashoutBatch.DataBind()
                End If
            End If
        Catch
            Throw
        End Try

    End Sub
    'This function is used to open opup for print page.
    Private Sub PrintReport()
        Dim popupScript As String
        popupScript = "<script language='javascript'>openReportPrinter()</script>"
        If (Not Me.IsStartupScriptRegistered("PopupPrintColor")) Then
            Page.RegisterStartupScript("PopupPrintColor", popupScript)
        End If
    End Sub

    'Added By SG: 2012.12.31
    Private Sub FilterRecordsOnCheckChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkAll.Click, linkHighlighted.Click, linkPlain.Click
        Dim stQuery As String = String.Empty

        If sender.ID.ToString() = "linkAll" Then
            stQuery = String.Empty
            labelFilter.Text = "ALL"
        ElseIf sender.ID.ToString() = "linkHighlighted" Then
            stQuery = "(IsHighlighted = 1 OR IsRMDEligible = True)"
            labelFilter.Text = "Highlighted"
        ElseIf sender.ID.ToString() = "linkPlain" Then
            stQuery = "(IsHighlighted = 0 AND IsRMDEligible = False)"
            labelFilter.Text = "Non-Highlighted"
        End If

        If LinkEligibleList.Visible = True AndAlso Not stQuery = String.Empty Then
            stQuery += " AND Remarks <> ''"
        ElseIf LinkEligibleList.Visible = True AndAlso stQuery = String.Empty Then
            stQuery += "Remarks <> ''"
        ElseIf LinkNotEligibleList.Visible = True AndAlso Not stQuery = String.Empty Then
            stQuery += " AND Remarks = ''"
        ElseIf LinkNotEligibleList.Visible = True AndAlso stQuery = String.Empty Then
            stQuery += "Remarks = ''"
        End If

        'Added By SG: 2013.01.02
        BindHighlightedData(stQuery)

        'Added By SG: 2012.01.04: BT-1511
        Me.TotalAmountSelected = 0
        Me.SelectedCashouts = 0

        TextboxTotalAmtSelected.Text = "0"
        TextBoxCshoutsSelected.Text = "0"
    End Sub

    'Added By SG: 2013.01.02
    Private Sub BindHighlightedData(ByVal stQuery As String)
        Dim dt As DataTable
        Dim dr() As DataRow

        Try
            If Not Session("FilterDatatable") Is Nothing Then
                dt = TryCast(Session("FilterDatatable"), DataTable)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    dr = dt.Select(stQuery)
                    If Not dr Is Nothing AndAlso dr.Length > 0 Then
                        'Added By SG: 2012.13.03: Datatable added in session for maintaining sorting functionality in grid
                        Session("SegregateDatatable") = dr.CopyToDataTable()

                        DataGridCashOut.DataSource = dr.CopyToDataTable()
                        DataGridCashOut.DataBind()
                        LabelNoRecords.Visible = False

                        Me.TotalNoOfCashouts = dr.CopyToDataTable().Rows.Count

                        Dim nAmount As Object
                        nAmount = dr.CopyToDataTable().Compute("SUM(EligibleBalance)", "")
                        Me.TotalAmount = CType(nAmount, Decimal)

                        TextBoxCashoutList.Text = Me.TotalNoOfCashouts.ToString()
                        TextboxTotalAmt.Text = "$" + nAmount.ToString()
                    Else
                        Session("SegregateDatatable") = Nothing
                        LabelNoRecords.Visible = True
                    End If
                Else
                    Session("SegregateDatatable") = Nothing
                    LabelNoRecords.Visible = True
                End If
            Else
                Session("SegregateDatatable") = Nothing
                LabelNoRecords.Visible = True
            End If

            'Added By SG: 2013.01.04
            If LabelNoRecords.Visible = True Then
                Session("SegregateDatatable") = Nothing

                DataGridCashOut.DataSource = Nothing
                DataGridCashOut.DataBind()

                Me.TotalNoOfCashouts = 0
                Me.TotalAmount = 0

                TextboxTotalAmt.Text = "0"
                TextBoxCashoutList.Text = "0"
            End If
        Catch ex As Exception
            Throw
        Finally
            dr = Nothing
            dt = Nothing
        End Try
    End Sub
    'Start/2014.11.28/SR/BT 2728: Cash out payment register mail shows wrong information
    'Following function will return Distinct records for GuiFundeventid column.
    Public Shared Function GetDistinctRecords(dt As DataTable, Columns As String()) As DataTable
        Dim dtUniqRecords As New DataTable()
        Try
            dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
            Return dtUniqRecords
        Catch ex As Exception
            Throw
        End Try
    End Function
    'End/2014.11.28/SR/BT 2728: Cash out payment register mail shows wrong information
End Class