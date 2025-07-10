'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaAnnuityTypesMain.aspx.vb
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 4:22:14 PM
' Program Specification Name	:	YMCA PS 5.4.1.3
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	Shefali Bharti  
' Changed on			:	08/12/2005
' Change Description	:	Coding
' Cache to Session       :   Ragesh 34231 02/03/06 Cache to Session
'*******************************************************************************
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


Public Class MetaAnnuityTypesMain
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MetaAnnuityTypesMain.aspx")
    'End issue id YRS 5.0-940

    Dim g_dataset_dsAnnuityTypes As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    
    Protected WithEvents LabelIcrease As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxIncreasing As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxPopup As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxLastToDie As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxSsLevelling As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxJointSurv As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxInsReserve As System.Web.UI.WebControls.CheckBox
    Dim g_bool_DeleteFlag As New Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridMetaAnnuityMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuityType As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityType As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuityBaseType As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityBaseType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAnnCatCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCodeOrder As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCodeOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEffDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffDate As YMCAUI.DateUserControl
    Protected WithEvents LabelTerminDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTerminDate As YMCAUI.DateUserControl
    Protected WithEvents LabelJointPctg As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxJointPctg As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxIcrease As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAnnCatCode As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAdmin As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeading As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents Menu1 As skmMenu.Menu

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
        'Me.DataGridMetaAnnuityMain.DataSource = CommonModule.CreateDataSource
        'Me.DataGridMetaAnnuityMain.DataBind()
        Try

        
            'If Session("LoggedUserKey") Is Nothing Then
            '    Response.Redirect("Login.aspx", False)
            'End If
        Me.LabelAnnCatCode.AssociatedControlID = Me.TextBoxAnnCatCode.ID
        Me.LabelAnnuityBaseType.AssociatedControlID = Me.TextBoxAnnuityBaseType.ID
        Me.LabelAnnuityType.AssociatedControlID = Me.TextBoxAnnuityType.ID
        Me.LabelCodeOrder.AssociatedControlID = Me.TextBoxCodeOrder.ID
        Me.LabelDesc.AssociatedControlID = Me.TextBoxDesc.ID

        Me.LabelIcrease.AssociatedControlID = Me.TextBoxIcrease.ID
        Me.LabelJointPctg.AssociatedControlID = Me.TextBoxJointPctg.ID
        Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
        Me.LabelShortDesc.AssociatedControlID = Me.TextBoxShortDesc.ID


        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
        ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
        If Not Me.IsPostBack Then
            Session("UniqueIdOfSelectedItem") = Nothing
            Session("dataset_index") = Nothing

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag
            Me.LabelNoRecordFound.Visible = False
            Me.TextBoxTerminDate.Enabled = False
            Me.TextBoxEffDate.Enabled = False

            PopulateData(False)
            ' populating the textboxes with the current row
            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
            End If
            'if Search flag is true then call 'SearchPopulate' method else call 'populate' method
        Else
            g_bool_SearchFlag = Session("BoolSearchFlag")
            g_bool_EditFlag = Session("BoolEditFlag")
            g_bool_DeleteFlag = Session("BoolDeleteFlag")

                'If g_bool_SearchFlag = True Then
                '    SearchAndPopulateData()
                'Else
                '    'PopulateData()
                'End If
            Me.LabelNoRecordFound.Visible = False
        End If
        If Request.Form("Yes") = "Yes" Then
            DeleteSub()
            'g_integer_count = 0
            Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
            SetSelectedImageOnDataGrid()
        End If

        If Request.Form("No") = "No" Then
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                'PopulateData()
            End If
        End If
        If Request.Form("Ok") = "OK" Then
            Me.ButtonCancel.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonSave.Enabled = False
            '' Me.ButtonEdit.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True

            Me.ButtonSearch.Enabled = True
            Me.TextBoxFind.ReadOnly = False

            Me.TextBoxAnnCatCode.ReadOnly = True
            Me.TextBoxAnnuityType.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = True
            Me.TextBoxAnnuityBaseType.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
         
            Me.TextBoxIcrease.ReadOnly = True
            Me.CheckBoxIncreasing.Enabled = False
            Me.CheckboxInsReserve.Enabled = False
            Me.TextBoxJointPctg.ReadOnly = True
            Me.CheckboxJointSurv.Enabled = False
            Me.CheckboxLastToDie.Enabled = False
            Me.CheckboxPopup.Enabled = False
            Me.TextBoxShortDesc.ReadOnly = True
            Me.CheckboxSsLevelling.Enabled = False

            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub DeleteSub()
        Try
            Dim l_DataSet As New DataSet
            Dim parameterSelectedAnnuityType As String
            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            If Not Session("l_dataset_dsAnnuityTypes") Is Nothing Then
                l_DataSet = CType(Session("l_dataset_dsAnnuityTypes"), DataSet)
            End If

            g_bool_DeleteFlag = True
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            Dim l_DataRow As DataRow()
            If Not l_DataSet Is Nothing Then
                If Session("UniqueIdOfSelectedItem") Is Nothing Then
                    parameterSelectedAnnuityType = l_DataSet.Tables(0).Rows(0).Item("Annuity Type")
                Else
                    parameterSelectedAnnuityType = CType(Session("UniqueIdOfSelectedItem"), String)
                End If
                l_DataRow = l_DataSet.Tables(0).Select("[Annuity Type] = '" & parameterSelectedAnnuityType & "'")
                l_DataRow(0).Delete()
            End If
            YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.InsertAnnuityTypes(l_DataSet)
            PopulateData(False)

            ' Making TextBoxes Blank
            Me.TextBoxAnnCatCode.Text = String.Empty
            Me.TextBoxAnnuityBaseType.Text = String.Empty
            Me.TextBoxCodeOrder.Text = String.Empty
            Me.TextBoxAnnuityType.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxEffDate.Text = String.Empty
            Me.TextBoxIcrease.Text = String.Empty
            Me.TextBoxJointPctg.Text = String.Empty
           
            Me.TextBoxShortDesc.Text = String.Empty

            Me.TextBoxTerminDate.Text = String.Empty

            Me.CheckBoxIncreasing.Checked = False
            Me.CheckboxInsReserve.Checked = False
            Me.CheckboxJointSurv.Checked = False
            Me.CheckboxLastToDie.Checked = False
            Me.CheckboxPopup.Checked = False
            Me.CheckboxSsLevelling.Checked = False

            Me.ButtonDelete.Enabled = False
            ''Me.ButtonEdit.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
        
    End Sub

    Public Sub PopulateData(Optional ByVal parameterNeedImageSetting As Boolean = False)
        'Dim Cache As CacheManager
        Try
            Dim l_dataset_dsAnnuityTypes As New DataSet
            l_dataset_dsAnnuityTypes = YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.LookupAnnuityTypes()
            viewstate("Dataset_AnnuityTypes") = l_dataset_dsAnnuityTypes
            Session("Original_dsAnnuityTypes") = l_dataset_dsAnnuityTypes

            Session("l_dataset_dsAnnuityTypes") = l_dataset_dsAnnuityTypes

            If Not l_dataset_dsAnnuityTypes Is Nothing Then
                Session("UniqueIdOfSelectedItem") = Convert.ToString(l_dataset_dsAnnuityTypes.Tables(0).Rows(0)("Annuity Type")).Trim()
            End If
            'g_dataset_dsAnnuityTypes = YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.LookupAnnuityTypes()
            'viewstate("Dataset_AnnuityTypes") = g_dataset_dsAnnuityTypes
            'Cache = CacheFactory.GetCacheManager()

            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache.Add("Annuity Types", g_dataset_dsAnnuityTypes)
            'Session("g_dataset_dsAnnuityTypes") = g_dataset_dsAnnuityTypes

            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            If l_dataset_dsAnnuityTypes.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridMetaAnnuityMain.DataSource = l_dataset_dsAnnuityTypes.Tables(0)
                Me.DataGridMetaAnnuityMain.DataBind()
                Me.DataGridMetaAnnuityMain.SelectedIndex = 0
                If parameterNeedImageSetting = True Then
                    Me.SetSelectedImageOnDataGrid()
                End If


            End If
            'If g_dataset_dsAnnuityTypes.Tables.Count = 0 Then
            '    ''Me.ButtonEdit.Enabled = False
            '    Me.TextBoxFind.ReadOnly = True
            '    Me.ButtonSearch.Enabled = False
            '    Me.ButtonDelete.Enabled = False
            '    Me.LabelNoRecordFound.Visible = True
            'Else

            '    Me.DataGridMetaAnnuityMain.DataSource = g_dataset_dsAnnuityTypes.Tables(0)
            '    Me.DataGridMetaAnnuityMain.DataBind()

            'End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsAnnuityTypes, Me.DataGridMetaAnnuityMain, " Annuity Base Type, Category Code, Short Description,Code Order, Eff Date, Termination Date,Joint Survivor Pctg,Increase Pctg,Increasing,Popup,Last to Die,Ssleveling,Joint Survivor,Ins Reserve")

            'Me.DataGridMetaAnnuityMain.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                ' g_dataset_dsAnnuityTypes = Cache.GetData("Annuity Types")
                'g_dataset_dsAnnuityTypes = CType(Session("g_dataset_dsAnnuityTypes"), DataSet)
                ''/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsAnnuityTypes.Tables(0).Clear()
                'Me.DataGridMetaAnnuityMain.DataSource = g_dataset_dsAnnuityTypes
                'Me.DataGridMetaAnnuityMain.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub
    Private Function SetSelectedImageOnDataGrid()
        Try
            Dim i As Integer
            For i = 0 To Me.DataGridMetaAnnuityMain.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridMetaAnnuityMain.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridMetaAnnuityMain.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If

            Next
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Private Function PopulateDataIntoControls(ByVal parameterSelectedAnnuityType As String) As Boolean

        Dim l_DataSet As New DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow()

        Try

            If Not Session("l_dataset_dsAnnuityTypes") Is Nothing Then
                l_DataSet = CType(Session("l_dataset_dsAnnuityTypes"), DataSet)
            End If


            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Annuity Types")

                If Not l_DataTable Is Nothing Then

                    If Session("UniqueIdOfSelectedItem") Is Nothing Then
                        parameterSelectedAnnuityType = l_DataTable.Rows(0).Item("Annuity Type")
                    End If
                    l_DataRow = l_DataTable.Select("[Annuity Type] = '" & parameterSelectedAnnuityType & "'")


                    If l_DataRow.Length > 0 Then

                        Me.TextBoxAnnuityType.Text = CType(l_DataRow(0)("Annuity Type"), String).Trim
                        If l_DataRow(0)(" Annuity Base Type").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxAnnuityBaseType.Text = String.Empty
                        Else
                            Me.TextBoxAnnuityBaseType.Text = CType(l_DataRow(0)(" Annuity Base Type"), String).Trim
                        End If

                        If l_DataRow(0)("Category Code").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxAnnCatCode.Text = String.Empty
                        Else
                            Me.TextBoxAnnCatCode.Text = CType(l_DataRow(0)("Category Code"), String).Trim
                        End If

                        If l_DataRow(0)("Short Description").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxShortDesc.Text = String.Empty
                        Else
                            Me.TextBoxShortDesc.Text = CType(l_DataRow(0)("Short Description"), String).Trim
                        End If

                        If l_DataRow(0)("Description").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxDesc.Text = String.Empty
                        Else
                            Me.TextBoxDesc.Text = CType(l_DataRow(0)("Description"), String).Trim
                        End If

                        If l_DataRow(0)("Code Order").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxCodeOrder.Text = String.Empty
                        Else
                            Me.TextBoxCodeOrder.Text = CType(l_DataRow(0)("Code Order"), String).Trim

                        End If

                        If l_DataRow(0)("Eff Date").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxEffDate.Text = "01/01/1900"
                        Else
                            Me.TextBoxEffDate.Text = CType(l_DataRow(0)("Eff Date"), String).Trim
                        End If

                        If l_DataRow(0)("Termination Date").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxTerminDate.Text = "01/01/1900"
                        Else
                            Me.TextBoxTerminDate.Text = CType(l_DataRow(0)("Termination Date"), String).Trim
                        End If

                        If l_DataRow(0)("Ssleveling").GetType.ToString = "System.DBNull" Then
                            Me.CheckboxSsLevelling.Checked = False
                        Else
                            If CType(l_DataRow(0)("Ssleveling"), Boolean) Then
                                Me.CheckboxSsLevelling.Checked = True
                            Else
                                Me.CheckboxSsLevelling.Checked = False
                            End If
                        End If

                        If l_DataRow(0)("Joint Survivor").GetType.ToString = "System.DBNull" Then
                            Me.CheckboxJointSurv.Checked = False
                        Else
                            If CType(l_DataRow(0)("Joint Survivor"), Boolean) Then
                                Me.CheckboxJointSurv.Checked = True
                            Else
                                Me.CheckboxJointSurv.Checked = False
                            End If
                        End If

                        If l_DataRow(0)("Ins Reserve").GetType.ToString = "System.DBNull" Then
                            Me.CheckboxInsReserve.Checked = False
                        Else
                            If CType(l_DataRow(0)("Ins Reserve"), Boolean) Then
                                Me.CheckboxInsReserve.Checked = True
                            Else
                                Me.CheckboxInsReserve.Checked = False
                            End If

                        End If

                        If l_DataRow(0)("Joint Survivor Pctg").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxJointPctg.Text = 0
                        Else
                            Me.TextBoxJointPctg.Text = CType(l_DataRow(0)("Joint Survivor Pctg"), String).Trim
                        End If

                        If l_DataRow(0)("Increase Pctg").GetType.ToString = "System.DBNull" Then
                            Me.TextBoxIcrease.Text = 0
                        Else
                            Me.TextBoxIcrease.Text = CType(l_DataRow(0)("Increase Pctg"), String).Trim
                        End If

                        If l_DataRow(0)("Increasing").GetType.ToString = "System.DBNull" Then
                            Me.CheckBoxIncreasing.Checked = False
                        Else
                            If CType(l_DataRow(0)("Increasing"), Boolean) Then
                                Me.CheckBoxIncreasing.Checked = True
                            Else
                                Me.CheckBoxIncreasing.Checked = False
                            End If
                        End If


                        If l_DataRow(0)("Popup").GetType.ToString = "System.DBNull" Then
                            Me.CheckboxPopup.Checked = False
                        Else
                            If CType(l_DataRow(0)("Popup"), Boolean) Then
                                Me.CheckboxPopup.Checked = True
                            Else
                                Me.CheckboxPopup.Checked = False
                            End If
                        End If

                        If l_DataRow(0)("Last to Die").GetType.ToString = "System.DBNull" Then
                            Me.CheckboxLastToDie.Checked = False
                        Else
                            If CType(l_DataRow(0)("Last to Die"), Boolean) Then
                                Me.CheckboxLastToDie.Checked = True
                            Else
                                Me.CheckboxLastToDie.Checked = False
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



        Catch
            Throw
        End Try



    End Function

    Private Sub DataGridMetaAnnuityMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaAnnuityMain.SelectedIndexChanged
        Try
            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            'Me.ButtonEdit.Enabled = True

            'g_integer_count = DataGridMetaAnnuityMain.SelectedIndex '(((DataGridMetaAnnuityMain.CurrentPageIndex) * DataGridMetaAnnuityMain.PageSize) + DataGridMetaAnnuityMain.SelectedIndex)
            Me.SetSelectedImageOnDataGrid()
            Session("UniqueIdOfSelectedItem") = Me.DataGridMetaAnnuityMain.SelectedItem.Cells(1).Text.Trim()
            'Shubhrata
            Dim l_integer_count As Integer = 0
            'g_integer_count = DataGridAccountTypes.SelectedIndex '(((DataGridAccountTypes.CurrentPageIndex) * DataGridAccountTypes.PageSize) + DataGridAccountTypes.SelectedIndex)
            l_integer_count = DataGridMetaAnnuityMain.SelectedIndex
            Session("dataset_index") = l_integer_count

            If Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem")) = True Then
                ''Me.ButtonEdit.Enabled = True
                Me.ButtonDelete.Enabled = True
            Else
                ''Me.ButtonEdit.Enabled = True
                Me.ButtonDelete.Enabled = True
            End If

            Me.TextBoxAnnCatCode.ReadOnly = False
            Me.TextBoxAnnuityType.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = False
            Me.TextBoxAnnuityBaseType.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False

            Me.TextBoxIcrease.ReadOnly = False
            Me.CheckBoxIncreasing.Enabled = True
            Me.CheckboxInsReserve.Enabled = True
            Me.TextBoxJointPctg.ReadOnly = False
            Me.CheckboxJointSurv.Enabled = True
            Me.CheckboxLastToDie.Enabled = True
            Me.CheckboxPopup.Enabled = True
            Me.TextBoxShortDesc.ReadOnly = False
            Me.CheckboxSsLevelling.Enabled = True

            Me.TextBoxTerminDate.Enabled = True
            Me.TextBoxEffDate.Enabled = True


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False
            'Me.ButtonEdit.Enabled = False

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            Session("BoolSearchFlag") = False

            
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub SearchAndPopulateData()

        Dim l_dataset_dsAnnuityTypes As New DataSet
        Try

            l_dataset_dsAnnuityTypes = YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.SearchAnnuityTypes(Me.TextBoxFind.Text)
            viewstate("Dataset_AnnuityTypes") = l_dataset_dsAnnuityTypes
            Session("l_dataset_dsAnnuityTypes") = l_dataset_dsAnnuityTypes

            If Not l_dataset_dsAnnuityTypes Is Nothing AndAlso l_dataset_dsAnnuityTypes.Tables.Count > 0 AndAlso l_dataset_dsAnnuityTypes.Tables(0).Rows.Count > 0 Then
                Session("UniqueIdOfSelectedItem") = Convert.ToString(l_dataset_dsAnnuityTypes.Tables(0).Rows(0)("Annuity Type")).Trim()
            End If


            If l_dataset_dsAnnuityTypes.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridMetaAnnuityMain.CurrentPageIndex = 0
                Me.DataGridMetaAnnuityMain.DataSource = l_dataset_dsAnnuityTypes
                Me.DataGridMetaAnnuityMain.DataBind()

            End If


        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then

                Me.LabelNoRecordFound.Visible = True
                viewstate("Dataset_AccountTypes") = Nothing
                l_dataset_dsAnnuityTypes = CType(Session("Original_dsAnnuityTypes"), DataSet)
                l_dataset_dsAnnuityTypes.Tables(0).Clear()
                Me.DataGridMetaAnnuityMain.DataSource = l_dataset_dsAnnuityTypes
                Me.DataGridMetaAnnuityMain.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try



    End Sub
    Private Sub DataGridMetaAnnuityMain_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaAnnuityMain.PageIndexChanged
        DataGridMetaAnnuityMain.CurrentPageIndex = e.NewPageIndex

        'Bind the DataGrid again with the Data Source
        'depending on wheather Search Flag is true or False

        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If
    End Sub
    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If
        Try

        
        g_bool_SearchFlag = True
        Session("BoolSearchFlag") = g_bool_SearchFlag


        ' Me.TextBoxFind.Text = ""
        Me.TextBoxAnnCatCode.Text = String.Empty
        Me.TextBoxAnnuityBaseType.Text = String.Empty
        Me.TextBoxCodeOrder.Text = String.Empty
        Me.TextBoxAnnuityType.Text = String.Empty
        Me.TextBoxDesc.Text = String.Empty
        Me.TextBoxEffDate.Text = String.Empty
        Me.TextBoxIcrease.Text = String.Empty
        Me.TextBoxJointPctg.Text = String.Empty
        Me.TextBoxShortDesc.Text = String.Empty
        Me.TextBoxTerminDate.Text = String.Empty
        Me.CheckBoxIncreasing.Checked = False
        Me.CheckboxInsReserve.Checked = False
        Me.CheckboxJointSurv.Checked = False
        Me.CheckboxLastToDie.Checked = False
        Me.CheckboxPopup.Checked = False
        Me.CheckboxSsLevelling.Checked = False

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

