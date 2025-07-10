'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	RollInReminderForm.aspx.vb
' Author Name		:	
' Creation Date		:	
' Description		:	This form is show the process of the batch processing(s)
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Anudeep            06.06.2014      BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
'Anudeep            16.07.2014      BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'Anudeep            17.07.2014      BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'Anudeep            10.17.2014      BT:2691:Get the Merge PDF path from the configuration key
'Dinesh Kanojia     09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
'B. Jagadeesh       04.23.2015      BT:2570:YRS 5.0-2380- Add parameters to generating the pdf for Annuity Beneficiary Death Follow-up
'Anudeep            05/08/2015      BT:2825:YRS 5.0-2500:Letter of Acceptance In YRS 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2015.10.10      YRS-AT-2614: YRS: files for IDM - .idx filename needs to match .pdf filename
'Anudeep A          2016.03.07      YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
'Anudeep A          2016.07.05      YRS-AT-2985 - YRS bug: (hotfix) Withdrawal EForm DOCCODE change for signed
'Anudeep A          2016.27.06      BT-1156:Add a precautionary measure while IDM creation
'Chandra sekar      2016.24.10      YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
'Manthan Rajguru    2016.11.10      YRS-AT-3197 -  YRS enh: MRD letter needs to allow for multiple FundIDs passed as input parameter
'Sanjay GS Rawat    2016.11.22      YRS-AT-3118 - YRS bug - Cash Out Batch issues - sending files into IDM after freeze/retry 
'Santosh Bura       2016.12.06      YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224) 
'Manthan Rajguru	2017.05.05      YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977) 
'Santosh Bura       2017.05.04      YRS-AT-3400 & YRS-AT-3401
'Manthan Rajguru	2017.05.29      YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977) 
'*******************************************************************************
Imports System.Data
Imports System.IO
Imports YMCAUI
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports YMCARET.YmcaBusinessObject
Imports YMCAObjects
Imports PdfSharp
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf.IO
Imports YMCARET.CommonUtilities

Public Class BatchRequestCreation
    Inherits System.Web.UI.Page


#Region "Variable"

    Dim l_datatable_SelectedCashOuts As DataTable
    Dim l_datatable_AccountBreakDown As DataTable
    Dim l_datatable_AtsRefunds As DataTable
    Dim l_datatable_AtsRefRequests As DataTable
    Dim l_datatable_atsRefRequestDetails As DataTable
    Dim l_datatable_FileSystem As DataTable

    Dim m_decimal_totalamount As Decimal
    Dim m_decimal_withheldtax As Decimal
    Dim m_decimal_terminationPIA As Decimal
    Dim l_integer_PersonAge As Integer
    Dim m_decimal_MinimumDistributedAge As Decimal
    Dim m_decimal_MaximumPIAAmount As Decimal
    Dim m_decimal_MinimumDistributedTaxRate As Decimal
    Dim m_decimal_FederalTaxRate As Decimal
    Dim m_integer_RefundExpiryDate As Integer
    Dim l_decimal_TaxRate As Decimal
    Dim l_string_UniqueId As String = ""
    Dim m_string_GetPersonName As String = ""
    Dim m_string_PersonId As String
    Dim m_string_FundEventId As String
    Dim m_string_BatchId As String
    Dim m_bool_IsVested As Boolean
    Dim m_bool_IsTerminated As Boolean
    Dim m_string_StatusType As String = ""


    Dim m_bool_PrviewRpt As Boolean = False
    Dim m_bool_CreatePdf As Boolean = False
    Dim m_bool_Createidx As Boolean = False
    Dim m_bool_CopyFilesToIDM As Boolean = False
    Dim m_bool_logondb As Boolean = False
    Dim m_str_ReportName As String = Nothing
    Dim m_str_ReportHeader As String = Nothing
    Dim m_str_PDFFileName As String = Nothing
    Dim m_str_IDXFileName As String = Nothing
    Dim m_str_YMCAId As String = Nothing
    Dim m_str_PersId As String
    Dim m_str_DocType As String = Nothing
    Dim m_arr_ReportParams As ArrayList = Nothing
    Dim m_dtFileList As New DataTable
    Dim m_dtReportTracking As New DataTable
    Dim m_str_VignettePath As String
    Dim m_str_letter_YmcaPart As String
    Dim m_str_app_type As String = Nothing
    Dim m_str_PayRollDates As String
    Dim m_str_OutputFileType As String
    Dim m_str_YMCANo As String
    ''''Added by sanjay
    Dim m_bool_PdfCreated As Boolean
    Dim m_bool_idxCreated As String
    Dim m_str_EntityId As String
    Dim p_ds_rpttracking As New DataSet
    Dim l_str_Exception As String
    Dim l_str_ExceptionMode As String = "IE"
    Dim l_str_rptTrackingId As String

    'Added By SG: 2012.09.05: BT-960
    Dim l_RefRequestsID As String
    '''''''''Ends Here
    Dim l_bln_multipleParms_Exists As Boolean = False 'AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report

    'Dim m_dtFileList As DataTable
    Dim objCashOutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass
    Dim stTimeStampForFile As String 'Manthan Rajguru | 2015.10.10 | YRS-AT-2614 | Local Variable to hold time stamp value which will be accessed by pdf and idx file name properties

#End Region

