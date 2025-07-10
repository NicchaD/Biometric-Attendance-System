'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Swopna            18 Jan,2008      YRPS-4461
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Imports System.Data.SqlClient
Public Class ErrorForSession
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBoxMess As System.Web.UI.WebControls.TextBox
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
        Try
            'Response.Clear()
            Dim l_string_FormType As String
            Dim l_string_message As String
            Dim ss As String
            Dim strDefaultMess = "Your Session has been terminated  by Administrator. Please try logging in after some time."

            If Not Request.QueryString.Get("FormType") Is Nothing Then
                l_string_FormType = Request.QueryString.Get("FormType")
            Else
                l_string_FormType = ""
            End If

            'ss= Your Session has been terminated  by Administrator"
            TextBoxMess.Text = strDefaultMess
            If Not Request.QueryString("Message") = Nothing Then
                ss = Request.QueryString("Message")
                If ss.IndexOf("Thread") = -1 Then
                    TextBoxMess.Text = ss
                End If
                If ss = "Prevent Login" Then
                    'Added by Swopna on 18 Jan,2008 in response to YRPS-4461
                    '****************
                    Dim l_DataSet As DataSet
                    Dim l_DataTable As DataTable
                    Dim l_DataRow As DataRow
                    Dim l_stringKey As String = "ADMIN_APP_LOCK_REASON"

                    l_DataSet = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.SearchConfigurationMaintenance(l_stringKey)

                    l_DataTable = l_DataSet.Tables(0)

                    If Not l_DataTable Is Nothing Then
                        TextBoxMess.Text = CType(l_DataTable.Rows(0)("Description"), String).Trim
                    End If

                    '****************
                    'TextBoxMess.Text = "Application is currently locked for month-end process.Please try logging in after some time."
                End If
            End If
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
            'Added by Swopna on 18 Jan,2008 in response to YRPS-4461
            '****************
        Catch SqlEx As SqlException
            If SqlEx.Number = 60006 Then
                TextBoxMess.Text = "Application is currently locked for month-end process.Please try logging in after some time."
                ButtonHome.Visible = True
            End If
            '****************
        Catch ex As Exception
            
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Session.Abandon()
    End Sub

    Private Sub ButtonHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonHome.Click
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
End Class
