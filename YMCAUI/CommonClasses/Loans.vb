' ******************************************************************
' Project ID: YMCA
' Author: Manthan Rajguru
' Created on: 09/04/2018
' Summary of Functionality: Loan Object
' ***************************************
'
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
'                               |               |               |                  
' ------------------------------------------------------------------------------------------------------
' ******************************************************************
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports YMCAObjects
Imports YMCARET.YmcaBusinessObject.MetaMessageBO
Imports YMCAObjects.MetaMessageList
Imports PdfSharp
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf.IO
Imports System.Web

Public Class Loans

    Private localParticipantLetter As YMCAObjects.LinkDetail
    Public Property ParticipantLetter() As YMCAObjects.LinkDetail
        Get
            Return localParticipantLetter
        End Get
        Set(ByVal value As YMCAObjects.LinkDetail)
            localParticipantLetter = value
        End Set
    End Property

    Private localYMCALetter As YMCAObjects.LinkDetail
    Public Property YMCALetter() As YMCAObjects.LinkDetail
        Get
            Return localYMCALetter
        End Get
        Set(ByVal value As YMCAObjects.LinkDetail)
            localYMCALetter = value
        End Set
    End Property

    ''' <summary>
    ''' Processes loan request. It does following steps:
    ''' 1. Validates Loan Request
    ''' 2. Saves details into database
    ''' 3. Generates PDF for participant
    ''' 4. Generates PDF for YMCA
    ''' 5. Sends Email to Participant & YMCA
    ''' 6. Sends Email copy to IDM
    ''' </summary>
    ''' <param name="loan">Loan Details</param>
    ''' <returns>Loan Details with status of process</returns>
    ''' <remarks></remarks>
    Public Function Process(ByVal loan As YMCAObjects.Loan) As YMCAObjects.Loan
        Dim errorMessage As String
        Dim erroNo As Integer
        Try
            If Not loan Is Nothing Then
                'Step 1 - Validate Loan Request
                loan = DoCommonValidation(loan)
                If String.IsNullOrEmpty(loan.Error) Then
                    'Step 2 - Save Details into Database
                    errorMessage = YMCARET.YmcaBusinessObject.LoanInformationBOClass.SaveProcessingData(loan.PersId, loan.RequestNo, loan.FundId, loan.Amount, loan.DeductionTable)
                    If Not String.IsNullOrEmpty(errorMessage) AndAlso errorMessage.ToUpper = "ERROR" Then
                        loan.Error = MESSAGE_LOAN_PROCESS_UNSUCCESFULL.ToString
                    Else
                        loan.Participant = New LoanOperation
                        loan.YMCA = New LoanOperation
                        If Not String.IsNullOrEmpty(loan.PaymentMethodCode) AndAlso loan.PaymentMethodCode.ToUpper = PaymentMethod.CHECK Then
                            'Set report details for participant
                            loan.Participant = SetReportDetails(loan.Participant, EnumEntityCode.PERSON)
                            'Set report details for YMCA
                            'loan.YMCA = SetReportDetails(loan.YMCA, EnumEntityCode.YMCA) 'MMR | 2018.10.31 | Commented to not set report details as letter will not be generated for YMCA
                        End If

                        'Step 3 - Generate PDF for participant
                        loan = GeneratePDF(loan, EnumEntityCode.PERSON)

                        'Step 4 - Generate PDF for YMCA
                        'loan = GeneratePDF(loan, EnumEntityCode.YMCA) 'MMR | 2018.10.31 | Commented as letter will not be generated for YMCA

                        'Step 5 - Send Email to Participant & YMCA
                        loan = SendEmail(loan)

                        'Step 6 - Maintain Error
                        If Not String.IsNullOrEmpty(loan.Error) Then
                            errorMessage = loan.Error
                            If (Integer.TryParse(errorMessage, erroNo)) Then
                                loan.Error = GetMessageByTextMessageNo(erroNo)
                            End If
                        End If
                    End If
                End If
            End If
            Return loan
        Catch ex As Exception
            HelperFunctions.LogException("Loans_Process", ex)
            loan.Error = GetMessageByTextMessageNo(MESSAGE_LOAN_PROCESS_UNSUCCESFULL)
            ex = Nothing
            Return loan
        Finally
            errorMessage = String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Validates loan details.
    ''' </summary>
    ''' <param name="loan">Loan Details</param>
    ''' <returns>Loan Details with validation status</returns>
    ''' <remarks></remarks>
    Public Function DoCommonValidation(ByVal loan As YMCAObjects.Loan) As YMCAObjects.Loan
        Dim reason As String
        Try
            If Not loan Is Nothing Then
                reason = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanValidationReason(loan.RequestNo)
                If Not String.IsNullOrEmpty(reason) Then
                    loan.Error = reason
                End If
            End If
            Return loan
        Catch
            Throw
        Finally
            reason = String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Sets report details based on entity type
    ''' </summary>
    ''' <param name="loanOperation">Loan's entity related operation dtails</param>
    ''' <param name="entity">Entity Type (PERSON/YMCA)</param>
    ''' <returns>Loan's entity related operation dtails</returns>
    ''' <remarks></remarks>
    Public Function SetReportDetails(ByVal loanOperation As YMCAObjects.LoanOperation, ByVal entity As EnumEntityCode) As YMCAObjects.LoanOperation
        If loanOperation Is Nothing Then
            loanOperation = New LoanOperation
        End If
        If entity = EnumEntityCode.PERSON Then
            loanOperation.Report = "Loan Letter to Participant.rpt"
            loanOperation.AppType = "P"
            loanOperation.DocTypeCode = IDMDocumentCodes.LoanProcessing_ParticipantLetter
        ElseIf entity = EnumEntityCode.YMCA Then
            loanOperation.Report = "Loan Letter to Association.rpt"
            loanOperation.AppType = "A"
            loanOperation.DocTypeCode = IDMDocumentCodes.LoanProcessing_YMCALetter
        End If
        Return loanOperation
    End Function

    ''' <summary>
    ''' Generates PDF based on entity type.
    ''' </summary>
    ''' <param name="loans">Loan Details</param>
    ''' <param name="entity">Entity Type (PERSON/YMCA)</param>
    ''' <returns>Loan Details</returns>
    ''' <remarks></remarks>
    Public Function GeneratePDF(ByVal loan As YMCAObjects.Loan, ByVal entity As EnumEntityCode) As YMCAObjects.Loan
        If Not loan Is Nothing AndAlso String.IsNullOrEmpty(loan.Error) Then
            If Not String.IsNullOrEmpty(loan.PaymentMethodCode) AndAlso loan.PaymentMethodCode.ToUpper = PaymentMethod.CHECK Then
                Try
                    loan = GenerateReports(loan, entity)
                Catch ex As Exception
                    HelperFunctions.LogException("Loans_GeneratePDF", ex)
                    loan.Error = MESSAGE_LOAN_PDF_FAILED.ToString()
                    ex = Nothing
                End Try
            End If
        End If
        Return loan
    End Function

    ''' <summary>
    ''' Generates PDF based on entity typ. It does following steps:
    ''' 1. Initializes required objects and values for IDM Class (IDM class, YMCAID, persid)
    ''' 2. Sets IDM Properties required for PDF Generation by calling method SetIDMProperties
    ''' 3. Calls ExportToPDF method of IDM Class to generate PDF
    ''' 4. Sets PDF File path by calling method GetPDFFilePath
    ''' </summary>
    ''' <param name="loan">Loan details</param>
    ''' <param name="entity">Entity Type (PERSON/YMCA)</param>
    ''' <returns>Loan details with pdf details</returns>
    ''' <remarks></remarks>
    Private Function GenerateReports(ByVal loan As YMCAObjects.Loan, ByVal entity As EnumEntityCode) As YMCAObjects.Loan
        Dim IDM As IDMforAll
        Dim operation As YMCAObjects.LoanOperation
        Dim listParamValues As ArrayList
        Dim errorMessage As String
        Try
            'Step 1 - Initializing objects and variables
            IDM = New IDMforAll 'initialise IDM class
            listParamValues = New ArrayList

            If IDM.DatatableFileList(False) Then
            Else
                Throw New Exception("Unable to create dependent table")
            End If

            IDM.PersId = loan.PersId
            listParamValues.Add(loan.PersId)
            IDM.YMCAID = loan.YMCAId

            If entity = EnumEntityCode.PERSON Then
                operation = loan.Participant
            ElseIf entity = EnumEntityCode.YMCA Then
                operation = loan.YMCA
            End If

            'Step 2 - Assign values to the IDM properties
            SetIDMProperties(operation, listParamValues, IDM)

            'Step 3 - Generate PDF
            errorMessage = IDM.ExportToPDF()
            If Not String.IsNullOrEmpty(errorMessage) Then
                operation.IDMStatus = False
                HelperFunctions.LogMessage(errorMessage)
                loan.Error = MESSAGE_LOAN_PDF_FAILED.ToString()
            Else
                operation.IDMStatus = True
            End If

            'step 4 - Set PDF File Path
            operation = GetFilePathForIDM(operation, IDM.SetdtFileList)

            If entity = EnumEntityCode.PERSON Then
                loan.Participant = operation
            ElseIf entity = EnumEntityCode.YMCA Then
                loan.YMCA = operation
            End If

            Return loan
        Catch
            Throw
        Finally
            errorMessage = Nothing
            operation = Nothing
            listParamValues = Nothing
            IDM = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Sets IDM properties.
    ''' </summary>
    ''' <param name="loanOperation">Loan's entity related operation dtails</param>
    ''' <param name="values">Report Parameter values</param>
    ''' <param name="IDM">IDM Class object</param>
    ''' <remarks></remarks>
    Private Sub SetIDMProperties(ByVal loanOperation As YMCAObjects.LoanOperation, ByVal values As ArrayList, ByVal IDM As IDMforAll)
        If Not loanOperation Is Nothing Then
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = loanOperation.AppType
            IDM.OutputFileType = "TDLoan_" & loanOperation.DocTypeCode & " "
            IDM.DocTypeCode = loanOperation.DocTypeCode
            IDM.ReportName = loanOperation.Report
            IDM.ReportParameters = values
        End If
    End Sub

    ''' <summary>
    ''' Provides PDF file path.
    ''' </summary>
    ''' <param name="filePathTable">File path</param>
    ''' <returns>File Path</returns>
    ''' <remarks></remarks>
    Private Function GetPDFFilePath(ByVal filePathTable As DataTable, ByVal fileType As String) As String
        Dim filePath As String
        Dim fileListRow As DataRow()
        Dim list As New ArrayList()
        Dim files As String()
        Dim despPathRow As DataRow
        Try
            If HelperFunctions.isNonEmpty(filePathTable) Then
                If Not String.IsNullOrEmpty(fileType) Then
                    If fileType = "PDF" Then
                        fileListRow = filePathTable.Select("SourceFolder LIKE '%PDF%'")
                    ElseIf fileType = "IDX" Then
                        fileListRow = filePathTable.Select("SourceFolder LIKE '%IDX%'")
                    End If
                For iCount As Integer = 0 To fileListRow.Length - 1
                    Dim info As System.IO.FileInfo
                    Dim ds As New System.IO.DirectoryInfo(fileListRow(iCount)("SourceFolder").ToString())
                    info = ds.GetFiles(fileListRow(iCount)("SourceFile").ToString().Replace(fileListRow(iCount)("SourceFolder").ToString() + "\", ""))(0)
                    ' HACK: Just skip the protected samples file...
                    If info.Name.IndexOf("protected") = -1 Then
                        list.Add(info.FullName)
                    End If
                Next
                files = list.ToArray(GetType(String))
                For Each file In files
                    filePath = file
                Next
                Else
                    despPathRow = filePathTable.Rows(0)
                    If despPathRow("DestFolder").GetType.ToString <> "System.DBNull" Or (despPathRow("DestFolder")).GetType.ToString <> "" Then
                        filePath = despPathRow("DestFolder").ToString()
                    End If
                End If
            End If
            Return filePath
        Catch
            Throw
        Finally
            filePath = Nothing
            fileListRow = Nothing
            list = Nothing
            files = Nothing
        End Try
    End Function
    ''' <summary>
    ''' Get PDF source and destination path
    ''' </summary>
    ''' <param name="operation">Loan operation object</param>
    ''' <param name="filePathTable">file Path Table</param>
    ''' <returns>Loan operation object</returns>
    ''' <remarks></remarks>
    Private Function GetFilePathForIDM(operation As YMCAObjects.LoanOperation, ByVal filePathTable As DataTable) As YMCAObjects.LoanOperation
        Try
            If HelperFunctions.isNonEmpty(filePathTable) Then
                operation.FilePath = GetPDFFilePath(filePathTable, "PDF")
                operation.IDXPath = GetPDFFilePath(filePathTable, "IDX")
                operation.IDMDestinationPath = GetPDFFilePath(filePathTable, "")
            End If
            Return operation
        Catch
            Throw
        Finally
            operation = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Helps to send email
    ''' </summary>
    ''' <param name="loan">Loan details</param>
    ''' <returns>Loan details</returns>
    ''' <remarks></remarks>
    Private Function SendEmail(ByVal loan As YMCAObjects.Loan) As YMCAObjects.Loan
        Dim mailClient As YMCARET.YmcaBusinessObject.MailUtil
        Dim result As YMCAObjects.ReturnObject(Of Boolean)
        Dim ymcaAttachment As String

        If Not loan Is Nothing AndAlso String.IsNullOrEmpty(loan.Error) Then
            mailClient = New YMCARET.YmcaBusinessObject.MailUtil
            'START : MMR | 2018.10.31 | Commented to not set attachment file path as letter will not be generated for YMCA and will not be part of email communication to LPA
            'If (Not loan.YMCA Is Nothing AndAlso Not String.IsNullOrEmpty(loan.YMCA.FilePath)) Then
            '    ymcaAttachment = loan.YMCA.FilePath
            'End If
            'END : MMR | 2018.10.31 | Commented to not set attachment file path as letter will not be generated for YMCA and will not be part of email communication to LPA
            result = mailClient.SendLoanEmail(YMCAObjects.LoanStatus.DISB, loan.LoanNumber, loan.PaymentMethodCode, ymcaAttachment)

            If (Not loan.Participant Is Nothing) Then
                loan.Participant.EmailStatus = result.Value
            End If

            If (Not loan.YMCA Is Nothing) Then
                loan.YMCA.EmailStatus = result.Value
            End If

            If Not result.Value Then
                loan.Error = result.MessageList(0)
            End If
        End If
        Return loan
    End Function

    ''' <summary>
    ''' Consolidates all provided pdf documents into one pdf document
    ''' </summary>
    ''' <param name="listOfLoans">List of Loans with pdf document details</param>
    ''' <param name="batchID">Batch ID which helps to create consolidated file</param>
    ''' <remarks></remarks>
    Public Sub ConsolidateAllLoanPDF(ByVal listOfLoans As List(Of YMCAObjects.Loan), ByVal batchID As String)
        Dim listOfParticipantPDFs As List(Of String)
        Dim listOfYMCAPDFs As List(Of String)

        Dim participantPDFFilePath As String
        Dim ymcaPDFFilePath As String

        Dim pathAndFileName As String
        Dim relativePath As String
        Try
            'Copy PDF letter to IDM Server
            Try
                CopyFilesToIDMServer(listOfLoans)
            Catch ex As Exception
                ex = Nothing
            End Try

            'Step 1 - Set Participant Merged PDF file name, Fetch List of participant Letter & Merge PDF by calling MergePDFs function
            Try
                participantPDFFilePath = String.Format("ConsolidatedDocumentParticipant_{0}.pdf", batchID)
                pathAndFileName = String.Format("{0}\{1}", Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), participantPDFFilePath)
                relativePath = String.Format("{0}\\{1}", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), participantPDFFilePath)

                listOfParticipantPDFs = listOfLoans.Where(Function(t) t.PaymentMethodCode.ToUpper = PaymentMethod.CHECK).Select(Function(t) t.Participant.FilePath).ToList()
                If Not listOfParticipantPDFs Is Nothing Then
                    If MergePDFs(listOfParticipantPDFs, pathAndFileName) Then
                        Me.ParticipantLetter = GetLink("Participant", relativePath)
                    End If
                End If
            Catch ex As Exception
                ex = Nothing
            End Try

            'Step 3 - Set YMCA Merged PDF file name, Fetch List of YMCA Letter & Merge PDF by calling MergePDFs function
            Try
                ymcaPDFFilePath = String.Format("ConsolidatedDocumentYMCA_{0}.pdf", batchID)

                pathAndFileName = String.Format("{0}\{1}", Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), ymcaPDFFilePath)
                relativePath = String.Format("{0}\\{1}", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), ymcaPDFFilePath)

                listOfYMCAPDFs = listOfLoans.Where(Function(t) t.PaymentMethodCode.ToUpper = PaymentMethod.CHECK).Select(Function(t) t.YMCA.FilePath).ToList()
                If Not listOfYMCAPDFs Is Nothing Then
                    If MergePDFs(listOfYMCAPDFs, pathAndFileName) Then
                        Me.YMCALetter = GetLink("LPA", relativePath)
                    End If
                End If
            Catch ex As Exception
                ex = Nothing
            End Try
        Catch
            Throw
        Finally
            listOfParticipantPDFs = Nothing
            listOfYMCAPDFs = Nothing
            pathAndFileName = Nothing
            relativePath = Nothing
            participantPDFFilePath = Nothing
            ymcaPDFFilePath = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' Sends copy of PDF to IDM Server
    ''' </summary>
    ''' <param name="listOfLoans">Loans object</param>
    ''' <remarks></remarks>
    Public Sub CopyFilesToIDMServer(ByVal listOfLoans As List(Of YMCAObjects.Loan))
        Dim idmPathDetails As DataTable

        Try
            'Table to set IDM Path Details 
            idmPathDetails = New DataTable()
            idmPathDetails.Columns.Add("SourceFolder", System.Type.GetType("System.String"))
            idmPathDetails.Columns.Add("SourceFile", System.Type.GetType("System.String"))
            idmPathDetails.Columns.Add("DestFolder", System.Type.GetType("System.String"))
            idmPathDetails.Columns.Add("DestFile", System.Type.GetType("System.String"))

            If Not listOfLoans Is Nothing Then
                For Each file As YMCAObjects.Loan In listOfLoans
                    PrepareIDMPathDetails(idmPathDetails, file.Participant)
                Next
                'Set IDM Path details in session
                If HelperFunctions.isNonEmpty(idmPathDetails) Then
                    Web.HttpContext.Current.Session("FTFileList") = idmPathDetails
                End If
            End If
        Catch
            Throw
        Finally
            idmPathDetails = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Set IDM Path Details and store it in table
    ''' </summary>
    ''' <param name="idmPathDetails">idm Path Details</param>
    ''' <param name="operation">operation</param>
    ''' <remarks></remarks>
    Private Sub PrepareIDMPathDetails(ByRef idmPathDetails As DataTable, ByVal operation As YMCAObjects.LoanOperation)
        Dim idmPathDetailsRow As DataRow
        Dim sourceFolder As String, sourceFile As String, destFolder As String, destFile As String

        Try
            If Not String.IsNullOrEmpty(operation.FilePath) Then
                idmPathDetailsRow = idmPathDetails.NewRow()
                sourceFolder = System.IO.Path.GetDirectoryName(operation.FilePath)
                sourceFile = System.IO.Path.GetFileName(operation.FilePath)
                destFolder = operation.IDMDestinationPath
                destFile = sourceFile
                idmPathDetails.Rows.Add(Me.AddFileListRow(sourceFolder, sourceFile, destFolder, destFile, idmPathDetailsRow))
            End If
            If Not String.IsNullOrEmpty(operation.IDXPath) Then
                idmPathDetailsRow = idmPathDetails.NewRow()
                sourceFolder = System.IO.Path.GetDirectoryName(operation.IDXPath)
                sourceFile = System.IO.Path.GetFileName(operation.IDXPath)
                destFolder = operation.IDMDestinationPath
                destFile = sourceFile
                idmPathDetails.Rows.Add(Me.AddFileListRow(sourceFolder, sourceFile, destFolder, destFile, idmPathDetailsRow))
            End If
        Catch
            Throw
        Finally
            idmPathDetailsRow = Nothing
            sourceFolder = String.Empty
            sourceFile = String.Empty
            destFolder = String.Empty
            destFile = String.Empty
        End Try
    End Sub

    ''' <summary>
    ''' Prepare file path details in datarow
    ''' </summary>
    ''' <param name="sourceFolder">source Folder(Generated file folder)</param>
    ''' <param name="sourceFile">source File (File Name)</param>
    ''' <param name="destFolder">dest Folder(Path to copy file)</param>
    ''' <param name="destFile">dest File (File Name to be copied)</param>
    ''' <param name="row">row</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddFileListRow(ByVal sourceFolder As String, ByVal sourceFile As String, ByVal destFolder As String, ByVal destFile As String, ByVal row As DataRow) As DataRow
        Dim values As DataRow
        Try
            If sourceFolder <> "" And sourceFile <> "" And destFolder <> "" And destFile <> "" Then
                values = row
                values("SourceFolder") = sourceFolder
                If Right(sourceFolder, 1) = "\" Then
                    values("SourceFile") = String.Format("{0}{1}", sourceFolder, sourceFile)
                Else
                    values("SourceFile") = String.Format("{0}\{1}", sourceFolder, sourceFile)
                End If

                values("DestFolder") = destFolder
                If Right(destFolder, 1) = "\" Then
                    values("DestFile") = String.Format("{0}{1}", destFolder, destFile)
                Else
                    values("DestFile") = String.Format("{0}\{1}", destFolder, destFile)
                End If
            End If
            Return values
        Catch
            Throw
        End Try
    End Function

    Public Function GetLink(ByVal displayText As String, ByVal filePath As String) As YMCAObjects.LinkDetail
        Dim link As YMCAObjects.LinkDetail
        Dim encryptLink As EncryptedQueryString
        Try
            encryptLink = New EncryptedQueryString()
            encryptLink.Add("link", filePath)

            link = New LinkDetail()
            link.DisplayText = displayText
            link.URL = String.Format("DocumentViewer.aspx?p={0}", encryptLink.ToString())
            Return link
        Catch
            Throw
        Finally
            encryptLink = Nothing
            link = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consolidates all PDF files into one PDF document.
    ''' </summary>
    ''' <param name="listOfLoans">List Of Loans</param>
    ''' <param name="destinationFilePath">Full filepath of document</param>
    ''' <returns>Boolean (True/False)</returns>
    ''' <remarks></remarks>
    Private Function MergePDFs(ByVal listOfLoans As List(Of String), ByVal destinationFilePath As String) As Boolean
        Dim pdfDocOutputDocument As PdfDocument
        Dim inputDocument As PdfDocument
        Dim pdfPage As PdfPage
        Dim pageCount As Integer
        Dim count As Integer
        Dim isMergedPDFSaved As Boolean
        Try
            isMergedPDFSaved = False
            If Not listOfLoans Is Nothing AndAlso Not String.IsNullOrEmpty(destinationFilePath) Then
                pdfDocOutputDocument = New PdfDocument
                For Each file As String In listOfLoans
                    ' Open the document to import pages from it.
                    inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import)
                    ' Iterate pages
                    pageCount = inputDocument.PageCount
                    For count = 0 To pageCount - 1
                        ' Get the page from the external document...
                        pdfPage = inputDocument.Pages(count)
                        ' ...and add it to the output document.
                        pdfDocOutputDocument.AddPage(pdfPage)
                    Next
                Next
                pdfDocOutputDocument.Save(destinationFilePath)
                isMergedPDFSaved = True
            End If
            Return isMergedPDFSaved
        Catch
            Throw
        Finally
            pdfPage = Nothing
            inputDocument = Nothing
            pdfDocOutputDocument = Nothing
        End Try
    End Function
End Class
