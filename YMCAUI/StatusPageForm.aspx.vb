' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'******************************************************************************************************************************************************
'Modification History
'******************************************************************************************************************************************************
'Modified By                        Date                                    Desription
'******************************************************************************************************************************************************
'Apanra Samala                      16-May-2007                             To use in File copy utility
'Aparna Samala                      28-Jun-2007                             To use in Delinquency Letters
'Neeraj Singh                       12/Nov/2009                             Added form name for security issue YRS 5.0-940 
'Anudeep A                          24/sep/2014                             BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'******************************************************************************************************************************************************

Public Class StatusPageForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("StatusPageForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonHome As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents LabelProcessStatus As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxProcessStatus As System.Web.UI.WebControls.TextBox

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
        Dim LabelModuleName As Label
        Try
            LabelModuleName = Master.FindControl("LabelModuleName")
            If LabelModuleName IsNot Nothing Then
                LabelModuleName.Text = "Status of the Process"
            End If
            'Put user code to initialize the page here
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Dim l_string_FormType As String
            Dim l_string_message As String
            If Not Request.QueryString.Get("FormType") Is Nothing Then
                l_string_FormType = Request.QueryString.Get("FormType")
            Else
                l_string_FormType = ""
            End If
            'by Aparna YREN-3197 16/05/2007 -To use for Copy Files Utility
            ' Me.LabelProcessStatus.Text = "Status of the Month End Process on " + System.DateTime.Now().ToString()
            If Request.QueryString("CopyFile") = 1 Then
                Me.LabelProcessStatus.Text = "Status of the File Transfer Process on " + System.DateTime.Now().ToString()
                'by Aparna -for delinquency Letters -28/06/2007
            ElseIf Request.QueryString("CopyFile") = 2 Then
                Me.LabelProcessStatus.Text = "Please contact System Administrator."
            ElseIf Request.QueryString.Get("ProcessType") = "MonthEndInterest" Then
                Me.LabelProcessStatus.Text = "Status of the Month End Process on " + System.DateTime.Now().ToString()
            ElseIf Request.QueryString.Get("ProcessType") = "DailyInterest" Then
                Me.LabelProcessStatus.Text = "Status of the Daily Intrest Process on " + System.DateTime.Now().ToString()
            End If

            l_string_message = Server.UrlDecode(Request.QueryString("Message"))
            Me.LabelProcessStatus.Visible = True
            Me.TextBoxProcessStatus.Visible = True
            Me.TextBoxProcessStatus.Text = l_string_message
            'ButtonClose.Visible = True
            ButtonHome.Visible = True
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("StatusPageForm_Page_Load", ex)
        End Try
    End Sub

    Private Sub ButtonHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonHome.Click
        Try
            Response.Redirect("MainWebForm.aspx", True)
        Catch
            Throw
        End Try
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Try
            Dim closeWindow1 As String = "window.close();"
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "CloseWindow1", closeWindow1, True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("StatusPageForm_ButtonClose_Click", ex)
        End Try

    End Sub

End Class
