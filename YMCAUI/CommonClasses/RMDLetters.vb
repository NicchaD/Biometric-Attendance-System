'************************************************************************************************************************
' Author                    : Pramod Prakash Pokale
' Created on                : 04/21/2017
' Summary of Functionality  : Hosts common methods required for various RMD Letter pages
' Declared in Version       : 20.2.0 | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
' Manthan Rajguru			    | 2017.05.04    | 20.2.0	    | YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)
' Santosh Bura                  | 05/04/2017    | 20.2.0        | YRS-AT-3400 & YRS-AT-3401
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Imports YMCAObjects

Public Class RMDLetters
    Public Function ProcessLetters(ByVal batchID As String, ByVal count As Integer, ByVal processName As String, tempData As DataSet, ByVal idxCreated As Integer, ByVal pdfCreated As Integer, moduleName As String) As ReturnStatusValues
        Dim isSuccess As Boolean
        Dim result As ReturnStatusValues
        Dim selectedBatchRecords As DataTable
        Dim batchProcess As BatchRequestCreation
        Dim errorDataList As New List(Of ExceptionLog)
        Dim fileList As DataTable
        Dim participantTable As DataTable
        Dim participantRow As DataRow
        Dim batchSize As Integer
        Dim reportFileName As String
        Dim tempFileList As DataTable
        Dim combinedPlanTypeRecord As DataTable
        Dim combinedDataList As DataTable
        Dim docType, letterCode, reportName, outputFileType As String
        Dim param1 As String
        Dim dtArrErrorDataList As DataTable
        Dim fundNo As String
        Dim links As List(Of LinkDetail)
        Dim link As LinkDetail
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: Calling ProcessLetters")
            batchProcess = New BatchRequestCreation
            fileList = New DataTable
            isSuccess = True
            batchSize = GetBatchSize()

            result = New ReturnStatusValues
            result.strBatchId = batchID
            result.strReportType = moduleName
            result.iProcessCount = count
            result.strretValue = "pending"
            result.iIdxCreated = idxCreated
            result.iPdfCreated = pdfCreated

            selectedBatchRecords = tempData.Tables("SelectedBatchRecords")
            dtArrErrorDataList = tempData.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                For Each drArr As DataRow In dtArrErrorDataList.Rows
                    errorDataList.Add(New ExceptionLog(drArr("FundNo"), drArr("Errors"), drArr("Description")))
                Next
            End If

            If Not selectedBatchRecords.Columns.Contains("IsReportPrinted") Then
                selectedBatchRecords.Columns.Add("IsReportPrinted")
            End If
            'START: MMR | 2017.04.24 | YRS-AT-3205 | Added column got print letter ID
            If Not selectedBatchRecords.Columns.Contains("PrintLetterID") Then
                selectedBatchRecords.Columns.Add("PrintLetterID")
            End If
            'END: MMR | 2017.05.05 | YRS-AT-3205 | Added column got print letter ID

            'START: MMR | 2017.05.05 | YRS-AT-3205 | Commented existing code as not required
            'YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: InsertPrintLetters Method Calling.")
            'If Not selectedBatchRecords Is Nothing Then
            '    For i = count To selectedBatchRecords.Rows.Count - 1
            '        If i - count >= batchSize Then
            '            Exit For
            '        End If
            '        YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(selectedBatchRecords.Rows(i)("FUNDNo").ToString, selectedBatchRecords.Rows(i)("PersonId").ToString, selectedBatchRecords.Rows(i)("LetterCode").ToString, String.Format("PlanType={0}", selectedBatchRecords.Rows(i)("PlanType").ToString))
            '    Next
            'End If
            'YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: InsertPrintLetters Method Calling.")
            'END: MMR | 2017.05.05 | YRS-AT-3205 | Commented existing code as not required

            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: InsertPrintLetters Method Calling.")
            participantTable = CreateParticipantTableSchema()
            If HelperFunctions.isNonEmpty(selectedBatchRecords) Then
                For iProcessCount = count To selectedBatchRecords.Rows.Count - 1
                    If iProcessCount - count >= batchSize Then
                        Exit For
                    End If
                    participantRow = participantTable.NewRow()
                    participantRow("PersonId") = selectedBatchRecords.Rows(iProcessCount)("PersonId")
                    participantRow("RefRequestID") = selectedBatchRecords.Rows(iProcessCount)("RefRequestID")
                    participantRow("SSNo") = selectedBatchRecords.Rows(iProcessCount)("SSNo")
                    participantRow("FirstName") = selectedBatchRecords.Rows(iProcessCount)("FirstName")
                    participantRow("FUNDNo") = selectedBatchRecords.Rows(iProcessCount)("FUNDNo")
                    participantRow("LastName") = selectedBatchRecords.Rows(iProcessCount)("LastName")
                    participantRow("MiddleName") = selectedBatchRecords.Rows(iProcessCount)("MiddleName")
                    participantRow("LetterCode") = selectedBatchRecords.Rows(iProcessCount)("LetterCode")
                    participantRow("PrintLetterID") = YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(selectedBatchRecords.Rows(iProcessCount)("FUNDNo").ToString, selectedBatchRecords.Rows(iProcessCount)("PersonId").ToString, selectedBatchRecords.Rows(iProcessCount)("LetterCode").ToString, String.Format("PlanType={0}", selectedBatchRecords.Rows(iProcessCount)("PlanType").ToString)) 'MMR | 2017.05.05 | YRS-AT-3205 | Getting print letter ID

                    'START: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Passing FundEventID and MRD Amount
                    If selectedBatchRecords.Columns.Contains("FundEventId") Then
                        participantRow("FundEventId") = selectedBatchRecords.Rows(iProcessCount)("FundEventId")
                    End If
                    If selectedBatchRecords.Columns.Contains("MRDAmount") Then
                        participantRow("MRDAmount") = selectedBatchRecords.Rows(iProcessCount)("MRDAmount")
                    End If
                    'END: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Passing FundEventID and MRD Amount

                    selectedBatchRecords.Rows(iProcessCount)("IsReportPrinted") = 1
                    selectedBatchRecords.Rows(iProcessCount)("PrintLetterID") = participantRow("PrintLetterID") 'MMR | 2017.05.05 | YRS-AT-3205 | setting print letter id
                    participantTable.Rows.Add(participantRow)
                Next
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: InsertPrintLetters Method Calling.") 'MMR | 2017.05.05 | YRS-AT-3205 | Added log trace for error

            Dim columnNameToDistinct As String() = {"LetterCode"}
            Dim distinctRecord As DataTable = HelperFunctions.GetDistinctRecords(participantTable, columnNameToDistinct)
            For l As Integer = 0 To distinctRecord.Rows.Count - 1
                combinedPlanTypeRecord = GetFilterByLetterCode(participantTable, distinctRecord.Rows(l)("LetterCode").ToString())
                If HelperFunctions.isNonEmpty(combinedPlanTypeRecord) Then
                    docType = String.Empty
                    letterCode = String.Empty
                    reportName = String.Empty
                    outputFileType = String.Empty
                    param1 = String.Empty
                    If moduleName = CommonClass.BatchProcess.RMDInitLetters.ToString Then
                        param1 = "RMDPrintLetter"
                        If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDCSHLT.ToString() Then
                            docType = CommonClass.RPTLetter.RMDCSHLT.ToString()
                            letterCode = CommonClass.RPTLetter.RMDCSHLT.ToString()
                            reportName = "RMD CashOut Letter.rpt"
                            outputFileType = "RMD_CashOut_Letter_" + CommonClass.RPTLetter.RMDCSHLT.ToString()
                        Else
                            letterCode = CommonClass.RPTLetter.RMDINIT.ToString()
                            docType = "RMDINTLT"
                            reportName = "RMD Initial Letter.rpt"
                            outputFileType = "RMD_Initial_Letter_" + docType
                        End If
                    ElseIf moduleName = CommonClass.BatchProcess.RMDFollwLetters.ToString Then
                        'START : SB | 2016.11.21 | YRS-AT-3203 | changes made for Followup cashout letters --Previous code is commented   
                        param1 = "RMDPrintLetter"
                        If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDCFLLT.ToString() Then
                            docType = CommonClass.RPTLetter.RMDCFLLT.ToString()
                            letterCode = CommonClass.RPTLetter.RMDCFLLT.ToString()
                            reportName = "RMD CashOut Followup Letter.rpt"
                            outputFileType = "RMD_CashOut_Followup_Letter_" + CommonClass.RPTLetter.RMDCFLLT.ToString()
                        Else
                            docType = "RMDFLWLT"
                            letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString()
                            reportName = "RMD Followup Letter.rpt"
                            outputFileType = "RMD_Followup_Letter_" + docType
                        End If
                        'START: MMR | 2017.05.03 | YRS-AT-3205 | Added code to set report details like document type, letter code etc for RMD Special QDRO Letters
                    ElseIf moduleName = CommonClass.BatchProcess.RMDSpecialQDInitLetters.ToString Then
                        param1 = "RMDSpecialQDROLetters"
                        If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDIAPLT.ToString() Then
                            docType = IDMDocumentCodes.RMD_SpecialQD_InitialLetter.ToString()
                            letterCode = CommonClass.RPTLetter.RMDIAPLT.ToString()
                            reportName = "RMDSpecialQDInitialLetter.rpt"
                            outputFileType = "RMD_SplQD_Initial_Letter_" + CommonClass.RPTLetter.RMDIAPLT.ToString()
                        Else
                            docType = IDMDocumentCodes.RMD_SpecialQD_InitialLetter.ToString()
                            letterCode = CommonClass.RPTLetter.RMDIAPLT.ToString()
                            reportName = "RMDSpecialQDInitialLetter.rpt"
                            outputFileType = "RMD_SplQD_Initial_Letter_" + CommonClass.RPTLetter.RMDIAPLT.ToString()
                        End If
                    ElseIf moduleName = CommonClass.BatchProcess.RMDSpecialQDFollowLetters.ToString Then
                        param1 = "RMDSpecialQDROLetters"
                        If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDFAPLT.ToString() Then
                            docType = IDMDocumentCodes.RMD_SpecialQD_FollowupLetter.ToString()
                            letterCode = CommonClass.RPTLetter.RMDFAPLT.ToString()
                            reportName = "RMDSpecialQDFollowupLetter.rpt"
                            outputFileType = "RMD_SplQD_Followup_Letter_" + CommonClass.RPTLetter.RMDFAPLT.ToString()
                        Else
                            docType = IDMDocumentCodes.RMD_SpecialQD_FollowupLetter.ToString()
                            letterCode = CommonClass.RPTLetter.RMDFAPLT.ToString()
                            reportName = "RMDSpecialQDFollowupLetter.rpt"
                            outputFileType = "RMD_SplQD_Followup_Letter_" + CommonClass.RPTLetter.RMDFAPLT.ToString()
                        End If
                        'END: MMR | 2017.05.03 | YRS-AT-3205 | Added code to set report details like document type, letter code etc for RMD Special QDRO Letters
                        'START: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Handling RMDPrintLetterForNonRespondent and RMDPrintLetterForSatisfiedNotElected report conditions
                    ElseIf moduleName = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString Then
                        param1 = "RMDPrintLetterForNonRespondent"
                        If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDNRLET.ToString() Then
                            docType = CommonClass.RPTLetter.RMDNRLET.ToString()
                            letterCode = CommonClass.RPTLetter.RMDNRLET.ToString()
                            reportName = "RMDReminderToNonRespondent.rpt"
                            outputFileType = String.Format("RMD_NonRespondent_Letter_{0}", CommonClass.RPTLetter.RMDNRLET.ToString())
                        End If
                    ElseIf moduleName = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString Then
                        param1 = "RMDPrintLetterForSatisfiedButNotElected"
                        If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDERLET.ToString() Then
                            docType = CommonClass.RPTLetter.RMDERLET.ToString()
                            letterCode = CommonClass.RPTLetter.RMDERLET.ToString()
                            reportName = "RMDReminderToAnnualElection.rpt"
                            outputFileType = String.Format("RMD_SatisfiedNotElectedAnnual_Letter_{0}", CommonClass.RPTLetter.RMDERLET.ToString())
                        End If
                        'END: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Handling RMDPrintLetterForNonRespondent and RMDPrintLetterForSatisfiedNotElected report conditions
                    End If
                    batchProcess.InvokeBatchRequestCreation(0, combinedPlanTypeRecord, docType, reportName, outputFileType, param1, errorDataList, tempFileList, idxCreated, pdfCreated)
                    UpdateFileList(tempFileList, fileList, letterCode, docType)
                    tempFileList = Nothing
                End If
            Next

            If selectedBatchRecords.Rows.Count > result.iProcessCount + batchSize Then
                If HelperFunctions.isNonEmpty(fileList) Then
                    If Not tempData.Tables.Contains("dtFileList") Then
                        fileList.TableName = "dtFileList"
                        tempData.Tables.Add(fileList)
                    End If
                    tempData.Tables("dtFileList").Merge(fileList)
                End If
                result.iProcessCount += batchSize
                Return result
            End If

            If result.iProcessCount = selectedBatchRecords.Rows.Count Then
                result.strretValue = "success"
            Else
                result.strretValue = "pending"
            End If
            result.iProcessCount = selectedBatchRecords.Rows.Count

            fundNo = String.Empty
            If HelperFunctions.isNonEmpty(tempData) Then
                If HelperFunctions.isNonEmpty(fileList) Then
                    If Not tempData.Tables.Contains("dtFileList") Then
                        fileList.TableName = "dtFileList"
                        tempData.Tables.Add(fileList)
                    End If
                    tempData.Tables("dtFileList").Merge(fileList)

                    links = New List(Of LinkDetail)()
                    Web.HttpContext.Current.Session("MergedPdf_Filename") = Nothing

                    distinctRecord = HelperFunctions.GetDistinctRecords(tempData.Tables("SelectedBatchRecords"), columnNameToDistinct)
                    For l As Integer = 0 To distinctRecord.Rows.Count - 1
                        combinedDataList = GetFilterByLetterCode(tempData.Tables("dtFileList"), distinctRecord.Rows(l)("LetterCode").ToString())
                        reportFileName = GetReportName(distinctRecord.Rows(l)("LetterCode").ToString())
                        If tempData.Tables("dtFileList").Rows.Count > 0 Then
                            batchProcess.MergePDFs(combinedDataList, reportFileName, batchID)
                        End If

                        link = SetFilePath(distinctRecord.Rows(l)("LetterCode").ToString(), Web.HttpContext.Current.Session("MergedPdf_Filename"))
                        If Not link Is Nothing Then
                            links.Add(link)
                        End If
                    Next
                    result.Links = links
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RMD Print Letters Process", ex)
            result.strretValue = "error"
            errorDataList.Add(New ExceptionLog("Exception", "Process Method", ex.Message))
            Return result
        Finally
            dtArrErrorDataList = New DataTable("ArrErrorDataList")
            dtArrErrorDataList.Columns.Add("FundNo")
            dtArrErrorDataList.Columns.Add("Errors")
            dtArrErrorDataList.Columns.Add("Description")
            Dim dr As DataRow
            For Each exlog As ExceptionLog In errorDataList
                dr = dtArrErrorDataList.NewRow()
                dr("FundNo") = exlog.FundNo
                dr("Errors") = exlog.Errors
                dr("Description") = exlog.Decription
                dtArrErrorDataList.Rows.Add(dr)
            Next
            If tempData.Tables.Contains("ArrErrorDataList") Then
                tempData.Tables.Remove("ArrErrorDataList")
            End If
            dtArrErrorDataList.TableName = "ArrErrorDataList"
            tempData.Tables.Add(dtArrErrorDataList)

            If HelperFunctions.isNonEmpty(selectedBatchRecords) Then
                Dim dv As New DataView(selectedBatchRecords)
                dv.RowFilter = "IsReportPrinted = 1"

                result.iTotalCount = selectedBatchRecords.Rows.Count
                result.iIdxCreated = idxCreated
                result.iPdfCreated = pdfCreated
                result.iTotalIDXPDFCount = dv.Count
            End If
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(batchID, moduleName, tempData)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Process Method Calling.")
        End Try
        Return result
    End Function

    'Start:'Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
    ''' <summary>
    ''' GetBatchSize will get the batch size for batch creation process.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBatchSize() As Integer
        Dim dsbatchSize As DataSet
        Dim intBatchsize As Integer
        Try
            dsbatchSize = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("BATCH_SIZE")
            If HelperFunctions.isNonEmpty(dsbatchSize) Then
                intBatchsize = dsbatchSize.Tables(0).Rows(0)("Value")
            Else
                intBatchsize = 2
            End If
            Return intBatchsize
        Catch
            Throw
        End Try
    End Function
    'End: Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations

    'START : CS | 2016.10.24 | YRS-AT-3088 | Filter the record based on the Letter code 
    Private Function GetFilterByLetterCode(ByVal fileRecords As DataTable, ByVal letterCode As String) As DataTable
        Dim filelist As DataRow()
        Dim filelistrows As DataRow
        Dim combinedPlanTypeRecord As DataTable
        filelist = fileRecords.Select(String.Format("LetterCode='{0}'", letterCode))
        combinedPlanTypeRecord = fileRecords.Clone()
        For Each drRecp As DataRow In filelist
            filelistrows = combinedPlanTypeRecord.NewRow
            filelistrows.ItemArray = drRecp.ItemArray
            combinedPlanTypeRecord.Rows.Add(filelistrows)
        Next
        Return combinedPlanTypeRecord
    End Function
    'END : CS | 2016.10.24 | YRS-AT-3088 | Filter the record based on the Letter code 

    'START : CS | 2016.10.24 | YRS-AT-3088 | Updated the Letter code in the File list data table
    Private Sub UpdateFileList(ByVal fileListCurrent As DataTable, ByRef dtFileList As DataTable, ByVal letterCode As String, ByVal reportDocCode As String)
        Dim Rows As DataRow
        Dim fileRecordRowsExists As DataRow()
        If HelperFunctions.isNonEmpty(fileListCurrent) Then
            If Not HelperFunctions.isNonEmpty(dtFileList) Then
                dtFileList = fileListCurrent.Clone()
            End If

            If Not dtFileList.Columns.Contains("Lettercode") Then
                dtFileList.Columns.Add("Lettercode", System.Type.GetType("System.String"))
            End If
            'For Each drRecp As DataRow In fileListCurrent.Rows

            fileRecordRowsExists = fileListCurrent.Select(String.Format("SourceFile LIKE '%{0}%'", reportDocCode))
            For Each drRecps1 As DataRow In fileRecordRowsExists
                Rows = dtFileList.NewRow
                Rows("FundNo") = drRecps1("FundNo").ToString()
                Rows("DestFolder") = drRecps1("DestFolder")
                Rows("DestFile") = drRecps1("DestFile")
                Rows("SourceFile") = drRecps1("SourceFile")
                Rows("SourceFolder") = drRecps1("SourceFolder")
                Rows("Lettercode") = letterCode
                dtFileList.Rows.Add(Rows)
            Next
            dtFileList.AcceptChanges()
        End If
    End Sub
    'END : CS | 2016.10.24 | YRS-AT-3088 | Updated the Letter code in the File list data table

    'START : CS | 2016.10.24 | YRS-AT-3088 | Return the Report File Name based on letter code
    Private Function GetReportName(ByVal letterCode As String) As String
        Dim reportfileName As String = String.Empty
        If letterCode = CommonClass.RPTLetter.RMDCSHLT.ToString() Then
            reportfileName = "RMD_Cash_Out_Letter"
        ElseIf letterCode = CommonClass.RPTLetter.RMDINIT.ToString() Then
            reportfileName = "RMD_Initial_Letter"
        ElseIf letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString() Then
            reportfileName = "RMD_Followup_Letter"
            'START : SB | 2016.11.21 | YRS-AT-3203 | Setting cashout follow up report file name
        ElseIf letterCode = CommonClass.RPTLetter.RMDCFLLT.ToString() Then
            reportfileName = "RMD_CashOut_Followup_Letter"
            'END   : SB | 2016.11.21 | YRS-AT-3203 | Setting cashout follow up report file name
            'START: MMR | 2017.05.03 | YRS-AT-3205 | Added code to set report name for RMD Special QDRO Letters
        ElseIf letterCode = CommonClass.RPTLetter.RMDIAPLT.ToString() Then
            reportfileName = "RMD_SplQD_Initial_Letter"
        ElseIf letterCode = CommonClass.RPTLetter.RMDFAPLT.ToString() Then
            reportfileName = "RMD_SplQD_Followup_Letter"
            'END: MMR | 2017.05.03 | YRS-AT-3205 | Added code to set report name for RMD Special QDRO Letters
            'START: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Handling RMDPrintLetterForNonRespondent and RMDPrintLetterForSatisfiedNotElected report conditions
        ElseIf letterCode = CommonClass.RPTLetter.RMDERLET.ToString() Then
            reportfileName = "RMDReminderToAnnualElection"
        ElseIf letterCode = CommonClass.RPTLetter.RMDNRLET.ToString() Then
            reportfileName = "RMDReminderToNonRespondent"
            'END: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Handling RMDPrintLetterForNonRespondent and RMDPrintLetterForSatisfiedNotElected report conditions
        End If
        Return reportfileName
    End Function
    'END : CS | 2016.10.24 | YRS-AT-3088 | Return the Report File Name based on letter code

    Private Function SetFilePath(ByVal letterCode As String, ByVal mergedPdfFileName As String) As LinkDetail
        Dim encryptLink As EncryptedQueryString
        Dim link As LinkDetail
        Dim displayText As String
        Dim pathAndFileName, relativePath As String
        Try
            pathAndFileName = String.Format("{0}\{1}", Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), mergedPdfFileName)
            relativePath = String.Format("{0}\\{1}", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), mergedPdfFileName)
            displayText = String.Empty

            If Not FileIO.FileSystem.FileExists(pathAndFileName) Then
                ' After moving code to user control, Server.MapPath is containing "UserControls" folder also in file address. So replacing it.
                If (pathAndFileName.IndexOf("\UserControls") >= 0) Then
                    pathAndFileName = pathAndFileName.Replace("\UserControls", "")
                End If
            End If

            link = Nothing
            If FileIO.FileSystem.FileExists(pathAndFileName) Then
                If letterCode = CommonClass.RPTLetter.RMDCSHLT.ToString() Then
                    displayText = "Print RMD Cashout Letter"
                ElseIf letterCode = CommonClass.RPTLetter.RMDINIT.ToString() Then
                    displayText = "Print RMD Initial Letter"
                ElseIf letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString() Then
                    displayText = "Print RMD Followup Letter"
                ElseIf letterCode = CommonClass.RPTLetter.RMDCFLLT.ToString() Then
                    displayText = "Print RMD Cashout Followup Letter"
                    'START: MMR | 2017.05.03 | YRS-AT-3205 | Added code to set link text for RMD Special QDRO initial/Followup Letters
                ElseIf letterCode = CommonClass.RPTLetter.RMDIAPLT.ToString() Then
                    displayText = "Print Special QDRO Participants Initial Letter"
                ElseIf letterCode = CommonClass.RPTLetter.RMDFAPLT.ToString() Then
                    displayText = "Print RMD Special QDRO Participants Follow-up Letter" 'START: MMR | 2017.05.16 | YRS-AT-3356 | Added "-" in text
                    'END: MMR | 2017.05.03 | YRS-AT-3205 | Added code to set link text for RMD Special QDRO initial/Followup Letters
                    'START: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Handling RMDPrintLetterForNonRespondent and RMDPrintLetterForSatisfiedNotElected report conditions
                ElseIf letterCode = CommonClass.RPTLetter.RMDNRLET.ToString() Then
                    displayText = "Print Annual Letter for Non-Respondents"
                ElseIf letterCode = CommonClass.RPTLetter.RMDERLET.ToString() Then
                    displayText = "Print Satisfied RMD but Annual not Elected"
                    'END: SB | 05/04/2017 | YRS-AT-3400 & 3401 | Handling RMDPrintLetterForNonRespondent and RMDPrintLetterForSatisfiedNotElected report conditions
                End If

                encryptLink = New EncryptedQueryString()
                encryptLink.Add("link", relativePath)

                link = New LinkDetail()
                link.DisplayText = displayText
                link.URL = String.Format("DocumentViewer.aspx?p={0}", encryptLink.ToString())
            End If
            Return link
        Catch
            Throw
        Finally
            relativePath = Nothing
            pathAndFileName = Nothing
            displayText = Nothing
            link = Nothing
            encryptLink = Nothing
        End Try
    End Function

    Private Function CreateParticipantTableSchema() As DataTable
        Dim Schema As New DataTable
        Schema.Columns.Add("PersonId")
        Schema.Columns.Add("RefRequestID")
        Schema.Columns.Add("FUNDNo")
        Schema.Columns.Add("SSNo")
        Schema.Columns.Add("FirstName")
        Schema.Columns.Add("LastName")
        Schema.Columns.Add("MiddleName")
        Schema.Columns.Add("LetterCode")
        Schema.Columns.Add("PrintLetterID") 'MMR | 2017.05.05 | YRS-AT-3205 | Added column for print letter ID
        'START: SB | 05/04/2017 | YRS-AT-3400 & 3401 | FundEventID and MRDAmount required for non respondent letter
        Schema.Columns.Add("FundEventId")
        Schema.Columns.Add("MRDAmount")
        'END: SB | 05/04/2017 | YRS-AT-3400 & 3401 | FundEventID and MRDAmount required for non respondent letter
        Return Schema
    End Function
End Class
