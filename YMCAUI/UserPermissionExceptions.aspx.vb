'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
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
Public Class UserPermissionExceptions
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserPermissionExceptions.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonHome As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents LabelDBError As System.Web.UI.WebControls.Label
    Protected WithEvents LinkButtonErrorDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TextBoxErrorMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents LinkbuttonHideDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Labellook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridUserPermissionsExceptions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelNoRecFound As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_DataSet_dsLookUpSecItems As DataSet
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not Me.IsPostBack Then
                Session("Permission_Excep_Sort") = Nothing
                PopulateItems()
                Me.ButtonSave.Enabled = False
            Else
                Me.ButtonSave.Enabled = False
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)

        End Try
       
    End Sub
    Public Sub PopulateItems()
        Try
            Dim l_String_SearchChar As String
            If Me.TextBoxSearch.Text = "" Then
                l_String_SearchChar = ""
            Else
                l_String_SearchChar = Me.TextBoxSearch.Text.Trim()
            End If
            If Not Session("Session_UserId") = "" Then
                g_DataSet_dsLookUpSecItems = YMCARET.YmcaBusinessObject.UserPermissionExceptionsBOClass.LookUpSecItems(l_String_SearchChar, Convert.ToInt16(Session("Session_UserId")))
                If g_DataSet_dsLookUpSecItems.Tables("PerExSecItems").Rows.Count = 0 Then
                    Me.LabelNoRecFound.Visible = True
                    Me.ButtonSave.Enabled = False
                    Me.DataGridUserPermissionsExceptions.Visible = False

                Else
                    Me.DataGridUserPermissionsExceptions.Visible = True
                    Me.LabelNoRecFound.Visible = False
                    Me.ButtonSave.Enabled = True
                    Me.DataGridUserPermissionsExceptions.DataSource = g_DataSet_dsLookUpSecItems.Tables("PerExSecItems")
                    viewstate("Perms_Exceptions_Sort") = g_DataSet_dsLookUpSecItems
                    Me.DataGridUserPermissionsExceptions.DataBind()

                End If
                ''''Else
                ''''g_DataSet_dsLookUpSecItems = YMCARET.YmcaBusinessObject.UserPermissionExceptionsBOClass.LookUpAllItems(l_String_SearchChar)
                ''''If g_DataSet_dsLookUpSecItems.Tables("AllItems").Rows.Count = 0 Then
                ''''    Me.LabelNoRecFound.Visible = True
                ''''    Me.DataGridUserPermissionsExceptions.Visible = False
                ''''Else
                ''''    Me.DataGridUserPermissionsExceptions.Visible = True
                ''''    Me.LabelNoRecFound.Visible = False
                ''''    Me.DataGridUserPermissionsExceptions.DataSource = g_DataSet_dsLookUpSecItems.Tables("AllItems")
                ''''    Me.DataGridUserPermissionsExceptions.DataBind()

                ''''End If
            End If
      
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Try
            PopulateItems()
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)

        End Try
    End Sub

    Private Sub DataGridUserPermissionsExceptions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridUserPermissionsExceptions.SelectedIndexChanged
        Try
            Dim l_button_select As ImageButton
            Dim i As Integer
            Me.ButtonSave.Enabled = True
            For i = 0 To Me.DataGridUserPermissionsExceptions.Items.Count - 1
                If i = Me.DataGridUserPermissionsExceptions.SelectedIndex Then
                    l_button_select = Me.DataGridUserPermissionsExceptions.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select = Me.DataGridUserPermissionsExceptions.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\select.gif"
                End If

            Next
            'PopulateItems()
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)

        End Try

    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
            Me.ButtonSave.Enabled = False
            Dim l_integer_userId As Integer
            Dim l_integer_ItemCode As Integer
            Dim l_integer_access As Integer
            Dim drop As DropDownList
            Dim labItemCode As Label
            If Not Session("Session_UserId") = "" Then

                l_integer_userId = Convert.ToInt16(Session("Session_UserId"))

                labItemCode = CType(Me.DataGridUserPermissionsExceptions.Items(Me.DataGridUserPermissionsExceptions.SelectedIndex).Cells(1).FindControl("lblItemCode"), Label)
                l_integer_ItemCode = Convert.ToInt16(labItemCode.Text.Trim)

                drop = CType(Me.DataGridUserPermissionsExceptions.Items(Me.DataGridUserPermissionsExceptions.SelectedIndex).Cells(3).FindControl("DrpAccess"), DropDownList)
                l_integer_access = Convert.ToInt16(drop.SelectedValue)
                YMCARET.YmcaBusinessObject.UserPermissionExceptionsBOClass.InsertUserAccExceptions(l_integer_userId, l_integer_ItemCode, l_integer_access)

            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

        Session("Refresh_Flag") = "True"
        Server.Transfer("UserProperties.aspx?QueryPermission=True", False)
    End Sub

    Private Sub DataGridUserPermissionsExceptions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridUserPermissionsExceptions.ItemDataBound
        Try
            Dim i As Integer
            Dim drop As New DropDownList
            Dim dglabelAccess As Label
            Dim dgLabelType As Label
            Dim strItemType As String
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                drop = CType(e.Item.FindControl("drpAccess"), DropDownList)

                dgLabelType = CType(e.Item.FindControl("lblType"), Label)
                If dgLabelType.Text.ToUpper() = "Menu Bar".ToUpper() Or dgLabelType.Text.ToUpper() = "Menu Pad".ToUpper() Then
                    Dim _obj As New ListItem
                    _obj.Text = "Full"
                    _obj.Value = 1
                    drop.Items.Add(_obj)

                    _obj = New ListItem
                    _obj.Text = "None"
                    _obj.Value = 9
                    drop.Items.Add(_obj)
                Else
                    Dim _obj As New ListItem
                    _obj.Text = "Full"
                    _obj.Value = 1
                    drop.Items.Add(_obj)

                    _obj = New ListItem
                    _obj.Text = "Read-Only"
                    _obj.Value = 8
                    drop.Items.Add(_obj)

                    _obj = New ListItem
                    _obj.Text = "None"
                    _obj.Value = 9
                    drop.Items.Add(_obj)
                End If

            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                                    "window.close();" & _
                                                                    "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
                Page.RegisterStartupScript("CloseWindow5", closeWindow5)
            End If
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
        Server.Transfer("UserProperties.aspx?QueryPermission=True", False)
    End Sub

    Private Sub DataGridUserPermissionsExceptions_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridUserPermissionsExceptions.SortCommand
        Try
            ''Dim l_ds_grp_props_sort As DataSet
            ''DataGridUserPermissions2.SelectedIndex = -1
            If Not viewstate("Perms_Exceptions_Sort") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_DataSet_dsLookUpSecItems = viewstate("Perms_Exceptions_Sort")
                dv = g_DataSet_dsLookUpSecItems.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Permission_Excep_Sort") Is Nothing Then
                    If Session("Permission_Excep_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridUserPermissionsExceptions.DataSource = Nothing
                Me.DataGridUserPermissionsExceptions.DataSource = dv
                Me.DataGridUserPermissionsExceptions.DataBind()
                Session("Permission_Excep_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Session("Permission_Excep_Sort") = Nothing
        Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                                          "window.close();" & _
                                                                          "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        End If
    End Sub
End Class
