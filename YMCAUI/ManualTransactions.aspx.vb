'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ManualTransactions.aspx.vb
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 3:16:00 PM
' Program Specification Name	:	YMCA PS 3.12.7.1 
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : Hafiz 04Feb06
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Aparna Samala      12-Oct-2007     Allow the SSNO to be entered with hypens and then remove hypens
'Dilip Yadav :      30-July-09 :    To Implement the N-Tier AnnuityBasis Type logic based on Transaction date.
'Dilip Yadav :      27-Oct-09  :    To Revert back the Implement the N-Tier AnnuityBasis Type logic based on Transaction date.
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Shashi Shekhar     19/Feb/2010     Restrict Data Archived Participants To proceed ahead from Find list.
'Sanjay R.          2010.06.17      Enhancement changes(CType to DirectCast)
'Shashi Shekhar     2010-12-09      For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in Title on all screens.
'Shashi             03 Mar. 2011    Replacing Header formating with user control (YRS 5.0-450 )
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************

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

'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class ManualTransactions
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("ManualTransactions.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridManualTransList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripManualTrans As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageManualTrans As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents LabelWelcomeNote As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFundId As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundId As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelHireDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxHireDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTermDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSN As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSN As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYMCA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCA As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAcctType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListAcctType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelTransType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListTransType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelYMCAPreTax As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCAPreTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMComp As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMComp As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPreTax As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPreTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPostTax As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPostTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAcctDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAcctDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRecDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRecDate As YMCAUI.DateUserControl
    Protected WithEvents LabelTransDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTransDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxFundDate As YMCAUI.DateUserControl
    Protected WithEvents LabelFundDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNotes As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuityBasis As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid


    Protected WithEvents ReqAcctType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqTransType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqNotes As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents CheckBoxClear As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxOverride As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelNoRecFound As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonPrincipal As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonInterest As System.Web.UI.WebControls.RadioButton

    Protected WithEvents RadioButtonPost96 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonPre96 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonRollIns As System.Web.UI.WebControls.RadioButton

    Protected WithEvents ButtonPHR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClearAll As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderManualTrans As System.Web.UI.WebControls.PlaceHolder

    Protected WithEvents LiteralAcctType As System.Web.UI.WebControls.Literal
    Protected WithEvents LiteralTransType As System.Web.UI.WebControls.Literal
    Protected WithEvents LiteralTempIndex As System.Web.UI.WebControls.Literal
    Protected WithEvents DataGridHistory As System.Web.UI.WebControls.DataGrid
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
    Dim g_dataset_dsLookupParticipants As New DataSet
    Dim g_String_Exception_Message As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            Me.TextBoxYMCAPreTax.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxYMCAPreTax.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            Me.TextBoxPreTax.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxPreTax.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            Me.TextBoxPostTax.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxPostTax.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            Me.TextBoxMComp.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxMComp.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            If Not Me.IsPostBack Then
                Session("Sort_ManualTransact") = Nothing
                Me.TabStripManualTrans.Items(1).Enabled = False
            Else
                If (Request.Form("OK") = "OK") Then
                    Me.ReqNotes.Enabled = False
                    Me.ReqAcctType.Enabled = False
                    Me.ReqTransType.Enabled = False
                Else
                    Me.ReqNotes.Enabled = True
                    Me.ReqAcctType.Enabled = True
                    Me.ReqTransType.Enabled = True
                End If
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub TabStripManualTrans_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripManualTrans.SelectedIndexChange
        Try

            If Me.TabStripManualTrans.SelectedIndex = 0 Then
                LiteralTempIndex.Text = Convert.ToString(Session("Selected_participant_index"))
                LiteralAcctType.Text = Convert.ToString(Me.DropDownListAcctType.SelectedItem.Text.Trim())
                LiteralTransType.Text = Convert.ToString(Me.DropDownListTransType.SelectedItem.Text.Trim())

                'Dim l_dataset_ManualTransParticipants As DataSet
                'l_dataset_ManualTransParticipants = AppDomain.CurrentDomain.GetData("ParticipantManualTransacts")
                'Me.DataGridManualTransList.DataSource = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants")
                'Me.DataGridManualTransList.DataBind()
            ElseIf Me.TabStripManualTrans.SelectedIndex = 1 Then
                If LiteralTempIndex.Text = Convert.ToString(Session("Selected_participant_index")) Then
                    PopulateTransactions()
                    Me.DropDownListAcctType.SelectedIndex = -1
                    Me.DropDownListTransType.SelectedIndex = -1
                    Me.DropDownListAcctType.Items.FindByText(Convert.ToString(LiteralAcctType.Text)).Selected = True
                    Me.DropDownListTransType.Items.FindByText(Convert.ToString(LiteralTransType.Text)).Selected = True
                Else
                    Me.TextBoxNotes.Text = ""
                    Me.TextBoxPreTax.Text = "0.00"
                    Me.TextBoxPostTax.Text = "0.00"
                    Me.TextBoxYMCAPreTax.Text = "0.00"
                    PopulateTransactions()
                End If
            End If
            Me.MultiPageManualTrans.SelectedIndex = Me.TabStripManualTrans.SelectedIndex
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Public Sub PopulateTransactions()
        Try
            Dim l_dataset_dsAcctTypes As New DataSet
            Dim l_datset_dsAcctDate As New DataSet
            Dim l_dataset_dsGetTransactTypes As New DataSet
            Dim l_integer_ParicipantIndex As Integer
            Dim l_dataset_ManualTransParticipants As DataSet
            Dim l_dataset_dsGetHistory As DataSet
            Dim l_string_PersId As String
            Dim l_string_FundEventId As String
            Dim l_string_YMCAId As String
            Dim l_String_YMCAName As String
            Dim l_String_SSN As String
            Dim l_String_HistoryListItem As String = ""
            Dim l_drow As DataRow

            l_integer_ParicipantIndex = Session("Selected_participant_index")
            l_dataset_ManualTransParticipants = AppDomain.CurrentDomain.GetData("ParticipantManualTransacts")
            l_string_PersId = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("PersID").ToString().Trim()
            l_string_FundEventId = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("FundEventID").ToString().Trim()
            l_string_YMCAId = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("YMCAID").ToString().Trim()

            If Not (l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("HireDate").ToString().Trim() = "") Then
                'l_Date_HireDate = CType(l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("HireDate").ToString().Trim(), Date)
                Me.TextBoxHireDate.Text = Convert.ToDateTime(l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex).Item("HireDate")).ToString("MM/dd/yyyy")

            Else
                Me.TextBoxHireDate.Text = "NULL"
            End If

            If Not l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("TermDate").ToString().Trim() = "" Then
                Me.TextBoxTermDate.Text = Convert.ToDateTime(l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("TermDate")).ToString("MM/dd/yyyy")
            Else
                Me.TextBoxTermDate.Text = "NULL"
            End If

            l_dataset_dsAcctTypes = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.getAcctTypes(l_string_PersId, l_string_FundEventId)
            If Not l_dataset_dsAcctTypes Is Nothing Then
                Me.DropDownListAcctType.DataSource = l_dataset_dsAcctTypes.Tables("AcctTypes")
                Me.DropDownListAcctType.DataTextField = l_dataset_dsAcctTypes.Tables("AcctTypes").Columns("Short_Desc").ToString().Trim()
                Me.DropDownListAcctType.DataValueField = l_dataset_dsAcctTypes.Tables("AcctTypes").Columns("Acct_Type").ToString().Trim()
                Me.DropDownListAcctType.DataBind()
                Me.DropDownListAcctType.Items.Add("")
                Me.DropDownListAcctType.Items.FindByText("").Selected = True
            End If
            ' binding the transaction type dropdown
            l_dataset_dsGetTransactTypes = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.GetTransactTypes()

            'Shubhrata May9th YRST 2240
            Session("l_dataset_dsGetTransactTypes") = l_dataset_dsGetTransactTypes
            'Shubhrata May9th YRST2240

            If Not l_dataset_dsGetTransactTypes Is Nothing Then
                Me.DropDownListTransType.DataSource = l_dataset_dsGetTransactTypes.Tables("Transact_Types")
                Me.DropDownListTransType.DataTextField = l_dataset_dsGetTransactTypes.Tables("Transact_Types").Columns("ShortDescription").ToString().Trim()
                Me.DropDownListTransType.DataValueField = l_dataset_dsGetTransactTypes.Tables("Transact_Types").Columns("TransactType").ToString().Trim()
                Me.DropDownListTransType.DataBind()
                Me.DropDownListTransType.Items.Add("")
                Me.DropDownListTransType.Items.FindByText("").Selected = True
            End If

            'binding the history datagrid
            l_dataset_dsGetHistory = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.getHistory(l_string_PersId, l_string_FundEventId)
            Me.DataGridHistory.DataSource = l_dataset_dsGetHistory.Tables("History")
            Me.DataGridHistory.DataBind()

            l_String_YMCAName = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("YMCAName").ToString().Trim()
            l_String_SSN = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("SSN").ToString().Trim()

            Me.TextBoxYMCA.Text = l_String_YMCAName
            Me.TextBoxSSN.Text = l_String_SSN

            l_datset_dsAcctDate = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.GetAccountingDate()
            Session("DataSetAcctDate") = l_datset_dsAcctDate
            Me.TextBoxAcctDate.Text = Convert.ToDateTime(l_datset_dsAcctDate.Tables("Acct_Date").Rows(0)("AcctDate")).ToString("MM/dd/yyyy")


            'setting the dates in Rec Date, Fund Date and Trans Date to system's date

            Me.TextBoxFundDate.Text = Convert.ToDateTime(Date.Today).ToString("MM/dd/yyyy")
            Me.TextBoxRecDate.Text = Convert.ToDateTime(Date.Today).ToString("MM/dd/yyyy")
            Me.TextBoxTransDate.Text = Convert.ToDateTime(Date.Today).ToString("MM/dd/yyyy")
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub CheckBoxOverride_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxOverride.CheckedChanged
        Try
            If CheckBoxOverride.Checked = True Then
                Me.TextBoxYMCAPreTax.Enabled = True
            Else
                Me.TextBoxYMCAPreTax.Enabled = False
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Public Sub PopulateParticipants()
        Try
            Dim l_string_SSN As String
            Dim l_string_FundIdNo As String
            Dim l_string_FirstName As String
            Dim l_string_LastName As String

            l_string_SSN = Me.TextBoxSSNo.Text.ToString().Trim()
            l_string_FundIdNo = Me.TextBoxFundId.Text.ToString().Trim()
            l_string_FirstName = Me.TextBoxFirstName.Text.ToString().Trim()
            l_string_LastName = Me.TextBoxLastName.Text.ToString().Trim()

            g_dataset_dsLookupParticipants = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.LookUpParticipants(l_string_SSN, l_string_FundIdNo, l_string_FirstName, l_string_LastName)
            If g_dataset_dsLookupParticipants.Tables("ManualTransactParticipants").Rows.Count = 0 Then
                Me.LabelNoRecFound.Visible = True
                DataGridManualTransList.Visible = False
            Else
                DataGridManualTransList.Visible = True
                Me.DataGridManualTransList.DataSource = g_dataset_dsLookupParticipants.Tables("ManualTransactParticipants")
                viewstate("DS_Sort_ManualTrans") = g_dataset_dsLookupParticipants

                Me.DataGridManualTransList.DataBind()
                Me.LabelNoRecFound.Visible = False
            End If

            'Me.DataGridManualTransList.SelectedIndex = 0

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try

            If (TextBoxFirstName.Text = "" And TextBoxLastName.Text = "" And TextBoxFundId.Text = "" And TextBoxSSNo.Text = "") Then
                MessageBox.Show(PlaceHolderManualTrans, "YRS", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.Stop)
            Else
                TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")
                PopulateParticipants()
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxLastName.Text = ""
            Me.TextBoxFirstName.Text = ""
            Me.TextBoxFundId.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.LabelNoRecFound.Visible = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridManualTransList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridManualTransList.SelectedIndexChanged
        Try

            '----Shashi Shekhar:2010-02-19: Code to handle Archived Participants from list--------------
            If Me.DataGridManualTransList.SelectedItem.Cells(10).Text.ToUpper.Trim() = "TRUE" Then
                MessageBox.Show(Me.PlaceHolderManualTrans, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                Me.TabStripManualTrans.Items(1).Enabled = False
                Headercontrol.PageTitle = String.Empty
                Exit Sub
            End If
            '---------------------------------------------------------------------------------------


            Me.TabStripManualTrans.Items(1).Enabled = True
            Me.TabStripManualTrans.SelectedIndex = 1
            Me.MultiPageManualTrans.SelectedIndex = 1
            Session("Selected_participant_index") = Me.DataGridManualTransList.SelectedIndex
            Session("SSNoManualTransact") = Me.DataGridManualTransList.SelectedItem.Cells(1).Text.Trim

            If (Me.DataGridManualTransList.SelectedItem.Cells(11).Text <> "System.DBNull" And Me.DataGridManualTransList.SelectedItem.Cells(11).Text <> "") Then
                Headercontrol.PageTitle = "Manual Transactions"
                Headercontrol.FundNo = Me.DataGridManualTransList.SelectedItem.Cells(11).Text
            End If

            PopulateTransactions()

            Me.TextBoxNotes.Text = ""
            Me.TextBoxPreTax.Text = "0.00"
            Me.TextBoxPostTax.Text = "0.00"
            Me.TextBoxYMCAPreTax.Text = "0.00"

            Me.DropDownListAcctType.SelectedIndex = -1
            Me.DropDownListTransType.SelectedIndex = -1
            Me.DropDownListAcctType.Items.FindByText("").Selected = True
            Me.DropDownListTransType.Items.FindByText("").Selected = True
            'Me.DropDownListTransType.Items.FindByText(Convert.ToString(LiteralTransType.Text)).Selected = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
        
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_datset_dsAcctDate As New DataSet
            Dim l_integer_ParicipantIndex As Integer
            Dim l_dataset_ManualTransParticipants As DataSet
            Dim l_dataset_dsGetTransactions As DataSet
            'Shubhrata YRST 2240
            Dim l_dataset_dsGetTransactTypesForInterests As New DataSet
            Dim l_string_TransactType As String
            Dim l_datarow As DataRow

            'Shubhrata YRST 2240

            Dim l_boolean_MessageFlag As Boolean = False

            Dim l_Date_acctDate As Date
            Dim l_Date_FundDate As Date
            Dim l_Date_RecDate As Date
            Dim l_Date_TransDate As Date
            Dim l_Date_HireDate As Date
            Dim l_Date_TermDate As Date

            Dim l_string_AnnuityBasisType As String
            Dim l_string_PersId As String
            Dim l_string_FundEventId As String
            Dim l_string_YMCAId As String
            Dim l_String_YMCAName As String
            Dim l_String_SSN As String

            Dim l_decimal_SumPerPreTax As Decimal = 0
            Dim l_decimal_SumPerPosttax As Decimal = 0
            Dim l_decimal_SumYMCAPreTax As Decimal = 0
            Dim l_decimal_SumMonthlyComp As Decimal = 0

            Dim l_decimal_PerPreTax As Decimal
            Dim l_decimal_PerPosttax As Decimal
            Dim l_decimal_YMCAPreTax As Decimal
            Dim l_decimal_MonthlyComp As Decimal
            Dim l_integer_Output As Integer

            If Me.DropDownListAcctType.SelectedItem.ToString = "" Then
                Exit Sub
            End If


            l_dataset_ManualTransParticipants = AppDomain.CurrentDomain.GetData("ParticipantManualTransacts")
            l_string_PersId = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("PersID").ToString().Trim()
            l_string_FundEventId = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("FundEventID").ToString().Trim()
            l_string_YMCAId = l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("YMCAID").ToString().Trim()
            '''l_Date_HireDate = CType(l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("HireDate").ToString().Trim(), Date)
            '' l_datset_dsAcctDate = System.AppDomain.CurrentDomain.GetData("DataSetAcctDate")
            l_datset_dsAcctDate = Session("DataSetAcctDate")
            l_Date_acctDate = CType(l_datset_dsAcctDate.Tables("Acct_Date").Rows(0)("AcctDate").ToString().Trim(), Date)

            l_integer_ParicipantIndex = Session("Selected_participant_index")

            If Me.DropDownListAcctType.SelectedValue = "RT" Or Me.DropDownListAcctType.SelectedValue = "RP" And l_boolean_MessageFlag = False Then
                If Not RadioButtonRollIns.Checked = True Then
                    ' show error message
                    'top=130 and left=230 removed by anita on 01-06-2007
                    MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Basis type ROLLING is allowed for account type RT and RP only.Please change basis type or account type.", MessageBoxButtons.Stop)
                    l_boolean_MessageFlag = True
                End If
            End If

            'only negavtive values to be allowed for roll ins
            If Me.RadioButtonRollIns.Checked And l_boolean_MessageFlag = False Then
                If CType(Me.TextBoxPostTax.Text.Trim(), Decimal) > 0 Or CType(Me.TextBoxPreTax.Text.Trim(), Decimal) > 0 Or CType(Me.TextBoxYMCAPreTax.Text.Trim(), Decimal) > 0 Then
                    'show error...no negative values for roll in annuity basis type
                    'top=130 and left=230 removed by anita on 01-06-2007
                    MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Only negative amounts are allowed for Roll-Ins.Please use Roll-Ins module for positive amounts.", MessageBoxButtons.Stop)
                    l_boolean_MessageFlag = True
                End If
            End If

            'comparing the dates

            l_Date_HireDate = Me.TextBoxHireDate.Text
            If Me.TextBoxFundDate.Text <> "" Then
                l_Date_FundDate = Me.TextBoxFundDate.Text
            Else
                l_Date_FundDate = Convert.ToDateTime("01/01/1900")
            End If
            If Me.TextBoxRecDate.Text <> "" Then
                l_Date_RecDate = Me.TextBoxRecDate.Text
            Else
                l_Date_RecDate = Convert.ToDateTime("01/01/1900")
            End If
            If Me.TextBoxTransDate.Text <> "" Then
                l_Date_TransDate = Me.TextBoxTransDate.Text
            Else
                l_Date_TransDate = Convert.ToDateTime("01/01/1900")
            End If

            If Not Me.TextBoxTermDate.Text.Trim().ToUpper() = "Null".ToUpper() Then
                l_Date_TermDate = Me.TextBoxTermDate.Text
            End If

            'Shubhrata May9th YRST2240
            l_dataset_dsGetTransactTypesForInterests = Session("l_dataset_dsGetTransactTypes")
            l_string_TransactType = Me.DropDownListTransType.SelectedItem.Value

            For Each l_datarow In l_dataset_dsGetTransactTypesForInterests.Tables("Transact_Types").Rows

                If l_datarow("TransactType").ToString.ToUpper = l_string_TransactType Then
                    Exit For
                End If

            Next

            If DirectCast(l_datarow("Interest"), Boolean) <> True Then   'Directcast(for Ctype) implemented by SR:2010.06.28 for migration
                'Shubhrata May9th YRST2240  

                If l_dataset_ManualTransParticipants.Tables("ManualTransactParticipants").Rows(l_integer_ParicipantIndex)("Status").ToString().Trim() = "Inactive" Then
                    If l_boolean_MessageFlag = False And l_Date_TransDate <> Convert.ToDateTime("01/01/1900") Then
                        If (System.DateTime.Compare(l_Date_HireDate, l_Date_TransDate) = 1 Or System.DateTime.Compare(l_Date_TransDate, l_Date_TermDate) = 1) Then
                            'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                            'top=130 and left=230 removed by anita on 01-06-2007
                            MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_TermDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                            l_boolean_MessageFlag = True
                        End If
                    End If
                    If l_boolean_MessageFlag = False And l_Date_RecDate <> Convert.ToDateTime("01/01/1900") Then
                        If (System.DateTime.Compare(l_Date_HireDate, l_Date_RecDate) = 1 Or System.DateTime.Compare(l_Date_RecDate, l_Date_TermDate) = 1) Then
                            'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                            'top=130 and left=230 removed by anita on 01-06-2007
                            MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_TermDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                            l_boolean_MessageFlag = True
                        End If
                    End If
                    If l_boolean_MessageFlag = False And l_Date_FundDate <> Convert.ToDateTime("01/01/1900") Then
                        If (System.DateTime.Compare(l_Date_HireDate, l_Date_FundDate) = 1 Or System.DateTime.Compare(l_Date_FundDate, l_Date_TermDate) = 1) Then
                            'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                            'top=130 and left=230 removed by anita on 01-06-2007
                            MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_TermDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                            l_boolean_MessageFlag = True
                        End If
                    End If

                Else
                    If l_boolean_MessageFlag = False And l_Date_FundDate <> Convert.ToDateTime("01/01/1900") Then
                        If (System.DateTime.Compare(l_Date_HireDate, l_Date_FundDate) = 1 Or System.DateTime.Compare(l_Date_FundDate, l_Date_acctDate) = 1) Then
                            'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                            'top=130 and left=230 removed by anita on 01-06-2007
                            MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_acctDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                            l_boolean_MessageFlag = True
                        End If
                    End If
                    If l_boolean_MessageFlag = False And l_Date_RecDate <> Convert.ToDateTime("01/01/1900") Then
                        If (System.DateTime.Compare(l_Date_HireDate, l_Date_RecDate) = 1 Or System.DateTime.Compare(l_Date_RecDate, l_Date_acctDate) = 1) Then
                            'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                            'top=130 and left=230 removed by anita on 01-06-2007
                            MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_acctDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                            l_boolean_MessageFlag = True
                        End If
                    End If
                    If l_boolean_MessageFlag = False And l_Date_TransDate <> Convert.ToDateTime("01/01/1900") Then
                        If (System.DateTime.Compare(l_Date_HireDate, l_Date_TransDate) = 1 Or System.DateTime.Compare(l_Date_TransDate, l_Date_acctDate) = 1) Then
                            'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                            'top=130 and left=230 removed by anita on 01-06-2007
                            MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_acctDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                            l_boolean_MessageFlag = True
                        End If
                    End If

                    '''If l_boolean_MessageFlag = False And (System.DateTime.Compare(l_Date_HireDate, l_Date_FundDate) = 1 Or System.DateTime.Compare(l_Date_FundDate, l_Date_acctDate) = 1 Or System.DateTime.Compare(l_Date_HireDate, l_Date_RecDate) = 1 Or System.DateTime.Compare(l_Date_RecDate, l_Date_acctDate) = 1 Or System.DateTime.Compare(l_Date_HireDate, l_Date_TransDate) = 1 Or System.DateTime.Compare(l_Date_TransDate, l_Date_acctDate) = 1) Then
                    '''    'if the fund date, rec date, trans date do not lie between hire date and acct date..show error message
                    '''    MessageBox.Show(130, 230, PlaceHolderManualTrans, "YMCA-YRS", "Transaction's funding date, received date and transaction date should be between " + l_Date_HireDate.ToShortDateString() + " and " + l_Date_acctDate.ToShortDateString() + ".Please correct the corresponding date and continue.", MessageBoxButtons.Stop)
                    '''    l_boolean_MessageFlag = True
                    '''End If

                End If
            End If
            ' -- Commented (As per mail from client)logic (To Implement N-Tier Annuitybasis ) by : Dilip Yadav : 27-Oct-09 : 
            '' ---- Start : Added By Dilip : 31-July-09 : To Implement N-Tier Annuitybasis logic ---- 
            'Dim l_string_Annuitygroup As String

            'If Me.RadioButtonPost96.Checked = True Then
            '    l_string_Annuitygroup = "PST"

            'ElseIf Me.RadioButtonPre96.Checked = True Then
            '    l_string_Annuitygroup = "PRE"

            'ElseIf Me.RadioButtonRollIns.Checked Then
            '    l_string_Annuitygroup = "ROL"
            'End If

            'Dim l_ds_AnnuityBasisType As DataSet
            'l_ds_AnnuityBasisType = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.GetAnnuityBasisType(l_Date_TransDate, l_string_Annuitygroup)

            'If HelperFunctions.isNonEmpty(l_ds_AnnuityBasisType) Then
            '    l_string_AnnuityBasisType = l_ds_AnnuityBasisType.Tables(0).Rows(0)("AnnuityBasisType").ToString()
            'End If          
            '' ---- END : Added By Dilip : 31-July-09 : To Implement N-Tier Annuitybasis logic ---- 
            'getting the annuity basis type
            If Me.RadioButtonPost96.Checked = True Then
                l_string_AnnuityBasisType = "PST96"
            ElseIf Me.RadioButtonPre96.Checked = True Then
                l_string_AnnuityBasisType = "PRE96"
            ElseIf Me.RadioButtonRollIns.Checked Then
                l_string_AnnuityBasisType = "ROLL"
            End If

            l_decimal_PerPreTax = Me.TextBoxPreTax.Text
            l_decimal_PerPosttax = Me.TextBoxPostTax.Text
            l_decimal_YMCAPreTax = Me.TextBoxYMCAPreTax.Text
            l_decimal_MonthlyComp = Me.TextBoxMComp.Text

            'fetching the corresponding data for the transaction in dataset
            l_dataset_dsGetTransactions = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.GetTransactions(l_string_PersId, l_string_FundEventId, l_string_AnnuityBasisType, Me.DropDownListAcctType.SelectedValue.Trim(), Me.DropDownListTransType.SelectedValue.Trim())

            If Not l_dataset_dsGetTransactions.Tables("Transactions").Rows.Count = 0 Then
                l_decimal_SumPerPreTax = l_dataset_dsGetTransactions.Tables("Transactions").Rows(0)("PersonalPreTax").ToString().Trim()
                l_decimal_SumPerPosttax = l_dataset_dsGetTransactions.Tables("Transactions").Rows(0)("PersonalPostTax").ToString().Trim()
                l_decimal_SumYMCAPreTax = l_dataset_dsGetTransactions.Tables("Transactions").Rows(0)("YMCAPreTax").ToString().Trim()
                l_decimal_SumMonthlyComp = l_dataset_dsGetTransactions.Tables("Transactions").Rows(0)("MonthlyComp").ToString().Trim()

                If (l_decimal_PerPreTax < 0) And (l_decimal_PerPreTax + l_decimal_SumPerPreTax) < 0 And l_boolean_MessageFlag = False Then
                    'top=130 and left=230 removed by anita on 01-06-2007
                    MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction amount can't be more than -" + l_decimal_SumPerPreTax + "Please correct personal pre tax value before proceeding.", MessageBoxButtons.Stop)
                    l_boolean_MessageFlag = True
                End If
                If (l_decimal_PerPosttax < 0) And (l_decimal_PerPosttax + l_decimal_SumPerPosttax) < 0 And l_boolean_MessageFlag = False Then
                    'top=130 and left=230 removed by anita on 01-06-2007
                    MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction amount can't be more than -" + l_decimal_PerPosttax + "Please correct personal post tax value before proceeding.", MessageBoxButtons.Stop)
                    l_boolean_MessageFlag = True
                End If
                If (l_decimal_YMCAPreTax < 0) And (l_decimal_YMCAPreTax + l_decimal_SumYMCAPreTax) < 0 And l_boolean_MessageFlag = False Then
                    'top=130 and left=230 removed by anita on 01-06-2007
                    MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction amount can't be more than -" + l_decimal_YMCAPreTax + "Please correct YMCA pre tax value before proceeding.", MessageBoxButtons.Stop)
                    l_boolean_MessageFlag = True
                End If
                If (l_decimal_MonthlyComp < 0) And (l_decimal_MonthlyComp + l_decimal_SumMonthlyComp) < 0 And l_boolean_MessageFlag = False Then
                    'top=130 and left=230 removed by anita on 01-06-2007
                    MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction amount can't be more than -" + l_decimal_MonthlyComp + "Please correct monthly comp value before proceeding.", MessageBoxButtons.Stop)
                    l_boolean_MessageFlag = True
                End If
            End If

            'getting the annuity basis type
            If Me.RadioButtonPost96.Checked = True Then
                l_string_AnnuityBasisType = "PST96"
            ElseIf Me.RadioButtonPre96.Checked = True Then
                l_string_AnnuityBasisType = "PRE96"
            ElseIf Me.RadioButtonRollIns.Checked Then
                l_string_AnnuityBasisType = "ROLL"
            End If

            'validations done...save the data
            If l_boolean_MessageFlag = False Then
                l_integer_Output = YMCARET.YmcaBusinessObject.ManualTransactionsBOClass.SaveTransaction(l_string_PersId, l_string_YMCAId, l_string_FundEventId, Me.DropDownListAcctType.SelectedValue.Trim(), Me.DropDownListTransType.SelectedValue.Trim(), l_string_AnnuityBasisType, l_decimal_MonthlyComp, l_decimal_PerPreTax, l_decimal_PerPosttax, l_decimal_YMCAPreTax, l_Date_RecDate, l_Date_TransDate, l_Date_FundDate, l_Date_acctDate, Me.TextBoxNotes.Text.Trim())
            End If
            If l_boolean_MessageFlag = False And l_integer_Output = 0 Then
                'top=130 and left=230 removed by anita on 01-06-2007
                MessageBox.Show(PlaceHolderManualTrans, "YMCA-YRS", "Transaction Saved SuccessFully", MessageBoxButtons.OK)
            End If
            'clearing the texboxes after save...if the user opts for..
            If Me.CheckBoxClear.Checked And l_boolean_MessageFlag = False Then
                Me.TextBoxNotes.Text = ""
                Me.TextBoxYMCAPreTax.Text = "0.00"
                Me.TextBoxPreTax.Text = "0.00"
                Me.TextBoxPostTax.Text = "0.00"
                Me.TextBoxFundDate.Text = Date.Today.ToShortDateString()
                Me.TextBoxRecDate.Text = Date.Today.ToShortDateString()
                'Me.TextBoxTransDate.Text = Date.Today.ToShortDateString()
                Me.TextBoxTransDate.Text = Date.Today.ToLongDateString()
                Me.DropDownListAcctType.SelectedIndex = -1
                Me.DropDownListTransType.SelectedIndex = -1
                Me.DropDownListAcctType.Items.FindByText("").Selected = True
                Me.DropDownListTransType.Items.FindByText("").Selected = True
                Me.RadioButtonPrincipal.Checked = True
                Me.RadioButtonPost96.Checked = True
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonClearAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClearAll.Click
        Try
            Me.TextBoxNotes.Text = ""
            Me.TextBoxYMCAPreTax.Text = "0.00"
            Me.TextBoxPreTax.Text = "0.00"
            Me.TextBoxPostTax.Text = "0.00"
            Me.TextBoxFundDate.Text = Date.Today.ToShortDateString()
            Me.TextBoxRecDate.Text = Date.Today.ToShortDateString()
            'Me.TextBoxTransDate.Text = Date.Today.ToShortDateString()
            Me.TextBoxTransDate.Text = Date.Today.ToLongDateString()
            Me.DropDownListAcctType.SelectedIndex = -1
            Me.DropDownListTransType.SelectedIndex = -1
            Me.DropDownListAcctType.Items.FindByText("").Selected = True
            Me.DropDownListTransType.Items.FindByText("").Selected = True
            Me.RadioButtonPrincipal.Checked = True
            Me.RadioButtonPost96.Checked = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridManualTransList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridManualTransList.ItemCommand
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.CommandName = "Select") Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            Dim i As Integer
            i = 0
            While i < DataGridManualTransList.Items.Count
                l_button_Select = DataGridManualTransList.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i <> e.Item.ItemIndex Then
                        l_button_Select.ImageUrl = "images\select.gif"
                    Else
                        l_button_Select.ImageUrl = "images\selected.gif"
                    End If
                End If
                i = i + 1

            End While
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Session("Sort_ManualTransact") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub RadioButtonInterest_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonInterest.CheckedChanged
        Try
            If Me.RadioButtonInterest.Checked Then
                Me.TextBoxPostTax.Enabled = False
            Else
                Me.TextBoxPostTax.Enabled = True
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonPHR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPHR.Click
        Try
            ''Me.ProcessReport()
            Session("ManualTransacts") = "ManualTransacts"
            Session("SSNo") = Session("SSNoManualTransact")
            Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('RetireesFrmMoverWebForm.aspx', 'ReportPopUp', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        End Try

    End Sub
    Private Sub ProcessReport()
        Try
            'Report Name = DeathBenefitOptions.rpt
            'Number of Parameters = 1
            'Parameter : PersID

            'Set the Report Variables in Session
            Session("strReportName") = "YRSParticipantHistoryReportBySSNo"
            Session("ArrList") = Nothing

            Session("ListBoxSelectedItems") = Nothing



            'Call ReportViewer.aspx 
            Me.OpenReportViewer()

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub
    Private Sub OpenReportViewer()
        Try
            'Call ReportViewer.aspx 
            Dim popupScript As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub DataGridManualTransList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridManualTransList.SortCommand
        Try
            Me.DataGridManualTransList.SelectedIndex = -1
            If Not viewstate("DS_Sort_ManualTrans") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsLookupParticipants = viewstate("DS_Sort_ManualTrans")
                dv = g_dataset_dsLookupParticipants.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Sort_ManualTransact") Is Nothing Then
                    If Session("Sort_ManualTransact").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridManualTransList.DataSource = Nothing
                Me.DataGridManualTransList.DataSource = dv
                Me.DataGridManualTransList.DataBind()
                Session("Sort_ManualTransact") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

   
End Class
