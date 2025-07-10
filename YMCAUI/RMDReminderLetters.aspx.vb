'************************************************************************************************************************
' Author                    : Santosh Bura
' Created on                : 04/07/2017
' Summary of Functionality  : This form is used to print Annual Letter for Non-Respondents letter for new persons who not been marked exhausted, have not been marked as "Requested Annual RMD"
'                             and have an unsatisfied RMD due for at least one prior year.
' Declared in Version       : 20.2.0 | YRS-AT-3400 & YRS-AT-3401
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name  | Date       | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
'Shilpa N         | 02/28/2019  |              | YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************
Imports YMCAObjects
Imports System.IO

Public Class RMDReminderLetters
    Inherits System.Web.UI.Page

#Region "Global declaration"
    Private Const RMD_LETTER_PERSID As Integer = 1
    Private Const RMD_LETTER_SSNO As Integer = 2
    Private Const RMD_LETTER_FUNDNO As Integer = 3
    Private Const RMD_LETTER_FIRSTNAME As Integer = 4
    Private Const RMD_LETTER_MIDDLENAME As Integer = 5
    Private Const RMD_LETTER_LASTNAME As Integer = 6
    Private Const RMD_LETTER_ISLOCKED As Integer = 7
    Private Const RMD_LETTER_PLANTYPE As Integer = 8
    Private Const RMD_LETTER_NAME As Integer = 9
    Private Const RMD_LETTER_CURRENTBALANCE As Integer = 10
    Private Const RMD_LETTER_RMDAMOUNT As Integer = 11
    Private Const RMD_LETTER_PAIDAMOUNT As Integer = 12
    Private Const RMD_LETTER_FUNDSTATUS As Integer = 13
    Private Const RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE As Integer = 14
    Private Const RMD_LETTER_LETTERLASTGENERATD As Integer = 15
    Private Const RMD_LETTER_GUIFUNDEVENTID As Integer = 17
#End Region

