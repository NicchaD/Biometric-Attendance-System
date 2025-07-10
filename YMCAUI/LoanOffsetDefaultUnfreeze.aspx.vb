'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	LoanOffsetDefaultUnfreeze.aspx.vb
' Author Name		:	Dinesh kanojia
' Contact No		:	
' Creation Date		:	04/25/2014
' Description		:	This form is used to list all loan which are eligible to marked as Offset / Default and for Ageging 
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Sanjay S           2015.09.18      YRS-AT-2441 -  Modifications for 403b Loans 
'Anudeep A          2015.09.22      YRS-AT-2609 -  YRS enh: Loans CSS colors and Reporting functionality
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Anudeep A          2015.10.12      YRS-AT-2662 - YRS enh: follow up to Loans release - nightly sql server job for Offset and Unfreeze processing
'Anudeep A          2016.01.22      YRS-AT-2688 - YRS enh-Add Default Date column in Loan Utility (TrackIT 24420)
'Bala               2016.02.25      YRS-AT-2658 - RMD-Print letters screen error."String was not recognized as a valid Boolean."
'Bala               2016.04.19      YRS-AT-2862 - YRS enhancement-Loan Utility enhancements: freeze headings, ignore unfunded payments for default
'Anudeep A          2016.04.18      YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
'Anudeep A          2016.05.28      YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Manthan Rajguru    2016.07.21      YRS-AT-3050 -  YRS bug: typo on 'participants' for automate 'Close' of fully paid Loans (TrackIT 25242)
'Santosh Bura       2018.04.03      YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
'Vinayan C          2018.10.11      YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab 
'Manthan Rajguru    2019.02.12      YRS-AT-4330 - YRS bug: TD Loans: Loan Utility - AutoClosed Loans issues (TrackIT 37178) 
'Shilpa N           2019.03.08      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'********************************************************************************************************************************

Imports YMCARET.YmcaBusinessObject
Imports YMCAUI.SessionManager.SessionLoanUtility

Public Class LoanOffsetDefaultUnfreeze
    Inherits System.Web.UI.Page
    'Start: Bala: 25.02.2016: YRS-AT-2568: Creating properties for this class
#Region "Property"
    Public Property SelectedTab() As String
        Get
            Return ViewState("LoanUtilitySelectedTab")
        End Get
        Set(ByVal value As String)
            ViewState("LoanUtilitySelectedTab") = value
        End Set
    End Property
#End Region
    'End: Bala: 25.02.2016: YRS-AT-2568: Creating properties for this class

    'START: SB | 2018.04.03 | YRS-AT-3101 | Declaring Integer variables to hold cell index values of datagrid 
#Region "Column Indexes"
    ' Indexes defined for "PAID" loan table 
    Dim mConst_DataGridLoanPaidIndexOfAge As Integer = 7
    Dim mConst_DataGridLoanPaidIndexOfDaysOverDue As Integer = 9
    Dim mConst_DataGridLoanPaidIndexOfMessageNo As Integer = 11
    Dim mConst_DataGridLoanPaidIndexOfIsOffsetEligible As Integer = 13

    ' Indexes defined for "DEFALT" loan table
    Dim mConst_DataGridLoanDefaltIndexOfAge As Integer = 7
    Dim mConst_DataGridLoanDefaltIndexOfIsTerminated As Integer = 11
    Dim mConst_DataGridLoanDefaltIndexOfMessageNo As Integer = 13

    ' Indexes defined for "OFFSET" loan table
    Dim mConst_DataGridLoanOffsetIndexOfPersID As Integer = 1
    Dim mConst_DataGridLoanOffsetIndexOfLoanRequestID As Integer = 2
    Dim mConst_DataGridLoanOffsetIndexOfMessageNo As Integer = 9
    Dim mConst_DataGridLoanOffsetIndexOfDefaultDate As Integer = 10
    Dim mConst_DataGridLoanOffsetIndexOfPrintLettersID As Integer = 12

    ' Indexes defined for "Auto Default" loan table
    Dim mConst_DataGridLoanAutoDefaltIndexOfPersID As Integer = 1
    Dim mConst_DataGridLoanAutoDefaltIndexOfRequestID As Integer = 2
    Dim mConst_DataGridLoanAutoDefaltIndexOfDefaultDate As Integer = 5
    Dim mConst_DataGridLoanAutoDefaltIndexOfDPrinterLettersID As Integer = 8

    ' Indexes defined for "Unfreeze Phantom Interest" loan table
    Dim mConst_DataGridLoanUnfreezePhantomInterestIndexOfAge As Integer = 7
    Dim mConst_DataGridLoanUnfreezePhantomInterestIndexOfFrozenDays As Integer = 10

    ' START: MMR | 2019.02.12 |YRS-AT-4330 | Indexes defined for "Auto closed" loan table
    Dim mConst_GridViewAutoClosedLoansIndexOfFundNo As Integer = 0
    Dim mConst_GridViewAutoClosedLoansIndexOfPersId As Integer = 1
    Dim mConst_GridViewAutoClosedLoansIndexOfLoanRequestId As Integer = 2
    Dim mConst_GridViewAutoClosedLoansIndexOfLoanNumber As Integer = 3
    Dim mConst_GridViewAutoClosedLoansIndexOfName As Integer = 4
    Dim mConst_GridViewAutoClosedLoansIndexOfYMCAName As Integer = 5
    Dim mConst_GridViewAutoClosedLoansIndexOfClosedDate As Integer = 6
    Dim mConst_GridViewAutoClosedLoansIndexOfPaymentMethod As Integer = 7
    Dim mConst_GridViewAutoClosedLoansIndexOfParticipantReport As Integer = 8
    Dim mConst_GridViewAutoClosedLoansIndexOfYMCAReport As Integer = 9
    Dim mConst_GridViewAutoClosedLoansIndexOfPersonPrintlettersId As Integer = 10
    Dim mConst_GridViewAutoClosedLoansIndexOfYMCAPrintlettersId As Integer = 11
    Dim mConst_GridViewAutoClosedLoansIndexOfYmcaId As Integer = 12
    ' END: MMR | 2019.02.12 |YRS-AT-4330 | Indexes defined for "Auto closed" loan table

#End Region
    'END: SB | 2018.04.03 | YRS-AT-3101 | Declaring Integer variables to hold cell index values of datagrid 

#Region "Events"

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            If Not Session("NoAccess") Is Nothing Then
                MessageBox.Show(PlaceHolderMessage, "YMCA-YRS", "You are not authorized to view this page.", MessageBoxButtons.Stop)
                Session("NoAccess") = Nothing
            End If

            If Not IsPostBack Then
                'Show hide link tabs
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                Else
                    BindGrid()
                    'Start: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
                    'Select Case Session("SelectTab")
                    Select Case Me.SelectedTab
                        'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
                        Case "lnkDefaultedLoans"
                            lnkDefaultedLoans_Click(sender, e)
                        Case "lnkUnfreeze_Phantom_Int"
                            lnkUnfreeze_Phantom_Int_Click(sender, e)
                            'Start: AA:12.10.2015 Added case for new loan ofsset loans tab 
                        Case "lnkOffsetLoans"
                            lnkOffsetLoans_Click(sender, e)
                            'End: AA:12.10.2015 Added case for new loan ofsset loans tab 
                        Case Else
                            lnkOffset_Default_Ageing_Click(sender, e)
                    End Select
                    ClearSessions() 'AA 09.23.2015 YRS AT-2609 Added to clear session variables
                End If
            Else
                If Not Session("LoanUnFreezeSucess") Is Nothing And Not Session("strFundId") Is Nothing Then
                    Dim objDictMsg As New Dictionary(Of String, String)
                    objDictMsg.Add("FUNDNO", Session("strFundId"))
                    If Session("LoanUnFreezeSucess") = "UNFREEZED" Then
                        HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_LOAN_HAS_UNFREEZED, Nothing, objDictMsg)
                    ElseIf Session("LoanUnFreezeSucess") = "UNFREEZED_EMAIL_ERROR" Then
                        HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_LOAN_HAS_UNFREEZED_EMAIL_ERROR, Nothing, objDictMsg)
                    End If


                    Session("LoanUnFreezeSucess") = Nothing
                    Session("strFundId") = Nothing
                    'Start: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
                    'Select Case Session("SelectTab")
                    Select Case Me.SelectedTab
                        'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
                        Case "lnkDefaultedLoans"
                            lnkDefaultedLoans_Click(sender, e)
                        Case "lnkUnfreeze_Phantom_Int"
                            lnkUnfreeze_Phantom_Int_Click(sender, e)
                            'Start: AA:12.10.2015 Added case for new loan ofsset loans tab 
                        Case "lnkOffsetLoans"
                            lnkOffsetLoans_Click(sender, e)
                            'End: AA:12.10.2015 Added case for new loan ofsset loans tab 
                            'Start: AA:04.18.2015 YRS-AT-2831 Added case for new loan Auto Defaulted loans tab 
                        Case "lnkAutoDefaultLoans"
                            lnkAutoDefaultLoans_Click(sender, e)
                            'End: AA:04.18.2015 YRS-AT-2831 Added case for new loan Auto Defaulted loans tab 
                            'Start: AA:06.29.2016 YRS-AT-2830 Added case for new loan Auto Defaulted loans tab 
                        Case "lnkAutoClosedLoans"
                            lnkAutoClosedLoans_Click(sender, e)
                            'End: AA:06.29.2016 YRS-AT-2830 Added case for new loan Auto Defaulted loans tab 
                        Case Else
                            lnkOffset_Default_Ageing_Click(sender, e)
                    End Select
                    'Start: AA:12.10.2015 Added to clear neccesary session variables after generation of report
                ElseIf Session("LoanPrintReport") IsNot Nothing Then
                    Session("strLoanRequestId") = Nothing
                    Session("strPersid") = Nothing
                    If Session("LoanPrintReport") = "ReportAndIDM" Then
                        BindGrid()
                        'Start: AA:04.18.2015 YRS-AT-2831 Added condition to handle new loan Auto Defaulted loans tab 
                        If Me.SelectedTab = "lnkOffsetLoans" Then
                            lnkOffsetLoans_Click(sender, e)
                            'Else 
                        ElseIf Me.SelectedTab = "lnkAutoDefaultLoans" Then
                            lnkAutoDefaultLoans_Click(sender, e)
                            'End: AA:04.18.2015 YRS-AT-2831 Added condition to handle new loan Auto Defaulted loans tab 
                            'Start: AA:06.29.2016 YRS-AT-2830 Added condition to handle new loan Auto Closed loans tab 
                        ElseIf Me.SelectedTab = "lnkAutoClosedLoans" Then
                            lnkAutoClosedLoans_Click(sender, e)
                            'End: AA:06.29.2016 YRS-AT-2830 Added condition to handle new loan Auto Closed loans tab 
                        End If
                        Session("strDocCode") = Nothing

                    End If
                    Session("LoanPrintReport") = Nothing
                    'End: AA:12.10.2015 Added to clear neccesary session variables after generation of report
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_Page_Load", ex)
        End Try
    End Sub
    'Start: Bala: 04.20.2016: YRS-AT-2862
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Select Case Me.SelectedTab
            Case "lnkOffset_Default_Ageing"
                lblLoanStatusDesc.Text = "Loan Status: PAID"
            Case "lnkDefaultedLoans"
                lblLoanStatusDesc.Text = "Loan Status: DEFAULT" 'VC | 2018.10.11 | YRS-AT-4018 | Replaced DEFALT with DEFAULT
            Case "lnkUnfreeze_Phantom_Int"
                lblLoanStatusDesc.Text = "Loan Status: DEFAULT" 'VC | 2018.10.11 | YRS-AT-4018 | Replaced DEFALT with DEFAULT
            Case "lnkOffsetLoans"
                lblLoanStatusDesc.Text = "Loan Status: OFFSET"
            Case "lnkAutoDefaultLoans"
                lblLoanStatusDesc.Text = "Loan Status: DEFAULT" 'VC | 2018.10.11 | YRS-AT-4018 | Replaced DEFALT with DEFAULT
                'Start:AA:07.01.2016 YRS-At-2830 Added to show the loan status as closed in label
            Case "lnkAutoClosedLoans"
                lblLoanStatusDesc.Text = "Loan Status: CLOSED"
                'End:AA:07.01.2016 YRS-At-2830 Added to show the loan status as closed in label
        End Select
    End Sub
    'End: Bala: 04.20.2016: YRS-AT-2862
