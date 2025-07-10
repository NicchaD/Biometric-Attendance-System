'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AddBeneficiary.aspx.vb
' Cache-Session     :   Vipul 02Feb06
'*******************************************************************************

'Modification History
'****************************************************************************************************************************************************
'Modified By        Date            Description
'****************************************************************************************************************************************************
'Ashutosh Patil     06-Jun-2007     YREN-3490
'Ashutosh Patil     21-Jun-2007     YREN-3490 Self Adding as a Benificiary check and addition of existing Benificiary check.
'Ashutosh Patil     10-Jul-2007     YREN-3490 Benificiary Check
'Ashutosh Patil     12-Jul-2007     YREN-3490 Checking if SSNo is empty or not
'Nikunj Patel       27-Aug-2007     Added condition to check if the row has already been deleted or not. Fixing issue reported in bugtracker-id-69
'                   31-Aug-2007     Bug reopened for the same issue but for a different set of conditions
'                   13-Sep-2007     Also check for Page.IsValid before saving
'                   26-Sep-2007     Caliing page.Validate before calling Page.IsValid to save. related to 2007.09.13.
'Mohammed Hafiz     27-Mar-2008     YRPS-4704 - Should not Allow the Beneficiary code of SP if person is single
'Neeraj Singh                       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar     27-Dec-2010     For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi             04 Mar. 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Priya				26-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
'Anudeep            07-Feb-2013     YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
'Anudeep            13-Jun-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            10-Jul-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            10.jul.2013     Bt-2084:Beneficiaries add and update SSN validation change
'Anudeep            15.jul.2013     BT-2084:Beneficiaries add and update SSN validation change
'Anudeep            29.jul.2013     Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
'Sanjay R.          2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep            2013.10.21      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2013.11.06      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2014.02.16      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.02.19      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.02.20      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.05.26      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.06.11      BT:2554:YRS 5.0-2375 : Notes display if you have spaces in name.
'Shashank Patel     2014.11.20      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
'Anudeep            2015.05.05      BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.13      YRS-AT-2588: implement some basic telephone number validation Rules
'Santosh Bura       2016.07.07      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856)
'Manthan Rajguru    2016.07.18      YRS-AT-2919 -  YRS Enh: Beneficiary Details - nonPerson - should allow optional Date
'Manthan Rajguru    2016.07.27      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru    2016.08.02      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru 	2017.12.04		YRS-AT-3756 -  YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'Santosh Bura       2017.12.14      YRS-AT-3756 -  YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) .
'Santosh Bura       2018.04.30      YRS-AT-3353 -  YRS bug: typos in MessageBox for AddBeneficiary.aspx.vb. 
'Manthan Rajguru    2018.05.10      YRS-AT-3353 -  YRS bug: typos in MessageBox for AddBeneficiary.aspx.vb. 
'Vinayan C          2018.06.18      YRS-AT-3996 -  YRS Bug: Change Beneficiary error message " to itself" to "of himself/herself".
'Dharmesh CB        2018.11.20      YRS-AT-4136 -  YRS enh: Person Maintenance screen: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
'****************************************************************************************************************************************************
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports System.Data.SqlClient
Public Class AddBeneficiary
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AddBeneficiary.aspx")
    'End issue id YRS 5.0-940



