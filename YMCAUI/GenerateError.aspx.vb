' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 

Public Class GenerateError
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("GenerateError.aspx")
    'End issue id YRS 5.0-940
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
        Dim closeWindow4 As String
        Dim l_stringalertmessage As String

        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If


            'l_stringalertmessage = "<script>alert(' " + Session("GenerateErrors").ToString().Replace("\", "\\").Trim() + " ');  </script>"
            l_stringalertmessage = "<script>alert(' " + Session("GenerateErrors").ToString.Replace("\", "\\").Replace("'", "\'") + " ');  </script>"

            Response.Write(l_stringalertmessage)

            Session("GenerateErrors") = Nothing

            closeWindow4 = "<script language='javascript'> self.close(); </script>"

            Response.Write(closeWindow4)

            ' If (Not Me.IsStartupScriptRegistered("CloseWindowErorr")) Then
            '     Page.RegisterStartupScript("CloseWindowErorr", closeWindow4)
            ' End If
        Catch ex As Exception
           
        
       End Try

    End Sub

    
End Class
