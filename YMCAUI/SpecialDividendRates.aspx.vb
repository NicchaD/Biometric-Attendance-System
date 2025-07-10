'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	SpecialDividendRates.aspx.vb
' Author Name		:	Shubhrata
' Employee ID		:	34774
' Email				:	shubhrata.tripathi@3i-infotech.com
' Contact No		:	8642
' Creation Time		:	12/12/2005 2.00 PM
'GEmini Id- YREN 2950
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modified By                    Date                Description
'**************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Singh                   16/june/2010        Migration related changes.
'Shashi Singh                   07/July/2010        Code review changes.
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja K                        2019.28.02          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'*******************************************************************************
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
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class SpecialDividendRates
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SpecialDividendRates.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelPayRollDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPercentage As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPercentage As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelCompletedOn As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxCompletedOn As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPayRollDate As YMCAUI.DateUserControl
    Protected WithEvents LabelCompletedBy As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxCompletedBy As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridSpecialDividendData As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidatorForNumPercentage As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents CustomValidator1 As System.Web.UI.WebControls.CustomValidator
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Global Declarations"
    Dim g_String_Exception_Message As String = ""
#End Region
#Region "Properties"
    Private Property DataSetSpecialDividendData() As DataSet
        Get
            If Not (Session("DataSetSpecialDividendData")) Is Nothing Then
                Return (DirectCast(Session("DataSetSpecialDividendData"), DataSet))
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As DataSet)
            Session("DataSetSpecialDividendData") = Value
        End Set
    End Property
    Private Property DataTableForSave() As DataTable
        Get
            If Not (Session("DataTableForSave")) Is Nothing Then
                Return (DirectCast(Session("DataTableForSave"), DataTable))
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As DataTable)
            Session("DataTableForSave") = Value
        End Set
    End Property
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_dataset_LoadExperienceDividendData As New DataSet
        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Not IsPostBack Then
                Session("SelectedIndex") = Nothing
                Session("UniqueIdOfSelectedItem") = Nothing
                Session("IsDateValid") = Nothing
                Me.TextBoxPercentage.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.MakeTextBoxesReadOnly()

                Me.ButtonAdd.Enabled = True
                Me.ButtonDelete.Enabled = True
                Me.ButtonOK.Enabled = True

                Me.ButtonCancel.Enabled = False

                Me.TextBoxPayRollDate.RequiredDate = True
                Viewstate("IsAddorEdit") = Nothing
                l_dataset_LoadExperienceDividendData = Me.LoadExperienceDividendData()
                Me.LoadExpDividendIntoControls(l_dataset_LoadExperienceDividendData, Session("UniqueIdOfSelectedItem"))
                Me.SetSelectedImageOnDataGrid()
                Me.EnableDisableSaveButton()
            End If
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
            If IsPostBack Then
                If Request.Form("Yes") = "Yes" Then
                    Me.DeleteExpDividendData(Me.DataGridSpecialDividendData.SelectedItem.Cells(1).Text)
                End If
                If Request.Form("Ok") = "OK" And Session("IsDateValid") <> False Then
                    l_dataset_LoadExperienceDividendData = Me.LoadExperienceDividendData()
                    Me.LoadExpDividendIntoControls(l_dataset_LoadExperienceDividendData, Session("UniqueIdOfSelectedItem"))
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
    Private Function LoadExperienceDividendData() As DataSet
        Dim l_dataset_GetExperienceDividendData As DataSet = New DataSet
        Try
            l_dataset_GetExperienceDividendData = YMCARET.YmcaBusinessObject.SpecialDividendRatesBOClass.GetExperienceDividendData
            If Not l_dataset_GetExperienceDividendData Is Nothing Then
                If l_dataset_GetExperienceDividendData.Tables.Count > 0 Then
                    If l_dataset_GetExperienceDividendData.Tables("SpecialDividendData").Rows.Count > 0 Then
                        If Session("SelectedIndex") Is Nothing Then
                            Me.DataGridSpecialDividendData.SelectedIndex = 0
                        Else
                            Me.DataGridSpecialDividendData.SelectedIndex = Session("SelectedIndex")
                        End If
                        'we will stroe the dataset for add, edit 
                        Me.DataSetSpecialDividendData = l_dataset_GetExperienceDividendData
                        Me.DataGridSpecialDividendData.DataSource = l_dataset_GetExperienceDividendData.Tables("SpecialDividendData")
                        Me.DataGridSpecialDividendData.DataBind()
                        If Session("UniqueIdOfSelectedItem") Is Nothing Then
                            Session("UniqueIdOfSelectedItem") = Convert.ToString(l_dataset_GetExperienceDividendData.Tables("SpecialDividendData").Rows(0)("UniqueID"))
                        End If

                    ElseIf l_dataset_GetExperienceDividendData.Tables("SpecialDividendData").Rows.Count = 0 Then
                        Me.LabelNoRecordFound.Visible = True
                        Me.ButtonDelete.Enabled = False
                        Me.DataSetSpecialDividendData = l_dataset_GetExperienceDividendData

                    End If
                End If
            End If
            Return l_dataset_GetExperienceDividendData
        Catch
            Throw
        End Try
    End Function
    Private Function LoadExpDividendIntoControls(ByVal parameterDataSet As DataSet, ByVal paramaterUniqueIdOfSelectedItem As String)
        Dim l_datarow_LoanExpDividendData As DataRow()
        Try
            'Putting the values of the first row in Text Boxes
            If Not parameterDataSet Is Nothing Then
                If parameterDataSet.Tables.Count > 0 Then
                    If parameterDataSet.Tables("SpecialDividendData").Rows.Count > 0 Then
                        l_datarow_LoanExpDividendData = parameterDataSet.Tables("SpecialDividendData").Select("UniqueId= '" & paramaterUniqueIdOfSelectedItem & "'")
                        If l_datarow_LoanExpDividendData.Length > 0 Then
                            If Not IsDBNull(l_datarow_LoanExpDividendData(0)("PayRollDate")) Then
                                Me.TextBoxPayRollDate.Text = l_datarow_LoanExpDividendData(0)("PayRollDate")
                            Else
                                Me.TextBoxPayRollDate.Text = ""
                            End If
                            If Not IsDBNull(l_datarow_LoanExpDividendData(0)("Percentage")) Then
                                Me.TextBoxPercentage.Text = l_datarow_LoanExpDividendData(0)("Percentage")
                            Else
                                Me.TextBoxPercentage.Text = ""
                            End If
                            If Not IsDBNull(l_datarow_LoanExpDividendData(0)("Status")) Then
                                Me.DropdownlistStatus.SelectedValue = l_datarow_LoanExpDividendData(0)("Status")
                            Else
                                Me.DropdownlistStatus.SelectedValue = ""
                            End If
                            If Not IsDBNull(l_datarow_LoanExpDividendData(0)("CompletedOn")) Then
                                Me.TextboxCompletedOn.Text = l_datarow_LoanExpDividendData(0)("CompletedOn")
                            Else
                                Me.TextboxCompletedOn.Text = ""
                            End If
                            If Not IsDBNull(l_datarow_LoanExpDividendData(0)("CompletedBy")) Then
                                Me.TextboxCompletedBy.Text = l_datarow_LoanExpDividendData(0)("CompletedBy")
                            Else
                                Me.TextboxCompletedBy.Text = ""
                            End If
                        End If

                        'Ready the data table in case user doesnt selects any records
                        Dim l_datatable_SpecialDividendData As DataTable
                        Dim l_datarow_SpecialDividendData As DataRow
                        l_datatable_SpecialDividendData = Me.DataSetSpecialDividendData.Tables("SpecialDividendData").Clone
                        l_datarow_SpecialDividendData = l_datatable_SpecialDividendData.NewRow()
                        l_datarow_SpecialDividendData("UniqueId") = Me.DataGridSpecialDividendData.SelectedItem.Cells(1).Text
                        l_datarow_SpecialDividendData("PayRollDate") = Me.DataGridSpecialDividendData.SelectedItem.Cells(2).Text
                        l_datarow_SpecialDividendData("Percentage") = Me.DataGridSpecialDividendData.SelectedItem.Cells(3).Text
                        l_datarow_SpecialDividendData("Status") = Me.DataGridSpecialDividendData.SelectedItem.Cells(4).Text
                        If Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text.Trim.ToUpper <> "&NBSP;" Then
                            l_datarow_SpecialDividendData("CompletedOn") = Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text
                        End If

                        l_datarow_SpecialDividendData("CompletedBy") = Me.DataGridSpecialDividendData.SelectedItem.Cells(6).Text

                        l_datatable_SpecialDividendData.Rows.Add(l_datarow_SpecialDividendData)
                        l_datatable_SpecialDividendData.AcceptChanges()
                        'this data table will be used for saving after modifications on edit and add button click 

                        Me.DataTableForSave = l_datatable_SpecialDividendData

                        'Me.EnableDisableSaveButton()
                    End If
                End If

            End If

        Catch
            Throw
        End Try
    End Function
    Private Function SaveToDataBase()
        Try
            Dim l_dataset_SaveToDB As New DataSet
            If Not Me.DataTableForSave Is Nothing Then
                l_dataset_SaveToDB.Tables.Add(Me.DataTableForSave)
                YMCARET.YmcaBusinessObject.SpecialDividendRatesBOClass.InsertUpdateExperienceDividendData(l_dataset_SaveToDB)
            End If
            Dim l_dataset_ExperienceDividendData As New DataSet

            If Viewstate("IsAddorEdit") = "Add" Then
                Session("SelectedIndex") = Nothing
                Session("UniqueIdOfSelectedItem") = Nothing
                l_dataset_ExperienceDividendData = Me.LoadExperienceDividendData()
                Me.LoadExpDividendIntoControls(l_dataset_ExperienceDividendData, Session("UniqueIdOfSelectedItem"))
                Me.DataGridSpecialDividendData.SelectedIndex = 0
            ElseIf Viewstate("IsAddorEdit") = "Edit" Then
                l_dataset_ExperienceDividendData = Me.LoadExperienceDividendData()
                Me.LoadExpDividendIntoControls(l_dataset_ExperienceDividendData, Session("UniqueIdOfSelectedItem"))
                Me.SetSelectedImageOnDataGridOnSorting()
            End If
            Viewstate("IsAddorEdit") = Nothing
        Catch
            Throw
        End Try
    End Function
    Private Function DeleteExpDividendData(ByVal paramaterUniqueId As String)
        Dim l_dataset_ExperienceDividendData As New DataSet
        Try
            YMCARET.YmcaBusinessObject.SpecialDividendRatesBOClass.DeleteExpDividendDate(paramaterUniqueId)
            Session("UniqueIdOfSelectedItem") = Nothing
            Session("SelectedIndex") = Nothing
            l_dataset_ExperienceDividendData = Me.LoadExperienceDividendData()
            If l_dataset_ExperienceDividendData.Tables(0).Rows.Count = 0 Then
                Me.DataGridSpecialDividendData.DataSource = Nothing
                Me.DataGridSpecialDividendData.DataBind()
                Me.TextBoxPayRollDate.Text = ""
                Me.TextBoxPercentage.Text = ""
                Me.MakeTextBoxesReadOnly()
                Me.ButtonSave.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.ButtonCancel.Enabled = False
                Me.RequiredFieldValidatorForNumPercentage.Enabled = False
                'Me.RangeValidatorNumPercentage.Enabled = False
                Me.TextBoxPayRollDate.RequiredDate = False
            Else

                Me.LoadExpDividendIntoControls(l_dataset_ExperienceDividendData, Session("UniqueIdOfSelectedItem"))
                Me.SetSelectedImageOnDataGrid()
            End If

        Catch
            Throw
        End Try
    End Function

    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Try
            If Me.DataGridSpecialDividendData.SelectedIndex = -1 Then
                'prompt the user to select atleast one record to delete
                'LAST PARAMETER REMOVED BY ANITA FOR IE7
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Please select a record to delete.", MessageBoxButtons.OK)
            Else
                If Me.DropdownlistStatus.SelectedValue = "C" And Me.TextboxCompletedOn.Text <> "" And Me.TextboxCompletedBy.Text <> "" Then
                    'LAST PARAMETER REMOVED BY ANITA FOR IE7
                    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "The record cannot be deleted as Special Dividend is complete.", MessageBoxButtons.Stop)
                Else
                    'LAST PARAMETER REMOVED BY ANITA FOR IE7
                    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Are you sure you want to delete?", MessageBoxButtons.YesNo)
                End If
            End If
            Me.ButtonAdd.Enabled = True
            Me.MakeTextBoxesReadOnly()
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim l_dataset_ExperienceDividendData As New DataSet
        Try
            If Me.LabelNoRecordFound.Visible = True Then
                Me.TextBoxPayRollDate.Text = ""
                Me.TextBoxPercentage.Text = ""
                Me.ButtonDelete.Enabled = False
            Else
                Me.ButtonDelete.Enabled = True
            End If
            l_dataset_ExperienceDividendData = Me.LoadExperienceDividendData()
            Me.LoadExpDividendIntoControls(l_dataset_ExperienceDividendData, Session("UniqueIdOfSelectedItem"))
            Me.DataGridSpecialDividendData.SelectedIndex = Session("SelectedIndex")
            Viewstate("IsAddorEdit") = Nothing
            Session("IsDateValid") = Nothing
            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False

            Me.MakeTextBoxesReadOnly()
            CheckReadOnlyMode() 'method called here
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
        Dim l_datarow_SaveForEdit As DataRow
        Try
            'If Me.DropdownlistStatus.SelectedValue = "C" And Me.TextboxCompletedOn.Text <> "" And Me.TextboxCompletedBy.Text <> "" Then

            '    Me.DropdownlistStatus.Enabled = False
            'Else
            '    'Me.DropdownlistStatus.Enabled = True
            'End If
            Me.TextBoxPayRollDate.Enabled = True
            Me.TextBoxPercentage.ReadOnly = False
            Me.TextboxCompletedBy.Enabled = False
            Me.TextboxCompletedOn.Enabled = False
            Me.ButtonSave.Enabled = True
            Me.ButtonEdit.Enabled = False
            'set flag for edit 
            Viewstate("IsAddorEdit") = "Edit"

        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_datarow_SaveForAdd As DataRow
        Dim l_datarow_SaveForEdit As DataRow
        Dim l_datatable_SaveForAdd As DataTable
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'we will reset the DataTableForSave as now newly added rows are needed to be carried

            If Not Viewstate("IsAddorEdit") Is Nothing Then
                If Viewstate("IsAddorEdit") = "Add" Then
                    'only if the payment date >= todays date the new record is allowed
                    If CType(Me.TextBoxPayRollDate.Text.Trim, System.DateTime) < System.DateTime.Now.Date Then
                        'last parameter set to false by Anita on 31-05-2007
                        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Payroll date cannot be less than today's date.", MessageBoxButtons.Stop, False)
                        Session("IsDateValid") = False
                        Exit Sub
                    Else
                        Session("IsDateValid") = True
                        Me.DataTableForSave = Nothing
                        If Not Me.DataSetSpecialDividendData Is Nothing Then
                            If Me.DataSetSpecialDividendData.Tables.Count > 0 Then
                                l_datatable_SaveForAdd = Me.DataSetSpecialDividendData.Tables("SpecialDividendData").Clone
                                l_datarow_SaveForAdd = l_datatable_SaveForAdd.NewRow()
                                l_datarow_SaveForAdd("UniqueId") = ""
                                l_datarow_SaveForAdd("PayRollDate") = Me.TextBoxPayRollDate.Text.Trim
                                If Me.TextBoxPercentage.Text.Trim.Length = 0 Then
                                    l_datarow_SaveForAdd("Percentage") = 0.0
                                Else
                                    l_datarow_SaveForAdd("Percentage") = Me.TextBoxPercentage.Text.Trim
                                End If

                                l_datarow_SaveForAdd("Status") = Me.DropdownlistStatus.SelectedValue
                                'If Me.TextboxCompletedOn.Text.Trim.Length = 0 Then
                                '    l_datarow_SaveForAdd("CompletedOn") = "01/01/1900"
                                'Else
                                '    l_datarow_SaveForAdd("CompletedOn") = Me.TextboxCompletedOn.Text.Trim
                                'End If
                                'l_datarow_SaveForAdd("CompletedBy") = Me.TextboxCompletedBy.Text.Trim
                                l_datatable_SaveForAdd.Rows.Add(l_datarow_SaveForAdd)
                                Me.DataTableForSave = l_datatable_SaveForAdd

                            End If
                        End If


                    End If

                    'ElseIf Viewstate("IsAddorEdit") = "Edit" Then
                Else
                    If Not Me.DataTableForSave Is Nothing Then
                        l_datarow_SaveForEdit = Me.DataTableForSave.Rows(0)

                        l_datarow_SaveForEdit("PayRollDate") = Me.TextBoxPayRollDate.Text.Trim
                        If Me.TextBoxPercentage.Text.Trim.Length = 0 Then
                            l_datarow_SaveForEdit("Percentage") = 0.0
                        Else
                            l_datarow_SaveForEdit("Percentage") = Me.TextBoxPercentage.Text.Trim
                        End If
                        l_datarow_SaveForEdit("Status") = Me.DropdownlistStatus.SelectedValue
                        'If Me.TextboxCompletedOn.Text.Trim.Length = 0 Then
                        '    l_datarow_SaveForEdit("CompletedOn") = "01/01/1900"
                        'Else
                        '    l_datarow_SaveForEdit("CompletedOn") = Me.TextboxCompletedOn.Text.Trim
                        'End If
                        'l_datarow_SaveForEdit("CompletedBy") = Me.TextboxCompletedBy.Text.Trim

                    End If
                    Me.ButtonEdit.Enabled = False
                End If
            End If

            Me.ButtonSave.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True
            Me.ButtonDelete.Enabled = True
            Me.ButtonCancel.Enabled = False
            Me.LabelNoRecordFound.Visible = False
            Me.MakeTextBoxesReadOnly()
            'Fire the save code
            Me.SaveToDataBase()

        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            'to clear the text boxes
            Me.TextBoxPayRollDate.Text = ""
            Me.DropdownlistStatus.SelectedValue = "P"
            Me.TextBoxPercentage.Text = ""
            Me.TextboxCompletedOn.Text = ""
            Me.TextboxCompletedBy.Text = ""

            'to remove read only of text boxes
            Me.TextBoxPayRollDate.Enabled = True
            Me.DropdownlistStatus.Enabled = False
            Me.TextBoxPercentage.ReadOnly = False
            Me.TextboxCompletedOn.ReadOnly = True
            Me.TextboxCompletedBy.ReadOnly = True

            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.ButtonEdit.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.RequiredFieldValidatorForNumPercentage.Enabled = True
            'Me.RangeValidatorNumPercentage.Enabled = True
            Me.TextBoxPayRollDate.RequiredDate = True
            Me.DataGridSpecialDividendData.SelectedIndex = -1
            ' a flag to distinguish between add/edit
            Viewstate("IsAddorEdit") = "Add"


        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridSpecialDividendData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridSpecialDividendData.SelectedIndexChanged
        Dim l_datatable_SpecialDividendData As DataTable
        Dim l_datarow_SpecialDividendData As DataRow
        Try
            Me.SetSelectedImageOnDataGrid()
            'Store the selected index of DataGrid
            Session("SelectedIndex") = Me.DataGridSpecialDividendData.SelectedIndex
            Session("UniqueIdOfSelectedItem") = Me.DataGridSpecialDividendData.SelectedItem.Cells(1).Text
            'Enable Cancel Button

            Me.ButtonCancel.Enabled = True
            Me.TextBoxPercentage.ReadOnly = False
            Me.TextBoxPayRollDate.Enabled = True
            Me.EnableDisableSaveButton()
            Me.ButtonDelete.Enabled = True
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.RequiredFieldValidatorForNumPercentage.Enabled = True
            'Me.RangeValidatorNumPercentage.Enabled = True
            Me.TextBoxPayRollDate.RequiredDate = True
            ViewState("IsAddorEdit") = "Edit"

            If Me.DataGridSpecialDividendData.SelectedItem.Cells(2).Text.Trim.ToUpper <> "&NBSP;" Then
                Me.TextBoxPayRollDate.Text = Me.DataGridSpecialDividendData.SelectedItem.Cells(2).Text.Trim
            Else
                Me.TextBoxPayRollDate.Text = ""
            End If
            If Me.DataGridSpecialDividendData.SelectedItem.Cells(3).Text.Trim.ToUpper <> "&NBSP;" Then
                Me.TextBoxPercentage.Text = Me.DataGridSpecialDividendData.SelectedItem.Cells(3).Text.Trim
            Else
                Me.TextBoxPercentage.Text = ""
            End If
            If Me.DataGridSpecialDividendData.SelectedItem.Cells(4).Text.Trim.ToUpper <> "&NBSP;" Then
                Me.DropdownlistStatus.SelectedValue = Me.DataGridSpecialDividendData.SelectedItem.Cells(4).Text.Trim
            Else
                Me.DropdownlistStatus.SelectedValue = ""
            End If
            If Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text.Trim.ToUpper <> "&NBSP;" Then
                Me.TextboxCompletedOn.Text = Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text.Trim
            Else
                Me.TextboxCompletedOn.Text = ""
            End If
            If Me.DataGridSpecialDividendData.SelectedItem.Cells(6).Text.Trim.ToUpper <> "&NBSP;" Then
                Me.TextboxCompletedBy.Text = Me.DataGridSpecialDividendData.SelectedItem.Cells(6).Text.Trim
            Else
                Me.TextboxCompletedBy.Text = ""
            End If
            If Not Me.DataTableForSave Is Nothing Then
                Me.DataTableForSave = Nothing
            End If

            l_datatable_SpecialDividendData = Me.DataSetSpecialDividendData.Tables("SpecialDividendData").Clone
            l_datarow_SpecialDividendData = l_datatable_SpecialDividendData.NewRow()
            l_datarow_SpecialDividendData("UniqueId") = Me.DataGridSpecialDividendData.SelectedItem.Cells(1).Text
            l_datarow_SpecialDividendData("PayRollDate") = Me.DataGridSpecialDividendData.SelectedItem.Cells(2).Text
            l_datarow_SpecialDividendData("Percentage") = Me.DataGridSpecialDividendData.SelectedItem.Cells(3).Text
            l_datarow_SpecialDividendData("Status") = Me.DataGridSpecialDividendData.SelectedItem.Cells(4).Text
            If Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text.Trim.ToUpper <> "&NBSP;" Then
                l_datarow_SpecialDividendData("CompletedOn") = Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text
            End If

            l_datarow_SpecialDividendData("CompletedBy") = Me.DataGridSpecialDividendData.SelectedItem.Cells(6).Text

            l_datatable_SpecialDividendData.Rows.Add(l_datarow_SpecialDividendData)
            l_datatable_SpecialDividendData.AcceptChanges()
            'this data table will be used for saving after modifications on edit and add button click 

            Me.DataTableForSave = l_datatable_SpecialDividendData
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub


    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Viewstate("IsAddorEdit") = Nothing
            Session("SpecialDividendData_Sort") = Nothing
            Session("SelectedIndex") = Nothing
            Session("UniqueIdOfSelectedItem") = Nothing
            Session("IsDateValid") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Function MakeTextBoxesReadOnly()
        Try
            Me.TextBoxPayRollDate.Enabled = False

            Me.DropdownlistStatus.Enabled = False
            Me.TextBoxPercentage.ReadOnly = True
            Me.TextboxCompletedOn.ReadOnly = True
            Me.TextboxCompletedBy.ReadOnly = True

        Catch
            Throw
        End Try
    End Function
    Private Function EnableDisableSaveButton()
        Try
            'If Me.DataGridSpecialDividendData.SelectedItem.Cells(4).Text.Trim.ToUpper = "C" And Me.DataGridSpecialDividendData.SelectedItem.Cells(5).Text.Trim.ToUpper <> "&NBSP;" And Me.DataGridSpecialDividendData.SelectedItem.Cells(6).Text.Trim.ToUpper <> "&NBSP;" Then
            If Me.DataGridSpecialDividendData.Items.Count > 0 Then
                If Me.DataGridSpecialDividendData.SelectedItem.Cells(4).Text.Trim.ToUpper = "C" Then
                    'Me.ButtonEdit.Enabled = False
                    Me.ButtonSave.Enabled = False
                Else
                    'Me.ButtonEdit.Enabled = True
                    Me.ButtonSave.Enabled = True
                End If
            End If
        Catch
            Throw
        End Try
    End Function
    Private Function SetSelectedImageOnDataGrid()
        Try
            Dim i As Integer
            For i = 0 To Me.DataGridSpecialDividendData.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridSpecialDividendData.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridSpecialDividendData.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If

            Next
        Catch

        End Try

    End Function

    Private Sub DataGridSpecialDividendData_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridSpecialDividendData.SortCommand
        Try
            Dim l_ds_Sort_SpecialDividendData As DataSet

            Me.DataGridSpecialDividendData.SelectedIndex = -1
            If Not Me.DataSetSpecialDividendData Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_Sort_SpecialDividendData = Me.DataSetSpecialDividendData
                dv = l_ds_Sort_SpecialDividendData.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("SpecialDividendData_Sort") Is Nothing Then
                    If Session("SpecialDividendData_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridSpecialDividendData.DataSource = Nothing
                Me.DataGridSpecialDividendData.DataSource = dv
                Me.DataGridSpecialDividendData.DataBind()
                Session("SpecialDividendData_Sort") = dv.Sort
                Me.DataGridSpecialDividendData.SelectedIndex = Session("SelectedIndex")
                Me.LoadExpDividendIntoControls(Me.DataSetSpecialDividendData, Session("UniqueIdOfSelectedItem"))
                Me.SetSelectedImageOnDataGridOnSorting()
                Me.SetSelectedImageOnDataGrid()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Function SetSelectedImageOnDataGridOnSorting()
        Try
            Dim dgi As DataGridItem
            Dim l_counter As Integer
            l_counter = -1
            For Each dgi In Me.DataGridSpecialDividendData.Items
                l_counter = l_counter + 1
                If dgi.Cells(1).Text = Session("UniqueIdOfSelectedItem") Then

                    Exit For
                End If

            Next
            Me.DataGridSpecialDividendData.SelectedIndex = l_counter
        Catch

        End Try
    End Function
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()

        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            DataGridSpecialDividendData.Enabled = False
            DataGridSpecialDividendData.ToolTip = tooltip
            ButtonDelete.Enabled = False
            ButtonDelete.ToolTip = tooltip
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 
End Class
