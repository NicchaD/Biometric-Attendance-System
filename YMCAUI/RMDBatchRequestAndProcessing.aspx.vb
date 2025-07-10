'*******************************************************************************'
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	MRDRequestandProcessing..aspx.vb
' Author Name		:	Dinesh
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
'
' Designed by			:	Dinesh/Hafiz
' Designed on			:	04/11/2014
'
'*******************************************************************************

'**********************************************************************************************************************  
'** Modification History    
'**********************************************************************************************************************    
'** Modified By     Date(MM/DD/YYYY)    Description  
'**********************************************************************************************************************  
'Anudeep            10/27/2014      BT:2691-Get the Merge PDF path from the configuration key
'Dinesh Kanojia     12/10/2014      BT:2732:Cashout observations
'Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations
'Dinesh Kanojia     12/22/2014      BT:2735:Add activities logging functionality in RMD and RollIn batch creation.
'Anudeep            02/04/2015      BT:2139:YRS 5.0-2165:RMD enhancements
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Sanjay GS Rawat    2016.04.05      YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)
'Chandra sekar      2016.10.17      YRS-AT-2476 - Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)
'Chandra sekar      2016.10.24      YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
'Manthan Rajguru    2016.10.27      YRS-AT-2922 -  YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)     
'Santosh Bura       2016.11.21      YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224)     
'Santosh Bura       2017.01.17      YRS-AT-3297 -  YRS-AT-3297 -  YRS enh: RMD Process screen: treat plans separately 
'Pramod P. Pokale   2017.05.02      YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186)
'Manthan Rajguru    2017.05.04      YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186)
'Pooja Kumkar       2019.04.15      YRS-AT-3170 -  YRS-AT-3170 is already resolved while, doing the changes for YRS-AT-2476. Added same line again to checkin changes under the ticket YRS-AT-3170.
'*********************************************************************************************************************/ 

Imports System.IO
Imports System.Collections
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Web.Script.Serialization
Imports YMCAObjects
Imports YMCARET.CommonUtilities

Public Class RMDBatchRequestAndProcessing
    Inherits System.Web.UI.Page


#Region "Varaibles"

    Public ds_EligibleRDMRecords As DataSet
    Public ds_NonEligibleRDMRecords As DataSet

    Dim numberOfThreads As Integer = 2

#End Region

