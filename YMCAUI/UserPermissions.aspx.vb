'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session        
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Nikunj Patel                   16/Dec/2009         Added code to prevent postback and enable the save cancel buttons always
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
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
Public Class UserPermissions
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserGroupAdministration.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonFull As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonReadOnly As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonNone As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonMembers As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridUserPermissions1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridUserPermissions2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_DataSet_dsLookUpAllItems As DataSet
    Dim g_DataSet_dsLookUpPermissions As DataSet
    Dim g_Parameter_ItemIndex As String
    Dim g_Parameter_ItemType As String
    Dim iItem As Boolean = False
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        'Dim l_Parameter_ItemType As String
        Try
            ''Me.ButtonMembers.Attributes.Add("onclick", "javascript: return NewWindow('GroupMembers.aspx','GroupMembers','720','400','yes','center');")
            If Not Me.IsPostBack Then
                Session("Permissions_Sort") = Nothing
                g_Parameter_ItemType = ""
                PopulateItemData()
                Session("Session_ItemIndex") = Me.DataGridUserPermissions1.Items(0).Cells(2).Text.ToString().Trim
                Me.DataGridUserPermissions1.SelectedIndex = 0
                PopulateAccessData()
                'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
                Me.ButtonCancel.Enabled = True
                Me.ButtonSave.Enabled = True
            Else
                If Me.DataGridUserPermissions1.SelectedIndex = -1 Then
                    Me.DataGridUserPermissions1.SelectedIndex = 0
                End If
                'PopulateAccessData()
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub
    Public Sub PopulateItemData()
        Try
            Dim l_Parameter_ItemType As String
            l_Parameter_ItemType = Me.DropDownList1.SelectedItem.Text
            If l_Parameter_ItemType = "All" Then
                g_Parameter_ItemType = ""
            Else
                g_Parameter_ItemType = Me.DropDownList1.SelectedValue
            End If
            g_DataSet_dsLookUpAllItems = YMCARET.YmcaBusinessObject.UserPermissionsBOClass.LookUpAllSecuredItems(g_Parameter_ItemType)
            If Not g_DataSet_dsLookUpAllItems.Tables.Count = 0 Then
                If Not g_DataSet_dsLookUpAllItems.Tables(0).Rows.Count = 0 Then
                    viewstate("g_DataSet_dsLookUpAllItems") = g_DataSet_dsLookUpAllItems
                    Me.DataGridUserPermissions1.DataSource = g_DataSet_dsLookUpAllItems.Tables("AllItems")
                    ''viewstate("Permissions_Sort") = g_DataSet_dsLookUpAllItems
                    Me.DataGridUserPermissions1.DataBind()
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub PopulateAccessData()
        Try
            g_Parameter_ItemIndex = Me.DataGridUserPermissions1.SelectedItem.Cells(2).Text.ToString().Trim
            Session("Session_ItemIndex") = g_Parameter_ItemIndex
            g_DataSet_dsLookUpPermissions = YMCARET.YmcaBusinessObject.UserPermissionsBOClass.LookUpPermissions(g_Parameter_ItemIndex)
            If Not g_DataSet_dsLookUpPermissions.Tables.Count = 0 Then
                If Not g_DataSet_dsLookUpPermissions.Tables(0).Rows.Count = 0 Then
                    viewstate("g_DataSet_dsLookUpPermissions") = g_DataSet_dsLookUpPermissions
                    Me.DataGridUserPermissions2.DataSource = g_DataSet_dsLookUpPermissions.Tables("GroupPermissions")
                    Me.DataGridUserPermissions2.DataBind()
                End If
            End If
            Dim l_Parameter_ItemType As String
            l_Parameter_ItemType = Me.DropDownList1.SelectedItem.Text
            If l_Parameter_ItemType = "All" Then
                If Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Bar".ToUpper() Or Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Pad".ToUpper() Then
                    Me.ButtonReadOnly.Enabled = False
                Else
                    Me.ButtonReadOnly.Enabled = True
                End If
            ElseIf Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Bar".ToUpper() Or Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Pad".ToUpper() Then
                Me.ButtonReadOnly.Enabled = False
            Else
                Me.ButtonReadOnly.Enabled = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridUserPermissions1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridUserPermissions1.SelectedIndexChanged
        Try
            Dim l_button_select As ImageButton
            Dim i As Integer
            Me.DataGridUserPermissions2.Visible = True
            Me.ButtonNone.Enabled = True
            Me.ButtonFull.Enabled = True
            Me.ButtonReadOnly.Enabled = True
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            g_Parameter_ItemIndex = Me.DataGridUserPermissions1.SelectedItem.Cells(2).Text.ToString().Trim
            Session("Session_ItemIndex") = g_Parameter_ItemIndex
            If Me.DataGridUserPermissions1.SelectedItem.Cells(3).Text.ToString().ToUpper().Trim = "Menu Bar".ToUpper() Or Me.DataGridUserPermissions1.SelectedItem.Cells(3).Text.ToString().ToUpper().Trim = "Menu Pad".ToUpper() Then
                Me.ButtonReadOnly.Enabled = False
            Else
                Me.ButtonReadOnly.Enabled = True
            End If
            PopulateAccessData()
            PopulateItemData()

            For i = 0 To Me.DataGridUserPermissions1.Items.Count - 1
                If i = Me.DataGridUserPermissions1.SelectedIndex Then
                    l_button_select = Me.DataGridUserPermissions1.Items(i).FindControl("ImageButtonSelect1")
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select = Me.DataGridUserPermissions1.Items(i).FindControl("ImageButtonSelect1")
                    l_button_select.ImageUrl = "images\select.gif"
                End If
            Next
            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonSave.Enabled = False
            'Me.ButtonCancel.Enabled = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub


    Private Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Dim l_Parameter_ItemType As String
            l_Parameter_ItemType = Me.DropDownList1.SelectedItem.Text
            If l_Parameter_ItemType = "All" Then
                g_Parameter_ItemType = ""
            Else
                g_Parameter_ItemType = Me.DropDownList1.SelectedValue
            End If
            'checking for the enabling and disabling of the "read only" button
            If l_Parameter_ItemType = "All" Then
                If Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Bar".ToUpper() Or Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Pad".ToUpper() Then
                    Me.ButtonReadOnly.Enabled = False
                End If
            ElseIf Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Bar".ToUpper() Or Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim = "Menu Pad".ToUpper() Then
                Me.ButtonReadOnly.Enabled = False
            Else
                Me.ButtonReadOnly.Enabled = True
            End If
            g_Parameter_ItemIndex = Me.DataGridUserPermissions1.Items(0).Cells(2).Text.ToString().Trim()
            Session("Session_ItemIndex") = g_Parameter_ItemIndex
            PopulateItemData()
            Me.DataGridUserPermissions1.SelectedIndex = 0
            PopulateAccessData()
            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonSave.Enabled = False
            'Me.ButtonCancel.Enabled = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub


    Private Sub DataGridUserPermissions2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridUserPermissions2.ItemDataBound
        Try
            Dim i As Integer
            Dim drop As New DropDownList
            Dim dglabelAccess As Label
            Dim strItemType As String
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                drop = CType(e.Item.FindControl("drpAccess"), DropDownList)
                If Me.DataGridUserPermissions1.SelectedIndex = -1 Then
                    If Me.DropDownList1.SelectedItem.Text.ToUpper() = "All".ToUpper() Then
                        strItemType = Me.DataGridUserPermissions1.Items(0).Cells(3).Text.ToString().ToUpper().Trim
                        If strItemType.ToUpper() = "Menu Bar".ToUpper() Or strItemType.ToUpper() = "Menu Pad".ToUpper() Then
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
                    ElseIf Me.DropDownList1.SelectedItem.Text.ToUpper() = "Menu Bar".ToUpper() Or Me.DropDownList1.SelectedItem.Text.ToUpper() = "Menu Pad".ToUpper() Then
                        Dim _obj As New ListItem
                        _obj.Text = "Full"
                        _obj.Value = 1
                        drop.Items.Add(_obj)

                        _obj = New ListItem
                        _obj.Text = "None"
                        _obj.Value = 9
                        drop.Items.Add(_obj)

                    ElseIf Me.DropDownList1.SelectedItem.Text.ToUpper() = "Control".ToUpper() Or Me.DropDownList1.SelectedItem.Text.ToUpper() = "Form".ToUpper() Then
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
                Else
                    If Me.DataGridUserPermissions1.SelectedItem.Cells(3).Text.ToUpper() = "Menu Bar".ToUpper() Or Me.DataGridUserPermissions1.SelectedItem.Cells(3).Text.ToUpper() = "Menu Pad".ToUpper() Then
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

                dglabelAccess = CType(e.Item.FindControl("lblAccess"), Label)
                If dglabelAccess.Text.ToUpper().Trim() = "Full".ToUpper() Then
                    drop.SelectedValue = 1
                ElseIf dglabelAccess.Text.ToUpper().Trim() = "Read-Only".ToUpper() Then
                    drop.SelectedValue = 8
                ElseIf dglabelAccess.Text.ToUpper().Trim() = "None".ToUpper() Then
                    drop.SelectedValue = 9
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

    Private Sub DataGridUserPermissions2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridUserPermissions2.SelectedIndexChanged
        Try
            Dim l_button_select As ImageButton
            Dim i As Integer
            For i = 0 To Me.DataGridUserPermissions2.Items.Count - 1
                If i = Me.DataGridUserPermissions2.SelectedIndex Then
                    l_button_select = Me.DataGridUserPermissions2.Items(i).FindControl("ImageButtonSelect2")
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select = Me.DataGridUserPermissions2.Items(i).FindControl("ImageButtonSelect2")
                    l_button_select.ImageUrl = "images\select.gif"
                End If

            Next
            g_DataSet_dsLookUpPermissions = viewstate("g_DataSet_dsLookUpPermissions")
            Session("Session_GroupId") = g_DataSet_dsLookUpPermissions.Tables("GroupPermissions").Rows(Me.DataGridUserPermissions2.SelectedIndex)("Group Key").ToString()
            Me.ButtonMembers.Enabled = True
            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonSave.Enabled = False
            'Me.ButtonCancel.Enabled = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub

    Private Sub ButtonMembers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMembers.Click
        Try
            'Added by neeraj on 25-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonSave.Enabled = False
            'Me.ButtonCancel.Enabled = False

            Dim popupScript As String = "<script language='javascript'>" & _
                             "window.open('GroupMembers.aspx','GrpMembers_PopUp', " & _
                             "'width=720, height=400, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                             "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript4")) Then
                Page.RegisterStartupScript("PopupScript4", popupScript)
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Session("Permissions_Sort") = Nothing
            Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                               "window.close()" & _
                                                               "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
                Page.RegisterStartupScript("CloseWindow5", closeWindow5)
            End If
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
            'Added by neeraj on 25-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim drop As DropDownList
            Dim l_string_AccessList As String
            Dim l_string_GroupList As String
            Dim l_integer_AccessItemsCount As Integer
            Dim l_integer_ItemCode As Integer
            g_Parameter_ItemIndex = Session("Session_ItemIndex")
            l_string_GroupList = ""
            l_string_AccessList = ""

            l_integer_ItemCode = CType(g_Parameter_ItemIndex, Integer)
            g_DataSet_dsLookUpPermissions = ViewState("g_DataSet_dsLookUpPermissions")
            If Not g_DataSet_dsLookUpPermissions.Tables.Count = 0 Then
                If Not g_DataSet_dsLookUpPermissions.Tables(0).Rows.Count = 0 Then
                    For l_integer_AccessItemsCount = 0 To Me.DataGridUserPermissions2.Items.Count - 1
                        drop = CType(Me.DataGridUserPermissions2.Items(l_integer_AccessItemsCount).Cells(2).FindControl("drpAccess"), DropDownList)
                        l_string_AccessList = l_string_AccessList + drop.SelectedValue.ToString()
                        l_string_AccessList = l_string_AccessList + ","
                        l_string_GroupList = l_string_GroupList + g_DataSet_dsLookUpPermissions.Tables("GroupPermissions").Rows(l_integer_AccessItemsCount)("Group Key").ToString().Trim()
                        l_string_GroupList = l_string_GroupList + ","
                    Next
                    YMCARET.YmcaBusinessObject.UserPermissionsBOClass.UpdateGroupPermissions(l_integer_ItemCode, l_string_GroupList, l_string_AccessList)
                End If
            End If
            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonCancel.Enabled = False
            'Me.ButtonSave.Enabled = False
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Saved", MessageBoxButtons.OK)
            Session("User_modified") = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    'NP:2009.12.16:YRS 5.0-969 - This method is not being called from the aspx page as autopostback is being set to false to avoid postbacks
    Public Sub enableButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ButtonSave.Enabled = True
            ButtonCancel.Enabled = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub

    Private Sub ButtonFull_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFull.Click
        Try
            Dim drop As DropDownList
            Dim l_integer_AccessItemsCount As Integer
            For l_integer_AccessItemsCount = 0 To Me.DataGridUserPermissions2.Items.Count - 1
                drop = CType(Me.DataGridUserPermissions2.Items(l_integer_AccessItemsCount).Cells(2).FindControl("drpAccess"), DropDownList)
                drop.SelectedValue = 1
            Next
            ButtonSave.Enabled = True
            ButtonCancel.Enabled = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub ButtonNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNone.Click
        Try
            Dim drop As DropDownList
            Dim l_integer_AccessItemsCount As Integer
            For l_integer_AccessItemsCount = 0 To Me.DataGridUserPermissions2.Items.Count - 1
                drop = CType(Me.DataGridUserPermissions2.Items(l_integer_AccessItemsCount).Cells(2).FindControl("drpAccess"), DropDownList)
                drop.SelectedValue = 9
            Next
            ButtonSave.Enabled = True
            ButtonCancel.Enabled = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub ButtonReadOnly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReadOnly.Click
        Try
            Dim drop As DropDownList
            Dim l_integer_AccessItemsCount As Integer
            For l_integer_AccessItemsCount = 0 To Me.DataGridUserPermissions2.Items.Count - 1
                drop = CType(Me.DataGridUserPermissions2.Items(l_integer_AccessItemsCount).Cells(2).FindControl("drpAccess"), DropDownList)
                drop.SelectedValue = 8
            Next
            ButtonSave.Enabled = True
            ButtonCancel.Enabled = True
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
            PopulateAccessData()
            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonCancel.Enabled = False
            'Me.ButtonSave.Enabled = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub DataGridUserPermissions1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridUserPermissions1.ItemDataBound
        Try
            e.Item.Cells(2).Visible = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub DataGridUserPermissions1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridUserPermissions1.SortCommand
        Try
            ''Dim l_ds_User_Props_sort As DataSet
            DataGridUserPermissions1.SelectedIndex = -1
            Me.DataGridUserPermissions2.DataSource = Nothing
            Me.DataGridUserPermissions2.DataBind()
            Me.DataGridUserPermissions2.Visible = False
            Me.ButtonNone.Enabled = False
            Me.ButtonFull.Enabled = False
            Me.ButtonReadOnly.Enabled = False
            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonSave.Enabled = False
            'Me.ButtonCancel.Enabled = False
            If Not viewstate("g_DataSet_dsLookUpAllItems") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_DataSet_dsLookUpAllItems = viewstate("g_DataSet_dsLookUpAllItems")
                dv = g_DataSet_dsLookUpAllItems.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Permissions_Sort") Is Nothing Then
                    If Session("Permissions_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridUserPermissions1.DataSource = Nothing
                Me.DataGridUserPermissions1.DataSource = dv
                Me.DataGridUserPermissions1.DataBind()
                Session("Permissions_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridUserPermissions2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridUserPermissions2.SortCommand
        Try
            ''Dim l_ds_grp_props_sort As DataSet
            ''DataGridUserPermissions2.SelectedIndex = -1
            If Not viewstate("g_DataSet_dsLookUpPermissions") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_DataSet_dsLookUpPermissions = viewstate("g_DataSet_dsLookUpPermissions")
                dv = g_DataSet_dsLookUpPermissions.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Permissions_Sort") Is Nothing Then
                    If Session("Permissions_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridUserPermissions2.DataSource = Nothing
                Me.DataGridUserPermissions2.DataSource = dv
                Me.DataGridUserPermissions2.DataBind()
                Session("Permissions_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
End Class
