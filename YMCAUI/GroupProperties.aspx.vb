'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session   
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Neeraj Singh                   14/Dec/2009         Added read only button for issue id BT1065
'Nikunj Patel                   16/Dec/2009         Added code to prevent postback and enable the save cancel buttons always
'Priya                          30-March-2010       BT-468:Shows error page Cast from string “” to type 'Double' is not valid.
'Anudeep                        10-oct-2012         Bt-1208 Changes made in YMCA Maintenance screen
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
Public Class GroupProperties
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UserGroupAdministration.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelGroup As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGroup As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGroupDetail As System.Web.UI.WebControls.Label
    Protected WithEvents LabelText As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonFullAccess As System.Web.UI.WebControls.Button
    Protected WithEvents LabelSecuredItemType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListItemType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ButtonNone As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridGroupProps As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonFirst As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPrevious As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonNext As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonLast As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonMembers As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderGroupProps As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonReadOnly As System.Web.UI.WebControls.Button

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
    Dim g_DataSet_dsGroups As DataSet
    Dim g_DataSet_dsGroupProperties As DataSet
    Dim g_DataSet_dsSecuredItems As DataSet
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Dim l_String As String
            Dim l_DataRow() As DataRow
            ''Me.ButtonMembers.Attributes.Add("onclick", "javascript: return NewWindow('GroupMembers.aspx','GroupMembers','720','400','yes','center');")

            g_DataSet_dsGroups = AppDomain.CurrentDomain.GetData("DataSetUserGroups")

            If Not Me.IsPostBack Then
                Session("Grp_Props_Sort") = Nothing
                Group_Key = Session("Session_GroupId")
                Session("Add_Flag") = False
                If Session("Session_PageId") = "Properties" Then
                    Session("Group_Del") = False
                    PopulateData()
                    'PopulateSecuredItems()
                    'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
                    Me.ButtonCancel.Enabled = True
                    Me.ButtonSave.Enabled = True

                    l_String = "[Group Key] = '" & Group_Key & "'"
                    l_DataRow = g_DataSet_dsGroups.Tables(0).Select(l_String)
                    Me.LabelGroupDetail.Text = "--" & l_DataRow(0)("Group")
                    If Not IsDBNull(l_DataRow(0)("Group")) Then
                        Me.TextBoxGroup.Text = l_DataRow(0)("Group")
                    End If
                    If Not IsDBNull(l_DataRow(0)("Description")) Then
                        Me.TextboxDesc.Text = l_DataRow(0)("Description")
                    End If
                Else
                    Me.LabelGroupDetail.Text = "-- Add Group"
                    PopulateSecuredItems()
                    Me.TextBoxGroup.Text = ""
                    Me.TextboxDesc.Text = ""
                    Me.ButtonCancel.Enabled = True
                    Me.ButtonSave.Enabled = True
                    Me.ButtonPrint.Enabled = False
                    Me.ButtonOk.Enabled = False
                    Me.ButtonMembers.Enabled = False
                    Me.ButtonDelete.Enabled = False
                    Me.ButtonAdd.Enabled = False
                End If

            Else
                If Request.Form("Yes") = "Yes" And Session("Group_Del") = True Then
                    DeleteGroup()
                Else
                    Session("Group_Del") = False
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
    Public Sub PopulateData()
        Try
            Dim l_Str_ControlType As String
            If Me.DropDownListItemType.SelectedItem.Text = "All" Then
                l_Str_ControlType = ""
            Else
                l_Str_ControlType = Me.DropDownListItemType.SelectedValue
            End If
            g_DataSet_dsGroupProperties = YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.LookUpGroupProperties(Session("Session_GroupId"), l_Str_ControlType)

            DataGridGroupProps.DataSource = Nothing
            DataGridGroupProps.DataBind()
            DataGridGroupProps.DataSource = g_DataSet_dsGroupProperties.Tables("GroupProperties")
            viewstate("Group_Prop_Sort") = g_DataSet_dsGroupProperties
            DataGridGroupProps.DataBind()

        
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub PopulateSecuredItems()
        Try
            Dim l_Str_ControlType As String
            If Me.DropDownListItemType.SelectedItem.Text = "All" Then
                l_Str_ControlType = ""
            Else
                l_Str_ControlType = Me.DropDownListItemType.SelectedValue
            End If
            g_DataSet_dsSecuredItems = YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.LookUpSecuredItems(l_Str_ControlType)
            DataGridGroupProps.DataSource = g_DataSet_dsSecuredItems.Tables("SecuredItems")
            viewstate("Group_Prop_Sort") = g_DataSet_dsSecuredItems
            DataGridGroupProps.DataBind()
       
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub DataGridGroupProps_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridGroupProps.ItemDataBound
        Try
            Dim i As Integer
            Dim drop As DropDownList
            Dim dglabelAccess As Label
            Dim dglabelType As Label
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                drop = CType(e.Item.FindControl("DropDownGridAccess"), DropDownList)
                dglabelType = CType(e.Item.FindControl("lblType"), Label)
                If dglabelType.Text.Trim().ToUpper() = "Menu bar".ToUpper() Or dglabelType.Text.Trim().ToUpper() = "Menu pad".ToUpper() Then
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
                If Session("Session_PageId") = "Properties" Then
                    If dglabelAccess.Text.Trim().ToUpper() = "Full".ToUpper() Then
                        'drop.SelectedItem.Text = "Full"
                        drop.SelectedValue = 1
                    ElseIf dglabelAccess.Text.Trim().ToUpper() = "Read-Only".ToUpper() Then
                        ' drop.SelectedItem.Text = "Read-Only"
                        drop.SelectedValue = 8
                    ElseIf dglabelAccess.Text.Trim().ToUpper() = "None".ToUpper() Then
                        '  drop.SelectedItem.Text = "None"
                        drop.SelectedValue = 9
                    End If
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

    Private Sub DropDownListItemType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListItemType.SelectedIndexChanged
        Try
            'Priya : 30-March-2010:BT-468:Shows error page Cast from string “” to type 'Double' is not valid.
            'ConvertSession value to string
            If Not Convert.ToString(Session("Session_GroupId")) = "" Then
                PopulateData()
            Else
                PopulateSecuredItems()
            End If
            'Added by Anudeep:10.09.2012 for bt-1208
            ButtonSave.Enabled = True
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
                MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_string_SecuredItems As String
            Dim l_string_Access As String
            Dim l_integer_ItemCount As Integer
            Dim l_integer_GroupId As Integer
            Dim l_boolean_MessageFlag As Boolean = False
            Dim l_ds_GrpKey As DataSet

            l_string_SecuredItems = ""
            l_string_Access = ""

            'Priya : 30-March-2010:BT-468:Shows error page Cast from string “” to type 'Double' is not valid.
            'ConvertSession value to string
            If Not Convert.ToString(Session("Session_GroupId")) = "" Then
                l_integer_GroupId = CType(Session("Session_GroupId"), Integer)
            End If

            If Me.TextBoxGroup.Text = "" Then
                'last parameter of Show method set to false and top=250 left=120 removed  by Anita on 31-05-2007
                MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", "Group must be entered in order to Save.", MessageBoxButtons.OK, False)
                l_boolean_MessageFlag = True
            End If
            If Me.TextboxDesc.Text = "" And l_boolean_MessageFlag = False Then
                'last parameter of Show method set to false and top=250 left=120 removed by Anita on 31-05-2007
                MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", "Description must be entered in order to Save.", MessageBoxButtons.OK, False)
                l_boolean_MessageFlag = True
            End If
            If l_boolean_MessageFlag = False Then
                For l_integer_ItemCount = 0 To Me.DataGridGroupProps.Items.Count - 1
                    Dim l_labelCode As Label
                    Dim l_DropAccess As DropDownList

                    l_labelCode = CType(Me.DataGridGroupProps.Items(l_integer_ItemCount).Cells(0).FindControl("lblItemCode"), Label)
                    l_DropAccess = CType(Me.DataGridGroupProps.Items(l_integer_ItemCount).Cells(2).FindControl("DropDownGridAccess"), DropDownList)

                    l_string_SecuredItems = l_string_SecuredItems + l_labelCode.Text.ToString.Trim()

                    If l_DropAccess.SelectedItem.Value = 1 Then

                        l_string_Access = l_string_Access + "1"

                    ElseIf l_DropAccess.SelectedItem.Value = 8 Then

                        l_string_Access = l_string_Access + "8"

                    Else

                        l_string_Access = l_string_Access + "9"

                    End If

                    l_string_SecuredItems = l_string_SecuredItems + ","
                    l_string_Access = l_string_Access + ","
                Next
                'Priya : 30-March-2010:BT-468:Shows error page Cast from string “” to type 'Double' is not valid.
                'ConvertSession value to string
                If Convert.ToString(Session("Session_GroupId")) = "" Or Session("Add_Flag") = True Then
                    Dim err_output As Integer
                    err_output = YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.InsertGroupDetails(Me.TextBoxGroup.Text.Trim(), Me.TextboxDesc.Text.Trim(), l_string_SecuredItems, l_string_Access)
                    If err_output = 1 Then
                        'last parameter of Show method set to false and top=250 left=120 removed  by Anita on 31-05-2007
                        MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", "Group """ + Me.TextBoxGroup.Text.Trim + """ Already on File.", MessageBoxButtons.OK, False)
                        Me.ButtonSave.Enabled = True
                        Me.ButtonCancel.Enabled = True
                    Else
                        l_ds_GrpKey = YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.GetAddedGroupKey(Me.TextBoxGroup.Text.Trim())
                        Session("Session_GroupId") = l_ds_GrpKey.Tables(0).Rows(0)("Group Key")
                        Me.ButtonMembers.Enabled = True
                        Me.ButtonDelete.Enabled = True
                        'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
                        'Me.ButtonCancel.Enabled = False
                        'Me.ButtonSave.Enabled = False
                        MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", "Saved", MessageBoxButtons.OK)
                    End If
                Else
                    YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.UpdateGroupDetails(l_integer_GroupId, Me.TextBoxGroup.Text.Trim(), Me.TextboxDesc.Text.Trim(), l_string_SecuredItems, l_string_Access)
                    Me.ButtonMembers.Enabled = True
                    Me.ButtonDelete.Enabled = True
                    'Added by Anudeep:10.09.2012 for bt-1208
                    Me.ButtonSave.Enabled = False

                    'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
                    'Me.ButtonCancel.Enabled = False
                    'Me.ButtonSave.Enabled = False
                    MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", "Saved", MessageBoxButtons.OK)
                    Session("Add_Flag") = False
                End If

                Me.ButtonOk.Enabled = True
                Session("Group_Modified") = True
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
                MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Session("Group_Del") = True
            MessageBox.Show(250, 120, PlaceHolderGroupProps, "YMCA-YRS", "Are you SURE you want to delete " + Me.TextBoxGroup.Text.Trim() + " ?", MessageBoxButtons.YesNo)
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    Public Sub DeleteGroup()
        Try
            Dim l_integer_GroupKey As Integer
            l_integer_GroupKey = CType(Session("Session_GroupId"), Integer)
            YMCARET.YmcaBusinessObject.GroupPropertiesBOClass.DeleteGroupDetails(l_integer_GroupKey)
            Session("Session_GroupId") = Nothing
            '' Code Commented by Vartika on 15th dec 2005

            ''PopulateSecuredItems()
            ''Me.TextBoxGroup.Text = ""
            ''Me.TextboxDesc.Text = ""

            '' End of Code Commented by Vartika on 15th dec 2005
            Session("Group_Del") = False
            Session("Group_Modified") = True

            '' Code added by Vartika on 15th dec 2005
            Dim closeWindow6 As String = "<script language='javascript'>" & _
                                                   "window.opener.document.forms(0).submit();self.close();" & _
                                                   "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow6")) Then
                Page.RegisterStartupScript("CloseWindow6", closeWindow6)
            End If
            '' End of Code added by Vartika on 15th dec 2005
        Catch ex As Exception
            Throw ex
        End Try
        
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            'Added by neeraj on 25-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderGroupProps, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            PopulateSecuredItems()
            'Session("Session_GroupId") = ""
            Session("Add_Flag") = True
            Me.TextBoxGroup.Text = ""
            Me.TextboxDesc.Text = ""
            Me.ButtonCancel.Enabled = True
            Me.ButtonSave.Enabled = True
            Me.ButtonPrint.Enabled = False
            Me.ButtonOk.Enabled = False
            Me.ButtonMembers.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.LabelGroupDetail.Text = "-- Add Group"
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
            Dim l_String As String
            Dim l_DataRow() As DataRow
            If Session("Session_PageId") = "Properties" Then
                
                l_String = "[Group Key] = '" & Session("Session_GroupId") & "'"
                l_DataRow = g_DataSet_dsGroups.Tables(0).Select(l_String)
                PopulateData()

                If Not IsDBNull(l_DataRow(0)("Group")) Then
                    Me.TextBoxGroup.Text = l_DataRow(0)("Group")
                End If
                If Not IsDBNull(l_DataRow(0)("Description")) Then
                    Me.TextboxDesc.Text = l_DataRow(0)("Description")
                End If
            Else
                Me.TextBoxGroup.Text = ""
                Me.TextboxDesc.Text = ""
                PopulateSecuredItems()
            End If

            'NP:2009.12.16:YRS 5.0-969 - We will always keep the buttons enabled
            'Me.ButtonCancel.Enabled = False
            'Me.ButtonSave.Enabled = False
            Me.ButtonPrint.Enabled = True
            Me.ButtonOk.Enabled = True
            Me.ButtonMembers.Enabled = True
            Me.ButtonDelete.Enabled = True
            Me.ButtonAdd.Enabled = True
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
            ''Session("Session_GroupId") = ""
            Dim popupScript As String = "<script language='javascript'>" & _
                             "window.open('GroupMembers.aspx','GrpMembersPopUp', " & _
                             "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
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

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            Session("Grp_Props_Sort") = Nothing
            Session("blnGrpOpen") = False
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
    End Sub
    'NP:2009.12.16:YRS 5.0-969 - This method is not being called from the aspx page as autopostback is being set to false to avoid postbacks
    Public Sub enableSaveCancel(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
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
    Private Sub ButtonNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNone.Click
        Try
            Dim l_DropAccess As DropDownList
            Dim l_integer_ItemCount As Integer
            For l_integer_ItemCount = 0 To Me.DataGridGroupProps.Items.Count - 1
                l_DropAccess = CType(Me.DataGridGroupProps.Items(l_integer_ItemCount).Cells(2).FindControl("DropDownGridAccess"), DropDownList)
                l_DropAccess.SelectedValue = 9
            Next
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub ButtonFullAccess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFullAccess.Click
        Try
            Dim l_DropAccess As DropDownList
            Dim l_integer_ItemCount As Integer
            For l_integer_ItemCount = 0 To Me.DataGridGroupProps.Items.Count - 1
                l_DropAccess = CType(Me.DataGridGroupProps.Items(l_integer_ItemCount).Cells(2).FindControl("DropDownGridAccess"), DropDownList)
                l_DropAccess.SelectedValue = 1
            Next
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub DataGridGroupProps_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridGroupProps.SortCommand
        Try
            Dim l_ds_grp_props_sort As DataSet
            DataGridGroupProps.SelectedIndex = -1
            If Not viewstate("Group_Prop_Sort") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_grp_props_sort = viewstate("Group_Prop_Sort")
                dv = l_ds_grp_props_sort.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Grp_Props_Sort") Is Nothing Then
                    If Session("Grp_Props_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridGroupProps.DataSource = Nothing
                Me.DataGridGroupProps.DataSource = dv
                Me.DataGridGroupProps.DataBind()
                Session("Grp_Props_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    'Neeraj : 14-Dec-2009 issueID BT1065
    Private Sub ButtonReadOnly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReadOnly.Click
        Try
            Dim l_DropAccess As DropDownList
            Dim l_integer_ItemCount As Integer
            For l_integer_ItemCount = 0 To Me.DataGridGroupProps.Items.Count - 1
                l_DropAccess = CType(Me.DataGridGroupProps.Items(l_integer_ItemCount).Cells(2).FindControl("DropDownGridAccess"), DropDownList)
                l_DropAccess.SelectedValue = 8
            Next
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub
    'issueID BT1065 :End
End Class
