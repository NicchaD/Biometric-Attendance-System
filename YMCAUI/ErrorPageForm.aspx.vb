'*******************************************************************************
'Modification History
'****************************************************
'Modified by          Date          Description
'****************************************************
'Vipul                04Feb06       Cache-Session 
'Nikunj Patel         2009.04.28    Removing Login check from the error page
'Priya                2010-06-30    Changes made for enhancement in vs-2010 
'Anudeep A            24-sep-2014   BT:2625 :YRS 5.0-2405-Consistent screen header sections
'*******************************************************************************

Public Class ErrorPageForm
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonHome As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents LabelDBError As System.Web.UI.WebControls.Label
    Protected WithEvents LinkButtonErrorDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TextBoxErrorMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents LinkbuttonHideDetails As System.Web.UI.WebControls.LinkButton


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
        Dim l_string_FormType As String
        Dim l_string_message As String

        l_string_FormType = ""
        l_string_message = ""
        Dim LabelHeader As Label
        Try

            LabelHeader = Master.FindControl("LabelModuleName")
            If LabelHeader IsNot Nothing Then
                LabelHeader.Text = "Error Page"
            End If

            If Not Request.QueryString.Get("FormType") Is Nothing Then
                l_string_FormType = Request.QueryString.Get("FormType")
            Else
                l_string_FormType = ""
            End If

            If Not Request.QueryString.Get("DBMessage") Is Nothing Then
                l_string_message = Server.UrlDecode(Request.QueryString("DBMessage"))

                Me.LabelDBError.Visible = True
                Me.LabelDBError.Text = "Database Error : Please contact the DataBase Administrator."
            Else
                l_string_message = Server.UrlDecode(Request.QueryString("Message"))

                Me.LabelDBError.Visible = True
                Me.LabelDBError.Text = "Network Error : Please contact the Administrator."
            End If

            If l_string_message Is Nothing Then

                Dim ex As Exception
                ex = CType(Session("ExceptionObject"), Exception)

                Me.LabelDBError.Visible = True
                Me.LabelDBError.Text = Server.HtmlEncode(ex.Message.ToString())

                If Not ex.StackTrace Is Nothing Then
                    l_string_message = ex.StackTrace.ToString()
                End If

            End If

            'if there are no details/stack trace of the error occured then by default hide the text box displayed on this page.
            If l_string_message Is Nothing Then
                Me.TextBoxErrorMessage.Visible = False
            Else

                'otherwise display details of error & let the textbox be visible by default.
                Me.TextBoxErrorMessage.Visible = True
                Me.TextBoxErrorMessage.Text = l_string_message
            End If

            'set the visibility of button / links on the error page based 
            'on the type of page (parent / child - popup) originated the error.
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

        Catch
            Throw
        End Try
    End Sub

    Private Sub ButtonHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonHome.Click
        Try

            Response.Redirect("MainWebForm.aspx")

        Catch
            Throw
        End Try
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Try

            Dim closeWindow1 As String = "<script language='javascript'>" & _
                                                         "window.close()" & _
                                                         "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow1")) Then
                Page.RegisterStartupScript("CloseWindow1", closeWindow1)
            End If

        Catch
            Throw
        End Try
    End Sub

    Private Sub LinkButtonErrorDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonErrorDetails.Click
        Try
            Me.TextBoxErrorMessage.Visible = True
        Catch
            Throw
        End Try
    End Sub

    Private Sub LinkbuttonHideDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkbuttonHideDetails.Click
        Try
            Me.TextBoxErrorMessage.Visible = False
        Catch
            Throw
        End Try
    End Sub
End Class
