'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	SessionManager.vb
' Author Name		:	Shashi Shekhar Singh  
' Employee ID		:	51426
' Email				:	shashi.singh@3i-infotech.com
' Contact No		:	8684
' Creation Time		:	03-April-2010
' Program Specification Name	: YMCA_PS_Maintenance_YMCA
'Description        :  To handle sessions in application through this class as one place 
'*******************************************************************************

'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Shashi Shekhar     2010-04-14      Exclude Contact info (i.e Email,Telephone,extn properties)from YMCAMaintenance as per Gemini-1051
'Anudeep A          2014.08.12      BT:2460:YRS 5.0-2331 - Death Benefit Application Form 
'Shashank           2014.08.18      BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
'Jagadeesh          2015.04.28      BJ 2015.04.15- YRS 5.0-2380 :Added session properties to Annuity Beneficiary Death and Annuity Beneficiary Death Followup
'Anudeep A          2015.12.10      YRS-AT-2662 - YRS enh: follow up to Loans release - nightly sql server job for Offset and Unfreeze processing
'Anudeep A          2016.18.04      YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
'Anudeep A          2016.04.12      YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Santosh B          2017.04.28      YRS-AT-3400 - YRS enh: due MAY 2017 - RMD Print Letters- Letter to Non-respondents (new screen). (2 of 3 tickets) (TrackIT 29186)
'              						YRS-AT-3401 - YRS enh: due MAY 2017 - RMD Print Letters- Satisfied but not elected (new screen). (3 of 3 tickets) (TrackIT 29186&# 
'Manthan Rajguru    2017.05.10      YRS-AT-3205 - YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)
'Pramod P. Pokale   2017.09.14      YRS-AT-3665 - YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
'Vinayan C          2018.07.27      YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'Santosh Bura       2018.07.30      YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'Manthan Rajguru    2017.07.26      YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'Megha Lad          2019.10.03      YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing 
'Shilpa Nagargoje   01/03/2020      YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File)
'Shilpa Nagargoje   01/11/2020      YRS-AT-4641 -  YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
'*********************************************************************************************************************


#Region "Namespaces"
Imports System
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports YMCAObjects

#End Region
Namespace SessionManager

    Public Class SessionHandler

        Private l_YMCAMaintenance As YMCAMaintenance
        Public Property YMCAMaintenance() As YMCAMaintenance
            Get
                Return l_YMCAMaintenance
            End Get
            Set(ByVal Value As YMCAMaintenance)
                l_YMCAMaintenance = Value
            End Set
        End Property

    End Class



    Public Class YMCAMaintenance

        Public Sub New()
            ' 
            ' TODO: Add constructor logic here 
            ' 
        End Sub
