'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	BeneficiarySettlementForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 3:15:06 PM
' Program Specification Name	:	YMCA PS 3.14.1.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	
' Changed on			:	
' Change Description	:	
'*******************************************************************************
' Cache-Session         :   Vipul 02Feb06 
'*******************************************************************************
'Changed By:preeti On:9thFeb06 IssueId:YRST-2092
'*******************************************************************************
'****************************************************
'Modification History
'****************************************************
'Modified by            Date            Description
'****************************************************
'Aparna Samala          06/03/2007      YREN-3015
'NP                     2007.07.03      Trying to rewrite the whole code for settlement for plan split
'NP                     2007.08.20      Splitting the initialization into two lines. This should fix bug reported in bug tracker as 
'NP                     2007.08.24      Bug 88. Benefit Options being hidden for Retired participants on selection.
'NP                     2007.08.31      Updated code to allow 11 characters in the SSN field
'NP                     2007.08.31      Updated code to handle the proper display of selected image in the selection button for beneficiaries. This only worked partially. Changing code to do the selection the right way - Look at 2007.09.13.
'NP                     2007.09.06      Updated code to allow searches on Fund No
'NP                     2007.09.12      Fixing issue 175
'NP                     2007.09.13      Updating code to ensure the selection of the right radio button using the correct way. Removed changes due to 2007.08.31.
'NP                     2007.09.18      Updating code to ensure the correct selection of decedent record and to sort records properly for the search results
'NP                     2007.11.07      Adding check to ensure there were options to select from the Settlement options and then ensure that one was selected from among them.
'NP                     2008.01.23      YRPS-4009 Changing code to perform basic validations before beginning the settlement process
'Priya                  2008.10.05      Disable settlement button if no options are available 
'Nikunj Patel           2009.04.20      Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Neeraj Singh           2009.11.12      Added form name for security issue YRS 5.0-940 
'Shashi Shekhar         2010.02.16      Restrict Data Archived Participants To proceed in Find list.
'Sanjay R.              2010.06.18      Enhancement changes(CType to DirectCast)
'Sanjay R.              2010.07.12      Code Review changes.(Region,variable Decalration etc.) 
'Shashi Shekhar         28 Feb 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Sanjay                 2011.04.27      YRS 5.0-1292,DeathNotify funtion is called for validating retired beneficiaries.
'Dinesh K               2002.12.07      BT - 1433 : Problem in Death Settlement.
'Shashank Patel			2013.04.12		YRS 5.0-1990:similar SSNs are being updates across the board
'Shashank Patel         2014.04.02      BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death 
'Sanjay R               2014.05.06      YRS 5.0-2188: RMDs for Beneficieries
'Sanjay R               2014.07.09      BT 2593 - UI changes in Beneficiary information page
'Sanjay R.              2014.07.25      BT 2615:Second Beneficiary get settled automatically after settling first Beneficiary
'Manthan Rajguru        2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru         2016.04.22      YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1
'Manthan Rajguru        2016.05.20      YRS-AT-2270 -  withholding amount was doubled (Death Beneficiary/Annuity)
'Santosh Bura			2016.10.13 		YRS-AT-3095 -  YRS enh-allow regenerate RMD for deceased participants (TrackIT 27024)  
'Santosh Bura			2016.11.25 		YRS-AT-3022 -  YRS enhancement.--YRS death settlement screen.Track it 26636
'Santosh Bura           2017.07.28      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Santosh Bura           2018.01.11      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'****************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports YMCAUI.SessionManager

Public Class BeneficiarySettlementForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("BeneficiarySettlementForm.aspx")
    'End issue id YRS 5.0-940


#Region "EnumMaxlength"
    Public Enum EnumMaxlength
        SSNo = 11   'NP:PS:2007.08.31 - Changing value from 9 to 11
        FirstName = 20
        LastName = 30
        FundNo = 30 'NP:PS:2007.09.06 - Adding Fund value
    End Enum
#End Region

    Protected WithEvents RadioButton1 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ButtonRefund As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox

    'Dim g_DeathFundEventStatus As String 'Preeti - death changes
    'Dim g_Relation As String
    Dim g_DeathDate As DateTime 'Preeti - death changes
    Dim g_l_string_BeneficiaryID As String 'Changed By Preeti June 7th
    Dim l_string_returnStatusDA As String = ""
    Dim l_string_returnStatusDR As String = ""



#Region "Local Variables"
    Dim l_dataset_SearchResults As DataSet      'Store search results information of the search that was performed
    Dim l_dataset_ActiveBeneficiaries As DataSet    'Store any active beneficiaries of the deceased participant
    Dim l_dataset_RetiredBeneficiaries As DataSet   'Store any retired beneficiaries of the deceased participant
    Dim l_dataset_ActiveSettlementOption_RetirementPlan As DataSet  'Store Retirement Plan Settlement Options for Active Beneficiaries
    Dim l_dataset_ActiveSettlementOption_SavingsPlan As DataSet     'Store Savings Plan Settlement Options for Active Beneficiaries
    Dim l_dataset_RetiredSettlementOption_RetirementPlan As DataSet 'Store Retirement Plan Settlement Options for Retired Beneficiaries
    Dim l_dataset_RetiredSettlementOption_SavingsPlan As DataSet    'Store Savings Plan Settlement Options for Retired Beneficiaries
