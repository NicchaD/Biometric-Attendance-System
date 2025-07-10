
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	VoidDisbursementVRManager.aspx.vb
' Author Name		:	Imran
' Employee ID		:	51494
' Email				:	
' Contact No		:	
' Creation Time		:	08/18/2009 
'*******************************************************************************
'**     DATE            AUTHOR          REASONS
'*******************************************************************************
'***    30/10/2009      imran           For BT 1003
'**     30/10/2009      Imran           Save and PHR button in single line
'**     05-11-09        Priya J         changes made to view datagrid after no records found message.
'**     11-Nov-2009     priya J         Validation if deduction amount is greater than net amount.
'**     11/11/09        Imran           Check total of deductions are less than disbursement of net amount.
'**     12/11/2009      Imran           Remove validation on require Request id for disbursement.     
'**     13/11/2009      Imran           For BT 1014- Previous person details shows in disbursement tab.     
'**     16/11/09        Imran           Check total of deductions are less than disbursement of net amount.
'**     17/11/09        Imran           set false flag for refund Popup 
'**     20/11/2009      Imran           Trimming of search fields 
'       Neeraj Singh    19/Nov/2009     Added form name for security issue YRS 5.0-940 
'**     30-Nov-09       Priya           Added trim to dr("WithholdingTypeCode") as per nikunj mail 30-11-09
'**     1/12/09         Imran           handling existing deductions
'**     4/12/09         Imran           Commented validation for total of deductions are less than disbursement of gross amount in validatevoidreplace method.
'**     07/12/09        Imran           For select deductions is greater than Net amount
'**     19/12/2009      Imran           For Gemini Issue  968 -When disbursement is cash check then checkbox disable
'       15/Feb/2010     Shashi Shekhar  Restrict Data Archived Participants To proceed in Find list
'**     17/Mar/2010     Shashi Shekhar  Allow the user to access certain functions only for Retired participants (status RD) even if they are archived (Ref:Handling usability issue of Data Archive)
'*'     29-April-2010   Priya           YRS 5.0-1060-Voiding a Check
'*'     10-May-2010     Priya           YRS 5.0-1060-Voiding a Check
'*      28-June-2010    Imran           Chenges for Enhancements
'       09 Dec-2010     Shashi Shekhar  For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'       28 Feb 2011     Shashi Shekhar  Replacing Header formating with user control (YRS 5.0-450 )
'       27-Apr-2011     Imran           BT:825- Security issue in void reverse
'       16-May-2011     Imran           BT:767-YRS 5.0-1273 : Incorrect validation- The Void control is always passing the id of the first disbursement for validating if the data is missing or not
'       2014.08.13      Sanjay          BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
'       2015.09.16      Manthan R.      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports System.Globalization
Imports System.Web.UI.HtmlControls.HtmlGenericControl
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text
Imports System.IO
Public Class VoidDisbursementReplaceManager
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'Dim strFormName As String = New String("VoidDisbursementReplaceManager.aspx")'Commented by Shashi Shekhar:2009-12-17:To Resolve the security Issue (Ref:Security Review excel sheet)
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents DataGridVRManager As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridDisbursements As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripVRManager As Microsoft.Web.UI.WebControls.TabStrip

    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeSSN As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents MultiPageVRManager As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAccNo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAccountNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBankInfo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxBankInfo As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEntityType As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxEntityType As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEntityAddress As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxEntityAddress As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLegalEntity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLegalEntity As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownListStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelReason As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownListReason As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents chkChangeStatus As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDeductionCashOutsCharges As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelNotes As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents TestCheckBox As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents PlaceHolderMessageBox As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents litPersID As System.Web.UI.WebControls.Literal
    Protected WithEvents litDisbID As System.Web.UI.WebControls.Literal
    Protected WithEvents litFundID As System.Web.UI.WebControls.Literal
    Protected WithEvents litFlag As System.Web.UI.WebControls.Literal
    Protected WithEvents litWHAmount As System.Web.UI.WebControls.Literal
    Protected WithEvents litGross As System.Web.UI.WebControls.Literal
    Protected WithEvents litPayeeId As System.Web.UI.WebControls.Literal
    Protected WithEvents litAddressID As System.Web.UI.WebControls.Literal
    Protected WithEvents litDisbNbr As System.Web.UI.WebControls.Literal
    Protected WithEvents LabelRecordNotFound As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPHR As System.Web.UI.WebControls.Button
    Protected WithEvents Datalist1 As System.Web.UI.WebControls.DataList
    Protected WithEvents DataList2 As System.Web.UI.WebControls.DataList
    Protected WithEvents dgDeduction As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgItemDetails As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datalist3 As System.Web.UI.WebControls.DataList
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents VoidUserControl1 As YMCAUI.VoidUserControl
    Protected WithEvents frmVRManager As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents Menu1 As skmMenu.Menu

    'Added by imran on  29/10/2009 for UnGrouping Disbursement list
    Protected WithEvents VoidDisbursement1 As YMCAUI.VoidDisbursement

    Protected WithEvents TextBoxAddress As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxAccountNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxBankInfo As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxEntityType As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxEntityAddress As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxLegalEntity As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox



    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelCheckNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCheckNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents dgExistingDeduction As System.Web.UI.WebControls.DataGrid

    Protected WithEvents LabelAddressCheckSent As System.Web.UI.WebControls.Label
    Protected WithEvents hdnDisbId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdnListDisbId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ButtonCloseVR As System.Web.UI.WebControls.Button
    Protected WithEvents hypExistingDeductions As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object



    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'Global variable declaration
    Dim g_dataset_dsDisbursements As New DataSet
    Dim g_dataset_dsDisbursementsByPersId As New DataSet
    Dim g_dataset_dsWithholdingInfo As New DataSet
    Dim g_dataset_dsRelatedDisbursement As New DataSet
    Dim g_boolean_flag As Boolean
    Dim g_search_flag As Boolean
    Dim _iCounter As Integer = 0
    Dim objRefund As RefundClass
    Dim objLoan As LoanClass
    Dim objAnnuity As AnnuityClass

