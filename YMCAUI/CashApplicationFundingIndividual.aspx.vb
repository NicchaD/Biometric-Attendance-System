
'****************************************************************************************************************************
' Project Name		:	YMCA-YRS
' FileName			:	CashApplicationFundingIndividual.aspx
' Author Name		:	Shashank Patel
' Employee ID		:	55381
' Email			    :	shashank.patel@3i-infotech.com
' Creation Date	    :	08/23/2013 
' Program Specification Name	: YRS-SRS- Receipts-Cash Application-Person.doc	
' Unit Test Plan Name			:	
' Description					: BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
'****************************************************************************************************************************
' Change History 
'****************************************************************************************************************************
'Date			Modified by		Description	
'****************************************************************************************************************************
'2014.02.07		Shashank		YRS 5.0-842 (Add validation for partially funding for loan transactions)
'2014.05.08     Shashank        BT-2531 -Funding transactions for a single participant screen is hung for thousand transaction(s) in transmittal
'2014.05.15     Shashank        BT-2531 -Funding transactions for a single participant screen is hung for thousand transaction(s) in transmittal
'2015.09.16     Manthan Rajguru YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'2015.12.14     Bala            YRS-AT-2642: YRS enh: Remove ssno from sending loan defalulted email.
'2016.04.12     Anudeep A       YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'2016.07.27     Pramod P. P.    YRS-AT-3058 - YRS bug: timing issue Loan Utility - defaulting a Loan and then transmittal w/payment, Auto closed as successfully paid off? (trackIt 26945) 
'******************************************************************************************************************************

Imports YMCARET.YmcaBusinessObject
Public Class CashApplicationFundingIndividual
	Inherits System.Web.UI.Page

	Dim strFormName As String = "CashApplicationFundingIndividual.aspx"
#Region "Properties"



	Public Property SelectedYMCAID() As String
		Get
			Return Convert.ToString(ViewState("YMCAGuiID"))
		End Get
		Set(ByVal value As String)
			ViewState("YMCAGuiID") = value
		End Set
	End Property
	Public Property SelectedGuiFundEventID() As String
		Get
			Return Convert.ToString(ViewState("GuiFundEventID"))
		End Get
		Set(ByVal value As String)
			ViewState("GuiFundEventID") = value
		End Set
	End Property
	Public Property SelectedGuiTransmittalID() As String
		Get
			Return Convert.ToString(ViewState("GuiTransmittalID"))
		End Get
		Set(ByVal value As String)
			ViewState("GuiTransmittalID") = value
		End Set
	End Property

    'SP 2014.05.08 BT-2531 -Change viewstate to session
	Public Property Transaction() As DataTable
		Get
            Return DirectCast(Session("ViewTransaction"), DataTable)
		End Get
		Set(ByVal value As DataTable)
            Session("ViewTransaction") = value
		End Set
	End Property
	Public Property AmountPaid() As Decimal
		Get
			Return Convert.ToDecimal(ViewState("AmountPaid"))
		End Get
		Set(ByVal value As Decimal)
			ViewState("AmountPaid") = value
		End Set
	End Property
	Public Property AmountDue() As Decimal
		Get
			Return Convert.ToDecimal(ViewState("AmountDue"))
		End Get
		Set(ByVal value As Decimal)
			ViewState("AmountDue") = value
		End Set
	End Property
	Public Property AmountPaidNotUse() As Decimal
		Get
			Return Convert.ToDecimal(ViewState("AmountPaidNotUse"))
		End Get
		Set(ByVal value As Decimal)
			ViewState("AmountPaidNotUse") = value
		End Set
	End Property
	Public Property FundIdNo As String
		Get
			Return Convert.ToString(ViewState("FundIdNo"))
		End Get
		Set(ByVal value As String)
			ViewState("FundIdNo") = value
		End Set
    End Property
    'SP 2014.05.12 -BT-2531 -Change viewstate to session
	Public Property CurrentTransactions As DataTable
		Get
            Return DirectCast(Session("Transactions"), DataTable)
		End Get
		Set(ByVal value As DataTable)
            Session("Transactions") = value
		End Set
    End Property
    'SP 2014.05.12 -BT-2531--BT-2531 -Change viewstate to session
	Public Property CurrentReceipt As DataTable
		Get
			Return DirectCast(ViewState("Receipts"), DataTable)
		End Get
		Set(ByVal value As DataTable)
			ViewState("Receipts") = value
		End Set
	End Property
	Public Property OriginalCredits() As Decimal
		Get
			Return Convert.ToDecimal(ViewState("OriginalCredits"))
		End Get
		Set(ByVal value As Decimal)
			ViewState("OriginalCredits") = value
		End Set
	End Property

	Public Property SortExpressionYMCA As String
		Get
			Return Convert.ToString(ViewState("SortExpYMCAGrid"))
		End Get
		Set(ByVal value As String)
			ViewState("SortExpYMCAGrid") = value
		End Set
	End Property
	Public Property YMCAList As DataSet
		Get
			Return DirectCast(ViewState("YMCAList"), DataSet)
		End Get
		Set(ByVal value As DataSet)
			ViewState("YMCAList") = value
		End Set
	End Property

	Public Property SelectedReceiptIndex() As Int32
		Get
			Return IIf(ViewState("ReceiptIndex") Is Nothing, -1, Convert.ToInt32(ViewState("ReceiptIndex")))
		End Get
		Set(ByVal value As Int32)
			ViewState("ReceiptIndex") = value
		End Set
	End Property
	Public Property SortExpressionPerson As String
		Get
			Return Convert.ToString(ViewState("SortExpPerson"))
		End Get
		Set(ByVal value As String)
			ViewState("SortExpPerson") = value
		End Set
	End Property
	Public Property SortExpressionTransmittal As String
		Get
			Return Convert.ToString(ViewState("SortExpTransmittal"))
		End Get
		Set(ByVal value As String)
			ViewState("SortExpTransmittal") = value
		End Set
	End Property
	Public Property PersonSearchList As DataSet
		Get
			Return DirectCast(ViewState("PersonSearchList"), DataSet)
		End Get
		Set(ByVal value As DataSet)
			ViewState("PersonSearchList") = value
		End Set
	End Property
	Public Property TransmittalSearchList As DataSet
		Get
			Return DirectCast(ViewState("TransmittalSearchList"), DataSet)
		End Get
		Set(ByVal value As DataSet)
			ViewState("TransmittalSearchList") = value
		End Set
	End Property
#End Region
    'Private totAmount As Decimal = 0 'SP 2014.05.12 -BT-2531

#Region "Page Events"


	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		DirectCast(Master.FindControl("Validationsummary1"), ValidationSummary).Visible = False
		If Not (Page.IsPostBack) Then

			RangeValidatorUCDate.MaximumValue = Date.Now.ToShortDateString()
			lnkSearchYMCA.Visible = False
			lblSearchYMCA.Visible = True
			lblSearchPerson.Visible = False
			lblSelectTransaction.Visible = False
			tdSearchYMCA.Style.Add("background-color", "#93BEEE")
            tdSearchYMCA.Style.Add("color", "#000000")
            Transaction = Nothing 'SP 2014.05.08 BT-2531  
            CurrentTransactions = Nothing 'SP 2014.05.08 BT-2531
		End If

	End Sub

#End Region

