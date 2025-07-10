'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ShowBalancesQDRONonRetired.aspx.vb
' Author Name		:	Amit Nigam	
' Employee ID		:	36413
' Email			    :	amit.nigam@3i-infotech.com
' Contact No		:	080-39876761
' Creation Time	    :	08/07/2008 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

'
' Changed by			:	
' Changed on			:	
' Change Description	:	
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod Prakash Pokale          2016.09.02          YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
'                                                   1. Removed unused private field variables
'                                                   2. Removed unused private properties
'                                                   3. Removed unused functions and procedures
'                                                   4. Changed balance display code
'Pramod Prakash Pokale          2016.09.28          YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
'Pramod Prakash Pokale          2017.01.04          YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing
'                                                   YRS-AT-3265 -  YRS enh:improve usability of QDRO split screens (TrackIT 28050)
'*******************************************************************************

Imports System.Collections.Generic 'PPP | 09/02/2016 | YRS-AT-2529 

Public Class ShowBalancesQDRONonRetired
    Inherits System.Web.UI.Page
    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    ''Protected WithEvents DataListParticipant As System.Web.UI.WebControls.DataList
    Protected WithEvents DatalistBeneficiary As System.Web.UI.WebControls.DataList
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    'Protected WithEvents DatagridSummaryBalList As System.Web.UI.WebControls.DataGrid 'PPP | 09/02/2016 | YRS-AT-2529 | Control is not required
    Protected WithEvents divBeneficiaryTable As System.Web.UI.HtmlControls.HtmlGenericControl 'PPP | 09/02/2016 | YRS-AT-2529 | Introduced new div control which will hold HTML control created by code behind

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents divParticipantTable As System.Web.UI.HtmlControls.HtmlGenericControl 'PPP | 01/05/2017 | YRS-AT-3145 & 3265

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Properties"
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to add the properties for sessions.                                //
    '***************************************************************************************************//
    'PPP | 09/02/2016 | YRS-AT-2529 | Restuctred private properties (Removed old, renamed old and introduced new)
    Private Property PersID() As String
        Get
            If Not Session("PersSSID") Is Nothing Then
                Return Session("PersSSID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersSSID") = Value
        End Set
    End Property

    Private Property BeneficiaryDetailsTable() As DataTable
        Get
            If Not (Session("dtBenifAccount")) Is Nothing Then

                Return (CType(Session("dtBenifAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtBenifAccount") = Value
        End Set
    End Property

    Private Property SplitConfigurationDetails() As DataTable
        Get
            If Not (Session("dtPecentageCount")) Is Nothing Then

                Return (DirectCast(Session("dtPecentageCount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtPecentageCount") = Value
        End Set
    End Property

    Private Property RecepientAfterSplitAccountsDetail() As DataSet
        Get
            If Not (Session("dsAllRecipantAccountsDetail")) Is Nothing Then
                Return (DirectCast(Session("dsAllRecipantAccountsDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsAllRecipantAccountsDetail") = Value
        End Set
    End Property

    Private Property ParticipantAfterSplitAccountsDetail() As DataSet
        Get
            If Not (Session("dsAllPartAccountsDetail")) Is Nothing Then

                Return (DirectCast(Session("dsAllPartAccountsDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsAllPartAccountsDetail") = Value
        End Set
    End Property

    Private Property ParticipantOriginalBalance() As DataSet
        Get
            If Not (Session("PartAccountDetail")) Is Nothing Then
                Return (DirectCast(Session("PartAccountDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("PartAccountDetail") = Value
        End Set
    End Property

    'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265
    Private Property ParticipantTotalBalanceAfterSplitManageFees() As DataTable
        Get
            If Not (Session("participantTotalBalanceAfterSplitManageFees")) Is Nothing Then

                Return (DirectCast(Session("participantTotalBalanceAfterSplitManageFees"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("participantTotalBalanceAfterSplitManageFees") = Value
        End Set
    End Property
   
    Private Property RecipientBalanceAfterSplitManageFees() As DataTable
        Get
            If Not (Session("recipientBalanceAfterSplitManageFees")) Is Nothing Then
                Return (DirectCast(Session("recipientBalanceAfterSplitManageFees"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("recipientBalanceAfterSplitManageFees") = Value
        End Set
    End Property
    'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 

#End Region

#Region "VARIABLE DECLARATION"
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to declare the variables used in the class.                        //
    '***************************************************************************************************//
    'PPP | 09/02/2016 | YRS-AT-2529 | Removed unused local variables 
    Protected WithEvents DataListParticipant As System.Web.UI.WebControls.DataList
#End Region

#Region "PAGE LOAD"
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to adding attribute to control.                //
    '***************************************************************************************************//
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'PPP | 09/02/2016 | YRS-AT-2529 | Restructrued Page_Load. Old code can be checked from TF.
        Try
            If Not IsPostBack Then 'PPP | 09/21/2016 | YRS-AT-2529 | Load data in grids only at first load
                If Not Me.ParticipantAfterSplitAccountsDetail Is Nothing Then  'PPP | 09/21/2016 | YRS-AT-2529 | Check split data exists for participant, if exists then only move ahead for data loading as well as we can assume recipient data also exists
                    LoadParticipant()
                    DrawBeneficiaryTable()
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Page_Load", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#End Region

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

#Region "PRIVATE  EEVENTS"
    'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | DataListParticipant is not in use so commenting its events also
    ''***************************************************************************************************//
    ''Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    ''Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Class Description         :This class is being used to create the datagrid in the datalist         //
    ''                           to show heirarchy                                                        //
    ''***************************************************************************************************//
    'Private Sub DataListParticipant_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataListParticipant.ItemDataBound
    '    'PPP | 09/02/2016 | YRS-AT-2529 | Restructrued DataListParticipant_ItemDataBound. Old code can be checked from TF.
    '    Dim dgParticipant As New DataGrid
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            dgParticipant = DirectCast(e.Item.FindControl("DatagridSummaryBalList"), DataGrid)
    '            LoadParticipantAfterSplitBalances(dgParticipant)
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("DataListParticipant_ItemDataBound", ex)
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    Finally
    '        dgParticipant = Nothing
    '    End Try
    'End Sub
    'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | DataListParticipant is not in use so commenting its events also

    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Close the Window                                                 //
    '***************************************************************************************************//
    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Dim closeWindow As String = "<script language='javascript'>" & _
                                                   "window.close()" & _
                                                   "</script>"
            Page.RegisterStartupScript("CloseWindow", closeWindow)
        Catch ex As Exception
            HelperFunctions.LogException("DatalistBeneficiary_ItemDataBound", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
#End Region

    Private Sub LoadParticipant()
        Dim participantDetail As DataSet = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetParticipantDetail(Me.PersID)
        'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Participant table will be designed using HTML control objects
        'DataListParticipant.DataSource = participantDetail
        'DataListParticipant.DataBind()
        DrawParticipantTable(participantDetail)
        'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Participant table will be designed using HTML control objects
    End Sub

    'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Drawing Participant table
    Private Sub DrawParticipantTable(ByVal participantDetails As DataSet)
        Dim mainTable As System.Web.UI.HtmlControls.HtmlTable
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim participantAfterSplitValuesTable As DataTable
        Dim planTypeToDisplay As String
        Try
            mainTable = New HtmlTable()
            mainTable.Attributes.Add("width", "98%")
            mainTable.Rows.Add(CreateBeneficiaryTableRow("SSNo.", "Last Name", "First Name", "Fund status", True))
            mainTable.Rows.Add(CreateBeneficiaryTableRow(participantDetails.Tables(0).Rows(0)("SSNo"), participantDetails.Tables(0).Rows(0)("LastName"), participantDetails.Tables(0).Rows(0)("FirstName"), "Active", False))

            planTypeToDisplay = GetParticipantPlanToShowOnSummary()
            participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable(planTypeToDisplay, Me.ParticipantAfterSplitAccountsDetail, Me.ParticipantOriginalBalance)

            If Not participantAfterSplitValuesTable Is Nothing Then
                mainTable.Rows.Add(CreateParticipantAccountDetailsRow(participantAfterSplitValuesTable))
            End If

            divParticipantTable.Controls.Add(mainTable)
        Catch
            Throw
        Finally
            planTypeToDisplay = Nothing
            participantAfterSplitValuesTable = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
            mainTable = Nothing
        End Try
    End Sub

    Private Function CreateParticipantAccountDetailsRow(ByVal participantAfterSplitValuesTable As DataTable) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim accountTable As System.Web.UI.HtmlControls.HtmlTable
        Dim accountRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim accountCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total As String

        Dim feeDetails As DataTable
        Dim retirementFee, savingsFee As Decimal

        Dim isRetirementPlanSplitExists, isSavingsPlanSplitExists As Boolean
        Try
            retirementFee = 0
            savingsFee = 0
            isRetirementPlanSplitExists = False
            isSavingsPlanSplitExists = False
            If HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) Then
                feeDetails = Me.ParticipantTotalBalanceAfterSplitManageFees
                retirementFee = Convert.ToDecimal(feeDetails.Rows(0)("RetirementFee"))
                savingsFee = Convert.ToDecimal(feeDetails.Rows(0)("SavingsFee"))
                isRetirementPlanSplitExists = Convert.ToBoolean(feeDetails.Rows(0)("IsRetirementPlanSplitExists"))
                isSavingsPlanSplitExists = Convert.ToBoolean(feeDetails.Rows(0)("IsSavingsPlanSplitExists"))
            End If

            accountTable = New HtmlTable()
            accountTable.Attributes.Add("class", "DataGrid_Grid")
            accountTable.Attributes.Add("cellspacing", "0")
            accountTable.Attributes.Add("rules", "all")
            accountTable.Attributes.Add("border", "1")
            accountTable.Attributes.Add("style", "width:100%;border-collapse:collapse;")

            'Add headers
            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_HeaderStyle", "Acct", "Taxable", "Non-Taxable", "Interest", "YMCA Taxable", "YMCA Interest", "Acct. Total "))

            accountCell = New HtmlTableCell()
            accountCell.Attributes.Add("colspan", "7")
            accountCell.Attributes.Add("style", "text-indent: 20px;")
            If isRetirementPlanSplitExists And isSavingsPlanSplitExists Then
                accountCell.InnerText = String.Format("Fees: Retirement (${0}); Savings (${1})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture), savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
            ElseIf isRetirementPlanSplitExists Then
                accountCell.InnerText = String.Format("Fees: Retirement (${0})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
            ElseIf isSavingsPlanSplitExists Then
                accountCell.InnerText = String.Format("Fees: Savings (${0})", savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
            End If

            accountRow = New HtmlTableRow()
            accountRow.Attributes.Add("class", "DataGrid_HeaderStyle")
            accountRow.Cells.Add(accountCell)
            accountTable.Rows.Add(accountRow)

            If HelperFunctions.isNonEmpty(participantAfterSplitValuesTable) Then
                For rowCounter As Integer = 0 To participantAfterSplitValuesTable.Rows.Count - 1
                    accountType = participantAfterSplitValuesTable.Rows(rowCounter)("AcctType")
                    personalPreTax = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("PersonalPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    personalPostTax = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("PersonalPostTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    personalInterest = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("PersonalInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    ymcaTaxable = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("YMCAPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    ymcaInterest = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("YMCAInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    total = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("TotalTotal")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)

                    If (rowCounter Mod 2) = 0 Then
                        accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_NormalStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                    Else
                        accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_AlternateStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                    End If
                Next
            End If

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("colspan", "4")
            mainTableCell.Controls.Add(accountTable)

            mainTableRow = New HtmlTableRow
            mainTableRow.Cells.Add(mainTableCell)

            Return mainTableRow
        Catch
            Throw
        Finally
            accountType = Nothing
            personalPreTax = Nothing
            personalPostTax = Nothing
            personalInterest = Nothing
            ymcaTaxable = Nothing
            ymcaInterest = Nothing
            total = Nothing
            participantAfterSplitValuesTable = Nothing
            accountCell = Nothing
            accountRow = Nothing
            accountTable = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
        End Try
    End Function
    'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Drawing Participant table

    'START: PPP | 09/02/2016 | YRS-AT-2529 | 
    'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Participant table is drawn using HTML table so thos method is not required
    'Private Sub LoadParticipantAfterSplitBalances(ByVal grid As DataGrid)
    '    'START: PPP | 09/28/2016 | YRS-AT-2529 | Plan will be decided based on the split's recorded till now, as well as all account balances of participant will be shown
    '    '' Always show all participants splited accounts, So passing "Both" as plan type
    '    'Dim participantAfterSplitValuesTable As DataTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable("Both", Me.ParticipantAfterSplitAccountsDetail, Me.ParticipantOriginalBalance)
    '    'grid.DataSource = participantAfterSplitValuesTable
    '    'grid.DataBind()

    '    Dim participantAfterSplitValuesTable As DataTable
    '    Dim planTypeToDisplay As String
    '    Try
    '        planTypeToDisplay = GetParticipantPlanToShowOnSummary()
    '        participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable(planTypeToDisplay, Me.ParticipantAfterSplitAccountsDetail, Me.ParticipantOriginalBalance)
    '        If HelperFunctions.isNonEmpty(participantAfterSplitValuesTable) Then
    '            grid.DataSource = participantAfterSplitValuesTable
    '            grid.DataBind()
    '        End If
    '    Catch
    '        Throw
    '    Finally
    '        planTypeToDisplay = Nothing
    '        participantAfterSplitValuesTable = Nothing
    '    End Try
    '    'END: PPP | 09/28/2016 | YRS-AT-2529 | Plan will be decided based on the split's recorded till now, as well as all account balances of participant will be shown
    'End Sub
    'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Participant table is drawn using HTML table so thos method is not required

    Private Sub DrawBeneficiaryTable()
        Dim splitConfigurationTable As DataTable
        Dim beneficiaryTable As New DataTable

        Dim mainTable As System.Web.UI.HtmlControls.HtmlTable
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim beneficiaryPersID As String, beneficiaryFundEventID As String
        Try
            If Not Me.BeneficiaryDetailsTable Is Nothing And Not Me.SplitConfigurationDetails Is Nothing Then
                splitConfigurationTable = Me.SplitConfigurationDetails
                beneficiaryTable = Me.BeneficiaryDetailsTable

                mainTable = New HtmlTable()
                mainTable.Attributes.Add("width", "680")
                mainTable.Rows.Add(CreateBeneficiaryTableRow("SSNo.", "Last Name", "First Name", "Fund status", True))
                For Each beneficiary In beneficiaryTable.Rows
                    beneficiaryPersID = beneficiary("id")
                    beneficiaryFundEventID = beneficiary("RecpFundEventId")
                    If (splitConfigurationTable.Select(String.Format("PersId='{0}'", beneficiaryPersID)).Length > 0) Then
                        mainTable.Rows.Add(CreateBeneficiaryTableRow(beneficiary("SSNo"), beneficiary("LastName"), beneficiary("FirstName"), "QD", False))
                        mainTable.Rows.Add(CreateBeneficiarySplitDetailsRow(beneficiaryPersID, beneficiaryFundEventID, SplitConfigurationDetails))
                    End If
                Next

                divBeneficiaryTable.Controls.Add(mainTable)
            End If
        Catch
            Throw
        Finally
            beneficiaryPersID = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
            mainTable = Nothing
            beneficiaryTable = Nothing
            splitConfigurationTable = Nothing
        End Try
    End Sub

    Private Function CreateBeneficiaryTableRow(ByVal ssn As String, ByVal lastName As String, ByVal firstName As String, ByVal fundStatus As String, ByVal isAddCSSClass As Boolean) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell
        Try
            mainTableRow = New HtmlTableRow
            If isAddCSSClass Then
                mainTableRow.Attributes.Add("class", "DataGrid_HeaderStyle")
            End If

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 01/05/2017 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = ssn
            mainTableRow.Cells.Add(mainTableCell)

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 01/05/2017 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = lastName
            mainTableRow.Cells.Add(mainTableCell)

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 01/05/2017 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = firstName
            mainTableRow.Cells.Add(mainTableCell)

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 01/05/2017 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = fundStatus
            mainTableRow.Cells.Add(mainTableCell)

            Return mainTableRow
        Catch
            Throw
        Finally
            mainTableCell = Nothing
            mainTableRow = Nothing
        End Try

    End Function

    Private Function CreateBeneficiarySplitDetailsRow(ByVal beneficiaryPersID As String, ByVal beneficiaryFundEventID As String, ByVal splitConfigurationTable As DataTable) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim accountTable As System.Web.UI.HtmlControls.HtmlTable
        Dim accountRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim accountCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim rows As DataRow()

        Dim planType As String
        Dim amount As Decimal, percentage As Decimal

        Dim beneficiaryAccountsTable As DataTable

        Dim accountType As String, personalPreTax As String, personalPostTax As String, personalInterest As String, ymcaTaxable As String, ymcaInterest As String, total As String

        'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265
        Dim feeDetails As DataTable
        Dim retirementFee, savingsFee As Decimal
        Dim feeDetailsText As String

        Dim isRetirementPlanSplitExists, isSavingsPlanSplitExists As Boolean
        'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265
        Try
            'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Handling fee
            retirementFee = 0
            savingsFee = 0
            If HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees) Then
                feeDetails = Me.RecipientBalanceAfterSplitManageFees
                For Each feeRow As DataRow In feeDetails.Rows
                    If Convert.ToString(feeRow("PersId")) = beneficiaryPersID Then
                        retirementFee = Convert.ToDecimal(feeRow("RetirementFee"))
                        savingsFee = Convert.ToDecimal(feeRow("SavingsFee"))
                        isRetirementPlanSplitExists = Convert.ToBoolean(feeRow("IsRetirementPlanSplitExists"))
                        isSavingsPlanSplitExists = Convert.ToBoolean(feeRow("IsSavingsPlanSplitExists"))
                        Exit For
                    End If
                Next

            End If
            'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Handling fee

            accountTable = New HtmlTable()
            accountTable.Attributes.Add("class", "DataGrid_Grid")
            accountTable.Attributes.Add("cellspacing", "0")
            accountTable.Attributes.Add("rules", "all")
            accountTable.Attributes.Add("border", "1")
            accountTable.Attributes.Add("style", "width:100%;border-collapse:collapse;")

            'Add headers
            'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Changed heading
            'accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_HeaderStyle", "Acct", "EmpTaxable", "EmpNon-Taxable", "EmpInterest", "YMCATaxable", "YMCAInterest", "Acct Total "))
            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_HeaderStyle", "Acct", "Taxable", "Non-Taxable", "Interest", "YMCA Taxable", "YMCA Interest", "Acct. Total "))
            'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Changed heading

            rows = splitConfigurationTable.Select(String.Format("PersId='{0}'", beneficiaryPersID))
            For Each row As DataRow In rows
                planType = Convert.ToString(row("PlanType"))
                amount = Convert.ToDecimal(row("Amount"))
                percentage = Convert.ToDecimal(row("Percentage"))

                accountCell = New HtmlTableCell()
                accountCell.Attributes.Add("colspan", "7")
                accountCell.Attributes.Add("style", "text-indent: 20px;")

                'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Conditional display of fee
                If isRetirementPlanSplitExists And isSavingsPlanSplitExists And planType.ToLower() = "both" Then
                    feeDetailsText = String.Format("Fees: Retirement (${0}); Savings (${1})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture), savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
                ElseIf isRetirementPlanSplitExists And planType.ToLower() = "retirement" Then
                    feeDetailsText = String.Format("Fees: Retirement (${0})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
                ElseIf isSavingsPlanSplitExists And planType.ToLower() = "savings" Then
                    feeDetailsText = String.Format("Fees: Savings (${0})", savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
                End If
                'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Conditional display of fee

                If amount > 0 Then
                    'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Added fee
                    'accountCell.InnerText = String.Format("Plan: {0} (Split ${1})", planType, amount.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
                    accountCell.InnerHtml = String.Format("Plan: {0} (Split ${1})&nbsp;&nbsp;&nbsp;&nbsp;{2}", planType, amount.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture), feeDetailsText)
                    'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Added fee
                Else
                    'START: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Added fee
                    'accountCell.InnerText = String.Format("Plan: {0} (Split {1}%)", planType, percentage.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
                    accountCell.InnerHtml = String.Format("Plan: {0} (Split {1}%)&nbsp;&nbsp;&nbsp;&nbsp;{2}", planType, percentage.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture), feeDetailsText)
                    'END: PPP | 01/05/2017 | YRS-AT-3145 & 3265 | Added fee
                End If

                accountRow = New HtmlTableRow()
                accountRow.Attributes.Add("class", "DataGrid_HeaderStyle")
                accountRow.Cells.Add(accountCell)
                accountTable.Rows.Add(accountRow)

                beneficiaryAccountsTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(beneficiaryFundEventID, planType, Me.RecepientAfterSplitAccountsDetail, Me.ParticipantOriginalBalance) 'CreateBeneficiaryAccountTable(beneficiaryFundEventID, planType)
                If HelperFunctions.isNonEmpty(beneficiaryAccountsTable) Then
                    For rowCounter As Integer = 0 To beneficiaryAccountsTable.Rows.Count - 1
                        accountType = beneficiaryAccountsTable.Rows(rowCounter)("AcctType")
                        personalPreTax = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("PersonalPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        personalPostTax = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("PersonalPostTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        personalInterest = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("PersonalInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        ymcaTaxable = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("YMCAPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        ymcaInterest = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("YMCAInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        total = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("TotalTotal")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)

                        If (rowCounter Mod 2) = 0 Then
                            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_NormalStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                        Else
                            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_AlternateStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                        End If
                    Next
                End If
            Next

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("colspan", "4")
            mainTableCell.Controls.Add(accountTable)

            mainTableRow = New HtmlTableRow
            mainTableRow.Cells.Add(mainTableCell)

            Return mainTableRow
        Catch
            Throw
        Finally
            feeDetailsText = Nothing 'PPP | 01/05/2017 | YRS-AT-3145 & 3265 
            feeDetails = Nothing 'PPP | 01/05/2017 | YRS-AT-3145 & 3265 
            accountType = Nothing
            personalPreTax = Nothing
            personalPostTax = Nothing
            personalInterest = Nothing
            ymcaTaxable = Nothing
            ymcaInterest = Nothing
            total = Nothing
            beneficiaryAccountsTable = Nothing
            planType = Nothing
            rows = Nothing
            accountCell = Nothing
            accountRow = Nothing
            accountTable = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
        End Try
    End Function

    Private Function CreateBeneficiarySplitRow(ByVal cssClass As String, ByVal accountType As String, ByVal personalPreTax As String, ByVal personalPostTax As String, ByVal personalInterest As String, ByVal ymcaTaxable As String, ByVal ymcaInterest As String, ByVal total As String) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim accountRow As System.Web.UI.HtmlControls.HtmlTableRow
        'Dim accountCell As System.Web.UI.HtmlControls.HtmlTableCell 'PPP | 01/04/2017 | YRS-AT-3145 & 3265
        Try
            accountRow = New HtmlTableRow
            accountRow.Attributes.Add("class", cssClass)

            'START: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Cell creation is ported out to method, so that it can be controled from one location
            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = accountType
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(accountType))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = personalPreTax
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(personalPreTax))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = personalPostTax
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(personalPostTax))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = personalInterest
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(personalInterest))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = ymcaTaxable
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(ymcaTaxable))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = ymcaInterest
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(ymcaInterest))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = total
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(total))
            'END: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Cell creation is ported out to method, so that it can be controled from one location

            Return accountRow
        Catch
            Throw
        Finally
            'accountCell = Nothing 'PPP | 01/04/2017 | YRS-AT-3145 & 3265
            accountRow = Nothing
        End Try
    End Function

    'END: PPP | 09/02/2016 | YRS-AT-2529 | 

    'START: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Method creates html table cell (<td>)
    Public Function CreateBeneficirySplitCell(ByVal data As String) As HtmlTableCell
        Dim accountCell As New System.Web.UI.HtmlControls.HtmlTableCell
        accountCell.Attributes.Add("align", "right")
        accountCell.InnerText = data
        Return accountCell
    End Function
    'END: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Method creates html table cell (<td>)

    'START: PPP | 09/28/2016 | YRS-AT-2529 | It will determine which Plan has been splited till now, if Ret. is splited in between all ben. then it will return only Retirement, if Ret and Sav is splited among Ben.s then Both will re returned
    Private Function GetParticipantPlanToShowOnSummary() As String
        Dim splitConfigurationTable As DataTable
        Dim planType, definedPlanType As String
        Dim isSavingsPlanExists, isRetirementPlanExists As Boolean
        Try
            planType = "Both"
            isSavingsPlanExists = False
            isRetirementPlanExists = False
            splitConfigurationTable = Me.SplitConfigurationDetails
            If Not splitConfigurationTable Is Nothing Then
                For Each row As DataRow In splitConfigurationTable.Rows
                    definedPlanType = Convert.ToString(row("PlanType")).ToUpper
                    If definedPlanType = "RETIREMENT" Then
                        isRetirementPlanExists = True
                    ElseIf definedPlanType = "SAVINGS" Then
                        isSavingsPlanExists = True
                    End If
                Next
            End If

            If isRetirementPlanExists And Not isSavingsPlanExists Then
                planType = "Retirement"
            ElseIf Not isRetirementPlanExists And isSavingsPlanExists Then
                planType = "Savings"
            End If
            Return planType
        Catch
            Throw
        Finally
            definedPlanType = Nothing
            planType = Nothing
            splitConfigurationTable = Nothing
        End Try
    End Function
    'END: PPP | 09/28/2016 | YRS-AT-2529 | It will determine which Plan has been splited till now, if Ret. is splited in between all ben. then it will return only Retirement, if Ret and Sav is splited among Ben.s then Both will re returned
End Class
