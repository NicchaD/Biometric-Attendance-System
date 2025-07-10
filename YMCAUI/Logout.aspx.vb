'*******************************************************************************
'Modification History 
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Public Class Logout
    Inherits System.Web.UI.Page

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
        'Put user code to initialize the page here
        'Dim CloseWindow As String

        'Dineshk            02/03/2013              BT: 1860:Maintaining user login history/sessions information 
        Dim strSessionInfo As String
        Dim dtSessions As DataTable
        If Not Application("dtAllSessionId") Is Nothing Then
            dtSessions = CType(Application("dtAllSessionId"), DataTable)
            If Not Session("LoggedUserKey") Is Nothing Then
                strSessionInfo = YMCARET.YmcaBusinessObject.Login.UpdateSessionInfo(dtSessions.Rows(0)("chvSessionId").ToString(), Convert.ToInt32(Session("LoggedUserKey").ToString()), Session("LoggedUserKey").ToString(), dtSessions.Rows(0)("chvIpAddress").ToString(), "LoggedOut", DateTime.Now, DateTime.Now, Convert.ToInt32(Session("LoggedUserKey").ToString()), Convert.ToInt32(Session("LoggedUserKey").ToString()), dtSessions.Rows(0)("HostName").ToString(), Convert.ToBoolean(dtSessions.Rows(0)("KillSession").ToString()))
            End If
        End If
        'Dineshk            02/03/2013              BT: 1860:Maintaining user login history/sessions information 

        Session.Abandon()

        'If Session("LoggedUserName") = "False" Then
        '    Server.Transfer("Login.aspx")
        'End If
        If Not Request.QueryString("Msg") Is Nothing Then
            Response.Redirect("Login.aspx")
            'Server.Transfer("Login.aspx")
        Else
            Response.Redirect("Login.aspx")
        End If

        'CloseWindow = "<script language='javascript'>window.close()</script>"
        'Page.RegisterStartupScript("CloseWindow", CloseWindow)

    End Sub

End Class