#Region "Methods"

	Private Sub BindYMCAGrid()
		Dim strYmcaNo As String
		Dim dv As DataView
		Dim dsYmcas As DataSet
		Try

			strYmcaNo = Me.TextBoxYmcaNo.Text.Trim
			If Not strYmcaNo = "" Then
				strYmcaNo = strYmcaNo.Trim().PadLeft(6, "0")
				Me.TextBoxYmcaNo.Text = strYmcaNo.Trim
			End If
			If (HelperFunctions.isNonEmpty(YMCAList)) Then
				dsYmcas = YMCAList.Copy()
			Else
				dsYmcas = CashApplicationBOClass.LookUpYmca(strYmcaNo, TextBoxYmcaName.Text.Trim(), TextBoxCity.Text.Trim, TextBoxState.Text.ToUpper.Trim)
				YMCAList = dsYmcas.Copy()
			End If
			gvYmca.SelectedIndex = -1
			If Not dsYmcas Is Nothing Then
				If Not dsYmcas.Tables("Ymcas") Is Nothing Then
					If dsYmcas.Tables("Ymcas").Rows.Count > 0 Then

						dv = dsYmcas.Tables("Ymcas").DefaultView

						If Not String.IsNullOrEmpty(SortExpressionYMCA) Then
							dv.Sort() = SortExpressionYMCA
						End If

						Me.gvYmca.DataSource = dv
						Me.gvYmca.DataBind()
						Me.gvYmca.Visible = True
						Me.LabelRecordNotFound.Visible = False
						Me.spnSelectYMCA.Visible = True
					Else
						Me.gvYmca.Visible = False
						Me.LabelRecordNotFound.Visible = True
						Me.spnSelectYMCA.Visible = False
					End If
				End If
			End If

		Catch ex As Exception
			HelperFunctions.LogException("btnYmcaFind_Click-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try
	End Sub


	'This method is used to sort the YMCA grid
	Public Sub SortYMCAgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)

		Dim SortExpression As String
		Try
			SortExpression = e.SortExpression
			If Not SortExpressionYMCA Is Nothing Then
				If SortExpressionYMCA.Trim.EndsWith(" DESC") Then
					SortExpressionYMCA = SortExpression + " ASC"
				Else
					SortExpressionYMCA = SortExpression + " DESC"
				End If
			Else
				SortExpressionYMCA = SortExpression + " DESC"
			End If
		Catch
			Throw
		End Try
	End Sub
	'This method is used to sort the person grid
	Public Sub SortPersongrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)

		Dim SortExpression As String
		Try
			SortExpression = e.SortExpression
			If Not SortExpressionPerson Is Nothing Then
				If SortExpressionPerson.Trim.EndsWith(" DESC") Then
					SortExpressionPerson = SortExpression + " ASC"
				Else
					SortExpressionPerson = SortExpression + " DESC"
				End If
			Else
				SortExpressionPerson = SortExpression + " DESC"
			End If
		Catch
			Throw
		End Try
	End Sub
	'This method is used to sort the person grid
	Public Sub SortTransmittalgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)

		Dim SortExpression As String
		Try
			SortExpression = e.SortExpression
			If Not SortExpressionTransmittal Is Nothing Then
				If SortExpressionTransmittal.Trim.EndsWith(" DESC") Then
					SortExpressionTransmittal = SortExpression + " ASC"
				Else
					SortExpressionTransmittal = SortExpression + " DESC"
				End If
			Else
				SortExpressionTransmittal = SortExpression + " DESC"
			End If
		Catch
			Throw
		End Try
	End Sub
	Public Sub ResetImages()
		Try
			imgbtnAmountPaidNotUse.ImageUrl = "images/Cash-UnApplied.jpg"
			imgbtnCreditAvailable.ImageUrl = "images/Cash-UnApplied.jpg"
			imgBtnReceipts.ImageUrl = "images/Cash-UnApplied.jpg"
		Catch ex As Exception
			HelperFunctions.LogException("ResetImages-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try
	End Sub

	'This method search the person in selected YCMA 
	Public Sub SearchParticipantbyYmcaId(ByVal guiYmcaId As String, ByVal strSSNo As String, ByVal strFundNo As String, ByVal strFirstName As String, ByVal strLastName As String)

		Dim dsPersonList As DataSet
		Dim dv As DataView
		Try
			gvPerson.SelectedIndex = -1
			If (HelperFunctions.isNonEmpty(PersonSearchList)) Then
				dsPersonList = PersonSearchList.Copy()
			Else
				dsPersonList = CashApplicationBOClass.GetParticipantsByYmcaID(guiYmcaId, strSSNo, strFundNo, strFirstName, strLastName)
				gvTransmittal.DataSource = Nothing 'SP 2014.02.07 -Clear grid
				gvTransmittal.DataBind()
				PersonSearchList = dsPersonList.Copy()
			End If

			If (HelperFunctions.isNonEmpty(dsPersonList)) Then
				dv = dsPersonList.Tables(0).DefaultView

				If Not String.IsNullOrEmpty(SortExpressionPerson) Then
					dv.Sort() = SortExpressionPerson
				End If

				gvPerson.DataSource = dv
				gvPerson.DataBind()
			Else
				gvPerson.DataSource = Nothing
				gvPerson.DataBind()
			End If
			If HelperFunctions.isNonEmpty(dsPersonList) Then
				spnSelectPerson.Visible = True
			Else
				spnSelectPerson.Visible = False
			End If

		Catch ex As Exception
			HelperFunctions.LogException("SearchParticipantbyYmcaId-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try
	End Sub
	'This method search all the un-funded transmittal by guifundeventid
	Private Sub PopulateTransmittalByFundEventId(ByVal guiYmcaID As String, ByVal guiFundEventID As String)
		Dim dsTransmittal As DataSet
		Dim dv As DataView
		Try
			gvTransmittal.SelectedIndex = -1
			If Not String.IsNullOrEmpty(guiYmcaID) AndAlso Not String.IsNullOrEmpty(guiFundEventID) Then
				If (HelperFunctions.isNonEmpty(TransmittalSearchList)) Then
					dsTransmittal = TransmittalSearchList.Copy()
				Else
					dsTransmittal = CashApplicationBOClass.GetTransmittalsByFundID(guiYmcaID, guiFundEventID)
					TransmittalSearchList = dsTransmittal.Copy()
				End If
				If (HelperFunctions.isNonEmpty(dsTransmittal)) Then
					dv = dsTransmittal.Tables(0).DefaultView

					If Not String.IsNullOrEmpty(SortExpressionTransmittal) Then
						dv.Sort() = SortExpressionTransmittal
					End If
					gvTransmittal.DataSource = dv
					gvTransmittal.DataBind()
					spnSelectTransmittal.Visible = True
				Else
					gvTransmittal.DataSource = Nothing
					gvTransmittal.DataBind()
					spnSelectTransmittal.Visible = False
				End If
				ViewState("Transmittal") = dsTransmittal
			End If

		Catch ex As Exception
			HelperFunctions.LogException("PopulateTransmittalByFundEventId-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try
	End Sub

	'This method search all the transaction(s) for selected person in a selected transmittal 
	Private Sub PouplateTransactionsByTransmittalId(ByVal strGuiYmcaID As String, ByVal strGuiFundEventID As String, ByVal strGuiTransmittalID As String)
		Dim dsTransaction As DataSet
		Dim dtTransactions As DataTable
		Dim dsYmcaReceipts As DataSet
		Try
			If Not String.IsNullOrEmpty(strGuiYmcaID) AndAlso Not String.IsNullOrEmpty(strGuiFundEventID) AndAlso Not String.IsNullOrEmpty(strGuiTransmittalID) Then
				SetFundedDate()
				BindCreditAmount()
				BindTransmittalAmount()
				dsTransaction = CashApplicationBOClass.GetTransactionsByTransmittalID(strGuiYmcaID, strGuiFundEventID, strGuiTransmittalID)
				dtTransactions = FillTransactionTable(dsTransaction)
				Transaction = dtTransactions
                CurrentTransactions = dtTransactions.Copy() 'SP 2014.05.12 BT-2531
                gvTrn.DataSource = dtTransactions
                gvTrn.DataBind()

                'SP 2014.05.12 BT-2531 -Start
                If (HelperFunctions.isNonEmpty(dtTransactions)) Then
                    lblTransmitalAmount.Text = String.Format("{0:0.00}", dtTransactions.Compute("SUM(TransactionAmount)", ""))
                Else
                    lblTransmitalAmount.Text = String.Format("{0:0.00}", 0.0)
                End If
                lblTotalSelectedAmount.Text = "0.00"
                lblTotalAppliedAmount.Text = "0.00"
                'SP 2014.05.12 BT-2531 -End

				dsYmcaReceipts = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaReceipts(strGuiYmcaID)
				If (HelperFunctions.isNonEmpty(dsYmcaReceipts)) Then
					CurrentReceipt = dsYmcaReceipts.Tables(0)
				End If
				SelectedReceiptIndex = -1
				gvReceipts.SelectedIndex = -1
				Me.gvReceipts.DataSource = dsYmcaReceipts
				Me.gvReceipts.DataBind()
				If (HelperFunctions.isNonEmpty(dsYmcaReceipts)) Then
					imgBtnReceipts.Visible = True
				Else
					imgBtnReceipts.Visible = False
				End If
			End If
		Catch ex As Exception
			HelperFunctions.LogException("PouplateTransactionsByTransmittalId-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try
	End Sub
	''This method set the transmittal detail like amoutpaidnotused, amount paid, toatal amount 
	Public Sub BindTransmittalAmount()
		Dim dsTransmittal As DataSet
		Try
			dsTransmittal = CashApplicationBOClass.GetTransmittalDetailsByTransmittalID(SelectedGuiTransmittalID)
			If HelperFunctions.isNonEmpty(dsTransmittal) Then
				If dsTransmittal.Tables(0).Rows.Count > 0 Then
					AmountDue = Convert.ToDecimal(dsTransmittal.Tables(0).Rows(0)("AmountDue"))
					AmountPaid = Convert.ToDecimal(dsTransmittal.Tables(0).Rows(0)("AmountPaid"))
					AmountPaidNotUse = Convert.ToDecimal(IIf(dsTransmittal.Tables(0).Rows(0)("AmountPaidNotUse").ToString() = String.Empty, "0.00", dsTransmittal.Tables(0).Rows(0)("AmountPaidNotUse")))
					txtAmountDue.Text = String.Format("{0:0.00}", AmountDue)
					txtAountPaid.Text = String.Format("{0:0.00}", AmountPaid)
                    txtAmountPaidNotUse.Text = String.Format("{0:0.00}", AmountPaidNotUse)
				End If
			End If
		Catch ex As Exception
			HelperFunctions.LogException("BindTransmittalAmount-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try

	End Sub
	'Set the funded date from database
	Public Sub SetFundedDate()
		Try

			Dim strAcctDate As String
			Dim dsAcctDate As DataSet
			Dim dtmAcctDate As DateTime
			Dim dtmCurrentDate As DateTime

			'Set Current Date
			dtmCurrentDate = System.DateTime.Now.Date
			'Get the accounting date 
			dsAcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
			strAcctDate = dsAcctDate.Tables(0).Rows(0).Item(0).ToString()
			If Not strAcctDate.Equals(String.Empty) Then
				dtmAcctDate = Convert.ToDateTime(strAcctDate).Date
			Else
				dtmAcctDate = System.DateTime.Now.Date
			End If

			If (dtmAcctDate.Year = dtmCurrentDate.Year And dtmAcctDate.Month < dtmCurrentDate.Month) Or dtmAcctDate.Year < dtmCurrentDate.Year Then

				Me.dtUserFundeddate.Text = dtmAcctDate.Date.ToString("MM/dd/yyyy")
			Else

				Me.dtUserFundeddate.Text = dtmCurrentDate.Date.ToString("MM/dd/yyyy")

			End If
		Catch ex As Exception
			HelperFunctions.LogException("SetFundedDate-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)

		End Try
	End Sub
	'Create transmittal schema for final save to the database
	Private Function FillTransactionTable(ByVal dsTransaction As DataSet) As DataTable
		Dim dtTransaction As New DataTable

		Try
			dtTransaction.Columns.Add("Slctd")
			dtTransaction.Columns("Slctd").DefaultValue = False
			dtTransaction.Columns.Add("UniqueId")
			dtTransaction.Columns.Add("FundEventID")
			dtTransaction.Columns.Add("TransactionAmount", GetType(Double))
			dtTransaction.Columns("TransactionAmount").DefaultValue = 0.0
			dtTransaction.Columns.Add("TotalAmountPaidnotUsed", GetType(Double))
			dtTransaction.Columns("TotalAmountPaidnotUsed").DefaultValue = 0.0
			dtTransaction.Columns.Add("TotalAppliedReceiptAmount", GetType(Double))
			dtTransaction.Columns("TotalAppliedReceiptAmount").DefaultValue = 0.0
			dtTransaction.Columns.Add("TotalAppliedCreditAmount", GetType(Double))
			dtTransaction.Columns("TotalAppliedCreditAmount").DefaultValue = 0.0
			dtTransaction.Columns.Add("Balance", GetType(Double))
			dtTransaction.Columns("Balance").DefaultValue = 0.0
			dtTransaction.Columns.Add("FundedDate", GetType(String))
			dtTransaction.Columns("FundedDate").DefaultValue = String.Empty
			dtTransaction.Columns.Add("AccountType", GetType(String))
			dtTransaction.Columns.Add("TransactionDate", GetType(String))
			dtTransaction.Columns.Add("TransactionType", GetType(String))
			dtTransaction.Columns.Add("dtmPaidDate", GetType(String))
			dtTransaction.Columns("dtmPaidDate").DefaultValue = String.Empty

			If HelperFunctions.isNonEmpty(dsTransaction) Then
				Dim drRowItem As DataRow
				For Each drRow As DataRow In dsTransaction.Tables(0).Rows
					drRowItem = dtTransaction.NewRow()
					drRowItem("UniqueId") = drRow("UniqueId")
					drRowItem("FundEventID") = drRow("FundEventID")
					drRowItem("TransactionAmount") = IIf(drRow("TransactionAmount") Is Nothing, 0.0, drRow("TransactionAmount"))
					drRowItem("AccountType") = drRow("AccountType")
					drRowItem("TransactionDate") = drRow("TransactionDate")
					drRowItem("TransactionDate") = drRow("TransactionDate")
					drRowItem("Balance") = drRow("TransactionAmount")
					drRowItem("TransactionType") = drRow("TransactionType")
					dtTransaction.Rows.Add(drRowItem)
				Next
				dtTransaction.AcceptChanges()
			End If
			Return dtTransaction
		Catch ex As Exception
			Throw
		End Try
	End Function
	'Clear the view state 
	Private Sub ClearAll()
		Try
			OriginalCredits = Nothing
			SelectedGuiFundEventID = Nothing
			SelectedGuiTransmittalID = Nothing
			Transaction = Nothing
			AmountPaidNotUse = Nothing
			AmountDue = Nothing
			AmountPaid = Nothing
			YMCAList = Nothing
			SelectedYMCAID = Nothing
		Catch ex As Exception
			HelperFunctions.LogException("ClearAll-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try

	End Sub
	'Bind the Ymca credit amount 
	Private Sub BindCreditAmount()
		Try
			Dim dblCredits As Decimal
			dblCredits = Convert.ToDecimal(YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaCredit(SelectedYMCAID))
			txtCreditAvailable.Text = String.Format("{0:0.00}", dblCredits)
			OriginalCredits = dblCredits
			CurrentTransactions = Nothing
			CurrentReceipt = Nothing
		Catch ex As Exception
			HelperFunctions.LogException("BindCreditAmount-CashApplication-person", ex)
			HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
		End Try
	End Sub
	'Send the last remaining installment of the loan if paid i.e loan is re-paid in full
	Private Sub SendMail(ByVal dtDatatable As DataTable, ByVal strFundIdNo As String)
		Dim obj As MailUtil
		obj = New MailUtil
		'Dim strSSNo As String
		'Dim strFirstname As String
		'Dim strMiddlename As String
		'Dim strLastName As String
		'Dim strParFundIdNo As String
		'Dim strHeading As String
		Dim sbDetails As StringBuilder
		'Dim dtCashAppDataTable As New DataTable

		'Dim drow As DataRow
		Dim dtPersondetails As New DataTable

		Try

			If (HelperFunctions.isNonEmpty(dtDatatable)) Then
				dtPersondetails = dtDatatable.Select("intFundIdNo =" + strFundIdNo).CopyToDataTable()

				If (HelperFunctions.isNonEmpty(dtPersondetails)) Then
					sbDetails = New StringBuilder

					sbDetails.Append("You may now close the loan for following participants as the final payment of their TD Loan has been funded.")

					sbDetails.Append("<table width='50%' border='1' >")
					sbDetails.Append("<tr><td align='center'><B>Fund Id No.</B></td><td align='center'><B>First Name</B></td><td align='center'><B>Last Name</B></td></tr>")
					For Each drow In dtPersondetails.Rows

						sbDetails.Append("<tr>")
						sbDetails.Append("<td>" & drow("intFundIdNo").ToString() & "</td>")
						sbDetails.Append("<td>" & drow("chvFirstName").ToString() & "</td>")
						sbDetails.Append("<td>" & drow("chvLastName").ToString() & "</td>")
						sbDetails.Append("</tr>")

					Next
					sbDetails.Append("</table>")

					obj.MailCategory = "TDLoan"
					If obj.MailService = False Then Exit Sub
					obj.MailFormatType = Mail.MailFormat.Html
					obj.SendCc = ""
					obj.MailMessage = sbDetails.ToString()
					obj.Subject = "The Tax Deferred Loan for the following participant has been repaid in full."

					obj.Send()
				End If
			End If
		Catch
			Throw
		End Try
    End Sub
    'Start: Bala: YRS-AT-2642: Following function is not required. Moved it to LoanClass()
    'send email to default TD loan for paid
    'Private Sub SendLoanDefaultedMail(ByVal dtDatatable As DataTable, ByVal strFundIdNo As String)

    ' This method will be sent email if person defaluted for 
    'Dim obj As MailUtil
    'obj = New MailUtil
    'Dim sbLoanDefaultedDetails As StringBuilder
    'Dim drow As DataRow
    'Dim dtPersondetails As New DataTable

    'Try
    '    If (HelperFunctions.isNonEmpty(dtDatatable)) Then

    '        dtPersondetails = dtDatatable.Select("intFundIdNo =" + strFundIdNo).CopyToDataTable()
    '        If (HelperFunctions.isNonEmpty(dtPersondetails)) Then

    '                sbLoanDefaultedDetails = New StringBuilder
    '                For Each drow In dtPersondetails.Rows
    '                    sbLoanDefaultedDetails.Append("First Name : " + drow("chvFirstName").ToString() + ControlChars.CrLf + "Last Name : " + drow("chvLastName").ToString() + ControlChars.CrLf + "Ymca Name : " + drow("chvYmcaName").ToString() + ControlChars.CrLf + "Ymca Number : " + drow("chrYmcano").ToString() + ControlChars.CrLf + "Social Security No : " + drow("chrSSNo").ToString() + ControlChars.CrLf + "Fund Id Number : " + drow("intFundIdNo").ToString() + ControlChars.CrLf + ControlChars.CrLf)
    '                Next

    '                obj.MailCategory = "TDLoan_Defaulted"
    '                If obj.MailService = False Then Exit Sub

    '                obj.MailMessage = sbLoanDefaultedDetails.ToString()

    '                obj.Subject = "Tax Deferred Loan Defaulted."

    '                obj.Send()
    '            End If
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'send the service fail email
    'End: Bala: YRS-AT-2642: Following function is not required. Moved it to LoanClass()
    Private Sub SendServiceFailedMail(ByVal iLoggedId As Int64)
        Dim obj As MailUtil
        Try
            If iLoggedId > 0 Then

                obj = New MailUtil

                obj.MailCategory = "ADMIN"
                If obj.MailService = False Then Exit Sub

                obj.MailMessage = "ServiceTime and vesting update failed for BatchID :" + Convert.ToString(iLoggedId)

                obj.Subject = "ServiceTime and vesting update failed."

                obj.Send()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'set the selected YMCA search Tab 
    Private Sub SetSelectedYMCASearchTab()
        Try
            tdSearchYMCA.Style.Add("background-color", "#93BEEE")
            tdSearchYMCA.Style.Add("color", "#000000")
            tdSearchPerson.Style.Add("background-color", "#4172A9")
            tdSearchPerson.Style.Add("color", "#ffffff")
            tdSelectTransaction.Style.Add("background-color", "#4172A9")
            tdSelectTransaction.Style.Add("color", "#ffffff")
            lnkSearchYMCA.Visible = False
            lblSearchYMCA.Visible = True
            lnkSearchPerson.Visible = True
            lblSearchPerson.Visible = False
            lnkSelectTransaction.Visible = True
            lblSelectTransaction.Visible = False
            MV.ActiveViewIndex = 0
        Catch ex As Exception
            HelperFunctions.LogException("SetSelectedYMCASearchTab-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'Set the selected Person search & transmittal tab
    Private Sub SetSelectedPersonSearchTab()
        Try
            tdSearchPerson.Style.Add("background-color", "#93BEEE")
            tdSearchPerson.Style.Add("color", "#000000")
            tdSearchYMCA.Style.Add("background-color", "#4172A9")
            tdSearchYMCA.Style.Add("color", "#ffffff")
            tdSelectTransaction.Style.Add("background-color", "#4172A9")
            tdSelectTransaction.Style.Add("color", "#ffffff")
            lnkSearchYMCA.Visible = True
            lblSearchYMCA.Visible = False
            lnkSearchPerson.Visible = False
            lblSearchPerson.Visible = True
            lnkSelectTransaction.Visible = True
            lblSelectTransaction.Visible = False
            MV.ActiveViewIndex = 1
        Catch ex As Exception
            HelperFunctions.LogException("SetSelectedPersonSearchTab-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'Set the selected transaction  tab
    Private Sub SetSelectedTransactionTab()
        Try

            tdSelectTransaction.Style.Add("background-color", "#93BEEE")
            tdSelectTransaction.Style.Add("color", "#000000")
            tdSearchYMCA.Style.Add("background-color", "#4172A9")
            tdSearchYMCA.Style.Add("color", "#ffffff")
            tdSearchPerson.Style.Add("background-color", "#4172A9")
            tdSearchPerson.Style.Add("color", "#ffffff")
            lnkSelectTransaction.Visible = False
            lblSelectTransaction.Visible = True
            lnkSearchPerson.Visible = True
            lblSearchPerson.Visible = False
            lnkSearchYMCA.Visible = True
            lblSearchYMCA.Visible = False
            MV.ActiveViewIndex = 2
        Catch ex As Exception
            HelperFunctions.LogException("SetSelectedTransactionTab-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'This function create transmittal header schema dataset for selected transmittal
    Private Function SelectTransmittalRow(ByVal dstransmittal As DataSet, ByVal strGuiTransmittalID As String) As DataSet

        Try
            Dim drTransmittalRow As DataRow
            Dim dsFinalTransmittal As DataSet
            'Creating the table Transmittals from the YmcaTransmittls datatable
            Dim dtransmittals As New DataTable
            dtransmittals.Columns.Add("Slctd")
            dtransmittals.Columns("Slctd").DefaultValue = "false"
            dtransmittals.Columns.Add("UniqueId")
            dtransmittals.Columns.Add("YmcaId")
            dtransmittals.Columns.Add("AmtDue", GetType(Decimal))
            dtransmittals.Columns("AmtDue").DefaultValue = 0
            dtransmittals.Columns.Add("AmtPaid", GetType(Decimal))
            dtransmittals.Columns("AmtPaid").DefaultValue = 0
            dtransmittals.Columns.Add("AmtCredit", GetType(Decimal))
            dtransmittals.Columns("AmtCredit").DefaultValue = 0
            dtransmittals.Columns.Add("TotAppliedRcpts", GetType(Decimal))
            dtransmittals.Columns("TotAppliedRcpts").DefaultValue = 0
            dtransmittals.Columns.Add("TotAppliedCredit", GetType(Decimal))
            dtransmittals.Columns("TotAppliedCredit").DefaultValue = 0
            dtransmittals.Columns.Add("TotAmountPaidNotUse", GetType(Decimal))
            dtransmittals.Columns("TotAmountPaidNotUse").DefaultValue = 0
            dtransmittals.Columns.Add("OrgAppliedCredit", GetType(Decimal))
            dtransmittals.Columns("OrgAppliedCredit").DefaultValue = 0
            dtransmittals.Columns.Add("OrgAppliedRcpts", GetType(Decimal))
            dtransmittals.Columns("OrgAppliedRcpts").DefaultValue = 0.0

            dtransmittals.Columns.Add("Balance", GetType(Decimal))
            dtransmittals.Columns("Balance").DefaultValue = 0.0
            dtransmittals.Columns.Add("FundedDate", GetType(String))
            dtransmittals.Columns("FundedDate").DefaultValue = String.Empty
            dtransmittals.Columns.Add("dtmPaidDate", GetType(String))
            dtransmittals.Columns("dtmPaidDate").DefaultValue = String.Empty

            If Not dstransmittal Is Nothing Then
                If dstransmittal.Tables(0).Rows.Count > 0 Then
                    drTransmittalRow = dstransmittal.Tables(0).Select("Uniqueid='" + strGuiTransmittalID + "'")(0)
                End If
                dsFinalTransmittal = New DataSet()

                Dim dr As DataRow
                dr = dtransmittals.NewRow()
                dr.Item("Slctd") = True
                dr.Item("UniqueId") = drTransmittalRow("UniqueId")
                dr.Item("YmcaId") = drTransmittalRow("YmcaId")
                dr.Item("AmtDue") = IIf(IsDBNull(drTransmittalRow("AmtDue")), 0, drTransmittalRow("AmtDue"))
                dr.Item("AmtPaid") = IIf(IsDBNull(drTransmittalRow("AmtPaid")), 0, drTransmittalRow("AmtPaid"))
                dr.Item("AmtCredit") = IIf(IsDBNull(drTransmittalRow("AmtCredits")), 0, drTransmittalRow("AmtCredits"))
                dr.Item("TotAppliedRcpts") = 0
                dr.Item("TotAppliedCredit") = 0
                dr.Item("OrgAppliedCredit") = IIf(IsDBNull(drTransmittalRow("AppliedCredit")), 0, drTransmittalRow("AppliedCredit"))
                dr.Item("OrgAppliedRcpts") = IIf(IsDBNull(drTransmittalRow("AppliedReceipts")), 0, drTransmittalRow("AppliedReceipts"))
                dr.Item("Balance") = IIf(IsDBNull(drTransmittalRow("Balance")), 0, drTransmittalRow("Balance"))
                dtransmittals.Rows.Add(dr)
            End If
            dsFinalTransmittal.Tables.Add(dtransmittals)
            Return dsFinalTransmittal
        Catch ex As Exception
            HelperFunctions.LogException("SelectTransmittalRow-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Function

    'SP 2014.02.07 YRS 5.0-842 Add validation for partially funding for loan transactions -Start  
    Private Function ValidateLoanSelectedTransaction() As Boolean
        Dim bIsCheckedAnyLoanTransacts As Boolean
        Dim bIsCheckedAllLoanTransaction As Boolean
        Dim CheckBox As CheckBox
        Dim SubCheckbox As CheckBox
        Try
            bIsCheckedAnyLoanTransacts = False
            bIsCheckedAllLoanTransaction = True
            ''Check if any Loan transacts is checked
            For Each item As GridViewRow In gvTrn.Rows
                CheckBox = Nothing
                CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)
                If CheckBox IsNot Nothing AndAlso CheckBox.Checked Then
                    If item.Cells(5).Text.Trim().ToUpper() = "LIPR" Or item.Cells(5).Text.Trim().ToUpper() = "LRPR" Then
                        bIsCheckedAnyLoanTransacts = True
                        Exit For
                    End If

                End If

            Next
            If (bIsCheckedAnyLoanTransacts) Then
                bIsCheckedAllLoanTransaction = True
                For Each item As GridViewRow In gvTrn.Rows
                    SubCheckbox = Nothing
                    SubCheckbox = DirectCast(item.FindControl("chkSel"), CheckBox)
                    If SubCheckbox IsNot Nothing AndAlso (Not SubCheckbox.Checked) Then
                        If item.Cells(5).Text.Trim().ToUpper() = "LIPR" Or item.Cells(5).Text.Trim().ToUpper() = "LRPR" Then
                            bIsCheckedAllLoanTransaction = False
                            Exit For
                        End If
                    End If
                Next
            End If
            Return bIsCheckedAllLoanTransaction
        Catch ex As Exception

        End Try

    End Function
    'SP 2014.02.07 YRS 5.0-842 Add validation for partially funding for loan transactions -End 

    'SP 2014.05.12 -BT -2531 -Start
    Private Sub ResetImageOnUpdateTotal()
        Dim dAmtCredit As Decimal
        Dim dOriginalCreditAmount As Decimal
        Dim dAmtPaidnotUsed As Decimal
        Dim dOriginalAmountPaidNotUse As Decimal
        Dim dSelectedReceiptAmount As Decimal
        Dim dtReceipts As DataTable
        Dim dReceiptAmount As Decimal

        dAmtCredit = txtCreditAvailable.Text
        dOriginalCreditAmount = OriginalCredits

        dAmtPaidnotUsed = txtAmountPaidNotUse.Text
        dOriginalAmountPaidNotUse = AmountPaidNotUse

        'check credit amount
        If (dAmtCredit = dOriginalCreditAmount) Then
            imgbtnCreditAvailable.ImageUrl = "images/Cash-UnApplied.jpg"
        End If
        'check for amountpaid not used
        If (dAmtPaidnotUsed = dOriginalAmountPaidNotUse) Then
            imgbtnAmountPaidNotUse.ImageUrl = "images/Cash-UnApplied.jpg"
        End If

        'checkign receipt amount
        If (SelectedReceiptIndex >= 0) Then
            dSelectedReceiptAmount = Convert.ToDecimal(gvReceipts.Rows(SelectedReceiptIndex).Cells(4).Text().Trim())

            If Not CurrentReceipt Is Nothing Then
                dtReceipts = CurrentReceipt
            End If
            If dtReceipts.Rows.Count > 0 Then
                dReceiptAmount = Convert.ToDecimal(dtReceipts.Rows(SelectedReceiptIndex).Item("Amount"))
            End If
            If (dReceiptAmount = dSelectedReceiptAmount) Then
                imgBtnReceipts.ImageUrl = "images/Cash-UnApplied.jpg"
            End If
        End If
    End Sub

    Private Sub UpdateSelectedTotalAmount()
        Dim dblSelectedTotalAmount As Decimal
        Dim dblSelectedAppliedlAmount As Decimal  'SP 2014.05.15 -BT-2531 -
        Try

            dblSelectedTotalAmount = 0
            For Each item As GridViewRow In gvTrn.Rows
                Dim checkbox As CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)

                If (Not checkbox Is Nothing) Then

                    If checkbox.Checked Then
                        dblSelectedTotalAmount = dblSelectedTotalAmount + Convert.ToDecimal(item.Cells(2).Text.Trim())
                        dblSelectedAppliedlAmount = dblSelectedAppliedlAmount + (Convert.ToDecimal(item.Cells(2).Text.Trim()) - Convert.ToDecimal(item.Cells(3).Text.Trim()))  'SP 2014.05.15 -BT-2531 -
                    End If
                End If
            Next

            lblTotalSelectedAmount.Text = String.Format("{0:0.00}", dblSelectedTotalAmount)
            lblTotalAppliedAmount.Text = String.Format("{0:0.00}", dblSelectedAppliedlAmount) 'SP 2014.05.15 -BT-2531 -
        Catch
            Throw
        End Try


    End Sub

    Private Sub UpdateTransaction()
        Try
            Dim i As Integer
            Dim dblSelectedTotalAmount As Decimal
            Dim dSelectedAllpiedAmount As Decimal
            Dim dblTotalAmountPaidNotUse As Decimal
            Dim dblReceiptDiff As Decimal
            Dim dblTotalCredits As Decimal
            Dim dblAmountPaidNotUseDiff As Decimal
            Dim dblCreditdDiff As Decimal
            Dim dtTransaction As DataTable
            Dim dtOriginalTransaction As DataTable
            Dim dtReceipts As DataTable
            dblSelectedTotalAmount = 0
            dblTotalAmountPaidNotUse = 0
            dblTotalCredits = 0
            dSelectedAllpiedAmount = 0
            If Not CurrentReceipt Is Nothing Then
                dtReceipts = CurrentReceipt
            End If

            If Not CurrentTransactions Is Nothing Then
                dtTransaction = CurrentTransactions.Copy() 'SP 2014.05.12 -BT-2531
            End If

            For Each item As GridViewRow In gvTrn.Rows
                Dim checkbox As CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)

                If (Not checkbox Is Nothing) Then

                    If checkbox.Checked = False Then
                        dtTransaction.Rows(i).Item("Slctd") = False
                        dblTotalAmountPaidNotUse = Convert.ToDecimal(txtAmountPaidNotUse.Text)
                        dblTotalCredits = Convert.ToDecimal(txtCreditAvailable.Text)
                        dtOriginalTransaction = DirectCast(Transaction, DataTable)

                        If Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAppliedCreditAmount")) <> Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount")) Then
                            dblCreditdDiff = Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAppliedCreditAmount")) - Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount"))
                            txtCreditAvailable.Text = String.Format("{0:0.00}", Math.Round(dblTotalCredits, 2) + Math.Round(dblCreditdDiff, 2))

                            dtTransaction.Rows(i).Item("TotalAppliedCreditAmount") = Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount"))
                            dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))
                        End If

                        If dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount").ToString().Trim() <> dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount").ToString().Trim() Then
                            dblReceiptDiff = Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount")) - Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount"))
                            dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount") = Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount"))
                            dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))
                            dtReceipts.Rows(SelectedReceiptIndex).Item("Amount") = Convert.ToDecimal(dtReceipts.Rows(SelectedReceiptIndex).Item("Amount")) + dblReceiptDiff

                        End If
                        If dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed").ToString().Trim() <> dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed").ToString().Trim() Then
                            dblAmountPaidNotUseDiff = Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed")) - Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed"))
                            txtAmountPaidNotUse.Text = String.Format("{0:0.00}", Math.Round(dblTotalAmountPaidNotUse, 2) + Math.Round(dblAmountPaidNotUseDiff, 2))

                            dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed") = Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed"))
                            dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))
                        End If
                        If Not Convert.ToDecimal(dtTransaction.Rows(i).Item("Balance")) = 0 Then

                            dtTransaction.Rows(i).Item("FundedDate") = String.Empty
                            dtTransaction.Rows(i).Item("dtmPaidDate") = String.Empty

                        End If
                        i = i + 1

                    Else
                        dblSelectedTotalAmount = dblSelectedTotalAmount + dtTransaction.Rows(i).Item("TransactionAmount")
                        dSelectedAllpiedAmount = dSelectedAllpiedAmount + (dtTransaction.Rows(i).Item("TransactionAmount") - dtTransaction.Rows(i).Item("Balance")) 'SP 2014.05.15 -BT-2531 -
                        dtTransaction.Rows(i).Item("Slctd") = True
                        i = i + 1
                    End If


                End If
            Next

            gvTrn.DataSource = dtTransaction
            gvTrn.DataBind()
            CurrentTransactions = dtTransaction.Copy()
            CurrentReceipt = dtReceipts
            lblTotalSelectedAmount.Text = String.Format("{0:0.00}", dblSelectedTotalAmount)
            lblTotalAppliedAmount.Text = String.Format("{0:0.00}", dSelectedAllpiedAmount) 'SP 2014.05.15 -BT-2531 -
        Catch
            Throw
        End Try
    End Sub

    Private Function ValidateUpdateAmountNotClicked() As Boolean
        Dim CheckBox As CheckBox
        Dim SubCheckbox As CheckBox
        Dim bIsCheckedUncheckedbalanceTransacts As Boolean
        Try
            bIsCheckedUncheckedbalanceTransacts = False

            For Each item As GridViewRow In gvTrn.Rows
                CheckBox = Nothing
                CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)
                If CheckBox IsNot Nothing Then
                    If Not CheckBox.Checked And Convert.ToDecimal(item.Cells(2).Text.Trim()) <> Convert.ToDecimal(item.Cells(3).Text.Trim()) Then
                        bIsCheckedUncheckedbalanceTransacts = True
                        Exit For
                    End If
                    'Else

                End If

            Next
            Return bIsCheckedUncheckedbalanceTransacts
        Catch
            Throw
        End Try

    End Function
    'SP 2014.05.12 -BT -2531 -End
