'*********************************************************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	FindInfo.aspx.vb
'
'Changed Hisorty
'------------   ----------------    ---------------------------------------------------
' Date          Author              Description
'------------   ----------------    ---------------------------------------------------
' 02-Feb-2006   Vipul               Cache-Session
' 14-Sep-2006   Shubhrata           TD Loans Phase 2
' 13-Jan-2007   Shubhrata           YREN - 3014 to create new loan requests on termination in loan maintenance screen
'                                   hence now we are also fetching original loan number so that its available 
'                                   in Loanmaintenance Screen
' 24-Jan-2007   Shubhrata           YREN-3023 to fetch fundid no for loan letters
' 16-Apr-2007   Mohammed Hafiz      YREN-3257
' 16-May-2007   Shubhrata           PlanSplitChanges(To set FundStatus Session for Retirees Page as well)
' 25-Jul-2007   Asween              Initialise PersID and FundStatusID for RetEst and RetProc respectively
' 29-May-2007   Ashutosh Patil      For Loan Details
' 05-Sep-2007   Nikunj Patel        Allow hyphen's in the SSN search field
' 08-Apr-2008   Swopna              Phase IV changes(Related to additional fund event status)
' 13-Jun-2008   Nikunj Patel        Retirement Estimates issue as reported in email dt: 2008.06.13 - Adding code to clear session variables used by Estimates
' 04-Jan-2009   Ashish Srivastava   Change QDRO Request Pending message
' 12/Nov/2009   Neeraj Singh        Added form name for security issue YRS 5.0-940 
' 12/Feb/2010   Shashi Shekhar      Restrict Data Archived Participants To proceed in Find list Except Person 
' 06/jun/2010   Neeraj Singh        Enhancement for .net 4.0
' 07/jun/2010   Neeraj Singh        review changes done
' 2010.06.21    Ashish Srivastava   YRS 5.0-1115
' 16-Aug-2010   Deven               Added logic for directly showing person detail when user comes
' 22 Dec 2010:  Shashi shekhar:     YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
' 24-Feb-2011:  Shashi Shekhar:     BT-774,For wrong FundEventId application not giving any message.
' 22.07.2011 :  Bhavna              BT706 ,kill POA Session
' 15.09.2011    Sanket Vaidya       BT-798 : System should not allow disability retirement for QD and BF fundevents
' 14.10.2011    Sanjeev Gupta       YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
' 2012.02.15	Bhavna S			For BT-941,YRS 5.0-1432:New report of checks issued after date of death
' 28-May-2012	Priya		        YRS 5.0-1576: update marital status if spouse beneficiary entered
' 2012.06.21	Bhavna S		    For BT-991,YRS 5.0-1530 - Need ability to reactivate a terminated employee
' 2012-10-04    Hafiz Rehman        YRS 5.0-1541
' 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
' 2013-10-17    Anudeep             BT:2253:Javascript error occured while opening the person lookup screen
' 2013-12-17    Anudeep             BT:2311:13.3.0 Observations
' 2014-01-27    Anudeep             BT:2311:13.3.0 Observations
' 2014-02-03    Anudeep             BT:2292:YRS 5.0-2248 - YRS 5.0-2248 - YRS Pin number
' 2014-02-14    Anudeep             BT:2292:YRS 5.0-2248 - YRS 5.0-2248 - YRS Pin number
' 2014-05-14    Anudeep             BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'2014-09-24     Anudeep             BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'2015.11.03     Anudeep A           BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
'2015-04-28     Jagadeesh           BJ:2015.04.15:BT2507:YRS 5.0-2380
'2015.09.16     Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'2016.01.12     Manthan Rajguru     YRS-AT-2485 -  Add new search capability to YRS Person Maintenance-seach by last four digits of SSN
'2016.02.17     Anudeep A           YRS-AT-2640 - YRS enh: Withdrawals Phase2:Sprint2: allow AdminTool link to launch a prepopulated Yrs withdrawal page
'2016.06.28     Anudeep A           YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'2016.06.30     Sanjay GS Rawat     YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
'2017.01.10     Manthan Rajguru     YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing 
'                                   YRS-AT-3265 -  YRS enh:improve usability of QDRO split screens (TrackIT 28050)         
'2017.01.09     Manthan Rajguru     YRS-AT-3299 -  YRS enh:improve usability of QDRO split screens(Retired) (TrackIT 28050) 
'2017.09.18     Pramod P. Pokale    YRS-AT-3631 -  YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
'2017.09.21     Sanjay GS Rawat     YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity 
'2017.09.13     Santosh Bura        YRS-AT-3541 -  YRS enh: Data Corrections Tool -Admin screen function to allow modification of RDB amounts (excessive data corrections) 
'2018.09.05     Vinayan C           YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
'2018.10.03     Manthan Rajguru     YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'2020.04.17     Megha Lad           YRS-AT-4854 - COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688)
'*********************************************************************************************************************************

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Security
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports System.Linq
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCAObjects

