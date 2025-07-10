'*******************************************************************************
' Cache-Session     :   Vipul 04Feb06   
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'*******************************************************************************
Public Class LockBoxErrorForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("LockBoxErrorForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridLockBoxImportError As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button

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
        Try
            Dim l_dt_ErrorTable As New DataTable
            l_dt_ErrorTable = Session("Error_Table")
            Me.DataGridLockBoxImportError.DataSource = l_dt_ErrorTable
            Me.DataGridLockBoxImportError.DataBind()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Response.Redirect("ReceiptsLockBoxImportForm.aspx")
    End Sub
End Class