#Region "Properties"
    Public Property SelectedParticipants() As DataSet
        Get
            Dim data As DataSet = Nothing
            If Not Session("SelectedRMDPrintLetters") Is Nothing Then
                data = (CType(Session("SelectedRMDPrintLetters"), DataSet))
                If Not HelperFunctions.isNonEmpty(data) Then
                    data = Nothing
                End If
            End If
            Return data
        End Get
        Set(ByVal Value As DataSet)
            Session("SelectedRMDPrintLetters") = Value
        End Set
    End Property

    Public Property SelectedRMDReminderLetters() As DataTable        ' We are setting this property to slect the selected records for reminder letter generation
        Get
            If Not (Session("SelectedRMDReminderLetters")) Is Nothing Then
                Return (CType(Session("SelectedRMDReminderLetters"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            Session("SelectedRMDReminderLetters") = Value
        End Set
    End Property

    'TO get/set the Report Name 
    Public Property ReportName() As String
        Get
            If Not Session("ReportName") Is Nothing Then
                Return (CType(Session("ReportName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ReportName") = Value
        End Set
    End Property

    'TO get/set the DocumentCode for letter generation
    Public Property DocumentCode() As String
        Get
            If Not Session("DocumentCode") Is Nothing Then
                Return (CType(Session("DocumentCode"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("DocumentCode") = Value
        End Set
    End Property

    'TO get/set the Module Namefor letter generation
    Public Property ModuleType() As String
        Get
            If Not Session("ModuleType") Is Nothing Then
                Return (CType(Session("ModuleType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ModuleType") = Value
        End Set
    End Property

    Public Property LetterCode() As String
        Get
            If Not Session("LetterCode") Is Nothing Then
                Return (CType(Session("LetterCode"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("LetterCode") = Value
        End Set
    End Property

    'To get/set the SubFolderName  for Saving reprint copy of Reminder Annual Letters location
    Public Property SubFolderName() As String
        Get
            If Not Session("SubFolderName") Is Nothing Then
                Return (CType(Session("SubFolderName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("SubFolderName") = Value
        End Set
    End Property

    Public Property IsReadOnlyAccess() As Boolean
        Get
            If Not Session("IsReadOnlyAccess") Is Nothing Then
                Return (CType(Session("IsReadOnlyAccess"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Session("IsReadOnlyAccess") = value
        End Set
    End Property

    Public Property ReadOnlyWarningMessage() As String
        Get
            If Not Session("ReadOnlyWarningMessage") Is Nothing Then
                Return (CType(Session("ReadOnlyWarningMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("ReadOnlyWarningMessage") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMD Reminder Letters page load", "START: Page Load.")
            If Not IsPostBack Then
                If Not Request.QueryString("Process") Is Nothing Then
                    SetPageDetails(Request.QueryString("Process"))
                End If
                GetLastProccesedDate()
                GetRMDReminderRecords()
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMD Reminder Letters page load", "END: Page Load.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDReminderLetters_Page_Load", ex)
        End Try
    End Sub

    'Function to get the records to display on the gridview from the database
    Private Sub GetRMDReminderRecordsData(ModuleType As String)
        Dim data As DataSet
        Try
            If (ModuleType = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString) Then
                data = YMCARET.YmcaBusinessObject.MRDBO.GetRMDReminderLettersForNonRespondent()
            ElseIf (ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString) Then
                data = YMCARET.YmcaBusinessObject.MRDBO.GetRMDReminderLettersForAnnualNotElected()
            End If
            If HelperFunctions.isNonEmpty(data) Then
                If data.Tables.Count > 0 Then
                    ViewState("RMDPrintLetters_dtReminderRecords") = data.Tables(0)
                    EnablePrintButtons(True)
                Else
                    ViewState("RMDPrintLetters_dtReminderRecords") = Nothing
                    EnablePrintButtons(False)
                End If
            Else
                EnablePrintButtons(False)
            End If
        Catch
            Throw
        Finally
            data = Nothing
        End Try
    End Sub

    Private Sub gvLetters_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLetters.RowDataBound
        Dim records As New DataTable
        Dim row As DataRow()
        Dim processDate As Date
        Dim isLetterGenerated As Boolean
        Dim filePath As String
        Dim relativePath As String
        Dim lnkLastGenerated As LinkButton
        Dim lblLetterLastGenerated As Label
        Dim encryptLink As EncryptedQueryString
        Try
            isLetterGenerated = False
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("RMDReminderLetters_sort"), e)
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                records = DirectCast(ViewState("RMDPrintLetters_dtReminderRecords"), DataTable)
                row = records.Select("FundIdNo = '" + e.Row.Cells(RMD_LETTER_FUNDNO).Text + "'")
                If row.Length > 0 Then
                    For Each dr As DataRow In row
                        If dr("IsLocked") Then
                            e.Row.CssClass = "BG_ColourIsLocked"
                        ElseIf Convert.ToDouble(dr("CurrentBalance").ToString()) <= 0 Or (Convert.ToDouble(dr("CurrentBalance").ToString()) < (Convert.ToDouble(dr("CurrentMRD").ToString()) - Convert.ToDouble(dr("CurrentPaid").ToString()))) Then
                            e.Row.CssClass = "BG_ColourIsBlocked"
                        End If
                    Next
                End If
                lnkLastGenerated = e.Row.FindControl("lnkBtnLastGen")
                lblLetterLastGenerated = e.Row.FindControl("lblLetterLastGen")
                If lnkLastGenerated IsNot Nothing AndAlso lblLetterLastGenerated IsNot Nothing Then
                    lnkLastGenerated.Visible = False
                    lblLetterLastGenerated.Visible = False
                    If (e.Row.Cells(RMD_LETTER_LETTERLASTGENERATD).Text.Trim = "") Then
                        lblLetterLastGenerated.Visible = True
                    Else
                        lnkLastGenerated.Visible = True
                        filePath = String.Format("{0}\{1}\{2}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), Me.SubFolderName, Trim(e.Row.Cells(RMD_LETTER_GUIFUNDEVENTID).Text))
                        relativePath = String.Format("{0}\\{1}\\{2}.pdf", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), Me.SubFolderName, Trim(e.Row.Cells(RMD_LETTER_GUIFUNDEVENTID).Text))

                        lnkLastGenerated.Attributes.Remove("href")
                        If File.Exists(filePath) Then
                            encryptLink = New EncryptedQueryString()
                            encryptLink.Add("link", relativePath)

                            lnkLastGenerated.OnClientClick = String.Format("javascript:return OpenPDF('DocumentViewer.aspx?p={0}');", encryptLink.ToString())
                        Else
                            lnkLastGenerated.Attributes.CssStyle(HtmlTextWriterStyle.Color) = "black"
                            lnkLastGenerated.Attributes.CssStyle(HtmlTextWriterStyle.Cursor) = "default"
                            lnkLastGenerated.Attributes.CssStyle(HtmlTextWriterStyle.TextDecoration) = "none"
                            lnkLastGenerated.OnClientClick = Nothing
                            lnkLastGenerated.ToolTip = Nothing
                        End If
                    End If
                End If

                '  Convert Due date field from datetime format to date only
                processDate = Convert.ToDateTime(e.Row.Cells(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).Text)
                e.Row.Cells(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).Text = processDate.ToString("MM/dd/yyyy")

                e.Row.Cells(RMD_LETTER_NAME).Text = System.Web.HttpUtility.HtmlDecode(e.Row.Cells(RMD_LETTER_NAME).Text)  '  Displaying full name with special characters 
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDReminderAnnualLetters_gvLetters_RowDataBound", ex)
        Finally
            encryptLink = Nothing
            lblLetterLastGenerated = Nothing
            lnkLastGenerated = Nothing
            relativePath = Nothing
            filePath = Nothing
            row = Nothing
            records = Nothing
        End Try
    End Sub

    Private Sub gvLetters_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvLetters.Sorting
        Dim dv As DataView
        Dim records As DataTable
        Dim sortExpression As String
        Try
            sortExpression = e.SortExpression
            records = DirectCast(ViewState("RMDPrintLetters_dtReminderRecords"), DataTable)
            If HelperFunctions.isNonEmpty(records) Then
                dv = records.DefaultView
                dv.Sort = sortExpression
                If ViewState("RMDReminderLetters_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("RMDReminderLetters_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("RMDReminderLetters_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvLetters, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_gvLetters_Sorting", ex)
        Finally
            sortExpression = Nothing
            records = Nothing
            dv = Nothing
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessionVariables()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDReminderLetters_btnClose_Click", ex)
        End Try
    End Sub

    Private Sub GetLastProccesedDate()
        Dim lastProcessedDate As String
        Try
            lastProcessedDate = YMCARET.YmcaBusinessObject.MRDBO.GetLastRMDProcessedDate()
            If String.IsNullOrEmpty(lastProcessedDate) Then
                lastProcessedDate = "12/31/2010"
            End If
            ViewState("ProcessDate") = lastProcessedDate
            lblMRDMsg.Text = String.Format(" Last RMD Closed Batch: {0}", lastProcessedDate)
        Catch
            Throw
        Finally
            lastProcessedDate = Nothing
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDReminderLetters_btnSearch_Click", ex)
        End Try
    End Sub

    ' Filter Data based on value select/provide from screen
    Private Sub SearchRecords()
        Dim dv As DataView
        Dim records As DataTable
        Dim sort As GridViewCustomSort
        Dim filterCriteria As String
        Dim list As ListItem
        Dim yearData As DataTable
        Try
            records = DirectCast(ViewState("RMDPrintLetters_dtReminderRecords"), DataTable)
            If HelperFunctions.isNonEmpty(records) Then
                dv = records.DefaultView
                If ddlAcctLocked.SelectedValue <> "All" Then
                    filterCriteria = "ISLocked = " + ddlAcctLocked.SelectedValue
                End If
                If ddlInsufficientBal.SelectedValue <> "All" Then
                    If ddlInsufficientBal.SelectedValue = "Yes" Then
                        If filterCriteria = String.Empty Then
                            filterCriteria = "(CurrentBalance <= 0 OR CurrentBalance < (CurrentMRD - CurrentPaid))"
                        Else
                            filterCriteria += " AND (CurrentBalance <= 0 OR CurrentBalance < (CurrentMRD - CurrentPaid))"
                        End If
                    ElseIf ddlInsufficientBal.SelectedValue = "No" Then
                        If filterCriteria = String.Empty Then
                            filterCriteria = "CurrentBalance > 0"
                        Else
                            filterCriteria += " AND CurrentBalance > 0"
                        End If
                    End If
                End If

                If txtFundId.Text <> String.Empty Then
                    If filterCriteria = String.Empty Then
                        filterCriteria = "FundIdNo = '" + txtFundId.Text + "'"
                    Else
                        filterCriteria += " AND FundIdNo = '" + txtFundId.Text + "'"
                    End If
                End If

                dv.RowFilter = filterCriteria
                ViewState("RMDReminderLetters_Filter") = filterCriteria
                If ViewState("RMDReminderLetters_sort") IsNot Nothing Then
                    sort = ViewState("RMDReminderLetters_sort")
                    dv.Sort = sort.SortExpression + " " + sort.SortDirection
                End If
                If HelperFunctions.isNonEmpty(dv) Then
                    If dv.Table.Rows.Count > 0 Then
                        EnablePrintButtons(True)
                    Else
                        EnablePrintButtons(False)
                    End If
                Else
                    EnablePrintButtons(False)
                End If
            End If
            If (Me.ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString) Then
                gvLetters.Columns(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).HeaderText = "Due Date"
            End If
            HelperFunctions.BindGrid(gvLetters, dv, True)
        Catch
            Throw
        Finally
            dv = Nothing
            records = Nothing
            sort = Nothing
            filterCriteria = Nothing
            list = Nothing
            yearData = Nothing
        End Try
    End Sub

    ' Print RMDReminder Letters based on selection.
    Private Sub btnPrintLetters_Click(sender As Object, e As EventArgs) Handles btnPrintLetters.Click
        Dim selectedParticipantCounter As Integer
        Try
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            End If
            GetSelectedRecordsForPrinting()
            selectedParticipantCounter = 0
            If HelperFunctions.isNonEmpty(Me.SelectedParticipants) Then
                selectedParticipantCounter = Me.SelectedParticipants.Tables(0).Rows.Count
            End If
            If selectedParticipantCounter > 0 Then
                BatchProcessProgressControl.AllowReprint = True
                BatchProcessProgressControl.FolderForReprint = Me.SubFolderName
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Open", String.Format("{0}('Are you sure you want to print Letter for selected {1} record(s)');", BatchProcessProgressControl.OpenConfirmDialogMethodName, selectedParticipantCounter.ToString()), True)
            Else
                HelperFunctions.ShowMessageToUser("Please select the record(s) to print the letter", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDReminderLetters_btnPrintLetters_Click", ex)
        End Try
    End Sub

    ' Print SSRS reports for Non-Respondent Participants
    Protected Sub btnPrintList_Click(sender As Object, e As EventArgs) Handles btnPrintList.Click
        Try
            'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            'If Me.IsReadOnlyAccess Then
            'HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
            'Exit Sub
            'End If
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            GetSelectedRecordsForList()
            If ModuleType = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString Then
                Session("ReportName") = "RMDReminderToNonRespondentList"  'Report Name used while generating SSRS reports in callrepot.aspx page
            ElseIf ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString Then
                Session("ReportName") = "RMDReminderToAnnualElectionList"  'Report Name used while generating SSRS reports in callrepot.aspx page
            End If

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDReminderLetters_btnPrintList_Click", ex)
        End Try
    End Sub

    ' Get Selected records from gridview
    Private Sub GetSelectedRecordsForList()
        Dim printLettersData As New DataTable
        Dim row As DataRow
        Dim letterLastGenerated As String

        printLettersData.Columns.Add("FundNo")
        printLettersData.Columns.Add("PlanType")
        printLettersData.Columns.Add("CurrentBalance")
        printLettersData.Columns.Add("RMDAmount")
        printLettersData.Columns.Add("PaidAmount")
        printLettersData.Columns.Add("FundStatus")
        If Me.ModuleType = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString Then
            printLettersData.Columns.Add("EarliestUnsatisfiedDueDate")
        ElseIf Me.ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString Then
            printLettersData.Columns.Add("DueDate")
        End If
        printLettersData.Columns.Add("Selected")
        printLettersData.Columns.Add("ProcessDate")
        printLettersData.Columns.Add("ReportName")
        printLettersData.Columns.Add("LetterLastGenerated")

        If HelperFunctions.isNonEmpty(gvLetters) Then


            For count As Integer = 0 To gvLetters.Rows.Count - 1
                Dim chkBox As New CheckBox
                chkBox = CType(gvLetters.Rows(count).FindControl("chkSelect"), CheckBox)
                If Not chkBox Is Nothing Then
                    row = printLettersData.NewRow
                    If chkBox.Checked Then
                        row("Selected") = "Yes"
                    Else
                        row("Selected") = "No"
                    End If
                    row("FundNo") = gvLetters.Rows(count).Cells(RMD_LETTER_FUNDNO).Text
                    row("PlanType") = gvLetters.Rows(count).Cells(RMD_LETTER_PLANTYPE).Text
                    row("CurrentBalance") = gvLetters.Rows(count).Cells(RMD_LETTER_CURRENTBALANCE).Text
                    row("RMDAmount") = gvLetters.Rows(count).Cells(RMD_LETTER_RMDAMOUNT).Text
                    row("PaidAmount") = gvLetters.Rows(count).Cells(RMD_LETTER_PAIDAMOUNT).Text
                    row("FundStatus") = gvLetters.Rows(count).Cells(RMD_LETTER_FUNDSTATUS).Text
                    row("ProcessDate") = gvLetters.Rows(count).Cells(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).Text
                    If Me.ModuleType = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString Then
                        row("EarliestUnsatisfiedDueDate") = gvLetters.Rows(count).Cells(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).Text
                    ElseIf Me.ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString Then
                        row("DueDate") = gvLetters.Rows(count).Cells(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).Text
                    End If
                    row("ReportName") = BatchProcessProgressControl.DialogTitle
                    letterLastGenerated = String.Empty
                    letterLastGenerated = gvLetters.Rows(count).Cells(RMD_LETTER_LETTERLASTGENERATD).Text.Trim()
                    If (letterLastGenerated = "") Then
                        row("LetterLastGenerated") = "-"
                    Else
                        row("LetterLastGenerated") = gvLetters.Rows(count).Cells(RMD_LETTER_LETTERLASTGENERATD).Text
                    End If
                    printLettersData.Rows.Add(row)
                End If
            Next
            SessionManager.SessionParticipantRMDs.ReminderLetters = printLettersData
        End If

    End Sub

    Private Sub GetSelectedRecordsForPrinting()
        Dim selectedRMD As New CheckBox
        Dim selectedRecords As New DataTable
        Dim row As DataRow
        Dim printLettersDataSet As DataSet
        Try
            selectedRecords.Columns.Add("PersonId")
            selectedRecords.Columns.Add("SSNo")
            selectedRecords.Columns.Add("FUNDNo")
            selectedRecords.Columns.Add("FirstName")
            selectedRecords.Columns.Add("MiddleName")
            selectedRecords.Columns.Add("LastName")
            selectedRecords.Columns.Add("ISLocked")
            selectedRecords.Columns.Add("PlanType")
            selectedRecords.Columns.Add("Name")
            selectedRecords.Columns.Add("CurrentBalance")
            selectedRecords.Columns.Add("MRDAmount")
            selectedRecords.Columns.Add("PaidAmount")
            selectedRecords.Columns.Add("FundStatus")
            selectedRecords.Columns.Add("EarliestUnSatisfiedDueDate")
            selectedRecords.Columns.Add("LetterLastGenerated")
            selectedRecords.Columns.Add("FundEventId")
            selectedRecords.Columns.Add("LetterCode")
            selectedRecords.Columns.Add("RefRequestID")

            If (ModuleType = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString) Then
                BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString()
            ElseIf (ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString) Then
                BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString()
            End If

            '-- Setting batch id
            BatchProcessProgressControl.BatchID = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(DateTime.Now.ToString("ddMMMyyyy")).Tables(0).Rows(0)(0))

            Dim i As Integer = 0
            For i = 0 To gvLetters.Rows.Count - 1
                selectedRMD = gvLetters.Rows(i).FindControl("chkSelect")
                If Not IsNothing(selectedRMD) Then
                    If selectedRMD.Checked = True Then
                        row = selectedRecords.NewRow()
                        row("PersonId") = gvLetters.Rows(i).Cells(RMD_LETTER_PERSID).Text.ToString()
                        row("SSNo") = gvLetters.Rows(i).Cells(RMD_LETTER_SSNO).Text.ToString()
                        row("FUNDNo") = gvLetters.Rows(i).Cells(RMD_LETTER_FUNDNO).Text.ToString()

                        row("FirstName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_FIRSTNAME).Text.ToString())
                        row("MiddleName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_MIDDLENAME).Text.ToString())
                        row("LastName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_LASTNAME).Text.ToString())

                        row("ISLocked") = gvLetters.Rows(i).Cells(RMD_LETTER_ISLOCKED).Text.ToString()
                        row("PlanType") = gvLetters.Rows(i).Cells(RMD_LETTER_PLANTYPE).Text.ToString()
                        row("Name") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_NAME).Text.ToString())

                        row("CurrentBalance") = gvLetters.Rows(i).Cells(RMD_LETTER_CURRENTBALANCE).Text.ToString()
                        row("MRDAmount") = gvLetters.Rows(i).Cells(RMD_LETTER_RMDAMOUNT).Text.ToString()
                        row("PaidAmount") = gvLetters.Rows(i).Cells(RMD_LETTER_PAIDAMOUNT).Text.ToString()

                        row("FundStatus") = gvLetters.Rows(i).Cells(RMD_LETTER_FUNDSTATUS).Text.ToString()
                        row("EarliestUnSatisfiedDueDate") = gvLetters.Rows(i).Cells(RMD_LETTER_EARLIESTUNSATISFIEDDUEDATE).Text.ToString()
                        row("LetterLastGenerated") = gvLetters.Rows(i).Cells(RMD_LETTER_LETTERLASTGENERATD).Text.ToString()

                        row("FundEventId") = gvLetters.Rows(i).Cells(RMD_LETTER_GUIFUNDEVENTID).Text.ToString()
                        row("LetterCode") = Me.LetterCode
                        row("RefRequestID") = ""

                        selectedRecords.Rows.Add(row)
                    End If
                End If
            Next

            selectedRecords.TableName = "SelectedBatchRecords"
            printLettersDataSet = New DataSet()
            printLettersDataSet.Tables.Add(selectedRecords)
            Me.SelectedParticipants = printLettersDataSet
        Catch
            Throw
        End Try
    End Sub

    Private Sub GetRMDReminderRecords()
        Try
            ddlAcctLocked.SelectedValue = "All"
            ddlInsufficientBal.SelectedValue = "No"
            ViewState("RMDReminderLetters_Filter") = Nothing
            ViewState("RMDReminderLetters_sort") = Nothing
            GetRMDReminderRecordsData(Me.ModuleType)
            SearchRecords()
        Catch
            Throw
        End Try
    End Sub

    Private Sub BatchProcessProgressControl_HandlePUCCloseButtonClick(sender As Object, e As EventArgs) Handles BatchProcessProgressControl.HandlePUCCloseButtonClick
        GetRMDReminderRecordsData(Me.ModuleType)
        SearchRecords()
    End Sub

    '  Function to clear all the session variables and function to assign batch date for generating letters
    Private Sub ClearSessionVariables()
        Me.SelectedParticipants = Nothing
        Me.SelectedRMDReminderLetters = Nothing
        Me.ReportName = Nothing
        Me.DocumentCode = Nothing
        Me.ModuleType = Nothing
        Me.LetterCode = Nothing
        Me.SubFolderName = Nothing
        Me.ReadOnlyWarningMessage = Nothing
        Session("IsReadOnlyAccess") = Nothing
        Session("FTFileList") = Nothing
        Session("strReportName") = Nothing
        Session("MergedPdf_Filename") = Nothing
        Session("RMDBatchFundNo") = Nothing
    End Sub

    'Assigning  the property/Session variables , sets different values as per which page is clicked i.e Non-Respondent or RMD Satisfied Not Elected Letter
    Private Sub SetPageDetails(pageName As String)
        Dim rptName As String, NameConv As String, DocCode As String
        Select Case (pageName)
            Case "RMDNonRespondent"
                Me.DocumentCode = YMCAObjects.IDMDocumentCodes.RMD_NonRespondent_AnnualLetter
                Me.ReportName = "RMDReminderToNonRespondent.rpt"
                lblDescription.Text = "List of person(s) eligible for generating Annual Letter for Non-Respondents."
                Me.ModuleType = CommonClass.BatchProcess.RMDNonRespondentLetters.ToString
                Me.LetterCode = CommonClass.RPTLetter.RMDNRLET.ToString()
                Me.SubFolderName = "RMDReminderToNonRespondent"
                BatchProcessProgressControl.DialogTitle = "Annual Letter for Non-Respondents"
            Case "RMDNotElected"
                Me.DocumentCode = YMCAObjects.IDMDocumentCodes.RMD_SatisfiedNotElected_AnnualLetter
                Me.ReportName = "RMDReminderToAnnualElection.rpt"
                lblDescription.Text = "List of person(s) eligible for generating Satisfied RMD but Annual not Elected"
                Me.ModuleType = CommonClass.BatchProcess.RMDSatisfiedButNotElectedLetters.ToString
                Me.LetterCode = CommonClass.RPTLetter.RMDERLET.ToString()
                Me.SubFolderName = "RMDReminderToAnnualElection"
                BatchProcessProgressControl.DialogTitle = "Satisfied RMD but Annual not Elected"
        End Select
        CheckAccessRights(pageName)
    End Sub


    Private Sub CheckAccessRights(pageName As String)
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization(String.Format("RMDReminderLetters.aspx?Process={0}", pageName), Convert.ToInt32(Session("LoggedUserKey")))
            If Not String.IsNullOrEmpty(readOnlyWarningMessage) AndAlso Not readOnlyWarningMessage.ToUpper().Contains("TRUE") Then
                Me.IsReadOnlyAccess = True
                Me.ReadOnlyWarningMessage = readOnlyWarningMessage
            End If
        Catch
            Throw
        Finally
            readOnlyWarningMessage = Nothing
        End Try
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function CheckAccessRightsForReprint() As ReturnObject(Of Boolean)
        Dim result As ReturnObject(Of Boolean) = New ReturnObject(Of Boolean)
        result.Value = False
        If Not HttpContext.Current.Session("IsReadOnlyAccess") Is Nothing Then
            result.Value = Convert.ToBoolean(HttpContext.Current.Session("IsReadOnlyAccess"))

            If Not HttpContext.Current.Session("ReadOnlyWarningMessage") Is Nothing Then
                result.MessageList = New List(Of String)
                result.MessageList.Add(Convert.ToString(HttpContext.Current.Session("ReadOnlyWarningMessage")))
            End If
        End If
        Return result
    End Function

    Private Sub EnablePrintButtons(ByVal isEnable As Boolean)
        btnPrintLetters.Enabled = isEnable
        btnPrintList.Enabled = isEnable
    End Sub
End Class