#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DropDownListAccountType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TextBoxContributionAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffectiveDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTermDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSSN As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBirthDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBirth As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRelationship As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBenefitPercentage As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBenefitGroup As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBenefitLevel As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBenefitType As System.Web.UI.WebControls.Label
    Protected WithEvents cmbType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSSNNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBirthDate As YMCAUI.DateUserControl
    Protected WithEvents cmbRelation As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtPercentage As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbLevel As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator8 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents RFVBirthDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFirstNamevalidator As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Protected WithEvents AddressWebUserControl1 As AddressUserControlNew
    Protected WithEvents lblAddressChange As System.Web.UI.WebControls.Label
    Protected WithEvents lnkParticipantAddress As System.Web.UI.WebControls.LinkButton
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'SP 2014.11.20 -BT-2310\YRS 5.0-2255-Start
    Protected WithEvents rbdtnlstBeneficiaryType As RadioButtonList
    Protected WithEvents pnlRepresenattiveDetails As Panel
    Protected WithEvents ddlRepSalutaionCode As DropDownList
    Protected WithEvents txtRepFirstName As TextBox
    Protected WithEvents txtRepLastName As TextBox
    Protected WithEvents txtRepTelephoneNo As TextBox
    Protected WithEvents lblRepresentative As Label
    Protected WithEvents vdlSummary As ValidationSummary
    Protected WithEvents DivMainMessage As HtmlGenericControl
    Protected WithEvents divErrorMsg As HtmlGenericControl 'Dharmesh : 11/20/2018 : YRS-AT-4136 : display error message for participant who enrolled on or after 2019
    'SP 2014.11.20 -BT-2310\YRS 5.0-2255-End
    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Added Edit Button,Table Row and Drop Down to find reason for SSN change
    Protected WithEvents ButtonRetireBeneficiariesSSNEdit As System.Web.UI.WebControls.Button
    Protected WithEvents trSSNChangeReason As HtmlTableRow
    Protected WithEvents ddlBeneficiariesSSNChangeReason As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rqvBenefSSNoChangeReason As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents CompareOldSSNvalidator As System.Web.UI.WebControls.CompareValidator
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Added Edit Button,Table Row and Drop Down to find reason for SSN change
    Protected WithEvents chkPhonySSN As System.Web.UI.WebControls.CheckBox 'Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Declared checkbox control
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

    'SP 2014.11.24      BT-2310\YRS 5.0-2255 -Start
    Public Property RelationShips As DataSet
        Get
            Return ViewState("Relationship")
        End Get
        Set(value As DataSet)
            ViewState("Relationship") = value
        End Set
    End Property
    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Moved to Common class Enum Files
    'Public Enum EnumBeneficiaryTypes
    '    HBENE
    '    NHBENE

    'End Enum
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  Moved to Common class Enum Files
    'SP  2014.11.24       BT-2310\YRS 5.0-2255 -End
    'Start - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Declared property to store error message value
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
    'End - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Declared property to store error message value

    'START: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to set deceased beneficiary exists or not
    Public Property IsRetiredDeceasedBeneficiaryExists() As Boolean
        Get
            If Not (ViewState("IsRetiredDeceasedBeneficiaryExists")) Is Nothing Then
                Return (CType(ViewState("IsRetiredDeceasedBeneficiaryExists"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsRetiredDeceasedBeneficiaryExists") = Value
        End Set
    End Property
    'END: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to set deceased beneficiary exists or not

    'START: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to store deceased beneficiary details
    Public Property RetiredDeceasedBeneficiary As DataSet
        Get
            If Not (Session("RetiredDeceasedBeneficiary")) Is Nothing Then
                Return (CType(Session("RetiredDeceasedBeneficiary"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("RetiredDeceasedBeneficiary") = Value
        End Set
    End Property
    'END: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to store deceased beneficiary details

    'START: MMR | 2017.12.04 | YRS-AT-3756 | Declared property to store dropdown selected value
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try

            Me.txtPercentage.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.txtPercentage.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            '-----------------------------------------------------------------
            'Shashi:04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Retiree Information - Add/Update Beneficiary"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If
            '-----------------------------------------------------------------

            If Not IsPostBack Then


                Dim l_string_PersId As String
                l_string_PersId = Session("PersId")
                Dim l_dataset_RelationShips As New DataSet
                Dim l_dataset_BenefitGroups As New DataSet
                Dim l_dataset_BeneficiaryLvl As New DataSet
                Dim l_dataset_DeathBeneficiary As New DataSet
                Dim l_datarow_relationship As DataRow()
                Dim dvLvl As DataView
                Me.txtBirthDate.RequiredDate = True
                'START | MMR | 2017.12.04 | YRS-AT-3756 | Declared Object variable                
                Dim deceasedBeneficiary As New DataSet
                Dim beneficiaryType As String
                'END | MMR | 2017.12.04 | YRS-AT-3756 | Declared Object variable                


                l_dataset_RelationShips = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getRelationShips()
                RelationShips = l_dataset_RelationShips 'SP  2014.11.24       BT-2310\YRS 5.0-2255
                l_dataset_BenefitGroups = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getBenefitGroups()
                l_dataset_BeneficiaryLvl = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getBenefitLevels()
                l_dataset_DeathBeneficiary = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getBenefitTypes()

                'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                AddressWebUserControl1.LoadAddressDetail(Nothing)


                'Start:Anudeep:07.02.2013 : Changes made to show active relationships and already selected inactive relationships
                'cmbRelation.DataSource = l_dataset_RelationShips
                'cmbRelation.DataTextField = "Description"
                'cmbRelation.DataValueField = "CodeValue"
                'cmbRelation.DataBind()


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
                    beneficiaryType = "RETIRE"
                    deceasedBeneficiary = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetDeceasedBeneficiary(Session("PersId"), beneficiaryType)
                    Me.RetiredDeceasedBeneficiary = deceasedBeneficiary
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | Getting deceased beneficiary details

                If Session("Flag") = "AddBeneficiaries" Then
                    'SP 2014.11.20 BT-2310\YRS 5.0-2255- Start
                    'Commneted the below code and moved into function based on the changes of YRS 5.0-2255
                    'If Not l_dataset_RelationShips Is Nothing Then
                    '    l_datarow_relationship = l_dataset_RelationShips.Tables(0).Select("Active = True")
                    '    If l_datarow_relationship.Length > 0 Then
                    '        cmbRelation.DataSource = l_datarow_relationship.CopyToDataTable()
                    '        cmbRelation.DataTextField = "Description"
                    '        cmbRelation.DataValueField = "CodeValue"
                    '    Else
                    '        cmbRelation.DataSource = Nothing
                    '    End If
                    '    cmbRelation.DataBind()
                    'End If
                    BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.HBENE)
                    EnableDisableRepresentativeDetails(False)
                    'SP 2014.11.20 BT-2310\YRS 5.0-2255- End

                    'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Added below code to display only level 1 option in the screen for adding a new benefeiciary
                    dvLvl = l_dataset_BeneficiaryLvl.Tables(0).DefaultView
                    dvLvl.RowFilter = "Code='LVL1'"
                    cmbLevel.DataSource = dvLvl
                    cmbLevel.DataTextField = "Description"
                    cmbLevel.DataValueField = "Code"
                    cmbLevel.DataBind()

                    ' cmbRelation.Items.Insert(0, "") SP 2014.11.20 BT-2310\YRS 5.0-2255
                    cmbType.Items.Insert(0, "")
                    cmbLevel.Items.Insert(0, "")
                    cmbGroup.Items.Insert(0, "")
                    ' // START : SB | 07/07/2016 | YRS-AT-2382 |Add beneficiary make reason and edit button  invisible
                    trSSNChangeReason.Visible = False
                    ButtonRetireBeneficiariesSSNEdit.Visible = False
                    rqvBenefSSNoChangeReason.Enabled = False
                    txtSSNNo.Enabled = True
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Add beneficiary make reason and edit button  invisible

                    'START: MMR | 2017.12.04 | YRS-AT-3756 | Dispalying dropdown based on deceased beneficairy existence and binding dropdown if beneficairy exists                    
                    If HelperFunctions.isNonEmpty(Me.RetiredDeceasedBeneficiary) Then
                        BindDeceasedBeneficiaryDropDown(Me.RetiredDeceasedBeneficiary, Me.DropdownSelectedValue)
                        Me.IsRetiredDeceasedBeneficiaryExists = True
                    End If
                    ShowHideDeceasedBeneficiaryDropDown(Me.IsRetiredDeceasedBeneficiaryExists)
                    'END: MMR | 2017.12.04 | YRS-AT-3756 | Dispalying dropdown based on deceased beneficairy existence and binding dropdown if beneficairy exists
                End If
                'start:Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
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

                    If Session("BeneficiaryAddress") Is Nothing Then
                        dtAddress = Address.CreateAddressDatatable()
                    Else
                        dtAddress = Session("BeneficiaryAddress")
                    End If
                    Dim drAddressRow As DataRow
                    Dim drRow As DataRow()

                    Beneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)

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
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    'If Not Request.QueryString("UniqueID") Is Nothing Then
                    '    txtFirstName.Text = Session("Name")
                    '    txtLastName.Text = Session("Name2")
                    '    txtSSNNo.Text = Session("TaxID")
                    '    cmbRelation.SelectedValue = Session("Rel")
                    '    txtBirthDate.Text = Session("Birthdate")
                    '    txtPercentage.Text = Session("Pct")
                    '    cmbGroup.SelectedValue = Session("Groups")
                    '    If cmbGroup.SelectedValue = "CONT" Then
                    '        cmbLevel.Enabled = True
                    '        cmbLevel.SelectedValue = Session("Lvl")
                    '    Else
                    '        cmbLevel.Items.Insert(0, "")
                    '        cmbLevel.SelectedIndex = 0
                    '        cmbLevel.Enabled = False
                    '    End If


                    '   txtPercentage.Text = Session("Pct")
                    '   cmbType.SelectedValue = Session("Type")
                    'End If
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
                        If HelperFunctions.isNonEmpty(dtAddress) And (drRow Is Nothing OrElse drRow.Length = 0) Then
                            drRow = dtAddress.Select("NewId='" & Request.QueryString("NewID") & "'")
                        End If
                    End If

                    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For hiding the reason table row at page load and binding the reasons in drop down
                    ShowOrHideEditSSN(False)
                    BindReasonDropDown()
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For hiding the reason table row at page load and binding the reasons in drop down
                    If Not drUpdated Is Nothing Then


                        'If HelperFunctions.isNonEmpty(dtAddress) And Not String.IsNullOrEmpty(drUpdated("TaxID").ToString()) Then
                        '    drRow = dtAddress.Select("BenSSNo='" & drUpdated("TaxID") & "'")
                        'End If
                        Me.AddressWebUserControl1.LoadAddressDetail(drRow)

                        'SP 2014.11.20 BT-2310\YRS 5.0-2255 -Start

                        If IsRelationshipTypeIsHumanBenaficiary(RelationShips, drUpdated) Then
                            EnableDisableRepresentativeDetails(False)
                            BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.HBENE)
                            EnableFirstName(True)
                            ChangeBeneficiaryNameCaption(False)
                            ShowOrHideForHumans(False)   ' // SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility for Human and Non Humans
                        Else
                            EnableDisableRepresentativeDetails(True)
                            BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.NHBENE)
                            rbdtnlstBeneficiaryType.SelectedIndex = 1
                            EnableFirstName(False)
                            'Start - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                            'EnableBirthDateControls(False)
                            Me.txtBirthDate.RequiredDate = False
                            'End - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                            ChangeBeneficiaryNameCaption(True)
                            ShowOrHideForHumans(True)   ' // SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility for Human and Non Humans
                        End If
                        'Below code is commented  & moved into above function "BindRelationShipsByFilter" for YRS 5.0-2255
                        'If Not l_dataset_RelationShips Is Nothing Then
                        '    If Not IsDBNull(drUpdated("Rel")) Then
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
                        'SP 2014.11.20 BT-2310\YRS 5.0-2255 -End

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
                        'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

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
                        DisableControlForSettledBeneficiaryWithMessage(drUpdated)  '2014.11.24 BT-2310\YRS 5.0-2255 - Disable if beneficiary is settled

                        'START: MMR | 2017.12.04 | Assigning value to dropdown
                        If HelperFunctions.isNonEmpty(Me.RetiredDeceasedBeneficiary) And rbdtnlstBeneficiaryType.SelectedIndex = 0 Then
                            If Not IsDBNull(drUpdated("DeletedBeneficiaryID")) Then
                                Me.DropdownSelectedValue = drUpdated("DeletedBeneficiaryID")
                                BindDeceasedBeneficiaryDropDown(Me.RetiredDeceasedBeneficiary, Me.DropdownSelectedValue)
                            Else
                                BindDeceasedBeneficiaryDropDown(Me.RetiredDeceasedBeneficiary, Me.DropdownSelectedValue)
                            End If
                            Me.IsRetiredDeceasedBeneficiaryExists = True
                        End If
                        ShowHideDeceasedBeneficiaryDropDown(Me.IsRetiredDeceasedBeneficiaryExists)
                        'END: MMR | 2017.12.04 | Assigning value to dropdown
                    End If
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
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
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
                    ' Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting Visible and Enabled property of checkbox at the time of updating beneficiary
                    chkPhonySSN.Visible = False
                    chkPhonySSN.Enabled = False
                    ' End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting Visible and Enabled property of checkbox at the time of updating beneficiary
                End If
                '**Code Added By Ashutosh on 14-June-06**
                ' DisableControls() 'SP 2014.11.20 BT-2310\YRS 5.0-2255 -Commented below code because its already handle based on beneficiary type selection
                '*******End Ashu Code****
                Session("PhonySSNo") = Nothing
            End If

            'Added By Ashutosh Patil as on 05-Feb-07
            'YREN-3049
            'SP 2014.11.20 BT-2310\YRS 5.0-2255 -Start 
            'Commented below code because its already handle based on beneficiary type selection
            'If Session("Flag") = "AddBeneficiaries" Then
            '    Call DisableControls()
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

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

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
            ElseIf cmbGroup.SelectedItem.Value = "CONT" Then
                'Anudeep:06.11.2013:BT:1455:YRS 5.0-1733:Added below code to remove blank record when group has selected to contingent
                If cmbLevel.Items(0).Value = "" Then
                    cmbLevel.Items.Remove("")
                End If
                cmbLevel.Enabled = True
                cmbLevel.SelectedIndex = 0
            Else
                'Anudeep:06.11.2013:BT:1455:YRS 5.0-1733:Added below code to check whether already blank record exists
                If cmbLevel.Items(0).Value <> "" Then
                    cmbLevel.Items.Insert(0, "")
                End If
                cmbLevel.SelectedIndex = 0
                cmbLevel.Enabled = False
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        Session("Flag") = ""
        Session("MaritalStatus") = Nothing 'YRPS-4704
        Session("Rel") = Nothing
        msg = msg + "<Script Language='JavaScript'>"
        msg = msg + "self.close();"
        msg = msg + "</Script>"

        Response.Write(msg)
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim l_string_Message As String
        Dim l_bln_BenExists As Boolean
        Dim l_str_SSNoDetails As String
        Dim l_dataset_RelationShips As DataSet
        Dim strWSMessage As String
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


            ''Added By Ashutosh Patil as on 06-Feb-07
            ''YREN - 3049
            'If txtBirthDate.Text <> "" Then
            '    If txtBirthDate.Text > Today.Date() Then
            '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Birth date entered.", MessageBoxButtons.Stop)
            '        Exit Sub
            '    End If
            'End If

            'Added By Ashutosh Patil as on 06-Jun-2007
            'Validations for Phony SSNo
            'YREN - 3490
            'START : Dharmesh : 11/20/2018 : YRS-AT-4136 : Check participant is enrolled on or after 1/1/2019 and has c annuity and display error message "1005"
            ' display text is like "Participant was first enrolled on or after $$CutOffDate$$, so no Retired Death Benefit is available. You may only add a beneficiary for Insured Reserves."
            If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) _
                AndAlso (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(CType(Session("AnnuityDetails"), DataSet))) _
                AndAlso cmbType.SelectedValue = "RETIRE" Then
                ShowErrorMessage(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_FOR_INSURED_RESERVES, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText())
                Exit Sub
            End If
            'END : Dharmesh : 11/20/2018 : YRS-AT-4136 : Check participant is enrolled on or after 1/1/2019 
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
            If HelperFunctions.isNonEmpty(Me.RetiredDeceasedBeneficiary) And (ddlDeceasedBeneficiary.Visible = True) Then
                If ((Trim(ddlDeceasedBeneficiary.SelectedItem.Text) <> "None") And (cmbRelation.SelectedValue = "SP")) Then
                    MessageBox.Show(PlaceHolder1, "YMCA", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PART_MAINT_INVAILD_RELATIONSHIP_CODE_BENEF).DisplayText, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If
            'END: SB | 2017.12.14 | YRS-AT-3756 | Added code below to validate new added beneficiary that need to be replacement for the deceased spouse beneficiary should not have relation as spouse. If relationship is selected as spouse display error message and exit

            If Me.txtSSNNo.Text.ToString().Trim() <> "" Then


                If Not Session("Person_Info") Is Nothing Then
                    l_str_SSNoDetails = Session("Person_Info")
                    'START: VC | 2018.06.28 | YRS-AT-3996 | Added code to Remove '-' from participant SSN"
                    l_str_SSNoDetails = l_str_SSNoDetails.Replace("-", "")
                    'END: VC | 2018.06.28 | YRS-AT-3996 | Added code to Remove '-' from participant SSN"
                Else
                    l_str_SSNoDetails = String.Empty
                End If

                'Ashutosh Patil Self Adding as a Benificiary check and addition of existing Benificiary check as on 21-Jun-2007
                'Start Ashutosh Patil
                If Me.txtSSNNo.Text.Trim().Equals(Right(l_str_SSNoDetails, 9)) Then
                    'START: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    ''START: SB | 04/30/2018 | YRS-AT-3353 | Corrected typo for word "Benificiary" to "Beneficiary" along with removal of exta space between "can not"
                    ''MessageBox.Show(PlaceHolder1, "YMCA", "Participant can not be added as Benificiary to itself.", MessageBoxButtons.Stop)
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary to itself.", MessageBoxButtons.Stop)
                    ''END: SB | 04/30/2018 | YRS-AT-3353 | Corrected typo for word "Benificiary" to "Beneficiary" along with removal of exta space between "can not"
                    MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary of himself/herself.", MessageBoxButtons.Stop)
                    'END: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    Exit Sub
                End If
                'End Ashutosh Patil

                'YRPS-4704 - Start
                If Not Session("MaritalStatus") Is Nothing Then
                    If cmbRelation.SelectedValue = "SP" Then
                        '26-May-2012 YRS 5.0-1576: update marital status if spouse beneficiary entered
                        Session("MaritalStatus") = "M"
                        'If Session("MaritalStatus") <> "M" Then
                        '	MessageBox.Show(PlaceHolder1, "YMCA", "Cannot select relationship as Spouse if participant is Single. Please change participant marital status to Married or select different relationship.", MessageBoxButtons.Stop)
                        '	Exit Sub
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
                    'START: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Phony SSN already exists", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA", "Placeholder SSN already exists", MessageBoxButtons.Stop)
                    'END: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                    Exit Sub
                End If
                'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Checking valid phony SSN and displaying message
                'Start:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'If Session("Flag") = "AddBeneficiaries" Then
                '    'Added by Anudeep:10.07.2013 For Validating the existing beneficary : Bt-2084:Beneficiaries add and update SSN validation change
                '    l_bln_BenExists = IsBenificaryExist(Me.txtSSNNo.Text.Trim())
                '    If l_bln_BenExists Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Benificiary is Already in use for Participant " & l_str_SSNoDetails & "", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'Else
                '    Dim l_str_SSNo As String
                '    If Not Session("TaxID") Is Nothing Then
                '        l_str_SSNo = Session("TaxID")
                '        If l_str_SSNo.ToString().Trim() <> Me.txtSSNNo.Text.ToString().Trim() Then
                'YREN-3490 Benificiary Check
                'Start Ashutosh Patil as on 10-Jul-2007
                'Added by Anudeep:10.07.2013 For Validating the existing beneficary : Bt-2084:Beneficiaries add and update SSN validation change
                If (Me.txtSSNNo.Text.Trim() <> Phony_SSN_SYSTEM_GENERATED) Then ' Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added validation to check for existing SSN 
                    l_bln_BenExists = IsBeneficiaryExist(Me.txtSSNNo.Text.ToString().Trim())
                    If l_bln_BenExists Then
                        'START: SB | 04/30/2018 | YRS-AT-3353 | Corrected typo for word "Benificiary" to "Beneficiary"
                        'MessageBox.Show(PlaceHolder1, "YMCA", "Benificiary " & Me.txtSSNNo.Text.ToString().Trim() & " is already in use for the Participant.", MessageBoxButtons.Stop)   
                        MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary " & Me.txtSSNNo.Text.ToString().Trim() & " is already in use for the Participant.", MessageBoxButtons.Stop)
                        'END: SB | 04/30/2018 | YRS-AT-3353 | Corrected typo for word "Benificiary" to "Beneficiary"
                        Exit Sub
                    End If
                End If
                'End:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'End Ashutosh Patil as on 10-Jul-2007
                'End If
                '        End If
                '    End If

                If (Me.txtSSNNo.Text.Trim() <> Phony_SSN_SYSTEM_GENERATED) Then ' Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added validation to allow system genrated phony SSN 
                    l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.txtSSNNo.Text.Trim())

                    If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                        Session("PhonySSNo") = "Not_Phony_SSNo"
                        If Me.InvalidSSNErrMsgShown <> True Then 'Start - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Validating poperty value to allow message to be shown
                            MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                        Else
                            Me.InvalidSSNErrMsgShown = False 'End - Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | setting property value to false
                        End If
                        Exit Sub
                    ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                        'START: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                        'MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                        MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Placeholder SSNo ?", MessageBoxButtons.YesNo)
                        'END: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                        Exit Sub
                    ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                        'START: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                        'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                        Throw New Exception("No Key defined for Placeholder SSNo in AtsMetaConfiguration.")
                        'END: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                    End If
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
                End If
            ElseIf Not (cmbRelation.SelectedValue = "TR" Or cmbRelation.SelectedValue = "IN" Or cmbRelation.SelectedValue = "ES") Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter beneficiary SSN/Tax No.", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Ashutosh Patil as on 12-Jul-2007
            'YREN-3490 
            'Condition if SSNo is not empty then only validate the data

            Call SaveBenificiaryDetails()


            'Commented By Ashutosh Patil as on 06-Jun-2007
            'YREN-3490
            ''Vipul 01Feb06 Cache-Session
            ''Dim l_CacheManager As CacheManager
            ''l_CacheManager = CacheFactory.GetCacheManager

            ''Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
            'Beneficiaries = CType(Session("BeneficiariesRetired"), DataSet)
            ''Vipul 01Feb06 Cache-Session
            'Session("RP_DataChanged") = True
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
            '    drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue
            '    Beneficiaries.Tables(0).Rows.Add(drUpdated)

            '    'Vipul 01Feb06 Cache-Session
            '    'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
            '    Session("BeneficiariesRetired") = Beneficiaries
            '    'Vipul 01Feb06 Cache-Session
            'ElseIf Session("Flag") = "EditBeneficiaries" Then

            '    If Not Request.QueryString("UniqueID") Is Nothing Then

            '        'Vipul 01Feb06 Cache-Session
            '        'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
            '        Beneficiaries = CType(Session("BeneficiariesRetired"), DataSet)
            '        'Vipul 01Feb06 Cache-Session

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
            '            drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue

            '            Session("EditBeneficiaries") = True
            '            'Vipul 01Feb06 Cache-Session
            '            'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
            '            Session("BeneficiariesRetired") = Beneficiaries
            '            'Vipul 01Feb06 Cache-Session
            '        End If
            '    End If

            '    If Not Request.QueryString("Index") Is Nothing Then

            '        'Vipul 01Feb06 Cache-Session
            '        'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
            '        Beneficiaries = CType(Session("BeneficiariesRetired"), DataSet)
            '        'Vipul 01Feb06 Cache-Session

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
            '            drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue


            '            Session("EditBeneficiaries") = True
            '            'Vipul 01Feb06 Cache-Session
            '            'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
            '            Session("BeneficiariesRetired") = Beneficiaries
            '            'Vipul 01Feb06 Cache-Session
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
                'START: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                'l_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
                l_String_Exception_Message = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
                'END: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
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
    Private Sub ShowErrorMessage(stMessage As String)
        divErrorMsg.InnerHtml = IIf(String.IsNullOrEmpty(divErrorMsg.InnerHtml.Trim), "", divErrorMsg.InnerHtml + "</br>")
        divErrorMsg.InnerHtml = divErrorMsg.InnerHtml + stMessage
        divErrorMsg.Visible = True
    End Sub
    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
    'Commented below code because this below activities has been moved on rbdtnlstBeneficiaryType_SelectedIndexChanged event
    'Private Sub cmbRelation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRelation.SelectedIndexChanged
    '    'Try
    '    '    If cmbRelation.SelectedValue = "TR" Or cmbRelation.SelectedValue = "OT" Or cmbRelation.SelectedValue = "IN" Or cmbRelation.SelectedValue = "ES" Then
    '    '        txtBirthDate.RequiredDate = False
    '    '        txtBirthDate.Text = ""
    '    '        txtBirthDate.Enabled = False
    '    '    Else
    '    '        txtBirthDate.RequiredDate = True
    '    '        txtBirthDate.Enabled = True
    '    '    End If
    '    'Catch ex As Exception
    '    '    Dim l_String_Exception_Message As String
    '    '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '    '    Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

    '    'End Try
    '    Try
    '        DisableControls()
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End

