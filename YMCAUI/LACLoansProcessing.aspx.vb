'************************************************************************************************************************
' Author: Manthan Rajguru
' Created on: 07/24/2018
' Summary of Functionality: Helps to process multiple loans from single screen
' Declared in Version: 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
'   Vinayan C                   | 2018.11.06    |  20.6.0       | YRS-AT-4017 -  YRS enh: EFT Loans Project: "new" Loan Admin webpages (approve/decline/process) 
'   Megha Lad                   | 2019.01.09    |  20.6.2       | YRS-AT-4244 - YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462)              
'   Shilpa N                    | 02/28/2019    |               | YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'   Megha Lad                   | 2019.03.06    |  20.6.2       | YRS-AT-4244 - YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 
'   Santosh Bura                | 2019.03.15    |  20.6.2       | YRS-AT-4244 - YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462)              
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************
Imports YMCAObjects
Public Class LACLoansProcessing
    Inherits System.Web.UI.Page

    Private Const mConst_gvLoansForProcessingIndexOfLoanRequestId As Integer = 1
    Private Const mConst_gvLoansForProcessingIndexOfPersId As Integer = 2
    Private Const mConst_gvLoansForProcessingIndexOfFundEventId As Integer = 3
    Private Const mConst_gvLoansForProcessingIndexOfFundNo As Integer = 4
    Private Const mConst_gvLoansForProcessingIndexOfName As Integer = 5
    Private Const mConst_gvLoansForProcessingIndexOfYMCAName As Integer = 6
    Private Const mConst_gvLoansForProcessingIndexOfSavingsBalance As Integer = 7
    Private Const mConst_gvLoansForProcessingIndexOfLoanStatus As Integer = 8
    Private Const mConst_gvLoansForProcessingIndexOfRequestDate As Integer = 9
    Private Const mConst_gvLoansForProcessingIndexOfPaymentMethod As Integer = 10
    Private Const mConst_gvLoansForProcessingIndexOfRequestedAmount As Integer = 11
    Private Const mConst_gvLoansForProcessingIndexOfIsONDRequested As Integer = 12
    Private Const mConst_gvLoansForProcessingIndexOfLoanNumber As Integer = 14
    Private Const mConst_gvLoansForProcessingIndexOfSSN As Integer = 15
    Private Const mConst_gvLoansForProcessingIndexOfFirstName As Integer = 16
    Private Const mConst_gvLoansForProcessingIndexOfMiddleName As Integer = 17
    Private Const mConst_gvLoansForProcessingIndexOfLastName As Integer = 18
    Private Const mConst_gvLoansForProcessingIndexOfYmcaID As Integer = 19
    Private Const mConst_gvLoansForProcessingIndexOfYmcaNo As Integer = 20
    Private Const mConst_gvLoansForProcessingIndexOfONDAmount As Integer = 21


