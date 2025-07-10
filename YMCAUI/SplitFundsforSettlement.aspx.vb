'******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		            :	YMCA-YRS
' FileName			            :	SplitFundsforSettlement.aspx.vb
' Author Name		            :	Priya Jawale
' Employee ID		            :	37786
' Email				            :	priya.jawale@3i-infotech.com
' Contact No		            :	8642
' Creation Time		            :	7/01/2008 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			        :	
' Changed on			        :	
' Change Description	        :	
'*******************************************************************************
' Cache-Session                 :   Vipul 02Feb06 
'*******************************************************************************
'Changed By:            On:             IssueId: 
'*******************************************************************************
'Priya                  2008.10.05      Bug ID : 521 Runtime error for decedents with Savings type beneficiary
'Priya                  2008.10.05      Bug id : 528 Spelling should be successful.
'Nikunj Patel           2008.09.15      Uncommenting code in Search Results' selected index changed event.
'Priya                  2008.09.16      Format Values of Datagrid for Retired n saving plan for both deceased and beneficiary in itemdata bound column.
'Priya                  2008-oct-17     Wrong grand total shows on screen for beneficiary
'Nikunj Patel           2009.04.20      Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Neeraj Singh           12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Shashi Shekhar         16/Feb/2010     Restrict Data Archived Participants To proceed in Find list.
'Sanjay R.              2010.06.17      Enhancement changes(CType to DirectCast)
'Sanjay R.              2010.07.12      Code Review changes(Region,variable declarations etc.)
'Shashi Shekhar         2010-12-24      For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi Shekhar         2011-02-10      For YRS 5.0-1236 : Need ability to freeze/lock account
'Shashi Shekhar         14 - Feb -2011  For BT-750 While QDRO split message showing wrong.
'Shashi Shekhar         28 Feb 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Sanjay R.              2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep                2015.05.06      BT:2824:YRS 5.0-2499:Web Service for Password Rewrite
'Manthan Rajguru        2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Threading
Imports System.Reflection

'Imports System.Web
'Imports System.Web.UI
'Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Web.UI.WebControls

Public Class SplitFundsforSettlement
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SplitFundsforSettlement.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

#Region "Local Variables"
    Dim l_dataset_SearchResults As DataSet      'Store search results information of the search that was performed
    Dim l_dataset_ActiveBeneficiaries As DataSet    'Store any active beneficiaries of the deceased participant
    Dim l_dataset_RetiredBeneficiaries As DataSet   'Store any retired beneficiaries of the deceased participant
    Dim l_dataset_ActiveSettlementOption_RetirementPlan As DataSet  'Store Retirement Plan Settlement Options for Active Beneficiaries
    Dim l_dataset_ActiveSettlementOption_SavingsPlan As DataSet     'Store Savings Plan Settlement Options for Active Beneficiaries
    Dim l_dataset_RetiredSettlementOption_RetirementPlan As DataSet 'Store Retirement Plan Settlement Options for Retired Beneficiaries
    Dim l_dataset_RetiredSettlementOption_SavingsPlan As DataSet    'Store Savings Plan Settlement Options for Retired Beneficiaries
    Dim l_dataset_DeceasedInformation As DataSet 'Store Bebeficiary and Deceased Information
    Dim stringBeneficiaryId As String

#End Region

    Protected WithEvents TabStripBeneficiarySettlement As Microsoft.Web.UI.WebControls.TabStrip
    'Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder

    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button

    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridSearchResults As System.Web.UI.WebControls.DataGrid


    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents Label_Decedent As System.Web.UI.WebControls.Label
    Protected WithEvents Label_Beneficiary As System.Web.UI.WebControls.Label

    Protected WithEvents DataGridActiveBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridBeneficiary_RetirementPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridActiveBenefitOptions_SavingsPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridBeneficiary_SavingPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridActiveBenefitOptions_RetirementPlan As System.Web.UI.WebControls.DataGrid


    Protected WithEvents label_GrandTotal_Beneficiary_Taxable As System.Web.UI.WebControls.Label
    Protected WithEvents label_GrandTotal_Beneficiary_NonTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents label_GrandTotal_Beneficiary_YMCA As System.Web.UI.WebControls.Label
    Protected WithEvents label_GrandTotal_Beneficiary_Total As System.Web.UI.WebControls.Label

    Protected WithEvents label_GrandTotal_Deceased_Taxable As System.Web.UI.WebControls.Label
    Protected WithEvents label_GrandTotal_Deceased_NonTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents label_GrandTotal_Deceased_YMCA As System.Web.UI.WebControls.Label
    Protected WithEvents label_GrandTotal_Deceased_Total As System.Web.UI.WebControls.Label


    Protected WithEvents MultiPageSplitFundsSettlement As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonSplitfundsforBeneficiaries As System.Web.UI.WebControls.Button
    Protected WithEvents label_RecorNotFound As System.Web.UI.WebControls.Label
    Protected WithEvents label_Split_Error_Messages As System.Web.UI.WebControls.Label
    Protected WithEvents panel_BeneficiaryDetails As System.Web.UI.WebControls.Panel
    Protected WithEvents Panel_Split_Error_Messages As System.Web.UI.WebControls.Panel
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'PRIYA
#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        ViewState("Temp") = String.Empty
        StoreLocalVariablesToCache()
        Return MyBase.SaveViewState()
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        ViewState("Temp") = String.Empty
        InitializeLocalVariablesFromCache()
        MyBase.LoadViewState(savedState)
    End Sub

    Private Sub InitializeLocalVariablesFromCache()
        'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
        l_dataset_SearchResults = Session("BSF_l_dataset_SearchResults")
        l_dataset_ActiveBeneficiaries = Session("BSF_l_dataset_ActiveBeneficiaries")
        l_dataset_DeceasedInformation = Session("BSF_l_dataset_DeceasedInformation")
        stringBeneficiaryId = Session("BeneficiaryId")
        'l_dataset_RetiredBeneficiaries = Session("BSF_l_dataset_RetiredBeneficiaries")
        'l_dataset_ActiveSettlementOption_RetirementPlan = Session("BSF_l_dataset_ActiveSettlementOption_RetirementPlan")
        'l_dataset_ActiveSettlementOption_SavingsPlan = Session("BSF_l_dataset_ActiveSettlementOption_SavingsPlan")
        'l_dataset_RetiredSettlementOption_RetirementPlan = Session("BSF_l_dataset_RetiredSettlementOption_RetirementPlan")
        'l_dataset_RetiredSettlementOption_SavingsPlan = Session("BSF_l_dataset_RetiredSettlementOption_SavingsPlan")
    End Sub
    Private Sub StoreLocalVariablesToCache()
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("BSF_l_dataset_SearchResults") = l_dataset_SearchResults
        Session("BSF_l_dataset_ActiveBeneficiaries") = l_dataset_ActiveBeneficiaries
        Session("BSF_l_dataset_DeceasedInformation") = l_dataset_DeceasedInformation
        Session("BeneficiaryId") = stringBeneficiaryId
        'Session("BSF_l_dataset_RetiredBeneficiaries") = l_dataset_RetiredBeneficiaries
        'Session("BSF_l_dataset_ActiveSettlementOption_RetirementPlan") = l_dataset_ActiveSettlementOption_RetirementPlan
        'Session("BSF_l_dataset_ActiveSettlementOption_SavingsPlan") = l_dataset_ActiveSettlementOption_SavingsPlan
        'Session("BSF_l_dataset_RetiredSettlementOption_RetirementPlan") = l_dataset_RetiredSettlementOption_RetirementPlan
        'Session("BSF_l_dataset_RetiredSettlementOption_SavingsPlan") = l_dataset_RetiredSettlementOption_SavingsPlan
    End Sub