#Region "Private Function"
    Private Sub DisableControls()
        'Function made by ashutosh on 14-June-06
        Try
            Dim string_Value As String
            string_Value = cmbRelation.SelectedValue.ToUpper()
            If string_Value = "ES" Or string_Value = "IN" Or string_Value = "TR" Then
                txtBirthDate.RequiredDate = False
                txtBirthDate.Text = String.Empty
                txtBirthDate.Enabled = False

                RequiredFirstNamevalidator.Visible = False
                RequiredFirstNamevalidator.Enabled = False
            Else
                txtBirthDate.RequiredDate = True
                txtBirthDate.Enabled = True

                RequiredFirstNamevalidator.Visible = True
                RequiredFirstNamevalidator.Enabled = True
            End If
        Catch ex As Exception
            Throw

        End Try


    End Sub
    Private Function SaveBenificiaryDetails() As String

        Page.Validate() 'NP:PS:2007.09.26 - Calling page.Validate to be able to call Page.IsValid function. Related to 2007.09.13
        If Page.IsValid = False Then Exit Function 'NP:PS:2007.09.13 - Adding code to check if the page validators have all fired

        Try
            Dim msg As String

            Dim Beneficiaries As New DataSet
            Dim drRows() As DataRow

            Dim drUpdated As DataRow
            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Dim dtAddress As DataTable
            Dim strBeneTaxNumber As String = String.Empty

            'Added By Ashutosh Patil as on 06-Feb-07
            'YREN - 3049
            If txtBirthDate.Text <> "" Then
                If txtBirthDate.Text > Today.Date() Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Birth date entered.", MessageBoxButtons.Stop)
                    Exit Function
                End If
            End If


            'Vipul 01Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager

            'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
            Beneficiaries = CType(Session("BeneficiariesRetired"), DataSet)
            'Vipul 01Feb06 Cache-Session          
            Session("RP_DataChanged") = True
            If Session("Flag") = "AddBeneficiaries" Then
                'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                Dim strNewId As String = Guid.NewGuid().ToString()
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
                drUpdated("NewId") = strNewId

                '2014.11.24 BT-2310\YRS 5.0-2255 -Start
                'Add code to capture representative details
                drUpdated("RepFirstName") = txtRepFirstName.Text.Trim()
                drUpdated("RepLastName") = txtRepLastName.Text.Trim()
                drUpdated("RepSalutation") = ddlRepSalutaionCode.SelectedValue.Trim()
                drUpdated("RepTelephone") = txtRepTelephoneNo.Text.Trim()
                '2014.11.24 BT-2310\YRS 5.0-2255 -End               
                'START: MMR | 2017.12.04 | YRS-AT-3756 | Added deleted beneficiary unique ID
                If ddlDeceasedBeneficiary.Visible = True Then
                    drUpdated("DeletedBeneficiaryID") = ddlDeceasedBeneficiary.SelectedValue
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | Added deleted beneficiary unique ID
                Beneficiaries.Tables(0).Rows.Add(drUpdated)

                'Vipul 01Feb06 Cache-Session
                'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                Session("BeneficiariesRetired") = Beneficiaries
                'Vipul 01Feb06 Cache-Session

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
                'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            ElseIf Session("Flag") = "EditBeneficiaries" Then

                If Not Request.QueryString("UniqueID") Is Nothing Then

                    'Vipul 01Feb06 Cache-Session
                    'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
                    Beneficiaries = CType(Session("BeneficiariesRetired"), DataSet)
                    'Vipul 01Feb06 Cache-Session

                    If Not IsNothing(Beneficiaries) Then
                        'drRows = Beneficiaries.Tables(0).Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
                        'drUpdated = drRows(0)
                        'drUpdated("Name") = txtFirstName.Text
                        'drUpdated("Name2") = txtLastName.Text
                        'drUpdated("TaxID") = txtSSNNo.Text
                        'drUpdated("Rel") = cmbRelation.SelectedValue
                        'drUpdated("Birthdate") = txtBirthDate.Text
                        'drUpdated("Groups") = cmbGroup.SelectedValue
                        'drUpdated("Lvl") = cmbLevel.SelectedValue
                        'drUpdated("Pct") = txtPercentage.Text
                        'drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue

                        'Session("EditBeneficiaries") = True
                        ''Vipul 01Feb06 Cache-Session
                        ''l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                        'Session("BeneficiariesRetired") = Beneficiaries
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

                            '2014.11.24 BT-2310\YRS 5.0-2255 -Start
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
                                If Convert.ToString(drUpdated("RepSalutation")) <> ddlRepSalutaionCode.SelectedValue.Trim() Then
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

                            '2014.11.24 BT-2310\YRS 5.0-2255 -End                           
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
                                Session("BeneficiariesRetired") = Beneficiaries
                            End If
                            'Vipul 01Feb06 Cache-Session
                        End If
                    End If
                    'Vipul 01Feb06 Cache-Session
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
                        drAddressRow("BeneID") = Request.QueryString("UniqueID")
                        drAddressRow("PersID") = Session("PersId").ToString()
                        'drAddressRow("guiEntityId") = IIf(String.IsNullOrEmpty(AddressWebUserControl1.guiEntityId), Nothing, AddressWebUserControl1.guiEntityId)
                        If drRow Is Nothing OrElse drRow.Length = 0 Then
                            dtAddress.Rows.Add(drAddressRow)
                        End If
                        Session("BeneficiaryAddress") = dtAddress
                    End If
                End If
            End If
            'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

            'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            'If Not Request.QueryString("Index") Is Nothing Then
            If Not Request.QueryString("NewID") Is Nothing Then

                'Vipul 01Feb06 Cache-Session
                'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
                Beneficiaries = CType(Session("BeneficiariesRetired"), DataSet)
                'Vipul 01Feb06 Cache-Session

                If Not IsNothing(Beneficiaries) Then

                    Beneficiaries.Tables(0).DefaultView.RowStateFilter = DataViewRowState.CurrentRows   'NP:PS:2007.08.31 - Handling index properly
                    drUpdated = Beneficiaries.Tables(0).Select("NewId='" & Request.QueryString("NewID") & "'")(0)
                    drUpdated("Name") = txtFirstName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                    drUpdated("Name2") = txtLastName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                    drUpdated("TaxID") = txtSSNNo.Text
                    drUpdated("Rel") = cmbRelation.SelectedValue
                    drUpdated("Birthdate") = txtBirthDate.Text
                    drUpdated("Groups") = cmbGroup.SelectedValue
                    drUpdated("Lvl") = cmbLevel.SelectedValue
                    drUpdated("Pct") = txtPercentage.Text
                    drUpdated("BeneficiaryTypeCode") = cmbType.SelectedValue

                    '2014.11.24 BT-2310\YRS 5.0-2255 -Start
                    drUpdated("RepFirstName") = txtRepFirstName.Text.Trim()
                    drUpdated("RepLastName") = txtRepLastName.Text.Trim()
                    drUpdated("RepSalutation") = ddlRepSalutaionCode.SelectedValue.Trim()
                    drUpdated("RepTelephone") = txtRepTelephoneNo.Text.Trim()
                    '2014.11.24 BT-2310\YRS 5.0-2255 -End


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
                        drAddressRow("NewId") = Request.QueryString("NewID")
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
                    'Vipul 01Feb06 Cache-Session
                    'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                    Session("BeneficiariesRetired") = Beneficiaries
                    'Vipul 01Feb06 Cache-Session
                End If
            End If

            Session("Rel") = Nothing
            '28-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
            'Session("MaritalStatus") = Nothing 'YRPS-4704

            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.document.forms(0).submit();"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Throw
        End Try
    End Function
    Sub Remove()
        'this  RequiredFieldValidator3 for txtSSNNo.Test RequiredFieldValidator1 for txtFirstNameon 14-June-06
    End Sub

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
        'This function will check if any Benificiary already exists and if found then it will return "Benificiary_Exists"

        Dim l_BenExist_Dataset As DataSet
        Dim l_BenSSNo_DataRow As DataRow()
        Try
            IsBeneficiaryExist = False
            If Not Session("BeneficiariesRetired") Is Nothing Then
                l_BenExist_Dataset = New DataSet
                l_BenExist_Dataset = CType(Session("BeneficiariesRetired"), DataSet)
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
#End Region

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
                If Me.txtSSNNo.Text.Trim().Equals(Right(l_str_SSNoDetails.Replace("-", ""), 9)) Then
                    'START: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    ''START: SB | 04/30/2018 | YRS-AT-3353 | Corrected typo for word "Benificiary" to "Beneficiary" ,along with extra space removed between can not 
                    ''MessageBox.Show(PlaceHolder1, "YMCA", "Participant can not be added as Benificiary to itself.", MessageBoxButtons.Stop)
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary to itself.", MessageBoxButtons.Stop)
                    ''END: SB | 04/30/2018 | YRS-AT-3353 | Corrected typo for word "Benificiary" to "Beneficiary" ,along with extra space removed between can not 
                    MessageBox.Show(PlaceHolder1, "YMCA", "Participant cannot be added as Beneficiary of himself/herself.", MessageBoxButtons.Stop)
                    'END: VC | 2018.06.18 | YRS-AT-3996 | Commented existing message and added new message with text "of himself/herself" instead of "to itself"
                    Exit Sub
                End If
                'Start:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'If Session("Flag") = "AddBeneficiaries" Then
                '    'Added by Anudeep:10.07.2013 For Validating the existing beneficary : Bt-2084:Beneficiaries add and update SSN validation change
                '    l_bln_BenExists = IsBenificaryExist(Me.txtSSNNo.Text.Trim())
                '    If l_bln_BenExists Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary is Already in use for Participant " & l_str_SSNoDetails & "", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'Else
                '    Dim l_str_SSNo As String
                '    If Not Session("TaxID") Is Nothing Then
                '        l_str_SSNo = Session("TaxID")
                '        If l_str_SSNo.ToString().Trim() <> Me.txtSSNNo.Text.ToString().Trim() Then
                'YREN-3490 Benificiary Check
                'Start Ashutosh Patil as on 10-Jul-2007
                'Added by Anudeep:10.07.2013 For Validating the existing beneficary : Bt-2084:Beneficiaries add and update SSN validation change
                l_bln_BenExists = IsBeneficiaryExist(Me.txtSSNNo.Text.ToString().Trim())
                If l_bln_BenExists Then
                    MessageBox.Show(PlaceHolder1, "YMCA", "Beneficiary " & Me.txtSSNNo.Text.ToString().Trim() & " is already in use for the Participant.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'End:Anudeep:15.07.2013 :BT-2084:Beneficiaries add and update SSN validation change
                'End Ashutosh Patil as on 10-Jul-2007
                'End If
                '        End If
                '    End If


                l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.txtSSNNo.Text.Trim())

                If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                    Session("PhonySSNo") = "Not_Phony_SSNo"
                    Me.InvalidSSNErrMsgShown = True 'Manthan Rajguru | 2016.08.02 | YRS-AT-2560 | Setting error message value in viewstate
                    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                    Exit Sub
                    'ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                    '    MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                    '    Exit Sub
                ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                    'START: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                    'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                    Throw New Exception("No Key defined for Placeholder SSNo in AtsMetaConfiguration.")
                    'END: SB | 04/30/2018 | YRS-AT-3353 | Replaced "Phony" keyword with "Placeholder"
                End If

                dtAddress = Session("BeneficiaryAddress")
                'Anudeep:12.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
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
            'START : MMR | 05/10/2018 | YRS-AT-3353 | Handling exeption error and redirecting it to error page
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
        'END : MMR | 05/10/2018 | YRS-AT-3353 | Handling exeption error and redirecting it to error page
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
                dr_PrimaryAddress(0)("effectiveDate") = Today.ToShortDateString 'AA:26.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from participant effective date
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

    Private Function IsRelationshipTypeIsHumanBenaficiary(ByVal dsRelationShips As DataSet, ByVal drRelationship As DataRow) As Boolean

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
    Private Sub rbdtnlstBeneficiaryType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbdtnlstBeneficiaryType.SelectedIndexChanged
        Try
            'check if beneficiary type is selected as human Or nonhuman
            If (rbdtnlstBeneficiaryType.SelectedIndex = 1) Then
                Me.IsRetiredDeceasedBeneficiaryExists = False 'MMR | 2017.12.04 | YRS-AT-3756 | setting property value for showing or hiding dropdown control and caption text for Non - human beneficiary
                EnableDisableRepresentativeDetails(True)
                BindRelationShipsByFilter(RelationShips, EnumBeneficiaryTypes.NHBENE)
                ChangeBeneficiaryNameCaption(True)
                'Start - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                'EnableBirthDateControls(False)
                Me.txtBirthDate.RequiredDate = False
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
                If HelperFunctions.isNonEmpty(Me.RetiredDeceasedBeneficiary) Then
                    Me.IsRetiredDeceasedBeneficiaryExists = True
                    BindDeceasedBeneficiaryDropDown(Me.RetiredDeceasedBeneficiary, Me.DropdownSelectedValue)
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | setting property value for showing or hiding dropdown control and caption text for human beneficiary
            End If
            ShowHideDeceasedBeneficiaryDropDown(IsRetiredDeceasedBeneficiaryExists) 'MMR | 2017.12.04 | YRS-AT-3756 | showing deceased beneficiary dropdown control and caption text for human beneficiary

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
    Private Sub EnableFirstName(ByVal isEnable As Boolean)
        RequiredFirstNamevalidator.Visible = isEnable
        RequiredFirstNamevalidator.Enabled = isEnable
    End Sub
    Private Sub ChangeBeneficiaryNameCaption(ByVal isChanged As Boolean)
        Try
            If isChanged Then
                LabelFirstName.Text = "Entity Name 1"
                LabelLastName.Text = "Entity Name 2"
                RequiredFieldValidator2.ErrorMessage = "Please enter entity name 2."
                LabelBirthDate.Text = "Established Date" 'Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Change label text if Non-Human beneficiary selected
            Else
                LabelFirstName.Text = "First Name"
                LabelLastName.Text = "Last Name"
                RequiredFieldValidator2.ErrorMessage = "Please enter last name."
                LabelBirthDate.Text = "Birth Date" 'Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Change label text if Non-Human beneficiary selected
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Function IsRepresentativeFieldsEmpty() As String
        Try
            If rbdtnlstBeneficiaryType.SelectedIndex = 1 Then 'if non human beneficiary is selected
                If Not String.IsNullOrEmpty(txtRepTelephoneNo.Text.Trim) Or Not String.IsNullOrEmpty(ddlRepSalutaionCode.SelectedValue) Or Not String.IsNullOrEmpty(txtRepFirstName.Text) Or Not String.IsNullOrEmpty(txtRepLastName.Text) Then
                    If (String.IsNullOrEmpty(txtRepFirstName.Text) AndAlso (Not String.IsNullOrEmpty(txtRepLastName.Text))) Then
                        Return Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_REP_FIRST_NAME
                    ElseIf (Not String.IsNullOrEmpty(txtRepFirstName.Text) AndAlso String.IsNullOrEmpty(txtRepLastName.Text)) Then
                        Return Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_REP_LAST_NAME
                    ElseIf String.IsNullOrEmpty(txtRepFirstName.Text) And String.IsNullOrEmpty(txtRepLastName.Text) Then
                        Return Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_REP_FIRST_AND_LAST_NAME
                        'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    ElseIf Not String.IsNullOrEmpty(txtRepTelephoneNo.Text.Trim) Then
                        If AddressWebUserControl1.DropDownListCountryValue = "" Then 'If no address is mentioned then apply US telephone rules
                            Return Validation.Telephone(txtRepTelephoneNo.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                        ElseIf AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
                            Return Validation.Telephone(txtRepTelephoneNo.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                        Else
                            Return String.Empty
                        End If
                        'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
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
            ButtonOK.Enabled = False
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
    Protected Sub ButtonRetireBeneficiariesSSNEdit_Click(sender As Object, e As EventArgs) Handles ButtonRetireBeneficiariesSSNEdit.Click
        Try
            Dim stCheckSecurity As String = SecurityCheck.Check_Authorization("ButtonRetireBeneficiariesSSNEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not stCheckSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", stCheckSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            If ButtonRetireBeneficiariesSSNEdit.Text = "Edit" Then
                ShowOrHideEditSSN(True)
                ButtonRetireBeneficiariesSSNEdit.Text = "Cancel"
            ElseIf ButtonRetireBeneficiariesSSNEdit.Text = "Cancel" Then
                ShowOrHideEditSSN(False)
                ButtonRetireBeneficiariesSSNEdit.Text = "Edit"
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
            ButtonRetireBeneficiariesSSNEdit.Visible = False
        Else
            txtSSNNo.Enabled = bFlag
            ButtonRetireBeneficiariesSSNEdit.Visible = Not bFlag
        End If
        trSSNChangeReason.Visible = False
        rqvBenefSSNoChangeReason.Enabled = False
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Enabling/Disabling SSN textbox and visibility of TableRow

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For binding reason in drop down from database &  For storing New SSN and Reason in Audit DataTable
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

    Private Sub UpdateAduitDataTable(strBeneficiaryUniqueID As String, strNewSSN As String)
        Dim dtExistingDataTable As DataTable
        Dim dr As DataRow()
        Try
            dtExistingDataTable = CType(Session("AuditBeneficiariesTable"), DataTable)
            If HelperFunctions.isNonEmpty(dtExistingDataTable) Then
                dr = dtExistingDataTable.Select(String.Format("UniqueID='{0}'", strBeneficiaryUniqueID))
                If dr.Length > 0 Then
                    dr(0)("NewSSN") = strNewSSN
                    dr(0)("Reason") = ddlBeneficiariesSSNChangeReason.SelectedItem.Text
                    dr(0)("IsEdited") = "True"
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For storing New SSN and Reason in Audit DataTable &  For storing New SSN and Reason in Audit DataTable
    ' Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting textbox properties and values on checkbox check changed event
    Private Sub chkPhonySSN_CheckedChanged(sender As Object, e As EventArgs) Handles chkPhonySSN.CheckedChanged
        If chkPhonySSN.Checked = True Then
            txtSSNNo.Text = Phony_SSN_SYSTEM_GENERATED
            txtSSNNo.Enabled = False
        ElseIf chkPhonySSN.Checked = False Then
            txtSSNNo.Enabled = True
            txtSSNNo.Text = ""
        End If
    End Sub
    ' End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Setting textbox properties and values on checkbox check changed event

    'START: MMR | 2017.12.04 | YRS-AT-3756 | Added to bind deceased beneficiary to dropdown
    Private Sub BindDeceasedBeneficiaryDropDown(ByVal deceasedBeneficiary As DataSet, ByVal deceasedBenef As String)
        If HelperFunctions.isNonEmpty(deceasedBeneficiary) Then
            ddlDeceasedBeneficiary.DataSource = deceasedBeneficiary
            ddlDeceasedBeneficiary.DataTextField = "BeneficiaryName"
            ddlDeceasedBeneficiary.DataValueField = "intUniqueId"
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