#Region "Add Officer and Add Contact"

        Private l_FundNo As String
        Public Property FundNo() As String
            Get
                Return l_FundNo
            End Get
            Set(ByVal value As String)
                l_FundNo = value
            End Set
        End Property

        Private l_Title As String
        Public Property Title() As String
            Get
                Return l_Title
            End Get
            Set(ByVal value As String)
                l_Title = value
            End Set
        End Property

        Private l_TitleName As String
        Public Property TitleName() As String
            Get
                Return l_TitleName
            End Get
            Set(ByVal value As String)
                l_TitleName = value
            End Set
        End Property

        Private l_FirstName As String
        Public Property FirstName() As String
            Get
                Return l_FirstName
            End Get
            Set(ByVal value As String)
                l_FirstName = value
            End Set
        End Property

        Private l_MiddleName As String
        Public Property MiddleName() As String
            Get
                Return l_MiddleName
            End Get
            Set(ByVal value As String)
                l_MiddleName = value
            End Set
        End Property

        Private l_LastName As String
        Public Property LastName() As String
            Get
                Return l_LastName
            End Get
            Set(ByVal value As String)
                l_LastName = value
            End Set
        End Property
        'Shashi Shekhar:2010-04-14:Exclude Contact info (i.e Email,Telephone,extn properties)from YMCAMaintenance as per Gemini-1051
        'Private l_Email As String
        'Public Property Email() As String
        '    Get
        '        Return l_Email
        '    End Get
        '    Set(ByVal value As String)
        '        l_Email = value
        '    End Set
        'End Property

        'Private l_Phone As String
        'Public Property Phone() As String
        '    Get
        '        Return l_Phone
        '    End Get
        '    Set(ByVal value As String)
        '        l_Phone = value
        '    End Set
        'End Property

        'Private l_Extn As String
        'Public Property Extn() As String
        '    Get
        '        Return l_Extn
        '    End Get
        '    Set(ByVal value As String)
        '        l_Extn = value
        '    End Set
        'End Property

        Private l_OfficerSearchCancel As Boolean
        Public Property OfficerSearchCancel() As Boolean
            Get
                Return l_OfficerSearchCancel
            End Get
            Set(ByVal value As Boolean)
                l_OfficerSearchCancel = value
            End Set
        End Property


        Private l_ContactSearchCancel As Boolean
        Public Property ContactSearchCancel() As Boolean
            Get
                Return l_ContactSearchCancel
            End Get
            Set(ByVal value As Boolean)
                l_ContactSearchCancel = value
            End Set
        End Property


        Private l_ContactNote As String
        Public Property ContactNote() As String
            Get
                Return l_ContactNote
            End Get
            Set(ByVal value As String)
                l_ContactNote = value
            End Set
        End Property


        Private l_ContactType As String
        Public Property ContactType() As String
            Get
                Return l_ContactType
            End Get
            Set(ByVal value As String)
                l_ContactType = value
            End Set
        End Property
