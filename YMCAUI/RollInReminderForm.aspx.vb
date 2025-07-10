'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	RollInReminderForm.aspx.vb
' Author Name		:	Anudeep  
' Creation Date		:	05/28/2014
' Description		:	This form is used to print reminder letter o letter of acceptance for open RollIn(s)
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Dinesh.k           08/25/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.
'Anudeep            09/25/2014      BT:2344:YRS 5.0-2279:Add in Administration Screen ability to change messages for YRS or Web site
'Anudeep            10/27/2014      BT:2691-Get the Merge PDF path from the configuration key
'Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
'Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations
'Dinesh Kanojia     01/06/2015      BT:2735:Add activities logging functionality in RMD and RollIn batch creation.
'Anudeep            05/08/2015      BT:2825:YRS 5.0-2500:Letter of Acceptance In YRS 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Imports YMCAUI.SessionManager
Imports YMCAObjects.CommonClass
Imports YMCAObjects.MetaMessageList
Imports YMCAObjects
Imports YMCARET.YmcaBusinessObject.MetaMessageBO

Public Class RollInReminderForm
    Inherits System.Web.UI.Page

    Protected Property dsOpenRollIns As DataSet
        Get
            Return SessionRollIns.RollInReminderForm_dsOpenRollIns
        End Get
        Set(value As DataSet)
            SessionRollIns.RollInReminderForm_dsOpenRollIns = value
        End Set
    End Property

    Protected Property objSortState As GridViewCustomSort
        Get
            Return SessionRollIns.RollInReminderForm_objSortState
        End Get
        Set(value As GridViewCustomSort)
            SessionRollIns.RollInReminderForm_objSortState = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Rollin Reminder Form page load", "Page Load Call.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            If Not IsPostBack Then
                ClearSessions()
                LoadRollIns()
            ElseIf Session("Rollin_MergedPdfs_Filename") IsNot Nothing Then
                Dim strFileName As String
                'Start:AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
                Dim strPath As String
                strFileName = Session("Rollin_MergedPdfs_Filename")
                strPath = System.Configuration.ConfigurationSettings.AppSettings("MergePDFPath") + "\\"
                If FileIO.FileSystem.FileExists(HttpContext.Current.Server.MapPath("~\" + strPath) + strFileName) Then
                    strFileName = strPath + strFileName
                    ScriptManager.RegisterStartupScript(Me, GetType(Page), "CallPdf", "OpenPDF('" + strFileName + "');", True)
                End If
                'End:AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
                ClearSessions()
                LoadRollIns()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Loads the all the open rollin(s) data and fill in grid , and binds the text which tells about how much time from the rollin(s) are open 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadRollIns()
        Try
            objSortState = Nothing
            dsOpenRollIns = YMCARET.YmcaBusinessObject.RollInReminderBOClass.GetOpenRollIns()
            HelperFunctions.BindGrid(gvRolloverRoll, dsOpenRollIns, True)
            If dsOpenRollIns IsNot Nothing AndAlso dsOpenRollIns.Tables.Count > 2 Then
                If dsOpenRollIns.Tables(2).Rows(0)("Datepart") = "D" Then
                    lblheadtext.Text = GetMessage(MESSAGE_ROLLIN_LIST_ROLLIN_FROM_LAST_DAYS, dsOpenRollIns.Tables(2).Rows(0)("DateValue"), "DAYS")
                ElseIf dsOpenRollIns.Tables(2).Rows(0)("Datepart") = "M" Then
                    lblheadtext.Text = GetMessage(MESSAGE_ROLLIN_LIST_ROLLIN_FROM_LAST_MONTHS, dsOpenRollIns.Tables(2).Rows(0)("DateValue"), "MONTH")
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> LoadRollIns", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function

    'Start:Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.
    <System.Web.Services.WebMethod()> _
    Public Shared Function RollInProcess(ByVal strBatchId As String, ByVal iCount As Integer, ByVal strProcessName As String, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim strretval As String = String.Empty
        Dim dsTemp As DataSet
        Dim objReturnStatusValues = New ReturnStatusValues()
        Dim objRollInPrintLetters As New RollInReminderForm
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("RollInProcess", "Start: Ajax Calling for Rollin.")
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RollIn Process", "Rollin Reminder letter Process Initiate for BatchID: " + strBatchId)
            dsTemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule)
            objReturnStatusValues = objRollInPrintLetters.Process(strBatchId, iCount, strProcessName, dsTemp, iIDXCreated, iPDFCreated, strModule)
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)

            If HelperFunctions.isNonEmpty(dsTemp.Tables("dtFileList")) Then
                HttpContext.Current.Session("FTFileList") = dsTemp.Tables("dtFileList")
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Process Letter", ex)
            objReturnStatusValues.strretValue = "error"
            Return objReturnStatusValues
            Throw ex
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("RollInProcess", "Finish: Ajax Calling for Rollin.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
        Return objReturnStatusValues
    End Function
    'Start:Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
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
    'End:Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
    Private Function Process(ByVal strBatchId As String, ByVal iCount As Integer, ByVal strProcessName As String, dsTemp As DataSet, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim blnSuccess As Boolean = True
        Dim strProgressStatus As String = String.Empty
        Dim dtFileListSource As DataTable
        Dim strVal As String = String.Empty
        Dim i As Integer = 0
        Dim strReturn As String = String.Empty
        Dim dttemp As DataTable
        Dim objReturnStatusValues As New ReturnStatusValues
        Dim dtSelectedBatchRecords As DataTable
        Dim objBatchProcess As New BatchRequestCreation
        Dim ArrErrorDataList = New List(Of ExceptionLog)
        Dim dtFileList As DataTable
        Dim strBatchError As String
        Dim dt As New DataTable
        Dim BatchSize As Integer = GetBatchSize() ''Start:Dinesh Kanojia         2014.12.10          BT-2732:Cashout observations
        Dim iProcessCount As Integer = 0
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: Process method calling.")
        Try
            objReturnStatusValues.strBatchId = strBatchId
            objReturnStatusValues.strReportType = strModule
            objReturnStatusValues.iProcessCount = iCount
            objReturnStatusValues.strretValue = "pending"
            objReturnStatusValues.iIdxCreated = iIDXCreated
            objReturnStatusValues.iPdfCreated = iPDFCreated

            dtSelectedBatchRecords = dsTemp.Tables("SelectedBatchRecords")

            Dim dtArrErrorDataList As DataTable = dsTemp.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                For Each drArr As DataRow In dtArrErrorDataList.Rows
                    ArrErrorDataList.Add(New ExceptionLog(drArr("FundNo"), drArr("Errors"), drArr("Description")))
                Next
            End If

            If Not dtSelectedBatchRecords.Columns.Contains("IsReportPrinted") Then
                dtSelectedBatchRecords.Columns.Add("IsReportPrinted")
            End If

            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: InsertPrintLetters method calling.")
            If Not dtSelectedBatchRecords Is Nothing Then
                For i = iCount To dtSelectedBatchRecords.Rows.Count - 1
                    If i - iCount >= BatchSize Then
                        Exit For
                    End If
                    'YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(dtSelectedBatchRecords.Rows(i)("FUNDNo").ToString, dtSelectedBatchRecords.Rows(i)("PersonId").ToString, dtSelectedBatchRecords.Rows(i)("LetterCode").ToString)
                    YMCARET.YmcaBusinessObject.RollInReminderBOClass.InsertPrintLetters(dtSelectedBatchRecords.Rows(i)("RefRequestID"))
                Next
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: InsertPrintLetters method calling.")

            Dim dr As DataRow
            dt.Columns.Add("PersonId")
            dt.Columns.Add("RefRequestID")
            dt.Columns.Add("FUNDNo")
            dt.Columns.Add("SSNo")
            dt.Columns.Add("FirstName")
            dt.Columns.Add("LastName")
            dt.Columns.Add("MiddleName")
            ' dt.Columns.Add("LetterCode")
            dt.Columns.Add("PartAccno")
            dt.Columns.Add("InstitutionName")
            dt.Columns.Add("addr1")
            dt.Columns.Add("addr2")
            dt.Columns.Add("addr3")
            dt.Columns.Add("city")
            dt.Columns.Add("StateName")
            dt.Columns.Add("zipCode")



            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                For iProcessCount = iCount To dtSelectedBatchRecords.Rows.Count - 1
                    If iProcessCount - iCount >= BatchSize Then
                        Exit For
                    End If
                    dr = dt.NewRow()
                    dr("PersonId") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonId")
                    dr("RefRequestID") = dtSelectedBatchRecords.Rows(iProcessCount)("RefRequestID")
                    dr("SSNo") = dtSelectedBatchRecords.Rows(iProcessCount)("SSNo")
                    dr("FirstName") = dtSelectedBatchRecords.Rows(iProcessCount)("FirstName")
                    dr("FUNDNo") = dtSelectedBatchRecords.Rows(iProcessCount)("FUNDNo")
                    dr("LastName") = dtSelectedBatchRecords.Rows(iProcessCount)("LastName")
                    dr("MiddleName") = dtSelectedBatchRecords.Rows(iProcessCount)("MiddleName")
                    'dr("LetterCode") = dtSelectedBatchRecords.Rows(iProcessCount)("LetterCode")
                    dr("PartAccno") = dtSelectedBatchRecords.Rows(iProcessCount)("PartAccno")
                    dr("InstitutionName") = dtSelectedBatchRecords.Rows(iProcessCount)("InstitutionName")
                    dr("addr1") = dtSelectedBatchRecords.Rows(iProcessCount)("addr1")
                    dr("addr2") = dtSelectedBatchRecords.Rows(iProcessCount)("addr2")
                    dr("addr3") = dtSelectedBatchRecords.Rows(iProcessCount)("addr3")
                    dr("city") = dtSelectedBatchRecords.Rows(iProcessCount)("city")
                    dr("StateName") = dtSelectedBatchRecords.Rows(iProcessCount)("StateName")
                    dr("zipCode") = dtSelectedBatchRecords.Rows(iProcessCount)("zipCode")
                    dtSelectedBatchRecords.Rows(iProcessCount)("IsReportPrinted") = 1
                    dt.Rows.Add(dr)
                Next
            End If
            If HelperFunctions.isNonEmpty(dt) Then
                Dim l_stringDocType As String = String.Empty
                Dim l_StringReportName As String = String.Empty
                Dim l_string_OutputFileType As String = String.Empty
                Dim strParam1 As String = String.Empty
                l_stringDocType = "ROLINLSR"
                l_StringReportName = "Letter of Acceptance.rpt"
                l_string_OutputFileType = "RollIn_ROLINLSR_" + l_stringDocType
                strParam1 = "RollIn"
                Session("strReportName") = l_StringReportName.Substring(0, l_StringReportName.Length - 4)
                strBatchError = objBatchProcess.InvokeBatchRequestCreation(0, dt, l_stringDocType, l_StringReportName, l_string_OutputFileType, strParam1, ArrErrorDataList, dtFileList, iIDXCreated, iPDFCreated)
                'Else
                '    Throw New Exception("Selected records not found.")
            End If

            If dtSelectedBatchRecords.Rows.Count > objReturnStatusValues.iProcessCount + BatchSize Then
                If HelperFunctions.isNonEmpty(dtFileList) Then
                    If Not dsTemp.Tables.Contains("dtFileList") Then
                        dtFileList.TableName = "dtFileList"
                        dsTemp.Tables.Add(dtFileList)
                    End If
                    dsTemp.Tables("dtFileList").Merge(dtFileList)
                End If
                objReturnStatusValues.iProcessCount += BatchSize
                Return objReturnStatusValues
            End If
            'Start: Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations

            If objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count Then
                objReturnStatusValues.strretValue = "success"
            Else
                objReturnStatusValues.strretValue = "pending"
            End If

            objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count

            'End: Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations

            Dim strFundNo As String = String.Empty

            If HelperFunctions.isNonEmpty(dsTemp) Then
                If HelperFunctions.isNonEmpty(dtFileList) Then
                    If Not dsTemp.Tables.Contains("dtFileList") Then
                        dtFileList.TableName = "dtFileList"
                        dsTemp.Tables.Add(dtFileList)
                    End If
                    dsTemp.Tables("dtFileList").Merge(dtFileList)
                    If dsTemp.Tables("dtFileList").Rows.Count > 0 Then
                        objBatchProcess.MergePDFs(dsTemp.Tables("dtFileList"))
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RMD Print Letters Process", ex)
            objReturnStatusValues.strretValue = "error"
            ArrErrorDataList.Add(New ExceptionLog("Exception", "Process Method", ex.Message))
            Return objReturnStatusValues
            Throw ex
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Process method calling.")
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
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)
        End Try
        Return objReturnStatusValues
    End Function
    'End:Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.

    ''' <summary>
    ''' To print the letters 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim lstRecords As List(Of Dictionary(Of String, String))
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnPrint_Click", "Start: Print Button Click.")
        Try
            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization("RollInReminderForm.aspx", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("RollInReminderPrint", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            lstRecords = GetSelectedRecords()
            If lstRecords.Count > 0 Then
                SessionRollIns.dsRecords_RollInReminderForm = lstRecords
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnPrint_Click", "Start: Calling Ajax.")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CallPopup", "ShowConfirmDialog('" + GetMessage(MESSAGE_ROLLIN_REMINDER_CONFIRM_PRINT, lstRecords.Count.ToString, "PERSONS") + "');", True)
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnPrint_Click", "Finish: Calling Ajax.")
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_REMINDER_SELECT_RECORD)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> btnPrint_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnPrint_Click", "Finish: Print Button Click.")

        End Try
    End Sub
    ''' <summary>
    ''' This method will gets list of selected records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedRecords() As List(Of Dictionary(Of String, String))
        Dim lstUserdetails As New List(Of Dictionary(Of String, String))
        Dim guiRolloverId As Guid
        Dim drRollIN As DataRow()
        Try
            Dim i As Integer = 0
            For Each gvrow As GridViewRow In gvRolloverRoll.Rows
                Dim result As Boolean = DirectCast(gvrow.FindControl("chkSelect"), CheckBox).Checked
                drRollIN = dsOpenRollIns.Tables(0).Select("RolloverID = '" + gvRolloverRoll.DataKeys(gvrow.RowIndex).Value.ToString + "'")
                If result Then
                    lstUserdetails.Add(New Dictionary(Of String, String)() From { _
                            {"Id", gvRolloverRoll.DataKeys(gvrow.RowIndex).Value.ToString.ToUpper},
                            {"FundNo", gvrow.Cells(2).Text},
                            {"InstitutionName", gvrow.Cells(4).Text.Replace("&amp;", "&")},
                            {"PartAccno", IIf(gvrow.Cells(5).Text = "&nbsp;", "", gvrow.Cells(5).Text)},
                            {"PersID", gvrow.Cells(1).Text},
                            {"InstitutionId", IIf(gvrow.Cells(7).Text = "&nbsp;", "", gvrow.Cells(7).Text)},
                            {"FirstName", IIf(drRollIN.Length < 0, "", drRollIN(0)("FirstName"))},
                            {"MiddleName", IIf(drRollIN.Length < 0, "", drRollIN(0)("MiddleName"))},
                            {"LastName", IIf(drRollIN.Length < 0, "", drRollIN(0)("LastName"))},
                            {"SSNo", IIf(drRollIN.Length < 0, "", drRollIN(0)("SSNo"))}
                        }
                        )
                End If
            Next
            Return lstUserdetails
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> GetSelectedRecords", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function
    ''' <summary>
    ''' On click of close button this method will redirect the main page and clears all sessions
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessions()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' On databind of gvRolloverRoll this method will be called for hiding the columns and set sorting arrows
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvRolloverRoll_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRolloverRoll.RowDataBound
        Try
            If e.Row.RowType <> DataControlRowType.EmptyDataRow Then
                e.Row.Cells(1).Visible = False
                e.Row.Cells(7).Visible = False
                If e.Row.RowType = DataControlRowType.Header Then
                    HelperFunctions.SetSortingArrows(objSortState, e)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> gvRolloverRoll_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' This method will be used to display the followup history
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvRolloverRoll_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRolloverRoll.SelectedIndexChanged
        Dim strRolloverID As String
        Dim dvFollowupHist As DataView
        Dim strName As String
        Dim strInstName As String
        Dim strPAcno As String
        Try
            strRolloverID = gvRolloverRoll.DataKeys(gvRolloverRoll.SelectedRow.RowIndex).Value.ToString.ToUpper
            dvFollowupHist = dsOpenRollIns.Tables(1).DefaultView
            dvFollowupHist.RowFilter = "RefId='" + strRolloverID + "'"
            strName = gvRolloverRoll.SelectedRow.Cells(3).Text.Replace("&nbsp;", "").Replace("&amp;", "&")
            strInstName = gvRolloverRoll.SelectedRow.Cells(4).Text.Replace("&nbsp;", "").Replace("&amp;", "&")
            strPAcno = gvRolloverRoll.SelectedRow.Cells(5).Text.Replace("&nbsp;", "").Replace("&amp;", "&")
            If HelperFunctions.isNonEmpty(dvFollowupHist) Then
                HelperFunctions.BindGrid(gvRollFollowupHist, dvFollowupHist)
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "ShowFollowUpHistDialog('" + strName + "','" + strInstName + "','" + strPAcno + "');", True)
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_REMINDER_NO_HISTORY)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> gvRolloverRoll_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' For sorting the grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvRolloverRoll_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvRolloverRoll.Sorting
        Try
            HelperFunctions.gvSorting(objSortState, e.SortExpression, dsOpenRollIns.Tables(0).DefaultView)
            HelperFunctions.BindGrid(gvRolloverRoll, dsOpenRollIns, True)
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> gvRolloverRoll_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Clears all the sessions
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClearSessions()
        Try
            dsOpenRollIns = Nothing
            objSortState = Nothing
            SessionRollIns.dsRecords_RollInReminderForm = Nothing
            Session("Rollin_MergedPdfs_Filename") = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> ClearSessions", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function

    ''' <summary>
    ''' For clear the selected records on click of no button in confirm dialg box
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ClearRollinPrintSession() As String
        Try
            SessionRollIns.dsRecords_RollInReminderForm = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> ClearRollinPrintSession", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function

    ''' <summary>
    ''' It will sets a session variable to print the letters for selected  members
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function PrintRollinLetters() As String
        Dim lstRecords As List(Of Dictionary(Of String, String))
        Dim dtRecords As DataTable
        Dim dsRecords As New DataSet
        Dim strBatchId As String = String.Empty
        Dim strModuleType As String = String.Empty
        Try
            If SessionRollIns.dsRecords_RollInReminderForm IsNot Nothing Then
                lstRecords = SessionRollIns.dsRecords_RollInReminderForm
                dtRecords = ConvertListToDatableRollin(lstRecords)
                dtRecords.TableName = "SelectedBatchRecords"
                dsRecords.Tables.Add(dtRecords)
                strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.MRDBO.GetNextBatchId(Date.Now.ToString("MM/dd/yyyy")).Tables(0).Rows(0)(0))
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PrintRollinLetters", "Reminder letter print for BatchId:" + strBatchId)
                strModuleType = BatchProcess.RMDRollins.ToString
                YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModuleType, dsRecords)
            End If
            SessionRollIns.dsRecords_RollInReminderForm = Nothing
            Return (strBatchId + "$$$" + strModuleType)
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> PrintRollinLetters", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    ''' <summary>
    ''' To get the message from resource file
    ''' </summary>
    ''' <param name="strMessageKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMessage(ByVal intMessageKey As Integer, Optional ByRef strParam As String = Nothing, Optional ByRef strParamKey As String = Nothing) As String
        Dim strMessage As String
        Dim dictParam As Dictionary(Of String, String)
        Try
            'strMessage = GetGlobalResourceObject("RollIn", strMessageKey).ToString()
            If strParam Is Nothing Then
                strMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(intMessageKey)
            ElseIf strParam IsNot Nothing Then
                dictParam = New Dictionary(Of String, String)
                dictParam.Add(strParamKey, strParam)
                strMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(intMessageKey, dictParam)
            End If

            Return strMessage
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> GetMessage", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Function

    Private Sub btnPrintList_Click(sender As Object, e As EventArgs) Handles btnPrintList.Click
        Try
            GetSelectedRecordsForList()
            Session("ReportName") = "Roll In Reminder List"
            Dim popupScript1 As String = "<script language='javascript'>" & _
                                        "window.open('CallReport.aspx', 'ReportPopUp1', " & _
                                        "'width=824,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                                        "</script>"

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
        Catch ex As Exception
            HelperFunctions.LogException("RollInReminderForm --> btnPrintList_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Public Function GetSelectedRecordsForList() As DataTable
        Dim dtPrintLetters As New DataTable
        Dim drPrintLetters As DataRow
        dtPrintLetters.Columns.Add("FundId")
        dtPrintLetters.Columns.Add("Name")
        dtPrintLetters.Columns.Add("InstitutionName")
        dtPrintLetters.Columns.Add("PartAccno")
        dtPrintLetters.Columns.Add("DocRcvdDate")
        dtPrintLetters.Columns.Add("RequestDate")
        dtPrintLetters.Columns.Add("Selected")
        dtPrintLetters.Columns.Add("ReportName")
        For iCount As Integer = 0 To gvRolloverRoll.Rows.Count - 1
            Dim chkBox As New CheckBox
            chkBox = CType(gvRolloverRoll.Rows(iCount).FindControl("chkSelect"), CheckBox)
            If Not chkBox Is Nothing Then
                drPrintLetters = dtPrintLetters.NewRow
                If chkBox.Checked Then
                    drPrintLetters("Selected") = "Yes"
                Else
                    drPrintLetters("Selected") = "No"
                End If
                drPrintLetters("FundId") = gvRolloverRoll.Rows(iCount).Cells(2).Text
                drPrintLetters("Name") = gvRolloverRoll.Rows(iCount).Cells(3).Text
                drPrintLetters("InstitutionName") = gvRolloverRoll.Rows(iCount).Cells(4).Text.Replace("&amp;", "&")
                drPrintLetters("PartAccno") = IIf(gvRolloverRoll.Rows(iCount).Cells(5).Text = "&nbsp;", "", gvRolloverRoll.Rows(iCount).Cells(5).Text)
                drPrintLetters("DocRcvdDate") = gvRolloverRoll.Rows(iCount).Cells(6).Text
                drPrintLetters("RequestDate") = IIf(gvRolloverRoll.Rows(iCount).Cells(8).Text = "&nbsp;", "", gvRolloverRoll.Rows(iCount).Cells(8).Text)
                drPrintLetters("ReportName") = "Roll In Reminder List"
                dtPrintLetters.Rows.Add(drPrintLetters)
            End If
        Next
        SessionRollIns.RollinPrintLetters = dtPrintLetters
        Return dtPrintLetters

    End Function

    Public Shared Function GetRollinPrintList() As DataTable
        Dim dtPrintLetters As New DataTable
        dtPrintLetters = DirectCast(SessionRollIns.RollinPrintLetters, DataTable)
        Return dtPrintLetters
    End Function

    Private Shared Function ConvertListToDatableRollin(ByVal objList As List(Of Dictionary(Of String, String))) As DataTable
        Try
            Dim l_datatable_PrintLetters As New DataTable
            Dim l_datarow_PrintLetters As DataRow
            Dim dsAddress As DataSet
            Dim drAddress As DataRow()
            l_datatable_PrintLetters.Columns.Add("PersonId")
            l_datatable_PrintLetters.Columns.Add("RefRequestID")
            l_datatable_PrintLetters.Columns.Add("FUNDNo")
            l_datatable_PrintLetters.Columns.Add("SSNo")
            l_datatable_PrintLetters.Columns.Add("PartAccno")
            l_datatable_PrintLetters.Columns.Add("InstitutionName")
            l_datatable_PrintLetters.Columns.Add("FirstName")
            l_datatable_PrintLetters.Columns.Add("LastName")
            l_datatable_PrintLetters.Columns.Add("MiddleName")
            l_datatable_PrintLetters.Columns.Add("addr1")
            l_datatable_PrintLetters.Columns.Add("addr2")
            l_datatable_PrintLetters.Columns.Add("addr3")
            l_datatable_PrintLetters.Columns.Add("city")
            l_datatable_PrintLetters.Columns.Add("StateName")
            l_datatable_PrintLetters.Columns.Add("zipCode")

            If objList.Count > 0 Then
                For Icount As Integer = 0 To objList.Count - 1
                    If objList(Icount)("InstitutionId") <> String.Empty Then
                        dsAddress = Address.GetAddressByEntity(objList(Icount)("InstitutionId"), EnumEntityCode.INST)
                        drAddress = dsAddress.Tables(0).Select("isPrimary = True")
                    End If
                    l_datarow_PrintLetters = l_datatable_PrintLetters.NewRow()
                    l_datarow_PrintLetters("PersonId") = objList(Icount)("PersID")
                    l_datarow_PrintLetters("RefRequestID") = objList(Icount)("Id")
                    l_datarow_PrintLetters("SSNo") = objList(Icount)("SSNo")
                    l_datarow_PrintLetters("FirstName") = objList(Icount)("FirstName")
                    l_datarow_PrintLetters("FUNDNo") = objList(Icount)("FundNo")
                    l_datarow_PrintLetters("LastName") = objList(Icount)("LastName")
                    l_datarow_PrintLetters("MiddleName") = objList(Icount)("MiddleName")
                    l_datarow_PrintLetters("PartAccno") = objList(Icount)("PartAccno")
                    l_datarow_PrintLetters("InstitutionName") = objList(Icount)("InstitutionName")
                    If (drAddress IsNot Nothing AndAlso drAddress.Length > 0) Then
                        l_datarow_PrintLetters("addr1") = drAddress(0)("addr1").ToString().Replace(",", "").Trim
                        l_datarow_PrintLetters("addr2") = drAddress(0)("addr2").ToString().Replace(",", "").Trim
                        l_datarow_PrintLetters("addr3") = drAddress(0)("addr3").ToString().Replace(",", "").Trim
                        l_datarow_PrintLetters("city") = drAddress(0)("city").ToString().Replace(",", "").Trim
                        l_datarow_PrintLetters("StateName") = drAddress(0)("state").ToString().Replace(",", "").Trim 'AA 05.08.2015 BT:2825:YRS 5.0-2500:Added to change the format of address
                        l_datarow_PrintLetters("zipCode") = drAddress(0)("zipCode").ToString().Replace(",", "").Trim
                    Else
                        l_datarow_PrintLetters("addr1") = ""
                        l_datarow_PrintLetters("addr2") = ""
                        l_datarow_PrintLetters("addr3") = ""
                        l_datarow_PrintLetters("city") = ""
                        l_datarow_PrintLetters("StateName") = ""
                        l_datarow_PrintLetters("zipCode") = ""
                    End If
                    l_datatable_PrintLetters.Rows.Add(l_datarow_PrintLetters)
                Next
            End If

            Return l_datatable_PrintLetters
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class