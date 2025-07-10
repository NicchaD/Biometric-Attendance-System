'Change History
'******************************************************************************

'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'******************************************************************************

Public Class LockBoxExportToExcel
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("LockBoxExportToExcel.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid

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
        If Page.IsPostBack = False Then

            ExportToPopUp()
        End If
    End Sub
    Private Sub ExportToPopUp()
        Dim dtExport As DataTable


        If Not Session("dtLockBoxExport") Is Nothing Then

            Try

                'Response.Clear()
                Response.Buffer = True
                dtExport = CType(Session("dtLockBoxExport"), DataTable)
                DataGrid1.DataSource = dtExport
                DataGrid1.DataBind()
                Session("dtLockBoxExport") = Nothing


                Response.ContentType = "Application/x-msexcel"""
                ' Response.ContentType = "text/csv"
                Response.AddHeader("Content-Disposition", "attachment;filename=LockBox.csv")
                ' Response.AddHeader("Content-Disposition", "filename.csv")
                ' Response.ContentType = "text/csv"
                Response.Charset = ""
                Me.EnableViewState = False
                Dim oStringWriter As New System.IO.StringWriter
                Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
                DataGrid1.RenderControl(oHtmlTextWriter)
                Response.Write(oStringWriter.ToString())
            Catch ex As Exception
                Dim l_String_Exception_Message As String
                l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
                Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)



            End Try
            Response.End()

        End If

    End Sub
End Class
