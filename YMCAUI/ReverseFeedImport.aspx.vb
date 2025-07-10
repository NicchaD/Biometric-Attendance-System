'**************************************************************************************************************/
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:   ReverseFeedImport.aspx.vb
' Author Name		:	Pooja Kumkar
' Employee ID		:	
' Email	    	    :	
' Contact No		:	
' Creation Time 	:	12/27/2019
' Description	    :	To display difference of participant payroll file.
' Declared in Version : 20.7.2 | YRS-AT-4641 -  YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
'**************************************************************************************************************
' MODIFICATION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------------------
'**************************************************************************************************************/
Imports System.Data.SqlClient
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Drawing
Public Class ReverseFeedImport
    Inherits System.Web.UI.Page
#Region "Property"
    Public Property SelectedTab() As String
        Get
            Return ViewState("SelectedTab")
        End Get
        Set(ByVal value As String)
            ViewState("SelectedTab") = value
        End Set
    End Property
    Public Property ReadOnlyWarningMessage() As String
        Get
            If Not ViewState("ReadOnlyWarningMessage") Is Nothing Then
                Return (CType(ViewState("ReadOnlyWarningMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ReadOnlyWarningMessage") = value
        End Set
    End Property
    Public Property ImportBaseHeaderID As Integer
        Get
            If Not ViewState("ImportBaseHeaderID") Is Nothing Then
                Return CType(ViewState("ImportBaseHeaderID"), Integer)
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("ImportBaseHeaderID") = value
        End Set
    End Property

    Public Property ExportBaseHeaderID As Integer
        Get
            If Not ViewState("ExportBaseHeaderID") Is Nothing Then
                Return CType(ViewState("ExportBaseHeaderID"), Integer)
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("ExportBaseHeaderID") = value
        End Set
    End Property

    Public Property IsReadOnlyAccess() As Boolean
        Get
            If Not ViewState("IsReadOnlyAccess") Is Nothing Then
                Return (CType(ViewState("IsReadOnlyAccess"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsReadOnlyAccess") = value
        End Set
    End Property
    Public Property ImportBaseHeaderStatus() As String
        Get
            If Not ViewState("ImportBaseHeaderStatus") Is Nothing Then
                Return (CType(ViewState("ImportBaseHeaderStatus"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ImportBaseHeaderStatus") = value
        End Set
    End Property

#End Region
#Region "Enum"
    Public Enum ReverseFeedTab
        ReviewApprove
        Exception
    End Enum
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds_PageLevelDetails As DataSet
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport page load", "START: Page Load.")
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'Set Readonly access right and message value
            CheckAccessRights()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
            ds_PageLevelDetails = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetHeaderBaseID()
            If (HelperFunctions.isNonEmpty(ds_PageLevelDetails)) Then
                SetValueInHeaderFooter(ds_PageLevelDetails) 'Calling function to set values in header and summary section of page.
            End If
            If Not IsPostBack Then
                If ds_PageLevelDetails.Tables(0).Rows.Count > 0 Then
                    If (Me.ImportBaseHeaderID > 0) Then
                        Me.BindExceptionGrid() 'Bind grid of Exception tab.
                    End If
                    SetDefaultValueOnPageLoad()
                    If Me.ImportBaseHeaderStatus = "PENDING" Then
                        btnApprove.Enabled = True
                        btnDiscard.Enabled = True
                    ElseIf Me.ImportBaseHeaderStatus = "APPROVED" Then
                        btnApprove.Enabled = False
                        btnDiscard.Enabled = True
                    ElseIf Me.ImportBaseHeaderStatus = "FAILED" Then
                        btnApprove.Enabled = False
                        btnDiscard.Enabled = True
                        btnPrintList.Enabled = False
                    End If
                Else
                    btnDiscard.Enabled = False
                    btnApprove.Enabled = False
                    btnPrintList.Enabled = False
                    SetDefaultValue()
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_REVERSEFEED_RECORD_NOT_FOUND).DisplayText, EnumMessageTypes.Information)
                End If
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport page load", "END: Page Load.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.LogException("ReverseFeedImport --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub
#Region "Button Click Events"

    Public Sub lnkReviewApprove_Click(sender As Object, e As EventArgs) Handles lnkReviewApprove.Click
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport lnkReviewApprove_Click", "START")

            tdHeaderSectionFilter.Visible = True
            lblChangeAnnuitantsMsg.Visible = True
            SetFlag(False)
            SetTabValue(ReverseFeedTab.ReviewApprove)
            Me.SelectedTab = "lnkReviewApprove"
            If Me.ImportBaseHeaderStatus = "PENDING" Then
                BindReviewApproveGrid()
                btnApprove.Enabled = True
                btnDiscard.Enabled = True
            ElseIf Me.ImportBaseHeaderStatus = "APPROVED" Then
                BindReviewApproveGrid()
                btnApprove.Enabled = False
                btnDiscard.Enabled = True
            ElseIf Me.ImportBaseHeaderStatus = "FAILED" Then
                btnApprove.Enabled = False
                btnDiscard.Enabled = True
                btnPrintList.Enabled = False
            Else
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_REVERSEFEED_RECORD_NOT_FOUND).DisplayText, EnumMessageTypes.Information)
            End If

            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport lnkReviewApprove_Click", "END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.LogException("ReverseFeedImport --> lnkReviewApprove_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub lnkException_Click(sender As Object, e As EventArgs) Handles lnkException.Click
        Me.BindExceptionGrid()
        SetFlag(False)
        SetTabValue(ReverseFeedTab.Exception)
        Me.SelectedTab = "lnkException"
        If Me.ImportBaseHeaderID = 0 Then
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_REVERSEFEED_RECORD_NOT_FOUND).DisplayText, EnumMessageTypes.Information)
        End If
    End Sub

    Private Sub btnConfirmDialogYesApprove_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYesApprove.Click
        Dim resultCount As Integer
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport btnConfirmDialogYesApprove_Click", "START")
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                resultCount = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.UpdateImportBaseHeaderStatus(Me.ImportBaseHeaderID, "APPROVED")
                If resultCount > -1 Then
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_REVERSEFEED_APPROVED_SUCCESS).DisplayText, EnumMessageTypes.Success)
                End If
                btnApprove.Enabled = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport btnApprove_Click", "END")
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            End If
        Catch ex As SqlException
            HelperFunctions.LogException("btnConfirmDialogYesApprove_Click", ex)
            HelperFunctions.ShowMessageToUser("Unable to approve reverse feed import, some sql unhandled exception occured. Please try again", EnumMessageTypes.Error, Nothing)
        Catch ex As Exception
            HelperFunctions.LogException("ReverseFeedImport --> btnConfirmDialogYesApprove_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub btnConfirmDialogYesDiscard_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYesDiscard.Click
        Dim dsHeaderData As DataSet
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "btnConfirmDialogYesDiscard_Click START")
            'if user has read-only or none rights, message will be displayed and further process will not happen.
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                If Me.ImportBaseHeaderID > 0 Then

                    'Throw New Exception
                    'Below method will be called to discard Reverse Feed
                    If (YMCARET.YmcaBusinessObject.ReverseFeedImportBO.DiscardReverseFeed(Me.ImportBaseHeaderID)) Then
                        'After sucessfull discard, acknowledgment message will be shown to user on the screen
                        HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_REVERSEFEED_DISCARD_SUCCESS).DisplayText, EnumMessageTypes.Success)
                    End If
                    dsHeaderData = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetHeaderBaseID()
                    SetValueInHeaderFooter(dsHeaderData)
                    BindReviewApproveGrid()
                    Me.BindExceptionGrid()
                    btnDiscard.Enabled = False
                    btnApprove.Enabled = False
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogYesDiscard_Click", ex)
            HelperFunctions.ShowMessageToUser("Unable to discard reverse feed, Some unhandled exception occured. Please try again", EnumMessageTypes.Error, Nothing)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "btnConfirmDialogYesDiscard_Click END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
