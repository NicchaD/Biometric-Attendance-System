'************************************************************************************************************************
' Author: Santosh Bura 
' Created on: 07/24/2018
' Summary of Functionality: Loan statistics tab for actual count of loan statuses.
' Declared in Version: 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' 			                  | 	         |		           | 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Public Class LACStatistics
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim accessPermission As Integer 'VC | 2018.11.09 | YRS-AT-4017 -  Declared variable to store page access rights
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'START : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
            accessPermission = CheckFormAuthorizations()
            If accessPermission <> 0 AndAlso accessPermission <> -3 Then
                Session("accessPermission") = Nothing
                'END : VC | 2018.11.09 | YRS-AT-4017 -  Code to check page authorization
                If Not Me.IsPostBack Then
                    LoadStatisticsData()
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("LACStatistics_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("LACStatistics_btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    ''' <summary>
    ''' Method displays count of all the loan statuses 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStatisticsData()
        Dim loanStatistics As DataSet
        Dim status As String
        Dim count As String
        Dim total As Integer = 0
        Try
            'get all statistics information 
            loanStatistics = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanStatistics()
            If HelperFunctions.isNonEmpty(loanStatistics) Then
                For Each statisticsRow As DataRow In loanStatistics.Tables("LoanStatistics").Rows
                    status = statisticsRow("status")
                    count = Convert.ToString(statisticsRow("CountOfStatus"))
                    Select Case status.ToUpper()
                        Case LoanStatus.PAID
                            Me.lblPaid.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.ACCEPTED
                            Me.lblAccepted.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.DECLINED
                            Me.lblDeclined.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.CANCEL
                            Me.lblCanceled.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.EXP
                            Me.lblExpired.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.WITHDRAWN
                            Me.lblWithdrawn.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.PEND
                            Me.lblPending.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.APPROVED
                            Me.lblApproved.Text = count
                            total = total + Convert.ToInt32(count)
                        Case LoanStatus.REJECTED
                            Me.lblRejected.Text = count
                            total = total + Convert.ToInt32(count)
                        Case Else
                    End Select
                Next
                Me.lblTotal.Text = total.ToString()
            End If
        Catch
            Throw
        Finally
            loanStatistics = Nothing
            status = Nothing
            count = Nothing
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
End Class