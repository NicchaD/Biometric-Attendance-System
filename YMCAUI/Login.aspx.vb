'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	login.aspx.vb
' Author Name		:	
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	
' Changed by			:	
' Changed on			:	
' Change Description	:	

'*******************************************************************************
'Changed By         Changes on              Description
'*******************************************************************************
'Priya              13-April-2010           As per Hafiz mail send on:12April-2010 :Issues identified with 7.4.2 code release
'Neeraj Singh       06/jun/2010             Enhancement for .net 4.0
'Neeraj Singh       07/jun/2010             review changes done
'Deven              16-Aug-2010             Added logic for directly showing YMCA detail when user comes
'                                           from another web site
'Dineshk            02/03/2013              BT: 1861:Maintaining release versions in a database
'Anudeep            18/04/2013              BT: 1967-YRS 5.0-2026: Spelling mistakes in YRS.
'Anudeep            18/11/2013              BT: 2302-Modify login messages
'Manthan Rajguru    2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N           2019.03.08              YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'*******************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

'Imports System.Security
'Imports System.Threading


Public Class Login
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelUserId As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPassword As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPassword As System.Web.UI.WebControls.TextBox
    'Protected WithEvents OKImageButton As System.Web.UI.WebControls.ImageButton
    Protected WithEvents OKButton As System.Web.UI.WebControls.Button
    'Protected WithEvents ResetImageButton As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ResetButton As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderLogin As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
    'Anudeep:18.11.2013-BT:2302-Commented below line becasue message should be call from div
    'Protected WithEvents vldtrCustom As System.Web.UI.WebControls.CustomValidator
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_ds_UserCredentials As DataSet
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Me.LabelPassword.AssociatedControlID = Me.TextBoxPassword.ID
        Me.LabelUserId.AssociatedControlID = Me.TextBoxUserID.ID

        'Dim Cache As CacheManager
        Dim l_int32_devmode As Int32

        Try

            'If Not IsNothing(Request.QueryString("msg")) Then
            '    If Request.QueryString("msg") = "2" Then
            '        lblMsg.Text = "Your session is expired please re-login again."
            '    End If
            'End If

            If Me.IsPostBack = False Then
                ' Cache = CacheFactory.GetCacheManager()
                'Cache.Flush()
                Session.Clear()
                Session.Abandon()
                l_int32_devmode = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings("DEVMode"))
                Dim SetFocus As String

                If l_int32_devmode = 0 Then

                    'Dim aID As Principal.WindowsIdentity
                    Dim startpos As Int32
                    Dim endpos As Int32

                    'Dim userNamewithDomain As String = aID.GetCurrent.Name
                    Dim userNamewithDomain As String = Page.User.Identity.Name

                    Dim userName As String
                    startpos = 0
                    startpos = userNamewithDomain.IndexOf("\")
                    userName = userNamewithDomain.Remove(0, startpos + 1)
                    Me.TextBoxUserID.Text = userName

                    SetFocus = "<script language='javascript'>" & _
                                            "document.Form1.all.TextBoxPassword.focus();" & _
                                            "</script>"



                Else

                    SetFocus = "<script language='javascript'>" & _
                                            "document.Form1.all.TextBoxUserID.focus();" & _
                                            "</script>"

                End If

                Page.RegisterStartupScript("PopupScriptFocus", SetFocus)

            End If

        Catch ex As Exception
            Throw ex
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)

        End Try

    End Sub


    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Dim l_bool_msgFlag As Boolean
        ' Dim l_Cache As CacheManager
        Dim l_string_DBPWD As String
        Dim l_int32_devmode As Int32
        Dim dsGetServerDBVersion As DataSet

        Try

            l_bool_msgFlag = False

            If Me.TextBoxUserID.Text.Trim = "" And Me.TextBoxPassword.Text.Trim = "" And l_bool_msgFlag = False Then
                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                'commented following code to show message from validation summary
                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "Login Id Field Cannot Be Blank.", MessageBoxButtons.Stop)
                CustomMessage(GetMessageFromResource("LOGIN_USERID_BLANK") + "<br/>" + GetMessageFromResource("LOGIN_PASSWORD_BLANK"))
                l_bool_msgFlag = True
            ElseIf Me.TextBoxUserID.Text.Trim = "" And l_bool_msgFlag = False Then
                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                'commented following code to show message from validation summary
                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "Login Id Field Cannot Be Blank.", MessageBoxButtons.Stop)
                CustomMessage(GetMessageFromResource("LOGIN_USERID_BLANK"))
                l_bool_msgFlag = True
            ElseIf Me.TextBoxPassword.Text.Trim = "" And l_bool_msgFlag = False Then
                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                'commented following code to show message from validation summary
                CustomMessage(GetMessageFromResource("LOGIN_PASSWORD_BLANK"))
            Else

                'l_Cache = CacheFactory.GetCacheManager()

                Try
                    l_int32_devmode = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings("DEVMode"))
                    l_string_DBPWD = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("DBUserPassword")).Trim()

                    If l_int32_devmode = 0 Then
                        Try

                            'Dim aID As Principal.WindowsIdentity
                            Dim startpos As Int32
                            Dim endpos As Int32

                            'Dim userNamewithDomain As String = aID.GetCurrent.Name
                            Dim userNamewithDomain As String = Page.User.Identity.Name
                            Dim userName As String
                            startpos = 0
                            startpos = userNamewithDomain.IndexOf("\")
                            userName = userNamewithDomain.Remove(0, startpos + 1)

                            'compare input name with domain name
                            'If userName.Trim().ToUpper() <> Me.TextBoxUserID.Text.Trim().ToUpper() Then
                            '    'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                            '    'commented following code to show message from validation summary
                            '    'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "User """ + Me.TextBoxUserID.Text.Trim + """. Mismatch on the application and windows user.", MessageBoxButtons.Stop)
                            '    CustomMessage(String.Format(GetMessageFromResource("LOGIN_USER_MISMATCH_APPLICATION_WINDOWS"), Me.TextBoxUserID.Text.Trim))
                            '    Me.TextBoxPassword.Text = ""
                            '    Exit Sub

                            'End If
                            'Priya: 13-April-2010: :As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
                            ' Authenticate the User In Ad Server
                            'If function return 1 then LDAP_Path is not defined
                            'If function return 2 then LDAP_Domain is not defined
                            'If function return 3 then LDAP_Path is null
                            'If function return 4 then LDAP_Domain is null
                            'If function return 5 then not authenticated user
                            'If function return 0 then authenticated 
                            'If AuthenticateUser() = False Then
                            Dim int_ErrorNo As Integer = 0

                            int_ErrorNo = AuthenticateUser()

                            'to display error message based on output of AuthenticateUser method
                            If int_ErrorNo = 1 Then
                                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                                'commented following code to show message from validation summary
                                ' MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "LDAP_Path key not defined in web.config file. Please contact administrator.", MessageBoxButtons.Stop)
                                CustomMessage(GetMessageFromResource("LOGIN_LDAP_PATH_NOT_DEFINED"))
                            ElseIf int_ErrorNo = 2 Then
                                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                                'commented following code to show message from validation summary
                                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "LDAP_Domain key not defined in web.config file. Please contact administrator.", MessageBoxButtons.Stop)
                                CustomMessage(GetMessageFromResource("LOGIN_LDAP_DOMAIN_NOT_DEFINED"))
                            ElseIf int_ErrorNo = 3 Then
                                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                                'commented following code to show message from validation summary
                                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "LDAP_Path key value not specified in web.config file. Please contact administrator.", MessageBoxButtons.Stop)
                                CustomMessage(GetMessageFromResource("LOGIN_LDAP_PATH_NOT_SPECIFIED"))
                            ElseIf int_ErrorNo = 4 Then
                                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                                'commented following code to show message from validation summary
                                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "LDAP_Domain key value not specified in web.config file. Please contact administrator.", MessageBoxButtons.Stop)
                                CustomMessage(GetMessageFromResource("LOGIN_LDAP_DOMAIN_NOT_SPECIFIED"))
                            ElseIf int_ErrorNo = 6 Then
                                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                                'commented following code to show message from validation summary
                                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "InValid LDAP_Path key value in web.config file. Please contact administrator.", MessageBoxButtons.Stop)
                                CustomMessage(GetMessageFromResource("LOGIN_LDAP_PATH_INVALID"))

                            ElseIf int_ErrorNo = 5 Then
                                'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                                'commented following code to show message from validation summary
                                'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "Windows login failed for the user """ + Me.TextBoxUserID.Text.Trim + """. Please renter your password or contact administrator.", MessageBoxButtons.Stop)
                                CustomMessage(String.Format(GetMessageFromResource("LOGIN_WINDOWS_LOGIN_FAILED"), Me.TextBoxUserID.Text.Trim))
                                Me.TextBoxPassword.Text = ""
                            End If

                            If int_ErrorNo <> 0 Then Exit Sub

                        Catch ex As Exception
                            'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                            'commented following code to show message from validation summary
                            'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "Windows login failed for the user """ + Me.TextBoxUserID.Text.Trim + """. Please renter your password or contact administrator.", MessageBoxButtons.Stop)
                            CustomMessage(String.Format(GetMessageFromResource("LOGIN_WINDOWS_LOGIN_FAILED"), Me.TextBoxUserID.Text.Trim))
                            Me.TextBoxPassword.Text = ""
                            Exit Sub
                        End Try
                    End If

                    ' l_Cache.Add("LoggedUserKey", Me.TextBoxUserID.Text.Trim.ToUpper())
                    'l_Cache.Add("LoggedUserpwd", l_string_DBPWD.Trim())
                    'START Dinesh Kanojia         03/02/2013 BT: 1861:Maintaining release versions in a database
                    dsGetServerDBVersion = YMCARET.YmcaBusinessObject.Login.GetServerDBVersion()
                    Session("ServerName") = dsGetServerDBVersion.Tables(0).Rows(0)("ServerName").ToString()
                    Session("DBName") = dsGetServerDBVersion.Tables(0).Rows(0)("DatabaseName").ToString()
                    Session("Version") = dsGetServerDBVersion.Tables(0).Rows(0)("ReleaseVersion").ToString()
                    Session("ServiceVersion") = dsGetServerDBVersion.Tables(0).Rows(0)("ServiceVersion").ToString()
                    Session("DatabaseVersion") = dsGetServerDBVersion.Tables(0).Rows(0)("DatabaseVersion").ToString()
                    Session("ReleaseParentVersionID") = dsGetServerDBVersion.Tables(0).Rows(0)("ReleaseParentVersionID").ToString()
                    Session("PatchApplyDate") = dsGetServerDBVersion.Tables(0).Rows(0)("PatchApplyDate").ToString()
                    Session("PatchReleaseDate") = dsGetServerDBVersion.Tables(0).Rows(0)("PatchReleaseDate").ToString()
                    'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                    'To get the message from resource file
                    'Session("LoggedUserName") = "You are logged in as " + Me.TextBoxUserID.Text.Trim.ToUpper()
                    'AA:18.11.2013-BT:2302- Commented below line and added a line because already "<li>" has been removed while getting message
                    'Session("LoggedUserName") = String.Format(GetMessageFromResource("LOGIN_LOGGEDIN_USER").Replace("<li>", "").Replace("</li>", ""), Me.TextBoxUserID.Text.Trim.ToUpper())
                    Session("LoggedUserName") = String.Format(GetMessageFromResource("LOGIN_LOGGEDIN_USER"), Me.TextBoxUserID.Text.Trim.ToUpper())
                    Session("LoginId") = Me.TextBoxUserID.Text.Trim
                    g_ds_UserCredentials = YMCARET.YmcaBusinessObject.Login.LookUpUserCredentialsForLogin(Me.TextBoxUserID.Text)

                    'END Dinesh Kanojia         03/02/2013 BT: 1861:Maintaining release versions in a database
                Catch ex As Exception
                    'If ex.Message.Substring(0, 38) = "Mismatch on the windows and login user" Then
                    'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "User """ + Me.TextBoxUserID.Text.Trim + """ Mismatch on the Application and windows user.", MessageBoxButtons.Stop)
                    'Else

                    CustomMessage(ex.Message.ToString())
                    Exit Sub
                    'End If

                End Try

                'g_ds_UserCredentials = YMCARET.YmcaBusinessObject.Login.LookUpUserCredentialsForLogin(Me.TextBoxUserID.Text)
                If g_ds_UserCredentials.Tables(0).Rows.Count = 0 And l_bool_msgFlag = False Then
                    'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                    'commented following code to show message from validation summary
                    'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "User """ + Me.TextBoxUserID.Text.Trim + """ does not exist.", MessageBoxButtons.Stop)
                    CustomMessage(String.Format(GetMessageFromResource("LOGIN_USER_DOES_NOT_EXIST"), Me.TextBoxUserID.Text.Trim))
                Else
                    If l_int32_devmode = 1 Then
                        If g_ds_UserCredentials.Tables(0).Rows(0)("Logged_User_Password").ToString.ToUpper.Trim = Me.TextBoxPassword.Text.ToUpper.Trim Then
                            Session("LoggedUserKey") = g_ds_UserCredentials.Tables(0).Rows(0)("Logged_User_Key").ToString.Trim
                            Session("LoggedUserGroup") = g_ds_UserCredentials.Tables(1).Rows(0)("ugr_group").ToString.Trim      'Shilpa N | 03/08/2019 | YRS-AT-4248 | Added session for user role to check YRS read only mode
                            '*****Code Added by ashutosh on 24-04-06******************

                            If AddRow() Then
                                '************************************************************
                                'Start: Changed by Deven to directly display Person or YMCA detail screen, 16-Aug-2010
                                If Not Request.QueryString("PageType") Is Nothing Then
                                    'Response.Redirect("YRSLandingPage.aspx?" + Convert.ToString(Request.QueryString), False)
                                    Response.Redirect(System.Configuration.ConfigurationSettings.AppSettings("YRSLandingPage") + "?" + Convert.ToString(Request.QueryString), False)

                                Else
                                    Response.Redirect("MainWebForm.aspx", False)
                                End If
                                'End:
                            Else
                                ' -- DONT DO ANYTHING 
                            End If
                        Else
                            'Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
                            'commented following code to show message from validation summary
                            'MessageBox.Show(PlaceHolderLogin, "YMCA-YRS", "Invalid password entered", MessageBoxButtons.Stop)
                            CustomMessage(GetMessageFromResource("LOGIN_PASSWORD_INVALID"))
                        End If

                    Else
                        Session("LoggedUserKey") = g_ds_UserCredentials.Tables(0).Rows(0)("Logged_User_Key").ToString.Trim
                        Session("LoggedUserGroup") = g_ds_UserCredentials.Tables(1).Rows(0)("ugr_group").ToString.Trim      'Shilpa N | 03/08/2019 | YRS-AT-4248 | Added session for user role to check YRS read only mode
                        '*****Code Added by ashutosh on 24-04-06******************

                        If AddRow() Then
                            '************************************************************
                            'Start: Changed by Deven to directly display Person or YMCA detail screen, 16-Aug-2010
                            If Not Request.QueryString("PageType") Is Nothing Then
                                'Response.Redirect("YRSLandingPage.aspx?" + Convert.ToString(Request.QueryString), False)
                                Response.Redirect(System.Configuration.ConfigurationSettings.AppSettings("YRSLandingPage") + "?" + Convert.ToString(Request.QueryString), False)
                            Else
                                Response.Redirect("MainWebForm.aspx", False)
                            End If
                            'End:
                        Else
                            ' -- DONT DO ANYTHING 
                        End If


                    End If

                End If

                If Me.TextBoxUserID.Text.Trim.ToUpper.Equals("ADMIN") Then
                    '*****Code Added by ashutosh on 24-04-06******************

                    If AddRow() Then
                        '************************************************************
                        Response.Redirect("MainWebForm.aspx", False)
                    Else
                        ' -- DONT DO ANYTHING 
                    End If
                End If
            End If

        Catch ex As Threading.ThreadAbortException
            'Response.Write("hello")
        Catch ex As Exception
            Throw ex
        Finally
            'dsGetServerDBVersion.Dispose()
        End Try

    End Sub

    Private Sub ResetButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResetButton.Click
        Try
            Me.TextBoxUserID.Text = ""
            Me.TextBoxPassword.Text = ""
        Catch ex As Exception
            Throw ex

            'g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
            'Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    '*****Code Added by ashutosh on 24-04-06******************
    Private Function AddRow() As Boolean
        Dim dtSessions As DataTable
        Dim dr As DataRow
        Dim objUser As New UserInfo
        Dim string_FilePath As String
        Dim ds As New DataSet
        Dim strSessionInfo As String
        Try
            'Code Added by ashutosh BugId-2574 for storing the ButtonNewSessionKill status in XML File
            'not in Application object on 08-Sep-2006.

            string_FilePath = ConfigurationSettings.AppSettings("SessionXml").ToString()
            If File.Exists(string_FilePath + "\AdminSession.xml") Then
                ds.ReadXml(string_FilePath + "\AdminSession.xml")
            End If
            If ds.Tables.Count > 0 Then
                'If Not ds.Tables(0) Is Nothing Then

                If ds.Tables(0).Rows(0)("NewSessionKill") = "True" Then
                    Dim dsGroupMembers As DataSet
                    Dim drArray As DataRow() 'Code added by ashutosh on 16-June-06 for sesion management Prevent Login
                    dsGroupMembers = YMCARET.YmcaBusinessObject.Login.GetGroupMembers("Super Admin")
                    'check for group member
                    If Not dsGroupMembers Is Nothing Then
                        If dsGroupMembers.Tables(0).Rows.Count > 0 Then
                            drArray = dsGroupMembers.Tables(0).Select("Group_Member_Code=" & Session("LoggedUserKey") & "")
                            If drArray.Length = 0 Then
                                IsNewSessionKill()
                                AddRow = False
                                Exit Function
                            End If
                        Else
                            IsNewSessionKill()
                            AddRow = False
                            Exit Function
                        End If
                    Else
                        IsNewSessionKill()
                        AddRow = False
                        Exit Function
                    End If
                End If
            End If
            'End If
            'If Application("appNewSessionKill") = "True" Then
            '    Dim dsGroupMembers As DataSet
            '    Dim drArray As DataRow() 'Code added by ashutosh on 16-June-06 for sesion management Prevent Login
            '    dsGroupMembers = YMCARET.YmcaBusinessObject.Login.GetGroupMembers("Super Admin")
            '    If Not dsGroupMembers Is Nothing Then
            '        If dsGroupMembers.Tables(0).Rows.Count > 0 Then
            '            drArray = dsGroupMembers.Tables(0).Select("Group_Member_Code=" & Session("LoggedUserKey") & "")
            '            If drArray.Length = 0 Then
            '                IsNewSessionKill()
            '            End If
            '        Else
            '            IsNewSessionKill()
            '        End If
            '    Else
            '        IsNewSessionKill()
            '    End If
            'End If
            'End of Code By Ashutosh 08-Sep-2006
            'END Dinesh Kanojia         03/02/2013 BT: 1861:Maintaining release versions in a database
            If Not Application("dtAllSessionId") Is Nothing Then
                dtSessions = CType(Application("dtAllSessionId"), DataTable)
                dr = dtSessions.NewRow()
                dr("chvSessionId") = Session.SessionID.ToString()
                dr("intUserId") = Convert.ToInt32(Session("LoggedUserKey")) 'Me.TextBoxUserID.Text.Trim()
                dr("HostName") = objUser.HostName
                dr("chvIpAddress") = objUser.HostAddress
                dr("KillSession") = False
                dr("dtmLoggedOn") = System.DateTime.Now.Date.ToShortDateString()
                dtSessions.Rows.Add(dr)

                strSessionInfo = YMCARET.YmcaBusinessObject.Login.AddSessionInfo(Session.SessionID.ToString(), Convert.ToInt32(Session("LoggedUserKey").ToString()), Me.TextBoxUserID.Text.Trim(), objUser.HostAddress, "LoggedIn", DateTime.Now, DateTime.Now, Convert.ToInt32(Session("LoggedUserKey").ToString()), Convert.ToInt32(Session("LoggedUserKey").ToString()), objUser.HostName, False)

                AddRow = True
            Else
                AddRow = False
            End If
            'END Dinesh Kanojia         03/02/2013 BT: 1861:Maintaining release versions in a database
        Catch ex As Exception
            Throw ex
        Finally
            'dtSessions.Dispose()
            objUser = Nothing
            'ds.Dispose()
        End Try

    End Function

    Sub IsNewSessionKill()
        Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Prevent Login"), False)
        'Server.Transfer("ErrorForSession.aspx?Message=" + Server.UrlEncode("Prevent Login"), False)
    End Sub
    '***********************************************************************Ashutosh*****
    'Priya: 13-April-2010: :As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
    'If function return 1 then LDAP_Path is not defined
    'If function return 2 then LDAP_Domain is not defined
    'If function return 3 then LDAP_Path is null
    'If function return 4 then LDAP_Domain is null
    'If function return 5 then not authenticated user 
    'If function return 6 then LDAP_Path is not valid
    'If function return 0 then authenticated
    Function AuthenticateUser() As Integer
        Dim adobjvalidate As New YMCARET.YmcaBusinessObject.ADHelper
        Dim l_string_LDAP_Domain As String = String.Empty
        Dim l_string_LDAP_Path As String = String.Empty
        ' Dim l_string_array_LDAP() As String
        Dim l_string_array_LDAP As New ArrayList
        Dim int_ReturnValue As Integer
        Dim str_LDAP_Path As String

        Try

            If Not IsNothing(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path")) Then
                l_string_LDAP_Path = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path")).Trim()
            Else
                int_ReturnValue = 1 'AuthenticateUser = False  'No LDAPPath is defined
                Return int_ReturnValue
            End If


            If l_string_LDAP_Path = String.Empty Then
                int_ReturnValue = 3 'AuthenticateUser = False  'No LDAPPath  value is not specified
                Return int_ReturnValue
            End If

            ' l_string_array_LDAP = l_string_LDAP_Path.Split(";")
            For Each s As String In l_string_LDAP_Path.Split(";")
                If (s <> String.Empty) Then

                    If (System.Text.RegularExpressions.Regex.IsMatch(s, "^(LDAP?://)?((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){3}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})$")) = True Then
                        l_string_array_LDAP.Add(s)
                    Else
                        int_ReturnValue = 6 'LDAP_Path is not in proper format
                        Return int_ReturnValue
                    End If
                End If

            Next

            If l_string_array_LDAP.Count <= 0 Then
                int_ReturnValue = 3   'AuthenticateUser = False  'No LDAPPath is defined
            Else

                If IsNothing(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Domain")) Then
                    int_ReturnValue = 2 'If LDAP_Domain is not defined in web.config
                    Return int_ReturnValue
                End If
                l_string_LDAP_Domain = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Domain")).Trim()

                If l_string_LDAP_Domain = String.Empty Then
                    int_ReturnValue = 4 'If LDAP_Domain is Empty
                    Return int_ReturnValue
                End If


                Dim j As Integer
                For j = 0 To l_string_array_LDAP.Count - 1
                    If l_string_array_LDAP(j).Trim() = String.Empty Then
                        int_ReturnValue = 3   'If LDAP_Path is Empty
                        Return int_ReturnValue
                    End If



                    If adobjvalidate.IsAuthenticated(Convert.ToString(l_string_array_LDAP(j).Trim()), l_string_LDAP_Domain.Trim(), Me.TextBoxUserID.Text.Trim(), Me.TextBoxPassword.Text.Trim()) Then
                        int_ReturnValue = 0 'Authenticate user
                        Return int_ReturnValue
                    Else
                        int_ReturnValue = 5 ' user is not authenticate
                    End If
                Next
            End If
            Return int_ReturnValue
        Catch ex As Exception
            Throw ex

            'Return 5 ' user is not authenticate
            'g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
            ''Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Finally
            adobjvalidate = Nothing
            l_string_array_LDAP = Nothing
        End Try

    End Function

    Function AuthenticateUser1() As Boolean
        Dim l_string_LDAP_Path As String
        Dim l_string_LDAP_Domain As String
        'Priya:12-April-2010:As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
        'Priya: 12-April-2010: Assign default value as string.empty to string variable
        l_string_LDAP_Path = String.Empty
        Dim adobjvalidate As New YMCARET.YmcaBusinessObject.ADHelper
        Try

            'check for path LDAP_Path1
            'Priya:12-April-2010:As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
            'Priya: 12-April-2010: Check whether LDAP_Path1 is exists itno wen.config or not
            If Not IsNothing(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path1")) Then
                l_string_LDAP_Path = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path1")).Trim()
            End If
            l_string_LDAP_Domain = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Domain")).Trim()
            If adobjvalidate.IsAuthenticated(l_string_LDAP_Path, l_string_LDAP_Domain.Trim(), Me.TextBoxUserID.Text.Trim(), Me.TextBoxPassword.Text.Trim()) Then
                AuthenticateUser1 = True
                Exit Function
            End If

            l_string_LDAP_Path = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path2")).Trim()
            l_string_LDAP_Domain = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Domain")).Trim()
            If adobjvalidate.IsAuthenticated(l_string_LDAP_Path, l_string_LDAP_Domain.Trim(), Me.TextBoxUserID.Text.Trim(), Me.TextBoxPassword.Text.Trim()) Then
                AuthenticateUser1 = True
                Exit Function
            End If

            l_string_LDAP_Path = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path3")).Trim()
            l_string_LDAP_Domain = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Domain")).Trim()
            If adobjvalidate.IsAuthenticated(l_string_LDAP_Path, l_string_LDAP_Domain.Trim(), Me.TextBoxUserID.Text.Trim(), Me.TextBoxPassword.Text.Trim()) Then
                AuthenticateUser1 = True
                Exit Function

            End If

            l_string_LDAP_Path = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Path4")).Trim()
            l_string_LDAP_Domain = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("LDAP_Domain")).Trim()
            If adobjvalidate.IsAuthenticated(l_string_LDAP_Path, l_string_LDAP_Domain.Trim(), Me.TextBoxUserID.Text.Trim(), Me.TextBoxPassword.Text.Trim()) Then
                AuthenticateUser1 = True
            Else
                AuthenticateUser1 = False
            End If

        Catch ex As Exception
            Throw ex

            'g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
            'Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Finally
            adobjvalidate = Nothing
        End Try
    End Function
    'Start:Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
    'To get all the messages from resource file
    Public Function GetMessageFromResource(ByVal strMessagekey As String) As String
        Dim strMessagevalue As String
        Try
            Try
                strMessagevalue = GetGlobalResourceObject("LoginMessages", strMessagekey).ToString()
            Catch
                strMessagevalue = "There is no message withrespect to given key " + strMessagekey
            End Try
            'AA:18.11.2013-BT:2302- Commented below line and added a return statement to display message without Bullets
            'Return "<li>" + strMessagevalue + "</li>"
            Return strMessagevalue
        Catch
            Throw
        End Try
    End Function
    'To show the message in custom validator
    Public Function CustomMessage(ByVal strMessage As String)
        'Anudeep:18.11.2013-BT:2302-Commented below line becasue message should be call from div 
        'vldtrCustom.IsValid = False
        'vldtrCustom.ErrorMessage = strMessage
        'Anudeep:18.11.2013-BT:2302-Added below line to display message from div in login pages
        HelperFunctions.ShowMessageToUser(strMessage, EnumMessageTypes.Error, Nothing)
    End Function
    'End:Anudeep:18.04.2013-BT:1967-YRS 5.0-2026: Spelling mistakes in YRS.
End Class