Public Class FindInfo
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("FindInfo.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Public Enum LoadDatasetMode
        Table
        Session
    End Enum
    'START : MMR | 2016.11.23 | YRS-AT-3145 | Declared Constant variable
#Region "Constant Values"
    Const SSNo As Integer = 1
    'START: MMR | 2017.01.10 | YRS-AT-3145 & 3265 | Changed constant values
    'Const firstName As Integer = 2
    'Const lastName As Integer = 3
    Const lastName As Integer = 2
    Const firstName As Integer = 3
    'END: MMR | 2017.01.10 | YRS-AT-3145 & 3265 | Changed constant values
    Const middleName As Integer = 4
    Const fundStatus As Integer = 5
    Const persId As Integer = 6
    Const fundEventId As Integer = 7
    Const QDRORequestId As Integer = 8
    Const isArchived As Integer = 9
    Const fundIdNo As Integer = 10
    Const isAccountLocked As Integer = 11

    'START: PPP | 09/18/2017 | YRS-AT-3631 | Private fields which holds index information of DC Tools - Change Fund Status result grid columns
    Private ssnDCToolFundStatusIndex As Integer = 1
    Private lastNameDCToolFundStatusIndex As Integer = 2
    Private firstNameDCToolFundStatusIndex As Integer = 3
    Private middleNameDCToolFundStatusIndex As Integer = 4
    Private fundStatusDCToolFundStatusIndex As Integer = 5
    Private persIDDCToolFundStatusIndex As Integer = 6
    Private fundEventIDDCToolFundStatusIndex As Integer = 7
    Private fundNoDCToolFundStatusIndex As Integer = 8
    Private isArchivedDCToolFundStatusIndex As Integer = 9
    'END: PPP | 09/18/2017 | YRS-AT-3631 | Private fields which holds index information of DC Tools - Change Fund Status result grid columns
#End Region
    'END : MMR | 2016.11.23 | YRS-AT-3145 | Declared Constant variable

    Private Property SessionPageCount() As Int32
        Get
            If Not (Session("MemberInfoPageCount")) Is Nothing Then

                Return (DirectCast(Session("MemberInfoPageCount"), Int32))
            Else
                Return 0
            End If
        End Get

        Set(ByVal Value As Int32)
            Session("MemberInfoPageCount") = Value
        End Set
    End Property

    Private Property SessionDataSetg_dataset_dsMemberInfo() As DataSet
        Get
            If Not (Session("g_dataset_dsMemberInfo")) Is Nothing Then

                Return (DirectCast(Session("g_dataset_dsMemberInfo"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("g_dataset_dsMemberInfo") = Value
        End Set
    End Property

    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    'Protected WithEvents DataGridFindInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvFindInfo As System.Web.UI.WebControls.GridView
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoDataFound As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblPhone As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents btnYes As Global.System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As Global.System.Web.UI.WebControls.Button
    Protected WithEvents btnCancel As Global.System.Web.UI.WebControls.Button
    'Protected WithEvents dgPager As DataGridPager
    'Protected WithEvents YMCA_Toolbar_WebUserControl1 As YMCAUI.YMCA_Toolbar_WebUserControl
    'Protected WithEvents YMCA_Footer_WebUserControl1 As YMCAUI.YMCA_Footer_WebUserControl

    'Protected WithEvents LabelModuleName As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'START | SR | 2016.06.30 | YRS-AT-2484 | Add checkbox to get participant whose fundstatus changed during Year.
    Protected WithEvents lnkFundStatusChanged As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblSearchSSN As System.Web.UI.WebControls.Label
    'END | SR | 2016.06.30 | YRS-AT-2484 | Add checkbox to get participant whose fundstatus changed during Year.

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_dataset_dsMemberInfo As New DataSet
    Dim g_string_FormName As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim Cache As CacheManager = CacheFactory.GetCacheManager()
        Dim SetFocus As String

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        'Commented By Aparna Samala -02/01/2007 - not used anymore
        ' Session("VignettePath") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsResultsForm.aspx?fundid="
        'Commented By Aparna Samala -02/01/2007 - not used anymore
        ''Me.TextBoxCity.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.TextBoxState.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.TextBoxFirstName.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.TextboxFundNo.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.TextBoxLastName.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        ''Me.TextBoxSSNo.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")
        '''Me.TextBoxSSNo.Attributes.Add("onblur", "javascript:return _OnBlur_TextBox();")

        ''to call find click
        TextBoxSSNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        TextBoxCity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        TextBoxState.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        TextBoxFirstName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        TextboxFundNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        TextBoxLastName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtEmail.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        txtPhone.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
        Try
            'Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            'Menu1.DataBind()

            'gvFindInfo.PageSize = 15
            'dgPager.Grid = gvFindInfo
            'dgPager.PagesToDisplay = 10

            If Not IsPostBack Then
                SetFocus = "<script language='javascript'>" & _
                                            "document.getElementById('" + TextboxFundNo.ClientID + "').focus(); " & _
                                        "</script>" 'Anudeep:BT-2253:18.10.2013 Handled not to occur javacript error because of master page "document.Form1.all.TextboxFundNo.focus();" & _
                Page.RegisterStartupScript("ScriptFocus", SetFocus)

                'Session("FindInfo_Sort") = Nothing
                'dgPager.Visible = False

                'If Session("Page") = "Person" Then 'commented by hafiz on 24-Oct-2007 for showing the last search results when user is re-directed to find person screen. 
                If Not Session("Page") Is Nothing Then 'added by hafiz on 24-Oct-2007
                    Me.TextBoxSSNo.Text = Session("Txt_SSNo")
                    Me.TextBoxLastName.Text = Session("Txt_LastName")
                    Me.TextBoxFirstName.Text = Session("Txt_FirstName")
                    Me.TextBoxCity.Text = Session("Txt_City")
                    Me.TextBoxState.Text = Session("Txt_State")
                    Me.TextboxFundNo.Text = Session("Txt_FundNo")

                    'Added by Dinesh
                    Me.txtPhone.Text = Session("Txt_Phone")
                    Me.txtEmail.Text = Session("Txt_Email")
                    Dim l_button_select As ImageButton
                    PopulateData(LoadDatasetMode.Session)
                    Me.gvFindInfo.SelectedIndex = Session("Grid_Index")
                    'AA:17.12.2013 : BT:2311 Added to show seleted record
                    l_button_select = TryCast(Me.gvFindInfo.SelectedRow.Cells(0).Controls(0), ImageButton)
                    l_button_select.ImageUrl = "images\selected.gif"
                    Session("Page") = Nothing
                Else
                    Session("gvFindInfo_PageIndex") = Nothing
                    Session("FindInfo_Sort") = Nothing
                    'AA:27.01.2014 : BT:2311 Added to clear the grid selected index session
                    Session("Grid_Index") = Nothing
                End If

                'Vipul 01Feb06 Cache-To-Session
                'Cache.Remove("BankingDtls")
                'Cache.Remove("FedWithDrawals")
                'Cache.Remove("GenWithDrawals")
                'Cache.Remove("dtNotes")
                'Cache.Remove("BeneficiariesActive")
                'Cache.Remove("BeneficiariesRetired")

                Session("BankingDtls") = Nothing
                Session("FedWithDrawals") = Nothing
                Session("GenWithDrawals") = Nothing
                Session("dtNotes") = Nothing
                Session("BeneficiariesActive") = Nothing
                Session("BeneficiariesRetired") = Nothing
                'Vipul 01Feb06 Cache-To-Session

                Session("DeathNotification") = Nothing
                Session("blnAddBankingRetirees") = Nothing
                Session("blnUpdateBankingRetirees") = Nothing
                Session("blnAddFedWithHoldings") = Nothing
                Session("blnUpdateFedWithHoldings") = Nothing
                Session("DeathNotification") = Nothing
                Session("blnAddGenWithHoldings") = Nothing
                Session("blnUpdateGenWithDrawals") = Nothing
                Session("blnAddNotes") = Nothing
                Session("blnUpdateNotes") = Nothing
                Session("Flag") = Nothing
                Session("icounter") = Nothing
                Session("Date_Deceased") = Nothing
                Session("R_BankName") = Nothing
                Session("R_BankABANumber") = Nothing
                Session("R_BankAccountNumber") = Nothing
                Session("R_BankPaymentMethod") = Nothing
                Session("R_BankAccountType") = Nothing
                Session("R_BankEffectiveDate") = Nothing
                Session("PersId") = Nothing
                Session("AnnuityId") = Nothing
                Session("SelectBank_BankName") = Nothing
                Session("SelectBank_BankABANumber") = Nothing
                Session("BankAccountNumber") = Nothing
                Session("BankEffectiveDate") = Nothing
                Session("BankPaymentMethod") = Nothing
                Session("BankAccountType") = Nothing
                Session("blnCancel") = Nothing
                Session("cmbTaxEntity") = Nothing
                Session("cmbWithHolding") = Nothing
                Session("txtAddlAmount") = Nothing
                Session("cmbMaritalStatus") = Nothing
                Session("txtExemptions") = Nothing
                'Shubhrata Sep14th 2006 TD loans phase 2
                Session("PersonID") = Nothing
                Session("LoanStatus") = Nothing
                Session("LoanRequestId") = Nothing
                Session("FundID") = Nothing
                'Shubhrata Sep14th 2006 TD loans phase 2
                'Added By Dilip Patada for Retired Participent
                Session("ISRetired") = Nothing
                'Added By Bhavna For POA
                Session("POAClicked") = Nothing
                Session("POA") = Nothing
                'BS:2012.02.01:BT-941,YRS 5.0-1432:- Clear Session("FundNo") 
                Session("FundNo") = Nothing

                '28-May-2012 YRS 5.0-1576: update marital status if spouse beneficiary entered
                Session("MaritalStatus") = Nothing
                'End YRS 5.0-1576: update marital status if spouse beneficiary entered


                'BS:2012.06.15:BT:991:YRS 5.0-1530:

                'BS:2012.06.21:BT:991:YRS 5.0-1530:

                Session("ReactivationStart") = Nothing


                Session("ReactivationStart") = Nothing
                Session("IsTDContract") = Nothing
                Session("LoandetailsId") = Nothing 'AA:2015.03.11  BT-2699:YRS 5.0-2441 : Added to clear session

                'BJ:2015.04.15:BT2507:YRS 5.0-2380
                SessionManager.SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityId = Nothing
                SessionManager.SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityType = Nothing
                'START : MMR | 2016.11.23 | YRS-AT-3145 | Clearing session values
                Session("PersSSID") = Nothing
                Session("FundEventID") = Nothing
                Session("RequestID") = Nothing
                Session("IsArchived") = Nothing
                Session("IsAccountLock") = Nothing
                'END : MMR | 2016.11.23 | YRS-AT-3145 | Clearing session values
                'START: MMR | 2017.01.10 | YRS-AT-3299 | Clearing session values
                Session("LastName") = Nothing
                Session("FirstName") = Nothing
                Session("FundStatus") = Nothing
                'END: MMR | 2017.01.10 | YRS-AT-3299 | Clearing session values
                'Added By Dilip Patada 
                g_string_FormName = Convert.ToString(Request.QueryString.Get("Name"))

                'by Aparna 13/09/2007 - Show Module Name
                'Dim LabelModuleName As Label = DirectCast(Master.FindControl("LabelModuleName"), Label)
                'If Not LabelModuleName Is Nothing Then
                '    If (g_string_FormName = "Person") Then
                '        LabelModuleName.Text = "Find Information " + "--" + " Maintenance Person"
                '    ElseIf (g_string_FormName = "LoanOptions") Then
                '        LabelModuleName.Text = "Find Information " + "--" + " Loan Maintenance "
                '    ElseIf (g_string_FormName = "Estimates") Then
                '        LabelModuleName.Text = "Find Information " + "--" + " Retirement Estimates "
                '    ElseIf (g_string_FormName = "Process") Then
                '        LabelModuleName.Text = "Find Information " + "--" + " Retirement Process "
                '        'Sanjeev Gupta 14th Oct 2011 BT ID - 925 Regenerate RMD
                '    ElseIf (g_string_FormName = "RegenerateMRD") Then
                '        LabelModuleName.Text = "Find Information " + "--" + " Regenerate RMD"
                '    End If
                'End If

                If (g_string_FormName = "Person") Then
                    Me.LabelFundNo.Visible = True
                    Me.TextboxFundNo.Visible = True
                    Me.LabelCity.Visible = True
                    Me.LabelState.Visible = True
                    Me.TextBoxCity.Visible = True
                    Me.TextBoxState.Visible = True
                    ' STARTS 2013-03-19    Dinesh Kanojia    Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    Me.lblEmail.Visible = True
                    Me.lblPhone.Visible = True
                    Me.txtPhone.Visible = True
                    Me.txtEmail.Visible = True
                    ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    'One more condition added by Shubhrata Sep 14th 2006 TD Loans Phase 2

                ElseIf (g_string_FormName = "LoanOptions") Then
                    Me.LabelCity.Visible = False
                    Me.LabelState.Visible = False
                    Me.TextBoxCity.Visible = False
                    Me.TextBoxState.Visible = False

                    ' STARTS 2013-03-19    Dinesh Kanojia    Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    Me.lblEmail.Visible = False
                    Me.lblPhone.Visible = False
                    Me.txtPhone.Visible = False
                    Me.txtEmail.Visible = False
                    ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    'START : MMR | 2016.11.23 | YRS-AT-3145 | Displaying and hiding controls
                ElseIf (g_string_FormName = "NonRetiredQdro") Then
                    Me.LabelFundNo.Visible = True
                    Me.TextboxFundNo.Visible = True
                    Me.LabelFirstName.Visible = True
                    Me.TextBoxFirstName.Visible = True
                    Me.LabelLastName.Visible = True
                    Me.TextBoxLastName.Visible = True
                    Me.LabelCity.Visible = True
                    Me.TextBoxCity.Visible = True
                    Me.LabelState.Visible = True
                    Me.TextBoxState.Visible = True
                    Me.lblEmail.Visible = False
                    Me.txtEmail.Visible = False
                    Me.lblPhone.Visible = False
                    Me.txtPhone.Visible = False
                    'END : MMR | 2016.11.23 | YRS-AT-3145 | Displaying and hiding controls
                    'START : MMR | 2017.01.09 | YRS-AT-3299 | Displaying and hiding controls
                ElseIf (g_string_FormName = "RetiredQdro") Then
                    Me.LabelFundNo.Visible = True
                    Me.TextboxFundNo.Visible = True
                    Me.LabelFirstName.Visible = True
                    Me.TextBoxFirstName.Visible = True
                    Me.LabelLastName.Visible = True
                    Me.TextBoxLastName.Visible = True
                    Me.LabelCity.Visible = True
                    Me.TextBoxCity.Visible = True
                    Me.LabelState.Visible = True
                    Me.TextBoxState.Visible = True
                    Me.lblEmail.Visible = False
                    Me.txtEmail.Visible = False
                    Me.lblPhone.Visible = False
                    Me.txtPhone.Visible = False
                    'END : MMR | 2017.01.09 | YRS-AT-3299 | Displaying and hiding controls
                    'START : SB | 2017.09.13 | YRS-AT-3541 | Displaying and hiding controls for DC Edit Remaining Death Benefit form
                ElseIf (g_string_FormName = "DCToolsEditRemainingDeathBenefit") Then
                    Me.LabelCity.Visible = False
                    Me.LabelState.Visible = False
                    Me.TextBoxCity.Visible = False
                    Me.TextBoxState.Visible = False
                    Me.lblEmail.Visible = False
                    Me.lblPhone.Visible = False
                    Me.txtPhone.Visible = False
                    Me.txtEmail.Visible = False
                    Me.LabelFundNo.Visible = True
                    Me.TextboxFundNo.Visible = True
                    'END : SB | 2017.09.13 | YRS-AT-3541 | Displaying and hiding controls  for DC  Edit Remaining Death Benefit form 
                Else
                    Me.LabelCity.Visible = False
                    Me.LabelState.Visible = False
                    Me.TextBoxCity.Visible = False
                    Me.TextBoxState.Visible = False

                    ' STARTS 2013-03-19    Dinesh Kanojia    Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    Me.lblEmail.Visible = False
                    Me.lblPhone.Visible = False
                    Me.txtPhone.Visible = False
                    Me.txtEmail.Visible = False
                    ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance

                    Me.LabelFundNo.Visible = True
                    Me.TextboxFundNo.Visible = True
                End If

                'START | YRS-AT-2484 | Display link only for RMD Regenerate
                If (g_string_FormName = "RegenerateMRD") Then
                    lnkFundStatusChanged.Visible = True
                Else
                    lnkFundStatusChanged.Visible = False
                End If
                'START | YRS-AT-2484 | Display link only for RMD Regenerate

            Else
                'dgPager.Visible = True

            End If

            'Start: Added by Deven for Seamless Login logic, 16 Aug. 2010
            'MMR | 2018.10.03 | YRS-AT-4017 | Added Code in line no 512 to allow entry into individual loan processing screen from new Loan Admin console screen
            If Not Session("Seamless_From") Is Nothing And Not Page.IsPostBack Then
                If Convert.ToString(Session("Seamless_From")).ToLower = "personmaintenance" Or Convert.ToString(Session("Seamless_From")).ToLower = "loanoptions" _
                    Or Convert.ToString(Session("Seamless_From")).ToLower = "withdrawals" _
                    Or Convert.ToString(Session("Seamless_From")).ToLower = "loanrequestandprocessing" _
                Then 'AA:02.17.2016 YRS-AT-2640 Added to open the withdrawal page if the input passed as 'Withdrawals'
                    If Not Session("Seamless_SSN") Is Nothing Or Not Session("Seamless_Fund") Is Nothing Then
                        If Not Session("Seamless_SSN") Is Nothing Then
                            Me.TextBoxSSNo.Text = Convert.ToString(Session("Seamless_SSN")).Trim()
                        End If
                        If Not Session("Seamless_Fund") Is Nothing Then
                            Me.TextboxFundNo.Text = Convert.ToString(Session("Seamless_Fund")).Trim()
                        End If
                    ElseIf Not Session("Seamless_GuiPersId") Is Nothing Or Not Session("Seamless_guiFundEventID") Is Nothing Then
                        Dim l_dtPersonDataTable As DataTable
                        l_dtPersonDataTable = YMCARET.YmcaBusinessObject.FindInfo.GetPersonDetail(Convert.ToString(Session("Seamless_guiFundEventID")), Convert.ToString(Session("Seamless_GuiPersId")))
                        If Not l_dtPersonDataTable Is Nothing Then
                            If l_dtPersonDataTable.Rows.Count > 0 Then
                                Me.TextBoxSSNo.Text = Convert.ToString(l_dtPersonDataTable.Rows(0)("SSNo"))
                                Me.TextboxFundNo.Text = Convert.ToString(l_dtPersonDataTable.Rows(0)("FundNo"))
                            Else
                                'SS: 24-Feb-2011: BT-774,For wrong FundEventId application not giving any message. 
                                LabelNoDataFound.Visible = True
                                Me.gvFindInfo.Visible = False
                                'dgPager.Visible = False
                            End If
                        End If
                    End If



                    If Not String.IsNullOrEmpty(Me.TextBoxSSNo.Text.Trim()) Or Not String.IsNullOrEmpty(Me.TextboxFundNo.Text.Trim()) Then
                        ButtonFind_Click(sender, e)
                        If Not gvFindInfo.DataSource Is Nothing Then
                            Dim l_dsGridDataSource As DataView
                            l_dsGridDataSource = DirectCast(gvFindInfo.DataSource, DataView)
                            If l_dsGridDataSource.Count = 1 Then
                                gvFindInfo.SelectedIndex = 0
                                'AA:27.01.2014 : BT:2311 changed the method name
                                gvFindInfo_SelectedIndexChanged(sender, e)
                                'Start:AA:02.17.2016 YRS-AT-2640 Added to open the withdrawal page if multiple fundevents exists for the person as we have fundeventid in requestid
                            ElseIf l_dsGridDataSource.Count > 1 AndAlso Convert.ToString(Session("Seamless_From")).ToLower = "withdrawals" Then
                                For iCount As Int32 = 0 To gvFindInfo.Rows.Count - 1
                                    If Session("Seamless_guiFundEventID") = gvFindInfo.Rows(iCount).Cells(5).Text Then
                                        gvFindInfo.SelectedIndex = iCount
                                        gvFindInfo_SelectedIndexChanged(sender, e)
                                        Exit For
                                    End If
                                Next
                            End If
                            'End:AA:02.17.2016 YRS-AT-2640 Added to open the withdrawal page if multiple fundevents exists for the person as we have fundeventid in requestid
                        End If
                    End If

                    Session("Seamless_From") = Nothing
                    Session("Seamless_Fund") = Nothing
                    Session("Seamless_SSN") = Nothing
                    Session("Seamless_GuiPersId") = Nothing
                    Session("Seamless_guiFundEventID") = Nothing
                End If
            End If
            'End: Seamless Login logic

        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            'Throw ex
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindInfo_pageload", ex)
        End Try
    End Sub

    Function PopulateData(ByVal parameter_load As LoadDatasetMode)
        'Dim cache As CacheManager
        Dim l_PagingOn As Boolean
        Dim dvFindinfo As DataView
        Dim Sorting As GridViewCustomSort
        Dim strName As String
        Try
            strName = Convert.ToString(Request.QueryString.Get("Name"))
            If parameter_load = LoadDatasetMode.Table Then
                'Sanket Vaidya : BT-798 System should not allow disability retirement for QD and BF fundevents
                Dim retirementType As String = Request.QueryString.Get("RetType")
                If retirementType <> String.Empty AndAlso retirementType = "Disability" Then
                    g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.TextBoxSSNo.Text.Trim(), Me.TextboxFundNo.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), strName, Me.TextBoxCity.Text.Trim(), Me.TextBoxState.Text.Trim(), retirementType)
                Else
                    'Start:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to get the rollover details- As rollover query and person maintenance query are same
                    If strName = "Rollover" Then
                        strName = "Person"
                    End If
                    'End:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to get the rollover details- As rollover query and person maintenance query are same
                    ' STARTS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    'g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.TextBoxSSNo.Text.Trim(), Me.TextboxFundNo.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), Convert.ToString(Request.QueryString.Get("Name")), Me.TextBoxCity.Text.Trim(), Me.TextBoxState.Text.Trim())
                    'Start:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - updated the query string name with variable
                    g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.TextBoxSSNo.Text.Trim(), Me.TextboxFundNo.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), strName, Me.TextBoxCity.Text.Trim(), Me.TextBoxState.Text.Trim(), txtEmail.Text.Trim, txtPhone.Text.Trim)
                    'End:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - updated the query string name with variable
                    ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                End If
                Me.SessionDataSetg_dataset_dsMemberInfo = g_dataset_dsMemberInfo
            Else
                g_dataset_dsMemberInfo = Me.SessionDataSetg_dataset_dsMemberInfo
            End If
            ''cache = CacheFactory.GetCacheManager()
            ''cache.Add("Search_Info", g_dataset_dsMemberInfo)


            If Not g_dataset_dsMemberInfo Is Nothing Then

                'l_PagingOn = g_dataset_dsMemberInfo.Tables(0).Rows.Count > 5000

                If Me.gvFindInfo.PageIndex >= Me.SessionPageCount And Me.SessionPageCount <> 0 Then Exit Function

                If (g_dataset_dsMemberInfo.Tables("Persons").Rows.Count > 0) Then

                    ' START : 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    If txtEmail.Text.Trim = "" And g_dataset_dsMemberInfo.Tables("Persons").Columns.Contains("Email") Then
                        g_dataset_dsMemberInfo.Tables("Persons").Columns.RemoveAt(11)
                    End If

                    If txtPhone.Text.Trim = "" And g_dataset_dsMemberInfo.Tables("Persons").Columns.Contains("ContactNo.") Then
                        g_dataset_dsMemberInfo.Tables("Persons").Columns.RemoveAt(10)
                    End If

                    ' END : 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    Session("ds") = g_dataset_dsMemberInfo
                    LabelNoDataFound.Visible = False
                    Me.gvFindInfo.Visible = True

                    'If l_PagingOn Then
                    '    'dgPager.Visible = True
                    '    gvFindInfo.AllowPaging = True
                    'Else
                    '    dgPager.Visible = False
                    '    gvFindInfo.AllowPaging = False
                    'End If




                    If parameter_load = LoadDatasetMode.Table And l_PagingOn Then

                        'gvFindInfo.AllowPaging = False
                        gvFindInfo.AllowPaging = True
                        gvFindInfo.PageIndex = 0
                        gvFindInfo.PageSize = 15
                        'dgPager.Grid = gvFindInfo
                        'dgPager.PagesToDisplay = 10
                        'dgPager.Visible = True
                        'dgPager.CurrentPage = 0
                    Else

                    End If
                Else
                    'dgPager.Visible = False
                End If

                Me.gvFindInfo.SelectedIndex = -1
                Me.gvFindInfo.PageIndex = 0
                dvFindinfo = g_dataset_dsMemberInfo.Tables(0).DefaultView
                'AA:17.12.2013 : BT:2311 Added to sort records
                If Session("FindInfo_Sort") IsNot Nothing Then
                    'dvFindinfo.Sort = Session("FindInfo_Sort")
                    Sorting = Session("FindInfo_Sort")
                    dvFindinfo.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
                End If
                If Session("gvFindInfo_PageIndex") IsNot Nothing Then
                    Me.gvFindInfo.PageIndex = Session("gvFindInfo_PageIndex")
                End If
                Me.gvFindInfo.DataSource = dvFindinfo
                'Me.SessionPageCount = Me.gvFindInfo.PageCount
                Me.gvFindInfo.DataBind()


                'CommonModule.HideColumnsinDataGrid(g_dataset_dsMemberInfo, Me.DataGridFindInfo, "PersID,FundIdNo,FundUniqueId")


            Else
                LabelNoDataFound.Visible = True
                Me.gvFindInfo.Visible = False
                'dgPager.Visible = False

            End If

        Catch ex As SqlException
            LabelNoDataFound.Visible = True
            Me.gvFindInfo.Visible = False
            'dgPager.Visible = False

        Catch ex As Exception
            'Throw ex
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("PopulateData", ex)

        End Try

    End Function

    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            Session("Person_Info") = Nothing
            Session("WithDrawn_member") = Nothing
            Session("FindInfo_Sort") = Nothing
            'AA:27.01.2014 : BT:2311 Added to clear the grid selected index session
            Session("Grid_Index") = Nothing
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")    'NP:PS:2007.09.05 - Allowing the insertion of hyphen's in the SSN
            If Request.QueryString.Get("Name") = "Person" Then

                ' START : 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                If (TextBoxFirstName.Text.Trim = "" And TextBoxLastName.Text.Trim = "" And TextboxFundNo.Text.Trim = "" And TextBoxSSNo.Text.Trim = "" And TextBoxCity.Text.Trim = "" And TextBoxState.Text.Trim = "" And txtEmail.Text.Trim = "" And txtPhone.Text.Trim = "") Then
                    'If (TextBoxFirstName.Text.Trim = "" And TextBoxLastName.Text.Trim = "" And TextboxFundNo.Text.Trim = "" And TextBoxSSNo.Text.Trim = "" And TextBoxCity.Text.Trim = "" And TextBoxState.Text.Trim = "") Then
                    ' END : 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    'MessageBox.Show(PlaceHolder1, "YRS", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                    'Start -- Manthan Rajguru | 2016.01.12 | YRS-AT-2485 | Message displayed if SSNO input is * and other fields are blank
                ElseIf (TextBoxFirstName.Text.Trim = "" And TextBoxLastName.Text.Trim = "" And TextboxFundNo.Text.Trim = "" And TextBoxSSNo.Text.Trim = "*" And TextBoxCity.Text.Trim = "" And TextBoxState.Text.Trim = "" And txtEmail.Text.Trim = "" And txtPhone.Text.Trim = "") Then
                    HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                    'End -- Manthan Rajguru | 2016.01.12 | YRS-AT-2485 | Message displayed if SSNO input is * and other fields are blank
                Else
                    PopulateData(LoadDatasetMode.Table)
                End If
            Else
                If (TextBoxFirstName.Text = "" And TextBoxLastName.Text = "" And TextboxFundNo.Text = "" And TextBoxSSNo.Text = "") Then
                    'MessageBox.Show(PlaceHolder1, "YRS", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                    'Start -- Manthan Rajguru | 2016.01.12 | YRS-AT-2485 | Message displayed if SSNO input is * and other fields are blank
                ElseIf (TextBoxFirstName.Text = "" And TextBoxLastName.Text = "" And TextboxFundNo.Text = "" And TextBoxSSNo.Text = "*") Then
                    HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                    'End -- Manthan Rajguru | 2016.01.12 | YRS-AT-2485 | Message displayed if SSNO input is * and other fields are blank
                Else
                    PopulateData(LoadDatasetMode.Table)
                End If
            End If


        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            'Throw ex
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindInfo-- ButtonFind_click", ex)
        End Try
    End Sub
    'AA:17.12.2013 : BT:2311 Added page indexing for the selected records
    Private Sub gvFindInfo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvFindInfo.PageIndexChanging
        Try
            Me.gvFindInfo.SelectedIndex = -1
            Me.gvFindInfo.PageIndex = e.NewPageIndex
            Session("gvFindInfo_PageIndex") = e.NewPageIndex
            Dim dv As DataView
            Dim Sorting As GridViewCustomSort
            Try
                g_dataset_dsMemberInfo = DirectCast(Session("ds"), DataSet)
                If HelperFunctions.isNonEmpty(g_dataset_dsMemberInfo) Then
                    dv = g_dataset_dsMemberInfo.Tables(0).DefaultView
                    If Not Session("FindInfo_Sort") Is Nothing Then
                        Sorting = Session("FindInfo_Sort")
                        dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
                    End If
                    If Not Session("gvFindInfo_PageIndex") Is Nothing Then
                        Me.gvFindInfo.PageIndex = Session("gvFindInfo_PageIndex")
                    End If
                    gvFindInfo.DataSource = dv
                    gvFindInfo.DataBind()
                End If


            Catch ex As Exception
                HelperFunctions.LogException("FindInfo_gvFindInfo_pageIndexing", ex)
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            Finally
                dv = Nothing
            End Try

        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_gvFindInfo_pageIndexing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'AA:27.01.2014 : BT:2311 changed the method name
    Private Sub gvFindInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFindInfo.SelectedIndexChanged
        Dim i As Integer
        Dim retirementType As String
        Dim l_button_select As ImageButton
        Dim strName As String = String.Empty
        Dim strPIN As String = String.Empty
        Try

            'If Me.DataGridFindInfo.SelectedItem.Cells(10).Text.ToUpper.Trim() = "TRUE" Then
            '    MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "This person's data has been archived. Unable to continue process.", MessageBoxButtons.Stop, False)
            '    Exit Sub
            'End If


            Session("Txt_SSNo") = Me.TextBoxSSNo.Text.Trim
            Session("Txt_LastName") = Me.TextBoxLastName.Text.Trim
            Session("Txt_FirstName") = Me.TextBoxFirstName.Text.Trim
            Session("Txt_City") = Me.TextBoxCity.Text.Trim
            Session("Txt_State") = Me.TextBoxState.Text.Trim
            Session("Txt_FundNo") = Me.TextboxFundNo.Text.Trim
            Session("Grid_Index") = Me.gvFindInfo.SelectedIndex

            'Added by Dinesh
            Session("Txt_Phone") = Me.txtPhone.Text.Trim
            Session("Txt_Email") = Me.txtEmail.Text.Trim
            'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added below code to show the selected image on selecting record 
            While i < Me.gvFindInfo.Rows.Count
                l_button_select = TryCast(Me.gvFindInfo.Rows(i).Cells(0).Controls(0), ImageButton)
                If i = Me.gvFindInfo.SelectedIndex Then
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\selected.gif"
                    End If
                Else
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If

                End If
                i = i + 1
            End While

            'AA:27.01.2014 : BT:2311 cleared the commented data
            If (Convert.ToString(Request.QueryString.Get("Name")) = "Person") Then
                'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Getting PIN and Verifying
                strPIN = YMCARET.YmcaBusinessObject.FindInfo.GetUserPIN(Me.gvFindInfo.SelectedRow.Cells(1).Text)
                VerifyPIN(Me.gvFindInfo.SelectedRow.Cells(3).Text, Me.gvFindInfo.SelectedRow.Cells(4).Text, Me.gvFindInfo.SelectedRow.Cells(2).Text, strPIN, Me.gvFindInfo.SelectedRow.Cells(1).Text)

            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "Estimates") Then
                'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Getting PIN and Verifying
                strPIN = YMCARET.YmcaBusinessObject.FindInfo.GetUserPIN(Me.gvFindInfo.SelectedRow.Cells(1).Text)
                VerifyPIN(Me.gvFindInfo.SelectedRow.Cells(3).Text, Me.gvFindInfo.SelectedRow.Cells(4).Text, Me.gvFindInfo.SelectedRow.Cells(2).Text, strPIN, Me.gvFindInfo.SelectedRow.Cells(1).Text)

            ElseIf Request.QueryString.Get("Name") = "Refund" Then
                'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Getting PIN and Verifying
                strPIN = YMCARET.YmcaBusinessObject.FindInfo.GetUserPIN(Me.gvFindInfo.SelectedRow.Cells(4).Text)
                VerifyPIN(Me.gvFindInfo.SelectedRow.Cells(2).Text, Me.gvFindInfo.SelectedRow.Cells(3).Text, Me.gvFindInfo.SelectedRow.Cells(1).Text, strPIN, Me.gvFindInfo.SelectedRow.Cells(4).Text)
                'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added Loan request searching part into findinfo page
            ElseIf Request.QueryString.Get("Name") = "LoanRequestAndProcessing" Then
                'If Me.gvFindInfo.SelectedRow.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(1).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                Session("PersonID") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                Session("FundID") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(1).Text + "'")(0)("IsLocked").ToString().ToUpper() = "FALSE" Then
                    Session("IsAccountLock") = False
                ElseIf SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(1).Text + "'")(0)("IsLocked").ToString().ToUpper() = "TRUE" Then
                    Session("IsAccountLock") = True
                End If
                Response.Redirect("LoanInformation.aspx", False)
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "Process") Then

                '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
                'AA:27.01.2014 : BT:2311 changed to get the actual value of archived value to select the value from dataset.
                'If Me.gvFindInfo.SelectedRow.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("SSN ='" + Me.gvFindInfo.SelectedRow.Cells(1).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    'MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                '---------------------------------------------------------------------------------------


                Dim l_string_SSNO As String = Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(1).Text)
                If Me.IsPendingQDRO(l_string_SSNO) = True Then
                    'MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "This person has a pending QDRO. Unable to continue Retirement process.", MessageBoxButtons.Stop, False)
                    HelperFunctions.ShowMessageToUser("This person has a pending QDRO. Unable to continue Retirement process.", EnumMessageTypes.Error)
                Else
                    Session("PersId") = Nothing
                    Session("SSNo") = Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(1).Text)
                    Session("FundId") = Me.gvFindInfo.SelectedRow.Cells(6).Text
                    Session("RE_FundEventStatus") = Me.gvFindInfo.SelectedRow.Cells(7).Text.ToString().Trim().ToUpper()
                    '------------------------------------------------------------------------------
                    'Shashi Shekhar: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
                    Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(9).Text.Trim
                    '------------------------------------------------------------------------------
                    strName = Me.gvFindInfo.SelectedRow.Cells(2).Text + "," + Me.gvFindInfo.SelectedRow.Cells(3).Text
                    'Added by Sanket:for Adding one more parameter Retirement Type'Normal'or 'Disability'
                    retirementType = Request.QueryString.Get("RetType")
                    Response.Redirect("RetirementProcessingForm.aspx?RetType=" + retirementType, False)
                    '------------------------------------------------------------------------------
                End If
                'Shubhrata Sep 14th 2006,TD loans phase2
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "LoanOptions") Then

                '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
                'AA:27.01.2014 : BT:2311 changed to get the actual value of archived value to select the value from dataset.
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID='" + Me.gvFindInfo.SelectedRow.Cells(7).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                '---------------------------------------------------------------------------------------



                If Me.gvFindInfo.SelectedRow.Cells(1).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("SSNo") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                Else
                    Session("SSNo") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(2).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("LastName") = Me.gvFindInfo.SelectedRow.Cells(2).Text
                Else
                    Session("LastName") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(3).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("FirstName") = Me.gvFindInfo.SelectedRow.Cells(3).Text
                Else
                    Session("FirstName") = ""
                End If
                If Me.gvFindInfo.SelectedRow.Cells(4).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("MiddleName") = Me.gvFindInfo.SelectedRow.Cells(4).Text
                Else
                    Session("MiddleName") = ""
                End If
                If Me.gvFindInfo.SelectedRow.Cells(7).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("PersonID") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                Else
                    Session("PersonID") = ""
                End If
                If Me.gvFindInfo.SelectedRow.Cells(8).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("FundID") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                Else
                    Session("FundID") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("LoanStatus") = Me.gvFindInfo.SelectedRow.Cells(6).Text
                Else
                    Session("LoanStatus") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(9).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("LoanRequestId") = Me.gvFindInfo.SelectedRow.Cells(9).Text
                Else
                    Session("LoanRequestId") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(10).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("RequestDate") = Me.gvFindInfo.SelectedRow.Cells(10).Text
                Else
                    Session("RequestDate") = Nothing
                End If
                'YREN 3014 Jan 12th 2006 Shubhrata
                If Me.gvFindInfo.SelectedRow.Cells(11).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("OrigLoanNo") = Me.gvFindInfo.SelectedRow.Cells(11).Text
                Else
                    Session("OrigLoanNo") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(12).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("EmpEventId") = Me.gvFindInfo.SelectedRow.Cells(12).Text
                Else
                    Session("EmpEventId") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(13).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("YmcaId") = Me.gvFindInfo.SelectedRow.Cells(13).Text
                Else
                    Session("YmcaId") = Nothing
                End If
                If Me.gvFindInfo.SelectedRow.Cells(14).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("TDBalance") = Me.gvFindInfo.SelectedRow.Cells(14).Text
                Else
                    Session("TDBalance") = Nothing
                End If
                'YREN 3014 Jan 12th 2006 Shubhrata
                'Shubhrata YREN-3023 Jan 24th 2007
                If Me.gvFindInfo.SelectedRow.Cells(15).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("FundIdNo") = Me.gvFindInfo.SelectedRow.Cells(15).Text
                Else
                    Session("FundIdNo") = 0
                End If
                'Ashutosh Patil For Loan Details
                If Me.gvFindInfo.SelectedRow.Cells(16).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("LoanYMCANo") = Me.gvFindInfo.SelectedRow.Cells(16).Text
                Else
                    Session("LoanYMCANo") = 0
                End If
                ' Shashi Shekhar: 24-dec-2010: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
                If Me.gvFindInfo.SelectedRow.Cells(15).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(15).Text
                Else
                    Session("FundNo") = Nothing
                End If
                strName = Me.gvFindInfo.SelectedRow.Cells(2).Text + "," + Me.gvFindInfo.SelectedRow.Cells(3).Text

                Response.Redirect("LoanOptions.aspx", False)
                'Shubhrata Sep 14th 2006,TD loans phase2

                'Sanjeev Gupta 14th Oct 2011 BT ID - 925 Regenerate RMD
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "RegenerateMRD") Then
                Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                Session("SSNo") = Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(2).Text)
                Session("FundId") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                Response.Redirect("RegenerateMRD.aspx", False)
                strName = Me.gvFindInfo.SelectedRow.Cells(3).Text + "," + Me.gvFindInfo.SelectedRow.Cells(4).Text
                'Start:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to redirect rollover page
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "Rollover") Then
                Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                Session("FundId") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                Session("FundEvent") = Me.gvFindInfo.SelectedRow.Cells(9).Text.ToUpper()
                Response.Redirect("RollIn.aspx", False)
                'End:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to redirect rollover page
                'Start:BJ:15.04.2015 BT:2570 - YRS 5.0-2380 - Added to redirect Annuity Beneficiary page
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "Ann_Ben_Death") Then
                If Me.gvFindInfo.SelectedRow.Cells(7).Text.ToUpper().Trim = "TRUE" Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                SessionManager.SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityType = Me.gvFindInfo.SelectedRow.Cells(5).Text
                SessionManager.SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityId = Me.gvFindInfo.SelectedRow.Cells(6).Text

                Response.Redirect("AnnuityBeneficiaryDeath.aspx", False)
                'End:BJ:15.04.2015 BT:2570 - YRS 5.0-2380 - Added to redirect Annuity Beneficiary page
                'START : MMR | 2016.11.23 | YRS-AT-3145 | setting "IsAccountLock" value in a session
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "NonRetiredQdro") Then
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(SSNo).Text) Then
                    Session("PersSSID") = Me.gvFindInfo.SelectedRow.Cells(SSNo).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(fundIdNo).Text) Then
                    Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(fundIdNo).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(persId).Text) Then
                    Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(persId).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(fundEventId).Text) Then
                    Session("FundEventID") = Me.gvFindInfo.SelectedRow.Cells(fundEventId).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(QDRORequestId).Text) Then
                    Session("RequestID") = Me.gvFindInfo.SelectedRow.Cells(QDRORequestId).Text
                End If
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsArchived").ToString().ToUpper() = "FALSE" Then
                    Session("IsArchived") = False
                ElseIf SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    Session("IsArchived") = True
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                End If
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsLocked").ToString().ToUpper() = "FALSE" Then
                    Session("IsAccountLock") = False
                ElseIf SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsLocked").ToString().ToUpper() = "TRUE" Then
                    Session("IsAccountLock") = True
                End If
                Response.Redirect("NonRetiredQdro.aspx", False)
                'END : MMR | 2016.11.23 | YRS-AT-3145 | setting "IsAccountLock" value in a session

                'START: MMR | 2017.01.09 | YRS-AT-3299 | Setting session values for retired QDRO person
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "RetiredQdro") Then
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(SSNo).Text) Then
                    Session("PersSSID") = Me.gvFindInfo.SelectedRow.Cells(SSNo).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(lastName).Text) Then
                    Session("LastName") = Me.gvFindInfo.SelectedRow.Cells(lastName).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(firstName).Text) Then
                    Session("FirstName") = Me.gvFindInfo.SelectedRow.Cells(firstName).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(fundIdNo).Text) Then
                    Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(fundIdNo).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(persId).Text) Then
                    Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(persId).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(fundEventId).Text) Then
                    Session("FundEventID") = Me.gvFindInfo.SelectedRow.Cells(fundEventId).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(QDRORequestId).Text) Then
                    Session("RequestID") = Me.gvFindInfo.SelectedRow.Cells(QDRORequestId).Text
                End If
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsLocked").ToString().ToUpper() = "FALSE" Then
                    Session("IsAccountLock") = False
                ElseIf SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsLocked").ToString().ToUpper() = "TRUE" Then
                    Session("IsAccountLock") = True
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(fundStatus).Text) Then
                    Session("FundStatus") = Me.gvFindInfo.SelectedRow.Cells(fundStatus).Text
                End If
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsArchived").ToString().ToUpper() = "FALSE" Then
                    Session("IsArchived") = False
                ElseIf SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(persId).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    Session("IsArchived") = True
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                End If
                Response.Redirect("RetiredQdro.aspx", False)
                'END: MMR | 2017.01.09 | YRS-AT-3299 | Setting session values for retired QDRO person
                'START: PPP | 09/18/2017 | YRS-AT-3631 | Following condition will handle search result of DC Tools - Change Fund Status
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "DCToolsChangeFundStatus") Then
                Session("FundID") = Nothing

                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(isArchivedDCToolFundStatusIndex).Text) AndAlso Convert.ToBoolean(Me.gvFindInfo.SelectedRow.Cells(isArchivedDCToolFundStatusIndex).Text) Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If

                If Me.gvFindInfo.SelectedRow.Cells(fundEventIDDCToolFundStatusIndex).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("FundID") = Me.gvFindInfo.SelectedRow.Cells(fundEventIDDCToolFundStatusIndex).Text
                End If

                Response.Redirect("DCToolsChangeFundStatus.aspx", False)
                'END: PPP | 09/18/2017 | YRS-AT-3631 | Following condition will handle search result of DC Tools - Change Fund Status
                ' START | SR | 2017.09.21 | YRS-AT-3666 | Set session values for reverse Annuity
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "DCToolsReverseAnnuity") Then

                Session("FundNo") = Nothing
                Session("LastName") = Nothing
                Session("FirstName") = Nothing
                Session("PersId") = Nothing
                Session("FundEventID") = Nothing
                Session("FundStatus") = Nothing

                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(10).Text) AndAlso Convert.ToBoolean(Me.gvFindInfo.SelectedRow.Cells(10).Text) Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If

                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(7).Text) Then
                    Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(3).Text) Then
                    Session("LastName") = Me.gvFindInfo.SelectedRow.Cells(3).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(4).Text) Then
                    Session("FirstName") = Me.gvFindInfo.SelectedRow.Cells(4).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(1).Text) Then
                    Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                End If
                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(8).Text) Then
                    Session("FundEventID") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                End If

                If Not String.IsNullOrEmpty(Me.gvFindInfo.SelectedRow.Cells(6).Text) Then
                    Session("FundStatus") = Me.gvFindInfo.SelectedRow.Cells(6).Text
                End If

                Response.Redirect("DCToolsReverseAnnuity.aspx", False)
                ' END | SR | 2017.09.21 | YRS-AT-3666 | Set session values for reverse Annuity
                'START : SB | 2017.09.13 | YRS-AT-3541 | Setting session values for DC Edit Remaining Death Benefit  amount for participant 
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "DCToolsEditRemainingDeathBenefit") Then
                'Code to handle Archived Participants from from dataset
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID='" + Me.gvFindInfo.SelectedRow.Cells(6).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Sub
                End If

                If Me.gvFindInfo.SelectedRow.Cells(7).Text.Trim.ToUpper() <> "&NBSP;" Then
                    Session("FundID") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                Else
                    Session("FundID") = Nothing
                End If

                Response.Redirect("DCToolsEditRemainingDeathBenefit.aspx", False)
                'END : SB | 2017.09.13 | YRS-AT-3541 | Setting session values for DC Edit Remaining Death Benefit amount for participant
            End If
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_gvFindInfo_SelectIndexchange", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFirstName.Text = ""
            Me.TextboxFundNo.Text = ""
            Me.TextBoxLastName.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.TextBoxCity.Text = ""
            Me.TextBoxState.Text = ""
            Me.txtEmail.Text = ""
            Me.txtPhone.Text = ""
            'Me.DataGridFindInfo.DataSource = Nothing
            'Me.DataGridFindInfo.AllowPaging = False
            'dgPager.Visible = False


            'Me.DataGridFindInfo.

            'Me.LabelNoDataFound.Visible = False
        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            HelperFunctions.LogException("Find Info--ButtonClear", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)

        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Session("FindInfo_Sort") = Nothing
        Session("Page") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub gvFindInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFindInfo.RowDataBound
        Dim l_button_Select As ImageButton
        Dim sortingexpression As String
        Dim sortingdirection As String = String.Empty
        Try
            'l_button_Select = e.Row.FindControl("ImageButtonSelect")
            'If (e.Row.ItemIndex = gvFindInfo.SelectedIndex And gvFindInfo.SelectedIndex >= 0) Then
            '    l_button_Select.ImageUrl = "images\selected.gif"
            'End If            
            HelperFunctions.SetSortingArrows(Session("FindInfo_Sort"), e)
            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
                Select Case Request.QueryString.Get("Name")
                    Case "Person"
                        ' e.Row.Cells(6).Visible = False
                        e.Row.Cells(10).Visible = False 'Shashi Shekhar:2010-02-12 :Hide IsArchived Field in grid
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(7).Visible = False
                        If e.Row.Cells.Count > 8 Then
                            e.Row.Cells(8).Visible = False
                            e.Row.Cells(9).Visible = False
                        End If
                        'AA:27.01.2014 : BT:2311 added to show the labels with spaces
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        'modified by hafiz on 16-Apr-2007 for YREN-3257
                        'Case "Estimates"
                    Case "Estimates"
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(8).Visible = False
                        'added by hafiz on 16-Apr-2007 for YREN-3257
                        e.Row.Cells(9).Visible = False
                        e.Row.Cells(10).Visible = False  'Shashi Shekhar:2010-02-12 :Hide IsArchived Field in grid
                        'AA:27.01.2014 : BT:2311 added to show the labels with spaces
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                            DirectCast(e.Row.Cells(7).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund No"
                        End If
                    Case "Process"
                        e.Row.Cells(6).Visible = False
                        e.Row.Cells(7).Visible = False
                        e.Row.Cells(8).Visible = False  'Shashi Shekhar:2010-02-12 :Hide IsArchived Field in grid
                        e.Row.Cells(9).Visible = False 'Shashi Shekhar:24-Dec-2010 :Hide intFundNo Field in grid For YRS 5.0-450, BT-643
                        'Shubhrata Sep 14th 2006 TD Loans Phase 2
                        'AA:27.01.2014 : BT:2311 added to show the labels with spaces
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(2).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                    Case "LoanOptions"
                        e.Row.Cells(7).Visible = False
                        e.Row.Cells(8).Visible = False
                        e.Row.Cells(9).Visible = False
                        e.Row.Cells(10).Visible = False
                        e.Row.Cells(11).Visible = False
                        e.Row.Cells(12).Visible = False
                        e.Row.Cells(13).Visible = False
                        e.Row.Cells(14).Visible = False
                        e.Row.Cells(15).Visible = False
                        e.Row.Cells(16).Visible = False
                        e.Row.Cells(17).Visible = False  'Shashi Shekhar:2010-02-12 :Hide IsArchived Field in grid
                        'Shubhrata Sep 14th 2006 TD Loans Phase 2
                        'AA:27.01.2014 : BT:2311 added to show the labels with spaces
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(2).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Loan Status"
                        End If
                        'Sanjeev Gupta 14th Oct 2011 BT ID - 925 Regenerated RMD
                    Case "RegenerateMRD"
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(8).Visible = False
                        e.Row.Cells(9).Visible = False
                        e.Row.Cells(10).Visible = False
                        'AA:27.01.2014 : BT:2311 added to show the labels with spaces
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                            DirectCast(e.Row.Cells(7).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund No"
                        End If
                        'START | SR | 2016.07.05 | YRS-AT-2484 | Convert Term date  & Termed On to date format for column sorting
                        If e.Row.RowType = DataControlRowType.DataRow And lnkFundStatusChanged.Visible = True And lnkFundStatusChanged.Text = "Go back to Find screen" Then
                            e.Row.Cells(11).Text = Convert.ToDateTime(e.Row.Cells(11).Text).Date.ToShortDateString()
                            e.Row.Cells(12).Text = Convert.ToDateTime(e.Row.Cells(12).Text).Date.ToShortDateString()
                        End If
                        'END | SR | 2016.07.05 | YRS-AT-2484 | Convert Term date  & Termed On to date format for column sorting
                        'Start:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added Refund & Loan request searching part into findinfo page
                    Case "Refund"
                        e.Row.Cells(4).Visible = False
                        e.Row.Cells(5).Visible = False
                        e.Row.Cells(8).Visible = False
                        e.Row.Cells(9).Visible = False
                    Case "LoanRequestAndProcessing"
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(8).Visible = False
                        e.Row.Cells(9).Visible = False
                        e.Row.Cells(10).Visible = False
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                            DirectCast(e.Row.Cells(7).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund No." 'Anudeep A:24.09.2014 - BT:2625 :YRS 5.0-2405-added to show the header name as fund no
                        End If
                        'End:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added Refund & Loan request searching part into findinfo page                       
                        'Start:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to show / hide the columns in grid for rollover data
                    Case "Rollover"
                        e.Row.Cells(10).Visible = False
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(7).Visible = False
                        If e.Row.Cells.Count > 8 Then
                            e.Row.Cells(8).Visible = False
                            e.Row.Cells(9).Visible = False
                        End If
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        'End:AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to show / hide the columns in grid for rollover data
                        'Start:BJ:15.04.2015 BT:2570 - YRS 5.0-2380 - Added to show / hide the columns in grid for Annuity Beneficiary Death data
                    Case "Ann_Ben_Death"
                        e.Row.Cells(6).Visible = False
                        e.Row.Cells(7).Visible = False
                        'End:BJ:15.04.2015 BT:2570 - YRS 5.0-2380 - Added to show / hide the columns in grid for Annuity Beneficiary Death data
                        'START : MMR | 2016.11.23 | YRS-AT-3145 | Hiding unwanted columns and formating header name
                    Case "NonRetiredQdro"
                        e.Row.Cells(persId).Visible = False
                        e.Row.Cells(fundEventId).Visible = False
                        e.Row.Cells(QDRORequestId).Visible = False
                        e.Row.Cells(isArchived).Visible = False
                        e.Row.Cells(fundIdNo).Visible = False
                        e.Row.Cells(isAccountLocked).Visible = False
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(SSNo).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "SSN"
                            DirectCast(e.Row.Cells(lastName).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(firstName).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(middleName).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(fundStatus).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        'END : MMR | 2016.11.23 | YRS-AT-3145 | Hiding unwanted columns and formating header name
                        'START : MMR | 2016.11.23 | YRS-AT-3145 | Hiding unwanted columns and formating header name
                    Case "RetiredQdro"
                        e.Row.Cells(persId).Visible = False
                        e.Row.Cells(fundEventId).Visible = False
                        e.Row.Cells(QDRORequestId).Visible = False
                        e.Row.Cells(isArchived).Visible = False
                        e.Row.Cells(fundIdNo).Visible = False
                        e.Row.Cells(isAccountLocked).Visible = False
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(SSNo).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "SSN"
                            DirectCast(e.Row.Cells(lastName).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(firstName).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(middleName).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(fundStatus).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        'END : MMR | 2016.11.23 | YRS-AT-3145 | Hiding unwanted columns and formating header name
                        'START: PPP | 09/18/2017 | YRS-AT-3631 | Handles result grid of DC Tools - Change Fund Status
                    Case "DCToolsChangeFundStatus"
                        e.Row.Cells(persIDDCToolFundStatusIndex).Visible = False
                        e.Row.Cells(fundEventIDDCToolFundStatusIndex).Visible = False
                        e.Row.Cells(fundNoDCToolFundStatusIndex).Visible = False
                        e.Row.Cells(isArchivedDCToolFundStatusIndex).Visible = False
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(ssnDCToolFundStatusIndex).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "SSN"
                            DirectCast(e.Row.Cells(lastNameDCToolFundStatusIndex).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(firstNameDCToolFundStatusIndex).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(middleNameDCToolFundStatusIndex).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(fundStatusDCToolFundStatusIndex).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        'END: PPP | 09/18/2017 | YRS-AT-3631 | Handles result grid of DC Tools - Change Fund Status

                        'START : SB | 2017.09.13 | YRS-AT-3541 | Hiding unwanted columns and formating header name
                    Case "DCToolsEditRemainingDeathBenefit"
                        e.Row.Cells(6).Visible = False
                        e.Row.Cells(7).Visible = False
                        e.Row.Cells(8).Visible = False
                        e.Row.Cells(9).Visible = False
                        e.Row.Cells(10).Visible = False 'Hide IsArchived Field in grid

                        'To show the labels with spaces
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(2).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        'END : SB | 2017.09.13 | YRS-AT-3541 | Hiding unwanted columns and formating header name
                        ' START | SR | 2017.09.21 | YRS-AT-3666 | Set rows visibility for reverse Annuity.
                    Case "DCToolsReverseAnnuity"
                        e.Row.Cells(10).Visible = False
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(7).Visible = False
                        If e.Row.Cells.Count > 8 Then
                            e.Row.Cells(8).Visible = False
                            e.Row.Cells(9).Visible = False
                        End If
                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Last Name"
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "First Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Middle Name"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Fund Status"
                        End If
                        ' END | SR | 2017.09.21 | YRS-AT-3666 | Set rows visibility for reverse Annuity.
                    Case Else
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(7).Visible = False
                        If e.Row.Cells.Count > 8 Then
                            e.Row.Cells(8).Visible = False
                        End If
                End Select
            End If
            'If e.Row.ItemType = ListItemType.Header Then
            '    e.Row.Cells(6).Text = "FundStatus"
            'End If
        Catch ex As Exception
            'commented by hafiz on 29May2007 - 
            'Since ButtonFind is redirecting to error page for raised exception 
            '& if this event as well redirects to error page then proper error message is not getting displayed.
            'ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, True)
            HelperFunctions.LogException("FindInfo_gvFindInfo_RowdataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvFindInfo_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFindInfo.Sorting
        Dim dv As New DataView
        Try
            Me.gvFindInfo.SelectedIndex = -1
            If Not Session("ds") Is Nothing Then
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsMemberInfo = DirectCast(Session("ds"), DataSet)
                dv = g_dataset_dsMemberInfo.Tables(0).DefaultView
                dv.Sort = SortExpression
                HelperFunctions.gvSorting(Session("FindInfo_Sort"), e.SortExpression, dv)
                If Not Session("gvFindInfo_PageIndex") Is Nothing Then
                    Me.gvFindInfo.PageIndex = Session("gvFindInfo_PageIndex")
                End If
                Me.gvFindInfo.DataSource = Nothing
                Me.gvFindInfo.DataSource = dv
                Me.gvFindInfo.DataBind()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_gvFindInfo_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            'dv.Dispose()
        End Try
    End Sub

    'Private Sub dgPager_PageChanged(ByVal PgNumber As Integer) Handles dgPager.PageChanged
    '    Try
    '        If Me.IsPostBack Then
    '            PopulateData(LoadDatasetMode.Session)
    '            'lblFrom.Text = dgPager.CurrentPageStart
    '            'lblTo.Text = dgPager.CurrentPageEnd
    '            'lblTotal.Text = dgPager.TotalRecords
    '            'lblCurrentPage.Text = dgPager.CurrentPage
    '            'lblTotalPages.Text = dgPager.Totalpages
    '        End If
    '    Catch ex As Exception
    '        'Dim l_String_Exception_Message As String
    '        'l_String_Exception_Message = ex.Message.Trim.ToString()
    '        'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '        Throw ex
    '    End Try
    'End Sub

    Private Function IsPendingQDRO(ByVal p_string_SSNO As String) As Boolean
        Dim l_DataTable As DataTable
        Dim l_bool_Result As Boolean
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.FindInfo.GetQDRODetails_BYSSNO(p_string_SSNO)
            If Not (l_DataTable) Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    l_bool_Result = True
                Else
                    l_bool_Result = False
                End If
            Else
                l_bool_Result = False
            End If

            l_DataTable = Nothing
            Return l_bool_Result
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_IspendingQDRO", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function

    'NP:2008.06.13 - Adding a function call to clear all session variables used by Retirement Estimates screen
    Private Sub ClearSession_RetireEstimates()
        Try
            Session("RE_FundEventStatus") = Nothing
            Session("SSNo") = Nothing
            Session("FundId") = Nothing
            Session("PersID") = Nothing
            Session("PlanType") = Nothing
            Session("RE_FundEventStatus") = Nothing

            Session("PercentageSelected") = Nothing
            Session("Print") = Nothing

            Session("mnyDeathBenefitAmount") = Nothing
            Session("Page") = Nothing
            Session("StrReportName") = Nothing
            Session("GUID") = Nothing

            Session("DefaultEmpEventID") = Nothing
            Session("dsBasicAccounts") = Nothing
            Session("dsElectiveAccountsDet") = Nothing
            Session("EarliestStartWorkDate") = Nothing
            Session("EmpHistoryInfoNotset") = Nothing
            Session("employmentSalaryInformation") = Nothing
            Session("LatestStartWorkDate") = Nothing
            Session("NoActivePlans") = Nothing
            Session("ProceedWithEstimation") = Nothing
            Session("RE_NoBasicAccountContribution") = Nothing
            Session("selectedEmployment") = Nothing
            Session("UsePlan") = Nothing

            'Phase IV - Start
            Session("dsPersonEmploymentDetails") = Nothing
            Session("dsElectiveAccounts") = Nothing
            Session("dsParticipantBeneficiaries") = Nothing
            Session("businessLogic") = Nothing
            Session("OrgBenTypeIsQDRO") = Nothing
            Session("RE_RetirementDate") = Nothing
            'Phase IV - End
            'Ashish:2010-06-21 YRS 5.0-1115 ,Start
            Session("RE_PRAType") = Nothing
            Session("FundNo") = Nothing
            'Ashish:2010-06-21 YRS 5.0-1115 ,End

            'HR - Start - 10/04/2012 - YRS 5.0-1541
            Session("OrgBenTypeIsQDROorRBEN") = Nothing
            Session("RP_ExactAgeEffDate") = Nothing

            Session("AreJAnnuityUnavailable") = Nothing
            Session("InputBeneficiaryType") = Nothing

            Session("RetireeBirthDatePresent") = Nothing
            Session("dsSelectedParticipantBeneficiary") = Nothing

            Session("isYmcaLegacyAcctTotalExceed") = Nothing
            Session("isYmcaAcctTotalExceed") = Nothing
            'HR - End - 10/04/2012 - YRS 5.0-1541
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_ClearSession_retireEstimates", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
#Region "PIN NUMBER"


    'Start: AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To get & verify the PIN value
    Public Function VerifyPIN(ByRef strFirstName As String, ByRef strLastName As String, ByRef strSSN As String, ByRef strPIN As String, ByRef strGuiPersid As String)
        Dim arrPINdtls(1) As String
        Dim dsMetaconfiguration As DataSet
        Try
            arrPINdtls = Session("PersPIN")
            If strPIN.Trim.Length = 4 And (arrPINdtls Is Nothing OrElse arrPINdtls(0) <> strGuiPersid) Then
                Try
                    dsMetaconfiguration = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.SearchConfigurationMaintenance("ENABLE_PIN_VERIFICATION_PROCESS")
                Catch ex As Exception

                End Try
                If HelperFunctions.isEmpty(dsMetaconfiguration) Then
                    Throw New Exception("'ENABLE_PIN_VERIFICATION_PROCESS' key not defined in atsMetaConfiguration")
                ElseIf dsMetaconfiguration.Tables(0).Rows(0)("Value").ToString().ToLower().Trim() = "true" Then
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Verify", "VerifyPIN('" + strFirstName + "','" + strLastName + "','" + strSSN + "','" + strPIN + "');", True)
                ElseIf dsMetaconfiguration.Tables(0).Rows(0)("Value").ToString().ToLower().Trim() = "false" Then
                    Openselectedpage()
                End If
            Else
                Openselectedpage()
            End If
            ReDim arrPINdtls(1)
            arrPINdtls(0) = strGuiPersid
            arrPINdtls(1) = strPIN
            Session("PersPIN") = arrPINdtls
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_VerifYPIN", ex)
            Throw ex
        End Try
    End Function

    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            LogPINActions(PINVerificationActionTypes.YES)
            Openselectedpage()
        Catch ex As Exception
            Session("PersPIN") = Nothing
            HelperFunctions.LogException("FindInfo_btnYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            LogPINActions(PINVerificationActionTypes.NO)
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Close", "ClosePINdialog();", True)
            Session("PersPIN") = Nothing
        Catch ex As Exception
            Session("PersPIN") = Nothing
            HelperFunctions.LogException("FindInfo_btnNo_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim arrPINdtls(1) As String
        Try
            LogPINActions(PINVerificationActionTypes.CANCEL)
            Openselectedpage()
            arrPINdtls = Session("PersPIN")
            arrPINdtls(0) = ""
            Session("PersPIN") = arrPINdtls
        Catch ex As Exception
            Session("PersPIN") = Nothing
            HelperFunctions.LogException("FindInfo_btnCancel_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'To open the page 
    Public Function Openselectedpage()
        Dim strName As String = String.Empty

        Try
            If (Convert.ToString(Request.QueryString.Get("Name")) = "Person") Then
                Select Case (Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(9).Text.ToUpper.Trim))
                    Case "RD", "DR", "RP", "RQ", "DQ", "RQTA"
                        Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                        Session("SSNo") = Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(2).Text)
                        Session("FundStatusType") = Me.gvFindInfo.SelectedRow.Cells(9).Text.Trim.ToUpper()
                        Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                        Session("FundId") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                        strName = Me.gvFindInfo.SelectedRow.Cells(3).Text + "," + Me.gvFindInfo.SelectedRow.Cells(4).Text
                        'added By Dilip 
                        Session("ISRetired") = True
                        'START : VC | 2018.09.05 | YRS-AT-4018 -  Commented existing code and added code to redirect to person maintenance even participant is retired
                        'Response.Redirect("RetireesInformationWebform.aspx", False)
                        If Session("FlagLoans") = "Loans" Then
                            Session("FundStatus") = Me.gvFindInfo.SelectedRow.Cells(6).Text.Trim.ToUpper()
                            Response.Redirect("ParticipantsInformation.aspx", False)
                        Else
                            Response.Redirect("RetireesInformationWebform.aspx", False)
                        End If
                        'END : VC | 2018.09.05 | YRS-AT-4018 -  Commented existing code and added code to redirect to person maintenance even participant is retired
                    Case Else
                        Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                        Session("SSNo") = Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(2).Text)
                        Session("FundStatus") = Me.gvFindInfo.SelectedRow.Cells(6).Text.Trim.ToUpper()
                        Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                        Session("FundId") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                        Session("FundStatusType") = Me.gvFindInfo.SelectedRow.Cells(9).Text.Trim.ToUpper()
                        strName = Me.gvFindInfo.SelectedRow.Cells(3).Text + "," + Me.gvFindInfo.SelectedRow.Cells(4).Text
                        'added By Dilip 
                        Session("ISRetired") = False
                        Response.Redirect("ParticipantsInformation.aspx", False)
                End Select
                'Swopna Phase IV changes 8 Apr,2008**************end

            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "Estimates") Then

                '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
                'AA:27.01.2014 : BT:2311 changed to get the actual value of archived value to select the value from dataset.
                'If Me.gvFindInfo.SelectedRow.Cells(10).Text.ToUpper.Trim() = "TRUE" Then
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersID ='" + Me.gvFindInfo.SelectedRow.Cells(1).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Function
                End If
                '---------------------------------------------------------------------------------------

                ClearSession_RetireEstimates()  'NP:2008.06.13 - Calling function to clear all session variables used by Retirement Estimates
                Session("PersId") = Me.gvFindInfo.SelectedRow.Cells(1).Text
                Session("SSNo") = Convert.ToString(Me.gvFindInfo.SelectedRow.Cells(2).Text)
                Session("FundId") = Me.gvFindInfo.SelectedRow.Cells(8).Text
                'added by hafiz on for YREN-3257
                Session("RE_FundEventStatus") = Me.gvFindInfo.SelectedRow.Cells(9).Text.ToString().Trim().ToUpper()
                'Ashish:2010.06.21 YRS 5.0-1115
                Session("FundNo") = Me.gvFindInfo.SelectedRow.Cells(7).Text
                strName = Me.gvFindInfo.SelectedRow.Cells(3).Text + "," + Me.gvFindInfo.SelectedRow.Cells(4).Text
                Response.Redirect("RetirementEstimateWebForm.aspx", False)

            ElseIf Request.QueryString.Get("Name") = "Refund" Then

                Session("PersonID") = Me.gvFindInfo.SelectedRow.Cells(4).Text
                Session("FundID") = Me.gvFindInfo.SelectedRow.Cells(5).Text
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersonID ='" + Me.gvFindInfo.SelectedRow.Cells(4).Text + "'")(0)("IsArchived").ToString().ToUpper() = "TRUE" Then
                    HelperFunctions.ShowMessageToUser("Selected participant's data has been archived, Please unarchive the data to continue the process.", EnumMessageTypes.Error)
                    Exit Function
                End If
                If SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersonID ='" + Me.gvFindInfo.SelectedRow.Cells(4).Text + "'")(0)("IsLocked").ToString().ToUpper() = "FALSE" Then
                    Session("IsAccountLock") = False
                ElseIf SessionDataSetg_dataset_dsMemberInfo.Tables(0).Select("PersonID ='" + Me.gvFindInfo.SelectedRow.Cells(4).Text + "'")(0)("IsLocked").ToString().ToUpper() = "TRUE" Then
                    Session("IsAccountLock") = True
                End If

                If Page.Request.QueryString("RT") Is Nothing Then
                    Response.Redirect("RefundRequestForm.aspx", False)
                Else
                    Dim RefundType As String
                    RefundType = CType(Page.Request.QueryString("RT"), String)
                    If RefundType.Trim = "SPEC" Then
                        Response.Redirect("RefundRequestForm.aspx?RT=SPEC", False)
                    ElseIf RefundType.Trim = "DISAB" Then
                        Response.Redirect("RefundRequestForm.aspx?RT=DISAB", False)
                        'START : ML | 10-04-2020 | YRS-AT-4854 |  Redirect to new Refund request page for Covid19 Participant Withdrawal Process
                    ElseIf RefundType.Trim = "C19" Then
                        Response.Redirect("RefundRequestForm_C19.aspx", False)
                        'END : ML | 10-04-2020 | YRS-AT-4854 |  Redirect to new Refund request page for Covid19 Participant Withdrawal Process
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_Openselectedpage", ex)
            Throw ex
        End Try
    End Function
    'Log the PIN actions
    Public Function LogPINActions(ByRef PINAction As YMCAObjects.PINVerificationActionTypes)
        Dim PIN As New PINVerificationEventArgs
        Dim arrPINdtls(1) As String
        Try
            arrPINdtls = Session("PersPIN")
            If (Convert.ToString(Request.QueryString.Get("Name")) = "Person") Then
                'Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                'PIN.guiPersId = Guid.Parse(Me.gvFindInfo.SelectedRow.Cells(1).Text)
                PIN.guiPersId = Me.gvFindInfo.SelectedRow.Cells(1).Text
                'End:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                PIN.PINModule = PINVerificationModule.MAINTENANCEPERSON
            ElseIf (Convert.ToString(Request.QueryString.Get("Name")) = "Estimates") Then
                'Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                'PIN.guiPersId = Guid.Parse(Me.gvFindInfo.SelectedRow.Cells(1).Text)
                PIN.guiPersId = Me.gvFindInfo.SelectedRow.Cells(1).Text
                'End:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                PIN.PINModule = PINVerificationModule.RETIREMENTESTIMATE
            ElseIf Request.QueryString.Get("Name") = "Refund" Then
                'Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                'PIN.guiPersId = Guid.Parse(Me.gvFindInfo.SelectedRow.Cells(4).Text)
                PIN.guiPersId = Me.gvFindInfo.SelectedRow.Cells(4).Text
                'End:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                PIN.PINModule = PINVerificationModule.WITHDRAWAL
            End If
            PIN.PinNumber = arrPINdtls(1)
            PIN.IsSucceeded = True
            PIN.PINActionType = PINAction
            YMCARET.YmcaBusinessObject.YMCAHandler.OnPINVerification(HttpContext.Current, PIN)
        Catch ex As Exception
            HelperFunctions.LogException("FindInfo_LogPINActions", ex)
            Throw ex
        End Try
    End Function
    'End: AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To get & verify the PIN value
#End Region

    ' START | SR | 2016.06.30 | YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
    Private Sub lnkFundStatusChanged_Click(sender As Object, e As EventArgs) Handles lnkFundStatusChanged.Click
        ' If user select to "List All RMD eligible Persons" then hide all the form controls & display data in grid
        If Convert.ToString(Request.QueryString.Get("Name")) = "RegenerateMRD" And lnkFundStatusChanged.Text = "List All RMD Eligible Terminated Persons" Then
            Dim dvFindinfo As DataView
            Dim Sorting As GridViewCustomSort
            ' Hide all the form controls & display data in grid 
            Me.TextBoxSSNo.Visible = False
            Me.TextBoxLastName.Visible = False
            Me.TextBoxFirstName.Visible = False
            Me.TextboxFundNo.Visible = False
            Me.LabelSSNo.Visible = False
            Me.LabelLastName.Visible = False
            Me.LabelFirstName.Visible = False
            Me.LabelFundNo.Visible = False
            Me.lblSearchSSN.Visible = False
            Me.ButtonFind.Visible = False
            Me.ButtonClear.Visible = False
            lnkFundStatusChanged.Text = "Go back to Find screen"
            'Get the list of all RMD Eligible Terminated Persons and bind it to datagrid
            g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.FindInfo.GetFundStatusChangedParticipant()
            If (HelperFunctions.isNonEmpty(g_dataset_dsMemberInfo)) Then
                gvFindInfo.AllowPaging = True
                gvFindInfo.PageIndex = 0
                gvFindInfo.PageSize = 15
                Me.gvFindInfo.SelectedIndex = -1
                dvFindinfo = g_dataset_dsMemberInfo.Tables(0).DefaultView
                Session("ds") = g_dataset_dsMemberInfo
                If Session("FindInfo_Sort") IsNot Nothing Then
                    'dvFindinfo.Sort = Session("FindInfo_Sort")
                    Sorting = Session("FindInfo_Sort")
                    dvFindinfo.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
                End If
                If Session("gvFindInfo_PageIndex") IsNot Nothing Then
                    Me.gvFindInfo.PageIndex = Session("gvFindInfo_PageIndex")
                End If
                Me.gvFindInfo.DataSource = dvFindinfo
                Me.gvFindInfo.DataBind()
                Me.gvFindInfo.Visible = True
            End If

            ' If user select to "Back" then reset form to origional state.
        ElseIf Convert.ToString(Request.QueryString.Get("Name")) = "RegenerateMRD" And lnkFundStatusChanged.Text = "Go back to Find screen" Then
            'Reset form to origional state.
            Me.TextBoxSSNo.Visible = True
            Me.TextBoxLastName.Visible = True
            Me.TextBoxFirstName.Visible = True
            Me.TextboxFundNo.Visible = True
            Me.LabelSSNo.Visible = True
            Me.LabelLastName.Visible = True
            Me.LabelFirstName.Visible = True
            Me.LabelFundNo.Visible = True
            Me.lblSearchSSN.Visible = True
            Me.ButtonFind.Visible = True
            Me.ButtonClear.Visible = True
            lnkFundStatusChanged.Text = "List All RMD Eligible Terminated Persons"
            Me.gvFindInfo.DataSource = Nothing
            Me.gvFindInfo.DataBind()
            Me.gvFindInfo.Visible = False
        End If
    End Sub
    ' END | SR | 2016.06.30 | YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
End Class