#End Region
    'PRIYA
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'display the menu here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            If Not IsPostBack Then
                Me.TabStripBeneficiarySettlement.Items(1).Enabled = False
                If HelperFunctions.isEmpty(l_dataset_DeceasedInformation) Then
                    ButtonSplitfundsforBeneficiaries.Enabled = False
                Else
                    ButtonSplitfundsforBeneficiaries.Enabled = True
                End If
            End If

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            'Priya

            If Not Request.Form("Yes") Is Nothing Then  ' Action to be taken if user selected YES
                If Request.Form("Yes").Trim.ToUpper = "YES" Then
                    DoSplitFund()
                    ButtonSplitfundsforBeneficiaries.Enabled = False
                End If
            End If
            'End priya
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'this function is used to split fund.
    Private Sub DoSplitFund()
        Dim string_PerssID As String
        Dim string_FundEventId As String
        Dim l_dataset_FundSplit As DataSet
        Dim errorMsg As String
        Try
            If Not IsNothing(l_dataset_DeceasedInformation) Then


                string_PerssID = Convert.ToString(l_dataset_DeceasedInformation.Tables(0).Rows(0).Item("GuiPersID"))
                string_FundEventId = Convert.ToString(l_dataset_DeceasedInformation.Tables(0).Rows(0).Item("guiFundEventID"))
                l_dataset_FundSplit = YMCARET.YmcaBusinessObject.SplitFundsforSettlement.LookUp_DeceasedInformation(string_PerssID, Session("g_DeathDate"), "YES", string_FundEventId, errorMsg)

                If (errorMsg <> String.Empty) Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", errorMsg, MessageBoxButtons.Stop)
                    ButtonSplitfundsforBeneficiaries.Enabled = False
                    Exit Sub
                End If
                'Bug id : 528 Priya Spelling should be successful.
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Split fund process successful", MessageBoxButtons.OK)
                'End Bug id : 528
            End If
        Catch ex As Exception
            Throw
        End Try
        'afer success for fun from sql shows followinbg message as per PS
        'Split funds process sucessful
    End Sub

    Private Sub TabStripBeneficiarySettlement_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabStripBeneficiarySettlement.SelectedIndexChange
        Try
            MultiPageSplitFundsSettlement.SelectedIndex = TabStripBeneficiarySettlement.SelectedIndex
            'PRIYA 26-june-2008
            If (MultiPageSplitFundsSettlement.SelectedIndex = 0) Then
                'initialize loacl variables from session
                'ButtonPrint.Visible = True
                InitializeLocalVariablesFromCache()
                'Bing grid to show search criteria
                BindGrid(DataGridSearchResults, l_dataset_SearchResults)
            Else
                'ButtonPrint.Visible = False
            End If
            'PRIYA 26-june-2008
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

