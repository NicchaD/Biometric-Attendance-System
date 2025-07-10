'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RMDCloseForm.aspx.vb
' Author Name		:	Sanjay R.
' Contact No		:	022-67928637
' Creation Date		:	02/08/2013
' Description		:	This form is used to View & store Death follow status
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Dinesh Kanojia     17/10/2014      BT:2740: RMD New Observation from client QA testing
'Sanjay R.          18/12/2014      BT:2740: RMD New Observation from client QA testing
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Chandra sekar      2016.11.01      YRS-AT-2922 - YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
'Pooja K            2019.02.28      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'*******************************************************************************
Imports YMCAUI.SessionManager
Public Class RMDCloseForm
    Inherits System.Web.UI.Page
    Protected Property dsRMDProcessLog As DataSet
        Get
            Return ViewState("dsRMDProcessLog")
        End Get
        Set(ByVal value As DataSet)
            ViewState("dsRMDProcessLog") = value
        End Set
    End Property
    Dim dsRMDStatistics As DataSet
    Dim intCount As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Method called here 
        If Not IsPostBack Then
            SessionParticipantRMDs.RMDStatistics = Nothing
            SessionParticipantRMDs.GetRMDs = Nothing
            GetRMDProcessLog()
            GetRMDStatistics()
            btnCloseCurrentRMD.Text = "Close " + lblDate.Text + " RMD"
        Else
            dsRMDStatistics = SessionParticipantRMDs.RMDStatistics
        End If
    End Sub
    Public Sub GetRMDProcessLog()
        Try
            dsRMDProcessLog = YMCARET.YmcaBusinessObject.MRDBO.GetRMDProcessLog()
            SessionParticipantRMDs.GetRMDs = dsRMDProcessLog
            If HelperFunctions.isNonEmpty(dsRMDProcessLog) Then
                gvRMDProcessLog.DataSource = dsRMDProcessLog.Tables(0)
                gvRMDProcessLog.DataBind()
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Sub ProcessCurrentMRD()
        Dim dtProcess As Date
        Dim strMsg As String
        Try
            If String.IsNullOrEmpty(dsRMDStatistics.Tables("ProcessDate").Rows(0).Item(0).ToString().Trim) Then
                HelperFunctions.ShowMessageToUser(GetRMDMessage("MESSAGE_RMD_DATE_BLANK"), EnumMessageTypes.Success, Nothing)
                Exit Sub
            End If

            If HelperFunctions.isNonEmpty(dsRMDStatistics) Then
                dtProcess = CType(dsRMDStatistics.Tables("ProcessDate").Rows(0).Item(0).ToString(), Date)
                intCount = YMCARET.YmcaBusinessObject.MRDBO.SaveCurrentMRD(dtProcess)
            End If

            If intCount > 0 Then
                'START: CS | 11/01/2016 | YRS-AT-2922 | Saves NotEligible and Eligible but not processed employees in AtsRMDRecordsAtClosure table
                YMCARET.YmcaBusinessObject.MRDBO.InsertNotProcessedAndNonEligibleParticipants(dtProcess)
                'END: CS | 11/01/2016 | YRS-AT-2922 | Saves NotEligible and Eligible but not processed employees in AtsRMDRecordsAtClosure table
                HelperFunctions.ShowMessageToUser(String.Format(GetRMDMessage("MESSAGE_CLOSED_SUCCESS"), lblDate.Text), EnumMessageTypes.Success, Nothing)

            Else
                HelperFunctions.ShowMessageToUser(GetRMDMessage("MESSAGE_CLOSEED_OR_NODATE"), EnumMessageTypes.Information, Nothing)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#Region "Function to get Display Message from message code."
    Private Function GetRMDMessage(ByVal stMessageCode As String) As String

        Dim l_Message As String
        l_Message = String.Empty

        If stMessageCode = "MESSAGE_RMD_DATE_BLANK" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_RMD_DATE_BLANK
        End If

        If stMessageCode = "MESSAGE_CLOSED_SUCCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSED_SUCCESS
        End If

        If stMessageCode = "MESSAGE_CLOSEED_OR_NODATE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSEED_OR_NODATE
        End If

        If stMessageCode = "MESSAGE_CONFIRM_CLOSE_RMD_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CONFIRM_CLOSE_RMD_PROCESS
        End If

        If stMessageCode = "MESSAGE_UNSATISFIED_RMD_CONTINUE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_UNSATISFIED_RMD_CONTINUE
        End If

        If stMessageCode = "MESSAGE_RMD_GENERATE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_RMD_GENERATE
        End If

        Return l_Message
    End Function
