'***************
'Added by - Dinesh Kanojia
'Added on - 15 Jun,2015
'Change description - Added code to generate request for special cashout.
'***************
'**********************************************************************************************************************
'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(MM/DD/YYYY)       Issue ID          Description  
'**********************************************************************************************************************
'   Sanjay S.          2015.07.01           YRS 5.0-2523        Create script to populate tables for Release blanks
'   Manthan Rajguru    2015.09.16           YRS-AT-2550         YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'   Anudeep A          2015.10.21           YRS-AT-2463         Cashout utility for participants with two plans. One release blank rather than two per participant (TrackIT 21783)
'**********************************************************************************************************************
Imports System.IO
Imports YMCARET.YmcaBusinessObject

Public Class SpecialCashout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim LabelModuleName As Label
            LabelModuleName = Master.FindControl("LabelModuleName")
            If LabelModuleName IsNot Nothing Then
                LabelModuleName.Text = "Special Cashout"
            End If
            If Not Session("SpecialCashoutprocess") Is Nothing Then
                If Convert.ToBoolean(Session("SpecialCashoutprocess")) Then
                    If Not Request.QueryString("batchId") Is Nothing Then
                        HelperFunctions.ShowMessageToUser("Special CashOut Batch ID: " + Request.QueryString("batchId") + " processed successfully.", EnumMessageTypes.Success)
                    End If
                End If
                Session("special") = Nothing
                Session("strSpecialBatchId") = Nothing
                Session("SpecialCashoutprocess") = Nothing
                Session("dtSelectedSpecialBatchRecords") = Nothing
            End If

            Dim dtBatchRecords As DataTable
            Dim objCashoutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass
            Dim dv As DataView
            Try
                objCashoutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
                'Anudeep A:2015.10.21 YRS-AT-2463 Added a to filter as per the cashout range
                dtBatchRecords = objCashoutBOClass.GetDataTableCashoutBatchRecords("SPECIAL CASHOUT")
                If HelperFunctions.isNonEmpty(dtBatchRecords) Then
                    dv = dtBatchRecords.DefaultView
                    'dv.RowFilter = "chvCashOutRangeDesc = 'SPECIAL CASHOUT' " 'Anudeep A:2015.10.21 YRS-AT-2463 Commented as already filtering done in stored procedure
                    dv.Sort = "chvBatchid DESC"
                    rptCashoutBatchId.DataSource = dv.ToTable
                    rptCashoutBatchId.DataBind()
                End If
            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("lnkProcessStatus_Click", ex)
            End Try
        End If
    End Sub

#Region "Events"
#Region "Click Events"
    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Dim strFileName As String = ""
        Dim strFileContent As String = ""
        Dim ds As New DataSet
        Dim strValidFileType() As String = {"txt", "csv"}
        Dim strExtention As String = ""
        Dim bIsValid As Boolean = False
        If FileUpld.HasFile Then
            strFileName = FileUpld.FileName.ToString()
            strExtention = System.IO.Path.GetExtension(FileUpld.PostedFile.FileName)
            For i As Integer = 0 To strValidFileType.Length - 1
                If strExtention = "." + strValidFileType(i) Then
                    bIsValid = True
                    Exit For
                End If
            Next
            If bIsValid = False Then
                HelperFunctions.ShowMessageToUser("Invalid File. Please upload a files extension " + String.Join(", ", strValidFileType), EnumMessageTypes.Error)
                Exit Sub
            End If

            Using reader As StreamReader = New StreamReader(FileUpld.PostedFile.InputStream)
                strFileContent = reader.ReadToEnd().Replace(",\r\n", ",").ToString()
                Dim objCashOutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass = New CashOutBOClass
                ds = objCashOutBOClass.GetSpecialEligibleParticipants(strFileContent)
                If (HelperFunctions.isNonEmpty(ds)) Then
                    lblCount.Text = "No. of Records to be processed:" + ds.Tables(0).Rows.Count.ToString
                    gvCashoutList.DataSource = ds.Tables(0)
                    gvCashoutList.DataBind()
                    Session("dtSelectedSpecialBatchRecords") = ds.Tables(0)
                Else
                    gvCashoutList.DataSource = Nothing
                    gvCashoutList.DataBind()
                    lblCount.Text = "No. of Records to be processed: 0"
                End If
            End Using
            tdCashoutBatchId.Visible = False
        Else
            tdCashoutBatchId.Visible = True
        End If
    End Sub

    Protected Sub btnRequest_Click(sender As Object, e As EventArgs) Handles btnRequest.Click
        Dim dtSelectedBatchRecords As DataTable
        dtSelectedBatchRecords = CType(Session("dtSelectedSpecialBatchRecords"), DataTable)
        If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
            If dtSelectedBatchRecords.Rows.Count > 0 Then
                lblConfirmMessage.Text = "Are you sure you want to create a batch for listed " + dtSelectedBatchRecords.Rows.Count.ToString() + " person(s)"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript3", "showConfirmdialog()", True)
            Else
                HelperFunctions.ShowMessageToUser("List of person(s) not found to create a batch.", EnumMessageTypes.Error)
            End If
        Else
            HelperFunctions.ShowMessageToUser("List of person(s) not found to create a batch.", EnumMessageTypes.Error)
        End If
    End Sub

    Protected Sub lnkCashoutBatchId_Click(sender As Object, e As EventArgs)
        Dim strBatchId As String
        If Not sender Is Nothing AndAlso sender.Text() IsNot Nothing Then
            strBatchId = sender.Text()
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Response.Redirect("SpecialCashout.aspx", False)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Session("special") = "special"
        Session("SpecialCashoutprocess") = True
        Response.Redirect("SpecialCashout.aspx?batchId=" + Session("strSpecialBatchId"), False)
    End Sub

