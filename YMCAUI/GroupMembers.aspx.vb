'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	GroupMembers.aspx.vb
' Author Name		:	Vartika Jain
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	10/13/2005 10:15:27 AM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session    
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya							05-Jan-2011			BT-615:YRS 5.0-969 : User Access screen changes are in aspx file added j-Query to handle scrol position commented code of selected insex change of list box and aaded to JQuery
'Anudeep A.                     08-Aug-2012         BT-1057:YRS 5.0-1630: Group members not appearing in sorted order 
'Anudeep A.                     01-Oct-2012         BT-1057:Group memeber are not sorting by alphabetical order when click on 'Add All' button. 
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

'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Public Class GroupMembers
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserGroupAdministration.aspx")
    'End issue id YRS 5.0-940
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAllUsers As System.Web.UI.WebControls.Label
    Protected WithEvents ListBoxAllUsers As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents ButtonAddUser As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddAllUsers As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveUser As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveAllUsers As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelGroupMembers As System.Web.UI.WebControls.Label
    Protected WithEvents ListboxGroupMembers As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents PlaceHolderUserMembers As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim Group_Key As String
    Dim g_DataSet_dsUsers As DataSet
    Dim g_DataSet_dsMembersForGroup As DataSet
    Dim g_DataSet_dsGroups As DataSet
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Group_Key = Session("Session_GroupId")
            g_DataSet_dsUsers = AppDomain.CurrentDomain.GetData("DataSetUsers")

            If Not Me.IsPostBack Then
                PopulateMemberData()
                PopulateAvailableUserData()
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
        
    End Sub
    Public Sub PopulateAvailableUserData()
        Try
            Dim i As Integer
            Dim l_boolean_Flag As Boolean
            Dim dt_DataTableAvailableUsers As New DataTable
            dt_DataTableAvailableUsers.Columns.Add("User Key")
            dt_DataTableAvailableUsers.Columns.Add("FullName")
            dt_DataTableAvailableUsers.AcceptChanges()
            i = 0
            For Each l_newdatarow As DataRow In g_DataSet_dsUsers.Tables(0).Rows
                l_boolean_Flag = True
                For Each l_row As DataRow In g_DataSet_dsMembersForGroup.Tables(0).Rows
                    If l_newdatarow("User Key").ToString.Trim() = l_row("User Key").ToString.Trim() Then
                        l_boolean_Flag = False
                        Exit For
                    Else
                        l_boolean_Flag = True
                    End If
                Next
                If l_boolean_Flag = True Then
                    If Not IsDBNull(g_DataSet_dsUsers.Tables(0).Rows(i).Item("User Key")) Then
                        Dim dr As DataRow = dt_DataTableAvailableUsers.NewRow()
                        dr.Item(0) = g_DataSet_dsUsers.Tables(0).Rows(i).Item("User Key").ToString()
                        dr.Item(1) = g_DataSet_dsUsers.Tables(0).Rows(i).Item("FullName").ToString()
                        dt_DataTableAvailableUsers.Rows.Add(dr)
                        dt_DataTableAvailableUsers.AcceptChanges()
                    End If
                End If
                i = i + 1
            Next
            Me.ListBoxAllUsers.DataSource = Nothing
            Me.ListBoxAllUsers.DataSource = dt_DataTableAvailableUsers
            Me.ListBoxAllUsers.DataTextField = "FullName"
            Me.ListBoxAllUsers.DataValueField = "User Key"
            Me.ListBoxAllUsers.DataBind()
            SortList(ListBoxAllUsers)
      
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub PopulateMemberData()
        Try
            Dim dt_DataTableGroupUsers As New DataTable
            dt_DataTableGroupUsers.Columns.Add("User Key")
            dt_DataTableGroupUsers.Columns.Add("FullName")
            Dim i As Integer

            g_DataSet_dsMembersForGroup = YMCARET.YmcaBusinessObject.GroupMembersBOClass.SearchMembersForGroup(Group_Key)
            If g_DataSet_dsMembersForGroup.Tables.Count = 0 Then
            Else
                i = 0
                For Each l_newDataRow As DataRow In g_DataSet_dsMembersForGroup.Tables(0).Rows
                    Dim dr As DataRow = dt_DataTableGroupUsers.NewRow()
                    dr.Item(0) = g_DataSet_dsMembersForGroup.Tables(0).Rows(i).Item("User Key").ToString()
                    dr.Item(1) = g_DataSet_dsMembersForGroup.Tables(0).Rows(i).Item("FullName").ToString()
                    dt_DataTableGroupUsers.Rows.Add(dr)
                    dt_DataTableGroupUsers.AcceptChanges()

                    i = i + 1
                Next
                Me.ListboxGroupMembers.DataSource = dt_DataTableGroupUsers
                Me.ListboxGroupMembers.DataTextField = "FullName"
                Me.ListboxGroupMembers.DataValueField = "User Key"
                Me.ListboxGroupMembers.DataBind()
                SortList(ListboxGroupMembers)
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    'commented by Anudeep on 14-sep-2012 :issue is YRS 5.0-1630 on 08-aug-2012 
    'Private Sub ButtonAddUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAddUser.Click
    '    Try
    '        Dim l_Integer_nextuser As Integer
    '        Dim l_Integer_AddUser As Integer
    '        ListBoxAllUsers.Multiple = True

    '        For l_Integer_AddUser = 0 To Me.ListBoxAllUsers.Items.Count - 1
    '            If Me.ListBoxAllUsers.Items(l_Integer_AddUser).Selected Then
    '                Me.ListboxGroupMembers.Items.Add(Me.ListBoxAllUsers.Items(l_Integer_AddUser))
    '            End If
    '        Next
    '        For l_Integer_AddUser = Me.ListBoxAllUsers.Items.Count - 1 To 0 Step -1
    '            If Me.ListBoxAllUsers.Items(l_Integer_AddUser).Selected Then
    '                Me.ListBoxAllUsers.Items.RemoveAt(l_Integer_AddUser)
    '                l_Integer_nextuser = l_Integer_AddUser
    '            End If
    '        Next

    '        'added by anudeep  for issue id YRS 5.0-1630 on 08-aug-2012 : start
    '        If Me.ListBoxAllUsers.Items.Count = l_Integer_nextuser Then
    '            Me.ListBoxAllUsers.SelectedIndex = l_Integer_nextuser - 1
    '        Else
    '            Me.ListBoxAllUsers.SelectedIndex = l_Integer_nextuser
    '        End If
    '        SortList(ListboxGroupMembers)

    '        'for issue id YRS 5.0-1630 : End
    '        Me.ListboxGroupMembers.SelectedIndex = -1
    '        Me.ButtonAddUser.Enabled = False
    '        Me.ButtonAddUser.Enabled = True
    '        Me.ButtonSave.Enabled = True
    '        Me.ButtonCancel.Enabled = True
    '        If ListBoxAllUsers.Items.Count = 0 Then
    '            Me.ButtonAddAllUsers.Enabled = False
    '            Me.ButtonAddUser.Enabled = False
    '        End If
    '        ListBoxAllUsers.SelectedIndex = ListBoxAllUsers.SelectedIndex
    '        'added by anudeep  for issue id YRS 5.0-1630 on 08-aug-2012 : end
    '        Me.ListboxGroupMembers.SelectedIndex = -1
    '        Me.ButtonAddUser.Enabled = False
    '        Me.ButtonSave.Enabled = True
    '        Me.ButtonCancel.Enabled = True
    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try
    'End Sub
    'end of comments

    'Private Sub ListBoxAllUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxAllUsers.SelectedIndexChanged
    '    Try
    '        Me.ButtonAddUser.Enabled = True
    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try
    'End Sub

    'Private Sub ListboxGroupMembers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListboxGroupMembers.SelectedIndexChanged
    '    Try
    '        Me.ButtonRemoveUser.Enabled = True
    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try
    'End Sub

    Private Sub ButtonAddAllUsers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddAllUsers.Click
        Try
            Dim l_Integer_AddAllUsers As Integer
            For l_Integer_AddAllUsers = 0 To Me.ListBoxAllUsers.Items.Count - 1
                Me.ListboxGroupMembers.Items.Add(Me.ListBoxAllUsers.Items(l_Integer_AddAllUsers))
            Next
            For l_Integer_AddAllUsers = Me.ListBoxAllUsers.Items.Count - 1 To 0 Step -1
                Me.ListBoxAllUsers.Items.RemoveAt(l_Integer_AddAllUsers)
            Next
            'Added by Anudeep on 01-10-2012 for Bt-1057
            SortList(ListboxGroupMembers)
            Me.ListboxGroupMembers.SelectedIndex = -1
            Me.ButtonAddAllUsers.Enabled = False
            Me.ButtonRemoveAllUsers.Enabled = True
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    'commented by Anudeep on 14-sep-2012 :issue is YRS 5.0-1630 on 08-aug-2012 
    'Private Sub ButtonRemoveUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveUser.Click
    '    Try
    '        Dim l_Integer_nextmember As Integer
    '        Dim l_Integer_RemoveUser As Integer
    '        For l_Integer_RemoveUser = 0 To Me.ListboxGroupMembers.Items.Count - 1
    '            If Me.ListboxGroupMembers.Items(l_Integer_RemoveUser).Selected Then
    '                Me.ListBoxAllUsers.Items.Add(Me.ListboxGroupMembers.Items(l_Integer_RemoveUser))
    '            End If
    '        Next
    '        For l_Integer_RemoveUser = Me.ListboxGroupMembers.Items.Count - 1 To 0 Step -1
    '            If Me.ListboxGroupMembers.Items(l_Integer_RemoveUser).Selected Then
    '                Me.ListboxGroupMembers.Items.RemoveAt(l_Integer_RemoveUser)
    '                l_Integer_nextmember = l_Integer_RemoveUser
    '            End If
    '        Next

    '        'added by anudeep  for issue id YRS 5.0-1630 on 08-aug-2012 : start
    '        If Me.ListboxGroupMembers.Items.Count = l_Integer_nextmember Then
    '            Me.ListboxGroupMembers.SelectedIndex = l_Integer_nextmember - 1
    '        Else
    '            Me.ListboxGroupMembers.SelectedIndex = l_Integer_nextmember
    '        End If

    '        ListboxGroupMembers.Focus()
    '        SortList(ListBoxAllUsers)
    '        ListboxGroupMembers.Multiple = False
    '        'for issue id YRS 5.0-1630 : End
    '        Me.ListBoxAllUsers.SelectedIndex = -1
    '        Me.ButtonRemoveUser.Enabled = True
    '        Me.ButtonSave.Enabled = True
    '        Me.ButtonCancel.Enabled = True
    '        If ListboxGroupMembers.Items.Count = 0 Then
    '            Me.ButtonRemoveAllUsers.Enabled = False
    '            Me.ButtonRemoveUser.Enabled = False
    '        End If 'added by anudeep  for issue id YRS 5.0-1630 on 08-aug-2012 : end

    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try
    'End Sub

    Private Sub ButtonRemoveAllUsers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveAllUsers.Click
        Try
            Dim l_Integer_RemoveAllUsers As Integer
            For l_Integer_RemoveAllUsers = 0 To Me.ListboxGroupMembers.Items.Count - 1
                Me.ListBoxAllUsers.Items.Add(Me.ListboxGroupMembers.Items(l_Integer_RemoveAllUsers))
            Next
            For l_Integer_RemoveAllUsers = Me.ListboxGroupMembers.Items.Count - 1 To 0 Step -1
                Me.ListboxGroupMembers.Items.RemoveAt(l_Integer_RemoveAllUsers)
            Next
            'Added by Anudeep on 01-10-2012 for Bt-1057
            SortList(ListBoxAllUsers)
            Me.ListBoxAllUsers.SelectedIndex = -1
            Me.ButtonRemoveAllUsers.Enabled = False
            Me.ButtonAddAllUsers.Enabled = True
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
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
                MessageBox.Show(PlaceHolderUserMembers, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_String_UserList As String
            Dim l_Integer_MemberCount As Integer
            Dim l_Integer_GroupKey As Integer
            l_String_UserList = ""
            l_String_UserList = DirectCast(Request.Form("sortedList"), String)
            'commented by Anudeep on 14-sep-2012 :issue is YRS 5.0-1630 on 08-aug-2012 
            'For l_Integer_MemberCount = 0 To Me.ListboxGroupMembers.Items.Count - 1
            '    l_String_UserList = l_String_UserList + Me.ListboxGroupMembers.Items(l_Integer_MemberCount).Value
            '    l_String_UserList = l_String_UserList + ","
            'Next
            l_Integer_GroupKey = CType(Group_Key, Integer)
            YMCARET.YmcaBusinessObject.GroupMembersBOClass.UpdateGroupMembers(l_Integer_GroupKey, l_String_UserList)
            Me.ButtonCancel.Enabled = False
            Me.ButtonSave.Enabled = False
            PopulateMemberData()
            PopulateAvailableUserData()
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
            PopulateMemberData()
            PopulateAvailableUserData()
            Me.ButtonCancel.Enabled = False
            Me.ButtonSave.Enabled = False
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
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

    Public Sub SortList(ByVal list As HtmlSelect)
        Dim ayl_arraylist_memberslist As New SortedList
        For Each i As Object In list.Items
            ayl_arraylist_memberslist.Add(i.Text, i.Value)
        Next
        list.Items.Clear()
        list.DataSource = ayl_arraylist_memberslist
        list.DataTextField = "Key"
        list.DataValueField = "Value"
        list.DataBind()
    End Sub

    
End Class