#Region "Property"
    Public Property ListOfLoansForProcessing() As DataTable
        Get
            'START: ML|2019.01.28|YRS-AT-4244 - Maintain Session for Listofloanprocessing changes.
            If Not Session("ListOfLoansForProcessing") Is Nothing Then
                Return (DirectCast(Session("ListOfLoansForProcessing"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataTable)
            Session("ListOfLoansForProcessing") = value
        End Set
    End Property
    'START: ML|2019.01.09|YRS-AT-4244 - Maintain ViewState for OND status changes.
    Public Property ListOfLoansForSaveOND() As DataTable
        Get
            If Not ViewState("ListOfLoansForSaveOND") Is Nothing Then
                Return (DirectCast(ViewState("ListOfLoansForSaveOND"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataTable)
            ViewState("ListOfLoansForSaveOND") = value
        End Set
    End Property
    'END: ML|2019.01.09|YRS-AT-4244 -  Maintain ViewState for OND status changes.

    Public Property FilterData() As String
        Get
            If Not ViewState("FilterData") Is Nothing Then
                Return (CType(ViewState("FilterData"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("FilterData") = value
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

    Public Property ReportName() As String
        Get
            If Not Session("ReportName") Is Nothing Then
                Return (CType(Session("ReportName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("ReportName") = value
        End Set
    End Property

    Public Property AccessPermissionMessage() As String
        Get
            If Not ViewState("AccessPermissionMessage") Is Nothing Then
                Return (CType(ViewState("AccessPermissionMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("AccessPermissionMessage") = value
        End Set
    End Property

    'START : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
    'Public Property AccessPermissionRight() As String
    '    Get
    '        If Not ViewState("AccessPermissionRight") Is Nothing Then
    '            Return (CType(ViewState("AccessPermissionRight"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("AccessPermissionRight") = value
    '    End Set
    'End Property
    'END : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button

    Public Property ValidationMessage() As String
        Get
            If Not ViewState("ValidationMessage") Is Nothing Then
                Return (CType(ViewState("ValidationMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ValidationMessage") = value
        End Set
    End Property

    Public Property LoansSelectedForProcessing() As List(Of YMCAObjects.Loan)
        Get
            If Not Session("LoansSelectedForProcessing") Is Nothing Then
                Return (DirectCast(Session("LoansSelectedForProcessing"), List(Of YMCAObjects.Loan)))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As List(Of YMCAObjects.Loan))
            Session("LoansSelectedForProcessing") = value
        End Set
    End Property

    Public Property LoansProcessingBatchID() As String
        Get
            If Not Session("LoansProcessingBatchID") Is Nothing Then
                Return Convert.ToString(Session("LoansProcessingBatchID"))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("LoansProcessingBatchID") = value
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
        'START : ML | 2019.01.28 | YRS-AT-4244 - Declared variable to maintain Disburse button status
        Dim listofLoanProcessingToDisburse As DataTable
        Dim rows As DataRow
        Dim searchText As String
        Dim count As Integer
        'END :ML | 2019.01.28 | YRS-AT-4244 - Declared variable to maintain Disburse button status
        Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
            'START : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            accessPermission = CheckFormAuthorizations()
            If accessPermission <> 0 AndAlso accessPermission <> -3 Then
                Session("accessPermission") = Nothing
                'END : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
                If Not Page.IsPostBack Then
                    ClearSessionValues()
                    CheckAccessRights() 'VC | 2018.11.09 | YRS-AT-4017 - Calling method to check read only rights.
                    Me.btnDisburse.Attributes.Add("OnClick", "ValidateDisbursement();")
                    LoadListOfLoansForProcessing()
                    'SetAccessRightsDetailsForDisburseButton()'VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
                End If
                'START : ML | 2019.01.28 | YRS-AT-4244 - Handle Disburse button status
                searchText = String.Format("Select = '{0}'", "1")
                listofLoanProcessingToDisburse = (DirectCast(HttpContext.Current.Session("ListOfLoansForProcessing"), DataTable))
                'START : SB | 2019.03.15 | YRS-AT-4244 - Checking the datatable is not empty, and then if not then get the count
                'count = listofLoanProcessingToDisburse.Select(searchText).Count SB TODO START
                If HelperFunctions.isNonEmpty(listofLoanProcessingToDisburse) Then
                    count = listofLoanProcessingToDisburse.Select(searchText).Count
                End If
                'END : SB | 2019.03.15 | YRS-AT-4244 - Checking the datatable is not empty, and then if not then get the count
                If (count > 0) Then
                    btnDisburse.Enabled = True
                Else
                    btnDisburse.Enabled = False
                End If
                'END : ML | 2019.01.28 | YRS-AT-4244 - Handle Disburse button status
            End If
            divConfirmBusinessDayProcessing.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_ADMIN_DONT_PROCESS_ON_LAST_BUSINS_DAY).DisplayText ' VC | 2018.11.22 | YRS-AT-4017 | Adding message to warn user to avoid loan processing on last business day.
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Fetch all the pending loans for processing and bind it to the grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadListOfLoansForProcessing()
        Dim dtListOfLoansForProcessing As New DataTable
        Try
            dtListOfLoansForProcessing = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoansForProcessing()
            If HelperFunctions.isNonEmpty(dtListOfLoansForProcessing) Then
                Me.ListOfLoansForProcessing = dtListOfLoansForProcessing
                BindData(dtListOfLoansForProcessing)
            Else
                'START: MMR | 2018.11.15 | YRS-AT-4017 | Added to bind empty data in gridview to display "No Records found" text
                Me.ListOfLoansForProcessing = Nothing    'SB | 2019.03.28 | YRS-AT-4244 | Added code to set session values to nothing if no loans are present to process
                Me.gvLoansForProcessing.DataSource = Nothing
                Me.gvLoansForProcessing.DataBind()
                lblRecords.InnerText = gvLoansForProcessing.Rows.Count
                'END: MMR | 2018.11.15 | YRS-AT-4017 | Added to bind empty data in gridview to display "No Records found" text
                btnPrintList.Enabled = False
            End If
        Catch
            Throw
        Finally
            dtListOfLoansForProcessing = Nothing
        End Try
    End Sub

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
            HelperFunctions.BindGrid(gvLoansForProcessing, dv, True)
            lblRecords.InnerText = loanTable.Rows.Count
        Catch
            Throw
        Finally
            sortData = Nothing
            dv = Nothing
        End Try
    End Sub

    Private Sub gvLoansForProcessing_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvLoansForProcessing.RowCommand
        Try
            Session("Seamless_Fund") = e.CommandArgument.ToString.Trim()
            If e.CommandName = "loanrequestandprocessing" Then
                Session("Seamless_From") = "loanrequestandprocessing"
                Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=LoanRequestAndProcessing", False)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_gvLoansForProcessing_RowCommand", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvLoansForProcessing_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLoansForProcessing.RowDataBound
        Dim sort As New GridViewCustomSort
        'START : ML | 2019.01.28 | YRS-AT-4244 - Declare Variable to maintain value in header checkbox
        Dim searchText As String
        Dim listofLoanProcessingToDisburse As DataTable
        Dim selectedRows, totalRows As Integer
        Dim chkSelectAll As CheckBox
        'END : ML | 2019.01.28 | YRS-AT-4244 - Declare Variable to maintain value in header checkbox
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                If Not Me.SortData Is Nothing Then
                    sort = Me.SortData
                    HelperFunctions.SetSortingArrows(sort, e)
                End If
                'START : ML | 2019.01.28 | YRS-AT-4244 - Set value of header checkbox based on data is session
                chkSelectAll = TryCast(e.Row.FindControl("chkSelectAll"), CheckBox)
                searchText = String.Format("Select = '{0}'", "1")
                listofLoanProcessingToDisburse = (DirectCast(HttpContext.Current.Session("ListOfLoansForProcessing"), DataTable))
                selectedRows = listofLoanProcessingToDisburse.Select(searchText).Count
                totalRows = listofLoanProcessingToDisburse.Rows.Count
                If selectedRows = totalRows Then
                    chkSelectAll.Checked = True
                Else
                    chkSelectAll.Checked = False
                End If
                'END : ML | 2019.01.28 | YRS-AT-4244 - Set value of header checkbox based on data is session
            End If
            'START: Shilpa N | 03/05/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                Dim imgbtnloanprocess As ImageButton
                imgbtnloanprocess = e.Row.FindControl("imgProcessing")
                If imgbtnloanprocess IsNot Nothing Then
                    imgbtnloanprocess.Enabled = False
                    imgbtnloanprocess.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                End If
            End If
            'END: Shilpa N | 03/05/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_gvLoansForProcessing_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            sort = Nothing
        End Try
    End Sub

    Private Sub gvLoansForProcessing_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvLoansForProcessing.Sorting
        Dim dv As New DataView
        Dim listOfLoansForProcessing As New DataTable
        Dim sortExpression As String
        Try
            sortExpression = e.SortExpression
            listOfLoansForProcessing = Me.ListOfLoansForProcessing
            If HelperFunctions.isNonEmpty(listOfLoansForProcessing) Then
                dv = listOfLoansForProcessing.DefaultView
                dv.Sort = sortExpression
                If Not String.IsNullOrEmpty(Me.FilterData) Then
                    dv.RowFilter = Me.FilterData
                End If
                HelperFunctions.gvSorting(Me.SortData, e.SortExpression, dv)
                HelperFunctions.BindGrid(gvLoansForProcessing, dv, True)
                lblRecords.InnerText = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_gvLoansForProcessing_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            dv = Nothing
            listOfLoansForProcessing = Nothing
            sortExpression = String.Empty
        End Try
    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Dim checkSecurity As String
        Try
            checkSecurity = SecurityCheck.Check_Authorization("btnfindloanprocessing", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
            Else
                SearchRecords()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvLoansForProcessing_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            checkSecurity = Nothing
        End Try
    End Sub

    Private Sub SearchRecords()
        Dim dataViewFilter As New DataView
        Dim filter As String = String.Empty
        Dim listOfLoansForProcessing As DataTable
        Dim sort As GridViewCustomSort
        Try
            If HelperFunctions.isNonEmpty(Me.ListOfLoansForProcessing) Then
                listOfLoansForProcessing = Me.ListOfLoansForProcessing
                listOfLoansForProcessing.DefaultView.RowFilter = filter 'ML | 2019.01.31 | YRS-AT-4244 | Resetting rowfilter to empty due to change in ListOfLoansForProcessing property, changed ViewState to Session.
                dataViewFilter = listOfLoansForProcessing.DefaultView
                'Combination 1 - Fund no selected and All Checkbox(EFT & CHECK) or CHECK or EFT Selceted
                If Not String.IsNullOrEmpty(txtFundNo.Text.Trim) Then
                    filter = String.Format("FundNo = {0}", txtFundNo.Text)
                    If chkCHECK.Checked AndAlso chkEFT.Checked Then
                        filter += String.Format("AND PaymentMethod IN ('{0}','{1}')", chkCHECK.Text.ToUpper.Trim, chkEFT.Text.ToUpper.Trim)
                    ElseIf chkCHECK.Checked Then
                        filter += String.Format("AND PaymentMethod = '{0}'", chkCHECK.Text.ToUpper.Trim)
                    ElseIf chkEFT.Checked Then
                        filter += String.Format("AND PaymentMethod = '{0}'", chkEFT.Text.ToUpper.Trim)
                    End If
                End If

                If String.IsNullOrEmpty(txtFundNo.Text.Trim) Then
                    If chkCHECK.Checked AndAlso chkEFT.Checked Then
                        filter = String.Format("PaymentMethod IN ('{0}','{1}')", chkCHECK.Text.ToUpper.Trim, chkEFT.Text.ToUpper.Trim)
                    ElseIf chkCHECK.Checked Then
                        filter = String.Format("PaymentMethod = '{0}'", chkCHECK.Text.ToUpper.Trim)
                    ElseIf chkEFT.Checked Then
                        filter = String.Format("PaymentMethod = '{0}'", chkEFT.Text.ToUpper.Trim)
                    End If
                End If

                If Not String.IsNullOrEmpty(filter) Then
                    dataViewFilter.RowFilter = filter
                End If
                Me.FilterData = filter

                If Not Me.SortData Is Nothing Then
                    sort = Me.SortData
                    dataViewFilter.Sort = sort.SortExpression + " " + sort.SortDirection
                End If
                HelperFunctions.BindGrid(gvLoansForProcessing, dataViewFilter, True)
                lblRecords.InnerText = dataViewFilter.Count
                If dataViewFilter.Count = 0 Then
                    btnPrintList.Enabled = False
                Else
                    btnPrintList.Enabled = True
                End If
            End If

        Catch
            Throw
        Finally
            dataViewFilter = Nothing
            filter = String.Empty
            listOfLoansForProcessing = Nothing
            sort = Nothing
        End Try
    End Sub

    Private Sub ClearSessionValues()
        Me.SortData = Nothing
        Me.FilterData = String.Empty
        Me.ListOfLoansForProcessing = Nothing
        Me.ReportName = String.Empty
        'Me.AccessPermissionRight = String.Empty'VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
        Me.AccessPermissionMessage = String.Empty
        Me.ValidationMessage = String.Empty
        Me.LoansSelectedForProcessing = Nothing
        Me.LoansProcessingBatchID = Nothing
        'START: ML|2019.01.09|YRS-AT-4244 -Empty Viewstate SaveONDlist and disable SaveOND button
        Me.ListOfLoansForSaveOND = Nothing
        btnSaveOND.Enabled = False
        'END: ML|2019.01.09|YRS-AT-4244 -Empty Viewstate SaveONDlist and disable SaveOND button
        btnDisburse.Enabled = False
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Try
            txtFundNo.Text = ""
            chkCHECK.Checked = False
            chkEFT.Checked = False
            Me.FilterData = String.Empty
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnClear_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessionValues()
            Response.Redirect("MainWebform.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnPrintList_Click(sender As Object, e As EventArgs) Handles btnPrintList.Click
        Try
            'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            ''START: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            ' If Me.IsReadOnlyAccess Then
            'HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
            'Exit Sub
            ' End If
            'END: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            PrepareLoanList(True)
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnPrintList_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnDisburse_Click(sender As Object, e As EventArgs) Handles btnDisburse.Click
        Dim list As List(Of YMCAObjects.Loan)
        Dim batchId As String
        Dim confirmationMessage As String 'VC | 2018.11.22 | YRS-AT-4017 - Declared variable
        Try
            'START: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            End If
            'END: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message

            'START:ML|2019.01.09| YRS-AT-4244- Handle if user try to Disburse with out saving OND changes(if OND  selected)
            If HelperFunctions.isNonEmpty(ListOfLoansForSaveOND) Then
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DISBURSE_WITHOUTSAVEOND).DisplayText, EnumMessageTypes.Error, Nothing)
                Exit Sub
            End If
            'END:ML|2019.01.09| YRS-AT-4244- Handle if user try to Disburse with out saving OND changes(if OND  selected) 

            'If Me.AccessPermissionRight = "true" Then'VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
            PrepareLoanList(False)
            list = Me.LoansSelectedForProcessing
            If Not list Is Nothing AndAlso list.Count > 0 Then
                'START: Prepare batch entry
                batchId = Convert.ToString(YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetNextBatchId(Date.Today).Tables(0).Rows(0)(0))
                Me.LoansProcessingBatchID = batchId
                MaintainBatch(list, batchId)
                'END: Prepare batch entry
                'START: VC | 2018.11.22 | YRS-AT-4017 | Commented existing code and added code to fetch confirmation message from DB and passing it to javascript method
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}','{1}');", ConfirmDialog.ClientID, confirmationMessage), True)
                confirmationMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_ADMIN_LOAN_DISBURSE_CONFIRMATION).DisplayText
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}','{1}');", ConfirmDialog.ClientID, confirmationMessage), True)
                'END: VC | 2018.11.22 | YRS-AT-4017 | Commented existing code and added code to fetch confirmation message from DB and passing it to javascript method
            Else
                'HelperFunctions.ShowMessageToUser(Me.AccessPermissionMessage, EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_NOT_SELECTED_DISBURSEMENT).DisplayText, EnumMessageTypes.Error, Nothing)
            End If
            'START : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
            'Else
            'HelperFunctions.ShowMessageToUser(Me.AccessPermissionMessage, EnumMessageTypes.Error)
            'End If
            'END : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnDisburse_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            batchId = Nothing
            list = Nothing
        End Try
    End Sub

    'START : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button
    'Private Sub SetAccessRightsDetailsForDisburseButton()
    '    Dim checkSecurity As String
    '    Try
    '        Me.AccessPermissionRight = "false"
    '        checkSecurity = SecurityCheck.Check_Authorization("btndisburseloanprocessing", Convert.ToInt32(Session("LoggedUserKey")))
    '        If checkSecurity.Trim().ToUpper() = "TRUE" Then
    '            Me.AccessPermissionRight = "true"
    '        End If
    '        Me.AccessPermissionMessage = checkSecurity
    '    Catch
    '        Throw
    '    Finally
    '        checkSecurity = Nothing
    '    End Try
    'End Sub
    'END : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button

    Private Function GetLoansForPrinting(ByVal gridRow As System.Web.UI.WebControls.GridViewRow, ByVal dataRow As DataRow) As DataRow
        Dim printLetters As New DataTable
        Dim printLettersRow As DataRow
        Dim dateText As String
        Dim loanCheckBox As CheckBox
        Dim ondCheckBox As CheckBox
        Try
            loanCheckBox = CType(gridRow.FindControl("chkSelect"), CheckBox)
            If Not loanCheckBox Is Nothing Then

                If loanCheckBox.Checked Then
                    dataRow("Selected") = "Yes"
                Else
                    dataRow("Selected") = "No"
                End If
                dataRow("FundId") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfFundNo).Text
                dataRow("Name") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfName).Text
                dataRow("YMCAName") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfYMCAName).Text
                dataRow("SavingsBalance") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfSavingsBalance).Text
                dataRow("LoanStatus") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfLoanStatus).Text
                dateText = gridRow.Cells(mConst_gvLoansForProcessingIndexOfRequestDate).Text
                If dateText.ToUpper.Trim = "&NBSP;" Then
                    dataRow("RequestDate") = ""
                Else
                    dataRow("RequestDate") = dateText
                End If
                dataRow("PaymentMethod") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfPaymentMethod).Text
                dataRow("RequestedAmt") = gridRow.Cells(mConst_gvLoansForProcessingIndexOfRequestedAmount).Text

                ondCheckBox = CType(gridRow.FindControl("chkONDRequested"), CheckBox)
                If Not ondCheckBox Is Nothing Then
                    If ondCheckBox.Checked Then
                        dataRow("ONDRequested") = "Yes"
                    Else
                        dataRow("ONDRequested") = "No"
                    End If
                End If
                dataRow("ReportName") = "Loan Processing Report"
            End If
            Return dataRow
        Catch
            Throw
        Finally
            loanCheckBox = Nothing
            ondCheckBox = Nothing
            printLetters = Nothing
            printLettersRow = Nothing
            dateText = Nothing
        End Try
    End Function

    Private Function GetSelectedLoansForProcessing(ByVal gridRow As System.Web.UI.WebControls.GridViewRow, ByVal deductions As DataSet) As YMCAObjects.Loan
        Dim loan As YMCAObjects.Loan
        Dim isLoanSelectedForProcessing As Boolean
        Dim loanCheckBox As CheckBox
        Dim ondCheckBox As CheckBox
        Dim localCopyOfDeductions As DataSet
        Try
            isLoanSelectedForProcessing = False
            loanCheckBox = CType(gridRow.FindControl("chkSelect"), CheckBox)
            If Not loanCheckBox Is Nothing AndAlso loanCheckBox.Checked Then
                isLoanSelectedForProcessing = True
            End If

            If isLoanSelectedForProcessing Then
                loan = New YMCAObjects.Loan()
                loan.LoanNumber = Convert.ToInt32(gridRow.Cells(mConst_gvLoansForProcessingIndexOfLoanNumber).Text)
                loan.SSN = gridRow.Cells(mConst_gvLoansForProcessingIndexOfSSN).Text
                loan.PersId = gridRow.Cells(mConst_gvLoansForProcessingIndexOfPersId).Text
                loan.FundId = gridRow.Cells(mConst_gvLoansForProcessingIndexOfFundEventId).Text
                loan.FundNo = Convert.ToInt32(gridRow.Cells(mConst_gvLoansForProcessingIndexOfFundNo).Text)
                loan.FirstName = gridRow.Cells(mConst_gvLoansForProcessingIndexOfFirstName).Text
                loan.MiddleName = gridRow.Cells(mConst_gvLoansForProcessingIndexOfMiddleName).Text
                loan.LastName = gridRow.Cells(mConst_gvLoansForProcessingIndexOfLastName).Text
                loan.YMCAId = gridRow.Cells(mConst_gvLoansForProcessingIndexOfYmcaID).Text
                loan.YMCANo = gridRow.Cells(mConst_gvLoansForProcessingIndexOfYmcaNo).Text
                loan.Amount = Convert.ToDouble(gridRow.Cells(mConst_gvLoansForProcessingIndexOfRequestedAmount).Text)
                loan.RequestNo = Convert.ToInt32(gridRow.Cells(mConst_gvLoansForProcessingIndexOfLoanRequestId).Text)
                loan.PaymentMethodCode = gridRow.Cells(mConst_gvLoansForProcessingIndexOfPaymentMethod).Text
                loan.Status = gridRow.Cells(mConst_gvLoansForProcessingIndexOfLoanStatus).Text

                localCopyOfDeductions = deductions.Copy()
                ondCheckBox = CType(gridRow.FindControl("chkONDRequested"), CheckBox)
                If Not ondCheckBox Is Nothing AndAlso ondCheckBox.Checked Then
                    loan.ONDAmount = Convert.ToDouble(gridRow.Cells(mConst_gvLoansForProcessingIndexOfONDAmount).Text)
                Else
                    localCopyOfDeductions.Tables(0).Rows.Clear()
                    localCopyOfDeductions.AcceptChanges()
                End If
                loan.DeductionTable = localCopyOfDeductions
                Return loan
            Else
                Return Nothing
            End If
        Catch
            Throw
        Finally
            loanCheckBox = Nothing
            ondCheckBox = Nothing
            loan = Nothing
        End Try
    End Function

    Private Sub PrepareLoanList(ByVal isForPrinting As Boolean)
        Dim printLetters As New DataTable
        Dim listOfSelectedLoans As List(Of YMCAObjects.Loan)
        Dim singleLoan As YMCAObjects.Loan
        Dim deductions As DataSet
        Try
            If HelperFunctions.isNonEmpty(gvLoansForProcessing) Then
                If (isForPrinting) Then
                    printLetters = CreateTableForReports()
                    For iCount As Integer = 0 To gvLoansForProcessing.Rows.Count - 1
                        printLetters.Rows.Add(GetLoansForPrinting(gvLoansForProcessing.Rows(iCount), printLetters.NewRow))
                    Next

                    SessionManager.SessionLoanAdmin.LoanProcessingList = printLetters
                    Me.ReportName = "LoanProcessing"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
                Else
                    deductions = GetONDFeeDataSet()

                    listOfSelectedLoans = New List(Of YMCAObjects.Loan)()
                    For iCount As Integer = 0 To gvLoansForProcessing.Rows.Count - 1
                        singleLoan = GetSelectedLoansForProcessing(gvLoansForProcessing.Rows(iCount), deductions)
                        If Not singleLoan Is Nothing Then
                            listOfSelectedLoans.Add(singleLoan)
                        End If
                    Next

                    If listOfSelectedLoans.Count > 0 Then
                        Me.LoansSelectedForProcessing = listOfSelectedLoans
                    End If
                End If
            End If
        Catch
            Throw
        Finally
            deductions = Nothing
            printLetters = Nothing
            singleLoan = Nothing
            listOfSelectedLoans = Nothing
        End Try
    End Sub

    Private Function CreateTableForReports() As DataTable
        Dim printLetters As New DataTable

        printLetters.Columns.Add("Selected")
        printLetters.Columns.Add("FundId")
        printLetters.Columns.Add("Name")
        printLetters.Columns.Add("YMCAName")
        printLetters.Columns.Add("SavingsBalance")
        printLetters.Columns.Add("LoanStatus")
        printLetters.Columns.Add("RequestDate")
        printLetters.Columns.Add("PaymentMethod")
        printLetters.Columns.Add("RequestedAmt")
        printLetters.Columns.Add("ONDRequested")
        printLetters.Columns.Add("ReportName")

        Return printLetters
    End Function

    Private Function GetONDFeeDataSet() As DataSet
        Dim feeTable As DataTable
        Dim deductions As DataSet
        Try
            feeTable = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetDeductions
            For Each row As DataRow In feeTable.Rows
                If (Convert.ToString(row("CodeValue")).Trim() <> "OND") Then
                    row.Delete()
                End If
            Next
            feeTable.AcceptChanges()
            feeTable.TableName = "Deductions"

            deductions = New DataSet()
            deductions.Tables.Add(feeTable.Copy())

            Return deductions
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing.GetONDFeeTable", ex)
            Return Nothing
        Finally
            feeTable = Nothing
            deductions = Nothing
        End Try
    End Function

    <System.Web.Services.WebMethod(True)> _
    Public Shared Function GetListOfLoans() As List(Of Integer)
        Dim list As List(Of YMCAObjects.Loan)
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "GetListOfLoans START")
            list = (DirectCast(System.Web.HttpContext.Current.Session("LoansSelectedForProcessing"), List(Of YMCAObjects.Loan)))
            Return (From loan In list Select loan.RequestNo).ToList()
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_GetListOfLoans", ex)
            Return Nothing
        Finally
            list = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "GetListOfLoans END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    <System.Web.Services.WebMethod(True)> _
    Public Shared Sub ProcessLoan(ByVal requestID As Integer)
        Dim list As List(Of YMCAObjects.Loan)
        Dim loan As YMCAObjects.Loan
        Dim client As Loans
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "ProcessLoan START")
            list = (DirectCast(System.Web.HttpContext.Current.Session("LoansSelectedForProcessing"), List(Of YMCAObjects.Loan)))
            If Not list Is Nothing AndAlso list.Count > 0 Then
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", String.Format("Processing loan request id: {0}", requestID.ToString()))
                loan = list.Find(Function(p) p.RequestNo = requestID)
                If Not loan Is Nothing Then
                    client = New Loans()
                    loan = client.Process(loan)
                    MaintainBatch(list, Convert.ToString(System.Web.HttpContext.Current.Session("LoansProcessingBatchID")))
                End If
            End If
            System.Web.HttpContext.Current.Session("LoansSelectedForProcessing") = list ' This will make sure updated loan with PDF details and error object goes back into session
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_ProcessLoan", ex)
        Finally
            client = Nothing
            loan = Nothing
            list = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "ProcessLoan END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    <System.Web.Services.WebMethod(True)> _
    Public Shared Function GetSummaryReport() As Dictionary(Of String, String)
        Dim list As List(Of YMCAObjects.Loan)
        Dim result As List(Of YMCAObjects.Loan)
        Dim errorList As List(Of YMCAObjects.Loan)
        Dim combinedErrorList As List(Of YMCAObjects.Loan)
        Dim details As Dictionary(Of String, String)
        Dim errorCount As Integer
        Dim batchID As String
        Dim errorFilePath As String

        Dim client As Loans
        Dim participantLetter As String
        Dim ymcaLetter As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "GetSummaryReport START")

            batchID = Convert.ToString(System.Web.HttpContext.Current.Session("LoansProcessingBatchID"))

            details = New Dictionary(Of String, String)()
            combinedErrorList = New List(Of YMCAObjects.Loan)()

            list = (DirectCast(System.Web.HttpContext.Current.Session("LoansSelectedForProcessing"), List(Of YMCAObjects.Loan)))
            If Not list Is Nothing AndAlso list.Count > 0 Then
                result = list.FindAll(Function(p) p.PaymentMethodCode = PaymentMethod.EFT)
                If Not result Is Nothing AndAlso result.Count > 0 Then
                    errorCount = 0
                    errorList = result.FindAll(Function(p) p.Error <> String.Empty)
                    If Not errorList Is Nothing AndAlso errorList.Count > 0 Then
                        errorCount = errorList.Count
                        combinedErrorList.AddRange(errorList)
                    End If

                    details.Add("EFTProcessed", (result.Count - errorCount).ToString())
                    details.Add("EFTFailed", errorCount.ToString())
                    details.Add("EFTTotal", result.Count.ToString())
                End If

                result = list.FindAll(Function(p) p.PaymentMethodCode = PaymentMethod.CHECK)
                If Not result Is Nothing AndAlso result.Count > 0 Then
                    errorCount = 0
                    errorList = result.FindAll(Function(p) p.Error <> String.Empty)
                    If Not errorList Is Nothing AndAlso errorList.Count > 0 Then
                        errorCount = errorList.Count
                        combinedErrorList.AddRange(errorList)
                    End If

                    details.Add("CHECKProcessed", (result.Count - errorCount).ToString())
                    details.Add("CHECKFailed", errorCount.ToString())
                    details.Add("CHECKTotal", result.Count.ToString())

                    ' Consolidate PDF's
                    client = New Loans
                    client.ConsolidateAllLoanPDF(result, batchID)
                    If Not client.ParticipantLetter Is Nothing Then
                        participantLetter = client.ParticipantLetter.URL
                    End If
                    If Not client.YMCALetter Is Nothing Then
                        ymcaLetter = client.YMCALetter.URL
                    End If

                    If Not String.IsNullOrEmpty(participantLetter) Then
                        details.Add("ParticipantLetter", participantLetter)
                    End If
                    If Not String.IsNullOrEmpty(ymcaLetter) Then
                        details.Add("YmcaLetter", ymcaLetter)
                    End If
                End If
            End If

            ' Prepare error file for downloading, it will be created at temp location defined at cnfig key : MergePDFPath, and file name will be Error_BatchID.txt
            If Not combinedErrorList Is Nothing And combinedErrorList.Count > 0 Then
                errorFilePath = PrepareErrorFile(combinedErrorList, batchID)
                If Not String.IsNullOrEmpty(errorFilePath) Then
                    details.Add("ErrorFilePath", errorFilePath)
                End If
            End If

            Return details
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_GetSummaryReport", ex)
            Return Nothing
        Finally
            batchID = Nothing
            errorFilePath = Nothing
            details = Nothing
            errorList = Nothing
            combinedErrorList = Nothing
            result = Nothing
            list = Nothing

            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "GetSummaryReport END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Shared Function PrepareErrorFile(ByVal errorList As List(Of YMCAObjects.Loan), ByVal batchID As String) As String
        Dim tx As System.IO.TextWriter
        Dim folderPath As String
        Dim errorFilePath As String
        Dim relativePath As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "PrepareErrorFile START")
            errorFilePath = String.Empty
            If Not errorList Is Nothing AndAlso errorList.Count > 0 Then
                folderPath = HttpContext.Current.Server.MapPath(String.Format("~\{0}", System.Configuration.ConfigurationSettings.AppSettings("MergedBatchDocumentPath")))
                If FileIO.FileSystem.DirectoryExists(folderPath) Then
                    errorFilePath = String.Format("{0}\Error_{1}.txt", folderPath, batchID)
                    relativePath = String.Format("{0}\\Error_{1}.txt", System.Configuration.ConfigurationManager.AppSettings("MergedBatchDocumentPath"), batchID)

                    tx = New System.IO.StreamWriter(errorFilePath)
                    For Each loan As YMCAObjects.Loan In errorList
                        tx.WriteLine(String.Format("Loan Number: {0}, Loan Request ID: {1}, Error: {2}", loan.LoanNumber, loan.RequestNo, loan.Error))
                    Next
                    tx.Close()
                    tx.Dispose()
                End If
            End If
            Return relativePath
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_PrepareErrorFile", ex)
            Return Nothing
        Finally
            errorFilePath = Nothing
            folderPath = Nothing
            tx = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "PrepareErrorFile END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Shared Sub MaintainBatch(ByVal list As List(Of YMCAObjects.Loan), ByVal batchID As String)
        Dim moduleName As String
        Dim xmlData As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "MaintainBatch START")
            moduleName = YMCAObjects.Module.LACLoanProcessing
            xmlData = HelperFunctions.ConvertToXML(list)
            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(batchID, moduleName, xmlData)
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_MaintainBatch", ex)
        Finally
            moduleName = Nothing
            xmlData = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "MaintainBatch END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Protected Sub btnCloseSummary_Click(sender As Object, e As EventArgs)
        Try
            LoadListOfLoansForProcessing()
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnCloseSummary_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub btnUncheckONDDialogBoxYes_Click(sender As Object, e As EventArgs)
        Dim requestID As String
        Try
            requestID = hdnSelectedLoanLoanRequestID.Value
            ChangeONDSelection(requestID, False)
            hdnSelectedLoanLoanRequestID.Value = "0"
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnCheckONDDialogBoxYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            requestID = Nothing
        End Try
    End Sub

    Protected Sub btnCheckONDDialogBoxYes_Click(sender As Object, e As EventArgs)
        Dim requestID As String
        Try
            requestID = hdnSelectedLoanLoanRequestID.Value
            ChangeONDSelection(requestID, True)
            hdnSelectedLoanLoanRequestID.Value = "0"
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_btnCheckONDDialogBoxYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            requestID = Nothing
        End Try
    End Sub

    Protected Sub ChangeONDSelection(ByVal requestID As String, ByVal isChecked As Boolean)
        Dim listOfLoansForProcessing As DataTable
        Dim rows As DataRow()
        'START: ML | 2019.01.09 | YRS-AT-4244 |Varibale declaration for OND save changes   

        Dim listOfLoansForSaveOND As DataTable
        Dim Ondrows As DataRow()
        Dim searchText As String
        'END: ML | 2019.01.09 | YRS-AT-4244 |Varibale declaration for OND save changes   
        Try

            listOfLoansForProcessing = Me.ListOfLoansForProcessing

            'START: ML | 2019.01.09 | YRS-AT-4244 | Fill datatable from viewstate,if Empty create new datatable        
            listOfLoansForSaveOND = Me.ListOfLoansForSaveOND
            If HelperFunctions.isEmpty(listOfLoansForSaveOND) Then
                listOfLoansForSaveOND = New DataTable
                listOfLoansForSaveOND.Columns.Add("Rowno", GetType(Integer))
                listOfLoansForSaveOND.Columns.Add("LoanRequestID", GetType(String))
                listOfLoansForSaveOND.Columns.Add("IsONDRequested", GetType(String))
            End If
            'END: ML | 2019.01.09 | YRS-AT-4244 | Fill datatable from viewstate,if Empty create new datatable    

            If HelperFunctions.isNonEmpty(listOfLoansForProcessing) Then
                rows = listOfLoansForProcessing.Select(String.Format("LoanRequestId={0}", requestID))
                If HelperFunctions.isNonEmpty(rows) AndAlso rows.Length > 0 Then
                    rows(0)("IsONDRequested") = isChecked
                    listOfLoansForProcessing.AcceptChanges()
                    Me.ListOfLoansForProcessing = listOfLoansForProcessing

                    'START: ML | 2019.01.09 | YRS-AT-4244 | If user had select same OND checkbox it will delete in listOfLoansForSaveOND Datatable else add new record
                    If HelperFunctions.isNonEmpty(listOfLoansForSaveOND) Then
                        Ondrows = listOfLoansForSaveOND.Select(String.Format("LoanRequestId={0}", requestID))
                        If HelperFunctions.isNonEmpty(Ondrows) AndAlso Ondrows.Length > 0 Then
                            '  Ondrows(0)("IsONDRequested") = isChecked   ML | 2019.03.06 | YRS-AT-4244 | Delete unchanged OND records. 
                            Ondrows(0).Delete()
                        Else
                            listOfLoansForSaveOND.Rows.Add(listOfLoansForSaveOND.Rows.Count + 1, rows(0)("LoanRequestId").ToString(), rows(0)("IsONDRequested").ToString())
                        End If
                    Else
                        listOfLoansForSaveOND.Rows.Add(listOfLoansForSaveOND.Rows.Count + 1, rows(0)("LoanRequestId").ToString(), rows(0)("IsONDRequested").ToString())
                    End If
                    listOfLoansForSaveOND.AcceptChanges()
                    Me.ListOfLoansForSaveOND = listOfLoansForSaveOND
                    ''END: ML | 2019.01.09 | YRS-AT-4244 | If user had select same OND checkbox it will update in listOfLoansForSaveOND Datatable else add new record
                    BindData(listOfLoansForProcessing)
                End If

                'START : ML | 2019.03.06 | YRS-AT-4244 | Set SAVEOND button status based on records counts and clear dirty flag. 
                If (listOfLoansForSaveOND.Rows.Count > 0) Then
                    btnSaveOND.Enabled = True
                Else
                    btnSaveOND.Enabled = False
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ClearDirty();", True)  ' ML | 2019.03.08 | YRS-AT-4244 | Clear Dirty flag Value
                End If
                'END : : ML | 2019.03.06 | YRS-AT-4244 | Set SAVEOND button status based on records counts and clear dirty flag.

            End If



        Catch ex As Exception
            HelperFunctions.LogException("LACLoansProcessing_ChangeONDSelection", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            rows = Nothing
            listOfLoansForProcessing = Nothing
            'START: ML | 2019.01.09 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462)              
            Ondrows = Nothing
            listOfLoansForSaveOND = Nothing
            'END: ML | 2019.01.09 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462)              
        End Try
    End Sub

    'START: ML | 2019.01.09 | YRS-AT-4244 | OND selection changes will update in database           
    Private Sub btnSaveOND_Click(sender As Object, e As EventArgs) Handles btnSaveOND.Click

        Dim xmlData As String
        Dim listOfLoansForSaveOND As DataTable
        listOfLoansForSaveOND = Me.ListOfLoansForSaveOND

        Try
            'In Readonly mode user can not change OND status.
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "SAVEOND START")

            If HelperFunctions.isNonEmpty(listOfLoansForSaveOND) Then
                'listOfLoansForSaveOND datatable convert into xmlData
                xmlData = HelperFunctions.ConvertDataTableToXML(listOfLoansForSaveOND)

                'if xmlData Update into database successfully 
                If (YMCARET.YmcaBusinessObject.LoanInformationBOClass.UpdateONDStatus(xmlData)) Then
                    Dim dic As New Dictionary(Of String, String)
                    dic.Add("Rows", Me.ListOfLoansForSaveOND.Rows.Count.ToString)
                    Me.ListOfLoansForSaveOND = Nothing
                    btnSaveOND.Enabled = False

                    'After sucessfull update, acknowledgment message will be shown to user at top of screen.
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_OND_UPDATE, dic).DisplayText, EnumMessageTypes.Success, Nothing)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ClearDirty();", True)
                End If
            End If
        Catch Ex As Exception
            HelperFunctions.LogException("btnSaveOND_Click", Ex)
            HelperFunctions.ShowMessageToUser(Ex.Message, EnumMessageTypes.Error)
        Finally
            xmlData = Nothing
            listOfLoansForSaveOND = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("LACLoansProcessing", "SAVEOND END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    'END: ML | 2019.01.09 |YRS-AT-4244 | OND selection changes will update in database               

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

    'START: ML|2019.01.28|YRS-AT-4244 - Update checkbox value in session datatable to handle in postback mode

    <System.Web.Services.WebMethod(True)> _
    Public Shared Function CheckUncheckLoanProcessing(ByVal checkedStatusFalse As String, ByVal checkedStatusTrue As String) As String
        Dim selectedTrue As String()
        Dim selectedFalse As String()
        Dim searchText As String
        Dim listofLoanProcessingToDisburse As DataTable
        Dim rows As DataRow()

        Try
            listofLoanProcessingToDisburse = (DirectCast(HttpContext.Current.Session("ListOfLoansForProcessing"), DataTable))

            'Spliting coma separated checkbox status into string array
            selectedTrue = checkedStatusTrue.Split(New Char() {","c})
            'Updating data tables's Selected column value based on splited checkbox statues
            For Each element As String In selectedTrue
                If Not String.IsNullOrEmpty(element.ToString) Then
                    searchText = String.Format("LoanRequestId = '{0}'", element.ToString)
                    rows = listofLoanProcessingToDisburse.Select(searchText)
                    If (rows.Length > 0) Then
                        rows(0)("Select") = 1
                    End If
                End If
            Next
            listofLoanProcessingToDisburse.AcceptChanges()
            'Spliting coma separated checkbox status into string array
            selectedFalse = checkedStatusFalse.Split(New Char() {","c})
            'Updating data tables's Selected column value based on splited checkbox statues

            For Each element As String In selectedFalse
                If Not String.IsNullOrEmpty(element.ToString) Then
                    searchText = String.Format("LoanRequestId = '{0}'", element.ToString)
                    rows = listofLoanProcessingToDisburse.Select(searchText)
                    If (rows.Length > 0) Then
                        rows(0)("Select") = 0
                    End If
                End If
            Next

            listofLoanProcessingToDisburse.AcceptChanges()
            HttpContext.Current.Session("ListOfLoansForProcessing") = listofLoanProcessingToDisburse
        Catch ex As Exception
            Throw ex
        Finally
            selectedTrue = Nothing
            selectedFalse = Nothing
            searchText = Nothing
            listofLoanProcessingToDisburse = Nothing
            rows = Nothing
        End Try

    End Function
    'START: ML|2019.01.28|YRS-AT-4244 - Update checkbox value in session datatable to handle in postback mode


End Class