#Region "EVENTS"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'BT:825- Security issue in void reverse
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        ''Shubhrata 05-Sep-2006 TD Loans Phase 2
        'If Request.QueryString.Get("Activity") Is Nothing Then
        '    Response.Redirect("MainWebForm.aspx", False)
        'End If
        'Shubhrata 05-Sep-2006 TD Loans Phase 2

        Try
            If Not (IsPostBack) Then
                g_boolean_flag = False
                Clear_Session()
                Me.TabStripVRManager.Items(1).Enabled = False
                If Not IsNothing(DataGridDisbursements) Then
                    Me.DataGridDisbursements.SelectedIndex = 0
                End If
                'Added on 22/10/2009 For Label Header
                LoadGeneraltab("")

                If Not Request.QueryString.Get("Type") Is Nothing Then
                    LoadDeductions()
                End If

            End If

            If Not Session("IsZeroFunding") Is Nothing Then
                If Session("IsZeroFunding") = True Then
                    Session("IsZeroFunding") = Nothing
                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "No amount due. Net amount after withholding = $0.00", MessageBoxButtons.OK)
                End If
            End If

            Me.LabelSSNo.AssociatedControlID = Me.TextBoxSSNo.ID
            Me.LabelFundNo.AssociatedControlID = Me.TextBoxFundNo.ID
            Me.LabelLastName.AssociatedControlID = Me.TextBoxLastName.ID
            Me.LabelFirstName.AssociatedControlID = Me.TextBoxFirstName.ID
            Me.LabelCheckNo.AssociatedControlID = Me.TextBoxCheckNo.ID
            Me.LabelRecordNotFound.Visible = False
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            'ButtonSave.Attributes.Add("onclick", "javascript:return OpenBreakdownWindow('" + litPersID.Text + "','" + litDisbID.Text + "','" + litFundID.Text + "','" + litFlag.Text + "');")
            'If Request.Form("OK") = "OK" Then
            '    FinalStatus()
            'End If

            AddHandler VoidUserControl1.DisbursementbyPersIdEvent, AddressOf PopulateDisbursementTextBoxes
            AddHandler VoidUserControl1.ClearDisbursementGridEvent, AddressOf ClearDisbursementTextBoxes
            'Added by imran on  29/10/2009 for UnGrouping Disbursement list
            AddHandler VoidDisbursement1.DisbursementbyPersIdEvent, AddressOf PopulateDisbursementTextBoxes
            AddHandler VoidDisbursement1.ClearDisbursementGridEvent, AddressOf ClearDisbursementTextBoxes

            If Request.Form("Yes") = "Yes" Then
                If Session("flag") = "Pending" Then
                    Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                    Session("flag") = "notpending"
                    ValidateVoidReplace()
                    '  SearchAndPopulateData(True)
                End If
                '10-May-2010        Priya       YRS 5.0-1060-Voiding a Check

                If Not IsNothing(Session("ANNPaid")) Then
                    If Session("ANNPaid") = "Yes" Then
                        Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                        Session("ANNPaid") = "No"
                        ValidateVoidReplace()
                    End If
                End If
            End If
            'End 10-May-2010:YRS 5.0-1060-Voiding a Check

            If Request.Form("No") = "No" Then
                SearchAndPopulateData(True)
                If Session("flag") = "Pending" Then
                    Session("flag") = "notpending"
                End If
            End If

            If Request.Form("MessageBoxIDNo") = "Cancel" Then
                SearchAndPopulateData(True)
            End If
            If Request.Form("OK") = "OK" Then

                'Added by imran on 11/11/2009
                If Session("flag") <> "Deduction" Then
                    FinalStatus()
                    'Added by imran on 29/10/2009 
                    ClearDisbursementTextBoxes()
                    If VoidUserControl1.Action = "REFUND" Then
                        VoidUserControl1.PopulateDisbursementGrid()
                    Else
                        VoidDisbursement1.PopulateDisbursementGrid()
                    End If
                End If

            End If
            'Added by imran on 30/10/2009 For BT 1003
        Catch ex As SqlException
            Session("Called") = False
            Dim l_string_Type As String
            l_string_Type = Request.QueryString.Get("Type")
            If ex.Number = 60006 Then
                ' Response.Redirect("VoidDisbursementVRManager.aspx?Type=" & l_string_Type & "")
                LoadGeneraltab("")

                If Not IsNothing(DataGridVRManager) Then
                    Me.DataGridVRManager.SelectedIndex = -1
                End If
                RepopulatePopulateData()
                'DataGridVRManager.SelectedIndex = 0
                Me.TabStripVRManager.Items(1).Enabled = False
                Me.MultiPageVRManager.SelectedIndex = 0
                Me.TabStripVRManager.SelectedIndex = 0
            End If
        Catch ex As Exception
            Session("Called") = False
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub
    Private Sub TabStripVRManager_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripVRManager.SelectedIndexChange
        Try
            If (TabStripVRManager.SelectedIndex <> 0) Then
                Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                'ButtonSave.Enabled = False
                ButtonSave.Enabled = True
                Session("DisbursementByPersIdIndex") = 0
                'aparna yren-2646
                Session("DisbursementId") = Nothing
                'aparna
                SearchAndPopulateData(True)


            Else
                Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                'ButtonSave.Enabled = False
                ButtonSave.Enabled = True
                Session("DisbursementByPersIdIndex") = 0
                'aparna yren-2646
                Session("DisbursementId") = Nothing
                'aparna
                'SearchAndPopulateData(True)

            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "") 'by Aparna 19/09/2007
            TextBoxSSNo.Text = TextBoxSSNo.Text.Trim()
            'Added by imran on 20/11/2009 For Triming search fields
            TextBoxFundNo.Text = TextBoxFundNo.Text.Trim()
            TextBoxSSNo.Text = TextBoxSSNo.Text.Trim()
            TextBoxCheckNo.Text = TextBoxCheckNo.Text.Trim()
            TextBoxFirstName.Text = TextBoxFirstName.Text.Trim()
            TextBoxLastName.Text = TextBoxLastName.Text.Trim()
            'Call this method to get the list of participant
            If Not (TextBoxCheckNo.Text = "" And TextBoxFirstName.Text = "" And TextBoxLastName.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNo.Text = "") Then

                Session("DisbursementIndex") = 0
                Session("DisbursementByPersIdIndex") = 0
                Session("DisbursementId") = Nothing
                'Added by imran on 13/11/2009 For BT-1014
                Me.TabStripVRManager.Items(1).Enabled = False
                SearchAndPopulateData(False)
                'Added no 22/10/2009 For Label Header
                LoadGeneraltab("")


            Else
                Session("g_search_flag") = True
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "This search will take a long time, so please define a search criteria.", MessageBoxButtons.OK)
            End If

        Catch ex As System.Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub DataGridVRManager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridVRManager.SelectedIndexChanged


        Dim l_string_Type As String

        Dim l_PersId As String
        Dim dr As DataRow()
        Try

            '----Shashi Shekhar:2010-02-15: Code to handle Archived Participants from list--------------
            'Shashi Shekhar:2010-03-17:Handling usability issue of Data Archive
            l_PersId = Me.DataGridVRManager.SelectedItem.Cells(1).Text.Trim
            If Not Session("dsDisbursements") Is Nothing Then
                g_dataset_dsDisbursements = Session("dsDisbursements")
            End If
            dr = g_dataset_dsDisbursements.Tables("Disbursements").Select("[PersId]= '" & l_PersId & "'")

            l_string_Type = Request.QueryString.Get("Type")

            If l_string_Type = "REFUND" Then

                'If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                If (dr(0).Item("IsArchived") = True) Then
                    MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.PageTitle = "Void and Replace Disbursement / Withdrawals"
                    Exit Sub
                End If

            ElseIf l_string_Type = "TDLOAN" Then

                'If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                If (dr(0).Item("IsArchived") = True) Then
                    MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.PageTitle = "Void and Replace Disbursement / Loan"
                    Exit Sub
                End If

            ElseIf l_string_Type = "ANNUITY" Then
                'Priya 29-April-2010 : YRS 5.0-1060-Voiding a Check
                Session("StatusType") = dr(0).Item("StatusType").trim
                'End YRS 5.0-1060

                'If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "RD")) Then
                    MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.PageTitle = "Void and Replace Disbursement / Annuity"
                    Exit Sub
                End If

            End If


            '-----------------------------------------------------------------------------------------------



            Session("DisbursementIndex") = DataGridVRManager.SelectedIndex
            Session("_iCounter") = 0
            Session("SSNoVRManager") = Me.DataGridVRManager.SelectedItem.Cells(2).Text.Trim
            'aparna 10/01/2007
            'Me.DataGridDisbursements.SelectedIndex = 0
            Session("DisbursementByPersIdIndex") = 0 'added by priya on 12/15/2008 for YRS 5.0-626
            'aparna 10/01/2007
            ' PopulateDisbursementGrid()
            ClearDisbursementTextBoxes()
            If Me.DataGridVRManager.Items.Count > 0 Then
                Me.TabStripVRManager.Items(1).Enabled = True

            End If
            'Added by imran on  29/10/2009 for UnGrouping Disbursement list

            If Request.QueryString.Get("Type") = "REFUND" Then
                Me.VoidUserControl1.PopulateDisbursementGrid()
                Me.VoidDisbursement1.Visible = False
            Else
                Session("VRManager_Sort") = Nothing
                Me.VoidDisbursement1.PopulateDisbursementGrid()
                Me.VoidUserControl1.Visible = False
            End If


            Me.MultiPageVRManager.SelectedIndex = 1
            Me.TabStripVRManager.SelectedIndex = 1
            'If LabelNoRecordFound.Visible = False Then
            'End If
            'Added on 22/10/2009 For Header Text
            LoadGeneraltab(Me.DataGridVRManager.SelectedItem.Cells(1).Text.Trim)
            hypExistingDeductions.Visible = False

            If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                If g_dataset_dsDisbursementsByPersId.Tables.Count > 0 Then
                    If (g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows.Count > 0) Then
                        PopulateDisbursementTextBoxes()
                    End If
                Else
                    ClearDisbursementTextBoxes()
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click, ButtonCloseVR.Click

        Session("VRManager_Sort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)

    End Sub
    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Dim checkSecurity As String
        Dim strFormName As String

        Dim datagrid As DataGrid
        Dim dglst As DataList
        Dim cnt As Integer
        Dim NetAmt As Decimal
        Dim DedAmt As Decimal
        Dim DedExistAmt As Decimal
        cnt = 0
        hdnDisbId.Value = ""
        Session("flag") = Nothing
        hdnListDisbId.Value = ""
        Try

            'Commented by Shashi Shekhar:2009-12-17
            'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            'If Not checkSecurity.Equals("True") Then
            '    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'End : YRS 5.0-940

            '---------------------------------------------------------------------------------------
            'Added by Shashi Shekhar:2009-12-17: To resolve the security Issue(Reg:Security Review excel sheet)


            If VoidUserControl1.Action = "REFUND" Then
                strFormName = "VoidDisbursementReplaceManager.aspx?Type=REFUND"
                checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            ElseIf VoidUserControl1.Action = "ANNUITY" Then
                strFormName = "VoidDisbursementReplaceManager.aspx?Type=ANNUITY"
                checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            ElseIf VoidUserControl1.Action = "TDLOAN" Then
                strFormName = "VoidDisbursementReplaceManager.aspx?Type=TDLOAN"
                checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            End If

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            '--------------------------------------------------------------------------------------


            If VoidUserControl1.Action = "REFUND" Then
                dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)


                For Each item As DataListItem In dglst.Items
                    Dim img As ImageButton
                    Dim td As HtmlTableCell
                    img = item.Controls(0).FindControl("Imagebutton1")
                    If img.ImageUrl = "../images/selected.gif" Then
                        datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing(datagrid) Then
                            Dim i As Integer

                            For i = 0 To datagrid.Items.Count - 1
                                Dim chk As CheckBox
                                Dim LabelDisbId As Label
                                chk = datagrid.Items(i).FindControl("chkRefund")
                                LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
                                If Not IsNothing(chk) Then
                                    If chk.Checked Then
                                        NetAmt = Convert.ToDecimal(datagrid.Items(i).Cells(7).Text)
                                        hdnListDisbId.Value = hdnListDisbId.Value + "," + LabelDisbId.Text.Trim()
                                    End If
                                End If
                            Next
                        End If

                    End If
                    cnt += 1
                Next


            Else
                Dim i As Integer
                datagrid = CType(VoidDisbursement1.FindControl("Datagrid1"), DataGrid)
                For i = 0 To datagrid.Items.Count - 1
                    Dim btnImage As ImageButton
                    btnImage = datagrid.Items(i).FindControl("ImagebtnAddress")
                    If Not IsNothing(btnImage) Then
                        If btnImage.ImageUrl = "../images/selected.gif" Then
                            '    If btnImage.ImageUrl = "../images/details.gif" Then
                            NetAmt = Convert.ToDecimal(datagrid.Items(i).Cells(6).Text)
                            hdnListDisbId.Value = hdnListDisbId.Value + "," + datagrid.Items(i).Cells(1).Text
                        End If
                    End If
                Next

            End If


            If hdnListDisbId.Value.Length = 0 Then
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select a Disbursement.", MessageBoxButtons.Stop)
                Exit Sub
            End If



            'Added by imran on 16/12/2009  for Gemini Issue  968 -When disbursement is cash check then checkbox disable
            g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
            If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                Dim parameterListDisbId As Array
                Dim strOutPut As String
                Dim i As Integer
                Dim l_searchExpr As String
                Dim l_datarow_CurrentRow As DataRow


                If hdnListDisbId.Value.Length > 0 Then
                    parameterListDisbId = hdnListDisbId.Value.Split(",")
                End If
                ''**If Not Session("DisbursementId") Is Nothing Then
                Dim l_dr_CurrentRow As DataRow()

                If parameterListDisbId.Length >= 2 Then
                    For i = 1 To parameterListDisbId.Length - 1
                        strOutPut += "','" + parameterListDisbId.GetValue(i)

                    Next
                End If
                strOutPut = strOutPut.Substring(3, strOutPut.Length - 3)

                l_searchExpr = "DisbursementID in  ('" + strOutPut + "') and bitpaid=1"
                l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)

                If l_dr_CurrentRow.Length > 0 Then

                    '29-April-2010:Priya: YRS 5.0-1060-Voiding a Check
                    If VoidUserControl1.Action = "ANNUITY" Then
                        If Not Session("StatusType") Is Nothing OrElse Convert.ToString(Session("StatusType")).Trim = "" Then
                            If Convert.ToString(Session("StatusType")).Trim = "DR" OrElse Convert.ToString(Session("StatusType")).Trim = "DD" OrElse Convert.ToString(Session("StatusType")).Trim = "DF" OrElse Convert.ToString(Session("StatusType")).Trim = "DQ" Then
                                Session("ANNPaid") = "Yes"
                                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "The payment has been cashed. Do you wish to continue voiding this payment?", MessageBoxButtons.YesNo)
                                Exit Sub
                            End If
                        End If
                    End If
                    'End YRS 5.0-1060-Voiding a Check

                    'l_datarow_CurrentRow = l_dr_CurrentRow(0)
                    'If CType(l_datarow_CurrentRow("bitpaid"), String).ToUpper = "TRUE" Then
                    'End If
                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement is cashed. Cannot replace it.", MessageBoxButtons.Stop)
                    Exit Sub

                End If
            End If


            ''Added by imran on 16/11/2009 For  Check total of deductions are less than of disbursement's net amount.

            If dgExistingDeduction.Items.Count > 0 Then
                For cnt = 0 To dgExistingDeduction.Items.Count - 1
                    Dim CheckBoxDeduction As CheckBox
                    CheckBoxDeduction = dgExistingDeduction.Items(cnt).FindControl("CheckBoxDeduction")
                    If Not IsNothing(CheckBoxDeduction) Then
                        'Added by imran on 07/12/2009 For deselected of existing deductions is added in to net amount
                        If CheckBoxDeduction.Checked = False Then
                            DedExistAmt += Convert.ToDecimal(dgExistingDeduction.Items(cnt).Cells(3).Text)
                        End If
                    End If
                Next

            End If

            For cnt = 0 To dgDeduction.Items.Count - 1
                Dim CheckBoxDeduction As CheckBox
                CheckBoxDeduction = dgDeduction.Items(cnt).FindControl("CheckBoxDeduction")
                If Not IsNothing(CheckBoxDeduction) Then
                    If CheckBoxDeduction.Checked Then
                        DedAmt += Convert.ToDecimal(dgDeduction.Items(cnt).Cells(3).Text)
                    End If
                End If
            Next

            If ((NetAmt + DedExistAmt) - DedAmt) < 0.01 Then
                'MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Net Amount of the Refund cannot be less than $0.01. Please adjust the deductions ", MessageBoxButtons.Stop)
                'MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Deduction amount can not be exceed than net amount. Please adjust amount. ", MessageBoxButtons.Stop)
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Net amount can not be less than $0.01. Please adjust deduction(s). ", MessageBoxButtons.Stop)

                Session("flag") = "Deduction"
                Exit Sub
            End If

            ValidateVoidReplace()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFundNo.Text = ""
            Me.TextBoxLastName.Text = ""
            Me.TextBoxFirstName.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.TextBoxCheckNo.Text = ""
            Me.DataGridVRManager.Visible = False
            Me.TabStripVRManager.Items(1).Enabled = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub DataGridVRManager_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridVRManager.SortCommand
        Try
            Me.DataGridVRManager.SelectedIndex = -1
            If Not Session("DS_Sort_VRManager") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
                'g_dataset_dsDisbursements = viewstate("DS_Sort_VRManager")
                g_dataset_dsDisbursements = Session("DS_Sort_VRManager")
                dv = g_dataset_dsDisbursements.Tables(0).DefaultView
                dv.Sort = SortExpression

                If Not Session("VRManager_Sort") Is Nothing Then
                    If Session("VRManager_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If

                Me.DataGridVRManager.DataSource = Nothing
                Me.DataGridVRManager.DataSource = dv
                Me.DataGridVRManager.DataBind()
                Session("VRManager_Sort") = dv.Sort
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonPHR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPHR.Click
        Try

            Session("VRManager") = "VRManager"
            Session("SSNo") = Session("SSNoVRManager")
            Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('RetireesFrmMoverWebForm.aspx', 'ReportPopUp', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
            SearchAndPopulateData(True)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Added on 11/11/09 by imran
    Sub CheckBox_CheckedChanged(ByVal Source As Object, ByVal e As System.EventArgs)

        Dim strID As String
        Dim chk As CheckBox
        chk = CType(Source, CheckBox)
        Dim datagrid As DataGrid
        Dim dglst As DataList
        Dim cnt As Integer
        Dim NetAmt As Decimal
        Dim DedAmt As Decimal
        cnt = 0
        Try

            strID = chk.ClientID
            If Not IsNothing(chk) Then


                If VoidUserControl1.Action = "REFUND" Then
                    dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)


                    For Each item As DataListItem In dglst.Items
                        Dim img As ImageButton
                        img = item.Controls(0).FindControl("Imagebutton1")
                        If img.ImageUrl = "../images/selected.gif" Then
                            DataGrid = CType(item.FindControl("dgItemDetails"), DataGrid)
                            If Not IsNothing(DataGrid) Then
                                Dim i As Integer

                                For i = 0 To DataGrid.Items.Count - 1
                                    Dim chkdisb As CheckBox
                                    Dim LabelDisbId As Label
                                    chkdisb = DataGrid.Items(i).FindControl("chkRefund")
                                    LabelDisbId = DataGrid.Items(i).FindControl("LabelDisbId")
                                    If Not IsNothing(chkdisb) Then
                                        If chkdisb.Checked Then
                                            NetAmt = Convert.ToDecimal(DataGrid.Items(i).Cells(7).Text)
                                        End If
                                    End If
                                Next
                            End If

                        End If
                        cnt += 1
                    Next


                Else
                    Dim i As Integer
                    Dim dg As DataGrid
                    dg = CType(VoidDisbursement1.FindControl("datagrid1"), DataGrid)
                    For i = 0 To dg.Items.Count - 1
                        Dim btnImage As ImageButton
                        btnImage = dg.Items(i).FindControl("ImagebtnAddress")
                        If Not IsNothing(btnImage) Then
                            If btnImage.ImageUrl = "../images/selected.gif" Then
                                NetAmt = Convert.ToDecimal(dg.Items(i).Cells(6).Text)
                            End If
                        End If
                    Next

                End If


                Dim j As Integer
                For j = 0 To dgExistingDeduction.Items.Count - 1
                    Dim CheckBoxDeduction As CheckBox
                    CheckBoxDeduction = dgExistingDeduction.Items(j).FindControl("CheckBoxDeduction")
                    If Not IsNothing(CheckBoxDeduction) Then
                        If CheckBoxDeduction.Checked Then
                            DedAmt += Convert.ToDecimal(dgExistingDeduction.Items(j).Cells(3).Text)
                        End If
                    End If
                Next

                If NetAmt < DedAmt And NetAmt > 0 Then
                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Net Amount of the Refund cannot be less than $0.01. Please adjust the deductions ", MessageBoxButtons.Stop)
                    For j = 0 To dgExistingDeduction.Items.Count - 1
                        Dim CheckBoxDeduction As CheckBox
                        CheckBoxDeduction = dgExistingDeduction.Items(j).FindControl("CheckBoxDeduction")
                        If Not IsNothing(CheckBoxDeduction) Then
                            If CheckBoxDeduction.Checked Then
                                If strID = CheckBoxDeduction.ClientID Then
                                    CheckBoxDeduction.Checked = False
                                End If

                            End If
                        End If
                    Next
                    Session("flag") = "Deduction"
                    Exit Sub

                End If


            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
#End Region

#Region "METHODS"
    'Method to populate the datagrid with the list of employees.
    Public Sub SearchAndPopulateData(ByVal _postback As Boolean)
        Dim l_integer_AnnuityOnly As Integer
        Dim l_string_Type As String
        Try
            l_string_Type = Request.QueryString.Get("Type")
            If l_string_Type = "REFUND" Then
                VoidUserControl1.Action = "REFUND"
                l_integer_AnnuityOnly = 0
                VoidUserControl1.Activity = "REFUNDREPLACE"
            ElseIf (l_string_Type = "TDLOAN") Then
                VoidUserControl1.Action = "TDLOAN"
                VoidUserControl1.Activity = "LOANREPLACE"

                'Added by Imran on 29/10/2009
                VoidDisbursement1.Action = "TDLOAN"
                VoidDisbursement1.Activity = "LOANREPLACE"
                l_integer_AnnuityOnly = 21
            ElseIf (l_string_Type = "ANNUITY") Then

                VoidUserControl1.Action = "ANNUITY"
                VoidUserControl1.Activity = "VReplace"

                'Added by Imran on 29/10/2009
                VoidDisbursement1.Action = "ANNUITY"
                VoidDisbursement1.Activity = "VReplace"
                l_integer_AnnuityOnly = 1
            End If




            If Not _postback Then
                g_dataset_dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.LookUpDisbursements(Me.TextBoxFundNo.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxSSNo.Text.Trim(), Me.TextBoxCheckNo.Text.Trim(), l_integer_AnnuityOnly)
                'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
                'ViewState("dsDisbursement") = g_dataset_dsDisbursements

                'g_dataset_dsDisbursements.Tables(0).DefaultView.Table().Select("order by Number desc")
                Session("dsDisbursements") = g_dataset_dsDisbursements

            Else
                'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
                'If Not ViewState("dsDisbursement") Is Nothing Then
                '    g_dataset_dsDisbursements = ViewState("dsDisbursement")
                'End If

                If Not Session("dsDisbursements") Is Nothing Then
                    g_dataset_dsDisbursements = Session("dsDisbursements")
                End If

            End If

            If g_dataset_dsDisbursements.Tables("Disbursements").Rows.Count = 0 Then
                Me.LabelRecordNotFound.Visible = True
                Me.DataGridVRManager.Visible = False
            Else
                Me.DataGridVRManager.DataSource = g_dataset_dsDisbursements.Tables(0).DefaultView

                Session("DS_Sort_VRManager") = g_dataset_dsDisbursements
                Me.DataGridVRManager.DataBind()

                If Not (Session("DisbursementIndex") > 0) Then
                    Session("DisbursementIndex") = 0
                End If

                Me.LabelRecordNotFound.Visible = False
                Me.DataGridVRManager.Visible = True
                'Call the following method to load the Disbursements Grid.


            End If
        Catch ex As SqlException
            ' Throw (ex)
            If Not Session("dsDisbursements") Is Nothing Then
                Session("dsDisbursements") = Nothing
            End If
            'Added on 22/10/2009 by imran
            Session("g_search_flag") = True
            Me.TabStripVRManager.Items(1).Enabled = False
            'MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
            LabelRecordNotFound.Visible = True
            '05-11-09 : changes made to view datagrid after no records found message.
            DataGridVRManager.Visible = False
        End Try
    End Sub
    'Added by imran on 30/10/2009
    'Method to Repopulate the datagrid with the list of employees.
    Public Sub RepopulatePopulateData()
        Dim l_integer_AnnuityOnly As Integer
        Dim l_string_Type As String
        Try
            l_string_Type = Request.QueryString.Get("Type")
            If l_string_Type = "REFUND" Then
                VoidUserControl1.Action = "REFUND"
                l_integer_AnnuityOnly = 0
                VoidUserControl1.Activity = "REFUNDREPLACE"
            ElseIf (l_string_Type = "TDLOAN") Then
                VoidUserControl1.Action = "TDLOAN"
                VoidUserControl1.Activity = "LOANREPLACE"

                'Added by Imran on 29/10/2009
                VoidDisbursement1.Action = "TDLOAN"
                VoidDisbursement1.Activity = "LOANREPLACE"
                l_integer_AnnuityOnly = 21
            ElseIf (l_string_Type = "ANNUITY") Then

                VoidUserControl1.Action = "ANNUITY"
                VoidUserControl1.Activity = "VReplace"

                'Added by Imran on 29/10/2009
                VoidDisbursement1.Action = "ANNUITY"
                VoidDisbursement1.Activity = "VReplace"
                l_integer_AnnuityOnly = 1
            End If


            g_dataset_dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.LookUpDisbursements(Me.TextBoxFundNo.Text, Me.TextBoxFirstName.Text, Me.TextBoxLastName.Text, Me.TextBoxSSNo.Text, Me.TextBoxCheckNo.Text, l_integer_AnnuityOnly)
            Session("dsDisbursements") = g_dataset_dsDisbursements



            If g_dataset_dsDisbursements.Tables("Disbursements").Rows.Count = 0 Then
                Me.LabelRecordNotFound.Visible = True
                Me.DataGridVRManager.Visible = False
            Else
                Me.DataGridVRManager.DataSource = g_dataset_dsDisbursements.Tables(0).DefaultView

                ' Session("DS_Sort_VRManager") = g_dataset_dsDisbursements
                Me.DataGridVRManager.DataBind()

                If Not (Session("DisbursementIndex") > 0) Then
                    Session("DisbursementIndex") = 0
                End If

                Me.LabelRecordNotFound.Visible = False
                Me.DataGridVRManager.Visible = True
                'Call the following method to load the Disbursements Grid.


            End If
        Catch ex As SqlException
            ' Throw (ex)
            If Not Session("dsDisbursements") Is Nothing Then
                Session("dsDisbursements") = Nothing
            End If
            'Added on 22/10/2009 by imran
            Session("g_search_flag") = True
            Me.TabStripVRManager.Items(1).Enabled = False
            ' MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
            Me.LabelRecordNotFound.Visible = True
            Me.DataGridVRManager.Visible = False
        End Try
    End Sub
    Public Sub PopulateDisbursementTextBoxes()
        Dim l_integer_CurrentIndex As Integer
        Dim l_datarow_CurrentRow As DataRow

        Try
            l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
            g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

            If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                Dim l_dr_CurrentRow As DataRow()
                Dim l_searchExpr As String
                If Not Session("DisbursementId") Is Nothing Then
                    l_searchExpr = "DisbursementID = '" + Session("DisbursementId") + "'"
                    l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                    If l_dr_CurrentRow.Length > 0 Then
                        l_datarow_CurrentRow = l_dr_CurrentRow(0)
                    Else
                        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data could not be retrieved.", MessageBoxButtons.OK)
                        Exit Sub
                    End If
                Else
                    l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
                End If

                If Not l_datarow_CurrentRow Is Nothing Then
                    'If ((l_datarow_CurrentRow("PayeeAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeAddress").ToString <> "")) Then
                    '    Me.TextBoxAddress.Text = CType(l_datarow_CurrentRow.Item("PayeeAddress"), String)
                    'Else
                    '    Me.TextBoxAddress.Text = "No address found"
                    'End If

                    'If ((l_datarow_CurrentRow("AcctNumber").ToString <> "System.DBNull") And (l_datarow_CurrentRow("AcctNumber").ToString <> "")) Then
                    '    Me.TextBoxAccountNo.Text = CType(l_datarow_CurrentRow.Item("AcctNumber"), String)
                    'Else
                    '    Me.TextBoxAccountNo.Text = ""
                    'End If

                    'If ((l_datarow_CurrentRow("LegalEntityType").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityType").ToString <> "")) Then
                    '    Me.TextBoxEntityType.Text = CType(l_datarow_CurrentRow.Item("LegalEntityType"), String)
                    'Else
                    '    Me.TextBoxEntityType.Text = ""
                    'End If

                    'If ((l_datarow_CurrentRow("BankInfo").ToString <> "System.DBNull") And (l_datarow_CurrentRow("BankInfo").ToString <> "")) Then
                    '    Me.TextBoxBankInfo.Text = CType(l_datarow_CurrentRow.Item("BankInfo"), String)
                    'Else
                    '    Me.TextBoxBankInfo.Text = ""
                    'End If

                    'If ((l_datarow_CurrentRow("LegalEntityAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityAddress").ToString <> "")) Then
                    '    Me.TextBoxEntityAddress.Text = CType(l_datarow_CurrentRow.Item("LegalEntityAddress"), String)
                    'Else
                    '    Me.TextBoxEntityAddress.Text = ""
                    'End If


                    'If ((l_datarow_CurrentRow("LegalEntityName").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityName").ToString <> "")) Then
                    '    Me.TextBoxLegalEntity.Text = CType(l_datarow_CurrentRow.Item("LegalEntityName"), String)
                    'Else
                    '    Me.TextBoxLegalEntity.Text = ""
                    'End If

                    'If ((l_datarow_CurrentRow("ActionNotes").ToString <> "System.DBNull") And (l_datarow_CurrentRow("ActionNotes").ToString <> "")) Then
                    '    Me.TextBoxNotes.Text = CType(l_datarow_CurrentRow.Item("ActionNotes"), String)
                    'Else
                    '    Me.TextBoxNotes.Text = ""
                    'End If

                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Sub ClearDisbursementTextBoxes()

        Try
            Dim l_CheckBox As CheckBox

            Me.TextBoxAddress.Text = ""
            Me.TextBoxAccountNo.Text = ""
            Me.TextBoxEntityType.Text = ""
            Me.TextBoxBankInfo.Text = ""
            Me.TextBoxEntityAddress.Text = ""
            Me.TextBoxLegalEntity.Text = ""


            For Each l_DataGridItem As DataGridItem In dgDeduction.Items
                l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")
                If Not l_CheckBox Is Nothing Then
                    l_CheckBox.Checked = False
                End If
            Next

            dgExistingDeduction.DataSource = Nothing
            dgExistingDeduction.DataBind()
            hypExistingDeductions.Visible = False

        Catch
            Throw
        End Try
    End Sub
    Public Sub FinalStatus()
        Try


            If (Session("g_search_flag") = True) Then
                Session("g_search_flag") = True
                Me.LabelRecordNotFound.Visible = False
                Me.DataGridVRManager.Visible = False

            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub ChangeStatus(ByVal parameterDisbId As String)
        Dim l_string_NewStatus As String
        Dim l_datarow_CurrentRow As DataRow
        Dim l_integer_CurrentIndex As Integer
        Dim l_string_DisbId As String
        Dim l_string_DisbType As String
        Dim l_datarow_WithholdingInfo As DataRow
        Dim l_boolean_GoForBreakdown As Boolean = False
        Dim l_string_FundEventId As String
        Dim l_double_WHAmount As Double
        Dim l_double_Gross As Double
        Dim l_string_ReplaceId As String
        Dim l_string_PersId As String
        Dim l_integer_GridIndex As Integer
        Dim l_datarow_CurrentRow_PersId As DataRow
        Dim Strarr As Array
        Dim l_string_Activity As String
        'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
        Dim dblDisbTaxableAmt As Double = 0.0
        Dim dblDisbNonTaxableAmt As Double = 0.0
        Dim dblProRateFactor As Double = 0.0
        'End:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.

        Try
            'l_string_NewStatus = Me.DropDownListStatus.SelectedValue.Trim()
            l_string_NewStatus = "REPLACE"
            l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
            g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

            l_string_Activity = ""
            l_string_Activity = Request.QueryString.Get("Type")

            'Added on 29/09/2009 by imran
            If Not Session("dsDisbursements") Is Nothing Then
                g_dataset_dsDisbursements = Session("dsDisbursements")
                l_integer_GridIndex = Session("DisbursementIndex") 'commented by priya on 12/15/2008 for YRS 5.0-626
                l_datarow_CurrentRow_PersId = g_dataset_dsDisbursements.Tables("Disbursements").Rows(l_integer_GridIndex)
                l_string_PersId = CType(l_datarow_CurrentRow_PersId("PersId"), String)
                litPersID.Text = l_string_PersId
            End If
            If Not g_dataset_dsDisbursementsByPersId Is Nothing Then

                'Aparna YREN 2646 15/09/2006
                Dim l_dr_CurrentRow As DataRow()
                Dim l_searchExpr As String

                'BT:767-YRS 5.0-1273 : Incorrect validation- The Void control is always passing the id of the first disbursement for validating if the data is missing or not
                'If Not Session("DisbursementId") Is Nothing Then
                If parameterDisbId <> "" Then
                    l_searchExpr = "DisbursementID = '" + parameterDisbId + "'"
                    l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                    'l_datarow_CurrentRow = l_dr_CurrentRow(0)
                    If l_dr_CurrentRow.Length > 0 Then
                        l_datarow_CurrentRow = l_dr_CurrentRow(0)
                    Else
                        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data couldnot be retrieved.", MessageBoxButtons.OK)
                        Exit Sub
                    End If
                Else
                    l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
                End If

                If ((l_datarow_CurrentRow("DisbursementID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("DisbursementID").ToString <> "")) Then
                    l_string_DisbId = CType(l_datarow_CurrentRow.Item("DisbursementID"), String)
                End If

                If ((l_datarow_CurrentRow("Type").ToString <> "System.DBNull") And (l_datarow_CurrentRow("Type").ToString <> "")) Then
                    l_string_DisbType = CType(l_datarow_CurrentRow.Item("Type"), String)
                    litDisbID.Text = l_string_DisbId
                End If

                If ((l_datarow_CurrentRow("PayeeEntityID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeEntityID").ToString <> "")) Then
                    litPayeeId.Text = l_datarow_CurrentRow.Item("PayeeEntityID").ToString
                End If

                If ((l_datarow_CurrentRow("AddressID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("AddressID").ToString <> "")) Then
                    litAddressID.Text = l_datarow_CurrentRow.Item("AddressID").ToString
                End If
                'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
                If Not (String.IsNullOrEmpty(l_datarow_CurrentRow("DisbTaxableAmount").ToString)) Then
                    dblDisbTaxableAmt = l_datarow_CurrentRow.Item("DisbTaxableAmount").ToString
                End If

                If Not (String.IsNullOrEmpty(l_datarow_CurrentRow("DisbNonTaxableAmount").ToString)) Then
                    dblDisbNonTaxableAmt = l_datarow_CurrentRow.Item("DisbNonTaxableAmount").ToString
                End If
                'End:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
                'If ((l_datarow_CurrentRow("Number").ToString <> "System.DBNull") And (l_datarow_CurrentRow("Number").ToString <> "")) Then
                '    litDisbNbr.Text = l_datarow_CurrentRow.Item("Number").ToString
                'End If

                If l_string_NewStatus = "REPLACE" And l_string_Activity = "REFUND" Then
                    g_dataset_dsWithholdingInfo = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetWithholdingInfo(l_string_DisbId)

                    If g_dataset_dsWithholdingInfo.Tables("WithholdingInfo").Rows.Count > 0 Then

                        l_datarow_WithholdingInfo = g_dataset_dsWithholdingInfo.Tables("WithholdingInfo").Rows(0)

                        If (l_string_DisbType.Trim() = "REF" OrElse l_string_DisbType.Trim() = "ADT" OrElse l_string_DisbType.Trim() = "RDT") _
                                And l_string_NewStatus = "REPLACE" And CType(l_datarow_WithholdingInfo("DetailRows"), Integer) = 0 Then
                            l_boolean_GoForBreakdown = True
                        End If

                        If l_string_NewStatus = "REISSUE" And (CType(l_datarow_WithholdingInfo("DetailRows"), Integer) = 0 Or CType(l_datarow_WithholdingInfo("RefundRefRows"), Integer) = 0) Then
                            l_boolean_GoForBreakdown = True
                        End If



                        If l_boolean_GoForBreakdown Then

                            If ((l_datarow_WithholdingInfo("FundEventID").ToString <> "System.DBNull") And (l_datarow_WithholdingInfo("FundEventID").ToString <> "")) Then
                                l_string_FundEventId = Convert.ToString(l_datarow_WithholdingInfo("FundEventID"))
                                litFundID.Text = l_string_FundEventId
                            Else
                                litFundID.Text = ""
                            End If

                            If ((l_datarow_WithholdingInfo("WithHoldingAmount").ToString <> "System.DBNull") And (l_datarow_WithholdingInfo("WithHoldingAmount").ToString <> "")) Then
                                l_double_WHAmount = CType(l_datarow_WithholdingInfo("WithHoldingAmount"), Double)
                                litWHAmount.Text = l_double_WHAmount
                            End If

                            If ((l_datarow_WithholdingInfo("GrossAmount").ToString <> "System.DBNull") And (l_datarow_WithholdingInfo("GrossAmount").ToString <> "")) Then
                                l_double_Gross = CType(l_datarow_WithholdingInfo("GrossAmount"), Double)
                                litGross.Text = l_double_Gross
                            End If
                            'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
                            If l_double_Gross > 0 Then
                                dblProRateFactor = l_double_WHAmount / l_double_Gross
                            End If
                            CreateDisbursementDetails(l_string_DisbId, dblDisbTaxableAmt, dblDisbNonTaxableAmt, dblProRateFactor)
                            savevoidReplace()
                            'End:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
                        Else
                            savevoidReplace()

                        End If
                    End If
                ElseIf l_string_Activity = "TDLOAN" Or l_string_Activity = "ANNUITY" Then
                    savevoidReplace()

                End If

            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & Replace Withdrawal --> ChangeStatus", ex)
            Throw
        End Try

    End Sub
    Public Sub PopulateDisbursementTextBoxes(ByVal DisbursementID As String)
        Dim l_integer_CurrentIndex As Integer
        Dim l_datarow_CurrentRow As DataRow

        Try

            '  l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
            g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

            If DisbursementID <> "" Then
                PopulateDeductionDisbursementGrid(DisbursementID)
            End If


            If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                Dim l_dr_CurrentRow As DataRow()
                Dim l_searchExpr As String
                'If Not Session("DisbursementId") Is Nothing Then
                If Not DisbursementID Is Nothing Then
                    l_searchExpr = "DisbursementID = '" & DisbursementID & " '" '+ Session("DisbursementId") + "'"
                    l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                    If l_dr_CurrentRow.Length > 0 Then
                        l_datarow_CurrentRow = l_dr_CurrentRow(0)
                        '    Else

                        '        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data couldnot be retrieved.", MessageBoxButtons.OK)
                        '        Exit Sub
                        '    End If
                        'Else
                        '    Throw New Exception("DisbusrementId not found.")
                        '    'l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
                    End If

                    If Not l_datarow_CurrentRow Is Nothing Then
                        If ((l_datarow_CurrentRow("PayeeAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeAddress").ToString <> "")) Then
                            Me.TextBoxAddress.Text = CType(l_datarow_CurrentRow.Item("PayeeAddress"), String)
                        Else
                            Me.TextBoxAddress.Text = "No address found"
                        End If

                        If ((l_datarow_CurrentRow("AcctNumber").ToString <> "System.DBNull") And (l_datarow_CurrentRow("AcctNumber").ToString <> "")) Then
                            Me.TextBoxAccountNo.Text = CType(l_datarow_CurrentRow.Item("AcctNumber"), String)
                        Else
                            Me.TextBoxAccountNo.Text = ""
                        End If

                        If ((l_datarow_CurrentRow("LegalEntityType").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityType").ToString <> "")) Then
                            Me.TextBoxEntityType.Text = CType(l_datarow_CurrentRow.Item("LegalEntityType"), String)
                        Else
                            Me.TextBoxEntityType.Text = ""
                        End If

                        If ((l_datarow_CurrentRow("BankInfo").ToString <> "System.DBNull") And (l_datarow_CurrentRow("BankInfo").ToString <> "")) Then
                            Me.TextBoxBankInfo.Text = CType(l_datarow_CurrentRow.Item("BankInfo"), String)
                        Else
                            Me.TextBoxBankInfo.Text = ""
                        End If

                        If ((l_datarow_CurrentRow("LegalEntityAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityAddress").ToString <> "")) Then
                            Me.TextBoxEntityAddress.Text = CType(l_datarow_CurrentRow.Item("LegalEntityAddress"), String)
                        Else
                            Me.TextBoxEntityAddress.Text = ""
                        End If


                        If ((l_datarow_CurrentRow("LegalEntityName").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityName").ToString <> "")) Then
                            Me.TextBoxLegalEntity.Text = CType(l_datarow_CurrentRow.Item("LegalEntityName"), String)
                        Else
                            Me.TextBoxLegalEntity.Text = ""
                        End If

                        'If ((l_datarow_CurrentRow("ActionNotes").ToString <> "System.DBNull") And (l_datarow_CurrentRow("ActionNotes").ToString <> "")) Then
                        '    Me.TextBoxNotes.Text = CType(l_datarow_CurrentRow.Item("ActionNotes"), String)
                        'Else
                        '    Me.TextBoxNotes.Text = ""
                        'End If
                    Else
                        ClearDisbursementTextBoxes()

                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Sub PopulateDeductionDisbursementGrid(ByVal DisbursementID As String)
        Dim g_datatable_dsDisbursementsWithhold As DataTable
        Try

            Dim dr As DataRow
            Dim l_CheckBox As CheckBox

            'For Each l_DataGridItem As DataGridItem In dgExistingDeduction.Items

            '    l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")
            '    If Not l_CheckBox Is Nothing Then
            '        l_CheckBox.Checked = False
            '    End If
            'Next

            dgExistingDeduction.DataSource = Nothing
            dgExistingDeduction.DataBind()

            g_datatable_dsDisbursementsWithhold = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.WithholdingsByDisbursement(DisbursementID)



            If Not g_datatable_dsDisbursementsWithhold Is Nothing Then
                If g_datatable_dsDisbursementsWithhold.Rows.Count > 0 Then
                    hypExistingDeductions.Visible = True
                    dgExistingDeduction.DataSource = g_datatable_dsDisbursementsWithhold
                    dgExistingDeduction.DataBind()
                    hypExistingDeductions.Text = "Existing deductions :$ "
                    hypExistingDeductions.Text = hypExistingDeductions.Text & g_datatable_dsDisbursementsWithhold.Compute("SUM(Amount)", "").ToString() & " (Show)"


                    'For Each dr In g_datatable_dsDisbursementsWithhold.Rows

                    '    For Each l_DataGridItem As DataGridItem In dgExistingDeduction.Items

                    '        l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")

                    '        'Priya 30-Nov-09 Added trim to dr("WithholdingTypeCode") as per nikunj mail 30-11-09
                    '        If Convert.ToString(dr("WithholdingTypeCode")).Trim = l_DataGridItem.Cells.Item(1).Text.Trim Then

                    '            If Not l_CheckBox Is Nothing Then
                    '                l_CheckBox.Checked = True
                    '            End If
                    '        End If
                    '    Next

                    'Next
                Else
                    hypExistingDeductions.Visible = False

                End If


            End If

            ''---------------------------------------
            'Dim l_CheckBox As CheckBox
            'For Each l_DataGridItem As DataGridItem In dgExistingDeduction.Items

            '    l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")


            '    If Not l_CheckBox Is Nothing Then
            '        l_CheckBox.Checked = True
            '    End If
            'Next
            ''-------------------------------------

        Catch
            Throw
        End Try
    End Sub
    Private Sub Clear_Session()
        Session("flag") = Nothing
        Session("LoanReplace") = Nothing
        Session("CashOutsReplace") = Nothing
        Session("ReplaceFees") = Nothing
        Session("VRManager_Sort") = Nothing
        Session("ShowDisbursement") = Nothing
        Session("DisbursementByPersIdIndex") = Nothing
        Session("g_search_flag") = Nothing
        'Priya 29-April-2010 : YRS 5.0-1060-Voiding a Check
        Session("StatusType") = Nothing
        Session("ANNPaid") = Nothing
        'End YRS 5.0-1060
        Session("g_search_flag") = Nothing
        Session("DisbursementId") = Nothing

    End Sub
    Private Sub ValidateVoidReplace()
        Dim l_datarow_CurrentRow As DataRow
        Dim l_integer_CurrentIndex As Integer
        Dim l_datetime_CompareDate As DateTime
        Dim Strarr As Array
        Dim StrListDisbId As String
        Dim parameterDisbId As String
        Try


            StrListDisbId = hdnListDisbId.Value
            If hdnDisbId.Value.Length > 0 Then
                StrListDisbId = StrListDisbId.Replace(hdnDisbId.Value, "")
            End If

            If StrListDisbId.Length > 0 Then
                'hdnDisbId.Value = hdnDisbId.Value.Substring(1, hdnDisbId.Value.Length - 1)
                Strarr = StrListDisbId.Split(",")
                Dim i As Integer
                If Strarr.Length >= 2 Then
                    For i = 1 To Strarr.Length - 1
                        hdnDisbId.Value = Strarr.GetValue(i)
                        parameterDisbId = hdnDisbId.Value

                        g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")


                        If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                            ''**If Not Session("DisbursementId") Is Nothing Then
                            Dim l_dr_CurrentRow As DataRow()
                            Dim l_searchExpr As String
                            l_searchExpr = "DisbursementID = '" + parameterDisbId + "'"
                            l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                            If l_dr_CurrentRow.Length > 0 Then
                                l_datarow_CurrentRow = l_dr_CurrentRow(0)

                                '1. Validate Disbursement alredy voided    
                                If CType(l_datarow_CurrentRow("Voided"), String).ToUpper = "YES" And (CType(l_datarow_CurrentRow("Reversed"), String)).ToUpper = "YES" Then
                                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement(s) are already voided and reversed. Cannot Replace.", MessageBoxButtons.Stop)
                                    Exit Sub
                                End If

                                'Commented by imran on 4/12/2009 This validation is already done in button click event
                                ''Added by priya on 11-Nov-2009 Validation if deduction amount is greater than net amount.
                                ''2. Validation for if deduction amount is greateer than withdrawal amount
                                'Dim j As Integer
                                'Dim amount As Decimal
                                'amount = 0
                                'For j = 0 To dgExistingDeduction.Items.Count - 1
                                '    Dim chk As CheckBox
                                '    chk = dgExistingDeduction.Items(j).FindControl("CheckBoxDeduction")
                                '    If Not IsNothing(chk) Then
                                '        If chk.Checked Then
                                '            amount = amount + Convert.ToDouble(dgExistingDeduction.Items(j).Cells(3).Text.Trim())
                                '        End If
                                '    End If
                                'Next

                                'If amount >= Convert.ToDecimal(l_datarow_CurrentRow("Amount")) Then
                                '    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", " Please adjust deduction(s).Deduction amount can not be greater than or equal to net amount.", MessageBoxButtons.Stop)
                                '    Session("flag") = "Pending"
                                '    Exit Sub
                                'End If

                                '3.validate date of issue older 
                                l_datetime_CompareDate = DateAdd(DateInterval.Year, 1, Convert.ToDateTime(l_datarow_CurrentRow("Issueddate")))
                                If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then
                                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Issue date for disbursement #" & l_datarow_CurrentRow("Number") & " is older than a year. Do you wish to continue?", MessageBoxButtons.YesNo)
                                    Session("flag") = "Pending"
                                    Exit Sub
                                End If



                            End If
                        End If

                        '------
                    Next
                End If
            End If
            'Added Imran on 05/10/2009

            StrListDisbId = hdnListDisbId.Value
            If StrListDisbId.Length > 0 Then
                Strarr = StrListDisbId.Split(",")
                Dim i As Integer
                If Strarr.Length >= 2 Then
                    'For i = 1 To Strarr.Length - 1
                    If Strarr.GetValue(1) <> "" Then
                        ChangeStatus(Strarr.GetValue(1))
                    End If
                    'Next
                End If
            End If





        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Throw (ex)
        End Try
    End Sub
    'Added on 06/10/2009
    Private Sub savevoidReplace()
        Dim l_dataset_disbDetail As New DataSet
        Dim strOutPut As String
        Dim parameterListDisbId As Array
        Dim i As Integer
        Dim l_DisbursementID As DataColumn = New DataColumn("UniqueID", System.Type.GetType("System.String"))
        Dim l_WithholdingTypeCode As DataColumn = New DataColumn("CodeValue", System.Type.GetType("System.String"))
        'Dim l_Amount As DataColumn = New DataColumn("l_Amount", System.Double.Type.GetType("System.Int64"))
        Dim l_Amount As DataColumn = New DataColumn("Amount", System.Type.GetType("System.Double"))
        Dim l_temprow As DataRow
        Dim l_dataset As New DataSet
        Dim l_DataRow As DataRow
        Dim l_datatable As New DataTable("Deductions")
        Dim l_string_Type As String
        Try

            If hdnListDisbId.Value.Length > 0 Then
                parameterListDisbId = hdnListDisbId.Value.Split(",")
            End If
            If Not Session("DisbursementDetails") Is Nothing Then
                l_dataset_disbDetail = Session("DisbursementDetails")
                Session("DisbursementDetails") = Nothing
            End If
            ButtonSave.Enabled = True

            l_datatable.Columns.Add(l_DisbursementID)
            l_datatable.Columns.Add(l_WithholdingTypeCode)
            l_datatable.Columns.Add(l_Amount)


            For i = 0 To dgExistingDeduction.Items.Count - 1
                Dim chk As CheckBox
                Dim LabelDed As Label
                chk = dgExistingDeduction.Items(i).FindControl("CheckBoxDeduction")

                If Not IsNothing(chk) Then
                    If chk.Checked Then
                        l_temprow = l_datatable.NewRow
                        l_temprow("UniqueID") = String.Empty
                        l_temprow("CodeValue") = dgExistingDeduction.Items(i).Cells(1).Text.Trim()
                        l_temprow("Amount") = Convert.ToDouble(dgExistingDeduction.Items(i).Cells(3).Text.Trim())
                        l_datatable.Rows.Add(l_temprow)
                    End If
                End If
            Next

            For i = 0 To dgDeduction.Items.Count - 1
                Dim chk As CheckBox
                Dim LabelDed As Label
                chk = dgDeduction.Items(i).FindControl("CheckBoxDeduction")

                If Not IsNothing(chk) Then
                    If chk.Checked Then
                        l_temprow = l_datatable.NewRow
                        l_temprow("UniqueID") = String.Empty
                        l_temprow("CodeValue") = dgDeduction.Items(i).Cells(1).Text.Trim()
                        l_temprow("Amount") = Convert.ToDouble(dgDeduction.Items(i).Cells(3).Text.Trim())
                        l_datatable.Rows.Add(l_temprow)
                    End If
                End If
            Next

            If l_datatable.Rows.Count > 0 Then
                l_dataset.Tables.Add(l_datatable)
            End If

            l_string_Type = Request.QueryString.Get("Type")

            strOutPut = ""
            strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoVoidWithdrawalReplace(parameterListDisbId, l_dataset, l_dataset_disbDetail, l_string_Type)

            If (strOutPut <> "") Then
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", strOutPut, MessageBoxButtons.Stop)
                Exit Sub
            Else
                'MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Record saved successfully.", MessageBoxButtons.OK)
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement has been replaced successfully.", MessageBoxButtons.OK)
                hdnListDisbId.Value = ""
                Session("g_search_flag") = False
                Session("DisbursementId") = Nothing
                'Dim l_datetime_CompareDate As DateTime
                ''commented by priya on 12/15/2008 for YRS 5.0-626
                ''l_datetime_CompareDate = DateAdd(DateInterval.Month, 1, Convert.ToDateTime(Session("dsDisbursementsByPersId").Tables(0).Rows(Session("DisbursementIndex"))("AccountDate")))
                '''added by priya on 12/15/2008 for YRS 5.0-626
                'l_datetime_CompareDate = DateAdd(DateInterval.Month, 1, Convert.ToDateTime(Session("dsDisbursementsByPersId").Tables(0).Rows(Session("DisbursementByPersIdIndex"))("AccountDate")))
                'If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then
                '    SendMail()
                'End If


                'ButtonSave.Enabled = False
                Exit Sub
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Throw (ex)
        End Try





    End Sub
    Private Sub SendMail()


        Dim obj As YMCAUI.MailUtil
        obj = New YMCAUI.MailUtil
        Dim l_string_details As String
        Dim l_stringCategory As String = "REFUND"

        Dim l_str_msg As String
        Dim l_CashAppDataTable As New DataTable
        Dim drow As DataRow

        Dim datagrid As DataGrid
        Dim dglst As DataList
        Dim cnt As Integer
        Dim l_string_FundIdNo As String
        Dim l_disbursement_type As String
        Dim l_payment_type As String
        Dim l_check_number As String
        Dim l_disbursement_date As DateTime
        Dim l_voided_date As DateTime
        Dim dsDisbursements As DataSet
        Dim strHTML As StringBuilder
        Dim l_Gross As Decimal


        l_string_FundIdNo = Convert.ToString(Session("dsDisbursements").Tables(0).Rows(0)("FundIDNo")).Trim 'g_dataset_dsDisbursements.Tables(0).Rows(0)("FundIDNo")
        If l_string_FundIdNo = "" Then
            Exit Sub
        End If
        Try
            '=================================================
            If VoidUserControl1.Action = "REFUND" Then
                dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)

                cnt = 0
                For Each item As DataListItem In dglst.Items
                    Dim img As ImageButton

                    img = item.Controls(0).FindControl("Imagebutton1")
                    If img.ImageUrl = "../images/selected.gif" Then
                        datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing(datagrid) Then
                            Dim i As Integer

                            For i = 0 To datagrid.Items.Count - 1
                                Dim chk As CheckBox
                                Dim LabelDisbId As Label
                                chk = datagrid.Items(i).FindControl("chkRefund")
                                LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
                                If Not IsNothing(chk) Then
                                    If chk.Checked Then
                                        dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(LabelDisbId.Text)

                                        If Not dsDisbursements Is Nothing Then
                                            If dsDisbursements.Tables(0).Rows.Count > 0 Then
                                                If i = 0 Then
                                                    strHTML = New StringBuilder
                                                    strHTML.Append("<Html>")
                                                    '-------------CSS------------------------
                                                    strHTML.Append("<style type=text/css>")
                                                    strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
                                                    strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
                                                    strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
                                                    strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

                                                    strHTML.Append("</style>")
                                                    '-------------------------------------

                                                    strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:-</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Disbursement Type:-</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Voided Date:-</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

                                                End If

                                                If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
                                                    strHTML.Append("<tr><td class=labelbold>Disbursement Number:-</td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Disbursement Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td><td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</td></tr>")
                                                Else
                                                    strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Disbursement Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td><td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</td></tr>")
                                                End If

                                                If dsDisbursements.Tables(1).Rows.Count > 0 Then
                                                    Dim counter As Integer
                                                    strHTML.Append("<tr><td colspan=4>")
                                                    strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
                                                    For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
                                                        strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
                                                        l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

                                                    Next
                                                    strHTML.Append("</table>")
                                                    strHTML.Append("</td></tr>")

                                                End If

                                            End If


                                        End If


                                    End If
                                End If
                            Next
                            If l_Gross <> 0 Then
                                strHTML.Append("<tr><td class=labelbold>Gross Fees:-</td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
                            End If

                            strHTML.Append("</table></html>")
                        End If

                    End If
                    cnt += 1
                Next


            ElseIf VoidUserControl1.Action = "TDLOAN" Then

                dglst = CType(VoidUserControl1.FindControl("datalistTDLoan"), DataList)

                For Each item As DataListItem In dglst.Items
                    Dim img As ImageButton
                    img = item.Controls(0).FindControl("imgTDLoan")
                    If img.ImageUrl = "../images/selected.gif" Then
                        datagrid = CType(item.FindControl("dgLoan"), DataGrid)
                        If Not IsNothing(datagrid) Then
                            Dim i As Integer

                            For i = 0 To datagrid.Items.Count - 1
                                Dim chk As CheckBox
                                chk = datagrid.Items(i).FindControl("chkLoan")
                                If Not IsNothing(chk) Then
                                    If chk.Checked Then
                                        ' hdnListDisbId.Value = hdnListDisbId.Value + "," + DataGrid.Items(i).Cells(1).Text
                                        dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(datagrid.Items(i).Cells(1).Text)

                                        If Not dsDisbursements Is Nothing Then
                                            If dsDisbursements.Tables(0).Rows.Count > 0 Then
                                                If i = 0 Then
                                                    strHTML = New StringBuilder
                                                    strHTML.Append("<Html>")
                                                    '-------------CSS------------------------
                                                    strHTML.Append("<style type=text/css>")
                                                    strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
                                                    strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
                                                    strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
                                                    strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

                                                    strHTML.Append("</style>")
                                                    '-------------------------------------

                                                    strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:-</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Disbursement Type:-</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Voided Date:-</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

                                                End If

                                                If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
                                                    strHTML.Append("<tr><td class=labelbold>Disbursement Number:-</td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Disbursement Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</sapn></td></tr>")
                                                Else
                                                    strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Disbursement Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td></tr>")
                                                End If

                                                If dsDisbursements.Tables(1).Rows.Count > 0 Then
                                                    Dim counter As Integer
                                                    strHTML.Append("<tr><td colspan=4>")
                                                    strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
                                                    For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
                                                        strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
                                                        l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

                                                    Next
                                                    strHTML.Append("</table>")
                                                    strHTML.Append("</td></tr>")

                                                End If

                                            End If


                                        End If

                                        ''--------Chek end if                         
                                    End If
                                End If
                            Next
                            If l_Gross <> 0 Then
                                strHTML.Append("<tr><td class=labelbold>Gross Fees:-</td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
                            End If

                            strHTML.Append("</table></html>")
                        End If

                    End If
                Next


            ElseIf VoidUserControl1.Action = "ANNUITY" Then

                dglst = CType(VoidUserControl1.FindControl("datalistAnnuity"), DataList)

                For Each item As DataListItem In dglst.Items
                    Dim img As ImageButton
                    img = item.Controls(0).FindControl("imgAnnuity")
                    If img.ImageUrl = "../images/selected.gif" Then
                        datagrid = CType(item.FindControl("dgAnnuity"), DataGrid)
                        If Not IsNothing(datagrid) Then
                            Dim i As Integer

                            For i = 0 To datagrid.Items.Count - 1
                                Dim chk As CheckBox
                                chk = datagrid.Items(i).FindControl("chkAnnuity")
                                If Not IsNothing(chk) Then
                                    If chk.Checked Then
                                        ' hdnListDisbId.Value = hdnListDisbId.Value + "," + DataGrid.Items(i).Cells(1).Text
                                        dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(datagrid.Items(i).Cells(1).Text)

                                        If Not dsDisbursements Is Nothing Then
                                            If dsDisbursements.Tables(0).Rows.Count > 0 Then
                                                If i = 0 Then
                                                    strHTML = New StringBuilder
                                                    strHTML.Append("<Html>")
                                                    '-------------CSS------------------------
                                                    strHTML.Append("<style type=text/css>")
                                                    strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
                                                    strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
                                                    strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
                                                    strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

                                                    strHTML.Append("</style>")
                                                    '-------------------------------------

                                                    strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:-</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Disbursement Type:-</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Voided Date:-</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

                                                End If

                                                If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
                                                    strHTML.Append("<tr><td class=labelbold>Disbursement Number:-</td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Disbursement Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</sapn></td></tr>")
                                                Else
                                                    strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Disbursement Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td></tr>")
                                                End If

                                                If dsDisbursements.Tables(1).Rows.Count > 0 Then
                                                    Dim counter As Integer
                                                    strHTML.Append("<tr><td colspan=4>")
                                                    strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
                                                    strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
                                                    For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
                                                        strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
                                                        l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

                                                    Next
                                                    strHTML.Append("</table>")
                                                    strHTML.Append("</td></tr>")

                                                End If

                                            End If


                                        End If

                                    End If
                                End If
                            Next
                            If l_Gross.ToString() <> "0" Then
                                strHTML.Append("<tr><td class=labelbold>Gross Fees:-</td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
                            End If

                            strHTML.Append("</table></html>")
                        End If

                    End If
                Next

            End If
            '=================================================

            If obj.MailService = False Then
                Exit Sub
            End If
            obj.SendCc = ""
            obj.MailMessage = strHTML.ToString() '"A withdrawal for fund Id " & l_string_FundIdNo & " which was processed in a prior month has been reversed.  Please review and make necessary interest adjustments as soon as possible."
            obj.Subject = "Withdrawal from prior month has been Replaced."
            obj.MailCategory = "VOID"
            obj.MailFormatType = Mail.MailFormat.Html
            obj.Send()
        Catch
            Throw
        End Try
    End Sub
    'Added on 22/10/2009 For add Header label
    Public Sub LoadGeneraltab(ByVal l_string_PersId As String)
        Try
            Dim g_dataset_GeneralInfo As DataSet
            Dim FName As String
            Dim MName As String
            Dim LName As String
            Dim SSNO As String
            Dim l_string_Type As String

            If Not Request.QueryString.Get("Type") Is Nothing Then
                l_string_Type = Request.QueryString.Get("Type")

                If l_string_Type = "REFUND" Then
                    Headercontrol.PageTitle = "Void and Replace Disbursement / Withdrawals"
                ElseIf (l_string_Type = "TDLOAN") Then
                    Headercontrol.PageTitle = "Void and Replace Disbursement / Loan"
                ElseIf (l_string_Type = "ANNUITY") Then
                    Headercontrol.PageTitle = "Void and Replace Disbursement / Annuity"
                Else
                    Headercontrol.PageTitle = "Void Disbursement"
                End If

            End If
            If l_string_PersId <> "" Then
                g_dataset_GeneralInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(l_string_PersId)
                If Not g_dataset_GeneralInfo Is Nothing Then
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "") Then
                        FName = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString().Trim
                    End If

                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "") Then
                        LName = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString().Trim
                    End If
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "") Then
                        MName = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString().Trim
                    End If

                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "") Then
                        SSNO = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString().Trim
                    End If

                    Session("Person_Info") = Me.TextBoxSSNo.Text
                    Dim strSSN As String = SSNO.Insert(3, "-")
                    strSSN = strSSN.Insert(6, "-")

                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    'Start: Shashi Shekhar: 2010-12-09: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                        Headercontrol.FundNo = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim
                    End If

                End If
            End If
        Catch ex As Exception
            Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub
#End Region

#Region "FUNCTION"
    Private Function LoadDeductions()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Dim l_DataTable As DataTable

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager()
            'Hafiz 03Feb06 Cache-Session

            l_DataTable = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDeductionsList

            dgDeduction.DataSource = l_DataTable
            dgDeduction.DataBind()

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager.Add("R_Deductions", l_DataTable)
            Session("R_Deductions") = l_DataTable
            'Hafiz 03Feb06 Cache-Session

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function IsCashOut(ByVal parameterDisbursementId As String) As Boolean
        Try
            Dim l_IsCashOut As Integer = 0
            l_IsCashOut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.IsCashOut(parameterDisbursementId)
            If l_IsCashOut = 1 Then
                Return True
            Else
                Return False
            End If
        Catch
            Throw
        End Try
    End Function
    Private Function GetDeductions() As Integer
        Dim l_DataTable As DataTable
        Dim l_drow As DataRow
        Dim l_deductionamt As Integer
        l_deductionamt = 0
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDeductions
            If l_DataTable.Rows.Count > 0 Then
                For Each l_drow In l_DataTable.Rows
                    If Not IsDBNull(l_drow("Amount")) Then
                        l_deductionamt = Convert.ToInt32(l_drow("Amount"))
                    End If
                Next
            ElseIf l_DataTable.Rows.Count = 0 Then
                'here we have hard coded search fee as 25 $ when there is no data in configuration table
                l_deductionamt = 25
            End If

            Return l_deductionamt
        Catch
            Throw
        End Try
    End Function
#End Region

    'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
    Public Sub CreateDisbursementDetails(ByVal DisbursementId As String, ByVal dblDisbTaxableAmt As Double, ByVal dblDisbNonTaxableAmt As Double, ByVal dblProRateFactor As Double)
        Dim dsDisbDetail As New DataSet
        Dim tempRow As DataRow
        Try
            dsDisbDetail.Tables.Add("DisbursementDetails")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Clear()
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("DisbursementID")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("DisbursementNumber")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("chrAcctType")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("chrAcctBreakDownType")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("TaxablePrincipal")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("TaxableInterest")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("NonTaxablePrincipal")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("WithheldPrincipal")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("WithheldInterest")
            tempRow = dsDisbDetail.Tables("DisbursementDetails").NewRow

            tempRow("DisbursementID") = DisbursementId
            tempRow("DisbursementNumber") = litDisbNbr.Text.Trim()
            tempRow("chrAcctType") = "BA"
            tempRow("chrAcctBreakDownType") = "18"
            tempRow("TaxablePrincipal") = dblDisbTaxableAmt
            tempRow("TaxableInterest") = 0
            tempRow("NonTaxablePrincipal") = dblDisbNonTaxableAmt
            tempRow("WithheldPrincipal") = Math.Round(dblDisbTaxableAmt * dblProRateFactor, 2)
            tempRow("WithheldInterest") = Math.Round(dblDisbNonTaxableAmt * dblProRateFactor, 2)
            dsDisbDetail.Tables("DisbursementDetails").Rows.Add(tempRow)
            Session("DisbursementDetails") = dsDisbDetail
        Catch ex As Exception
            Session("DisbursementDetails") = Nothing
            HelperFunctions.LogException("Void & Replace Withdrawal --> CreateDisbursementDetails", ex)
            Throw ex
        End Try
    End Sub
    'End :SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
    


End Class
