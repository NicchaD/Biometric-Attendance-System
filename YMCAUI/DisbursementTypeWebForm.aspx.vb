'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	DisbursementTypeWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 4:58:02 PM
' Program Specification Name	:	Doc 5.4.1.6
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	SrimuruganG 
' Changed on			:	25.July.2005
' Change Description	:	For Coding...

' Changed By            :   Dhananjay 
' Changed on            :   03/11/2005 
' Changed Description   :   Changed  As per issue no 199  
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session    
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Neeraj Singh                   06/jun/2010         Enhancement for .net 4.0
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja K                        2019.28.02          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
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
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class DisbursementTypeWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DisbursementTypeWebForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents LabelDisbursementType As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxEditable As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelActive As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEditable As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridDisbursement As System.Web.UI.WebControls.DataGrid
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents TextBoxSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMessage As System.Web.UI.WebControls.Label
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidatorDisType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents TextBoxDisbursementType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxShortDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelShortDescription As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDescription As System.Web.UI.WebControls.Label
    Protected WithEvents LabelGLAccountNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGLAccountNo As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum FormMode
        View
        Add
        Edit
        Save
        None
    End Enum
    '** This property is used to handle session state.

    Private Property SessionFormMode() As FormMode
        Get
            If Not (Session("FormMode")) Is Nothing Then
                Return (CType(Session("FormMode"), FormMode))
            Else
                Return FormMode.None
            End If
        End Get
        Set(ByVal Value As FormMode)
            Session("FormMode") = Value
        End Set
    End Property

    Private Property SessionRowIndex() As Integer
        Get
            If Not (Session("RowIndex")) Is Nothing Then
                Return (CType(Session("RowIndex"), Integer))
            Else
                Return -1
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RowIndex") = Value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
            If Page.IsPostBack() Then

                Select Case (SessionFormMode)
                    Case FormMode.View
                        ' Action for Add button Click
                        Me.EnableDisableControls(False)

                        'Case FormMode.Add
                        ' Action for Add button Click

                        'Me.EnableDisableControls(True)


                        'Case FormMode.Edit
                        ' Action for Edit button Click


                        'Case FormMode.Save
                        ' Action for Save button Click

                    Case Else
                        'Me.LoadDataGridFromCache()

                End Select



            Else
                Session("UniqueIdOfSelectedItem") = Nothing
                Me.LabelLook.AssociatedControlID = Me.TextBoxSearch.ID
                Me.LabelDisbursementType.AssociatedControlID = Me.TextBoxDisbursementType.ID
                Me.LabelShortDescription.AssociatedControlID = Me.TextBoxShortDescription.ID
                Me.LabelDescription.AssociatedControlID = Me.TextBoxDescription.ID
                Me.LabelGLAccountNo.AssociatedControlID = Me.TextBoxGLAccountNo.ID
                Me.LabelActive.AssociatedControlID = Me.CheckBoxActive.ID
                Me.LabelEditable.AssociatedControlID = Me.CheckBoxEditable.ID

                Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                Menu1.DataBind()

                Me.ClearControls()
                Me.EnableDisableControls(False)
                Me.LoadDataGrid()

                If (Me.SessionRowIndex <> -1) Then
                    'Me.PopulateDataIntoControls(Me.SessionRowIndex)
                    Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
                    ''Me.ButtonEdit.Enabled = True
                Else
                    ''Me.ButtonEdit.Enabled = False
                End If

            End If


            If Request.Form("Ok") = "OK" Then
                Me.ButtonCancel.Enabled = False
                Me.ButtonSave.Enabled = False
                ''Me.ButtonEdit.Enabled = False
                Me.ButtonAdd.Enabled = True
                Me.ButtonOK.Enabled = True

                Me.ButtonSearch.Enabled = True
                Me.TextBoxSearch.ReadOnly = False

                Me.EnableDisableControls(False)
                Me.LoadDataGridFromCache()
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub EnableDisableControls(ByVal Value As Boolean)
        Me.TextBoxDisbursementType.ReadOnly = Not Value
        Me.TextBoxShortDescription.ReadOnly = Not Value
        Me.TextBoxDescription.ReadOnly = Not Value
        Me.TextBoxGLAccountNo.ReadOnly = Not Value

        Me.CheckBoxActive.Enabled = Value
        Me.CheckBoxEditable.Enabled = Value

    End Sub

    Private Sub ClearControls()
        Me.TextBoxDisbursementType.Text = String.Empty
        Me.TextBoxShortDescription.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxGLAccountNo.Text = String.Empty

        Me.CheckBoxActive.Checked = False
        Me.CheckBoxEditable.Checked = False

        Me.CheckBoxActive.Enabled = False
        Me.CheckBoxEditable.Enabled = False
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            Me.LoadDataGridFromCache()
            Me.SessionFormMode = FormMode.Add

            Me.ClearControls()
            Me.EnableDisableControls(True)

            ''Me.ButtonEdit.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonSearch.Enabled = False
            Me.TextBoxSearch.ReadOnly = True

            Me.ButtonSave.Enabled = True

            Me.ButtonCancel.Enabled = True

            'Me.DataGridDisbursement.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function LoadDataGridFromCache()
        Dim l_DataSet As DataSet

        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager
        'l_Cache = CacheFactory.GetCacheManager

        Try

            'Vipul 04Feb06 Cache-Session
            'l_DataSet = CType(l_Cache.GetData("DisbursementTypes"), DataSet)
            l_DataSet = CType(Session("DisbursementTypes"), DataSet)
            'Vipul 04Feb06 Cache-Session

            '************************************
            'Code Added by Vartika on 08/12/2005
            ViewState("Dataset_Disbursement") = l_DataSet
            Me.DataGridDisbursement.DataSource = l_DataSet
            Me.DataGridDisbursement.DataBind()
            ''CommonModule.HideColumnsinDataGrid(l_DataSet, Me.DataGridDisbursement, "Description, GL Account No, Active, Editable,CreatedBy, CreatedOn")
            '************************************

        Catch
            Response.Redirect("ErrorPageForm.aspx")
        End Try

    End Function

    Private Function LoadDataGrid()
        Dim l_DataSet As DataSet
        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager

        Try
            '************************************
            'Code Added by Dhananjay on 03/11/2005
            Session("RowIndex") = 0
            '*************************************
            l_DataSet = YMCARET.YmcaBusinessObject.DisbursementTypeBOClass.LookupDisbursementTypes()

            If Not l_DataSet Is Nothing AndAlso l_DataSet.Tables.Count > 0 AndAlso l_DataSet.Tables(0).Rows.Count > 0 Then
                Session("UniqueIdOfSelectedItem") = Convert.ToString(l_DataSet.Tables(0).Rows(0)("Disbursement Type")).Trim()
            End If


            '************************************
            'Code Added by Vartika on 08/12/2005
            ViewState("Dataset_Disbursement") = l_DataSet
            Me.DataGridDisbursement.DataSource = l_DataSet
            Me.DataGridDisbursement.DataBind()
            ''CommonModule.HideColumnsinDataGrid(l_DataSet, Me.DataGridDisbursement, "Description, GL Account No, Active, Editable,CreatedBy, CreatedOn")
            '************************************


            '*******************************************************
            'Code Added by Dhananjay on 03/11/2005
            DataGridDisbursement.SelectedIndex = Session("Index")
            '*******************************************************
            ' Adding the Dataset into Cache

            'Vipul 04Feb06 Cache-Session
            'l_Cache = CacheFactory.GetCacheManager
            'l_Cache.Add("DisbursementTypes", l_DataSet)
            Session("DisbursementTypes") = l_DataSet
            'Vipul 04Feb06 Cache-Session

            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True

            ''Me.ButtonEdit.Enabled = False
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False

            ' Set the selecte index for keeping session RowIndex, if dataset is not empty.
            If Not l_DataSet Is Nothing Then
                If l_DataSet.Tables.Count > 0 Then
                    Me.SessionRowIndex = 0
                Else
                    Me.SessionRowIndex = -1
                End If

            Else
                Me.SessionRowIndex = -1
            End If

            'Me.DataGridDisbursement.AllowSorting = True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub DataGridDisbursement_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridDisbursement.SelectedIndexChanged
        Try
            Me.LoadDataGridFromCache()
            'If Me.SessionFormMode = FormMode.Add Or Me.SessionFormMode = FormMode.Edit Then Return

            Session("UniqueIdOfSelectedItem") = Me.DataGridDisbursement.SelectedItem.Cells(1).Text.Trim()

            If Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem")) = True Then
                'Me.ButtonEdit.Enabled = True
                'Else
                'Me.ButtonEdit.Enabled = True
            End If

            Me.SessionRowIndex = Me.DataGridDisbursement.SelectedIndex
            Me.SessionFormMode = FormMode.None
            'Code Added by Dhananjay on 03/11/2005 
            Session("Index") = Me.DataGridDisbursement.SelectedIndex

            '***********************************************************
            ''Code Added by vartika on 8th Dec 2005
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonSearch.Enabled = False
            Me.TextBoxSearch.ReadOnly = True

            Me.ButtonCancel.Enabled = True
            Me.ButtonSave.Enabled = True

            Me.SessionFormMode = FormMode.Edit

            Me.TextBoxDisbursementType.ReadOnly = True
            Me.TextBoxShortDescription.ReadOnly = False
            Me.TextBoxDescription.ReadOnly = False
            Me.TextBoxGLAccountNo.ReadOnly = False

            Me.CheckBoxEditable.Enabled = True
            Me.CheckBoxActive.Enabled = True

            ''Me.TextBoxDisbursementType.ReadOnly = True
            ''Me.TextBoxShortDescription.ReadOnly = True
            ''Me.TextBoxDescription.ReadOnly = True
            ''Me.TextBoxGLAccountNo.ReadOnly = True

            ''Me.CheckBoxEditable.Enabled = False
            ''Me.CheckBoxActive.Enabled = False

            '***********************************************************
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function PopulateDataIntoControls(ByVal parameterDisbursementType As String) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow()

        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager
        'l_Cache = CacheFactory.GetCacheManager()

        Try

            'Vipul 04Feb06 Cache-Session
            'l_DataSet = CType(l_Cache.GetData("DisbursementTypes"), DataSet)
            l_DataSet = CType(Session("DisbursementTypes"), DataSet)
            'Vipul 04Feb06 Cache-Session

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Disbursement Types")

                If Not l_DataTable Is Nothing Then

                    'If index < l_DataTable.Rows.Count Then

                    'l_DataRow = l_DataTable.Rows.Item(index)
                    If Session("UniqueIdOfSelectedItem") Is Nothing Then
                        parameterDisbursementType = l_DataTable.Rows(0).Item("Disbursement Type")
                        Me.DataGridDisbursement.SelectedIndex = 0

                    End If
                    l_DataRow = l_DataTable.Select("[Disbursement Type] = '" & parameterDisbursementType & "'")


                    If (Not l_DataRow Is Nothing AndAlso l_DataRow.Length > 0) Then

                        Me.TextBoxDisbursementType.Text = CType(l_DataRow(0)("Disbursement Type"), String).Trim
                        Me.TextBoxShortDescription.Text = CType(l_DataRow(0)("Short Description"), String).Trim
                        Me.TextBoxDescription.Text = CType(l_DataRow(0)("Description"), String).Trim
                        Me.TextBoxGLAccountNo.Text = CType(l_DataRow(0)("GL Account No"), String).Trim

                        Me.CheckBoxActive.Checked = CType(l_DataRow(0)("Active"), Boolean)
                        Me.CheckBoxEditable.Checked = CType(l_DataRow(0)("Editable"), Boolean)

                        Return True
                    Else
                        Return False
                    End If
                    'Else
                    '    Return False
                    'End If
                Else
                    Return False
                End If

            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try



    End Function


    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940


            'MessageBox.Show(Me.MessageBoxPlaceHolder, "Test", "Test", MessageBoxButtons.YesNo)
            Me.LoadDataGridFromCache()

            If Me.SessionFormMode = FormMode.Add Then
                Me.SaveDisbursementType()
            ElseIf Me.SessionFormMode = FormMode.Edit Then
                Me.UpdateDisbursementType()
            End If
            Me.DataGridDisbursement.SelectedIndex = -1
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60024 Then
                MessageBox.Show(MessageBoxPlaceHolder, "Please Confirm", "Duplicate Record", MessageBoxButtons.OK)
                If Request.Form("OK") = "OK" Then
                    Exit Sub
                End If
            Else
                Throw sqlEx
            End If
        End Try
    End Sub

    Private Function UpdateDisbursementType() As Boolean
        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager
        'l_Cache = CacheFactory.GetCacheManager()

        Try

            'Vipul 04Feb06 Cache-Session
            'l_DataSet = CType(l_Cache.GetData("DisbursementTypes"), DataSet)
            l_DataSet = CType(Session("DisbursementTypes"), DataSet)
            'Vipul 04Feb06 Cache-Session

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Disbursement Types")

                If Not l_DataTable Is Nothing Then

                    If Me.SessionRowIndex <> -1 Then

                        l_DataRow = l_DataTable.Rows.Item(Me.SessionRowIndex)

                        If Not l_DataRow Is Nothing Then

                            l_DataRow("Disbursement Type") = Me.TextBoxDisbursementType.Text.Trim.ToUpper
                            l_DataRow("Short Description") = Me.TextBoxShortDescription.Text.Trim
                            l_DataRow("Description") = Me.TextBoxDescription.Text.Trim
                            l_DataRow("GL Account No") = Me.TextBoxGLAccountNo.Text.Trim
                            l_DataRow("Active") = Me.CheckBoxActive.Checked
                            l_DataRow("Editable") = Me.CheckBoxEditable.Checked
                            l_DataRow("CreatedOn") = Now
                            l_DataRow("CreatedBy") = "1"

                            'Update the DataSet into DataBase
                            YMCARET.YmcaBusinessObject.DisbursementTypeBOClass.UpdateDataSet(l_DataSet)

                            Me.ClearControls()
                            Me.EnableDisableControls(False)
                            Me.LoadDataGrid()

                            If (Me.SessionRowIndex <> -1) Then
                                'Me.PopulateDataIntoControls(Session("Index"))
                                Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
                                ''Me.ButtonEdit.Enabled = True
                            Else
                                ''Me.ButtonEdit.Enabled = False
                            End If

                            'Me.DataGridDisbursement.Enabled = True
                            Me.ButtonSearch.Enabled = True
                            Me.TextBoxSearch.ReadOnly = False
                            Me.SessionFormMode = FormMode.None

                        End If

                    End If

                End If

            End If


        Catch
            Throw
        End Try

    End Function

    Private Function SaveDisbursementType() As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager
        'l_Cache = CacheFactory.GetCacheManager()

        Try

            'Vipul 04Feb06 Cache-Session
            'l_DataSet = CType(l_Cache.GetData("DisbursementTypes"), DataSet)
            l_DataSet = CType(Session("DisbursementTypes"), DataSet)
            'Vipul 04Feb06 Cache-Session

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Disbursement Types")

                If Not l_DataTable Is Nothing Then

                    l_DataRow = l_DataTable.NewRow

                    l_DataRow("Disbursement Type") = Me.TextBoxDisbursementType.Text.Trim.ToUpper
                    l_DataRow("Short Description") = Me.TextBoxShortDescription.Text.Trim
                    l_DataRow("Description") = Me.TextBoxDescription.Text.Trim
                    l_DataRow("GL Account No") = Me.TextBoxGLAccountNo.Text.Trim
                    l_DataRow("Active") = Me.CheckBoxActive.Checked
                    l_DataRow("Editable") = Me.CheckBoxEditable.Checked
                    l_DataRow("CreatedOn") = Now
                    l_DataRow("CreatedBy") = "1"

                    l_DataTable.Rows.Add(l_DataRow)

                    'Update the DataSet into DataBase
                    YMCARET.YmcaBusinessObject.DisbursementTypeBOClass.UpdateDataSet(l_DataSet)

                    Me.ClearControls()
                    Me.EnableDisableControls(False)
                    Me.LoadDataGrid()

                    If (Me.SessionRowIndex <> -1) Then
                        'Me.PopulateDataIntoControls(Me.SessionRowIndex) 'Changed bu Dhananjay on 03/11/2005
                        Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
                        ''Me.ButtonEdit.Enabled = True
                    Else
                        ''Me.ButtonEdit.Enabled = False
                    End If

                    'Me.DataGridDisbursement.Enabled = True
                    Me.ButtonSearch.Enabled = True
                    Me.TextBoxSearch.ReadOnly = False
                    Me.SessionFormMode = FormMode.None

                End If

            End If


        Catch ex As Exception
            Throw ex
        End Try


    End Function

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    Me.LoadDataGridFromCache()
    ''    Me.ButtonEdit.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonOK.Enabled = False
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.TextBoxSearch.ReadOnly = True

    ''    Me.ButtonCancel.Enabled = True
    ''    Me.ButtonSave.Enabled = True

    ''    Me.SessionFormMode = FormMode.Edit

    ''    Me.TextBoxDisbursementType.ReadOnly = True
    ''    Me.TextBoxShortDescription.ReadOnly = False
    ''    Me.TextBoxDescription.ReadOnly = False
    ''    Me.TextBoxGLAccountNo.ReadOnly = False

    ''    Me.CheckBoxEditable.Enabled = True
    ''    Me.CheckBoxActive.Enabled = True

    ''    'Me.DataGridDisbursement.Enabled = False

    ''End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Me.LoadDataGridFromCache()
            'Me.DataGridDisbursement.Enabled = True
            Me.ButtonSearch.Enabled = True
            Me.TextBoxSearch.ReadOnly = False
            Session("UniqueIdOfSelectedItem") = Nothing
            If SessionFormMode = FormMode.Edit Or SessionFormMode = FormMode.Add Then
                ' Cancel the edit mode.

                If Me.SessionRowIndex <> -1 Then
                    'Me.PopulateDataIntoControls(Me.SessionRowIndex)
                    Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
                    ''Me.ButtonEdit.Enabled = True
                Else
                    Me.ClearControls()
                    ''Me.ButtonEdit.Enabled = False
                End If

                Me.SessionFormMode = FormMode.None

                Me.ButtonSave.Enabled = False
                Me.ButtonCancel.Enabled = False

                Me.ButtonAdd.Enabled = True
                Me.ButtonOK.Enabled = True
                'Me.DataGridDisbursement.SelectedIndex = -1
                Me.EnableDisableControls(False)

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        '*************************************************************************
        'Changed by Dhananjay on 3/11/2005 As per issue no 199
        'Added ButtonSearch.CauseValidation = False
        '*************************************************************************
        ButtonSearch.CausesValidation = False
        'End of the change 
        Try
            Me.LoadDataGridFromCache()
            If Me.TextBoxSearch.Text.Trim.Length > 0 Then

                SearchAndLoadDataGrid(Me.TextBoxSearch.Text.Trim)

                If Me.SessionRowIndex > -1 Then
                    Me.ClearControls()

                    If Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem")) = True Then
                        ''Me.ButtonEdit.Enabled = True
                    Else
                        ''Me.ButtonEdit.Enabled = False
                    End If

                Else
                    Me.ClearControls()

                    Me.EnableDisableControls(False)

                    Me.ButtonSave.Enabled = False
                    Me.ButtonCancel.Enabled = False
                    ''Me.ButtonEdit.Enabled = False

                    Me.ButtonAdd.Enabled = True
                    Me.ButtonOK.Enabled = True

                End If
            Else

                Me.ClearControls()
                Me.EnableDisableControls(False)
                Me.LoadDataGrid()

                If (Me.SessionRowIndex <> -1) Then
                    'Me.PopulateDataIntoControls(Me.SessionRowIndex)
                    Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
                    ''Me.ButtonEdit.Enabled = True
                Else
                    ''Me.ButtonEdit.Enabled = False
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Private Function SearchAndLoadDataGrid(ByVal searchString As String)

        Dim l_DataSet As DataSet
        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager

        Try

            l_DataSet = YMCARET.YmcaBusinessObject.DisbursementTypeBOClass.LookupDisbursementTypes(searchString)


            '************************************
            'Code Added by Vartika on 08/12/2005
            ViewState("Dataset_Disbursement") = l_DataSet
            Me.DataGridDisbursement.DataSource = l_DataSet
            Me.DataGridDisbursement.DataBind()
            ''CommonModule.HideColumnsinDataGrid(l_DataSet, Me.DataGridDisbursement, "Description, GL Account No, Active, Editable,CreatedBy, CreatedOn")
            '************************************

            ' Adding the Dataset into Cache

            'Vipul 04Feb06 Cache-Session
            'l_Cache = CacheFactory.GetCacheManager
            'l_Cache.Add("DisbursementTypes", l_DataSet)
            Session("DisbursementTypes") = l_DataSet
            'Vipul 04Feb06 Cache-Session

            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True

            ''Me.ButtonEdit.Enabled = False
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False

            If Not l_DataSet Is Nothing AndAlso l_DataSet.Tables.Count > 0 AndAlso l_DataSet.Tables(0).Rows.Count > 0 Then
                Session("UniqueIdOfSelectedItem") = Convert.ToString(l_DataSet.Tables(0).Rows(0)("Disbursement Type")).Trim()
            End If

            ' Set the selecte index for keeping session RowIndex, if dataset is not empty.
            'If Not l_DataSet Is Nothing Then

            '    If l_DataSet.Tables.Count > 0 Then
            '        Me.SessionRowIndex = 0
            '    Else
            '        Me.SessionRowIndex = -1
            '    End If

            'Else
            '    Me.SessionRowIndex = -1
            'End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Function

    Sub DataGrid_Sort(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)


        Dim l_DataSet As DataSet
        'Vipul 04Feb06 Cache-Session
        'Dim l_Cache As CacheManager
        'l_Cache = CacheFactory.GetCacheManager

        Try

            'Vipul 04Feb06 Cache-Session
            'l_DataSet = CType(l_Cache.GetData("DisbursementTypes"), DataSet)
            l_DataSet = CType(Session("DisbursementTypes"), DataSet)
            'Vipul 04Feb06 Cache-Session

            l_DataSet.Tables.Item(0).DefaultView.Sort = "Disbursement Type DESC"

            Me.DataGridDisbursement.DataSource = l_DataSet.Tables.Item(0).DefaultView
            Me.DataGridDisbursement.DataBind()

        Catch

        End Try

    End Sub


    Private Sub DataGridDisbursement_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridDisbursement.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridDisbursement.SelectedIndex And Me.DataGridDisbursement.SelectedIndex >= 0) Then
            Me.DataGridDisbursement.SelectedIndex = e.Item.ItemIndex
            l_button_Select.ImageUrl = "images\selected.gif"
            Session("Index") = Me.DataGridDisbursement.SelectedIndex

        End If
        e.Item.Cells(2).Visible = False
        e.Item.Cells(4).Visible = False
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
        e.Item.Cells(7).Visible = False
        e.Item.Cells(8).Visible = False

    End Sub

    Private Sub DataGridDisbursement_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridDisbursement.SortCommand
        Try
            Dim l_DataSet As DataSet
            Me.DataGridDisbursement.SelectedIndex = -1
            If Not ViewState("Dataset_Disbursement") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_DataSet = ViewState("Dataset_Disbursement")
                dv = l_DataSet.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("DisbursementSort") Is Nothing Then
                    If Session("DisbursementSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridDisbursement.DataSource = Nothing
                Me.DataGridDisbursement.DataSource = dv
                Me.DataGridDisbursement.DataBind()
                Session("DisbursementSort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Session("DisbursementSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            DataGridDisbursement.Enabled = False
            DataGridDisbursement.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
