'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ReissueRefundForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:12:31 PM
' Program Specification Name	:	YMCA PS 3.7.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Hafiz 03Feb06 Cache-Session
'*******************************************************************************
'Changed By:        On:                  IssueId            Issue Description

'Hafiz              03Feb06             Cache-Session       Cache-Session
'Shubhrata          Dec 6th 2006        YREN 2948           All the data from orginal check is not appearing on the reissued check
'Aparna             06-Feb 2007         YREN-3046           it is not deducting "Deductions" for a particular case
'Aparna             05-Feb 2007         YREN-3048           Page did not do a refresh when a transaction completes
'Aparna             05-Feb 2007         YREN-3047           Tax Rate change not effecting in final net Amount.      
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 03Feb06 Cache-Session
'Modified by   
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Ashutosh Patil     09-Apr-2007     YREN-30278,YREN-3029
'Aparna Samala      01-Aug-2007     YREN-3591
'Anita              2008.01.07      BT-345
'Anil Gupta         2008.01.08      YRPS-4449 Making Taxrate TextBoxe Editable
'Ashish Srivastava  29-July-2008    solved issue YRS 5.0-480  
'NP/PP/SR       	2009.05.18      Removing unused button declarations and events
'Priya              2009-10-09      Changes made for PhaseV_Part3
'Priya              2009-10-23      Changes mage for BT-994, 995,983,997
'Priya              2009-10-30      Changes made for BT-997
'Nikunj Patel       2009.10.31      System was giving DBNull to decimal cast when no rollover over No was selected
'Priya              2009-11-11      BT-1016
'Priya              16-Nov-2009     BT-1017: Problem in Withdrawal - Re-issue process.
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya              31-Dec-2009     BT-1078 : Handling of existing decuctions in Withdrawal Reissue
'Priya              13-Jan-2010     YRS 5.0-992 : remove payee3  part from coding
'Priya              05-Feb-2010     YRS 5.0-992 : change if condition from l_DisbursementDataTable.Rows.Count = 3 to l_DisbursementDataTable.Rows.Count = 2
'Priya              10-Feb-2010     BT-1113 : Deductions not deducting properly from Payee1 and Payee2
'Imran              24-Feb-2010     'Added by imran for Gemini -YRS 5.0-1023 DisbursementID null exception throw 
'Priya              08-April-2010   YRS 5.0-1046: It is creating negative check amount.
'Priya              11/30/2010      BT-592,YRS 5.0-1171 : Disbursement Ref Id not filled in 
'Shashi Shekhar     03-Jan-2010     For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Priya				10-Feb-2011 		BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
'Priya				22-Feb-2011		BT-768:  Error page showing while doing withdrawal re-issue.
'Shashi             24 Feb 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Imran              20-June-2011    BT:863  In Withdrawal-Reissue screen, set decimal size to two
'Imran              21-June-2011    BT:862  Withholding amount not deducting while Withdrawal-Reissue
'Imran              21-June-2011    BT:864  Withdrawal-Reissue inserting 20% withholding for MRD amount   
'Imran              28-June-2011    BT:866- In Withdrawal-Reissue screen, Payee1 details shows wrong.
'Imran              29-June-2011    BT:865- 'BT:865-Could not Withdrawal-Reissue the MRD disbursement 
'Imran              2011.July.04    BT:884-MRD Amount Additional Withholding issue.
'Imran              2011.July.04    BT:884-MRD Amount Additional Withholding issue.
'Imran              2011-July-21    BT:862-Withholding amount not deducting while Withdrawal-Reissue with MRD 
'Imran              2011-July-25    BT:865-Handle existing Withholding amount deducting while Withdrawal-Reissue with MRD 
'Bhavna             2011.10.20      BT:944-YRS 5.0-1451 - Modify to account for non-taxable $$ in rollover
'Bhavna             2011.10.31      BT:944-YRS 5.0-1451 - Reopen issue 
'Bhavna             2011.11.08      BT:944-YRS 5.0-1451 - Reopen issue 
'Bhavna             2011.11.24      BT:944-Add NonTaxable into MRD 
'Bhavna             2012.01.27      BT:988-YRS 5.0-1529 - Process does not work properly if name has an embedded apostrophe
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Sanjay S.          2015.09.30      YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only 
'Bala               2016.01.19      YRS-AT-2398: Customer Service requires a Special Handling alert for officers.
'Bala               2016.03.16      YRS-AT-2398  Customer Service requires a Special Handling alert for officers.
'Sanjay GS Rawat    2016.04.28      YRS-AT-2142 - Error in void reissue when tax rate is changed. 
'Manthan Rajguru    2016.06.16      YRS-AT-2962 -  YRS enh: Configurable Withdrawals project (YRS)
'********************************************************************************************************************************
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports System.Globalization
Imports System.Web.UI.HtmlControls.HtmlGenericControl

Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCARET.YmcaBusinessObject 'Manthan | 2016.06.16 | YRS-AT-2962 | Namespace used to call BO class methods
Public Class ReissueRefundForm
	Inherits System.Web.UI.Page
	'below line is Added by Neeraj for issue id YRS 5.0-940 
	Dim strFormName As String = New String("ReissueRefundForm.aspx")
	'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtZip As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelAddress As System.Web.UI.WebControls.Label
	Protected WithEvents LabelRollover As System.Web.UI.WebControls.Label
	Protected WithEvents LabelWithholding As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayee1 As System.Web.UI.WebControls.TextBox
    'SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only 
    Protected WithEvents TextboxPayee2 As RolloverInstitution 'Payee2  Textbox should have intellisense function. Hence, TextboxPayee2 replaced with RolloverInstitution Textbox
    'Protected WithEvents TextBoxPayee2 As System.Web.UI.WebControls.TextBox
    'SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only 
	Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelTaxable As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxTaxable2 As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelNonTaxable As System.Web.UI.WebControls.Label
	Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelTaxRate As System.Web.UI.WebControls.Label
	Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelTax As System.Web.UI.WebControls.Label
	Protected WithEvents DataGridDeductions As System.Web.UI.WebControls.DataGrid
	Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
	Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
	Protected WithEvents LabelYesNo As System.Web.UI.WebControls.Label
	Protected WithEvents LabelPayee1 As System.Web.UI.WebControls.Label
	Protected WithEvents LabelPayee2 As System.Web.UI.WebControls.Label
	Protected WithEvents LabelState As System.Web.UI.WebControls.Label
	Protected WithEvents CheckBoxRolloverYes As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CheckBoxRolloverNo As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CheckBoxWithholdingYes As System.Web.UI.WebControls.CheckBox
	Protected WithEvents CheckBoxWithholdingNo As System.Web.UI.WebControls.CheckBox
	Protected WithEvents Menu1 As skmMenu.Menu
	Protected WithEvents LabelDeduction As System.Web.UI.WebControls.Label
	Protected WithEvents CheckBoxDeduction As System.Web.UI.WebControls.CheckBox
	Protected WithEvents chkExistingDeductions As System.Web.UI.WebControls.CheckBox
	Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents labelZip As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
	Protected WithEvents lblState As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxCountry As System.Web.UI.WebControls.TextBox
	Protected WithEvents labelCountry As System.Web.UI.WebControls.Label
	Protected WithEvents Label2 As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxPin As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelDeductions As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxNet2 As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
	Protected WithEvents DataGridReissue As System.Web.UI.WebControls.DataGrid
	Protected WithEvents dgExistingDeductions As System.Web.UI.WebControls.DataGrid
	Protected WithEvents TabStripWithdrawalReissue As Microsoft.Web.UI.WebControls.TabStrip
	Protected WithEvents MultiPageVRManager As Microsoft.Web.UI.WebControls.MultiPage
	Protected WithEvents buttonDetailsOK As System.Web.UI.WebControls.Button
	Protected WithEvents hypExistingDeductions As System.Web.UI.WebControls.HyperLink
	Protected WithEvents TextBoxPayeeAmt As System.Web.UI.WebControls.TextBox
	Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
	Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
	'Mrd
	Protected WithEvents TextBoxMRDTaxable As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxMRDNonTaxable As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxMRDTax As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxMRDNet As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxMRDDeductions As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxMRDRate As System.Web.UI.WebControls.TextBox
	'payee1
	Protected WithEvents TextBoxTaxable As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxNonTaxable As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxTax As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxNet As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxDeductions As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxRate As System.Web.UI.WebControls.TextBox
	'payee2
	Protected WithEvents TextBoxTaxable1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxNonTaxable1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxTax1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxNet1 As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxDeductions1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxRate1 As System.Web.UI.WebControls.TextBox
    'Start: Bala: 01/19/2019: YRS-AT-2398: Control checkbox and hiddenfield is added 
    Protected WithEvents HiddenFieldOfficersDetails As System.Web.UI.WebControls.HiddenField
    Protected WithEvents LabelSpecialHandling As System.Web.UI.WebControls.Label
    'End: Bala: 01/19/2019: YRS-AT-2398: Control checkbox and hiddenfield is added

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

