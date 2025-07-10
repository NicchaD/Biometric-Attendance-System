'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	InterestRatesWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 5:11:11 PM
' Program Specification Name	:	Doc 5.4.1.12
' Unit Test Plan Name			:	
' Description					:	
'
' Changed by			:	Shefali Bharti
' Changed on			:	29-07-2005
' Change Description	:	Coding
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session        
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
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


Public Class InterestRatesWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("InterestRatesWebForm.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsInterestRate As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDescription As System.Web.UI.WebControls.Label
    Protected WithEvents LabelShortDescription As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxShortDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAccountType As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Dim g_bool_DeleteFlag As New Boolean
    '''''
    Dim _fromLoad As Boolean


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Shared Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYear As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxInterestRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridInterestRates As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelAcctType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYear As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInterestRate As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
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
        'Me.DataGridInterestRates.DataSource = CommonModule.CreateDataSource
        'Me.DataGridInterestRates.DataBind()
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
        Me.LabelAcctType.AssociatedControlID = Me.TextBoxAccountType.ID
        Me.LabelShortDescription.AssociatedControlID = Me.TextBoxShortDescription.ID
        Me.LabelDescription.AssociatedControlID = Me.TextBoxDescription.ID
        Me.LabelInterestRate.AssociatedControlID = Me.TextBoxInterestRate.ID
        Me.LabelYear.AssociatedControlID = Me.TextBoxYear.ID

        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
        Me.Page.EnableViewState = False
        CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
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
            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If
        Else
            g_bool_SearchFlag = Session("BoolSearchFlag")
            g_bool_EditFlag = Session("BoolEditFlag")
            g_bool_DeleteFlag = Session("BoolDeleteFlag")

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            Me.LabelNoRecordFound.Visible = False
        End If

        If Request.Form("Yes") = "Yes" Then
            deleteSub()
            g_integer_count = 0
            Me.PopulateDataIntoControls(g_integer_count)
        End If

        'If Request.Form("No") = "No" Then
        '    If g_bool_SearchFlag = True Then
        '        SearchAndPopulateData()
        '    Else
        '        PopulateData()
        '    End If
        'End If

        If Request.Form("Ok") = "OK" Then
            Me.ButtonCancel.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonSave.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOk.Enabled = True

            Me.ButtonSearch.Enabled = True
            Me.TextBoxFind.ReadOnly = False

            Me.TextBoxAccountType.ReadOnly = True
            Me.TextBoxYear.ReadOnly = True
            Me.TextBoxShortDescription.ReadOnly = True
            Me.TextBoxDescription.ReadOnly = True
            Me.TextBoxInterestRate.ReadOnly = True

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
        End If

    End Sub

    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsInterestRate

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Interest Rates")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxAccountType.Text = CType(l_DataRow("Accttype"), String).Trim
                            If l_DataRow("Year").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxYear.Text = String.Empty
                            Else
                                Me.TextBoxYear.Text = CType(l_DataRow("Year"), String).Trim
                            End If

                            If l_DataRow("Short Desc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDescription.Text = String.Empty
                            Else
                                Me.TextBoxShortDescription.Text = CType(l_DataRow("Short Desc"), String).Trim
                            End If

                            If l_DataRow("Description").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDescription.Text = String.Empty
                            Else
                                Me.TextBoxDescription.Text = CType(l_DataRow("Description"), String).Trim
                            End If

                            If l_DataRow("Interest Rate").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxInterestRate.Text = 0
                            Else
                                Me.TextBoxInterestRate.Text = CType(l_DataRow("Interest Rate"), String).Trim
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
    Public Sub deleteSub()
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If
        g_bool_DeleteFlag = True
        Session("BoolDeleteFlag") = g_bool_DeleteFlag

        Dim l_DataRow As DataRow
        If Not g_dataset_dsInterestRate Is Nothing Then

            g_integer_count = Session("dataset_index")
            l_DataRow = g_dataset_dsInterestRate.Tables(0).Rows(g_integer_count)
            l_DataRow.Delete()
        End If
        YMCARET.YmcaBusinessObject.InterestRatesBOClass.InsertInterestRate(g_dataset_dsInterestRate)
        PopulateData()

        ' Making TextBoxes Blank
        Me.TextBoxAccountType.Text = String.Empty
        Me.TextBoxYear.Text = String.Empty
        Me.TextBoxShortDescription.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxInterestRate.Text = String.Empty

        Me.ButtonDelete.Enabled = False
        ''Me.ButtonEdit.Enabled = False
    End Sub

    Public Sub PopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()
        Try

            g_dataset_dsInterestRate = YMCARET.YmcaBusinessObject.InterestRatesBOClass.LookupInterestRate()
            viewstate("Dataset_InterestRates") = g_dataset_dsInterestRate

            'Vipul 04Feb06 Cache-Session
            'Cache.Add("Interest Rates", g_dataset_dsInterestRate)
            Session("Interest Rates") = g_dataset_dsInterestRate
            'Vipul 04Feb06 Cache-Session

            If g_dataset_dsInterestRate.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else
                Me.DataGridInterestRates.DataSource = g_dataset_dsInterestRate.Tables(0)
                Me.DataGridInterestRates.DataBind()
            End If
            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsInterestRate, Me.DataGridInterestRates, "Year, Short Desc, Interest Rate, Month")


        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try

    End Sub

    Private Sub DataGridInterestRates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridInterestRates.SelectedIndexChanged


        'Me.ButtonEdit.Enabled = True
        g_integer_count = DataGridInterestRates.SelectedIndex

        Session("dataset_index") = g_integer_count

        If Me.PopulateDataIntoControls(g_integer_count) = True Then
            'Me.ButtonEdit.Enabled = True
            Me.ButtonDelete.Enabled = True
        Else
            'Me.ButtonEdit.Enabled = True
            Me.ButtonDelete.Enabled = True
        End If

        Me.TextBoxAccountType.ReadOnly = True
        Me.TextBoxYear.ReadOnly = True
        Me.TextBoxShortDescription.ReadOnly = False
        Me.TextBoxDescription.ReadOnly = False
        Me.TextBoxInterestRate.ReadOnly = False


        Me.ButtonSave.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.TextBoxFind.ReadOnly = True
        Me.ButtonSearch.Enabled = False
        Me.ButtonAdd.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.ButtonOk.Enabled = False
        ''Me.ButtonEdit.Enabled = False

        g_bool_AddFlag = False
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_EditFlag = True
        Session("BoolEditFlag") = g_bool_EditFlag

        g_bool_DeleteFlag = False
        Session("BoolDeleteFlag") = g_bool_DeleteFlag



    End Sub

    Private Sub DataGridInterestRates_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridInterestRates.PageIndexChanged

        DataGridInterestRates.CurrentPageIndex = e.NewPageIndex
        'Bind the DataGrid again with the Data Source
        'depending on wheather Search Flag is true or False

        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If
    End Sub
    Public Sub SearchAndPopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()

        Try

            g_dataset_dsInterestRate = YMCARET.YmcaBusinessObject.InterestRatesBOClass.SearchInterestRates(Me.TextBoxFind.Text)
            viewstate("Dataset_InterestRates") = g_dataset_dsInterestRate
            If g_dataset_dsInterestRate.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True
                'Code Added by Dhananjay on 03/11/2005 
                Me.DataGridInterestRates.DataSource = g_dataset_dsInterestRate
                Me.DataGridInterestRates.DataBind()
            Else
                Me.DataGridInterestRates.CurrentPageIndex = 0
                Me.DataGridInterestRates.DataSource = g_dataset_dsInterestRate
                Me.DataGridInterestRates.DataBind()

            End If
            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsInterestRate, Me.DataGridInterestRates, "Year, Short Desc, Interest Rate, Month")

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True

                'Vipul 04Feb06 Cache-Session                
                'g_dataset_dsInterestRate = Cache.GetData("Interest Rates")
                g_dataset_dsInterestRate = Session("Interest Rates")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsInterestRate.Tables(0).Clear()
                'g_dataset_dsInterestRate.Tables(0).Rows.Clear()
                Me.DataGridInterestRates.DataSource = g_dataset_dsInterestRate
                Me.DataGridInterestRates.DataBind()

            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try



    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If

        g_bool_SearchFlag = True
        Session("BoolSearchFlag") = g_bool_SearchFlag

        Me.TextBoxAccountType.Text = String.Empty
        Me.TextBoxYear.Text = String.Empty
        Me.TextBoxShortDescription.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxInterestRate.Text = String.Empty
        'g_integer_count = Session("dataset_index")
        'If (g_integer_count <> -1) Then
        '    Me.PopulateDataIntoControls(g_integer_count)
        'End If
        ' Do search & populate the data.
        SearchAndPopulateData()

    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If

        g_bool_AddFlag = True
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_SearchFlag = False
        Session("BoolSearchFlag") = g_bool_SearchFlag

        'Making Readonly False for all TextBoxes
        Me.TextBoxAccountType.ReadOnly = False
        Me.TextBoxYear.ReadOnly = False
        Me.TextBoxShortDescription.ReadOnly = False
        Me.TextBoxDescription.ReadOnly = False
        Me.TextBoxInterestRate.ReadOnly = False

        Me.ButtonSave.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.TextBoxFind.ReadOnly = True
        Me.ButtonSearch.Enabled = False
        ''Me.ButtonEdit.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.ButtonOk.Enabled = False
        Me.ButtonAdd.Enabled = False

        ' Making TextBoxes Blank
        Me.TextBoxAccountType.Text = String.Empty
        Me.TextBoxYear.Text = String.Empty
        Me.TextBoxShortDescription.Text = String.Empty
        Me.TextBoxDescription.Text = String.Empty
        Me.TextBoxInterestRate.Text = String.Empty

    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    'If g_bool_SearchFlag = True Then
    ''    '    SearchAndPopulateData()
    ''    'Else
    ''    '    PopulateData()
    ''    'End If

    ''    ' Enable / Disable the controls.

    ''    Me.TextBoxAccountType.ReadOnly = True
    ''    Me.TextBoxYear.ReadOnly = True
    ''    Me.TextBoxShortDescription.ReadOnly = False
    ''    Me.TextBoxDescription.ReadOnly = False
    ''    Me.TextBoxInterestRate.ReadOnly = False


    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonDelete.Enabled = False
    ''    Me.ButtonOk.Enabled = False
    ''    Me.ButtonEdit.Enabled = False

    ''    g_bool_AddFlag = False
    ''    Session("BoolAddFlag") = g_bool_AddFlag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''    g_bool_DeleteFlag = False
    ''    Session("BoolDeleteFlag") = g_bool_DeleteFlag

    ''End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_DataRow As DataRow
        Dim InsertRow As DataRow

        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            g_bool_AddFlag = Session("BoolAddFlag")

            If g_bool_AddFlag = True Then
                If Not g_dataset_dsInterestRate Is Nothing Then

                    InsertRow = g_dataset_dsInterestRate.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Accttype") = TextBoxAccountType.Text.Trim
                    InsertRow.Item("Year") = TextBoxYear.Text.Trim
                    If Me.TextBoxShortDescription.Text.Trim.Length = 0 Then
                        InsertRow.Item("Short Desc") = String.Empty
                    Else
                        InsertRow.Item("Short Desc") = TextBoxShortDescription.Text.Trim
                    End If

                    If Me.TextBoxDescription.Text.Trim.Length = 0 Then
                        InsertRow.Item("Description") = String.Empty
                    Else
                        InsertRow.Item("Description") = TextBoxDescription.Text.Trim
                    End If

                    If Me.TextBoxInterestRate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Interest Rate") = 0
                    Else
                        InsertRow.Item("Interest Rate") = Me.TextBoxInterestRate.Text.Trim
                    End If




                    ' Insert the row into Table.
                    g_dataset_dsInterestRate.Tables(0).Rows.Add(InsertRow)
                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else
                If Not g_dataset_dsInterestRate Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsInterestRate.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        If Me.TextBoxShortDescription.Text.Trim.Length = 0 Then
                            l_DataRow("Short Desc") = String.Empty
                        Else
                            l_DataRow("Short Desc") = TextBoxShortDescription.Text.Trim
                        End If

                        If Me.TextBoxDescription.Text.Trim.Length = 0 Then
                            l_DataRow("Description") = String.Empty
                        Else
                            l_DataRow("Description") = TextBoxDescription.Text.Trim
                        End If

                        If Me.TextBoxInterestRate.Text.Trim.Length = 0 Then
                            l_DataRow("Interest Rate") = 0
                        Else
                            l_DataRow("Interest Rate") = TextBoxInterestRate.Text.Trim
                        End If
                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If
                    End If

                End If
            End If
            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.InterestRatesBOClass.InsertInterestRate(g_dataset_dsInterestRate)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            '' Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = True

            Me.TextBoxAccountType.ReadOnly = True
            Me.TextBoxYear.ReadOnly = True
            Me.TextBoxShortDescription.ReadOnly = True
            Me.TextBoxDescription.ReadOnly = True
            Me.TextBoxInterestRate.ReadOnly = True
            Me.DataGridInterestRates.SelectedIndex = -1


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

        Me.TextBoxAccountType.ReadOnly = True
        Me.TextBoxYear.ReadOnly = True
        Me.TextBoxShortDescription.ReadOnly = True
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxInterestRate.ReadOnly = True
        Me.DataGridInterestRates.SelectedIndex = -1

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

        MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you SURE you want to delete this Acct Type?", MessageBoxButtons.YesNo)

    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Session("InterestSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub DataGridInterestRates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridInterestRates.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridInterestRates.SelectedIndex And Me.DataGridInterestRates.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(2).Visible = False
        e.Item.Cells(3).Visible = False
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
    End Sub

    Private Sub DataGridInterestRates_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridInterestRates.SortCommand

        Try
            If Not viewstate("Dataset_InterestRates") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsInterestRate = viewstate("Dataset_InterestRates")
                dv = g_dataset_dsInterestRate.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("InterestSort") Is Nothing Then
                    If Session("InterestSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridInterestRates.DataSource = Nothing
                Me.DataGridInterestRates.DataSource = dv
                Me.DataGridInterestRates.DataBind()
                Session("InterestSort") = dv.Sort
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
            DataGridInterestRates.Enabled = False
            DataGridInterestRates.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
