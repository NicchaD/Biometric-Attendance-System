
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	DeferredPaymentDetail.aspx.vb
' Author Name		:	Ashish Srivastava 
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
'
' Designed by			:	
' Designed on			:	
'
' Changed by			:	
' Changed on			:	
' 

'*******************************************************************************

'**********************************************************************************************************************  
'** Modification History    
'**********************************************************************************************************************    
'** Modified By        Date(MM/DD/YYYY)       Issue ID                Description  
'**********************************************************************************************************************  
'Ashish Srivastva       25-Jan-2009                                   Display incorrect tax amount
'Ashish Srivastva       8-feb-2010                                    Resolve Issue BT-1109
'Manthan Rajguru        2015.09.16          YRS-AT-2550               YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*********************************************************************************************************************/ 

Public Class DeferredPaymentDetail
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    Private Sub InitializeComponent()

    End Sub

    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayee1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayee1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTaxRate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridPayee1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextboxTaxRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxablePayee1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxablePayee1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNetPayee1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayee2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPayee2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTaxable2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxable2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNet2 As System.Web.UI.WebControls.Label
    Protected WithEvents DatagridPayee2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextboxTaxablePayee2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxablePayee2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNetPayee2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents DatagridDeductions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelDeductions As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxableFinal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxableFinal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxFinal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNetFinal As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxDeductions As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxableFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxableFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNetFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents txtRemainingMBWMoney As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDeferredInstallmentAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    'Protected WithEvents CheckBoxDeduction As System.Web.UI.WebControls.CheckBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    'Declare Class level variable
    Private g_strRefRequestID As String
    Private g_intInstallmentID As Int64
    Private g_intFundIdNo As Int64
    'Private dtInstallmentList As DataTable
    'Private dtInstTransactDtl As DataTable
    Private g_dsDeferredInstList As DataSet