#End Region

#Region "Control Events"


    Private Sub btnYmcaFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYmcaFind.Click
        Try
            lblHdr.Text = "Search YMCA"
            YMCAList = Nothing
            SelectedYMCAID = Nothing
            YMCAList = Nothing
            SortExpressionYMCA = String.Empty
            BindYMCAGrid()
            spnSeletedYMCA.InnerText = String.Empty
            spnSelectedPerson.Text = String.Empty
            SortExpressionTransmittal = String.Empty
            SortExpressionPerson = String.Empty
            PersonSearchList = Nothing
            TransmittalSearchList = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("btnYmcaFind_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'SP 2014.05.12 -BT-2531-Start
    'Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
    '	Try

    '		Dim ck1 As CheckBox = CType(sender, CheckBox)
    '		Dim dgItem As GridViewRow = CType(ck1.NamingContainer, GridViewRow)
    '		Dim i As Integer = dgItem.RowIndex
    '           Dim dblSelectedTotalAmount As Decimal
    '		Dim dblTotalAmountPaidNotUse As Decimal
    '		Dim dblReceiptDiff As Decimal
    '		Dim dblTotalCredits As Decimal
    '		Dim dblAmountPaidNotUseDiff As Decimal
    '		Dim dblCreditdDiff As Decimal
    '		Dim dtTransaction As DataTable
    '		Dim dtOriginalTransaction As DataTable
    '		Dim dtReceipts As DataTable
    '           dblSelectedTotalAmount = 0
    '		dblTotalAmountPaidNotUse = 0
    '		dblTotalCredits = 0

    '		If Not CurrentReceipt Is Nothing Then
    '			dtReceipts = CurrentReceipt
    '		End If

    '		If Not CurrentTransactions Is Nothing Then
    '               dtTransaction = CurrentTransactions.Copy()
    '		End If
    '		If ck1.Checked = True Then
    '			dtTransaction.Rows(i).Item("Slctd") = True
    '		Else
    '			dtTransaction.Rows(i).Item("Slctd") = False
    '		End If
    '		If ck1.Checked = False Then
    '			dblTotalAmountPaidNotUse = Convert.ToDecimal(txtAmountPaidNotUse.Text)
    '			dblTotalCredits = Convert.ToDecimal(txtCreditAvailable.Text)
    '			dtOriginalTransaction = DirectCast(Transaction, DataTable)

    '			If Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAppliedCreditAmount")) <> Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount")) Then
    '				dblCreditdDiff = Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAppliedCreditAmount")) - Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount"))
    '				txtCreditAvailable.Text = String.Format("{0:0.00}", Math.Round(dblTotalCredits, 2) + Math.Round(dblCreditdDiff, 2))

    '				dtTransaction.Rows(i).Item("TotalAppliedCreditAmount") = dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount")
    '				dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))
    '			End If

    '			If dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount").ToString().Trim() <> dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount").ToString().Trim() Then
    '				dblReceiptDiff = Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount")) - Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount"))
    '				dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount") = dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount")
    '				dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))
    '				dtReceipts.Rows(SelectedReceiptIndex).Item("Amount") = Convert.ToDecimal(dtReceipts.Rows(SelectedReceiptIndex).Item("Amount")) + dblReceiptDiff

    '			End If
    '			If dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed").ToString().Trim() <> dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed").ToString().Trim() Then
    '				dblAmountPaidNotUseDiff = Convert.ToDecimal(dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed")) - Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed"))
    '				txtAmountPaidNotUse.Text = String.Format("{0:0.00}", Math.Round(dblTotalAmountPaidNotUse, 2) + Math.Round(dblAmountPaidNotUseDiff, 2))

    '				dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed") = dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed")
    '				dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))
    '			End If
    '			If Not Convert.ToDecimal(dtTransaction.Rows(i).Item("Balance")) = 0 Then

    '				dtTransaction.Rows(i).Item("FundedDate") = String.Empty
    '				dtTransaction.Rows(i).Item("dtmPaidDate") = String.Empty

    '			End If

    '			gvTrn.DataSource = dtTransaction
    '			gvTrn.DataBind()
    '               CurrentTransactions = dtTransaction.Copy()
    '			CurrentReceipt = dtReceipts
    '		End If
    '	Catch ex As Exception
    '		HelperFunctions.LogException("Check_Clicked-CashApplication-person", ex)
    '		HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
    '	End Try
    'End Sub
    'SP 2014.05.12 -BT-2531-End

    Private Sub gvYmca_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvYmca.PageIndexChanging
        Try
            Me.gvYmca.PageIndex = e.NewPageIndex
            BindYMCAGrid()
        Catch ex As Exception
            HelperFunctions.LogException("gvYmca_PageIndexChanging-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvYmca_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvYmca.RowCommand
        Try
            Dim strYmcaId As String
            If e.CommandName.ToLower() = "select" Then
                strYmcaId = Convert.ToString(e.CommandArgument).Trim

                For Each item As GridViewRow In gvYmca.Rows
                    If (item.Cells(2).Text.Trim() = strYmcaId) Then
                        lblHdr.Text = item.Cells(2).Text.Trim() + " - " + item.Cells(3).Text.Trim()
                        spnSeletedYMCA.InnerText = lblHdr.Text
                        SelectedYMCAID = gvYmca.DataKeys(item.RowIndex).Item("UniqueId").ToString()
                        trYmcaSearch.Visible = True
                        ScriptManager.RegisterClientScriptBlock(Me.up, Me.GetType(), "YmcaSearch", "$('#divYmcaSearch').toggle(100);", True)
                        SetSelectedPersonSearchTab()
                        SortExpressionPerson = String.Empty
                        PersonSearchList = Nothing
                        Me.SearchParticipantbyYmcaId(SelectedYMCAID, String.Empty, String.Empty, String.Empty, String.Empty)
                        SelectedGuiTransmittalID = Nothing
                        SelectedGuiFundEventID = Nothing
                        gvTransmittal.DataSource = Nothing
                        gvTransmittal.DataBind()
                        SortExpressionTransmittal = String.Empty
                        TransmittalSearchList = Nothing
                        spnSelectTransmittal.Visible = False
                        lblHdr.Text = "Select Person & Transmittal"
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            HelperFunctions.LogException("gvYmca_RowCommand-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub btnYmcaClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYmcaClear.Click
        Try
            TextBoxCity.Text = String.Empty
            TextBoxState.Text = String.Empty
            TextBoxYmcaNo.Text = String.Empty
            TextBoxYmcaName.Text = String.Empty
            Me.gvYmca.Visible = False
            Me.gvYmca.DataSource = Nothing
            Me.gvYmca.DataBind()
            Me.LabelRecordNotFound.Visible = False
            ClearAll()
            lblHdr.Text = "Search YMCA"
            spnSelectYMCA.Visible = False
            gvPerson.DataSource = Nothing
            gvPerson.DataBind()
            gvTransmittal.DataSource = Nothing
            gvTransmittal.DataBind()
            gvReceipts.DataSource = Nothing
            gvReceipts.DataBind()
            SortExpressionTransmittal = String.Empty
            SortExpressionPerson = String.Empty
            SortExpressionYMCA = String.Empty
            PersonSearchList = Nothing
            TransmittalSearchList = Nothing
            YMCAList = Nothing
            spnSeletedYMCA.InnerText = String.Empty
            spnSelectedPerson.Text = String.Empty
        Catch ex As Exception
            HelperFunctions.LogException("btnYmcaClear_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub gvPerson_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPerson.PageIndexChanging
        Try
            Me.gvPerson.PageIndex = e.NewPageIndex
            Me.SearchParticipantbyYmcaId(SelectedYMCAID, txtSearchSSN.Text.Trim(), txtSearchFundNo.Text.Trim(), txtSearchFirstName.Text.Trim(), txtSearchLastName.Text.Trim())
        Catch ex As Exception
            HelperFunctions.LogException("gvPerson_PageIndexChanging-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvPerson_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPerson.RowCommand

        Dim strFundNo As String
        Try
            If e.CommandName.ToLower() = "select" Then
                strFundNo = Convert.ToString(e.CommandArgument).Trim
                FundIdNo = strFundNo
                For Each item As GridViewRow In gvPerson.Rows
                    If (item.Cells(4).Text.Trim() = strFundNo) Then
                        spnSelectedPerson.Text = "Fund No: " + strFundNo + " - " + item.Cells(6).Text.Trim() + ", " + item.Cells(5).Text.Trim()
                        SelectedGuiFundEventID = gvPerson.DataKeys(item.RowIndex).Item("FundEventID").ToString()
                        trSearchPanel.Visible = True
                        SelectedGuiTransmittalID = Nothing
                        SortExpressionTransmittal = String.Empty
                        TransmittalSearchList = Nothing
                        gvTransmittal.PageIndex = 0
                        'ScriptManager.RegisterClientScriptBlock(Me.updtContent, Me.GetType(), "PersonSearch", "$('#divSearchCriteria').toggle(100);", True)
                        PopulateTransmittalByFundEventId(SelectedYMCAID, SelectedGuiFundEventID)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvPerson_RowCommand-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try


    End Sub

    Private Sub gvTransmittal_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTransmittal.PageIndexChanging
        Try
            Me.gvTransmittal.PageIndex = e.NewPageIndex
            Me.PopulateTransmittalByFundEventId(SelectedYMCAID, SelectedGuiFundEventID)
        Catch ex As Exception
            HelperFunctions.LogException("gvTransmittal_PageIndexChanging-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvTransmittal_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransmittal.RowCommand
        Dim strTransmittalNo As String
        Dim dsTransmittal As DataSet
        Try
            If e.CommandName.ToLower() = "select" Then
                strTransmittalNo = Convert.ToString(e.CommandArgument).Trim

                spnSelectedTransmittal.InnerText = "Transmittal No: " + strTransmittalNo + " "
                spnFundDetails.Text = spnSelectedPerson.Text

                For Each item As GridViewRow In gvTransmittal.Rows
                    If (item.Cells(2).Text.Trim() = strTransmittalNo) Then
                        SelectedGuiTransmittalID = gvTransmittal.DataKeys(item.RowIndex).Item("UniqueId").ToString()
                        Exit For
                    End If
                Next

                If Not String.IsNullOrEmpty(strTransmittalNo) Then

                    PouplateTransactionsByTransmittalId(SelectedYMCAID, SelectedGuiFundEventID, SelectedGuiTransmittalID)
                    ResetImages()
                    SetSelectedTransactionTab()
                    lblHdr.Text = "Select Transaction(s) & Apply Funds"
                Else
                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_TRANSMITTAL, EnumMessageTypes.Error, Nothing)

                End If

            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvTransmittal_RowCommand-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)

        End Try
    End Sub

    Private Sub btnPersonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonFind.Click

        Try
            lblHdr.Text = "Select Person & Transmittal"
            spnSelectedPerson.Text = String.Empty
            PersonSearchList = Nothing
            SortExpressionPerson = String.Empty
            TransmittalSearchList = Nothing
            SortExpressionTransmittal = String.Empty
            Me.SearchParticipantbyYmcaId(SelectedYMCAID, txtSearchSSN.Text.Trim(), txtSearchFundNo.Text.Trim(), txtSearchFirstName.Text.Trim(), txtSearchLastName.Text.Trim())
        Catch ex As Exception
            HelperFunctions.LogException("btnPersonFind_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub gvTransactions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrn.RowDataBound
        Try
            Select Case e.Row.RowType
                Case DataControlRowType.DataRow
                    'totAmount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TransactionAmount")) 'SP BT-2531 -Commented
                    If (Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Balance")) = 0) Then
                        e.Row.Cells(0).Style.Add("background-color", "#93BEEE")
                    End If
                    'SP BT-2531 -Commented -Start
                    'Case DataControlRowType.Footer
                    '	Dim lblamount As Label = DirectCast(e.Row.FindControl("lbltotal"), Label)
                    '	e.Row.Cells(2).Text = String.Format("{0:0.00}", totAmount)
                    '	e.Row.Cells(2).Font.Bold = True
                    'SP BT-2531 -Commented -End
            End Select
        Catch ex As Exception
            HelperFunctions.LogException("gvTransactions_RowDataBound-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


    Private Sub btnSearchYmcaClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchYmcaClose.Click
        Try
            ClearAll()
            Response.Redirect("MainWebForm.aspx", True)
        Catch ex As Exception
            HelperFunctions.LogException("btnSearchYmcaClose_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnSearchYmcaNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchYmcaNext.Click
        Try
            If String.IsNullOrEmpty(SelectedYMCAID) Then
                SetSelectedYMCASearchTab()
                HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_YMCA, EnumMessageTypes.Error, Nothing)
                lblHdr.Text = "Search YMCA"
            Else
                SetSelectedPersonSearchTab()
                lblHdr.Text = "Select Person & Transmittal"
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnSearchYmcaNext_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub btnPersonPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonPrevious.Click
        Try
            SetSelectedYMCASearchTab()
            lblHdr.Text = "Search YMCA"
        Catch ex As Exception
            HelperFunctions.LogException("btnPersonPrevious_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub btnPersonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonClose.Click
        Try
            ClearAll()
            Response.Redirect("MainWebForm.aspx", True)
        Catch ex As Exception
            HelperFunctions.LogException("btnPersonClose_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnPersonNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonNext.Click
        Try
            If Not String.IsNullOrEmpty(SelectedYMCAID) AndAlso Not String.IsNullOrEmpty(SelectedGuiFundEventID) AndAlso Not String.IsNullOrEmpty(SelectedGuiTransmittalID) Then
                SetSelectedTransactionTab()
                lblHdr.Text = "Select Transaction(s) & Apply Funds"
            ElseIf (String.IsNullOrEmpty(SelectedGuiFundEventID) Or String.IsNullOrEmpty(SelectedGuiTransmittalID)) And Not String.IsNullOrEmpty(SelectedYMCAID) Then
                SetSelectedPersonSearchTab()
                If (String.IsNullOrEmpty(SelectedGuiFundEventID)) Then
                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_PERSON, EnumMessageTypes.Error, Nothing)
                Else
                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_TRANSMITTAL, EnumMessageTypes.Error, Nothing)
                End If

                lblHdr.Text = "Select Person & Transmittal"
            Else
                SetSelectedYMCASearchTab()
                HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_YMCA, EnumMessageTypes.Error, Nothing)
                lblHdr.Text = "Search YMCA"
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnPersonNext_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub btnTransactionPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransactionPrevious.Click
        Try
            btnTransactionProcess.Enabled = True
            If Not String.IsNullOrEmpty(SelectedGuiFundEventID) And Not String.IsNullOrEmpty(SelectedYMCAID) Then
                SetSelectedPersonSearchTab()
                lblHdr.Text = "Select Person & Transmittal"
            Else
                SetSelectedYMCASearchTab()
                HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_YMCA, EnumMessageTypes.Error, Nothing)
                lblHdr.Text = "Search YMCA"
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnTransactionPrevious_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub btnTransactionClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransactionClose.Click
        Try
            ClearAll()
            MV.ActiveViewIndex = 0
            Response.Redirect("MainWebForm.aspx", True)
        Catch ex As Exception
            HelperFunctions.LogException("btnTransactionClose_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try


    End Sub

    Private Sub btnTransactionProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransactionProcess.Click
        Try
            Dim bServiceUpdate As Boolean
            Dim iLoggedBatchID As Int64
            Dim dsAcctDate As DataSet
            Dim dtmMinFundedDateRange As DateTime
            Dim dtmMaxFundedDateRange As DateTime
            Dim dtFundedDate As DateTime
            Dim strAcctDate As String
            Dim dtmAcctDate As DateTime
            Dim dtmCurrentDate As DateTime
            Dim dsTransmittal As DataSet
            Dim dblBalance As Decimal
            Try
                Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                End If

                If Page.IsValid Then

                    'get the accounting date
                    dsAcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
                    If (HelperFunctions.isNonEmpty(dsAcctDate) And dsAcctDate.Tables(0).Rows.Count > 0) Then
                        strAcctDate = dsAcctDate.Tables(0).Rows(0).Item(0).ToString()
                        If String.IsNullOrEmpty(strAcctDate) Then
                            strAcctDate = System.DateTime.Now.Date.ToString()
                        End If
                    Else
                        strAcctDate = System.DateTime.Now.Date.ToString()
                    End If

                    ''Set Current Date
                    dtmCurrentDate = System.DateTime.Now.Date
                    ''Get the accounting date 

                    If Not strAcctDate.Equals(String.Empty) Then
                        dtmAcctDate = Convert.ToDateTime(strAcctDate).Date
                    Else
                        dtmAcctDate = System.DateTime.Now.Date
                    End If

                    If (dtmAcctDate.Year = dtmCurrentDate.Year And dtmAcctDate.Month < dtmCurrentDate.Month) Or dtmAcctDate.Year < dtmCurrentDate.Year Then
                        dtmMinFundedDateRange = dtmAcctDate.AddDays(-(dtmAcctDate.Day - 1))
                        dtmMaxFundedDateRange = dtmAcctDate
                    Else
                        dtmMinFundedDateRange = dtmCurrentDate.AddDays(-(dtmCurrentDate.Day - 1))
                        dtmMaxFundedDateRange = dtmCurrentDate

                    End If

                    'This method will update transactions if user clicks directly on process button instead of clicking update amount button.
                    'SP 2014.05.12 BT-2531 
                    If (ValidateUpdateAmountNotClicked()) Then
                        HelperFunctions.ShowMessageToUser(Resources.CashApplication.TRANSACTION_TOTAL_MESSAGE, EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If

                    'Create Transmittal Header records for finally Saved
                    If Not (ViewState("Transmittal") Is Nothing) Then

                        dsTransmittal = DirectCast(ViewState("Transmittal"), DataSet)
                        If (HelperFunctions.isNonEmpty(dsTransmittal) And dsTransmittal.Tables(0).Rows.Count > 0) Then
                            dsTransmittal = SelectTransmittalRow(dsTransmittal, SelectedGuiTransmittalID)
                        End If
                    End If

                    'Validate the Funded Date
                    If Not dtUserFundeddate.Text.Trim() = "" Then
                        dtFundedDate = CType(dtUserFundeddate.Text, DateTime).Date
                        If dtFundedDate < dtmMinFundedDateRange Or dtFundedDate > dtmMaxFundedDateRange Then
                            If dtmMinFundedDateRange.Month = dtmCurrentDate.Month Then
                                HelperFunctions.ShowMessageToUser(Resources.CashApplication.INVALID_FUNDED_DATE_RANGE, EnumMessageTypes.Error, Nothing)
                            Else
                                HelperFunctions.ShowMessageToUser(Resources.CashApplication.CURRENT_BUSSINESS_FUNDED_DATE, EnumMessageTypes.Error, Nothing)
                            End If

                            Exit Sub
                        End If
                    Else
                        HelperFunctions.ShowMessageToUser(Resources.CashApplication.EMPTY_FUNDED_DATE, EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If

                    'SP 2014.02.07 Add validation for partially funding for loan transactions -Start 
                    'validate if loan related transacts is partially checked
                    If (Not ValidateLoanSelectedTransaction()) Then
                        HelperFunctions.ShowMessageToUser(Resources.CashApplication.PARTIAL_LOAN_TRANSACTION_FUNDED, EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If
                    'SP 2014.02.07 Add validation for partially funding for loan transactions -End

                    Dim dtTransactions As DataTable
                    Dim dtReceipts As DataTable

                    Dim dblTotalPaid As Decimal
                    'Post the payment to the Transmittals
                    dblTotalPaid = 0

                    dtTransactions = DirectCast(CurrentTransactions, DataTable).Copy() 'SP 2014.05.12 -BT-2531
                    dtReceipts = CurrentReceipt

                    Dim dblTotAppliedRcpts As Decimal
                    Dim dblTotAvail As Decimal
                    Dim dblTotCredit As Decimal
                    Dim dblTotAmountPaidnotUse As Decimal
                    Dim sbTransmittalsId As New StringBuilder

                    'Check If any Transaction selected or not
                    Dim iCheckedIDs As Int64 = (From msgRow In gvTrn.Rows
                    Where DirectCast(msgRow.FindControl("chkSel"), CheckBox).Checked
                    Select (gvTrn.DataKeys(msgRow.RowIndex).Value.ToString())).Count()

                    If (iCheckedIDs <= 0) Then
                        HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_TRANSACTION, EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If

                    'Validation for Balance amount is not funded
                    If Not dtReceipts Is Nothing Then
                        If dtReceipts.Rows.Count > 0 And SelectedReceiptIndex > -1 Then
                            If dtReceipts.Rows(SelectedReceiptIndex).Item("Amount") > 0 Then
                                dblTotAvail = dtReceipts.Rows(SelectedReceiptIndex).Item("Amount")
                            Else
                                dblTotAvail = 0
                            End If
                        End If
                    End If

                    'Check If any Partial amount is applied
                    Dim checkbox As CheckBox
                    For Each drRow As GridViewRow In gvTrn.Rows
                        checkbox = DirectCast(drRow.FindControl("chkSel"), CheckBox)
                        If (Not checkbox Is Nothing And checkbox.Checked) Then
                            If Math.Round(Convert.ToDecimal(drRow.Cells(3).Text), 2) <> 0 Then
                                If Convert.ToDecimal(txtAmountPaidNotUse.Text > 0) Or Convert.ToDecimal(txtCreditAvailable.Text) > 0 Or dblTotAvail > 0 Then
                                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.TRANSACTION_NOT_FUNDED, EnumMessageTypes.Error, Nothing)
                                    Exit Sub
                                Else
                                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.PARTIAL_FUNDING_TRANSACTION, EnumMessageTypes.Error, Nothing)
                                    Exit Sub
                                End If
                            End If
                        End If
                    Next

                    Dim i As Integer

                    Dim iIndexReceipt As Integer = -1
                    If Not SelectedReceiptIndex = -1 Then
                        If gvReceipts.Rows.Count > 0 Then
                            iIndexReceipt = SelectedReceiptIndex
                        End If
                    End If

                    'Add the sum of total selected transaction applied Credit/Applied receipt/applied amount not use amount
                    If Not dsTransmittal Is Nothing Then
                        For Each item As GridViewRow In gvTrn.Rows
                            checkbox = Nothing
                            checkbox = DirectCast(item.FindControl("chkSel"), CheckBox)
                            For Each drRow As DataRow In dtTransactions.Rows

                                If (drRow("uniqueId") = gvTrn.DataKeys(item.RowIndex).Value) Then

                                    If Math.Round(Convert.ToDecimal(drRow("Balance")), 2) = 0 And checkbox.Checked Then
                                        dblTotAppliedRcpts += drRow("TotalAppliedReceiptAmount")
                                        dblTotCredit += drRow("TotalAppliedCreditAmount")
                                        dblTotAmountPaidnotUse += drRow("TotalAmountPaidnotUsed")
                                        drRow("FundedDate") = Me.dtUserFundeddate.Text
                                        drRow("dtmPaidDate") = Me.dtUserFundeddate.Text

                                    End If
                                End If
                            Next
                        Next

                        'Setting total applied recripts & credit amount in a transmittal
                        dsTransmittal.Tables(0).Rows(0)("TotAppliedRcpts") = dsTransmittal.Tables(0).Rows(0)("TotAppliedRcpts") + dblTotAppliedRcpts
                        dsTransmittal.Tables(0).Rows(0)("TotAppliedCredit") = dsTransmittal.Tables(0).Rows(0)("TotAppliedCredit") + dblTotCredit
                        dsTransmittal.Tables(0).Rows(0)("Slctd") = "True"
                        dsTransmittal.Tables(0).Rows(0)("TotAmountPaidNotUse") = dsTransmittal.Tables(0).Rows(0)("TotAmountPaidNotUse") + dblTotAmountPaidnotUse
                        dsTransmittal.Tables(0).Rows(0)("FundedDate") = Me.dtUserFundeddate.Text
                        dsTransmittal.Tables(0).Rows(0)("dtmPaidDate") = Me.dtUserFundeddate.Text

                        If Not (dtTransactions Is Nothing) Then
                            For i = 0 To dtTransactions.Rows.Count - 1
                                If Convert.ToBoolean(dtTransactions.Rows(i)("Slctd")) = True Then

                                    'l_double_TotCredit += dtTransactions.Rows(i).Item("TotalAppliedCreditAmount") - dtTransactions.Rows(i).Item("OrgAppliedCredit")
                                    If Math.Round(Convert.ToDecimal(dtTransactions.Rows(i)("Balance")), 2) = 0 Then
                                        dtTransactions.Rows(i)("FundedDate") = Me.dtUserFundeddate.Text
                                        dtTransactions.Rows(i)("dtmPaidDate") = Me.dtUserFundeddate.Text

                                    End If

                                End If
                            Next


                            'check if amouunt is not applied on any transaction(s)
                            If dblTotAppliedRcpts = 0 And dblTotCredit = 0 And dblTotAmountPaidnotUse = 0 Then
                                HelperFunctions.ShowMessageToUser(Resources.CashApplication.TRANSACTION_NOT_FUNDED, EnumMessageTypes.Error, Nothing)
                                Exit Sub
                            End If

                            Dim dsLoanPersonalDetails As DataSet
                            'Dim bflag As Boolean

                            'Dim l_double_PayAmount As Double
                            'Dim l_string_Output As String
                            'bflag = True

                            Dim l_datarow_Receipts As DataRow
                            If iIndexReceipt >= 0 Then
                                l_datarow_Receipts = dtReceipts.Rows(iIndexReceipt)
                            Else
                                l_datarow_Receipts = Nothing
                            End If

                            'Save the transaction for funding
                            dsLoanPersonalDetails = YMCARET.YmcaBusinessObject.CashApplicationBOClass.SaveTransmittals(dsTransmittal.Tables(0), l_datarow_Receipts, dtTransactions, dblTotAppliedRcpts, strAcctDate, bServiceUpdate, iLoggedBatchID)
                            ' this part will do email sending. 
                            If Not dsLoanPersonalDetails Is Nothing And (Not FundIdNo Is Nothing) Then
                                'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                                'Dim dtIDMFTList As New DataTable 'AA:04.27.2016 YRS-AT-2830 Added to copy the files in IDM
                                'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                                If dsLoanPersonalDetails.Tables.Count > 0 Then
                                    ' email will be sent if the last remaining installment of the loan if paid i.e loan is re-paid in full
                                    If dsLoanPersonalDetails.Tables("LoanpaidPersonDetails").Rows.Count > 0 Then
                                        Try
                                            'start:AA:04.12.2016 YRS-AT-2830 Changed to call from common function where it will close loan and send email 
                                            'SendMail(dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"), FundIdNo)

                                            'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                                            'Call New LoanClass().SendLoanPaidClosedMail(dsLoanPersonalDetails.Tables("LoanpaidPersonDetails").Select(String.Format("intFundIdNo =" + FundIdNo)).CopyToDataTable(), dtIDMFTList)
                                            Call New LoanClass().SendLoanPaidClosedMail(dsLoanPersonalDetails.Tables("LoanpaidPersonDetails").Select(String.Format("intFundIdNo =" + FundIdNo)).CopyToDataTable())
                                            'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                                            'End:AA:04.12.2016 YRS-AT-2830: Changed to call from common function where it will close loan and send email 
                                        Catch ex As Exception
                                            HelperFunctions.ShowMessageToUser(Resources.CashApplication.FAILURE_SENDING_PAID_LOAN_EMAIL, EnumMessageTypes.Error, Nothing)
                                            HelperFunctions.LogException("btnTransactionProcess_Click_sendmail_CashApplication-person", ex)
                                        End Try

                                    End If

                                    'Email sent for defaulted loan
                                    If dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails").Rows.Count > 0 Then
                                        Try
                                            'Start: Bala: YRS-AT-2642: Remove ssno from sending email.
                                            'SendLoanDefaultedMail(dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"), FundIdNo)
                                            'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                                            'Call New LoanClass().SendLoanDefaultedClosedMail(dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails").Select(String.Format("[Fund Id Number] ={0}", FundIdNo)).CopyToDataTable(), dtIDMFTList) 'AA:04.12.2016 YRS-AT-2830:Changed to copy idm the file
                                            Call New LoanClass().SendLoanDefaultedClosedMail(dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails").Select(String.Format("[Fund Id Number] ={0}", FundIdNo)).CopyToDataTable())
                                            'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                                            'End: Bala: YRS-AT-2642: Remove ssno from sending email.
                                        Catch ex As Exception
                                            HelperFunctions.ShowMessageToUser(Resources.CashApplication.FAILURE_SENDING_DEFAULT_LOAN_EMAIL, EnumMessageTypes.Error, Nothing)
                                            HelperFunctions.LogException("btnTransactionProcess_Click_SendLoanDefaultedMail_CashApplication_person", ex)
                                        End Try

                                    End If
                                    'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                                    ''Start:AA:04.27.2016 YRS-AT-2830:Added below lines to copy the files in the IDM 
                                    'If HelperFunctions.isNonEmpty(dtIDMFTList) Then
                                    '    Session("FTFileList") = dtIDMFTList

                                    '    'Call the ASPX to copy the file.
                                    '    Dim popupScript As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp_5', " & _
                                    '    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"

                                    '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript5", popupScript, True)

                                    'End If
                                    ''End:AA:04.27.2016 YRS-AT-2830:Added below lines to copy the files in the IDM 
                                    'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                                End If
                            End If

                            'Email sent when service update failed
                            If Not bServiceUpdate Then
                                Try
                                    SendServiceFailedMail(iLoggedBatchID)
                                Catch ex As Exception
                                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.FAILURE_VESTING_SERVICE_UPDATE, EnumMessageTypes.Error, Nothing)
                                    HelperFunctions.LogException("btnTransactionProcess_Click_SendServiceFailedMail_CashApplication_person", ex)
                                End Try

                            End If

                            'SP 2014.05.12 BT-2531 -Start
                            Transaction = Nothing
                            CurrentTransactions = Nothing
                            lblTotalSelectedAmount.Text = "0.00"
                            lblTotalAppliedAmount.Text = "0.00"
                            'SP 2014.05.12 BT-2531  -End

                            PouplateTransactionsByTransmittalId(SelectedYMCAID, SelectedGuiFundEventID, SelectedGuiTransmittalID)

                            'check that if grid has any records then check any 
                            'source of income is available then stay in current tab else set the second tab
                            If Not gvTrn Is Nothing And gvTrn.Rows.Count > 0 Then
                                If HelperFunctions.isEmpty(CurrentReceipt) And OriginalCredits <= 0 And AmountPaidNotUse <= 0 Then
                                    SelectedGuiTransmittalID = Nothing
                                    TransmittalSearchList = Nothing
                                    SortExpressionTransmittal = String.Empty
                                    SetSelectedPersonSearchTab()
                                    PopulateTransmittalByFundEventId(SelectedYMCAID, SelectedGuiFundEventID)
                                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SUCCESSFULL_PROCESS, EnumMessageTypes.Success, Nothing)
                                Else
                                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SUCCESSFULL_PROCESS, EnumMessageTypes.Success, Nothing)
                                End If


                            Else
                                SelectedGuiTransmittalID = Nothing
                                SetSelectedPersonSearchTab()
                                TransmittalSearchList = Nothing
                                SortExpressionTransmittal = String.Empty
                                PopulateTransmittalByFundEventId(SelectedYMCAID, SelectedGuiFundEventID)
                                HelperFunctions.ShowMessageToUser(Resources.CashApplication.SUCCESSFULL_PROCESS, EnumMessageTypes.Success, Nothing)
                            End If

                            ResetImages()

                        End If ' For dtTransmittals is nothing

                    End If

                End If
            Catch ex As Exception
                HelperFunctions.LogException("btnTransactionProcess_Click-CashApplication-person", ex)
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            End Try
        Catch ex As Exception

        End Try
    End Sub


    Private Sub imgbtnAmountPaidNotUse_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnAmountPaidNotUse.Click

        Dim dtTransaction As DataTable
        Dim dtOriginalTransaction As DataTable
        Dim dblAmtPaidnotUsed As Decimal
        Dim dblBalance As Decimal
        Dim dblTempAmountPaidnotUsed As Decimal
        Dim dblOriginalAmountPaidNotUse As Decimal

        Dim drArry As DataRow() 'SP 2014.05.15 -BT-2531
        Dim dr As DataRow 'SP 2014.05.15 -BT-2531
        Try
            dblAmtPaidnotUsed = txtAmountPaidNotUse.Text
            dblOriginalAmountPaidNotUse = AmountPaidNotUse
            dtTransaction = DirectCast(CurrentTransactions, DataTable).Copy() 'SP 2014.05.12 -BT-2531
            dtOriginalTransaction = DirectCast(Transaction, DataTable).Copy() 'SP 2014.05.12 -BT-2531

            'Update total selected amount
            UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531

            'For Un-applied Amountpaidnotused amount in all transactions
            If (dblAmtPaidnotUsed <> dblOriginalAmountPaidNotUse) Then
                HiddenFieldDirty.Value = "false"
                imgbtnAmountPaidNotUse.ImageUrl = "images/Cash-UnApplied.jpg"
                For i = 0 To dtTransaction.Rows.Count - 1

                    dblTempAmountPaidnotUsed = Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAmountPaidnotUsed"))
                    dtTransaction.Rows(i).Item("TotalAmountPaidnotUsed") = dblTempAmountPaidnotUsed

                    dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))

                    If Not Math.Round(Convert.ToDecimal(dtTransaction.Rows(i).Item("Balance")), 2) = 0 Then
                        dtTransaction.Rows(i).Item("FundedDate") = String.Empty
                        dtTransaction.Rows(i).Item("dtmPaidDate") = String.Empty
                        'dtTransaction.Rows(i).Item("Slctd") = "false"
                    End If
                Next
                gvTrn.DataSource = dtTransaction
                gvTrn.DataBind()

                txtAmountPaidNotUse.Text = String.Format("{0:0.00}", dblOriginalAmountPaidNotUse)
                CurrentTransactions = dtTransaction.Copy() 'SP 2014.05.12 -BT-2531
                'Update total selected amount
                UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531

                Exit Sub
            End If

            dtTransaction.PrimaryKey = {dtTransaction.Columns("uniqueId")}
            'For Applied Amountpaidnotused amount in all transactions
            For Each item As GridViewRow In gvTrn.Rows

                dblAmtPaidnotUsed = txtAmountPaidNotUse.Text
                Dim checkbox As CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)
                If (Not checkbox Is Nothing And checkbox.Checked) Then 'find checked checkbox

                    ' For Each dr As DataRow In dtTransaction.Rows  'SP 2014.05.15 -BT-2531
                    'SP 2014.05.15 -BT-2531 -Added datable select
                    drArry = dtTransaction.Select("uniqueId=" + "'" + gvTrn.DataKeys(item.RowIndex).Value + "'")
                    If (drArry.Length > 0) Then
                        dr = drArry(0)
                        'SP 2014.05.15 -BT-2531 -
                        dr("Slctd") = True
                        dblBalance = dr("Balance")
                        If (dblBalance > 0 And dblAmtPaidnotUsed > 0) Then    'Only selected transaction
                            imgbtnAmountPaidNotUse.ImageUrl = "images/ButtonCashApp.gif"
                            HiddenFieldDirty.Value = "true"
                            If (dblBalance <= dblAmtPaidnotUsed) Then
                                dr("Slctd") = True
                                dr("TotalAmountPaidnotUsed") = Convert.ToDecimal(dr("TotalAmountPaidnotUsed")) + Convert.ToDecimal(dr("Balance"))
                                dr("Balance") = Convert.ToDecimal(dr("TransactionAmount")) - (Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("TotalAmountPaidnotUsed")))
                                txtAmountPaidNotUse.Text = String.Format("{0:0.00}", (dblAmtPaidnotUsed - dr("TotalAmountPaidnotUsed")))
                                dblBalance = 0
                            End If
                            If (dblBalance >= dblAmtPaidnotUsed) Then
                                dr("Slctd") = True
                                dr("TotalAmountPaidnotUsed") = Convert.ToDecimal(dr("TotalAmountPaidnotUsed")) + dblAmtPaidnotUsed
                                dr("Balance") = Convert.ToDecimal(dr("TransactionAmount")) - (Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("TotalAmountPaidnotUsed")))
                                txtAmountPaidNotUse.Text = "0.00"
                            End If
                            If dblBalance = 0 Then
                                If Math.Round(Convert.ToDecimal(dr("Balance")), 2) = dblBalance Then
                                    dr("FundedDate") = Me.dtUserFundeddate.Text
                                    dr("dtmPaidDate") = Me.dtUserFundeddate.Text
                                Else
                                    dr("FundedDate") = String.Empty
                                    dr("dtmPaidDate") = String.Empty
                                End If
                            End If
                        End If
                    End If
                    ' Next 'SP 2014.05.15 -BT-2531 -

                End If
            Next
            gvTrn.DataSource = dtTransaction
            gvTrn.DataBind()
            'Update total selected amount
            UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531
            'for setting imgae
            'If (AmountPaidNotUse = Convert.ToDouble(txtAmountPaidNotUse.Text)) Then
            '	imgbtnAmountPaidNotUse.ImageUrl = "images/Cash-UnApplied.jpg"
            'End If
            CurrentTransactions = dtTransaction.Copy() 'SP 2014.05.12 -BT-2531
        Catch ex As Exception
            HelperFunctions.LogException("imgbtnAmountPaidNotUse_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub imgBtnReceipts_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnReceipts.Click
        Dim dtTransaction As DataTable
        Dim dtOriginalTransaction As DataTable
        Dim dblAmtCredit As Decimal
        Dim dblBalance As Decimal
        Dim dblTempTotalAppliedReceiptAmount As Decimal
        Dim dblTotAppliedCredit As Decimal
        Dim iIndex As Integer
        Dim dtReceipts As DataTable
        Dim strAmount As String
        Dim dblSelectedReceiptAmount As Decimal
        Dim dblReceiptAmount As Double
        'SP 2014.05.15 -BT-2531 -
        Dim drArry As DataRow()
        Dim dr As DataRow
        'SP 2014.05.15 -BT-2531 -

        If Not CurrentReceipt Is Nothing Then
            dtReceipts = CurrentReceipt
        End If


        Try
            If Not dtReceipts Is Nothing Then


                If (SelectedReceiptIndex < 0) Then
                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_RECEIPT, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                End If

                If (SelectedReceiptIndex >= 0) Then
                    strAmount = gvReceipts.Rows(SelectedReceiptIndex).Cells(4).Text()
                    dblSelectedReceiptAmount = Convert.ToDecimal(strAmount)
                End If
                If dtReceipts.Rows.Count > 0 Then

                    dblReceiptAmount = Convert.ToDecimal(dtReceipts.Rows(SelectedReceiptIndex).Item("Amount"))

                    dtTransaction = DirectCast(CurrentTransactions, DataTable).Copy() 'SP 2014.05.12 -BT-2531
                    dtOriginalTransaction = DirectCast(Transaction, DataTable).Copy() 'SP 2014.05.12 -BT-2531


                    'For Un-Applied receipt amount in all transactions
                    If (dblReceiptAmount <> dblSelectedReceiptAmount) Then
                        imgBtnReceipts.ImageUrl = "images/Cash-UnApplied.jpg"
                        HiddenFieldDirty.Value = "false"
                        For i = 0 To dtTransaction.Rows.Count - 1

                            dblTempTotalAppliedReceiptAmount = Convert.ToDecimal(dtOriginalTransaction.Rows(i).Item("TotalAppliedReceiptAmount"))
                            dtTransaction.Rows(i).Item("TotalAppliedReceiptAmount") = dblTempTotalAppliedReceiptAmount

                            dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))

                            If Not Math.Round(Convert.ToDecimal(dtTransaction.Rows(i).Item("Balance")), 2) = 0 Then
                                dtTransaction.Rows(i).Item("FundedDate") = String.Empty
                                dtTransaction.Rows(i).Item("dtmPaidDate") = String.Empty

                            End If
                        Next
                        gvTrn.DataSource = dtTransaction
                        gvTrn.DataBind()

                        CurrentTransactions = dtTransaction.Copy() 'SP 2014.05.12 -BT-2531
                        dtReceipts.Rows(SelectedReceiptIndex).Item("Amount") = dblSelectedReceiptAmount
                        CurrentReceipt = dtReceipts
                        'Update total selected amount
                        UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531
                        Exit Sub
                    End If


                    dtTransaction.PrimaryKey = {dtTransaction.Columns("uniqueId")} 'SP 2014.05.15 -BT-2531 -
                    'For Applied receipt amount in selected transactions
                    For Each item As GridViewRow In gvTrn.Rows
                        dblReceiptAmount = Convert.ToDecimal(dtReceipts.Rows(SelectedReceiptIndex).Item("Amount"))
                        Dim checkbox As CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)
                        If (Not checkbox Is Nothing And checkbox.Checked) Then 'find checked checkbox
                            'For Each dr As DataRow In dtTransaction.Rows 'SP 2014.05.15 -BT-2531 - added datatable select
                            drArry = dtTransaction.Select("uniqueId=" + "'" + gvTrn.DataKeys(item.RowIndex).Value + "'")
                            If (drArry.Length > 0) Then

                                dr = drArry(0)
                                dblBalance = dr("Balance")
                                dr("Slctd") = True 'SP 2014.05.15 -BT-2531 -
                                If (dblBalance > 0 And dblReceiptAmount > 0) Then    'Only selected transaction
                                    imgBtnReceipts.ImageUrl = "images/ButtonCashApp.gif"
                                    HiddenFieldDirty.Value = "true"
                                    If (dblBalance <= dblReceiptAmount) Then
                                        dr("Slctd") = True
                                        dr("TotalAppliedReceiptAmount") = Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("Balance"))
                                        dr("Balance") = Convert.ToDecimal(dr("TransactionAmount")) - (Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("TotalAmountPaidnotUsed")))
                                        dtReceipts.Rows(SelectedReceiptIndex).Item("Amount") = dblReceiptAmount - dblBalance
                                        dblBalance = 0
                                    End If
                                    If (dblBalance >= dblReceiptAmount) Then
                                        dr("Slctd") = True
                                        dr("TotalAppliedReceiptAmount") = Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + dblReceiptAmount
                                        dr("Balance") = Convert.ToDecimal(dr("TransactionAmount")) - (Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("TotalAmountPaidnotUsed")))
                                        dtReceipts.Rows(SelectedReceiptIndex).Item("Amount") = 0
                                    End If

                                End If
                                If dblBalance = 0 Then
                                    If Math.Round(Convert.ToDecimal(dr.Item("Balance")), 2) = dblBalance Then
                                        dr.Item("FundedDate") = Me.dtUserFundeddate.Text
                                        dr.Item("dtmPaidDate") = Me.dtUserFundeddate.Text
                                    Else
                                        dr.Item("FundedDate") = String.Empty
                                        dr.Item("dtmPaidDate") = String.Empty
                                    End If
                                End If
                                'Next 'SP 2014.05.15 -BT-2531 -
                            End If


                        End If
                    Next

                    gvTrn.DataSource = dtTransaction
                    CurrentTransactions = dtTransaction.Copy() 'SP 2014.05.12 -BT-2531
                    gvTrn.DataBind()
                    CurrentReceipt = dtReceipts
                    'Update total selected amount
                    UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531
                End If
                'If (receiptAmount = selectedReceiptAmount) Then
                '	imgBtnReceipts.ImageUrl = "images/Cash-UnApplied.jpg"
                'End If
            End If


        Catch ex As Exception
            HelperFunctions.LogException("imgBtnReceipts_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub


    'Private Sub dgReceipts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgReceipts.SelectedIndexChanged
    '	Dim dtTransactions As DataTable
    '	Dim iIndex As Integer
    '	Dim bFlag As Boolean
    '	Dim dtReceipts As DataTable
    '	Try
    '		dtReceipts = CurrentReceipt

    '		Dim dblAppliedRecipts As Double = 0

    '		''Revert all the applied receipt amount in current transaction
    '		If Not CurrentTransactions Is Nothing Then
    '			dtTransactions = CurrentTransactions

    '			For iIndex = 0 To dtTransactions.Rows.Count - 1
    '				imgBtnReceipts.ImageUrl = "images/Cash-UnApplied.jpg"
    '				dblAppliedRecipts = dtTransactions.Rows(iIndex).Item("TotalAppliedReceiptAmount")
    '				dtTransactions.Rows(iIndex).Item("TotalAppliedReceiptAmount") = 0
    '				dtTransactions.Rows(iIndex).Item("Balance") = Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("TransactionAmount")) - Convert.ToDecimal(dblAppliedRecipts) - Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("TotalAmountPaidnotUsed")) - Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("TotalAppliedCreditAmount"))
    '				If Not Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("Balance")) = 0 Then
    '					dtTransactions.Rows(iIndex).Item("FundedDate") = String.Empty
    '					dtTransactions.Rows(iIndex).Item("dtmPaidDate") = String.Empty
    '				End If
    '			Next
    '			gvTrn.DataSource = dtTransactions
    '			gvTrn.DataBind()

    '			CurrentTransactions = dtTransactions
    '			'dtTransactions = Nothing
    '		End If
    '	Catch ex As Exception
    '		HelperFunctions.LogException("dgReceipts_SelectedIndexChanged-CashApplication-person", ex)
    '		HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
    '	End Try
    'End Sub

    Private Sub imgbtnCreditAvailable_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnCreditAvailable.Click
        Dim dtTransaction As DataTable
        Dim dtOriginalTransaction As DataTable
        Dim dblAmtCredit As Decimal
        Dim dblBalance As Decimal
        Dim dTempTotalAppliedCreditAmount As Decimal
        Dim dblOriginalCreditAmount As Decimal
        'SP 2014.05.15 -BT-2531 -
        Dim drArry As DataRow()
        Dim dr As DataRow
        Dim checkbox As CheckBox
        'SP 2014.05.15 -BT-2531 -
        Try
            dblAmtCredit = txtCreditAvailable.Text
            dblOriginalCreditAmount = OriginalCredits
            dtTransaction = DirectCast(CurrentTransactions, DataTable).Copy() 'SP 2014.05.12 -BT-2531
            dtOriginalTransaction = DirectCast(Transaction, DataTable).Copy() 'SP 2014.05.12 -BT-2531



            'For Un-Applied credit amount in all transactions
            If (dblAmtCredit <> dblOriginalCreditAmount) Then
                imgbtnCreditAvailable.ImageUrl = "images/Cash-UnApplied.jpg"
                HiddenFieldDirty.Value = "false"
                For i = 0 To dtTransaction.Rows.Count - 1

                    dTempTotalAppliedCreditAmount = dtOriginalTransaction.Rows(i).Item("TotalAppliedCreditAmount")
                    dtTransaction.Rows(i).Item("TotalAppliedCreditAmount") = dTempTotalAppliedCreditAmount

                    dtTransaction.Rows(i).Item("Balance") = Convert.ToDecimal(dtTransaction.Rows(i)("TransactionAmount")) - (Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAppliedCreditAmount")) + Convert.ToDecimal(dtTransaction.Rows(i)("TotalAmountPaidnotUsed")))

                    If Not Math.Round(Convert.ToDecimal(dtTransaction.Rows(i).Item("Balance")), 2) = 0 Then
                        dtTransaction.Rows(i).Item("FundedDate") = String.Empty
                        dtTransaction.Rows(i).Item("dtmPaidDate") = String.Empty

                    End If
                Next
                gvTrn.DataSource = dtTransaction
                gvTrn.DataBind()

                txtCreditAvailable.Text = String.Format("{0:0.00}", dblOriginalCreditAmount)
                CurrentTransactions = dtTransaction.Copy() 'SP 2014.05.12 -BT-2531
                'Update total selected amount
                UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531
                Exit Sub
            End If


            dtTransaction.PrimaryKey = {dtTransaction.Columns("uniqueId")}
            'For Applied credit amount in selected transactions
            For Each item As GridViewRow In gvTrn.Rows
                dblAmtCredit = txtCreditAvailable.Text
                checkbox = DirectCast(item.FindControl("chkSel"), CheckBox)
                If (Not checkbox Is Nothing And checkbox.Checked) Then 'find checked checkbox
                    'For Each dr As DataRow In dtTransaction.Rows  'SP 2014.05.15 -BT-2531 -
                    'SP 2014.05.15 -BT-2531 - add datatable select
                    drArry = dtTransaction.Select("uniqueId=" + "'" + gvTrn.DataKeys(item.RowIndex).Value + "'")
                    If (drArry.Length > 0) Then
                        dr = drArry(0)
                        ' 'SP 2014.05.15 -BT-2531 -
                        dblBalance = dr("Balance")
                        dr("Slctd") = True 'SP 2014.05.15 -BT-2531 -
                        If (dblBalance > 0 And dblAmtCredit > 0) Then    'Only selected transaction
                            imgbtnCreditAvailable.ImageUrl = "images/ButtonCashApp.gif"
                            'HiddenFieldDirty.Value = "true"

                            If (dblBalance <= dblAmtCredit) Then
                                dr("Slctd") = True
                                dr("TotalAppliedCreditAmount") = Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("Balance"))
                                dr("Balance") = Convert.ToDecimal(dr("TransactionAmount")) - (Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("TotalAmountPaidnotUsed")))
                                txtCreditAvailable.Text = String.Format("{0:0.00}", dblAmtCredit - dr("TotalAppliedCreditAmount"))
                                dblBalance = 0
                            End If
                            If (dblBalance >= dblAmtCredit) Then
                                dr("Slctd") = True
                                dr("TotalAppliedCreditAmount") = Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + dblAmtCredit
                                dr("Balance") = Convert.ToDecimal(dr("TransactionAmount")) - (Convert.ToDecimal(dr("TotalAppliedReceiptAmount")) + Convert.ToDecimal(dr("TotalAppliedCreditAmount")) + Convert.ToDecimal(dr("TotalAmountPaidnotUsed")))
                                txtCreditAvailable.Text = "0.00"
                            End If
                            If dblBalance = 0 Then
                                If Math.Round(Convert.ToDecimal(dr("Balance")), 2) = dblBalance Then
                                    dr("FundedDate") = Me.dtUserFundeddate.Text
                                    dr("dtmPaidDate") = Me.dtUserFundeddate.Text
                                Else
                                    dr("FundedDate") = String.Empty
                                    dr("dtmPaidDate") = String.Empty
                                End If
                            End If
                        End If

                    End If
                    ' Next  'SP 2014.05.15 -BT-2531 -

                End If
            Next

            gvTrn.DataSource = dtTransaction
            gvTrn.DataBind()
            'Update total selected amount
            UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531
            'If (amtCredit = originalCreditAmount) Then
            '	imgbtnCreditAvailable.ImageUrl = "images/Cash-UnApplied.jpg"
            'Else

            'End If
            CurrentTransactions = dtTransaction.Copy() 'SP 2014.05.12 -BT-2531

        Catch ex As Exception
            HelperFunctions.LogException("imgbtnCreditAvailable_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub lnkSearchYMCA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearchYMCA.Click
        Try
            btnTransactionProcess.Enabled = True
            SetSelectedYMCASearchTab()
            lblHdr.Text = "Search YMCA"
        Catch ex As Exception
            HelperFunctions.LogException("lnkSearchYMCA_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub lnkSearchPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearchPerson.Click
        Try
            btnTransactionProcess.Enabled = True
            If String.IsNullOrEmpty(SelectedYMCAID) Then
                SetSelectedYMCASearchTab()
                HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_YMCA, EnumMessageTypes.Error, Nothing)
                lblHdr.Text = "Search YMCA"
            Else
                SetSelectedPersonSearchTab()
                lblHdr.Text = "Select Person & Transmittal"
            End If
        Catch ex As Exception
            HelperFunctions.LogException("lnkSearchPerson_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub

    Private Sub lnkSelectTransaction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSelectTransaction.Click
        Try
            If Not String.IsNullOrEmpty(SelectedYMCAID) AndAlso Not String.IsNullOrEmpty(SelectedGuiFundEventID) AndAlso Not String.IsNullOrEmpty(SelectedGuiTransmittalID) Then
                SetSelectedTransactionTab()
                lblHdr.Text = "Select Transaction(s) & Apply Funds"
            ElseIf (String.IsNullOrEmpty(SelectedGuiFundEventID) Or String.IsNullOrEmpty(SelectedGuiTransmittalID)) And Not String.IsNullOrEmpty(SelectedYMCAID) Then
                SetSelectedPersonSearchTab()
                If (String.IsNullOrEmpty(SelectedGuiFundEventID)) Then
                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_PERSON, EnumMessageTypes.Error, Nothing)
                Else
                    HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_TRANSMITTAL, EnumMessageTypes.Error, Nothing)
                End If

                lblHdr.Text = "Select Person & Transmittal"
            Else
                SetSelectedYMCASearchTab()
                HelperFunctions.ShowMessageToUser(Resources.CashApplication.SELECT_YMCA, EnumMessageTypes.Error, Nothing)
                lblHdr.Text = "Search YMCA"
            End If
        Catch ex As Exception
            HelperFunctions.LogException("lnkSelectTransaction_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub
    Private Sub btnPersonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonClear.Click
        Try
            txtSearchFirstName.Text = String.Empty
            txtSearchFundNo.Text = String.Empty
            txtSearchLastName.Text = String.Empty
            txtSearchSSN.Text = String.Empty
            gvPerson.DataSource = Nothing
            gvPerson.DataBind()
            SelectedGuiFundEventID = Nothing
            SelectedGuiTransmittalID = Nothing
            gvTransmittal.DataSource = Nothing
            gvTransmittal.DataBind()
            SortExpressionTransmittal = String.Empty
            SortExpressionPerson = String.Empty
            PersonSearchList = Nothing
            TransmittalSearchList = Nothing
            spnSelectedPerson.Text = String.Empty
            spnSelectTransmittal.Visible = False
        Catch ex As Exception
            HelperFunctions.LogException("btnPersonClear_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub
    'SP 2014.05.12 BT-2531  -Start
    Private Sub btnUpdateAmount_Click(sender As Object, e As EventArgs) Handles btnUpdateAmount.Click
        Try
            UpdateTransaction()
            ResetImageOnUpdateTotal()
        Catch ex As Exception
            HelperFunctions.LogException("btnUpdateAmount_Click-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'SP 2014.05.12 BT-2531  -End


    Private Sub gvYmca_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvYmca.Sorting
        Try
            If (HelperFunctions.isNonEmpty(YMCAList)) Then
                SortYMCAgrid(e)
                BindYMCAGrid()
            End If

        Catch ex As Exception
            HelperFunctions.LogException("gvYmca_Sorting-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

#End Region

    Private Sub gvReceipts_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReceipts.RowCommand
        Dim dtTransactions As DataTable
        Dim iIndex As Integer
        Dim bFlag As Boolean
        Dim dtReceipts As DataTable
        Try
            dtReceipts = CurrentReceipt

            Dim dblAppliedRecipts As Decimal = 0
            If (e.CommandName.ToLower = "select") Then

                SelectedReceiptIndex = Convert.ToString(e.CommandArgument)
                If (SelectedReceiptIndex > -1) Then
                    ''Revert all the applied receipt amount in current transaction
                    If Not CurrentTransactions Is Nothing Then
                        dtTransactions = CurrentTransactions.Copy() 'SP 2014.05.12 -BT-2531

                        For iIndex = 0 To dtTransactions.Rows.Count - 1
                            imgBtnReceipts.ImageUrl = "images/Cash-UnApplied.jpg"
                            dblAppliedRecipts = dtTransactions.Rows(iIndex).Item("TotalAppliedReceiptAmount")
                            dtTransactions.Rows(iIndex).Item("TotalAppliedReceiptAmount") = 0
                            dtTransactions.Rows(iIndex).Item("Balance") = Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("TransactionAmount")) - Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("TotalAmountPaidnotUsed")) - Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("TotalAppliedCreditAmount"))
                            If Not Convert.ToDecimal(dtTransactions.Rows(iIndex).Item("Balance")) = 0 Then
                                dtTransactions.Rows(iIndex).Item("FundedDate") = String.Empty
                                dtTransactions.Rows(iIndex).Item("dtmPaidDate") = String.Empty
                            End If
                        Next
                        gvTrn.DataSource = dtTransactions
                        gvTrn.DataBind()

                        CurrentTransactions = dtTransactions.Copy() 'SP 2014.05.12 -BT-2531
                        'Update total selected amount
                        UpdateSelectedTotalAmount() 'SP 2014.05.12 -BT-2531
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvReceipts_RowCommand-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvTransmittal_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTransmittal.Sorting
        Try
            If (HelperFunctions.isNonEmpty(TransmittalSearchList)) Then
                SortTransmittalgrid(e)
                Me.PopulateTransmittalByFundEventId(SelectedYMCAID, SelectedGuiFundEventID)
            End If

        Catch ex As Exception
            HelperFunctions.LogException("gvTransmittal_Sorting-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvPerson_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPerson.Sorting
        Try
            If (HelperFunctions.isNonEmpty(PersonSearchList)) Then
                SortPersongrid(e)
                Me.SearchParticipantbyYmcaId(SelectedYMCAID, txtSearchSSN.Text.Trim(), txtSearchFundNo.Text.Trim(), txtSearchFirstName.Text.Trim(), txtSearchLastName.Text.Trim())
            End If

        Catch ex As Exception
            HelperFunctions.LogException("gvPerson_Sorting-CashApplication-person", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


End Class