#End Region

#Region "Methods/Functions"

    Private Sub SetFlag(ByRef boolFlag As Boolean)
        lnkReviewApprove.Visible = Not boolFlag
        lnkException.Visible = Not boolFlag

        lblReviewApprove.Visible = boolFlag
        lblException.Visible = boolFlag
    End Sub
    Public Sub SetValueInHeaderFooter(ByVal ds_PageLevelDetails As DataSet)
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "SetValueInHeaderFooter START")
            Me.ImportBaseHeaderID = Nothing
            Me.ExportBaseHeaderID = Nothing
            Me.ImportBaseHeaderStatus = Nothing
            If (HelperFunctions.isNonEmpty(ds_PageLevelDetails)) Then
                If (ds_PageLevelDetails.Tables("ReverseFeedPageLevelDetails").Rows.Count > 0) Then
                    Me.ImportBaseHeaderID = ds_PageLevelDetails.Tables("ReverseFeedPageLevelDetails").Rows(0)("ImportBaseHeaderId")
                    Me.ExportBaseHeaderID = ds_PageLevelDetails.Tables("ReverseFeedPageLevelDetails").Rows(0)("ExportBaseHeaderId")
                    hdnImportbaseID.Value = Me.ImportBaseHeaderID
                    hdnExportBaseID.Value = Me.ExportBaseHeaderID
                    lblFileImportDate.Text = ds_PageLevelDetails.Tables("ReverseFeedPageLevelDetails").Rows(0)("FinalImportDate")
                    lblPayrollDate.Text = ds_PageLevelDetails.Tables("ReverseFeedPageLevelDetails").Rows(0)("PayrollDate")
                    Me.ImportBaseHeaderStatus = ds_PageLevelDetails.Tables("ReverseFeedPageLevelDetails").Rows(0)("chvStatus")
                    If (ds_PageLevelDetails.Tables("ImportBaseSummaryRecords").Rows.Count > 0) Then
                        SetTotalValuesinSummarySection(ds_PageLevelDetails.Tables("ImportBaseSummaryRecords"))
                    End If
                End If
            Else
                SetTotalValuesinSummarySection(ds_PageLevelDetails.Tables("ImportBaseSummaryRecords"))
                lblFileImportDate.Text = String.Empty
                lblPayrollDate.Text = String.Empty
            End If

        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "SetValueInHeaderFooter END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    Public Sub BindReviewApproveGrid()
        Dim ds As DataSet = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetDataInGrid(Me.ImportBaseHeaderID)
        Session("gvData") = ds.Tables(0)
        If (HelperFunctions.isNonEmpty(ds)) Then
            If (ds.Tables("ReverseFeedDetails").Rows.Count > 0) Then
                gvAnn.DataSource = ds.Tables("ReverseFeedDetails")
                gvAnn.DataBind()
            End If
        Else
            gvAnn.DataSource = ds.Tables("ReverseFeedDetails")
            gvAnn.DataBind()
        End If

        lblReviewApproveRecords.InnerText = ds.Tables("ReverseFeedDetails").Rows.Count
        lblReviewApproveRecords.Visible = True
        lblRARecordList.Visible = True
    End Sub
    Public Sub BindExceptionGrid()
        Dim dsException As DataSet
        dsException = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetDataInExceptionTabGrid(Me.ImportBaseHeaderID)
        Session("dsException") = dsException.Tables("ReverseFeedExceptionDetails")
        If (HelperFunctions.isNonEmpty(dsException.Tables("ReverseFeedExceptionDetails"))) Then
            If (dsException.Tables("ReverseFeedExceptionDetails").Rows.Count > 0) Then
                gvException.DataSource = dsException.Tables("ReverseFeedExceptionDetails")
                gvException.DataBind()
                lblAnnuitantsErrWrn.Visible = True
            End If
        Else
            gvException.DataSource = dsException.Tables("ReverseFeedExceptionDetails")
            gvException.DataBind()
            lblAnnuitantsErrWrn.Visible = False

        End If
        lblExceptionRecords.InnerText = dsException.Tables("ReverseFeedExceptionDetails").Rows.Count
        If (HelperFunctions.isNonEmpty(dsException.Tables("ReverseFeedFileError"))) Then
            If (dsException.Tables("ReverseFeedFileError").Rows.Count > 0) Then
                gvFileErrWrn.DataSource = dsException.Tables("ReverseFeedFileError")
                gvFileErrWrn.DataBind()
                divFileError.Style("display") = "block"
                lblFileError.Visible = True

            End If
        Else
            gvFileErrWrn.DataSource = dsException.Tables("ReverseFeedFileError")
            gvFileErrWrn.DataBind()
            divFileError.Style("display") = "none"
            lblFileError.Visible = False


        End If
    End Sub



    <System.Web.Services.WebMethod()> _
    Public Shared Function GetReverseFeedDifferenceDetails(ByVal requestedFundId As String, ByVal requestedImportBaseId As String, ByVal requestedExportBaseId As String)
        Dim dsReverseFeedDiff As DataSet
        Dim dsException As DataSet
        Dim ReverseFeedDiffList As List(Of YMCAObjects.ReverseFeed)
        Dim ReverseFeedErrorWarnList As List(Of YMCAObjects.ReverseFeedErrorWarning)
        Dim objrt As List(Of returnMasterList)
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "GetReverseFeedDifferenceDetails START")
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", String.Format("GetReverseFeedDifferenceDetails ImportBaseHeaderID : {0},ExportBaseHeaderID : {1}, FundID : {2} ", requestedImportBaseId, requestedExportBaseId, requestedFundId))

            If Not String.IsNullOrEmpty(requestedFundId) Then
                dsReverseFeedDiff = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetDifferenceData(Convert.ToInt32(requestedImportBaseId), Convert.ToInt32(requestedExportBaseId), Convert.ToInt32(requestedFundId))

                ReverseFeedDiffList = New List(Of YMCAObjects.ReverseFeed)()
                For i As Integer = 0 To dsReverseFeedDiff.Tables(0).Rows.Count - 1
                    Dim objReverseFeed As YMCAObjects.ReverseFeed = New YMCAObjects.ReverseFeed()
                    objReverseFeed.Description = dsReverseFeedDiff.Tables(0).Rows(i)("Description").ToString()
                    objReverseFeed.CurrentPayrollAmount = dsReverseFeedDiff.Tables(0).Rows(i)("CurrentPayrollAmount").ToString()
                    objReverseFeed.LastPayrollAmount = dsReverseFeedDiff.Tables(0).Rows(i)("LastPayrollAmount").ToString()
                    ReverseFeedDiffList.Add(objReverseFeed)
                Next
                ReverseFeedErrorWarnList = New List(Of YMCAObjects.ReverseFeedErrorWarning)()
                For i As Integer = 0 To dsReverseFeedDiff.Tables(1).Rows.Count - 1
                    Dim objReverseFeedErr As YMCAObjects.ReverseFeedErrorWarning = New YMCAObjects.ReverseFeedErrorWarning()
                    objReverseFeedErr.Type = dsReverseFeedDiff.Tables(1).Rows(i)("chrType").ToString()
                    objReverseFeedErr.Reason = dsReverseFeedDiff.Tables(1).Rows(i)("chvErrorDesc").ToString()
                    ReverseFeedErrorWarnList.Add(objReverseFeedErr)
                Next
                objrt = New List(Of returnMasterList)
                Dim toReturn As returnMasterList = New returnMasterList() With {
                   .ReverseFeedDifference = ReverseFeedDiffList,
                   .ReverseFeedErrorWarning = ReverseFeedErrorWarnList
                }

                Return toReturn
            Else
                Return Nothing
            End If

        Catch ex As Exception
            HelperFunctions.LogException("ReverseFeedImport_GetReverseFeedDifferenceDetails", ex)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "GetReverseFeedDifferenceDetails END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetErrorWarning(ByVal requestedBaseId As String, ByVal requestedImportBaseId As String, ByVal requestedFundNo As String)
        Dim dsErrorWarning As DataSet
        Dim ReverseFeedDiffList As List(Of YMCAObjects.ReverseFeedErrorWarning)
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "GetErrorWarning START")
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", String.Format("GetErrorWarning ImportBaseHeaderID : {0},BaseID : {1}", requestedImportBaseId, requestedBaseId, requestedFundNo))

            If Not String.IsNullOrEmpty(requestedBaseId) Then
                dsErrorWarning = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetDataExceptionList(requestedImportBaseId, requestedBaseId, requestedFundNo)

                ReverseFeedDiffList = New List(Of YMCAObjects.ReverseFeedErrorWarning)()
                For i As Integer = 0 To dsErrorWarning.Tables(0).Rows.Count - 1
                    Dim objReverseFeed As YMCAObjects.ReverseFeedErrorWarning = New YMCAObjects.ReverseFeedErrorWarning
                    objReverseFeed.FundNo = dsErrorWarning.Tables(0).Rows(i)("intFundNo").ToString()
                    objReverseFeed.Type = dsErrorWarning.Tables(0).Rows(i)("chrType").ToString()
                    objReverseFeed.Reason = dsErrorWarning.Tables(0).Rows(i)("chvErrorDesc").ToString()
                    ReverseFeedDiffList.Add(objReverseFeed)
                Next
                Return ReverseFeedDiffList
            Else
                Return Nothing
            End If

        Catch ex As Exception
            HelperFunctions.LogException("ReverseFeedImport_GetErrorWarning", ex)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "GetErrorWarning END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Protected Sub OnPageIndexChangingExceptionGrid(sender As Object, e As GridViewPageEventArgs)
        gvException.PageIndex = e.NewPageIndex
        Me.BindExceptionGrid()
    End Sub
    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvAnn.PageIndex = e.NewPageIndex
        lnkReviewApprove_Click(sender, e)
    End Sub
    Private Sub SetTabValue(ByRef enumTabSelected As ReverseFeedTab)
        If enumTabSelected = ReverseFeedTab.ReviewApprove Then
            lnkReviewApprove.Visible = False
            lblReviewApprove.Visible = True
            tdReviewApprove.Visible = True
            tdException.Visible = False
            gvAnn.Visible = True
            gvException.Visible = False

            lblReviewApproveRecords.Visible = True
            lblExceptionRecords.Visible = False
        ElseIf enumTabSelected = ReverseFeedTab.Exception Then
            lnkException.Visible = False
            lblException.Visible = True
            tdReviewApprove.Visible = False
            tdException.Visible = True
            btnDiscard.Visible = True
            gvAnn.Visible = False
            gvException.Visible = True
            lblExceptionRecords.Visible = True
            lblReviewApproveRecords.Visible = False

        End If
    End Sub
    ''' <summary>
    ''' This method will check read-only access rights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("ReverseFeedImport.aspx", Convert.ToInt32(Session("LoggedUserKey")))
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

    Private Sub SetTotalValuesinSummarySection(ByVal dtFileData As DataTable)

        Dim d_totalGrossAmount As Decimal = 0.0
        Dim d_totalStateTaxWithheldAmount As Decimal = 0.0
        Dim d_totalFedTaxWithheldAmount As Decimal = 0.0
        Dim d_totalLocalTaxWithheldAmount As Decimal = 0.0
        Dim d_totalOtherWithheldAmount As Decimal = 0.0
        Dim d_totalNetAmount As Decimal = 0.0
        Dim int_totalRecords As Integer = 0


        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "SetTotalValuesinSummarySection START")
            If (HelperFunctions.isNonEmpty(dtFileData)) Then
                If (Not IsDBNull(dtFileData.Rows(0)("numGrossAmt"))) Then
                    d_totalGrossAmount = Convert.ToDecimal(dtFileData.Rows(0)("numGrossAmt").ToString())
                    lblTotalGrossAmountValue.InnerText = "$" + d_totalGrossAmount.ToString("#,##0.00").ToString()
                Else
                    lblTotalGrossAmountValue.InnerText = "$ 0.00"
                End If
                If (Not IsDBNull(dtFileData.Rows(0)("numTotalStateTaxAmount"))) Then
                    d_totalStateTaxWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numTotalStateTaxAmount").ToString())
                    lblTotalStateTaxWithheldValue.InnerText = "$" + d_totalStateTaxWithheldAmount.ToString("#,##0.00").ToString()
                Else
                    lblTotalStateTaxWithheldValue.InnerText = "$ 0.00"
                End If
                If (Not IsDBNull(dtFileData.Rows(0)("numTotalFederalAmount"))) Then
                    d_totalFedTaxWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numTotalFederalAmount").ToString())
                    lblTotalFedTaxWithheldValue.InnerText = "$" + d_totalFedTaxWithheldAmount.ToString("#,##0.00").ToString()
                Else
                    lblTotalFedTaxWithheldValue.InnerText = "$ 0.00"
                End If
                If (Not IsDBNull(dtFileData.Rows(0)("numLocalFlatAmount"))) Then
                    d_totalLocalTaxWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numLocalFlatAmount").ToString())
                    d_totalOtherWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numTotalDeductionAmt").ToString())
                    lblTotalLocalTaxandOtherWithheldValue.InnerText = "$" + (d_totalLocalTaxWithheldAmount + d_totalOtherWithheldAmount).ToString("#,##0.00").ToString()
                Else
                    lblTotalLocalTaxandOtherWithheldValue.InnerText = "$ 0.00"
                End If

                If (Not IsDBNull(dtFileData.Rows(0)("numNetAmount"))) Then
                    d_totalNetAmount = Convert.ToDecimal(dtFileData.Rows(0)("numNetAmount").ToString())
                    lblTotalNetAmountValue.InnerText = "$" + d_totalNetAmount.ToString("#,##0.00").ToString()
                Else
                    lblTotalNetAmountValue.InnerText = "$ 0.00"
                End If

                If (Not IsDBNull(dtFileData.Rows(0)("intTotalRecords"))) Then
                    int_totalRecords = Convert.ToDecimal(dtFileData.Rows(0)("intTotalRecords").ToString())
                    lblTotalNoOfRecordsValue.InnerText = int_totalRecords
                Else
                    lblTotalNoOfRecordsValue.InnerText = 0
                End If
            Else
                lblTotalGrossAmountValue.InnerText = String.Empty
                lblTotalStateTaxWithheldValue.InnerText = String.Empty
                lblTotalFedTaxWithheldValue.InnerText = String.Empty
                lblTotalLocalTaxandOtherWithheldValue.InnerText = String.Empty
                lblTotalNetAmountValue.InnerText = String.Empty
                lblTotalNoOfRecordsValue.InnerText = String.Empty
            End If

        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "SetTotalValuesinSummarySection END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
