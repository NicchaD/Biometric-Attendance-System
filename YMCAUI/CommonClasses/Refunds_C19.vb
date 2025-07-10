'/**************************************************************************************************************/
'// Copyright YMCA Retirement Fund All Rights Reserved. 
'//
'// Author: Manthan Rajguru
'// Created on: 04/09/2020
'// Summary of Functionality: New class for COVID19 withdrawal request
'// Declared in Version: 20.8.1.2 | YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688) 
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'//  Megha Lad	                | 17.04.2020   |	20.8.1.2     | YRS-AT-4874 -  COVID - Special withdrawal functionality needed in YRS processing screen due to CARE Act/COVID-19 (Track IT - 41688)
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/


Imports System.Web
Imports System.IO

Public Class Refunds_C19
    Inherits System.Web.UI.Page

#Region "global declarations"
    'START - Retirement plan account groups
    Const m_const_RetirementPlan_AP As String = "RAP"
    Const m_const_RetirementPlan_AM As String = "RAM"
    Const m_const_RetirementPlan_RG As String = "RRG"
    Const m_const_RetirementPlan_SA As String = "RSA"
    Const m_const_RetirementPlan_SS As String = "RSS"
    Const m_const_RetirementPlan_RP As String = "RRP"
    Const m_const_RetirementPlan_SR As String = "RSR"
    'Begin Code Merge by Dilip on 07-05-2009
    'Priya 13-Jan-2009 : YRS 5.0-637 AC Account interest components added new constant for AC account type group.
    Const m_const_RetirementPlan_AC As String = "RAC"
    'End Code Merge by Dilip on 07-05-2009
    'Phase V YMCA Legacy acct & YMCA Acct const variable  by Dilip
    Const m_const_YMCA_Legacy_Acct As String = "YMCA (Legacy) Acct"
    Const m_const_YMCA__Acct As String = "YMCA Acct"
    'Phase V YMCA Legacy acct & YMCA Acct const variable by Dilip
    'Added Ganeswar 10thApril09 for BA Account Phase-V /Begin
    Const m_const_RetirementPlan_BA As String = "RBA"
    'Added Ganeswar 10thApril09 for BA Account Phase-V /End
    'END - Retirement plan account groups
    'START - Ssvings plan account groups
    Const m_const_SavingsPlan_TD As String = "STD"
    Const m_const_SavingsPlan_TM As String = "STM"
    Const m_const_SavingsPlan_RT As String = "SRT"
    'END - Savings plan account groups
    Dim m_DataTableContributionDataTable As DataTable
    Dim m_DataTableWithholdingDeductions As DataTable
    Dim m_DataTableDisplayConsolidatedTotal As DataTable
    Dim m_decimal_VolWithdrawalTotal As Decimal = "0.00"
    Dim m_decimal_HardWithdrawalTotal As Decimal = "0.00"

    Dim m_DataTableDisplayRetirementPlan As DataTable
    Dim m_DataTableDisplaySavingsPlan As DataTable
    'Added by Ganeswar for Partial refund
    Dim l_FinalrefundDataSet As New DataSet
    Dim blnAddNewColumn As Boolean = False
    Dim bool_RemoveRounding As Boolean = False
    'Added by Parveen On 16 Dec 2009 For Rollover Options
    Const m_const_PartialRollOver As String = "PARTIAL"
    Const m_const_AllTaxable As String = "ALLTAXABLE"
    'Added by Parveen On 16 Dec 2009 For Rollover Options
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
#End Region

    'Begin Code Merge by Dilip on 07-05-2009
#Region "General Utility Functions"
    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function
    Private Function isNonEmpty(ByRef obj As Object) As Boolean
        If obj Is Nothing Then Return False
        If TypeOf (obj) Is DataSet Then
            Return isNonEmpty(DirectCast(obj, DataSet))
        End If
        If Convert.ToString(obj).Trim = String.Empty Then Return False
        Return True
    End Function
    Private Function isEmpty(ByRef ds As DataSet) As Boolean
        Return Not isNonEmpty(ds)
    End Function
    Private Property NumPercentageFactorofMoneyRetirement() As Decimal
        Get
            If Not Session("PercentageFactorofMoneyRetirement_C19") Is Nothing Then
                Return (DirectCast(Session("PercentageFactorofMoneyRetirement_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PercentageFactorofMoneyRetirement_C19") = Value
        End Set
    End Property

    Private Property NumPercentageFactorofMoneySavings() As Decimal
        Get
            If Not Session("PercentageFactorofMoneySavings_C19") Is Nothing Then
                Return (DirectCast(Session("PercentageFactorofMoneySavings_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PercentageFactorofMoneySavings_C19") = Value
        End Set
    End Property
    'START: MMR | 07/16/2019 | YRS-AT-4498 | Declared properties to hold hardship configuration values (Loan allowed, TD Contributions allowed & Interest component allowed)
    Public Property IsTDContributionsAllowed() As Boolean
        Get
            If Not Session("isTDContributionsAllowed_C19") Is Nothing Then
                Return (CType(Session("isTDContributionsAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("isTDContributionsAllowed_C19") = Value
        End Set
    End Property

    Public Property IsLoanCriteriaNotRequired() As Boolean
        Get
            If Not Session("isLoanCriteriaNotRequired_C19") Is Nothing Then
                Return (CType(Session("isLoanCriteriaNotRequired_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("isLoanCriteriaNotRequired_C19") = Value
        End Set
    End Property

    Public Property IsInterestAllowed() As Boolean
        Get
            If Not Session("isInterestAllowed_C19") Is Nothing Then
                Return (CType(Session("isInterestAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("isInterestAllowed_C19") = Value
        End Set
    End Property

    Public Property HardshipWithdrawalTaxable() As Decimal
        Get
            If Not Session("hardshipWithdrawalTaxable_C19") Is Nothing Then
                Return (CType(Session("hardshipWithdrawalTaxable_C19"), Decimal))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("hardshipWithdrawalTaxable_C19") = Value
        End Set
    End Property

    Public Property HardshipWithdrawalNonTaxable() As Decimal
        Get
            If Not Session("hardshipWithdrawalNonTaxable_C19") Is Nothing Then
                Return (CType(Session("hardshipWithdrawalNonTaxable_C19"), Decimal))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("hardshipWithdrawalNonTaxable_C19") = Value
        End Set
    End Property
    'END: MMR | 07/16/2019 | YRS-AT-4498 | Declared properties to hold hardship configuration values (Loan allowed, TD Contributions allowed & Interest component allowed)
#End Region
    'End Code Merge by Dilip on 07-05-2009

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Declared properties for Covid functionality
#Region "Covid Properties"
    Public Property CovidTaxRate() As Decimal
        Get
            If Not Session("CovidTaxRate") Is Nothing Then
                Return (CType(Session("CovidTaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CovidTaxRate") = Value
        End Set
    End Property

    Public Property ApplicableFederalTaxRate() As Decimal
        Get
            If Not Session("ApplicableFederalTaxRate") Is Nothing Then
                Return (CType(Session("ApplicableFederalTaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("ApplicableFederalTaxRate") = Value
        End Set
    End Property

    Public Property CovidTaxableAmount() As Decimal
        Get
            If Not Session("CovidTaxableBalance") Is Nothing Then
                Return (CType(Session("CovidTaxableBalance"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CovidTaxableBalance") = Value
        End Set
    End Property

    Public Property NonCovidTaxableAmount() As Decimal
        Get
            If Not Session("NonCovidTaxableBalance") Is Nothing Then
                Return (CType(Session("NonCovidTaxableBalance"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NonCovidTaxableBalance") = Value
        End Set
    End Property

    Public Property CovidNonTaxableAmount() As Decimal
        Get
            If Not Session("CovidNonTaxableAmount") Is Nothing Then
                Return (CType(Session("CovidNonTaxableAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CovidNonTaxableAmount") = Value
        End Set
    End Property

    Public Property NonCovidNonTaxableAmount() As Decimal
        Get
            If Not Session("NonCovidNonTaxableAmount") Is Nothing Then
                Return (CType(Session("NonCovidNonTaxableAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NonCovidNonTaxableAmount") = Value
        End Set
    End Property

    Public Property CovidAmountRemaining() As Decimal
        Get
            If Not Session("CovidAmountRemaining") Is Nothing Then
                Return (CType(Session("CovidAmountRemaining"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CovidAmountRemaining") = Value
        End Set
    End Property

    Public Property CovidAmountLimit() As Decimal
        Get
            If Not Session("CovidAmountLimit") Is Nothing Then
                Return (CType(Session("CovidAmountLimit"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CovidAmountLimit") = Value
        End Set
    End Property

    Public Property CovidAmountUsed() As Decimal
        Get
            If Not Session("CovidAmountUsed") Is Nothing Then
                Return (CType(Session("CovidAmountUsed"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CovidAmountUsed") = Value
        End Set
    End Property
#End Region
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Declared properties for Covid functionality


#Region " Refund Functions "

#Region "Full Or Pers Refund"


    '' This function for Full Refund.
    Public Function DoFullOrPersRefund(ByVal parameterCalledBy As String)

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_AccountGroup As String
        m_DataTableContributionDataTable = Nothing
        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_RetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_RetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_SavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_SavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTable_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                End If
            End If


            m_DataTableContributionDataTable = DoFlagSetForFullOrPersRefund(l_ContributionDataTable)


        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function DoFlagSetForFullOrPersRefund(ByVal l_ContributionDataTable As DataTable) As DataTable
        Dim l_DataRow As DataRow
        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_AccountGroup As String
        Me.YMCAAvailableAmount = 0.0
        m_DataTableContributionDataTable = Nothing
        Try

            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    'Reset the flag
                    l_UserSide = True
                    l_YMCASide = True

                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total") Then


                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then
                                l_UserSide = True

                                If Me.RefundType = "PERS" Then
                                    l_YMCASide = False
                                    'Priya 5-Jan-2008 YRS 5.0-634 : SHIRA refund request for non-vested participant 
                                    'added Or Me.RefundType = "SHIRA"
                                ElseIf Me.RefundType = "REG" Or Me.RefundType = "SHIRA" Then
                                    ' START | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money 

                                    If BA_Legacy_combined_Amt_Switch_ON() Then
                                        l_YMCASide = True
                                    Else
                                        If Me.SessionStatusType.Trim().ToUpper() = "QD" Then ' SR | 2017.07.03 | YRS-AT-3161 | For QD participants, there will be no termination date hence Termination PIA condition not applicable.
                                            l_YMCASide = True
                                        ElseIf Me.IsVested And Me.TerminationPIA < Me.MaximumPIAAmount Then
                                            l_YMCASide = True
                                        Else
                                            l_YMCASide = False
                                            'Modified by Shubhrata Feb26th 2007,YREN-3022
                                            Me.RefundType = "PERS"
                                            'Modified by Shubhrata Feb26th 2007,YREN-3022 
                                        End If

                                    End If
                                    ' END | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money
                                End If
                            End If

                            Select Case (l_AccountGroup.Trim.ToUpper)
                                'Start-RetirementPlanGroup
                                Case m_const_RetirementPlan_AM
                                    l_UserSide = True
                                    l_YMCASide = True
                                    'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                                Case m_const_RetirementPlan_BA
                                    l_UserSide = True
                                    l_YMCASide = True
                                    'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                                Case m_const_RetirementPlan_AP
                                    l_UserSide = True
                                    l_YMCASide = False

                                Case m_const_RetirementPlan_RP
                                    l_UserSide = True
                                    l_YMCASide = False

                                Case m_const_RetirementPlan_SR
                                    'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                                    'on the condition that participant is not active
                                    'If Me.IsVested And Me.TerminationPIA < Me.MaximumPIAAmount Then
                                    '    l_UserSide = True
                                    '    l_YMCASide = True
                                    'Else
                                    '    l_UserSide = False
                                    '    l_YMCASide = False
                                    'End If
                                    If Me.SessionStatusType.Trim.ToUpper <> "AE" Then
                                        l_UserSide = True
                                        l_YMCASide = True
                                    Else
                                        l_UserSide = False
                                        l_YMCASide = False
                                    End If
                                    'End -RetirementPlanGroup
                                    'Start- Savings Plan group
                                    'Begin Code Merge by Dilip on 07-05-2009
                                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new Case for AC account
                                Case m_const_RetirementPlan_AC
                                    l_UserSide = True
                                    l_YMCASide = False
                                    'End 14-Jan-2009
                                    'End Code Merge by Dilip on 07-05-2009
                                Case m_const_SavingsPlan_TD
                                    l_UserSide = True
                                    l_YMCASide = False
                                Case m_const_SavingsPlan_TM
                                    l_UserSide = True
                                    l_YMCASide = True
                                Case m_const_SavingsPlan_RT
                                    l_UserSide = True
                                    l_YMCASide = False
                                    'End - Savings Plan group
                            End Select

                            ''Modify the values

                            If l_UserSide Then

                                If l_YMCASide = False Then

                                    l_DataRow("YMCATaxable") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"
                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("YMCATotal"), Decimal)
                                    l_DataRow("YMCATotal") = "0.00"

                                End If

                                l_DataRow("Selected") = "True"

                                ' Me.YMCAAvailableAmount += DirectCast(l_DataRow("YMCATaxable"), Decimal)

                            Else
                                l_DataRow.Delete()
                            End If

                        End If
                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()
                DoFlagSetForFullOrPersRefund = l_ContributionDataTable

            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region


#Region " Voluntry Refund "

    Public Function DoVoluntryRefund(ByVal parameterCalledBy As String, ByVal parameterboollVoluntryAccounts As Boolean)


        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String

        'Me.YMCAAvailableAmount = 0.0
        'Me.VoluntryAmount = 0.0
        m_DataTableContributionDataTable = Nothing
        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_RetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_RetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_SavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_SavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTable_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                End If
            End If


            m_DataTableContributionDataTable = DoFlagSetForVoluntryRefund(l_ContributionDataTable, parameterboollVoluntryAccounts)


        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function DoFlagSetForVoluntryRefund(ByVal l_ContributionDataTable As DataTable, ByVal parameterboollVoluntryAccounts As Boolean) As DataTable

        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String

        'Me.YMCAAvailableAmount = 0.0
        Me.VoluntryAmountForProcessing = 0.0
        m_DataTableContributionDataTable = Nothing
        Try


            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    'Reset the flag
                    l_UserSide = True
                    l_YMCASide = True
                    l_Interest = True

                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        'AA:22.06.2015 BT-2699:YRS 5.0-2441 : Added not to include ln or ld for voluntary amount
                        If Not (((DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total")) Or (((DirectCast(l_DataRow("AccountGroup"), String).Trim = "")) Or (DirectCast(l_DataRow("AccountType"), String).Trim = "LD") Or (DirectCast(l_DataRow("AccountType"), String).Trim = "LN"))) Then


                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then
                                l_UserSide = False
                                l_YMCASide = False
                                l_Interest = False
                            End If

                            Select Case (l_AccountGroup.Trim.ToUpper)
                                'Start - Retirement Plan Group
                                Case m_const_RetirementPlan_AM
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True

                                    ' If (parameterboollVoluntryAccounts) And (Me.IsTerminated) Then
                                    If Me.IsTerminated Then
                                        l_UserSide = True
                                        l_YMCASide = True
                                        l_Interest = True
                                    End If

                                Case m_const_RetirementPlan_AP
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True



                                Case m_const_RetirementPlan_RP
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True



                                Case m_const_RetirementPlan_SR
                                    'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                                    'on the condition that participant is not active
                                    'l_UserSide = False
                                    'l_YMCASide = False
                                    'l_Interest = False
                                    If Me.SessionStatusType.Trim.ToUpper <> "AE" Then
                                        l_UserSide = True
                                        l_YMCASide = False
                                        l_Interest = True
                                        If Me.IsTerminated Then
                                            l_UserSide = True
                                            l_YMCASide = True
                                            l_Interest = True
                                        End If
                                    Else
                                        l_UserSide = False
                                        l_YMCASide = False
                                        l_Interest = False
                                    End If
                                    'Begin Code Merge by Dilip on 07-05-2009
                                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new case
                                Case m_const_RetirementPlan_AC
                                    If (Me.IsTerminated) Then

                                        l_UserSide = True
                                        l_YMCASide = False
                                    Else
                                        l_UserSide = False
                                        l_YMCASide = False
                                        l_Interest = False
                                    End If

                                    'end 14-Jan-2009
                                    'End Code Merge by Dilip on 07-05-2009
                                    'End - Retirement Plan Group
                                    'Start - Savings Plan Group
                                Case m_const_SavingsPlan_TD
                                    l_UserSide = False
                                    l_YMCASide = False
                                    l_Interest = False

                                    'If parameterboollVoluntryAccounts = True Then

                                    '    l_UserSide = False
                                    '    l_YMCASide = False
                                    '    l_Interest = False

                                    ' START | SR | 2019.08.09 | YRS-AT-YRS-AT-2169 | If person is terminated and fund status other than NP,PENP,RDNP then inlcude TD contributions
                                    'If Me.IsTerminated Or Me.PersonAge >= 59.06 Then
                                    If (Me.IsTerminated And Not IsNonParticipatingPerson()) Or Me.PersonAge >= 59.06 Then
                                        ' END | SR | 2019.08.09 | YRS-AT-YRS-AT-2169 | If person is terminated and fund status other than NP,PENP,RDNP then inlcude TD contributions
                                        l_UserSide = True
                                        l_YMCASide = False
                                        l_Interest = True
                                    End If

                                    'End If


                                Case m_const_SavingsPlan_TM
                                    l_UserSide = False
                                    l_YMCASide = False
                                    l_Interest = False

                                    'If parameterboollVoluntryAccounts = True Then

                                    '    l_UserSide = False
                                    '    l_YMCASide = False
                                    '    l_Interest = False

                                    If Me.IsTerminated Or Me.PersonAge >= 59.06 Then
                                        l_UserSide = True
                                        'VPR Phase V Part 1 Commented to load TM Matched 
                                        '    l_YMCASide = False
                                        l_YMCASide = True
                                        l_Interest = True
                                    End If

                                    'End If
                                Case m_const_SavingsPlan_RT
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True
                                    'End - Savings Plan Group
                            End Select
                            If Me.IsTerminated = False Then
                                l_YMCASide = False
                            End If

                            ''Modify the values

                            If l_UserSide Then

                                If l_YMCASide = False Then

                                    l_DataRow("YMCATaxable") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"
                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("YMCATotal"), Decimal)
                                    l_DataRow("YMCATotal") = "0.00"

                                End If

                                If l_Interest = False Then

                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                    l_DataRow("Emp.Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) - DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                    l_DataRow("YMCATotal") = DirectCast(l_DataRow("YMCATotal"), Decimal) - DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                    l_DataRow("Interest") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"

                                End If

                                l_DataRow("Selected") = "True"

                                Me.VoluntryAmountForProcessing += DirectCast(l_DataRow("Emp.Total"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                'Me.YMCAAvailableAmount += DirectCast(l_DataRow("YMCATaxable"), Decimal)

                            Else
                                l_DataRow.Delete()
                            End If

                        Else
                            'l_DataRow.Delete()
                        End If

                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()
                DoFlagSetForVoluntryRefund = l_ContributionDataTable

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region
#Region " Hardship Refund "

    Public Function DoHardshipRefund(ByVal parameterCalledBy As String) As Boolean

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow


        'Me.YMCAAvailableAmount = 0.0
        'Me.VoluntryAmount = 0.0
        m_DataTableContributionDataTable = Nothing
        If Me.IsTerminated Then
            '"Participant Must be Active in the Fund to Qualify for a Hardship Refund"

            Return False
        End If

        If Me.PersonAge >= 59.06 Then
            '"Participant Must be Younger than 59 1/2 to Qualify for a Hardship Refund"

            Return False
        End If

        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_RetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_RetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_SavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_SavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTable_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                End If
            End If

            m_DataTableContributionDataTable = DoFlagSetForHardshipRefund(l_ContributionDataTable, False)

            Return True
        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function DoHardshipRefundForDisplay(ByVal parameterCalledBy As String) As Boolean

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow


        'Me.YMCAAvailableAmount = 0.0
        'Me.VoluntryAmount = 0.0
        m_DataTableContributionDataTable = Nothing
        If Me.IsTerminated Then
            '"Participant Must be Active in the Fund to Qualify for a Hardship Refund"

            Return False
        End If

        If Me.PersonAge >= 59.06 Then
            '"Participant Must be Younger than 59 1/2 to Qualify for a Hardship Refund"

            Return False
        End If

        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                End If
            End If

            m_DataTableContributionDataTable = DoFlagSetForHardshipRefundForDisplay(l_ContributionDataTable, False)

            Return True
        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function DoFlagSetForHardshipRefund(ByVal l_ContributionDataTable As DataTable, ByVal NeedHardshipAvailAmt As Boolean) As DataTable
        Dim l_DataRow As DataRow
        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String
        'Me.YMCAAvailableAmount = 0.0
        'Me.VoluntryAmount = 0.0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "DoFlagSetForHardshipRefund() START") ' SR | 2019.07.24 | YRS-AT- 4498 - Log data for auditing.
            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    'Reset the flag
                    l_UserSide = True
                    l_YMCASide = False
                    l_Interest = True

                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total") Then


                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then
                                l_UserSide = False
                                l_YMCASide = False
                            End If

                            Select Case (l_AccountGroup.Trim.ToUpper)
                                'Start - Retirement Plan Group
                                Case m_const_RetirementPlan_AM
                                    l_UserSide = True
                                    l_YMCASide = False

                                Case m_const_RetirementPlan_AP
                                    l_UserSide = True
                                    l_YMCASide = False


                                Case m_const_RetirementPlan_RP
                                    l_UserSide = True
                                    l_YMCASide = False
                                    'Commented by Shubhrata Plan Split Changes as there is no existing acct type as "AS"    
                                    'Case "AS"
                                    '    l_UserSide = False
                                    '    l_Interest = False

                                Case m_const_RetirementPlan_SR

                                    'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                                    'on the condition that participant is not active
                                    'l_UserSide = False
                                    'l_Interest = False

                                    l_UserSide = False
                                    l_YMCASide = False

                                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new case
                                Case m_const_RetirementPlan_AC
                                    l_UserSide = False
                                    l_YMCASide = False
                                    'End 14-Jan-2009

                                    'End - Retirement Plan Group
                                    'Start - Savings Plan Group
                                Case m_const_SavingsPlan_TD
                                    l_UserSide = True
                                    l_YMCASide = False

                                    Session("HardshipAmountWithInterest_C19") = l_DataRow("Total")
                                    'START | 2019.07.30 |  SR | YRS-AT-4498 | Add interests & Non-Taxable to Available Hardship TD amount
                                    If IsInterestAllowed Then
                                        If NeedHardshipAvailAmt = True Then
                                            Me.HardshipAvailable += DirectCast(l_DataRow("Total"), Decimal)
                                            'START | 2019.07.30 |  SR | YRS-AT-3870 | Get Hardship Taxable & Nontaxable components.
                                            Me.HardshipWithdrawalTaxable += DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("Interest"), Decimal)
                                            Me.HardshipWithdrawalNonTaxable += DirectCast(l_DataRow("Non-Taxable"), Decimal)
                                            'END | 2019.07.30 |  SR | YRS-AT-3870 | Get Hardship Taxable & Nontaxable components.
                                        End If
                                    Else
                                        l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                        l_DataRow("Emp.Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                        l_DataRow("Interest") = "0.00"
                                        'Me.HardshipAvailable = DirectCast(l_DataRow("Total"), Decimal)
                                        If NeedHardshipAvailAmt = True Then
                                            ' START | 2019.07.23 | SR | YRS-AT-3870 | Include non-taxable portion also for hardship withdrawal
                                            'START | 2019.07.30 |  SR | YRS-AT-3870 | Get Hardship Taxable & Nontaxable components.
                                            Me.HardshipWithdrawalTaxable += DirectCast(l_DataRow("Taxable"), Decimal)
                                            Me.HardshipWithdrawalNonTaxable += DirectCast(l_DataRow("Non-Taxable"), Decimal)
                                            'END | 2019.07.30 |  SR | YRS-AT-3870 | Get Hardship Taxable & Nontaxable components.
                                            'Me.HardshipAvailable += DirectCast(l_DataRow("Taxable"), Decimal)
                                            Me.HardshipAvailable += DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("Non-Taxable"), Decimal)
                                            ' END | 2019.07.23 | SR | YRS-AT-3870 | Include non-taxable portion also for hardship withdrawal
                                        End If
                                    End If
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Taxable : {0}, Non-Taxable: {1}, Interest:{2}, HardshipAvailable: {3}, IsInterestAllowed : {4} ", l_DataRow("Taxable"), l_DataRow("Non-Taxable"), l_DataRow("Interest"), HardshipAvailable, IsInterestAllowed))
                                    'END | 2019.07.30 |  SR | YRS-AT-4498 | Add interests & Non-Taxable to Available Hardship TD amount.
                                Case m_const_SavingsPlan_TM
                                    l_UserSide = True
                                    l_YMCASide = False


                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Emp.Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Interest") = "0.00"
                                    If NeedHardshipAvailAmt = True Then
                                        Me.HardshipAvailable += DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("YMCATaxable"), Decimal)
                                    End If

                                Case m_const_SavingsPlan_RT
                                    l_UserSide = True
                                    l_YMCASide = False
                                    'Start - Savings Plan Group
                            End Select


                            ''Modify the values

                            If l_UserSide Then

                                If l_YMCASide = False Then

                                    l_DataRow("YMCATaxable") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"
                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("YMCATotal"), Decimal)
                                    l_DataRow("YMCATotal") = "0.00"

                                End If

                                If l_Interest = False Then


                                    l_DataRow("Emp.Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Interest") = "0.00"

                                End If
                                If Me.RefundState = "R" Then
                                    l_DataRow("Selected") = "True"
                                End If

                                'Me.YMCAAvailableAmount += DirectCast(l_DataRow("YMCATaxable"), Decimal)

                            Else
                                l_DataRow.Delete()
                            End If

                        End If
                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()
                DoFlagSetForHardshipRefund = l_ContributionDataTable

            End If
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DoFlagSetForHardshipRefund()", String.Format("Error : {0} ", ex.Message)) ' SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.
            Throw
            ' START | SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "DoFlagSetForHardshipRefund() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
        ' END | SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.
    End Function
    Public Function DoFlagSetForHardshipRefundForDisplay(ByVal l_ContributionDataTable As DataTable, ByVal NeedHardshipAvailAmt As Boolean) As DataTable
        Dim l_DataRow As DataRow
        Dim l_bool_SelectAccts As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String
        Me.YMCAAvailableAmount = 0.0
        Me.VoluntryAmount = 0.0
        Try

            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    'Reset the flag
                    l_bool_SelectAccts = True

                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total") Then


                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then
                                l_bool_SelectAccts = False
                                l_DataRow("Total") = "0.00"
                                l_DataRow("Taxable") = "0.00"
                                l_DataRow("Non-Taxable") = "0.00"
                                l_DataRow("Interest") = "0.00"

                            End If

                            Select Case (l_AccountGroup.Trim.ToUpper)
                                'Start - Retirement Plan Group


                                Case m_const_RetirementPlan_SR

                                    l_bool_SelectAccts = False
                                    l_DataRow("Total") = "0.00"
                                    l_DataRow("Taxable") = "0.00"
                                    l_DataRow("Non-Taxable") = "0.00"
                                    l_DataRow("Interest") = "0.00"
                                    'Begin Code Merge by Dilip on 07-05-2009
                                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new case for Ac Account Type
                                Case m_const_RetirementPlan_AC
                                    l_bool_SelectAccts = False
                                    l_DataRow("Total") = "0.00"
                                    l_DataRow("Taxable") = "0.00"
                                    l_DataRow("Non-Taxable") = "0.00"
                                    l_DataRow("Interest") = "0.00"
                                    'End 14-Jan-2009
                                    'End Code Merge by Dilip on 07-05-2009
                                Case m_const_SavingsPlan_TD
                                    l_bool_SelectAccts = True

                                    Session("HardshipAmountWithInterest_C19") = l_DataRow("Total")
                                    'START | 2019.08.07 |  SR | YRS-AT-4498 Starts | Add interests & Non-Taxable to Available Hardship TD amount
                                    If IsInterestAllowed Then
                                        Me.HardshipAvailable = DirectCast(l_DataRow("Total"), Decimal)
                                    Else
                                        l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                        l_DataRow("Interest") = "0.00"
                                        If NeedHardshipAvailAmt = True Then
                                            ' START | 2019.07.23 | SR | YRS-AT-3870 | Include non-taxable portion also for hardship withdrawal
                                            'Me.HardshipAvailable += DirectCast(l_DataRow("Taxable"), Decimal)
                                            Me.HardshipAvailable += DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("NonTaxable"), Decimal)
                                            ' END | 2019.07.23 | SR | YRS-AT-3870 | Include non-taxable portion also for hardship withdrawal
                                        End If
                                        'Me.HardshipAvailable = DirectCast(l_DataRow("Total"), Decimal)
                                    End If
                                    'END | 2019.08.07 |  SR | YRS-AT-4498 | Add interests & Non-Taxable to Available Hardship TD amount                                    
                                Case m_const_SavingsPlan_TM
                                    If l_DataRow("AccountType") = "TM" Then
                                        l_bool_SelectAccts = True

                                        l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("Interest"), Decimal)
                                        l_DataRow("Interest") = "0.00"
                                        If NeedHardshipAvailAmt = True Then
                                            Me.HardshipAvailable += DirectCast(l_DataRow("Taxable"), Decimal) 'todo eariler it was includinf YMCA taxable as well
                                        End If
                                    ElseIf l_DataRow("AccountType") = "TM-Matched" Then
                                        l_bool_SelectAccts = False
                                    End If


                            End Select


                            ''Modify the values
                            If l_bool_SelectAccts = True Then
                                If Me.RefundState = "R" Then
                                    l_DataRow("Selected") = "True"
                                End If

                                Me.YMCAAvailableAmount += DirectCast(l_DataRow("Taxable"), Decimal)
                                If l_DataRow("Total") = "0.00" Then
                                    l_DataRow.Delete()
                                End If
                            Else
                                l_DataRow.Delete()
                            End If






                        End If
                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()
                DoFlagSetForHardshipRefundForDisplay = l_ContributionDataTable

            End If
        Catch ex As Exception
            Throw
        End Try
    End Function


#End Region


#Region " Special Refund "
    'Shubhrata Mar30th,2007 YREN-3183 In accordance to Mark Posey's mail all money has to be withdrawn from all accounts
    'for SPEC refund.
    Public Function DoSpecialRefund(ByVal parameterCalledBy As String)


        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean

        Dim l_AccountGroup As String
        m_DataTableContributionDataTable = Nothing
        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                End If
            End If

            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows



                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total") Then


                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper
                            'Intialise the flags as false- Shubhrata Mar30th,2007
                            l_YMCASide = False
                            l_UserSide = False
                            'Commented by Shubhrata Mar30,2007 

                            'Modified by Shubhrata Mar30th,2007 For SPEC all money has to be withdrawn from all acoounts
                            'for a member who is vested.Non-vested members are not eligible for SPEC
                            If Me.IsVested = True Then
                                If Me.IsBasicAccount(l_DataRow) = True Then
                                    l_UserSide = True
                                    l_YMCASide = True
                                End If
                                Select Case (l_AccountGroup.ToUpper.Trim)
                                    'Start - Retirement Plan Group
                                    Case m_const_RetirementPlan_AM
                                        l_UserSide = True
                                        l_YMCASide = True

                                    Case m_const_RetirementPlan_AP
                                        l_UserSide = True
                                        l_YMCASide = True

                                    Case m_const_RetirementPlan_RP
                                        l_UserSide = True
                                        l_YMCASide = True

                                    Case m_const_RetirementPlan_SR
                                        l_UserSide = True
                                        l_YMCASide = True
                                        'Begin Code Merge by Dilip on 07-05-2009
                                        'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new case for Ac Account Type
                                    Case m_const_RetirementPlan_AC
                                        l_UserSide = True
                                        l_YMCASide = True
                                        'End Code Merge by Dilip on 07-05-2009
                                        'End 14-Jan-2009
                                        'End - Retirement Plan Group
                                        'Start - Savings Plan Group
                                    Case m_const_SavingsPlan_TD
                                        l_UserSide = True
                                        l_YMCASide = True

                                    Case m_const_SavingsPlan_TM
                                        l_UserSide = True
                                        l_YMCASide = True
                                    Case m_const_SavingsPlan_RT
                                        l_UserSide = True
                                        l_YMCASide = True
                                        'End - Savings Plan Group
                                End Select

                            Else
                                l_YMCASide = False
                                l_UserSide = False
                            End If


                            If l_UserSide Then

                                If l_YMCASide = False Then

                                    l_DataRow("YMCATaxable") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"
                                    l_DataRow("Total") = DirectCast(l_DataRow("Total"), Decimal) - DirectCast(l_DataRow("YMCATotal"), Decimal)
                                    l_DataRow("YMCATotal") = "0.00"

                                End If

                                l_DataRow("Selected") = "True"

                            Else
                                l_DataRow.Delete()
                            End If

                        End If
                    End If ' Main If...
                Next
                l_ContributionDataTable.AcceptChanges()
                m_DataTableContributionDataTable = l_ContributionDataTable
                ' Will have to integrate removed codes \\VPR 

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function



    '#Region " Disability Refund "

    Public Function DoDisabilityRefund(ByVal parameterCalledBy As String)

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_AccountType As String
        m_DataTableContributionDataTable = Nothing
        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                End If
            End If


            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (DirectCast(l_DataRow("AccountType"), String).Trim = "Total") Then

                            l_AccountType = DirectCast(l_DataRow("AccountType"), String).Trim.ToUpper

                            l_DataRow("Selected") = "True"

                        End If

                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()


                m_DataTableContributionDataTable = l_ContributionDataTable

            End If
        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region
#Region "Load Functions"

    '' This function is used to Get the Schema Table for AtsRefundRequests.
    Public Function LoadSchemaRefundTable()

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable


        Try
            l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSchemaRefundTable



            If Not l_DataSet Is Nothing Then



                l_DataTable = l_DataSet.Tables("atsRefunds")
                Session("AtsRefund_C19") = l_DataTable


                l_DataTable = l_DataSet.Tables("atsRefunds")
                Session("R_Refunds_C19") = l_DataTable


                l_DataTable = l_DataSet.Tables("atsRefRequests")
                Session("AtsRefRequests_C19") = l_DataTable


                l_DataTable = l_DataSet.Tables("atsRefRequestDetails")
                Session("AtsRefRequestDetails_C19") = l_DataTable


            End If

        Catch ex As Exception
            Throw
        End Try

    End Function




#Region " Calculation of Minimum Distribution Amount "

    Public Function CheckMRD()
        Dim dtMrdRecords As DataTable
        Try
            'dtMrdRecords = YMCARET.YmcaBusinessObject.RefundRequest.getmr
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#Region " OLD MRD CODE"

    '*****************************************************************************************************************
    '* Determine if a Minimum Distribution is Required for this Refund
    '* 1. Must be Terminated
    '* 2. Must have attained the Minimum age (70.5) at this Writing
    '* 3. Must be Rolling Money Over
    '* 4. Today's Date Must be after March 31st in the Year Following the Termination
    '*****************************************************************************************************************
    'Modified for Plan Split Changes



    Public Function CalculateMinimumDistributionAmount(ByVal MRDforPartialorMarket As Boolean)

        Dim l_DecimalDistributionPeriod As Decimal
        'Plan Split Variables
        Dim l_datarowarray_RetirementPlan As DataRow()
        Dim l_datarowarray_SavingsPlan As DataRow()
        Dim l_decimal_TotalRetirementPlanMoney As Decimal = 0
        Dim l_decimal_TotalSavingsPlanMoney As Decimal = 0
        Dim l_decimal_RetirementPlan_MinDistributionAmount As Decimal = 0
        Dim l_decimal_SavingsPlan_MinDistributionAmount As Decimal = 0
        Dim i As Integer = 0

        'Me.MinimumDistributionAmount = 0.0

        Try

            ' Check for Termination. This for Step 1.
            If Not Me.IsTerminated Then
                Return 0
            End If

            '' Check for Minimum age.
            If Me.PersonAge < 70.5 Then
                Return 0
            End If


            '' Check for the Date.
            If Me.CheckForTerminationDate = True Then

                '***********************************************************************************************
                '* Okay a Minimum Distribution is Required for this Person
                '* Let's go to the Table and find out how much
                '* Then Deduct it From the Payee's Accounts Until We Reach That Amount
                '***********************************************************************************************
                If MRDforPartialorMarket = False Then

                    'l_DecimalDistributionPeriod = YMCARET.YmcaBusinessObject.RefundRequest.GetDistributionPeriod(CType(Math.Round(Me.PersonAge, 2) - 0.5, Integer))
                    'Dim l_DataTable As DataTable
                    'Dim l_Decimal_Total As Decimal

                    'l_DataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)

                    ''the amount will be calculated for the plans individually 
                    'l_datarowarray_RetirementPlan = l_DataTable.Select("PlanType = 'RETIREMENT '")
                    'If l_datarowarray_RetirementPlan.Length > 0 Then
                    '    For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                    '        l_decimal_TotalRetirementPlanMoney += DirectCast(l_datarowarray_RetirementPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCAInterest"), Decimal)
                    '    Next
                    'End If
                    'l_datarowarray_SavingsPlan = l_DataTable.Select("PlanType = 'SAVINGS '")
                    'If l_datarowarray_SavingsPlan.Length > 0 Then
                    '    For i = 0 To l_datarowarray_SavingsPlan.Length - 1
                    '        l_decimal_TotalSavingsPlanMoney += DirectCast(l_datarowarray_SavingsPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCAInterest"), Decimal)
                    '    Next
                    'End If

                    'If l_decimal_TotalRetirementPlanMoney <> 0 Then
                    '    If l_DecimalDistributionPeriod <> 0 Then
                    '        l_decimal_RetirementPlan_MinDistributionAmount = Math.Round((l_decimal_TotalRetirementPlanMoney) / l_DecimalDistributionPeriod, 2)
                    '    End If
                    'End If
                    'If l_decimal_TotalSavingsPlanMoney <> 0 Then
                    '    If l_DecimalDistributionPeriod <> 0 Then
                    '        l_decimal_SavingsPlan_MinDistributionAmount = Math.Round((l_decimal_TotalSavingsPlanMoney) / l_DecimalDistributionPeriod, 2)
                    '    End If
                    'End If

                    'Me.RetirementPlan_MinDistributionAmount = l_decimal_RetirementPlan_MinDistributionAmount
                    'Me.Saving_MinDistributionAmount = l_decimal_SavingsPlan_MinDistributionAmount




                    'Me.RetirementPlan_MinDistributionAmount = dtMrdRecords.Select("")

                    Select Case Me.PlanTypeChosen.Trim.ToUpper
                        Case "RETIREMENT"
                            Me.MinimumDistributionAmount = l_decimal_RetirementPlan_MinDistributionAmount
                        Case "SAVINGS"
                            Me.MinimumDistributionAmount = l_decimal_SavingsPlan_MinDistributionAmount
                        Case "BOTH"
                            Me.MinimumDistributionAmount = l_decimal_RetirementPlan_MinDistributionAmount + l_decimal_SavingsPlan_MinDistributionAmount
                    End Select
                End If

                ''**********************************************************************************************
                '* Let's Take away taxable Monies from the current amounts table
                ''**********************************************************************************************

                'Commented the code and modified the same for MRD issue-Amit Nigam 
                'Me.DoMinimumDistributionCalculation()
                Session("MinimumDistributionTable_C19") = Nothing
                If MRDforPartialorMarket = False Then
                    Me.DoMinimumDistributionCalculation(Me.MinimumDistributionAmount, "RETIREMENT", MRDforPartialorMarket)
                Else
                    'Neeraj BT- 563: 
                    'check if plan type is RETIREMENT or BOTH 
                    If (Me.RetirementPlan_MinDistributionAmount > 0.0 AndAlso Me.PlanTypeChosen.Trim.ToUpper <> "SAVINGS") Then
                        Me.DoMinimumDistributionCalculation(Me.RetirementPlan_MinDistributionAmount, "RETIREMENT", MRDforPartialorMarket)
                    End If
                    'check if plan type is SAVINGS or BOTH
                    If Me.Saving_MinDistributionAmount > 0.0 AndAlso Me.PlanTypeChosen.Trim.ToUpper <> "RETIREMENT" Then
                        Me.DoMinimumDistributionCalculation(Me.Saving_MinDistributionAmount, "SAVINGS", MRDforPartialorMarket)
                    End If
                    'take the sum to Set Total MinimumDistributionAmount
                    Using dtTemp As DataTable = CType(Session("MinimumDistributionTable_C19"), DataTable)
                        If Not dtTemp Is Nothing AndAlso dtTemp.Rows.Count > 0 Then
                            'Me.MinimumDistributionAmount = CType(dtTemp.Compute("SUM(Taxable)", ""), Decimal)
                            Me.MinimumDistributionAmount = CType(dtTemp.Compute("SUM(Taxable)", ""), Decimal) + CType(dtTemp.Compute("SUM([Non-Taxable])", ""), Decimal)
                        End If
                    End Using
                End If
                'Neeraj BT- 563 end: 
                'Commented the code and modified the same for MRD issue-Amit Nigam

            End If
        Catch ex As Exception
            Throw
        End Try


    End Function
    'neeraj BT-664 : MRD Requirement 
    'calculate Applicable MRD Amount
    Public Sub CalculateMinimumDistributionAmount()
        Dim decMrdToBeSatisfiedForRetirementPlan As Decimal = 0.0
        Dim decMrdToBeSatisfiedForSavingPlan As Decimal = 0.0
        Dim dtCalculatedDataTableForCurrentAccounts As DataTable
        Dim l_datarowarray_RetirementPlan, l_datarowarray_SavingsPlan As DataRow()
        Dim l_decimal_TotalRetirementPlanMoney As Decimal = 0.0
        Dim l_decimal_TotalSavingsPlanMoney As Decimal = 0.0
        'IB:Added on 04/12/2010 For BT-688 Refund process screen showing total MRD amount if requested is less than MRD
        Dim l_RequestedAmounts_DataTable As DataTable
        Dim dtMrdRecords As DataTable     ' SB | 08/31/2016 | YRS-AT-3028 | DataTable to hold Recalaculated MRD values
        Try
            ' dtCalculatedDataTableForCurrentAccounts = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            l_RequestedAmounts_DataTable = DirectCast(Session("RequestedAccounts_C19"), DataTable)
            '  START : SB | 08/31/2016 | YRS-AT-3028 | Dataset to hold MrdValues and assiging them to datatables
            Dim dsAtsMrdRecords As DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetRMDForWithdrawalProcessing(Me.FundID)
            'Dim dtMrdRecords As DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetMRDRecords(Me.FundID)
            If HelperFunctions.isNonEmpty(dsAtsMrdRecords) AndAlso HelperFunctions.isNonEmpty(dsAtsMrdRecords.Tables(0)) Then
                dtMrdRecords = dsAtsMrdRecords.Tables(0)
            End If
            '  END : SB | 08/31/2016 | YRS-AT-3028 | Dataset to hold MrdValues and assiging them to datatables
            'IB:BT:800 Get participant's alll mrd record.To check any unsatisfied mrd record is pending at the time of saving 
            If Not dtMrdRecords Is Nothing AndAlso dtMrdRecords.Rows.Count > 0 Then
                Session("dtParticipantAllMrdRecords_C19") = dtMrdRecords.Copy()
            End If
            '  START : SB | 08/31/2016 | YRS-AT-3028 | Checking MrdRecords table having records and storing it session variable
            If HelperFunctions.isNonEmpty(dsAtsMrdRecords) AndAlso HelperFunctions.isNonEmpty(dsAtsMrdRecords.Tables(1)) Then
                ReCalculatedRMDValues = dsAtsMrdRecords
            End If
            '  END : SB | 08/31/2016 | YRS-AT-3028 | Checking MrdRecords table having records and storing it session variable

            'Commented By SG: 2012.03.14: BT-1010: condition for Expired Date is removed, so all previous unsatisfied RMD include for disbursement
            'SG:BT-962: YRS 5.0-1492:No RMD if person is actively employed (includes fund status RA) on 2012.03.03
            'If Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA" Then
            '    For Each l_DataRow As DataRow In dtMrdRecords.Rows
            '        If l_DataRow.IsNull("dtmExpireDate") = False AndAlso Convert.ToDateTime(l_DataRow("dtmExpireDate")) < System.DateTime.Now().AddDays(1) Then
            '            l_DataRow.Delete()
            '        End If
            '    Next
            'End If

            If HelperFunctions.isNonEmpty(dtMrdRecords) AndAlso Not dtMrdRecords.Columns("dtmExpireDate") Is Nothing Then   'SB | 08/31/2016 | YRS-AT-3028 | Checking MrdRecordTable
                dtMrdRecords.Columns.Remove("dtmExpireDate")
                dtMrdRecords.AcceptChanges()
            End If

            If Not dtMrdRecords Is Nothing AndAlso dtMrdRecords.Rows.Count > 0 Then
                Session("dtMrdRecords_C19") = dtMrdRecords
            End If
            'BT:726:Harshala-Application allowing to create MRD refund for Active participant-START : Added two condtions for status type.
            'If Not dtMrdRecords Is Nothing AndAlso dtMrdRecords.Rows.Count > 0 Then
            'If Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA" AndAlso HelperFunctions.isNonEmpty(dtMrdRecords) Then
            '--SR | 2016.08.25 | YRS-AT-3084 - QD participant do not have employment record hence allow them to bypass employment condition for RMD process.
            If ((IsTerminatedEmployment() Or SessionStatusType.Trim() = "QD") AndAlso HelperFunctions.isNonEmpty(dtMrdRecords)) Then 'SR/2015.06.24-BT 2903/YRS-AT-2542 - YRS bug discovered in which RMD (Required Minimum Distribution) records are being marked as eligible-yes for participants with pre-eligible (PE or RE) Fund statuses.
                'BT:726:Harshala-Application allowing to create MRD refund for Active participant-END.
                'calculate total MRD to be paid 
                l_datarowarray_RetirementPlan = dtMrdRecords.Select("PlanType = 'RETIREMENT'")
                If l_datarowarray_RetirementPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                        decMrdToBeSatisfiedForRetirementPlan += DirectCast(l_datarowarray_RetirementPlan(i)("Balance"), Decimal)
                    Next
                End If
                l_datarowarray_SavingsPlan = dtMrdRecords.Select("PlanType = 'SAVINGS'")
                If l_datarowarray_SavingsPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_SavingsPlan.Length - 1
                        decMrdToBeSatisfiedForSavingPlan += DirectCast(l_datarowarray_SavingsPlan(i)("Balance"), Decimal)
                    Next
                End If

                l_datarowarray_RetirementPlan = Nothing
                l_datarowarray_SavingsPlan = Nothing

                ''the amount will be calculated for the plans individually 
                'l_datarowarray_RetirementPlan = dtCalculatedDataTableForCurrentAccounts.Select("PlanType = 'RETIREMENT'")
                'If l_datarowarray_RetirementPlan.Length > 0 Then
                '    For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                '        l_decimal_TotalRetirementPlanMoney += DirectCast(l_datarowarray_RetirementPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCAInterest"), Decimal)
                '    Next
                'End If
                'l_datarowarray_SavingsPlan = dtCalculatedDataTableForCurrentAccounts.Select("PlanType = 'SAVINGS'")
                'If l_datarowarray_SavingsPlan.Length > 0 Then
                '    For i = 0 To l_datarowarray_SavingsPlan.Length - 1
                '        l_decimal_TotalSavingsPlanMoney += DirectCast(l_datarowarray_SavingsPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCAInterest"), Decimal)
                '    Next
                'End If
                l_datarowarray_RetirementPlan = l_RequestedAmounts_DataTable.Select("PlanType = 'RETIREMENT'")
                If l_datarowarray_RetirementPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                        'BT:948-  YRS 5.0-1457:must include non-taxable money as well   
                        ' l_decimal_TotalRetirementPlanMoney += DirectCast(l_datarowarray_RetirementPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCAInterest"), Decimal)
                        l_decimal_TotalRetirementPlanMoney += DirectCast(l_datarowarray_RetirementPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("Non-Taxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_RetirementPlan(i)("YMCAInterest"), Decimal)
                    Next
                End If
                l_datarowarray_SavingsPlan = l_RequestedAmounts_DataTable.Select("PlanType = 'SAVINGS'")
                If l_datarowarray_SavingsPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_SavingsPlan.Length - 1
                        'l_decimal_TotalSavingsPlanMoney += DirectCast(l_datarowarray_SavingsPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCAInterest"), Decimal)
                        l_decimal_TotalSavingsPlanMoney += DirectCast(l_datarowarray_SavingsPlan(i)("Taxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("Non-Taxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("Interest"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray_SavingsPlan(i)("YMCAInterest"), Decimal)
                    Next
                End If

                If l_decimal_TotalRetirementPlanMoney >= decMrdToBeSatisfiedForRetirementPlan Then
                    Me.RetirementPlan_MinDistributionAmount = decMrdToBeSatisfiedForRetirementPlan
                    'Me.Saving_MinDistributionAmount = l_decimal_SavingsPlan_MinDistributionAmount
                Else
                    Me.RetirementPlan_MinDistributionAmount = l_decimal_TotalRetirementPlanMoney
                End If

                If l_decimal_TotalSavingsPlanMoney >= decMrdToBeSatisfiedForSavingPlan Then
                    Me.Saving_MinDistributionAmount = decMrdToBeSatisfiedForSavingPlan
                Else
                    Me.Saving_MinDistributionAmount = l_decimal_TotalSavingsPlanMoney
                End If


                Session("MinimumDistributionTable_C19") = Nothing
                Session("CurrentAccountsWithMRD_C19") = Nothing ' SR | 2016.01.25 | YRS-AT-2664 - Clear session because this method has been called twice

                Dim drRetirementMrdRecods As DataRow() = dtMrdRecords.Select("PlanType='RETIREMENT'")
                'check if plan type is RETIREMENT or BOTH 
                If drRetirementMrdRecods.Length > 0 AndAlso Me.RetirementPlan_MinDistributionAmount > 0.0 AndAlso Me.PlanTypeChosen.Trim.ToUpper <> "SAVINGS" Then
                    'Me.DoMinimumDistributionCalculation(Me.RetirementPlan_MinDistributionAmount, "RETIREMENT")
                    Me.DoMinimumDistributionCalculation(dtMrdRecords, "RETIREMENT")
                End If

                Dim drSavingMrdRecords As DataRow() = dtMrdRecords.Select("PlanType='SAVINGS'")
                'check if plan type is SAVINGS or BOTH
                If drSavingMrdRecords.Length > 0 AndAlso Me.Saving_MinDistributionAmount > 0.0 AndAlso Me.PlanTypeChosen.Trim.ToUpper <> "RETIREMENT" Then
                    'Me.DoMinimumDistributionCalculation(Me.Saving_MinDistributionAmount, "SAVINGS")
                    Me.DoMinimumDistributionCalculation(dtMrdRecords, "SAVINGS")
                End If

                'take the sum to Set Total MinimumDistributionAmount
                Using dtTemp As DataTable = CType(Session("MinimumDistributionTable_C19"), DataTable)
                    If Not dtTemp Is Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        'Me.MinimumDistributionAmount = CType(dtTemp.Compute("SUM(Taxable)", ""), Decimal)
                        Me.MinimumDistributionAmount = CType(dtTemp.Compute("SUM(Taxable)", ""), Decimal) + CType(dtTemp.Compute("SUM(NonTaxable)", ""), Decimal)
                    End If
                End Using
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'neeraj BT 664 :MRD Requirement
    ' satisfiy the MRD amount from different Accounts
    'Public Function DoMinimumDistributionCalculation(ByVal paramMinimumDistributionAmount As Decimal, ByVal paramPlanType As String) As String
    '    'modified the code for MRD issue-Amit Nigam

    '    Dim l_MinimumDistributionTable As DataTable
    '    Dim l_CurrentAccountDataTable As DataTable
    '    Dim l_DataTable As DataTable

    '    Dim l_Decimal_PersonalPreTax As Decimal
    '    Dim l_Decimal_PersoanlPostTax As Decimal
    '    Dim l_Decimal_PersonalInterest As Decimal
    '    Dim l_Decimal_PersonalTotal As Decimal

    '    Dim l_Decimal_YMCAPreTax As Decimal
    '    Dim l_Decimal_YMCAInterest As Decimal
    '    Dim l_Decimal_YMCATotal As Decimal

    '    Dim l_Decimal_TaxRate As Decimal

    '    Dim l_Decimal_MinimumDistributionAmount As Decimal

    '    Dim l_Boolean_IsMinimumDistribution As Boolean
    '    Dim l_MinimumdistributionDataRow As DataRow
    '    Dim l_Decimal_Taxable As Decimal
    '    Dim l_Decimal_NonTaxable As Decimal

    '    Try

    '        l_DataTable = DirectCast(Session("AtsRefund"), DataTable)

    '        'Neeraj BT- 563: 
    '        If Not Session("MinimumDistributionTable_C19") Is Nothing Then
    '            l_MinimumDistributionTable = CType(Session("MinimumDistributionTable_C19"), DataTable)
    '        Else
    '            If Not l_DataTable Is Nothing Then
    '                l_MinimumDistributionTable = l_DataTable.Clone
    '            Else
    '                Return 0
    '            End If
    '        End If
    '        'Neeraj BT- 563: 

    '        l_CurrentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)

    '        'IB:BT:760- Taxable amount showing wrong When refund taken with exclude YMCA.
    '        For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows

    '            If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" And l_DataRow("AccountType").ToString() <> "Total" Then
    '                If IsBasicAccount(l_DataRow) Then
    '                    If Not IncludeorExcludeYMCAMoney(l_DataRow("AccountGroup").ToString.ToUpper(), l_DataRow("AccountType").ToString.ToUpper()) = True Then
    '                        l_DataRow("YMCATaxable") = 0.0
    '                        l_DataRow("YMCAInterest") = 0.0
    '                        l_DataRow("Total") = Convert.ToDouble(l_DataRow("Total")) - Convert.ToDouble(l_DataRow("YMCATotal"))
    '                        l_DataRow("YMCATotal") = 0.0
    '                    End If
    '                End If

    '            End If
    '            'If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" Or l_DataRow("AccountType").ToString() = "Total" Then

    '            'End If
    '        Next


    '        If l_CurrentAccountDataTable Is Nothing Then Return 0

    '        Me.MinimumDistributionAmount = 0.0
    '        l_Decimal_MinimumDistributionAmount = paramMinimumDistributionAmount

    '        For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
    '            'modified the code for MRD issue-Amit Nigam
    '            If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
    '                If l_DataRow("PlanType") = paramPlanType Then
    '                    'modified the code for MRD issue-Amit Nigam
    '                    If l_Decimal_MinimumDistributionAmount = 0 Then Exit For

    '                    '' Get the all values to Caluclate.

    '                    If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" Then
    '                        l_Decimal_PersonalPreTax = CType(l_DataRow("Taxable"), Decimal)
    '                    Else
    '                        l_Decimal_PersonalPreTax = 0
    '                    End If

    '                    'If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
    '                    '    l_Decimal_PersoanlPostTax = DirectCast(l_DataRow("Non-Taxable"), Decimal)
    '                    'Else
    '                    '    l_Decimal_PersoanlPostTax = 0
    '                    'End If

    '                    If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
    '                        l_Decimal_PersoanlPostTax = DirectCast(l_DataRow("Non-Taxable"), Decimal)
    '                    Else
    '                        l_Decimal_PersoanlPostTax = 0
    '                    End If

    '                    If l_DataRow("Interest").GetType.ToString <> "System.DBNull" Then
    '                        l_Decimal_PersonalInterest = CType(l_DataRow("Interest"), Decimal)
    '                    Else
    '                        l_Decimal_PersonalInterest = 0
    '                    End If

    '                    If l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" Then
    '                        l_Decimal_YMCAPreTax = CType(l_DataRow("YMCATaxable"), Decimal)
    '                    Else
    '                        l_Decimal_YMCAPreTax = 0
    '                    End If

    '                    If l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" Then
    '                        l_Decimal_YMCAInterest = CType(l_DataRow("YMCAInterest"), Decimal)
    '                    Else
    '                        l_Decimal_YMCAInterest = 0
    '                    End If


    '                    '' Start calculation .. 

    '                    If (l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest + l_Decimal_YMCAPreTax + l_Decimal_YMCAInterest + l_Decimal_PersoanlPostTax) > 0 Then ''Main If

    '                        l_Boolean_IsMinimumDistribution = False

    '                        '' For Personal Taxable
    '                        If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalPreTax Then


    '                            l_Boolean_IsMinimumDistribution = True
    '                            l_Decimal_Taxable = l_Decimal_PersonalPreTax
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_PersonalPreTax
    '                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersonalPreTax

    '                            l_DataRow("Taxable") = "0.00"
    '                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                 CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                 CType(l_DataRow("Interest"), Decimal)

    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + _
    '                                                CType(l_DataRow("YMCATotal"), Decimal)

    '                        Else


    '                            l_Boolean_IsMinimumDistribution = True
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("Taxable") = l_DataRow("Taxable") - l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + _
    '                                                CType(l_DataRow("YMCATotal"), Decimal)



    '                            l_Decimal_Taxable = l_Decimal_MinimumDistributionAmount
    '                            l_Decimal_MinimumDistributionAmount = 0.0

    '                        End If


    '                        '' For Personal Interest
    '                        If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalInterest Then



    '                            l_Boolean_IsMinimumDistribution = True
    '                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_PersonalInterest

    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_PersonalInterest
    '                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersonalInterest

    '                            l_DataRow("Interest") = "0.00"

    '                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + _
    '                                                CType(l_DataRow("YMCATotal"), Decimal)


    '                        Else



    '                            l_Boolean_IsMinimumDistribution = True
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("Interest") = l_DataRow("Interest") - l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)


    '                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount
    '                            l_Decimal_MinimumDistributionAmount = 0.0

    '                        End If

    '                        ''For Personal Non-taxable

    '                        If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersoanlPostTax Then



    '                            l_Boolean_IsMinimumDistribution = True
    '                            l_Decimal_NonTaxable = l_Decimal_PersoanlPostTax

    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_PersoanlPostTax
    '                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersoanlPostTax

    '                            l_DataRow("Non-Taxable") = "0.00"

    '                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + _
    '                                                CType(l_DataRow("YMCATotal"), Decimal)


    '                        Else



    '                            l_Boolean_IsMinimumDistribution = True
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("Non-Taxable") = l_DataRow("Non-Taxable") - l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)


    '                            l_Decimal_NonTaxable = l_Decimal_MinimumDistributionAmount
    '                            l_Decimal_MinimumDistributionAmount = 0.0

    '                        End If



    '                        ''For YMCA Taxable.

    '                        If l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAPreTax Then


    '                            l_Boolean_IsMinimumDistribution = True
    '                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_YMCAPreTax
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_YMCAPreTax

    '                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_YMCAPreTax

    '                            l_DataRow("YMCATaxable") = "0.00"

    '                            l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

    '                        Else


    '                            l_Boolean_IsMinimumDistribution = True
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("YMCATaxable") = l_DataRow("YMCATaxable") - l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

    '                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount
    '                            l_Decimal_MinimumDistributionAmount = 0.0

    '                        End If

    '                        If l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAInterest Then


    '                            l_Boolean_IsMinimumDistribution = True

    '                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_YMCAInterest
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_YMCAInterest
    '                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_YMCAInterest

    '                            l_DataRow("YMCAInterest") = "0.00"

    '                            l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)


    '                        Else

    '                            l_Boolean_IsMinimumDistribution = True
    '                            Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("YMCAInterest") = l_Decimal_YMCAInterest - l_Decimal_MinimumDistributionAmount

    '                            l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

    '                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount
    '                            l_Decimal_MinimumDistributionAmount = 0

    '                        End If

    '                        '' This Values has to be updated in the Table.

    '                        If l_Boolean_IsMinimumDistribution = True Then

    '                            ''Get TaxRate

    '                            l_MinimumdistributionDataRow = l_MinimumDistributionTable.NewRow

    '                            l_MinimumdistributionDataRow("Taxable") = l_Decimal_Taxable
    '                            l_MinimumdistributionDataRow("TaxRate") = Me.MinDistributionTaxRate
    '                            l_MinimumdistributionDataRow("Tax") = Math.Round(l_Decimal_Taxable * (Me.MinDistributionTaxRate / 100.0), 2)
    '                            l_MinimumdistributionDataRow("NonTaxable") = l_Decimal_NonTaxable '"0.00"
    '                            l_MinimumdistributionDataRow("AcctType") = l_DataRow("AccountType")
    '                            l_MinimumdistributionDataRow("Payee") = Me.Payee1Name
    '                            l_MinimumdistributionDataRow("FundedDate") = System.DBNull.Value
    '                            l_MinimumdistributionDataRow("RefRequestsID") = Me.SessionRefundRequestID


    '                            l_MinimumDistributionTable.Rows.Add(l_MinimumdistributionDataRow)

    '                        End If
    '                        l_DataRow.AcceptChanges()


    '                        'modified the code for MRD issue-Amit Nigam
    '                    End If
    '                End If
    '                'modified the code for MRD issue-Amit Nigam
    '            End If '' Main if

    '        Next


    '        If l_Decimal_MinimumDistributionAmount > Me.MinimumDistributionAmount Then

    '            Return (" Minimum Distrubtion Not Met.  A Minimum Distribution of " + Math.Round(Me.MinimumDistributionAmount, 2).ToString + " is Required.. There isn't enough Taxable Money to Roll any over.")

    '        End If

    '        Session("MinimumDistributionTable_C19") = l_MinimumDistributionTable
    '        l_CurrentAccountDataTable.AcceptChanges()

    '        Session("CalculatedDataTableForCurrentAccounts_C19") = l_CurrentAccountDataTable

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function


    'Modified for Plan Split Changes
    'modified the code for MRD issue-Amit Nigam
    'Public Function DoMinimumDistributionCalculation() As String

    Public Function DoMinimumDistributionCalculation(ByVal paramRMDDataTable As DataTable, ByVal paramPlanType As String) As String
        Dim l_MinimumDistributionTable As DataTable
        Dim l_CurrentAccountDataTable As DataTable
        Dim l_DataTable As DataTable

        Dim l_TotalTaxable As Decimal
        Dim l_TotalNonTaxable As Decimal
        Dim l_MRDTaxable As Decimal
        Dim l_MRDNonTaxable As Decimal

        Dim NumFactorofTaxable As Decimal
        Dim NumFactorofNonTaxable As Decimal
        Dim l_datarowarray As DataRow()

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersoanlPostTax As Decimal
        Dim l_Decimal_PersonalInterest As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal
        Dim l_Decimal_YMCAInterest As Decimal
        Dim l_Decimal_TaxRate As Decimal
        Dim l_MinimumdistributionDataRow As DataRow
        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_TotalMRD As Decimal
        Dim l_temp_DataTable As DataTable

        Dim dtCurrentAccountDataTable As DataTable  'SR | 2016.01.22 | YRS-AT-2664 - Add variable

        Try

            l_DataTable = DirectCast(Session("AtsRefund_C19"), DataTable)
            l_temp_DataTable = l_DataTable.Clone
            If Not Session("MinimumDistributionTable_C19") Is Nothing Then
                l_MinimumDistributionTable = CType(Session("MinimumDistributionTable_C19"), DataTable)
            Else
                If Not l_DataTable Is Nothing Then
                    l_MinimumDistributionTable = l_DataTable.Clone
                Else
                    Return 0
                End If
            End If

            l_CurrentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)

            For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows

                If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" And l_DataRow("AccountType").ToString() <> "Total" Then
                    If IsBasicAccount(l_DataRow) Then
                        If Not IncludeorExcludeYMCAMoney(l_DataRow("AccountGroup").ToString.ToUpper(), l_DataRow("AccountType").ToString.ToUpper()) = True Then
                            l_DataRow("YMCATaxable") = 0.0
                            l_DataRow("YMCAInterest") = 0.0
                            l_DataRow("Total") = Convert.ToDouble(l_DataRow("Total")) - Convert.ToDouble(l_DataRow("YMCATotal"))
                            l_DataRow("YMCATotal") = 0.0
                        End If
                    End If

                End If
            Next

            ' START | SR | 2016.01.22 | YRS-AT-2664 - Add RMD Taxable Amount in Rollover.
            If (Session("CurrentAccountsWithMRD_C19")) Is Nothing Then
                Session("CurrentAccountsWithMRD_C19") = l_CurrentAccountDataTable.Copy
            End If
            ' END | SR | 2016.01.22 | YRS-AT-2664 - Add RMD Taxable Amount in Rollover.

            Dim drMrdRecods As DataRow() = paramRMDDataTable.Select("PlanType='" + paramPlanType + "'")
            If drMrdRecods.Length > 0 Then
                For i = 0 To drMrdRecods.Length - 1
                    l_MRDTaxable += DirectCast(drMrdRecods(i)("MRDTaxable"), Decimal)
                    l_MRDNonTaxable += DirectCast(drMrdRecods(i)("MRDNonTaxable"), Decimal)
                    l_Decimal_TotalMRD += DirectCast(drMrdRecods(i)("Balance"), Decimal)
                Next
            End If
            If l_MRDNonTaxable + l_MRDTaxable = 0 Then Return 0

            If l_CurrentAccountDataTable Is Nothing Then Return 0

            l_datarowarray = l_CurrentAccountDataTable.Select("PlanType = '" + paramPlanType + "'")
            If l_datarowarray.Length > 0 Then
                For i = 0 To l_datarowarray.Length - 1
                    l_TotalTaxable += DirectCast(l_datarowarray(i)("Taxable"), Decimal) + DirectCast(l_datarowarray(i)("Interest"), Decimal) + DirectCast(l_datarowarray(i)("YMCATaxable"), Decimal) + DirectCast(l_datarowarray(i)("YMCAInterest"), Decimal)
                Next
            End If
            l_datarowarray = l_CurrentAccountDataTable.Select("PlanType = '" + paramPlanType + "'")
            If l_datarowarray.Length > 0 Then
                For i = 0 To l_datarowarray.Length - 1
                    l_TotalNonTaxable += DirectCast(l_datarowarray(i)("Non-Taxable"), Decimal)
                Next
            End If

            If l_TotalTaxable < l_MRDTaxable AndAlso l_TotalNonTaxable > l_MRDNonTaxable Then
                l_MRDNonTaxable += (l_MRDTaxable - l_TotalTaxable)
                l_MRDTaxable = l_TotalTaxable
            End If


            If l_TotalNonTaxable < l_MRDNonTaxable AndAlso l_TotalTaxable > l_MRDTaxable Then
                l_MRDTaxable += (l_MRDNonTaxable - l_TotalNonTaxable)
                l_MRDNonTaxable = l_TotalNonTaxable

            End If

            'START | SR | YRS-AT-3742 | If Refund type is other than "Only RMD" then satisfy RMD amount from Non taxable component first & remaining from taxable component.
            If Me.RefundType.Trim.ToUpper() <> "MRD" Then
                If ((l_MRDTaxable + l_MRDNonTaxable) >= l_TotalNonTaxable) Then
                    l_MRDTaxable = l_MRDTaxable - (l_TotalNonTaxable - l_MRDNonTaxable)
                    l_MRDNonTaxable = l_TotalNonTaxable
                ElseIf (l_TotalNonTaxable > (l_MRDTaxable + l_MRDNonTaxable)) Then
                    'l_TotalNonTaxable = l_TotalNonTaxable + l_MRDNonTaxable
                    l_MRDNonTaxable = (l_MRDNonTaxable + l_MRDTaxable)
                    l_MRDTaxable = 0
                End If
            End If
            'END | SR | YRS-AT-3742 | If Refund type is other than "Only RMD" then satisfy RMD amount from Non taxable component first & remaining from taxable component.

            If l_TotalTaxable >= l_MRDTaxable AndAlso l_MRDTaxable > 0 Then
                NumFactorofTaxable = IIf(l_TotalTaxable > 0, l_MRDTaxable / l_TotalTaxable, 0)
            ElseIf l_TotalTaxable < l_MRDTaxable AndAlso l_MRDTaxable > 0 Then
                l_MRDTaxable = l_TotalTaxable
                NumFactorofTaxable = 1
            Else
                NumFactorofTaxable = 0
            End If

            If l_TotalNonTaxable >= l_MRDNonTaxable AndAlso l_MRDNonTaxable > 0 Then
                NumFactorofNonTaxable = IIf(l_TotalNonTaxable > 0, l_MRDNonTaxable / l_TotalNonTaxable, 0)
            ElseIf l_TotalNonTaxable < l_MRDNonTaxable AndAlso l_MRDNonTaxable > 0 Then
                l_MRDNonTaxable = l_TotalNonTaxable
                NumFactorofNonTaxable = 1
            Else
                NumFactorofNonTaxable = 0
            End If


            For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
                If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                    If l_DataRow("PlanType") = paramPlanType Then

                        If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_PersonalPreTax = Math.Round(CType(l_DataRow("Taxable"), Decimal) * NumFactorofTaxable, 2)
                            l_DataRow("Taxable") = CType(l_DataRow("Taxable"), Decimal) - l_Decimal_PersonalPreTax

                        Else
                            l_Decimal_PersonalPreTax = 0
                        End If

                        If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_PersoanlPostTax = Math.Round(DirectCast(l_DataRow("Non-Taxable"), Decimal) * NumFactorofNonTaxable, 2)
                            l_DataRow("Non-Taxable") = l_DataRow("Non-Taxable") - l_Decimal_PersoanlPostTax
                        Else
                            l_Decimal_PersoanlPostTax = 0
                        End If

                        If l_DataRow("Interest").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_PersonalInterest = Math.Round(CType(l_DataRow("Interest"), Decimal) * NumFactorofTaxable, 2)
                            l_DataRow("Interest") = l_DataRow("Interest") - l_Decimal_PersonalInterest
                        Else
                            l_Decimal_PersonalInterest = 0
                        End If

                        If l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_YMCAPreTax = Math.Round(CType(l_DataRow("YMCATaxable"), Decimal) * NumFactorofTaxable, 2)
                            l_DataRow("YMCATaxable") = l_DataRow("YMCATaxable") - l_Decimal_YMCAPreTax
                        Else
                            l_Decimal_YMCAPreTax = 0
                        End If

                        If l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_YMCAInterest = Math.Round(CType(l_DataRow("YMCAInterest"), Decimal) * NumFactorofTaxable, 2)
                            l_DataRow("YMCAInterest") = l_DataRow("YMCAInterest") - l_Decimal_YMCAInterest
                        Else
                            l_Decimal_YMCAInterest = 0
                        End If

                        l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
                                            CType(l_DataRow("Non-Taxable"), Decimal) + _
                                            CType(l_DataRow("Interest"), Decimal)


                        l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
                        l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)


                        l_Decimal_Taxable = l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest + l_Decimal_YMCAPreTax + l_Decimal_YMCAInterest
                        l_Decimal_NonTaxable = l_Decimal_PersoanlPostTax
                        l_MinimumdistributionDataRow = l_temp_DataTable.NewRow

                        l_MinimumdistributionDataRow("Taxable") = l_Decimal_Taxable
                        l_MinimumdistributionDataRow("NonTaxable") = l_Decimal_NonTaxable
                        l_MinimumdistributionDataRow("TaxRate") = Me.MinDistributionTaxRate
                        l_MinimumdistributionDataRow("Tax") = Math.Round((l_Decimal_Taxable) * (Me.MinDistributionTaxRate / 100.0), 2)

                        l_MinimumdistributionDataRow("AcctType") = l_DataRow("AccountType")
                        l_MinimumdistributionDataRow("Payee") = Me.Payee1Name
                        l_MinimumdistributionDataRow("FundedDate") = System.DBNull.Value
                        l_MinimumdistributionDataRow("RefRequestsID") = Me.SessionRefundRequestID
                        l_temp_DataTable.Rows.Add(l_MinimumdistributionDataRow)

                    End If
                    l_DataRow.AcceptChanges()
                End If
            Next
            'Dim l_RMDSatisfied_Taxable As Decimal
            'Dim l_RMDSatisfied_NonTaxable As Decimal
            'If paramPlanType = "RETIREMENT" Then
            '    l_RMDSatisfied_Taxable = l_MinimumDistributionTable.Compute("SUM(TAXABLE)", "")
            'End If


            'Session("MinimumDistributionTable_C19") = l_MinimumDistributionTable
            l_CurrentAccountDataTable.AcceptChanges()
            Session("CalculatedDataTableForCurrentAccounts_C19") = l_CurrentAccountDataTable

            l_DataTable = FixedRoundingRMD(l_temp_DataTable, l_MRDTaxable, l_MRDNonTaxable, paramPlanType)

            For Each l_DataRow As DataRow In l_temp_DataTable.Rows

                l_MinimumdistributionDataRow = l_MinimumDistributionTable.NewRow
                l_MinimumdistributionDataRow("Taxable") = l_DataRow("Taxable")
                l_MinimumdistributionDataRow("NonTaxable") = l_DataRow("NonTaxable")
                l_MinimumdistributionDataRow("TaxRate") = l_DataRow("TaxRate")
                l_MinimumdistributionDataRow("Tax") = Math.Round((l_DataRow("Taxable")) * (Me.MinDistributionTaxRate / 100.0), 2)

                l_MinimumdistributionDataRow("AcctType") = l_DataRow("AcctType")
                l_MinimumdistributionDataRow("Payee") = Me.Payee1Name
                l_MinimumdistributionDataRow("FundedDate") = System.DBNull.Value
                l_MinimumdistributionDataRow("RefRequestsID") = Me.SessionRefundRequestID
                l_MinimumDistributionTable.Rows.Add(l_MinimumdistributionDataRow)
            Next

            Session("MinimumDistributionTable_C19") = l_MinimumDistributionTable


        Catch ex As Exception
            Throw
        End Try
    End Function
    'START :ML |2020.04.23 | YRS-AT-4874 | Calculate COVID contribution 
    ''' <summary>
    ''' This method is used to prorate Current Balance and Split Current Account in two different table.
    ''' 1. Get current Account Datatable. This datatable contain remaing balance after RMD
    ''' 2. Calculate Covid factor.
    ''' 3. Prorate Current Account Datatable using covid factor. Store current Account datatable. This datatable contain remaining balance after COVID
    ''' 4. Create datatable for COVIDProrated amount.
    ''' 5. Fix rounding issue for both table.
    ''' </summary>
    ''' <param name="paramCovidAmountRequested"></param>
    ''' <param name="paramCovidTaxRate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CalculateCOVIDDistributionAmount(ByVal paramCovidAmountRequested As Decimal, ByVal paramCovidTaxRate As Decimal)
        Dim dtRefundDatatable As DataTable
        Dim dtCurrentAccount As DataTable
        Dim drCurrentAccount As DataRow()

        Dim dtCOVIDProrateAccount As DataTable
        Dim drCOVIDProrateAccount As DataRow

        Dim totalTaxableRetirement As Decimal
        Dim totalTaxableSaving As Decimal
        Dim totalNonTaxableRetirement As Decimal
        Dim totalNonTaxableSaving As Decimal
        Dim totalAvailableAmount As Decimal

        Dim covidFactor As Decimal = 1
        Dim Taxable, NonTaxable, Interest, YMCATaxable, YMCAInterest As Decimal
        Dim TotalTaxable, TotalNontaxable As Decimal
        Try

            If paramCovidAmountRequested < 0 Then
                Return 0
            End If
            dtRefundDatatable = DirectCast(Session("AtsRefund_C19"), DataTable)
            dtCOVIDProrateAccount = dtRefundDatatable.Clone

            dtCurrentAccount = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            For Each l_DataRow As DataRow In dtCurrentAccount.Rows
                If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" And l_DataRow("AccountType").ToString() <> "Total" Then
                    If IsBasicAccount(l_DataRow) Then
                        If Not IncludeorExcludeYMCAMoney(l_DataRow("AccountGroup").ToString.ToUpper(), l_DataRow("AccountType").ToString.ToUpper()) = True Then
                            l_DataRow("YMCATaxable") = 0.0
                            l_DataRow("YMCAInterest") = 0.0
                            l_DataRow("Total") = Convert.ToDouble(l_DataRow("Total")) - Convert.ToDouble(l_DataRow("YMCATotal"))
                            l_DataRow("YMCATotal") = 0.0
                        End If
                    End If
                End If
            Next

            drCurrentAccount = dtCurrentAccount.Select("PlanType = 'RETIREMENT'")
            If drCurrentAccount.Length > 0 Then
                For i = 0 To drCurrentAccount.Length - 1
                    totalTaxableRetirement += DirectCast(drCurrentAccount(i)("Taxable"), Decimal) + DirectCast(drCurrentAccount(i)("Interest"), Decimal) + DirectCast(drCurrentAccount(i)("YMCATaxable"), Decimal) + DirectCast(drCurrentAccount(i)("YMCAInterest"), Decimal)
                    totalNonTaxableRetirement += DirectCast(drCurrentAccount(i)("Non-Taxable"), Decimal)
                Next
            End If
            drCurrentAccount = dtCurrentAccount.Select("PlanType = 'SAVINGS'")
            If drCurrentAccount.Length > 0 Then
                For i = 0 To drCurrentAccount.Length - 1
                    totalTaxableSaving += DirectCast(drCurrentAccount(i)("Taxable"), Decimal) + DirectCast(drCurrentAccount(i)("Interest"), Decimal) + DirectCast(drCurrentAccount(i)("YMCATaxable"), Decimal) + DirectCast(drCurrentAccount(i)("YMCAInterest"), Decimal)
                    totalNonTaxableSaving += DirectCast(drCurrentAccount(i)("Non-Taxable"), Decimal)
                Next
            End If


            totalAvailableAmount = totalTaxableRetirement + totalNonTaxableRetirement + totalTaxableSaving + totalNonTaxableSaving
            If paramCovidAmountRequested < totalAvailableAmount Then covidFactor = (paramCovidAmountRequested / CDec(totalAvailableAmount))
            Me.CovidTaxableAmount = Math.Round((totalTaxableRetirement + totalTaxableSaving) * covidFactor, 2)
            Me.CovidNonTaxableAmount = Math.Round((totalNonTaxableRetirement + totalNonTaxableSaving) * covidFactor, 2)

            'If the prorated amounts do not match, then adjust the value from the bigger amount
            If (Me.CovidTaxableAmount + Me.CovidNonTaxableAmount) <> paramCovidAmountRequested Then
                If (Me.CovidTaxableAmount > Me.CovidNonTaxableAmount) Then
                    Me.CovidTaxableAmount = paramCovidAmountRequested - Me.CovidNonTaxableAmount
                Else
                    Me.CovidNonTaxableAmount = paramCovidAmountRequested - Me.CovidTaxableAmount
                End If
            End If

            For Each l_DataRow As DataRow In dtCurrentAccount.Rows
                If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then

                    If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" Then
                        Taxable = Math.Round(CType(l_DataRow("Taxable"), Decimal) * covidFactor, 2)
                        l_DataRow("Taxable") = CType(l_DataRow("Taxable"), Decimal) - Taxable
                    Else
                        Taxable = 0
                    End If

                    If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
                        NonTaxable = Math.Round(CType(l_DataRow("Non-Taxable"), Decimal) * covidFactor, 2)
                        l_DataRow("Non-Taxable") = CType(l_DataRow("Non-Taxable"), Decimal) - NonTaxable
                    Else
                        NonTaxable = 0
                    End If

                    If l_DataRow("Interest").GetType.ToString <> "System.DBNull" Then
                        Interest = Math.Round(CType(l_DataRow("Interest"), Decimal) * covidFactor, 2)
                        l_DataRow("Interest") = CType(l_DataRow("Interest"), Decimal) - Interest
                    Else
                        Interest = 0
                    End If

                    If l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" Then
                        YMCATaxable = Math.Round(CType(l_DataRow("YMCATaxable"), Decimal) * covidFactor, 2)
                        l_DataRow("YMCATaxable") = CType(l_DataRow("YMCATaxable"), Decimal) - YMCATaxable
                    Else
                        YMCATaxable = 0
                    End If

                    If l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" Then
                        YMCAInterest = Math.Round(CType(l_DataRow("YMCAInterest"), Decimal) * covidFactor, 2)
                        l_DataRow("YMCAInterest") = CType(l_DataRow("YMCAInterest"), Decimal) - YMCAInterest
                    Else
                        YMCAInterest = 0
                    End If

                    l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
                                        CType(l_DataRow("Non-Taxable"), Decimal) + _
                                        CType(l_DataRow("Interest"), Decimal)


                    l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

                    l_DataRow.AcceptChanges()

                    TotalTaxable = Taxable + Interest + YMCATaxable + YMCAInterest
                    TotalNontaxable = NonTaxable
                    drCOVIDProrateAccount = dtCOVIDProrateAccount.NewRow

                    drCOVIDProrateAccount("Taxable") = TotalTaxable
                    drCOVIDProrateAccount("NonTaxable") = TotalNontaxable
                    drCOVIDProrateAccount("TaxRate") = paramCovidTaxRate
                    drCOVIDProrateAccount("Tax") = Math.Round((TotalTaxable) * (paramCovidTaxRate / 100.0), 2)

                    drCOVIDProrateAccount("AcctType") = l_DataRow("AccountType")
                    drCOVIDProrateAccount("Payee") = Me.Payee1Name
                    drCOVIDProrateAccount("FundedDate") = System.DBNull.Value
                    drCOVIDProrateAccount("RefRequestsID") = Me.SessionRefundRequestID
                    dtCOVIDProrateAccount.Rows.Add(drCOVIDProrateAccount)
                End If
            Next

            dtCurrentAccount.AcceptChanges()
            dtCOVIDProrateAccount.AcceptChanges()
            FixedRoundingCOVID(dtCOVIDProrateAccount, dtCurrentAccount, Me.CovidTaxableAmount, Me.CovidNonTaxableAmount) 'SC : 2020.05.05 | Fix Rounding Issue
            Session("CalculatedDataTableForCurrentAccounts_C19") = dtCurrentAccount
            Session("COVIDProrateAccountDataTable") = dtCOVIDProrateAccount
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' This method will fix the  rounding issue with the final Taxable and Non-Taxable covid amounts
    ''' 1. Accepts total prorated Taxable, prorated Non taxable covid amounts
    ''' 2. Calculates sum of total taxable, non taxable amounts on Account level for Covid
    ''' 3. Adjust Covidprorated datatable with Rounding value
    ''' 4. Adjust current account balance datatable with Rounding value 
    ''' 5. Adjust the Tax computed value based on the rounding and also match computed tax at the total level by adjusting largest record
    ''' </summary>
    Private Function FixedRoundingCOVID(ByVal DatatableToBeFixed As DataTable, ByVal l_CurrentAccountDataTable As DataTable, ByVal covidTaxable As Decimal, ByVal covidNonTaxable As Decimal)

        Dim datarow As Integer
        Dim totalReqAmount As Decimal
        Dim RoundingTaxableValue As Decimal
        Dim RoundingNonTaxableValue As Decimal

        Dim requestedamount As Decimal
        Dim totalTaxableReqAmount As Decimal
        Dim totalNonTaxableReqAmount As Decimal
        Dim accountType As String
        Dim selectedRowFromDataTable As DataRow
        Dim selectedRowTaxableValue As Decimal


        Dim l_drRow As DataRow

        Try
            requestedamount = covidTaxable + covidNonTaxable
            If requestedamount <= 0 Then
                Return DatatableToBeFixed
            End If

            For Each l_DataRow As DataRow In DatatableToBeFixed.Rows
                totalTaxableReqAmount += l_DataRow("Taxable")
                totalNonTaxableReqAmount += l_DataRow("NonTaxable")
            Next

            RoundingTaxableValue = covidTaxable - totalTaxableReqAmount
            RoundingNonTaxableValue = covidNonTaxable - totalNonTaxableReqAmount

            If RoundingTaxableValue <> 0 Then
                For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
                    If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                        accountType = l_DataRow("AccountType").ToString()
                        selectedRowFromDataTable = Me.GetDataRowForSelectedTable(DatatableToBeFixed, accountType)
                        If RoundingTaxableValue <> 0 And Not selectedRowFromDataTable Is Nothing Then

                            If ((l_DataRow("Taxable").GetType.ToString <> "System.DBNull" AndAlso (CType(l_DataRow("Taxable"), Decimal) - RoundingTaxableValue) >= 0) _
                                And (selectedRowFromDataTable("Taxable").GetType.ToString <> "System.DBNull" AndAlso (CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue) >= 0)) Then
                                l_DataRow("Taxable") = CType(l_DataRow("Taxable"), Decimal) - RoundingTaxableValue
                                selectedRowTaxableValue = CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue
                                selectedRowFromDataTable("Taxable") = selectedRowTaxableValue
                                selectedRowFromDataTable("Tax") = Math.Round((selectedRowTaxableValue) * (CType(selectedRowFromDataTable("TaxRate"), Decimal) / 100.0), 2)
                                RoundingTaxableValue = 0
                            ElseIf ((l_DataRow("Interest").GetType.ToString <> "System.DBNull" AndAlso (CType(l_DataRow("Interest"), Decimal) - RoundingTaxableValue) >= 0) _
                                 And (selectedRowFromDataTable("Taxable").GetType.ToString <> "System.DBNull" AndAlso (CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue) >= 0)) Then
                                l_DataRow("Interest") = l_DataRow("Interest") - RoundingTaxableValue
                                selectedRowTaxableValue = CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue
                                selectedRowFromDataTable("Taxable") = selectedRowTaxableValue
                                selectedRowFromDataTable("Tax") = Math.Round((selectedRowTaxableValue) * (CType(selectedRowFromDataTable("TaxRate"), Decimal) / 100.0), 2)
                                RoundingTaxableValue = 0
                            ElseIf ((l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" AndAlso (CType(l_DataRow("YMCATaxable"), Decimal) - RoundingTaxableValue) >= 0) _
                                 And (selectedRowFromDataTable("Taxable").GetType.ToString <> "System.DBNull" AndAlso (CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue) >= 0)) Then
                                l_DataRow("YMCATaxable") = CType(l_DataRow("YMCATaxable"), Decimal) - RoundingTaxableValue
                                selectedRowTaxableValue = CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue
                                selectedRowFromDataTable("Taxable") = selectedRowTaxableValue
                                selectedRowFromDataTable("Tax") = Math.Round((selectedRowTaxableValue) * (CType(selectedRowFromDataTable("TaxRate"), Decimal) / 100.0), 2)
                                RoundingTaxableValue = 0
                            ElseIf ((l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" AndAlso (CType(l_DataRow("YMCAInterest"), Decimal) - RoundingTaxableValue) >= 0) _
                                And (selectedRowFromDataTable("Taxable").GetType.ToString <> "System.DBNull" AndAlso (CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue) >= 0)) Then
                                l_DataRow("YMCAInterest") = l_DataRow("YMCAInterest") - RoundingTaxableValue
                                selectedRowTaxableValue = CType(selectedRowFromDataTable("Taxable"), Decimal) + RoundingTaxableValue
                                selectedRowFromDataTable("Taxable") = selectedRowTaxableValue
                                selectedRowFromDataTable("Tax") = Math.Round((selectedRowTaxableValue) * (CType(selectedRowFromDataTable("TaxRate"), Decimal) / 100.0), 2)
                                RoundingTaxableValue = 0
                            End If
                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
                                                CType(l_DataRow("Interest"), Decimal)

                            l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)
                            l_DataRow.AcceptChanges()
                            selectedRowFromDataTable.AcceptChanges()

                        End If
                    End If
                Next
            End If



            If RoundingNonTaxableValue <> 0 Then
                For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
                    If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                        accountType = l_DataRow("AccountType").ToString()
                        selectedRowFromDataTable = Me.GetDataRowForSelectedTable(DatatableToBeFixed, accountType)
                        If RoundingNonTaxableValue <> 0 And Not selectedRowFromDataTable Is Nothing Then
                            If ((l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" AndAlso (CType(l_DataRow("Non-Taxable"), Decimal) - RoundingNonTaxableValue) >= 0) _
                                And (selectedRowFromDataTable("NonTaxable").GetType.ToString <> "System.DBNull" AndAlso (CType(selectedRowFromDataTable("NonTaxable"), Decimal) + RoundingNonTaxableValue) >= 0)) Then
                                l_DataRow("Non-Taxable") = CType(l_DataRow("Non-Taxable"), Decimal) - RoundingNonTaxableValue
                                selectedRowFromDataTable("NonTaxable") = CType(selectedRowFromDataTable("NonTaxable"), Decimal) + RoundingNonTaxableValue
                                RoundingNonTaxableValue = 0
                            End If
                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
                                           CType(l_DataRow("Non-Taxable"), Decimal) + _
                                           CType(l_DataRow("Interest"), Decimal)
                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)
                            l_DataRow.AcceptChanges()
                            selectedRowFromDataTable.AcceptChanges()
                        End If
                    End If
                Next
            End If

            If (RoundingTaxableValue <> 0 And RoundingNonTaxableValue <> 0) Then
                Throw New System.Exception("System is not able to handle COVID Rounding in prorated table")
            End If

            'Adjust the TaxRate amount here from the largest record to match with the expected value. 
            'This assumes that we will be able to adjust all rounding effect in it.
            Dim totalTaxableAmount As Decimal, totalTax As Decimal, taxRate As Decimal
            Dim drLargest As DataRow
            For Each dr As DataRow In DatatableToBeFixed.Rows
                totalTaxableAmount += dr("Taxable")
                totalTax += dr("Tax")
                taxRate = dr("TaxRate")
                If drLargest Is Nothing OrElse drLargest("Taxable") < dr("Taxable") Then
                    drLargest = dr
                End If
            Next
            Dim expectedTaxAmount As Decimal = Math.Round(totalTaxableAmount * taxRate / 100.0, 2)
            If expectedTaxAmount <> totalTax Then
                drLargest("Tax") = expectedTaxAmount - (totalTax - drLargest("Tax"))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    'END : ML  | 2020.05.05 | YRS-AT-4874 | Fix the rounding issue for Covid Amounts 

    Private Function FixedRoundingRMD(ByVal DatatableToBeFixed As DataTable, ByVal RmdTaxable As Decimal, ByVal RmdNonTaxble As Decimal, ByVal paraPlanType As String) As DataTable

        Dim datarow As Integer
        Dim totalReqAmount As Decimal
        Dim RoundingTaxableValue As Decimal
        Dim RoundingNonTaxableValue As Decimal

        Dim requestedamount As Decimal
        Dim totalTaxableReqAmount As Decimal
        Dim totalNonTaxableReqAmount As Decimal
        Dim l_CurrentAccountDataTable As DataTable
        Dim l_drRow As DataRow

        Try
            requestedamount = RmdTaxable + RmdNonTaxble
            If requestedamount <= 0 Then
                Return DatatableToBeFixed
            End If

            l_CurrentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)

            For Each l_DataRow As DataRow In DatatableToBeFixed.Rows
                totalTaxableReqAmount += l_DataRow("Taxable")
                totalNonTaxableReqAmount += l_DataRow("NonTaxable")
            Next

            If RmdTaxable <> totalTaxableReqAmount Then
                RoundingTaxableValue = RmdTaxable - totalTaxableReqAmount
            End If

            If RmdNonTaxble <> totalNonTaxableReqAmount Then
                RoundingNonTaxableValue = RmdNonTaxble - totalNonTaxableReqAmount
            End If

            If RoundingTaxableValue = 0 AndAlso RoundingNonTaxableValue = 0 Then
                Return DatatableToBeFixed
            End If

            If RoundingTaxableValue <> 0 Then
                l_drRow = DatatableToBeFixed.Rows(0)
                l_drRow.BeginEdit()
                If l_drRow("Taxable") > 0 Then
                    l_drRow("Taxable") = l_drRow("Taxable") + RoundingTaxableValue
                End If
            End If

            If RoundingNonTaxableValue <> 0 Then
                l_drRow = DatatableToBeFixed.Rows(0)
                l_drRow.BeginEdit()
                If l_drRow("NonTaxable") > 0 Then
                    l_drRow("NonTaxable") = l_drRow("NonTaxable") + RoundingNonTaxableValue
                End If

            End If
            If RoundingTaxableValue <> 0 Then
                For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
                    If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                        If l_DataRow("PlanType") = paraPlanType Then
                            If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" AndAlso CType(l_DataRow("Taxable"), Decimal) > 0 AndAlso RoundingTaxableValue <> 0 Then
                                l_DataRow("Taxable") = CType(l_DataRow("Taxable"), Decimal) - RoundingTaxableValue
                                RoundingTaxableValue = 0
                            ElseIf l_DataRow("Interest").GetType.ToString <> "System.DBNull" AndAlso CType(l_DataRow("Interest"), Decimal) > 0 AndAlso RoundingTaxableValue <> 0 Then
                                l_DataRow("Interest") = l_DataRow("Interest") - RoundingTaxableValue
                                RoundingTaxableValue = 0
                            ElseIf l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" AndAlso CType(l_DataRow("YMCATaxable"), Decimal) > 0 AndAlso RoundingTaxableValue <> 0 Then
                                l_DataRow("YMCATaxable") = CType(l_DataRow("YMCATaxable"), Decimal) - RoundingTaxableValue
                                RoundingTaxableValue = 0
                            ElseIf l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" AndAlso CType(l_DataRow("YMCAInterest"), Decimal) > 0 AndAlso RoundingTaxableValue <> 0 Then
                                l_DataRow("YMCAInterest") = l_DataRow("YMCAInterest") - RoundingTaxableValue
                                RoundingTaxableValue = 0
                            End If
                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
                                                CType(l_DataRow("Interest"), Decimal)

                            l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)
                            l_DataRow.AcceptChanges()
                        End If

                    End If
                Next
            End If


            If RoundingNonTaxableValue <> 0 Then
                For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
                    If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                        If l_DataRow("PlanType") = paraPlanType Then
                            If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" AndAlso CType(l_DataRow("Non-Taxable"), Decimal) > 0 AndAlso RoundingNonTaxableValue <> 0 Then
                                l_DataRow("Non-Taxable") = CType(l_DataRow("Taxable"), Decimal) - RoundingNonTaxableValue
                                RoundingNonTaxableValue = 0
                            End If
                            l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
                                                CType(l_DataRow("Interest"), Decimal)
                            l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)
                            l_DataRow.AcceptChanges()
                        End If

                    End If
                Next
            End If


            Session("CalculatedDataTableForCurrentAccounts_C19") = l_CurrentAccountDataTable

            Return DatatableToBeFixed

        Catch ex As Exception

        End Try
    End Function

#End Region

    Public Function DoMinimumDistributionCalculation(ByVal MinimumDistributionAmount As Decimal, ByVal plantype As String, ByVal MRDforPartialorMarket As Boolean) As String
        'modified the code for MRD issue-Amit Nigam

        Dim l_MinimumDistributionTable As DataTable
        Dim l_CurrentAccountDataTable As DataTable
        Dim l_DataTable As DataTable

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersoanlPostTax As Decimal
        Dim l_Decimal_PersonalInterest As Decimal
        Dim l_Decimal_PersonalTotal As Decimal

        Dim l_Decimal_YMCAPreTax As Decimal
        Dim l_Decimal_YMCAInterest As Decimal
        Dim l_Decimal_YMCATotal As Decimal

        Dim l_Decimal_TaxRate As Decimal

        Dim l_Decimal_MinimumDistributionAmount As Decimal

        Dim l_Boolean_IsMinimumDistribution As Boolean
        Dim l_MinimumdistributionDataRow As DataRow
        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal

        Try

            l_DataTable = DirectCast(Session("AtsRefund_C19"), DataTable)

            'Neeraj BT- 563: 
            If Not Session("MinimumDistributionTable_C19") Is Nothing Then
                l_MinimumDistributionTable = CType(Session("MinimumDistributionTable_C19"), DataTable)
            Else
                If Not l_DataTable Is Nothing Then
                    l_MinimumDistributionTable = l_DataTable.Clone
                Else
                    Return 0
                End If
            End If
            'Neeraj BT- 563: 

            l_CurrentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)


            If l_CurrentAccountDataTable Is Nothing Then Return 0

            If Me.MinimumDistributionAmount = 0 Then Return 0

            '' Assign the values 
            'modified the code for MRD issue-Amit Nigam
            Me.MinimumDistributionAmount = MinimumDistributionAmount
            'modified the code for MRD issue-Amit Nigam
            l_Decimal_MinimumDistributionAmount = Me.MinimumDistributionAmount
            Me.MinimumDistributionAmount = 0.0


            For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows
                'modified the code for MRD issue-Amit Nigam
                If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                    If l_DataRow("PlanType") = plantype Or MRDforPartialorMarket = False Then
                        'modified the code for MRD issue-Amit Nigam
                        If l_Decimal_MinimumDistributionAmount = 0 Then Exit For

                        '' Get the all values to Caluclate.

                        If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_PersonalPreTax = DirectCast(l_DataRow("Taxable"), Decimal)
                        Else
                            l_Decimal_PersonalPreTax = 0
                        End If

                        If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_PersoanlPostTax = DirectCast(l_DataRow("Non-Taxable"), Decimal)
                        Else
                            l_Decimal_PersoanlPostTax = 0
                        End If

                        If l_DataRow("Interest").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_PersonalInterest = DirectCast(l_DataRow("Interest"), Decimal)
                        Else
                            l_Decimal_PersonalInterest = 0
                        End If

                        If l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_YMCAPreTax = DirectCast(l_DataRow("YMCATaxable"), Decimal)
                        Else
                            l_Decimal_YMCAPreTax = 0
                        End If

                        If l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" Then
                            l_Decimal_YMCAInterest = DirectCast(l_DataRow("YMCAInterest"), Decimal)
                        Else
                            l_Decimal_YMCAInterest = 0
                        End If


                        '' Start calculation .. 

                        If (l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest + l_Decimal_YMCAPreTax + l_Decimal_YMCAInterest + l_Decimal_PersoanlPostTax) > 0 Then ''Main If

                            l_Boolean_IsMinimumDistribution = False

                            '' For Personal Taxable
                            If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalPreTax Then


                                l_Boolean_IsMinimumDistribution = True
                                l_Decimal_Taxable = l_Decimal_PersonalPreTax
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_PersonalPreTax
                                l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersonalPreTax
                                l_DataRow("Emp.Total") = DirectCast(l_DataRow("Taxable"), Decimal) + _
                                                     DirectCast(l_DataRow("Non-Taxable"), Decimal) + _
                                                     DirectCast(l_DataRow("Interest"), Decimal)

                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + _
                                                    DirectCast(l_DataRow("YMCATotal"), Decimal)

                            Else


                                l_Boolean_IsMinimumDistribution = True
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

                                l_DataRow("Taxable") = l_DataRow("Taxable") - l_Decimal_MinimumDistributionAmount

                                l_DataRow("Emp.Total") = DirectCast(l_DataRow("Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Non-Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Interest"), Decimal)

                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + _
                                                    DirectCast(l_DataRow("YMCATotal"), Decimal)



                                l_Decimal_Taxable = l_Decimal_MinimumDistributionAmount
                                l_Decimal_MinimumDistributionAmount = 0.0

                            End If


                            '' For Personal Interest
                            If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalInterest Then



                                l_Boolean_IsMinimumDistribution = True
                                l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_PersonalInterest

                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_PersonalInterest
                                l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersonalInterest

                                l_DataRow("Interest") = "0.00"

                                l_DataRow("Emp.Total") = DirectCast(l_DataRow("Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Non-Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Interest"), Decimal)

                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + _
                                                    DirectCast(l_DataRow("YMCATotal"), Decimal)


                            Else



                                l_Boolean_IsMinimumDistribution = True
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

                                l_DataRow("Interest") = l_DataRow("Interest") - l_Decimal_MinimumDistributionAmount

                                l_DataRow("Emp.Total") = DirectCast(l_DataRow("Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Non-Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Interest"), Decimal)

                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)


                                l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount
                                l_Decimal_MinimumDistributionAmount = 0.0

                            End If
                            '' For Personal Non-Taxable
                            If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersoanlPostTax Then


                                l_Boolean_IsMinimumDistribution = True
                                l_Decimal_NonTaxable = l_Decimal_PersoanlPostTax
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_PersoanlPostTax
                                l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersoanlPostTax
                                l_DataRow("Emp.Total") = DirectCast(l_DataRow("Taxable"), Decimal) + _
                                                     DirectCast(l_DataRow("Non-Taxable"), Decimal) + _
                                                     DirectCast(l_DataRow("Interest"), Decimal)

                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + _
                                                    DirectCast(l_DataRow("YMCATotal"), Decimal)

                            Else


                                l_Boolean_IsMinimumDistribution = True
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

                                l_DataRow("Non-Taxable") = l_DataRow("Non-Taxable") - l_Decimal_MinimumDistributionAmount

                                l_DataRow("Emp.Total") = DirectCast(l_DataRow("Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Non-Taxable"), Decimal) + _
                                                    DirectCast(l_DataRow("Interest"), Decimal)

                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + _
                                                    DirectCast(l_DataRow("YMCATotal"), Decimal)



                                l_Decimal_PersoanlPostTax = l_Decimal_MinimumDistributionAmount
                                l_Decimal_MinimumDistributionAmount = 0.0

                            End If

                            ''For YMCA Taxable.

                            If l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAPreTax Then


                                l_Boolean_IsMinimumDistribution = True
                                l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_YMCAPreTax
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_YMCAPreTax

                                l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_YMCAPreTax

                                l_DataRow("YMCATaxable") = "0.00"

                                l_DataRow("YMCATotal") = DirectCast(l_DataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)

                            Else


                                l_Boolean_IsMinimumDistribution = True
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

                                l_DataRow("YMCATaxable") = l_DataRow("YMCATaxable") - l_Decimal_MinimumDistributionAmount

                                l_DataRow("YMCATotal") = DirectCast(l_DataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)

                                l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount
                                l_Decimal_MinimumDistributionAmount = 0.0

                            End If

                            If l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAInterest Then


                                l_Boolean_IsMinimumDistribution = True

                                l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_YMCAInterest
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_YMCAInterest
                                l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_YMCAInterest

                                l_DataRow("YMCAInterest") = "0.00"

                                l_DataRow("YMCATotal") = DirectCast(l_DataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)


                            Else

                                l_Boolean_IsMinimumDistribution = True
                                Me.MinimumDistributionAmount = Me.MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount

                                l_DataRow("YMCAInterest") = l_Decimal_YMCAInterest - l_Decimal_MinimumDistributionAmount

                                l_DataRow("YMCATotal") = DirectCast(l_DataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                l_DataRow("Total") = DirectCast(l_DataRow("Emp.Total"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)

                                l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount
                                l_Decimal_MinimumDistributionAmount = 0

                            End If

                            '' This Values has to be updated in the Table.

                            If l_Boolean_IsMinimumDistribution = True Then

                                ''Get TaxRate

                                l_MinimumdistributionDataRow = l_MinimumDistributionTable.NewRow

                                l_MinimumdistributionDataRow("Taxable") = l_Decimal_Taxable
                                l_MinimumdistributionDataRow("TaxRate") = Me.MinDistributionTaxRate
                                l_MinimumdistributionDataRow("Tax") = Math.Round(l_Decimal_Taxable * (Me.MinDistributionTaxRate / 100.0), 2)
                                l_MinimumdistributionDataRow("NonTaxable") = l_Decimal_PersoanlPostTax '"0.00"
                                l_MinimumdistributionDataRow("AcctType") = l_DataRow("AccountType")
                                l_MinimumdistributionDataRow("Payee") = Me.Payee1Name
                                l_MinimumdistributionDataRow("FundedDate") = System.DBNull.Value
                                l_MinimumdistributionDataRow("RefRequestsID") = Me.SessionRefundRequestID


                                l_MinimumDistributionTable.Rows.Add(l_MinimumdistributionDataRow)

                            End If



                            'modified the code for MRD issue-Amit Nigam
                        End If
                    End If
                    'modified the code for MRD issue-Amit Nigam
                End If '' Main if

            Next


            If l_Decimal_MinimumDistributionAmount > Me.MinimumDistributionAmount Then

                Return (" Minimum Distrubtion Not Met.  A Minimum Distribution of " + Math.Round(Me.MinimumDistributionAmount, 2).ToString + " is Required.. There isn't enough Taxable Money to Roll any over.")

            End If

            Session("MinimumDistributionTable_C19") = l_MinimumDistributionTable
            l_CurrentAccountDataTable.AcceptChanges()

            Session("CalculatedDataTableForCurrentAccounts_C19") = l_CurrentAccountDataTable

        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Function CheckForTerminationDate() As Boolean

        Dim l_DataTable As DataTable = Nothing
        Dim l_DataRow As DataRow
        Dim l_DateTime As DateTime
        Dim l_TerminationDate As DateTime

        Try

            l_DataTable = DirectCast(Session("Member Employment_C19"), DataTable)

            If l_DataTable Is Nothing Then Return False

            For Each l_DataRow In l_DataTable.Rows

                If Not l_DataRow("TermDate").GetType.ToString = "System.DBNull" Then

                    If DateTime.Compare(l_TerminationDate, CType(l_DataRow("TermDate"), DateTime)) < 0 Then
                        l_TerminationDate = CType(l_DataRow("TermDate"), DateTime)
                    End If

                End If

            Next

            If Now.Year >= l_TerminationDate.Year And (Convert.ToString(Now.Month) + Convert.ToString(Now.Day)) > "0331" Then
                Return True
            Else
                Return False
            End If


        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region 'Calculation of Minimum Distribution Amount
    Public Function CreatePayees()
        Dim l_DataColumn As DataColumn

        Dim l_CurrentDataTable As DataTable
        Dim l_PayeeDataTable As DataTable

        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee3DataTable As DataTable

        Dim l_PayeeDataRow As DataRow

        Dim l_AccountType As String

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalInterest As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal
        Dim l_Decimal_YMCAInterest As Decimal
        Dim l_RequestedDataTable As DataTable
        Dim l_AccountGroup As String = ""
        Dim l_bool_AMMatched As Boolean = True
        Dim l_bool_TMMatched As Boolean = True
        Dim l_dataTable As DataTable
        Me.inTaxable = 0.0
        Me.inNonTaxable = 0.0
        Me.inGross = 0.0

        Me.inVolAcctsUsed = 0.0
        Me.inVolAcctsAvailable = 0.0

        Try
            l_RequestedDataTable = DirectCast(Session("RequestedAccounts_C19"), DataTable)
            For Each dr As DataRow In l_RequestedDataTable.Rows
                If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                    If dr("AccountType") = "AM" Then
                        If dr("YMCATotal") = 0 Then
                            l_bool_AMMatched = False
                        End If
                    End If
                    If dr("AccountType") = "TM" Then
                        If dr("YMCATotal") = 0 Then
                            l_bool_TMMatched = False
                        End If
                    End If
                End If
            Next

            'Commnented By Parveen on 07-Dec-2009 to get the data according to the withdrawal Type from Database
            'If Me.ISMarket = -1 Then
            '    l_CurrentDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarket(Me.FundID, Session("RefundRequestID"))
            'Else
            '    l_CurrentDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            'End If
            'Commnented By Parveen on 07-Dec-2009 to get the data according to the withdrawal Type from Database

            'commented for the MRD issue reported by Mark-28 jan,2010
            'l_CurrentDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarket(Me.FundID, Session("RefundRequestID"))
            'commented for the MRD issue reported by Mark-28 jan,2010
            l_CurrentDataTable = Session("CalculatedDataTableForCurrentAccounts_C19")


            For Each dr As DataRow In l_CurrentDataTable.Rows
                If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                    If dr("AccountType") = "AM" Then
                        If l_bool_AMMatched = False Then
                            dr("YMCATaxable") = "0.00"
                            dr("YMCAInterest") = "0.00"
                            dr("YMCATotal") = "0.00"
                        End If
                    End If
                    If dr("AccountType") = "TM" Then
                        If l_bool_TMMatched = False Then
                            dr("YMCATaxable") = "0.00"
                            dr("YMCAInterest") = "0.00"
                            dr("YMCATotal") = "0.00"
                        End If
                    End If
                End If
            Next


            l_PayeeDataTable = DirectCast(Session("AtsRefund_C19"), DataTable)


            If (Not l_CurrentDataTable Is Nothing) And (Not l_PayeeDataTable Is Nothing) Then

                l_Payee1DataTable = l_PayeeDataTable.Clone
                l_Payee2DataTable = l_PayeeDataTable.Clone
                l_Payee3DataTable = l_PayeeDataTable.Clone
                'Commnetd by Ganeswar Sahoo for Account Group Need to add in Payee data table on 14-09-2009
                '  If Me.RefundType.Trim.ToUpper = "HARD" Then
                'Commnetd by Ganeswar Sahoo for Account Group Need to add in Payee data table on 14-09-2009
                l_DataColumn = New DataColumn("Use", System.Type.GetType("System.Boolean"))
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
                l_DataColumn.DefaultValue = "False"
                '' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

                l_Payee1DataTable.Columns.Add(l_DataColumn)

                l_DataColumn = New DataColumn("Use", System.Type.GetType("System.Boolean"))
                l_DataColumn.DefaultValue = "False"


                l_Payee2DataTable.Columns.Add(l_DataColumn)

                l_DataColumn = New DataColumn("Use", System.Type.GetType("System.Boolean"))
                l_DataColumn.DefaultValue = "False"

                l_Payee3DataTable.Columns.Add(l_DataColumn)


                l_DataColumn = New DataColumn("AccountGroup", System.Type.GetType("System.String"))
                l_DataColumn.DefaultValue = m_const_SavingsPlan_TD


                l_Payee1DataTable.Columns.Add(l_DataColumn)

                l_DataColumn = New DataColumn("AccountGroup", System.Type.GetType("System.String"))
                l_DataColumn.DefaultValue = m_const_SavingsPlan_TD

                l_Payee2DataTable.Columns.Add(l_DataColumn)

                l_DataColumn = New DataColumn("AccountGroup", System.Type.GetType("System.String"))
                l_DataColumn.DefaultValue = m_const_SavingsPlan_TD

                l_Payee3DataTable.Columns.Add(l_DataColumn)
                'Commnetd by Ganeswar Sahoo for Account Group Need to add in Payee data table on 14-09-2009
                '   End If
                'Commnetd by Ganeswar Sahoo for Account Group Need to add in Payee data table on 14-09-2009
                'Added Ganeswar 10thApril09 for BA Account Phase-V /begin


                'Added Amit 20thAugust 09 for for interechanging dataTable for partial withdrawal.
                'Commnented By Parveen on 07-Dec-2009 to get the data according to the withdrawal Type from Database
                'If Me.RefundType = "PART" Then
                '    l_dataTable = l_RequestedDataTable
                'Else
                '    l_dataTable = l_CurrentDataTable 'New 
                'End If
                l_dataTable = l_CurrentDataTable
                'Commnented By Parveen on 07-Dec-2009 to get the data according to the withdrawal Type from Database

                ''Added By Ganeswar Sahoo for MKT on 16-10-2009
                'If Me.ISMarket = -1 Then
                '    l_dataTable = Session("m_DataTableMarketDisplayPayeeGrid")
                'End If
                ''Added By Ganeswar Sahoo for MKT on 16-10-2009

                'Added Amit 20thAugust 09 for for interechanging dataTable for partial withdrawal.
                If l_RequestedDataTable.Rows.Count > 0 Then
                    'For Each l_DataRow As DataRow In l_RequestedDataTable.Rows
                    For Each l_DataRow As DataRow In l_dataTable.Rows
                        'Added Ganeswar 10thApril09 for BA Account Phase-V /End

                        '' CHECK ISSUE
                        If Not l_DataRow("AccountType").GetType.ToString.Trim = "System.DBNull" Then

                            l_AccountType = DirectCast(l_DataRow("AccountType"), String).Trim
                            If Not l_DataRow("AccountGroup").GetType.ToString.Trim = "System.DBNull" Then
                                l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper
                            End If


                            If (l_AccountType = "") Or (l_AccountType = "Total") Then
                            Else
                                If Me.RefundType.Trim.ToUpper = "HARD" And (l_AccountGroup = m_const_SavingsPlan_TD Or l_AccountGroup = m_const_SavingsPlan_TM) Then
                                Else
                                    'Added By Ganeswar Sahoo for MKT on 16-10-2009
                                    'Me.IsExistInRequestedAccounts(l_AccountType.Trim.ToUpper) = True
                                    'Commented by Dilip for MKT on 26-10-2009
                                    If Me.IsExistInRequestedAccounts(l_AccountType.Trim.ToUpper) = True Then 'Or l_AccountType.ToString.ToUpper() = "MB" Then
                                        'Commented by Dilip for MKT on 26-10-2009
                                        'Added By Dilip for MKT on 26-10-2009
                                        '    If Me.IsExistInRequestedAccounts(l_AccountType.Trim.ToUpper) = True Or l_AccountType.ToString.ToUpper() = "MR" Or l_AccountType.ToString.ToUpper() = "MS" Then  'Added By Dilip for MKT on 26-10-2009
                                        'Added By Ganeswar Sahoo for MKT on 16-10-2009

                                        ''l_Payee1DataTable.ImportRow(l_DataRow)

                                        l_PayeeDataRow = l_Payee1DataTable.NewRow


                                        If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                                            l_Decimal_PersonalPreTax = 0.0
                                        Else
                                            l_Decimal_PersonalPreTax = DirectCast(l_DataRow("Taxable"), Decimal)
                                        End If

                                        If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                                            l_Decimal_PersonalInterest = 0.0
                                        Else
                                            l_Decimal_PersonalInterest = DirectCast(l_DataRow("Interest"), Decimal)
                                        End If
                                        'Added Ganeswar 08july 09 for BA Account Phase-V /Start
                                        l_Decimal_YMCAPreTax = 0.0
                                        l_Decimal_YMCAInterest = 0.0

                                        'Added By Dilip for MKT on 26-10-2009
                                        If (l_AccountType.ToString.ToUpper() = "AM" And l_bool_AMMatched = True) Or _
                                            (l_AccountType.ToString.ToUpper() = "TM" And l_bool_TMMatched = True) Or _
                                           IncludeorExcludeYMCAMoney(l_AccountGroup.ToString.ToUpper(), l_AccountType.ToString.ToUpper()) = True Then

                                            'If (l_AccountType.ToString.ToUpper() = "AM" And l_bool_AMMatched = True) Or _
                                            '    (l_AccountType.ToString.ToUpper() = "TM" And l_bool_TMMatched = True) Or _
                                            '    (l_AccountType.ToString.ToUpper() = "MR" Or l_AccountType.ToString.ToUpper() = "MS") Or _
                                            '   IncludeorExcludeYMCAMoney(l_AccountGroup.ToString.ToUpper(), l_AccountType.ToString.ToUpper()) = True Then
                                            'Added By Ganeswar Sahoo for MKT on 16-10-2009
                                            If l_DataRow("YMCATaxable").GetType.ToString = "System.DBNull" Then
                                                l_Decimal_YMCAPreTax = 0.0
                                            Else
                                                l_Decimal_YMCAPreTax = DirectCast(l_DataRow("YMCATaxable"), Decimal)
                                            End If

                                            If l_DataRow("YMCAInterest").GetType.ToString = "System.DBNull" Then
                                                l_Decimal_YMCAInterest = 0.0
                                            Else
                                                l_Decimal_YMCAInterest = DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                            End If
                                        End If

                                        l_PayeeDataRow("AcctType") = l_DataRow("AccountType")
                                        l_PayeeDataRow("Taxable") = l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax
                                        l_PayeeDataRow("NonTaxable") = l_DataRow("Non-Taxable")
                                        l_PayeeDataRow("TaxRate") = Me.TaxRate
                                        l_PayeeDataRow("Tax") = (l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax) * (Me.TaxRate / 100.0)
                                        l_PayeeDataRow("Payee") = Me.Payee1Name
                                        l_PayeeDataRow("FundedDate") = System.DBNull.Value
                                        l_PayeeDataRow("RequestType") = Me.RefundType
                                        l_PayeeDataRow("RefRequestsID") = Me.SessionRefundRequestID

                                        'Commnetd by Ganeswar Sahoo for Account Group Need to add in Payee data table on 14-09-2009
                                        'If Me.RefundType.Trim.ToUpper = "HARD" Then
                                        'l_PayeeDataRow("Use") = "True"
                                        'l_PayeeDataRow("AccountGroup") = l_DataRow("AccountGroup")
                                        'End If


                                        l_PayeeDataRow("Use") = "True"
                                        l_PayeeDataRow("AccountGroup") = l_DataRow("AccountGroup")

                                        'Commnetd by Ganeswar Sahoo for Account Group Need to add in Payee data table on 14-09-2009
                                        l_Payee1DataTable.Rows.Add(l_PayeeDataRow)
                                        If Me.RefundType.Trim.ToUpper = "HARD" Then
                                            Me.inTaxable += DirectCast(l_PayeeDataRow("Taxable"), Decimal)
                                            Me.inNonTaxable += DirectCast(l_PayeeDataRow("NonTaxable"), Decimal)
                                            Me.inGross += DirectCast(l_PayeeDataRow("Taxable"), Decimal) + DirectCast(l_PayeeDataRow("NonTaxable"), Decimal)

                                            Me.inVolAcctsUsed += DirectCast(l_PayeeDataRow("Taxable"), Decimal) + DirectCast(l_PayeeDataRow("NonTaxable"), Decimal)
                                            Me.inVolAcctsAvailable += DirectCast(l_PayeeDataRow("Taxable"), Decimal) + DirectCast(l_PayeeDataRow("NonTaxable"), Decimal)

                                        End If
                                    End If
                                End If

                            End If
                        End If
                    Next
                End If



                'Added  by Ganeswar For HardShip RollOver To avoid TD Account on 25-05-2009
                If Me.PersonAge < 59.06 And Me.RefundType.Trim.ToUpper = "VOL" And Me.IsTerminated = False Then
                    Dim l_PayeeDataRows As DataRow
                    If l_Payee1DataTable.Rows.Count > 0 Then
                        'Modified the code to remove the TD Account 
                        For i As Integer = 0 To l_Payee1DataTable.Rows.Count - 1
                            l_PayeeDataRows = l_Payee1DataTable.Rows(i)
                            If l_PayeeDataRows("AcctType") = "TD" Then
                                l_Payee1DataTable.Rows(i).Delete()
                                Exit For
                            End If
                        Next
                        'Modified the code to remove the TM Account 
                        For i As Integer = 0 To l_Payee1DataTable.Rows.Count - 1
                            l_PayeeDataRows = l_Payee1DataTable.Rows(i)
                            If l_PayeeDataRows("AcctType") = "TM" Then
                                l_Payee1DataTable.Rows(i).Delete()
                                Exit For
                            End If
                        Next
                    End If
                End If
                'Added  by Ganeswar For HardShip RollOver TD Account on 25-05-2009
                Session("Payee1DataTable_C19") = l_Payee1DataTable
                Session("Payee2DataTable_C19") = l_Payee2DataTable
                Session("Payee3DataTable_C19") = l_Payee3DataTable
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function IsExistInRequestedAccounts(ByVal parameterAccountType As String) As Boolean


        Dim l_RequestedDataTable As DataTable

        Dim l_FoundRows() As DataRow
        Dim l_QueryString As String

        Try

            l_RequestedDataTable = DirectCast(Session("RequestedAccounts_C19"), DataTable)


            If Not l_RequestedDataTable Is Nothing Then
                l_QueryString = "AccountType = '" & parameterAccountType.Trim.ToUpper & "'"

                l_FoundRows = l_RequestedDataTable.Select(l_QueryString)

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If


            Else
                Return False
            End If



        Catch ex As Exception
            Throw
        End Try

    End Function
    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1746:Include partial withdrawals 
    'Added two parameter for partial withdrawal
    'Public Function FixRoundingIssue(ByVal DatatableToBeFixed As DataTable, ByVal requestedamount As Decimal, ByVal parameterPlanType As String, Optional ByVal bIsPartial As Boolean = False, Optional ByVal strTerminated As String = "")
    '2013.Oct.23 : Dinesh Kanojia(DK): BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
    'Remove teo paramter for partial withdrawal
    Public Function FixRoundingIssue(ByVal DatatableToBeFixed As DataTable, ByVal requestedamount As Decimal, ByVal parameterPlanType As String)
        Dim datarow As Integer
        Dim totalReqAmount As Decimal
        Dim RoundingValue As Decimal
        Dim NonTaxable As Decimal
        Dim Taxable As Decimal
        Dim Interest As Decimal

        Dim l_dr_NewRowRefundRequestTable As DataRow

        Try
            If requestedamount.GetType.ToString <> "System.DBNull" Then
                For datarow = 0 To DatatableToBeFixed.Rows.Count - 1
                    '------ Modified for WebServiece by pavan ---- Begin  -----------
                    'totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("Total")

                    'AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to exclude select checkbox for ln & ld accounts while computations
                    If (DatatableToBeFixed.Rows(datarow).Item("Total").GetType.ToString <> "System.DBNull") AndAlso (DatatableToBeFixed.Rows(datarow).Item("AccountType").ToString.ToUpper.Trim <> "LN") AndAlso (DatatableToBeFixed.Rows(datarow).Item("AccountType").ToString.ToUpper.Trim <> "LD") Then
                        'IB:As on 21/June/2010 BT-545:Casting problem
                        'Code Commented : 2013.Oct.23 : Dinesh Kanojia(DK): BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                        'If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + DirectCast(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                        'Added by Dinesh on 29/07/2013
                        'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1746:Include partial withdrawals
                        'If bIsPartial Then
                        '    If (CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") And (CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "TD") Then
                        '        totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("Total")
                        '    End If
                        'Else
                        '    If (CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                        '        totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("Total")
                        '    End If
                        'End If
                        'Code Added : 2013.Oct.23 : Dinesh Kanojia(DK): BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                        If (CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((CType(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                            totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("Total")
                        End If
                        'End: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                    End If
                    '------ Modified for WebServiece by pavan ---- END    -----------
                Next
                'Code Commented : '2013.Oct.23 : Dinesh Kanojia(DK): BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                'Added by Dinesh on 29/07/2013
                'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                'If Not bIsPartial Then
                '    If Not requestedamount = totalReqAmount Then
                '        RoundingValue = requestedamount - totalReqAmount
                '    End If
                'Else
                '    RoundingValue = 0
                'End If
                'Code Added : '2013.Oct.23 : Dinesh Kanojia(DK): BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                If Not requestedamount = totalReqAmount Then
                    RoundingValue = requestedamount - totalReqAmount
                End If
                'End: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                If RoundingValue <> 0 Then
                    l_dr_NewRowRefundRequestTable = DatatableToBeFixed.Rows(0)
                    l_dr_NewRowRefundRequestTable.BeginEdit()
                    If l_dr_NewRowRefundRequestTable("Taxable") > 0 Then
                        l_dr_NewRowRefundRequestTable("Taxable") = l_dr_NewRowRefundRequestTable("Taxable") + RoundingValue
                    ElseIf l_dr_NewRowRefundRequestTable("Non-Taxable") > 0 Then
                        l_dr_NewRowRefundRequestTable("Non-Taxable") = l_dr_NewRowRefundRequestTable("Non-Taxable") + RoundingValue
                    ElseIf l_dr_NewRowRefundRequestTable("Interest") > 0 Then
                        l_dr_NewRowRefundRequestTable("Interest") = l_dr_NewRowRefundRequestTable("Interest") + RoundingValue
                    End If

                    l_dr_NewRowRefundRequestTable("Total") = l_dr_NewRowRefundRequestTable("Total") + RoundingValue
                    l_dr_NewRowRefundRequestTable.EndEdit()

                    '-------Added by imran Imran on 15/04/2010  Rounding Issue generated at the time of partial amt
                    For datarow = 0 To DatatableToBeFixed.Rows.Count - 1

                        'AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to exclude select checkbox for ln & ld accounts while computations
                        If (DatatableToBeFixed.Rows(datarow).Item("Total").GetType.ToString <> "System.DBNull") AndAlso (DatatableToBeFixed.Rows(datarow).Item("AccountType").ToString.ToUpper.Trim <> "LN") AndAlso (DatatableToBeFixed.Rows(datarow).Item("AccountType").ToString.ToUpper.Trim <> "LD") Then
                            'IB:As on 21/June/2010 BT-545:Casting problem
                            'Code Commented : 2013.Oct.23 : Dinesh Kanojia(DK): BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                            ' If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + DirectCast(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then

                            'Added by Dinesh on 29/07/2013
                            'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                            'If bIsPartial And strTerminated.ToUpper.Trim = "YES" Then
                            '    If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                            '        Taxable += DatatableToBeFixed.Rows(datarow).Item("Taxable")
                            '        NonTaxable += DatatableToBeFixed.Rows(datarow).Item("Non-Taxable")
                            '        Interest += DatatableToBeFixed.Rows(datarow).Item("Interest")
                            '    End If
                            'ElseIf bIsPartial And strTerminated.ToUpper.Trim = "NO" Then
                            '    If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") And (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "TD") Then
                            '        Taxable += DatatableToBeFixed.Rows(datarow).Item("Taxable")
                            '        NonTaxable += DatatableToBeFixed.Rows(datarow).Item("Non-Taxable")
                            '        Interest += DatatableToBeFixed.Rows(datarow).Item("Interest")
                            '    End If
                            'Else
                            '    If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                            '        Taxable += DatatableToBeFixed.Rows(datarow).Item("Taxable")
                            '        NonTaxable += DatatableToBeFixed.Rows(datarow).Item("Non-Taxable")
                            '        Interest += DatatableToBeFixed.Rows(datarow).Item("Interest")
                            '    End If
                            'End If

                            If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim <> "Total") And ((DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim) <> "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                                Taxable += DatatableToBeFixed.Rows(datarow).Item("Taxable")
                                NonTaxable += DatatableToBeFixed.Rows(datarow).Item("Non-Taxable")
                                Interest += DatatableToBeFixed.Rows(datarow).Item("Interest")
                            End If
                            'End: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                        End If

                        'AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to exclude select checkbox for ln & ld accounts while computations
                        If (DatatableToBeFixed.Rows(datarow).Item("Total").GetType.ToString <> "System.DBNull") AndAlso (DatatableToBeFixed.Rows(datarow).Item("AccountType").ToString.ToUpper.Trim <> "LN") AndAlso (DatatableToBeFixed.Rows(datarow).Item("AccountType").ToString.ToUpper.Trim <> "LD") Then
                            If (DirectCast(DatatableToBeFixed.Rows(datarow).Item("AccountType"), String).Trim = "Total") Then
                                DatatableToBeFixed.Rows(datarow).Item("Taxable") = Taxable
                                DatatableToBeFixed.Rows(datarow).Item("Non-Taxable") = NonTaxable
                                DatatableToBeFixed.Rows(datarow).Item("Interest") = Interest
                                DatatableToBeFixed.Rows(datarow).Item("Total") = Taxable + NonTaxable + Interest
                            End If
                        End If
                    Next
                    '-------End by imran


                    If parameterPlanType = "Retirement" Then
                        Session("RetirementAccountTypeAdjusted_C19") = l_dr_NewRowRefundRequestTable("AccountType").ToString().Trim()
                    ElseIf parameterPlanType = "Savings" Then
                        Session("SavingsAccountTypeAdjusted_C19") = l_dr_NewRowRefundRequestTable("AccountType").ToString().Trim()
                    End If
                Else
                    Me.Session("RoundingAmount_C19") = Nothing
                End If
            End If
            Return DatatableToBeFixed
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function FixRoundingIssueProcessing(ByVal DatatableToBeFixed As DataTable, ByVal requestedamount As Decimal, ByVal strType As String)
        Dim datarow As Integer
        Dim totalReqAmount As Decimal
        Dim RoundingValue As Decimal
        Dim l_dr_NewRowRefundRequestTable As DataRow
        'Added to check the deduction for rounding issue for the account type-Amit
        Dim l_datarowforDeduction As Integer
        Dim bool_flagChecked As Boolean = False
        'Added to check the deduction for rounding issue for the account type-Amit
        Try
            If requestedamount.GetType.ToString <> "System.DBNull" Then
                For datarow = 0 To DatatableToBeFixed.Rows.Count - 1
                    If strType = "PayeeDataDrid" Then
                        totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("Taxable")
                        totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("NonTaxable")
                    Else
                        totalReqAmount += DatatableToBeFixed.Rows(datarow).Item("Total")
                        'Added to check the deduction for rounding issue for the account type-Amit
                        If bool_flagChecked = False Then
                            If totalReqAmount > 0.01 Then
                                l_datarowforDeduction = datarow
                                bool_flagChecked = True
                            End If
                        End If
                        'Added to check the deduction for rounding issue for the account type-Amit
                    End If
                Next
                If Not requestedamount = totalReqAmount Then
                    RoundingValue = totalReqAmount - requestedamount
                End If
                If RoundingValue <> 0 Then
                    If DatatableToBeFixed.Rows.Count > 0 Then
                        l_dr_NewRowRefundRequestTable = DatatableToBeFixed.Rows(l_datarowforDeduction)
                    Else
                        l_dr_NewRowRefundRequestTable = DatatableToBeFixed.Rows(0)
                    End If

                    l_dr_NewRowRefundRequestTable.BeginEdit()
                    If strType = "PayeeDataDrid" Then
                        If l_dr_NewRowRefundRequestTable("Taxable") > 0 Then
                            l_dr_NewRowRefundRequestTable("Taxable") = l_dr_NewRowRefundRequestTable("Taxable") - RoundingValue
                        ElseIf l_dr_NewRowRefundRequestTable("NonTaxable") > 0 Then
                            l_dr_NewRowRefundRequestTable("NonTaxable") = l_dr_NewRowRefundRequestTable("NonTaxable") - RoundingValue
                        End If
                    Else
                        If l_dr_NewRowRefundRequestTable("Taxable") > 0 Then
                            l_dr_NewRowRefundRequestTable("Taxable") = l_dr_NewRowRefundRequestTable("Taxable") - RoundingValue
                        ElseIf l_dr_NewRowRefundRequestTable("Non-Taxable") > 0 Then
                            l_dr_NewRowRefundRequestTable("Non-Taxable") = l_dr_NewRowRefundRequestTable("Non-Taxable") - RoundingValue
                        End If
                        l_dr_NewRowRefundRequestTable("Total") = l_dr_NewRowRefundRequestTable("Total") - RoundingValue
                        l_dr_NewRowRefundRequestTable.EndEdit()
                    End If
                End If
            End If
            Return DatatableToBeFixed
        Catch ex As Exception
            Throw
        End Try
    End Function


    'Modified for Plan Split Changes
    Public Function LoadRefundableDataTable()
        Dim l_AccountContributionTable As DataTable
        Dim l_RefundableDataTable As DataTable
        Dim l_DataColumn As DataColumn
        Dim l_DataRow As DataRow
        'Plan Split Variables
        Dim l_datarowarray_RetirementPlan As DataRow()
        Dim l_datarowarray_SavingsPlan As DataRow()
        Dim l_decimal_TotalRetirementPlanMoney As Decimal = 0
        Dim l_decimal_TotalSavingsPlanMoney As Decimal = 0
        Dim i As Integer = 0

        Try

            l_RefundableDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundableDataTable(Me.FundID)

            Me.DoFilterRefundableTable(l_RefundableDataTable)
            If Me.RefundType.Trim.ToUpper() = "REG" Or Me.RefundType.Trim.ToUpper() = "PERS" Then
                Dim l_Bool_Useit As Boolean
                Dim l_Bool_Yside As Boolean
                Dim l_CurrentRow As DataRow
                Me.YMCAAvailableAmount = 0.0
                For Each l_CurrentRow In l_RefundableDataTable.Rows
                    l_Bool_Useit = True
                    l_Bool_Yside = True


                    If IsBasicAccount(l_CurrentRow) Then
                        l_Bool_Useit = True
                        'VPR Commented to include YMCA Side of Money which is less than or equal to MaximumPIAAmount
                        ' If Me.IsVested = True And Me.TerminationPIA <= Me.MaximumPIAAmount Then
                        ' START | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money 
                        If BA_Legacy_combined_Amt_Switch_ON() Then
                            l_Bool_Yside = True
                        Else
                            If Me.IsVested = True And Me.TerminationPIA <= Me.MaximumPIAAmount Then
                                l_Bool_Yside = True
                            Else
                                l_Bool_Yside = False
                            End If
                        End If
                        ' END | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money

                        ' START | SR | 2017.07.03 | YRS-AT-3161 | For QD participants, there will be no termination date hence Termination PIA condition not applicable.
                        If Me.SessionStatusType.Trim.ToUpper() = "QD" And Me.RefundType.Trim.ToUpper() = "REG" Then
                            l_Bool_Yside = True
                        End If
                        ' END | SR | 2017.07.03 | YRS-AT-3161 | For QD participants, there will be no termination date hence Termination PIA condition not applicable.

                        'Ragesh TODO 
                        If Me.IsPersonalOnly Then
                            'Added By Ganeswar for include and exclude the mony for YMCA Account
                            'l_Bool_Yside = False
                            l_Bool_Yside = IncludeorExcludeYMCAMoney(l_CurrentRow("AccountGroup").ToString.ToUpper(), l_CurrentRow("AccountType").ToString.ToUpper())
                            'Added Ganeswar 12thMay09 for BA Account Phase-V /End
                        End If
                        'End By Ganeswar for include and exclude the mony for YMCA Account
                        'Start - Retirement Plan Group
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_AM Then
                        l_Bool_Useit = True
                        l_Bool_Yside = True
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_AP Then
                        l_Bool_Useit = True
                        l_Bool_Yside = False

                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_RP Then
                        l_Bool_Useit = True
                        l_Bool_Yside = False

                        'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                        'on the condition that participant is not active
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_SR Then
                        'If Me.IsVested = True And Me.TerminationPIA < Me.MaximumPIAAmount Then
                        '    l_Bool_Useit = True
                        '    l_Bool_Yside = True
                        'Else
                        '    l_Bool_Useit = False
                        '    l_Bool_Yside = False
                        'End If
                        'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                        'on the condition that participant is not active
                        If Me.SessionStatusType.Trim.ToUpper <> "AE" Then
                            l_Bool_Useit = True
                            l_Bool_Yside = True
                        Else
                            l_Bool_Useit = False
                            l_Bool_Yside = False
                        End If
                        'COmmented by Shubhrata Feb 27th,2007
                        'If Me.IsPersonalOnly Then
                        '    l_Bool_Yside = False
                        'End If
                        'Begin Code Merge by Dilip on 07-05-2009
                        'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new case for Ac Account Type
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_AC Then
                        l_Bool_Useit = True
                        l_Bool_Yside = False
                        'End Code Merge by Dilip on 07-05-2009
                        'End 14-Jan-2009
                        'End - Retirement Plan Group
                        'Start - Savings Plan Group
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_SavingsPlan_TD Then
                        l_Bool_Useit = True
                        l_Bool_Yside = False
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_SavingsPlan_TM Then

                        l_Bool_Useit = True
                        l_Bool_Yside = True
                    ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_SavingsPlan_RT Then
                        l_Bool_Useit = True
                        l_Bool_Yside = False
                        'End  - Savings Plan Group
                    End If

                    If Not l_Bool_Useit Then
                        l_CurrentRow.Delete()
                    End If

                    If Not l_Bool_Yside Then
                        l_CurrentRow("YMCATaxable") = 0.0
                        l_CurrentRow("YMCAInterest") = 0.0
                        l_CurrentRow("Total") = Convert.ToDouble(l_CurrentRow("Total")) - Convert.ToDouble(l_CurrentRow("YMCATotal"))
                        l_CurrentRow("YMCATotal") = 0.0
                    End If
                    Me.YMCAAvailableAmount = Me.YMCAAvailableAmount + l_CurrentRow("YmcaTaxable")
                Next
                l_RefundableDataTable.AcceptChanges()
            End If




            Session("RefundableDataTable_C19") = l_RefundableDataTable


        Catch ex As Exception
            Throw
        End Try
    End Function
    'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
    Public Function IncludeorExcludeYMCAMoney(ByVal parameterAccountGroup, ByVal parameterAccountType) As Boolean
        Dim boolpersonal As Boolean
        Dim boolYMCA As Boolean
        Dim boolPretax As Boolean
        Dim boolPosttax As Boolean
        Dim stringAccountBreakdowntypes As String

        Try
            boolpersonal = False
            boolYMCA = True
            boolPretax = False
            boolPosttax = False

            stringAccountBreakdowntypes = GetAccountBreakDownType(parameterAccountGroup, boolpersonal, boolYMCA, boolPretax, boolPosttax)
            IncludeorExcludeYMCAMoney = YMCARET.YmcaBusinessObject.RefundRequest.IncludeorExcludeYMCAMoney(stringAccountBreakdowntypes, parameterAccountType, Me.SessionRefundRequestID)

        Catch ex As Exception
            Throw

        End Try
    End Function
    'Added Ganeswar 10thApril09 for BA Account Phase-V /End
    Public Function DoFilterRefundableTable(ByVal parameterDataTable As DataTable)

        Dim l_DataRow As DataRow
        Dim l_AccountType As String

        Try
            If Not parameterDataTable Is Nothing Then

                For Each l_DataRow In parameterDataTable.Rows

                    l_AccountType = IIf(l_DataRow("AccountType").GetType.ToString.Trim = "System.DBNull", String.Empty, DirectCast(l_DataRow("AccountType"), String))

                    If Not l_AccountType = String.Empty Then

                        If Me.IsExistInRequestedAccounts(l_AccountType) = False Then
                            l_DataRow.Delete()
                        End If

                    End If
                Next

                '' Accept the Changes, so that all the deleted row will not be appear.
                parameterDataTable.AcceptChanges()

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function LoadAccountBreakDown()

        Dim l_DataTable As DataTable
        Try

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetAccountBreakDown()

            Session("AccountBreakDown_C19") = l_DataTable

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function LoadCalculatedTableForPersonalMonies()


        Dim l_CalculatedDataTableForCurrentAccounts As DataTable
        Dim l_CalculatedDataTableForPersonal As DataTable
        Dim l_DataRow As DataRow
        Try

            l_CalculatedDataTableForCurrentAccounts = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            l_CalculatedDataTableForPersonal = l_CalculatedDataTableForCurrentAccounts.Clone
            For Each l_DataRow In l_CalculatedDataTableForCurrentAccounts.Rows
                l_CalculatedDataTableForPersonal.ImportRow(l_DataRow)
            Next

            For Each l_DataRow In l_CalculatedDataTableForPersonal.Rows
                'Added to stop reducing the Ymca account amount from the table-Amit 15-09-09
                If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then
                    If DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_RG Or _
                           DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_SS Or _
                           DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_SA Or _
                           DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_BA Then
                        l_DataRow("YMCATaxable") = 0
                        l_DataRow("YMCAInterest") = 0
                        l_DataRow("YMCATotal") = 0
                    End If
                End If
                'Added to stop reducing the Ymca account amount from the tabel-Amit 15-09-09
            Next
            Session("CalculatedDataTableForPersonalMonies_C19") = l_CalculatedDataTableForPersonal
            Session("PermanentDataTableForPersonalMonies_C19") = l_CalculatedDataTableForPersonal

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function LoadDeductions(ByVal parameterDataGridDeductions As DataGrid)

        Dim l_DataTable As DataTable

        Try


            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetDeductions


            parameterDataGridDeductions.DataSource = l_DataTable
            parameterDataGridDeductions.DataBind()


            Session("DeductionsDataTable_C19") = l_DataTable


        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region 'Load Functions
#Region "Refund Request Functions"
    Public Function CalculateTotal(ByVal l_CalculatedDataTable As DataTable) As DataTable

        ' Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False

        Try

            If Not l_CalculatedDataTable Is Nothing Then

                ' If the field Total is already exist in the Table then Delete the Row.
                For Each l_DataRow In l_CalculatedDataTable.Rows
                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then
                        If DirectCast(l_DataRow("AccountType"), String) = "Total" Then
                            l_BooleanFlag = True
                            Exit For
                        End If
                    End If
                Next

                If l_BooleanFlag = False Then
                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                Else
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("Non-Taxable") = "0.00"
                    l_DataRow("Interest") = "0.00"
                    l_DataRow("Emp.Total") = "0.00"
                    l_DataRow("YMCATaxable") = "0.00"
                    l_DataRow("YMCAInterest") = "0.00"
                    l_DataRow("YMCATotal") = "0.00"
                    l_DataRow("Total") = "0.00"
                End If

                If Me.RefundType = "VOL" Then
                    l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "Selected=True")
                    l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "Selected=True")
                    l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "Selected=True")
                    l_DataRow("Emp.Total") = l_CalculatedDataTable.Compute("SUM ([Emp.Total])", "Selected=True")
                    l_DataRow("YMCATaxable") = l_CalculatedDataTable.Compute("SUM (YMCATaxable)", "Selected=True")
                    l_DataRow("YMCAInterest") = l_CalculatedDataTable.Compute("SUM (YMCAInterest)", "Selected=True")
                    l_DataRow("YMCATotal") = l_CalculatedDataTable.Compute("SUM (YMCATotal)", "Selected=True")
                    l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "Selected=True")
                Else
                    l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "")
                    l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "")
                    l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "")
                    l_DataRow("Emp.Total") = l_CalculatedDataTable.Compute("SUM ([Emp.Total])", "")
                    l_DataRow("YMCATaxable") = l_CalculatedDataTable.Compute("SUM (YMCATaxable)", "")
                    l_DataRow("YMCAInterest") = l_CalculatedDataTable.Compute("SUM (YMCAInterest)", "")
                    l_DataRow("YMCATotal") = l_CalculatedDataTable.Compute("SUM (YMCATotal)", "")
                    l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "")
                End If

                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Taxable") = "0.00"
                End If

                If l_DataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Non-Taxable") = "0.00"
                End If

                If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Interest") = "0.00"
                End If

                If l_DataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Emp.Total") = "0.00"
                End If

                If l_DataRow("YMCATaxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("YMCATaxable") = "0.00"
                End If

                If l_DataRow("YMCAInterest").GetType.ToString = "System.DBNull" Then
                    l_DataRow("YMCAInterest") = "0.00"
                End If

                If l_DataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
                    l_DataRow("YMCATotal") = "0.00"
                End If

                If l_DataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Total") = "0.00"
                End If

                ''Set the Total Amount .
                If (l_DataRow("Total").GetType.ToString = "System.DBNull") Then
                    ' Me.TotalAmount = 0.0
                Else
                    'Me.TotalAmount = DirectCast(l_DataRow("Total"), Decimal)
                End If

                '' Set The emp . Toatal Amount

                If (l_DataRow("Emp.Total").GetType.ToString = "System.DBNull") Then
                    ' Me.PersonTotalAmount = 0.0
                Else
                    'Me.PersonTotalAmount = DirectCast(l_DataRow("Emp.Total"), Decimal)
                End If


                If l_BooleanFlag = False Then
                    l_DataRow("AccountType") = "Total"
                    l_CalculatedDataTable.Rows.Add(l_DataRow)
                End If

                CalculateTotal = l_CalculatedDataTable
            End If
        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function GetSchemaRefundTable()

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable

        Try
            l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSchemaRefundTable

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("atsRefunds")
                Session("AtsRefund_C19") = l_DataTable

                l_DataTable = l_DataSet.Tables("atsRefRequests")
                Session("AtsRefRequests_C19") = l_DataTable

                l_DataTable = l_DataSet.Tables("atsRefRequestDetails")
                Session("AtsRefRequestDetails_C19") = l_DataTable

            End If
        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region
#Region " Save Refund Request"
    'Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
    'Public Function UpdateRefundRequestTable(ByVal parameterdatatable As DataTable, ByVal blnPartial As Boolean, ByVal PrameterPlanType As String, ByVal PartialAmount As Decimal, ByVal bool_SaveRefund As Boolean, ByVal bool_IncludeYMCAAcct As Boolean, ByVal bool_IncludeYMCALegacyAcct As Boolean, ByVal ParameterProcessId As String, ByVal bool_bitIsMarket As Boolean, ByVal ParameterMarketPercentage As Decimal, ByVal ParameterMarketDefferedPmtPercentage As Decimal, ByVal FlagIsMarketPerPlan As Boolean) As Boolean
    Public Function UpdateRefundRequestTable(ByVal parameterdatatable As DataTable, ByVal blnPartial As Boolean, ByVal PrameterPlanType As String, ByVal PartialAmount As Decimal, ByVal bool_SaveRefund As Boolean, ByVal bool_IncludeYMCAAcct As Boolean, ByVal bool_IncludeYMCALegacyAcct As Boolean, ByVal ParameterProcessId As String, ByVal bool_bitIsMarket As Boolean, ByVal ParameterMarketPercentage As Decimal, ByVal ParameterMarketDefferedPmtPercentage As Decimal, ByVal FlagIsMarketPerPlan As Boolean, ByVal Bool_NotIncludeAMMatched As Boolean, ByVal Bool_NotIncludeTMMatched As Boolean) As Boolean

        Dim l_RefundRequestDataTable As DataTable
        Dim l_RefundRequestDetailsDataTable As DataTable
        Dim l_ContributionDataTable As DataTable
        Dim l_RefundDataSet As DataSet
        Dim l_StringUniqueId As String
        Dim l_DataRow As DataRow
        Dim l_ContributionDataRow As DataRow
        Dim l_Integer_Tax As Decimal
        Session("OnlyYmcaSideforAM_C19") = Nothing
        Dim l_dt_TransactionType As DataTable
        Dim l_bool_IsRWPR As Boolean = False

        Try
            l_dt_TransactionType = GetTransactionType(Me.FundID)
            For Each dr As DataRow In l_dt_TransactionType.Rows
                If dr("AcctType").GetType.ToString() <> "System.DBNull" Then
                    If dr("AcctType") = "AM" Then
                        If dr("TransactType").GetType.ToString() <> "System.DBNull" Then
                            If dr("TransactType") = "RWPR" Then
                                l_bool_IsRWPR = True
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next
            If blnPartial = True Then
                l_ContributionDataTable = parameterdatatable
            Else
                l_ContributionDataTable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
            End If
            If l_ContributionDataTable Is Nothing Then Return False

            l_RefundRequestDataTable = DirectCast(Session("AtsRefRequests_C19"), DataTable).Clone
            If blnAddNewColumn = False Then
                l_RefundRequestDataTable.Columns.Add("PlanType", System.Type.GetType("System.String"))
                l_RefundRequestDataTable.Columns.Add("ProRatedPercentage", System.Type.GetType("System.Decimal"))
                'Added as per the additional partial request for the reports-Amit 25-09-2009
                l_RefundRequestDataTable.Columns.Add("ProcessID", System.Type.GetType("System.String"))
                'New Modification for Market Based Withdrawal Amit Nigam 29/oct/2009        
                l_RefundRequestDataTable.Columns.Add("IsMarket", System.Type.GetType("System.Boolean"))
                'New Modification for Market Based Withdrawal Amit Nigam 29/oct/2009        
                'Added as per the additional partial request for the reports-Amit 25-09-2009
                l_RefundRequestDataTable.Columns.Add("DefferedPmtPercentage", System.Type.GetType("System.Decimal"))
                l_RefundRequestDataTable.Columns.Add("FirstInstPercentage", System.Type.GetType("System.Decimal"))

            End If
            If l_RefundRequestDataTable Is Nothing Then Return False
            l_DataRow = l_RefundRequestDataTable.NewRow
            l_DataRow("UniqueID") = Me.PersonID
            l_DataRow("PersID") = Me.PersonID
            l_DataRow("FundEventID") = Me.FundID
            l_DataRow("RefundType") = Me.RefundType
            l_DataRow("RequestStatus") = "PEND"
            l_DataRow("StatusDate") = Now.Date
            l_DataRow("RequestDate") = Now.Date
            l_DataRow("ReleaseBlankType") = "???"
            If PartialAmount = 0 Then
                l_DataRow("Amount") = Me.TotalAmount
            Else
                If PartialAmount = Me.TotalAmount Then
                    l_DataRow("Amount") = Me.TotalAmount
                    l_DataRow("Amount") = Me.TotalAmount
                Else
                    PartialAmount = PartialAmount - Me.TotalAmount
                    Me.TotalAmount = Me.TotalAmount + PartialAmount
                    l_DataRow("Amount") = Me.TotalAmount
                End If
            End If

            l_DataRow("ExpireDate") = Now.Date.AddDays(Me.RefundExpiryDate)
            l_DataRow("TaxRate") = Me.FederalTaxRate
            l_DataRow("Taxable") = Me.TaxedAmount
            l_DataRow("NonTaxable") = Me.NonTaxedAmount
            If PrameterPlanType = "SAVINGS" Or PrameterPlanType = "BOTH" Then
                l_DataRow("HardShipAmt") = 0.0
            End If


            l_DataRow("Deductions") = "0.00"

            l_Integer_Tax = DirectCast(Me.TaxAmount, Decimal)

            If Me.MinimumDistributionAmount > 0.0 Then
                l_Integer_Tax = l_Integer_Tax - (Me.MinimumDistributionAmount * (Me.MinimumDistributionTaxRate / 100.0))
            End If

            l_DataRow("Tax") = l_Integer_Tax

            l_DataRow("AddressID") = Me.AddressId
            l_DataRow("MinDistribution") = Me.MinimumDistributionAmount
            l_DataRow("ReleaseSentDate") = Now.Date

            If Me.PlanChosen = "SAVINGS" Then
                l_DataRow("PIA") = "0.00"
            Else
                l_DataRow("PIA") = Me.TerminationPIA
            End If
            l_DataRow("PlanType") = PrameterPlanType
            'Added as per the additional partial request for the reports-Amit 25-09-2009
            l_DataRow("ProcessId") = ParameterProcessId
            'Added as per the additional partial request for the reports-Amit 25-09-2009
            'New Modification for Market Based Withdrawal Amit Nigam 29/oct/2009        
            l_DataRow("IsMarket") = FlagIsMarketPerPlan
            'New Modification for Market Based Withdrawal Amit Nigam 29/oct/2009    

            If PrameterPlanType = "RETIREMENT" Then
                l_DataRow("ProRatedPercentage") = Me.NumPercentageFactorofMoneyRetirement

            ElseIf PrameterPlanType = "SAVINGS" Then
                l_DataRow("ProRatedPercentage") = Me.NumPercentageFactorofMoneySavings
            Else
                l_DataRow("ProRatedPercentage") = 0.0
            End If
            If bool_bitIsMarket = True Then
                l_DataRow("FirstInstPercentage") = ParameterMarketPercentage / 100
            Else
                l_DataRow("FirstInstPercentage") = 0
            End If
            'If PrameterPlanType = "RETIREMENT" Then
            '    If bool_bitIsMarket = False Then
            '        l_DataRow("ProRatedPercentage") = Me.NumPercentageFactorofMoneyRetirement
            '        l_DataRow("FirstInstPercentage") = 0
            '    Else
            '        l_DataRow("ProRatedPercentage") = 0
            '        l_DataRow("FirstInstPercentage") = ParameterMarketPercentage / 100
            '    End If

            'ElseIf PrameterPlanType = "SAVINGS" Then
            '    If bool_bitIsMarket = False Then
            '        l_DataRow("ProRatedPercentage") = Me.NumPercentageFactorofMoneySavings
            '        l_DataRow("FirstInstPercentage") = 0.0
            '    Else
            '        l_DataRow("ProRatedPercentage") = 0.0
            '        l_DataRow("FirstInstPercentage") = ParameterMarketPercentage / 100

            '    End If

            'Else
            '    If bool_bitIsMarket = False Then
            '        l_DataRow("ProRatedPercentage") = 0.0
            '        l_DataRow("FirstInstPercentage") = 0.0
            '    Else
            '        l_DataRow("ProRatedPercentage") = 0.0
            '        l_DataRow("FirstInstPercentage") = ParameterMarketPercentage / 100
            '    End If

            'End If
            l_DataRow("DefferedPmtPercentage") = ParameterMarketDefferedPmtPercentage


            l_RefundRequestDataTable.Rows.Add(l_DataRow)

            '****************************************************
            'This segment for making Refund Request Details.. 
            '****************************************************
            'FixRoundingIssue(l_RefundRequestDataTable, RequestedAmount)
            'This Delcaration for Updating the Refund Request Details.

            Dim l_String_AccountType As String
            Dim l_String_UniqueID As String
            Dim l_String_RefRequestsID As String
            Dim l_String_AccountBreakDownType As String
            Dim l_Decimal_PersonalInterest As Decimal
            Dim l_Decimal_YmcaInterest As Decimal
            Dim l_Decimal_AccountTotal As Decimal
            Dim l_Decimal_PersonalPostTax As Decimal
            Dim l_Decimal_PersonalPreTax As Decimal
            Dim l_Decimal_PersonalTotal As Decimal
            Dim l_Decimal_YMCAPreTax As Decimal
            Dim l_Decimal_YMCATotal As Decimal
            Dim l_Decimal_GrandTotal As Decimal
            Dim l_Decimal_Total As Decimal
            Dim l_Decimal_PreTax As Decimal
            Dim l_Decimal_PostTax As Decimal
            Dim l_Decimal_Interst As Decimal
            Dim l_Decimal_TDAmount As Decimal = 0.0
            Dim l_Decimal_OtherAmount As Decimal = 0.0
            Dim l_Integer_SortOrder As Integer

            'Plan Split Variable 
            Dim l_String_AccountGroup As String = ""
            l_RefundRequestDetailsDataTable = DirectCast(Session("AtsRefRequestDetails_C19"), DataTable).Clone
            Dim l_bool_Selected As Boolean = False
            If Me.RefundType = "HARD" Then
                Session("ReportHWL5_C19") = False
            End If
            For Each l_ContributionDataRow In l_ContributionDataTable.Rows

                If l_ContributionDataRow.RowState <> DataRowState.Deleted Then ' If the row is deleted then Don;t cont the values
                    If Me.RefundType = "VOL" Then
                        If l_ContributionDataRow("Selected").GetType.ToString() = "System.DBNull" Then
                            l_bool_Selected = False
                        Else
                            l_bool_Selected = DirectCast(l_ContributionDataRow("Selected"), Boolean)
                        End If
                    Else
                        l_bool_Selected = True
                    End If

                    If l_bool_Selected = True Then
                        If Not (l_ContributionDataRow("AccountType").GetType.ToString = "System.DBNull") Then
                            'Modified By Parveen For Market Based Withdrawal on 16-Nov-2009
                            ' If Not (DirectCast(l_ContributionDataRow("AccountType"), String) = "Total") And Not (DirectCast(l_ContributionDataRow("AccountType"), String) = "Market Based (" + DirectCast(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then
                            If Not (DirectCast(l_ContributionDataRow("AccountType"), String) = "Total") And Not (DirectCast(l_ContributionDataRow("AccountType"), String) = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)") Then

                                l_Decimal_PreTax = 0.0
                                l_Decimal_PostTax = 0.0
                                l_Decimal_Interst = 0.0
                                l_Decimal_PersonalPostTax = 0.0
                                l_Decimal_PersonalPreTax = 0.0
                                l_Decimal_PersonalInterest = 0.0
                                l_Decimal_PersonalTotal = 0.0
                                l_Decimal_YMCAPreTax = 0.0
                                l_Decimal_YmcaInterest = 0.0
                                l_Decimal_YMCATotal = 0.0
                                l_Decimal_AccountTotal = 0.0
                                l_Decimal_GrandTotal = 0.0
                                Session("OnlyYmcaSideforAM_C19") = Nothing
                                l_String_AccountType = DirectCast(l_ContributionDataRow("AccountType"), String).Trim.ToUpper
                                l_String_AccountGroup = DirectCast(l_ContributionDataRow("AccountGroup"), String).Trim.ToUpper()

                                'If Hard then dont include TD Amount for decideing the Release blank report
                                If Me.RefundType = "HARD" Then
                                    If l_String_AccountGroup = m_const_RetirementPlan_AP Or l_String_AccountGroup = m_const_RetirementPlan_RP Or l_String_AccountGroup = m_const_SavingsPlan_RT Then
                                        l_Decimal_Total = DirectCast(l_ContributionDataRow("Total"), Decimal)
                                        'Set Sesion To decide which report to call
                                        Session("ReportHWL5_C19") = True
                                    Else
                                        l_Decimal_Total = 0
                                    End If
                                Else
                                    l_Decimal_Total = DirectCast(l_ContributionDataRow("Total"), Decimal)
                                End If

                                If l_String_AccountGroup = m_const_SavingsPlan_TD Or l_String_AccountGroup = m_const_SavingsPlan_TM Then
                                    l_Decimal_TDAmount += DirectCast(l_ContributionDataRow("Total"), Decimal)
                                End If

                                l_Decimal_OtherAmount += l_Decimal_Total
                                Session("TotalAmtForReleaseBlnk_C19") = l_Decimal_OtherAmount
                                Session("inOtherAmount_C19") = l_Decimal_OtherAmount
                                If l_ContributionDataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
                                    l_Decimal_PersonalTotal = 0.0
                                    l_ContributionDataRow("Emp.Total") = "0.00"
                                Else
                                    l_Decimal_PersonalTotal = DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
                                End If

                                If l_ContributionDataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
                                    l_Decimal_YMCATotal = 0.0
                                    l_ContributionDataRow("YMCATotal") = "0.00"
                                Else
                                    l_Decimal_YMCATotal = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
                                End If
                                Do While (l_Decimal_PersonalTotal + l_Decimal_YMCATotal) > 0.0
                                    l_Decimal_Interst = 0.0
                                    If l_String_AccountGroup = m_const_RetirementPlan_AM And l_Decimal_PersonalTotal = 0.0 Then
                                        Session("OnlyYmcaSideforAM_C19") = True
                                        If l_bool_IsRWPR = True Then
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, False, True, False, False)
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)
                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                        Else
                                            l_String_AccountBreakDownType = "07"
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)
                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                        End If
                                    End If


                                    If DirectCast(l_ContributionDataRow("YMCATotal"), Decimal) > 0.0 Then
                                        If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
                                        Else
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, False, True, False, False)
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)
                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                        End If

                                        If Not l_DataRow Is Nothing Then

                                            l_DataRow("YMCAPreTax") = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAPreTax"), Decimal)
                                            l_DataRow("YMCAInterest") = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                            l_DataRow("YMCATotal") = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                            l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                            l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

                                        Else
                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow
                                            l_Decimal_YMCAPreTax = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal)
                                            l_Decimal_YmcaInterest = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal)
                                            l_Decimal_YMCATotal = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
                                            l_Decimal_AccountTotal = l_Decimal_YMCATotal
                                            l_Decimal_GrandTotal = l_Decimal_YMCATotal

                                            l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
                                            l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
                                            l_DataRow("YMCATotal") = l_Decimal_YMCATotal
                                            l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                            l_DataRow("GrandTotal") = l_Decimal_GrandTotal

                                            '' Add additional  Detalis..
                                            l_DataRow("AcctType") = l_String_AccountType
                                            l_DataRow("PersonalPostTax") = "0.00"
                                            l_DataRow("PersonalPreTax") = "0.00"
                                            l_DataRow("PersonalInterest") = "0.00"
                                            l_DataRow("PersonalTotal") = "0.00"

                                            l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                            l_DataRow("SortOrder") = l_Integer_SortOrder

                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                        End If

                                        l_ContributionDataRow("YMCATaxable") = "0.0"
                                        l_ContributionDataRow("YMCAInterest") = "0.0"
                                        l_ContributionDataRow("YMCATotal") = "0.0"

                                        l_Decimal_YMCAPreTax = 0.0
                                        l_Decimal_YmcaInterest = 0.0
                                        l_Decimal_YMCATotal = 0.0
                                        l_Decimal_AccountTotal = 0.0
                                        l_Decimal_GrandTotal = 0.0


                                        l_Decimal_YMCATotal = 0.0


                                    ElseIf DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) > 0.0 Then
                                        If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
                                        Else
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, False, True)

                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                        End If


                                        If DirectCast(l_ContributionDataRow("Taxable"), Decimal) > 0.0 Then

                                            l_Decimal_PreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)


                                        Else
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                        End If

                                        'Priya 06-Jan-2009 YRS 5.0-529 Incorrect atsRefRequestDetails records for SS account   
                                        If (l_String_AccountGroup = m_const_RetirementPlan_SS) Then
                                            'for PersonalPretax and Interest AccountDreakDownType = 2
                                            If (l_Decimal_PreTax > 0.0 Or l_Decimal_Interst > 0.0) Then

                                                Dim l_String_Account_BreakDown_Type As String
                                                Dim l_Integer_Sort_Order As Integer
                                                Dim l_SS_DataRow As DataRow

                                                l_String_Account_BreakDown_Type = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, True, False)
                                                l_Integer_Sort_Order = Me.GetAccountBreakDownSortOrder(l_String_Account_BreakDown_Type)

                                                l_SS_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)

                                                If l_SS_DataRow Is Nothing Then
                                                    l_SS_DataRow = l_RefundRequestDetailsDataTable.NewRow


                                                    l_Decimal_PostTax = 0.0

                                                    l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst + l_Decimal_PreTax


                                                    l_Decimal_AccountTotal = l_Decimal_Total
                                                    l_Decimal_GrandTotal = l_Decimal_AccountTotal

                                                    l_SS_DataRow("PersonalPreTax") = l_Decimal_PreTax
                                                    l_SS_DataRow("PersonalPostTax") = l_Decimal_PostTax
                                                    l_SS_DataRow("PersonalInterest") = l_Decimal_Interst
                                                    l_SS_DataRow("PersonalTotal") = l_Decimal_Total

                                                    l_SS_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                                    l_SS_DataRow("GrandTotal") = l_Decimal_GrandTotal
                                                    l_SS_DataRow("AcctType") = l_String_AccountType

                                                    l_SS_DataRow("YMCAPreTax") = "0.00"
                                                    l_SS_DataRow("YMCAInterest") = "0.00"
                                                    l_SS_DataRow("YMCATotal") = "0.00"

                                                    l_SS_DataRow("AcctBreakDownType") = l_String_Account_BreakDown_Type
                                                    l_SS_DataRow("SortOrder") = l_Integer_Sort_Order

                                                    l_Decimal_Interst = 0.0
                                                    l_Decimal_PreTax = 0.0

                                                    l_RefundRequestDetailsDataTable.Rows.Add(l_SS_DataRow)
                                                Else
                                                    l_SS_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
                                                    l_SS_DataRow("PersonalPostTax") = 0.0
                                                    l_SS_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                                    l_SS_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + 0.0 + l_Decimal_Interst + l_Decimal_PreTax
                                                    l_SS_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
                                                    l_SS_DataRow("GrandTotal") = l_DataRow("AcctTotal")

                                                End If

                                            End If

                                        End If
                                        If Not l_DataRow Is Nothing Then

                                            'TODO : verify with client for the logic as per foxpro logic written above but commented - Hafiz & Shubhrata on 21-Nov-2006 
                                            l_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
                                            l_DataRow("PersonalPostTax") = DirectCast(l_DataRow("PersonalPostTax"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                            'Priya : 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
                                            l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) + l_Decimal_Interst + l_Decimal_PreTax
                                            l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
                                            l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

                                        Else

                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow
                                            l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst + l_Decimal_PreTax
                                            l_Decimal_AccountTotal = l_Decimal_Total
                                            l_Decimal_GrandTotal = l_Decimal_AccountTotal


                                            l_DataRow("PersonalPreTax") = l_Decimal_PreTax

                                            l_DataRow("PersonalPostTax") = l_Decimal_PostTax
                                            l_DataRow("PersonalInterest") = l_Decimal_Interst
                                            l_DataRow("PersonalTotal") = l_Decimal_Total
                                            l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                            l_DataRow("GrandTotal") = l_Decimal_GrandTotal
                                            l_DataRow("AcctType") = l_String_AccountType
                                            l_DataRow("YMCAPreTax") = "0.00"
                                            l_DataRow("YMCAInterest") = "0.00"
                                            l_DataRow("YMCATotal") = "0.00"
                                            l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                            l_DataRow("SortOrder") = l_Integer_SortOrder
                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                        End If


                                        l_ContributionDataRow("Taxable") = "0.00"
                                        l_ContributionDataRow("Interest") = DirectCast(l_ContributionDataRow("Interest"), Decimal) - l_Decimal_Interst
                                        l_ContributionDataRow("Emp.Total") = l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest



                                        l_Decimal_PersonalTotal = 0.0

                                    Else

                                        If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
                                        Else
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, True, False)
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)

                                        End If




                                        If DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) > 0.0 Then

                                            l_Decimal_PreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)

                                            l_Decimal_Interst = l_Decimal_Interst * (l_Decimal_PreTax / (DirectCast(l_ContributionDataRow("Emp.Total"), Decimal) - DirectCast(l_ContributionDataRow("Interest"), Decimal)))

                                        Else
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                        End If

                                        If Not l_DataRow Is Nothing Then
                                            l_DataRow("PersonalPreTax") = DirectCast(l_DataRow("PersonalPreTax"), Decimal) + DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + l_Decimal_Interst
                                            l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Taxable"), Decimal) + l_Decimal_Interst
                                            l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("PersonalTotal"), Decimal)
                                            l_DataRow("GrandTotal") = l_DataRow("AcctTotal")
                                        Else
                                            Dim blnValue As Boolean = False
                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow

                                            l_Decimal_PersonalPreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_Decimal_PersonalInterest = l_Decimal_Interst
                                            l_Decimal_PersonalTotal = l_Decimal_Interst + l_Decimal_PersonalPreTax
                                            l_Decimal_AccountTotal = l_Decimal_PersonalTotal
                                            l_Decimal_GrandTotal = l_Decimal_AccountTotal

                                            Dim blnPers As Boolean = False

                                            'If Not bool_RemoveRounding Then
                                            '    If Not Session("RoundingAmount") Is Nothing Then
                                            '        If l_Decimal_PersonalPreTax > Math.Abs(Me.Session("RoundingAmount")) Then
                                            '            l_Decimal_PersonalPreTax = l_Decimal_PersonalPreTax + Math.Abs(Me.Session("RoundingAmount"))
                                            '            blnValue = True
                                            '            blnPers = True
                                            '        End If
                                            '        If l_Decimal_PersonalInterest > Math.Abs(Me.Session("RoundingAmount")) And blnValue = False Then
                                            '            l_Decimal_PersonalInterest = l_Decimal_PersonalInterest + Math.Abs(Me.Session("RoundingAmount"))
                                            '            blnValue = True
                                            '            blnPers = True
                                            '        End If
                                            '        If l_Decimal_PersonalTotal > Math.Abs(Me.Session("RoundingAmount")) And blnPers = True Then
                                            '            l_Decimal_PersonalTotal = l_Decimal_PersonalTotal + Math.Abs(Me.Session("RoundingAmount"))
                                            '        End If

                                            '        Dim blnYmaca As Boolean = False
                                            '        If l_Decimal_YMCAPreTax > Math.Abs(Me.Session("RoundingAmount")) And blnValue = False Then
                                            '            l_Decimal_YMCAPreTax = l_Decimal_YMCAPreTax + Math.Abs(Me.Session("RoundingAmount"))
                                            '            blnYmaca = True
                                            '        End If
                                            '        If l_Decimal_YmcaInterest > Math.Abs(Me.Session("RoundingAmount")) And blnValue = False Then
                                            '            l_Decimal_YmcaInterest = l_Decimal_YmcaInterest + Math.Abs(Me.Session("RoundingAmount"))
                                            '            blnYmaca = True
                                            '        End If

                                            '        If l_Decimal_YMCATotal > Math.Abs(Me.Session("RoundingAmount")) And blnYmaca = True Then
                                            '            l_Decimal_YMCATotal = l_Decimal_YMCATotal + Math.Abs(Me.Session("RoundingAmount"))
                                            '        End If

                                            '        If l_Decimal_AccountTotal > Math.Abs(Me.Session("RoundingAmount")) And (blnYmaca = True Or blnPers = True) Then
                                            '            l_Decimal_AccountTotal = l_Decimal_AccountTotal + Math.Abs(Me.Session("RoundingAmount"))
                                            '        End If

                                            '        If l_Decimal_GrandTotal > Math.Abs(Me.Session("RoundingAmount")) And (blnYmaca = True Or blnPers = True) Then
                                            '            l_Decimal_GrandTotal = l_Decimal_GrandTotal + Math.Abs(Me.Session("RoundingAmount"))
                                            '        End If
                                            '        bool_RemoveRounding = True
                                            '    End If
                                            'End If
                                            l_DataRow("PersonalPreTax") = l_Decimal_PersonalPreTax
                                            l_DataRow("PersonalInterest") = l_Decimal_PersonalInterest
                                            l_DataRow("PersonalTotal") = l_Decimal_PersonalTotal
                                            l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                            l_DataRow("GrandTotal") = l_Decimal_GrandTotal

                                            l_DataRow("AcctType") = l_String_AccountType
                                            l_DataRow("PersonalPostTax") = l_ContributionDataRow("Non-Taxable")
                                            l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
                                            l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
                                            l_DataRow("YMCATotal") = l_Decimal_YMCATotal

                                            l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                            l_DataRow("SortOrder") = l_Integer_SortOrder

                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                        End If



                                        l_ContributionDataRow("Non-Taxable") = "0.00"
                                        l_ContributionDataRow("Interest") = DirectCast(l_ContributionDataRow("Interest"), Decimal) - l_Decimal_Interst
                                        l_ContributionDataRow("Emp.Total") = l_ContributionDataRow("Interest")


                                        l_Decimal_PersonalPostTax = 0.0
                                        l_Decimal_PersonalInterest = 0.0
                                        l_Decimal_PersonalTotal = 0.0
                                        l_Decimal_AccountTotal = 0.0
                                        l_Decimal_GrandTotal = 0.0

                                        l_Decimal_PersonalTotal = 0.0
                                        l_Decimal_YMCATotal = 0.0

                                    End If
                                Loop

                            End If
                        End If
                    End If
                End If
            Next

            ''' Update the database


            l_RefundDataSet = New DataSet("RefundRequest")



            If l_Decimal_TDAmount > 0.0 Then
                l_DataRow = l_RefundRequestDataTable.Rows(0)


                'Me.TDAccountAmount()
                'Me.TDTotalAmount()
                'Me.TDUsedAmount()
                If PrameterPlanType = "SAVINGS" Or PrameterPlanType = "BOTH" Then
                    'l_DataRow("HardShipAmt") = Me.Session("TDTotalAmount") ' "0.00"
                    l_DataRow("HardShipAmt") = l_Decimal_TDAmount
                End If
            End If

            'Added to create the modifications for the rounding issue in l_RefundRequestDataTable -Amit 09-09-2009
            If l_RefundRequestDataTable.Rows(0).Item("PlanType") = "RETIREMENT" And Me.RefundType = "PART" Then
                If Not l_RefundRequestDataTable.Rows(0).Item("Amount") = Me.NumRequestedAmountRetirement Then
                    Dim l_dr_NewRowRefundRequestTable As DataRow
                    l_dr_NewRowRefundRequestTable = l_RefundRequestDataTable.Rows(0)
                    l_dr_NewRowRefundRequestTable.BeginEdit()
                    Dim l_amountdiff As Decimal
                    l_amountdiff = Me.NumRequestedAmountRetirement - Convert.ToDecimal(l_dr_NewRowRefundRequestTable.Item("Amount"))
                    l_dr_NewRowRefundRequestTable.Item("Amount") = l_RefundRequestDataTable.Rows(0).Item("Amount") + l_amountdiff
                    l_dr_NewRowRefundRequestTable.EndEdit()
                End If
            ElseIf l_RefundRequestDataTable.Rows(0).Item("PlanType") = "SAVINGS" And Me.RefundType = "PART" Then
                If Not l_RefundRequestDataTable.Rows(0).Item("Amount") = Me.NumRequestedAmountSavings Then
                    Dim l_dr_NewRowRefundRequestTable As DataRow
                    l_dr_NewRowRefundRequestTable = l_RefundRequestDataTable.Rows(0)
                    l_dr_NewRowRefundRequestTable.BeginEdit()
                    Dim l_amountdiff As Decimal
                    l_amountdiff = Me.NumRequestedAmountSavings - Convert.ToDecimal(l_dr_NewRowRefundRequestTable.Item("Amount"))
                    l_dr_NewRowRefundRequestTable.Item("Amount") = l_RefundRequestDataTable.Rows(0).Item("Amount") + l_amountdiff
                    l_dr_NewRowRefundRequestTable.EndEdit()
                End If
            End If
            'Added to create the modifications for the rounding issue in l_RefundRequestDataTable -Amit 09-09-2009


            l_RefundRequestDetailsDataTable = ManageRoundingIssues(l_RefundRequestDetailsDataTable, l_RefundRequestDataTable)


            If l_FinalrefundDataSet.Tables.Count = 0 Then
                l_FinalrefundDataSet.Tables.Add(l_RefundRequestDataTable)
                l_FinalrefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable)
            Else
                l_RefundDataSet.Tables.Add(l_RefundRequestDataTable)
                l_RefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable)
            End If
            Dim StringUniqueId As ArrayList = New ArrayList
            If bool_SaveRefund Then
                'Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
                'StringUniqueId = YMCARET.YmcaBusinessObject.RefundRequest.InsertRefunds(l_RefundDataSet, l_FinalrefundDataSet, bool_IncludeYMCAAcct, bool_IncludeYMCALegacyAcct)
                StringUniqueId = YMCARET.YmcaBusinessObject.RefundRequest.InsertRefunds(l_RefundDataSet, l_FinalrefundDataSet, bool_IncludeYMCAAcct, bool_IncludeYMCALegacyAcct, Bool_NotIncludeAMMatched, Bool_NotIncludeTMMatched)
                'Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
            End If

            Session("RefundRequestIDs_c19") = StringUniqueId
            ''''updating the generated UniqueId returned from the proc into the cache dataset.
            ''''Added on Aug 2009 By Ganeswar To keep the Unique Id in Seesion so that Multiple report Can Open in the squence
            '''If StringUniqueId.Count > 0 Then
            '''    For UniqueId As Integer = 0 To StringUniqueId.Count - 1
            '''        If UniqueId = 1 Then
            '''            l_RefundDataSet.Tables(0).Rows(0)(0) = StringUniqueId.Item(UniqueId)
            '''            l_RefundDataSet.Tables(0).AcceptChanges()
            '''            Me.Session("RequestUniqueId") = l_RefundDataSet.Tables(0).Rows(0)(0)

            '''        Else
            '''            l_FinalrefundDataSet.Tables(0).Rows(0)(0) = StringUniqueId.Item(UniqueId)
            '''            l_FinalrefundDataSet.Tables(0).AcceptChanges()
            '''            Me.Session("DetailsUniqueId") = l_FinalrefundDataSet.Tables(0).Rows(0)(0)
            '''            Session("AtsRefRequests") = l_FinalrefundDataSet.Tables(0)
            '''            Session("AtsRefRequestDetails") = l_FinalrefundDataSet.Tables(0)
            '''        End If
            '''    Next
            '''End If
            ''''Added on Aug 2009 By Ganeswar 
        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function ManageRoundingIssues(ByVal parameter__RefundRequestDetailsDataTable As DataTable, ByVal parameter_RefundRequestDataTable As DataTable) As DataTable
        Dim l_decimaltotal As Decimal = 0
        Dim l_decimalRoundingValue As Decimal
        Dim ll_round As Boolean = False
        Dim l_datarows As DataRow()
        Dim l_datarow As DataRow
        Dim l_adjustNewRow As Boolean = False
        Dim l_AcctType As String = String.Empty

        Try
            l_decimaltotal = DirectCast(parameter__RefundRequestDetailsDataTable.Compute("SUM (GrandTotal)", ""), Decimal)

            If parameter_RefundRequestDataTable.Rows(0)("PlanType") = "RETIREMENT" And parameter_RefundRequestDataTable.Rows(0)("RefundType") = "PART" Then
                If l_decimaltotal <> Me.NumRequestedAmountRetirement Then
                    l_decimalRoundingValue = Me.NumRequestedAmountRetirement - l_decimaltotal
                    If Not Session("RetirementAccountTypeAdjusted_C19") Is Nothing Then
                        l_AcctType = Session("RetirementAccountTypeAdjusted_C19")
                    End If
                    ll_round = True
                End If
            ElseIf parameter_RefundRequestDataTable.Rows(0)("PlanType") = "SAVINGS" And parameter_RefundRequestDataTable.Rows(0)("RefundType") = "PART" Then
                If l_decimaltotal <> Me.NumRequestedAmountSavings Then
                    l_decimalRoundingValue = Me.NumRequestedAmountSavings - l_decimaltotal
                    If Not Session("SavingsAccountTypeAdjusted_C19") Is Nothing Then
                        l_AcctType = Session("SavingsAccountTypeAdjusted_C19")
                    End If
                    ll_round = True
                End If
            End If

            If ll_round = True Then


                If Not l_AcctType.Trim() = String.Empty Then
                    l_datarows = parameter__RefundRequestDetailsDataTable.Select("AcctType = '" & l_AcctType.Trim() & "'")
                    If l_datarows.Length > 0 Then

                        l_datarow = l_datarows(0)

                        If AdjustDatarow(l_datarow, l_decimalRoundingValue) = False Then
                            l_adjustNewRow = True
                        End If

                    Else
                        l_adjustNewRow = True
                    End If

                Else
                    l_adjustNewRow = True
                End If


                If l_adjustNewRow = True Then
                    For Each l_datarow In parameter__RefundRequestDetailsDataTable.Rows
                        If AdjustDatarow(l_datarow, l_decimalRoundingValue) = True Then
                            ' one row adjusted exit function
                            Exit For
                        End If
                    Next

                    '' take the first row and adjust it
                End If

            End If

            ManageRoundingIssues = parameter__RefundRequestDetailsDataTable


        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function AdjustDatarow(ByVal l_datarow As DataRow, ByVal l_decimalRoundingValue As Decimal) As Boolean
        Try

            'adjust the value in any of the columbn based on the ganeswar code.
            If l_datarow("PersonalPostTax") > Math.Abs(l_decimalRoundingValue) Then
                l_datarow("PersonalPostTax") = l_datarow("PersonalPostTax") + l_decimalRoundingValue
            ElseIf l_datarow("PersonalPreTax") > Math.Abs(l_decimalRoundingValue) Then
                l_datarow("PersonalPreTax") = l_datarow("PersonalPreTax") + l_decimalRoundingValue
            ElseIf l_datarow("PersonalInterest") > Math.Abs(l_decimalRoundingValue) Then
                l_datarow("PersonalInterest") = l_datarow("PersonalInterest") + l_decimalRoundingValue
            ElseIf l_datarow("YmcaPreTax") > Math.Abs(l_decimalRoundingValue) Then
                l_datarow("YmcaPreTax") = l_datarow("YmcaPreTax") + l_decimalRoundingValue
            ElseIf l_datarow("YmcaInterest") > Math.Abs(l_decimalRoundingValue) Then
                l_datarow("YmcaInterest") = l_datarow("YmcaInterest") + l_decimalRoundingValue
            Else
                AdjustDatarow = False
                Exit Function
            End If


            l_datarow("PersonalTotal") = l_datarow("PersonalPostTax") + l_datarow("PersonalPreTax") + l_datarow("PersonalInterest")
            l_datarow("YMCATotal") = l_datarow("YmcaPreTax") + l_datarow("YmcaInterest")
            l_datarow("AcctTotal") = l_datarow("PersonalTotal") + l_datarow("YMCATotal")
            l_datarow("GrandTotal") = l_datarow("AcctTotal")

            AdjustDatarow = True

        Catch ex As Exception
            Throw
        End Try
    End Function

    ''Public Function UpdateRefundRequestTable_OLD(ByVal parameterdatatable As DataTable, ByVal blnPartial As Boolean, ByVal PrameterPlanType As String, ByVal PartialAmount As Decimal, ByVal bool_SaveRefund As Boolean, ByVal bool_IncludeYMCAAcct As Boolean, ByVal bool_IncludeYMCALegacyAcct As Boolean) As Boolean

    ''    'Hafiz 03Feb06 Cache-Session
    ''    'Dim l_CacheManger As CacheManager
    ''    'Hafiz 03Feb06 Cache-Session
    ''    Dim l_RefundRequestDataTable As DataTable
    ''    Dim l_RefundRequestDetailsDataTable As DataTable
    ''    Dim l_ContributionDataTable As DataTable
    ''    Dim l_RefundDataSet As DataSet
    ''    Dim l_StringUniqueId As String

    ''    Dim l_DataRow As DataRow
    ''    Dim l_ContributionDataRow As DataRow
    ''    Dim l_Integer_Tax As Decimal
    ''    Session("OnlyYmcaSideforAM_C19") = Nothing
    ''    Dim l_dt_TransactionType As DataTable
    ''    Dim l_bool_IsRWPR As Boolean = False
    ''    Try
    ''        l_dt_TransactionType = GetTransactionType(Me.FundID)
    ''        For Each dr As DataRow In l_dt_TransactionType.Rows
    ''            If dr("AcctType").GetType.ToString() <> "System.DBNull" Then
    ''                If dr("AcctType") = "AM" Then
    ''                    If dr("TransactType").GetType.ToString() <> "System.DBNull" Then
    ''                        If dr("TransactType") = "RWPR" Then
    ''                            l_bool_IsRWPR = True
    ''                            Exit For
    ''                        End If
    ''                    End If
    ''                End If
    ''            End If
    ''        Next
    ''        'Added by Ganeswar for "Partial Refund" on 29th June 2009
    ''        If blnPartial = True Then
    ''            l_ContributionDataTable = parameterdatatable
    ''        Else
    ''            l_ContributionDataTable = DirectCast(Session("CalculatedDataTable"), DataTable)
    ''        End If
    ''        'Added by Ganeswar for "Partial Refund" on 29th June 2009

    ''        If l_ContributionDataTable Is Nothing Then Return False

    ''        '' Get Refund request Table to Update.

    ''        'Hafiz 03Feb06 Cache-Session
    ''        'l_RefundRequestDataTable = DirectCast(l_CacheManger.GetData("AtsRefRequests"), DataTable).Clone
    ''        l_RefundRequestDataTable = DirectCast(Session("AtsRefRequests"), DataTable).Clone
    ''        'Hafiz 03Feb06 Cache-Session
    ''        'Added by Ganeswar  PlanType for "Partial Refund" on 29th June 2009
    ''        If blnAddNewColumn = False Then
    ''            l_RefundRequestDataTable.Columns.Add("PlanType", System.Type.GetType("System.String"))
    ''            l_RefundRequestDataTable.Columns.Add("ProRatedPercentage", System.Type.GetType("System.Decimal"))
    ''            'blnAddNewColumn = True
    ''        End If
    ''        'Added by Ganeswar  PlanType for "Partial Refund" on 29th June 2009
    ''        If l_RefundRequestDataTable Is Nothing Then Return False

    ''        '' This segment for adding the Values to Refund table...
    ''        l_DataRow = l_RefundRequestDataTable.NewRow



    ''        l_DataRow("UniqueID") = Me.PersonID
    ''        'l_DataRow("UniqueID") = System.Guid.NewGuid.ToString()
    ''        l_DataRow("PersID") = Me.PersonID
    ''        l_DataRow("FundEventID") = Me.FundID
    ''        l_DataRow("RefundType") = Me.RefundType
    ''        l_DataRow("RequestStatus") = "PEND"
    ''        l_DataRow("StatusDate") = Now.Date
    ''        l_DataRow("RequestDate") = Now.Date
    ''        l_DataRow("ReleaseBlankType") = "???"
    ''        If PartialAmount = 0 Then
    ''            l_DataRow("Amount") = Me.TotalAmount
    ''        Else
    ''            If PartialAmount = Me.TotalAmount Then
    ''                l_DataRow("Amount") = Me.TotalAmount
    ''                l_DataRow("Amount") = Me.TotalAmount
    ''            Else
    ''                PartialAmount = PartialAmount - Me.TotalAmount
    ''                Me.TotalAmount = Me.TotalAmount + PartialAmount
    ''                l_DataRow("Amount") = Me.TotalAmount
    ''            End If
    ''        End If

    ''        l_DataRow("ExpireDate") = Now.Date.AddDays(Me.RefundExpiryDate)
    ''        l_DataRow("TaxRate") = Me.FederalTaxRate
    ''        l_DataRow("Taxable") = Me.TaxedAmount
    ''        l_DataRow("NonTaxable") = Me.NonTaxedAmount
    ''        'Added by Ganeswar Aug 04th 2009
    ''        'If PartialAmount = 0 Then
    ''        '    l_DataRow("HardShipAmt") = "0.00"
    ''        'Else
    ''        If PrameterPlanType = "SAVINGS" Or PrameterPlanType = "BOTH" Then
    ''            l_DataRow("HardShipAmt") = Me.Session("TDTotalAmount") ' "0.00"
    ''        End If
    ''        'l_DataRow("HardShipAmt") = "0.00"
    ''        'Added by Ganeswar Aug 04th 2009

    ''        l_DataRow("Deductions") = "0.00"

    ''        l_Integer_Tax = DirectCast(Me.TaxAmount, Decimal)

    ''        If Me.MinimumDistributionAmount > 0.0 Then
    ''            l_Integer_Tax = l_Integer_Tax - (Me.MinimumDistributionAmount * (Me.MinimumDistributionTaxRate / 100.0))
    ''        End If

    ''        l_DataRow("Tax") = l_Integer_Tax
    ''        'hardcoded 0 for addressid to avoid use of integerid from addrshist table
    ''        'changed by ruchi on 14 jun 2006
    ''        'reverted this back to integer id on 29th jun 2006 as per requirement.
    ''        l_DataRow("AddressID") = Me.AddressId
    ''        l_DataRow("MinDistribution") = Me.MinimumDistributionAmount
    ''        l_DataRow("ReleaseSentDate") = Now.Date
    ''        'l_DataRow("PIA") = Me.TerminationPIA
    ''        If Me.PlanChosen = "SAVINGS" Then
    ''            l_DataRow("PIA") = "0.00"
    ''        Else
    ''            l_DataRow("PIA") = Me.TerminationPIA
    ''        End If
    ''        'Added by Ganeswar  PlanType for "Partial Refund" on 29th June 2009
    ''        l_DataRow("PlanType") = PrameterPlanType
    ''        If PrameterPlanType = "RETIREMENT" Then
    ''            l_DataRow("ProRatedPercentage") = Me.NumPercentageFactorofMoneyRetirement
    ''        ElseIf PrameterPlanType = "SAVINGS" Then
    ''            l_DataRow("ProRatedPercentage") = Me.NumPercentageFactorofMoneySavings
    ''        Else
    ''            l_DataRow("ProRatedPercentage") = 0.0
    ''        End If



    ''        'Added by Ganeswar  PlanType for "Partial Refund" on 29th June 2009
    ''        l_RefundRequestDataTable.Rows.Add(l_DataRow)

    ''        '****************************************************
    ''        'This segment for making Refund Request Details.. 
    ''        '****************************************************

    ''        'This Delcaration for Updating the Refund Request Details.

    ''        Dim l_String_AccountType As String
    ''        Dim l_String_UniqueID As String
    ''        Dim l_String_RefRequestsID As String
    ''        Dim l_String_AccountBreakDownType As String

    ''        Dim l_Decimal_PersonalInterest As Decimal
    ''        Dim l_Decimal_YmcaInterest As Decimal
    ''        Dim l_Decimal_AccountTotal As Decimal
    ''        Dim l_Decimal_PersonalPostTax As Decimal
    ''        Dim l_Decimal_PersonalPreTax As Decimal
    ''        Dim l_Decimal_PersonalTotal As Decimal
    ''        Dim l_Decimal_YMCAPreTax As Decimal
    ''        Dim l_Decimal_YMCATotal As Decimal
    ''        Dim l_Decimal_GrandTotal As Decimal
    ''        Dim l_Decimal_Total As Decimal
    ''        Dim l_Decimal_PreTax As Decimal
    ''        Dim l_Decimal_PostTax As Decimal
    ''        Dim l_Decimal_Interst As Decimal

    ''        Dim l_Decimal_TDAmount As Decimal = 0.0
    ''        Dim l_Decimal_OtherAmount As Decimal = 0.0

    ''        Dim l_Integer_SortOrder As Integer

    ''        'Plan Split Variable 
    ''        Dim l_String_AccountGroup As String = ""
    ''        l_RefundRequestDetailsDataTable = DirectCast(Session("AtsRefRequestDetails"), DataTable).Clone
    ''        'Hafiz 03Feb06 Cache-Session
    ''        Dim l_bool_Selected As Boolean = False
    ''        'BY Aparna YREN-3262 
    ''        If Me.RefundType = "HARD" Then
    ''            Session("ReportHWL5_C19") = False
    ''        End If
    ''        For Each l_ContributionDataRow In l_ContributionDataTable.Rows

    ''            If l_ContributionDataRow.RowState <> DataRowState.Deleted Then ' If the row is deleted then Don;t cont the values
    ''                If Me.RefundType = "VOL" Then
    ''                    If l_ContributionDataRow("Selected").GetType.ToString() = "System.DBNull" Then
    ''                        l_bool_Selected = False
    ''                    Else
    ''                        l_bool_Selected = DirectCast(l_ContributionDataRow("Selected"), Boolean)
    ''                    End If
    ''                Else
    ''                    l_bool_Selected = True
    ''                End If

    ''                If l_bool_Selected = True Then
    ''                    If Not (l_ContributionDataRow("AccountType").GetType.ToString = "System.DBNull") Then

    ''                        If Not (DirectCast(l_ContributionDataRow("AccountType"), String) = "Total") Then


    ''                            l_Decimal_PreTax = 0.0
    ''                            l_Decimal_PostTax = 0.0
    ''                            l_Decimal_Interst = 0.0
    ''                            l_Decimal_PersonalPostTax = 0.0
    ''                            l_Decimal_PersonalPreTax = 0.0
    ''                            l_Decimal_PersonalInterest = 0.0
    ''                            l_Decimal_PersonalTotal = 0.0
    ''                            l_Decimal_YMCAPreTax = 0.0
    ''                            l_Decimal_YmcaInterest = 0.0
    ''                            l_Decimal_YMCATotal = 0.0
    ''                            l_Decimal_AccountTotal = 0.0
    ''                            l_Decimal_GrandTotal = 0.0
    ''                            Session("OnlyYmcaSideforAM_C19") = Nothing
    ''                            l_String_AccountType = DirectCast(l_ContributionDataRow("AccountType"), String).Trim.ToUpper
    ''                            l_String_AccountGroup = DirectCast(l_ContributionDataRow("AccountGroup"), String).Trim.ToUpper()
    ''                            'Commented by Aparna -yren-3262
    ''                            ' l_Decimal_Total = DirectCast(l_ContributionDataRow("Total"), Decimal)
    ''                            'by Aparna YREN-3262 Chcek for Hardship-21/05/2007
    ''                            'If Hard then dont include TD Amount for decideing the Release blank report
    ''                            If Me.RefundType = "HARD" Then
    ''                                If l_String_AccountGroup = m_const_RetirementPlan_AP Or l_String_AccountGroup = m_const_RetirementPlan_RP Or l_String_AccountGroup = m_const_SavingsPlan_RT Then
    ''                                    l_Decimal_Total = DirectCast(l_ContributionDataRow("Total"), Decimal)
    ''                                    'by Aparna
    ''                                    'Set Sesion To decide which report to call
    ''                                    Session("ReportHWL5_C19") = True
    ''                                Else

    ''                                    l_Decimal_Total = 0
    ''                                End If
    ''                            Else
    ''                                l_Decimal_Total = DirectCast(l_ContributionDataRow("Total"), Decimal)
    ''                            End If

    ''                            'by aparna -mnyHardShipAmt is not getting updated 06/06/2007- as per Email from Raj Adusumilli
    ''                            'In hardship withdrawal form  “Contribution to Tax-Deferred savings plan Account”---Amount should show HARDSHIP AMOUNT
    ''                            ' TD amount not considered-- need to check the TD amount

    ''                            'by aparna -06/06/2007

    ''                            If l_String_AccountGroup = m_const_SavingsPlan_TD Or l_String_AccountGroup = m_const_SavingsPlan_TM Then
    ''                                l_Decimal_TDAmount += DirectCast(l_ContributionDataRow("Total"), Decimal)
    ''                            End If

    ''                            'start - commented by hafiz on 11-May-2007 for YREN-3118
    ''                            'If l_String_AccountType.Trim.ToUpper = "TD" Or l_String_AccountType.Trim.ToUpper = "TM" Then
    ''                            '    l_Decimal_TDAmount += l_Decimal_Total
    ''                            '    'Shubhrata Mar 2nd,2007 YREN-3118
    ''                            '    Session("TotalAmtForReleaseBlnk_C19") = l_Decimal_TDAmount
    ''                            '    'Shubhrata Mar 2nd,2007 YREN-3118
    ''                            'Else
    ''                            '    l_Decimal_OtherAmount += l_Decimal_Total
    ''                            '    'Shubhrata Mar 2nd,2007 YREN-3118
    ''                            '    Session("TotalAmtForReleaseBlnk_C19") = l_Decimal_OtherAmount
    ''                            '    'Shubhrata Mar 2nd,2007 YREN-3118
    ''                            'End If
    ''                            'end - commented by hafiz on 11-May-2007 for YREN-3118
    ''                            'start - added by hafiz on 11-May-2007 for YREN-3118
    ''                            l_Decimal_OtherAmount += l_Decimal_Total
    ''                            'Shubhrata Mar 2nd,2007 YREN-3118
    ''                            Session("TotalAmtForReleaseBlnk_C19") = l_Decimal_OtherAmount
    ''                            'Shubhrata Mar 2nd,2007 YREN-3118
    ''                            'end - added by hafiz on 11-May-2007 for YREN-3118
    ''                            Session("inOtherAmount_C19") = l_Decimal_OtherAmount

    ''                            'If l_String_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPostTax + l_Decimal_PersonalPreTax) = 0.0) Then


    ''                            '    ' l_String_AccountBreakDownType = "07"
    ''                            '    'l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

    ''                            '    l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
    ''                            '    'l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)

    ''                            '    If Not l_DataRow Is Nothing Then
    ''                            '        'The Row is already exist So. Replace the values.

    ''                            '        l_DataRow("YMCAPreTax") = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAPreTax"), Decimal)
    ''                            '        l_DataRow("YMCAInterest") = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
    ''                            '        l_DataRow("YMCATotal") = DirectCast(l_ContributionDataRow("YMCAPreTax"), Decimal) + DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
    ''                            '        l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
    ''                            '        l_DataRow("GrandTotal") = l_DataRow("AcctTotal")
    ''                            '    Else
    ''                            '        ' The row is not present, So create a new Row
    ''                            '        '' Row is not present

    ''                            '        l_DataRow = l_RefundRequestDetailsDataTable.NewRow

    ''                            '        '		m.YmcaPreTax 	= c_Available.YmcaPreTax
    ''                            '        '		m.YmcaInterest	= c_Available.YmcaInterestBalance
    ''                            '        '		m.YMCATotal		= m.YMCAPreTax + m.YmcaInterest
    ''                            '        '		m.AcctTotal		= m.YMCATotal
    ''                            '        '		m.GrandTotal	= m.YMCATotal

    ''                            '        l_Decimal_YMCAPreTax = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal)
    ''                            '        l_Decimal_YmcaInterest = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal)
    ''                            '        l_Decimal_YMCATotal = l_Decimal_YMCAPreTax + l_Decimal_YmcaInterest
    ''                            '        l_Decimal_AccountTotal = l_Decimal_YMCATotal
    ''                            '        l_Decimal_GrandTotal = l_Decimal_YMCATotal

    ''                            '        l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
    ''                            '        l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
    ''                            '        l_DataRow("YMCATotal") = l_Decimal_YMCATotal
    ''                            '        l_DataRow("AcctTotal") = l_Decimal_AccountTotal
    ''                            '        l_DataRow("GrandTotal") = l_Decimal_GrandTotal

    ''                            '        'Assign the other Values. 

    ''                            '        ''l_DataRow("Uniqueid") = l_ContributionDataRow("")
    ''                            '        ''l_DataRow("RefRequestsID") = l_ContributionDataRow("")
    ''                            '        l_DataRow("AcctType") = l_String_AccountType
    ''                            '        'Changed by Shubhrata Aug14th,2007
    ''                            '        'l_DataRow("PersonalPostTax") = l_ContributionDataRow("Taxable")
    ''                            '        'l_DataRow("PersonalPreTax") = l_ContributionDataRow("Non-Taxable")
    ''                            '        'Changed by Shubhrata Aug14th,2007
    ''                            '        l_DataRow("PersonalPostTax") = l_ContributionDataRow("Non-Taxable")
    ''                            '        l_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
    ''                            '        l_DataRow("PersonalInterest") = l_ContributionDataRow("Interest")
    ''                            '        l_DataRow("PersonalTotal") = l_ContributionDataRow("Emp.Total")

    ''                            '        l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
    ''                            '        l_DataRow("SortOrder") = l_Integer_SortOrder

    ''                            '        l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

    ''                            '    End If

    ''                            '    l_ContributionDataRow("YMCATaxable") = "0.00"

    ''                            '    l_Decimal_YMCAPreTax = 0.0
    ''                            '    l_Decimal_YmcaInterest = 0.0
    ''                            '    l_Decimal_AccountTotal = 0.0
    ''                            '    l_Decimal_GrandTotal = 0.0

    ''                            '    l_Decimal_YMCATotal = 0.0
    ''                            '    l_Decimal_PersonalTotal = 0.0
    ''                            'Else

    ''                            '' Do calculation on Refund Request Details side.
    ''                            If l_ContributionDataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
    ''                                l_Decimal_PersonalTotal = 0.0
    ''                                l_ContributionDataRow("Emp.Total") = "0.00"
    ''                            Else
    ''                                l_Decimal_PersonalTotal = DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
    ''                            End If

    ''                            If l_ContributionDataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
    ''                                l_Decimal_YMCATotal = 0.0
    ''                                l_ContributionDataRow("YMCATotal") = "0.00"
    ''                            Else
    ''                                l_Decimal_YMCATotal = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
    ''                            End If

    ''                            'End If

    ''                            ''' Do while .. Loop
    ''                            Do While (l_Decimal_PersonalTotal + l_Decimal_YMCATotal) > 0.0
    ''                                l_Decimal_Interst = 0.0
    ''                                If l_String_AccountGroup = m_const_RetirementPlan_AM And l_Decimal_PersonalTotal = 0.0 Then
    ''                                    Session("OnlyYmcaSideforAM_C19") = True
    ''                                    If l_bool_IsRWPR = True Then
    ''                                        l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, False, True, False, False)
    ''                                        l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)
    ''                                        l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
    ''                                    Else
    ''                                        l_String_AccountBreakDownType = "07"
    ''                                        l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)
    ''                                        l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
    ''                                        'l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)
    ''                                    End If
    ''                                End If


    ''                                If DirectCast(l_ContributionDataRow("YMCATotal"), Decimal) > 0.0 Then
    ''                                    '' If YMCA Total is greater than 0.00
    ''                                    If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
    ''                                    Else
    ''                                        l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, False, True, False, False)


    ''                                        l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

    ''                                        l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
    ''                                        ' l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)
    ''                                    End If


    ''                                    If Not l_DataRow Is Nothing Then

    ''                                        'YmcaPreTax		WITH	r_RefRequestDetails.YmcaPreTax		+ 	;
    ''                                        '							c_Available.YmcaPreTax,						;
    ''                                        'YmcaInterest	WITH	r_RefRequestDetails.YmcaInterest		+ 	;
    ''                                        '							c_Available.YmcaInterestBalance,			;
    ''                                        'YMCATotal		WITH	r_RefRequestDetails.YMCATotal			+	;
    ''                                        '							c_Available.YMCATotal						;
    ''                                        'AcctTotal		WITH	r_RefRequestDetails.PersonalTotal	+	;
    ''                                        '							r_RefRequestDetails.YMCATotal, 			;
    ''                                        'GrandTotal		WITH	r_RefRequestDetails.AcctTotal 			;

    ''                                        'Vipul 15Feb2006 YRST-2088
    ''                                        'l_DataRow("YMCAPreTax") = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAPreTax"), Decimal)
    ''                                        'Should be the statement below
    ''                                        l_DataRow("YMCAPreTax") = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAPreTax"), Decimal)
    ''                                        'Vipul 15Feb2006 YRST-2088

    ''                                        l_DataRow("YMCAInterest") = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
    ''                                        l_DataRow("YMCATotal") = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
    ''                                        l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
    ''                                        l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

    ''                                    Else
    ''                                        l_DataRow = l_RefundRequestDetailsDataTable.NewRow

    ''                                        'm.YmcaPreTax = c_Available.YmcaPreTax
    ''                                        'm.YmcaInterest = c_Available.YmcaInterestBalance
    ''                                        'm.YMCATotal = m.YmcaTotal + c_Available.YmcaTotal
    ''                                        'm.AcctTotal = m.AcctTotal + m.YMCATotal
    ''                                        'm.GrandTotal = m.GrandTotal + m.YMCATotal

    ''                                        l_Decimal_YMCAPreTax = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal)
    ''                                        l_Decimal_YmcaInterest = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal)
    ''                                        l_Decimal_YMCATotal = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
    ''                                        'l_Decimal_YMCATotal += DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
    ''                                        'l_Decimal_AccountTotal += l_Decimal_YMCATotal
    ''                                        'l_Decimal_GrandTotal += l_Decimal_YMCATotal
    ''                                        l_Decimal_AccountTotal = l_Decimal_YMCATotal
    ''                                        l_Decimal_GrandTotal = l_Decimal_YMCATotal


    ''                                        l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
    ''                                        l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
    ''                                        l_DataRow("YMCATotal") = l_Decimal_YMCATotal
    ''                                        l_DataRow("AcctTotal") = l_Decimal_AccountTotal
    ''                                        l_DataRow("GrandTotal") = l_Decimal_GrandTotal

    ''                                        '' Add additional  Detalis..
    ''                                        l_DataRow("AcctType") = l_String_AccountType
    ''                                        l_DataRow("PersonalPostTax") = "0.00"
    ''                                        l_DataRow("PersonalPreTax") = "0.00"
    ''                                        l_DataRow("PersonalInterest") = "0.00"
    ''                                        l_DataRow("PersonalTotal") = "0.00"

    ''                                        l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
    ''                                        l_DataRow("SortOrder") = l_Integer_SortOrder

    ''                                        l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

    ''                                    End If

    ''                                    l_ContributionDataRow("YMCATaxable") = "0.0"
    ''                                    l_ContributionDataRow("YMCAInterest") = "0.0"
    ''                                    l_ContributionDataRow("YMCATotal") = "0.0"

    ''                                    l_Decimal_YMCAPreTax = 0.0
    ''                                    l_Decimal_YmcaInterest = 0.0
    ''                                    l_Decimal_YMCATotal = 0.0
    ''                                    l_Decimal_AccountTotal = 0.0
    ''                                    l_Decimal_GrandTotal = 0.0

    ''                                    'l_Decimal_PersonalPostTax = 0.0
    ''                                    'l_Decimal_PersonalPreTax = 0.0
    ''                                    'l_Decimal_PersonalInterest = 0.0
    ''                                    'l_Decimal_PersonalTotal = 0.0

    ''                                    '' Make YMCA total is Thru.
    ''                                    l_Decimal_YMCATotal = 0.0


    ''                                ElseIf DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) > 0.0 Then
    ''                                    '' If Personal Total is greater than 0.00
    ''                                    If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
    ''                                    Else
    ''                                        l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, False, True)

    ''                                        l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

    ''                                        l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
    ''                                        'l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)
    ''                                    End If


    ''                                    If DirectCast(l_ContributionDataRow("Taxable"), Decimal) > 0.0 Then

    ''                                        l_Decimal_PreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
    ''                                        l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
    ''                                        l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)

    ''                                        'l_Decimal_Interst = l_Decimal_Interst * (l_Decimal_PostTax / (DirectCast(l_ContributionDataRow("Emp.Total"), Decimal) - DirectCast(l_ContributionDataRow("Interest"), Decimal)))
    ''                                        'Commented by Shubhrata Nov 14th 2006,Reason YREN 2766-to change in the interest calculation as
    ''                                        'stated by George Brim on Nov 14th by email.
    ''                                    Else
    ''                                        l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
    ''                                    End If

    ''                                    'Priya 06-Jan-2009 YRS 5.0-529 Incorrect atsRefRequestDetails records for SS account   
    ''                                    If (l_String_AccountGroup = m_const_RetirementPlan_SS) Then
    ''                                        'for PersonalPretax and Interest AccountDreakDownType = 2
    ''                                        If (l_Decimal_PreTax > 0.0 Or l_Decimal_Interst > 0.0) Then

    ''                                            Dim l_String_Account_BreakDown_Type As String
    ''                                            Dim l_Integer_Sort_Order As Integer
    ''                                            Dim l_SS_DataRow As DataRow

    ''                                            l_String_Account_BreakDown_Type = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, True, False)
    ''                                            l_Integer_Sort_Order = Me.GetAccountBreakDownSortOrder(l_String_Account_BreakDown_Type)

    ''                                            l_SS_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)

    ''                                            If l_SS_DataRow Is Nothing Then
    ''                                                l_SS_DataRow = l_RefundRequestDetailsDataTable.NewRow


    ''                                                l_Decimal_PostTax = 0.0
    ''                                                'Priya : 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
    ''                                                l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst + l_Decimal_PreTax
    ''                                                'l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst
    ''                                                'End 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records

    ''                                                l_Decimal_AccountTotal = l_Decimal_Total
    ''                                                l_Decimal_GrandTotal = l_Decimal_AccountTotal

    ''                                                l_SS_DataRow("PersonalPreTax") = l_Decimal_PreTax
    ''                                                l_SS_DataRow("PersonalPostTax") = l_Decimal_PostTax
    ''                                                l_SS_DataRow("PersonalInterest") = l_Decimal_Interst
    ''                                                l_SS_DataRow("PersonalTotal") = l_Decimal_Total

    ''                                                l_SS_DataRow("AcctTotal") = l_Decimal_AccountTotal
    ''                                                l_SS_DataRow("GrandTotal") = l_Decimal_GrandTotal
    ''                                                l_SS_DataRow("AcctType") = l_String_AccountType

    ''                                                l_SS_DataRow("YMCAPreTax") = "0.00"
    ''                                                l_SS_DataRow("YMCAInterest") = "0.00"
    ''                                                l_SS_DataRow("YMCATotal") = "0.00"

    ''                                                l_SS_DataRow("AcctBreakDownType") = l_String_Account_BreakDown_Type
    ''                                                l_SS_DataRow("SortOrder") = l_Integer_Sort_Order

    ''                                                l_Decimal_Interst = 0.0
    ''                                                l_Decimal_PreTax = 0.0

    ''                                                l_RefundRequestDetailsDataTable.Rows.Add(l_SS_DataRow)
    ''                                            Else
    ''                                                l_SS_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
    ''                                                l_SS_DataRow("PersonalPostTax") = 0.0
    ''                                                l_SS_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + DirectCast(l_ContributionDataRow("Interest"), Decimal)
    ''                                                'Priya : 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
    ''                                                l_SS_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + 0.0 + l_Decimal_Interst + l_Decimal_PreTax
    ''                                                'l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) + l_Decimal_Interst
    ''                                                'End 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
    ''                                                l_SS_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
    ''                                                l_SS_DataRow("GrandTotal") = l_DataRow("AcctTotal")
    ''                                            End If

    ''                                        End If

    ''                                    End If
    ''                                    'End 06-Jan-2009 Incorrect atsRefRequestDetails records for SS account   

    ''                                    If Not l_DataRow Is Nothing Then

    ''                                        'PersonalPostTax	WITH 	r_RefRequestDetails.PersonalPostTax		+	;
    ''                                        '										c_Available.PersonalPostTax,				;
    ''                                        'PersonalInterest	WITH	r_RefRequestDetails.PersonalInterest	+ 	;
    ''                                        '       								lnInterest,							 				;
    ''                                        'PersonalTotal		WITH 	r_RefRequestDetails.PersonalTotal		+ 	;
    ''                                        '										c_Available.PersonalPreTax					+	;
    ''                                        '										lnInterest 											;
    ''                                        'AcctTotal			WITH 	r_RefRequestDetails.PersonalTotal		+	;
    ''                                        '								r_RefRequestDetails.PersonalTotal,			;
    ''                                        'GrandTotal			WITH  r_RefRequestDetails.AcctTotal 	

    ''                                        'TODO : verify with client for the logic as per foxpro logic written above but commented - Hafiz & Shubhrata on 21-Nov-2006 
    ''                                        l_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
    ''                                        l_DataRow("PersonalPostTax") = DirectCast(l_DataRow("PersonalPostTax"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
    ''                                        l_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + DirectCast(l_ContributionDataRow("Interest"), Decimal)
    ''                                        'Priya : 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
    ''                                        l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) + l_Decimal_Interst + l_Decimal_PreTax
    ''                                        'l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) + l_Decimal_Interst
    ''                                        'End 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
    ''                                        l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
    ''                                        l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

    ''                                    Else

    ''                                        l_DataRow = l_RefundRequestDetailsDataTable.NewRow

    ''                                        'm.PersonalPostTax = c_Available.PersonalPostTax
    ''                                        'm.PersonalInterest = lnInterest
    ''                                        'm.PersonalTotal = m.PersonalPostTax + m.PersonalInterest
    ''                                        'm.AcctTotal = m.PersonalTotal
    ''                                        'm.GrandTotal = m.AcctTotal

    ''                                        l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
    ''                                        'Priya : 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records
    ''                                        l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst + l_Decimal_PreTax
    ''                                        'l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst
    ''                                        'End 19-Dec-2008 : YRS 5.0-529 Incorrect atsRefRequestDetails records

    ''                                        l_Decimal_AccountTotal = l_Decimal_Total
    ''                                        l_Decimal_GrandTotal = l_Decimal_AccountTotal

    ''                                        l_DataRow("PersonalPreTax") = l_Decimal_PreTax
    ''                                        l_DataRow("PersonalPostTax") = l_Decimal_PostTax
    ''                                        l_DataRow("PersonalInterest") = l_Decimal_Interst
    ''                                        l_DataRow("PersonalTotal") = l_Decimal_Total
    ''                                        l_DataRow("AcctTotal") = l_Decimal_AccountTotal
    ''                                        l_DataRow("GrandTotal") = l_Decimal_GrandTotal

    ''                                        l_DataRow("AcctType") = l_String_AccountType
    ''                                        l_DataRow("YMCAPreTax") = "0.00"
    ''                                        l_DataRow("YMCAInterest") = "0.00"
    ''                                        l_DataRow("YMCATotal") = "0.00"

    ''                                        l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
    ''                                        l_DataRow("SortOrder") = l_Integer_SortOrder

    ''                                        l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

    ''                                    End If

    ''                                    'REPLACE	PersonalPostTax			WITH 	0.00,												;
    ''                                    'PersonalInterestBalance	WITH 	c_Available.PersonalInterestBalance	-	;
    ''                                    '									 	lnInterest 										;
    ''                                    '

    ''                                    'REPLACE	PersonalTotal				WITH 	PersonalPreTax		+ 							;
    ''                                    '                     PersonalInterestBalance()

    ''                                    l_ContributionDataRow("Taxable") = "0.00"
    ''                                    l_ContributionDataRow("Interest") = DirectCast(l_ContributionDataRow("Interest"), Decimal) - l_Decimal_Interst
    ''                                    l_ContributionDataRow("Emp.Total") = l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest

    ''                                    'l_Decimal_PersonalPostTax = 0.0
    ''                                    'l_Decimal_PersonalInterest = 0.0
    ''                                    'l_Decimal_PersonalTotal = 0.0
    ''                                    'l_Decimal_AccountTotal = 0.0
    ''                                    'l_Decimal_GrandTotal = 0.0

    ''                                    l_Decimal_PersonalTotal = 0.0

    ''                                Else
    ''                                    '' Do otherwise
    ''                                    If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
    ''                                    Else
    ''                                        l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, True, False)
    ''                                        l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

    ''                                        l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
    ''                                        'l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)
    ''                                    End If




    ''                                    If DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) > 0.0 Then

    ''                                        l_Decimal_PreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
    ''                                        l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
    ''                                        l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)

    ''                                        l_Decimal_Interst = l_Decimal_Interst * (l_Decimal_PreTax / (DirectCast(l_ContributionDataRow("Emp.Total"), Decimal) - DirectCast(l_ContributionDataRow("Interest"), Decimal)))

    ''                                    Else
    ''                                        l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
    ''                                    End If

    ''                                    If Not l_DataRow Is Nothing Then

    ''                                        'PersonalPreTax		WITH 	r_RefRequestDetails.PersonalPreTax		+ 	;
    ''                                        '											c_Available.PersonalPreTax,					;
    ''                                        'PersonalInterest	WITH	r_RefRequestDetails.PersonalInterest	+	;
    ''                                        '											lnInterest 											;
    ''                                        'PersonalTotal		WITH 	r_RefRequestDetails.PersonalTotal		+	;
    ''                                        '											PersonalPreTax									+	;
    ''                                        '											lnInterest 											;
    ''                                        'AcctTotal			WITH 	r_RefRequestDetails.PersonalTotal		+	;
    ''                                        '											r_RefRequestDetails.PersonalTotal,			;
    ''                                        'GrandTotal			WITH  r_RefRequestDetails.AcctTotal 				;

    ''                                        l_DataRow("PersonalPreTax") = DirectCast(l_DataRow("PersonalPreTax"), Decimal) + DirectCast(l_ContributionDataRow("Taxable"), Decimal)
    ''                                        l_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + l_Decimal_Interst
    ''                                        l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Taxable"), Decimal) + l_Decimal_Interst
    ''                                        l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("PersonalTotal"), Decimal)
    ''                                        l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

    ''                                    Else
    ''                                        'm.PersonalPreTax = c_Available.PersonalPreTax
    ''                                        'm.PersonalInterest = lnInterest
    ''                                        'm.PersonalTotal = m.PersonalPreTax + lnInterest
    ''                                        'm.AcctTotal = m.PersonalTotal
    ''                                        'm.GrandTotal = m.AcctTotal

    ''                                        l_DataRow = l_RefundRequestDetailsDataTable.NewRow

    ''                                        l_Decimal_PersonalPreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
    ''                                        l_Decimal_PersonalInterest = l_Decimal_Interst
    ''                                        l_Decimal_PersonalTotal = l_Decimal_Interst + l_Decimal_PersonalPreTax
    ''                                        l_Decimal_AccountTotal = l_Decimal_PersonalTotal
    ''                                        l_Decimal_GrandTotal = l_Decimal_AccountTotal

    ''                                        l_DataRow("PersonalPreTax") = l_Decimal_PersonalPreTax
    ''                                        l_DataRow("PersonalInterest") = l_Decimal_PersonalInterest
    ''                                        l_DataRow("PersonalTotal") = l_Decimal_PersonalTotal
    ''                                        l_DataRow("AcctTotal") = l_Decimal_AccountTotal
    ''                                        l_DataRow("GrandTotal") = l_Decimal_GrandTotal

    ''                                        l_DataRow("AcctType") = l_String_AccountType
    ''                                        l_DataRow("PersonalPostTax") = l_ContributionDataRow("Non-Taxable")
    ''                                        l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
    ''                                        l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
    ''                                        l_DataRow("YMCATotal") = l_Decimal_YMCATotal

    ''                                        l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
    ''                                        l_DataRow("SortOrder") = l_Integer_SortOrder

    ''                                        l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

    ''                                    End If

    ''                                    'PersonalPreTax				WITH 	0.00, 												;
    ''                                    'PersonalInterestBalance	WITH 	c_Available.PersonalInterestBalance	-		;
    ''                                    '													lnInterest 											;
    ''                                    '	

    ''                                    'REPLACE PersonalTotal				WITH c_Available.PersonalInterestBalance			;
    ''                                    '

    ''                                    l_ContributionDataRow("Non-Taxable") = "0.00"
    ''                                    l_ContributionDataRow("Interest") = DirectCast(l_ContributionDataRow("Interest"), Decimal) - l_Decimal_Interst
    ''                                    l_ContributionDataRow("Emp.Total") = l_ContributionDataRow("Interest")

    ''                                    'm.PersonalPreTax = 0.0
    ''                                    'm.PersonalInterest = 0.0
    ''                                    'm.PersonalTotal = 0.0
    ''                                    'm.AcctTotal = 0.0
    ''                                    'm.GrandTotal = 0.0

    ''                                    l_Decimal_PersonalPostTax = 0.0
    ''                                    l_Decimal_PersonalInterest = 0.0
    ''                                    l_Decimal_PersonalTotal = 0.0
    ''                                    l_Decimal_AccountTotal = 0.0
    ''                                    l_Decimal_GrandTotal = 0.0

    ''                                    l_Decimal_PersonalTotal = 0.0
    ''                                    l_Decimal_YMCATotal = 0.0

    ''                                End If
    ''                            Loop

    ''                        End If
    ''                    End If
    ''                End If
    ''            End If
    ''        Next

    ''        ''' Update the database

    ''        'l_RefundRequestDataTable.AcceptChanges()
    ''        'l_RefundRequestDetailsDataTable.AcceptChanges()

    ''        l_RefundDataSet = New DataSet("RefundRequest")

    ''        'Populate HardShip Refund amount if inTDAmounts > 0.00
    ''        'Added on 04-27-06 Ragesh 34231

    ''        If l_Decimal_TDAmount > 0.0 Then
    ''            l_DataRow = l_RefundRequestDataTable.Rows(0)
    ''            'l_DataRow("HardShipAmt") = l_Decimal_TDAmount
    ''            If PrameterPlanType = "SAVINGS" Then
    ''                l_DataRow("HardShipAmt") = Me.Session("TDTotalAmount") ' "0.00"
    ''            End If
    ''        End If
    ''        'l_RefundDataSet.Tables.Add(l_RefundRequestDataTable)
    ''        'l_RefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable)
    ''        If l_FinalrefundDataSet.Tables.Count = 0 Then
    ''            l_FinalrefundDataSet.Tables.Add(l_RefundRequestDataTable)
    ''            l_FinalrefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable)
    ''        Else
    ''            l_RefundDataSet.Tables.Add(l_RefundRequestDataTable)
    ''            l_RefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable)
    ''        End If
    ''        Dim StringUniqueId As ArrayList = New ArrayList
    ''        If bool_SaveRefund Then
    ''            StringUniqueId = YMCARET.YmcaBusinessObject.RefundRequest.InsertRefunds(l_RefundDataSet, l_FinalrefundDataSet, bool_IncludeYMCAAcct, bool_IncludeYMCALegacyAcct)
    ''        End If
    ''        'updating the generated UniqueId returned from the proc into the cache dataset.
    ''        'Added on Aug 2009 By Ganeswar To keep the Unique Id in Seesion so that Multiple report Can Open in the squence
    ''        If StringUniqueId.Count > 0 Then
    ''            For UniqueId As Integer = 0 To StringUniqueId.Count - 1
    ''                If UniqueId = 1 Then
    ''                    l_RefundDataSet.Tables(0).Rows(0)(0) = StringUniqueId.Item(UniqueId)
    ''                    l_RefundDataSet.Tables(0).AcceptChanges()
    ''                    Me.Session("RequestUniqueId") = l_RefundDataSet.Tables(0).Rows(0)(0)

    ''                Else
    ''                    l_FinalrefundDataSet.Tables(0).Rows(0)(0) = StringUniqueId.Item(UniqueId)
    ''                    l_FinalrefundDataSet.Tables(0).AcceptChanges()
    ''                    Me.Session("DetailsUniqueId") = l_FinalrefundDataSet.Tables(0).Rows(0)(0)
    ''                    Session("AtsRefRequests") = l_FinalrefundDataSet.Tables(0)
    ''                    Session("AtsRefRequestDetails") = l_FinalrefundDataSet.Tables(0)
    ''                End If
    ''            Next
    ''        End If
    ''        'Added on Aug 2009 By Ganeswar 
    ''    Catch ex As Exception
    ''        Throw
    ''    End Try

    ''End Function

#End Region



#Region "Refund Processing Functions"
    Public Function SaveRefundRequestProcess(ByVal parameterDataGridDeductions As DataGrid, ByVal l_dec_TDAmount As Decimal, ByVal l_dec_TDRequestedAmount As Decimal, ByVal l_decimalTaxAmount As Decimal, ByVal l_ForcetoPersonalWithdrawal As Boolean, ByVal l_dec_DeferedRollOverAmount As Decimal) As String

        Dim l_ErrorMessage As String = String.Empty

        '' 
        Dim l_Deciaml_FederalTax As Decimal
        Dim l_Decimal_DisbNonTaxable As Decimal
        Dim l_Decimal_DisbTaxable As Decimal

        Dim l_Decimal_PersonalTax As Decimal
        Dim l_Decimal_YMCATax As Decimal
        Dim l_Decimal_YMCAInterest As Decimal

        Dim l_lnz As Integer
        Dim l_lnNum As Integer
        Dim l_First As Boolean


        Dim l_Decimal_TaxableCtr As Decimal
        Dim l_Decimal_TaxCtr As Decimal
        Dim l_Decimal_NonTaxCtr As Decimal

        Dim l_PesonID As String
        Dim l_YMCAID As String
        Dim l_DisbursementID As String

        Dim l_CurrencyCode As String
        'Added by Dilip On 24-11-2009 Defered Rollover
        Dim l_Decimal_DeferedTaxablePercentage As Decimal
        Dim l_Decimal_DeferedNonTaxablePercentage As Decimal
        'Added by Dilip On 24-11-2009 Defered Rollover


        Try


            '***********************************************************************************************
            '* Create an Array of the Available Annuity Basis Types
            '***********************************************************************************************
            Me.CreateAnnuityBasisTypes()


            '********************************************************************************************
            '* Set the tables to add the Rows 
            '********************************************************************************************
            Me.OpenFiles()

            Me.SetWithholdingDeductionProperties(parameterDataGridDeductions)
            '********************************************************************************************
            '* Set the Accounting Date
            '********************************************************************************************
            'This.idAcctDate = GetAcctDate() 
            'As per the application they never used this Value. This date canbe assigned in Stored Procedure
            'Level if Needed.

            '********************************************************************************************
            '* Get the Address History Identifier
            '********************************************************************************************
            Me.GetAddressID() '' Call this method while assigning the value.


            '***********************************************************************************************
            '* Determine what Currency Code to Use
            '***********************************************************************************************
            Me.CurrencyCode = Me.GetCurrencyCode()


            '***********************************************************************************************
            '* Determine if it's a Hardship Refund
            '***********************************************************************************************

            'If the account type is HardShip then do calculation of HARDSHIP, otherwise ignore this Part.
            '09-Dec-2009        Parveen     Changes for the Hardship and VOL withdrawal Issue
            If Me.RefundType = "HARD" Or CType(Session("ForceToVoluntaryWithdrawal_C19"), Boolean) = True Then
                Dim l_datatable_RefundRequestCreation As DataTable

                'START | 2019.07.30 |  SR | YRS-AT-4498 Starts | Prorate hardship amount across Taxable, Non-Taxable & interests.
                'Me.HardShipCalculation()
                ProrateHardshipAmountForPayee1()
                ProrateCurrentAmountForHardship()
                'END | 2019.07.30 |  SR | YRS-AT-4498 Starts | Prorate hardship amount across Taxable, Non-Taxable & interests.

                Session("Initial_RefRequestId_C19") = Me.SessionRefundRequestID
                l_datatable_RefundRequestCreation = Me.PrepareTableforHardshipProcessing()
                If Me.IsHardShip = False Then
                    Me.RefundType = "VOL"
                Else
                    Me.RefundType = "HARD"
                End If
                Session("NeedsNewRequests_C19") = True
                Me.MakeTableforRefundRequest(l_datatable_RefundRequestCreation, Me.PlanTypeChosen)
            End If

            '***********************************************************************************************
            '* Get the Available Transactions for this FundEvent
            '***********************************************************************************************
            l_ErrorMessage = Me.GetTransactionDetails(Me.FundID, l_ForcetoPersonalWithdrawal)

            '************************************************************************************************
            '* Reduce c_AvailBalance to the Level of c_Refundable
            '* c_AvailBalance became needed when the addition of the transactionid was used to seperate rollover accounts
            '************************************************************************************************
            '*-- Remove any accounts in c_AvailBalance that do not exist in c_Refundable
            '*-- by Annuity Basis Type and Account Type
            '*-- Unless it's a Hardship Refund in which case leave the TD and TM Amounts
            If l_ErrorMessage <> String.Empty Then
                SaveRefundRequestProcess = l_ErrorMessage
                Exit Function
            End If

            Me.ReduceAvailableBalance()

            'Added by Dilip On 24-11-2009 Defered Rollover
            If (Me.ISMarket = -1) Then
                l_Decimal_DeferedTaxablePercentage = CreateDeferedTaxablePercentage()
                l_Decimal_DeferedNonTaxablePercentage = 100 - l_Decimal_DeferedTaxablePercentage
            End If
            'Added by Dilip On 24-11-2009 Defered Rollover

            '***********************************************************************************************
            '* Create Rows for Disbursements, Disbursement Details, Transacts and Refunds
            '***********************************************************************************************
            '***********************************************************************************************
            '* Loop Through the Possible Annuity Basis Types
            '***********************************************************************************************
            Me.CreateRowsForDisbursements(l_dec_TDAmount, l_dec_TDRequestedAmount)

            If (Me.ISMarket = -1) Then
                SetTransactTypeForMarketWithdrawal()
            End If


            '' Make all tables to Update into DataBase.
            Me.MakeTablesToUpdate(l_decimalTaxAmount)

            '*********************************************************************************************************
            '* Update The Witholdings Table with the Additional Refund Deductions
            '*********************************************************************************************************

            'Neeraj 16-nov-2010 BT- 664: MRD Disbursement linkage

            CreateMrdDisbursementLinkage()


            'End MRD Disbursement linkage
            l_ErrorMessage = Me.UpdateWithHoldingsWithDeductions()

            If l_ErrorMessage <> String.Empty Then

                SaveRefundRequestProcess = l_ErrorMessage

                '      MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", l_ErrorMessage, MessageBoxButtons.Stop, True)
                Exit Function
            End If


            SaveRefundRequestProcess = Me.SaveAllTable(Math.Round(l_decimalTaxAmount, 2), Math.Round(l_dec_DeferedRollOverAmount, 2), Math.Round(l_Decimal_DeferedTaxablePercentage, 2), Math.Round(l_Decimal_DeferedNonTaxablePercentage, 2))

            'SaveRefundRequestProcess = String.Empty            


        Catch ex As Exception

            Throw

        End Try
    End Function
    Private Sub CreateMrdDisbursementLinkage()

        Dim l_DisbursementsDataTable As DataTable
        Dim l_dtMrdRecords As DataTable
        Dim l_drDisbursementRows As DataRow()
        Dim l_dtMrdRecordsDisbursements As DataTable
        Dim l_DisbursementDetailssDataTable As DataTable
        Dim decMrdAmountTobeSatisfied As Decimal
        Dim decDisbursementAmount As Decimal
        Dim drMrdRecordsDisbursement As DataRow
        Dim decMrdDisbursementLinkedAmount As Decimal
        Dim decMrdDisbursementRetirementAmount As Decimal
        Dim decMrdDisbursementSavingsAmount As Decimal

        Dim l_datarowarray_RetirementPlan As DataRow()
        Dim l_datarowarray_SavingsPlan As DataRow()

        Dim drMrdRetirementPlan As DataRow()
        Dim drMrdSavingsPlan As DataRow()

        Dim decDisbursementTaxableAmount As Decimal
        Dim decDisbursementNonTaxableAmount As Decimal
        Dim decMrdDisbursementRetirementTaxableAmount As Decimal
        Dim decMrdDisbursementSavingsTaxableAmount As Decimal
        Dim decMrdDisbursementRetirementNonTaxableAmount As Decimal
        Dim decMrdDisbursementSavingsNonTaxableAmount As Decimal

        Try
            l_dtMrdRecords = CType(Session("dtMrdRecords_C19"), DataTable)

            If HelperFunctions.isEmpty(l_dtMrdRecords) Then
                Exit Sub
            End If
            l_DisbursementsDataTable = DirectCast(Session("R_Disbursements_C19"), DataTable)
            l_DisbursementDetailssDataTable = DirectCast(Session("R_DisbursementDetails_C19"), DataTable)

            l_dtMrdRecordsDisbursements = DirectCast(Session("R_MrdRecordsDisbursements_C19"), DataTable)

            If Not l_DisbursementsDataTable Is Nothing AndAlso l_DisbursementsDataTable.Rows.Count > 0 AndAlso l_dtMrdRecords.Rows.Count > 0 Then
                'get payee1 data from disbursement table
                l_drDisbursementRows = l_DisbursementsDataTable.Select("PayeeEntityTypeCode='PERSON'", "")

                If l_drDisbursementRows.Length > 0 Then
                    For Each drDisbursement As DataRow In l_drDisbursementRows
                        'decDisbursementAmount = CType(drDisbursement("TaxableAmount"), Decimal)
                        'decDisbursementAmount = CType(drDisbursement("TaxableAmount"), Decimal) + CType(drDisbursement("NonTaxableAmount"), Decimal)
                        decDisbursementTaxableAmount = CType(drDisbursement("TaxableAmount"), Decimal)
                        decDisbursementNonTaxableAmount = CType(drDisbursement("NonTaxableAmount"), Decimal)

                        l_datarowarray_RetirementPlan = l_DisbursementDetailssDataTable.Select("PlanType='RETIREMENT' and DisbursementID='" + drDisbursement("UniqueID").ToString() + "'")
                        If l_datarowarray_RetirementPlan.Length > 0 Then
                            For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                                'decMrdDisbursementRetirementAmount += CType(l_datarowarray_RetirementPlan(i)("TaxablePrincipal"), Decimal) + CType(l_datarowarray_RetirementPlan(i)("TaxableInterest"), Decimal)
                                'decMrdDisbursementRetirementAmount += CType(l_datarowarray_RetirementPlan(i)("TaxablePrincipal"), Decimal) + CType(l_datarowarray_RetirementPlan(i)("NonTaxablePrincipal"), Decimal) + CType(l_datarowarray_RetirementPlan(i)("TaxableInterest"), Decimal)
                                decMrdDisbursementRetirementTaxableAmount += CType(l_datarowarray_RetirementPlan(i)("TaxablePrincipal"), Decimal) + CType(l_datarowarray_RetirementPlan(i)("TaxableInterest"), Decimal)
                                decMrdDisbursementRetirementNonTaxableAmount += CType(l_datarowarray_RetirementPlan(i)("NonTaxablePrincipal"), Decimal)
                            Next
                        End If
                        l_datarowarray_SavingsPlan = l_DisbursementDetailssDataTable.Select("PlanType='SAVINGS' and DisbursementID='" + drDisbursement("UniqueID").ToString() + "'")
                        If l_datarowarray_SavingsPlan.Length > 0 Then
                            For i = 0 To l_datarowarray_SavingsPlan.Length - 1
                                'decMrdDisbursementSavingsAmount += CType(l_datarowarray_SavingsPlan(i)("TaxablePrincipal"), Decimal) + CType(l_datarowarray_SavingsPlan(i)("TaxableInterest"), Decimal)
                                'decMrdDisbursementSavingsAmount += CType(l_datarowarray_SavingsPlan(i)("TaxablePrincipal"), Decimal) + CType(l_datarowarray_SavingsPlan(i)("NonTaxablePrincipal"), Decimal) + CType(l_datarowarray_SavingsPlan(i)("TaxableInterest"), Decimal)
                                decMrdDisbursementSavingsTaxableAmount += CType(l_datarowarray_SavingsPlan(i)("TaxablePrincipal"), Decimal) + CType(l_datarowarray_SavingsPlan(i)("TaxableInterest"), Decimal)
                                decMrdDisbursementSavingsNonTaxableAmount += CType(l_datarowarray_SavingsPlan(i)("NonTaxablePrincipal"), Decimal)
                            Next
                        End If

                        drMrdRetirementPlan = l_dtMrdRecords.Select("PlanType='RETIREMENT'", "MrdYear ASC")
                        drMrdSavingsPlan = l_dtMrdRecords.Select("PlanType='SAVINGS'", "MrdYear ASC")

                        drMrdRecordsDisbursement = l_dtMrdRecordsDisbursements.NewRow()
                        If (Not drMrdRetirementPlan Is Nothing AndAlso drMrdRetirementPlan.Length > 0) AndAlso (Not l_datarowarray_RetirementPlan Is Nothing AndAlso l_datarowarray_RetirementPlan.Length > 0) Then
                            'CreateMRDLinkageDataRowPlanWise(drMrdRetirementPlan, l_dtMrdRecordsDisbursements, decMrdDisbursementRetirementAmount, decDisbursementAmount, drDisbursement("UniqueID"))
                            CreateMRDLinkageDataRowPlanWise(drMrdRetirementPlan, l_dtMrdRecordsDisbursements, decMrdDisbursementRetirementTaxableAmount, decMrdDisbursementRetirementNonTaxableAmount, decDisbursementAmount, drDisbursement("UniqueID"))
                        End If

                        If (Not drMrdSavingsPlan Is Nothing AndAlso drMrdSavingsPlan.Length > 0) AndAlso (Not l_datarowarray_SavingsPlan Is Nothing AndAlso l_datarowarray_SavingsPlan.Length > 0) Then
                            'CreateMRDLinkageDataRowPlanWise(drMrdSavingsPlan, l_dtMrdRecordsDisbursements, decMrdDisbursementSavingsAmount, decDisbursementAmount, drDisbursement("UniqueID"))
                            CreateMRDLinkageDataRowPlanWise(drMrdSavingsPlan, l_dtMrdRecordsDisbursements, decMrdDisbursementSavingsTaxableAmount, decMrdDisbursementSavingsNonTaxableAmount, decDisbursementAmount, drDisbursement("UniqueID"))
                        End If

                    Next
                End If
                'Added on 09/June/2011 BT:853-Application giving yellow page on Withdrawal Re-Issue.Mrd Linkage with rollover disbursement
                'get payee2 data from disbursement table
                l_drDisbursementRows = l_DisbursementsDataTable.Select("PayeeEntityTypeCode<>'PERSON'", "")

                If l_drDisbursementRows.Length > 0 Then
                    For Each drDisbursement As DataRow In l_drDisbursementRows

                        drMrdRetirementPlan = l_dtMrdRecords.Select("PlanType='RETIREMENT'", "MrdYear DESC")
                        drMrdSavingsPlan = l_dtMrdRecords.Select("PlanType='SAVINGS'", "MrdYear DESC")

                        If (Not drMrdRetirementPlan Is Nothing AndAlso drMrdRetirementPlan.Length > 0) AndAlso (Not l_datarowarray_RetirementPlan Is Nothing AndAlso l_datarowarray_RetirementPlan.Length > 0) Then
                            drMrdRecordsDisbursement = l_dtMrdRecordsDisbursements.NewRow()
                            'CreateMRDLinkageDataRowPlanWise(drMrdRetirementPlan, l_dtMrdRecordsDisbursements, decMrdDisbursementRetirementAmount, decDisbursementAmount, drDisbursement("UniqueID"))
                            drMrdRecordsDisbursement("guiDisbursementId") = drDisbursement("UniqueID")
                            drMrdRecordsDisbursement("intMrdTrackingId") = drMrdRetirementPlan(0)("intMrdRecordsId")
                            drMrdRecordsDisbursement("mnyPaidAmount") = 0
                            drMrdRecordsDisbursement("mnyTaxablePaidAmount") = 0
                            drMrdRecordsDisbursement("mnyNonTaxablePaidAmount") = 0
                            l_dtMrdRecordsDisbursements.Rows.Add(drMrdRecordsDisbursement)
                            l_dtMrdRecordsDisbursements.AcceptChanges()
                        End If

                        If (Not drMrdSavingsPlan Is Nothing AndAlso drMrdSavingsPlan.Length > 0) AndAlso (Not l_datarowarray_SavingsPlan Is Nothing AndAlso l_datarowarray_SavingsPlan.Length > 0) Then
                            drMrdRecordsDisbursement = l_dtMrdRecordsDisbursements.NewRow()
                            ' CreateMRDLinkageDataRowPlanWise(drMrdSavingsPlan, l_dtMrdRecordsDisbursements, decMrdDisbursementSavingsAmount, decDisbursementAmount, drDisbursement("UniqueID"))
                            drMrdRecordsDisbursement("guiDisbursementId") = drDisbursement("UniqueID")
                            drMrdRecordsDisbursement("intMrdTrackingId") = drMrdSavingsPlan(0)("intMrdRecordsId")
                            drMrdRecordsDisbursement("mnyPaidAmount") = 0
                            drMrdRecordsDisbursement("mnyTaxablePaidAmount") = 0
                            drMrdRecordsDisbursement("mnyNonTaxablePaidAmount") = 0
                            l_dtMrdRecordsDisbursements.Rows.Add(drMrdRecordsDisbursement)
                            l_dtMrdRecordsDisbursements.AcceptChanges()
                        End If

                    Next
                End If

                l_dtMrdRecordsDisbursements.AcceptChanges()
                Session("R_MrdRecordsDisbursements_C19") = l_dtMrdRecordsDisbursements
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Private Sub CreateMRDLinkageDataRowPlanWise(ByVal paramDrPlanWiseMrdRecords As DataRow(), ByVal paramDtMrdRecordsDisbursements As DataTable, ByVal paramPlanWiseDisbursementAmount As Decimal, ByRef paramTotalDisbursementAmount As Decimal, ByVal paramStrDisbursementID As Object)
    Private Sub CreateMRDLinkageDataRowPlanWise(ByVal paramDrPlanWiseMrdRecords As DataRow(), ByVal paramDtMrdRecordsDisbursements As DataTable, ByVal paramTotalDisbursementTaxableAmount As Decimal, ByVal paramTotalDisbursementNonTaxableAmount As Decimal, ByRef paramTotalDisbursementAmount As Decimal, ByVal paramStrDisbursementID As Object)
        Dim drMrdRecord, drMrdRecordsDisbursement As DataRow
        Dim decMrdAmountTobeSatisfied As Decimal
        Dim decMrdDisbursementLinkedAmount As Decimal
        Dim paramPlanWiseDisbursementAmount As Decimal
        Dim paramPlanWiseTaxableAmount As Decimal
        Dim paramPlanWiseNonTaxableAmount As Decimal

        Dim decMrdDisbursementTaxableLinkedAmount As Decimal
        Dim decMrdDisbursementNonTaxableLinkedAmount As Decimal
        Dim decMrdAmountTobeSatisfiedTaxable As Decimal
        Dim decMrdAmountTobeSatisfiedNonTaxable As Decimal
        Try
            '' In AtsMrdRecordsDisbursements table SAVINGS plan MRD amount not updating in mnyPaidAmount
            'NP:  Bug:690:  Attempt to satisfy all unsatisfied MRD records starting from the oldest record.
            paramPlanWiseDisbursementAmount = paramTotalDisbursementTaxableAmount + paramTotalDisbursementNonTaxableAmount
            paramPlanWiseTaxableAmount = paramTotalDisbursementTaxableAmount
            paramPlanWiseNonTaxableAmount = paramTotalDisbursementNonTaxableAmount
            For drindex As Int16 = 0 To paramDrPlanWiseMrdRecords.Length - 1
                drMrdRecord = paramDrPlanWiseMrdRecords(drindex)
                drMrdRecordsDisbursement = paramDtMrdRecordsDisbursements.NewRow()

                decMrdAmountTobeSatisfied = CType(drMrdRecord("Balance"), Decimal)
                decMrdAmountTobeSatisfiedTaxable = CType(drMrdRecord("MRDTaxable"), Decimal)
                decMrdAmountTobeSatisfiedNonTaxable = CType(drMrdRecord("MRDNonTaxable"), Decimal)

                'If this is the last MRD record to link to then link all of the remaining plan balance to this record
                'If drindex = paramDrPlanWiseMrdRecords.Length - 1 Then
                'SG: 2012.03.14: BT-1010: Remaining plan balance is > 0 then create record for MRD record.
                If drindex = paramDrPlanWiseMrdRecords.Length - 1 AndAlso paramPlanWiseDisbursementAmount > 0 Then

                    'forcefully assigning extra disbursement amount with last updated Mrdrecord
                    drMrdRecordsDisbursement("guiDisbursementId") = paramStrDisbursementID
                    drMrdRecordsDisbursement("intMrdTrackingId") = drMrdRecord("intMrdRecordsId")
                    drMrdRecordsDisbursement("mnyPaidAmount") = paramPlanWiseDisbursementAmount
                    drMrdRecordsDisbursement("mnyTaxablePaidAmount") = paramPlanWiseTaxableAmount
                    drMrdRecordsDisbursement("mnyNonTaxablePaidAmount") = paramPlanWiseNonTaxableAmount

                    paramTotalDisbursementAmount -= paramPlanWiseDisbursementAmount
                    paramTotalDisbursementTaxableAmount -= paramPlanWiseTaxableAmount
                    paramTotalDisbursementNonTaxableAmount -= paramPlanWiseNonTaxableAmount

                    decMrdDisbursementLinkedAmount = paramPlanWiseDisbursementAmount
                    decMrdDisbursementTaxableLinkedAmount = paramPlanWiseTaxableAmount
                    decMrdDisbursementNonTaxableLinkedAmount = paramPlanWiseNonTaxableAmount

                    paramPlanWiseDisbursementAmount = 0
                    paramTotalDisbursementTaxableAmount = 0
                    paramTotalDisbursementNonTaxableAmount = 0

                    paramDtMrdRecordsDisbursements.Rows.Add(drMrdRecordsDisbursement)
                    paramDtMrdRecordsDisbursements.AcceptChanges()
                    Exit For
                End If

                'This is not the last MRD record to link to. Identify if there is any unsatisfied MRD requirement for this record.
                'If so then link that amount and reduce the plan balance available for linking for the next MRD requirement.
                If decMrdAmountTobeSatisfied > 0 Then
                    If paramPlanWiseDisbursementAmount <= 0 Then Exit For

                    drMrdRecordsDisbursement("guiDisbursementId") = paramStrDisbursementID
                    drMrdRecordsDisbursement("intMrdTrackingId") = drMrdRecord("intMrdRecordsId")

                    If decMrdAmountTobeSatisfied >= paramPlanWiseDisbursementAmount Then
                        paramTotalDisbursementAmount -= paramPlanWiseDisbursementAmount
                        paramTotalDisbursementTaxableAmount -= paramPlanWiseTaxableAmount
                        paramTotalDisbursementNonTaxableAmount -= paramPlanWiseNonTaxableAmount

                        decMrdDisbursementLinkedAmount = paramPlanWiseDisbursementAmount
                        decMrdDisbursementTaxableLinkedAmount = paramPlanWiseTaxableAmount
                        decMrdDisbursementNonTaxableLinkedAmount = paramPlanWiseNonTaxableAmount

                        paramPlanWiseDisbursementAmount = 0
                        paramPlanWiseTaxableAmount = 0
                        paramPlanWiseNonTaxableAmount = 0
                    Else
                        paramTotalDisbursementAmount -= decMrdAmountTobeSatisfied
                        paramTotalDisbursementTaxableAmount -= decMrdAmountTobeSatisfiedTaxable
                        paramTotalDisbursementNonTaxableAmount -= decMrdAmountTobeSatisfiedNonTaxable

                        decMrdDisbursementLinkedAmount = decMrdAmountTobeSatisfied
                        decMrdDisbursementTaxableLinkedAmount = decMrdAmountTobeSatisfiedTaxable
                        decMrdDisbursementNonTaxableLinkedAmount = decMrdAmountTobeSatisfiedNonTaxable

                        paramPlanWiseDisbursementAmount -= decMrdAmountTobeSatisfied
                        paramPlanWiseTaxableAmount -= decMrdAmountTobeSatisfiedTaxable
                        paramPlanWiseNonTaxableAmount -= decMrdAmountTobeSatisfiedNonTaxable


                    End If

                    If decMrdDisbursementLinkedAmount > 0 Then
                        drMrdRecordsDisbursement("mnyPaidAmount") = decMrdDisbursementLinkedAmount
                        drMrdRecordsDisbursement("mnyTaxablePaidAmount") = decMrdDisbursementTaxableLinkedAmount
                        drMrdRecordsDisbursement("mnyNonTaxablePaidAmount") = decMrdDisbursementNonTaxableLinkedAmount
                        paramDtMrdRecordsDisbursements.Rows.Add(drMrdRecordsDisbursement)
                    End If
                End If

            Next
            paramDtMrdRecordsDisbursements.AcceptChanges()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function CreateDataForMarketWithdrawal() As DataTable
        Try
            Dim l_currentDatable As DataTable
            Dim l_requestedDatable As DataTable
            Dim l_marketDatable As DataTable
            Dim l_datarow As DataRow()
            Dim l_availableDatarow As DataRow
            Dim l_TempString As String

            l_currentDatable = DirectCast(Session("ArrayRefundableDataTable_C19"), DataTable).Copy()
            l_requestedDatable = DirectCast(Session("AvailableBalance_C19"), DataTable)

            l_marketDatable = l_currentDatable.Clone()


            For Each l_availableDatarow In l_currentDatable.Rows
                Dim l_length As Integer = 0
                Dim l_stringAccttype As String = String.Empty
                l_length = l_availableDatarow("AnnuityBasisType").ToString().Trim().Length
                l_stringAccttype = (l_availableDatarow("AnnuityBasisType").ToString().Trim().Substring(l_length - 4, 2))
                If getBitMarketValue(l_stringAccttype, l_requestedDatable) = True Then
                    l_marketDatable.ImportRow(l_availableDatarow)
                End If
            Next

            For Each l_availableDatarow In l_requestedDatable.Rows

                If (l_availableDatarow("TransactionRefID").GetType.ToString = "System.DBNull") Then
                    'tempstring = "AcctType = '" + l_availableDatarow("AcctType") + "' and AnnuityBasisType = '" + l_availableDatarow("AnnuityBasisType") + "' and chrTransactType = '" + l_availableDatarow("MoneyType").Trim() + "'"
                    l_TempString = "AnnuityBasisType='" + l_availableDatarow("AnnuityBasisType").ToString().Trim() + l_availableDatarow("AcctType").ToString().Trim() + l_availableDatarow("MoneyType").ToString().Trim() + "'"
                    l_datarow = l_marketDatable.Select(l_TempString)
                    If l_datarow.Length > 0 Then
                        l_datarow(0)("Taxable") = l_datarow(0)("Taxable") - l_availableDatarow("PersonalPreTax")
                        l_datarow(0)("Non-Taxable") = l_datarow(0)("Non-Taxable") - l_availableDatarow("PersonalPostTax")
                        l_datarow(0)("YMCATaxable") = l_datarow(0)("YMCATaxable") - l_availableDatarow("YmcaPreTax")
                        'l_marketDatable.ImportRow(l_datarow(0))
                    End If

                Else
                    'tempstring = "AcctType = '" + l_availableDatarow("AcctType") + "' and AnnuityBasisType = '" + l_availableDatarow("AnnuityBasisType") + "' and chrTransactType = 'RF" + l_availableDatarow("MoneyType").Trim() + "' and  TransactionRefId='" + l_availableDatarow("TransactionRefId") + "'"
                    l_TempString = "AnnuityBasisType='" + l_availableDatarow("AnnuityBasisType").ToString().Trim() + l_availableDatarow("AcctType").ToString().Trim() + l_availableDatarow("MoneyType").ToString().Trim() + "' and TransactionRefId='" + l_availableDatarow("TransactionRefId").ToString() + "'"

                    l_datarow = l_marketDatable.Select(l_TempString)
                    If l_datarow.Length > 0 Then
                        l_datarow(0)("Taxable") = l_datarow(0)("Taxable") - l_availableDatarow("PersonalPreTax")
                        l_datarow(0)("Non-Taxable") = l_datarow(0)("Non-Taxable") - l_availableDatarow("PersonalPostTax")
                        l_datarow(0)("YMCATaxable") = l_datarow(0)("YMCATaxable") - l_availableDatarow("YmcaPreTax")
                        '' l_marketDatable.ImportRow(l_datarow(0))
                    End If

                End If


            Next
            l_marketDatable.AcceptChanges()
            Session("MarketDataTable_C19") = l_marketDatable
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetAddressID() As String

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = Session("PersonInformation_C19")

            If Not l_DataSet Is Nothing Then

                ' To Find Address ID
                l_DataTable = l_DataSet.Tables("Member Address")

                If Not l_DataTable Is Nothing Then

                    For Each l_DataRow In l_DataTable.Rows
                        If l_DataRow("AddressID").GetType.ToString <> "System.DBNull" Then
                            If DirectCast(l_DataRow("AddressID"), String) <> "" Then
                                Return (DirectCast(l_DataRow("AddressID"), String))
                            Else
                                Return ""
                            End If
                        Else
                            Return ""
                        End If
                    Next

                End If

                Return ""

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    '***********************************************************************************************
    '* Create an Array of the Available Annuity Basis Types
    '***********************************************************************************************
    Private Function CreateAnnuityBasisTypes()


        Dim l_AnnuityBasisTypeDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_Counter As Integer

        Dim l_AnnuityBasisType() As String

        Try
            l_AnnuityBasisTypeDataTable = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.LookUpAnnuityBasisTypes()

            If Not l_AnnuityBasisTypeDataTable Is Nothing Then

                l_Counter = 0

                ReDim l_AnnuityBasisType(l_AnnuityBasisTypeDataTable.Rows.Count)

                For Each l_DataRow In l_AnnuityBasisTypeDataTable.Rows
                    If Not (l_DataRow("AnnuityBasisType").GetType.ToString.Trim = "System.DBNull") Then
                        l_AnnuityBasisType(l_Counter) = DirectCast(l_DataRow("AnnuityBasisType"), String)
                    End If

                    l_Counter += 1
                Next
            End If

            Session("AnnuityBasisTypeArray_C19") = l_AnnuityBasisType

        Catch ex As Exception
            Throw
        End Try

    End Function
    '' This Function is uesed Get the Schema for the further Transaction.
    Private Function OpenFiles() As Boolean

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable

        Try

            l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundTransactionSchemas

            If l_DataSet Is Nothing Then Return False
            Session("RefundTransaction_C19") = l_DataSet

            '' Add seperate files for transaction ...

            '' There are 4 tables needed for Making transaction . 
            '' Transaction, Refunds, Disbursement and DisbursementDetails.
            ''{"Transactions", "RolloverInstitutions", "Disbursements", "DisbursementDetails", "DisbursementWithholding", "DisbursementRefunds"};

            l_DataTable = l_DataSet.Tables("Transactions")
            Session("R_Transactions_C19") = l_DataTable


            l_DataTable = l_DataSet.Tables("Disbursements")
            Session("R_Disbursements_C19") = l_DataTable


            l_DataTable = l_DataSet.Tables("DisbursementDetails")
            Session("R_DisbursementDetails_C19") = l_DataTable


            l_DataTable = l_DataSet.Tables("DisbursementWithholding")
            Session("R_DisbursementWithholding_C19") = l_DataTable


            l_DataTable = l_DataSet.Tables("DisbursementRefunds")
            Session("R_DisbursementRefunds_C19") = l_DataTable

            'neeraj 16-Nov2010 : schema of table [AtsMrdRecordsDisbursements]
            l_DataTable = l_DataSet.Tables("MrdRecordsDisbursements")
            Session("R_MrdRecordsDisbursements_C19") = l_DataTable

            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function

    '***********************************************************************************************
    '* Determine what Currency Code to Use
    '***********************************************************************************************
    Private Function GetCurrencyCode() As String
        Try
            Return YMCARET.YmcaBusinessObject.RefundRequest.GetPersonBankingBeforeEffectiveDate(Me.PersonID)
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function HardShipCalculation()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_CurrentAccountDataTable As DataTable

        Dim l_Decimal_PersonalTotal As Decimal
        Dim l_Decimal_YMCATotal As Decimal
        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_Tax As Decimal

        Dim l_Payee1DataTable As DataTable

        Dim l_FoundRows() As DataRow
        Dim l_QueryString As String
        Dim l_Payee1DataRow As DataRow
        Dim l_index As Integer
        Dim l_CounterAmount As Decimal

        Try

            'Me.IsHardShip = False

            If Me.HardshipAmount <= 0.0 Then Return 0

            If Me.TDUsedAmount <= 0.0 Then Return 0

            'Me.IsHardShip = True

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_CurrentAccountDataTable = DirectCast(l_CacheManager.GetData("CalculatedDataTableForCurrentAccounts"), DataTable)
            'l_Payee1DataTable = DirectCast(l_CacheManager.GetData("Payee1DataTable"), DataTable)

            l_CurrentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            l_CounterAmount = 0.0

            If Not l_CurrentAccountDataTable Is Nothing Then


                For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows

                    If Not l_DataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
                        l_Decimal_PersonalTotal = DirectCast(l_DataRow("Emp.Total"), Decimal)
                    Else
                        l_Decimal_PersonalTotal = 0.0
                    End If


                    If Not l_DataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
                        l_Decimal_YMCATotal = DirectCast(l_DataRow("YMCATotal"), Decimal)
                    Else
                        l_Decimal_YMCATotal = 0.0
                    End If

                    If Not l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                        l_Decimal_PersonalPreTax = DirectCast(l_DataRow("Taxable"), Decimal)
                    Else
                        l_Decimal_PersonalPreTax = 0.0
                    End If



                    If l_Decimal_PersonalTotal + l_Decimal_YMCATotal > 0.0 Then

                        '' If the Total is > o then Do update in the Payee1 DataTable 
                        If Not l_DataRow("AccountGroup").GetType.ToString = "System.DBNull" Then
                            If l_DataRow("AccountGroup") = m_const_SavingsPlan_TD Or l_DataRow("AccountGroup") = m_const_SavingsPlan_TM Then


                                l_QueryString = "AccountGroup = '" & l_DataRow("AccountGroup") & "'"

                                If Not l_Payee1DataTable Is Nothing Then

                                    l_FoundRows = l_Payee1DataTable.Select(l_QueryString)



                                    If l_FoundRows Is Nothing Or l_FoundRows.Length = 0 Then

                                        l_Payee1DataRow = l_Payee1DataTable.NewRow

                                        l_Payee1DataRow("Taxable") = "0.00"
                                        l_Payee1DataRow("Tax") = "0.00"
                                        If l_Decimal_PersonalPreTax < (Me.HardshipAmount - l_CounterAmount) Then
                                            l_Payee1DataRow("Taxable") = DirectCast(l_Payee1DataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                            l_CounterAmount += l_Decimal_PersonalPreTax
                                        Else
                                            l_Payee1DataRow("Taxable") = DirectCast(l_Payee1DataRow("Taxable"), Decimal) + (Me.HardshipAmount - l_CounterAmount)
                                            l_CounterAmount += (Me.HardshipAmount - l_CounterAmount)
                                        End If

                                        l_Payee1DataRow("Tax") = DirectCast(l_Payee1DataRow("Tax"), Decimal) + (DirectCast(l_Payee1DataRow("Taxable"), Decimal) * (Me.HardShipTaxRate / 100.0))


                                        l_Payee1DataRow("RefRequestsID") = Me.SessionRefundRequestID
                                        l_Payee1DataRow("AcctType") = l_DataRow("AccountType")
                                        l_Payee1DataRow("Payee") = Me.Payee1Name
                                        l_Payee1DataRow("RequestType") = "HARD"
                                        l_Payee1DataRow("TaxRate") = Me.HardShipTaxRate
                                        l_Payee1DataRow("NonTaxable") = "0.00"

                                        l_Payee1DataTable.Rows.Add(l_Payee1DataRow)

                                    Else
                                        If l_FoundRows.Length > 0 Then
                                            l_Payee1DataRow = l_FoundRows(0)

                                            Dim l_row As DataRow

                                            For Each l_row In l_Payee1DataTable.Rows
                                                If l_DataRow("AccountType") = l_row("AcctType") Then
                                                    Exit For
                                                End If
                                                l_index += 1
                                            Next
                                        End If
                                        If l_Decimal_PersonalPreTax < (Me.HardshipAmount - l_CounterAmount) Then
                                            l_Payee1DataTable.Rows(l_index).Item("Taxable") = DirectCast(l_Payee1DataTable.Rows(l_index).Item("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                            l_CounterAmount += l_Decimal_PersonalPreTax
                                        Else
                                            l_Payee1DataTable.Rows(l_index).Item("Taxable") = DirectCast(l_Payee1DataTable.Rows(l_index).Item("Taxable"), Decimal) + (Me.HardshipAmount - l_CounterAmount)
                                            l_CounterAmount += (Me.HardshipAmount - l_CounterAmount)
                                        End If

                                        l_Payee1DataTable.Rows(l_index).Item("Tax") = DirectCast(l_Payee1DataTable.Rows(l_index).Item("Tax"), Decimal) + (DirectCast(l_Payee1DataRow("Taxable"), Decimal) * (Me.HardShipTaxRate / 100.0))


                                        l_Payee1DataTable.Rows(l_index).Item("RefRequestsID") = Me.SessionRefundRequestID
                                        l_Payee1DataTable.Rows(l_index).Item("AcctType") = l_DataRow("AccountType")
                                        l_Payee1DataTable.Rows(l_index).Item("Payee") = Me.Payee1Name
                                        l_Payee1DataTable.Rows(l_index).Item("RequestType") = "HARD"
                                        l_Payee1DataTable.Rows(l_index).Item("TaxRate") = Me.HardShipTaxRate
                                        l_Payee1DataTable.Rows(l_index).Item("NonTaxable") = "0.00"

                                    End If

                                    l_Payee1DataTable.AcceptChanges()
                                    If l_CounterAmount = Me.HardshipAmount Then
                                        Exit For
                                    End If

                                End If

                            End If
                        End If
                    End If
                Next

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetTransactionDetails(ByVal parameterFundID As String, ByVal parameterForcetoPersonalWithdrawal As Boolean) As String

        Dim l_DataTable As DataTable
        Dim l_RequestedDataTable As DataTable
        Dim l_bool_AMMatched As Boolean = True
        Dim l_bool_TMMatched As Boolean = True
        Dim l_DataTablePartial As DataTable
        Dim l_DataTableCurrentBalance As DataTable
        Dim bool_Continue As Boolean
        Dim l_DataTableforMarket As DataTable

        Try
            If Me.RefundType = "PART" Or Me.RefundType = "MRD" Then
                l_DataTablePartial = YMCARET.YmcaBusinessObject.RefundRequest.GetPartialWithdrawalTransaction(Session("RefundRequestID_C19"))
            End If

            l_DataTableCurrentBalance = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactions(parameterFundID)

            l_DataTableforMarket = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarketForSaveProcess(parameterFundID, Session("RefundRequestID_C19"))
            Session("DataforIsMarketCheck_C19") = l_DataTableforMarket

            If Me.RefundType = "PART" Then
                bool_Continue = validatePartialAmount(l_DataTableCurrentBalance, l_DataTablePartial)
                If bool_Continue = False Then
                    GetTransactionDetails = "Error: Not enough amount to cover the balance with requested accounts"
                    Exit Function
                End If
            End If


            'Added an if condition to check the transaction details from the atsrefrequesttransacts table-18 august amit
            '09-Dec-2009        Parveen     Changes for the Hardship and VOL withdrawal Issue
            If Me.RefundType = "HARD" Or CType(Session("ForceToVoluntaryWithdrawal_C19"), Boolean) = True Then
                'START | 2019.07.30 |  SR | YRS-AT-4498 | Add interests & Non-Taxable to Available Hardship TD amount
                l_DataTable = ProrateTransactionDataForHardship(l_DataTableCurrentBalance)
                'l_DataTable = l_DataTableCurrentBalance
                'START | 2019.07.30 |  SR | YRS-AT-4498 | Add interests & Non-Taxable to Available Hardship TD amount
            Else
                l_DataTable = l_DataTableforMarket
            End If
            Session("ForceToVoluntaryWithdrawal_C19") = Nothing
            'Commented by Parveen on 08-Dec-2009 To DataTableFor market fo all the withdrawals
            'If Me.RefundType = "PART" Then
            '    l_DataTable = l_DataTablePartial
            'ElseIf Me.ISMarket = -1 Then
            '    l_DataTable = l_DataTableforMarket 'l_DataTableCurrentBalance
            'Else
            '    l_DataTable = l_DataTableCurrentBalance
            'End If
            'Commented by Parveen on 08-Dec-2009 To DataTableFor market fo all the withdrawals
            'Added an if condition to check the transaction details from the atsrefrequesttransacts table-18 august amit

            Session("AvailableBalance_C19") = l_DataTable

            l_RequestedDataTable = DirectCast(Session("RequestedAccounts_C19"), DataTable)

            If Not l_RequestedDataTable Is Nothing Then
                For Each dr As DataRow In l_RequestedDataTable.Rows
                    If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                        If dr("AccountType") = "AM" Then
                            If dr("YMCATotal") = 0 Then
                                l_bool_AMMatched = False
                            End If
                        End If
                        If dr("AccountType") = "TM" Then
                            If dr("YMCATotal") = 0 Then
                                l_bool_TMMatched = False
                            End If
                        End If
                    End If
                Next
            End If
            If l_bool_AMMatched = False Then
                For Each l_DataRow As DataRow In l_DataTable.Rows
                    If DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_AM Then
                        l_DataRow("YmcaPreTax") = "0.00"
                    End If
                Next
            End If
            If l_bool_TMMatched = False Then
                For Each l_DataRow As DataRow In l_DataTable.Rows
                    If DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_SavingsPlan_TM Then
                        l_DataRow("YmcaPreTax") = "0.00"
                    End If
                Next
            End If
            'If Me.IsPersonalOnly = True Then
            If parameterForcetoPersonalWithdrawal = True Then

                For Each l_DataRow As DataRow In l_DataTable.Rows
                    'Begin Code Merge by Dilip on 07-05-2009
                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new add condition for Ac Account Type

                    'If DirectCast(l_DataRow("AccountGroup"), String).Trim <> m_const_RetirementPlan_AM And DirectCast(l_DataRow("AccountGroup"), String).Trim <> m_const_RetirementPlan_SR And DirectCast(l_DataRow("AccountGroup"), String).Trim <> m_const_SavingsPlan_TM And DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_AC Then
                    '    l_DataRow("YmcaPreTax") = "0.00"
                    'End If

                    'This Line Added by Ganeswar for YMCASide of money was deducting at the time when the User will choose the pesionalmonies ,This was happening due to AC account code was added
                    If DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_RG Or _
                        DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_SS Or _
                        DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_SA Or _
                        DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_BA Then
                        l_DataRow("YmcaPreTax") = "0.00"
                    End If
                    'This Line Added by Ganeswar for YMCASide of money was deducting at the time when the User will choose the pesionalmonies ,This was happening due to AC account code was added

                    'End Code Merge by Dilip on 07-05-2009
                Next


                l_DataTable = DirectCast(Session("CalculatedDataTableForRefundable_C19"), DataTable)


                If Not l_DataTable Is Nothing Then
                    For Each l_DataRow As DataRow In l_DataTable.Rows
                        'This Line commented by Ganeswar for YMCASide of money was deducting at the time when the User will choose the pesionalmonies ,This was happening due to AC account code was added
                        'Begin Code Merge by Dilip on 07-05-2009
                        'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new add condition for Ac Account Type
                        'If DirectCast(l_DataRow("AccountGroup"), String).Trim <> m_const_RetirementPlan_AM And DirectCast(l_DataRow("AccountGroup"), String).Trim <> m_const_RetirementPlan_SR And DirectCast(l_DataRow("AccountGroup"), String).Trim <> m_const_SavingsPlan_TM And DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_AC Then
                        '    'End Code Merge by Dilip on 07-05-2009
                        '    l_DataRow("YMCATaxable") = "0.00"
                        '    l_DataRow("YMCAInterest") = "0.00"
                        '    l_DataRow("YMCATotal") = "0.00"
                        '    l_DataRow("Total") = l_DataRow("Emp.Total")
                        'End If

                        If DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_RG Or _
                                   DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_SS Or _
                                   DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_SA Or _
                                   DirectCast(l_DataRow("AccountGroup"), String).Trim = m_const_RetirementPlan_BA Then
                            'This Line commented by Ganeswar for YMCASide of money was deducting at the time when the User will choose the pesionalmonies ,This was happening due to AC account code was added
                            l_DataRow("YMCATaxable") = "0.00"
                            l_DataRow("YMCAInterest") = "0.00"
                            l_DataRow("YMCATotal") = "0.00"
                            l_DataRow("Total") = l_DataRow("Emp.Total")
                        End If
                    Next
                End If

            End If

            GetTransactionDetails = String.Empty

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Added by Dilip  on 24-11-2009 for Defered Rollover
    Public Function GetIsMarket(ByVal parameterFundID As String, ByVal parameterPlanType As String) As Boolean
        Dim l_DataTableforMarket As DataTable
        Dim l_QueryString As String
        Dim l_DataRows() As DataRow
        Try
            l_QueryString = "IsMarket =True and PlanType='" & parameterPlanType.Trim & "'"
            l_DataTableforMarket = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarketForSaveProcess(parameterFundID, Session("RefundRequestID_C19"))
            If Not l_DataTableforMarket Is Nothing Then
                l_DataRows = l_DataTableforMarket.Select(l_QueryString)
                If Not l_DataRows Is Nothing Then
                    If l_DataRows.Length > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetTransactionofDefferedPayment(ByVal parameterFundID As String) As Decimal
        Dim l_RemainingDeffered As Decimal
        Dim l_RemainingDefferedInstallment As Decimal
        Dim l_firstInstallment As Decimal
        Dim l_OtherWithdrawalAmount As Decimal
        Dim l_TotalMarketWithdrawalAmount As Decimal
        Dim l_MarketAccountType As String = String.Empty

        Dim l_firstInstallmentTaxable As Decimal
        Dim l_firstInstallmentNonTaxable As Decimal
        Dim l_OtherWithdrawalAmountTaxable As Decimal
        Dim l_OtherWithdrawalAmountNonTaxable As Decimal
        Dim l_RemainingDefferedInstallmentTaxable As Decimal
        Dim l_RemainingDefferedInstallmentNonTaxable As Decimal

        Dim l_DataTable As DataTable
        Dim l_RequestedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_CurrentDataRow As DataRow
        Dim l_defferedRows() As DataRow
        Dim l_DataTablePartial As DataTable
        Dim l_DataTableCurrentBalance As DataTable
        Dim bool_Continue As Boolean
        Dim l_DataTableforMarket As DataTable

        Dim l_RefundableDataTable As DataTable
        Try
            If Me.RefundType = "PART" Then
                l_DataTablePartial = YMCARET.YmcaBusinessObject.RefundRequest.GetPartialWithdrawalTransaction(Session("RefundRequestID_C19"))
            End If

            l_DataTableCurrentBalance = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactions(parameterFundID)

            l_DataTableforMarket = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarketForSaveProcess(parameterFundID, Session("RefundRequestID_C19"))

            'Added By Parveen on 7-Nov-2009 for Calculating Deferred payments
            l_RefundableDataTable = DirectCast(Session("RefundableDataTable_C19"), DataTable)
            'Added By Parveen on 7-Nov-2009 for Calculating Deferred payments

            If Me.RefundType = "PART" Then
                bool_Continue = validatePartialAmount(l_DataTableCurrentBalance, l_DataTablePartial)
                If bool_Continue = False Then
                    GetTransactionofDefferedPayment = 0.0
                    Exit Function
                End If
            End If


            'Added an if condition to check the transaction details from the atsrefrequesttransacts table-18 august amit
            If Me.RefundType = "PART" Or Me.RefundType = "MRD" Then
                l_DataTable = l_DataTablePartial
            ElseIf Me.ISMarket = -1 Then
                l_DataTable = l_DataTableforMarket 'l_DataTableCurrentBalance
            Else
                l_DataTable = l_DataTableCurrentBalance
            End If

            'Added an if condition to check the transaction details from the atsrefrequesttransacts table-18 august amit
            If Not l_DataTable Is Nothing Then
                l_defferedRows = l_DataTable.Select("IsMarket=True")
                If Not l_defferedRows Is Nothing Then
                    If l_defferedRows.Length > 0 Then
                        For Each l_DataRow In l_defferedRows
                            'l_firstInstallmentTaxable()
                            'l_firstInstallmentNonTaxable()
                            If l_MarketAccountType.IndexOf(l_DataRow("AcctType")) = -1 Then
                                l_MarketAccountType = l_MarketAccountType + "'" + l_DataRow("AcctType") + "',"
                            End If
                            l_firstInstallment = l_firstInstallment + DirectCast(l_DataRow("PersonalPreTax"), Decimal) + DirectCast(l_DataRow("PersonalPostTax"), Decimal) + DirectCast(l_DataRow("YmcaPreTax"), Decimal)
                        Next
                    End If

                End If
                'Added and Commented By Parveen on 7-Nov-2009 for Calculating Deferred payments
                l_defferedRows = l_RefundableDataTable.Select(" AccountType in(" & l_MarketAccountType.Substring(0, l_MarketAccountType.Length - 1) & ")")
                If Not l_defferedRows Is Nothing Then
                    If l_defferedRows.Length > 0 Then

                        For Each l_DataRow In l_defferedRows
                            l_TotalMarketWithdrawalAmount = l_TotalMarketWithdrawalAmount + DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("Non-Taxable"), Decimal) + DirectCast(l_DataRow("Interest"), Decimal) + DirectCast(l_DataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                        Next
                    End If
                End If
                'l_defferedRows = l_DataTable.Select("IsMarket=False")
                'If Not l_defferedRows Is Nothing Then
                '    If l_defferedRows.Length > 0 Then

                '        For Each l_DataRow In l_defferedRows
                '            l_OtherWithdrawalAmount = l_OtherWithdrawalAmount + DirectCast(l_DataRow("PersonalPreTax"), Decimal) + DirectCast(l_DataRow("PersonalPostTax"), Decimal) + DirectCast(l_DataRow("YmcaPreTax"), Decimal)
                '        Next
                '    End If
                'End If
                l_RemainingDeffered = l_TotalMarketWithdrawalAmount - l_firstInstallment 'DirectCast(Session("NumTotalWithdrawalAmount"), Decimal) - (l_firstInstallment + l_OtherWithdrawalAmount)
                'Added and Commented By Parveen on 7-Nov-2009 for Calculating Deferred payments
                Me.DefferredPaymentAmount = l_RemainingDeffered
                l_RemainingDefferedInstallment = Math.Round(l_RemainingDeffered * Me.DefferredInstallment / (100 - Me.FirstInstallment), 2)
                Me.DefferredInstallmentAmount = l_RemainingDefferedInstallment
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CreateDeferedTaxablePercentage() As Decimal
        Dim l_DataTable As DataTable
        Dim l_DataTableMarket As DataTable
        Dim l_DataRow As DataRow
        Dim l_decimalDeferedTaxable As Decimal = 0.0
        Dim l_decimalDeferedNonTaxable As Decimal = 0.0
        Dim l_decimalDeferedTaxablePercentage As Decimal = 0.0
        l_DataTableMarket = DirectCast(Session("DataforIsMarketCheck_C19"), DataTable)
        l_DataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)
        For Each l_DataRow In l_DataTable.Rows
            If (getBitMarketValue(l_DataRow("AcctType"), l_DataTableMarket)) Then
                l_decimalDeferedTaxable = l_decimalDeferedTaxable + DirectCast(l_DataRow("Taxable"), Decimal)
                l_decimalDeferedNonTaxable = l_decimalDeferedNonTaxable + DirectCast(l_DataRow("NonTaxable"), Decimal)
            End If
        Next
        If ((l_decimalDeferedTaxable + l_decimalDeferedNonTaxable) > 0) Then
            l_decimalDeferedTaxablePercentage = l_decimalDeferedTaxable * 100 / (l_decimalDeferedTaxable + l_decimalDeferedNonTaxable)

        End If
        Return l_decimalDeferedTaxablePercentage
    End Function
    Private Function getBitMarketValue(ByVal parameterAccountType As String, ByVal parameterMarketDataTable As DataTable) As Boolean
        Try
            Dim l_dt_MarketDataTable As New DataTable
            Dim l_dr_DataRow As DataRow
            Dim FoundDataRows() As DataRow
            l_dt_MarketDataTable = parameterMarketDataTable
            If Not l_dt_MarketDataTable Is Nothing Then
                For Each l_dr_DataRow In l_dt_MarketDataTable.Rows
                    FoundDataRows = l_dt_MarketDataTable.Select("Accttype = '" & parameterAccountType.ToString() & "' and IsMarket = '" & True & "' ")
                    If FoundDataRows.Length > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    'Added by Dilip  on 24-11-2009 for Defered Rollover

    Private Function validatePartialAmount(ByVal ParameterCurrentBalanceDatatable As DataTable, ByVal partialDataTable As DataTable) As Boolean
        Try
            Dim bool_StopWithdrawal As Boolean
            Dim drowTransactRefIDnull() As DataRow
            Dim drowTransactRefIDNotnull() As DataRow
            Dim FoundDataRows() As DataRow
            Dim FoundDataRow As DataRow
            Dim l_TransactRefID As String = "System.DbNull"
            Dim dec_OriginalAmount As Decimal
            Dim dec_PartialAmount As Decimal
            Dim l_continue As Boolean

            drowTransactRefIDnull = partialDataTable.Select("TransactionRefID  is null")
            drowTransactRefIDNotnull = partialDataTable.Select("TransactionRefID is not null")

            l_continue = True
            If drowTransactRefIDnull.Length > 0 Then
                For Each l_dataRow As DataRow In drowTransactRefIDnull
                    FoundDataRows = ParameterCurrentBalanceDatatable.Select("Accttype = '" & l_dataRow("Accttype").ToString() & "' and AnnuityBasisType = '" & l_dataRow("AnnuityBasisType").ToString() & "' and MoneyType = '" & l_dataRow("MoneyType").ToString() & "'")
                    If FoundDataRows.Length > 0 Then
                        FoundDataRow = FoundDataRows(0)
                        If Convert.ToDecimal(FoundDataRow("PersonalPreTax")) >= Convert.ToDecimal(l_dataRow("PersonalPreTax")) Or _
                           Convert.ToDecimal(FoundDataRow("PersonalPostTax")) >= Convert.ToDecimal(l_dataRow("PersonalPostTax")) Or _
                           Convert.ToDecimal(FoundDataRow("YMCAPreTax")) >= Convert.ToDecimal(l_dataRow("YMCAPreTax")) Then
                            l_continue = True
                        Else
                            l_continue = True
                            Exit For
                        End If
                    Else
                        l_continue = False
                        Exit For
                    End If


                Next
            End If
            If l_continue = True Then
                If drowTransactRefIDNotnull.Length > 0 Then
                    For Each l_dataRow As DataRow In drowTransactRefIDNotnull
                        FoundDataRows = ParameterCurrentBalanceDatatable.Select("Accttype = '" & l_dataRow("Accttype").ToString() & "' and AnnuityBasisType = '" & l_dataRow("AnnuityBasisType").ToString() & "' and MoneyType = '" & l_dataRow("MoneyType").ToString() & "' and TransactionRefID = '" & l_dataRow("TransactionRefID").ToString() & "'")
                        If FoundDataRows.Length > 0 Then
                            FoundDataRow = FoundDataRows(0)
                            If Convert.ToDecimal(FoundDataRow("PersonalPreTax")) >= Convert.ToDecimal(l_dataRow("PersonalPreTax")) Or _
                               Convert.ToDecimal(FoundDataRow("PersonalPostTax")) >= Convert.ToDecimal(l_dataRow("PersonalPostTax")) Or _
                               Convert.ToDecimal(FoundDataRow("YMCAPreTax")) >= Convert.ToDecimal(l_dataRow("YMCAPreTax")) Then
                                l_continue = True
                            Else
                                l_continue = True
                                Exit For
                            End If
                        Else
                            l_continue = False
                            Exit For
                        End If


                    Next
                End If

            End If


            validatePartialAmount = l_continue
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function GetTransactionType(ByVal parameterFundId As String) As DataTable
        Dim l_DataTable As DataTable
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionType(parameterFundId)
            Return l_DataTable
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ReduceAvailableBalance()
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_RefundableDataTable As DataTable
        Dim l_ArrayRefundDataTable As DataTable
        Dim l_AvailableBalanceDataTable As DataTable

        Dim l_RefundDataRow As DataRow
        Dim l_AvailableBalanceDataRow As DataRow
        Dim l_ArrayRefundDataRow As DataRow

        Dim l_AnnuityBasisArray() As String

        Dim l_Counter As Integer
        Dim l_AnnuityBasisType As String
        Dim l_TempString As String

        Dim l_Decimal_PersonalInterest As Decimal
        Dim l_Decimal_YMCAInterest As Decimal

        Dim l_FoundRows() As DataRow

        Dim l_BooleanFlag As Boolean = False
        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalPostTax As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal
        Dim l_RowDeleteFlag As Boolean = False

        Try

            l_AnnuityBasisArray = DirectCast(Session("AnnuityBasisTypeArray_C19"), String())

            l_RefundableDataTable = DirectCast(Session("RefundableDataTable_C19"), DataTable) 'DirectCast(Session("RequestedAccounts_C19"), DataTable)   'DirectCast(Session("RefundableDataTable_C19"), DataTable) '

            l_ArrayRefundDataTable = l_RefundableDataTable.Clone



            '**************************************************************************************
            '' Do the calculation for the Refundable DataTable C_Refundable.
            '**************************************************************************************

            If (Not l_AnnuityBasisArray Is Nothing) And (Not l_RefundableDataTable Is Nothing) Then

                For l_Counter = 0 To l_AnnuityBasisArray.Length - 1
                    l_AnnuityBasisType = l_AnnuityBasisArray(l_Counter)

                    If Not l_AnnuityBasisType Is Nothing Then



                        For Each l_RefundDataRow In l_RefundableDataTable.Rows

                            If Not l_RefundDataRow("AnnuityBasisType").GetType.ToString = "System.DBNull" Then

                                If l_AnnuityBasisType.Trim = DirectCast(l_RefundDataRow("AnnuityBasisType"), String).Trim Then

                                    If Not (l_RefundDataRow("Interest").GetType.ToString().Trim = "System.DBNull") Then
                                        l_Decimal_PersonalInterest = DirectCast(l_RefundDataRow("Interest"), Decimal)
                                    Else
                                        l_Decimal_PersonalInterest = 0.0
                                    End If


                                    If Not (l_RefundDataRow("YMCAInterest").GetType.ToString().Trim = "System.DBNull") Then
                                        l_Decimal_YMCAInterest = DirectCast(l_RefundDataRow("YMCAInterest"), Decimal)
                                    Else
                                        l_Decimal_YMCAInterest = 0.0
                                    End If


                                    '' Start calculation 

                                    If l_Decimal_PersonalInterest + l_Decimal_YMCAInterest > 0.0 Then
                                        'Commented the code as per the bug reported by client for deletion of TD account-Amit Aug 31,2009
                                        'l_TempString = DirectCast(l_RefundDataRow("AnnuityBasisType"), String) & DirectCast(l_RefundDataRow("AccountType"), String) & "IN"
                                        l_TempString = l_RefundDataRow("AnnuityBasisType").ToString().Trim() + l_RefundDataRow("AccountType").ToString().Trim() + "IN"
                                        'Commented the code as per the bug reported by client for deletion of TD account-Amit Aug 31,2009 

                                        l_FoundRows = l_ArrayRefundDataTable.Select("AnnuityBasisType = '" & l_TempString & "'")

                                        If Not l_FoundRows Is Nothing Then

                                            If l_FoundRows.Length < 1 Then

                                                l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow

                                                l_ArrayRefundDataRow("AnnuityBasisType") = l_TempString

                                                l_ArrayRefundDataRow("Taxable") = "0.00"
                                                l_ArrayRefundDataRow("Non-Taxable") = "0.00"
                                                l_ArrayRefundDataRow("YMCATaxable") = "0.00"
                                                l_ArrayRefundDataRow("PlanType") = l_RefundDataRow("PlanType")
                                                l_ArrayRefundDataRow("TransactionRefId") = l_RefundDataRow("TransactionRefId")

                                                l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow)
                                            Else
                                                l_ArrayRefundDataRow = l_FoundRows(0)
                                            End If

                                        Else
                                            '' create a NewRow.
                                            l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow

                                            l_ArrayRefundDataRow("AnnuityBasisType") = l_TempString

                                            l_ArrayRefundDataRow("Taxable") = "0.00"
                                            l_ArrayRefundDataRow("Non-Taxable") = "0.00"
                                            l_ArrayRefundDataRow("YMCATaxable") = "0.00"
                                            l_ArrayRefundDataRow("PlanType") = l_RefundDataRow("PlanType")
                                            l_ArrayRefundDataRow("TransactionRefId") = l_RefundDataRow("TransactionRefId")

                                            l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow)

                                        End If

                                        l_ArrayRefundDataRow("Taxable") = DirectCast(l_ArrayRefundDataRow("Taxable"), Decimal) + IIf(l_RefundDataRow("Interest").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_RefundDataRow("Interest"), Decimal))                         'arRefundable[lnx,2] = 0.00
                                        l_ArrayRefundDataRow("Non-Taxable") = "0.00"                                                                                                                                                                                    'arRefundable[lnx,3] = 0.00
                                        l_ArrayRefundDataRow("YMCATaxable") = DirectCast(l_ArrayRefundDataRow("YMCATaxable"), Decimal) + IIf(l_RefundDataRow("YMCAInterest").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_RefundDataRow("YMCAInterest"), Decimal))        'arRefundable[lnx,4] = 0.00

                                    End If

                                    '' *****
                                    'Commented the code as per the bug reported by client for deletion of TD account-Amit Aug 31,2009
                                    'l_TempString = DirectCast(l_RefundDataRow("AnnuityBasisType"), String) & DirectCast(l_RefundDataRow("AccountType"), String) & "PR"
                                    l_TempString = l_RefundDataRow("AnnuityBasisType").ToString().Trim() + l_RefundDataRow("AccountType").ToString().Trim() + "PR"
                                    'Commented the code as per the bug reported by client for deletion of TD account-Amit Aug 31,2009
                                    l_FoundRows = l_ArrayRefundDataTable.Select("AnnuityBasisType = '" & l_TempString & "'")

                                    If Not l_FoundRows Is Nothing Then

                                        If l_FoundRows.Length < 1 Then

                                            l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow

                                            l_ArrayRefundDataRow("AnnuityBasisType") = l_TempString

                                            l_ArrayRefundDataRow("Taxable") = "0.00"
                                            l_ArrayRefundDataRow("Non-Taxable") = "0.00"
                                            l_ArrayRefundDataRow("YMCATaxable") = "0.00"
                                            l_ArrayRefundDataRow("PlanType") = l_RefundDataRow("PlanType")
                                            l_ArrayRefundDataRow("TransactionRefId") = l_RefundDataRow("TransactionRefId")
                                            l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow)
                                        Else
                                            l_ArrayRefundDataRow = l_FoundRows(0)
                                        End If

                                    Else
                                        '' create a NewRow.
                                        l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow

                                        l_ArrayRefundDataRow("AnnuityBasisType") = l_TempString

                                        l_ArrayRefundDataRow("Taxable") = "0.00"
                                        l_ArrayRefundDataRow("Non-Taxable") = "0.00"
                                        l_ArrayRefundDataRow("YMCATaxable") = "0.00"
                                        l_ArrayRefundDataRow("PlanType") = l_RefundDataRow("PlanType")
                                        l_ArrayRefundDataRow("TransactionRefId") = l_RefundDataRow("TransactionRefId")

                                        l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow)

                                    End If

                                    l_ArrayRefundDataRow("Taxable") = DirectCast(l_ArrayRefundDataRow("Taxable"), Decimal) + IIf(l_RefundDataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_RefundDataRow("Taxable"), Decimal))                         'arRefundable[lnx,2] = 0.00
                                    l_ArrayRefundDataRow("Non-Taxable") = DirectCast(l_ArrayRefundDataRow("Non-Taxable"), Decimal) + IIf(l_RefundDataRow("Non-Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_RefundDataRow("Non-Taxable"), Decimal))         'arRefundable[lnx,3] = 0.00
                                    l_ArrayRefundDataRow("YMCATaxable") = DirectCast(l_ArrayRefundDataRow("YMCATaxable"), Decimal) + IIf(l_RefundDataRow("YMCATaxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_RefundDataRow("YMCATaxable"), Decimal))        'arRefundable[lnx,4] = 0.00

                                End If

                            End If


                        Next
                    End If '' Check for Not Null.
                Next
            End If



            Session("ArrayRefundableDataTable_C19") = l_ArrayRefundDataTable
            If Me.ISMarket = -1 Then
                Me.CreateDataForMarketWithdrawal()
            End If

            '**********************************************************************************
            'Do Calculation for Available Balance. 
            '**********************************************************************************


            l_AvailableBalanceDataTable = DirectCast(Session("AvailableBalance_C19"), DataTable)


            If (Not l_AnnuityBasisArray Is Nothing) And (Not l_AvailableBalanceDataTable Is Nothing) Then

                For l_Counter = 0 To l_AnnuityBasisArray.Length - 1
                    l_AnnuityBasisType = l_AnnuityBasisArray(l_Counter)

                    If Not l_AnnuityBasisType Is Nothing Then



                        For Each l_AvailableBalanceDataRow In l_AvailableBalanceDataTable.Rows

                            If l_AvailableBalanceDataRow.RowState <> DataRowState.Deleted Then '' Check for Deleted Row.


                                If Not l_AvailableBalanceDataRow("AnnuityBasisType").GetType.ToString = "System.DBNull" Then

                                    If l_AnnuityBasisType.Trim = DirectCast(l_AvailableBalanceDataRow("AnnuityBasisType"), String).Trim Then


                                        '' Start calculation 
                                        'Commented the code as per the bug reported by client for deletion of TD account-Amit Aug 31,2009
                                        'l_TempString = l_AvailableBalanceDataRow("AnnuityBasisType") & l_AvailableBalanceDataRow("AcctType") & l_AvailableBalanceDataRow("MoneyType")
                                        l_TempString = l_AvailableBalanceDataRow("AnnuityBasisType").ToString().Trim() + l_AvailableBalanceDataRow("AcctType").ToString().Trim() + l_AvailableBalanceDataRow("MoneyType").ToString().Trim()
                                        'Commented the code as per the bug reported by client for deletion of TD account-Amit Aug 31,2009

                                        l_FoundRows = l_ArrayRefundDataTable.Select("AnnuityBasisType = '" & l_TempString & "'")

                                        l_BooleanFlag = False
                                        If Not l_FoundRows Is Nothing Then

                                            If l_FoundRows.Length < 1 Then
                                                l_BooleanFlag = False
                                            Else
                                                l_BooleanFlag = True
                                            End If
                                        Else
                                            l_BooleanFlag = False
                                        End If


                                        If l_BooleanFlag = False Then

                                            '' Check for the HardShip 

                                            If l_AvailableBalanceDataRow.RowState <> DataRowState.Deleted Then

                                                If Me.IsHardShip And (DirectCast(l_AvailableBalanceDataRow("MoneyType"), String) = "PR") Then

                                                    If (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String) = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String) = m_const_SavingsPlan_TM) Then
                                                        l_AvailableBalanceDataRow("YmcaPreTax") = "0.00"
                                                    End If
                                                Else
                                                    l_AvailableBalanceDataRow.Delete()
                                                    l_RowDeleteFlag = True
                                                End If
                                            End If

                                        Else
                                            l_ArrayRefundDataRow = l_FoundRows(0)

                                            If l_AvailableBalanceDataRow.RowState <> DataRowState.Deleted Then

                                                l_Decimal_PersonalPreTax = IIf(l_AvailableBalanceDataRow("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal))
                                                'l_Decimal_PersonalPostTax = IIf(l_AvailableBalanceDataRow("PersonalPostTax").GetType.ToString = "System.DBNull", 0, DirectCast(l_AvailableBalanceDataRow("PersonalPostTax"), Decimal))
                                                l_Decimal_PersonalPostTax = IIf(l_AvailableBalanceDataRow("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, (l_AvailableBalanceDataRow("PersonalPostTax")))
                                                l_Decimal_YMCAPreTax = IIf(l_AvailableBalanceDataRow("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalanceDataRow("YmcaPreTax"), Decimal))


                                                If l_Decimal_PersonalPreTax >= DirectCast(l_ArrayRefundDataRow("Taxable"), Decimal) Then
                                                    l_AvailableBalanceDataRow("PersonalPreTax") = l_ArrayRefundDataRow("Taxable")
                                                    l_ArrayRefundDataRow("Taxable") = "0.00"
                                                Else
                                                    l_ArrayRefundDataRow("Taxable") = DirectCast(l_ArrayRefundDataRow("Taxable"), Decimal) - l_Decimal_PersonalPreTax
                                                End If


                                                If l_Decimal_PersonalPostTax >= DirectCast(l_ArrayRefundDataRow("Non-Taxable"), Decimal) Then
                                                    l_AvailableBalanceDataRow("PersonalPostTax") = l_ArrayRefundDataRow("Non-Taxable")
                                                    l_ArrayRefundDataRow("Non-Taxable") = "0.00"
                                                Else
                                                    l_ArrayRefundDataRow("Non-Taxable") = DirectCast(l_ArrayRefundDataRow("Non-Taxable"), Decimal) - l_Decimal_PersonalPostTax
                                                End If


                                                If l_Decimal_YMCAPreTax >= DirectCast(l_ArrayRefundDataRow("YMCATaxable"), Decimal) Then
                                                    l_AvailableBalanceDataRow("YmcaPreTax") = l_ArrayRefundDataRow("YMCATaxable") '-- Ref Array Index
                                                    l_ArrayRefundDataRow("YMCATaxable") = "0.00"
                                                Else
                                                    l_ArrayRefundDataRow("YMCATaxable") = DirectCast(l_ArrayRefundDataRow("YMCATaxable"), Decimal) - l_Decimal_YMCAPreTax
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next

                Session("ArrayRefundableDataTable_C19") = l_ArrayRefundDataTable

                l_AvailableBalanceDataTable.AcceptChanges()

            End If

            'End of function.

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function GetDataRow(ByVal parameterDataTable As DataTable, ByVal parameterAnnuityBasisType As String) As DataRow()

        Dim l_QueryString As String
        Dim l_FoundRows() As DataRow

        Try

            If Not parameterDataTable Is Nothing Then

                l_QueryString = "AnnuityBasisType = '" & parameterAnnuityBasisType.Trim & "'"

                l_FoundRows = parameterDataTable.Select(l_QueryString)

                If l_FoundRows Is Nothing Then Return Nothing

                If l_FoundRows.Length = 0 Then Return Nothing

                Return l_FoundRows

            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function ClearZerosRows(ByVal parameterDataTable As DataTable) As DataTable

        Dim l_DataRow As DataRow
        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal

        Dim l_BooleanFlag As Boolean = False

        Try

            If parameterDataTable Is Nothing Then Return Nothing

            parameterDataTable.AcceptChanges()

            For Each l_DataRow In parameterDataTable.Rows
                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                    l_Decimal_Taxable = 0.0
                Else

                    l_Decimal_Taxable = DirectCast(l_DataRow("Taxable"), Decimal)
                End If
                If l_DataRow("NonTaxable").GetType.ToString = "System.DBNull" Then
                    l_Decimal_NonTaxable = 0.0
                Else
                    l_Decimal_NonTaxable = DirectCast(l_DataRow("NonTaxable"), Decimal)
                End If

                If (l_Decimal_Taxable + l_Decimal_NonTaxable) = 0.0 Then
                    l_DataRow.Delete()

                    l_BooleanFlag = True
                End If

            Next

            If l_BooleanFlag = True Then
                parameterDataTable.AcceptChanges()
            End If

            Return parameterDataTable

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function GetDataRowForSelectedTable(ByVal parameterDataTable As DataTable, ByVal parameterAccountType As String, Optional ByVal parameterColumnName As String = "") As DataRow

        Dim l_QueryString As String
        Dim l_FoundRows() As DataRow

        Try

            If Not parameterDataTable Is Nothing Then

                If parameterColumnName = "" Then
                    l_QueryString = "AcctType = '" & parameterAccountType.Trim & "'"
                Else
                    l_QueryString = parameterColumnName & " = '" & parameterAccountType.Trim & "'"
                End If


                l_FoundRows = parameterDataTable.Select(l_QueryString)

                If l_FoundRows Is Nothing Then Return Nothing

                If l_FoundRows.Length = 0 Then Return Nothing

                Return l_FoundRows(0)

            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function CreateDataForRefund(ByVal dtAvoilableDataTable As DataTable, ByVal dtTransactionsDataTable As DataTable)
        Dim drAvailableBalance As DataRow
        Dim drTransaction As DataRow

        dtTransactionsDataTable.Columns.Add("UniqueId", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("RefRequestId", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("AcctType", System.Type.GetType("System.String"))
        ' dtTransactionsDataTable.Columns.Add("Taxable", System.Type.GetType("System.Decimal"))
        'dtTransactionsDataTable.Columns.Add("NonTaxable", System.Type.GetType("System.Decimal"))
        dtTransactionsDataTable.Columns.Add("Tax", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("TaxRate", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("Payee", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("FundedDate", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("RequestType", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("TransactId", System.Type.GetType("System.String"))
        'dtTransactionsDataTable.Columns.Add("AnnuityBasisType", System.Type.GetType("System.String"))
        dtTransactionsDataTable.Columns.Add("DisburshmentId", System.Type.GetType("System.String"))

        For intcount As Integer = 0 To dtTransactionsDataTable.Rows.Count - 1
            drAvailableBalance = dtTransactionsDataTable.Rows(intcount)
            For intcounts As Integer = 0 To dtAvoilableDataTable.Rows.Count - 1
                drTransaction = dtAvoilableDataTable.Rows(intcounts)
                Dim strAccountType As String
                Dim FinalAccountType As String
                strAccountType = drAvailableBalance("AnnuityBasisType").ToString()
                strAccountType = strAccountType.Substring(6)
                FinalAccountType = strAccountType.Remove(2, 2)
                If drTransaction("AcctType") = FinalAccountType Then
                    drAvailableBalance.BeginEdit()
                    drAvailableBalance("UniqueId") = drTransaction("UniqueId")
                    drAvailableBalance("RefRequestId") = drTransaction("RefRequestId")
                    drAvailableBalance("AcctType") = drTransaction("AcctType")

                    drAvailableBalance("Taxable") = drAvailableBalance("Taxable")
                    drAvailableBalance("NonTaxable") = drAvailableBalance("NonTaxable")
                    drAvailableBalance("Tax") = drTransaction("Tax")

                    drAvailableBalance("TaxRate") = drTransaction("TaxRate")
                    drAvailableBalance("Payee") = drTransaction("Payee")

                    drAvailableBalance("FundedDate") = drTransaction("FundedDate")
                    drAvailableBalance("RequestType") = drTransaction("RequestType")
                    drAvailableBalance("TransactId") = drTransaction("TransactId")
                    drAvailableBalance("AnnuityBasisType") = drAvailableBalance("AnnuityBasisType")
                    drAvailableBalance("DisburshmentId") = drTransaction("DisburshmentId")
                    drAvailableBalance.EndEdit()
                End If
            Next
        Next
        Return dtTransactionsDataTable
    End Function

    'Modified for Addition of new account types , now the calculations will be based on Account group

    'START : ML  | 2020.05.18 | YRS-AT-4874 | Merge NonCovid and Covid data in Pay1Datatable.
    ''' <summary>
    ''' 1. Read Payee1Datatable  Datatable (NonCovid Data)
    ''' 2. Read CovidProrated DataTable
    ''' 3. Update Payee1Datatable with (NonCOVID  + COVID ) Datatable
    ''' 4. Calculate Blended Rate and update Datatable 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MergeNonCOVIDandCOVIDDatatable()
        Dim dtNonCovidProratedDatatable As DataTable
        Dim dtCovidProratedDatatable As DataTable

        Dim covidTaxable As Decimal
        Dim nonCovidTaxable As Decimal
        Dim TotalTaxable As Decimal
        Dim covidNonTaxable As Decimal
        Dim nonCovidNonTaxable As Decimal
        Dim TotalNonTaxable As Decimal
        Dim covidTax As Decimal
        Dim noncovidTax As Decimal
        Dim totalTax As Decimal
        Try
            dtNonCovidProratedDatatable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
            dtCovidProratedDatatable = DirectCast(Session("COVIDProrateAccountDataTable"), DataTable)

            If HelperFunctions.isNonEmpty(Session("COVIDProrateAccountDataTable")) Then
                dtCovidProratedDatatable = HelperFunctions.DeepCopy(Of DataTable)(DirectCast(Session("COVIDProrateAccountDataTable"), DataTable))

                If (HelperFunctions.isNonEmpty(dtNonCovidProratedDatatable)) Then

                    For Each drNonCovidProratedDatatable As DataRow In dtNonCovidProratedDatatable.Rows
                        For Each drCovidProratedDatatable As DataRow In dtCovidProratedDatatable.Rows

                            If drCovidProratedDatatable("AcctType").ToString().Trim() = drNonCovidProratedDatatable("AcctType").ToString.Trim() Then

                                covidTaxable = If(drCovidProratedDatatable("Taxable") Is DBNull.Value, 0, CType(drCovidProratedDatatable("Taxable"), Decimal))
                                nonCovidTaxable = If(drNonCovidProratedDatatable("Taxable") Is DBNull.Value, 0, CType(drNonCovidProratedDatatable("Taxable"), Decimal))
                                TotalTaxable = covidTaxable + nonCovidTaxable

                                covidNonTaxable = If(drCovidProratedDatatable("NonTaxable") Is DBNull.Value, 0, CType(drCovidProratedDatatable("NonTaxable"), Decimal))
                                nonCovidNonTaxable = If(drNonCovidProratedDatatable("NonTaxable") Is DBNull.Value, 0, CType(drNonCovidProratedDatatable("NonTaxable"), Decimal))
                                TotalNonTaxable = covidNonTaxable + nonCovidNonTaxable

                                covidTax = If(drCovidProratedDatatable("TaxRate") Is DBNull.Value, 0, Math.Round((covidTaxable) * (drCovidProratedDatatable("TaxRate") / 100.0), 2))
                                noncovidTax = If(drNonCovidProratedDatatable("TaxRate") Is DBNull.Value, 0, Math.Round((nonCovidTaxable) * (drNonCovidProratedDatatable("TaxRate") / 100.0), 2))

                                totalTax = covidTax + noncovidTax

                                drNonCovidProratedDatatable("Taxable") = TotalTaxable
                                drNonCovidProratedDatatable("NonTaxable") = TotalNonTaxable
                                drNonCovidProratedDatatable("TaxRate") = (totalTax * 100.0) / TotalTaxable
                                drNonCovidProratedDatatable("Tax") = totalTax
                            End If
                        Next
                        drNonCovidProratedDatatable.AcceptChanges()
                    Next
                    dtNonCovidProratedDatatable.AcceptChanges()
                End If
            End If
            Session("Payee1DataTable_C19") = dtNonCovidProratedDatatable
        Catch
        End Try
    End Function
    'END : ML  | 2020.05.18 | YRS-AT-4874 | Merge NonCovid and Covid data in Pay1Datatable.

    Private Function CreateRowsForDisbursements(ByVal l_dec_TDAmount As Decimal, ByVal l_dec_TDRequestedAmount As Decimal)

        Dim l_AvailableBalanceDataTable1 As DataTable
        Dim l_AvailableBalanceDataTable As DataTable
        Dim l_SelectDataTable As DataTable

        Dim l_AnnuityBasisTypeArray() As String

        Dim l_AnnuityBasisType As String
        Dim l_AccountType As String

        Dim l_AvailableBalanceDataRow As DataRow
        Dim l_AvailableBalanceDataRows() As DataRow

        Dim l_Counter As Integer
        Dim l_PayeeCounter As Integer

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalPostTax As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal
        'Addition of new account types
        Dim l_AccountGroup As String = ""
        'Addition of new account types
        Dim l_AccountTypeAM As DataRow()
        Dim l_dt_TransactionType As DataTable
        Dim l_dt_TransactionDataTable As DataTable
        Dim l_decimal_AM_PersonalSideMoney As Decimal = 0.0
        Dim l_decimal_AM_YmcaSideMoney As Decimal = 0.0
        Session("HasPersonalSideForAM_C19") = Nothing
        Dim l_bool_IsRWPR As Boolean = False
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "CreateRowsForDisbursements() START")
            l_dt_TransactionType = GetTransactionType(Me.FundID)
            For Each dr As DataRow In l_dt_TransactionType.Rows
                If dr("AcctType").GetType.ToString() <> "System.DBNull" Then
                    If dr("AcctType") = "AM" Then
                        If dr("TransactType").GetType.ToString() <> "System.DBNull" Then
                            If dr("TransactType") = "RWPR" Then
                                l_bool_IsRWPR = True
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next
            l_AvailableBalanceDataTable = DirectCast(Session("AvailableBalance_C19"), DataTable)
            ' l_dt_TransactionDataTable = Session("ArrayRefundableDataTable_C19")
            MergeNonCOVIDandCOVIDDatatable()  ' ML | YRS-AT-4874 | Merge NonCovid and COVID datatable.


            'Get the personal and ymca total for AM account for the breakdown code 
            For Each dr As DataRow In l_AvailableBalanceDataTable.Rows
                If dr("AcctType").GetType.ToString() <> "System.DBNull" Then
                    If dr("AcctType").GetType.ToString() <> "Total" Then
                        If dr("AcctType") = "AM" Then
                            l_decimal_AM_PersonalSideMoney = l_decimal_AM_PersonalSideMoney + IIf(dr("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(dr("PersonalPreTax"), Decimal)) + IIf(dr("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(dr("PersonalPostTax"), Decimal))
                            l_decimal_AM_YmcaSideMoney = l_decimal_AM_YmcaSideMoney + IIf(dr("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(dr("YmcaPreTax"), Decimal))
                        End If
                    End If
                End If
            Next

            If l_decimal_AM_PersonalSideMoney > 0.0 Then
                Session("HasPersonalSideForAM_C19") = True
            Else
                Session("HasPersonalSideForAM_C19") = Nothing
            End If
            'if transact type is RWPR then we have to put 09,10 and not 07 .
            If l_bool_IsRWPR = True And l_decimal_AM_PersonalSideMoney = 0.0 Then
                Session("HasPersonalSideForAM_C19") = True
            End If
            'Get the personal and ymca total for AM account for the breakdown code 


            l_AnnuityBasisTypeArray = DirectCast(Session("AnnuityBasisTypeArray_C19"), String())


            If (Not l_AvailableBalanceDataTable Is Nothing) And (Not l_AnnuityBasisTypeArray Is Nothing) Then

                l_Counter = 0

                For Each l_AnnuityBasisType In l_AnnuityBasisTypeArray

                    If Not l_AnnuityBasisType Is Nothing Then

                        l_AvailableBalanceDataRows = (Me.GetDataRow(l_AvailableBalanceDataTable, l_AnnuityBasisType.Trim))

                        If Not l_AvailableBalanceDataRows Is Nothing Then

                            For Each l_AvailableBalanceDataRow In l_AvailableBalanceDataRows

                                If Not l_AvailableBalanceDataRow Is Nothing Then

                                    l_Decimal_PersonalPreTax = IIf(l_AvailableBalanceDataRow("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal))
                                    'l_Decimal_PersonalPostTax = IIf(l_AvailableBalanceDataRow("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalanceDataRow("PersonalPostTax"), Decimal))
                                    l_Decimal_PersonalPostTax = IIf(l_AvailableBalanceDataRow("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, (l_AvailableBalanceDataRow("PersonalPostTax")))
                                    l_Decimal_YMCAPreTax = IIf(l_AvailableBalanceDataRow("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalanceDataRow("YmcaPreTax"), Decimal))

                                    If (l_Decimal_PersonalPostTax + l_Decimal_PersonalPreTax + l_Decimal_YMCAPreTax) > 0.0 Then

                                        l_AccountType = DirectCast(l_AvailableBalanceDataRow("AcctType"), String)
                                        l_AccountGroup = DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper

                                        '***********************************************************************************************
                                        '* Loop Throuh all four Payee Cursors
                                        '* The fourth Payee contains information for Minimum Distributions
                                        '***********************************************************************************************

                                        For l_PayeeCounter = 1 To 4

                                            ''Fix the DataTable to Use. 


                                            Select Case l_PayeeCounter
                                                Case 1
                                                    l_SelectDataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)

                                                Case 2
                                                    l_SelectDataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)

                                                Case 3
                                                    l_SelectDataTable = DirectCast(Session("Payee3DataTable_C19"), DataTable)

                                                Case 4
                                                    l_SelectDataTable = DirectCast(Session("MinimumDistributionTable_C19"), DataTable)
                                            End Select


                                            '' Check for HardShip
                                            If l_Counter = 0 And l_PayeeCounter = 1 Then '&& First Time Through

                                                Me.ChangedRefundType = String.Empty


                                                If (Me.IsHardShip = False) And (Me.RefundType = "HARD") Then
                                                    '' Then this Refund is not actually hardShip. So make this refund Type as Vol.
                                                    Me.ChangedRefundType = "VOL"

                                                End If

                                            End If '' Check for harship.


                                            ''' Start do calculation.

                                            Dim l_SelectedRowFromDataTable As DataRow
                                            Dim l_CurrentDataTable As DataTable
                                            Dim l_CurrentDataRow As DataRow
                                            Dim l_TransactionType As String
                                            Dim l_String As String
                                            Dim l_SelectedRowFromDataTables As DataRow
                                            l_CurrentDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)


                                            If Not l_SelectDataTable Is Nothing Then

                                                l_SelectDataTable = Me.ClearZerosRows(l_SelectDataTable)

                                                'l_SelectedRowFromDataTables = CreateDataForRefund(l_SelectDataTable, l_dt_TransactionDataTable)

                                                l_SelectedRowFromDataTable = Me.GetDataRowForSelectedTable(l_SelectDataTable, l_AccountType)
                                                ' l_SelectedRowFromDataTable = Me.GetDataRowForSelectedTable(l_SelectedRowFromDataTables, l_AccountType)

                                                If Not l_SelectedRowFromDataTable Is Nothing Then

                                                    If Not l_CurrentDataTable Is Nothing Then
                                                        l_CurrentDataRow = Me.GetDataRowForSelectedTable(l_CurrentDataTable, l_AccountType, "AccountType")



                                                        If l_CurrentDataRow Is Nothing Then
                                                            l_AvailableBalanceDataRow("PersonalPreTax") = "0.00"
                                                            l_AvailableBalanceDataRow("PersonalPostTax") = "0.00"
                                                            l_AvailableBalanceDataRow("YmcaPreTax") = "0.00"
                                                        End If
                                                        ''
                                                        Do
                                                            l_TransactionType = String.Empty
                                                            '' This segmen to Findout && Principle Hardship
                                                            'START | 2019.07.26 |  SR | YRS-AT-4498 Starts | Commneted below line and added new condition to existing code to check for interest part to be incuded in disbursemt records or not. 
                                                            'If Me.IsHardShip And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                            If Me.IsHardShip And Not IsInterestAllowed And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                                'END | 2019.07.26 |  SR | YRS-AT-4498 Starts | Commneted below line and added new condition to existing code to check for interest part to be incuded in disbursemt records or not. 

                                                                If DirectCast(l_AvailableBalanceDataRow("MoneyType"), String).Trim = "IN" Then
                                                                    Exit Do
                                                                End If

                                                                'START | 2019.07.26 |  SR | YRS-AT-4498 Starts | Commneted below line as Non-Taxable componenet is available for Hardship TD amount
                                                                'If Me.HardshipAmount < 0.01 Or DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal) < 0.01 Then
                                                                '    Exit Do
                                                                'End If

                                                                If Me.HardshipAmount < 0.01 Or (DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal) < 0.01 And DirectCast(l_AvailableBalanceDataRow("PersonalPostTax"), Decimal) < 0.01) Then
                                                                    Exit Do
                                                                End If

                                                                'START | 2019.07.26 |  SR | YRS-AT-4498 | Create RHPR Transaction record only if Available balance and Balance remaining for payee is higher than 0.
                                                                If (DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("Taxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RHPR"
                                                                End If

                                                                '&& Principle Personal PostTax
                                                                If (Convert.ToDecimal(l_AvailableBalanceDataRow("PersonalPostTax")) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("NonTaxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RHPR"
                                                                End If
                                                                'END | 2019.07.26 |  SR | YRS-AT-4498 | Create RHPR Transaction record only if Available balance and Balance remaining for payee is higher than 0.

                                                                'l_TransactionType = "RHPR" -- SR | YRS-AT-4498 Starts | Commneted below line as Non-Taxable componenet is available for Hardship TD amount

                                                                '' This segment for checking && Principle
                                                            ElseIf DirectCast(l_AvailableBalanceDataRow("MoneyType"), String).Trim() = "PR" Then
                                                                If (DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("Taxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RFPR"
                                                                    ' START | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHPR' for Principle amount in Hardship amount.  
                                                                    If IsHardShip And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                                        l_TransactionType = "RHPR"
                                                                    End If
                                                                    ' END | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHPR' for Principle amount in Hardship amount.  
                                                                   
                                                                End If

                                                                '&& Principle Personal PostTax
                                                                ' If (DirectCast(l_AvailableBalanceDataRow("PersonalPostTax"), Decimal) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("NonTaxable"), Decimal) > 0.0) Then
                                                                If (Convert.ToDecimal(l_AvailableBalanceDataRow("PersonalPostTax")) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("NonTaxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RFPR"
                                                                    ' START | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHPR' for Principle amount in Hardship amount.  
                                                                    If IsHardShip And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                                        l_TransactionType = "RHPR"
                                                                    End If
                                                                    ' END | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHPR' for Principle amount in Hardship amount.  
                                                                  
                                                                End If
                                                                ' && Principle Personal PreTax
                                                                If (DirectCast(l_AvailableBalanceDataRow("YmcaPreTax"), Decimal) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("Taxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RFPR"
                                                                    ' START | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHPR' for Principle amount in Hardship amount.  
                                                                    If IsHardShip And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                                        l_TransactionType = "RHPR"
                                                                    End If
                                                                    ' END | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHPR' for Principle amount in Hardship amount.  
                                                                   
                                                                End If
                                                            Else
                                                                If (DirectCast(l_AvailableBalanceDataRow("PersonalPreTax"), Decimal) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("Taxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RFIN"
                                                                    ' START | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHIN' for Principle amount in Hardship amount.  
                                                                    If IsHardShip And IsInterestAllowed And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                                        l_TransactionType = "RHIN"
                                                                    End If
                                                                    ' END | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHIN' for Principle amount in Hardship amount.  
                                                                  
                                                                End If

                                                                '&& Interest Personal
                                                                If (DirectCast(l_AvailableBalanceDataRow("YmcaPreTax"), Decimal) > 0.0) And (DirectCast(l_SelectedRowFromDataTable("Taxable"), Decimal) > 0.0) Then
                                                                    l_TransactionType = "RFIN"
                                                                    ' START | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHIN' for Principle amount in Hardship amount.  
                                                                    If IsHardShip And IsInterestAllowed And ((DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TD) Or (DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper = m_const_SavingsPlan_TM)) Then
                                                                        l_TransactionType = "RHIN"
                                                                    End If
                                                                    ' END | SR | 2019.07.26 | YRS-AT-4498 | Add transact type 'RHIN' for Principle amount in Hardship amount.  
                                                                  
                                                                End If
                                                            End If '' IsHardShip..

                                                            '' Check for the Transaction Type
                                                            If l_TransactionType = String.Empty Then
                                                                Exit Do
                                                            Else
                                                                l_AccountType = DirectCast(l_AvailableBalanceDataRow("AcctType"), String).Trim.ToUpper
                                                                l_AccountGroup = DirectCast(l_AvailableBalanceDataRow("AccountGroup"), String).Trim.ToUpper
                                                                ' This is used to set the person is taking TD amount. 
                                                                If l_AccountGroup = m_const_SavingsPlan_TD Then
                                                                    Me.IsTookTDAccount = True
                                                                End If

                                                            End If

                                                            Select Case l_TransactionType.Trim
                                                                'START: SR | 07/24/2019 | YRS-AT-4498 | Refactored code by commenting existing case of RHPR transaction as same method will be called which is used for RFPR transaction
                                                                'Case "RHPR"
                                                                '    '' Do HardShip Priciple.
                                                                '    Me.HardshipPrinciple(l_SelectedRowFromDataTable, l_AvailableBalanceDataRow, l_AccountType, l_PayeeCounter, Me.CurrencyCode, l_TransactionType, l_AnnuityBasisType.Trim, l_AccountGroup.Trim.ToUpper(), l_dec_TDAmount, l_dec_TDRequestedAmount)
                                                                'END: SR | 07/24/2019 | YRS-AT-4498 | Refactored code by commenting existing case of RHPR transaction as same method will be called which is used for RFPR transaction
                                                                Case "RFPR", "RHPR" 'SR | 07/24/2019 | YRS-AT-4498 | Added case for RHPR Transaction
                                                                    '' Do Refund Principle.
                                                                    Me.RefundPrinciple(l_SelectedRowFromDataTable, l_AvailableBalanceDataRow, l_AccountType, l_PayeeCounter, Me.CurrencyCode, l_TransactionType, l_AnnuityBasisType.Trim, l_AccountGroup.Trim.ToUpper())

                                                                Case "RFIN", "RHIN" 'SR | 07/24/2019 | YRS-AT-4498 | Added case for RHIN Transaction
                                                                    '' Do Refund Interest.
                                                                    Me.RefundInterest(l_SelectedRowFromDataTable, l_AvailableBalanceDataRow, l_AccountType, l_PayeeCounter, Me.CurrencyCode, l_TransactionType, l_AnnuityBasisType.Trim, l_AccountGroup.Trim.ToUpper())

                                                            End Select


                                                        Loop



                                                    End If '' Current DataTable



                                                End If




                                            End If  ' Main ''' Start do calculation.

                                        Next '' For 1 to 4


                                    End If
                                End If '' DataRow


                            Next '' For array of selected Rows '' Me.GetRow

                        End If '' Get Row Nothing .

                    End If '' '' Annuity Basis Type array. Nothing
                    l_Counter += 1 ' Increment the Counter
                Next '' Annuity Basis Type array.


            End If '' main if.



        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Error : {0} ", ex.Message))
            Throw
            ' START | SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "CreateRowsForDisbursements() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            ' END | SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.

        End Try

    End Function


#Region " Hardship Principle "
    'Modified for new acct types wherein acct groups will be considered for fetching records for acct breakdown type 
    Private Function HardshipPrinciple(ByVal parameterSelectedDataRow As DataRow, ByVal parameterAvailableBalance As DataRow, ByVal parameterAccountType As String, ByVal parameterIndex As Integer, ByVal parameterCurrencyCode As String, ByVal parameterTransactionType As String, ByVal parameterAnnuityBasisType As String, ByVal parameterAccountGroup As String, ByVal l_dec_TDAmount As Decimal, ByVal l_dec_TDRequestedAmount As Decimal)

        Dim l_SelectedDataRow As DataRow
        Dim l_AvailableBalance As DataRow

        Dim l_AccountBreakDownType As String
        Dim l_AccountType As String

        Dim l_SortOrder As Integer
        'neeraj 16-10-2010 BT-664 : added plan type variable
        Dim l_PlanType As String

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_TaxRate As Decimal
        Dim l_Decimal_Taxable As Decimal

        Dim l_DisbursementDataRow As DataRow
        Dim l_DisbursementDetailsDataRow As DataRow
        Dim l_RefundsDataRow As DataRow
        Dim l_TransactionDataRow As DataRow
        'Addition of new account types variables
        Dim l_AccountGroup As String = ""
        'Addition of new account types variables
        Try

            '' Assign the values to Local Varibales to Understand the variable Names. :)

            l_SelectedDataRow = parameterSelectedDataRow
            l_AvailableBalance = parameterAvailableBalance
            l_AccountType = parameterAccountType
            'Addition of new account types variables
            l_AccountGroup = parameterAccountGroup
            'Addition of new account types variables
            'neeraj 16-11-2010 Bt-664: added new variable plantype in disbursement detail table
            l_PlanType = parameterAvailableBalance("PlanType")

            '' Check for the parameters.
            If (Not l_SelectedDataRow Is Nothing) And (Not l_AvailableBalance Is Nothing) And (Not l_AccountType Is Nothing) And (Not l_AccountGroup Is Nothing) Then

                ''
                l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, True, False, True, False)
                l_SortOrder = Me.GetAccountBreakDownSortOrder(l_AccountBreakDownType)



                l_DisbursementDataRow = Me.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode)

                If Not l_DisbursementDataRow Is Nothing Then
                    ' IB:added on 23/Aug/2010- BT 567: Casting problem 
                    l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, CType(l_DisbursementDataRow("UniqueID").ToString(), String), l_SortOrder, l_PlanType)
                    'l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, DirectCast(l_DisbursementDataRow("UniqueID"), String), l_SortOrder)
                    l_TransactionDataRow = Me.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType)


                    If Not l_TransactionDataRow Is Nothing Then
                        ' IB:added on 23/Aug/2010- BT 567: Casting problem 
                        'l_RefundsDataRow = Me.SetRefundsDataRow(DirectCast(l_TransactionDataRow("UniqueID"), String), parameterSelectedDataRow, l_AccountType, DirectCast(l_DisbursementDataRow("UniqueID"), String), parameterAnnuityBasisType)
                        l_RefundsDataRow = Me.SetRefundsDataRow(CType(l_TransactionDataRow("UniqueID").ToString(), String), parameterSelectedDataRow, l_AccountType, CType(l_DisbursementDataRow("UniqueID").ToString(), String), parameterAnnuityBasisType)


                        l_Decimal_PersonalPreTax = IIf(l_AvailableBalance("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal))
                        l_Decimal_TaxRate = IIf(l_SelectedDataRow("TaxRate").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("TaxRate"), Decimal))

                        'Added By ganeswar on july 13th for HardshipAmount
                        If l_Decimal_TaxRate = 0 And l_dec_TDAmount = 0 And l_dec_TDRequestedAmount = 0 Then
                            l_Decimal_TaxRate = 0
                        ElseIf l_Decimal_TaxRate = 0 And l_dec_TDAmount > 0 Then
                            l_Decimal_TaxRate = 10
                        End If
                        If l_dec_TDAmount > 0 Then
                            'l_Decimal_TaxRate = Math.Round((l_dec_TDAmount / l_dec_TDRequestedAmount * 100.0), 14)
                        Else
                            l_dec_TDAmount = (DirectCast(l_SelectedDataRow("Taxable"), Decimal) * (l_Decimal_TaxRate / 100.0))
                        End If
                        'Added By ganeswar on july 13th for HardshipAmount

                        If Me.HardshipAmount >= l_Decimal_PersonalPreTax Then

                            l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_PersonalPreTax
                            l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + l_Decimal_PersonalPreTax

                            'l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0))
                            l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + l_dec_TDAmount

                            l_TransactionDataRow("PersonalPreTax") = DirectCast(l_TransactionDataRow("PersonalPreTax"), Decimal) + (l_Decimal_PersonalPreTax * (-1.0))

                            l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                            'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (DirectCast(l_SelectedDataRow("Taxable"), Decimal) * (l_Decimal_TaxRate / 100.0))
                            l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + l_dec_TDAmount
                            l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate

                            Me.HardshipAmount = Me.HardshipAmount - l_Decimal_PersonalPreTax
                            l_SelectedDataRow("Taxable") = DirectCast(l_SelectedDataRow("Taxable"), Decimal) - l_Decimal_PersonalPreTax
                            l_AvailableBalance("PersonalPreTax") = "0.00"

                        Else

                            l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + DirectCast(l_SelectedDataRow("Taxable"), Decimal)

                            l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + DirectCast(l_SelectedDataRow("Taxable"), Decimal)
                            'l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (DirectCast(l_SelectedDataRow("Taxable"), Decimal) * (l_Decimal_TaxRate / 100.0))
                            l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + l_dec_TDAmount

                            l_TransactionDataRow("PersonalPreTax") = DirectCast(l_TransactionDataRow("PersonalPreTax"), Decimal) + (DirectCast(l_SelectedDataRow("Taxable"), Decimal) * (-1.0))

                            l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + DirectCast(l_SelectedDataRow("Taxable"), Decimal)
                            'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (DirectCast(l_SelectedDataRow("Taxable"), Decimal) * (l_Decimal_TaxRate / 100.0))
                            l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + l_dec_TDAmount
                            l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate

                            Me.HardshipAmount = Me.HardshipAmount - DirectCast(l_SelectedDataRow("Taxable"), Decimal)
                            l_AvailableBalance("PersonalPreTax") = DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal) - DirectCast(l_SelectedDataRow("Taxable"), Decimal)

                            l_SelectedDataRow("Taxable") = "0.00"

                        End If

                    End If
                End If
            End If '''' Check for the parameters.




        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region     'Hardship Principle 
    Private Function SetDisbursementDataRow(ByVal parameterIndex As Integer, ByVal parameterCurrencyCode As String) As DataRow '-- SetDisbArray - in FoxPro application.

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_FoundRows() As DataRow

        Dim l_QueryString As String

        Try

            l_DataTable = DirectCast(Session("R_Disbursements_C19"), DataTable)


            If Not l_DataTable Is Nothing Then

                If parameterIndex = 1 Or parameterIndex = 4 Then
                    l_QueryString = "PayeeEntityID = '" & Me.PersonID.ToString().Trim & "'"
                Else
                    '' ID's for Institution Name.

                    If parameterIndex = 2 Then
                        l_QueryString = "PayeeEntityID = '" & Me.Payee2ID.ToString().Trim & "'"

                    ElseIf parameterIndex = 3 Then
                        l_QueryString = "PayeeEntityID = '" & Me.Payee3ID.ToString().Trim & "'"
                    End If
                End If

                l_FoundRows = l_DataTable.Select(l_QueryString)

                l_DataRow = Nothing

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        l_DataRow = l_FoundRows(0)
                    End If
                End If


                If l_DataRow Is Nothing Then

                    l_DataRow = l_DataTable.NewRow

                    ''l_DataRow("UniqueID") = System.DBNull.Value     '' this Unique iD is used to keep track of other details
                    'l_DataRow("UniqueID") = l_DataTable.Rows.Count  '' this ID is used in Disbursement Details as Disbusement ID. 
                    'Neeraj 04May2010
                    l_DataRow("UniqueID") = System.Guid.NewGuid.ToString()

                    Select Case parameterIndex
                        Case 1
                            l_DataRow("PayeeEntityID") = Me.PersonID    '&& Payee1 Entity ID
                        Case 2
                            l_DataRow("PayeeEntityID") = Me.Payee2ID    '&& Payee2 Entity ID
                        Case 3
                            l_DataRow("PayeeEntityID") = Me.Payee3ID    '&& Payee3 Entity ID
                        Case 4
                            l_DataRow("PayeeEntityID") = Me.PersonID    '&& Payee1 Entity ID
                            ' ML : 05/05/2020 | YRS-AT-4874 | Check for COVID Distribution
                        Case 5
                            l_DataRow("PayeeEntityID") = Me.PersonID    '&& Payee1 Entity ID
                            ' ML : 05/05/2020 | YRS-AT-4874 | Check for COVID Distribution
                    End Select

                    l_DataRow("PayeeAddrID") = Me.PayeeAddressID                '&& Payee Address ID
                    l_DataRow("PayeeEntityTypeCode") = IIf((parameterIndex = 1 Or parameterIndex = 4), "PERSON", "ROLINS")  '&& Payee Entity Type ' ML : 05/05/2020 | YRS-AT-4874 | Check for COVID Distribution
                    l_DataRow("DisbursementType") = "REF"                       '&& Disbursement Type
                    l_DataRow("IrsTaxTypeCode") = System.DBNull.Value           '&& Irs Tax Type Code
                    l_DataRow("TaxableAmount") = "0.00"                         '&& Taxable Amount
                    l_DataRow("NonTaxableAmount") = "0.00"                      '&& Non Taxable Amount
                    l_DataRow("PaymentMethodCode") = "CHECK"                    '&& Payment Method
                    l_DataRow("CurrencyCode") = parameterCurrencyCode           '&& Currency Code
                    l_DataRow("BankID") = System.DBNull.Value                   '&& Bank ID
                    l_DataRow("DisbursementNumber") = System.DBNull.Value       '&& Disbursement Number (Check No)
                    l_DataRow("PersID") = Me.PersonID                           '&& Person ID
                    l_DataRow("DisbursementRefID") = System.DBNull.Value        '&& Disburs Ref ID
                    l_DataRow("Rollover") = System.DBNull.Value                 '&& Rollover Institution Type

                    l_DataTable.Rows.Add(l_DataRow)

                    Return l_DataRow

                Else
                    Return l_DataRow
                End If

            End If

            Return l_DataRow

        Catch ex As Exception
            Throw
        End Try


    End Function
    Private Function SetDisbursementDetailsDataRow(ByVal parameterAccountType As String, ByVal parameterAcctBreakdownType As String, ByVal parameterDisbursementID As String, ByVal parameterSortOrder As String, ByVal parameterPalnType As String) As DataRow

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_FoundRows() As DataRow

        Dim l_QueryString As String

        Try

            l_DataTable = DirectCast(Session("R_DisbursementDetails_C19"), DataTable)

            If Not l_DataTable Is Nothing Then

                l_QueryString = "AcctType = '" & parameterAccountType.ToString & "' AND AcctBreakdownType = '" & parameterAcctBreakdownType & "' AND DisbursementID = '" & parameterDisbursementID & "'"


                l_FoundRows = l_DataTable.Select(l_QueryString)

                l_DataRow = Nothing

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        l_DataRow = l_FoundRows(0)
                    End If
                End If


                If l_DataRow Is Nothing Then

                    l_DataRow = l_DataTable.NewRow

                    l_DataRow("UniqueID") = System.DBNull.Value
                    l_DataRow("DisbursementID") = parameterDisbursementID
                    l_DataRow("AcctType") = parameterAccountType
                    l_DataRow("AcctBreakdownType") = parameterAcctBreakdownType
                    l_DataRow("SortOrder") = parameterSortOrder

                    l_DataRow("TaxablePrincipal") = "0.00"
                    l_DataRow("TaxableInterest") = "0.00"
                    l_DataRow("NonTaxablePrincipal") = "0.00"
                    l_DataRow("TaxWithheldPrincipal") = "0.00"
                    l_DataRow("TaxWithheldInterest") = "0.00"
                    l_DataRow("PlanType") = parameterPalnType
                    l_DataTable.Rows.Add(l_DataRow)

                    Return l_DataRow

                Else
                    Return l_DataRow
                End If

            End If

            Return l_DataRow

        Catch ex As Exception
            Throw
        End Try



    End Function
    Private Function SetTransactionDataRow(ByVal parameterAvailableBalance As DataRow, ByVal parameterIndex As Integer, ByVal parameterTransactionType As String) As DataRow

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_FoundRows() As DataRow

        Dim l_QueryString As String

        Dim l_PayeeID As String
        Dim l_AccountType As String
        Dim l_TransactType As String
        Dim l_AnnuityBasisType As String
        Dim l_TransactionRefID As String

        Try

            l_DataTable = DirectCast(Session("R_Transactions_C19"), DataTable)

            If (parameterIndex = 1) Or (parameterIndex = 4) Then
                l_PayeeID = Me.PersonID
            ElseIf parameterIndex = 2 Then
                l_PayeeID = Me.Payee2ID
            ElseIf parameterIndex = 3 Then
                l_PayeeID = Me.Payee3ID
            End If


            If (Not l_DataTable Is Nothing) And (Not parameterAvailableBalance Is Nothing) Then

                l_AccountType = DirectCast(parameterAvailableBalance("AcctType"), String)
                l_TransactType = parameterTransactionType
                l_AnnuityBasisType = DirectCast(parameterAvailableBalance("AnnuityBasisType"), String)

                l_QueryString = "AcctType = '" & l_AccountType.Trim & "' AND TransactType = '" & l_TransactType.Trim & "' AND AnnuityBasisType = '" & l_AnnuityBasisType.Trim & "' AND Creator = '" & l_PayeeID.Trim & "'"

                l_FoundRows = l_DataTable.Select(l_QueryString)

                l_DataRow = Nothing

                Dim l_AvailableBalanceRefID As String
                Dim l_TempDataRowRefID As String

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        l_DataRow = l_FoundRows(0)


                        If parameterAvailableBalance("TransactionRefID").GetType.ToString.Trim = "System.DBNull" Then
                            l_AvailableBalanceRefID = String.Empty
                        Else
                            l_AvailableBalanceRefID = parameterAvailableBalance("TransactionRefID").ToString


                        End If

                        If l_DataRow("TransactionRefID").GetType.ToString.Trim = "System.DBNull" Then
                            l_TempDataRowRefID = String.Empty
                        Else
                            l_TempDataRowRefID = l_DataRow("TransactionRefID").ToString
                        End If

                        If Not (l_AvailableBalanceRefID.Trim = l_TempDataRowRefID.Trim) Then
                            l_DataRow = Nothing
                        End If

                    End If
                End If

                If l_DataRow Is Nothing Then

                    l_DataRow = l_DataTable.NewRow

                    'Neeraj 04May2010:
                    l_DataRow("UniqueID") = System.Guid.NewGuid.ToString() 'l_DataTable.Rows.Count
                    l_DataRow("PersID") = Me.PersonID
                    l_DataRow("FundEventID") = Me.FundID
                    l_DataRow("YmcaID") = System.DBNull.Value           '' We can add in store Proc Itself
                    l_DataRow("AcctType") = l_AccountType
                    l_DataRow("TransactType") = l_TransactType
                    l_DataRow("AnnuityBasisType") = l_AnnuityBasisType
                    l_DataRow("MonthlyComp") = "0.00"
                    l_DataRow("PersonalPreTax") = "0.00"
                    l_DataRow("PersonalPostTax") = "0.00"
                    l_DataRow("YmcaPreTax") = "0.00"
                    l_DataRow("ReceivedDate") = Date.Now
                    l_DataRow("AccountingDate") = Date.Now          '' Needs to Get Account date. in SP we can do.
                    l_DataRow("TransactDate") = Date.Now
                    l_DataRow("FundedDate") = Date.Now              '' Needs to Get Account date. in SP we can do.
                    l_DataRow("TransmittalID") = System.DBNull.Value
                    l_DataRow("TransactionRefID") = parameterAvailableBalance("TransactionRefID")
                    l_DataRow("Creator") = l_PayeeID                '' temperarily i am stroing the Payee ID to filter the Rows, in the above filter.

                    l_DataTable.Rows.Add(l_DataRow)

                    Return l_DataRow

                Else
                    Return l_DataRow
                End If

            End If

            Return l_DataRow

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function SetRefundsDataRow(ByVal parameterTransactionID As String, ByVal paramterSelectedDataRow As DataRow, ByVal parameterAccountType As String, ByVal parameterDisbursementID As String, ByVal parameterAnnuityBasisType As String)

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_FoundRows() As DataRow

        Dim l_QueryString As String

        Try

            l_DataTable = DirectCast(Session("R_Refunds_C19"), DataTable)


            If Not l_DataTable Is Nothing Then

                l_QueryString = "TransactID = '" & parameterTransactionID & "'"

                l_FoundRows = l_DataTable.Select(l_QueryString)

                l_DataRow = Nothing

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        l_DataRow = l_FoundRows(0)
                    End If
                End If


                If l_DataRow Is Nothing Then

                    l_DataRow = l_DataTable.NewRow

                    'Neeraj 04May2010:
                    l_DataRow("Uniqueid") = System.Guid.NewGuid.ToString()      'System.DBNull.Value                     '&& UniqueID
                    l_DataRow("RefRequestsID") = Me.SessionRefundRequestID
                    l_DataRow("AcctType") = parameterAccountType                    '&& Account Type
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                    l_DataRow("Tax") = "0.00"
                    l_DataRow("TaxRate") = "0.00"
                    l_DataRow("Payee") = paramterSelectedDataRow("Payee")
                    l_DataRow("FundedDate") = System.DBNull.Value

                    '' This chek for Check Whether given refund is VOL Or Realy HardShip
                    If Me.ChangedRefundType = String.Empty Then
                        l_DataRow("RequestType") = Me.RefundType
                    Else
                        l_DataRow("RequestType") = Me.ChangedRefundType
                    End If

                    l_DataRow("TransactID") = parameterTransactionID
                    'l_DataRow("AnnuityBasisType") = paramterSelectedDataRow("AnnuityBasisType")
                    l_DataRow("AnnuityBasisType") = parameterAnnuityBasisType
                    l_DataRow("DisbursementID") = parameterDisbursementID

                    l_DataTable.Rows.Add(l_DataRow)

                    Return l_DataRow

                Else
                    Return l_DataRow
                End If

            End If

            Return l_DataRow

        Catch ex As Exception
            Throw
        End Try
    End Function
#Region " Refund Principle "
    'Modified for addition of new acct types -- breakdown types will be fetched in accordance to acct groups and not acct types
    Private Function RefundPrinciple(ByVal parameterSelectedDataRow As DataRow, ByVal parameterAvailableBalance As DataRow, ByVal parameterAccountType As String, ByVal parameterIndex As Integer, ByVal parameterCurrencyCode As String, ByVal parameterTransactionType As String, ByVal parameterAnnuityBasisType As String, ByVal parameterAccountGroup As String)

        '***********************************************************************************************
        '* Use Principle for Refund
        '***********************************************************************************************
        Dim l_SelectedDataRow As DataRow
        Dim l_AvailableBalance As DataRow

        Dim l_AccountBreakDownType As String
        Dim l_AccountType As String
        'neeraj 16-10-2010 BT-664 : added plan type variable
        Dim l_PlanType As String

        Dim l_SortOrder As Integer

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalPostTax As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal

        Dim l_Decimal_TaxRate As Decimal
        Dim l_Decimal_SelectedTaxableAmount As Decimal
        Dim l_Decimal_SelectedNonTaxableAmount As Decimal

        Dim l_DisbursementDataRow As DataRow
        Dim l_DisbursementDetailsDataRow As DataRow
        Dim l_RefundsDataRow As DataRow
        Dim l_TransactionDataRow As DataRow
        'Session("HasPersonalSideForAM") = Nothing
        'Addition of new acct types variable
        Dim l_AccountGroup As String = ""
        'Addition of new acct types variable

        Try
            '' Assign the Values. 

            l_SelectedDataRow = parameterSelectedDataRow
            l_AvailableBalance = parameterAvailableBalance
            l_AccountType = parameterAccountType
            'Addition of new acct types
            l_AccountGroup = parameterAccountGroup.Trim.ToUpper()
            'Addition of new acct types
            'neeraj 16-11-2010 Bt-664: added new variable plantype in disbursement detail table
            l_PlanType = parameterAvailableBalance("PlanType")

            If l_SelectedDataRow Is Nothing Then Return 0

            If l_AvailableBalance Is Nothing Then Return 0

            l_Decimal_SelectedTaxableAmount = IIf(l_SelectedDataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("Taxable"), Decimal))
            l_Decimal_SelectedNonTaxableAmount = IIf(l_SelectedDataRow("NonTaxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("NonTaxable"), Decimal))

            l_Decimal_TaxRate = IIf(l_SelectedDataRow("TaxRate").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("TaxRate"), Decimal))

            l_Decimal_PersonalPreTax = IIf(l_AvailableBalance("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal))
            l_Decimal_PersonalPostTax = IIf(l_AvailableBalance("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPostTax"), Decimal))
            l_Decimal_YMCAPreTax = IIf(l_AvailableBalance("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("YmcaPreTax"), Decimal))



            '**** Personal Side Principle Taxable

            If l_Decimal_SelectedTaxableAmount > 0.0 And l_Decimal_PersonalPreTax > 0.0 Then
                'If l_AccountGroup = m_const_RetirementPlan_AM Then
                '    Session("HasPersonalSideForAM") = True
                'End If

                If l_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) = 0.0) Then
                    l_AccountBreakDownType = "07"
                Else
                    l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, True, False, True, False)
                End If

                l_SortOrder = Me.GetAccountBreakDownSortOrder(l_AccountBreakDownType)


                l_DisbursementDataRow = Me.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode)

                If Not l_DisbursementDataRow Is Nothing Then


                    l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, l_DisbursementDataRow("UniqueID").ToString(), l_SortOrder, l_PlanType)


                    l_TransactionDataRow = Me.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType)


                    If Not l_TransactionDataRow Is Nothing Then

                        l_RefundsDataRow = Me.SetRefundsDataRow(l_TransactionDataRow("UniqueID").ToString(), parameterSelectedDataRow, l_AccountType, l_DisbursementDataRow("UniqueID").ToString(), parameterAnnuityBasisType)

                        If Not l_RefundsDataRow Is Nothing Then
                            If l_Decimal_SelectedTaxableAmount >= l_Decimal_PersonalPreTax Then

                                l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_PersonalPreTax

                                l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + l_Decimal_PersonalPreTax
                                l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0)), 2)

                                l_TransactionDataRow("PersonalPreTax") = DirectCast(l_TransactionDataRow("PersonalPreTax"), Decimal) + (l_Decimal_PersonalPreTax * (-1.0))

                                l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                'l_RefundsDataRow("Tax") =DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0))
                                l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0))
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate


                                l_SelectedDataRow("Taxable") = DirectCast(l_SelectedDataRow("Taxable"), Decimal) - l_Decimal_PersonalPreTax
                                'START: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount after decuting from the personal pre tax bucket
                                'START: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                'If Me.IsHardShip Then
                                If (Me.IsHardShip And ((l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TD) Or (l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TM))) Then
                                    'END: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                    Me.HardshipAmount = Me.HardshipAmount - l_Decimal_PersonalPreTax
                                End If
                                'END: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount after decuting from the personal pre tax bucket
                                l_AvailableBalance("PersonalPreTax") = "0.00"

                            Else

                                l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_SelectedTaxableAmount

                                l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + l_Decimal_SelectedTaxableAmount
                                l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)

                                l_TransactionDataRow("PersonalPreTax") = DirectCast(l_TransactionDataRow("PersonalPreTax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (-1.0))

                                l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_SelectedTaxableAmount
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0))
                                l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate

                                l_AvailableBalance("PersonalPreTax") = DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal) - DirectCast(l_SelectedDataRow("Taxable"), Decimal)
                                'START: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount (this will be reduced to zero as all the requested amount will be used up from this bucket)
                                'START: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                'If Me.IsHardShip Then
                                If (Me.IsHardShip And ((l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TD) Or (l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TM))) Then
                                    'END: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                    Me.HardshipAmount = Me.HardshipAmount - DirectCast(l_SelectedDataRow("Taxable"), Decimal)
                                End If
                                'END: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount (this will be reduced to zero as all the requested amount will be used up from this bucket)
                                l_SelectedDataRow("Taxable") = "0.00"

                            End If
                        End If
                    End If
                End If


            End If '**** Personal Side Principle Taxable


            '********Reinitializing local variables
            l_Decimal_SelectedTaxableAmount = IIf(l_SelectedDataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("Taxable"), Decimal))
            l_Decimal_SelectedNonTaxableAmount = IIf(l_SelectedDataRow("NonTaxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("NonTaxable"), Decimal))

            l_Decimal_TaxRate = IIf(l_SelectedDataRow("TaxRate").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("TaxRate"), Decimal))

            l_Decimal_PersonalPreTax = IIf(l_AvailableBalance("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal))
            l_Decimal_PersonalPostTax = IIf(l_AvailableBalance("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPostTax"), Decimal))
            l_Decimal_YMCAPreTax = IIf(l_AvailableBalance("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("YmcaPreTax"), Decimal))

            '**** YMCA Side Principle Taxable
            '*********Modified by Shubhrata in accordance to mail by Mark Posey Dated July 12th,2007
            '*****For acct types AM,SR,TM YMCA side is allowed for Personal Refund and for the other acct groups YMCA side is not allowed if Personal Refund takes place.
            If (l_Decimal_SelectedTaxableAmount > 0.0 And l_Decimal_YMCAPreTax > 0.0) Then
                'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                'If (((l_AccountGroup = m_const_RetirementPlan_AM Or l_AccountGroup = m_const_RetirementPlan_SR Or l_AccountGroup = m_const_SavingsPlan_TM) And (IsPersonalOnly = True)) Or (IsPersonalOnly = False)) Then
                If True Then
                    'Added Ganeswar 10thApril09 for BA Account Phase-V /End
                    If Me.IsVested = True Or l_AccountGroup = m_const_RetirementPlan_AM Or l_AccountGroup = m_const_SavingsPlan_TM Then

                        If Not Session("HasPersonalSideForAM_C19") Is Nothing Then
                            l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, False, True, False, False)
                        Else
                            If l_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) = 0.0) Then
                                l_AccountBreakDownType = "07"
                            Else
                                l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, False, True, False, False)
                            End If
                        End If

                        l_SortOrder = Me.GetAccountBreakDownSortOrder(l_AccountBreakDownType)


                        l_DisbursementDataRow = Me.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode)

                        If Not l_DisbursementDataRow Is Nothing Then


                            l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, l_DisbursementDataRow("UniqueID").ToString(), l_SortOrder, l_PlanType)


                            l_TransactionDataRow = Me.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType)


                            If Not l_TransactionDataRow Is Nothing Then

                                l_RefundsDataRow = Me.SetRefundsDataRow(l_TransactionDataRow("UniqueID").ToString(), parameterSelectedDataRow, l_AccountType, l_DisbursementDataRow("UniqueID").ToString(), parameterAnnuityBasisType)

                                If Not l_RefundsDataRow Is Nothing Then

                                    If l_Decimal_SelectedTaxableAmount >= l_Decimal_YMCAPreTax Then

                                        l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_YMCAPreTax

                                        l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + l_Decimal_YMCAPreTax
                                        l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100.0)), 2)

                                        l_TransactionDataRow("YmcaPreTax") = DirectCast(l_TransactionDataRow("YmcaPreTax"), Decimal) + (l_Decimal_YMCAPreTax * (-1.0))
                                        'Commented by Shubhrata YREN - 3025 Jan 19th 2007
                                        'l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                        'Added by  Shubhrata YREN - 3025 Jan 19th 2007
                                        l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_YMCAPreTax

                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100.0))
                                        l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100.0)), 2)
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate


                                        l_SelectedDataRow("Taxable") = DirectCast(l_SelectedDataRow("Taxable"), Decimal) - l_Decimal_YMCAPreTax
                                        l_AvailableBalance("YmcaPreTax") = "0.00"


                                    Else

                                        l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_SelectedTaxableAmount

                                        l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + l_Decimal_SelectedTaxableAmount
                                        l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)

                                        l_TransactionDataRow("YmcaPreTax") = DirectCast(l_TransactionDataRow("YmcaPreTax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (-1.0))

                                        l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_SelectedTaxableAmount
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0))
                                        l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate

                                        l_AvailableBalance("YmcaPreTax") = l_Decimal_YMCAPreTax - l_Decimal_SelectedTaxableAmount
                                        l_SelectedDataRow("Taxable") = "0.00"

                                    End If
                                End If
                            End If
                        End If
                    End If ''IsVested 
                End If
            End If
            '********Reinitializing local variables
            l_Decimal_SelectedTaxableAmount = IIf(l_SelectedDataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("Taxable"), Decimal))
            l_Decimal_SelectedNonTaxableAmount = IIf(l_SelectedDataRow("NonTaxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("NonTaxable"), Decimal))

            l_Decimal_TaxRate = IIf(l_SelectedDataRow("TaxRate").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("TaxRate"), Decimal))

            l_Decimal_PersonalPreTax = IIf(l_AvailableBalance("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal))
            l_Decimal_PersonalPostTax = IIf(l_AvailableBalance("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPostTax"), Decimal))
            l_Decimal_YMCAPreTax = IIf(l_AvailableBalance("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("YmcaPreTax"), Decimal))

            '**** Personal Side Principle NonTaxable
            If l_Decimal_SelectedNonTaxableAmount > 0.0 And l_Decimal_PersonalPostTax > 0.0 Then

                If l_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) = 0.0) Then
                    l_AccountBreakDownType = "07"
                Else
                    l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, True, False, False, True)
                End If

                l_SortOrder = Me.GetAccountBreakDownSortOrder(l_AccountBreakDownType)



                l_DisbursementDataRow = Me.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode)

                If Not l_DisbursementDataRow Is Nothing Then


                    l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, l_DisbursementDataRow("UniqueID").ToString(), l_SortOrder, l_PlanType)


                    l_TransactionDataRow = Me.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType)


                    If Not l_TransactionDataRow Is Nothing Then

                        'Neeraj 04May2010:
                        l_RefundsDataRow = Me.SetRefundsDataRow(l_TransactionDataRow("UniqueID").ToString(), parameterSelectedDataRow, l_AccountType, l_DisbursementDataRow("UniqueID").ToString(), parameterAnnuityBasisType)

                        If Not l_RefundsDataRow Is Nothing Then

                            If l_Decimal_SelectedNonTaxableAmount >= l_Decimal_PersonalPostTax Then

                                l_DisbursementDataRow("NonTaxableAmount") = DirectCast(l_DisbursementDataRow("NonTaxableAmount"), Decimal) + l_Decimal_PersonalPostTax
                                l_DisbursementDetailsDataRow("NonTaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("NonTaxablePrincipal"), Decimal) + l_Decimal_PersonalPostTax
                                l_TransactionDataRow("PersonalPostTax") = DirectCast(l_TransactionDataRow("PersonalPostTax"), Decimal) + (l_Decimal_PersonalPostTax * (-1.0))
                                l_RefundsDataRow("NonTaxable") = DirectCast(l_RefundsDataRow("NonTaxable"), Decimal) + l_Decimal_PersonalPostTax

                                l_SelectedDataRow("NonTaxable") = DirectCast(l_SelectedDataRow("NonTaxable"), Decimal) - l_Decimal_PersonalPostTax
                                'START: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount after decuting from the personal post tax bucket
                                'START: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                'If Me.IsHardShip Then
                                If (Me.IsHardShip And ((l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TD) Or (l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TM))) Then
                                    'END: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                    Me.HardshipAmount = Me.HardshipAmount - l_Decimal_PersonalPostTax
                                End If
                                'END: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount after decuting from the personal post tax bucket
                                l_AvailableBalance("PersonalPostTax") = "0.00"


                            Else

                                l_DisbursementDataRow("NonTaxableAmount") = DirectCast(l_DisbursementDataRow("NonTaxableAmount"), Decimal) + l_Decimal_SelectedNonTaxableAmount
                                l_DisbursementDetailsDataRow("NonTaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("NonTaxablePrincipal"), Decimal) + l_Decimal_SelectedNonTaxableAmount
                                l_TransactionDataRow("PersonalPostTax") = DirectCast(l_TransactionDataRow("PersonalPostTax"), Decimal) + (l_Decimal_SelectedNonTaxableAmount * (-1.0))
                                l_RefundsDataRow("NonTaxable") = DirectCast(l_RefundsDataRow("NonTaxable"), Decimal) + l_Decimal_SelectedNonTaxableAmount

                                l_AvailableBalance("PersonalPostTax") = DirectCast(l_AvailableBalance("PersonalPostTax"), Decimal) - l_Decimal_SelectedNonTaxableAmount
                                'START: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount (this will be reduced to zero as all the requested amount will be used up from this bucket)
                                'START: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                'If Me.IsHardShip Then
                                If (Me.IsHardShip And ((l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TD) Or (l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TM))) Then
                                    'END: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                    Me.HardshipAmount = Me.HardshipAmount - DirectCast(l_SelectedDataRow("NonTaxable"), Decimal)
                                End If
                                'END: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount (this will be reduced to zero as all the requested amount will be used up from this bucket)
                                l_SelectedDataRow("NonTaxable") = "0.00"

                            End If

                        End If

                    End If
                End If

            End If
        Catch ex As Exception
            Throw
        End Try



    End Function

#End Region 'Refund Principle


#Region " Refund Interest "

    '***********************************************************************************************
    '* Refund Interest Amounts
    '* Modified for addition of new acct types -- to fetch breakdown types in accordance to acct groups
    '***********************************************************************************************

    Private Function RefundInterest(ByVal parameterSelectedDataRow As DataRow, ByVal parameterAvailableBalance As DataRow, ByVal parameterAccountType As String, ByVal parameterIndex As Integer, ByVal parameterCurrencyCode As String, ByVal parameterTransactionType As String, ByVal parameterAnnuityBasisType As String, ByVal parameterAccountGroup As String)

        Dim l_SelectedDataRow As DataRow
        Dim l_AvailableBalance As DataRow

        Dim l_AccountBreakDownType As String
        Dim l_AccountType As String

        Dim l_SortOrder As Integer

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalPostTax As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal

        Dim l_Decimal_TaxRate As Decimal
        Dim l_Decimal_SelectedTaxableAmount As Decimal
        Dim l_Decimal_SelectedNonTaxableAmount As Decimal

        Dim l_DisbursementDataRow As DataRow
        Dim l_DisbursementDetailsDataRow As DataRow
        Dim l_RefundsDataRow As DataRow
        Dim l_TransactionDataRow As DataRow
        'anita yren 4450
        '  Session("HasPersonalSideForAM") = Nothing
        'anita yren 4450
        'Addition of new acct variables
        Dim l_AccountGroup As String = ""
        'Addition of new acct variables
        Dim l_PlanType As String
        Try

            '' Assign the Values. 

            l_SelectedDataRow = parameterSelectedDataRow
            l_AvailableBalance = parameterAvailableBalance
            l_AccountType = parameterAccountType
            'Addition of new acct
            l_AccountGroup = parameterAccountGroup.Trim.ToUpper()
            'Addition of new acct
            'neeraj 16-11-2010 Bt-664: added new variable plantype in disbursement detail table
            l_PlanType = parameterAvailableBalance("PlanType")

            If l_SelectedDataRow Is Nothing Then Return 0

            If l_AvailableBalance Is Nothing Then Return 0

            l_Decimal_SelectedTaxableAmount = IIf(l_SelectedDataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("Taxable"), Decimal))
            l_Decimal_SelectedNonTaxableAmount = IIf(l_SelectedDataRow("NonTaxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("NonTaxable"), Decimal))

            l_Decimal_TaxRate = IIf(l_SelectedDataRow("TaxRate").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("TaxRate"), Decimal))

            l_Decimal_PersonalPreTax = IIf(l_AvailableBalance("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal))
            l_Decimal_PersonalPostTax = IIf(l_AvailableBalance("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPostTax"), Decimal))
            l_Decimal_YMCAPreTax = IIf(l_AvailableBalance("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("YmcaPreTax"), Decimal))


            '***********************************************************************************************
            '* Personal Side Interest
            '***********************************************************************************************

            If l_Decimal_SelectedTaxableAmount > 0.0 And l_Decimal_PersonalPreTax > 0.0 Then
                'If l_AccountGroup = m_const_RetirementPlan_AM Then
                '    Session("HasPersonalSideForAM") = True
                'End If
                If l_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) = 0.0) Then
                    l_AccountBreakDownType = "07"
                    'Begin Code Merge by Dilip on 07-05-2009
                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added ElseIf condition for AccountGroup
                ElseIf l_AccountGroup = m_const_RetirementPlan_AC Then
                    l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, True, False, True, False)
                    'End 14-Jan-2008
                    'End Code Merge by Dilip on 07-05-2009
                Else
                    l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, True, False, False, True)
                End If

                l_SortOrder = Me.GetAccountBreakDownSortOrder(l_AccountBreakDownType)


                l_DisbursementDataRow = Me.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode)

                If Not l_DisbursementDataRow Is Nothing Then


                    l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, l_DisbursementDataRow("UniqueID").ToString(), l_SortOrder, l_PlanType)


                    l_TransactionDataRow = Me.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType)


                    If Not l_TransactionDataRow Is Nothing Then

                        l_RefundsDataRow = Me.SetRefundsDataRow(l_TransactionDataRow("UniqueID").ToString(), parameterSelectedDataRow, l_AccountType, l_DisbursementDataRow("UniqueID").ToString(), parameterAnnuityBasisType)

                        If Not l_RefundsDataRow Is Nothing Then

                            If l_Decimal_SelectedTaxableAmount >= l_Decimal_PersonalPreTax Then

                                l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_PersonalPreTax

                                l_DisbursementDetailsDataRow("TaxableInterest") = DirectCast(l_DisbursementDetailsDataRow("TaxableInterest"), Decimal) + l_Decimal_PersonalPreTax
                                l_DisbursementDetailsDataRow("TaxWithheldInterest") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldInterest"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0)), 2)

                                l_TransactionDataRow("PersonalPreTax") = DirectCast(l_TransactionDataRow("PersonalPreTax"), Decimal) + (l_Decimal_PersonalPreTax * (-1.0))

                                l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0))
                                l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0)), 2)
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate


                                l_SelectedDataRow("Taxable") = DirectCast(l_SelectedDataRow("Taxable"), Decimal) - l_Decimal_PersonalPreTax
                                'START: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount after decuting from the personal pre tax bucket
                                'START: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                'If Me.IsHardShip Then
                                If (Me.IsHardShip And ((l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TD) Or (l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TM))) Then
                                    'END: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                    Me.HardshipAmount = Me.HardshipAmount - l_Decimal_PersonalPreTax
                                End If
                                'END: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount after decuting from the personal pre tax bucket

                                l_AvailableBalance("PersonalPreTax") = "0.00"

                            Else

                                l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_SelectedTaxableAmount

                                l_DisbursementDetailsDataRow("TaxableInterest") = DirectCast(l_DisbursementDetailsDataRow("TaxableInterest"), Decimal) + l_Decimal_SelectedTaxableAmount
                                l_DisbursementDetailsDataRow("TaxWithheldInterest") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldInterest"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)

                                l_TransactionDataRow("PersonalPreTax") = DirectCast(l_TransactionDataRow("PersonalPreTax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (-1.0))

                                l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_SelectedTaxableAmount
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0))
                                l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)
                                'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate


                                l_AvailableBalance("PersonalPreTax") = DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal) - l_Decimal_SelectedTaxableAmount
                                'START: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount (this will be reduced to zero as all the requested amount will be used up from this bucket)
                                'START: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                'If Me.IsHardShip Then
                                If (Me.IsHardShip And ((l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TD) Or (l_AccountGroup.Trim.ToUpper = m_const_SavingsPlan_TM))) Then
                                    'END: SR | 08/28/2019 | YRS-AT-4498 | Reduce requested Hardship Amount only if account group is TD or TM
                                    Me.HardshipAmount = Me.HardshipAmount - DirectCast(l_SelectedDataRow("Taxable"), Decimal)
                                End If
                                'END: SR | 07/24/2019 | YRS-AT-4498 | Added to reduce hardship amount (this will be reduced to zero as all the requested amount will be used up from this bucket)
                                l_SelectedDataRow("Taxable") = "0.00"

                            End If
                        End If
                    End If
                End If
            End If '' Mian if

            l_Decimal_SelectedTaxableAmount = IIf(l_SelectedDataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("Taxable"), Decimal))
            l_Decimal_SelectedNonTaxableAmount = IIf(l_SelectedDataRow("NonTaxable").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("NonTaxable"), Decimal))
            l_Decimal_TaxRate = IIf(l_SelectedDataRow("TaxRate").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_SelectedDataRow("TaxRate"), Decimal))

            l_Decimal_PersonalPreTax = IIf(l_AvailableBalance("PersonalPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPreTax"), Decimal))
            l_Decimal_PersonalPostTax = IIf(l_AvailableBalance("PersonalPostTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("PersonalPostTax"), Decimal))
            l_Decimal_YMCAPreTax = IIf(l_AvailableBalance("YmcaPreTax").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_AvailableBalance("YmcaPreTax"), Decimal))

            '***********************************************************************************************
            '* YMCA Side Interest
            '***********************************************************************************************
            If (l_Decimal_SelectedTaxableAmount > 0.0 And l_Decimal_YMCAPreTax > 0.0) Then
                'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                'If (((l_AccountGroup = m_const_RetirementPlan_AM Or l_AccountGroup = m_const_RetirementPlan_SR Or l_AccountGroup = m_const_SavingsPlan_TM) And Me.IsPersonalOnly = True) Or (Me.IsPersonalOnly = False)) Then
                If True Then
                    'Added Ganeswar 10thApril09 for BA Account Phase-V /End
                    If Me.IsVested = True Or l_AccountGroup = m_const_RetirementPlan_AM Or l_AccountGroup = m_const_SavingsPlan_TM Then
                        If Not Session("HasPersonalSideForAM_C19") Is Nothing Then
                            l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, False, True, False, False)
                        Else
                            If l_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) = 0.0) Then
                                l_AccountBreakDownType = "07"
                            Else
                                l_AccountBreakDownType = Me.GetAccountBreakDownType(l_AccountGroup, False, True, False, False)
                            End If
                        End If


                        l_SortOrder = Me.GetAccountBreakDownSortOrder(l_AccountBreakDownType)

                        l_DisbursementDataRow = Me.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode)

                        If Not l_DisbursementDataRow Is Nothing Then


                            l_DisbursementDetailsDataRow = Me.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, l_DisbursementDataRow("UniqueID").ToString(), l_SortOrder, l_PlanType)

                            l_TransactionDataRow = Me.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType)


                            If Not l_TransactionDataRow Is Nothing Then

                                l_RefundsDataRow = Me.SetRefundsDataRow(l_TransactionDataRow("UniqueID").ToString(), parameterSelectedDataRow, l_AccountType, l_DisbursementDataRow("UniqueID").ToString(), parameterAnnuityBasisType)

                                If Not l_RefundsDataRow Is Nothing Then

                                    If l_Decimal_SelectedTaxableAmount >= l_Decimal_YMCAPreTax Then

                                        l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_YMCAPreTax

                                        l_DisbursementDetailsDataRow("TaxableInterest") = DirectCast(l_DisbursementDetailsDataRow("TaxableInterest"), Decimal) + l_Decimal_YMCAPreTax
                                        l_DisbursementDetailsDataRow("TaxWithheldInterest") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldInterest"), Decimal) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100.0)), 2)

                                        l_TransactionDataRow("YmcaPreTax") = DirectCast(l_TransactionDataRow("YmcaPreTax"), Decimal) + (l_Decimal_YMCAPreTax * (-1.0))
                                        'Commented by Shubhrata YREN - 3025 Jan 19th 2007
                                        'l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                        'Added by Shubhrata YREN - 3025 Jan 19th 2007
                                        l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_YMCAPreTax
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100.0))
                                        l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100.0)), 2)
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate


                                        l_SelectedDataRow("Taxable") = DirectCast(l_SelectedDataRow("Taxable"), Decimal) - l_Decimal_YMCAPreTax
                                        l_AvailableBalance("YmcaPreTax") = "0.00"


                                    Else

                                        l_DisbursementDataRow("TaxableAmount") = DirectCast(l_DisbursementDataRow("TaxableAmount"), Decimal) + l_Decimal_SelectedTaxableAmount

                                        l_DisbursementDetailsDataRow("TaxablePrincipal") = DirectCast(l_DisbursementDetailsDataRow("TaxablePrincipal"), Decimal) + l_Decimal_SelectedTaxableAmount
                                        l_DisbursementDetailsDataRow("TaxWithheldPrincipal") = Math.Round(DirectCast(l_DisbursementDetailsDataRow("TaxWithheldPrincipal"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)

                                        l_TransactionDataRow("YmcaPreTax") = DirectCast(l_TransactionDataRow("YmcaPreTax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (-1.0))

                                        l_RefundsDataRow("Taxable") = DirectCast(l_RefundsDataRow("Taxable"), Decimal) + l_Decimal_SelectedTaxableAmount
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        'l_RefundsDataRow("Tax") = DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0))
                                        l_RefundsDataRow("Tax") = Math.Round(DirectCast(l_RefundsDataRow("Tax"), Decimal) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0)), 2)
                                        'Modified and added the math.round function to remove the rounding issue-Amit 01-10-2009
                                        l_RefundsDataRow("TaxRate") = l_Decimal_TaxRate

                                        l_AvailableBalance("YmcaPreTax") = l_Decimal_YMCAPreTax - l_Decimal_SelectedTaxableAmount
                                        l_SelectedDataRow("Taxable") = "0.00"

                                    End If
                                End If
                            End If

                        End If
                    End If ''IsVested 
                End If
            End If

        Catch ex As Exception
            Throw
        End Try


    End Function

#End Region 'Refund Interest
    '' This Function is used to do some modification to update into Database. 
    '' We already calculated values, now we needs to upodate in DisbursementWithHolding & DisbursementRefunds.

    Private Function SetTransactTypeForMarketWithdrawal()
        Dim l_requestedDatable As DataTable
        Dim l_TransactionsDataTable As DataTable
        Dim l_TransactDatarow As DataRow
        Dim l_TempString As String

        l_TransactionsDataTable = DirectCast(Session("R_Transactions_C19"), DataTable)
        l_requestedDatable = DirectCast(Session("AvailableBalance_C19"), DataTable)

        For Each l_TransactDatarow In l_TransactionsDataTable.Rows
            Dim l_stringAccttype As String = String.Empty

            l_stringAccttype = l_TransactDatarow("AcctType").ToString().Trim()
            If getBitMarketValue(l_stringAccttype, l_requestedDatable) = True Then
                If (l_TransactDatarow("TransactType") = "RFPR") Then
                    l_TransactDatarow.BeginEdit()
                    l_TransactDatarow("TransactType") = "MWPR"
                    l_TransactDatarow.EndEdit()

                End If
                If (l_TransactDatarow("TransactType") = "RFIN") Then
                    l_TransactDatarow.BeginEdit()
                    l_TransactDatarow("TransactType") = "MWIN"
                    l_TransactDatarow.EndEdit()
                End If
            End If
        Next

    End Function


    Private Function MakeTablesToUpdate(ByVal l_decimalTaxAmount As Decimal)

        Dim l_RefundsDataTable As DataTable
        Dim l_TransactionsDataTable As DataTable
        Dim l_DisbursementsDataTable As DataTable
        Dim l_DisbursementDetailsDataTable As DataTable
        Dim l_DisbursementWithholdingDataTable As DataTable
        Dim l_DisbursementRefundsDataTable As DataTable

        Dim l_Decimal_TaxableCounter As Decimal
        Dim l_Decimal_NonTaxableCounter As Decimal

        Dim l_DataRow As DataRow

        Dim l_FoundRow() As DataRow
        Dim l_QueryString As String

        Dim l_Decimal_TaxWithheldPrincipal As Decimal
        Dim l_Decimal_TaxWithheldInterest As Decimal
        Dim l_Decimal_TaxCounter As Decimal
        Dim l_WithholdingDataRow As DataRow
        Dim l_DisbursementRefundDataRow As DataRow


        Try

            l_RefundsDataTable = DirectCast(Session("R_Refunds_C19"), DataTable)
            l_TransactionsDataTable = DirectCast(Session("R_Transactions_C19"), DataTable)
            l_DisbursementsDataTable = DirectCast(Session("R_Disbursements_C19"), DataTable)
            l_DisbursementDetailsDataTable = DirectCast(Session("R_DisbursementDetails_C19"), DataTable)
            l_DisbursementWithholdingDataTable = DirectCast(Session("R_DisbursementWithholding_C19"), DataTable)
            l_DisbursementRefundsDataTable = DirectCast(Session("R_DisbursementRefunds_C19"), DataTable)


            '' Get Values to Taxable & NonTaxable Counter.

            If Not l_DisbursementsDataTable Is Nothing Then

                If l_DisbursementsDataTable.Rows.Count > 0 Then

                    l_Decimal_TaxableCounter = DirectCast(l_DisbursementsDataTable.Compute("SUM (TaxableAmount)", ""), Decimal)
                    l_Decimal_NonTaxableCounter = DirectCast(l_DisbursementsDataTable.Compute("SUM (NonTaxableAmount)", ""), Decimal)
                End If
            End If


            '' Now Update DisbursementWithHoldingDataTable.

            l_Decimal_TaxCounter = 0.0

            If Not l_DisbursementDetailsDataTable Is Nothing Then

                For Each l_DataRow In l_DisbursementDetailsDataTable.Rows

                    l_Decimal_TaxWithheldPrincipal = Math.Round(IIf(l_DataRow("TaxWithheldPrincipal").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_DataRow("TaxWithheldPrincipal"), Decimal)), 2)
                    l_Decimal_TaxWithheldInterest = Math.Round(IIf(l_DataRow("TaxWithheldInterest").GetType.ToString = "System.DBNull", 0.0, DirectCast(l_DataRow("TaxWithheldInterest"), Decimal)), 2)

                    If l_Decimal_TaxWithheldPrincipal > 0.0 Or l_Decimal_TaxWithheldInterest > 0.0 Then

                        ''We need to add into DisbursementWithHolding DataTable 

                        l_Decimal_TaxCounter = Math.Round(l_Decimal_TaxCounter, 2) + Math.Round(l_Decimal_TaxWithheldInterest, 2) + Math.Round(l_Decimal_TaxWithheldPrincipal, 2)

                        'Neeraj 04May2010:
                        l_QueryString = "DisbursementID = '" & l_DataRow("DisbursementID").ToString().Trim & "'"

                        l_FoundRow = l_DisbursementWithholdingDataTable.Select(l_QueryString)

                        l_WithholdingDataRow = Nothing

                        If Not l_FoundRow Is Nothing Then
                            If l_FoundRow.Length > 0 Then
                                l_WithholdingDataRow = l_FoundRow(0)
                            End If
                        End If


                        '' update the values.
                        If l_WithholdingDataRow Is Nothing Then

                            l_WithholdingDataRow = l_DisbursementWithholdingDataTable.NewRow

                            l_WithholdingDataRow("DisbursementID") = l_DataRow("DisbursementID")
                            l_WithholdingDataRow("WithholdingTypeCode") = "FEDTAX"
                            l_WithholdingDataRow("Amount") = l_Decimal_TaxWithheldInterest + l_Decimal_TaxWithheldPrincipal

                            l_DisbursementWithholdingDataTable.Rows.Add(l_WithholdingDataRow)

                        Else
                            l_WithholdingDataRow("Amount") = DirectCast(l_WithholdingDataRow("Amount"), Decimal) + l_Decimal_TaxWithheldInterest + l_Decimal_TaxWithheldPrincipal
                        End If
                    End If
                Next
            End If

            '' DisbursementDetails Table is Ready.
            '' Transaction Table is also ready


            '' Make DisbursementRefunds DataTable 
            If Not l_RefundsDataTable Is Nothing Then

                For Each l_DataRow In l_RefundsDataTable.Rows

                    l_QueryString = "DisbursementID = '" & l_DataRow("DisbursementID").ToString() & "'"

                    l_FoundRow = l_DisbursementRefundsDataTable.Select(l_QueryString)

                    l_DisbursementRefundDataRow = Nothing
                    If Not l_FoundRow Is Nothing Then
                        If l_FoundRow.Length > 0 Then
                            l_DisbursementRefundDataRow = l_FoundRow(0)
                        End If
                    End If

                    If l_DisbursementRefundDataRow Is Nothing Then

                        l_DisbursementRefundDataRow = l_DisbursementRefundsDataTable.NewRow

                        l_DisbursementRefundDataRow("RefRequestID") = l_DataRow("RefRequestsID")
                        l_DisbursementRefundDataRow("DisbursementID") = l_DataRow("DisbursementID")

                        l_DisbursementRefundsDataTable.Rows.Add(l_DisbursementRefundDataRow)
                    End If
                Next
            End If

            '' Now all DataTables are Ready to Update in the DataBase.

            '' Finaly we needs to Update the RefRequestTable. 

            '*********************************************************************************************************************
            '* Update the RefRequest record with the actual Refund Amounts
            '********************************************************************************************************************

            Dim l_RefundRequestDataTable As DataTable
            Dim l_RefunRequestDataRow As DataRow

            'Hafiz 03Feb06 Cache-Session
            'l_RefundRequestDataTable = DirectCast(l_CacheManager.GetData("RefundRequestTable"), DataTable)
            l_RefundRequestDataTable = DirectCast(Session("RefundRequestTable_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_RefundRequestDataTable Is Nothing Then

                l_QueryString = "UniqueID ='" & Me.SessionRefundRequestID.Trim & "'"

                l_FoundRow = l_RefundRequestDataTable.Select(l_QueryString)

                l_RefunRequestDataRow = Nothing

                If Not l_FoundRow Is Nothing Then
                    If l_FoundRow.Length > 0 Then
                        l_RefunRequestDataRow = l_FoundRow(0)
                    End If
                End If

                If Not l_RefunRequestDataRow Is Nothing Then

                    l_RefunRequestDataRow("Gross Amt.") = l_Decimal_TaxableCounter + l_Decimal_NonTaxableCounter
                    l_RefunRequestDataRow("RequestStatus") = "DISB"
                    l_RefunRequestDataRow("StatusDate") = Date.Now
                    l_RefunRequestDataRow("NonTaxable") = l_Decimal_NonTaxableCounter
                    l_RefunRequestDataRow("Taxable") = l_Decimal_TaxableCounter
                    'Added by Ganeswar on 26th Aug for Rounding Issue in NetAmount-10-09-2009
                    If Not Me.RefundType.Trim = "HARD" Then
                        If l_Decimal_TaxCounter = l_decimalTaxAmount Then
                            l_RefunRequestDataRow("Tax") = l_Decimal_TaxCounter
                        ElseIf l_Decimal_TaxCounter < l_decimalTaxAmount Then
                            l_RefunRequestDataRow("Tax") = l_Decimal_TaxCounter + 0.01
                        ElseIf l_Decimal_TaxCounter > l_decimalTaxAmount Then
                            l_RefunRequestDataRow("Tax") = l_Decimal_TaxCounter - 0.01
                        End If
                    Else
                        l_RefunRequestDataRow("Tax") = l_Decimal_TaxCounter
                    End If
                    'PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    If Me.RefundType.Trim = "SHIRA" Then
                        l_RefunRequestDataRow("RefundType") = "REG"
                        l_RefunRequestDataRow("RefundTypeHeader") = "REG"
                        Me.RefundType = "REG"
                    End If
                    'END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    'Added by Ganeswar on 26th Aug for Rounding Issue in NetAmount-10-09-2009
                    l_RefunRequestDataRow("Deductions") = Me.DeductionsAmount
                    If l_Decimal_TaxCounter <> 0 And l_Decimal_TaxableCounter <> 0 Then
                        l_RefunRequestDataRow("TaxRate") = (l_Decimal_TaxCounter * 100) / (l_Decimal_TaxableCounter)
                    Else
                        l_RefunRequestDataRow("TaxRate") = 0
                    End If

                    '* Check the RefRequest Status and see if during the Distrubution Process
                    '* The Refund Type was altered from a Regular Refund to a Personal Side Only Refund

                    If Me.IsPersonalOnly = True And Me.RefundType.Trim = "REG" Then
                        l_RefunRequestDataRow("RefundType") = "PERS"
                    End If
                End If
            End If

            '' Thats all.. Now fire to Database. 


        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function UpdateWithHoldingsWithDeductions() As String

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Dim l_Decimal_Deductions As Decimal
        Dim l_QueryString As String

        Dim l_DisbursementID As String
        Dim l_DisbursementIDForDeductions As String
        Dim l_PersonID As String

        Dim l_DisbursementsDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_DisbursementDataRow As DataRow
        Dim l_FoundRows() As DataRow

        Try
            l_DisbursementsDataTable = DirectCast(Session("R_Disbursements_C19"), DataTable)

            If Not l_DisbursementsDataTable Is Nothing Then

                If l_DisbursementsDataTable.Rows.Count > 0 Then
                    '' Getting DisbursementID and PersonID
                    l_DataRow = l_DisbursementsDataTable.Rows(0)

                    If Not l_DataRow Is Nothing Then

                        l_DisbursementID = IIf(l_DataRow("UniqueID").GetType.ToString = "System.DBNull", String.Empty, l_DataRow("UniqueID").ToString())
                        l_PersonID = IIf(l_DataRow("PersID").GetType.ToString = "System.DBNull", String.Empty, l_DataRow("PersID").ToString())

                    End If
                End If

                l_DisbursementIDForDeductions = String.Empty

                l_QueryString = "PayeeEntityTypeCode = 'PERSON' AND (TaxableAmount + NonTaxableAmount) > " & Me.DeductionsAmount.ToString.Trim & " AND PersID = '" & l_PersonID & "'"

                l_FoundRows = l_DisbursementsDataTable.Select(l_QueryString)

                l_DisbursementDataRow = Nothing

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        l_DisbursementDataRow = l_FoundRows(0)
                    End If
                End If

                '' If Row is Deducted then Add into Disbursement 

                If l_DisbursementDataRow Is Nothing Then

                    '' Search for LEGAL
                    l_QueryString = "PayeeEntityTypeCode = 'LEGAL' AND (TaxableAmount + NonTaxableAmount) > " & Me.DeductionsAmount.ToString.Trim & " AND PersID = '" & l_PersonID & "'"

                    l_FoundRows = l_DisbursementsDataTable.Select(l_QueryString)

                    l_DisbursementDataRow = Nothing

                    If Not l_FoundRows Is Nothing Then
                        If l_FoundRows.Length > 0 Then
                            l_DisbursementDataRow = l_FoundRows(0)
                        End If
                    End If

                    If l_DisbursementDataRow Is Nothing Then
                        '' Search in ROLIN

                        l_QueryString = "PayeeEntityTypeCode = 'ROLINS' AND (TaxableAmount + NonTaxableAmount) > " & Me.DeductionsAmount.ToString.Trim & " AND PersID = '" & l_PersonID & "'"

                        l_FoundRows = l_DisbursementsDataTable.Select(l_QueryString)

                        l_DisbursementDataRow = Nothing

                        If Not l_FoundRows Is Nothing Then
                            If l_FoundRows.Length > 0 Then
                                l_DisbursementDataRow = l_FoundRows(0)
                            End If
                        End If

                        If l_DisbursementDataRow Is Nothing Then
                            '' No Error.
                        Else
                            l_DisbursementIDForDeductions = l_DisbursementDataRow("UniqueID").ToString()
                        End If


                    Else
                        l_DisbursementIDForDeductions = l_DisbursementDataRow("UniqueID").ToString()
                    End If

                Else
                    l_DisbursementIDForDeductions = l_DisbursementDataRow("UniqueID").ToString()
                End If

                '' Update the WithHolding DataTable

                ''Return with Error.
                If l_DisbursementIDForDeductions = String.Empty Then Return ("Error: Unable to locate a Check that has enough to Cover the Deduction")


                '' Otherwise Collect the selected Deduction Amnt.


                Dim l_CheckBox As CheckBox

                Dim l_DisbursementWithHoldingDataTable As DataTable
                Dim l_WithHoldingDataRow As DataRow

                l_DisbursementWithHoldingDataTable = DirectCast(Me.DataTableGetWithholdingDeductions, DataTable)


                If Not l_DisbursementWithHoldingDataTable Is Nothing Then
                    'the withholding data table is already having the Type code and the amount, we only need to update the 
                    'disbursement id
                    For Each drow As DataRow In l_DisbursementWithHoldingDataTable.Rows
                        drow("DisbursementID") = l_DisbursementIDForDeductions
                    Next
                End If

                '' Deductions also Thru.. 

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    'Added new code  by Ganeswar For Adjust DisbursementWithholding Amount field value on 05-10-2009
    Private Function SaveAllTable(ByVal l_decimalTaxAmount As Decimal, ByVal l_decDeferedRollOverAmount As Decimal, ByVal l_Decimal_DeferedTaxablePercentage As Decimal, ByVal l_Decimal_DeferedNonTaxablePercentage As Decimal) As String
        'Private Function SaveAllTable() As String
        'Added new code  by Ganeswar For Adjust DisbursementWithholding Amount field value on 05-10-2009

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_stringProcessMessage As String = String.Empty

        Dim l_RefundsDataTable As DataTable
        Dim l_TransactionsDataTable As DataTable
        Dim l_DisbursementsDataTable As DataTable
        Dim l_DisbursementDetailsDataTable As DataTable
        Dim l_DisbursementWithholdingDataTable As DataTable
        Dim l_DisbursementRefundsDataTable As DataTable

        Dim l_RefundRequestDataTable As DataTable
        Dim l_UpdateAvailableBalance As DataTable
        Dim l_MarketDataTable As DataTable
        Dim l_dtMrdRecordsDisbursements As DataTable

        Dim l_DataSet As DataSet
        Dim l_PartialDataSet As DataSet
        Dim l_dr_PartialTransactionRow As DataRow
        Dim l_BooleanFlag As Boolean
        Dim l_ds_NewRequests As New DataSet
        Dim l_dr_PartialDataRow As DataRow

        'Declare the variable for TransactData
        Dim strUniqueId As String
        Dim strFundEventId As String
        Dim strYmcaID As String = Nothing
        Dim strPerssID As String
        Dim strTransactType As String
        Dim strAnnuityBasisType As String
        Dim decPersonalPreTax As Decimal
        Dim decPersonalPostTax As Decimal
        Dim decYmcaPreTax As Decimal
        Dim MonthlyComp As Decimal
        Dim dtAccountingDate As String
        Dim dtTransactDate As String
        Dim dtTrnsactionRefId As String
        Dim strCreated As String
        Dim strCreator As String
        Dim strUpdated As String
        Dim strUpdater As String
        '  START : SB | 08/31/2016 | YRS-AT-3028 | Declaring variables to hold values of Mrd Records 
        Dim dsMrdRecordsForWithdrawal As DataSet
        Dim dtMrdForWithdrawal As DataTable
        '  END : SB | 08/31/2016 | YRS-AT-3028 | Declaring variables to hold values of Mrd Records 
        Try
            PrepareDisbursementDetailsAfterTaxes()
            l_DisbursementsDataTable = DirectCast(Session("R_Disbursements_C19"), DataTable)
            l_DisbursementDetailsDataTable = DirectCast(Session("R_DisbursementDetails_C19"), DataTable)
            l_RefundsDataTable = DirectCast(Session("R_Refunds_C19"), DataTable)
            l_DisbursementWithholdingDataTable = DirectCast(Session("R_DisbursementWithholding_C19"), DataTable)
            l_DisbursementRefundsDataTable = DirectCast(Session("R_DisbursementRefunds_C19"), DataTable)
            l_RefundRequestDataTable = DirectCast(Session("RefundRequestTable_C19"), DataTable)
            l_TransactionsDataTable = DirectCast(Session("R_Transactions_C19"), DataTable)
            l_dtMrdRecordsDisbursements = DirectCast(Session("R_MrdRecordsDisbursements_C19"), DataTable)

            l_UpdateAvailableBalance = Session("ArrayRefundableDataTable_C19")
            If Me.ISMarket = -1 Then
                l_MarketDataTable = Session("MarketDataTable_C19")
            End If
            '  START : SB | 08/31/2016 | YRS-AT-3028 | Reteriving the values from session variables to local variables  
            dsMrdRecordsForWithdrawal = ReCalculatedRMDValues
            If HelperFunctions.isNonEmpty(dsMrdRecordsForWithdrawal) AndAlso HelperFunctions.isNonEmpty(dsMrdRecordsForWithdrawal.Tables(1)) Then
                dtMrdForWithdrawal = dsMrdRecordsForWithdrawal.Tables(1)
            End If    ' 
            '  END : SB | 08/31/2016 | YRS-AT-3028 | Reteriving the values from session variables to local variables  
            'below code commented by neeraj 21-Oct-2009 issue id 982: as it was overriding the deduction amount for non fedtax data.
            'Added new code  by Ganeswar For Adjust DisbursementWithholding Amount field value on 05-10-2009
            'If l_DisbursementWithholdingDataTable.Rows.Count > 0 Then
            '    Dim l_dr_NewRowDisbursementWithholding As DataRow
            '    If l_DisbursementWithholdingDataTable.Rows(0).Item("Amount") <> l_decimalTaxAmount Then
            '        l_dr_NewRowDisbursementWithholding = l_DisbursementWithholdingDataTable.Rows(0)
            '        l_dr_NewRowDisbursementWithholding.BeginEdit()
            '        l_dr_NewRowDisbursementWithholding("Amount") = l_decimalTaxAmount
            '        l_dr_NewRowDisbursementWithholding.EndEdit()
            '    End If
            'End If

            If l_DisbursementWithholdingDataTable.Rows.Count > 0 Then
                Dim l_dr_NewRowDisbursementWithholding As DataRow
                For Each dr As DataRow In l_DisbursementWithholdingDataTable.Rows
                    If DirectCast(dr("WithholdingTypeCode"), String) = "FEDTAX" And DirectCast(dr("Amount"), Decimal) <> l_decimalTaxAmount Then
                        dr.BeginEdit()
                        dr("Amount") = l_decimalTaxAmount
                        dr.EndEdit()
                    End If
                Next
            End If
            ' End *** Neeraj 21-Oct-2009

            'Added new code  by Ganeswar For Adjust DisbursementWithholding Amount field value on 05-10-2009

            'Added new code  by Ganeswar For Adjust RefundsDataTable Tax field value on 05-10-2009

            'l_RefundsDataTable = AdjustRoundingValue(l_RefundsDataTable, l_decimalTaxAmount)
            If l_RefundsDataTable.Rows.Count > 0 Then
                Dim l_RefundsTotal As Decimal
                Dim l_RoundingTotal As Decimal
                Dim l_dr_NewRowRefundsDataTable As DataRow

                l_RefundsTotal = l_RefundsDataTable.Compute("SUM (Tax)", "")
                l_RefundsTotal = Math.Round((l_RefundsTotal), 2)
                If l_RefundsTotal <> l_decimalTaxAmount Then
                    l_RoundingTotal = l_decimalTaxAmount - l_RefundsTotal
                    l_dr_NewRowRefundsDataTable = l_RefundsDataTable.Rows(0)
                    l_dr_NewRowRefundsDataTable.BeginEdit()
                    If l_dr_NewRowRefundsDataTable("Tax") > l_RoundingTotal Then
                        l_dr_NewRowRefundsDataTable("Tax") = l_dr_NewRowRefundsDataTable("Tax") + l_RoundingTotal
                    End If
                    l_dr_NewRowRefundsDataTable.EndEdit()
                End If
            End If
            'Added new code  by Ganeswar For Adjust RefundsDataTable Tax field value on 05-10-2009

            'Added new code  by Ganeswar For Adjust DisbursementDetails Tax field value on 05-10-2009
            If l_DisbursementDetailsDataTable.Rows.Count > 0 Then
                Dim l_RefundsTotal As Decimal
                Dim l_RoundingPrincipalTotal As Decimal
                Dim l_RoundingInterestTotal As Decimal
                Dim l_RoundingTotal As Decimal
                Dim l_dr_NewRowDisbursementDetails As DataRow
                Dim l_TaxWithheldAmount As Decimal
                l_RoundingPrincipalTotal = l_DisbursementDetailsDataTable.Compute("SUM (TaxWithheldPrincipal)", "")
                l_RoundingInterestTotal = l_DisbursementDetailsDataTable.Compute("SUM (TaxWithheldInterest)", "")

                l_RefundsTotal = l_RoundingPrincipalTotal + l_RoundingInterestTotal

                l_RefundsTotal = Math.Round((l_RefundsTotal), 2)
                If l_RefundsTotal <> l_decimalTaxAmount Then
                    l_RoundingTotal = l_decimalTaxAmount - l_RefundsTotal
                    For i As Integer = 0 To l_DisbursementDetailsDataTable.Rows.Count - 1
                        l_dr_NewRowDisbursementDetails = l_DisbursementDetailsDataTable.Rows(i)
                        'neeraj 19-11-2010 BT-664 : to solve rounding for issue 1132 reopened issue
                        l_TaxWithheldAmount = CType(l_dr_NewRowDisbursementDetails("TaxWithheldPrincipal"), Decimal)

                        'If l_dr_NewRowDisbursementDetails("TaxWithheldPrincipal") > l_RoundingTotal Then
                        If l_TaxWithheldAmount > l_RoundingTotal AndAlso l_TaxWithheldAmount + l_RoundingTotal > 0 Then
                            l_dr_NewRowDisbursementDetails.BeginEdit()
                            l_dr_NewRowDisbursementDetails("TaxWithheldPrincipal") = l_TaxWithheldAmount + l_RoundingTotal
                            l_dr_NewRowDisbursementDetails.EndEdit()
                            Exit For
                        End If

                        l_TaxWithheldAmount = CType(l_dr_NewRowDisbursementDetails("TaxWithheldPrincipal"), Decimal)

                        'If l_dr_NewRowDisbursementDetails("TaxWithheldInterest") > l_RoundingTotal Then
                        If l_TaxWithheldAmount > l_RoundingTotal AndAlso l_TaxWithheldAmount + l_RoundingTotal > 0 Then
                            l_dr_NewRowDisbursementDetails.BeginEdit()
                            l_dr_NewRowDisbursementDetails("TaxWithheldInterest") = l_TaxWithheldAmount + l_RoundingTotal
                            l_dr_NewRowDisbursementDetails.EndEdit()
                            Exit For
                        End If
                        'End Neeraj change 19-11-2010
                    Next
                End If
            End If
            'Added new code  by Ganeswar For Adjust DisbursementDetails Tax field value on 05-10-2009

            'Added new code by Ganeswar For HardShip RollOver on 25-05-2009
            If Me.RefundType = "VOL" And Me.IsHardShip = False Then
                Dim l_dr_NewRowRefundRequestTable As DataRow
                If l_RefundRequestDataTable.Rows.Count > 0 Then
                    For intcnt As Integer = 0 To l_RefundRequestDataTable.Rows.Count - 1
                        l_dr_NewRowRefundRequestTable = l_RefundRequestDataTable.Rows(intcnt)
                        l_dr_NewRowRefundRequestTable.BeginEdit()
                        l_dr_NewRowRefundRequestTable("RefundType") = Me.RefundType
                        l_dr_NewRowRefundRequestTable.EndEdit()
                    Next
                End If
            End If
            'Added new code by Ganeswar For HardShip RollOver on 25-05-2009

            'Added new code by Parveen For Updating Rolloveroptions and amount on 15-12-2009
            If l_RefundRequestDataTable.Rows.Count > 0 Then
                l_RefundRequestDataTable.Columns.Add("RolloverOptions")
                l_RefundRequestDataTable.Columns.Add("FirstRolloverAmt")
                l_RefundRequestDataTable.Columns.Add("TotalRolloverAmt")
                If Me.RolloverOptions.Length <> 0 Then
                    l_RefundRequestDataTable.Rows(0)("RolloverOptions") = Me.RolloverOptions
                End If
                If Me.RolloverOptions = m_const_PartialRollOver Then
                    l_RefundRequestDataTable.Rows(0)("FirstRolloverAmt") = Me.FirstRolloverAmt
                    l_RefundRequestDataTable.Rows(0)("TotalRolloverAmt") = Me.TotalRolloverAmt
                End If
                If Me.RolloverOptions = m_const_AllTaxable Then
                    l_RefundRequestDataTable.Rows(0)("FirstRolloverAmt") = Me.FirstRolloverAmt
                End If
            End If
            'Added new code by Parveen For Updating Rolloveroptions and amount on 15-12-2009


            '' Create a New DataSet and Push the DataTable 
            l_DataSet = New DataSet("RefundTables")

            l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementsDataTable, "DisbursementsDataTable"))
            l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementDetailsDataTable, "DisbursementDetailsDataTable"))
            l_DataSet.Tables.Add(Me.CopyTable(l_TransactionsDataTable, "TransactionsDataTable"))
            l_DataSet.Tables.Add(Me.CopyTable(l_RefundsDataTable, "RefundsDataTable"))
            l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementWithholdingDataTable, "DisbursementWithholdingDataTable"))
            l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementRefundsDataTable, "DisbursementRefundsDataTable"))
            l_DataSet.Tables.Add(Me.CopyTable(l_RefundRequestDataTable, "RefundRequestDataTable"))
            'neeraj 17-11-2010 Bt-664 : added dataTable MrdRecordsDisbursements in final dataset to be saved in DB
            l_DataSet.Tables.Add(Me.CopyTable(l_dtMrdRecordsDisbursements, "MrdRecordsDisbursements"))

            If Me.ISMarket = -1 Then
                l_DataSet.Tables.Add(Me.CopyTable(l_MarketDataTable, "MarketDataTable"))
            End If
            If HelperFunctions.isNonEmpty(dtMrdForWithdrawal) Then
                l_DataSet.Tables.Add(Me.CopyTable(dtMrdForWithdrawal, "AtsMrdRecordsWithDrawalUpdation")) ' SB | 08/31/2016 | YRS-AT-3028 | Mrd Record table whose values need to be updated are  copied into Dataset 
            End If
            '' Save the DataTables.
            'Start- Modified for Plan Split Changes 
            If Not Session("RefundsDataSetForNewRequests_C19") Is Nothing Then

                l_ds_NewRequests = DirectCast(Session("RefundsDataSetForNewRequests_C19"), DataSet)
            End If
            l_BooleanFlag = YMCARET.YmcaBusinessObject.RefundRequest_C19.SaveRefundRequestProcess(l_DataSet, Me.PersonID, Me.FundID, Me.RefundType, Me.IsVested, Me.IsTerminated, Me.IsTookTDAccount, Me.SessionStatusType, l_ds_NewRequests, Session("NeedsNewRequests_C19"), Session("Initial_RefRequestId_C19"), IIf(Me.ISMarket = -1, True, False), Me.TaxRate, Me.Payee1Name, Me.Payee2ID, l_decDeferedRollOverAmount, Me.ISMarket, l_Decimal_DeferedTaxablePercentage, l_Decimal_DeferedNonTaxablePercentage, IRSOverride) 'SR | 2016.09.23 | YRS-AT-3164 - Send IRSOverride flag.
            'End - Modified for Plan Split Changes

            If l_BooleanFlag = True Then
                l_stringProcessMessage = "Requested refund is Processed Successfully."

                'Set the property value to nothing to initialize the session value after save-Amit 08-oct-2009
                Me.IsHardShip = Nothing
                'Set the property value to nothing to initialize the session value after save-Amit 08-oct-2009
                'START | SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NP/PENP and RDNP participants which are terminated
                If Me.RefundType = "HARD" AndAlso Not IRSOverride AndAlso Not IsNonParticipatingPerson() Then
                    'START | SR | 2019.07.23 | YRS-AT-4498 | If TD contribution is allowed as per hardship configuration then do not send mail to LPA.
                    If (Not IsTDContributionsAllowed) Then
                        ProcessSendMail()
                    End If
                    'END | SR 2019.07.23 | YRS-AT-4498 |If TD contribution is allowed as per hardship configuration then do not send mail to LPA.
                    'If (RefundType = "HARD" AndAlso IRSOverride = False) Then 'SR | 2016.09.23 | YRS-AT-3164 - If IRSOverRide flag is True then do not send mail to LPA of Louisiana YMCA 
                    'END | SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NP/PENP and RDNP participants which are terminated
                    '21-Nov-2008 :Priya BugId - YRS 5.0-576 Apply functionality to send email after hardship withdrawal.

                    'End 21-Nov-2008
                End If
            Else
                l_stringProcessMessage = "Error: Requested refund is not processed due to some failures in SQL Database."
            End If

            SaveAllTable = l_stringProcessMessage

        Catch ex As Exception
            Throw
        End Try
    End Function

    ''Added new code for by Ganeswar For Adjust RefundsDataTable Tax field value on 05-10-2009
    'Private Function AdjustRoundingValue(ByVal l_RefundsDataTable As DataTable, ByVal l_decimalTaxAmount As Decimal)
    '    Try

    '        Return l_RefundsDataTable
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Added new code for by Ganeswar For Adjust RefundsDataTable Tax field value on 05-10-2009

    '21-Nov-2008 :Priya BugId - YRS 5.0-576 Apply functionality to send email after hardship withdrawal.
    Private Function ProcessSendMail()
        Dim dsDetailsEmailAddress As DataSet
        Dim int_LPA As Integer = 1
        Dim int_CFO As Integer = 2
        Dim int_CEO As Integer = 3
        Dim string_YMCANO As String
        Dim string_LPA_EmailAdd As String
        Dim string_CFO_EmailAdd As String
        Dim string_CEO_EmailAdd As String
        Dim string_errorMessage As String
        Try
            '09-Jan-2009 :Priya BugId - YRS 5.0-576 check for YMCA number.
            Dim dtYMCAInformation As DataTable
            dtYMCAInformation = Session("Member Employment_C19")
            If isNonEmpty(dtYMCAInformation) Then
                'Priya 09-Feb-09 : YRS 5.0-695 Email to YMCA after Hardship withdrawal
                Dim drYmcasNo As DataRow()
                drYmcasNo = dtYMCAInformation.Select("StatusType = 'A'")
                If (isNonEmpty(drYmcasNo) AndAlso drYmcasNo.Length > 0) Then
                    Dim i As Integer
                    For i = 0 To drYmcasNo.Length - 1
                        string_YMCANO = Convert.ToString(drYmcasNo(i)("YMCANo")).Trim

                        If string_YMCANO = String.Empty Then
                            Throw New Exception("Participant is not actively employed at any YMCA, therefore no mail sent.")
                            Exit Function
                        End If


                        string_LPA_EmailAdd = ""
                        string_CFO_EmailAdd = ""
                        string_CEO_EmailAdd = ""

                        'Implemented YRS 5.0-576 code into loop 
                        dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_LPA)
                        'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                        If isNonEmpty(dsDetailsEmailAddress) Then
                            If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                string_LPA_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                            Else
                                dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_CFO)
                                'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                                If isNonEmpty(dsDetailsEmailAddress) Then
                                    If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                        string_CFO_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                                    Else
                                        dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_CEO)
                                        'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                                        If isNonEmpty(dsDetailsEmailAddress) Then
                                            If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                                string_CEO_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                                            End If
                                        End If
                                    End If
                                Else
                                    dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_CEO)
                                    'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                                    If isNonEmpty(dsDetailsEmailAddress) Then
                                        If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                            string_CEO_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                                        End If
                                    End If

                                End If
                            End If

                        Else
                            dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_CFO)
                            'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                            If isNonEmpty(dsDetailsEmailAddress) Then
                                If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                    string_CFO_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                                Else
                                    dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_CEO)
                                    'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                                    If isNonEmpty(dsDetailsEmailAddress) Then
                                        If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                            string_CEO_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                                        End If
                                    End If
                                End If

                            Else
                                dsDetailsEmailAddress = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANO, int_CEO)
                                'If (dsDetailsEmailAddress.Tables(0).Rows.Count > 0) Then
                                If isNonEmpty(dsDetailsEmailAddress) Then
                                    If Not IsDBNull(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")) Then
                                        string_CEO_EmailAdd = Convert.ToString(dsDetailsEmailAddress.Tables(0).Rows(0)("EmailAddr")).Trim
                                    End If
                                End If

                            End If
                        End If

                        If (string_LPA_EmailAdd <> String.Empty) Then
                            SendMail(string_LPA_EmailAdd)
                        ElseIf (string_CFO_EmailAdd <> String.Empty) Then
                            SendMail(string_CFO_EmailAdd)
                        Else
                            SendMail(string_CEO_EmailAdd)
                        End If
                    Next
                Else
                    Throw New Exception("Participant is not actively employed at any YMCA, therefore no mail sent.")
                    Exit Function
                End If
                'End 09-Feb-09 YRS 5.0-695 

            Else
                Exit Function
                'End If
            End If
            '09-Jan-2009 :Priya BugId - YRS 5.0-576 check for YMCA number.

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function SendMail(ByVal string_MailID)
        Dim string_Message As String
        Dim string_MailMessage As String

        Dim string_ParticipantName As String
        Dim string_SmartAccountRestart As String
        Dim strPreventTDContriFrom As String
        Dim strPreventTDContriTo As String
        Dim dtLockingPeriod As DataTable
        Dim dictParameters As Dictionary(Of String, String) 'MMR | 2017.11.29 | YRS-AT-3415 | Delcared object variable for dynamic email content 
        Dim obj As MailUtil
        obj = New MailUtil
        Try
            '09-Jan-2009 :Priya BugId - YRS 5.0-576 check for participant name.
            Dim dsPersonInformation As DataSet
            'If isNonEmpty(Session("PersonInformation_C19")) Then
            dsPersonInformation = Session("PersonInformation_C19")

            'SR:2013.10.28 : BT-2247/YRS 5.0-2229: Get TD Locking period for person
            dtLockingPeriod = YMCARET.YmcaBusinessObject.RefundRequest.GetTDLockingPeriod(Session("RefundRequestID_C19"))

            If HelperFunctions.isNonEmpty(dtLockingPeriod) Then
                strPreventTDContriFrom = dtLockingPeriod.Rows(0).Item(0).ToString()
                strPreventTDContriTo = dtLockingPeriod.Rows(0).Item(1).ToString()
            End If
            'End, SR:2013.10.28 : BT-2247/YRS 5.0-2229: Get TD Locking period for person


            If HelperFunctions.isNonEmpty(dsPersonInformation) Then 'MMR | 2017.11.29 | YRS-AT-3415 | Used common method instead of local method 'isNonEmpty' to check empty object value
                'If (Session("PersonInformation_C19").Tables(0).Rows.Count > 0) Then
                'With Session("PersonInformation_C19").Tables(0).Rows(0)
                With dsPersonInformation.Tables(0).Rows(0)
                    If HelperFunctions.isNonEmpty(.Item("First Name")) Then 'MMR | 2017.11.29 | YRS-AT-3415 | Used common method instead of local method 'isNonEmpty' to check empty object value
                        string_ParticipantName = Convert.ToString(.Item("First Name")).Trim + " "
                    End If

                    If HelperFunctions.isNonEmpty(.Item("Middle Name")) Then 'MMR | 2017.11.29 | YRS-AT-3415 | Used common method instead of local method 'isNonEmpty' to check empty object value
                        string_ParticipantName = string_ParticipantName + Convert.ToString(.Item("Middle Name")).Trim + " "
                    End If

                    If HelperFunctions.isNonEmpty(.Item("Last Name")) Then 'MMR | 2017.11.29 | YRS-AT-3415 | Used common method instead of local method 'isNonEmpty' to check empty object value
                        string_ParticipantName = string_ParticipantName + Convert.ToString(.Item("Last Name")).Trim
                    End If
                End With
                'string_ParticipantName = Session("PersonInformation_C19").Tables(0).Rows(0)(2) + " " + Session("PersonInformation_C19").Tables(0).Rows(0)(3) + " " + Session("PersonInformation_C19").Tables(0).Rows(0)(1)
                ' End If
                'End 09-Jan-2009 :BugId - YRS 5.0-576
            Else
                Exit Function
            End If
            'Priya 22-Jan-2009 : YRS 5.0-666 get harship renewal month from atsMetaConfiguration DB table.
            'START: MMR | 2017.11.29 | YRS-AT-3415 | Commented as not used anywhere
            'Dim ds_HardshipRenewalMonths As DataSet
            'ds_HardshipRenewalMonths = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.getConfigurationValue("HARDSHIP_RENEWAL_MONTHS")
            'Dim HardshipRenewalMonths As Integer

            'If isNonEmpty(ds_HardshipRenewalMonths) AndAlso Not IsDBNull(ds_HardshipRenewalMonths.Tables(0).Rows(0)("Value")) AndAlso Convert.ToString(ds_HardshipRenewalMonths.Tables(0).Rows(0)("Value")).Trim <> "" Then
            '    HardshipRenewalMonths = Convert.ToInt32(Convert.ToString(ds_HardshipRenewalMonths.Tables(0).Rows(0)("Value")).Trim)
            'Else
            '    HardshipRenewalMonths = 6
            '    'Throw New Exception("Hardship renewal month is not defined.")
            'End If

            'string_SmartAccountRestart = System.DateTime.Today.Date.AddMonths(HardshipRenewalMonths).ToShortDateString()
            'END: MMR | 2017.11.29 | YRS-AT-3415 | Commented as not used anywhere
            'string_SmartAccountRestart = System.DateTime.Today.Date.AddMonths(6).AddDays(1).ToShortDateString()
            'END 22-Jan-2009 
            'Priya 16-March-2009 Change Time of Customer Service Department 8:45 AM to 6:00 previously it was 8:45 AM to 8:00 as per Hafiz mail on March 13, 2009

            'Priya 17-March-2009 Made changes into MailMessage, message selected from database as per hafiz mail on 16-March-2009
            'START: MMR | 2017.11.29 | YRS-AT-3415 | Commented as mail will be template driven and not hardcoded
            'Dim strMailLastParagraphMsg As String
            'strMailLastParagraphMsg = YMCARET.YmcaBusinessObject.MailBOClass.GetMailLastParagraph().Trim()
            'END: MMR | 2017.11.29 | YRS-AT-3415 | Commented as mail will be template driven and not hardcoded
            'START : Commented and new line Added By : Dilip Yadav : 11-Aug-2009 : To Resolve Gemini Issue ID :YRS 5.0-859
            'string_MailMessage = "Hardship withdrawals are subject to federal regulations. When a participant takes a hardship withdrawal from their 403(b) Smart Account, they cannot make contributions to either the After-tax Account or 403(b) Smart Account for a period of 6 months following the withdrawal. " & vbCrLf & vbCrLf & _
            '                      string_ParticipantName & " has taken a hardship withdrawal. Please adjust the processing of your payroll and do not take payroll deductions for voluntary accounts. Contributions to an After-tax Account and/or a 403(b) Smart Account may restart on " & string_SmartAccountRestart & ".  Payments to all other accounts are not affected and must continue to be submitted." & vbCrLf & vbCrLf & strMailLastParagraphMsg
            ''2011.Apr.19        Sanket      BT:808 -YRS 5.0-1304 : Hardship withdrawal email- verbiage change
            'string_MailMessage = "Hardship withdrawals are subject to federal regulations. When a participant takes a hardship withdrawal from their 403(b) Smart Account, they cannot make contributions to either the After-tax Account or 403(b) Smart Account for a period of 6 months following the withdrawal. " & vbCrLf & vbCrLf & _
            'string_ParticipantName & " has taken a hardship withdrawal. Please adjust the processing of your payroll and do not take payroll deductions for voluntary accounts. Contributions to an After-tax Account and/or a 403(b) Smart Account may restart on " & string_SmartAccountRestart & ".  Payments to all other accounts are not affected and must continue to be submitted." & vbCrLf & vbCrLf & strMailLastParagraphMsg


            'SR:2013.10.28 : BT-2247/YRS 5.0-2229: Commented below line of code and added new content of mail given by Ed(attachecd in BT)
            'string_MailMessage = "Hardship withdrawals are subject to federal regulations. When a participant takes a hardship withdrawal from their 403(b) Smart Account, they cannot make contributions to that account for a period of 6 months following the withdrawal. " & vbCrLf & vbCrLf & _
            '                      string_ParticipantName & " has taken a hardship withdrawal. Please adjust the processing of your payroll and do not take payroll deductions for the 403(b) Smart Account. Contributions to the 403(b) Smart Account may restart on " & string_SmartAccountRestart & ".  Payments to all other accounts are not affected and must continue to be submitted." & vbCrLf & vbCrLf & strMailLastParagraphMsg

            'START: MMR | 2017.11.29 | YRS-AT-3415 | Commented as mail will be template driven and not hardcoded
            'string_MailMessage = "Hardship withdrawals are subject to federal regulations. When a participant takes a hardship withdrawal from their 403(b) Smart Account, they cannot make contributions to that account for a period of 6 months following the withdrawal. " & vbCrLf & vbCrLf & _
            '                      string_ParticipantName & " has taken a hardship withdrawal. Please adjust the processing of your payroll to stop deductions for the 403(b) Smart Account. The final contribution that can be made is for pay date " & strPreventTDContriFrom & ". Contributions to the 403(b) Smart Account may restart on " & strPreventTDContriTo & ".  Payments to all other accounts are not affected and must continue to be submitted. If the participant has an outstanding loan, loan payments must continue." & vbCrLf & vbCrLf & strMailLastParagraphMsg
            'END: MMR | 2017.11.29 | YRS-AT-3415 | Commented as mail will be template driven and not hardcoded
            'End, SR:2013.10.28 : BT-2247/YRS 5.0-2229: Commented below line of code and added new content of mail given by Ed(attachecd in BT)


            'END: Commented and new line Added By : Dilip Yadav : 11-Aug-2009 : To Resolve Gemini Issue ID :YRS 5.0-859

            'string_MailMessage = "Hardship withdrawals are subject to federal regulations. When a participant takes a hardship withdrawal from their 403(b) Smart Account, they cannot make contributions to either the After-tax Account or 403(b) Smart Account for a period of 6 months following the withdrawal. " & vbCrLf & vbCrLf & _
            '                    string_ParticipantName & " has taken a hardship withdrawal. Please adjust the processing of your payroll and do not take payroll deductions for voluntary accounts. Contributions to an After-tax Account and/or a 403(b) Smart Account may restart on " & string_SmartAccountRestart & ".  Payments to all other accounts are not affected and must continue to be submitted." & vbCrLf & vbCrLf & _
            '                    "If you have any questions or need additional information, please feel free to contact our Customer Service Department at 800-RET-YMCA, Monday through Friday, 8:45 AM - 6:00 PM Eastern Time. "
            'End 17-March-2009

            '10.01.2012:Dinesh:BT:1135:YRS 5.0-1659:New Configuration value for hardship withdrawal email sender
            'obj.MailCategory = "ADMIN"
            'START: MMR | 2017.11.29 | YRS-AT-3415 | Commented as mail will be template driven and not hardcoded
            'obj.MailCategory = "HARD"
            'If obj.MailService = False Then Exit Function

            'If string_MailID = String.Empty Then
            '    obj.ToMail = obj.FromMail
            'Else
            '    obj.ToMail = string_MailID
            'End If

            'obj.MailMessage = string_MailMessage
            'obj.Subject = "Hardship Withdrawal"
            'obj.Send()
            'END: MMR | 2017.11.29 | YRS-AT-3415 | Commented as mail will be template driven and not hardcoded
            'START: MMR | 2017.11.29 | YRS-AT-3415 | YRS REPORTS enh: update to hardship letter (TrackIT 29702)
            'Added parameters for email content
            dictParameters = New Dictionary(Of String, String)
            dictParameters.Add("PersonName", string_ParticipantName)
            dictParameters.Add("SmartAccountStopDate", strPreventTDContriFrom)
            dictParameters.Add("SmartAccountRestartDate", strPreventTDContriTo)
            'Sending mail to LPA's using mail utility tool
            If obj IsNot Nothing Then
                obj.SendMailMessage(EnumEmailTemplateTypes.HARDSHIP_TD_TERMN_INTIMATION, "", string_MailID, "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
            End If
            'END: MMR | 2017.11.29 | YRS-AT-3415 | YRS REPORTS enh: update to hardship letter (TrackIT 29702) 
        Catch ex As Exception
            Throw
        End Try

    End Function
    'End 21-Nov-2008

    Public Function CopyTable(ByVal parameterDataTable As DataTable, ByVal parameterDataTableName As String) As DataTable

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try
            If parameterDataTable Is Nothing Then Return Nothing

            l_DataTable = parameterDataTable.Clone

            If Not parameterDataTableName.Trim = String.Empty Then
                l_DataTable.TableName = parameterDataTableName
            End If

            For Each l_DataRow In parameterDataTable.Rows
                l_DataTable.ImportRow(l_DataRow)
            Next

            Return l_DataTable

        Catch ex As Exception
            Throw
        End Try

    End Function
    'This Function will set the Withholding amount and type code in the properties so that it can be used the 
    'Refunds.vb class
    Private Function SetWithholdingDeductionProperties(ByVal parameterDataGridDeductions As DataGrid)
        Dim l_CheckBox As CheckBox
        Dim dt_WithholdingDeductionProperties As DataTable
        Dim l_WithHoldingDataRow As DataRow
        Try
            dt_WithholdingDeductionProperties = DirectCast(Session("R_DisbursementWithholding_C19"), DataTable)
            For Each l_DataGridItem As DataGridItem In parameterDataGridDeductions.Items

                l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")

                If Not l_CheckBox Is Nothing Then

                    If l_CheckBox.Checked = True Then

                        If l_DataGridItem.Cells.Item(3).Text.Trim = String.Empty Then
                        Else

                            l_WithHoldingDataRow = dt_WithholdingDeductionProperties.NewRow

                            'neeraj 07May2010:
                            'l_WithHoldingDataRow("DisbursementID") = ""
                            l_WithHoldingDataRow("WithholdingTypeCode") = l_DataGridItem.Cells.Item(1).Text.Trim
                            l_WithHoldingDataRow("Amount") = CType(l_DataGridItem.Cells.Item(3).Text.Trim, Decimal)

                            dt_WithholdingDeductionProperties.Rows.Add(l_WithHoldingDataRow)

                        End If

                    End If
                End If
            Next
            dt_WithholdingDeductionProperties.AcceptChanges()
            Me.DataTableGetWithholdingDeductions = dt_WithholdingDeductionProperties
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region " Other Refund Functions "

    Private Function IsBasicAccount(ByVal parameterDataRow As DataRow) As Boolean

        Try

            If parameterDataRow("IsBasicAccount").GetType.ToString() = "System.DBNull" Then Return False

            If CType(parameterDataRow("IsBasicAccount"), Boolean) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function GetAccountBreakDownSortOrder(ByVal paramterAccountType As String) As Integer

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_AccountType As String

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = DirectCast(l_CacheManager.GetData("AccountBreakDown"), DataTable)
            l_DataTable = DirectCast(Session("AccountBreakDown_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    l_AccountType = DirectCast(l_DataRow("chrAcctBreakDownType"), String)

                    If l_AccountType <> "" Then
                        If l_AccountType.Trim.ToUpper = paramterAccountType.Trim.ToUpper Then
                            Return (DirectCast(l_DataRow("intSortOrder"), Integer))
                        End If
                    End If
                Next

            Else
                Return 0
            End If


        Catch ex As Exception
            Throw
        End Try


    End Function
    Public Function GetAccountBreakDownType(ByVal parameterAccountGroup As String, ByVal parameterPersonal As Boolean, ByVal parameterYMCA As Boolean, ByVal parameterPreTax As Boolean, ByVal parameterPostTax As Boolean) As String

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_FoundRows As DataRow()
        Dim l_QueryString As String

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = DirectCast(l_CacheManager.GetData("AccountBreakDown"), DataTable)
            l_DataTable = DirectCast(Session("AccountBreakDown_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then

                'l_QueryString = "chrAcctType = '" & parameterAccountType.Trim & "' "
                l_QueryString = "chvAcctGroups = '" & parameterAccountGroup.Trim & "' "

                If parameterPersonal = True Then
                    l_QueryString &= " AND bitPersonal = 1 "
                End If

                If parameterYMCA = True Then
                    l_QueryString &= " AND bitYmca = 1 "
                End If

                If parameterPreTax = True Then
                    l_QueryString &= " AND bitPreTax = 1 "
                End If

                If parameterPostTax = True Then
                    l_QueryString &= " AND bitPostTax = 1 "
                End If


                'Select Case parameterNoOfParam
                '    Case 2

                '        l_QueryString &= " AND bitPersonal = " & parameterPersonal
                '        l_QueryString &= " AND bitYmca = " & parameterYMCA

                '    Case 3

                '        l_QueryString &= " AND bitPersonal = " & parameterPersonal
                '        l_QueryString &= " AND bitYmca = " & parameterYMCA
                '        l_QueryString &= " AND bitPreTax = " & parameterPreTax

                '    Case 4
                '        l_QueryString &= " AND bitPersonal = " & parameterPersonal
                '        l_QueryString &= " AND bitYmca = " & parameterYMCA
                '        l_QueryString &= " AND bitPreTax = " & parameterPreTax
                '        l_QueryString &= " AND bitPostTax = " & parameterPostTax

                'End Select

                l_FoundRows = l_DataTable.Select(l_QueryString)

                If l_FoundRows.Length > 0 Then
                    l_DataRow = l_FoundRows(0)

                    Return (DirectCast(l_DataRow("chrAcctBreakDownType"), String))
                Else
                    Return String.Empty
                End If

            Else
                Return String.Empty
            End If


        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetDataRow(ByVal parameterAccountBreakDownType As String, ByVal parameterSortOrder As Integer, ByVal parameterDataTable As DataTable, ByVal parameterAcctType As String) As DataRow

        Dim l_QueryString As String
        Dim l_FoundRow As DataRow()
        Try
            If Not parameterDataTable Is Nothing Then
                l_QueryString = "AcctBreakDownType = '" & parameterAccountBreakDownType.Trim.ToUpper & "' AND SortOrder = " & parameterSortOrder & " AND AcctType = '" & parameterAcctType & "'"

                l_FoundRow = parameterDataTable.Select(l_QueryString)

                If l_FoundRow.Length > 0 Then
                    Return l_FoundRow(0)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch
            Throw
        End Try
    End Function

#End Region
    Public Function PrepareTableforHardshipProcessing() As DataTable
        Dim l_ContributionTableforHardshipProcessing As DataTable
        Dim l_CalculatedDataTableForCurrentAccounts As DataTable
        Dim l_Payee1DataTable As DataTable
        Dim dr_UsedRow As DataRow()
        Dim l_index As Integer = 0
        Dim l_AccountTypesUsed As String = ""
        Dim l_AccountGroupsUsed As String = ""
        Try
            If Not Session("CalculatedDataTableForCurrentAccounts_C19") Is Nothing Then
                l_CalculatedDataTableForCurrentAccounts = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
                l_ContributionTableforHardshipProcessing = l_CalculatedDataTableForCurrentAccounts.Clone()
            End If

            If Not Session("Payee1DataTable_C19") Is Nothing Then
                l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
                For Each dr As DataRow In l_Payee1DataTable.Rows
                    If dr("AcctType").GetType.ToString() <> "System.DBNull" Then
                        l_AccountTypesUsed = dr("AcctType").ToString()
                        If dr("AccountGroup").GetType.ToString() <> "System.DBNull" Then
                            l_AccountGroupsUsed = dr("AccountGroup").ToString()
                        End If
                        dr_UsedRow = l_CalculatedDataTableForCurrentAccounts.Select("AccountType = '" & l_AccountTypesUsed & "'")
                        If dr_UsedRow.Length > 0 Then
                            l_ContributionTableforHardshipProcessing.ImportRow(dr_UsedRow(0))
                            l_ContributionTableforHardshipProcessing.Rows(l_index)("Selected") = "True"
                            If l_AccountGroupsUsed = m_const_SavingsPlan_TD Or l_AccountGroupsUsed = m_const_SavingsPlan_TM Then
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("Taxable") = DirectCast(dr("Taxable"), Decimal)
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("Interest") = "0.0"
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("Emp.Total") = l_ContributionTableforHardshipProcessing.Rows(l_index)("Taxable") + l_ContributionTableforHardshipProcessing.Rows(l_index)("Interest") + l_ContributionTableforHardshipProcessing.Rows(l_index)("Non-Taxable")
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("Interest") = "0.0"
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("YMCATaxable") = "0.0"
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("YMCAInterest") = "0.0"
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("YMCATotal") = "0.0"
                                l_ContributionTableforHardshipProcessing.Rows(l_index)("Total") = l_ContributionTableforHardshipProcessing.Rows(l_index)("Emp.Total") + l_ContributionTableforHardshipProcessing.Rows(l_index)("YMCATotal")
                            End If
                            l_index = l_index + 1
                        End If

                    End If
                Next

            End If
            Return l_ContributionTableforHardshipProcessing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function MakeTableforRefundRequest(ByVal paramaterCalculatedDataTable As DataTable, ByVal PrameterPlanType As String) As Boolean
        Dim l_dt_CalculatedDataTable As DataTable
        Dim l_DataRows As DataRow()
        Dim l_TaxedAmount As Decimal
        Dim l_TaxedInterestAmount As Decimal
        Dim l_NonTaxedAmount As Decimal
        Dim l_RefundRequestDataTable As DataTable
        Dim l_RefundRequestDetailsDataTable As DataTable
        Dim l_ContributionDataTable As DataTable
        Dim l_RefundDataSet As DataSet
        Dim l_StringUniqueId As String

        Dim l_DataRow As DataRow
        Dim l_ContributionDataRow As DataRow
        Dim l_Integer_Tax As Decimal
        Session("OnlyYmcaSideforAM_C19") = Nothing

        Try
            l_ContributionDataTable = Me.CalculateTotal(paramaterCalculatedDataTable)
            If l_ContributionDataTable Is Nothing Then Return False

            Me.TotalAmount = Me.TotalAmount
            l_DataRows = l_ContributionDataTable.Select("AccountType = 'Total'")
            If l_DataRows.Length > 0 Then
                If l_DataRows(0)("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_NonTaxedAmount = 0.0
                Else
                    l_NonTaxedAmount = DirectCast(l_DataRows(0)("Non-Taxable"), Decimal)
                End If

                If l_DataRows(0)("Total").GetType.ToString = "System.DBNull" Then
                    l_TaxedAmount = 0.0
                Else
                    l_TaxedAmount = DirectCast(l_DataRows(0)("Total"), Decimal)
                End If
                l_TaxedAmount = l_TaxedAmount - l_NonTaxedAmount
            End If

            'For Ref Requests
            l_RefundRequestDataTable = DirectCast(Session("AtsRefRequests_C19"), DataTable).Clone
            If l_RefundRequestDataTable.Columns.Contains("PlanType") = False Then
                l_RefundRequestDataTable.Columns.Add("PlanType", System.Type.GetType("System.String"))
            End If
            If l_RefundRequestDataTable Is Nothing Then Return False

            '' This segment for adding the Values to Refund table...
            l_DataRow = l_RefundRequestDataTable.NewRow



            l_DataRow("UniqueID") = Me.PersonID
            l_DataRow("PersID") = Me.PersonID
            l_DataRow("FundEventID") = Me.FundID
            l_DataRow("RefundType") = Me.RefundType
            l_DataRow("RequestStatus") = "PEND"
            l_DataRow("StatusDate") = Now.Date
            'START | 2019.08.22 | SR | YRS-AT-4551 - Get request date from existing refund request.
            l_DataRow("RequestDate") = GetRefundRequestTableValue("RequestDate", Now.Date)
            'l_DataRow("RequestDate") = Now.Date
            'END | 2019.08.22 | SR | YRS-AT-4551 - Get request date from existing refund request.
            l_DataRow("ReleaseBlankType") = "???"
            l_DataRow("Amount") = Me.TotalAmount
            'START | 2019.08.22 | SR | YRS-AT-4551 - Get expiry date from existing refund request.
            l_DataRow("ExpireDate") = GetRefundRequestTableValue("ExpireDate", Now.Date.AddDays(Me.RefundExpiryDate))
            'l_DataRow("ExpireDate") = Now.Date.AddDays(Me.RefundExpiryDate)
            'END | 2019.08.22 | SR | YRS-AT-4551 - Get expiry date from existing refund request.
            l_DataRow("TaxRate") = 20.0
            l_DataRow("Taxable") = l_TaxedAmount
            l_DataRow("NonTaxable") = l_NonTaxedAmount
            'Added By Ganeswar Aug-04-2009
            '09-Dec-2009        Parveen     Changes for the Hardship and VOL withdrawal Issue
            If PrameterPlanType = "SAVINGS" Or CType(Session("ForceToVoluntaryWithdrawal_C19"), Boolean) = True Then
                'l_DataRow("HardShipAmt") = Me.Session("TDTotalAmount") ' "0.00"
                l_DataRow("HardShipAmt") = "0.00"
            End If
            'l_DataRow("HardShipAmt") = "0.00"
            l_DataRow("Deductions") = "0.00"
            l_Integer_Tax = CType(((l_TaxedAmount) * (20.0 / 100.0)).ToString("0.00"), Decimal)
            l_DataRow("Tax") = l_Integer_Tax
            l_DataRow("AddressID") = 0 ' we are passing 0 here as the address Id is handled in the proc
            l_DataRow("MinDistribution") = 0.0
            'START | 2019.08.22 | SR | YRS-AT-4551 - Get Release Sent Date from existing refund request.
            l_DataRow("ReleaseSentDate") = GetRefundRequestTableValue("ReleaseSentDate", Now.Date)
            'l_DataRow("ReleaseSentDate") = Now.Date
            'END | 2019.08.22 | SR | YRS-AT-4551 - Get Release Sent Date from existing refund request.
            l_DataRow("PIA") = Me.TerminationPIA
            l_DataRow("PlanType") = PrameterPlanType
            l_RefundRequestDataTable.Rows.Add(l_DataRow)


            'For ref request details

            '****************************************************
            'This segment for making Refund Request Details.. 
            '****************************************************

            'This Delcaration for Updating the Refund Request Details.

            Dim l_String_AccountType As String
            Dim l_String_UniqueID As String
            Dim l_String_RefRequestsID As String
            Dim l_String_AccountBreakDownType As String

            Dim l_Decimal_PersonalInterest As Decimal
            Dim l_Decimal_YmcaInterest As Decimal
            Dim l_Decimal_AccountTotal As Decimal
            Dim l_Decimal_PersonalPostTax As Decimal
            Dim l_Decimal_PersonalPreTax As Decimal
            Dim l_Decimal_PersonalTotal As Decimal
            Dim l_Decimal_YMCAPreTax As Decimal
            Dim l_Decimal_YMCATotal As Decimal
            Dim l_Decimal_GrandTotal As Decimal
            Dim l_Decimal_Total As Decimal
            Dim l_Decimal_PreTax As Decimal
            Dim l_Decimal_PostTax As Decimal
            Dim l_Decimal_Interst As Decimal

            Dim l_Decimal_TDAmount As Decimal = 0.0
            Dim l_Decimal_OtherAmount As Decimal = 0.0

            Dim l_Integer_SortOrder As Integer

            'Plan Split Variable 
            Dim l_String_AccountGroup As String = ""
            l_RefundRequestDetailsDataTable = DirectCast(Session("AtsRefRequestDetails_C19"), DataTable).Clone

            Dim l_bool_Selected As Boolean = False

            For Each l_ContributionDataRow In l_ContributionDataTable.Rows

                If l_ContributionDataRow.RowState <> DataRowState.Deleted Then ' If the row is deleted then Don;t cont the values
                    If Me.RefundType = "VOL" Then
                        If l_ContributionDataRow("Selected").GetType.ToString() = "System.DBNull" Then
                            l_bool_Selected = False
                        Else
                            l_bool_Selected = CType(l_ContributionDataRow("Selected"), Boolean)
                        End If
                    Else
                        l_bool_Selected = True
                    End If

                    If l_bool_Selected = True Then
                        If Not (l_ContributionDataRow("AccountType").GetType.ToString = "System.DBNull") Then

                            If Not (DirectCast(l_ContributionDataRow("AccountType"), String) = "Total") Then


                                l_Decimal_PreTax = 0.0
                                l_Decimal_PostTax = 0.0
                                l_Decimal_Interst = 0.0

                                l_Decimal_PersonalPostTax = 0.0
                                l_Decimal_PersonalPreTax = 0.0
                                l_Decimal_PersonalInterest = 0.0
                                l_Decimal_PersonalTotal = 0.0
                                l_Decimal_YMCAPreTax = 0.0
                                l_Decimal_YmcaInterest = 0.0
                                l_Decimal_YMCATotal = 0.0
                                l_Decimal_AccountTotal = 0.0
                                l_Decimal_GrandTotal = 0.0

                                l_String_AccountType = DirectCast(l_ContributionDataRow("AccountType"), String).Trim.ToUpper

                                l_String_AccountGroup = DirectCast(l_ContributionDataRow("AccountGroup"), String).Trim.ToUpper()

                                l_Decimal_Total = DirectCast(l_ContributionDataRow("Total"), Decimal)


                                If l_String_AccountGroup = m_const_SavingsPlan_TD Or l_String_AccountGroup = m_const_SavingsPlan_TM Then
                                    l_Decimal_TDAmount += DirectCast(l_ContributionDataRow("Total"), Decimal)
                                End If


                                l_Decimal_OtherAmount += l_Decimal_Total

                                Session("inOtherAmount_C19") = l_Decimal_OtherAmount

                                If l_String_AccountGroup = m_const_RetirementPlan_AM And ((l_Decimal_PersonalPostTax + l_Decimal_PersonalPreTax) = 0.0) Then
                                    Session("OnlyYmcaSideforAM_C19") = True
                                    l_String_AccountBreakDownType = "07"
                                    l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

                                    l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                    'l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)

                                    If Not l_DataRow Is Nothing Then
                                        'The Row is already exist So. Replace the values.

                                        l_DataRow("YMCAPreTax") = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAPreTax"), Decimal)
                                        l_DataRow("YMCAInterest") = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                        l_DataRow("YMCATotal") = DirectCast(l_ContributionDataRow("YMCAPreTax"), Decimal) + DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                        l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                        l_DataRow("GrandTotal") = l_DataRow("AcctTotal")
                                    Else


                                        l_DataRow = l_RefundRequestDetailsDataTable.NewRow


                                        l_Decimal_YMCAPreTax = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal)
                                        l_Decimal_YmcaInterest = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal)
                                        l_Decimal_YMCATotal = l_Decimal_YMCAPreTax + l_Decimal_YmcaInterest
                                        l_Decimal_AccountTotal = l_Decimal_YMCATotal
                                        l_Decimal_GrandTotal = l_Decimal_YMCATotal

                                        l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
                                        l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
                                        l_DataRow("YMCATotal") = l_Decimal_YMCATotal
                                        l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                        l_DataRow("GrandTotal") = l_Decimal_GrandTotal

                                        l_DataRow("AcctType") = l_String_AccountType
                                        l_DataRow("PersonalPostTax") = l_ContributionDataRow("Non-Taxable")
                                        l_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
                                        l_DataRow("PersonalInterest") = l_ContributionDataRow("Interest")
                                        l_DataRow("PersonalTotal") = l_ContributionDataRow("Emp.Total")

                                        l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                        l_DataRow("SortOrder") = l_Integer_SortOrder

                                        l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                    End If

                                    l_ContributionDataRow("YMCATaxable") = "0.00"

                                    l_Decimal_YMCAPreTax = 0.0
                                    l_Decimal_YmcaInterest = 0.0
                                    l_Decimal_AccountTotal = 0.0
                                    l_Decimal_GrandTotal = 0.0

                                    l_Decimal_YMCATotal = 0.0
                                    l_Decimal_PersonalTotal = 0.0
                                Else

                                    '' Do calculation on Refund Request Details side.
                                    If l_ContributionDataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
                                        l_Decimal_PersonalTotal = 0.0
                                        l_ContributionDataRow("Emp.Total") = "0.00"
                                    Else
                                        l_Decimal_PersonalTotal = DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
                                    End If

                                    If l_ContributionDataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
                                        l_Decimal_YMCATotal = 0.0
                                        l_ContributionDataRow("YMCATotal") = "0.00"
                                    Else
                                        l_Decimal_YMCATotal = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
                                    End If

                                End If

                                ''' Do while .. Loop
                                Do While (l_Decimal_PersonalTotal + l_Decimal_YMCATotal) > 0.0

                                    l_Decimal_Interst = 0.0

                                    If DirectCast(l_ContributionDataRow("YMCATotal"), Decimal) > 0.0 Then
                                        '' If YMCA Total is greater than 0.00
                                        If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
                                        Else
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, False, True, False, False)
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)

                                        End If


                                        If Not l_DataRow Is Nothing Then


                                            l_DataRow("YMCAPreTax") = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAPreTax"), Decimal)
                                            l_DataRow("YMCAInterest") = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                            l_DataRow("YMCATotal") = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                            l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("YMCATotal"), Decimal)
                                            l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

                                        Else
                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow

                                            l_Decimal_YMCAPreTax = DirectCast(l_ContributionDataRow("YMCATaxable"), Decimal)
                                            l_Decimal_YmcaInterest = DirectCast(l_ContributionDataRow("YMCAInterest"), Decimal)
                                            l_Decimal_YMCATotal = DirectCast(l_ContributionDataRow("YMCATotal"), Decimal)
                                            l_Decimal_AccountTotal = l_Decimal_YMCATotal
                                            l_Decimal_GrandTotal = l_Decimal_YMCATotal


                                            l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
                                            l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
                                            l_DataRow("YMCATotal") = l_Decimal_YMCATotal
                                            l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                            l_DataRow("GrandTotal") = l_Decimal_GrandTotal

                                            '' Add additional  Detalis..
                                            l_DataRow("AcctType") = l_String_AccountType
                                            l_DataRow("PersonalPostTax") = "0.00"
                                            l_DataRow("PersonalPreTax") = "0.00"
                                            l_DataRow("PersonalInterest") = "0.00"
                                            l_DataRow("PersonalTotal") = "0.00"

                                            l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                            l_DataRow("SortOrder") = l_Integer_SortOrder

                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                        End If

                                        l_ContributionDataRow("YMCATaxable") = "0.0"
                                        l_ContributionDataRow("YMCAInterest") = "0.0"
                                        l_ContributionDataRow("YMCATotal") = "0.0"

                                        l_Decimal_YMCAPreTax = 0.0
                                        l_Decimal_YmcaInterest = 0.0
                                        l_Decimal_YMCATotal = 0.0
                                        l_Decimal_AccountTotal = 0.0
                                        l_Decimal_GrandTotal = 0.0

                                        'l_Decimal_PersonalPostTax = 0.0
                                        'l_Decimal_PersonalPreTax = 0.0
                                        'l_Decimal_PersonalInterest = 0.0
                                        'l_Decimal_PersonalTotal = 0.0

                                        '' Make YMCA total is Thru.
                                        l_Decimal_YMCATotal = 0.0


                                    ElseIf DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) > 0.0 Then
                                        '' If Personal Total is greater than 0.00
                                        If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
                                        Else
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, False, True)
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                            'l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable)
                                        End If


                                        If DirectCast(l_ContributionDataRow("Taxable"), Decimal) > 0.0 Then

                                            l_Decimal_PreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                        Else
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                        End If


                                        If Not l_DataRow Is Nothing Then

                                            l_DataRow("PersonalPreTax") = l_ContributionDataRow("Taxable")
                                            l_DataRow("PersonalPostTax") = DirectCast(l_DataRow("PersonalPostTax"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                            l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) + l_Decimal_Interst
                                            l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Emp.Total"), Decimal)
                                            l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

                                        Else

                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow


                                            l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst
                                            l_Decimal_AccountTotal = l_Decimal_Total
                                            l_Decimal_GrandTotal = l_Decimal_AccountTotal

                                            l_DataRow("PersonalPreTax") = l_Decimal_PreTax
                                            l_DataRow("PersonalPostTax") = l_Decimal_PostTax
                                            l_DataRow("PersonalInterest") = l_Decimal_Interst
                                            l_DataRow("PersonalTotal") = l_Decimal_Total
                                            l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                            l_DataRow("GrandTotal") = l_Decimal_GrandTotal

                                            l_DataRow("AcctType") = l_String_AccountType
                                            l_DataRow("YMCAPreTax") = "0.00"
                                            l_DataRow("YMCAInterest") = "0.00"
                                            l_DataRow("YMCATotal") = "0.00"

                                            l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                            l_DataRow("SortOrder") = l_Integer_SortOrder

                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                        End If


                                        l_ContributionDataRow("Taxable") = "0.00"
                                        l_ContributionDataRow("Interest") = DirectCast(l_ContributionDataRow("Interest"), Decimal) - l_Decimal_Interst
                                        l_ContributionDataRow("Emp.Total") = l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest


                                        l_Decimal_PersonalTotal = 0.0

                                    Else
                                        '' Do otherwise
                                        If Not Session("OnlyYmcaSideforAM_C19") Is Nothing Then
                                        Else
                                            l_String_AccountBreakDownType = Me.GetAccountBreakDownType(l_String_AccountGroup, True, False, True, False)
                                            l_Integer_SortOrder = Me.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType)

                                            l_DataRow = Me.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType)
                                        End If



                                        If DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal) > 0.0 Then

                                            l_Decimal_PreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_Decimal_PostTax = DirectCast(l_ContributionDataRow("Non-Taxable"), Decimal)
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)

                                            l_Decimal_Interst = l_Decimal_Interst * (l_Decimal_PreTax / (DirectCast(l_ContributionDataRow("Emp.Total"), Decimal) - DirectCast(l_ContributionDataRow("Interest"), Decimal)))

                                        Else
                                            l_Decimal_Interst = DirectCast(l_ContributionDataRow("Interest"), Decimal)
                                        End If

                                        If Not l_DataRow Is Nothing Then

                                            l_DataRow("PersonalPreTax") = DirectCast(l_DataRow("PersonalPreTax"), Decimal) + DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_DataRow("PersonalInterest") = DirectCast(l_DataRow("PersonalInterest"), Decimal) + l_Decimal_Interst
                                            l_DataRow("PersonalTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_ContributionDataRow("Taxable"), Decimal) + l_Decimal_Interst
                                            l_DataRow("AcctTotal") = DirectCast(l_DataRow("PersonalTotal"), Decimal) + DirectCast(l_DataRow("PersonalTotal"), Decimal)
                                            l_DataRow("GrandTotal") = l_DataRow("AcctTotal")

                                        Else

                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow

                                            l_Decimal_PersonalPreTax = DirectCast(l_ContributionDataRow("Taxable"), Decimal)
                                            l_Decimal_PersonalInterest = l_Decimal_Interst
                                            l_Decimal_PersonalTotal = l_Decimal_Interst + l_Decimal_PersonalPreTax
                                            l_Decimal_AccountTotal = l_Decimal_PersonalTotal
                                            l_Decimal_GrandTotal = l_Decimal_AccountTotal

                                            l_DataRow("PersonalPreTax") = l_Decimal_PersonalPreTax
                                            l_DataRow("PersonalInterest") = l_Decimal_PersonalInterest
                                            l_DataRow("PersonalTotal") = l_Decimal_PersonalTotal
                                            l_DataRow("AcctTotal") = l_Decimal_AccountTotal
                                            l_DataRow("GrandTotal") = l_Decimal_GrandTotal

                                            l_DataRow("AcctType") = l_String_AccountType
                                            l_DataRow("PersonalPostTax") = l_ContributionDataRow("Non-Taxable")
                                            l_DataRow("YMCAPreTax") = l_Decimal_YMCAPreTax
                                            l_DataRow("YMCAInterest") = l_Decimal_YmcaInterest
                                            l_DataRow("YMCATotal") = l_Decimal_YMCATotal

                                            l_DataRow("AcctBreakDownType") = l_String_AccountBreakDownType
                                            l_DataRow("SortOrder") = l_Integer_SortOrder

                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow)

                                        End If


                                        l_ContributionDataRow("Non-Taxable") = "0.00"
                                        l_ContributionDataRow("Interest") = DirectCast(l_ContributionDataRow("Interest"), Decimal) - l_Decimal_Interst
                                        l_ContributionDataRow("Emp.Total") = l_ContributionDataRow("Interest")

                                        l_Decimal_PersonalPostTax = 0.0
                                        l_Decimal_PersonalInterest = 0.0
                                        l_Decimal_PersonalTotal = 0.0
                                        l_Decimal_AccountTotal = 0.0
                                        l_Decimal_GrandTotal = 0.0

                                        l_Decimal_PersonalTotal = 0.0
                                        l_Decimal_YMCATotal = 0.0

                                    End If
                                Loop

                            End If
                        End If
                    End If
                End If
            Next

            l_RefundDataSet = New DataSet("RefundRequest")

            If l_Decimal_TDAmount > 0.0 Then
                l_DataRow = l_RefundRequestDataTable.Rows(0)
                l_DataRow("HardShipAmt") = l_Decimal_TDAmount
            End If

            'l_StringUniqueId = YMCARET.YmcaBusinessObject.RefundRequest.InsertRefunds(l_RefundDataSet)
            l_StringUniqueId = YMCARET.YmcaBusinessObject.RefundRequest.getGUI_ID()
            'updating the generated UniqueId returned from the proc into the cache dataset.
            If l_StringUniqueId <> "" Then
                l_RefundRequestDataTable.Rows(0)(0) = l_StringUniqueId
                For Each dr As DataRow In l_RefundRequestDetailsDataTable.Rows
                    dr("RefRequestsID") = l_StringUniqueId
                Next

                l_RefundRequestDataTable.AcceptChanges()
                l_RefundRequestDetailsDataTable.AcceptChanges()
                Session("RefundRequestID_C19") = l_StringUniqueId
                Session("AtsRefRequests_C19") = l_RefundRequestDataTable

                Session("AtsRefRequestDetails_C19") = l_RefundRequestDetailsDataTable
                l_RefundDataSet.Tables.Add(l_RefundRequestDataTable)
                l_RefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable)
                Session("RefundsDataSetForNewRequests_C19") = l_RefundDataSet
                Dim l_dt_NewRefundRequestTable As DataTable
                Dim l_dr_NewRowRefundRequestTable As DataRow
                If Not Session("RefundRequestTable_C19") Is Nothing Then
                    l_dt_NewRefundRequestTable = DirectCast(Session("RefundRequestTable_C19"), DataTable).Clone
                    l_dr_NewRowRefundRequestTable = l_dt_NewRefundRequestTable.NewRow()

                    l_dr_NewRowRefundRequestTable("UniqueID") = l_StringUniqueId

                    l_dr_NewRowRefundRequestTable("FundEventID") = Me.FundID
                    l_dr_NewRowRefundRequestTable("PersonID") = Me.PersonID
                    l_dr_NewRowRefundRequestTable("ExpireDate") = Now.Date.AddDays(Me.RefundExpiryDate)
                    l_dr_NewRowRefundRequestTable("RefundType") = Me.RefundType
                    '17-Dec-2009        Parveen     Changes for the BugID-1073
                    l_dr_NewRowRefundRequestTable("RefundTypeHeader") = Me.RefundType
                    '17-Dec-2009        Parveen     Changes for the BugID-1073
                    l_dr_NewRowRefundRequestTable("RequestStatus") = "PEND"
                    l_dr_NewRowRefundRequestTable("StatusDate") = Now.Date
                    l_dr_NewRowRefundRequestTable("RequestDate") = Now.Date
                    l_dr_NewRowRefundRequestTable("Gross Amt.") = Me.TotalAmount
                    l_dr_NewRowRefundRequestTable("Net Amt. Excl. Deductions") = (Me.TotalAmount - l_Integer_Tax)
                    l_dr_NewRowRefundRequestTable("ReleaseBlankType") = "???"
                    l_dr_NewRowRefundRequestTable("ReleaseSentDate") = Now.Date
                    l_dr_NewRowRefundRequestTable("AddressID") = 0
                    l_dr_NewRowRefundRequestTable("NonTaxable") = l_NonTaxedAmount
                    l_dr_NewRowRefundRequestTable("Taxable") = l_TaxedAmount
                    l_dr_NewRowRefundRequestTable("Tax") = l_Integer_Tax
                    l_dr_NewRowRefundRequestTable("TaxRate") = 20.0
                    l_dr_NewRowRefundRequestTable("PIA") = Me.TerminationPIA
                    l_dr_NewRowRefundRequestTable("MinDistribution") = 0.0
                    If PrameterPlanType = "SAVINGS" Then
                        'l_dr_NewRowRefundRequestTable("HardShipAmt") = Me.Session("TDTotalAmount") ' "0.00"
                        l_dr_NewRowRefundRequestTable("HardShipAmt") = "0.00"
                    End If
                    'l_dr_NewRowRefundRequestTable("HardShipAmt") = "0.00"
                    l_dr_NewRowRefundRequestTable("Deductions") = "0.00"

                    l_dt_NewRefundRequestTable.Rows.Add(l_dr_NewRowRefundRequestTable)
                    l_dt_NewRefundRequestTable.AcceptChanges()
                    Session("RefundRequestTable_C19") = l_dt_NewRefundRequestTable
                End If


                Return True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "Properties "
    'Added by Amit on 23-11-2009
    Public Property FirstInstallment() As Decimal
        Get
            If Not Session("FirstInstallment_C19") Is Nothing Then
                Return (DirectCast(Session("FirstInstallment_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("FirstInstallment_C19") = Value
        End Set
    End Property
    Public Property DefferredInstallment() As Decimal
        Get
            If Not Session("DefferredInstallment_C19") Is Nothing Then
                Return (DirectCast(Session("DefferredInstallment_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("DefferredInstallment_C19") = Value
        End Set
    End Property
    'Added by Amit on 23-11-2009

    'Added by Dilip  on 23-11-2009 for Defered Rollover
    Public Property DefferredPaymentAmount() As Decimal
        Get
            If Not Session("DefferredPaymentAmount_C19") Is Nothing Then
                Return (DirectCast(Session("DefferredPaymentAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("DefferredPaymentAmount_C19") = Value
        End Set
    End Property
    Public Property DefferredInstallmentAmount() As Decimal
        Get
            If Not Session("DefferredInstallmentAmount_C19") Is Nothing Then
                Return (DirectCast(Session("DefferredInstallmentAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("DefferredInstallmentAmount_C19") = Value
        End Set
    End Property
    Public Property DefferredInstallmentAmountTaxable() As Decimal
        Get
            If Not Session("DefferredInstallmentAmountTaxable_C19") Is Nothing Then
                Return (DirectCast(Session("DefferredInstallmentAmountTaxable_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("DefferredInstallmentAmountTaxable_C19") = Value
        End Set
    End Property
    Public Property DefferredInstallmentAmountNonTaxable() As Decimal
        Get
            If Not Session("DefferredInstallmentAmountNonTaxable_C19") Is Nothing Then
                Return (DirectCast(Session("DefferredInstallmentAmountNonTaxable_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("DefferredInstallmentAmountNonTaxable_C19") = Value
        End Set
    End Property
    'Added by Dilip  on 23-11-2009 for Defered Rollover

    'added the property as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter
    Public Property DisplayMessageForLetters() As Boolean
        Get
            If Not Session("DisplayMessageForLetters_C19") Is Nothing Then
                Return (CType(Session("DisplayMessageForLetters_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("DisplayMessageForLetters_C19") = Value
        End Set
    End Property
    'added the property the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter
    'Added the Property to check the save is done or not-Amit 09-09-2009
    Public Property IsSaveProcessComplete() As Boolean
        Get
            If Not Session("IsSaveProcessComplete_C19") Is Nothing Then
                Return (CType(Session("IsSaveProcessComplete_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsSaveProcessComplete_C19") = Value
        End Set
    End Property
    'Added the Property to check the save is done or not-Amit 09-09-2009
    Public Property DataTableDisplayConsolidatedTotal() As DataTable
        Get
            Return m_DataTableDisplayConsolidatedTotal
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableDisplayConsolidatedTotal = Value
        End Set
    End Property

    Public Property RequestUniqueId() As DataTable
        Get
            Return RequestUniqueId
        End Get
        Set(ByVal Value As DataTable)
            RequestUniqueId = Value
        End Set
    End Property

    Public Property DetailsUniqueId() As DataTable
        Get
            Return DetailsUniqueId
        End Get
        Set(ByVal Value As DataTable)
            DetailsUniqueId = Value
        End Set
    End Property


    Public Property DataTableDisplayRetirementPlan() As DataTable
        Get
            Return m_DataTableDisplayRetirementPlan
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableDisplayRetirementPlan = Value
        End Set
    End Property
    Public Property DataTableDisplaySavingsPlan() As DataTable
        Get
            Return m_DataTableDisplaySavingsPlan
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableDisplaySavingsPlan = Value
        End Set
    End Property
    Public Property AllowMarketBased() As Boolean
        Get
            If Not Session("AllowMarketBased_C19") Is Nothing Then
                Return (CType(Session("AllowMarketBased_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowMarketBased_C19") = Value
        End Set
    End Property








    Public Property VoluntaryWithdrawalTotal() As Decimal
        Get
            Return m_decimal_VolWithdrawalTotal
        End Get
        Set(ByVal Value As Decimal)
            m_decimal_VolWithdrawalTotal = Value
        End Set
    End Property
    Public Property HardshipWithdrawalTotal() As Decimal
        Get
            Return m_decimal_HardWithdrawalTotal
        End Get
        Set(ByVal Value As Decimal)
            m_decimal_HardWithdrawalTotal = Value
        End Set
    End Property
    Public Property RefundState() As String
        Get
            If Not Session("RefundState_C19") Is Nothing Then
                Return (DirectCast(Session("RefundState_C19"), String))
            Else
                Return "R"

            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundState_C19") = Value
        End Set
    End Property
    'Plan Split Changes
    Public Property PlanTypeChosen() As String
        Get
            If Not Session("PlanTypeChosen_C19") Is Nothing Then
                Return (DirectCast(Session("PlanTypeChosen_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PlanTypeChosen_C19") = Value
        End Set
    End Property
    'To Get / Set MinDistributionTaxRate 
    Public Property MinDistributionTaxRate() As Decimal
        Get
            If Not Session("MinDistributionTaxRate_C19") Is Nothing Then
                Return (CType(Session("MinDistributionTaxRate_C19"), Decimal))
            Else
                Return 10
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinDistributionTaxRate_C19") = Value
        End Set
    End Property
    'To Get / Set Tax Rate
    Public Property TaxRate() As Decimal
        Get
            If Not Session("TaxRate_C19") Is Nothing Then
                Return (CType(Session("TaxRate_C19"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxRate_C19") = Value
        End Set
    End Property
    'To get the withholding type code for the selected 
    Public Property DataTableGetWithholdingDeductions() As DataTable
        Get
            Return m_DataTableWithholdingDeductions
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableWithholdingDeductions = Value
        End Set
    End Property
    ' to get the Payee1 LastName and First Name
    Public Property Payee1Name() As String
        Get
            If Not Session("Payee1Name_C19") Is Nothing Then
                Return (DirectCast(Session("Payee1Name_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee1Name_C19") = Value
        End Set
    End Property
    ' To get / set Deductions amount.
    Private Property DeductionsAmount() As Decimal
        Get
            If Not Session("DeductionsAmount_C19") Is Nothing Then
                Return (DirectCast(Session("DeductionsAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("DeductionsAmount_C19") = Value
        End Set
    End Property

    Private Property RoundingAmount() As Decimal
        Get
            If Not Session("RoundingAmount_C19") Is Nothing Then
                Return (DirectCast(Session("RoundingAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RoundingAmount_C19") = Value
        End Set
    End Property

    'Added By Ganeswar on 2009 Aug 04
    Private Property TDTotalAmount() As Decimal
        Get
            If Not Session("TDTotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("TDTotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDTotalAmount_C19") = Value
        End Set
    End Property
    'Added By Ganeswar on 2009 Aug 04

    ' To get / set Payee1 Addresss ID
    Private Property PayeeAddressID() As String
        Get
            If Not Session("PayeeAddressID_C19") Is Nothing Then
                Return (DirectCast(Session("PayeeAddressID_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PayeeAddressID_C19") = Value
        End Set
    End Property
    ' To get / set Payee2 ID
    Private Property Payee2ID() As String
        Get
            If Not Session("Payee2ID_C19") Is Nothing Then
                Return (DirectCast(Session("Payee2ID_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee2ID_C19") = Value
        End Set
    End Property

    ' To get / set Payee3 ID
    Private Property Payee3ID() As String
        Get
            If Not Session("Payee3ID_C19") Is Nothing Then
                Return (DirectCast(Session("Payee3ID_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee3ID_C19") = Value
        End Set
    End Property
    ' To get / set RefundType
    Private Property ChangedRefundType() As String
        Get
            If Not Session("ChangedRefundType_C19") Is Nothing Then
                Return (DirectCast(Session("ChangedRefundType_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("ChangedRefundType_C19") = Value
        End Set
    End Property
    'To Keep the flag to store Whether Type is Personal Or Not.
    Private Property IsPersonalOnly() As Boolean
        Get
            If Not (Session("IsPersonalOnly_C19")) Is Nothing Then
                Return (CType(Session("IsPersonalOnly_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPersonalOnly_C19") = Value
        End Set
    End Property
    ''To Keep the Currency Code.
    Private Property CurrencyCode() As String
        Get
            If Not Session("CurrencyCode_C19") Is Nothing Then
                Return (DirectCast(Session("CurrencyCode_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("CurrencyCode_C19") = Value
        End Set
    End Property
    'To Keep the RefundRequest ID for Selected Member
    Private Property SessionRefundRequestID() As String
        Get
            If Not (Session("RefundRequestID_C19")) Is Nothing Then
                Return (DirectCast(Session("RefundRequestID_C19"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("RefundRequestID_C19") = Value
        End Set
    End Property
    'To Get / Set HardShip Amount, incase of HardShip.
    Private Property HardshipAmount() As Decimal
        Get
            If Not Session("HardshipAmount_C19") Is Nothing Then
                Return (DirectCast(Session("HardshipAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipAmount_C19") = Value
        End Set
    End Property
    'To Get / Set HardShipTaxRate
    Private Property HardShipTaxRate() As Decimal
        Get
            If Not Session("HardShipTaxRate_C19") Is Nothing Then
                Return (DirectCast(Session("HardShipTaxRate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardShipTaxRate_C19") = Value
        End Set
    End Property
    Private Property TDUsedAmount() As Decimal
        Get
            If Not Session("TDUsedAmount_C19") Is Nothing Then
                Return (DirectCast(Session("TDUsedAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDUsedAmount_C19") = Value
        End Set
    End Property
    Public Property ContributionDataTable() As DataTable
        Get
            If Not m_DataTableContributionDataTable Is Nothing Then
                Return m_DataTableContributionDataTable
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableContributionDataTable = Value
        End Set
    End Property


    'To set & get Minimum Distribution Amount
    Private Property MinimumDistributionAmount() As Decimal
        Get
            If Not Session("MinimumDistributionAmount_C19") Is Nothing Then
                Return (DirectCast(Session("MinimumDistributionAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributionAmount_C19") = Value
        End Set
    End Property

    Private Property RetirementPlan_MinDistributionAmount() As Decimal
        Get
            If Not Session("RetirementPlan_MinDistributionAmount_C19") Is Nothing Then
                Return (CType(Session("RetirementPlan_MinDistributionAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_MinDistributionAmount_C19") = Value
        End Set
    End Property
    Private Property Saving_MinDistributionAmount() As Decimal
        Get
            If Not Session("Saving_MinDistributionAmount_C19") Is Nothing Then
                Return (CType(Session("Saving_MinDistributionAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Saving_MinDistributionAmount_C19") = Value
        End Set
    End Property
    'To Get / Set the default Tax Rate for a Minimum
    Private Property MinimumDistributionTaxRate() As Decimal
        Get
            If Not Session("MinimumDistributionTaxRate_C19") Is Nothing Then
                Return (DirectCast(Session("MinimumDistributionTaxRate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributionTaxRate_C19") = Value
        End Set
    End Property

    'To Get / Set Federal Tax Rate
    Private Property FederalTaxRate() As Decimal
        Get
            If Not Session("FederalTaxRate_C19") Is Nothing Then
                Return (DirectCast(Session("FederalTaxRate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("FederalTaxRate_C19") = Value
        End Set
    End Property

    '' To Get / Set the Refund Expiry Date
    Private Property RefundExpiryDate() As Integer
        Get
            If Not Session("RefundExpiryDate_C19") Is Nothing Then
                Return (CType(Session("RefundExpiryDate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RefundExpiryDate_C19") = Value
        End Set
    End Property

    ' To Get / set the MinimumDistributedAge
    Private Property MinimumDistributedAge() As Decimal
        Get
            If Not Session("MinimumDistributedAge_C19") Is Nothing Then
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributedAge_C19") = Value
        End Set
    End Property

    'To Get / Set Maximum BA PIA Amount Phase V. by Dilip
    Private Property MaximumBAAmount() As Decimal
        Get
            If Not Session("MaximumBAAmount_C19") Is Nothing Then
                Return (DirectCast(Session("MaximumBAAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MaximumBAAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Maximum PIA Amount.
    Private Property MaximumPIAAmount() As Decimal
        Get
            If Not Session("MaximumPIAAmount_C19") Is Nothing Then
                Return (DirectCast(Session("MaximumPIAAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MaximumPIAAmount_C19") = Value
        End Set
    End Property
    'Shubhrata Jan 9th 2006 - to replace hard coded value of 5000(min PIA to retire)
    Private Property MinimumPIAToRetire() As Decimal
        Get
            If Not Session("MinimumPIAToRetire_C19") Is Nothing Then
                Return (DirectCast(Session("MinimumPIAToRetire_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumPIAToRetire_C19") = Value
        End Set
    End Property
    'Shubhrata Jan 9th 2006
    ' To Get / Set Is vested flag
    Private Property IsVested() As Boolean
        Get
            If Not Session("IsVested_C19") Is Nothing Then
                Return (CType(Session("IsVested_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsVested_C19") = Value
        End Set
    End Property

    ' To Get / Set terminated flag
    Private Property IsTerminated() As Boolean
        Get
            If Not Session("IsTerminated_C19") Is Nothing Then
                Return (CType(Session("IsTerminated_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsTerminated_C19") = Value
        End Set
    End Property

    ' To get / set Person Age
    Private Property PersonAge() As Decimal
        Get
            If Not Session("PersonAge_C19") Is Nothing Then
                Return (DirectCast(Session("PersonAge_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PersonAge_C19") = Value
        End Set
    End Property

    ' To Get / Set Termination PIA
    Private Property TerminationPIA() As Decimal
        Get
            If Not Session("TerminationPIA_C19") Is Nothing Then
                Return (DirectCast(Session("TerminationPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TerminationPIA_C19") = Value
        End Set
    End Property

    'To Get / Set CurrentPIA
    Private Property CurrentPIA() As Decimal
        Get
            If Not Session("CurrentPIA_C19") Is Nothing Then
                Return (DirectCast(Session("CurrentPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CurrentPIA_C19") = Value
        End Set
    End Property
    ' BA Account YMCA PhaseV 08-04-2009 by Dilip
    ' To Get / Set BATermination PIA
    Private Property BATerminationPIA() As Decimal
        Get
            If Not Session("BATerminationPIA_C19") Is Nothing Then
                Return (DirectCast(Session("BATerminationPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("BATerminationPIA_C19") = Value
        End Set
    End Property

    'To Get / Set BA CurrentPIA
    Private Property BACurrent() As Decimal
        Get
            If Not Session("BACurrent_C19") Is Nothing Then
                Return (DirectCast(Session("BACurrent_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("BACurrent_C19") = Value
        End Set
    End Property
    ' BA Account YMCA PhaseV 08-04-2009 by Dilip
    'To get / set YMCA Amount
    Public Property YMCAAvailableAmount() As Decimal
        Get
            If Not Session("YMCAAvailableAmount_C19") Is Nothing Then
                Return (DirectCast(Session("YMCAAvailableAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("YMCAAvailableAmount_C19") = Value
        End Set
    End Property
    Public Property HasPersonalMoney() As Decimal
        Get
            If Not Session("HasPersonalMoney_C19") Is Nothing Then
                Return (DirectCast(Session("HasPersonalMoney_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HasPersonalMoney_C19") = Value
        End Set
    End Property
    ' To get / set VoluntryAmount
    Public Property VoluntryAmountForProcessing() As Decimal
        Get
            If Not Session("VoluntryAmountForProcessing_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmountForProcessing_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmountForProcessing_C19") = Value
        End Set
    End Property
    Public Property VoluntryAmount() As Decimal
        Get
            If Not Session("VoluntryAmount_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmount_C19") = Value
        End Set
    End Property
    Public Property VoluntryAmount_Retirement() As Decimal
        Get
            If Not Session("VoluntryAmount_Retirement_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmount_Retirement_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmount_Retirement_C19") = Value
        End Set
    End Property
    Public Property VoluntryAmount_Savings() As Decimal
        Get
            If Not Session("VoluntryAmount_Savings_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmount_Savings_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmount_Savings_C19") = Value
        End Set
    End Property

    ' To get / set TD Account Amount
    Private Property TDAccountAmount() As Decimal
        Get
            If Not Session("TDAccountAmount_C19") Is Nothing Then
                Return (DirectCast(Session("TDAccountAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDAccountAmount_C19") = Value
        End Set
    End Property


    'To get / set the PersonID property.    
    Private Property PersonID() As String
        Get
            If Not Session("PersonID") Is Nothing Then
                Return (DirectCast(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property

    ' To get / set FundID
    Private Property FundID() As String
        Get
            If Not Session("FundID") Is Nothing Then
                Return (DirectCast(Session("FundID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("FundID") = Value
        End Set
    End Property

    ' To get / set RefundType
    Private Property RefundType() As String
        Get
            If Not Session("RefundType_C19") Is Nothing Then
                Return (DirectCast(Session("RefundType_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundType_C19") = Value
        End Set
    End Property

    'To Keep the Refund Request Datagrid to Refresh 
    'Added By Dilip on 30-10-2009  for market base withdrawal
    Private Property ISMarket() As Int16
        Get
            If Not Session("ISMarket_C19") Is Nothing Then
                Return (CType(Session("ISMarket_C19"), Int16))
            Else
                Return 1
            End If

        End Get
        Set(ByVal Value As Int16)
            Session("ISMarket_C19") = Value
        End Set
    End Property
    'Added By Dilip on 30-10-2009  for market base withdrawal

    Private Property SessionIsRefundRequest() As Boolean
        Get
            If Not (Session("IsRefundRequest_C19")) Is Nothing Then
                Return (CType(Session("IsRefundRequest_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundRequest_C19") = Value
        End Set
    End Property

    'To Keep the flag to Raise the Popup window
    Private Property SessionIsRefundPopupAllowed() As Boolean
        Get
            If Not (Session("IsRefundPopupAllowed_C19")) Is Nothing Then
                Return (CType(Session("IsRefundPopupAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundPopupAllowed_C19") = Value
        End Set
    End Property


    'To Get / Set Total Amount
    Public Property TotalAmount() As Decimal
        Get
            If Not Session("TotalAmount_C19") Is Nothing Then
                'BT 567: Casting problem
                'Return (DirectCast(Session("TotalAmount"), String))
                Return (CType(Session("TotalAmount_C19"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property PersonTotalAmount() As Decimal
        Get
            If Not Session("PersonTotalAmount_C19") Is Nothing Then
                'Return (DirectCast(Session("PersonTotalAmount"), String))
                Return (CType(Session("PersonTotalAmount_C19"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("PersonTotalAmount_C19") = Value
        End Set
    End Property


    'To Get / Set Type Choosen.
    Private Property IsTypeChoosen() As Boolean
        Get
            If Not Session("IsTypeChoosen_C19") Is Nothing Then
                Return (CType(Session("IsTypeChoosen_C19"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("IsTypeChoosen_C19") = Value
        End Set
    End Property
    ''added by ruchi to keep track of the amount available
    Private Property CanRequest() As Boolean
        Get
            If Not Session("CanRequest_C19") Is Nothing Then
                Return (CType(Session("CanRequest_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("CanRequest_C19") = Value
        End Set
    End Property
    'added by Shubhrata YREN-3039,Feb13th 2007
    'To Keep the StatusType for Selected Member
    Private Property SessionStatusType() As String
        Get
            If Not (Session("StatusType_C19")) Is Nothing Then
                Return (Trim(DirectCast(Session("StatusType_C19"), String))) '--SR | 2016.08.25 | YRS-AT-3084 - Remove unnecessary spaces.
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("StatusType_C19") = Value
        End Set
    End Property
    'Shubhrata Plan Split Changes
    Private Property PlanChosen() As String
        Get
            If Not Session("PlanChosen_C19") Is Nothing Then
                Return (DirectCast(Session("PlanChosen_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PlanChosen_C19") = Value
        End Set
    End Property
    Public Property RetirementPlan_TotalAmount() As Decimal
        Get
            If Not Session("RetirementPlan_TotalAmount_C19") Is Nothing Then
                'Return (DirectCast(Session("RetirementPlan_TotalAmount"), String))
                Return (CType(Session("RetirementPlan_TotalAmount_C19"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_TotalAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property RetirementPlan_PersonTotalAmount() As Decimal
        Get
            If Not Session("RetirementPlan_PersonTotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("RetirementPlan_PersonTotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_PersonTotalAmount_C19") = Value
        End Set
    End Property
    Public Property SavingsPlan_TotalAmount() As Decimal
        Get
            If Not Session("SavingsPlan_TotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("SavingsPlan_TotalAmount_C19"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SavingsPlan_TotalAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property SavingsPlan_PersonTotalAmount() As Decimal
        Get
            If Not Session("SavingsPlan_PersonTotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("SavingsPlan_PersonTotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SavingsPlan_PersonTotalAmount_C19") = Value
        End Set
    End Property
    Private Property IsRetiredActive() As Boolean
        Get
            If Not Session("IsRetiredActive_C19") Is Nothing Then
                Return (CType(Session("IsRetiredActive_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRetiredActive_C19") = Value
        End Set
    End Property
    Private Property AllowedRetirementPlan() As Boolean
        Get
            If Not Session("AllowedRetirementPlan_C19") Is Nothing Then
                Return (CType(Session("AllowedRetirementPlan_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowedRetirementPlan_C19") = Value
        End Set
    End Property
    Private Property AllowedPersonalSideRefund() As Boolean
        Get
            If Not Session("AllowPersonalSideRefund_C19") Is Nothing Then
                Return (CType(Session("AllowPersonalSideRefund_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowPersonalSideRefund_C19") = Value
        End Set
    End Property
    ' To Get / Set Is HardShip flag
    Public Property IsHardShip() As Boolean
        Get
            If Not Session("IsHardShip_C19") Is Nothing Then
                Return (CType(Session("IsHardShip_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsHardShip_C19") = Value
        End Set
    End Property

    ' To Get / Set Is HardShip flag
    Private Property IsTookTDAccount() As Boolean
        Get
            If Not Session("IsTookTDAccount_C19") Is Nothing Then
                Return (CType(Session("IsTookTDAccount_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsTookTDAccount_C19") = Value
        End Set
    End Property

    'Shubhrata Plan Split Changes
    'Properties for generalising the refund request
    Public Property AddressId() As Integer
        Get
            If Not Session("AddressId_C19") Is Nothing Then
                Return (DirectCast(Session("AddressId_C19"), Integer))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Integer)
            Session("AddressId_C19") = Value
        End Set
    End Property
    Public Property TaxAmount() As Decimal
        Get
            If Not Session("TotalTaxAmount_C19") Is Nothing Then
                Return (DirectCast(Session("TotalTaxAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalTaxAmount_C19") = Value
        End Set
    End Property
    Public Property TaxedAmount() As Decimal
        Get
            If Not Session("TaxedAmount_C19") Is Nothing Then
                Return (DirectCast(Session("TaxedAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TaxedAmount_C19") = Value
        End Set
    End Property
    Public Property NonTaxedAmount() As Decimal
        Get
            If Not Session("NonTaxedAmount_C19") Is Nothing Then
                Return (DirectCast(Session("NonTaxedAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("NonTaxedAmount_C19") = Value
        End Set
    End Property
    Private Property HardshipAvailable() As Decimal
        Get
            If Not Session("HardshipAvailable_C19") Is Nothing Then
                Return (DirectCast(Session("HardshipAvailable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipAvailable_C19") = Value
        End Set
    End Property
    'To Get / Set Maximum Amount Limit for BA age under 55.
    Private Property BAMaxLimit() As Decimal
        Get
            If Not Session("BAMaxLimit_C19") Is Nothing Then
                Return (DirectCast(Session("BAMaxLimit_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("BAMaxLimit_C19") = Value
        End Set
    End Property
    Private Property NumRequestedAmountRetirement() As Decimal
        Get
            If Not Session("RequestedAmountRetirement_C19") Is Nothing Then
                Return (DirectCast(Session("RequestedAmountRetirement_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmountRetirement_C19") = Value
        End Set
    End Property

    Private Property NumRequestedAmountSavings() As Decimal
        Get
            If Not Session("RequestedAmountSavings_C19") Is Nothing Then
                Return (DirectCast(Session("RequestedAmountSavings_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmountSavings_C19") = Value
        End Set
    End Property
    'Added by Parveen on 15-Dec-2009 for the RolloverOptions
    Public Property RolloverOptions() As String
        Get
            If Not Session("RolloverOptions_C19") Is Nothing Then
                Return (DirectCast(Session("RolloverOptions_C19"), String))
            Else
                Return String.Empty

            End If

        End Get
        Set(ByVal Value As String)
            Session("RolloverOptions_C19") = Value
        End Set
    End Property

    Public Property FirstRolloverAmt() As Decimal
        Get
            If Not Session("FirstRolloverAmt_C19") Is Nothing Then
                Return (DirectCast(Session("FirstRolloverAmt_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("FirstRolloverAmt_C19") = Value
        End Set
    End Property

    Public Property TotalRolloverAmt() As Decimal
        Get
            If Not Session("TotalRolloverAmt_C19") Is Nothing Then
                Return (DirectCast(Session("TotalRolloverAmt_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TotalRolloverAmt_C19") = Value
        End Set
    End Property
    'Added by Parveen on 15-Dec-2009 for the RolloverOptions

    ' START | SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
    Public Property IRSOverride() As Boolean
        Get
            If Not String.IsNullOrEmpty(ViewState("IRSOverride")) Then
                Return ViewState("IRSOverride")
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IRSOverride") = Value
        End Set
    End Property
    ' END | SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim

#Region "properties for Hardship_Create Payees"
    Public Property inTaxable() As Decimal
        Get
            If Not Session("inTaxable_C19") Is Nothing Then
                Return (DirectCast(Session("inTaxable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inTaxable_C19") = Value
        End Set
    End Property

    Public Property inNonTaxable() As Decimal
        Get
            If Not Session("inNonTaxable_C19") Is Nothing Then
                Return (DirectCast(Session("inNonTaxable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inNonTaxable_C19") = Value
        End Set
    End Property

    Public Property inGross() As Decimal
        Get
            If Not Session("inGross_C19") Is Nothing Then
                Return (DirectCast(Session("inGross_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inGross_C19") = Value
        End Set
    End Property

    Public Property inVolAcctsUsed() As Decimal
        Get
            If Not Session("inVolAcctsUsed_C19") Is Nothing Then
                Return (DirectCast(Session("inVolAcctsUsed_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inVolAcctsUsed_C19") = Value
        End Set
    End Property

    Public Property inVolAcctsAvailable() As Decimal
        Get
            If Not Session("inVolAcctsAvailable_C19") Is Nothing Then
                Return (DirectCast(Session("inVolAcctsAvailable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inVolAcctsAvailable_C19") = Value
        End Set
    End Property
    'START: SB | 09/02/2016 | YRS-AT-3028 | Property declared to hold current RMD Values
    Private Property ReCalculatedRMDValues() As DataSet
        Get
            If Not (Session("ReCalculatedRMDValuesForWithDrawal_C19")) Is Nothing Then

                Return (DirectCast(Session("ReCalculatedRMDValuesForWithDrawal_C19"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("ReCalculatedRMDValuesForWithDrawal_C19") = Value
        End Set
    End Property
    'END: SB | 09/02/2016 | YRS-AT-3028 | Property declared to hold current RMD Values
#End Region 'properties for Hardship_Create Payees
#End Region
#Region "Modified Functions For Display"
#Region " Voluntry Refund Display"

    Public Function DoVoluntryRefundForDisplay(ByVal parameterCalledBy As String, ByVal parameterboollVoluntryAccounts As Boolean)


        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String

        Me.YMCAAvailableAmount = 0.0
        Me.VoluntryAmount = 0.0
        m_DataTableContributionDataTable = Nothing
        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                End If
            End If


            m_DataTableContributionDataTable = DoFlagSetForVoluntryRefundForDisplay(l_ContributionDataTable, parameterboollVoluntryAccounts, parameterCalledBy)


        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function DoFlagSetForVoluntryRefundForDisplay(ByVal l_ContributionDataTable As DataTable, ByVal parameterboollVoluntryAccounts As Boolean, ByVal parameterCalledBy As String) As DataTable

        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean = True
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String
        If parameterCalledBy = "IsRetirement" Then
            Me.VoluntryAmount_Retirement = 0.0
        End If
        If parameterCalledBy = "IsSavings" Then
            Me.VoluntryAmount_Savings = 0.0
        End If
        Me.YMCAAvailableAmount = 0.0
        Me.VoluntryAmount = 0.0
        m_DataTableContributionDataTable = Nothing
        Try


            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows


                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (((DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total")) Or (((DirectCast(l_DataRow("AccountGroup"), String).Trim = "")))) Then

                            l_UserSide = True
                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then
                                l_UserSide = False
                                l_DataRow("Total") = "0.00"
                                l_DataRow("Taxable") = "0.00"
                                l_DataRow("Non-Taxable") = "0.00"
                                l_DataRow("Interest") = "0.00"
                            End If

                            Select Case (l_AccountGroup.Trim.ToUpper)
                                'Start - Retirement Plan Group
                                Case m_const_RetirementPlan_AM

                                    If Me.IsTerminated = False Then
                                        If l_DataRow("AccountType") = "AM-Matched" Then
                                            l_UserSide = False
                                            l_DataRow("Total") = "0.00"
                                            l_DataRow("Taxable") = "0.00"
                                            l_DataRow("Non-Taxable") = "0.00"
                                            l_DataRow("Interest") = "0.00"
                                        End If
                                    End If

                                Case m_const_RetirementPlan_SR

                                    If Me.IsTerminated = False Then
                                        l_UserSide = False
                                        l_DataRow("Total") = "0.00"
                                        l_DataRow("Taxable") = "0.00"
                                        l_DataRow("Non-Taxable") = "0.00"
                                        l_DataRow("Interest") = "0.00"
                                    End If
                                    'Begin Code Merge by Dilip on 07-05-2009
                                    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new Case for AC account
                                Case m_const_RetirementPlan_AC

                                    If Me.IsTerminated = False Then
                                        l_UserSide = False
                                        l_DataRow("Total") = "0.00"
                                        l_DataRow("Taxable") = "0.00"
                                        l_DataRow("Non-Taxable") = "0.00"
                                        l_DataRow("Interest") = "0.00"
                                    End If
                                    'End Code Merge by Dilip on 07-05-2009
                                Case m_const_SavingsPlan_TD
                                    'modified the if condition to allow the voluntary withdrawal-Amit
                                    If Me.IsTerminated Or Me.PersonAge >= 59.06 Then

                                    Else
                                        l_UserSide = False
                                        l_DataRow("Total") = "0.00"
                                        l_DataRow("Taxable") = "0.00"
                                        l_DataRow("Non-Taxable") = "0.00"
                                        l_DataRow("Interest") = "0.00"
                                    End If

                                    'End If


                                Case m_const_SavingsPlan_TM

                                    If Me.IsTerminated = False Then
                                        If l_DataRow("AccountType") = "TM-Matched" Then
                                            l_UserSide = False
                                            l_DataRow("Total") = "0.00"
                                            l_DataRow("Taxable") = "0.00"
                                            l_DataRow("Non-Taxable") = "0.00"
                                            l_DataRow("Interest") = "0.00"
                                        ElseIf l_DataRow("AccountType") = "TM" Then
                                            'modified the if condition to allow the voluntary withdrawal-Amit
                                            If Me.PersonAge >= 59.06 Then
                                            Else
                                                l_UserSide = False
                                                l_DataRow("Total") = "0.00"
                                                l_DataRow("Taxable") = "0.00"
                                                l_DataRow("Non-Taxable") = "0.00"
                                                l_DataRow("Interest") = "0.00"
                                            End If

                                        End If
                                        'modified the if condition to allow the voluntary withdrawal-Amit
                                    ElseIf Me.IsTerminated = True Or Me.PersonAge >= 59.06 Then

                                        l_UserSide = True
                                    End If

                                    'End If
                            End Select


                            ''Modify the values
                            If parameterCalledBy = "IsRetirement" Then
                                Me.VoluntryAmount_Retirement += DirectCast(l_DataRow("Total"), Decimal)
                            End If
                            If parameterCalledBy = "IsSavings" Then
                                Me.VoluntryAmount_Savings += DirectCast(l_DataRow("Total"), Decimal)
                            End If
                            Me.VoluntryAmount += DirectCast(l_DataRow("Total"), Decimal)
                            If parameterCalledBy <> "IsConsolidated" Then
                                'Begin Code Merge by Dilip on 07-05-2009
                                'Priya 15-Jan-2009 : YRS 5.0-637 AC Account interest components added new Case for AC account
                                'Me.YMCAAvailableAmount += DirectCast(l_DataRow("Taxable"), Decimal)
                                If l_AccountGroup <> m_const_RetirementPlan_AC Then
                                    Me.YMCAAvailableAmount += DirectCast(l_DataRow("Taxable"), Decimal)
                                End If
                                'end 15-Jan-2009
                                'End Code Merge by Dilip on 07-05-2009
                            End If

                            If l_UserSide = False Then
                                l_DataRow.Delete()
                            ElseIf l_UserSide = True Then
                                l_DataRow("Selected") = "True"
                                If l_DataRow("Total") = "0.00" Then
                                    l_DataRow.Delete()
                                End If
                            End If


                        End If

                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()
                DoFlagSetForVoluntryRefundForDisplay = l_ContributionDataTable
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region
#Region "Full Or Pers Refund For Display"


    '' This function for Full Refund.
    Public Function DoFullOrPersRefundForDisplay(ByVal parameterCalledBy As String)

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_AccountGroup As String
        m_DataTableContributionDataTable = Nothing
        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If
                'New Modification for Market Based Withdrawal Amit Nigam 06/oct/2009   
                If Not Session("Calculated_DisplayRetirementPlanAcctContributionforMarket_C19") Is Nothing And Session("blnMarketCheck_C19") = False Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContributionforMarket_C19"), DataTable)
                End If
                'New Modification for Market Based Withdrawal Amit Nigam 06/oct/2009        
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                    l_ContributionDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                End If
            End If


            m_DataTableContributionDataTable = DoFlagSetForFullOrPersRefundForDisplay(l_ContributionDataTable, parameterCalledBy)


        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function DoFlagSetForFullOrPersRefundForDisplay(ByVal l_ContributionDataTable As DataTable, ByVal parameterCalledBy As String) As DataTable
        Dim l_DataRow As DataRow
        Dim l_bool_SelectAcct As Boolean
        Dim l_AccountGroup As String
        Dim l_integer_QDROParticipantAge As Integer
        If parameterCalledBy = "IsRetirement" Then
            Me.YMCAAvailableAmount = 0.0
            Me.HardshipAmount = 0.0
            'Me.HasPersonalMoney = 0.0
        End If
        m_DataTableContributionDataTable = Nothing
        Try

            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    'Reset the flag
                    l_bool_SelectAcct = True

                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (DirectCast(l_DataRow("AccountGroup"), String).Trim = "Total") Then

                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then

                                If Me.RefundType = "PERS" Then
                                    If l_DataRow("AccountType") = m_const_YMCA_Legacy_Acct Then
                                        'BA Account Date 
                                        'l_DataRow("Taxable") = "0.00"
                                        'l_DataRow("Non-Taxable") = "0.00"
                                        'l_DataRow("Interest") = "0.00"
                                        'l_DataRow("Total") = "0.00"
                                        'l_bool_SelectAcct = False
                                    End If

                                ElseIf Me.RefundType = "REG" Then
                                    'BY APARNA 25/10/2007
                                    If Me.SessionStatusType.ToUpper.Trim = "QD" Then
                                        If Not Session("QDROParticipantAge_C19") Is Nothing Then
                                            l_integer_QDROParticipantAge = DirectCast(Session("QDROParticipantAge_C19"), Integer)
                                            If Me.PersonAge >= 55 Or l_integer_QDROParticipantAge >= 50 Then
                                            Else
                                                ' START | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money 
                                                If Not BA_Legacy_combined_Amt_Switch_ON() Then
                                                    If Me.IsVested And Me.TerminationPIA < Me.MaximumPIAAmount Then

                                                    Else
                                                        If l_DataRow("AccountType") = m_const_YMCA_Legacy_Acct Then
                                                            l_DataRow("Taxable") = "0.00"
                                                            l_DataRow("Non-Taxable") = "0.00"
                                                            l_DataRow("Interest") = "0.00"
                                                            l_DataRow("Total") = "0.00"
                                                            l_bool_SelectAcct = False
                                                        End If
                                                        Me.RefundType = "PERS"
                                                    End If
                                                End If
                                                ' END | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money

                                            End If
                                        End If
                                        'BY APARNA 25/10/2007
                                    Else
                                        'BA Ymca Phase V by Dilip
                                        'If Me.IsVested And Me.BATerminationPIA < 5000 Then

                                        'Else
                                        '    If l_DataRow("AccountType") = "BA ER" Then
                                        '        l_DataRow("Taxable") = "0.00"
                                        '        l_DataRow("Non-Taxable") = "0.00"
                                        '        l_DataRow("Interest") = "0.00"
                                        '        l_DataRow("Total") = "0.00"
                                        '        l_bool_SelectAcct = False
                                        '    End If

                                        '    'Me.RefundType = "PERS"
                                        'End If
                                        'BA Ymca Phase V by Dilip 
                                        ' START | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money 
                                        If Not BA_Legacy_combined_Amt_Switch_ON() Then
                                            If Me.IsVested And Me.TerminationPIA > Me.MaximumPIAAmount Then
                                                If l_DataRow("AccountType") = m_const_YMCA_Legacy_Acct Then
                                                    l_DataRow("Taxable") = "0.00"
                                                    l_DataRow("Non-Taxable") = "0.00"
                                                    l_DataRow("Interest") = "0.00"
                                                    l_DataRow("Total") = "0.00"
                                                    l_bool_SelectAcct = False
                                                End If
                                                Me.RefundType = "PERS"
                                            End If
                                        End If
                                        ' END | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money
                                    End If

                                End If
                                If l_bool_SelectAcct = True Then
                                    If l_DataRow("AccountType") = m_const_YMCA_Legacy_Acct Then
                                        If parameterCalledBy = "IsRetirement" Then
                                            Me.YMCAAvailableAmount += DirectCast(l_DataRow("Taxable"), Decimal)
                                        End If
                                    End If
                                    If l_DataRow("AccountType") = "Personal" Then
                                        If parameterCalledBy = "IsRetirement" Then
                                            Me.HasPersonalMoney += DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("Interest"), Decimal)
                                        End If
                                    End If
                                End If
                            End If

                            If l_bool_SelectAcct = True Then
                                l_DataRow("Selected") = "True"
                                If l_DataRow("Total") = "0.00" Then
                                    l_DataRow.Delete()
                                End If
                            Else
                                l_DataRow.Delete()
                            End If


                        End If
                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()
                DoFlagSetForFullOrPersRefundForDisplay = l_ContributionDataTable

            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals 
    'Added two parameter for partial withdrawal
    'Public Function CalculateTotalForDisplay(ByVal l_CalculatedDataTable As DataTable, ByVal bool_check As Boolean, Optional ByVal bIsPartial As Boolean = False, Optional ByVal strTerminated As String = "") As DataTable
    '2013.Oct.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
    'Remove option parameter from function
    Public Function CalculateTotalForDisplay(ByVal l_CalculatedDataTable As DataTable, ByVal bool_check As Boolean) As DataTable

        ' Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_AccountGroup As String
        PersonTotalAmount = 0
        'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
        Dim l_MarketDataRow As DataRow
        Dim l_TotalDataRow As DataRow()
        Dim l_MarketRow As DataRow()
        Dim MarketAccountType As String
        'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
        Try

            If Not l_CalculatedDataTable Is Nothing Then
                For Each l_DataRow In l_CalculatedDataTable.Rows
                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then
                        If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then
                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper
                            'SHubhrata May 19th 2008, to inlude SR And AM as well for Personal Refunds
                            'Begin Code Merge by Dilip on 07-05-2009
                            'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components Added or condition for AC account type
                            ' VPR Changed the if condition for computing the personal total and excluding ymca money.
                            If (IsBasicAccount(l_DataRow) = True And Not (l_DataRow("AccountGroup") = "RBA" Or l_DataRow("AccountGroup") = "REY")) Or l_AccountGroup = m_const_RetirementPlan_AM Or l_AccountGroup = m_const_RetirementPlan_SR Or l_AccountGroup = m_const_RetirementPlan_AC Then
                                'End Code Merge by Dilip on 07-05-2009
                                Me.PersonTotalAmount += DirectCast(l_DataRow("Total"), Decimal)
                                'VPR Commented due to wrong computation of personal total.
                                'Exit For
                            End If
                        End If

                    End If
                Next


                ' If the field Total is already exist in the Table then Delete the Row.
                For Each l_DataRow In l_CalculatedDataTable.Rows
                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then
                        If DirectCast(l_DataRow("AccountType"), String) = "Total" Then
                            l_BooleanFlag = True
                            Exit For
                        End If
                    End If
                Next

                If l_BooleanFlag = False Then
                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                Else
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("Non-Taxable") = "0.00"
                    l_DataRow("Interest") = "0.00"
                    l_DataRow("Total") = "0.00"
                End If


                ' Phase V Changes Dilip, to inlcude Personal refund for computing total based on selected check box
                'If Me.RefundType = "VOL" Then
                'Modified By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                'IB:As on 21/June/2010 BT-545:Casting problem
                'MarketAccountType = "Market Based (" + DirectCast(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
                MarketAccountType = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
                If Me.RefundType = "VOL" Or Me.RefundType = "PERS" Then
                    l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "Selected=True And AccountType <> '" + MarketAccountType + "'")
                    l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "Selected=True And AccountType <> '" + MarketAccountType + "'")
                    l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "Selected=True And AccountType <> '" + MarketAccountType + "'")
                    If bool_check = True Then
                        l_DataRow("TaxWithheld") = l_CalculatedDataTable.Compute("SUM (TaxWithheld)", "Selected=True And AccountType <> '" + MarketAccountType + "'")
                    End If
                    l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "Selected=True And AccountType <> '" + MarketAccountType + "'")
                Else
                    'Code Commented : '2013.Oct.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                    'If bIsPartial And strTerminated.ToUpper = "YES" Then
                    '    l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "AccountType <> '" + MarketAccountType + "'")
                    '    l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "AccountType <> '" + MarketAccountType + "'")
                    '    l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "AccountType <> '" + MarketAccountType + "'")
                    '    If bool_check = True Then
                    '        l_DataRow("TaxWithheld") = l_CalculatedDataTable.Compute("SUM (TaxWithheld)", "Selected=True And AccountType <> '" + MarketAccountType + "'")
                    '    End If
                    '    l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "AccountType <> '" + MarketAccountType + "'")
                    'ElseIf bIsPartial And strTerminated.ToUpper = "NO" Then
                    '    l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'TD' ")
                    '    l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'TD' ")
                    '    l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'TD' ")
                    '    If bool_check = True Then
                    '        l_DataRow("TaxWithheld") = l_CalculatedDataTable.Compute("SUM (TaxWithheld)", "Selected=True And AccountType <> '" + MarketAccountType + "' AND AccountType <> 'TD' ")
                    '    End If
                    '    l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'TD' ")
                    'Else
                    'Code Added : 2013.Oct.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                    l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'LN' AND AccountType <> 'LD'")
                    l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'LN' AND AccountType <> 'LD'")
                    l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'LN' AND AccountType <> 'LD'")
                    If bool_check = True Then
                        l_DataRow("TaxWithheld") = l_CalculatedDataTable.Compute("SUM (TaxWithheld)", "Selected=True And AccountType <> '" + MarketAccountType + "' AND AccountType <> 'LN' AND AccountType <> 'LD'")
                    End If
                    l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "AccountType <> '" + MarketAccountType + "' AND AccountType <> 'LN' AND AccountType <> 'LD'")
                End If
                'End If
                'End: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals
                'Modified By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009

                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Taxable") = "0.00"
                End If

                If l_DataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Non-Taxable") = "0.00"
                End If

                If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Interest") = "0.00"
                End If

                If l_DataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Total") = "0.00"
                End If
                If bool_check = True Then
                    If l_DataRow("TaxWithheld").GetType.ToString = "System.DBNull" Then
                        l_DataRow("TaxWithheld") = "0.00"
                    End If
                End If

                ''Set the Total Amount .
                If (l_DataRow("Total").GetType.ToString = "System.DBNull") Then
                    Me.TotalAmount = 0.0
                Else
                    Me.TotalAmount = DirectCast(l_DataRow("Total"), Decimal)
                End If

                '' Set The emp . Toatal Amount


                If l_BooleanFlag = False Then
                    l_DataRow("AccountType") = "Total"
                    l_CalculatedDataTable.Rows.Add(l_DataRow)
                End If

                If Me.RefundType <> "SPEC" Or Me.RefundType <> "DISAB" Or Me.RefundType = "HARD" Then
                    'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                    l_TotalDataRow = l_CalculatedDataTable.Select("AccountType='Total'")
                    If l_TotalDataRow.Length > 0 Then
                        If DirectCast(l_TotalDataRow(0)("Total"), Decimal) > DirectCast(Session("MarketBasedThreshold_C19"), Decimal) And Me.AllowMarketBased = True And Me.RefundType <> "HARD" Then
                            l_MarketRow = l_CalculatedDataTable.Select("AccountType='" + MarketAccountType + "'")
                            If l_MarketRow.Length = 0 Then
                                l_MarketDataRow = l_CalculatedDataTable.NewRow()
                                'IB:As on 21/June/2010 BT-545:Casting problem
                                'l_MarketDataRow("AccountType") = "Market Based (" + DirectCast(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
                                l_MarketDataRow("AccountType") = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
                                l_MarketDataRow("Taxable") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Taxable") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                l_MarketDataRow("Non-Taxable") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Non-Taxable") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                l_MarketDataRow("Interest") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Interest") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))

                                If bool_check = True Then
                                    l_MarketDataRow("TaxWithheld") = Convert.ToDecimal(Format(l_TotalDataRow(0)("TaxWithheld") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                End If
                                'commented and modified to adjust the rounding issues-17/11/2009 Amit
                                '_MarketDataRow("Total") = l_TotalDataRow(0)("Total") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100)
                                'l_MarketDataRow("Total") = l_MarketDataRow("Taxable") + l_MarketDataRow("Non-Taxable") + l_MarketDataRow("Interest")
                                l_MarketDataRow("Total") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Total") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))

                                'commented and modified to adjust the rounding issues-17/11/2009 Amit
                                Dim totalamount As Decimal

                                totalamount = l_MarketDataRow("Taxable") + l_MarketDataRow("Non-Taxable") + l_MarketDataRow("Interest")
                                ' totalamount = adjustAmountforRoundingValueinMarket(l_CalculatedDataTable)
                                If Convert.ToDecimal(l_MarketDataRow("Total")) <> totalamount Then
                                    Dim diff As Double
                                    'diff = totalamount - Convert.ToDecimal(l_MarketDataRow("Total"))
                                    diff = Convert.ToDouble(l_MarketDataRow("Total")) - totalamount
                                    'If l_MarketDataRow("Total") > totalamount Then
                                    '    diff = Convert.ToDouble(l_MarketDataRow("Total")) - totalamount
                                    '    'diff = totalamount - Convert.ToDouble(l_MarketDataRow("Total"))
                                    'Else
                                    '    'diff = Convert.ToDouble(l_MarketDataRow("Total")) - totalamount
                                    '    diff = totalamount - Convert.ToDouble(l_MarketDataRow("Total"))
                                    'End If
                                    If l_MarketDataRow("Taxable") > 0 Then
                                        l_MarketDataRow("Taxable") = l_MarketDataRow("Taxable") + diff
                                        'l_MarketDataRow("Total") = l_MarketDataRow("Total") + diff
                                    ElseIf l_MarketDataRow("Non-Taxable") > 0 Then
                                        l_MarketDataRow("Non-Taxable") = l_MarketDataRow("Non-Taxable") + diff
                                        'l_MarketDataRow("Total") = l_MarketDataRow("Total") + diff
                                    ElseIf l_MarketDataRow("Interest") > 0 Then
                                        l_MarketDataRow("Interest") = l_MarketDataRow("Interest") + diff
                                        'l_MarketDataRow("Total") = l_MarketDataRow("Total") + diff
                                    End If
                                End If

                                'commented and modified to adjust the rounding issues-17/11/2009 Amit
                                l_CalculatedDataTable.Rows.Add(l_MarketDataRow)
                            Else
                                'l_MarketRow(0)("AccountType") = "Market Based (" + DirectCast(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
                                l_MarketRow(0)("AccountType") = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
                                l_MarketRow(0)("Taxable") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Taxable") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                l_MarketRow(0)("Non-Taxable") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Non-Taxable") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                l_MarketRow(0)("Interest") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Interest") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                If bool_check = True Then
                                    l_MarketRow(0)("TaxWithheld") = Convert.ToDecimal(Format(l_TotalDataRow(0)("TaxWithheld") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                                End If
                                'commented and modified to adjust the rounding issues-17/11/2009 Amit
                                'l_MarketRow(0)("Total") = l_TotalDataRow(0)("Total") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100)
                                'l_MarketRow(0)("Total") = l_MarketRow(0)("Taxable") + l_MarketRow(0)("Non-Taxable") + l_MarketRow(0)("Interest")
                                l_MarketRow(0)("Total") = Convert.ToDecimal(Format(l_TotalDataRow(0)("Total") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))

                                Dim totalamount As Decimal
                                totalamount = l_MarketRow(0)("Taxable") + l_MarketRow(0)("Non-Taxable") + l_MarketRow(0)("Interest")
                                'totalamount = adjustAmountforRoundingValueinMarket(l_CalculatedDataTable)
                                If Convert.ToDecimal(l_MarketRow(0)("Total")) <> totalamount Then
                                    Dim diff As Double
                                    diff = Convert.ToDouble(l_MarketRow(0)("Total")) - totalamount
                                    'diff = totalamount - Convert.ToDecimal(l_MarketRow(0)("Total"))
                                    'If l_MarketRow(0)("Total") > totalamount Then
                                    '    diff = totalamount - Convert.ToDouble(l_MarketRow(0)("Total"))
                                    'Else
                                    '    diff = Convert.ToDouble(l_MarketRow(0)("Total")) - totalamount
                                    'End If
                                    'If l_MarketRow(0)("Total") > totalamount Then
                                    '    diff = Convert.ToDouble(l_MarketRow(0)("Total")) - totalamount
                                    '    'diff = totalamount - Convert.ToDouble(l_MarketDataRow("Total"))
                                    'Else
                                    '    'diff = Convert.ToDouble(l_MarketDataRow("Total")) - totalamount
                                    '    diff = totalamount - Convert.ToDouble(l_MarketRow(0)("Total"))
                                    'End If
                                    If l_MarketRow(0)("Taxable") > 0 Then
                                        l_MarketRow(0)("Taxable") = l_MarketRow(0)("Taxable") + diff
                                        'l_MarketRow(0)("Total") = l_MarketRow(0)("Total") + diff
                                    ElseIf l_MarketRow(0)("Non-Taxable") > 0 Then
                                        l_MarketRow(0)("Non-Taxable") = l_MarketRow(0)("Non-Taxable") + diff
                                        'l_MarketRow(0)("Total") = l_MarketRow(0)("Total") + diff
                                    ElseIf l_MarketRow(0)("Interest") > 0 Then
                                        l_MarketRow(0)("Interest") = l_MarketRow(0)("Interest") + diff
                                        'l_MarketRow(0)("Total") = l_MarketRow(0)("Total") + diff
                                    End If
                                End If

                                'commented and modified to adjust the rounding issues-17/11/2009 Amit
                            End If

                        Else

                            If l_CalculatedDataTable.Rows(l_CalculatedDataTable.Rows.Count - 1)("AccountType") = MarketAccountType Then
                                l_CalculatedDataTable.Rows.RemoveAt(l_CalculatedDataTable.Rows.Count - 1)
                            End If
                        End If
                    End If


                    'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                End If

                CalculateTotalForDisplay = l_CalculatedDataTable
            End If
        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function adjustAmountforRoundingValueinMarket(ByVal l_CalculatedDataTable As DataTable) As Double
        Try
            Dim totalamount As Decimal = 0.0
            For Each drow As DataRow In l_CalculatedDataTable.Rows
                If drow("Selected").GetType.ToString() <> "System.DBNull" Then
                    If drow("AccountType").ToString.Trim() <> "Market Based (10%)" And drow("AccountType").ToString.ToUpper.Trim() = "TOTAL" And drow("AccountType").GetType.ToString() <> "System.DBNull" And drow("Selected") = True Then
                        totalamount += Convert.ToDecimal(Format(drow("Taxable") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00")) + Convert.ToDecimal(Format(drow("Non-Taxable") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00")) + Convert.ToDecimal(Format(drow("Interest") * (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal) / 100), "0.00"))
                    End If
                End If
            Next
            Return totalamount
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
    'This fucntion is NOT USED for display the Requested record to the Requested Datagrid in the Refund Processing screen.
    Public Function CreatDisplayTables(ByVal parameterDatatableRetirement As DataTable)
        Try
            Dim l_dt_DisplayRetirementPlan As New DataTable
            Dim l_dt_DisplaySavingsPlan As New DataTable
            Dim l_dt_DisplayConsolidatedTotal As New DataTable
            Dim DataRow As DataRow
            Dim l_dr_DisplayRetirementPlan As DataRow
            Dim l_dr_DisplaySavingsPlan As DataRow
            Dim l_dr_DisplayConsolidatedTotal As DataRow
            Dim l_decimalTaxable As Decimal = 0.0
            Dim l_decimalNonTaxable As Decimal = 0.0
            Dim l_decimalInterest As Decimal = 0.0
            Dim l_decimalTotal As Decimal = 0.0
            Dim l_decimalYMCATaxable As Decimal = 0.0
            Dim l_decimalYMCAInterest As Decimal = 0.0
            Dim l_decimalYMCATotal As Decimal = 0.0
            If Not parameterDatatableRetirement Is Nothing Then
                l_dt_DisplayRetirementPlan.Columns.Add("IsBasicAccount", System.Type.GetType("System.Boolean"))
                l_dt_DisplayRetirementPlan.Columns.Add("AccountType", System.Type.GetType("System.String"))
                l_dt_DisplayRetirementPlan.Columns.Add("AccountGroup", System.Type.GetType("System.String"))
                l_dt_DisplayRetirementPlan.Columns.Add("PlanType", System.Type.GetType("System.String"))
                l_dt_DisplayRetirementPlan.Columns.Add("Taxable", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("Non-Taxable", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("Interest", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("Total", System.Type.GetType("System.Decimal"))

                If parameterDatatableRetirement.Rows.Count > 0 Then
                    For Each DataRow In parameterDatatableRetirement.Rows
                        If DataRow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                            If CType(DataRow("IsBasicAccount"), Boolean) = True Then

                                If DirectCast(DataRow("Emp.Total"), Decimal) > 0 Then
                                    l_decimalTaxable += DirectCast(DataRow("Taxable"), Decimal)
                                    l_decimalNonTaxable += DirectCast(DataRow("Non-Taxable"), Decimal)
                                    l_decimalInterest += DirectCast(DataRow("Interest"), Decimal)
                                    l_decimalTotal += DirectCast(DataRow("Emp.Total"), Decimal)
                                End If

                                If DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent > 5000 And Me.PersonAge < 55 Then
                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent <= 5000 And Me.TerminationPIA > 25000 And Me.PersonAge < 55 Then
                                    l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                    'l_dr_DisplayRetirementPlan("AccountType") = "BA ER" -----for YMCA (Legacy) Acct
                                    l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                    l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                    l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                    l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                    l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                    l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                    l_dr_DisplayRetirementPlan("Total") = DataRow("YMCATotal")
                                    l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent <= 25000 And Me.TerminationPIA <= 25000 And Me.PersonAge > 55 Then
                                    l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                    'l_dr_DisplayRetirementPlan("AccountType") = "BA ER" -----for YMCA (Legacy) Acct
                                    l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                    l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                    l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                    l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                    l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                    l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                    l_dr_DisplayRetirementPlan("Total") = DataRow("YMCATotal")
                                    l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent <= 5000 And Me.TerminationPIA <= 25000 And Me.PersonAge < 55 Then
                                    l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                    'l_dr_DisplayRetirementPlan("AccountType") = "BA ER" -----for YMCA (Legacy) Acct
                                    l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                    l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                    l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                    l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                    l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                    l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                    l_dr_DisplayRetirementPlan("Total") = DataRow("YMCATotal")
                                    l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent > 5000 And Me.TerminationPIA <= 25000 And Me.PersonAge > 55 Then

                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent <= 5000 And Me.TerminationPIA > 5000 And Me.PersonAge < 55 Then

                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent <= 25000 And Me.PersonAge > 55 Then
                                    l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                    'l_dr_DisplayRetirementPlan("AccountType") = "BA ER" -----for YMCA (Legacy) Acct
                                    l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                    l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                    l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                    l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                    l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                    l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                    l_dr_DisplayRetirementPlan("Total") = DataRow("YMCATotal")
                                    l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And Me.BACurrent <= 5000 Then
                                    l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                    'l_dr_DisplayRetirementPlan("AccountType") = "BA ER" -----for YMCA (Legacy) Acct
                                    l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                    l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                    l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                    l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                    l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                    l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                    l_dr_DisplayRetirementPlan("Total") = DataRow("YMCATotal")
                                    l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) > 0 Then
                                    l_decimalYMCATaxable += DirectCast(DataRow("YMCATaxable"), Decimal)
                                    l_decimalYMCAInterest += DirectCast(DataRow("YMCAInterest"), Decimal)
                                    l_decimalYMCATotal += DirectCast(DataRow("YMCATotal"), Decimal)
                                End If
                            Else
                                If DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_AM Then
                                    If DirectCast(DataRow("Emp.Total"), Decimal) > 0 Then
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = "AM"
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = DataRow("Taxable")
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = DataRow("Non-Taxable")
                                        l_dr_DisplayRetirementPlan("Interest") = DataRow("Interest")
                                        l_dr_DisplayRetirementPlan("Total") = DataRow("Emp.Total")
                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                    End If
                                    If DirectCast(DataRow("YMCATotal"), Decimal) > 0 Then
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = "AM-Matched"
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                        l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                        l_dr_DisplayRetirementPlan("Total") = DataRow("YMCATotal")
                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                                    End If
                                Else
                                    If DirectCast(DataRow("YMCATotal"), Decimal) > 0 Then
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = DataRow("AccountType")
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = DataRow("YMCATaxable")
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                        l_dr_DisplayRetirementPlan("Interest") = DataRow("YMCAInterest")
                                        l_dr_DisplayRetirementPlan("Total") = DataRow("Total")
                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                                    Else
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = DataRow("AccountType")
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = DataRow("Taxable")
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = DataRow("Non-Taxable")
                                        l_dr_DisplayRetirementPlan("Interest") = DataRow("Interest")
                                        l_dr_DisplayRetirementPlan("Total") = DataRow("Total")

                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    If l_decimalTotal > 0 Then
                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                        l_dr_DisplayRetirementPlan("AccountType") = "Personal"
                        l_dr_DisplayRetirementPlan("AccountGroup") = "REP" 'todo
                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                        l_dr_DisplayRetirementPlan("IsBasicAccount") = "True"
                        l_dr_DisplayRetirementPlan("Taxable") = l_decimalTaxable
                        l_dr_DisplayRetirementPlan("Non-Taxable") = l_decimalNonTaxable
                        l_dr_DisplayRetirementPlan("Interest") = l_decimalInterest
                        l_dr_DisplayRetirementPlan("Total") = l_decimalTotal
                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                    End If
                    If l_decimalYMCATotal > 0 And Me.BACurrent <= 25000 And Me.TerminationPIA > 25000 And Me.PersonAge > 55 Then
                    ElseIf l_decimalYMCATotal > 0 And Me.BACurrent > 25000 And Me.TerminationPIA > 25000 And Me.PersonAge > 55 Then
                    ElseIf l_decimalYMCATotal > 0 And Me.BACurrent > 5000 And Me.TerminationPIA > 25000 And Me.PersonAge < 55 Then
                    ElseIf l_decimalYMCATotal > 0 And Me.BACurrent <= 5000 And Me.TerminationPIA > 25000 And Me.PersonAge < 55 Then
                    ElseIf l_decimalYMCATotal > 0 Then
                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                        'l_dr_DisplayRetirementPlan("AccountType") = "YMCA" ---“YMCA (Legacy) Acct” 
                        l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA_Legacy_Acct
                        l_dr_DisplayRetirementPlan("AccountGroup") = "REY" 'todo
                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                        l_dr_DisplayRetirementPlan("IsBasicAccount") = "True"
                        l_dr_DisplayRetirementPlan("Taxable") = l_decimalYMCATaxable
                        l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                        l_dr_DisplayRetirementPlan("Interest") = l_decimalYMCAInterest
                        l_dr_DisplayRetirementPlan("Total") = l_decimalYMCATotal
                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                    End If
                End If
            End If
            l_dt_DisplayRetirementPlan.AcceptChanges()
            Return l_dt_DisplayRetirementPlan
        Catch ex As Exception
            Throw
        End Try
        'This fucntion is NOT USED for display the Requested record to the Requested Datagrid in the Refund Processing screen.
    End Function
    '986
    Public Function CreateDisplayDatatables(ByVal parameterDatatableRetirement As DataTable, ByVal parameterDatatableSavings As DataTable, ByVal parameterNumPercentageFactorSavingsPlan As Decimal, ByVal parameterNumPercentageFactorRetirementPlan As Decimal)
        Try

            Dim l_dt_DisplayRetirementPlan As New DataTable
            Dim l_dt_DisplaySavingsPlan As New DataTable
            Dim l_dt_DisplayConsolidatedTotal As New DataTable
            Dim DataRow As DataRow
            Dim l_dr_DisplayRetirementPlan As DataRow
            Dim l_dr_DisplaySavingsPlan As DataRow
            Dim l_dr_DisplayConsolidatedTotal As DataRow
            Dim l_decimalTaxable As Decimal = 0.0
            Dim l_decimalNonTaxable As Decimal = 0.0
            Dim l_decimalInterest As Decimal = 0.0
            Dim l_decimalTotal As Decimal = 0.0
            Dim l_decimalYMCATaxable As Decimal = 0.0
            Dim l_decimalYMCAInterest As Decimal = 0.0
            Dim l_decimalYMCATotal As Decimal = 0.0
            Me.NumPercentageFactorofMoneyRetirement = parameterNumPercentageFactorRetirementPlan
            Me.NumPercentageFactorofMoneySavings = parameterNumPercentageFactorSavingsPlan
            If Not parameterDatatableRetirement Is Nothing Then

                l_dt_DisplayRetirementPlan.Columns.Add("IsBasicAccount", System.Type.GetType("System.Boolean"))
                'l_dt_DisplayRetirementPlan.Columns("Selected").DefaultValue = True
                l_dt_DisplayRetirementPlan.Columns.Add("AccountType", System.Type.GetType("System.String"))
                l_dt_DisplayRetirementPlan.Columns.Add("AccountGroup", System.Type.GetType("System.String"))
                l_dt_DisplayRetirementPlan.Columns.Add("PlanType", System.Type.GetType("System.String"))
                l_dt_DisplayRetirementPlan.Columns.Add("Taxable", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("Non-Taxable", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("Interest", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("TaxWithheld", System.Type.GetType("System.Decimal"))
                l_dt_DisplayRetirementPlan.Columns.Add("Total", System.Type.GetType("System.Decimal"))
                'set default values



                If parameterDatatableRetirement.Rows.Count > 0 Then
                    For Each DataRow In parameterDatatableRetirement.Rows
                        'IsBasicAccount
                        If DataRow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                            If CType(DataRow("IsBasicAccount"), Boolean) = True Then
                                'Emp.Total 
                                If DirectCast(DataRow("Emp.Total"), Decimal) > 0 Then
                                    l_decimalTaxable += DirectCast(DataRow("Taxable"), Decimal)
                                    l_decimalNonTaxable += DirectCast(DataRow("Non-Taxable"), Decimal)
                                    l_decimalInterest += DirectCast(DataRow("Interest"), Decimal)
                                    l_decimalTotal += DirectCast(DataRow("Emp.Total"), Decimal)
                                End If

                                If Me.IsVested = True And ((DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And DirectCast(DataRow("YMCATotal"), Decimal) <= Me.BAMaxLimit) Or _
                                    (DirectCast(DataRow("YMCATotal"), Decimal) > 0 And DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA And (Me.RefundType.Trim.ToUpper = "DISAB" Or Me.RefundType.Trim.ToUpper = "SPEC"))) Then

                                    l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                    'l_dr_DisplayRetirementPlan("AccountType") = "BA ER" -----for YMCA (Legacy) Acct
                                    l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                    l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                    l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                    l_dr_DisplayRetirementPlan("Taxable") = Math.Round((DataRow("YMCATaxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                    l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                    l_dr_DisplayRetirementPlan("Interest") = Math.Round((DataRow("YMCAInterest") * parameterNumPercentageFactorRetirementPlan), 2)
                                    'l_dr_DisplayRetirementPlan("Total") = Math.Round((DataRow("YMCATotal") * parameterNumPercentageFactorRetirementPlan), 2)
                                    l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                                    l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)
                                    l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                                ElseIf DirectCast(DataRow("YMCATotal"), Decimal) And Not (DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_BA) Then
                                    l_decimalYMCATaxable += DirectCast(DataRow("YMCATaxable"), Decimal)
                                    l_decimalYMCAInterest += DirectCast(DataRow("YMCAInterest"), Decimal)
                                    l_decimalYMCATotal += DirectCast(DataRow("YMCATotal"), Decimal)
                                End If
                            Else

                                If DirectCast(DataRow("AccountGroup"), String) = m_const_RetirementPlan_AM Then
                                    If DirectCast(DataRow("Emp.Total"), Decimal) > 0 Then
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = "AM"
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((DataRow("Taxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = Math.Round((DataRow("Non-Taxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((DataRow("Interest") * parameterNumPercentageFactorRetirementPlan), 2)
                                        'l_dr_DisplayRetirementPlan("Total") = Math.Round((DataRow("Emp.Total") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                                        l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)

                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                    End If
                                    If DirectCast(DataRow("YMCATotal"), Decimal) > 0 Then
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = "AM-Matched"
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((DataRow("YMCATaxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((DataRow("YMCAInterest") * parameterNumPercentageFactorRetirementPlan), 2)
                                        'l_dr_DisplayRetirementPlan("Total") = Math.Round((DataRow("YMCATotal") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                                        l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)


                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                                    End If
                                Else
                                    If DirectCast(DataRow("YMCATotal"), Decimal) > 0 Then
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = DataRow("AccountType")
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((DataRow("YMCATaxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((DataRow("YMCAInterest") * parameterNumPercentageFactorRetirementPlan), 2)
                                        'l_dr_DisplayRetirementPlan("Total") = Math.Round((DataRow("Total") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                                        l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)

                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                                    Else
                                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                                        l_dr_DisplayRetirementPlan("AccountType") = DataRow("AccountType")
                                        l_dr_DisplayRetirementPlan("AccountGroup") = DataRow("AccountGroup")
                                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                                        l_dr_DisplayRetirementPlan("IsBasicAccount") = DataRow("IsBasicAccount")
                                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((DataRow("Taxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("Non-Taxable") = Math.Round((DataRow("Non-Taxable") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((DataRow("Interest") * parameterNumPercentageFactorRetirementPlan), 2)
                                        'l_dr_DisplayRetirementPlan("Total") = Math.Round((DataRow("Total") * parameterNumPercentageFactorRetirementPlan), 2)
                                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                                        l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)


                                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                                    End If
                                End If
                            End If
                        End If

                    Next

                    If l_decimalTotal > 0 Then
                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                        l_dr_DisplayRetirementPlan("AccountType") = "Personal"
                        l_dr_DisplayRetirementPlan("AccountGroup") = "REP" 'todo
                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                        l_dr_DisplayRetirementPlan("IsBasicAccount") = "True"
                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((l_decimalTaxable * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("Non-Taxable") = Math.Round((l_decimalNonTaxable * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((l_decimalInterest * parameterNumPercentageFactorRetirementPlan), 2)
                        'l_dr_DisplayRetirementPlan("Total") = Math.Round((l_decimalTotal * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                        l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)


                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)

                    End If
                    'commented the code to exclude the YMCA account. May 20 2009 
                    'If l_decimalYMCATotal > 0 Then 
                    If Me.IsVested = True And ((l_decimalYMCATotal > 0 And Me.TerminationPIA <= Me.MaximumPIAAmount) Or _
                          (l_decimalYMCATotal > 0 And (Me.RefundType.Trim.ToUpper = "DISAB" Or Me.RefundType.Trim.ToUpper = "SPEC"))) Then

                        l_dr_DisplayRetirementPlan = l_dt_DisplayRetirementPlan.NewRow()
                        l_dr_DisplayRetirementPlan("AccountType") = m_const_YMCA_Legacy_Acct
                        l_dr_DisplayRetirementPlan("AccountGroup") = "REY" 'todo
                        l_dr_DisplayRetirementPlan("PlanType") = "Retirement"
                        l_dr_DisplayRetirementPlan("IsBasicAccount") = "True"
                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((l_decimalYMCATaxable * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("Non-Taxable") = 0
                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((l_decimalYMCAInterest * parameterNumPercentageFactorRetirementPlan), 2)
                        'l_dr_DisplayRetirementPlan("Total") = Math.Round((l_decimalYMCATotal * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round((((l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Interest") * parameterNumPercentageFactorRetirementPlan) * Me.FederalTaxRate) / 100), 2)
                        l_dr_DisplayRetirementPlan("Total") = Math.Round(l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest"), 2)

                        l_dt_DisplayRetirementPlan.Rows.Add(l_dr_DisplayRetirementPlan)
                    End If
                End If
            End If



            'If Not parameterDatatableSavings Is Nothing Then
            l_dt_DisplaySavingsPlan.Columns.Add("IsBasicAccount", System.Type.GetType("System.Boolean"))
            'l_dt_DisplaySavingsPlan.Columns("Selected").DefaultValue = True
            l_dt_DisplaySavingsPlan.Columns.Add("AccountType", System.Type.GetType("System.String"))
            l_dt_DisplaySavingsPlan.Columns.Add("AccountGroup", System.Type.GetType("System.String"))
            l_dt_DisplaySavingsPlan.Columns.Add("PlanType", System.Type.GetType("System.String"))
            l_dt_DisplaySavingsPlan.Columns.Add("Taxable", System.Type.GetType("System.Decimal"))
            l_dt_DisplaySavingsPlan.Columns.Add("Non-Taxable", System.Type.GetType("System.Decimal"))
            l_dt_DisplaySavingsPlan.Columns.Add("Interest", System.Type.GetType("System.Decimal"))
            l_dt_DisplaySavingsPlan.Columns.Add("TaxWithheld", System.Type.GetType("System.Decimal"))
            l_dt_DisplaySavingsPlan.Columns.Add("Total", System.Type.GetType("System.Decimal"))
            'l_dr_DisplaySavingsPlan = l_dt_DisplaySavingsPlan.NewRow()
            'l_dt_DisplaySavingsPlan.Rows.Add(l_dr_DisplaySavingsPlan)
            'Changes done for partial Refund
            If parameterDatatableSavings.Rows.Count > 0 Then
                For Each drow As DataRow In parameterDatatableSavings.Rows
                    If drow("AccountGroup").GetType.ToString <> "System.DBNull" Then
                        If DirectCast(drow("AccountGroup"), String) = m_const_SavingsPlan_TM Then
                            If DirectCast(drow("Emp.Total"), Decimal) > 0 Then
                                l_dr_DisplaySavingsPlan = l_dt_DisplaySavingsPlan.NewRow()
                                l_dr_DisplaySavingsPlan("AccountType") = "TM"
                                l_dr_DisplaySavingsPlan("AccountGroup") = drow("AccountGroup")
                                l_dr_DisplaySavingsPlan("PlanType") = drow("PlanType")
                                l_dr_DisplaySavingsPlan("IsBasicAccount") = drow("IsBasicAccount")
                                l_dr_DisplaySavingsPlan("Taxable") = Math.Round((drow("Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                                l_dr_DisplaySavingsPlan("Non-Taxable") = Math.Round((drow("Non-Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                                l_dr_DisplaySavingsPlan("Interest") = Math.Round((drow("Interest") * parameterNumPercentageFactorSavingsPlan), 2)
                                'l_dr_DisplaySavingsPlan("Total") = Math.Round((drow("Emp.Total") * parameterNumPercentageFactorSavingsPlan), 2)
                                l_dr_DisplaySavingsPlan("Total") = Math.Round(l_dr_DisplaySavingsPlan("Taxable") + l_dr_DisplaySavingsPlan("Non-Taxable") + l_dr_DisplaySavingsPlan("Interest"), 2)

                                l_dt_DisplaySavingsPlan.Rows.Add(l_dr_DisplaySavingsPlan)
                            End If
                            If DirectCast(drow("YMCATotal"), Decimal) > 0 Then
                                l_dr_DisplaySavingsPlan = l_dt_DisplaySavingsPlan.NewRow()
                                l_dr_DisplaySavingsPlan("AccountType") = "TM-Matched"
                                l_dr_DisplaySavingsPlan("AccountGroup") = drow("AccountGroup")
                                l_dr_DisplaySavingsPlan("PlanType") = drow("PlanType")
                                l_dr_DisplaySavingsPlan("IsBasicAccount") = drow("IsBasicAccount")
                                l_dr_DisplaySavingsPlan("Taxable") = Math.Round((drow("YMCATaxable") * parameterNumPercentageFactorSavingsPlan), 2)
                                l_dr_DisplaySavingsPlan("Non-Taxable") = 0
                                l_dr_DisplaySavingsPlan("Interest") = Math.Round((drow("YMCAInterest") * parameterNumPercentageFactorSavingsPlan), 2)
                                'l_dr_DisplaySavingsPlan("Total") = Math.Round((drow("YMCATotal") * parameterNumPercentageFactorSavingsPlan), 2)
                                l_dr_DisplaySavingsPlan("Total") = Math.Round(l_dr_DisplaySavingsPlan("Taxable") + l_dr_DisplaySavingsPlan("Non-Taxable") + l_dr_DisplaySavingsPlan("Interest"), 2)

                                l_dt_DisplaySavingsPlan.Rows.Add(l_dr_DisplaySavingsPlan)

                            End If
                        Else
                            l_dr_DisplaySavingsPlan = l_dt_DisplaySavingsPlan.NewRow()
                            l_dr_DisplaySavingsPlan("AccountType") = drow("AccountType")
                            l_dr_DisplaySavingsPlan("AccountGroup") = drow("AccountGroup")
                            l_dr_DisplaySavingsPlan("PlanType") = drow("PlanType")
                            l_dr_DisplaySavingsPlan("IsBasicAccount") = drow("IsBasicAccount")
                            If l_dr_DisplaySavingsPlan("AccountType") = "TD" Then
                                Dim decTDTotalAmount As Decimal
                                Me.Session("TDTotalAmount_C19") = Math.Round(drow("Total"), 2)
                            End If
                            l_dr_DisplaySavingsPlan("Taxable") = Math.Round((drow("Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                            l_dr_DisplaySavingsPlan("Non-Taxable") = Math.Round((drow("Non-Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                            l_dr_DisplaySavingsPlan("Interest") = Math.Round((drow("Interest") * parameterNumPercentageFactorSavingsPlan), 2)
                            'l_dr_DisplaySavingsPlan("Total") = Math.Round((drow("Total") * parameterNumPercentageFactorSavingsPlan), 2)
                            If Me.RefundType.Trim = "HARD" Then
                                If l_dr_DisplaySavingsPlan("AccountType") = "TD" Then

                                    Dim FedTaxrate As Decimal
                                    FedTaxrate = 10

                                    l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round((((l_dr_DisplaySavingsPlan("Taxable") * parameterNumPercentageFactorSavingsPlan) * FedTaxrate) / 100), 2)
                                Else
                                    l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round((((l_dr_DisplaySavingsPlan("Taxable") + l_dr_DisplaySavingsPlan("Interest") * parameterNumPercentageFactorSavingsPlan) * Me.FederalTaxRate) / 100), 2)
                                End If
                            Else
                                l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round((((l_dr_DisplaySavingsPlan("Taxable") + l_dr_DisplaySavingsPlan("Interest") * parameterNumPercentageFactorSavingsPlan) * Me.FederalTaxRate) / 100), 2)
                            End If
                            l_dr_DisplaySavingsPlan("Total") = Math.Round(l_dr_DisplaySavingsPlan("Taxable") + l_dr_DisplaySavingsPlan("Non-Taxable") + l_dr_DisplaySavingsPlan("Interest"), 2)


                            l_dt_DisplaySavingsPlan.Rows.Add(l_dr_DisplaySavingsPlan)

                        End If
                    End If
                Next

                'End If

            End If
            'Changes done for partial Refund
            l_dt_DisplayRetirementPlan.AcceptChanges()
            l_dt_DisplaySavingsPlan.AcceptChanges()

            l_dt_DisplayConsolidatedTotal = l_dt_DisplaySavingsPlan.Clone()
            For Each dr As DataRow In l_dt_DisplayRetirementPlan.Rows
                l_dt_DisplayConsolidatedTotal.ImportRow(dr)
            Next

            For Each dr As DataRow In l_dt_DisplaySavingsPlan.Rows
                l_dt_DisplayConsolidatedTotal.ImportRow(dr)
            Next
            Me.DataTableDisplayConsolidatedTotal = l_dt_DisplayConsolidatedTotal
            Me.DataTableDisplayRetirementPlan = l_dt_DisplayRetirementPlan
            Me.DataTableDisplaySavingsPlan = l_dt_DisplaySavingsPlan
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region
#Region "YRSPS 4212"

    Private Function PrepareDisbursementDetailsAfterTaxes()
        Try
            Dim l_DisbursementDetailsDataTable As New DataTable
            Dim l_decimal_TotalTaxableAmount As Decimal = 0.0
            Dim i As Integer = 0
            Dim l_int_LastRowIndex As Integer = 0
            Dim l_decimal_LastRowTaxWithheldPrincipal As Decimal = 0.0
            Dim l_decimal_LastRowTaxWithheldInterest As Decimal = 0.0
            Dim dr_DisbursementDetailsWithTaxableAmount As DataRow()
            Dim l_DisbursementWithholdingDataTable As DataTable
            Dim dv_DisbusementsDataView As New DataView
            If Not Session("R_DisbursementDetails_C19") Is Nothing Then
                l_DisbursementDetailsDataTable = DirectCast(Session("R_DisbursementDetails_C19"), DataTable)
            Else
                Exit Function
            End If

            If Session("R_DisbursementWithholding_C19") Is Nothing Then Exit Function

            l_DisbursementWithholdingDataTable = DirectCast(Session("R_DisbursementWithholding_C19"), DataTable)
            If l_DisbursementWithholdingDataTable.Rows.Count = 0 Then Exit Function
            'We will get total taxable amount from Withholding tables 
            'YRSPS 4212  added filter condition, to pick only Federal Tax withholding
            'YRSPS 4212,added on 30-Jan-2008(Reference-Prashant's email dated-29-Jan-2008,on selecting rollover option and selecting a value from DatagridDeductions,error is raised)
            '*********
            'l_decimal_TotalTaxableAmount = DirectCast(l_DisbursementWithholdingDataTable.Compute("SUM (Amount)", "WithholdingTypeCode = 'FEDTAX'"), Decimal)

            For Each dr As DataRow In l_DisbursementWithholdingDataTable.Rows
                If dr("WithholdingTypeCode").GetType.ToString <> "System.DBNull" Then
                    If DirectCast(dr("WithholdingTypeCode"), String) = "FEDTAX" Then
                        l_decimal_TotalTaxableAmount = DirectCast(l_DisbursementWithholdingDataTable.Compute("SUM (Amount)", "WithholdingTypeCode = 'FEDTAX'"), Decimal)
                    Else
                        l_decimal_TotalTaxableAmount = "0.00"
                    End If

                End If

            Next
            '*********
            'YRSPS 4212  added filter condition, to pick only Federal Tax withholding
            dr_DisbursementDetailsWithTaxableAmount = l_DisbursementDetailsDataTable.Select("TaxWithheldPrincipal + TaxWithheldInterest > 0")
            If dr_DisbursementDetailsWithTaxableAmount.Length > 0 Then
                For i = 0 To dr_DisbursementDetailsWithTaxableAmount.Length - 2
                    l_decimal_TotalTaxableAmount = l_decimal_TotalTaxableAmount - (dr_DisbursementDetailsWithTaxableAmount(i)("TaxWithheldPrincipal") + dr_DisbursementDetailsWithTaxableAmount(i)("TaxWithheldInterest"))
                Next
                '*************
                'added on 30-Jan-2008(Reference-Prashant's email dated-29-Jan-2008,on selecting rollover option and selecting a value from DatagridDeductions,error is raised)
                l_int_LastRowIndex = dr_DisbursementDetailsWithTaxableAmount.Length - 1
                l_decimal_LastRowTaxWithheldPrincipal = DirectCast(dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal"), Decimal)
                l_decimal_LastRowTaxWithheldInterest = DirectCast(dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldInterest"), Decimal)
                '******************
            End If
            'Commented on  30-Jan-2008(Reference-Prashant's email dated-29-Jan-2008,on selecting rollover option and selecting a value from DatagridDeductions,error is raised)
            'l_int_LastRowIndex = dr_DisbursementDetailsWithTaxableAmount.Length - 1
            'l_decimal_LastRowTaxWithheldPrincipal = DirectCast(dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal"), Decimal)
            'l_decimal_LastRowTaxWithheldInterest = DirectCast(dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldInterest"), Decimal)

            If l_decimal_TotalTaxableAmount > 0 And (l_decimal_LastRowTaxWithheldPrincipal + l_decimal_LastRowTaxWithheldInterest) > 0 Then

                If l_decimal_LastRowTaxWithheldPrincipal > 0 And l_decimal_LastRowTaxWithheldInterest <= 0 Then
                    dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal") = l_decimal_TotalTaxableAmount
                ElseIf l_decimal_LastRowTaxWithheldPrincipal <= 0 And l_decimal_LastRowTaxWithheldInterest > 0 Then
                    dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldInterest") = l_decimal_TotalTaxableAmount
                ElseIf l_decimal_LastRowTaxWithheldPrincipal > 0 And l_decimal_LastRowTaxWithheldInterest > 0 Then
                    If l_decimal_TotalTaxableAmount >= (l_decimal_LastRowTaxWithheldPrincipal + l_decimal_LastRowTaxWithheldInterest) Then
                        dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal") = Math.Round(DirectCast(dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal"), Decimal) + (l_decimal_TotalTaxableAmount - (l_decimal_LastRowTaxWithheldPrincipal + l_decimal_LastRowTaxWithheldInterest)), 2)
                    Else
                        dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal") = Math.Round(DirectCast(dr_DisbursementDetailsWithTaxableAmount(l_int_LastRowIndex)("TaxWithheldPrincipal"), Decimal) + ((l_decimal_LastRowTaxWithheldPrincipal + l_decimal_LastRowTaxWithheldInterest) - l_decimal_TotalTaxableAmount), 2)
                    End If
                End If

            End If
            l_DisbursementDetailsDataTable.AcceptChanges()
            Session("R_DisbursementDetails_C19") = l_DisbursementDetailsDataTable
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

    Public Function IsTerminatedEmployment() As Boolean
        Dim dtEmployment As DataTable = Nothing
        Dim blnIsTerminatedEmployment As Boolean = False
        Try
            dtEmployment = CType(Session("Member Employment_C19"), DataTable)
            If HelperFunctions.isNonEmpty(dtEmployment) Then
                If dtEmployment.Select("StatusType='A'").Length > 0 Then
                    blnIsTerminatedEmployment = False
                Else
                    blnIsTerminatedEmployment = True
                End If
            End If
            Return blnIsTerminatedEmployment
        Catch
            Throw
        End Try
    End Function
    'START | SR | 2016.10.4 |YRS-AT-2962 - If configuration withdrawal is ON then apply combined amount limit rule
    Public Function BA_Legacy_combined_Amt_Switch_ON() As Boolean
        Dim BA_LEGACY_COMBINED_AMT_WITHDRAWAL_CONFIG As DataTable = Nothing
        Dim is_BA_Legacy_combined_Amt_Switch_ON As Boolean = False
        Try
            'START | ML | 2019.06.10 | YRS-AT-4461 | New parameter to apply either BA_Legacy Combined Min Age Or Default age from atsMetaConfiguration.
            'BA_LEGACY_COMBINED_AMT_WITHDRAWAL_CONFIG = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(PersonAge, IsTerminatedEmployment())
            BA_LEGACY_COMBINED_AMT_WITHDRAWAL_CONFIG = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(PersonAge, IsTerminatedEmployment(), True)
            'END | ML | 2019.06.10 | YRS-AT-4461 | New parameter to apply either BA_Legacy Combined Min Age Or Default age from atsMetaConfiguration.
            If HelperFunctions.isNonEmpty(BA_LEGACY_COMBINED_AMT_WITHDRAWAL_CONFIG) Then
                is_BA_Legacy_combined_Amt_Switch_ON = If(String.IsNullOrEmpty(BA_LEGACY_COMBINED_AMT_WITHDRAWAL_CONFIG.Rows(0)("bitIsBALegacyCombinedRule").ToString()), False, Convert.ToBoolean(BA_LEGACY_COMBINED_AMT_WITHDRAWAL_CONFIG.Rows(0)("bitIsBALegacyCombinedRule")))

            End If
            Return is_BA_Legacy_combined_Amt_Switch_ON
        Catch
            Throw
        End Try
    End Function
    'END | SR | 2016.10.4 |YRS-AT-2962 - If configuration withdrawal is ON then apply combined amount limit rule

    'START: SB | 2019.07.22 | YRS-AT-2169 | 20.6.5 | Function that will vaildate fund status is non participants or not (NP/PENP and RDNP participants)
    Public Function IsNonParticipatingPerson() As Boolean
        Dim fundStatusType() As String = {"NP", "PENP", "RDNP"}
        Dim result As Boolean = False
        Dim fundStatus As String = Session("StatusType_C19")

        For Each chvFundStatus As String In fundStatusType
            If chvFundStatus.Trim.ToUpper() = fundStatus.Trim.ToUpper() Then
                result = True
                Exit For
            End If
        Next
        Return result
    End Function
    'END: SB | 2019.07.22 | YRS-AT-2169 | 20.6.5 | Function that will vaildate fund status is non participants or not (NP/PENP and RDNP participants)

    'START | SR | 2019.07.18 | 20.6.5 | YRS-AT-4498 & 3870 - Prorate requested amount Payee 1.  
    ''' <summary>
    ''' This function returns Prorate requested amount for Payee 1 from Session("CalculatedDataTableForCurrentAccounts_C19") for Hardship processing.
    ''' </summary>
    ''' <param name=""></param>
    ''' <returns>void</returns>
    ''' <remarks></remarks>
    Private Function ProrateHardshipAmountForPayee1()
        Dim currentAccountDataTable As DataTable
        Dim totalHardshipAmount As Decimal
        Dim proratingFator As Decimal
        Dim payee1DataTable As DataTable
        Dim payee1DataRow As DataRow
        Dim personalPreTax As Decimal
        Dim persoanlPostTax As Decimal
        Dim personalInterest As Decimal
        Dim ymcaPreTax As Decimal
        Dim ymcaPostTax As Decimal
        Dim ymcaInterest As Decimal
        Dim taxable As Decimal
        Dim nonTaxable As Decimal
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ProrateHardshipAmountForPayee1() START")
            If Me.HardshipAmount <= 0.0 Then Return 0
            If Me.TDUsedAmount <= 0.0 Then Return 0

            currentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)

            Dim drHardshipRows As DataRow() = currentAccountDataTable.Select("AccountType='TD' OR AccountType='TM'")
            If drHardshipRows.Length > 0 Then
                For i = 0 To drHardshipRows.Length - 1
                    If IsInterestAllowed Then 'If interest component is allowed then consider interest component in total hardship amount.
                        totalHardshipAmount += DirectCast(drHardshipRows(i)("Total"), Decimal)
                    Else 'If interest component is not allowed then do not consider interest component in total hardship amount.
                        totalHardshipAmount += DirectCast(drHardshipRows(i)("Taxable"), Decimal) + DirectCast(drHardshipRows(i)("Non-Taxable"), Decimal)
                    End If
                Next
            End If
            ' Get prorate factor for payee1
            If totalHardshipAmount >= Me.HardshipAmount AndAlso Me.HardshipAmount > 0 Then
                proratingFator = IIf(totalHardshipAmount > 0, HardshipAmount / totalHardshipAmount, 0)
            ElseIf totalHardshipAmount < Me.HardshipAmount AndAlso Me.HardshipAmount > 0 Then
                Me.HardshipAmount = totalHardshipAmount
                proratingFator = 1
            Else
                proratingFator = 0
            End If

            For Each l_DataRow As DataRow In currentAccountDataTable.Rows
                If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                    If l_DataRow("AccountGroup") = m_const_SavingsPlan_TD Or l_DataRow("AccountGroup") = m_const_SavingsPlan_TM Then
                        'initialize variable everytime in loop
                        personalPreTax = 0
                        persoanlPostTax = 0
                        personalInterest = 0
                        ymcaPreTax = 0
                        ymcaInterest = 0

                        If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" Then
                            personalPreTax = Math.Round(CType(l_DataRow("Taxable"), Decimal) * proratingFator, 2)
                        End If

                        If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
                            persoanlPostTax = Math.Round(DirectCast(l_DataRow("Non-Taxable"), Decimal) * proratingFator, 2)
                        End If
                        'If interest component is allowed then consider interest component for prorating else consider personal interest component 0.
                        If IsInterestAllowed Then
                            If l_DataRow("Interest").GetType.ToString <> "System.DBNull" Then
                                personalInterest = Math.Round(CType(l_DataRow("Interest"), Decimal) * proratingFator, 2)
                            End If
                        End If

                        If l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" Then
                            ymcaPreTax = Math.Round(CType(l_DataRow("YMCATaxable"), Decimal) * proratingFator, 2)
                        End If

                        If l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" Then
                            ymcaInterest = Math.Round(CType(l_DataRow("YMCAInterest"), Decimal) * proratingFator, 2)
                        End If

                        taxable = personalPreTax + personalInterest + ymcaPreTax + ymcaInterest
                        nonTaxable = persoanlPostTax

                        If taxable + nonTaxable > 0.0 Then
                            If Not l_DataRow("AccountGroup").GetType.ToString = "System.DBNull" Then
                                If l_DataRow("AccountGroup") = m_const_SavingsPlan_TD Or l_DataRow("AccountGroup") = m_const_SavingsPlan_TM Then

                                    If Not payee1DataTable Is Nothing Then
                                        payee1DataRow = payee1DataTable.NewRow
                                        payee1DataRow("Taxable") = taxable
                                        payee1DataRow("NonTaxable") = nonTaxable
                                        payee1DataRow("TaxRate") = Me.HardShipTaxRate
                                        payee1DataRow("Tax") = taxable * (Me.HardShipTaxRate / 100.0)
                                        payee1DataRow("AcctType") = l_DataRow("AccountType")
                                        payee1DataRow("Payee") = Me.Payee1Name
                                        payee1DataRow("FundedDate") = System.DBNull.Value
                                        payee1DataRow("RefRequestsID") = Me.SessionRefundRequestID
                                        payee1DataRow("RequestType") = "HARD"
                                        payee1DataTable.Rows.Add(payee1DataRow)
                                        l_DataRow.AcceptChanges()
                                    End If
                                End If
                            End If
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("ProratingFator:{0}, Taxable:{1}, NonTaxable:{2}, RefRequestId:{3}, AccountType{4} ", proratingFator, taxable, nonTaxable, SessionRefundRequestID, l_DataRow("AccountType")))
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Error :", ex.Message))
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ProrateHardshipAmountForPayee1() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    'START | SR | 2019.07.18 | 20.6.5 | YRS-AT-4498 & 3870 - Prorate calculated current accounts.  
    ''' <summary>
    ''' This function returns Prorate data available in Session("CalculatedDataTableForCurrentAccounts_C19") for Hardship processing.
    ''' </summary>
    ''' <param name=""></param>
    ''' <returns>void</returns>
    ''' <remarks></remarks>
    Private Function ProrateCurrentAmountForHardship()
        Dim currentAccountDataTable As DataTable
        Dim totalHardshipAmount As Decimal
        Dim proratingFator As Decimal
        Dim payee1DataTable As DataTable
        Dim payee1DataRow As DataRow
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ProrateCurrentAmountForHardship() START")
            'Me.IsHardShip = False
            If Me.HardshipAmount <= 0.0 Then Return 0
            If Me.TDUsedAmount <= 0.0 Then Return 0

            currentAccountDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)

            Dim drHardshipRows As DataRow() = currentAccountDataTable.Select("AccountType='TD' OR AccountType='TM'")
            If drHardshipRows.Length > 0 Then
                For i = 0 To drHardshipRows.Length - 1
                    If IsInterestAllowed Then 'If interest component is allowed then consider interest component in total hardship amount
                        totalHardshipAmount += DirectCast(drHardshipRows(i)("Total"), Decimal)
                    Else 'If interest component is not allowed then do not consider interest component in total hardship amount
                        totalHardshipAmount += Math.Round(DirectCast(drHardshipRows(i)("Taxable"), Decimal) + DirectCast(drHardshipRows(i)("Non-Taxable"), Decimal), 2)
                    End If
                Next
            End If

            ' Get prorate factor for payee1
            If totalHardshipAmount >= Me.HardshipAmount AndAlso Me.HardshipAmount > 0 Then
                proratingFator = IIf(totalHardshipAmount > 0, HardshipAmount / totalHardshipAmount, 0)
            ElseIf totalHardshipAmount < Me.HardshipAmount AndAlso Me.HardshipAmount > 0 Then
                Me.HardshipAmount = totalHardshipAmount
                proratingFator = 1
            Else
                proratingFator = 0
            End If

            For Each l_DataRow As DataRow In currentAccountDataTable.Rows
                If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                    If l_DataRow("AccountGroup") = m_const_SavingsPlan_TD Or l_DataRow("AccountGroup") = m_const_SavingsPlan_TM Then
                        If l_DataRow("Taxable").GetType.ToString <> "System.DBNull" Then
                            l_DataRow("Taxable") = Math.Round(CType(l_DataRow("Taxable"), Decimal) * proratingFator, 2)
                        Else
                            l_DataRow("Taxable") = 0
                        End If

                        If l_DataRow("Non-Taxable").GetType.ToString <> "System.DBNull" Then
                            l_DataRow("Non-Taxable") = Math.Round(DirectCast(l_DataRow("Non-Taxable"), Decimal) * proratingFator, 2)
                        Else
                            l_DataRow("Non-Taxable") = 0
                        End If
                        'If interest component is allowed then consider interest component for prorating else consider personal interest component 0.
                        If IsInterestAllowed Then
                            If l_DataRow("Interest").GetType.ToString <> "System.DBNull" Then
                                l_DataRow("Interest") = Math.Round(CType(l_DataRow("Interest"), Decimal) * proratingFator, 2)
                            Else
                                l_DataRow("Interest") = 0
                            End If
                        Else
                            l_DataRow("Interest") = 0
                        End If

                        If l_DataRow("Emp.Total").GetType.ToString <> "System.DBNull" Then
                            l_DataRow("Emp.Total") = Math.Round(DirectCast(l_DataRow("Non-Taxable"), Decimal) + DirectCast(l_DataRow("Taxable"), Decimal) + DirectCast(l_DataRow("Interest"), Decimal), 2)
                        Else
                            l_DataRow("Emp.Total") = 0
                        End If

                        If l_DataRow("YMCATaxable").GetType.ToString <> "System.DBNull" Then
                            l_DataRow("YMCATaxable") = Math.Round(CType(l_DataRow("YMCATaxable"), Decimal) * proratingFator, 2)
                        Else
                            l_DataRow("YMCATaxable") = 0
                        End If
                        'If interest component is allowed then consider interest component for prorating else consider Ymca interest component 0.
                        If IsInterestAllowed Then
                            If l_DataRow("YMCAInterest").GetType.ToString <> "System.DBNull" Then
                                l_DataRow("YMCAInterest") = Math.Round(CType(l_DataRow("YMCAInterest"), Decimal) * proratingFator, 2)
                            Else
                                l_DataRow("YMCAInterest") = 0
                            End If
                        Else
                            l_DataRow("YMCAInterest") = 0
                        End If

                        If l_DataRow("YMCATotal").GetType.ToString <> "System.DBNull" Then
                            l_DataRow("YMCATotal") = Math.Round(DirectCast(l_DataRow("YMCATaxable"), Decimal) + DirectCast(l_DataRow("YMCAInterest"), Decimal), 2)
                        Else
                            l_DataRow("YMCATotal") = 0
                        End If

                        If l_DataRow("Total").GetType.ToString <> "System.DBNull" Then
                            l_DataRow("Total") = Math.Round(CType(l_DataRow("YMCATotal"), Decimal) + CType(l_DataRow("Emp.Total"), Decimal), 2)
                        Else
                            l_DataRow("YMCATotal") = 0
                        End If
                        l_DataRow.AcceptChanges()
                    End If
                End If
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Taxable : {0}, Non-Taxable: {1}, Interest:{2}, YMCATaxable : {3}, YMCAInterest: {4}, proratingFator:{5}", l_DataRow("Taxable"), l_DataRow("Non-Taxable"), l_DataRow("Interest"), l_DataRow("YMCATaxable"), l_DataRow("YMCAInterest"), proratingFator))
            Next
            ' calculate total component for each column
            currentAccountDataTable = CalculateTotal(currentAccountDataTable)
            currentAccountDataTable.AcceptChanges()

        Catch ex As Exception
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ProrateCurrentAmountForHardship() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function
    ''' <summary>
    ''' This function returns Prorate available balance in datatable for Hardship processing.
    ''' </summary>
    ''' <param name="configValue">Datatable</param>
    ''' <returns>(Datatable)</returns>
    ''' <remarks></remarks>
    Private Function ProrateTransactionDataForHardship(ByVal transactionBalance As DataTable) As DataTable
        Dim currentAccountDataTable As DataTable
        Dim totalHardshipAmount As Decimal
        Dim proratingFator As Decimal
        Dim payee1DataTable As DataTable
        Dim payee1DataRow As DataRow
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ProrateTransactionDataForHardship() START")
            currentAccountDataTable = transactionBalance
            Dim drHardshipRows As DataRow() = currentAccountDataTable.Select("AccountGroup='STD' OR AccountGroup='STM'")
            If drHardshipRows.Length > 0 Then
                For i = 0 To drHardshipRows.Length - 1
                    If IsInterestAllowed Then
                        totalHardshipAmount += DirectCast(drHardshipRows(i)("PersonalPreTax"), Decimal) + DirectCast(drHardshipRows(i)("PersonalPostTax"), Decimal) + DirectCast(drHardshipRows(i)("YmcaPreTax"), Decimal)
                    Else
                        If (drHardshipRows(i)("MoneyType") = "PR") Then
                            totalHardshipAmount += DirectCast(drHardshipRows(i)("PersonalPreTax"), Decimal) + DirectCast(drHardshipRows(i)("PersonalPostTax"), Decimal) + DirectCast(drHardshipRows(i)("YmcaPreTax"), Decimal)
                        End If
                    End If

                Next
            End If

            If totalHardshipAmount >= Me.HardshipAmount AndAlso Me.HardshipAmount > 0 Then
                proratingFator = IIf(totalHardshipAmount > 0, HardshipAmount / totalHardshipAmount, 0)
            ElseIf totalHardshipAmount < Me.HardshipAmount AndAlso Me.HardshipAmount > 0 Then
                Me.HardshipAmount = totalHardshipAmount
                proratingFator = 1
            Else
                proratingFator = 0
            End If

            For Each l_DataRow As DataRow In currentAccountDataTable.Rows
                If l_DataRow("PlanType").GetType.ToString() <> "System.DBNull" Then
                    If l_DataRow("AccountGroup") = m_const_SavingsPlan_TD Or l_DataRow("AccountGroup") = m_const_SavingsPlan_TM Then
                        If ((l_DataRow("MoneyType") = "PR") Or (l_DataRow("MoneyType") = "IN" AndAlso IsInterestAllowed = True)) Then

                            If l_DataRow("PersonalPreTax").GetType.ToString <> "System.DBNull" Then
                                l_DataRow("PersonalPreTax") = Math.Round(CType(l_DataRow("PersonalPreTax"), Decimal) * proratingFator, 2)
                            Else
                                l_DataRow("PersonalPreTax") = 0
                            End If

                            If l_DataRow("PersonalPostTax").GetType.ToString <> "System.DBNull" Then
                                l_DataRow("PersonalPostTax") = Math.Round(DirectCast(l_DataRow("PersonalPostTax"), Decimal) * proratingFator, 2)
                            Else
                                l_DataRow("PersonalPostTax") = 0
                            End If

                            If l_DataRow("YmcaPreTax").GetType.ToString <> "System.DBNull" Then
                                l_DataRow("YmcaPreTax") = Math.Round(CType(l_DataRow("YmcaPreTax"), Decimal) * proratingFator, 2)
                            Else
                                l_DataRow("YmcaPreTax") = 0
                            End If
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("ProratingFator : {0}, PersonalPreTax : {1}, PersonalPostTax: {2}, YmcaPreTax:{3}", proratingFator, l_DataRow("PersonalPreTax"), l_DataRow("PersonalPostTax"), l_DataRow("YmcaPreTax")))
                        End If
                        l_DataRow.AcceptChanges()
                    End If
                End If
            Next
            Return currentAccountDataTable
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Error : {0}", ex.Message))
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ProrateTransactionDataForHardship() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    'START: MMR | 07/23/2019 | YRS-AT-4498 | This method will fetch all configurations related to refund and set their values in respective declared properties
    ''' <summary>
    ''' This method will fetch all configurations related to refund and set their values in respective declared properties
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadRefundConfiguration()
        Dim refundConfiuration As DataTable
        Dim category As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "Refunds.LoadRefundConfiguration() START")
            category = "Refund"
            refundConfiuration = YMCARET.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise(category)

            If HelperFunctions.isNonEmpty(refundConfiuration) Then
                For Each configuraionRow In refundConfiuration.Rows
                    Select Case Convert.ToString(configuraionRow("Key")).Trim().ToUpper()
                        Case "HARDSHIP_ALLOW_TDCONTRIBUTION_FLAG"
                            Me.IsTDContributionsAllowed = IsHardshipConfigurationsEnabled(Convert.ToString(configuraionRow("Value")))
                        Case "HARDSHIP_PRELOAN_CRITERIA_FLAG"
                            Me.IsLoanCriteriaNotRequired = IsHardshipConfigurationsEnabled(Convert.ToString(configuraionRow("Value")))
                        Case "HARDSHIP_INCLUDE_INTEREST_FLAG"
                            Me.IsInterestAllowed = IsHardshipConfigurationsEnabled(Convert.ToString(configuraionRow("Value")))
                            'START - MMR | 04/20/2020 |YRS-AT-4854 | Fetch all configurations related to Covid Refund
                        Case "COVID_REFUND_EXEMPT_AMOUNT"
                            Me.CovidAmountLimit = Convert.ToDecimal(configuraionRow("Value"))
                        Case "REFUND_FEDERALTAXRATE_COVID19"
                            Me.CovidTaxRate = Convert.ToDecimal(configuraionRow("Value"))
                            'END - MMR | 04/20/2020 |YRS-AT-4854 | Fetch all configurations related to Covid Refund
                    End Select
                Next
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("IsTDContributionsAllowed : {0}, IsLoanCriteriaNotRequired: {1}, IsInterestAllowed:{2} ", Me.IsTDContributionsAllowed, Me.IsLoanCriteriaNotRequired, Me.IsInterestAllowed))
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Error : {0}", ex.Message))
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "Refunds.LoadRefundConfiguration() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    'END: MMR | 07/23/2019 | YRS-AT-4498 | This method will fetch all configurations related to refund and set their values in respective declared properties

    'START: MMR | YRS-AT-4498 | This function returns true if configuration value is True.
    ''' <summary>
    ''' This function returns true if configuration value is True.
    ''' </summary>
    ''' <param name="configValue">configValue</param>
    ''' <returns>(TRUE/FALSE)</returns>
    ''' <remarks></remarks>
    Private Function IsHardshipConfigurationsEnabled(ByVal configValue As String) As Boolean
        Dim isAllowed As Boolean = False

        If Not String.IsNullOrEmpty(configValue) AndAlso configValue.ToUpper() = "TRUE" Then
            isAllowed = True
        End If

        Return isAllowed
    End Function
    'END: MMR | YRS-AT-4498 | This function returns true if configuration value is True.

    ' START | SR | 2019.08.22 | YRS-AT-4551 | Get refund request table value from existing refund request.
    Private Function GetRefundRequestTableValue(ByVal columnName As String, ByVal defaultValue As String) As String
        Dim columnValue As String = defaultValue
        Dim requestedDataRows As DataRow()
        Dim refundRequest As DataTable
        ' Check session should not be empty.
        If Not Session("RefundRequestDataTable_C19") Is Nothing Then
            refundRequest = DirectCast(Session("RefundRequestDataTable_C19"), DataTable)
            If HelperFunctions.isNonEmpty(refundRequest) Then
                requestedDataRows = refundRequest.Select("UniqueID = '" + Me.SessionRefundRequestID + "'")
                If requestedDataRows.Length > 0 Then
                    columnValue = CType(requestedDataRows(0)(columnName), String).Trim
                End If
            End If
        End If
        Return columnValue
    End Function
    ' END | SR | 2019.08.22 | YRS-AT-4551 | Get refund request table value from existing refund request.

#Region "Covid Methods"
    'START - MMR | 04/20/2020 |YRS-AT-4854 | Added Methods for COVID Tax calculation
    ''' <summary>
    ''' Function will return tax rate to be applied on taxable amount based on COVID Rules
    ''' </summary>
    ''' <param name="totalTaxableAmount">Covid Taxable amount</param>
    ''' <param name="remainingCovidBenefitAmount">Non-Covid Taxable amount</param>
    ''' <returns>Tax Rate</returns>
    ''' <remarks></remarks>
    Public Function GetCovidApplicableTaxRate(ByVal covidTaxableAmount As Decimal, ByVal nonCovidTaxableAmount As Decimal) As Decimal
        Dim totalTaxableAmount As Decimal
        Dim covidTaxWithheldAmount As Decimal
        Dim nonCovidTaxWithheldAmount As Decimal
        Dim taxRate As Decimal

        'Getting total taxable amount based on which blended tax rate will be calculated
        totalTaxableAmount = covidTaxableAmount + nonCovidTaxableAmount

        'Computing tax on covid taxable amount based on defined tax rate for Covid in configuration
        covidTaxWithheldAmount = (covidTaxableAmount * Me.CovidTaxRate) / 100

        'Computing tax on taxable amount on which Covid tax benefit not applicable based on key defined for regular federal tax rate in configuration
        nonCovidTaxWithheldAmount = (nonCovidTaxableAmount * Me.ApplicableFederalTaxRate) / 100

        'Computing Final tax rate which will be applied to arrive at tax amount which will be withheld from other than Covid taxable amount
        taxRate = ((covidTaxWithheldAmount + nonCovidTaxWithheldAmount) / totalTaxableAmount) * 100

        Return taxRate
    End Function
    ''' <summary>
    ''' Method will return Covid taxable and non-covid taxable amount
    ''' </summary>
    ''' <param name="totalAvailableAmount">Total amount available for withdrawal</param>
    ''' <param name="totalTaxableAmount">Total taxable amount available for withdrawal</param>
    ''' <param name="remainingCovidAmount">Covid benefit amount available</param>
    ''' <remarks></remarks>
    Public Sub GetTaxableAmountForCovidTaxRateCalculation(ByVal totalAvailableAmount As Decimal, ByVal totalTaxableAmount As Decimal, ByVal totalNontaxableAmount As Decimal, ByVal remainingCovidAmount As Decimal)
        Dim covidFactor As Decimal
        Dim nonCovidAmount As Decimal
        Dim nonCovidFactor As Decimal

        covidFactor = 1
        '---------------Covid Factor--------'
        'Getting factor of Covid amount from total amount which will be used to get covid taxable amount
        If remainingCovidAmount < totalAvailableAmount Then
            covidFactor = (remainingCovidAmount / totalAvailableAmount)
        End If

        '---------------Getting Covid And Non-covid taxable and non-taxable amount--------'
        'Get Covid taxable amount from total taxable balance based on derived covid factor
        Me.CovidTaxableAmount = (totalTaxableAmount * covidFactor)

        'Get Covid non-taxable amount from total balance based on derived covid factor
        Me.CovidNonTaxableAmount = (totalNontaxableAmount * covidFactor)

        'Get Non-Covid taxable and non-taxable amount from total taxable balance based on derived non covid factor
        Me.NonCovidTaxableAmount = totalTaxableAmount - Me.CovidTaxableAmount
        Me.NonCovidNonTaxableAmount = totalNontaxableAmount - Me.CovidNonTaxableAmount
    End Sub

    ''' <summary>
    ''' Method will return Covid amount Used
    ''' </summary>
    ''' <param name="fundNo">Fund No</param>
    ''' <returns>Covid amount used</returns>
    ''' <remarks></remarks>
    Public Function GetCovidAmountUsed(ByVal fundNo As Integer) As Decimal
        Dim covidAmountUsed As Decimal

        covidAmountUsed = YMCARET.YmcaBusinessObject.RefundRequest_C19.GetCovidAmountUsed(fundNo)

        Return covidAmountUsed
    End Function
#End Region
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Added Methods for COVID Tax calculation
End Class