#End Region

#Region "Button Events"

    Private Sub lnkDefaultedLoans_Click(sender As Object, e As EventArgs) Handles lnkDefaultedLoans.Click
        Dim dtRecords As DataTable
        Dim dsLoansDetails As DataSet
        Try
            'Start: AA:12.10.2015 Commented and put common code of all tabs put into one method
            'lnkOffset_Default_Ageing.Visible = True
            'lnkUnfreeze_Phantom_Int.Visible = True
            'lnkDefaultedLoans.Visible = False
            'lblDefaultedLoans.Visible = True
            'lblUnfreeze_Phantom_Int.Visible = False
            'lblOffset_Default_Ageing.Visible = False
            'gvOffSetDefaultAgeing.Visible = False
            'gvDefaultedLoans.Visible = True
            'gvUnfreeze.Visible = False
            'trDefaultLoan.Visible = True
            'trDefaultAgeing.Visible = False
            'trUnFreeze.Visible = False
            'Start:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
            'chkDefaultEligible.Visible = False
            'chkOffsetEligible.Visible = True
            'chkDefaultEligible.Checked = False
            'chkOffsetEligible.Checked = False
            SetAllControls(False)
            SetTabValues(LoanUtilityTab.Default)
            'End: AA:12.10.2015 Commented and put common code of all tabs put into one method
            'End:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
            lblDescription.Text = "Defaulted Loans"
            'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            'Session("SelectTab") = "lnkDefaultedLoans"
            Me.SelectedTab = "lnkDefaultedLoans"
            'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            ViewState("DefaultedLoans_Filter") = Nothing
            ViewState("DefaultedLoans_sort") = Nothing
            'Start - SR:2015.09.18:YRS-AT-2441 - If data does not exist then bind with no records 
            dsLoansDetails = DirectCast(Session("dsLoansDetailsList"), DataSet)
            If dsLoansDetails IsNot Nothing Then
                If dsLoansDetails.Tables.Count > 1 Then
                    dtRecords = dsLoansDetails.Tables(1)
                End If
                If HelperFunctions.isNonEmpty(dtRecords) Then
                    gvDefaultedLoans.DataSource = dtRecords
                    gvDefaultedLoans.DataBind()
                    ViewState("DefaultedLoans_dtLoanList") = dtRecords
                    LblCount.Text = dtRecords.Rows.Count
                Else
                    gvDefaultedLoans.DataSource = Nothing
                    gvDefaultedLoans.DataBind()
                    ViewState("DefaultedLoans_dtLoanList") = Nothing
                    LblCount.Text = "0"
                End If
            Else
                gvDefaultedLoans.DataSource = Nothing
                gvDefaultedLoans.DataBind()
                ViewState("DefaultedLoans_dtLoanList") = Nothing
                LblCount.Text = "0"
            End If
            'End - SR:2015.09.18:YRS-AT-2441 - If data does not exist then bind with no records 
            gvDefaultedLoans.SelectedIndex = -1
            gvDefaultedLoans.PageIndex = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_lnkDefaultedLoans_Click", ex)
        End Try
    End Sub

    Private Sub lnkOffset_Default_Ageing_Click(sender As Object, e As EventArgs) Handles lnkOffset_Default_Ageing.Click
        Dim dtRecords As DataTable
        Dim dsLoansDetails As DataSet
        Try
            'Start: AA:12.10.2015 Commented and put common code of all tabs put into one method
            'lnkOffset_Default_Ageing.Visible = False
            'lnkUnfreeze_Phantom_Int.Visible = True
            'lnkDefaultedLoans.Visible = True
            'lblDefaultedLoans.Visible = False
            'lblUnfreeze_Phantom_Int.Visible = False
            'lblOffset_Default_Ageing.Visible = True
            'gvOffSetDefaultAgeing.Visible = True
            'gvDefaultedLoans.Visible = False
            'gvUnfreeze.Visible = False
            'trDefaultLoan.Visible = False
            'trDefaultAgeing.Visible = True
            'trUnFreeze.Visible = False            
            'Start:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
            'chkDefaultEligible.Visible = True
            'chkOffsetEligible.Visible = True
            'chkDefaultEligible.Checked = False
            'chkOffsetEligible.Checked = False
            SetAllControls(False)
            SetTabValues(LoanUtilityTab.Aging)
            'End: AA:12.10.2015 Commented and put common code of all tabs put into one method
            'End:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
            lblDescription.Text = "Offset / Default / Aging"
            'Start: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            'Session("SelectTab") = "lnkOffset_Default_Ageing"
            Me.SelectedTab = "lnkOffset_Default_Ageing"
            'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            ViewState("Offset_Default_Ageing_Filter") = Nothing
            ViewState("Offset_Default_Ageing_sort") = Nothing
            'Start - SR:2015.09.18:YRS-AT-2441 - If data does not exist then bind with no records 
            dsLoansDetails = DirectCast(Session("dsLoansDetailsList"), DataSet)
            If HelperFunctions.isNonEmpty(dsLoansDetails) Then
                If dsLoansDetails.Tables.Count > 0 Then
                    dtRecords = dsLoansDetails.Tables(0)
                End If
                If HelperFunctions.isNonEmpty(dtRecords) Then
                    gvOffSetDefaultAgeing.DataSource = dtRecords
                    gvOffSetDefaultAgeing.DataBind()
                    ViewState("Offset_Default_Ageing_dtLoanList") = dtRecords
                    LblCount.Text = dtRecords.Rows.Count
                Else
                    gvOffSetDefaultAgeing.DataSource = Nothing
                    gvOffSetDefaultAgeing.DataBind()
                    ViewState("Offset_Default_Ageing_dtLoanList") = Nothing
                    LblCount.Text = "0"
                End If
            Else
                gvOffSetDefaultAgeing.DataSource = Nothing
                gvOffSetDefaultAgeing.DataBind()
                ViewState("Offset_Default_Ageing_dtLoanList") = Nothing
                LblCount.Text = "0"
            End If
            'End - SR:2015.09.18:YRS-AT-2441 - If data does not exist then bind with no records 
            gvOffSetDefaultAgeing.SelectedIndex = -1
            gvOffSetDefaultAgeing.PageIndex = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_lnkOffset_Default_Ageing_Click", ex)
        End Try
    End Sub

    Private Sub lnkUnfreeze_Phantom_Int_Click(sender As Object, e As EventArgs) Handles lnkUnfreeze_Phantom_Int.Click
        Dim dtRecords As DataTable
        Dim dsLoansDetails As DataSet
        Try
            'Start: AA:12.10.2015 Commented and put common code of all tabs put into one method
            'lnkOffset_Default_Ageing.Visible = True
            'lnkUnfreeze_Phantom_Int.Visible = False
            'lnkDefaultedLoans.Visible = True
            'lblDefaultedLoans.Visible = False
            'lblUnfreeze_Phantom_Int.Visible = True
            'lblOffset_Default_Ageing.Visible = False
            'gvOffSetDefaultAgeing.Visible = False
            'gvDefaultedLoans.Visible = False
            'gvUnfreeze.Visible = True
            'trDefaultLoan.Visible = False
            'trDefaultAgeing.Visible = False
            'trUnFreeze.Visible = True
            'Start:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
            'chkDefaultEligible.Visible = False
            'chkOffsetEligible.Visible = False
            'chkDefaultEligible.Checked = False
            'chkOffsetEligible.Checked = False
            SetAllControls(False)
            SetTabValues(LoanUtilityTab.Freeze)
            'End: AA:12.10.2015 Commented and put common code of all tabs put into one method
            'END:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
            lblDescription.Text = "Unfreeze Phantom Interest"
            'Start: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            'Session("SelectTab") = "lnkUnfreeze_Phantom_Int"
            Me.SelectedTab = "lnkUnfreeze_Phantom_Int"
            'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            ViewState("Unfreeze_Phantom_Int_Filter") = Nothing
            ViewState("Unfreeze_Phantom_Int_sort") = Nothing
            'Start - SR:2015.09.18:YRS-AT-2441 - If data does not exist then bind with no records 
            dsLoansDetails = DirectCast(Session("dsLoansDetailsList"), DataSet)
            If dsLoansDetails IsNot Nothing Then
                If dsLoansDetails.Tables.Count > 2 Then
                    dtRecords = dsLoansDetails.Tables(2)
                End If
                dtRecords = DirectCast(Session("dsLoansDetailsList"), DataSet).Tables(2)
                If HelperFunctions.isNonEmpty(dtRecords) Then
                    gvUnfreeze.DataSource = dtRecords
                    gvUnfreeze.DataBind()
                    ViewState("Unfreeze_Phantom_Int_dtLoanList") = dtRecords
                    LblCount.Text = dtRecords.Rows.Count
                Else
                    gvUnfreeze.DataSource = Nothing
                    gvUnfreeze.DataBind()
                    ViewState("Unfreeze_Phantom_Int_dtLoanList") = Nothing
                    LblCount.Text = "0"
                End If
            Else
                gvUnfreeze.DataSource = Nothing
                gvUnfreeze.DataBind()
                ViewState("Unfreeze_Phantom_Int_dtLoanList") = Nothing
                LblCount.Text = "0"
            End If
            'End - SR:2015.09.18:YRS-AT-2441 - If data does not exist then bind with no records 
            gvUnfreeze.SelectedIndex = -1
            gvUnfreeze.PageIndex = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_lnkUnfreeze_Phantom_Int_Click", ex)
        End Try

    End Sub

    'Start: AA:12.10.2015 Added a new link button to show automated offset loans
    Private Sub lnkOffsetLoans_Click(sender As Object, e As EventArgs) Handles lnkOffsetLoans.Click
        Dim dtRecords As DataTable
        Dim dsLoansDetails As DataSet
        Try
            SetAllControls(False)
            SetTabValues(LoanUtilityTab.Offset)

            lblDescription.Text = "Auto-Offset Loans"
            'Start: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            'Session("SelectTab") = "lnkOffsetLoans"
            Me.SelectedTab = "lnkOffsetLoans"
            'End: Bala: 25.02.2016: YRS-AT-2568: Renaming and changing the session variable to view state to differentiate between the session used in this and the RMDPrintLetters page
            ViewState("OffsetLoans_Filter") = Nothing
            ViewState("OffsetLoans_sort") = Nothing

            dsLoansDetails = DirectCast(Session("dsLoansDetailsList"), DataSet)
            If dsLoansDetails IsNot Nothing Then
                If dsLoansDetails.Tables.Count > 4 Then
                    dtRecords = dsLoansDetails.Tables(4)
                End If
                If HelperFunctions.isNonEmpty(dtRecords) Then
                    gvOffsetLoans.DataSource = dtRecords
                    gvOffsetLoans.DataBind()
                    ViewState("OffsetLoans_dtLoanList") = dtRecords
                    LblCount.Text = dtRecords.Rows.Count
                Else
                    gvOffsetLoans.DataSource = Nothing
                    gvOffsetLoans.DataBind()
                    ViewState("OffsetLoans_dtLoanList") = Nothing
                    LblCount.Text = "0"
                End If
            Else
                gvOffsetLoans.DataSource = Nothing
                gvOffsetLoans.DataBind()
                ViewState("OffsetLoans_dtLoanList") = Nothing
                LblCount.Text = "0"
            End If

            gvOffsetLoans.SelectedIndex = -1
            gvOffsetLoans.PageIndex = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_lnkUnfreeze_Phantom_Int_Click", ex)
        End Try
    End Sub
    'End: AA:12.10.2015 Added a new link button to show automated offset loans
    'Start: AA:04.18.2016 YRS-AT-2831:Added a new link button to show automated default loans
    Private Sub lnkAutoDefaultLoans_Click(sender As Object, e As EventArgs) Handles lnkAutoDefaultLoans.Click
        Dim dtRecords As DataTable
        Dim dsLoansDetails As DataSet
        Try
            SetAllControls(False)
            SetTabValues(LoanUtilityTab.AutoDefault)

            lblDescription.Text = "Auto-Default Loans"
            Me.SelectedTab = "lnkAutoDefaultLoans"
            ViewState("AutoDefaultLoans_Filter") = Nothing
            ViewState("AutoDefaultLoans_sort") = Nothing

            dsLoansDetails = DirectCast(Session("dsLoansDetailsList"), DataSet)
            If dsLoansDetails IsNot Nothing Then
                If dsLoansDetails.Tables.Count > 5 Then
                    dtRecords = dsLoansDetails.Tables(5)
                End If
                If HelperFunctions.isNonEmpty(dtRecords) Then
                    gvAutoDefaultLoans.DataSource = dtRecords
                    gvAutoDefaultLoans.DataBind()
                    ViewState("AutoDefaultLoans_dtLoanList") = dtRecords
                    LblCount.Text = dtRecords.Rows.Count
                Else
                    gvAutoDefaultLoans.DataSource = Nothing
                    gvAutoDefaultLoans.DataBind()
                    ViewState("AutoDefaultLoans_dtLoanList") = Nothing
                    LblCount.Text = "0"
                End If
            Else
                gvAutoDefaultLoans.DataSource = Nothing
                gvAutoDefaultLoans.DataBind()
                ViewState("AutoDefaultLoans_dtLoanList") = Nothing
                LblCount.Text = "0"
            End If

            gvAutoDefaultLoans.SelectedIndex = -1
            gvAutoDefaultLoans.PageIndex = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_lnkAutoDefaultLoans_Click", ex)
        End Try
    End Sub
    'End: AA:04.18.2016 YRS-AT-2831: Added a new link button to show automated default loans
    'Start: AA:05.29.2016 YRS-AT-2830: Added a new link button to show automated closed loans
    Private Sub lnkAutoClosedLoans_Click(sender As Object, e As EventArgs) Handles lnkAutoClosedLoans.Click
        Dim dtRecords As DataTable
        Dim dsLoansDetails As DataSet
        Try
            SetAllControls(False)
            SetTabValues(LoanUtilityTab.AutoClosed)

            lblDescription.Text = "Auto-Closed Loans"
            Me.SelectedTab = "lnkAutoClosedLoans"
            ViewState("AutoClosedLoans_Filter") = Nothing
            ViewState("AutoClosedLoans_Sort") = Nothing

            gvAutoClosedLoans.DataSource = Nothing
            gvAutoClosedLoans.DataBind()
            ViewState("AutoClosedLoans_dtLoanList") = Nothing
            LblCount.Text = "0"

            dsLoansDetails = DirectCast(Session("dsLoansDetailsList"), DataSet)
            If dsLoansDetails IsNot Nothing Then
                If dsLoansDetails.Tables.Count > 6 Then
                    dtRecords = dsLoansDetails.Tables(6)
                End If
                If HelperFunctions.isNonEmpty(dtRecords) Then
                    gvAutoClosedLoans.DataSource = dtRecords
                    gvAutoClosedLoans.DataBind()
                    ViewState("AutoClosedLoans_dtLoanList") = dtRecords
                    LblCount.Text = dtRecords.Rows.Count
                End If
            End If

            gvAutoClosedLoans.SelectedIndex = -1
            gvAutoClosedLoans.PageIndex = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_lnkAutoClosedLoans_Click", ex)
        End Try
    End Sub
    'End: AA:05.29.2016 YRS-AT-2830: Added a new link button to show automated closed loans

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        SearchRecords()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtFundId.Text = ""
        'Start:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
        chkDefaultEligible.Checked = False
        chkOffsetEligible.Checked = False
        'End:AA:09.22.2015 YRS AT-2609 Added below lines to clear the checkbox filers
        SearchRecords()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ClearSessions() 'AA 09.23.2015 YRS AT-2609 Added to clear session variables
        Response.Redirect("MainWebform.aspx", False)
    End Sub
    'Start:AA:09.22.2015 YRS AT-2609 Added below lines to print the list shown in the grid
    Private Sub btnPrintList_Click(sender As Object, e As EventArgs) Handles btnPrintList.Click
        Dim dtRecords As New DataTable
        Dim dv As New DataView
        Dim Sort As GridViewCustomSort
        Dim strFilter As String = String.Empty
        Dim strSessionName As String = String.Empty
        Dim strReportName As String = String.Empty
        Dim dtFinalTable As DataTable
        Try

            If lblDefaultedLoans.Visible Then
                dtRecords = DirectCast(ViewState("DefaultedLoans_dtLoanList"), DataTable)
                strFilter = ViewState("DefaultedLoans_Filter")
                Sort = ViewState("DefaultedLoans_sort")
                strSessionName = "dtDefaulted"
                strReportName = "Loan_Default"
            ElseIf lblOffset_Default_Ageing.Visible Then
                dtRecords = DirectCast(ViewState("Offset_Default_Ageing_dtLoanList"), DataTable)
                strFilter = ViewState("Offset_Default_Ageing_Filter")
                Sort = ViewState("Offset_Default_Ageing_sort")
                strSessionName = "dtOffset_Default_Ageing"
                strReportName = "Loan_Offset_Default_Aging"
            ElseIf lblUnfreeze_Phantom_Int.Visible Then
                dtRecords = DirectCast(ViewState("Unfreeze_Phantom_Int_dtLoanList"), DataTable)
                strFilter = ViewState("Unfreeze_Phantom_Int_Filter")
                Sort = ViewState("Unfreeze_Phantom_Int_sort")
                strSessionName = "dtUnfreeze"
                strReportName = "Loan_Unfreeze_Phantom_Interest"
                'Start: AA:12.10.2015 Added print records which are in the offset loans grid
            ElseIf lblOffsetLoans.Visible Then
                dtRecords = DirectCast(ViewState("OffsetLoans_dtLoanList"), DataTable)
                strFilter = ViewState("OffsetLoans_Filter")
                Sort = ViewState("OffsetLoans_sort")
                strSessionName = "dtAutoLoansOffset"
                strReportName = "Auto_Loan_Offset"
                'End: AA:12.10.2015 Added print records which are in the offset loans grid
                'Start: AA:04.18.2016 YRS-AT-2831:Added print records which are in the Auto defulted loans grid
            ElseIf lblAutoDefaultLoans.Visible Then
                dtRecords = DirectCast(ViewState("AutoDefaultLoans_dtLoanList"), DataTable)
                strFilter = ViewState("AutoDefaultLoans_Filter")
                Sort = ViewState("AutoDefaultLoans_sort")
                strSessionName = "dtAutoLoansDefault"
                strReportName = "Auto_Loan_Default"
                'End: AA:04.18.2016 YRS-AT-2831:Added print records which are in the Auto defulted loans grid
                'Start: AA:06.29.2016 YRS-AT-2830:Added print records which are in the Auto closed loans grid
            ElseIf lblAutoClosedLoans.Visible Then
                dtRecords = DirectCast(ViewState("AutoClosedLoans_dtLoanList"), DataTable)
                strFilter = ViewState("AutoClosedLoans_Filter")
                Sort = ViewState("AutoClosedLoans_Sort")
                strSessionName = "dtAutoLoansClosed"
                strReportName = "Auto_Loan_Closed"
                'End: AA:04.18.2016 YRS-AT-2831:Added print records which are in the Auto closed loans grid
            End If

            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView

                If Not String.IsNullOrEmpty(strFilter) Then
                    dv.RowFilter = strFilter
                End If

                If Sort IsNot Nothing Then
                    dv.Sort = Sort.SortExpression + " " + Sort.SortDirection
                End If
            End If

            If HelperFunctions.isEmpty(dv) Then
                HelperFunctions.ShowMessageToUser("No record(s) exist to print.", EnumMessageTypes.Error)
                Exit Sub
            End If

            dtFinalTable = dv.ToTable.Copy()

            If Not lblUnfreeze_Phantom_Int.Visible Then
                dtFinalTable.Columns.Add("OffsetReason", GetType(String))
                For Each dr As DataRow In dtFinalTable.Rows
                    If dr IsNot Nothing Then
                        'Start AA:04.30.2016 YRS-AT-2831 checking the 
                        'If dr("MessageNo") > 0 Then
                        If dtFinalTable.Columns.Contains("MessageNo") AndAlso dr("MessageNo") > 0 Then
                            dr("OffsetReason") = MetaMessageBO.GetMessageByMessageNo(dr("MessageNo").ToString()).DisplayCode
                        End If
                        'End:AA:09.23.2015 YRS AT-2609 Added below lines to show Age as Y and M format in report
                        If dtFinalTable.Columns.Contains("Age") Then 'AA:12.10.2015 Added to check whether the age column exits in the datatable or not
                            dr("Age") = GetAgeFormat(dr("Age").ToString().Trim)
                        End If
                    End If

                Next
            Else
                For Each dr As DataRow In dtFinalTable.Rows
                    If dr IsNot Nothing Then
                        dr("Age") = GetAgeFormat(dr("Age").ToString().Trim)
                        'End:AA:09.23.2015 YRS AT-2609 Added below lines to show Age as Y and M format in report
                    End If
                Next
            End If
            'Start: AA 22.01.2016 YRS-AT-2688 Changed the column defination to show only date not datetime
            If dtFinalTable.Columns.Contains("DefaultDate") AndAlso dtFinalTable.Columns("DefaultDate").DataType = GetType(DateTime) Then
                dtFinalTable.Columns.Remove("DefaultDate")
                dtFinalTable.Columns.Add("DefaultDate", GetType(String))
                For iCount As Integer = 0 To dtFinalTable.Rows.Count - 1
                    dtFinalTable.Rows(iCount)("DefaultDate") = Convert.ToDateTime(dv(iCount)("DefaultDate")).ToShortDateString
                Next
            End If
            'End: AA 22.01.2016 YRS-AT-2688 Changed the column defination to show only date not datetime
            'Start: AA 29.05.2016 YRS-AT-2830 Added below code to show only date format
            If dtFinalTable.Columns.Contains("ClosedDate") AndAlso dtFinalTable.Columns("ClosedDate").DataType = GetType(DateTime) Then
                dtFinalTable.Columns.Remove("ClosedDate")
                dtFinalTable.Columns.Add("ClosedDate", GetType(String))
                For iCount As Integer = 0 To dtFinalTable.Rows.Count - 1
                    dtFinalTable.Rows(iCount)("ClosedDate") = Convert.ToDateTime(dv(iCount)("ClosedDate")).ToShortDateString
                Next
            End If
            'End: AA 29.05.2016 YRS-AT-2830 Added below code to show only date format
            Session(strSessionName) = dtFinalTable

            Session("ReportName") = strReportName

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_btnPrintList_Click", ex)
        End Try
    End Sub
    'End:AA:09.22.2015 YRS AT-2609 Added below lines to print the list shown in the grid