#End Region

#Region "Grid Controls"

    Private Sub rptCashoutBatchId_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptCashoutBatchId.ItemCommand
        Dim BatchID As String = ""
        If e.CommandName = "CashoutBatchId" Then
            BatchID = e.CommandArgument
            Session("SpecialPrintBatchID") = BatchID
            If (Not Session("SpecialPrintBatchID") Is Nothing AndAlso Session("SpecialPrintBatchID").ToString().Length > 1) Then
                Session("strReportName") = "Withdrawals_PortableProject"
                Session("strModuleName") = "CashOut"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openReportViewer", "openReportViewer();", True)
            End If
        End If
    End Sub

    Private Sub rptCashoutBatchId_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptCashoutBatchId.ItemDataBound
        Dim lnkRMDProcessBatchId As LinkButton
        Dim strBatchId As String
        Dim htmlGenericControl As HtmlTableCell
        Try
            If e.Item.ItemIndex = 0 Then
                lnkRMDProcessBatchId = e.Item.FindControl("lnkCashoutBatchId")
                strBatchId = lnkRMDProcessBatchId.Text.Trim
                htmlGenericControl = e.Item.FindControl("liCashoutBatchId")
                htmlGenericControl.Attributes("class") = "tabSelectedLink"
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("rptRMDBatchId_ItemDataBound", ex)
        End Try
    End Sub

#End Region
#End Region

#Region "Function and Methods"

