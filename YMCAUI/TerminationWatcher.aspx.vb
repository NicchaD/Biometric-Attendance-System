'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	TerminationWatcher.aspx.vb
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
'Anudeep          06-nov-2012       Changes Made as Per Observations listed in bugtraker For yrs 5.0-1484 on 06-nov 2012
'Anudeep          14-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 14-nov-2012
'Anudeep          16-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 14-nov-2012
'Anudeep          22-nov-2012       Changes Made for Select All Check box must be not Enable only When all records in gridvew are invalid or Processed
'Anudeep          22-nov-2012       Code added for not minimizing the report frequently 
'Anudeep          26-nov-2012       Code added for if not exists the selected plan then add in dropdown
'Anudeep          12-Dec-2012       Changes made to show source of watcher
'Anudeep          09-Dec-2012       Bt-1545-YRS 5.0-1762:Color code lines in grid if unfunded transactions exist
'Anudeep          15-jan-2013       Bt-1545-YRS 5.0-1762: Added property to get background color for unfunded transaction exists
'Anudeep          11-oct-2013       Bt-957-YRS 5.0-1484:New Utility to replace Refund Watcher program
'Anudeep          16-dec-2013       BT:2311-13.3.0 Observations
'Anudeep          11-mar-2014       BT:957: YRS 5.0-1484:New Utility to replace Refund Watcher program
'Sanjay R.        04-Jul-2014       BT 2464/YRS 5.0-1484: Termination watcher changes(Re-Work).
'Manthan Rajguru  2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Imports System
Imports System.Data.SqlClient
Imports System.Resources
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text.RegularExpressions

Public Class TerminationWatcher

    Inherits System.Web.UI.Page
    'AA:16.12.2013 - BT:2311 added the checkbox array variable to get the count of checkbox selected 
    Dim checkBoxArray As ArrayList



