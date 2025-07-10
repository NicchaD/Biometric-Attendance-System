'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	TerminationWatcherProcessPurge.aspx.vb
' Author Name		:	Priya Patil
' Employee ID		:	37786
' Email			    :	priya.jawale@3i-infotech.com
' Contact No		:	8416
' Creation Time	:	8/25/2012 6:36:14 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Anudeep          01-Nov-2012       Changes made as Per observations listed in bugtraker for Yrs 5.0-1484 on 01 nov 2012
'Anudeep          05-nov-2012       Changes Made as Per Observations listed in bugtrake For yrs 5.0-1484 on 06-nov 2012
'Anudeep          19-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
'Anudeep          11-oct-2013       Bt-957-YRS 5.0-1484:New Utility to replace Refund Watcher program
'Anudeep          23-dec-2013       BT:2311 13.3.0 Observations
'Manthan Rajguru  2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Public Class TerminationWatcherProcessPurge
    Inherits System.Web.UI.Page

    Private Property dsPurgeApplicant() As DataSet
        Get
            Return (CType(ViewState("dsPurgeApplicant"), DataSet))
        End Get
        Set(ByVal Value As DataSet)
            ViewState("dsPurgeApplicant") = Value
        End Set
    End Property
    Private Property dsProcessedApplicant() As DataSet
        Get
            Return (CType(ViewState("dsProcessedApplicant"), DataSet))
        End Get
        Set(ByVal Value As DataSet)
            ViewState("dsProcessedApplicant") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            If Not IsPostBack Then
                'AssignTextToNote()
                Dim dskey As DataSet

                dskey = YMCARET.YmcaBusinessObject.TerminationWatcherBO.getConfigurationValue("TW_PURGE_PERIOD")
                If Not IsNothing(Request.QueryString("form")) Then
                    If Not IsNothing(Request.QueryString("Action")) Then
                        If Request.QueryString("Action") = "Purge" Then
                            lblHead.Text = "Termination Watcher – Purge Applicants"
                            Dim key As String
                            key = dskey.Tables(0).Rows(0).Item(4).ToString()
                            If key.Contains("M") Then
                                key = key.Replace("M", " Months")
                            ElseIf key.Contains("D") Then
                                key = key.Replace("D", " Days")
                            End If
                            lblpurgedescr.Text = String.Format(messageFromResourceFile("MESSAGE_TW_PURGE_RECORD_DESCR", ""), key)
                            Session("Flag") = "Purge"
                            Session("Delete") = False
                            gvPurge.Visible = True
                            'Anudeep:19-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
                            divProcess.Visible = False
                            divPurge.Visible = True
                            gvProcessed.Visible = False
                            FetchPurgeData(Request.QueryString("form").ToString().Trim)
                        ElseIf Request.QueryString("Action") = "Process" Then
                            lblHead.Text = "Termination Watcher –  Processed Applicants"
                            'Added by Anudeep : 30-10-2012 as per observations for Add description of process pop up
                            lblpurgedescr.Text = messageFromResourceFile("MESSAGE_TW_PROCESS_RECORD_DESCR", "")
                            gvPurge.Visible = False
                            gvProcessed.Visible = True
                            'Anudeep:19-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
                            divPurge.Visible = False
                            divProcess.Visible = True
                            btnDelete.Visible = False
                            FetchProcessedData(Request.QueryString("form").ToString().Trim)
                        ElseIf Request.QueryString("Action") = "Invalid" Then
                            lblHead.Text = "Termination Watcher –   Invalid Applicants"
                            gvPurge.Visible = False
                            gvProcessed.Visible = True
                            btnDelete.Visible = False
                            FetchInvalidData(Request.QueryString("form").ToString().Trim)
                        End If
                    End If
                End If
            End If
            If Request.Form("Yes") = "Yes" Then
                If Session("Flag") = "Purge" Then
                    PurgeRecords()
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Get the selected records from gridview
    Private Function getSelectedRecords(ByVal gv As GridView) As List(Of String)
        Dim LTerminationIds As New List(Of String)
        Dim chkAll, chk As New CheckBox
        Dim i As Integer = 0
        Try
            For i = 0 To gv.Rows.Count - 1
                chkAll = gv.Rows(i).FindControl("chkSelectAll")
                chk = gvPurge.Rows(i).FindControl("chkSelect")
                If Not IsNothing(chk) Then
                    If chk.Checked = True Then
                        LTerminationIds.Add(gvPurge.DataKeys(i).Value.ToString())
                    End If
                End If
            Next
            Return LTerminationIds
        Catch
            Throw
        End Try
    End Function
    'Purge Records
    Private Sub PurgeRecords()
        Dim strResult As String = String.Empty
        Dim LTerminationIds As New List(Of String)
        Try
            LTerminationIds = getSelectedRecords(gvPurge)
            If LTerminationIds.Count > 0 Then
                strResult = YMCARET.YmcaBusinessObject.TerminationWatcherBO.PurgeTerminationWatcherData(LTerminationIds)
            End If
            If strResult = "-1" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_RECORD_NOT_EXISTS", ""), MessageBoxButtons.OK)
            ElseIf strResult = "-2" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_PURGE_ERROR", ""), MessageBoxButtons.OK)
            ElseIf strResult = "-3" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_PURGE_ERROR_TBLTWNOTES", ""), MessageBoxButtons.OK)
            ElseIf strResult = "1" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_DELETE_SUCESS", ""), MessageBoxButtons.OK)
                If Not IsNothing(Request.QueryString("form").ToString().Trim) Then
                    FetchPurgeData(Request.QueryString("form").ToString().Trim)
                End If

            End If
        Catch
            Throw
        End Try

    End Sub
    'Gets the Purge data from db
    Private Sub FetchPurgeData(ByVal strType As String)
        'Dim dv As DataView
        Try
            dsPurgeApplicant = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListPurgeRecord(strType)
            If HelperFunctions.isNonEmpty(dsPurgeApplicant) Then
                LabelNoDataFound.Visible = False
                'AA:23.12.2013 - BT:2311 changed to get the record as per the strType
                gvPurge.DataSource = dsPurgeApplicant
                gvPurge.DataBind()
                'Added by Anudeep as per Observations 23-oct-2012 
                'shows how many record exists in grid
                lblTotalCount.Text = messageFromResourceFile("MESSAGE_TW_SHOW_TOTAL_RECORDS", "") + " " + dsPurgeApplicant.Tables(0).Rows.Count.ToString()
            Else
                LabelNoDataFound.Visible = True
                btnDelete.Enabled = False
                gvPurge.DataSource = Nothing
                gvPurge.DataBind()
                lblTotalCount.Text = ""
            End If
        Catch
            Throw
        End Try

    End Sub
    'Gets the Processed data from db
    Private Sub FetchProcessedData(ByVal strType As String)
        Try

            dsProcessedApplicant = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListProcessedRecord(strType)
            If HelperFunctions.isNonEmpty(dsProcessedApplicant) Then
                gvProcessed.DataSource = dsProcessedApplicant
                gvProcessed.DataBind()

                LabelNoDataFound.Visible = False
            Else
                gvProcessed.DataSource = Nothing
                gvProcessed.DataBind()
                LabelNoDataFound.Visible = True
                lblTotalCount.Text = ""
            End If
            If Not IsNothing(Request.QueryString("Flag")) Then
                Dim struniqueid As String
                Dim LTerminationIds As New List(Of String)
                If Request.QueryString("Flag") = "Process" Then
                    LTerminationIds = Session("LTerminationIds")
                    For Each item As String In LTerminationIds
                        If struniqueid = String.Empty Then
                            struniqueid = "'" & item & "'"
                        Else
                            struniqueid = struniqueid & ",'" & item & "'"
                        End If
                    Next
                    dsProcessedApplicant.Tables(0).DefaultView.RowFilter = "UniqueID in (" & struniqueid & ")"
                    'Added by Anudeep as per Observations 23-oct-2012 
                    'shows how many record exists in grid
                    lblTotalCount.Text = messageFromResourceFile("MESSAGE_TW_SHOW_TOTAL_RECORDS", "") + " " + dsProcessedApplicant.Tables(0).DefaultView.Count.ToString()
                    gvProcessed.DataSource = dsProcessedApplicant.Tables(0).DefaultView
                    gvProcessed.DataBind()
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Gets the invalid data from db
    Private Sub FetchInvalidData(ByVal strType As String)
        Try
            dsProcessedApplicant = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListInvalidRecord(strType)
            If HelperFunctions.isNonEmpty(dsProcessedApplicant) Then
                LabelNoDataFound.Visible = False
                gvProcessed.DataSource = dsProcessedApplicant
                gvProcessed.DataBind()
            Else
                LabelNoDataFound.Visible = True
                gvProcessed.DataSource = Nothing
                gvProcessed.DataBind()
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnDelete", Convert.ToInt32(Session("LoggedUserKey")))
        Try
            Session("Delete") = True
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            Dim LTerminationIds As New List(Of String)
            LTerminationIds = getSelectedRecords(gvPurge)
            If (LTerminationIds.Count = 0) Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_SELECT_RECORD", ""), MessageBoxButtons.OK)
            Else
                'Anudeep:05-nov-2012- Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
                Session("Refresh") = "YES"
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_DELETE_CONFIRMATION", ""), MessageBoxButtons.YesNo)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Processed Gridview Page Indexing
    Private Sub gvProcessed_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProcessed.PageIndexChanging
        Dim dv As DataView
        Try
            If (Not ViewState("Add_Sort") Is Nothing) And (Not dsProcessedApplicant Is Nothing) Then
                dv = dsProcessedApplicant.Tables(0).DefaultView
                dv.Sort = ViewState("Add_Sort")
                Me.gvProcessed.PageIndex = e.NewPageIndex
                gvProcessed.DataSource = dv
                gvProcessed.DataBind()
            Else
                Me.gvProcessed.PageIndex = e.NewPageIndex
                If Request.QueryString("Action") = "Process" Then
                    gvPurge.Visible = False
                    gvProcessed.Visible = True
                    btnDelete.Visible = False
                    FetchProcessedData(Request.QueryString("form").ToString().Trim)
                ElseIf Request.QueryString("Action") = "Invalid" Then
                    gvPurge.Visible = False
                    gvProcessed.Visible = True
                    btnDelete.Visible = False
                    FetchInvalidData(Request.QueryString("form").ToString().Trim)
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Purge Gridview Page Indexing
    Private Sub gvPurge_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPurge.PageIndexChanging
        Dim dv As DataView
        Try
            If (Not ViewState("Add_Sort") Is Nothing) And (Not dsPurgeApplicant Is Nothing) Then
                dv = dsPurgeApplicant.Tables(0).DefaultView
                dv.Sort = ViewState("Add_Sort")
                Me.gvPurge.PageIndex = e.NewPageIndex
                gvPurge.DataSource = dv
                gvPurge.DataBind()
            Else
                Me.gvPurge.PageIndex = e.NewPageIndex
                FetchPurgeData(Request.QueryString("form").ToString().Trim)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Purge RowDataBound
    Private Sub gvPurge_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPurge.RowDataBound
        Dim processed, strLastcontributionReceived, strUnfundedTransactions, strCreatedOn As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strLastcontributionReceived = Convert.ToString(drv("LastcontributionREceived"))
                strUnfundedTransactions = Convert.ToString(drv("UnfundedTransactions"))
                'Anudeep:19-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
                strCreatedOn = Convert.ToString(drv("CreatedDate"))
                processed = Convert.ToString(drv("Processed"))
                e.Row.Cells(14).Text = Convert.ToDateTime(strCreatedOn).Date
                If processed = "2" Then
                    e.Row.ForeColor = Drawing.Color.Red
                    e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_INVALID_TOOLTIP", "") 'Resources.TerminationWatcher.MESSAGE_TW_INVALID_TOOLTIP.ToString()	'"Invalid Record."
                    e.Row.Cells(13).Text = "Invalid record"
                ElseIf processed = "1" Then
                    e.Row.Cells(13).Text = "Processed"
                ElseIf processed = "0" Then
                    e.Row.Cells(13).Text = "Pending"
                End If

                If strLastcontributionReceived.ToUpper() = "YES" Then
                    If processed <> "2" Then
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTE_LASTCONTRI_TEXT", "")
                    End If
                End If
                If strUnfundedTransactions.ToUpper() = "YES" Then
                    If processed <> "2" Then
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTE_UNFUNDED_TEXT", "")
                    End If
                End If
                If strUnfundedTransactions.ToUpper() = "YES" AndAlso strLastcontributionReceived.ToUpper() = "YES" Then
                    If processed <> "2" Then
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTE_UNFUNDEDANDLASTCONTRI_TEXT", "")
                    End If
                End If

            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Purge Gridview Sorting
    Private Sub gvPurge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPurge.Sorting
        Dim l_dataset As DataSet
        Me.gvPurge.SelectedIndex = -1
        Dim dv As DataView
        Dim SortExpression As String
        Try
            If Not dsPurgeApplicant Is Nothing Then
                SortExpression = e.SortExpression
                dv = dsPurgeApplicant.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not ViewState("Add_Sort") Is Nothing Then
                    If ViewState("Add_Sort").ToString.Trim.EndsWith("DESC") Then
                        dv.Sort = SortExpression + " ASC"
                    Else
                        dv.Sort = SortExpression + " DESC"
                    End If
                Else
                    dv.Sort = SortExpression + " DESC"
                End If
                gvPurge.DataSource = dv
                gvPurge.DataBind()
                ViewState("Add_Sort") = dv.Sort()
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Processed Gridview RowDataBound
    Private Sub gvProcessed_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProcessed.RowDataBound
        Dim processed, strLastcontributionReceived, strUnfundedTransactions, strCreatedOn As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strLastcontributionReceived = Convert.ToString(drv("LastcontributionREceived"))
                strUnfundedTransactions = Convert.ToString(drv("UnfundedTransactions"))
                'Anudeep:19-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
                strCreatedOn = Convert.ToString(drv("CreatedDate"))
                If Request.QueryString("Action") <> "Invalid" Then
                    processed = Convert.ToString(drv("Processed"))
                    e.Row.Cells(13).Text = Convert.ToDateTime(strCreatedOn).Date
                    If processed = "2" Then
                        e.Row.ForeColor = Drawing.Color.Red
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_INVALID_TOOLTIP", "")
                        e.Row.Cells(12).Text = "Invalid record"
                    ElseIf processed = "1" Then
                        e.Row.Cells(12).Text = "Processed"
                    ElseIf processed = "0" Then
                        e.Row.Cells(12).Text = "Pending"
                    End If
                End If

                If strLastcontributionReceived.ToUpper() = "YES" Then
                    e.Row.Cells(7).Text = "Yes"
                    If processed <> "2" Then
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTE_LASTCONTRI_TEXT", "")
                    End If
                End If
                If strUnfundedTransactions.ToUpper() = "YES" Then
                    If processed <> "2" Then
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTE_UNFUNDED_TEXT", "")
                    End If
                End If
                If strUnfundedTransactions.ToUpper() = "YES" AndAlso strLastcontributionReceived.ToUpper() = "YES" Then
                    If processed <> "2" Then
                        e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTE_UNFUNDEDANDLASTCONTRI_TEXT", "")
                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Processed Gridview Sorting
    Private Sub gvProcessed_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvProcessed.Sorting
        Dim l_dataset As DataSet
        Me.gvProcessed.SelectedIndex = -1
        Dim dv As DataView
        Dim SortExpression As String
        Try
            If Not dsProcessedApplicant Is Nothing Then
                SortExpression = e.SortExpression
                dv = dsProcessedApplicant.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not ViewState("Add_Sort") Is Nothing Then
                    If ViewState("Add_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                gvProcessed.DataSource = dv
                gvProcessed.DataBind()
                ViewState("Add_Sort") = dv.Sort()
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim msg As String
        Try
            'Added by Anudeep : 30-10-2012 as per observations for tooltip shown after Purge close
            If Session("Delete") Then
                Session("Refresh") = "YES"
                msg = msg + "<Script Language='JavaScript'>"
                msg = msg + "self.close();"
                msg = msg + "window.opener.document.forms(0).submit();"
                msg = msg + "</Script>"
            Else
                msg = msg + "<Script Language='JavaScript'>"
                msg = msg + "self.close();"
                msg = msg + "</Script>"
            End If
            Response.Write(msg)
            'AA:23.12.2013 - BT:2311 added below line to clear the session while closing
            Session("Flag") = Nothing
        Catch
            Throw
        End Try
    End Sub

    'gets the message from Resource file
    Private Function messageFromResourceFile(ByVal strMessage As String, ByVal strParameter As String) As String
        Dim strReturnMessage As String = String.Empty
        Dim TW As Resources.TerminationWatcher
        Try
            If strParameter = String.Empty Then
                strReturnMessage = TW.ResourceManager.GetString(strMessage).Trim()
            End If
            Return strReturnMessage
        Catch
            Throw
        End Try
    End Function
End Class