#Region "Edit Button - Now Removed"
    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    ' Enable / Disable the controls.
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    Me.TextBoxAnnCatCode.ReadOnly = False
    ''    Me.TextBoxAnnuityType.ReadOnly = True
    ''    Me.TextBoxCodeOrder.ReadOnly = False
    ''    Me.TextBoxAnnuityBaseType.ReadOnly = False
    ''    Me.TextBoxDesc.ReadOnly = False

    ''    Me.TextBoxIcrease.ReadOnly = False
    ''    Me.TextBoxIncreasing.ReadOnly = False
    ''    Me.TextBoxInsReserve.ReadOnly = False
    ''    Me.TextBoxJointPctg.ReadOnly = False
    ''    Me.TextBoxJointSurv.ReadOnly = False
    ''    Me.TextBoxLastToDie.ReadOnly = False
    ''    Me.TextBoxPopup.ReadOnly = False
    ''    Me.TextBoxShortDesc.ReadOnly = False
    ''    Me.TextBoxSsLevelling.ReadOnly = False




    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
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

#End Region

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try

            'Enable / Disable the controls
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOK.Enabled = True

            'Making Readonly True for all TextBoxes
            Me.TextBoxAnnCatCode.ReadOnly = True
            Me.TextBoxAnnuityType.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = True
            Me.TextBoxAnnuityBaseType.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True

            Me.TextBoxIcrease.ReadOnly = True
            Me.TextBoxJointPctg.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.CheckBoxIncreasing.Enabled = False
            Me.CheckboxInsReserve.Enabled = False
            Me.CheckboxJointSurv.Enabled = False
            Me.CheckboxLastToDie.Enabled = False
            Me.CheckboxPopup.Enabled = False
            Me.CheckboxSsLevelling.Enabled = False


            Me.DataGridMetaAnnuityMain.SelectedIndex = -1
            Me.TextBoxTerminDate.Enabled = False
            Me.TextBoxEffDate.Enabled = False
            ' Making TextBoxes Blank
            g_bool_AddFlag = Session("BoolAddFlag")
            'If g_bool_AddFlag = True Then
            '    Me.TextBoxAnnCatCode.Text = String.Empty
            '    Me.TextBoxAnnuityType.Text = String.Empty
            '    Me.TextBoxCodeOrder.Text = String.Empty
            '    Me.TextBoxAnnuityBaseType.Text = String.Empty
            '    Me.TextBoxDesc.Text = String.Empty
            '    Me.TextBoxEffDate.Text = String.Empty
            '    Me.TextBoxIcrease.Text = String.Empty
            '    Me.TextBoxIncreasing.Text = String.Empty
            '    Me.TextBoxInsReserve.Text = String.Empty
            '    Me.TextBoxJointPctg.Text = String.Empty
            '    Me.TextBoxJointSurv.Text = String.Empty
            '    Me.TextBoxLastToDie.Text = String.Empty
            '    Me.TextBoxPopup.Text = String.Empty
            '    Me.TextBoxShortDesc.Text = String.Empty
            '    Me.TextBoxSsLevelling.Text = String.Empty
            '    Me.TextBoxTerminDate.Text = String.Empty
            'Else
            g_integer_count = Session("dataset_index")
            Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))

            'End If



            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            If g_bool_SearchFlag = True Then
                g_bool_SearchFlag = False
                Session("BoolSearchFlag") = g_bool_SearchFlag

            End If
            Session("UniqueIdOfSelectedItem") = Nothing
            PopulateData(False)
            Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
            Me.DataGridMetaAnnuityMain.SelectedIndex = -1
            Me.SetSelectedImageOnDataGrid()

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Session("AnnTypeSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If
        Try

            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            g_bool_AddFlag = True
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag

            'Making Readonly False for all TextBoxes
            Me.TextBoxAnnCatCode.ReadOnly = False
            Me.TextBoxAnnuityType.ReadOnly = False
            Me.TextBoxCodeOrder.ReadOnly = False
            Me.TextBoxAnnuityBaseType.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False

            Me.TextBoxIcrease.ReadOnly = False

            Me.TextBoxJointPctg.ReadOnly = False

            Me.TextBoxShortDesc.ReadOnly = False

            Me.TextBoxTerminDate.Enabled = True
            Me.TextBoxEffDate.Enabled = True


            Me.CheckBoxIncreasing.Enabled = True
            Me.CheckboxInsReserve.Enabled = True
            Me.CheckboxJointSurv.Enabled = True
            Me.CheckboxLastToDie.Enabled = True
            Me.CheckboxPopup.Enabled = True
            Me.CheckboxSsLevelling.Enabled = True

            ' Making TextBoxes Blank
            Me.TextBoxAnnCatCode.Text = String.Empty
            Me.TextBoxAnnuityBaseType.Text = String.Empty
            Me.TextBoxCodeOrder.Text = String.Empty
            Me.TextBoxAnnuityType.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxEffDate.Text = String.Empty
            Me.TextBoxIcrease.Text = String.Empty

            Me.TextBoxJointPctg.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty

            Me.TextBoxTerminDate.Text = String.Empty

            Me.CheckBoxIncreasing.Checked = False
            Me.CheckboxInsReserve.Checked = False
            Me.CheckboxJointSurv.Checked = False
            Me.CheckboxLastToDie.Checked = False
            Me.CheckboxPopup.Checked = False
            Me.CheckboxSsLevelling.Checked = False


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False
        Catch ex As Exception
            Throw ex
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

            Dim l_dataset_dsAnnuityTypes As New DataSet
            If Not Session("l_dataset_dsAnnuityTypes") Is Nothing Then
                l_dataset_dsAnnuityTypes = CType(Session("l_dataset_dsAnnuityTypes"), DataSet)
            End If
            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            'If add Flag Is true then do Insertion else update
            g_bool_AddFlag = Session("BoolAddFlag")
            If g_bool_AddFlag = True Then
                If Not l_dataset_dsAnnuityTypes Is Nothing Then

                    InsertRow = l_dataset_dsAnnuityTypes.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Annuity Type") = TextBoxAnnuityType.Text.Trim
                    If Me.TextBoxAnnuityBaseType.Text.Trim.Length = 0 Then
                        InsertRow.Item(" Annuity Base Type") = String.Empty
                    Else
                        InsertRow.Item(" Annuity Base Type") = TextBoxAnnuityBaseType.Text.Trim
                    End If

                    If Me.TextBoxAnnCatCode.Text.Trim.Length = 0 Then
                        InsertRow.Item("Category Code") = String.Empty
                    Else
                        InsertRow.Item("Category Code") = TextBoxAnnCatCode.Text.Trim
                    End If

                    If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Short Description") = String.Empty
                    Else
                        InsertRow.Item("Short Description") = TextBoxShortDesc.Text.Trim
                    End If

                    If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Description") = String.Empty
                    Else
                        InsertRow.Item("Description") = TextBoxDesc.Text.Trim
                    End If

                    If Me.TextBoxCodeOrder.Text.Trim.Length = 0 Then
                        InsertRow.Item("Code Order") = 0
                    Else
                        InsertRow.Item("Code Order") = TextBoxCodeOrder.Text.Trim
                    End If

                    If Me.TextBoxEffDate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Eff Date") = "01/01/1900"
                    Else
                        InsertRow.Item("Eff Date") = TextBoxEffDate.Text.Trim
                    End If

                    If Me.TextBoxTerminDate.Text.Trim.Length = 0 Then
                        'Start of Comment by Hafiz on 18April2006
                        'InsertRow.Item("Termination Date") = "01/01/1900"
                        'End of Comment by Hafiz on 18April2006

                        'Start of Code Add by Hafiz on 18April2006
                        InsertRow.Item("Termination Date") = System.DBNull.Value
                        'End of Code Add by Hafiz on 18April2006
                    Else
                        InsertRow.Item("Termination Date") = TextBoxTerminDate.Text.Trim
                    End If

                    If Me.TextBoxJointPctg.Text.Trim.Length = 0 Then
                        InsertRow.Item("Joint Survivor Pctg") = 0
                    Else
                        InsertRow.Item("Joint Survivor Pctg") = TextBoxJointPctg.Text.Trim
                    End If

                    If Me.TextBoxIcrease.Text.Trim.Length = 0 Then
                        InsertRow.Item("Increase Pctg") = 0
                    Else
                        InsertRow.Item("Increase Pctg") = TextBoxIcrease.Text.Trim
                    End If

                    If Me.CheckBoxIncreasing.Checked = False Then
                        InsertRow.Item("Increasing") = 0
                    Else
                        InsertRow.Item("Increasing") = 1
                    End If

                    If Me.CheckboxPopup.Checked = False Then
                        InsertRow.Item("Popup") = 0
                    Else
                        InsertRow.Item("Popup") = 1
                    End If

                    If Me.CheckboxLastToDie.Checked = False Then
                        InsertRow.Item("Last to Die") = 0
                    Else
                        InsertRow.Item("Last to Die") = 1
                    End If

                    If Me.CheckboxSsLevelling.Checked = False Then
                        InsertRow.Item("Ssleveling") = 0
                    Else
                        InsertRow.Item("Ssleveling") = 1
                    End If

                    If Me.CheckboxJointSurv.Checked = False Then
                        InsertRow.Item("Joint Survivor") = 0
                    Else
                        InsertRow.Item("Joint Survivor") = 1
                    End If

                    If Me.CheckboxInsReserve.Checked = False Then
                        InsertRow.Item("Ins Reserve") = 0
                    Else
                        InsertRow.Item("Ins Reserve") = 1
                    End If


                    ' Insert the row into Table.
                    l_dataset_dsAnnuityTypes.Tables(0).Rows.Add(InsertRow)

                    g_integer_count = 0
                    'Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not l_dataset_dsAnnuityTypes Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = l_dataset_dsAnnuityTypes.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        ' Update the values in current(selected) DataRow

                        l_DataRow(" Annuity Base Type") = TextBoxAnnuityType.Text.Trim
                        If Me.TextBoxAnnuityBaseType.Text.Trim.Length = 0 Then
                            l_DataRow(" Annuity Base Type") = String.Empty
                        Else
                            l_DataRow(" Annuity Base Type") = TextBoxAnnuityBaseType.Text.Trim
                        End If

                        If Me.TextBoxAnnCatCode.Text.Trim.Length = 0 Then
                            l_DataRow("Category Code") = String.Empty
                        Else
                            l_DataRow("Category Code") = TextBoxAnnCatCode.Text.Trim
                        End If

                        If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Short Description") = String.Empty
                        Else
                            l_DataRow("Short Description") = TextBoxShortDesc.Text.Trim
                        End If

                        If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Description") = String.Empty
                        Else
                            l_DataRow("Description") = TextBoxDesc.Text.Trim
                        End If

                        If Me.TextBoxCodeOrder.Text.Trim.Length = 0 Then
                            l_DataRow("Code Order") = 0
                        Else
                            l_DataRow("Code Order") = TextBoxCodeOrder.Text.Trim
                        End If

                        If Me.TextBoxEffDate.Text.Trim.Length = 0 Then
                            l_DataRow("Eff Date") = "01/01/1900"
                        Else
                            l_DataRow("Eff Date") = TextBoxEffDate.Text.Trim
                        End If

                        If Me.TextBoxTerminDate.Text.Trim.Length = 0 Then
                            l_DataRow("Termination Date") = "01/01/1900"
                        Else
                            l_DataRow("Termination Date") = TextBoxTerminDate.Text.Trim
                        End If

                        If Me.TextBoxJointPctg.Text.Trim.Length = 0 Then
                            l_DataRow("Joint Survivor Pctg") = 0
                        Else
                            l_DataRow("Joint Survivor Pctg") = TextBoxJointPctg.Text.Trim
                        End If

                        If Me.TextBoxIcrease.Text.Trim.Length = 0 Then
                            l_DataRow("Increase Pctg") = 0
                        Else
                            l_DataRow("Increase Pctg") = TextBoxIcrease.Text.Trim
                        End If

                        If Me.CheckBoxIncreasing.Checked = False Then
                            l_DataRow("Increasing") = 0
                        Else
                            l_DataRow("Increasing") = 1
                        End If

                        If Me.CheckboxPopup.Checked = False Then
                            l_DataRow("Popup") = 0
                        Else
                            l_DataRow("Popup") = 1
                        End If

                        If Me.CheckboxLastToDie.Checked = False Then
                            l_DataRow("Last to Die") = 0
                        Else
                            l_DataRow("Last to Die") = 1
                        End If

                        If Me.CheckboxSsLevelling.Checked = False Then
                            l_DataRow("Ssleveling") = 0
                        Else
                            l_DataRow("Ssleveling") = 1
                        End If

                        If Me.CheckboxJointSurv.Checked = False Then
                            l_DataRow("Joint Survivor") = 0
                        Else
                            l_DataRow("Joint Survivor") = 1
                        End If

                        If Me.CheckboxInsReserve.Checked = False Then
                            l_DataRow("Ins Reserve") = 0
                        Else
                            l_DataRow("Ins Reserve") = 1
                        End If

                        g_integer_count = Session("dataset_index")
                        'If (g_integer_count <> -1) Then
                        '    Me.PopulateDataIntoControls(g_integer_count)
                        'End If
                        Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
                        SetSelectedImageOnDataGridOnSorting()
                    End If

                End If
            End If

            'Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.InsertAnnuityTypes(l_dataset_dsAnnuityTypes)
            If g_bool_AddFlag = True Then
                Session("UniqueIdOfSelectedItem") = Nothing
            End If
            PopulateData(False)
            Me.PopulateDataIntoControls(Session("UniqueIdOfSelectedItem"))
            SetSelectedImageOnDataGridOnSorting()
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = True

            'Making Readonly True for all TextBoxes
            Me.TextBoxAnnCatCode.ReadOnly = True
            Me.TextBoxAnnuityType.ReadOnly = True
            Me.TextBoxCodeOrder.ReadOnly = True
            Me.TextBoxAnnuityBaseType.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True

            Me.TextBoxIcrease.ReadOnly = True
            Me.CheckBoxIncreasing.Enabled = False
            Me.CheckboxInsReserve.Enabled = False
            Me.TextBoxJointPctg.ReadOnly = True
            Me.CheckboxJointSurv.Enabled = False
            Me.CheckboxLastToDie.Enabled = False
            Me.CheckboxPopup.Enabled = False
            Me.TextBoxShortDesc.ReadOnly = True
            Me.CheckboxSsLevelling.Enabled = False
            Me.DataGridMetaAnnuityMain.SelectedIndex = -1

            Me.TextBoxTerminDate.Enabled = False
            Me.TextBoxEffDate.Enabled = False

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
    Private Function SetSelectedImageOnDataGridOnSorting()
        Try
            Dim dgi As DataGridItem
            Dim l_counter As Integer
            l_counter = -1
            For Each dgi In Me.DataGridMetaAnnuityMain.Items
                l_counter = l_counter + 1
                If dgi.Cells(1).Text = Session("UniqueIdOfSelectedItem") Then
                    Exit For
                End If
            Next
            Me.DataGridMetaAnnuityMain.SelectedIndex = l_counter
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If
        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you SURE you want to delete this Annuity Type?", MessageBoxButtons.YesNo)
    End Sub

    Private Sub DataGridMetaAnnuityMain_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaAnnuityMain.ItemDataBound
        Try

            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridMetaAnnuityMain.SelectedIndex And Me.DataGridMetaAnnuityMain.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(2).Visible = False
            e.Item.Cells(3).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(6).Visible = False
            e.Item.Cells(7).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
            e.Item.Cells(12).Visible = False
            e.Item.Cells(13).Visible = False
            e.Item.Cells(14).Visible = False
            e.Item.Cells(15).Visible = False
            e.Item.Cells(16).Visible = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridMetaAnnuityMain_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaAnnuityMain.SortCommand
        Try
            Me.DataGridMetaAnnuityMain.SelectedIndex = -1
            If Not viewstate("Dataset_AnnuityTypes") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsAnnuityTypes = viewstate("Dataset_AnnuityTypes")
                dv = g_dataset_dsAnnuityTypes.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("AnnTypeSort") Is Nothing Then
                    If Session("AnnTypeSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridMetaAnnuityMain.DataSource = Nothing
                Me.DataGridMetaAnnuityMain.DataSource = dv
                Me.DataGridMetaAnnuityMain.DataBind()
                Session("AnnTypeSort") = dv.Sort
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
            DataGridMetaAnnuityMain.Enabled = False
            DataGridMetaAnnuityMain.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
