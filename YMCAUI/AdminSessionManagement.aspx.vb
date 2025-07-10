'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	AdminSessionManagement.aspx.vb
' Author Name		:	Ashutosh Goswami
' Employee ID		:	cvas1257
' Email				:	Ashutosh.Goswami@3i-infotech.com
' Contact No		:	8568
' Creation Time		:	6/13/2006 12:37:46 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	
'

' Changed on	    :	12/01/2005
'****************************************************
'Modification History
'****************************************************
'Modified by            Date              Description
'****************************************************
'Neeraj Singh           12/Nov/2009       Added form name for security issue YRS 5.0-940 
'Dinesh Kanojia         03/02/2013        BT: 1860:Maintaining user login history/sessions information 
'Anudeep                29/072013         Bt-1983:Security rights not applied for UserSession.aspx
'Anudeep A              24/09/2014        BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'Manthan Rajguru        2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports System.IO
Public Class AdminSessionManagement
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AdminSessionManagement.aspx")
    'End issue id YRS 5.0-940

    Public Enum enumMessageBoxType
        Javascript = 0
        DotNet = 1
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents gvSession As System.Web.UI.WebControls.GridView
    Protected WithEvents ButtonSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonShow As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonNewSessionKill As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSendMail As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnHistory As System.Web.UI.WebControls.Button

    'Protected WithEvents chkSelectAll As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Private m_int_const_CheckboxIndex As Int16 = 0
    Private m_int_const_SessionIdIndex As Int16 = 1
    Private m_int_const_UserIdIndex As Int16 = 2
    Private m_int_const_UserNameIndex As Int16 = 3
    Private m_int_const_IPAddressIndex As Int16 = 5

    Private Property SessionPageCount() As Int32
        Get
            If Not (Session("MemberInfoPageCount")) Is Nothing Then

                Return (CType(Session("MemberInfoPageCount"), Int32))
            Else
                Return 0
            End If
        End Get

        Set(ByVal Value As Int32)
            Session("MemberInfoPageCount") = Value
        End Set
    End Property

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        If Page.IsPostBack = False Then
            Try
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                End If

                Session("ManageSessionListSort") = Nothing
                Session("ManageSessionListPageIndex") = Nothing

                '-------------------------------------------------------------------------------------------------
                'Shashi Shekhar:2009-12-31:Commented old call and added new call to eliminate the conflict of checksecurity access between server side and client side.
                ' Me.ButtonNewSessionKill.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonNewSessionKill');")
                Me.ButtonNewSessionKill.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonNewSessionKill.ID))
                '--------------------------------------------------------------------------------------------------------

                getSecuredControls()

                GetActiveUsers()

            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("AdminSessionManagement_Page_Load", ex)
            End Try
        End If
        Try
            'Code Added by ashutosh BugId-2574 for storing the ButtonNewSessionKill status in XML File
            'not in Application object on 08-Sep-2006.
            Dim ds As New DataSet
            Dim string_FilePath As String
            string_FilePath = ConfigurationSettings.AppSettings("SessionXml").ToString()
            If File.Exists(string_FilePath + "\AdminSession.xml") Then
                ds.ReadXml(string_FilePath + "\AdminSession.xml")
            End If

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows(0)(0) = "True" Then
                    ButtonNewSessionKill.Text = "Allow Login"
                Else
                    ButtonNewSessionKill.Text = "Prevent Login"
                End If
            End If
            If ds.Tables.Count = 0 Then
                ButtonNewSessionKill.Text = "Prevent Login"
            End If
            'End ******** ashutosh Code
        Catch ex As Exception
            'Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_Page_Load", ex)
        End Try
    End Sub

