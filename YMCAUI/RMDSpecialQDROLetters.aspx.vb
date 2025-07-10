'************************************************************************************************************************
' Author                    : Manthan Milan Rajguru
' Created on                : 04/24/2017
' Summary of Functionality  : Listing QD participants with first RMD due in December for generating initial and follow-up letters
' Declared in Version       : 20.2.0 | YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
'Shilpa N                       | 02/28/2019    |                |   YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************
Imports YMCAObjects
Imports System.IO

Public Class RMDSpecialQDROLetters
    Inherits System.Web.UI.Page

    Private Const RMD_LETTER_PERSID As Integer = 1
    Private Const RMD_LETTER_FUNDNO As Integer = 2
    Private Const RMD_LETTER_PLANTYPE As Integer = 3
    Private Const RMD_LETTER_NAME As Integer = 4
    Private Const RMD_LETTER_CURRENTBALANCE As Integer = 5
    Private Const RMD_LETTER_RMDAMOUNT As Integer = 6
    Private Const RMD_LETTER_PAIDAMOUNT As Integer = 7
    Private Const RMD_LETTER_FUNDSTATUS As Integer = 8
    Private Const RMD_LETTER_DUEDATE As Integer = 9
    Private Const RMD_LETTER_SSNO As Integer = 10
    Private Const RMD_LETTER_FIRSTNAME As Integer = 11
    Private Const RMD_LETTER_LASTNAME As Integer = 12
    Private Const RMD_LETTER_MIDDLENAME As Integer = 13
    Private Const RMD_LETTER_INITIAL_LASTGENERATED As Integer = 14
    Private Const RMD_LETTER_FOLLOWUP_LASTGENERATED As Integer = 15
    Private Const RMD_LETTER_INITIAL_LASTGENERATED_LINK As Integer = 16
    Private Const RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK As Integer = 17
    Private Const RMD_LETTER_FUNDID As Integer = 18

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMD Special QD Print Letters page load", "START: Page Load.")
            If Not IsPostBack Then
                CheckAccessRights()
                GetLastProccesedDate()
                If Not (SelectedTab = PrintLetterTabStrips.NONE) Then
                    If (SelectedTab = PrintLetterTabStrips.INITIAL) Then
                        lnkInitial_Click(sender, e)
                    ElseIf (SelectedTab = PrintLetterTabStrips.FOLLOWUP) Then
                        lnkFollowup_Click(sender, e)
                    End If
                Else
                    lnkInitial_Click(sender, e)
                End If
                BatchProcessProgressControl.DialogTitle = "RMD Special QDRO Print Letters"
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMD Special QD Print Letters page load", "END: Page Load.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_Page_Load", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Get First time eligible RMD for Current year for initial Process
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetInitialRecords()
        Dim specialQDInitialRecords As DataSet
        Try
            ViewState("RMDSpecialQDInitialRecords") = Nothing
            specialQDInitialRecords = YMCARET.YmcaBusinessObject.MRDBO.GetRMDSpecialQDInitialLetterParticipants()

            If HelperFunctions.isNonEmpty(specialQDInitialRecords) Then
                ViewState("RMDSpecialQDInitialRecords") = specialQDInitialRecords.Tables(0)
                If specialQDInitialRecords.Tables(0).Rows.Count > 0 Then
                    EnablePrintButtons(True)
                Else
                    EnablePrintButtons(False)
                End If
            Else
                EnablePrintButtons(False)
            End If
        Catch
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Get records for Follow up records after initial letters get printed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetFollowupRecords()
        Dim specialQDFolloupRecords As DataSet

        Try
            ViewState("RMDSpecialQDFollowupRecords") = Nothing
            specialQDFolloupRecords = YMCARET.YmcaBusinessObject.MRDBO.GetRMDSpecialQDFollowupLetterParticipants()

            If HelperFunctions.isNonEmpty(specialQDFolloupRecords) Then
                ViewState("RMDSpecialQDFollowupRecords") = specialQDFolloupRecords.Tables(0)
                If specialQDFolloupRecords.Tables(0).Rows.Count > 0 Then
                    EnablePrintButtons(True)
                Else
                    EnablePrintButtons(False)
                End If
            Else
                EnablePrintButtons(False)
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub gvLetters_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLetters.RowDataBound
        Dim specialQDRecords As New DataTable
        Dim specialQDRows As DataRow()
        Dim processDate As Date
        Dim followUpLetterPrintDate As String
        Dim lblDash As Label
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("RMDPrintLetters_Initial_sort"), e)
                If SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                    e.Row.Cells(RMD_LETTER_INITIAL_LASTGENERATED_LINK).ControlStyle.CssClass = "show"
                    e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).ControlStyle.CssClass = "show"
                    e.Row.Cells(RMD_LETTER_INITIAL_LASTGENERATED_LINK).CssClass = "show"
                    e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).CssClass = "show"
                ElseIf SelectedTab = PrintLetterTabStrips.INITIAL Then
                    e.Row.Cells(RMD_LETTER_INITIAL_LASTGENERATED_LINK).ControlStyle.CssClass = "hide"
                    e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).ControlStyle.CssClass = "hide"
                    e.Row.Cells(RMD_LETTER_INITIAL_LASTGENERATED_LINK).CssClass = "hide"
                    e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).CssClass = "hide"
                End If
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                If SelectedTab = PrintLetterTabStrips.INITIAL Then
                    specialQDRecords = DirectCast(ViewState("RMDSpecialQDInitialRecords"), DataTable)
                    e.Row.Cells(RMD_LETTER_INITIAL_LASTGENERATED_LINK).ControlStyle.CssClass = "hide"
                    e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).ControlStyle.CssClass = "hide"
                ElseIf SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                    specialQDRecords = DirectCast(ViewState("RMDSpecialQDFollowupRecords"), DataTable)
                    e.Row.Cells(RMD_LETTER_INITIAL_LASTGENERATED_LINK).ControlStyle.CssClass = "show"
                    ActivateLink("RMDSpecialQDInitialLetter", "lnkReprintInitialLetter", e)

                    e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).ControlStyle.CssClass = "show"
                    followUpLetterPrintDate = e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED).Text
                    If followUpLetterPrintDate = "" Or followUpLetterPrintDate.ToUpper() = "&NBSP;" Then
                        lblDash = TryCast(e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).FindControl("lblDash"), Label)
                        If Not lblDash Is Nothing Then
                            lblDash.Style("display") = "normal"
                        Else
                            e.Row.Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED_LINK).Text = "-"
                        End If
                    Else
                        ActivateLink("RMDSpecialQDFollowupLetter", "lnkReprintFollowupLetter", e)
                    End If
                End If

                specialQDRows = specialQDRecords.Select("FundIdNo = '" + e.Row.Cells(RMD_LETTER_FUNDNO).Text + "'")
                If specialQDRows.Length > 0 Then
                    For Each row As DataRow In specialQDRows
                        If lbllnkFollowup.Visible = True Then
                            If Not row("FollowupGenerated") Is Nothing Then
                                If tdFollow.Visible And row("FollowupGenerated").ToString.ToLower = "yes" And tdFollowUpLegends.Visible Then
                                    e.Row.CssClass = "BG_ColourFollowUpLetter"
                                End If
                            End If
                        End If
                        If row("ISLocked") Then
                            e.Row.CssClass = "BG_ColourIsLocked"
                        ElseIf Convert.ToDouble(row("CurrentBalance").ToString()) <= 0 Or (Convert.ToDouble(row("CurrentBalance").ToString()) < (Convert.ToDouble(row("MRDAmount").ToString()) - Convert.ToDouble(row("PaidAmount").ToString()))) Then
                            e.Row.CssClass = "BG_ColourIsBlocked"
                        End If
                    Next
                End If
                processDate = Convert.ToDateTime(e.Row.Cells(RMD_LETTER_DUEDATE).Text)
                e.Row.Cells(RMD_LETTER_DUEDATE).Text = processDate.ToString("MM/dd/yyyy")
                e.Row.Cells(RMD_LETTER_NAME).Text = System.Web.HttpUtility.HtmlDecode(e.Row.Cells(RMD_LETTER_NAME).Text)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_gvLetters_RowDataBound", ex)
        End Try
    End Sub

    Private Sub gvLetters_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvLetters.Sorting
        Dim specialQDView As New DataView
        Dim specialQDRecords As DataTable
        Dim sortExpression As String
        Try
            specialQDRecords = Nothing
            sortExpression = e.SortExpression
            If lbllnkInitial.Visible Then
                specialQDRecords = DirectCast(ViewState("RMDSpecialQDInitialRecords"), DataTable)
            ElseIf lbllnkFollowup.Visible = True Then
                specialQDRecords = DirectCast(ViewState("RMDSpecialQDFollowupRecords"), DataTable)
            End If
            If HelperFunctions.isNonEmpty(specialQDRecords) Then
                specialQDView = specialQDRecords.DefaultView
                specialQDView.Sort = sortExpression
                If ViewState("RMDPrintLetters_Initial_Filter") IsNot Nothing Then
                    specialQDView.RowFilter = ViewState("RMDPrintLetters_Initial_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("RMDPrintLetters_Initial_sort"), e.SortExpression, specialQDView)
                HelperFunctions.BindGrid(gvLetters, specialQDView, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_gvLetters_Sorting", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Redirect to mainpage.aspx
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessionVariables()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_btnClose_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Get Last Processing date of RMD
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetLastProccesedDate()
        Dim lastProcessedDate As String = String.Empty
        Try
            lastProcessedDate = YMCARET.YmcaBusinessObject.MRDBO.GetLastRMDProcessedDate()
            If String.IsNullOrEmpty(lastProcessedDate) Then
                lastProcessedDate = "12/31/2010"
            End If
            ViewState("ProcessDate") = lastProcessedDate
            lblMRDMsg.Text = " Last RMD Closed Batch: " + lastProcessedDate
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_btnSearch_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filter Data based on value select/provide from screen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SearchRecords()
        Dim specialQDView As New DataView
        Dim specialQDRecords As DataTable
        Dim sort As GridViewCustomSort
        Dim strFilter As String = String.Empty
        Try
            specialQDRecords = Nothing
            If SelectedTab = PrintLetterTabStrips.INITIAL Then
                specialQDRecords = DirectCast(ViewState("RMDSpecialQDInitialRecords"), DataTable)
            ElseIf SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                specialQDRecords = DirectCast(ViewState("RMDSpecialQDFollowupRecords"), DataTable)
            End If

            If HelperFunctions.isNonEmpty(specialQDRecords) Then
                specialQDView = specialQDRecords.DefaultView
                If ddlAcctLocked.SelectedValue <> "All" Then
                    strFilter = String.Format("ISLocked = {0}", ddlAcctLocked.SelectedValue)
                End If
                If ddlInsufficientBal.SelectedValue <> "All" Then
                    If ddlInsufficientBal.SelectedValue = "Yes" Then
                        If strFilter = String.Empty Then
                            strFilter = "(CurrentBalance <= 0 OR CurrentBalance < (MRDAmount - PaidAmount))"
                        Else
                            strFilter += " AND (CurrentBalance <= 0 OR CurrentBalance < (MRDAmount - PaidAmount))"
                        End If
                    ElseIf ddlInsufficientBal.SelectedValue = "No" Then
                        If strFilter = String.Empty Then
                            strFilter = "CurrentBalance > 0"
                        Else
                            strFilter += " AND CurrentBalance > 0"
                        End If
                    End If
                End If

                If txtFundId.Text <> String.Empty Then
                    If strFilter = String.Empty Then
                        strFilter = String.Format("FundIdNo = '{0}'", txtFundId.Text)
                    Else
                        strFilter += String.Format(" AND FundIdNo = '{0}'", txtFundId.Text)
                    End If
                End If
                If lbllnkFollowup.Visible = True Then
                    If ddlFollowUp.SelectedValue <> "All" Then
                        If strFilter = String.Empty Then
                            strFilter = String.Format("FollowupGenerated = '{0}'", ddlFollowUp.SelectedValue)
                        Else
                            strFilter += String.Format(" AND FollowupGenerated = '{0}'", ddlFollowUp.SelectedValue)
                        End If
                    End If
                End If
                specialQDView.RowFilter = strFilter

                ViewState("RMDPrintLetters_Initial_Filter") = strFilter
                If ViewState("RMDPrintLetters_Initial_sort") IsNot Nothing Then
                    sort = ViewState("RMDPrintLetters_Initial_sort")
                    specialQDView.Sort = sort.SortExpression + " " + sort.SortDirection
                End If
                If HelperFunctions.isNonEmpty(specialQDView) Then
                    If specialQDView.Table.Rows.Count > 0 Then
                        EnablePrintButtons(True)
                    Else
                        EnablePrintButtons(False)
                    End If
                Else
                    EnablePrintButtons(False)
                End If
            End If
            HelperFunctions.BindGrid(gvLetters, specialQDView, True)
        Catch
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Print Initial / Followup Letters based on selection.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintLetters_Click(sender As Object, e As EventArgs) Handles btnPrintLetters.Click
        Dim selectedParticipantCounter As Integer
        Try
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            End If
            GetSelectedInitialRMDLetterRecords()
            selectedParticipantCounter = 0
            If HelperFunctions.isNonEmpty(Me.SelectedSpecialQDParticipants) Then
                selectedParticipantCounter = Me.SelectedSpecialQDParticipants.Tables(0).Rows.Count
            End If
            If selectedParticipantCounter > 0 Then
                BatchProcessProgressControl.AllowReprint = True
                If BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDSpecialQDInitLetters.ToString() Then
                    BatchProcessProgressControl.FolderForReprint = "RMDSpecialQDInitialLetter"
                ElseIf BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDSpecialQDFollowLetters.ToString() Then
                    BatchProcessProgressControl.FolderForReprint = "RMDSpecialQDFollowupLetter"
                End If
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", String.Format("{0}('Are you sure you want to print Letter for selected {1} record(s)');", BatchProcessProgressControl.OpenConfirmDialogMethodName, selectedParticipantCounter.ToString()), True)
            Else
                HelperFunctions.ShowMessageToUser("Please select the record(s) to print the letter", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_btnPrintLetters_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Print SSRS reports for Inital and Followup
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrintList_Click(sender As Object, e As EventArgs) Handles btnPrintList.Click
        Try
            'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            'If Me.IsReadOnlyAccess Then
            'HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
            'Exit Sub
            'End If
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            GetSelectedRecords()
            If SelectedTab = PrintLetterTabStrips.INITIAL Then
                Session("ReportName") = "RMDSpecialQDInitialLetterList"
            ElseIf SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                Session("ReportName") = "RMDSpecialQDFollowupLetterList"
            End If
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_btnPrintList_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Get Selected records from gridview
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Sub GetSelectedRecords()
        Dim printLetters As New DataTable
        Dim printLettersRow As DataRow
        Dim dateText As String

        printLetters.Columns.Add("FundNo")
        printLetters.Columns.Add("PlanType")
        printLetters.Columns.Add("CurrentBalance")
        printLetters.Columns.Add("RMDAmount")
        printLetters.Columns.Add("PaidAmount")
        printLetters.Columns.Add("FundStatus")
        printLetters.Columns.Add("DueDate")
        printLetters.Columns.Add("Selected")
        printLetters.Columns.Add("ProcessDate")
        If SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
            printLetters.Columns.Add("InitialLetterLastGenerated")
            printLetters.Columns.Add("FollowupLetterLastGenerated")
        End If
        printLetters.Columns.Add("ReportName")

        If HelperFunctions.isNonEmpty(gvLetters) Then
            For iCount As Integer = 0 To gvLetters.Rows.Count - 1
                Dim chkBox As New CheckBox
                chkBox = CType(gvLetters.Rows(iCount).FindControl("chkSelect"), CheckBox)
                If Not chkBox Is Nothing Then
                    printLettersRow = printLetters.NewRow
                    If chkBox.Checked Then
                        printLettersRow("Selected") = "Yes"
                    Else
                        printLettersRow("Selected") = "No"
                    End If
                    printLettersRow("FundNo") = gvLetters.Rows(iCount).Cells(RMD_LETTER_FUNDNO).Text
                    printLettersRow("PlanType") = gvLetters.Rows(iCount).Cells(RMD_LETTER_PLANTYPE).Text
                    printLettersRow("CurrentBalance") = gvLetters.Rows(iCount).Cells(RMD_LETTER_CURRENTBALANCE).Text
                    printLettersRow("RMDAmount") = gvLetters.Rows(iCount).Cells(RMD_LETTER_RMDAMOUNT).Text
                    printLettersRow("PaidAmount") = gvLetters.Rows(iCount).Cells(RMD_LETTER_PAIDAMOUNT).Text
                    printLettersRow("FundStatus") = gvLetters.Rows(iCount).Cells(RMD_LETTER_FUNDSTATUS).Text
                    printLettersRow("DueDate") = gvLetters.Rows(iCount).Cells(RMD_LETTER_DUEDATE).Text
                    printLettersRow("ProcessDate") = gvLetters.Rows(iCount).Cells(RMD_LETTER_DUEDATE).Text
                    If lnkInitial.Visible Then
                        dateText = gvLetters.Rows(iCount).Cells(RMD_LETTER_INITIAL_LASTGENERATED).Text
                        If dateText.ToUpper() = "&NBSP;" Then
                            dateText = "-"
                        End If
                        printLettersRow("InitialLetterLastGenerated") = dateText

                        dateText = gvLetters.Rows(iCount).Cells(RMD_LETTER_FOLLOWUP_LASTGENERATED).Text
                        If dateText.ToUpper() = "&NBSP;" Then
                            dateText = "-"
                        End If
                        printLettersRow("FollowupLetterLastGenerated") = dateText

                        printLettersRow("ReportName") = "Special QDRO Participants Follow-up Letters"
                    ElseIf lnkFollowup.Visible Then
                        printLettersRow("ReportName") = "Special QDRO Participants Initial Letters"
                    End If
                    printLetters.Rows.Add(printLettersRow)
                End If
            Next
            SessionManager.SessionParticipantRMDs.SpecialQDROLetters = printLetters
        End If
    End Sub

    Private Sub GetSelectedInitialRMDLetterRecords()
        Dim selectedRecordsDataset As DataSet
        Dim selectedRecords As DataTable
        Dim chkSelectedRMD As CheckBox
        Dim selectedRow As DataRow
        Dim counter As Integer
        Dim MRDDueDate As String
        Try
            MRDDueDate = DateTime.Now.ToShortDateString()
            If HelperFunctions.isNonEmpty(gvLetters) Then
                selectedRecords = New DataTable()
                selectedRecords.Columns.Add("LetterCode")
                selectedRecords.Columns.Add("PersonId")
                selectedRecords.Columns.Add("RefRequestID")
                selectedRecords.Columns.Add("FUNDNo")
                selectedRecords.Columns.Add("SSNo")
                selectedRecords.Columns.Add("FirstName")
                selectedRecords.Columns.Add("LastName")
                selectedRecords.Columns.Add("MiddleName")
                selectedRecords.Columns.Add("PlanType")
                selectedRecords.Columns.Add("RmdAmount")
                selectedRecords.Columns.Add("PaidAmount")
                selectedRecords.Columns.Add("Name")
                selectedRecords.Columns.Add("Fundstatus")
                selectedRecords.Columns.Add("Duedate")
                selectedRecords.Columns.Add("CurrentBalance")
                selectedRecords.Columns.Add("FundEventId")

                'Setting module name for printing letters
                If SelectedTab = PrintLetterTabStrips.INITIAL Then
                    BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDSpecialQDInitLetters.ToString()
                ElseIf SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                    BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDSpecialQDFollowLetters.ToString()
                End If

                'setting batch ID
                BatchProcessProgressControl.BatchID = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(MRDDueDate).Tables(0).Rows(0)(0))

                For counter = 0 To gvLetters.Rows.Count - 1
                    chkSelectedRMD = gvLetters.Rows(counter).FindControl("chkSelect")
                    If Not IsNothing(chkSelectedRMD) Then
                        If chkSelectedRMD.Checked = True Then
                            selectedRow = selectedRecords.NewRow()
                            If SelectedTab = PrintLetterTabStrips.INITIAL Then
                                selectedRow("LetterCode") = CommonClass.RPTLetter.RMDIAPLT.ToString()
                            ElseIf SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                                selectedRow("LetterCode") = CommonClass.RPTLetter.RMDFAPLT.ToString()
                            End If
                            selectedRow("PersonId") = gvLetters.Rows(counter).Cells(RMD_LETTER_PERSID).Text.ToString()
                            selectedRow("RefRequestID") = ""
                            selectedRow("FUNDNo") = gvLetters.Rows(counter).Cells(RMD_LETTER_FUNDNO).Text.ToString()
                            selectedRow("SSNo") = gvLetters.Rows(counter).Cells(RMD_LETTER_SSNO).Text.ToString()
                            selectedRow("FirstName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_FIRSTNAME).Text.ToString())
                            selectedRow("LastName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_LASTNAME).Text.ToString())
                            selectedRow("MiddleName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_MIDDLENAME).Text.ToString())

                            selectedRow("PlanType") = gvLetters.Rows(counter).Cells(RMD_LETTER_PLANTYPE).Text.ToString()
                            selectedRow("RmdAmount") = gvLetters.Rows(counter).Cells(RMD_LETTER_RMDAMOUNT).Text.ToString()
                            selectedRow("PaidAmount") = gvLetters.Rows(counter).Cells(RMD_LETTER_PAIDAMOUNT).Text.ToString()
                            selectedRow("Name") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_NAME).Text.ToString())
                            selectedRow("Fundstatus") = gvLetters.Rows(counter).Cells(RMD_LETTER_FUNDSTATUS).Text.ToString()
                            selectedRow("Duedate") = gvLetters.Rows(counter).Cells(RMD_LETTER_DUEDATE).Text.ToString()
                            selectedRow("CurrentBalance") = gvLetters.Rows(counter).Cells(RMD_LETTER_CURRENTBALANCE).Text.ToString()
                            selectedRow("FundEventId") = gvLetters.Rows(counter).Cells(RMD_LETTER_FUNDID).Text.ToString()
                            selectedRecords.Rows.Add(selectedRow)
                        End If
                    End If
                Next
                selectedRecords.TableName = "SelectedBatchRecords"
                selectedRecordsDataset = New DataSet()
                selectedRecordsDataset.Tables.Add(selectedRecords)
                Me.SelectedSpecialQDParticipants = selectedRecordsDataset
            End If
        Catch
            Throw
        Finally
            MRDDueDate = Nothing
            selectedRow = Nothing
            chkSelectedRMD = Nothing
            selectedRecords = Nothing
            selectedRecordsDataset = Nothing
        End Try
    End Sub

    Private Sub lnkInitial_Click(sender As Object, e As EventArgs) Handles lnkInitial.Click
        Try
            SelectedTab = PrintLetterTabStrips.INITIAL
            ShowHideInitialLetterTabDetails(True)
            ShowHideFollowupLetterTabDetails(False)
            ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            ViewState("RMDPrintLetters_Initial_sort") = Nothing
            GetInitialRecords()
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_lnkInitial_Click", ex)
        End Try
    End Sub

    Private Sub lnkFollowup_Click(sender As Object, e As EventArgs) Handles lnkFollowup.Click
        Try
            SelectedTab = PrintLetterTabStrips.FOLLOWUP
            ShowHideInitialLetterTabDetails(False)
            ShowHideFollowupLetterTabDetails(True)
            GetFollowupRecords()
            ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            ViewState("RMDPrintLetters_Initial_sort") = Nothing
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDROLetters_lnkInitial_Click", ex)
        End Try
    End Sub

    Private Sub BatchProcessProgressControl_HandlePUCCloseButtonClick(sender As Object, e As EventArgs) Handles BatchProcessProgressControl.HandlePUCCloseButtonClick
        Dim specialQDFollowup, specialQDInitial As DataSet
        Try
            If SelectedTab = PrintLetterTabStrips.FOLLOWUP Then
                specialQDFollowup = YMCARET.YmcaBusinessObject.MRDBO.GetRMDSpecialQDFollowupLetterParticipants()

                ViewState("RMDSpecialQDFollowupRecords") = Nothing
                If HelperFunctions.isNonEmpty(specialQDFollowup) Then
                    ViewState("RMDSpecialQDFollowupRecords") = specialQDFollowup.Tables(0)
                End If
            ElseIf SelectedTab = PrintLetterTabStrips.INITIAL Then
                specialQDInitial = YMCARET.YmcaBusinessObject.MRDBO.GetRMDSpecialQDInitialLetterParticipants()

                ViewState("RMDSpecialQDInitialRecords") = Nothing
                If HelperFunctions.isNonEmpty(specialQDInitial) Then
                    ViewState("RMDSpecialQDInitialRecords") = specialQDInitial.Tables(0)
                End If
            End If

            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDSpecialQDRO_btnClosePop_Click", ex)
        Finally
            specialQDFollowup = Nothing
            specialQDInitial = Nothing
        End Try
    End Sub

    'To Display filter fields and messages for Initial Print Letter Tab 
    Private Sub ShowHideInitialLetterTabDetails(ByVal isVisible As Boolean)
        lnkInitial.Visible = Not isVisible
        lbllnkInitial.Visible = isVisible
        If (isVisible) Then
            btnPrintLetters.Text = "Print Initial Letter"
            lblDescription.Text = "List of person(s) eligible for generating Special QDRO Participants initial letters."

            ddlAcctLocked.SelectedValue = "All"
            ddlFollowUp.SelectedValue = "All"
            ddlInsufficientBal.SelectedValue = "No"
        End If
    End Sub

    'To Display filter fields and messages for Follow up Letter Tab 
    Private Sub ShowHideFollowupLetterTabDetails(ByVal isVisible As Boolean)
        tdFollow.Visible = isVisible
        tdFollowUpLegends.Visible = isVisible
        ddlFollowUp.Visible = isVisible
        txtFollowup.Visible = isVisible
        lnkFollowup.Visible = Not isVisible
        lbllnkFollowup.Visible = isVisible
        If (isVisible) Then
            btnPrintLetters.Text = "Print Follow-up Letter"
            lblDescription.Text = "List of person(s) eligible for generating Special QDRO Participants follow-up letters."

            ddlAcctLocked.SelectedValue = "All"
            ddlFollowUp.SelectedValue = "All"
            ddlInsufficientBal.SelectedValue = "No"
        End If
    End Sub

#Region "Property"
    Public Property SelectedTab() As PrintLetterTabStrips     ' We are setting this property to set the value of the selected tab in print letter 
        Get
            If Not (ViewState("SelectedTab")) Is Nothing Then
                Return (CType(ViewState("SelectedTab"), PrintLetterTabStrips))
            Else
                Return PrintLetterTabStrips.NONE
            End If
        End Get
        Set(ByVal Value As PrintLetterTabStrips)
            ViewState("SelectedTab") = Value
        End Set
    End Property

    Public Property SelectedSpecialQDParticipants() As DataSet
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

    Private Sub ClearSessionVariables()
        Session("ReportName") = Nothing
        SessionManager.SessionParticipantRMDs.SpecialQDROLetters = Nothing
        Me.SelectedSpecialQDParticipants = Nothing
        Me.ReadOnlyWarningMessage = Nothing
        Session("IsReadOnlyAccess") = Nothing
    End Sub

    Private Sub ActivateLink(ByVal folderName As String, ByVal controlName As String, ByVal e As GridViewRowEventArgs)
        Dim fundEventId, physicalPath, relativePath As String
        Dim row As GridViewRow
        Dim encryptLink As EncryptedQueryString
        Dim link As String
        Dim linkButton As LinkButton
        Try
            linkButton = Nothing
            row = e.Row
            fundEventId = row.Cells(RMD_LETTER_FUNDID).Text.Trim()
            physicalPath = String.Format("{0}\{1}\{2}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), folderName, fundEventId)
            relativePath = String.Format("{0}\\{1}\\{2}.pdf", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), folderName, fundEventId)

            linkButton = TryCast(row.FindControl(controlName), LinkButton)
            If Not linkButton Is Nothing Then
                linkButton.Attributes.Remove("href")
                If File.Exists(physicalPath) Then
                    encryptLink = New EncryptedQueryString
                    encryptLink.Add("link", relativePath)
                    link = String.Format("javascript:return OpenPDF('DocumentViewer.aspx?p={0}')", encryptLink.ToString())
                    linkButton.OnClientClick = link
                Else
                    linkButton.Attributes.CssStyle(HtmlTextWriterStyle.Color) = "black"
                    linkButton.Attributes.CssStyle(HtmlTextWriterStyle.Cursor) = "default"
                    linkButton.Attributes.CssStyle(HtmlTextWriterStyle.TextDecoration) = "none"
                    linkButton.ToolTip = ""
                    linkButton.OnClientClick = Nothing
                End If
            End If
        Catch
            Throw
        Finally
            link = Nothing
            encryptLink = Nothing
            row = Nothing
            fundEventId = Nothing
            physicalPath = Nothing
            relativePath = Nothing
        End Try
    End Sub

    Private Sub EnablePrintButtons(ByVal isEnable As Boolean)
        btnPrintLetters.Enabled = isEnable
        btnPrintList.Enabled = isEnable
    End Sub

    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("RMDSpecialQDROLetters.aspx", Convert.ToInt32(Session("LoggedUserKey")))
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
End Class