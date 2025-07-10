'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AccountTypesForm.aspx.vb
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 4:14:07 PM
' Program Specification Name	:	YMCA PS 5.4.1.1
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

' Changed by			:	Tushar Joshi    
' Changed on			:	26th.July.2005
' Change Description	:	Coding 
' Changed by			:	Shefali Bharti  
' Changed on			:	08/19/2005
' Change Description	:	Coding
'/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'****************************************************
'Modidfication History 
'****************************************************
'Modified By        Date            Description
'****************************************************
'Shubhrata          05/17/2007      Plan Split Changes : to relate the account types with the Account Groups and the Plan type
'Neeraj Singh       12/11/2009      Added form name for security issue YRS 5.0-940 
'Nikunj Patel		06/15/2010		Rewriting and rearranging code to make it work with .Net 4.0 and to reduce the use of sessions
'Sanjay Rawat       20/08/2010      Code changed to avoid null value to be inserted
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja K            2019.28.02      YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'*******************************************************************************

Option Strict Off
Option Explicit On

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Data.SqlClient

Public Class AccountTypesForm
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridAccountTypes As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelAcctType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLongDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEffDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRefund As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBasicAcct1 As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxBasicAcct1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelVestReq As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxVestReq As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelBasicAcct2 As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxBasicAcct2 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelLumpSum As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxLumpSum As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAdmin As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents TextBoxAcctType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLongDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxEffDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxTermDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxRefund As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEmployerMoney As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxEmployerMoney As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelEmployeeMoney As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxEmployeeMoney As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelEmpTaxDefer As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxEmpTaxDefer As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelIncDeathBen As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxIncDeathBen As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    'Plan Split Changes
    Protected WithEvents LabelAccountGroup As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListAccountGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RadioButtonRetirementPlan As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonSavingsPlans As System.Web.UI.WebControls.RadioButton

    'Plan Split Changes
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Global Variable declaration goes here.

    Dim g_dataset_dsAccountType As DataSet
    Dim g_bool_SearchFlag As Boolean
    Dim g_integer_count As Integer
    Dim strFormName As String = New String("AccountTypesForm.aspx")
    Dim Page_Mode As String = String.Empty
    Dim g_string_UniqueIdOfSelectedItem As String = String.Empty
    Dim g_MessageBox_CallBack_Function As String = String.Empty

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'If Session("LoggedUserKey") Is Nothing Then
        '	Response.Redirect("Login.aspx", False)
        'End If

        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
        Try
            If Not Me.IsPostBack Then
                Page_Mode = String.Empty
                g_string_UniqueIdOfSelectedItem = String.Empty
                DisableDataEntryControls()
                DisableSaveCancelButtons()
                Me.TextBoxEffDate.RequiredDate = True

                g_bool_SearchFlag = False

                LoadAccountGroups()
                LoadAccountData()
                HelperFunctions.BindGrid(DataGridAccountTypes, g_dataset_dsAccountType.Tables(0).DefaultView)
            End If
            If IsPostBack AndAlso g_MessageBox_CallBack_Function <> String.Empty Then
                HandleMessageBox()
                Exit Sub
            End If
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub HandleMessageBox()
        Dim strFunctions() As String = g_MessageBox_CallBack_Function.Split(":")
        g_MessageBox_CallBack_Function = String.Empty
        If strFunctions.Length = 0 OrElse strFunctions.Length Mod 2 = 1 Then Exit Sub
        Dim i As Integer = 0
        For i = 0 To strFunctions.Length - 1 Step 2
            If Request(strFunctions(i)) IsNot Nothing AndAlso String.Compare(Request(strFunctions(i)), strFunctions(i), True) = 0 Then
                ExecuteMethod(strFunctions(i + 1))
                Exit For
            End If
        Next
    End Sub
    Private Sub ExecuteMethod(ByVal methodName)
        Select Case methodName
            Case "DiscardAndAdd"
            Case "DiscardAndSearch"
            Case "DiscardAndSelectItem"
        End Select
    End Sub

    Public Sub LoadAccountData()
        If g_bool_SearchFlag = True Then
            g_dataset_dsAccountType = YMCARET.YmcaBusinessObject.AccountTypes.SearchAccountType(Me.TextBoxFind.Text)
        Else
            g_dataset_dsAccountType = YMCARET.YmcaBusinessObject.AccountTypes.LookUpAccountType()
        End If

        If HelperFunctions.isEmpty(g_dataset_dsAccountType) Then
            LabelNoRecordFound.Visible = True
            DataGridAccountTypes.SelectedIndex = -1
            HelperFunctions.BindGrid(DataGridAccountTypes, g_dataset_dsAccountType)
        Else
            LabelNoRecordFound.Visible = False
            DataGridAccountTypes.SelectedIndex = -1
            HelperFunctions.BindGrid(DataGridAccountTypes, g_dataset_dsAccountType)
        End If
    End Sub
    Private Function PopulateDataIntoControls(ByVal parameterSelectedAcctType As String) As Boolean

        If parameterSelectedAcctType = String.Empty Then
            DisableDataEntryControls()
            DisableSaveCancelButtons()
            DataGridAccountTypes.SelectedIndex = -1
            g_string_UniqueIdOfSelectedItem = String.Empty
        End If

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        If g_dataset_dsAccountType Is Nothing Then Return False

        If HelperFunctions.isEmpty(g_dataset_dsAccountType) Then Return False

        l_DataTable = g_dataset_dsAccountType.Tables("Account Type")
        If l_DataTable Is Nothing Then Return False

        l_DataRow = HelperFunctions.GetRowForUpdation(l_DataTable, "[Acct. Type]", parameterSelectedAcctType)

        If l_DataRow Is Nothing Then Return False

        If Not l_DataRow.IsNull("Acct. Type") Then
            Me.TextBoxAcctType.Text = DirectCast(l_DataRow("Acct. Type"), String)
        Else
            Me.TextBoxAcctType.Text = ""
        End If
        If Not l_DataRow.IsNull("Short Description") Then
            Me.TextBoxShortDesc.Text = DirectCast(l_DataRow("Short Description"), String)
        Else
            Me.TextBoxShortDesc.Text = ""
        End If
        If Not l_DataRow.IsNull(" Long Desc") Then
            Me.TextBoxLongDesc.Text = DirectCast(l_DataRow(" Long Desc"), String)
        Else
            Me.TextBoxLongDesc.Text = ""
        End If
        If Not l_DataRow.IsNull("Effec. Date") Then
            Me.TextBoxEffDate.Text = DirectCast(l_DataRow("Effec. Date"), Date).ToShortDateString()
        Else
            Me.TextBoxEffDate.Text = ""
        End If
        If Not l_DataRow.IsNull("Termination Date") Then
            Me.TextBoxTermDate.Text = DirectCast(l_DataRow("Termination Date"), Date).ToShortDateString()
        Else
            Me.TextBoxTermDate.Text = ""
        End If
        If Not l_DataRow.IsNull("Refund Priority Level") Then
            Me.TextBoxRefund.Text = DirectCast(l_DataRow("Refund Priority Level"), Integer)
        Else
            Me.TextBoxRefund.Text = "0"
        End If
        If Not l_DataRow.IsNull("Basic Acct") Then
            Me.CheckBoxBasicAcct1.Checked = DirectCast(l_DataRow("Basic Acct"), Boolean)
        Else
            Me.CheckBoxBasicAcct1.Checked = False
        End If
        If Not l_DataRow.IsNull("Vesting Required") Then
            Me.CheckBoxVestReq.Checked = DirectCast(l_DataRow("Vesting Required"), Boolean)
        Else
            Me.CheckBoxVestReq.Checked = False
        End If
        If Not l_DataRow.IsNull("Salary Required") Then
            Me.CheckBoxBasicAcct2.Checked = DirectCast(l_DataRow("Salary Required"), Boolean)
        Else
            Me.CheckBoxBasicAcct2.Checked = False
        End If

        If Not l_DataRow.IsNull("Lump Sum") Then
            Me.CheckBoxLumpSum.Checked = DirectCast(l_DataRow("Lump Sum"), Boolean)
        Else
            Me.CheckBoxLumpSum.Checked = False
        End If
        If Not l_DataRow.IsNull("Employer Money") Then
            Me.CheckBoxEmployerMoney.Checked = DirectCast(l_DataRow("Employer Money"), Boolean)
        Else
            Me.CheckBoxEmployerMoney.Checked = False
        End If
        If Not l_DataRow.IsNull("Employee Money") Then
            Me.CheckBoxEmployeeMoney.Checked = DirectCast(l_DataRow("Employee Money"), Boolean)
        Else
            Me.CheckBoxEmployeeMoney.Checked = False
        End If
        If Not l_DataRow.IsNull("Included Death Bene.") Then
            Me.CheckBoxIncDeathBen.Checked = DirectCast(l_DataRow("Included Death Bene."), Boolean)
        Else
            Me.CheckBoxIncDeathBen.Checked = False
        End If
        If Not l_DataRow.IsNull("Employer Tax Defer") Then
            Me.CheckBoxEmpTaxDefer.Checked = DirectCast(l_DataRow("Employer Tax Defer"), Boolean)
        Else
            Me.CheckBoxEmpTaxDefer.Checked = False
        End If
        'Shubhrata Plan Split Changes
        If l_DataRow.IsNull("PlanType") Then
            Me.RadioButtonRetirementPlan.Checked = False
            Me.RadioButtonSavingsPlans.Checked = False
        ElseIf String.Compare(DirectCast(l_DataRow("PlanType"), String).Trim, "RETIREMENT") = 0 Then
            Me.RadioButtonRetirementPlan.Checked = True
            Me.RadioButtonSavingsPlans.Checked = False
        ElseIf String.Compare(DirectCast(l_DataRow("PlanType"), String).Trim, "SAVINGS") = 0 Then
            Me.RadioButtonRetirementPlan.Checked = False
            Me.RadioButtonSavingsPlans.Checked = True
        End If
        If l_DataRow.IsNull("AcctGroups") Then
            Me.DropDownListAccountGroup.SelectedIndex = 0
            Me.DropDownListAccountGroup.SelectedValue = ""
        Else
            Me.DropDownListAccountGroup.DataValueField = CType(l_DataRow("AcctGroups"), String).Trim.ToUpper
            Me.SetAccountGroupDropDownSelected(CType(l_DataRow("AcctGroups"), String).Trim.ToUpper)
        End If

        g_string_UniqueIdOfSelectedItem = parameterSelectedAcctType
        EnableDataEntryControls()
        EnableSaveCancelButtons()
        'Shubhrata Plan Split Changes
        Return True

    End Function

    Private Sub DataGridAccountTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridAccountTypes.SelectedIndexChanged
        Try
            g_string_UniqueIdOfSelectedItem = DataGridAccountTypes.SelectedItem.Cells(1).Text
            If PopulateDataIntoControls(g_string_UniqueIdOfSelectedItem) = True Then
                Page_Mode = "EDIT"
                EnableSaveCancelButtons()
                EnableDataEntryControls()
            Else
                Page_Mode = String.Empty
                DisableDataEntryControls()
                DisableSaveCancelButtons()
                'TODO: Show an error message here if we were not able to go into EDIT mode
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Try
            Page_Mode = String.Empty
            If g_dataset_dsAccountType IsNot Nothing Then g_dataset_dsAccountType.RejectChanges()
            g_string_UniqueIdOfSelectedItem = String.Empty
            If TextBoxFind.Text.Trim() = String.Empty Then
                g_bool_SearchFlag = False
            Else
                g_bool_SearchFlag = True
            End If
            BlankOutAllDataEntryControls()
            ' Do search & populate the data.
            LoadAccountData()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Page_Mode = String.Empty
            'Enable / Disable the controls
            DisableSaveCancelButtons()
            BlankOutAllDataEntryControls()
            DisableDataEntryControls()
            If g_dataset_dsAccountType IsNot Nothing Then g_dataset_dsAccountType.RejectChanges()
            g_string_UniqueIdOfSelectedItem = String.Empty
            DataGridAccountTypes.SelectedIndex = -1
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Session("ATF_g_dataset_dsAccountType") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            Page_Mode = "ADD"
            If g_dataset_dsAccountType IsNot Nothing Then g_dataset_dsAccountType.RejectChanges()
            g_string_UniqueIdOfSelectedItem = String.Empty
            DataGridAccountTypes.SelectedIndex = -1
            HelperFunctions.BindGrid(DataGridAccountTypes, g_dataset_dsAccountType)
            BlankOutAllDataEntryControls()
            EnableDataEntryControls()
            EnableSaveCancelButtons()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Function PageHasChanges() As Boolean
        If g_dataset_dsAccountType Is Nothing Then Return False
        If Page_Mode = "ADD" Then Return True
        Dim dr As DataRow = HelperFunctions.GetRowForUpdation(g_dataset_dsAccountType.Tables(0), "[Acct. Type]", g_string_UniqueIdOfSelectedItem)
        If Not dr Is Nothing Then
            AssignValuesToDataRow(dr)
        End If
        Return g_dataset_dsAccountType.HasChanges()
    End Function

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_DataRow As DataRow

        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'If add Flag Is true then perform additional validations
            If Page_Mode = "ADD" Then
                'Validation
                If Me.DropDownListAccountGroup.SelectedValue = String.Empty Then
                    MessageBox.Show(PlaceHolder1, "YRS", "Please select an Account Group.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                If RadioButtonRetirementPlan.Checked = False AndAlso RadioButtonSavingsPlans.Checked = False Then
                    MessageBox.Show(PlaceHolder1, "YRS", "Please select a plan type", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                l_DataRow = g_dataset_dsAccountType.Tables(0).NewRow()
                AssignValuesToDataRow(l_DataRow)
                g_dataset_dsAccountType.Tables(0).Rows.Add(l_DataRow)
            Else
                If HelperFunctions.isNonEmpty(g_dataset_dsAccountType) Then
                    l_DataRow = HelperFunctions.GetRowForUpdation(g_dataset_dsAccountType.Tables(0), "[Acct. Type]", g_string_UniqueIdOfSelectedItem)
                    If Not l_DataRow Is Nothing Then
                        AssignValuesToDataRow(l_DataRow)
                    End If
                End If
            End If
            ' Call business layer to Save the DataSet
            YMCARET.YmcaBusinessObject.AccountTypes.SaveAccountType(g_dataset_dsAccountType)

            g_string_UniqueIdOfSelectedItem = String.Empty
            Page_Mode = String.Empty
            DataGridAccountTypes.SelectedIndex = -1
            HelperFunctions.BindGrid(DataGridAccountTypes, g_dataset_dsAccountType.Tables(0).DefaultView)
            BlankOutAllDataEntryControls()
            DisableDataEntryControls()
            DisableSaveCancelButtons()

        Catch sqlEx As SqlException
            If sqlEx.Number = 60010 Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Duplicate Record", MessageBoxButtons.OK)
            Else
                Dim ex As Exception
                Dim l_String_Exception_Message As String
                l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
                Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridAccountTypes_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridAccountTypes.SortCommand
        'NP:Comment - Rewriting the logic for sorting the datagrid
        Try
            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are search results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGridAccountTypes.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(g_dataset_dsAccountType) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        g_dataset_dsAccountType.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        g_dataset_dsAccountType.Tables(0).DefaultView.Sort = IIf(g_dataset_dsAccountType.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    g_dataset_dsAccountType.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                HelperFunctions.BindGrid(DataGridAccountTypes, g_dataset_dsAccountType.Tables(0).DefaultView)
                SetSelectedImageOnDataGridOnSorting()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Shubhrata May 17th 2007 Plan Split Changes
    Private Sub LoadAccountGroups()
        Dim l_dataset_GetAccountGroups As DataSet
        l_dataset_GetAccountGroups = YMCARET.YmcaBusinessObject.AccountTypes.GetAccountGroups()
        If HelperFunctions.isNonEmpty(l_dataset_GetAccountGroups) Then
            Me.DropDownListAccountGroup.DataSource = l_dataset_GetAccountGroups
            Me.DropDownListAccountGroup.DataValueField = "AccountGroups"
            Me.DropDownListAccountGroup.DataTextField = "AccountGroupDescription"
            Me.DropDownListAccountGroup.DataBind()
            Me.DropDownListAccountGroup.Items.Insert(0, "-Account Groups-")
            Me.DropDownListAccountGroup.Items(0).Value = ""
        End If
    End Sub

    Private Sub SetAccountGroupDropDownSelected(ByVal parameterAccountGroup As String)
        Dim li As ListItem = Me.DropDownListAccountGroup.Items.FindByValue(parameterAccountGroup)
        If li Is Nothing Then
            li = New ListItem(parameterAccountGroup, parameterAccountGroup)
            DropDownListAccountGroup.Items.Add(li)
            Me.DropDownListAccountGroup.SelectedIndex = DropDownListAccountGroup.Items.Count - 1
        Else
            DropDownListAccountGroup.SelectedIndex = DropDownListAccountGroup.Items.IndexOf(li)
        End If
    End Sub
    Private Function SetSelectedImageOnDataGridOnSorting()
        Try
            Dim dgi As DataGridItem
            Dim l_counter As Integer
            l_counter = -1
            For Each dgi In Me.DataGridAccountTypes.Items
                l_counter = l_counter + 1
                If dgi.Cells(1).Text = g_string_UniqueIdOfSelectedItem Then
                    Exit For
                End If
            Next
            Me.DataGridAccountTypes.SelectedIndex = l_counter
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Shubhrata May 17th 2007 Plan Split Changes

    Private Function AssignValueToColumn(ByVal dr As DataRow, ByVal columnName As String, ByRef tb As TextBox) As Boolean
        If dr.IsNull(columnName) Then
            If tb.Text.Trim <> String.Empty Then dr(columnName) = tb.Text.Trim()
        Else
            If tb.Text.Trim <> DirectCast(dr(columnName), String) Then dr(columnName) = tb.Text.Trim()
        End If
        Return True
    End Function
    Private Function AssignValueToColumn(ByVal dr As DataRow, ByVal columnName As String, ByRef chkBox As CheckBox) As Boolean
        If dr.IsNull(columnName) Then
            'If chkBox.Checked Then dr(columnName) = chkBox.Checked
            dr(columnName) = IIf(chkBox.Checked, True, False)  'SR:2010.08.20 - To avoid null value to be inserted
        Else
            If chkBox.Checked <> DirectCast(dr(columnName), Boolean) Then dr(columnName) = chkBox.Checked
        End If
    End Function
    Private Function AssignValueToColumn(ByVal dr As DataRow, ByVal columnName As String, ByRef drpDwnList As DropDownList) As Boolean
        If dr.IsNull(columnName) Then
            If drpDwnList.SelectedValue <> String.Empty Then dr(columnName) = drpDwnList.SelectedValue
        Else
            If drpDwnList.SelectedValue <> DirectCast(dr(columnName), String) Then dr(columnName) = drpDwnList.SelectedValue
        End If
    End Function
    Private Function AssignValueToColumn(ByVal dr As DataRow, ByVal columnName As String, ByRef dtControl As DateUserControl) As Boolean
        Dim dbDate, uiDate As String
        If dr.IsNull(columnName) = False Then
            dbDate = DirectCast(dr(columnName), DateTime).ToString("MM/dd/yyyy")
        Else
            dbDate = String.Empty
        End If
        If dtControl.Text.trim = String.Empty Then
            uiDate = String.Empty
        Else
            uiDate = CType(dtControl.Text.Trim, DateTime).ToString("MM/dd/yyyy")
        End If
        If dbDate <> uiDate Then
            dr(columnName) = DateTime.Parse(dtControl.Text.Trim)
        End If
        Return True
    End Function

    Private Sub AssignValuesToDataRow(ByVal drAccountRow As DataRow)
        'drAccountRow.Item("Acct. Type") = TextBoxAcctType.Text.Trim
        'drAccountRow.Item("Short Description") = TextBoxShortDesc.Text.Trim
        'drAccountRow.Item(" Long Desc") = TextBoxLongDesc.Text.Trim
        'Dim dateTemp As Date
        'If TextBoxEffDate.Text.Trim = String.Empty Then
        '	drAccountRow.Item("Effec. Date") = DBNull.Value
        'Else
        '	DateTime.TryParse(TextBoxEffDate.Text.Trim, dateTemp)
        '	drAccountRow.Item("Effec. Date") = dateTemp
        'End If
        'If TextBoxTermDate.Text.Trim = String.Empty Then
        '	drAccountRow.Item("Termination Date") = DBNull.Value
        'Else
        '	DateTime.TryParse(TextBoxTermDate.Text.Trim, dateTemp)
        '	drAccountRow.Item("Termination Date") = dateTemp
        'End If

        Dim intRefundPriority As Integer
        Integer.TryParse(TextBoxRefund.Text.Trim, intRefundPriority)
        If drAccountRow.IsNull("Refund Priority Level") Then
            drAccountRow.Item("Refund Priority Level") = intRefundPriority
        Else
            If intRefundPriority <> DirectCast(drAccountRow("Refund Priority Level"), Integer) Then drAccountRow.Item("Refund Priority Level") = intRefundPriority
        End If

        'drAccountRow.Item("Basic Acct") = CheckBoxBasicAcct1.Checked
        'drAccountRow.Item("Vesting Required") = CheckBoxVestReq.Checked

        'drAccountRow.Item("Salary Required") = CheckBoxBasicAcct2.Checked
        'drAccountRow.Item("Lump Sum") = CheckBoxLumpSum.Checked

        'drAccountRow.Item("Employer Money") = CheckBoxEmployerMoney.Checked
        'drAccountRow.Item("Employee Money") = CheckBoxEmployeeMoney.Checked

        'drAccountRow.Item("Employer Tax Defer") = CheckBoxEmpTaxDefer.Checked
        'drAccountRow.Item("Included Death Bene.") = CheckBoxIncDeathBen.Checked

        'Shubhrata Plan Split Changes
        If Me.RadioButtonRetirementPlan.Checked = True Then
            If drAccountRow.Item("PlanType").ToString() <> "RETIREMENT" Then drAccountRow.Item("PlanType") = "RETIREMENT"
        Else
            If drAccountRow.Item("PlanType").ToString() <> "SAVINGS" Then drAccountRow.Item("PlanType") = "SAVINGS"
        End If
        'If DropDownListAccountGroup.SelectedValue = String.Empty Then
        '	drAccountRow.Item("AcctGroups") = DBNull.Value
        'Else
        '	drAccountRow.Item("AcctGroups") = Me.DropDownListAccountGroup.SelectedValue
        'End If
        AssignValueToColumn(drAccountRow, "Acct. Type", TextBoxAcctType)
        AssignValueToColumn(drAccountRow, "Short Description", TextBoxShortDesc)
        AssignValueToColumn(drAccountRow, " Long Desc", TextBoxLongDesc)
        AssignValueToColumn(drAccountRow, "Effec. Date", TextBoxEffDate)
        AssignValueToColumn(drAccountRow, "Termination Date", TextBoxTermDate)
        AssignValueToColumn(drAccountRow, "Basic Acct", CheckBoxBasicAcct1)
        AssignValueToColumn(drAccountRow, "Vesting Required", CheckBoxVestReq)
        AssignValueToColumn(drAccountRow, "Salary Required", CheckBoxBasicAcct2)
        AssignValueToColumn(drAccountRow, "Lump Sum", CheckBoxLumpSum)
        AssignValueToColumn(drAccountRow, "Employer Money", CheckBoxEmployerMoney)
        AssignValueToColumn(drAccountRow, "Employee Money", CheckBoxEmployeeMoney)
        AssignValueToColumn(drAccountRow, "Employer Tax Defer", CheckBoxEmpTaxDefer)
        AssignValueToColumn(drAccountRow, "Included Death Bene.", CheckBoxIncDeathBen)
        AssignValueToColumn(drAccountRow, "AcctGroups", DropDownListAccountGroup)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        HelperFunctions.SetSelectedImageOfDataGrid(DataGridAccountTypes, Nothing, "ImageButtonSelect")
    End Sub

    Private Sub BlankOutAllDataEntryControls()
        ' Making TextBoxes Blank
        Me.TextBoxAcctType.Text = String.Empty
        Me.TextBoxEffDate.Text = String.Empty
        Me.TextBoxLongDesc.Text = String.Empty
        Me.TextBoxRefund.Text = String.Empty
        Me.TextBoxShortDesc.Text = String.Empty
        Me.TextBoxTermDate.Text = String.Empty
        Me.CheckBoxBasicAcct1.Checked = False
        Me.CheckBoxBasicAcct2.Checked = False
        Me.CheckBoxEmployeeMoney.Checked = False
        Me.CheckBoxEmployerMoney.Checked = False
        Me.CheckBoxEmpTaxDefer.Checked = False
        Me.CheckBoxIncDeathBen.Checked = False
        Me.CheckBoxLumpSum.Checked = False
        Me.CheckBoxVestReq.Checked = False

        'Plan Split Changes
        Me.RadioButtonSavingsPlans.Checked = False
        Me.RadioButtonRetirementPlan.Checked = False
        Me.DropDownListAccountGroup.SelectedValue = ""
        'Plan Split Changes
    End Sub

    Private Sub SetDataEntryControls(ByVal status As Boolean)

        Me.TextBoxAcctType.ReadOnly = Not status
        Me.TextBoxAcctType.Enabled = status
        Me.TextBoxEffDate.Enabled = status

        Me.TextBoxLongDesc.ReadOnly = Not status
        Me.TextBoxRefund.ReadOnly = Not status
        Me.TextBoxShortDesc.ReadOnly = Not status
        Me.TextBoxTermDate.Enabled = status

        Me.CheckBoxBasicAcct1.Enabled = status
        Me.CheckBoxBasicAcct2.Enabled = status
        Me.CheckBoxEmployeeMoney.Enabled = status
        Me.CheckBoxEmployerMoney.Enabled = status
        Me.CheckBoxEmpTaxDefer.Enabled = status
        Me.CheckBoxIncDeathBen.Enabled = status
        Me.CheckBoxLumpSum.Enabled = status
        Me.CheckBoxVestReq.Enabled = status
        'Plan Split Changes
        Me.DropDownListAccountGroup.Enabled = status
        Me.RadioButtonRetirementPlan.Enabled = status
        Me.RadioButtonSavingsPlans.Enabled = status
        'Plan Split Changes
    End Sub
    Private Sub DisableDataEntryControls()
        SetDataEntryControls(False)
    End Sub
    Private Sub EnableDataEntryControls()
        SetDataEntryControls(True)
        If Page_Mode = "EDIT" Then  'Disable the Account Group and Account Name and Plan Type fields
            TextBoxAcctType.Enabled = False
            DropDownListAccountGroup.Enabled = False
            RadioButtonRetirementPlan.Enabled = False
            RadioButtonSavingsPlans.Enabled = False
        End If
    End Sub

    Private Sub SetSaveCancelButtons(ByVal status As Boolean)
        ButtonSave.Enabled = status
        ButtonCancel.Enabled = status
        ButtonAdd.Enabled = Not status
        ButtonOK.Enabled = Not status
        DataGridAccountTypes.Enabled = Not status
        TextBoxFind.Enabled = Not status
        ButtonSearch.Enabled = Not status
    End Sub
    Private Sub DisableSaveCancelButtons()
        SetSaveCancelButtons(False)
    End Sub
    Private Sub EnableSaveCancelButtons()
        SetSaveCancelButtons(True)
    End Sub

#Region "Persistence Mechanism"
    Protected Overrides Function SaveViewState() As Object
        ViewState("Page_Mode") = Page_Mode
        ViewState("BoolSearchFlag") = g_bool_SearchFlag
        ViewState("StringUniqueIdOfSelectedItem") = g_string_UniqueIdOfSelectedItem
        ViewState("g_MessageBox_CallBack_Function") = g_MessageBox_CallBack_Function
        Session("ATF_g_dataset_dsAccountType") = g_dataset_dsAccountType
        Return MyBase.SaveViewState()
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        g_dataset_dsAccountType = TryCast(Session("ATF_g_dataset_dsAccountType"), DataSet)
        g_MessageBox_CallBack_Function = DirectCast(ViewState("g_MessageBox_CallBack_Function"), String)
        g_string_UniqueIdOfSelectedItem = DirectCast(ViewState("StringUniqueIdOfSelectedItem"), String)
        g_bool_SearchFlag = DirectCast(ViewState("BoolSearchFlag"), Boolean)
        Page_Mode = DirectCast(ViewState("Page_Mode"), String)
    End Sub
#End Region
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            DataGridAccountTypes.Enabled = False
            DataGridAccountTypes.ToolTip = tooltip
            ButtonAdd.Enabled = False
            ButtonAdd.ToolTip = tooltip
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
