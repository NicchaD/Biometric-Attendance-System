'************************************************************************************************************************
' Author: Pramod Prakash Pokale
' Created on: 09/18/2017
' Summary of Functionality: Provides options to change fund event status
' Declared in Version: 20.4.0 | YRS-AT-3631 -  YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950)
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' 			                  | 	         |		           | 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Public Class DCToolsChangeFundStatus
    Inherits System.Web.UI.Page

    Private changeFundStatusTab As Integer = 1
    Private reviewTab As Integer = 2

    Public Property SessionInfo() As YMCAObjects.DCTools.FundStatus
        Get
            If Not Session("DCToolsChangeFundStatusSession") Is Nothing Then
                Return TryCast(Session("DCToolsChangeFundStatusSession"), YMCAObjects.DCTools.FundStatus)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As YMCAObjects.DCTools.FundStatus)
            Session("DCToolsChangeFundStatusSession") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sessionDetails As YMCAObjects.DCTools.FundStatus
        Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            ElseIf Not IsPostBack Then

                CheckAccessRights()

                sessionDetails = New YMCAObjects.DCTools.FundStatus()
                sessionDetails.FundEventID = Convert.ToString(Session("FundID"))
                ClearSession()
                sessionDetails = LoadData(sessionDetails)

                SetModuleName(sessionDetails)
                ManageTab(changeFundStatusTab, sessionDetails)
                Me.SessionInfo = sessionDetails
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DCToolsChangeFundStatus_Page_Load", ex)
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub ClearSession()
        Me.SessionInfo = Nothing
        Session("FundID") = Nothing
    End Sub

    Private Sub SetModuleName(ByVal sessionDetails As YMCAObjects.DCTools.FundStatus)
        Dim lblModuleName As Label
        Try
            lblModuleName = Master.FindControl("LabelModuleName")
            If lblModuleName IsNot Nothing Then
                lblModuleName.Text = String.Format("Administration > DC Tools > Change Fund Status - Fund Id {0} - {1}, {2} {3}", sessionDetails.FundNo.ToString(), sessionDetails.LastName, sessionDetails.FirstName, sessionDetails.MiddleName)
            End If
        Catch
            Throw
        Finally
            lblModuleName = Nothing
        End Try
    End Sub

    Private Sub ManageTab(ByVal tab As Integer, ByRef sessionDetails As YMCAObjects.DCTools.FundStatus)
        Try
            Select Case tab
                Case 1
                    divChangeFundStatus.Style("display") = "block"
                    divReviewAndSubmit.Style("display") = "none"

                    tdChangeFundStatus.Attributes("class") = "ActiveTab"
                    If String.IsNullOrEmpty(sessionDetails.NewFundEventStatus) Then
                        'Being loaded 1st time
                        tdReviewAndSubmit.Attributes("class") = "InActiveDisabledTab"
                    Else
                        tdReviewAndSubmit.Attributes("class") = "InActiveTab"
                    End If
                    btnNext.Style("display") = "block"
                    btnPrevious.Style("display") = "none"
                    btnSaveFundStatusChanges.Style("display") = "none"

                    lblExistingFundStatus.Text = sessionDetails.OldFullFundEventStatus
                    lblCurrentBalance.Text = String.Format("${0}", sessionDetails.Balance)

                    If (Not String.IsNullOrEmpty(sessionDetails.NewFundEventStatus)) Then
                        ddlNewFundStatus.SelectedValue = sessionDetails.NewFundEventStatus
                    End If

                    If (Not String.IsNullOrEmpty(sessionDetails.UserNotes)) Then
                        txtNotes.Text = sessionDetails.UserNotes
                    End If
                Case 2
                    divChangeFundStatus.Style("display") = "none"
                    divReviewAndSubmit.Style("display") = "block"

                    tdChangeFundStatus.Attributes("class") = "InActiveTab"
                    tdReviewAndSubmit.Attributes("class") = "ActiveTab"

                    btnNext.Style("display") = "none"
                    btnPrevious.Style("display") = "block"
                    btnSaveFundStatusChanges.Style("display") = "block"

                    lblReviewExistingFundStatus.Text = sessionDetails.OldFullFundEventStatus
                    lblReviewNewFundStatus.Text = sessionDetails.NewFullFundEventStatus
                    lblReviewNotes.Text = String.Format("{0} {1}", String.Format(sessionDetails.SystemNotes, sessionDetails.OldFundEventStatus, sessionDetails.NewFundEventStatus), IIf(String.IsNullOrEmpty(sessionDetails.UserNotes), String.Empty, sessionDetails.UserNotes))
            End Select
        Catch
            Throw
        End Try
    End Sub

    Private Function LoadData(ByRef sessionDetails As YMCAObjects.DCTools.FundStatus) As YMCAObjects.DCTools.FundStatus
        Dim ds As DataSet
        Try
            If Not sessionDetails Is Nothing Then
                ds = YMCARET.YmcaBusinessObject.DCToolsBO.GetFundStatusDetails(sessionDetails.FundEventID)
                If HelperFunctions.isNonEmpty(ds) Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        sessionDetails.OldFundEventStatus = Convert.ToString(ds.Tables(0).Rows(0)("StatusType"))
                        sessionDetails.OldFullFundEventStatus = Convert.ToString(ds.Tables(0).Rows(0)("DisplayStatus"))
                        sessionDetails.SystemNotes = Convert.ToString(ds.Tables(0).Rows(0)("SystemNotes"))
                        sessionDetails.Balance = Convert.ToDecimal(ds.Tables(0).Rows(0)("TotalBalance"))

                        sessionDetails.LastName = Convert.ToString(ds.Tables(0).Rows(0)("LastName"))
                        sessionDetails.FirstName = Convert.ToString(ds.Tables(0).Rows(0)("FirstName"))
                        sessionDetails.MiddleName = Convert.ToString(ds.Tables(0).Rows(0)("MiddleName"))
                        sessionDetails.PersID = Convert.ToString(ds.Tables(0).Rows(0)("PersID"))
                        sessionDetails.FundNo = Convert.ToString(ds.Tables(0).Rows(0)("FundIdNo"))
                    End If

                    If ds.Tables(1).Rows.Count > 0 Then
                        ddlNewFundStatus.DataSource = ds.Tables(1)
                        ddlNewFundStatus.DataBind()

                        SelectRecommendedFundEventStatus(sessionDetails.FundEventID)
                    End If
                End If
            End If
            Return sessionDetails
        Catch
            Throw
        Finally
            ds = Nothing
        End Try
    End Function

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim sessionDetails As YMCAObjects.DCTools.FundStatus
        Try
            sessionDetails = Me.SessionInfo
            If Not sessionDetails Is Nothing Then
                If ddlNewFundStatus.SelectedValue <> "-1" Then
                    sessionDetails.NewFundEventStatus = ddlNewFundStatus.SelectedValue
                    sessionDetails.NewFullFundEventStatus = ddlNewFundStatus.SelectedItem.Text
                Else
                    sessionDetails.NewFundEventStatus = String.Empty
                    sessionDetails.NewFullFundEventStatus = String.Empty
                End If
                sessionDetails.UserNotes = txtNotes.Text.Trim()

                If Validate(sessionDetails) Then
                    ManageTab(reviewTab, sessionDetails)
                End If
            End If
            Me.SessionInfo = sessionDetails
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DCToolsChangeFundStatus_btnNext_Click", ex)
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    Private Function Validate(ByRef sessionDetails As YMCAObjects.DCTools.FundStatus) As Boolean
        Dim errorMessage As String
        Dim isValid As Boolean
        Try
            isValid = True
            If String.IsNullOrEmpty(sessionDetails.NewFundEventStatus) Then
                errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_SELECT).DisplayText
                'HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_SELECT)
                isValid = False
            End If

            If String.IsNullOrEmpty(sessionDetails.UserNotes) Then
                If String.IsNullOrEmpty(errorMessage) Then
                    errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_NOTES).DisplayText
                Else
                    errorMessage = String.Format("{0} <br /> {1}", errorMessage, YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_NOTES).DisplayText)
                End If
                isValid = False
            End If

            If Not isValid Then
                HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error)
            End If
            Return isValid
        Catch
            Throw
        Finally
            errorMessage = Nothing
        End Try
    End Function

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Dim sessionDetails As YMCAObjects.DCTools.FundStatus
        Try
            sessionDetails = Me.SessionInfo
            If Not sessionDetails Is Nothing Then
                ManageTab(changeFundStatusTab, sessionDetails)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DCToolsChangeFundStatus_btnNext_Click", ex)
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    Protected Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        Dim sessionDetails As YMCAObjects.DCTools.FundStatus
        Dim activityLogData As YMCAObjects.YMCAActionEntry
        Dim logData As String
        Dim dictParameter As Dictionary(Of String, String)
        Try
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                sessionDetails = Me.SessionInfo
                If Not sessionDetails Is Nothing Then
                    logData = String.Format("<DC><OldStatus>{0}</OldStatus><NewSatus>{1}</NewSatus><Balance>${2}</Balance></DC>", sessionDetails.OldFundEventStatus, sessionDetails.NewFundEventStatus, sessionDetails.Balance.ToString())
                    activityLogData = GetActivityLogData(YMCAObjects.ActionYRSActivityLog.CHANGE_FUND_STATUS, sessionDetails.PersID, YMCAObjects.EntityTypes.PERSON, YMCAObjects.Module.DCTools, logData)
                    If (YMCARET.YmcaBusinessObject.DCToolsBO.SaveFundStatusChanges(sessionDetails, activityLogData)) Then

                        dictParameter = New Dictionary(Of String, String)
                        dictParameter.Add("FundNo", sessionDetails.FundNo.ToString())
                        dictParameter.Add("Name", String.Format("{0}, {1} {2}", sessionDetails.LastName, sessionDetails.FirstName, sessionDetails.MiddleName))

                        HelperFunctions.ShowMessageOnNextPage(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_SUCCESS, Nothing, dictParameter)
                        ClearSession()
                        Response.Redirect("FindInfo.aspx?Name=DCToolsChangeFundStatus", True)
                    Else
                        HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_UPDATE, EnumMessageTypes.Error)
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DCToolsChangeFundStatus_btnConfirmDialogYes_Click", ex)
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    Public Function GetActivityLogData(ByVal action As String, ByVal perssID As String, ByVal source As YMCAObjects.EntityTypes, ByVal moduleName As String, ByVal logData As String) As YMCAObjects.YMCAActionEntry
        Dim logEntry As YMCAObjects.YMCAActionEntry
        Try
            logEntry = New YMCAObjects.YMCAActionEntry
            logEntry.Action = action
            logEntry.ActionBy = Session("LoginId")
            logEntry.Data = logData
            logEntry.EntityId = perssID
            logEntry.EntityType = source
            logEntry.Module = moduleName
            logEntry.SuccessStatus = True
            Return logEntry
        Catch
            Throw
        Finally
            logEntry = Nothing
        End Try
    End Function

    Private Sub SelectRecommendedFundEventStatus(ByVal fundEventID As String)
        Dim recommendedFundEventStatus As String
        Try
            recommendedFundEventStatus = YMCARET.YmcaBusinessObject.FundEventStatus.GetRecommendedFundEventStatus(fundEventID)

            If Not String.IsNullOrEmpty(recommendedFundEventStatus) Then
                ddlNewFundStatus.SelectedValue = recommendedFundEventStatus
            Else
                ddlNewFundStatus.SelectedIndex = 0
            End If
        Catch
            Throw
        Finally
            recommendedFundEventStatus = String.Empty
        End Try
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSession()
            Response.Redirect("FindInfo.aspx?Name=DCToolsChangeFundStatus", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DCToolsChangeFundStatus_btnConfirmDialogYes_Click", ex)
        End Try
    End Sub

    ''' <summary>
    ''' THis method will check read-only access rights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("FindInfo.aspx?Name=DCToolsChangeFundStatus", Convert.ToInt32(Session("LoggedUserKey")))
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
End Class