#End Region

#Region "Grid Events"



#Region "Row DataBound Events"
    Private Sub gvOffSetDefaultAgeing_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvOffSetDefaultAgeing.RowDataBound
        Dim IsOffsetEligible As Boolean
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("Offset_Default_Ageing_sort"), e)
                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'e.Row.Cells(12).Visible = False 'AA:09.23.2015 YRS AT-2609 Added to hide the isoffseteligible column
                e.Row.Cells(mConst_DataGridLoanPaidIndexOfIsOffsetEligible).Visible = False
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 
                'e.Row.Cells(12).Visible = False 'AA:09.23.2015 YRS AT-2609 Added to hide the isoffseteligible column
                e.Row.Cells(mConst_DataGridLoanPaidIndexOfIsOffsetEligible).Visible = False
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                Dim lnkMaintenance As LinkButton = DirectCast(e.Row.FindControl("lnkMaintenance"), LinkButton) '18.Jun.2015 B.Jagadeesh BT-2699 YRS 5.0-2441 Modifications for 403b Loans
                'Start:AA:09.23.2015 YRS AT-2609 Changed to show age in Y and M format from a function
                'Dim strAge As String
                'Dim chrSplitVal As Char() = {"."}
                'Dim strSplitAge As String()
                'strAge = e.Row.Cells(6).Text.Trim
                'strSplitAge = strAge.Split(chrSplitVal, StringSplitOptions.RemoveEmptyEntries)
                'If strSplitAge.Length > 0 Then
                '    e.Row.Cells(6).Text = strSplitAge(0) + "Y / " + strSplitAge(1) + "M"
                'End If

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 
                'e.Row.Cells(6).Text = GetAgeFormat(e.Row.Cells(6).Text.Trim)
                e.Row.Cells(mConst_DataGridLoanPaidIndexOfAge).Text = GetAgeFormat(e.Row.Cells(mConst_DataGridLoanPaidIndexOfAge).Text.Trim)
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                'End:AA:09.23.2015 YRS AT-2609 Changed to show age in Y and M format from a function
                'Start: AA 22.01.2016 YRS-AT-2688 Changed the column numbers to accomodate a new default date column

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'IsOffsetEligible = Convert.ToBoolean(e.Row.Cells(12).Text.Trim) 'AA:09.23.2015 YRS AT-2609 Added to get the isoffseteligible column value
                'If e.Row.Cells(10).Text.Trim <> "" AndAlso e.Row.Cells(10).Text.Trim <> "0" Then
                IsOffsetEligible = Convert.ToBoolean(e.Row.Cells(mConst_DataGridLoanPaidIndexOfIsOffsetEligible).Text.Trim) 'AA:09.23.2015 YRS AT-2609 Added to get the isoffseteligible column value
                If e.Row.Cells(mConst_DataGridLoanPaidIndexOfMessageNo).Text.Trim <> "" AndAlso e.Row.Cells(mConst_DataGridLoanPaidIndexOfMessageNo).Text.Trim <> "0" Then
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 

                    'AA:08.12.20115 YRS 5.0-2441 :Modified to display code insteasdof full text
                    'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 
                    'e.Row.Cells(10).Text = MetaMessageBO.GetMessageByMessageNo(e.Row.Cells(10).Text.Trim).DisplayCode
                    e.Row.Cells(mConst_DataGridLoanPaidIndexOfMessageNo).Text = MetaMessageBO.GetMessageByMessageNo(e.Row.Cells(mConst_DataGridLoanPaidIndexOfMessageNo).Text.Trim).DisplayCode
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                Else
                    'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 
                    'e.Row.Cells(10).Text = ""
                    e.Row.Cells(mConst_DataGridLoanPaidIndexOfMessageNo).Text = ""
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                End If
                'End: AA 22.01.2016 YRS-AT-2688 Changed the column numbers to accomodate a new default date column
                'Start:18.Jun.2015 B.Jagadeesh BT-2699 YRS 5.0-2441 Modifications for 403b Loans

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If Val(e.Row.Cells(8).Text.ToString().Trim) < Val(ViewState("Max_Cure_Period_CutOff_Days")) Then
                If Val(e.Row.Cells(mConst_DataGridLoanPaidIndexOfDaysOverDue).Text.ToString().Trim) < Val(ViewState("Max_Cure_Period_CutOff_Days")) Then
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable                
                    lnkMaintenance.Visible = False
                    'Start:AA:09.22.2015 YRS AT-2609 Added below lines to show the color differntiation for default and offset
                ElseIf IsOffsetEligible Then
                    e.Row.CssClass = "BG_Colour_Loan_Offset"
                Else
                    e.Row.CssClass = "BG_Colour_Loan_Default"
                End If
                'End:AA:09.22.2015 YRS AT-2609 Added below lines to show the color differntiation for default and offset
                'End:18.Jun.2015 B.Jagadeesh BT-2699 YRS 5.0-2441 Modifications for 403b Loans
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub gvDefaultedLoans_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDefaultedLoans.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("DefaultedLoans_sort"), e)
            End If

            'Start - SR:2015.09.18:YRS-AT-2441 - If data exist then only hide cells(11)
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'e.Row.Cells(10).Visible = False
                e.Row.Cells(mConst_DataGridLoanDefaltIndexOfIsTerminated).Visible = False
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
            End If
            'End - SR:2015.09.18: YRS-AT-2441 - If data exist then only hide cells(11)

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim strAge As String
                Dim blnIsTerminated As Boolean
                'Start:AA:09.23.2015 YRS AT-2609 Changed to show age in Y and M format from a function
                'Dim chrSplitVal As Char() = {"."}
                Dim lnkMaintenance As LinkButton
                'Dim strSplitAge As String()

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'strAge = e.Row.Cells(6).Text.Trim
                'blnIsTerminated = Convert.ToBoolean(e.Row.Cells(10).Text.Trim)
                strAge = e.Row.Cells(mConst_DataGridLoanDefaltIndexOfAge).Text.Trim
                blnIsTerminated = Convert.ToBoolean(e.Row.Cells(mConst_DataGridLoanDefaltIndexOfIsTerminated).Text.Trim)
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                'strSplitAge = strAge.Split(chrSplitVal, StringSplitOptions.RemoveEmptyEntries)
                'If strSplitAge.Length > 0 Then
                '    e.Row.Cells(6).Text = strSplitAge(0) + "Y / " + strSplitAge(1) + "M"
                'End If

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'e.Row.Cells(6).Text = GetAgeFormat(e.Row.Cells(6).Text.Trim)
                e.Row.Cells(mConst_DataGridLoanDefaltIndexOfAge).Text = GetAgeFormat(e.Row.Cells(mConst_DataGridLoanDefaltIndexOfAge).Text.Trim)
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                'End:AA:09.23.2015 YRS AT-2609 Changed to show age in Y and M format from a function
                lnkMaintenance = (DirectCast(e.Row.FindControl("lnkMaintenance"), LinkButton))
                'Start: AA 22.01.2016 YRS-AT-2688 Changed the column numbers to accomodate a new default date column

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If e.Row.Cells(12).Text.Trim <> "" AndAlso e.Row.Cells(12).Text.Trim <> "0" Then
                If e.Row.Cells(mConst_DataGridLoanDefaltIndexOfMessageNo).Text.Trim <> "" AndAlso e.Row.Cells(mConst_DataGridLoanDefaltIndexOfMessageNo).Text.Trim <> "0" Then
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 

                    'AA:08.12.20115 YRS 5.0-2441 :Modified to display code insteasdof full text

                    'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                    'e.Row.Cells(12).Text = MetaMessageBO.GetMessageByMessageNo(e.Row.Cells(12).Text.Trim).DisplayCode
                    e.Row.Cells(mConst_DataGridLoanDefaltIndexOfMessageNo).Text = MetaMessageBO.GetMessageByMessageNo(e.Row.Cells(mConst_DataGridLoanDefaltIndexOfMessageNo).Text.Trim).DisplayCode
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                Else
                    'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 
                    'e.Row.Cells(12).Text = ""
                    e.Row.Cells(mConst_DataGridLoanDefaltIndexOfMessageNo).Text = ""
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 
                End If
                'End: AA 22.01.2016 YRS-AT-2688 Changed the column numbers to accomodate a new default date column
                If strAge >= ViewState("Loan_Offset_Age") OrElse blnIsTerminated Then
                    lnkMaintenance.Visible = True
                    e.Row.CssClass = "BG_Colour_Loan_Offset" 'AA:09.22.2015 YRS AT-2609 Added below lines to show the color differntiation for offset
                Else
                    lnkMaintenance.Visible = False
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub gvUnfreeze_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUnfreeze.RowDataBound
        Try
            Dim lnkunfreeze As LinkButton
            'START: Shilpa N | 03/06/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                lnkunfreeze = e.Row.FindControl("lnkUnFreeze")
                If lnkunfreeze IsNot Nothing Then
                    lnkunfreeze.Enabled = False
                    lnkunfreeze.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                End If
            End If
            'END: Shilpa N | 03/06/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("Unfreeze_Phantom_Int_sort"), e)
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                'Start:AA:09.23.2015 YRS AT-2609 Changed to show age in Y and M format from a function
                'Dim strAge As String
                'Dim chrSplitVal As Char() = {"."}
                'Dim strSplitAge As String()
                'strAge = e.Row.Cells(6).Text.Trim
                'strSplitAge = strAge.Split(chrSplitVal, StringSplitOptions.RemoveEmptyEntries)
                'If strSplitAge.Length > 0 Then
                '    e.Row.Cells(6).Text = strSplitAge(0) + "Y / " + strSplitAge(1) + "M"
                'End If

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'e.Row.Cells(6).Text = GetAgeFormat(e.Row.Cells(6).Text.Trim)
                e.Row.Cells(mConst_DataGridLoanUnfreezePhantomInterestIndexOfAge).Text = GetAgeFormat(e.Row.Cells(mConst_DataGridLoanUnfreezePhantomInterestIndexOfAge).Text.Trim)
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'End:AA:09.23.2015 YRS AT-2609 Changed to show age in Y and M format from a function
                'Start:18.Jun.2015 B.Jagadeesh BT-2699 YRS 5.0-2441 Modifications for 403b Loans

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If Val(e.Row.Cells(9).Text.ToString().Trim) > Val(ViewState("Loan_UnFreeze_Days")) Then
                If Val(e.Row.Cells(mConst_DataGridLoanUnfreezePhantomInterestIndexOfFrozenDays).Text.ToString().Trim) > Val(ViewState("Loan_UnFreeze_Days")) Then
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                    tdUnfreeze.Style.Add("color", "red")
                    lnkUnfreeze_Phantom_Int.Style.Add("color", "red")
                    lblUnfreeze_Phantom_Int.Style.Add("color", "red")
                End If
                'End:18.Jun.2015 B.Jagadeesh BT-2699 YRS 5.0-2441 Modifications for 403b Loans
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Start: AA:12.10.2015 Added to bound columns data values of offset loans grid
    Private Sub gvOffsetLoans_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvOffsetLoans.RowDataBound
        Try
            Dim lnkReport As LinkButton
            'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                lnkReport = e.Row.FindControl("lnkReport")
                If lnkReport IsNot Nothing Then
                    lnkReport.Enabled = False
                    lnkReport.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                End If
            End If
            'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("OffsetLoans_sort"), e)
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If e.Row.Cells(11).Text.ToString().Trim <> "0" Then
                If e.Row.Cells(mConst_DataGridLoanOffsetIndexOfPrintLettersID).Text.ToString().Trim <> "0" Then
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                    e.Row.CssClass = "BG_Colour_Loan_Report"
                    lnkReport = e.Row.FindControl("lnkReport")
                    If lnkReport IsNot Nothing Then
                        lnkReport.Text = "View Letter"
                    End If
                End If

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If e.Row.Cells(8).Text.Trim <> "" AndAlso e.Row.Cells(8).Text.Trim <> "0" Then
                'e.Row.Cells(8).Text = MetaMessageBO.GetMessageByMessageNo(e.Row.Cells(8).Text.Trim).DisplayCode
                'Else
                'e.Row.Cells(8).Text = ""
                ' End If
                If e.Row.Cells(mConst_DataGridLoanOffsetIndexOfMessageNo).Text.Trim <> "" AndAlso e.Row.Cells(mConst_DataGridLoanOffsetIndexOfMessageNo).Text.Trim <> "0" Then
                    e.Row.Cells(mConst_DataGridLoanOffsetIndexOfMessageNo).Text = MetaMessageBO.GetMessageByMessageNo(e.Row.Cells(mConst_DataGridLoanOffsetIndexOfMessageNo).Text.Trim).DisplayCode
                Else
                    e.Row.Cells(mConst_DataGridLoanOffsetIndexOfMessageNo).Text = ""
                End If
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable 

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvOffsetLoans_RowDataBound", ex)
        End Try
    End Sub
    'End: AA:12.10.2015 Added to bound columns data values of offset loans grid

    'Start: AA:04.18.2016 YRS_AT-2831 Added to bound columns data values of auto defaulted loans grid
    Private Sub gvAutoDefaultLoans_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAutoDefaultLoans.RowDataBound
        Try
            Dim lnkReport As LinkButton
            'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                lnkReport = e.Row.FindControl("lnkReport")
                If lnkReport IsNot Nothing Then
                    lnkReport.Enabled = False
                    lnkReport.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                End If
            End If
            'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("AutoDefaultLoans_sort"), e)
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If e.Row.Cells(7).Text.ToString().Trim <> "0" Then
                If e.Row.Cells(mConst_DataGridLoanAutoDefaltIndexOfDPrinterLettersID).Text.ToString().Trim <> "0" Then
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                    e.Row.CssClass = "BG_Colour_Loan_Report"
                    lnkReport = e.Row.FindControl("lnkReport")
                    If lnkReport IsNot Nothing Then
                        lnkReport.Text = "View Letter"
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvAutoDefaultLoans_RowDataBound", ex)
        End Try
    End Sub
    'End: AA:04.18.2016 YRS_AT-2831 Added to bound columns data values of auto defaulted loans grid

    'Start: AA:06.29.2016 YRS_AT-2830 Added to bound columns data values of auto closed loans grid
    Private Sub gvAutoClosedLoans_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAutoClosedLoans.RowDataBound
        Dim lnkReport As LinkButton
        Dim blnIsParticipantLetterGenerated As Boolean
        Dim blnIsYMCALetterGenerated As Boolean
        Try
            'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                lnkReport = e.Row.FindControl("lnkParticipantReport")
                Dim lnkymcareport As LinkButton
                lnkymcareport = e.Row.FindControl("lnkYMCAReport")
                Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                If lnkReport IsNot Nothing Then
                    lnkReport.Enabled = False
                    lnkReport.ToolTip = toolTip
                    lnkymcareport.Enabled = False
                    lnkymcareport.ToolTip = toolTip
                End If
            End If
            'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

            blnIsParticipantLetterGenerated = False
            blnIsYMCALetterGenerated = False
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("AutoClosedLoans_sort"), e)
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(mConst_GridViewAutoClosedLoansIndexOfPersonPrintlettersId).Text.ToString().Trim <> "0" Then 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                    lnkReport = e.Row.FindControl("lnkParticipantReport")
                    If lnkReport IsNot Nothing Then
                        'Start - Manthan Rajguru | 2016.07.21 | YRS-AT-3050 | Corrected mispelled word in link text
                        'lnkReport.Text = "View Particpant Letter"
                        lnkReport.Text = "View Participant Letter"
                        'End - Manthan Rajguru | 2016.07.21 | YRS-AT-3050 | Corrected mispelled word in link text
                    End If
                    blnIsParticipantLetterGenerated = True
                End If
                If e.Row.Cells(mConst_GridViewAutoClosedLoansIndexOfYMCAPrintlettersId).Text.ToString().Trim <> "0" AndAlso e.Row.Cells(mConst_GridViewAutoClosedLoansIndexOfYMCAPrintlettersId).Text.ToString().Trim <> "-1" Then 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                    lnkReport = e.Row.FindControl("lnkYMCAReport")
                    If lnkReport IsNot Nothing Then
                        lnkReport.Text = "View YMCA Letter"
                    End If
                    blnIsYMCALetterGenerated = True
                    ' To hide ymca letter generating leter because participant is not active in the ymca from where he took loan
                ElseIf e.Row.Cells(mConst_GridViewAutoClosedLoansIndexOfYMCAPrintlettersId).Text.ToString().Trim = "-1" Then 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                    lnkReport = e.Row.FindControl("lnkYMCAReport")
                    If lnkReport IsNot Nothing Then
                        lnkReport.Visible = False
                    End If
                End If
                If blnIsParticipantLetterGenerated And blnIsYMCALetterGenerated Then
                    e.Row.CssClass = "BG_Colour_Loan_Report"
                ElseIf blnIsParticipantLetterGenerated Then
                    e.Row.CssClass = "BG_Colour_Loan_Participant_Report"
                ElseIf blnIsYMCALetterGenerated Then
                    e.Row.CssClass = "BG_Colour_Loan_YMCA_Report"
                End If

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvAutoClosedLoans_RowDataBound", ex)
        End Try
    End Sub
    'End: AA:06.29.2016 YRS_AT-2830 Added to bound columns data values of auto closed loans grid