#Region "Property"
    'START : CS |2016.10.17 | YRS-AT-2476 |  TO get/set the Selected MRD record count
    Public Property SelectedMrdRecordsCount() As Integer
        Get
            If Not ViewState("SelectedMrdRecordsCount") Is Nothing Then
                Return (CType(ViewState("SelectedMrdRecordsCount"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("SelectedMrdRecordsCount") = Value
        End Set
    End Property
    'END : CS |2016.10.17 | YRS-AT-2476 |  TO get/set the Selected MRD record count

    'START : MMR |2016.11.01 | YRS-AT-2922 | Declared property to set Batch id, RMDYear, RMDDueDate and closed year
    Public Property RMDBatchID As String
        Get
            If Not Session("BatchID") Is Nothing Then
                Return (CType(Session("BatchID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("BatchID") = Value
        End Set
    End Property

    Public Property RMDYear As Integer
        Get
            If Not Session("RMDYear") Is Nothing Then
                Return (CType(Session("RMDYear"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RMDYear") = Value
        End Set
    End Property

    Public Property RMDDueDate As String
        Get
            If Not Session("RMDDueDate") Is Nothing Then
                Return (CType(Session("RMDDueDate"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("RMDDueDate") = Value
        End Set
    End Property

    Public Property IsMarchClosed As Boolean
        Get
            If Not (Session("IsMarchClosed")) Is Nothing Then
                Return (CType(Session("IsMarchClosed"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsMarchClosed") = Value
        End Set
    End Property

    Public Property IsDecemberClosed As Boolean
        Get
            If Not (Session("IsDecemberClosed")) Is Nothing Then
                Return (CType(Session("IsDecemberClosed"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsDecemberClosed") = Value
        End Set
    End Property
    'END : MMR |2016.11.01 | YRS-AT-2922 | Declared property to set Batch id, RMDYear, RMDDueDate and closed year
#End Region

    'START : SB |2017.01.17 | YRS-AT-3297 | Declaring global variable for cashout participants
#Region "Global Declaration"
    Const iRMDAmount As Integer = 4
    Const iPaidAmount As Integer = 5
    Const iRemarks As Integer = 22
    Const iCashout As Integer = 25
#End Region
    'END : SB |2017.01.17 | YRS-AT-3297 | Declaring global variable for cashout participants

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMD Batch Request and Processing page load", "Page Load Call.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            CheckReadOnlyMode() 'PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If Not Page.IsPostBack Then
                'GetLastProccesedDate()
                ' BindEligibleRMDRecords()
                lnkEligiblepersons_Click(sender, e)

                'START : MMR |2016.11.01 | YRS-AT-2922 | Resetting property values on page load based on condition
                If Session("RMDClose") Is Nothing Then
                    RMDYear = 0
                    RMDDueDate = String.Empty
                    RMDBatchID = String.Empty
                    IsDecemberClosed = False
                    IsMarchClosed = False
                End If
                'END : MMR |2016.11.01 | YRS-AT-2922 | Resetting property values on page load based on condition
            End If

            'If Not Request.QueryString("RMD") Is Nothing Then
            '    If Request.QueryString("RMD") = "close" Then
            If Not Session("RMDClose") Is Nothing And Session("RMDClose") = True Then
                btnProcess.Visible = False
                btnPrintReport.Visible = False
                BindRMDProcessStatus()

                '  If Not Session("strReportName") Is Nothing Then
                '      Dim popupScript2 As String = "<script language='javascript'>" & _
                '"window.open('FT\\ReportViewer.aspx', 'ReportPopUp2', " & _
                '"'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                '"</script>"
                '      If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                '          Page.RegisterStartupScript("PopupScript2", popupScript2)
                '      End If
                '  End If
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
                Session("Rollin_MergedPdfs_Filename") = Nothing

                'START : MMR |2016.11.01 | YRS-AT-2922 | Changed label text for RMD Batch process tab
                'lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure."
                lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure/Outstanding."
                'END : MMR |2016.11.01 | YRS-AT-2922 | Changed label text for RMD Batch process tab
                btnProcess.Visible = False
                Session("RMDClose") = Nothing
                'START : MMR |2016.11.01 | YRS-AT-2922 | Resetting property values
                RMDYear = 0
                RMDDueDate = String.Empty
                RMDBatchID = String.Empty
                IsDecemberClosed = False
                IsMarchClosed = False
                'END : MMR |2016.11.01 | YRS-AT-2922 | Resetting property values
            End If

        Catch objException As Exception
            HelperFunctions.ShowMessageToUser(objException.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RMDBatchRequestAndProcessing_Page_Load", objException)
        End Try
    End Sub

    Private Sub SearchRMDRecords()
        Dim dtRecords As DataTable
        Dim dvRecords As DataView
        Dim strFilter As String = String.Empty
        If lbllnkEligible.Visible Then
            If Not ViewState("RMDEligible") Is Nothing Then
                dtRecords = CType(ViewState("RMDEligible"), DataTable)
                If dtRecords.Rows.Count > 0 Then
                    dvRecords = New DataView(dtRecords)
                    If ddlRMDYear.Items.Count > 0 Then
                        strFilter = " MRDExpireDate =#" + ddlRMDYear.SelectedValue + "#"
                    End If
                    dvRecords.RowFilter = strFilter
                    HelperFunctions.BindGrid(gvRMDProcess, dvRecords, True)
                    ViewState("RMDEligibleFilter") = strFilter
                End If
            End If
            lblRMDLastProcessDate.Text = "Current RMD Processing Date: " + ddlRMDYear.SelectedValue.Trim
        ElseIf lbllnkNonEligible.Visible Then
            If Not ViewState("NonEligibleRMD") Is Nothing Then
                dtRecords = CType(ViewState("NonEligibleRMD"), DataTable)
                If dtRecords.Rows.Count > 0 Then
                    dvRecords = New DataView(dtRecords)

                    If ddlNonEligibleRMDYear.Items.Count > 0 Then
                        strFilter = " MRDExpireDate =#" + ddlNonEligibleRMDYear.SelectedValue + "#"
                    End If

                    If ddlAcctLocked.SelectedValue.ToLower <> "all" Then
                        If ddlAcctLocked.SelectedValue.ToLower = "no" Then
                            If strFilter = String.Empty Then
                                strFilter = "  IsLocked = 0 "
                            Else
                                strFilter += " And  IsLocked = 0  "
                            End If
                        ElseIf ddlAcctLocked.SelectedValue.ToLower = "yes" Then
                            If strFilter = String.Empty Then
                                strFilter = "  IsLocked = 1 "
                            Else
                                strFilter += " And  IsLocked = 1  "
                            End If
                        End If
                    End If

                    If ddlInsufficientBal.SelectedValue.ToLower <> "all" Then
                        If ddlInsufficientBal.SelectedValue.ToLower = "no" Then
                            If strFilter = String.Empty Then
                                strFilter = " IsBlocked = 0 "
                            Else
                                strFilter += " And IsBlocked = 0  "
                            End If
                        ElseIf ddlInsufficientBal.SelectedValue.ToLower = "yes" Then
                            If strFilter = String.Empty Then
                                strFilter = " IsBlocked = 1 "
                            Else
                                strFilter += " And IsBlocked = 1  "
                            End If
                        End If
                    End If

                    If ddlNotEnrolled.SelectedValue.ToLower <> "all" Then
                        If ddlNotEnrolled.SelectedValue.ToLower = "yes" Then
                            If strFilter = String.Empty Then
                                strFilter = " bitAnnualMRDRequested = 1"
                            Else
                                strFilter += " And bitAnnualMRDRequested = 1"
                            End If
                        ElseIf ddlNotEnrolled.SelectedValue.ToLower = "no" Then
                            If strFilter = String.Empty Then
                                strFilter = " bitAnnualMRDRequested= 0"
                            Else
                                strFilter += " And bitAnnualMRDRequested = 0"
                            End If
                        End If
                    End If


                    If ddlPriorRMD.SelectedValue.ToLower <> "all" Then
                        If ddlPriorRMD.SelectedValue.ToLower = "yes" Then
                            If strFilter = String.Empty Then
                                strFilter = " IsMultipleYearMRD = 1"
                            Else
                                strFilter += " AND IsMultipleYearMRD = 1"
                            End If
                        ElseIf ddlPriorRMD.SelectedValue.ToLower = "no" Then
                            If strFilter = String.Empty Then
                                strFilter = " IsMultipleYearMRD = 0"
                            Else
                                strFilter += " AND IsMultipleYearMRD = 0"
                            End If
                        End If
                    End If

                    If ddlPendReq.SelectedValue.ToLower <> "all" Then
                        If ddlPendReq.SelectedValue.ToLower = "yes" Then
                            If strFilter = String.Empty Then
                                strFilter = " PendingRequest = 1"
                            Else
                                strFilter += " AND PendingRequest = 1"
                            End If
                        ElseIf ddlPendReq.SelectedValue.ToLower = "no" Then
                            If strFilter = String.Empty Then
                                strFilter = " PendingRequest = 0"
                            Else
                                strFilter += " AND PendingRequest = 0"
                            End If
                        End If
                    End If

                End If
                dvRecords.RowFilter = strFilter
                ViewState("RMDNonEligibleFilter") = strFilter
                HelperFunctions.BindGrid(gvNonEligible, dvRecords, True)
                lblRMDLastProcessDate.Text = "Current RMD Processing Date: " + ddlNonEligibleRMDYear.SelectedValue
            End If
        End If
    End Sub
    'START: MMR |2016.02.11 | YRS-AT-2922 | Commented code as not used anywhere
    'Private Sub BindProcessStatusRMDRecords()
    '    Dim dsSelectedBatchRecords As DataSet
    '    Try
    '        If Not Session("strBatchId") Is Nothing Then

    '            dsSelectedBatchRecords = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(Session("strBatchId"), BatchProcess.RMDBatchProcess)

    '            'dsSelectedBatchRecords = CType(Session("DisburseMRDRecords"), DataSet)

    '            If HelperFunctions.isNonEmpty(dsSelectedBatchRecords) Then
    '                gvRMDProcess.Visible = False
    '                gvRMDProcess.DataBind()
    '                gvProcessStatus.Visible = True
    '                gvProcessStatus.DataSource = dsSelectedBatchRecords
    '                gvProcessStatus.DataBind()
    '                lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure."
    '                ViewState("ProcessStatusRMDRecords") = dsSelectedBatchRecords
    '                If Not Session("DisburseMRDRecords") Is Nothing Then
    '                    btnProcess.Visible = False
    '                    btnPrintReport.Visible = False
    '                    lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure."
    '                End If
    '                HelperFunctions.ShowMessageToUser(GetRMDMessage("MESSAGE_BATCH_PROCESS"), EnumMessageTypes.Success)
    '            Else
    '                HelperFunctions.ShowMessageToUser(GetRMDMessage("MESSAGE_SELECT_RECORDS_BATCH_PROCESS"), EnumMessageTypes.Error)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("BindProcessStatusRMDRecords", ex)
    '    End Try
    'End Sub

    'Public Function CreateAndCopyRMDForm(ByVal dtDisburseRMDProcess As DataTable, ByVal blnCopyIDM As Boolean, ByVal strReportType As String, Optional ByRef dtFileList As DataTable = Nothing) As DataTable

    '    Dim dtTempDisburseRMD As DataTable
    '    Dim objBatchProcess As New BatchRequestCreation
    '    Dim dvSucessRMD As DataView

    'End Function
    'END: MMR |2016.02.11 | YRS-AT-2922 | Commented code as not used anywhere

    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String, ByVal strPerssID As String) As String
        Dim l_StringErrorMessage As String
        Dim IDM As New IDMforAll
        Try
            'Anudeep 04.12.2012 Code changes to copy report into IDM folder
            'gets the columns for idm and stored in session varilable 

            If Session("FTFileList") Is Nothing Then
                If IDM.DatatableFileList(False) Then
                    Session("FTFileList") = IDM.SetdtFileList
                End If
            End If

            If Not Session("FTFileList") Is Nothing Then
                IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"

            'IDM.PersId = Session("String_PersId")
            IDM.PersId = strPerssID
            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList


            If Not Session("FTFileList") Is Nothing Then
                Try
                    Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                Catch
                    Throw
                End Try
            End If

            Return l_StringErrorMessage

        Catch ex As Exception
            HelperFunctions.LogException("MRD Process --> Copy to IDM -->" + ex.Message.Trim.ToString(), ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Finally
            IDM = Nothing
        End Try
    End Function

    Private Sub BindEligibleRMDRecords()
        Dim strColumn() As String = {"MRDExpireDate"}
        Dim lstItem As ListItem
        Dim dtTmpEligible As DataTable
        Dim dvFilter As DataView
        Dim dtBatchRecords As DataTable
        tblEligibleRMDProcess.Visible = True
        tblNonEligibleRMDProcess.Visible = False
        tblRMDProcessStatus.Visible = False

        lbllnkEligible.Visible = True
        lnkEligiblePerson.Visible = False
        lbllnkNonEligible.Visible = False
        lnkNotEligiblePerson.Visible = True

        'lnkEligiblePerson.CssClass = "tabSelectedLink"
        'lnkNotEligiblePerson.CssClass = "tabNotSelectedLink"
        lblTabName.Text = "List of person(s) Eligible for processing RMD(s)."
        btnProcess.Visible = True
        ViewState("EligibleRMD_sort") = Nothing

        Try
            ds_EligibleRDMRecords = YMCARET.YmcaBusinessObject.MRDBO.GetRMDRecordsForBatchProcess()
            Session("dsMRD") = ds_EligibleRDMRecords
            'START: CS |2016.10.17 | YRS-AT-2476 | Displaying both plans
            ' dvFilter = ds_EligibleRDMRecords.Tables("BatchRecords").DefaultView 
            dvFilter = ds_EligibleRDMRecords.Tables("BatchRecordsDisplay").DefaultView
            'END: CS |2016.10.17 | YRS-AT-2476 | Displaying both plans
            'AA:02/04/2015 BT:2139:YRS 5.0-2165: Added # to filter the records with date format
            dvFilter.RowFilter = " PendingRequest = 0 AND IsMultipleYearMRD = 0 AND MRDExpireDate >=  '#" + ds_EligibleRDMRecords.Tables("ProcessDate").Rows(0).Item(0).ToString() + "#'"
            dtBatchRecords = dvFilter.ToTable
            If HelperFunctions.isNonEmpty(ds_EligibleRDMRecords) Then
                lblRMDLastProcessDate.Text = "Current RMD Processing Date: " + ds_EligibleRDMRecords.Tables("ProcessDate").Rows(0).Item(0).ToString()
                Session("RMDLastProcessDate") = ds_EligibleRDMRecords.Tables("ProcessDate").Rows(0).Item(0).ToString()
                If HelperFunctions.isNonEmpty(dtBatchRecords) Then

                    If Not ViewState("RMDEligibleFilter") Is Nothing Then
                        dvFilter.RowFilter = ViewState("RMDEligibleFilter")
                    End If

                    If dtBatchRecords.Rows.Count > 0 Then
                        'dtBatchRecords = ds_EligibleRDMRecords.Tables("BatchRecords")
                        HelperFunctions.BindGrid(gvRMDProcess, dvFilter, True)
                        ViewState("RMDEligible") = dtBatchRecords
                        If ddlRMDYear.Items.Count = 0 Then
                            If dtBatchRecords.Rows.Count > 0 Then
                                'dtTmpEligible = ds_EligibleRDMRecords.Tables("BatchRecords").DefaultView.ToTable(True, strColumn)
                                dtTmpEligible = dvFilter.ToTable(True, strColumn)
                                For iCount As Integer = 0 To dtTmpEligible.Rows.Count - 1
                                    lstItem = New ListItem()
                                    Dim dtRMDExpireDate As Date
                                    dtRMDExpireDate = Convert.ToDateTime(dtTmpEligible.Rows(iCount)("MRDExpireDate").ToString())
                                    If dtRMDExpireDate.Month = 3 Then
                                        lstItem.Text = "Mar - " + Convert.ToString(dtRMDExpireDate.Year)
                                    ElseIf dtRMDExpireDate.Month = 12 Then
                                        lstItem.Text = "Dec - " + Convert.ToString(dtRMDExpireDate.Year)
                                    End If
                                    lstItem.Value = dtTmpEligible.Rows(iCount)("MRDExpireDate").ToString()
                                    ddlRMDYear.Items.Add(lstItem)
                                Next
                            End If

                            If dtTmpEligible.Rows.Count > 0 Then
                                ddlRMDYear.Items(0).Selected = True
                            End If
                            lblRMDLastProcessDate.Text = "Current RMD Processing Date: " + ddlRMDYear.SelectedValue.Trim
                        End If
                    Else
                        gvRMDProcess.DataSource = Nothing
                        gvRMDProcess.DataBind()
                        ViewState("RMDEligible") = Nothing
                        HelperFunctions.ShowMessageToUser("No Record exist for current RMD Process date. Please close current RMD to process next RMD date.", EnumMessageTypes.Information)
                    End If
                Else
                    gvRMDProcess.DataSource = Nothing
                    gvRMDProcess.DataBind()
                    ViewState("RMDEligible") = Nothing
                    HelperFunctions.ShowMessageToUser("No Record exist for current RMD Process date. Please close current RMD to process next RMD date.", EnumMessageTypes.Information)
                End If
            Else
                gvRMDProcess.DataSource = Nothing
                gvRMDProcess.DataBind()
                ViewState("RMDEligible") = Nothing
                HelperFunctions.ShowMessageToUser("No Record exist for current RMD Process date. Please close current RMD to process next RMD date.", EnumMessageTypes.Information)
            End If

            If Not Session("DisburseMRDRecords") Is Nothing And gvProcessStatus.Visible Then
                btnProcess.Visible = False
                lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure."
            End If


        Catch
            Throw
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub BindNonEligibleRMDRecords()
        Dim strColumn() As String = {"MRDExpireDate"}
        Dim lstItem As ListItem
        Dim dtTmpNonEligible As DataTable
        Dim dt As DataTable

        tblNonEligibleRMDProcess.Visible = True
        tblEligibleRMDProcess.Visible = False
        tblRMDProcessStatus.Visible = False

        lbllnkEligible.Visible = False
        lnkEligiblePerson.Visible = True
        lbllnkNonEligible.Visible = True
        lnkNotEligiblePerson.Visible = False

        ViewState("NonEligibleRMD_sort") = Nothing
        'lnkEligiblePerson.CssClass = "tabNotSelectedLink"
        'lnkNotEligiblePerson.CssClass = "tabSelectedLink"
        lblTabName.Text = "List of person(s) Non Eligible for processing RMD(s)."
        btnProcess.Visible = False
        Try
            ds_NonEligibleRDMRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchNonEligibleMRDRecords(Nothing)
            If HelperFunctions.isNonEmpty(ds_NonEligibleRDMRecords) Then
                lblRMDLastProcessDate.Text = "Current RMD Processing Date: " + ds_NonEligibleRDMRecords.Tables("ProcessDate").Rows(0).Item(0).ToString()
                If HelperFunctions.isNonEmpty(ds_NonEligibleRDMRecords.Tables("BatchRecords")) Then
                    If ds_NonEligibleRDMRecords.Tables("BatchRecords").Rows.Count > 0 Then
                        ViewState("GenerateRMD_sort") = Nothing

                        dt = ds_NonEligibleRDMRecords.Tables("BatchRecords")
                        If HelperFunctions.isNonEmpty(dt) Then
                            If dt.Rows.Count > 0 Then
                                For Each dr As DataRow In dt.Rows
                                    If dr("bitAnnualMRDRequested").ToString.ToLower = "false" Then
                                        'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed "Enroll" to "Enrolled"
                                        'dr("Remarks") = "Not Enroll For Annual RMD Payment."
                                        dr("Remarks") = "Not Enrolled For Annual RMD Payment."
                                        'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed "Enroll" to "Enrolled"
                                    End If
                                    If dr("IsLocked").ToString.ToLower = "true" Then
                                        dr("Remarks") = "Account Locked."
                                    End If
                                    If dr("IsBlocked").ToString.ToLower = "1" Then
                                        dr("Remarks") = "Insufficient balance."
                                    End If
                                    If dr("IsMultipleYearMRD").ToString.ToLower = "1" Then
                                        'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed "exists" to "exist"
                                        'dr("Remarks") = "Prior period RMD's exists."
                                        dr("Remarks") = "Prior period RMD's exist."
                                        'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed "exists" to "exist"
                                    End If
                                    If dr("PendingRequest").ToString.ToLower = "1" Then
                                        dr("Remarks") = "Pending Request."
                                    End If
                                Next
                            End If
                        End If
                        Dim dv As DataView
                        dv = dt.DefaultView
                        'AA:02/04/2015 BT:2139:YRS 5.0-2165: Added # to filter the records with date format
                        dv.RowFilter = " Remarks <> '' AND MRDExpireDate >=  '#" + ds_NonEligibleRDMRecords.Tables("ProcessDate").Rows(0).Item(0).ToString() + "#'"

                        gvNonEligible.DataSource = dv.ToTable
                        gvNonEligible.DataBind()

                        ViewState("NonEligibleRMD") = dv.ToTable
                        Session("NonEligibleGrid") = gvNonEligible

                        'Bind Years
                        If ddlNonEligibleRMDYear.Items.Count = 0 Then
                            If ds_NonEligibleRDMRecords.Tables("BatchRecords").Rows.Count > 0 Then
                                dtTmpNonEligible = ds_NonEligibleRDMRecords.Tables("BatchRecords").DefaultView.ToTable(True, strColumn)
                                For iCount As Integer = 0 To dtTmpNonEligible.Rows.Count - 1
                                    lstItem = New ListItem()
                                    Dim dtRMDExpireDate As Date
                                    dtRMDExpireDate = Convert.ToDateTime(dtTmpNonEligible.Rows(iCount)("MRDExpireDate").ToString())
                                    If dtRMDExpireDate.Month = 3 Then
                                        lstItem.Text = "Mar - " + Convert.ToString(dtRMDExpireDate.Year)
                                    ElseIf dtRMDExpireDate.Month = 12 Then
                                        lstItem.Text = "Dec - " + Convert.ToString(dtRMDExpireDate.Year)
                                    End If
                                    lstItem.Value = dtTmpNonEligible.Rows(iCount)("MRDExpireDate").ToString()
                                    ddlNonEligibleRMDYear.Items.Add(lstItem)
                                Next
                            End If
                            If dtTmpNonEligible.Rows.Count > 0 Then
                                ddlNonEligibleRMDYear.Items(0).Selected = True
                            End If
                            lblRMDLastProcessDate.Text = "Current RMD Processing Date: " + ddlNonEligibleRMDYear.SelectedValue.Trim
                        End If
                    Else
                        gvNonEligible.DataSource = Nothing
                        gvNonEligible.DataBind()
                        Session("NonEligibleGrid") = Nothing
                        HelperFunctions.ShowMessageToUser("Records not found for non eligible RMD(s).", EnumMessageTypes.Information)
                    End If
                Else
                    gvNonEligible.DataSource = Nothing
                    gvNonEligible.DataBind()
                    Session("NonEligibleGrid") = Nothing
                    HelperFunctions.ShowMessageToUser("Records not found for non eligible RMD(s).", EnumMessageTypes.Information)
                End If
            Else
                gvNonEligible.DataSource = Nothing
                gvNonEligible.DataBind()
                Session("NonEligibleGrid") = Nothing
                HelperFunctions.ShowMessageToUser("Records not found for non eligible RMD(s).", EnumMessageTypes.Information)
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Function GetSelectedRMDBatchRecordsForPrintLetters() As DataTable
        Dim dtRMDBatchPrintList As New DataTable
        Dim drRMDBatchPrintList As DataRow
        dtRMDBatchPrintList.Columns.Add("FundId")
        dtRMDBatchPrintList.Columns.Add("PlanType")
        dtRMDBatchPrintList.Columns.Add("CurrentBalance")
        dtRMDBatchPrintList.Columns.Add("RMDAmount")
        dtRMDBatchPrintList.Columns.Add("PaidAmount")
        dtRMDBatchPrintList.Columns.Add("FundStatus")
        dtRMDBatchPrintList.Columns.Add("DueDate")
        dtRMDBatchPrintList.Columns.Add("Selected")
        dtRMDBatchPrintList.Columns.Add("ProcessDate")
        dtRMDBatchPrintList.Columns.Add("ReportName")

        For iCount As Integer = 0 To gvRMDProcess.Rows.Count - 1
            Dim chkBox As New CheckBox
            chkBox = CType(gvRMDProcess.Rows(iCount).FindControl("chkRMDProcessEligible"), CheckBox)
            If Not chkBox Is Nothing Then
                drRMDBatchPrintList = dtRMDBatchPrintList.NewRow
                If chkBox.Checked Then
                    drRMDBatchPrintList("Selected") = "Yes"
                Else
                    drRMDBatchPrintList("Selected") = "No"
                End If
                drRMDBatchPrintList("FundId") = gvRMDProcess.Rows(iCount).Cells(1).Text
                drRMDBatchPrintList("PlanType") = gvRMDProcess.Rows(iCount).Cells(2).Text
                drRMDBatchPrintList("CurrentBalance") = gvRMDProcess.Rows(iCount).Cells(3).Text
                drRMDBatchPrintList("RMDAmount") = gvRMDProcess.Rows(iCount).Cells(4).Text
                drRMDBatchPrintList("PaidAmount") = gvRMDProcess.Rows(iCount).Cells(5).Text
                drRMDBatchPrintList("FundStatus") = gvRMDProcess.Rows(iCount).Cells(6).Text
                drRMDBatchPrintList("DueDate") = gvRMDProcess.Rows(iCount).Cells(10).Text
                drRMDBatchPrintList("ProcessDate") = ddlRMDYear.SelectedValue.Trim
                drRMDBatchPrintList("ReportName") = "RDM Batch Process"
                dtRMDBatchPrintList.Rows.Add(drRMDBatchPrintList)
            End If
        Next
        Session("RMDBatchProcessPrintLetters") = dtRMDBatchPrintList
        Session("ReportName") = "RMDBatchProcessPrintLetterList"
        Return dtRMDBatchPrintList
    End Function

    Protected Sub lnkEligiblepersons_Click(sender As Object, e As EventArgs) Handles lnkEligiblePerson.Click
        Try
            lnkEligiblePerson.Visible = False
            lbllnkEligible.Visible = True
            lnkNotEligiblePerson.Visible = True
            lbllnkNonEligible.Visible = False
            lnkProcessStatus.Visible = True
            lblProcessStatus.Visible = False
            'btnPrintReport.Visible = True 'CS |2016.10.17 | YRS-AT-2476 | To resolve the issues of visibility of Print button
            btnPrintReport.Visible = True 'PK | 2019.04.15 | YRS-AT-3170 | YRS-AT-3170 is already resolved while, doing the changes for YRS-AT-2476. Added same line again to checkin changes under the ticket YRS-AT-3170.
            BindEligibleRMDRecords()
            SearchRMDRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("lnkEligiblepersons_Click", ex)
        End Try

    End Sub

    Protected Sub lnkNonEligiblepersons_Click(sender As Object, e As EventArgs) Handles lnkNotEligiblePerson.Click
        Try
            lnkEligiblePerson.Visible = True
            lbllnkEligible.Visible = False
            lnkNotEligiblePerson.Visible = False
            lbllnkNonEligible.Visible = True
            lnkProcessStatus.Visible = True
            lblProcessStatus.Visible = False
            'btnPrintReport.Visible = True 'CS |2016.10.17 | YRS-AT-2476 | To resolve the issues of visibility of Print button
            btnPrintReport.Visible = True 'PK | 2019.04.15 | YRS-AT-3170 | YRS-AT-3170 is already resolved while, doing the changes for YRS-AT-2476. Added same line again to checkin changes under the ticket YRS-AT-3170.
            BindNonEligibleRMDRecords()
            SearchRMDRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("lnkNonEligiblepersons_Click", ex)
        End Try
    End Sub


    Private Sub lnkProcessStatus_Click(sender As Object, e As EventArgs) Handles lnkProcessStatus.Click
        Try
            If sender IsNot Nothing AndAlso sender.Text() IsNot Nothing Then
                'START : MMR |2016.11.01 | YRS-AT-2922 | Resetting property values
                RMDYear = 0
                RMDDueDate = String.Empty
                RMDBatchID = String.Empty
                IsDecemberClosed = False
                IsMarchClosed = False
                'END : MMR |2016.11.01 | YRS-AT-2922 | Resetting property values
                BindRMDProcessStatus()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("lnkProcessStatus_Click", ex)
        End Try
    End Sub


    Private Sub BindRMDProcessStatus()
        Dim dsRMDBatchProcess As DataSet

        lnkEligiblePerson.Visible = True
        lbllnkEligible.Visible = False
        lnkNotEligiblePerson.Visible = True
        lbllnkNonEligible.Visible = False
        lnkProcessStatus.Visible = False
        lblProcessStatus.Visible = True
        tblRMDProcessStatus.Visible = True
        tblEligibleRMDProcess.Visible = False
        tblNonEligibleRMDProcess.Visible = False
        'START : MMR |2016.11.01 | YRS-AT-2922 | Changed label text for RMD batch process status
        'lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure."
        lblTabName.Text = "Process status of all the selected RMD(s), Success/Failure/Outstanding."
        'END : MMR |2016.11.01 | YRS-AT-2922 | Changed label text for RMD batch process status
        btnProcess.Visible = False
        btnPrintReport.Visible = False
        Try
            dsRMDBatchProcess = YMCARET.YmcaBusinessObject.MRDBO.GetAllTempBatchID()
            If (HelperFunctions.isNonEmpty(dsRMDBatchProcess)) Then
                'START : MMR |2016.11.01 | YRS-AT-2922 | Commented existing code and binding dropdown values and batch process status
                'rptRMDBatchId.DataSource = dsRMDBatchProcess.Tables(0)
                'rptRMDBatchId.DataBind()
                ViewState("RMDProcessDetails") = dsRMDBatchProcess
                BindRMDYearDropdown()
                BindRMDMonthDropdown(dropdownlistRMDYear.SelectedValue)
                BindRMDBatchIDDropdown(dropdownlistRMDYear.SelectedValue, dropdownlistRMDMonth.SelectedValue)
                BindRMDStatus(dropdownBatchId.SelectedItem.Text, dropdownlistRMDYear.SelectedValue, dropdownlistRMDMonth.SelectedValue, IsMarchClosed, IsDecemberClosed)
                'END : MMR |2016.11.01 | YRS-AT-2922 | Commented existing code and binding dropdown values and batch process status
            End If
        Catch
            Throw
        End Try
    End Sub

    'Private Sub GetLastProccesedDate()
    '    Dim strLastProcessedDate As String = String.Empty
    '    Try
    '        strLastProcessedDate = YMCARET.YmcaBusinessObject.MRDBO.GetLastRMDProcessedDate()
    '        ViewState("ProcessDate") = strLastProcessedDate
    '        If Not String.IsNullOrEmpty(strLastProcessedDate) Then
    '            lblRMDLastProcessDate.Text = "RMD Last Processed On : " + strLastProcessedDate
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Private Sub gvNonEligible_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvNonEligible.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            HelperFunctions.SetSortingArrows(ViewState("NonEligibleRMD_sort"), e)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'If (Convert.ToString(e.Row.Cells(12).Text.ToLower) = "true") Then
            '    e.Row.CssClass = "BG_ColourIsNotEnrollAnnualMRDPayments"
            'ElseIf (Convert.ToString(e.Row.Cells(13).Text.ToLower) = "true") Then
            '    e.Row.CssClass = "BG_ColourIsLocked"
            'ElseIf (Convert.ToString(e.Row.Cells(14).Text.ToLower) = "1") Then
            '    e.Row.CssClass = "BG_ColourIsBlocked"
            'ElseIf (Convert.ToString(e.Row.Cells(15).Text.ToLower) = "1") Then
            '    e.Row.CssClass = "BG_ColourIsMultipleYearMRD"
            'End If

            'If (Convert.ToString(e.Row.Cells(12).Text.ToLower) = "false") Then
            '    e.Row.Cells(16).Text = "Not Enroll For Annual RMD Payment."
            'End If
            'If (Convert.ToString(e.Row.Cells(13).Text.ToLower) = "true") Then
            '    e.Row.Cells(16).Text = "Account Locked."
            'End If
            'If (Convert.ToString(e.Row.Cells(14).Text.ToLower) = "true") Then
            '    e.Row.Cells(16).Text = "Insufficient balance."
            'End If
            'If (Convert.ToString(e.Row.Cells(15).Text.ToLower) = "1") Then
            '    e.Row.Cells(16).Text = "Prior period RMD's exists."
            'End If
            'If (Convert.ToString(e.Row.Cells(17).Text.ToLower) = "1") Then
            '    e.Row.Cells(16).Text = "Pending Request."
            'End If


        End If
    End Sub

    Protected Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Try
            Dim strParam(4) As String
            SearchRMDRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnView_Click", ex)
        End Try

    End Sub

    Public Function GetRMDBatchProcessPrintLetters() As DataTable
        Return CType(Session("RMDBatchProcessPrintLetters"), DataTable)
    End Function

    Public Function GetRMDBatchProcessNonEligibleList() As DataTable
        Return CType(Session("RMDBatchProcessNonEligibleList"), DataTable)
    End Function

    Protected Sub btnPrintReport_Click(sender As Object, e As EventArgs) Handles btnPrintReport.Click
        Try
            If lnkEligiblePerson.Visible = False Then
                GetSelectedRMDBatchRecordsForPrintLetters()
            ElseIf lnkNotEligiblePerson.Visible = False Then
                GetSelectedRMDBatchRecordsForNonEligible()
            End If
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnPrintReport_Click", ex)
        End Try

    End Sub

    Protected Sub gvRMDProcess_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvRMDProcess.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = CType(ViewState("RMDEligible"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If Not ViewState("RMDEligibleFilter") Is Nothing Then
                    dv.RowFilter = ViewState("RMDEligibleFilter")
                End If
                HelperFunctions.gvSorting(ViewState("EligibleRMD_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvRMDProcess, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvRMDProcess_Sorting", ex)
        End Try
    End Sub

    Protected Sub gvNonEligible_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvNonEligible.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = CType(ViewState("NonEligibleRMD"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If Not ViewState("RMDNonEligibleFilter") Is Nothing Then
                    dv.RowFilter = ViewState("RMDNonEligibleFilter")
                End If
                HelperFunctions.gvSorting(ViewState("NonEligibleRMD_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvNonEligible, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvNonEligible_Sorting", ex)
        End Try
    End Sub

    Protected Sub gvRMDProcess_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRMDProcess.RowDataBound
        Dim chkChecklist As CheckBox

        If e.Row.RowType = DataControlRowType.Header Then
            HelperFunctions.SetSortingArrows(ViewState("EligibleRMD_sort"), e)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'START : SB | 2017.01.18 | YRS-AT-3297 | Replacing hardcoded cell numbers with configurable constant variables
            ' Disabling the selection option if RMD is fully satisfled ( Paid Amount  >= MRD Amount )
            ' If (Convert.ToDecimal(e.Row.Cells(5).Text) >= Convert.ToDecimal(e.Row.Cells(4).Text)) Then
            If (Convert.ToDecimal(e.Row.Cells(iPaidAmount).Text) >= Convert.ToDecimal(e.Row.Cells(iRMDAmount).Text)) Then
                'END : SB | 2017.01.18 | YRS-AT-3297 | Replacing integer variablw with const 
                chkChecklist = e.Row.Cells(0).FindControl("chkRMDProcessEligible")
                chkChecklist.Enabled = False
                chkChecklist.Checked = False
                e.Row.Cells(0).Enabled = False
                chkChecklist.ID = "chkidChange"
            End If
            'START | SR | 2016.04.12 | YRS-AT-2843 - Highlight Recalculated RMD for Participant
            'SB | 2017.01.17 | Following code replaced by using global varaible  
            'If (e.Row.Cells(22).Text = "U") Then
            '    e.Row.BackColor = System.Drawing.Color.LightCoral
            'ElseIf (Not (String.IsNullOrEmpty(e.Row.Cells(22).Text.Trim())) And (e.Row.Cells(22).Text.Trim() <> "U")) Then
            '    e.Row.BackColor = System.Drawing.Color.LightGreen
            'End If
            If (e.Row.Cells(iRemarks).Text = "U") Then
                e.Row.BackColor = System.Drawing.Color.LightCoral
            ElseIf (Not (String.IsNullOrEmpty(e.Row.Cells(iRemarks).Text.Trim())) And (e.Row.Cells(iRemarks).Text.Trim() <> "U")) Then
                e.Row.BackColor = System.Drawing.Color.LightGreen
            End If
            'END | SR | 2016.04.12 | YRS-AT-2843 - Highlight Recalculated RMD for Participant 

            'START : SB | 2017.01.17 | YRS-AT-3297 | Highlight cashout eligible records
            If (e.Row.Cells(iCashout).Text = "1") Then
                chkChecklist = e.Row.Cells(0).FindControl("chkRMDProcessEligible")
                If Not chkChecklist Is Nothing Then
                    chkChecklist.Enabled = False
                    chkChecklist.Checked = False
                    e.Row.Cells(0).Enabled = False
                    chkChecklist.ID = "chkidChange"
                    e.Row.CssClass = "BG_ColourSubjectToCashoutLetter"
                End If
            End If
            'END : SB | 2017.01.17 | YRS-AT-3297 | Highlight cashout eligible records
        End If
    End Sub

    Protected Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        Dim dtSelectedRecords As DataTable
        Try
            dtSelectedRecords = ProcessMRD()
            If HelperFunctions.isNonEmpty(dtSelectedRecords) Then
                If dtSelectedRecords.Rows.Count > 0 Then
                    lblMessage.Text = String.Format("Are you sure you want to process RMD for the {0} record(s)?", Me.SelectedMrdRecordsCount) 'CS |2016.10.17 | YRS-AT-2476 | TO get/set the Selected MRD record count
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showConfirmdialog();", True)
                Else
                    HelperFunctions.ShowMessageToUser("Please select the records(s) to process RMD.", EnumMessageTypes.Error)
                End If
            Else
                HelperFunctions.ShowMessageToUser("Please select the records(s) to process RMD.", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnProcess_Click", ex)
        End Try
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            btnProcess.Enabled = False
            btnProcess.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 

    Public Function ProcessMRD() As DataTable
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("ProcessMRD", "Start: Select RMD records to process")
        Dim cloneDS As New DataSet
        Dim l_CheckBox As CheckBox
        Dim dt As DataTable
        Dim l_MRDRow As DataRow
        Dim blnSuccess As Boolean
        Dim alThreads As ArrayList = New ArrayList()
        Dim dtSelectedBatchRecords As New DataSet
        'START : CS |2016.10.17 | YRS-AT-2476 | Declared the object
        Dim individualPlanWiseRecords As New DataSet
        Dim batchRecordRows As DataRow()
        'END : CS |2016.10.17 | YRS-AT-2476 | Declared the object

        Try
            dt = New DataTable
            dt.Columns.Add("FUNDNO", System.Type.GetType("System.Int32"))
            dt.Columns.Add("PlanType", System.Type.GetType("System.String"))
            dt.Columns.Add("CurrentBalance", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("MRDAmount", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("PaidAmount", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("MRDYear", System.Type.GetType("System.Int32"))
            dt.Columns.Add("StatusTypeDescription", System.Type.GetType("System.String"))
            dt.Columns.Add("FundEventID", System.Type.GetType("System.String"))
            dt.Columns.Add("Process_Status", System.Type.GetType("System.String"))
            dt.Columns.Add("IsLocked", System.Type.GetType("System.Boolean"))
            dt.Columns.Add("IsBlocked", System.Type.GetType("System.Int16"))
            dt.Columns.Add("IsMultipleYearMRD", System.Type.GetType("System.Int16"))
            dt.Columns.Add("IsLess5kFirstTimeMrd", System.Type.GetType("System.Int16"))
            dt.Columns.Add("Name", System.Type.GetType("System.String"))
            dt.Columns.Add("ProcessDate", System.Type.GetType("System.String"))
            dt.Columns.Add("WithholdingPercent", System.Type.GetType("System.String"))
            dt.Columns.Add("IsPrintReport", System.Type.GetType("System.Boolean"))
            dt.Columns.Add("MRDUniqueID", System.Type.GetType("System.Int32"))
            dt.Columns.Add("PersonId", System.Type.GetType("System.String"))

            dt.Columns.Add("FirstName", System.Type.GetType("System.String"))
            dt.Columns.Add("LastName", System.Type.GetType("System.String"))
            dt.Columns.Add("MiddleName", System.Type.GetType("System.String"))
            dt.Columns.Add("SSNo", System.Type.GetType("System.String"))
            'START | SR | 2016.04.05 | YRS-AT-2843 - Add new columns to get RMD taxable andRMD Non taxable amount.
            dt.Columns.Add("Remarks", System.Type.GetType("System.String"))
            dt.Columns.Add("RmdTaxableAmount", System.Type.GetType("System.Decimal"))
            dt.Columns.Add("RmdNonTaxableAmount", System.Type.GetType("System.Decimal"))
            'END | SR | 2016.04.05 | YRS-AT-2843 - Add new columns to get RMD taxable andRMD Non taxable amount.


            Dim dscDatacolumn As New DataColumn("RefRequestId", System.Type.GetType("System.String"), "FUNDNO")

            dt.Columns.Add(dscDatacolumn)
            'START : CS |2016.10.17 | YRS-AT-2476 | Individual Planwise MRD Record
            individualPlanWiseRecords = CType(Session("dsMRD"), DataSet)
            Me.SelectedMrdRecordsCount = 0
            'END : CS |2016.10.17 | YRS-AT-2476 | Individual Planwise MRD Record
            For iCount As Integer = 0 To gvRMDProcess.Rows.Count - 1

                l_CheckBox = gvRMDProcess.Rows(iCount).FindControl("chkRMDProcessEligible")

                If Not l_CheckBox Is Nothing Then

                    If l_CheckBox.Checked Then
                        'START : CS |2016.10.17 | YRS-AT-2476 | Combined Both Plan will be converted into individual Plan
                        SelectedMrdRecordsCount = SelectedMrdRecordsCount + 1
                        If gvRMDProcess.Rows(iCount).Cells(2).Text = "BOTH" Then
                            batchRecordRows = individualPlanWiseRecords.Tables("BatchRecords").Select(String.Format("FundIdNo='{0}' AND MRDYear='{1}' AND PendingRequest = 0 AND IsMultipleYearMRD = 0 AND MRDExpireDate >=  '#" + individualPlanWiseRecords.Tables("ProcessDate").Rows(0).Item(0).ToString() + "#'", CType(gvRMDProcess.Rows(iCount).Cells(1).Text, Integer), CType(gvRMDProcess.Rows(iCount).Cells(8).Text, String)))
                            If batchRecordRows.Length > 0 Then
                                For Each drRowRecAccountbalance In batchRecordRows
                                    l_MRDRow = dt.NewRow
                                    l_MRDRow("FUNDNO") = drRowRecAccountbalance("FundIdNo").ToString()
                                    l_MRDRow("PlanType") = drRowRecAccountbalance("PlanType").ToString()
                                    l_MRDRow("CurrentBalance") = drRowRecAccountbalance("CurrentBalance").ToString()
                                    l_MRDRow("MRDAmount") = drRowRecAccountbalance("MRDAmount").ToString()
                                    l_MRDRow("PaidAmount") = drRowRecAccountbalance("PaidAmount").ToString()
                                    l_MRDRow("StatusTypeDescription") = drRowRecAccountbalance("StatusTypeDescription").ToString()
                                    l_MRDRow("FundEventID") = drRowRecAccountbalance("FundEventID").ToString()
                                    l_MRDRow("MRDYear") = drRowRecAccountbalance("MRDYear").ToString()
                                    l_MRDRow("Process_Status") = drRowRecAccountbalance("Process_Status").ToString()
                                    l_MRDRow("IsLocked") = drRowRecAccountbalance("IsLocked").ToString()
                                    l_MRDRow("IsBlocked") = drRowRecAccountbalance("IsBlocked").ToString()
                                    l_MRDRow("IsMultipleYearMRD") = drRowRecAccountbalance("IsMultipleYearMRD").ToString()
                                    l_MRDRow("IsLess5kFirstTimeMrd") = 0
                                    l_MRDRow("Name") = drRowRecAccountbalance("Name").ToString()
                                    l_MRDRow("ProcessDate") = ddlRMDYear.SelectedValue.Trim 'Session("RMDLastProcessDate").ToString.Trim
                                    l_MRDRow("WithholdingPercent") = ""
                                    l_MRDRow("IsPrintReport") = 0
                                    l_MRDRow("MRDUniqueID") = drRowRecAccountbalance("MRDUniqueID").ToString()
                                    l_MRDRow("PersonId") = drRowRecAccountbalance("PerssID").ToString()
                                    l_MRDRow("FirstName") = drRowRecAccountbalance("FirstName").ToString()
                                    l_MRDRow("LastName") = drRowRecAccountbalance("LastName").ToString()
                                    l_MRDRow("MiddleName") = drRowRecAccountbalance("MiddleName").ToString()
                                    l_MRDRow("SSNo") = drRowRecAccountbalance("SSNO").ToString()
                                    l_MRDRow("Remarks") = drRowRecAccountbalance("Remarks").ToString()
                                    l_MRDRow("RmdTaxableAmount") = drRowRecAccountbalance("RmdTaxableAmount").ToString()
                                    l_MRDRow("RmdNonTaxableAmount") = drRowRecAccountbalance("RmdNonTaxableAmount").ToString()
                                    dt.Rows.Add(l_MRDRow)
                                Next
                            End If
                            'END : CS |2016.10.17 | YRS-AT-2476 | Combined Both Plan will be converted into individual Plan
                        Else
                            l_MRDRow = dt.NewRow
                            l_MRDRow("FUNDNO") = gvRMDProcess.Rows(iCount).Cells(1).Text
                            l_MRDRow("PlanType") = gvRMDProcess.Rows(iCount).Cells(2).Text
                            l_MRDRow("CurrentBalance") = gvRMDProcess.Rows(iCount).Cells(3).Text
                            l_MRDRow("MRDAmount") = gvRMDProcess.Rows(iCount).Cells(4).Text
                            l_MRDRow("PaidAmount") = gvRMDProcess.Rows(iCount).Cells(5).Text
                            l_MRDRow("StatusTypeDescription") = gvRMDProcess.Rows(iCount).Cells(6).Text
                            l_MRDRow("FundEventID") = gvRMDProcess.Rows(iCount).Cells(7).Text
                            l_MRDRow("MRDYear") = gvRMDProcess.Rows(iCount).Cells(8).Text
                            l_MRDRow("Process_Status") = gvRMDProcess.Rows(iCount).Cells(9).Text
                            l_MRDRow("IsLocked") = gvRMDProcess.Rows(iCount).Cells(13).Text
                            l_MRDRow("IsBlocked") = gvRMDProcess.Rows(iCount).Cells(14).Text
                            l_MRDRow("IsMultipleYearMRD") = gvRMDProcess.Rows(iCount).Cells(15).Text
                            l_MRDRow("IsLess5kFirstTimeMrd") = 0
                            l_MRDRow("Name") = gvRMDProcess.Rows(iCount).Cells(21).Text
                            l_MRDRow("ProcessDate") = ddlRMDYear.SelectedValue.Trim 'Session("RMDLastProcessDate").ToString.Trim
                            l_MRDRow("WithholdingPercent") = ""
                            l_MRDRow("IsPrintReport") = 0
                            'START | SR | 2016.04.05 | YRS-AT-2843 - Get RMD Unique id to update recalculated RMD for sole spousal Beneficiary more than 10 yrs younger.
                            'l_MRDRow("MRDUniqueID") = 0
                            l_MRDRow("MRDUniqueID") = gvRMDProcess.Rows(iCount).Cells(16).Text
                            'END | SR | 2016.04.05 | YRS-AT-2843 - Get RMD Unique id to update recalculated RMD for sole spousal Beneficiary more than 10 yrs younger.
                            l_MRDRow("PersonId") = gvRMDProcess.Rows(iCount).Cells(11).Text
                            l_MRDRow("FirstName") = gvRMDProcess.Rows(iCount).Cells(17).Text
                            l_MRDRow("LastName") = gvRMDProcess.Rows(iCount).Cells(18).Text
                            l_MRDRow("MiddleName") = gvRMDProcess.Rows(iCount).Cells(19).Text
                            l_MRDRow("SSNo") = gvRMDProcess.Rows(iCount).Cells(20).Text
                            'START | SR | 2016.04.05 | YRS-AT-2843 - Get Remarks, RMD taxable and RMD Non taxable amount
                            l_MRDRow("Remarks") = gvRMDProcess.Rows(iCount).Cells(22).Text
                            l_MRDRow("RmdTaxableAmount") = gvRMDProcess.Rows(iCount).Cells(23).Text
                            l_MRDRow("RmdNonTaxableAmount") = gvRMDProcess.Rows(iCount).Cells(24).Text
                            'END | SR | 2016.04.05 | YRS-AT-2843 - Get Remarks, RMD taxable and RMD Non taxable amount
                            dt.Rows.Add(l_MRDRow)
                        End If
                    End If
                End If
            Next


            dt.AcceptChanges()
            dt.TableName = "SelectedBatchRecords"
            dtSelectedBatchRecords.Tables.Add(dt)
            Session("SelectedRecords") = dtSelectedBatchRecords

            Return dt
        Catch
            Throw
        End Try
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("ProcessMRD", "Finish: Select RMD records to process")
    End Function

    Private Function GetDataRow(ByVal gridItem As GridViewRow) As DataRow()
        Try
            Dim ds_MRDRecords As DataSet
            Dim dr As DataRow()
            'If Not Session("dsMRD") Is Nothing Then
            '    ds_MRDRecords = CType(Session("dsMRD"), DataSet)
            '    dr = ds_MRDRecords.Tables("BatchRecords").Select("FundIdNo='" & CType(gridItem.Cells.Item(1).Text.Trim, Integer) & "' And PlanType='" & CType(gridItem.Cells.Item(2).Text.Trim, String) & "' ")
            'End If
            If Not ViewState("RMDEligible") Is Nothing Then
                ds_MRDRecords = CType(Session("dsMRD"), DataSet)
                dr = ds_MRDRecords.Tables("BatchRecords").Select("FundIdNo='" & CType(gridItem.Cells.Item(1).Text.Trim, Integer) & "' And PlanType='" & CType(gridItem.Cells.Item(2).Text.Trim, String) & "' ")
            End If

            Return dr
        Catch
            Throw
        End Try
    End Function

    Private Function CreateRMDRequestAndProcessStatus(ByVal dr As DataRow, ByVal strBatchId As String, ByRef detailRecord As DataSet) 'CS | 2016.10.17 | YRS-AT-2476 | Added "detailRecord"  Parameter to carry out MRD Record
        Dim objService As New YRSWebService.YRSWithdrawalService
        Dim objWebServiceReturn As New YRSWebService.WebServiceReturn
        Dim exMsg As Exception
        Dim strVal As String
        Dim records As DataRow() 'CS | 2016.10.17 | YRS-AT-2476
        objService.PreAuthenticate = True
        objService.Credentials = System.Net.CredentialCache.DefaultCredentials
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRMDRequestAndProcessStatus", "Start: CreateRMDRequestAndProcessStatus method call")
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestAndProcess", "Start: Web Service method SaveRefundRequestAndProcess called")
        objWebServiceReturn = objService.SaveRefundRequestAndProcess(SaveArraylistPropertys(dr("FundEventID").ToString(), dr("PlanType").ToString(), dr("MRDYear").ToString(), dr("ProcessDate").ToString()), True)
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestAndProcess", "Finish: Web Service method SaveRefundRequestAndProcess called")
        If objWebServiceReturn.ReturnStatus.Equals(YRSWebService.Status.Success) Then
            dr("Process_Status") = "Success : RMD Process completed Successfully for FundId No. - " + dr("FUNDNO").ToString()
            dr("IsPrintReport") = True
            Session("RMDFundNo") += "|" + dr("FUNDNO").ToString()
            Logger.Write("Success", "Application", 0, 0, System.Diagnostics.TraceEventType.Verbose)
            'START : CS |2016.10.17 | YRS-AT-2476 | Both plan record will get converted into individual plan before going into staging table
            records = detailRecord.Tables("SelectedBatchRecords").Select(String.Format("FUNDNO='{0}' AND MRDYear='{1}'", CType(dr("FUNDNO"), Integer), CType(dr("MRDYear"), String)))
            If HelperFunctions.isNonEmpty(records) Then
                For Each drRows As DataRow In records
                    drRows("Process_Status") = String.Format("Success : RMD Process completed Successfully for FundId No. - {0} ", drRows("FUNDNO").ToString())
                    drRows("IsPrintReport") = True
                    strVal = YMCARET.YmcaBusinessObject.MRDBO.InsertStagingLogs(strBatchId, drRows("FundEventID").ToString(), drRows("FundEventID").ToString(), drRows("MRDUniqueID").ToString(), drRows("FUNDNO").ToString(), "RDMProcess")
                Next
                detailRecord.AcceptChanges()
            End If
            'END : CS |2016.10.17 | YRS-AT-2476 | Both plan record will get converted into individual plan before going into staging table
            'TODO

        ElseIf objWebServiceReturn.ReturnStatus.Equals(YRSWebService.Status.Warning) Then
            dr("Process_Status") = "Warning :" + objWebServiceReturn.Message
            dr("IsPrintReport") = True
            exMsg = New Exception(objWebServiceReturn.Message)
            ExceptionPolicy.HandleException(exMsg, "Exception Policy")
        Else
            dr("Process_Status") = "Failed : " + objWebServiceReturn.Message
            dr("IsPrintReport") = False
            exMsg = New Exception(objWebServiceReturn.Message)
            ExceptionPolicy.HandleException(exMsg, "Exception Policy")
        End If
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRMDRequestAndProcessStatus", "Finish: CreateRMDRequestAndProcessStatus method call")
    End Function

    'Start:Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations
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

    Private Function Process(ByVal strBatchId As String, ByVal strReportType As String, ByVal iCount As Integer, ByVal strProcessName As String, dsTemp As DataSet, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
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
        Dim dt As DataTable
        'START : CS |2016.10.17 | YRS-AT-2476 | Declaring  object
        Dim combinedPlanTypeRecord As DataTable
        Dim records As DataRow()
        Dim tempRecords As DataSet
        Dim planTypeRecords As DataTable
        'END : CS |2016.10.17 | YRS-AT-2476 | Declaring  object
        Dim BatchSize As Integer = GetBatchSize() ' Dinesh Kanojia     12/10/2014      BT-2732:Cashout observations

        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: Process method call")
        Try
            objReturnStatusValues.strBatchId = strBatchId
            objReturnStatusValues.strReportType = strReportType
            objReturnStatusValues.iProcessCount = iCount
            objReturnStatusValues.strretValue = "pending"
            objReturnStatusValues.iIdxCreated = iIDXCreated
            objReturnStatusValues.iPdfCreated = iPDFCreated
            'START : CS |2016.10.17 | YRS-AT-2476 | To get Combined Plan wise MRD Record
            planTypeRecords = dsTemp.Tables("SelectedBatchRecords")
            tempRecords = YMCARET.YmcaBusinessObject.MRDBO.GetMrdRecordPlanWise(strBatchId, strModule)
            dtSelectedBatchRecords = tempRecords.Tables("BatchMRDRecords")
            'END : CS |2016.10.17 | YRS-AT-2476 | To get Combined Plan wise MRD Record

            Dim dtArrErrorDataList As DataTable = dsTemp.Tables("ArrErrorDataList")
            If Not dtArrErrorDataList Is Nothing Then
                For Each dr As DataRow In dtArrErrorDataList.Rows
                    ArrErrorDataList.Add(New ExceptionLog(dr("FundNo"), dr("Errors"), dr("Description")))
                Next
            End If
            'START : CS |2016.10.17 | YRS-AT-2476 | add the new column in the  MRD Record Combined Plan wise
            If Not planTypeRecords.Columns.Contains("IsReportPrinted") Then
                planTypeRecords.Columns.Add("IsReportPrinted")
            End If
            'END : CS |2016.10.17 | YRS-AT-2476 | add the new column in the  MRD Record Combined Plan wise

            If Not dtSelectedBatchRecords.Columns.Contains("IsReportPrinted") Then
                dtSelectedBatchRecords.Columns.Add("IsReportPrinted")
            End If

            If Not dtSelectedBatchRecords Is Nothing Then

                For i = iCount To dtSelectedBatchRecords.Rows.Count - 1
                    If i - iCount >= BatchSize Then
                        Exit For
                    End If
                    'START | SR | 2016.04.05 | YRS-AT-2843 - Update Recalculated RMD for Participant 
                    'START : CS |2016.10.17 | YRS-AT-2476 | Individual Planwise MRD Record
                    records = planTypeRecords.Select(String.Format("FUNDNO='{0}' AND MRDYear='{1}'", CType(dtSelectedBatchRecords.Rows(i)("FUNDNO"), Integer), CType(dtSelectedBatchRecords.Rows(i)("MRDYear"), String)))
                    If (dtSelectedBatchRecords.Rows(i)("Remarks").ToString.Trim() = "U") Then 'Check whether RMD is recalculated
                        For Each dr As DataRow In records
                            YMCARET.YmcaBusinessObject.MRDBO.UpdateParticipantRMDforSolePrimaryBeneficiary(Convert.ToInt32(dr("MRDUniqueID").ToString()), Convert.ToDecimal(dr("RmdTaxableAmount").ToString()), Convert.ToDecimal(dr("RmdNonTaxableAmount").ToString()))
                        Next
                    End If
                    'END : CS |2016.10.17 | YRS-AT-2476 | Individual Planwise MRD Record
                    'END | SR | 2016.04.05 | YRS-AT-2843 - Update Recalculated RMD for Participant 
                    CreateRMDRequestAndProcessStatus(dtSelectedBatchRecords.Rows(i), strBatchId, dsTemp)
                Next
                'START : CS |2016.10.17 | YRS-AT-2476 | Filter Print status in the  MRD Record Combined Plan wise datatable
                Dim combinedPlanView As New DataView(dtSelectedBatchRecords)
                combinedPlanView.RowFilter = " IsPrintReport = true and IsReportPrinted is null "
                combinedPlanTypeRecord = combinedPlanView.ToTable
                'END : CS |2016.10.17 | YRS-AT-2476 | Filter Print status in the  MRD Record Combined Plan wise datatable

                'START : CS |2016.10.17 | YRS-AT-2476 | View is created on plan wise records
                'Dim dv As New DataView(dtSelectedBatchRecords)
                Dim dv As New DataView(planTypeRecords)
                'END : CS |2016.10.17 | YRS-AT-2476 | View is created on plan wise records
                dv.RowFilter = " IsPrintReport = true and IsReportPrinted is null "

                dt = dv.ToTable

                For Each dr As DataRow In dtSelectedBatchRecords.Rows
                    If dr("IsPrintReport") = True Then
                        dr("IsReportPrinted") = "1"
                    End If
                Next
                'START : CS |2016.10.17 | YRS-AT-2476 |  update Print status in the  MRD Record Combined Plan wise datatable
                dtSelectedBatchRecords.AcceptChanges()
                For Each dr As DataRow In planTypeRecords.Rows
                    If dr("IsPrintReport") = True Then
                        dr("IsReportPrinted") = "1"
                    End If
                Next
                dsTemp.AcceptChanges()
                'END : CS |2016.10.17 | YRS-AT-2476 |  update Print status in the  MRD Record Combined Plan wise datatable

                If HelperFunctions.isNonEmpty(dt) Then
                    Dim l_stringDocType As String = String.Empty
                    Dim l_StringReportName As String = String.Empty
                    Dim l_string_OutputFileType As String = String.Empty
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: Assign Report type")
                    If strReportType = "GraceRMDReport" Then
                        l_stringDocType = "RMDAPRLT"
                        l_StringReportName = "RMD Cover letter for April check.rpt"
                        l_string_OutputFileType = "RMD Cover letter for April check_" + l_stringDocType
                    ElseIf strReportType = "RegularRMDReport" Then
                        l_stringDocType = "RMDDECLT"
                        l_StringReportName = "RMD Cover letter for December check.rpt"
                        l_string_OutputFileType = "RMD Cover letter for December check_" + l_stringDocType
                    End If
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Assign Report type")
                    Session("strReportName") = l_StringReportName.Substring(0, l_StringReportName.Length - 4)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: InvokeBatchRequestCreation")
                    'START: CS |2016.10.17 | YRS-AT-2476 | Passing Combined  MRD Record to Print letter
                    'strBatchError = objBatchProcess.InvokeBatchRequestCreation(0, dt, l_stringDocType, l_StringReportName, l_string_OutputFileType, strReportType, ArrErrorDataList, dtFileList, iIDXCreated, iPDFCreated)
                    strBatchError = objBatchProcess.InvokeBatchRequestCreation(0, combinedPlanTypeRecord, l_stringDocType, l_StringReportName, l_string_OutputFileType, strReportType, ArrErrorDataList, dtFileList, iIDXCreated, iPDFCreated)
                    'END: CS |2016.10.17 | YRS-AT-2476 | Passing Combined  MRD Record to Print letter
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: InvokeBatchRequestCreation")
                End If
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
            'objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count
            'objReturnStatusValues.strretValue = "success"
            'Dim strFundNo As String = String.Empty
            'strFundNo = Session("RMDFundNo")

            Dim strFundNo As String = String.Empty
            If objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count Then
                objReturnStatusValues.strretValue = "success"
                strFundNo = Session("RMDFundNo")
            Else
                objReturnStatusValues.strretValue = "pending"
            End If

            objReturnStatusValues.iProcessCount = dtSelectedBatchRecords.Rows.Count

            'End: Dinesh Kanojia     12/12/2014      BT:2737:Withdrawal observations


            If HelperFunctions.isNonEmpty(dsTemp) Then
                If HelperFunctions.isNonEmpty(dtFileList) Then
                    If Not dsTemp.Tables.Contains("dtFileList") Then
                        dtFileList.TableName = "dtFileList"
                        dsTemp.Tables.Add(dtFileList)
                    End If
                    dsTemp.Tables("dtFileList").Merge(dtFileList)

                    If dsTemp.Tables("dtFileList").Rows.Count > 0 Then
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Start: Merging PDF files")
                        objBatchProcess.MergePDFs(dsTemp.Tables("dtFileList"))
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Merging PDF files")
                    End If
                End If
            End If

            If Not strFundNo Is Nothing Then
                If (strFundNo.Length > 1) Then
                    strFundNo.Remove(0, 1)
                End If
                Session("RMDFundNo") = strFundNo
            Else
                Throw New Exception("None of the records is been processed for RMD batch process. Please refer the RMD Process Status tab for details.")
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Process RMD", ex)
            objReturnStatusValues.strretValue = "error"
            ArrErrorDataList.Add(New ExceptionLog("Exception", "Process Method", ex.Message))
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

            'If HelperFunctions.isNonEmpty(dtFileList) Then
            '    If Not dsTemp.Tables.Contains("dtFileList") Then
            '        dtFileList.TableName = "dtFileList"
            '        dsTemp.Tables.Add(dtFileList)
            '    End If
            '    dsTemp.Tables("dtFileList").Merge(dtFileList)
            'End If

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
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("Process", "Finish: Process method call")
        Return objReturnStatusValues

    End Function

    Private Function GetRMDMessage(ByVal stMessageCode As String) As String

        Dim l_Message As String
        l_Message = String.Empty

        If stMessageCode = "MESSAGE_CLOSE_RMD_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSE_RMD_PROCESS
        End If

        If stMessageCode = "MESSAGE_RMD_DATE_BLANK" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_RMD_DATE_BLANK
        End If

        If stMessageCode = "MESSAGE_CLOSED_SUCCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSED_SUCCESS
        End If

        If stMessageCode = "MESSAGE_CLOSEED_OR_NODATE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSEED_OR_NODATE
        End If

        If stMessageCode = "MESSAGE_BATCH_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_BATCH_PROCESS
        End If

        If stMessageCode = "MESSAGE_SELECT_RECORDS_BATCH_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_SELECT_RECORDS_BATCH_PROCESS
        End If

        If stMessageCode = "MESSAGE_CONFIRM_CLOSE_RMD_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CONFIRM_CLOSE_RMD_PROCESS
        End If

        If stMessageCode = "MESSAGE_LOAD_RECORDS_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_LOAD_RECORDS_PROCESS
        End If

        If stMessageCode = "MESSAGE_UNSATISFIED_RMD_CONTINUE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_UNSATISFIED_RMD_CONTINUE
        End If

        'Start :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
        If stMessageCode = "MESSAGE_PRINT_LETTER" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_PRINT_LETTER
        End If

        If stMessageCode = "MESSAGE_SELECT_INITIAL_LETTERS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_SELECT_INITIAL_LETTERS
        End If

        If stMessageCode = "MESSAGE_ELIGIBLE_INITIAL_LETTER" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_ELIGIBLE_INITIAL_LETTER
        End If

        'End :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
        Return l_Message

    End Function

    Private Function SaveArraylistPropertys(ByVal FundID As String, ByVal PlanType As String, ByVal MRDYear As Integer, ByVal MRDProcessingDate As String) As String
        Try
            Dim objProperty As New SavePropertys
            Dim ArraySlectedRetirementAccounts As String()
            Dim ArraySlectedSavingsAccounts As String()
            Dim RetirementRefundType As String
            Dim SavingsRefundType As String
            Dim RetirementMRDYear As Integer
            Dim SavingsMRDYear As Integer

            If PlanType = "RETIREMENT" Then
                RetirementRefundType = "MRD"
                RetirementMRDYear = MRDYear
                SavingsRefundType = String.Empty
                SavingsMRDYear = 0
            End If

            If PlanType = "SAVINGS" Then
                SavingsRefundType = "MRD"
                SavingsMRDYear = MRDYear
                RetirementRefundType = String.Empty
                RetirementMRDYear = 0
            End If
            'START : CS |2016.10.17 | YRS-AT-2476 | To Create and Process the RMD Request for Both Plan
            If PlanType = "BOTH" Then
                RetirementRefundType = "MRD"
                SavingsMRDYear = MRDYear
                SavingsRefundType = "MRD"
                RetirementMRDYear = MRDYear
            End If
            'END : CS |2016.10.17 | YRS-AT-2476 | To Create and Process the RMD Request for Both Plan
            objProperty.FundEventID = FundID
            objProperty.RetirementPlanWithdrawalType = RetirementRefundType
            objProperty.SavingsPlanWithdrawalType = SavingsRefundType

            objProperty.SelectedRetirementPlanAccounts = ArraySlectedRetirementAccounts
            objProperty.SelectedSavingsPlanAccounts = ArraySlectedSavingsAccounts

            objProperty.RetirementPlanPartialAmount = 0
            objProperty.SavingsPlanPartialAmount = 0

            If (RetirementRefundType = "MRD") Then
                objProperty.RetirementMRDYear = RetirementMRDYear
            End If
            If (SavingsRefundType = "MRD") Then
                objProperty.SavingsMRDYear = SavingsMRDYear
            End If

            objProperty.RMDProcessingDate = MRDProcessingDate

            Dim strPropertysXML As String
            strPropertysXML = ConvertToXML(objProperty)
            strPropertysXML = strPropertysXML.Replace("<string>", "<AcctId>")
            strPropertysXML = strPropertysXML.Replace("</string>", "</AcctId>")

            Return strPropertysXML
        Catch
            Throw
        End Try
    End Function

    Public Function ConvertToXML(ByVal objCl As Object) As String
        Dim objXml As New XmlSerializer(objCl.GetType())

        Dim objSW As New StringWriter
        objXml.Serialize(objSW, objCl)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(objSW.ToString())

        Dim XmlStr As String = ""
        XmlStr += "<" & xmlDoc.DocumentElement.Name & ">"
        XmlStr += xmlDoc.DocumentElement.InnerXml
        XmlStr += "</" & xmlDoc.DocumentElement.Name & ">"

        Return XmlStr
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function RMDProcess(ByVal strBatchId As String, ByVal strReportType As String, ByVal iCount As Integer, ByVal strProcessName As String, ByVal iIDXCreated As Integer, ByVal iPDFCreated As Integer, strModule As String) As ReturnStatusValues
        Dim strretval As String = String.Empty
        Dim dsTemp As DataSet
        Dim objReturnStatusValues = New ReturnStatusValues()
        Dim objRMDBatchRequestnadProcessing As New RMDBatchRequestAndProcessing
        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RMDProcess", "RMD Batch Request and Processing Process Initiate for BatchID: " + strBatchId)
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("RMDProcess", "Start: Get data from temp table")
            dsTemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("RMDProcess", "Finish: Get data from temp table")
            objReturnStatusValues = objRMDBatchRequestnadProcessing.Process(strBatchId, strReportType, iCount, strProcessName, dsTemp, iIDXCreated, iPDFCreated, strModule)
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)

            If HelperFunctions.isNonEmpty(dsTemp.Tables("dtFileList")) Then
                HttpContext.Current.Session("FTFileList") = dsTemp.Tables("dtFileList")
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Process RMD", ex)
            objReturnStatusValues.strretValue = "error"
            Return objReturnStatusValues
            Throw ex
        End Try
        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Return objReturnStatusValues
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetStatus(ByVal strBatchId As String) As ReturnStatusValues
        Dim strReturnHtml As String = String.Empty
        Dim dtStatingLogs As DataSet
        dtStatingLogs = YMCARET.YmcaBusinessObject.MRDBO.GetStagingDetails(strBatchId, "RDMProcess")
        Dim objReturnStatusValues = New ReturnStatusValues()

        objReturnStatusValues.iProcessCount = Convert.ToInt32(dtStatingLogs.Tables(0).Rows.Count.ToString)
        'objReturnStatusValues.iTotalCount = Convert.ToInt32(strTotalCount)
        objReturnStatusValues.iIdxCopied = Convert.ToInt32(dtStatingLogs.Tables(0).Select("bitIdxCopied=1").Count)
        objReturnStatusValues.iIdxCreated = Convert.ToInt32(dtStatingLogs.Tables(0).Select("bitIdxCreated=1").Count)
        objReturnStatusValues.iPdfCopied = Convert.ToInt32(dtStatingLogs.Tables(0).Select("bitPdfCopied=1").Count)
        objReturnStatusValues.iPdfCreated = Convert.ToInt32(dtStatingLogs.Tables(0).Select("bitPdfCreated=1").Count)

        Return objReturnStatusValues
    End Function

    <System.Web.Services.WebMethod()> _
    Public Function GetIDMTrackingLogs() As IDMStatus()
        Dim dt As DataTable
        Dim objListIDMStatus As New List(Of IDMStatus)
        ' dt = YMCARET.YmcaBusinessObject.MRDBO.GetIDMTrackingLogDetails().Tables(0)
        If HelperFunctions.isNonEmpty(dt) Then
            For Each dtRows As DataRow In dt.Rows
                Dim ObjIdm As New IDMStatus
                ObjIdm.Copied = dtRows("Copied")
                ObjIdm.IdxCreated = dtRows("IdxCreated")
                ObjIdm.PdfCreated = dtRows("PdfCreated")
                objListIDMStatus.Add(ObjIdm)
            Next
        End If
        Return objListIDMStatus.ToArray
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Session("RMDClose") = True
            Response.Redirect("RMDBatchRequestAndProcessing.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnClose_Click", ex)
        End Try
    End Sub

    Private Sub gvProcessStatus_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvProcessStatus.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            HelperFunctions.SetSortingArrows(ViewState("ProcessStatus_sort"), e)
        End If
    End Sub

    Private Sub gvProcessStatus_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvProcessStatus.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = CType(ViewState("ProcessStatusRMDRecords"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                HelperFunctions.gvSorting(ViewState("ProcessStatus_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvProcessStatus, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvProcessStatus_Sorting", ex)
        End Try
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Dim strBatchId As String = String.Empty
        Dim dtRMDLastProcessDate As Date
        Dim strReportProcessName As String
        Dim dsTemp As DataSet
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Yes Button Click RMD process initiated")
            If Not Session("SelectedRecords") Is Nothing Then
                If Not Session("RMDLastProcessDate") Is Nothing Then
                    Dim strModule As String = "RMDBatchProcess"
                    ' strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.MRDBO.GetNextBatchId(Session("RMDLastProcessDate")).Tables(0).Rows(0)(0))
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Method call GetNextBatchId")
                    strBatchId = Convert.ToString(YMCARET.YmcaBusinessObject.MRDBO.GetNextBatchId(ddlRMDYear.SelectedValue.Trim).Tables(0).Rows(0)(0))
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("btnYes_Click", "RMD Batch Request and Processing Process Initiate for BatchID: " + strBatchId)
                    Session("strBatchId") = strBatchId
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Method call GetNextBatchId")
                    dtRMDLastProcessDate = CType(ddlRMDYear.SelectedValue.Trim, Date)
                    dsTemp = CType(Session("SelectedRecords"), DataSet)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Method call InsertAtsTemp")
                    YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dsTemp)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Method call InsertAtsTemp")
                    Select Case dtRMDLastProcessDate.Month
                        Case 3
                            strReportProcessName = "GraceRMDReport"
                        Case 12
                            strReportProcessName = "RegularRMDReport"
                        Case Else
                            Throw New Exception("Unable to determine the type of RMD report to generate.")
                    End Select
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Start: Calling AjaxMethod")
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "RMDPROCESS", "CallProcess('" + strBatchId + "', '" + strReportProcessName + "','" + strModule + "');", True)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Calling AjaxMethod")
                    'START : MMR |2016.11.01 | YRS-AT-2922 | setting property values to get batch process status details after processing batch
                    If dtRMDLastProcessDate.Month = 3 Then
                        RMDYear = dtRMDLastProcessDate.Year - 1
                    Else
                        RMDYear = dtRMDLastProcessDate.Year
                    End If
                    RMDDueDate = dtRMDLastProcessDate.ToString("MM/dd/yyyy")
                    RMDBatchID = strBatchId
                    'END : MMR |2016.11.01 | YRS-AT-2922 | setting property values to get batch process status details after processing batch
                Else
                    Throw New Exception("RMDLast Process Date not found.")
                End If
            Else
                HelperFunctions.ShowMessageToUser("Please select the person(s) to print the letter", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnYes_Click", ex)
        End Try
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("btnYes_Click", "Finish: Yes Button Click RMD process initiated")
        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
    End Sub

    Private Sub btnCloseCurrentMRD_Click(sender As Object, e As EventArgs) Handles btnCloseCurrentMRD.Click
        Try
            Response.Redirect("RMDCloseForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnCloseCurrentMRD_Click", ex)
        End Try
    End Sub

    Private Sub btnRMDFilter_Click(sender As Object, e As EventArgs) Handles btnRMDFilter.Click
        Try
            SearchRMDRecords()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnRMDFilter_Click", ex)
        End Try
    End Sub

    'START : MMR |2016.11.01 | YRS-AT-2922 | Commented existing code as not required
    'Protected Sub lnkRMDProcessBatchId_Click(sender As Object, e As EventArgs)
    '    Dim strBatchId As String
    '    If Not sender Is Nothing AndAlso sender.Text() IsNot Nothing Then
    '        strBatchId = sender.Text()
    '        BindRMDStatus(strBatchId)
    '    End If
    'End Sub
    'END : MMR |2016.11.01 | YRS-AT-2922 | Commented existing code as not required

    Private Sub BindRMDStatus(strBatchId As String, RMDYear As Integer, RMDDueDate As String, IsMarchClosed As Boolean, IsDecemberClosed As Boolean) 'MMR | 2016.10.27 | YRS-AT-2922 | Added parameters to filter criteria for searching RMDBatch process status
        Dim dsBatchProcess As DataSet
        Try
            If Not String.IsNullOrEmpty(strBatchId) Then
                'START : CS | 2016.10.17 | YRS-AT-2476 | To displaying Combined Plan wise MRD Record
                ' dsBatchProcess = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, BatchProcess.RMDBatchProcess.ToString)
                'MMR | 2016.10.27 | YRS-AT-2922 | Commented Existing code and called method with new parameters
                'dsBatchProcess = YMCARET.YmcaBusinessObject.MRDBO.GetMrdRecordPlanWise(strBatchId, BatchProcess.RMDBatchProcess.ToString)
                dsBatchProcess = YMCARET.YmcaBusinessObject.MRDBO.GetMrdRecordProcessStatus(strBatchId, RMDYear, RMDDueDate, IsMarchClosed, IsDecemberClosed) 'MMR | 2016.10.27 | YRS-AT-2922 | Added parameters to filter criteria for searching RMDBatch process status
                'END : CS | 2016.10.17 | YRS-AT-2476 | To displaying Combined Plan wise MRD Record
                'MMR | 2016.10.27 | YRS-AT-2922 | Commented Existing code and called method with new parameters
                'HighlightSubMenu(strBatchId)
                If HelperFunctions.isNonEmpty(dsBatchProcess) Then
                    'START : CS | 2016.10.17 | YRS-AT-2476 | To displaying Combined Plan wise MRD Record
                    ' If dsBatchProcess.Tables.Contains("SelectedBatchRecords") Then
                    If dsBatchProcess.Tables.Contains("BatchMRDRecords") Then
                        ' END : CS | 2016.10.17 | YRS-AT-2476 | To displaying Combined Plan wise MRD Record
                        Session("gvProcessBatchStatus_sort") = Nothing
                        'START : CS | 2016.10.17 | YRS-AT-2476 | To displaying Combined Plan wise MRD Record
                        'gvProcessBatchStatus.DataSource = dsBatchProcess.Tables("SelectedBatchRecords")
                        gvProcessBatchStatus.DataSource = dsBatchProcess.Tables("BatchMRDRecords")
                        'END : CS |2016.10.17 | YRS-AT-2476 | To displaying Combined Planwise MRD Record

                        gvProcessBatchStatus.DataBind()
                        'START : CS |2016.10.17 | YRS-AT-2476 | To displaying Combined Planwise MRD Record
                        'Session("gvProcessBatchStatus") = dsBatchProcess.Tables("SelectedBatchRecords")
                        Session("gvProcessBatchStatus") = dsBatchProcess.Tables("BatchMRDRecords")
                        'END : CS |2016.10.17 | YRS-AT-2476 | To displaying Combined Planwise MRD Record
                    Else
                        gvProcessBatchStatus.EmptyDataText = "No records found."
                        Session("gvProcessBatchStatus") = Nothing
                        gvProcessBatchStatus.DataSource = Nothing
                        gvProcessBatchStatus.DataBind()
                    End If

                    If dsBatchProcess.Tables.Contains("ArrErrorDataList") Then
                        Session("ArrErrorDataList_sort") = Nothing
                        gvRMDBatchErrorList.DataSource = dsBatchProcess.Tables("ArrErrorDataList")
                        gvRMDBatchErrorList.DataBind()
                        Session("ArrErrorDataList") = dsBatchProcess.Tables("ArrErrorDataList")
                    Else
                        gvRMDBatchErrorList.EmptyDataText = "No records found."
                        Session("ArrErrorDataList") = Nothing
                        gvRMDBatchErrorList.DataSource = Nothing
                        gvRMDBatchErrorList.DataBind()
                    End If
                Else
                    'START : MMR |2016.11.01 | YRS-AT-2922 | Showing message if no record found to bind grid
                    gvProcessBatchStatus.EmptyDataText = "No records found."
                    Session("gvProcessBatchStatus") = Nothing
                    gvProcessBatchStatus.DataSource = Nothing
                    gvProcessBatchStatus.DataBind()

                    gvRMDBatchErrorList.EmptyDataText = "No records found."
                    Session("ArrErrorDataList") = Nothing
                    gvRMDBatchErrorList.DataSource = Nothing
                    gvRMDBatchErrorList.DataBind()
                    'END : MMR |2016.11.01 | YRS-AT-2922 | Showing message if no record found to bind grid
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    'START: MMR | 2016.10.24 | YRS-AT-2922 | Commented existing code as not required
    'Public Sub HighlightSubMenu(ByRef strRMDYear As String)
    '    Try
    '        Dim htmlGenericControl As HtmlTableCell
    '        Dim lnkRMD As LinkButton
    '        For Each rptItem As RepeaterItem In rptRMDBatchId.Items
    '            lnkRMD = rptItem.FindControl("lnkRMDProcessBatchId")
    '            htmlGenericControl = rptItem.FindControl("liRMDProcessBatchId")
    '            If lnkRMD.Text = strRMDYear Then
    '                htmlGenericControl.Attributes("class") = "tabSelectedLink"
    '                lblMessage.Text += "BatchID " + strRMDYear
    '            Else
    '                htmlGenericControl.Attributes("class") = ""
    '                htmlGenericControl.Attributes("padding-bottom") = "10px"
    '            End If
    '        Next
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("GenerateRMD_HighlightSubMenu", ex)
    '    End Try
    'End Sub
    'END: MMR | 2016.10.24 | YRS-AT-2922 | Commented existing code as not required

    Private Sub gvProcessBatchStatus_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvProcessBatchStatus.PageIndexChanging
        Dim dv As DataView
        gvProcessBatchStatus.PageIndex = e.NewPageIndex
        dv = CType(Session("gvProcessBatchStatus"), DataTable).DefaultView
        HelperFunctions.BindGrid(gvProcessBatchStatus, dv, True)
    End Sub

    Private Sub gvProcessBatchStatus_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvProcessBatchStatus.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            HelperFunctions.SetSortingArrows(Session("gvProcessBatchStatus_sort"), e)
        End If
    End Sub

    Private Sub gvProcessBatchStatus_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvProcessBatchStatus.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = CType(Session("gvProcessBatchStatus"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                HelperFunctions.gvSorting(Session("gvProcessBatchStatus_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvProcessBatchStatus, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvProcessBatchStatus_Sorting", ex)
        End Try
    End Sub

    Private Sub gvRMDBatchErrorList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvRMDBatchErrorList.PageIndexChanging
        Dim dv As DataView
        gvRMDBatchErrorList.PageIndex = e.NewPageIndex
        dv = CType(Session("ArrErrorDataList"), DataTable).DefaultView
        HelperFunctions.BindGrid(gvRMDBatchErrorList, dv, True)
    End Sub

    Private Sub gvRMDBatchErrorList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRMDBatchErrorList.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            HelperFunctions.SetSortingArrows(Session("ArrErrorDataList_sort"), e)
        End If
    End Sub

    Private Sub gvRMDBatchErrorList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvRMDBatchErrorList.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = CType(Session("ArrErrorDataList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                HelperFunctions.gvSorting(Session("ArrErrorDataList_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvRMDBatchErrorList, dv, True)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvRMDBatchErrorList_Sorting", ex)
        End Try
    End Sub

    'START: MMR | 2016.10.24 | YRS-AT-2922 | Commented existing code as not required
    'Private Sub rptRMDBatchId_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptRMDBatchId.ItemDataBound
    '    Dim lnkRMDProcessBatchId As LinkButton
    '    Dim strBatchId As String
    '    Dim htmlGenericControl As HtmlTableCell
    '    Try
    '        If e.Item.ItemIndex = 0 Then
    '            lnkRMDProcessBatchId = e.Item.FindControl("lnkRMDProcessBatchId")
    '            BindRMDStatus(lnkRMDProcessBatchId.Text.Trim)
    '            strBatchId = lnkRMDProcessBatchId.Text.Trim
    '            htmlGenericControl = e.Item.FindControl("liRMDProcessBatchId")
    '            htmlGenericControl.Attributes("class") = "tabSelectedLink"
    '            lblMessage.Text += "BatchID " + lnkRMDProcessBatchId.Text
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("rptRMDBatchId_ItemDataBound", ex)
    '    End Try
    'End Sub
    'END: MMR | 2016.10.24 | YRS-AT-2922 | Commented existing code as not required

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("btnCancel_Click", ex)
        End Try
    End Sub

    Public Function GetSelectedRMDBatchRecordsForNonEligible() As DataTable
        Dim dtRMDBatchNonEligibleList As New DataTable("dtRMDBatchNonEligibleList")
        Dim drRMDBatchNonEligibleList As DataRow
        dtRMDBatchNonEligibleList.Columns.Add("FundId")
        dtRMDBatchNonEligibleList.Columns.Add("PlanType")
        dtRMDBatchNonEligibleList.Columns.Add("CurrentBalance")
        dtRMDBatchNonEligibleList.Columns.Add("RMDAmount")
        dtRMDBatchNonEligibleList.Columns.Add("PaidAmount")
        dtRMDBatchNonEligibleList.Columns.Add("FundStatus")
        dtRMDBatchNonEligibleList.Columns.Add("DueDate")
        dtRMDBatchNonEligibleList.Columns.Add("Remarks")
        dtRMDBatchNonEligibleList.Columns.Add("ProcessDate")
        dtRMDBatchNonEligibleList.Columns.Add("ReportName")

        For iCount As Integer = 0 To gvNonEligible.Rows.Count - 1
            drRMDBatchNonEligibleList = dtRMDBatchNonEligibleList.NewRow
            drRMDBatchNonEligibleList("FundId") = gvNonEligible.Rows(iCount).Cells(0).Text
            drRMDBatchNonEligibleList("PlanType") = gvNonEligible.Rows(iCount).Cells(1).Text
            drRMDBatchNonEligibleList("CurrentBalance") = gvNonEligible.Rows(iCount).Cells(2).Text
            drRMDBatchNonEligibleList("RMDAmount") = gvNonEligible.Rows(iCount).Cells(3).Text
            drRMDBatchNonEligibleList("PaidAmount") = gvNonEligible.Rows(iCount).Cells(4).Text
            drRMDBatchNonEligibleList("FundStatus") = gvNonEligible.Rows(iCount).Cells(5).Text
            drRMDBatchNonEligibleList("DueDate") = gvNonEligible.Rows(iCount).Cells(9).Text
            drRMDBatchNonEligibleList("ProcessDate") = ddlNonEligibleRMDYear.SelectedValue.Trim
            drRMDBatchNonEligibleList("Remarks") = gvNonEligible.Rows(iCount).Cells(15).Text.Replace("&#39;", "'").Replace("&nbsp;", " ").Replace("&amp;", "&")
            drRMDBatchNonEligibleList("ReportName") = "Non Eligible RMD Persons"
            dtRMDBatchNonEligibleList.Rows.Add(drRMDBatchNonEligibleList)
        Next
        Session("RMDBatchProcessNonEligibleList") = dtRMDBatchNonEligibleList
        Session("ReportName") = "RMDBatchProcessNonEligibleList"
        Return dtRMDBatchNonEligibleList
    End Function

    'START: MMR | 2016.10.24 | YRS-AT-2922 | populating RMD year,due dates and Batch ID in Dropdown and binding dropdown on selection change event
    Public Sub BindRMDYearDropdown()
        Dim dsRMDProcessedBatch As DataSet
        If Not ViewState("RMDProcessDetails") Is Nothing Then
            dsRMDProcessedBatch = DirectCast(ViewState("RMDProcessDetails"), DataSet)
        End If
        dropdownlistRMDYear.DataSource = dsRMDProcessedBatch.Tables("RMDYear")
        dropdownlistRMDYear.DataTextField = "Year"
        dropdownlistRMDYear.DataValueField = "Year"
        dropdownlistRMDYear.DataBind()
        If RMDYear <> 0 Then
            dropdownlistRMDYear.SelectedValue = RMDYear
        Else
            dropdownlistRMDYear.SelectedValue = dsRMDProcessedBatch.Tables("RMDYear").Rows(0)("Year")
        End If
    End Sub

    Public Sub BindRMDMonthDropdown(ByVal year As String)
        Dim dsRMDProcessDetails As DataSet
        Dim dvRMDProcessDetails As DataView
        Dim dvRMDProcessDetailsBatch As DataView
        Dim cnt As Integer = 0

        IsMarchClosed = False
        IsDecemberClosed = False
        dropdownlistRMDMonth.DataSource = Nothing
        dropdownlistRMDMonth.Items.Clear()
        If Not ViewState("RMDProcessDetails") Is Nothing Then
            dsRMDProcessDetails = DirectCast(ViewState("RMDProcessDetails"), DataSet)
            dvRMDProcessDetails = dsRMDProcessDetails.Tables("RMDDueDate").DefaultView
            dvRMDProcessDetails.RowFilter = String.Format("RMDYear = {0}", year)

            If Not String.IsNullOrEmpty(dvRMDProcessDetails(0)("Mar_Closed_On").ToString()) And dvRMDProcessDetails(0)("Mar_Closed_On") <> "01/01/1900" Then
                IsMarchClosed = True
            End If
            If Not String.IsNullOrEmpty(dvRMDProcessDetails(0)("Dec_Closed_On").ToString()) And dvRMDProcessDetails(0)("Dec_Closed_On") <> "01/01/1900" Then
                IsDecemberClosed = True
            End If


            If (IsMarchClosed = False And IsDecemberClosed = False) Or (IsMarchClosed And IsDecemberClosed) Then
                dropdownlistRMDMonth.Items.Insert(0, New ListItem("Any", "ANY"))
                dropdownlistRMDMonth.SelectedIndex = 0
                cnt = cnt + 1
            End If

            Dim duedate As String

            duedate = String.Format("{0}{1}", "Dec-", year)
            If IsDecemberClosed Then
                duedate = String.Format("{0}{1}", duedate, " (Closed)")
            End If
            dropdownlistRMDMonth.Items.Insert(cnt, New ListItem(duedate, String.Format("{0}{1}", "12/31/", year)))

            cnt += 1

            duedate = String.Format("{0}{1}", "Mar-", Convert.ToString(Convert.ToInt32(year) + 1))
            If IsMarchClosed Then
                duedate = String.Format("{0}{1}", duedate, " (Closed)")
            End If
            dropdownlistRMDMonth.Items.Insert(cnt, New ListItem(duedate, String.Format("{0}{1}", "03/31/", Convert.ToInt32(year) + 1)))
            If Not String.IsNullOrEmpty(RMDDueDate) Then
                dropdownlistRMDMonth.SelectedValue = RMDDueDate
            End If
        End If
    End Sub

    Public Sub BindRMDBatchIDDropdown(ByVal year As String, ByVal dueDate As String)
        Dim dsRMDProcessDetails As DataSet
        Dim dvRMDProcessDetailsBatch As DataView

        dropdownBatchId.DataSource = Nothing
        dropdownBatchId.Items.Clear()

        If Not ViewState("RMDProcessDetails") Is Nothing Then
            dsRMDProcessDetails = DirectCast(ViewState("RMDProcessDetails"), DataSet)
            dvRMDProcessDetailsBatch = dsRMDProcessDetails.Tables("BatchId").DefaultView
            If dueDate.ToUpper() <> "ANY" Then
                dvRMDProcessDetailsBatch.RowFilter = String.Format("RMDYear = {0} AND DueDate = '{1}'", year, dueDate)
            Else
                dvRMDProcessDetailsBatch.RowFilter = String.Format("RMDYear = {0}", year)
            End If
            dropdownBatchId.DataSource = dvRMDProcessDetailsBatch
            dropdownBatchId.DataTextField = "BatchId"
            dropdownBatchId.DataValueField = "BatchId"
            dropdownBatchId.DataBind()
            dropdownBatchId.Items.Insert(0, New ListItem("All", "All"))
            If Not String.IsNullOrEmpty(RMDBatchID) Then
                dropdownBatchId.SelectedValue = RMDBatchID
            Else
                dropdownBatchId.SelectedValue = "All"
            End If
        End If
    End Sub

    Private Sub dropdownlistRMDYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dropdownlistRMDYear.SelectedIndexChanged
        Try
            BindRMDMonthDropdown(dropdownlistRMDYear.SelectedValue)
            BindRMDBatchIDDropdown(dropdownlistRMDYear.SelectedValue, dropdownlistRMDMonth.SelectedValue)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("dropdownlistRMDYear_SelectedIndexChanged", ex)
        End Try
    End Sub

    Private Sub dropdownlistRMDMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dropdownlistRMDMonth.SelectedIndexChanged
        Try
            BindRMDBatchIDDropdown(dropdownlistRMDYear.SelectedValue, dropdownlistRMDMonth.SelectedValue)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("dropdownlistRMDMonth_SelectedIndexChanged", ex)
        End Try
    End Sub
    'END: MMR | 2016.10.24 | YRS-AT-2922 | populating RMD year,due dates and Batch ID in Dropdown and binding dropdown on selection change event

    'START: MMR | 2016.10.27 |  YRS-AT-2922 | Get RMDBatchProcess Status details on click of view button
    Private Sub buttonSearch_Click(sender As Object, e As EventArgs) Handles buttonSearch.Click
        Try
            If dropdownBatchId.SelectedValue <> "" AndAlso dropdownlistRMDMonth.SelectedValue <> "" AndAlso dropdownlistRMDYear.SelectedValue <> "" Then
                BindRMDStatus(dropdownBatchId.SelectedValue, dropdownlistRMDYear.SelectedValue, dropdownlistRMDMonth.SelectedValue, IsMarchClosed, IsDecemberClosed)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("buttonSearch_Click", ex)
        End Try
    End Sub
    'END: MMR | 2016.10.27 |  YRS-AT-2922 | Get RMDBatchProcess Status details on click of view button
End Class

Public Class ReturnStatusValues

    Public iTotalCount As Integer
    Public iProcessCount As Integer
    Public iIdxCreated As Integer
    Public iPdfCreated As Integer
    Public iIdxCopied As Integer
    Public iPdfCopied As Integer
    Public strProcessName As String
    Public strBatchId As String
    Public strretValue As String
    Public strReportType As String
    'START : CS | 2016.10.24 | YRS-AT-3088 |  Declared the variable for(Intial/Cashout/Followup) file path 
    Public initialLetterPath As String
    Public cashOutLetterPath As String
    Public followupLetterPath As String
    Public cashOutFollowupLetterPath As String 'SB | 2016.11.21 | YRS-T-3203 | For cashout follow up records
    'END : CS | 2016.10.24 | YRS-AT-3088 |   Declared the variable for(Intial/Cashout/Followup) file path 
    Public iTotalIDXPDFCount As Integer
    Public Links As List(Of YMCAObjects.LinkDetail) 'PPP | 05/02/2017 | YRS-AT-3356 | This variable holds list of links which will be listed on progress control
    Public AllowReprint As Boolean = False 'MMR | YRS-AT-3356 | Declared variable which will indicate whether to copy files for reprinting
End Class

<Serializable()> _
Public Class IDMStatus
    Private m_idxCreated As Integer
    Private m_pdfCreated As Integer
    Private m_Copied As Integer

    Public Property IdxCreated() As Integer
        Get
            Return m_idxCreated
        End Get
        Set(value As Integer)
            m_idxCreated = value
        End Set
    End Property


    Public Property PdfCreated As Integer
        Get
            Return m_pdfCreated
        End Get
        Set(value As Integer)
            m_pdfCreated = value
        End Set
    End Property

    Public Property Copied As Integer
        Get
            Return m_Copied
        End Get
        Set(value As Integer)
            m_Copied = value
        End Set
    End Property

End Class

<Serializable()> _
Public Class SavePropertys

    Private m_FundEventID As String
    Public Property FundEventID() As String
        Get
            Return m_FundEventID
        End Get
        Set(ByVal value As String)
            m_FundEventID = value
        End Set
    End Property

    Private m_RetirementRefundType As String
    Public Property RetirementPlanWithdrawalType() As String
        Get
            Return m_RetirementRefundType
        End Get
        Set(ByVal value As String)
            m_RetirementRefundType = value
        End Set
    End Property

    Private m_SavingsRefundType As String
    Public Property SavingsPlanWithdrawalType() As String
        Get
            Return m_SavingsRefundType
        End Get
        Set(ByVal value As String)
            m_SavingsRefundType = value
        End Set
    End Property

    Private m_RetirementPlanPartialAmount As String
    Public Property RetirementPlanPartialAmount() As String
        Get
            Return m_RetirementPlanPartialAmount
        End Get
        Set(ByVal value As String)
            m_RetirementPlanPartialAmount = value
        End Set
    End Property

    Private m_SavingsPlanPartialAmount As String
    Public Property SavingsPlanPartialAmount() As String
        Get
            Return m_SavingsPlanPartialAmount
        End Get
        Set(ByVal value As String)
            m_SavingsPlanPartialAmount = value
        End Set
    End Property

    Private m_RetirementAcounts As String()
    Public Property SelectedRetirementPlanAccounts() As String()
        Get
            Return m_RetirementAcounts
        End Get
        Set(ByVal value As String())
            m_RetirementAcounts = value
        End Set
    End Property

    Private m_SlectedSavingsAcounts As String()
    Public Property SelectedSavingsPlanAccounts() As String()
        Get
            Return m_SlectedSavingsAcounts
        End Get
        Set(ByVal value As String())
            m_SlectedSavingsAcounts = value
        End Set
    End Property
    Private m_RetirementMRDYear As Integer
    Public Property RetirementMRDYear() As Integer
        Get
            Return m_RetirementMRDYear
        End Get
        Set(ByVal value As Integer)
            m_RetirementMRDYear = value
        End Set
    End Property
    Private m_SavingsMRDYear As Integer
    Public Property SavingsMRDYear() As Integer
        Get
            Return m_SavingsMRDYear
        End Get
        Set(ByVal value As Integer)
            m_SavingsMRDYear = value
        End Set
    End Property
    Private m_RMDProcessingDate As String
    Public Property RMDProcessingDate() As String
        Get
            Return m_RMDProcessingDate
        End Get
        Set(ByVal value As String)
            m_RMDProcessingDate = value
        End Set
    End Property
End Class


