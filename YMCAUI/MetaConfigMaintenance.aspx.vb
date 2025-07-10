'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaConfigMaintenance.aspx.vb
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 4:23:10 PM
' Program Specification Name	:	YMCA PS 5.4.1.4
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

'
' Changed by			:	Shefali Bharti  
' Changed on			:	2-08-2005
' Change Description	:	Coding

' Changed by			:	Vartika Jain 
' Changed on			:	12-06-2005
' Change Description	:	Bug Fixing
' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
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


Public Class MetaConfigMaintenance
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MetaConfigMaintenance.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsConfigurationMaintenance As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonEditForm As System.Web.UI.WebControls.Button
    Dim g_bool_DeleteFlag As New Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxKey As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCatCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxDataType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxValue As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxKeywordId As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelKey As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCatCode As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDataType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelValue As System.Web.UI.WebControls.Label
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelKeywordId As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridMetaConfig As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAdmin As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeading As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    '''Protected WithEvents ButtonEditForm As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button

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
        'Me.DataGridMetaConfig.DataSource = CommonModule.CreateDataSource
        'Me.DataGridMetaConfig.DataBind()
        Me.LabelCatCode.AssociatedControlID = Me.TextBoxCatCode.ID
        Me.LabelDataType.AssociatedControlID = Me.TextBoxDataType.ID
        Me.LabelDesc.AssociatedControlID = Me.TextBoxDesc.ID
        Me.LabelKey.AssociatedControlID = Me.TextBoxKey.ID
        Me.LabelKeywordId.AssociatedControlID = Me.TextBoxKeywordId.ID
        Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
        Me.LabelShortDesc.AssociatedControlID = Me.TextBoxShortDesc.ID
        Me.LabelValue.AssociatedControlID = Me.TextBoxValue.ID
        Try

        
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
            '''Me.ButtonEditForm.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOk.Enabled = True
            Me.ButtonSearch.Enabled = True
            Me.TextBoxFind.ReadOnly = False

            Me.TextBoxCatCode.ReadOnly = True
            Me.TextBoxDataType.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxKey.ReadOnly = True
            Me.TextBoxKeywordId.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxValue.ReadOnly = True

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
            If Not g_dataset_dsConfigurationMaintenance Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsConfigurationMaintenance.Tables(0).Rows(g_integer_count)
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.MetaConfigMaintenance.InsertConfigurationMaintenance(g_dataset_dsConfigurationMaintenance)
            PopulateData()

            ' Making TextBoxes Blank
            Me.TextBoxCatCode.Text = String.Empty
            Me.TextBoxDataType.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxKey.Text = String.Empty
            Me.TextBoxKeywordId.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxValue.Text = String.Empty

            Me.ButtonDelete.Enabled = False
            ''' Me.ButtonEditForm.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub PopulateData()
        'Dim Cache As CacheManager
        Try

            g_dataset_dsConfigurationMaintenance = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.LookupConfigurationMaintenance()
            viewstate("Dataset_Configuration") = g_dataset_dsConfigurationMaintenance

            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("Configuration Maintenance", g_dataset_dsConfigurationMaintenance)
            Session("g_dataset_dsConfigurationMaintenance") = g_dataset_dsConfigurationMaintenance
            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

            If g_dataset_dsConfigurationMaintenance.Tables.Count = 0 Then
                '''Me.ButtonEditForm.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridMetaConfig.DataSource = g_dataset_dsConfigurationMaintenance.Tables(0)
                Me.DataGridMetaConfig.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsConfigurationMaintenance, Me.DataGridMetaConfig, "Category Code, Data Type, Value,Short Desc, Description, Keyword Id")

            ' Set the selecte index for keeping session RowIndex, if dataset is not empty.
            'If Not g_dataset_dsConfigurationMaintenance Is Nothing Then
            '    If g_dataset_dsConfigurationMaintenance.Tables.Count > 0 Then
            '        g_integer_count = 0
            '        Session("dataset_index") = g_integer_count
            '    Else
            '        g_integer_count = -1
            '        Session("dataset_index") = g_integer_count
            '    End If

            'Else
            '    g_integer_count = -1
            '    Session("dataset_index") = g_integer_count
            'End If

            'Me.DataGridMetaConfig.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsConfigurationMaintenance = Cache.GetData("Configuration Maintenance")
                g_dataset_dsConfigurationMaintenance = CType(Session("g_dataset_dsConfigurationMaintenance"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

                g_dataset_dsConfigurationMaintenance.Tables(0).Clear()
                Me.DataGridMetaConfig.DataSource = g_dataset_dsConfigurationMaintenance
                Me.DataGridMetaConfig.DataBind()
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
            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            g_dataset_dsConfigurationMaintenance = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.SearchConfigurationMaintenance(Me.TextBoxFind.Text)
            viewstate("Dataset_Configuration") = g_dataset_dsConfigurationMaintenance
            If g_dataset_dsConfigurationMaintenance.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridMetaConfig.CurrentPageIndex = 0
                Me.DataGridMetaConfig.DataSource = g_dataset_dsConfigurationMaintenance
                Me.DataGridMetaConfig.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsConfigurationMaintenance, Me.DataGridMetaConfig, "Category Code, Data Type, Value,Short Desc, Description, Keyword Id")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsConfigurationMaintenance = Cache.GetData("Configuration Maintenance")
                g_dataset_dsConfigurationMaintenance = CType(Session("g_dataset_dsConfigurationMaintenance"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                g_dataset_dsConfigurationMaintenance.Tables(0).Clear()
                Me.DataGridMetaConfig.DataSource = g_dataset_dsConfigurationMaintenance
                Me.DataGridMetaConfig.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try



    End Sub


    Private Sub DataGridMetaConfig_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaConfig.SelectedIndexChanged
        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If

        'Me.ButtonEditForm.Enabled = True
        g_integer_count = DataGridMetaConfig.SelectedIndex

        Me.TextBoxCatCode.ReadOnly = False
        Me.TextBoxDataType.ReadOnly = False
        Me.TextBoxDesc.ReadOnly = False
        Me.TextBoxKey.ReadOnly = True
        Me.TextBoxKeywordId.ReadOnly = True
        Me.TextBoxShortDesc.ReadOnly = False
        Me.TextBoxValue.ReadOnly = False


        Me.ButtonSave.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.TextBoxFind.ReadOnly = True
        Me.ButtonSearch.Enabled = False
        Me.ButtonAdd.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.ButtonOk.Enabled = False
        ''Me.ButtonEditForm.Enabled = False

        g_bool_AddFlag = False
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_EditFlag = True
        Session("BoolEditFlag") = g_bool_EditFlag

        g_bool_DeleteFlag = False
        Session("BoolDeleteFlag") = g_bool_DeleteFlag

        Session("dataset_index") = g_integer_count

        If Me.PopulateDataIntoControls(g_integer_count) = True Then
            '' Me.ButtonEditForm.Enabled = True
            Me.ButtonDelete.Enabled = True
        Else
            ''Me.ButtonEditForm.Enabled = True
            Me.ButtonDelete.Enabled = True
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
            ' Me.TextBoxFind.Text = ""
            Me.TextBoxCatCode.Text = String.Empty
            Me.TextBoxDataType.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxKey.Text = String.Empty
            Me.TextBoxKeywordId.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxValue.Text = String.Empty

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
    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        'Dim l_Cache As CacheManager

        Try

            ' l_Cache = CacheFactory.GetCacheManager()
            l_DataSet = g_dataset_dsConfigurationMaintenance '(l_Cache.GetData("Annuity Basis Types"), DataSet)

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Configuration Maintenance")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxKey.Text = CType(l_DataRow("Key"), String).Trim
                            If l_DataRow(" Category Code").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxCatCode.Text = String.Empty
                            Else
                                Me.TextBoxCatCode.Text = CType(l_DataRow(" Category Code"), String).Trim
                            End If

                            If l_DataRow("Value").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxValue.Text = String.Empty
                            Else
                                Me.TextBoxValue.Text = CType(l_DataRow("Value"), String).Trim
                            End If

                            If l_DataRow("Data Type").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDataType.Text = String.Empty
                            Else
                                Me.TextBoxDataType.Text = CType(l_DataRow("Data Type"), String).Trim
                            End If

                            If l_DataRow("Short Desc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDesc.Text = String.Empty
                            Else
                                Me.TextBoxShortDesc.Text = CType(l_DataRow("Short Desc"), String).Trim
                            End If

                            If l_DataRow("Description").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDesc.Text = String.Empty
                            Else
                                Me.TextBoxDesc.Text = CType(l_DataRow("Description"), String).Trim

                            End If

                            If l_DataRow("Keyword Id").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxKeywordId.Text = 0
                            Else
                                Me.TextBoxKeywordId.Text = CType(l_DataRow("Keyword Id"), String).Trim
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

#Region "Edit Button - Now Removed"
    '''Private Sub ButtonEditForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEditForm.Click
    '''    ' Enable / Disable the controls.
    '''    If g_bool_SearchFlag = True Then
    '''        SearchAndPopulateData()
    '''    Else
    '''        PopulateData()
    '''    End If

    '''    Me.TextBoxCatCode.ReadOnly = False
    '''    Me.TextBoxDataType.ReadOnly = False
    '''    Me.TextBoxDesc.ReadOnly = False
    '''    Me.TextBoxKey.ReadOnly = True
    '''    Me.TextBoxKeywordId.ReadOnly = True
    '''    Me.TextBoxShortDesc.ReadOnly = False
    '''    Me.TextBoxValue.ReadOnly = False


    '''    Me.ButtonSave.Enabled = True
    '''    Me.ButtonCancel.Enabled = True
    '''    Me.TextBoxFind.ReadOnly = True
    '''    Me.ButtonSearch.Enabled = False
    '''    Me.ButtonAdd.Enabled = False
    '''    Me.ButtonDelete.Enabled = False
    '''    Me.ButtonOk.Enabled = False
    '''    Me.ButtonEditForm.Enabled = False

    '''    g_bool_AddFlag = False
    '''    Session("BoolAddFlag") = g_bool_AddFlag

    '''    g_bool_EditFlag = True
    '''    Session("BoolEditFlag") = g_bool_EditFlag

    '''    g_bool_DeleteFlag = False
    '''    Session("BoolDeleteFlag") = g_bool_DeleteFlag

    '''End Sub
#End Region


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
        Me.TextBoxCatCode.ReadOnly = False
        Me.TextBoxDataType.ReadOnly = False
        Me.TextBoxDesc.ReadOnly = False
        Me.TextBoxKey.ReadOnly = False
        Me.TextBoxKeywordId.ReadOnly = True
        Me.TextBoxShortDesc.ReadOnly = False
        Me.TextBoxValue.ReadOnly = False

        Me.ButtonSave.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.TextBoxFind.ReadOnly = True
        Me.ButtonSearch.Enabled = False
        '''Me.ButtonEditForm.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.ButtonOk.Enabled = False
        Me.ButtonAdd.Enabled = False

        ' Making TextBoxes Blank
        Me.TextBoxCatCode.Text = String.Empty
        Me.TextBoxDataType.Text = String.Empty
        Me.TextBoxDesc.Text = String.Empty
        Me.TextBoxKey.Text = String.Empty
        Me.TextBoxKeywordId.Text = "Automated Generated GUID"
        Me.TextBoxShortDesc.Text = String.Empty
        Me.TextBoxValue.Text = String.Empty

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        PopulateData()

        'Enable / Disable the controls
        Me.ButtonSave.Enabled = False
        Me.ButtonCancel.Enabled = False
        Me.ButtonAdd.Enabled = True
        '''Me.ButtonEditForm.Enabled = False
        Me.ButtonDelete.Enabled = False
        Me.TextBoxFind.ReadOnly = False
        Me.ButtonSearch.Enabled = True
        Me.ButtonOk.Enabled = True

        'Making Readonly False for all TextBoxes
        Me.TextBoxCatCode.ReadOnly = True
        Me.TextBoxDataType.ReadOnly = True
        Me.TextBoxDesc.ReadOnly = True
        Me.TextBoxKey.ReadOnly = True
        Me.TextBoxKeywordId.ReadOnly = True
        Me.TextBoxShortDesc.ReadOnly = True
        Me.TextBoxValue.ReadOnly = True
        Me.DataGridMetaConfig.SelectedIndex = -1

        g_integer_count = Session("dataset_index")
        If (g_integer_count <> -1) Then
            Me.PopulateDataIntoControls(g_integer_count)
        End If
        'Me.TextBoxCatCode.Text = String.Empty
        'Me.TextBoxDataType.Text = String.Empty
        'Me.TextBoxDesc.Text = String.Empty
        'Me.TextBoxKey.Text = String.Empty
        'Me.TextBoxKeywordId.Text = String.Empty
        'Me.TextBoxShortDesc.Text = String.Empty
        'Me.TextBoxValue.Text = String.Empty

        g_bool_AddFlag = False
        Session("BoolAddFlag") = g_bool_AddFlag

        g_bool_SearchFlag = False
        Session("BoolSearchFlag") = g_bool_SearchFlag

        g_bool_DeleteFlag = False
        Session("BoolDeleteFlag") = g_bool_DeleteFlag

        g_bool_EditFlag = False
        Session("BoolEditFlag") = g_bool_EditFlag


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
                If Not g_dataset_dsConfigurationMaintenance Is Nothing Then

                    InsertRow = g_dataset_dsConfigurationMaintenance.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Key") = TextBoxKey.Text.Trim
                    If Me.TextBoxCatCode.Text.Trim.Length = 0 Then
                        InsertRow.Item(" Category Code") = String.Empty
                    Else
                        InsertRow.Item(" Category Code") = TextBoxCatCode.Text.Trim
                    End If

                    If Me.TextBoxDataType.Text.Trim.Length = 0 Then
                        InsertRow.Item("Data Type") = String.Empty
                    Else
                        InsertRow.Item("Data Type") = TextBoxDataType.Text.Trim
                    End If

                    If Me.TextBoxValue.Text.Trim.Length = 0 Then
                        InsertRow.Item("Value") = String.Empty
                    Else
                        InsertRow.Item("Value") = TextBoxValue.Text.Trim
                    End If

                    If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Short Desc") = String.Empty
                    Else
                        InsertRow.Item("Short Desc") = TextBoxShortDesc.Text.Trim
                    End If

                    If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Description") = String.Empty
                    Else
                        InsertRow.Item("Description") = TextBoxDesc.Text.Trim
                    End If



                    ' Insert the row into Table.
                    g_dataset_dsConfigurationMaintenance.Tables(0).Rows.Add(InsertRow)

                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not g_dataset_dsConfigurationMaintenance Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsConfigurationMaintenance.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        l_DataRow("Key") = TextBoxKey.Text.Trim
                        If Me.TextBoxCatCode.Text.Trim.Length = 0 Then
                            l_DataRow(" Category Code") = String.Empty
                        Else
                            l_DataRow(" Category Code") = TextBoxCatCode.Text.Trim
                        End If

                        If Me.TextBoxDataType.Text.Trim.Length = 0 Then
                            l_DataRow("Data Type") = String.Empty
                        Else
                            l_DataRow("Data Type") = TextBoxDataType.Text.Trim
                        End If

                        If Me.TextBoxValue.Text.Trim.Length = 0 Then
                            l_DataRow("Value") = String.Empty
                        Else
                            l_DataRow("Value") = TextBoxValue.Text.Trim
                        End If

                        If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Short Desc") = String.Empty
                        Else
                            l_DataRow("Short Desc") = TextBoxShortDesc.Text.Trim
                        End If

                        If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Description") = String.Empty
                        Else
                            l_DataRow("Description") = TextBoxDesc.Text.Trim
                        End If
                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If
                    End If

                End If

            End If



            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.MetaConfigMaintenance.InsertConfigurationMaintenance(g_dataset_dsConfigurationMaintenance)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            '''Me.ButtonEditForm.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = True

            Me.TextBoxCatCode.ReadOnly = False
            Me.TextBoxDataType.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False
            Me.TextBoxKey.ReadOnly = False
            Me.TextBoxKeywordId.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = False
            Me.TextBoxValue.ReadOnly = False
            Me.DataGridMetaConfig.SelectedIndex = -1

            ' Making TextBoxes Blank
            'Me.TextBoxCatCode.Text = String.Empty
            'Me.TextBoxDataType.Text = String.Empty
            'Me.TextBoxDesc.Text = String.Empty
            'Me.TextBoxKey.Text = String.Empty
            'Me.TextBoxKeywordId.Text = String.Empty
            'Me.TextBoxShortDesc.Text = String.Empty
            'Me.TextBoxValue.Text = String.Empty

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
        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you SURE you want to delete this Key?", MessageBoxButtons.YesNo)
    End Sub

    Private Sub DataGridMetaConfig_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaConfig.PageIndexChanged
        DataGridMetaConfig.CurrentPageIndex = e.NewPageIndex

        'Bind the DataGrid again with the Data Source
        'depending on wheather Search Flag is true or False

        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Session("ConfigSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub DataGridMetaConfig_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaConfig.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridMetaConfig.SelectedIndex And Me.DataGridMetaConfig.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(2).Visible = False
        e.Item.Cells(3).Visible = False
        e.Item.Cells(4).Visible = False
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
        e.Item.Cells(7).Visible = False
    End Sub

    Private Sub DataGridMetaConfig_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaConfig.SortCommand
        Try
            Me.DataGridMetaConfig.SelectedIndex = -1
            If Not viewstate("Dataset_Configuration") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsConfigurationMaintenance = viewstate("Dataset_Configuration")
                dv = g_dataset_dsConfigurationMaintenance.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("ConfigSort") Is Nothing Then
                    If Session("ConfigSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridMetaConfig.DataSource = Nothing
                Me.DataGridMetaConfig.DataSource = dv
                Me.DataGridMetaConfig.DataBind()
                Session("ConfigSort") = dv.Sort
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
            DataGridMetaConfig.Enabled = False
            DataGridMetaConfig.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
