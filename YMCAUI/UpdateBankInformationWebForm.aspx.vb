'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session   
'NP/PP/SR                       2009.05.18          Optimizing the YMCA Screen
'Sanjay Rawat                   2009.06.02          Optimizing the Catch Block
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya                          2010-06-03          Changes made for enhancement in vs-2010 
'Shashank Patel                 2014.10.13          BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class UpdateBankInformationWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateBankInformationWebForm.aspx")
    'End issue id YRS 5.0-940

    Dim g_bool_AddFlagBankInfo As Boolean
    Dim g_dataset_dsPaymentMethod As DataSet
    Dim g_dataset_dsAccountType As DataSet
    Dim g_BankId As String
    Dim Page_Mode As String = String.Empty
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PopCalendar1 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Dim l_ds_datasetBankInfo As DataSet


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelBankName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonBanks As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelBankABANo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAccountNumber As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAccountNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListPaymentMethod As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents LabelAccuntType As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownAccountType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxABANumber As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring 
#Region "Properties"

    '1. Define Property 
    Public Property Session(sName As String) As Object
        Get
            Return MyBase.Session(Me.UniqueSessionId + sName)
        End Get
        Set(value As Object)
            MyBase.Session(Me.UniqueSessionId + sName) = value
        End Set
    End Property

    ' 2. UniqueSession-forMultiTabs
    Public ReadOnly Property UniqueSessionId As String
        Get
            If Request.QueryString("UniqueSessionID") = Nothing Then
                Return String.Empty
            Else
                Return Request.QueryString("UniqueSessionID").ToString()
            End If

        End Get
    End Property
#End Region
    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring 
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim InsertRowPaymentMethod As DataRow
        Dim InsertRowAccountType As DataRow
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.TextBoxEffectiveDate.RequiredDate = True
        Try
            If Not Me.IsPostBack Then
                Dim Id As String
                If Not Request("BankId") Is Nothing Then
                    Id = Request("BankId").Trim()
                End If

                Dim b As Bank = GetbankById(Id)
                If b Is Nothing Then
                    Throw New Exception("Invalid Bank Id passed. Please close this window and try again.")
                End If
                g_dataset_dsPaymentMethod = YMCARET.YmcaBusinessObject.UpdateBankInformationBOClass.LookUpPaymentMethod()
                'adding an empty row dynamically
                InsertRowPaymentMethod = g_dataset_dsPaymentMethod.Tables(0).NewRow()
                InsertRowPaymentMethod.Item("chvCodevalue") = String.Empty
                InsertRowPaymentMethod.Item("chvShortDescription") = String.Empty
                g_dataset_dsPaymentMethod.Tables(0).Rows.Add(InsertRowPaymentMethod)
                Me.DropDownListPaymentMethod.DataSource = g_dataset_dsPaymentMethod
                Me.DropDownListPaymentMethod.DataMember = "Payment Method"
                Me.DropDownListPaymentMethod.DataTextField = "chvShortDescription"
                Me.DropDownListPaymentMethod.DataValueField = "chvCodevalue"
                Me.DropDownListPaymentMethod.DataBind()

                g_dataset_dsAccountType = YMCARET.YmcaBusinessObject.UpdateBankInformationBOClass.LookUpAccountType()
                'adding an empty row dynamically
                InsertRowAccountType = g_dataset_dsAccountType.Tables(0).NewRow()
                InsertRowAccountType.Item("chvCodeValue") = String.Empty
                InsertRowAccountType.Item("chvShortDescription") = String.Empty
                g_dataset_dsAccountType.Tables(0).Rows.Add(InsertRowAccountType)
                Me.DropdownAccountType.DataSource = g_dataset_dsAccountType
                Me.DropdownAccountType.DataMember = "Account Type"
                Me.DropdownAccountType.DataTextField = "chvShortDescription"
                Me.DropdownAccountType.DataValueField = "chvCodeValue"
                Me.DropdownAccountType.DataBind()
                'if add item button is clicked on Bank's tab              
                Me.TextBoxBankName.Text = b.Name.Trim
                Me.TextBoxABANumber.Text = b.ABANumber.Trim
                Me.TextBoxAccountNumber.Text = b.AccountNumber.Trim
                Me.DropDownListPaymentMethod.SelectedValue = b.PaymentMethod.Trim()
                Me.DropdownAccountType.SelectedValue = b.AccountType.Trim()
                Me.TextBoxEffectiveDate.Text = b.EffectiveDate.Trim
                g_BankId = b.BankId

                If Id Is Nothing OrElse Id = String.Empty Then
                    Page_Mode = "ADD"
                Else
                    Page_Mode = "UPDATE"
                End If
            Else
                If Not IsNothing(Session("Sel_BankName")) Then
                    Me.TextBoxBankName.Text = CType(Session("Sel_BankName"), String).Trim
                    Session("Sel_BankName") = Nothing
                End If
                If Not IsNothing(Session("Sel_BankABANumber")) Then
                    Me.TextBoxABANumber.Text = CType(Session("Sel_BankABANumber"), String).Trim
                    Session("Sel_BankABANumber") = Nothing
                End If
                If Not IsNothing(Session("Sel_SelectBank_GuiUniqueiD")) Then
                    g_BankId = Session("Sel_SelectBank_GuiUniqueiD")
                    Session("Sel_SelectBank_GuiUniqueiD") = Nothing
                End If
            End If
            Session("SelectBank") = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
            End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim msg As String = ""
        Dim objBank As New Bank

        Try
            If Me.DropDownListPaymentMethod.SelectedValue = String.Empty Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Payment Method must be entered in order to Save.", MessageBoxButtons.OK)
                Exit Sub
            End If

            objBank.Name = Me.TextBoxBankName.Text
            objBank.ABANumber = Me.TextBoxABANumber.Text
            objBank.AccountNumber = Me.TextBoxAccountNumber.Text
            objBank.PaymentMethod = Me.DropDownListPaymentMethod.SelectedValue
            objBank.AccountType = Me.DropdownAccountType.SelectedValue
            objBank.EffectiveDate = Me.TextBoxEffectiveDate.Text
            objBank.BankId = g_BankId
            objBank.Id = IIf(Request("BankId") Is Nothing, String.Empty, Request("BankId"))

            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "BANK"
            objPopupAction.Action = PopupResult.ActionTypes.ADD
            objPopupAction.State = objBank
            Session("PopUpAction") = objPopupAction


            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
            End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String = ""
        Try
            g_bool_AddFlagBankInfo = CType(Session("BoolAddFlagBankInfo"), Boolean)
            If g_bool_AddFlagBankInfo = True Then
                Session("Sel_SelectBank_GuiUniqueiD") = Nothing
            End If

            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "BANK"
            objPopupAction.Action = PopupResult.ActionTypes.CANCEL
            objPopupAction.State = Nothing
            Session("PopUpAction") = objPopupAction

            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
        End Try
    End Sub

    Private Sub ButtonBanks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBanks.Click
        Try
            Dim popupScript As String = "<script language='javascript'>" & "window.open('SelectBank.aspx?UniqueSessionID=" + UniqueSessionId + "', 'YMCAYRS1', " & "'width=720, height=450, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
                Page.RegisterStartupScript("PopupScriptRR", popupScript)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
        End Try
    End Sub

