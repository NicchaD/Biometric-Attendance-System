'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	UserProperties.aspx.vb
' Author Name		:	Vartika Jain
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	9/15/2005 12:29:35 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added code for security issue YRS 5.0-940 
'Neeraj Singh                   06/jun/2010         Enhancement for .net 4.0
'Neeraj Singh                   07/jun/2010         review changes done
'Priya							05-Jan-2011			BT-615:YRS 5.0-969 : User Access screen changes are in aspx file added j-Query to handle scrol position commented code of selected insex change of list box and aaded to JQuery
'Anudeep A.                     26-Sep-2012         BT-1153:User Membership not appearing in sorted order 
'Anudeep A.                     01-oct-2012         BT-1153:Available groups are not shorting by alphabetical order when click on 'Add All' button 
'Anudeep A.                     26-nov-2012         Changed by anudeep to for checking the list if it empty and to show added data after adding a new user details
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Megha Lad                      2019.08.19          YRS-AT-4546 - YRS bug: fix typo "menber" should be "member"
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
Public Class UserProperties
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserGroupAdministration.aspx")
    'End issue id YRS 5.0-940
    Dim AddUser As Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TabStripUserProperties As Microsoft.Web.UI.WebControls.TabStrip

    Protected WithEvents ButtonAddGroup As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddAllGroups As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveGroup As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveAllGroups As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddItem As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDeleteItem As System.Web.UI.WebControls.Button
    Protected WithEvents LabelUserDetails As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAvlGroups As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMemof As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMiddleInitial As System.Web.UI.WebControls.Label
    Protected WithEvents LabelUsername As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPassword As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPhone As System.Web.UI.WebControls.Label
    Protected WithEvents LabelExtn As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLastLogin As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label

    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMiddleInitial As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxExtn As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLastLogin As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents CheckBoxActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DataGridUserProps As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ListBoxAvailableGroups As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents ListboxMemberOf As System.Web.UI.HtmlControls.HtmlSelect

    Protected WithEvents MultiPageUserProperties As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ListBox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents ListBox2 As System.Web.UI.WebControls.ListBox
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolderUserProps As System.Web.UI.WebControls.PlaceHolder
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_DataSet_dsUsers As DataSet
    Dim g_DataSet_dsGroups As DataSet
    Dim g_DataSet_dsGroupForUser As DataSet
    Dim g_DataSet_dsLookUpAllItems As DataSet
    Dim g_String_Exception_Message As String
    Dim dtTemp As New DataTable
    Dim user_key As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Me.TextboxPhone.Attributes.Add("onkeypress", "javascript:ValidateNumeric()")
            Me.TextboxFax.Attributes.Add("onkeypress", "javascript:ValidateNumeric()")
            Me.TextboxExtn.Attributes.Add("onkeypress", "javascript:ValidateNumeric()")
            Me.TextBoxFirstName.Attributes.Add("onkeypress", "javascript:ValidateAlphabet()")
            Me.TextboxLastName.Attributes.Add("onkeypress", "javascript:ValidateAlphabet()")
            Me.TextboxMiddleInitial.Attributes.Add("onkeypress", "javascript:ValidateAlphabet()")

            g_DataSet_dsGroups = AppDomain.CurrentDomain.GetData("DataSetUserGroups")
            g_DataSet_dsUsers = YMCARET.YmcaBusinessObject.UserGroupAdministrationBOClass.LookupUsers("")
            viewstate("g_DataSet_dsUsers") = g_DataSet_dsUsers
            If Not Me.IsPostBack Then
                'g_DataSet_dsUsers = AppDomain.CurrentDomain.GetData("DataSetUsers")
                Session("AddUser") = False
                user_key = Session("Session_UserId")
                If Request.QueryString("QueryPermission") = "True" Then
                    Session("User_Del") = False
                    Session("Item_Del") = False

                    Me.TabStripUserProperties.SelectedIndex = 1
                    Me.MultiPageUserProperties.SelectedIndex = 1
                    Me.ButtonDelete.Enabled = False
                    Me.ButtonAdd.Enabled = False
                    Me.ButtonOk.Enabled = True
                    Me.ButtonCancel.Enabled = False
                    Me.ButtonSave.Enabled = False
                End If

                If Session("Session_PageId") = "Properties" Then
                    PopulateUserdata()
                    PopulateAssociatedGroupData()
                    PopulateAvailableGroupData()
                    populateExceptionData()

                ElseIf Request.QueryString("QueryPermission") = "True" Then
                    PopulateUserdata()
                    PopulateAssociatedGroupData()
                    PopulateAvailableGroupData()
                    populateExceptionData()

                Else
                    Me.TabStripUserProperties.Items(1).Enabled = False
                    Me.LabelUserDetails.Text = " -- Add User"
                    Me.TextboxExtn.Text = ""
                    Me.TextboxFax.Text = ""
                    Me.TextBoxFirstName.Text = ""
                    Me.TextboxLastLogin.Text = "(none)"
                    Me.TextboxMiddleInitial.Text = ""
                    Me.TextboxLastName.Text = ""
                    Me.TextboxPassword.Text = ""
                    Me.TextboxPhone.Text = ""
                    Me.TextboxUserName.Text = ""
                    Me.CheckBoxActive.Checked = True
                    Me.ButtonRemoveAllGroups.Enabled = False
                    Me.ButtonAdd.Enabled = False
                    Me.ButtonPrint.Enabled = False
                    Me.ButtonDelete.Enabled = False
                    Me.ButtonOk.Enabled = False
                    Me.ButtonCancel.Enabled = True
                    Me.ButtonSave.Enabled = True

                    PopulateAllGroupsData()
                End If




            Else
                'code for is post back
                Me.TextboxPassword.Attributes("value") = Me.TextboxPassword.Text
                If (Request.Form("Yes") = "Yes" And Session("Item_Del") = True) Then
                    DeleteItem()
                Else
                    Session("Item_Del") = False
                End If
                If Request.Form("Yes") = "Yes" And Session("User_Del") = True Then
                    DeleteUser()

                Else
                    Session("User_Del") = False
                End If

            End If
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)

        End Try
        If Session("blnOpen") = False Then
            Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                                 "window.close();window.opener.location.reload();" & _
                                                                 "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
                Page.RegisterStartupScript("CloseWindow5", closeWindow5)
            End If
        End If
        
    End Sub

    Public Sub PopulateAllGroupsData()
        Try
            Dim i As Integer
            Dim dt_DataTableAllGroups As New DataTable
            dt_DataTableAllGroups.Columns.Add("Group Key")
            dt_DataTableAllGroups.Columns.Add("Group")
            dt_DataTableAllGroups.AcceptChanges()
            i = 0

            For Each l_GroupDataRow As DataRow In g_DataSet_dsGroups.Tables(0).Rows

                Dim dr As DataRow = dt_DataTableAllGroups.NewRow()
                dr.Item(0) = g_DataSet_dsGroups.Tables(0).Rows(i).Item("Group Key").ToString()
                dr.Item(1) = g_DataSet_dsGroups.Tables(0).Rows(i).Item("Group").ToString()
                dt_DataTableAllGroups.Rows.Add(dr)
                dt_DataTableAllGroups.AcceptChanges()

                i = i + 1
            Next

            Me.ListBoxAvailableGroups.DataSource = Nothing
            Me.ListBoxAvailableGroups.DataSource = dt_DataTableAllGroups
            Me.ListBoxAvailableGroups.DataTextField = "Group"
            Me.ListBoxAvailableGroups.DataValueField = "Group Key"
            Me.ListBoxAvailableGroups.DataBind()
            SortList(ListBoxAvailableGroups)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub PopulateAvailableGroupData()
        Try

            Dim i As Integer
            Dim dt_DataTableAvailableGroups As New DataTable
            dt_DataTableAvailableGroups.Columns.Add("Group Key")
            dt_DataTableAvailableGroups.Columns.Add("Group")
            dt_DataTableAvailableGroups.AcceptChanges()
            Dim l_boolean_Flag As Boolean

            i = 0
            For Each l_newdatarow As DataRow In g_DataSet_dsGroups.Tables(0).Rows
                l_boolean_Flag = True
                For Each l_row As DataRow In g_DataSet_dsGroupForUser.Tables(0).Rows
                    If l_newdatarow("Group").ToString.Trim() = l_row("Group").ToString.Trim() Then
                        l_boolean_Flag = False
                        Exit For
                    Else
                        l_boolean_Flag = True
                    End If
                Next
                If l_boolean_Flag = True Then
                    If Not IsDBNull(g_DataSet_dsGroups.Tables(0).Rows(i).Item("Group")) Then
                        Dim dr As DataRow = dt_DataTableAvailableGroups.NewRow()
                        dr.Item(0) = g_DataSet_dsGroups.Tables(0).Rows(i).Item("Group Key").ToString()
                        dr.Item(1) = g_DataSet_dsGroups.Tables(0).Rows(i).Item("Group").ToString()
                        dt_DataTableAvailableGroups.Rows.Add(dr)
                        dt_DataTableAvailableGroups.AcceptChanges()

                    End If
                End If
                i = i + 1
            Next


            Me.ListBoxAvailableGroups.DataSource = Nothing
            Me.ListBoxAvailableGroups.DataSource = dt_DataTableAvailableGroups
            Me.ListBoxAvailableGroups.DataTextField = "Group"
            Me.ListBoxAvailableGroups.DataValueField = "Group Key"
            Me.ListBoxAvailableGroups.DataBind()
            SortList(ListBoxAvailableGroups)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub PopulateUserdata()

        Try
            Dim l_String As String
            Dim l_DataRow() As DataRow

            l_String = "[User Key] = '" & Session("Session_UserId") & "'"
            l_DataRow = g_DataSet_dsUsers.Tables(0).Select(l_String)

            If Not IsDBNull(l_DataRow(0).Item("First Name")) Then
                Me.TextBoxFirstName.Text = l_DataRow(0)("First Name").ToString().Trim()
            End If
            If Not IsDBNull(l_DataRow(0)("Last Name")) Then
                Me.TextboxLastName.Text = l_DataRow(0)("Last Name").ToString().Trim()
            End If
            '''If Not IsDBNull(l_DataRow(0)("Last Login")) Then
            ''' Me.TextboxLastLogin.Text = l_DataRow(0)("Last Login").ToString().Trim()
            '''Else
            Me.TextboxLastLogin.Text = "(none)"
            '''End If
            If Not IsDBNull(l_DataRow(0)("Phone")) Then
                Me.TextboxPhone.Text = l_DataRow(0)("Phone").ToString().Trim()
            End If
            If Not IsDBNull(l_DataRow(0)("Middle Initial")) Then
                Me.TextboxMiddleInitial.Text = l_DataRow(0)("Middle Initial").ToString().Trim()
            End If
            If Not IsDBNull(l_DataRow(0)("Password")) Then
                Me.TextboxPassword.Attributes("value") = l_DataRow(0)("Password").ToString().Trim()
            End If
            If Not IsDBNull(l_DataRow(0)("Username")) Then
                Me.TextboxUserName.Text = l_DataRow(0)("Username").ToString().Trim()
            End If
            If Not IsDBNull(l_DataRow(0)("Extn")) Then
                Me.TextboxExtn.Text = l_DataRow(0)("Extn").ToString().Trim()
            End If
            If Not IsDBNull(l_DataRow(0)("Fax")) Then
                Me.TextboxFax.Text = l_DataRow(0)("Fax").ToString().Trim()
            End If

            If CType(l_DataRow(0)("Active"), String) = "Yes" Then
                Me.CheckBoxActive.Checked = True
            Else
                Me.CheckBoxActive.Checked = False
                Me.ButtonAddAllGroups.Enabled = False
                Me.ButtonRemoveAllGroups.Enabled = False
            End If
            Me.LabelUserDetails.Text = "--" & l_DataRow(0)("Last Name").ToString().Trim() + " " + l_DataRow(0)("First Name").ToString().Trim()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub TabStripUserProperties_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripUserProperties.SelectedIndexChange
        Try
            Me.MultiPageUserProperties.SelectedIndex = Me.TabStripUserProperties.SelectedIndex
            If Me.MultiPageUserProperties.SelectedIndex = 1 Then
                Me.ButtonDelete.Enabled = True
                Me.ButtonAdd.Enabled = True
                Me.ButtonOk.Enabled = True
                Me.ButtonCancel.Enabled = False
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
    Public Sub PopulateAssociatedGroupData()
        Dim dt_DataTableAvailableGroups As New DataTable
        dt_DataTableAvailableGroups.Columns.Add("Group Key")
        dt_DataTableAvailableGroups.Columns.Add("Group")
        dt_DataTableAvailableGroups.AcceptChanges()
        Try
            Dim i As Integer
            g_DataSet_dsGroupForUser = YMCARET.YmcaBusinessObject.UserPropertiesBOClass.SearchGroupsForUser(Session("Session_userId"))
            If g_DataSet_dsGroupForUser.Tables.Count = 0 Then
                'code to be written
            Else
                'Dim l_arrGroups(g_DataSet_dsGroupForUser.Tables(0).Rows.Count - 1) As String
                i = 0

                For Each l_GroupDataRow As DataRow In g_DataSet_dsGroupForUser.Tables(0).Rows

                    Dim dr As DataRow = dt_DataTableAvailableGroups.NewRow()
                    dr.Item(0) = g_DataSet_dsGroupForUser.Tables(0).Rows(i).Item("Group Key").ToString()
                    dr.Item(1) = g_DataSet_dsGroupForUser.Tables(0).Rows(i).Item("Group").ToString()
                    dt_DataTableAvailableGroups.Rows.Add(dr)
                    dt_DataTableAvailableGroups.AcceptChanges()

                    i = i + 1
                Next

                Me.ListboxMemberOf.DataSource = dt_DataTableAvailableGroups
                Me.ListboxMemberOf.DataTextField = "Group"
                Me.ListboxMemberOf.DataValueField = "Group Key"
                Me.ListboxMemberOf.DataBind()
                SortList(ListboxMemberOf)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'commented by Anudeep on 26-sep-2012 :Bt-1153 - Start
    'Private Sub ButtonAddGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddGroup.Click
    '    Try
    '        Dim l_Integer_Selected_AvailableGroup As Integer
    '        For l_Integer_Selected_AvailableGroup = 0 To Me.ListBoxAvailableGroups.Items.Count - 1
    '            If Me.ListBoxAvailableGroups.Items(l_Integer_Selected_AvailableGroup).Selected Then
    '                Me.ListboxMemberOf.Items.Add(Me.ListBoxAvailableGroups.Items(l_Integer_Selected_AvailableGroup))
    '            End If
    '        Next
    '        For l_Integer_Selected_AvailableGroup = Me.ListBoxAvailableGroups.Items.Count - 1 To 0 Step -1
    '            If Me.ListBoxAvailableGroups.Items(l_Integer_Selected_AvailableGroup).Selected Then
    '                Me.ListBoxAvailableGroups.Items.RemoveAt(l_Integer_Selected_AvailableGroup)
    '            End If
    '        Next
    '        Me.ListboxMemberOf.SelectedIndex = -1
    '        Me.ButtonAddGroup.Enabled = False
    '        Me.ButtonSave.Enabled = True
    '        Me.ButtonCancel.Enabled = True
    '    Catch ex As SqlException

    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception

    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try
    'commented by Anudeep on 26-sep-2012 :Bt-1153 -End
    'End Sub
    'Priya							05-Jan-2011			BT-615:YRS 5.0-969 : User Access screen changes are in aspx file added j-Query to handle scrol position commented code of selected insex change of list box and aaded to JQuery
    'Private Sub ListBoxAvailableGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxAvailableGroups.SelectedIndexChanged
    '	Try
    '		If Me.CheckBoxActive.Checked = True Then
    '			Me.ButtonAddGroup.Enabled = True
    '		Else
    '			Me.ButtonAddGroup.Enabled = False
    '		End If
    '	Catch ex As SqlException

	'		g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'		Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
	'	Catch ex As Exception

	'		g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'		Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
	'	End Try


	'End Sub
	'   Private Sub ListboxMemberOf_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListboxMemberOf.SelectedIndexChanged
	'       Try
	'           If Me.CheckBoxActive.Checked = True Then
	'               Me.ButtonRemoveGroup.Enabled = True
	'           Else
	'               Me.ButtonRemoveGroup.Enabled = False
	'           End If

	'       Catch ex As SqlException

	'           g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'           Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
	'       Catch ex As Exception

	'           g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'           Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)

	'       End Try

	'   End Sub

	Private Sub ButtonAddAllGroups_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddAllGroups.Click
		Try
			Dim l_Integer_AllAvailableGroups As Integer
			For l_Integer_AllAvailableGroups = 0 To Me.ListBoxAvailableGroups.Items.Count - 1
				Me.ListboxMemberOf.Items.Add(Me.ListBoxAvailableGroups.Items(l_Integer_AllAvailableGroups))
			Next
			For l_Integer_AllAvailableGroups = Me.ListBoxAvailableGroups.Items.Count - 1 To 0 Step -1
				Me.ListBoxAvailableGroups.Items.RemoveAt(l_Integer_AllAvailableGroups)
            Next
            'Added by Anudeep on 01-10-2012 for Bt-1057
            SortList(ListboxMemberOf)
			Me.ButtonAddAllGroups.Enabled = False
			Me.ButtonRemoveAllGroups.Enabled = True
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
    Private Sub ButtonRemoveAllGroups_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveAllGroups.Click
        Try
            Dim l_Integer_AllMemberOf As Integer
            For l_Integer_AllMemberOf = 0 To Me.ListboxMemberOf.Items.Count - 1
                Me.ListBoxAvailableGroups.Items.Add(Me.ListboxMemberOf.Items(l_Integer_AllMemberOf))
            Next
            For l_Integer_AllMemberOf = Me.ListboxMemberOf.Items.Count - 1 To 0 Step -1
                Me.ListboxMemberOf.Items.RemoveAt(l_Integer_AllMemberOf)
            Next
            'Added by Anudeep on 01-10-2012 for Bt-1057
            SortList(ListBoxAvailableGroups)
            Me.ButtonRemoveAllGroups.Enabled = False
            Me.ButtonAddAllGroups.Enabled = True
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
    'commented by Anudeep on 26-sep-2012 :Bt-1153
    'Private Sub ButtonRemoveGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveGroup.Click
    '    Try
    '        Dim l_Integer_Selected_MemberOf As Integer
    '        For l_Integer_Selected_MemberOf = 0 To Me.ListboxMemberOf.Items.Count - 1
    '            If Me.ListboxMemberOf.Items(l_Integer_Selected_MemberOf).Selected Then
    '                Me.ListBoxAvailableGroups.Items.Add(Me.ListboxMemberOf.Items(l_Integer_Selected_MemberOf))
    '            End If
    '        Next
    '        For l_Integer_Selected_MemberOf = Me.ListboxMemberOf.Items.Count - 1 To 0 Step -1
    '            If Me.ListboxMemberOf.Items(l_Integer_Selected_MemberOf).Selected Then
    '                Me.ListboxMemberOf.Items.RemoveAt(l_Integer_Selected_MemberOf)
    '            End If
    '        Next
    '        Me.ListBoxAvailableGroups.SelectedIndex = -1
    '        Me.ButtonRemoveGroup.Enabled = False
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
    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            Session("blnOpen") = False
            Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                         "window.opener.document.forms(0).submit();self.close();" & _
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
        Session("Refresh_Flag") = Nothing

    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Session("AddUser") = True
        Try
            'Added by neeraj on 25-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Me.TabStripUserProperties.SelectedIndex = 0
            Me.MultiPageUserProperties.SelectedIndex = 0
            Me.TabStripUserProperties.Items(1).Enabled = False
            Me.TextboxExtn.Text = ""
            Me.TextboxFax.Text = ""
            Me.TextBoxFirstName.Text = ""
            Me.TextboxLastLogin.Text = "(none)"
            Me.TextboxMiddleInitial.Text = ""
            Me.TextboxLastName.Text = ""
            Me.TextboxPassword.Attributes("Value") = ""
            Me.TextboxPhone.Text = ""
            Me.TextboxUserName.Text = ""
            Me.CheckBoxActive.Checked = True

            Dim l_Integer_AllMemberOf As Integer
            For l_Integer_AllMemberOf = 0 To Me.ListboxMemberOf.Items.Count - 1
                Me.ListBoxAvailableGroups.Items.Add(Me.ListboxMemberOf.Items(l_Integer_AllMemberOf))
            Next
            For l_Integer_AllMemberOf = Me.ListboxMemberOf.Items.Count - 1 To 0 Step -1
                Me.ListboxMemberOf.Items.RemoveAt(l_Integer_AllMemberOf)
            Next
            Me.LabelUserDetails.Text = "-- Add User"
            Me.ButtonAdd.Enabled = False
            Me.ButtonPrint.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = False
            Me.ButtonCancel.Enabled = True
            Me.ButtonSave.Enabled = True
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)

        End Try

    End Sub
    Private Sub TextboxLastName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxLastName.TextChanged
        Try
            Dim l_String_UserName As String
            If Me.TextboxUserName.Text = "" Then
                l_String_UserName = Me.TextBoxFirstName.Text.Substring(0, 1)
                If Me.TextboxMiddleInitial.Text = "" Then
                    l_String_UserName = l_String_UserName + "X"
                Else
                    l_String_UserName = l_String_UserName + LTrim(RTrim(Me.TextboxMiddleInitial.Text.ToUpper.Trim))
                End If
                l_String_UserName = l_String_UserName + LTrim(RTrim(Me.TextboxLastName.Text.ToUpper.Trim))
                Me.TextboxUserName.Text = l_String_UserName.ToUpper()
            End If
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub
    Private Sub CheckBoxActive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxActive.CheckedChanged
        Try
            If Me.CheckBoxActive.Checked = False Then
                Me.TextBoxFirstName.ReadOnly = True
                Me.TextboxLastName.ReadOnly = True
                Me.TextboxMiddleInitial.ReadOnly = True
                Me.TextboxPassword.ReadOnly = True
                Me.TextboxUserName.ReadOnly = True
                Me.TextboxPhone.ReadOnly = True
                Me.TextboxExtn.ReadOnly = True
                Me.TextboxFax.ReadOnly = True
                Me.ButtonAddAllGroups.Enabled = False
                Me.ButtonRemoveAllGroups.Enabled = False
            End If
            If Me.CheckBoxActive.Checked = True Then
                Me.TextBoxFirstName.ReadOnly = False
                Me.TextboxLastName.ReadOnly = False
                Me.TextboxMiddleInitial.ReadOnly = False
                Me.TextboxPassword.ReadOnly = False
                Me.TextboxUserName.ReadOnly = False
                Me.TextboxPhone.ReadOnly = False
                Me.TextboxExtn.ReadOnly = False
                Me.TextboxFax.ReadOnly = False
                Me.ButtonAddAllGroups.Enabled = True
                If Not Me.ListboxMemberOf.Items.Count = 0 Then
                    Me.ButtonRemoveAllGroups.Enabled = True
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
    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_Bool_MessageFlag As Boolean
        Dim l_ds_userDetails As DataSet
        Dim l_string_GroupList As String
        l_Bool_MessageFlag = False
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If l_Bool_MessageFlag = False And Me.TextboxPassword.Text.Length < 6 Then
                'LAST PARAMETER CHANGED TO FALSE BY ANITA ON JUNE 01-2007
                MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", "Password must be atleast 6 characters long .", MessageBoxButtons.OK, False)
                l_Bool_MessageFlag = True
            End If
            l_string_GroupList = DirectCast(Request.Form("sortedList"), String)
            'Anudeep :26.11.2012 Changed by anudeep to for checking the list if it empty
            If l_Bool_MessageFlag = False And l_string_GroupList.Length = 0 Then
                'LAST PARAMETER CHANGED TO FALSE BY ANITA ON JUNE 01-2007
                'START | ML | 2019.08.19 | YRS-AT-4546 - YRS bug: fix typo "menber" should be "member"
                'MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", "Unable to save because this user has not been specified as a menber of alteast one group.", MessageBoxButtons.OK, False)
                MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", "Unable to save because this user has not been specified as a member of atleast one group.", MessageBoxButtons.OK, False)

                'END | ML | 2019.08.19 | YRS-AT-4546 - YRS bug: fix typo "menber" should be "member"
                l_Bool_MessageFlag = True
            End If

            If l_Bool_MessageFlag = False Then


                Dim l_string_Active As String
                Dim l_integer_UserId As Integer
                l_string_GroupList = ""
                Dim l_integer_GroupCount As Integer
                'commented by Anudeep on 26-sep-2012 :Bt-1153
                'For l_integer_GroupCount = 0 To Me.ListboxMemberOf.Items.Count - 1
                '    l_string_GroupList = l_string_GroupList + Me.ListboxMemberOf.Items(l_integer_GroupCount).Value
                '    l_string_GroupList = l_string_GroupList + ","
                'Next
                'Added by Anudeep on 26-sep-2012 :issue is Bt-1153 
                l_string_GroupList = DirectCast(Request.Form("sortedList"), String)
                If Me.CheckBoxActive.Checked = True Then
                    l_string_Active = "Y"
                Else
                    l_string_Active = "N"
                End If

                If Session("AddUser") = True Or Session("Session_UserId") = "" Then
                    Dim err_output As Integer
                    err_output = YMCARET.YmcaBusinessObject.UserPropertiesBOClass.InsertUserDetails(Me.TextBoxFirstName.Text, Me.TextboxMiddleInitial.Text, Me.TextboxLastName.Text, Me.TextboxPhone.Text, Me.TextboxExtn.Text, Me.TextboxFax.Text, Me.TextboxPassword.Text, l_string_Active, Me.TextboxUserName.Text, l_string_GroupList)
                    If err_output = 1 Then
                        'parameter top=140 and left=130 removed by Anita on 01-06-2007 and last parameter set to false
                        MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", "User """ + Me.TextboxUserName.Text.Trim + """ Already On File", MessageBoxButtons.OK, False)
                    Else
                        l_ds_userDetails = YMCARET.YmcaBusinessObject.UserPropertiesBOClass.GetUserKey(Me.TextboxUserName.Text.Trim())
                        Session("Session_UserId") = l_ds_userDetails.Tables("UserKey").Rows(0)("User Key").ToString().Trim()
                        'Anudeep 26.11.2012  Added for to show user details after adding a new user
                        PopulateAssociatedGroupData()
                        PopulateAvailableGroupData()
                        Me.ButtonCancel.Enabled = False
                        Me.ButtonSave.Enabled = False
                        Me.ButtonAdd.Enabled = True
                        Me.ButtonDelete.Enabled = True
                        Me.ButtonOk.Enabled = True
                        Me.TabStripUserProperties.Items(1).Enabled = True
                        Session("User_Added") = True
                        Session("AddUser") = False
                    End If
                ElseIf Not Session("Session_UserId") = "" Then
                    l_integer_UserId = CType(Session("Session_UserId"), Integer)
                    YMCARET.YmcaBusinessObject.UserPropertiesBOClass.UpdateUserDetails(l_integer_UserId, Me.TextBoxFirstName.Text, Me.TextboxMiddleInitial.Text, Me.TextboxLastName.Text, Me.TextboxPhone.Text, Me.TextboxExtn.Text, Me.TextboxFax.Text, Me.TextboxPassword.Text, l_string_Active, Me.TextboxUserName.Text, l_string_GroupList)
                    Dim i As Integer
                    For i = 0 To Me.DataGridUserProps.Items.Count - 1
                        Dim l_label_ItemCode As Label
                        l_label_ItemCode = CType(Me.DataGridUserProps.Items(i).FindControl("lblItemCode"), Label)
                        Dim l_itemCode As Integer
                        l_itemCode = CType(l_label_ItemCode.Text, Integer)

                        Dim l_drop As DropDownList
                        l_drop = CType(Me.DataGridUserProps.Items(i).FindControl("DrpAccess"), DropDownList)
                        Dim l_Access As Integer
                        If l_drop.SelectedItem.Text.ToUpper().Trim() = "Full".ToUpper.Trim() Then
                            l_Access = 1
                        ElseIf l_drop.SelectedItem.Text.ToUpper.Trim = "Read-Only".ToUpper.Trim() Then
                            l_Access = 8
                        Else
                            l_Access = 9
                        End If
                        YMCARET.YmcaBusinessObject.UserPropertiesBOClass.UpdateUserAccExceptions(l_integer_UserId, l_itemCode, l_Access)
                        'Added by Anudeep on 26-sep-2012 :issue is Bt-1153 
                        PopulateAssociatedGroupData()
                        PopulateAvailableGroupData()

                    Next
                    Me.ButtonCancel.Enabled = False
                    Me.ButtonSave.Enabled = False
                    Me.ButtonAdd.Enabled = True
                    Me.ButtonDelete.Enabled = True
                    Me.ButtonOk.Enabled = True
                    Me.TabStripUserProperties.Items(1).Enabled = True
                    Session("User_Added") = True
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
    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Try
            'Added by neeraj on 25-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Session("User_Del") = True
            Session("User_Added") = True
            MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", "Are you SURE you want to delete " + Me.TextBoxFirstName.Text.Trim() + " " + Me.TextboxLastName.Text.Trim + "?", MessageBoxButtons.YesNo, False)
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    Private Sub ButtonDeleteItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeleteItem.Click
        Try
            Session("Item_Del") = True
            Session("Item_Modified") = True
            Dim lblDesc As Label
            lblDesc = CType(Me.DataGridUserProps.SelectedItem.FindControl("lblItemDesc"), Label)
            MessageBox.Show(PlaceHolderUserProps, "YMCA-YRS", "Are you SURE you want to delete the access permission to " + lblDesc.Text + " ?", MessageBoxButtons.YesNo, False)
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    Public Sub DeleteUser()
        Dim l_integer_UserId As Integer
        l_integer_UserId = CType(user_key, Integer)
        Try
            YMCARET.YmcaBusinessObject.UserPropertiesBOClass.DeleteUserDetails(l_integer_UserId)
           
            Me.DataGridUserProps.DataSource = Nothing
            Me.DataGridUserProps.DataBind()


        Catch ex As Exception
            Throw ex
        End Try
        Session("User_Del") = False
        Session("blnOpen") = False
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            If Session("Session_PageId") = "Properties" Then
                PopulateUserdata()
                PopulateAssociatedGroupData()
                PopulateAvailableGroupData()
                populateExceptionData()
                Me.TabStripUserProperties.Items(1).Enabled = True

            Else

                Me.TextboxExtn.Text = ""
                Me.TextboxFax.Text = ""
                Me.TextBoxFirstName.Text = ""
                Me.TextboxLastLogin.Text = ""
                Me.TextboxMiddleInitial.Text = ""
                Me.TextboxLastName.Text = ""
                Me.TextboxPassword.Text = ""
                Me.TextboxPhone.Text = ""
                Me.TextboxUserName.Text = ""
                Me.CheckBoxActive.Checked = True
                Me.ButtonRemoveAllGroups.Enabled = False
                PopulateAllGroupsData()

            End If
            Me.ButtonCancel.Enabled = False
            Me.ButtonSave.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = True
            Me.ButtonAddAllGroups.Enabled = True
            Session("AddUser") = False
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
        Session("Refresh_Flag") = Nothing
    End Sub
    Private Sub ButtonAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddItem.Click
        Try
            If Session("Refresh_Flag") = "True" Then
                Response.Redirect("UserPermissionExceptions.aspx", False)
            Else
                Dim popupScript As String = "<script language='javascript'>" & _
                                                           "window.open('UserPermissionExceptions.aspx','CustomPopUp', " & _
                                                           "'width=720, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                                           "</script>"

                If Not (Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript)
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
    Public Sub populateExceptionData()
        Try
            Dim dsAccExceptions As DataSet
            Dim l_integer_userId As Integer
            l_integer_userId = Convert.ToInt16(Session("Session_UserId"))
            dsAccExceptions = YMCARET.YmcaBusinessObject.UserPermissionExceptionsBOClass.LookUpUserAccExceptions(l_integer_userId)
            Me.DataGridUserProps.DataSource = dsAccExceptions
            Me.DataGridUserProps.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridUserProps_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridUserProps.ItemDataBound
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

                dglabelAccess = CType(e.Item.FindControl("lblAccess"), Label)
                If dglabelAccess.Text.ToUpper().Trim() = 1 Then
                    drop.SelectedValue = 1
                ElseIf dglabelAccess.Text.ToUpper().Trim() = 8 Then
                    drop.SelectedValue = 8
                ElseIf dglabelAccess.Text.ToUpper().Trim() = 9 Then
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
    Public Sub enableSave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.ButtonOk.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonAdd.Enabled = False
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub

    Public Sub DeleteItem()
        Try
            If Me.DataGridUserProps.Items.Count <> 0 Then
                Dim l_integer_userid As Integer
                Dim l_label_code As Label
                Dim l_integer_itemcode As Integer
                l_integer_userid = Convert.ToInt16(Session("Session_UserId"))
                l_label_code = CType(Me.DataGridUserProps.SelectedItem.Cells(1).FindControl("lblItemCode"), Label)
                l_integer_itemcode = Convert.ToInt16(l_label_code.Text.Trim)
                YMCARET.YmcaBusinessObject.UserPropertiesBOClass.DeleteUserAccExceptions(l_integer_userid, l_integer_itemcode)
                populateExceptionData()
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Session("Item_del") = False
        Me.DataGridUserProps.SelectedIndex = -1
        Me.ButtonDeleteItem.Enabled = False
    End Sub

    Private Sub DataGridUserProps_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridUserProps.SelectedIndexChanged
        Try
            Me.ButtonDeleteItem.Enabled = True
        Catch ex As SqlException

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    'Added by Anudeep on 26-sep-2012 :issue is Bt-1153 
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
