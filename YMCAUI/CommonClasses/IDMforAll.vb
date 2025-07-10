'*******************************************************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	IDMforAll.vb
' Author Name		:	Ashutosh Patil
' Employee ID		:	36307
' Email				:	ashutosh.patil@3i-infotech.com
' Extn      		:	8568
' Creation Date		:	12-Feb-2007
' Purpose           :   This class will set the properties for all reports and depending upon then
'                       the properties the corresponding letters(reports) will be generated.
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Desription
'********************************************************************************************************************************
'Ashutosh Patil     02-Apr-2007     YREN-3195 
'Ashutosh Patil     10-Apr-2007     YREN-3195 
'Aparna Samala      17-Apr-2007     YREN-3197
'Mohammed Hafiz     8-May-2007      YREN-3334 for refreshing the report, to generate pdf for the passed parameter even if it is hardcoded in the report.
'Sanjay Rawat       8-June-2009     To Keep track of all IDX and PDF file generated.
'Imran              02-Sep-2010     Check validation of data before creation of Idx file in CreatePersIdxForTower() method
'Sanjay S.			2011.04.15	    BT-747: Updating code to handle issue where the field going into the database was defined as int16 while it should have been int32.
'Sanjeev Gupta(SG)  2012.09.05      BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
'B. Jagadeesh       2015.04.28      BT-2570: YRS 5.0-2380:Added for retreiving the tracking id to store into Print Letters
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2015.10.09      YRS-AT-2614:  YRS: files for IDM - .idx filename needs to match .pdf filename 
'Manthan Rajguru    2016.05.05      YRS-AT-2909 -  Support Request: printing crystal report com exception LoanDefaultLetter.rpt
'Anudeep A          2016.07.05      YRS-AT-2985 - YRS bug: (hotfix) Withdrawal EForm DOCCODE change for signed
'Anudeep A          2016.27.06      BT-1156:Add a precautionary measure while IDM creation
'Chandra sekar      2016.07.12      YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
'********************************************************************************************************************************
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
Imports Microsoft.Reporting.WebForms
Imports Microsoft.ReportingServices
'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
Public Class IDMforAll

