'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCAUI
' FileName			:	FindEDIExlcusionInfo.aspx.vb
' Author Name		:	Ashutosh Patil
' Employee ID		:	36307
' Email				:	ashutosh.patil@3i-infotech.com
' Extn      		:	8568
' Creation Date		:	03-May-2007
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By                  Date                                Description
'********************************************************************************************************************************
'Ashutosh Patil               28-May-2007                         KeyPress Event for FundNo txtFundNo   
'Neeraj Singh                 12/Nov/2009                         Added form name for security issue YRS 5.0-940 
'Dinesh Kanojia               2013-03-19                          Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
'Hafiz/Anudeep         		  2014-06-20                          BT:2524 :YRS 5.0-2360 - EDI exclustion list ( This change includes master page implementation , jquery implementation)
'Shashank                     2014-03-08                          BT-2619\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
'Manthan Rajguru              2015.09.16                          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Imports System
'Imports System.Collections
'Imports System.ComponentModel
'Imports System.Data
Imports System.Data.SqlClient
'Imports System.Drawing
'Imports System.Threading
'Imports System.Reflection
'Imports System.Security
'Imports System.Web
'Imports System.Web.SessionState
'Imports System.Web.UI
'Imports System.Web.UI.WebControls
'Imports System.Web.UI.HtmlControls
'Imports System.Data.DataRow
'Imports System.Security.Permissions

'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Imports Microsoft.Practices.EnterpriseLibrary.Data
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCAObjects
Public Class FindEDIExlcusionInfo
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("EmployeeEventsWebForm.aspx")
    'End issue id YRS 5.0-940

    Dim g_dataset_dsMemberInfo As New DataSet
    Dim g_string_FormName As String
#Region " Web Form Designer Generated Code "
    'Protected WithEvents DataGridFindInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvdFindInfo As GridView
    Protected WithEvents lblNoDataFound As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnClear As System.Web.UI.WebControls.Button
    Protected WithEvents btnFind As System.Web.UI.WebControls.Button
    Protected WithEvents txtState As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblState As System.Web.UI.WebControls.Label
    Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCity As System.Web.UI.WebControls.Label
    Protected WithEvents txtFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLastName As System.Web.UI.WebControls.Label
    Protected WithEvents txtSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Protected WithEvents lblFundNo As System.Web.UI.WebControls.Label
    'Protected WithEvents dgPager As DataGridPager
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum LoadDatasetMode
        Table
        Session
    End Enum
#Region " Constant Variables "     'Constant varaibles to be used in case of the datagrid columns are required to change 
    Const m_const_int_SSNo As Integer = 1
    Const m_const_int_FirstName As Integer = 2
    Const m_const_int_MiddleName As Integer = 3
    Const m_const_int_LastName As Integer = 4
    Const m_const_int_FundNo As Integer = 5
    Const m_const_int_PerssId As Integer = 6