#End Region
    Private Sub gvException_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvException.RowDataBound
        Dim ImgErrorRecords As ImageButton
        Dim ImgWarningRecords As ImageButton
        Dim rowIndex As Integer
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "gvException_RowDataBound START")

            If e.Row.RowType = DataControlRowType.Header Then
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then

                ImgErrorRecords = TryCast(e.Row.FindControl("imgErrorDetails"), ImageButton)
                ImgWarningRecords = TryCast(e.Row.FindControl("imgWarningDetails"), ImageButton)
                Dim baseID As Integer = TryCast(e.Row.FindControl("hdnBaseId"), HiddenField).Value
                Dim fundNo As Integer = TryCast(e.Row.FindControl("hdnFundNo"), HiddenField).Value
                Dim dsException As DataTable = Session("dsException")
                If (HelperFunctions.isNonEmpty(dsException)) Then
                    rowIndex = e.Row.DataItemIndex
                    If (Convert.ToInt32(dsException.Rows(rowIndex)("ErrorCount").ToString()) > 0) Then
                        ImgErrorRecords.ToolTip = dsException.Rows(rowIndex)("ErrorCount").ToString() + " Error(s)"
                        ImgErrorRecords.Visible = True
                        ImgErrorRecords.Attributes.Add("onclick", "javascript: ShowErrorWarningDialog(" + baseID.ToString() + "," + fundNo.ToString() + "); return false;")
                    Else
                        ImgErrorRecords.Visible = False
                    End If
                    If (Convert.ToInt32(dsException.Rows(rowIndex)("WarningCount").ToString()) > 0) Then
                        ImgWarningRecords.ToolTip = dsException.Rows(rowIndex)("WarningCount").ToString() + " Warning(s)"
                        ImgWarningRecords.Visible = True
                        ImgWarningRecords.Attributes.Add("onclick", "javascript: ShowErrorWarningDialog(" + baseID.ToString() + "," + fundNo.ToString() + "); return false;")
                    Else

                        ImgWarningRecords.Visible = False
                    End If

                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ReverseFeedImport_gvException_RowDataBound", ex)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ReverseFeedImport", "gvException_RowDataBound END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Private Sub SetDefaultValueOnPageLoad()
        lblReviewApprove.Visible = False
        lblException.Visible = True
        lnkException.Visible = False
        tdReviewApprove.Visible = False
        tdException.Visible = True
        gvAnn.Visible = False
        lblReviewApproveRecords.Visible = False
        lblChangeAnnuitantsMsg.Visible = False
        tdHeaderSectionFilter.Visible = False
        tdHeaderExceptionSection.Visible = True
        gvException.Visible = True
        btnDiscard.Visible = True
        lblExceptionMsg.Visible = True
        lblRARecordList.Visible = False
        lblExpListRecords.Visible = True
    End Sub

    Private Sub btnPrintList_Click(sender As Object, e As EventArgs) Handles btnPrintList.Click
        Dim strSessionName As String = String.Empty
        Dim strReportName As String = String.Empty
        Dim dsSelectedReverseFeedFileRecords As New DataSet
        Dim dtRecords As New DataSet

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Reverse Feed Import", "btnPrintList_Click() START")
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                dsSelectedReverseFeedFileRecords = YMCARET.YmcaBusinessObject.ReverseFeedImportBO.GetDataForPrintList(Me.ImportBaseHeaderID)
                dtRecords = dsSelectedReverseFeedFileRecords
                strSessionName = "dsSelectedReverseFeedFileRecords"
                strReportName = "ReverseFeed_File_Import"

                If HelperFunctions.isEmpty(dsSelectedReverseFeedFileRecords) Then
                    HelperFunctions.ShowMessageToUser("No record(s) exist to print.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                Session(strSessionName) = dsSelectedReverseFeedFileRecords
                Session("ReportName") = strReportName
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Reverse Feed Import", "btnPrintList_Click() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.LogException("btnPrintList_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)

        End Try
    End Sub
    Private Sub SetDefaultValue()
        lblReviewApprove.Visible = False
        lblException.Visible = True
        lnkException.Visible = False
        tdReviewApprove.Visible = False
        tdException.Visible = True
        gvAnn.Visible = False
        lblReviewApproveRecords.Visible = False
        lblChangeAnnuitantsMsg.Visible = False
        tdHeaderSectionFilter.Visible = False
        tdHeaderExceptionSection.Visible = True
        gvException.Visible = True
        btnApprove.Enabled = False
        lblExceptionMsg.Visible = True
        lblRARecordList.Visible = False
        lblExpListRecords.Visible = True
    End Sub

End Class

Public Class returnMasterList
    Public ReverseFeedDifference As List(Of YMCAObjects.ReverseFeed)
    Public ReverseFeedErrorWarning As List(Of YMCAObjects.ReverseFeedErrorWarning)
End Class