#End Region
#Region "Local Constants"
    Const SELECTED_IMAGE_BUTTON_URL = "images\selected.gif"
    Const NORMAL_IMAGE_BUTTON_URL = "images\select.gif"
    Const ACTIVE_RETIREMENT_OPTIONS_RADIO_CONTROL_ID = "RadioLabel_Active_RP"
    Const ACTIVE_SAVINGS_OPTIONS_RADIO_CONTROL_ID = "RadioLabel_Active_SP"
    Const RETIRED_RETIREMENT_OPTIONS_RADIO_CONTROL_ID = "RadioLabel_Retired_RP"
    Const RETIRED_SAVINGS_OPTIONS_RADIO_CONTROL_ID = "RadioLabel_Retired_SP"
    Const ACTIVE_BENEFICIARY_TAB_INDEX = 1
    Const RETIRED_BENEFICIARY_TAB_INDEX = 2
    Const LINEBREAK = "<BR />" & vbCrLf
    Const PARTICIPANT_LIST = 0  ' SB | 2017.10.12 | YRS-AT-3324 | Constant declaration 
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnDelete As System.Web.UI.WebControls.Button
    Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents btnCancel As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrint As System.Web.UI.WebControls.Button
    Protected WithEvents btnOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents TabStripBeneficiarySettlement As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageBeneficiarySettlement As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents DataGridSearchResults As System.Web.UI.WebControls.DataGrid

    Protected WithEvents LabelActiveBeneficiaries As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridActiveBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelActiveBenefitOptions_RetirementPlan As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridActiveBenefitOptions_RetirementPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelActiveBenefitOptions_SavingsPlan As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridActiveBenefitOptions_SavingsPlan As System.Web.UI.WebControls.DataGrid

    Protected WithEvents LabelRetiredBeneficiaries As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridRetiredBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelRetiredBenefitOptions_RetirementPlan As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridRetiredBenefitOptions_SavingsPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelRetiredBenefitOptions_SavingsPlan As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridRetiredBenefitOptions_RetirementPlan As System.Web.UI.WebControls.DataGrid

    Protected WithEvents LabelLookFor As System.Web.UI.WebControls.Label

    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLookFor As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSettleRetiredBeneficiary As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSettleActiveBeneficiary As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents DivMessage As System.Web.UI.HtmlControls.HtmlGenericControl ' SB | 10/13/2016 | YRS-AT-3095 |  Div added to display warning message for if RMD need to be regenerated for deceased participant
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    ' START : SB | 10/13/2016 | YRS-AT-3095 |  Property to set RMD regenerate true or false and set guiFundEventId
    Public Property RegenerateRMD() As Boolean     ' We are setting this flag to know the backdated termination of the participant took place so we need to regenerate RMD for such particpants
        Get
            If Not (ViewState("RegenerateRMD")) Is Nothing Then
                Return (CType(ViewState("RegenerateRMD"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("RegenerateRMD") = Value
        End Set
    End Property

    Public Property FundEventID() As String     ' Storing guiUniqueFundId of the selected participant
        Get
            If Not (ViewState("FundEventID")) Is Nothing Then
                Return (CType(ViewState("FundEventID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("FundEventID") = Value
        End Set
    End Property
    ' END : SB | 10/13/2016 | YRS-AT-3095 |  Property to set guiFundEventId of the participant and set guiFundEventId

    'START: SB | 2017.08.02 | YRS-AT-3324 | Added property that will return the reason for restriction of Death Settlement for RMD eligible participants whose RMD is not generated
    Private Property ReasonForRestriction() As String
        Get
            If Not ViewState("ReasonForRestriction") Is Nothing Then
                Return (CType(ViewState("ReasonForRestriction"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("ReasonForRestriction") = Value
        End Set
    End Property


    ' Added property that will vaildate Death Settlement is allowed or not for RMD eligible participants 
    Public Property IsDeathSettlementAllowedForRMDEligibleParticipants() As Boolean
        Get
            If ViewState("DeathSettlementAllowedForRMDEligibleParticipants") Is Nothing Then
                'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Check the death date is valid and if not valid by default date is initialized and additional parameter is added to function for checking deceased participants mrd records uptill deceased year
                'ViewState("DeathSettlementAllowedForRMDEligibleParticipants") = Validation.IsRMDExist(YMCAObjects.Module.Death_Settlement, FundEventID, Me.ReasonForRestriction)
                Dim deceasedDate As String
                deceasedDate = g_DeathDate
                If String.IsNullOrEmpty(deceasedDate) Then
                    deceasedDate = ""
                End If
                ViewState("DeathSettlementAllowedForRMDEligibleParticipants") = Validation.IsRMDExist(YMCAObjects.Module.Death_Settlement, FundEventID, Me.ReasonForRestriction, deceasedDate)
                'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Check the death date is valid and if not valid by default date is initialized and additional parameter is added to function for checking deceased participants mrd records uptill deceased year
            End If
            Return CType(ViewState("DeathSettlementAllowedForRMDEligibleParticipants"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("DeathSettlementAllowedForRMDEligibleParticipants") = Value
        End Set
    End Property
    'END: SB | 2017.08.02 | YRS-AT-3324 |  Added property that will return the reason for restriction of Death Settlement for RMD eligible participants whose RMD is not generated


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            'Session("CurrentForm") - This variable stores the name of the form that was last used to enter data
            'Session("Success_BIScreen")    - This variable stores the status of the BIScreen form during data entry
            'Session("Success_BRScreen")    - This variable stores the status of the BRScreen form during data entry
            If (Page.IsPostBack = False) Then
                'If this is the first time the page is being loaded then
                Session("CurrentForm") = "BS"
                TextBoxFundNo.MaxLength = EnumMaxlength.FundNo
                TextBoxSSNo.MaxLength = EnumMaxlength.SSNo
                TextBoxFirstName.MaxLength = EnumMaxlength.FirstName
                TextBoxLastName.MaxLength = EnumMaxlength.LastName
                TabStripBeneficiarySettlement.Items(ACTIVE_BENEFICIARY_TAB_INDEX).Enabled = False    'Disable the Active Beneficiaries Tab
                TabStripBeneficiarySettlement.Items(RETIRED_BENEFICIARY_TAB_INDEX).Enabled = False   'Disable the Retired Beneficiaries Tab
                ButtonPrint.Enabled = False
                'Start-SR:2014.05.23- Beneficiary RMDs
                SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
                SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
                'Session("RMD_PersStatusType") = Nothing
                SessionBeneficiaryRMDs.DeceasedFundStatus = Nothing
                'End-SR:2014.05.23- Beneficiary RMDs
            Else
                'This is a post-back and we are coming back the second time
                If Session("CurrentForm") = "BI" Then
                    If Session("Success_BIScreen") = True Then
                        'We have come from the BI screen and BI was successful then call Refund Screen
                        Call_RefundBeneficiaryForm()
                    Else
                        'We have come from the BI screen but BI was not successful. Simply display current page and an error message
                        Session("CurrentForm") = "BS"
                        'NP:PS:2007.07.03 - BindBeneficiaryDetailsPages()
                    End If
                ElseIf Session("CurrentForm") = "BR" Then
                    'Display Process Status & Reset Session("CurrentForm")
                    If Session("Success_BRScreen") = True Then
                        'We have come back from the refund form and it was successful. Process any settlement that we need to perform.
                        Update_FinalSettlement()
                    Else
                        'We came from the BR screen but BR was not successful. Simply display the current page and an error message
                        Session("CurrentForm") = "BS"
                        'NP:PS:2007.07.03 - BindBeneficiaryDetailsPages()
                    End If
                End If
            End If

            'This part of the code deals with message prompting
            'The yes/no options are replies for the message prompting for confirmation of beneficiary settlement
            'The ok option is reply for any error messages or informational messages given to the user.
            If Request.Form("Yes") = "Yes" Then
                'If the user confirms that he/she wishes to settle beneficiary, then we need to call the BeneficiaryInformation Form, which collects information about the beneficiary
                CallBeneficiaryInformationForm()
            ElseIf Request.Form("No") = "No" Then
                'NP:PS:2007.07.03 - BindBeneficiaryDetailsPages()
            ElseIf Request.Form("OK") = "OK" Then
                'NP:PS:2007.07.03 - BindBeneficiaryDetailsPages()
            End If

            'START: SB | 2017.08.02 | YRS-AT-3324 | Added method to validate Death Settlement restricitions for RMD eligbile participants
            If HelperFunctions.isNonEmpty(FundEventID) Then
                If Not Me.IsDeathSettlementAllowedForRMDEligibleParticipants Then
                    ShowRegenerateRMDWarning()
                    Exit Sub
                End If
            End If
            'END: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate Death Settlement restricitions for RMD eligbile participants

            ' START : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant need to regenerate or not if yes then display warning message
            If (RegenerateRMD) Then
                ShowRegenerateRMDWarning()
            End If
            ' END : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant need to regenerate or not if yes then display warning message
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
        System.Diagnostics.Trace.WriteLine(getDebuggingInformation())
    End Sub
    Private Function getDebuggingInformation() As String
        Dim msg As String = String.Empty

        Try
            If Me.MultiPageBeneficiarySettlement.SelectedIndex = Me.ACTIVE_BENEFICIARY_TAB_INDEX Then
                'Get Beneficiary Id
                msg &= "Beneficiary Id: " & Me.l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString() & LINEBREAK
                'Get first Option Id
                msg &= "Retirement Plan Option: "
                If HelperFunctions.isNonEmpty(Me.l_dataset_ActiveSettlementOption_RetirementPlan) Then
                    Me.l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView.RowFilter = "BeneficiaryId='" & Me.l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString() & "'"
                    If HelperFunctions.isNonEmpty(Me.l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView) Then
                        msg &= l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView.Item(Me.DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex)("UniqueId").ToString()
                    End If
                End If
                msg &= LINEBREAK
                'Get second Option Id
                msg &= "Savings Plan Option: "
                If HelperFunctions.isNonEmpty(Me.l_dataset_ActiveSettlementOption_SavingsPlan) Then
                    Me.l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView.RowFilter = "BeneficiaryId='" & Me.l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString() & "'"
                    If HelperFunctions.isNonEmpty(Me.l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView) Then
                        msg &= l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView.Item(Me.DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex)("UniqueId").ToString()
                    End If
                End If
                msg &= LINEBREAK
            End If
            If Me.MultiPageBeneficiarySettlement.SelectedIndex = Me.RETIRED_BENEFICIARY_TAB_INDEX Then
                'Get Beneficiary Id
                msg &= "Beneficiary Id: " & Me.l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString() & LINEBREAK
                'Get first Option Id
                msg &= "Retirement Plan Option: "
                If HelperFunctions.isNonEmpty(Me.l_dataset_RetiredSettlementOption_RetirementPlan) Then
                    Me.l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView.RowFilter = "BeneficiaryId='" & Me.l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString() & "'"
                    If HelperFunctions.isNonEmpty(Me.l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView) Then
                        msg &= l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView.Item(Me.DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex)("UniqueId").ToString()
                    End If
                Else
                    msg &= "NOTHING"
                End If
                msg &= LINEBREAK
                'Get second Option Id
                msg &= "Savings Plan Option: "
                If HelperFunctions.isNonEmpty(Me.l_dataset_RetiredSettlementOption_SavingsPlan) Then
                    Me.l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView.RowFilter = "BeneficiaryId='" & Me.l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString() & "'"
                    If HelperFunctions.isNonEmpty(Me.l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView) Then
                        msg &= l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView.Item(Me.DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex)("UniqueId").ToString()
                    End If
                Else
                    msg &= "NOTHING"
                End If
                msg &= LINEBREAK
            End If

            msg &= LINEBREAK
            msg &= "Information for BI screen" + LINEBREAK
            msg &= getStringFromSessionForDebugging("BS_SelectedOption_RP")
            msg &= getStringFromSessionForDebugging("BS_SelectedOption_SP")

            msg &= "Information from BI screen" + LINEBREAK
            msg &= getStringFromSessionForDebugging("Success_BIScreen")
            msg &= getStringFromSessionForDebugging("SP_Parameters_DeathBenefitOptionID")
            msg &= getStringFromSessionForDebugging("SP_Parameters_AnnuityOption")

            msg &= getStringFromSessionForDebugging("SP_Parameters_DeathBenefitOptionID_SP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_AnnuityOption_SP")

            msg &= LINEBREAK
            msg &= "Information for BR screen" + LINEBREAK
            msg &= getStringFromSessionForDebugging("BS_SelectedOption_RP")
            msg &= getStringFromSessionForDebugging("BS_SelectedOption_SP")

            msg &= "Information from BR screen" + LINEBREAK
            msg &= getStringFromSessionForDebugging("Success_BRScreen")
            msg &= getStringFromSessionForDebugging("SP_Parameters_RolloverTaxable_RP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_RolloverNonTaxable_RP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_WithholdingPct_RP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_RolloverInstitutionID_RP")

            msg &= getStringFromSessionForDebugging("SP_Parameters_RolloverTaxable_SP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_RolloverNonTaxable_SP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_WithholdingPct_SP")
            msg &= getStringFromSessionForDebugging("SP_Parameters_RolloverInstitutionID_SP")
        Catch ex As Exception
            msg &= LINEBREAK + "Error in getDebuggingInformation: " & ex.Message & ex.StackTrace()
        End Try
        Return msg
    End Function
    Private Function getStringFromSessionForDebugging(ByVal key As String) As String
        If Session(key) Is Nothing Then Return "Session(" & key & ")=NOTHING" & LINEBREAK
        If Session(key).ToString() = "" Then Return "Session(" & key & ")=EMPTY" & LINEBREAK
        Return "Session(" & key & ")=" & Session(key) & LINEBREAK
    End Function
    Private Sub TabStripBeneficiarySettlement_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripBeneficiarySettlement.SelectedIndexChange
        MultiPageBeneficiarySettlement.SelectedIndex = TabStripBeneficiarySettlement.SelectedIndex

        'START: SB | 2017.10.12 | YRS-AT-3324 | On selection of list tab, clear and hide the div message along with clearing the viewstate object
        If (TabStripBeneficiarySettlement.SelectedIndex = PARTICIPANT_LIST) Then
            DivMessage.InnerHtml = ""
            DivMessage.Attributes.Add("display", "none")
            ' ClearSessions()  'SB | 2017.12.07 | YRS-AT-3324 | clear session event is handled in clear button click
        End If
        'END: SB | 2017.10.12 | YRS-AT-3324 | On selection of list tab, clear and hide the div message along with clearing the viewstate object

    End Sub

#Region "Search screen related code"
    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        'Perform Validations

        TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")    'Feature: User can put SSN in 222-22-2222 format also
        If TextBoxSSNo.Text.Trim = "" AndAlso TextBoxLastName.Text.Trim = "" AndAlso TextBoxFirstName.Text.Trim = "" AndAlso TextBoxFundNo.Text.Trim = "" Then

            BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
            MessageBox.Show(PlaceHolder1, "Beneficiary Settlement ", "Please Enter a Search Value. ", MessageBoxButtons.Stop)
            Exit Sub
        End If

        Try
            l_dataset_SearchResults = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_MemberListForDeceased(TextBoxSSNo.Text.Trim, TextBoxLastName.Text.Trim, TextBoxFirstName.Text.Trim, TextBoxFundNo.Text.Trim)
            'Check if any results were returned??
            If HelperFunctions.isEmpty(l_dataset_SearchResults) Then
                'Changed By:preeti On:9thFeb06 IssueId:YRST-2092
                Me.DataGridSearchResults.AllowSorting = False
                Me.TabStripBeneficiarySettlement.Items(1).Enabled = False
                Me.TabStripBeneficiarySettlement.Items(2).Enabled = False
                BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
                MessageBox.Show(PlaceHolder1, "Beneficiary Settlement ", "No Records Found. ", MessageBoxButtons.Stop)
                Exit Sub
            End If
            TabStripBeneficiarySettlement.Items(1).Enabled = False  'Default - cannot select
            TabStripBeneficiarySettlement.Items(2).Enabled = False  'Default - cannot select
            CleanUpExistingBenefitOptions()     'Default - Cleanup
            'NP:PS:2007.09.13 - Not selecting the first result by default. - DataGridSearchResults.SelectedIndex = 0
            BindGrid(DataGridSearchResults, l_dataset_SearchResults)
            'NP:PS:2007.09.13 - Not selecting the first result by default. - DataGridSearchResults_SelectedIndexChanged(DataGridSearchResults, Nothing)
            'NP:PS:2007.07.03 - Do not need this here. It should be handled by the selected Index changed event. Process_BeneficiaryDetails()
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub
    Private Sub DataGridSearchResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridSearchResults.SelectedIndexChanged
        Try

            'Clear Session
            'DK:2012.12.07  - BT - 1433 : Problem in Death Settlement.
            ClearSessions()

            '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
            'Shashi Shekhar:2010-04-08:Handling usability issue of Data Archive
            Dim l_SSNo As String
            Dim dr As DataRow()
            l_SSNo = Me.DataGridSearchResults.SelectedItem.Cells(1).Text.Trim
            dr = l_dataset_SearchResults.Tables(0).Select("[SS No]= '" & l_SSNo & "'")

            FundEventID = Convert.ToString(dr(0).Item("FundEventId"))  ' SB | 10/13/2016 | YRS-AT-3095 |  Set guiFundEventId of the participant

            ' If Me.DataGrid_Search.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" And Me.DataGrid_Search.SelectedItem.Cells(7).Text.ToUpper.Trim() <> "RD" Then
            If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "RD")) Then
                'Shashi Shekhar:2010-04-08: Adding Stauts "DR" also with "RD" for Handling usability issue of Data Archive
                If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "DR")) Then
                    ' l_dataset_SearchResults()
                    MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    TabStripBeneficiarySettlement.Items(ACTIVE_BENEFICIARY_TAB_INDEX).Enabled = False    'Disable the Active Beneficiaries Tab
                    TabStripBeneficiarySettlement.Items(RETIRED_BENEFICIARY_TAB_INDEX).Enabled = False   'Disable the Retired Beneficiaries Tab

                    Exit Sub
                End If
            End If
            '---------------------------------------------------------------------------------------




            ''----Shashi Shekhar:2010-02-16: Code to handle Archived Participants from list--------------
            'If Me.DataGridSearchResults.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
            '    MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
            '    TabStripBeneficiarySettlement.Items(ACTIVE_BENEFICIARY_TAB_INDEX).Enabled = False    'Disable the Active Beneficiaries Tab
            '    TabStripBeneficiarySettlement.Items(RETIRED_BENEFICIARY_TAB_INDEX).Enabled = False   'Disable the Retired Beneficiaries Tab
            '    Me.LabelTitle.Text = ""
            '    Exit Sub
            'End If
            ''---------------------------------------------------------------------------------------



            CleanUpExistingBenefitOptions()
            Process_BeneficiaryDetails()
            'If the user has clicked only then we take the user to the selected tab. This can be found by looking at the EventArgs value. If this routine is called from the code, that value would be null
            'Check if only one tab is enabled and if so then take the user there
            If Not e Is Nothing AndAlso TabStripBeneficiarySettlement.Items(1).Enabled = True AndAlso TabStripBeneficiarySettlement.Items(2).Enabled = False Then
                'Only first tab is enabled. Change the display to the first tab
                TabStripBeneficiarySettlement.SelectedIndex = 1
                TabStripBeneficiarySettlement_SelectedIndexChange(TabStripBeneficiarySettlement, Nothing)
            ElseIf Not e Is Nothing AndAlso TabStripBeneficiarySettlement.Items(1).Enabled = False AndAlso TabStripBeneficiarySettlement.Items(2).Enabled = True Then
                'Only second tab is enabled. Change the display to the second tab
                TabStripBeneficiarySettlement.SelectedIndex = 2
                TabStripBeneficiarySettlement_SelectedIndexChange(TabStripBeneficiarySettlement, Nothing)
            End If
            'NP:PS:2007.09.18 - Changing code to Handle display of the grid properly after sorting
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                BindGrid(DataGridSearchResults, l_dataset_SearchResults.Tables(0).DefaultView)
            Else
                BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
            End If
            'Start: SR:2014.05.07- RMDs for Beneficiaries
            'Session("RMD_PersStatusType") = dr(0).Item("StatusType").trim
            SessionBeneficiaryRMDs.DeceasedFundStatus = dr(0).Item("StatusType").trim

            'End: SR:2014.05.07- RMDs for Beneficiaries

            'START: SB | 2017.08.02 | YRS-AT-3324 | Check if Death settlement is allowed for RMD eligbile participants whose RMD is not generated
            If HelperFunctions.isNonEmpty(FundEventID) Then
                If Not Me.IsDeathSettlementAllowedForRMDEligibleParticipants Then
                    ShowRegenerateRMDWarning()
                Else
                    DivMessage.Attributes.Add("display", "none")
                End If
            End If
            'END: SB | 2017.08.02 | YRS-AT-3324 | Check if Death settlement is allowed for RMD eligbile participants whose RMD is not generated

            ' START : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant and set value for RegenerateRMD variable and display warning message if value is true
            CheckPreviousYearRMDStatus()
            If (RegenerateRMD) Then
                ShowRegenerateRMDWarning()
            Else
                DivMessage.Attributes.Add("display", "none")
            End If
            ' END : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant and set value for RegenerateRMD variable and display warning message if value is true
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'NP:PS:2007.09.18 - Handling the Sorting and selection of results properly
    Private Sub DataGridSearchResults_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridSearchResults.SortCommand
        Try
            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are search results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGridSearchResults.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = IIf(l_dataset_SearchResults.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                BindGrid(DataGridSearchResults, l_dataset_SearchResults.Tables(0).DefaultView)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub CleanUpExistingBenefitOptions()
        'Cleanup all local variables for Active Beneficiaries and Retired Beneficiaries
        l_dataset_ActiveBeneficiaries = Nothing : BindGrid(DataGridActiveBeneficiaries, l_dataset_ActiveBeneficiaries)
        l_dataset_ActiveSettlementOption_RetirementPlan = Nothing : BindGrid(DataGridActiveBenefitOptions_RetirementPlan, l_dataset_ActiveSettlementOption_RetirementPlan)
        l_dataset_ActiveSettlementOption_SavingsPlan = Nothing : BindGrid(DataGridActiveBenefitOptions_SavingsPlan, l_dataset_ActiveSettlementOption_SavingsPlan)
        l_dataset_RetiredBeneficiaries = Nothing : BindGrid(DataGridRetiredBeneficiaries, l_dataset_RetiredBeneficiaries)
        l_dataset_RetiredSettlementOption_RetirementPlan = Nothing : BindGrid(DataGridRetiredBenefitOptions_RetirementPlan, l_dataset_RetiredSettlementOption_RetirementPlan)
        l_dataset_RetiredSettlementOption_SavingsPlan = Nothing : BindGrid(DataGridRetiredBenefitOptions_SavingsPlan, l_dataset_RetiredSettlementOption_SavingsPlan)
    End Sub
#End Region

#Region "Active Beneficiary Settlement code"
    Private Sub ButtonSettleActiveBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSettleActiveBeneficiary.Click
        Try
            'Check if we have selected a beneficiary
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940

            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If DataGridActiveBeneficiaries.SelectedIndex < 0 Then
                MessageBox.Show(PlaceHolder1, "Error", "Please select a beneficiary first", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'START: SB | 2017.08.03 | YRS-AT-3324 | Checking if the Death Settlement is allowed for RMD elgilible participants whose RMD is not generated
            If Not Me.IsDeathSettlementAllowedForRMDEligibleParticipants Then
                MessageBox.Show(PlaceHolder1, "Error", Me.ReasonForRestriction, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'END: SB | 2017.08.03 | YRS-AT-3324 | Checking if the Death Settlement is allowed for RMD elgilible participants whose RMD is not generated

            ' START : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant and display error message in message box 
            If (RegenerateRMD) Then
                MessageBox.Show(PlaceHolder1, "Error", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATHSETTLEMENT_RMDREGENERATIONWARNING, MessageBoxButtons.Stop)
                Exit Sub
            End If
            ' END : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant and display error message in message box 
            If isSettled(l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString()) Then
                MessageBox.Show(PlaceHolder1, "Error", "Beneficiary already settled", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Check if we have selected an option for settlement - Retirement plan and Savings plan
            If DataGridActiveBenefitOptions_RetirementPlan.Items.Count > 0 AndAlso _
                                DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex < 0 Then  'NP:2007.11.07 - Adding check to ensure there were options to select from the datagrid
                MessageBox.Show(PlaceHolder1, "Error", "Please select a settlement option. If options are available under both plans they must be selected simultaneously", MessageBoxButtons.Stop)
                Exit Sub
            End If
            If DataGridActiveBenefitOptions_SavingsPlan.Items.Count > 0 AndAlso _
                                DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex < 0 Then 'NP:2007.11.07 - Adding check to ensure there were options to select from the datagrid
                MessageBox.Show(PlaceHolder1, "Error", "Please select a settlement option. If options are available under both plans they must be selected simultaneously", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'SP:2013.04.15:YRS 5.0-1990 -start
            Session("SelectedActiveBeneficiaryUniqueID") = l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString()
            'SP:2013.04.15:YRS 5.0-1990 -End
            ValidateSettlement()
            'Start-SR:2014.05.23- Beneficiary RMDs
            SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
            SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
            'End-SR:2014.05.23- Beneficiary RMDs
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DataGridActiveBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBeneficiaries.SelectedIndexChanged
        Try
            Dim i As Integer = DataGridActiveBeneficiaries.SelectedIndex
            'BindGrid(DataGridActiveBeneficiaries, l_dataset_ActiveBeneficiaries)    'NP:PS:2007.08.31 - Adding code to change the image of the button when the row is selected
            DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex = -1
            DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex = -1
            BindBeneficiaryDeathOptionDetails(l_dataset_ActiveBeneficiaries.Tables(0).Rows(i)("UniqueId").ToString())

            ButtonSettleActiveBeneficiary.Enabled = Not isSettled(l_dataset_ActiveBeneficiaries.Tables(0).Rows(i)("UniqueId").ToString())
            'Priya: 2008.10.05 Disable settlement button if no options are available 
            If isSettled(l_dataset_ActiveBeneficiaries.Tables(0).Rows(i)("UniqueId").ToString()) = Not True Then
                ButtonSettleActiveBeneficiary.Enabled = hasOptions(l_dataset_ActiveBeneficiaries.Tables(0).Rows(i)("UniqueId").ToString())
            End If
            'End 2008.10.05

            'START: SB | 2017.08.02 | YRS-AT-3324 | Added method to validate Death Settlement restricitions for RMD eligbile participants
            If HelperFunctions.isNonEmpty(FundEventID) Then
                If Not Me.IsDeathSettlementAllowedForRMDEligibleParticipants Then
                    ShowRegenerateRMDWarning()
                    Exit Sub
                End If
            End If
            'END: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate Death Settlement restricitions for RMD eligbile participants


            ' START : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant and display error message if value is true
            If (RegenerateRMD) Then
                ButtonSettleActiveBeneficiary.Enabled = False
                ShowRegenerateRMDWarning()
            End If
            ' END : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of the participant and display error message if value is true
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'NP:PS:2007.09.13 - Adding code to change the image of the button when the row is selected
    Private Sub DataGridActiveBeneficiaries_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBeneficiaries.PreRender
        Dim dg As DataGrid = DirectCast(sender, DataGrid)  'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dgi As DataGridItem
        Dim rBtn As ImageButton
        For Each dgi In dg.Items
            rBtn = dgi.Cells(0).FindControl("Imagebutton1")
            If Not rBtn Is Nothing Then
                If Not rBtn Is Nothing Then
                    If dgi.ItemType = ListItemType.SelectedItem Then
                        rBtn.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                    Else
                        rBtn.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub DataGridActiveBenefitOptions_RetirementPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBenefitOptions_RetirementPlan.ItemDataBound
        Dim hideColumnIndex As Integer
        hideColumnIndex = l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).Columns.IndexOf("PlanType")
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
        hideColumnIndex = l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).Columns.IndexOf("DeathFundEventStatus")
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
    End Sub
    'NP:PS:2007.09.13 - Handling the display of the selected image button properly for the Benefit Options
    Private Sub DataGridActiveBenefitOptions_RetirementPlan_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBenefitOptions_RetirementPlan.PreRender
        Dim dg As DataGrid = DirectCast(sender, DataGrid) 'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dgi As DataGridItem
        Dim ib As ImageButton
        For Each dgi In dg.Items
            ib = DirectCast(dgi.Cells(0).FindControl(ACTIVE_RETIREMENT_OPTIONS_RADIO_CONTROL_ID), ImageButton)  'Changed from CType to Directcast by SR:2010.06.17 for migration
            If Not ib Is Nothing Then
                If dgi.ItemType = ListItemType.SelectedItem Then
                    ib.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                Else
                    ib.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                End If
            End If
        Next
    End Sub
    Private Sub DataGridActiveBenefitOptions_SavingsPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBenefitOptions_SavingsPlan.ItemDataBound
        'Hide unnecessary columns
        Dim hideColumnIndex As Integer
        hideColumnIndex = l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).Columns.IndexOf("PlanType")    'NP:PS:2007.09.12 - Pulling information from the right dataset
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
        hideColumnIndex = l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).Columns.IndexOf("DeathFundEventStatus")    'NP:PS:2007.09.12 - Pulling information from the right dataset
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
    End Sub
    'NP:PS:2007.09.13 - Handling the display of the selected image button properly for the Benefit Options
    Private Sub DataGridActiveBenefitOptions_SavingsPlan_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBenefitOptions_SavingsPlan.PreRender
        Dim dg As DataGrid = DirectCast(sender, DataGrid) 'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dgi As DataGridItem
        Dim ib As ImageButton
        For Each dgi In dg.Items
            ib = DirectCast(dgi.Cells(0).FindControl(ACTIVE_SAVINGS_OPTIONS_RADIO_CONTROL_ID), ImageButton)  'Changed from CType to Directcast by SR:2010.06.17 for migration
            If Not ib Is Nothing Then
                If dgi.ItemType = ListItemType.SelectedItem Then
                    ib.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                Else
                    ib.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                End If
            End If
        Next
    End Sub
#End Region

#Region "Retired Beneficiary Settlement code"
    Private Sub ButtonSettleRetiredBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSettleRetiredBeneficiary.Click

        Try

            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'Check if we have selected a beneficiary
            If DataGridRetiredBeneficiaries.SelectedIndex < 0 Then
                MessageBox.Show(PlaceHolder1, "Error", "Please select a beneficiary first", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Check if the selected beneficiary is not settled already
            If isSettled(l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString()) Then
                MessageBox.Show(PlaceHolder1, "Error", "Beneficiary already settled", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Check if we have selected an option for settlement - Retirement plan and Savings plan
            If DataGridRetiredBenefitOptions_RetirementPlan.Items.Count > 0 AndAlso _
                    DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex < 0 Then 'NP:2007.11.07 - Adding check to ensure there were options to select from the datagrid
                MessageBox.Show(PlaceHolder1, "Error", "Please select a settlement option. If options are available under both plans they must be selected simultaneously", MessageBoxButtons.Stop)
                Exit Sub
            End If
            If DataGridRetiredBenefitOptions_SavingsPlan.Items.Count > 0 AndAlso _
                    DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex < 0 Then    'NP:2007.11.07 - Adding check to ensure there were options to select from the datagrid
                MessageBox.Show(PlaceHolder1, "Error", "Please select a settlement option. If options are available under both plans they must be selected simultaneously", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'SR:2011.04.27 -  YRS 5.0-1292,DeathNotify funtion is called for validating retired beneficiaries.
            Dim l_int_returnStatus As Integer
            l_int_returnStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyPrerequisites("PERSON", Session("ParticipantEntityId"), Session("g_DeathDate"), l_string_returnStatusDR, l_string_returnStatusDA)
            If l_int_returnStatus < 0 Then
                MessageBox.Show(PlaceHolder1, "Verify", "Death settlement failed due to error Network Error:" + Chr(13) + "Cannot Proceed", MessageBoxButtons.OK)
                Exit Sub
            End If
            'SP 2014.04.02 BT-2420\YRS 5.0-2309 - Start
            If (YMCARET.YmcaBusinessObject.BeneficiarySettlement.CheckAnnuityReversedAfterDeathBeneficiaryOptionCreated(Session("ParticipantEntityId"))) Then
                MessageBox.Show(PlaceHolder1, "Error", "Annuity payment voided after death notification. Recalculate before proceeding.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'SP 2014.04.02 BT-2420\YRS 5.0-2309 - End

            'SP:2013.04.15:YRS 5.0-1990 -start
            Session("SelectedRetiredBeneficiaryUniqueID") = l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString()
            'SP:2013.04.15:YRS 5.0-1990 -End
            'Start-SR:2014.05.23- Beneficiary RMDs
            SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
            SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
            'End-SR:2014.05.23- Beneficiary RMDs
            ValidateSettlement()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    Private Sub DataGridRetiredBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetiredBeneficiaries.SelectedIndexChanged
        Try
            Dim i As Integer = DataGridRetiredBeneficiaries.SelectedIndex
            'BindGrid(DataGridRetiredBeneficiaries, l_dataset_RetiredBeneficiaries)  'NP:PS:2007.08.31 - Adding code to change the image of the button when the row is selected
            DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex = -1
            DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex = -1
            BindBeneficiaryDeathOptionDetails(l_dataset_RetiredBeneficiaries.Tables(0).Rows(i)("UniqueId").ToString())
            ButtonSettleRetiredBeneficiary.Enabled = Not isSettled(l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString())
            'Priya: 2008.10.05 Disable settlement button if no options are available 
            If isSettled(l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString()) = Not True Then
                ButtonSettleRetiredBeneficiary.Enabled = hasOptions(l_dataset_RetiredBeneficiaries.Tables(0).Rows(i)("UniqueId").ToString())
            End If

            'End : 2008.10.05
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'NP:PS:2007.09.13 - Adding code to change the image of the button when the row is selected
    Private Sub DataGridRetiredBeneficiaries_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetiredBeneficiaries.PreRender
        Dim dg As DataGrid = DirectCast(sender, DataGrid) 'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dgi As DataGridItem
        Dim rBtn As ImageButton
        For Each dgi In dg.Items
            rBtn = dgi.Cells(0).FindControl("Imagebutton2")
            If Not rBtn Is Nothing Then
                If Not rBtn Is Nothing Then
                    If dgi.ItemType = ListItemType.SelectedItem Then
                        rBtn.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                    Else
                        rBtn.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub DataGridRetiredBenefitOptions_RetirementPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRetiredBenefitOptions_RetirementPlan.ItemDataBound
        Dim hideColumnIndex As Integer
        hideColumnIndex = l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).Columns.IndexOf("PlanType")
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
        hideColumnIndex = l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).Columns.IndexOf("DeathFundEventStatus")
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
    End Sub
    'NP:PS:2007.09.13 - Handling the display of the selected image button properly for the Benefit Options
    Private Sub DataGridRetiredBenefitOptions_RetirementPlan_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetiredBenefitOptions_RetirementPlan.PreRender
        Dim dg As DataGrid = DirectCast(sender, DataGrid) 'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dgi As DataGridItem
        Dim ib As ImageButton
        For Each dgi In dg.Items
            ib = DirectCast(dgi.Cells(0).FindControl(RETIRED_RETIREMENT_OPTIONS_RADIO_CONTROL_ID), ImageButton) 'Changed from CType to Directcast by SR:2010.06.17 for migration
            If Not ib Is Nothing Then
                If dgi.ItemType = ListItemType.SelectedItem Then
                    ib.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                Else
                    ib.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                End If
            End If
        Next
    End Sub
    Private Sub DataGridRetiredBenefitOptions_SavingsPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRetiredBenefitOptions_SavingsPlan.ItemDataBound
        'Hide unnecessary columns
        Dim hideColumnIndex As Integer
        hideColumnIndex = l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).Columns.IndexOf("PlanType")   'NP:PS:2007.09.12 - Pulling information from the right dataset
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
        hideColumnIndex = l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).Columns.IndexOf("DeathFundEventStatus")   'NP:PS:2007.09.12 - Pulling information from the right dataset
        If hideColumnIndex > -1 AndAlso e.Item.Cells.Count > (hideColumnIndex + 1) Then e.Item.Cells(hideColumnIndex + 1).Visible = False
    End Sub
    'NP:PS:2007.09.13 - Handling the display of the selected image button properly for the Benefit Options
    Private Sub DataGridRetiredBenefitOptions_SavingsPlan_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetiredBenefitOptions_SavingsPlan.PreRender
        Dim dg As DataGrid = DirectCast(sender, DataGrid) 'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dgi As DataGridItem
        Dim ib As ImageButton
        For Each dgi In dg.Items
            ib = DirectCast(dgi.Cells(0).FindControl(RETIRED_SAVINGS_OPTIONS_RADIO_CONTROL_ID), ImageButton) 'Changed from CType to Directcast by SR:2010.06.17 for migration
            If Not ib Is Nothing Then
                If dgi.ItemType = ListItemType.SelectedItem Then
                    ib.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                Else
                    ib.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                End If
            End If
        Next
    End Sub
#End Region

    'This Utility section works as a charm
#Region "General Utility Functions"
    'dg = The datagrid to bind data to
    'ds = The dataset which contains the data
    'forceVisible = Whether the datagrid should be displayed if it does not contain any data
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = ds.Tables(0)
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        If dv Is Nothing OrElse dv.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = dv
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
#End Region

#Region "Miscellaneous Code"
    'This function is the first entry point for Beneficiary Settlement. This function populates the various 
    'datasets for active and retired beneficiaries and also populates the settlement options for the selected
    'beneficiary in their own datasets.
    Private Sub Process_BeneficiaryDetails()
        'Here we populate the Active and/or Retired Tab with the appropriate beneficiaries and their benefit options
        'First decide which tabs to activate. 
        'Second populate the applicable local datasets.
        'Then call the populate functions for those tabs. That would handle everything else.

        'Pick the row that matches the SSN of the selected participant to obtain the PersId
        Dim l_string_persID As String   'Store the participant ID
        Dim arrDr() As DataRow
        Dim dr As DataRow
        'NP:PS:2007.09.18 - Selecting the right row from the table using an alternate method
        'arrDr = l_dataset_SearchResults.Tables("r_MemberListForDeceased").Select("[SS No]= '" & Me.DataGridSearchResults.SelectedItem.Cells(1).Text.Trim() & "'")
        'If arrDr.Length = 0 Then Exit Sub 'If no matches then exit sub 
        'dr = arrDr(0)
        If HelperFunctions.isEmpty(l_dataset_SearchResults) Then Exit Sub 'We do not have any search results to work with
        dr = l_dataset_SearchResults.Tables(0).DefaultView.Item(DataGridSearchResults.SelectedIndex).Row
        If dr Is Nothing Then Exit Sub 'No Matches found - highly unlikely

        g_DeathDate = Date.Parse(dr("Death Date"))
        Session.Add("g_DeathDate", g_DeathDate)

        If dr.IsNull("PersID") Then
            'If the Participants' Id is not available then we cannot proceed. Exit sub.
            l_string_persID = "" : Exit Sub
        End If

        l_string_persID = dr("PersID").ToString().Trim()

        'Shashi Shekhar     28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
        Headercontrol.pageTitle = "Settle Beneficiaries"
        Headercontrol.guiPerssId = dr("PersID").ToString().Trim().Trim


        'By Aparna -YREN-3015- require this id to check the Currency code so that same can be used for Beneficiary
        Session("ParticipantEntityId") = l_string_persID
        'By Aparna -YREN-3015


        'We now have the Id of the selected participant. 
        'Let us identify if we need to populate the Active tab or the Retired Tab
        Dim l_dataset_BS_Lookup_ActiveorRetiredDetails As DataSet
        l_dataset_BS_Lookup_ActiveorRetiredDetails = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_ActiveorRetiredDetails(l_string_persID)
        If HelperFunctions.isEmpty(l_dataset_BS_Lookup_ActiveorRetiredDetails) Then
            'Some error occured. Abort, abort, abort...
            TabStripBeneficiarySettlement.Items(1).Enabled = False  'Disable the Active Beneficiaries Tab
            TabStripBeneficiarySettlement.Items(2).Enabled = False  'Disable the Retired Beneficiaries Tab
        End If
        'If the dataset that is returned is empty then disable both Active and Retired tabs
        If HelperFunctions.isEmpty(l_dataset_BS_Lookup_ActiveorRetiredDetails) Then
            TabStripBeneficiarySettlement.Items(1).Enabled = False
            TabStripBeneficiarySettlement.Items(2).Enabled = False
        Else
            'If "ActiveCnt" > 0 then enable Active Beneficiaries tab
            TabStripBeneficiarySettlement.Items(1).Enabled = (l_dataset_BS_Lookup_ActiveorRetiredDetails.Tables(0).Rows(0)("ActiveCnt") > 0)
            'If "RetiredCnt" > 0 then enable Retired Beneficiaries tab
            TabStripBeneficiarySettlement.Items(2).Enabled = (l_dataset_BS_Lookup_ActiveorRetiredDetails.Tables(0).Rows(0)("RetiredCnt") > 0)
        End If

        'Populate the Active Tab if needed
        If TabStripBeneficiarySettlement.Items(1).Enabled Then
            l_dataset_ActiveBeneficiaries = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_BeneficiariesPrimeSettle(l_string_persID, "DA")
        Else
            l_dataset_ActiveBeneficiaries = Nothing
        End If
        If HelperFunctions.isNonEmpty(l_dataset_ActiveBeneficiaries) Then
            DataGridActiveBeneficiaries.SelectedIndex = 0
            'Populate the Benefit Options information
            Dim l_dataset_temp_Benefit_Options As DataSet = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_DeathBenefitOptions(l_string_persID, "DA")
            SplitOptionsIntoIndividualPlans(l_dataset_temp_Benefit_Options, l_dataset_ActiveSettlementOption_RetirementPlan, l_dataset_ActiveSettlementOption_SavingsPlan)
            DataGridActiveBeneficiaries_SelectedIndexChanged(Me, Nothing)
        End If
        BindGrid(DataGridActiveBeneficiaries, l_dataset_ActiveBeneficiaries)

        'Populate the Retired Tab if needed
        If TabStripBeneficiarySettlement.Items(2).Enabled Then
            l_dataset_RetiredBeneficiaries = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_BeneficiariesPrimeSettle(l_string_persID, "DR")
        Else
            l_dataset_RetiredBeneficiaries = Nothing
        End If
        If HelperFunctions.isNonEmpty(l_dataset_RetiredBeneficiaries) Then
            DataGridRetiredBeneficiaries.SelectedIndex = 0
            'Populate the Benefit Options information
            Dim l_dataset_temp_Benefit_Options As DataSet = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_DeathBenefitOptions(l_string_persID, "DR")
            SplitOptionsIntoIndividualPlans(l_dataset_temp_Benefit_Options, l_dataset_RetiredSettlementOption_RetirementPlan, l_dataset_RetiredSettlementOption_SavingsPlan)
            DataGridRetiredBeneficiaries_SelectedIndexChanged(Me, Nothing)
        End If
        BindGrid(DataGridRetiredBeneficiaries, l_dataset_RetiredBeneficiaries)

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            'Clear Sessions
            'DK:2012.12.07  - BT - 1433 : Problem in Death Settlement.
            ClearSessions()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub
    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            TextBoxFirstName.Text = ""
            TextBoxLastName.Text = ""
            TextBoxSSNo.Text = ""
            TextBoxFundNo.Text = ""
            BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
            'START - SB | 2017.12.07 | YRS-AT-3324 | Clearing session variables, disable Active and Retire Beneficiary tab
            ClearSessions()
            TabStripBeneficiarySettlement.Items(ACTIVE_BENEFICIARY_TAB_INDEX).Enabled = False    'Disable the Active Beneficiaries Tab
            TabStripBeneficiarySettlement.Items(RETIRED_BENEFICIARY_TAB_INDEX).Enabled = False   'Disable the Retired Beneficiaries Tab
            DivMessage.InnerHtml = ""
            DivMessage.Attributes.Add("display", "none")
            'END - SB | 2017.12.07 | YRS-AT-3324 | Clearing session variables, disable Active and Retire Beneficiary tab
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    'This function works with Death Settlement Options dataviews and initializes the selected index of the datagrid
    'If the participant has been settled then it selects the settled option by looking at the dataview else it selected the default option i.e. 0
    Private Sub InitializeDataGridSelectedIndex(ByVal dg As DataGrid, ByVal dv As DataView)
        If HelperFunctions.isNonEmpty(dv) Then
            Dim dr As DataRowView
            Dim i As Integer = -1
            For i = 0 To dv.Count - 1
                dr = dv.Item(i)
                If Not dr.Row.IsNull("Settled") Then
                    dg.SelectedIndex = i
                    Exit Sub
                End If
            Next
            'If dg.SelectedIndex < 0 Then dg.SelectedIndex = 0
        End If
    End Sub
    Private Function isSettled(ByVal beneficiaryId As String) As Boolean
        Dim result As String
        Try
            If getBeneficiaryType(beneficiaryId) = "ACTIVE" Then
                If l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex).IsNull("Status") Then Return False
                result = l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("Status").ToString()
            ElseIf getBeneficiaryType(beneficiaryId) = "RETIRED" Then
                If l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex).IsNull("Status") Then Return False
                result = l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("Status").ToString()
                'Else
                '    MessageBox.Show(PlaceHolder1, "Unexpected Error", "Unable to Identify if Beneficiary is already settled or not", MessageBoxButtons.OK)
            End If
            If Not result Is Nothing AndAlso result.ToUpper() = "SETTLD" Then Return True ''updated by sanjay on 24 Aug 09 for YRS 5.0-874
            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Priya 2008.08.04
    Private Function hasOptions(ByVal beneficiaryId As String) As Boolean
        ' Dim l_dataset_ActiveSettlementOption_RetirementPlan As DataSet  'Store Retirement Plan Settlement Options for Active Beneficiaries
        'Dim l_dataset_ActiveSettlementOption_SavingsPlan As DataSet     'Store Savings Plan Settlement Options for Active Beneficiaries
        'Dim l_dataset_RetiredSettlementOption_RetirementPlan As DataSet 'Store Retirement Plan Settlement Options for Retired Beneficiaries
        'Dim l_dataset_RetiredSettlementOption_SavingsPlan As DataSet
        '"BeneficiaryID='" + l_string_BeneficiaryID + "'"
        Try
            If getBeneficiaryType(beneficiaryId) = "ACTIVE" Then
                ' If l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex).IsNull("Status") Then Return False
                If HelperFunctions.isNonEmpty(l_dataset_ActiveBeneficiaries) AndAlso l_dataset_ActiveBeneficiaries.Tables(0).Select("UniqueId='" + beneficiaryId + "'").Length > 0 Then
                    If HelperFunctions.isNonEmpty(l_dataset_ActiveSettlementOption_RetirementPlan) AndAlso l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).Select("BeneficiaryID='" + beneficiaryId + "'").Length > 0 Then
                        Return True
                    End If
                    If HelperFunctions.isNonEmpty(l_dataset_ActiveSettlementOption_SavingsPlan) AndAlso l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).Select("BeneficiaryID='" + beneficiaryId + "'").Length > 0 Then
                        Return True
                    End If

                End If
            ElseIf getBeneficiaryType(beneficiaryId) = "RETIRED" Then
                'If l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex).IsNull("Status") Then Return False
                'If isNonEmpty(l_dataset_RetiredBeneficiaries) AndAlso l_dataset_RetiredBeneficiaries.Tables(0).Select("UniqueId='" + beneficiaryId + "'").Length > 0 Then
                '    Return True
                'End If
                If HelperFunctions.isNonEmpty(l_dataset_RetiredSettlementOption_RetirementPlan) AndAlso l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).Select("BeneficiaryID='" + beneficiaryId + "'").Length > 0 Then
                    Return True
                End If
                If HelperFunctions.isNonEmpty(l_dataset_RetiredSettlementOption_SavingsPlan) AndAlso l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).Select("BeneficiaryID='" + beneficiaryId + "'").Length > 0 Then
                    Return True
                End If
            ElseIf getBeneficiaryType(beneficiaryId) = "UNKNOWN" Then
                Return False
            End If
            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function


    'NP:PS:2007.07.10 - Added code to bind the grids
    'BindBeneficiaryDeathOptionDetails function takes the BeneficiaryId and binds the appropriate DataGrids
    Private Sub BindBeneficiaryDeathOptionDetails(ByVal l_string_BeneficiaryID As String)
        'We need to identify which type of beneficiary is this - ACTIVE or RETIRED
        Dim l_string_BeneficiaryType As String = getBeneficiaryType(l_string_BeneficiaryID)
        Dim l_dataset_CurrentSettlementOption As DataSet
        Dim l_string_RowFilter As String = "BeneficiaryID='" + l_string_BeneficiaryID + "'"
        Dim tmp_dv As DataView
        Try
            Select Case l_string_BeneficiaryType
                Case "ACTIVE"
                    'ACTIVE - RETIREMENT PLAN SECTION
                    If HelperFunctions.isEmpty(l_dataset_ActiveSettlementOption_RetirementPlan) Then    'If there is no data then change the text
                        LabelActiveBenefitOptions_RetirementPlan.Text = "Retirement Plan Settlement Options not available for this beneficiary"
                    Else
                        'Set the filters and check if data is available
                        tmp_dv = l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView
                        tmp_dv.RowFilter = l_string_RowFilter
                        If HelperFunctions.isEmpty(tmp_dv) Then
                            LabelActiveBenefitOptions_RetirementPlan.Text = "Retirement Plan Settlement Options not available for this beneficiary"
                        Else
                            InitializeDataGridSelectedIndex(DataGridActiveBenefitOptions_RetirementPlan, tmp_dv)
                            LabelActiveBenefitOptions_RetirementPlan.Text = "Retirement Plan Settlement Options"
                        End If
                        'Then we bind grid to the datasource which will hide/disable the datagrid if required
                        BindGrid(DataGridActiveBenefitOptions_RetirementPlan, tmp_dv)
                    End If

                    'ACTIVE - SAVINGS PLAN SECTION
                    If HelperFunctions.isEmpty(l_dataset_ActiveSettlementOption_SavingsPlan) Then
                        LabelActiveBenefitOptions_SavingsPlan.Text = "Savings Plan Settlement Options not available for this beneficiary"
                    Else
                        'Set the filters and check if data is available
                        tmp_dv = l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView
                        tmp_dv.RowFilter = l_string_RowFilter
                        If HelperFunctions.isEmpty(tmp_dv) Then
                            LabelActiveBenefitOptions_SavingsPlan.Text = "Savings Plan Settlement Options not available for this beneficiary"
                        Else
                            InitializeDataGridSelectedIndex(DataGridActiveBenefitOptions_SavingsPlan, tmp_dv)
                            LabelActiveBenefitOptions_SavingsPlan.Text = "Savings Plan Settlement Options"
                        End If
                        'Then we bind grid to the datasource which will hide/disable the datagrid if required
                        BindGrid(DataGridActiveBenefitOptions_SavingsPlan, tmp_dv)
                    End If

                    If DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex > -1 OrElse DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex > -1 Then
                        ButtonSettleActiveBeneficiary.Enabled = True
                    Else
                        ButtonSettleActiveBeneficiary.Enabled = False
                    End If
                Case "RETIRED"
                    'RETIRED - RETIREMENT PLAN SECTION
                    If HelperFunctions.isEmpty(l_dataset_RetiredSettlementOption_RetirementPlan) Then
                        LabelRetiredBenefitOptions_RetirementPlan.Text = "Retirement Plan Settlement Options not available for this beneficiary"
                    Else
                        tmp_dv = l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView
                        tmp_dv.RowFilter = l_string_RowFilter
                        If HelperFunctions.isEmpty(tmp_dv) Then
                            LabelRetiredBenefitOptions_RetirementPlan.Text = "Retirement Plan Settlement Options not available for this beneficiary"
                        Else
                            InitializeDataGridSelectedIndex(DataGridRetiredBenefitOptions_RetirementPlan, tmp_dv)
                            LabelRetiredBenefitOptions_RetirementPlan.Text = "Retirement Plan Settlement Options"
                        End If
                        BindGrid(DataGridRetiredBenefitOptions_RetirementPlan, tmp_dv)
                    End If

                    'RETIRED - SAVINGS PLAN SECTION
                    If HelperFunctions.isEmpty(l_dataset_RetiredSettlementOption_SavingsPlan) Then
                        LabelRetiredBenefitOptions_SavingsPlan.Text = "Savings Plan Settlement Options not available for this beneficiary"
                    Else
                        tmp_dv = l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView
                        tmp_dv.RowFilter = l_string_RowFilter
                        If HelperFunctions.isEmpty(tmp_dv) Then
                            LabelRetiredBenefitOptions_SavingsPlan.Text = "Savings Plan Settlement Options not available for this beneficiary"
                        Else
                            InitializeDataGridSelectedIndex(DataGridRetiredBenefitOptions_SavingsPlan, tmp_dv)
                            LabelRetiredBenefitOptions_SavingsPlan.Text = "Savings Plan Settlement Options"
                        End If
                        BindGrid(DataGridRetiredBenefitOptions_SavingsPlan, tmp_dv)
                    End If

                    If DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex > -1 OrElse DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex > -1 Then
                        ButtonSettleRetiredBeneficiary.Enabled = True
                    Else
                        ButtonSettleRetiredBeneficiary.Enabled = False
                    End If
                Case Else

                    ButtonSettleActiveBeneficiary.Enabled = False
                    ButtonSettleRetiredBeneficiary.Enabled = False
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'This function takes the beneficiary Id and identifies if it is an ACTIVE beneficiary or a RETIRED beneficiary
    'Expected Input:    Beneficiary Id for the selected deceased participant
    'Expected Output:   ACTIVE or RETIRED or UNKNOWN
    Private Function getBeneficiaryType(ByVal id As String) As String
        Try
            If HelperFunctions.isNonEmpty(l_dataset_ActiveBeneficiaries) AndAlso l_dataset_ActiveBeneficiaries.Tables(0).Select("UniqueId='" + id + "'").Length > 0 Then
                Return "ACTIVE"
            ElseIf HelperFunctions.isNonEmpty(l_dataset_RetiredBeneficiaries) AndAlso l_dataset_RetiredBeneficiaries.Tables(0).Select("UniqueId='" + id + "'").Length > 0 Then
                Return "RETIRED"
            Else
                Return "UNKNOWN"
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    'This function takes ds as input and returns two datasets which contain information from ds
    'Expected Input:    Dataset containing list of options for all beneficiaries of a deceased participant
    'Expected Output:  Datasets containing list of RetirementPlan Options and SavingsPlan Options for all beneficiaries of the deceased participant
    Private Sub SplitOptionsIntoIndividualPlans(ByVal ds As DataSet, ByRef ds_RetirementPlan As DataSet, ByRef ds_SavingsPlan As DataSet)
        'If invalid data or incomplete data is passed then return null and exit
        Try
            If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                ds_RetirementPlan = Nothing : ds_SavingsPlan = Nothing
                Exit Sub
            End If
            Dim dr As DataRow
            ds.Tables(0).DefaultView.RowFilter = "PlanType = 'RETIREMENT' OR PlanType is NULL"
            If ds.Tables(0).DefaultView.Count > 0 Then
                ds_RetirementPlan = ds.Clone()
                Dim ienum As IEnumerator = ds.Tables(0).DefaultView.GetEnumerator()
                While ienum.MoveNext()
                    ds_RetirementPlan.Tables(0).Rows.Add(DirectCast(ienum.Current, DataRowView).Row.ItemArray()) 'Changed from CType to Directcast by SR:2010.06.17 for migration
                End While
            End If
            ds.Tables(0).DefaultView.RowFilter = "PlanType = 'SAVINGS'"
            If ds.Tables(0).DefaultView.Count > 0 Then
                ds_SavingsPlan = ds.Clone()
                Dim ienum As IEnumerator = ds.Tables(0).DefaultView.GetEnumerator()
                While ienum.MoveNext()
                    ds_SavingsPlan.Tables(0).Rows.Add(DirectCast(ienum.Current, DataRowView).Row.ItemArray()) 'Changed from CType to Directcast by SR:2010.06.17 for migration
                End While
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ValidateSettlement() As Boolean
        'Start - Manthan | 2016.05.20 | YRS-AT-2270 | Commented existing code and added space and new line in between message text to display it properly. 
        'MessageBox.Show(PlaceHolder1, "Verify", IIf(l_string_returnStatusDR = "", "Are you sure you want to settle this beneficiary?", "Are you sure you want to settle this beneficiary?" + l_string_returnStatusDR + "Do you wish to proceed?"), MessageBoxButtons.YesNo)
        MessageBox.Show(PlaceHolder1, "Verify", IIf(l_string_returnStatusDR = "", "Are you sure you want to settle this beneficiary?", "Are you sure you want to settle this beneficiary?" + " " + l_string_returnStatusDR + LINEBREAK + "Do you wish to proceed?"), MessageBoxButtons.YesNo)
        'End - Manthan | 2016.05.20 | YRS-AT-2270 | Commented existing code and added space and new line in between message text to display it properly.
    End Function

    'This function binds the beneficiaries for the current tab
    'It also calls a function to bind Benefit Options for the beneficiaries
    'Need to split this to Bind Active Beneficiaries and Bind Retired Beneficiaries
    Private Sub BindBeneficiaryDetailsPages()

        ''''Dim l_DataTable_BeneficiaryCurrentBeneficiaries As DataTable
        ''''Dim l_string_BeneficiaryID As String
        ''''If (Not DataSet_CurrentBeneficiaries Is Nothing) Then
        ''''    l_DataTable_BeneficiaryCurrentBeneficiaries = Me.DataSet_CurrentBeneficiaries.Tables("r_BeneficiaryPrimesettle")
        ''''Else
        ''''    l_DataTable_BeneficiaryCurrentBeneficiaries = Nothing
        ''''End If

        ''''If l_DataTable_BeneficiaryCurrentBeneficiaries Is Nothing _
        ''''    OrElse l_DataTable_BeneficiaryCurrentBeneficiaries.Rows.Count <= 0 Then

        ''''    DataGridActiveBeneficiaries.DataSource = Nothing
        ''''    DataGridActiveBeneficiaries.DataBind()

        ''''    DataGridRetiredBeneficiaries.DataSource = Nothing
        ''''    DataGridRetiredBeneficiaries.DataBind()
        ''''    l_string_BeneficiaryID = ""
        ''''    g_l_string_BeneficiaryID = l_string_BeneficiaryID
        ''''Else
        ''''    If (Me.Beneficiarycategory = "A") Then
        ''''        BindGrid(DataGridActiveBeneficiaries, DataSet_CurrentBeneficiaries)
        ''''        If Not IsDBNull(l_DataTable_BeneficiaryCurrentBeneficiaries.Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueID")) Then
        ''''            l_string_BeneficiaryID = l_DataTable_BeneficiaryCurrentBeneficiaries.Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueID").ToString().Trim()
        ''''            g_l_string_BeneficiaryID = l_string_BeneficiaryID
        ''''            'Preeti- Death changes
        ''''            'g_Relation = DataGridActiveBeneficiaries.Items(DataGridActiveBeneficiaries.SelectedIndex).Cells(8).Text
        ''''            'g_DeathFundEventStatus = DataGridActiveBeneficiaries.Items(DataGridActiveBeneficiaries.SelectedIndex).Cells(15).Text
        ''''        End If
        ''''    ElseIf Me.Beneficiarycategory = "R" Then
        ''''        BindGrid(DataGridRetiredBeneficiaries, DataSet_CurrentBeneficiaries)
        ''''        l_string_BeneficiaryID = l_DataTable_BeneficiaryCurrentBeneficiaries.Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueID").ToString().Trim()
        ''''        g_l_string_BeneficiaryID = l_string_BeneficiaryID
        ''''    End If
        ''''End If

        ''''BindBeneficiaryDeathOptionDetails(l_string_BeneficiaryID)

    End Sub

    Private Sub ButtonRefund_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonRefund.Command
        Try
            Call_RefundBeneficiaryForm()
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    'This function identifies the appropriate parameters and then calls the Beneficiary Information screen with the parameters in the Session or the URL
    Private Sub CallBeneficiaryInformationForm()

        Dim g_l_string_BeneficiaryID As String
        Dim selectedOptionRetirementPlan, selectedOptionSavingsPlan As String  'OptionId of the selected options for Retirement and savings plan
        Dim dv As DataView
        Dim errMessage As String = String.Empty

        'NP:PS:2007.08.20 - Splitting the initialization into two lines. Apparently this is a bug in the framework.
        selectedOptionRetirementPlan = String.Empty
        selectedOptionSavingsPlan = String.Empty

        Try
            If (MultiPageBeneficiarySettlement.SelectedIndex = ACTIVE_BENEFICIARY_TAB_INDEX) Then
                g_l_string_BeneficiaryID = l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString()
                If HelperFunctions.isNonEmpty(l_dataset_ActiveSettlementOption_RetirementPlan) Then
                    dv = l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        'Validate: If settlement option is available then it has to be selected
                        If DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex < 0 Then
                            errMessage += "A settlement option must be selected from the Retirement Plan" + LINEBREAK
                        Else
                            selectedOptionRetirementPlan = dv.Item(Me.DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex)("UniqueId").ToString()
                        End If
                    End If
                End If
                If HelperFunctions.isNonEmpty(l_dataset_ActiveSettlementOption_SavingsPlan) Then
                    dv = l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        'Validate: If settlement option is available then it has to be selected
                        If DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex < 0 Then
                            errMessage += "A settlement option must be selected from the Savings Plan" + LINEBREAK
                        Else
                            selectedOptionSavingsPlan = dv.Item(Me.DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex)("UniqueId").ToString()
                        End If
                    End If
                End If
            ElseIf (MultiPageBeneficiarySettlement.SelectedIndex = RETIRED_BENEFICIARY_TAB_INDEX) Then
                g_l_string_BeneficiaryID = l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString()
                If HelperFunctions.isNonEmpty(l_dataset_RetiredSettlementOption_RetirementPlan) Then
                    dv = l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        'Validate: If settlement option is available then it has to be selected
                        If DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex < 0 Then
                            errMessage += "A settlement option must be selected from the Retirement Plan" + LINEBREAK
                        Else
                            selectedOptionRetirementPlan = dv.Item(Me.DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex)("UniqueId").ToString()
                        End If
                    End If
                End If
                If HelperFunctions.isNonEmpty(l_dataset_RetiredSettlementOption_SavingsPlan) Then
                    dv = l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        'Validate: If settlement option is available then it has to be selected
                        If DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex < 0 Then
                            errMessage += "A settlement option must be selected from the Savings Plan" + LINEBREAK
                        Else
                            selectedOptionSavingsPlan = dv.Item(Me.DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex)("UniqueId").ToString()
                        End If
                    End If
                End If
            End If

            'Validate: Display any error messages to the user and exit sub
            If errMessage <> String.Empty Then
                MessageBox.Show(PlaceHolder1, "Error", errMessage, MessageBoxButtons.Stop, MessageBoxIcon.Error)
                Exit Sub
            End If

            'NP:YRPS-4009:2008.01.23 Updating code to perform PreRequisite validation of the settlement options
            'We now have all the selected options and we have already validated the selections.
            'We will now validate the settlement options themselves
            Dim l_int_returnValue As Integer
            l_int_returnValue = YMCARET.YmcaBusinessObject.BeneficiarySettlement.PerformPreRequisiteValidations(selectedOptionRetirementPlan, selectedOptionSavingsPlan, errMessage)
            If l_int_returnValue <> 1 Then
                MessageBox.Show(PlaceHolder1, "Error", errMessage, MessageBoxButtons.Stop, MessageBoxIcon.Error)
                Exit Sub
            End If

            'We now have all the selected options and we have already validated the selections.
            'Pass this information to the BI screen via sessions since passing the option Id from the url is not safe
            If (selectedOptionRetirementPlan <> "" Or selectedOptionSavingsPlan <> "") Then
                'Session: Pass data into Session so that it is accessible to the Beneficiary Information screen
                Session("BS_SelectedOption_RP") = selectedOptionRetirementPlan
                Session("BS_SelectedOption_SP") = selectedOptionSavingsPlan
                Session("SP_Parameters_AnnuityOption_RP") = Nothing   'Initialize the Annuity option for Retirement plan
                Session("SP_Parameters_AnnuityOption_SP") = Nothing   'Initialize the Annuity option for Savings plan
                'Start:SR:2014.07.09-BT 2593 - UI changed to fix the size of Beneficiary Information page
                Dim l_string_clientScript As String = "<script language='javascript'>" & _
                        "window.open('BeneficiaryInformation.aspx?','CustomPopUpBeneficiaryInformation', " & _
                        "'width=1000, height=740, titlebar=yes, resizable=no,top=0,left=0,scrollbars=yes,status=yes')" & _
                       "</script>"
                'End:SR:2014.07.09-BT 2593 - UI changed to fix the size of Beneficiary Information page
                '"window.open('BeneficiaryInformation.aspx?BenefitOptionID=" & selectedOptionRetirementPlan & "','CustomPopUpBeneficiaryInformation', " & _
                '                    window.moveTo(0,0);
                'window.resizeTo(screen.width,screen.height);

                If (Not IsClientScriptBlockRegistered("clientScript")) Then
                    RegisterClientScriptBlock("clientScript", l_string_clientScript)
                End If
            Else
                'Unable to obtain/identify the selected options. Display appropriate error message if possible
                MessageBox.Show(PlaceHolder1, "Error", "Beneficiary Settlement failed due to error!", MessageBoxButtons.Stop)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub Call_RefundBeneficiaryForm()
        Dim i As DataGridItem
        Dim g_l_string_BeneficiaryID As String
        Dim selectedOptionRetirementPlan, selectedOptionSavingsPlan As String  'OptionId of the selected options for Retirement and savings plan
        Dim l_string_POptionRetirementPlan, l_string_POptionSavingsPlan As String   'POption Type for each plan - values are A, B, C or D
        Dim dv As DataView

        Try
            selectedOptionRetirementPlan = String.Empty
            selectedOptionSavingsPlan = String.Empty
            l_string_POptionRetirementPlan = String.Empty
            l_string_POptionSavingsPlan = String.Empty
            'No need to validate selected options here since they have already been validated when a call was made to 
            'the Beneficiary Information screen. Not a good idea but then we would need to write the error handling code for it.
            If (MultiPageBeneficiarySettlement.SelectedIndex = ACTIVE_BENEFICIARY_TAB_INDEX) Then
                g_l_string_BeneficiaryID = l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString()
                If HelperFunctions.isNonEmpty(l_dataset_ActiveSettlementOption_RetirementPlan) Then
                    dv = l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        selectedOptionRetirementPlan = dv.Item(Me.DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex)("UniqueId").ToString()
                        l_string_POptionRetirementPlan = dv.Item(Me.DataGridActiveBenefitOptions_RetirementPlan.SelectedIndex)("Pmt").ToString()
                    End If
                End If
                If HelperFunctions.isNonEmpty(l_dataset_ActiveSettlementOption_SavingsPlan) Then
                    dv = l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        selectedOptionSavingsPlan = dv.Item(Me.DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex)("UniqueId").ToString()
                        l_string_POptionSavingsPlan = dv.Item(Me.DataGridActiveBenefitOptions_SavingsPlan.SelectedIndex)("Pmt").ToString()
                    End If
                End If
            ElseIf (Me.MultiPageBeneficiarySettlement.SelectedIndex = RETIRED_BENEFICIARY_TAB_INDEX) Then
                g_l_string_BeneficiaryID = l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString()
                If HelperFunctions.isNonEmpty(l_dataset_RetiredSettlementOption_RetirementPlan) Then
                    dv = l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        selectedOptionRetirementPlan = dv.Item(Me.DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex)("UniqueId").ToString()
                        l_string_POptionRetirementPlan = dv.Item(Me.DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndex)("Pmt").ToString()
                    End If
                End If
                If HelperFunctions.isNonEmpty(l_dataset_RetiredSettlementOption_SavingsPlan) Then
                    dv = l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView
                    dv.RowFilter = "BeneficiaryId = '" & g_l_string_BeneficiaryID & "'"
                    If HelperFunctions.isNonEmpty(dv) Then
                        selectedOptionSavingsPlan = dv.Item(Me.DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex)("UniqueId").ToString()
                        l_string_POptionSavingsPlan = dv.Item(Me.DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndex)("Pmt").ToString()
                    End If
                End If
            End If

            'Session("string_POption") = l_string_POption
            'Session("SP_Parameters_DeathBenefitOptionID") = l_string_selectedValue

            'We now have all the selected options and we have already validated the selections.
            'Pass this information to the BI screen via sessions since passing the option Id from the url is not safe
            'Session: Pass data into Session so that it is accessible to the Beneficiary Refund screen
            If (l_string_POptionRetirementPlan <> "" AndAlso l_string_POptionRetirementPlan <> "A") Then
                Session("BS_SelectedOption_RP") = selectedOptionRetirementPlan
            Else    'Initialize the parameters that come in from the BR screen to nulls
                Session("BS_SelectedOption_RP") = Nothing
                'Session("SP_Parameters_DeathBenefitOptionID_RP") = selectedOptionRetirementPlan 'NP:PS:2007.07.26 - This is already being set when we return from the beneficiary screen
                Session("SP_Parameters_RolloverTaxable_RP") = Nothing
                Session("SP_Parameters_RolloverNonTaxable_RP") = Nothing
                Session("SP_Parameters_RolloverInstitutionID_RP") = Nothing
                Session("SP_Parameters_WithholdingPct_RP") = Nothing
            End If
            If (l_string_POptionSavingsPlan <> "" AndAlso l_string_POptionSavingsPlan.Trim <> "A") Then
                Session("BS_SelectedOption_SP") = selectedOptionSavingsPlan
            Else    'Initialize the parameters that come in from the BR screen to nulls
                Session("BS_SelectedOption_SP") = Nothing
                'Session("SP_Parameters_DeathBenefitOptionID_SP") = selectedOptionSavingsPlan    'NP:PS:2007.07.26 - This is not required since we are populating this from the Beneficiary Information screen
                Session("SP_Parameters_RolloverTaxable_SP") = Nothing
                Session("SP_Parameters_RolloverNonTaxable_SP") = Nothing
                Session("SP_Parameters_RolloverInstitutionID_SP") = Nothing
                Session("SP_Parameters_WithholdingPct_SP") = Nothing
            End If
            'Call Beneficiary refund screen only if POption is not "A"
            If (l_string_POptionSavingsPlan <> "" AndAlso l_string_POptionSavingsPlan.Trim <> "A") Or _
                    (l_string_POptionRetirementPlan <> "" AndAlso l_string_POptionRetirementPlan <> "A") Then

                Dim l_string_clientScript As String = "<script language='javascript'>" & _
                                    "window.open('RefundBeneficiary.aspx','CustomPopUp', " & _
                                    "'width=1020, height=760, resizable=yes,top=0,left=0,scrollbars=yes')" & _
                                    "</script>"
                '   "window.open('RefundBeneficiary.aspx?BenefitOptionID=" & selectedOptionRetirementPlan & "','CustomPopUp', " & _
                If (Not IsClientScriptBlockRegistered("clientScript")) Then
                    RegisterClientScriptBlock("clientScript", l_string_clientScript)
                End If
            Else
                'Session("string_POption") = l_string_POption
                Session("Success_BRScreen") = True
                Session("CurrentForm") = "BR"
                Update_FinalSettlement()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'This function looks at the various parameters and then calls the settlement process with the right values
    Private Sub Update_FinalSettlement()

        Dim l_string_OutputMessage As String
        Dim dsRMDs_RP As DataSet
        Dim dsRMDs_SP As DataSet
        'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Declared objects and variables
        Dim dsFinalDedsAnnuity As DataSet
        Dim strDedAnnuity As String
        Dim dsFinalDedsLumpsum As DataSet
        Dim strDedLumpsum As String
        'End - Manthan | 2016.04.22 | YRS-AT-2206 | Declared objects and variables
        l_string_OutputMessage = ""
        'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Storing all the deductions values selected in string in XML format
        ' -- Deductions for Annuity option
        If Not Session("FinalDeductionsAnnuity") Is Nothing Then
            dsFinalDedsAnnuity = DirectCast(Session("FinalDeductionsAnnuity"), DataSet)
        End If
        If HelperFunctions.isNonEmpty(dsFinalDedsAnnuity) Then
            strDedAnnuity = dsFinalDedsAnnuity.GetXml()
        End If

        ' -- Deductions for Lumpsum option
        If Not Session("FinalDeductionsLumpsum") Is Nothing Then
            dsFinalDedsLumpsum = DirectCast(Session("FinalDeductionsLumpsum"), DataSet)
        End If
        If HelperFunctions.isNonEmpty(dsFinalDedsLumpsum) Then
            strDedLumpsum = dsFinalDedsLumpsum.GetXml()
        End If
        'End - Manthan | 2016.04.22 | YRS-AT-2206 | Initialize variable and Storing all the deductions values selected in string in XML format

        'Session("CurrentForm") = "BS"
        'Exit Sub

        'NP:PS:2007.07.23 - Updating code to allow the possibility of settling on two options
        ''Call the BO Update method with 2 para & 4 null para 
        'If Session("string_POption") = "A" Then
        '    YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_FinalSettlementofBeneficiary( _
        '    Session("SP_Parameters_DeathBenefitOptionID"), _
        '    Session("SP_Parameters_AnnuityOption"), _
        '    Nothing, _
        '    Nothing, _
        '    Nothing, _
        '    Nothing, _
        '    l_string_OutputMessage)
        '    'Update_FinalSettlementofBeneficiary(string paramDeathBenefitOptionID, string paramAnnuityOption, double paramRolloverTaxable, double paramRolloverNonTaxable, string paramRolloverInstitutionID,double paramWithholdingPct, out string outpara_ErrorMessage )
        'Else
        '    'Refund beneficiary case .. Call the BO Update method with 6 paras 
        '    YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_FinalSettlementofBeneficiary( _
        '    Session("SP_Parameters_DeathBenefitOptionID"), _
        '    Session("SP_Parameters_AnnuityOption"), _
        '    Session("SP_Parameters_RolloverTaxable"), _
        '    Session("SP_Parameters_RolloverNonTaxable"), _
        '    Session("SP_Parameters_RolloverInstitutionID"), _
        '    Session("SP_Parameters_WithholdingPct"), _
        '    l_string_OutputMessage)
        'End If
        Try
            'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Passed three parameters for deduction selected for Annuity and Lumpsum option
            YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_FinalSettlementofBeneficiary( _
                Session("SP_Parameters_DeathBenefitOptionID_RP"), _
                Session("SP_Parameters_AnnuityOption_RP"), _
                Session("SP_Parameters_RolloverTaxable_RP"), _
                Session("SP_Parameters_RolloverNonTaxable_RP"), _
                Session("SP_Parameters_RolloverInstitutionID_RP"), _
                Session("SP_Parameters_WithholdingPct_RP"), _
                Session("SP_Parameters_DeathBenefitOptionID_SP"), _
                Session("SP_Parameters_AnnuityOption_SP"), _
                Session("SP_Parameters_RolloverTaxable_SP"), _
                Session("SP_Parameters_RolloverNonTaxable_SP"), _
                Session("SP_Parameters_RolloverInstitutionID_SP"), _
                Session("SP_Parameters_WithholdingPct_SP"), _
                strDedAnnuity, _
                strDedLumpsum, _
                l_string_OutputMessage, _
                Session("TotDeductionsLumpsumAmt"),
                Session("SP_Parameters_IsRolloverToOwnIRA_RP"), Session("SP_Parameters_IsRolloverToOwnIRA_SP"))  ' SB | 2016.11.25 | YRS-AT-3022 | Added session variables to store values for RolloverToOwnIRA options
            'End - Manthan | 2016.04.22 | YRS-AT-2206 | Passed three parameters for deduction selected for Annuity and Lumpsum option
            Session("CurrentForm") = "BS"
            If l_string_OutputMessage.Trim.Length = 0 Then
                'Start-SR:2014.05.23- Beneficiary RMDs
                dsRMDs_RP = SessionBeneficiaryRMDs.BeneficiaryRMDs_RP
                dsRMDs_SP = SessionBeneficiaryRMDs.BeneficiaryRMDs_SP

                If HelperFunctions.isNonEmpty(dsRMDs_RP) Then
                    YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.SaveBeneficiaryRMDs(Session("ParticipantEntityId"), Session("BS_SelectedOption_RP"), dsRMDs_RP)
                End If

                If HelperFunctions.isNonEmpty(dsRMDs_SP) Then
                    YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.SaveBeneficiaryRMDs(Session("ParticipantEntityId"), Session("BS_SelectedOption_SP"), dsRMDs_SP)
                End If
                'End-SR:2014.05.23- Beneficiary RMDs
                Process_BeneficiaryDetails()
                'BindSearchGrid()
                MessageBox.Show(PlaceHolder1, "Beneficiary Settlement", "Beneficiary Settlement Successful!", MessageBoxButtons.OK)
            Else
                MessageBox.Show(PlaceHolder1, "Beneficiary Settlement-Error", "Beneficiary Settlement failed due to error!" + LINEBREAK + l_string_OutputMessage, MessageBoxButtons.Stop)
            End If
            Session("Success_BRScreen") = False 'SR:2014.07.25 - BT 2615:Second Beneficiary get settled automatically after settling first Beneficiary

        Catch ex As Exception
            Session("Success_BRScreen") = False 'SR:2014.07.25 - BT 2615:Second Beneficiary get settled automatically after settling first Beneficiary
            HelperFunctions.LogException("Beneficiary Settlement --> Update_FinalSettlement", ex)
            Throw
        End Try
    End Sub

#End Region

#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        StoreLocalVariablesToCache()
        ViewState("temp") = String.Empty
        Return MyBase.SaveViewState()
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        InitializeLocalVariablesFromCache()
        ViewState("temp") = String.Empty
        MyBase.LoadViewState(savedState)
    End Sub

    Private Sub InitializeLocalVariablesFromCache()
        'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
        l_dataset_SearchResults = Session("BSF_l_dataset_SearchResults")
        l_dataset_ActiveBeneficiaries = Session("BSF_l_dataset_ActiveBeneficiaries")
        l_dataset_RetiredBeneficiaries = Session("BSF_l_dataset_RetiredBeneficiaries")
        l_dataset_ActiveSettlementOption_RetirementPlan = Session("BSF_l_dataset_ActiveSettlementOption_RetirementPlan")
        l_dataset_ActiveSettlementOption_SavingsPlan = Session("BSF_l_dataset_ActiveSettlementOption_SavingsPlan")
        l_dataset_RetiredSettlementOption_RetirementPlan = Session("BSF_l_dataset_RetiredSettlementOption_RetirementPlan")
        l_dataset_RetiredSettlementOption_SavingsPlan = Session("BSF_l_dataset_RetiredSettlementOption_SavingsPlan")
    End Sub
    Private Sub StoreLocalVariablesToCache()
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("BSF_l_dataset_SearchResults") = l_dataset_SearchResults
        Session("BSF_l_dataset_ActiveBeneficiaries") = l_dataset_ActiveBeneficiaries
        Session("BSF_l_dataset_RetiredBeneficiaries") = l_dataset_RetiredBeneficiaries
        Session("BSF_l_dataset_ActiveSettlementOption_RetirementPlan") = l_dataset_ActiveSettlementOption_RetirementPlan
        Session("BSF_l_dataset_ActiveSettlementOption_SavingsPlan") = l_dataset_ActiveSettlementOption_SavingsPlan
        Session("BSF_l_dataset_RetiredSettlementOption_RetirementPlan") = l_dataset_RetiredSettlementOption_RetirementPlan
        Session("BSF_l_dataset_RetiredSettlementOption_SavingsPlan") = l_dataset_RetiredSettlementOption_SavingsPlan
    End Sub
#End Region

    'This function handles the selected index change event of the Benefit Options datagrid.
    'We re-bind the datasource with the datagrid. The image button is changed from the DataBound Event

    Public Sub SelectOnlyOne(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridActiveBenefitOptions_RetirementPlan.SelectedIndexChanged, DataGridActiveBenefitOptions_SavingsPlan.SelectedIndexChanged, DataGridRetiredBenefitOptions_RetirementPlan.SelectedIndexChanged, DataGridRetiredBenefitOptions_SavingsPlan.SelectedIndexChanged
        Dim dg As DataGrid = DirectCast(sender, DataGrid)  'Changed from CType to Directcast by SR:2010.06.17 for migration
        Dim dv As DataView
        Dim filterCondition As String = "BeneficiaryId = '"

        Try
            If Me.TabStripBeneficiarySettlement.SelectedIndex = Me.ACTIVE_BENEFICIARY_TAB_INDEX Then
                filterCondition &= Me.l_dataset_ActiveBeneficiaries.Tables(0).Rows(DataGridActiveBeneficiaries.SelectedIndex)("UniqueId").ToString()
            ElseIf Me.TabStripBeneficiarySettlement.SelectedIndex = Me.RETIRED_BENEFICIARY_TAB_INDEX Then
                filterCondition &= Me.l_dataset_RetiredBeneficiaries.Tables(0).Rows(DataGridRetiredBeneficiaries.SelectedIndex)("UniqueId").ToString()
            End If
            filterCondition &= "'"
            Select Case dg.ID
                Case Me.DataGridActiveBenefitOptions_RetirementPlan.ID
                    dv = l_dataset_ActiveSettlementOption_RetirementPlan.Tables(0).DefaultView
                Case Me.DataGridActiveBenefitOptions_SavingsPlan.ID
                    dv = l_dataset_ActiveSettlementOption_SavingsPlan.Tables(0).DefaultView
                Case Me.DataGridRetiredBenefitOptions_RetirementPlan.ID
                    dv = l_dataset_RetiredSettlementOption_RetirementPlan.Tables(0).DefaultView
                Case Me.DataGridRetiredBenefitOptions_SavingsPlan.ID
                    dv = l_dataset_RetiredSettlementOption_SavingsPlan.Tables(0).DefaultView
                Case Else
                    MessageBox.Show(PlaceHolder1, "Error", "Error calling SelectOnlyOne", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
            End Select
            dv.RowFilter = filterCondition
            BindGrid(dg, dv)
            InitializeDataGridSelectedIndex(dg, dv)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DataGridSearchResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridSearchResults.ItemDataBound
        e.Item.Cells(8).Visible = False 'Shashi Shekhar:2010-02-16 :Hide IsArchived Field in grid
        e.Item.Cells(9).Visible = False 'Shashi Shekhar:2010-02-16 :Hide StatusType Field in grid
        e.Item.Cells(10).Visible = False 'Shashi Shekhar:2010-12-08 :Hide FundIdNo Field in grid This is taken For YRS 5.0-450, BT-643
    End Sub
    'DK:2012.12.07  - BT - 1433 : Problem in Death Settlement.
    Private Sub ClearSessions()

        Session("SP_Parameters_DeathBenefitOptionID_RP") = Nothing
        Session("SP_Parameters_AnnuityOption_RP") = Nothing
        Session("SP_Parameters_RolloverTaxable_RP") = Nothing
        Session("SP_Parameters_RolloverNonTaxable_RP") = Nothing
        Session("SP_Parameters_RolloverInstitutionID_RP") = Nothing
        Session("SP_Parameters_WithholdingPct_RP") = Nothing

        Session("SP_Parameters_DeathBenefitOptionID_SP") = Nothing
        Session("SP_Parameters_AnnuityOption_SP") = Nothing
        Session("SP_Parameters_RolloverTaxable_SP") = Nothing
        Session("SP_Parameters_RolloverNonTaxable_SP") = Nothing
        Session("SP_Parameters_RolloverInstitutionID_SP") = Nothing
        Session("SP_Parameters_WithholdingPct_SP") = Nothing

        Session("g_DeathDate") = Nothing
        Session("ParticipantEntityId") = Nothing
        Session("BS_SelectedOption_RP") = Nothing
        Session("BS_SelectedOption_SP") = Nothing
        Session("SP_Parameters_AnnuityOption_RP") = Nothing
        Session("SP_Parameters_AnnuityOption_SP") = Nothing
        'SP:2013.04.12 :YRS 5.0-1990 -start
        Session("SelectedActiveBeneficiaryUniqueID") = Nothing
        Session("SelectedRetiredBeneficiaryUniqueID") = Nothing
        'SP:2013.04.12 :YRS 5.0-1990 -end
        'Start-SR:2014.05.23- Beneficiary RMDs
        'Session("RMD_PersStatusType") = Nothing
        SessionBeneficiaryRMDs.DeceasedFundStatus = Nothing
        SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
        SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
        'End-SR:2014.05.23- Beneficiary RMDs
        'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Clearing session value
        Session("FinalDeductionsLumpsum") = Nothing
        Session("FinalDeductionsAnnuity") = Nothing
        Session("TotDeductionsLumpsumAmt") = Nothing
        'End - Manthan | 2016.04.22 | YRS-AT-2206 | Clearing session value
        ViewState("ReasonForRestriction") = Nothing
        ViewState("DeathSettlementAllowedForRMDEligibleParticipants") = Nothing
        ViewState("FundEventID") = Nothing
    End Sub
    ' START : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of participant need to regenerate then show warning message
    Private Sub CheckPreviousYearRMDStatus()
        RegenerateRMD = False
        If HelperFunctions.isNonEmpty(FundEventID) Then
            RegenerateRMD = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyActions_IsRMDRegenerateRequired(FundEventID)
        End If
    End Sub
    Private Function ShowRegenerateRMDWarning()
        DivMessage.Visible = True

        'START: SB | 2017.08.02 | YRS-AT-3324 | Displaying the message if Death settlement is allowed for RMD eligbile participants whose RMD is not generated
        If HelperFunctions.isNonEmpty(Me.ReasonForRestriction) Then
            DivMessage.InnerHtml = Me.ReasonForRestriction
            ButtonSettleActiveBeneficiary.Enabled = False
            'START: SB | 2017.10.12 | YRS-AT-3324 | On selection of RetireBeneficiaires list tab, hide the div message ( ReasonForRestriction error message is related to Non Retired money)
            If (TabStripBeneficiarySettlement.SelectedIndex = PARTICIPANT_LIST) Then
                DivMessage.InnerHtml = ""
                DivMessage.Attributes.Add("display", "none")
            ElseIf (TabStripBeneficiarySettlement.SelectedIndex = ACTIVE_BENEFICIARY_TAB_INDEX) Then
                DivMessage.InnerHtml = Me.ReasonForRestriction
                DivMessage.Attributes.Add("display", "block")
            ElseIf (TabStripBeneficiarySettlement.SelectedIndex = RETIRED_BENEFICIARY_TAB_INDEX) Then
                DivMessage.InnerHtml = ""
                DivMessage.Attributes.Add("display", "none")
            End If
            'DivMessage.Attributes.Add("display", "block")
            'END: SB | 2017.10.12 | YRS-AT-3324 | On selection of RetireBeneficiaires list tab, hide the div message ( ReasonForRestriction error message is related to Non Retired money)
            Exit Function
        End If
        'END: SB | 2017.08.02 | YRS-AT-3324 | Displaying the message if Death settlement is allowed for RMD eligbile participants whose RMD is not generated

        If String.IsNullOrEmpty(DivMessage.InnerHtml.Trim) Then
            DivMessage.InnerHtml = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATHSETTLEMENT_RMDREGENERATIONWARNING
            ButtonSettleActiveBeneficiary.Enabled = False
            DivMessage.Attributes.Add("display", "block")
        End If
    End Function
    ' END : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of participant need to regenerate then show warning message

End Class