#End Region
    'Private Property SessionPageCount() As Int32
    '    Get
    '        If Not (Session("MemberInfoPageCount")) Is Nothing Then

    '            Return (CType(Session("MemberInfoPageCount"), Int32))
    '        Else
    '            Return 0
    '        End If
    '    End Get

    '    Set(ByVal Value As Int32)
    '        Session("MemberInfoPageCount") = Value
    '    End Set
    'End Property
    Protected Property objSortState As GridViewCustomSort
        Get
            Return Session("RollInReminderForm_objSortState")
        End Get
        Set(value As GridViewCustomSort)
            Session("RollInReminderForm_objSortState") = value
        End Set
    End Property
    Private Property SessionDataSetg_dataset_dsMemberInfo() As DataSet
        Get
            If Not (Session("g_dataset_dsMemberInfo")) Is Nothing Then

                Return (CType(Session("g_dataset_dsMemberInfo"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("g_dataset_dsMemberInfo") = Value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim Cache As CacheManager = CacheFactory.GetCacheManager()
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        'Commented By Aparna Samala -02/01/2007 - not used anymore
        ' Session("VignettePath") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsResultsForm.aspx?fundid="
        'Commented By Aparna Samala -02/01/2007 - not used anymore
        ''Me.txtCity.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.txtState.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.txtFirstName.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.txtFundNo.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.txtLastName.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.txtSSNo.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        '''Me.txtSSNo.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")

        ''to call find click
        txtSSNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtCity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtState.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtFirstName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtFundNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtLastName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.UniqueID + "').click();return false;}} else {return true}; ")
        Me.txtSSNo.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        'Added By Ashutosh Patil as on 28-May-2007
        'KeyPress Event for txtFundNo
        Me.txtFundNo.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        Dim lblPopupmodulename As New Label
        Try

            'DataGridFindInfo.PageSize = 10
            'dgPager.Grid = DataGridFindInfo
            'dgPager.PagesToDisplay = 10

            If Not IsPostBack Then
                objSortState = Nothing
                'dgPager.Visible = False
                lblPopupmodulename = Master.FindControl("lblPopupmodulename")
                If lblPopupmodulename IsNot Nothing Then
                    lblPopupmodulename.Text = "EDI Exclusion List > Find Information"
                End If
                'If Session("Page") = "Person" Then
                '    Me.txtSSNo.Text = Session("TxtSSNo")
                '    Me.txtLastName.Text = Session("TxtLastName")
                '    Me.txtFirstName.Text = Session("TxtFirstName")
                '    Me.txtCity.Text = Session("TxtCity")
                '    Me.txtState.Text = Session("TxtState")
                '    Me.txtFundNo.Text = Session("TxtFundNo")
                '    PopulateData(LoadDatasetMode.Session)
                '    Me.DataGridFindInfo.SelectedIndex = Session("GridIndex")
                '    Session("Page") = Nothing
                'End If

                g_string_FormName = Convert.ToString(Request.QueryString.Get("Name"))
                If (g_string_FormName = "EDIExclusionList") Then
                    Me.lblCity.Visible = False
                    Me.lblState.Visible = False
                    Me.txtCity.Visible = False
                    Me.txtState.Visible = False
                Else
                    Me.lblCity.Visible = False
                    Me.lblState.Visible = False
                    Me.txtCity.Visible = False
                    Me.txtState.Visible = False
                    Me.lblFundNo.Visible = False
                    Me.txtFundNo.Visible = False
                End If

            Else

            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try
    End Sub
    Function PopulateData(ByVal parameter_load As LoadDatasetMode)
        'Dim cache As CacheManager
        'Dim l_PagingOn As Boolean

        Try
            If parameter_load = LoadDatasetMode.Table Then
                ' STARTS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                'g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.txtSSNo.Text.Trim(), Me.txtFundNo.Text.Trim(), Me.txtLastName.Text.Trim(), Me.txtFirstName.Text.Trim(), Convert.ToString(Request.QueryString.Get("Name")), Me.txtCity.Text.Trim(), Me.txtState.Text.Trim())
                g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.txtSSNo.Text.Trim(), Me.txtFundNo.Text.Trim(), Me.txtLastName.Text.Trim(), Me.txtFirstName.Text.Trim(), Convert.ToString(Request.QueryString.Get("Name")), Me.txtCity.Text.Trim(), Me.txtState.Text.Trim(), String.Empty, String.Empty)
                ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                Me.SessionDataSetg_dataset_dsMemberInfo = g_dataset_dsMemberInfo

            Else
                g_dataset_dsMemberInfo = Me.SessionDataSetg_dataset_dsMemberInfo
            End If
            ''cache = CacheFactory.GetCacheManager()
            ''cache.Add("Search_Info", g_dataset_dsMemberInfo)


            If Not g_dataset_dsMemberInfo Is Nothing Then

                'l_PagingOn = g_dataset_dsMemberInfo.Tables(0).Rows.Count > 5000

                'If Me.DataGridFindInfo.CurrentPageIndex >= Me.SessionPageCount And Me.SessionPageCount <> 0 Then Exit Function
                'If Me.gvdFindInfo.PageIndex >= Me.SessionPageCount And Me.SessionPageCount <> 0 Then Exit Function

                If (g_dataset_dsMemberInfo.Tables("Persons").Rows.Count > 0) Then
                    Session("ds") = g_dataset_dsMemberInfo
                    lblNoDataFound.Visible = False
                    'Me.DataGridFindInfo.Visible = True
                    Me.gvdFindInfo.Visible = True

                    'If l_PagingOn Then
                    '    'dgPager.Visible = True
                    '    DataGridFindInfo.AllowPaging = True
                    'Else
                    '    'dgPager.Visible = False
                    '    DataGridFindInfo.AllowPaging = False
                    'End If


                    'If parameter_load = LoadDatasetMode.Table And l_PagingOn Then

                    '    DataGridFindInfo.AllowPaging = False
                    '    DataGridFindInfo.AllowPaging = True
                    '    DataGridFindInfo.CurrentPageIndex = 0
                    '    DataGridFindInfo.PageSize = 10
                    '    'dgPager.Grid = DataGridFindInfo
                    '    'dgPager.PagesToDisplay = 10
                    '    'dgPager.Visible = True
                    '    'dgPager.CurrentPage = 0
                    'Else

                    'End If
                Else
                    'dgPager.Visible = False
                End If

                'Me.DataGridFindInfo.SelectedIndex = -1
                'Me.DataGridFindInfo.DataSource = g_dataset_dsMemberInfo
                'Me.SessionPageCount = Me.DataGridFindInfo.PageCount
                'Me.DataGridFindInfo.DataBind()
                Me.gvdFindInfo.SelectedIndex = -1
                Me.gvdFindInfo.DataSource = g_dataset_dsMemberInfo
                Me.gvdFindInfo.DataBind()

                'CommonModule.HideColumnsinDataGrid(g_dataset_dsMemberInfo, Me.DataGridFindInfo, "PersID,FundIdNo,FundUniqueId")


            Else
                lblNoDataFound.Visible = True
                'Me.DataGridFindInfo.Visible = False
                Me.gvdFindInfo.Visible = False
                Me.gvdFindInfo.DataSource = Nothing
                Me.gvdFindInfo.DataBind()
                'dgPager.Visible = False

            End If

        Catch ex As SqlException
            lblNoDataFound.Visible = True
            Me.gvdFindInfo.DataSource = Nothing
            Me.gvdFindInfo.DataBind()
            'Me.DataGridFindInfo.Visible = False
            'dgPager.Visible = False

        Catch
            Throw

        End Try

    End Function
    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click

        Try
            Session("PersonInfo") = Nothing
            Session("WithDrawnmember") = Nothing

            If (txtFirstName.Text = "" And txtLastName.Text = "" And txtFundNo.Text = "" And txtSSNo.Text = "") Then
                'MessageBox.Show(PlaceHolder1, "YRS", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
                'HelperFunctions.ShowMessageToUser(Resources.EDIExclusionList.MESSAGE_EDI_SEARCH_CRITERIA, EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_SEARCH_CRITERIA)
            Else
                Call PopulateData(LoadDatasetMode.Table)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub gvdFindInfo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvdFindInfo.PageIndexChanging
        Try
            If Not Session("ds") Is Nothing Then
                Dim dv As New DataView
                g_dataset_dsMemberInfo = CType(Session("ds"), DataSet)
                dv = g_dataset_dsMemberInfo.Tables(0).DefaultView
                If Not objSortState Is Nothing Then
                    dv.Sort = objSortState.SortExpression + " " + objSortState.SortDirection
                End If
                Session("FindInfoEDIPageIndex") = e.NewPageIndex
                gvdFindInfo.PageIndex = e.NewPageIndex
                gvdFindInfo.SelectedIndex = -1
                HelperFunctions.BindGrid(gvdFindInfo, dv, True)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub gvdFindInfo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gvdFindInfo.SelectedIndexChanged
        Dim i As Integer
        Dim l_button_select As ImageButton
        Dim l_dataExistintable_datatable As DataTable
        Dim l_datatable_AddRecord As DataTable
        Dim l_dataset_AddRecord As DataSet
        Dim l_SSNo_Exists As DataRow()
        Dim l_str_whereclause As String
        Dim l_datarow_AddRecord As DataRow
        Try
            'Session("TxtSSNo") = Me.txtSSNo.Text.Trim
            'Session("TxtLastName") = Me.txtLastName.Text.Trim
            'Session("TxtFirstName") = Me.txtFirstName.Text.Trim
            'Session("TxtCity") = Me.txtCity.Text.Trim
            'Session("TxtState") = Me.txtState.Text.Trim
            'Session("TxtFundNo") = Me.txtFundNo.Text.Trim
            'Session("GridIndex") = Me.DataGridFindInfo.SelectedIndex

            While i < Me.gvdFindInfo.Rows.Count
                If i = Me.gvdFindInfo.SelectedIndex Then
                    l_button_select = New ImageButton
                    l_button_select = Me.gvdFindInfo.Rows(i).FindControl("ImageButtonSelect")
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\selected.gif"
                    End If
                Else
                    l_button_select = New ImageButton
                    l_button_select = Me.gvdFindInfo.Rows(i).FindControl("ImageButtonSelect")
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If

                End If
                i = i + 1
            End While

            If Not Session("EDIExclusionData") Is Nothing Then
                l_dataset_AddRecord = CType(Session("EDIExclusionData"), DataSet)
                l_str_whereclause = " SSNo = '" & Convert.ToString(Me.gvdFindInfo.SelectedRow.Cells(1).Text) & "'"
                l_SSNo_Exists = l_dataset_AddRecord.Tables(0).Select(l_str_whereclause)

                For Each l_DataRow As DataRow In l_SSNo_Exists
                    l_DataRow = l_SSNo_Exists(0)
                    If l_DataRow Is Nothing Then
                        Exit For
                    Else
                        'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "SSNo Already exist in EDI Exclusion list", MessageBoxButtons.Stop)
                        'HelperFunctions.ShowMessageToUser(Resources.EDIExclusionList.MESSAGE_EDI_SSN_EXISTS, EnumMessageTypes.Error)
                        HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_SSN_EXISTS)
                        Exit Sub
                    End If
                Next

            End If


            If (Convert.ToString(Request.QueryString.Get("Name")) = "EDIExclusionList") Then
                Session("EDPersId") = Me.gvdFindInfo.SelectedRow.Cells(m_const_int_PerssId).Text 'Me.DataGridFindInfo.SelectedItem.Cells(6).Text
                Session("EDSSNo") = Convert.ToString(Me.gvdFindInfo.SelectedRow.Cells(m_const_int_SSNo).Text) 'Me.DataGridFindInfo.SelectedItem.Cells(1).Text
                If Me.gvdFindInfo.SelectedRow.Cells(m_const_int_LastName).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("EDLastName") = Me.gvdFindInfo.SelectedRow.Cells(m_const_int_LastName).Text ''Me.DataGridFindInfo.SelectedItem.Cells(4).Text
                Else
                    Session("EDLastName") = Nothing
                End If
                If Me.gvdFindInfo.SelectedRow.Cells(m_const_int_MiddleName).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("EDMiddleName") = Me.gvdFindInfo.SelectedRow.Cells(m_const_int_MiddleName).Text 'Me.DataGridFindInfo.SelectedItem.Cells(3).Text
                Else
                    Session("EDMiddleName") = ""
                End If
                If Me.gvdFindInfo.SelectedRow.Cells(m_const_int_FirstName).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("EDFirstName") = Me.gvdFindInfo.SelectedRow.Cells(m_const_int_FirstName).Text 'Me.DataGridFindInfo.SelectedItem.Cells(2).Text
                Else
                    Session("EDFirstName") = ""
                End If
                If Me.gvdFindInfo.SelectedRow.Cells(m_const_int_FundNo).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("EDFundNo") = Me.gvdFindInfo.SelectedRow.Cells(m_const_int_FundNo).Text 'Me.DataGridFindInfo.SelectedItem.Cells(5).Text
                Else
                    Session("EDFundNo") = ""
                End If
                Session("FindInfoFromEDIListScreen") = True
            End If
            Dim l_string_clientScript As String = "<script language='javascript'>" & _
             "window.opener.document.forms(0).submit();" & _
                                "self.close()" & _
                               "</script>" '  "window.opener.location.href=window.opener.location.href;" & _

            'If (Not IsClientScriptBlockRegistered("clientScript")) Then
            '    RegisterClientScriptBlock("clientScript", l_string_clientScript)
            'End If
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clientScript", l_string_clientScript, False)

            Session("FindInfoSort") = Nothing
            Session("FindInfoEDIPageIndex") = Nothing
            Session("ds") = Nothing
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            Me.txtFirstName.Text = ""
            Me.txtFundNo.Text = ""
            Me.txtLastName.Text = ""
            Me.txtSSNo.Text = ""
            Me.txtCity.Text = ""
            Me.txtState.Text = ""
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("FindInfoSort") = Nothing
        Session("Page") = Nothing
        Dim l_string_clientScript As String = "<script language='javascript'>" & _
             "window.opener.document.forms(0).submit();" & _
                                "self.close()" & _
                               "</script>" '  "window.opener.location.href=window.opener.location.href;" & _

        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clientScript", l_string_clientScript, False)
        'If (Not IsClientScriptBlockRegistered("clientScript")) Then
        '    RegisterClientScriptBlock("clientScript", l_string_clientScript)
        'End If
    End Sub
    Private Sub gvdFindInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvdFindInfo.RowDataBound
        Dim l_button_Select As ImageButton
        Try
            l_button_Select = e.Row.Cells(0).FindControl("ImageButtonSelect")
            If (e.Row.RowIndex = gvdFindInfo.SelectedIndex And gvdFindInfo.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(objSortState, e)
            End If
            'Select Case Request.QueryString.Get("Name")
            '    Case "EDIExclusionList"
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(m_const_int_FundNo).Visible = False
                e.Row.Cells(m_const_int_PerssId).Visible = False
            End If
            'End Select
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub gvdFindInfo_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvdFindInfo.Sorting
        Try
            If Not Session("ds") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsMemberInfo = CType(Session("ds"), DataSet)
                dv = g_dataset_dsMemberInfo.Tables(0).DefaultView
                If Session("FindInfoEDIPageIndex") IsNot Nothing Then
                    gvdFindInfo.PageIndex = Session("FindInfoEDIPageIndex")
                End If
                'dv.Sort = SortExpression
                'If Not Session("FindInfoSort") Is Nothing Then
                '    If Session("FindInfoSort").ToString.Trim.EndsWith("ASC") Then
                '        dv.Sort = SortExpression + " DESC"
                '    Else
                '        dv.Sort = SortExpression + " ASC"
                '    End If
                'Else
                '    dv.Sort = SortExpression + " ASC"
                'End If
                'Me.gvdFindInfo.DataSource = Nothing
                'Me.gvdFindInfo.DataSource = dv
                'Me.gvdFindInfo.DataBind()
                'Session("FindInfoSort") = dv.Sort
                HelperFunctions.gvSorting(objSortState, e.SortExpression, dv)
                HelperFunctions.BindGrid(gvdFindInfo, dv, True)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Private Sub dgPager_PageChanged(ByVal PgNumber As Integer)
    '    Try
    '        If Me.IsPostBack Then
    '            PopulateData(LoadDatasetMode.Session)
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    Private Function IsPendingQDRO(ByVal p_string_SSNO As String) As Boolean
        Dim l_DataTable As DataTable
        Dim l_bool_Result As Boolean
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.FindInfo.GetQDRODetails_BYSSNO(p_string_SSNO)
            If Not (l_DataTable) Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    l_bool_Result = True
                Else
                    l_bool_Result = False
                End If
            Else
                l_bool_Result = False
            End If

            l_DataTable = Nothing
            Return l_bool_Result
        Catch
            Throw
        End Try
    End Function
End Class
