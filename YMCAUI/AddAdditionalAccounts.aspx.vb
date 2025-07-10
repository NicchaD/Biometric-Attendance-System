'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	AddAdditionalAccounts.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	6/13/2005 12:35:48 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	Pop Up For Participants Information WebForm
'*********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By			Date				Description
'********************************************************************************************************************************
'Ruchi Saxena           10/13/2005          Coding
'Ragesh                 02/02/06            Cache to Session
'Preeti                 13Feb06             IssueId:YMCA-1752 Changes on HTML as well for PopcalendarDate control. 
'                                           Changned ScriptsValidators from Validate Date to No validate
'Rahul Nasa             Feb22,06            Ymca No. is not displayed properly.
'Vipul                  01March06           YRS Enhancement : Change in TD Hardship Suspension Period from 12 months to 6 months
'Preeti                 08Mar06             IssueId:YMCA-1991 ISValidresolution function added. Enhancement : Change in TD Hardship Suspension Period from 12 months to 6 months.
'                                           YMCA-1991 is using yrs_usp_ValidateDateForTDRenewal store proc also there has to be key naming HARDSHIP_RENEWAL_MONTHS with value as 
'                                           12 Months in  AtsMetaConfiguration
'Ashutosh Patil			30-Mar-2007			YREN-3205
'Ashutosh Patil         27-Apr-2007         For Avoiding runtime error recordcount greater than zero check applied to 
'                                           dataset while assiging Session("EmpEventId") = l_string_EmpEventId
'Mohammed Hafiz         25-Jul-2007         YREN-3617
'Swopna                 01-Aug-08           YRS-5.0-478
'Mohammed Hafiz         29-Aug-2008         YRS 5.0-531
'Priya Jawale           01-Oct-2008         YRS 5.0-478 Added new keyword
'Shashi Shekhar         26-Oct-2010         For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Imran                  14-Dec-2009         BT-1064 check duplicate accounts 
'Shashi Singh           02-Dec-2010         For YRS-5.0-1186, BT-656-AP account not acceptable as of 1/1/2010
'Shashi Shekhar         10-Dec-20100        For YRS 5.0-1239, BT-699 - "Account Exist" message need to be generic.
'Shashi shekhar         28-Dec-2010         For YRS 5.0-567 , BT-648: TD Contract termination date --Make it "not editable"
'Shashi                 04 Mar. 2011        Replacing Header formating with user control (YRS 5.0-450 )
'Shashi                 27 may 2011         For Term date conversion (string to date type)
'Shagufta               12 July 2011        BT-882 Additional Accounts issue.Elective Overlapping validation
'Shagufta               13 July 2011        Issue Re-open-BT-882 Additional Accounts issue-.Elective Overlapping validation(validation for Checking existing termination date is empty or null)
'Shagufta Chaudhari     2011.08.04          BT-893 , YRS 5.0-1360 : Allow Lump Sum additional accts record 
'Shashank Patel         2012.02.07          Made changes to rename contribution type monthly to Dollar
'prasad jadhav			2012.02.08			BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
'prasad jadhav			2012.03.02			(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
'prasad jadhav			2012.03.05			(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
'prasad jadhav			2012.03.06			BT-1008,Application allowing to update Voluntary account termination date other than most recent terminated record for non active participant.
'prasad jadhav			2012.03.15			(REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
'prasad jadhav			2012.03.21			bugID:1019 Unable to remove termination date of most recent terminated TD Acct. for Active participant.
'prasad jadhav			2012.04.02			YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
'prasad jadhav			2012.04.25			YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Public Class AddAdditionalAccounts
	Inherits System.Web.UI.Page

	Dim strFormName As String = New String("AddAdditionalAccounts.aspx")


