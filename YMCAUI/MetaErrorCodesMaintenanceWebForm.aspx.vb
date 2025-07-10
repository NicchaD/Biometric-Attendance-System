'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaErrorCodesMaintenanceWebForm.aspx.vb
' Author Name		:	Sameer Joshi
' Employee ID		:	33156
' Email				:	sameer.joshi23i-infotech.com
' Contact No		:	55928743
' Creation Time		:	5/17/2005 3:09:54 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	Meta Error Codes Maintenance .
'
' Changed by			:	Shefali Bharti  
' Changed on			:	28-07-2005
' Change Description	:	Coding
' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'****************************************************
'Modification History
'*******************************************************************************
'Modified by            Date                Description
'*******************************************************************************
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Sanjay R.              2010.06.29          Enhancement changes(CType to DirectCast)
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
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

Public Class MetaErrorCodesMaintenanceWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MetaErrorCodesMaintenanceWebForm.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsErrorCodes As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Mandatory As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Menu1 As skmMenu.Menu
    Dim g_bool_DeleteFlag As New Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Shared Sub InitializeComponent()

    End Sub


    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents LabelDescription As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCategory As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCategory As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelErrorLevel As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxErrorLevel As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridMetaErrorCodesMaintenance As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents LabelErrorCode As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxErrorCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here dtgMetaErrorCodesMaintenance


        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
        ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
        If Not Me.IsPostBack Then
            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag
            Me.LabelNoRecordFound.Visible = False
            PopulateData()
            ' populating the textboxes with the current row
            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If
            'if Search flag is true then call 'SearchPopulate' method else call 'populate' method

        Else
            g_bool_SearchFlag = Session("BoolSearchFlag")
            g_bool_EditFlag = Session("BoolEditFlag")
            g_bool_DeleteFlag = Session("BoolDeleteFlag")

            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            Me.LabelNoRecordFound.Visible = False

        End If
        If Request.Form("Yes") = "Yes" Then
            DeleteSub()
            g_integer_count = 0
            Me.PopulateDataIntoControls(g_integer_count)
        End If

        If Request.Form("No") = "No" Then
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
        End If

        If Request.Form("Ok") = "OK" Then
            Me.ButtonCancel.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonSave.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOk.Enabled = True

            Me.ButtonSearch.Enabled = True
            Me.TextBoxFind.ReadOnly = False

            Me.TextBoxErrorCode.ReadOnly = True
            Me.TextBoxDescription.ReadOnly = True
            Me.TextBoxCategory.ReadOnly = True
            Me.TextBoxErrorLevel.ReadOnly = True

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

        End If
        
    End Sub
    Public Sub DeleteSub()
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If

        g_bool_DeleteFlag = True
        Session("BoolDeleteFlag") = g_bool_DeleteFlag

        Dim l_DataRow As DataRow
        If Not g_dataset_dsErrorCodes Is Nothing Then

            g_integer_count = Session("dataset_index")
            l_DataRow = g_dataset_dsErrorCodes.Tables(0).Rows(g_integer_count)
            l_DataRow.Delete()
        End If
        YMCARET.YmcaBusinessObject.MetaErrorCodesMaintenanceBOClass.InsertErrorCodes(g_dataset_dsErrorCodes)
        PopulateData()

        ' Making TextBoxes Blank
        Me.TextBoxCategory.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxErrorCode.Text = String.Empty
        Me.TextBoxErrorLevel.Text = String.Empty

        Me.ButtonDelete.Enabled = False
        ''Me.ButtonEdit.Enabled = False
    End Sub
    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsErrorCodes

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Error Codes")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxErrorCode.Text = DirectCast(l_DataRow("Error Code"), String).Trim 'Directcast(for Ctype) implemented by SR:2010.06.30 for migration
                            If l_DataRow("Description").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDescription.Text = String.Empty
                            Else
                                Me.TextBoxDescription.Text = DirectCast(l_DataRow("Description"), String).Trim 'Directcast(for Ctype) implemented by SR:2010.06.30 for migration
                            End If

                            If l_DataRow("Category").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxCategory.Text = String.Empty
                            Else
                                Me.TextBoxCategory.Text = DirectCast(l_DataRow("Category"), String).Trim 'Directcast(for Ctype) implemented by SR:2010.06.30 for migration
                            End If

                            If l_DataRow("Error Level").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxErrorLevel.Text = 0
                            Else
                                Me.TextBoxErrorLevel.Text = DirectCast(l_DataRow("Error Level"), String).Trim  'Directcast(for Ctype) implemented by SR:2010.06.30 for migration
                            End If



                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Else
                Return False
            End If


        Catch
            Throw
        End Try
    End Function

    Public Sub PopulateData()
        'Dim Cache As CacheManager
        Try
            g_dataset_dsErrorCodes = YMCARET.YmcaBusinessObject.MetaErrorCodesMaintenanceBOClass.LookupErrorCodes()
            viewstate("Dataset_ErrorCodes") = g_dataset_dsErrorCodes
            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("Error Codes", g_dataset_dsErrorCodes)
            Session("g_dataset_dsErrorCodes") = g_dataset_dsErrorCodes
            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

            If g_dataset_dsErrorCodes.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridMetaErrorCodesMaintenance.DataSource = g_dataset_dsErrorCodes.Tables(0)
                Me.DataGridMetaErrorCodesMaintenance.DataBind()

            End If
            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsErrorCodes, Me.DataGridMetaErrorCodesMaintenance, "Category, Error Level")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsErrorCodes = Cache.GetData("Error Codes")
                g_dataset_dsErrorCodes = DirectCast(Session("g_dataset_dsErrorCodes"), DataSet) 'Directcast(for Ctype) implemented by SR:2010.06.30 for migration
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

                g_dataset_dsErrorCodes.Tables(0).Clear()
                Me.DataGridMetaErrorCodesMaintenance.DataSource = g_dataset_dsErrorCodes
                Me.DataGridMetaErrorCodesMaintenance.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If


        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try

    End Sub

    Public Sub SearchAndPopulateData()

        'Dim Cache As CacheManager
        Try
            'Cache = CacheFactory.GetCacheManager()
            g_dataset_dsErrorCodes = YMCARET.YmcaBusinessObject.MetaErrorCodesMaintenanceBOClass.SearchErrorCodes(Me.TextBoxFind.Text)
            viewstate("Dataset_ErrorCodes") = g_dataset_dsErrorCodes
            'Cache.Add("Error Codes", g_dataset_dsErrorCodes)

            If g_dataset_dsErrorCodes.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridMetaErrorCodesMaintenance.CurrentPageIndex = 0
                Me.DataGridMetaErrorCodesMaintenance.DataSource = g_dataset_dsErrorCodes
                Me.DataGridMetaErrorCodesMaintenance.DataBind()

            End If
            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsErrorCodes, Me.DataGridMetaErrorCodesMaintenance, "Category, Error Level")

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                PopulateData()
                Me.LabelNoRecordFound.Visible = True

                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsErrorCodes = Cache.GetData("Error Codes")
                g_dataset_dsErrorCodes = DirectCast(Session("g_dataset_dsErrorCodes"), DataSet) 'Directcast(for Ctype) implemented by SR:2010.06.30 for migration
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

                g_dataset_dsErrorCodes.Tables(0).Clear()
                Me.DataGridMetaErrorCodesMaintenance.DataSource = g_dataset_dsErrorCodes
                Me.DataGridMetaErrorCodesMaintenance.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try



    End Sub

    Private Sub DataGridMetaErrorCodesMaintenance_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaErrorCodesMaintenance.SelectedIndexChanged
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If

        ''Me.ButtonEdit.Enabled = True
        g_integer_count = DataGridMetaErrorCodesMaintenance.SelectedIndex '(((DataGridMetaErrorCodesMaintenance.CurrentPageIndex) * DataGridMetaErrorCodesMaintenance.PageSize) + DataGridMetaErrorCodesMaintenance.SelectedIndex)

        Session("dataset_index") = g_integer_count

        If Me.PopulateDataIntoControls(g_integer_count) = True Then
            ''Me.ButtonEdit.Enabled = True
            Me.ButtonDelete.Enabled = True
        Else
            ''Me.ButtonEdit.Enabled = True
            Me.ButtonDelete.Enabled = True
        End If
        Me.TextBoxErrorCode.ReadOnly = True
        Me.TextBoxDescription.ReadOnly = False
        Me.TextBoxCategory.ReadOnly = False
        Me.TextBoxErrorLevel.ReadOnly = False

        Me.ButtonSave.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.TextBoxFind.ReadOnly = True
        Me.ButtonSearch.Enabled = False
        Me.ButtonAdd.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.ButtonOk.Enabled = False

        g_bool_AddFlag = False
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_EditFlag = True
        Session("BoolEditFlag") = g_bool_EditFlag

        g_bool_DeleteFlag = False
        Session("BoolDeleteFlag") = g_bool_DeleteFlag
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If

        g_bool_SearchFlag = True
        Session("BoolSearchFlag") = g_bool_SearchFlag

        ' Clear the controls.
        ' Me.TextBoxFind.Text = ""
        Me.TextBoxCategory.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxErrorCode.Text = String.Empty
        Me.TextBoxErrorLevel.Text = String.Empty
        'g_integer_count = Session("dataset_index")
        'If (g_integer_count <> -1) Then
        '    Me.PopulateDataIntoControls(g_integer_count)
        'End If

        ' Do search & populate the data.
        SearchAndPopulateData()

    End Sub

    Private Sub DataGridMetaErrorCodesMaintenance_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaErrorCodesMaintenance.PageIndexChanged
        DataGridMetaErrorCodesMaintenance.CurrentPageIndex = e.NewPageIndex

        'Bind the DataGrid again with the Data Source
        'depending on wheather Search Flag is true or False

        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If

        g_bool_AddFlag = True
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_SearchFlag = False
        Session("BoolSearchFlag") = g_bool_SearchFlag

        'Making Readonly False for all TextBoxes
        Me.TextBoxErrorCode.ReadOnly = False
        Me.TextBoxDescription.ReadOnly = False
        Me.TextBoxCategory.ReadOnly = False
        Me.TextBoxErrorLevel.ReadOnly = False

        Me.ButtonSave.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.TextBoxFind.ReadOnly = True
        Me.ButtonSearch.Enabled = False
        ''Me.ButtonEdit.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.ButtonOk.Enabled = False

        ' Making TextBoxes Blank
        Me.TextBoxCategory.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxErrorCode.Text = String.Empty
        Me.TextBoxErrorLevel.Text = String.Empty
    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    ' Enable / Disable the controls.

    ''    Me.TextBoxErrorCode.ReadOnly = True
    ''    Me.TextBoxDescription.ReadOnly = False
    ''    Me.TextBoxCategory.ReadOnly = False
    ''    Me.TextBoxErrorLevel.ReadOnly = False

    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonDelete.Enabled = False
    ''    Me.ButtonOk.Enabled = False

    ''    g_bool_AddFlag = False
    ''    Session("BoolAddFlag") = g_bool_AddFlag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''    g_bool_DeleteFlag = False
    ''    Session("BoolDeleteFlag") = g_bool_DeleteFlag

    ''End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim InsertRow As DataRow
        Dim l_DataRow As DataRow

        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            'If add Flag Is true then do Insertion else update
            g_bool_AddFlag = Session("BoolAddFlag")

            If g_bool_AddFlag = True Then
                If Not g_dataset_dsErrorCodes Is Nothing Then

                    InsertRow = g_dataset_dsErrorCodes.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Error Code") = TextBoxErrorCode.Text.Trim
                    If Me.TextBoxDescription.Text.Trim.Length = 0 Then
                        InsertRow.Item("Description") = ""
                    Else
                        InsertRow.Item("Description") = TextBoxDescription.Text.Trim
                    End If

                    If Me.TextBoxCategory.Text.Trim.Length = 0 Then
                        InsertRow.Item("Category") = ""
                    Else
                        InsertRow.Item("Category") = TextBoxCategory.Text.Trim
                    End If

                    If Me.TextBoxErrorLevel.Text.Trim.Length = 0 Then
                        InsertRow.Item("Error Level") = ""
                    Else
                        InsertRow.Item("Error Level") = TextBoxErrorLevel.Text.Trim
                    End If


                End If

                ' Insert the row into Table.
                g_dataset_dsErrorCodes.Tables(0).Rows.Add(InsertRow)
                g_integer_count = 0
                Me.PopulateDataIntoControls(g_integer_count)
            Else

                If Not g_dataset_dsErrorCodes Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsErrorCodes.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        If Me.TextBoxDescription.Text.Trim.Length = 0 Then
                            l_DataRow("Description") = ""
                        Else
                            l_DataRow("Description") = TextBoxDescription.Text.Trim
                        End If

                        If Me.TextBoxCategory.Text.Trim.Length = 0 Then
                            l_DataRow("Category") = ""
                        Else
                            l_DataRow("Category") = TextBoxCategory.Text.Trim
                        End If

                        If Me.TextBoxErrorLevel.Text.Trim.Length = 0 Then
                            l_DataRow("Error Level") = ""
                        Else
                            l_DataRow("Error Level") = TextBoxErrorLevel.Text.Trim
                        End If

                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If

                    End If

                End If

            End If



            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.MetaErrorCodesMaintenanceBOClass.InsertErrorCodes(g_dataset_dsErrorCodes)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = True

            Me.TextBoxErrorCode.ReadOnly = True
            Me.TextBoxDescription.ReadOnly = True
            Me.TextBoxCategory.ReadOnly = True
            Me.TextBoxErrorLevel.ReadOnly = True
            Me.DataGridMetaErrorCodesMaintenance.SelectedIndex = -1


            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag


        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60010 Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Duplicate Record", MessageBoxButtons.OK)
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        PopulateData()

        'Enable / Disable the controls
        Me.ButtonSave.Enabled = False
        Me.ButtonCancel.Enabled = False
        Me.ButtonAdd.Enabled = True
        ''Me.ButtonEdit.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.TextBoxFind.ReadOnly = False
        Me.ButtonSearch.Enabled = True
        Me.ButtonOk.Enabled = True

        Me.TextBoxErrorCode.ReadOnly = True
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxCategory.ReadOnly = True
        Me.TextBoxErrorLevel.ReadOnly = True
        Me.DataGridMetaErrorCodesMaintenance.SelectedIndex = -1
        g_integer_count = Session("dataset_index")
        If (g_integer_count <> -1) Then
            Me.PopulateDataIntoControls(g_integer_count)
        End If

        g_bool_AddFlag = False
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_SearchFlag = False
        Session("BoolSearchFlag") = g_bool_SearchFlag

        g_bool_DeleteFlag = False
        Session("BoolDeleteFlag") = g_bool_DeleteFlag

        g_bool_EditFlag = False
        Session("BoolEditFlag") = g_bool_EditFlag

    End Sub



    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If

        MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you SURE you want to delete this Error Codes?", MessageBoxButtons.YesNo)

    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Session("ErrorCodeSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub DataGridMetaErrorCodesMaintenance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaErrorCodesMaintenance.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridMetaErrorCodesMaintenance.SelectedIndex And Me.DataGridMetaErrorCodesMaintenance.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(3).Visible = False
        e.Item.Cells(4).Visible = False
    End Sub

    Private Sub DataGridMetaErrorCodesMaintenance_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaErrorCodesMaintenance.SortCommand
        Try
            Me.DataGridMetaErrorCodesMaintenance.SelectedIndex = -1
            If Not viewstate("Dataset_ErrorCodes") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsErrorCodes = viewstate("Dataset_ErrorCodes")
                dv = g_dataset_dsErrorCodes.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("ErrorCodeSort") Is Nothing Then
                    If Session("ErrorCodeSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridMetaErrorCodesMaintenance.DataSource = Nothing
                Me.DataGridMetaErrorCodesMaintenance.DataSource = dv
                Me.DataGridMetaErrorCodesMaintenance.DataBind()
                Session("ErrorCodeSort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)


        End Try
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            DataGridMetaErrorCodesMaintenance.Enabled = False
            DataGridMetaErrorCodesMaintenance.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
