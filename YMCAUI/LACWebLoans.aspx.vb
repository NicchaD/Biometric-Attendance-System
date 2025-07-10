'************************************************************************************************************************
' Author: Vinayan C
' Created on: 7/19/2018
' Summary of Functionality: Pending loan information and filter
' Declared in Version: 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name               | Date        | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
' Shilpa N                     | 02/28/2019  |               | YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Imports YMCARET.YmcaBusinessObject.MetaMessageBO
Imports YMCAObjects.MetaMessageList
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class LACWebLoans
    Inherits System.Web.UI.Page

    Const mConst_gvLoanPendingApprovalIndexOfFundId As Integer = 0
    Const mConst_gvLoanPendingApprovalIndexOfDateTime As Integer = 1
    Const mConst_gvLoanPendingApprovalIndexOfParticipantName As Integer = 2
    Const mConst_gvLoanPendingApprovalIndexOfLoanAmount As Integer = 3
    Const mConst_gvLoanPendingApprovalIndexOfLoanStatus As Integer = 4
    Const mConst_gvLoanPendingApprovalIndexOfPaymentMethod As Integer = 5
    Const mConst_gvLoanPendingApprovalIndexOfDate As Integer = 6


#Region "Properties"
    Public Property SearchCriteria() As String
        Get
            If Not Session("SearchCriteria") Is Nothing Then
                Return (CType(Session("SearchCriteria"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("SearchCriteria") = value
        End Set
    End Property

    Public Property SelectedTabId() As String
        Get
            If Not ViewState("SelectedTabId") Is Nothing Then
                Return (CType(ViewState("SelectedTabId"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("SelectedTabId") = value
        End Set
    End Property

    Public Property ShowHideGridHeader() As Boolean
        Get
            If Not ViewState("ShowHideGridHeader") Is Nothing Then
                Return (CType(ViewState("ShowHideGridHeader"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("ShowHideGridHeader") = value
        End Set
    End Property

    Public Property SortData() As GridViewCustomSort
        Get
            If Not ViewState("SortData") Is Nothing Then
                Return (DirectCast(ViewState("SortData"), GridViewCustomSort))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As GridViewCustomSort)
            ViewState("SortData") = value
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
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim accessPermission As Integer 'VC | 2018.11.09 | YRS-AT-4017 -  Declared variable to store page access rights
        Try
            'START : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            accessPermission = CheckFormAuthorizations()
            If accessPermission <> 0 AndAlso accessPermission <> -3 Then
                Session("accessPermission") = Nothing
                'END : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
                If Not IsPostBack Then
                    CheckAccessRights() 'VC | 2018.11.09 | YRS-AT-4017 - Calling method to check read only rights.
                    Me.SelectedTabId = Request.QueryString("id")
                    'Set tab selection based on query string ID (1 - Loans Pending Approval Tab/ 2 - Search Tab)
                    SetTabAttributes(Me.SelectedTabId)
                    'Populate Web loan data based on tab selected
                    LoadLoanPendingApprovalTab()
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACWebLoans_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Sets tab attributes based on Query string Id
    ''' </summary>
    ''' <param name="id">Tab Id</param>
    ''' <remarks></remarks>
    Private Sub SetTabAttributes(ByVal id As String)
        Try
            Select Case id
                'Loan Pending Approval tab
                Case "1"
                    trFilter.Visible = False
                    tdLoansApproval.Attributes.Add("class", "tabSelected")
                    tdSearch.Attributes.Add("class", "tabNotSelected")
                    'Search Tab
                Case "2"
                    trFilter.Visible = True
                    tdLoansApproval.Attributes.Add("class", "tabNotSelected")
                    tdSearch.Attributes.Add("class", "tabSelected")
                    ClearFilterCriteria()
                    If Request.QueryString("From") = "ParticipantsInformation" Then
                        PopulateSearchCriteria()
                    Else
                        ClearFilterCriteria()
                        chkPending.Checked = True
                    End If
                    'Default tab selection if ID not available - Loan Pending Approval tab
                Case Else
                    trFilter.Visible = False
                    tdLoansApproval.Attributes.Add("class", "tabSelected")
                    tdSearch.Attributes.Add("class", "tabNotSelected")
                    ClearFilterCriteria()
            End Select
        Catch
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Populating the data grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLoanPendingApprovalTab()
        Dim listOfWebLoans As DataTable
        Dim webLoan As YMCAObjects.WebLoan
        Dim fullName, startDate, endDate, fundId, ssn, amount, status As String
        Try
            listOfWebLoans = New DataTable()
            'Get only pending loans for approval for tab selected - Loans Pending Approval
            If Me.SelectedTabId = "1" Or String.IsNullOrEmpty(Me.SelectedTabId) Then
                webLoan = GetWebLoan("", "", "", "", "", "", LoanStatus.PEND)
                listOfWebLoans = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetWebLoans(webLoan)
            Else
                'Get Web loans based on filter criteria for tab selected - Search
                startDate = txtStartDate.Text.Trim()
                endDate = txtEndDate.Text.Trim()
                fundId = txtFundId.Text.Trim()
                amount = txtAmount.Text.Trim()
                fullName = txtName.Text.Trim()
                status = FilterSelectedStatus()
                webLoan = GetWebLoan(fullName, startDate, endDate, fundId, ssn, amount, status)
                Me.SearchCriteria = YMCARET.CommonUtilities.Utilities.SerializeToXML(webLoan)
                listOfWebLoans = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetWebLoans(webLoan)
            End If
            'Bind data to grid
            If HelperFunctions.isNonEmpty(listOfWebLoans) Then
                Session("dtLoanPendingApproval") = listOfWebLoans
                btnPrintReport.Enabled = True
                BindData(listOfWebLoans)
            Else
                Me.gvLoanPendingApproval.DataSource = Nothing
                Me.gvLoanPendingApproval.DataBind()
                lblRecords.Text = listOfWebLoans.Rows.Count
                btnPrintReport.Enabled = False
            End If
        Catch
            Throw
        Finally
            fullName = Nothing
            startDate = Nothing
            endDate = Nothing
            fundId = Nothing
            ssn = Nothing
            amount = Nothing
            status = Nothing
            webLoan = Nothing
            listOfWebLoans = Nothing
        End Try
    End Sub

    Private Sub gvLoanPendingApproval_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLoanPendingApproval.RowDataBound
        Dim sort As New GridViewCustomSort
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                If Not Me.SortData Is Nothing Then
                    sort = Me.SortData
                    HelperFunctions.SetSortingArrows(sort, e)
                End If
            ElseIf e.Row.RowType = DataControlRowType.EmptyDataRow Then
                If Me.ShowHideGridHeader Then
                    e.Row.Style.Add("display", "none")
                    Me.ShowHideGridHeader = Nothing
                Else
                    e.Row.Style.Add("display", "normal")
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACWebLoans_gvLoanPendingApproval_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            sort = Nothing
        End Try
    End Sub

    Private Sub gvLoanPendingApproval_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvLoanPendingApproval.Sorting
        Dim listOfWebLoans As DataTable
        Dim webLoanDataView As DataView
        Dim sortExpression As String
        Try
            sortExpression = e.SortExpression
            listOfWebLoans = DirectCast(Session("dtLoanPendingApproval"), DataTable)
            If Not listOfWebLoans Is Nothing Then
                webLoanDataView = listOfWebLoans.DefaultView
                webLoanDataView.Sort = sortExpression
                HelperFunctions.gvSorting(Me.SortData, sortExpression, webLoanDataView)
                HelperFunctions.BindGrid(gvLoanPendingApproval, webLoanDataView, True)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvLoanPendingApproval_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            sortExpression = Nothing
            webLoanDataView = Nothing
            listOfWebLoans = Nothing
        End Try
    End Sub

    Private Sub gvLoanPendingApproval_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLoanPendingApproval.RowCommand
        Dim fundNo As String
        Dim landingURL As String
        Dim loanOriginationId As String
        Try
            If e.CommandName.ToLower = "fundid" Then
                'START : VC | 2018.09.05 | YRS-AT-4018 - Added code to store loan number in session while redirecting
                fundNo = e.CommandSource.Text.ToString().Trim
                loanOriginationId = e.CommandArgument.ToString().Trim
                landingURL = String.Format("{0}?PageType=personmaintenance&DataType=FundNo&Value={1}", ConfigurationManager.AppSettings("YRSLandingPage"), fundNo)
                Session("FlagLoans") = "Loans"
                Session("LoanOriginationId") = loanOriginationId.Replace("{string}", "").Trim()
                If Me.SelectedTabId = "2" Then
                    Session("LACTab") = "Search"
                Else
                    Session("LACTab") = "PendingLoans"
                End If
                Response.Redirect(landingURL, False)
                'END : VC | 2018.09.05 | YRS-AT-4018 - Added  code to store loan number in session while redirecting
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvLoanPendingApproval_ItemCommand", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub btnPrintReport_Click(sender As Object, e As EventArgs) Handles btnPrintReport.Click
        Try
            'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access. 
            ' 'START: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            ' If Me.IsReadOnlyAccess Then
            'HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
            'Exit Sub   
            'End If
            ''END: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            GetWebLoansForPrint()
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
        Catch ex As Exception
            HelperFunctions.LogException("btnPrintReport_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Populating a datatable with datagrid contents for printing
    ''' </summary>
    ''' <returns>Returns a datatable </returns>
    ''' <remarks></remarks>
    Public Function GetWebLoansForPrint() As DataTable
        Dim loansPendingApprovalData As New DataTable("dsWebLoans")
        Dim pendingLoanRow As DataRow

        loansPendingApprovalData.Columns.Add("FundId")
        loansPendingApprovalData.Columns.Add("DateTime")
        loansPendingApprovalData.Columns.Add("ParticipantName")
        loansPendingApprovalData.Columns.Add("LoanAmount")
        loansPendingApprovalData.Columns.Add("Status")
        loansPendingApprovalData.Columns.Add("PaymentMethod")
        loansPendingApprovalData.Columns.Add("ReportName")
        loansPendingApprovalData.Columns.Add("ReportHeader")

        For counter As Integer = 0 To gvLoanPendingApproval.Rows.Count - 1
            pendingLoanRow = loansPendingApprovalData.NewRow
            pendingLoanRow("FundId") = TryCast(gvLoanPendingApproval.Rows(counter).FindControl("lnkFundIdNo"), LinkButton).Text
            pendingLoanRow("DateTime") = gvLoanPendingApproval.Rows(counter).Cells(mConst_gvLoanPendingApprovalIndexOfDateTime).Text
            pendingLoanRow("ParticipantName") = gvLoanPendingApproval.Rows(counter).Cells(mConst_gvLoanPendingApprovalIndexOfParticipantName).Text
            pendingLoanRow("LoanAmount") = gvLoanPendingApproval.Rows(counter).Cells(mConst_gvLoanPendingApprovalIndexOfLoanAmount).Text
            pendingLoanRow("Status") = gvLoanPendingApproval.Rows(counter).Cells(mConst_gvLoanPendingApprovalIndexOfLoanStatus).Text
            pendingLoanRow("PaymentMethod") = gvLoanPendingApproval.Rows(counter).Cells(mConst_gvLoanPendingApprovalIndexOfPaymentMethod).Text
            If Me.SelectedTabId = "2" Then
                pendingLoanRow("ReportName") = "Loans Report" 'MMR | 2018.11.15 | YRS-AT-4017 | Added word "Report" in Report Name
                pendingLoanRow("ReportHeader") = "Loans Report"
            Else
                pendingLoanRow("ReportName") = "Loans Pending Approval Report" 'MMR | 2018.11.15 | YRS-AT-4017 | Added word "Report" in Report Name
                pendingLoanRow("ReportHeader") = "Loans Pending Approval Report"
            End If
            loansPendingApprovalData.Rows.Add(pendingLoanRow)
        Next
        Session("dsWebLoans") = loansPendingApprovalData
        Session("ReportName") = "WebLoanList"

        Return loansPendingApprovalData
    End Function

    Protected Sub btnFind_Click(sender As Object, e As EventArgs)
        Dim checkSearchCriteriaExist As Integer
        Try
            checkSearchCriteriaExist = 0
            If txtStartDate.Text = String.Empty AndAlso txtEndDate.Text = String.Empty AndAlso txtFundId.Text = String.Empty Then

                If txtName.Text = String.Empty AndAlso txtAmount.Text = String.Empty Then

                    If (chkAll.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkCanceled.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkAccepted.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkApproved.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkDeclined.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkExpired.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkPaid.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkPending.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkWithdrawn.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    ElseIf (chkRejected.Checked = True) Then
                        checkSearchCriteriaExist = 1
                    End If
                Else
                    checkSearchCriteriaExist = 1
                End If
            Else
                checkSearchCriteriaExist = 1
            End If

            If checkSearchCriteriaExist = 0 Then
                HelperFunctions.ShowMessageToUser(GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_SEARCH_CRITERIA_MISSING), EnumMessageTypes.Error)
                Me.gvLoanPendingApproval.DataSource = Nothing
                Me.gvLoanPendingApproval.DataBind()
                lblRecords.Text = Me.gvLoanPendingApproval.Rows.Count
                btnPrintReport.Enabled = False
            Else
                LoadLoanPendingApprovalTab()
            End If

        Catch ex As Exception
            HelperFunctions.LogException("btnFind_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        Try
            Me.ShowHideGridHeader = True
            ClearFilterCriteria()
            btnPrintReport.Enabled = False
        Catch ex As Exception
            HelperFunctions.LogException("btnClear_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Method for clearing all search criteria
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearFilterCriteria()
        txtStartDate.Text = ""
        txtEndDate.Text = ""
        txtFundId.Text = ""
        txtAmount.Text = ""
        txtName.Text = ""
        chkAll.Checked = False
        chkCanceled.Checked = False
        chkAccepted.Checked = False
        chkApproved.Checked = False
        chkExpired.Checked = False
        chkPaid.Checked = False
        chkPending.Checked = False
        chkDeclined.Checked = False
        chkWithdrawn.Checked = False
        chkRejected.Checked = False
        If Not Session("dtLoanPendingApproval") Is Nothing Then
            gvLoanPendingApproval.DataSource = DirectCast(Session("dtLoanPendingApproval"), DataTable).Clone()
            gvLoanPendingApproval.DataBind()
        End If
        lblRecords.Text = gvLoanPendingApproval.Rows.Count
    End Sub

    ''' <summary>
    ''' Sets selected search criteria status as coma separated string
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FilterSelectedStatus() As String
        Dim status As String = ""
        If (chkAll.Checked = True) Then
            status += String.Format("{0},", String.Join(",", GetType(LoanStatus).GetFields().Select(Function(t) t.Name).ToArray()))
        Else
            If (chkCanceled.Checked = True) Then
                status += String.Format("{0},", LoanStatus.CANCEL)
            End If
            If (chkAccepted.Checked = True) Then
                status += String.Format("{0},", LoanStatus.ACCEPTED)
            End If
            If (chkApproved.Checked = True) Then
                status += String.Format("{0},", LoanStatus.APPROVED)
            End If
            If (chkDeclined.Checked = True) Then
                status += String.Format("{0},", LoanStatus.DECLINED)
            End If
            If (chkExpired.Checked = True) Then
                status += String.Format("{0},", LoanStatus.EXP)
            End If
            If (chkPaid.Checked = True) Then
                status += String.Format("{0},", LoanStatus.PAID)
            End If
            If (chkPending.Checked = True) Then
                status += String.Format("{0},", LoanStatus.PEND)
            End If
            If (chkWithdrawn.Checked = True) Then
                status += String.Format("{0},", LoanStatus.WITHDRAWN)
            End If
            If (chkRejected.Checked = True) Then
                status += String.Format("{0},", LoanStatus.REJECTED)
            End If
        End If

        If status <> String.Empty Then
            status = status.Trim().Substring(0, status.Length - 1)
        End If
        Return status
    End Function

    Protected Sub btnClose_Click(sender As Object, e As EventArgs)
        Try
            Me.SelectedTabId = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Function GetWebLoan(ByVal fullName As String, ByVal startDate As String, ByVal endDate As String, ByVal fundId As String, ByVal ssn As String, ByVal amount As String, ByVal status As String) As YMCAObjects.WebLoan
        Dim webLoan As YMCAObjects.WebLoan = New YMCAObjects.WebLoan
        webLoan.StartDate = startDate
        webLoan.EndDate = endDate
        webLoan.FundId = fundId
        webLoan.SSN = ssn
        webLoan.Amount = amount
        webLoan.FullName = fullName
        webLoan.Status = status
        Return webLoan
    End Function
    'START : VC | 2018.09.05 | YRS-AT-4018 -  Added method load search criteria if redirected from person maintenance
    ''' <summary>
    ''' Maintains selected search criteria on navigation from person maintenance to search tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateSearchCriteria()
        Dim filterWebLoan As YMCAObjects.WebLoan

        Try
            If Not String.IsNullOrEmpty(Me.SearchCriteria) Then
                Dim serializer As System.Xml.Serialization.XmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(YMCAObjects.WebLoan))
                Dim reader As System.IO.TextReader = New System.IO.StringReader(Me.SearchCriteria)
                filterWebLoan = serializer.Deserialize(reader)
            End If

            If Not filterWebLoan Is Nothing Then
                txtStartDate.Text = filterWebLoan.StartDate
                txtEndDate.Text = filterWebLoan.EndDate
                txtFundId.Text = filterWebLoan.FundId
                txtAmount.Text = filterWebLoan.Amount
                txtName.Text = filterWebLoan.FullName
                Dim status = filterWebLoan.Status.Split(",")
                For i = 0 To UBound(status)
                    If status(i) = LoanStatus.ACCEPTED Then
                        chkAccepted.Checked = True
                    End If
                    If status(i) = LoanStatus.APPROVED Then
                        chkApproved.Checked = True
                    End If
                    If status(i) = LoanStatus.CANCEL Then
                        chkCanceled.Checked = True
                    End If
                    If status(i) = LoanStatus.DECLINED Then
                        chkDeclined.Checked = True
                    End If
                    If status(i) = LoanStatus.EXP Then
                        chkExpired.Checked = True
                    End If
                    If status(i) = LoanStatus.PAID Then
                        chkPaid.Checked = True
                    End If
                    If status(i) = LoanStatus.PEND Then
                        chkPending.Checked = True
                    End If
                    If status(i) = LoanStatus.REJECTED Then
                        chkRejected.Checked = True
                    End If
                    If status(i) = LoanStatus.WITHDRAWN Then
                        chkWithdrawn.Checked = True
                    End If
                Next
                Me.SearchCriteria = Nothing
                filterWebLoan = Nothing
            End If
        Catch
            Throw
        Finally
            filterWebLoan = Nothing
        End Try
    End Sub
    'END : VC | 2018.09.05 | YRS-AT-4018 -  Added method load search criteria if redirected from person maintenance

    ''' <summary>
    ''' Binds data to grid and also maintains its sorting
    ''' </summary>
    ''' <param name="loanTable"></param>
    ''' <remarks></remarks>
    Private Sub BindData(ByVal loanTable As DataTable)
        Dim dv As DataView
        Dim sortData As GridViewCustomSort
        Try
            dv = loanTable.DefaultView
            'START: Calling sorting here to maintain Sort State on every occassion of refresh attempts, like OND check/uncheck
            sortData = Me.SortData
            If (Not sortData Is Nothing) Then
                If sortData.SortDirection.ToUpper() = "ASC" Then
                    sortData.SortDirection = "DESC"
                Else
                    sortData.SortDirection = "ASC"
                End If
                HelperFunctions.gvSorting(sortData, sortData.SortExpression, dv)
            End If
            'END: Calling sorting here to maintain Sort State on every occassion of refresh attempts, like OND check/uncheck
            HelperFunctions.BindGrid(gvLoanPendingApproval, dv, True)
            lblRecords.Text = loanTable.Rows.Count
        Catch
            Throw
        Finally
            sortData = Nothing
            dv = Nothing
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