#Region "**************Buttons Handle Events***********"

    Private Sub ButtonShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonShow.Click
        Dim dtSessions As DataTable
        Dim drArray As DataRow()
        Dim ss As String
        Dim i As Integer
        Dim ss1 As String
        Try
            'Added by neeraj on 23-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'START Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 
            'ViewState_SortExpression = Nothing
            GetActiveUsers()
            'END Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information             

        Catch ex As Exception
            'Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_ButtonShow_Click", ex)
        End Try
    End Sub

    Private Sub ButtonSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click

        Dim chkFlag As System.Web.UI.WebControls.CheckBox
        Dim strSessionId As String
        Dim gvRow As GridViewRow
        Dim dtAllSessionId As DataTable
        Dim drArray As DataRow()
        Dim dr As DataRow
        Dim strSessionInfo As String
        Dim dtSessions As DataTable
        Dim dsGetSessionInfo As DataTable
        Try
            'Added by neeraj on 23-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            Dim boolSessionSelected As Boolean = False

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'START Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 
            If Not Application("dtAllSessionId") Is Nothing Then
                dtSessions = CType(Application("dtAllSessionId"), DataTable)
                For Each gvRow In gvSession.Rows
                    chkFlag = gvRow.FindControl("chkFlag")
                    If chkFlag.Checked Then
                        'strSessionInfo = YMCARET.YmcaBusinessObject.Login.UpdateSessionInfo(gvRow.Cells(0).Text, Convert.ToInt32(gvRow.Cells(1).Text), gvRow.Cells(2).Text, gvRow.Cells(4).Text, "Killed", DateTime.Now, DateTime.Now, Convert.ToInt32(Session("LoggedUserKey").ToString()), Convert.ToInt32(Session("LoggedUserKey").ToString()), gvRow.Cells(3).Text, True)
                        strSessionInfo = YMCARET.YmcaBusinessObject.Login.UpdateSessionInfo(gvRow.Cells(m_int_const_SessionIdIndex).Text, Convert.ToInt32(gvRow.Cells(m_int_const_UserIdIndex).Text), gvRow.Cells(m_int_const_UserNameIndex).Text, gvRow.Cells(m_int_const_IPAddressIndex).Text, "Killed", DateTime.Now, DateTime.Now, Convert.ToInt32(Session("LoggedUserKey").ToString()), Convert.ToInt32(Session("LoggedUserKey").ToString()), gvRow.Cells(3).Text, True)
                        boolSessionSelected = True
                    End If
                Next

                'HR:08/14/2014
                If boolSessionSelected = False Then
                    HelperFunctions.ShowMessageToUser("Please select a person's session to kill", EnumMessageTypes.Error)
                    Exit Sub
                End If
            End If

            dsGetSessionInfo = YMCARET.YmcaBusinessObject.Login.GetCurrentDayOnlineUserInfo().Tables(0)
            If Not dsGetSessionInfo Is Nothing Then
                'dsGetSessionInfo.DefaultView.RowFilter = "chvSessionStatus <> 'LoggedOut' "
                Application("dtAllSessionId") = dsGetSessionInfo
                gvSession.DataSource = dsGetSessionInfo
                gvSession.PageIndex = 0
                gvSession.DataBind()

            End If
            'ButtonSelectAll.Text = "Select All"
            'END Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 

            'Commented By Dinesh to get the sessioninfo from database.
            'dtAllSessionId = DirectCast(Application("dtAllSessionId"), DataTable)
            'For Each dgItem In DataGridSession.Items
            '    chkFlag = dgItem.FindControl("chkFlag")
            '    If chkFlag.Checked Then
            '        'dtAllSessionId = Nothing
            '        strSessionId = dgItem.Cells(0).Text
            '        drArray = dtAllSessionId.Select("SessionId='" & strSessionId & "'")
            '        drArray(0)("KillSession") = True
            '        Application("appOldSessionKill") = "True"
            '    End If
            'Next

            'DataGridSession.DataSource = dsGetSessionInfo
            'DataGridSession.DataBind()

        Catch ex As Exception
            'Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_ButtonSubmit_Click", ex)
        End Try
    End Sub
    Private Sub ButtonNewSessionKill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNewSessionKill.Click
        ' 'Code Added by ashutosh BugId-2574 for storing the ButtonNewSessionKill status in XML File
        'not in Application object on 08-Sep-2006.
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dc As DataColumn
        Dim dr As DataRow
        Dim string_FilePath As String
        Try
            'Added by neeraj on 23-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            string_FilePath = ConfigurationSettings.AppSettings("SessionXml").ToString()
            If File.Exists(string_FilePath + "\AdminSession.xml") Then
                ds.ReadXml(string_FilePath + "\AdminSession.xml")
            End If
            If ds.Tables.Count = 0 Then
                dc = New DataColumn("NewSessionKill")
                dt.Columns.Add(dc)
                dr = dt.NewRow()
                dr("NewSessionKill") = "True"
                dt.Rows.Add(dr)
                ds.Tables.Add(dt)
            End If
            If ButtonNewSessionKill.Text = "Prevent Login" Then
                ds.Tables(0).Rows(0)(0) = "True"
                ButtonNewSessionKill.Text = "Allow Login"
                ' Application("appNewSessionKill") = "True"
            Else
                ds.Tables(0).Rows(0)(0) = "False"
                ' Application("appNewSessionKill") = Nothing
                Application("appOldSessionKill") = Nothing
                ButtonNewSessionKill.Text = "Prevent Login"
            End If
            ds.WriteXml(string_FilePath + "\AdminSession.xml")
        Catch ex As Exception
            'Server.Transfer IS being used because response.redirect create a new session in it.

            'Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_ButtonNewSessionKill_Click", ex)
        End Try
        'End of Ashutosh Code
    End Sub
    Private Sub ButtonSendMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSendMail.Click
        Dim dtAllSessionId As DataTable
        Dim gvRow As GridViewRow
        Dim chkFlag As System.Web.UI.WebControls.CheckBox
        Dim strSessionId As String
        Dim drArray As DataRow()
        Dim SelectedUsers As New DataTable
        Try
            'Added by neeraj on 23-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            dtAllSessionId = DirectCast(Application("dtAllSessionId"), DataTable)
            SelectedUsers = dtAllSessionId.Clone

            For Each gvRow In gvSession.Rows
                chkFlag = gvRow.FindControl("chkFlag")
                If chkFlag.Checked Then
                    'strSessionId = gvRow.Cells(0).Text
                    strSessionId = gvRow.Cells(m_int_const_SessionIdIndex).Text
                    drArray = dtAllSessionId.Select("chvSessionId='" & strSessionId & "'")
                    If drArray.Length > 0 Then
                        SelectedUsers.ImportRow(drArray(0))
                        SelectedUsers.AcceptChanges()
                    End If
                End If
            Next

            If SelectedUsers.Rows.Count > 0 Then
                If SendMail(SelectedUsers) Then

                End If
            Else
                HelperFunctions.ShowMessageToUser("Please select a session record to send email to logged in person.", EnumMessageTypes.Error)
            End If

        Catch ex As Exception
            'Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_ButtonSendMail_Click", ex)
        End Try
    End Sub
    Private Function SendMail(ByVal p_datatable_SelectedUsers As DataTable) As Boolean
        Dim obj As MailUtil
        Try
            If p_datatable_SelectedUsers.Rows.Count > 0 Then
                obj = New MailUtil
                obj.FromMail = "admin@ymcaret.org"
                obj.ToMail = "AllUsers@ymcaret.org"
                obj.Subject = "Please logout of the YRS Application."
                obj.MailMessage = "Please logout of the YRS Application, as some housekeeping activity needs to be performed."
                obj.Send()


                HelperFunctions.ShowMessageToUser("Email has beed sent to the selected user(s)", EnumMessageTypes.Success)

            End If
        Catch
            Throw
        Finally
            obj = Nothing
        End Try
    End Function
    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType)
        Dim alertWindow As String
        Try
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", pstrMessage, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch
            Throw
        End Try
    End Sub