#Region "Private Property"

    Private Property TaxRate() As Decimal
        Get
            If Not ViewState("TaxRate") Is Nothing Then
                Return (CType(ViewState("TaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("TaxRate") = Value
        End Set
    End Property
    
    Private Property SessionDeferred_PaymentList() As DataSet
        Get
            If Not Session("Deferred_PaymentList") Is Nothing Then
                Return (CType(Session("Deferred_PaymentList"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("Deferred_PaymentList") = Value
        End Set
    End Property

#End Region
#Region "Private Function"
    Private Function LoadDeductions()

        Dim l_DataTable As DataTable
        Dim l_totalWithholding As Decimal = 0
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetDeductions
            Me.DatagridDeductions.DataSource = l_DataTable
            Me.DatagridDeductions.DataBind()

            If Not Session("dtWithHoldingDeduction") Is Nothing Then
                l_DataTable = CType(Session("dtWithHoldingDeduction"), DataTable)
                Dim l_filter As DataRow() = l_DataTable.Select("guiRefRequestID='" + g_strRefRequestID + "' AND intInstallmentID=" + g_intInstallmentID.ToString)
                If l_filter.Length > 0 Then

                    For Each itm As DataGridItem In DatagridDeductions.Items
                        Dim l_codevalue As String = itm.Cells(1).Text.Trim.ToUpper
                        For Each dr As DataRow In l_filter
                            If l_codevalue = Convert.ToString(dr("chvWithholdingTypeCode")).Trim.ToUpper Then
                                Dim l_CheckBox As CheckBox = DirectCast(itm.FindControl("CheckBoxDeduction"), CheckBox)
                                If Not l_CheckBox Is Nothing Then
                                    l_CheckBox.Checked = True
                                    l_totalWithholding += Convert.ToDecimal(dr("mnyAmount"))
                                End If
                            End If
                        Next
                    Next
                    TextboxDeductions.Text = Convert.ToString(l_totalWithholding)


                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function LoadInstallmentData()

        Dim dtDeferredPaymentList As DataTable
        Dim dtInstTransactdtl As DataTable
        Dim drFoundDiferredList As DataRow()
        Try
            If Not g_dsDeferredInstList Is Nothing Then

                If g_dsDeferredInstList.Tables.Count = 2 Then
                    dtDeferredPaymentList = g_dsDeferredInstList.Tables("dtDefPaymentList")
                    dtInstTransactdtl = g_dsDeferredInstList.Tables("dtInstallmentTransactdetail")
                    drFoundDiferredList = dtDeferredPaymentList.Select("guiRefRequestID='" + g_strRefRequestID + "' AND intInstallmentID ='" + g_intInstallmentID.ToString() + "'")
                    If drFoundDiferredList.Length > 0 Then
                        LoadPayee1Details(drFoundDiferredList(0), dtInstTransactdtl)
                        LoadPayee2Details(drFoundDiferredList(0), dtInstTransactdtl)

                        ShowData(drFoundDiferredList(0), dtInstTransactdtl)
                    End If

                    'Load 

                End If


            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ShowData(ByVal paramdrFoundDiferredList As DataRow, ByVal paramdtInstTransactdtl As DataTable)
        Dim drFoundTransactDtl As DataRow()
        Dim l_DeferredInstallmentAmt As Decimal
        Dim l_totalAmont As Decimal
        Dim l_RemainingAmont As Decimal
        Dim payeename As String
        Try
            If Not paramdrFoundDiferredList Is Nothing Then
                Dim strFilter As String = "guiTransactionRefID='" + g_strRefRequestID + "' AND intInstallmentID ='" + g_intInstallmentID.ToString() + "'"
                drFoundTransactDtl = paramdtInstTransactdtl.Select(strFilter)
                If drFoundTransactDtl.Length > 0 Then
                    l_DeferredInstallmentAmt = Math.Round(Convert.ToDecimal(paramdtInstTransactdtl.Compute("SUM(mnyPersonalPostTax) + SUM(mnyPersonalPreTax) + SUM(mnyYmcaPreTax)", strFilter)), 2)
                    l_totalAmont = Convert.ToDecimal(paramdrFoundDiferredList("mnyTotalTaxable")) + Convert.ToDecimal(paramdrFoundDiferredList("mnyTotalNonTaxable"))
                    l_RemainingAmont = l_totalAmont - l_DeferredInstallmentAmt
                End If

                txtRemainingMBWMoney.Text = Convert.ToString(l_RemainingAmont)
                txtDeferredInstallmentAmt.Text = Convert.ToString(l_DeferredInstallmentAmt)
                payeeName = paramdrFoundDiferredList("chvLastName").ToString().Trim()
                payeeName += IIf(paramdrFoundDiferredList("chvMiddleName").ToString() = "", String.Empty, " " + paramdrFoundDiferredList("chvMiddleName").ToString().Trim())
                payeeName += IIf(paramdrFoundDiferredList("chvFirstName").ToString() = "", String.Empty, " " + paramdrFoundDiferredList("chvFirstName").ToString().Trim())
                TextBoxPayee1.Text = payeename
                LabelTitle.Text = payeename + ", Fund No.#: " + g_intFundIdNo.ToString()
                TextboxPayee2.Text = Convert.ToString(paramdrFoundDiferredList("chvRolloverInstitutionName"))

                DispalyDataInControls()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function DispalyDataInControls()
        Dim l_TaxableAmout1 As Decimal
        Dim l_TaxableAmout2 As Decimal
        Dim l_TaxableAmout3 As Decimal
        Dim l_NonTaxableAmout1 As Decimal
        Dim l_NonTaxableAmout2 As Decimal
        Dim l_NonTaxableAmout3 As Decimal
        Dim l_NetAmout1 As Decimal
        Dim l_NetAmout2 As Decimal
        Dim l_NetAmout3 As Decimal
        Dim l_Tax As Decimal
        Dim l_Tax3 As Decimal

        Dim l_TaxRate As Decimal
        Dim l_WithHoldingDedction As Decimal

        Try
            l_TaxableAmout1 = Convert.ToDecimal(IIf(TextboxTaxablePayee1.Text.Trim = String.Empty, 0, TextboxTaxablePayee1.Text.Trim))
            l_TaxableAmout2 = Convert.ToDecimal(IIf(TextboxTaxablePayee2.Text.Trim = String.Empty, 0, TextboxTaxablePayee2.Text.Trim))
            l_NonTaxableAmout1 = Convert.ToDecimal(IIf(TextboxNonTaxablePayee1.Text.Trim = String.Empty, 0, TextboxNonTaxablePayee1.Text.Trim))
            l_NonTaxableAmout2 = Convert.ToDecimal(IIf(TextboxNonTaxablePayee2.Text.Trim = String.Empty, 0, TextboxNonTaxablePayee2.Text.Trim))
            'l_TaxRate = IIf(TextboxTaxRate.Text.Trim = String.Empty, 0, Convert.ToDecimal(TextboxTaxRate.Text.Trim))
            l_TaxRate = Me.TaxRate
            ViewState("PrevTaxRate") = Me.TaxRate

            l_Tax = Convert.ToDecimal(IIf(TextboxTax.Text.Trim = String.Empty, 0, TextboxTax.Text.Trim))
            ViewState("PrevTaxAmt") = l_Tax
            'l_WithHoldingDedction = IIf(TextboxDeductions.Text.Trim = String.Empty, 0, Convert.ToDecimal(TextboxDeductions.Text.Trim))

            If TextboxDeductions.Text.Trim = String.Empty Then
                l_WithHoldingDedction = 0
            Else
                l_WithHoldingDedction = Convert.ToDecimal(TextboxDeductions.Text.Trim)
            End If
            l_NetAmout1 = l_TaxableAmout1 + l_NonTaxableAmout1 - l_Tax
            l_NetAmout2 = l_TaxableAmout2 + l_NonTaxableAmout2

            l_TaxableAmout3 = l_TaxableAmout1 + l_TaxableAmout2
            l_NonTaxableAmout3 = l_NonTaxableAmout1 + l_NonTaxableAmout2
            l_Tax3 = l_Tax
            l_NetAmout3 = l_NetAmout1 + l_NetAmout2 - l_WithHoldingDedction

            TextboxTaxablePayee1.Text = Math.Round(l_TaxableAmout1, 2)
            TextboxNonTaxablePayee1.Text = Math.Round(l_NonTaxableAmout1, 2)
            'TextboxTax.Text = Math.Round(l_Tax, 2)
            TextboxNetPayee1.Text = Math.Round(l_NetAmout1, 2)

            TextboxTaxablePayee2.Text = Math.Round(l_TaxableAmout2, 2)
            TextboxNonTaxablePayee2.Text = Math.Round(l_NonTaxableAmout2, 2)
            TextboxNetPayee2.Text = Math.Round(l_NetAmout2, 2)

            TextboxNetFinal.Text = Math.Round(l_NetAmout3, 2)
            TextboxTaxableFinal.Text = Math.Round(l_TaxableAmout3, 2)
            TextboxNonTaxableFinal.Text = Math.Round(l_NonTaxableAmout3, 2)
            TextboxTaxFinal.Text = Math.Round(l_Tax3, 2)

            If l_NetAmout2 = 0 Then
                TextboxPayee2.ReadOnly = True
            End If
            If l_TaxableAmout1 = 0 Then
                TextboxTax.ReadOnly = True
                TextboxTaxRate.ReadOnly = True
            Else
                TextboxTax.ReadOnly = False
                TextboxTaxRate.ReadOnly = False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function LoadPayee1Details(ByVal paraDeferredListRow As DataRow, ByVal paradtInstTransactdtl As DataTable)

        Dim drFoundTransactDtl As DataRow()
        Dim strFilter As String
        Dim dtPayee1AcctDtl As DataTable
        Dim mnyTaxable As Decimal = 0
        Dim mnyNonTaxable As Decimal = 0
        ' Dim mnyTaxAmt As Decimal = 0
        Dim mnyTaxableTotal As Decimal = 0
        Dim mnyNonTaxableTotal As Decimal = 0
        Dim mnyTaxTotal As Decimal = 0
        Try
            If Not paradtInstTransactdtl Is Nothing AndAlso paradtInstTransactdtl.Rows.Count > 0 Then

                strFilter = "guiTransactionRefID='" + g_strRefRequestID + "' AND intInstallmentID ='" + g_intInstallmentID.ToString() + "' AND chvPayeeEntityTypeCode='PERSON'"
                drFoundTransactDtl = paradtInstTransactdtl.Select(strFilter)
                If drFoundTransactDtl.Length > 0 Then
                    dtPayee1AcctDtl = CreatePayeeAcctTable()
                    If paradtInstTransactdtl.Select(strFilter + " AND chrAcctType='MR'").Length > 0 Then
                        mnyTaxable = Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPreTax)+SUM(mnyYmcaPreTax)", strFilter + " AND chrAcctType='MR'"))
                        mnyTaxableTotal = mnyTaxable
                        mnyNonTaxable = Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPostTax)", strFilter + " AND chrAcctType='MR'"))
                        mnyNonTaxableTotal = mnyNonTaxable
                        'mnyTaxAmt = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyTaxDeduction)", strFilter + " AND chrAcctType='MR'")), 2)
                        'mnyTaxTotal = mnyTaxAmt
                        CreatePayeeAcctTableRow(dtPayee1AcctDtl, "MR", mnyTaxable, mnyNonTaxable)
                    End If
                    If paradtInstTransactdtl.Select(strFilter + " AND chrAcctType='MS'").Length > 0 Then
                        mnyTaxable = 0
                        mnyNonTaxable = 0
                        'mnyTaxAmt = 0
                        mnyTaxable = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPreTax)+SUM(mnyYmcaPreTax)", strFilter + " AND chrAcctType='MS'")), 2)
                        mnyTaxableTotal += mnyTaxable
                        mnyNonTaxable = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPostTax)", strFilter + " AND chrAcctType='MS'")), 2)
                        mnyNonTaxableTotal += mnyNonTaxable
                        'mnyTaxAmt = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyTaxDeduction)", strFilter + " AND chrAcctType='MS'")), 2)
                        'mnyTaxTotal += mnyTaxAmt
                        CreatePayeeAcctTableRow(dtPayee1AcctDtl, "MS", mnyTaxable, mnyNonTaxable)
                    End If
                    If paraDeferredListRow.Item("mnyTaxRatePct").GetType.ToString <> "System.DBNull" Then
                        TextboxTaxRate.Text = Convert.ToDecimal(paraDeferredListRow("mnyTaxRatePct"))
                        Me.TaxRate = Convert.ToDecimal(paraDeferredListRow("mnyTaxRatePct"))

                    End If
                    mnyTaxTotal = CalculateTaxAmount(Me.TaxRate)
                    DataGridPayee1.DataSource = dtPayee1AcctDtl
                    DataGridPayee1.DataBind()
                    TextboxTaxablePayee1.Text = Math.Round(mnyTaxableTotal, 2).ToString()
                    TextboxNonTaxablePayee1.Text = Math.Round(mnyNonTaxableTotal, 2).ToString()
                    TextboxTax.Text = mnyTaxTotal.ToString()
                    TextboxNetPayee1.Text = Math.Round((mnyTaxableTotal + mnyNonTaxableTotal) - mnyTaxTotal, 2).ToString()



                End If

            End If
        Catch ex As Exception
            Throw ex

        End Try
    End Function
    Private Function LoadPayee2Details(ByVal paraDeferredListRow As DataRow, ByVal paradtInstTransactdtl As DataTable)

        Dim drFoundTransactDtl As DataRow()
        Dim strFilter As String
        Dim dtPayee2AcctDtl As DataTable
        Dim mnyTaxable As Decimal = 0
        Dim mnyNonTaxable As Decimal = 0
        Dim mnyTaxAmt As Decimal = 0
        Dim mnyTaxableTotal As Decimal = 0
        Dim mnyNonTaxableTotal As Decimal = 0
        Dim mnyTaxTotal As Decimal = 0
        Try
            If Not paradtInstTransactdtl Is Nothing AndAlso paradtInstTransactdtl.Rows.Count > 0 Then

                strFilter = "guiTransactionRefID='" + g_strRefRequestID + "' AND intInstallmentID ='" + g_intInstallmentID.ToString() + "' AND chvPayeeEntityTypeCode='ROLINS'"
                drFoundTransactDtl = paradtInstTransactdtl.Select(strFilter)
                If drFoundTransactDtl.Length > 0 Then
                    dtPayee2AcctDtl = CreatePayeeAcctTable()
                    If paradtInstTransactdtl.Select(strFilter + " AND chrAcctType='MR'").Length > 0 Then
                        mnyTaxable = Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPreTax)+SUM(mnyYmcaPreTax)", strFilter + " AND chrAcctType='MR'"))
                        mnyTaxableTotal = mnyTaxable
                        mnyNonTaxable = Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPostTax)", strFilter + " AND chrAcctType='MR'"))
                        mnyNonTaxableTotal = mnyNonTaxable
                        mnyTaxAmt = Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyTaxDeduction)", strFilter + " AND chrAcctType='MR'"))
                        mnyTaxTotal = mnyTaxAmt
                        CreatePayeeAcctTableRow(dtPayee2AcctDtl, "MR", mnyTaxable, mnyNonTaxable)
                    End If
                    If paradtInstTransactdtl.Select(strFilter + " AND chrAcctType='MS'").Length > 0 Then
                        mnyTaxable = 0
                        mnyNonTaxable = 0
                        mnyTaxAmt = 0
                        mnyTaxable = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPreTax)+SUM(mnyYmcaPreTax)", strFilter + " AND chrAcctType='MS'")), 2)
                        mnyTaxableTotal += mnyTaxable
                        mnyNonTaxable = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyPersonalPostTax)", strFilter + " AND chrAcctType='MS'")), 2)
                        mnyNonTaxableTotal += mnyNonTaxable
                        mnyTaxAmt = Math.Round(Convert.ToDecimal(paradtInstTransactdtl.Compute("SUM(mnyTaxDeduction)", strFilter + " AND chrAcctType='MS'")), 2)
                        mnyTaxTotal += mnyTaxAmt
                        CreatePayeeAcctTableRow(dtPayee2AcctDtl, "MS", mnyTaxable, mnyNonTaxable)
                    End If

                    DatagridPayee2.DataSource = dtPayee2AcctDtl
                    DatagridPayee2.DataBind()
                    TextboxTaxablePayee2.Text = Math.Round(mnyTaxableTotal, 2).ToString()
                    TextboxNonTaxablePayee2.Text = Math.Round(mnyNonTaxableTotal, 2).ToString()
                    TextboxNetPayee2.Text = Math.Round(mnyTaxableTotal + mnyNonTaxableTotal, 2).ToString()


                End If

            End If
        Catch ex As Exception
            Throw ex

        End Try
    End Function
    Private Function CreatePayeeAcctTable() As DataTable
        Dim dtPayee As DataTable
        Try
            dtPayee = New DataTable
            dtPayee.Columns.Add(New DataColumn("chrAccttype", GetType(System.String)))
            dtPayee.Columns.Add(New DataColumn("mnyTaxable", GetType(System.Decimal)))
            dtPayee.Columns.Add(New DataColumn("mnyNonTaxable", GetType(System.Decimal)))
            Return dtPayee

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function CreatePayeeAcctTableRow(ByRef paraDtPayeeAcct As DataTable, ByVal paraAcctType As String, ByVal paramnyTaxable As Decimal, ByVal paramnyNonTaxable As Decimal)
        Dim drPayee As DataRow
        Try
            If Not paraDtPayeeAcct Is Nothing Then
                drPayee = paraDtPayeeAcct.NewRow
                drPayee("chrAccttype") = paraAcctType
                drPayee("mnyTaxable") = Math.Round(paramnyTaxable, 2)
                drPayee("mnyNonTaxable") = Math.Round(paramnyNonTaxable, 2)
                paraDtPayeeAcct.Rows.Add(drPayee)
            End If
        Catch ex As Exception
            Throw

        End Try
    End Function
