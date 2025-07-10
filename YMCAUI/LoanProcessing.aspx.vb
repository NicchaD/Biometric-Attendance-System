'Shubhrata - Modified On Jan 25th 2007 Reason YREN-3035 to entail email functionality
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Ashutosh Patil     23-Apr-2007     YREN-3028,YREN-3029 
'Ashutosh Patil     22-May-2007     Change in Email Functionality
'Priya              16-March-2009   Change Time of Customer Service Department 8:45 AM to 6:00 previously it was 8:45 AM to 8:00 as per Hafiz mail on March 13, 2009 
'Priya              17-March-2009   Made changes into MailMessage, message selected from database as per hafiz mail on 16-March-2009
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Imran              3-Dec-2009      Check total of deductions are less than of loan's  amount.
'Anudeep            10-sep-2013     BT:2185:Address and telephone issue
'Anudeep            10-sep-2013     YRS 5.0-2168:address being re-inserted upon save of any update
'Anudeep A          08-sep-2014     BT:2625 :YRS 5.0-2405-Consistent screen header sections
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2018.05.30      YRS-AT-3936 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for TD Loan "Request Processing" (TrackIT 33024)
'Vinayan C			2018.11.22		YRS-AT-4018 -  YRS enh: EFT Loans Project: “Update” YRS Maintenance: Person: Loan Tab 
'********************************************************************************************************************************
Imports YMCAObjects.MetaMessageList
Public Class LoanProcessing
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("LoanProcessing.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelLoanRequest As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonLoanRequestYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonLoanRequestNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelAddress As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonAddressYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonAddressNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelSpousalWaiver As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonSpousalWaiverYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonSpousalWaiverNo As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonAddress As System.Web.UI.WebControls.Button
    'Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAccountBalance As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAccountBalance As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTDAvailable As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTDAvailable As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRequestAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxRequestAmount As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DatagridDeductions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvDeductions As System.Web.UI.WebControls.GridView
    Protected WithEvents LabelLoanAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLoanAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFees As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFees As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonProcess As System.Web.UI.WebControls.Button
    'Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYes As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNo As System.Web.UI.WebControls.Label
    Protected WithEvents AddressWebUserControl1 As AddressUserControl
    'Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextboxAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextboxAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxzip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxCountry As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxCity1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TexBoxState As System.Web.UI.WebControls.TextBox
    'START: MMR | 2018.05.30 | YRS-AT-3936 | Declared controls
    Protected WithEvents lblPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents txtPaymentMethod As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblBankName As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAccountType As System.Web.UI.WebControls.Label
    Protected WithEvents txtAccountType As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblBankABA As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankABA As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblBankAccountNumber As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankAccountNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents trBankName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trAccountType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trBankABA As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trBankAccountNumber As System.Web.UI.HtmlControls.HtmlTableRow
    'END: MMR | 2018.05.30 | YRS-AT-3936 | Declared controls

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Form Properties"
    'To Keep the Selected PersonID 
    Private Property SessionPersonID() As String
        Get
            If Not (Session("PersonID")) Is Nothing Then
                Return (CType(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property

    'To Keep the FundID for Selected Member
    Private Property SessionFundID() As String
        Get
            If Not (Session("FundID")) Is Nothing Then
                Return (CType(Session("FundID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FundID") = Value
        End Set
    End Property

    Private Property SessionDisbID() As String
        Get
            If Not (Session("DisbID")) Is Nothing Then
                Return (CType(Session("DisbID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("DisbID") = Value
        End Set
    End Property

    Private Property SessionLoanRequestId() As Integer
        Get
            If Not (Session("LoanRequestId")) Is Nothing Then
                Return (CType(Session("LoanRequestId"), Integer))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("LoanRequestId") = Value
        End Set
    End Property
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim LabelTitle As Label
        Try
            If Not IsPostBack Then
                'Shubhrata YREN 2699
                Session("ProcessClick") = Nothing
                'Shubhrata YREN 2699
                LabelTitle = Master.FindControl("lblPopupmodulename")
                If Not Session("Title") Is Nothing And LabelTitle IsNot Nothing Then
                    LabelTitle.Text = Session("Title")
                End If
                LoadDeductions()
                'anita and shubhrata
                If Not Session("ds_PrimaryAddress") Is Nothing Then

                    Me.LoadFromPopUp()
                End If
                'anita and shubhrata
                If Not Session("AccountBalance") Is Nothing Then
                    Me.TextboxAccountBalance.Text = FormatCurrency(CType(Session("AccountBalance"), Decimal))
                End If
                If Not Session("TDBalance") Is Nothing Then
                    Me.TextboxTDAvailable.Text = FormatCurrency(CType(Session("TDBalance"), Decimal))
                End If
                If Not Session("AmountRequested") Is Nothing Then
                    Me.TextboxRequestAmount.Text = FormatCurrency(CType(Session("AmountRequested"), Decimal))
                    Me.TextboxLoanAmount.Text = FormatCurrency(CType(Session("AmountRequested"), Decimal))
                    Me.TextboxNet.Text = FormatCurrency(CType(Session("AmountRequested"), Decimal))
                End If


                If Not Session("AddressInfo") Is Nothing Then
                    Dim l_DataTable As DataTable
                    l_DataTable = CType(Session("AddressInfo"), DataTable)
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadAddressInfoToControls(l_DataTable.Rows.Item(0))
                    End If
                End If
                Me.LoadPaymentDetails() 'MMR | 2018.05.30 | YRS-AT-3936 | Loading payment details such as payment method, Account type,Bank ABA and Account Number
            End If
            If Not Session("MaritalStatus") Is Nothing Then
                If CType(Session("MaritalStatus"), String).ToUpper = "MARRIED" Then
                    Me.RadioButtonSpousalWaiverNo.Enabled = True
                    Me.RadioButtonSpousalWaiverYes.Enabled = True
                Else
                    Me.RadioButtonSpousalWaiverNo.Enabled = False
                    Me.RadioButtonSpousalWaiverYes.Enabled = False
                    Me.RadioButtonSpousalWaiverNo.Checked = True
                End If
            End If
            If IsPostBack() Then
                Dim l_integer_Counter As Integer
                Dim l_double_Deductions As Double
                For l_integer_Counter = 0 To Me.gvDeductions.Rows.Count - 1
                    Dim l_Checkbox As CheckBox
                    l_Checkbox = Me.gvDeductions.Rows(l_integer_Counter).FindControl("CheckBoxDeduction")
                    If Not l_Checkbox Is Nothing Then
                        If l_Checkbox.Checked Then
                            Dim l_Textbox As TextBox
                            l_Textbox = Me.gvDeductions.Rows(l_integer_Counter).FindControl("TextboxAmount")
                            If Not l_Textbox Is Nothing Then
                                If l_Textbox.Text.Length <> 0 Then
                                    Try
                                        l_double_Deductions = l_double_Deductions + Convert.ToDouble(l_Textbox.Text)
                                    Catch ex As Exception
                                        HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_AMOUNT_INVALID)
                                        Exit Sub
                                    End Try
                                End If
                            End If
                        End If
                    End If

                Next
                TextboxFees.Text = FormatCurrency(l_double_Deductions)
                TextboxNet.Text = FormatCurrency(Convert.ToDouble(TextboxLoanAmount.Text) - l_double_Deductions)
                'anita apr 27th
                If Not Session("ds_PrimaryAddress") Is Nothing Then

                    Me.LoadFromPopUp()
                End If
                'anita apr 27th
            End If
        Catch ex As Exception

            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_Page_load", ex)
        End Try

    End Sub
    Private Function LoadDeductions()
        Dim l_DataTable As DataTable
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetDeductions
            Me.gvDeductions.DataSource = l_DataTable
            Me.gvDeductions.DataBind()
            Session("DeductionsDataTable") = l_DataTable
        Catch ex As Exception

            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_LoadDeductions", ex)

        End Try

    End Function
    Private Function LoadAddressInfoToControls(ByVal parameterDataRow As DataRow)
        Dim drAddress As DataRow()
        Try
            If Not parameterDataRow Is Nothing Then

                drAddress = parameterDataRow.Table.Select()
                AddressWebUserControl1.LoadAddressDetail(drAddress)




            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_LoadAddressInfoToControls", ex)
        End Try

    End Function

    Private Sub ButtonProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonProcess.Click
        Try
            Dim l_string_Message As String
            Dim l_DataSet As New DataSet
            Dim l_DataTable As DataTable
            Dim l_DeductionTable As DataTable
            Dim l_DataRow As DataRow
            Dim cnt As Integer
            Dim GrossAmt As Decimal
            Dim DedAmt As Decimal
            If CheckNullTransactionRefId() = True Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_TRANSACTION_REF_MISSING_FOR_RT)
                Exit Sub
            End If
            'Added by imran on 03/12/2009  Check total of deductions are less than of loan amount.
            GrossAmt = Convert.ToDecimal(IIf(TextboxLoanAmount.Text = "", "0", TextboxLoanAmount.Text))
            For cnt = 0 To gvDeductions.Rows.Count - 1
                Dim CheckBoxDeduction As CheckBox
                CheckBoxDeduction = gvDeductions.Rows(cnt).FindControl("CheckBoxDeduction")
                If Not IsNothing(CheckBoxDeduction) Then
                    If CheckBoxDeduction.Checked Then
                        Dim l_Textbox As TextBox
                        l_Textbox = Me.gvDeductions.Rows(cnt).FindControl("TextboxAmount")
                        DedAmt += Convert.ToDecimal(l_Textbox.Text)
                    End If
                End If
            Next
            'Change condition by imran on 4/12/2009 
            If (GrossAmt - DedAmt) < 0.01 Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_NET_AMOUNT_LESSTHAN_ZERO)
                Exit Sub
            End If
            ''Dim l_DataColumn As New DataColumn
            ''l_DataColumn.ColumnName = "DisbId"
            ''l_DataColumn.DataType = DbType.String.GetType
            'Shubhrata YREN 2699
            If Session("ProcessClick") Is Nothing Then
                Session("ProcessClick") = True
                'Shubhrata YREN 2699
                Me.UpdateDisbursementWithholding()
                l_DataTable = CType(Session("DeductionsDataTable"), DataTable)

                l_DeductionTable = l_DataTable.Clone()
                ''l_DeductionTable.Columns.Add(l_DataColumn)

                For Each l_DataRow In l_DataTable.Rows
                    If l_DataRow.RowState = DataRowState.Modified Then
                        l_DeductionTable.ImportRow(l_DataRow)
                    End If
                Next
                ' If l_DeductionTable.Rows.Count > 0 Then
                l_DeductionTable.TableName = "Deductions"
                l_DataSet.Tables.Add(l_DeductionTable)
                ' End If
                l_string_Message = YMCARET.YmcaBusinessObject.LoanInformationBOClass.SaveProcessingData(Me.SessionPersonID, Me.SessionLoanRequestId, Me.SessionFundID, Convert.ToDouble(Me.TextboxLoanAmount.Text), l_DataSet)
                If l_string_Message <> "Error" Then
                    Session("ReloadLoan") = True
                    ''Shubhrata YREN 2699
                    Session("ProcessClick") = False
                    Me.ProcessReport()
                Else
                    Session("ReloadLoan") = False
                End If


                'Moved Code from  Loan Information Page to here.


                Dim msg As String

                msg = msg + "window.opener.document.forms(0).submit();"

                msg = msg + "self.close();"


                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Close", msg, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_ButtonProcess_Click", ex)

        End Try

    End Sub
    Protected Sub Text_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim TxtBox As TextBox = CType(sender, TextBox)
            Dim gvRow As GridViewRow = CType(TxtBox.NamingContainer, GridViewRow)
            Dim i As Integer = gvRow.RowIndex
            If TxtBox.Text.Trim = String.Empty Then
                TxtBox.Text = 0
            End If
            Try
                TxtBox.Text = Math.Round(Convert.ToDecimal(TxtBox.Text.Trim), 2)
            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_AMOUNT_INVALID)
                Exit Sub
            End Try

            If Convert.ToDouble(TxtBox.Text.Trim) < 0 Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_AMOUNT_NEGATIVE)
                Exit Sub
            End If


        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_Text_Changed", ex)
        End Try

    End Sub
    Private Function UpdateDisbursementWithholding()
        Dim l_DataTable_Deductions As DataTable

        Try
            l_DataTable_Deductions = CType(Session("DeductionsDataTable"), DataTable)
            If Not l_DataTable_Deductions Is Nothing Then
                Dim l_integer_Counter As Integer
                Dim l_double_Deductions As Double
                For l_integer_Counter = 0 To Me.gvDeductions.Rows.Count - 1
                    Dim l_Checkbox As CheckBox
                    l_Checkbox = Me.gvDeductions.Rows(l_integer_Counter).FindControl("CheckBoxDeduction")
                    If Not l_Checkbox Is Nothing Then
                        If l_Checkbox.Checked Then
                            Dim l_Textbox As TextBox
                            l_Textbox = Me.gvDeductions.Rows(l_integer_Counter).FindControl("TextboxAmount")
                            If Not l_Textbox Is Nothing Then
                                If l_Textbox.Text.Length <> 0 Then
                                    l_DataTable_Deductions.Rows(l_integer_Counter).Item("Amount") = l_Textbox.Text

                                End If
                            End If
                        End If
                    End If

                Next
                '  l_DataTable_Deductions.AcceptChanges()
                Session("DeductionsDataTable") = l_DataTable_Deductions
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_UpdateDisbursementWithholding", ex)

        End Try


    End Function
    Private Function EnableProcessButton()
        Try
            If (Me.RadioButtonAddressNo.Checked = True Or Me.RadioButtonAddressYes.Checked = True) And (Me.RadioButtonLoanRequestYes.Checked = True) And (Me.RadioButtonSpousalWaiverNo.Checked = True Or Me.RadioButtonSpousalWaiverYes.Checked = True) Then
                Me.ButtonProcess.Enabled = True
            Else
                Me.ButtonProcess.Enabled = False
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_EnableProcessButton", ex)
        End Try
    End Function

    Private Sub RadioButtonLoanRequestYes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonLoanRequestYes.CheckedChanged
        Try
            EnableProcessButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_RadioButtonLoanRequestYes_CheckedChanged", ex)

        End Try

    End Sub

    Private Sub RadioButtonLoanRequestNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonLoanRequestNo.CheckedChanged
        Try
            EnableProcessButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_RadioButtonLoanRequestNo_CheckedChanged", ex)

        End Try

    End Sub

    Private Sub RadioButtonAddressYes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonAddressYes.CheckedChanged
        Try
            If Me.RadioButtonAddressYes.Checked = True Then
                Me.ButtonAddress.Enabled = True
            Else
                Me.ButtonAddress.Enabled = False
            End If

            EnableProcessButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_RadioButtonAddressYes_CheckedChanged", ex)
        End Try

    End Sub

    Private Sub RadioButtonAddressNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonAddressNo.CheckedChanged
        Try
            Me.ButtonAddress.Enabled = False
            EnableProcessButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_RadioButtonAddressNo_CheckedChanged", ex)

        End Try

    End Sub

    Private Sub RadioButtonSpousalWaiverYes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonSpousalWaiverYes.CheckedChanged
        Try
            EnableProcessButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_RadioButtonSpousalWaiverYes_CheckedChanged", ex)

        End Try

    End Sub

    Private Sub RadioButtonSpousalWaiverNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonSpousalWaiverNo.CheckedChanged
        Try
            EnableProcessButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_RadioButtonSpousalWaiverNo_CheckedChanged", ex)

        End Try

    End Sub
    'anita and shubhrata
    Private Function LoadFromPopUp()
        Dim l_str_zip As String
        Dim drAddress As DataRow()
        'Function made by ashutosh 17/04/06
        If Not Session("ds_PrimaryAddress") Is Nothing Then
            Dim dataset_AddressInfo As DataSet
            Dim l_DataSet As DataSet
            Dim l_DataTable As DataTable
            Dim datarow_Row As DataRow
            Dim datarow_NewRow As DataRow
            dataset_AddressInfo = (CType(Session("ds_PrimaryAddress"), DataSet))
            'Start:Anudeep:12.09.2013-Bt:2185- Handled to not to occur error if address changed from pop-up
            If dataset_AddressInfo.Tables("Address").Rows.Count > 0 Then
                drAddress = dataset_AddressInfo.Tables("Address").Rows(0).Table.Select()
                AddressWebUserControl1.LoadAddressDetail(drAddress)
                dataset_AddressInfo = Nothing
            End If
        End If
    End Function
    'anita and shubhrata
    Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
        Try
            Dim n As String
            Dim m As String()
            Dim myNum As String
            Dim myDec As String
            Dim len As Integer
            Dim i As Integer
            Dim val As String
            If paramNumber = 0 Then
                val = 0
            Else
                n = paramNumber.ToString()
                m = (Math.Round(n * 100) / 100).ToString().Split(".")
                myNum = m(0).ToString()

                len = myNum.Length
                Dim fmat(len) As String
                For i = 0 To len - 1
                    fmat(i) = myNum.Chars(i)
                Next
                Array.Reverse(fmat)
                For i = 1 To len - 1
                    If i Mod 3 = 0 Then
                        fmat(i + 1) = fmat(i + 1) & ","
                    End If
                Next
                Array.Reverse(fmat)
                If m.Length = 1 Then
                    val = String.Join("", fmat) + ".00"
                Else
                    myDec = m(1).ToString
                    If myDec.Length = 1 Then
                        myDec = myDec + "0"
                    End If
                    val = String.Join("", fmat) + "." + myDec
                End If

            End If

            Return val

        Catch ex As Exception
            Return paramNumber
        End Try

    End Function

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Dim msg As String
            msg = msg + "self.close();"

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Close", msg, True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_ButtonOK_Click", ex)

        End Try

    End Sub

    Private Sub ButtonAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddress.Click
        Try
            'START : VC | 2018.11.22 | YRS-AT-4018 - To show error message if user tries to update address while loan is in progress
            Dim messageNumber As Integer
            messageNumber = YMCARET.YmcaBusinessObject.CommonBOClass.ValidatePIIRestrictions(Session("PersonID"))
            'If messageNumber is exist then show error message
            If HelperFunctions.isNonEmpty(messageNumber) And messageNumber <> 0 Then
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(messageNumber).DisplayText, EnumMessageTypes.Error)
                Exit Sub
            End If
            'END : VC | 2018.11.22 | YRS-AT-4018 - To show error message if user tries to update address while loan is in progress

            'anita and shubhrata 
            Session("ds_PrimaryAddress") = Nothing
            Dim popupScript As String
            popupScript = "window.open('UpdateAddressDetails.aspx', 'CustomPopUp', " & _
                                                      "'width=700, height=600, menubar=no, Resizable=no,top=50,left=120, scrollbars=yes')"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript, True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_ButtonAddress_Click", ex)
        End Try
    End Sub

    Private Sub ProcessReport()
        Dim l_StringMessage As String
        Dim l_string_ToEmailAddrs As String = ""
        Dim l_boolEmailSent As Boolean = False
        Try


            Session("strReportName") = "Loan Letter to Association"
            Session("PersID") = Me.SessionPersonID
            Session("strReportName_1") = "Loan Letter to Participant"
            Session("OpenReport") = True
            Session("LoanProcessed") = True

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_ButtonOK_Click", ex)
        End Try

    End Sub

    Private Function GetToEMailAddrs(ByVal paramterYmcaNo As String) As String
        Try
            Dim l_dataset_EMailAddrs As New DataSet
            Dim l_datarow_EMailAddrs As DataRow
            Dim l_string_EMailAddrs As String = ""
            l_dataset_EMailAddrs = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(paramterYmcaNo, 1)
            'we are passing 1 as we need only for TRANSM
            If Not l_dataset_EMailAddrs Is Nothing Then
                If l_dataset_EMailAddrs.Tables.Count > 0 Then
                    If l_dataset_EMailAddrs.Tables(0).Rows.Count > 0 Then
                        l_datarow_EMailAddrs = l_dataset_EMailAddrs.Tables(0).Rows(0)
                        If ((l_datarow_EMailAddrs("EmailAddr")).GetType.ToString <> "System.DBNull" Or (l_datarow_EMailAddrs("EmailAddr")).GetType.ToString <> "") Then
                            l_string_EMailAddrs = l_datarow_EMailAddrs("EmailAddr").ToString().Trim()
                        End If
                    End If
                End If
            End If
            Return l_string_EMailAddrs

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_GetToEMailAddrs", ex)
        End Try
    End Function

