'****************************************************
'Modification History
'*******************************************************************
'Modified by                    Date                Description
'*******************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************
Imports YMCARET.YmcaBusinessObject
Public Class SafeHarbor
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SafeHarbor.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents placeDolderMessageBox As System.Web.UI.WebControls.PlaceHolder

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
        Try

            'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Dim l_bool_flg As Boolean
        l_bool_flg = False
        If Not IsPostBack Then
            Dim l_string_Return As String
            Dim l_obj_SafeHarborDisbursmentBOClass As New SafeHarborDisbursmentBOClass
            l_string_Return = l_obj_SafeHarborDisbursmentBOClass.CreateDisbursementBL("SHIRA")
            If l_string_Return = ".F." Then
                MessageBox.Show(placeDolderMessageBox, "Safe Harbour", "There is no pending cashout SHIRA Request to Process", MessageBoxButtons.OK, False)
            End If
        Else

        End If

        If Request.Form("OK") = "OK" Then
            MessageBox.Show(placeDolderMessageBox, "Safe Harbour", "Safe Harbour Disbursemenst are complete", MessageBoxButtons.OK, False)
            Response.Redirect("MainWebForm.aspx", False)
        End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

End Class
