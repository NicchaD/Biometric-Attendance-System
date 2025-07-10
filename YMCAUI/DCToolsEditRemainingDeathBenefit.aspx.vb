'/**************************************************************************************************************/
'// Author: Santosh Bura
'// Created on: 09/13/2017
'// Summary of Functionality: New Admin Screen to Edit Remaining Death Benefit amount
'// Declared in Version: 20.4.0 | YRS-AT-3541 -  YRS enh: Data Corrections Tool -Admin screen function to allow modification of RDB amounts (excessive data corrections) 
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'// 			                | 	           |		         | 
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/
Public Class DCToolsEditRemainingDeathBenefit
    Inherits System.Web.UI.Page

    Dim editTab As Integer = 1
    Dim reviewTab As Integer = 2


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

    Public Property EditRemainingDeathBenefitAmountDetails() As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        Get
            If Not ViewState("EditRemainingDeathBenefitAmount") Is Nothing Then
                Return TryCast(ViewState("EditRemainingDeathBenefitAmount"), YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount)
            ViewState("EditRemainingDeathBenefitAmount") = value
        End Set
    End Property

    Public Property TempTransactInsertEntriesTable() As DataTable
        Get
            If Not ViewState("TempTransactInsertEntriesTable") Is Nothing Then
                Return TryCast(ViewState("TempTransactInsertEntriesTable"), DataTable)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataTable)
            ViewState("TempTransactInsertEntriesTable") = value
        End Set
    End Property

    'This will store all the information related to particpant's annuities, personal details, transacts sum information and system generated notes message
    Public Property RemainingDeathBenefitPageInformation() As DataSet
        Get
            If Not ViewState("RemainingDeathBenefitPageInformation") Is Nothing Then
                Return TryCast(ViewState("RemainingDeathBenefitPageInformation"), DataSet)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataSet)
            ViewState("RemainingDeathBenefitPageInformation") = value
        End Set
    End Property

    Public ReadOnly Property FundEventID() As String
        Get
            If Not Session("FundID") Is Nothing Then
                Return CType(Session("FundID"), String)
            Else
                Return String.Empty
            End If
        End Get
    End Property