#End Region

    End Class

    'Start-SR:2014.05.28- YRS 5.0-2188 - Beneficiary RMDs
    Public Class SessionBeneficiaryRMDs
        Public Shared Property BeneficiaryRMDs_RP As DataSet
            Get
                Return HttpContext.Current.Session("BeneficiaryRMDs_RP")
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("BeneficiaryRMDs_RP") = value
            End Set
        End Property

        Public Shared Property BeneficiaryRMDs_SP As DataSet
            Get
                Return HttpContext.Current.Session("BeneficiaryRMDs_SP")
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("BeneficiaryRMDs_SP") = value
            End Set
        End Property
        Public Shared Property DeceasedFundStatus As String
            Get
                Return HttpContext.Current.Session("BeneficiaryRMDs_DeceasedFundStatus")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("BeneficiaryRMDs_DeceasedFundStatus") = value
            End Set
        End Property

    End Class
    'End-SR:2014.05.28- YRS 5.0-2188 - Beneficiary RMDs

    'Start:AA 2014.06.24- YRS5.0-1618 - YRS 5.0-1618 :Enhancements to Roll In process
    Public Class SessionRollIns
        Public Shared Property Rollin_GetAddress_ReloadDialog As String
            Get
                Return HttpContext.Current.Session("Rollin_GetAddress_ReloadDialog")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("Rollin_GetAddress_ReloadDialog") = value
            End Set
        End Property

        Public Shared Property Rollin_GetAddress_dtAddress As DataTable
            Get
                Return HttpContext.Current.Session("Rollin_GetAddress_dtAddress")
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("Rollin_GetAddress_dtAddress") = value
            End Set
        End Property
        Public Shared Property Rollin_AddressFill As Boolean
            Get
                Return HttpContext.Current.Session("Rollin_AddressFill")
            End Get
            Set(ByVal value As Boolean)
                HttpContext.Current.Session("Rollin_AddressFill") = value
            End Set
        End Property
        Public Shared Property RollIn_SaveReciepts As Boolean
            Get
                Return HttpContext.Current.Session("RollIn_SaveReciepts")
            End Get
            Set(ByVal value As Boolean)
                HttpContext.Current.Session("RollIn_SaveReciepts") = value
            End Set
        End Property
        Public Shared Property RollIn_SaveRollover As Boolean
            Get
                Return HttpContext.Current.Session("RollIn_SaveRollover")
            End Get
            Set(ByVal value As Boolean)
                HttpContext.Current.Session("RollIn_SaveRollover") = value
            End Set
        End Property
        Public Shared Property RollIn_CancelRollover As Boolean
            Get
                Return HttpContext.Current.Session("RollIn_CancelRollover")
            End Get
            Set(ByVal value As Boolean)
                HttpContext.Current.Session("RollIn_CancelRollover") = value
            End Set
        End Property
        Public Shared Property RollIn_CancelRollover_RolloverId As String
            Get
                Return HttpContext.Current.Session("RollIn_CancelRollover_RolloverId")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("RollIn_CancelRollover_RolloverId") = value
            End Set
        End Property
        Public Shared Property Rollover_ReceivedDate As String
            Get
                Return HttpContext.Current.Session("Rollover_ReceivedDate")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("Rollover_ReceivedDate") = value
            End Set
        End Property
        Public Shared Property RollInReminderForm_dsOpenRollIns As DataSet
            Get
                Return HttpContext.Current.Session("RollInReminderForm_dsOpenRollIns")
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("RollInReminderForm_dsOpenRollIns") = value
            End Set
        End Property
        Public Shared Property RollInReminderForm_objSortState As GridViewCustomSort
            Get
                Return HttpContext.Current.Session("RollInReminderForm_objSortState")
            End Get
            Set(ByVal value As GridViewCustomSort)
                HttpContext.Current.Session("RollInReminderForm_objSortState") = value
            End Set
        End Property
        Public Shared Property dsRecords_RollInReminderForm As List(Of Dictionary(Of String, String))
            Get
                Return HttpContext.Current.Session("dsRecords_RollInReminderForm")
            End Get
            Set(ByVal value As List(Of Dictionary(Of String, String)))
                HttpContext.Current.Session("dsRecords_RollInReminderForm") = value
            End Set
        End Property
        Public Shared Property RollinPrintLetters As DataTable
            Get
                Return HttpContext.Current.Session("RollinPrintLetters")
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("RollinPrintLetters") = value
            End Set
        End Property

        Public Shared Property RollIn_gvAddrs_SelectedChanged As Boolean
            Get
                Return HttpContext.Current.Session("RollIn_gvAddrs_SelectedChanged")
            End Get
            Set(ByVal value As Boolean)
                HttpContext.Current.Session("RollIn_gvAddrs_SelectedChanged") = value
            End Set
        End Property

    End Class
    'END:AA 2014.06.24- YRS5.0-1618 - YRS 5.0-1618 :Enhancements to Roll In process
    'Start-SR:2014.05.28- BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
    Public Class SessionParticipantRMDs
        Public Shared Property RMDStatistics As DataSet
            Get
                Return HttpContext.Current.Session("RMDStatistics")
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("RMDStatistics") = value
            End Set
        End Property

        Public Shared Property GetRMDs As DataSet
            Get
                Return HttpContext.Current.Session("GetRMDs")
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("GetRMDs") = value
            End Set
        End Property

        'START: SB | 2017.04.28 | YRS-AT-3400 and 3401 | Added property for NonRespondent and RMD satisfied not elected annual letter which will contain participant details for "Print List"
        Public Shared Property ReminderLetters As DataTable
            Get
                If Not (HttpContext.Current.Session("PrintReminderLetters")) Is Nothing Then
                    Return DirectCast(HttpContext.Current.Session("PrintReminderLetters"), DataTable)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("PrintReminderLetters") = value
            End Set
        End Property
        'END: SB | 2017.04.28 | YRS-AT-3400 and 3401 | Added property for NonRespondent and RMD satisfied not elected annual letter which will contain participant details for "Print List"

        'START: MMR | 2017.05.10 | YRS-AT-3205 | Added property to hold details for special QDRO letters for Print list
        Public Shared Property SpecialQDROLetters As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("PrintSpecialQDROLetters"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("PrintSpecialQDROLetters") = value
            End Set
        End Property
        'END: MMR | 2017.05.10 | YRS-AT-3205 | Added property to hold details for special QDRO letters for Print list
    End Class
    'End-SR:2014.05.28- BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.

    'Start: AA:2014.08.12-BT:2460:YRS 5.0-2331 - Death Benefit Application Form 
    Public Class SessionDeathCalc
        Public Shared Property DeathCalc_AnnuityJointSurviour As String
            Get
                Return HttpContext.Current.Session("DeathCalc_AnnuityJointSurviour")
            End Get
            Set(value As String)
                HttpContext.Current.Session("DeathCalc_AnnuityJointSurviour") = value
            End Set
        End Property
    End Class
    'End: AA:2014.08.12-BT:2460:YRS 5.0-2331 - Death Benefit Application Form 

    'Start SP 2014.08.18  BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
    Public Class SessionMetaMesasages
        Public Shared Property MetaMesasagesList As List(Of MetaMessage)
            Get
                Return HttpContext.Current.Session("MetaMesasagesList")
            End Get
            Set(ByVal value As List(Of MetaMessage))
                HttpContext.Current.Session("MetaMesasagesList") = value
            End Set
        End Property
        Public Shared Property IsUpdatedSuccuessFull As Boolean
            Get
                Return HttpContext.Current.Session("IsUpdatedSuccuessFull")
            End Get
            Set(ByVal value As Boolean)
                HttpContext.Current.Session("IsUpdatedSuccuessFull") = value
            End Set
        End Property
        'End SP 2014.08.18  BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
    End Class
    'Start:BJ 2015.04.15- YRS 5.0-2380 :Added session to Annuity Beneficiary Death & Annuity Beneficiary Death Follow-up
    Public Class SessionAnnuityBeneficiaryDeath
        Public Shared Property AnnBeneDeathAnnuityId As String
            Get
                Return HttpContext.Current.Session("AnnBeneDeathAnnuityId")
            End Get
            Set(value As String)
                HttpContext.Current.Session("AnnBeneDeathAnnuityId") = value
            End Set
        End Property
        Public Shared Property AnnBeneDeathAnnuityType As String
            Get
                Return HttpContext.Current.Session("AnnBeneDeathAnnuityType")
            End Get
            Set(value As String)
                HttpContext.Current.Session("AnnBeneDeathAnnuityType") = value
            End Set
        End Property

        Public Shared Property AnnBeneDeathPrintLettersId As String
            Get
                Return HttpContext.Current.Session("AnnBeneDeathPrintLettersId")
            End Get
            Set(value As String)
                HttpContext.Current.Session("AnnBeneDeathPrintLettersId") = value
            End Set
        End Property

        Public Shared Property AnnBeneDeathInitialLetterParams As String()
            Get
                Return HttpContext.Current.Session("AnnBeneDeathInitialLetterParams")
            End Get
            Set(value As String())
                HttpContext.Current.Session("AnnBeneDeathInitialLetterParams") = value
            End Set
        End Property



    End Class

    Public Class SessionAnnuityBeneficiaryDeathFollowup
        Public Shared Property AnnBeneDeathFollowupPendingList As DataSet
            Get
                Return HttpContext.Current.Session("AnnBeneDeathFollowupPendingList")
            End Get
            Set(value As DataSet)
                HttpContext.Current.Session("AnnBeneDeathFollowupPendingList") = value
            End Set
        End Property

        Public Shared Property AnnBeneDeathFollowupSelectedRecords As DataTable
            Get
                Return HttpContext.Current.Session("AnnBeneDeathFollowupSelectedRecords")
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("AnnBeneDeathFollowupSelectedRecords") = value
            End Set
        End Property
        Public Shared Property AnnBeneDeathFollowupGenerateReportFor As String
            Get
                Return HttpContext.Current.Session("AnnBeneDeathFollowupGenerateReportFor")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("AnnBeneDeathFollowupGenerateReportFor") = value
            End Set
        End Property

        Public Shared Property AnnBeneDeathFollowup_FirstFollowup As String
            Get
                Return HttpContext.Current.Session("AnnBeneDeathFollowup_FirstFollowup")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("AnnBeneDeathFollowup_FirstFollowup") = value
            End Set
        End Property

        Public Shared Property AnnBeneDeathFollowup_SecondFollowup As String
            Get
                Return HttpContext.Current.Session("AnnBeneDeathFollowup_SecondFollowup")
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session("AnnBeneDeathFollowup_SecondFollowup") = value
            End Set
        End Property
    End Class
    'End:BJ 2015.04.15- YRS 5.0-2380 :Enhancements to Annuity Beneficiary Death and Annuity Beneficiary Death Followup
    Public Class SessionLoanUtility
        Public Shared Property dtOffset_Default_Ageing As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dtOffset_Default_Ageing"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtOffset_Default_Ageing") = value
            End Set
        End Property
        Public Shared Property dtDefaulted As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dtDefaulted"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtDefaulted") = value
            End Set
        End Property
        Public Shared Property dtUnfreeze As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dtUnfreeze"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtUnfreeze") = value
            End Set
        End Property
        'Start: AA:12.10.2015 Added case for new loan ofsset loans tab grid
        Public Shared Property dtAutoLoansOffset As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dtAutoLoansOffset"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtAutoLoansOffset") = value
            End Set
        End Property
        'End: AA:12.10.2015 Added case for new loan ofsset loans tab grid
        'Start: AA:04.18.2015 YRS-AT-2831:Added case for new loan Auto defaulted loans tab grid
        Public Shared Property dtAutoLoansDefault As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dtAutoLoansDefault"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtAutoLoansDefault") = value
            End Set
        End Property
        'Start: AA:04.18.2015 YRS-AT-2831:Added case for new loan Auto defaulted loans tab grid
        'Start: AA:06.29.2016 YRS-AT-2830:Added case for new loan Auto closed loans tab grid
        Public Shared Property dtAutoLoansClosed As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dtAutoLoansClosed"), DataTable)
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtAutoLoansClosed") = value
            End Set
        End Property
        'Start: AA:06.29.2016 YRS-AT-2830:Added case for new loan Auto closed loans tab grid
        'START : SB | 07/30/2018 | YRS-AT-4017 : Added for loan exceptions grid 
        Public Shared Property dtLoanExceptions As DataTable
            Get
                ' If HelperFunctions.isNonEmpty(HttpContext.Current.Session("dtLoanExceptions")) Then
                Return DirectCast(HttpContext.Current.Session("dtLoanExceptions"), DataTable)
                'End If
            End Get
            Set(ByVal value As DataTable)
                HttpContext.Current.Session("dtLoanExceptions") = value
            End Set
        End Property
        'END : SB | 07/30/2018 | YRS-AT-4017 : Added for loan exceptions grid 
        'START: SN | 01/02/2020 | YRS-AT-4599 | Added for DAR Import File Report
        Public Shared Property dsSelectedDARFileRecords As DataSet
            Get
                Return DirectCast(HttpContext.Current.Session("dsSelectedDARFileRecords"), DataSet)
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("dsSelectedDARFileRecords") = value
            End Set
        End Property
        'END: SN | 01/02/2020 | YRS-AT-4599 | Added for DAr Import File Report

        'START: SN | 01/13/2020 | YRS-AT-4641 | Added for Reverse Feed Import Report
        Public Shared Property GetReverseFeedRecords As DataSet
            Get
                Return DirectCast(HttpContext.Current.Session("dsSelectedReverseFeedFileRecords"), DataSet)
            End Get
            Set(ByVal value As DataSet)
                HttpContext.Current.Session("dsSelectedReverseFeedFileRecords") = value
            End Set
        End Property
        'END: SN | 01/13/2020 | YRS-AT-4641 | Added for Reverse Feed Import Report
    End Class

    'START: PPP | 09/14/2017 | YRS-AT-3665 | This class will be used to maintain YMCA Find Info screen, so added here instead of YRS Object Library
    Public Class SessionFindYmcaInfo
        Private ymcaNoValue As String
        Public Property YmcaNo() As String
            Get
                Return ymcaNoValue
            End Get
            Set(ByVal value As String)
                ymcaNoValue = value
            End Set
        End Property

        Private nameValue As String
        Public Property Name() As String
            Get
                Return nameValue
            End Get
            Set(ByVal value As String)
                nameValue = value
            End Set
        End Property

        Private cityValue As String
        Public Property City() As String
            Get
                Return cityValue
            End Get
            Set(ByVal value As String)
                cityValue = value
            End Set
        End Property

        Private stateValue As String
        Public Property State() As String
            Get
                Return stateValue
            End Get
            Set(ByVal value As String)
                stateValue = value
            End Set
        End Property

        Private gridIndexValue As Integer
        Public Property GridIndex() As Integer
            Get
                Return gridIndexValue
            End Get
            Set(ByVal value As Integer)
                gridIndexValue = value
            End Set
        End Property

        Private searchResultValue As DataSet
        Public Property SearchResult() As DataSet
            Get
                Return searchResultValue
            End Get
            Set(ByVal value As DataSet)
                searchResultValue = value
            End Set
        End Property

        Private pageCountValue As String
        Public Property PageCount() As String
            Get
                Return pageCountValue
            End Get
            Set(ByVal value As String)
                pageCountValue = value
            End Set
        End Property

        Private pageIndexValue As Integer
        Public Property PageIndex() As Integer
            Get
                Return pageIndexValue
            End Get
            Set(ByVal value As Integer)
                pageIndexValue = value
            End Set
        End Property

        Private gvCustomSortValue As GridViewCustomSort
        Public Property GVCustomSort() As GridViewCustomSort
            Get
                Return gvCustomSortValue
            End Get
            Set(ByVal value As GridViewCustomSort)
                gvCustomSortValue = value
            End Set
        End Property
    End Class
    'END: PPP | 09/14/2017 | YRS-AT-3665 | This class will be used to maintain YMCA Find Info screen, so added here instead of YRS Object Library

    'START: VC | 2018.07.27 | YRS-AT-4017 | Class will have LAC session properties used for reports
    Public Class SessionLoanAdmin
        Public Shared Property ListOfWebLoan As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("dsWebLoans"), DataTable)
            End Get
            Set(value As DataTable)
                HttpContext.Current.Session("dsWebLoans") = value
            End Set
        End Property

        'START: MMR | 07/26/2018 | YRS-AT-4017 | Common class for Loan admin to hold datatable for print list
        Public Shared Property LoanProcessingList As DataTable
            Get
                Return DirectCast(HttpContext.Current.Session("LoanProcessingForPrintList"), DataTable)
            End Get
            Set(value As DataTable)
                HttpContext.Current.Session("LoanProcessingForPrintList") = value
            End Set
        End Property
        'END: MMR | 07/26/2018 | YRS-AT-4017 | Common class for Loan admin to hold datatable for print list
    End Class
    'END: VC | 2018.07.27 | YRS-AT-4017 | Class will have LAC session properties used for reports

    'START :ML | 2019.10.03 | YRS-AT-4598 | Class will have StateWithholding details
    Public Class SessionStateWithholding
        Public Shared Property LstSWHPerssDetail() As List(Of YMCAObjects.StateWithholdingDetails)
            Get
                Return CType(HttpContext.Current.Session("LstSWHPerssDetail"), List(Of YMCAObjects.StateWithholdingDetails))
            End Get
            Set(ByVal Value As List(Of YMCAObjects.StateWithholdingDetails))
                HttpContext.Current.Session("LstSWHPerssDetail") = Value
            End Set
        End Property
    End Class
    'END :ML | 2019.10.03 | YRS-AT-4598 | Class will have StateWithholding details

End Namespace