#End Region

#Region "Row Command Events"

    Private Sub gvOffSetDefaultAgeing_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvOffSetDefaultAgeing.RowCommand
        Session("Seamless_Fund") = e.CommandArgument.ToString.Trim
        If e.CommandName = "LoanMaintenance" Then
            Session("Seamless_From") = "loanoptions"
            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=LoanOptions", False)
        ElseIf e.CommandName = "FundId" Then
            Session("Seamless_From") = "personmaintenance"
            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)
        End If
    End Sub

    Private Sub gvDefaultedLoans_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDefaultedLoans.RowCommand
        Session("Seamless_Fund") = e.CommandArgument.ToString.Trim
        If e.CommandName = "LoanMaintenance" Then
            Session("Seamless_From") = "loanoptions"
            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=LoanOptions", False)
        ElseIf e.CommandName = "FundId" Then
            Session("Seamless_From") = "personmaintenance"
            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)
        End If
    End Sub

    Private Sub gvUnfreeze_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvUnfreeze.RowCommand
        If e.CommandName = "unfreeze" Then
            Dim lnkFundId As LinkButton
            Dim intIndex As Integer = 0
            Dim gvRow As GridViewRow
            Dim strMessage As String
            gvRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
            intIndex = gvRow.RowIndex
            lnkFundId = DirectCast(gvUnfreeze.Rows(intIndex).FindControl("lnkFundIdNo"), LinkButton)
            Session("strFundId") = lnkFundId.Text
            strMessage = MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_CONFIRM_UNFREEZED)
            strMessage = strMessage.Replace("$$FUNDNO$$", Session("strFundId"))
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "UNFREEZE", "ShowConfirmDialog('" + e.CommandArgument + "','" + strMessage + "','" + Session("strFundId") + "');", True)
        ElseIf e.CommandName = "FundId" Then
            Session("Seamless_From") = "personmaintenance"
            Session("Seamless_Fund") = e.CommandArgument.ToString.Trim
            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)
        End If
    End Sub

    'Start: AA:12.10.2015 Added to open report on report button click
    Private Sub gvOffsetLoans_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvOffsetLoans.RowCommand
        Try
            If e.CommandName = "Report" Then
                Dim lnkFundId As LinkButton
                Dim intIndex As Integer = 0
                Dim gvRow As GridViewRow
                Dim strFundidNo, strDefaultDate, strPersid, strReportName, strLoanRequestId, strMessage As String
                gvRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                intIndex = gvRow.RowIndex
                lnkFundId = DirectCast(gvOffsetLoans.Rows(intIndex).FindControl("lnkFundIdNo"), LinkButton)
                Session("FundNo") = lnkFundId.Text

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'Session("DefaultDate") = gvOffsetLoans.Rows(intIndex).Cells(9).Text
                Session("DefaultDate") = gvOffsetLoans.Rows(intIndex).Cells(mConst_DataGridLoanOffsetIndexOfDefaultDate).Text
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                strReportName = "LoanDefaultLetter" 'AA:07.01.2016 YRS-AT-2830 Added to pass report name as a parameter and to set to a session variable to report viewer

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If gvOffsetLoans.Rows(intIndex).Cells(11).Text.Trim = "0" Then
                '    strPersid = gvOffsetLoans.Rows(intIndex).Cells(1).Text
                '    strLoanRequestId = gvOffsetLoans.Rows(intIndex).Cells(2).Text
                If gvOffsetLoans.Rows(intIndex).Cells(mConst_DataGridLoanOffsetIndexOfPrintLettersID).Text.Trim = "0" Then
                    strPersid = gvOffsetLoans.Rows(intIndex).Cells(mConst_DataGridLoanOffsetIndexOfPersID).Text
                    strLoanRequestId = gvOffsetLoans.Rows(intIndex).Cells(mConst_DataGridLoanOffsetIndexOfLoanRequestID).Text
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                    Session("strLoanRequestId") = strLoanRequestId
                    Session("strPersid") = strPersid
                    Session("strDocCode") = "LOANAUOF" 'AA:04.18.2016 YRS-AT-2831 Added to handle the print report method for the both auto offset and auto defaulted loans
                    strMessage = MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_CONFIRM_PRINT_REPORT)
                    strMessage = strMessage.Replace("$$FUNDNO$$", Session("FundNo"))
                    'Start AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                    'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReportConfirm", "ShowReportConfirmDialog('" + strMessage + "');", True)
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReportConfirm", String.Format("ShowReportConfirmDialog('{0}','{1}')", strMessage, strReportName), True)
                    'End AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                Else
                    'Start AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                    'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReport", "OpenReportViewer();", True)
                    Session("strReportName") = strReportName
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReport", String.Format("OpenReportViewer('{0}')", strReportName), True)
                    'ENd AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                End If
            ElseIf e.CommandName = "FundId" Then
                Session("Seamless_From") = "personmaintenance"
                Session("Seamless_Fund") = e.CommandArgument.ToString.Trim
                Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvOffsetLoans_RowCommand", ex)
        End Try
    End Sub
    'End: AA:12.10.2015 Added to open report on report button click

    'Start: AA:04.18.2016 YRS-AT-2831:Added to open report on report button click
    Private Sub gvAutoDefaultLoans_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAutoDefaultLoans.RowCommand
        Try
            If e.CommandName = "Report" Then
                Dim lnkFundId As LinkButton
                Dim intIndex As Integer = 0
                Dim gvRow As GridViewRow
                Dim strFundidNo, strDefaultDate, strPersid, strReportName, strLoanRequestId, strMessage As String
                gvRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                intIndex = gvRow.RowIndex
                lnkFundId = DirectCast(gvAutoDefaultLoans.Rows(intIndex).FindControl("lnkFundIdNo"), LinkButton)
                Session("FundNo") = lnkFundId.Text

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'Session("DefaultDate") = gvAutoDefaultLoans.Rows(intIndex).Cells(5).Text
                Session("DefaultDate") = gvAutoDefaultLoans.Rows(intIndex).Cells(mConst_DataGridLoanAutoDefaltIndexOfDefaultDate).Text
                'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                strReportName = "LoanDefaultLetter" 'AA:07.01.2016 YRS-AT-2830 Added to pass report name as a parameter and to set to a session variable to report viewer

                'START: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable
                'If gvAutoDefaultLoans.Rows(intIndex).Cells(7).Text.Trim = "0" Then
                '    strPersid = gvAutoDefaultLoans.Rows(intIndex).Cells(1).Text
                '    strLoanRequestId = gvAutoDefaultLoans.Rows(intIndex).Cells(2).Text
                If gvAutoDefaultLoans.Rows(intIndex).Cells(mConst_DataGridLoanAutoDefaltIndexOfDPrinterLettersID).Text.Trim = "0" Then
                    strPersid = gvAutoDefaultLoans.Rows(intIndex).Cells(mConst_DataGridLoanAutoDefaltIndexOfPersID).Text
                    strLoanRequestId = gvAutoDefaultLoans.Rows(intIndex).Cells(mConst_DataGridLoanAutoDefaltIndexOfRequestID).Text
                    'END: SB | 2018.04.03 | YRS-AT-3101 | Hardcoded Index values are replaced with variable

                    Session("strLoanRequestId") = strLoanRequestId
                    Session("strPersid") = strPersid
                    Session("strDocCode") = "LOANDFEM"
                    strMessage = MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_CONFIRM_PRINT_REPORT)
                    strMessage = strMessage.Replace("$$FUNDNO$$", Session("FundNo"))
                    'Start AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                    'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReportConfirm", "ShowReportConfirmDialog('" + strMessage + "');", True)
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReportConfirm", String.Format("ShowReportConfirmDialog('{0}','{1}')", strMessage, strReportName), True)
                    'End AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                Else
                    'Start AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                    'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReport", "OpenReportViewer();", True)
                    Session("strReportName") = strReportName
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReport", String.Format("OpenReportViewer('{0}')", strReportName), True)
                    'ENd AA:07.01.2016 YRS-AT-2830 Changed below code to pass report name as a paramter
                End If
            ElseIf e.CommandName = "FundId" Then
                Session("Seamless_From") = "personmaintenance"
                Session("Seamless_Fund") = e.CommandArgument.ToString.Trim
                Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvAutoDefaultLoans_RowCommand", ex)
        End Try
    End Sub
    'End: AA:04.18.2016 YRS-AT-2831:Added to open report on report button click

    'Start: AA:06.28.2016 YRS-AT-2830:Added to open report on click of the links
    Private Sub gvAutoClosedLoans_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAutoClosedLoans.RowCommand
        Dim lnkFundId As LinkButton
        Dim intIndex As Integer
        Dim gvRow As GridViewRow
        Dim strFundidNo, strPersId, strYMCAId, strReportName, strLoanNumber, strLoanRequestId, strMessage As String
        Try
            If e.CommandName.Contains("Report") Then
                intIndex = 0
                gvRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)
                intIndex = gvRow.RowIndex
                lnkFundId = DirectCast(gvAutoClosedLoans.Rows(intIndex).FindControl("lnkFundIdNo"), LinkButton)
                Session("FundNo") = lnkFundId.Text
                'START: 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                strPersId = gvAutoClosedLoans.Rows(intIndex).Cells(mConst_GridViewAutoClosedLoansIndexOfPersId).Text
                strLoanRequestId = gvAutoClosedLoans.Rows(intIndex).Cells(mConst_GridViewAutoClosedLoansIndexOfLoanRequestId).Text
                'END: 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                Session("strPersid") = strPersId
                Session("perseID") = strPersId
                Session("strLoanRequestId") = strLoanRequestId
                If e.CommandName.Contains("ReportPartipant") Then
                    strReportName = "Loan Satisfied-part"
                    If gvAutoClosedLoans.Rows(intIndex).Cells(mConst_GridViewAutoClosedLoansIndexOfPersonPrintlettersId).Text.Trim = "0" Then 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                        Session("strDocCode") = "LNLETTR5"
                        strMessage = MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_CONFIRM_PRINT_REPORT)
                        strMessage = strMessage.Replace("$$FUNDNO$$", Session("FundNo"))
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReportConfirm", String.Format("ShowReportConfirmDialog('{0}','{1}')", strMessage, strReportName), True)
                    Else
                        Session("strReportName") = strReportName
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReport", String.Format("OpenReportViewer('{0}')", strReportName), True)
                    End If
                    strLoanNumber = gvAutoClosedLoans.Rows(intIndex).Cells(mConst_GridViewAutoClosedLoansIndexOfLoanNumber).Text 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                    Session("OrigLoanNo") = strLoanNumber
                ElseIf e.CommandName.Contains("ReportYMCA") Then
                    strReportName = "Loan Satisfied-PA"
                    If gvAutoClosedLoans.Rows(intIndex).Cells(mConst_GridViewAutoClosedLoansIndexOfYMCAPrintlettersId).Text.Trim = "0" Then 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                        Session("strDocCode") = "LNLETTR6"
                        strMessage = MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_CONFIRM_PRINT_REPORT)
                        strMessage = strMessage.Replace("$$FUNDNO$$", Session("FundNo"))
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReportConfirm", String.Format("ShowReportConfirmDialog('{0}','{1}')", strMessage, strReportName), True)
                    Else
                        Session("strReportName_1") = strReportName
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenReport", String.Format("OpenReportViewer('{0}')", strReportName), True)
                    End If
                    strYMCAId = gvAutoClosedLoans.Rows(intIndex).Cells(mConst_GridViewAutoClosedLoansIndexOfYmcaId).Text 'MMR | 2019.02.12  | YRS-AT-4330 | Accessing cell index using named index instead of cell index number
                    Session("FormYMCAId") = strYMCAId
                End If
            ElseIf e.CommandName = "FundId" Then
                Session("Seamless_From") = "personmaintenance"
                Session("Seamless_Fund") = e.CommandArgument.ToString.Trim
                Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvAutoClosedLoans_RowCommand", ex)
        End Try
    End Sub
    'End: AA:06.28.2016 YRS-AT-2830:Added to open report on click of the links

