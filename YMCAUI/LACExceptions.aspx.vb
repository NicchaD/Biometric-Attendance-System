'************************************************************************************************************************
' Author: Manthan Rajguru
' Created on: 08/03/2018
' Summary of Functionality: List all pending loans with reasons.
' Declared in Version: 20.6.0 | YRS-AT-4017 YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' 	Vinayan C				  | 10/10/2018 	 | 20.6.0	       | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 		                  | 	         |		           | 
'   Shilpa N                  | 02/28/2019   | 20.6.3          | YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************


Imports System
Imports System.Configuration
Public Class LACExceptions
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Global Variable Declaration
    ''' </summary>
    ''' <remarks></remarks>
    Private Const mConst_gvLoansForExceptionsIndexOfPersId As Integer = 0
    Private Const mConst_gvLoansForExceptionsIndexOfFundNo As Integer = 1
    Private Const mConst_gvLoansForExceptionsIndexOfName As Integer = 2
    Private Const mConst_gvLoansForExceptionsIndexOfYMCAName As Integer = 3
    Private Const mConst_gvLoansForExceptionsIndexOfSavingsBalance As Integer = 4
    Private Const mConst_gvLoansForExceptionsIndexOfFundStatus As Integer = 5
    Private Const mConst_gvLoansForExceptionsIndexOfLoanStatus As Integer = 6
    Private Const mConst_gvLoansForExceptionsIndexOfRequestDate As Integer = 7
    Private Const mConst_gvLoansForExceptionsIndexOfPaymentMethod As Integer = 8
    Private Const mConst_gvLoansForExceptionsIndexOfRequestedAmount As Integer = 9
    Private Const mConst_gvLoansForExceptionsIndexOfIsONDRequested As Integer = 10
    Private Const mConst_gvLoansForExceptionsIndexOfReason As Integer = 11

    ''' <summary>
    ''' Property Declaration
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
#Region "Property"
    ''' <summary>
    ''' List of loan exceptions 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Property for listing loan exception</remarks>
    Public Property ListOfLoansForExceptions() As DataTable
        Get
            If Not ViewState("ListOfLoansForExceptions") Is Nothing Then
                Return (DirectCast(ViewState("ListOfLoansForExceptions"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataTable)
            ViewState("ListOfLoansForExceptions") = value
        End Set
    End Property
    ''' <summary>
    ''' Filtering Data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Filtering data in grid</remarks>
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
    ''' <summary>
    ''' Sort Data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Sorting data in grid</remarks>
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
    ''' <summary>
    ''' Report Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Property for reportname </remarks>
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
    ''' <summary>
    ''' Tooltip background color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Backgroundcolor() As String
        Get
            If CType(ViewState("Backgroundcolor"), String) = Nothing Then
                Return String.Empty
            Else
                Return (CType(ViewState("Backgroundcolor"), String))
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("Backgroundcolor") = Value
        End Set
    End Property
    Public Property SearchCriteriaExceptions() As String
        Get
            If Not Session("SearchCriteriaExceptions") Is Nothing Then
                Return (CType(Session("SearchCriteriaExceptions"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("SearchCriteriaExceptions") = value
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
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            'START : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
            accessPermission = CheckFormAuthorizations()
            If accessPermission <> 0 AndAlso accessPermission <> -3 Then
                Session("accessPermission") = Nothing
                'END : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
                If Not IsPostBack Then
                    ClearSession()
                    CheckAccessRights() 'VC | 2018.11.09 | YRS-AT-4017 - Calling method to check read only rights.  
                    LoadListOfLoansForExceptions()
                    If Request.QueryString("From") = "ParticipantsInformation" Then
                        If Not Me.SearchCriteriaExceptions Is Nothing Then
                            txtFundNo.Text = Me.SearchCriteriaExceptions
                            Me.SearchCriteriaExceptions = Nothing
                        End If
                        SearchRecords()
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACLoansExceptions_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    ''' <summary>
    ''' Fetch all the pending loans and bind it to the grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadListOfLoansForExceptions()
        Dim LoansExceptionsList As New DataTable
        Try
            ' getting list of loans with exceptions from database
            LoansExceptionsList = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanExceptions()
            If HelperFunctions.isNonEmpty(LoansExceptionsList) Then
                Me.ListOfLoansForExceptions = LoansExceptionsList
                lblRecords.InnerText = LoansExceptionsList.Rows.Count
            Else
                btnPrintExceptions.Enabled = False
                lblNoRecord.InnerText = LoansExceptionsList.Rows.Count
            End If
            gvLoansForExceptions.DataSource = LoansExceptionsList
            gvLoansForExceptions.DataBind()
        Catch
            Throw
        Finally
            LoansExceptionsList = Nothing
        End Try
    End Sub

    Private Sub gvLoansForExceptions_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvLoansForExceptions.RowCommand
        Dim fundNo As String = e.CommandArgument.ToString().Trim
        'Handling page redirection to loan request.
        Session("Seamless_Fund") = e.CommandArgument.ToString.Trim()
        If e.CommandName = "loanrequestandprocessing" Then
            Session("Seamless_From") = "loanrequestandprocessing"
            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=LoanRequestAndProcessing", False)
        End If

        'Handling page redirection to person maintenance loan tab.
        If e.CommandName = "personmaintenanceloan" Then
            Session("FlagLoans") = "Loans"
            Session("LACTab") = "Exceptions"
            Dim urlLanding As String = System.Configuration.ConfigurationManager.AppSettings("YRSLandingPage") + "?PageType=personmaintenance&DataType=FundNo&Value=" + fundNo
            Response.Redirect(urlLanding, False)
        End If
    End Sub

    Private Sub gvLoansForExceptions_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvLoansForExceptions.RowDataBound
        Dim sort As New GridViewCustomSort
        Dim fundNo, reason As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim addToolTipDiv As HtmlContainerControl = Tooltip

        Try
            If e.Row.RowType = DataControlRowType.Header Then
                If Not Me.SortData Is Nothing Then
                    sort = Me.SortData
                    HelperFunctions.SetSortingArrows(sort, e)
                End If
            End If

            ' To show tooltip on click 
            If e.Row.RowType = DataControlRowType.DataRow Then
                fundNo = Convert.ToString(drv("FundNo"))
                reason = Convert.ToString(drv("Reason"))

                If Backgroundcolor Is Nothing Or Backgroundcolor = String.Empty Then
                    Backgroundcolor = Drawing.Color.LightYellow.Name
                End If

                If (reason <> "") Then
                    'OnMouseClick tooltip will show with reason and once mouse out it will hide tool tip.
                    e.Row.Cells(mConst_gvLoansForExceptionsIndexOfReason).Attributes.Add("onclick", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + reason + "');")
                    e.Row.Cells(mConst_gvLoansForExceptionsIndexOfReason).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                Else
                    e.Row.Cells(mConst_gvLoansForExceptionsIndexOfReason).Attributes.Add("onclick", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + "" + "','" + reason + "');")
                    e.Row.Cells(mConst_gvLoansForExceptionsIndexOfReason).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                End If
            End If
            'START: Shilpa N | 03/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                Dim imgbtnloanprocess As ImageButton
                imgbtnloanprocess = e.Row.FindControl("imgProcessing")
                If imgbtnloanprocess IsNot Nothing Then
                    imgbtnloanprocess.Enabled = False
                    imgbtnloanprocess.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                End If
            End If
            'END: Shilpa N | 03/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

        Catch ex As Exception
            HelperFunctions.LogException("gvLoansForExceptions_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            sort = Nothing
        End Try
    End Sub

    Private Sub gvLoansForExceptions_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvLoansForExceptions.Sorting
        Dim dv As New DataView
        Dim loanDataWithExceptions As New DataTable
        Dim sortExpression As String

        Try
            sortExpression = e.SortExpression
            If HelperFunctions.isNonEmpty(Me.ListOfLoansForExceptions) Then
                loanDataWithExceptions = Me.ListOfLoansForExceptions
                dv = loanDataWithExceptions.DefaultView
                dv.Sort = sortExpression
                If Not String.IsNullOrEmpty(Me.FilterData) Then
                    dv.RowFilter = Me.FilterData
                End If
                HelperFunctions.gvSorting(Me.SortData, e.SortExpression, dv)
                HelperFunctions.BindGrid(gvLoansForExceptions, dv, True)
                lblRecords.InnerText = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvLoansForExceptions_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            dv = Nothing
            loanDataWithExceptions = Nothing
            sortExpression = String.Empty
        End Try
    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Try
            SearchRecords()
        Catch ex As Exception
            HelperFunctions.LogException("LACExceptions_btnFind_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    ''' <summary>
    ''' Search Records based on fund no
    ''' </summary>
    ''' <remarks>It will search records based on fund no </remarks>
    Private Sub SearchRecords()
        Dim dvFilter As New DataView
        Dim filter As String = String.Empty
        Dim ListOfLoansWithExceptions As New DataTable
        Dim sort As New GridViewCustomSort

        Try
            'Sort data on basis of entered fund no.
            If HelperFunctions.isNonEmpty(Me.ListOfLoansForExceptions) Then
                ListOfLoansWithExceptions = Me.ListOfLoansForExceptions
                dvFilter = ListOfLoansWithExceptions.DefaultView
                If Not String.IsNullOrEmpty(txtFundNo.Text.Trim) Then
                    filter = String.Format("FundNo = {0}", txtFundNo.Text.Trim)
                    Me.SearchCriteriaExceptions = txtFundNo.Text.Trim
                Else
                    Me.SearchCriteriaExceptions = Nothing
                End If

                If Not String.IsNullOrEmpty(filter) Then
                    dvFilter.RowFilter = filter
                End If
                Me.FilterData = filter

                If Not Me.SortData Is Nothing Then
                    sort = Me.SortData
                    dvFilter.Sort = sort.SortExpression + " " + sort.SortDirection
                End If
                HelperFunctions.BindGrid(gvLoansForExceptions, dvFilter, True)
                lblRecords.InnerText = dvFilter.Count
                If dvFilter.Count = 0 Then
                    btnPrintExceptions.Enabled = False
                Else
                    btnPrintExceptions.Enabled = True
                End If
            End If
        Catch
            Throw
        Finally
            dvFilter = Nothing
            filter = String.Empty
            ListOfLoansWithExceptions = Nothing
            sort = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' Clear all session values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSession()
        Me.SortData = Nothing
        Me.FilterData = String.Empty
        Me.ListOfLoansForExceptions = Nothing
        Me.ReportName = String.Empty
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Try
            txtFundNo.Text = ""
            chkCHECK.Checked = False
            chkEFT.Checked = False
            Me.FilterData = String.Empty
            SearchRecords()
            Me.SearchCriteriaExceptions = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("LACException_btnClear_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSession()
            Me.SearchCriteriaExceptions = Nothing
            Response.Redirect("MainWebform.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("LACExceptions_btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnPrintExceptions_Click(sender As Object, e As EventArgs) Handles btnPrintExceptions.Click
        Try
            'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            ''START: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            ' If Me.IsReadOnlyAccess Then
            ' HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
            ' Exit Sub
            'End If
            ''END: VC | 2018.11.09 | YRS-AT-4017 - If page rights is read only then show error message
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            GetSelectedRecords()
        Catch ex As Exception
            HelperFunctions.LogException("LACException_btnPrintExceptions_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    ''' <summary>
    ''' Function to print record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSelectedRecords()
        Dim printLetters As New DataTable
        Dim printLettersRow As DataRow
        Dim dateText As String
        Dim fundNoLink As New LinkButton
        Dim reasonLabel As New Label
        Dim reason As String ' VC | 10/10/2018 | YRS-AT-4017 | Declared a variable Reason

        Try
            'Adding columns which are available in report.
            printLetters.Columns.Add("FundId")
            printLetters.Columns.Add("Name")
            printLetters.Columns.Add("YMCAName")
            printLetters.Columns.Add("SavingsBalance")
            printLetters.Columns.Add("FundStatus")
            printLetters.Columns.Add("Status")
            printLetters.Columns.Add("RequestDate")
            printLetters.Columns.Add("PaymentMethod")
            printLetters.Columns.Add("RequestedAmt")
            printLetters.Columns.Add("ONDRequested")
            printLetters.Columns.Add("Reason")
            printLetters.Columns.Add("ReportName")

            If HelperFunctions.isNonEmpty(gvLoansForExceptions) Then
                For iCount As Integer = 0 To gvLoansForExceptions.Rows.Count - 1

                    'Assiging text to each column in report.
                    printLettersRow = printLetters.NewRow
                    fundNoLink = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfFundNo).FindControl("FundNo")
                    If Not fundNoLink Is Nothing Then
                        printLettersRow("FundId") = fundNoLink.Text.Trim()
                    End If
                    printLettersRow("Name") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfName).Text
                    printLettersRow("YMCAName") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfYMCAName).Text
                    printLettersRow("SavingsBalance") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfSavingsBalance).Text
                    printLettersRow("FundStatus") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfFundStatus).Text
                    printLettersRow("Status") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfLoanStatus).Text
                    dateText = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfRequestDate).Text
                    If dateText.ToUpper.Trim = "&NBSP;" Then
                        printLettersRow("RequestDate") = ""
                    Else
                        printLettersRow("RequestDate") = dateText
                    End If
                    printLettersRow("PaymentMethod") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfPaymentMethod).Text
                    printLettersRow("RequestedAmt") = gvLoansForExceptions.Rows(iCount).Cells(mConst_gvLoansForExceptionsIndexOfRequestedAmount).Text
                    'For reason column extra reason has been taken to print full reason column
                    reasonLabel = gvLoansForExceptions.Rows(iCount).FindControl("ReportReason")
                    If Not reasonLabel Is Nothing Then
                        'START: VC | 10/10/2018 | YRS-AT-4017 | Commented existing code and added new code to replace </br> element from reason 
                        'START: MMR | 2018.11.15 | YRS-AT-4017 | Removed commented code,Commented existing code to replace <br/> tag with new value as it was not working and added code to show multiple reasons in new line.
                        'reason = reasonLabel.Text.Replace("</br>", ", ").Trim()
                        'reason = reason.TrimEnd(",")
                        reason = reasonLabel.Text.Replace("<br />", Environment.NewLine)
                        'END: MMR | 2018.11.15 | YRS-AT-4017 | Removed commented code,Commented existing code to replace <br/> tag with new value as it was not working and added code to show multiple reasons in new line.
                        printLettersRow("Reason") = reason
                        'END: VC | 10/10/2018 | YRS-AT-4017 | Commented existing code and added new code to replace </br> element from reason 
                    End If
                    Dim chkBoxOND As New CheckBox
                    chkBoxOND = CType(gvLoansForExceptions.Rows(iCount).FindControl("chkONDRequested"), CheckBox)
                    If Not chkBoxOND Is Nothing Then
                        If chkBoxOND.Checked Then
                            printLettersRow("ONDRequested") = "Yes"
                        Else
                            printLettersRow("ONDRequested") = "No"
                        End If
                    End If

                    printLettersRow("ReportName") = "Loan Exceptions Report"
                    printLetters.Rows.Add(printLettersRow)

                Next
                SessionManager.SessionLoanUtility.dtLoanExceptions = printLetters
                Me.ReportName = "LoanExceptions"
                'Calling report 
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)
            End If
        Catch
            Throw
        Finally
            printLetters = Nothing
            printLettersRow = Nothing
            dateText = String.Empty
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