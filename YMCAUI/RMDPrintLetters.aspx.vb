'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	RMDPrintLetters.aspx.vb
' Author Name		:	Anudeep  
' Contact No		:	
' Creation Date		:	04/25/2014
' Description		:	This form is used to print initial letters for new persons who are eleigble for current year MRD for first time
' & generate follow up letter who are already took inital letters.
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.
'Anudeep            10/27/2014      BT:2691-Get the Merge PDF path from the configuration key
'Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
'Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations
'Dinesh Kanojia     01/06/2015      BT:2735:Add activities logging functionality in RMD and RollIn batch creation.
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Chandra sekar      2016.10.24      YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
'Santosh Bura		2016.10.19 		YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380)  
'Santosh Bura       2016.11.21      YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224) 
'Santosh Bura       2017.01.12      YRS-AT-3298 -  YRS enh: RMD Print Letters screen: treat plans separately 
'Pramod  P. Pokale  2017.04.17      YRS-AT-3237 -  YRS bug: RMD Print Letters changes year when some letters generated (TrackIT 28170)
'Pramod  P. Pokale  2017.04.21      YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186)
'Manthan Rajguru    2017.05.10      YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186)
'Shilpa N         | 02/28/2019  |   YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'*******************************************************************************
Imports YMCAObjects
Imports System.IO ' SB | 10/23/2016 | YRS-AT-2685