#Region "Persistence Mechanism"
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        Page_Mode = ViewState("Page_Mode")
        g_BankId = ViewState("Sel_SelectBank_GuiUniqueiD")
    End Sub
    Protected Overrides Function SaveViewState() As Object
        ViewState("Page_Mode") = Page_Mode
        ViewState("Sel_SelectBank_GuiUniqueiD") = g_BankId
        Return MyBase.SaveViewState()
    End Function
#End Region

    Private Function GetbankById(ByVal bankId As String) As Bank
        Dim n As Bank
        If bankId = Nothing OrElse bankId = String.Empty Then
            n = New Bank
            n.EffectiveDate = String.Empty
            n.ABANumber = String.Empty
            n.AccountNumber = String.Empty
            n.BankId = String.Empty
            n.Name = String.Empty
            n.PaymentMethod = String.Empty
            n.Id = String.Empty
            n.AccountType = String.Empty
            Return n
        End If
        If Session("YMCA BankInfo") Is Nothing Then Return Nothing
        Dim ds As DataSet = DirectCast(Session("YMCA BankInfo"), DataSet)
        If HelperFunctions.isEmpty(ds) Then Return Nothing
        Dim dr As DataRow()
        dr = ds.Tables(0).Select("guiUniqueId = '" & bankId & "'")
        If dr Is Nothing OrElse dr.Length = 0 Then Return Nothing
        n = New Bank
        'NPTODO: Handle Null values here
        n.Id = IIf(dr(0).IsNull("guiUniqueID"), String.Empty, dr(0).Item("guiUniqueID"))
        n.EffectiveDate = IIf(dr(0).IsNull("Effective Date"), String.Empty, String.Format("{0:MM/dd/yyyy}", Date.Parse(dr(0).Item("Effective Date"))))
        n.ABANumber = IIf(dr(0).IsNull("Bank ABA#"), String.Empty, dr(0).Item("Bank ABA#"))
        n.AccountNumber = IIf(dr(0).IsNull("Account #"), String.Empty, dr(0).Item("Account #"))
        n.AccountType = IIf(dr(0).IsNull("Account Type"), String.Empty, dr(0).Item("Account Type"))
        n.Name = IIf(dr(0).IsNull("Name"), String.Empty, dr(0).Item("Name"))
        n.PaymentMethod = IIf(dr(0).IsNull("Payment Method"), String.Empty, dr(0).Item("Payment Method"))
        n.BankId = IIf(dr(0).IsNull("guiBankID"), String.Empty, dr(0).Item("guiBankID"))

        n.Id = n.Id.Trim
        n.EffectiveDate = n.EffectiveDate.Trim
        n.ABANumber = n.ABANumber.Trim
        n.AccountNumber = n.AccountNumber.Trim
        n.AccountType = n.AccountType.Trim
        n.Name = n.Name.Trim
        n.PaymentMethod = n.PaymentMethod.Trim
        n.BankId = n.BankId.Trim

        Return n
    End Function

End Class
