'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA YRS
' FileName			:	UserGroupAdministration.aspx.vb
' Author Name		:	Vartika Jain
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	8/24/2005 3:13:59 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Neeraj Singh                   06/jun/2010         Enhancement for .net 4.0
'Neeraj Singh                   07/jun/2010         review changes done
'neeraj singh                   27-09-2010          removed directCast 
'Priya Jawale					29-12-2010			YRS 5.0-969 : User Access screen put added YmcaDataGrid class for datagrid to haddle scroll.
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
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class UserGroupAdministration
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserGroupAdministration.aspx")
    'End issue id YRS 5.0-940
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents RadioButtonAllUsers As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonActiveUsers As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonInactiveUsers As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ButtonReports As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridUsers As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridGroups As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonUserProperties As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUserDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUserMembership As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGroupProperties As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGroupDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGroupNew As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGroupMembers As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPermissions As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonNewUser As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderUserAdmin As System.Web.UI.WebControls.PlaceHolder
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_dataset_dsLookupUsers As New DataSet
    Dim g_dataset_dsLookupUserGroups As New DataSet
    Dim g_string_userActive As String
    Dim g_String_Exception_Message As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            g_string_userActive = ""
            Me.ButtonPermissions.Attributes.Add("onclick", "javascript: return NewWindow('UserPermissions.aspx','Permissions','750','680','yes','center');")
            ''Me.ButtonUserMembership.Attributes.Add("onclick", "javascript: return NewWindow('UserMembership.aspx','UserMembership','750','500','yes','center');")
            ''Me.ButtonGroupMembers.Attributes.Add("onclick", "javascript: return NewWindow('GroupMembers.aspx','GroupMembers','750','500','yes','center');")
            Me.ButtonReports.Attributes.Add("onclick", "javascript: return NewWindow('SecurityReports.aspx','Reports','750','500','yes','center');")

            If Not Me.IsPostBack Then
                populateData()
                Session("User_modified") = False
                Session("User_Del") = False
                Session("Group_Del") = False
                Session("blnOpen") = True
                Session("blnGrpOpen") = True
                Session("Group_Modified") = False
                Session("User_Added") = False
                Me.DataGridUsers.SelectedIndex = 0
                Me.DataGridGroups.SelectedIndex = 0
            Else
                Session("blnGrpOpen") = True
                Session("blnOpen") = True
                If Request.Form("Yes") = "Yes" And Session("User_Del") = True Then
                    DeleteUser()
                Else
                    Session("User_Del") = False
                End If
                If Request.Form("Yes") = "Yes" And Session("Group_Del") = True Then
                    DeleteGroup()
                Else
                    Session("Group_Del") = False
                End If
                If Session("User_Added") = True Then
                    populateData()
                    Session("User_Added") = False
                Else
                    Session("User_Added") = False
                End If
                If Session("Group_Modified") = True Then
                    populateData()
                    Session("Group_Modified") = False
                Else
                    Session("Group_Modified") = False
                End If
            End If
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Public Sub populateData()

        Dim l_dataset_users As DataSet

        Try
            If RadioButtonAllUsers.Checked = True Then
                g_string_userActive = ""
                g_dataset_dsLookupUsers = Nothing
                g_dataset_dsLookupUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers(g_string_userActive)
            ElseIf RadioButtonActiveUsers.Checked = True Then
                g_string_userActive = "Y"
                g_dataset_dsLookupUsers = Nothing
                g_dataset_dsLookupUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers(g_string_userActive)
            ElseIf RadioButtonInactiveUsers.Checked = True Then
                g_string_userActive = "N"
                g_dataset_dsLookupUsers = Nothing
                g_dataset_dsLookupUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers(g_string_userActive)
            End If
            Me.DataGridUsers.DataSource = Nothing
            Me.DataGridUsers.DataBind()
            Me.DataGridUsers.DataSource = g_dataset_dsLookupUsers.Tables("Users")
            viewstate("Ds_Sort_AdminUsr") = g_dataset_dsLookupUsers
            Me.DataGridUsers.DataBind()

            g_dataset_dsLookupUserGroups = Nothing
            g_dataset_dsLookupUserGroups = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUserGroups()
            'g_dataset_dsLookupUserGroups = AppDomain.CurrentDomain.GetData("DataSetUserGroups")

            If g_dataset_dsLookupUserGroups.Tables.Count = 0 Then
                'code to be written
            Else
                Me.DataGridGroups.DataSource = Nothing
                Me.DataGridGroups.DataBind()
                Me.DataGridGroups.DataSource = g_dataset_dsLookupUserGroups.Tables(0)
                viewstate("DS_Sort_AdminGrp") = g_dataset_dsLookupUserGroups
                Me.DataGridGroups.DataBind()
            End If

            Session("User_Added") = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ButtonUserProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUserProperties.Click
        Try
            If Session("blnOpen") <> False Then
                If Me.DataGridUsers.SelectedIndex = -1 Then
                    MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Please select a User Record.", MessageBoxButtons.Stop)
                Else
                    Session("Session_UserId") = Me.DataGridUsers.SelectedItem.Cells(2).Text
                    Session("Session_PageId") = "Properties"
                    Dim popupScript As String = "<script language='javascript'>" & _
                              "window.open('UserProperties.aspx','CustomPopUp', " & _
                              "'width=720, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                              "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", popupScript)
                    End If
                End If
            Else
                Session("blnOpen") = True
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridUsers.SelectedIndexChanged
        Try
            Session("Session_PageId") = "Properties"
            Session("Session_UserId") = Me.DataGridUsers.SelectedItem.Cells(2).Text
            Me.ButtonUserDelete.Enabled = True
            Me.ButtonUserProperties.Enabled = True
            Me.ButtonUserMembership.Enabled = True

            Dim i As Integer
            Dim l_button_select As ImageButton

            For i = 0 To Me.DataGridUsers.Items.Count - 1
                If i = Me.DataGridUsers.SelectedIndex Then
                    l_button_select = Me.DataGridUsers.Items(i).FindControl("ImageButtonSelectUser")
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select = Me.DataGridUsers.Items(i).FindControl("ImageButtonSelectUser")
                    l_button_select.ImageUrl = "images\select.gif"
                End If
            Next
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridGroups.SelectedIndexChanged
        Try
            Dim l_button_select As ImageButton
            Dim i As Integer
            For i = 0 To Me.DataGridGroups.Items.Count - 1
                If i = Me.DataGridGroups.SelectedIndex Then
                    l_button_select = Me.DataGridGroups.Items(i).FindControl("ImagebuttonSelectGroup")
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select = Me.DataGridGroups.Items(i).FindControl("ImagebuttonSelectGroup")
                    l_button_select.ImageUrl = "images\select.gif"
                End If

            Next


            Session("Session_GroupId") = Me.DataGridGroups.SelectedItem.Cells(1).Text
            Me.ButtonGroupProperties.Enabled = True
            Me.ButtonGroupDelete.Enabled = True
            Me.ButtonGroupMembers.Enabled = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonUserMembership_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUserMembership.Click
        Try
            If Me.DataGridUsers.SelectedIndex < 0 Then
                MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Please select a User Record.", MessageBoxButtons.Stop)
            Else
                Session("Session_UserId") = Me.DataGridUsers.SelectedItem.Cells(2).Text
                Dim popupScript As String = "<script language='javascript'>" & _
                             "window.open('UserMembership.aspx','UsrMembersPopUp', " & _
                             "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                             "</script>"

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript)
                End If
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub ButtonGroupMembers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGroupMembers.Click
        Try
            If Me.DataGridGroups.SelectedIndex < 0 Then
                MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Please Select a Group.", MessageBoxButtons.Stop)
            Else
                Session("Session_GroupId") = Me.DataGridGroups.SelectedItem.Cells(1).Text
                Dim popupScript As String = "<script language='javascript'>" & _
                                            "window.open('GroupMembers.aspx','UsrMembersPopUp', " & _
                                            "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                            "</script>"

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript)
                End If
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub ButtonGroupProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGroupProperties.Click
        Try
            If Session("blnGrpOpen") <> False Then

                If Me.DataGridGroups.SelectedIndex = -1 Then
                    MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Please Select a Record.", MessageBoxButtons.Stop)
                Else
                    Session("Session_GroupId") = Me.DataGridGroups.SelectedItem.Cells(1).Text
                    Session("Session_PageId") = "Properties"

                    Dim popupScript As String = "<script language='javascript'>" & _
                                   "window.open('GroupProperties.aspx','CustomPopUp', " & _
                                   "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                   "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
                        Page.RegisterStartupScript("PopupScript3", popupScript)
                    End If

                    ''Dim vReturnValue As String
                    ''vReturnValue = "<script language='javascript'>" & _
                    ''               "window.showModalDialog('GroupProperties.aspx','modalpopup', ' help:0,center=yes ,Resizable:1,scrollbars=yes')" & _
                    ''                "</script>"
                    ''Page.RegisterStartupScript("PopupScript3", vReturnValue)

                End If
            Else
                Session("blnGrpOpen") = True
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
        
    End Sub

    Private Sub RadioButtonActiveUsers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonActiveUsers.CheckedChanged
        Try
            g_string_userActive = "Y"
            populateData()
            'g_dataset_dsLookupUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers(g_string_userActive)
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
       
    End Sub

    Private Sub RadioButtonAllUsers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAllUsers.CheckedChanged
        Try
            g_string_userActive = ""
            populateData()
            'g_dataset_dsLookupUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers(g_string_userActive)
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
      
    End Sub

    Private Sub RadioButtonInactiveUsers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonInactiveUsers.CheckedChanged
        Try
            g_string_userActive = "N"
            populateData()
            'g_dataset_dsLookupUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers(g_string_userActive)
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonNewUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNewUser.Click
        Try
            If Session("blnOpen") <> False Then
                Session("Session_UserId") = ""
                Session("Session_PageId") = ""
                Dim popupScript As String = "<script language='javascript'>" & _
                                    "window.open('UserProperties.aspx','CustomPopUp', " & _
                                "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                "</script>"

                If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                    Page.RegisterStartupScript("PopupScript1", popupScript)
                End If
                Me.DataGridUsers.SelectedIndex = 0
            Else
                Session("blnOpen") = True
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonUserDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUserDelete.Click
        Try
            If Me.DataGridUsers.SelectedIndex < 0 Then
                MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Please select a User Record.", MessageBoxButtons.Stop)
            Else
                Session("User_Del") = True
                Session("Session_UserId") = Me.DataGridUsers.SelectedItem.Cells(2).Text
                MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Are you SURE you want to delete " + Me.DataGridUsers.SelectedItem.Cells(1).Text.ToString() + "?", MessageBoxButtons.YesNo)
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Public Sub DeleteUser()
        Try
            Dim l_integer_UserId As Integer
            'neeraj singh 27-09-2010 : removed direct cast from below line  
            l_integer_UserId = CType(Session("Session_UserId"), Integer)
            YMCARET.YmcaBusinessObject.UserPropertiesBOClass.DeleteUserDetails(l_integer_UserId)
            populateData()
            Me.DataGridUsers.SelectedIndex = 0
            Session("User_Del") = False
        Catch ex As Exception
            Throw ex
        End Try
       
    End Sub

    Private Sub ButtonGroupDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGroupDelete.Click
        Try
            If Me.DataGridGroups.SelectedIndex < 0 Then
                MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Please select a Group to Delete.", MessageBoxButtons.Stop)
            Else
                Session("Group_Del") = True
                Session("Session_GroupId") = Me.DataGridGroups.SelectedItem.Cells(1).Text
                MessageBox.Show(PlaceHolderUserAdmin, "YMCA-YRS", "Are you SURE you want to delete " + Me.DataGridGroups.SelectedItem.Cells(2).Text.ToString() + "?", MessageBoxButtons.YesNo)
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Public Sub DeleteGroup()
        Try
            Dim l_integer_GroupId As Integer
            l_integer_GroupId = CType(Session("Session_GroupId"), Integer)
            YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.DeleteGroupDetails(l_integer_GroupId)
            populateData()
            Me.DataGridGroups.SelectedIndex = 0
            Session("Group_Del") = False
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonGroupNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGroupNew.Click
        Try
            If Session("blnOpen") <> False Then
                Session("Session_PageId") = ""
                Session("Session_GroupId") = ""
                Dim popupScript As String = "<script language='javascript'>" & _
                               "window.open('GroupProperties.aspx','CustomPopUp', " & _
                               "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                               "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript4")) Then
                    Page.RegisterStartupScript("PopupScript4", popupScript)
                End If
                Me.DataGridGroups.SelectedIndex = 0
            Else
                Session("blnOpen") = True
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub



    Private Sub DataGridUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridUsers.ItemDataBound
        Try
            e.Item.Cells(2).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(6).Visible = False

            e.Item.Cells(7).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
            e.Item.Cells(12).Visible = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub DataGridGroups_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridGroups.ItemDataBound
        Try
            e.Item.Cells(1).Visible = False
            e.Item.Cells(4).Visible = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    
    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click


            Session("User_modified") = Nothing
            Session("User_Del") = Nothing
            Session("Group_Del") = Nothing
            Session("blnOpen") = Nothing
            Session("Group_Modified") = Nothing
            Session("User_Added") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
      
    End Sub

    Private Sub DataGridGroups_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridGroups.SortCommand
        Try
            Me.DataGridGroups.SelectedIndex = -1
            If Not viewstate("DS_Sort_AdminGrp") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsLookupUserGroups = viewstate("DS_Sort_AdminGrp")
                dv = g_dataset_dsLookupUserGroups.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("UsrGrpAdmin") Is Nothing Then
                    If Session("UsrGrpAdmin").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridGroups.DataSource = Nothing
                Me.DataGridGroups.DataSource = dv
                Me.DataGridGroups.DataBind()
                Session("UsrGrpAdmin") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridUsers_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridUsers.SortCommand
        Try
            Me.DataGridUsers.SelectedIndex = -1
            If Not viewstate("Ds_Sort_AdminUsr") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsLookupUsers = viewstate("Ds_Sort_AdminUsr")
                dv = g_dataset_dsLookupUsers.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("UsrGrpAdmin") Is Nothing Then
                    If Session("UsrGrpAdmin").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridUsers.DataSource = Nothing
                Me.DataGridUsers.DataSource = dv
                Me.DataGridUsers.DataBind()
                Session("UsrGrpAdmin") = dv.Sort
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
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonUserProperties.Enabled = False
            ButtonUserProperties.ToolTip = tooltip

            ButtonUserDelete.Enabled = False
            ButtonUserDelete.ToolTip = tooltip

            ButtonNewUser.Enabled = False
            ButtonNewUser.ToolTip = tooltip

            ButtonUserMembership.Enabled = False
            ButtonUserMembership.ToolTip = tooltip

            ButtonGroupProperties.Enabled = False
            ButtonGroupProperties.ToolTip = tooltip

            ButtonGroupDelete.Enabled = False
            ButtonGroupDelete.ToolTip = tooltip

            ButtonGroupNew.Enabled = False
            ButtonGroupNew.ToolTip = tooltip

            ButtonGroupMembers.Enabled = False
            ButtonGroupMembers.ToolTip = tooltip

            ButtonPermissions.Enabled = False
            ButtonPermissions.ToolTip = tooltip

            DataGridUsers.Enabled = False
            DataGridUsers.ToolTip = tooltip

            DataGridGroups.Enabled = False
            DataGridGroups.ToolTip = tooltip
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 
End Class