Public Class RMDPrintLetters
    Inherits System.Web.UI.Page
    'Dim IDMALL As New IDMforAll 'MMR | 2017.05.15 | YRS-AT-3356 | Commented as not used anywhere
    'START : CS | 2016.10.24 | YRS-AT-3088 |  Declared the Constant for the Data grid Cells index
    Private Const RMD_LETTER_PERSID As Integer = 1
    Private Const RMD_LETTER_FUNDNO As Integer = 2
    Private Const RMD_LETTER_SSNO As Integer = 10
    Private Const RMD_LETTER_FIRSTNAME As Integer = 11
    Private Const RMD_LETTER_LASTNAME As Integer = 12
    Private Const RMD_LETTER_MIDDLENAME As Integer = 13
    Private Const RMD_LETTER_ISCASHOUTELIGIBLE As Integer = 14
    Private Const RMD_LETTER_PLANTYPE As Integer = 3
    Private Const RMD_LETTER_RMDAMOUNT As Integer = 6
    Private Const RMD_LETTER_PAIDAMOUNT As Integer = 7
    Private Const RMD_LETTER_NAME As Integer = 4
    Private Const RMD_LETTER_FUNDSTATUS As Integer = 8
    Private Const RMD_LETTER_DUEDATE As Integer = 9
    Private Const RMD_LETTER_CURRENTBALANCE As Integer = 5
    'END : CS | 2016.10.24 | YRS-AT-3088 |  Declared the Constant for the Data grid Cells index

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
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMD Print Letters page load", "Page Load Call.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            'If Not IsPostBack Then   
            '    ClearSessionVariables()   'Function to Reset all the session variables
            'End If
            'START: PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence RMDLettersProgressPopupClosed property is no longer required
            'If Not IsPostBack Or RMDLettersProgressPopupClosed = True Then   ' SB | 10/13/2016 | YRS-AT-2685 | To maintain the state which tab is selected after letter generation new condition is added
            If Not IsPostBack Then
                'END: PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence RMDLettersProgressPopupClosed property is no longer required
                'Load Default values from DB
                CheckAccessRights() 'MMR | 2017.05.16 | YRS-AT-3356 | called to store read only access rights and warning message
                GetLastProccesedDate()
                'GetInitialRecords() ' SB | 10/13/2016 | YRS-AT-2685 |  Renamed old Fuction name from 'GetInititialRecords' to 'GetInitialRecords',Commented avoid multiple repeatation

                ' START : SB | 10/13/2016 | YRS-AT-2685 | Instead of storing values in Session now values are stored in ViewState and selected tab information is stored in ENUM 
                'If (Not Session("Selecttab") Is Nothing) Then
                '    If Convert.ToBoolean(Session("Selecttab")) Then
                '        lnkFollowup_Click(sender, e)
                '    Else
                '        lnkInitial_Click(sender, e)
                '    End If
                '    'Session("Selecttab") = Nothing
                'Else
                '    lnkInitial_Click(sender, e)
                'End If

                If Not (SelectedTab = EnumTabStrips.NONE) Then

                    If (SelectedTab = EnumTabStrips.INITIAL) Then
                        lnkInitial_Click(sender, e)
                    ElseIf (SelectedTab = EnumTabStrips.FOLLOWUP) Then
                        lnkFollowup_Click(sender, e)
                    ElseIf (SelectedTab = EnumTabStrips.REPRINT) Then
                        lnkReprintLetter_Click(sender, e)
                    End If
                Else
                    lnkInitial_Click(sender, e)
                End If
                ' END : SB | 10/13/2016 | YRS-AT-2685 | Instead of storing values in Session now values are stored in ViewState and selected tab information is stored in ENUM 
                BatchProcessProgressControl.DialogTitle = "RMD Print Letters" 'PPP | 04/21/2017 | YRS-AT-3356 | Setting dialog title
            End If

            ' START : SB | 10/13/2016 | YRS-AT-2685 | Previous code commented and new code resets the value of state variables 
            'If Not Session("Close") Is Nothing Then
            '    Dim dtRecords As DataTable
            '    Dim dv As DataView
            '    ViewState("RMDPrintLetters_dtFolloupRecords") = Nothing
            '    If lbllnkInitial.Visible Then
            '        GetInitialRecords() ' SB | 10/13/2016 | YRS-AT-2685 |  Renamed old Fuction name from 'GetInititialRecords' to 'GetInitialRecords'
            '        dtRecords = DirectCast(ViewState("RMDPrintLetters_dtInitialRecords"), DataTable)
            '    ElseIf lbllnkFollowup.Visible = True Then
            '        GetFollowupRecords()
            '        dtRecords = DirectCast(ViewState("RMDPrintLetters_dtFolloupRecords"), DataTable)
            '    End If
            '    If HelperFunctions.isNonEmpty(dtRecords) Then
            '        dv = dtRecords.DefaultView
            '        If ViewState("RMDPrintLetters_Initial_Filter") IsNot Nothing Then
            '            dv.RowFilter = ViewState("RMDPrintLetters_Initial_Filter")
            '        End If
            '        If ViewState("RMDPrintLetters_Initial_sort") IsNot Nothing Then
            '            dv.Sort = ViewState("RMDPrintLetters_Initial_sort")
            '        End If
            '    End If

            '    HelperFunctions.BindGrid(gvLetters, dv, True)
            '    'START : CS | 2016.10.24 | YRS-AT-3088 | Below logic is moved to aspx file
            '    'Dim strFileName As String
            '    ''Start:AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
            '    'Dim strPath As String
            '    'strFileName = Session("Rollin_MergedPdfs_Filename")
            '    'strPath = System.Configuration.ConfigurationSettings.AppSettings("MergePDFPath") + "\"
            '    'If FileIO.FileSystem.FileExists(HttpContext.Current.Server.MapPath("~\" + strPath) + strFileName) Then
            '    '    strFileName = strPath + strFileName
            '    '    'ScriptManager.RegisterStartupScript(Me, GetType(Page), "CallPdf", "OpenPDF('" + strFileName + "');", True)
            '    'End If
            '    'END : CS | 2016.10.24 | YRS-AT-3088 | Below logic is moved to aspx file
            '    'End:AA: 10/27/2014 BT:2691- Changed to get the value from the configuration key to avoid access denied error
            '    Session("Rollin_MergedPdfs_Filename") = Nothing
            '    Session("Close") = Nothing
            'End If

            'START: PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence RMDLettersProgressPopupClosed property is no longer required
            'If RMDLettersProgressPopupClosed = True Then
            '    ViewState("RMDPrintLetters_dtFolloupRecords") = Nothing
            '    Session("Rollin_MergedPdfs_Filename") = Nothing
            '    RMDLettersProgressPopupClosed = False
            '    Exit Sub
            'End If
            'END: PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence RMDLettersProgressPopupClosed property is no longer required

            ' END : SB | 10/13/2016 | YRS-AT-2685 | Previous code commented and new code resets the value of state variables 
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_Page_Load", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Get First time eligible RMD for Current year for initail Process
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetInitialRecords() ' SB | 10/13/2016 | YRS-AT-2685 |  Renamed old Fuction name from 'GetInititialRecords' to 'GetInitialRecords'
        Dim dsInitialRecords As DataSet
        Dim dtInitialRecords As DataTable
        Dim strMrdYear() As String = {"MRDYear"}
        Dim lstItem As ListItem
        Try

            dsInitialRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchRMDForInitialLetter()
            dtInitialRecords = dsInitialRecords.Tables(0).DefaultView.ToTable(True, strMrdYear)
            ddlRMDYear.Items.Clear()
            'If Not ddlRMDYear.Items.Count > 0 Then
            For iCount As Integer = 0 To dtInitialRecords.Rows.Count - 1
                lstItem = New ListItem
                lstItem.Text = dtInitialRecords.Rows(iCount)("MRDYear").ToString
                lstItem.Value = dtInitialRecords.Rows(iCount)("MRDYear").ToString
                ddlRMDYear.Items.Add(lstItem)
            Next
            If dtInitialRecords.Rows.Count > 0 Then
                ddlRMDYear.Items(dtInitialRecords.Rows.Count - 1).Selected = True
            End If
            'End If

            If dsInitialRecords.Tables.Count > 0 Then
                ViewState("RMDPrintLetters_dtInitialRecords") = dsInitialRecords.Tables(0)
            Else
                ViewState("RMDPrintLetters_dtInitialRecords") = Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Get records for Follow up records after initial letters get printed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetFollowupRecords()
        Dim dsFolloupRecords As DataSet
        Dim strMrdYear() As String = {"MRDYear"}
        Dim dtFollowUp As DataTable
        Dim lstItem As ListItem
        Try
            'If ViewState("RMDPrintLetters_dtFolloupRecords") Is Nothing Then
            dsFolloupRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchRMDForFollowupLetter()
            dtFollowUp = dsFolloupRecords.Tables(0).DefaultView.ToTable(True, strMrdYear)
            ddlRMDYear.Items.Clear()
            'If Not ddlRMDYear.Items.Count > 0 Then
            For iCount As Integer = 0 To dtFollowUp.Rows.Count - 1
                lstItem = New ListItem
                lstItem.Text = dtFollowUp.Rows(iCount)("MRDYear").ToString
                lstItem.Value = dtFollowUp.Rows(iCount)("MRDYear").ToString
                ddlRMDYear.Items.Add(lstItem)
            Next
            If dtFollowUp.Rows.Count > 0 Then
                ddlRMDYear.Items(dtFollowUp.Rows.Count - 1).Selected = True
            End If
            ' End If

            If dsFolloupRecords.Tables.Count > 0 Then
                ViewState("RMDPrintLetters_dtFolloupRecords") = dsFolloupRecords.Tables(0)
            Else
                ViewState("RMDPrintLetters_dtFolloupRecords") = Nothing
            End If
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub gvLetters_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLetters.RowDataBound
        Dim dtRecords As New DataTable
        Dim dra As DataRow()
        Dim isReprintLetter As Boolean = False ' SB | 10/23/2016 | YRS-AT-2685 |  Defining variable to check Re-print RMD Letter tab is selected or not
        Dim processDate As Date   ' SB | 10/23/2016 | YRS-AT-2685 |  Defining variable to convert date timestamp to date format only
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("RMDPrintLetters_Initial_sort"), e)
                'START: SB | 10/23/2016 | YRS-AT-2685 | Hiding checkbox column from "Reprint Letters" tab
                If SelectedTab = EnumTabStrips.REPRINT Then
                    e.Row.Cells(0).Visible = False
                End If
                'END: SB | 10/23/2016 | YRS-AT-2685 | Hiding checkbox column from "Reprint Letters" tab
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                If SelectedTab = EnumTabStrips.INITIAL Then      ' SB | 10/23/2016 | YRS-AT-2685 |  Checking if Initial tab is selected
                    'If lbllnkInitial.Visible = True Then        ' SB | 10/23/2016 | YRS-AT-2685 |  Instead of checking which label is visible, SelectedTab value is now checked    
                    dtRecords = DirectCast(ViewState("RMDPrintLetters_dtInitialRecords"), DataTable)
                ElseIf SelectedTab = EnumTabStrips.FOLLOWUP Then  ' SB | 10/23/2016 | YRS-AT-2685 |  Checking if Follow-up tab is selected
                    'ElseIf lbllnkFollowup.Visible = True Then    ' SB | 10/23/2016 | YRS-AT-2685 |  Instead of checking which label is visible, SelectedTab value is now checked    
                    dtRecords = DirectCast(ViewState("RMDPrintLetters_dtFolloupRecords"), DataTable)
                    ' START : SB | 10/23/2016 | YRS-AT-2685 | If Reprint RMD Letter tab is selected then assign value 
                ElseIf SelectedTab = EnumTabStrips.REPRINT Then
                    'ElseIf lbllnkReprintLetter.Visible = True Then
                    isReprintLetter = True
                    e.Row.Cells(0).Visible = False
                    ' END : SB | 10/23/2016 | YRS-AT-2685 | If Reprint RMD Letter tab is selected then assign value 
                End If

                isReprintLetter = (SelectedTab = EnumTabStrips.INITIAL Or SelectedTab = EnumTabStrips.FOLLOWUP)  ' SB | 10/23/2016 | YRS-AT-2685 | Value true only for Initial and follow-up tab selection 

                If (isReprintLetter = True) Then ' SB | 10/23/2016 | YRS-AT-2685 | For Initial tab do color coding row wise for Conditions to check 'IsLocked' and is Follow-up generated or not in Follow-up tab.
                    dra = dtRecords.Select("FundIdNo = '" + e.Row.Cells(2).Text + "'")
                    If dra.Length > 0 Then
                        For Each dr As DataRow In dra
                            If lbllnkFollowup.Visible = True Then
                                If Not dr("FollowupGenerated") Is Nothing Then
                                    If tdFollow.Visible And dr("FollowupGenerated").ToString.ToLower = "yes" And tdFollowUpLegends.Visible Then
                                        e.Row.CssClass = "BG_ColourFollowUpLetter"
                                    End If
                                End If
                            End If
                            If dr("ISLocked") Then
                                e.Row.CssClass = "BG_ColourIsLocked"
                            ElseIf Convert.ToDouble(dr("CurrentBalance").ToString()) <= 0 Or (Convert.ToDouble(dr("CurrentBalance").ToString()) < (Convert.ToDouble(dr("MRDAmount").ToString()) - Convert.ToDouble(dr("PaidAmount").ToString()))) Then
                                e.Row.CssClass = "BG_ColourIsBlocked"
                            End If
                        Next
                    End If
                End If

                'START : CS | 2016.10.24 | YRS-AT-3088 | Color code the Cash out eligible Participants
                If (e.Row.Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text = "1") Then
                    e.Row.CssClass = "BG_ColourSubjectToCashoutLetter"
                End If
                'END : CS | 2016.10.24 | YRS-AT-3088 | Color code the Cash out eligible Participants

                ' START : SB | 10/23/2016 | YRS-AT-2685 | Convert Due date field from datetime format to date only
                processDate = Convert.ToDateTime(e.Row.Cells(RMD_LETTER_DUEDATE).Text)
                e.Row.Cells(RMD_LETTER_DUEDATE).Text = processDate.ToString("MM/dd/yyyy")
                ' END : SB | 10/23/2016 | YRS-AT-2685 | Convert Due date field from datetime format to date only
                e.Row.Cells(RMD_LETTER_NAME).Text = System.Web.HttpUtility.HtmlDecode(e.Row.Cells(RMD_LETTER_NAME).Text)  ' SB | 11/22/2016 | YRS-AT-3203 | Displaying full name with special characters 
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_gvLetters_RowDataBound", ex)
        End Try
    End Sub

    Private Sub gvLetters_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvLetters.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try

            Dim SortExpression As String
            SortExpression = e.SortExpression
            If lbllnkInitial.Visible Then
                dtRecords = DirectCast(ViewState("RMDPrintLetters_dtInitialRecords"), DataTable)
            ElseIf lbllnkFollowup.Visible = True Then
                dtRecords = DirectCast(ViewState("RMDPrintLetters_dtFolloupRecords"), DataTable)
            End If
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If ViewState("RMDPrintLetters_Initial_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("RMDPrintLetters_Initial_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("RMDPrintLetters_Initial_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvLetters, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_gvLetters_Sorting", ex)
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
            ClearSessionVariables() 'MMR | 2017.05.16 | YRS-AT-3356 | Clear Session values before exiting from page
            Response.Redirect("MainWebForm.aspx", False) 'PPP | 2017.05.24 | YRS-AT-3356 | Passing 2nd parameter (endRequest = False) to avoid "Thread was being aborted." error.
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_btnClose_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Get Last Processing date of RMD
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetLastProccesedDate()
        Dim strLastProcessedDate As String = String.Empty
        Try
            strLastProcessedDate = YMCARET.YmcaBusinessObject.MRDBO.GetLastRMDProcessedDate()
            If String.IsNullOrEmpty(strLastProcessedDate) Then
                strLastProcessedDate = "12/31/2010"
            End If
            ViewState("ProcessDate") = strLastProcessedDate
            lblMRDMsg.Text = " Last RMD Closed Batch: " + strLastProcessedDate

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_btnSearch_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filter Data based on value select/provide from screen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SearchRecords()
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Dim Sort As GridViewCustomSort
        Dim strFilter As String = String.Empty
        Dim strMrdYear() As String = {"MRDYear"}
        Dim lstItem As ListItem
        Dim dtYear As DataTable
        Try

            If SelectedTab = EnumTabStrips.INITIAL Then   ' SB | 10/23/2016 | YRS-AT-2685 | Change the Logic of checking which tab is selected 
                ' If lbllnkInitial.Visible Then     ' Previous code commented
                dtRecords = DirectCast(ViewState("RMDPrintLetters_dtInitialRecords"), DataTable)
            ElseIf SelectedTab = EnumTabStrips.FOLLOWUP Then   ' SB | 10/23/2016 | YRS-AT-2685 | Change the Logic of checking which tab is selected 
                ' ElseIf lbllnkFollowup.Visible = True Then    ' Previous code commented
                dtRecords = DirectCast(ViewState("RMDPrintLetters_dtFolloupRecords"), DataTable)
            End If

            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                If ddlAcctLocked.SelectedValue <> "All" Then
                    strFilter = "ISLocked = " + ddlAcctLocked.SelectedValue
                End If
                If ddlInsufficientBal.SelectedValue <> "All" Then
                    If ddlInsufficientBal.SelectedValue = "Yes" Then
                        If strFilter = String.Empty Then
                            'START: MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 | Added condition to check current balance less than difference of mrd amt and paid amt
                            'strFilter = "CurrentBalance <= 0"
                            strFilter = "(CurrentBalance <= 0 OR CurrentBalance < (MRDAmount - PaidAmount))"
                            'END: MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 | Added condition to check current balance less than difference of mrd amt and paid amt
                        Else
                            'START: MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 | Added condition to check current balance less than difference of mrd amt and paid amt
                            'strFilter += " AND CurrentBalance <= 0"
                            strFilter += " AND (CurrentBalance <= 0 OR CurrentBalance < (MRDAmount - PaidAmount))"
                            'END: MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 | Added condition to check current balance less than difference of mrd amt and paid amt
                        End If
                    ElseIf ddlInsufficientBal.SelectedValue = "No" Then
                        If strFilter = String.Empty Then
                            strFilter = "CurrentBalance > 0"
                        Else
                            strFilter += " AND CurrentBalance > 0"
                        End If
                    End If
                End If

                If ddlRMDYear.Items.Count > 0 Then
                    If ddlRMDYear.SelectedValue <> "" Then
                        If strFilter = String.Empty Then
                            strFilter = " MRDYear = " + ddlRMDYear.SelectedValue
                        Else
                            strFilter += " And MRDYear = " + ddlRMDYear.SelectedValue
                        End If

                    End If
                End If

                If txtFundId.Text <> String.Empty Then
                    If strFilter = String.Empty Then
                        strFilter = "FundIdNo = '" + txtFundId.Text + "'"
                    Else
                        strFilter += " AND FundIdNo = '" + txtFundId.Text + "'"
                    End If
                End If
                If lbllnkFollowup.Visible = True Then
                    If ddlFollowUp.SelectedValue <> "All" Then
                        If strFilter = String.Empty Then
                            strFilter = "FollowupGenerated = '" + ddlFollowUp.SelectedValue + "'"
                        Else
                            strFilter += " AND FollowupGenerated = '" + ddlFollowUp.SelectedValue + "'"
                        End If
                    End If
                End If
                dv.RowFilter = strFilter

                'lblMRDMsg.Text = " Last RMD Closed Batch : " + dv.ToTable.Rows(0)("MRDExpireDate").ToString

                ViewState("RMDPrintLetters_Initial_Filter") = strFilter
                If ViewState("RMDPrintLetters_Initial_sort") IsNot Nothing Then
                    Sort = ViewState("RMDPrintLetters_Initial_sort")
                    dv.Sort = Sort.SortExpression + " " + Sort.SortDirection
                End If
            End If
            HelperFunctions.BindGrid(gvLetters, dv, True)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Print Initial / Followup Letters based on selection.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintLetters_Click(sender As Object, e As EventArgs) Handles btnPrintLetters.Click
        'START: PPP | 04/25/2017 | YRS-AT-3356 | Not saving selected participants in a list, directly saving into data table which will be used in a common print letter functions 
        Dim selectedParticipantCounter As Integer
        'Dim objLstRMDPrintLetters As List(Of YMCAObjects.RMDPrintLetters)
        'END: PPP | 04/25/2017 | YRS-AT-3356 | Not saving selected participants in a list, directly saving into data table which will be used in a common print letter functions 
        Try
            'START: MMR | 2017.05.16 | YRS-AT-3356 | Stop user by showing message if he has only read only access rights
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            End If
            'END: MMR | 2017.05.16 | YRS-AT-3356 | Stop user by showing message if he has only read only access rights

            'START: PPP | 04/25/2017 | YRS-AT-3356 | Not saving selected participants in a list, directly saving into data table which will be used in a common print letter functions 
            'objLstRMDPrintLetters = GetSelectedInitailRMDLetterRecords()
            ReadSelectedInitailRMDLetter()
            'START: MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 | Checking for selected records
            'selectedParticipantCounter = Me.SelectedParticipants.Tables(0).Rows.Count
            selectedParticipantCounter = 0
            If HelperFunctions.isNonEmpty(Me.SelectedParticipants) Then 'MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 |Checking data exists in table or not
                selectedParticipantCounter = Me.SelectedParticipants.Tables(0).Rows.Count
            End If
            'END: MMR | YRS-AT-3356 | 2017.05.10 | YRS-AT-3205 | Checking for selected records
            If selectedParticipantCounter > 0 Then
                'If objLstRMDPrintLetters.Count > 0 Then
                'END: PPP | 04/25/2017 | YRS-AT-3356 | Not saving selected participants in a list, directly saving into data table which will be used in a common print letter functions 

                'START: PPP | 04/21/2017 | YRS-AT-3356 | ConfirmDialog box moved to BatchProcessProgressControl
                '' START : SB | 2017.01.12 | YRS-AT-3298 | Message on confirm dialog box changed,  new message is generic message for all  types of letters ( RMD Initial / RMD Cashout / RMD Follow up / RMD Cashout Followup ) for records as multiple records exists for single person 
                '' lblMessage.Text = "Are you sure you want to print RMD Letter for selected " + objLstRMDPrintLetters.Count.ToString() + " person(s)"
                'lblMessage.Text = "Are you sure you want to print Letter for selected " + objLstRMDPrintLetters.Count.ToString() + " record(s)"
                '' END : SB | 2017.01.12 | YRS-AT-3298 | Message on confirm dialog box changed,  new message is generic message for all  types of letters ( RMD Initial / RMD Cashout / RMD Follow up / RMD Cashout Followup ) for records as multiple records exists for single person 
                'BatchProcessProgressControl.ConfirmationMessage = "Are you sure you want to print Letter for selected " + objLstRMDPrintLetters.Count.ToString() + " record(s)"
                'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showConfirmdialog();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Open", String.Format("{0}('Are you sure you want to print Letter for selected {1} record(s)');", BatchProcessProgressControl.OpenConfirmDialogMethodName, selectedParticipantCounter.ToString()), True)
                'END: PPP | 04/21/2017 | YRS-AT-3356 | ConfirmDialog box moved to BatchProcessProgressControl
            Else
                ' START : SB | 2017.01.12 | YRS-AT-3298 | Message is generic message for all Plan type and records
                'HelperFunctions.ShowMessageToUser("Please select the person(s) to print the letter", EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser("Please select the record(s) to print the letter", EnumMessageTypes.Error)
                ' END: SB | 2017.01.12 | YRS-AT-3298 | Message is generic message for all Plan type and records
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_btnPrintLetters_Click", ex)
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
            ''START: MMR | 2017.05.16 | YRS-AT-3356 | Stop user by showing message if he has only read only access rights
            'If Me.IsReadOnlyAccess Then
            'HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
            'Exit Sub
            ''End If
            ''END: MMR | 2017.05.16 | YRS-AT-3356 | Stop user by showing message if he has only read only access rights
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access. 
            'Session("gvSelectedRow") = gvLetters 'MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required
            GetSelectedRecords()
            Session("ReportName") = "RMDPrintLetterList"
            'Session("gvSelectedRow") = Nothing 'MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_btnPrintList_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Get Selected records from gridview
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Function GetSelectedRecords() As DataTable 'MMR | 2017.05.15 | YRS-AT-3356 | Changed method type as not required to return anything from method
    Private Sub GetSelectedRecords() 'MMR | 2017.05.15 | YRS-AT-3356 | Declared Method without return type
        Dim dtPrintLetters As New DataTable
        Dim drPrintLetters As DataRow
        'Dim gvSelectedRow As New GridView 'MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required

        dtPrintLetters.Columns.Add("FundId")
        dtPrintLetters.Columns.Add("PlanType")
        dtPrintLetters.Columns.Add("CurrentBalance")
        dtPrintLetters.Columns.Add("RMDAmount")
        dtPrintLetters.Columns.Add("PaidAmount")
        dtPrintLetters.Columns.Add("FundStatus")
        dtPrintLetters.Columns.Add("DueDate")
        dtPrintLetters.Columns.Add("Selected")
        dtPrintLetters.Columns.Add("ProcessDate")
        dtPrintLetters.Columns.Add("ReportName")

        'START: MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required
        'If Not Session("gvSelectedRow") Is Nothing Then
        '    gvSelectedRow = CType(Session("gvSelectedRow"), GridView)
        'End If
        'END: MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required

        'START: MMR | 2017.05.15 | YRS-AT-3356 | Using GridView name directly instead of storing it in declared object and then using it.
        'For iCount As Integer = 0 To gvSelectedRow.Rows.Count - 1 
        For iCount As Integer = 0 To gvLetters.Rows.Count - 1
            'END: MMR | 2017.05.15 | YRS-AT-3356 | Using GridView name directly instead of storing it in declared object and then using it.
            Dim chkBox As New CheckBox
            'START: MMR | 2017.05.15 | YRS-AT-3356 | Using GridView name directly instead of storing it in declared object and then using it.
            'chkBox = CType(gvSelectedRow.Rows(iCount).FindControl("chkSelect"), CheckBox)
            chkBox = CType(gvLetters.Rows(iCount).FindControl("chkSelect"), CheckBox)
            'END: MMR | 2017.05.15 | YRS-AT-3356 | Using GridView name directly instead of storing it in declared object and then using it.
            If Not chkBox Is Nothing Then
                drPrintLetters = dtPrintLetters.NewRow
                If chkBox.Checked Then
                    drPrintLetters("Selected") = "Yes"
                Else
                    drPrintLetters("Selected") = "No"
                End If
                drPrintLetters("FundId") = gvLetters.Rows(iCount).Cells(2).Text
                drPrintLetters("PlanType") = gvLetters.Rows(iCount).Cells(3).Text
                drPrintLetters("CurrentBalance") = gvLetters.Rows(iCount).Cells(5).Text
                drPrintLetters("RMDAmount") = gvLetters.Rows(iCount).Cells(6).Text
                drPrintLetters("PaidAmount") = gvLetters.Rows(iCount).Cells(7).Text
                drPrintLetters("FundStatus") = gvLetters.Rows(iCount).Cells(8).Text
                drPrintLetters("DueDate") = gvLetters.Rows(iCount).Cells(9).Text
                drPrintLetters("ProcessDate") = gvLetters.Rows(iCount).Cells(9).Text 'ViewState("ProcessDate").ToString()
                If lnkInitial.Visible Then
                    drPrintLetters("ReportName") = "RMD Follow Up Letters"
                ElseIf lnkFollowup.Visible Then
                    drPrintLetters("ReportName") = "RMD Initial Letters"
                End If
                dtPrintLetters.Rows.Add(drPrintLetters)
            End If
        Next
        Session("PrintLetters") = dtPrintLetters
        'START: MMR | 2017.05.15 | YRS-AT-3356 | Commneted as not required
        'Return dtPrintLetters
        'End Function
        'END: MMR | 2017.05.15 | YRS-AT-3356 | Commneted as not required
    End Sub

    ''' <summary>
    ''' Store PrintLetters records in session and reuse in webmethod
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPrintLetters() As DataTable
        Return CType(Session("PrintLetters"), DataTable)
    End Function

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
            'START : CS | 2016.10.24 | YRS-AT-3088 | Added new columns in the Initial /Cash out /Followup Letters
            l_datatable_PrintLetters.Columns.Add("PlanType")
            l_datatable_PrintLetters.Columns.Add("Name")
            l_datatable_PrintLetters.Columns.Add("CurrentBalance")
            l_datatable_PrintLetters.Columns.Add("MRDAmount")
            l_datatable_PrintLetters.Columns.Add("PaidAmount")
            l_datatable_PrintLetters.Columns.Add("StatusTypeDescription")
            l_datatable_PrintLetters.Columns.Add("MRDExpireDate")
            l_datatable_PrintLetters.Columns.Add("IsCashOutEligible")
            'END : CS | 2016.10.24 | YRS-AT-3088 | Added new columns in the Initial /Cash out /Followup Letters

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
                    'START : CS | 2016.10.24 | YRS-AT-3088 | Added new columns in the Initial /Cash out /Followup Letters
                    l_datarow_PrintLetters("PlanType") = objList(Icount).Plantype
                    l_datarow_PrintLetters("Name") = objList(Icount).Name
                    l_datarow_PrintLetters("CurrentBalance") = objList(Icount).CurrentBalance
                    l_datarow_PrintLetters("MRDAmount") = objList(Icount).RmdAmount
                    l_datarow_PrintLetters("PaidAmount") = objList(Icount).PaidAmount
                    l_datarow_PrintLetters("StatusTypeDescription") = objList(Icount).Fundstatus
                    l_datarow_PrintLetters("MRDExpireDate") = objList(Icount).Duedate
                    l_datarow_PrintLetters("IsCashOutEligible") = objList(Icount).IsCashOutEligible
                    'END : CS | 2016.10.24 | YRS-AT-3088 | Added new columns in the Initial /Cash out /Followup Letters
                    l_datatable_PrintLetters.Rows.Add(l_datarow_PrintLetters)
                Next
            End If

            Return l_datatable_PrintLetters
        Catch
            Throw
        End Try

    End Function

    'START: PPP | 05/02/2017 | YSR-AT-3356 | "Yes" button of confirm dialog functionality is moved to "UserControls/BatchProcessProgressUCWebMethods.aspx/CreateRMDBatch" function
    'Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
    '    Dim objLstRMDPrintLetters As List(Of YMCAObjects.RMDPrintLetters)
    '    Dim dtRecords As DataTable
    '    Dim dsPrintLetters As New DataSet
    '    Dim dv As New DataView
    '    Dim strBatchId As String = String.Empty
    '    Dim strModuleType As String = String.Empty
    '    Dim strMRDDueDate As String = GetSelectedMRDYearDueDate()  'SB | 10/23/2016 | YRS-AT-2685 | Getting the process date from the function

    '    'START : CS | 2016.10.24 | YRS-AT-3088 | Assigning Null values Session Property
    '    Me.InitialLetterFileName = Nothing
    '    Me.CashOutLetterFileName = Nothing
    '    Me.FollowLetterFileName = Nothing
    '    Me.CashOutFollowupLetterFileName = Nothing   'SB | 11/21/2016 | YRS-AT-3203 | Assigning Null value to cashout followup file path holder property
    '    'END : CS | 2016.10.24 | YRS-AT-3088 | Assigning Null values Session Property
    '    Try
    '        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Yes Button Click.")
    '        If Not Session("SelectedRMDPrintLetters") Is Nothing Then
    '            If lbllnkFollowup.Visible Then
    '                'START: SB | 10/23/2016 | YRS-AT-2685 | Values are now being stored in Viewstate instead of Session
    '                ' Session("Selecttab") = True       
    '                SelectedTab = EnumTabStrips.FOLLOWUP
    '                'END: SB | 10/23/2016 | YRS-AT-2685 | Values are now being stored in Viewstate instead of Session
    '            End If
    '            objLstRMDPrintLetters = CType(Session("SelectedRMDPrintLetters"), List(Of YMCAObjects.RMDPrintLetters))
    '            If SelectedTab = EnumTabStrips.FOLLOWUP Then      ' SB | 11/21/2016 | YRS-AT-3203 |  Checking if Follow up tab is selected
    '                ' If lbllnkFollowup.Visible = True Then       ' SB | 11/21/2016 | YRS-AT-3203 |  Instead of checking which label is visible, SelectedTab value is now checked 
    '                If HelperFunctions.isNonEmpty(objLstRMDPrintLetters) Then
    '                    Session("l_datatable_PrintInitialLetters") = Nothing
    '                    ' Session("l_datatable_PrintFollowUpLetters") = objLstRMDPrintLetters
    '                    dtRecords = ConvertListToDatable(objLstRMDPrintLetters)
    '                    dtRecords.TableName = "SelectedBatchRecords"
    '                    dsPrintLetters.Tables.Add(dtRecords)
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Get Next BatchId for FollowUp Letter.")
    '                    'START: SB | 10/23/2016 | YRS-AT-2685 | Change the Logic of getting the Next Batch Id 
    '                    'strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(ViewState("ProcessDate")).Tables(0).Rows(0)(0))
    '                    strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(strMRDDueDate).Tables(0).Rows(0)(0))
    '                    'END: SB | 10/23/2016 | YRS-AT-2685 | Change the Logic of getting the Next Batch Id  
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("btnYes_Click", "RMD Initial Print Letters Process Initiate for BatchID: " + strBatchId)
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Get Next BatchId for FollowUp Letter.")
    '                    strModuleType = CommonClass.BatchProcess.RMDFollwLetters.ToString
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: InsertAtsTemp for FollowUp Letter.")
    '                    YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModuleType, dsPrintLetters)
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: InsertAtsTemp for FollowUp Letter.")
    '                End If
    '            End If
    '            If SelectedTab = EnumTabStrips.INITIAL Then      ' SB | 11/21/2016 | YRS-AT-3203 |  Checking if Initial tab is selected
    '                'If lbllnkInitial.Visible = True Then        ' SB | 11/21/2016 | YRS-AT-3203 |  Instead of checking which label is visible, SelectedTab value is now checked  
    '                If HelperFunctions.isNonEmpty(objLstRMDPrintLetters) Then
    '                    Session("l_datatable_PrintFollowUpLetters") = Nothing
    '                    ' Session("l_datatable_PrintInitialLetters") = objLstRMDPrintLetters
    '                    dtRecords = ConvertListToDatable(objLstRMDPrintLetters)
    '                    dtRecords.TableName = "SelectedBatchRecords"
    '                    dsPrintLetters.Tables.Add(dtRecords)
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Get Next BatchId for Initial Letter.")
    '                    'START: SB | 10/23/2016 | YRS-AT-2685 | Change the Logic of getting the Next Batch Id 
    '                    'strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.MRDBO.GetNextBatchId(ViewState("ProcessDate")).Tables(0).Rows(0)(0))
    '                    strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(strMRDDueDate).Tables(0).Rows(0)(0))
    '                    'END: SB | 10/23/2016 | YRS-AT-2685 | Change the Logic of getting the Next Batch Id 
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("btnYes_Click", "RMD FollowUp Print Letters Process Initiate for BatchID: " + strBatchId)
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Get Next BatchId for Initial Letter.")
    '                    strModuleType = CommonClass.BatchProcess.RMDInitLetters.ToString
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: InsertAtsTemp for Initial Letter.")
    '                    YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModuleType, dsPrintLetters)
    '                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: InsertAtsTemp for Initial Letter.")
    '                End If
    '            End If
    '            'Start: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.

    '            'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CallPopup", "showPanel('modalRMD','" + strBatchId + "','" + strModuleType + "');", True)
    '            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Calling Ajax to Print Letters.")
    '            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "RMDPROCESS", "CallProcess('" + strBatchId + "','" + strModuleType + "');", True)
    '            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Calling Ajax to Print Letters.")
    '            'END: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.

    '        Else
    '            HelperFunctions.ShowMessageToUser("Selection records not found.", EnumMessageTypes.Error)
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("RMDPrintLetters_btnYes_Click", ex)
    '    Finally
    '        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Yes Button Click.")
    '        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
    '    End Try
    'End Sub
    'END: PPP | 05/02/2017 | YSR-AT-3356 | "Yes" button of confirm dialog functionality is moved to "UserControls/BatchProcessProgressUCWebMethods.aspx/CreateRMDBatch" function

    ''Start: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.
    '<System.Web.Services.WebMethod()> _
    'Public Shared Function RMDProcess(ByVal strBatchId As String, ByVal iCount As Integer, ByVal strProcessName As String, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
    '    Dim strretval As String = String.Empty
    '    Dim dsTemp As DataSet
    '    Dim objReturnStatusValues = New ReturnStatusValues()
    '    Dim objRMDPrintLetters As New RMDPrintLetters
    '    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "RMD PrintLetters Process Initiate for BatchID: " + strBatchId)
    '    Try
    '        dsTemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule)
    '        objReturnStatusValues = objRMDPrintLetters.Process(strBatchId, iCount, strProcessName, dsTemp, iIDXCreated, iPDFCreated, strModule)
    '        YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)
    '        If HelperFunctions.isNonEmpty(dsTemp.Tables("dtFileList")) Then
    '            HttpContext.Current.Session("FTFileList") = dsTemp.Tables("dtFileList")
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("Process Letter", ex)
    '        objReturnStatusValues.strretValue = "error"
    '        Return objReturnStatusValues
    '        Throw ex
    '    Finally
    '        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
    '    End Try
    '    Return objReturnStatusValues
    'End Function

    ' ''Start:'Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
    '' ''' <summary>
    '' ''' GetBatchSize will get the batch size for batch creation process.
    '' ''' </summary>
    '' ''' <returns></returns>
    '' ''' <remarks></remarks>
    ''Private Function GetBatchSize() As Integer
    ''    Dim dsbatchSize As DataSet
    ''    Dim intBatchsize As Integer
    ''    Try
    ''        dsbatchSize = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("BATCH_SIZE")
    ''        If HelperFunctions.isNonEmpty(dsbatchSize) Then
    ''            intBatchsize = dsbatchSize.Tables(0).Rows(0)("Value")
    ''        Else
    ''            intBatchsize = 2
    ''        End If
    ''        Return intBatchsize
    ''    Catch
    ''        Throw
    ''    End Try
    ''End Function
    ' ''End: Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
    'Private Function Process(ByVal strBatchId As String, ByVal iCount As Integer, ByVal strProcessName As String, dsTemp As DataSet, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
    '    Dim blnSuccess As Boolean = True
    '    Dim strProgressStatus As String = String.Empty
    '    Dim dtFileListSource As DataTable
    '    Dim strVal As String = String.Empty
    '    Dim i As Integer = 0
    '    Dim strReturn As String = String.Empty
    '    Dim dttemp As DataTable
    '    Dim objReturnStatusValues As New ReturnStatusValues
    '    Dim dtSelectedBatchRecords As DataTable
    '    Dim objBatchProcess As New BatchRequestCreation
    '    Dim ArrErrorDataList = New List(Of ExceptionLog)
    '    Dim dtFileList As DataTable
    '    Dim strBatchError As String
    '    Dim dt As New DataTable
    '    Dim BatchSize As Integer = GetBatchSize() ' 'Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
    '    Dim iProcessCount As Integer = 0
    '    'START : CS | 2016.10.24 | YRS-AT-3088 | Declared of Variables and object
    '    Dim pathAndFileName As String
    '    Dim reportFileName As String
    '    Dim tempFileList As DataTable
    '    Dim combinedPlanTypeRecord As DataTable
    '    Dim combinedDataList As DataTable
    '    'END : CS | 2016.10.24 | YRS-AT-3088 | Declared of Variables and object
    '    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: Process Method Calling.")
    '    Try
    '        objReturnStatusValues.strBatchId = strBatchId
    '        objReturnStatusValues.strReportType = strModule
    '        objReturnStatusValues.iProcessCount = iCount
    '        objReturnStatusValues.strretValue = "pending"
    '        objReturnStatusValues.iIdxCreated = iIDXCreated
    '        objReturnStatusValues.iPdfCreated = iPDFCreated

    '        dtSelectedBatchRecords = dsTemp.Tables("SelectedBatchRecords")

    '        Dim dtArrErrorDataList As DataTable = dsTemp.Tables("ArrErrorDataList")
    '        If Not dtArrErrorDataList Is Nothing Then
    '            For Each drArr As DataRow In dtArrErrorDataList.Rows
    '                ArrErrorDataList.Add(New ExceptionLog(drArr("FundNo"), drArr("Errors"), drArr("Description")))
    '            Next
    '        End If

    '        If Not dtSelectedBatchRecords.Columns.Contains("IsReportPrinted") Then
    '            dtSelectedBatchRecords.Columns.Add("IsReportPrinted")
    '        End If
    '        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: InsertPrintLetters Method Calling.")
    '        If Not dtSelectedBatchRecords Is Nothing Then
    '            For i = iCount To dtSelectedBatchRecords.Rows.Count - 1
    '                If i - iCount >= BatchSize Then
    '                    Exit For
    '                End If
    '                'START : SB | 2016.12.05 | YRS-AT-3203 | Added new parameter for saving plan type description in print letter ,previous code commented
    '                '   YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(dtSelectedBatchRecords.Rows(i)("FUNDNo").ToString, dtSelectedBatchRecords.Rows(i)("PersonId").ToString, dtSelectedBatchRecords.Rows(i)("LetterCode").ToString)
    '                YMCARET.YmcaBusinessObject.MRDBO.InsertPrintLetters(dtSelectedBatchRecords.Rows(i)("FUNDNo").ToString, dtSelectedBatchRecords.Rows(i)("PersonId").ToString, dtSelectedBatchRecords.Rows(i)("LetterCode").ToString, String.Format("PlanType={0}", dtSelectedBatchRecords.Rows(i)("PlanType").ToString))
    '                'END : SB | 2016.12.05 | YRS-AT-3203 | Added new parameter for saving plan type description in print letter ,previous code commented
    '            Next
    '        End If
    '        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: InsertPrintLetters Method Calling.")

    '        Dim dr As DataRow
    '        dt.Columns.Add("PersonId")
    '        dt.Columns.Add("RefRequestID")
    '        dt.Columns.Add("FUNDNo")
    '        dt.Columns.Add("SSNo")
    '        dt.Columns.Add("FirstName")
    '        dt.Columns.Add("LastName")
    '        dt.Columns.Add("MiddleName")
    '        dt.Columns.Add("LetterCode")

    '        If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
    '            For iProcessCount = iCount To dtSelectedBatchRecords.Rows.Count - 1
    '                If iProcessCount - iCount >= BatchSize Then
    '                    Exit For
    '                End If
    '                dr = dt.NewRow()
    '                dr("PersonId") = dtSelectedBatchRecords.Rows(iProcessCount)("PersonId")
    '                dr("RefRequestID") = dtSelectedBatchRecords.Rows(iProcessCount)("RefRequestID")
    '                dr("SSNo") = dtSelectedBatchRecords.Rows(iProcessCount)("SSNo")
    '                dr("FirstName") = dtSelectedBatchRecords.Rows(iProcessCount)("FirstName")
    '                dr("FUNDNo") = dtSelectedBatchRecords.Rows(iProcessCount)("FUNDNo")
    '                dr("LastName") = dtSelectedBatchRecords.Rows(iProcessCount)("LastName")
    '                dr("MiddleName") = dtSelectedBatchRecords.Rows(iProcessCount)("MiddleName")
    '                dr("LetterCode") = dtSelectedBatchRecords.Rows(iProcessCount)("LetterCode")
    '                dtSelectedBatchRecords.Rows(iProcessCount)("IsReportPrinted") = 1
    '                dt.Rows.Add(dr)
    '            Next
    '        End If
    '        'START : CS | 2016.10.24 | YRS-AT-3088 | Based on the Letter Coder the Initial/Followup/Cashout Letter will be created in the Location
    '        Dim columnNameToDistinct As String() = {"LetterCode"}
    '        Dim distinctRecord As DataTable = HelperFunctions.GetDistinctRecords(dt, columnNameToDistinct)
    '        For l As Integer = 0 To distinctRecord.Rows.Count - 1
    '            combinedPlanTypeRecord = GetFilterByLetterCode(dt, distinctRecord.Rows(l)("LetterCode").ToString())
    '            If HelperFunctions.isNonEmpty(combinedPlanTypeRecord) Then
    '                Dim l_stringDocType As String = String.Empty
    '                Dim letterCode As String = String.Empty
    '                Dim l_StringReportName As String = String.Empty
    '                Dim l_string_OutputFileType As String = String.Empty
    '                Dim strParam1 As String = String.Empty
    '                If strModule = CommonClass.BatchProcess.RMDInitLetters.ToString Then
    '                    strParam1 = "RMDPrintLetter"
    '                    If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDCSHLT.ToString() Then
    '                        l_stringDocType = CommonClass.RPTLetter.RMDCSHLT.ToString()
    '                        letterCode = CommonClass.RPTLetter.RMDCSHLT.ToString()
    '                        l_StringReportName = "RMD CashOut Letter.rpt"
    '                        l_string_OutputFileType = "RMD_CashOut_Letter_" + CommonClass.RPTLetter.RMDCSHLT.ToString()
    '                    Else
    '                        letterCode = CommonClass.RPTLetter.RMDINIT.ToString()
    '                        l_stringDocType = "RMDINTLT"
    '                        l_StringReportName = "RMD Initial Letter.rpt"
    '                        l_string_OutputFileType = "RMD_Initial_Letter_" + l_stringDocType
    '                    End If
    '                ElseIf strModule = CommonClass.BatchProcess.RMDFollwLetters.ToString Then
    '                    'START : SB | 2016.11.21 | YRS-AT-3203 | changes made for Followup cashout letters --Previous code is commented   
    '                    'l_stringDocType = "RMDFLWLT"
    '                    'letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString()
    '                    'l_StringReportName = "RMD Followup Letter.rpt"
    '                    'l_string_OutputFileType = "RMD_Followup_Letter_" + l_stringDocType
    '                    strParam1 = "RMDPrintLetter"
    '                    If distinctRecord.Rows(l)("LetterCode").ToString() = CommonClass.RPTLetter.RMDCFLLT.ToString() Then
    '                        l_stringDocType = CommonClass.RPTLetter.RMDCFLLT.ToString()
    '                        letterCode = CommonClass.RPTLetter.RMDCFLLT.ToString()
    '                        l_StringReportName = "RMD CashOut Followup Letter.rpt"
    '                        l_string_OutputFileType = "RMD_CashOut_Followup_Letter_" + CommonClass.RPTLetter.RMDCFLLT.ToString()
    '                    Else
    '                        l_stringDocType = "RMDFLWLT"
    '                        letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString()
    '                        l_StringReportName = "RMD Followup Letter.rpt"
    '                        l_string_OutputFileType = "RMD_Followup_Letter_" + l_stringDocType
    '                    End If
    '                    'END : SB | 2016.11.21 | YRS-AT-3203 | chnages made for Followup cashout letters --Previous code is commented
    '                End If
    '                Session("strReportName") = l_StringReportName.Substring(0, l_StringReportName.Length - 4)
    '                strBatchError = objBatchProcess.InvokeBatchRequestCreation(0, combinedPlanTypeRecord, l_stringDocType, l_StringReportName, l_string_OutputFileType, strParam1, ArrErrorDataList, tempFileList, iIDXCreated, iPDFCreated)
    '                UpdateFileList(tempFileList, dtFileList, letterCode, l_stringDocType)
    '                tempFileList = Nothing
    '                'Else
    '                '    Throw New Exception("Selected records not found.")
    '            End If
    '        Next
    '        'END : CS | 2016.10.24 | YRS-AT-3088 | Based on the Letter Coder the Initial/Followup/Cashout Letter will be created in the Location

    '        If dtSelectedBatchRecords.Rows.Count > objReturnStatusValues.iProcessCount + BatchSize Then
    '            If HelperFunctions.isNonEmpty(dtFileList) Then
    '                If Not dsTemp.Tables.Contains("dtFileList") Then
    '                    dtFileList.TableName = "dtFileList"
    '                    dsTemp.Tables.Add(dtFileList)
    '                End If
    '                dsTemp.Tables("dtFileList").Merge(dtFileList)
    '            End If
    '            objReturnStatusValues.iProcessCount += BatchSize
    '            Return objReturnStatusValues
    '        End If


    '        'Start: Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations
    '        If objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count Then
    '            objReturnStatusValues.strretValue = "success"
    '        Else
    '            objReturnStatusValues.strretValue = "pending"
    '        End If

    '        objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count

    '        'End: Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations

    '        Dim strFundNo As String = String.Empty

    '        If HelperFunctions.isNonEmpty(dsTemp) Then
    '            If HelperFunctions.isNonEmpty(dtFileList) Then
    '                If Not dsTemp.Tables.Contains("dtFileList") Then
    '                    dtFileList.TableName = "dtFileList"
    '                    dsTemp.Tables.Add(dtFileList)
    '                End If
    '                dsTemp.Tables("dtFileList").Merge(dtFileList)
    '                'START : CS | 2016.10.24 | YRS-AT-3088 | Logic is changed according to Letter code
    '                'If dsTemp.Tables("dtFileList").Rows.Count > 0 Then
    '                '    objBatchProcess.MergePDFs(dsTemp.Tables("dtFileList"))
    '                'End If
    '                'END : CS | 2016.10.24 | YRS-AT-3088 | Logic is changed according to Letter code

    '                'START : CS | 2016.10.24 | YRS-AT-3088 | Based on the Letter Coder the Initial/Followup/Cashout Letter PDF are mergering
    '                Session("MergedPdf_Filename") = Nothing
    '                RMDRePrintInitialLetterFileNameWithFullPath = String.Empty
    '                RMDRePrintCashoutLetterFileNameWithFullPath = String.Empty
    '                RMDRePrintFollowupLetterFileNameWithFullPath = String.Empty
    '                distinctRecord = HelperFunctions.GetDistinctRecords(dsTemp.Tables("SelectedBatchRecords"), columnNameToDistinct)
    '                For l As Integer = 0 To distinctRecord.Rows.Count - 1
    '                    combinedDataList = GetFilterByLetterCode(dsTemp.Tables("dtFileList"), distinctRecord.Rows(l)("LetterCode").ToString())
    '                    reportFileName = GetReportName(distinctRecord.Rows(l)("LetterCode").ToString())
    '                    If dsTemp.Tables("dtFileList").Rows.Count > 0 Then
    '                        objBatchProcess.MergePDFs(combinedDataList, reportFileName, strBatchId)
    '                    End If

    '                    SetFilePath(distinctRecord.Rows(l)("LetterCode").ToString(), Session("MergedPdf_Filename"))
    '                Next
    '                'END : CS | 2016.10.24 | YRS-AT-3088 | Based on the Letter Coder the Initial/Followup/Cashout Letter PDF are mergering

    '            End If
    '        End If

    '    Catch ex As Exception
    '        HelperFunctions.LogException("RMD Print Letters Process", ex)
    '        objReturnStatusValues.strretValue = "error"
    '        ArrErrorDataList.Add(New ExceptionLog("Exception", "Process Method", ex.Message))
    '        Return objReturnStatusValues
    '        Throw ex
    '    Finally
    '        Dim dtArrErrorDataList As New DataTable("ArrErrorDataList")
    '        dtArrErrorDataList.Columns.Add("FundNo")
    '        dtArrErrorDataList.Columns.Add("Errors")
    '        dtArrErrorDataList.Columns.Add("Description")
    '        Dim dr As DataRow
    '        For Each exlog As ExceptionLog In ArrErrorDataList
    '            dr = dtArrErrorDataList.NewRow()
    '            dr("FundNo") = exlog.FundNo
    '            dr("Errors") = exlog.Errors
    '            dr("Description") = exlog.Decription
    '            dtArrErrorDataList.Rows.Add(dr)
    '        Next
    '        If dsTemp.Tables.Contains("ArrErrorDataList") Then
    '            dsTemp.Tables.Remove("ArrErrorDataList")
    '        End If
    '        dtArrErrorDataList.TableName = "ArrErrorDataList"
    '        dsTemp.Tables.Add(dtArrErrorDataList)

    '        If HelperFunctions.isNonEmpty(dtSelectedBatchRecords) Then
    '            Dim dv As New DataView(dtSelectedBatchRecords)
    '            dv.RowFilter = "IsReportPrinted = 1"

    '            objReturnStatusValues.iTotalCount = dtSelectedBatchRecords.Rows.Count
    '            objReturnStatusValues.iIdxCreated = iIDXCreated
    '            objReturnStatusValues.iPdfCreated = iPDFCreated
    '            objReturnStatusValues.iTotalIDXPDFCount = dv.Count
    '            'START : CS | 2016.10.24 | YRS-AT-3088 | Assigning the File path to the Letter
    '            objReturnStatusValues.cashOutLetterPath = Me.CashOutLetterFileName
    '            objReturnStatusValues.initialLetterPath = Me.InitialLetterFileName
    '            objReturnStatusValues.followupLetterPath = Me.FollowLetterFileName
    '            objReturnStatusValues.cashOutFollowupLetterPath = Me.CashOutFollowupLetterFileName    ' SB | 2016.11.21 | YRS-AT-3203 | For cashout followup assign the file path
    '            'END : CS | 2016.10.24 | YRS-AT-3088 | Assigning the File path to the Letter
    '        End If
    '        YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)
    '        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Process Method Calling.")
    '    End Try
    '    Return objReturnStatusValues
    'End Function
    ''End: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.

    'START: PPP | 05/02/2017 | YSR-AT-3356 | Replaced "GetSelectedInitailRMDLetterRecords" with "ReadSelectedInitailRMDLetter" which directly transfer selected participant as DataSet to session instead of list of objects
    'Private Function GetSelectedInitailRMDLetterRecords() As List(Of YMCAObjects.RMDPrintLetters)

    '    Dim objLstRMDPrintLetters As New List(Of YMCAObjects.RMDPrintLetters)
    '    Dim chkSelectedRMD As New CheckBox
    '    Try
    '        Dim i As Integer = 0
    '        If HelperFunctions.isNonEmpty(gvLetters) Then

    '        End If
    '        For i = 0 To gvLetters.Rows.Count - 1
    '            chkSelectedRMD = gvLetters.Rows(i).FindControl("chkSelect")
    '            If Not IsNothing(chkSelectedRMD) Then
    '                If chkSelectedRMD.Checked = True Then
    '                    Dim objRMDPrintLetters As New YMCAObjects.RMDPrintLetters()
    '                    If SelectedTab = EnumTabStrips.INITIAL Then      ' SB | 11/21/2016 | YRS-AT-3203 |  Checking if Initial tab is selected
    '                        'If lbllnkInitial.Visible = True Then        ' SB | 11/21/2016 | YRS-AT-3203 |  Instead of checking which label is visible, SelectedTab value is now checked  
    '                        'START : CS | 2016.10.24 | YRS-AT-3088 | Add the Cash out Eligible Participants
    '                        If Not gvLetters.Rows(i).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString() Is Nothing Then
    '                            If Convert.ToInt32(gvLetters.Rows(i).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString()) = 1 Then
    '                                objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDCSHLT.ToString()
    '                            Else
    '                                objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDINIT.ToString()
    '                            End If
    '                        Else
    '                            objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDINIT.ToString()
    '                        End If
    '                        'END : CS | 2016.10.24 | YRS-AT-3088 | Add the Cash out Eligible Participants

    '                    ElseIf SelectedTab = EnumTabStrips.FOLLOWUP Then  ' SB | 11/21/2016 | YRS-AT-2685 |  Checking if Follow-up tab is selected
    '                        'ElseIf lbllnkFollowup.Visible = True Then    ' SB | 11/21/2016 | YRS-AT-2685 |  Instead of checking which label is visible, SelectedTab value is now checked   
    '                        'START : SB | 2016.11.21 | YRS-AT-3203 | Add the Cash out Eligible Participants for Follow-up
    '                        ' objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString()
    '                        If Not gvLetters.Rows(i).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString() Is Nothing Then
    '                            If Convert.ToInt32(gvLetters.Rows(i).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString()) = 1 Then
    '                                objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDCFLLT.ToString()
    '                            Else
    '                                objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString()
    '                            End If
    '                        Else
    '                            objRMDPrintLetters.strLetterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString()
    '                        End If
    '                        'END : SB | 2016.11.21 | YRS-AT-3203 | Add the Cash out Eligible Participants for Follow-up
    '                    End If
    '                    objRMDPrintLetters.strPersID = gvLetters.Rows(i).Cells(RMD_LETTER_PERSID).Text.ToString()
    '                    objRMDPrintLetters.strRefId = ""
    '                    'START : CS | 2016.10.24 | YRS-AT-3088 | Changing index values to Constant values
    '                    objRMDPrintLetters.strFundNo = gvLetters.Rows(i).Cells(RMD_LETTER_FUNDNO).Text.ToString()
    '                    objRMDPrintLetters.strSSNo = gvLetters.Rows(i).Cells(RMD_LETTER_SSNO).Text.ToString()
    '                    objRMDPrintLetters.strFirstName = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_FIRSTNAME).Text.ToString())   ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
    '                    objRMDPrintLetters.strLastName = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_LASTNAME).Text.ToString())     ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
    '                    objRMDPrintLetters.strMiddleName = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_MIDDLENAME).Text.ToString()) ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
    '                    'END : CS | 2016.10.24 | YRS-AT-3088 | Changing index values to Constant values

    '                    'START : CS | 2016.10.24 | YRS-AT-3088 | New columns are added
    '                    objRMDPrintLetters.Plantype = gvLetters.Rows(i).Cells(RMD_LETTER_PLANTYPE).Text.ToString()
    '                    objRMDPrintLetters.RmdAmount = gvLetters.Rows(i).Cells(RMD_LETTER_RMDAMOUNT).Text.ToString()
    '                    objRMDPrintLetters.PaidAmount = gvLetters.Rows(i).Cells(RMD_LETTER_PAIDAMOUNT).Text.ToString()
    '                    objRMDPrintLetters.Name = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(i).Cells(RMD_LETTER_NAME).Text.ToString())                ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
    '                    objRMDPrintLetters.Fundstatus = gvLetters.Rows(i).Cells(RMD_LETTER_FUNDSTATUS).Text.ToString()
    '                    objRMDPrintLetters.Duedate = gvLetters.Rows(i).Cells(RMD_LETTER_DUEDATE).Text.ToString()
    '                    objRMDPrintLetters.CurrentBalance = gvLetters.Rows(i).Cells(RMD_LETTER_CURRENTBALANCE).Text.ToString()
    '                    objRMDPrintLetters.IsCashOutEligible = Convert.ToInt32(gvLetters.Rows(i).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text)
    '                    'END : CS | 2016.10.24 | YRS-AT-3088 | New columns are added
    '                    objLstRMDPrintLetters.Add(objRMDPrintLetters)
    '                End If
    '            End If
    '        Next
    '        Session("SelectedRMDPrintLetters") = objLstRMDPrintLetters
    '        Return objLstRMDPrintLetters
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Private Sub ReadSelectedInitailRMDLetter()
        Dim printLettersDataSet As DataSet
        Dim printLettersData As DataTable
        Dim printLettersRow As DataRow
        Dim checkboxControl As CheckBox
        Dim counter As Integer
        Dim batchID As String
        Try
            If HelperFunctions.isNonEmpty(gvLetters) Then
                printLettersData = New DataTable()
                printLettersData.Columns.Add("PersonId")
                printLettersData.Columns.Add("RefRequestID")
                printLettersData.Columns.Add("FUNDNo")
                printLettersData.Columns.Add("SSNo")
                printLettersData.Columns.Add("FirstName")
                printLettersData.Columns.Add("LastName")
                printLettersData.Columns.Add("MiddleName")
                printLettersData.Columns.Add("LetterCode")
                printLettersData.Columns.Add("PlanType")
                printLettersData.Columns.Add("Name")
                printLettersData.Columns.Add("CurrentBalance")
                printLettersData.Columns.Add("MRDAmount")
                printLettersData.Columns.Add("PaidAmount")
                printLettersData.Columns.Add("StatusTypeDescription")
                printLettersData.Columns.Add("MRDExpireDate")
                printLettersData.Columns.Add("IsCashOutEligible")

                'Setting required sessions
                '-- Setting module name which is required for printing
                If SelectedTab = EnumTabStrips.INITIAL Then
                    BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDInitLetters.ToString()
                ElseIf SelectedTab = EnumTabStrips.FOLLOWUP Then
                    BatchProcessProgressControl.ModuleName = CommonClass.BatchProcess.RMDFollwLetters.ToString()
                End If

                '-- Setting batch id
                BatchProcessProgressControl.BatchID = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(GetSelectedMRDYearDueDate()).Tables(0).Rows(0)(0))

                For counter = 0 To gvLetters.Rows.Count - 1
                    checkboxControl = gvLetters.Rows(counter).FindControl("chkSelect")
                    If Not IsNothing(checkboxControl) Then
                        If checkboxControl.Checked = True Then
                            printLettersRow = printLettersData.NewRow()
                            If SelectedTab = EnumTabStrips.INITIAL Then
                                If Not gvLetters.Rows(counter).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString() Is Nothing Then
                                    If Convert.ToInt32(gvLetters.Rows(counter).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString()) = 1 Then
                                        printLettersRow("LetterCode") = CommonClass.RPTLetter.RMDCSHLT.ToString()
                                    Else
                                        printLettersRow("LetterCode") = CommonClass.RPTLetter.RMDINIT.ToString()
                                    End If
                                Else
                                    printLettersRow("LetterCode") = CommonClass.RPTLetter.RMDINIT.ToString()
                                End If
                            ElseIf SelectedTab = EnumTabStrips.FOLLOWUP Then
                                If Not gvLetters.Rows(counter).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString() Is Nothing Then
                                    If Convert.ToInt32(gvLetters.Rows(counter).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text.ToString()) = 1 Then
                                        printLettersRow("LetterCode") = CommonClass.RPTLetter.RMDCFLLT.ToString()
                                    Else
                                        printLettersRow("LetterCode") = CommonClass.RPTLetter.RMDFOLLOW.ToString()
                                    End If
                                Else
                                    printLettersRow("LetterCode") = CommonClass.RPTLetter.RMDFOLLOW.ToString()
                                End If
                            End If
                            printLettersRow("PersonId") = gvLetters.Rows(counter).Cells(RMD_LETTER_PERSID).Text.ToString()
                            printLettersRow("RefRequestID") = ""
                            printLettersRow("FUNDNo") = gvLetters.Rows(counter).Cells(RMD_LETTER_FUNDNO).Text.ToString()
                            printLettersRow("SSNo") = gvLetters.Rows(counter).Cells(RMD_LETTER_SSNO).Text.ToString()
                            printLettersRow("FirstName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_FIRSTNAME).Text.ToString())   ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
                            printLettersRow("LastName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_LASTNAME).Text.ToString())     ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
                            printLettersRow("MiddleName") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_MIDDLENAME).Text.ToString()) ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
                            printLettersRow("PlanType") = gvLetters.Rows(counter).Cells(RMD_LETTER_PLANTYPE).Text.ToString()
                            printLettersRow("MRDAmount") = gvLetters.Rows(counter).Cells(RMD_LETTER_RMDAMOUNT).Text.ToString()
                            printLettersRow("PaidAmount") = gvLetters.Rows(counter).Cells(RMD_LETTER_PAIDAMOUNT).Text.ToString()
                            printLettersRow("Name") = System.Net.WebUtility.HtmlDecode(gvLetters.Rows(counter).Cells(RMD_LETTER_NAME).Text.ToString())                ' SB | 2016.11.21 | YRS-AT-3203 | allowing special characters in name field 
                            printLettersRow("StatusTypeDescription") = gvLetters.Rows(counter).Cells(RMD_LETTER_FUNDSTATUS).Text.ToString()
                            printLettersRow("MRDExpireDate") = gvLetters.Rows(counter).Cells(RMD_LETTER_DUEDATE).Text.ToString()
                            printLettersRow("CurrentBalance") = gvLetters.Rows(counter).Cells(RMD_LETTER_CURRENTBALANCE).Text.ToString()
                            printLettersRow("IsCashOutEligible") = Convert.ToInt32(gvLetters.Rows(counter).Cells(RMD_LETTER_ISCASHOUTELIGIBLE).Text)
                            printLettersData.Rows.Add(printLettersRow)
                        End If
                    End If
                Next

                printLettersData.TableName = "SelectedBatchRecords"
                printLettersDataSet = New DataSet()
                printLettersDataSet.Tables.Add(printLettersData)
                Me.SelectedParticipants = printLettersDataSet
            End If
        Catch ex As Exception
            Throw ex
        Finally
            printLettersDataSet = Nothing
            printLettersData = Nothing
            printLettersRow = Nothing
            checkboxControl = Nothing
        End Try
    End Sub
    'END: PPP | 05/02/2017 | YSR-AT-3356 | Replaced "GetSelectedInitailRMDLetterRecords" with "ReadSelectedInitailRMDLetter" which directly transfer selected participant as DataSet to session instead of list of objects

    ''' <summary>
    ''' Open Report Viewwer to view data
    ''' </summary>
    ''' <param name="objLstRmdInitialLetters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function OpenReportViewer(ByVal objLstRmdInitialLetters As List(Of YMCAObjects.RMDPrintLetters))
        Try
            If lbllnkInitial.Visible Then
                Session("strReportName") = "RMD Initial Letter"
            ElseIf lbllnkFollowup.Visible = True Then
                Session("strReportName") = "RMD Followup Letter"
            End If
            For iCount As Integer = 0 To objLstRmdInitialLetters.Count - 1
                Session("RMDBatchFundNo") = GetPipeSeperated(objLstRmdInitialLetters)
                Exit For
            Next
            Dim popupScript As String = " newwindow = window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " +
                                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');" +
                                        " if (window.focus) {newwindow.focus()}" +
                                        " if (!newwindow.closed) {newwindow.focus()}"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", popupScript, True)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Get Pipe Seperated Data
    ''' </summary>
    ''' <param name="lstGuids"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    Private Sub lnkInitial_Click(sender As Object, e As EventArgs) Handles lnkInitial.Click
        Try
            ' START : SB | 10/23/2016 | YRS-AT-2685 |  Previous Code is commented as New common funtion is created to achieve given  functionality
            'Session("Selecttab") = Nothing
            'tdFollow.Visible = False
            'tdFollowUpLegends.Visible = False
            'ddlFollowUp.Visible = False
            'txtFollowup.Visible = False
            'tdCashoutLegends.Visible = True 'CS | 2016.10.24 | YRS-AT-3088 | To display the Cash out Legends
            'lnkInitial.Visible = False
            'lbllnkInitial.Visible = True
            'lnkFollowup.Visible = True
            'lbllnkFollowup.Visible = False
            'btnPrintLetters.Text = "Print Initial Letter"
            'lblDescription.Text = "List of person(s) eligible for generating initial letters."
            'ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            'ViewState("RMDPrintLetters_Initial_sort") = Nothing
            'ddlAcctLocked.SelectedValue = "All"
            'ddlFollowUp.SelectedValue = "All"
            'ddlInsufficientBal.SelectedValue = "No"

            'GetInititialRecords()

            'SearchRecords()

            ''dtRecords = DirectCast(ViewState("RMDPrintLetters_dtInitialRecords"), DataTable)
            ''ElseIf lbllnkFollowup.Visible = True Then
            ''dtRecords = DirectCast(ViewState("RMDPrintLetters_dtFolloupRecords"), DataTable)
            ' END : SB | 10/23/2016 | YRS-AT-2685 |  Previous Code is commented as New common funtion is created to achieve given  functionality

            ' SB | 2016.12.05 | YRS-AT-3203 | Below line commented as this functionality is handled in ShowHideReprintLetterTabDetails() function
            ' tdCashoutLegends.Visible = True 'CS | 2016.10.24 | YRS-AT-3088 | To display the Cash out Legends

            SelectedTab = EnumTabStrips.INITIAL 'SB | 10/23/2016 | YRS-AT-2685 |  Selected Tab is stored in Viewstate 

            ShowHideInitialLetterTabDetails(True)
            ShowHideFollowupLetterTabDetails(False)
            ShowHideReprintLetterTabDetails(False)

            ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            ViewState("RMDPrintLetters_Initial_sort") = Nothing

            GetInitialRecords() 'SB | 10/13/2016 | YRS-AT-2685 |  Renamed old Fuction name from 'GetInititialRecords' to 'GetInitialRecords'
            SearchRecords()

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_lnkInitial_Click", ex)
        End Try
    End Sub

    Private Sub lnkFollowup_Click(sender As Object, e As EventArgs) Handles lnkFollowup.Click
        Try
            ' START : SB | 10/23/2016 | YRS-AT-2685 |  Previous Code is commented as New common funtion is created to achieve given  functionality
            'Session("Selecttab") = True
            'tdFollow.Visible = True
            'tdFollowUpLegends.Visible = True
            'tdCashoutLegends.Visible = False 'CS | 2016.10.24 | YRS-AT-3088 | To disappear the Cash out Legends
            'ddlFollowUp.Visible = True
            'txtFollowup.Visible = True
            'lnkInitial.Visible = True
            'lbllnkInitial.Visible = False
            'lnkFollowup.Visible = False
            'lbllnkFollowup.Visible = True
            'btnPrintLetters.Text = "Print Follow-up Letter"
            'lblDescription.Text = "List of person(s) eligible for generating follow-up letters."
            'GetFollowupRecords()
            'ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            'ViewState("RMDPrintLetters_Initial_sort") = Nothing
            'ddlAcctLocked.SelectedValue = "All"
            'ddlFollowUp.SelectedValue = "All"
            'ddlInsufficientBal.SelectedValue = "No"
            'SearchRecords()
            ' END : SB | 10/23/2016 | YRS-AT-2685 |  Previous Code is commented as New common funtion is created to achieve given  functionality

            ' SB | 2016.12.05 | YRS-AT-3203 | Below line commented as this functionality is handled in ShowHideReprintLetterTabDetails() function
            'tdCashoutLegends.Visible = False 'CS | 2016.10.24 | YRS-AT-3088 | To disappear the Cash out Legends

            SelectedTab = EnumTabStrips.FOLLOWUP
            ShowHideInitialLetterTabDetails(False)
            ShowHideFollowupLetterTabDetails(True)
            ShowHideReprintLetterTabDetails(False)
            GetFollowupRecords()
            ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            ViewState("RMDPrintLetters_Initial_sort") = Nothing
            SearchRecords()

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_lnkInitial_Click", ex)
        End Try
    End Sub

    'START: PPP | 04/21/2017 | YRS-AT-3356 | btnClosePop was part of progress window which is moved to BatchProcessProgressControl, so handling BatchProcessProgressControl event provided for Close button click
    'Private Sub btnClosePop_Click(sender As Object, e As EventArgs) Handles btnClosePop.Click
    Private Sub BatchProcessProgressControl_HandlePUCCloseButtonClick(sender As Object, e As EventArgs) Handles BatchProcessProgressControl.HandlePUCCloseButtonClick
        'END: PPP | 04/21/2017 | YRS-AT-3356 | btnClosePop was part of progress window which is moved to BatchProcessProgressControl, so handling BatchProcessProgressControl event provided for Close button click
        'START: PPP | 04/17/2017 | YRS-AT-3237 | When popup will get closed we need to refresh the ViewState which holds data and call SearchRecords() method which will refresh grid
        ''Session("RMDClose") = True           ' SB | 10/23/2016 | YRS-AT-2685 | Value stored in Property instead of storing directly in session
        'RMDLettersProgressPopupClosed = True  ' SB | 10/23/2016 | YRS-AT-2685 | Value stored in session variable through Property
        'Response.Redirect("RMDPrintLetters.aspx", True) ' CS | 2016.10.24 | YRS-AT-3088 | Opening has beed change into UI.
        Dim dsFolloupRecords, dsInitialRecords As DataSet
        Try
            If SelectedTab = EnumTabStrips.FOLLOWUP Then
                dsFolloupRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchRMDForFollowupLetter()

                ViewState("RMDPrintLetters_dtFolloupRecords") = Nothing
                If dsFolloupRecords.Tables.Count > 0 Then
                    ViewState("RMDPrintLetters_dtFolloupRecords") = dsFolloupRecords.Tables(0)
                End If
            ElseIf SelectedTab = EnumTabStrips.INITIAL Then
                dsInitialRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchRMDForInitialLetter()

                ViewState("RMDPrintLetters_dtInitialRecords") = Nothing
                If dsInitialRecords.Tables.Count > 0 Then
                    ViewState("RMDPrintLetters_dtInitialRecords") = dsInitialRecords.Tables(0)
                End If
            End If

            SearchRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_btnClosePop_Click", ex)
        Finally
            dsFolloupRecords = Nothing
            dsInitialRecords = Nothing
        End Try
        'END: PPP | 04/17/2017 | YRS-AT-3237 | When popup will get closed we need to refresh the ViewState which holds data and call SearchRecords() method which will refresh grid
    End Sub

    'START: PPP | 04/21/2017 | YSR-AT-3356 | Moved following functions to CommonClasses\RMDLetters.cs class
    ''START : CS | 2016.10.24 | YRS-AT-3088 | Updated the Letter code in the File list data table
    'Private Sub UpdateFileList(ByVal fileListCurrent As DataTable, ByRef dtFileList As DataTable, ByVal letterCode As String, ByVal reportDocCode As String)
    '    Dim Rows As DataRow
    '    Dim fileRecordRows As DataRow()
    '    Dim fileRecordRowsExists As DataRow()
    '    If HelperFunctions.isNonEmpty(fileListCurrent) Then
    '        If dtFileList Is Nothing Then
    '            dtFileList = fileListCurrent.Clone()
    '        End If

    '        If Not dtFileList.Columns.Contains("Lettercode") Then
    '            dtFileList.Columns.Add("Lettercode", System.Type.GetType("System.String"))
    '        End If
    '        'For Each drRecp As DataRow In fileListCurrent.Rows

    '        fileRecordRowsExists = fileListCurrent.Select(String.Format("SourceFile LIKE '%{0}%'", reportDocCode))
    '        For Each drRecps1 As DataRow In fileRecordRowsExists
    '            Rows = dtFileList.NewRow
    '            Rows("FundNo") = drRecps1("FundNo").ToString()
    '            Rows("DestFolder") = drRecps1("DestFolder")
    '            Rows("DestFile") = drRecps1("DestFile")
    '            Rows("SourceFile") = drRecps1("SourceFile")
    '            Rows("SourceFolder") = drRecps1("SourceFolder")
    '            Rows("Lettercode") = letterCode
    '            dtFileList.Rows.Add(Rows)
    '        Next
    '        dtFileList.AcceptChanges()
    '    End If
    'End Sub
    ''END : CS | 2016.10.24 | YRS-AT-3088 | Updated the Letter code in the File list data table

    ''START : CS | 2016.10.24 | YRS-AT-3088 | Filter the record based on the Letter code 
    'Private Function GetFilterByLetterCode(ByVal fileRecords As DataTable, ByVal letterCode As String) As DataTable
    '    Dim filelist As DataRow()
    '    Dim filelistrows As DataRow
    '    Dim combinedPlanTypeRecord As DataTable
    '    filelist = fileRecords.Select(String.Format("LetterCode='{0}'", letterCode))
    '    combinedPlanTypeRecord = fileRecords.Clone()
    '    For Each drRecp As DataRow In filelist
    '        filelistrows = combinedPlanTypeRecord.NewRow
    '        filelistrows.ItemArray = drRecp.ItemArray
    '        combinedPlanTypeRecord.Rows.Add(filelistrows)
    '    Next
    '    Return combinedPlanTypeRecord
    'End Function
    ''END : CS | 2016.10.24 | YRS-AT-3088 | Filter the record based on the Letter code 

    ''START : CS | 2016.10.24 | YRS-AT-3088 | Return the Report File Name based on letter code
    'Private Function GetReportName(ByVal letterCode As String) As String
    '    Dim reportfileName As String
    '    If letterCode = CommonClass.RPTLetter.RMDCSHLT.ToString() Then
    '        reportfileName = "RMD_Cash_Out_Letter"
    '    ElseIf letterCode = CommonClass.RPTLetter.RMDINIT.ToString() Then
    '        reportfileName = "RMD_Initial_Letter"
    '    ElseIf letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString() Then
    '        reportfileName = "RMD_Followup_Letter"
    '        'START : SB | 2016.11.21 | YRS-AT-3203 | Setting cashout follow up report file name
    '    ElseIf letterCode = CommonClass.RPTLetter.RMDCFLLT.ToString() Then
    '        reportfileName = "RMD_CashOut_Followup_Letter"
    '        'END   : SB | 2016.11.21 | YRS-AT-3203 | Setting cashout follow up report file name
    '    End If
    '    Return reportfileName
    'End Function
    ''END : CS | 2016.10.24 | YRS-AT-3088 | Return the Report File Name based on letter code

    ''START : CS | 2016.10.24 | YRS-AT-3088 | Return the Report File path Name based on letter code
    'Private Sub SetFilePath(ByVal letterCode As String, ByVal mergedPdfFileName As String)
    '    Dim pathAndFileName As String = String.Format("{0}\{1}", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), mergedPdfFileName)
    '    Dim relativePath As String = String.Format("{0}\\{1}", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), mergedPdfFileName)
    '    If FileIO.FileSystem.FileExists(pathAndFileName) Then
    '        If letterCode = CommonClass.RPTLetter.RMDCSHLT.ToString() Then
    '            Me.CashOutLetterFileName = "DocumentViewer.aspx?ReportType=2"
    '            RMDRePrintCashoutLetterFileNameWithFullPath = relativePath
    '        ElseIf letterCode = CommonClass.RPTLetter.RMDINIT.ToString() Then
    '            Me.InitialLetterFileName = "DocumentViewer.aspx?ReportType=1"
    '            RMDRePrintInitialLetterFileNameWithFullPath = relativePath
    '        ElseIf letterCode = CommonClass.RPTLetter.RMDFOLLOW.ToString() Then
    '            Me.FollowLetterFileName = "DocumentViewer.aspx?ReportType=3"
    '            RMDRePrintFollowupLetterFileNameWithFullPath = relativePath
    '            'START : SB | 2016.11.21 | YRS-AT-3203 | Creating cashout followup file printing link and setting file path
    '        ElseIf letterCode = CommonClass.RPTLetter.RMDCFLLT.ToString() Then
    '            Me.CashOutFollowupLetterFileName = "DocumentViewer.aspx?ReportType=4"
    '            RMDRePrintCashOutFollowupLetterFileNameWithFullPath = relativePath
    '            'END : SB | 2016.11.21 | YRS-AT-3203 | Creating cashout followup file printing link and setting file path
    '        End If
    '    End If
    'End Sub
    'END: PPP | 04/21/2017 | YSR-AT-3356 | Moved following functions to CommonClasses\RMDLetters.cs class

    ' START : SB | 10/13/2016 | YRS-AT-2685 |  Changes to hide Reprint letter filters , tab selected functions, function to BatchId(s) with details, property defined for variables,defining ENUM, and dipaly link dynamically for initial letter/Cashout letter 
    Private Sub lnkReprintLetter_Click(sender As Object, e As EventArgs) Handles lnkReprintLetter.Click
        Try
            SelectedTab = EnumTabStrips.REPRINT

            ShowHideInitialLetterTabDetails(False)
            ShowHideFollowupLetterTabDetails(False)
            ShowHideReprintLetterTabDetails(True)

            GetRePrintLetterRecords()

            ViewState("RMDPrintLetters_Initial_Filter") = Nothing
            ViewState("RMDPrintLetters_Initial_sort") = Nothing

            FillRMDReprintRecords()

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_lnkReprintLetter_Click", ex)
        End Try
    End Sub

    'To Display filter fields and messages for Reprint RMD Letter Tab 
    Private Sub ShowHideReprintLetterTabDetails(ByVal IsVisible As Boolean)

        trColorCodingLegendsForReprintLetters.Visible = IsVisible
        lbllnkReprintLetter.Visible = IsVisible
        trRMDBatchWiseDataControl.Visible = IsVisible
        ddlRMDYear.Enabled = True
        rptRMDBatchIDs.Visible = IsVisible
        trColorCodingLegends.Visible = Not IsVisible   'Making color code legends visible false
        divBatchwiseInformation.Visible = IsVisible
        trFilterInsufficientBal.Visible = Not IsVisible
        trFilterInsufficientBalDDL.Visible = Not IsVisible
        trFilterAccountLocked.Visible = Not IsVisible
        trFilterAccountDDL.Visible = Not IsVisible
        trFilterFollowupGenerated.Visible = Not IsVisible
        trFilterFollowupGeneratedDDL.Visible = Not IsVisible
        Table3.Visible = Not IsVisible
        lnkReprintLetter.Visible = Not IsVisible
        divToShowReprintFollowupLink.Visible = False
        divToShowReprintLinks.Visible = False
        If (IsVisible) Then
            btnPrintLetters.Text = "Re-Print Letter"
            lblDescription.Text = "Re-print Initial/Follow-up Letter(s)"
        End If
        btnPrintList.Visible = Not IsVisible
        btnPrintLetters.Visible = Not IsVisible
        tdCashoutLegends.Visible = Not IsVisible 'SB | 2016.12.06 | YRS-AT-3203 | Not to display the Cash out Legends in re-print letter tab
    End Sub

    'To Display filter fields and messages for Initial Print Letter Tab 
    Private Sub ShowHideInitialLetterTabDetails(ByVal isVisible As Boolean)
        lnkInitial.Visible = Not isVisible
        lbllnkInitial.Visible = isVisible
        divToShowReprintLinks.Visible = False
        divToShowReprintFollowupLink.Visible = False
        ddlRMDYear.Enabled = True
        If (isVisible) Then
            btnPrintLetters.Text = "Print Initial Letter"
            lblDescription.Text = "List of person(s) eligible for generating initial letters."

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
        divToShowReprintLinks.Visible = False
        divToShowReprintFollowupLink.Visible = False
        ddlRMDYear.Enabled = True
        If (isVisible) Then
            btnPrintLetters.Text = "Print Follow-up Letter"
            lblDescription.Text = "List of person(s) eligible for generating follow-up letters."

            ddlAcctLocked.SelectedValue = "All"
            ddlFollowUp.SelectedValue = "All"
            ddlInsufficientBal.SelectedValue = "No"
        End If

    End Sub

    'Function to get all the batchid(s) For Reprint RMD letter Tab and bind them to repeater control
    Private Sub GetRePrintLetterRecords()
        Dim dsReprintLetterRecords As DataSet
        Dim dtReprintLetterRecords As DataTable
        Dim strMrdYear() As String = {"MRDYear"}
        Dim lstItem As ListItem
        Dim batchDetailView As New DataView
        Dim filterByYear As String = String.Empty
        Try

            dsReprintLetterRecords = YMCARET.YmcaBusinessObject.MRDBO.GetRMDLetterBatchIDList(BatchProcess.RMDPrintLetters.ToString)  'Getting all the generated batch id's 
            If (HelperFunctions.isNonEmpty(dsReprintLetterRecords)) AndAlso dsReprintLetterRecords.Tables(0).Rows.Count > 0 Then

                dtReprintLetterRecords = dsReprintLetterRecords.Tables(0).DefaultView.ToTable(True, strMrdYear)  'Getting the distinct  MRD years
                ddlRMDYear.Items.Clear()

                For iCount As Integer = 0 To dtReprintLetterRecords.Rows.Count - 1
                    lstItem = New ListItem
                    lstItem.Text = dtReprintLetterRecords.Rows(iCount)("MRDYear").ToString
                    lstItem.Value = dtReprintLetterRecords.Rows(iCount)("MRDYear").ToString
                    ddlRMDYear.Items.Add(lstItem)    'Inserting MRD year values in drop down
                Next
                If dtReprintLetterRecords.Rows.Count > 0 Then
                    ddlRMDYear.Items(dtReprintLetterRecords.Rows.Count - 1).Selected = True    'assign  the recent MRD Year as default selected value in MRD Year
                End If

                If dsReprintLetterRecords.Tables.Count > 0 Then
                    RMDRePrintBatchRecords = dsReprintLetterRecords.Tables(0)     'Saving Batchid in ViewState
                Else
                    RMDRePrintBatchRecords = Nothing
                End If
                If (HelperFunctions.isNonEmpty(dsReprintLetterRecords.Tables(0))) Then
                    batchDetailView = dsReprintLetterRecords.Tables(0).DefaultView   'Data View to filter batch records as per year selection
                    If ddlRMDYear.Items.Count > 0 Then
                        If ddlRMDYear.SelectedValue <> "" AndAlso String.IsNullOrEmpty(filterByYear) Then
                            filterByYear = String.Format(" MRDYear = {0}", ddlRMDYear.SelectedValue)
                        End If
                    End If
                    batchDetailView.RowFilter = filterByYear
                    rptRMDBatchIDs.DataSource = batchDetailView
                    rptRMDBatchIDs.DataBind()
                End If
            Else                        ' If no batch records we found then RMD year drop down is disabled followed by "no recods found" message is displayed on screen  
                divBatchTitle.InnerHtml = "No Batch Exists"
                ddlRMDYear.Items.Clear()
                ddlRMDYear.Enabled = False
                gvLetters.EmptyDataText = "No records found."
                gvLetters.DataSource = Nothing
                gvLetters.DataBind()
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ' Filling the dropdown for batch id in RMD reprint letter tab
    Private Sub FillRMDReprintRecords()
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Dim Sort As GridViewCustomSort
        Dim strFilter As String = String.Empty
        Dim strMrdYear() As String = {"MRDYear"}
        Dim lstItem As ListItem
        Dim dtYear As DataTable
        Try
            If lbllnkReprintLetter.Visible Then
                dtRecords = DirectCast(RMDRePrintBatchRecords, DataTable)

            End If

            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView

                If ddlRMDYear.Items.Count > 0 Then
                    If ddlRMDYear.SelectedValue <> "" Then
                        If strFilter = String.Empty Then
                            strFilter = " MRDYear = " + ddlRMDYear.SelectedValue
                        Else
                            strFilter += " And MRDYear = " + ddlRMDYear.SelectedValue
                        End If

                    End If
                End If

                dv.RowFilter = strFilter

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub rptRMDBatchIDs_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptRMDBatchIDs.ItemDataBound
        Dim lnkRMDRePrintLetterBatchId As LinkButton
        Dim strBatchId As String
        Dim htmlGenericControl As HtmlTableCell
        Try
            If e.Item.ItemIndex = 0 Then
                lnkRMDRePrintLetterBatchId = e.Item.FindControl("lnkRMDRePrintLetterBatchID")
                BindRMDReprintLetter(lnkRMDRePrintLetterBatchId.Text.Trim)
                strBatchId = lnkRMDRePrintLetterBatchId.Text.Trim
                DisplayPrintLetterLinks(strBatchId)
                htmlGenericControl = e.Item.FindControl("liRMDRePrintLetterBatchIDs")
                htmlGenericControl.Attributes("class") = "tabSelectedLink"
                'lblMessage.Text += "BatchID " + lnkRMDRePrintLetterBatchId.Text 'PPP | 04/21/2017 | YRS-AT-3356 | lblMessage is commented and moved to control but this update is not required on control also. This label is on confirmation dialog box
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("rptRMDBatchIDs_ItemDataBound", ex)
        End Try
    End Sub

    'Function to get the details after selecting the batch id and displaying it in table format
    Protected Sub lnkRMDRePrintLetterBatchID_Click(sender As Object, e As EventArgs)
        Dim strBatchId As String
        If Not sender Is Nothing AndAlso sender.Text() IsNot Nothing Then
            strBatchId = sender.Text()
            BindRMDReprintLetter(strBatchId)
            DisplayPrintLetterLinks(strBatchId)
        End If
    End Sub

    'Get the details of RMD letter by using batch id as  input parameter
    Private Sub BindRMDReprintLetter(strBatchId As String)
        Dim dsBatchProcess As DataSet
        Dim strDivMessageRMDReprintBatchType As String
        Try
            If Not String.IsNullOrEmpty(strBatchId) Then
                dsBatchProcess = YMCARET.YmcaBusinessObject.MRDBO.GetRMDLetterDetailsByBatchId(strBatchId, BatchProcess.RMDPrintLetters.ToString)
                HighlightSubMenu(strBatchId)
                If HelperFunctions.isNonEmpty(dsBatchProcess) Then
                    If dsBatchProcess.Tables.Contains("SelectedBatchRecords") Then
                        RMDRePrintSelectedBatchDetails = dsBatchProcess.Tables("SelectedBatchRecords")
                        gvLetters.DataSource = dsBatchProcess.Tables("SelectedBatchRecords")
                        gvLetters.DataBind()
                        If (strBatchId.Contains("I")) Then
                            strDivMessageRMDReprintBatchType = String.Format("RMD Initial Letter for Batch Id - {0}", strBatchId)
                        ElseIf (strBatchId.Contains("F")) Then
                            strDivMessageRMDReprintBatchType = String.Format("RMD Follow-Up Letter for Batch Id - {0}", strBatchId)
                        End If

                    Else
                        gvLetters.EmptyDataText = "No records found."
                        RMDRePrintSelectedBatchDetails = Nothing
                        gvLetters.DataSource = Nothing
                        gvLetters.DataBind()
                        strDivMessageRMDReprintBatchType = String.Format("No Records to display for Batch Id - {0}", strBatchId)
                    End If

                    divBatchwiseInformation.InnerHtml = strDivMessageRMDReprintBatchType
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("lnkRMDRePrintLettterBatchID_Click", ex)
        End Try
    End Sub

    'Highlight the selected BatchId
    Public Sub HighlightSubMenu(ByRef strRMDYear As String)
        Try
            Dim htmlGenericControl As HtmlTableCell
            Dim lnkRMD As LinkButton
            For Each rptItem As RepeaterItem In rptRMDBatchIDs.Items
                lnkRMD = rptItem.FindControl("lnkRMDRePrintLetterBatchID")
                htmlGenericControl = rptItem.FindControl("liRMDRePrintLetterBatchIDs")
                If lnkRMD.Text = strRMDYear Then
                    htmlGenericControl.Attributes("class") = "tabSelectedLink"
                    'lblMessage.Text += "BatchID " + strRMDYear 'PPP | 04/21/2017 | YRS-AT-3356 | lblMessage is commented and moved to control but this update is not required on control also. This label is on confirmation dialog box
                Else
                    htmlGenericControl.Attributes("class") = ""
                    htmlGenericControl.Attributes("padding-bottom") = "10px"
                End If
            Next
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_HighlightSubMenu", ex)
        End Try
    End Sub
#Region "Property"
    Public Property SelectedTab() As EnumTabStrips     ' We are setting this property to set the value of the selected tab in print letter 
        Get
            If Not (Session("SelectedTab")) Is Nothing Then
                Return (CType(Session("SelectedTab"), EnumTabStrips))
            Else
                Return EnumTabStrips.NONE
            End If
        End Get
        Set(ByVal Value As EnumTabStrips)
            Session("SelectedTab") = Value
        End Set
    End Property

    'START: PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence this property is no longer required
    'Public Property RMDLettersProgressPopupClosed() As Boolean         ' We are setting this property to set the value of the after click of close button in letter generation pop up window.
    '    Get
    '        If Not (Session("RMDLettersProgressPopupClosed")) Is Nothing Then
    '            Return (CType(Session("RMDLettersProgressPopupClosed"), Boolean))
    '        Else
    '            Return False
    '        End If
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        Session("RMDLettersProgressPopupClosed") = Value
    '    End Set
    'End Property
    'END: PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence this property is no longer required

    Public Property RMDRePrintBatchRecords() As DataTable        ' We are setting this property to set the details of the all the batchid(s)
        Get
            If Not (ViewState("RMDRePrintBatchRecords")) Is Nothing Then
                Return (CType(ViewState("RMDRePrintBatchRecords"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            ViewState("RMDRePrintBatchRecords") = Value
        End Set
    End Property

    Public Property RMDRePrintSelectedBatchDetails() As DataTable        ' We are setting this property to set the details (datatable) of the selected batch id  in reprint RMD letter
        Get
            If Not (ViewState("RMDRePrintSelectedBatchDetails")) Is Nothing Then
                Return (CType(ViewState("RMDRePrintSelectedBatchDetails"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            ViewState("RMDRePrintSelectedBatchDetails") = Value
        End Set
    End Property

    'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
    'Public Property RMDRePrintInitialLetterFileNameWithFullPath() As String        ' We are setting this property to set the Path of the file when batch id  in reprint RMD letter is selected for initial print letters
    '    Get
    '        If Not (Session("RMDRePrintInitialLetterFileNameWithPath")) Is Nothing Then
    '            Return (CType(Session("RMDRePrintInitialLetterFileNameWithPath"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("RMDRePrintInitialLetterFileNameWithPath") = Value
    '    End Set
    'End Property

    'Public Property RMDRePrintCashoutLetterFileNameWithFullPath() As String        ' We are setting this property to set the Path of the file when batch id  in reprint RMD letter is selected for initial print letters
    '    'if cashout letter is present then store the filename with full path 
    '    Get
    '        If Not (Session("RMDRePrintCashoutLetterFileNameWithFullPath")) Is Nothing Then
    '            Return (CType(Session("RMDRePrintCashoutLetterFileNameWithFullPath"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("RMDRePrintCashoutLetterFileNameWithFullPath") = Value
    '    End Set
    'End Property

    'Public Property RMDRePrintFollowupLetterFileNameWithFullPath() As String        ' We are setting this property to set the Path of the file when batch id  in reprint RMD letter is selected for follow-up print letters
    '    Get
    '        If Not (Session("RMDRePrintFollowupLetterFileNameWithFullPath")) Is Nothing Then
    '            Return (CType(Session("RMDRePrintFollowupLetterFileNameWithFullPath"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("RMDRePrintFollowupLetterFileNameWithFullPath") = Value
    '    End Set
    'End Property
    'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required

    'START: MMR | 2017.05.16 | YRS-AT-3356 | Declared property to store page read only access rights
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
    'END: MMR | 2017.05.16 | YRS-AT-3356 | Declared property to store page read only access rights
#End Region
    'Enum to know which tab is selected 
    Public Enum EnumTabStrips
        NONE
        INITIAL
        FOLLOWUP
        REPRINT
    End Enum

    'Display the Links in reprint rmd letter tab
    Private Sub DisplayPrintLetterLinks(strBatchId As String)
        Dim encryptLink As EncryptedQueryString 'PPP | 05/02/2017 | YRS-AT-3356 | EncryptedQueryString helps to encrypt links

        Dim strFilePath As String
        Dim strArrLetterType() As String
        Dim relativePath As String
        strArrLetterType = strBatchId.Split("-")
        strFilePath = String.Empty
        lnkRMDRePrintInitialLetter.Visible = False
        lnkRMDRePrintCashoutLetter.Visible = False
        divToShowReprintLinks.Visible = False
        divToShowReprintFollowupLink.Visible = False

        If (Trim(strArrLetterType(1)) = "I") Then
            strFilePath = String.Format("{0}\RMD_Initial_Letter_{1}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), Trim(strArrLetterType(0)))
            relativePath = String.Format("{0}\\RMD_Initial_Letter_{1}.pdf", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), Trim(strArrLetterType(0)))
            If File.Exists(strFilePath) Then
                'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page
                encryptLink = New EncryptedQueryString()
                encryptLink.Add("link", relativePath)
                lnkRMDRePrintInitialLetter.Attributes.Add("onClick", String.Format("javascript:return OpenPDF('DocumentViewer.aspx?p={0}')", encryptLink.ToString()))
                'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page

                lnkRMDRePrintInitialLetter.Visible = True
                divToShowReprintLinks.Visible = True
                divToShowReprintFollowupLink.Visible = False
                'RMDRePrintInitialLetterFileNameWithFullPath = relativePath 'PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
                spanPipeForCashout.Visible = False
            End If

            strFilePath = String.Format("{0}\RMD_Cash_Out_Letter_{1}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), Trim(strArrLetterType(0)))
            relativePath = String.Format("{0}\\RMD_Cash_Out_Letter_{1}.pdf", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), Trim(strArrLetterType(0)))
            If File.Exists(strFilePath) Then
                'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page
                encryptLink = New EncryptedQueryString()
                encryptLink.Add("link", relativePath)
                lnkRMDRePrintCashoutLetter.Attributes.Add("onClick", String.Format("javascript:return OpenPDF('DocumentViewer.aspx?p={0}')", encryptLink.ToString()))
                'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page

                lnkRMDRePrintCashoutLetter.Visible = True
                divToShowReprintLinks.Visible = True
                divToShowReprintFollowupLink.Visible = False
                'RMDRePrintCashoutLetterFileNameWithFullPath = relativePath 'PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
                If (lnkRMDRePrintInitialLetter.Visible = True And lnkRMDRePrintCashoutLetter.Visible = True) Then
                    spanPipeForCashout.Visible = True
                End If
            End If
            'End If
        ElseIf (Trim(strArrLetterType(1)) = "F") Then
            strFilePath = String.Format("{0}\RMD_Followup_Letter_{1}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), Trim(strArrLetterType(0)))
            relativePath = String.Format("{0}\\RMD_Followup_Letter_{1}.pdf", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), Trim(strArrLetterType(0)))
            If File.Exists(strFilePath) Then
                'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page
                encryptLink = New EncryptedQueryString()
                encryptLink.Add("link", relativePath)
                lnkRMDRePrintFollowupLetter.Attributes.Add("onClick", String.Format("javascript:return OpenPDF('DocumentViewer.aspx?p={0}')", encryptLink.ToString()))
                'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page

                lnkRMDRePrintFollowupLetter.Visible = True
                divToShowReprintLinks.Visible = False
                divToShowReprintFollowupLink.Visible = True
                'RMDRePrintFollowupLetterFileNameWithFullPath = relativePath 'PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
            End If
            ' START : SB | 11/22/2016 | YRS-AT-3203 |  Setting path for cashout followup letter
            strFilePath = String.Format("{0}\RMD_CashOut_Followup_Letter_{1}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath")), Trim(strArrLetterType(0)))
            relativePath = String.Format("{0}\\RMD_CashOut_Followup_Letter_{1}.pdf", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), Trim(strArrLetterType(0)))
            If File.Exists(strFilePath) Then
                'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page
                encryptLink = New EncryptedQueryString()
                encryptLink.Add("link", relativePath)
                lnkRMDRePrintCashoutFollowupLetter.Attributes.Add("onClick", String.Format("javascript:return OpenPDF('DocumentViewer.aspx?p={0}')", encryptLink.ToString()))
                'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing encrypted link to DocumentViewer page

                lnkRMDRePrintCashoutFollowupLetter.Visible = True
                divToShowReprintLinks.Visible = False
                divToShowReprintFollowupLink.Visible = True
                'RMDRePrintCashOutFollowupLetterFileNameWithFullPath = relativePath 'PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
                If (lnkRMDRePrintFollowupLetter.Visible = True And lnkRMDRePrintCashoutFollowupLetter.Visible = True) Then
                    spanPipeForCashoutFollowup.Visible = True
                End If
            End If
            ' END : SB | 11/22/2016 | YRS-AT-3203 |  Setting path for cashout followup letter
        End If
    End Sub

    'START: PPP | 05/02/2017 | YRS-AT-3356 | Event handling not required now, links hold encrypted relateve path in them
    'Private Sub lnkRMDRePrintInitialLetter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRMDRePrintInitialLetter.Click
    '    Try
    '        'Call popup to open RMD letter for Print & Save. For RMD letter pass report type as 1
    '        Dim popupScript As String = "<script language='javascript'>" & _
    '                                     "window.open('DocumentViewer.aspx?ReportType=1', 'OpenRMDInitialLetterPDF', 'width=1024, height=768, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no')" & _
    '                                     "</script>"
    '        If popupScript <> String.Empty Then
    '            ScriptManager.RegisterStartupScript(uplGenerateRMD, uplGenerateRMD.GetType(), "CallRMDLetterViewFunction", popupScript, False)
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub lnkRMDRePrintCashoutLetter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRMDRePrintCashoutLetter.Click
    '    Try
    '        ' Call popup to open Cashout letter for Print & Save. For cashout letter pass report type as 2
    '        Dim popupScript As String = "<script language='javascript'>" & _
    '                                       "window.open('DocumentViewer.aspx?ReportType=2', 'OpenRMDCashoutLetterPDF', 'width=1024, height=768, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no')" & _
    '                                       "</script>"
    '        If popupScript <> String.Empty Then
    '            ScriptManager.RegisterStartupScript(uplGenerateRMD, uplGenerateRMD.GetType(), "CallRMDLetterViewFunction", popupScript, False)
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub lnkRMDRePrintFollowupLetter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRMDRePrintFollowupLetter.Click
    '    Try
    '        'Call popup to open RMD letter for Print & Save. For RMD letter pass report type as 3
    '        Dim popupScript As String = "<script language='javascript'>" & _
    '                                     "window.open('DocumentViewer.aspx?ReportType=3', 'OpenRMDFollowupLetterPDF', 'width=1024, height=768, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no')" & _
    '                                     "</script>"
    '        If popupScript <> String.Empty Then
    '            ScriptManager.RegisterStartupScript(uplGenerateRMD, uplGenerateRMD.GetType(), "CallRMDLetterViewFunction", popupScript, False)
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'END: PPP | 05/02/2017 | YRS-AT-3356 | Event handling not required now, links hold encrypted relateve path in them
    ' END : SB | 10/13/2016 | YRS-AT-2685 |  Changes to hide Reprint letter filters , tab selected functions, function to BatchId(s) with details, property defined for variables,defining ENUM, and dipaly link dynamically for initial letter/Cashout letter

    'START : CS |2016.10.24 | YRS-AT-3088 |  TO get/set the Cashout letter file name
    Public Property CashOutLetterFileName() As String
        Get
            If Not Session("CashOutLetterFilename") Is Nothing Then
                Return (CType(Session("CashOutLetterFilename"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("CashOutLetterFilename") = Value
        End Set
    End Property
    'END : CS |2016.10.24 | YRS-AT-3088 |  TO get/set the Cashout letter file name

    'START : CS |2016.10.24 | YRS-AT-3088 |  TO get/set the Follow up letter file name
    Public Property FollowLetterFileName() As String
        Get
            If Not Session("FollowLetterFilename") Is Nothing Then
                Return (CType(Session("FollowLetterFilename"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FollowLetterFilename") = Value
        End Set
    End Property
    'END : CS |2016.10.24 | YRS-AT-3088 |  TO get/set the Follow up letter file name

    'START : CS |2016.10.24 | YRS-AT-3088 |  TO get/set the initial letter file name
    Public Property InitialLetterFileName() As String
        Get
            If Not Session("InitialLetterFileName") Is Nothing Then
                Return (CType(Session("InitialLetterFileName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("InitialLetterFileName") = Value
        End Set
    End Property
    'END : CS |2016.10.24 | YRS-AT-3088 |  TO get/set the initial letter file name

    'START : SB |2016.11.07 | YRS-AT-2685 |  Function to clear all the session variables and function to assign batch date for generating letters
    Private Sub ClearSessionVariables()
        'Session("gvSelectedRow") = Nothing 'MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required
        Session("ReportName") = Nothing
        'Session("gvSelectedRow") = Nothing 'MMR | 2017.05.15 | YRS-AT-3356 | Commented as not required
        Session("PrintLetters") = Nothing
        Session("SelectedRMDPrintLetters") = Nothing
        ' Session("Selecttab") =Nothing   'This SelectedTab=Nothing   'Do not Clear this session variable as value of Selected tab is used in page load function
        Session("l_datatable_PrintInitialLetters") = Nothing
        Session("l_datatable_PrintFollowUpLetters") = Nothing
        Session("FTFileList") = Nothing
        Session("strReportName") = Nothing
        Session("MergedPdf_Filename") = Nothing
        'Session("strReportName") = Nothing       '  SB |2016.11.22 | YRS-AT-3203 |  repeated code commented
        Session("RMDBatchFundNo") = Nothing
        'Me.RMDLettersProgressPopupClosed = Nothing ' PPP | 04/17/2017 | YRS-AT-3237 | Close button is now able to refresh the data without reloading the page hence RMDLettersProgressPopupClosed property is no longer required
        'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
        'Me.RMDRePrintInitialLetterFileNameWithFullPath = Nothing
        'Me.RMDRePrintCashoutLetterFileNameWithFullPath = Nothing
        'Me.RMDRePrintFollowupLetterFileNameWithFullPath = Nothing
        'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
        Me.CashOutLetterFileName = Nothing
        Me.FollowLetterFileName = Nothing
        Me.InitialLetterFileName = Nothing
        'START: MMR | 2017.05.16 | YRS-AT-3356 | Clear Session values
        Me.ReadOnlyWarningMessage = Nothing
        Session("IsReadOnlyAccess") = Nothing
        Session("SelectedTab") = Nothing
        'END: MMR | 2017.05.16 | YRS-AT-3356 | Clear Session values
    End Sub

    'Function to assign batchid date for generating letters assigning next year date as orocessing date
    Private Function GetSelectedMRDYearDueDate() As String
        Dim intYear As Integer = ddlRMDYear.SelectedItem.Text
        Return ("03/31/" + (intYear + 1).ToString())
    End Function
    'END : SB |2016.11.07 | YRS-AT-2685 |  Function to clear all the session variables and function to assign batch date for generating letters

    'START : SB |2016.11.08 | YRS-AT-2685 |  Filtering of batch Records for year selection  in Reprint Letter tab
    Private Sub ddlRMDYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRMDYear.SelectedIndexChanged
        Dim batchDetailView As New DataView
        Dim batchRecordsTable As DataTable
        Dim filterByYear As String = String.Empty
        Try
            If SelectedTab = EnumTabStrips.REPRINT Then
                If (HelperFunctions.isNonEmpty(RMDRePrintBatchRecords)) Then
                    batchRecordsTable = RMDRePrintBatchRecords
                End If
            End If
            If HelperFunctions.isNonEmpty(batchRecordsTable) Then
                batchDetailView = batchRecordsTable.DefaultView
                If ddlRMDYear.Items.Count > 0 Then
                    If ddlRMDYear.SelectedValue <> "" AndAlso String.IsNullOrEmpty(filterByYear) Then
                        filterByYear = " MRDYear = " + ddlRMDYear.SelectedValue
                    End If
                End If
                batchDetailView.RowFilter = filterByYear
                rptRMDBatchIDs.DataSource = batchDetailView
                rptRMDBatchIDs.DataBind()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDPrintLetters_ddlRMDYear_SelectedIndexChanged", ex)
        End Try
    End Sub
    'END : SB |2016.11.08 | YRS-AT-2685 |  Filtering of batch Records for year selection  in Reprint Letter tab

    'START : SB |2016.11.21 | YRS-AT-3203 |  TO get/set the Follow up Cashout letter file name
    'START: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required
    'Public Property CashOutFollowupLetterFileName() As String
    '    Get
    '        If Not Session("CashOutFollowupLetterFileName") Is Nothing Then
    '            Return (CType(Session("CashOutFollowupLetterFileName"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("CashOutFollowupLetterFileName") = Value
    '    End Set
    'End Property

    'Public Property RMDRePrintCashOutFollowupLetterFileNameWithFullPath() As String        ' We are setting this property to set the Path of the file when batch id  in reprint RMD letter is selected for cashout follow-up print letters
    '    Get
    '        If Not (Session("RMDRePrintCashOutFollowupLetterFileNameWithFullPath")) Is Nothing Then
    '            Return (CType(Session("RMDRePrintCashOutFollowupLetterFileNameWithFullPath"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("RMDRePrintCashOutFollowupLetterFileNameWithFullPath") = Value
    '    End Set
    'End Property
    'END: PPP | 05/02/2017 | YRS-AT-3356 | Directly passing file path to DocumentViewer so a separate property to hold it is not required

    'START: PPP | 05/02/2017 | YRS-AT-3356 | Event handling not required now, links hold encrypted relateve path in them
    'Private Sub lnkRMDRePrintCashoutFollowupLetter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRMDRePrintCashoutFollowupLetter.Click   'After clicking the link for cashoutfollowup letter pdf file should be generated in new window
    '    Try
    '        'Call popup to open RMD letter for Print & Save. For RMD letter pass report type as 4
    '        Dim popupScript As String = "<script language='javascript'>" & _
    '                                     "window.open('DocumentViewer.aspx?ReportType=4', 'OpenRMDCashoutFollowupLetterPDF', 'width=1024, height=768, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no')" & _
    '                                     "</script>"
    '        If popupScript <> String.Empty Then
    '            ScriptManager.RegisterStartupScript(uplGenerateRMD, uplGenerateRMD.GetType(), "CallRMDLetterViewFunction", popupScript, False)
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("lnkRMDRePrintCashoutFollowupLetter_Click", ex)
    '        Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())), False)
    '    End Try
    'End Sub
    'END: PPP | 05/02/2017 | YRS-AT-3356 | Event handling not required now, links hold encrypted relateve path in them
    'END :  SB |2016.11.21 | YRS-AT-3203 |  TO get/set the Follow up Cashout letter file name

    'START: MMR | 2017.05.16 | YRS-AT-3356 | Method will store read only access rights in property on load which will be used to check in controls to stop user from performing further operations
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("RMDPrintLetters.aspx", Convert.ToInt32(Session("LoggedUserKey")))
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
    'END: MMR | 2017.05.16 | YRS-AT-3356 | Method will store read only access rights in property on load which will be used to check in controls to stop user from performing further operations

    'START: MMR | 2017.05.16 | YRS-AT-3356 | Method to stop user from performing further operations on reprint links
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
    'END: MMR | 2017.05.16 | YRS-AT-3356 | Method to stop user from performing further operations on reprint links
End Class