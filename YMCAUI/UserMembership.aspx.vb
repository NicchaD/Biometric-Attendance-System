'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya							05-Jan-2011			BT-615:YRS 5.0-969 : User Access screen changes are in aspx file added j-Query to handle scrol position commented code of selected insex change of list box and aaded to JQuery
'Anudeep A.                     26-Sep-2012         BT-1153:User Membership not appearing in sorted order 
'Anudeep A.                     01-oct-2012         BT-1153:Available groups are not shorting by alphabetical order when click on 'Add All' button 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Imports System.Collections
Imports System.Data.SqlClient
Public Class UserMembership
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserGroupAdministration.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ListBoxAvailableGroups As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents ButtonAddGroup As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddAllGroups As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveGroup As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveAllGroups As System.Web.UI.WebControls.Button
    Protected WithEvents ListboxMemberOf As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAvailableGroups As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMemberOf As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderUserMembership As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim user_key As String
    Dim g_DataSet_dsUsers As DataSet
    Dim g_DataSet_dsGroupForUser As DataSet
    Dim g_DataSet_dsGroups As DataSet
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            user_key = Session("Session_UserId")
            g_DataSet_dsGroups = AppDomain.CurrentDomain.GetData("DataSetUserGroups")

            If Not Me.IsPostBack Then
                PopulateAssociatedGroupData()
                PopulateAvailableGroupData()
            Else
                ' Code to be written
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
      
    End Sub
    Public Sub PopulateAvailableGroupData()
        Try
            Dim i As Integer
            Dim l_boolean_Flag As Boolean
            Dim dt_DataTableAvailableGroups As New DataTable
            dt_DataTableAvailableGroups.Columns.Add("Group Key")
            dt_DataTableAvailableGroups.Columns.Add("Group")
            dt_DataTableAvailableGroups.AcceptChanges()
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
            Me.ListBoxAvailableGroups.DataValueField = "Group key"
            Me.ListBoxAvailableGroups.DataBind()
            SortList(ListBoxAvailableGroups)
        Catch ex As Exception
            Throw ex
        End Try
       
    End Sub
    Public Sub PopulateAssociatedGroupData()
        Try
            Dim dt_DataTableAvailableGroups As New DataTable
            dt_DataTableAvailableGroups.Columns.Add("Group Key")
            dt_DataTableAvailableGroups.Columns.Add("Group")
            dt_DataTableAvailableGroups.AcceptChanges()
            Dim i As Integer
            g_DataSet_dsGroupForUser = YMCARET.YmcaBusinessObject.UserMembershipBOClass.SearchGroupsForUser(user_key)
            If g_DataSet_dsGroupForUser.Tables.Count = 0 Then
                'code to be written
            Else
                'Dim l_arrGroups(g_DataSet_dsGroupForUser.Tables(0).Rows.Count - 1) As String
                i = 0
                For Each l_newDataRow As DataRow In g_DataSet_dsGroupForUser.Tables(0).Rows
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
    'commented by Anudeep on 26-sep-2012 :Bt-1153
    'Private Sub ButtonAddGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddGroup.Click
    '    Dim l_Integer_Selected_AvailableGroup As Integer
    '    Try
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
    '        Me.ButtonAddGroup.Enabled = False
    '        Me.ButtonSave.Enabled = True
    '        Me.ButtonCancel.Enabled = True
    '        Me.ListboxMemberOf.SelectedIndex = -1
    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try
    'End Sub


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
    '        Me.ButtonRemoveGroup.Enabled = False
    '        Me.ButtonSave.Enabled = True
    '        Me.ButtonCancel.Enabled = True
    '        Me.ListBoxAvailableGroups.SelectedIndex = -1
    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
    '    End Try

    'End Sub

    Private Sub ButtonAddAllGroups_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddAllGroups.Click
        Try
            Dim l_Integer_AllGroups As Integer
            For l_Integer_AllGroups = 0 To Me.ListBoxAvailableGroups.Items.Count - 1
                Me.ListboxMemberOf.Items.Add(Me.ListBoxAvailableGroups.Items(l_Integer_AllGroups))
            Next
            For l_Integer_AllGroups = Me.ListBoxAvailableGroups.Items.Count - 1 To 0 Step -1
                Me.ListBoxAvailableGroups.Items.RemoveAt(l_Integer_AllGroups)
            Next
            'Added by Anudeep on 01-10-2012 for Bt-1153
            SortList(ListboxMemberOf)
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.ButtonAddAllGroups.Enabled = False
            Me.ButtonRemoveAllGroups.Enabled = True
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
            Dim l_Integer_AllMemberof As Integer
            For l_Integer_AllMemberof = 0 To Me.ListboxMemberOf.Items.Count - 1
                Me.ListBoxAvailableGroups.Items.Add(Me.ListboxMemberOf.Items(l_Integer_AllMemberof))
            Next
            For l_Integer_AllMemberof = Me.ListboxMemberOf.Items.Count - 1 To 0 Step -1
                Me.ListboxMemberOf.Items.RemoveAt(l_Integer_AllMemberof)
            Next
            'Added by Anudeep on 01-10-2012 for Bt-1153
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
	'Priya							05-Jan-2011			BT-615:YRS 5.0-969 : User Access screen changes are in aspx file added j-Query to handle scrol position
	'commented code here and aaded to JQuery
	'Private Sub ListBoxAvailableGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxAvailableGroups.SelectedIndexChanged
	'    Try
	'        Me.ButtonAddGroup.Enabled = True
	'    Catch ex As SqlException
	'        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
	'    Catch ex As Exception
	'        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
	'    End Try

	'End Sub

	'Private Sub ListboxMemberOf_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListboxMemberOf.SelectedIndexChanged
	'    Try
	'        Me.ButtonRemoveGroup.Enabled = True
	'    Catch ex As SqlException
	'        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
	'    Catch ex As Exception
	'        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
	'    End Try

	'End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_string_GroupList As String
        Dim l_integer_GroupCount As Integer
        Dim l_Bool_MessageFlag As Boolean
        l_Bool_MessageFlag = False
        l_string_GroupList = ""
        Dim l_int_UserId As Integer
        Try
            'Added by neeraj on 25-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderUserMembership, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            If l_Bool_MessageFlag = False And Me.ListboxMemberOf.Items.Count = 0 Then
                MessageBox.Show(PlaceHolderUserMembership, "YMCA-YRS", "Unable to save because this user has not been specified as a menber of alteast one group.", MessageBoxButtons.OK, False)
                l_Bool_MessageFlag = True
            End If
            If l_Bool_MessageFlag = False Then
                'commented by Anudeep on 26-sep-2012 :issue is Bt-1153 
                'For l_integer_GroupCount = 0 To Me.ListboxMemberOf.Items.Count - 1
                '    l_string_GroupList = l_string_GroupList + Me.ListboxMemberOf.Items(l_integer_GroupCount).Value
                '    l_string_GroupList = l_string_GroupList + ","
                'Next
                'Added by Anudeep on 26-sep-2012 :issue is Bt-1153  
                l_string_GroupList = DirectCast(Request.Form("sortedList"), String)
                l_int_UserId = CType(user_key, Integer)
                YMCARET.YmcaBusinessObject.UserMembershipBOClass.UpdateUserMembership(l_int_UserId, l_string_GroupList)
                Me.ButtonOk.Enabled = True
                'Added by Anudeep on 26-sep-2012 :issue is Bt-1153  
                PopulateAssociatedGroupData()
                PopulateAvailableGroupData()
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
            PopulateAssociatedGroupData()
            PopulateAvailableGroupData()
            Me.ButtonCancel.Enabled = False
            Me.ButtonSave.Enabled = False
            Me.ButtonOk.Enabled = True
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