#Region "Search screen related code"
    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            'Perform Validations
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")    'Feature: User can put SSN in 222-22-2222 format also
            If TextBoxSSNo.Text.Trim = "" AndAlso TextBoxLastName.Text.Trim = "" AndAlso TextBoxFirstName.Text.Trim = "" AndAlso TextBoxFundNo.Text.Trim = "" Then
                BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
                MessageBox.Show(PlaceHolder1, "Beneficiary Settlement ", "Please Enter a Search Value. ", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'l_dataset_SearchResults = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_MemberListForDeceased(TextBoxSSNo.Text.Trim, TextBoxLastName.Text.Trim, TextBoxFirstName.Text.Trim, TextBoxFundNo.Text.Trim)
            l_dataset_SearchResults = YMCARET.YmcaBusinessObject.SplitFundsforSettlement.LookUp_MemberListForSplitFunds(TextBoxSSNo.Text.Trim, TextBoxLastName.Text.Trim, TextBoxFirstName.Text.Trim, TextBoxFundNo.Text.Trim)
            'l_dataset_SearchResults = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.LookUp_DeathCalc_MemberListForDeath(Me.TextBoxSSNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim, TextBoxFundNo.Text.Trim)

            'Check if any results were returned??
            If HelperFunctions.isEmpty(l_dataset_SearchResults) Then
                'Changed By:preeti On:9thFeb06 IssueId:YRST-2092
                'Me.DataGridSearchResults.AllowSorting = False
                ' Me.TabStripBeneficiarySettlement.Items(0).Enabled = False
                Me.TabStripBeneficiarySettlement.Items(1).Enabled = False
                BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
                label_RecorNotFound.Visible = True
                label_RecorNotFound.Text = "No Records Found."
                'MessageBox.Show(PlaceHolder1, "Beneficiary Settlement ", "No Records Found. ", MessageBoxButtons.Stop)
                Exit Sub
            End If
            StoreLocalVariablesToCache()
            label_RecorNotFound.Visible = False
            TabStripBeneficiarySettlement.Items(1).Enabled = False  'Default - cannot select
            CleanUpExistingBenefitOptions()     'Default - Cleanup
            'NP:PS:2007.09.13 - Not selecting the first result by default. - DataGridSearchResults.SelectedIndex = 0
            BindGrid(DataGridSearchResults, l_dataset_SearchResults)

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub
    Private Sub DataGridSearchResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridSearchResults.SelectedIndexChanged
        'When a Participant is selected in Search Grid 
        Try
            '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
            If Me.DataGridSearchResults.SelectedItem.Cells(6).Text.ToUpper.Trim() = "TRUE" Then
                MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                Me.TabStripBeneficiarySettlement.Items(1).Enabled = False

                Exit Sub
            End If
            '---------------------------------------------------------------------------------------
            'Shashi Shekhar:10 Feb 2011:For YRS 5.0-1236
            Session("IsLocked") = Nothing
            If Me.DataGridSearchResults.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                Session("IsLocked") = "True"
            Else
                Session("IsLocked") = "False"
            End If

            Session("splitfundSSN") = Me.DataGridSearchResults.SelectedItem.Cells(1).Text.Trim
            '------------------------------------------------------------------------------------------------





            Process_BeneficiaryDetails()

            'Check if only one tab is enabled and if so then take the user there
            'PRIYA 26-june-2008 - make tabstrip enable is true
            Me.TabStripBeneficiarySettlement.Items(0).Enabled = True
            Me.TabStripBeneficiarySettlement.Items(1).Enabled = True
            'END PRIYA 26-june-2008
            'Enable the beneficiary details tab. 
            TabStripBeneficiarySettlement.SelectedIndex = 1
            TabStripBeneficiarySettlement_SelectedIndexChange(TabStripBeneficiarySettlement, Nothing)

            'NP:PS:2007.09.18 - Changing code to Handle display of the grid properly after sorting
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                BindGrid(DataGridSearchResults, l_dataset_SearchResults.Tables(0).DefaultView)
            Else
                BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
            End If
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception   'NP:2008.09.15 - Uncommenting code to catch unknown errors
            Response.Write(ex.Message)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'NP:PS:2007.09.18 - Handling the Sorting and selection of results properly
    Private Sub DataGridSearchResults_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridSearchResults.SortCommand
        Try
            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are search results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGridSearchResults.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = IIf(l_dataset_SearchResults.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                BindGrid(DataGridSearchResults, l_dataset_SearchResults.Tables(0).DefaultView)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub CleanUpExistingBenefitOptions()
        'Cleanup all local variables for Active Beneficiaries and Retired Beneficiaries
        'VP-TMP' l_dataset_ActiveBeneficiaries = Nothing : BindGrid(DataGridActiveBeneficiaries, l_dataset_ActiveBeneficiaries)
        'VP-TMP' l_dataset_ActiveSettlementOption_RetirementPlan = Nothing : BindGrid(DataGridActiveBenefitOptions_RetirementPlan, l_dataset_ActiveSettlementOption_RetirementPlan)
        'VP-TMP' l_dataset_ActiveSettlementOption_SavingsPlan = Nothing : BindGrid(DataGridActiveBenefitOptions_SavingsPlan, l_dataset_ActiveSettlementOption_SavingsPlan)
        'VP-TMP' l_dataset_RetiredBeneficiaries = Nothing : BindGrid(DataGridRetiredBeneficiaries, l_dataset_RetiredBeneficiaries)
        'VP-TMP' l_dataset_RetiredSettlementOption_RetirementPlan = Nothing : BindGrid(DataGridRetiredBenefitOptions_RetirementPlan, l_dataset_RetiredSettlementOption_RetirementPlan)
        'VP-TMP' l_dataset_RetiredSettlementOption_SavingsPlan = Nothing : BindGrid(DataGridRetiredBenefitOptions_SavingsPlan, l_dataset_RetiredSettlementOption_SavingsPlan)
    End Sub
#End Region


    'This Utility section works as a charm
#Region "General Utility Functions"
    'dg = The datagrid to bind data to
    'ds = The dataset which contains the data
    'forceVisible = Whether the datagrid should be displayed if it does not contain any data
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        Try
            If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                dg.DataSource = Nothing
                dg.DataBind()
                dg.Visible = forceVisible
                Exit Sub
            Else
                dg.DataSource = ds.Tables(0)
                dg.DataBind()
                dg.Visible = True
            End If
        Catch
            Throw
        End Try

    End Sub
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        Try
            If dv Is Nothing OrElse dv.Count = 0 Then
                dg.DataSource = Nothing
                dg.DataBind()
                dg.Visible = forceVisible
                Exit Sub
            Else
                dg.DataSource = dv
                dg.DataBind()
                dg.Visible = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

    'This function is the first entry point for Beneficiary Settlement. This function populates the various 
    'datasets for active and retired beneficiaries and also populates the settlement options for the selected
    'beneficiary in their own datasets.

    Private Sub Process_BeneficiaryDetails()
        'Pick the row that matches the SSN of the selected participant to obtain the PersId
        Dim l_string_persID As String   'Store the participant ID
        Dim arrDr() As DataRow
        Dim dr As DataRow
        Dim g_DeathDate As DateTime

        If HelperFunctions.isEmpty(l_dataset_SearchResults) Then Exit Sub 'We do not have any search results to work with
        dr = l_dataset_SearchResults.Tables(0).DefaultView.Item(DataGridSearchResults.SelectedIndex).Row
        If dr Is Nothing Then Exit Sub 'No Matches found - highly unlikely

        g_DeathDate = Date.Parse(dr("Death Date"))
        Session("g_DeathDate") = g_DeathDate


        If dr.IsNull("PersID") Then
            'If the Participants' Id is not available then we cannot proceed. Exit sub.
            l_string_persID = "" : Exit Sub
        End If

        l_string_persID = dr("PersID").ToString().Trim()
        Session("BAC_PersID") = l_string_persID

        'Shashi Shekhar     28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
        Headercontrol.pageTitle = "Split Fund For Settlement"
        Headercontrol.guiPerssId = dr("PersID").ToString().Trim()


        Me.Label_Decedent.Text = "Decedent Information - " + Left(dr("SS No").ToString().Trim(), 3) + "-" + dr("SS No").ToString().Substring(3, 2).Trim() + "-" + dr("SS No").ToString().Substring(5, 4).Trim()
        l_dataset_ActiveBeneficiaries = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_BeneficiariesPrimeSettle(l_string_persID, "DA")


        If HelperFunctions.isNonEmpty(l_dataset_ActiveBeneficiaries) Then
            DataGridActiveBeneficiaries.SelectedIndex = 0
            stringBeneficiaryId = Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(0).Item(0))

            Dim string_SSNO_Selected_Beneficiary As String
            string_SSNO_Selected_Beneficiary = Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(0).Item(6))

            If string_SSNO_Selected_Beneficiary = string_SSNO_Selected_Beneficiary.Empty Or string_SSNO_Selected_Beneficiary = "" Then
                Label_Beneficiary.Text = "Beneficiary Information - " + Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(0).Item(5))

            Else
                Label_Beneficiary.Text = "Beneficiary Information - " + Left(string_SSNO_Selected_Beneficiary.Trim(), 3) + "-" + string_SSNO_Selected_Beneficiary.Substring(3, 2).Trim() + "-" + string_SSNO_Selected_Beneficiary.Substring(5, 4).Trim()
            End If

            'if string_ssno_selected_beneficiary.empty <> string.empty then
            '    label_beneficiary.text = "beneficiary information - " + left(string_ssno_selected_beneficiary.trim(), 3) + "-" + string_ssno_selected_beneficiary.substring(3, 2).trim() + "-" + string_ssno_selected_beneficiary.substring(5, 4).trim()
            'end if
            BindGrid(DataGridActiveBeneficiaries, l_dataset_ActiveBeneficiaries)

            'PRIYA As on 04-08-08
            'Showing Beneficiary details.
            'YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_BeneficiaryInformation(l_string_persID, "DA")
            Try
                'l_dataset_DeceasedInformation = YMCARET.YmcaBusinessObject.SplitFundsforSettlement.LookUp_DeceasedInformation(l_string_persID, System.DateTime.Now(),"NA", "")
                Dim stringFundEventId As String
                stringFundEventId = YMCARET.YmcaBusinessObject.SplitFundsforSettlement.LookUp_FundEventIdForSplitFunds(l_string_persID)
                Dim errorMsg As String
                l_dataset_DeceasedInformation = YMCARET.YmcaBusinessObject.SplitFundsforSettlement.LookUp_DeceasedInformation(l_string_persID, g_DeathDate, "NO", stringFundEventId, errorMsg)

                If (errorMsg <> String.Empty) Then

                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", errorMsg, MessageBoxButtons.Stop)

                    label_Split_Error_Messages.Visible = True
                    Panel_Split_Error_Messages.Visible = True
                    label_Split_Error_Messages.Text = errorMsg.Replace(Chr(13), "<BR/>") '&vbcrlf)
                    panel_BeneficiaryDetails.Visible = False

                    ButtonSplitfundsforBeneficiaries.Enabled = False
                    DataGridActiveBenefitOptions_RetirementPlan.Visible = False
                    DataGridActiveBenefitOptions_SavingsPlan.Visible = False
                    DatagridBeneficiary_RetirementPlan.Visible = False
                    DatagridBeneficiary_SavingPlan.Visible = False

                    label_GrandTotal_Beneficiary_Taxable.Text = " "
                    label_GrandTotal_Beneficiary_NonTaxable.Text = " "
                    label_GrandTotal_Beneficiary_YMCA.Text = " "
                    label_GrandTotal_Beneficiary_Total.Text = " "

                    label_GrandTotal_Deceased_Taxable.Text = " "
                    label_GrandTotal_Deceased_NonTaxable.Text = " "
                    label_GrandTotal_Deceased_YMCA.Text = " "
                    label_GrandTotal_Deceased_Total.Text = " "

                    Exit Sub
                Else
                    label_Split_Error_Messages.Visible = False
                    Panel_Split_Error_Messages.Visible = False
                    panel_BeneficiaryDetails.Visible = True
                End If

                If (l_dataset_DeceasedInformation.Tables(1).Rows.Count > 0) Then

                    ButtonSplitfundsforBeneficiaries.Enabled = True

                    'Beneficiary Information for Saving                                          
                    l_dataset_DeceasedInformation.Tables(1).DefaultView.RowFilter = "PlanType = 'RETIREMENT' AND  UniqueId = '" & Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(0).Item(0)) & "'"
                    BindGrid(DatagridBeneficiary_RetirementPlan, l_dataset_DeceasedInformation.Tables(1).DefaultView)

                    '2008-oct-17 : Priya Wrong grand total shows on screen
                    l_dataset_DeceasedInformation.Tables(1).DefaultView.RowFilter = "PlanType = 'SAVINGS' AND  UniqueId = '" & Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(0).Item(0)) & "'"
                    BindGrid(DatagridBeneficiary_SavingPlan, l_dataset_DeceasedInformation.Tables(1).DefaultView)
                    'End 2008-oct-17 

                    'Dim stringBeneficiaryId As String
                    'stringBeneficiaryId = Convert.ToString(l_dataset_DeceasedInformation.Tables(1).Rows(0).Item(0))

                    'label_GrandTotal_Beneficiary_Taxable.Text = Convert.ToString(Math.Round(Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")), 2))
                    'label_GrandTotal_Beneficiary_NonTaxable.Text = Convert.ToString(Math.Round(Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", " UniqueId = '" & stringBeneficiaryId & "'")), 2))
                    'label_GrandTotal_Beneficiary_YMCA.Text = Convert.ToString(Math.Round(Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")), 2))
                    'label_GrandTotal_Beneficiary_Total.Text = Convert.ToString(Math.Round(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'") + l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", " UniqueId = '" & stringBeneficiaryId & "'") + l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", " UniqueId = '" & stringBeneficiaryId & "'"), 2))
                    ''Convert.ToString(Math.Round(Convert.ToDecimal(label_GrandTotal_Beneficiary_Taxable.Text) + Convert.ToDecimal(label_GrandTotal_Beneficiary_NonTaxable.Text) + Convert.ToDecimal(label_GrandTotal_Beneficiary_YMCA.Text), 2))
                    If Not IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")) Then
                        label_GrandTotal_Beneficiary_Taxable.Text = Math.Round(Convert.ToDouble(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")), 2).ToString("###,###,##0.00") 'Convert.ToString(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")
                    Else
                        label_GrandTotal_Beneficiary_Taxable.Text = 0
                    End If


                    If Not IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", " UniqueId = '" & stringBeneficiaryId & "'")) Then
                        label_GrandTotal_Beneficiary_NonTaxable.Text = Math.Round(Convert.ToDouble(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", " UniqueId = '" & stringBeneficiaryId & "'")), 2).ToString("###,###,##0.00")
                    Else
                        label_GrandTotal_Beneficiary_NonTaxable.Text = 0
                    End If


                    If Not IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")) Then
                        label_GrandTotal_Beneficiary_YMCA.Text = (Math.Round(Convert.ToDouble(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")), 2)).ToString("###,###,##0.00")
                    Else
                        label_GrandTotal_Beneficiary_YMCA.Text = 0
                    End If

                    label_GrandTotal_Beneficiary_Total.Text = Math.Round(Convert.ToDouble(label_GrandTotal_Beneficiary_Taxable.Text) + Convert.ToDouble(label_GrandTotal_Beneficiary_NonTaxable.Text) + Convert.ToDouble(label_GrandTotal_Beneficiary_YMCA.Text), 2).ToString("###,###,##0.00")


                End If

                If (l_dataset_DeceasedInformation.Tables(0).Rows.Count > 0) Then

                    ButtonSplitfundsforBeneficiaries.Enabled = True

                    'Deceased Information
                    l_dataset_DeceasedInformation.Tables(0).DefaultView.RowFilter = "PlanType = 'RETIREMENT'"
                    BindGrid(DataGridActiveBenefitOptions_RetirementPlan, l_dataset_DeceasedInformation.Tables(0).DefaultView)

                    l_dataset_DeceasedInformation.Tables(0).DefaultView.RowFilter = "PlanType = 'SAVINGS'"
                    BindGrid(DataGridActiveBenefitOptions_SavingsPlan, l_dataset_DeceasedInformation.Tables(0).DefaultView)

                    label_GrandTotal_Deceased_Taxable.Text = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPreTax)", " ")).ToString("###,###,##0.00")
                    label_GrandTotal_Deceased_NonTaxable.Text = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPostTax)", " ")).ToString("###,###,##0.00")
                    label_GrandTotal_Deceased_YMCA.Text = Convert.ToDecimal(Math.Round(Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyYmcaPreTax)", "")), 2)).ToString("###,###,##0.00")

                    label_GrandTotal_Deceased_Total.Text = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPreTax)", " ") + l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPostTax)", " ") + l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyYmcaPreTax)", "")).ToString("###,###,##0.00")
                    'Convert.ToString(Convert.ToDecimal(label_GrandTotal_Beneficiary_Taxable.Text) + Convert.ToDecimal(label_GrandTotal_Beneficiary_NonTaxable.Text) + Convert.ToDecimal(label_GrandTotal_Beneficiary_YMCA.Text))

                End If
            Catch ex As Exception
                Throw ex
            End Try
        Else
            Me.TabStripBeneficiarySettlement.SelectedIndex = 0
            Me.TabStripBeneficiarySettlement.Items(1).Enabled = False
            ButtonSplitfundsforBeneficiaries.Enabled = False
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "No beneficiary has been defined, atleast 1 Primary Beneficiary of type MEMBER must be defined for funds to be split", MessageBoxButtons.Stop)
        End If
    End Sub
    'PRIYA
    Private Sub DataGridActiveBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBeneficiaries.SelectedIndexChanged
        Try
            ' InitializeLocalVariablesFromCache()
            Dim i As Integer = DataGridActiveBeneficiaries.SelectedIndex
            ' BindGrid(DataGridActiveBeneficiaries, l_dataset_ActiveBeneficiaries)    'NP:PS:2007.08.31 - Adding code to change the image of the button when the row is selected
            'If isNonEmpty(l_dataset_DeceasedInformation) AndAlso isNonEmpty(l_dataset_DeceasedInformation.Tables(1)) Then
            If HelperFunctions.isNonEmpty(l_dataset_ActiveBeneficiaries) Then

                Dim stringBeneficiaryInform As String
                stringBeneficiaryInform = Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(i).Item(6))

                'Label_Beneficiary.Text = "Beneficiary Information - " + Left(stringBeneficiaryInform.Trim(), 3) + "-" + stringBeneficiaryInform.Substring(3, 2).Trim() + "-" + stringBeneficiaryInform.Substring(5, 4).Trim()
                If stringBeneficiaryInform = stringBeneficiaryInform.Empty Or stringBeneficiaryInform = "" Then
                    Label_Beneficiary.Text = "Beneficiary Information - " + Convert.ToString(l_dataset_ActiveBeneficiaries.Tables(0).Rows(i).Item(5))

                Else
                    Label_Beneficiary.Text = "Beneficiary Information - " + Left(stringBeneficiaryInform.Trim(), 3) + "-" + stringBeneficiaryInform.Substring(3, 2).Trim() + "-" + stringBeneficiaryInform.Substring(5, 4).Trim()
                End If
            End If

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'PRIYA

    Private Sub ButtonSplitfundsforBeneficiaries_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSplitfundsforBeneficiaries.Click
        Try
            Dim strWSMessage As String
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account

            If Not Session("IsLocked") = Nothing Then
                If Session("IsLocked").ToString.Trim.ToLower = "true" Then

                    Dim l_dsLockResDetails As DataSet
                    Dim l_reasonLock As String
                    'Shashi Shekhar : 14 - Feb -2011: For BT-750 While QDRO split message showing wrong.
                    If Not Session("splitfundSSN") = Nothing Then
                        l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(Session("splitfundSSN").ToString().Trim)
                    End If

                    If Not l_dsLockResDetails Is Nothing Then
                        If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                            If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then

                                l_reasonLock = l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
                            End If
                        End If
                    End If
                    If l_reasonLock = "" Then
                        MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Participant account is locked. Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                    Else
                        MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Participant account is locked due to " + l_reasonLock + "." + " Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                    End If
                    Exit Sub
                End If
            End If
            '-------------------------------------------------------------------------------------
            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("BAC_PersID"))
            If strWSMessage <> "NoPending" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Process Restricted", "openDialog('" + strWSMessage + "','Pers');", True)

                Exit Sub
            End If
            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            MessageBox.Show(Me.PlaceHolder1, "YMCA_YRS", "Are you sure you want to split funds for this decedent?", MessageBoxButtons.YesNo, False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub AssignGrandTotalBeneficiariesValues(ByVal stringBeneficiaryId As String)
        Try
            If HelperFunctions.isNonEmpty(l_dataset_DeceasedInformation) AndAlso HelperFunctions.isNonEmpty(l_dataset_DeceasedInformation.Tables(1)) Then
                Dim decimal_GrandTotal_Beneficiary_Taxable As Decimal
                Dim decimal_GrandTotal_Beneficiary_NonTaxable As Decimal
                Dim decimal_GrandTotal_Beneficiary_YMCA As Decimal
                Dim decimal_GrandTotal_Beneficiary_Total As Decimal

                'Bug ID : 521 Priya runtime error of math.round
                If Not IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")) Then
                    decimal_GrandTotal_Beneficiary_Taxable = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", " UniqueId = '" & stringBeneficiaryId & "'"))
                End If

                If Not IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", " UniqueId = '" & stringBeneficiaryId & "'")) Then
                    decimal_GrandTotal_Beneficiary_NonTaxable = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", " UniqueId = '" & stringBeneficiaryId & "'"))
                End If

                If Not IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", " UniqueId = '" & stringBeneficiaryId & "'")) Then
                    decimal_GrandTotal_Beneficiary_YMCA = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", " UniqueId = '" & stringBeneficiaryId & "'"))
                End If

                label_GrandTotal_Beneficiary_Taxable.Text = decimal_GrandTotal_Beneficiary_Taxable.ToString("###,###,##0.00")
                label_GrandTotal_Beneficiary_NonTaxable.Text = decimal_GrandTotal_Beneficiary_NonTaxable.ToString("###,###,##0.00")
                label_GrandTotal_Beneficiary_YMCA.Text = decimal_GrandTotal_Beneficiary_YMCA.ToString("###,###,##0.00")
                label_GrandTotal_Beneficiary_Total.Text = (decimal_GrandTotal_Beneficiary_Taxable + decimal_GrandTotal_Beneficiary_NonTaxable + decimal_GrandTotal_Beneficiary_YMCA).ToString("###,###,##0.00")
                'End of Bug ID : 521 
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DataGridActiveBeneficiaries_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridActiveBeneficiaries.ItemCommand
        If e.CommandName.ToUpper = "SELECT" Then
            Try
                stringBeneficiaryId = Convert.ToString(e.CommandArgument)
                AssignGrandTotalBeneficiariesValues(stringBeneficiaryId)
                'If Not IsNothing(l_dataset_DeceasedInformation) And isNonEmpty(l_dataset_DeceasedInformation.Tables(1)) Then
                If HelperFunctions.isNonEmpty(l_dataset_DeceasedInformation) AndAlso HelperFunctions.isNonEmpty(l_dataset_DeceasedInformation.Tables(1)) Then
                    'Retirement Plan Information
                    l_dataset_DeceasedInformation.Tables(1).DefaultView.RowFilter = "PlanType = 'RETIREMENT' AND UniqueId = '" & Convert.ToString(e.CommandArgument) & "'"
                    'l_dataset_DeceasedInformation.Tables(1).DefaultView.Table.Select("UniqueId = '" & Convert.ToString(e.CommandArgument) & "'")
                    BindGrid(DatagridBeneficiary_RetirementPlan, l_dataset_DeceasedInformation.Tables(1).DefaultView)

                    'Saving Plan Information
                    l_dataset_DeceasedInformation.Tables(1).DefaultView.RowFilter = "PlanType = 'SAVINGS' AND UniqueId = '" & Convert.ToString(e.CommandArgument) & "'"
                    'l_dataset_DeceasedInformation.Tables(1).DefaultView.Table.Select("UniqueId = '" & Convert.ToString(e.CommandArgument) & "'")
                    BindGrid(DatagridBeneficiary_SavingPlan, l_dataset_DeceasedInformation.Tables(1).DefaultView)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub
    Private Sub DataGridActiveBenefitOptions_RetirementPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBenefitOptions_RetirementPlan.ItemDataBound
        Dim l_label_Deceased_Ret_Total As Label
        Dim l_label_Deceased_Ret_taxable As Label
        Dim l_label_Deceased_Ret_NonTaxable As Label
        Dim l_label_Deaceased_Ret_YMCA As Label

        Dim l_label_Taxable_Deceased_Ret_Total As Label
        Dim l_label_NonTaxable_Deceased_Ret_Total As Label
        Dim l_label_YMCA_Deceased_Ret_Total As Label
        Dim l_label_Total_Deceased_Ret_Total As Label
        Try

            If (e.Item.ItemIndex <> -1) Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    'l_label_Deceased_Ret_Total = CType(e.Item.FindControl("label_Deceased_Ret_Total"), Label)  'commented by SR:2010.06.17 for migration
                    l_label_Deceased_Ret_Total = DirectCast(e.Item.FindControl("label_Deceased_Ret_Total"), Label)

                    If Not IsNothing(l_label_Deceased_Ret_Total) Then
                        'l_label_Deceased_Ret_taxable = CType(e.Item.FindControl("label_Deceased_Ret_taxable"), Label)  'commented by SR:2010.06.17 for migration
                        l_label_Deceased_Ret_taxable = DirectCast(e.Item.FindControl("label_Deceased_Ret_taxable"), Label)
                        If Not IsNothing(l_label_Deceased_Ret_taxable) Then
                            l_label_Deceased_Ret_taxable.Text = Decimal.Parse(l_label_Deceased_Ret_taxable.Text).ToString("###,###,##0.00")
                            l_label_Deceased_Ret_Total.Text = Convert.ToDecimal(l_label_Deceased_Ret_taxable.Text).ToString("###,###,##0.00")
                        End If

                        'l_label_Deceased_Ret_NonTaxable = CType(e.Item.FindControl("label_Deceased_Ret_NonTaxable"), Label)  'commented by SR:2010.06.17 for migration
                        l_label_Deceased_Ret_NonTaxable = DirectCast(e.Item.FindControl("label_Deceased_Ret_NonTaxable"), Label)
                        If Not IsNothing(l_label_Deceased_Ret_NonTaxable) Then
                            l_label_Deceased_Ret_NonTaxable.Text = Decimal.Parse(l_label_Deceased_Ret_NonTaxable.Text).ToString("###,###,##0.00")
                            l_label_Deceased_Ret_Total.Text = (Convert.ToDecimal(l_label_Deceased_Ret_Total.Text) + Convert.ToDecimal(l_label_Deceased_Ret_NonTaxable.Text)).ToString("###,###,##0.00")
                            'Convert.ToString(Convert.ToDecimal(l_label_Deceased_Ret_Total.Text) + Convert.ToDecimal(l_label_Deceased_Ret_NonTaxable.Text)).ToString("###,###,##0.00")
                        End If

                        l_label_Deaceased_Ret_YMCA = e.Item.FindControl("label_Deaceased_Ret_YMCA")
                        If Not IsNothing(l_label_Deaceased_Ret_YMCA) Then
                            l_label_Deaceased_Ret_YMCA.Text = Decimal.Parse(l_label_Deaceased_Ret_YMCA.Text).ToString("###,###,##0.00")
                            l_label_Deceased_Ret_Total.Text = (Convert.ToDecimal(l_label_Deceased_Ret_Total.Text) + Convert.ToDecimal(l_label_Deaceased_Ret_YMCA.Text)).ToString("###,###,##0.00")
                            'Convert.ToString(Convert.ToDecimal(l_label_Deceased_Ret_Total.Text) + Convert.ToDecimal(l_label_Deaceased_Ret_YMCA.Text)).ToString("###,###,##0.00")
                        End If

                    End If

                End If
            End If

            If e.Item.ItemType = ListItemType.Footer Then
                'Assiging total of Taxable fund
                'l_label_Taxable_Deceased_Ret_Total = CType(e.Item.FindControl("label_Taxable_Deceased_Ret_Total"), Label)  'commented by SR:2010.06.17 for migration
                l_label_Taxable_Deceased_Ret_Total = DirectCast(e.Item.FindControl("label_Taxable_Deceased_Ret_Total"), Label)
                If Not IsNothing(l_label_Taxable_Deceased_Ret_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'RETIREMENT'")) Then
                        l_label_Taxable_Deceased_Ret_Total.Text = 0
                    Else
                        l_label_Taxable_Deceased_Ret_Total.Text = CType(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'RETIREMENT'"), Decimal).ToString("###,###,##0.00")
                    End If


                End If

                'Assiging total of NonTaxable fund
                'l_label_NonTaxable_Deceased_Ret_Total = CType(e.Item.FindControl("label_NonTaxable_Deceased_Ret_Total"), Label)   'commented by SR:2010.06.17 for migration
                l_label_NonTaxable_Deceased_Ret_Total = DirectCast(e.Item.FindControl("label_NonTaxable_Deceased_Ret_Total"), Label)
                If Not IsNothing(l_label_NonTaxable_Deceased_Ret_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'RETIREMENT'")) Then
                        l_label_NonTaxable_Deceased_Ret_Total.Text = 0
                    Else
                        l_label_NonTaxable_Deceased_Ret_Total.Text = CType(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'RETIREMENT'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of YMCA fund
                'l_label_YMCA_Deceased_Ret_Total = CType(e.Item.FindControl("label_YMCA_Deceased_Ret_Total"), Label)     'commented by SR:2010.06.17 for migration
                l_label_YMCA_Deceased_Ret_Total = DirectCast(e.Item.FindControl("label_YMCA_Deceased_Ret_Total"), Label)
                If Not IsNothing(l_label_YMCA_Deceased_Ret_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'RETIREMENT'")) Then
                        l_label_YMCA_Deceased_Ret_Total.Text = 0
                    Else
                        l_label_YMCA_Deceased_Ret_Total.Text = Convert.ToDecimal(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'RETIREMENT'")).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of Total fund
                'l_label_Total_Deceased_Ret_Total = CType(e.Item.FindControl("label_Total_Deceased_Ret_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total")   'commented by SR:2010.06.17 for migration
                l_label_Total_Deceased_Ret_Total = DirectCast(e.Item.FindControl("label_Total_Deceased_Ret_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total")   
                If Not IsNothing(l_label_Total_Deceased_Ret_Total) Then
                    l_label_Total_Deceased_Ret_Total.Text = (Convert.ToDecimal(l_label_YMCA_Deceased_Ret_Total.Text) + Convert.ToDecimal(l_label_NonTaxable_Deceased_Ret_Total.Text) + Convert.ToDecimal(l_label_Taxable_Deceased_Ret_Total.Text)).ToString("###,###,##0.00")
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub DatagridBeneficiary_RetirementPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridBeneficiary_RetirementPlan.ItemDataBound
        Dim l_label_Taxable_Beneficiary_Ret_Total As Label
        Dim l_label_NonTaxable_Beneficiary_Ret_Total As Label
        Dim l_label_YMCA_Beneficiary_Ret_Total As Label
        Dim l_label_Total_Beneficiary_Ret_Total As Label

        Dim l_label_Beneficiary_Ret_Total As Label
        Dim l_label_taxable As Label
        Dim l_label_NonTaxable As Label
        Dim l_label_YMCA As Label
        Try

            If (e.Item.ItemIndex <> -1) Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                    'l_label_Beneficiary_Ret_Total = CType(e.Item.FindControl("label_Beneficiary_Ret_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total")  'commented by SR:2010.06.17 for migration
                    l_label_Beneficiary_Ret_Total = DirectCast(e.Item.FindControl("label_Beneficiary_Ret_Total"), Label)

                    If Not IsNothing(l_label_Beneficiary_Ret_Total) Then
                        'l_label_taxable = CType(e.Item.FindControl("label_taxable"), Label) 'commented by SR:2010.06.17 for migration
                        l_label_taxable = DirectCast(e.Item.FindControl("label_taxable"), Label)
                        If Not IsNothing(l_label_taxable) Then
                            l_label_taxable.Text = Decimal.Parse(l_label_taxable.Text).ToString("###,###,##0.00")
                            l_label_Beneficiary_Ret_Total.Text = (Convert.ToDecimal(l_label_taxable.Text)).ToString("###,###,##0.00")

                            l_label_NonTaxable = e.Item.FindControl("label_NonTaxable")
                            If Not IsNothing(l_label_NonTaxable) Then
                                l_label_NonTaxable.Text = Decimal.Parse(l_label_NonTaxable.Text).ToString("###,###,##0.00")
                                l_label_Beneficiary_Ret_Total.Text = (Convert.ToDecimal(l_label_Beneficiary_Ret_Total.Text) + Convert.ToDecimal(l_label_NonTaxable.Text)).ToString("###,###,##0.00")
                            End If

                            l_label_YMCA = e.Item.FindControl("label_YMCA")
                            If Not IsNothing(l_label_YMCA) Then
                                l_label_YMCA.Text = Decimal.Parse(l_label_YMCA.Text).ToString("###,###,##0.00")
                                l_label_Beneficiary_Ret_Total.Text = (Convert.ToDecimal(l_label_Beneficiary_Ret_Total.Text) + Convert.ToDecimal(l_label_YMCA.Text)).ToString("###,###,##0.00")
                            End If

                        End If
                    End If
                End If
            End If
            If e.Item.ItemType = ListItemType.Footer Then


                Dim decimal_Deceased_Ret_TaxableTotal As Decimal
                'decimal_Deceased_Ret_TaxableTotal = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal)  'commented by SR:2010.06.17 for migration
                decimal_Deceased_Ret_TaxableTotal = DirectCast(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal)

                Dim decimal_Deceased_Ret_NonTaxableTotal As Decimal
                'decimal_Deceased_Ret_NonTaxableTotal = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal) 'commented by SR:2010.06.17 for migration
                decimal_Deceased_Ret_NonTaxableTotal = DirectCast(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal)

                Dim decimal_Deceased_Ret_YMCATotal As Decimal
                'decimal_Deceased_Ret_YMCATotal = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal) 'commented by SR:2010.06.17 for migration
                decimal_Deceased_Ret_YMCATotal = DirectCast(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal)

                Dim decimal_Deceased_Ret_Total As Decimal
                ' decimal_Deceased_Ret_Total = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal) 'commented by SR:2010.06.17 for migration
                decimal_Deceased_Ret_Total = DirectCast(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'RETIREMENT' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal)


                'Assiging total of Taxable fund 
                ' l_label_Taxable_Beneficiary_Ret_Total = CType(e.Item.FindControl("label_Taxable_Beneficiary_Ret_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_Taxable_Beneficiary_Ret_Total = DirectCast(e.Item.FindControl("label_Taxable_Beneficiary_Ret_Total"), Label)

                If Not IsNothing(l_label_Taxable_Beneficiary_Ret_Total) Then
                    l_label_Taxable_Beneficiary_Ret_Total.Text = decimal_Deceased_Ret_TaxableTotal.ToString("###,###,##0.00")
                End If

                'Assiging total of NonTaxable fund
                'l_label_NonTaxable_Beneficiary_Ret_Total = CType(e.Item.FindControl("label_NonTaxable_Beneficiary_Ret_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_NonTaxable_Beneficiary_Ret_Total = DirectCast(e.Item.FindControl("label_NonTaxable_Beneficiary_Ret_Total"), Label)
                If Not IsNothing(l_label_NonTaxable_Beneficiary_Ret_Total) Then
                    l_label_NonTaxable_Beneficiary_Ret_Total.Text = decimal_Deceased_Ret_NonTaxableTotal.ToString("###,###,##0.00")

                End If

                'Assiging total of YMCA fund
                'l_label_YMCA_Beneficiary_Ret_Total = CType(e.Item.FindControl("label_YMCA_Beneficiary_Ret_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_YMCA_Beneficiary_Ret_Total = DirectCast(e.Item.FindControl("label_YMCA_Beneficiary_Ret_Total"), Label)

                If Not IsNothing(l_label_YMCA_Beneficiary_Ret_Total) Then

                    l_label_YMCA_Beneficiary_Ret_Total.Text = decimal_Deceased_Ret_YMCATotal.ToString("###,###,##0.00")
                End If

                'Assiging total of Total fund
                'l_label_Total_Beneficiary_Ret_Total = CType(e.Item.FindControl("label_Total_Beneficiary_Ret_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_Total_Beneficiary_Ret_Total = DirectCast(e.Item.FindControl("label_Total_Beneficiary_Ret_Total"), Label)
                If Not IsNothing(l_label_Total_Beneficiary_Ret_Total) Then
                    l_label_Total_Beneficiary_Ret_Total.Text = (Convert.ToDecimal(l_label_Taxable_Beneficiary_Ret_Total.Text) + Convert.ToDecimal(l_label_NonTaxable_Beneficiary_Ret_Total.Text) + Convert.ToDecimal(l_label_YMCA_Beneficiary_Ret_Total.Text)).ToString("###,###,##0.00")
                    'Convert.ToString(Math.Round(decimal_Deceased_Ret_TaxableTotal + decimal_Deceased_Ret_NonTaxableTotal + decimal_Deceased_Ret_YMCATotal, 2))
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridActiveBenefitOptions_SavingsPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBenefitOptions_SavingsPlan.ItemDataBound
        Dim l_label_Taxable_Deceased_Saving_Total As Label
        Dim l_label_NonTaxable_Deceased_Saving_Total As Label
        Dim l_label_YMCA_Deceased_Saving_Total As Label
        Dim l_label_Total_Deceased_Saving_Total As Label

        Dim l_label_Deceased_Saving_Total As Label
        Dim l_label_taxable As Label
        Dim l_label_NonTaxable As Label
        Dim l_label_YMCA As Label
        Try

            If (e.Item.ItemIndex <> -1) Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    'l_label_Deceased_Saving_Total = CType(e.Item.FindControl("label_Deceased_Saving_Total"), Label)  'commented by SR:2010.06.17 for migration
                    l_label_Deceased_Saving_Total = DirectCast(e.Item.FindControl("label_Deceased_Saving_Total"), Label)
                    If Not IsNothing(l_label_Deceased_Saving_Total) Then
                        'l_label_taxable = CType(e.Item.FindControl("label_Saving_taxable"), Label) 'commented by SR:2010.06.17 for migration
                        l_label_taxable = DirectCast(e.Item.FindControl("label_Saving_taxable"), Label)
                        If Not IsNothing(l_label_taxable) Then
                            l_label_taxable.Text = Decimal.Parse(l_label_taxable.Text).ToString("###,###,##0.00")
                            l_label_Deceased_Saving_Total.Text = (Convert.ToDecimal(l_label_taxable.Text)).ToString("###,###,##0.00")
                        End If

                        l_label_NonTaxable = e.Item.FindControl("label_NonTaxable")
                        If Not IsNothing(l_label_NonTaxable) Then
                            l_label_NonTaxable.Text = Decimal.Parse(l_label_NonTaxable.Text).ToString("###,###,##0.00")
                            l_label_Deceased_Saving_Total.Text = (Convert.ToDecimal(l_label_Deceased_Saving_Total.Text) + Convert.ToDecimal(l_label_NonTaxable.Text)).ToString("###,###,##0.00")
                        End If

                        l_label_YMCA = e.Item.FindControl("label_YMCA")
                        If Not IsNothing(l_label_YMCA) Then
                            l_label_YMCA.Text = Decimal.Parse(l_label_YMCA.Text).ToString("###,###,##0.00")
                            l_label_Deceased_Saving_Total.Text = (Convert.ToDecimal(l_label_Deceased_Saving_Total.Text) + Convert.ToDecimal(l_label_YMCA.Text)).ToString("###,###,##0.00")
                        End If
                    End If
                End If
            End If

            If e.Item.ItemType = ListItemType.Footer Then

                'Assiging total of Taxable fund
                'l_label_Taxable_Deceased_Saving_Total = CType(e.Item.FindControl("label_Taxable_Deceased_Saving_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_Taxable_Deceased_Saving_Total = DirectCast(e.Item.FindControl("label_Taxable_Deceased_Saving_Total"), Label)
                If Not IsNothing(l_label_Taxable_Deceased_Saving_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'SAVINGS'")) Then
                        l_label_Taxable_Deceased_Saving_Total.Text = 0
                    Else
                        l_label_Taxable_Deceased_Saving_Total.Text = CType(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'SAVINGS'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of NonTaxable fund
                'l_label_NonTaxable_Deceased_Saving_Total = CType(e.Item.FindControl("label_NonTaxable_Deceased_Saving_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_NonTaxable_Deceased_Saving_Total = DirectCast(e.Item.FindControl("label_NonTaxable_Deceased_Saving_Total"), Label)
                If Not IsNothing(l_label_NonTaxable_Deceased_Saving_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'SAVINGS'")) Then
                        l_label_NonTaxable_Deceased_Saving_Total.Text = 0
                    Else
                        l_label_NonTaxable_Deceased_Saving_Total.Text = CType(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'SAVINGS'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of YMCA fund
                'l_label_YMCA_Deceased_Saving_Total = CType(e.Item.FindControl("label_YMCA_Deceased_Saving_Total"), Label)  'commented by SR:2010.06.17 for migration
                l_label_YMCA_Deceased_Saving_Total = DirectCast(e.Item.FindControl("label_YMCA_Deceased_Saving_Total"), Label)
                If Not IsNothing(l_label_YMCA_Deceased_Saving_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'SAVINGS'")) Then
                        l_label_YMCA_Deceased_Saving_Total.Text = 0
                    Else
                        l_label_YMCA_Deceased_Saving_Total.Text = CType(l_dataset_DeceasedInformation.Tables(0).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'SAVINGS'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of Total fund
                'l_label_Total_Deceased_Saving_Total = CType(e.Item.FindControl("label_Total_Deceased_Saving_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total") 'commented by SR:2010.06.17 for migration
                l_label_Total_Deceased_Saving_Total = CType(e.Item.FindControl("label_Total_Deceased_Saving_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total")
                If Not IsNothing(l_label_Total_Deceased_Saving_Total) Then
                    l_label_Total_Deceased_Saving_Total.Text = (Convert.ToDecimal(l_label_YMCA_Deceased_Saving_Total.Text) + Convert.ToDecimal(l_label_NonTaxable_Deceased_Saving_Total.Text) + Convert.ToDecimal(l_label_Taxable_Deceased_Saving_Total.Text)).ToString("###,###,##0.00")
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DatagridBeneficiary_SavingPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridBeneficiary_SavingPlan.ItemDataBound
        Dim l_label_Saving_Beneficiary_Total As Label
        Dim l_label_Saving_Beneficiary_taxable As Label
        Dim l_label_Saving_Beneficiary_NonTaxable As Label
        Dim l_label_Saving_Beneficiary_YMCA As Label

        'For footer
        Dim l_label_Taxable_Beneficiary_Saving_Total As Label
        Dim l_label_NonTaxable_Beneficiary_Saving_Total As Label
        Dim l_label_YMCA_Beneficiary_Saving_Total As Label
        Dim l_label_Total_Beneficiary_Saving_Total As Label

        Try
            If (e.Item.ItemIndex <> -1) Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    'l_label_Saving_Beneficiary_Total = CType(e.I tem.FindControl("label_Saving_Beneficiary_Total"), Label) 'commented by SR:2010.06.17 for migration
                    l_label_Saving_Beneficiary_Total = DirectCast(e.Item.FindControl("label_Saving_Beneficiary_Total"), Label)
                    If Not IsNothing(l_label_Saving_Beneficiary_Total) Then
                        'l_label_Saving_Beneficiary_taxable = CType(e.Item.FindControl("label_Saving_Beneficiary_taxable"), Label)  'commented by SR:2010.06.17 for migration
                        l_label_Saving_Beneficiary_taxable = DirectCast(e.Item.FindControl("label_Saving_Beneficiary_taxable"), Label)
                        If Not IsNothing(l_label_Saving_Beneficiary_taxable) Then
                            l_label_Saving_Beneficiary_taxable.Text = Decimal.Parse(l_label_Saving_Beneficiary_taxable.Text).ToString("###,###,##0.00")
                            l_label_Saving_Beneficiary_Total.Text = (Convert.ToDecimal(l_label_Saving_Beneficiary_taxable.Text)).ToString("###,###,##0.00")

                        End If

                        l_label_Saving_Beneficiary_NonTaxable = e.Item.FindControl("label_Saving_Beneficiary_NonTaxable")
                        If Not IsNothing(l_label_Saving_Beneficiary_NonTaxable) Then

                            l_label_Saving_Beneficiary_NonTaxable.Text = Decimal.Parse(l_label_Saving_Beneficiary_NonTaxable.Text).ToString("###,###,##0.00")
                            l_label_Saving_Beneficiary_Total.Text = (Convert.ToDecimal(l_label_Saving_Beneficiary_Total.Text) + Convert.ToDecimal(l_label_Saving_Beneficiary_NonTaxable.Text)).ToString("###,###,##0.00")
                        End If

                        l_label_Saving_Beneficiary_YMCA = e.Item.FindControl("label_Saving_Beneficiary_YMCA")
                        If Not IsNothing(l_label_Saving_Beneficiary_YMCA) Then
                            l_label_Saving_Beneficiary_YMCA.Text = Decimal.Parse(l_label_Saving_Beneficiary_YMCA.Text).ToString("###,###,##0.00")
                            l_label_Saving_Beneficiary_Total.Text = (Convert.ToDecimal(l_label_Saving_Beneficiary_Total.Text) + Convert.ToDecimal(l_label_Saving_Beneficiary_YMCA.Text)).ToString("###,###,##0.00")
                        End If

                    End If
                End If
            End If

            If e.Item.ItemType = ListItemType.Footer Then

                'Assiging total of Taxable fund
                'l_label_Taxable_Beneficiary_Saving_Total = CType(e.Item.FindControl("label_Taxable_Beneficiary_Saving_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_Taxable_Beneficiary_Saving_Total = DirectCast(e.Item.FindControl("label_Taxable_Beneficiary_Saving_Total"), Label)
                If Not IsNothing(l_label_Taxable_Beneficiary_Saving_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'SAVINGS' AND UniqueID = '" & stringBeneficiaryId & "'")) Then
                        l_label_Taxable_Beneficiary_Saving_Total.Text = 0
                    Else
                        l_label_Taxable_Beneficiary_Saving_Total.Text = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPreTax)", "PlanType = 'SAVINGS' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of NonTaxable fund
                'l_label_NonTaxable_Beneficiary_Saving_Total = CType(e.Item.FindControl("label_NonTaxable_Beneficiary_Saving_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_NonTaxable_Beneficiary_Saving_Total = DirectCast(e.Item.FindControl("label_NonTaxable_Beneficiary_Saving_Total"), Label)
                If Not IsNothing(l_label_NonTaxable_Beneficiary_Saving_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'SAVINGS' AND UniqueID = '" & stringBeneficiaryId & "'")) Then
                        l_label_NonTaxable_Beneficiary_Saving_Total.Text = 0
                    Else
                        l_label_NonTaxable_Beneficiary_Saving_Total.Text = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyPersonalPostTax)", "PlanType = 'SAVINGS' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of YMCA fund
                'l_label_YMCA_Beneficiary_Saving_Total = CType(e.Item.FindControl("label_YMCA_Beneficiary_Saving_Total"), Label) 'commented by SR:2010.06.17 for migration
                l_label_YMCA_Beneficiary_Saving_Total = DirectCast(e.Item.FindControl("label_YMCA_Beneficiary_Saving_Total"), Label)
                If Not IsNothing(l_label_YMCA_Beneficiary_Saving_Total) Then
                    If IsDBNull(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'SAVINGS' AND UniqueID = '" & stringBeneficiaryId & "'")) Then
                        l_label_YMCA_Beneficiary_Saving_Total.Text = 0
                    Else
                        l_label_YMCA_Beneficiary_Saving_Total.Text = CType(l_dataset_DeceasedInformation.Tables(1).Compute("SUM(mnyYmcaPreTax)", "PlanType = 'SAVINGS' AND UniqueID = '" & stringBeneficiaryId & "'"), Decimal).ToString("###,###,##0.00")
                    End If
                End If

                'Assiging total of Total fund
                'l_label_Total_Beneficiary_Saving_Total = CType(e.Item.FindControl("label_Total_Beneficiary_Saving_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total") 'commented by SR:2010.06.17 for migration
                l_label_Total_Beneficiary_Saving_Total = DirectCast(e.Item.FindControl("label_Total_Beneficiary_Saving_Total"), Label) 'e.Item.FindControl("label_Beneficiary_Ret_Total")
                If Not IsNothing(l_label_Total_Beneficiary_Saving_Total) Then
                    l_label_Total_Beneficiary_Saving_Total.Text = (Convert.ToDecimal(l_label_YMCA_Beneficiary_Saving_Total.Text) + Convert.ToDecimal(l_label_NonTaxable_Beneficiary_Saving_Total.Text) + Convert.ToDecimal(l_label_Taxable_Beneficiary_Saving_Total.Text)).ToString("###,###,##0.00")
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFirstName.Text = ""
            Me.TextBoxLastName.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.TextBoxFundNo.Text = ""  'NP:PS:2007.09.27 - Clearing Fund Number box.
            BindGrid(DataGridSearchResults, CType(Nothing, DataSet))
            Session("BAC_PersID") = Nothing 'SR:2013.08.05 - YRS 5.0-2070 : Clean up session

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Session("BAC_PersID") = Nothing   'SR:2013.08.05 - YRS 5.0-2070 : Clean up session
            'Session("DeathCalc_Sort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub DataGridSearchResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridSearchResults.ItemDataBound
        e.Item.Cells(6).Visible = False  'Shashi Shekhar:2010-02-17 :Hide IsArchived Field in grid
        e.Item.Cells(8).Visible = False 'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
    End Sub
End Class
