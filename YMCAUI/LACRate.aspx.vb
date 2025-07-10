'************************************************************************************************************************
' Author: Santosh Bura 
' Created on: 07/24/2018
' Summary of Functionality: Edit/Update Prime Interest Rate in Loan Admin Module.
' Declared in Version: 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' Vinayan C		              | 2018.11.14	 |	20.6.0         | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Public Class LACRate
    Inherits System.Web.UI.Page

    'This will store all the information related to loan prime rate interest rate and prime rate history
    Public Property PrimeRateDetails() As DataSet
        Get
            If Not ViewState("PrimeRateDetails") Is Nothing Then
                Return TryCast(ViewState("PrimeRateDetails"), DataSet)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataSet)
            ViewState("PrimeRateDetails") = value
        End Set
    End Property
    'This will strore max prime rate config value
    Public Property MaxPrimeRate() As Integer
        Get
            If Not ViewState("MaxPrimeRate") Is Nothing Then
                Return DirectCast(ViewState("MaxPrimeRate"), Integer)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("MaxPrimeRate") = value
        End Set
    End Property

    'START : VC | 2018.11.09 | YRS-AT-4017 -  Declared properties
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
    'END : VC | 2018.11.09 | YRS-AT-4017 -  Declared properties

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim accessPermission As Integer 'VC | 2018.11.09 | YRS-AT-4017 -  Declared variable to store page access rights
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "YRS", "BindEvents();", True)
            'START : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
            accessPermission = CheckFormAuthorizations()
            If accessPermission <> 0 AndAlso accessPermission <> -3 Then
                Session("accessPermission") = Nothing
                'END : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
                If Not Me.IsPostBack Then
                    CheckAccessRights() 'VC | 2018.11.09 | YRS-AT-4017 - Calling method to check read only rights.
                    'Reset Controls
                    Me.ResetContols(True)
                    'Get Prime rate details
                    Me.LoadPrimeRateHistory()
                    BindGrid()
                    GetMaxPrimeRateConfigValue()
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACRate_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnEditRate_Click(sender As Object, e As EventArgs) Handles btnEditRate.Click
        Dim checkSecurity As String
        Try
            'START: VC | 2018.11.09 | YRS-AT-4017 - Commented existing code and added new code to show error message if the page rights is read only
            'checkSecurity = SecurityCheck.Check_Authorization("btnEditRate", Convert.ToInt32(Session("LoggedUserKey")))
            'If Not checkSecurity.Equals("True") Then
            '    HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
            '    Exit Sub
            'Else
            '    ResetContols(False)
            'End If
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                checkSecurity = SecurityCheck.Check_Authorization("btnEditRate", Convert.ToInt32(Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                    Exit Sub
                Else
                    ResetContols(False)
                End If
            End If
            'END: VC | 2018.11.09 | YRS-AT-4017 - Commented existing code and added new code to show error message if the page rights is read only
        Catch ex As Exception
            HelperFunctions.LogException("LACRate_btnEditRate_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        Try
            YMCARET.YmcaBusinessObject.LoanInformationBOClass.SavePrimeRate(Me.txtNewPrimeRate.Text)
            HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_LOAN_INTEREST_RATE_SUCCESSFUL)
            LoadPrimeRateHistory()
            BindGrid()
            ResetContols(True)
        Catch ex As Exception
            HelperFunctions.LogException("LACRate_btnConfirmDialogYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Binds Prime rate history data to grid 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindGrid()
        Dim rateHistory As DataTable
        Try
            If Not Me.PrimeRateDetails Is Nothing AndAlso HelperFunctions.isNonEmpty(Me.PrimeRateDetails.Tables(1)) Then
                rateHistory = PrimeRateDetails.Tables(1)
            End If
            lblcount.Text = rateHistory.Rows.Count
            gvLoan.DataSource = rateHistory
            gvLoan.DataBind()
        Catch
            Throw
        Finally
            rateHistory = Nothing
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("LACRate_btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnCancelRate_Click(sender As Object, e As EventArgs) Handles btnCancelRate.Click
        Try
            ResetContols(True)
        Catch ex As Exception
            HelperFunctions.LogException("LACRate_btnCancelRate_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Reset the controls based on input value
    ''' </summary>
    ''' <param name="resetValue"></param>
    ''' <remarks></remarks>
    Private Sub ResetContols(resetValue As Boolean)
        Me.txtNewPrimeRate.Text = "0.00"
        Me.btnEditRate.Enabled = resetValue
        Me.btnSaveRate.Enabled = Not resetValue
        Me.btnCancelRate.Enabled = Not resetValue
        Me.txtNewPrimeRate.Enabled = Not resetValue
    End Sub

    ''' <summary>
    ''' Method to get the prime rate history details
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadPrimeRateHistory()
        Dim primeRateDetails As DataSet
        Try
            ResetContols(True)
            'get current prime rate and prime rate history
            primeRateDetails = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetPrimeRate()

            If (HelperFunctions.isNonEmpty(primeRateDetails) And HelperFunctions.isNonEmpty(primeRateDetails.Tables("PrimeRate").Rows(0)("chvValue").ToString())) Then
                lblRate.Text = primeRateDetails.Tables("PrimeRate").Rows(0)("chvValue").ToString()
                lblRate.Text = String.Format("{0}%", lblRate.Text)
                Me.PrimeRateDetails = primeRateDetails
            Else
                lblRate.Text = "0.00%"
            End If
        Catch
            Throw
        Finally
            primeRateDetails = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' Get max config value for prime rate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetMaxPrimeRateConfigValue()
        Dim metaConfigurationKey As DataSet
        Try
            metaConfigurationKey = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("PRIME_RATE_MAX")
            If HelperFunctions.isNonEmpty(metaConfigurationKey) AndAlso HelperFunctions.isNonEmpty(metaConfigurationKey.Tables(0)) Then
                Me.MaxPrimeRate = CType(metaConfigurationKey.Tables(0).Rows(0)("Value").ToString(), Integer)
            End If
        Catch
            Throw
        Finally
            metaConfigurationKey = Nothing
        End Try
    End Sub

    Private Sub btnSaveRate_Click(sender As Object, e As EventArgs) Handles btnSaveRate.Click
        Dim checkSecurity As String
        Dim dictParameter As Dictionary(Of String, String)
        Dim currentRate As String ' VC | 2018.11.14 | YRS-AT-4018 -  Declared variable
        Try
            dictParameter = New Dictionary(Of String, String)
            'Check for Access Permission
            checkSecurity = (SecurityCheck.Check_Authorization("btnEditRate", Convert.ToInt32(Session("LoggedUserKey"))))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'Check user has entered valid prime rate
            If Decimal.Parse(Me.txtNewPrimeRate.Text) < 0.0 Or Decimal.Parse(Me.txtNewPrimeRate.Text) > Decimal.Parse(Me.MaxPrimeRate) Then
                dictParameter.Add("Max", Me.MaxPrimeRate)
                HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_LOAN_INTEREST_RANGE, dictParameter)
            Else
                'START: VC | 2018.11.14 | YRS-AT-4018 -  Commented existing code and added new code to check new rate and current rate are same
                'ScriptManager.RegisterStartupScript(Me, GetType(Page), "Open", "ShowDialog();", True)
                currentRate = lblRate.Text.Replace("%", "").Trim()
                'If current rae and new rate are equal then show error message else show confirmation message
                If (currentRate = txtNewPrimeRate.Text) Then
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_ADMIN_LOAN_RATE_NOT_CHANGED).DisplayText, EnumMessageTypes.Error)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(Page), "Open", "ShowDialog();", True)
                End If
                'END: VC | 2018.11.14 | YRS-AT-4018 -  Commented existing code and added new code to check new rate and current rate are same
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnSaveRate_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    'START : VC | 2018.11.09 | YRS-AT-4017 -  Added method to check page authorization
    Private Function CheckFormAuthorizations() As Integer
        Dim accessPermission As Integer
        Dim userId As Integer
        Dim formName As String
        Try
            userId = Convert.ToInt32(Session("LoggedUserKey"))
            formName = "LACWebLoans.aspx"
            accessPermission = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(userId, formName)
            If Not (Session("accessPermission") Is Nothing) Then
                Session("accessPermission") = Nothing
                Response.Redirect("MainWebForm.aspx", False)
            Else
                Session("accessPermission") = accessPermission
            End If
            'Checking whether the user is authorized to view this page or not.
            If accessPermission = 0 Then
                MessageBox.Show(PlaceHolderSecurityCheck, "YMCA-YRS", "You are not authorized to view this page.", MessageBoxButtons.Stop)
            ElseIf accessPermission = -3 Then
                MessageBox.Show(PlaceHolderSecurityCheck, "YMCA-YRS", "There is no mapping for this item to the logged in User.", MessageBoxButtons.Stop)
            End If
            Return accessPermission
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'END : VC | 2018.11.09 | YRS-AT-4017 -  Added method to check page authorization

    'START : VC | 2018.11.09 | YRS-AT-4017 -  Added method to check page read only rights
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("LACWebLoans.aspx", Convert.ToInt32(Session("LoggedUserKey")))
            If Not String.IsNullOrEmpty(readOnlyWarningMessage) AndAlso Not readOnlyWarningMessage.ToUpper().Contains("TRUE") Then
                Me.IsReadOnlyAccess = True
                Me.ReadOnlyWarningMessage = readOnlyWarningMessage
            Else
                Me.IsReadOnlyAccess = False
                Me.ReadOnlyWarningMessage = ""
            End If
        Catch
            Throw
        Finally
            readOnlyWarningMessage = Nothing
        End Try
    End Sub
    'END : VC | 2018.11.09 | YRS-AT-4017 -  Added method to check page read only rights
End Class