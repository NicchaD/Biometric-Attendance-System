'Modification History
'*******************************************************************************************************************************************************************************************************************
'Modified by                    Date                Description
'*******************************************************************************************************************************************************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************************************************************************************************************************************************


Public Class ACHDebitExportUpdateForm
    Inherits System.Web.UI.Page

    Dim strFormName As String = New String("ACHDebitExportUpdateForm.aspx")


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelYMCAName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCAName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPaymentDate As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxPaymentDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYMCANo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxYMCANo As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents TextboxOldPaymentDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPaymentDate As YMCAUI.DateUserControl
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim guiuniqueid As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        TextBoxPaymentDate.Attributes.Add("onkeypress", "javascript:IsValidDate('TextBoxPaymentDate');")

        Me.TextBoxAmount.Attributes.Add("onkeypress", "javascript:return ValidateDecimal(this.value);")
        ' Me.TextBoxAmount.Attributes.Add("onkeypress", "javascript:HandleAmountFiltering(this);")

        'Me.TextBoxAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

        Me.TextBoxPaymentDate.RequiredDate = True
        If Not IsPostBack Then
            guiuniqueid = Session("UniqueId")
            TextboxYMCANo.Text = Session("YmcaNo")
            TextBoxYMCAName.Text = Session("YmcaName")

            TextBoxAmount.Text = Session("Amount")
            TextBoxPaymentDate.Text = Session("PaymentDate")

        End If
        guiuniqueid = Session("UniqueId")
        'Session("NewAmount") = TextBoxAmount.Text
        ' Session("NewPaymentDate") = TextBoxPaymentDate.Text



    End Sub
    Private Sub UpdateACHDebit()



        Dim l_output As Integer
        Dim NewAmount As Double
        Dim NewPaymentDate As String
        'TextBoxAmount.Text = String.Format("{c}", TextBoxAmount.Text)
        Session("NewAmount") = Convert.ToDouble(TextBoxAmount.Text)
        Session("NewPaymentDate") = TextBoxPaymentDate.Text
        If Not Session("NewAmount") Is Nothing Then


            If Not Session("NewPaymentDate") Is Nothing Then

                If Session("NewAmount") <> 0 Then
                    If Session("NewAmount") <= 999999999999.99 Then

                        NewAmount = Session("NewAmount")
                        NewPaymentDate = Session("NewPaymentDate")
                        l_output = YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.UpdateACHDebits(guiuniqueid, NewAmount, NewPaymentDate)
                        Session("l_output") = l_output
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub ButtonUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdate.Click
        Try
            UpdateACHDebit()
            Dim msg As String

            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.document.forms(0).submit();"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        'Dim l_cancel As String
        ' l_cancel = Session("cancel")
        Session("cancel") = True
        Dim msg As String
        msg = msg + "<Script Language='JavaScript'>"

        msg = msg + "window.opener.document.forms(0).submit();"

        msg = msg + "self.close();"

        msg = msg + "</Script>"
        Response.Write(msg)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        'If Session("cancel") = False Then
        Session("l_counter") = 0
        ' End If

    End Sub

    Private Sub TextBoxAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxAmount.TextChanged
        'If TextBoxAmount.Text.Length <= 14 Then
        '    TextBoxAmount.Text = TextBoxAmount.Text
        'End If
    End Sub
End Class