#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Not Request.QueryString Is Nothing Then
                If Not Request.QueryString("RefID") Is Nothing Then
                    g_strRefRequestID = Request.QueryString("RefID")
                End If
                If Not Request.QueryString("InstID") Is Nothing AndAlso Request.QueryString("InstID") <> String.Empty Then
                    g_intInstallmentID = Convert.ToInt64(Request.QueryString("InstID"))
                End If
            End If
            If Not Request.QueryString("FundIdNo") Is Nothing AndAlso Request.QueryString("FundIdNo").ToString() <> String.Empty Then
                g_intFundIdNo = Convert.ToInt64(Request.QueryString("FundIdNo"))
            End If

            If Not SessionDeferred_PaymentList Is Nothing Then
                g_dsDeferredInstList = CType(SessionDeferred_PaymentList, DataSet)
            End If

            'txtRemainingMBWMoney.Text = 

            If Not IsPostBack Then
                Me.TextboxTaxRate.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.TextboxTaxRate.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                Me.TextboxTax.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.TextboxTax.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

                LoadDeductions()
                LoadInstallmentData()
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlDecode(ex.Message), False)

        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Response.Write("<script>self.close(); </script>")

    End Sub

    'Private Sub CheckBoxDeduction_Checked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxDeduction.CheckedChanged
    Protected Sub CheckBoxDeduction_Checked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim l_CheckBox As CheckBox
        Dim dgItem As DataGridItem
        Dim l_AppliedDeduction As Decimal = 0
        Dim l_netpayye1 As Decimal = 0
        Dim l_netpayye2 As Decimal = 0
        Dim l_AddDeduction As Decimal = 0
        Try
            Dim l_TotalWithholding As Decimal = 0

            l_netpayye1 = Convert.ToDecimal(IIf(TextboxNetPayee1.Text.Trim = String.Empty, 0, TextboxNetPayee1.Text))
            l_netpayye2 = Convert.ToDecimal(IIf(TextboxNetPayee2.Text.Trim = String.Empty, 0, TextboxNetPayee2.Text.Trim))
            l_TotalWithholding = Convert.ToDecimal(IIf(TextboxDeductions.Text = String.Empty, 0, TextboxDeductions.Text))

            l_CheckBox = CType(sender, CheckBox)
            dgItem = CType(l_CheckBox.NamingContainer, DataGridItem)
            If Not l_CheckBox Is Nothing AndAlso Not dgItem Is Nothing Then
                l_AddDeduction = Convert.ToDecimal(dgItem.Cells(3).Text.Trim)
                If l_CheckBox.Checked Then
                    If l_TotalWithholding + l_AddDeduction > l_netpayye1 Then
                        If Not TextboxPayee2.ReadOnly Then
                            If l_TotalWithholding + l_AddDeduction > l_netpayye2 Then
                                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Withholding Deduction Amount.", MessageBoxButtons.Stop, False)
                                l_CheckBox.Checked = False
                                Exit Sub
                            ElseIf l_TotalWithholding + l_AddDeduction = l_netpayye2 Then
                                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
                                l_CheckBox.Checked = False
                                Exit Sub
                            End If
                        Else
                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Withholding Deduction Amount.", MessageBoxButtons.Stop, False)
                            l_CheckBox.Checked = False
                            Exit Sub
                        End If
                    ElseIf l_TotalWithholding + l_AddDeduction = l_netpayye1 Then
                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
                        l_CheckBox.Checked = False
                        Exit Sub
                    End If
                    l_TotalWithholding = l_TotalWithholding + l_AddDeduction
                Else
                    l_TotalWithholding = l_TotalWithholding - l_AddDeduction
                End If
            End If

            'For Each itm As DataGridItem In DatagridDeductions.Items
            '    Dim l_blnChecked As Boolean
            '    l_CheckBox = DirectCast(itm.FindControl("CheckBoxDeduction"), CheckBox)
            '    If (Not l_CheckBox Is Nothing) AndAlso (l_CheckBox.Checked) Then
            '        l_TotalWithholding += Convert.ToDecimal(itm.Cells(3).Text.Trim)
            '        If l_TotalWithholding > l_netpayye1 Then
            '            If Not TextboxPayee2.ReadOnly Then
            '                If l_TotalWithholding > l_netpayye2 Then
            '                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Withholding Deduction Amount.", MessageBoxButtons.Stop, False)
            '                    l_CheckBox.Checked = False
            '                    Exit Sub
            '                ElseIf l_TotalWithholding = l_netpayye2 Then
            '                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
            '                    l_CheckBox.Checked = False
            '                    Exit Sub
            '                End If
            '            Else
            '                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Withholding Deduction Amount.", MessageBoxButtons.Stop, False)
            '                l_CheckBox.Checked = False
            '                Exit Sub
            '            End If
            '        ElseIf l_TotalWithholding = l_netpayye1 Then
            '            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
            '            l_CheckBox.Checked = False
            '            Exit Sub
            '            'payee1 after deduction > 0.01
            '        End If
            '    End If
            'Next




                TextboxDeductions.Text = String.Format("{0:0.00}", l_TotalWithholding.ToString())
                'TextboxNetFinal.Text = Convert.ToString(Math.Round(l_netpayye1 + l_netpayye2 - Convert.ToDecimal(l_TotalWithholding), 2))
                DispalyDataInControls()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_BooleanFlag As Boolean = False
        Dim dtDeferredPaymentList As DataTable
        Dim dtInstTransactdtl As DataTable
        Dim drFoundDiferredList As DataRow()
        Dim l_CheckBox As CheckBox
        Dim l_dtWitholdingDeduction As DataTable
        Dim l_taxRate As Decimal = 0
        Dim l_minTaxable As Decimal = 0
        Dim l_payeeTaxableAmt As Decimal = 0
        Dim l_Tax As Decimal = 0

        Try

            If SessionDeferred_PaymentList Is Nothing Then Exit Sub
            If Not TextboxPayee2.ReadOnly Then
                If TextboxPayee2.Text.Trim.Length <= 0 Then
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please enter Rollover Institution Name.", MessageBoxButtons.Stop, False)
                    Exit Sub
                End If
            End If

            If Not SessionDeferred_PaymentList Is Nothing Then
                g_dsDeferredInstList = CType(SessionDeferred_PaymentList, DataSet)

                If g_dsDeferredInstList.Tables.Count = 2 Then
                    dtDeferredPaymentList = g_dsDeferredInstList.Tables("dtDefPaymentList")
                    dtInstTransactdtl = g_dsDeferredInstList.Tables("dtInstallmentTransactdetail")
                    drFoundDiferredList = dtDeferredPaymentList.Select("guiRefRequestID='" + g_strRefRequestID + "' AND intInstallmentID ='" + g_intInstallmentID.ToString() + "'")
                    If drFoundDiferredList.Length > 0 Then

                        '******** validation    *********************
                        '*** check for rollover institution name
                        If Not drFoundDiferredList(0)("chvRolloverInstitutionName").GetType.ToString().ToUpper = "SYSTEM.DBNULL" Then
                            If Convert.ToString(drFoundDiferredList(0)("chvRolloverInstitutionName")).Trim.Length > 0 Then
                                If TextboxPayee2.Text.Trim().Length <= 0 Then
                                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please enter Rollover Institution Name.", MessageBoxButtons.Stop, False)
                                    Exit Sub
                                End If
                            End If
                        End If
                        '***  end
                        'validation for Tax rate and tax amount it cannot be empty
                        If TextboxTaxRate.Text = String.Empty Then
                            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Rate entered. Tax Rate can not be empty.", MessageBoxButtons.Stop, False)
                            Exit Sub
                        Else
                            l_taxRate = Convert.ToDecimal(TextboxTaxRate.Text)
                        End If

                        If TextboxTax.Text = String.Empty Then
                            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax amount entered. Tax amont can not be empty.", MessageBoxButtons.Stop, False)
                            Exit Sub
                        Else
                            l_Tax = Convert.ToDecimal(TextboxTax.Text)
                        End If
                        'validation for Tax rate ,it should be >20 and <100
                        If l_taxRate >= 100 Or l_taxRate < 20 Then
                            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Rate entered. Tax Rate should between 20% to 100%.", MessageBoxButtons.Stop, False)
                            Exit Sub

                        End If

                        If TextboxTaxablePayee1.Text <> String.Empty Then
                            l_payeeTaxableAmt = Convert.ToDecimal(Me.TextboxTaxablePayee1.Text)
                            ' l_minTaxable = Math.Round(l_payeeTaxableAmt * 0.2, 2)
                            'l_minTaxable = Math.Round(CalculateTaxAmount(20), 2)
                            l_minTaxable = CalculateTaxAmount(20)
                        End If

                        'validation for tax amount
                        If l_payeeTaxableAmt > 0 AndAlso (l_Tax < l_minTaxable Or l_Tax >= l_payeeTaxableAmt) Then
                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Amount entered.", MessageBoxButtons.Stop, False)
                            Exit Sub
                        End If
                        If TextboxNetFinal.Text <> String.Empty Then
                            If Convert.ToDecimal(TextboxNetFinal.Text) < 0.01 Then
                                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
                                Exit Sub
                            End If
                        End If

                        '************* validation *****************
                        'update tax rate and Rollover name into main list table
                        drFoundDiferredList(0)("mnyTaxRatePct") = l_taxRate
                        If TextboxPayee2.Text.Trim() <> String.Empty Then
                            drFoundDiferredList(0)("chvRolloverInstitutionName") = TextboxPayee2.Text.Trim()
                        End If
                        dtDeferredPaymentList.AcceptChanges()
                        l_BooleanFlag = True
                    Else
                        l_BooleanFlag = False
                    End If

                    If l_BooleanFlag = True AndAlso Not Session("dtWithHoldingDeduction") Is Nothing Then
                        l_dtWitholdingDeduction = CType(Session("dtWithHoldingDeduction"), DataTable)

                        Dim l_drFoundWithHoding As DataRow() = l_dtWitholdingDeduction.Select("guiRefRequestID='" + g_strRefRequestID + "' AND intInstallmentID=" + g_intInstallmentID.ToString())
                        For Each dr As DataRow In l_drFoundWithHoding
                            l_dtWitholdingDeduction.Rows.Remove(dr)
                        Next
                        l_dtWitholdingDeduction.AcceptChanges()
                        l_drFoundWithHoding = Nothing

                        For Each itm As DataGridItem In DatagridDeductions.Items
                            Dim l_blnChecked As Boolean
                            l_CheckBox = DirectCast(itm.FindControl("CheckBoxDeduction"), CheckBox)
                            If (Not l_CheckBox Is Nothing) AndAlso (l_CheckBox.Checked) Then
                                Dim l_withhodingDatatRow As DataRow = l_dtWitholdingDeduction.NewRow()
                                l_withhodingDatatRow("guiRefRequestID") = g_strRefRequestID
                                l_withhodingDatatRow("intInstallmentID") = g_intInstallmentID
                                l_withhodingDatatRow("chvWithholdingTypeCode") = itm.Cells(1).Text.Trim
                                l_withhodingDatatRow("mnyAmount") = Convert.ToDecimal(itm.Cells(3).Text.Trim)
                                l_dtWitholdingDeduction.Rows.Add(l_withhodingDatatRow)
                                l_withhodingDatatRow = Nothing
                            End If
                        Next
                    End If
                End If
            End If

            If l_BooleanFlag = True Then
                Session("DP_PopUpClose") = True
                Response.Write("<script> window.opener.document.forms(0).submit(); self.close(); </script>")
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub TextboxTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxTaxRate.TextChanged
        Dim l_TaxableAmount1 As Decimal
        Dim l_TaxRate As Decimal
        Dim l_FinalNetAmount As Decimal = 0
        Dim l_TaxAmount As Decimal = 0
        Dim l_TotalDeduction As Decimal = 0
        Try
            If TextboxTaxRate.Text = String.Empty Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Rate entered. Tax Rate can not be empty.", MessageBoxButtons.Stop, False)
                Exit Sub
            Else
                l_TaxRate = Convert.ToDecimal(TextboxTaxRate.Text.Trim)
            End If

            If TextboxTaxablePayee1.Text = String.Empty Then
                'Me.TextboxTaxablePayee1.Text = 0
                'Exit Sub
            Else
                l_TaxableAmount1 = Convert.ToDecimal(Me.TextboxTaxablePayee1.Text)
            End If


            If l_TaxRate >= 100 Or l_TaxRate < 20 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Rate entered. Tax Rate should between 20% to 100%.", MessageBoxButtons.Stop, False)
                If Not ViewState("PrevTaxRate") Is Nothing Then
                    TextboxTaxRate.Text = Convert.ToDecimal(ViewState("PrevTaxRate"))
                End If
                Exit Sub
            End If
            'l_TaxAmount = Math.Round(CalculateTaxAmount(l_TaxRate), 2)
            l_TaxAmount = CalculateTaxAmount(l_TaxRate)
            l_TotalDeduction = Convert.ToDecimal(IIf(TextboxDeductions.Text = String.Empty, 0, TextboxDeductions.Text))
            If TextboxNetFinal.Text <> String.Empty Then
                l_FinalNetAmount = Convert.ToDecimal(TextboxNetFinal.Text)

                'l_TotalDeduction = Convert.ToDecimal(IIf(TextboxDeductions.Text = String.Empty, 0, TextboxDeductions.Text))
                If l_TaxableAmount1 - l_TotalDeduction - l_TaxAmount < 0.01 Then
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
                    If Not ViewState("PrevTaxRate") Is Nothing Then
                        TextboxTaxRate.Text = Convert.ToDecimal(ViewState("PrevTaxRate"))
                    End If
                    Exit Sub
                End If
            End If


            TextboxTax.Text = l_TaxAmount
            Me.TaxRate = l_TaxRate

            DispalyDataInControls()
            'TextboxNetPayee1.Text = Math.Round(Me.TaxableAmtPayee1 - Me.TaxRate, 2)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub TextboxTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxTax.TextChanged
        Dim l_Tax As Decimal
        Dim l_TaxRate As Decimal
        Dim l_TaxableAmount1 As Decimal
        Dim l_previousTaxAmt As Decimal = 0
        Dim l_previousTaxRate As Decimal = 0
        Dim l_minTaxable As Decimal = 0
        Dim l_FinalNetAmount As Decimal = 0
        Dim l_TotalDeduction As Decimal = 0

        Try
            If TextboxTax.Text = String.Empty Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Amount entered. Tax Amount can not be empty.", MessageBoxButtons.Stop, False)
                Exit Sub
            Else
                l_Tax = Convert.ToDecimal(Me.TextboxTax.Text)
            End If

            If TextboxTaxablePayee1.Text = String.Empty Then
                'Me.TextboxTaxablePayee1.Text = 0
                'Exit Sub
            Else
                l_TaxableAmount1 = Convert.ToDecimal(Me.TextboxTaxablePayee1.Text)
                'l_minTaxable = Math.Round(CalculateTaxAmount(20), 2)
                l_minTaxable = CalculateTaxAmount(20)
            End If

            If l_TaxableAmount1 > 0 AndAlso l_Tax < l_minTaxable Or l_Tax >= l_TaxableAmount1 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Amount entered.", MessageBoxButtons.Stop, False)
                If Not ViewState("PrevTaxAmt") Is Nothing Then
                    TextboxTax.Text = Convert.ToDecimal(ViewState("PrevTaxAmt"))
                End If
                Exit Sub
            Else

            End If
            'If TextboxTaxRate.Text = String.Empty Then
            '    'Me.TextboxTaxRate.Text = 0
            '    'Me.TaxRate = 0
            'Else
            '    l_previousTaxRate = Convert.ToDecimal(TextboxTaxRate.Text.Trim)
            'End If

            'l_previousTaxAmt = Math.Round(l_TaxableAmount1 * l_previousTaxRate / 100, 2)
            If TextboxNetFinal.Text <> String.Empty Then
                l_FinalNetAmount = Convert.ToDecimal(TextboxNetFinal.Text)
                l_TotalDeduction = Convert.ToDecimal(IIf(TextboxDeductions.Text = String.Empty, 0, TextboxDeductions.Text))
                If l_TaxableAmount1 - l_TotalDeduction - l_Tax < 0.01 Then
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Net amount can not be less than $0.01.", MessageBoxButtons.Stop, False)
                    If Not ViewState("PrevTaxAmt") Is Nothing Then
                        TextboxTax.Text = Convert.ToDecimal(ViewState("PrevTaxAmt"))
                    End If
                    Exit Sub
                End If
            End If

            If l_TaxableAmount1 <> 0 Then
                l_TaxRate = l_Tax * 100 / l_TaxableAmount1
                Me.TaxRate = l_TaxRate
                'TextboxTaxRate.Text = Math.Round(l_TaxRate, 2)
                TextboxTaxRate.Text = l_TaxRate
            End If

            DispalyDataInControls()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DatagridDeductions_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridDeductions.ItemCreated
        Dim elemType As ListItemType = e.Item.ItemType
        Dim chk As CheckBox

        If (elemType = ListItemType.Item) Or (elemType = ListItemType.AlternatingItem) Then
            chk = CType(e.Item.FindControl("CheckBoxDeduction"), CheckBox)
            AddHandler chk.CheckedChanged, AddressOf CheckBoxDeduction_Checked
        End If

    End Sub
    Private Function CalculateTaxAmount(ByVal paraTaxRate As Decimal) As Decimal
        Dim l_TotalTaxAmount As Decimal = 0
        Dim l_TaxAmount As Decimal = 0
        Dim l_personalTaxDeduction As Decimal = 0
        Dim l_ymcaTaxDeduction As Decimal = 0
        Dim l_dsDiferredPayment As DataSet
        Dim l_dtTransactionDetails As DataTable
        Dim drFoundTransactDtl As DataRow()
        Dim l_TaxOnInterest As Decimal = 0
        Dim l_TaxOnPrincipal As Decimal = 0
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function
            l_dsDiferredPayment = CType(SessionDeferred_PaymentList, DataSet)
            l_dtTransactionDetails = l_dsDiferredPayment.Tables("dtInstallmentTransactdetail")
            If Not l_dtTransactionDetails Is Nothing AndAlso l_dtTransactionDetails.Rows.Count > 0 Then
                Dim strFilter As String = "guiTransactionRefID='" + g_strRefRequestID + "' AND intInstallmentID ='" + g_intInstallmentID.ToString() + "' AND chvPayeeEntityTypeCode ='PERSON'"
                drFoundTransactDtl = l_dtTransactionDetails.Select(strFilter)
                If drFoundTransactDtl.Length > 0 Then
                    For Each drTransaction As DataRow In drFoundTransactDtl
                        l_personalTaxDeduction = 0
                        l_ymcaTaxDeduction = 0
                        If drTransaction("chrTransactType").ToString.Trim.ToUpper = "MWPR" Then
                            l_personalTaxDeduction = Convert.ToDecimal(drTransaction.Item("mnyPersonalPreTax")) * paraTaxRate / 100
                            l_ymcaTaxDeduction = Convert.ToDecimal(drTransaction.Item("mnyYmcaPretax")) * paraTaxRate / 100
                            l_TaxOnPrincipal += l_personalTaxDeduction + l_ymcaTaxDeduction
                        End If
                        If drTransaction("chrTransactType").ToString.Trim.ToUpper = "MWIN" Then
                            l_personalTaxDeduction = Convert.ToDecimal(drTransaction.Item("mnyPersonalPreTax")) * paraTaxRate / 100
                            l_ymcaTaxDeduction = Convert.ToDecimal(drTransaction.Item("mnyYmcaPretax")) * paraTaxRate / 100
                            l_TaxOnInterest += l_personalTaxDeduction + l_ymcaTaxDeduction
                        End If
                        'l_personalTaxDeduction = Convert.ToDecimal(drTransaction.Item("mnyPersonalPreTax")) * paraTaxRate / 100
                        'l_ymcaTaxDeduction = Convert.ToDecimal(drTransaction.Item("mnyYmcaPretax")) * paraTaxRate / 100
                        'l_TotalTaxAmount += l_personalTaxDeduction + l_ymcaTaxDeduction

                    Next
                    l_TotalTaxAmount = Math.Round(l_TaxOnPrincipal, 2) + Math.Round(l_TaxOnInterest, 2)
                End If
            End If
            Return l_TotalTaxAmount
        Catch ex As Exception
            Throw ex

        End Try
    End Function
End Class