#End Region

#Region "********************Datagrid Handle Events***************"

    Private Sub gvSession_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSession.RowDataBound
        Try
            Dim strSessionId As String
            Dim strCurrentSessionId As String
            Dim chkFlag As System.Web.UI.WebControls.CheckBox

            HelperFunctions.SetSortingArrows(Session("ManageSessionListSort"), e)

            If e.Row.RowType = ListItemType.Header Or e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                'strSessionId = e.Row.Cells(0).Text
                strSessionId = e.Row.Cells(m_int_const_SessionIdIndex).Text

                e.Row.Cells(1).Attributes.Add("style", "display:none")
                e.Row.Cells(2).Attributes.Add("style", "display:none")

                strCurrentSessionId = Session.SessionID.ToString()
                If strSessionId = strCurrentSessionId Then
                    'e.Row.Cells(2).ForeColor = System.Drawing.Color.Red
                    e.Row.Cells(m_int_const_UserNameIndex).ForeColor = System.Drawing.Color.Red
                    chkFlag = e.Row.FindControl("chkFlag")
                    chkFlag.ID = "chkCurrent"
                    chkFlag.CssClass = "Current"
                    chkFlag.Enabled = False
                    e.Row.Cells(m_int_const_CheckboxIndex).Enabled = False
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_gvSession_RowDataBound", ex)
        End Try
    End Sub

    Private Sub gvSession_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSession.Sorting
        Dim dv As New DataView
        Dim SortExpression As String
        SortExpression = e.SortExpression
        Try

 
            '*************************
            dv = DirectCast((YMCARET.YmcaBusinessObject.Login.GetCurrentDayOnlineUserInfo().Tables(0)), DataTable).DefaultView
            dv.Sort = SortExpression

            HelperFunctions.gvSorting(Session("ManageSessionListSort"), e.SortExpression, dv)
            HelperFunctions.BindGrid(gvSession, dv, True)
            '*********************


        Catch ex As Exception
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message), False)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_gvSession_Sorting", ex)
            'Throw ex
        Finally
        End Try
    End Sub

    Private Sub gvSession_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSession.PageIndexChanging
        Try
            If e.NewPageIndex >= 0 Then
                Session("ManageSessionPageIndex") = e.NewPageIndex
                gvSession.PageIndex = e.NewPageIndex
                GetActiveUsers()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("AdminSessionManagement_gvSession_PageIndexChanging", ex)
        End Try
    End Sub

