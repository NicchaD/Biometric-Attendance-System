' Cache-Session         : Hafiz 04Feb06
'*******************************************************************************
'Modification History
'*******************************************************************************
'Modified By        Date            Desription
'*******************************************************************************
'Mohammed Hafiz     19-Jan-2007     for removing viewstate
'NP/PP/SR       	2009.05.18      Optimizing the YMCA Screen
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Shashank Patel     2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2016.10.21      YRS-AT-3066 -  YRS bug: ABA numbers need to be 9 digits. Please display error (TrackIT 26986)
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
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class SelectBank
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SelectBank.aspx")
    'End issue id YRS 5.0-940
    Dim g_bool_AddFlagSelectBank As Boolean
    Dim g_bool_SearchFlagSelectBank As Boolean
    Dim g_dataset_dsYMCASelectBank As DataSet
    Dim g_integer_countSelectBank As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelBankNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBankName As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridSelectBank As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextBoxBankNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxBankName As System.Web.UI.WebControls.TextBox

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
        ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        If Not Me.IsPostBack Then

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Retiree Inforamtion - Add/View Bank Information"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If

            g_bool_AddFlagSelectBank = False
            Session("BoolAddFlagSelectBank") = g_bool_AddFlagSelectBank

            g_bool_SearchFlagSelectBank = False
            Session("BoolSearchFlagSelectBank") = g_bool_SearchFlagSelectBank

            Session("SelectBank") = True
        Else
            g_bool_SearchFlagSelectBank = Session("BoolSearchFlagSelectBank")
            If g_bool_SearchFlagSelectBank = True Then
                g_bool_SearchFlagSelectBank = False
                Session("BoolSearchFlagSelectBank") = g_bool_SearchFlagSelectBank

                If Request.Form("OK") = "OK" Then
                    'Do Nothing
                End If
            End If
            Session("SelectBank") = True
        End If
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        msg = ""

        Session("SelectBank Cancel") = True
        msg = msg + "<Script Language='JavaScript'>"
        'msg = msg + "window.opener.location.href=window.opener.location.href;"
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)

    End Sub

    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        g_bool_SearchFlagSelectBank = True
        Session("BoolSearchFlagSelectBank") = g_bool_SearchFlagSelectBank
        If Me.TextBoxBankName.Text = String.Empty And Me.TextBoxBankNo.Text = String.Empty Then
            MessageBox.Show(PlaceHolder1, "Please Confirm", "Please Enter a search value", MessageBoxButtons.OK)
        Else
            PopulateYMCASelectBank()
        End If
    End Sub

    Public Sub PopulateYMCASelectBank()
        'Hafiz 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Hafiz 04Feb06 Cache-Session
        Try
            g_dataset_dsYMCASelectBank = YMCARET.YmcaBusinessObject.SelectBankBOClass.SearchYMCASelectBank(Me.TextBoxBankName.Text, Me.TextBoxBankNo.Text)
            'start - commented by hafiz on 19-Jan-2007 for removing viewstate.
            'ViewState("ds_YMCASelectBank") = g_dataset_dsYMCASelectBank
            'end - commented by hafiz on 19-Jan-2007 for removing viewstate.

            'Hafiz 04Feb06 Cache-Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("YMCA Select Bank", g_dataset_dsYMCASelectBank)
            Session("YMCA Select Bank") = g_dataset_dsYMCASelectBank
            'Hafiz 04Feb06 Cache-Session
            Me.DataGridSelectBank.Visible = True
            'CommonModule.HideColumnsinDataGrid(g_dataset_dsYMCAList, Me.DataGridYMCA, "guiUniqueId")
            Me.DataGridSelectBank.DataSource = g_dataset_dsYMCASelectBank.Tables(0)
            Me.DataGridSelectBank.DataBind()
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.DataGridSelectBank.Visible = False
                MessageBox.Show(PlaceHolder1, "Please Confirm", "No records found.", MessageBoxButtons.OK)
            Else
                Response.Redirect("ErrorPageForm.aspx?FormType=Popup")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup")

        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Me.TextBoxBankName.Text = String.Empty
        Me.TextBoxBankNo.Text = String.Empty
    End Sub

    Private Sub DataGridSelectBank_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridSelectBank.SelectedIndexChanged
        Dim l_DataSetYMCASelectBank As DataSet
        Dim l_DataTableYMCASelectBank As DataTable
        Dim l_DataRowYMCASelectBank As DataRow
        Dim l_StringGuiUniqueiD As String
        Dim msg As String = ""
        Try
            'added by hafiz on 15Jun2006
            Session("blnRetireeSelectBank") = True

            Session("SelectBank Cancel") = False
            g_integer_countSelectBank = DataGridSelectBank.SelectedIndex
            Session("dataset_index_SelectBank") = g_integer_countSelectBank

            PopulateYMCASelectBank()

            l_DataSetYMCASelectBank = g_dataset_dsYMCASelectBank
            If Me.DataGridSelectBank.Visible = True Then 'MMR | 2016.10.21 | YRS-AT-3066 | Avoid error while searching bank not existing in the system
                l_DataRowYMCASelectBank = l_DataSetYMCASelectBank.Tables(0).Rows.Item(g_integer_countSelectBank)
                l_StringGuiUniqueiD = l_DataRowYMCASelectBank.Item("guiUniqueId")
                Session("SelectBank_GuiUniqueiD") = l_StringGuiUniqueiD
                Session("SelectBank_BankName") = l_DataRowYMCASelectBank.Item("Name")
                Session("SelectBank_BankABANumber") = l_DataRowYMCASelectBank.Item("Bank ABA#")


                'Code added by Atul Deo on 18th oct 2005 
                'If Session("blnRetireeSelectBank") = True Then
                Session("Sel_SelectBank_GuiUniqueiD") = l_StringGuiUniqueiD

                Session("Sel_BankName") = l_DataRowYMCASelectBank.Item("Name")
                Session("Sel_BankABANumber") = l_DataRowYMCASelectBank.Item("Bank ABA#")
                '*************Code Upadetd by ashutosh on 06-May-06


                ''******************************
                msg = msg + "<Script Language='JavaScript'>"
                'Code Edited by ashutosh for YMcaWEbForm Add session Session("blnAddBankingYMCA") on 25/05/06
                If Session("blnAddBankingRetirees") = True Then
                    msg = msg + "window.opener.location.href=window.opener.location.href;"
                Else
                    msg = msg + "window.opener.document.forms(0).submit();"

                End If
                msg = msg + "self.close()"
                msg = msg + "</Script>"
                Page.RegisterStartupScript("PopupScriptNew2", msg)
                'End If


                'msg = msg + "window.open('UpdateBankInfo.aspx','map');"
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            ''''Server.Transfer("ErrorPageForm.aspx?FormType=Popup")
        End Try

    End Sub

    Private Sub DataGridSelectBank_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridSelectBank.ItemDataBound
        Dim l_button_Select As ImageButton
        Try
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridSelectBank.SelectedIndex And Me.DataGridSelectBank.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If

            e.Item.Cells(1).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            ''''Server.Transfer("ErrorPageForm.aspx?FormType=Popup")
        End Try

    End Sub

    Private Sub DataGridSelectBank_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridSelectBank.SortCommand

        'Hafiz 04Feb06 Cache-Session
        'Dim cache As CacheManager
        'cache = CacheFactory.GetCacheManager
        'Hafiz 04Feb06 Cache-Session

        Try
            'start - commented by hafiz on 19-Jan-2007 for removing viewstate.
            'If Not viewstate("ds_YMCASelectBank") Is Nothing Then
            'end - commented by hafiz on 19-Jan-2007 for removing viewstate.
            If Not Session("YMCA Select Bank") Is Nothing Then
                Dim dv As New DataView
                'g_dataset_dsYMCASelectBank = viewstate("ds_YMCASelectbank")
                'Hafiz 04Feb06 Cache-Session
                'g_dataset_dsYMCASelectBank = Cache.GetData("YMCA Select Bank")
                g_dataset_dsYMCASelectBank = Session("YMCA Select Bank")
                'Hafiz 04Feb06 Cache-Session
                dv = g_dataset_dsYMCASelectBank.Tables(0).DefaultView
                dv.Sort = e.SortExpression
                Me.DataGridSelectBank.DataSource = Nothing
                Me.DataGridSelectBank.DataSource = dv
                Me.DataGridSelectBank.DataBind()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            ''''Server.Transfer("ErrorPageForm.aspx?FormType=Popup")
        End Try
    End Sub
End Class
