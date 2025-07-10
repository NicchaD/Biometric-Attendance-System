'****************************************************
'Modification History
'****************************************************
'Modified by     Date            Description
'****************************************************
'Hafiz           04Feb06         Cache-Session
'NP/PP/SR        2009.05.18      Optimizing the YMCA Screen
'Dilip    :      07-July-2009   :  Remove extra '>' from Name word.
'Neeraj Singh    12/Nov/2009    Added form name for security issue YRS 5.0-940 
'prasad Jadhav   2011-10-19     YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
'prasad Jadhav   2011-10-28     For BT-918 YRS 5.0-1383 : Changes to allow only officers in officers tab  
'Anudeep A       2013-10-30     BT2272:Unable to Modify and Add contact information in YMCA maintenance
'Shashank Patel  2014.10.13     BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru 2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'****************************************************
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
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class SelectTitle
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SelectTitle.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsTitle As DataSet
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Dim g_integer_count As Integer
    Dim g_bool_SearchFlag As Boolean
    Dim g_bool_flagTitleCancel As Boolean
    Dim path As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBoxLook As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridSelectTitle As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring 
#Region "Properties"

    '1. Define Property 
    Public Property Session(sName As String) As Object
        Get
            Return MyBase.Session(Me.UniqueSessionId + sName)
        End Get
        Set(value As Object)
            MyBase.Session(Me.UniqueSessionId + sName) = value
        End Set
    End Property

    ' 2. UniqueSession-forMultiTabs
    Public ReadOnly Property UniqueSessionId As String
        Get
            If Request.QueryString("UniqueSessionID") = Nothing Then
                Return String.Empty
            Else
                Return Request.QueryString("UniqueSessionID").ToString()
            End If

        End Get
    End Property
