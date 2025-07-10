'************************************************************************************************************************
' Author                    : Pramod Prakash Pokale
' Created on                : 04/21/2017
' Summary of Functionality  : Hosts web methods utilised by Batch Process user control 
' Declared in Version       : 20.2.0 | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
' ' Manthan Rajguru			    | 2017.05.04    |	20.2.0	    | YRS-AT-3205 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186)
'   Manthan Rajguru			    | 2017.05.16    |	20.2.0	    | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************
'START: MMR | 2017.05.04 | YRS-AT-3205 | Namespace to access methods
Imports System.IO
Imports YMCAObjects
'END: MMR | 2017.05.04 | YRS-AT-3205 | Namespace to access methods

Public Class BatchProcessProgressUCWebMethods
    Inherits System.Web.UI.Page
    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function ProcessRMDLetters(ByVal batchID As String, ByVal count As Integer, ByVal processName As String, ByVal idxCreated As Integer, ByVal pdfCreated As Integer, ByVal moduleName As String, ByVal allowReprint As Boolean) As ReturnStatusValues 'MMR | 2017.05.04 | YRS-AT-3205 | Added parameter to allow reprint or not
        Dim tempData As DataSet
        Dim result As ReturnStatusValues
        Dim letter As RMDLetters
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", String.Format("RMD PrintLetters process initiate for BatchID: {0}", batchID))
            letter = New RMDLetters()

            tempData = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(batchID, moduleName)
            result = letter.ProcessLetters(batchID, count, processName, tempData, idxCreated, pdfCreated, moduleName)
            result.AllowReprint = allowReprint 'MMR | 2017.05.04 | YRS-AT-3205 | Setting status for reprint
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(batchID, moduleName, tempData)
            If HelperFunctions.isNonEmpty(tempData.Tables("dtFileList")) Then
                HttpContext.Current.Session("FTFileList") = tempData.Tables("dtFileList")
            End If
            Return result
        Catch ex As Exception
            HelperFunctions.LogException("BatchProcessProgressUCWebMethods.ProcessRMDLetters", ex)
            result = New ReturnStatusValues()
            result.strretValue = "error"
            Return result
            'Throw ex
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", String.Format("RMD PrintLetters process completed for BatchID: {0}", batchID))
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function CreateRMDBatch(ByVal batchID As String, ByVal moduleName As String) As String
        Dim printLettersDataSet As DataSet
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", String.Format("Start saving batch for BatchID: {0}", batchID))
            If (Not String.IsNullOrEmpty(batchID) AndAlso Not String.IsNullOrEmpty(moduleName)) Then
                If (Not HttpContext.Current.Session("SelectedRMDPrintLetters") Is Nothing) Then
                    printLettersDataSet = TryCast(HttpContext.Current.Session("SelectedRMDPrintLetters"), DataSet)
                    YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(batchID, moduleName, printLettersDataSet)
                Else
                    batchID = String.Empty
                End If
            Else
                batchID = String.Empty
            End If
            Return batchID
        Catch ex As Exception
            HelperFunctions.LogException("BatchProcessProgressUCWebMethods.CreateRMDBatch", ex)
            Return String.Empty
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", String.Format("Created batch for BatchID: {0}", batchID))
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function GetRMDBatchID() As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "Fetching RMD batch ID")
            Return (New BatchProcessProgressControl()).BatchID
        Catch ex As Exception
            HelperFunctions.LogException("BatchProcessProgressUCWebMethods.GetRMDBatchID", ex)
            Return String.Empty
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "Completed fetching RMD batch ID")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function GetRMDModuleName() As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "Fetching RMD module name")
            Return (New BatchProcessProgressControl()).ModuleName
        Catch ex As Exception
            HelperFunctions.LogException("BatchProcessProgressUCWebMethods.GetRMDModuleName", ex)
            Return String.Empty
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "Completed fetching RMD module name")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    'START: MMR | 2017.05.04 | YRS-AT-3205 | Added method to return reprint status
    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function GetReprintStatus() As Boolean
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "Fetching RMD Reprint Status")
            Return BatchProcessProgressControl.AllowReprint
        Catch ex As Exception
            HelperFunctions.LogException("BatchProcessProgressUCWebMethods.GetReprintStatus", ex)
            Return False
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "Completed fetching RMD Reprint Status")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function
    'END: MMR | YRS-AT-3205 | Added method to make copy of letters in new folder for reprinting

    'START: MMR | YRS-AT-3205 | Added method to make copy of letters in new folder for reprinting
    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function MakeCopiesOfLetterForReprinting(ByVal batchID As String)
        Dim detailsOfLetterForReprinting As DataSet
        Dim sourcePath, sourceFile, sourcePathAndFileName As String
        Dim destinationPath, destinationPathAndFileName As String
        Dim fundEventID As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Start: MakeCopiesOfLetterForReprinting process is started")
            sourcePath = String.Empty
            sourceFile = String.Empty
            destinationPath = String.Empty
            sourcePathAndFileName = String.Empty
            destinationPathAndFileName = String.Empty
            fundEventID = String.Empty

            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Start: Get XML data to copy generated PDF files to Local Folder for Reprinting ")
            detailsOfLetterForReprinting = YMCARET.YmcaBusinessObject.MRDBO.GetIDMDetailsForReprinting(batchID)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Finish: Get XML data to copy generated PDF files to Local Folder for Reprinting")

            If HelperFunctions.isNonEmpty(detailsOfLetterForReprinting) Then
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Start: Get source and destination from XMl data to copy generated PDF files to Local Folder for Reprinting")
                For Each rows As DataRow In detailsOfLetterForReprinting.Tables(0).Rows
                    sourcePath = rows("chvPdfFilePath").ToString()
                    sourceFile = rows("chvPdfFileName").ToString()
                    fundEventID = rows("FundEventID").ToString()

                    sourcePathAndFileName = String.Format("{0}\{1}", sourcePath, sourceFile) 'Setting source path and file name to be copied
                    If String.IsNullOrEmpty(sourcePathAndFileName) Then
                        Exit For
                    End If

                    If Not HttpContext.Current.Session("FolderForReprint") Is Nothing Then
                        destinationPath = HttpContext.Current.Server.MapPath(String.Format("~\{0}\{1}\", System.Configuration.ConfigurationSettings.AppSettings("MergedBatchDocumentPath"), HttpContext.Current.Session("FolderForReprint")))
                        ' Server.MapPath could contain "UserControls" folder also in file address. So replacing it.
                        If (destinationPath.IndexOf("\UserControls") >= 0) Then
                            destinationPath = destinationPath.Replace("\UserControls", "")
                        End If
                    End If

                    If Not Directory.Exists(destinationPath) Then
                        Directory.CreateDirectory(destinationPath)
                    End If

                    destinationPathAndFileName = String.Format("{0}{1}.pdf", destinationPath, fundEventID, ".pdf") 'Setting new path and file name for reprinting

                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Start: Check if destination folder exists or not")
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Finish: Check if destination folder exists or not")
                    Try
                        'START: MMR |2017.05.16 | YRS-AT-3356 | Delete existing file from local folder before copying latest generated PDF so that correct file should open for reprinting
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Start: Searching file in Local Folder for deleting")
                        If File.Exists(destinationPathAndFileName) Then
                            File.Delete(destinationPathAndFileName)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", String.Format("Deleted existing PDF file: {0} from Local Folder for Reprinting before saving new generated PDF", destinationPathAndFileName))
                        End If
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Finish: Searching file in Local Folder for deleting")
                        'END: MMR |2017.05.16 | YRS-AT-3356 | Delete existing file from local folder before copying latest generated PDF so that correct file should open for reprinting
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", String.Format("Start: Copy PDF file from source ({0}) to Local Folder ({1}) for Reprinting", sourcePathAndFileName, destinationPathAndFileName))
                        File.Copy(sourcePathAndFileName, destinationPathAndFileName, True)
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Finish: Copy PDF files from source to Local Folder for Reprinting")
                    Catch ex As Exception
                        HelperFunctions.LogException("RMDRePrintLetters_MakeCopiesOfLetterForReprinting()", ex)  ' Added error log to track any error occured while Copying files from source to destination folder
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Start: Exception raise by copying files to Local Folder for Reprinting")
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeCopiesOfLetterForReprinting Method", "Finish: Exception raise by copying files to Local Folder for Reprinting")
                        Throw
                    End Try
                Next
            End If
        Catch ex As Exception
            HelperFunctions.LogException("MakeCopiesOfLetterForReprinting()", ex)
            Throw ex
        Finally
            detailsOfLetterForReprinting = Nothing
            sourcePath = Nothing
            sourceFile = Nothing
            destinationPath = Nothing
            sourcePathAndFileName = Nothing
            destinationPathAndFileName = Nothing
            fundEventID = Nothing
            HttpContext.Current.Session("AllowReprint") = Nothing
            HttpContext.Current.Session("FolderForReprint") = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace() 'MMR |2017.05.16 | YRS-AT-3356 | End of trace log
        End Try
    End Function
    'END: MMR | YRS-AT-3205 | Added method to make copy of letters in new folder for reprinting
End Class