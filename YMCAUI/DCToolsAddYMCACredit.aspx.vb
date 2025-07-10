'/**************************************************************************************************************/
'// Author: Manthan Rajguru
'// Created on: 09/13/2017
'// Summary of Functionality: New Admin Screen to manually add YMCA credits
'// Declared in Version: 20.4.0 | YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'// 			                | 	           |		         | 
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/

Public Class DCToolsAddYMCACredit
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim applyCreditTab As Integer = 1
    Dim reviewTab As Integer = 2
#End Region

#Region "Properties"
    Public Property YmcaCreditDetail() As YMCAObjects.DCTools.YMCACredit
        Get
            If Not ViewState("YmcaCreditDetail") Is Nothing Then
                Return TryCast(ViewState("YmcaCreditDetail"), YMCAObjects.DCTools.YMCACredit)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As YMCAObjects.DCTools.YMCACredit)
            ViewState("YmcaCreditDetail") = value
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
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            txtReceivedDate.MaxDate = Date.Today
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
            If Not Me.IsPostBack Then
                Dim ymcaID As String = String.Empty
                ClearSessionValues()
                'Set Readonly access right and message value
                CheckAccessRights()
                SetModuleName()
                If Not Session("DCYmcaIDForAddYmcaCredit") Is Nothing Then
                    ymcaID = Session("DCYmcaIDForAddYmcaCredit")
                    Session("DCYmcaIDForAddYmcaCredit") = Nothing
                    DisplayYMCAInformation(ymcaID)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DCToolsAddYMCACredit_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Check and sets read-only access rights and message which needs to be displayed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            'Check if user has rights and if not then it will set message returned from the method to be displayed to user
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("FindYmcaInfo.aspx?Name=DCToolsAddYmcaCredit", Convert.ToInt32(Session("LoggedUserKey")))
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

    ''' <summary>
    ''' Sets module details header section
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetModuleName()
        Dim moduleName As Label
        Try
            moduleName = Master.FindControl("LabelModuleName")
            If moduleName IsNot Nothing Then
                moduleName.Text = "Administration > DC Tools > Add YMCA Credit"
            End If
        Catch
            Throw
        Finally
            moduleName = Nothing
        End Try
    End Sub

    ''' <summary>
    '''  Display YMCA No, Name, current credit and set other details required to be displayed in Review screen
    ''' </summary>
    ''' <param name="ymcaID">YMCA ID</param>
    ''' <remarks></remarks>
    Private Sub DisplayYMCAInformation(ymcaID As String)
        Dim creditDetail As New YMCAObjects.DCTools.YMCACredit

        If Not Me.YmcaCreditDetail Is Nothing Then
            creditDetail = Me.YmcaCreditDetail
        End If

        If Not String.IsNullOrEmpty(ymcaID) Then
            creditDetail = SetYmcaCreditDetails(ymcaID, creditDetail)
            lblDispYMCANo.Text = creditDetail.YMCANo
            lblDispYMCAName.Text = creditDetail.YMCAName
            lblDispCurrentCredit.Text = String.Format("${0}", creditDetail.CurrentCredit)
        End If
        Me.YmcaCreditDetail = creditDetail
    End Sub

    ''' <summary>
    ''' Returns YMCA No, name, current credit, Received Accounting Date and System generated note details
    ''' </summary>
    ''' <param name="ymcaId"> YMCA ID</param>
    ''' <param name="yCreditDetail">YMCA Credit Details</param>
    ''' <returns>YMCA Credit Details</returns>
    ''' <remarks></remarks>
    Private Function SetYmcaCreditDetails(ymcaId As String, creditDetail As YMCAObjects.DCTools.YMCACredit) As YMCAObjects.DCTools.YMCACredit
        Dim ymcaCreditDetails As DataTable
        Dim receivedAccountingDate As DateTime

        ymcaCreditDetails = YMCARET.YmcaBusinessObject.DCToolsBO.GetYMCACreditDetails(ymcaId)
        creditDetail.YMCAID = ymcaId
        If HelperFunctions.isNonEmpty(ymcaCreditDetails) Then
            For Each rowYmcaCreditDetails As DataRow In ymcaCreditDetails.Rows
                creditDetail.YMCANo = rowYmcaCreditDetails("YMCANo").ToString()
                creditDetail.YMCAName = rowYmcaCreditDetails("YMCAName").ToString()
                creditDetail.CurrentCredit = rowYmcaCreditDetails("TotalAmount").ToString()
                receivedAccountingDate = rowYmcaCreditDetails("ReceivedAccountingDate")
                creditDetail.ReceivedAccountingDate = receivedAccountingDate
                creditDetail.SystemNotes = rowYmcaCreditDetails("Notes").ToString()
            Next
        End If

        Return creditDetail
    End Function

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            'Before proceeding to next tab , ValidateControls method will check if all required fields have been entered by user, if not then error message will be displayed.
            If ValidateControls() Then
                'After successfull validation, SetInputControlDetails method will set details from apply credit tab.
                SetInputControlDetails()
                'Below method will take care of enabling/disabling tabs and information to be displayed on apply credit tab & Review & Submit Tab.
                ManageTabs(reviewTab)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnNext_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' Validate controls on Apply Credit tab and return false if fails to validate else true
    ''' </summary>
    ''' <returns> returns true if controls are valid else false</returns>
    ''' <remarks></remarks>
    Private Function ValidateControls() As Boolean
        Dim errorMessageAmount As String = String.Empty
        Dim errorMessageReceivedDate As String = String.Empty
        Dim errorMessageNotes As String = String.Empty
        Dim finalErrorMessage As String = String.Empty
        Dim isValid As Boolean = True
        Dim creditAmount As Decimal

        'Check for empty value and store messages in local variable
        If Not (Decimal.TryParse(txtAmount.Text.Trim, creditAmount)) Then
            errorMessageAmount = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_AMOUNT).DisplayText
            isValid = False
        ElseIf Decimal.Parse(txtAmount.Text.Trim) = 0.0 Then
            errorMessageAmount = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_AMOUNT).DisplayText
            isValid = False
        ElseIf (Decimal.Parse(YmcaCreditDetail.CurrentCredit) + Decimal.Parse(txtAmount.Text)) < 0 Then
            errorMessageAmount = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_TOTALCREDITAMOUNT).DisplayText
            isValid = False
        Else
            'Returns error message if credit amount is more than threshold limit specified in configuation table
            errorMessageAmount = ValidateCreditAmount(Decimal.Parse(txtAmount.Text.Trim))
            If Not String.IsNullOrEmpty(errorMessageAmount) Then
                isValid = False
            End If
        End If
        If String.IsNullOrEmpty(txtNotes.Text.Trim) Then
            errorMessageNotes = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_NOTES).DisplayText
            isValid = False
        End If
        If String.IsNullOrEmpty(txtReceivedDate.Text.Trim) Then
            errorMessageReceivedDate = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_RECEIVEDDATE).DisplayText
            isValid = False
        ElseIf Date.Parse(txtReceivedDate.Text) > Date.Today Then
            errorMessageReceivedDate = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_RECEIVEDDATE_FUTURE).DisplayText
            isValid = False
        End If

        'Set single message or combine messages based on below conditions satisfied for display on UI
        If Not String.IsNullOrEmpty(errorMessageAmount) AndAlso String.IsNullOrEmpty(errorMessageReceivedDate) AndAlso String.IsNullOrEmpty(errorMessageNotes) Then
            finalErrorMessage = errorMessageAmount
        ElseIf Not String.IsNullOrEmpty(errorMessageReceivedDate) AndAlso String.IsNullOrEmpty(errorMessageAmount) AndAlso String.IsNullOrEmpty(errorMessageNotes) Then
            finalErrorMessage = errorMessageReceivedDate
        ElseIf Not String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageReceivedDate) AndAlso String.IsNullOrEmpty(errorMessageAmount) Then
            finalErrorMessage = errorMessageNotes
        ElseIf Not String.IsNullOrEmpty(errorMessageAmount) AndAlso Not String.IsNullOrEmpty(errorMessageReceivedDate) AndAlso String.IsNullOrEmpty(errorMessageNotes) Then
            finalErrorMessage = String.Format("{0}<br>{1}", errorMessageAmount, errorMessageReceivedDate)
        ElseIf Not String.IsNullOrEmpty(errorMessageAmount) AndAlso Not String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageReceivedDate) Then
            finalErrorMessage = String.Format("{0}<br>{1}", errorMessageAmount, errorMessageNotes)
        ElseIf Not String.IsNullOrEmpty(errorMessageReceivedDate) AndAlso Not String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageAmount) Then
            finalErrorMessage = String.Format("{0}<br>{1}", errorMessageReceivedDate, errorMessageNotes)
        ElseIf Not (String.IsNullOrEmpty(errorMessageReceivedDate) AndAlso String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageAmount)) Then
            finalErrorMessage = String.Format("{0}<br>{1}<br>{2}", errorMessageAmount, errorMessageReceivedDate, errorMessageNotes)
        End If

        'Display message if validation fails
        If isValid = False Then
            HelperFunctions.ShowMessageToUser(finalErrorMessage, EnumMessageTypes.Error, Nothing)
            Exit Function
        End If

        Return isValid
    End Function

    ''' <summary>
    '''  Set Credit details entered by user
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetInputControlDetails()
        Dim creditDetail As New YMCAObjects.DCTools.YMCACredit

        creditDetail = YmcaCreditDetail
        If Not String.IsNullOrEmpty(txtAmount.Text) Then
            creditDetail.AddedCredit = txtAmount.Text
        End If
        If Not String.IsNullOrEmpty(txtAmount.Text) Then
            creditDetail.ReceivedDate = txtReceivedDate.Text
        End If
        If Not String.IsNullOrEmpty(txtNotes.Text) Then
            creditDetail.UserNotes = txtNotes.Text
        End If
        creditDetail.CreditSourceCode = "OVRPAY"

        Me.YmcaCreditDetail = creditDetail
    End Sub

    ''' <summary>
    ''' Sets details which needs to be displayed on review tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayReviewTabDetails()
        Dim creditDetail As YMCAObjects.DCTools.YMCACredit
        Dim systemNotes As String

        If Not YmcaCreditDetail Is Nothing Then
            creditDetail = YmcaCreditDetail
            systemNotes = String.Format(creditDetail.SystemNotes, creditDetail.AddedCredit, creditDetail.ReceivedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture))

            lblReviewDispYMCANo.Text = creditDetail.YMCANo
            lblReviewDispYMCAName.Text = creditDetail.YMCAName
            lblReviewDispCurrentCredit.Text = String.Format("${0}", creditDetail.CurrentCredit)
            lblReviewDispAddedCredit.Text = String.Format("${0}", creditDetail.AddedCredit)
            lblReviewDispTotalCredit.Text = String.Format("${0}", (Decimal.Parse(creditDetail.CurrentCredit) + Decimal.Parse(creditDetail.AddedCredit)).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
            lblReviewDispCreditSrcCode.Text = creditDetail.CreditSourceCode
            lblReviewDispRecdDate.Text = creditDetail.ReceivedDate
            lblReviewDispRecdAcctDate.Text = String.Format("{0:MM/dd/yyy}", creditDetail.ReceivedAccountingDate)

            If Not String.IsNullOrEmpty(creditDetail.UserNotes) Then
                lblReviewDispNotes.Text = String.Format("{0} {1}", systemNotes, creditDetail.UserNotes)
            Else
                lblReviewDispNotes.Text = systemNotes
            End If
        End If
    End Sub

    ''' <summary>
    ''' Sets details which need to be displayed on apply credit tab after coming back from Review Tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayApplyCreditTabDetails()
        If Not Me.YmcaCreditDetail Is Nothing Then
            lblDispYMCANo.Text = YmcaCreditDetail.YMCANo
            lblDispYMCAName.Text = YmcaCreditDetail.YMCAName
            lblDispCurrentCredit.Text = String.Format("${0}", YmcaCreditDetail.CurrentCredit)
            txtAmount.Text = YmcaCreditDetail.AddedCredit
            txtReceivedDate.Text = YmcaCreditDetail.ReceivedDate
            txtNotes.Text = YmcaCreditDetail.UserNotes
        End If
    End Sub

    ''' <summary>
    ''' Display tab details
    ''' </summary>
    ''' <param name="selectedTab">Apply credit/Review & Submit Tab</param>
    ''' <remarks></remarks>
    Private Sub ManageTabs(ByVal selectedTab As Integer)
        If selectedTab <> 0 Then
            'Set Review tab selection and also information to be displayed
            If selectedTab = reviewTab Then
                divApplyCredit.Style.Add("display", "none")
                tdApplyCredit.Attributes("class") = "InActiveTab"
                divReviewAndSubmit.Style.Add("display", "block")
                tdReviewAndSubmit.Attributes("class") = "ActiveTab"
                btnPrevious.Style.Add("display", "block")
                btnNext.Style.Add("display", "none")
                btnSaveYMCACredit.Style.Add("display", "block")
                DisplayReviewTabDetails()
                'Set Apply credit tab selection and also information to be displayed
            ElseIf selectedTab = applyCreditTab Then
                divReviewAndSubmit.Style.Add("display", "none")
                tdReviewAndSubmit.Attributes("class") = "InActiveTab"
                divApplyCredit.Style.Add("display", "block")
                tdApplyCredit.Attributes("class") = "ActiveTab"
                btnPrevious.Style.Add("display", "none")
                btnNext.Style.Add("display", "block")
                btnSaveYMCACredit.Style.Add("display", "none")
                DisplayApplyCreditTabDetails()
            End If
        End If
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            'Below method will display details previously entered by user on apply credit tab, after coming back from review and submit tab.
            ManageTabs(applyCreditTab)
        Catch ex As Exception
            HelperFunctions.LogException("btnPrevious_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        Dim activityLogEntry As YMCAObjects.YMCAActionEntry
        Dim ymcaNo As Dictionary(Of String, String)
        Try
            'if user has read-only or none rights, message will be displayed and further process will not happen.
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                'CreateDataForActivityLog method will prepare data for yrs activity log
                activityLogEntry = New YMCAObjects.YMCAActionEntry
                activityLogEntry = CreateDataForActivityLog(YmcaCreditDetail)

                ymcaNo = New Dictionary(Of String, String)
                ymcaNo.Add("YMCANo", YmcaCreditDetail.YMCANo)

                If Not Me.YmcaCreditDetail Is Nothing AndAlso Not activityLogEntry Is Nothing Then
                    'Below method will be called to save credits and activity log details
                    If (YMCARET.YmcaBusinessObject.DCToolsBO.SaveYMCACredits(YmcaCreditDetail, activityLogEntry)) Then
                        ClearSessionValues()

                        'After sucessfull entry, acknowledgment message will be shown to user on find YMCA screen
                        HelperFunctions.ShowMessageOnNextPage(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_YMCACREDITS_SUCCESS, Nothing, ymcaNo)
                        Response.Redirect("FindYmcaInfo.aspx?Name=DCToolsAddYmcaCredit", True)
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            activityLogEntry = Nothing
            ymcaNo = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Sets and get details for entry into activity log
    ''' </summary>
    ''' <param name="ymcaCreditDetail">YMCA Credit Detail</param>
    ''' <returns>Data for entry into Activity log</returns>
    ''' <remarks></remarks>
    Private Function CreateDataForActivityLog(ByVal ymcaCreditDetail As YMCAObjects.DCTools.YMCACredit) As YMCAObjects.YMCAActionEntry
        Dim action As New YMCAObjects.YMCAActionEntry

        If Not ymcaCreditDetail Is Nothing Then
            action.Action = YMCAObjects.ActionYRSActivityLog.ADD_YMCA_CREDITS
            action.ActionBy = HttpContext.Current.Session("LoginId")
            action.Data = String.Empty
            action.EntityId = ymcaCreditDetail.YMCAID
            action.EntityType = YMCAObjects.EntityTypes.YMCA
            action.Module = YMCAObjects.Module.DCTools
            action.SuccessStatus = True
        End If

        Return action
    End Function

    ''' <summary>
    ''' Clear session values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSessionValues()
        Me.ReadOnlyWarningMessage = Nothing
        Session("IsReadOnlyAccess") = Nothing
        Me.YmcaCreditDetail = Nothing
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessionValues()
            Response.Redirect("FindYmcaInfo.aspx?Name=DCToolsAddYmcaCredit", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' Check and returns error message if credit amount is more than that of threshold amount specified in configuration table
    ''' </summary>
    ''' <param name="creditAmount">Credit Amount</param>
    ''' <returns>Error message</returns>
    ''' <remarks></remarks>
    Private Function ValidateCreditAmount(ByVal creditAmount As Decimal) As String
        Dim ymcaCreditValidationDetails As New DataTable
        Dim errorMessage As String = String.Empty
        Dim errorMessageNo As Integer
        Dim thresholdAmount As New Dictionary(Of String, String)

        'Get Message no and threshold limit 
        ymcaCreditValidationDetails = YMCARET.YmcaBusinessObject.DCToolsBO.ValidateCreditAmount(Decimal.Parse(creditAmount))
        If HelperFunctions.isNonEmpty(ymcaCreditValidationDetails) Then
            errorMessageNo = ymcaCreditValidationDetails.Rows(0)("MessageNo")
            thresholdAmount.Add("THRESHOLDLIMIT", ymcaCreditValidationDetails.Rows(0)("ThresholdAmount"))
            'Get Message text based on message no
            If errorMessageNo <> 0 Then
                errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(errorMessageNo, thresholdAmount).DisplayText
            End If
        End If

        Return errorMessage
    End Function
End Class