#End Region

#Region "Sorting Events"

    Private Sub gvOffSetDefaultAgeing_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvOffSetDefaultAgeing.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = DirectCast(ViewState("Offset_Default_Ageing_dtLoanList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If ViewState("Offset_Default_Ageing_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("Offset_Default_Ageing_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("Offset_Default_Ageing_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvOffSetDefaultAgeing, dv, True)
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("Offset_Default_Ageing_sorting", ex)
        End Try
    End Sub

    Private Sub gvDefaultedLoans_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvDefaultedLoans.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = DirectCast(ViewState("DefaultedLoans_dtLoanList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If ViewState("DefaultedLoans_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("DefaultedLoans_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("DefaultedLoans_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvDefaultedLoans, dv, True)
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvDefaultedLoans_Sorting", ex)
        End Try
    End Sub

    Private Sub gvUnfreeze_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvUnfreeze.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = DirectCast(ViewState("Unfreeze_Phantom_Int_dtLoanList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If ViewState("Unfreeze_Phantom_Int_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("Unfreeze_Phantom_Int_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("Unfreeze_Phantom_Int_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvUnfreeze, dv, True)
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("Unfreeze_Phantom_Int_sorting", ex)
        End Try
    End Sub

    'Start: AA:12.10.2015 Added to sort the offset grid records
    Private Sub gvOffsetLoans_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvOffsetLoans.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression
            dtRecords = DirectCast(ViewState("OffsetLoans_dtLoanList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = SortExpression
                If ViewState("OffsetLoans_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("OffsetLoans_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("OffsetLoans_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvOffsetLoans, dv, True)
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvOffsetLoans_Sorting", ex)
        End Try
    End Sub
    'End: AA:12.10.2015 Added to sort the offset grid records
    'Start: AA:04.18.2016 YRS-AT-2831:Added to sort the auto defaulted grid records
    Private Sub gvAutoDefaultLoans_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAutoDefaultLoans.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Dim strSortExpression As String
        Try
            strSortExpression = e.SortExpression
            dtRecords = DirectCast(ViewState("AutoDefaultLoans_dtLoanList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = strSortExpression
                If ViewState("AutoDefaultLoans_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("AutoDefaultLoans_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("AutoDefaultLoans_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvAutoDefaultLoans, dv, True)
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvAutoDefaultLoans_Sorting", ex)
        End Try
    End Sub
    'ENd: AA:04.18.2016 YRS-AT-2831:Added to sort the auto defaulted grid records

    'Start: AA:06.29.2016 YRS-AT-2830:Added to sort the auto closed loans grid records
    Private Sub gvAutoClosedLoans_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAutoClosedLoans.Sorting
        Dim dv As New DataView
        Dim dtRecords As DataTable
        Dim strSortExpression As String
        Try
            strSortExpression = e.SortExpression
            dtRecords = DirectCast(ViewState("AutoClosedLoans_dtLoanList"), DataTable)
            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView
                dv.Sort = strSortExpression
                If ViewState("AutoClosedLoans_Filter") IsNot Nothing Then
                    dv.RowFilter = ViewState("AutoClosedLoans_Filter")
                End If
                HelperFunctions.gvSorting(ViewState("AutoClosedLoans_sort"), e.SortExpression, dv)
                HelperFunctions.BindGrid(gvAutoClosedLoans, dv, True)
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvAutoClosedLoans_Sorting", ex)
        End Try
    End Sub
    'End: AA:06.29.2016 YRS-AT-2830:Added to sort the auto closed loans grid records

#End Region

#End Region

#End Region

#Region "Methods And Functions"

    Private Sub SearchRecords()
        Dim dv As New DataView
        Dim dtRecords As New DataTable
        Dim Sort As GridViewCustomSort
        Dim strFilter As String = String.Empty
        Dim intMaxCurePeriodCutOffDays As Integer
        Try
            If lblDefaultedLoans.Visible Then
                dtRecords = DirectCast(ViewState("DefaultedLoans_dtLoanList"), DataTable)
            ElseIf lblOffset_Default_Ageing.Visible Then
                dtRecords = DirectCast(ViewState("Offset_Default_Ageing_dtLoanList"), DataTable)
            ElseIf lblUnfreeze_Phantom_Int.Visible Then
                dtRecords = DirectCast(ViewState("Unfreeze_Phantom_Int_dtLoanList"), DataTable)
            ElseIf lblOffsetLoans.Visible Then 'AA:04.18.2016 YRS-AT-2831 Added for the serach records based on tab selection
                dtRecords = DirectCast(ViewState("OffsetLoans_dtLoanList"), DataTable) 'AA:12.10.2015 Added to search records in offset loans grid based on fund no.
            ElseIf lblAutoDefaultLoans.Visible Then 'AA:06.28.2016 YRS-AT-2830 Added to search records in auto defaulted loans grid based on fund no.
                dtRecords = DirectCast(ViewState("AutoDefaultLoans_dtLoanList"), DataTable) 'AA:04.18.2015 YRS-AT-2831 Added to search records in auto defaulted loans grid based on fund no.
                'Start:AA:06.28.2016 YRS-AT-2830 Added to search records in auto closed loans grid based on fund no.
            ElseIf lblAutoClosedLoans.Visible Then
                dtRecords = DirectCast(ViewState("AutoClosedLoans_dtLoanList"), DataTable)
                'End:AA:06.28.2016 YRS-AT-2830 Added to search records in auto closed loans grid based on fund no.
            End If

            If HelperFunctions.isNonEmpty(dtRecords) Then
                dv = dtRecords.DefaultView

                If Not String.IsNullOrEmpty(txtFundId.Text.Trim()) Then
                    strFilter = "FundIdNo = " + txtFundId.Text.Trim()
                End If
                'Start:AA:09.22.2015 YRS AT-2609 Added below lines to filter the records as per the checkbox select and unselect
                If lblOffset_Default_Ageing.Visible AndAlso (chkDefaultEligible.Checked OrElse chkOffsetEligible.Checked) Then
                    intMaxCurePeriodCutOffDays = ViewState("Max_Cure_Period_CutOff_Days")

                    If chkDefaultEligible.Checked AndAlso chkOffsetEligible.Checked Then
                        If Not String.IsNullOrEmpty(strFilter.Trim()) Then
                            strFilter += " And " + "DaysOverdue > " + intMaxCurePeriodCutOffDays.ToString()
                        Else
                            strFilter = "DaysOverdue > " + intMaxCurePeriodCutOffDays.ToString()
                        End If
                    ElseIf chkDefaultEligible.Checked Then
                        If Not String.IsNullOrEmpty(strFilter.Trim()) Then
                            strFilter += " And " + "(DaysOverdue > " + intMaxCurePeriodCutOffDays.ToString() + " AND IsOffsetEligible = False)"
                        Else
                            strFilter = "DaysOverdue > " + intMaxCurePeriodCutOffDays.ToString() + " AND IsOffsetEligible = False"
                        End If
                    ElseIf chkOffsetEligible.Checked Then
                        If Not String.IsNullOrEmpty(strFilter.Trim()) Then
                            strFilter += " And " + "(DaysOverdue > " + intMaxCurePeriodCutOffDays.ToString() + " AND IsOffsetEligible = True)"
                        Else
                            strFilter = "DaysOverdue > " + intMaxCurePeriodCutOffDays.ToString() + " AND IsOffsetEligible = True"
                        End If
                    End If
                ElseIf lblDefaultedLoans.Visible AndAlso chkOffsetEligible.Checked Then
                    If Not String.IsNullOrEmpty(strFilter.Trim()) Then
                        strFilter += " And " + "MessageNo > 0"
                    Else
                        strFilter = "MessageNo > 0"
                    End If
                End If

                If Not String.IsNullOrEmpty(strFilter.Trim()) Then
                    dv.RowFilter = strFilter
                End If
                'End:AA:09.22.2015 YRS AT-2609 Added below lines to filter the records as per the checkbox select and unselect
                If lblDefaultedLoans.Visible Then
                    ViewState("DefaultedLoans_Filter") = strFilter
                    If ViewState("DefaultedLoans_sort") IsNot Nothing Then
                        Sort = ViewState("DefaultedLoans_sort")
                        dv.Sort = Sort.SortExpression + " " + Sort.SortDirection
                    End If
                    HelperFunctions.BindGrid(gvDefaultedLoans, dv, True)
                ElseIf lblOffset_Default_Ageing.Visible Then
                    ViewState("Offset_Default_Ageing_Filter") = strFilter
                    If ViewState("Offset_Default_Ageing_sort") IsNot Nothing Then
                        Sort = ViewState("Offset_Default_Ageing_sort")
                        dv.Sort = Sort.SortExpression + " " + Sort.SortDirection
                    End If
                    HelperFunctions.BindGrid(gvOffSetDefaultAgeing, dv, True)
                ElseIf lblUnfreeze_Phantom_Int.Visible Then
                    ViewState("Unfreeze_Phantom_Int_Filter") = strFilter
                    If ViewState("Unfreeze_Phantom_Int_sort") IsNot Nothing Then
                        Sort = ViewState("Unfreeze_Phantom_Int_sort")
                        dv.Sort = Sort.SortExpression + " " + Sort.SortDirection
                    End If
                    HelperFunctions.BindGrid(gvUnfreeze, dv, True)
                    'Start: AA:12.10.2015 Added to sort the offset grid records
                ElseIf lblOffsetLoans.Visible Then
                    ViewState("OffsetLoans_Filter") = strFilter
                    If ViewState("OffsetLoans_sort") IsNot Nothing Then
                        Sort = ViewState("OffsetLoans_sort")
                        dv.Sort = Sort.SortExpression + " " + Sort.SortDirection
                    End If
                    HelperFunctions.BindGrid(gvOffsetLoans, dv, True)
                    'End: AA:12.10.2015 Added to sort the offset grid records
                    'Start: AA:05.02.2016 YRS-AT-2831  Added to sort the Auto defaulted grid records
                ElseIf lblAutoDefaultLoans.Visible Then
                    ViewState("AutoDefaultLoans_Filter") = strFilter
                    If ViewState("AutoDefaultLoans_sort") IsNot Nothing Then
                        Sort = ViewState("AutoDefaultLoans_sort")
                        dv.Sort = String.Format("{0} {1}", Sort.SortExpression, Sort.SortDirection)
                    End If
                    HelperFunctions.BindGrid(gvAutoDefaultLoans, dv, True)
                    'End: AA:05.02.2016 YRS-AT-2831  Added to sort the Auto defaulted grid records
                    'Start: AA:06.28.2016 YRS-AT-2830  Added to sort the Auto closed loans grid records
                ElseIf lblAutoClosedLoans.Visible Then
                    ViewState("AutoClosedLoans_Filter") = strFilter
                    If ViewState("AutoClosedLoans_Sort") IsNot Nothing Then
                        Sort = ViewState("AutoClosedLoans_Sort")
                        dv.Sort = String.Format("{0} {1}", Sort.SortExpression, Sort.SortDirection)
                    End If
                    HelperFunctions.BindGrid(gvAutoClosedLoans, dv, True)
                    'Start: AA:06.28.2016 YRS-AT-2830  Added to sort the Auto closed loans grid records
                End If
                LblCount.Text = dv.Count
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub BindGrid()
        Try
            Dim dsLoansDetailsList As DataSet
            Dim drFrozen As DataRow() = Nothing
            Dim drOffset As DataRow() = Nothing 'AA:12.10.2015 Added for offset loans tab link button highlight
            dsLoansDetailsList = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanDetailsForUtility()
            If dsLoansDetailsList IsNot Nothing Then
                Session("dsLoansDetailsList") = dsLoansDetailsList
                'Start:18.Jun.2015 B.Jagadeesh BT-2699 YRS 5.0-2441 Modifications for 403b Loans
                If dsLoansDetailsList.Tables.Count > 2 Then
                    ViewState("Loan_UnFreeze_Days") = dsLoansDetailsList.Tables(3).Select("chvKey = 'LOAN_UNFREEZE_DAYS'")(0).Item("chvValue").ToString()
                    ViewState("Max_Cure_Period_CutOff_Days") = dsLoansDetailsList.Tables(3).Select("chvKey = 'MAX_CURE_PERIOD_CUTOFF_DAYS'")(0).Item("chvValue").ToString()
                    ViewState("Loan_Offset_Age") = dsLoansDetailsList.Tables(3).Select("chvKey = 'LOAN_OFFSET_AGE'")(0).Item("chvValue").ToString()
                End If
                If dsLoansDetailsList.Tables.Count > 1 Then
                    If dsLoansDetailsList.Tables(2).Rows.Count > 0 Then
                        drFrozen = dsLoansDetailsList.Tables(2).Select("FrozenDays >" & ViewState("Loan_UnFreeze_Days").ToString())
                        If drFrozen.Length > 0 Then
                            tdUnfreeze.Style.Add("color", "red")
                            lnkUnfreeze_Phantom_Int.Style.Add("color", "red")
                            lblUnfreeze_Phantom_Int.Style.Add("color", "red")
                        End If
                    End If
                End If
                'Start: AA:12.10.2015 Added for offset loans tab link button highlight
                If dsLoansDetailsList.Tables.Count > 4 Then
                    If dsLoansDetailsList.Tables(4).Rows.Count > 0 Then
                        drOffset = dsLoansDetailsList.Tables(4).Select("PrintlettersId ='0'")
                        If drOffset.Length > 0 Then
                            lnkOffsetLoans.Style.Add("color", "red")
                            lblOffsetLoans.Style.Add("color", "red")
                        ElseIf drOffset.Length = 0 And IsPostBack Then
                            lnkOffsetLoans.Style.Remove("color")
                            lblOffsetLoans.Style.Remove("color")
                        End If
                    End If
                End If
                'ENd: AA:12.10.2015 Added for offset loans tab link button highlight

                'Start: AA:04.18.2016 YRS_AT-2831:Added for auto defaulted loans tab link button highlight
                If dsLoansDetailsList.Tables.Count > 5 Then
                    If dsLoansDetailsList.Tables(5).Rows.Count > 0 Then
                        drOffset = dsLoansDetailsList.Tables(5).Select("PrintlettersId ='0'")
                        If drOffset.Length > 0 Then
                            lnkAutoDefaultLoans.Style.Add("color", "red")
                            lblAutoDefaultLoans.Style.Add("color", "red")
                        ElseIf drOffset.Length = 0 And IsPostBack Then
                            lnkAutoDefaultLoans.Style.Remove("color")
                            lblAutoDefaultLoans.Style.Remove("color")
                        End If
                    End If
                End If
                'Start: AA:04.18.2016 YRS_AT-2831:Added for auto defaulted loans tab link button highlight

                'Start: AA:06.29.2016 YRS_AT-2830:Added for auto closed loans tab link button highlight
                If dsLoansDetailsList.Tables.Count > 6 Then
                    If dsLoansDetailsList.Tables(6).Rows.Count > 0 Then
                        drOffset = dsLoansDetailsList.Tables(6).Select("PersonPrintlettersId ='0' OR YMCAPrintlettersId = '0'")
                        If drOffset.Length > 0 Then
                            lnkAutoClosedLoans.Style.Add("color", "red")
                            lblAutoClosedLoans.Style.Add("color", "red")
                        ElseIf drOffset.Length = 0 And IsPostBack Then
                            lnkAutoClosedLoans.Style.Remove("color")
                            lblAutoClosedLoans.Style.Remove("color")
                        End If
                    End If
                End If
                'End: AA:06.29.2016 YRS-AT-2830:Added for auto closed loans tab link button highlight
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_BindGrid", ex)
        End Try
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Sub UnFreezeLoan(ByVal strLoanDetailsId As String)
        Dim intLoanDetailsId As Integer
        Try
            intLoanDetailsId = Convert.ToInt32(strLoanDetailsId)
            HttpContext.Current.Session("LoanUnFreezeSucess") = LoanClass.FreezeUnfreeze(intLoanDetailsId)
            Dim objLoanOffsetDefaultUnfreeze As New LoanOffsetDefaultUnfreeze
            objLoanOffsetDefaultUnfreeze.BindGrid()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_UnFreezeLoan", ex)
        End Try
    End Sub
    'Start:AA 09.23.2015 YRS AT-2609 Added to clear session variables
    Private Sub ClearSessions()
        Try
            dtOffset_Default_Ageing = Nothing
            dtDefaulted = Nothing
            dtUnfreeze = Nothing
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_ClearSessions", ex)
        End Try
    End Sub
    'End:AA 09.23.2015 YRS AT-2609 Added to clear session variables

    Private Function GetAgeFormat(ByVal strAge As String)
        Dim strSplitAge As String()
        Try
            strSplitAge = strAge.Split({"."}, StringSplitOptions.RemoveEmptyEntries)
            If strSplitAge.Length > 0 Then
                strAge = strSplitAge(0) + "Y / " + strSplitAge(1) + "M"
            End If
            Return strAge
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_ClearSessions", ex)
        End Try
    End Function
    'Start: AA:12.10.2015 Added for Copy report into IDM and reseting all controls of all tabs and displaying controls based on tab selection
    Private Function CopyIDM(ByVal paramstrReportname As String, ByVal paramstrlnletter As String, ByVal strPersonID As String, ByRef intIDMTrackingId As Integer, ByVal strRefID As String) As String 'AA:04.21.2016 YRS-AT-2831 added a new parameter strrefid to store refid in tracking log table
        'Mofifed By Ashutosh Patil as on 18-Apr-2007
        'For Loan Report
        Dim par_Arrylist As New ArrayList
        Dim IDMAll As New IDMforAll
        Dim strErrorMessage As String
        Try
            If Session("FTFileList") Is Nothing Then
                If IDMAll.DatatableFileList(False) Then
                    Session("FTFileList") = IDMAll.SetdtFileList
                End If
            End If
            If Not Session("FTFileList") Is Nothing Then
                IDMAll.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            IDMAll.PreviewReport = True
            IDMAll.LogonToDb = True
            IDMAll.CreatePDF = True
            IDMAll.CreateIDX = True
            IDMAll.CopyFilesToIDM = True
            IDMAll.PersId = strPersonID
            IDMAll.ReportName = paramstrReportname.ToString
            IDMAll.DocTypeCode = paramstrlnletter.ToString
            IDMAll.OutputFileType = paramstrlnletter.ToString
            'Start:AA:07.01.2016 YRS-AT-2830 Added below code to set idm paramteres based on the input report name
            If paramstrReportname.ToString = "Loan Satisfied-part.rpt" Then
                IDMAll.AppType = "P"
                par_Arrylist.Add(strPersonID.Trim)
                par_Arrylist.Add(HttpContext.Current.Session("OrigLoanNo"))
            ElseIf paramstrReportname.ToString = "Loan Satisfied-PA.rpt" Then
                IDMAll.AppType = "A"
                par_Arrylist.Add(strPersonID.Trim)
                IDMAll.YMCAID = HttpContext.Current.Session("FormYMCAId")
            ElseIf paramstrReportname.ToString = "LoanDefaultLetter.rpt" Then
                IDMAll.AppType = "P"
                par_Arrylist.Add(CType(Session("FundNo"), String).ToString.Trim)
                par_Arrylist.Add(Session("DefaultDate"))
            End If
            'End:AA:07.01.2016 YRS-AT-2830 Added below code to set idm paramteres based on the input report name
            IDMAll.ReportParameters = par_Arrylist
            IDMAll.RefRequestsID = strRefID 'AA:04.21.2016 YRS-AT-2831 Added to store refid in tracking log table
            strErrorMessage = IDMAll.ExportToPDF()

            If IDMAll.IDMTrackingId <> 0 Then
                intIDMTrackingId = IDMAll.IDMTrackingId
            End If
            Return strErrorMessage
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_" + "SetProperties", ex)
        End Try
    End Function

    Private Sub SetAllControls(ByRef blnFlag As Boolean)
        Try
            'links
            lnkOffset_Default_Ageing.Visible = Not blnFlag
            lnkUnfreeze_Phantom_Int.Visible = Not blnFlag
            lnkDefaultedLoans.Visible = Not blnFlag
            lnkOffsetLoans.Visible = Not blnFlag
            lnkAutoDefaultLoans.Visible = Not blnFlag 'AA:04.18.2016 YRS-AT-2831:Added to clear values
            lnkAutoClosedLoans.Visible = Not blnFlag 'AA:06.29.2016 YRS-AT-2830:Added to clear values for auto closed loans

            'labels
            lblDefaultedLoans.Visible = blnFlag
            lblUnfreeze_Phantom_Int.Visible = blnFlag
            lblOffset_Default_Ageing.Visible = blnFlag
            lblOffsetLoans.Visible = blnFlag
            lblAutoDefaultLoans.Visible = blnFlag 'AA:04.18.2016 YRS-AT-2831:Added to clear values
            lblAutoClosedLoans.Visible = blnFlag 'AA:06.29.2016 YRS-AT-2830:Added to clear values for auto closed loans
            'gridviews
            gvOffSetDefaultAgeing.Visible = blnFlag
            gvDefaultedLoans.Visible = blnFlag
            gvUnfreeze.Visible = blnFlag
            gvOffsetLoans.Visible = blnFlag
            gvAutoDefaultLoans.Visible = blnFlag 'AA:04.18.2016 YRS-AT-2831:Added to clear values
            gvAutoClosedLoans.Visible = blnFlag 'AA:06.29.2016 YRS-AT-2830:Added to clear values for auto closed loans

            'table rows
            trDefaultLoan.Visible = blnFlag
            trDefaultAgeing.Visible = blnFlag
            trUnFreeze.Visible = blnFlag
            trOffsetLoans.Visible = blnFlag
            trAutoDefaultLoans.Visible = blnFlag 'AA:04.18.2016 YRS-AT-2831:Added to clear values
            trAutoClosedLoans.Visible = blnFlag 'AA:06.29.2016 YRS-AT-2830:Added to clear values for auto closed loans

            'check box
            chkDefaultEligible.Visible = blnFlag
            chkOffsetEligible.Visible = blnFlag
            chkDefaultEligible.Checked = blnFlag
            chkOffsetEligible.Checked = blnFlag
            txtFundId.Text = String.Empty

            'Start: AA:04.18.2016 YRS-AT-2831:Added to clear grid view for easy transmission between tabs
            gvOffSetDefaultAgeing.DataSource = Nothing
            gvOffSetDefaultAgeing.DataBind()
            gvDefaultedLoans.DataSource = Nothing
            gvDefaultedLoans.DataBind()
            gvUnfreeze.DataSource = Nothing
            gvUnfreeze.DataBind()
            gvOffsetLoans.DataSource = Nothing
            gvOffsetLoans.DataBind()
            gvAutoDefaultLoans.DataSource = Nothing
            gvAutoDefaultLoans.DataBind()
            'End: AA:04.18.2016 YRS-AT-2831:Added to clear grid view for easy transmission between tabs
            'Start: AA:06.29.2016 YRS-AT-2830:Added to clear values for auto closed loans
            gvAutoClosedLoans.DataSource = Nothing
            gvAutoClosedLoans.DataBind()
            'ENd: AA:06.29.2016 YRS-AT-2830:Added to clear values for auto closed loans
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_" + "SetAllControls", ex)
        End Try
    End Sub

    Private Sub SetTabValues(ByRef enumTabSelected As LoanUtilityTab)
        Try
            If enumTabSelected = LoanUtilityTab.Aging Then
                lnkOffset_Default_Ageing.Visible = False
                lblOffset_Default_Ageing.Visible = True
                gvOffSetDefaultAgeing.Visible = True
                trDefaultAgeing.Visible = True
                chkDefaultEligible.Visible = True
                chkOffsetEligible.Visible = True
            ElseIf enumTabSelected = LoanUtilityTab.Default Then
                lnkDefaultedLoans.Visible = False
                lblDefaultedLoans.Visible = True
                gvDefaultedLoans.Visible = True
                trDefaultLoan.Visible = True
                chkOffsetEligible.Visible = True
            ElseIf enumTabSelected = LoanUtilityTab.Freeze Then
                lnkUnfreeze_Phantom_Int.Visible = False
                lblUnfreeze_Phantom_Int.Visible = True
                gvUnfreeze.Visible = True
                trUnFreeze.Visible = True
            ElseIf enumTabSelected = LoanUtilityTab.Offset Then
                lnkOffsetLoans.Visible = False
                lblOffsetLoans.Visible = True
                gvOffsetLoans.Visible = True
                trOffsetLoans.Visible = True
                'Start:AA:05.03.2016 YRS-AT-2831 Added to set tab values for auto defaulted tab
            ElseIf enumTabSelected = LoanUtilityTab.AutoDefault Then
                lnkAutoDefaultLoans.Visible = False
                lblAutoDefaultLoans.Visible = True
                gvAutoDefaultLoans.Visible = True
                trAutoDefaultLoans.Visible = True
                'End:AA:05.03.2016 YRS-AT-2831 Added to set tab values for auto defaulted tab
                'Start:AA:06.28.2016 YRS-AT-2830 Added to set tab values for auto closed tab
            ElseIf enumTabSelected = LoanUtilityTab.AutoClosed Then
                lnkAutoClosedLoans.Visible = False
                lblAutoClosedLoans.Visible = True
                gvAutoClosedLoans.Visible = True
                trAutoClosedLoans.Visible = True
                'End:AA:06.28.2016 YRS-AT-2830 Added to set tab values for auto closed tab
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_" + "SetTabValues", ex)
        End Try


    End Sub
    'Start AA:06.29.2016 YRS-AT-2830 Added paramater to pass report name from the links
    'Public Shared Sub PrintReport()
    <System.Web.Services.WebMethod()> _
    Public Shared Sub PrintReport(ByVal strReportName As String)
        'Dim strLoanRequestId, strPersid,strReportName, strErrorMessage As String
        Dim strLoanRequestId, strPersid, strErrorMessage As String
        'End AA:06.29.2016 YRS-AT-2830 Added paramater to pass report name from the links
        Dim strDocCode As String 'AA:05.03.2016 YRS-AT-2831 Added to copy file into idm dynamically based on the doccode 
        Dim objLoanOffsetDefaultUnfreeze As LoanOffsetDefaultUnfreeze
        Dim intPrintLetter, intIDMTrackingId As Integer
        Try
            'strReportName = "LoanDefaultLetter" 'AA:06.29.2016 YRS-AT-2830 Added paramater to pass report name from the links
            strDocCode = HttpContext.Current.Session("strDocCode") 'AA:05.03.2016 YRS-AT-2831 Added to copy file into idm dynamically based on the doccode 
            objLoanOffsetDefaultUnfreeze = New LoanOffsetDefaultUnfreeze()
            strLoanRequestId = HttpContext.Current.Session("strLoanRequestId")
            strPersid = HttpContext.Current.Session("strPersid")
            strErrorMessage = objLoanOffsetDefaultUnfreeze.CopyIDM(String.Format("{0}{1}", strReportName, ".rpt"), strDocCode, strPersid, intIDMTrackingId, strLoanRequestId) 'AA:05.03.2016 YRS-AT-2831 changed to copy file into idm dynamically based on the doccode 
            'Start AA:07.01.2016 Changed the if condition to pass into the condtional code. previously it was skipping the below code
            'If strErrorMessage IsNot String.Empty Then
            If String.IsNullOrEmpty(strErrorMessage) Then
                'End AA:07.01.2016 Changed the if condition to pass into the condtional code. previously it was skipping the below code
                intPrintLetter = YMCARET.YmcaBusinessObject.AnnuityBeneficiaryDeathBOClass.InsertPrintLetters(strLoanRequestId, strPersid, strDocCode)  'AA:05.03.2016 YRS-AT-2831 changed to copy file into idm dynamically based on the doccode
                If (intPrintLetter <> Nothing AndAlso intPrintLetter <> 0) AndAlso (intIDMTrackingId <> Nothing AndAlso intIDMTrackingId <> 0) Then
                    YMCARET.YmcaBusinessObject.AnnuityBeneficiaryDeathBOClass.UpdatePrintLetters(intPrintLetter, intIDMTrackingId)
                End If
                HttpContext.Current.Session("LoanPrintReport") = "ReportAndIDM"
            End If

            'Start AA:07.01.2016 Added below code to set session variable based on the report name.
            If strReportName = "Loan Satisfied-PA" Then
                HttpContext.Current.Session("strReportName_1") = strReportName
            Else
                HttpContext.Current.Session("strReportName") = strReportName
            End If
            'End AA:07.01.2016 Added below code to set session variable based on the report name.

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_PrintReport", ex)
        End Try
    End Sub
    <System.Web.Services.WebMethod()> _
    Public Shared Sub DontPrintReport()
        Try
            HttpContext.Current.Session("strLoanRequestId") = Nothing
            HttpContext.Current.Session("strPersid") = Nothing
            'Start:AA:07.01.2016 YRS-AT-2830 Added below code to Clear sessions which are not needed after the printing report
            HttpContext.Current.Session("strDocCode") = Nothing
            HttpContext.Current.Session("perseID") = Nothing
            HttpContext.Current.Session("OrigLoanNo") = Nothing
            HttpContext.Current.Session("FormYMCAId") = Nothing
            'End:AA:07.01.2016 YRS-AT-2830 Added below code to Clear sessions which are not needed after the printing report
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOffsetDefaultUnfreeze_PrintReport", ex)
        End Try
    End Sub

    'End: AA:12.10.2015 Added for Copy report into IDM and reseting all controls of all tabs and displaying controls based on tab selection
#End Region



End Class