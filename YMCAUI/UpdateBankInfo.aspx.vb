'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	UpdateBankInfo.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:12:31 PM
' Program Specification Name	:	YMCA PS 3.7.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Hafiz 03Feb06 Cache-Session
'*******************************************************************************
'Changed By:        On:                  IssueId            Issue Description
'Hafiz              04Feb06                                 Cache-Session   
'Aparna Samala      30 Jan - 2007       YREN -3037          Only View details instead of updating
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Ashutosh Patil     23-Mar-2007     YREN-3222
'Ashutosh Patil     28-Mar-2007     YREN-3222
'SR                 28-Jul-2009     To choose either american fund or canadian fund.
'Dilip Yadav        22-Sep-2009     YRS 5.0.896 : Re-enable View Details in YRS Banking tab for Retirees 
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Imran              16-June-2010    Enhancement changes(CType to DirectCast)
'Priya              22-Oct-2010     BT:-584,YRS 5.0-1183 : Remove check option from Banking tab
'Shashi Shekhar:    26-Oct-2010     For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Shashi             04 Mar. 2011    Replacing Header formating with user control (YRS 5.0-450 )
'Shashi             13-Apr-2011     For YRS 5.0-877 : Changes to Banking Information maintenance.
'Shashi Shekhar     27-Apr-2011     For BT-584 , Remove check option from Banking tab
'Shashank			08-Jan-2014		For BT:1745\YRS 5.0-1907 - Effective dates of banking records
'Shashank			17-Jan-2014		For BT:1745\YRS 5.0-1907 - Effective dates of banking records
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
'Name:Shubhrata Date:Aug 17th 2006 IssueId: YREN 2638 Reason: 2 bank records with same effective date producing 2 checks 
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class UpdateBankInfo
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateBankInfo.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelBankName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonBanks As System.Web.UI.WebControls.Button
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelBankABANo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxABANumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelAccountNumber As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAccountNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListPaymentMethod As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelAccuntType As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownAccountType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCheckPayment As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents trPartcipant As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents rdoFundtype As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents trFundType As System.Web.UI.HtmlControls.HtmlTableRow

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
        Try

            '----------------------------------------------------------------------------------------------------------------
            'Shashi:04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Retiree Information - Add/View Bank Information"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If
            '----------------------------------------------------------------------------------------------------------------

            Dim g_dataset_dsPaymentMethod As New DataSet
            Dim InsertRowPaymentMethod As DataRow
            Dim g_dataset_dsAccountType As New DataSet
            Dim InsertRowAccountType As DataRow

            'Hafiz 04Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            'Hafiz 04Feb06 Cache-Session
            'Code Added By ashutosh on 06-May-06
            'If Session("SelectBank") Is Nothing Then
            TextBoxEffectiveDate.RequiredDate = True
            'ValidationSummary1.EnableViewState = True
            'ValidationSummary1.EnableClientScript = True
            'ValidationSummary1.ShowSummary = True
            'RequiredFieldValidator2.Visible = True
            'RequiredFieldValidator4.Visible = True
            'RequiredFieldValidator4.
            'RequiredFieldValidator1.Enabled = True
            'RequiredFieldValidator3.Enabled = True
            'ButtonOK.CausesValidation = True
            'Else
            'ValidationSummary1.EnableViewState = False
            'ValidationSummary1.EnableClientScript = False
            'ValidationSummary1.ShowSummary = False
            ' RequiredFieldValidator2.Visible = False
            'RequiredFieldValidator4.Visible = False
            'RequiredFieldValidator1.Enabled = False
            'RequiredFieldValidator3.Enabled = False
            'ButtonOK.CausesValidation = False
            Session("SelectBank") = Nothing
            'End If
            '******************
            If Not IsPostBack Then
                'g_dataset_dsPaymentMethod = YMCARET.YmcaBusinessObject.UpdateBankInformationBOClass.LookUpPaymentMethod()
                ''adding an empty row dynamically
                'InsertRowPaymentMethod = g_dataset_dsPaymentMethod.Tables(0).NewRow()
                'InsertRowPaymentMethod.Item("chvCodevalue") = String.Empty
                'InsertRowPaymentMethod.Item("chvShortDescription") = String.Empty
                'g_dataset_dsPaymentMethod.Tables(0).Rows.Add(InsertRowPaymentMethod)
                'Me.DropDownListPaymentMethod.DataSource = g_dataset_dsPaymentMethod
                'Me.DropDownListPaymentMethod.DataMember = "Payment Method"
                'Me.DropDownListPaymentMethod.DataTextField = "chvShortDescription"
                'Me.DropDownListPaymentMethod.DataValueField = "chvCodevalue"
                'Me.DropDownListPaymentMethod.DataBind()
                'g_dataset_dsPaymentMethod = cache("BankingDtls")
                'Me.DropDownListPaymentMethod.DataSource = g_dataset_dsPaymentMethod
                'Me.DropDownListPaymentMethod.DataTextField = "PaymentDesc"
                'Me.DropDownListPaymentMethod.DataValueField = "PaymentDesc"
                'Me.DropDownListPaymentMethod.DataBind()
                Dim l_string_PersId As String
                l_string_PersId = Session("PersId")
                'Priya 22-Oct-2010: BT:-584,YRS 5.0-1183 : Remove check option from Banking tab
                If DropDownListPaymentMethod.Items.FindByValue("EFT") IsNot Nothing Then
                    DropDownListPaymentMethod.SelectedValue = "EFT"
                End If
                'End 22-Oct-2010
                Dim dsBanks As DataSet
                dsBanks = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBanks(l_string_PersId)
                'Updated by sanjay to select Fund type based on ParticipantType Field
                If dsBanks.Tables(0).Rows.Count > 0 Then
                    'Shashi Shekhar Singh:13-Apr.-2011:For YRS 5.0-877 : Changes to Banking Information maintenance.
                    trFundType.Visible = False

                    '----------Shashi Shekhar Singh:27-04-2011: For BT-584 , Remove check option from Banking tab
                    'If (dsBanks.Tables(0).Rows(0)("ParticipantType").ToString().Trim = "U") Then
                    '    rdoFundtype.SelectedValue = dsBanks.Tables(0).Rows(0)("ParticipantType")
                    '    Me.DropDownListPaymentMethod.Items.Remove(DropDownListPaymentMethod.Items.FindByValue("CHECK"))
                    '    Me.DropDownListPaymentMethod.DataBind()
                    'Else
                    '    Me.DropDownListPaymentMethod.Items.Remove(DropDownListPaymentMethod.Items.FindByValue("CHECK"))
                    '    Me.DropDownListPaymentMethod.DataBind()
                    'End If
                    '------------------------------------------------------------------------------------

                    If Not rdoFundtype.Items.FindByValue(dsBanks.Tables(0).Rows(0)("ParticipantType").ToString().Trim) Is Nothing Then
                        rdoFundtype.SelectedValue = dsBanks.Tables(0).Rows(0)("ParticipantType").ToString.Trim
                    End If
                    Me.rdoFundtype.Enabled = False
                Else
                    'Shashi Shekhar Singh:13-Apr.-2011:For YRS 5.0-877 : Changes to Banking Information maintenance.
                    trFundType.Visible = False
                    rdoFundtype.SelectedValue = "U"
                    Me.rdoFundtype.Enabled = False
                End If
                ' Ends here by sanjay
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
                If Not Request.QueryString("UniqueID") Is Nothing Or Not Request.QueryString("Index") Is Nothing Then
                    TextBoxBankName.Text = Session("R_BankName")
					TextBoxABANumber.Text = IIf(CType(Session("R_BankABANumber"), String).Trim.Equals("&nbsp;"), "", Session("R_BankABANumber")) 'SP : 2014.01.17 BT:1745\YRS 5.0-1907  (added iif to check if &nsbp; then blank else value in session)
					TextBoxAccountNumber.Text = IIf(CType(Session("R_BankAccountNumber"), String).Trim.Equals("&nbsp;"), "", Session("R_BankAccountNumber")) 'SP : 2014.01.17 BT:1745\YRS 5.0-1907  (added iif to check if &nsbp; then blank else value in session)
                    DropDownListPaymentMethod.SelectedValue = CType(Session("R_BankPaymentMethod"), String).ToUpper
                    DropdownAccountType.SelectedValue = Session("R_BankAccountType")
                    TextBoxEffectiveDate.Text = Session("R_BankEffectiveDate")
                    'by Aparna -YREN-3037- 30/01/2007
                    Me.ButtonOK.Visible = False
                    Me.ButtonCancel.Text = "Ok"
                    Me.ButtonBanks.Visible = False
                    Me.DropdownAccountType.Enabled = False
                    Me.DropDownListPaymentMethod.Enabled = False
                    Me.TextBoxEffectiveDate.Enabled = False
                    Me.TextBoxAccountNumber.ReadOnly = True
                    Me.TextBoxBankName.ReadOnly = True
                    Me.TextBoxABANumber.ReadOnly = True

                    'Else
                    '    Me.ButtonOK.Visible = True
                    '    Me.ButtonCancel.Text = "Cancel"
                    '    Me.ButtonBanks.Visible = True
                    '    Me.DropdownAccountType.Enabled = True
                    '    Me.DropDownListPaymentMethod.Enabled = True
                    'by Aparna -YREN-3037- 30/01/2007
                End If
            End If
            If Session("blnRetireeSelectBank") = True Then
                TextBoxBankName.Text = Session("Sel_BankName")
                TextBoxABANumber.Text = Session("Sel_BankABANumber")
                Session("blnRetireeSelectBank") = False
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub ButtonBanks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBanks.Click
        'storing general tab's control's value in session
        Dim msg1 As String = ""
        'commented by hafiz on 15Jun2006
        'Session("blnRetireeSelectBank") = True
        Session("R_BankName") = Me.TextBoxBankName.Text
        Session("R_BankABANumber") = Me.TextBoxABANumber.Text
        Session("R_BankAccountNumber") = Me.TextBoxAccountNumber.Text
        Session("R_BankPaymentMethod") = Me.DropDownListPaymentMethod.SelectedValue
        Session("R_BankAccountType") = Me.DropdownAccountType.SelectedValue
        Session("R_BankEffectiveDate") = Me.TextBoxEffectiveDate.Text

        msg1 = msg1 & "<Script Language='JavaScript'>"
        msg1 = msg1 & "window.open('SelectBank.aspx','SelectBank','width=800, height=500, menubar=no, Resizable=NO,top=80,left=120, scrollbars=yes')"
        msg1 = msg1 & "</script>"
        Page.RegisterStartupScript("PopupScriptNew4", msg1)
    End Sub
    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            'Code added by Shubhrata YREN 2638 Aug 17th 2006
            Dim l_dataset_effdate As DataSet
			Dim l_datatable_effdate As DataTable
			Dim drow As DataRow()
			Dim strMaxEffectiveDate As String 'SP :2014.01.07 : BT:1745\YRS 5.0-1907 -End
			Dim l_date_FirstDateofNextMonth As Date
			Dim l_date_EffDate As Date

            'Ashutosh Patil as on 23-Mar-2007
            'YREN-3222 
            l_date_EffDate = TextBoxEffectiveDate.Text
            l_date_FirstDateofNextMonth = DateAdd(DateInterval.Month, 1, DateValue(Month(Today.Date()) & "/01/" & Year(Today.Date())))
            'Ashutosh Patil as on 23-Mar-2007
            'YREN-3222 
            'Ashutosh Patil as on 28-Mar-2007
            'YREN-3222 
            If l_date_EffDate > l_date_FirstDateofNextMonth Then
                MessageBox.Show(Me.PlaceHolder1, "YMCA YRS", "Effective Date can not be greater than first of the next month.", MessageBoxButtons.Stop)
                Exit Sub
            End If


            If rdoFundtype.SelectedValue Is Nothing Or rdoFundtype.SelectedIndex = -1 Then

                MessageBox.Show(Me.PlaceHolder1, "YMCA YRS", "Please select the Type of funds.", MessageBoxButtons.Stop)
                Exit Sub
            End If


            If Not Session("BankingDtls") Is Nothing Then
                l_dataset_effdate = DirectCast(Session("BankingDtls"), DataSet)
                If l_dataset_effdate.Tables(0).Rows.Count > 0 Then
                    l_datatable_effdate = l_dataset_effdate.Tables(0)
                    If Session("EffDateAddBank") = True Then
						drow = l_datatable_effdate.Select("EffecDate>=#" & CType(Me.TextBoxEffectiveDate.Text, Date) & "#")	 'SP :2014.01.07 : BT:1745\YRS 5.0-1907 - Effective dates of banking records (added > sign select criteria)
						strMaxEffectiveDate = GetMaxEffectiveDate(l_datatable_effdate)
                    ElseIf Session("EffDateUpdateBank") = True Then
                        If Session("R_BankEffectiveDate") <> Me.TextBoxEffectiveDate.Text Then
                            'drow = l_datatable_effdate.Select("BankID='" & Session("Sel_SelectBank_GuiUniqueiD") & "' AND  EffecDate= '" & Me.TextBoxEffectiveDate.Text & "'")
							drow = l_datatable_effdate.Select("EffecDate>=#" & CType(Me.TextBoxEffectiveDate.Text, Date) & "#")	'SP :2014.01.07 : BT:1745\YRS 5.0-1907 - Effective dates of banking records(added > sign select criteria)
							strMaxEffectiveDate = GetMaxEffectiveDate(l_datatable_effdate)
                        End If
                    End If
					If Not drow Is Nothing Then
						If drow.Length > 0 Then
							MessageBox.Show(Me.PlaceHolder1, "YMCA YRS", "Effective Date must be on or after " + strMaxEffectiveDate, MessageBoxButtons.Stop)	'SP :2014.01.07 : BT:1745\YRS 5.0-1907 
							Exit Sub
						End If
					End If
				End If
			End If
			'Code added by Shubhrata YREN 2638 Aug 17th 2006
			ValidationSummary1.Enabled = True

			If Request.QueryString("UniqueID") Is Nothing And Request.QueryString("Index") Is Nothing Then
				Session("blnAddBankingRetirees") = True
			End If

			'End Add
			Dim msg As String

			Dim dsBanking As New DataSet
			Dim drRows() As DataRow

			Dim drUpdated As DataRow

			'Hafiz 04Feb06 Cache-Session
			'Dim l_CacheManager As CacheManager
			'l_CacheManager = CacheFactory.GetCacheManager
			'Hafiz 04Feb06 Cache-Session


			Session("BankEffectiveDate") = Me.TextBoxEffectiveDate.Text
			Session("BankPaymentMethod") = Me.DropDownListPaymentMethod.SelectedValue

			Session("SelectBank_BankName") = Me.TextBoxBankName.Text
			Session("SelectBank_BankABANumber") = Me.TextBoxABANumber.Text
			Session("BankAccountNumber") = Me.TextBoxAccountNumber.Text

			If Session("BankPaymentMethod") = "CHECK" Then
				Session("SelectBank_BankName") = "CHECK"
				Session("SelectBank_BankABANumber") = "N/A"
				Session("BankAccountNumber") = "N/A"
				Me.TextBoxBankName.Text = "CHECK"
				Me.TextBoxABANumber.Text = "N/A"
				Me.TextBoxAccountNumber.Text = "N/A"
			End If

			Session("BankAccountType") = Me.DropdownAccountType.SelectedValue
			Session("BankAccountTypeText") = Me.DropdownAccountType.SelectedItem.Text 'Added by Dilip yadav : 22.09.2009
			Session("ParticipantType") = rdoFundtype.SelectedValue	 'Added by sanjay for Fund Type on 28-Jul-09

			If Not Request.QueryString("UniqueID") Is Nothing Then

				'Hafiz 04Feb06 Cache-Session
				'dsBanking = CType(l_CacheManager.GetData("BankingDtls"), DataSet)
				dsBanking = DirectCast(Session("BankingDtls"), DataSet)
				'Hafiz 04Feb06 Cache-Session

				If Not IsNothing(dsBanking) Then
					'drRows = dsBanking.Tables(0).Select("BankID='" & Request.QueryString("UniqueID") & "'")
					'YREN 2546
					'comented & added by hafiz on 9Aug2006 - YREN-2591
					'drRows = dsBanking.Tables(0).Select("BankID='" & Request.QueryString("UniqueID") & "'")
					drRows = dsBanking.Tables(0).Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
					drUpdated = drRows(0)
					drUpdated("BankName") = Me.TextBoxBankName.Text
					drUpdated("BankABA#") = Me.TextBoxABANumber.Text
					drUpdated("AccountNo") = Me.TextBoxAccountNumber.Text
					drUpdated("EffecDate") = Me.TextBoxEffectiveDate.Text
					drUpdated("PaymentDesc") = Me.DropDownListPaymentMethod.SelectedValue
					drUpdated("EFTDesc") = Me.DropdownAccountType.SelectedValue
					drUpdated("PersonID") = Session("PersId")
					'Added By Ashutosh Patil for YERN-2979
					'Start Ashutosh Patil
					drUpdated("dtmEffDate") = Me.TextBoxEffectiveDate.Text
					'End Ashutosh Patil

					'drUpdated("BankID") = Request.QueryString("UniqueID")
					'drUpdated.AcceptChanges()
					'Adding guiBankId to update the bank name in the database---YREN 2546
					drUpdated("BankID") = Session("Sel_SelectBank_GuiUniqueiD")
					Session("blnUpdateBankingRetirees") = True

					Session("EnableSaveCancel") = True

					'Hafiz 04Feb06 Cache-Session
					'l_CacheManager.Add("BankingDtls", dsBanking)
					Session("BankingDtls") = dsBanking
					'Hafiz 04Feb06 Cache-Session
				End If
			End If

			If Not Request.QueryString("Index") Is Nothing Then

				'Hafiz 04Feb06 Cache-Session
				'dsBanking = CType(l_CacheManager.GetData("BankingDtls"), DataSet)
				dsBanking = DirectCast(Session("BankingDtls"), DataSet)
				'Hafiz 04Feb06 Cache-Session

				If Not IsNothing(dsBanking) Then

					drUpdated = dsBanking.Tables(0).Rows(Request.QueryString("Index"))
					drUpdated("BankName") = Me.TextBoxBankName.Text
					drUpdated("BankABA#") = Me.TextBoxABANumber.Text
					drUpdated("AccountNo") = Me.TextBoxAccountNumber.Text
					drUpdated("EffecDate") = Me.TextBoxEffectiveDate.Text
					drUpdated("PaymentDesc") = Me.DropDownListPaymentMethod.SelectedValue
					drUpdated("EFTDesc") = Me.DropdownAccountType.SelectedValue
					drUpdated("PersonID") = Session("PersId")
					'drUpdated("BankID") = Request.QueryString("UniqueID")
					'drUpdated("BankID") = System.DBNull.Value
					'Adding guiBankId to update the bank name in the database---YREN 2546
					drUpdated("BankID") = Session("Sel_SelectBank_GuiUniqueiD")
					'Added By Ashutosh Patil for YERN-2979 as on 24-Jan-2007
					drUpdated("dtmEffDate") = Me.TextBoxEffectiveDate.Text
					Session("EnableSaveCancel") = True

					Session("blnUpdateBankingRetirees") = True
					'Hafiz 04Feb06 Cache-Session
					'l_CacheManager.Add("BankingDtls", dsBanking)
					Session("BankingDtls") = dsBanking
					'Hafiz 04Feb06 Cache-Session
				End If


			End If




			msg = msg + "<Script Language='JavaScript'>"

			msg = msg + "window.opener.document.forms(0).submit();"

			msg = msg + "self.close();"

			msg = msg + "</Script>"
			Response.Write(msg)
		Catch ex As Exception
			Throw

		End Try


    End Sub

    Private Sub ButtonCheckPayment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCheckPayment.Click
        Try
            If Request.QueryString("UniqueID") Is Nothing And Request.QueryString("Index") Is Nothing Then
                Session("blnAddBankingRetirees") = True
            End If

            'End Add
            Dim msg As String

            Dim dsBanking As New DataSet
            Dim drRows() As DataRow

            Dim drUpdated As DataRow

            'Hafiz 04Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager
            'Hafiz 04Feb06 Cache-Session

            Session("SelectBank_BankName") = Me.TextBoxBankName.Text
            Session("SelectBank_BankABANumber") = Me.TextBoxABANumber.Text
            Session("BankAccountNumber") = Me.TextBoxAccountNumber.Text
            Session("BankEffectiveDate") = Me.TextBoxEffectiveDate.Text
            Session("BankPaymentMethod") = Me.DropDownListPaymentMethod.SelectedValue
            Session("BankAccountType") = Me.DropdownAccountType.SelectedValue
            Session("BankAccountTypeText") = Me.DropdownAccountType.SelectedItem.Text 'Added by Dilip Yadav : 22.09.2009


            If Not Request.QueryString("UniqueID") Is Nothing Then

                'Hafiz 04Feb06 Cache-Session
                'dsBanking = CType(l_CacheManager.GetData("BankingDtls"), DataSet)
                dsBanking = DirectCast(Session("BankingDtls"), DataSet)
                'Hafiz 04Feb06 Cache-Session

                If Not IsNothing(dsBanking) Then
                    'drRows = dsBanking.Tables(0).Select("BankID='" & Request.QueryString("UniqueID") & "'")
                    'YREN 2546
                    drRows = dsBanking.Tables(0).Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
                    drUpdated = drRows(0)
                    drUpdated("BankName") = Me.TextBoxBankName.Text
                    drUpdated("BankABA#") = Me.TextBoxABANumber.Text
                    drUpdated("AccountNo") = Me.TextBoxAccountNumber.Text
                    drUpdated("EffecDate") = Me.TextBoxEffectiveDate.Text
                    drUpdated("PaymentDesc") = "Check"
                    drUpdated("EFTDesc") = Me.DropdownAccountType.SelectedValue
                    drUpdated("PersonID") = Session("PersId")
                    drUpdated("BankID") = Request.QueryString("UniqueID")
                    'Adding guiBankId to update the bank name in the database---YREN 2546
                    drUpdated("BankID") = Session("Sel_SelectBank_GuiUniqueiD")
                    Session("EnableSaveCancel") = True

                    Session("blnUpdateBankingRetirees") = True
                    'Hafiz 04Feb06 Cache-Session
                    'l_CacheManager.Add("BankingDtls", dsBanking)
                    Session("BankingDtls") = dsBanking
                    'Hafiz 04Feb06 Cache-Session
                End If
            End If

            If Not Request.QueryString("Index") Is Nothing Then

                'Hafiz 04Feb06 Cache-Session
                'dsBanking = CType(l_CacheManager.GetData("BankingDtls"), DataSet)
                dsBanking = DirectCast(Session("BankingDtls"), DataSet)
                'Hafiz 04Feb06 Cache-Session

                If Not IsNothing(dsBanking) Then

                    drUpdated = dsBanking.Tables(0).Rows(Request.QueryString("Index"))
                    drUpdated("BankName") = Me.TextBoxBankName.Text
                    drUpdated("BankABA#") = Me.TextBoxABANumber.Text
                    drUpdated("AccountNo") = Me.TextBoxAccountNumber.Text
                    drUpdated("EffecDate") = Me.TextBoxEffectiveDate.Text
                    drUpdated("PaymentDesc") = "Check"
                    drUpdated("EFTDesc") = Me.DropdownAccountType.SelectedValue
                    drUpdated("PersonID") = Session("PersId")
                    'drUpdated("BankID") = Request.QueryString("UniqueID")
                    Session("EnableSaveCancel") = True

                    Session("blnUpdateBankingRetirees") = True
                    'Hafiz 04Feb06 Cache-Session
                    'l_CacheManager.Add("BankingDtls", dsBanking)
                    Session("BankingDtls") = dsBanking
                    'Hafiz 04Feb06 Cache-Session
                End If


            End If

            msg = msg + "<Script Language='JavaScript'>"

            ' msg = msg + "window.opener.location.href=window.opener.location.href;"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            'Code Added by ashutosh on 06-May-06***********

            Session("SelectBank") = Nothing
            '*************End Ashutosh Code********
            Response.Write(msg)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Session("EffDateAddBank") = Nothing
            Session("EffDateUpdateBank") = Nothing
            Session("R_BankEffectiveDate") = Nothing
            Dim msg As String

            Session("blnUpdateBankingRetirees") = False

            Session("blnAddBankingRetirees") = False

            msg = msg + "<Script Language='JavaScript'>"

            'msg = msg + "window.opener.location.href=window.opener.location.href;"

            msg = msg + "self.close();"

            msg = msg + "</Script>"

            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

	'SP :2014.01.07 : BT:1745\YRS 5.0-1907 - Effective dates of banking records -Start
	'This method find the max effective date.
	Private Function GetMaxEffectiveDate(ByVal dtBankingRecords As DataTable) As String
		Dim strMaxEffectiveDate As String
		Try
			strMaxEffectiveDate = String.Empty
			If HelperFunctions.isNonEmpty(dtBankingRecords) Then
				strMaxEffectiveDate = CType(dtBankingRecords.Compute("Max(EffecDate)", Nothing), Date).AddDays(1).ToShortDateString()	'dtBankingRecords.AsEnumerable().Max(Function(w) w("EffecDate"))	
			End If
		Catch
			Throw
		End Try
		Return strMaxEffectiveDate
	End Function
	'SP :2014.01.07 : BT:1745\YRS 5.0-1907 - Effective dates of banking records -End
End Class