#Region "YMCA PHASEIV"
    Private Function CheckNullTransactionRefId() As Boolean
        Dim l_int_TransRefIdIsNull As Integer = 0
        Try
            l_int_TransRefIdIsNull = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CheckNullTransactionRefId(Me.SessionFundID)
            If l_int_TransRefIdIsNull = 1 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanProcessing_CheckNullTransactionRefId", ex)
        End Try
    End Function
#End Region

    'START: MMR | 2018.05.30 | YRS-AT-3936 | Loading payment details such as payment method, Account type,Bank ABA and Account Number
    ''' <summary>
    ''' 1. Displays Payment Method (EFT/CHECK) and Bank Details(Account type, Bank ABA and Account Number)
    ''' 2. Auto select Deduction checkbox based on OND Type and show select OND Amount and net amount
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadPaymentDetails()
        Dim count As Integer
        Dim deductionCheckBox As CheckBox
        Dim deductionAmountTextBox As TextBox
        Dim deductionLabel As Label

        If Not Session("PaymentMethodCode") Is Nothing Then
            'Check if payment method is 'EFT', then display bank details such as account type, Bank ABA and Account Number
            If Session("PaymentMethodCode") = YMCAObjects.PaymentMethod.EFT Then
                txtPaymentMethod.Text = Session("PaymentMethodCode")
                txtBankName.Text = Session("BankName")
                txtAccountType.Text = Session("AccountType")
                txtBankABA.Text = Session("BankABA")
                txtBankAccountNumber.Text = Session("BankAccountNumber")
                'Check if payment method is 'CHECK', then hide Bank details and display only Payment method and if OND type is there, autoselect deduction checkbox based on OND Type and show net amount by deducting OND amount from requested amount
            ElseIf Session("PaymentMethodCode") = YMCAObjects.PaymentMethod.CHECK Then
                'Hide Bank payment details
                trBankName.Style.Add("display", "none")
                trAccountType.Style.Add("display", "none")
                trBankABA.Style.Add("display", "none")
                trBankAccountNumber.Style.Add("display", "none")
                'Display Payment method
                txtPaymentMethod.Text = Session("PaymentMethodCode")
                'Auto select deduction checkbox if OND type is available
                If Not String.IsNullOrEmpty(Session("ONDType")) Then
                    For count = 0 To Me.gvDeductions.Rows.Count - 1
                        deductionLabel = gvDeductions.Rows(count).FindControl("Label1")
                        If deductionLabel.Text.ToUpper.Trim = Session("ONDType").ToString().ToUpper.Trim Then
                            deductionCheckBox = gvDeductions.Rows(count).FindControl("CheckBoxDeduction")
                            deductionCheckBox.Checked = True
                            If deductionCheckBox.Checked Then
                                deductionAmountTextBox = gvDeductions.Rows(count).FindControl("TextboxAmount")
                                'Diaplying selected OND fees from deductiongrid and net amount by deducting OND fees from requested amount
                                If Not deductionAmountTextBox Is Nothing AndAlso deductionAmountTextBox.Text.Length <> 0 Then
                                    TextboxFees.Text = FormatCurrency(Convert.ToDecimal(deductionAmountTextBox.Text))
                                    TextboxNet.Text = FormatCurrency(Convert.ToDecimal(TextboxLoanAmount.Text) - Convert.ToDecimal(TextboxFees.Text))
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        End If
    End Sub
    'END: MMR | 2018.05.30 | YRS-AT-3936 | Loading payment details such as payment method, Account type,Bank ABA and Account Number
End Class