#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents LabelYMCA As System.Web.UI.WebControls.Label
	Protected WithEvents LabelAccountType As System.Web.UI.WebControls.Label
	Protected WithEvents DropDownListAccountType As System.Web.UI.WebControls.DropDownList
	Protected WithEvents LabelContractType As System.Web.UI.WebControls.Label
	Protected WithEvents DropDownListContractType As System.Web.UI.WebControls.DropDownList
	Protected WithEvents LabelContribution As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxContribution As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelContributionAmount As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxContributionAmount As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxEffectiveDate As System.Web.UI.WebControls.TextBox
	Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
	Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxTermDate As System.Web.UI.WebControls.TextBox
	Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
	Protected WithEvents DropDownListYmca As System.Web.UI.WebControls.DropDownList
	Protected WithEvents PopcalendarDate As RJS.Web.WebControl.PopCalendar
	Protected WithEvents Popcalendar1 As RJS.Web.WebControl.PopCalendar
	Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
	Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
	Protected WithEvents valCustomTermDate As System.Web.UI.WebControls.CustomValidator
	Protected WithEvents ValCustomEffDate As System.Web.UI.WebControls.CustomValidator
	Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents ValidSumm As System.Web.UI.WebControls.ValidationSummary
	Protected WithEvents TextboxHireDate As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextboxEnrolldate As System.Web.UI.WebControls.TextBox
	Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
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

		'Added By preeti on 6 April 06 for Issue 1991 -Start
		TextBoxEffectiveDate.Attributes.Add("onblur", "javascript: GetFirstDayOfCurrentMonth('TextBoxEffectiveDate');")
		TextBoxTermDate.Attributes.Add("onblur", "javascript: GetFirstDayOfCurrentMonth('TextBoxTermDate');")
		'Added By preeti on 6 April 06 for Issue 1991 -end

		Me.LabelAccountType.AssociatedControlID = Me.DropDownListAccountType.ID
		Me.LabelContractType.AssociatedControlID = Me.DropDownListContractType.ID
		Me.LabelContribution.AssociatedControlID = Me.TextBoxContribution.ID
		Me.LabelContributionAmount.AssociatedControlID = Me.TextBoxContributionAmount.ID
		Me.LabelEffectiveDate.AssociatedControlID = Me.TextBoxEffectiveDate.ID
		Me.LabelTermDate.AssociatedControlID = Me.TextBoxTermDate.ID
		Me.LabelYMCA.AssociatedControlID = Me.DropDownListYmca.ID
		Me.TextBoxContribution.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
		Me.TextBoxContribution.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

		Me.TextBoxContributionAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
		Me.TextBoxContributionAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
		' Me.DropDownListContractType.Attributes.Add("onChange", "Javascript:OnBlur_Dropdown();")

		'Ashutosh Patil as on 30-Mar-2007
		'YREN-3203,YREN-3205
		Dim l_string_HireDate As Date
		Dim l_string_EnrollDate As Date

		Try
			Dim l_Dataset_YmcaList As DataSet
			Dim l_string_EmpEventId As String

			'----------------------------------------------------------------------------------------------------------------
			'Shashi: 04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )

			If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Participant Information -  Add / Update Additional Accounts"
				Headercontrol.FundNo = Session("FundNo").ToString().Trim()
			End If
			'------------------------------------------------------------------------------------------------------------------

			If Session("Flag") = "AddAccounts" Then


				'Shashi Singh : 2010-12-02 : For YRS-5.0-1186, BT-656---------------------------------
				If DropDownListAccountType.Items.FindByValue("AP") IsNot Nothing Then
					DropDownListAccountType.Items.Remove(DropDownListAccountType.Items.FindByValue("AP"))
				End If
				'-------------------------------------------------------------------------------------

				'Shubhrata
				If Not IsPostBack Then

					l_Dataset_YmcaList = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.GetYmcaList(Session("PersId"), Session("FundId"))
					Me.DropDownListYmca.DataSource = l_Dataset_YmcaList.Tables(0)
					Session("l_Dataset_YmcaList") = l_Dataset_YmcaList
					Me.DropDownListYmca.DataTextField = "YmcaName"

					'Rahul 22 Feb,06
					'Me.DropDownListYmca.DataValueField = "YmcaId"
					'Shubhrata 

					'Me.DropDownListYmca.DataValueField = "YmcaNo"  'commented by hafiz on 25-Jul-2007 - YREN-3617
					Me.DropDownListYmca.DataValueField = "EmpEventId" 'added by hafiz on 25-Jul-2007 - YREN-3617
					'Shubhrata
					'Session("YmcaNo") = l_Dataset_YmcaList.Tables(0).Rows(0)("YmcaNo")
					'Shubhrata
					'Rahul 22 Feb,06
					'Ashutosh Patil as on 30-Mar-2007
					'YREN-3203,YREN-3205
					'Commented by Swopna,01Aug08,YRS-5.0-478-Start
					'If Session("HireDate") Is Nothing Then
					'    l_string_HireDate = CType("1/1/1922", System.DateTime)
					'Else
					'    l_string_HireDate = Session("HireDate")
					'End If

					'If Session("EnrollmentDate") Is Nothing Then
					'    l_string_EnrollDate = CType("1/1/1922", System.DateTime)
					'Else
					'    l_string_EnrollDate = Session("EnrollmentDate")
					'End If

					'Me.TextboxHireDate.Text = l_string_HireDate
					'Me.TextboxEnrolldate.Text = l_string_EnrollDate
					'Commented by Swopna,01Aug08,YRS-5.0-478-End
					Me.DropDownListYmca.DataBind()
					'Commented and Modified By Ashutosh Patil as on 27-Apr-2007
					'For avoiding error if no records are present
					If l_Dataset_YmcaList.Tables(0).Rows.Count > 0 Then
						If l_Dataset_YmcaList.Tables(0).Rows(0).Item("EmpEventId").ToString() <> "System.DBNull" And l_Dataset_YmcaList.Tables(0).Rows(0).Item("EmpEventId").ToString() <> "" Then
							l_string_EmpEventId = l_Dataset_YmcaList.Tables(0).Rows(0).Item("EmpEventId").ToString()
							Session("EmpEventId") = l_string_EmpEventId
						End If
					End If
					'If l_Dataset_YmcaList.Tables(0).Rows(0).Item("EmpEventId").ToString() <> "System.DBNull" And l_Dataset_YmcaList.Tables(0).Rows(0).Item("EmpEventId").ToString() <> "" Then
					'    l_string_EmpEventId = l_Dataset_YmcaList.Tables(0).Rows(0).Item("EmpEventId").ToString()
					'    Session("EmpEventId") = l_string_EmpEventId
					'End If
					'Swopna YRS-5.0-478-01Aug08 -Start
				Else
					'when postback happens
					If Not Session("l_dataset_Employment") Is Nothing Then
						If CType(Session("l_dataset_Employment"), DataSet).Tables.Count <> 0 Then
							Dim l_dt_Employment As DataTable
							l_dt_Employment = CType(Session("l_dataset_Employment"), DataSet).Tables(0)
							If l_dt_Employment.Rows.Count <> 0 Then
								If Not Me.DropDownListYmca.SelectedItem Is Nothing Then
									Dim drow() As DataRow
									'commented by hafiz on 29-Aug-2008
									'drow = l_dt_Employment.Select("YmcaName='" & CType(Me.DropDownListYmca.SelectedItem.Text, String) & "' and Termdate is null")
									'added by hafiz on 29-Aug-2008
									drow = l_dt_Employment.Select("UniqueId='" & CType(Me.DropDownListYmca.SelectedItem.Value, String) & "' and Termdate is null")
									If drow.Length > 0 Then

										Dim i As Integer
										For i = 0 To drow.Length - 1

											If drow(i)("Termdate").GetType.ToString() = "System.DBNull" Then

												If drow(i)("HireDate").GetType.ToString() <> "System.DBNull" Then

													l_string_HireDate = DateValue(CType(drow(i)("HireDate"), String))
												Else
													l_string_HireDate = CType("1/1/1922", System.DateTime)
												End If

												If drow(i)("BasicPaymentDate").GetType.ToString() <> "System.DBNull" Then
													l_string_EnrollDate = DateValue(CType(drow(i)("BasicPaymentDate"), String))
												Else
													l_string_EnrollDate = CType("1/1/1922", System.DateTime)
												End If
											End If
										Next
									End If
								End If 'Not Me.DropDownListYmca.SelectedItem Is Nothing
							End If
						Else
							l_string_HireDate = CType("1/1/1922", System.DateTime)
							l_string_EnrollDate = CType("1/1/1922", System.DateTime)
						End If

						Me.TextboxHireDate.Text = l_string_HireDate
						Me.TextboxEnrolldate.Text = l_string_EnrollDate
					End If
					'Swopna YRS-5.0-478-01Aug08 -End
				End If
			ElseIf Session("Flag") = "EditAccounts" And Session("Flag") <> "EditedAccounts" And Not IsPostBack Then
				'YRS 5.0-478 Priya : added new keyword
				Dim YMCAListItem As New ListItem
				If Not Request.QueryString("UniqueId") Is Nothing Then
					l_Dataset_YmcaList = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.GetAdditionalAccount(Request.QueryString("UniqueId"))
					If l_Dataset_YmcaList.Tables(0).Rows.Count > 0 Then
						'added by Hafiz on 29-Aug-2008 for YRS 5.0-531
						YMCAListItem.Text = l_Dataset_YmcaList.Tables(0).Rows(0).Item("YmcaName").ToString.Trim
						YMCAListItem.Value = l_Dataset_YmcaList.Tables(0).Rows(0).Item("YmcaId").ToString.Trim

						'commented by Hafiz on 29-Aug-2008 for YRS 5.0-531
						'DropDownListYmca.Items.Add(l_Dataset_YmcaList.Tables(0).Rows(0).Item("YmcaName").ToString.Trim)

						'added by Hafiz on 29-Aug-2008 for YRS 5.0-531
						DropDownListYmca.Items.Add(YMCAListItem)

						DropDownListYmca.SelectedIndex = 1
					End If
				Else

					'added by Hafiz on 29-Aug-2008 for YRS 5.0-531
					YMCAListItem.Text = Session("YmcaName")
					YMCAListItem.Value = Session("EmpEventId")

					'commented by Hafiz on 29-Aug-2008 for YRS 5.0-531
					'DropDownListYmca.Items.Add(Session("YmcaName"))

					'added by Hafiz on 29-Aug-2008 for YRS 5.0-531
					DropDownListYmca.Items.Add(YMCAListItem)

					DropDownListYmca.SelectedIndex = 1
				End If

				DropDownListAccountType.SelectedValue = Session("AccountType")

				'SP : Made changes to rename contribution type monthly to Dollar -Start
				'Commented by SP: Start
				'If Session("BasisCode").ToString.Trim.ToUpper = "MONTHLY" Then
				'    DropDownListContractType.SelectedValue = "M"
				'ElseIf Session("BasisCode").ToString.Trim.ToUpper = "PERCENT" Then
				'    DropDownListContractType.SelectedValue = "P"
				'ElseIf Session("BasisCode").ToString.Trim.ToUpper = "LUMP SUM" Then
				'    DropDownListContractType.SelectedValue = "L"
				'End If
				'Commented by SP: End
				If Session("BasisCode").ToString.Trim.ToUpper = "M" Then
					DropDownListContractType.SelectedValue = "M"
				ElseIf Session("BasisCode").ToString.Trim.ToUpper = "P" Then
					DropDownListContractType.SelectedValue = "P"
				ElseIf Session("BasisCode").ToString.Trim.ToUpper = "L" Then
					DropDownListContractType.SelectedValue = "L"
				End If
				'SP : Made changes to rename contribution type monthly to Dollar -End

				TextBoxContributionAmount.Text = Session("ContributionAmt")

				TextBoxContribution.Text = Session("Contribution%")

				TextBoxEffectiveDate.Text = Session("EffectiveDate")
				If Session("TerminationDate") = "&nbsp;" Then
					TextBoxTermDate.Text = ""
				Else
					TextBoxTermDate.Text = Session("TerminationDate")
				End If

				DropDownListYmca.Enabled = False
				DropDownListAccountType.Enabled = False
				DropDownListContractType.Enabled = False
				TextBoxContribution.Enabled = False
				TextBoxContributionAmount.Enabled = False
				TextBoxEffectiveDate.Enabled = False
				'------------------------------------------------------------------------------------------------------
				'Shashi shekhar:28-Dec-2010: For YRS 5.0-567 , BT-648: TD Contract termination date --Make it "not editable"
				If ((Session("TerminationDate") = Nothing) Or (Session("TerminationDate") = "") Or (Session("TerminationDate") = "&nbsp;")) Then
					'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
					Dim datarow As DataRow
					datarow = findstatus()
					If Not datarow Is Nothing Then
						If IsDBNull(datarow("Termdate")) Then
							TextBoxTermDate.Enabled = False
						Else
							TextBoxTermDate.Enabled = True
						End If
					Else
						TextBoxTermDate.Enabled = True
					End If

					'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
					'Allow user to edit termination date of the most recent TD contract record excluding lump sum contribution
				ElseIf (EnableDisableTerminationTextBox(Session("TerminationDate"))) Then
					TextBoxTermDate.Enabled = True
				Else
					TextBoxTermDate.Enabled = False
					'Shagufta Chaudhari:2011.08.04:BT-893,YRS 5.0-1360 : Allow Lump Sum additional accts record 
					ButtonOK.Enabled = False
					'End:Shagufta chaudhari:2011.08.04:BT-893
				End If

				'------------------------------------------------------------------------------------------------------------

				End If
			'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			If IsPostBack Then
				If Request.Form("Yes") = "Yes" Then
					closewindow()
				ElseIf Request.Form("No") = "No" Then
					Session("l_dataset_AddAccount") = DirectCast(ViewState("OrignalDataset"), DataSet)
					TextBoxTermDate.Text = Session("TerminationDate")
				End If
			End If
		Catch ex As Exception

			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

		End Try

	End Sub



	Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
		Try

			Dim l_string_output As String
			Dim l_string_YmcaId As String = ""
			Dim l_string_AcctType As String
			Dim l_string_ContractType As String
			Dim l_string_ContibutionAmount As String
			Dim l_string_ContributionPctg As String
			Dim l_DataRows_AddAccount As DataRow() = Nothing
			Dim l_string_TerminateDate As Boolean = True
			'Dim l_Boolean_Flag As Boolean = True
			Dim l_string_CheckAcctType As String = ""
			Dim l_string_YmmcaNo As String = ""
			Dim l_string_EmpEventId As String = ""
			Dim dr As DataRow
			Dim l_Dataset_YmcaList As New DataSet
			Dim l_dataset_AddAccount As New DataSet
			Dim l_OriginalDS As DataSet

			Dim l_emp_datatable As DataTable
			Dim l_emp_datarow As DataRow
			Dim l_str_FundStatus As String
			Dim l_str_msg As String
			Dim l_str_YMCANo As String
			Dim l_bool_flag As Boolean
			Dim l_Term_date As Date
			Dim l_Enrollment_Date As Date
			'Dim l_Term_Month As String
			'Dim l_Term_Year As String
			'Dim l_Term_day As String
			Dim l_Max_HireDate As Date
			Dim l_Min_HireDate As Date
			'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			Dim datarow() As DataRow
			Dim showhide As Boolean
			'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			Dim statusrow As DataRow
			l_dataset_AddAccount = DirectCast(Session("l_dataset_AddAccount"), DataSet)
			l_Dataset_YmcaList = DirectCast(Session("l_Dataset_YmcaList"), DataSet)
			'Change By : Preeti On:08Mar06 IssueId:YMCA-1991 ISValidresolution function added. Enhancement : Change in TD Hardship Suspension Period from 12 months to 6 months. Start
			l_OriginalDS = DirectCast(Session("l_dataset_AddAccount"), DataSet).Copy()
			'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			ViewState("OrignalDataset") = l_OriginalDS
			'Change By : Preeti On:08Mar06 IssueId:YMCA-1991 ISValidresolution function added. Enhancement : Change in TD Hardship Suspension Period from 12 months to 6 months End
			'Start-Added by Pankaj on 20th April 2006

			'Ashutosh Patil as on 30-Mar-2007
			'YREN-3203,YREN-3205
			'Start Ashutosh Patil
			Dim Msg As String = BasicValidation()
			If Msg <> "" Then
				MessageBox.Show(PlaceHolder1, "Stop", Msg, MessageBoxButtons.Stop)
				Exit Sub
			End If

			'Shagufta Chaudhari:2011.08.04:BT-893 - This function checks whether new Effective Date is overlapping with an existing Payroll date or not.
			Dim validationMessage As String
			If Session("Flag") = "AddAccounts" Then
				l_DataRows_AddAccount = l_Dataset_YmcaList.Tables(0).Select("EmpEventId = '" & DropDownListYmca.SelectedItem.Value & "'")
				If l_DataRows_AddAccount.Length > 0 Then
					l_string_YmcaId = l_DataRows_AddAccount(0)("YmcaId").ToString()
					l_string_EmpEventId = l_DataRows_AddAccount(0)("EmpEventId").ToString()
					l_string_YmmcaNo = l_DataRows_AddAccount(0)("YmcaNo").ToString()
				Else
					MessageBox.Show(PlaceHolder1, "Stop", "Internal error", MessageBoxButtons.Stop)
					HelperFunctions.LogMessage("Unable to identify Ymcaid while adding new additional account contribution")
					Exit Sub
				End If

				validationMessage = IsValidPayrollDateOverlap(l_string_YmcaId, CDate(TextBoxEffectiveDate.Text))
				If validationMessage <> "" Then
					MessageBox.Show(PlaceHolder1, "Stop", validationMessage, MessageBoxButtons.Stop)
					Exit Sub
				End If
				'End: SC:2011.08.04:BT-893
				'Shagufta -12/07/2011 : Bug ID: 882'
				If TextBoxTermDate.Text.Trim() = "" Then
					validationMessage = IsAccountDateOverlap(CDate(TextBoxEffectiveDate.Text), Date.MaxValue)
				Else
					validationMessage = IsAccountDateOverlap(CDate(TextBoxEffectiveDate.Text), CDate(TextBoxTermDate.Text))
				End If
				If validationMessage <> "" Then
					MessageBox.Show(PlaceHolder1, "Stop", validationMessage, MessageBoxButtons.Stop)
					Exit Sub
				End If
				'End:Shagufta -12/07/2011 : Bug ID: 882'
			End If



			If Session("Flag") <> "EditAccounts" Then
				If Not Session("FundStatus") Is Nothing Then
					l_str_FundStatus = Session("FundStatus")
				Else
					l_str_FundStatus = String.Empty
				End If

				If DropDownListAccountType.SelectedValue.ToString = "AP" Then
					If l_str_FundStatus = "PRE-ELIGIBLE" Then
						MessageBox.Show(PlaceHolder1, "Stop", "AP Account can not be created for Pre-Eligible Participant.", MessageBoxButtons.Stop)
						Exit Sub
					Else
						If TextboxEnrolldate.Text.ToString = "" Then

							If Not Session("MaxHireDate") Is Nothing Then
								l_Max_HireDate = Session("MaxHireDate")
							Else
								l_Max_HireDate = CType("1/1/1922", System.DateTime)
							End If

							If Not Session("MinHireDate") Is Nothing Then
								l_Min_HireDate = Session("MinHireDate")
							Else
								l_Min_HireDate = CType("1/1/1922", System.DateTime)
							End If

							If DateDiff(DateInterval.Year, l_Max_HireDate, l_Min_HireDate) >= 2 Then
								l_Max_HireDate = DateValue(Month(l_Max_HireDate) & "/1/" & Year(l_Max_HireDate))
								If CType(TextBoxEffectiveDate.Text, System.DateTime) < CType(l_Max_HireDate, System.DateTime) Then
									MessageBox.Show(PlaceHolder1, "Stop", "Effective date must be greater than or equal to Enrollment date.", MessageBoxButtons.Stop)
									Exit Sub
								End If
							Else
								MessageBox.Show(PlaceHolder1, "Stop", "AP Account can not be created. Please check the eligibility.", MessageBoxButtons.Stop)
								Exit Sub
							End If
						Else
							If TextboxEnrolldate.Text = "1/1/1922" Then
								If Not Session("MaxHireDate") Is Nothing Then
									l_Max_HireDate = Session("MaxHireDate")
								Else
									l_Max_HireDate = CType("1/1/1922", System.DateTime)
								End If
								If CType(TextBoxEffectiveDate.Text, System.DateTime) < CType(l_Max_HireDate, System.DateTime) Then
									MessageBox.Show(PlaceHolder1, "Stop", "Effective date must be greater than or equal to Enrollment date.", MessageBoxButtons.Stop)
									Exit Sub
								End If
							Else
								If CType(TextBoxEffectiveDate.Text, System.DateTime) < CType(TextboxEnrolldate.Text, System.DateTime) Then
									MessageBox.Show(PlaceHolder1, "Stop", "Effective date must be greater than or equal to Enrollment date.", MessageBoxButtons.Stop)
									Exit Sub
								End If
							End If
						End If
					End If
				Else
					If CType(TextBoxEffectiveDate.Text, System.DateTime) < CType(TextboxHireDate.Text, System.DateTime) Then
						MessageBox.Show(PlaceHolder1, "Stop", "Effective date must be greater than or equal to Hire date.", MessageBoxButtons.Stop)
						Exit Sub
					End If
				End If
			End If

			validationMessage = PreEligibleValidation()
			If validationMessage <> "" Then
				MessageBox.Show(PlaceHolder1, "Stop", validationMessage, MessageBoxButtons.Stop)
				Exit Sub
			End If

			If DropDownListContractType.SelectedItem.Value <> "L" Then 'Shagufta Chaudhari:2011.08.04:BT-893 - This function checks whether new Effective Date is overlapping with an existing Payroll date or not.
				'Shubhrata May22 Reason YMCA 2152
				If Session("Flag") = "AddAccounts" Then
					'Shubhrata YREN 2564 jul 27th 2006
					'Dim l_string_YmmcaNo As String = Session("YmcaNo")
					'l_string_YmmcaNo = Me.DropDownListYmca.SelectedValue 'commented by hafiz on 25-Jul-2007 - YREN-3617
					l_string_EmpEventId = Me.DropDownListYmca.SelectedValue.ToString() 'commented by hafiz on 25-Jul-2007 - YREN-3617
					'Shubhrata
					Dim l_string_checkYmcaNo As String = ""
					Dim l_string_checkEmpEventId As String = ""
					l_string_CheckAcctType = DropDownListAccountType.SelectedValue
					'l_DataRows_AddAccount = l_dataset_AddAccount.Tables(0).Select("YmcaNo= '" & l_string_YmmcaNo & "'")
					Dim l_datarow As DataRow
					For Each l_datarow In l_Dataset_YmcaList.Tables(0).Rows
						'If l_string_YmmcaNo = l_datarow("YmcaNo") Then 'commented by hafiz on 25-Jul-2007 - YREN-3617
						If l_string_EmpEventId = l_datarow("EmpEventId").ToString() Then 'added by hafiz on 25-Jul-2007 - YREN-3617
							l_string_YmmcaNo = l_datarow("YmcaNo").ToString()
							l_string_YmcaId = l_datarow("YmcaId").ToString()
							l_string_EmpEventId = l_datarow("EmpEventId").ToString()
							'l_DataRows_AddAccount = l_dataset_AddAccount.Tables(0).Select("YmcaNo= '" & l_string_YmmcaNo & "'") 'commented by hafiz on 25-Jul-2007 - YREN-3617
							l_DataRows_AddAccount = l_dataset_AddAccount.Tables(0).Select("EmpEventId= '" & l_string_EmpEventId & "'") 'added by hafiz on 25-Jul-2007 - YREN-3617
							Exit For
						End If
					Next
					'Shubhrata YREN 2564
					For Each row As DataRow In l_DataRows_AddAccount
						l_string_checkYmcaNo = Convert.ToString(row("YmcaNo"))
						l_string_checkEmpEventId = row("EmpEventId").ToString()	'added by hafiz on 25-Jul-2007 - YREN-3617
						If Convert.ToString(row("AccountType")).Trim = l_string_CheckAcctType.Trim Then
							'If l_string_YmmcaNo = l_string_checkYmcaNo And Convert.ToString(row("TerminationDate")).Trim = "" Then 'commented by hafiz on 25-Jul-2007 - YREN-3617
							'If l_string_EmpEventId = l_string_checkEmpEventId And Convert.ToString(row("TerminationDate")).Trim = "" Then 'added by hafiz on 25-Jul-2007 - YREN-3617
							If l_string_EmpEventId.ToLower() = l_string_checkEmpEventId.ToLower() And (Convert.ToString(row("TerminationDate")).Trim = "" Or IsDBNull(row("TerminationDate"))) Then	'added by Imran on 14-Dec-2009 - BT-1064
								'IsDBNull(row("TerminationDate")) = True Then
								'Or row("TerminationDate") = "") 

								l_string_TerminateDate = False
							End If
						End If

					Next
					' End If

					If l_string_TerminateDate = False Then
						'------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
						'Start: Shashi Shekhar: 2010-12-10: For YRS 5.0-1239, BT-699 - "Account Exist" message need to be generic.
						'MessageBox.Show(PlaceHolder1, "Information", "There is an active TD/AP account for the selected YMCA " + l_string_checkYmcaNo, MessageBoxButtons.OK)
						MessageBox.Show(PlaceHolder1, "Information", "There is an active " + DropDownListAccountType.SelectedItem.Value.Trim() + " account for the selected YMCA " + l_string_checkYmcaNo, MessageBoxButtons.OK)
						'End: YRS 5.0-1239, BT-699
						'------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
						Exit Sub  'l_Boolean_Flag = False
					End If
				End If
			End If	'End: SC:2011.08.04:BT-893
			'Condition Modified Reason YMCA 2152
			'End-Added by Pankaj on 20th April 2006
			' If l_Boolean_Flag = True Then
			If TextBoxContributionAmount.Text = "" Then
				l_string_ContibutionAmount = "0"
			Else
				'l_string_ContibutionAmount = TextBoxContributionAmount.Text
				'anita and shubhrata apr24
				l_string_ContibutionAmount = FormatCurrency(Convert.ToDecimal(TextBoxContributionAmount.Text))
				'anita and shubhrata apr24
			End If
			If TextBoxContribution.Text = "" Then
				l_string_ContributionPctg = "0"
			Else
				l_string_ContributionPctg = TextBoxContribution.Text
			End If
			'l_string_YmcaId = DropDownListYmca.SelectedValue
			l_string_AcctType = DropDownListAccountType.SelectedValue
			l_string_ContractType = DropDownListContractType.SelectedValue
			If Session("Flag") = "AddAccounts" Then
				dr = l_dataset_AddAccount.Tables(0).NewRow
				'dr("EmpEventId") = Session("EmpEventId")
				dr("EmpEventId") = l_string_EmpEventId
				dr("YmcaId") = l_string_YmcaId
				'Rahul 22 Feb,06
				dr("YmcaNo") = l_string_YmmcaNo
				' dr("YmcaNo") = Session("YmcaNo")
				'Rahul 22 Feb,06
				dr("AccountType") = l_string_AcctType
				dr("BasisCode") = l_string_ContractType
				dr("Descriptions") = DropDownListContractType.SelectedItem.Text
				dr("Contribution%") = l_string_ContributionPctg

				'---------------------------------------------------------------------
				'Shashi:23 may 2011 :For Term date conversion (string to date type)
				If TextBoxTermDate.Text = "" Then
					dr("TerminationDate") = DBNull.Value
				Else
					dr("TerminationDate") = TextBoxTermDate.Text
				End If
				'-----------------------------------------------------------------
				If DropDownListContractType.SelectedItem.Value = "L" Then
					dr("TerminationDate") = TextBoxEffectiveDate.Text
				End If

				dr("EffectiveDate") = TextBoxEffectiveDate.Text
				dr("Contribution") = l_string_ContibutionAmount
				Session("YmcaName") = DropDownListYmca.SelectedItem.Text

				'added by Hafiz on 29-Aug-2008 for YRS 5.0-531
				Session("EmpEventId") = DropDownListYmca.SelectedItem.Value
				l_dataset_AddAccount.Tables(0).Rows.Add(dr)

				Session("l_dataset_AddAccount") = l_dataset_AddAccount

				'l_string_output = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.AddAdditionalAccount(Session("EmpEventId"), l_string_YmcaId, l_string_AcctType, l_string_ContractType, l_string_ContributionPctg, TextBoxEffectiveDate.Text, l_string_ContibutionAmount, TextBoxTermDate.Text)
			Else
				'Added by prasad 2012.02.03: For YRS 5.0-1531, BT-990: 
				'commented by prasad no need to add this session
				'Session("Flag") = "EditedAccounts"
				Dim drRows() As DataRow

				If Not Request.QueryString("UniqueId") Is Nothing Then
					drRows = l_dataset_AddAccount.Tables(0).Select("uniqueid='" & Request.QueryString("UniqueId") & "'")
					dr = drRows(0)

				End If
				If Not Request.QueryString("Index") Is Nothing Then
					dr = l_dataset_AddAccount.Tables(0).Rows(Request.QueryString("Index"))

				End If
				' l_string_output = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.UpdateAdditionalAccount(Session("UniqueId"), TextBoxTermDate.Text)

				'------------------------------------------------------------------
				'Shashi:23 may 2011 :For Term date conversion (string to date type)
				'dr("TerminationDate") = TextBoxTermDate.Text
				If Not Request.QueryString("UniqueId") Is Nothing OrElse Not Request.QueryString("Index") Is Nothing Then
					If TextBoxTermDate.Text = "" Then
						'Added by prasad 2012.02.03: For YRS 5.0-1531, BT-990: 
						validationMessage = CheckActiveRecord()
						If validationMessage <> "" Then
							MessageBox.Show(PlaceHolder1, "Stop", validationMessage, MessageBoxButtons.Stop)
							Exit Sub
						Else
							dr("TerminationDate") = DBNull.Value
							'Added by prasad bugID:1019 Unable to remove termination date of most recent terminated TD Acct. for Active participant.
							ViewState("MessageBoxFlag") = False

						End If
					Else
							'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date

							If Not dr("TerminationDate").Equals(DBNull.Value) And TextBoxTermDate.Enabled = True Then
								'Added by prasad 2012.03.06 for BT-1008,Application allowing to update Voluntary account termination date other than most recent terminated record for non active participant.
								'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
								If checkstatusandempterminationdate() Then
									Exit Sub
								Else
									dr("TerminationDate") = TextBoxTermDate.Text
									showhide = UpdateActiveAccountEffectiveDate(l_dataset_AddAccount, l_OriginalDS)
								End If
							Else
								'Added by prasad 2012.03.06 for BT-1008,Application allowing to update Voluntary account termination date other than most recent terminated record for non active participant.
								If checkstatusandempterminationdate() Then
									Exit Sub
								Else
									dr("TerminationDate") = TextBoxTermDate.Text
								End If
							End If

						End If
					End If
				'------------------------------------------------------------------------
			End If

			'Change By : Preeti On:08Mar06 IssueId:YMCA-1991 ISValidresolution function added. Functionality has been missing. Start
			If Session("Flag") = "AddAccounts" AndAlso IsValidResolution() = False Then
				Session("l_dataset_AddAccount") = l_OriginalDS
				Exit Sub
			End If

			'If IsValidResolution() = True Then
			'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			If showhide = False And ViewState("MessageBoxFlag") = False Then
				closewindow()
				'Added by prasad 2012.03.05 For:(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			ElseIf showhide = True And ViewState("MessageBoxFlag") = False Then
				Dim effdate As Date
				Dim oldeffdate As Date
				effdate = CType(TextBoxTermDate.Text, Date)
				oldeffdate = CType(ViewState("oldeffectivedate"), Date)
				'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
				If oldeffdate <> effdate.AddDays(1) Then
					MessageBox.Show(PlaceHolder1, "Stop", String.Format(Resources.AddAdditionalAccounts.MESSAGE_DO_YOU_WISH_TO_CHANGE_EFFECTIVE_DATE_OF_ACTIVE_RECORD, oldeffdate.ToString("MM/dd/yyy"), effdate.AddDays(1).ToString("MM/dd/yyyy")), MessageBoxButtons.YesNo)
				Else
					'Added by prasad 2012.03.05 For:(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
					closewindow()
				End If
				'Added by prasad 2012.03.05 For:(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			ElseIf showhide = False And ViewState("MessageBoxFlag") = True Then
				Session("l_dataset_AddAccount") = l_OriginalDS
			End If
			'Else
			'Session("l_dataset_AddAccount") = l_OriginalDS
			'End If

			'Close Window
			'Change By : Preeti On:08Mar06 IssueId:YMCA-1991 ISValidresolution function added. Functionality has been missing. End
			'End If
		Catch ex As Exception

			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

		End Try
	End Sub

	Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

		Dim msg As String
		msg = msg + "<Script Language='JavaScript'>"
		'Code commented Reason Same as YMCA 2152
		'msg = msg + "window.opener.document.forms(0).submit();"
		'Code commented Reason Same as YMCA 2152
		msg = msg + "self.close();"

		msg = msg + "</Script>"

		Response.Write(msg)


		'Dim closeWindow As String = "<script language='javascript'>" & _
		'                                                 "self.close();" & _
		'                                                 "</script>"

		'If (Not Me.IsStartupScriptRegistered("CloseWindow")) Then
		'    Page.RegisterStartupScript("CloseWindow", closeWindow)
		'End If

	End Sub
	'anita and shubhrata 
	Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
		Try
			Dim n As String
			Dim m As String()
			Dim myNum As String
			'Changed by Ruchi on 7th March,2006
			Dim myDec As String
			'end of change
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
				'start of change


				'end of change
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
	'anita and shubhrata
	'Shagufta Chaudhari :2011.08.04 -For BT-893,YRS 5.0-1360 : Allow Lump Sum additional accts record 
	Private Sub DropDownListContractType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownListContractType.SelectedIndexChanged
		Try
			'SP : Made changes to rename contribution type monthly to Dollar -Start
			'Commented by SP: Start
			'If Me.DropDownListContractType.SelectedItem.Text = "Monthly" Then
			'    Me.TextBoxContribution.Text = ""
			'    Me.TextBoxContribution.Enabled = False
			'    Me.TextBoxContributionAmount.Enabled = True
			'    Me.TextBoxTermDate.Enabled = True
			'ElseIf Me.DropDownListContractType.SelectedItem.Text = "Percent" Then
			'    Me.TextBoxContributionAmount.Text = ""
			'    Me.TextBoxContributionAmount.Enabled = False
			'    Me.TextBoxContribution.Enabled = True
			'    Me.TextBoxTermDate.Enabled = True
			'ElseIf Me.DropDownListContractType.SelectedItem.Text = "Lump Sum" Then
			'    Me.TextBoxContribution.Text = ""
			'    Me.TextBoxContribution.Enabled = False
			'    Me.TextBoxContributionAmount.Enabled = True
			'    Me.TextBoxTermDate.Text = ""
			'    Me.TextBoxTermDate.Enabled = False

			'End If
			'Commented by SP: End

			If Me.DropDownListContractType.SelectedValue.ToString().ToUpper() = "M" Then
				Me.TextBoxContribution.Text = ""
				Me.TextBoxContribution.Enabled = False
				Me.TextBoxContributionAmount.Enabled = True
				Me.TextBoxTermDate.Enabled = True
			ElseIf Me.DropDownListContractType.SelectedValue.ToString().ToUpper() = "P" Then
				Me.TextBoxContributionAmount.Text = ""
				Me.TextBoxContributionAmount.Enabled = False
				Me.TextBoxContribution.Enabled = True
				Me.TextBoxTermDate.Enabled = True
			ElseIf Me.DropDownListContractType.SelectedValue.ToString().ToUpper() = "L" Then
				Me.TextBoxContribution.Text = ""
				Me.TextBoxContribution.Enabled = False
				Me.TextBoxContributionAmount.Enabled = True
				Me.TextBoxTermDate.Text = ""
				Me.TextBoxTermDate.Enabled = False
			End If
			'SP : Made changes to rename contribution type monthly to Dollar -End
		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

		End Try

	End Sub	 'End:Shagufta :2011.08.04 :Bug ID-893'

	Private Sub PopcalendarDate_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PopcalendarDate.SelectionChanged
		Try
			Dim l_date As Date
			l_date = PopcalendarDate.SelectedDate
			If l_date.Day <> 1 Then
				''Ashutosh on 02-june-06 bug No  YREN-2342// Adding month when day >1 
				l_date = l_date.AddMonths(1)
				'*****************************
				l_date = Convert.ToString(l_date.Month()) + "/" + "1/" + Convert.ToString(l_date.Year())
				TextBoxEffectiveDate.Text = l_date
			Else
				TextBoxEffectiveDate.Text = l_date
			End If
		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

		End Try

	End Sub


	Private Sub Popcalendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Popcalendar1.SelectionChanged
		Try
			Dim l_date As Date
			l_date = Popcalendar1.SelectedDate
			If l_date.Day <> 1 Then
				''Ashutosh on 02-june-06 bug No  YREN-2342 when day >1 *************
				l_date = l_date.AddMonths(1)
				'***********************
				l_date = Convert.ToString(l_date.Month()) + "/" + "1/" + Convert.ToString(l_date.Year())
				TextBoxTermDate.Text = l_date
			Else
				TextBoxTermDate.Text = l_date
			End If
		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

		End Try
	End Sub
	'/b Change: by Vipul - 01March06 YRS Enhancement : Change in TD Hardship Suspension Period from 12 months to 6 months
	'Change By : Preeti On:08Mar06 IssueId:YMCA-1991 ISValidresolution function added. Enhancement : Change in TD Hardship Suspension Period from 12 months to 6 months Added this method
	Private Function IsValidResolution() As Boolean
		'Read Data entered by the user 
		Dim l_dataset_AddAccount As New DataSet
		Dim chrAcctType As String
		Dim dtsTerminationDate As String
		Dim dtmEffDate As String
		Dim l_string_output As String

		Dim lcPersID As String
		Dim intLastRow As Integer
		Dim dateTemp As Date
		Try

			'This table is used same as t_r_allmemberElectives in foxpro.
			l_dataset_AddAccount = DirectCast(Session("l_dataset_AddAccount"), DataSet)
			intLastRow = l_dataset_AddAccount.Tables(0).Rows.Count - 1
			chrAcctType = l_dataset_AddAccount.Tables(0).Rows(intLastRow)("AccountType")

			dtsTerminationDate = IIf((IsNothing(l_dataset_AddAccount.Tables(0).Rows(intLastRow)("TerminationDate")) = True Or IsDBNull(l_dataset_AddAccount.Tables(0).Rows(intLastRow)("TerminationDate")) = True), "", l_dataset_AddAccount.Tables(0).Rows(intLastRow)("TerminationDate"))

			dtmEffDate = l_dataset_AddAccount.Tables(0).Rows(intLastRow)("EffectiveDate")
			lcPersID = Session("PersId")

			If ((chrAcctType.Trim().ToUpper() = "TD" Or chrAcctType.Trim().ToUpper() = "AP") And dtsTerminationDate = "") Then
				If lcPersID <> "" Then
					l_string_output = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.ValidateDateForTDRenewal(lcPersID, dtmEffDate)
					If l_string_output <> "" Then
						MessageBox.Show(PlaceHolder1, "Error", l_string_output, MessageBoxButtons.Stop)
						Return False
					End If
				End If
			End If
			'lceffdate = .txtdtmeffdate.Value
			'lcaccttype = .cboacyfAccountType.Value
			'in combine table
			Dim strWhereCondition As String
			'where dtmeffdate = lceffdate And Isnull(dtsterminationdate) And chrAcctType = lcaccttype;
			strWhereCondition = "EffectiveDate = '" + dtmEffDate + "' And TerminationDate IS NULL And AccountType='" + chrAcctType.Trim().ToUpper() + "'"
			Dim dr As DataRow()
			dr = l_dataset_AddAccount.Tables(0).Select(strWhereCondition)
			If Not dr Is Nothing And dr.Length > 0 Then
				'Shashi Shekhar:27-05-2011: this validation seems never execute and there is nothing mention about this in corresponding PS odc(Discussed with Nikunj for this)
				'MessageBox.Show(PlaceHolder1, "Error", "Resolution is invalid.", MessageBoxButtons.Stop)
				'Return False
			Else
				Dim drMaxTermDate() As DataRow
				strWhereCondition = "EffectiveDate='" + dtmEffDate + "' And AccountType='" + chrAcctType.Trim().ToUpper() + "'"
				drMaxTermDate = l_dataset_AddAccount.Tables(0).Select(strWhereCondition) ', "TerminationDate DESC") 'Select Max(TerminationDate)

				If Not drMaxTermDate Is Nothing Then
					If drMaxTermDate.Length > 0 Then
						'sort for maximum Termination date 
						Dim i, j As Integer
						Dim drMax As DateTime

						For i = 0 To drMaxTermDate.Length - 1
							'First time initialization of non null date
							'Start: Shagufta Chaudhari: 2011-07-04: For BT-882 - drMaxTermDate(i)("TerminationDate") <> ""  has removed'
							'And Replace IsDBNull(drMaxTermDate(j)("TerminationDate")) with IsDBNull(drMaxTermDate(i)("TerminationDate"))
							'If Not drMaxTermDate(i)("TerminationDate") Is Nothing And drMaxTermDate(i)("TerminationDate") <> "" And Not IsDBNull(drMaxTermDate(j)("TerminationDate")) Then
							If Not drMaxTermDate(i)("TerminationDate") Is Nothing And Not IsDBNull(drMaxTermDate(i)("TerminationDate")) Then
								drMax = DateTime.Parse(drMaxTermDate(i)("TerminationDate"))
								Exit For
							End If
						Next
						If i < drMaxTermDate.Length - 1 Then
							For j = i To drMaxTermDate.Length - 1
								'Start: Shagufta Chaudhari: 2011-07-04: For BT-882 - drMaxTermDate(i)("TerminationDate") <> ""  has removed'
								'If Not drMaxTermDate(j)("TerminationDate") Is Nothing And drMaxTermDate(j)("TerminationDate") <> "" And Not IsDBNull(drMaxTermDate(j)("TerminationDate")) Then
								If Not drMaxTermDate(j)("TerminationDate") Is Nothing And Not IsDBNull(drMaxTermDate(j)("TerminationDate")) Then
									If drMax < DateTime.Parse(drMaxTermDate(j)("TerminationDate")) Then
										drMax = DateTime.Parse(drMaxTermDate(j)("TerminationDate"))
									End If
								End If
							Next
						End If
						'Ashutosh on 02-june-06 bug No  YREN-2342 ----------
						If TextBoxTermDate.Text <> "" Then

							If ViewState("Check") Is Nothing Then
								l_dataset_AddAccount.Tables(0).Rows(intLastRow)("EffectiveDate") = TextBoxEffectiveDate.Text
								MessageBox.Show(PlaceHolder1, "Information  ", "Termination date : " & drMaxTermDate(0)("TerminationDate"), MessageBoxButtons.OK)
								ViewState("Check") = "Ok"
								Return False
							End If
						End If
						' ------------********
						'Ashutosh on 02-june-06 bug No  YREN-2342
						'    If (DateTime.Parse(dtmEffDate) < drMax) Then
						'        TextBoxEffectiveDate.Text = drMax.ToShortDateString()
						'        'change it in to session variable
						'        l_dataset_AddAccount.Tables(0).Rows(intLastRow)("EffectiveDate") = TextBoxEffectiveDate.Text
						'        MessageBox.Show(PlaceHolder1, "Information", "Changing effective date to:" & drMaxTermDate(0)("TerminationDate"), MessageBoxButtons.OK)
						'        Return False
						'    End If
						''    ' *********
					End If
				End If
			End If
			Return True

		Catch ex As Exception
			Throw
		End Try

	End Function


	'Shagufta -12/07/2011 : This function checks whether new record is overlapping with an existing elective or not.(Bug ID: 882)
	'This function returns True if it finds an existing Account contribution with overlapping dates.
	Public Function IsAccountDateOverlap(ByVal dt_NewEffDate As Date, ByVal dt_NewTermDate As Date) As String
		If DropDownListContractType.SelectedItem.Value = "L" Then Return ""
		Dim l_dataset_AddAccount As DataSet
		Dim dt_TermDate, dt_Effdate As Date
		Dim IsDateOveralap As String = ""

		l_dataset_AddAccount = DirectCast(Session("l_dataset_AddAccount"), DataSet)
		If HelperFunctions.isEmpty(l_dataset_AddAccount) Then Return ""
		Dim row As DataRow
		For Each row In l_dataset_AddAccount.Tables(0).Rows
			dt_Effdate = CDate(row("EffectiveDate").ToString())

			'If different Account or different YMCA then no need to  validate
			If ((DropDownListYmca.SelectedItem.Value.Trim().ToUpper() <> row("EmpEventId").ToString().Trim().ToUpper()) _
			 OrElse (DropDownListAccountType.SelectedItem.Value.Trim().ToUpper() <> row("AccountType").ToString().Trim().ToUpper())) _
			OrElse (row("BasisCode").ToString() = "L") Then Continue For

			'if Existing Termination date is Empty or null
			If (row("TerminationDate").ToString()) = "" Then
				dt_TermDate = Date.MaxValue
			Else
				dt_TermDate = CDate(row("TerminationDate").ToString())
			End If

			'If existing elective has both effective and term date
			If dt_NewTermDate >= dt_Effdate And dt_NewEffDate <= dt_TermDate Then
				Return "The new record you are trying to add is overlapping with an existing elective."

			End If

		Next
		Return ""
	End Function

	'Shagufta Chaudhari:2011.08.04:BT-893 - This function checks whether new Effective Date is overlapping with an existing Payroll date or not.
	Private Function IsValidPayrollDateOverlap(ByVal parameterYmcaId As String, ByVal parameterEffectiveDate As DateTime) As String
		If DropDownListContractType.SelectedItem.Value <> "L" Then Return ""

		Dim l_dataset_AddAccount As DataSet = DirectCast(Session("l_dataset_AddAccount"), DataSet)
		Dim dt_Effdate As Date
		Dim coutput As String = ""
		Dim row As DataRow

		If HelperFunctions.isNonEmpty(l_dataset_AddAccount) Then
			Dim l_EmpEventId As String = DropDownListYmca.SelectedItem.Value.Trim().ToUpper()
			For Each row In l_dataset_AddAccount.Tables(0).Rows
				If l_EmpEventId <> row("EmpEventId").ToString().Trim().ToUpper() Then Continue For
				If DropDownListContractType.SelectedItem.Value <> row("BasisCode").ToString().Trim().ToUpper() Then Continue For
				dt_Effdate = CDate(row("EffectiveDate"))
				If parameterEffectiveDate = dt_Effdate Then Return "Cannot add a lump sum contribution for the selected YMCA and selected Account because one already exists with the same effective date"
			Next
		End If
		'Code to Compare Effective date with the Exisiting Payroll Date
		coutput = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.IsValidPayrollDateOverlap(parameterYmcaId, parameterEffectiveDate)
		Return coutput
	End Function	'End: SC:2011.08.04:BT-893

	'Shagufta Chaudhari:2011.08.11- Code optimization
	Private Function BasicValidation() As String
		If TextBoxEffectiveDate.Text.Trim = "" Then
			Return "Effective date can not be blank."
		End If

		If IsDate(TextBoxEffectiveDate.Text) = False Then
			Return "Invalid Effective Date."
		End If

		If TextBoxTermDate.Text <> "" AndAlso IsDate(TextBoxTermDate.Text) = False Then
			Return "Invalid Term Date."
		End If

		If TextBoxTermDate.Text <> "" AndAlso CDate(TextBoxTermDate.Text) < CDate(TextBoxEffectiveDate.Text) Then
			Return "Termination date cannot be earlier to effective date."
		End If
		'Added by prasad YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
		'Added by prasad as on 25-Apr-2012 YRS 5.0-1531: Need to be able to update atsEmpElectives termination date limit only for update
		If Session("Flag") = "EditAccounts" Then
			Return checkTerminationDateLimit()
		End If
		Return ""
	End Function	'End:Shagufta Chaudhari:2011.08.11- Code optimization

	'Ashutosh Patil as on 10-Apr-2007
	'YREN-3203,YREN-3205
	Private Function PreEligibleValidation() As String
		Dim l_bool_flag As Boolean
		Dim l_str_FundStatus As String
		Dim l_emp_datatable As DataTable
		Dim l_string_EmpEventId As String = ""
		Dim l_Term_date As Date
		Dim l_Enrollment_Date As Date
		Dim l_str_msg As String

		l_bool_flag = True
		If Not Session("FundStatus") Is Nothing Then
			l_str_FundStatus = Session("FundStatus")
		Else
			l_str_FundStatus = String.Empty
		End If

		If l_str_FundStatus.ToUpper() = "PRE-ELIGIBLE" Then	  ''updated by sanjay on 24 Aug 09 for YRS 5.0-874
			l_emp_datatable = DirectCast(Session("EmploymentAccountDataTable"), DataTable)
			'l_str_YMCANo = DropDownListYmca.SelectedValue.ToString 'commented by hafiz on 25-Jul-2007 - YREN-3617
			l_string_EmpEventId = DropDownListYmca.SelectedValue.ToString 'added by hafiz on 25-Jul-2007 - YREN-3617
			If l_emp_datatable.Rows.Count > 0 Then
				For Each l_emp_datarow In l_emp_datatable.Rows
					'If l_emp_datarow("YmcaNo") = l_str_YMCANo Then 'commented by hafiz on 25-Jul-2007 - YREN-3617
					If l_emp_datarow("UniqueId").ToString() <> l_string_EmpEventId Then Continue For
					'If l_emp_datarow("UniqueId").ToString() = l_string_EmpEventId Then 'added by hafiz on 25-Jul-2007 - YREN-3617
					'For a Terminated PRE-ELGIBLE Participant
					'Participant Hire Date - 3/16/2002 Enrollment Date - 4/1/2003 Termination Date - 7/1/2005
					'In this case Account can be created in between Enrollment Date and one month before Termination Date i.e. in between 4/1/2003 and 6/1/2005
					If IsDBNull(l_emp_datarow("TermDate")) = True OrElse l_emp_datarow("TermDate").ToString = "" Then Continue For
					'If IsDBNull(l_emp_datarow("TermDate")) = False And l_emp_datarow("TermDate").ToString <> "" Then
					'l_Term_Month = Month(l_emp_datarow("TermDate"))
					'l_Term_Year = Year(l_emp_datarow("TermDate"))
					'If l_Term_Month = 1 Then
					'    l_Term_Month = 12
					'    l_Term_Year = l_Term_Year - 1
					'Else
					'    l_Term_Month = l_Term_Month - 1
					'End If
					'l_Term_Month = l_Term_Month & "/"
					'l_Term_Year = "/" & l_Term_Year

					'l_Term_day = Day(l_emp_datarow("TermDate"))
					'optimizing code
					l_Term_date = CType(l_emp_datarow("TermDate"), Date).AddMonths(-1) 'DateValue(l_Term_Month & l_Term_day & l_Term_Year)

					If IsDBNull(l_emp_datarow("BasicPaymentDate")) = False Then
						l_Enrollment_Date = l_emp_datarow("BasicPaymentDate")
					Else
						l_bool_flag = False
						l_str_msg = "Accounts can be added only after Enrollment and one month before Termination for a terminated Participant."
						Exit For
					End If
					If CType(TextBoxEffectiveDate.Text, System.DateTime) < l_Enrollment_Date Or CType(TextBoxEffectiveDate.Text, System.DateTime) > l_Term_date Then
						l_str_msg = "Accounts can be added only after Enrollment and one month before Termination for a terminated Participant."
						l_bool_flag = False
						Exit For
					End If
					' End If
					'End If
				Next
			End If
		End If
		If l_bool_flag = False Then
			Return l_str_msg
		End If
		Return ""

	End Function	  'End Ashutosh Patil
	'Added by prasad 2012.02.08 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
	Private Function EnableDisableTerminationTextBox(ByVal terminationdate As String) As Boolean
		'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
		Dim datarowstatus As DataRow
		Try
			datarowstatus = findstatus()
			If Not datarowstatus Is Nothing Then
				If IsDBNull(datarowstatus("Termdate")) Then
					Return False
				Else
					Return mostrecentrecord(terminationdate)
				End If
			End If
			Return mostrecentrecord(terminationdate)
		Catch sqlEx As System.Data.SqlClient.SqlException
			Throw sqlEx
		Catch ex As Exception
			Throw ex
		End Try
	End Function
	'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
	Private Function UpdateActiveAccountEffectiveDate(ByVal para_l_dataset_AddAccount As DataSet, ByVal para_l_dataset_Orignal As DataSet) As Boolean
		Try
			Dim activerecordrow() As DataRow
			Dim mostrecentrecordrow() As DataRow
			Dim updatedrow() As DataRow
			Dim terminationdate As Date
			Dim effectivedate As Date
			'Added by prasad 2012.03.16 For:(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			'Dim limitdays As String
			'Added by prasad 2012.03.05 For:(REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			ViewState("MessageBoxFlag") = False
			activerecordrow = para_l_dataset_Orignal.Tables(0).Select("AccountType='TD' AND TerminationDate IS NULL AND BasisCode <> 'L' AND YmcaNo='" + Request.QueryString("YmcaId") + "'")
			'Changed by prasad YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
			'Added by prasad 2012.03.15 (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
			'limitdays = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.TdAccountDayLimit()
			'If activerecordrow.Length = 0 Then
			'Added by prasad 2012.03.15 (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
			'Return checkTerminationDateLimit(limitdays)
			'Else
			If activerecordrow.Length <> 0 Then
				If TextBoxTermDate.Enabled Then
					mostrecentrecordrow = para_l_dataset_Orignal.Tables(0).Select("AccountType='TD' AND BasisCode <> 'L' AND YmcaNo='" + Request.QueryString("YmcaId") + "'", "TerminationDate desc")
					effectivedate = CType(activerecordrow(0)("EffectiveDate"), Date)
					ViewState("oldeffectivedate") = effectivedate
					terminationdate = mostrecentrecordrow(0)("TerminationDate")
					'codition check whether most recent record and active records are in continuation
					'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
					If terminationdate.AddDays(1) = effectivedate OrElse terminationdate = effectivedate Then
						updatedrow = para_l_dataset_AddAccount.Tables(0).Select("AccountType='TD' AND TerminationDate IS NULL AND BasisCode <> 'L'AND YmcaNo='" + Request.QueryString("YmcaId") + "'")
						If updatedrow.Length <> 0 Then
							'Added by prasad 2012.03.15 (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
							'If checkTerminationDateLimit(limitdays) Then
							'Return False
							'End If
							effectivedate = CType(TextBoxTermDate.Text, Date)
							updatedrow(0)("EffectiveDate") = effectivedate.AddDays(1)
							Return True
						End If
					ElseIf CType(TextBoxTermDate.Text, Date) >= effectivedate Then
						MessageBox.Show(PlaceHolder1, "Stop", Resources.AddAdditionalAccounts.MESSAGE_ENTER_DATE_LESS_THAN_EFFECTIVE_DATE_OF_ACTIVE_RECORD & effectivedate, MessageBoxButtons.Stop)
						ViewState("MessageBoxFlag") = True
					End If
				End If
			End If
		Catch sqlEx As System.Data.SqlClient.SqlException
			Throw sqlEx
		Catch ex As Exception
			Throw ex
		End Try
	End Function
	'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
	Private Function CheckActiveRecord() As String
		Try
			'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			Dim datarowstatus As DataRow
			Dim activerecordrow() As DataRow
			Dim l_dataset_AddAccount As DataSet
			datarowstatus = findstatus()
			'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
			If Not datarowstatus Is Nothing Then
				Return Resources.AddAdditionalAccounts.MESSAGE_NON_ACTIVE_PARTICIPANT_CANNOT_KEEP_TERMINATION_DATE_BLANK
			Else
				l_dataset_AddAccount = DirectCast(Session("l_dataset_AddAccount"), DataSet)
				activerecordrow = l_dataset_AddAccount.Tables(0).Select("AccountType='TD' AND TerminationDate IS NULL AND BasisCode <> 'L' AND YmcaNo='" + Request.QueryString("YmcaId") + "'")
				If Session("TerminationDate") = Nothing OrElse Session("TerminationDate") = "&nbsp;" OrElse Session("TerminationDate") = "" OrElse activerecordrow.Length < 1 Then Return ""
				If activerecordrow.Length >= 1 Then
					Return Resources.AddAdditionalAccounts.MESSAGE_ACTIVE_RECORD_EXIST
				End If
			End If
		Catch ex As Exception
			Throw ex
		End Try
	End Function
	'Added by prasad 2012.02.09 For BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
	Private Function closewindow()
		Dim script As String
		script = script + "<Script Language='JavaScript'>"

		script = script + "window.opener.document.forms(0).submit();"

		script = script + "self.close();"

		script = script + "</Script>"
		Response.Write(script)
	End Function
	'Added by prasad 2012.03.02	For (REOPEN)BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
	Private Function findstatus() As DataRow
		Dim l_dataset_AddAccount As DataSet
		Dim l_employment As DataSet
		Dim datarowterminationdate() As DataRow
		Dim accountrow() As DataRow
		l_dataset_AddAccount = DirectCast(Session("l_dataset_AddAccount"), DataSet)
		accountrow = l_dataset_AddAccount.Tables(0).Select("AccountType='TD' AND BasisCode <> 'L' AND YmcaNo='" + Request.QueryString("YmcaId") + "'")
		l_employment = DirectCast(Session("l_dataset_Employment"), DataSet)
		datarowterminationdate = l_employment.Tables(0).Select("YmcaNo='" + Request.QueryString("YmcaId") + "'")
		If datarowterminationdate.Length <> 0 And accountrow.Length <> 0 Then
			If datarowterminationdate(0)("StatusType").ToString.Trim <> "A" And accountrow(0)("AccountType").ToString.Trim = "TD" Then
				Return datarowterminationdate(0)
			End If
		End If
	End Function
	'Added by prasad 2012.03.06 for BT-1008,Application allowing to update Voluntary account termination date other than most recent terminated record for non active participant.
	Private Function mostrecentrecord(ByVal terminationdate As String) As Boolean
		Dim l_terminationdate As DataSet
		Dim datarow() As DataRow
		If Not Session("l_dataset_AddAccount") Is Nothing Then
			l_terminationdate = DirectCast(Session("l_dataset_AddAccount"), DataSet)
			datarow = l_terminationdate.Tables(0).Select("AccountType='TD' AND BasisCode <> 'L' AND YmcaNo='" + Request.QueryString("YmcaId") + "'", "TerminationDate desc")
			If datarow.Length <> 0 Then
				If Not datarow(0).IsNull("TerminationDate") Then
					If terminationdate = datarow(0)("TerminationDate") Then
						Return True
					Else
						Return False
					End If
				End If
			End If
		End If
	End Function
	'Added by prasad 2012.03.06 for BT-1008,Application allowing to update Voluntary account termination date other than most recent terminated record for non active participant.
	Private Function checkstatusandempterminationdate()
		Dim statusrow As DataRow
		statusrow = findstatus()
		If Not statusrow Is Nothing Then
			If CType(TextBoxTermDate.Text, Date) > CType(statusrow("Termdate"), Date) Then
				MessageBox.Show(PlaceHolder1, "Stop", Resources.AddAdditionalAccounts.MESSAGE_ENTER_DATE_LESS_THAN_EMPLOYMENT_TERMINATION_DATE, MessageBoxButtons.Stop)
				Return True
			End If
		End If
	End Function
	'Added by prasad (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
	Private Function checkTerminationDateLimit() As String
		Dim length As Integer
		Dim lastchar As String
		Dim daymonth As Double
		Dim limit As String
		'Changed by prasad YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
		limit = YMCARET.YmcaBusinessObject.AddAdditionalAccountsBOClass.TdAccountDayLimit()
		length = limit.Length
		lastchar = limit.Substring(length - 1)
		limit = limit.Remove(length - 1)
		daymonth = CType(limit, Double)
		If lastchar.ToUpper = "M" Then
			If TextBoxTermDate.Text.Trim = "" Then Return ""
			If CType(TextBoxTermDate.Text, Date) <= Date.Now.AddMonths(daymonth) Then Return ""
			'Added by prasad bugID:1019 Unable to remove termination date of most recent terminated TD Acct. for Active participant.
			Return callDayLimitMessageBox(daymonth, "months")
		ElseIf lastchar.ToUpper = "D" Then
			If TextBoxTermDate.Text.Trim = "" Then Return ""
			If CType(TextBoxTermDate.Text, Date) <= Date.Now.AddDays(daymonth) Then Return ""
			'Added by prasad bugID:1019 Unable to remove termination date of most recent terminated TD Acct. for Active participant.
			Return callDayLimitMessageBox(daymonth, "days")
		End If
	End Function
	'Changed by prasad YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
	Private Function callDayLimitMessageBox(ByVal limit As String, ByVal dayMonth As String) As String
		Return String.Format(Resources.AddAdditionalAccounts.MESSAGE_ENTER_DATE_LESS_THAN_DAYSMONTHS_FROM_TODAY, limit, dayMonth)
	End Function
End Class