#Region " Index variables "     'Index varibles to be used in case of the datagrid columns are required to change 
    Dim annuitySource As Integer = 1
    Dim planType As Integer = 2
    Dim purchaseDate As Integer = 3
    Dim currentPayment As Integer = 4
    Dim deathBenefit As Integer = 5
    Dim remainingDeathBenefit As Integer = 6
    Dim annuityID As Integer = 7

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        txtTransactDate.MaxDate = Date.Today
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
        Try
            If Not IsPostBack Then
                'Clear session variables
                ClearSession()
                'Set Readonly access right and message value
                CheckAccessRights()
                'Get death benefit related pag information 
                If Not Me.FundEventID Is Nothing Then
                    Me.LoadAnnuities(Me.FundEventID)
                End If
                'Set Module name
                SetModuleName()
                If btnSubmit.Visible = True Then
                    btnSubmit.Attributes.Add("onclick", "javascript:return ShowDialog();")
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DCToolsEditRemainingDeathBenefit --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


    ' This method will check read-only access rights
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("FindInfo.aspx?Name=DCToolsEditRemainingDeathBenefit", Convert.ToInt32(Session("LoggedUserKey")))
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

    ' This method will set module details header section
    Private Sub SetModuleName()
        Dim moduleName As Label
        Dim fundNo, SSN, lastName, middleName, firstName As String
        Try
            fundNo = String.Empty
            SSN = String.Empty
            lastName = String.Empty
            middleName = String.Empty
            firstName = String.Empty

            moduleName = Master.FindControl("LabelModuleName")
            If moduleName IsNot Nothing Then
                'Check the person details exists or not of the participant if exists assign them to local variables
                If Not Me.RemainingDeathBenefitPageInformation Is Nothing AndAlso HelperFunctions.isNonEmpty(Me.RemainingDeathBenefitPageInformation.Tables(3)) Then
                    If Convert.ToString(Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(1)).Trim.ToUpper() <> "&NBSP;" Then
                        fundNo = Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(1).ToString()
                    End If

                    If Convert.ToString(Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(2)).Trim.ToUpper() <> "&NBSP;" Then
                        SSN = Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(2).ToString()
                    End If

                    If Convert.ToString(Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(3)).Trim.ToUpper() <> "&NBSP;" Then
                        lastName = Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(3).ToString()
                    End If

                    If Convert.ToString(Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(4)).Trim.ToUpper() <> "&NBSP;" Then
                        middleName = Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(4).ToString()
                    End If

                    If Convert.ToString(Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(5)).Trim.ToUpper() <> "&NBSP;" Then
                        firstName = Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(5).ToString()
                    End If
                End If
                moduleName.Text = String.Format("Administration > DC Tools >  Edit Remaining Death Benefit  -  Fund Id {0} -  {1}, {2} {3}", fundNo, lastName, firstName, middleName)
            End If
        Catch
            Throw
        Finally
            moduleName = Nothing
        End Try
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSession()
            Session("Page") = "DCToolsEditRemainingDeathBenefit"
            Response.Redirect("FindInfo.aspx?Name=DCToolsEditRemainingDeathBenefit", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DCToolsEditRemainingDeathBenefit_btnClose_Click", ex)
        End Try
    End Sub

    ' This method will load all the annuities (retirement type ) of the person and display it on page load
    Public Sub LoadAnnuities(FundeventId As String)
        Dim annuitiesInformation As DataSet
        Dim errorCode As Integer
        Try
            annuitiesInformation = Nothing
            If Not FundeventId Is Nothing Then
                'Get Annuities details  information
                annuitiesInformation = YMCARET.YmcaBusinessObject.DCToolsBO.GetAnnuitiesInformation(FundeventId, errorCode)
            End If
            'If linkage between disbursments and annuities is missing then display error message on the screen
            If (errorCode <> 0) Then
                HelperFunctions.ShowMessageToUser(errorCode, Nothing, Nothing)
                Me.gvAnnuitiesList.DataSource = Nothing
                divLegendInformation.Visible = False
            ElseIf HelperFunctions.isNonEmpty(annuitiesInformation.Tables(0)) Then
                Me.gvAnnuitiesList.DataSource = annuitiesInformation.Tables(0)
            End If

            If HelperFunctions.isNonEmpty(annuitiesInformation.Tables(0)) Or HelperFunctions.isNonEmpty(annuitiesInformation.Tables(1)) Or HelperFunctions.isNonEmpty(annuitiesInformation.Tables(2)) Or HelperFunctions.isNonEmpty(annuitiesInformation.Tables(3)) Then
                Me.RemainingDeathBenefitPageInformation = annuitiesInformation
            End If

            Me.gvAnnuitiesList.DataBind()
        Catch
            Throw
        Finally
            annuitiesInformation = Nothing
        End Try
    End Sub

    Private Sub gvAnnuitiesList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvAnnuitiesList.SelectedIndexChanged
        Dim i As Integer
        Dim imageButton As ImageButton
        Dim remainingDeathBenefitAmount As String
        Dim selectedAnnuityID As String
        Dim sessionDetails As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        Dim data As New DataSet
        Dim transactTable As New DataTable
        Dim row As DataRow()
        Dim totalSumOfTransactAmt As Decimal
        Dim currentRemainingAmount As Decimal = 0.0
        Dim mismatchedAmount As Decimal = 0.0
        'change the icon of the selected row
        Try
            While i < Me.gvAnnuitiesList.Rows.Count
                If i = Me.gvAnnuitiesList.SelectedIndex Then
                    imageButton = New ImageButton
                    imageButton = Me.gvAnnuitiesList.Rows(i).FindControl("ImageButtonSelect")
                    If Not imageButton Is Nothing Then
                        imageButton.ImageUrl = "images\selected.gif"
                    End If
                Else
                    imageButton = New ImageButton
                    imageButton = Me.gvAnnuitiesList.Rows(i).FindControl("ImageButtonSelect")
                    If Not imageButton Is Nothing Then
                        imageButton.ImageUrl = "images\select.gif"
                    End If
                End If
                i = i + 1
            End While
            'Clearing input controls
            txtNewAmount.Text = ""
            txtTransactDate.Text = ""
            txtNotes.Text = ""
            divEditAmount.Visible = False
            btnNext.Enabled = False

            sessionDetails = Nothing
            'Fill properties of object created of Remaining Death Benefit class 
            If Me.gvAnnuitiesList.SelectedRow.Cells(remainingDeathBenefit).Text.Trim.ToUpper() <> "&NBSP;" Then
                sessionDetails = New YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount()
                If Not Me.RemainingDeathBenefitPageInformation Is Nothing Then
                    data = DirectCast(Me.RemainingDeathBenefitPageInformation, DataSet)
                End If

                selectedAnnuityID = Convert.ToString(Me.gvAnnuitiesList.SelectedRow.Cells(annuityID).Text)
                If Not selectedAnnuityID Is Nothing Then
                    sessionDetails.AnnuityID = selectedAnnuityID
                End If
                sessionDetails.SelectedAnnuityIndex = gvAnnuitiesList.SelectedIndex

                remainingDeathBenefitAmount = Convert.ToString(Me.gvAnnuitiesList.SelectedRow.Cells(remainingDeathBenefit).Text)
                If Not remainingDeathBenefitAmount Is Nothing Then
                    currentRemainingAmount = Convert.ToDecimal(remainingDeathBenefitAmount)
                    lblDispOldAmount.Text = String.Format("${0}", remainingDeathBenefitAmount)
                    sessionDetails.OldAmount = currentRemainingAmount
                End If

                If HelperFunctions.isNonEmpty(data.Tables(1)) Then
                    transactTable = data.Tables(1)
                    If HelperFunctions.isNonEmpty(transactTable) Then
                        row = transactTable.Select("guiAnnuityID= '" & selectedAnnuityID & "'")
                        If Not row Is Nothing Then
                            If row.Length > 0 Then
                                totalSumOfTransactAmt = Convert.ToDecimal(row(0)("Total").ToString())
                                If Not Convert.IsDBNull(row(0)("AllowedTransactDate")) Then
                                    sessionDetails.AllowedTransactDate = Convert.ToDateTime(row(0)("AllowedTransactDate"))
                                Else
                                    sessionDetails.AllowedTransactDate = Date.MinValue
                                End If
                            End If
                        End If
                    End If
                    'if remaining death benefit amount and sum of transacts is mismatch then raise error messsage 
                    If (currentRemainingAmount <> totalSumOfTransactAmt) Then
                        mismatchedAmount = currentRemainingAmount - totalSumOfTransactAmt
                        sessionDetails.IsMismatchAmount = True
                        sessionDetails = Nothing
                        HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_MISMATCH_ERROR).DisplayText, EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If
                End If

                If HelperFunctions.isNonEmpty(data.Tables(2)) AndAlso data.Tables(2).Rows(0)("SystemNotes") <> "" Then
                    sessionDetails.SystemNotes = data.Tables(2).Rows(0)("SystemNotes").ToString()
                Else
                    sessionDetails.SystemNotes = ""
                End If

                If HelperFunctions.isNonEmpty(data.Tables(4)) AndAlso Not Convert.IsDBNull(data.Tables(4).Rows(0)("AdjustmentAllowedDate")) Then
                    sessionDetails.AdjustmentAllowedDate = Convert.ToDateTime(data.Tables(4).Rows(0)("AdjustmentAllowedDate"))
                Else
                    sessionDetails.AdjustmentAllowedDate = Date.MinValue
                End If

                If HelperFunctions.isNonEmpty(data.Tables(3)) Then
                    sessionDetails.RetireeID = data.Tables(3).Rows(0)(6).ToString()
                    sessionDetails.PerssID = data.Tables(3).Rows(0)(0).ToString()
                Else
                    sessionDetails.RetireeID = ""
                    sessionDetails.PerssID = ""
                End If

                divEditAmount.Visible = True
                btnNext.Enabled = True
            Else
                divEditAmount.Visible = False
                btnNext.Enabled = False
            End If
            Me.EditRemainingDeathBenefitAmountDetails = sessionDetails
        Catch ex As Exception
            HelperFunctions.LogException("DCToolsEditRemainingDeathBenefit --> gvAnnuitiesList_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            selectedAnnuityID = Nothing
            row = Nothing
            transactTable = Nothing
            data = Nothing
            sessionDetails = Nothing
            imageButton = Nothing
            remainingDeathBenefitAmount = Nothing
        End Try
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            If ValidateControls() Then
                SetInputControlDetails()
                ManageTabs(reviewTab)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DCToolsEditRemainingDeathBenefit_btnNext_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ' Function to validate input controls on Edit Remaining Death Benefit  tab
    Private Function ValidateControls() As Boolean
        Dim errorMessageAmount As String = String.Empty
        Dim errorMessageTransactDate As String = String.Empty
        Dim errorMessageNotes As String = String.Empty
        Dim finalErrorMessage As String = String.Empty
        Dim isValid As Boolean = True
        Dim tempAmount As Decimal
        Dim purchaseDateDetail As Dictionary(Of String, String)
        Dim sessionDetails As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        sessionDetails = Me.EditRemainingDeathBenefitAmountDetails

        If sessionDetails Is Nothing Then
            sessionDetails = New YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        End If

        If Not Decimal.TryParse(txtNewAmount.Text.Trim(), tempAmount) Then
            errorMessageAmount = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_AMOUNT).DisplayText
            isValid = False
        Else
            If tempAmount < 0.0 Then
                errorMessageAmount = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_AMOUNT).DisplayText
                isValid = False
            ElseIf tempAmount = sessionDetails.OldAmount Then
                errorMessageAmount = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_UNCHANGEDRDB_ERROR).DisplayText
                isValid = False
            End If
        End If

        If String.IsNullOrEmpty(txtNotes.Text.Trim()) Then
            errorMessageNotes = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_NOTES).DisplayText
            isValid = False
        End If

        If String.IsNullOrEmpty(txtTransactDate.Text.Trim) Then
            errorMessageTransactDate = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_DATE).DisplayText
            isValid = False
        ElseIf Date.Parse(txtTransactDate.Text) > Date.Today Then
            errorMessageTransactDate = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_TRANSACTDATE_FUTURE).DisplayText
            isValid = False
        ElseIf Date.Parse(txtTransactDate.Text) < sessionDetails.AllowedTransactDate Then
            purchaseDateDetail = New Dictionary(Of String, String)
            purchaseDateDetail.Add("PurchaseDate", sessionDetails.AllowedTransactDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture))
            errorMessageTransactDate = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_TRANSACTDATE_EARLIER, purchaseDateDetail).DisplayText
            isValid = False
        End If

        If Not String.IsNullOrEmpty(errorMessageAmount) AndAlso String.IsNullOrEmpty(errorMessageTransactDate) AndAlso String.IsNullOrEmpty(errorMessageNotes) Then
            finalErrorMessage = errorMessageAmount
        ElseIf Not String.IsNullOrEmpty(errorMessageTransactDate) AndAlso String.IsNullOrEmpty(errorMessageAmount) AndAlso String.IsNullOrEmpty(errorMessageNotes) Then
            finalErrorMessage = errorMessageTransactDate
        ElseIf Not String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageTransactDate) AndAlso String.IsNullOrEmpty(errorMessageAmount) Then
            finalErrorMessage = errorMessageNotes
        ElseIf Not String.IsNullOrEmpty(errorMessageAmount) AndAlso Not String.IsNullOrEmpty(errorMessageTransactDate) AndAlso String.IsNullOrEmpty(errorMessageNotes) Then
            finalErrorMessage = String.Format("{0}<br>{1}", errorMessageAmount, errorMessageTransactDate)
        ElseIf Not String.IsNullOrEmpty(errorMessageAmount) AndAlso Not String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageTransactDate) Then
            finalErrorMessage = String.Format("{0}<br>{1}", errorMessageAmount, errorMessageNotes)
        ElseIf Not String.IsNullOrEmpty(errorMessageTransactDate) AndAlso Not String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageAmount) Then
            finalErrorMessage = String.Format("{0}<br>{1}", errorMessageTransactDate, errorMessageNotes)
        ElseIf Not (String.IsNullOrEmpty(errorMessageTransactDate) AndAlso String.IsNullOrEmpty(errorMessageNotes) AndAlso String.IsNullOrEmpty(errorMessageAmount)) Then
            finalErrorMessage = String.Format("{0}<br>{1}<br>{2}", errorMessageAmount, errorMessageTransactDate, errorMessageNotes)
        End If
        'display final error message if any
        If isValid = False Then
            HelperFunctions.ShowMessageToUser(finalErrorMessage, EnumMessageTypes.Error, Nothing)
            Exit Function
        End If

        Return isValid
    End Function

    ' This method will display tab details
    Private Sub ManageTabs(ByVal selectedTab As Integer)
        Try
            If selectedTab <> 0 Then
                If selectedTab = reviewTab Then
                    divEditRemainingDeathBenefit.Style.Add("display", "none")
                    tdEditRDBAmount.Attributes("class") = "InActiveTab"
                    divReviewAndSubmit.Style.Add("display", "block")
                    tdReviewAndSubmit.Attributes("class") = "ActiveTab"
                    btnPrevious.Style.Add("display", "block")
                    btnNext.Style.Add("display", "none")
                    btnSubmit.Style.Add("display", "block")
                    divRequiredFields.Style.Add("display", "none")
                    DisplayReviewTabDetails()
                ElseIf selectedTab = editTab Then
                    divReviewAndSubmit.Style.Add("display", "none")
                    tdReviewAndSubmit.Attributes("class") = "InActiveTab"
                    divEditRemainingDeathBenefit.Style.Add("display", "block")
                    tdEditRDBAmount.Attributes("class") = "ActiveTab"
                    btnPrevious.Style.Add("display", "none")
                    btnNext.Style.Add("display", "block")
                    btnSubmit.Style.Add("display", "none")
                    divRequiredFields.Style.Add("display", "block")
                    DisplayEditRemainingDeathBenefitTabDetails()
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    ' This Method will set details which need to be displayed on review tab
    Private Sub DisplayReviewTabDetails()
        Dim sessionDetails As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        Try
            sessionDetails = Me.EditRemainingDeathBenefitAmountDetails
            If Not sessionDetails Is Nothing Then
                lblReviewDispOldAmount.Text = String.Format("${0}", sessionDetails.OldAmount)
                lblReviewDispNewAmount.Text = String.Format("${0}", sessionDetails.NewAmount)
                lblReviewDispTransactDate.Text = sessionDetails.TransactDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)

                If Not String.IsNullOrEmpty(sessionDetails.UserNotes) Then
                    lblReviewDispNotes.Text = String.Format("{0} {1}", sessionDetails.SystemNotes, sessionDetails.UserNotes)
                Else
                    lblReviewDispNotes.Text = sessionDetails.SystemNotes
                End If
            End If
        Catch
            Throw
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    ' This Method will set details which need to be displayed on Edit Remaining Death Benefit tab after coming back from Review Tab
    Private Sub DisplayEditRemainingDeathBenefitTabDetails()
        Dim sessionDetails As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        Try
            sessionDetails = Me.EditRemainingDeathBenefitAmountDetails
            If Not sessionDetails Is Nothing Then
                lblDispOldAmount.Text = String.Format("${0}", sessionDetails.OldAmount)
                txtNewAmount.Text = sessionDetails.NewAmount
                txtTransactDate.Text = sessionDetails.TransactDate
                txtNotes.Text = sessionDetails.UserNotes
            End If
        Catch
            Throw
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    ' This Method will set input details value into property
    Private Sub SetInputControlDetails()
        Dim sessionDetails As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount
        Dim systemNotes As String = String.Empty
        Dim transactEntries As DataTable
        Dim newRemainingDeathBenefitAmount As Decimal
        Dim remainingDeathBenefitAmount As Decimal

        sessionDetails = Me.EditRemainingDeathBenefitAmountDetails
        If sessionDetails Is Nothing Then
            sessionDetails = New YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount()
        End If

        If HelperFunctions.isNonEmpty(Me.RemainingDeathBenefitPageInformation.Tables(2)) AndAlso Me.RemainingDeathBenefitPageInformation.Tables(2).Rows(0)("SystemNotes") <> "" Then
            systemNotes = Me.RemainingDeathBenefitPageInformation.Tables(2).Rows(0)("SystemNotes").ToString()
        End If
        If Not String.IsNullOrEmpty(txtTransactDate.Text.Trim) Then
            sessionDetails.TransactDate = Convert.ToDateTime(txtTransactDate.Text)
        End If
        If Not String.IsNullOrEmpty(txtNewAmount.Text.Trim) Then
            sessionDetails.NewAmount = Convert.ToDecimal(txtNewAmount.Text.Trim)
            sessionDetails.TransactType = "DBADJ"
            'check if the new value is set to zero , if zero change transact type if the participant is deceased for more than 2 or more years
            If sessionDetails.NewAmount = 0.0 Then
                'Check if the participant is deceased or not.And checking the transact date with threshold deceased date of participant.
                If sessionDetails.AdjustmentAllowedDate <> Date.MinValue Then
                    If DateTime.Compare(sessionDetails.TransactDate, sessionDetails.AdjustmentAllowedDate) > 0 Then
                        sessionDetails.TransactType = "RDBFF"
                    End If
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(txtNotes.Text) Then
            sessionDetails.UserNotes = txtNotes.Text.Trim
        End If

        sessionDetails.SystemNotes = String.Format(systemNotes, sessionDetails.OldAmount, sessionDetails.NewAmount)
        newRemainingDeathBenefitAmount = Convert.ToDecimal(txtNewAmount.Text.Trim())
        If Not String.IsNullOrEmpty(sessionDetails.OldAmount) Then
            remainingDeathBenefitAmount = Convert.ToDecimal(sessionDetails.OldAmount)
        End If
        sessionDetails.NewDifferenceAmount = newRemainingDeathBenefitAmount - remainingDeathBenefitAmount
        Me.EditRemainingDeathBenefitAmountDetails = sessionDetails

        'Create records that will be inserted in transacts table
        CreateTempTransactInsertEntries(Me.EditRemainingDeathBenefitAmountDetails)
        If (Me.TempTransactInsertEntriesTable IsNot Nothing) Then
            transactEntries = CType(Me.TempTransactInsertEntriesTable, DataTable)
            If (HelperFunctions.isNonEmpty(transactEntries)) Then
                gvTransactView.DataSource = transactEntries
                gvTransactView.DataBind()
            End If
        End If
    End Sub

    'Records will be inserted into the AtsDeathBenefitTransacts table
    Private Sub CreateTempTransactInsertEntries(ByRef data As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount)
        'In this method values will be added in AtsDeathBenefitTransacts table
        Dim transactEntries As DataTable
        Dim row As DataRow
        Try
            Me.TempTransactInsertEntriesTable = Nothing
            transactEntries = New DataTable()
            transactEntries.Columns.Add("Transact Date")
            transactEntries.Columns.Add("Transact Type")
            transactEntries.Columns.Add("Amount")

            row = transactEntries.NewRow()
            row("Transact Date") = data.TransactDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            row("Transact Type") = data.TransactType
            row("Amount") = data.NewDifferenceAmount
            transactEntries.Rows.Add(row)

            transactEntries.AcceptChanges()
            Me.TempTransactInsertEntriesTable = transactEntries
        Catch
            Throw
        Finally
            transactEntries = Nothing
        End Try
    End Sub

    ' Function will save the records and redirects to findinfo page
    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        Dim activityLogEntry As YMCAObjects.YMCAActionEntry
        Dim messageParameters As Dictionary(Of String, String)
        Try
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                activityLogEntry = CreateDataForActivityLog(Me.EditRemainingDeathBenefitAmountDetails)

                'Update the Remainig death benefit amount and display acknowledgement message on findinfo screen
                If Not Me.EditRemainingDeathBenefitAmountDetails Is Nothing AndAlso Not activityLogEntry Is Nothing Then
                    If (YMCARET.YmcaBusinessObject.DCToolsBO.EditRemainingDeathBenefitAmount(Me.EditRemainingDeathBenefitAmountDetails, activityLogEntry)) Then
                        messageParameters = New Dictionary(Of String, String)
                        If Me.RemainingDeathBenefitPageInformation.Tables(3) IsNot Nothing Then
                            messageParameters.Add("FundNo", Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(1).ToString())
                            messageParameters.Add("Name", String.Format("{0}, {1} {2}", Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(3).ToString(), Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(5).ToString(), Me.RemainingDeathBenefitPageInformation.Tables(3).Rows(0)(4).ToString()))
                        End If
                        HelperFunctions.ShowMessageOnNextPage(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_DEATHBENEFIT_SUCCESS, Nothing, messageParameters)
                        Me.ClearSession()
                        Session("FundID") = Nothing
                        Response.Redirect("FindInfo.aspx?Name=DCToolsEditRemainingDeathBenefit", True)
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DCToolsEditRemainingDeathBenefit_btnConfirmDialogYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            activityLogEntry = Nothing
        End Try
    End Sub

    'set the details in activity log object
    Private Function CreateDataForActivityLog(ByVal EditRemainingDeathBenefitAmtDetails As YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount) As YMCAObjects.YMCAActionEntry
        Dim action As New YMCAObjects.YMCAActionEntry
        If Not EditRemainingDeathBenefitAmtDetails Is Nothing Then
            action.Action = YMCAObjects.ActionYRSActivityLog.EDIT_REMAINING_DEATH_BENEFIT
            action.ActionBy = HttpContext.Current.Session("LoginId")
            action.Data = ""
            action.EntityId = EditRemainingDeathBenefitAmtDetails.PerssID
            action.EntityType = YMCAObjects.EntityTypes.PERSON
            action.Module = YMCAObjects.Module.DCTools
            action.SuccessStatus = True
        End If
        Return action
    End Function

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            ManageTabs(editTab)
        Catch ex As Exception
            HelperFunctions.LogException("btnPrevious_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub ClearSession()
        Me.EditRemainingDeathBenefitAmountDetails = Nothing
        Me.RemainingDeathBenefitPageInformation = Nothing
        Me.TempTransactInsertEntriesTable = Nothing
    End Sub
End Class