#Region " Variable Declaration "
    Inherits System.Web.UI.Page
    Dim m_bool_PrviewRpt As Boolean
    Dim m_bool_CreatePdf As Boolean
    Dim m_bool_Createidx As Boolean
    Dim m_bool_CopyFilesToIDM As Boolean
    Dim m_bool_logondb As Boolean
    Dim m_str_ReportName As String
    Dim m_str_ReportHeader As String
    Dim m_str_PDFFileName As String
    Dim m_str_IDXFileName As String
    Dim m_str_YMCAId As String
    Dim m_str_PersId As String
    Dim m_str_DocType As String
    Dim m_arr_ReportParams As ArrayList
    Dim m_dtFileList As New DataTable
    Dim m_dtReportTracking As New DataTable
    Dim m_str_VignettePath As String
    Dim m_str_letter_YmcaPart As String
    Dim m_str_app_type As String
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

    'Dim m_dtFileList As DataTable
    Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim objCashOutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass

    Dim stTimeStampForFile As String 'Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Local Variable to hold time stamp value which will be accessed by pdf and idx file name properties
    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Dim objRDLCReportDataSource As New ReportDataSource
    Dim objRDLCReportViewer As New Microsoft.Reporting.WebForms.ReportViewer
    Dim dicRDLCReportParameters As Dictionary(Of String, String)
    Dim objRDLCReportParameter As New Generic.List(Of ReportParameter)
    Dim strReportType As String
    Dim dtReportData As New DataTable
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
#End Region 'Variable Declaration 
#Region " Properties "
    Public Sub New()
        m_bool_PrviewRpt = False
        m_bool_CreatePdf = False
        m_bool_Createidx = False
        m_bool_CopyFilesToIDM = False
        m_bool_logondb = False
        m_str_ReportName = Nothing
        m_str_PDFFileName = Nothing
        m_str_IDXFileName = Nothing
        m_arr_ReportParams = Nothing
        m_str_DocType = Nothing
        m_str_ReportHeader = Nothing
        m_str_YMCAId = Nothing
        m_str_app_type = Nothing

        stTimeStampForFile = String.Empty 'Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Assigning intitial value 
    End Sub
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
                '-- Start: Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_PDFFileName = l_str_YMCANo & "_" & l_str_DocType & "_" & Format(Now, "ddMMMyyyy_HHmmss")
                m_str_PDFFileName = String.Format("{0}_{1}_{2}", l_str_YMCANo, l_str_DocType, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.09 | YRS-AT 2614 | Timestamp is called through variable assigned in method
                'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            ElseIf Me.DocTypeCode = YMCAObjects.IDMDocumentCodes.Waived_Participant_List Then
                m_str_PDFFileName = String.Format("{0}_{1}", l_str_OutPutFileType, stTimeStampForFile)
                'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            Else
                If m_str_letter_YmcaPart = "A" Then
                    l_pdf_Filename = Me.OutputFileType
                    l_pdf_FileIndex = "_2"
                Else
                    l_pdf_Filename = Me.OutputFileType
                    l_pdf_FileIndex = "_1"
                End If
                '-- Start: Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_PDFFileName = "" & l_pdf_Filename & l_pdf_FileIndex & "_" & Format(Now, "ddMMMyyyy_HHmmss")
                m_str_PDFFileName = String.Format("{0}{1}_{2}", l_pdf_Filename, l_pdf_FileIndex, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.09 | YRS-AT 2614 | Timestamp is called through variable assigned in method
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
                '-- Start: Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_IDXFileName = l_str_YMCANo & "_" & l_str_DocType & "_" & Format(Now, "ddMMMyyyy_HHmmss")           
                m_str_IDXFileName = String.Format("{0}_{1}_{2}", l_str_YMCANo, l_str_DocType, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.09 | YRS-AT 2614 | Timestamp is called through variable assigned in method
                'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            ElseIf Me.DocTypeCode = YMCAObjects.IDMDocumentCodes.Waived_Participant_List Then
                m_str_IDXFileName = String.Format("{0}_{1}", l_str_OutPutFileType, stTimeStampForFile)
                'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
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
                '-- Start: Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Timestamp is called through variable assigned in method
                'm_str_IDXFileName = "" & l_idx_Filename & l_idx_FileIndex & "_" & Format(Now, "ddMMMyyyy_HHmmss")
                m_str_IDXFileName = String.Format("{0}{1}_{2}", l_idx_Filename, l_idx_FileIndex, stTimeStampForFile)
                '-- End: Manthan Rajguru | 2015.10.09 | YRS-AT 2614 | Timestamp is called through variable assigned in method
            End If

            Return m_str_IDXFileName

        End Get
        Set(ByVal Value As String)
            m_str_IDXFileName = Value
        End Set
    End Property

    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Public Property ReportType() As String
        Get
            If Not String.IsNullOrEmpty(strReportType) Then
                Return strReportType.ToString()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            strReportType = Value
        End Set
    End Property

    Public Property RDLCReportParameters As Dictionary(Of String, String)
        Get
            If Not dicRDLCReportParameters Is Nothing Then
                Return dicRDLCReportParameters
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal value As Dictionary(Of String, String))
            dicRDLCReportParameters = value
        End Set
    End Property
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

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
    Public Property LogonToDb() As Boolean
        Get
            Return m_bool_logondb
        End Get
        Set(ByVal Value As Boolean)
            m_bool_logondb = Value
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

    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Public Property ReportDataTable() As DataTable
        Get
            If Not dtReportData Is Nothing Then
                Return (CType(dtReportData, DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            dtReportData = Value
        End Set
    End Property
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

    'Added By SG: 2012.09.05: BT-960
    Public Property RefRequestsID() As String
        Get
            Return l_RefRequestsID
        End Get
        Set(ByVal value As String)
            l_RefRequestsID = value
        End Set
    End Property
    'Start:JB:17-04-2015 BT:2570- YRS 5.0-2380- Added for retreiving the tracking id to store into Print Letters
    Public ReadOnly Property IDMTrackingId() As Integer
        Get
            Return l_str_rptTrackingId
        End Get
    End Property
    'End:JB:17-04-2015 BT:2570- YRS 5.0-2380- Added for retreiving the tracking id to store into Print Letters

#End Region 'Properties 
#Region " Functions "
    Public Function GetDestinationPath(ByVal p_boolParticipant As Boolean) As String
        Dim l_string_ServerName As String
        Dim l_string_FilePath As String
        Try
            l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
            l_string_FilePath = YMCARET.YmcaBusinessObject.RefundRequest.GetVignettePath(l_string_ServerName)
            'Start:AA:06.27.2016 BT-1156 Add a precautionary measure while IDM creation 
            'If the destination path was not set in the serverlookup then the exception wil be raised
            If String.IsNullOrEmpty(l_string_FilePath) Then
                Dim ex As New Exception("IDM Destination path is not configured in serverlookup table. Please contact database administrator.")
                HelperFunctions.LogException("IDM_GetDestinationPath", ex)
                Throw ex
            End If
            'End:AA:06.27.2016 BT-1156 Add a precautionary measure while IDM creation 
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
    'by Aparna -YREN-3197 16/04/2007
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
    'by Aparna -YREN-3197 16/04/2007
    Public Function DatatableFileList(ByVal l_bool_Participant As Boolean) As Boolean
        Try
            m_dtFileList.Columns.Add("SourceFolder", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("SourceFile", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("DestFolder", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("DestFile", System.Type.GetType("System.String"))
            DatatableFileList = True
        Catch ex As Exception
            DatatableFileList = False
        End Try
    End Function
    Public Function ExportToPDF() As String

        '******************************************************************************************************
        ' Author Name		   : Ashutosh Patil
        ' Employee ID		   : 36307
        ' Extn                 : 8568
        ' Email				   : ashutosh.patil@3i-infotech.com
        ' Creation Time		   : 13-Feb-2007
        ' Description		   : This function will bifurgate for Letter to YMCA and Letter To Participant
        ' Modified By          : 
        ' Modified On          : 
        ' Reason for Change    : 
        '*******************************************************************************************************

        Dim l_DataTable_IdxDetails As DataTable
        Dim l_StringStaffFL As String
        Dim l_StringSLevel As String
        Dim l_StringTabCode As String
        Dim l_StringTabLetter As String
        Dim l_StringApp As String
        Dim l_StrngDoccode As String
        Dim l_StringDocTab As String
        Dim l_StringWfstatus As String
        Dim l_StringDocType As String
        Dim l_String_App_Type As String
        Dim l_OutputFileType As String
        Dim l_String_DocType As String
        Dim l_StringDocFileName As String
        Dim l_StringIndexFileName As String
        Dim l_StringReportName As String
        Dim l_StringDestFilePath As String
        'By Aparna YREN-3197 16/04/2007
        Dim l_stringSourceFilePath As String
        Dim l_stringPDFFilePath As String
        Dim l_stringIndexFilePath As String

        Dim l_BoolRetFlag As Boolean
        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_dataRow As DataRow

        Dim l_stringYMACA As String
        Dim l_stringYMACAName As String
        Dim l_stringYMACACity As String
        Dim l_stringYMACAState As String
        Dim l_Dataset_PartYMCA As DataSet
        Dim l_Dataset_ParticipantInfo As DataSet
        Dim l_DatatablePartYMCA As DataTable
        Dim l_DataRowPartYMCA As DataRow
        Dim l_str_Firstname As String
        Dim l_str_Middlename As String
        Dim l_str_Lastname As String
        Dim l_str_SSNO As String
        Dim l_str_FundNo As String
        Dim l_str_Report_Mode As String

        ''Added by sanjay
        Dim drAdd As DataRow
        Dim l_ds_RptTracking As New DataSet



        Try
            stTimeStampForFile = Format(Now, "ddMMMyyyy_HHmmss") 'Manthan Rajguru | 2015.10.09 | YRS-AT-2614 | Timestamp value assigned to variable

            l_String_App_Type = Me.AppType
            Select Case l_String_App_Type
                Case "A"
                    'by Aparna YREN-3197 16/04/2007
                    l_stringSourceFilePath = GetSourceFilePath(False)
                    '  m_str_VignettePath = GetVignettePath(False)
                    l_StringDestFilePath = GetDestinationPath(False)
                    'by Aparna YREN-3197 16/04/2007
                    If Not (Session("PersonInformation") Is Nothing) Then
                        l_DataSet = CType(Session("PersonInformation"), DataSet)
                        l_DataTable = l_DataSet.Tables("Member Details")
                        If l_DataTable.Rows.Count > 0 Then
                            l_dataRow = l_DataTable.Rows(0)
                        End If
                    End If
                    l_stringYMACA = ""
                    l_stringYMACAName = ""
                    l_stringYMACACity = ""
                    l_stringYMACAState = ""
                    l_Dataset_PartYMCA = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetYMCAInformation(Me.YMCAID)
                    If Not l_Dataset_PartYMCA Is Nothing Then
                        l_DatatablePartYMCA = l_Dataset_PartYMCA.Tables(0)
                        If l_DatatablePartYMCA.Rows.Count > 0 Then
                            l_DataRowPartYMCA = l_DatatablePartYMCA.Rows(0)
                            l_stringYMACA = l_DataRowPartYMCA("YMCANO").ToString()
                            l_stringYMACAName = l_DataRowPartYMCA("YMCAName").ToString()
                            l_stringYMACACity = l_DataRowPartYMCA("YMCACity").ToString()
                            l_stringYMACAState = l_DataRowPartYMCA("YCMAState").ToString()
                        End If
                    End If
                Case "P"
                    'by Aparna YREN-3197 16/04/2007
                    l_stringSourceFilePath = GetSourceFilePath(True)
                    '  m_str_VignettePath = GetVignettePath(False)
                    l_StringDestFilePath = GetDestinationPath(True)
                    'by Aparna YREN-3197 16/04/2007
                    'Added By Ashutosh Patil as on 18-Apr-2007
                    'For reports related to Participant
                    l_str_Firstname = ""
                    l_str_Middlename = ""
                    l_str_Lastname = ""
                    l_str_FundNo = ""
                    l_str_SSNO = ""
                    l_Dataset_PartYMCA = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(Me.PersId)
                    If Not l_Dataset_PartYMCA Is Nothing Then
                        l_DatatablePartYMCA = l_Dataset_PartYMCA.Tables(0)
                        If l_DatatablePartYMCA.Rows.Count > 0 Then
                            l_DataRowPartYMCA = l_DatatablePartYMCA.Rows(0)
                            l_str_Firstname = l_DataRowPartYMCA("FirstName").ToString()
                            l_str_Middlename = l_DataRowPartYMCA("MiddleName").ToString()
                            l_str_Lastname = l_DataRowPartYMCA("LastName").ToString()
                            l_str_FundNo = l_DataRowPartYMCA("FundNo").ToString()
                            l_str_SSNO = l_DataRowPartYMCA("SSNo").ToString()
                        End If
                    End If
            End Select
            If l_String_App_Type <> "" Then
                l_OutputFileType = Me.OutputFileType
                If l_stringSourceFilePath.Trim = "" Then
                    ExportToPDF = CType(Session("IndexFileWriteError"), String)
                    Exit Function
                End If
                'by Aparna -YREN-3197  16/04/2007 -Not Necessary
                m_str_letter_YmcaPart = l_String_App_Type
                l_StringReportName = Me.ReportName.ToString.Trim
                l_String_DocType = Me.DocTypeCode.ToString.Trim
                '****** Getting the details related to idx file creations only if they are 
                If Me.CreateIDX = True Then
                    If l_String_App_Type = "A" Then
                        l_DataTable_IdxDetails = YMCARET.YmcaBusinessObject.RefundRequest.GetDetailsForIdxCreationForYmca(l_String_DocType)
                    Else
                        l_DataTable_IdxDetails = YMCARET.YmcaBusinessObject.RefundRequest.GetDetailsForIdxCreation(l_String_DocType)
                    End If
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
                            'Modified By Ashutosh Patil as on 18-Apr-2007
                            If l_String_App_Type = "A" Then
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
                                'Start:AA:05.07.2016 YRS-At-2985 Added below code to fetch and add the wfstatus in the idx file
                            ElseIf l_String_App_Type = "P" Then
                                If l_DataTable_IdxDetails.Columns.Contains("lcWfstatus") AndAlso Not String.IsNullOrEmpty(l_DataTable_IdxDetails.Rows(0)("lcWfstatus").ToString()) Then
                                    l_StringWfstatus = l_DataTable_IdxDetails.Rows(0)("lcWfstatus")
                                Else
                                    l_StringWfstatus = "F"
                                End If
                                'End:AA:05.07.2016 YRS-At-2985 Added below code to fetch and add the wfstatus in the idx file
                            End If
                        Else
                            ExportToPDF = "Cannot find the details for the IDX configuration for " & l_String_DocType & " "
                            Exit Function
                        End If
                    Else
                        ExportToPDF = "Cannot find the details for the IDX configuration " & l_String_DocType & " "
                        Exit Function
                    End If
                    l_StringDocFileName = ""
                    l_StringDocFileName = Me.IDXFileName()
                    l_StringIndexFileName = l_StringDocFileName & ".idx"
                    'by Aparna YREN-3197 16/04/2007
                    l_stringIndexFilePath = l_stringSourceFilePath.Trim() & "IDX"
                End If
                '**** Add File Details For PDF only if PDF Files are supposed to be created
                If Me.CreatePDF = True Then
                    l_StringDocFileName = ""
                    l_StringDocFileName = Me.PDFFileName()
                    l_StringDocFileName = l_StringDocFileName & ".pdf"
                    'by Aparna YREN-3197 16/04/2007
                    l_stringPDFFilePath = l_stringSourceFilePath.Trim() & "PDF"
                    ' Session("StringDestFilePath") = l_StringDestFilePath & "\\"  & l_StringDocFileName
                    Session("StringDestFilePath") = l_stringPDFFilePath & "\\" & l_StringDocFileName
                    'Me.AddFileListRow(l_StringDestFilePath, l_StringDocFileName, m_str_VignettePath, l_StringDocFileName)
                    Me.AddFileListRow(l_stringPDFFilePath, l_StringDocFileName, l_StringDestFilePath, l_StringDocFileName)
                    'by Aparna YREN-3197 16/04/2007
                    If l_StringDocFileName.Trim = "" Then
                        ExportToPDF = "Cannot Generate Export File Name"
                        Exit Function
                    End If
                End If
                '' Added  by sanjay
                l_ds_RptTracking = initialiseFileTracking(l_String_App_Type, l_String_DocType, l_OutputFileType, l_StringTabCode, l_StringIndexFileName, l_stringIndexFilePath, l_StringDocFileName, l_stringPDFFilePath, l_StringDestFilePath, l_stringYMACA, l_str_FundNo)
                l_str_rptTrackingId = YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAOutputFileIDMTrackingLogs(l_ds_RptTracking)

                'Added By SG: BT-960
                If Not m_arr_ReportParams Is Nothing Then 'Chandra sekar |08/01/2016 | YRS-AT-2772
                    If Not m_str_ReportName = "safeharbor_mill.rpt" AndAlso m_arr_ReportParams.Count > 0 Then
                        l_RefRequestsID = m_arr_ReportParams(0).ToString()
                    End If
                End If
                ''Added  by sanjay Ends Here 
                '**** Show Report only if it is supposed to show 
                If Me.PreviewReport = True Then
                    Try
                        Call Me.ShowReport()
                    Catch ex As Exception
                        l_str_Exception = ""
                        If l_str_ExceptionMode = "IE" Then
                            l_str_Exception = ex.Message
                            If Not ex.InnerException Is Nothing Then
                                l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                            End If
                        ElseIf l_str_ExceptionMode = "ST" Then
                            l_str_Exception = ex.StackTrace
                        End If

                        Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", l_str_Exception)  '' Added  by sanjay
                        'SendMail("Pdf Creation Failed", "PDF file Tracking")
                        Throw
                    End Try
                End If
                Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "") '' Added  by sanjay
                '**** Creating IDX details only if they are required 
                If Me.CreateIDX = True Then
                    'Modified By Ashutosh Patil as on 18-Apr-2007
                    Try
                        If Not l_DatatablePartYMCA Is Nothing Then
                            If l_String_App_Type = "A" Then
                                'Start: AA:05.07.2016 YRS-AT-2985 Commented below lines and calling the new method name
                                'l_BoolRetFlag = Me.CreatePersIdxForTower(l_stringIndexFilePath, l_StringIndexFileName, l_stringYMACA, l_stringYMACAName, l_stringYMACACity, l_stringYMACAState, l_StringApp, l_String_DocType, l_StringDocTab, l_StringSLevel, l_StringTabCode, l_StringWfstatus)
                                l_BoolRetFlag = Me.CreateYMCAIdxForTower(l_stringIndexFilePath, l_StringIndexFileName, l_stringYMACA, l_stringYMACAName, l_stringYMACACity, l_stringYMACAState, l_StringApp, l_String_DocType, l_StringDocTab, l_StringSLevel, l_StringTabCode, l_StringWfstatus)
                                'End: AA:05.07.2016 YRS-AT-2985 Commented below lines and calling the new method name
                            Else
                                l_BoolRetFlag = Me.CreatePersIdxForTower(l_stringIndexFilePath, l_StringIndexFileName, l_str_FundNo, l_str_SSNO, l_str_Firstname, l_str_Middlename, l_str_Lastname, l_StringSLevel, l_String_DocType, l_StringStaffFL, l_StringTabCode _
                                                                         , l_StringWfstatus) 'AA:05.07.2016 YRS-AT-2985 Added to store wfstatus line in idx file
                            End If

                            m_bool_idxCreated = IIf(File.Exists(l_stringIndexFilePath & "\\" & l_StringIndexFileName) = True, True, False)

                            'Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "")

                            If l_BoolRetFlag = False Then
                                m_bool_idxCreated = False
                                Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "Unable to create IDX file.")
                            Else
                                Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", "")
                            End If

                        End If
                    Catch ex As Exception
                        l_BoolRetFlag = False
                        m_bool_idxCreated = False
                        l_str_Exception = ""
                        If l_str_ExceptionMode = "IE" Then
                            l_str_Exception = ex.Message
                            If Not ex.InnerException Is Nothing Then
                                l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                            End If
                        ElseIf l_str_ExceptionMode = "ST" Then
                            l_str_Exception = ex.StackTrace
                        End If

                        Me.FilesTracking(IIf(l_str_rptTrackingId = "", 0, Convert.ToInt32(l_str_rptTrackingId)), m_bool_idxCreated, m_bool_PdfCreated, l_StringIndexFileName, l_StringDocFileName, False, False, False, False, False, False, "C", l_str_Exception)
                    End Try


                    If l_BoolRetFlag Then
                        Me.AddFileListRow(l_stringIndexFilePath, l_StringIndexFileName, l_StringDestFilePath, l_StringIndexFileName)
                    End If
                End If
                If m_bool_Createidx = True Then
                    If File.Exists(l_stringIndexFilePath & "\\" & l_StringIndexFileName) = True Then
                        Dim l_StreamWriter As StreamWriter
                        l_StreamWriter = File.AppendText(l_stringIndexFilePath & "\\" & l_StringIndexFileName)
                        l_StreamWriter.WriteLine("spchar1=" & l_str_rptTrackingId)
                        l_StreamWriter.Close()
                    End If
                End If
                ExportToPDF = ""
            End If
        Catch ' ex As Exception 'commented by hafiz on 8-May-2007
            ExportToPDF = "Error While Writing File"
            Throw   'added by hafiz on 8-May-2007
        End Try
    End Function
    Private Function ShowReport() As Boolean
        Dim l_str_ReportName As String
        Try
            l_str_ReportName = Me.ReportName
            'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            'If l_str_ReportName.Trim <> String.Empty Then
            '   Me.GenerateReportToFile(l_str_ReportName)
            'End If
            If Not String.IsNullOrEmpty(l_str_ReportName) Then
                Select Case Me.ReportType
                    Case YMCAObjects.EnumReportType.RDLC.ToString()
                        Me.GenerateRDLCReportToFile(l_str_ReportName)
                    Case Else
                        Me.GenerateReportToFile(l_str_ReportName)
                End Select
            End If
            'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
        Catch
            Throw
        End Try
    End Function
    Public Function GenerateReportToFile(ByVal l_str_ReportName As String) As Boolean
        Dim l_string_filename As String
        Dim l_string_filepath As String
        Dim l_string_reportpath As String
        Dim crCon As New ConnectionInfo
        Dim CrTableLogonInfo As New TableLogOnInfo
        Dim CrTables As Tables
        Dim CrTable As Table
        Dim l_ArrListParamValues As ArrayList
        Dim l_string_DataSource As String
        Dim l_string_DatabaseName As String
        Dim l_string_UserID As String
        Dim l_string_Password As String
        Dim l_paramItem As String
        Dim l_String_DestFilePath As String
        Try
            l_String_DestFilePath = CType(Session("StringDestFilePath"), String)
            l_string_DataSource = ConfigurationSettings.AppSettings("DataSource")
            l_string_DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            l_string_UserID = ConfigurationSettings.AppSettings("UserID")
            l_string_Password = ConfigurationSettings.AppSettings("Password")
            l_string_reportpath = System.Configuration.ConfigurationSettings.AppSettings("ReportPath")
            l_string_filepath = l_string_reportpath.Trim() & "\\" & l_str_ReportName

            Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument

            objRpt.Load(l_string_filepath)
            objRpt.Refresh()    'added by hafiz on 8-May-2007 - YREN-3334
            l_ArrListParamValues = m_arr_ReportParams

            If l_ArrListParamValues.Count > 0 Then
                Dim l_paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = objRpt.ParameterFields
                Dim l_paramField As CrystalDecisions.Shared.ParameterField = l_paramFieldsCollection(0)
                Dim l_curValues As CrystalDecisions.Shared.ParameterValues = l_paramField.CurrentValues
                Dim l_discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                Dim i As Integer
                i = 0
                For i = 0 To l_ArrListParamValues.Count - 1
                    l_curValues = New CrystalDecisions.Shared.ParameterValues
                    l_discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    l_paramItem = l_ArrListParamValues.Item(i)
                    l_paramField = l_paramFieldsCollection(i)
                    l_curValues = l_paramField.CurrentValues
                    l_discreteValue.Value = l_paramItem.ToString
                    l_curValues.Add(l_discreteValue)
                    'l_paramField.CurrentValues = l_curValues
                    objRpt.ParameterFields(i).CurrentValues = l_curValues
                Next
            End If

            If Me.LogonToDb Then
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
                'start - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
                For iSubCount As Integer = 0 To objRpt.Subreports.Count - 1
                    CrTables = objRpt.Subreports(iSubCount).Database.Tables
                    For Each CrTable In CrTables
                        CrTableLogonInfo = CrTable.LogOnInfo
                        CrTableLogonInfo.ConnectionInfo = crCon
                        CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                        CrTable.Location = l_string_DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                    Next
                Next
                'end - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            End If
            objRpt.ExportToDisk(ExportFormatType.PortableDocFormat, l_String_DestFilePath)
            objRpt.Close()
            objRpt.Dispose()
            objRpt = Nothing
            GC.Collect()
            m_bool_PdfCreated = True
            m_bool_idxCreated = False
        Catch
            Throw
        End Try
    End Function
    Public Function AddFileListRow(ByVal p_String_SourceFolder As String, ByVal p_String_SourceFile As String, ByVal p_String_DestFolder As String, ByVal p_String_DestFile As String) As Boolean
        Dim l_curValues As DataRow
        Try
            If p_String_SourceFolder <> "" And p_String_SourceFile <> "" And p_String_DestFolder <> "" And p_String_DestFile <> "" Then
                l_curValues = m_dtFileList.NewRow()
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
                AddFileListRow = True
            End If
        Catch
            Throw
        End Try
    End Function
    'Start:AA:05.07.2016 YRS_AT_2985 Commented and changed the method name because the name is misleading the process and also the same method violating the method overloading rule
    'Private Function CreatePersIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringYMCANO As String, ByVal p_StringYMCAName As String, ByVal p_StringYMCACity As String, ByVal p_StringYMCAState As String, ByVal p_StringApp As String, ByVal p_StringDocCode As String, ByVal p_StringDoctab As String, ByVal p_StringLevel As String, ByVal p_StringTabCode As String, ByVal p_StringWfstatus As String) As Boolean
    Private Function CreateYMCAIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringYMCANO As String, ByVal p_StringYMCAName As String, ByVal p_StringYMCACity As String, ByVal p_StringYMCAState As String, ByVal p_StringApp As String, ByVal p_StringDocCode As String, ByVal p_StringDoctab As String, ByVal p_StringLevel As String, ByVal p_StringTabCode As String, ByVal p_StringWfstatus As String) As Boolean
        'End:AA:05.07.2016 YRS_AT_2985 Commented and changed the method name because the name is misleading the process and also the same method violating the method overloading rule
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
    Private Function CreatePersIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringFundId As String, ByVal p_StringSSNo As String, ByVal p_StringFirstName As String, ByVal p_StringMiddleName As String, ByVal p_StringLastName As String, ByVal p_StringLevel As String, ByVal p_StringDocCode As String, ByVal p_StringStaffFL As String, ByVal p_StringTabCode As String _
                                           , ByVal p_StringWfstatus As String) As Boolean 'AA 05.07.2016 YRS-AT-2985 Added this parameter to add the new line wfstatus in idx file
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

            'Start:AA 05.07.2016 YRS-AT-2985 Added this parameter to add the new line wfstatus in idx file
            If String.IsNullOrEmpty(p_StringWfstatus.Trim) Then
                Session("IndexFileWriteError") = "Invalid Wfstatus code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_IndexFileData.Append(System.Environment.NewLine + "wfstatus=" & p_StringWfstatus.Trim)
            'End:AA 05.07.2016 YRS-AT-2985 Added this parameter to add the new line wfstatus in idx file

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
    Private Function GetDestinationFolderName() As String
        Dim l_dataset As DataSet
        Dim l_datarow As DataRow
        Dim l_string_FolderName As String
        Dim l_string_OutputFileType As String
        l_string_OutputFileType = Me.OutputFileType
        Try
            l_dataset = YMCARET.YmcaBusinessObject.InterestProcessingBOClass.getMetaOutputFileType("" & l_string_OutputFileType & "")
            If Not l_dataset Is Nothing Then
                If l_dataset.Tables(0).Rows.Count < 1 Then
                    Session("IndexFileWriteError") = "Please Add the Record for the Output Directory in AtsMetaOutputFileTypes table to generate report"
                    GetDestinationFolderName = String.Empty
                    Exit Function
                End If
                l_datarow = l_dataset.Tables(0).Rows(0)
                If (l_datarow("OutputDirectory").GetType.ToString = "System.DBNull") Or CType(l_datarow("OutputDirectory"), String) = "" Then
                    Session("IndexFileWriteError") = "Output Directory value not found in the Table"
                    GetDestinationFolderName = String.Empty
                    Exit Function
                End If
                If Not Directory.Exists(CType(l_datarow("OutputDirectory"), String).Trim()) Then
                    Session("IndexFileWriteError") = "Directory does not exist for creating report file."
                    GetDestinationFolderName = String.Empty
                    Exit Function
                End If
                If (l_datarow("FilenamePrefix").GetType.ToString = "System.DBNull") Or CType(l_datarow("FilenamePrefix"), String) = "" Then
                    l_string_FolderName = CType(l_datarow("OutputDirectory"), String).Trim()
                Else
                    l_string_FolderName = CType(l_datarow("OutputDirectory"), String).Trim()
                End If
                GetDestinationFolderName = l_string_FolderName
            Else
                Session("IndexFileWriteError") = "Please Add the Record for the Output Directory in AtsMetaOutputFileTypes table to generate report"
                GetDestinationFolderName = String.Empty
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function
    '''''Added by sanjay
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
    Private Sub AddReportTrackingRow(ByVal dr As DataRow, ByVal DocCode As String, ByVal SubDocName As String, ByVal DocName As String, ByVal RefId As String, ByVal EntityType As String, ByVal EntityId As String, ByVal IDXFilename As String, ByVal IDXFilePath As String, ByVal PDFFileName As String, ByVal PDFFilePath As String, ByVal PdfCreated As Boolean, ByVal idxCreated As Boolean, ByVal DestFilePath As String)
        dr("ReportTrackingID") = 1
        dr("chvDoccode") = DocCode
        dr("chvSubDocName") = SubDocName
        dr("chvDocName") = DocName
        dr("chvRefId") = RefId
        dr("chrEntityType") = EntityType
        dr("chvEntityId") = EntityId
        dr("dtmDocdate") = Now.Today.ToString()
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
    '''''''''Ends Here
    Public Sub FilesTracking(ByVal pTrackingId As Integer, ByVal pIdxFileCreated As Boolean, ByVal pPdfFilecreated As Boolean, ByVal pIdxFileName As String, ByVal pPdfFileName As String, ByVal pIdxFileinitiated As Boolean, ByVal pIdxfileCopied As Boolean, ByVal pIdxFileDeleted As Boolean, ByVal pPdfFileinitiated As Boolean, ByVal pPdffileCopied As Boolean, ByVal pPdfFileDeleted As Boolean, ByVal pFileType As Char, ByVal pErrorMsg As String)
        Try
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateYMCAOutputFileIDMTrackingLogs(pTrackingId, pIdxFileCreated, pPdfFilecreated, pIdxFileName, pIdxFileinitiated, pIdxfileCopied, pIdxFileDeleted, pPdfFileName, pPdfFileinitiated, pPdffileCopied, pPdfFileDeleted, pFileType, pErrorMsg)
        Catch
            Throw
        End Try
    End Sub
    Private Sub SendMail(ByVal MessageBody As String, ByVal MessageSubject As String)
        Try
            Dim obj As MailUtil
            Dim l_strEmailCC As String = ""
            Dim l_Attachments As String = ""
            Dim l_Attachments1 As String = ""
            Dim l_Attachments2 As String = ""
            Dim l_Attachments3 As String = ""
            Dim l_str_msg As String
            Dim l_DataTable As New DataTable
            Dim l_DataRow As DataRow
            obj = New MailUtil
            obj.MailCategory = "ADMIN"
            If obj.MailService = False Then Exit Sub
            obj.ToMail = obj.FromMail
            obj.SendCc += l_strEmailCC
            obj.MailMessage = MessageBody
            obj.Subject = MessageSubject
            'If Session("StringDestFilePath") <> "" Then
            '    l_Attachments = Session("StringDestFilePath")
            '    If l_Attachments <> "" Then
            '        obj.MailAttachments.Add(l_Attachments)
            '    End If
            'End If
            obj.Send()
        Catch ex As Exception
            Throw
        End Try
    End Sub
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

    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Private Sub GenerateRDLCReportToFile(ByVal strReportName As String)
        Dim objWarnings As Warning()
        Dim strStreamIds As String()
        Dim bytesReportData As Byte()
        Try
            objRDLCReportViewer.LocalReport.ReportPath = "Reports\" & strReportName & ".rdlc"
            objRDLCReportDataSource = New ReportDataSource("dsWaivedParticipants", Me.ReportDataTable)
            objRDLCReportViewer.LocalReport.DataSources.Clear()

            If Not RDLCReportParameters Is Nothing AndAlso RDLCReportParameters.Count > 0 Then
                'Add the Parameters to RDLC Report, in the Dictionary Key is ParamaterName and Value is Parameter Values
                For Each dicReportParametersVal As KeyValuePair(Of String, String) In RDLCReportParameters
                    objRDLCReportParameter.Add(New ReportParameter(dicReportParametersVal.Key, dicReportParametersVal.Value.ToString(), True))
                Next

                objRDLCReportViewer.LocalReport.SetParameters(objRDLCReportParameter)
            End If

            objRDLCReportViewer.LocalReport.DataSources.Add(objRDLCReportDataSource)
            bytesReportData = objRDLCReportViewer.LocalReport.Render("pdf", Nothing, String.Empty, String.Empty, String.Empty, strStreamIds, objWarnings)
            Using Stream As New FileStream(Session("StringDestFilePath"), FileMode.Create)
                Stream.Write(bytesReportData, 0, bytesReportData.Length)
            End Using
            m_bool_PdfCreated = True
        Catch
            Throw
        Finally
            objRDLCReportViewer.Dispose()
            objRDLCReportDataSource = Nothing
        End Try
    End Sub
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
#End Region 'Functions
End Class