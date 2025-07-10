'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA YRS Maintenance
' FileName			:	YMCA_Toolbar_WebUserControl.ascx.vb
' Author Name		:	Mohammed Hafiz
' Employee ID		:	33284
' Email			    :	hafiz.rehman@3i-infotech.com
' Contact No		:	
' Creation Time	    :	26-Apr-2007
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	This control is used to make the home & logout links visible accordingly.
'                                   Since popup screens should not have these links only the parent screens should 
'                                   be having these links.
'                                   ShowLogoutLinkButton & ShowHomeLinkButton properties are used make these links 
'                                   visible or hidden
'                                   Also it is parameterised to show the servername, logged in username, application version information.
'                                   If these are not passed then the session variables will be looked upon to show these informations.
'                                   ServerName, UserName, DBName, AppVersion are properties with which information can be set to display.
'Changed Hisorty
'------------   ----------------    ---------------------------------------------------
' Date          Author              Description
'------------   ----------------    ---------------------------------------------------
' 
'*******************************************************************************
Public Class YMCA_Toolbar_WebUserControl
    Inherits System.Web.UI.UserControl
    Private m_bool_ShowHome As Boolean = True
    Private m_bool_ShowLogout As Boolean = True
    Private m_bool_Release As Boolean = False
    Private m_string_Version As String = ""
    Private m_string_DBName As String = ""
    Private m_string_ServerName As String = ""
    Private m_string_UserName As String = ""
    Protected WithEvents ImageHome As System.Web.UI.WebControls.Image
    Protected WithEvents ImageLogout As System.Web.UI.WebControls.Image
    Protected WithEvents LblUserName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelUserName As System.Web.UI.WebControls.Label
    Protected WithEvents ImageLogo As System.Web.UI.WebControls.Image
    Protected WithEvents ImageSpacer1 As System.Web.UI.WebControls.Image
    Protected WithEvents ImageSpacer2 As System.Web.UI.WebControls.Image
    Protected WithEvents ImageLine2 As System.Web.UI.WebControls.Image
    Protected WithEvents ImageLine1 As System.Web.UI.WebControls.Image
    Protected WithEvents ImageSpacer3 As System.Web.UI.WebControls.Image
    Protected WithEvents imgVersion As System.Web.UI.WebControls.Image
    Protected WithEvents dvShowVersion As System.Web.UI.HtmlControls.HtmlContainerControl
    Protected WithEvents LabelDBInfo As System.Web.UI.WebControls.Label

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        Dim TodayDate As String = String.Empty
        Dim strVersion As String = String.Empty
        Try

            'Put user code to initialize the page here
            If IsPostBack = False Then
                ImageHome.Visible = m_bool_ShowHome
                'ImageLogout.Visible = m_bool_ShowLogout
                ImageLogout.Visible = m_bool_ShowLogout

                imgVersion.Visible = m_bool_Release

                If m_bool_Release = False Then
                    dvShowVersion.Visible = False
                Else
                    dvShowVersion.Visible = True
                End If



                strVersion = "<table border='0' cellpadding='0' class='Td_WelcomeNoteContainer' cellspacing='0' width='100%'>"
                If Not Session("ServerName") Is Nothing Then
                    strVersion += "<tr><td width='45%'><b>&nbsp;Server Name: </b></td>" + "<td width='55%'>" + Session("ServerName").ToString() + "</td></tr>"
                End If

                If Not Session("DBName") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;DB Name: </b></td>" + "<td>" + Session("DBName").ToString() + "</td></tr>"
                End If

                If Not Session("Version") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;Release Version: </b></td>" + "<td>" + Session("Version").ToString() + "</td></tr>"
                End If

                If Not Session("ServiceVersion") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;Service Version: </b></td>" + "<td>" + Session("ServiceVersion").ToString() + "</td></tr>"
                End If

                If Not Session("DatabaseVersion") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;DB Script Version: </b></td>" + "<td>" + Session("DatabaseVersion").ToString() + "</td></tr>"
                End If

                If Not Session("ReleaseParentVersionID") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;Parent Version: </b></td>" + "<td>" + Session("ReleaseParentVersionID").ToString() + "</td></tr>"
                End If

                If Not Session("PatchApplyDate") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;Applied Date: </b></td>" + "<td>" + Session("PatchApplyDate").ToString() + "</td></tr>"
                End If

                If Not Session("PatchReleaseDate") Is Nothing Then
                    strVersion += "<tr><td><b>&nbsp;Release Date: </b></td>" + "<td>" + Session("PatchReleaseDate").ToString() + "</td></tr></table>"
                End If

                dvShowVersion.InnerHtml = strVersion.Trim()
                'dvShowVersion.InnerHtml = Session("ServerName") + "<br>" + Session("DBName") + "<br>" + Session("Version") + 
                '"<br>" + Session("ServiceVersion") + "<br>" + Session("DatabaseVersion") + "<br>" + 
                'Session("ReleaseParentVersionID") + "<br>" + Session("PatchApplyDate") + "<br>" + Session("PatchReleaseDate")
                If m_string_UserName <> "" Then
                    LabelUserName.Text = m_string_UserName
                Else
                    If Not Session("LoggedUserName") Is Nothing Then
                        LabelUserName.Text = Session("LoggedUserName")
                    End If
                End If
                TodayDate = Now().ToString("g")
                If m_string_ServerName <> "" And m_string_DBName <> "" And m_string_Version <> "" Then
                    LabelDBInfo.Text = m_string_DBName + " | " + m_string_ServerName + " | " + m_string_Version + " | " + TodayDate
                Else
                    If Not Session("DBName") Is Nothing And Not Session("ServerName") Is Nothing And Not Session("Version") Is Nothing Then
                        LabelDBInfo.Text = Session("DBName") + " | " + Session("ServerName") + " | " + Session("Version") + " | " + TodayDate
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
#Region "All Properties"
    Public Property ShowLogoutLinkButton() As Boolean
        Get
            Return m_bool_ShowLogout
        End Get
        Set(ByVal Value As Boolean)
            m_bool_ShowLogout = Value
        End Set
    End Property
    Public Property ShowReleaseLinkButton() As Boolean
        Get
            Return m_bool_Release
        End Get
        Set(ByVal Value As Boolean)
            m_bool_Release = Value
        End Set
    End Property
    Public Property ShowHomeLinkButton() As Boolean
        Get
            Return m_bool_ShowHome
        End Get
        Set(ByVal Value As Boolean)
            m_bool_ShowHome = Value
        End Set
    End Property
    Public Property AppVersion() As String
        Get
            Return m_string_Version
        End Get
        Set(ByVal Value As String)
            m_string_Version = Value
        End Set
    End Property
    Public Property DBName() As String
        Get
            Return m_string_DBName
        End Get
        Set(ByVal Value As String)
            m_string_DBName = Value
        End Set
    End Property
    Public Property ServerName() As String
        Get
            Return m_string_ServerName
        End Get
        Set(ByVal Value As String)
            m_string_ServerName = Value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return m_string_UserName
        End Get
        Set(ByVal Value As String)
            m_string_UserName = Value
        End Set
    End Property
#End Region

End Class