#Region "Properties"

    Private Property struniqueid() As String
        Get
            Return (CType(ViewState("struniqueid"), String))
        End Get
        Set(ByVal Value As String)
            ViewState("struniqueid") = Value
        End Set
    End Property

    Private Property strSearchFundNo() As String
        Get
            If CType(Session("SearchFundNo"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchFundNo"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchFundNo") = Value
        End Set
    End Property
    Private Property strSearchSSno() As String
        Get
            If CType(Session("SearchSSno"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchSSno"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchSSno") = Value
        End Set
    End Property
    Private Property strSearchFirstName() As String
        Get
            If CType(Session("SearchFirstName"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchFirstName"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchFirstName") = Value
        End Set
    End Property
    Private Property strSearchLastName() As String
        Get
            If CType(Session("SearchLastName"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchLastName"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchLastName") = Value
        End Set
    End Property


    Private Property strSearchColumnFundNo() As String
        Get
            If CType(Session("SearchColumnFundNo"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchColumnFundNo"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchColumnFundNo") = Value
        End Set
    End Property
    Private Property strSearchColumnSSNo() As String
        Get
            If CType(Session("SearchColumnSSNo"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchColumnSSNo"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchColumnFundNo") = Value
        End Set
    End Property
    Private Property strSearchColumnFirstName() As String
        Get
            If CType(Session("SearchColumnFirstName"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchColumnFirstName"), String))
            End If
        End Get
        Set(ByVal Value As String)
            Session("SearchColumnFirstName") = Value
        End Set
    End Property
    Private Property strSearchColumnLastName() As String
        Get
            If CType(Session("SearchColumnLastName"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(Session("SearchColumnLastName"), String))
            End If


        End Get
        Set(ByVal Value As String)
            Session("SearchColumnLastName") = Value
        End Set
    End Property
    'Anudeep:15.01.2013 Added to get background color for unfunded transaction exists
    Private Property strBackgroundcolor() As String
        Get
            If CType(ViewState("strBackgroundcolor"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(ViewState("strBackgroundcolor"), String))
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("strBackgroundcolor") = Value
        End Set
    End Property


    Private Property dsStatusData() As DataSet
        Get
            Return (CType(Session("gvStatuslist"), DataSet))
        End Get
        Set(ByVal value As DataSet)
            Session("gvStatuslist") = value
        End Set
    End Property
    Private Property dtWithdrawalData() As DataTable
        Get
            Return (CType(Session("dtWithdrawallist"), DataTable))
        End Get
        Set(ByVal value As DataTable)
            Session("dtWithdrawallist") = value
        End Set
    End Property
    Private Property dtRetirementData() As DataTable
        Get
            Return (CType(Session("dtRetirementlist"), DataTable))
        End Get
        Set(ByVal value As DataTable)
            Session("dtRetirementlist") = value
        End Set
    End Property

    Private Property dsProccesedData() As DataSet
        Get
            Return (CType(Session("dtProccessed"), DataSet))
        End Get
        Set(ByVal value As DataSet)
            Session("dtProccessed") = value
        End Set
    End Property
    Private Property dsInvalidRecords() As DataSet
        Get
            Return (CType(Session("dsInvalidRecords"), DataSet))
        End Get
        Set(ByVal value As DataSet)
            Session("dsInvalidRecords") = value
        End Set
    End Property

    Private Property dvNotes() As DataView
        Get
            Return (CType(Session("dvNotes"), DataView))
        End Get
        Set(ByVal value As DataView)
            Session("dvNotes") = value
        End Set
    End Property
    'AA:20.03.2014 - BT:957: YRS 5.0-1484 - Commented below variable because filtering will be done on emp events not on fund events
    'AA:10.10.2013 :YRS 5.0-1484:Added to store fundevents list in viewstate 
    'Private Property dtMetaFundEvents() As DataTable
    '    Get
    '        Return (CType(ViewState("dvMetaFundEvents"), DataTable))
    '    End Get
    '    Set(ByVal value As DataTable)
    '        ViewState("dvMetaFundEvents") = value
    '    End Set
    'End Property


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Not IsPostBack Then
                Session("LastText") = Nothing
                Session("FundText") = Nothing
                Session("SSNText") = Nothing
                Session("FirstText") = Nothing
                Session("TW_CHECKED_ITEMS") = Nothing 'AA:11.03.2014 - BT:957: YRS 5.0-1484 Clearing session variable
                getSecuredControls()
                btnPurge.Attributes.Add("onclick", "javascript: OpenPopUpProcessPurge('Status','Purge'); return false;")
                'AA:10.10.2013 :YRS 5.0-1484:Commented below line for adding master page in termination watcher page
                'Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                'Menu1.DataBind()
                'Anudeep:15.01.2013 Added to get background color for unfunded transaction exists
                If strBackgroundcolor Is Nothing Or strBackgroundcolor = String.Empty Then
                    strBackgroundcolor = System.Configuration.ConfigurationSettings.AppSettings("TW_UnFunded_Color")
                End If
                'AA:10.10.2013 :YRS 5.0-1484:Moved below line to get TM records in termianted applicants tab when first time loaded
                lnkTMApllicant.Visible = False
                LoadTab()

                lbllnkTMApplicant.Visible = True
                lbllnkWithdrawalApplicant.Visible = False
                lbllnkRetirementApplicant.Visible = False
                tdTMApplicant.Style.Add("background-color", "#93BEEE")
                tdTMApplicant.Style.Add("color", "#000000")
                btnAdd.Attributes.Add("onclick", "javascript: OpenPopUp('All'); return false;")
                'AA:10.10.2013 :YRS 5.0-1484:Added to disable button add and hiding employement drop-down in filer search
                btnAdd.Enabled = False
                ddlEmployment.Visible = False
                lblEmployment.Visible = False
                'AA:11.03.2014 - BT:957: YRS 5.0-1484 
                '- Added to show process buttons to visible only in termainated applicants 
                '- Added to show purge buttons to hide in terminated applicants 
                btnProcess.Visible = True
                btnProcessAll.Visible = True
                btnPurge.Visible = False
                ddlWatcherType.Visible = True
                lblWatchertype.Visible = True
                lblUnfundedtransactions.Visible = False
                ddlUnFundedtransactions.Visible = False
                lblLastContributionRecieved.Visible = False
                ddlLastContributionRecieved.Visible = False
                'Start,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                btnYes.Attributes.Add("onclick", "javascript: closeDialog('ConfirmDialog')")
                btnNo.Attributes.Add("onclick", "javascript: closeDialog('ConfirmDialog')")
                btnOK.Attributes.Add("onclick", "javascript: closeDialog('ConfirmDialog')")
                btnCancel.Attributes.Add("onclick", "javascript: closeDialog('ConfirmDialog')")

                'End,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            End If
            If Session("Refresh") = "YES" Then

                If lnkTMApllicant.Visible = False Then
                    LoadTab()
                    StatusSearch()
                ElseIf lnkWithdrawal.Visible = False Then
                    dtWithdrawalData = Nothing
                    'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                    LoadData()
                    LoadWithdrawalTab()
                    StatusSearch()
                ElseIf lnkRetirement.Visible = False Then
                    dtRetirementData = Nothing
                    'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                    LoadData()
                    LoadRetirementTab()
                    StatusSearch()
                End If

                Session("Refresh") = Nothing
            End If

        Catch
            Throw
        End Try
    End Sub

    Private Sub LoadTab()
        Try
            LoadData()
            LoadStatusTab()
        Catch
            Throw
        End Try

    End Sub
    'It gets the data from db
    Private Sub LoadData()
        Try
            dsStatusData = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListPerson("")
            dsProccesedData = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListProcessedRecord("Status")
            dsInvalidRecords = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListInvalidRecord("Status")
            If HelperFunctions.isNonEmpty(dsStatusData.Tables(1)) Then
                dvNotes = dsStatusData.Tables(1).DefaultView
            End If
            'AA:20.03.2014 - BT:957: YRS 5.0-1484 Commented below code because filtering functionality is done on empstatus not on fund events status
            'AA:10.10.2013 :YRS 5.0-1484:Added to get the meta fundevents and store in a viewstate
            'If dsStatusData.Tables.Count > 1 AndAlso HelperFunctions.isNonEmpty(dsStatusData.Tables(2)) Then
            '    dtMetaFundEvents = dsStatusData.Tables(2)
            'End If
            Dim dvWithdrawal As New DataView(dsStatusData.Tables(0), "Type='Withdrawal' AND (([UnfundedTransactions] = 'Yes' Or [LastcontributionREceived] ='No')" + _
                                                 "OR  ([Emp Status] IN ('A','M','N') AND [UnfundedTransactions] = 'No' AND [LastcontributionREceived] ='Yes'))", "Processed", DataViewRowState.CurrentRows)
            dtWithdrawalData = dvWithdrawal.ToTable
            Dim dvRetirement As New DataView(dsStatusData.Tables(0), "Type='Retirement' AND (([UnfundedTransactions] = 'Yes' Or [LastcontributionREceived] ='No')" + _
                                                 "OR  ([Emp Status] IN ('A','M','N') AND [UnfundedTransactions] = 'No' AND [LastcontributionREceived] ='Yes'))", "Processed", DataViewRowState.CurrentRows)
            dtRetirementData = dvRetirement.ToTable

        Catch
            Throw
        End Try
    End Sub
    'It loads the status Tab
    Private Sub LoadStatusTab()
        Dim dsEmployee As DataSet
        Dim dataview As New DataView
        Dim sortstate As New GridViewCustomSort()
        Try
            If HelperFunctions.isEmpty(dsStatusData) Then
                LoadData()
            End If
            dsEmployee = dsStatusData
            If HelperFunctions.isNonEmpty(dsEmployee.Tables(0)) Then
                dataview = dsEmployee.Tables(0).DefaultView
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
                dataview.Sort = "CreatedDate Desc"
                'Anudeep:07-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
                sortstate.SortExpression = "CreatedDate"
                sortstate.SortDirection = "DESC"
                'ViewState("Sort") = dataview.Sort
                ViewState("Sort") = sortstate
                'AA:10.10.2013 :YRS 5.0-1484:Added to show only terminated records in terminated particiapants
                'dataview.RowFilter = "Status = 'TM'"
                'AA:20.03.2014 - BT:957: YRS 5.0-1484 terminated applicants will be shown only 
                ' if emp status is "t" and no unfunded transaction exists and lat contribution is received
                dataview.RowFilter = "[Emp Status] = 'T'"
                BindGrid(dataview, gvStatusList)
            Else
                BindGrid(Nothing, gvStatusList)

            End If

        Catch
            Throw
        End Try
    End Sub
    'Binds gridview 
    Private Sub BindGrid(ByVal dv As DataView, ByVal gv As GridView)
        Try
            gv.DataSource = dv
            gv.DataBind()

            'Added by Anudeep as per Observations 23-oct-2012 
            'shows how many record exists in grid
            If Not dv Is Nothing Then
                If dv.Count <> 0 Then
                    lblTotalCount.Text = messageFromResourceFile("MESSAGE_TW_SHOW_TOTAL_RECORDS", "") + " " + dv.Count.ToString()
                    LabelStatusNoDataFound.Visible = False
                Else
                    lblTotalCount.Text = ""
                    LabelStatusNoDataFound.Visible = True
                End If
            Else
                lblTotalCount.Text = ""
                LabelStatusNoDataFound.Visible = True
            End If

        Catch
            Throw
        End Try
    End Sub
    'Loads Withdrawal Tab
    Private Sub LoadWithdrawalTab()
        Dim sortstate As New GridViewCustomSort()
        Try
            If HelperFunctions.isEmpty(dsStatusData) Then
                LoadData()
            End If

            If HelperFunctions.isNonEmpty(dtWithdrawalData) Then
                Dim dataview As New DataView
                dataview = dtWithdrawalData.DefaultView
                dataview.Sort = "CreatedDate Desc"
                sortstate.SortExpression = "CreatedDate"
                sortstate.SortDirection = "DESC"
                ViewState("Sort") = sortstate
                BindGrid(dataview, gvStatusList)
            Else
                BindGrid(Nothing, gvStatusList)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Loads retirement Tab
    Private Sub LoadRetirementTab()
        Dim sortstate As New GridViewCustomSort()
        Try

            If HelperFunctions.isEmpty(dsStatusData) Then
                LoadData()
            End If

            If HelperFunctions.isNonEmpty(dtRetirementData) Then
                Dim dataview As New DataView
                dataview = dtRetirementData.DefaultView
                dataview.Sort = "CreatedDate Desc"
                sortstate.SortExpression = "CreatedDate"
                sortstate.SortDirection = "DESC"
                ViewState("Sort") = sortstate
                BindGrid(dataview, gvStatusList)
            Else
                BindGrid(Nothing, gvStatusList)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Updates the  selected Record
    Private Function UpdateRecord(ByVal strUniqueId As String, ByVal strPlanType As String)
        Dim strResult As String
        Try
            If (strUniqueId <> String.Empty AndAlso strPlanType <> String.Empty) Then
                strResult = YMCARET.YmcaBusinessObject.TerminationWatcherBO.UpdateTerminationWatcherData(strUniqueId, strPlanType)
            End If

            If strResult = "-2" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_RECORD_NOT_EXISTS", "")
            ElseIf strResult = "-3" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_ADDUSER_RECORD_EXISTS", strPlanType)
            ElseIf strResult = "-1" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_UPDATE_ERROR", "")
            ElseIf strResult = "1" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_UPDATE_SUCESS", "")
            End If
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)

        Catch
            Throw
        End Try
    End Function

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim dtEmployee As DataTable
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)

            gvStatusList.EditIndex = -1
            Session("TW_CHECKED_ITEMS") = Nothing
            'AA:16.12.2013 - BT:2311 added for to get the continue the search or change the tab on okay click
            If ViewState("Search") IsNot Nothing Then
                StatusSearch()
                ViewState("Search") = Nothing
                Exit Sub
            End If

            If ViewState("tabchange") IsNot Nothing Then
                If ViewState("tabchange") = "Retirement" Then
                    lnkRetirement_Click(sender, e)
                End If
                If ViewState("tabchange") = "Terminated" Then
                    lnkTMApllicant_Click(sender, e)
                End If
                If ViewState("tabchange") = "Withdarwal" Then
                    lnkWithdrawal_Click(sender, e)
                End If

                ViewState("tabchange") = Nothing
                Exit Sub
            End If


            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            dtEmployee = getDataTable()
            dv = dtEmployee.DefaultView
            If Not ViewState("Sort") Is Nothing Then
                'dv.Sort() = ViewState("Sort")
                Sorting = ViewState("Sort")
                dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            If Not ViewState("PageIndex") Is Nothing Then
                Me.gvStatusList.PageIndex = ViewState("PageIndex")
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If

            BindGrid(dv, gvStatusList)

        Catch
            Throw
        End Try
    End Sub
    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Dim dtEmployee As DataTable
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)

            dtEmployee = getDataTable()
            dv = dtEmployee.DefaultView
            If Not ViewState("Sort") Is Nothing Then
                'dv.Sort() = ViewState("Sort")
                Sorting = ViewState("Sort")
                dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            If Not ViewState("PageIndex") Is Nothing Then
                Me.gvStatusList.PageIndex = ViewState("PageIndex")
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If

            BindGrid(dv, gvStatusList)
            Session("TW_CHECKED_ITEMS") = Nothing
            Session("Process") = Nothing
            Session("Flag") = Nothing
        Catch
            Throw
        End Try
    End Sub
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Dim dtEmployee As DataTable
        Dim dv As DataView
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)

            If Session("Flag") <> Nothing Then

                If Session("Flag") = "Delete" Then
                    Session("Flag") = Nothing
                    DeleteRecord(struniqueid)
                End If

                If Session("Flag") = "Process" Then
                    Session("Flag") = Nothing
                    'Start Anudeep:06-nov-2012 Changes made For Process Report
                    Dim Strings(0 To 1) As String
                    Dim strReportname As String
                    Dim strProcessid As String
                    Dim sReportPath As String
                    Dim TWRPT As IO.File

                    Strings = ProcessRecords()
                    strProcessid = Strings(1)
                    strReportname = GetReportName()
                    sReportPath = ConfigurationSettings.AppSettings("ReportPath")
                    sReportPath = sReportPath.Trim + "\\" + strReportname + ".rpt"
                    'Checking whether Report exists in Folder, If exists open Report else open popup
                    If TWRPT.Exists(sReportPath) Then
                        Session("strReportName") = strReportname
                        Session("intProcessID") = strProcessid
                        OpenReportViewer()
                    Else
                        If Strings(0) = "1" Then
                            Dim str As String = String.Empty
                            'Added by Anudeep : 30-10-2012 as per observations for No maximize window button in pop ups
                            str = str + "var newwindow = window.open('TerminationWatcherProcessPurge.aspx?form=Status&Action=Process&Flag=Process', 'YMCAYRS', 'width=800, height=600, menubar=no, resizable=no,top=120,left=120, scrollbars=yes', ''); "
                            'Anudeep:16-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                            str = str + " if (window.focus) {newwindow.focus()}"
                            str = str + " if (!newwindow.closed) {newwindow.focus()}"

                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "openWin", str, True)
                        End If
                    End If
                    'End Anudeep:06-nov-2012 Changes made For Process Report
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Delete The record
    Private Sub DeleteRecord(ByVal strUniqueId As String)
        Dim strResult As String
        Try
            If (strUniqueId <> String.Empty) Then
                strResult = YMCARET.YmcaBusinessObject.TerminationWatcherBO.DeleteTerminationWatcherData(strUniqueId)
            End If

            If strResult = "-2" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_RECORD_NOT_EXISTS", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
            ElseIf strResult = "-1" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_DELETE_ERROR", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)

            ElseIf strResult = "1" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_DELETE_SUCESS", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
                'Anudeep:06-nov-2012 Changes made For Process Report
                LoadData()
                StatusSearch()
            End If
        Catch
            Throw
        End Try
    End Sub
    'It handles Data Bounds to each row
    Private Sub gvStatusList_OnRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStatusList.RowDataBound
        Dim terminationWatcherID, strFundNo, strPersID, strprocessed, strfundEventId, strLastcontributionReceived, strUnfundedTransactions, strNotesCreatedBy, strNotesCreatedOn, strCreatedOn, strCreatedBy, strPlantype As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Try
            'AA:BT:957: YRS.5.0 1484 - Added the code to show the sorting arrows

            If e.Row.RowType = DataControlRowType.DataRow Then

                strPersID = Convert.ToString(drv("guiPersID"))
                strfundEventId = Convert.ToString(drv("guiFundEventId"))
                terminationWatcherID = Convert.ToString(drv("UniqueId"))
                strprocessed = Convert.ToString(drv("Processed"))
                strLastcontributionReceived = Convert.ToString(drv("LastcontributionREceived"))
                strUnfundedTransactions = Convert.ToString(drv("UnfundedTransactions"))
                strFundNo = Convert.ToString(drv("FundNo"))

                Dim chk As CheckBox
                chk = DirectCast(e.Row.FindControl("chkSelect"), CheckBox)
                Dim chkall As CheckBox
                chkall = DirectCast(gvStatusList.HeaderRow.FindControl("chkSelectAll"), CheckBox)
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                e.Row.Cells(10).Text = Convert.ToString(drv("LastName")) + ", " + Convert.ToString(drv("FirstName"))
                strCreatedOn = Convert.ToString(drv("CreatedDate"))
                strCreatedBy = Convert.ToString(drv("Creator"))
                'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                strPlantype = Convert.ToString(drv("PlanType"))
                Dim strTooltip As String
                'Anudeep:14-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                'Anudeep:07-nov-2013 Commented below line as it is handling to display in date format in aspx page
                'e.Row.Cells(18).Text = Convert.ToDateTime(strCreatedOn).Date
                ' Assign tooltip for invald
                'Anudeep:12.12.2012 Changes made to show source of watcher
                If Convert.ToString(drv("Source")) = "PERSON" Then
                    e.Row.Cells(19).Text = "Person"
                End If

                If strprocessed = "2" Then
                    e.Row.ForeColor = Drawing.Color.Red
                    strTooltip = messageFromResourceFile("MESSAGE_TW_INVALID_TOOLTIP", "")

                    If chkInvalidRecord.Checked Then
                        e.Row.Visible = True
                    Else
                        e.Row.Visible = False
                    End If

                    'e.Row.Cells(17).Text = "Invalid Record"
                    e.Row.Cells(17).Text = "Invalid" 'Anudeep 11.01.2013 Changed text from Invalid record to Invalid

                    'Showing latest Note in the remark Column
                    For i = 0 To dvNotes.Count - 1
                        If (dvNotes.Item(i).Row(0).ToString() = terminationWatcherID) Then
                            'Anudeep:14-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                            'AA:11.03.2014 - BT:957: YRS 5.0-1484 -Changed to show the remarks coumn correctly.
                            e.Row.Cells(22).Text = dvNotes.Item(i).Row(3).ToString()
                            e.Row.Cells(22).Width = 300
                        End If
                    Next
                    If Not IsNothing(chk) Then
                        chk.Enabled = False
                        chk.ID = "InvalidChk"
                        chk.Checked = False
                        e.Row.Cells(5).Enabled = False
                    End If
                    If Not IsNothing(chkall) Then
                        chkall.Enabled = False
                        'Anudeep:22-11-2012 Changes Made for Select All Check box must be not Enable only When all records in gridvew are invalid or Processed
                        'Removed Checkbox all Enabling Because it is handled in down


                    End If
                ElseIf strprocessed = "0" Then

                    e.Row.Cells(17).Text = "Pending"
                ElseIf strprocessed = "1" Then
                    If Not IsNothing(chk) Then
                        chk.Enabled = False
                        chk.ID = "Processed"
                        chk.Checked = False
                        e.Row.Cells(5).Enabled = False
                    End If
                    If Not IsNothing(chkall) Then
                        chkall.Enabled = False
                        'Anudeep:22-11-2012 Changes Made for Select All Check box must be not Enable only When all records in gridvew are invalid or Processed
                        'Removed Checkbox all Enabling Because it is handled in down

                    End If
                    If chkProccessedRecords.Checked Then
                        e.Row.Visible = True
                    Else
                        e.Row.Visible = False
                    End If

                    e.Row.Cells(17).Text = "Processed"
                End If
                'Start,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                Dim lblPlantype As Label = DirectCast(e.Row.FindControl("lblPlantype"), Label)
                If strPlantype.ToUpper = "RETIREMENT" Then
                    lblPlantype.Text = "RET"
                ElseIf strPlantype.ToUpper = "SAVINGS" Then
                    lblPlantype.Text = "SAV"
                End If
                'END,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                ' assign tooltip for unfunded trans and last contri recieved
                If strLastcontributionReceived.ToUpper() = "YES" Then
                    'e.Row.Cells(12).Text = "Yes"
                    If strprocessed <> "2" Then
                        strTooltip = messageFromResourceFile("MESSAGE_TW_NOTE_LASTCONTRI_TEXT", "")
                    End If

                    'ElseIf strLastcontributionReceived = "NO " Then
                    ' e.Row.Cells(12).Text = "No"
                End If
                If strUnfundedTransactions.ToUpper() = "YES" Then

                    'e.Row.Cells(13).Text = "Yes"
                    If strprocessed <> "2" Then
                        strTooltip = messageFromResourceFile("MESSAGE_TW_NOTE_UNFUNDED_TEXT", "")
                    End If
                    'Anudeep:09.01.2013 Bt-1545:YRS 5.0-1762:Color code lines in grid if unfunded transactions exist
                    lbltdUnfunded.Text = "Unfunded Transaction Exists"

                    'If no color defined in config file then setting a default color
                    If strBackgroundcolor Is Nothing Or strBackgroundcolor = String.Empty Then
                        strBackgroundcolor = Drawing.Color.LightYellow.Name
                    End If
                    e.Row.Style.Item("background-color") = strBackgroundcolor
                    tdUnfunded.Style.Item("background-color") = strBackgroundcolor
                    'ElseIf strUnfundedTransactions = "NO " Then
                    'e.Row.Cells(13).Text = "No"
                End If

                If strUnfundedTransactions.ToUpper() = "YES" AndAlso strLastcontributionReceived.ToUpper() = "YES" Then
                    If strprocessed <> "2" Then
                        strTooltip = messageFromResourceFile("MESSAGE_TW_NOTE_UNFUNDEDANDLASTCONTRI_TEXT", "")
                    End If
                End If

                'Notes icon
                Dim img As New ImageButton
                img = DirectCast(e.Row.FindControl("imgButtonNote"), ImageButton)
                If Not IsNothing(img) Then
                    'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                    img.Attributes.Add("onClick", "javascrpt:OpenNotesWindow( '" + terminationWatcherID + "','" + strPersID + "'); return false;")
                End If

                'assigning latest notes in tooltip
                Dim strNotes As String
                If HelperFunctions.isNonEmpty(dvNotes) Then
                    For i = 0 To dvNotes.Count - 1
                        If (dvNotes.Item(i).Row(0).ToString() = terminationWatcherID) Then
                            strNotes = "Notes: " + dvNotes.Item(i).Row(3).ToString().Trim()
                            strNotes = strNotes.Replace(vbNewLine, "\n")
                            strNotes = strNotes.Replace(Environment.NewLine, "\n")
                            strNotesCreatedBy = Convert.ToString(dvNotes.Item(i).Row(2))
                            strNotesCreatedOn = Convert.ToString(Convert.ToDateTime((dvNotes.Item(i).Row(1))))
                            strNotes = strNotes + "(Created By: " + strNotesCreatedBy + " On " + strNotesCreatedOn + ")"

                        End If
                    Next
                End If
                ' adding tooltip to div tag
                'AA:10.10.2013 :YRS 5.0-1484:Added to access the Tooltip div in master page
                'Dim addToolTipDiv As HtmlContainerControl = DirectCast(FindControl("Tooltip"), HtmlContainerControl)
                Dim addToolTipDiv As HtmlContainerControl = Tooltip
                ' assighning tooltip of div to each row
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                For i As Integer = 7 To e.Row.Cells.Count - 1
                    If (strTooltip <> "") Then
                        If (Not strNotes Is Nothing) Then
                            e.Row.Cells(i).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + strTooltip + "','" + strNotes + "','" + strCreatedOn + "','" + strCreatedBy + "','" + strFundNo + "');")
                            e.Row.Cells(i).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                        Else
                            e.Row.Cells(i).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + strTooltip + "','" + "" + "','" + strCreatedOn + "','" + strCreatedBy + "','" + strFundNo + "');")
                            e.Row.Cells(i).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                        End If
                    Else
                        If (Not strNotes Is Nothing) Then
                            e.Row.Cells(i).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + "" + "','" + strNotes + "','" + strCreatedOn + "','" + strCreatedBy + "','" + strFundNo + "');")
                            e.Row.Cells(i).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                        Else
                            e.Row.Cells(i).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + "" + "','" + "" + "','" + strCreatedOn + "','" + strCreatedBy + "','" + strFundNo + "');")
                            e.Row.Cells(i).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                        End If
                    End If
                Next
                'AA:10.10.2013 :YRS 5.0-1484:Added Removing edit,delete and notes icons from grid
                If Not lnkTMApllicant.Visible Then
                    e.Row.Cells(4).Visible = False
                    e.Row.Cells(5).Visible = False
                    e.Row.Cells(6).Visible = False
                Else
                    e.Row.Cells(3).Visible = False ' - 'AA:11.03.2014 - BT:957: YRS 5.0-1484 - Added to hide checkbox for withdrawal and retirement applicants.
                End If

                If gvStatusList.EditIndex = e.Row.RowIndex Then
                    'to overlook header row
                    'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                    Dim strlblPlantype As String = DirectCast(e.Row.FindControl("lblPlantype"), Label).Text
                    Dim dsPlantType As New DataSet
                    Dim dropDown As New DropDownList
                    If Not IsNothing(strlblPlantype) Then
                        'Start,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                        If strlblPlantype.ToUpper = "RET" Then
                            strlblPlantype = "RETIREMENT"
                        ElseIf strlblPlantype.ToUpper = "SAV" Then
                            strlblPlantype = "SAVINGS"
                        End If
                        'END,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                        dropDown = DirectCast(e.Row.FindControl("drdPlanType"), DropDownList)
                        If Not IsNothing(dropDown) Then
                            Dim dsApplicantPlantype As DataSet
                            dsApplicantPlantype = YMCARET.YmcaBusinessObject.TerminationWatcherBO.GetApplicantPlanType(strfundEventId)
                            If HelperFunctions.isNonEmpty(dsApplicantPlantype) Then
                                dropDown.DataSource = dsApplicantPlantype
                                dropDown.DataTextField = "PlanType"
                                dropDown.DataValueField = "PlanType".ToUpper()
                                dropDown.DataBind()
                                If dsApplicantPlantype.Tables(0).Rows.Count = 2 Then
                                    dropDown.Items.Insert(2, New ListItem("BOTH", "BOTH"))
                                End If
                            End If

                            'Start:Anudeep:26-11-2012 - Added the code for if not exists the selected plan then add in dropdown
                            If dropDown.Items.FindByValue(strlblPlantype) Is Nothing Then
                                dropDown.Items.Insert(dropDown.Items.Count, New ListItem(strlblPlantype, strlblPlantype))
                            End If
                            'End:Anudeep:26-11-2012 - Added the code for if not exists the selected plan then add in dropdown

                            dropDown.SelectedValue = strlblPlantype
                        End If
                    End If
                End If
                'Anudeep:22-11-2012 Changes Made for Select All Check box must be not Enable only When all records in gridvew are invalid or Processed

                'Checks If all the Rows in gridview only Invalid or Processed then check all is disabled else always enabled

                chkall.Enabled = False
                For i As Integer = 0 To gvStatusList.Rows.Count - 1
                    If (gvStatusList.Rows(i).Cells(17).Text = "Pending") Then
                        chkall.Enabled = True
                    End If
                Next

            ElseIf e.Row.RowType = DataControlRowType.Header Then
                'AA:10.10.2013 :YRS 5.0-1484:Added Removing edit,delete and notes icons from grid
                If Not lnkTMApllicant.Visible Then
                    e.Row.Cells(4).Visible = False
                    e.Row.Cells(5).Visible = False
                    e.Row.Cells(6).Visible = False
                Else
                    e.Row.Cells(3).Visible = False ' - 'AA:11.03.2014 - BT:957: YRS 5.0-1484 - Added to hide checkbox for withdrawal and retirement applicants.
                End If
                HelperFunctions.SetSortingArrows(ViewState("Sort"), e)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Process the selected records
    Private Function ProcessRecords() As String()
        Dim strResult As String = String.Empty
        Dim Strings(0 To 1) As String
        Dim LTerminationIds As New List(Of String)
        Dim dtEmployee As DataTable
        Try
            'AA:20.03.2014 - BT:957: YRS 5.0-1484 Changed functionality 
            'if process all button is clicked then all the records will be processed and 
            'if process selected button is clicked then selected records wil be procesed
            If Session("Process") IsNot Nothing Then
                If Session("Process") = "All" Then
                    LTerminationIds = GetAllRecordsId()
                ElseIf Session("Process") = "Selected" Then
                    LTerminationIds = GetSelectedRecordIds()
                End If
            End If
            Session("Process") = Nothing
            If LTerminationIds.Count > 0 Then
                Strings = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ProcessTerminationWatcherData(LTerminationIds)
                strResult = Strings(0)
            End If

            If strResult = "-1" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_RECORD_NOT_EXISTS", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
            ElseIf strResult = "-2" Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PROCESS_ERROR", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
            ElseIf strResult = "1" Then
                Session("LTerminationIds") = LTerminationIds
                'Anudeep:06-nov-2012 Changes made For Process Report
                LoadData()
                StatusSearch()
            End If

            Return Strings
        Catch
            Throw
        End Try
    End Function
    ''Gets the selected Records from grid
    Private Function GetSelectedRecordIds() As List(Of String)
        Dim LTerminationIds As New List(Of String)
        Dim chk As New CheckBox
        Dim userdetails As New ArrayList()
        Try
            'AA:20.03.2014 - BT:957: YRS 5.0-1484 Changed functionality 
            'if process selected button is clicked then selected records wil be procesed which are stored in session

            'AA:11.03.2014 - BT:957: YRS 5.0-1484 - Changed the logic to get al the records which are selected in all pages.
            'Dim i As Integer = 0
            'For i = 0 To gv.Rows.Count - 1
            '    chk = gv.Rows(i).FindControl("chkSelect")
            '    If Not IsNothing(chk) Then
            '        If chk.Checked = True Then
            '            LTerminationIds.Add(gv.DataKeys(i).Value.ToString())
            '        End If
            '    End If
            'Next
            SaveCheckedValues()
            If Session("TW_CHECKED_ITEMS") IsNot Nothing Then
                userdetails = DirectCast(Session("TW_CHECKED_ITEMS"), ArrayList)
            End If
            For iCount As Integer = 0 To userdetails.Count - 1
                LTerminationIds.Add(userdetails(iCount))
            Next
            Return LTerminationIds
        Catch
            Throw
        End Try
    End Function

    Private Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
                Exit Sub
            End If

            Dim LTerminationIds As New List(Of String)
            LTerminationIds = GetSelectedRecordIds()

            If (LTerminationIds.Count = 0) Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_SELECT_RECORD", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
            Else
                Session("Flag") = "Process"
                'AA:20.03.2014 - BT:957: YRS 5.0-1484 Changed functionality 
                'if process selected button is clicked then selected records will be procesed 
                Session("Process") = "Selected"
                lblMessage.Text = String.Format(messageFromResourceFile("MESSAGE_TW_PROCESS_CONFIRMATION", ""), LTerminationIds.Count)
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True)

            End If
            'AA:10.10.2013 :YRS 5.0-1484:added below line to clear the dirty flag
            SetOrClearDirtyFlag(String.Empty)
        Catch
            Throw
        End Try
    End Sub
    'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    Private Sub txtStatusFundNo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStatusFundNo.PreRender
        Try
            'AA:10.10.2013 :YRS 5.0-1484:To set the search button as default button to get access in master page
            Me.Form.DefaultButton = btnSearch.UniqueID
        Catch
            Throw
        End Try
    End Sub
    'It handles when fundNo is entered in the text Box
    Protected Sub txtserachFundNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtStatusFundNo.TextChanged
        Try
            Session("SearchLastName") = Nothing
            Session("SearchFirstName") = Nothing
            Session("FundNO") = "YES"
            Dim text As String = DirectCast(sender, TextBox).Text
            strSearchFundNo = text
            strSearchColumnFundNo = "FundNo"
            btnSearch.UseSubmitBehavior = True
        Catch
            Throw
        End Try
    End Sub
    'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    'AA:10.10.2013 :YRS 5.0-1484:Commented below line because default button will be set once
    'Private Sub txtStatusFirstName_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStatusFirstName.PreRender
    '    Try
    'Page.Form.DefaultButton = "btnSearch"
    '    Catch
    '        Throw
    '    End Try

    'End Sub

    Protected Sub txtserachFirstName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtStatusFirstName.TextChanged
        Try
            Session("SearchLastName") = Nothing
            Session("SearchFundNo") = Nothing
            Session("FirstName") = "YES"
            Dim text As String = DirectCast(sender, TextBox).Text
            strSearchFirstName = text
            strSearchColumnFirstName = "FirstName"
            btnSearch.UseSubmitBehavior = True
        Catch
            Throw
        End Try
    End Sub
    'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    'AA:10.10.2013 :YRS 5.0-1484:Commented below line because default button will be set once
    'Private Sub txtStatusLastName_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStatusLastName.PreRender
    'Page.Form.DefaultButton = "btnSearch"
    'End Sub
    'It handles when LastName is entered in the text Box
    Protected Sub txtserachLastName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtStatusLastName.TextChanged
        Try
            Session("SearchFirstName") = Nothing
            Session("SearchFundNo") = Nothing
            Session("LastName") = "YES"
            Dim text As String = DirectCast(sender, TextBox).Text
            strSearchLastName = text
            strSearchColumnLastName = "LastName"
            btnSearch.UseSubmitBehavior = True
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnRefresh", Convert.ToInt32(Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
                Exit Sub
            End If
            clearSession()

            If lnkTMApllicant.Visible = False Then
                LoadTab()
            ElseIf lnkWithdrawal.Visible = False Then
                dtWithdrawalData = Nothing
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                LoadData()
                LoadWithdrawalTab()
            ElseIf lnkRetirement.Visible = False Then
                dtRetirementData = Nothing
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                LoadData()
                LoadRetirementTab()
            End If
            'AA:10.10.2013 :YRS 5.0-1484:Commented below line and added to a line to get all textbox controls in masterpage
            'cleartextboxes(Form1)
            cleartextboxes(Master.FindControl("Form1"))
        Catch
            Throw
        End Try
    End Sub
    'Clears the Session variables 
    Private Sub clearSession()
        Try
            Session("LastName") = Nothing
            Session("FundNO") = Nothing
            Session("FirstName") = Nothing
            Session("SearchColumnLastName") = Nothing
            Session("SearchColumnFirstName") = Nothing
            Session("SearchColumnFundNo") = Nothing
            Session("SearchLastName") = Nothing
            Session("SearchFirstName") = Nothing
            Session("SearchSSno") = Nothing
            Session("SearchFundNo") = Nothing
            Session("LTerminationIds") = Nothing
            Session("Flag") = Nothing
            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            ViewState("Sort") = Nothing
            ViewState("FilterationString") = Nothing
            'AA:16.12.2013 - BT:2311 Added to clear the twrmination watcher checked items
            Session("TW_CHECKED_ITEMS") = Nothing
            'AA:10.10.2013 :YRS 5.0-1484:added below line to clear the dirty flag
            SetOrClearDirtyFlag(String.Empty)
        Catch
            Throw
        End Try

    End Sub
    'get the secured controls
    Public Sub getSecuredControls()

        Dim l_Int_UserId As Integer
        Dim l_String_FormName As String
        Dim l_control_Id As Integer
        Dim ds_AllsecItems As DataSet
        Dim l_int_access As Integer
        Dim l_string_controlNames As String
        Dim l_ds_ctrlNames As DataSet
        Dim l_intLoggedUser As Integer
        Try
            l_string_controlNames = ""
            l_Int_UserId = Convert.ToInt32(Session("LoggedUserKey"))
            l_String_FormName = Session("FormName").ToString().Trim()

            'To Find if the user belongs to the Notes Admin Group so that all the checkboxes in the notes grid be enabled

            l_intLoggedUser = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.GetLoginNotesUser(l_Int_UserId)

            ds_AllsecItems = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetSecuredControlsOnForm(l_String_FormName)
            If Not ds_AllsecItems Is Nothing Then
                If Not ds_AllsecItems.Tables(0).Rows.Count = 0 Then
                    For Each dr As DataRow In ds_AllsecItems.Tables(0).Rows
                        l_control_Id = Convert.ToInt32(dr("sfc_ControlId"))
                        l_ds_ctrlNames = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetControlNames(l_control_Id)
                        l_int_access = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(l_Int_UserId, l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim())
                        If l_int_access = 0 Or l_int_access = 1 Then
                            l_string_controlNames = l_string_controlNames + l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim() + ","
                        End If
                    Next
                    Me.HiddenSecControlName.Value = l_string_controlNames
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            clearSession()
            Response.Redirect("MainWebForm.aspx", False)
        Catch
            Throw
        End Try
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If Regex.IsMatch(txtStatusFundNo.Text, "^[a-zA-Z]") Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_VALIDATION_FUNDNO", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
                Exit Sub
            Else
                'AA:16.12.2013 - BT:2311 Added to show confirmation check the validation for any changes exists
                'AA:20.03.2014 - BT:957: YRS 5.0-1484 Changed to show the message when changes exists                
                If GetSelectedRecordIds().Count > 1 Or gvStatusList.EditIndex <> -1 Then
                    lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
                    ViewState("Search") = True
                Else
                    gvStatusList.EditIndex = -1
                    StatusSearch()
                    btnSearch.UseSubmitBehavior = True
                End If
            End If
        Catch
            Throw
        End Try

    End Sub
    'Search the grid according to given requirement
    Private Sub StatusSearch()
        Dim dtEmployee As New DataTable
        Dim strString As String = String.Empty
        Dim dataview As New DataView
        Dim strFundEnentStatus As String = String.Empty
        Dim Sorting As GridViewCustomSort
        'Dim dvMetaFundEvents As New DataView
        Try
            dtEmployee = getDataTable()
            If HelperFunctions.isNonEmpty(dtEmployee) Then
                dataview = dtEmployee.DefaultView
                dataview.RowFilter = String.Empty
                'Changed by ANudeep As per observations on  31-10-2012 for empty string
                If (txtStatusFundNo.Text.Trim() <> "") Then
                    strString = "FundNo" + " = " + txtStatusFundNo.Text.Trim()
                End If
                'Changed by ANudeep As per observations on  31-10-2012 for empty string
                If ((txtStatusFirstName.Text.Trim() <> String.Empty) And (strString = String.Empty)) Then
                    strString = strString + "FirstName LIKE '" + txtStatusFirstName.Text.Trim() + "%'"
                ElseIf (strString <> String.Empty) Then
                    strString = strString + " AND FirstName LIKE '" + txtStatusFirstName.Text.Trim() + "%'"
                End If
                'Changed by ANudeep As per observations on  31-10-2012 for empty string
                If ((txtStatusLastName.Text.Trim() <> String.Empty) And (strString = String.Empty)) Then
                    strString = strString + "LastName LIKE '" + txtStatusLastName.Text.Trim() + "%'"
                ElseIf (strString <> String.Empty) Then
                    strString = strString + " AND LastName LIKE '" + txtStatusLastName.Text.Trim() + "%'"
                End If

                'Start:AA:20.03.2014 - BT:957: YRS 5.0-1484 Commented below code because filtering functionality is done on empstatus not on fund events status
                'Start:AA:10.10.2013 :YRS 5.0-1484:Added to get the drop-down selected fund events records in grid
                If ((ddlEmployment.SelectedValue = "IAE") And (strString = String.Empty)) Then

                    'dvMetaFundEvents = dtMetaFundEvents.DefaultView
                    'dvMetaFundEvents.RowFilter = "bitService = 0"
                    'For i As Integer = 0 To dvMetaFundEvents.Count - 1
                    '    strFundEnentStatus += "'" + dvMetaFundEvents.Item(i).Row(0).ToString().Trim + "',"
                    'Next
                    'If strFundEnentStatus.Length > 0 Then
                    '    strFundEnentStatus = strFundEnentStatus.Remove(strFundEnentStatus.Length - 1, 1)
                    'End If

                    strString = strString + "[Emp Status] IN ('T','M','N')"
                ElseIf ((ddlEmployment.SelectedValue = "IAE") And (strString <> String.Empty)) Then

                    'dvMetaFundEvents = dtMetaFundEvents.DefaultView
                    'dvMetaFundEvents.RowFilter = "bitService = 0"
                    'For i As Integer = 0 To dvMetaFundEvents.Count - 1
                    '    strFundEnentStatus += "'" + dvMetaFundEvents.Item(i).Row(0).ToString().Trim + "',"
                    'Next
                    'If strFundEnentStatus.Length > 0 Then
                    '    strFundEnentStatus = strFundEnentStatus.Remove(strFundEnentStatus.Length - 1, 1)
                    'End If

                    strString = strString + " AND [Emp Status] IN ('T','M','N')"
                End If

                If ((ddlEmployment.SelectedValue = "AE") And (strString = String.Empty)) Then
                    'dvMetaFundEvents = dtMetaFundEvents.DefaultView
                    'dvMetaFundEvents.RowFilter = "bitService = 1"
                    'For i As Integer = 0 To dvMetaFundEvents.Count - 1
                    '    strFundEnentStatus += "'" + dvMetaFundEvents.Item(i).Row(0).ToString().Trim + "',"
                    'Next
                    'If strFundEnentStatus.Length > 0 Then
                    '    strFundEnentStatus = strFundEnentStatus.Remove(strFundEnentStatus.Length - 1, 1)
                    'End If

                    strString = strString + "[Emp Status] = 'A'"
                ElseIf ((ddlEmployment.SelectedValue = "AE") And (strString <> String.Empty)) Then
                    'dvMetaFundEvents = dtMetaFundEvents.DefaultView
                    'dvMetaFundEvents.RowFilter = "bitService = 1"
                    'For i As Integer = 0 To dvMetaFundEvents.Count - 1
                    '    strFundEnentStatus += "'" + dvMetaFundEvents.Item(i).Row(0).ToString().Trim + "',"
                    'Next
                    'If strFundEnentStatus.Length > 0 Then
                    '    strFundEnentStatus = strFundEnentStatus.Remove(strFundEnentStatus.Length - 1, 1)
                    'End If

                    strString = strString + " AND [Emp Status] = 'A'"
                End If
                'Start:AA:10.10.2013 :YRS 5.0-1484:Added to get the drop-down selected fund events records in grid
                'End:AA:20.03.2014 - BT:957: YRS 5.0-1484 Commented below code because filtering functionality is done on empstatus not on fund events status


                'Start: AA:20.03.2014 - BT:957: YRS 5.0-1484 added below code to filter terimnated applicants with watcher type
                If lbllnkTMApplicant.Visible = True And ddlWatcherType.SelectedValue <> "All" Then
                    If strString = String.Empty Then
                        strString = "Type ='" + ddlWatcherType.SelectedValue + "'"
                    ElseIf strString <> String.Empty Then
                        strString += " AND Type ='" + ddlWatcherType.SelectedValue + "'"
                    End If
                End If
                'End: AA:20.03.2014 - BT:957: YRS 5.0-1484 added below code to filter terimnated applicants with watcher type

                'Start: AA:20.03.2014 - BT:957: YRS 5.0-1484 added below code to filter withdrawal / retirement applicants with unfunded transactions and last contributions
                If (lbllnkWithdrawalApplicant.Visible = True Or lbllnkRetirementApplicant.Visible = True) And ddlUnFundedtransactions.SelectedValue <> "All" Then
                    If strString = String.Empty Then
                        strString = "UnfundedTransactions ='" + ddlUnFundedtransactions.SelectedValue + "'"
                    ElseIf strString <> String.Empty Then
                        strString += " AND UnfundedTransactions ='" + ddlUnFundedtransactions.SelectedValue + "'"
                    End If
                End If

                If (lbllnkWithdrawalApplicant.Visible = True Or lbllnkRetirementApplicant.Visible = True) And ddlLastContributionRecieved.SelectedValue <> "All" Then
                    If strString = String.Empty Then
                        strString = "LastcontributionREceived ='" + ddlLastContributionRecieved.SelectedValue + "'"
                    ElseIf strString <> String.Empty Then
                        strString += " AND LastcontributionREceived ='" + ddlLastContributionRecieved.SelectedValue + "'"
                    End If
                End If
                'End: AA:20.03.2014 - BT:957: YRS 5.0-1484 added below code to filter withdrawal / retirement applicants with unfunded transactions and last contributions

                dataview.RowFilter = strString

                If Not ViewState("Sort") Is Nothing Then
                    'dataview.Sort() = ViewState("Sort")
                    Sorting = ViewState("Sort")
                    dataview.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
                End If
                If Not ViewState("PageIndex") Is Nothing Then
                    Me.gvStatusList.PageIndex = ViewState("PageIndex")
                End If
                If HelperFunctions.isNonEmpty(dataview) Then

                    ViewState("FilterationString") = strString
                    BindGrid(dataview, gvStatusList)
                Else
                    BindGrid(Nothing, gvStatusList)
                End If
                btnSearch.UseSubmitBehavior = True

            Else
                BindGrid(Nothing, gvStatusList)
                btnSearch.UseSubmitBehavior = True
            End If
            dtEmployee = Nothing
        Catch
            Throw
        End Try
    End Sub
    'clears all textboxes
    Private Sub cleartextboxes(ByVal ctrl As Control)
        Try
            For Each ctrls As Control In ctrl.Controls
                If TypeOf ctrls Is TextBox Then
                    Dim t As TextBox = TryCast(ctrls, TextBox)
                    If t IsNot Nothing Then
                        t.Text = [String].Empty
                    End If
                Else
                    If ctrl.Controls.Count > 0 Then
                        cleartextboxes(ctrls)
                    End If
                End If
            Next
        Catch
            Throw
        End Try
    End Sub
    'to open all applicants tab
    Protected Sub lnkTMApllicant_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkTMApllicant.Click
        Try
            'AA:16.12.2013 - BT:2311 Added to validate the changes exists while tab change
            If (ViewState("tabchange") IsNot Nothing Or GetSelectedRecordIds().Count = 0) And gvStatusList.EditIndex = -1 Then
                tdTMApplicant.Style.Add("background-color", "#93BEEE")
                tdTMApplicant.Style.Add("color", "#000000")
                tdRetirement.Style.Add("background-color", "#4172A9")
                tdRetirement.Style.Add("color", "#ffffff")
                tdWithdrawal.Style.Add("background-color", "#4172A9")
                tdWithdrawal.Style.Add("color", "#ffffff")
                lnkTMApllicant.Visible = False
                lbllnkTMApplicant.Visible = True
                gvStatusList.EditIndex = -1
                lnkWithdrawal.Visible = True
                lbllnkWithdrawalApplicant.Visible = False
                lnkRetirement.Visible = True
                lbllnkRetirementApplicant.Visible = False
                'AA:10.10.2013 :YRS 5.0-1484:Changed All Applicants to Terminated Applicants
                LabelGenHdr.Text = "Terminated Applicants"
                ClearSearch()
                'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                clearSession()
                gvStatusList.PageIndex = 0
                LoadStatusTab()
                'AA:10.10.2013 :YRS 5.0-1484:Commented below line for not clearing attributs
                'btnPurge.Attributes.Clear()
                'btnAdd.Attributes.Clear()
                'AA:10.10.2013 :YRS 5.0-1484:Added to disable button add and hiding employement drop-down in filer search
                btnAdd.Enabled = False
                ddlEmployment.Visible = False
                lblEmployment.Visible = False
                'AA:11.03.2014 - BT:957: YRS 5.0-1484 
                '- Added to show process buttons to visible only in termainated applicants 
                '- Added to show purge buttons to hide in terminated applicants 
                '- Added to show drop-down wacher type and hide unfunded transactions and last contribution received drop-downs
                btnProcess.Visible = True
                btnProcessAll.Visible = True
                btnPurge.Visible = False
                ddlWatcherType.Visible = True
                lblWatchertype.Visible = True
                If ddlWatcherType.Items.Count > 0 Then
                    ddlWatcherType.SelectedIndex = 0
                End If
                lblUnfundedtransactions.Visible = False
                ddlUnFundedtransactions.Visible = False
                lblLastContributionRecieved.Visible = False
                ddlLastContributionRecieved.Visible = False
            Else
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
                ViewState("tabchange") = "Terminated"
            End If
        Catch
            Throw
        End Try
    End Sub
    'to open withdrawal tab
    Protected Sub lnkWithdrawal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkWithdrawal.Click
        Try
            'AA:16.12.2013 - BT:2311 Added to validate the changes exists while tab change
            If (ViewState("tabchange") IsNot Nothing Or GetSelectedRecordIds().Count = 0) And gvStatusList.EditIndex = -1 Then
                tdWithdrawal.Style.Add("background-color", "#93BEEE")
                tdWithdrawal.Style.Add("color", "#000000")
                tdRetirement.Style.Add("background-color", "#4172A9")
                tdRetirement.Style.Add("color", "#ffffff")
                tdTMApplicant.Style.Add("background-color", "#4172A9")
                tdTMApplicant.Style.Add("color", "#ffffff")
                lnkWithdrawal.Visible = False
                lbllnkWithdrawalApplicant.Visible = True
                lnkRetirement.Visible = True
                lbllnkRetirementApplicant.Visible = False
                gvStatusList.EditIndex = -1
                lnkTMApllicant.Visible = True
                lbllnkTMApplicant.Visible = False
                LabelGenHdr.Text = "Withdrawal Applicants"
                ClearSearch()
                'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                gvStatusList.PageIndex = 0
                clearSession()
                LoadWithdrawalTab()
                'AA:10.10.2013 :YRS 5.0-1484:Added to Enable button add and show employement drop-down in filer search
                btnAdd.Enabled = True
                ddlEmployment.Visible = True
                lblEmployment.Visible = True
                'AA:11.03.2014 - BT:957: YRS 5.0-1484 
                '- Added to show process buttons to hide in withdarwal applicants 
                '- Added to show purge buttons to hide in withdarwal applicants 
                '- Added to hide drop-down wacher type and show unfunded transactions and last contribution received drop-downs
                btnProcess.Visible = False
                btnProcessAll.Visible = False
                btnPurge.Visible = True
                ddlWatcherType.Visible = False
                lblWatchertype.Visible = False
                lblUnfundedtransactions.Visible = True
                ddlUnFundedtransactions.Visible = True
                lblLastContributionRecieved.Visible = True
                ddlLastContributionRecieved.Visible = True
            Else
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
                ViewState("tabchange") = "Withdarwal"
            End If
        Catch
            Throw
        End Try
    End Sub
    'to open retirement tab
    Protected Sub lnkRetirement_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkRetirement.Click
        Try
            'AA:16.12.2013 - BT:2311 Added to validate the changes exists while tab change
            If (ViewState("tabchange") IsNot Nothing Or GetSelectedRecordIds().Count = 0) And gvStatusList.EditIndex = -1 Then
                tdRetirement.Style.Add("background-color", "#93BEEE")
                tdRetirement.Style.Add("color", "#000000")
                tdTMApplicant.Style.Add("background-color", "#4172A9")
                tdTMApplicant.Style.Add("color", "#ffffff")
                tdWithdrawal.Style.Add("background-color", "#4172A9")
                tdWithdrawal.Style.Add("color", "#ffffff")
                lnkWithdrawal.Visible = True
                lbllnkWithdrawalApplicant.Visible = False
                lnkRetirement.Visible = False
                lbllnkRetirementApplicant.Visible = True
                gvStatusList.EditIndex = -1
                lnkTMApllicant.Visible = True
                lbllnkTMApplicant.Visible = False
                LabelGenHdr.Text = "Retirement Applicants"
                ClearSearch()
                'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                clearSession()
                gvStatusList.PageIndex = 0
                LoadRetirementTab()
                'AA:10.10.2013 :YRS 5.0-1484:Added to Enable button add and show employement drop-down in filer search
                btnAdd.Enabled = True
                ddlEmployment.Visible = True
                lblEmployment.Visible = True
                'AA:11.03.2014 - BT:957: YRS 5.0-1484 
                '- Added to show process buttons to hide in Retirement applicants 
                '- Added to show purge buttons to hide in Retirement applicants 
                '- Added to hide drop-down wacher type and show unfunded transactions and last contribution received drop-downs
                btnProcess.Visible = False
                btnProcessAll.Visible = False
                btnPurge.Visible = True
                ddlWatcherType.Visible = False
                lblWatchertype.Visible = False
                lblUnfundedtransactions.Visible = True
                ddlUnFundedtransactions.Visible = True
                lblLastContributionRecieved.Visible = True
                ddlLastContributionRecieved.Visible = True
            Else
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
                ViewState("tabchange") = "Retirement"
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles Gridview pageindexing
    Public Sub gvStatusList_OnPageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStatusList.PageIndexChanging
        Dim dv As DataView
        Dim dtEmployee As New DataTable
        Dim Sorting As GridViewCustomSort
        Try
            'AA:16.12.2013 - BT:2311 Commented below code and saving the checked values while changing the grid page
            'If gvStatusList.EditIndex <> -1 Then
            '    lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
            '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
            '    ViewState("SetPageIndex") = e.NewPageIndex
            'Else

            SaveCheckedValues()
            dtEmployee = getDataTable()
            dv = dtEmployee.DefaultView
            If Not ViewState("Sort") Is Nothing Then
                'dv.Sort() = ViewState("Sort")
                Sorting = ViewState("Sort")
                dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If
            Me.gvStatusList.PageIndex = e.NewPageIndex
            BindGrid(dv, gvStatusList)
            ViewState("PageIndex") = e.NewPageIndex
            dtEmployee = Nothing
            dv = Nothing
            PopulateCheckedValues()
            'End If

        Catch
            Throw
        End Try
    End Sub
    'Handles when cancel link is pressed in gridview
    Private Sub gvStatusList_OnRowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvStatusList.RowCancelingEdit
        Dim dtEmployee As DataTable
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        Try
            gvStatusList.EditIndex = -1
            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            dtEmployee = getDataTable()
            dv = dtEmployee.DefaultView
            If Not ViewState("Sort") Is Nothing Then
                'dv.Sort() = ViewState("Sort")
                Sorting = ViewState("Sort")
                dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            If Not ViewState("PageIndex") Is Nothing Then
                Me.gvStatusList.PageIndex = ViewState("PageIndex")
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If

            BindGrid(dv, gvStatusList)
            'AA:10.10.2013 :YRS 5.0-1484:added below line to clear the dirty flag
            SetOrClearDirtyFlag(String.Empty)
        Catch
            Throw
        End Try
    End Sub
    'Handles when Edit link is pressed in gridview
    Private Sub gvStatusList_OnRowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvStatusList.RowEditing
        'Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
        Dim dropdown As New DropDownList
        Dim dv As DataView
        Dim dtEmployee As DataTable
        Dim Sorting As GridViewCustomSort
        Try
            '        If Not checkSecurity.Equals("True") Then
            ''            lblMessage.Text = checkSecurity
            ''ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES') ", True)
            ''dtEmployee = getDataTable()
            ''dv = dtEmployee.DefaultView

            'If Not ViewState("Sort") Is Nothing Then
            '	dv.Sort() = ViewState("Sort")
            'End If
            'If Not ViewState("PageIndex") Is Nothing Then
            '	Me.gvStatusList.PageIndex = ViewState("PageIndex")
            'End If
            'If ViewState("FilterationString") <> "" Then
            '	dv.RowFilter = ViewState("FilterationString")
            'End If

            'BindGrid(dv, gvStatusList)

            ''ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "" + Page.ClientScript.GetPostBackEventReference(btnOK, "").ToString(), True)
            ''ButtonCreateBatch.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnOK, "").ToString())
            '            Exit Sub
            '        End If
            gvStatusList.EditIndex = e.NewEditIndex
            Dim strfundEventId As String = String.Empty
            Dim strPlanType As String = DirectCast(gvStatusList.Rows(e.NewEditIndex).FindControl("lblPlantype"), Label).Text
            Dim drv As DataRowView = DirectCast((gvStatusList.Rows(e.NewEditIndex).DataItem), DataRowView)
            dtEmployee = getDataTable()
            If HelperFunctions.isNonEmpty(dtEmployee) Then
                Dim l_Arr_DataRow As DataRow()
                l_Arr_DataRow = dtEmployee.Select("UniqueId='" + gvStatusList.DataKeys(e.NewEditIndex).Value.ToString() + "'")
                strfundEventId = l_Arr_DataRow(0)("guiFundEventId").ToString()
            End If

            If Not IsNothing(strPlanType) Then
                'Start,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                If strPlanType.ToUpper = "RET" Then
                    strPlanType = "RETIREMENT"
                ElseIf strPlanType.ToUpper = "SAV" Then
                    strPlanType = "SAVINGS"
                End If
                'END,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                dropdown = DirectCast(gvStatusList.Rows(e.NewEditIndex).FindControl("drdPlanType"), DropDownList)
                If Not IsNothing(dropdown) Then
                    Dim dsApplicantPlantype As DataSet
                    dsApplicantPlantype = YMCARET.YmcaBusinessObject.TerminationWatcherBO.GetApplicantPlanType(strfundEventId)
                    If HelperFunctions.isNonEmpty(dsApplicantPlantype) Then
                        dropdown.DataSource = dsApplicantPlantype
                        dropdown.DataTextField = "PlanType"
                        dropdown.DataValueField = "PlanType".ToUpper()
                        dropdown.DataBind()
                        If dsApplicantPlantype.Tables(0).Rows.Count = 2 Then
                            dropdown.Items.Insert(2, New ListItem("BOTH", "BOTH"))
                        End If
                    End If

                    'Start:Anudeep:26-11-2012 - Added the code for if not exists the selected plan then add in dropdown
                    If dropdown.Items.FindByValue(strPlanType) Is Nothing Then
                        dropdown.Items.Insert(dropdown.Items.Count, New ListItem(strPlanType, strPlanType))
                    End If
                    'End:Anudeep:26-11-2012 - Added the code for if not exists the selected plan then add in dropdown

                    dropdown.SelectedValue = strPlanType
                End If
            End If

            dv = dtEmployee.DefaultView

            If Not ViewState("Sort") Is Nothing Then
                'dv.Sort() = ViewState("Sort")
                Sorting = ViewState("Sort")
                dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            If Not ViewState("PageIndex") Is Nothing Then
                Me.gvStatusList.PageIndex = ViewState("PageIndex")
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If

            BindGrid(dv, gvStatusList)
            'AA:10.10.2013 :YRS 5.0-1484:added below line to set the dirty flag
            SetOrClearDirtyFlag(True)
        Catch
            Throw
        End Try
    End Sub
    'Handles when delete button is pressed in gridview
    Private Sub gvStatusList_OnRowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvStatusList.RowDeleting
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnDelete", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
                Exit Sub
            End If
            struniqueid = gvStatusList.DataKeys(e.RowIndex).Value.ToString()
            Session("Flag") = "Delete"
            lblMessage.Text = messageFromResourceFile("MESSAGE_TW_DELETE_CONFIRM", "")
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True)
            'AA:10.10.2013 :YRS 5.0-1484:added below line to clear the dirty flag
            SetOrClearDirtyFlag(String.Empty)
            Exit Sub
        Catch
            Throw
        End Try
    End Sub
    'Handles when Save link is pressed in gridview
    Private Sub gvStatusList_OnRowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvStatusList.RowUpdating
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
        Dim dtEmployee As DataTable
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        Try
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES')", True)
                Exit Sub
            End If

            Dim row As GridViewRow = DirectCast(sender, GridView).Rows(e.RowIndex)
            Dim struniqueid, strPlanType As String
            Dim strlblPlanType As String = DirectCast(gvStatusList.Rows(e.RowIndex).FindControl("lblPlantype"), Label).Text
            struniqueid = gvStatusList.DataKeys(e.RowIndex).Value.ToString()

            Dim drd As DropDownList = DirectCast(row.FindControl("drdPlanType"), DropDownList)
            If Not IsNothing(drd) Then
                strPlanType = drd.SelectedValue.Trim()
            End If
            'Start,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            If strlblPlanType.ToUpper = "RET" Then
                strlblPlanType = "RETIREMENT"
            ElseIf strlblPlanType.ToUpper = "SAV" Then
                strlblPlanType = "SAVINGS"
            End If
            'END,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            If strlblPlanType <> strPlanType Then
                UpdateRecord(struniqueid, strPlanType)
            End If
            gvStatusList.EditIndex = -1
            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            LoadData()
            dtEmployee = getDataTable()
            dv = dtEmployee.DefaultView
            If Not ViewState("Sort") Is Nothing Then
                'dv.Sort() = ViewState("Sort")
                Sorting = ViewState("Sort")
                dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            If Not ViewState("PageIndex") Is Nothing Then
                Me.gvStatusList.PageIndex = ViewState("PageIndex")
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If

            BindGrid(dv, gvStatusList)
            'AA:10.10.2013 :YRS 5.0-1484:added below line to clear the dirty flag
            SetOrClearDirtyFlag(String.Empty)

        Catch
            Throw
        End Try
    End Sub
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim dtEmployee As DataTable
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        Try
            'AA:16.12.2013 - BT:2311 Added to check validation while clearing search
            If GetSelectedRecordIds().Count > 1 Or gvStatusList.EditIndex <> -1 Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
                ViewState("Search") = True
            Else
                gvStatusList.EditIndex = -1
                ClearSearch()
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                dtEmployee = getDataTable()
                dv = dtEmployee.DefaultView
                If Not ViewState("Sort") Is Nothing Then
                    'dv.Sort() = ViewState("Sort")
                    Sorting = ViewState("Sort")
                    dv.Sort() = Sorting.SortExpression + " " + Sorting.SortDirection
                End If
                If Not ViewState("PageIndex") Is Nothing Then
                    Me.gvStatusList.PageIndex = ViewState("PageIndex")
                End If
                BindGrid(dv, gvStatusList)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Clears  the search 
    Private Sub ClearSearch()
        Try
            Session("SearchSSno") = Nothing
            Session("SearchFundNo") = Nothing
            Session("SearchFirstName") = Nothing
            Session("SearchLastName") = Nothing
            ViewState("FilterationString") = Nothing
            txtStatusFundNo.Text = ""
            txtStatusFirstName.Text = ""
            txtStatusLastName.Text = ""
            chkInvalidRecord.Checked = False
            chkProccessedRecords.Checked = False
            'AA:10.10.2013 :YRS 5.0-1484:added below line to clear the textboxes in master page
            'cleartextboxes(Form1)
            cleartextboxes(Master.FindControl("Form1"))
            'AA:10.10.2013 :YRS 5.0-1484:added below line to set the default status of dro-down to all
            ddlEmployment.SelectedValue = "All"
            ddlUnFundedtransactions.SelectedValue = "All"
            ddlLastContributionRecieved.SelectedValue = "All"
        Catch
            Throw
        End Try
    End Sub
    'handles gridview sorting
    Private Sub gvStatusList_OnSorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvStatusList.Sorting
        Try
            'AA:16.12.2013 - BT:2311 To save and populate the checked checkboxes
            SaveCheckedValues()
            Me.gvStatusList.SelectedIndex = -1
            Sortgrid(e)
            PopulateCheckedValues()
        Catch
            Throw
        End Try

    End Sub
    'Sorting the gridview
    Public Sub Sortgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim dv As DataView
        Dim SortExpression As String
        Dim dtEmployee As DataTable
        Try
            gvStatusList.EditIndex = -1
            SortExpression = e.SortExpression
            dtEmployee = getDataTable()
            dv = dtEmployee.DefaultView
            dv.Sort = SortExpression
            'AA:BT:957: YRS.5.0 1484 - Changed the sorting code to show the sorting arrows
            'If Not ViewState("Sort") Is Nothing Then
            '    If ViewState("Sort").ToString.Trim.EndsWith(" DESC") Then
            '        dv.Sort = SortExpression + " ASC"
            '    Else
            '        dv.Sort = SortExpression + " DESC"
            '    End If
            'Else
            '    dv.Sort = SortExpression + " DESC"
            'End If
            'ViewState("Sort") = dv.Sort()
            HelperFunctions.gvSorting(ViewState("Sort"), e.SortExpression, dv)
            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            If Not ViewState("PageIndex") Is Nothing Then
                Me.gvStatusList.PageIndex = ViewState("PageIndex")
            End If
            If ViewState("FilterationString") <> "" Then
                dv.RowFilter = ViewState("FilterationString")
            End If

            BindGrid(dv, gvStatusList)

        Catch
            Throw
        End Try
    End Sub
    'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    'Button cancel click
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            'AA:16.12.2013 - BT:2311 to clear the sessions
            ViewState("Search") = Nothing
            ViewState("tabchange") = Nothing
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch
            Throw
        End Try
    End Sub
    'Gets the message from resource file
    Private Function messageFromResourceFile(ByVal strMessage As String, ByVal strParameter As String) As String
        Dim strReturnMessage As String = String.Empty
        Dim TW As Resources.TerminationWatcher
        Try
            If strParameter = String.Empty Then
                strReturnMessage = TW.ResourceManager.GetString(strMessage).Trim()
            ElseIf strMessage = "MESSAGE_TW_ADDUSER_RECORD_EXISTS" Then
                strReturnMessage = String.Format(Resources.TerminationWatcher.MESSAGE_TW_ADDUSER_RECORD_EXISTS, strParameter)
            End If
            Return strReturnMessage
        Catch
            Throw
        End Try

    End Function

    'To get datatable to bind the grid 
    'Created by Anudeep on 19-10-2012 getDataTable() for gridbinding

    Public Function getDataTable() As DataTable
        Dim dtEmployee As New DataTable
        Try
            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            If Session("Refresh") = "YES" Then
                LoadData()
                Session("Refresh") = Nothing
            End If
            dtEmployee = dsStatusData.Tables(0).Clone()
            'checks whether processed chekbox is clicked
            If chkProccessedRecords.Checked Then
                For i As Integer = 0 To dsProccesedData.Tables(0).Rows.Count - 1
                    dtEmployee.ImportRow(dsProccesedData.Tables(0).Rows(i))
                Next
            End If
            ' checks whether invalid chekbox is checked
            If chkInvalidRecord.Checked Then
                For i As Integer = 0 To dsInvalidRecords.Tables(0).Rows.Count - 1
                    dtEmployee.ImportRow(dsInvalidRecords.Tables(0).Rows(i))
                Next
            End If

            If HelperFunctions.isEmpty(dsStatusData) Then
                LoadData()
            End If
            ' for all participants tab
            If lnkTMApllicant.Visible = False Then
                If Not dtEmployee Is Nothing Then
                    For i As Integer = 0 To dsStatusData.Tables(0).Rows.Count - 1
                        dtEmployee.ImportRow(dsStatusData.Tables(0).Rows(i))
                    Next
                Else
                    dtEmployee = dsStatusData.Tables(0)
                End If
                ' foe withdrawal tab
            ElseIf lnkWithdrawal.Visible = False Then
                If Not dtEmployee Is Nothing Then
                    For i As Integer = 0 To dtWithdrawalData.Rows.Count - 1
                        dtEmployee.ImportRow(dtWithdrawalData.Rows(i))
                    Next
                Else
                    dtEmployee = dtWithdrawalData
                End If
                ' for retirement tab
            ElseIf lnkRetirement.Visible = False Then
                If Not dtEmployee Is Nothing Then
                    For i As Integer = 0 To dtRetirementData.Rows.Count - 1
                        dtEmployee.ImportRow(dtRetirementData.Rows(i))
                    Next
                Else
                    dtEmployee = dtRetirementData
                End If
            End If

            'AA:10.10.2013 :YRS 5.0-1484:Added to show only terminated records in terminated particiapants
            If lnkTMApllicant.Visible = False Then
                'AA:20.03.2014 - BT:957: YRS 5.0-1484 _ Added to show the terminated applciants tab 
                ' if emp status is "t" and no unfunded transaction exists and lat contribution is received
                'Dim dvTerminated As New DataView(dtEmployee, "Status = 'TM'", "Processed", DataViewRowState.CurrentRows)
                Dim dvTerminated As New DataView(dtEmployee, "[Emp Status] = 'T'", "Processed", DataViewRowState.CurrentRows)
                dtEmployee = Nothing
                dtEmployee = dvTerminated.ToTable
                dvTerminated = Nothing
                'filters the all withdrawal records 
            ElseIf lnkWithdrawal.Visible = False Then
                'AA:20.03.2014 - BT:957: YRS 5.0-1484 _ Added to show the terminated applciants tab 
                ' if type is withdrawal who unfundtransacion exists or last contribution not recieved
                ' or if emp event statsus is 'A' and no undedrasaction exists and last contribution is not recieved
                Dim dvWithdrawal As New DataView(dtEmployee, "Type='Withdrawal' AND (([UnfundedTransactions] = 'Yes' Or [LastcontributionREceived] ='No')" + _
                                                 "OR  ([Emp Status] IN ('A','M','N') AND [UnfundedTransactions] = 'No' AND [LastcontributionREceived] ='Yes'))", "Processed", DataViewRowState.CurrentRows)
                dtEmployee = Nothing
                dtEmployee = dvWithdrawal.ToTable
                dvWithdrawal = Nothing
                ' filters all the retirement records
            ElseIf lnkRetirement.Visible = False Then
                'AA:20.03.2014 - BT:957: YRS 5.0-1484 _ Added to show the terminated applciants tab 
                ' if type is Retirement who unfundtransacion exists or last contribution not recieved
                ' or if emp event statsus is 'A' and no undedrasaction exists and last contribution is not recieved
                Dim dvRetirement As New DataView(dtEmployee, "Type='Retirement' AND (([UnfundedTransactions] = 'Yes' Or [LastcontributionREceived] ='No')" + _
                                                 "OR  ([Emp Status] IN ('A','M','N') AND [UnfundedTransactions] = 'No' AND [LastcontributionREceived] ='Yes'))", "Processed", DataViewRowState.CurrentRows)
                dtEmployee = Nothing
                dtEmployee = dvRetirement.ToTable
                dvRetirement = Nothing
            End If
            dtEmployee.DefaultView.Sort = "Processed Desc"
            Return dtEmployee
        Catch
            Throw
        End Try
    End Function

    ''AA:16.12.2013 - BT:2311  Commented belwo code to remove auto fill on check box check change

    'Start :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
    'Private Sub chkInvalidRecord_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInvalidRecord.CheckedChanged
    '    Dim dtEmployee As New DataTable
    '    Dim dv As DataView
    '    Try
    '        'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    '        If gvStatusList.EditIndex = -1 Then
    '            StatusSearch()
    '        Else
    '            lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
    '            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
    '            ViewState("checkboxChange") = "Invalid"
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub

    'Private Sub chkProccessedRecords_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkProccessedRecords.CheckedChanged
    '    Dim dtEmployee As New DataTable
    '    Dim dv As DataView
    '    Try
    '        'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    '        If gvStatusList.EditIndex = -1 Then
    '            'gvStatusList.EditIndex = -1
    '            StatusSearch()
    '        Else
    '            lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
    '            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
    '            ViewState("checkboxChange") = "Process"
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'End :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
    'Start Anudeep:06-nov-2012 Changes made For Process Report
    Private Function GetReportName() As String
        Dim dsTWProcessReport As DataSet
        Dim strReportname As String
        Try
            dsTWProcessReport = YMCARET.YmcaBusinessObject.TerminationWatcherBO.GetProcessReportName("TW_PROCESS_RPT")
            If HelperFunctions.isNonEmpty(dsTWProcessReport) Then
                strReportname = dsTWProcessReport.Tables(0).Rows(0)("Value")
            Else
                strReportname = "TerminationWatcher"
            End If
            Return strReportname
        Catch
            Throw
        End Try
    End Function

    Private Sub OpenReportViewer()
        Try
            'Call ReportViewer.aspx 
            'Anudeep:22-11-2012 Code added for not minimizing the report frequently 
            Dim popupScript As String = " newwindow = window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " +
                                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');" +
                                        " if (window.focus) {newwindow.focus()}" +
                                        " if (!newwindow.closed) {newwindow.focus()}"

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", popupScript, True)
        Catch
            Throw
        End Try
    End Sub
    'End Anudeep:06-nov-2012 Changes made For Process Report
    'AA:16.12.2013 - BT:2311 Commented below code to remove drop-down change 
    'AA:10.10.2013 :YRS 5.0-1484:Added to get the drop-down selected fund events records in grid
    'Private Sub ddlEmployment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEmployment.SelectedIndexChanged
    '    Try
    '        If gvStatusList.EditIndex = -1 Then
    '            StatusSearch()
    '        Else
    '            lblMessage.Text = messageFromResourceFile("MESSAGE_TW_PAGE_CONFIRMATION", "")
    '            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel')", True)
    '            ViewState("dropdownChange") = "Process"
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'AA:10.10.2013 :YRS 5.0-1484:added below line to set/clear the dirty flag
    Public Sub SetOrClearDirtyFlag(ByVal flag As String)
        Dim hdnDirty As HiddenField
        Try
            hdnDirty = Master.FindControl("HiddenFieldDirty")
            If Not hdnDirty Is Nothing Then
                If flag = "True" Then
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Mark", "mark_dirty();", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Clear", "clear_dirty();", True)
                End If
                hdnDirty.Value = flag
            End If
        Catch ex As Exception

        End Try

    End Sub
    'Start: AA:16.12.2013 - BT:2311 to save the checkbox checked and again populate then while paging ans sorting

    Private Sub PopulateCheckedValues()
        'For Select All check box
        Dim checkBoxArray As ArrayList = DirectCast(ViewState("checkBoxArray"), ArrayList)
        Dim checkAllIndex As String = "chkAll-" & gvStatusList.PageIndex

        If checkBoxArray.IndexOf(checkAllIndex) <> -1 Then
            Dim chkAll As CheckBox = DirectCast(gvStatusList.HeaderRow.Cells(0).FindControl("chkSelectAll"), CheckBox)
            chkAll.Checked = True
        End If
        'end, For Select All check box

        Dim userdetails As ArrayList = DirectCast(Session("TW_CHECKED_ITEMS"), ArrayList)
        If userdetails IsNot Nothing AndAlso userdetails.Count > 0 Then
            For Each gvrow As GridViewRow In gvStatusList.Rows
                Dim index As String = gvStatusList.DataKeys(gvrow.RowIndex).Value
                If userdetails.Contains(index) Then
                    Dim myCheckBox As CheckBox = DirectCast(gvrow.FindControl("chkSelect"), CheckBox)
                    myCheckBox.Checked = True
                End If
            Next
        End If
    End Sub
    Private Sub SaveCheckedValues()
        Dim userdetails As New ArrayList()
        Dim index As String = "-1"
        ' For select all
        Dim CheckBoxIndex As Integer
        Dim CheckAllWasChecked As Boolean = False
        If ViewState("checkBoxArray") IsNot Nothing Then
            checkBoxArray = DirectCast(ViewState("checkBoxArray"), ArrayList)
        Else
            checkBoxArray = New ArrayList()
        End If

        Dim chkAll As New CheckBox
        If gvStatusList.Rows.Count > 0 Then
            chkAll = DirectCast(gvStatusList.HeaderRow.Cells(0).FindControl("chkSelectAll"), CheckBox)
            Dim checkAllIndex As String = "chkAll-" & gvStatusList.PageIndex

            If chkAll.Checked Then
                If checkBoxArray.IndexOf(checkAllIndex) = -1 Then
                    checkBoxArray.Add(checkAllIndex)
                End If
            Else
                If checkBoxArray.IndexOf(checkAllIndex) <> -1 Then
                    checkBoxArray.Remove(checkAllIndex)
                    CheckAllWasChecked = True
                End If
            End If
        End If
        ViewState("checkBoxArray") = checkBoxArray
        'end,  For select all


        For Each gvrow As GridViewRow In gvStatusList.Rows
            index = gvStatusList.DataKeys(gvrow.RowIndex).Value
            Dim result As Boolean = DirectCast(gvrow.FindControl("chkSelect"), CheckBox).Checked  ' Check in the Session 
            If Session("TW_CHECKED_ITEMS") IsNot Nothing Then
                userdetails = DirectCast(Session("TW_CHECKED_ITEMS"), ArrayList)
            End If
            If result Then
                If Not userdetails.Contains(index) Then
                    userdetails.Add(index)
                End If
            Else
                userdetails.Remove(index)
            End If
        Next
        If userdetails IsNot Nothing AndAlso userdetails.Count > 0 Then
            Session("TW_CHECKED_ITEMS") = userdetails
        End If
    End Sub
    'Start: AA:20.03.2014 - BT:957: YRS 5.0-1484 _ Added process all button to process all the records in the terminated applicants tab    
    Private Sub btnProcessAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessAll.Click
        Dim intRecordsCount As Integer
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
                Exit Sub
            End If
            intRecordsCount = GetAllRecordsId.Count
            If (intRecordsCount = 0) Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_TW_SELECT_RECORD", "")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
            Else
                Session("Flag") = "Process"
                Session("Process") = "All"
                lblMessage.Text = String.Format(messageFromResourceFile("MESSAGE_TW_PROCESS_CONFIRMATION_ALL", ""), intRecordsCount)
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("Terminationwatcher_btnProcessAll_Click", ex)
        End Try
    End Sub
    Public Function GetAllRecordsId() As List(Of String)
        Dim LTerminationIds As New List(Of String)
        Dim dtList As DataTable
        Try
            dtList = getDataTable()
            For Each drRow As DataRow In dtList.Rows
                LTerminationIds.Add(drRow("UniqueId").ToString())
            Next
            Return LTerminationIds
        Catch ex As Exception
            Throw
        End Try
    End Function
    'End: AA:20.03.2014 - BT:957: YRS 5.0-1484 _ Added process all button to process all the records in the terminated applicants tab
End Class