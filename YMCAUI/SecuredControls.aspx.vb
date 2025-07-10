'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
' Hafiz                         04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Public Class SecuredControls
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SecuredControls.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents PlaceHolderSecuredControls As System.Web.UI.WebControls.PlaceHolder

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
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        If Not Me.IsPostBack Then
            checkAccess()
        Else
            If Request.Form("OK") = "OK" Then
                'Response.Redirect("MainWebForm.aspx", False)
                Exit Sub
            End If
        End If
    End Sub
    Public Sub checkAccess()
        Try
            Dim l_integer_AccPermission As Integer
            Dim l_String_ControlName As String
            Dim l_integer_UserId As Integer

            l_integer_UserId = Convert.ToInt32(Session("LoggedUserKey"))

            l_String_ControlName = Convert.ToString(Request.QueryString("ControlName"))

            l_integer_AccPermission = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(l_integer_UserId, l_String_ControlName)
            If l_integer_AccPermission = 0 Then
                MessageBox.Show(PlaceHolderSecuredControls, "YMCA-YRS", "You do not have access for this activity.", MessageBoxButtons.Stop)
            ElseIf l_integer_AccPermission = 2 Then

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
