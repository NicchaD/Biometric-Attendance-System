'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	UpdateBeneficiaries.aspx.vb
' Cache-Session     :   Vipul 03Feb06
'
'Name:Preeti Date:8thFeb06 IssueId:2071 Reason:Non-human beneficiaries should not require DOB.
'Name:Preeti Date:15thFeb06 IssueId: YRST-2053 Error message changes
'Nikunj Patel			26-Jun-2007			Added code to pull Beneficiary information from the database for the new Beneficiary Types
' Description:
' This page handles the Add/Update Beneficiary operations for the Active Participant

'Modification History
'*********************************************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Ashutosh Patil     06-Jun-2007     YREN-3490
'Ashutosh Patil     21-Jun-2007     YREN-3490 Self Adding as a Benificiary check and addition of existing Benificiary check.
'Ashutosh Patil     10-Jul-2007     YREN-3490 Benificiary Check
'Ashutosh Patil     12-Jul-2007     YREN-3490 Checking if SSNo is empty or not
'Nikunj Patel       27-Aug-2007     Added code to take the default PlanType based on Beneficiary Type Code.
'                   31-Aug-2007     Bug reopened for the same issue but for a different set of conditions
'                   13-Sep-2007     Also check for Page.IsValid before saving
'Aparna Samala      19-Sep-2007     To avoid error Msg "Specified argument was out of the range of valid values. Parameter name: Member"
'Nikunj Patel       26-Sep-2007     Caliing page.Validate before calling Page.IsValid to save. related to 2007.09.13.
'Mohammed Hafiz     27-Mar-2008     YRPS-4704 - Should not Allow the Beneficiary code of SP if person is single
'Nikunj Patel       10-Sep-2008     BT-550 - Changing code to avoid beneficiaries with the same tax number being defined for the same participant.
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar: 26-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Shashi             03 Mar. 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Priya				26-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
'Anudeep            07-Feb-2013     YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
'Anudeep            13-Jun-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            10-Jul-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            10.jul.2013     Bt-2084:Beneficiaries add and update SSN validation change
'Anudeep            15.jul.2013     Bt-2084:Beneficiaries add and update SSN validation change
'Anudeep            29.jul.2013     Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
'Sanjay R.          2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep            2013.10.21      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2013.11.06      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep A          2014.02.13      BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
'Anudeep            2014.02.16      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.02.19      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.02.20      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Shashank           2014.02.24      BT-2316\YRS 5.0-2262 : Spousal Consent date in YRS
'Anudeep            2014.05.26      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.06.11      BT:2554:YRS 5.0-2375 : Notes display if you have spaces in name.
'Shashank Patel     2014.11.20      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
'Anudeep            2015.05.06      BT:2824:YRS 5.0-2499:Web Service for Password Rewrite
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.12      YRS-AT-2588: implement some basic telephone number validation Rules
'Santosh Bura       2016.07.07      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiaries SSN (TrackIT 19856)
'Manthan Rajguru    2016.07.18      YRS-AT-2919 -  YRS Enh: Beneficiary Details - nonPerson - should allow optional Date
'Manthan Rajguru    2016.07.27      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru    2016.08.02      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru    2016.09.08      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru 	2017.12.04 		YRS-AT-3756	   YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'Santosh Bura       2017.12.14      YRS-AT-3756 -  YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) .
'Manthan Rajguru    2018.05.03      YRS-AT-3941 - YRS enh: change "phony SSN" beneficiary label to "placeholder SSN" (TrackIT 33287)
'Santosh Bura       2018.05.08      YRS-AT-3353 -  YRS bug: typos in MessageBox for AddBeneficiary.aspx.vb
'Vinayan C          2018.06.18      YRS-AT-3996 -  YRS Bug: Change Beneficiary error message " to itself" to "of himself/herself".
'*********************************************************************************************************************

Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports System.Data.SqlClient
Public Class UpdateBeneficiaries
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateBeneficiaries.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtBirthDate As YMCAUI.DateUserControl
    Protected WithEvents PopcalendarRecDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSSNNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbRelation As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtPercentage As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbLevel As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Protected WithEvents AddressWebUserControl1 As AddressUserControlNew
    'Protected WithEvents RFVBirthDate As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents custBirthdt As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents RequiredFirstNamevalidator As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator8 As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents lblAddressChange As System.Web.UI.WebControls.Label
    Protected WithEvents lnkParticipantAddress As System.Web.UI.WebControls.LinkButton
    'SP 2014.11.20 -BT-2310\YRS 5.0-2255-Start
    Protected WithEvents rbdtnlstBeneficiaryType As RadioButtonList
    Protected WithEvents pnlRepresenattiveDetails As Panel
    Protected WithEvents ddlRepSalutaionCode As DropDownList
    Protected WithEvents txtRepFirstName As TextBox
    Protected WithEvents txtRepLastName As TextBox
    Protected WithEvents txtRepTelephoneNo As TextBox
    Protected WithEvents LabelLastName As Label
    Protected WithEvents LabelFirstName As Label
    Protected WithEvents lblRepresentative As Label
    Protected WithEvents vdlSummary As ValidationSummary
    Protected WithEvents DivMainMessage As HtmlGenericControl
    'SP 2014.11.20 -BT-2310\YRS 5.0-2255-End
    Protected WithEvents lblBirthDate As Label 'Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Declaring label control

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Added Edit Button,Table Row and Drop Down to find reason for SSN change
    Protected WithEvents ButtonActiveBeneficiariesSSNEdit As System.Web.UI.WebControls.Button
    Protected WithEvents trSSNChangeReason As HtmlTableRow
    Protected WithEvents ddlBeneficiariesSSNChangeReason As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rqvBenefSSNoChangeReason As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents CompareOldSSNvalidator As System.Web.UI.WebControls.CompareValidator
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Added Edit Button,Table Row and Drop Down to find reason for SSN change
    Protected WithEvents chkPhonySSN As System.Web.UI.WebControls.CheckBox 'Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Declare checkbox control
    'START: MMR | 2017.12.04 | YRS-AT-3756 | Declared Dropdown and label control
    Protected WithEvents ddlDeceasedBeneficiary As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblDeceasedBeneficiary As System.Web.UI.WebControls.Label
    'END: MMR | 2017.12.04 | YRS-AT-3756 | Declared Dropdown and label control
    Private Const Phony_SSN_SYSTEM_GENERATED As String = "P********" 'Manthan Rajguru | 2016.07.29 |YRS-AT-2560 | Declared constant to show system generated phony SSN


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'SP 2014.11.27      BT-2310\YRS 5.0-2255 -Start
    Public ReadOnly Property RelationShips As DataSet
        Get
            Return Session("dataset_RelationShips")
        End Get
    End Property
    'Start - Manthan Rajguru | 2016.07.29 | YRS-AT-2560 | Declared property to store error message value
    Public Property InvalidSSNErrMsgShown() As Boolean
        Get
            If Not (ViewState("InvalidSSNErrMsgShown")) Is Nothing Then
                Return (CType(ViewState("InvalidSSNErrMsgShown"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("InvalidSSNErrMsgShown") = Value
        End Set
    End Property
    'End - Manthan Rajguru | 2016.07.29 | YRS-AT-2560 | Declared property to store error message value
    'START: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to set deceased beneficiary exists or not
    Public Property IsDeceasedBeneficiaryExists() As Boolean
        Get
            If Not (ViewState("IsDeceasedBeneficiaryExists")) Is Nothing Then
                Return (CType(ViewState("IsDeceasedBeneficiaryExists"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsDeceasedBeneficiaryExists") = Value
        End Set
    End Property
    'END: MMR | 2017.01.04 | YRS-AT-3756 | Declared property to set deceased beneficiary exists or not

    'START: MMR | 2017.01.04 | YRS-AT-3756 | Declared property to store deceased beneficiary details
    Public Property DeceasedBeneficiary As DataSet
        Get
            If Not (Session("DeceasedBeneficiary")) Is Nothing Then
                Return (CType(Session("DeceasedBeneficiary"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("DeceasedBeneficiary") = Value
        End Set
    End Property
    'END: MMR | 2017.01.04 | YRS-AT-3756 | Declared property to store deceased beneficiary details

    'START: MMR | 2017.01.04 | YRS-AT-3756 | Declared property to store dropdown selected value
    Public Property DropdownSelectedValue As String
        Get
            If Not (ViewState("DropdownSelectedValue")) Is Nothing Then
                Return (CType(ViewState("DropdownSelectedValue"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("DropdownSelectedValue") = Value
        End Set
    End Property
    'END: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to store dropdown selected value

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Moved to Common class Enum Files
    'Public Enum EnumBeneficiaryTypes
    '    HBENE
    '    NHBENE

    'End Enum
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Moved to Common class Enum Files
    'SP  2014.11.27       BT-2310\YRS 5.0-2255 -End   
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If


        Try

            '----------------------------------------------------------------------------------------------------------------
            'Shashi Shekhar: 26-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Participant Information - Add/Update Beneficiary"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()

            End If
            '-----------------------------------------------------------------------------------------------------------------

            ' If Me.txtPercentage.Text <> "" Then
            Me.txtPercentage.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.txtPercentage.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            'Me.txtBirthDate.Attributes.Add("onchange", "javascript:ValidateBirthDate();")
            ' End If

            'Name:Preeti Date:15thFeb06 IssueId: YRST-2053 Error message changes - Default Message START 
            RangeValidator1.ErrorMessage = "Beneficiary must equal 100 percent."
            'Name:Preeti Date:15thFeb06 IssueId: YRST-2053 Error message changes END 

            If Not IsPostBack Then

                Dim l_string_PersId As String
                l_string_PersId = Session("PersId")
                Dim l_dataset_RelationShips As New DataSet
                Dim l_dataset_BenefitGroups As New DataSet
                Dim l_dataset_BeneficiaryLvl As New DataSet
                Dim l_dataset_DeathBeneficiary As New DataSet
                Dim deceasedBeneficiary As New DataSet 'MMR | 2017.12.04 | YRS-AT-3756 | Declared object variable
                Dim l_datarow_relationship As DataRow()
                Dim dvLvl As DataView
                Dim beneficiaryType As String 'MMR | 2017.12.04 | YRS-AT-3756 | Declared variable to store beneficiary type 'Member' or 'retiree'

                ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Hiding the reason table row at page load and binding the reasons in drop down
                ShowOrHideEditSSN(False)
                BindReasonDropDown()
                ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Hiding the reason table row at page load and binding the reasons in drop down

                l_dataset_RelationShips = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.getRelationShips()
                l_dataset_BenefitGroups = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.getBenefitGroups()
                l_dataset_BeneficiaryLvl = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.getBenefitLevels()
                l_dataset_DeathBeneficiary = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.getBenefitTypes()
                Session("dataset_RelationShips") = l_dataset_RelationShips
                Me.txtBirthDate.RequiredDate = True
                'Start:Anudeep:07.02.2013 : Changes made to show active relationships and already selected inactive relationships


                'End:Anudeep:07.02.2013 : Changes made to show active relationships and already selected inactive relationships
                cmbGroup.DataSource = l_dataset_BenefitGroups
                cmbGroup.DataTextField = "Description"
                cmbGroup.DataValueField = "Code"
                cmbGroup.DataBind()

                'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Commented below code for showing the beneficiary level option with respect to the session flag
                'cmbLevel.DataSource = l_dataset_BeneficiaryLvl
                'cmbLevel.DataTextField = "Description"
                'cmbLevel.DataValueField = "Code"
                'cmbLevel.DataBind()

                cmbType.DataSource = l_dataset_DeathBeneficiary
                cmbType.DataValueField = "Type"
                cmbType.DataTextField = "Description"
                cmbType.DataBind()


                BindSalutationDropDown()  'SP 2014.11.20 BT-2310\YRS 5.0-2255              
                'START: MMR | 2017.12.04 | YRS-AT-3756 | Getting deceased beneficiary details
                If Not Session("PersId") Is Nothing Then
                    beneficiaryType = "MEMBER"
                    deceasedBeneficiary = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetDeceasedBeneficiary(Session("PersId"), beneficiaryType)
                    Me.DeceasedBeneficiary = deceasedBeneficiary
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | Getting deceased beneficiary details

                If Session("Flag") = "AddBeneficiaries" Then
                    If Not l_dataset_RelationShips Is Nothing Then
                        'SP 2014.11.27  BT-2310\YRS 5.0-2255 - Start
                        BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.HBENE)
                        EnableDisableRepresentativeDetails(False)
                        'Below code is commented for YRS 5.0-2255 moved into method "BindRelationShipsByFilter"
                        'l_datarow_relationship = l_dataset_RelationShips.Tables(0).Select("Active = True")
                        'If l_datarow_relationship.Length > 0 Then
                        '    cmbRelation.DataSource = l_datarow_relationship.CopyToDataTable()
                        '    cmbRelation.DataTextField = "Description"
                        '    cmbRelation.DataValueField = "CodeValue"
                        'Else
                        '    cmbRelation.DataSource = Nothing
                        'End If
                        'cmbRelation.DataBind()

                        'SP 2014.11.20 BT-2310\YRS 5.0-2255 - End
                    End If

                    'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Added below code to display only level 1 option in the screen for adding a new benefeiciary
                    dvLvl = l_dataset_BeneficiaryLvl.Tables(0).DefaultView
                    dvLvl.RowFilter = "Code='LVL1'"
                    cmbLevel.DataSource = dvLvl
                    cmbLevel.DataTextField = "Description"
                    cmbLevel.DataValueField = "Code"
                    cmbLevel.DataBind()

                    'cmbRelation.Items.Insert(0, "") 'SP 2014.11.20 BT-2310\YRS 5.0-2255
                    cmbType.Items.Insert(0, "")
                    cmbLevel.Items.Insert(0, "")
                    cmbGroup.Items.Insert(0, "")
                    Me.AddressWebUserControl1.LoadAddressDetail(Nothing)

                    ' // START : SB | 07/07/2016 | YRS-AT-2382 |Add beneficiary make reason and edit button  invisible
                    trSSNChangeReason.Visible = False
                    ButtonActiveBeneficiariesSSNEdit.Visible = False
                    rqvBenefSSNoChangeReason.Enabled = False
                    txtSSNNo.Enabled = True
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Add beneficiary make reason and edit button  invisible

                    'START: MMR | 2017.12.04 | YRS-AT-3756 | Displaying dropdown based on deceased beneficairy existence and binding dropdown if beneficairy exists                    
                    If HelperFunctions.isNonEmpty(Me.DeceasedBeneficiary) Then
                        BindDeceasedBeneficiaryDropDown(Me.DeceasedBeneficiary, Me.DropdownSelectedValue)
                        Me.IsDeceasedBeneficiaryExists = True
                    End If
                    ShowHideDeceasedBeneficiaryDropDown(Me.IsDeceasedBeneficiaryExists)
                    'END: MMR | 2017.12.04 | YRS-AT-3756 | Displaying dropdown based on deceased beneficairy existence and binding dropdown if beneficairy exists
                End If
                'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                Me.AddressWebUserControl1.EnableControls = True
                Me.AddressWebUserControl1.Rights = ""
                Me.AddressWebUserControl1.IsPrimary = 1

                ' Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting Visible and Enabled property of checkbox at the time of Adding beneficiary
                chkPhonySSN.Visible = True
                chkPhonySSN.Enabled = True
                ' End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting Visible and Enabled property of checkbox at the time of Adding beneficiary


                If Session("Flag") <> "AddBeneficiaries" Then


                    Dim Beneficiaries As DataSet
                    Dim drRows As DataRow()
                    Dim drUpdated As DataRow
                    Dim dtAddress As DataTable
                    Dim drAddressRow As DataRow
                    Dim drRow As DataRow()
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    If Session("BeneficiaryAddress") Is Nothing Then
                        dtAddress = Address.CreateAddressDatatable()
                    Else
                        dtAddress = Session("BeneficiaryAddress")
                    End If



                    Beneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)

                    If Not Request.QueryString("UniqueID") Is Nothing Then
                        If HelperFunctions.isNonEmpty(Beneficiaries) Then
                            drRows = Beneficiaries.Tables(0).Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
                            If drRows.Length > 0 Then
                                drUpdated = drRows(0)
                            End If
                        End If
                        If HelperFunctions.isNonEmpty(dtAddress) And Not String.IsNullOrEmpty(drUpdated("TaxID").ToString()) Then
                            drRow = dtAddress.Select("BenSSNo='" & drUpdated("TaxID") & "'")
                        End If
                        If HelperFunctions.isNonEmpty(dtAddress) And (drRow Is Nothing OrElse drRow.Length = 0) Then
                            drRow = dtAddress.Select("guiEntityId='" & Request.QueryString("UniqueID") & "'")
                        End If
                        If HelperFunctions.isNonEmpty(dtAddress) And (drRow Is Nothing OrElse drRow.Length = 0) Then
                            drRow = dtAddress.Select("BeneID='" & Request.QueryString("UniqueID") & "'")
                        End If

                    End If
                    'If Not Request.QueryString("Index") Is Nothing Then
                    If Not Request.QueryString("NewID") Is Nothing Then
                        If HelperFunctions.isNonEmpty(Beneficiaries) Then
                            drRows = Beneficiaries.Tables(0).Select("NewId='" & Request.QueryString("NewID") & "'")
                            If drRows.Length > 0 Then
                                drUpdated = drRows(0)
                            End If
                        End If

                        If HelperFunctions.isNonEmpty(dtAddress) And Not String.IsNullOrEmpty(drUpdated("TaxID").ToString()) Then
                            drRow = dtAddress.Select("BenSSNo='" & drUpdated("TaxID") & "'")
                        End If
                        If HelperFunctions.isNonEmpty(dtAddress) Then
                            drRow = dtAddress.Select("NewId='" & Request.QueryString("NewID") & "'")
                        End If
                    End If

                    If Not drUpdated Is Nothing Then

                        'If HelperFunctions.isNonEmpty(dtAddress) And Not String.IsNullOrEmpty(drUpdated("TaxID").ToString()) Then
                        '    drRow = dtAddress.Select("BenSSNo='" & drUpdated("TaxID") & "'")
                        'End If

                        Me.AddressWebUserControl1.LoadAddressDetail(drRow)

                        'SP 2014.11.27 BT-2310\YRS 5.0-2255 -Start
                        If IsRelationshipTypeIsHumanBeneficiary(RelationShips, drUpdated) Then
                            EnableDisableRepresentativeDetails(False)
                            BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.HBENE)
                            EnableFirstName(True)
                            ChangeBeneficiaryNameCaption(False)
                            ShowOrHideForHumans(False)   ' // SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility for Human and Non Humans
                        Else
                            EnableDisableRepresentativeDetails(True)
                            BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.NHBENE)
                            rbdtnlstBeneficiaryType.SelectedIndex = 1
                            'Start - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                            'EnableBirthDateControls(False)
                            Me.txtBirthDate.RequiredDate = False
                            'End - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                            EnableFirstName(False)
                            ChangeBeneficiaryNameCaption(True)
                            ShowOrHideForHumans(True)   ' // SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility for Human and Non Humans
                        End If

                        'Below code is commented  & moved into above function "BindRelationShipsByFilter" for YRS 5.0-2255
                        'If Not l_dataset_RelationShips Is Nothing Then
                        '    If Not String.IsNullOrEmpty(drUpdated("Rel").ToString()) Then
                        '        l_datarow_relationship = l_dataset_RelationShips.Tables(0).Select("Active = True OR CodeValue = '" + drUpdated("Rel").ToString() + "'")
                        '    Else
                        '        l_datarow_relationship = l_dataset_RelationShips.Tables(0).Select("Active = True")
                        '    End If
                        '    If l_datarow_relationship.Length > 0 Then
                        '        cmbRelation.DataSource = l_datarow_relationship.CopyToDataTable()
                        '        cmbRelation.DataTextField = "Description"
                        '        cmbRelation.DataValueField = "CodeValue"
                        '    Else
                        '        cmbRelation.DataSource = Nothing
                        '    End If
                        '    cmbRelation.DataBind()
                        'End If
                        'SP 2014.11.27 BT-2310\YRS 5.0-2255 -End

                        'txtFirstName.Text = Session("Name")
                        'txtLastName.Text = Session("Name2")
                        'txtSSNNo.Text = Session("TaxID")
                        'cmbRelation.SelectedValue = Session("Rel")
                        'txtBirthDate.Text = Session("Birthdate")
                        'txtPercentage.Text = Session("Pct")
                        'cmbGroup.SelectedValue = Session("Groups")
                        'If cmbGroup.SelectedValue = "CONT" Then
                        '    cmbLevel.Enabled = True
                        '    cmbLevel.SelectedValue = Session("Lvl")
                        'Else
                        '    cmbLevel.Items.Insert(0, "")
                        '    cmbLevel.SelectedIndex = 0
                        '    cmbLevel.Enabled = False
                        'End If
                        'txtPercentage.Text = Session("Pct")
                        'cmbType.SelectedValue = Session("Type").ToString.ToUpper.Trim()


                        Dim bFlag As Boolean = False

                        If Not IsDBNull(drUpdated("Name")) Then
                            txtFirstName.Text = drUpdated("Name")
                        End If
                        If Not IsDBNull(drUpdated("Name2")) Then
                            txtLastName.Text = drUpdated("Name2")
                        End If
                        If Not IsDBNull(drUpdated("TaxID")) Then
                            txtSSNNo.Text = drUpdated("TaxID")
                            Session("TaxID") = drUpdated("TaxID")
                            '// START : SB | 07/07/2016 | YRS-AT-2382 | Assigning Old SSN to compare validator
                            CompareOldSSNvalidator.ValueToCompare = Convert.ToString(drUpdated("TaxID"))
                            '// END : SB | 07/07/2016 | YRS-AT-2382 | Assigning Old SSN to compare validator
                        End If
                        If Not IsDBNull(drUpdated("Rel")) Then
                            'AA:19.02.2014: BT:2306:YRS 5.0-2251 : Added tostring to get the actual vaue
                            cmbRelation.SelectedValue = drUpdated("Rel").ToString().Trim()
                        End If
                        If Not IsDBNull(drUpdated("Birthdate")) Then
                            txtBirthDate.Text = drUpdated("Birthdate")
                        End If
                        If Not IsDBNull(drUpdated("Pct")) Then
                            txtPercentage.Text = drUpdated("Pct")
                        End If
                        If Not IsDBNull(drUpdated("Groups")) Then
                            cmbGroup.SelectedValue = drUpdated("Groups")
                        End If
                        If Not IsDBNull(drUpdated("Pct")) Then
                            txtPercentage.Text = drUpdated("Pct")
                        End If
                        'EndAnudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

                        '2014.11.24 BT-2310\YRS 5.0-2255 -Start
                        If Not IsDBNull(drUpdated("RepFirstName")) Then
                            txtRepFirstName.Text = drUpdated("RepFirstName")
                        End If
                        If Not IsDBNull(drUpdated("RepLastName")) Then
                            txtRepLastName.Text = drUpdated("RepLastName")
                        End If
                        If Not IsDBNull(drUpdated("RepSalutation")) Then
                            ddlRepSalutaionCode.SelectedValue = drUpdated("RepSalutation").ToString.Trim
                        End If
                        If Not IsDBNull(drUpdated("RepTelephone")) Then
                            txtRepTelephoneNo.Text = drUpdated("RepTelephone")
                        End If
                        '2014.11.24 BT-2310\YRS 5.0-2255 -End

                        'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Added below code to display only level 1 and already existing Level2 or level 3 option in the screen for updating benefeiciary
                        dvLvl = l_dataset_BeneficiaryLvl.Tables(0).DefaultView
                        If Not String.IsNullOrEmpty(drUpdated("Lvl").ToString()) AndAlso drUpdated("Lvl").ToString().Trim() <> "LVL1" Then
                            dvLvl.RowFilter = "Code IN ('LVL1','" + drUpdated("Lvl").ToString() + "')"
                        Else
                            dvLvl.RowFilter = "Code = 'LVL1'"
                        End If

                        cmbLevel.DataSource = dvLvl
                        cmbLevel.DataTextField = "Description"
                        cmbLevel.DataValueField = "Code"
                        cmbLevel.DataBind()
                        If cmbGroup.SelectedValue = "CONT" Then
                            cmbLevel.Enabled = True
                            cmbLevel.SelectedValue = drUpdated("Lvl")
                        Else
                            cmbLevel.Items.Insert(0, "")
                            cmbLevel.SelectedIndex = 0
                            cmbLevel.Enabled = False
                        End If
                        cmbType.SelectedValue = drUpdated("BeneficiaryTypeCode").ToString.ToUpper.Trim() 'by Aparna 14/09/2007
                        DisableControlForSettledBeneficiaryWithMessage(drUpdated) '2014.11.24 BT-2310\YRS 5.0-2255 - Disable if beneficiary is settled

                        'START: MMR | 2017.12.04 | YRS-AT-3756 | Assigning value to dropdown
                        If HelperFunctions.isNonEmpty(Me.DeceasedBeneficiary) Then
                            If Not IsDBNull(drUpdated("DeletedBeneficiaryID")) Then
                                Me.DropdownSelectedValue = drUpdated("DeletedBeneficiaryID")
                                BindDeceasedBeneficiaryDropDown(Me.DeceasedBeneficiary, Me.DropdownSelectedValue)
                            Else
                                BindDeceasedBeneficiaryDropDown(Me.DeceasedBeneficiary, Me.DropdownSelectedValue)
                            End If
                            Me.IsDeceasedBeneficiaryExists = True
                        End If
                        ShowHideDeceasedBeneficiaryDropDown(Me.IsDeceasedBeneficiaryExists)
                        'END: MMR | 2017.12.04 | YRS-AT-3756 | Assigning value to dropdown
                    End If
                    'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    'txtFirstName.Text = Replace(Session("Name"), "&nbsp;", "")
                    'txtLastName.Text = Session("Name2")
                    'txtSSNNo.Text = Replace(Session("TaxID"), "&nbsp;", "")
                    'cmbRelation.SelectedValue = Session("Rel")
                    'txtBirthDate.Text = Session("Birthdate")
                    'txtPercentage.Text = Session("Pct")
                    'cmbGroup.SelectedValue = Session("Groups")
                    'If cmbGroup.SelectedValue = "CONT" Then
                    '    cmbLevel.Enabled = True
                    '    cmbLevel.SelectedValue = Session("Lvl")
                    'Else
                    '    cmbLevel.Items.Insert(0, "")
                    '    cmbLevel.SelectedIndex = 0
                    '    cmbLevel.Enabled = False
                    'End If


                    'txtPercentage.Text = Session("Pct")
                    'cmbType.SelectedValue = Session("Type")
                    ' Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting Visible and Enabled property of checkbox at the time of updating beneficiary
                    chkPhonySSN.Visible = False
                    chkPhonySSN.Enabled = False
                    ' End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting Visible and Enabled property of checkbox at the time of updating beneficiary
                End If
                Session("PhonySSNo") = Nothing
            End If

            'Added By Ashutosh Patil as on 05-Feb-07
            'YREN-3049
            'SP 2014.11.20 BT-2310\YRS 5.0-2255 -Start
            'Commented below code because its already handle based on beneficiary type selection
            'If Session("Flag") = "EditBeneficiaries" And (cmbRelation.SelectedValue = "TR" Or cmbRelation.SelectedValue = "IN" Or cmbRelation.SelectedValue = "ES") Then
            'If Session("Flag") = "EditBeneficiaries" Then
            '    If (cmbRelation.SelectedValue = "TR" Or cmbRelation.SelectedValue = "IN" Or cmbRelation.SelectedValue = "ES") Then
            '        RequiredFirstNamevalidator.Visible = False
            '        RequiredFirstNamevalidator.Enabled = False

            '        Call DisableBirthDate()
            '    Else
            '        RequiredFirstNamevalidator.Visible = True
            '        RequiredFirstNamevalidator.Enabled = True
            '    End If
            'End If
            'SP 2014.11.20 BT-2310\YRS 5.0-2255 -End
            'Added By Ashutosh Patil as on 06-Jun-2007
            'YREN-3490
            If Request.Form("Yes") = "Yes" Then
                Call SaveBenificiaryDetails()
            End If

            If Session("PhonySSNo") = "Not_Phony_SSNo" Then
                Session("PhonySSNo") = Nothing
                Call SetControlFocus(Me.txtSSNNo)
            End If
            'SP 2014.02.24 BT-2316\YRS 5.0-2262 -Commented validation -Start
            'AA:20.02.2014 Added to save and close the pop-up after validating message
            'If Request.Form("OK") = "OK" Then
            '    If ViewState("SpousalWavierMessage") = True Then
            '        ViewState("SpousalWavierMessage") = Nothing
            '        Dim msg As String
            '        msg = msg + "<Script Language='JavaScript'>"
            '        msg = msg + "window.opener.document.forms(0).submit();"
            '        msg = msg + "self.close();"
            '        msg = msg + "</Script>"
            '        Response.Write(msg)
            '    End If
            'End If
            'SP 2014.02.24 BT-2316\YRS 5.0-2262 -Commented validation -End
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_string_Message As String
        Dim l_bln_BenExists As Boolean
        Dim l_str_SSNoDetails As String
        Dim l_dataset_RelationShips As DataSet
        Dim strWSMessage As String
        Dim stTelephoneError As String 'PPP | 2015.10.12 | YRS-AT-2588
        'Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Declaring variable
        Dim strBeneficiaryStatus As String
        Dim strPersID As String
        Dim strBeneficiaryID As String
        Dim bIsValidPhonySSN As Boolean
        'End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Declaring variable

        Try
            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
            If strWSMessage <> "NoPending" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Invalid Beneficiary Operation", "openDialog('" + strWSMessage + "','Bene');", True)
                'MessageBox.Show(170, 300, 500, 180, PlaceHolder1, "Beneficiary", "Add,Edit or Delete operation can not be performed due to Following reasons<br/>" + strMessage, MessageBoxButtons.OK, True)
                Exit Sub
            End If
            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application

            'Commented By Ashutosh Patil as on 06-Jun-2007
            'YREN-3490
            'A common function SaveBenificiaryDetails() will be called whenever Save Button is clicked
            'This will Save the details without if no PhonySSNo is found.
            'Dim msg As String

            'Dim Beneficiaries As New DataSet
            'Dim drRows() As DataRow

            'Dim drUpdated As DataRow

            'Vipul 03Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager

            'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)

            'Added By Ashutosh Patil as on 06-Feb-07
            'YREN - 3049
            'If txtBirthDate.Text <> "" Then
            '    If txtBirthDate.Text > Today.Date() Then
            '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Birth date entered.", MessageBoxButtons.Stop)
            '        Exit Sub
            '    End If
            'End If

            'Added By Ashutosh Patil as on 06-Jun-2007
            'Validations for Phony SSNo
            'YREN - 3490

            'Added By Ashutosh Patil as on 06-Jun-2007
            'Validations for Phony SSNo
            'YREN - 3490

            'Added By Ashutosh Patil as on 21-Jun-2007
            'YREN - 3490
            'Check 1 - Participant can not add benificiary to self.
            'Check 2 - If Benificiary already exists then also same Benificiary can not be added again.

            'Ashutosh Patil as on 12-Jul-2007
            'YREN-3490
            'Start:Anudeep:07.02.2013 : Changes made to show active relationships and already selected inactive relationships
            l_dataset_RelationShips = Session("dataset_RelationShips")
            If Not l_dataset_RelationShips Is Nothing Then
                If l_dataset_RelationShips.Tables(0).Select("Active = True AND CodeValue = '" + cmbRelation.SelectedValue + "'").Length = 0 Then
                    MessageBox.Show(PlaceHolder1, "YMCA", "The relationship selected earlier may have become inactive / invalid, so please re-select a related relationship", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If
            'End:Anudeep:07.02.2013 : Changes made to show active relationships and already selected inactive relationships

            'Anudeep:06.11.2013:BT:1455:YRS 5.0-1733:Added below code to check the beneficiary contingency level
            If cmbGroup.SelectedValue = "CONT" And cmbLevel.SelectedValue = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please select the beneficiary contingency level.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Added below code to validate benefeciary contigent level if user selected other than level1 then a message will be displayed
            If cmbGroup.SelectedValue = "CONT" And cmbLevel.SelectedValue <> "LVL1" Then
                MessageBox.Show(PlaceHolder1, "YMCA", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_BENEFIT_LVL2_LVL3_NOT_SUPPORTED, MessageBoxButtons.Stop)
                Exit Sub
            End If

            'SP 2014.11.27 BT-2310\YRS 5.0-2255 -Start
            Dim strMessage As String = IsRepresentativeFieldsEmpty()
            If Not (String.IsNullOrEmpty(strMessage)) Then
                MessageBox.Show(PlaceHolder1, "YMCA", strMessage, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'SP 2014.11.27 BT-2310\YRS 5.0-2255 -End

            'START: SB | 2017.12.14 | YRS-AT-3756 | Added code below to validate new added beneficiary that need to be replacement for the deceased spouse beneficiary should not have relation as spouse. If relationship is selected as spouse display error message and exit
            If HelperFunctions.isNonEmpty(Me.DeceasedBeneficiary) And (ddlDeceasedBeneficiary.Visible = True) Then
                If ((Trim(ddlDeceasedBeneficiary.SelectedItem.Text) <> "None") And (cmbRelation.SelectedValue = "SP")) Then
                    MessageBox.Show(PlaceHolder1, "YMCA", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PART_MAINT_INVAILD_RELATIONSHIP_CODE_BENEF).DisplayText, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If
            'END: SB | 2017.12.14 | YRS-AT-3756 | Added code below to validate new added beneficiary that need to be replacement for the deceased spouse beneficiary should not have relation as spouse. If relationship is selected as spouse display error message and exit

            If Me.txtSSNNo.Text.ToString().Trim() <> "" Then

                If Not Session("Person_Info") Is Nothing Then
                    l_str_SSNoDetails = Session("Person_Info")
                Else
                    l_str_SSNoDetails = String.Empty
                End If

                'Ashutosh Patil Self Adding as a Benificiary check and addition of existing Benificiary check as on 21-Jun-2007
                'Start Ashutosh Patil
                If Me.txtSSNNo.Text.Trim().Equals(Right(l_str_SSNoDetails, 9)) Then
                    'START: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    ''START : SB | 05/08/2018 | YRS-AT-3353 | corrected typo for words "can not" to one word "cannot" and word "Benificiary" to "Beneficiary"
                    ''MessageBox.Show(PlaceHolder1, "YMCA", "Participant can not be added as Benificiary to itself.", MessageBoxButtons.Stop)
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary to itself.", MessageBoxButtons.Stop)
                    ''END : SB | 05/08/2018 | YRS-AT-3353 | corrected typo for words "can not" to one word "cannot" and word "Benificiary" to "Beneficiary"
                    MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary of himself/herself.", MessageBoxButtons.Stop)
                    'END: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    Exit Sub
                End If
                'End Ashutosh Patil

                'YRPS-4704 - Start
                If Not Session("MaritalStatus") Is Nothing Then
                    If cmbRelation.SelectedValue = "SP" Then
                        'If Session("MaritalStatus") <> "M" Then
                        '26-May-2012 YRS 5.0-1576: update marital status if spouse beneficiary entered
                        Session("MaritalStatus") = "M"
                        'MessageBox.Show(PlaceHolder1, "YMCA", "Cannot select relationship as Spouse if participant is Single. Please change participant marital status to Married or select different relationship.", MessageBoxButtons.Stop)
                        'Exit Sub
                        'End If
                    End If
                End If
                'YRPS-4704 - End
                'Start - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Checking valid phony SSN and displaying message
                If Not Session("Flag") Is Nothing Then
                    If Session("Flag") = "AddBeneficiaries" Then
                        strBeneficiaryStatus = "INSERT"
                    ElseIf Session("Flag") = "EditBeneficiaries" Then
                        strBeneficiaryStatus = "UPDATE"
                    End If
                End If
                If Not Request.QueryString("UniqueID") Is Nothing Then
                    strBeneficiaryID = Request.QueryString("UniqueID")
                End If
                bIsValidPhonySSN = Validation.IsValidPhonySSN(txtSSNNo.Text.Trim, strBeneficiaryStatus, strBeneficiaryID, "DEATH")
                If Not bIsValidPhonySSN Then
                    'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Phony SSN already exists", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA", "Placeholder SSN already exists", MessageBoxButtons.Stop)
                    'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                    Exit Sub
                End If
                'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Checking valid phony SSN and displaying message
                'Start:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'If Session("Flag") = "AddBeneficiaries" Then
                '    l_bln_BenExists = IsBenificaryExist(Me.txtSSNNo.Text.Trim())
                '    If l_bln_BenExists Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary is already in use for Participant " & l_str_SSNoDetails & "", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'Else
                'Dim l_str_SSNo As String
                'If Not Session("TaxID") Is Nothing Then
                '    l_str_SSNo = Session("TaxID")
                'If l_str_SSNo.ToString().Trim() <> Me.txtSSNNo.Text.ToString().Trim() Then
                'YREN-3490 Benificiary Check
                'Start Ashutosh Patil as on 10-Jul-2007
                If (Me.txtSSNNo.Text.Trim() <> Phony_SSN_SYSTEM_GENERATED) Then ' Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added validation to check for existing SSN 
                    l_bln_BenExists = IsBeneficiaryExist(Me.txtSSNNo.Text.ToString().Trim())
                    If l_bln_BenExists Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary " & Me.txtSSNNo.Text.ToString().Trim() & " is already in use for the Participant.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'End:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'End Ashutosh Patil as on 10-Jul-2007
                'End If
                'End If
                'End If              
                If (Me.txtSSNNo.Text.Trim() <> Phony_SSN_SYSTEM_GENERATED) Then ' Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added validation to allow system genrated phony SSN 
                    l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.txtSSNNo.Text.Trim())

                    If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                        Session("PhonySSNo") = "Not_Phony_SSNo"
                        If Me.InvalidSSNErrMsgShown <> True Then 'Start - Manthan Rajguru | 2016.07.29 | YRS-AT-2560 | Validating poperty value to allow message to be shown
                            MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                        Else
                            Me.InvalidSSNErrMsgShown = False 'End - Manthan Rajguru | 2016.07.29 | YRS-AT-2560 | setting property value to false
                        End If
                        Exit Sub
                    ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                        'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                        'MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                        MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Placeholder SSNo ?", MessageBoxButtons.YesNo)
                        'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                        Exit Sub
                    ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                        'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                        'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                        Throw New Exception("No Key defined for Placeholder SSNo in AtsMetaConfiguration.")
                        'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                    End If
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                End If
            ElseIf Not (cmbRelation.SelectedValue = "TR" Or cmbRelation.SelectedValue = "IN" Or cmbRelation.SelectedValue = "ES") Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter beneficiary SSN/Tax No.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            Call SaveBenificiaryDetails()

            'Ashutosh Patil as on 12-Jul-2007
            'YREN-3490 
            'Condition if SSNo is not empty then only validate the data


            'Commented By Ashutosh Patil as on 06-Jun-2007
            'YREN-3490
            'Beneficiaries = CType(Session("BeneficiariesActive"), DataSet)
            ''Vipul 03Feb06 Cache-Session


            'If Session("Flag") = "AddBeneficiaries" Then

            '    drUpdated = Beneficiaries.Tables(0).NewRow
            '    drUpdated("PersId") = Session("PersId")
            '    drUpdated("Name") = txtFirstName.Text
            '    drUpdated("Name2") = txtLastName.Text
            '    drUpdated("TaxID") = txtSSNNo.Text
            '    drUpdated("Rel") = cmbRelation.SelectedValue
            '    drUpdated("Birthdate") = txtBirthDate.Text
            '    drUpdated("Groups") = cmbGroup.SelectedValue
            '    drUpdated("Lvl") = cmbLevel.SelectedValue
            '    drUpdated("Pct") = txtPercentage.Text
            '    Beneficiaries.Tables(0).Rows.Add(drUpdated)

            '    Session("BeneficiariesActive") = Beneficiaries
            '    'Vipul 03Feb06 Cache-Session
            '    'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)
            '    'Session("BeneficiariesActive") = Beneficiaries
            '    'Vipul 03Feb06 Cache-Session
            'ElseIf Session("Flag") = "EditBeneficiaries" Then

            '    If Not Request.QueryString("UniqueID") Is Nothing Then

            '        'Vipul 03Feb06 Cache-Session
            '        'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
            '        Beneficiaries = CType(Session("BeneficiariesActive"), DataSet)
            '        'Vipul 03Feb06 Cache-Session

            '        If Not IsNothing(Beneficiaries) Then
            '            drRows = Beneficiaries.Tables(0).Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
            '            drUpdated = drRows(0)
            '            drUpdated("Name") = txtFirstName.Text
            '            drUpdated("Name2") = txtLastName.Text
            '            drUpdated("TaxID") = txtSSNNo.Text
            '            drUpdated("Rel") = cmbRelation.SelectedValue
            '            drUpdated("Birthdate") = txtBirthDate.Text
            '            drUpdated("Groups") = cmbGroup.SelectedValue
            '            drUpdated("Lvl") = cmbLevel.SelectedValue
            '            drUpdated("Pct") = txtPercentage.Text

            '            Session("EditBeneficiaries") = True
            '            Session("BeneficiariesActive") = Beneficiaries
            '            'Vipul 03Feb06 Cache-Session
            '            'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)
            '            'Session("BeneficiariesActive") = Beneficiaries
            '            'Vipul 03Feb06 Cache-Session
            '        End If
            '    End If

            '    If Not Request.QueryString("Index") Is Nothing Then
            '        'Vipul 03Feb06 Cache-Session
            '        'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
            '        Beneficiaries = CType(Session("BeneficiariesActive"), DataSet)
            '        'Vipul 03Feb06 Cache-Session

            '        If Not IsNothing(Beneficiaries) Then
            '            drUpdated = Beneficiaries.Tables(0).Rows(Request.QueryString("Index"))
            '            drUpdated("Name") = txtFirstName.Text
            '            drUpdated("Name2") = txtLastName.Text
            '            drUpdated("TaxID") = txtSSNNo.Text
            '            drUpdated("Rel") = cmbRelation.SelectedValue
            '            drUpdated("Birthdate") = txtBirthDate.Text
            '            drUpdated("Groups") = cmbGroup.SelectedValue
            '            drUpdated("Lvl") = cmbLevel.SelectedValue
            '            drUpdated("Pct") = txtPercentage.Text

            '            Session("EditBeneficiaries") = True
            '            Session("BeneficiariesActive") = Beneficiaries
            '            'Vipul 03Feb06 Cache-Session
            '            'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)
            '            'Session("BeneficiariesActive") = Beneficiaries
            '            'Vipul 03Feb06 Cache-Session
            '        End If
            '    End If

            'End If


            'msg = msg + "<Script Language='JavaScript'>"

            'msg = msg + "window.opener.document.forms(0).submit();"

            'msg = msg + "self.close();"

            'msg = msg + "</Script>"
            'Response.Write(msg)
        Catch exsql As SqlException
            'Added By Ashutosh Patil as on 21-Jun-2007
            'YREN - 3490
            Dim l_String_Exception_Message As String
            If exsql.Number = 60006 And exsql.Procedure.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'l_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
                l_String_Exception_Message = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
            Else
                l_String_Exception_Message = Server.UrlEncode(exsql.Message.Trim.ToString())
            End If
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        Session("Flag") = ""
        Session("MaritalStatus") = Nothing 'YRPS-4704
        Session("Rel") = Nothing
        'AA:2014.02.13 - BT:2316:YRS 5.0-2262 - Added to clear session variable
        'Session("Pers_spousalWavier_CannotLocateSpouse") = Nothing 'SP 2014.02.24 Bt-2316\YRS 5.0-2262 - Spousal Consent date in YRS -Commented
        msg = msg + "<Script Language='JavaScript'>"
        msg = msg + "self.close();"
        msg = msg + "</Script>"

        Response.Write(msg)
    End Sub

    Private Sub cmbGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGroup.SelectedIndexChanged
        Try
            If cmbGroup.SelectedItem.Value = "PRIM" Then

                'Anudeep:06.11.2013:BT:1455:YRS 5.0-1733:Added below code to check whether already blank record exists
                If cmbLevel.Items(0).Value <> "" Then
                    cmbLevel.Items.Insert(0, "")
                End If
                cmbLevel.SelectedIndex = 0
                cmbLevel.Enabled = False
                'Name:Preeti Date:15thFeb06 IssueId: YRST-2053 Error message changes START
                RangeValidator1.ErrorMessage = "Primary memeber beneficiary must equal 100 percent."

            ElseIf cmbGroup.SelectedItem.Value = "CONT" Then
                'Anudeep:06.11.2013:BT:1455:YRS 5.0-1733:Added below code to remove blank record when group has selected to contingent
                If cmbLevel.Items(0).Value = "" Then
                    cmbLevel.Items.Remove("")
                End If
                'Name:Preeti Date:15thFeb06 IssueId: YRST-2053 Error message changes Ends
                cmbLevel.Enabled = True
                cmbLevel.SelectedIndex = 0
                RangeValidator1.ErrorMessage = "Contingent Level 1 member beneficiary must equal 100 percent."
            Else
                'Anudeep:06.11.2013:BT:1455:YRS 5.0-1733:Added below code to check whether already blank record exists
                If cmbLevel.Items(0).Value <> "" Then
                    cmbLevel.Items.Insert(0, "")
                End If
                cmbLevel.SelectedIndex = 0
                cmbLevel.Enabled = False
            End If
            RangeValidator1.Validate()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
    'Commented below code because this below activities has been moved on rbdtnlstBeneficiaryType_SelectedIndexChanged event
    'Private Sub cmbRelation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRelation.SelectedIndexChanged
    '    Try
    '        'Name:Preeti Date:8thFeb06 IssueId:2071 Reason:Non-human beneficiaries should not require DOB. START
    '        'ES(Estate)IN (Institution)TR(Trust) OTHER SHOULD ALLOW DATE
    '        If cmbRelation.SelectedValue = "TR" Or cmbRelation.SelectedValue = "IN" Or cmbRelation.SelectedValue = "ES" Then
    '            'Name:Preeti Date:8thFeb06 IssueId:2071 Reason:Non-human beneficiaries should not require DOB. END
    '            'Commented and Added By Ashutosh Patil as on 05-Feb-07
    '            'For YREN - 3049, Common function will be used for disabling the Birthdate textbox
    '            ''txtBirthDate.Text = ""
    '            ''txtBirthDate.Enabled = False
    '            ''txtBirthDate.RequiredDate = False
    '            Call DisableBirthDate()

    '            RequiredFirstNamevalidator.Visible = False
    '            RequiredFirstNamevalidator.Enabled = False
    '        Else
    '            txtBirthDate.Enabled = True
    '            txtBirthDate.RequiredDate = True

    '            RequiredFirstNamevalidator.Visible = True
    '            RequiredFirstNamevalidator.Enabled = True
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

    '    End Try
    'End Sub
    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End

    Private Sub cmbLevel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLevel.SelectedIndexChanged
        If cmbGroup.SelectedItem.Value = "PRIM" Then
            'Name:Preeti Date:15thFeb06 IssueId: YRST-2053 Error message changes START
            RangeValidator1.ErrorMessage = "Primary memeber beneficiary must equal 100 percent."
        Else
            If cmbLevel.SelectedItem.Value = "LVL1" Then
                RangeValidator1.ErrorMessage = "Contingent level 1 member beneficiary must equal 100 percent."
            ElseIf cmbLevel.SelectedItem.Value = "LVL2" Then
                RangeValidator1.ErrorMessage = "Contingent level 2 member beneficiary must equal 100 percent."
            ElseIf cmbLevel.SelectedItem.Value = "LVL3" Then
                RangeValidator1.ErrorMessage = "Contingent level 3 member beneficiary must equal 100 percent."
            End If
        End If
        RangeValidator1.Validate()
    End Sub
    Private Sub DisableBirthDate()
        'Added By Ashutosh Patil as on 05-Feb-07
        'YREN - 3049
        txtBirthDate.Text = ""
        txtBirthDate.Enabled = False
        txtBirthDate.RequiredDate = False
    End Sub
    Private Function SaveBenificiaryDetails() As String

        Page.Validate() 'NP:PS:2007.09.26 - Calling page.Validate to be able to call Page.IsValid function. Related to 2007.09.13
        If Page.IsValid = False Then Exit Function 'NP:PS:2007.09.13 - Adding code to check if the page validators have all fired

        Try
            Dim msg As String

            Dim Beneficiaries As New DataSet
            Dim drRows() As DataRow

            Dim drUpdated As DataRow
            Dim dtAddress As DataTable
            Dim strBeneTaxNumber As String = String.Empty
            'Vipul 03Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager

            'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)

            'Added By Ashutosh Patil as on 06-Feb-07
            'YREN - 3049
            If txtBirthDate.Text <> "" Then
                If txtBirthDate.Text > Today.Date() Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Birth date entered.", MessageBoxButtons.Stop)
                    Exit Function
                End If
            End If

            Beneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
            'Vipul 03Feb06 Cache-Session                        
            If Session("Flag") = "AddBeneficiaries" Then
                Dim strNewId As String = Guid.NewGuid().ToString

                drUpdated = Beneficiaries.Tables(0).NewRow
                drUpdated("PersId") = Session("PersId")
                drUpdated("Name") = txtFirstName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drUpdated("Name2") = txtLastName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drUpdated("TaxID") = txtSSNNo.Text
                drUpdated("Rel") = cmbRelation.SelectedValue
                drUpdated("Birthdate") = txtBirthDate.Text
                drUpdated("Groups") = cmbGroup.SelectedValue
                drUpdated("Lvl") = cmbLevel.SelectedValue
                drUpdated("Pct") = txtPercentage.Text
                drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue
                drUpdated("PlanType") = IIf(cmbType.SelectedValue = "SAVING", "Savings", "Retirement")  'NP:PS:2007.08.27 - Adding code to take the default PlanType based on Beneficiary Type Code.
                drUpdated("NewId") = strNewId

                '2014.11.28 BT-2310\YRS 5.0-2255 -Start
                'Add code to capture representative details
                drUpdated("RepFirstName") = txtRepFirstName.Text.Trim()
                drUpdated("RepLastName") = txtRepLastName.Text.Trim()
                drUpdated("RepSalutation") = ddlRepSalutaionCode.SelectedValue.Trim()
                drUpdated("RepTelephone") = txtRepTelephoneNo.Text.Trim()
                '2014.11.28 BT-2310\YRS 5.0-2255 -End
                'START: MMR | 2017.12.04 | YRS-AT-3756 | Added deleted beneficiary unique ID
                If ddlDeceasedBeneficiary.Visible = True Then
                    drUpdated("DeletedBeneficiaryID") = ddlDeceasedBeneficiary.SelectedValue
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | Added deleted beneficiary unique ID
                Beneficiaries.Tables(0).Rows.Add(drUpdated)

                Session("BeneficiariesActive") = Beneficiaries
                'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                If AddressWebUserControl1.Address1 <> "" And AddressWebUserControl1.City <> "" And
                            AddressWebUserControl1.DropDownListCountryValue <> "" Then

                    If Session("BeneficiaryAddress") Is Nothing Then
                        dtAddress = Address.CreateAddressDatatable()
                    Else
                        dtAddress = Session("BeneficiaryAddress")
                    End If
                    Dim drAddressRow As DataRow
                    Dim drRow As DataRow()
                    If txtSSNNo.Text.Trim() <> "" Then
                        drRow = dtAddress.Select("BenSSNo='" & txtSSNNo.Text.Trim() & "'")
                    End If

                    If Not drRow Is Nothing AndAlso drRow.Length > 0 Then
                        drAddressRow = drRow(0)
                    Else
                        drAddressRow = dtAddress.NewRow()
                    End If

                    drAddressRow("addr1") = AddressWebUserControl1.Address1.Replace(",", "").Trim()
                    drAddressRow("addr2") = AddressWebUserControl1.Address2.Replace(",", "").Trim()
                    drAddressRow("addr3") = AddressWebUserControl1.Address3.Replace(",", "").Trim()
                    drAddressRow("city") = AddressWebUserControl1.City.Replace(",", "").Trim()
                    drAddressRow("state") = AddressWebUserControl1.DropDownListStateValue.Replace(",", "").Trim()
                    drAddressRow("zipCode") = AddressWebUserControl1.ZipCode.Replace(",", "").Replace("-", "").Trim()
                    drAddressRow("country") = AddressWebUserControl1.DropDownListCountryValue.Replace(",", "").Trim()
                    drAddressRow("isActive") = True
                    drAddressRow("isPrimary") = True
                    If AddressWebUserControl1.EffectiveDate <> String.Empty Then
                        drAddressRow("effectiveDate") = AddressWebUserControl1.EffectiveDate
                    Else
                        drAddressRow("effectiveDate") = System.DateTime.Now()
                    End If
                    drAddressRow("isBadAddress") = AddressWebUserControl1.IsBadAddress
                    drAddressRow("addrCode") = "HOME"
                    drAddressRow("entityCode") = EnumEntityCode.PERSON.ToString()
                    'Anudeep:29.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                    If Not AddressWebUserControl1.Notes Is String.Empty Then
                        drAddressRow("Note") = "Beneficiary " + txtFirstName.Text.Trim + " " + txtLastName.Text.Trim + " " + AddressWebUserControl1.Notes ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                    Else
                        drAddressRow("Note") = ""
                    End If
                    drAddressRow("bitImportant") = AddressWebUserControl1.BitImp
                    drAddressRow("BenSSNo") = txtSSNNo.Text
                    drAddressRow("oldBenSSNo") = txtSSNNo.Text
                    drAddressRow("NewId") = strNewId
                    drAddressRow("PersID") = Session("PersId").ToString()
                    drAddressRow("guiEntityId") = IIf(String.IsNullOrEmpty(AddressWebUserControl1.guiEntityId), Nothing, AddressWebUserControl1.guiEntityId)
                    If drRow Is Nothing OrElse drRow.Length = 0 Then
                        dtAddress.Rows.Add(drAddressRow)
                    End If
                    Session("BeneficiaryAddress") = dtAddress
                End If
                'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                'Vipul 03Feb06 Cache-Session
                'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)
                'Session("BeneficiariesActive") = Beneficiaries
                'Vipul 03Feb06 Cache-Session
            ElseIf Session("Flag") = "EditBeneficiaries" Then

                If Not Request.QueryString("UniqueID") Is Nothing Then

                    'Vipul 03Feb06 Cache-Session
                    'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
                    Beneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
                    'Vipul 03Feb06 Cache-Session
                    Dim bFlag As Boolean = False

                    If Not IsNothing(Beneficiaries) Then

                        drRows = Beneficiaries.Tables(0).Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
                        drUpdated = drRows(0)
                        If Not IsDBNull(drUpdated("Name")) Or txtFirstName.Text <> "" Then
                            If drUpdated("Name").ToString() <> txtFirstName.Text Then
                                drUpdated("Name") = txtFirstName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("Name2")) Or txtLastName.Text <> "" Then
                            If drUpdated("Name2").ToString() <> txtLastName.Text Then
                                drUpdated("Name2") = txtLastName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                                bFlag = True
                            End If
                        End If

                        If Not IsDBNull(drUpdated("TaxID")) Or txtSSNNo.Text <> "" Then
                            strBeneTaxNumber = drUpdated("TaxID").ToString()
                            If drUpdated("TaxID").ToString() <> txtSSNNo.Text Then
                                drUpdated("TaxID") = txtSSNNo.Text
                                bFlag = True
                                ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Updating New SSN and Reason in Audit DataTable
                                If (rbdtnlstBeneficiaryType.SelectedIndex = 0) Then
                                    UpdateAduitDataTable(Convert.ToString(drUpdated("UniqueID")), txtSSNNo.Text)
                                End If
                                ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Updating New SSN and Reason in Audit DataTable
                            End If
                        End If
                        If Not IsDBNull(drUpdated("Rel")) Or cmbRelation.SelectedIndex <> 0 Then
                            If drUpdated("Rel") <> cmbRelation.SelectedValue Then
                                drUpdated("Rel") = cmbRelation.SelectedValue
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("Birthdate")) Or txtBirthDate.Text <> "" Then
                            If drUpdated("Birthdate").ToString() <> txtBirthDate.Text Then
                                drUpdated("Birthdate") = txtBirthDate.Text
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("Groups")) Or cmbGroup.SelectedIndex <> 0 Then
                            If drUpdated("Groups") <> cmbGroup.SelectedValue Then
                                drUpdated("Groups") = cmbGroup.SelectedValue
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("Lvl")) Or cmbLevel.SelectedIndex <> 0 Then
                            If drUpdated("Lvl").ToString() <> cmbLevel.SelectedValue Then
                                drUpdated("Lvl") = cmbLevel.SelectedValue
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("Pct")) Or txtPercentage.Text <> "" Then
                            If drUpdated("Pct").ToString() <> txtPercentage.Text Then
                                drUpdated("Pct") = txtPercentage.Text
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("BeneficiaryTypeCode")) Or cmbType.SelectedIndex <> 0 Then
                            If drUpdated("BeneficiaryTypeCode") <> cmbType.SelectedValue Then
                                drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("PlanType")) Or cmbType.SelectedIndex <> 0 Then
                            If drUpdated("PlanType") <> IIf(cmbType.SelectedValue = "SAVING", "Savings", "Retirement") Then
                                drUpdated("PlanType") = IIf(cmbType.SelectedValue = "SAVING", "Savings", "Retirement") 'NP:PS:2007.08.27 - Adding code to take the default PlanType based on Beneficiary Type Code.
                                bFlag = True
                            End If
                        End If

                        '2014.11.28 BT-2310\YRS 5.0-2255 -Start
                        'Add code to capture representative details
                        If Not IsDBNull(drUpdated("RepFirstName")) Or txtRepFirstName.Text.Trim() <> String.Empty Then
                            If Convert.ToString(drUpdated("RepFirstName")) <> txtRepFirstName.Text.Trim() Then
                                drUpdated("RepFirstName") = IIf(String.IsNullOrEmpty(txtRepFirstName.Text.Trim()), Nothing, txtRepFirstName.Text.Trim())
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("RepLastName")) Or txtRepLastName.Text.Trim() <> String.Empty Then
                            If Convert.ToString(drUpdated("RepLastName")) <> txtRepLastName.Text.Trim() Then
                                drUpdated("RepLastName") = IIf(String.IsNullOrEmpty(txtRepLastName.Text.Trim()), Nothing, txtRepLastName.Text.Trim())
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("RepSalutation")) Or ddlRepSalutaionCode.SelectedIndex <> 0 Then
                            If Convert.ToString(drUpdated("RepSalutation")).Trim() <> ddlRepSalutaionCode.SelectedValue.Trim() Then 'START: Manthan Rajguru | 2016.09.08 | YRS-AT-2560 | Added trim function while comaparing two values
                                drUpdated("RepSalutation") = IIf(String.IsNullOrEmpty(ddlRepSalutaionCode.SelectedValue.Trim()), Nothing, ddlRepSalutaionCode.SelectedValue.Trim())
                                bFlag = True
                            End If
                        End If
                        If Not IsDBNull(drUpdated("RepTelephone")) Or txtRepTelephoneNo.Text.Trim() <> String.Empty Then
                            If Convert.ToString(drUpdated("RepTelephone")) <> txtRepTelephoneNo.Text.Trim() Then
                                drUpdated("RepTelephone") = IIf(String.IsNullOrEmpty(txtRepTelephoneNo.Text.Trim()), Nothing, txtRepTelephoneNo.Text.Trim())
                                bFlag = True
                            End If
                        End If
                        '2014.11.28 BT-2310\YRS 5.0-2255 -End

                        'START: MMR | 2017.12.04 | YRS-AT-3756 | Updating beneficiary of deceased beneficiary
                        If Not IsDBNull(drUpdated("DeletedBeneficiaryID")) Or ddlDeceasedBeneficiary.SelectedValue <> String.Empty Then
                            If Convert.ToString(drUpdated("DeletedBeneficiaryID")) <> ddlDeceasedBeneficiary.SelectedValue Then
                                drUpdated("DeletedBeneficiaryID") = ddlDeceasedBeneficiary.SelectedValue
                                bFlag = True
                            End If
                        End If
                        'END: MMR | 2017.12.04 | YRS-AT-3756 | Updating beneficiary of deceased beneficiary

                        If bFlag = True Then
                            Session("EditBeneficiaries") = True
                            Session("BeneficiariesActive") = Beneficiaries
                        End If
                        'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        If AddressWebUserControl1.Address1 <> "" And AddressWebUserControl1.City <> "" And
                            AddressWebUserControl1.DropDownListCountryValue <> "" Then
                            Dim drAddressRow As DataRow
                            Dim drRow As DataRow()

                            If Session("BeneficiaryAddress") Is Nothing Then
                                dtAddress = Address.CreateAddressDatatable()
                            Else
                                dtAddress = Session("BeneficiaryAddress")

                                If strBeneTaxNumber.Trim() <> "" Then
                                    drRow = dtAddress.Select("BenSSNo='" & strBeneTaxNumber & "'")
                                End If

                                If drRow Is Nothing OrElse drRow.Length = 0 Then
                                    drRow = dtAddress.Select("guiEntityId='" & Request.QueryString("UniqueID") & "'")
                                End If

                                If drRow Is Nothing OrElse drRow.Length = 0 Then
                                    drRow = dtAddress.Select("BeneID='" & Request.QueryString("UniqueID") & "'")
                                End If

                            End If

                            If Not drRow Is Nothing AndAlso drRow.Length > 0 Then
                                drAddressRow = drRow(0)
                            Else
                                drAddressRow = dtAddress.NewRow()
                            End If
                            drAddressRow("addr1") = AddressWebUserControl1.Address1.Replace(",", "").Trim()
                            drAddressRow("addr2") = AddressWebUserControl1.Address2.Replace(",", "").Trim()
                            drAddressRow("addr3") = AddressWebUserControl1.Address3.Replace(",", "").Trim()
                            drAddressRow("city") = AddressWebUserControl1.City.Replace(",", "").Trim()
                            drAddressRow("state") = AddressWebUserControl1.DropDownListStateValue.Replace(",", "").Trim()
                            drAddressRow("zipCode") = AddressWebUserControl1.ZipCode.Replace(",", "").Replace("-", "").Trim()
                            drAddressRow("country") = AddressWebUserControl1.DropDownListCountryValue.Replace(",", "").Trim()
                            drAddressRow("isActive") = True
                            drAddressRow("isPrimary") = True
                            If AddressWebUserControl1.EffectiveDate <> String.Empty Then
                                drAddressRow("effectiveDate") = AddressWebUserControl1.EffectiveDate
                            Else
                                drAddressRow("effectiveDate") = System.DateTime.Now()
                            End If
                            drAddressRow("isBadAddress") = AddressWebUserControl1.IsBadAddress
                            drAddressRow("addrCode") = "HOME"
                            drAddressRow("entityCode") = EnumEntityCode.PERSON.ToString()
                            'Anudeep:29.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                            If Not AddressWebUserControl1.Notes Is String.Empty Then
                                drAddressRow("Note") = "Beneficiary " + txtFirstName.Text.Trim + " " + txtLastName.Text.Trim + " " + AddressWebUserControl1.Notes ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                            ElseIf String.IsNullOrEmpty((drAddressRow("Note").ToString())) Then
                                drAddressRow("Note") = ""
                            End If
                            drAddressRow("bitImportant") = AddressWebUserControl1.BitImp
                            drAddressRow("BenSSNo") = txtSSNNo.Text
                            drAddressRow("PersID") = Session("PersId").ToString()
                            drAddressRow("BeneID") = Request.QueryString("UniqueID")
                            'drAddressRow("guiEntityId") = IIf(String.IsNullOrEmpty(AddressWebUserControl1.guiEntityId), Nothing, AddressWebUserControl1.guiEntityId)
                            If drRow Is Nothing OrElse drRow.Length = 0 Then
                                dtAddress.Rows.Add(drAddressRow)
                            End If

                            Session("BeneficiaryAddress") = dtAddress
                        End If
                        'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        'Vipul 03Feb06 Cache-Session
                        'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)
                        'Session("BeneficiariesActive") = Beneficiaries
                        'Vipul 03Feb06 Cache-Session
                    End If
                End If

                'If Not Request.QueryString("Index") Is Nothing Then
                If Not Request.QueryString("NewID") Is Nothing Then

                    'Vipul 03Feb06 Cache-Session
                    'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
                    Beneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
                    'Vipul 03Feb06 Cache-Session

                    If Not IsNothing(Beneficiaries) Then
                        Beneficiaries.Tables(0).DefaultView.RowStateFilter = DataViewRowState.CurrentRows   'NP:PS:2007.08.31 - Handling the Index properly
                        drUpdated = Beneficiaries.Tables(0).Select("NewId='" & Request.QueryString("NewID") & "'")(0)
                        drUpdated("Name") = txtFirstName.Text.Trim 'AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                        drUpdated("Name2") = txtLastName.Text.Trim 'AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                        strBeneTaxNumber = drUpdated("TaxID")
                        drUpdated("TaxID") = txtSSNNo.Text
                        drUpdated("Rel") = cmbRelation.SelectedValue
                        drUpdated("Birthdate") = txtBirthDate.Text
                        drUpdated("Groups") = cmbGroup.SelectedValue
                        drUpdated("Lvl") = cmbLevel.SelectedValue
                        drUpdated("Pct") = txtPercentage.Text
                        drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue
                        drUpdated("PlanType") = IIf(cmbType.SelectedValue = "SAVING", "Savings", "Retirement")  'NP:PS:2007.08.27 - Adding code to take the default PlanType based on Beneficiary Type Code.

                        'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        If AddressWebUserControl1.Address1 <> "" And AddressWebUserControl1.City <> "" And
                            AddressWebUserControl1.DropDownListCountryValue <> "" Then
                            Dim drAddressRow As DataRow
                            Dim drRow As DataRow()

                            If Session("BeneficiaryAddress") Is Nothing Then
                                dtAddress = Address.CreateAddressDatatable()
                            Else
                                dtAddress = Session("BeneficiaryAddress")
                                drRow = dtAddress.Select("NewId='" & Request.QueryString("NewID") & "'")
                            End If

                            If Not drRow Is Nothing AndAlso drRow.Length > 0 Then
                                drAddressRow = drRow(0)
                            Else
                                drAddressRow = dtAddress.NewRow()
                                drAddressRow("NewId") = Request.QueryString("NewID")
                            End If

                            drAddressRow("addr1") = AddressWebUserControl1.Address1.Replace(",", "").Trim()
                            drAddressRow("addr2") = AddressWebUserControl1.Address2.Replace(",", "").Trim()
                            drAddressRow("addr3") = AddressWebUserControl1.Address3.Replace(",", "").Trim()
                            drAddressRow("city") = AddressWebUserControl1.City.Replace(",", "").Trim()
                            drAddressRow("state") = AddressWebUserControl1.DropDownListStateValue.Replace(",", "").Trim()
                            drAddressRow("zipCode") = AddressWebUserControl1.ZipCode.Replace(",", "").Replace("-", "").Trim()
                            drAddressRow("country") = AddressWebUserControl1.DropDownListCountryValue.Replace(",", "").Trim()
                            drAddressRow("isActive") = True
                            drAddressRow("isPrimary") = True

                            If AddressWebUserControl1.EffectiveDate <> String.Empty Then
                                drAddressRow("effectiveDate") = AddressWebUserControl1.EffectiveDate
                            Else
                                drAddressRow("effectiveDate") = System.DateTime.Now()
                            End If

                            drAddressRow("isBadAddress") = AddressWebUserControl1.IsBadAddress
                            drAddressRow("addrCode") = "HOME"
                            drAddressRow("entityCode") = EnumEntityCode.PERSON.ToString()
                            'Anudeep:29.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                            If Not AddressWebUserControl1.Notes Is String.Empty Then
                                drAddressRow("Note") = "Beneficiary " + txtFirstName.Text.Trim + " " + txtLastName.Text.Trim + " " + AddressWebUserControl1.Notes ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                            ElseIf String.IsNullOrEmpty((drAddressRow("Note").ToString())) Then
                                drAddressRow("Note") = ""
                            End If
                            drAddressRow("bitImportant") = AddressWebUserControl1.BitImp
                            drAddressRow("BenSSNo") = txtSSNNo.Text
                            drAddressRow("PersID") = Session("PersId").ToString()
                            drAddressRow("guiEntityId") = IIf(String.IsNullOrEmpty(AddressWebUserControl1.guiEntityId), Nothing, AddressWebUserControl1.guiEntityId)

                            If drRow Is Nothing OrElse drRow.Length = 0 Then
                                dtAddress.Rows.Add(drAddressRow)
                            End If

                            Session("BeneficiaryAddress") = dtAddress
                        Else
                            Dim drAddressRow As DataRow
                            Dim drRow As DataRow()

                            If Not Session("BeneficiaryAddress") Is Nothing Then
                                dtAddress = Session("BeneficiaryAddress")
                                drRow = dtAddress.Select("NewId='" & Request.QueryString("NewID") & "'")
                            End If

                            If drRow.Length > 0 Then
                                drRow(0).Delete()
                            End If

                        End If
                        'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

                        Session("EditBeneficiaries") = True
                        Session("BeneficiariesActive") = Beneficiaries

                        'Vipul 03Feb06 Cache-Session
                        'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)
                        'Session("BeneficiariesActive") = Beneficiaries
                        'Vipul 03Feb06 Cache-Session
                    End If
                End If

            End If
            Session("Rel") = Nothing
            '24-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
            'Session("MaritalStatus") = Nothing 'YRPS-4704
            'AA:2014.02.13 - BT:2316:YRS 5.0-2262 - Added to Clear session variable
            'Session("Pers_spousalWavier_CannotLocateSpouse") = Nothing 'SP 2014.02.24 Bt-2316\YRS 5.0-2262 - Spousal Consent date in YRS -Commented

            'AA:2014.02.13 - BT:2316:YRS 5.0-2262 - Added to check whether a spouse is updating or adding with less than 100% benfit
            'SP 2014.02.24 BT-2316\YRS 5.0-2262 -Commenetd validation -Start
            'If cmbRelation.SelectedValue.Trim.ToUpper() = "SP" And txtPercentage.Text.Trim <> 100 And cmbGroup.SelectedValue = "PRIM" And Session("Pers_spousalWavier_CannotLocateSpouse") <> True Then
            '    ViewState("SpousalWavierMessage") = True
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_SPOUSAL_WAVIER_DATE_VALIDATION, MessageBoxButtons.Stop, True)
            '    Exit Function
            'Else
            'SP 2014.02.24 BT-2316\YRS 5.0-2262 -Commenetd validation -End
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
            'End If  'SP 2014.02.24 BT-2316\YRS 5.0-2262 -Commenetd validation

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub SetControlFocus(ByVal TextBoxFocus As TextBox)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
                            "var obj = document.getElementById('" & TextBoxFocus.ID & "');" & _
                            "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Function IsBeneficiaryExist(ByVal paramstrBenificiaryNo) As Boolean
        'Created By Ashutosh Patil as on 21-Jun-2007
        'YREN-3490 
        'This function will check if any Benificiary already exists for concerned Participant and 
        'if found then it will return "Benificiary_Exists"
        Dim l_BenExist_Dataset As DataSet
        Dim l_BenSSNo_DataRow As DataRow()
        Try
            IsBeneficiaryExist = False
            'NP:BT-550:2008.09.10 - Pulling information from the Session variable Beneficiaries Active instead of Beneficiaries Retired since we are working with active beneficiaries
            If Not Session("BeneficiariesActive") Is Nothing Then
                l_BenExist_Dataset = New DataSet
                l_BenExist_Dataset = DirectCast(Session("BeneficiariesActive"), DataSet)
                If Not l_BenExist_Dataset Is Nothing Then
                    If l_BenExist_Dataset.Tables(0).Rows.Count > 0 Then
                        'Start:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                        'For Each l_BenSSNo_DataRow In l_BenExist_Dataset.Tables(0).Rows
                        '    'Added by Anudeep:10.07.2013 For Validating the existing beneficary : Bt-2084:Beneficiaries add and update SSN validation change
                        '    If l_BenSSNo_DataRow.RowState <> DataRowState.Deleted AndAlso _
                        '                paramstrBenificiaryNo.Equals(l_BenSSNo_DataRow("TaxId").ToString) _
                        '                And l_BenSSNo_DataRow("BeneficiaryTypeCode") = cmbType.SelectedValue Then 'NP:PS:2007.08.27 - Adding condition to check if the row has already been deleted or not.
                        '        IsBenificaryExist = True
                        '        Exit Function
                        '    End If
                        'Next

                        l_BenSSNo_DataRow = l_BenExist_Dataset.Tables(0).Select("TaxId = '" + paramstrBenificiaryNo + "' And BeneficiaryTypeCode='" + cmbType.SelectedValue + "'")
                        If l_BenSSNo_DataRow.Length > 0 Then

                            If Session("Flag") = "AddBeneficiaries" Then
                                If l_BenSSNo_DataRow(0).RowState <> DataRowState.Deleted Then
                                    IsBeneficiaryExist = True
                                End If
                            ElseIf Session("Flag") = "EditBeneficiaries" Then
                                If l_BenSSNo_DataRow(0).RowState <> DataRowState.Deleted AndAlso _
                                Not Request.QueryString("NewID") Is Nothing AndAlso _
                                l_BenSSNo_DataRow(0)("NewId").ToString() <> Request.QueryString("NewID") Then

                                    IsBeneficiaryExist = True

                                ElseIf l_BenSSNo_DataRow(0).RowState <> DataRowState.Deleted AndAlso _
                                    Not Request.QueryString("UniqueID") Is Nothing AndAlso _
                                    l_BenSSNo_DataRow(0)("UniqueID").ToString() <> Request.QueryString("UniqueID") Then

                                    IsBeneficiaryExist = True
                                End If
                            End If

                        End If
                        'End:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Private Sub txtSSNNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSSNNo.TextChanged
        Dim l_string_Message As String
        Dim l_bln_BenExists As Boolean
        Dim l_str_SSNoDetails As String
        Dim dtAddress As DataTable
        Dim drAddress As DataRow()
        Try
            lblAddressChange.Visible = False
            If Me.txtSSNNo.Text.ToString().Trim() <> "" Then
                If Not Session("Person_Info") Is Nothing Then
                    l_str_SSNoDetails = Session("Person_Info")
                Else
                    l_str_SSNoDetails = String.Empty
                End If

                'Ashutosh Patil Self Adding as a Benificiary check and addition of existing Benificiary check as on 21-Jun-2007
                'Start Ashutosh Patil
                If Me.txtSSNNo.Text.Trim().Equals(Right(l_str_SSNoDetails, 9)) Then
                    'START: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    ''START : SB | 05/08/2018 | YRS-AT-3353 | corrected typo for words "can not" to one word "cannot" 
                    ''MessageBox.Show(PlaceHolder1, "YMCA", "Participant can not be added as Beneficiary to itself.", MessageBoxButtons.Stop)
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary to itself.", MessageBoxButtons.Stop)
                    ''END : SB | 05/08/2018 | YRS-AT-3353 | corrected typo for words "can not" to one word "cannot" 
                    MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary of himself/herself.", MessageBoxButtons.Stop)
                    'END: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    Exit Sub
                End If

                'Start:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'If Session("Flag") = "AddBeneficiaries" Then
                '    l_bln_BenExists = IsBenificaryExist(Me.txtSSNNo.Text.Trim())
                '    If l_bln_BenExists Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary is already in use for Participant " & l_str_SSNoDetails & "", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'Else
                '    Dim l_str_SSNo As String
                '    If Not Session("TaxID") Is Nothing Then
                '        l_str_SSNo = Session("TaxID")
                '        If l_str_SSNo.ToString().Trim() <> Me.txtSSNNo.Text.ToString().Trim() Then
                '            'YREN-3490 Benificiary Check
                '            'Start Ashutosh Patil as on 10-Jul-2007
                If (Me.txtSSNNo.Text.Trim() <> Phony_SSN_SYSTEM_GENERATED) Then ' Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added validation to check for existing SSN 
                    l_bln_BenExists = IsBeneficiaryExist(Me.txtSSNNo.Text.ToString().Trim())
                    If l_bln_BenExists Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary " & Me.txtSSNNo.Text.ToString().Trim() & " is already in use for the Participant.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'End:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'End Ashutosh Patil as on 10-Jul-2007
                'End If
                '        End If
                '    End If


                l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.txtSSNNo.Text.Trim())

                If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                    Session("PhonySSNo") = "Not_Phony_SSNo"
                    Me.InvalidSSNErrMsgShown = True 'Manthan Rajguru | 2016.07.29 | YRS-AT-2560 | Setting error message value in viewstate
                    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                    Exit Sub
                    'ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                    '    MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                    '    Exit Sub
                ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                    'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                    'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                    Throw New Exception("No Key defined for Placeholder SSNo in AtsMetaConfiguration.")
                    'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                End If

                dtAddress = Session("BeneficiaryAddress")
                If Session("Flag") = "AddBeneficiaries" Or Request.QueryString("NewId") <> "" Then
                    AddressWebUserControl1.LoadAddressDetail(Nothing)
                    AddressWebUserControl1.guiEntityId = ""

                End If
                If Not dtAddress Is Nothing And txtSSNNo.Text.Trim() <> "" Then

                    drAddress = dtAddress.Select("BenSSNo='" & txtSSNNo.Text.Trim() & "'")
                    If drAddress.Length > 0 AndAlso drAddress(0).RowState <> DataRowState.Deleted Then
                        AddressWebUserControl1.LoadAddressDetail(drAddress)

                        If Session("Flag") = "AddBeneficiaries" Then
                            lblAddressChange.Visible = True
                        Else
                            If Not Request.QueryString("NewID") Is Nothing Then
                                If drAddress(0)("NewId").ToString() <> Request.QueryString("NewID") Then
                                    lblAddressChange.Visible = True
                                End If
                            ElseIf Not Request.QueryString("UniqueID") Is Nothing Then
                                If drAddress(0)("BeneID").ToString() <> Request.QueryString("UniqueID") Then
                                    lblAddressChange.Visible = True
                                End If
                            End If
                        End If

                    End If
                End If

            End If
            'START : MMR | 05/10/2018 | YRS-AT-3941 | Handling exception error and redirecting it to error page
        Catch ex As SqlException
            Dim errorMessage As String
            If ex.Number = 60006 And ex.Procedure.Trim.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                errorMessage = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
            Else
                errorMessage = Server.UrlEncode(ex.Message.Trim.ToString())
            End If
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + errorMessage, False)
        Catch ex As Exception
            Server.Transfer(String.Format("{0}{1}", "ErrorPageForm.aspx?DBMessage=", Server.UrlEncode(ex.Message.Trim.ToString())), False)
        End Try
        'END : MMR | 05/10/2018 | YRS-AT-3941 | Handling exception error and redirecting it to error page
    End Sub
    'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

    'Start:Anudeep:27.01.2014 -BT:2306:YRS 5.0-2251 : Added code to get the particiapnt address and set in the beneficiary address 
    Private Sub lnkParticipantAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkParticipantAddress.Click
        Dim dr_PrimaryAddress As DataRow()
        Dim dsAddress As DataSet
        Try
            If Session("PersId") IsNot Nothing Then
                dsAddress = Address.GetAddressByEntity(Session("PersId").ToString(), EnumEntityCode.PERSON)
                If HelperFunctions.isNonEmpty(dsAddress) Then
                    dr_PrimaryAddress = dsAddress.Tables("Address").Select("isPrimary = True")
                End If
            End If
            If dr_PrimaryAddress IsNot Nothing AndAlso dr_PrimaryAddress.Length > 0 Then
                dr_PrimaryAddress(0)("UniqueId") = ""
                dr_PrimaryAddress(0)("guiEntityId") = ""
                dr_PrimaryAddress(0)("effectiveDate") = Today.ToShortDateString  'AA:26.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from participant effective date
                AddressWebUserControl1.LoadAddressDetail(dr_PrimaryAddress)
                AddressWebUserControl1.Notes = "Address has been added / updated from participant address."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End:Anudeep:27.01.2014 -BT:2306:YRS 5.0-2251 : Added code to get the particiapnt address and set in the beneficiary address 


    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
    Private Sub BindSalutationDropDown()
        Dim dsSalutationCodes As DataSet
        Try
            dsSalutationCodes = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetSalutationCodes()
            ddlRepSalutaionCode.DataSource = dsSalutationCodes
            ddlRepSalutaionCode.DataValueField = "Code"
            ddlRepSalutaionCode.DataTextField = "Description"
            ddlRepSalutaionCode.DataBind()
            ddlRepSalutaionCode.Items.Insert(0, "")

        Catch
            Throw
        End Try
    End Sub
    Private Sub EnableBirthDateControls(ByVal isEnabled As Boolean)
        Try
            If Not isEnabled Then
                txtBirthDate.RequiredDate = isEnabled
                txtBirthDate.Text = String.Empty
                txtBirthDate.Enabled = isEnabled

                RequiredFirstNamevalidator.Visible = isEnabled
                RequiredFirstNamevalidator.Enabled = isEnabled

            Else
                txtBirthDate.RequiredDate = isEnabled
                txtBirthDate.Enabled = isEnabled

                RequiredFirstNamevalidator.Visible = isEnabled
                RequiredFirstNamevalidator.Enabled = isEnabled
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub ClearRepresentativeControls()
        Try
            txtRepFirstName.Text = String.Empty
            txtRepLastName.Text = String.Empty
            txtRepTelephoneNo.Text = String.Empty
            ddlRepSalutaionCode.SelectedIndex = 0
        Catch
            Throw
        End Try
    End Sub

    Private Sub ChangeBeneficiaryNameCaption(ByVal isChanged As Boolean)
        Try
            If isChanged Then
                LabelFirstName.Text = "Entity Name 1"
                LabelLastName.Text = "Entity Name 2"
                RequiredFieldValidator2.ErrorMessage = "Please enter entity name 2."
                lblBirthDate.Text = "Established Date" 'Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Change label text if Non-Human beneficiary selected
            Else
                LabelFirstName.Text = "First Name"
                LabelLastName.Text = "Last Name"
                RequiredFieldValidator2.ErrorMessage = "Please enter last name."
                lblBirthDate.Text = "Birth Date" 'Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Change label text if Non-Human beneficiary selected
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Function IsRepresentativeFieldsEmpty() As String
        Try
            If rbdtnlstBeneficiaryType.SelectedIndex = 1 Then 'if non human beneficiary is selected
                If Not String.IsNullOrEmpty(txtRepTelephoneNo.Text.Trim) Or Not String.IsNullOrEmpty(ddlRepSalutaionCode.SelectedValue) Or Not String.IsNullOrEmpty(txtRepFirstName.Text) Or Not String.IsNullOrEmpty(txtRepLastName.Text) Then
                    If (String.IsNullOrEmpty(txtRepFirstName.Text) AndAlso Not String.IsNullOrEmpty(txtRepLastName.Text)) Then
                        Return Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_REP_FIRST_NAME
                    ElseIf (Not String.IsNullOrEmpty(txtRepFirstName.Text) AndAlso String.IsNullOrEmpty(txtRepLastName.Text)) Then
                        Return Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_REP_LAST_NAME
                    ElseIf String.IsNullOrEmpty(txtRepFirstName.Text) And String.IsNullOrEmpty(txtRepLastName.Text) Then
                        Return Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_REP_FIRST_AND_LAST_NAME
                        'START: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    ElseIf Not String.IsNullOrEmpty(txtRepTelephoneNo.Text.Trim) Then
                        If AddressWebUserControl1.DropDownListCountryValue = "" Then 'If no address is mentioned then apply US telephone rules
                            Return Validation.Telephone(txtRepTelephoneNo.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                        ElseIf AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
                            Return Validation.Telephone(txtRepTelephoneNo.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                        Else
                            Return String.Empty
                        End If
                        'END: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    Else
                        Return String.Empty
                    End If
                Else
                    Return String.Empty
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function BindRelationShipsByFilter(ByVal dsRelationship As DataSet, ByVal beneficiaryType As EnumBeneficiaryTypes) As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsRelationship) Then
                cmbRelation.DataSource = dsRelationship.Tables(0).Select("Active = True And Category ='" + beneficiaryType.ToString + "'").CopyToDataTable()
                cmbRelation.DataTextField = "Description"
                cmbRelation.DataValueField = "CodeValue"
                cmbRelation.DataBind()
                cmbRelation.Items.Insert(0, "")
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function IsRelationshipTypeIsHumanBeneficiary(ByVal dsRelationShips As DataSet, ByVal drRelationship As DataRow) As Boolean

        Dim drFoundRows As DataRow()
        Dim isHumanBeneficiary As Boolean
        Try
            If (drRelationship IsNot Nothing AndAlso HelperFunctions.isNonEmpty(dsRelationShips)) Then

                drFoundRows = dsRelationShips.Tables(0).Select("CodeValue='" + drRelationship("Rel").ToString.Trim() + "'")
                If drFoundRows.Length > 0 Then
                    If Convert.ToString(drFoundRows(0)("Category")).Trim = EnumBeneficiaryTypes.HBENE.ToString() Then
                        isHumanBeneficiary = True
                    ElseIf Convert.ToString(drFoundRows(0)("Category")).Trim = EnumBeneficiaryTypes.NHBENE.ToString() Then
                        isHumanBeneficiary = False
                    Else
                        isHumanBeneficiary = True
                    End If
                End If

            End If
            Return isHumanBeneficiary
        Catch
            Throw
        End Try
    End Function

    Private Sub EnableDisableRepresentativeDetails(ByVal isEnable As Boolean)
        pnlRepresenattiveDetails.Visible = isEnable
        lblRepresentative.Visible = isEnable
    End Sub
    Private Sub EnableFirstName(ByVal isEnable As Boolean)
        RequiredFirstNamevalidator.Visible = isEnable
        RequiredFirstNamevalidator.Enabled = isEnable
    End Sub

    Private Sub rbdtnlstBeneficiaryType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbdtnlstBeneficiaryType.SelectedIndexChanged
        Try
            'check if beneficiary type is selected as human Or nonhuman
            If (rbdtnlstBeneficiaryType.SelectedIndex = 1) Then
                Me.IsDeceasedBeneficiaryExists = False 'MMR | 2017.12.04 | YRS-AT-3756 | setting property value for showing or hiding dropdown control and caption text for Non - human beneficiary
                EnableDisableRepresentativeDetails(True)
                BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.NHBENE)
                'Start - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                'EnableBirthDateControls(False)
                Me.txtBirthDate.RequiredDate = False
                ChangeBeneficiaryNameCaption(True)
                'End - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                EnableFirstName(False)
                ShowOrHideForHumans(True)   ' // SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility for Human and Non Humans               
                'Start - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Hiding checkbox for phony SSN generated through system for human beneficairy
                If Session("Flag") = "AddBeneficiaries" Then
                    chkPhonySSN.Visible = False
                    If chkPhonySSN.Checked = True Then
                        txtSSNNo.Text = ""
                    End If
                End If
                'End - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Hiding checkbox for phony SSN generated through system for human beneficairy
            ElseIf rbdtnlstBeneficiaryType.SelectedIndex = 0 Then
                EnableDisableRepresentativeDetails(False)
                EnableBirthDateControls(True)
                BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.HBENE)
                ClearRepresentativeControls()
                ChangeBeneficiaryNameCaption(False)
                EnableFirstName(True)
                ShowOrHideForHumans(False)   ' // SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility for Human and Non Humans
                'Start - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Showing checkbox for phony SSN generated through system for human beneficairy
                If Session("Flag") = "AddBeneficiaries" Then
                    chkPhonySSN.Visible = True
                    If chkPhonySSN.Checked = True Then
                        txtSSNNo.Text = Phony_SSN_SYSTEM_GENERATED
                        txtSSNNo.Enabled = False
                    End If
                End If
                'End - - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Showing checkbox for phony SSN generated through system for human beneficairy
                'START: MMR | 2017.12.04 | YRS-AT-3756 | setting property value for showing or hiding dropdown control and caption text for human beneficiary
                If HelperFunctions.isNonEmpty(Me.DeceasedBeneficiary) Then
                    Me.IsDeceasedBeneficiaryExists = True
                    BindDeceasedBeneficiaryDropDown(Me.DeceasedBeneficiary, Me.DropdownSelectedValue)
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | setting property value for showing or hiding dropdown control and caption text for human beneficiary
            End If
            ShowHideDeceasedBeneficiaryDropDown(Me.IsDeceasedBeneficiaryExists) 'MMR | 2017.12.04 | YRS-AT-3756 | Hiding deceased beneficiary dropdown control and caption text

        Catch
            Throw
        End Try
    End Sub
    Private Sub DisabledAllControl()
        Try
            rbdtnlstBeneficiaryType.Enabled = False
            txtFirstName.Enabled = False
            txtLastName.Enabled = False
            txtSSNNo.Enabled = False
            txtBirthDate.Enabled = False
            cmbRelation.Enabled = False
            txtPercentage.Enabled = False
            cmbGroup.Enabled = False
            cmbLevel.Enabled = False
            cmbType.Enabled = False
            lnkParticipantAddress.Enabled = False
            ddlRepSalutaionCode.Enabled = False
            txtRepFirstName.Enabled = False
            txtRepLastName.Enabled = False
            txtRepTelephoneNo.Enabled = False
            ButtonSave.Enabled = False
            AddressWebUserControl1.EnableControls = False
            txtBirthDate.Enabled = False
        Catch
            Throw
        End Try
    End Sub
    Private Sub DisableControlForSettledBeneficiaryWithMessage(ByVal drBeneficiary As DataRow)
        Try
            If (drBeneficiary IsNot Nothing) Then
                If Not IsDBNull(drBeneficiary("BeneficiaryStatusCode")) Then
                    If drBeneficiary("BeneficiaryStatusCode").ToString().Equals("SETTLD", StringComparison.CurrentCultureIgnoreCase) Then
                        DivMainMessage.Visible = True
                        DivMainMessage.InnerHtml = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_EDIT_SETTLED_BENEFICIARY
                        DisabledAllControl()
                    End If
                End If

            End If
        Catch
            Throw
        End Try
    End Sub
    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Handling Edit Button click 
    Protected Sub ButtonActiveBeneficiariesSSNEdit_Click(sender As Object, e As EventArgs) Handles ButtonActiveBeneficiariesSSNEdit.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonActiveBeneficiariesSSNEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            If ButtonActiveBeneficiariesSSNEdit.Text = "Edit" Then
                ShowOrHideEditSSN(True)
                ButtonActiveBeneficiariesSSNEdit.Text = "Cancel"
            ElseIf ButtonActiveBeneficiariesSSNEdit.Text = "Cancel" Then
                ShowOrHideEditSSN(False)
                ButtonActiveBeneficiariesSSNEdit.Text = "Edit"
                txtSSNNo.Text = Session("TaxID").ToString
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Handling Edit Button click

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility of TableRow
    Private Sub ShowOrHideEditSSN(bFlag As Boolean)
        txtSSNNo.Enabled = bFlag
        trSSNChangeReason.Visible = bFlag
        rqvBenefSSNoChangeReason.Enabled = bFlag
        rbdtnlstBeneficiaryType.Enabled = Not bFlag
        CompareOldSSNvalidator.Enabled = bFlag
        chkPhonySSN.Enabled = bFlag 'Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Setting enabled property of control
    End Sub

    Private Sub ShowOrHideForHumans(bFlag As Boolean)
        Dim bIsNotRequiredToMaintainAuditLog As Boolean = False
        Dim strPageName As String = "" 'Manthan Rajguru | 2016.07.29 |YRS-AT-2560 | Setting variable to empty
        Dim strUniqueID As String, strNewID As String

        If (Request.QueryString.AllKeys.Contains("Page")) Then
            strPageName = Request.QueryString("Page").ToLower
        End If

        'Start - Manthan Rajguru | 2016.07.29 |YRS-AT-2560 | setting flag to true while adding beneficairy
        If Session("Flag") = "AddBeneficiaries" Or strPageName = "retireprocess" Then
            bIsNotRequiredToMaintainAuditLog = True
        End If
        'End - Manthan Rajguru | 2016.07.29 |YRS-AT-2560 | setting flag to true while adding beneficairy

        If (Request.QueryString.AllKeys.Contains("NewID")) Then
            bIsNotRequiredToMaintainAuditLog = True
        End If

        If bIsNotRequiredToMaintainAuditLog Then
            txtSSNNo.Enabled = True
            ButtonActiveBeneficiariesSSNEdit.Visible = False
        Else
            txtSSNNo.Enabled = bFlag
            ButtonActiveBeneficiariesSSNEdit.Visible = Not bFlag
        End If
        trSSNChangeReason.Visible = False
        rqvBenefSSNoChangeReason.Enabled = False
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility of TableRow

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For binding reason in drop down from database 
    Private Sub BindReasonDropDown()
        Dim dsReasons As DataSet
        Try
            dsReasons = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.SearchLookups("ChangeSSNReason")
            ddlBeneficiariesSSNChangeReason.DataSource = dsReasons
            ddlBeneficiariesSSNChangeReason.DataTextField = "Desc"
            ddlBeneficiariesSSNChangeReason.DataValueField = "Code Value"

            ddlBeneficiariesSSNChangeReason.DataBind()
            ddlBeneficiariesSSNChangeReason.Items.Insert(0, New ListItem("-Select-", "-Select-"))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For binding reason in drop down from database 

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For storing New SSN and Reason in Audit DataTable
    Private Sub UpdateAduitDataTable(BeneficiaryUniqueId As String, NewSSN As String)
        Dim dtExistingDataTable As DataTable
        Dim dr As DataRow()
        Try
            dtExistingDataTable = CType(Session("AuditBeneficiariesTable"), DataTable)

            If HelperFunctions.isNonEmpty(dtExistingDataTable) Then
                dr = dtExistingDataTable.Select(String.Format("UniqueID='{0}'", BeneficiaryUniqueId))
                If dr.Length > 0 Then
                    dr(0)("NewSSN") = NewSSN
                    dr(0)("Reason") = ddlBeneficiariesSSNChangeReason.SelectedItem.Text
                    dr(0)("IsEdited") = "True"
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For storing New SSN and Reason in Audit DataTable
    ' Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting textbox properties and values on checkbox check changed event
    Private Sub chkPhonySSN_CheckedChanged(sender As Object, e As EventArgs) Handles chkPhonySSN.CheckedChanged
        If chkPhonySSN.Checked = True Then
            txtSSNNo.Text = Phony_SSN_SYSTEM_GENERATED
            txtSSNNo.Enabled = False
        ElseIf chkPhonySSN.Checked = False Then
            txtSSNNo.Text = ""
            txtSSNNo.Enabled = True
        End If
    End Sub
    'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Setting textbox properties and values on checkbox check changed event

    'START: MMR | 2017.12.04 | YRS-AT-3756 | Added to bind deceased beneficiary to dropdown
    Private Sub BindDeceasedBeneficiaryDropDown(ByVal deceasedBeneficiary As DataSet, ByVal deceasedBenef As String)
        If HelperFunctions.isNonEmpty(deceasedBeneficiary) Then
            ddlDeceasedBeneficiary.DataSource = deceasedBeneficiary
            ddlDeceasedBeneficiary.DataTextField = "BeneficiaryName"
            ddlDeceasedBeneficiary.DataValueField = "intUniqueID"
            ddlDeceasedBeneficiary.DataBind()
            ddlDeceasedBeneficiary.Items.Insert(0, New ListItem("None", 0))
            If Not String.IsNullOrEmpty(deceasedBenef) Then
                ddlDeceasedBeneficiary.SelectedValue = deceasedBenef
            End If
        End If
    End Sub
    'END: MMR | 2017.12.04 | YRS-AT-3756 | Added to bind deceased beneficiary to dropdown

    'START: MMR | 2017.12.04 | YRS-AT-3756 | Added to show or hide deceased beneficiary dropdown and caption text
    Private Sub ShowHideDeceasedBeneficiaryDropDown(ByVal bln As Boolean)
        ddlDeceasedBeneficiary.Visible = bln
        lblDeceasedBeneficiary.Visible = bln
    End Sub
    'END: MMR | 2017.12.04 | YRS-AT-3756 | Added to show or hide deceased beneficiary dropdown and caption text
End Class
