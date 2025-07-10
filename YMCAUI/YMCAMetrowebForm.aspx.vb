'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	YMCAMetrowebForm.aspx.vb
' Author Name		:	Shefali
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 4:53:04 PM
' Program Specification Name	:	Doc 3.1.3
' Unit Test Plan Name			:	
' Description					:	This is a Metro Popup window of General Tab
' Changed by			:	Shefali Bharti
' Changed on			:	29.08.2005
' Change Description	:	Coding 
'Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'Disabling ViewState    :   Anil 01/08/2008 BG 323
'****************************************************
'Modification History
'****************************************************
'Modified by         Date           Description
'****************************************************
'NP/PP/SR           2009.05.18      Optimizing the YMCA Screen
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
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


Public Class YMCAMetrowebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("YMCAMetrowebForm.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsYMCAMetro As New DataSet
    Dim g_bool_SearchFlagMetro As New Boolean
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBoxYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridYMCAMetro As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelYMCANo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label

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
        Me.LabelYMCANo.AssociatedControlID = Me.TextBoxYMCANo.ID
        Me.LabelName.AssociatedControlID = Me.TextBoxName.ID
        Me.LabelCity.AssociatedControlID = Me.TextBoxCity.ID
        Me.LabelState.AssociatedControlID = Me.TextBoxState.ID
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
            Me.TextBoxYMCANo.Attributes.Add("onblur", "javascript:FormatYMCANo();")
            If Not Me.IsPostBack Then
                Session("MetroSort") = Nothing

                g_bool_SearchFlagMetro = False
                Session("BoolSearchFlagMetro") = g_bool_SearchFlagMetro
                Session("Metro") = True
            Else
                g_bool_SearchFlagMetro = Session("BoolSearchFlagMetro")
                If g_bool_SearchFlagMetro = True Then
                    g_bool_SearchFlagMetro = False
                    Session("BoolSearchFlagMetro") = g_bool_SearchFlagMetro
                End If
                Session("Metro") = True
            End If

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

    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            g_bool_SearchFlagMetro = True
            Session("BoolSearchFlagMetro") = g_bool_SearchFlagMetro
            If Me.TextBoxYMCANo.Text = String.Empty And Me.TextBoxName.Text = String.Empty And Me.TextBoxCity.Text = String.Empty And Me.TextBoxState.Text = String.Empty Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Please Enter a search value", MessageBoxButtons.OK)
            Else
                PopulateYMCAMetro()
            End If
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

    Public Sub PopulateYMCAMetro()
        Try
            g_dataset_dsYMCAMetro = YMCARET.YmcaBusinessObject.YMCAMetroBOClass.SearchYMCAMetro(Me.TextBoxYMCANo.Text, Me.TextBoxName.Text, Me.TextBoxCity.Text, Me.TextBoxState.Text)
            Session("YMCA Metro") = g_dataset_dsYMCAMetro
            Me.DataGridYMCAMetro.Visible = True
            Me.DataGridYMCAMetro.DataSource = g_dataset_dsYMCAMetro.Tables(0)
            Me.DataGridYMCAMetro.DataBind()
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.DataGridYMCAMetro.Visible = False
                MessageBox.Show(PlaceHolder1, "Please Confirm", "No records found.", MessageBoxButtons.OK)
            Else
                Response.Redirect("ErrorPageForm.aspx?Message=" + sqlEx.Message)
            End If
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx?Message=" + secEx.Message)
        End Try
    End Sub

    Private Sub DataGridYMCAMetro_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCAMetro.SelectedIndexChanged
        Dim l_DataSetYMCAMetro As DataSet
        Dim l_DataTableYMCAMetro As DataTable
        Dim l_DataRowYMCAMetro As DataRow
        Dim l_StringGuiUniqueiD As String
        Dim l_StringYmcaMetroId As String
        Dim l_StringYmcaMetroName As String
        Dim l_string_YMCAUniqueId As String
        Dim l_dr_YmcaMetroDataRow As DataRow()
        'Priya
        Dim objMetro As Metro
        Dim msg As String
        msg = ""

        Try
            l_string_YMCAUniqueId = DataGridYMCAMetro.SelectedItem.Cells(3).Text
            PopulateYMCAMetro()
            l_DataSetYMCAMetro = g_dataset_dsYMCAMetro
            l_dr_YmcaMetroDataRow = l_DataSetYMCAMetro.Tables(0).Select("guiUniqueId= '" & l_string_YMCAUniqueId & "'")
            l_DataRowYMCAMetro = l_dr_YmcaMetroDataRow(0)

            objMetro = New Metro
            objMetro.MetroGuiUniqueiD = l_DataRowYMCAMetro.Item("guiUniqueId")
            objMetro.YmcaMetroName = l_DataRowYMCAMetro.Item("Description")

            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "METRO"
            objPopupAction.Action = PopupResult.ActionTypes.ADD
            objPopupAction.State = objMetro
            Session("PopUpAction") = objPopupAction

            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup")
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Me.TextBoxCity.Text = String.Empty
        Me.TextBoxName.Text = String.Empty
        Me.TextBoxState.Text = String.Empty
        Me.TextBoxYMCANo.Text = String.Empty
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim objMetro As Metro
        Dim msg As String
        msg = ""

        Dim objPopupAction As PopupResult = New PopupResult
        objPopupAction.Page = "METRO"
        objPopupAction.Action = PopupResult.ActionTypes.CANCEL
        objPopupAction.State = Nothing
        Session("PopUpAction") = objPopupAction

        msg = msg + "<Script Language='JavaScript'>"
        msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)
    End Sub
    Private Sub DataGridYMCAMetro_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYMCAMetro.ItemDataBound
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridYMCAMetro.SelectedIndex And Me.DataGridYMCAMetro.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(3).Visible = False
            e.Item.Cells(4).Visible = False

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

    Private Sub DataGridYMCAMetro_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridYMCAMetro.SortCommand
        Try
            'If Not viewstate("ds_YMCAMetro") Is Nothing Then
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            'g_dataset_dsYMCAMetro = viewstate("ds_YMCAMetro")
            g_dataset_dsYMCAMetro = Session("YMCA Metro")
            dv = g_dataset_dsYMCAMetro.Tables(0).DefaultView
            dv.Sort = SortExpression
            If Not Session("MetroSort") Is Nothing Then
                If Session("MetroSort").ToString.Trim.EndsWith("ASC") Then
                    dv.Sort = SortExpression + " DESC"
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
            Else
                dv.Sort = SortExpression + " ASC"
            End If
            Me.DataGridYMCAMetro.DataSource = Nothing
            Me.DataGridYMCAMetro.DataSource = dv
            Me.DataGridYMCAMetro.DataBind()
            Session("MetroSort") = dv.Sort
            'End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)


        End Try

    End Sub
End Class