#Region " Form Properties "
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

    ' To get / set DisbursementID
    Private Property DisbursementID() As String
        Get
            If Not Session("DisbursementID") Is Nothing Then
                Return (DirectCast(Session("DisbursementID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("DisbursementID") = Value
        End Set
    End Property


    'by Aparna YREN-3591 -SET OR GET TRANSACTID 01/08/07
    Private Property TransactID() As String
        Get
            If Not Session("TransactID") Is Nothing Then
                Return (DirectCast(Session("TransactID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("TransactID") = Value
        End Set
    End Property
    ' To get / set Payee2ID
    Private Property Payee2ID() As String
        Get
            If Not Session("Payee2ID") Is Nothing Then
                Return (DirectCast(Session("Payee2ID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee2ID") = Value
        End Set
    End Property
    Private Property Payee3ID() As String
        Get
            If Not Session("Payee3ID") Is Nothing Then
                Return (DirectCast(Session("Payee3ID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee3ID") = Value
        End Set
    End Property


    ' To Get / Set Is vested flag
    Private Property IsSuccessfull() As Boolean
        Get
            If Not Session("IsSuccessfull") Is Nothing Then
                Return (DirectCast(Session("IsSuccessfull"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsSuccessfull") = Value
        End Set
    End Property


    ' To get / set Deductions amount.
    Private Property DeductionsAmount() As Decimal
        Get
            If Not Session("DeductionsAmount") Is Nothing Then
                Return (DirectCast(Session("DeductionsAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("DeductionsAmount") = Value
        End Set
    End Property


    ' To get / set TaxRate amount.
    Private Property TaxRate() As Decimal
        Get
            If Not Session("TaxRate") Is Nothing Then
                Return (DirectCast(Session("TaxRate"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxRate") = Value
        End Set
    End Property

    Private Property AddressID() As Integer
        Get
            If Not Session("AddressID") Is Nothing Then
                Return (DirectCast(Session("AddressID"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("AddressID") = Value
        End Set
    End Property

    ' To get / set NetAmount amount.
    Private Property NetAmount() As Decimal
        Get
            If Not Session("NetAmount") Is Nothing Then
                Return (DirectCast(Session("NetAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NetAmount") = Value
        End Set
    End Property

    ' To get / set NetAmount amount.
    Private Property TaxAmount() As Decimal
        Get
            If Not Session("TaxAmount") Is Nothing Then
                Return (DirectCast(Session("TaxAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxAmount") = Value
        End Set
    End Property


    ' To get / set NetAmount amount.
    Private Property Taxable() As Decimal
        Get
            If Not Session("Taxable") Is Nothing Then
                Return (DirectCast(Session("Taxable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Taxable") = Value
        End Set
    End Property

    ' To get / set NetAmount amount.
    Private Property NonTaxable() As Decimal
        Get
            If Not Session("NonTaxable") Is Nothing Then
                Return (DirectCast(Session("NonTaxable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NonTaxable") = Value
        End Set
    End Property


    ''To Keep the Currency Code.
    Private Property CurrencyCode() As String
        Get
            If Not Session("CurrencyCode") Is Nothing Then
                Return (DirectCast(Session("CurrencyCode"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("CurrencyCode") = Value
        End Set
    End Property

    ' To get / set Payee1 Addresss ID
    Private Property PayeeAddressID() As String
        Get
            If Not Session("PayeeAddressID") Is Nothing Then
                Return (DirectCast(Session("PayeeAddressID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PayeeAddressID") = Value
        End Set
    End Property
    'anita bug tracker 345
    Private Property DisbursementType() As String
        Get
            If Not Session("DisbursementType") Is Nothing Then
                Return (DirectCast(Session("DisbursementType"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("DisbursementType") = Value
        End Set
    End Property
    'anita bug tracker 345
    'Priya 31-Aug-09
    Private Property Payee2Taxable() As Decimal
        Get
            If Not Session("Payee2Taxable") Is Nothing Then
                Return (DirectCast(Session("Payee2Taxable"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Payee2Taxable") = Value
        End Set
    End Property

    Private Property Payee2NonTaxable() As Decimal
        Get
            If Not Session("Payee2NonTaxable") Is Nothing Then
                Return (DirectCast(Session("Payee2NonTaxable"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Payee2NonTaxable") = Value
        End Set
    End Property
    Private Property Payee3Taxable() As Decimal
        Get
            If Not Session("Payee3Taxable") Is Nothing Then
                Return (DirectCast(Session("Payee3Taxable"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Payee3Taxable") = Value
        End Set
    End Property

    Private Property Payee3NonTaxable() As Decimal
        Get
            If Not Session("Payee3NonTaxable") Is Nothing Then
                Return (DirectCast(Session("Payee3NonTaxable"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Payee3NonTaxable") = Value
        End Set
    End Property
    'End 31-Aug-09
    '07-Sep-09
    Private Property RefRequestID() As String
        Get
            If Not Session("RequestID") Is Nothing Then
                Return (DirectCast(Session("RequestID"), String))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            Session("RequestID") = Value
        End Set
    End Property

    Private Property AllowRollOver() As String
        Get
            If Not ViewState("AllowRollOver") Is Nothing Then
                Return (DirectCast(ViewState("AllowRollOver"), String))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("AllowRollOver") = Value
        End Set
    End Property

    Private Property Payee1Taxable() As Decimal
        Get
            If Not Session("Payee1Taxable") Is Nothing Then
                Return (DirectCast(Session("Payee1Taxable"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Payee1Taxable") = Value
        End Set
    End Property

    Private Property Payee1NonTaxable() As Decimal
        Get
            If Not Session("Payee1NonTaxable") Is Nothing Then
                Return (DirectCast(Session("Payee1NonTaxable"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("Payee1NonTaxable") = Value
        End Set
    End Property

    Dim ShowMessage As Boolean

    '   Get
    'If Not ViewSate("ShowMessage") Is Nothing Then
    'Return (CType(Session("ShowMessage"), String))
    'Else
    '   Return False
    'End If
    'End Get
    'Set(ByVal Value As Boolean)
    '   Session("ShowMessage") = Value
    'End Set


    'End Property


    'End 07-Sep-09


#End Region
    'BS:2012.01.27:-Moving this  SanitizeValue() to SanitizeValueForJS() in HelperFunctions Class 
    'Private Function SanitizeValue(ByVal s As String) As String
    '   Return s.Replace("'", "\'")
    'End Function
    'BS:2011.10.21-YRS 5.0-1451- Create Common Method of javascript array for payee1,payee2,Mrd datatable
    Private Function CreateJavascriptArray(ByVal dt As DataTable) As String
        Dim javaScript As New System.Text.StringBuilder()
        javaScript.Append("[" & vbCrLf)
        Dim i, j As Integer
        Dim dr As DataRow
        If dt Is Nothing Then Return "[ ];"
        For i = 0 To dt.Rows.Count - 1
            dr = dt.Rows(i)
            javaScript.Append(" [ ")
            For j = 0 To dr.Table.Columns.Count - 1
                'javaScript.Append("'" + SanitizeValue(dr(j).ToString()) + "'")
                'BS:2012.01.27:-YRS 5.0-1529 - Process does not work properly if name has an embedded apostrophe 
                javaScript.Append("'" + HelperFunctions.SanitizeValueForJS(dr(j).ToString()) + "'")
                If ((j + 1) < dr.Table.Columns.Count) Then
                    javaScript.Append(",")
                End If
            Next
            javaScript.Append(" ]")
            If ((i + 1) < dt.Rows.Count) Then
                javaScript.Append("," & vbCrLf)
            End If
        Next
        javaScript.Append(vbCrLf & "];" & vbCrLf)
        Return javaScript.ToString()

    End Function
    'BS:2011.10.21-YRS 5.0-1451- Create javascript array for payee1,payee2,Mrd datatable,DisbursementType,AllowRolloverNo
    Private Sub CreatePayee1Payee2Mrd()
        Dim dtPayee1 As DataTable
        Dim dtPayee2 As DataTable
        Dim dtMrd As DataTable
        dtPayee1 = CopyTable(DirectCast(Session("Payee1DataTable"), DataTable), "Payee1DataTable")
        dtPayee2 = DirectCast(Session("Payee2DataTable"), DataTable)
        dtMrd = CopyTable(DirectCast(Session("R_MRDAcctBalance"), DataTable), "R_MRDAcctBalance")

        If Not dtPayee1 Is Nothing Then
            If dtPayee2 Is Nothing Then
                dtPayee2 = dtPayee1.Clone
            End If
            dtPayee2.Rows.Clear()
            For Each l_DataRow As DataRow In dtPayee1.Rows
                dtPayee2.ImportRow(l_DataRow)
            Next
            '' Just clear the Tax & Taxrate in the Payee 2 datatable.
            For Each l_DataRow As DataRow In dtPayee2.Rows
                l_DataRow("TaxRate") = "0.00"
                l_DataRow("Tax") = "0.00"
                l_DataRow("Taxable") = "0.0"
                l_DataRow("NonTaxable") = "0.0"
            Next
            dtPayee2.AcceptChanges()
        End If
        If dtPayee1 Is Nothing Then Return
        Dim javaScript As New System.Text.StringBuilder()
        'Code to Write javascript array  for Datatable
        'create javascript array for payee1

        javaScript.Append("var Payee1DataTable = ")
        javaScript.Append(CreateJavascriptArray(dtPayee1))
        'Payee2 to javascript
        javaScript.Append("var Payee2DataTable = " & vbCrLf)
        javaScript.Append(CreateJavascriptArray(dtPayee2))
        'MRD to javascript
        javaScript.Append("var MrdDataTable = " & vbCrLf)
        javaScript.Append(CreateJavascriptArray(dtMrd))
        'DisbursementType,AllowRolloverNo
        javaScript.Append(String.Format("var options = {{ DisbursementType:'{0}', CanRollOverNo:{1} }};" & vbCrLf, DisbursementType, IIf(AllowRollOver = "0", 0, 1)))
        If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("ArrayScript") Then
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ArrayScript", javaScript.ToString(), True)
        End If
        javaScript.Clear()
        javaScript.Append("$(document).ready(function() { initializeControl();});")
        Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), Me.ClientID, javaScript.ToString(), True)
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            If Not IsPostBack Then
                Session("TaxRate") = Nothing 'Manthan | 2016.06.16 | YRS-AT-2962 | Clearing session value
                Me.LoadReissuesDetails()
                Me.LoadReissueFromCache()
                TabStripWithdrawalReissue.Items(1).Enabled = False
            End If

            Me.EnableDisableSaveButton()
            If Me.Request.Form("OK") = "OK" And Me.IsSuccessfull = True Then
                Me.LoadReissuesDetails()
                Me.LoadDefalutValues()
                Me.SelectFromDataGrid()
                IsSuccessfull = False
                TabStripWithdrawalReissue.Items(1).Enabled = False
                TabStripWithdrawalReissue.SelectedIndex = 0
                MultiPageVRManager.SelectedIndex = 0
            End If
            If Not IsPostBack Then
                Me.LabelAddress1.AssociatedControlID = Me.TextBoxAddress1.ID
                Me.LabelAddress2.AssociatedControlID = Me.TextBoxAddress2.ID
                Me.LabelAddress3.AssociatedControlID = Me.TextBoxAddress3.ID
                Me.LabelCity.AssociatedControlID = Me.TextBoxCity.ID
                Me.LabelPayee2.AssociatedControlID = Me.LabelPayee2.ID
                Session("ds_PrimaryAddress") = Nothing
            End If
            'to do clear..BS
            If Not Session("ds_PrimaryAddress") Is Nothing Then
                LoadFromPopUp()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Function LoadFromPopUp()
        Dim dataset_AddressInfo As DataSet
        Dim datarow_Row As DataRow
        Dim l_str_zipcode As String
        dataset_AddressInfo = (DirectCast(Session("ds_PrimaryAddress"), DataSet))
        If dataset_AddressInfo.Tables(0).Rows.Count > 0 Then
            datarow_Row = dataset_AddressInfo.Tables("AddressInfo").Rows(0)
            TextBoxAddress1.Text = datarow_Row.Item("Address1")
            TextBoxAddress2.Text = datarow_Row.Item("Address2")
            TextBoxAddress3.Text = datarow_Row.Item("Address3")
            TextBoxCity.Text = datarow_Row.Item("City")
            TextBoxState.Text = datarow_Row.Item("StateName").ToString() 'datarow_Row.Item("state").ToString()
            TextBoxCountry.Text = datarow_Row.Item("CountryName").ToString() 'datarow_Row.Item("Country").ToString()
            If Me.TextBoxCountry.Text.ToString = "CANADA" Then
                l_str_zipcode = CType(datarow_Row("Zip"), String).Trim
                Me.TextBoxPin.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
            Else
                TextBoxPin.Text = datarow_Row.Item("Zip").ToString
            End If
            Session("ds_PrimaryAddress") = Nothing
            dataset_AddressInfo = Nothing
        End If
    End Function

    Private Function SelectFromDataGrid()
        Dim l_DataTable As DataTable
        Me.DataGridReissue.SelectedIndex = -1
        Me.LoadDefalutValues()
        l_DataTable = DirectCast(Session("RefundReissueDetails"), DataTable)
        Me.DataGridReissue.DataSource = l_DataTable
        Me.DataGridReissue.DataBind()
    End Function
    'BS:2011.10.20-YRS 5.0-1451-Create Payee1 datatable After MRD deduction and create structure for Payee2 datatable
    Private Function CreatePayees()
        Dim l_CurrentDataTable As DataTable
        Dim l_Payee1DataTable, l_Payee2DataTable, l_MRDDatatable As DataTable
        Dim l_PayeeDataRow As DataRow
        '10-Feb-2011 Priya		BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
        'l_CurrentDataTable = DirectCast(Session("R_Current"), DataTable)
        'BS:2011.11.01:reopen issue YRS 5.0-1451
        l_CurrentDataTable = CopyTable(DirectCast(Session("R_Current"), DataTable), "R_Current")

        l_MRDDatatable = DirectCast(Session("R_MRDAcctBalance"), DataTable)
        If (Not l_CurrentDataTable Is Nothing) Then
            l_Payee1DataTable = l_CurrentDataTable.Clone
            l_Payee1DataTable.Columns.Add("TaxRate")
            l_Payee1DataTable.Columns.Add("Tax")
            l_Payee1DataTable.Columns.Add("Payee")
            l_Payee1DataTable.Columns.Add("FundedDate")
            For Each l_DataRow As DataRow In l_CurrentDataTable.Rows
                l_PayeeDataRow = l_Payee1DataTable.NewRow()
                l_PayeeDataRow("AcctType") = l_DataRow("AcctType")
                If (Not l_MRDDatatable Is Nothing) Then
                    For Each l_mrdDataRow As DataRow In l_MRDDatatable.Rows
                        If l_DataRow("AcctType") = l_mrdDataRow("AcctType") Then
                            'BT:866- In Withdrawal-Reissue screen, Payee1 details shows wrong.
                            If l_DataRow.IsNull("Taxable") = False AndAlso Convert.ToDecimal(l_DataRow("Taxable")) >= Convert.ToDecimal(l_mrdDataRow("Taxable")) Then
                                l_PayeeDataRow("Taxable") = l_DataRow("Taxable") - l_mrdDataRow("Taxable")
                                l_DataRow("Taxable") = l_PayeeDataRow("Taxable")
                            Else
                                l_PayeeDataRow("Taxable") = 0.0
                                l_DataRow("Taxable") = 0.0
                            End If
                            'BS:2011.11.02:YRS 5.0-1451-reopen issue rmd(mrd) nontaxable component deduct from current nontaxable component
                            If l_DataRow.IsNull("NonTaxable") = False AndAlso Convert.ToDecimal(l_DataRow("NonTaxable")) >= Convert.ToDecimal(l_mrdDataRow("NonTaxable")) Then
                                l_PayeeDataRow("NonTaxable") = l_DataRow("NonTaxable") - l_mrdDataRow("NonTaxable")
                                l_DataRow("NonTaxable") = l_PayeeDataRow("NonTaxable")
                            Else
                                l_PayeeDataRow("NonTaxable") = 0.0
                                l_DataRow("NonTaxable") = 0.0
                            End If
                        End If
                    Next
                End If
                'End 10-Feb-2011 BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
                'l_PayeeDataRow("NonTaxable") = l_DataRow("NonTaxable")
                l_PayeeDataRow("TaxRate") = Me.TaxRate
                l_PayeeDataRow("Tax") = Convert.ToDecimal(l_DataRow("Taxable")) * (Me.TaxRate / 100.0)
                l_PayeeDataRow("Payee") = Me.TextBoxPayee1.Text
                l_PayeeDataRow("FundedDate") = System.DBNull.Value
                l_Payee1DataTable.Rows.Add(l_PayeeDataRow)
            Next
            l_Payee2DataTable = l_Payee1DataTable.Clone
            Session("Payee1DataTable") = l_Payee1DataTable
            Session("Payee2DataTable") = l_Payee2DataTable
        End If
    End Function

    Private Function LoadReissueFromCache()
        Dim l_DataTable As DataTable
        l_DataTable = DirectCast(Session("RefundReissueDetails"), DataTable)
        Me.DataGridReissue.DataSource = l_DataTable
        Me.DataGridReissue.DataBind()
    End Function

    Private Function LoadReissuesDetails()
        Dim l_DataTable As DataTable
        l_DataTable = YMCARET.YmcaBusinessObject.ReissueRefund.GetRefundReissuesDetails()
        Session("RefundReissueDetails") = l_DataTable
    End Function

    Private Sub DataGridReissue_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridReissue.ItemDataBound
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")

            If Not IsNothing(l_button_Select) Then
                If (e.Item.ItemIndex = Me.DataGridReissue.SelectedIndex) Then
                    l_button_Select.ImageUrl = "images\selected.gif"
                Else
                    l_button_Select.ImageUrl = "images\select.gif"
                End If
            End If

            e.Item.Cells(1).Visible = False
            e.Item.Cells(5).Visible = False
            e.Item.Cells(7).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
            e.Item.Cells(12).Visible = False
            e.Item.Cells(13).Visible = False
            e.Item.Cells(14).Visible = False
            e.Item.Cells(15).Visible = False
            e.Item.Cells(16).Visible = False
            e.Item.Cells(17).Visible = False 'AllowRollOverNo
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridReissue_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridReissue.SelectedIndexChanged
        MultiPageVRManager.SelectedIndex = 1
        TabStripWithdrawalReissue.SelectedIndex = 1
        TabStripWithdrawalReissue.Items(1).Enabled = True
        Try
            Session("TaxRate") = Nothing 'Manthan | 2016.06.16 | YRS-AT-2962 | Clearing session value
            LoadReissueFromCache()
            LoadDefalutValues()
            'Me.LoadSchemaRefundTable()

            '' Set PersonID, FundID & DisbursementID
            Me.SetIDValues(Me.DataGridReissue.SelectedIndex)
            If AllowRollOver = 0 Then
                Me.CheckBoxRolloverNo.Checked = True
                Me.CheckBoxRolloverYes.Enabled = False
                Me.CheckBoxRolloverNo.Enabled = False
            End If
            '' Load Address Details/
            Me.LoadGeneralInformation(Me.PersonID, Me.FundID)
            '' Load Member Deduction, Current Accounts & Current Details.
            Me.LoadMemberReissueDetails()
            '' Load Deduction Grid  -- This call to get all Deduction.
            Me.LoadDeductions()
            LoadExistingDeductions()
            'Priya 4-Jan-2010
            'Called GetDedutions() function to assign selected deductions value to DeductionsAmount variable.
            GetDedutions()
            '4-Jan-2010
            '' Load member's Deduction Details.
            Me.LoadDeductionsDetails()
            '' Load Create payee Details.
            Me.CreatePayees()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" & Server.UrlDecode(ex.Message.Trim), False)
        End Try

    End Sub

    'Private Function LoadSchemaRefundTable()
    '	Dim l_DataSet As DataSet
    '	Dim l_DataTable As DataTable
    '	l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSchemaRefundTable
    '	If Not l_DataSet Is Nothing Then
    '		l_DataTable = l_DataSet.Tables("atsRefunds")
    '		Session("R_Refunds") = l_DataTable
    '		l_DataTable = l_DataSet.Tables("atsRefRequests")
    '		Session("AtsRefRequests") = l_DataTable
    '		l_DataTable = l_DataSet.Tables("atsRefRequestDetails")
    '		Session("AtsRefRequestDetails") = l_DataTable
    '	End If
    'End Function

    Private Function LoadDeductionsDetails()
        Dim l_OldDeductionDataTable As DataTable
        Dim l_NewDeductionDataTable As DataTable

        Dim l_Counter As Integer
        Dim l_CodeValue As String
        Dim l_WithHoldTypeCode As String
        Dim l_FoundRows() As DataRow
        Dim l_NewDataRow As DataRow

        Me.LabelDeduction.Visible = True
        Me.DataGridDeductions.Visible = True
        Me.dgExistingDeductions.Visible = True


        l_OldDeductionDataTable = CType(Session("Old_Deductions"), DataTable)
        l_NewDeductionDataTable = CType(Session("R_Deductions"), DataTable)

        If l_OldDeductionDataTable Is Nothing Then Return 0

        If l_OldDeductionDataTable.Rows.Count < 1 Then Return 0


        For Each l_OldDataRow As DataRow In l_OldDeductionDataTable.Rows
            l_CodeValue = IIf(l_OldDataRow("WithholdingTypeCode").GetType.ToString = "System.DBNull", String.Empty, CType(l_OldDataRow("WithholdingTypeCode"), String))
            l_FoundRows = l_NewDeductionDataTable.Select("CodeValue = '" & l_CodeValue.Trim & "'")
            If l_FoundRows.Length < 1 Then
                l_NewDataRow = l_NewDeductionDataTable.NewRow()
                l_NewDataRow("CodeValue") = l_OldDataRow("WithholdingTypeCode")
                l_NewDataRow("ShortDescription") = l_OldDataRow("WithholdingTypeCode")
                l_NewDataRow("Amount") = l_OldDataRow("Amount")
                l_NewDeductionDataTable.Rows.Add(l_NewDataRow)
            Else
                l_NewDataRow = l_FoundRows(0)
                If Not l_NewDataRow Is Nothing Then
                    l_NewDataRow("Amount") = IIf(l_NewDataRow.IsNull("Amount"), 0.0, CType(l_NewDataRow("Amount"), Decimal)) + IIf(l_OldDataRow.IsNull("Amount"), 0.0, CType(l_OldDataRow("Amount"), Decimal))
                End If
            End If
        Next
    End Function

    Private Function SetIDValues(ByVal parameterIndex As Integer)

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim dsConfigTaxRate As DataSet 'Manthan | 2016.06.16 | YRS-AT-2962 | Declaring dataset to store tax rate details

        l_DataTable = CType(Session("RefundReissueDetails"), DataTable)
        If HelperFunctions.isNonEmpty(l_DataTable) Then
            If l_DataTable.Rows.Count >= parameterIndex Then
                l_DataRow = l_DataTable.Rows.Item(parameterIndex)
                If Not l_DataRow Is Nothing Then
                    'Set the ID values
                    Me.PersonID = IIf(l_DataRow("PersonID").GetType.ToString = "System.DBNull", String.Empty, CType(l_DataRow("PersonID"), String).Trim)
                    Me.FundID = IIf(l_DataRow("FundID").GetType.ToString = "System.DBNull", String.Empty, CType(l_DataRow("FundID"), String).Trim)
                    Me.DisbursementType = IIf(l_DataRow("DisbursementType").GetType.ToString.Trim = "System.DBNull", String.Empty, CType(l_DataRow("DisbursementType"), String).Trim)
                    RefRequestID = IIf(l_DataRow("RefRequestID").GetType.ToString = "System.DBNull", String.Empty, CType(l_DataRow("RefRequestID"), String).Trim)
                    AllowRollOver = IIf(l_DataRow("AllowRollOverNo").GetType.ToString = "System.DBNull", String.Empty, CType(l_DataRow("AllowRollOverNo"), String).Trim)

                    If Me.DisbursementType.Equals("ADT") Or Me.DisbursementType.Equals("RDT") Then
                        Me.TaxRate = 0
                        Me.TextBoxRate.Text = Me.TaxRate
                    Else
                        'Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to property
                        'Me.TaxRate = 20
                        dsConfigTaxRate = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.SearchConfigurationMaintenance("REFUND_FEDERALTAXRATE")
                        If HelperFunctions.isNonEmpty(dsConfigTaxRate) Then
                            Me.TaxRate = dsConfigTaxRate.Tables("Configuration Maintenance").Rows(0)("Value")
                        Else
                            Throw New Exception("'REFUND_FEDERALTAXRATE' key not defined in atsMetaConfiguration")
                        End If
                        'End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to property
                        Me.TextBoxRate.Text = Me.TaxRate
                    End If
                    'SS:24 Feb 2011: Replacing Header formating with user control(Ref:YRS 5.0-450 )
                    Headercontrol.PageTitle = "Reissue Withdrawal"
                    Headercontrol.SSNo = IIf(l_DataRow("SSNO").GetType.ToString = "System.DBNull", String.Empty, CType(l_DataRow("SSNo"), String))

                End If
            End If
        End If
    End Function

    Private Function LoadDefalutValues()

        'Priya 31-Dec-2009 BT-1078 : Handling of existing decuctions in Withdrawal Reissue
        'Me.DeductionsAmount = 0.0
        'Called GetDedutions() function to assign selected deductions value to DeductionsAmount variable.
        'GetDedutions()
        'TextBoxDeductions.Text = DeductionsAmount
        'End 31-Dec-2009

        Me.TextBoxAddress1.ReadOnly = True
        Me.TextBoxAddress2.ReadOnly = True
        Me.TextBoxAddress3.ReadOnly = True
        Me.TextBoxCity.ReadOnly = True
        Me.TextBoxState.ReadOnly = True
        Me.TextBoxCountry.ReadOnly = True
        Me.TextBoxPin.ReadOnly = True

        Me.TextBoxAddress1.Text = String.Empty
        Me.TextBoxAddress2.Text = String.Empty
        Me.TextBoxAddress3.Text = String.Empty
        Me.TextBoxCity.Text = String.Empty
        Me.TextBoxState.Text = String.Empty
        Me.TextBoxCountry.Text = String.Empty
        Me.TextBoxPin.Text = String.Empty

        'change by ashutosh visible to enable on 8/03/2006
        'Me.ButtonAddress.Enabled = False

        Me.LabelDeduction.Visible = False
        Me.DataGridDeductions.Visible = False
        dgExistingDeductions.Visible = False
        Me.TextBoxPayee1.ReadOnly = True
        Me.TextBoxPayee1.Text = String.Empty

        'BS:2011.10.12-YRS 5.0-1451-set default value on Textboxes
        'Mrd
        TextBoxMRDDeductions.Text = "0.00"
        TextBoxMRDNet.Text = "0.00"
        TextBoxMRDNonTaxable.Text = "0.00"
        TextBoxMRDRate.Text = 10
        TextBoxMRDTaxable.Text = "0.00"
        TextBoxMRDTax.Text = "0.00"
        TextBoxMRDDeductions.Text = "0.00"
        'Payee1
        TextBoxNonTaxable.Text = "0.00"
        TextBoxTaxable.Text = "0.00"
        'BS:2011.10.14-Cleanup
        'Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to textbox
        'TextBoxRate.Text = 20
        TextBoxRate.Text = Me.TaxRate
        'End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to textbox
        TextBoxTax.Text = "0.00"
        TextBoxNet.Text = "0.00"
        TextBoxDeductions.Text = "0.00"
        'Payee2
        TextBoxNonTaxable1.Text = "0.00"
        TextBoxTaxable1.Text = "0.00"
        TextBoxRate1.Text = "0.00"
        TextBoxTax1.Text = "0.00"
        TextBoxNet1.Text = "0.00"
        TextBoxDeductions1.Text = "0.00"
        'BS:2011.10.17-YRS 5.0-1451-disabled Control
        CheckBoxRolloverYes.Checked = False
        CheckBoxWithholdingYes.Checked = False
        CheckBoxRolloverNo.Checked = False
        CheckBoxWithholdingNo.Checked = False
        CheckBoxRolloverYes.Enabled = True
        CheckBoxRolloverNo.Enabled = True
        TextBoxPayee2.Text = String.Empty
        TextBoxPayeeAmt.Text = "0.00"
        LabelSpecialHandling.Text = "" 'Bala: 01/19/2019: YRS-AT-2398: Reseting the label value to empty.
    End Function

    Private Function LoadMemberReissueDetails()
        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable

        Try
            l_DataSet = YMCARET.YmcaBusinessObject.ReissueRefund.GetMemberReissueDetails(Me.PersonID, Me.DisbursementID, Me.RefRequestID)
            l_DataTable = l_DataSet.Tables.Item("Deductions")
            Session("Old_Deductions") = l_DataTable
            l_DataTable = l_DataSet.Tables.Item("Current")
            Session("R_Current") = l_DataTable
            '10-Feb-2011 Priya		BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
            l_DataTable = l_DataSet.Tables.Item("MRDAcctBalance")
            Session("R_MRDAcctBalance") = l_DataTable
            'End 10-Feb-2011 Priya		BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function LoadGeneralInformation(ByVal parameterPersonID As String, ByVal parameterFundEventID As String)
        Dim l_DataSet As DataSet = Nothing
        Dim l_DataTable As DataTable = Nothing
        l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.LookupMemberDetails(parameterPersonID, parameterFundEventID)
        If l_DataSet Is Nothing Then Return 0
        l_DataTable = l_DataSet.Tables("Member Details")
        If HelperFunctions.isNonEmpty(l_DataTable) Then
            If Not IsNothing(l_DataTable.Rows.Item(0)("First Name")) AndAlso Not l_DataTable.Rows.Item(0)("First Name") Is System.DBNull.Value Then
                Me.TextBoxPayee1.Text = CType(l_DataTable.Rows.Item(0)("First Name"), String).Trim

            End If

            If Not IsNothing(l_DataTable.Rows.Item(0)("Middle Name")) AndAlso Not l_DataTable.Rows.Item(0)("Middle Name") Is System.DBNull.Value Then
                Me.TextBoxPayee1.Text = Me.TextBoxPayee1.Text.Trim() & " " & CType(l_DataTable.Rows.Item(0)("Middle Name"), String).Trim

            End If

            If Not IsNothing(l_DataTable.Rows.Item(0)("Last Name")) AndAlso Not l_DataTable.Rows.Item(0)("Last Name") Is System.DBNull.Value Then
                Me.TextBoxPayee1.Text = Me.TextBoxPayee1.Text.Trim & " " & CType(l_DataTable.Rows.Item(0)("Last Name"), String).Trim

            End If
        End If
        l_DataTable = l_DataSet.Tables("Member Address")
        If HelperFunctions.isNonEmpty(l_DataTable) Then
            Me.LoadAddressInfoToControls(l_DataTable.Rows.Item(0))
        End If
        Session("PersonInformation") = l_DataSet
        'Start: Bala: 01/19/2019: YRS-AT-2398: Customer Service requires a Special Handling alert for officers
        l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSpecialHandlingDetails(parameterPersonID)
        l_DataTable = l_DataSet.Tables("SpecialHandlingDetails")
        'If Not l_DataTable Is Nothing Then 'Bala: 03.15.2016: YRS-AT-2398: Commented.
        If HelperFunctions.isNonEmpty(l_DataTable) Then 'Bala: 03.16.2016: YRS-AT-2398: Check if the table has no records.
            If Convert.ToBoolean(l_DataTable.Rows(0).Item("RequireSpecialHandling").ToString) Then
                Me.LabelSpecialHandling.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWALS_SPECIAL_HANDLING).DisplayText
                Me.LabelSpecialHandling.Visible = True
                Me.HiddenFieldOfficersDetails.Value = l_DataTable.Rows(0).Item("EmploymentDetail").ToString
            End If
        End If
        'End: Bala: 01/19/2019: YRS-AT-2398: Customer Service requires a Special Handling alert for officers
    End Function

	Private Function LoadAddressInfoToControls(ByVal parameterDataRow As DataRow)
		Dim l_str_zipcode As String

		If Not parameterDataRow Is Nothing Then
			Me.PayeeAddressID = CType(parameterDataRow("AddressID"), String)
			If (parameterDataRow.IsNull("Address1") = True OrElse parameterDataRow("Address1").ToString.Trim = String.Empty) Then
				Me.TextBoxAddress1.Text = String.Empty
			Else
				Me.TextBoxAddress1.Text = CType(parameterDataRow("Address1"), String).Trim
			End If
			If (parameterDataRow.IsNull("Address2") = True OrElse parameterDataRow("Address2").ToString.Trim = String.Empty) Then
				Me.TextBoxAddress2.Text = String.Empty
			Else
				Me.TextBoxAddress2.Text = CType(parameterDataRow("Address2"), String).Trim
			End If
			If (parameterDataRow.IsNull("Address3") = True OrElse parameterDataRow("Address3").ToString.Trim = String.Empty) Then
				Me.TextBoxAddress3.Text = String.Empty
			Else
				Me.TextBoxAddress3.Text = CType(parameterDataRow("Address3"), String).Trim
			End If
			'YREN-3028,YREN-3029
			If (parameterDataRow.IsNull("StateName") = True OrElse parameterDataRow("StateName").ToString.Trim = String.Empty) Then
				Me.TextBoxState.Text = String.Empty
			Else
				'Me.TextBoxAddress1.Text = Me.TextBoxAddress1.Text & "," & ControlChars.CrLf & CType(parameterDataRow("StateName"), String).Trim
				Me.TextBoxState.Text = CType(parameterDataRow("StateName"), String).Trim
			End If

			If (parameterDataRow.IsNull("City") = True OrElse parameterDataRow("City").ToString.Trim = String.Empty) Then
				Me.TextBoxCity.Text = String.Empty
			Else
				'Me.TextBoxAddress1.Text = Me.TextBoxAddress1.Text & ", " & CType(parameterDataRow("City"), String).Trim
				Me.TextBoxCity.Text = CType(parameterDataRow("City"), String).Trim
			End If

			'YREN-3028,YREN-3029
			If (parameterDataRow.IsNull("CountryName") = True OrElse parameterDataRow("CountryName").ToString.Trim = String.Empty) Then
				Me.TextBoxCountry.Text = String.Empty
			Else
				'Me.TextBoxAddress1.Text = Me.TextBoxAddress1.Text & "," & ControlChars.CrLf & CType(parameterDataRow("CountryName"), String).Trim
				Me.TextBoxCountry.Text = CType(parameterDataRow("CountryName"), String).Trim
			End If

			If (parameterDataRow.IsNull("Zip") = True OrElse parameterDataRow("Zip").ToString.Trim = String.Empty) Then
				Me.TextBoxPin.Text = String.Empty
			Else
				Me.TextBoxPin.Text = CType(parameterDataRow("Zip"), String).Trim
				If parameterDataRow("CountryName").ToString = "CANADA" Then
					l_str_zipcode = TextBoxPin.Text.Trim
					'Me.TextBoxAddress1.Text = Me.TextBoxAddress1.Text & ", " & l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
					Me.TextBoxPin.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
				Else
					'Me.TextBoxAddress1.Text = Me.TextBoxAddress1.Text & ", " & CType(parameterDataRow("Zip"), String).Trim
					Me.TextBoxPin.Text = CType(parameterDataRow("Zip"), String).Trim
				End If
			End If
		End If
	End Function

	Private Function LoadDeductions()
		Dim l_DataTable As DataTable
		l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetDeductions()
		Me.DataGridDeductions.DataSource = l_DataTable
		Me.DataGridDeductions.DataBind()
		Session("R_Deductions") = l_DataTable
	End Function
	Private Function LoadExistingDeductions()
		Dim l_DataTable As DataTable
		l_DataTable = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.ExistingWithholdingsByDisbursement(RefRequestID)
		If HelperFunctions.isNonEmpty(l_DataTable) Then
			hypExistingDeductions.Visible = True
			hypExistingDeductions.Text = "Existing deductions :$ "
			hypExistingDeductions.Text = hypExistingDeductions.Text & l_DataTable.Compute("SUM(Amount)", "").ToString() & " (Hide)"
			Me.dgExistingDeductions.DataSource = l_DataTable
			Me.dgExistingDeductions.DataBind()
			Session("R_ExistingDeductions") = l_DataTable
			dgExistingDeductions.Visible = True
			dgExistingDeductions.Style.Add("display", "block")
		Else
			hypExistingDeductions.Text = ""
			dgExistingDeductions.DataSource = Nothing
			Me.dgExistingDeductions.DataBind()
			dgExistingDeductions.Visible = False
		End If
	End Function
	'Priya              31-Dec-2009     BT-1078 : Handling of existing decuctions in Withdrawal Reissue
	Private Sub GetDedutions()
		Dim l_CheckBox As CheckBox
		Dim l_Decimal_Amount As Decimal = 0.0
		DeductionsAmount = 0
		Try
			'For Existing Deduction
			For Each l_DataGridItem As DataGridItem In Me.dgExistingDeductions.Items
				l_CheckBox = l_DataGridItem.FindControl("chkExistingDeductions")
				If Not l_CheckBox Is Nothing Then
					If l_CheckBox.Checked = True Then
						If l_DataGridItem.Cells.Item(3).Text.Trim <> "" Then
							l_Decimal_Amount += CType(l_DataGridItem.Cells.Item(3).Text.Trim, Decimal)
						End If
					End If
				End If
			Next
			'Priya  31-Dec-2009     BT-1078
			Me.DeductionsAmount = DeductionsAmount + l_Decimal_Amount
			'End 31-Dec-2009

			'New Deductions
			l_Decimal_Amount = 0
			Dim j = 0
			For j = 0 To DataGridDeductions.Items.Count - 1
				l_CheckBox = DataGridDeductions.Items(j).FindControl("CheckBoxDeduction")
				If Not l_CheckBox Is Nothing Then
					If l_CheckBox.Checked = True Then
						If (DataGridDeductions.Items(j).Cells(3).Text.Trim <> "") Then
							l_Decimal_Amount += CType(DataGridDeductions.Items(j).Cells(3).Text.Trim, Decimal)
						End If
					End If
				End If
			Next
			Me.DeductionsAmount = DeductionsAmount + l_Decimal_Amount
		Catch CaEx As InvalidCastException
			Me.DeductionsAmount = 0.0
			Me.TextBoxDeductions.Text = Me.DeductionsAmount
		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
		End Try
	End Sub
	'End 31-Dec-2009
	Private Sub DataGridDeductions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridDeductions.ItemDataBound
		Try
			e.Item.Cells(0).Visible = False
			If (e.Item.ItemIndex <> -1) Then
				If e.Item.Cells(3).Text.ToUpper <> "&NBSP;" And e.Item.Cells(3).Text <> String.Empty Then
					Dim l_decimal_try As Decimal
					l_decimal_try = Convert.ToDecimal(e.Item.Cells(3).Text)
					e.Item.Cells(3).Text = FormatCurrency(l_decimal_try)
				End If
			End If
		Catch ex As Exception
			Throw
		End Try
	End Sub
	'anita and shubhrata 
	Public Shared Function FormatCurrency(ByVal paramNumber As Decimal) As String
		Try
			Return paramNumber.ToString("#,###,##0.00")
		Catch ex As Exception
			Return paramNumber
		End Try

	End Function
	Private Sub EnableDisableSaveButton()
		If Me.IsAnswered = True Then
			' If Me.NetAmount > 0.0 Then
			'BT:865- Could not Withdrawal-Reissue the MRD disbursement
			' If Convert.ToDecimal(TextBoxNet.Text) > 0.01 OrElse Convert.ToDecimal(TextBoxNet2.Text) > 0.01 Then 'OrElse Convert.ToDecimal(TextBoxNet3.Text) > 0.01 Then

			'BS:2011.10.05 - cleanup-
			'If Convert.ToDecimal(TextBoxNet.Text) > 0.01 OrElse Convert.ToDecimal(TextBoxNet2.Text) > 0.01 OrElse TextBoxMRDNet.Text > 0.01 Then 'OrElse Convert.ToDecimal(TextBoxNet3.Text) > 0.01 Then
			If Convert.ToDecimal(TextBoxNet.Text) > 0.01 OrElse Convert.ToDecimal(TextBoxNet1.Text) > 0.01 OrElse TextBoxMRDNet.Text > 0.01 Then
				Me.ButtonSave.Visible = True
				Me.ButtonSave.Enabled = True
			End If
		Else
			ButtonSave.Enabled = False
		End If
	End Sub
	Private Function IsAnswered() As Boolean
		Dim l_BooleanFlag As Boolean = True
		If Me.CheckBoxRolloverYes.Checked = False AndAlso Me.CheckBoxRolloverNo.Checked = False Then
			l_BooleanFlag = False
		End If

		If Me.CheckBoxWithholdingYes.Checked = False AndAlso Me.CheckBoxWithholdingNo.Checked = False Then
			l_BooleanFlag = False
		End If

		Return l_BooleanFlag
	End Function
	Private Function SaveReIssue() As Boolean
		Try
			'********************************************************************************************
			'* Get the Address History Identifier - Needs to be rewised/
			'********************************************************************************************
			''Me.PayeeAddressID() -- refer
			Me.AddressID = Me.GetAddressID
			'***********************************************************************************************
			'* Determine what Currency Code to Use
			'***********************************************************************************************
			Me.CurrencyCode = Me.GetCurrencyCode()
			'********************************************************************************************
			'* Check for the Refundabele amount.
			'********************************************************************************************
			'Priya commented
			'If Me.GetRefundable(Me.FundID) = False Then
			'    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "No Refundable amount available. Please Contact Support.", MessageBoxButtons.Stop, False)
			'    Me.IsSuccessfull = False
			'    Return False
			'End If
			'********************************************************************************************
			'* Set the tables to add the Rows 
			'********************************************************************************************
			Me.OpenFiles()
			'********************************************************************************************
			'* Create Tables
			'********************************************************************************************
			Me.CreateDisbursementTables()
			'********************************************************************************************
			'* Create RefundRequest & Reissue Transaction.
			'********************************************************************************************
			' Me.CreateTransactionTables()
			'********************************************************************************************
			'* Save the DataTable ()
			'********************************************************************************************
			Me.SaveAllTable()
		Catch ex As Exception
			Throw
		End Try
	End Function
	Private Function SaveAllTable()

		Dim l_RefundsDataTable As DataTable
		Dim l_ReissueTransaction As DataTable
		Dim l_DisbursementsDataTable As DataTable
		Dim l_DisbursementDetailsDataTable As DataTable
		Dim l_DisbursementWithholdingDataTable As DataTable
		Dim l_DisbursementRefundsDataTable As DataTable
		'Dim l_RefundRequestDataTable As DataTable
		Dim l_DataSet As DataSet
		Dim l_BooleanFlag As Boolean

		Try
			l_DisbursementsDataTable = CType(Session("RefundTransaction"), DataSet).Tables("Disbursements")
			l_DisbursementDetailsDataTable = CType(Session("RefundTransaction"), DataSet).Tables("DisbursementDetails")
			l_DisbursementWithholdingDataTable = CType(Session("RefundTransaction"), DataSet).Tables("DisbursementWithholding")
			l_DisbursementRefundsDataTable = CType(Session("RefundTransaction"), DataSet).Tables("DisbursementRefunds")
			'l_RefundRequestDataTable = CType(Session("R_RefundRequest"), DataTable)


			'' Create a New DataSet and Push the DataTable 
			l_DataSet = New DataSet("RefundTables")

			l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementsDataTable, "DisbursementsDataTable"))
			l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementDetailsDataTable, "DisbursementDetailsDataTable"))
			l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementWithholdingDataTable, "DisbursementWithholdingDataTable"))
			l_DataSet.Tables.Add(Me.CopyTable(l_DisbursementRefundsDataTable, "DisbursementRefundsDataTable"))
			'l_DataSet.Tables.Add(Me.CopyTable(l_RefundRequestDataTable, "RefundRequestDataTable"))

			'' Save the DataTables.
			l_BooleanFlag = YMCARET.YmcaBusinessObject.ReissueRefund.SaveRefundReIssue(l_DataSet)

			If l_BooleanFlag = True Then
				ShowMessageBox("Withdrawal has been Re - issued Successfully.")
				'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Refund has been Re - issued Successfully.", MessageBoxButtons.OK, False)
				Me.IsSuccessfull = True
			Else
				ShowMessageBox("Requested withdrawal is not Re - Issued due to Error.")
				'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Requested refund is not Re - Issued due to Error.", MessageBoxButtons.Stop, False)
				Me.IsSuccessfull = False
			End If
		Catch ex As Exception
			Throw
		End Try
	End Function

	Private Function CopyTable(ByVal parameterDataTable As DataTable, ByVal parameterDataTableName As String) As DataTable

		Dim l_DataTable As DataTable
		Dim l_DataRow As DataRow

		If parameterDataTable Is Nothing Then Return Nothing

		l_DataTable = parameterDataTable.Clone
		If Not parameterDataTableName.Trim = String.Empty Then
			l_DataTable.TableName = parameterDataTableName
		End If

		For Each l_DataRow In parameterDataTable.Rows
			l_DataTable.ImportRow(l_DataRow)
		Next
		Return l_DataTable

	End Function

	Private Function CreateDisbursementTables()
		Dim l_string_DisbId As String
		Dim l_DataRow As DataRow
		Dim l_DisbursementDataTable As DataTable
		Dim l_RefundableDataTable As DataTable
		Dim l_ReissueTransacts As DataTable
		Dim l_DisbursementDetails As DataTable
		Dim l_DisbursementWithholdingDataTable As DataTable
		Dim l_DisbursementRefundsDataTable As DataTable = Nothing
		Dim strDisbursementRefID As String = ""

		Dim old_ReissueDisbursementDetails As DataSet
		'if disbursement detail exist for same fundId then its will come from database
		old_ReissueDisbursementDetails = YMCARET.YmcaBusinessObject.ReissueRefund.GetReissueDisbursementDetails(RefRequestID)
		If HelperFunctions.isNonEmpty(old_ReissueDisbursementDetails) Then
			If old_ReissueDisbursementDetails.Tables(0).Rows(0)("DisbursementID") <> Nothing Then
				strDisbursementRefID = old_ReissueDisbursementDetails.Tables(0).Rows(0)("DisbursementID")
			End If

		End If
		Try
			l_DisbursementDataTable = CType(Session("RefundTransaction"), DataSet).Tables("Disbursements")
			'declare variable 
			Dim i = 0
			Dim payee1, payee2 As Integer

			'if payee2 name & rollover Amt is exist then increment i & assign payee2 variable
			If (TextBoxPayee2.Text.Trim <> "" AndAlso TextBoxPayeeAmt.Text <> "") Then
				i = i + 1
				payee2 = 1
			End If

			If Convert.ToDecimal(TextBoxTaxable.Text) > 0 OrElse Convert.ToDecimal(TextBoxNonTaxable.Text) > 0 OrElse Convert.ToDecimal(TextBoxMRDTaxable.Text) > 0 Then
				i = i + 1
				payee1 = 1
			End If

			'Getting Computed current taxable,Nontaxable,Tax,Net value from GetRefundAdjustment

			Dim refAdjust As RefundAdjustment = ComputeSpecificRollover()

			' here we have use computed total taxable,nontaxable and mrdtaxable value from current session,here  using k value we have compute payee1 record & payee2 record.
			'if amount has not be rollover then its compute only payee1 record.

			If Not l_DisbursementDataTable Is Nothing Then
				Dim k As Integer
				For k = 0 To i - 1
					Dim dr_Disbursement As DataRow
					dr_Disbursement = l_DisbursementDataTable.NewRow
					dr_Disbursement("UniqueID") = k	'Guid.NewGuid().ToString()

					If Me.CheckBoxRolloverYes.Checked Then
						If (payee1 = 1) Then
							dr_Disbursement("PayeeEntityID") = Me.PersonID	  '&& Payee1 Entity ID
							dr_Disbursement("PayeeEntityTypeCode") = "PERSON"
							dr_Disbursement("TaxableAmount") = refAdjust.Payee1Taxable + refAdjust.MRDTaxable
							dr_Disbursement("NonTaxableAmount") = refAdjust.Payee1NonTaxable + refAdjust.NonMRDTaxable
							payee1 = 0
						ElseIf (payee2 = 1) Then
							dr_Disbursement("PayeeEntityID") = Me.Payee2ID	 '&& Payee2 Entity ID
							dr_Disbursement("PayeeEntityTypeCode") = "ROLINS"

							dr_Disbursement("TaxableAmount") = refAdjust.Payee2Taxable
							dr_Disbursement("NonTaxableAmount") = refAdjust.Payee2NonTaxable
							payee2 = 0
						End If
					Else
						dr_Disbursement("PayeeEntityID") = Me.PersonID	  '&& Payee1 Entity ID
						dr_Disbursement("PayeeEntityTypeCode") = "PERSON"
						dr_Disbursement("TaxableAmount") = refAdjust.Payee1Taxable + refAdjust.MRDTaxable
						dr_Disbursement("NonTaxableAmount") = refAdjust.Payee1NonTaxable + refAdjust.NonMRDTaxable

					End If

					dr_Disbursement("PayeeAddrID") = Me.PayeeAddressID

					dr_Disbursement("DisbursementType") = Me.DisbursementType
					'anita bug tracker 345        
					dr_Disbursement("IrsTaxTypeCode") = System.DBNull.Value				  '&& Irs Tax Type Code

					dr_Disbursement("PaymentMethodCode") = "CHECK"						  '&& Payment Method
					dr_Disbursement("CurrencyCode") = Me.CurrencyCode					  '&& Currency Code
					dr_Disbursement("BankID") = System.DBNull.Value						  '&& Bank ID
					dr_Disbursement("DisbursementNumber") = System.DBNull.Value			  '&& Disbursement Number (Check No)
					dr_Disbursement("PersID") = Me.PersonID								  '&& Person ID
					dr_Disbursement("DisbursementRefID") = strDisbursementRefID	'&& Disburs Ref ID
					dr_Disbursement("Rollover") = System.DBNull.Value					  '&& Rollover Institution Type

					l_DisbursementDataTable.Rows.Add(dr_Disbursement)

				Next

			End If

			'here we set newdisbursment table from oldDisbursement table
			setReissueDisbursementDetails(l_DisbursementDataTable, old_ReissueDisbursementDetails)

			l_RefundableDataTable = CType(Session("R_Refundable"), DataTable)
			l_ReissueTransacts = CType(Session("R_ReissueTransaction"), DataTable)
			l_DisbursementDetails = CType(Session("RefundTransaction"), DataSet).Tables("DisbursementDetails")

			Dim l_Decimal_PersonalPreTax As Decimal
			Dim l_Decimal_PersonalPostTax As Decimal
			Dim l_Decimal_PersonalYMCAPreTax As Decimal
			Dim l_ReissueTransactionDataRow As DataRow
			Dim l_DisbursementDetailsDataRow As DataRow
			Dim l_string_AcctType As String = ""
			Me.TransactID = Nothing

			l_DisbursementWithholdingDataTable = CType(Session("RefundTransaction"), DataSet).Tables("DisbursementWithholding")
			l_DisbursementRefundsDataTable = CType(Session("RefundTransaction"), DataSet).Tables("DisbursementRefunds")

			Dim l_WithholdingDataRow As DataRow
			Dim l_DisbursementRefundDataRow As DataRow
			Dim l_RefundsDataRow As DataRow
			Dim l_CheckBox As CheckBox

			' here we are going to handle additional deduction for disbursment  tables
			If Not l_DisbursementWithholdingDataTable Is Nothing Then
				'If TaxAmount <> 0 Or refAdjust.MRDTax > 0 Then
				'	l_WithholdingDataRow = l_DisbursementWithholdingDataTable.NewRow
				'	l_WithholdingDataRow("DisbursementID") = "0"
				'	l_WithholdingDataRow("WithholdingTypeCode") = "FEDTAX"
				'	l_WithholdingDataRow("Amount") = Me.TaxAmount + Math.Round(refAdjust.MRDTax, 2)
				'	l_DisbursementWithholdingDataTable.Rows.Add(l_WithholdingDataRow)
				'End If
				'BS:2011.11.08 :-reopen Issue :YRS 5.0-1451-TaxAmount varaible not use in current code
				If refAdjust.Payee1Tax <> 0 Or refAdjust.MRDTax > 0 Then
					l_WithholdingDataRow = l_DisbursementWithholdingDataTable.NewRow
					l_WithholdingDataRow("DisbursementID") = "0"
					l_WithholdingDataRow("WithholdingTypeCode") = "FEDTAX"
					l_WithholdingDataRow("Amount") = refAdjust.Payee1Tax + Math.Round(refAdjust.MRDTax, 2)
					l_DisbursementWithholdingDataTable.Rows.Add(l_WithholdingDataRow)
				End If
				'
				Dim l_WithholdingDataRow_DisbursementID As String
				If refAdjust.Payee1Deduction > 0 Then
					l_WithholdingDataRow_DisbursementID = "0"
				ElseIf refAdjust.Payee2Deduction > 0 AndAlso l_DisbursementDataTable.Rows.Count = 2 Then
					l_WithholdingDataRow_DisbursementID = "1"
				ElseIf refAdjust.Payee2Deduction > 0 Then
					l_WithholdingDataRow_DisbursementID = "0"
					'BS:2011.11.25--Add new disbursmentid for MrdDeduction
				ElseIf refAdjust.MrdDeduction > 0 Then
					l_WithholdingDataRow_DisbursementID = "0"
				ElseIf refAdjust.Payee1Net = 0 AndAlso refAdjust.Payee2Net = 0 AndAlso refAdjust.MrdNet > 0.0 Then
					l_WithholdingDataRow_DisbursementID = "0"
				End If
				If Me.DeductionsAmount > 0.0 Then

					For Each l_DataGridItem As DataGridItem In Me.DataGridDeductions.Items

						l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")

						If l_CheckBox Is Nothing Then Continue For
						If l_CheckBox.Checked = False Then Continue For
						If l_DataGridItem.Cells.Item(3).Text.Trim <> String.Empty Then
							'Add a new row into WithHolding DataTable.
							l_WithholdingDataRow = l_DisbursementWithholdingDataTable.NewRow
							l_WithholdingDataRow("DisbursementID") = l_WithholdingDataRow_DisbursementID
							l_WithholdingDataRow("WithholdingTypeCode") = l_DataGridItem.Cells.Item(0).Text.Trim
							l_WithholdingDataRow("Amount") = CType(l_DataGridItem.Cells.Item(3).Text.Trim, Decimal)
							l_DisbursementWithholdingDataTable.Rows.Add(l_WithholdingDataRow)
						End If
					Next
				End If
				If HelperFunctions.isNonEmpty(CType(Session("R_ExistingDeductions"), DataTable)) Then
					For Each l_DataGridItem As DataGridItem In Me.dgExistingDeductions.Items
						l_CheckBox = l_DataGridItem.FindControl("chkExistingDeductions")
						If l_CheckBox Is Nothing Then Continue For
						If l_CheckBox.Checked = False Then Continue For
						If l_DataGridItem.Cells.Item(3).Text.Trim <> String.Empty Then
							l_WithholdingDataRow = l_DisbursementWithholdingDataTable.NewRow
							l_WithholdingDataRow("DisbursementID") = l_WithholdingDataRow_DisbursementID
							l_WithholdingDataRow("WithholdingTypeCode") = l_DataGridItem.Cells.Item(0).Text.Trim
							l_WithholdingDataRow("Amount") = CType(l_DataGridItem.Cells.Item(3).Text.Trim, Decimal)
							l_DisbursementWithholdingDataTable.Rows.Add(l_WithholdingDataRow)
						End If
					Next
				End If
			End If
			'' add Disbursement Refunds.. 
			If Not l_DisbursementRefundsDataTable Is Nothing Then
				Dim i_disbursementCount As Integer
				For i_disbursementCount = 0 To l_DisbursementDataTable.Rows.Count - 1
					Dim dr_disbursementRefundDataRow As DataRow
					dr_disbursementRefundDataRow = l_DisbursementRefundsDataTable.NewRow

					dr_disbursementRefundDataRow("RefRequestID") = Me.RefRequestID
					dr_disbursementRefundDataRow("DisbursementID") = l_DisbursementDataTable.Rows(i_disbursementCount)("UniqueID")
					dr_disbursementRefundDataRow("Voided") = False
					dr_disbursementRefundDataRow("ActionType") = "REISUUE" 'BS:2011.10.21-YRS 5.0-1451-this value updated from proc:[yrs_usp_VD_InsertDisbRefunds]
					dr_disbursementRefundDataRow("ReissuedDisbursements") = True

					l_DisbursementRefundsDataTable.Rows.Add(dr_disbursementRefundDataRow)
				Next
			End If
		Catch
			Throw
		End Try
	End Function
	'BS:2011.10.04 -End-YRS 5.0-1451 
	Private Function DoValidation() As String
		Dim l_ErrorMessage As String
		If Convert.ToDecimal(TextBoxNet.Text) < 0.01 AndAlso Convert.ToDecimal(TextBoxNet1.Text) < 0.01 AndAlso Convert.ToDecimal(TextBoxMRDNet.Text) < 0.01 Then
			Return "The Net Amount of the Refund Can Not be Less Than $ 0.01. Please Adjust Amounts"
		End If
		'********************************************************************************************
		'* Check to see if they wanted a Rollover but Didn;t do it
		'********************************************************************************************
		'' Check for RollOver all option
		If Me.CheckBoxRolloverYes.Checked = True Then
			If Me.TextBoxPayee2.Text.Trim.Length < 1 AndAlso Me.TextBoxPayeeAmt.Text.Trim.Length < 1 Then
				Return "A Rollover was Requested but there is No 2nd Payee Information. Please enter the Payee 2 Information"
			End If
			'' Set the PayeeID & Payee Type..
			l_ErrorMessage = Me.GetPayeeID(Me.TextBoxPayee2.Text, "Payee2")

			If Not l_ErrorMessage = String.Empty Then
				Return l_ErrorMessage
			End If
		End If
		Return String.Empty
	End Function
	Private Function GetPayeeID(ByVal parameterPayeeName As String, ByVal strPayee As String) As String
		Dim l_ErrorMessage As String
		Dim l_string_RolloverInstitutionID As String
		'Get the InstitutionID 
		YMCARET.YmcaBusinessObject.RefundRequest.Get_RefundRolloverInstitutionID(parameterPayeeName, l_string_RolloverInstitutionID)
		If l_string_RolloverInstitutionID.Trim() = "" Then
			Return "Unable to retrive Rollover Institution Information Data"
		Else
			If strPayee = "Payee2" Then
				Me.Payee2ID = l_string_RolloverInstitutionID
			End If
			Return String.Empty
		End If
	End Function
	'***********************************************************************************************
	'* Determine what Currency Code to Use
	'***********************************************************************************************
	Private Function GetCurrencyCode() As String
		Return YMCARET.YmcaBusinessObject.RefundRequest.GetPersonBankingBeforeEffectiveDate(Me.PersonID)
	End Function

	'' This Function is uesed Get the Schema for the further Transaction.
	Private Function OpenFiles() As Boolean
		Dim l_DataSet As DataSet
		Dim l_DataTable As DataTable
		l_DataSet = YMCARET.YmcaBusinessObject.ReissueRefund.GetRefundSchemas()
		If l_DataSet Is Nothing Then Return False
		Session("RefundTransaction") = l_DataSet
		Return True
	End Function

	Private Function GetAccountBreakDownType(ByVal parameterAccountType As String, ByVal parameterPersonal As Boolean, ByVal parameterYMCA As Boolean, ByVal parameterPreTax As Boolean, ByVal parameterPostTax As Boolean) As String
		Dim l_DataTable As DataTable
		Dim l_DataRow As DataRow
		Dim l_FoundRows As DataRow()
		Dim l_QueryString As String

		l_DataTable = CType(Session("AccountBreakDown"), DataTable)
		If l_DataTable Is Nothing Then Return String.Empty

		l_QueryString = "chrAcctType = '" & parameterAccountType.Trim & "' "

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

		l_FoundRows = l_DataTable.Select(l_QueryString)

		If l_FoundRows.Length > 0 Then
			l_DataRow = l_FoundRows(0)
			Return (CType(l_DataRow("chrAcctBreakDownType"), String))
		Else
			Return String.Empty
		End If
	End Function

	Private Function GetAddressID() As Integer
		Dim l_DataSet As DataSet
		Dim l_DataTable As DataTable
		Dim l_DataRow As DataRow
		l_DataSet = Session("PersonInformation")
		If Not l_DataSet Is Nothing Then
			' To Find Address ID
			l_DataTable = l_DataSet.Tables("Member Address")

			If Not l_DataTable Is Nothing Then

				For Each l_DataRow In l_DataTable.Rows

					If CType(l_DataRow("AddressHistoryID"), Integer) > 0 Then
						Return (CType(l_DataRow("AddressHistoryID"), Integer))
					End If

				Next

			End If
			Return 0
		End If
	End Function

	Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
		Try
			Dim strOutputMessage As String
			'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
			Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
			If Not checkSecurity.Equals("True") Then
				MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
				Exit Sub
			End If
			'End : YRS 5.0-940
			If ShowMessage = True Then Exit Sub
			'BS:2011.10.10--YRS 5.0-1451 Basic Validation
			strOutputMessage = Validation()
			If strOutputMessage <> Nothing OrElse strOutputMessage <> String.Empty Then
				ShowMessageBox(strOutputMessage)
				Exit Sub
			End If
			'BS:2011.10.20-YRS 5.0-1451-call VerifyRefundAdjustment
			Dim strVerifyOutputMessage As String = VerifyRefundAdjustment()
			If strVerifyOutputMessage <> Nothing OrElse strVerifyOutputMessage <> String.Empty Then
				ShowMessageBox(strVerifyOutputMessage)
				Exit Sub
			End If
			Me.SaveReIssue()

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
		Response.Redirect("MainWebForm.aspx", False)
	End Sub

	'Priya 16-Nov-09 BT-1017: Problem in Withdrawal - Re-issue process. - Changed the method to return a string message rather than populating the message box directly. The caller must check the returned string for error handling.
	Private Sub TabStripWithdrawalReissue_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripWithdrawalReissue.SelectedIndexChange
		Me.MultiPageVRManager.SelectedIndex = Me.TabStripWithdrawalReissue.SelectedIndex
	End Sub
	'Priya 07-Sep-09
	Private Function SetNewReissueDisbursementRow(ByVal DisbursementId As String, ByVal dr As DataRow, ByRef dt As DataTable) As DataRow
		Dim drNew As DataRow()
		Dim l_dataRow As DataRow

		drNew = dt.Select("DisbursementID='" & DisbursementId & "' AND AcctType='" & dr("AcctType") & "' AND AcctBreakdownType='" & dr("AcctBreakdownType") & "'")
		'drNew = dt.Select("AcctType='" & dr("AcctType") & "' AND AcctBreakdownType='" & dr("AcctBreakdownType") & "'")

		If drNew Is Nothing OrElse drNew.Length = 0 Then
			l_dataRow = dt.NewRow()

			'Set Values to Datarow
			l_dataRow("UniqueID") = System.DBNull.Value						'&& UniqueID
			l_dataRow("DisbursementID") = DisbursementId 'dr("UniqueID")            '&& Disbursment ID, I am using for Temp.
			l_dataRow("AcctType") = dr("AcctType")			   '&& Account Type
			l_dataRow("AcctBreakdownType") = dr("AcctBreakdownType")   '&& Account Break Down
			If (dr("SortOrder") Is System.DBNull.Value) Then
				l_dataRow("SortOrder") = 999
			Else
				l_dataRow("SortOrder") = dr("SortOrder")
			End If

			'&& Check Sort order
			l_dataRow("TaxablePrincipal") = 0.0
			l_dataRow("TaxableInterest") = 0.0
			l_dataRow("TaxWithheldPrincipal") = 0.0
			l_dataRow("TaxWithheldInterest") = 0.0
			l_dataRow("NonTaxablePrincipal") = 0.0

			dt.Rows.Add(l_dataRow)
		Else
			l_dataRow = drNew(0)
		End If
		Return l_dataRow
	End Function

	'Priya 11/30/2010 BT-592,YRS 5.0-1171 : Disbursement Ref Id not filled in 
	'Change signature Pass another paarmeter to make disbursement details table
	Private Sub setReissueDisbursementDetails(ByVal disbursementDataTable As DataTable, ByVal ds_old_ReissueDisbursementDetails As DataSet)
		Dim old_ReissueDisbursementDetails As DataSet
		Dim new_ReissueDisbursementDetails As DataTable
		Dim l_Payee1TaxRate As Decimal
		If (TextBoxRate.Text <> String.Empty) Then
			l_Payee1TaxRate = Convert.ToDecimal(TextBoxRate.Text)
		Else
            'Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to l_Payee1TaxRate
            'l_Payee1TaxRate = 20.0
            l_Payee1TaxRate = Me.TaxRate
            'End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to l_Payee1TaxRate
        End If


		old_ReissueDisbursementDetails = ds_old_ReissueDisbursementDetails 'YMCARET.YmcaBusinessObject.ReissueRefund.GetReissueDisbursementDetails(RefRequestID)
		new_ReissueDisbursementDetails = old_ReissueDisbursementDetails.Tables("DisbursementDetails").Clone

		Dim filteredRow As DataRow()

		Dim strQuery, l_AcctType As String
		Dim l_Taxable, l_NonTaxable As Decimal
		Dim l_NewTaxable, l_TaxableBal, l_NewNonTaxable As String

		Dim i, k, j As Integer

		For i = 0 To disbursementDataTable.Rows.Count - 1

			For j = 0 To CType(Session("Payee1DataTable"), DataTable).Rows.Count - 1

				l_AcctType = CType(Session("Payee1DataTable"), DataTable).Rows(j)("AcctType")

				If (disbursementDataTable.Rows(i)("PayeeEntityTypeCode") = "PERSON") Then
					'14-Feb-2011 Priya BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2 
					'l_Taxable = Convert.ToDecimal(CType(Session("Payee1DataTable"), DataTable).Rows(j)("Taxable"))

					l_Taxable = Convert.ToDecimal(CType(Session("Payee1DataTable"), DataTable).Rows(j)("Taxable")) + Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("Taxable"))
					'End 14-Feb-2011 Priya BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2 
					'l_NonTaxable = Convert.ToDecimal(CType(Session("Payee1DataTable"), DataTable).Rows(j)("NonTaxable"))
					'BS:2011.11.02:YRS 5.0-1451-reopen issue rmd(mrd) nontaxable component deduct from current nontaxable component
					l_NonTaxable = Convert.ToDecimal(CType(Session("Payee1DataTable"), DataTable).Rows(j)("NonTaxable")) + Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("NonTaxable"))

				ElseIf (disbursementDataTable.Rows(i)("PayeeEntityTypeCode") = "ROLINS") Then
					l_Taxable = Convert.ToDecimal(CType(Session("Payee2DataTable"), DataTable).Rows(j)("Taxable"))
					l_NonTaxable = Convert.ToDecimal(CType(Session("Payee2DataTable"), DataTable).Rows(j)("NonTaxable"))
				End If

				strQuery = "AcctType = '" & l_AcctType & "'"
				filteredRow = old_ReissueDisbursementDetails.Tables(0).Select(strQuery)


				For k = 0 To filteredRow.Length - 1

					Dim l_dataRow As DataRow
					l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)	 'dr(0)

					'ForNonTaxable
					If l_NonTaxable >= Convert.ToDecimal(filteredRow(k)("NonTaxablePrincipal")) AndAlso Convert.ToDecimal(filteredRow(k)("NonTaxablePrincipal")) > 0 Then

						l_NonTaxable = l_NonTaxable - Convert.ToDecimal(filteredRow(k)("NonTaxablePrincipal"))
						'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)

						l_dataRow("NonTaxablePrincipal") = l_dataRow("NonTaxablePrincipal") + filteredRow(k)("NonTaxablePrincipal")

						filteredRow(k)("NonTaxablePrincipal") = "0"

					ElseIf (l_NonTaxable > 0 AndAlso Convert.ToDecimal(filteredRow(k)("NonTaxablePrincipal")) > 0) Then
						'If amount is less than existing the added row for new break
						'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)

						l_dataRow("NonTaxablePrincipal") = l_dataRow("NonTaxablePrincipal") + l_NonTaxable
						filteredRow(k)("NonTaxablePrincipal") = Convert.ToDecimal(filteredRow(k)("NonTaxablePrincipal")) - l_NonTaxable

						l_NonTaxable = 0

					End If
					'End NonTaxable


					'Taxable Principal
					'If payees TaxableAmount is greter than original amount
					If l_Taxable >= Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) AndAlso Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) > 0 Then

						'l_NewTaxable = filteredRow(k)("TaxablePrincipal")
						l_Taxable = l_Taxable - Convert.ToDecimal(filteredRow(k)("TaxablePrincipal"))
						'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)

						l_dataRow("TaxablePrincipal") = l_dataRow("TaxablePrincipal") + Convert.ToDecimal(filteredRow(k)("TaxablePrincipal"))
						filteredRow(k)("TaxablePrincipal") = 0.0

						'Taxable Interest
						'Amount is greated than principal amount then take amount from interst
						If l_Taxable >= Convert.ToDecimal(filteredRow(k)("TaxableInterest")) AndAlso Convert.ToDecimal(filteredRow(k)("TaxableInterest")) > 0 Then

							l_Taxable = l_Taxable - filteredRow(k)("TaxableInterest")
							'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)

							l_dataRow("TaxableInterest") = l_dataRow("TaxableInterest") + filteredRow(k)("TaxableInterest")
							filteredRow(k)("TaxableInterest") = 0.0

						ElseIf Convert.ToDecimal(l_Taxable) > 0 AndAlso Convert.ToDecimal(filteredRow(k)("TaxableInterest")) > 0 Then
							'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)
							l_dataRow("TaxableInterest") = l_dataRow("TaxableInterest") + l_Taxable
							filteredRow(k)("TaxableInterest") = Convert.ToDecimal(filteredRow(k)("TaxableInterest")) - l_Taxable
							l_Taxable = 0
						End If

					ElseIf Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) > 0 AndAlso l_Taxable > 0 Then

						'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)
						l_dataRow("TaxablePrincipal") = l_dataRow("TaxablePrincipal") + l_Taxable

						'Balance amount added into new row for participant
						filteredRow(k)("TaxablePrincipal") = Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) - l_Taxable
						l_Taxable = 0
						'BT-983
					ElseIf Convert.ToDecimal(filteredRow(k)("TaxableInterest")) > 0 Then
						'Amount is greated than principal amount then take amount from interst
						If l_Taxable >= Convert.ToDecimal(filteredRow(k)("TaxableInterest")) Then
							l_Taxable = l_Taxable - filteredRow(k)("TaxableInterest")
							'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)
							l_dataRow("TaxableInterest") = l_dataRow("TaxableInterest") + filteredRow(k)("TaxableInterest")
							filteredRow(k)("TaxableInterest") = 0.0
						ElseIf Convert.ToDecimal(l_Taxable) > 0 Then
							'l_dataRow = SetNewReissueDisbursementRow(disbursementDataTable.Rows(i)("UniqueID"), filteredRow(k), new_ReissueDisbursementDetails)  'dr(0)
							l_dataRow("TaxableInterest") = l_dataRow("TaxableInterest") + l_Taxable
							filteredRow(k)("TaxableInterest") = Convert.ToDecimal(filteredRow(k)("TaxableInterest")) - l_Taxable
							l_Taxable = 0
						End If
						'End BT-983
					End If


					If (disbursementDataTable.Rows(i)("PayeeEntityTypeCode") = "PERSON") Then
						If l_dataRow("TaxablePrincipal") <> 0 Then
							l_dataRow("TaxWithheldPrincipal") = FormatCurrency(Convert.ToDecimal(Math.Round((l_Payee1TaxRate / 100) * l_dataRow("TaxablePrincipal"), 2)))
						End If
						If l_dataRow("TaxableInterest") <> 0 Then
							l_dataRow("TaxWithheldInterest") = FormatCurrency(Convert.ToDecimal(Math.Round((l_Payee1TaxRate / 100) * l_dataRow("TaxableInterest"), 2)))
						End If
						''Added on 21/06/2011 For BT:864-Withdrawal-Reissue inserting 20% withholding for MRD amount
						'If (Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("Taxable")) > 0) Then
						'    Dim l_MrdTaxRate As Decimal
						'    l_MrdTaxRate = Convert.ToDecimal(TextBoxRate.Text) - Convert.ToDecimal(TextBoxMRDRate.Text)

						'    'If l_dataRow("TaxablePrincipal") > 0 AndAlso l_dataRow("TaxablePrincipal") >= Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("Taxable")) Then
						'    If l_dataRow("TaxablePrincipal") > 0 Then
						'        l_dataRow("TaxWithheldPrincipal") = l_dataRow("TaxWithheldPrincipal") - FormatCurrency(Convert.ToDecimal(Math.Round((l_MrdTaxRate / 100) * Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("Taxable")), 2)))
						'    End If

						'    If l_dataRow("TaxableInterest") > 0 Then
						'        l_dataRow("TaxWithheldInterest") = l_dataRow("TaxWithheldInterest") - FormatCurrency(Convert.ToDecimal(Math.Round((l_MrdTaxRate / 100) * Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("Taxable")), 2)))
						'    End If
						'End If

					End If
					If l_dataRow("TaxablePrincipal") = 0 AndAlso l_dataRow("TaxableInterest") = 0 AndAlso l_dataRow("NonTaxablePrincipal") = 0 Then
						l_dataRow.Delete()
					End If
				Next
			Next
		Next
		'---MRD
		For j = 0 To CType(Session("R_MRDAcctBalance"), DataTable).Rows.Count - 1
			Dim l_MRDTaxRate As Decimal
			If (TextBoxMRDRate.Text <> String.Empty) Then
				l_MRDTaxRate = Convert.ToDecimal(TextBoxMRDRate.Text)
			Else
				l_MRDTaxRate = 10.0
			End If
			l_AcctType = CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("AcctType")

			l_Taxable = Convert.ToDecimal(CType(Session("R_MRDAcctBalance"), DataTable).Rows(j)("Taxable"))

			strQuery = "AcctType = '" & l_AcctType & "' AND (TaxWithheldPrincipal <> 0 OR TaxWithheldInterest <> 0)"
			filteredRow = new_ReissueDisbursementDetails.Select(strQuery)


			For k = 0 To filteredRow.Length - 1

				Dim l_dataRow As DataRow
				'Taxable Principal
				'If payees TaxableAmount is greter than original amount
				If l_Taxable >= Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) AndAlso Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) > 0 Then

					'filteredRow(k)("TaxWithheldPrincipal") = Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) * Me.TextBoxMRDTax
					filteredRow(k)("TaxWithheldPrincipal") = Math.Round(Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) * FormatCurrency(Convert.ToDecimal(Math.Round((l_MRDTaxRate / 100), 2))), 2)

					l_Taxable = Math.Round(l_Taxable - Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")), 2)

					'Taxable Interest
					'Amount is greated than principal amount then take amount from interst
					If l_Taxable >= Convert.ToDecimal(filteredRow(k)("TaxableInterest")) AndAlso Convert.ToDecimal(filteredRow(k)("TaxableInterest")) > 0 Then
						filteredRow(k)("TaxWithheldInterest") = Math.Round(Convert.ToDecimal(filteredRow(k)("TaxableInterest")) * FormatCurrency(Convert.ToDecimal(Math.Round((l_MRDTaxRate / 100), 2))), 2)
						l_Taxable = Math.Round(l_Taxable - filteredRow(k)("TaxableInterest"), 2)

					ElseIf Convert.ToDecimal(l_Taxable) > 0 AndAlso Convert.ToDecimal(filteredRow(k)("TaxableInterest")) > 0 Then
						filteredRow(k)("TaxWithheldInterest") = Math.Round((Convert.ToDecimal(filteredRow(k)("TaxableInterest")) - l_Taxable) * FormatCurrency(Convert.ToDecimal(Math.Round((Me.TaxRate / 100), 2))), 2) _
						   + Math.Round(l_Taxable * FormatCurrency(Convert.ToDecimal(Math.Round((l_MRDTaxRate / 100), 2))), 2)
						l_Taxable = 0
					End If

				ElseIf Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) > 0 AndAlso l_Taxable > 0 Then

					filteredRow(k)("TaxWithheldPrincipal") = Math.Round((Convert.ToDecimal(filteredRow(k)("TaxablePrincipal")) - l_Taxable) * FormatCurrency(Convert.ToDecimal(Math.Round((Me.TaxRate / 100), 2))), 2) _
					   + Math.Round(l_Taxable * FormatCurrency(Convert.ToDecimal(Math.Round((l_MRDTaxRate / 100), 2))), 2)
					l_Taxable = 0

				ElseIf Convert.ToDecimal(filteredRow(k)("TaxableInterest")) > 0 Then
					'Amount is greated than principal amount then take amount from interst
					If l_Taxable >= Convert.ToDecimal(filteredRow(k)("TaxableInterest")) Then

						filteredRow(k)("TaxWithheldInterest") = Convert.ToDecimal(filteredRow(k)("TaxableInterest")) * FormatCurrency(Convert.ToDecimal(Math.Round((l_MRDTaxRate / 100), 2)))
						l_Taxable = Math.Round(l_Taxable - filteredRow(k)("TaxableInterest"), 2)

					ElseIf Convert.ToDecimal(l_Taxable) > 0 Then

						filteredRow(k)("TaxWithheldInterest") = Math.Round((Convert.ToDecimal(filteredRow(k)("TaxableInterest")) - l_Taxable) * FormatCurrency(Convert.ToDecimal(Math.Round((Me.TaxRate / 100), 2))), 2) _
						   + Math.Round(l_Taxable * FormatCurrency(Convert.ToDecimal(Math.Round((l_MRDTaxRate / 100), 2))), 2)
						l_Taxable = 0
					End If

				End If

			Next
		Next
		'BS:2011.10.05---YRS 5.0-1451 --calculate total tax
		Dim l_TotalTax As Decimal
		If HelperFunctions.isNonEmpty(new_ReissueDisbursementDetails) Then
			l_TotalTax = Convert.ToDecimal(Math.Round(new_ReissueDisbursementDetails.Compute("SUM(TaxWithheldInterest)+SUM(TaxWithheldPrincipal)", ""), 2))
			'If Math.Round(l_TotalTax, 2) <> Math.Round(Convert.ToDecimal(TextBoxMRDTax.Text), 2) + Math.Round(Convert.ToDecimal(TextBoxTax.Text), 2) Then
			'	l_TotalTax = Math.Round((Convert.ToDecimal(Math.Round(Convert.ToDecimal(TextBoxMRDTax.Text), 2) + Math.Round(Convert.ToDecimal(TextBoxTax.Text), 2))) - l_TotalTax, 2)
			If (TextBoxMRDTax.Text <> String.Empty) AndAlso (TextBoxTax.Text <> String.Empty) Then
				If Math.Round(l_TotalTax, 2) <> Math.Round(Convert.ToDecimal(TextBoxMRDTax.Text), 2) + Math.Round(Convert.ToDecimal(TextBoxTax.Text), 2) Then
					l_TotalTax = Math.Round((Convert.ToDecimal(Math.Round(Convert.ToDecimal(TextBoxMRDTax.Text), 2) + Math.Round(Convert.ToDecimal(TextBoxTax.Text), 2))) - l_TotalTax, 2)

					For i = 0 To new_ReissueDisbursementDetails.Rows.Count - 1
						If new_ReissueDisbursementDetails.Rows(i)("TaxWithheldPrincipal") > 0 And new_ReissueDisbursementDetails.Rows(i)("TaxWithheldPrincipal") >= l_TotalTax Then
							new_ReissueDisbursementDetails.Rows(i)("TaxWithheldPrincipal") += Convert.ToDecimal(Math.Round(l_TotalTax, 2))
							Exit For
						ElseIf new_ReissueDisbursementDetails.Rows(i)("TaxWithheldInterest") > 0 And new_ReissueDisbursementDetails.Rows(i)("TaxWithheldInterest") >= l_TotalTax Then
							new_ReissueDisbursementDetails.Rows(i)("TaxWithheldInterest") += Convert.ToDecimal(Math.Round(l_TotalTax, 2))
							Exit For
						End If
					Next
				End If
			End If
		End If

		CType(Session("RefundTransaction"), DataSet).Tables.Remove("DisbursementDetails")
		CType(Session("RefundTransaction"), DataSet).Tables.Add(new_ReissueDisbursementDetails)
	End Sub
	'End 07-Sep-09
	Private Sub buttonDetailsOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonDetailsOK.Click
		TabStripWithdrawalReissue.Items(1).Enabled = False
		TabStripWithdrawalReissue.SelectedIndex = 0
		MultiPageVRManager.SelectedIndex = 0

		DataGridReissue.SelectedIndex = -1
		LoadReissueFromCache()
		'BT-994
		LoadDefalutValues()
		'End BT-994
		'DataGridReissue.SelectedItemStyle.CssClass = "DataGrid_NormalStyle"
		'Dim l_button_Select As ImageButton
		'l_button_Select = CType(DataGridReissue.Items(DataGridReissue.SelectedIndex).FindControl("ImageButtonSelect"), ImageButton)
		'If Not IsNothing(l_button_Select) Then
		'    l_button_Select.ImageUrl = "images\select.gif"
		'End If
		'LabelHeader.Text = ""
	End Sub
	'30-10-09: BT-997
	Private Sub ShowMessageBox(ByVal strMessage As String)
		If ShowMessage = True Then Exit Sub
		ShowMessage = True
		'BT-1016
		If strMessage = "Withdrawal has been Re - issued Successfully." Then
			MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", strMessage, MessageBoxButtons.OK)
		Else
			MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", strMessage, MessageBoxButtons.Stop)
		End If

		IsSuccessfull = False
	End Sub
	'End BT-997
	'BS:2011.10.10--YRS 5.0-1451-do Validation 
	Private Function Validation() As String

		'Check to see if they wanted a Rollover but Didn;t do it
		If (CheckBoxRolloverYes.Checked = True) Then
			If Me.TextBoxPayee2.Text.Trim.Length < 1 AndAlso Me.TextBoxPayeeAmt.Text.Trim.Length < 1 Then
				Return "A Rollover was Requested but there is No 2nd Payee Information. Please enter the Payee 2 Information"
			End If
			If (TextBoxPayee2.Text = String.Empty) Then
				Return "Please enter Payee2 Name."
			End If
			If (TextBoxPayeeAmt.Text = String.Empty OrElse Not IsNumeric(TextBoxPayeeAmt.Text) OrElse TextBoxPayeeAmt.Text = "0.00") Then
				TextBoxPayeeAmt.Text = "0.00"
				Return "Please enter a valid Payee2 amount."
			End If
			'Set the PayeeID & Payee Type into Global variables
			Dim l_ErrorMessage As String = Me.GetPayeeID(Me.TextBoxPayee2.Text, "Payee2")
			If Not l_ErrorMessage = String.Empty Then Return l_ErrorMessage
		End If
		If (CheckBoxWithholdingYes.Checked = True) Then
			Dim MrdTaxrate As Decimal
			If Decimal.TryParse(TextBoxMRDRate.Text, MrdTaxrate) = False Then
				Return "Please enter a valid tax rate for MRD."
			End If
			If (MrdTaxrate > 100 OrElse MrdTaxrate < 10) Then
				TextBoxMRDRate.Text = "10"
				Return "Invalid Tax Rate entered. Tax Rate should between 10% to 100%."
			End If
			'Change on Tax Rate and check disbursment type if it is ADT,RDT then Tax rate will be zero 
			Dim Taxrate As Decimal
			If Decimal.TryParse(TextBoxRate.Text, Taxrate) = False Then
				Return "Please enter a valid tax rate for Payee1."
			End If
			If Me.DisbursementType.Equals("ADT") OrElse Me.DisbursementType.Equals("RDT") Then
				If Taxrate > 100 Or Taxrate < 0 Then
					TextBoxRate.Text = "0"
					Return "Invalid Tax Rate entered. Tax Rate should between 0% to 100%."
				End If
			Else
				If Taxrate > 100 OrElse Taxrate < 20 Then
                    'Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to textbox
                    'TextBoxRate.Text = "20"
                    TextBoxRate.Text = Me.TaxRate
                    'End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to textbox
                    Return "Invalid Tax Rate entered. Tax Rate should between 20% to 100%."
				End If
			End If
		End If
		'Check if the calculations from the UI are not returning -ve values
		If Convert.ToDecimal(TextBoxNet.Text) < 0.01 AndAlso Convert.ToDecimal(TextBoxNet1.Text) < 0.01 AndAlso Convert.ToDecimal(TextBoxMRDNet.Text) < 0.01 Then
			Return "The Net Amount of the Refund Can Not be Less Than $ 0.01. Please Adjust Amounts"
		End If
		Return String.Empty
	End Function
	'BS:2011.10.05--YRS 5.0-1451- create RefundAdjustment class for calculate MRD,Payee1,Payee2 component
	Public Class RefundAdjustment
		Public ReqRolloverAmt As Decimal = 0.0
		Public MRDTaxable As Decimal = 0.0
		Public NonMRDTaxable As Decimal = 0.0
		Public MRDTax As Decimal = 0.0
		Public MRDTaxRate As Decimal = 0.0
		Public MrdNet As Decimal = 0.0
		Public MrdDeduction As Decimal = 0.0
		Public Payee1Taxable As Decimal = 0.0
		Public Payee1NonTaxable As Decimal = 0.0
		Public Payee1Tax As Decimal = 0.0
		Public Payee1TaxRate As Decimal = 0.0
		Public Payee1Net As Decimal = 0.0
		Public Payee1Deduction As Decimal
		Public Payee2Taxable As Decimal = 0.0
		Public Payee2NonTaxable As Decimal = 0.0
		Public Payee2Tax As Decimal = 0.0
		Public Payee2TaxRate As Decimal = 0.0
		Public Payee2Net As Decimal = 0.0
		Public Payee2Deduction As Decimal = 0.0
		Public deductionBalance As Decimal = 0.0
		Public ErrorMsg As String = String.Empty
	End Class
	'BS:2011.10.20-YRS 5.0-1451-Computation for rollover money and also adjust payee1,payee2 datable
	Private Function ComputeSpecificRollover() As RefundAdjustment
		CreatePayees()
		'create class object and Datatable
		Dim refAdjust As RefundAdjustment = New RefundAdjustment()
		'Start*********'BS:2011.10.20-YRS 5.0-1451************After Rollover Compute Payee1,Payee2 Datatable***********************************
		'Decalre payee1 payee2 Variable for compute payee1,payee2 datatable after rollover
		Dim l_Payee1DataTable As DataTable
		Dim l_Payee2DataTable As DataTable
		Dim l_Payee1TempDataTable As DataTable
		Dim l_Payee2DataRow As DataRow
        Dim l_SumTaxable As Decimal
		Dim l_SumNonTaxable As Decimal
		Dim l_SumAccountBalance As Decimal
		Dim l_RequestedAccount As Decimal
		Dim l_NumPercentageFactorofMoneyRollOver As Double = 0.0
		Dim NumRequestedAmountPartialRollOver As Decimal
        Dim Total_Refund As Decimal
        Dim AdjustedMoney As Decimal
		Dim maxAmountAccountRowIndex As Integer = 0
        Dim maxAmountAccountRowValue As Integer = 0
		Try

			'Assign rollover Amount into decimal variable
			NumRequestedAmountPartialRollOver = Math.Round(Convert.ToDecimal(TextBoxPayeeAmt.Text), 2)
			'Copy current session of payee1 into l_Payee1DataTable before rollover
			'BS:2011.11.01:reopen issue YRS 5.0-1451
			'l_Payee1DataTable = CopyTable(DirectCast(Session("R_Current"), DataTable), "Current")
            l_Payee1DataTable = CopyTable(DirectCast(Session("Payee1DataTable"), DataTable), "Payee1DataTable")
            'Start-SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only. Implemented new logic based on YRS-AT-2501.
            If HelperFunctions.isNonEmpty(l_Payee1DataTable) Then
                Dim dblTaxableProrateFactor As Decimal = 0
                Dim dblNonTaxableProrateFactor As Decimal = 0
                Dim dblRemainingBalfterTaxable As Decimal = 0
                Dim dblRequestedTaxableAmount As Decimal = 0
                Dim dblRequestedNonTaxableAmount As Decimal = 0

                'If Not l_Payee1DataTable Is Nothing Then
                l_SumTaxable = IIf(l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0"))
                l_SumNonTaxable = IIf(l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0"))
                Total_Refund = l_SumTaxable + l_SumNonTaxable
                'here we will validate to rollover amount will not exceed from total refund Amount
                If (NumRequestedAmountPartialRollOver > Total_Refund) Then
                    TextBoxPayeeAmt.Text = 0.0
                    refAdjust.ErrorMsg = "Requested Amount Should be less than equal to Total Refund."
                    Return refAdjust
                    Exit Function
                End If
                'here we will calculate common factor amount which will br rollover from individual Account
                If NumRequestedAmountPartialRollOver <= l_SumTaxable Then
                    If (NumRequestedAmountPartialRollOver > 0) Then 'condition applied to avoid devide by zero exception when rollover option is not selected
                        dblTaxableProrateFactor = l_SumTaxable / NumRequestedAmountPartialRollOver
                    End If
                Else
                    dblRemainingBalfterTaxable = NumRequestedAmountPartialRollOver - l_SumTaxable
                    If (NumRequestedAmountPartialRollOver - dblRemainingBalfterTaxable) > 0 Then
                        dblTaxableProrateFactor = l_SumTaxable / (NumRequestedAmountPartialRollOver - dblRemainingBalfterTaxable)
                    Else
                        dblTaxableProrateFactor = 0
                    End If
                    If (dblRemainingBalfterTaxable > 0) Then
                        dblNonTaxableProrateFactor = l_SumNonTaxable / dblRemainingBalfterTaxable
                    Else
                        dblNonTaxableProrateFactor = 0
                    End If
                End If

                l_Payee1TempDataTable = l_Payee1DataTable.Clone
                'here we create clone of payee1 datatable for compute payee2 datatable
                If l_Payee2DataTable Is Nothing Then
                    l_Payee2DataTable = l_Payee1DataTable.Clone
                End If
                l_Payee2DataTable.Rows.Clear()
                'here we are creating Payee1 and Payee2 Datatable after deducting rollover amount as per Account
                For i As Int16 = 0 To l_Payee1DataTable.Rows.Count - 1
                    Dim l_DataRow As DataRow = l_Payee1DataTable.Rows(i)
                    l_Payee1TempDataTable.ImportRow(l_DataRow)
                    l_Payee2DataRow = l_Payee2DataTable.NewRow

                    If NumRequestedAmountPartialRollOver <= l_SumTaxable Then
                        dblRequestedTaxableAmount = IIf(l_DataRow.IsNull("Taxable"), 0, l_DataRow("Taxable"))
                    Else
                        dblRequestedTaxableAmount = IIf(l_DataRow.IsNull("Taxable"), 0, l_DataRow("Taxable"))
                        dblRequestedNonTaxableAmount = IIf(l_DataRow.IsNull("NonTaxable"), 0, l_DataRow("NonTaxable"))
                    End If
                    If dblRequestedNonTaxableAmount > 0 Then
                        If (dblNonTaxableProrateFactor > 0) Then
                            dblRequestedNonTaxableAmount = dblRequestedNonTaxableAmount / dblNonTaxableProrateFactor
                        Else
                            dblRequestedNonTaxableAmount = 0
                        End If
                    Else
                        dblRequestedNonTaxableAmount = 0
                    End If

                    If (dblTaxableProrateFactor > 0) Then
                        l_RequestedAccount = Math.Round(dblRequestedTaxableAmount / dblTaxableProrateFactor, 2)
                    Else
                        l_RequestedAccount = 0
                    End If

                    If (i = l_Payee1DataTable.Rows.Count - 1) Then
                        l_RequestedAccount = Math.Round(NumRequestedAmountPartialRollOver - (AdjustedMoney + dblRequestedNonTaxableAmount), 2)
                    End If

                    AdjustedMoney += l_RequestedAccount 'Track how much money we have paid to Payee2

                    l_Payee2DataRow("AcctType") = l_DataRow("AcctType")

                    If NumRequestedAmountPartialRollOver <= l_SumTaxable Then
                        l_Payee2DataRow("Taxable") = l_RequestedAccount
                        l_DataRow("Taxable") = Convert.ToDecimal(l_DataRow("Taxable")) - Convert.ToDecimal(l_Payee2DataRow("Taxable"))
                        l_Payee2DataRow("NonTaxable") = "0.00"
                    Else
                        l_Payee2DataRow("Taxable") = l_RequestedAccount
                        l_Payee2DataRow("NonTaxable") = Math.Round(dblRequestedNonTaxableAmount, 2)
                        l_DataRow("Taxable") = "0.00"
                        l_DataRow("NonTaxable") = Math.Round(IIf(Convert.ToDecimal(l_DataRow("NonTaxable")) = 0, "0.00", Math.Round(l_DataRow("NonTaxable"), 2)) - dblRequestedNonTaxableAmount, 2)
                        AdjustedMoney += dblRequestedNonTaxableAmount
                        dblRequestedNonTaxableAmount = 0.0
                    End If

                    'here we will assiging Acctype,computed taxable,Nontaxable then add into payee2 datatable,
                    l_Payee2DataRow("TaxRate") = "0.00"
                    l_Payee2DataRow("Tax") = "0.00"
                    l_Payee2DataRow("Payee") = l_DataRow("Payee")
                    l_Payee2DataRow("FundedDate") = l_DataRow("FundedDate")
                    l_Payee2DataTable.Rows.Add(l_Payee2DataRow)
                Next
            End If

            'Start-SR/2015.09.30:YRS-AT-2501- commented below line of code
            'direct assign session of payee2 datatable into variable here session will update with changes of payee2 datatable
            'If HelperFunctions.isNonEmpty(l_Payee1DataTable) Then
            '	'If Not l_Payee1DataTable Is Nothing Then
            '	l_SumTaxable = IIf(l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0"))
            '	l_SumNonTaxable = IIf(l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0"))
            '	Total_Refund = l_SumTaxable + l_SumNonTaxable
            '	'here we will validate to rollover amount will not exceed from total refund Amount
            '	If (NumRequestedAmountPartialRollOver > Total_Refund) Then
            '		TextBoxPayeeAmt.Text = 0.0
            '		refAdjust.ErrorMsg = "Requested Amount Should be less than equal to Total Refund."
            '		Return refAdjust
            '		Exit Function
            '	End If
            '	'here we will calculate common factor amount which will br rollover from individual Account
            '	If (l_SumTaxable + l_SumNonTaxable) <> 0 Then
            '		l_NumPercentageFactorofMoneyRollOver = NumRequestedAmountPartialRollOver / (l_SumTaxable + l_SumNonTaxable)
            '	End If
            '	l_Payee1TempDataTable = l_Payee1DataTable.Clone
            '	'here we create clone of payee1 datatable for compute payee2 datatable
            '	If l_Payee2DataTable Is Nothing Then
            '		l_Payee2DataTable = l_Payee1DataTable.Clone
            '	End If
            '	l_Payee2DataTable.Rows.Clear()
            '	'here we are creating Payee1 and Payee2 Datatable after deducting rollover amount as per Account
            '	For i As Int16 = 0 To l_Payee1DataTable.Rows.Count - 1
            '		Dim l_DataRow As DataRow = l_Payee1DataTable.Rows(i)
            '		l_Payee1TempDataTable.ImportRow(l_DataRow)
            '		l_Payee2DataRow = l_Payee2DataTable.NewRow

            '		l_SumAccountBalance = IIf(l_DataRow.IsNull("Taxable"), 0, l_DataRow("Taxable")) + IIf(l_DataRow.IsNull("NonTaxable"), 0, l_DataRow("NonTaxable"))

            '		'Identify and track the highest valued account row and its value
            '		If (l_SumAccountBalance > maxAmountAccountRowValue) Then
            '			maxAmountAccountRowValue = l_SumAccountBalance
            '			maxAmountAccountRowIndex = i
            '		End If

            '		'here compute requested amount which will be rollover acountwise 
            '		l_RequestedAccount = Math.Round(l_SumAccountBalance * l_NumPercentageFactorofMoneyRollOver, 2)
            '		AdjustedMoney += l_RequestedAccount	'Track how much money we have paid to Payee2
            '		'here we will assiging Acctype,computed taxable,Nontaxable then add into payee2 datatable,
            '		l_Payee2DataRow("AcctType") = l_DataRow("AcctType")
            '		l_Payee2DataRow("NonTaxable") = Math.Round(IIf(l_DataRow.IsNull("NonTaxable"), 0, l_DataRow("NonTaxable")) * l_NumPercentageFactorofMoneyRollOver, 2)
            '		l_Payee2DataRow("Taxable") = Math.Round(l_RequestedAccount - Convert.ToDecimal(l_Payee2DataRow("NonTaxable")), 2)
            '		l_DataRow("Taxable") = Math.Round(Convert.ToDecimal(l_DataRow("Taxable")) - Convert.ToDecimal(l_Payee2DataRow("Taxable")), 2)
            '		l_DataRow("NonTaxable") = Math.Round(Convert.ToDecimal(l_DataRow("NonTaxable")) - Convert.ToDecimal(l_Payee2DataRow("NonTaxable")), 2)
            '		l_Payee2DataTable.Rows.Add(l_Payee2DataRow)


            '	Next
            'End If
            ''Need code here to adjust any rounding issues from the maximum record
            'Adjust the difference in the taxable and nontaxable component of the highest valued account
            'End-SR/2015.09.30:YRS-AT-2501- commented below line of code
            'End-SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only. Implemented new logic based on YRS-AT-2501.

            If (AdjustedMoney <> NumRequestedAmountPartialRollOver) Then
                Dim Diff As Decimal = AdjustedMoney - NumRequestedAmountPartialRollOver
                Dim l_Payee1Row As DataRow = l_Payee1DataTable.Rows(maxAmountAccountRowIndex)
                Dim l_Payee2Row As DataRow = l_Payee2DataTable.Rows(maxAmountAccountRowIndex)
                'BS:2011.11.02:YRS 5.0-1451-reopenIssue-adjust nontaxable component
                If l_Payee1Row.IsNull("Taxable") = False AndAlso l_Payee1Row("Taxable") + Diff >= 0 Then
                    l_Payee1Row("Taxable") = l_Payee1Row("Taxable") + Diff
                    l_Payee2Row("Taxable") = l_Payee2Row("Taxable") - Diff
                ElseIf l_Payee1Row.IsNull("NonTaxable") = False AndAlso l_Payee1Row("NonTaxable") + Diff >= 0 Then
                    l_Payee1Row("NonTaxable") = l_Payee1Row("NonTaxable") + Diff
                    l_Payee2Row("NonTaxable") = l_Payee2Row("NonTaxable") - Diff
                Else
                    refAdjust.ErrorMsg = "Unable to adjust the difference in the taxable and nontaxable component"
                    Return refAdjust
                    Exit Function
                End If

            End If
            'here we have assign payee1 payee2 session from rollover payee1,payee2 datatable
            Session("Payee1DataTable") = l_Payee1DataTable
            Session("Payee2DataTable") = l_Payee2DataTable


            'End*********************After Rollover Compute Payee1,Payee2 Datatable***********************************
            'Start*************Compute Total Taxable NonTaxable,Tax,Taxrate,Net Amount of MRD,Payee1,Payee2 Textboxes******************
            Dim l_DataTable, l_DataTable2, l_MRDDataTable As DataTable
            Dim l_DecimalReqRolloverAmt As Decimal
            'here we are assigning Computed Session datatables into datatables
            l_DataTable = CType(Session("Payee1DataTable"), DataTable)
            l_DataTable2 = CType(Session("Payee2DataTable"), DataTable)
            l_MRDDataTable = CType(Session("R_MRDAcctBalance"), DataTable)
            'here we are assigning rollover textbox value into rollover variable
            If (TextBoxPayeeAmt.Text = String.Empty) Then
                refAdjust.ReqRolloverAmt = 0.0
            Else
                refAdjust.ReqRolloverAmt = Convert.ToDecimal(TextBoxPayeeAmt.Text)
            End If
            'Compute Total MRD Taxable,Nontaxable,Tax,Taxrate,Net Amount  Detail
            If HelperFunctions.isNonEmpty(l_MRDDataTable) Then
                refAdjust.MRDTaxable = Math.Round(CType(l_MRDDataTable.Compute("SUM(Taxable)", ""), Decimal), 2)
                refAdjust.NonMRDTaxable = Math.Round(CType(l_MRDDataTable.Compute("SUM(NonTaxable)", ""), Decimal), 2)
                Dim MrdTaxRate As Decimal
                If (TextBoxMRDRate.Text <> Nothing AndAlso TextBoxMRDRate.Text <> String.Empty) Then
                    MrdTaxRate = Convert.ToDecimal(TextBoxMRDRate.Text)
                Else
                    MrdTaxRate = 10.0
                End If
                refAdjust.MRDTaxRate = MrdTaxRate
                refAdjust.MRDTax = Math.Round((MrdTaxRate / 100) * refAdjust.MRDTaxable, 2)
                refAdjust.MRDTax = Math.Round(MrdTaxRate / 100 * refAdjust.MRDTaxable, 2)
                refAdjust.MrdNet = Math.Round(refAdjust.MRDTaxable - refAdjust.MRDTax, 2)
            End If
            'Compute Total Payee1 Taxable,Nontaxable,Tax,Taxrate,Net Amount  Detail
            If HelperFunctions.isNonEmpty(l_DataTable) Then
                refAdjust.Payee1Taxable = Math.Round(CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal), 2)
                refAdjust.Payee1NonTaxable = Math.Round(CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal), 2)
                Dim Payee1taxRate As Decimal
                If (TextBoxRate.Text <> String.Empty) Then
                    Payee1taxRate = Convert.ToDecimal(TextBoxRate.Text)
                Else
                    'Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to Payee1taxRate
                    'Payee1taxRate = 20.0
                    Payee1taxRate = Me.TaxRate
                    'End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to Payee1taxRate
                End If
                If Payee1taxRate = 0 Then
                    refAdjust.Payee1Tax = 0.0
                Else
                    refAdjust.Payee1Tax = Math.Round(((Payee1taxRate / 100) * refAdjust.Payee1Taxable), 2, MidpointRounding.AwayFromZero) 'SR | 2016.04.28 | YRS-AT-2142 - Error in void reissue when tax rate is changed. 
                End If

                refAdjust.Payee1Net = Math.Round((refAdjust.Payee1Taxable + refAdjust.Payee1NonTaxable) - refAdjust.Payee1Tax, 2)
            End If
            'Compute Total Payee2 Taxable,Nontaxable,Tax,Taxrate,Net Amount 
            If HelperFunctions.isNonEmpty(l_DataTable2) Then
                refAdjust.Payee2Taxable = Math.Round(CType(l_DataTable2.Compute("SUM (Taxable)", ""), Decimal), 2)
                refAdjust.Payee2NonTaxable = Math.Round(CType(l_DataTable2.Compute("SUM (NonTaxable)", ""), Decimal), 2)
                refAdjust.Payee2Net = Math.Round(refAdjust.Payee2Taxable + refAdjust.Payee2NonTaxable, 2)
            End If
            'Apply Deduction on Payee1,Mrd,Payee2
            GetDedutions()
            refAdjust.deductionBalance = DeductionsAmount
            If Me.DeductionsAmount < refAdjust.Payee1Net Then
                refAdjust.Payee1Deduction = DeductionsAmount
                refAdjust.Payee1Net = Math.Round(refAdjust.Payee1Net - DeductionsAmount, 2)
                refAdjust.deductionBalance = 0
            ElseIf refAdjust.deductionBalance < refAdjust.MrdNet Then
                refAdjust.Payee1Deduction = 0
                refAdjust.MrdNet = Math.Round(refAdjust.MrdNet - DeductionsAmount, 2)
                refAdjust.MrdDeduction = refAdjust.deductionBalance
                refAdjust.deductionBalance = 0
            ElseIf refAdjust.deductionBalance < refAdjust.Payee2Net Then
                refAdjust.Payee2Deduction = refAdjust.deductionBalance
                refAdjust.Payee2Net = refAdjust.Payee2Net - refAdjust.deductionBalance
                refAdjust.deductionBalance = 0
            Else
                refAdjust.ErrorMsg = "Unable to adjust deductions from any record"
                Return refAdjust
                Exit Function
            End If
            'BS:2011.10.31:YRS 5.0-1451: reopen issue
            If ((refAdjust.Payee1Net < 0.01 AndAlso (refAdjust.Payee1Taxable > 0.0 OrElse refAdjust.Payee1NonTaxable > 0.0)) OrElse (refAdjust.Payee2Net < 0.01 AndAlso (refAdjust.Payee2Taxable > 0.0 OrElse refAdjust.Payee2NonTaxable > 0.0)) OrElse (refAdjust.MrdNet < 0.01 AndAlso (refAdjust.NonMRDTaxable > 0.0 OrElse refAdjust.MRDTaxable > 0.0))) Then
                Me.DeductionsAmount = 0
                refAdjust.Payee1Deduction = DeductionsAmount
                refAdjust.Payee1Deduction = DeductionsAmount
                refAdjust.ErrorMsg = "Net amount can not be less than $0.01."
                Return refAdjust
                Exit Function
            End If
            'End******'BS:2011.10.20-YRS 5.0-1451*******Compute Total Taxable NonTaxable,Tax,Taxrate,Net Amount of MRD,Payee1,Payee2 Textboxes******************
            Return refAdjust ' String.Empty
        Catch ex As Exception
            Throw ex
        End Try
	End Function
	Public Class ClientCompute
		Public TextBoxRollOverAmt As Decimal = 0.0
		'Payee1
		Public TextBoxTaxablePayee1 As Decimal = 0.0
		Public TextBoxNonTaxablePayee1 As Decimal = 0.0
		Public TextBoxTaxPayee1 As Decimal = 0.0
		Public TextBoxNetPayee1 As Decimal = 0.0
		Public TextBoxDeductionPayee1 As Decimal = 0.0
		'Payee2
		Public TextBoxTaxablePayee2 As Decimal = 0.0
		Public TextBoxNonTaxablePayee2 As Decimal = 0.0
		Public TextBoxTaxPayee2 As Decimal = 0.0
		Public TextBoxNetPayee2 As Decimal = 0.0
		Public TextBoxDeductionPayee2 As Decimal = 0.0
	End Class
	'BS:2011.10.20-YRS 5.0-1451 - here verify all refund related calculation from client side
	Private Function VerifyRefundAdjustment()
		'here we are Assigning ComputeSpecificRollover() into Class object
		Dim refAdjust As RefundAdjustment = ComputeSpecificRollover()
		'here we are returning error if rollover amount exceed from total refundable amount
		If refAdjust.ErrorMsg.Trim <> Nothing OrElse refAdjust.ErrorMsg.Trim <> String.Empty Then
			Return refAdjust.ErrorMsg
			Exit Function
		End If
		'here declare client side varibale
		Dim client As RefundAdjustment = New RefundAdjustment()

		'declare variable for client side textbox value

		If (TextBoxPayeeAmt.Text <> String.Empty) Then
			client.ReqRolloverAmt = Decimal.Parse(TextBoxPayeeAmt.Text, NumberStyles.AllowDecimalPoint)
		End If
		'Start Assignmnet Client side Payee1,Payee2
		If (TextBoxTaxable.Text <> String.Empty) Then
			client.Payee1Taxable = Decimal.Parse(TextBoxTaxable.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxNonTaxable.Text <> String.Empty) Then
			client.Payee1NonTaxable = Decimal.Parse(TextBoxNonTaxable.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxTax.Text <> String.Empty) Then
			client.Payee1Tax = Decimal.Parse(TextBoxTax.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxNet.Text <> String.Empty) Then
			client.Payee1Net = Decimal.Parse(TextBoxNet.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxDeductions.Text <> String.Empty) Then
			client.Payee1Deduction = Decimal.Parse(TextBoxDeductions.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxTaxable1.Text <> Nothing OrElse TextBoxTaxable1.Text <> String.Empty) Then
			client.Payee2Taxable = Decimal.Parse(TextBoxTaxable1.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxNonTaxable1.Text <> String.Empty) Then
			client.Payee2NonTaxable = Decimal.Parse(TextBoxNonTaxable1.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxTax1.Text <> String.Empty) Then
			client.Payee2Tax = Decimal.Parse(TextBoxTax1.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxNet1.Text <> String.Empty) Then
			client.Payee2Net = Decimal.Parse(TextBoxNet1.Text, NumberStyles.AllowDecimalPoint)
		End If
		If (TextBoxDeductions1.Text <> String.Empty) Then
			client.Payee2Deduction = Decimal.Parse(TextBoxDeductions1.Text, NumberStyles.AllowDecimalPoint)
		End If
		'End Assignmnet Client side Payee1,Payee2
		'Compare Client Side and Server Side Taxable,NonTaxable,Tax,TaxRate,NetAmount
		If (client.Payee1Taxable <> refAdjust.Payee1Taxable OrElse client.Payee1NonTaxable <> refAdjust.Payee1NonTaxable OrElse client.Payee2Taxable <> refAdjust.Payee2Taxable OrElse client.Payee2NonTaxable <> refAdjust.Payee2NonTaxable OrElse client.Payee1Tax <> refAdjust.Payee1Tax OrElse client.Payee2Tax <> refAdjust.Payee2Tax OrElse client.Payee1Net <> refAdjust.Payee1Net OrElse client.Payee2Net <> refAdjust.Payee2Net OrElse client.ReqRolloverAmt <> refAdjust.ReqRolloverAmt) Then
			Return "There has been an error in computing the values. Please verify the values on the screen or contact support."
			HelperFunctions.LogMessage( _
			  String.Format("Requested Rollover of {0} out of {1}. Expected Payee1: T={2}, NT={3}. Actual Payee1: T={4}, NT={5}.Expected Payee2: T={6}, NT={7}. Actual Payee2: T={8}, NT={9}.", _
			  refAdjust.ReqRolloverAmt, refAdjust.Payee1Taxable + refAdjust.Payee1NonTaxable, refAdjust.Payee1Taxable, refAdjust.Payee1NonTaxable, client.Payee1Taxable, client.Payee1NonTaxable, refAdjust.Payee2Taxable, refAdjust.Payee2NonTaxable, client.Payee2Taxable, client.Payee2NonTaxable))
			Return False
		End If

	End Function
	'BS:2011.10.11-YRS 5.0-1451--On prerender event we have call initializecontrol()for intialize textboxes control 
	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		If (TabStripWithdrawalReissue.SelectedIndex = 1) Then
			CreatePayees()
			CreatePayee1Payee2Mrd()
		End If
	End Sub

	
End Class

