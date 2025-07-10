'***************
' Created a copy of this page from the ReportViewer.aspx page. With last change tracked as the following entry.
' Any future changes made to the ReportViewer.aspx file need to be replicated into this file as well.
' Priya                 19-Feb-2009           None                    Added trim to name as it displays spaces between name in report.
'***************
'**********************************************************************************************************************
'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(MM/DD/YYYY)     Issue ID        Description  
'**********************************************************************************************************************
'**********************************************************************************************************************

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class CashOutReportPrinter
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "


	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object
	Dim ParameterFieldsCollection As New CrystalDecisions.Shared.ParameterFields
	'Dim objRpt As New ReportDocument
	Protected WithEvents btnLast As System.Web.UI.WebControls.Button
	Protected WithEvents btnPrevious As System.Web.UI.WebControls.Button
	Protected WithEvents btnNext As System.Web.UI.WebControls.Button
	Protected WithEvents btnFirst As System.Web.UI.WebControls.Button
	Protected WithEvents btnExport As System.Web.UI.WebControls.Button
	Protected WithEvents CrystalReportViewer1 As CrystalDecisions.Web.CrystalReportViewer
	Protected WithEvents CrystalReportViewer2 As CrystalDecisions.Web.CrystalReportViewer
	Protected WithEvents CrystalReportViewer3 As CrystalDecisions.Web.CrystalReportViewer
	Dim objRptFormLessThan1K As New CrystalDecisions.CrystalReports.Engine.ReportDocument
	Dim objRptForm1Kto5K As New CrystalDecisions.CrystalReports.Engine.ReportDocument
	Dim objRptLetters As New CrystalDecisions.CrystalReports.Engine.ReportDocument
	Protected WithEvents hiddError As System.Web.UI.HtmlControls.HtmlInputHidden
	Dim strReportName As String
	Protected WithEvents ddlPrinterFormName As System.Web.UI.WebControls.DropDownList
	Protected WithEvents ddlPrinterLetterName As System.Web.UI.WebControls.DropDownList
	Protected WithEvents btnPrintsetting As System.Web.UI.WebControls.Button
	Protected WithEvents hiddReport As System.Web.UI.HtmlControls.HtmlInputHidden

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
		strReportName = CType(Session("strReportName"), String)
	End Sub