#End Region

#Region "******************Private Functions********************************"
    
    Public Sub getSecuredControls()
        Try
            Dim l_Int_UserId As Integer
            Dim l_String_FormName As String
            Dim l_control_Id As Integer
            Dim ds_AllsecItems As DataSet

            Dim l_int_access As Integer
            Dim l_string_controlNames As String
            Dim l_ds_ctrlNames As DataSet
            l_string_controlNames = ""
            l_Int_UserId = Convert.ToInt32(Session("LoggedUserKey"))

            'l_String_FormName = Session("FormName").ToString().Trim()
            l_String_FormName = "AdminSessionManagement.aspx"
            ds_AllsecItems = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetSecuredControlsOnForm(l_String_FormName)

            If Not ds_AllsecItems Is Nothing Then
                If Not ds_AllsecItems.Tables(0).Rows.Count = 0 Then
                    For Each dr As DataRow In ds_AllsecItems.Tables(0).Rows
                        l_control_Id = Convert.ToInt32(dr("sfc_ControlId"))
                        l_ds_ctrlNames = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetControlNames(l_control_Id)
                        l_int_access = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(l_Int_UserId, l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim())

                        If l_int_access = 0 Or l_int_access = 1 Then
                            l_string_controlNames = l_string_controlNames + l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim() + ","
                        End If

                    Next
                    Me.HiddenSecControlName.Value = l_string_controlNames
                    'Response.Write(l_string_controlNames)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region "****************Commented Code************************"

    


    'START Dinesh Kanojia  03/02/2013 BT: 1860:Maintaining user login history/sessions information 
    Private Sub GetActiveUsers()
        Dim dtGetSessionInfo As New DataTable
        Try
            dtGetSessionInfo = YMCARET.YmcaBusinessObject.Login.GetCurrentDayOnlineUserInfo().Tables(0)
            If Not dtGetSessionInfo Is Nothing Then
                'dsGetSessionInfo.DefaultView.RowFilter = "chvSessionStatus <> 'LoggedOut' "
                Application("dtAllSessionId") = dtGetSessionInfo

                '*******************************
                Dim dv As New DataView
                Dim Sorting As GridViewCustomSort

                dv = dtGetSessionInfo.DefaultView()

                If Session("ManageSessionListSort") IsNot Nothing Then
                    Sorting = Session("ManageSessionListSort")
                    dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
                End If

                If Session("ManageSessionListSort") IsNot Nothing Then
                    gvSession.PageIndex = Session("ManageSessionPageIndex")
                End If

                HelperFunctions.BindGrid(gvSession, dv, True)
                'gvSession.DataSource = dtGetSessionInfo
                'gvSession.DataBind()
                '*********************************

                Session("GetSessionInfo") = dtGetSessionInfo
                'ButtonSelectAll.Text = "Select All"
            End If
        Catch ex As Exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'ENDS Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 

#End Region

    'START Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 
    Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHistory.Click
        'Anudeep:29.07.2013 - Bt-1983:Security rights not applied for UserSession.aspx
        Try
            Response.Redirect("SecurityCheck.aspx?Form=UserSession.aspx")
        Catch
            Throw
        End Try
    End Sub
    'END Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 
End Class
