'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	LookupsMaintenance.aspx.vb
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 4:15:21 PM
' Program Specification Name	:	YMCA PS 5.4.1.15
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	Ruchi Saxena
' Changed on			:	08/02/2005
' Change Description	:	Data not coming in the grid 

' Changed by			:	Shefali Bharti
' Changed on			:	08/14/2005
' Change Description	:	Completed the coding
' Cache-Session         :   Vipul 04Feb06 
'*******************************************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Singh                   16/June/2010        Migration related changes
'Shashi Singh                   07/July/2010        Code review changes.
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


Public Class LookupsMaintenance
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("LookupsMaintenance.aspx")
    'End issue id YRS 5.0-940
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridLookupMaintenance As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGroup As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGroup As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSubGroup1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSubGroup1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSubGroup2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSubGroup3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSubGroup3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCodeOrder As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCodeOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEditable As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCodeValue As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCodeValue As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEffDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffDate As YMCAUI.DateUserControl
    Protected WithEvents LabelUniqueId As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxUniqueId As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents Menu1 As skmMenu.Menu
    ''Protected WithEvents ButtonEditForm As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEditable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSubGroup2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelActive As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxActive As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditForm As System.Web.UI.WebControls.Button


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim g_dataset_dsLookups As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Dim g_bool_DeleteFlag As New Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            'Me.DataGridLookupMaintenance.DataSource = CommonModule.CreateDataSource
            'Me.DataGridLookupMaintenance.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
            Me.LabelGroup.AssociatedControlID = Me.TextBoxGroup.ID
            Me.LabelSubGroup1.AssociatedControlID = Me.TextBoxSubGroup1.ID
            Me.LabelSubGroup2.AssociatedControlID = Me.TextBoxSubGroup2.ID
            Me.LabelSubGroup3.AssociatedControlID = Me.TextBoxSubGroup3.ID
            Me.LabelUniqueId.AssociatedControlID = Me.TextBoxUniqueId.ID
            Me.LabelEditable.AssociatedControlID = Me.TextBoxEditable.ID
            Me.LabelDesc.AssociatedControlID = Me.TextBoxDesc.ID
            Me.LabelCodeValue.AssociatedControlID = Me.TextBoxCodeValue.ID
            Me.LabelCodeOrder.AssociatedControlID = Me.TextBoxCodeOrder.ID
            Me.LabelShortDesc.AssociatedControlID = Me.TextBoxShortDesc.ID
            Me.LabelActive.AssociatedControlID = Me.TextBoxActive.ID
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
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
                ''Me.ButtonEditForm.Enabled = False
                Me.ButtonAdd.Enabled = True
                Me.ButtonOK.Enabled = True

                Me.ButtonSearch.Enabled = True
                Me.TextBoxFind.ReadOnly = False

                Me.TextBoxGroup.ReadOnly = True
                Me.TextBoxActive.ReadOnly = True
                Me.TextBoxCodeValue.ReadOnly = True
                Me.TextBoxDesc.ReadOnly = True
                Me.TextBoxEditable.ReadOnly = True
                Me.TextBoxEffDate.Enabled = False

                Me.TextBoxShortDesc.ReadOnly = True
                Me.TextBoxSubGroup1.ReadOnly = True
                Me.TextBoxSubGroup2.ReadOnly = True
                Me.TextBoxSubGroup3.ReadOnly = True
                Me.TextBoxUniqueId.ReadOnly = True
                Me.TextBoxCodeOrder.ReadOnly = True

                If g_bool_SearchFlag = True Then
                    SearchAndPopulateData()
                Else
                    PopulateData()
                End If
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
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
            If Not g_dataset_dsLookups Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsLookups.Tables(0).Rows(g_integer_count)
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.InsertLookups(g_dataset_dsLookups)
            PopulateData()



            Me.ButtonDelete.Enabled = False
            ''Me.ButtonEditForm.Enabled = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Public Sub PopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()
        Try

            g_dataset_dsLookups = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.LookupLookups()
            viewstate("Dataset_Lookups") = g_dataset_dsLookups

            'Vipul 04Feb06 Cache-Session
            'Cache.Add("Lookups", g_dataset_dsLookups)
            Session("Lookups") = g_dataset_dsLookups
            'Vipul 04Feb06 Cache-Session

            If g_dataset_dsLookups.Tables.Count = 0 Then
                ''Me.ButtonEditForm.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridLookupMaintenance.DataSource = g_dataset_dsLookups.Tables(0)
                Me.DataGridLookupMaintenance.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsLookups, Me.DataGridLookupMaintenance, "SubGroup1, SubGroup2,SubGroup3,Code Order,ShortDesc,Desc,Active,Editable,Effective Date,Unique ID")

            'Me.DataGridLookupMaintenance.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                'Vipul 04Feb06 Cache-Session
                'g_dataset_dsLookups = Cache.GetData("Lookups")
                g_dataset_dsLookups = Session("Lookups")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsLookups.Tables(0).Clear()
                Me.DataGridLookupMaintenance.DataSource = g_dataset_dsLookups
                Me.DataGridLookupMaintenance.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)

        End Try
    End Sub
    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsLookups

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Lookups")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxGroup.Text = CType(l_DataRow("Group"), String).Trim
                            If l_DataRow("SubGroup1").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxSubGroup1.Text = String.Empty
                            Else
                                Me.TextBoxSubGroup1.Text = CType(l_DataRow("SubGroup1"), String).Trim
                            End If

                            If l_DataRow("SubGroup2").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxSubGroup2.Text = String.Empty
                            Else
                                Me.TextBoxSubGroup2.Text = CType(l_DataRow("SubGroup2"), String).Trim
                            End If

                            If l_DataRow("SubGroup3").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxSubGroup3.Text = String.Empty
                            Else
                                Me.TextBoxSubGroup3.Text = CType(l_DataRow("SubGroup3"), String).Trim
                            End If

                            If l_DataRow("Code Order").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxCodeOrder.Text = String.Empty
                            Else
                                Me.TextBoxCodeOrder.Text = CType(l_DataRow("Code Order"), String).Trim
                            End If

                            Me.TextBoxCodeValue.Text = CType(l_DataRow("Code Value"), String).Trim

                            If l_DataRow("ShortDesc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDesc.Text = String.Empty
                            Else
                                Me.TextBoxShortDesc.Text = DirectCast(l_DataRow("ShortDesc"), String).Trim
                            End If

                            If l_DataRow("Desc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDesc.Text = String.Empty
                            Else
                                Me.TextBoxDesc.Text = DirectCast(l_DataRow("Desc"), String).Trim
                            End If

                            If l_DataRow("Active").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxActive.Text = "F"
                            Else
                                If CType(l_DataRow("Active"), Boolean) Then
                                    Me.TextBoxActive.Text = "T"
                                Else
                                    Me.TextBoxActive.Text = "F"
                                End If
                            End If

                            If l_DataRow("Editable").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxEditable.Text = "F"
                            Else
                                If CType(l_DataRow("Editable"), Boolean) Then
                                    Me.TextBoxEditable.Text = "T"
                                Else
                                    Me.TextBoxEditable.Text = "F"
                                End If
                            End If

                            If l_DataRow("Effective Date").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxEffDate.Text = "01/01/1900"
                            Else
                                Me.TextBoxEffDate.Text = CType(l_DataRow("Effective Date"), String).Trim
                            End If

                            If l_DataRow("Unique ID").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxUniqueId.Text = String.Empty
                            Else
                                Me.TextBoxUniqueId.Text = CType(l_DataRow("Unique ID"), String).Trim
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

    Public Sub SearchAndPopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()
        Try

            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            g_dataset_dsLookups = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.SearchLookups(Me.TextBoxFind.Text)
            viewstate("Dataset_Lookups") = g_dataset_dsLookups
            If g_dataset_dsLookups.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridLookupMaintenance.DataSource = g_dataset_dsLookups
                Me.DataGridLookupMaintenance.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsLookups, Me.DataGridLookupMaintenance, "SubGroup1, SubGroup2,SubGroup3,Code Order,ShortDesc,Desc,Active,Editable,Effective Date,Unique ID")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True

                'Vipul 04Feb06 Cache-Session
                'g_dataset_dsLookups = Cache.GetData("Lookups")
                g_dataset_dsLookups = Session("Lookups")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsLookups.Tables(0).Clear()
                Me.DataGridLookupMaintenance.DataSource = g_dataset_dsLookups
                Me.DataGridLookupMaintenance.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)

        End Try



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
        ' Me.TextBoxFind.Text = ""
        Me.TextBoxActive.Text = String.Empty
        Me.TextBoxCodeOrder.Text = String.Empty
        Me.TextBoxCodeValue.Text = String.Empty
        Me.TextBoxDesc.Text = String.Empty
        Me.TextBoxEffDate.Text = String.Empty
        Me.TextBoxEditable.Text = String.Empty
        Me.TextBoxEditable.Text = String.Empty
        Me.TextBoxGroup.Text = String.Empty
        Me.TextBoxShortDesc.Text = String.Empty
        Me.TextBoxSubGroup1.Text = String.Empty
        Me.TextBoxSubGroup2.Text = String.Empty
        Me.TextBoxSubGroup3.Text = String.Empty
        Me.TextBoxUniqueId.Text = String.Empty

        'g_integer_count = Session("dataset_index")
        'If (g_integer_count <> -1) Then
        '    Me.PopulateDataIntoControls(g_integer_count)
        'End If

        ' Do search & populate the data.
            SearchAndPopulateData()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub DataGridLookupMaintenance_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridLookupMaintenance.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            ''Me.ButtonEditForm.Enabled = True
            g_integer_count = DataGridLookupMaintenance.SelectedIndex

            Session("dataset_index") = g_integer_count
            If Me.PopulateDataIntoControls(g_integer_count) = True Then
               
                    ''Me.ButtonEditForm.Enabled = True
                    Me.ButtonDelete.Enabled = True

            Else
                ''Me.ButtonEditForm.Enabled = True
                Me.ButtonDelete.Enabled = True
            End If

                Me.TextBoxGroup.ReadOnly = True
                Me.TextBoxActive.ReadOnly = False
                Me.TextBoxCodeValue.ReadOnly = True
                Me.TextBoxDesc.ReadOnly = False
                Me.TextBoxEditable.ReadOnly = False
                Me.TextBoxEffDate.Enabled = True

                Me.TextBoxShortDesc.ReadOnly = False
                Me.TextBoxSubGroup1.ReadOnly = False
                Me.TextBoxSubGroup2.ReadOnly = False
                Me.TextBoxSubGroup3.ReadOnly = False
                Me.TextBoxUniqueId.ReadOnly = True
                Me.TextBoxCodeOrder.ReadOnly = False

                Me.ButtonSave.Enabled = True
                Me.ButtonCancel.Enabled = True
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonAdd.Enabled = False
                Me.ButtonDelete.Enabled = True
                Me.ButtonOK.Enabled = False
                ''Me.ButtonEditForm.Enabled = False

                g_bool_AddFlag = False
                Session("BoolAddFlag") = g_bool_AddFlag

                g_bool_EditFlag = True
                Session("BoolEditFlag") = g_bool_EditFlag

                g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridLookupMaintenance_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridLookupMaintenance.PageIndexChanged
        Try
            DataGridLookupMaintenance.CurrentPageIndex = e.NewPageIndex

            'Bind the DataGrid again with the Data Source
            'depending on wheather Search Flag is true or False

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    ''Private Sub ButtonEditForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEditForm.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    ' Enable / Disable the controls.


    ''    Me.TextBoxGroup.ReadOnly = True
    ''    Me.TextBoxActive.ReadOnly = False
    ''    Me.TextBoxCodeValue.ReadOnly = True
    ''    Me.TextBoxDesc.ReadOnly = False
    ''    Me.TextBoxEditable.ReadOnly = False
    ''    Me.TextBoxEffDate.Enabled = True

    ''    Me.TextBoxShortDesc.ReadOnly = False
    ''    Me.TextBoxSubGroup1.ReadOnly = False
    ''    Me.TextBoxSubGroup2.ReadOnly = False
    ''    Me.TextBoxSubGroup3.ReadOnly = False
    ''    Me.TextBoxUniqueId.ReadOnly = True
    ''    Me.TextBoxCodeOrder.ReadOnly = False

    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonDelete.Enabled = False
    ''    Me.ButtonOK.Enabled = False
    ''    Me.ButtonEditForm.Enabled = False

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
            Me.TextBoxGroup.ReadOnly = False
            Me.TextBoxActive.ReadOnly = False
            Me.TextBoxCodeValue.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False
            Me.TextBoxEditable.ReadOnly = False
            Me.TextBoxEffDate.Enabled = True

            Me.TextBoxShortDesc.ReadOnly = False
            Me.TextBoxSubGroup1.ReadOnly = False
            Me.TextBoxSubGroup2.ReadOnly = False
            Me.TextBoxSubGroup3.ReadOnly = False
            Me.TextBoxUniqueId.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = False


            ' Making TextBoxes Blank
            Me.TextBoxGroup.Text = String.Empty
            Me.TextBoxActive.Text = String.Empty
            Me.TextBoxCodeValue.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxEditable.Text = String.Empty
            Me.TextBoxEffDate.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxSubGroup1.Text = String.Empty
            Me.TextBoxSubGroup2.Text = String.Empty
            Me.TextBoxSubGroup3.Text = String.Empty
            Me.TextBoxUniqueId.Text = "Auto-Generated GUID"
            Me.TextBoxCodeOrder.Text = String.Empty


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            ''Me.ButtonEditForm.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            PopulateData()

            'Enable / Disable the controls
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOK.Enabled = True

            'Making Readonly True for all TextBoxes
            Me.TextBoxGroup.ReadOnly = True
            Me.TextBoxActive.ReadOnly = True
            Me.TextBoxCodeValue.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxEditable.ReadOnly = True
            Me.TextBoxEffDate.Enabled = False

            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxSubGroup1.ReadOnly = True
            Me.TextBoxSubGroup2.ReadOnly = True
            Me.TextBoxSubGroup3.ReadOnly = True
            Me.TextBoxUniqueId.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = True
            Me.DataGridLookupMaintenance.SelectedIndex = -1
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
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try


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
                If Not g_dataset_dsLookups Is Nothing Then

                    InsertRow = g_dataset_dsLookups.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Group") = TextBoxGroup.Text.Trim
                    If Me.TextBoxSubGroup1.Text.Trim.Length = 0 Then
                        InsertRow.Item("SubGroup1") = String.Empty
                    Else
                        InsertRow.Item("SubGroup1") = TextBoxCodeValue.Text.Trim
                    End If

                    If Me.TextBoxSubGroup2.Text.Trim.Length = 0 Then
                        InsertRow.Item("SubGroup2") = String.Empty
                    Else
                        InsertRow.Item("SubGroup2") = TextBoxSubGroup2.Text.Trim
                    End If

                    If Me.TextBoxSubGroup3.Text.Trim.Length = 0 Then
                        InsertRow.Item("SubGroup3") = String.Empty
                    Else
                        InsertRow.Item("SubGroup3") = TextBoxSubGroup3.Text.Trim
                    End If

                    If Me.TextBoxCodeOrder.Text.Trim.Length = 0 Then
                        InsertRow.Item("Code Order") = 0
                    Else
                        InsertRow.Item("Code Order") = TextBoxCodeOrder.Text.Trim
                    End If

                    InsertRow.Item("Code Value") = TextBoxCodeValue.Text.Trim

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

                    If Me.TextBoxActive.Text.Trim.Length = 0 Then
                        InsertRow.Item("Active") = 0
                    Else
                        If Me.TextBoxActive.Text.Trim = "F" Then
                            InsertRow.Item("Active") = 0
                        Else
                            InsertRow.Item("Active") = 1
                        End If
                    End If

                    If Me.TextBoxEditable.Text.Trim.Length = 0 Then
                        InsertRow.Item("Editable") = 0
                    Else
                        If Me.TextBoxEditable.Text.Trim = "F" Then
                            InsertRow.Item("Editable") = 0
                        Else
                            InsertRow.Item("Editable") = 1
                        End If
                    End If

                    If Me.TextBoxEffDate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Effective Date") = "01/01/1900"
                    Else
                        InsertRow.Item("Effective Date") = TextBoxEffDate.Text.Trim
                    End If

                    InsertRow.Item("Unique ID") = TextBoxUniqueId.Text.Trim

                    ' Insert the row into Table.
                    g_dataset_dsLookups.Tables(0).Rows.Add(InsertRow)

                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not g_dataset_dsLookups Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsLookups.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        l_DataRow("Group") = TextBoxGroup.Text.Trim
                        If Me.TextBoxSubGroup1.Text.Trim.Length = 0 Then
                            l_DataRow("SubGroup1") = String.Empty
                        Else
                            l_DataRow("SubGroup1") = TextBoxSubGroup1.Text.Trim
                        End If

                        If Me.TextBoxSubGroup2.Text.Trim.Length = 0 Then
                            l_DataRow("SubGroup2") = String.Empty
                        Else
                            l_DataRow("SubGroup2") = TextBoxSubGroup2.Text.Trim
                        End If

                        If Me.TextBoxSubGroup3.Text.Trim.Length = 0 Then
                            l_DataRow("SubGroup3") = String.Empty
                        Else
                            l_DataRow("SubGroup3") = TextBoxSubGroup3.Text.Trim
                        End If

                        If Me.TextBoxCodeOrder.Text.Trim.Length = 0 Then
                            l_DataRow("Code Order") = 0
                        Else
                            l_DataRow("Code Order") = TextBoxCodeOrder.Text.Trim
                        End If

                        l_DataRow("Code Value") = TextBoxCodeValue.Text.Trim

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

                        If Me.TextBoxActive.Text.Trim.Length = 0 Then
                            l_DataRow("Active") = 0
                        Else
                            If Me.TextBoxActive.Text.Trim = "F" Then
                                l_DataRow("Active") = 0
                            Else
                                l_DataRow("Active") = 1
                            End If
                        End If

                        If Me.TextBoxEditable.Text.Trim.Length = 0 Then
                            l_DataRow("Editable") = 0
                        Else
                            If Me.TextBoxEditable.Text.Trim = "F" Then
                                l_DataRow("Editable") = 0
                            Else
                                l_DataRow("Editable") = 1
                            End If
                        End If

                        If Me.TextBoxEffDate.Text.Trim.Length = 0 Then
                            l_DataRow("Effective Date") = "01/01/1900"
                        Else
                            l_DataRow("Effective Date") = TextBoxEffDate.Text.Trim
                        End If

                        l_DataRow("Unique ID") = TextBoxUniqueId.Text.Trim


                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If
                    End If

                End If
            End If

            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.InsertLookups(g_dataset_dsLookups)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = True

            Me.TextBoxGroup.ReadOnly = True
            Me.TextBoxActive.ReadOnly = True
            Me.TextBoxCodeValue.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxEditable.ReadOnly = True
            Me.TextBoxEffDate.Enabled = False

            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxSubGroup1.ReadOnly = True
            Me.TextBoxSubGroup2.ReadOnly = True
            Me.TextBoxSubGroup3.ReadOnly = True
            Me.TextBoxUniqueId.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = True

            Me.DataGridLookupMaintenance.SelectedIndex = -1
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

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect("MainWebForm.aspx")
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you SURE you want to delete this Lookups?", MessageBoxButtons.YesNo)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub


    Private Sub DataGridLookupMaintenance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridLookupMaintenance.ItemDataBound
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridLookupMaintenance.SelectedIndex And Me.DataGridLookupMaintenance.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(2).Visible = False
            e.Item.Cells(3).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(5).Visible = False
            e.Item.Cells(7).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
            e.Item.Cells(12).Visible = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub DataGridLookupMaintenance_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridLookupMaintenance.SortCommand

        Try
            DataGridLookupMaintenance.SelectedIndex = -1
            If Not viewstate("Dataset_Lookups") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsLookups = viewstate("Dataset_Lookups")
                dv = g_dataset_dsLookups.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("LookUpSort") Is Nothing Then
                    If Session("LookUpSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridLookupMaintenance.DataSource = Nothing
                Me.DataGridLookupMaintenance.DataSource = dv
                Me.DataGridLookupMaintenance.DataBind()
                Session("LookUpSort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonOK_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Session("LookUpSort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            DataGridLookupMaintenance.Enabled = False
            DataGridLookupMaintenance.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

End Class
