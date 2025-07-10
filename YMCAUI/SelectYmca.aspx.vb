'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Public Class SelectYmca
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SelectYmca.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridYMCAMetro As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelYMCANo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label

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

    End Sub

    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            Dim l_DataSet_Metro As DataSet
            ' If Session("Ymca") = "Metro" Then
            l_DataSet_Metro = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SearchYMCAMetro(TextBoxYMCANo.Text, TextBoxName.Text, TextBoxCity.Text, TextBoxState.Text)
            Me.DataGridYMCAMetro.DataSource = l_DataSet_Metro
            Me.DataGridYMCAMetro.DataBind()
            'Else
            'l_DataSet_Metro = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SearchYMCABranchOnCriteria(TextBoxYMCANo.Text, TextBoxName.Text, TextBoxCity.Text, TextBoxState.Text, Session("UniqueId"))
            'Me.DataGridYMCAMetro.DataSource = l_DataSet_Metro
            'Me.DataGridYMCAMetro.DataBind()
            ' End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DataGridYMCAMetro_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridYMCAMetro.SelectedIndexChanged
        Try
            'If Session("Ymca") = "Metro" Then
            Session("UniqueIdM") = Me.DataGridYMCAMetro.SelectedItem.Cells(3).Text.ToString.Trim
            Session("YmcaNo") = Me.DataGridYMCAMetro.SelectedItem.Cells(1).Text.ToString.Trim
            Session("YmcaDesc") = Me.DataGridYMCAMetro.SelectedItem.Cells(2).Text.ToString.Trim
            Session("CallFlag") = "False"
            'Dim closeWindow As String = "<script language='javascript'>" & _
            '                                        "self.close();" & _
            '                                        "</script>"

            'If (Not Me.IsStartupScriptRegistered("CloseWindow")) Then
            '    Page.RegisterStartupScript("CloseWindow", closeWindow)
            'End If
            ' Else
            'Session("UniqueIdB") = Me.DataGridYMCAMetro.SelectedItem.Cells(3).Text.ToString.Trim
            'Session("YmcaNoB") = Me.DataGridYMCAMetro.SelectedItem.Cells(1).Text.ToString.Trim
            'Session("YmcaDescB") = Me.DataGridYMCAMetro.SelectedItem.Cells(2).Text.ToString.Trim
            'Dim closeWindow As String = "<script language='javascript'>" & _
            '                                        "self.close();window.opener.location.reload();" & _
            '                                        "</script>"

            'If (Not Me.IsStartupScriptRegistered("CloseWindow")) Then
            '    Page.RegisterStartupScript("CloseWindow", closeWindow)
            'End If

            'End If
            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.document.forms(0).submit();"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DataGridYMCAMetro_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYMCAMetro.ItemDataBound
        Try
            e.Item.Cells(3).Visible = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DataGridYMCAMetro_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridYMCAMetro.PageIndexChanged

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Session("CallFlag") = "False"
            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Throw

        End Try

    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            TextBoxCity.Text = ""
            TextBoxName.Text = ""
            TextBoxState.Text = ""
            TextBoxYMCANo.Text = ""
            DataGridYMCAMetro.Visible = False
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