#Region "Properties"

    Public Property PreviewReport() As Boolean
        Get
            Return m_bool_PrviewRpt
        End Get
        Set(ByVal Value As Boolean)
            m_bool_PrviewRpt = Value
        End Set
    End Property
    Public Property CreatePDF() As Boolean
        Get
            Return m_bool_CreatePdf
        End Get
        Set(ByVal Value As Boolean)
            m_bool_CreatePdf = Value
        End Set
    End Property
    Public Property CreateIDX() As Boolean
        Get
            Return m_bool_Createidx
        End Get
        Set(ByVal Value As Boolean)
            m_bool_Createidx = Value
        End Set
    End Property
    Public Property CopyFilesToIDM() As Boolean
        Get
            Return m_bool_CopyFilesToIDM
        End Get
        Set(ByVal Value As Boolean)
            m_bool_CopyFilesToIDM = Value
        End Set
    End Property
    Public Property PDFFileName() As String
        Get
            Dim l_pdf_Filename As String
            Dim l_pdf_FileIndex As String
            Dim l_str_OutPutFileType As String
            Dim l_str_YMCANo As String
            Dim l_str_DocType As String

            l_str_OutPutFileType = Me.OutputFileType

            If l_str_OutPutFileType.ToString = "DLTTR" Then
                l_str_YMCANo = Me.GetYMCANO
                l_str_DocType = Me.DocTypeCode
                'm_str_PDFFileName = l_str_YMCANo & "_" & l_str_DocType & "_" & Format(Now, "ddMMMyyyy")  '' commented by sanjay to resolve deliquency letter problem
                '-- Start: Manthan Rajguru | 2015.10.10 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                ' m_str_PDFFileName = l_str_YMCANo & "_" & l_str_DocType & "_" & Format(Now, "ddMMMyyyy_HHmmss_fff")                
                m_str_PDFFileName = String.Format("{0}_{1}_{2}", l_str_YMCANo, l_str_DocType, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.10 | YRS-AT 2614 | Timestamp is called through variable assigned in method

            Else
                If m_str_letter_YmcaPart = "A" Then
                    l_pdf_Filename = Me.OutputFileType
                    l_pdf_FileIndex = "_2"
                Else
                    l_pdf_Filename = Me.OutputFileType
                    l_pdf_FileIndex = "_1"
                End If
                '-- Start: Manthan Rajguru | 2015.10.10 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_PDFFileName = "" & l_pdf_Filename & l_pdf_FileIndex & "_" & Format(Now, "ddMMMyyyy_HHmmss_fff")
                m_str_PDFFileName = String.Format("{0}{1}_{2}", l_pdf_Filename, l_pdf_FileIndex, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.10 | YRS-AT 2614 | Timestamp is called through variable assigned in method
            End If

            Return m_str_PDFFileName

        End Get
        Set(ByVal Value As String)
            m_str_PDFFileName = Value
        End Set
    End Property
    Public Property IDXFileName() As String
        Get
            Dim l_idx_Filename As String
            Dim l_idx_FileIndex As String
            Dim l_str_OutPutFileType As String
            Dim l_str_YMCANo As String
            Dim l_str_DocType As String

            l_str_OutPutFileType = Me.OutputFileType
            If l_str_OutPutFileType = "DLTTR" Then
                l_str_YMCANo = Me.GetYMCANO
                l_str_DocType = Me.DocTypeCode
                'm_str_IDXFileName = l_str_YMCANo & "_" & l_str_DocType & "_" & Format(Now, "ddMMMyyyy") '' commented by sanjay to resolve deliquency letter problem
                '-- Start: Manthan Rajguru | 2015.10.10 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_IDXFileName = l_str_YMCANo & "_" & l_str_DocType & "_" & Format(Now, "ddMMMyyyy_HHmmss_fff")
                m_str_IDXFileName = String.Format("{0}_{1}_{2}", l_str_YMCANo, l_str_DocType, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.10 | YRS-AT 2614 | Timestamp is called through variable assigned in method
            Else
                If m_str_letter_YmcaPart = "A" Then
                    l_idx_Filename = Me.OutputFileType
                    l_idx_FileIndex = "_2"
                Else
                    l_idx_Filename = Me.OutputFileType
                    l_idx_FileIndex = "_1"
                End If
                'Commented by SHubhrata Jan 4th,2007 YRSPS 4462 remove extra underscore
                'm_str_IDXFileName = "" & l_idx_Filename & "_" & l_idx_FileIndex & "_" & Format(Now, "ddMMMyyyy_HHmmss")
                'Added by Shubhrata Jan 4th,2007 YRSPS 4462 remove extra underscore
                '-- Start: Manthan Rajguru | 2015.10.10 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_IDXFileName = "" & l_idx_Filename & l_idx_FileIndex & "_" & Format(Now, "ddMMMyyyy_HHmmss_fff")
                m_str_IDXFileName = String.Format("{0}{1}_{2}", l_idx_Filename, l_idx_FileIndex, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.10 | YRS-AT 2614 | Timestamp is called through variable assigned in method
            End If

            Return m_str_IDXFileName

        End Get
        Set(ByVal Value As String)
            m_str_IDXFileName = Value
        End Set
    End Property
    Public Property DocTypeCode() As String
        Get
            If Not m_str_DocType Is String.Empty Then
                Return m_str_DocType.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_str_DocType = Value
        End Set
    End Property
    Public Property ReportParameters() As ArrayList
        Get
            If Not m_arr_ReportParams Is Nothing Then
                Return (CType(m_arr_ReportParams, ArrayList))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As ArrayList)
            m_arr_ReportParams = Value
        End Set
    End Property
    Public Property YMCAID() As String
        Get
            If Not m_str_YMCAId Is String.Empty Then
                Return m_str_YMCAId.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_str_YMCAId = Value
        End Set
    End Property
    Public Property PersId() As String
        Get
            If Not m_str_PersId Is String.Empty Then
                Return m_str_PersId.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_str_PersId = Value
        End Set
    End Property

    Public Property AppType() As String
        Get
            If Not m_str_app_type Is String.Empty Then
                Return m_str_app_type.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_str_app_type = Value
        End Set
    End Property
    Public Property ReportName() As String
        Get
            If Not m_str_ReportName Is String.Empty Then
                Return m_str_ReportName.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_str_ReportName = Value
        End Set
    End Property
    Public Property OutputFileType() As String
        Get
            If Not m_str_OutputFileType Is String.Empty Then
                Return m_str_OutputFileType.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_str_OutputFileType = Value
        End Set
    End Property
    Public Property GetYMCANO() As String
        Get
            Return m_str_YMCANo
        End Get
        Set(ByVal Value As String)
            m_str_YMCANo = Value
        End Set
    End Property
    Public Property SetdtFileList() As DataTable
        Get
            If Not m_dtFileList Is Nothing Then
                Return (CType(m_dtFileList, DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            m_dtFileList = Value
        End Set
    End Property

    'Added By SG: 2012.09.05: BT-960
    Public Property RefRequestsID() As String
        Get
            Return l_RefRequestsID
        End Get
        Set(ByVal value As String)
            l_RefRequestsID = value
        End Set
    End Property

    Public Property DataTableSelectedCashOuts() As DataTable
        Get
            Return l_datatable_SelectedCashOuts
        End Get
        Set(ByVal value As DataTable)
            l_datatable_SelectedCashOuts = value
        End Set
    End Property
    Public Property DataTableAtsRefunds() As DataTable
        Get
            Return l_datatable_AtsRefunds
        End Get
        Set(ByVal value As DataTable)
            l_datatable_AtsRefunds = value
        End Set
    End Property
    Public Property DataTableAtsRefRequests() As DataTable
        Get
            Return l_datatable_AtsRefRequests
        End Get
        Set(ByVal value As DataTable)
            l_datatable_AtsRefRequests = value
        End Set
    End Property
    Public Property DataTableatsRefRequestDetails() As DataTable
        Get
            Return l_datatable_atsRefRequestDetails
        End Get
        Set(ByVal value As DataTable)
            l_datatable_atsRefRequestDetails = value
        End Set
    End Property
    Public Property DataTableAccountBreakDown() As DataTable
        Get
            Return l_datatable_AccountBreakDown
        End Get
        Set(ByVal value As DataTable)
            l_datatable_AccountBreakDown = value
        End Set
    End Property

    Public Property IntegerPersonAge() As Integer
        Get
            Return l_integer_PersonAge
        End Get
        Set(ByVal value As Integer)
            l_integer_PersonAge = value
        End Set
    End Property
    Public Property MinimumDistributedAge() As Decimal
        Get
            Return m_decimal_MinimumDistributedAge
        End Get
        Set(ByVal value As Decimal)
            m_decimal_MinimumDistributedAge = value
        End Set
    End Property
    Public Property MinimumDistributedTaxRate() As Decimal
        Get
            Return m_decimal_MinimumDistributedTaxRate
        End Get
        Set(ByVal value As Decimal)
            m_decimal_MinimumDistributedTaxRate = value
        End Set
    End Property
    Public Property MaximumPIAAmount() As Decimal
        Get
            Return m_decimal_MaximumPIAAmount
        End Get
        Set(ByVal value As Decimal)
            m_decimal_MaximumPIAAmount = value
        End Set
    End Property
    Public Property FederalTaxRate() As Decimal
        Get
            Return m_decimal_FederalTaxRate
        End Get
        Set(ByVal value As Decimal)
            m_decimal_FederalTaxRate = value
        End Set
    End Property
    Public Property RefundExpiryDate() As Integer
        Get
            Return m_integer_RefundExpiryDate
        End Get
        Set(ByVal value As Integer)
            m_integer_RefundExpiryDate = value
        End Set
    End Property

    'Added By SG: 2012.09.06: BT-960
    Public Property TaxRate() As Decimal
        Get
            Return l_decimal_TaxRate
        End Get
        Set(ByVal value As Decimal)
            l_decimal_TaxRate = value
        End Set
    End Property

    Public Property LogonToDb() As Boolean
        Get
            Return m_bool_logondb
        End Get
        Set(ByVal Value As Boolean)
            m_bool_logondb = Value
        End Set
    End Property
    'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report
    Public Property IsMultipleParamExists As Boolean
        Get
            Return l_bln_multipleParms_Exists
        End Get
        Set(value As Boolean)
            l_bln_multipleParms_Exists = value
        End Set
    End Property
    'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report

    'START | SR | 2016.11.28 | YRS-AT-3118 - define properties to hold configured PDF attempts count & Process sleep time.
    Public Property AttemptsToCreatePDF As Integer
        Get
            If Not (Session("AttemptsToCreatePDF")) Is Nothing Then
                Return (CType(Session("AttemptsToCreatePDF"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            Session("AttemptsToCreatePDF") = value
        End Set
    End Property

    Public Property ProcessSleepTime As Integer
        Get
            If Not (Session("ProcessSleepTime")) Is Nothing Then
                Return (CType(Session("ProcessSleepTime"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            Session("ProcessSleepTime") = value
        End Set
    End Property
    'END | SR | 2016.11.28 | YRS-AT-3118 - define properties to hold configured PDF attempts count & Process sleep time.

#End Region

#Region "Enum"
    'Added By SG: 2012.06.05: BT-960
    Enum ProcessState
        Request
        Process
    End Enum

    'Added By SG: 2012.09.24: BT-960
    Enum CashoutRange
        Range1
        Range2
    End Enum
#End Region

#Region "Function"

    'Page Load will load html in modal popup
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim popupScript3 As String = String.Empty
        Dim strBatchId As String = String.Empty
        Dim strModuleType As String = String.Empty
        Try

            If Not Request.QueryString("submit") Is Nothing Then
                'Second time postback to start the batch request creation process.
                If Convert.ToBoolean(Request.QueryString("submit")) Then
                    'Start:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                    If Not Request.QueryString("Form") Is Nothing Then
                        If Convert.ToString(Request.QueryString("Form")).ToUpper = "RMD" Then
                            tdProcessCreation.InnerText = "1. Generating RMD Letter Entries."
                            If Not Request.QueryString("strBatchId") Is Nothing And Not Request.QueryString("strModuleType") Is Nothing Then
                                strBatchId = Request.QueryString("strBatchId").ToString
                                strModuleType = Request.QueryString("strModuleType").ToString
                                PrintLettersProcess(strBatchId, strModuleType)
                            Else
                                Throw New Exception("BatchId is not found to generate RDMLetters.")
                            End If
                            'END:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                            'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for copy rollin reminder letter in IDM
                        ElseIf Convert.ToString(Request.QueryString("Form")).ToUpper = "ROLLIN" Then
                            tdProcessCreation.InnerText = "1. Generating RollIn Followup Letter Entries"
                            'Start:AA:16.07.2014 BT:1051 - YRS 5.0:1618 
                            strBatchId = Request.QueryString("strBatchId").ToString
                            strModuleType = Request.QueryString("strModuleType").ToString
                            PrintRollInProcess(strBatchId, strModuleType)
                            'End:AA:16.07.2014 BT:1051 - YRS 5.0:1618 
                            'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for copy rollin reminder letter in IDM
                        Else
                            tdProcessCreation.InnerText = "1. Withdrawal Request Creation"
                            BatchRequestCreation()
                        End If
                    Else
                        tdProcessCreation.InnerText = "1. Withdrawal Request Creation"
                        BatchRequestCreation()
                    End If
                End If
            Else
                'Start:This code is aded by dines for YRS 5.0-2315
                If Not Request.QueryString("Form") Is Nothing Then
                    'Start:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                    If Convert.ToString(Request.QueryString("Form")).ToUpper = "RMD" Then
                        tdProcessCreation.InnerText = "1. Generating RMD Letter Entries."
                        popupScript3 = "<" & "script language='javascript'>" & _
                                       "FormSubmit('" + Convert.ToString(Request.QueryString("Form")).ToUpper + "','" + Convert.ToString(Request.QueryString("strBatchId")) + "','" + Convert.ToString(Request.QueryString("strModuleType")) + "')</" & "script" & ">"
                        'End:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                        'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for copy rollin reminder letter in IDM
                    ElseIf Convert.ToString(Request.QueryString("Form")).ToUpper = "ROLLIN" Then
                        tdProcessCreation.InnerText = "1. Generating RollIn Followup Letter Entries"
                        popupScript3 = "<" & "script language='javascript'>" & _
                                       "FormSubmit('" + Convert.ToString(Request.QueryString("Form")).ToUpper + "','" + Convert.ToString(Request.QueryString("strBatchId")) + "','" + Convert.ToString(Request.QueryString("strModuleType")) + "')</" & "script" & ">"
                        'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for copy rollin reminder letter in IDM
                    Else
                        tdProcessCreation.InnerText = "1. Withdrawal Request Creation"
                        popupScript3 = "<" & "script language='javascript'>" & _
                                               "FormSubmit()</" & "script" & ">"
                    End If
                Else
                    tdProcessCreation.InnerText = "1. Withdrawal Request Creation"
                    popupScript3 = "<" & "script language='javascript'>" & _
                                           "FormSubmit()</" & "script" & ">"
                End If
                'End:This code is aded by dines for YRS 5.0-2315
                'First time post back to render page.
                'Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                '                       "FormSubmit()</" & "script" & ">"
                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript21", popupScript3)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("BatchProcess_Page_Load", ex)
        End Try
    End Sub
    'Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
    Private Function GetPipeSeperated(ByVal lstGuids As List(Of YMCAObjects.RMDPrintLetters)) As String
        Dim sbuilder As New StringBuilder()

        For Each item As YMCAObjects.RMDPrintLetters In lstGuids
            sbuilder.Append("|" & item.strFundNo)
        Next
        If (sbuilder.Length > 1) Then
            sbuilder.Remove(0, 1)
        End If
        Return sbuilder.ToString()
    End Function
    'Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
    Private Function ConvertListToDatable(ByVal objList As List(Of YMCAObjects.RMDPrintLetters)) As DataTable
        Try
            Dim l_datatable_PrintLetters As New DataTable
            Dim l_datarow_PrintLetters As DataRow
            l_datatable_PrintLetters.Columns.Add("PersonId")
            l_datatable_PrintLetters.Columns.Add("RefRequestID")
            l_datatable_PrintLetters.Columns.Add("FUNDNo")
            l_datatable_PrintLetters.Columns.Add("SSNo")
            l_datatable_PrintLetters.Columns.Add("FirstName")
            l_datatable_PrintLetters.Columns.Add("LastName")
            l_datatable_PrintLetters.Columns.Add("MiddleName")
            l_datatable_PrintLetters.Columns.Add("LetterCode")

            If objList.Count > 0 Then
                For Icount As Integer = 0 To objList.Count - 1
                    l_datarow_PrintLetters = l_datatable_PrintLetters.NewRow()
                    l_datarow_PrintLetters("PersonId") = objList(Icount).strPersID
                    l_datarow_PrintLetters("RefRequestID") = objList(Icount).strRefId
                    l_datarow_PrintLetters("SSNo") = objList(Icount).strSSNo
                    l_datarow_PrintLetters("FirstName") = objList(Icount).strFirstName
                    l_datarow_PrintLetters("FUNDNo") = objList(Icount).strFundNo
                    l_datarow_PrintLetters("LastName") = objList(Icount).strLastName
                    l_datarow_PrintLetters("MiddleName") = objList(Icount).strMiddleName
                    l_datarow_PrintLetters("LetterCode") = objList(Icount).strLetterCode
                    l_datatable_PrintLetters.Rows.Add(l_datarow_PrintLetters)
                Next
            End If

            Return l_datatable_PrintLetters
        Catch ex As Exception

        End Try

    End Function
    'End:This code is aded by dines for YRS 5.0-2315
    'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added to get the datatable from the list

    'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added to get the datatable from the list
    'Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
    Public Sub PrintLettersProcess(ByVal strBatchId As String, strModuleType As String)
        Dim ArrErrorDataList As List(Of ExceptionLog) = Nothing
        Dim l_string_CashOutReqType As String = String.Empty
        Dim l_datatable_PrintLetters As DataTable
        Dim dsPrintLetters As New DataSet
        Dim objLstRMDPrintLetters As List(Of YMCAObjects.RMDPrintLetters)
        Dim l_bool_status As Boolean = False
        Dim iCount As Integer = 0
        Dim ProcessCount As Integer = 0
        Dim dtTempPrintLetters As DataTable
        Dim dttempCasjou As DataRow
        Dim bAllFilesCreated As Boolean = False
        Dim strDocType As String = String.Empty
        Dim strReportName As String = String.Empty
        Dim strParam1 As String = String.Empty
        Dim strOutputFileType As String = String.Empty
        Dim dtFileList As DataTable
        Dim iIDXCreated As Integer = 0
        Dim iPDfCreated As Integer = 0
        Dim iBatchCount As Integer = 10

        Try

            tdProcessCreation.InnerText = "1. Print Letters Creation"

            dsPrintLetters = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModuleType)
            If HelperFunctions.isNonEmpty(dsPrintLetters) Then
                If strModuleType = CommonClass.BatchProcess.RMDInitLetters.ToString Then
                    l_datatable_PrintLetters = dsPrintLetters.Tables("SelectedBatchRecords").Clone
                    strDocType = "RMDINTLT"
                    strReportName = "RMD Initial Letter.rpt"
                    strParam1 = "RMDPrintLetter"
                    strOutputFileType = "RMD_RMDINTLT"
                ElseIf strModuleType = CommonClass.BatchProcess.RMDFollwLetters.ToString Then
                    l_datatable_PrintLetters = dsPrintLetters.Tables("SelectedBatchRecords").Clone
                    strDocType = "RMDFLWLT"
                    strReportName = "RMD Followup Letter.rpt"
                    strParam1 = "RMDPrintLetter"
                    strOutputFileType = "RMD_RMDFLWLT"
                End If
            Else
                Exit Try
            End If

            If Not dsPrintLetters.Tables("SelectedBatchRecords").Columns.Contains("IDXCount") Then
                dsPrintLetters.Tables("SelectedBatchRecords").Columns.Add("IDXCount")
            End If

            If Not dsPrintLetters.Tables("SelectedBatchRecords").Columns.Contains("PDFCount") Then
                dsPrintLetters.Tables("SelectedBatchRecords").Columns.Add("PDFCount")
            End If

            l_datatable_PrintLetters = dsPrintLetters.Tables("SelectedBatchRecords")

            Dim dtArrErrorDataList As DataTable = dsPrintLetters.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                ArrErrorDataList = New List(Of ExceptionLog)
                For Each dr As DataRow In dtArrErrorDataList.Rows
                    ArrErrorDataList.Add(New ExceptionLog(dr("FundNo"), dr("Errors"), dr("Description")))
                Next
            Else
                ArrErrorDataList = New List(Of ExceptionLog)
            End If

            If Not ArrErrorDataList Is Nothing Then
                gvBatchRequestError.DataSource = ArrErrorDataList
                gvBatchRequestError.DataBind()
            End If

            If (HelperFunctions.isEmpty(l_datatable_PrintLetters)) Then
                ArrErrorDataList.Add(New ExceptionLog("", "Session Expired", "No records found for print letters process."))
                Exit Sub
            End If

            If Not Request Is Nothing Then
                'Query string is used as index no in datatable for batch request creation.
                If Not String.IsNullOrEmpty(Request.QueryString("count")) Then
                    iCount = Convert.ToInt32(Request.QueryString("count"))
                Else
                    'Create entry in log file against the set of records.
                    '        objCashOutBOClass.CreateAndProcessRequest(l_datatable_Cashouts, l_bool_status, l_string_CashOutReqType)
                End If
            End If




            dtTempPrintLetters = l_datatable_PrintLetters.Clone
            For ProcessCount = iCount To l_datatable_PrintLetters.Rows.Count - 1
                'number of records to be taken for batch creation process this count can be configurable.
                If ProcessCount - iCount >= iBatchCount Then
                    Exit For
                End If

                YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(l_datatable_PrintLetters.Rows(ProcessCount)("FUNDNo").ToString, l_datatable_PrintLetters.Rows(ProcessCount)("PersonId").ToString, l_datatable_PrintLetters.Rows(ProcessCount)("LetterCode").ToString, l_datatable_PrintLetters.Rows(ProcessCount)("PlanType").ToString)  ' SB | 2016.12.06 | YRS-AT-3203 | Added new parameter for plan type description in print letter 
                'storing set of records in temp table so that this much of records can be process for batch creation.
                dttempCasjou = dtTempPrintLetters.NewRow()
                dttempCasjou("PersonId") = l_datatable_PrintLetters.Rows(ProcessCount)("PersonId").ToString
                dttempCasjou("SSNo") = l_datatable_PrintLetters.Rows(ProcessCount)("SSNo").ToString
                dttempCasjou("FUNDNo") = l_datatable_PrintLetters.Rows(ProcessCount)("FUNDNo").ToString
                dttempCasjou("FirstName") = l_datatable_PrintLetters.Rows(ProcessCount)("FirstName").ToString
                dttempCasjou("LastName") = l_datatable_PrintLetters.Rows(ProcessCount)("LastName").ToString
                dttempCasjou("MiddleName") = l_datatable_PrintLetters.Rows(ProcessCount)("MiddleName").ToString
                dttempCasjou("RefRequestID") = l_datatable_PrintLetters.Rows(ProcessCount)("FUNDNo").ToString
                dttempCasjou("IDXCount") = 0
                dttempCasjou("PDFCount") = 0
                dtTempPrintLetters.Rows.Add(dttempCasjou)
            Next

            dvReqProgress.InnerText = ProcessCount.ToString() + " of " + l_datatable_PrintLetters.Rows.Count.ToString() + " Processed."

            InvokeBatchRequestCreation(0, dtTempPrintLetters, strDocType, strReportName, strOutputFileType, strParam1, ArrErrorDataList, dtFileList, iIDXCreated, iPDfCreated, True, True, True, True, True)

            If HelperFunctions.isNonEmpty(dtFileList) Then
                If Not dsPrintLetters.Tables.Contains("dtFileList") Then
                    dtFileList.TableName = "dtFileList"
                    dsPrintLetters.Tables.Add(dtFileList)
                End If
                dsPrintLetters.Tables("dtFileList").Merge(dtFileList)
            End If

            InvokeProcess(ProcessCount, dtTempPrintLetters, bAllFilesCreated, ArrErrorDataList, iIDXCreated, iPDfCreated, strBatchId, strModuleType, dsPrintLetters)

            If Not dsPrintLetters.Tables("dtFileList") Is Nothing Then
                Session("FTFileList") = dsPrintLetters.Tables("dtFileList")
            End If

            If Not Session("FTFileList") Is Nothing And bAllFilesCreated Then
                Try

                    imgRegComplete.CssClass = "show"
                    dvReg.Style.Add("display", "none")

                    imgCopiedWaiting.CssClass = "show"
                    imgCopiedComplete.CssClass = "hide"
                    Dim sbuilder As New StringBuilder()
                    Dim iPDFCount As Integer = 0
                    For Each dr As DataRow In dtTempPrintLetters.Rows
                        sbuilder.Append("|" & dr("FUNDNo").ToString)
                    Next
                    If (sbuilder.Length > 1) Then
                        sbuilder.Remove(0, 1)
                    End If

                    Session("RMDBatchFundNo") = sbuilder.ToString()
                    Dim popupScript As String = " newwindow1 = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                                                "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');" +
                                                " if (window.focus) {newwindow1.focus()}" +
                                                " if (!newwindow1.closed) {newwindow1.focus()}"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenCopy", popupScript, True)
                    dvCopiedProgress.InnerText = "Completed."

                    imgCopiedComplete.CssClass = "show"
                    imgCopiedWaiting.CssClass = "hide"
                    dvCopy.Style.Add("display", "none")
                    imgPDFComplete.CssClass = "show"
                    imgIDXComplete.CssClass = "show"
                    iPDFCount = dsPrintLetters.Tables("SelectedBatchRecords").Select(" PDFCount = 1 ").Count
                    dvCopiedProgress.InnerText = ProcessCount.ToString() + " of " + l_datatable_PrintLetters.Rows.Count.ToString() + " Processed."
                    'dvCopiedProgress.InnerText = iPDFCount.ToString() + " of " + l_datatable_PrintLetters.Rows.Count.ToString() + " Processed."
                    Session("strReportName") = strReportName.Substring(0, strReportName.Length - 4)

                    MergePDFs(dsPrintLetters.Tables("dtFileList"))

                Catch ex As Exception
                    Session("AllFilesCreated") = False
                    ArrErrorDataList.Add(New ExceptionLog("Exception", "Error Occured while generating reports", ex.Message))
                    Throw
                Finally
                    'If HelperFunctions.isNonEmpty(dtFileList) Then
                    '    If Not dsPrintLetters.Tables.Contains("dtFileList") Then
                    '        dtFileList.TableName = "dtFileList"
                    '        dsPrintLetters.Tables.Add(dtFileList)
                    '    End If
                    '    dsPrintLetters.Tables("dtFileList").Merge(dtFileList)
                    'End If
                End Try
            End If
        Catch ex As Exception

        Finally
            If Not ArrErrorDataList Is Nothing Then
                If ArrErrorDataList.Count > 0 Then
                    lblException.Visible = True
                    gvBatchRequestError.DataSource = ArrErrorDataList
                    gvBatchRequestError.DataBind()
                End If
            End If
        End Try
    End Sub
    'End:This code is aded by dines for YRS 5.0-2315


    'This function will set all required data to invoke batch request creation.
    Public Sub BatchRequestCreation()
        Try
            Dim l_string_CashOutReqType As String = String.Empty
            Dim l_datatable_Cashouts As DataTable
            Dim l_bool_status As Boolean = False
            Dim iCount As Integer = 0
            Dim ProcessCount As Integer = 0
            Dim objCashOutBOClass As New YMCARET.YmcaBusinessObject.CashOutBOClass
            Dim dtTempCashout As DataTable
            Dim dttempCasjou As DataRow
            Dim bAllFilesCreated As Boolean = False
            Dim ArrErrorDataList As List(Of ExceptionLog) = Nothing

            tdProcessCreation.InnerText = "1. Withdrawal Request Creation" 'This code is aded by dines for YRS 5.0-2315
            If Not Session("CashOutReqType") Is Nothing Then
                l_string_CashOutReqType = Session("CashOutReqType").ToString()
            End If
            If Not Session("l_datatable_Cashouts") Is Nothing Then
                'Store getting all selected records from cashout page for batch creation.
                l_datatable_Cashouts = Session("l_datatable_Cashouts")
                'l_datatable_Cashouts.WriteXml("f:\\cashout")
                dtTempCashout = l_datatable_Cashouts.Clone
            Else

            End If

            If Session("ErrorData") Is Nothing Then
                ArrErrorDataList = New List(Of ExceptionLog)
                gvBatchRequestError.DataSource = ArrErrorDataList
                gvBatchRequestError.DataBind()
                Session("ErrorData") = ArrErrorDataList
            Else
                ArrErrorDataList = Session("ErrorData")
                gvBatchRequestError.DataSource = ArrErrorDataList
                gvBatchRequestError.DataBind()
            End If

            If (HelperFunctions.isEmpty(l_datatable_Cashouts)) Then
                ArrErrorDataList.Add(New ExceptionLog("", "Session Expired", "No records found for batch request process."))
                Session("ErrorData") = ArrErrorDataList
                Exit Sub
            End If

            If Not Request Is Nothing Then
                'Query string is used as index no in datatable for batch request creation.
                If Not String.IsNullOrEmpty(Request.QueryString("count")) Then
                    iCount = Convert.ToInt32(Request.QueryString("count"))
                Else
                    'Create entry in log file against the set of records.
                    'dvReg.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
                    'dvIDXProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
                    'dvPDFProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
                    'dvCopiedProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
                    objCashOutBOClass.CreateAndProcessRequest(l_datatable_Cashouts, l_bool_status, l_string_CashOutReqType)
                End If
            End If

            imgRegComplete.CssClass = "show"
            dvReg.Style.Add("display", "none")
            dvReqProgress.InnerText = l_datatable_Cashouts.Rows.Count.ToString() + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."

            For ProcessCount = iCount To l_datatable_Cashouts.Rows.Count - 1
                'number of records to be taken for batch creation process this count can be configurable.
                If ProcessCount - iCount >= 10 Then
                    Exit For
                End If

                'storing set of records in temp table so that this much of records can be process for batch creation.
                dttempCasjou = dtTempCashout.NewRow()
                dttempCasjou("PersonId") = l_datatable_Cashouts.Rows(ProcessCount)("PersonId").ToString
                dttempCasjou("SSNO") = l_datatable_Cashouts.Rows(ProcessCount)("SSNO").ToString
                dttempCasjou("FUNDNo") = l_datatable_Cashouts.Rows(ProcessCount)("FUNDNo").ToString
                dttempCasjou("FirstName") = l_datatable_Cashouts.Rows(ProcessCount)("FirstName").ToString
                dttempCasjou("LastName") = l_datatable_Cashouts.Rows(ProcessCount)("LastName").ToString
                dttempCasjou("MiddleName") = l_datatable_Cashouts.Rows(ProcessCount)("MiddleName").ToString
                dttempCasjou("Name") = l_datatable_Cashouts.Rows(ProcessCount)("Name").ToString
                dttempCasjou("PersonAgeDOB") = l_datatable_Cashouts.Rows(ProcessCount)("PersonAgeDOB").ToString
                dttempCasjou("MaxTermDate") = l_datatable_Cashouts.Rows(ProcessCount)("MaxTermDate").ToString
                dttempCasjou("FundEventId") = l_datatable_Cashouts.Rows(ProcessCount)("FundEventId").ToString
                dttempCasjou("IsTerminated") = l_datatable_Cashouts.Rows(ProcessCount)("IsTerminated").ToString
                dttempCasjou("IsVested") = l_datatable_Cashouts.Rows(ProcessCount)("IsVested").ToString
                dttempCasjou("IntAddressId") = l_datatable_Cashouts.Rows(ProcessCount)("IntAddressId").ToString
                dttempCasjou("EligibleBalance") = l_datatable_Cashouts.Rows(ProcessCount)("EligibleBalance").ToString
                dttempCasjou("TaxableAmount") = l_datatable_Cashouts.Rows(ProcessCount)("TaxableAmount").ToString
                dttempCasjou("StatusType") = l_datatable_Cashouts.Rows(ProcessCount)("StatusType").ToString
                dttempCasjou("PlansType") = l_datatable_Cashouts.Rows(ProcessCount)("PlansType").ToString
                dttempCasjou("BatchId") = l_datatable_Cashouts.Rows(ProcessCount)("BatchId").ToString
                dttempCasjou("Selected") = l_datatable_Cashouts.Rows(ProcessCount)("Selected").ToString
                dttempCasjou("LastContributionDate") = l_datatable_Cashouts.Rows(ProcessCount)("LastContributionDate").ToString
                dttempCasjou("IsHighlighted") = l_datatable_Cashouts.Rows(ProcessCount)("IsHighlighted").ToString
                dttempCasjou("RefRequestID") = l_datatable_Cashouts.Rows(ProcessCount)("RefRequestID").ToString
                dttempCasjou("IsRMDEligible") = l_datatable_Cashouts.Rows(ProcessCount)("IsRMDEligible").ToString
                dttempCasjou("chvShortDescription") = l_datatable_Cashouts.Rows(ProcessCount)("chvShortDescription").ToString
                dttempCasjou("Remarks") = l_datatable_Cashouts.Rows(ProcessCount)("Remarks").ToString
                dttempCasjou("mnyEstimatedBalance") = l_datatable_Cashouts.Rows(ProcessCount)("mnyEstimatedBalance").ToString
                dtTempCashout.Rows.Add(dttempCasjou)
            Next



            'Batch request creation process is going to resume.
            'Start:This code is aded by dines for YRS 5.0-2315
            InvokeBatchRequestCreation(iCount, dtTempCashout, "REFREQST", "Withdrawals_New.rpt", "Withdrawal_REFREQST", "BIE", ArrErrorDataList)

            InvokeProcess(ProcessCount, l_datatable_Cashouts, bAllFilesCreated, ArrErrorDataList, 0, 0, "", "", Nothing)

            ''Once the set of records for batch request creation is been process no. of count will get displayed on popup as status.
            'If ProcessCount < l_datatable_Cashouts.Rows.Count Then
            '    If Not Session("IDXCount") Is Nothing Then
            '        dvIDXProgress.InnerText = Session("IDXCount").ToString + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        '                    dvIDX.Style.Add("display", "none")
            '    Else
            '        dvIDXProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        dvIDX.Style.Add("display", "block")
            '    End If


            '    If Not Session("PDFCount") Is Nothing Then
            '        dvPDFProgress.InnerText = Session("PDFCount").ToString + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        '                   dvPDF.Style.Add("display", "none")
            '    Else
            '        dvPDFProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        dvPDF.Style.Add("display", "block")
            '    End If


            '    '            dvReqStatus.InnerText = "Inprogress"
            '    'dvReqProgress.InnerText = ProcessCount.ToString() + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."

            '    ''           dvIDXStatus.InnerText = "Inprogress"
            '    'dvIDXProgress.InnerText = ProcessCount.ToString + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."

            '    ''          dvPDFStatus.InnerText = "Inprogress"
            '    'dvPDFProgress.InnerText = ProcessCount.ToString + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."

            '    'dvCopiedProgress.InnerText = "Pending."

            '    bAllFilesCreated = False

            '    Dim popupScript3 As String = "<" & "script language='javascript'>" & _
            '                            "windowrefreshPage(" + ProcessCount.ToString + ",'')</" & "script" & ">"

            '    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
            '        Page.RegisterStartupScript("PopupScript2", popupScript3)
            '    End If
            'Else

            '    If Not Session("IDXCount") Is Nothing Then
            '        If Convert.ToInt32(Session("IDXCount")) > 0 Then
            '            imgIDXComplete.CssClass = "show"
            '            dvIDX.Style.Add("display", "none")
            '            dvIDXProgress.InnerText = Session("IDXCount").ToString + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        Else
            '            imgIDXComplete.CssClass = "hide"
            '            dvIDX.Style.Add("display", "none")
            '            dvIDXProgress.InnerText = +"0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        End If
            '    Else
            '        imgIDXComplete.CssClass = "hide"
            '        dvIDX.Style.Add("display", "none")
            '        dvIDXProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '    End If


            '    If Not Session("PDFCount") Is Nothing Then
            '        If Convert.ToInt32(Session("PDFCount")) > 0 Then
            '            imgPDFComplete.CssClass = "show"
            '            dvPDF.Style.Add("display", "none")
            '            dvPDFProgress.InnerText = Session("PDFCount").ToString + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        Else
            '            imgPDFComplete.CssClass = "hide"
            '            dvPDF.Style.Add("display", "none")
            '            dvPDFProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '        End If
            '    Else
            '        imgPDFComplete.CssClass = "hide"
            '        dvPDF.Style.Add("display", "none")
            '        dvPDFProgress.InnerText = "0 of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
            '    End If


            '    lblMsg.Visible = True
            '    If ArrErrorDataList IsNot Nothing AndAlso ArrErrorDataList.Count > 0 Then
            '        lblException.Visible = True
            '        gvBatchRequestError.DataSource = ArrErrorDataList
            '        gvBatchRequestError.DataBind()
            '        Session("ErrorData") = Nothing
            '    End If
            '    lblMsg.Text = "Batch creation completed, No. of request created: " + ProcessCount.ToString()
            '    'Response.Write("Request Processing Completed. No. of request processed: " + ProcessCount.ToString())
            '    bAllFilesCreated = True
            '    Session("AllFilesCreated") = True
            '    Session("PDFCount") = Nothing
            '    Session("IDXCount") = Nothing

            'End If
            'End:This code is aded by dines for YRS 5.0-2315
            If Not Session("FTFileList") Is Nothing And bAllFilesCreated Then
                Try
                    imgCopiedWaiting.CssClass = "show"
                    imgCopiedComplete.CssClass = "hide"

                    Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                    dvCopiedProgress.InnerText = "Completed."
                    Dim BatchID As String = String.Empty
                    BatchID = CType(Session("l_datatable_Cashouts"), DataTable).Rows(0)("BatchId") 'This code is aded by dines for YRS 5.0-2315
                    Session("PrintBatchID") = BatchID
                    Session("CashoutBatchId") = BatchID
                    If (Not Session("PrintBatchID") Is Nothing AndAlso Session("PrintBatchID").ToString().Length > 1) Then
                        Session("strReportName") = "Withdrawals_New"
                        Session("strModuleName") = "CashOut"
                        Me.PrintReport()
                    End If
                    imgCopiedComplete.CssClass = "show"
                    imgCopiedWaiting.CssClass = "hide"
                    dvCopy.Style.Add("display", "none")
                    dvCopiedProgress.InnerText = ProcessCount.ToString() + " of " + l_datatable_Cashouts.Rows.Count.ToString() + " Processed."
                Catch ex As Exception
                    Session("AllFilesCreated") = False
                    ArrErrorDataList.Add(New ExceptionLog("Exception", "Error Occured while generating reports", ex.Message))
                    Throw
                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function AddAdditionalColumns(ByRef l_datatable As DataTable) As DataTable

        If Not l_datatable.Columns.Contains("DocType") Then
            l_datatable.Columns.Add("DocType")
        End If

        If Not l_datatable.Columns.Contains("ReportName") Then
            l_datatable.Columns.Add("ReportName")
        End If

        If Not l_datatable.Columns.Contains("Param1") Then
            l_datatable.Columns.Add("Param1")
        End If

        If Not l_datatable.Columns.Contains("OutputFileType") Then
            l_datatable.Columns.Add("OutputFileType")
        End If

        If Not l_datatable.Columns.Contains("AppType") Then
            l_datatable.Columns.Add("AppType")
        End If

        If Not l_datatable.Columns.Contains("YMCANO") Then
            l_datatable.Columns.Add("YMCANO")
        End If

        If Not l_datatable.Columns.Contains("YMCAName") Then
            l_datatable.Columns.Add("YMCAName")
        End If

        If Not l_datatable.Columns.Contains("YCMAState") Then
            l_datatable.Columns.Add("YCMAState")
        End If

        If Not l_datatable.Columns.Contains("YMCACity") Then
            l_datatable.Columns.Add("YMCACity")
        End If

        If Not l_datatable.Columns.Contains("PreviewReport") Then
            l_datatable.Columns.Add("PreviewReport")
        End If

        If Not l_datatable.Columns.Contains("LogonToDb") Then
            l_datatable.Columns.Add("LogonToDb")
        End If

        If Not l_datatable.Columns.Contains("CreatePDF") Then
            l_datatable.Columns.Add("CreatePDF")
        End If

        If Not l_datatable.Columns.Contains("CreateIDX") Then
            l_datatable.Columns.Add("CreateIDX")
        End If

        If Not l_datatable.Columns.Contains("CopyFilesToIDM") Then
            l_datatable.Columns.Add("CopyFilesToIDM")
        End If


        Return l_datatable
    End Function


    'This function will invokeBatchRequestCreation once all required data gather and set for request processing.
    Public Function InvokeBatchRequestCreation(ByVal iCount As Integer, ByVal dtTempCashout As DataTable, ByVal strDocType As String, ByVal strReportName As String, ByVal strOutputFileType As String, ByVal strParam1 As String, ByRef ArrErrorData As List(Of ExceptionLog), Optional ByRef dtFileList As DataTable = Nothing, Optional ByRef iIDXCreated As Integer = 0, Optional ByRef iPDFCreated As Integer = 0,
              Optional ByVal PreviewReport As Boolean = True, Optional ByVal LogonToDb As Boolean = True, Optional ByVal CreatePDF As Boolean = True, Optional ByVal CreateIDX As Boolean = True, Optional ByVal CopyFilesToIDM As Boolean = True) As String 'This code is aded by dines for YRS 5.0-2315
        Dim l_Dataset As DataSet
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String = String.Empty
        Dim l_datatable_Cashouts As DataTable
        Dim printLetterID, idmTrackingID As String 'MMR | 2017.05.05 | YRS-AT-3205 | Declared variable to store print letter id and idm tracking ID 
        Try

            If DatatableFileList(False) Then
            Else
                ArrErrorData.Add(New ExceptionLog("Exception", "Release Banks", "Unable to generate Release Blanks, Could not create dependent table"))
                Throw New Exception("Unable to generate Release Blanks, Could not create dependent table")
            End If

            'Additional columns added in a temp table for setting report properties.
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeBatchRequestCreation Method", "Start: Adding additional columns to store IDM properties")
            l_datatable_Cashouts = AddAdditionalColumns(dtTempCashout)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeBatchRequestCreation Method", "Finish: Adding additional columns to store IDM properties")

            'Values is been provide for addition columns.
            'Start:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
            'IDM property set for IDX and PDF files generation.
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeBatchRequestCreation Method", "Start: Provide values in additional columns to for IDM properties")
            For Each row In l_datatable_Cashouts.Rows
                row("DocType") = strDocType '"REFREQST"
                row("ReportName") = strReportName  '"Withdrawals_New.rpt"

                row("PreviewReport") = PreviewReport
                row("LogonToDb") = LogonToDb
                row("CreatePDF") = CreatePDF
                row("CreateIDX") = CreateIDX
                row("CopyFilesToIDM") = CopyFilesToIDM

                'Assign report parameters for rpt file to generate PDF files
                Select Case strParam1.Trim
                    Case "BIE"
                        'Start:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                        'row("Param1") = row("RefRequestID") + "|" + strParam1
                        'START: MMR| 2016.11.10 | YRS-AT-3197 | Commented existing code and removed "|" as only one value is passed instead of multiple parameters
                        'row("Param1") = row("PersonId") + "|" 
                        row("Param1") = row("PersonId")
                        'END: MMR| 2016.11.10 | YRS-AT-3197 | Commented existing code and removed "|" as only one value is passed instead of multiple parameters
                        'End:AA:07.03.2016 YRS-AT-2764 - Added to pass persid to new cashout letter report
                        row("ReportName") = strReportName
                        row("AppType") = "P"
                        'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added to add parameters for generating the pdf
                    Case "RollIn"
                        row("ReportName") = strReportName
                        'AA 05.08.2015 BT:2825:YRS 5.0-2500:Added to change the format of address
                        row("Param1") = row("FUNDNo") + "|" + row("InstitutionName") + "|" + row("addr1") + "|" + row("addr2") + "|" + row("addr3") + "|" + row("city") + ", " + row("StateName") + " " + row("zipCode") + "|" + row("PartAccno") + "|" + "Y"
                        row("AppType") = "P"
                        'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added to add parameters for generating the pdf
                    Case "RMDPrintLetter"
                        row("ReportName") = strReportName
                        row("Param1") = row("FUNDNo").ToString()
                        row("AppType") = "P"
                    Case "GraceRMDReport"
                        row("ReportName") = strReportName
                        row("Param1") = row("FUNDNo").ToString() + "|" + row("MRDYear").ToString()
                        row("AppType") = "P"
                    Case "RegularRMDReport"
                        row("ReportName") = strReportName
                        row("Param1") = row("FUNDNo").ToString() + "|" + row("MRDYear").ToString()
                        row("AppType") = "P"
                        'Start:BJ:23.04.2015 BT:2570 - YRS 5.0:2380 Added to add parameters for generating the pdf
                    Case "Ann_Bene_Death_FL"
                        row("ReportName") = strReportName
                        row("Param1") = row("FundNo").ToString() + "|" + row("SSNo").ToString()
                        row("AppType") = "P"
                        'End:BJ:23.04.2015 BT:2570 - YRS 5.0:2380 Added to add parameters for generating the pdf
                        'START: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Conditions added for RMDPrintLetterForNonRespondent AND RMDPrintLetterForSatisfiedNotElected reports
                    Case "RMDPrintLetterForNonRespondent"
                        row("ReportName") = strReportName
                        row("Param1") = String.Format("{0}|'{1}'", row("MRDAmount").ToString(), row("FundEventID").ToString())
                        row("AppType") = "P"
                    Case "RMDPrintLetterForSatisfiedButNotElected"
                        row("ReportName") = strReportName
                        row("Param1") = String.Format("'{0}'", row("FundEventID").ToString())
                        row("AppType") = "P"
                        'END: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Conditions added for RMDPrintLetterForNonRespondent AND RMDPrintLetterForSatisfiedNotElected reports
                        'START: MMR | 2017.05.29 | YRS-AT-3205 | Added for RMDSpecialQDRO initial and follow-up letters
                    Case "RMDSpecialQDROLetters"
                        row("ReportName") = strReportName
                        row("Param1") = String.Format("'{0}'", row("FundEventID").ToString())
                        row("AppType") = "P"
                        'END: MMR | 2017.05.29 | YRS-AT-3205 | Added for RMDSpecialQDRO initial and follow-up letters
                    Case Else
                        Throw New Exception("Encountered unhandled value (" + strParam1 + ") for passing an parameter to report.")
                End Select

                row("OutputFileType") = strOutputFileType '"Withdrawal_REFREQST"
                'End:This code is aded by dines for YRS 5.0-2315
            Next
            'End:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeBatchRequestCreation Method", "Finish: Provide values in additional columns to for IDM properties")
            If HelperFunctions.isNonEmpty(l_datatable_Cashouts) Then
                If l_datatable_Cashouts.Rows.Count > 0 Then
                    l_StringErrorMessage = Me.SetPropertiesForIDM(l_datatable_Cashouts, iCount, ArrErrorData, dtFileList, iIDXCreated, iPDFCreated)
                    'START: MMR | 2017.05.05 | YRS-AT-3205 | Setting print letter id and IDM Tracking ID from datatable
                    If l_datatable_Cashouts.Columns.Contains("PrintLetterID") AndAlso l_datatable_Cashouts.Columns.Contains("RptTrackingId") Then
                        For Each row As DataRow In l_datatable_Cashouts.Rows
                            printLetterID = String.Empty
                            If Not Convert.IsDBNull(row("PrintLetterID")) Then
                                printLetterID = Convert.ToString(row("PrintLetterID"))
                            End If
                            idmTrackingID = String.Empty
                            If Not Convert.IsDBNull(row("RptTrackingId")) Then
                                idmTrackingID = Convert.ToString(row("RptTrackingId"))
                            End If
                            YMCARET.YmcaBusinessObject.AnnuityBeneficiaryDeathBOClass.UpdatePrintLetters(printLetterID, idmTrackingID)
                        Next
                    End If
                    'END: MMR | 2017.05.05 | YRS-AT-3205 | Setting print letter id and IDM Tracking ID from datatable
                End If
            End If

            InvokeBatchRequestCreation = l_StringErrorMessage

        Catch ex As Exception
            Dim l_string_message As String = "Error Occured while generating reports"
            ArrErrorData.Add(New ExceptionLog("Exception", "Error Occured while generating reports", l_string_message))
            InvokeBatchRequestCreation = l_string_message
            HelperFunctions.LogException("InvokeBatchCreationProcess_Page_Load", ex)
            Throw
        Finally

        End Try
    End Function

    'This function create datatable which stores source and destination for copying files to idm.
    Public Function DatatableFileList(ByVal l_bool_Participant As Boolean) As Boolean
        Try
            'START : CS | 2016.10.24 | YRS-AT-3088 | Condition to check the Column name are already existing in the datatable
            If Not m_dtFileList.Columns.Contains("SourceFolder") Then
                m_dtFileList.Columns.Add("SourceFolder", System.Type.GetType("System.String"))
                m_dtFileList.Columns.Add("SourceFile", System.Type.GetType("System.String"))
                m_dtFileList.Columns.Add("DestFolder", System.Type.GetType("System.String"))
                m_dtFileList.Columns.Add("DestFile", System.Type.GetType("System.String"))
                m_dtFileList.Columns.Add("FundNo", System.Type.GetType("System.String"))
            End If
            'END : CS | 2016.10.24 | YRS-AT-3088 | Condition to check the Column name are already existing in the datatable
            DatatableFileList = True
        Catch ex As Exception
            DatatableFileList = False
        End Try
    End Function

    'This function will set IDM properties which futher used to generate idx and pdf files for batch request.
    Private Function SetPropertiesForIDM(ByVal dtGenerateCashOutPdf As DataTable, ByVal iCount As Integer, ByRef ArrErrorData As List(Of ExceptionLog), Optional ByRef dtFileList As DataTable = Nothing, Optional ByRef iIDXCreated As Integer = 0, Optional ByRef iPDFCreated As Integer = 0) As String
        Dim l_StringIDXErrorMessage As String = String.Empty
        Dim l_StringPDFErrorMessage As String = String.Empty
        Dim l_datatable_FTFileList As DataTable

        Try
            'Assign values to the properties

            'AppType = "P"

            'IDX files generation is going to resume.
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SetPropertiesForIDM Method", "Start: Invoke IDX files generation process")
            l_StringIDXErrorMessage = InvokeIDXGeneration(dtGenerateCashOutPdf, iCount, ArrErrorData, dtFileList, iIDXCreated, iPDFCreated)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SetPropertiesForIDM Method", "Finish: Invoke IDX files generation process")
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SetPropertiesForIDM Method", "Start: Invoke PDF files generation process")
            l_StringPDFErrorMessage = InvokePDFGeneration(dtGenerateCashOutPdf, iCount, ArrErrorData, iPDFCreated)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SetPropertiesForIDM Method", "Finish: Invoke PDF files generation process")


            Return l_StringIDXErrorMessage + "  " + l_StringPDFErrorMessage
        Catch
            Throw
        End Try
    End Function

    'This function will generates IDX files for all participants from new batch request.
    Public Function InvokeIDXGeneration(ByVal dtGenerateCashOutPdf As DataTable, ByVal iCount As Integer, ByRef ArrErrorData As List(Of ExceptionLog), Optional ByRef dtFileList As DataTable = Nothing, Optional ByRef iIDXCreated As Integer = 0, Optional ByRef iPDFCreated As Integer = 0) As String
        Dim l_DataTable_IdxDetails As DataTable
        Dim l_StringStaffFL As String = String.Empty
        Dim l_StringSLevel As String = String.Empty
        Dim l_StringTabCode As String = String.Empty
        Dim l_StringTabLetter As String = String.Empty
        Dim l_StringApp As String = String.Empty
        Dim l_StringDocTab As String = String.Empty
        Dim l_StringWfstatus As String = String.Empty
        Dim l_StringDocType As String = String.Empty
        Dim l_OutputFileType As String = String.Empty
        Dim l_String_DocType As String = String.Empty
        Dim l_StringDocFileName As String = String.Empty
        Dim l_StringIndexFileName As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_StringDestFilePath As String = String.Empty
        Dim l_stringSourceFilePath As String = String.Empty
        Dim l_stringPDFFilePath As String = String.Empty
        Dim l_stringIndexFilePath As String = String.Empty
        Dim l_BoolRetFlag As Boolean
        Dim l_stringYMACA As String = String.Empty
        Dim l_stringYMACAName As String = String.Empty
        Dim l_stringYMACACity As String = String.Empty
        Dim l_stringYMACAState As String = String.Empty
        Dim l_DatatablePartYMCA As DataTable = Nothing
        Dim l_str_Firstname As String = String.Empty
        Dim l_str_Middlename As String = String.Empty
        Dim l_str_Lastname As String = String.Empty
        Dim l_str_SSNO As String = String.Empty
        Dim l_str_FundNo As String = String.Empty
        Dim l_ds_RptTracking As New DataSet
        Dim l_str_PersId As String = String.Empty
        Dim ProcessCount As Integer = 0
        Dim l_iCount As Integer = 0
        Try
            If HelperFunctions.isNonEmpty(dtGenerateCashOutPdf) Then
                If dtGenerateCashOutPdf.Rows.Count > 0 Then

                    'No. of columns are added in parameter table for setting IDM properties.
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Adding columns to generate RPT")
                    If Not dtGenerateCashOutPdf.Columns.Contains("RptTrackingId") Then
                        dtGenerateCashOutPdf.Columns.Add("RptTrackingId")
                    End If

                    If Not dtGenerateCashOutPdf.Columns.Contains("IsIdxCreated") Then
                        dtGenerateCashOutPdf.Columns.Add("IsIdxCreated")
                    End If

                    If Not dtGenerateCashOutPdf.Columns.Contains("IsPdfCreated") Then
                        dtGenerateCashOutPdf.Columns.Add("IsPdfCreated")
                    End If

                    If Not dtGenerateCashOutPdf.Columns.Contains("IndexFileName") Then
                        dtGenerateCashOutPdf.Columns.Add("IndexFileName")
                    End If

                    If Not dtGenerateCashOutPdf.Columns.Contains("DocFileName") Then
                        dtGenerateCashOutPdf.Columns.Add("DocFileName")
                    End If

                    'DestFile is the location where PDF and IDX files is created
                    If Not dtGenerateCashOutPdf.Columns.Contains("DestFilePath") Then
                        dtGenerateCashOutPdf.Columns.Add("DestFilePath")
                    End If

                    If Not dtGenerateCashOutPdf.Columns.Contains("IsError") Then
                        dtGenerateCashOutPdf.Columns.Add("IsError")
                    End If
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Adding columns to generate RPT")
                    'Loop is getting started to created batch requested IDX files for sets of records.

                    For ProcessCount = iCount To dtGenerateCashOutPdf.Rows.Count - 1
                        stTimeStampForFile = String.Format("{0:ddMMMyyyy_HHmmss_fff}{1}", Now, ProcessCount.ToString()) 'Manthan Rajguru | 2015.10.10 | YRS-AT-2614 | Assigning timestamp as well as counter to a varaible which is used to genearte pdf and idx filename.

                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Loop to provide values in additional columns to generate RPT")
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Loop to create IDX files for selected participant in a batch")
                        'Threading.Thread.Sleep(1000)
                        Dim drGenerateCashoutPdf As DataRow
                        drGenerateCashoutPdf = dtGenerateCashOutPdf.Rows(ProcessCount)

                        Me.CreateIDX = drGenerateCashoutPdf("CreateIDX")
                        Me.CreatePDF = drGenerateCashoutPdf("CreatePDF")
                        Me.LogonToDb = drGenerateCashoutPdf("LogonToDb")
                        Me.PreviewReport = drGenerateCashoutPdf("PreviewReport")
                        Me.CopyFilesToIDM = drGenerateCashoutPdf("CopyFilesToIDM")

                        If drGenerateCashoutPdf("AppType").ToString.ToLower.Trim = "p" Then
                            l_stringSourceFilePath = GetSourceFilePath(True)
                            l_StringDestFilePath = GetDestinationPath(True)
                            l_str_Firstname = drGenerateCashoutPdf("FirstName").ToString()
                            l_str_Middlename = drGenerateCashoutPdf("MiddleName").ToString()
                            l_str_Lastname = drGenerateCashoutPdf("LastName").ToString()
                            l_str_FundNo = drGenerateCashoutPdf("FUNDNO").ToString()
                            l_str_SSNO = drGenerateCashoutPdf("SSNo").ToString()
                            l_str_PersId = drGenerateCashoutPdf("PersonId").ToString()
                        ElseIf drGenerateCashoutPdf("AppType").ToString.ToLower.Trim = "a" Then
                            l_stringSourceFilePath = GetSourceFilePath(False)
                            l_StringDestFilePath = GetDestinationPath(False)
                            l_stringYMACA = drGenerateCashoutPdf("YMCANO").ToString()
                            l_stringYMACAName = drGenerateCashoutPdf("YMCAName").ToString()
                            l_stringYMACACity = drGenerateCashoutPdf("YMCACity").ToString()
                            l_stringYMACAState = drGenerateCashoutPdf("YCMAState").ToString()
                        End If

                        If drGenerateCashoutPdf("AppType").ToString.Trim <> "" Then
                            Me.OutputFileType = drGenerateCashoutPdf("OutputFileType").ToString.Trim
                            Me.DocTypeCode = drGenerateCashoutPdf("DocType").ToString.Trim
                            Me.ReportName = drGenerateCashoutPdf("ReportName").ToString.Trim
                            Me.RefRequestsID = drGenerateCashoutPdf("RefRequestId").ToString.Trim

                            l_OutputFileType = Me.OutputFileType
                            If l_stringSourceFilePath.Trim = "" Then
                                InvokeIDXGeneration = CType(Session("IndexFileWriteError"), String)
                                Exit Function
                            End If

                            m_str_letter_YmcaPart = drGenerateCashoutPdf("AppType").ToString.Trim
                            l_StringReportName = drGenerateCashoutPdf("ReportName").ToString.Trim
                            l_String_DocType = drGenerateCashoutPdf("DocType").ToString.Trim

                            If Me.CreateIDX = True Then
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Get details for IDX creation based on doctype from database")
                                If drGenerateCashoutPdf("AppType").ToString.Trim.ToLower = "a" Then

                                    l_DataTable_IdxDetails = YMCARET.YmcaBusinessObject.RefundRequest.GetDetailsForIdxCreationForYmca(l_String_DocType)
                                Else
                                    l_DataTable_IdxDetails = YMCARET.YmcaBusinessObject.RefundRequest.GetDetailsForIdxCreation(l_String_DocType)
                                End If
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Get details for IDX creation based on doctype from database")
                                If Not (l_DataTable_IdxDetails Is Nothing) Then
                                    If l_DataTable_IdxDetails.Rows.Count > 0 Then
                                        If l_DataTable_IdxDetails.Rows(0)(0).GetType.ToString() <> "System.DBNull" Then
                                            l_StringStaffFL = l_DataTable_IdxDetails.Rows(0)(0)
                                        Else
                                            l_StringStaffFL = ""
                                        End If
                                        If l_DataTable_IdxDetails.Rows(0)(1).GetType.ToString() <> "System.DBNull" Then
                                            l_StringSLevel = l_DataTable_IdxDetails.Rows(0)(1)
                                        Else
                                            l_StringSLevel = ""
                                        End If
                                        If l_DataTable_IdxDetails.Rows(0)(2).GetType.ToString() <> "System.DBNull" Then
                                            l_StringTabCode = l_DataTable_IdxDetails.Rows(0)(2)
                                        Else
                                            l_StringTabCode = l_StringTabLetter
                                        End If
                                        If drGenerateCashoutPdf("AppType").ToString.Trim.ToLower = "a" Then
                                            If l_DataTable_IdxDetails.Rows(0)(3).GetType.ToString() <> "System.DBNull" Then
                                                l_StringApp = l_DataTable_IdxDetails.Rows(0)(3)
                                            Else
                                                l_StringApp = "A"
                                            End If
                                            If l_DataTable_IdxDetails.Rows(0)(4).GetType.ToString() <> "System.DBNull" Then
                                                l_StringDocTab = l_DataTable_IdxDetails.Rows(0)(4)
                                            Else
                                                l_StringDocTab = ""
                                            End If
                                            If l_DataTable_IdxDetails.Rows(0)(5).GetType.ToString() <> "System.DBNull" Then
                                                l_StringDocType = l_DataTable_IdxDetails.Rows(0)(5)
                                            Else
                                                l_StringDocType = ""
                                            End If
                                            If l_DataTable_IdxDetails.Rows(0)(6).GetType.ToString() <> "System.DBNull" Then
                                                l_StringWfstatus = l_DataTable_IdxDetails.Rows(0)(6)
                                            Else
                                                l_StringWfstatus = "F"
                                            End If
                                        ElseIf drGenerateCashoutPdf("AppType").ToString.Trim.ToLower = "p" Then
                                            'Start:AA:05.07.2016 YRS-At-2985 Added below code to fetch and add the wfstatus in the idx file
                                            If l_DataTable_IdxDetails.Columns.Contains("lcWfstatus") AndAlso Not String.IsNullOrEmpty(l_DataTable_IdxDetails.Rows(0)("lcWfstatus").ToString()) Then
                                                l_StringWfstatus = l_DataTable_IdxDetails.Rows(0)("lcWfstatus")
                                            Else
                                                l_StringWfstatus = "F"
                                            End If
                                            'End:AA:05.07.2016 YRS-At-2985 Added below code to fetch and add the wfstatus in the idx file
                                        End If
                                    Else
                                        InvokeIDXGeneration = "Cannot find the details for the IDX configuration for " & l_String_DocType & " "
                                        ArrErrorData.Add(New ExceptionLog(drGenerateCashoutPdf("FUNDNO").ToString(), "Cannot find the details for the IDX configuration.", l_String_DocType))
                                        Exit Function
                                    End If
                                Else
                                    InvokeIDXGeneration = "Cannot find the details for the IDX configuration " & l_String_DocType & " "
                                    ArrErrorData.Add(New ExceptionLog(drGenerateCashoutPdf("FUNDNO").ToString(), "Cannot find the details for the IDX configuration.", l_String_DocType))
                                    Exit Function
                                End If
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Get IDX filename and filepath")
                                l_StringDocFileName = ""
                                l_StringDocFileName = Me.IDXFileName()
                                l_StringIndexFileName = l_StringDocFileName & ".idx"
                                l_stringIndexFilePath = l_stringSourceFilePath.Trim() & "IDX"
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Get IDX filename and filepath")
                            End If
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Start generating PDF file process")
                            If Me.CreatePDF = True Then
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Get PDF filename and filepath")
                                l_StringDocFileName = ""
                                l_StringDocFileName = Me.PDFFileName()
                                l_StringDocFileName = l_StringDocFileName & ".pdf"
                                l_stringPDFFilePath = l_stringSourceFilePath.Trim() & "PDF"
                                drGenerateCashoutPdf("DestFilePath") = l_stringPDFFilePath & "\\" & l_StringDocFileName
                                Me.AddFileListRow(l_stringPDFFilePath, l_StringDocFileName, l_StringDestFilePath, l_StringDocFileName, l_str_SSNO, dtFileList)
                                If l_StringDocFileName.Trim = "" Then
                                    InvokeIDXGeneration = "Cannot Generate Export File Name"
                                    ArrErrorData.Add(New ExceptionLog(drGenerateCashoutPdf("FUNDNO").ToString(), "Cannot Generate Export File Name.", l_StringDocFileName))
                                    Exit Function
                                End If
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Get PDF filename and filepath")
                            End If
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Updating IDMtracking table")
                            l_ds_RptTracking = initialiseFileTracking(drGenerateCashoutPdf("AppType").ToString.Trim, l_String_DocType, l_OutputFileType, l_StringTabCode, l_StringIndexFileName, l_stringIndexFilePath, l_StringDocFileName, l_stringPDFFilePath, l_StringDestFilePath, l_stringYMACA, l_str_FundNo)
                            l_str_rptTrackingId = YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAOutputFileIDMTrackingLogs(l_ds_RptTracking)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Updating IDMtracking table")

                            drGenerateCashoutPdf("RptTrackingId") = l_str_rptTrackingId
                            drGenerateCashoutPdf("IsIdxCreated") = m_bool_idxCreated
                            drGenerateCashoutPdf("IsPdfCreated") = m_bool_PdfCreated
                            drGenerateCashoutPdf("IndexFileName") = l_StringIndexFileName
                            drGenerateCashoutPdf("DocFileName") = l_StringDocFileName

                            If Me.CreateIDX = True Then
                                Try
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Creation of IDX files in a temp location")
                                    If drGenerateCashoutPdf("AppType").ToString.Trim.ToLower = "a" Then
                                        'Start: AA:05.07.2016 YRS-AT-2985 Commented below lines and calling the new method name
                                        'l_BoolRetFlag = Me.CreatePersIdxForTower(l_stringIndexFilePath, l_StringIndexFileName, l_stringYMACA, l_stringYMACAName, l_stringYMACACity, l_stringYMACAState, l_StringApp, l_String_DocType, l_StringDocTab, l_StringSLevel, l_StringTabCode, l_StringWfstatus)
                                        l_BoolRetFlag = Me.CreateYMCAIdxForTower(l_stringIndexFilePath, l_StringIndexFileName, l_stringYMACA, l_stringYMACAName, l_stringYMACACity, l_stringYMACAState, l_StringApp, l_String_DocType, l_StringDocTab, l_StringSLevel, l_StringTabCode, l_StringWfstatus)
                                        'End: AA:05.07.2016 YRS-AT-2985 Commented below lines and calling the new method name
                                    Else
                                        l_BoolRetFlag = Me.CreatePersIdxForTower(l_stringIndexFilePath, l_StringIndexFileName, l_str_FundNo, l_str_SSNO, l_str_Firstname, l_str_Middlename, l_str_Lastname, l_StringSLevel, l_String_DocType, l_StringStaffFL, l_StringTabCode _
                                                                                 , l_StringWfstatus) 'AA:05.07.2016 YRS-AT-2985 Added to store wfstatus line in idx file
                                    End If
                                    m_bool_idxCreated = IIf(File.Exists(l_stringIndexFilePath & "\\" & l_StringIndexFileName) = True, True, False)
                                    If l_BoolRetFlag = False Then
                                        m_bool_idxCreated = False
                                        Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "Unable to create IDX file.")
                                    Else
                                        Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "")
                                    End If

                                    If iIDXCreated = 0 Then
                                        iIDXCreated += 1
                                    Else
                                        iIDXCreated += 1
                                    End If

                                    If dtGenerateCashOutPdf.Columns.Contains("IDXCount") Then
                                        drGenerateCashoutPdf("IDXCount") = 1
                                    End If

                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Creation of IDX files in a temp location")

                                Catch ex As Exception
                                    l_BoolRetFlag = False
                                    m_bool_idxCreated = False
                                    l_str_Exception = ""
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Exception raised while creating IDX files")
                                    If l_str_ExceptionMode = "IE" Then
                                        l_str_Exception = ex.Message
                                        If Not ex.InnerException Is Nothing Then
                                            l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                        End If
                                    ElseIf l_str_ExceptionMode = "ST" Then
                                        l_str_Exception = ex.StackTrace
                                    End If
                                    drGenerateCashoutPdf("IsError") = "1"
                                    ArrErrorData.Add(New ExceptionLog(l_str_FundNo.ToString(), "IDX files creation", ex.Message))
                                    Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", l_str_Exception)
                                    HelperFunctions.LogException("InvokeIDXGeneration", ex)
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Exception raised while creating IDX files")
                                End Try
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Capture source and destination files path to a filelistrow datatable")
                                If l_BoolRetFlag Then
                                    Me.AddFileListRow(l_stringIndexFilePath, l_StringIndexFileName, l_StringDestFilePath, l_StringIndexFileName, l_str_SSNO, dtFileList)
                                End If
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Capture source and destination files path to a filelistrow datatable")
                            End If
                            'IDX Files getting write into file and stored on a temp location.
                            If m_bool_Createidx = True Then
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Start writing IDX deatils in IDX files and stored on a ")
                                If File.Exists(l_stringIndexFilePath & "\\" & l_StringIndexFileName) = True Then
                                    Dim l_StreamWriter As StreamWriter
                                    l_StreamWriter = File.AppendText(l_stringIndexFilePath & "\\" & l_StringIndexFileName)
                                    l_StreamWriter.WriteLine("spchar1=" & l_str_rptTrackingId)
                                    l_StreamWriter.Close()
                                End If
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Start writing IDX deatils in IDX files")
                            End If
                        End If
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Loop to create IDX files for selected participant in a batch")
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Loop to provide values in additional columns to generate RPT")

                    Next
                End If
            End If
            InvokeIDXGeneration = ""
            Return InvokeIDXGeneration
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Start: Exception raised")
            InvokeIDXGeneration = "Error While Writing IDX File"
            ArrErrorData.Add(New ExceptionLog(l_str_FundNo.ToString(), "Error While Writing IDX File", ex.Message))
            HelperFunctions.LogException("InvokeIDXGeneration", ex)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokeIDXGeneration Method", "Finish: Exception raised")
            Throw
        End Try

        'If Me.PreviewReport = True Then
        '    Try
        '        'Invking PDF generation.
        '        InvokePDFGeneration(dtGenerateCashOutPdf, iCount, ArrErrorData, iPDFCreated)
        '    Catch ex As Exception
        '        l_str_Exception = ""
        '        If l_str_ExceptionMode = "IE" Then
        '            l_str_Exception = ex.Message
        '            If Not ex.InnerException Is Nothing Then
        '                l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
        '            End If
        '        ElseIf l_str_ExceptionMode = "ST" Then
        '            l_str_Exception = ex.StackTrace
        '        End If
        '        Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", l_str_Exception)  '' Added  by sanjay
        '        ArrErrorData.Add(New ExceptionLog(l_str_FundNo.ToString(), "PDF files creation", ex.Message))
        '        HelperFunctions.LogException("InvokePDFGeneration", ex)
        '        Throw
        '    End Try
        'End If
        '' Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "")


    End Function

    'This function will return the temp destination location from where generated idx and pdf files get stores so that later they can be move to IDM server
    Public Function GetDestinationPath(ByVal p_boolParticipant As Boolean) As String
        Dim l_string_ServerName As String
        Dim l_string_FilePath As String
        Try

            'Start:Dinesh Kanojia     09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
            'l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
            'l_string_FilePath = YMCARET.YmcaBusinessObject.RefundRequest.GetVignettePath(l_string_ServerName)

            If HttpContext.Current.Cache("l_string_ServerName") Is Nothing Then
                l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
                HttpContext.Current.Cache("l_string_ServerName") = l_string_ServerName
            Else
                l_string_ServerName = HttpContext.Current.Cache("l_string_ServerName")
            End If

            If HttpContext.Current.Cache("l_string_FilePath") Is Nothing Then
                l_string_FilePath = YMCARET.YmcaBusinessObject.RefundRequest.GetVignettePath(l_string_ServerName)
                HttpContext.Current.Cache("l_string_FilePath") = l_string_FilePath
            Else
                l_string_FilePath = HttpContext.Current.Cache("l_string_FilePath")
            End If
            'Start:AA:06.27.2016 BT-1156 Add a precautionary measure while IDM creation 
            'If the destination path was not set in the serverlookup then the exception wil be raised
            If String.IsNullOrEmpty(l_string_FilePath) Then
                Dim ex As New Exception("IDM Destination path is not configured in serverlookup table. Please contact database administrator.")
                HelperFunctions.LogException("BatchIDM_GetDestinationPath", ex)
                Throw ex
            End If
            'End:AA:06.27.2016 BT-1156 Add a precautionary measure while IDM creation 
            'End:Dinesh Kanojia     09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
            'Modified By Ashutosh Patil as on 18-Apr-2007
            If p_boolParticipant = True Then
                If Right(l_string_FilePath, 1) = "\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "PARTICIPANT\"
                Else
                    l_string_FilePath = l_string_FilePath.Trim() & "\PARTICIPANT\"
                End If
            Else
                If Right(l_string_FilePath, 1) = "\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "YMCA\"
                Else
                    l_string_FilePath = l_string_FilePath.Trim() & "\YMCA\"
                End If
            End If
            GetDestinationPath = l_string_FilePath
        Catch
            Throw
        End Try
    End Function

    'This function will return the temp source location where generated idx and pdf files get moved so that later they can be move to IDM server
    Private Function GetSourceFilePath(ByVal p_boolParticipant As Boolean) As String
        Dim l_string_ServerName As String
        Dim l_string_FilePath As String
        Try
            ' l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
            l_string_FilePath = ConfigurationSettings.AppSettings("IDMPath")
            'Modified By Ashutosh Patil as on 18-Apr-2007
            If p_boolParticipant = True Then
                If Right(l_string_FilePath, 1) = "\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "PARTICIPANT\"
                Else
                    l_string_FilePath = l_string_FilePath.Trim() & "\PARTICIPANT\"
                End If
            Else
                If Right(l_string_FilePath, 1) = "\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "YMCA\"
                Else
                    l_string_FilePath = l_string_FilePath.Trim() & "\YMCA\"
                End If
            End If
            GetSourceFilePath = l_string_FilePath
        Catch
            Throw
        End Try
    End Function

    'This function will created idx files content for YMCA.
    'Start AA:05.09.2016 YRS-AT-2985 Commented below line and added the a new method because it is violating the method overloading rule
    'Private Function CreatePersIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringYMCANO As String, ByVal p_StringYMCAName As String, ByVal p_StringYMCACity As String, ByVal p_StringYMCAState As String, ByVal p_StringApp As String, ByVal p_StringDocCode As String, ByVal p_StringDoctab As String, ByVal p_StringLevel As String, ByVal p_StringTabCode As String, ByVal p_StringWfstatus As String) As Boolean
    Private Function CreateYMCAIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringYMCANO As String, ByVal p_StringYMCAName As String, ByVal p_StringYMCACity As String, ByVal p_StringYMCAState As String, ByVal p_StringApp As String, ByVal p_StringDocCode As String, ByVal p_StringDoctab As String, ByVal p_StringLevel As String, ByVal p_StringTabCode As String, ByVal p_StringWfstatus As String) As Boolean
        'End AA:05.09.2016 YRS-AT-2985 Commented below line and added the a new method because it is violating the method overloading rule
        Dim l_StreamWriter As StreamWriter
        Dim l_BoolFileOpened As Boolean
        ''IB: Added on 02/Sep/2010: Check data before creation of IDX file
        Dim l_IndexFileData As StringBuilder = New StringBuilder()
        Try
            'Return True
            If p_StringFilePath.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid File Path Passed for Index file creation."
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid File Path Passed for Index file creation.", MessageBoxButtons.Stop)
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            If p_StringIndexFileName.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid File Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid File Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            If p_StringIndexFileName.Trim.Substring(p_StringIndexFileName.Trim.LastIndexOf(".")).ToUpper <> ".IDX" Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid File Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid File Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            If p_StringYMCANO.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid YMCA NO Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("ymcanum=" & p_StringYMCANO.Trim)
            l_IndexFileData.Append("ymcanum=" & p_StringYMCANO.Trim)
            If p_StringYMCAName.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid YMCA Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("name=" & p_StringYMCAName.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "name=" & p_StringYMCAName.Trim)

            If p_StringYMCACity = "System.DBNull" Then
                Session("IndexFileWriteError") = "Invalid YMCA City Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("city=" & p_StringYMCACity.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "city=" & p_StringYMCACity.Trim)
            If p_StringYMCAState.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid YMCA Sate Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("state=" & p_StringYMCAState.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "state=" & p_StringYMCAState.Trim)
            If p_StringDocCode.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Document Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("doccode=" & p_StringDocCode.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "doccode=" & p_StringDocCode.Trim)
            If p_StringDoctab.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Document Tab Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("descript=" & p_StringDoctab.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "descript=" & p_StringDoctab.Trim)
            If p_StringLevel.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid S Level Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("slevel=" & p_StringLevel.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "slevel=" & p_StringLevel.Trim)
            If p_StringTabCode.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Tab Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("subtab=" & p_StringTabCode.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "subtab=" & p_StringTabCode.Trim)
            If p_StringWfstatus.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid wfStatus Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("wfstatus=" & p_StringWfstatus.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "wfstatus=" & p_StringWfstatus.Trim)

            If File.Exists(p_StringFilePath & "\\" & p_StringIndexFileName) = True Then
                File.Delete(p_StringFilePath & "\\" & p_StringIndexFileName)
            End If

            l_StreamWriter = File.CreateText(p_StringFilePath & "\\" & p_StringIndexFileName)
            l_BoolFileOpened = True

            If File.Exists(p_StringFilePath & "\\" & p_StringIndexFileName) = False Then
                Session("IndexFileWriteError") = "Index file could not be created."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine(l_IndexFileData.ToString())
            l_StreamWriter.Close()
            l_BoolFileOpened = False
            Return True

        Catch ex As Exception
            If l_BoolFileOpened = True Then l_StreamWriter.Close()
            Throw
            'Return False

        End Try
    End Function

    'This function will created idx files content for participants.
    Private Function CreatePersIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringFundId As String, ByVal p_StringSSNo As String, ByVal p_StringFirstName As String, ByVal p_StringMiddleName As String, ByVal p_StringLastName As String, ByVal p_StringLevel As String, ByVal p_StringDocCode As String, ByVal p_StringStaffFL As String, ByVal p_StringTabCode As String _
                                           , ByVal p_StringWfstatus As String) As Boolean 'AA:05.07.2016 YRS-AT-2985 Added to store wfstatus line in idx file
        Dim l_StreamWriter As StreamWriter
        Dim l_BoolFileOpened As Boolean
        ''IB: Added on 02/Sep/2010: Check data before creation of IDX file
        Dim l_IndexFileData As StringBuilder = New StringBuilder()
        Try
            If p_StringFilePath.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid File Path Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            If p_StringIndexFileName.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid File Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            If p_StringIndexFileName.Trim.Substring(p_StringIndexFileName.Trim.LastIndexOf(".")).ToUpper <> ".IDX" Then
                Session("IndexFileWriteError") = "Invalid File Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            If p_StringFundId.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid FundID Number Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("fundid=" & p_StringFundId.Trim)
            l_IndexFileData.Append("fundid=" & p_StringFundId.Trim)
            If p_StringSSNo.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid SS Number Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("ssn=" & p_StringSSNo.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "ssn=" & p_StringSSNo.Trim)
            If p_StringFirstName.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid First Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("fname=" & p_StringFirstName.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "fname=" & p_StringFirstName.Trim)
            If p_StringMiddleName = "System.DBNull" Then
                Session("IndexFileWriteError") = "Invalid Middle Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("mname=" & p_StringMiddleName.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "mname=" & p_StringMiddleName.Trim)
            If p_StringLastName.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Last Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("lname=" & p_StringLastName.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "lname=" & p_StringLastName.Trim)
            If p_StringLevel.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid S Level Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("slevel=" & p_StringLevel.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "slevel=" & p_StringLevel.Trim)
            If p_StringDocCode.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Document Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("doccode=" & p_StringDocCode.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "doccode=" & p_StringDocCode.Trim)

            If p_StringStaffFL.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Staff Flag Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("stafffl=" & p_StringStaffFL.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "stafffl=" & p_StringStaffFL.Trim)
            If p_StringTabCode.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid Tab Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            'l_StreamWriter.WriteLine("subtab=" & p_StringTabCode.Trim)
            l_IndexFileData.Append(System.Environment.NewLine + "subtab=" & p_StringTabCode.Trim)

            'Start : AA:05.09.2016 YRS-AT-2985 Added below code to include wfstatus line in idx file
            If String.IsNullOrEmpty(p_StringWfstatus.Trim) Then
                Session("IndexFileWriteError") = "Invalid wfStatus Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_IndexFileData.Append(System.Environment.NewLine + "wfstatus=" & p_StringWfstatus.Trim)
            'End : AA:05.09.2016 YRS-AT-2985 Added below code to include wfstatus line in idx file

            If File.Exists(p_StringFilePath & "\\" & p_StringIndexFileName) = True Then
                File.Delete(p_StringFilePath & "\\" & p_StringIndexFileName)
            End If
            l_StreamWriter = File.CreateText(p_StringFilePath & "\\" & p_StringIndexFileName)
            l_BoolFileOpened = True
            If File.Exists(p_StringFilePath & "\\" & p_StringIndexFileName) = False Then
                Session("IndexFileWriteError") = "Index file could not be created."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine(l_IndexFileData.ToString())
            l_StreamWriter.Close()
            l_BoolFileOpened = False
            Return True
        Catch
            If l_BoolFileOpened = True Then l_StreamWriter.Close()
            Throw
            'Return False
        End Try
    End Function

    'This function create and entry in IDMTracking table to record the every status of batch request processing.
    Private Function initialiseFileTracking(ByVal pAppType As String, ByVal pdoctype As String, ByVal poutputfiletype As String, ByVal ptabcode As String, ByVal pIndexfilename As String, ByVal pindexfilepath As String, ByVal ppdffilename As String, ByVal ppdffilepath As String, ByVal pdestpdffilepath As String, ByVal pymca As String, ByVal pFundNo As String) As DataSet
        Dim drAdd As DataRow
        If pAppType = "A" Then
            m_str_EntityId = pymca
        ElseIf pAppType = "P" Then
            m_str_EntityId = pFundNo
        End If
        m_dtReportTracking.Columns.Clear()
        m_dtReportTracking.Rows.Clear()
        p_ds_rpttracking.Tables.Clear()
        CreateDatatableReportTracking()
        drAdd = m_dtReportTracking.NewRow()
        m_bool_PdfCreated = False
        m_bool_idxCreated = False
        'Added By SG: 2012.09.05: BT-960
        'AddReportTrackingRow(drAdd, pdoctype, poutputfiletype, ptabcode, ReportParameters(0), pAppType, m_str_EntityId, pIndexfilename, pindexfilepath, ppdffilename, ppdffilepath, m_bool_PdfCreated, m_bool_idxCreated, pdestpdffilepath)
        AddReportTrackingRow(drAdd, pdoctype, poutputfiletype, ptabcode, l_RefRequestsID, pAppType, m_str_EntityId, pIndexfilename, pindexfilepath, ppdffilename, ppdffilepath, m_bool_PdfCreated, m_bool_idxCreated, pdestpdffilepath)
        m_dtReportTracking.Rows.Add(drAdd)
        p_ds_rpttracking.Tables.Add(m_dtReportTracking)
        Return p_ds_rpttracking
    End Function

    'This function create datatable for IDM tracking table.
    Public Sub CreateDatatableReportTracking()
        m_dtReportTracking.Columns.Add("ReportTrackingID", System.Type.GetType("System.Int32"))
        m_dtReportTracking.Columns.Add("chvDoccode", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvSubDocName", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvDocName", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvRefId", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chrEntityType", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvEntityId", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("dtmDocdate", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvidxFilePath", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvIdxFileName", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("bitIdxCreated", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("DestFile", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("bitIdxCopyInitiated", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("bitIdxCopied", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("bitidxDeletedafterCopy", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("chvPdfFilePath", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("chvPdfFileName", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("bitPdfCreated", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("bitPdfCopyInitiated", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("bitPdfCopied", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("bitpdfDeletedafterCopy", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("chvDestnationFolder", System.Type.GetType("System.String"))
        m_dtReportTracking.Columns.Add("bitDocExistsInIDM", System.Type.GetType("System.Boolean"))
        m_dtReportTracking.Columns.Add("dtmdocVerifiedInIDM", System.Type.GetType("System.String"))
    End Sub

    'This function record data in IDM tracking table.
    Private Sub AddReportTrackingRow(ByVal dr As DataRow, ByVal DocCode As String, ByVal SubDocName As String, ByVal DocName As String, ByVal RefId As String, ByVal EntityType As String, ByVal EntityId As String, ByVal IDXFilename As String, ByVal IDXFilePath As String, ByVal PDFFileName As String, ByVal PDFFilePath As String, ByVal PdfCreated As Boolean, ByVal idxCreated As Boolean, ByVal DestFilePath As String)
        dr("ReportTrackingID") = 1
        dr("chvDoccode") = DocCode
        dr("chvSubDocName") = SubDocName
        dr("chvDocName") = DocName
        dr("chvRefId") = RefId
        dr("chrEntityType") = EntityType
        dr("chvEntityId") = EntityId
        dr("dtmDocdate") = DateTime.Now.Today.ToString()
        dr("chvidxFilePath") = IDXFilePath.Trim
        dr("chvIdxFileName") = IDXFilename
        dr("bitIdxCreated") = idxCreated
        dr("bitIdxCopyInitiated") = False
        dr("bitIdxCopied") = False
        dr("bitidxDeletedafterCopy") = False
        dr("chvPdfFilePath") = PDFFilePath
        dr("chvPdfFileName") = PDFFileName
        dr("bitPdfCreated") = PdfCreated
        dr("bitPdfCopyInitiated") = False
        dr("bitPdfCopied") = False
        dr("bitpdfDeletedafterCopy") = False
        dr("chvDestnationFolder") = DestFilePath
        dr("bitDocExistsInIDM") = False
        dr("dtmdocVerifiedInIDM") = Now.Today.ToString()
    End Sub

    'This function will record source and destination of generated idx and pdf files in datatable which futher use the same to move all files to IDM server.
    Public Function AddFileListRow(ByVal p_String_SourceFolder As String, ByVal p_String_SourceFile As String, ByVal p_String_DestFolder As String, ByVal p_String_DestFile As String, ByVal strFundNo As String, Optional ByRef dtFileList As DataTable = Nothing) As Boolean
        Dim l_curValues As DataRow
        Try
            If m_dtFileList Is Nothing Then
                m_dtFileList = CreateFileListSchema()
            End If
            If p_String_SourceFolder <> "" And p_String_SourceFile <> "" And p_String_DestFolder <> "" And p_String_DestFile <> "" Then
                l_curValues = m_dtFileList.NewRow()
                l_curValues("FundNo") = strFundNo
                l_curValues("SourceFolder") = p_String_SourceFolder
                If Right(p_String_SourceFolder, 1) = "\" Then
                    l_curValues("SourceFile") = p_String_SourceFolder & p_String_SourceFile
                Else
                    l_curValues("SourceFile") = p_String_SourceFolder & "\" & p_String_SourceFile
                End If

                l_curValues("DestFolder") = p_String_DestFolder
                If Right(p_String_DestFolder, 1) = "\" Then
                    l_curValues("DestFile") = p_String_DestFolder & p_String_DestFile
                Else
                    l_curValues("DestFile") = p_String_DestFolder & "\" & p_String_DestFile
                End If
                m_dtFileList.Rows.Add(l_curValues)
                '    Session("AddBatchFileList") = m_dtFileList
                dtFileList = m_dtFileList
                AddFileListRow = True
            End If
        Catch
            Throw
        End Try
    End Function

    'This function will generate PDF files and stores to temp location once all idx files is been created successfully.
    Public Function InvokePDFGeneration(ByVal dtGeneratePdf As DataTable, ByVal iCount As Integer, ByRef ArrErrorData As List(Of ExceptionLog), Optional ByRef iPDFCreated As Integer = 0)
        Dim l_string_filepath As String
        Dim l_string_reportpath As String
        Dim crCon As New ConnectionInfo
        Dim CrTableLogonInfo As New TableLogOnInfo
        Dim CrTables As Tables
        Dim CrTable As Table
        Dim l_string_DataSource As String
        Dim l_string_DatabaseName As String
        Dim l_string_UserID As String
        Dim l_string_Password As String
        Dim l_paramItem As String
        Dim l_String_DestFilePath As String
        'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        'Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim objRpt As CrystalDecisions.CrystalReports.Engine.ReportDocument
        'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        Dim strRequestId As String = String.Empty
        Dim ProcessCount As Integer = 0
        Dim l_str_FundNo As String = String.Empty
        Dim l_iCount As Integer = 0
        Dim strParameter As List(Of String())
        ' START | SR | 2016.11.22 | YRS-AT-3118 - Define variables.
        Dim restartCounter As Integer = 0
        Dim attemptsToCreatePDFKeyValue As DataSet
        Dim processSleepTimeKeyValue As DataSet
        ' END | SR | 2016.11.22 | YRS-AT-3118 - Define variables.
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Assign RPT values")
            ' l_String_DestFilePath = CType(Session("StringDestFilePath"), String)
            l_string_DataSource = ConfigurationSettings.AppSettings("DataSource")
            l_string_DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            l_string_UserID = ConfigurationSettings.AppSettings("UserID")
            l_string_Password = ConfigurationSettings.AppSettings("Password")
            l_string_reportpath = System.Configuration.ConfigurationSettings.AppSettings("ReportPath")
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Assign RPT values")

            ' START | SR | 2016.11.28 | YRS-AT-3118 - Get creat PDF attempt & Process sleep time on failure configured value from database.
            If (AttemptsToCreatePDF = 0) Then
                attemptsToCreatePDFKeyValue = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("PDFGENERATION_ATTEMPTS_ON_FAILURE")
                If HelperFunctions.isNonEmpty(attemptsToCreatePDFKeyValue) Then
                    AttemptsToCreatePDF = attemptsToCreatePDFKeyValue.Tables(0).Rows(0)("Value")
                Else
                    AttemptsToCreatePDF = 3
                End If
            End If

            If (ProcessSleepTime = 0) Then
                processSleepTimeKeyValue = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("PDFGENERATION_DELAY_ON_FAILURE")
                If HelperFunctions.isNonEmpty(processSleepTimeKeyValue) Then
                    ProcessSleepTime = processSleepTimeKeyValue.Tables(0).Rows(0)("Value")
                Else
                    ProcessSleepTime = 3000
                End If
            End If
            ' END | SR | 2016.11.28 | YRS-AT-3118 - Get creat PDF attempt & Process sleep time on failure configured value from database.

            If HelperFunctions.isNonEmpty(dtGeneratePdf) Then
                If dtGeneratePdf.Rows.Count > 0 Then
                    Session("rptObject") = Nothing
                    For ProcessCount = iCount To dtGeneratePdf.Rows.Count - 1
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Loop to create PDF files from RPT")
                        ' Threading.Thread.Sleep(1000)
                        Dim drGenerateCashoutPdf As DataRow
                        drGenerateCashoutPdf = dtGeneratePdf.Rows(ProcessCount)
                        l_String_DestFilePath = drGenerateCashoutPdf("DestFilePath")
                        'strRequestId = drGenerateCashoutPdf("RefRequestId").ToString.Trim
                        l_string_filepath = l_string_reportpath.Trim() & "\\" & drGenerateCashoutPdf("ReportName").ToString().Trim
                        'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report

                        Dim multipleParams As String() = drGenerateCashoutPdf("param1").ToString.Trim.Split("|")
                        strParameter = New List(Of String())
                        strParameter.Add(multipleParams)

                        restartCounter = 0
RestartPoint:           ' SR | 2016.11.22 | YRS-AT-3118 | If any error occurs program will again attempt to generate PDF from this point
                        'If IsMultipleParamExists Then
                        '    Dim multipleParams As String() = drGenerateCashoutPdf("param1").ToString.Trim.Split("|")
                        '    strParameter = New List(Of String())
                        '    strParameter.Add(multipleParams)
                        'Else
                        '    'Start:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                        '    'strParameter = New List(Of String())
                        '    'strParameter.Add(New String() {strRequestId, drGenerateCashoutPdf("param1").ToString.Trim})
                        '    'End:Dinesh .Kanojia    06.07.2014      BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                        'End If
                        'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report

                        l_str_FundNo = drGenerateCashoutPdf("FUNDNO").ToString()

                        'Start:Dinesh Kanojia   09-Nov-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                        'If Session("rptObject") Is Nothing Then
                        '    objRpt = New CrystalDecisions.CrystalReports.Engine.ReportDocument
                        '    objRpt.Load(l_string_filepath, CrystalDecisions.Shared.OpenReportMethod.OpenReportByTempCopy)
                        '    Session("rptObject") = objRpt
                        'Else
                        '    objRpt = CType(Session("rptObject"), CrystalDecisions.CrystalReports.Engine.ReportDocument)
                        'End If
                        Try ' SR | 2016.11.22 | YRS-AT-3118 - Catch error in case of error while creating PDF file. 
                            If objRpt Is Nothing Then
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Load RPT objects")
                                objRpt = New CrystalDecisions.CrystalReports.Engine.ReportDocument
                                'objRpt.Load(l_string_filepath, CrystalDecisions.Shared.OpenReportMethod.OpenReportByTempCopy)
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Load RPT objects")
                            End If

                            'objRpt.Refresh()
                            'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Assign RPT parameters to generate PDF files")
                            If strParameter.Count > 0 Then
                                'objRpt.ParameterFields.Clear()
                                Dim strparamValue() As String
                                strparamValue = strParameter.Item(0).ToArray()
                                objRpt.ParameterFields(0).CurrentValues.Clear() 'MMR| 2016.11.10 | YRS-AT-3197 | Clearing current values from report parameter to avoid duplicate PDF generation
                                Dim l_paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = objRpt.ParameterFields
                                Dim l_paramField As CrystalDecisions.Shared.ParameterField = l_paramFieldsCollection(0)
                                Dim l_curValues As CrystalDecisions.Shared.ParameterValues = l_paramField.CurrentValues
                                Dim l_discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                                Dim i As Integer
                                For i = 0 To strparamValue.Count - 1
                                    l_curValues = New CrystalDecisions.Shared.ParameterValues
                                    l_discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                                    l_paramItem = strparamValue(i)
                                    l_paramField = l_paramFieldsCollection(i)
                                    l_curValues = l_paramField.CurrentValues
                                    l_discreteValue.Value = l_paramItem.ToString
                                    l_curValues.Add(l_discreteValue)
                                    objRpt.ParameterFields(i).CurrentValues = l_curValues
                                Next
                            End If
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Assign RPT parameters to generate PDF files")
                            If Me.LogonToDb Then
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Assign datasource and login credential to RPT object")
                                CrTables = objRpt.Database.Tables
                                crCon.ServerName = l_string_DataSource
                                crCon.DatabaseName = l_string_DatabaseName
                                crCon.UserID = l_string_UserID
                                crCon.Password = l_string_Password
                                For Each CrTable In CrTables
                                    CrTableLogonInfo = CrTable.LogOnInfo
                                    CrTableLogonInfo.ConnectionInfo = crCon
                                    CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                                    CrTable.Location = l_string_DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                                Next
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Assign datasource and login credential to RPT object")
                            End If
                            'HelperFunctions.CreateDSTrackingIDMProcess("Initailizing Report generation start ", Me.PersId, Date.Now.ToString, "", "")
                            'objRpt.ExportToDisk(ExportFormatType.PortableDocFormat, l_String_DestFilePath)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Export to Disk")
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: copy generated PDf files to temp location")
                            objRpt.ExportToDisk(ExportFormatType.PortableDocFormat, drGenerateCashoutPdf("DestFilePath").ToString.Trim)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: copy generated PDf files to temp location")
                            m_bool_PdfCreated = True
                            m_bool_idxCreated = True
                            drGenerateCashoutPdf("IsIdxCreated") = True
                            drGenerateCashoutPdf("IsPdfCreated") = True
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Export to Disk")
                            'HelperFunctions.CreateDSTrackingIDMProcess("Report generated sucessfully", Me.PersId, "", Date.Now.ToString, "")
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Update IDMTracking table in database")
                            Me.FilesTracking(IIf(drGenerateCashoutPdf("RptTrackingId").ToString.Trim = "", 0, Convert.ToInt32(drGenerateCashoutPdf("RptTrackingId"))), drGenerateCashoutPdf("IsIdxCreated"), drGenerateCashoutPdf("IsPdfCreated"), drGenerateCashoutPdf("IndexFileName"), drGenerateCashoutPdf("DocFileName"), False, False, False, False, False, False, "C", "")
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Update IDMTracking table in database")

                            If iPDFCreated = 0 Then
                                iPDFCreated += 1
                            Else
                                iPDFCreated += 1
                            End If

                            If dtGeneratePdf.Columns.Contains("PDFCount") Then
                                drGenerateCashoutPdf("PDFCount") = 1
                            End If

                            ' START | SR | 2016.11.22 | YRS-AT-3118 - Catch error in case of error while creating PDF file. 
                        Catch ex As Exception
                            If restartCounter < AttemptsToCreatePDF Then
                                System.Threading.Thread.Sleep(ProcessSleepTime)
                                restartCounter = restartCounter + 1
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Retry PDF creation count :" + Convert.ToString(restartCounter))
                                ex = Nothing
                                GoTo RestartPoint
                            Else
                                Throw
                            End If
                        End Try
                        ' END | SR | 2016.11.22 | YRS-AT-3118 - Catch error in case of error while creating PDF file. 
                        'YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(drGenerateCashoutPdf("RefRequestID").ToString, drGenerateCashoutPdf("PersonId").ToString, drGenerateCashoutPdf("LetterCode").ToString)
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Loop to create PDF files from RPT")
                    Next
                    'dvMsg.InnerHtml = "No.of PDF Generated sucessfully: " + ProcessCount.ToString + " Out of: " + Convert.ToString(dtGeneratePdf.Rows.Count)
                    ''dvPDFProgress.InnerText = ProcessCount.ToString + " of " + Convert.ToString(dtGeneratePdf.Rows.Count) + " Processed."

                    'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                    'If ProcessCount > dtGeneratePdf.Rows.Count Then
                    '    objRpt.Close()
                    '    objRpt.Dispose()
                    '    objRpt = Nothing
                    '    GC.Collect()
                    '    Session("rptObject") = Nothing
                    'End If
                    'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                End If
            End If
        Catch ex As Exception
            ' dvMsg.InnerHtml = "No.of PDF Generated sucessfully: " + ProcessCount.ToString + " Out of: " + Convert.ToString(dtGeneratePdf.Rows.Count)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Exception raise while generating pdf files")
            ArrErrorData.Add(New ExceptionLog(l_str_FundNo.ToString(), "Error While Writing PDF File", ex.Message))
            HelperFunctions.LogException("InvokePDFGeneration", ex)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Exception raise while generating pdf files")
            Throw
        Finally
            'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Start: Destroy RPT object")
            objRpt.Close()
            objRpt.Dispose()
            objRpt = Nothing
            GC.Collect()
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("InvokePDFGeneration Method", "Finish: Destroy RPT object")
            'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        End Try
    End Function

    'This function will update the IDM tracking records which created earlier and will traking and updates every stages of batch request generations.
    Public Sub FilesTracking(ByVal pTrackingId As Integer, ByVal pIdxFileCreated As Boolean, ByVal pPdfFilecreated As Boolean, ByVal pIdxFileName As String, ByVal pPdfFileName As String, ByVal pIdxFileinitiated As Boolean, ByVal pIdxfileCopied As Boolean, ByVal pIdxFileDeleted As Boolean, ByVal pPdfFileinitiated As Boolean, ByVal pPdffileCopied As Boolean, ByVal pPdfFileDeleted As Boolean, ByVal pFileType As Char, ByVal pErrorMsg As String)
        Try
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateYMCAOutputFileIDMTrackingLogs(pTrackingId, pIdxFileCreated, pPdfFilecreated, pIdxFileName, pIdxFileinitiated, pIdxfileCopied, pIdxFileDeleted, pPdfFileName, pPdfFileinitiated, pPdffileCopied, pPdfFileDeleted, pFileType, pErrorMsg)
        Catch
            Throw
        End Try
    End Sub

    'This function will open printreport window to print genrated PDF in crystal report
    Private Sub PrintReport()
        Dim popupScript As String
        popupScript = "<script language='javascript'>openReportPrinter()</" + "script>"
        If (Not Me.IsStartupScriptRegistered("PopupPrintColor")) Then
            Page.RegisterStartupScript("PopupPrintColor", popupScript)
        End If
    End Sub

    'This function will get all participant information which required for batch request creation.
    Private Sub GetPersonName(ByVal parameterDataRow As DataRow)
        Dim l_firstname As String = ""
        Dim l_middlename As String = ""
        Dim l_lastname As String = ""
        Try
            If parameterDataRow("LastName").[GetType]().ToString() <> "System.DBNull" Then
                l_lastname = Convert.ToString(parameterDataRow("LastName"))
            Else
                l_lastname = String.Empty
            End If
            If parameterDataRow("FirstName").[GetType]().ToString() <> "System.DBNull" Then
                l_firstname = Convert.ToString(parameterDataRow("FirstName"))
            Else
                l_firstname = String.Empty
            End If
            If parameterDataRow("MiddleName").[GetType]().ToString() <> "System.DBNull" Then
                l_middlename = Convert.ToString(parameterDataRow("MiddleName"))
            Else
                l_middlename = String.Empty
            End If
            Me.m_string_GetPersonName = ((Convert.ToString(parameterDataRow("LastName"))) + " " + (Convert.ToString(parameterDataRow("MiddleName"))) & " ") + (Convert.ToString(parameterDataRow("FirstName")))
        Catch
            Throw
        End Try
    End Sub

    'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report
    Private Sub InvokeProcess(ByVal iProcessCount As Integer, ByVal dtProcesstable As DataTable, ByRef bAllFilesCreated As Boolean, ByVal ArrErrorDataList As List(Of ExceptionLog), iIDXCreated As Integer, iPDFCreated As Integer, strBatchId As String, strModuleType As String, dsPrintletters As DataSet)
        Dim iIDXCount As Integer = 0
        Dim iPDFCount As Integer = 0
        Try

            AddErrorException(dsPrintletters, ArrErrorDataList, strBatchId, strModuleType, dtProcesstable)

            iIDXCount = dsPrintletters.Tables("SelectedBatchRecords").Select("IDXCount = 1").Count
            iPDFCount = dsPrintletters.Tables("SelectedBatchRecords").Select("PDFCount = 1").Count

            If iProcessCount < dsPrintletters.Tables("SelectedBatchRecords").Rows.Count Then

                If iIDXCount > 0 Then
                    dvIDXProgress.InnerText = iIDXCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                Else
                    dvIDXProgress.InnerText = iIDXCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                    dvIDX.Style.Add("display", "block")
                End If

                If iPDFCount > 0 Then
                    dvPDFProgress.InnerText = iPDFCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                Else
                    dvPDFProgress.InnerText = iPDFCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                    dvPDF.Style.Add("display", "block")
                End If

                bAllFilesCreated = False

                If Not Request.QueryString("Form") Is Nothing Then
                    If Convert.ToString(Request.QueryString("Form")).ToUpper = "RMD" OrElse Convert.ToString(Request.QueryString("Form")).ToUpper = "ROLLIN" Then 'AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for refresh the page for rollin reminder letter
                        Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                                       "windowrefreshPage(" + iProcessCount.ToString + ",'" + Convert.ToString(Request.QueryString("Form")).ToUpper + "','" + strBatchId + "','" + strModuleType + "')</" & "script" & ">"

                        If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                            Page.RegisterStartupScript("PopupScript2", popupScript3)
                        End If
                    End If
                End If
            Else
                If iIDXCount > 0 Then
                    imgIDXComplete.CssClass = "show"
                    dvIDX.Style.Add("display", "none")
                    dvIDXProgress.InnerText = iIDXCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                Else
                    imgIDXComplete.CssClass = "hide"
                    dvIDX.Style.Add("display", "none")
                    dvIDXProgress.InnerText = iIDXCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                End If
                lblMsg.Visible = True
                imgRegComplete.CssClass = "show"
                dvReg.Style.Add("display", "none")

                If iPDFCount > 0 Then
                    imgPDFComplete.CssClass = "show"
                    dvPDF.Style.Add("display", "none")
                    dvPDFProgress.InnerText = iPDFCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                Else
                    imgPDFComplete.CssClass = "hide"
                    dvPDF.Style.Add("display", "none")
                    dvPDFProgress.InnerText = iPDFCount.ToString + " of " + dsPrintletters.Tables(0).Rows.Count.ToString() + " Processed."
                End If
                lblMsg.Text = "Print Letters process completed, No. of letters generated: " + iProcessCount.ToString()
                bAllFilesCreated = True
                Session("AllFilesCreated") = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AddErrorException(ByRef dsPrintletters As DataSet, ByRef ArrErrorDataList As List(Of ExceptionLog), strBatchId As String, StrModuleType As String, dtProcesstable As DataTable)
        If dsPrintletters.Tables.Contains("ArrErrorDataList") Then
            Dim dr As DataRow
            If Not ArrErrorDataList Is Nothing Then
                If ArrErrorDataList.Count > 0 Then
                    For Each exlog As ExceptionLog In ArrErrorDataList
                        dr = dsPrintletters.Tables("ArrErrorDataList").NewRow()
                        dr("FundNo") = exlog.FundNo
                        dr("Errors") = exlog.Errors
                        dr("Description") = exlog.Decription
                        dsPrintletters.Tables("ArrErrorDataList").Rows.Add(dr)
                    Next
                    lblException.Visible = True
                End If
            End If
        Else
            Dim dtArrErrorDataList As New DataTable("ArrErrorDataList")
            dtArrErrorDataList.Columns.Add("FundNo")
            dtArrErrorDataList.Columns.Add("Errors")
            dtArrErrorDataList.Columns.Add("Description")
            Dim dr As DataRow
            If Not ArrErrorDataList Is Nothing Then
                If ArrErrorDataList.Count > 0 Then
                    lblException.Visible = True
                    For Each exlog As ExceptionLog In ArrErrorDataList
                        dr = dtArrErrorDataList.NewRow()
                        dr("FundNo") = exlog.FundNo
                        dr("Errors") = exlog.Errors
                        dr("Description") = exlog.Decription
                        dtArrErrorDataList.Rows.Add(dr)
                    Next
                End If
            End If
            If dsPrintletters.Tables.Contains("ArrErrorDataList") Then
                dsPrintletters.Tables.Remove("ArrErrorDataList")
            End If
            dtArrErrorDataList.TableName = "ArrErrorDataList"
            dsPrintletters.Tables.Add(dtArrErrorDataList)
        End If
        If ArrErrorDataList IsNot Nothing AndAlso ArrErrorDataList.Count > 0 Then
            lblException.Visible = True
            gvBatchRequestError.DataSource = ArrErrorDataList
            gvBatchRequestError.DataBind()
        End If

        If dsPrintletters.Tables.Contains("SelectedBatchRecords") Then
            'dsPrintletters.Tables.Remove("SelectedBatchRecords")
            ' dsPrintletters.Tables("SelectedBatchRecords").Merge(dtProcesstable)
            For Each drProcess As DataRow In dtProcesstable.Rows
                For Each drPrintLetters As DataRow In dsPrintletters.Tables("SelectedBatchRecords").Rows
                    If StrModuleType = BatchProcess.RMDRollins.ToString Then
                        If drProcess("RefRequestID") = drPrintLetters("RefRequestID") Then
                            drPrintLetters("IDXCount") = drProcess("IDXCount")
                            drPrintLetters("PDFCount") = drProcess("PDFCount")
                        End If
                    Else
                        If drProcess("FUNDNo") = drPrintLetters("FUNDNo") Then
                            drPrintLetters("IDXCount") = drProcess("IDXCount")
                            drPrintLetters("PDFCount") = drProcess("PDFCount")
                        End If
                    End If
                Next
            Next
        End If
        'dtProcesstable.TableName = "SelectedBatchRecords"
        'dsPrintletters.Tables.Add(dtProcesstable)
        YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, StrModuleType, dsPrintletters)
    End Sub

    'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added for use multiple params while calling report
    'Start:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added to Print the rollin letters
    Private Function PrintRollInProcess(ByVal strBatchId As String, strModuleType As String)
        Dim dsPrintLetters As DataSet
        Dim dtPrintLetters As DataTable
        Dim dtTempPrintLetters As DataTable
        Dim drTempPrintLetters As DataRow
        Dim arrErrorDataList As List(Of ExceptionLog) = Nothing
        Dim strDocType As String = String.Empty
        Dim strReportName As String = String.Empty
        Dim strParam1 As String = String.Empty
        Dim strOutputFileType As String = String.Empty
        Dim iIDXCreated As Integer = 0
        Dim iPDfCreated As Integer = 0
        Dim ProcessCount As Integer = 0
        Dim iCount As Integer = 0
        Const ciBatchCount As Integer = 10
        Dim dtFileList As DataTable
        Dim blnAllFilesCreated As Boolean = False
        Dim popupScript As String
        Try
            tdProcessCreation.InnerText = "1. Print Letters Creation"
            dsPrintLetters = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModuleType)
            If Not dsPrintLetters Is Nothing Then
                dtTempPrintLetters = dsPrintLetters.Tables("SelectedBatchRecords").Clone
                strDocType = "ROLINLSR"
                strReportName = "Letter of Acceptance.rpt"
                strParam1 = "RollIn"
                strOutputFileType = "RollIn_ROLINLSR"
            End If

            If Not dsPrintLetters.Tables("SelectedBatchRecords").Columns.Contains("IDXCount") Then
                dsPrintLetters.Tables("SelectedBatchRecords").Columns.Add("IDXCount")
            End If

            If Not dsPrintLetters.Tables("SelectedBatchRecords").Columns.Contains("PDFCount") Then
                dsPrintLetters.Tables("SelectedBatchRecords").Columns.Add("PDFCount")
            End If

            dtPrintLetters = dsPrintLetters.Tables("SelectedBatchRecords")

            Dim dtArrErrorDataList As DataTable = dsPrintLetters.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                arrErrorDataList = New List(Of ExceptionLog)
                For Each dr As DataRow In dtArrErrorDataList.Rows
                    arrErrorDataList.Add(New ExceptionLog(dr("FundNo"), dr("Errors"), dr("Description")))
                Next
            Else
                arrErrorDataList = New List(Of ExceptionLog)
            End If

            If Not arrErrorDataList Is Nothing Then
                gvBatchRequestError.DataSource = arrErrorDataList
                gvBatchRequestError.DataBind()
            End If

            If (HelperFunctions.isEmpty(dtPrintLetters)) Then
                arrErrorDataList.Add(New ExceptionLog("", "Session Expired", "No records found for print letters process."))
                Exit Function
            End If

            If Not Request Is Nothing Then
                'Query string is used as index no in datatable for batch request creation.
                If Not String.IsNullOrEmpty(Request.QueryString("count")) Then
                    iCount = Convert.ToInt32(Request.QueryString("count"))
                End If
            End If



            dtTempPrintLetters = dtPrintLetters.Clone
            For ProcessCount = iCount To dtPrintLetters.Rows.Count - 1
                'number of records to be taken for batch creation process this count can be configurable.
                If ProcessCount - iCount >= ciBatchCount Then
                    Exit For
                End If

                YMCARET.YmcaBusinessObject.RollInReminderBOClass.InsertPrintLetters(dtPrintLetters.Rows(ProcessCount)("RefRequestID"))
                'storing set of records in temp table so that this much of records can be process for batch creation.
                drTempPrintLetters = dtTempPrintLetters.NewRow()
                drTempPrintLetters("PersonId") = dtPrintLetters.Rows(ProcessCount)("PersonId").ToString
                drTempPrintLetters("SSNo") = dtPrintLetters.Rows(ProcessCount)("SSNo").ToString
                drTempPrintLetters("FUNDNo") = dtPrintLetters.Rows(ProcessCount)("FUNDNo").ToString
                drTempPrintLetters("FirstName") = dtPrintLetters.Rows(ProcessCount)("FirstName").ToString
                drTempPrintLetters("LastName") = dtPrintLetters.Rows(ProcessCount)("LastName").ToString
                drTempPrintLetters("MiddleName") = dtPrintLetters.Rows(ProcessCount)("MiddleName").ToString
                drTempPrintLetters("RefRequestID") = dtPrintLetters.Rows(ProcessCount)("RefRequestID").ToString
                'Start:AA:17.07.2014 BT:1051 - YRS 5.0:1618 Added for missing parameters to report
                drTempPrintLetters("PartAccno") = dtPrintLetters.Rows(ProcessCount)("PartAccno").ToString
                drTempPrintLetters("InstitutionName") = dtPrintLetters.Rows(ProcessCount)("InstitutionName").ToString
                drTempPrintLetters("addr1") = dtPrintLetters.Rows(ProcessCount)("addr1").ToString
                drTempPrintLetters("addr2") = dtPrintLetters.Rows(ProcessCount)("addr2").ToString
                drTempPrintLetters("addr3") = dtPrintLetters.Rows(ProcessCount)("addr3").ToString
                drTempPrintLetters("city") = dtPrintLetters.Rows(ProcessCount)("city").ToString
                drTempPrintLetters("StateName") = dtPrintLetters.Rows(ProcessCount)("StateName").ToString
                drTempPrintLetters("zipCode") = dtPrintLetters.Rows(ProcessCount)("zipCode").ToString
                'End:AA:17.07.2014 BT:1051 - YRS 5.0:1618 Added for missing parameters to report
                drTempPrintLetters("IDXCount") = 0
                drTempPrintLetters("PDFCount") = 0
                dtTempPrintLetters.Rows.Add(drTempPrintLetters)
            Next

            dvReqProgress.InnerText = ProcessCount.ToString() + " of " + dtPrintLetters.Rows.Count.ToString() + " Processed."

            InvokeBatchRequestCreation(0, dtTempPrintLetters, strDocType, strReportName, strOutputFileType, strParam1, arrErrorDataList, dtFileList, iIDXCreated, iPDfCreated, True, True, True, True, True)

            If HelperFunctions.isNonEmpty(dtFileList) Then
                If Not dsPrintLetters.Tables.Contains("dtFileList") Then
                    dtFileList.TableName = "dtFileList"
                    dsPrintLetters.Tables.Add(dtFileList)
                End If
                dsPrintLetters.Tables("dtFileList").Merge(dtFileList)
            End If

            InvokeProcess(ProcessCount, dtTempPrintLetters, blnAllFilesCreated, arrErrorDataList, iIDXCreated, iPDfCreated, strBatchId, strModuleType, dsPrintLetters)

            If Not dsPrintLetters.Tables("dtFileList") Is Nothing Then
                Session("FTFileList") = dsPrintLetters.Tables("dtFileList")
            End If

            If Not Session("FTFileList") Is Nothing And blnAllFilesCreated Then

                imgRegComplete.CssClass = "show"
                dvReg.Style.Add("display", "none")

                imgCopiedComplete.CssClass = "show"
                dvCopy.Style.Add("display", "none")

                popupScript = " newwindow1 = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                                            "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');" +
                                            " if (window.focus) {newwindow1.focus()}" +
                                            " if (!newwindow1.closed) {newwindow1.focus()}"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenCopy", popupScript, True)


                dvCopiedProgress.InnerText = ProcessCount.ToString() + " of " + dtPrintLetters.Rows.Count.ToString() + " Processed."

                'AA:11.07.2014-BT:1051: Added code to call the function for Merging all the pdf copied in source folder
                MergePDFs(dsPrintLetters.Tables("dtFileList"))

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'End:AA:03.06.2014 BT:1051 - YRS 5.0:1618 Added to Print the rollin letters

    'Start:AA:11.07.2014    BT:1051: Added code to Merge all the pdf copied in source folder 
    ''' <summary>
    ''' Put your own code here to get the files to be concatenated.
    ''' </summary>
    Private Function GetFiles(ByVal dtFileList As DataTable) As String()

        Dim drFileList As DataRow()
        Dim list As New ArrayList()
        Try
            drFileList = dtFileList.Select("SourceFolder LIKE '%PDF%'")
            For iCount As Integer = 0 To drFileList.Length - 1
                Dim info As FileInfo
                Dim ds As New DirectoryInfo(drFileList(iCount)("SourceFolder").ToString())
                info = ds.GetFiles(drFileList(iCount)("SourceFile").ToString().Replace(drFileList(iCount)("SourceFolder").ToString() + "\", ""))(0)
                ' HACK: Just skip the protected samples file...
                If info.Name.IndexOf("protected") = -1 Then
                    list.Add(info.FullName)
                End If
            Next
            Return DirectCast(list.ToArray(GetType(String)), String())
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Imports all pages from a list of documents.
    ''' </summary>
    Public Sub MergePDFs(ByVal dtFileList As DataTable)
        Dim strFiles As String()
        Dim strFileName As String
        Dim strActualFileName As String
        Dim strDestPath As String
        Dim pdfPage As PdfPage
        Dim iCount As Integer
        Dim inputDocument As PdfDocument
        Dim strFile As String
        Dim pCount As Integer
        Try
            If HelperFunctions.isNonEmpty(dtFileList) Then


                ' Get some file names
                strFiles = GetFiles(dtFileList)
                ' Open the output document
                Dim pdocOutputDocument As New PdfDocument()
                ' Iterate files

                For Each strFile In strFiles
                    ' Open the document to import pages from it.
                    inputDocument = PdfReader.Open(strFile, PdfDocumentOpenMode.Import)
                    ' Iterate pages
                    pCount = inputDocument.PageCount
                    For iCount = 0 To pCount - 1
                        ' Get the page from the external document...
                        pdfPage = inputDocument.Pages(iCount)
                        ' ...and add it to the output document.
                        pdocOutputDocument.AddPage(pdfPage)
                    Next
                Next

                ' Save the document...
                'AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
                strDestPath = HttpContext.Current.Server.MapPath("~\" + System.Configuration.ConfigurationSettings.AppSettings("MergePDFPath")) + "\"

                If Not Directory.Exists(strDestPath) Then
                    Directory.CreateDirectory(strDestPath)
                End If

                strActualFileName = "ConsolidatedDocument_" + Now.Ticks.ToString + ".pdf"
                strFileName = strDestPath + strActualFileName

                pdocOutputDocument.Save(strFileName)

                '...and coply in the session to open pdf in the pop-up            
                Session("Rollin_MergedPdfs_Filename") = strActualFileName
            Else
                Throw New Exception("Files source does not exists.")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'START : CS | 2016.10.24 | YRS-AT-3088 | Merge PDF(s) based on the letter code 
    Public Sub MergePDFs(ByVal FileList As DataTable, ByVal FileName As String, ByVal BatchID As String)
        Dim Files As String()
        Dim tempfilename As String
        Dim destPathAndFileName As String
        Dim actualfilename As String
        Dim destPath As String
        Dim pdfPage As PdfPage
        Dim iCount As Integer
        Dim inputDocument As PdfDocument
        Dim pCount As Integer
        Try
            If HelperFunctions.isNonEmpty(FileList) Then
                ' Get some file names
                Files = GetFiles(FileList)
                ' Open the output document
                Dim pdocOutputDocument As New PdfDocument()
                ' Iterate files
                For Each tempfilename In Files
                    ' Open the document to import pages from it.
                    inputDocument = PdfReader.Open(tempfilename, PdfDocumentOpenMode.Import)
                    ' Iterate pages
                    pCount = inputDocument.PageCount
                    For iCount = 0 To pCount - 1
                        ' Get the page from the external document...
                        pdfPage = inputDocument.Pages(iCount)
                        ' ...and add it to the output document.
                        pdocOutputDocument.AddPage(pdfPage)
                    Next
                Next
                ' Save the document...
                'AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
                destPath = HttpContext.Current.Server.MapPath(String.Format("~\{0}\", System.Configuration.ConfigurationSettings.AppSettings("MergedBatchDocumentPath")))
                If Not Directory.Exists(destPath) Then
                    Directory.CreateDirectory(destPath)
                End If
                actualfilename = String.Format("{0}_{1}.pdf", FileName, BatchID)
                destPathAndFileName = destPath + actualfilename

                pdocOutputDocument.Save(destPathAndFileName)
                '...and coply in the session to open pdf in the pop-up            
                Session("MergedPdf_Filename") = actualfilename
            Else
                Throw New Exception("Files source does not exists.")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'END : CS | 2016.10.24 | YRS-AT-3088 | Merge PDF(s) based on the letter code 
    'End:AA:11.07.2014    BT:1051: Added code to Merge all the pdf copied in source folder 

#End Region

    Private Function CreateFileListSchema() As DataTable
        'Adding these columns for file list for FT copying.
        Dim dt As New DataTable("FILELIST")
        dt.Columns.Add("FundNo")
        dt.Columns.Add("DestFolder")
        dt.Columns.Add("DestFile")
        dt.Columns.Add("SourceFile")
        dt.Columns.Add("SourceFolder")
        Return dt
    End Function

End Class


Public Class ExceptionLog
    Private l_strFundNo As String
    Private l_strError As String
    Private l_strDescription As String
    Public Property FundNo() As String
        Get
            Return l_strFundNo
        End Get
        Set(ByVal Value As String)
            l_strFundNo = Value
        End Set
    End Property

    Public Property Errors() As String
        Get
            Return l_strError
        End Get
        Set(ByVal Value As String)
            l_strError = Value
        End Set
    End Property
    Public Property Decription() As String
        Get
            Return l_strDescription
        End Get
        Set(ByVal Value As String)
            l_strDescription = Value
        End Set
    End Property

    Public Sub New(ByVal strFundNo As String, ByVal strErrors As String, ByVal strDescription As String)
        FundNo = strFundNo
        Errors = strErrors
        Decription = strDescription
    End Sub

End Class