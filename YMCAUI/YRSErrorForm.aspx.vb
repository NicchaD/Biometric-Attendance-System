' 2009.04.28    Nikunj Patel    Removing Login check from the error page
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 

Public Class YRSErrorForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("YRSErrorForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDBError As System.Web.UI.WebControls.Label
    Protected WithEvents LinkButtonErrorDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LinkbuttonHideDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TextBoxErrorMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonHome As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button

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
        'NP:2009.04.28 - Removing Login check from the error page
        'If Session("LoggedUserKey") Is Nothing Then
        '    Response.Redirect("Login.aspx", False)
        'End If

        If IsPostBack = False Then
            Dim l_string_FormType As String
            If Not Request.QueryString.Get("FormType") Is Nothing Then
                l_string_FormType = Request.QueryString.Get("FormType")
            Else
                l_string_FormType = ""
            End If

            Dim l_string_message As String
            Dim ex As Exception = Session("YRSErrorObject")

            Server.ClearError()

            l_string_message = "Error Occurred : " & ex.Message.ToString()

            Me.LabelDBError.Visible = True
            Me.LabelDBError.Text = l_string_message

            l_string_message = ex.StackTrace.ToString()
            Me.TextBoxErrorMessage.Text = l_string_message

            If l_string_FormType = "" Then
                ButtonClose.Visible = False
                ButtonHome.Visible = True
            ElseIf l_string_FormType = "Popup" Then
                ButtonHome.Visible = False
                ButtonClose.Visible = True
            Else
                ButtonHome.Visible = False
                ButtonClose.Visible = False
            End If

            Session("YRSErrorObject") = Nothing
        End If
    End Sub

    Private Sub ButtonHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonHome.Click
        Session("YRSErrorObject") = Nothing
        Response.Redirect("MainWebForm.aspx")
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click

        Dim closeWindow1 As String = "<script language='javascript'>" & _
                                                     "window.close()" & _
                                                     "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow1")) Then
            Page.RegisterStartupScript("CloseWindow1", closeWindow1)
        End If
    End Sub

    Private Sub LinkButtonErrorDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonErrorDetails.Click
        Me.TextBoxErrorMessage.Visible = True
    End Sub

    Private Sub LinkbuttonHideDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkbuttonHideDetails.Click
        Me.TextBoxErrorMessage.Visible = False
    End Sub
End Class