#End Region
    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring 

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Me.DataGridSelectTitle.DataSource = CommonModule.CreateDataSource
        'Me.DataGridSelectTitle.DataBind()
        Me.LabelLook.AssociatedControlID = Me.TextBoxLook.ID
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        'Added by prasad for YRS 5.0-1383 : Changes to allow only officers in officers tab
        path = Request("Page")

        'AA:BT2272:Commented below line because it is showing person if the titles pop-up opened in ymca officers tab and not showing any title
        'If Not Session("FundNo") Is Nothing Then
        '    Headercontrol.PageTitle = "Participant Information - Add / Update Employement - Select A Position Title"
        '    Headercontrol.FundNo = Session("FundNo").ToString().Trim()        
        'End If

        Try
            If Not Me.IsPostBack Then

                g_bool_SearchFlag = False
                Session("BoolSearchFlagTitle") = g_bool_SearchFlag
                Me.LabelNoRecord.Visible = False
                Me.DataGridSelectTitle.Visible = True
                PopulateData()
                ' populating the textboxes with the current row
                g_integer_count = Session("dataset_index_Title")
                Session("TitleLoad") = True
            Else
                g_integer_count = Session("dataset_index_Title")
                g_bool_SearchFlag = Session("BoolSearchFlagTitle")

                'If g_bool_SearchFlag = True Then
                '    SearchAndPopulateData()
                'Else
                '    PopulateData()
                'End If
                Me.LabelNoRecord.Visible = False
                Me.DataGridSelectTitle.Visible = True
                Session("TitleLoad") = True
            End If


        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Public Sub PopulateData()
        'Hafiz 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Hafiz 04Feb06 Cache-Session
        Try

            g_dataset_dsTitle = YMCARET.YmcaBusinessObject.SelectTitleBOClass.LookUpTitle()
            Session("Title") = g_dataset_dsTitle
            'Added by prasad 2011-10-19 :YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
            If path = "AddOffiecer" Then
                FilterOfficer(g_dataset_dsTitle)
            ElseIf path = "AddEmployment" Then
                g_dataset_dsTitle.Tables(0).Columns.Remove("bitOfficer")
                g_dataset_dsTitle.Tables(0).Columns.Remove("bitAllowMultiple")
                ViewState("ds_YMCASelectTitle") = g_dataset_dsTitle
            End If

            'Hafiz 04Feb06 Cache-Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("Title", g_dataset_dsTitle)
            'Session("Title") = g_dataset_dsTitle
            'Hafiz 04Feb06 Cache-Session
            If g_dataset_dsTitle.Tables.Count = 0 Then
                Me.LabelNoRecord.Visible = True
                Me.DataGridSelectTitle.Visible = False
            Else

                Me.DataGridSelectTitle.DataSource = g_dataset_dsTitle.Tables(0)
                Me.DataGridSelectTitle.DataBind()

            End If

        Catch sqlEx As SqlException
            Response.Redirect("ErrorPageForm.aspx")

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub
    Public Sub FilterOfficer(ByVal dstitle As DataSet)
        Dim datarow() As DataRow
        Dim dt_Title As DataTable
        dt_Title = dstitle.Tables(0).Clone()
        datarow = dstitle.Tables(0).Select("bitOfficer = true")
        For Each dr As DataRow In DataRow
            dt_Title.ImportRow(dr)
        Next
        dstitle.Tables().Remove("Title")

        dstitle.Tables.Add(dt_Title)
        If dstitle.Tables(0).Columns.Contains("bitOfficer") Then
            dstitle.Tables(0).Columns.Remove("bitOfficer")
        End If
        If dstitle.Tables(0).Columns.Contains("bitAllowMultiple") Then
            dstitle.Tables(0).Columns.Remove("bitAllowMultiple")
        End If
        ViewState("ds_YMCASelectTitle") = dstitle
    End Sub

    Public Sub SearchAndPopulateData()
        'Hafiz 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Hafiz 04Feb06 Cache-Session
        Try
            'Hafiz 04Feb06 Cache-Session
            'Cache = CacheFactory.GetCacheManager()
            'Hafiz 04Feb06 Cache-Session
            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            g_dataset_dsTitle = YMCARET.YmcaBusinessObject.SelectTitleBOClass.SearchTitle(Me.TextBoxLook.Text)
            'Added by prasad YRS 5.0-1383 : Changes to allow only officers in officers tab
            If path = "AddOffiecer" Then
                FilterOfficer(g_dataset_dsTitle)
            ElseIf path = "AddEmployment" Then
                g_dataset_dsTitle.Tables(0).Columns.Remove("bitOfficer")
                ViewState("ds_YMCASelectTitle") = g_dataset_dsTitle
            End If
            If g_dataset_dsTitle.Tables.Count = 0 Then
                Me.LabelNoRecord.Visible = True
            Else
                Me.DataGridSelectTitle.DataSource = g_dataset_dsTitle
                Me.DataGridSelectTitle.DataBind()
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecord.Visible = True
                'Hafiz 04Feb06 Cache-Session
                'g_dataset_dsTitle = Cache.GetData("Title")
                g_dataset_dsTitle = Session("Title")
                'Hafiz 04Feb06 Cache-Session
                g_dataset_dsTitle.Tables(0).Clear()
                Me.DataGridSelectTitle.DataSource = g_dataset_dsTitle
                Me.DataGridSelectTitle.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try
    End Sub

    Private Sub DataGridSelectTitle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridSelectTitle.SelectedIndexChanged
        Dim l_Dataset As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_StringPositionType As String
        Dim l_StringShortDesc As String

        Dim msg As String
        msg = ""

        Try
            Session("TitleCancel") = False
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If


            g_integer_count = DataGridSelectTitle.SelectedIndex '(((DataGridAccountTypes.CurrentPageIndex) * DataGridAccountTypes.PageSize) + DataGridAccountTypes.SelectedIndex)
            Session("dataset_index_Title") = g_integer_count

            g_bool_SearchFlag = Session("BoolSearchFlagTitle")
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            l_Dataset = g_dataset_dsTitle
            l_DataRow = l_Dataset.Tables(0).Rows.Item(g_integer_count)

            l_StringPositionType = l_DataRow.Item("chrPositionType")
            'Commented and Added by Dilip : To resolve minor issue Reported by Adusumilli : 07-July-09
            'l_StringShortDesc = l_DataRow.Item(">Name")
            l_StringShortDesc = l_DataRow.Item("Name")
            Session("TitleType") = l_StringPositionType
            Session("TitleName") = l_StringShortDesc
            Session("TitleR") = l_StringShortDesc
            'msg = msg + "<Script Language='JavaScript'>"

            'msg = msg + "window.opener.location.href=window.opener.location.href;"

            'msg = msg + "self.close();"

            'msg = msg + "</Script>"
            'Response.Write(msg)


        Catch ex As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try

        'added by ruchi for her code....
        If Session("CallFlag") = "True" Then

            Session("CallFlag") = "False"
            'Response.Redirect("AddEmployment.aspx")
            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.location.href=window.opener.location.href;"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)

        Else
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
            'Response.Redirect("AddOfficerWebForm.aspx")
        End If

        'Response.Redirect("AddOfficerWebForm.aspx")
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Try

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            g_bool_SearchFlag = True
            Session("BoolSearchFlagTitle") = g_bool_SearchFlag
            SearchAndPopulateData()

        Catch ex As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        msg = ""
        Try
            g_bool_flagTitleCancel = True
            Session("TitleCancel") = g_bool_flagTitleCancel



            'Response.Redirect("AddOfficerWebForm.aspx")

        Catch ex As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try
        If Session("CallFlag") = "True" Then

            Session("CallFlag") = "False"
            Response.Redirect("AddEmployment.aspx")
        Else
            'Response.Redirect("AddOfficerWebForm.aspx")
            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.location.href=window.opener.location.href;"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)
        End If
    End Sub

    Private Sub DataGridSelectTitle_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridSelectTitle.ItemDataBound
        Dim l_button_Select As ImageButton

        Try
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridSelectTitle.SelectedIndex And Me.DataGridSelectTitle.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If

            e.Item.Cells(1).Visible = False
           

        Catch ex As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub DataGridSelectTitle_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridSelectTitle.SortCommand
        Try
            If Not viewstate("ds_YMCASelectTitle") Is Nothing Then
                Dim dv As New DataView
                g_dataset_dsTitle = viewstate("ds_YMCASelectTitle")
                dv = g_dataset_dsTitle.Tables(0).DefaultView
                dv.Sort = e.SortExpression
                Me.DataGridSelectTitle.DataSource = Nothing
                Me.DataGridSelectTitle.DataSource = dv
                Me.DataGridSelectTitle.DataBind()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)


        End Try
    End Sub
End Class