#Region "Web Methods"
    <System.Web.Services.WebMethod()> _
    Public Shared Function CashOutBatchCreationProcess(ByVal strBatchId As String, ByVal strReportType As String, ByVal strCashOutType As String, ByVal iCount As Integer, ByVal strProcessName As String, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim strretval As String = String.Empty
        Dim dsTemp As DataSet
        Dim objReturnStatusValues = New ReturnStatusValues()
        Dim objCashOut As New SpecialCashout

        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Cashout batch creation process")
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Pull data from batch creation log table")
            dsTemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Finish: Pull data from batch creation log table")
            objReturnStatusValues = objCashOut.Process(strBatchId, strReportType, strCashOutType, iCount, strProcessName, dsTemp, iIDXCreated, iPDFCreated, strModule)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Pull data from cache")
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)
            HttpContext.Current.Cache(strBatchId) = dsTemp
            If HelperFunctions.isNonEmpty(dsTemp.Tables("dtFileList")) Then
                HttpContext.Current.Session("FTFileList") = dsTemp.Tables("dtFileList")
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Finish: Cashout batch creation process")
        Catch ex As Exception

        End Try
        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Return objReturnStatusValues
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function StartCashoutProcess() As String
        Dim strBatchId As String = String.Empty
        Dim strReportProcessName As String
        Dim dsCashOutTemp As New DataSet
        Dim dt As DataTable
        Dim strCashOutType As String
        Dim XmlSelectedPersDetails As String
        Try
            If Not HttpContext.Current.Session("dtSelectedSpecialBatchRecords") Is Nothing Then

                HttpContext.Current.Session("g_bool_ProcessOK") = True
                HttpContext.Current.Session("SegregateDatatable") = Nothing

                XmlSelectedPersDetails = GetXmlPerDetailsFromDataSet(HttpContext.Current.Session("dtSelectedSpecialBatchRecords"))
                If Not String.IsNullOrEmpty(XmlSelectedPersDetails) Then
                    ExpiredRefRequestsForSelectedPerson(XmlSelectedPersDetails)
                End If

                Dim strModule As String = "CashOutBatchCreation"
                strBatchId = CType(HttpContext.Current.Session("dtSelectedSpecialBatchRecords"), DataTable).Rows(0)("BatchId")
                dt = CType(HttpContext.Current.Session("dtSelectedSpecialBatchRecords"), DataTable)
                Dim dt1 As New DataTable
                dt1 = dt.Copy
                dsCashOutTemp.Tables.Add(dt1)
                strReportProcessName = "BIE"
                strCashOutType = "special"
                YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsCashOutTemp)
                HttpContext.Current.Session("strSpecialBatchId") = strBatchId
                Return strBatchId + "," + strReportProcessName + "," + strModule + "," + strCashOutType
            Else
                Return "fail"
                Throw New Exception("Records not selected for batch creation.")
                HelperFunctions.ShowMessageToUser("Please select the person(s) to create a batch.", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnYes_Click", ex)
        End Try
    End Function

#End Region

#Region "Calling Functions"

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

    Private Function Process(ByVal strBatchId As String, ByVal strReportType As String, ByVal strCashOutType As String, ByVal iCount As Integer, ByVal strProcessName As String, dsTemp As DataSet, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim blnSuccess As Boolean = True
        Dim strProgressStatus As String = String.Empty
        Dim dtFileListSource As New DataTable
        Dim strVal As String = String.Empty
        Dim i As Integer = 0
        Dim strReturn As String = String.Empty
        Dim drtemp As DataRow
        Dim objReturnStatusValues As New ReturnStatusValues
        Dim dtSelectedBatchRecords As New DataTable
        Dim objBatchProcess As New BatchRequestCreation
        Dim ArrErrorDataList = New List(Of ExceptionLog)
        Dim dtFileList As New DataTable
        Dim l_bool_status As Boolean = False
        Dim dttemp As New DataTable
        Dim BatchSize As Integer = GetBatchSize()
        Dim iProcessCount As Integer = 0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CashOutBatchCreationProcess", "Start: Cashout batch creation process")
            objReturnStatusValues.strBatchId = strBatchId
            objReturnStatusValues.strReportType = strReportType
            objReturnStatusValues.iProcessCount = iCount
            objReturnStatusValues.strretValue = "pending"
            objReturnStatusValues.iIdxCreated = iIDXCreated
            objReturnStatusValues.iPdfCreated = iPDFCreated

            dtSelectedBatchRecords = dsTemp.Tables("SelectedBatchRecords")


            Dim dtArrErrorDataList As DataTable = dsTemp.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                For Each dr As DataRow In dtArrErrorDataList.Rows
                    ArrErrorDataList.Add(New ExceptionLog(dr("FundNo"), dr("Errors"), dr("Description")))
                Next
            End If

            If Not dtSelectedBatchRecords.Columns.Contains("IsReportPrinted") Then
                dtSelectedBatchRecords.Columns.Add("IsReportPrinted")
            End If

            dttemp = dtSelectedBatchRecords.Clone
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Start: Add participant in temp table.")
            For iProcessCount = iCount To dtSelectedBatchRecords.Rows.Count - 1
                If iProcessCount - iCount >= BatchSize Then
                    Exit For
                End If
                drtemp = dttemp.NewRow
                drtemp("PersonId") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonId").ToString
                drtemp("SSNO") = dtSelectedBatchRecords.Rows(iProcessCount)("SSNO").ToString
                drtemp("FUNDNo") = dtSelectedBatchRecords.Rows(iProcessCount)("FUNDNo").ToString
                drtemp("FirstName") = dtSelectedBatchRecords.Rows(iProcessCount)("FirstName").ToString
                drtemp("LastName") = dtSelectedBatchRecords.Rows(iProcessCount)("LastName").ToString
                drtemp("MiddleName") = dtSelectedBatchRecords.Rows(iProcessCount)("MiddleName").ToString
                drtemp("Name") = dtSelectedBatchRecords.Rows(iProcessCount)("Name").ToString
                drtemp("PersonAgeDOB") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonAgeDOB").ToString
                drtemp("MaxTermDate") = dtSelectedBatchRecords.Rows(iProcessCount)("MaxTermDate").ToString
                drtemp("FundEventId") = dtSelectedBatchRecords.Rows(iProcessCount)("FundEventId").ToString
                drtemp("IsTerminated") = dtSelectedBatchRecords.Rows(iProcessCount)("IsTerminated").ToString
                drtemp("IsVested") = dtSelectedBatchRecords.Rows(iProcessCount)("IsVested").ToString
                drtemp("PersonAge") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonAge").ToString 'SR:2015.07.01 - YRS 5.0-2523: Added to calaulate age to derive BAMaxLimit
                drtemp("IntAddressId") = dtSelectedBatchRecords.Rows(iProcessCount)("IntAddressId").ToString
                drtemp("EligibleBalance") = dtSelectedBatchRecords.Rows(iProcessCount)("EligibleBalance").ToString
                drtemp("TaxableAmount") = dtSelectedBatchRecords.Rows(iProcessCount)("TaxableAmount").ToString
                drtemp("StatusType") = dtSelectedBatchRecords.Rows(iProcessCount)("StatusType").ToString
                drtemp("PlansType") = dtSelectedBatchRecords.Rows(iProcessCount)("PlansType").ToString
                drtemp("BatchId") = dtSelectedBatchRecords.Rows(iProcessCount)("BatchId").ToString
                drtemp("Selected") = dtSelectedBatchRecords.Rows(iProcessCount)("Selected").ToString
                drtemp("LastContributionDate") = dtSelectedBatchRecords.Rows(iProcessCount)("LastContributionDate").ToString
                drtemp("IsHighlighted") = dtSelectedBatchRecords.Rows(iProcessCount)("IsHighlighted").ToString
                drtemp("RefRequestID") = dtSelectedBatchRecords.Rows(iProcessCount)("RefRequestID").ToString
                drtemp("IsRMDEligible") = dtSelectedBatchRecords.Rows(iProcessCount)("IsRMDEligible").ToString
                drtemp("chvShortDescription") = dtSelectedBatchRecords.Rows(iProcessCount)("chvShortDescription").ToString
                drtemp("Remarks") = dtSelectedBatchRecords.Rows(iProcessCount)("Remarks").ToString
                drtemp("mnyEstimatedBalance") = dtSelectedBatchRecords.Rows(iProcessCount)("mnyEstimatedBalance").ToString
                drtemp("IsReportPrinted") = "1"
                dtSelectedBatchRecords.Rows(iProcessCount)("IsReportPrinted") = "1"
                dttemp.Rows.Add(drtemp)
            Next
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Finish: Add participant in temp table.")
            If Not dttemp Is Nothing Then
                Dim objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Start: Creation of Refund request")
                objCashOutBOClass.CreateAndProcessRequest(dttemp, l_bool_status, strCashOutType)
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "End: Creation of Refund request")
            End If

            If HelperFunctions.isNonEmpty(dttemp) Then
                Dim l_stringDocType As String = String.Empty
                Dim l_StringReportName As String = String.Empty
                Dim l_string_OutputFileType As String = String.Empty

                l_stringDocType = "REFREQST"
                l_StringReportName = "Withdrawals_PortableProject.rpt"
                l_string_OutputFileType = "Withdrawal_REFREQST_" + l_stringDocType

                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "Start: Reuqest to create IDX and PDF files generation")
                objBatchProcess.InvokeBatchRequestCreation(0, dttemp, l_stringDocType, l_StringReportName, l_string_OutputFileType, strReportType, ArrErrorDataList, dtFileList, iIDXCreated, iPDFCreated)
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process Method", "End: Reuqest to create IDX and PDF files generation")
            End If

            If dtSelectedBatchRecords.Rows.Count > objReturnStatusValues.iProcessCount + BatchSize Then
                objReturnStatusValues.iProcessCount += BatchSize

                If HelperFunctions.isNonEmpty(dtFileList) Then
                    If Not dsTemp.Tables.Contains("dtFileList") Then
                        dtFileList.TableName = "dtFileList"
                        dsTemp.Tables.Add(dtFileList)
                    End If
                    dsTemp.Tables("dtFileList").Merge(dtFileList)
                End If
                Return objReturnStatusValues
            End If

            If objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count Then
                objReturnStatusValues.strretValue = "success"
                HttpContext.Current.Cache.Remove(strBatchId)
            Else
                objReturnStatusValues.strretValue = "pending"
            End If

            objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count

            If HelperFunctions.isNonEmpty(dtFileList) Then
                If Not dsTemp.Tables.Contains("dtFileList") Then
                    dtFileList.TableName = "dtFileList"
                    dsTemp.Tables.Add(dtFileList)
                End If
                dsTemp.Tables("dtFileList").Merge(dtFileList)
            End If


        Catch ex As Exception
            HelperFunctions.LogException("Process Cashout Batch Creation.", ex)
            objReturnStatusValues.strretValue = "error"
            ArrErrorDataList.Add(New ExceptionLog("Process Exception", "Process Cashout Batch Creation.", ex.Message))
            Return objReturnStatusValues
            Throw ex
        Finally
            Dim dtArrErrorDataList As New DataTable("ArrErrorDataList")
            dtArrErrorDataList.Columns.Add("FundNo")
            dtArrErrorDataList.Columns.Add("Errors")
            dtArrErrorDataList.Columns.Add("Description")
            Dim dr As DataRow
            For Each exlog As ExceptionLog In ArrErrorDataList
                dr = dtArrErrorDataList.NewRow()
                dr("FundNo") = exlog.FundNo
                dr("Errors") = exlog.Errors
                dr("Description") = exlog.Decription
                dtArrErrorDataList.Rows.Add(dr)
            Next
            If dsTemp.Tables.Contains("ArrErrorDataList") Then
                dsTemp.Tables.Remove("ArrErrorDataList")
            End If
            dtArrErrorDataList.TableName = "ArrErrorDataList"
            dsTemp.Tables.Add(dtArrErrorDataList)

            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                Dim dv As New DataView(dtSelectedBatchRecords)
                dv.RowFilter = "IsReportPrinted = 1"
                objReturnStatusValues.iTotalCount = dtSelectedBatchRecords.Rows.Count
                objReturnStatusValues.iIdxCreated = iIDXCreated
                objReturnStatusValues.iPdfCreated = iPDFCreated
                objReturnStatusValues.iTotalIDXPDFCount = dv.Count
            End If
        End Try
        Return objReturnStatusValues
    End Function

    Public Shared Function GetXmlPerDetailsFromDataSet(ByVal dtSelectedBatchRecords As DataTable) As String
        Dim sbOutput As StringBuilder
        Try
            sbOutput = New StringBuilder()
            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                sbOutput.Append("<PersonDetails>")
                For Each dr As DataRow In dtSelectedBatchRecords.Rows
                    sbOutput.Append("<Pers>")
                    sbOutput.Append("<FundEventId>" + dr("FundEventId").ToString + "</FundEventId>")
                    sbOutput.Append("<PlanType>" + dr("PlansType").ToString + "</PlanType>")
                    sbOutput.Append("</Pers>")
                Next
                sbOutput.Append("</PersonDetails>")
            End If
        Catch ex As Exception

        End Try
        Return sbOutput.ToString()
    End Function

    Public Shared Sub ExpiredRefRequestsForSelectedPerson(ByVal XmlSelectedPersDetails As String)
        Dim objCashOutDaClass As YMCARET.YmcaBusinessObject.CashOutBOClass
        Try
            objCashOutDaClass = New YMCARET.YmcaBusinessObject.CashOutBOClass()
            objCashOutDaClass.ExpiredRefRequestsForSelectedPerson(XmlSelectedPersDetails)
        Catch
            Throw
        End Try
    End Sub

#End Region

#End Region

    Private Sub gvCashoutList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCashoutList.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            HelperFunctions.SetSortingArrows(ViewState("SpecialCashout_sort"), e)
        End If
    End Sub

    Private Sub gvCashoutList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvCashoutList.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = DirectCast(Session("dtSelectedSpecialBatchRecords"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                HelperFunctions.gvSorting(ViewState("SpecialCashout_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvCashoutList, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("SpecialCashout_gvLetters_Sorting", ex)
        End Try
    End Sub
End Class