#End Region

    Private Sub gvRMDProcessLog_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvRMDProcessLog.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                'Build custom header.
                Dim oGridView As GridView = DirectCast(sender, GridView)
                Dim oGridViewRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
                Dim oTableCell As New TableCell()

                oTableCell.Text = ""
                oTableCell.ColumnSpan = 2
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell()
                oTableCell.Text = "December RMD" '2014.12.18/SR/BT:2740: December should come first in RMD Details grid
                oTableCell.HorizontalAlign = HorizontalAlign.Center
                oTableCell.ColumnSpan = 2
                oGridViewRow.Cells.Add(oTableCell)


                oTableCell = New TableCell()
                oTableCell.Text = "March RMD" '2014.12.18/SR/BT:2740: March should come second in RMD Details grid
                oTableCell.ColumnSpan = 2
                oTableCell.HorizontalAlign = HorizontalAlign.Center
                oGridViewRow.Cells.Add(oTableCell)
                oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Close Current RMD --> gvRMDProcessLog_RowCreated ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub btnCloseCurrentRMD_Click(sender As Object, e As EventArgs) Handles btnCloseCurrentRMD.Click
        Dim intCntUnsatisfiedRMD As Integer = 0
        Dim ErrMessage As String
        Dim dsRmds As DataSet
        Dim dsRMDStatistics As DataSet
        Dim dtRMDProcess As Date
        Dim intProcessingMRDYear As Integer
        Try
            dsRmds = SessionParticipantRMDs.GetRMDs
            dsRMDStatistics = SessionParticipantRMDs.RMDStatistics

            If HelperFunctions.isNonEmpty(dsRMDStatistics) Then
                If Not String.IsNullOrEmpty(dsRMDStatistics.Tables("ProcessDate").Rows(0).Item(0).ToString().Trim) Then
                    dtRMDProcess = CType(dsRMDStatistics.Tables("ProcessDate").Rows(0).Item(0).ToString(), Date)
                    intProcessingMRDYear = dtRMDProcess.Year
                  
                Else
                    Exit Sub
                End If
            End If

            If HelperFunctions.isNonEmpty(dsRmds) Then
                If dsRmds.Tables(0).Select("RMDYEAR=" + intProcessingMRDYear.ToString).Length = 0 Then
                    HelperFunctions.ShowMessageToUser(String.Format(GetRMDMessage("MESSAGE_RMD_GENERATE"), intProcessingMRDYear.ToString), EnumMessageTypes.Information, Nothing)
                    Exit Sub
                End If
            End If

            If Not String.IsNullOrEmpty(lblUnsatisfiedRMDCnt.Text.Trim) Then
                intCntUnsatisfiedRMD = lblUnsatisfiedRMDCnt.Text.Trim
            End If

            If intCntUnsatisfiedRMD > 0 Then
                ErrMessage = GetRMDMessage("MESSAGE_UNSATISFIED_RMD_CONTINUE").Replace("@CntUnsatisfiedRMD", intCntUnsatisfiedRMD.ToString())
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & ErrMessage & "','NO')", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & GetRMDMessage("MESSAGE_CONFIRM_CLOSE_RMD_PROCESS") & "','NO')", True)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Close Current RMD --> btnClose_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Try           
            ProcessCurrentMRD()
            GetRMDProcessLog()
            GetRMDStatistics()
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch ex As Exception
            HelperFunctions.LogException("Close Current RMD --> btnYes_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch ex As Exception
            HelperFunctions.LogException("Close Current RMD --> btnYes_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub gvRMDProcessLog_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRMDProcessLog.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(2).Text = "01/01/1900" Then
                e.Row.Cells(2).Text = ""
            End If
            If e.Row.Cells(4).Text = "01/01/1900" Then
                e.Row.Cells(4).Text = ""
            End If
        End If
    End Sub
    Private Sub btnCloseForm_Click(sender As Object, e As EventArgs) Handles btnCloseForm.Click
        Try
            SessionParticipantRMDs.RMDStatistics = Nothing
            SessionParticipantRMDs.GetRMDs = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("Close Current RMD --> btnClose_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Public Sub GetRMDStatistics()
        dsRMDStatistics = YMCARET.YmcaBusinessObject.MRDBO.GetRMDStatistics()
        SessionParticipantRMDs.RMDStatistics = dsRMDStatistics
        If HelperFunctions.isNonEmpty(dsRMDStatistics) Then
            lblDate.Text = dsRMDStatistics.Tables("ProcessDate").Rows(0).Item(0).ToString()
            'Code Added by Dinesh Kanojia on 17/10/2014
            'On further observation from mark/Raj
            'Dinesh Kanojia     17/10/2014      BT:2740: RMD New Observation from client QA testing
            btnCloseCurrentRMD.Text = "Close (" + dsRMDStatistics.Tables("ProcessDate").Rows(0).Item(0).ToString() + ") RMD"
            If HelperFunctions.isNonEmpty(dsRMDStatistics.Tables(1)) Then
                lblTotalRMDsCnt.Text = dsRMDStatistics.Tables(1).Rows(0)("TotalRMDs")
                lblProcessedRMDCnt.Text = dsRMDStatistics.Tables(1).Rows(0)("ProcessedRMDs")
                lblUnsatisfiedRMDCnt.Text = dsRMDStatistics.Tables(1).Rows(0)("PendingRMDs")
            End If
        End If
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            btnCloseCurrentRMD.Enabled = False
            btnCloseCurrentRMD.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class