#End Region
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here

		Dim sReportName As String
		'by aparna
		Dim bBoolReport As Boolean

		Try
			
			If Not (IsPostBack) Then
				GetPrinterName()
			End If

			'================================
			''Added By Aparna on 13th April

			If hiddError.Value <> String.Empty Then

				hiddReport.Value = "True"
				sReportName = strReportName

				Select Case sReportName

						'Priya 31.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
					Case "Cash Out"
						populateReportsValues()
						'END Priya 31.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
					

				End Select

			End If
		Catch
			Throw
		End Try
	End Sub
	Private Function populateReportsValues()
		Dim ArrListParamValues As New ArrayList
		Dim sReportName As String
		Dim boollogontoDB As Boolean
		'Added By Aparna on 17th April
		Dim l_dataSet As DataSet
		Dim l_datatable As DataTable
		Dim l_datarow As DataRow
		Dim l_String_tmp As String
		Dim l_string_Persid As String
		Dim ListBoxSelectedItems As New System.Web.UI.WebControls.ListBox
		Dim i As Integer

		If Not Session("ListBoxSelectedItems") Is Nothing Then
			ListBoxSelectedItems = Session("ListBoxSelectedItems")
		End If

		'end code Added By Aparna on 17th April

		Try

			sReportName = strReportName

			Select Case sReportName


		
				Case "Cash Out"
					'Priya 23.10.2012: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000  Commented code
					boollogontoDB = True

					If Not Session("PrintBatchID") Is Nothing OrElse Session("PrintBatchID") <> "" Then
						ArrListParamValues.Add(CType(Session("PrintBatchID"), String).ToString.Trim)
					End If


				Case Else

			End Select

			If sReportName.Trim <> String.Empty Then
				LoadReport(ArrListParamValues, sReportName, boollogontoDB)
			End If

		Catch
			Throw

		End Try

	End Function
	Private Function LoadReport(ByVal ArrListParamValues As ArrayList, ByVal sReportName As String, ByVal logontodb As Boolean) As Boolean
		'Dim objRpt As New cryst  
		Dim crCon As New ConnectionInfo
		Dim CrTableLogonInfo As New TableLogOnInfo
		Dim CrTables As Tables
		Dim CrTable As Table
		Dim TableCounter As Integer
		Dim DataSource As String
		Dim DatabaseName As String
		Dim UserID As String
		Dim Password As String
		Dim strXml As String
		Dim sReportPath As String
		Dim paramItem As String
		Dim dsShellPrinter As DataSet
		Dim drForm As DataRow()
		Dim drLetter As DataRow()
		Dim l_dr As DataRow
		Try
			DataSource = ConfigurationSettings.AppSettings("DataSource")
			DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
			UserID = ConfigurationSettings.AppSettings("UserID")
			Password = ConfigurationSettings.AppSettings("Password")
			sReportPath = ConfigurationSettings.AppSettings("ReportPath")
			Dim l_datatable_Cashouts As DataTable
			Dim l_dataset As DataSet
			Dim strLessThan1KReport, str1Kto5KReport, strLetters As String
			Dim strLessThan1KReportPath, str1Kto5KReportPath, strLettersPath As String
			Dim l_double_totalamtforreleaseblnk As Double
			Dim strReportLessthan1, strReport1to5K, strReportLetter As String

			l_datatable_Cashouts = Session("printBatchDdata")
			If Not IsNothing(l_datatable_Cashouts) Then
				If l_datatable_Cashouts.Rows.Count > 0 Then

					Dim l_dataset_CashOutFormsLetters As DataSet
					Dim objcashout As New Infotech.YmcaBusinessObject.CashOutBOClass()
					'l_dataset_CashOutFormsLetters = objcashout.getCashOutFormsLetters()

					l_dataset_CashOutFormsLetters = objcashout.getCashOutFormsLetters("CASHOUT_REPORT_FORM_LESSTHAN1K")

					If HelperFunctions.isNonEmpty(l_dataset_CashOutFormsLetters) Then
						strReportLessthan1 = l_dataset_CashOutFormsLetters.Tables(0).Rows(0)("Value")
						l_dataset_CashOutFormsLetters = Nothing
					End If
					l_dataset_CashOutFormsLetters = objcashout.getCashOutFormsLetters("CASHOUT_REPORT_FORM_1KTO5K")
					If HelperFunctions.isNonEmpty(l_dataset_CashOutFormsLetters) Then
						strReport1to5K = l_dataset_CashOutFormsLetters.Tables(0).Rows(0)("Value")
						l_dataset_CashOutFormsLetters = Nothing
					End If
					l_dataset_CashOutFormsLetters = objcashout.getCashOutFormsLetters("CASHOUT_REPORT_LETTER")
					If HelperFunctions.isNonEmpty(l_dataset_CashOutFormsLetters) Then
						strReportLetter = l_dataset_CashOutFormsLetters.Tables(0).Rows(0)("Value")
						l_dataset_CashOutFormsLetters = Nothing
					End If


					For Each row In l_datatable_Cashouts.Rows
						Try
							'Priya Patil 31.10.2012 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Added if requestid is not null
							If Not IsNothing(row("guiRefRequestID")) AndAlso row("guiRefRequestID").ToString <> "" Then
								l_dataset = Infotech.YmcaBusinessObject.RefundRequest.GetWithdrawalReportData(row("guiRefRequestID").ToString().Trim())


								If Not l_dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
									l_double_totalamtforreleaseblnk = l_double_totalamtforreleaseblnk + Convert.ToDouble(l_dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString())
								End If

								If l_double_totalamtforreleaseblnk < 1000 Then
									strLessThan1KReport = strReportLessthan1 'sReportName '"ReleaseBlankLess1K.rpt"
								ElseIf l_double_totalamtforreleaseblnk <= 5000 Then
									str1Kto5KReport = strReport1to5K ' sReportName '"ReleaseBlank1kto5k.rpt"
								End If
								'Chck str1Kto5KReport strLessThan1KReport variable n then exit for loop
								If Not strLessThan1KReport Is Nothing AndAlso str1Kto5KReport <> "" Then
									Exit For
								End If

							End If
						Catch
							Throw
						Finally
							If Not l_dataset Is Nothing Then
								l_dataset.Dispose()
							End If

							l_double_totalamtforreleaseblnk = Nothing
						End Try

					Next
				End If
			End If

			strLetters = strReportLetter 'sReportName '"safeharbor_mill.rpt"

			If strLessThan1KReport <> String.Empty Then
				strLessThan1KReportPath = sReportPath.Trim + "\\" + strLessThan1KReport + ".rpt"

				objRptFormLessThan1K.Load(strLessThan1KReportPath)
				objRptFormLessThan1K.Refresh()
				CrystalReportViewer1.ReportSource = objRptFormLessThan1K
			End If
			If str1Kto5KReport <> String.Empty Then
				str1Kto5KReportPath = sReportPath.Trim + "\\" + str1Kto5KReport + ".rpt"

				objRptForm1Kto5K.Load(str1Kto5KReportPath)
				objRptForm1Kto5K.Refresh()
				CrystalReportViewer2.ReportSource = objRptForm1Kto5K
			End If




			If strLetters <> String.Empty Then
				strLetters = sReportPath.Trim + "\\" + strLetters + ".rpt"

				objRptLetters.Load(strLetters)
				objRptLetters.Refresh()

				CrystalReportViewer3.ReportSource = objRptLetters
			End If



			
			If ArrListParamValues.Count > 0 Then

				If strLessThan1KReport <> String.Empty Then
					Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
					Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
					Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
					Dim discreteValue As New CrystalDecisions.Shared.ParameterDiscreteValue


					discreteValue.Value = Session("PrintBatchID")
					curValues.Add(discreteValue)
					objRptFormLessThan1K.ParameterFields(0).CurrentValues = curValues
					CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
				End If
				If str1Kto5KReport <> String.Empty Then
					Dim paramFieldsCollection1to5 As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer2.ParameterFieldInfo
					Dim paramField1to5 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection1to5(0)
					Dim curValues1to5 As CrystalDecisions.Shared.ParameterValues = paramField1to5.CurrentValues
					Dim discreteValue1to5 As New CrystalDecisions.Shared.ParameterDiscreteValue

					discreteValue1to5.Value = Session("PrintBatchID")
					curValues1to5.Add(discreteValue1to5)
					objRptForm1Kto5K.ParameterFields(0).CurrentValues = curValues1to5
					CrystalReportViewer2.ParameterFieldInfo = paramFieldsCollection1to5
				End If
				If strLetters <> String.Empty Then
					Dim paramFieldsCollectionLetters As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer3.ParameterFieldInfo
					Dim paramFieldLetters As CrystalDecisions.Shared.ParameterField = paramFieldsCollectionLetters(0)
					Dim curValuesLetters As CrystalDecisions.Shared.ParameterValues = paramFieldLetters.CurrentValues
					Dim discreteValueLetters As New CrystalDecisions.Shared.ParameterDiscreteValue

					discreteValueLetters.Value = Session("PrintBatchID")
					curValuesLetters.Add(discreteValueLetters)
					objRptLetters.ParameterFields(0).CurrentValues = curValuesLetters
					CrystalReportViewer3.ParameterFieldInfo = paramFieldsCollectionLetters
				End If

			End If
			

			If logontodb Then
				'CrTables = objRpt.Database.Tables
				If strLessThan1KReport <> String.Empty Then
					CrTables = objRptFormLessThan1K.Database.Tables

					crCon.ServerName = DataSource
					crCon.DatabaseName = DatabaseName
					crCon.UserID = UserID
					crCon.Password = Password

					For Each CrTable In CrTables
						CrTableLogonInfo = CrTable.LogOnInfo
						CrTableLogonInfo.ConnectionInfo = crCon
						CrTable.ApplyLogOnInfo(CrTableLogonInfo)
						CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
					Next
				End If

				If str1Kto5KReport <> String.Empty Then
					CrTables = objRptForm1Kto5K.Database.Tables

					crCon.ServerName = DataSource
					crCon.DatabaseName = DatabaseName
					crCon.UserID = UserID
					crCon.Password = Password

					For Each CrTable In CrTables
						CrTableLogonInfo = CrTable.LogOnInfo
						CrTableLogonInfo.ConnectionInfo = crCon
						CrTable.ApplyLogOnInfo(CrTableLogonInfo)
						CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
					Next
				End If

				If strLetters <> String.Empty Then
					CrTables = objRptLetters.Database.Tables

					crCon.ServerName = DataSource
					crCon.DatabaseName = DatabaseName
					crCon.UserID = UserID
					crCon.Password = Password

					For Each CrTable In CrTables
						CrTableLogonInfo = CrTable.LogOnInfo
						CrTableLogonInfo.ConnectionInfo = crCon
						CrTable.ApplyLogOnInfo(CrTableLogonInfo)
						CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
					Next
				End If
			End If



			'this coding for form printer
			If Not ViewState("Dataset_FormPrinter") Is Nothing Then
				'If Not ViewState("Dataset_Printer")  Is Nothing Then
				dsShellPrinter = ViewState("Dataset_FormPrinter")
				drForm = dsShellPrinter.Tables("Printers").Select("[PrinterID]= " & ddlPrinterFormName.SelectedValue & "")
				If Not IsNothing(drForm) Then



					'Infotech.YmcaBusinessObject.MetaShellPrintersBOClass.InsertUserPrinter(Session("LoggedUserKey"), dr(0)("ReportID"), Convert.ToInt64(ddlPrinterName.SelectedValue))
					Infotech.YmcaBusinessObject.MetaShellPrintersBOClass.InsertUserPrinter(Session("LoggedUserKey"), drForm(0)("ReportID"), Convert.ToInt64(ddlPrinterFormName.SelectedValue))

					If PrintReportFromServer(drForm(0)("PrinterConfigartion"), "Form") = False Then
						If hiddReport.Value = String.Empty Then
							Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"
							If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
								Page.RegisterStartupScript("DisplayError", popupScript)
							End If
						End If
						CrystalReportViewer1.DataBind()
						CrystalReportViewer2.DataBind()
					Else
						'print successfuly
						Dim popupScript As String = "<script language='javascript'>DisplaySuccess();</script>"
						If (Not Me.IsStartupScriptRegistered("PrintClose")) Then
							Page.RegisterStartupScript("PrintClose", popupScript)
						End If
						CrystalReportViewer1.ReportSource = Nothing
						CrystalReportViewer2.ReportSource = Nothing
					End If
				End If
			Else
				Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"
				If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
					Page.RegisterStartupScript("DisplayError", popupScript)
				End If
				CrystalReportViewer1.DataBind()
				CrystalReportViewer2.DataBind()
			End If

			'coding is for letter printering
			If Not ViewState("Dataset_LetterPrinter") Is Nothing Then
				'If Not ViewState("Dataset_Printer") Is Nothing Then
				dsShellPrinter = ViewState("Dataset_LetterPrinter")
				drLetter = dsShellPrinter.Tables("Printers").Select("[PrinterID]= " & ddlPrinterLetterName.SelectedValue & "")
				If Not IsNothing(drLetter) Then
					Infotech.YmcaBusinessObject.MetaShellPrintersBOClass.InsertUserPrinter(Session("LoggedUserKey"), drLetter(0)("ReportID"), Convert.ToInt64(ddlPrinterLetterName.SelectedValue))
					If PrintReportFromServer(drLetter(0)("PrinterConfigartion"), "Letter") = False Then
						If hiddReport.Value = String.Empty Then
							Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"
							If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
								Page.RegisterStartupScript("DisplayError", popupScript)
							End If
						End If
						CrystalReportViewer3.DataBind()
					Else
						'print successfuly
						Dim popupScript As String = "<script language='javascript'>DisplaySuccess();</script>"
						If (Not Me.IsStartupScriptRegistered("PrintClose")) Then
							Page.RegisterStartupScript("PrintClose", popupScript)
						End If
						CrystalReportViewer3.ReportSource = Nothing
					End If
				End If
			Else
				Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"
				If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
					Page.RegisterStartupScript("DisplayError", popupScript)
				End If
				CrystalReportViewer3.DataBind()
			End If


		Catch
			'HelperFunctions.LogException("LoadReport", ex)
			Throw
		End Try
	End Function
	Private Function PrintReportFromServer(ByVal parameters As String, ByVal Type As String) As Boolean

		Dim params As String() = parameters.Split(";")

		Dim printerName, paperType, paperOrientation As String

		Dim param As String()
		Dim testMode As Boolean = False


		Dim i As Integer
		Try

			For i = 0 To params.Length - 1

				If params(i).Trim <> String.Empty Then

					param = params(i).Trim.Split("=")

					If param.Length <> 2 Then

						'Incomplete parameters specified. Log message in error log
						hiddError.Value &= "Incomplete parameters specified."
						Return False

					End If

					'Only assign values if valid key value pair is found

					Select Case param(0).ToLower()

						Case "printername"

							printerName = param(1)

						Case "papertype"

							paperType = param(1)

						Case "paperorientation"

							paperOrientation = param(1)
						Case "testmode"

							testMode = Boolean.Parse(param(1))

					End Select
				End If

			Next

			If printerName <> String.Empty Then

				If Type = "Form" Then
					If Not IsNothing(objRptFormLessThan1K) Then
						objRptFormLessThan1K.PrintOptions.PrinterName = printerName	'//"Microsoft XPS Document Writer";
						'objRptFormLessThan1K.PrintOptions.PrinterName = printerName	'//"Microsoft XPS Document Writer";
					End If
					If Not IsNothing(objRptForm1Kto5K) Then
						objRptForm1Kto5K.PrintOptions.PrinterName = printerName	'//"Microsoft XPS Document Writer";
					End If
				End If

				If Type = "Letter" Then
					If Not IsNothing(objRptLetters) Then
						objRptLetters.PrintOptions.PrinterName = printerName	'//"Microsoft XPS Document Writer";
					End If
				End If
				'objRpt.PrintOptions.PrinterName = printerName '//"Microsoft XPS Document Writer";
			Else
				'Printer name not specified. Log a message in the error log
				hiddError.Value &= "Printer name not specified."
				Return False
			End If

			If printerName <> String.Empty AndAlso paperType <> String.Empty Then

				Dim paperSource As System.Drawing.Printing.PaperSource = GetSelectedPaperSource(printerName, paperType)

				If (Not paperSource Is Nothing) Then
					If Type = "Form" Then
						If Not IsNothing(objRptFormLessThan1K) Then
							objRptFormLessThan1K.PrintOptions.CustomPaperSource = paperSource
						End If
						If Not IsNothing(objRptForm1Kto5K) Then
							objRptForm1Kto5K.PrintOptions.CustomPaperSource = paperSource
						End If
					End If
					If Type = "Letter" Then
						If Not IsNothing(objRptLetters) Then
							objRptLetters.PrintOptions.CustomPaperSource = paperSource
						End If
					End If
					'objRpt.PrintOptions.CustomPaperSource = paperSource

				Else

					'Paper type not found. Log a message in the error log
					hiddError.Value &= "Paper Type not found on Printer."
					Return False

				End If

			End If

			If paperOrientation <> String.Empty Then

				Select Case paperOrientation.ToLower()

					Case "portrait"
						If Type = "Form" Then
							If Not IsNothing(objRptFormLessThan1K) Then
								objRptFormLessThan1K.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Portrait
							End If
							If Not IsNothing(objRptForm1Kto5K) Then
								objRptForm1Kto5K.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Portrait
							End If
						End If
						If Type = "Letter" Then
							If Not IsNothing(objRptLetters) Then
								objRptLetters.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Portrait

							End If
						End If
						'objRpt.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Portrait

					Case "landscape"
						If Type = "Form" Then
							If Not IsNothing(objRptFormLessThan1K) Then
								objRptFormLessThan1K.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Landscape
							End If
							If Not IsNothing(objRptForm1Kto5K) Then
								objRptForm1Kto5K.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Landscape
							End If
						End If
						If Type = "Letter" Then
							If Not IsNothing(objRptLetters) Then
								objRptLetters.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Landscape

							End If
						End If
						'objRpt.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Landscape

					Case Else

						'Invalid orientation specified. Log a message in the error log
						hiddError.Value &= "Paper orientation is invalid."

						Return False

				End Select

			End If

			If testMode = False Then
				If Type = "Form" Then
					If Not IsNothing(objRptFormLessThan1K) Then
						objRptFormLessThan1K.PrintToPrinter(1, False, 0, 0)
					End If
					If Not IsNothing(objRptForm1Kto5K) Then
						objRptForm1Kto5K.PrintToPrinter(1, False, 0, 0)
					End If
				End If
				If Type = "Letter" Then
					If Not IsNothing(objRptLetters) Then
						objRptLetters.PrintToPrinter(1, False, 0, 0)
					End If
				End If
				'objRpt.PrintToPrinter(1, False, 0, 0)
			End If
			Return True
		Catch
			'HelperFunctions.LogException("", ex)
			If hiddError.Value = "" Then
				hiddError.Value &= "There was a problem printing from the server. Please contact IT support."
			End If
			Return False

		End Try

	End Function
	Private Function GetSelectedPaperSource(ByVal printerName As String, ByVal paperSourceName As String) As System.Drawing.Printing.PaperSource

		Dim selectedPaperSource As System.Drawing.Printing.PaperSource = Nothing '//new System.Drawing.Printing.PaperSource();

		Dim printerSettings As System.Drawing.Printing.PrinterSettings = New System.Drawing.Printing.PrinterSettings

		printerSettings.PrinterName = printerName

		Dim paperSource As System.Drawing.Printing.PaperSource

		For Each paperSource In printerSettings.PaperSources

			If (String.Compare(paperSource.SourceName, paperSourceName, True) = 0) Then
				selectedPaperSource = paperSource
				Exit For
			End If
		Next
		Return selectedPaperSource


	End Function

	Private Sub GetPrinterName()

		Dim dsShellFromPrinter As New DataSet
		Dim dsShellLetterPrinter As New DataSet
		Try
			dsShellFromPrinter = Infotech.YmcaBusinessObject.MetaShellPrintersBOClass.GetUserShellPrinters(Session("LoggedUserKey"), "Cash Out Form")
			dsShellLetterPrinter = Infotech.YmcaBusinessObject.MetaShellPrintersBOClass.GetUserShellPrinters(Session("LoggedUserKey"), "Cash Out Letter")

			If dsShellFromPrinter.Tables.Count > 0 Then

				If Not dsShellFromPrinter.Tables("Printers").Rows.Count = 0 Then
					For Each dr As DataRow In dsShellFromPrinter.Tables(0).Rows
						ddlPrinterFormName.Items.Add(New ListItem(dr("PrinterName").ToString(), dr("PrinterID").ToString()))

					Next
					'Bind dropdown list of prnters
					ddlPrinterFormName.Items.Add(New ListItem("--Select One--", 0))

					'Shows last time printer selected
					If dsShellFromPrinter.Tables("UserPrinter").Rows.Count > 0 Then
						If Not ddlPrinterFormName.Items.FindByValue(dsShellFromPrinter.Tables("UserPrinter").Rows(0)("UserPrinterID")) Is Nothing Then
							ddlPrinterFormName.SelectedValue = dsShellFromPrinter.Tables("UserPrinter").Rows(0)("UserPrinterID")
						Else
							ddlPrinterFormName.SelectedValue = 0
						End If
					Else
						ddlPrinterFormName.SelectedValue = 0
					End If

					ViewState("Dataset_FormPrinter") = dsShellFromPrinter
				Else
					hiddError.Value += "There are no printers configured with the paper type required for printing this Form. Please print this form manually using the print button of Crystal Viewer."
				End If

			End If

			'coding start for letters printing fill drop down list
			If dsShellLetterPrinter.Tables.Count > 0 Then
				If Not dsShellLetterPrinter.Tables("Printers").Rows.Count = 0 Then
					For Each dr As DataRow In dsShellLetterPrinter.Tables(0).Rows

						ddlPrinterLetterName.Items.Add(New ListItem(dr("PrinterName").ToString(), dr("PrinterID").ToString()))
					Next

					ddlPrinterLetterName.Items.Add(New ListItem("--Select One--", 0))
					'Shows last time printer selected
					If dsShellLetterPrinter.Tables("UserPrinter").Rows.Count > 0 Then
						
						If Not ddlPrinterLetterName.Items.FindByValue(dsShellLetterPrinter.Tables("UserPrinter").Rows(0)("UserPrinterID")) Is Nothing Then
							ddlPrinterLetterName.SelectedValue = dsShellLetterPrinter.Tables("UserPrinter").Rows(0)("UserPrinterID")
						Else
							ddlPrinterLetterName.SelectedValue = 0
						End If
					Else
						ddlPrinterLetterName.SelectedValue = 0
					End If



					ViewState("Dataset_LetterPrinter") = dsShellLetterPrinter
				Else
					If hiddError.Value = "" Then


						hiddError.Value += "There are no printers configured with the paper type required for printing this Form. Please print this form manually using the print button of Crystal Viewer."
					End If
				End If
			End If
		Catch
			Throw
		End Try
	End Sub
	
	Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
		Try
			CrystalReportViewer1.ShowLastPage()
			CrystalReportViewer2.ShowLastPage()
			CrystalReportViewer3.ShowLastPage()
		Catch
			Throw
		End Try
	End Sub
	Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
		''ExportToDisk("PHR")
	End Sub
	Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirst.Click
		Try
			CrystalReportViewer1.ShowFirstPage()
			CrystalReportViewer2.ShowFirstPage()
			CrystalReportViewer3.ShowFirstPage()
		Catch
			Throw
		End Try
	End Sub
	Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
		Try
			CrystalReportViewer1.ShowNextPage()
			CrystalReportViewer2.ShowNextPage()
			CrystalReportViewer3.ShowNextPage()
		Catch
			Throw
		End Try
	End Sub
	Private Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
		Try
			CrystalReportViewer1.ShowPreviousPage()
			CrystalReportViewer2.ShowPreviousPage()
			CrystalReportViewer3.ShowPreviousPage()
		Catch
			Throw
		End Try
	End Sub

	Private Sub btnPrintsetting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintsetting.Click

		Try
			CallPrintReport()

		Catch

			Throw
		End Try
	End Sub

	

	Private Sub CallPrintReport()
		Try
			
			Dim sReportName As String
			Dim bBoolReport As Boolean
			Dim LstBox As New System.Web.UI.WebControls.ListBox
			sReportName = strReportName
			Select Case sReportName
				Case "Cash Out"
					populateReportsValues()

			End Select
		Catch
			Throw
		End Try
	End Sub

	

	Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
		Try

		
		If Not IsNothing(objRptFormLessThan1K) Then

			objRptFormLessThan1K.Close()
			objRptFormLessThan1K.Dispose()
			objRptFormLessThan1K = Nothing
		End If
		If Not IsNothing(objRptForm1Kto5K) Then
			objRptForm1Kto5K.Close()
			objRptForm1Kto5K.Dispose()
			objRptForm1Kto5K = Nothing
		End If
		If Not IsNothing(objRptLetters) Then
			objRptLetters.Close()
			objRptLetters.Dispose()
			objRptLetters = Nothing


		End If
		'objRpt.Close()
		'objRpt.Dispose()
		'objRpt = Nothing
		CrystalReportViewer1.Dispose()
		CrystalReportViewer1 = Nothing
		CrystalReportViewer2.Dispose()
		CrystalReportViewer2 = Nothing
		CrystalReportViewer3.Dispose()
		CrystalReportViewer3 = Nothing
			GC.Collect()
		Catch
			Throw
		End Try
	End Sub
	
End Class
