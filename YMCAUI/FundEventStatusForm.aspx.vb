'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	FundEventStatusForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:16:42 PM
' Program Specification Name	:	YMCA PS 5.4.1.8.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Changed by			:	Shefali Bharti  
' Changed on			:	08/12/2005
' Change Description	:	Coding
'****************************************************
'Modification History
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




Public Class FundEventStatusForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("FundEventStatusForm.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsFundEventStatus As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator4 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator5 As System.Web.UI.WebControls.RegularExpressionValidator
    Dim g_bool_DeleteFlag As New Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtLookUp As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridFundStatus As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextBoxFundStatusType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCanChange As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxDeposits As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxService As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAutoTerm As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxLookUp As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStatusType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCanChange As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInterest As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDeposits As System.Web.UI.WebControls.Label
    Protected WithEvents LabelService As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAutoTerm As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLookFor As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label

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
        'Me.DataGridFundStatus.DataSource = CommonModule.CreateDataSource
        'Me.DataGridFundStatus.DataBind()

        Me.LabelAutoTerm.AssociatedControlID = Me.TextBoxAutoTerm.ID
        Me.LabelCanChange.AssociatedControlID = Me.TextBoxCanChange.ID
        Me.LabelDeposits.AssociatedControlID = Me.TextBoxDeposits.ID
        Me.LabelDesc.AssociatedControlID = Me.TextBoxDesc.ID
        Me.LabelInterest.AssociatedControlID = Me.TextBoxInterest.ID
        Me.LabelLookFor.AssociatedControlID = Me.TextBoxLookUp.ID
        Me.LabelService.AssociatedControlID = Me.TextBoxService.ID
        Me.LabelShortDesc.AssociatedControlID = Me.TextBoxShortDesc.ID
        Me.LabelStatusType.AssociatedControlID = Me.TextBoxFundStatusType.ID
        Try

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
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
                ' populating the textboxes with the current row
                g_integer_count = Session("dataset_index")
                If (g_integer_count <> -1) Then
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

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
            ' deletes if yes
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
                Me.ButtonOK.Enabled = True

                Me.ButtonSearch.Enabled = True
                Me.TextBoxLookUp.ReadOnly = False

                Me.TextBoxAutoTerm.ReadOnly = True
                Me.TextBoxCanChange.ReadOnly = True
                Me.TextBoxDeposits.ReadOnly = True
                Me.TextBoxDesc.ReadOnly = True
                Me.TextBoxFundStatusType.ReadOnly = True
                Me.TextBoxInterest.ReadOnly = True
                Me.TextBoxService.ReadOnly = True
                Me.TextBoxShortDesc.ReadOnly = True
                If g_bool_SearchFlag = True Then
                    SearchAndPopulateData()
                Else
                    PopulateData()
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DeleteSub()
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            g_bool_DeleteFlag = True
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            Dim l_DataRow As DataRow
            If Not g_dataset_dsFundEventStatus Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsFundEventStatus.Tables(0).Rows(g_integer_count)
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.FundEventStatus.InsertFundEventStatus(g_dataset_dsFundEventStatus)
            PopulateData()



            Me.ButtonDelete.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
        ''Me.ButtonEdit.Enabled = False
    End Sub
    Public Sub PopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()

        Try
            g_dataset_dsFundEventStatus = YMCARET.YmcaBusinessObject.FundEventStatus.LookupInterestRate()
            Viewstate("Dataset_FundEvent") = g_dataset_dsFundEventStatus

            'Vipul 04Feb06 Cache-Session
            'Cache.Add("Fund Event Status", g_dataset_dsFundEventStatus)
            Session("Fund Event Status") = g_dataset_dsFundEventStatus
            'Vipul 04Feb06 Cache-Session

            If g_dataset_dsFundEventStatus.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxLookUp.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridFundStatus.DataSource = g_dataset_dsFundEventStatus.Tables(0)
                Me.DataGridFundStatus.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsFundEventStatus, Me.DataGridFundStatus, "Desc, Can Change,Interest,Deposits,Service,Autoterm")

            'Me.DataGridFundStatus.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True

                'Vipul 04Feb06 Cache-Session
                'g_dataset_dsFundEventStatus = Cache.GetData("Fund Event Status")
                g_dataset_dsFundEventStatus = Session("Fund Event Status")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsFundEventStatus.Tables(0).Clear()
                Me.DataGridFundStatus.DataSource = g_dataset_dsFundEventStatus
                Me.DataGridFundStatus.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub

    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsFundEventStatus

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Fund Event Status")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxFundStatusType.Text = CType(l_DataRow("Status"), String).Trim
                            If l_DataRow("ShortDesc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDesc.Text = String.Empty
                            Else
                                Me.TextBoxShortDesc.Text = CType(l_DataRow("ShortDesc"), String).Trim
                            End If

                            If l_DataRow("Desc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDesc.Text = String.Empty
                            Else
                                Me.TextBoxDesc.Text = CType(l_DataRow("Desc"), String).Trim
                            End If

                            If l_DataRow("Can Change").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxCanChange.Text = "F"
                            Else
                                If CType(l_DataRow("Can Change"), Boolean) Then
                                    Me.TextBoxCanChange.Text = "T"
                                Else
                                    Me.TextBoxCanChange.Text = "F"
                                End If
                            End If

                            If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxInterest.Text = "F"
                            Else
                                If CType(l_DataRow("Interest"), Boolean) Then
                                    Me.TextBoxInterest.Text = "T"
                                Else
                                    Me.TextBoxInterest.Text = "F"
                                End If
                            End If

                            If l_DataRow("Deposits").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDeposits.Text = "F"
                            Else
                                If CType(l_DataRow("Deposits"), Boolean) Then
                                    Me.TextBoxDeposits.Text = "T"
                                Else
                                    Me.TextBoxDeposits.Text = "F"
                                End If
                            End If

                            If l_DataRow("Service").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxService.Text = "F"
                            Else
                                If CType(l_DataRow("Service"), Boolean) Then
                                    Me.TextBoxService.Text = "T"
                                Else
                                    Me.TextBoxService.Text = "F"
                                End If
                            End If

                            If l_DataRow("Autoterm").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxAutoTerm.Text = "F"
                            Else
                                If CType(l_DataRow("Autoterm"), Boolean) Then
                                    Me.TextBoxAutoTerm.Text = "T"
                                Else
                                    Me.TextBoxAutoTerm.Text = "F"
                                End If
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


    Private Sub DataGridFundStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridFundStatus.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            ''Me.ButtonEdit.Enabled = True
            g_integer_count = DataGridFundStatus.SelectedIndex '(((DataGridFundStatus.CurrentPageIndex) * DataGridFundStatus.PageSize) + DataGridFundStatus.SelectedIndex)

            Session("dataset_index") = g_integer_count
            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                ''Me.ButtonEdit.Enabled = True
                Me.ButtonDelete.Enabled = True
            Else
            '' Me.ButtonEdit.Enabled = True
            Me.ButtonDelete.Enabled = True
            End If

            Me.TextBoxAutoTerm.ReadOnly = False
            Me.TextBoxCanChange.ReadOnly = False
            Me.TextBoxDeposits.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False
            Me.TextBoxFundStatusType.ReadOnly = True
            Me.TextBoxInterest.ReadOnly = False
            Me.TextBoxService.ReadOnly = False
            Me.TextBoxShortDesc.ReadOnly = False


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxLookUp.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            Me.ButtonAdd.Enabled = False
            'Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False
            ''Me.ButtonEdit.Enabled = False

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Method called here 
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridFundStatus_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridFundStatus.PageIndexChanged

        DataGridFundStatus.CurrentPageIndex = e.NewPageIndex


        'Bind the DataGrid again with the Data Source
        'depending on wheather Search Flag is true or False

        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            g_bool_SearchFlag = True
            Session("BoolSearchFlag") = g_bool_SearchFlag

            ' Clear the controls.
            Me.TextBoxAutoTerm.Text = String.Empty
            Me.TextBoxCanChange.Text = String.Empty
            Me.TextBoxDeposits.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxFundStatusType.Text = String.Empty
            Me.TextBoxInterest.Text = String.Empty
            Me.TextBoxService.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty

            'g_integer_count = Session("dataset_index")
            'If (g_integer_count <> -1) Then
            '    Me.PopulateDataIntoControls(g_integer_count)
            'End If


            ' Do search & populate the data.
            SearchAndPopulateData()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SearchAndPopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()

        Try

            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            g_dataset_dsFundEventStatus = YMCARET.YmcaBusinessObject.FundEventStatus.SearchFundEventStatus(Me.TextBoxLookUp.Text)
            Viewstate("Dataset_FundEvent") = g_dataset_dsFundEventStatus
            If g_dataset_dsFundEventStatus.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridFundStatus.CurrentPageIndex = 0
                Me.DataGridFundStatus.DataSource = g_dataset_dsFundEventStatus
                Me.DataGridFundStatus.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsFundEventStatus, Me.DataGridFundStatus, "Desc, Can Change,Interest,Deposits,Service,Autoterm")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True

                'Vipul 04Feb06 Cache-Session
                'g_dataset_dsFundEventStatus = Cache.GetData("Fund Event Status")
                g_dataset_dsFundEventStatus = Session("Fund Event Status")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsFundEventStatus.Tables(0).Clear()
                Me.DataGridFundStatus.DataSource = g_dataset_dsFundEventStatus
                Me.DataGridFundStatus.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try



    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    ' Enable / Disable the controls.
    ''    Me.TextBoxAutoTerm.ReadOnly = False
    ''    Me.TextBoxCanChange.ReadOnly = False
    ''    Me.TextBoxDeposits.ReadOnly = False
    ''    Me.TextBoxDesc.ReadOnly = False
    ''    Me.TextBoxFundStatusType.ReadOnly = True
    ''    Me.TextBoxInterest.ReadOnly = False
    ''    Me.TextBoxService.ReadOnly = False
    ''    Me.TextBoxShortDesc.ReadOnly = False


    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxLookUp.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonDelete.Enabled = False
    ''    Me.ButtonOK.Enabled = False
    ''    Me.ButtonEdit.Enabled = False

    ''    g_bool_AddFlag = False
    ''    Session("BoolAddFlag") = g_bool_AddFlag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''    g_bool_DeleteFlag = False
    ''    Session("BoolDeleteFlag") = g_bool_DeleteFlag

    ''End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
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
            Me.TextBoxAutoTerm.ReadOnly = False
            Me.TextBoxCanChange.ReadOnly = False
            Me.TextBoxDeposits.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False
            Me.TextBoxFundStatusType.ReadOnly = False
            Me.TextBoxInterest.ReadOnly = False
            Me.TextBoxService.ReadOnly = False
            Me.TextBoxShortDesc.ReadOnly = False


            ' Making TextBoxes Blank
            Me.TextBoxAutoTerm.Text = String.Empty
            Me.TextBoxCanChange.Text = String.Empty
            Me.TextBoxDeposits.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxFundStatusType.Text = String.Empty
            Me.TextBoxInterest.Text = String.Empty
            Me.TextBoxService.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty

            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxLookUp.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            PopulateData()

            'Enable / Disable the controls
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.TextBoxLookUp.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOK.Enabled = True

            'Making Readonly True for all TextBoxes
            Me.TextBoxAutoTerm.ReadOnly = True
            Me.TextBoxCanChange.ReadOnly = True
            Me.TextBoxDeposits.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxFundStatusType.ReadOnly = True
            Me.TextBoxInterest.ReadOnly = True
            Me.TextBoxService.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.DataGridFundStatus.SelectedIndex = -1
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


        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Session("FundEventSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

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
                If Not g_dataset_dsFundEventStatus Is Nothing Then

                    InsertRow = g_dataset_dsFundEventStatus.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Status") = TextBoxFundStatusType.Text.Trim
                    If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("ShortDesc") = String.Empty
                    Else
                        InsertRow.Item("ShortDesc") = TextBoxShortDesc.Text.Trim
                    End If

                    If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Desc") = String.Empty
                    Else
                        InsertRow.Item("Desc") = TextBoxDesc.Text.Trim
                    End If

                    If Me.TextBoxCanChange.Text.Trim.Length = 0 Then
                        InsertRow.Item("Can Change") = 0
                    Else
                        If Me.TextBoxCanChange.Text.Trim = "F" Then
                            InsertRow.Item("Can Change") = 0
                        Else
                            InsertRow.Item("Can Change") = 1
                        End If
                    End If

                    If Me.TextBoxInterest.Text.Trim.Length = 0 Then
                        InsertRow.Item("Interest") = 0
                    Else
                        If Me.TextBoxInterest.Text.Trim = "F" Then
                            InsertRow.Item("Interest") = 0
                        Else
                            InsertRow.Item("Interest") = 1
                        End If
                    End If

                    If Me.TextBoxDeposits.Text.Trim.Length = 0 Then
                        InsertRow.Item("Deposits") = 0
                    Else
                        If Me.TextBoxDeposits.Text.Trim = "F" Then
                            InsertRow.Item("Deposits") = 0
                        Else
                            InsertRow.Item("Deposits") = 1
                        End If
                    End If

                    If Me.TextBoxService.Text.Trim.Length = 0 Then
                        InsertRow.Item("Service") = 0
                    Else
                        If Me.TextBoxService.Text.Trim = "F" Then
                            InsertRow.Item("Service") = 0
                        Else
                            InsertRow.Item("Service") = 1
                        End If
                    End If

                    If Me.TextBoxAutoTerm.Text.Trim.Length = 0 Then
                        InsertRow.Item("Autoterm") = 0
                    Else
                        If Me.TextBoxAutoTerm.Text.Trim = "F" Then
                            InsertRow.Item("Autoterm") = 0
                        Else
                            InsertRow.Item("Autoterm") = 1
                        End If
                    End If

                    ' Insert the row into Table.
                    g_dataset_dsFundEventStatus.Tables(0).Rows.Add(InsertRow)

                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not g_dataset_dsFundEventStatus Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsFundEventStatus.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        l_DataRow("Status") = TextBoxFundStatusType.Text.Trim
                        If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                            l_DataRow("ShortDesc") = String.Empty
                        Else
                            l_DataRow("ShortDesc") = TextBoxShortDesc.Text.Trim
                        End If

                        If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Desc") = String.Empty
                        Else
                            l_DataRow("Desc") = TextBoxDesc.Text.Trim
                        End If

                        If Me.TextBoxCanChange.Text.Trim.Length = 0 Then
                            l_DataRow("Can Change") = 0
                        Else
                            If Me.TextBoxCanChange.Text.Trim = "F" Then
                                l_DataRow("Can Change") = 0
                            Else
                                l_DataRow("Can Change") = 1
                            End If
                        End If

                        If Me.TextBoxInterest.Text.Trim.Length = 0 Then
                            l_DataRow("Interest") = 0
                        Else
                            If Me.TextBoxInterest.Text.Trim = "F" Then
                                l_DataRow("Interest") = 0
                            Else
                                l_DataRow("Interest") = 1
                            End If
                        End If

                        If Me.TextBoxDeposits.Text.Trim.Length = 0 Then
                            l_DataRow("Deposits") = 0
                        Else
                            If Me.TextBoxDeposits.Text.Trim = "F" Then
                                l_DataRow("Deposits") = 0
                            Else
                                l_DataRow("Deposits") = 1
                            End If
                        End If

                        If Me.TextBoxService.Text.Trim.Length = 0 Then
                            l_DataRow("Service") = 0
                        Else
                            If Me.TextBoxService.Text.Trim = "F" Then
                                l_DataRow("Service") = 0
                            Else
                                l_DataRow("Service") = 1
                            End If
                        End If

                        If Me.TextBoxAutoTerm.Text.Trim.Length = 0 Then
                            l_DataRow("Autoterm") = 0
                        Else
                            If Me.TextBoxAutoTerm.Text.Trim = "F" Then
                                l_DataRow("Autoterm") = 0
                            Else
                                l_DataRow("Autoterm") = 1
                            End If
                        End If


                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If
                    End If

                End If
            End If
            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.FundEventStatus.InsertFundEventStatus(g_dataset_dsFundEventStatus)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxLookUp.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = True

            Me.TextBoxAutoTerm.ReadOnly = True
            Me.TextBoxCanChange.ReadOnly = True
            Me.TextBoxDeposits.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxFundStatusType.ReadOnly = True
            Me.TextBoxInterest.ReadOnly = True
            Me.TextBoxService.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.DataGridFundStatus.SelectedIndex = -1

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

    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If
        MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you SURE you want to delete this Code Value?", MessageBoxButtons.YesNo)
    End Sub

    Private Sub DataGridFundStatus_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridFundStatus.ItemDataBound

        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridFundStatus.SelectedIndex And Me.DataGridFundStatus.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(3).Visible = False
        e.Item.Cells(4).Visible = False
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
        e.Item.Cells(7).Visible = False
        e.Item.Cells(8).Visible = False

    End Sub

    Private Sub DataGridFundStatus_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridFundStatus.SortCommand

        Try
            Me.DataGridFundStatus.SelectedIndex = -1
            If Not Viewstate("Dataset_FundEvent") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsFundEventStatus = viewstate("Dataset_FundEvent")
                dv = g_dataset_dsFundEventStatus.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("FundEventSort") Is Nothing Then
                    If Session("FundEventSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridFundStatus.DataSource = Nothing
                Me.DataGridFundStatus.DataSource = dv
                Me.DataGridFundStatus.DataBind()
                Session("FundEventSort") = dv.Sort
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
            DataGridFundStatus.Enabled = False
            DataGridFundStatus.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 
End Class
