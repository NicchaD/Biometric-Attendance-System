'*******************************************************************************
'Modification History 
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Option Explicit On
Imports System
Imports System.Data
Imports YMCAUI.SessionManager
Imports YMCAObjects.MetaMessageList
Imports System.Web.Services
Imports YMCARET.CommonUtilities
Imports YMCARET.YmcaBusinessObject

Public Class AnnuityBenefitDeathFollowup
    Inherits System.Web.UI.Page
    Dim checkBoxArray As New ArrayList
    Protected Property dsABDFLPendingList As DataSet
        Get
            Return SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupPendingList
        End Get
        Set(value As DataSet)
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupPendingList = value
        End Set
    End Property
    Dim strFormName As String = "AnnuityBenefitDeathFollowup.aspx"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            If Not IsPostBack Then
                WebPerformanceTracer.LogInfoTrace("Annuity Beneficiary Death Follow-up Form page load", "Page Load Call.")
                WebPerformanceTracer.EndTrace()

                'Display title in the header section
                Dim lblModuleName As Label

                lblModuleName = Master.FindControl("LabelModuleName")
                If lblModuleName IsNot Nothing Then
                    lblModuleName.Text = "Activities > Death > Annuity Beneficiary - Death Certificate Follow-up"
                End If
                ClearSessions()
                'Load Pending Follow-up List
                GetAnnBenFromConfiguration()
                Load_ABDFLPendingList()
                tab_gv60DayClick.Value = SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_FirstFollowup & " Day Follow-up Letter"
                tab_gv90DayClick.Value = SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_SecondFollowup & " Day Follow-up Letter"
            Else
                If Session("Rollin_MergedPdfs_Filename") IsNot Nothing Then
                    Dim strFileName As String
                    'Start:AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
                    Dim strPath As String
                    strFileName = Session("Rollin_MergedPdfs_Filename")
                    strPath = System.Configuration.ConfigurationSettings.AppSettings("MergePDFPath") + "\\"
                    If FileIO.FileSystem.FileExists(HttpContext.Current.Server.MapPath("~\" + strPath) + strFileName) Then
                        strFileName = strPath + strFileName
                        ScriptManager.RegisterStartupScript(Me, GetType(Page), "CallPdf", "OpenPDF('" + strFileName + "');", True)
                    End If

                    If SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = "60Days" Then
                        HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_60DAY_FOLLOWUP_PRINTED_SUCCESSFULLY)
                    ElseIf SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = "90Days" Then
                        HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_90DAY_FOLLOWUP_PRINTED_SUCCESSFULLY)
                    End If
                    ClearSessions()
                    GetAnnBenFromConfiguration()
                    'Load Pending Follow-up List
                    Load_ABDFLPendingList()
                End If
                If hdnSaveResponse.Value = "SaveResponse" Then
                    SaveFollowupResponse()
                    Load_ABDFLPendingList()
                ElseIf hdnSaveResponse.Value = "NoResponse" Then
                    hdnSaveResponse.Value = ""
                    RevertFollowupChanges()
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBeneficiaryDeathFollowup --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Public Function Load_ABDFLPendingList()
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Load_ABDCPendingList method.")

            dsABDFLPendingList = AnnuityBenefitDeathFollowupBOClass.GetPendingFollowupDays()
            HelperFunctions.BindGrid(gvPendingFollowupList, dsABDFLPendingList, True)
            BindGrids()
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> Load_ABDFLPendingList", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Load_ABDCPendingList method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Sub gvPendingFollowupList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvPendingFollowupList.PageIndexChanging, gv60DayFollowup.PageIndexChanging, gv90DayFollowup.PageIndexChanging
        Try
            ViewState("PageIndex") = e.NewPageIndex
            BindGrids()
        Catch
            Throw
        End Try
    End Sub
    Public Sub BindGrids()
        Dim dtPending As DataTable
        Dim dt60Days As DataTable
        Dim dt90Days As DataTable
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        Dim strFilter As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(txtFundNoFilter.Text) Then
                strFilter = "[Participant Fund No.] = '" & Val(txtFundNoFilter.Text) & "'"
            End If

            If HelperFunctions.isNonEmpty(dsABDFLPendingList) Then
                dtPending = dsABDFLPendingList.Tables("ABDFLPendingList")
            End If
            tab_gvPendingClick.Attributes.Add("class", "tabNotSelectedLink")
            tab_gv60DayClick.Attributes.Add("class", "tabNotSelectedLink")
            tab_gv90DayClick.Attributes.Add("class", "tabNotSelectedLink")

            tab_gvPending.Attributes.Add("class", "gvTab tabNotSelected")
            tab_gv60Days.Attributes.Add("class", "gvTab tabNotSelected")
            tab_gv90Days.Attributes.Add("class", "gvTab tabNotSelected")
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "TabChange", "TabDisplay('#" & hdnSelectedTad.Value.ToString & "');", True)
            If hdnSelectedTad.Value.ToString = "tabContent_gvPending" Then
                If HelperFunctions.isNonEmpty(dtPending) Then
                    StoreCheckedValues(gvPendingFollowupList)
                    dv = dtPending.DefaultView
                    dv.RowFilter = strFilter
                    If Not ViewState("Sort") Is Nothing Then
                        Sorting = ViewState("Sort")
                        dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
                    End If

                    If Not ViewState("PageIndex") Is Nothing Then
                        gvPendingFollowupList.PageIndex = ViewState("PageIndex")
                    End If
                End If
                tab_gvPending.Attributes.Add("class", "gvTab tabSelected")
                tab_gvPendingClick.Attributes.Add("class", "tabSelectedLink")

                BindDataToGrid(gvPendingFollowupList, dv)
                PopulateCheckedValues(gvPendingFollowupList)
            ElseIf hdnSelectedTad.Value.ToString = "tabContent_gv60Days" Then
                dt60Days = get60Days()
                If HelperFunctions.isNonEmpty(dt60Days) Then
                    StoreCheckedValues(gv60DayFollowup)
                    dv = dt60Days.DefaultView
                    dv.RowFilter = strFilter
                    If Not ViewState("Sort") Is Nothing Then
                        Sorting = ViewState("Sort")
                        dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
                    End If

                    If Not ViewState("PageIndex") Is Nothing Then
                        gv60DayFollowup.PageIndex = ViewState("PageIndex")
                    End If
                End If
                tab_gv60Days.Attributes.Add("class", "gvTab tabSelected")
                tab_gv60DayClick.Attributes.Add("class", "tabSelectedLink")

                BindDataToGrid(gv60DayFollowup, dv)
                PopulateCheckedValues(gv60DayFollowup)
            ElseIf hdnSelectedTad.Value.ToString = "tabContent_gv90Days" Then
                dt90Days = get90Days()
                If HelperFunctions.isNonEmpty(dt90Days) Then
                    StoreCheckedValues(gv90DayFollowup)
                    dv = dt90Days.DefaultView
                    dv.RowFilter = strFilter
                    If Not ViewState("Sort") Is Nothing Then
                        Sorting = ViewState("Sort")
                        dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
                    End If

                    If Not ViewState("PageIndex") Is Nothing Then
                        gv90DayFollowup.PageIndex = ViewState("PageIndex")
                    End If
                End If
                tab_gv90Days.Attributes.Add("class", "gvTab tabSelected")
                tab_gv90DayClick.Attributes.Add("class", "tabSelectedLink")

                BindDataToGrid(gv90DayFollowup, dv)
                PopulateCheckedValues(gv90DayFollowup)
            End If
        Catch ex As Exception
            Throw
        Finally
            dv = Nothing
        End Try
    End Sub
    Public Function get60Days() As DataTable
        Dim dt60Days As DataTable
        Dim intFollowupDays As Integer = SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_FirstFollowup()
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Calling Get60Days method.")

            dt60Days = dsABDFLPendingList.Tables("ABDFLPendingList").Clone()
            For Each dr As DataRow In dsABDFLPendingList.Tables("ABDFLPendingList").Rows
                If IsDBNull(dr("60 day Follow-up Sent")) And IsDBNull(dr("90 day Follow-up Sent")) And Convert.ToDateTime(dr("Original Letter Date").ToString()).AddDays(intFollowupDays).Date < DateTime.Today.Date And Convert.ToBoolean(dr("DocumentsReceived")) = False Then
                    dt60Days.ImportRow(dr)
                End If
            Next
            Return dt60Days
        Catch
            Throw
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Calling Get60Days method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function
    Public Function get90Days() As DataTable
        Dim dt90Days As DataTable
        Dim intFollowupDays As Integer = SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_SecondFollowup()
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Calling get90Days method.")

            dt90Days = dsABDFLPendingList.Tables("ABDFLPendingList").Clone()
            For Each dr As DataRow In dsABDFLPendingList.Tables("ABDFLPendingList").Rows
                If IsDBNull(dr("90 day Follow-up Sent")) And Convert.ToDateTime(dr("Original Letter Date").ToString()).AddDays(intFollowupDays).Date < DateTime.Today.Date And Convert.ToBoolean(dr("DocumentsReceived")) = False Then
                    dt90Days.ImportRow(dr)
                End If
            Next
            Return dt90Days
        Catch
            Throw
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Calling get90Days method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Sub gvPendingFollowupList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPendingFollowupList.RowDataBound, gv90DayFollowup.RowDataBound, gv60DayFollowup.RowDataBound
        HelperFunctions.SetSortingArrows(ViewState("Sort"), e)
    End Sub

    Private Sub gvPendingFollowupList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvPendingFollowupList.Sorting
        Dim dtPendingList As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsABDFLPendingList) Then
                dtPendingList = dsABDFLPendingList.Tables("ABDFLPendingList")
                Sortgrid(e, dtPendingList.DefaultView)
                BindGrids()
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub gv60DayFollowup_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv60DayFollowup.Sorting
        Dim dt60Days As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsABDFLPendingList) Then
                dt60Days = get60Days()
                Sortgrid(e, dt60Days.DefaultView)
                BindGrids()
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub gv90DayFollowup_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv90DayFollowup.Sorting
        Dim dt90Days As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsABDFLPendingList) Then
                dt90Days = get90Days()
                Sortgrid(e, dt90Days.DefaultView)
                BindGrids()
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Sub Sortgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs, ByVal dv As DataView)
        Dim SortExpression As String
        Try
            SortExpression = e.SortExpression

            HelperFunctions.gvSorting(ViewState("Sort"), e.SortExpression, dv)
        Catch
            Throw
        End Try
    End Sub

    Private Sub tab_gvPendingClick_ServerClick(sender As Object, e As EventArgs) Handles tab_gvPendingClick.ServerClick, tab_gv60DayClick.ServerClick, tab_gv90DayClick.ServerClick
        ViewState("PageIndex") = Nothing
        ViewState("Sort") = Nothing
        txtFundNoFilter.Text = String.Empty

        RevertFollowupChanges()
    End Sub

    Private Sub btn60Print_Click(sender As Object, e As EventArgs) Handles btn60Print.Click
        Dim dtRecords As DataTable
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Button60Print click method.")

            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization("AnnuityBenefitDeathFollowup.aspx", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If

            Dim checkSecurity As String = SecurityCheck.Check_Authorization("DeathAnnBenDeathCerFollowup60Print", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)

            dtRecords = GetSelectedRecords(gv60DayFollowup, YMCAObjects.IDMDocumentCodes.Ann_Bene_Death_Followup_FirstFollowup)
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = "60Days"
            If dtRecords.Rows.Count > 0 Then
                SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords = dtRecords
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CallPopup", "ShowConfirmDialog('" + GetMessage(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CONFIRM_PRINT, dtRecords.Rows.Count.ToString, "BENEFICIARY") + "');", True)
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_SELECT_RECORD)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> Button60Print_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Button60Print click method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Private Function GetSelectedRecords(ByVal dgvPrint As GridView, ByVal strLetterCode As String) As DataTable
        Dim dtTable As DataTable
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: GetSelectedRecords method calling.")

            StoreCheckedValues(dgvPrint)

            dtTable = GetDatableToPrint(dsABDFLPendingList, strLetterCode)

            Return dtTable
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> GetSelectedRecords", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: GetSelectedRecords method calling.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Sub btn90Print_Click(sender As Object, e As EventArgs) Handles btn90Print.Click
        Dim dtRecords As DataTable
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Button90Print click method.")

            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization("AnnuityBenefitDeathFollowup.aspx", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If

            Dim checkSecurity As String = SecurityCheck.Check_Authorization("DeathAnnBenDeathCerFollowup90Print", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)

            dtRecords = GetSelectedRecords(gv90DayFollowup, YMCAObjects.IDMDocumentCodes.Ann_Bene_Death_Followup_SecondFollowup)
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = "90Days"
            If dtRecords.Rows.Count > 0 Then
                SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords = dtRecords
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CallPopup", "ShowConfirmDialog('" + GetMessage(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CONFIRM_PRINT, dtRecords.Rows.Count.ToString, "BENEFICIARY") + "');", True)
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_SELECT_RECORD)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> Button90Print_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Button90Print click method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Public Function GetMessage(ByVal intMessageKey As Integer, Optional ByRef strParam As String = Nothing, Optional ByRef strParamKey As String = Nothing) As String
        Dim strMessage As String
        Dim dictParam As Dictionary(Of String, String)
        Try
            If strParam Is Nothing Then
                strMessage = MetaMessageBO.GetMessageByTextMessageNo(intMessageKey)
            ElseIf strParam IsNot Nothing Then
                dictParam = New Dictionary(Of String, String)
                dictParam.Add(strParamKey, strParam)
                strMessage = MetaMessageBO.GetMessageByTextMessageNo(intMessageKey, dictParam)
            End If

            Return strMessage
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> GetMessage", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function PrintFollowupLetters() As String
        Dim dtRecords As DataTable
        Dim dsRecords As New DataSet
        Dim strBatchId As String = String.Empty
        Dim strModuleType As String = String.Empty
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Ajax method PrintFollowupLetters calling.")

            If SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords IsNot Nothing Then
                dtRecords = SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords
                dtRecords.TableName = "SelectedBatchRecords"
                dsRecords.Tables.Add(dtRecords)
                strBatchId = Convert.ToString(MRDBO.GetNextBatchId(Date.Now.ToString("MM/dd/yyyy")).Tables(0).Rows(0)(0))
                strModuleType = BatchProcess.AnnBeneDeathFollowup.ToString
                MRDBO.InsertAtsTemp(strBatchId, strModuleType, dsRecords)
            End If
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords = Nothing
            Return (strBatchId + "$$$" + strModuleType)
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBeneficiaryDeathFollowup --> PrintFollowupLetters", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Ajax method PrintFollowupLetters calling.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function AnnBeneDeathFollowupProcess(ByVal strBatchId As String, ByVal iCount As Integer, ByVal strProcessName As String, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim strretval As String = String.Empty
        Dim dsTemp As DataSet
        Dim objReturnStatusValues = New ReturnStatusValues()
        Dim objAnnuityBenefitDeathFollowup As New AnnuityBenefitDeathFollowup

        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Ajax method AnnBeneDeathFollowupPreocess calling .")

            dsTemp = MRDBO.GetAtsTemp(strBatchId, strModule)
            objReturnStatusValues = objAnnuityBenefitDeathFollowup.Process(strBatchId, iCount, strProcessName, dsTemp, iIDXCreated, iPDFCreated, strModule)

            MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)

            If HelperFunctions.isNonEmpty(dsTemp.Tables("dtFileList")) Then
                HttpContext.Current.Session("FTFileList") = dsTemp.Tables("dtFileList")
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Process Letter", ex)
            objReturnStatusValues.strretValue = "error"
            Return objReturnStatusValues
            Throw ex
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Ajjax method AnnBeneDeathFollowupProcess calling.")
            WebPerformanceTracer.EndTrace()
        End Try
        Return objReturnStatusValues
    End Function
    Private Function GetBatchSize() As Integer
        Dim dsbatchSize As DataSet
        Dim intBatchsize As Integer
        Try
            dsbatchSize = YMCACommonBOClass.getConfigurationValue("BATCH_SIZE")
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

        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Method Process is calling.")

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

            If Not dtSelectedBatchRecords.Columns.Contains("PrintLetterId") Then
                dtSelectedBatchRecords.Columns.Add("PrintLetterId")
            End If

            If Not dtSelectedBatchRecords Is Nothing Then
                For i = iCount To dtSelectedBatchRecords.Rows.Count - 1
                    If i - iCount >= BatchSize Then
                        Exit For
                    End If
                    dtSelectedBatchRecords.Rows(i).Item("PrintLetterId") = AnnuityBeneficiaryDeathBOClass.InsertPrintLetters(dtSelectedBatchRecords.Rows(i)("RefId"), dtSelectedBatchRecords.Rows(i)("PersId"), dtSelectedBatchRecords.Rows(i)("LetterCode"))
                Next
            End If

            Dim dr As DataRow
            dt.Columns.Add("FirstName")
            dt.Columns.Add("MiddleName")
            dt.Columns.Add("LastName")
            dt.Columns.Add("PersonId")
            dt.Columns.Add("RefRequestId")
            dt.Columns.Add("FundNo")
            dt.Columns.Add("SSNo")
            dt.Columns.Add("LetterCode")
            dt.Columns.Add("PrintLetterId")

            If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
                For iProcessCount = iCount To dtSelectedBatchRecords.Rows.Count - 1
                    If iProcessCount - iCount >= BatchSize Then
                        Exit For
                    End If
                    dr = dt.NewRow()
                    dr("FirstName") = dtSelectedBatchRecords.Rows(iProcessCount)("BeneFirstName")
                    dr("MiddleName") = dtSelectedBatchRecords.Rows(iProcessCount)("BeneMiddleName")
                    dr("LastName") = dtSelectedBatchRecords.Rows(iProcessCount)("BeneLastName")
                    dr("PersonId") = dtSelectedBatchRecords.Rows(iProcessCount)("PersId")
                    dr("RefRequestId") = dtSelectedBatchRecords.Rows(iProcessCount)("RefId")
                    dr("FundNo") = dtSelectedBatchRecords.Rows(iProcessCount)("FundNo")
                    dr("SSNo") = dtSelectedBatchRecords.Rows(iProcessCount)("SSN")
                    dr("LetterCode") = dtSelectedBatchRecords.Rows(iProcessCount)("LetterCode")
                    dr("PrintLetterId") = dtSelectedBatchRecords.Rows(iProcessCount)("PrintLetterId")
                    dtSelectedBatchRecords.Rows(iProcessCount)("IsReportPrinted") = 1
                    dt.Rows.Add(dr)
                Next
            End If
            If HelperFunctions.isNonEmpty(dt) Then
                Dim l_stringDocType As String = String.Empty
                Dim l_StringReportName As String = String.Empty
                Dim l_string_OutputFileType As String = String.Empty
                Dim strParam1 As String = String.Empty
                If SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = "60Days" Then
                    l_stringDocType = YMCAObjects.IDMDocumentCodes.Ann_Bene_Death_Followup_FirstFollowup
                    l_StringReportName = "DB_60 Day PopUp Letter.rpt"
                    l_string_OutputFileType = "AnnuityBenefitDeathFollowup__" + l_stringDocType
                ElseIf SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = "90Days" Then
                    l_stringDocType = YMCAObjects.IDMDocumentCodes.Ann_Bene_Death_Followup_SecondFollowup
                    l_StringReportName = "DB_90 Day PopUp Letter.rpt"
                    l_string_OutputFileType = "AnnuityBenefitDeathFollowup__" + l_stringDocType
                End If
                strParam1 = "Ann_Bene_Death_FL"

                Session("strReportName") = l_StringReportName.Substring(0, l_StringReportName.Length - 4)
                strBatchError = objBatchProcess.InvokeBatchRequestCreation(0, dt, l_stringDocType, l_StringReportName, l_string_OutputFileType, strParam1, ArrErrorDataList, dtFileList, iIDXCreated, iPDFCreated)
                'Else
                '    Throw New Exception("Selected records not found.")
                For Each dtRow As DataRow In dt.Rows
                    If Val(dtRow("PrintLetterId")) > 0 AndAlso Val(dtRow("rptTrackingId")) > 0 Then
                        AnnuityBeneficiaryDeathBOClass.UpdatePrintLetters(Convert.ToInt32(dtRow("PrintLetterId")), Convert.ToInt32(dtRow("rptTrackingId")))
                    End If
                Next dtRow
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
            HelperFunctions.LogException("Annuity Beneficiary Death Follow-up Print Letters Process", ex)
            objReturnStatusValues.strretValue = "error"
            ArrErrorDataList.Add(New ExceptionLog("Exception", "Process Method", ex.Message))
            Return objReturnStatusValues
            Throw ex
        Finally
            WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Process method calling.")
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
            MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)

            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Method Process is calling.")
            WebPerformanceTracer.EndTrace()
        End Try
        Return objReturnStatusValues
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ClearPrintFollowupLettersSession() As String
        Try
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> ClearPrintFollowupLettersSession", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function

    Private Function ClearSessions()
        Try
            hdnSaveResponse.Value = ""
            dsABDFLPendingList = Nothing
            ViewState("Sort") = Nothing
            ViewState("PageIndex") = Nothing
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_FirstFollowup = Nothing
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_SecondFollowup = Nothing
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupPendingList = Nothing
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = Nothing
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupSelectedRecords = Nothing
            Session("Rollin_MergedPdfs_Filename") = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> ClearSessions", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Function

    Private Shared Function GetDatableToPrint(ByVal dsFollowup As DataSet, ByVal strLetterCode As String) As DataTable
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Start: Method ConvertListToDatatable is calling.")

            Dim dtPrintRecords As New DataTable
            Dim drPrint As DataRow
            Dim drSelected As DataRow()

            dtPrintRecords.Columns.Add("PersId")
            dtPrintRecords.Columns.Add("RefId")
            dtPrintRecords.Columns.Add("FundNo")
            dtPrintRecords.Columns.Add("SSN")
            dtPrintRecords.Columns.Add("LetterCode")
            dtPrintRecords.Columns.Add("PrintLetterId")
            dtPrintRecords.Columns.Add("BeneFirstName")
            dtPrintRecords.Columns.Add("BeneMiddleName")
            dtPrintRecords.Columns.Add("BeneLastName")

            drSelected = dsFollowup.Tables(0).Select("bitPrint = " & True)

            If drSelected.Count > 0 Then
                For Icount As Integer = 0 To drSelected.Count - 1
                    drPrint = dtPrintRecords.NewRow()
                    drPrint("PersId") = drSelected(Icount)("guiPersID")
                    drPrint("RefId") = drSelected(Icount)("guiAnnuityJointSurvivorsID")
                    drPrint("FundNo") = drSelected(Icount)("Participant Fund No.")
                    drPrint("SSN") = drSelected(Icount)("BENESSN")
                    drPrint("LetterCode") = strLetterCode
                    drPrint("BeneFirstName") = drSelected(Icount)("BeneFirstName")
                    drPrint("BeneMiddleName") = drSelected(Icount)("BeneMiddleName")
                    drPrint("BeneLastName") = drSelected(Icount)("BeneLastName")
                    dtPrintRecords.Rows.Add(drPrint)
                Next
            End If

            Return dtPrintRecords
        Catch ex As Exception
            Throw ex
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeathFollowup", "Finish: Method ConvertListToDatatable is calling.")
            WebPerformanceTracer.EndTrace()
        End Try

    End Function

    Private Sub SaveFollowupResponse()
        hdnSaveResponse.Value = ""
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "SaveProcess", "ShowSaveConfirmDialog('Please wait data is processing','Process');", True)
            StoreCheckedValues(gvPendingFollowupList)
            AnnuityBenefitDeathFollowupBOClass.UpdateJointSurvivorDeathDocReceived(dsABDFLPendingList)

            Load_ABDFLPendingList()
            HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_SAVED_SUCCESSFULLY)

        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup--> SaveFollowupResponse", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click, btn60Close.Click, btn90Close.Click
        Try
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupPendingList = Nothing
            SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowupGenerateReportFor = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> ButtonClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim dtChanged As New DataTable
        Try
            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization("AnnuityBenefitDeathFollowup.aspx", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If

            Dim checkSecurity As String = SecurityCheck.Check_Authorization("DeathAnnBenDeathCerFollowupSave", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If

            StoreCheckedValues(gvPendingFollowupList)

            dtChanged = dsABDFLPendingList.Tables(0).GetChanges(DataRowState.Modified)
            If dtChanged Is Nothing Then
                btnSave.Enabled = False
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CHECK_UNCHECK_SAVE_CHANGES)
                Exit Sub
            End If

            Dim confirmationMessage As String = MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CONFIRM_SAVE)
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CallPopup", "ShowSaveConfirmDialog('" & confirmationMessage & "','YesNo');", True)

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> ButtonSave_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
        End Try
    End Sub

    Private Sub StoreCheckedValues(ByVal gv As GridView)
        Dim index As String = "-1"
        Dim drFollowup As DataRow()
        Dim dtChanged As New DataTable
        Try
            If gv.ID <> "gvPendingFollowupList" Then
                If Not dsABDFLPendingList.Tables(0).Columns.Contains("bitPrint") Then
                    Dim columnName As New DataColumn
                    columnName.ColumnName = "bitPrint"
                    columnName.DefaultValue = False
                    dsABDFLPendingList.Tables(0).Columns.Add(columnName)
                End If
                For Each gvrow As GridViewRow In gv.Rows
                    index = gv.DataKeys(gvrow.RowIndex).Value.ToString
                    Dim result As Boolean = DirectCast(gvrow.FindControl("chkSelect"), CheckBox).Checked  ' Check in the Session 
                    drFollowup = dsABDFLPendingList.Tables(0).Select("guiAnnuityJointSurvivorsID = '" + index + "'")
                    drFollowup(0).Item("bitPrint") = result
                Next
                drFollowup = dsABDFLPendingList.Tables(0).Select("bitPrint =True")
                If drFollowup.Count > 0 Then
                    btn60Print.Enabled = True
                    btn90Print.Enabled = True
                Else
                    btn60Print.Enabled = False
                    btn90Print.Enabled = False
                End If
            Else
                For Each gvrow As GridViewRow In gv.Rows
                    index = gv.DataKeys(gvrow.RowIndex).Value.ToString
                    Dim result As Boolean = DirectCast(gvrow.FindControl("chkSelect"), CheckBox).Checked  ' Check in the Session 
                    drFollowup = dsABDFLPendingList.Tables(0).Select("guiAnnuityJointSurvivorsID = '" + index + "'")
                    If drFollowup(0).Item("DocumentsReceived") <> result Then
                        drFollowup(0).Item("DocumentsReceived") = result
                    End If
                Next
                dtChanged = dsABDFLPendingList.Tables(0).GetChanges(DataRowState.Modified)
                If dtChanged IsNot Nothing Then
                    If dtChanged.Rows.Count > 0 Then
                        btnSave.Enabled = True
                    Else
                        btnSave.Enabled = False
                    End If
                Else
                    btnSave.Enabled = False
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub PopulateCheckedValues(ByVal gv As GridView)
        Dim drFollowup As DataRow()
        Try
            If gv.ID <> "gvPendingFollowupList" Then
                For Each gvrow As GridViewRow In gv.Rows
                    Dim index As String = gv.DataKeys(gvrow.RowIndex).Value.ToString

                    drFollowup = dsABDFLPendingList.Tables(0).Select("guiAnnuityJointSurvivorsID = '" + index + "'")
                    Dim myCheckBox As CheckBox = DirectCast(gvrow.FindControl("chkSelect"), CheckBox)
                    myCheckBox.Checked = drFollowup(0).Item("bitPrint")
                Next
            Else
                For Each gvrow As GridViewRow In gv.Rows
                    Dim index As String = gv.DataKeys(gvrow.RowIndex).Value.ToString

                    drFollowup = dsABDFLPendingList.Tables(0).Select("guiAnnuityJointSurvivorsID = '" + index + "'")
                    Dim myCheckBox As CheckBox = DirectCast(gvrow.FindControl("chkSelect"), CheckBox)
                    myCheckBox.Checked = drFollowup(0).Item("DocumentsReceived")
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub GetAnnBenFromConfiguration()
        Dim ds As DataSet
        Try
            ds = YMCARET.YmcaBusinessObject.AnnuityBenefitDeathFollowupBOClass.getAnnBenFromConfiguration("ANNBEN")
            If ds.Tables(0).Rows.Count > 0 Then
                SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_FirstFollowup = ds.Tables(0).Rows(0).Item("Value").ToString
                SessionAnnuityBeneficiaryDeathFollowup.AnnBeneDeathFollowup_SecondFollowup = ds.Tables(0).Rows(1).Item("Value").ToString
            End If

        Catch
            Throw
        End Try
    End Sub
    Public Sub BindDataToGrid(ByVal gv As GridView, ByVal dv As DataView)
        Try
            gv.DataSource = dv
            gv.DataBind()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        BindGrids()
    End Sub
    Private Sub RevertFollowupChanges()
        dsABDFLPendingList.Tables(0).RejectChanges()
        If dsABDFLPendingList.Tables(0).Columns.Contains("bitPrint") Then
            dsABDFLPendingList.Tables(0).Columns.Remove("bitPrint")
        End If
        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)
        BindDataToGrid(gvPendingFollowupList, Nothing)
        BindDataToGrid(gv60DayFollowup, Nothing)
        BindDataToGrid(gv90DayFollowup, Nothing)
        BindGrids()
    End Sub
End Class