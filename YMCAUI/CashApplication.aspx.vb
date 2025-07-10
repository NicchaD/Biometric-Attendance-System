'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCa-YRS
' FileName			:	CashApplication.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	10/24/2005 5:03:26 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
' Cache-Session     :   Vipul 03Feb06 
'*******************************************************************************
'Modified by Shubhrata Tripathi Dec 22nd 06 YREN 3006 To maintain transactions
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Desription
'********************************************************************************************************************************
'Vipul              03Feb06         Cache-Session
'Shubhrata Tripathi Dec 22nd 06     YREN 3006 To maintain transactions
'Ashutosh Patil     20-Feb-2007     ImageButton Intrest--If no records found in that case ther interest will not apply to the concrened YMCA.                    
'Ashutosh Patil     22-May-2007     Change in Email Functionality
'Mohammed Hafiz     26/07/2007      for changing the mail subject & content text as suggested by Mark Posey in mail dated : 26/07/2007
'Ashish Srivastava  12-May-2008     Added funded date date controm in transmittalgrid and logic of UEIN & ServiceUpade
'Ashish Srivastava  25-Sep-2008     Remove date control from transmittal grid and put out side from grid
'Ashish Srivastava  02-Dec-2008     Funded date should be within current business month
'Ashish Srivastava  22-May-2009     rounding issue for Ammount due 
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'prasad jadhav		2012.02.28		BT-999, YRS 5.0-1547 - loan payoff email not reporting multiple people
'Anudeep            2012.12.24      YRS 5.0-1717:Issue email when loan paid off 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Bala               2015.12.14      YRS-AT-2642: YRS enh: Remove ssno from sending loan defalulted email.
'Anudeep A          2016.04.12      YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Pramod P. Pokale   2016.07.27      YRS-AT-3058 - YRS bug: timing issue Loan Utility - defaulting a Loan and then transmittal w/payment, Auto closed as successfully paid off? (trackIt 26945) 
'********************************************************************************************************************************

Imports System.Text
Public Class CashApplication
	Inherits System.Web.UI.Page
	'below line is Added by Neeraj for issue id YRS 5.0-940 
	Dim strFormName As String = New String("CashApplication.aspx")
	'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
	Protected WithEvents PlaceHolderMessageBox As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents TabStripCashApplication As Microsoft.Web.UI.WebControls.TabStrip
	Protected WithEvents MultiPageCashApplication As Microsoft.Web.UI.WebControls.MultiPage
	Protected WithEvents Menu1 As skmMenu.Menu
	Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
	Protected WithEvents LabelRecordNotFound As System.Web.UI.WebControls.Label
	Protected WithEvents DataGridYmca As System.Web.UI.WebControls.DataGrid
	Protected WithEvents LabelYmcaNo As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxYmcaNo As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelYmcaName As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxYmcaName As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelState As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
	Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
	Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
	Protected WithEvents LabelCredits As System.Web.UI.WebControls.Label
	Protected WithEvents TextBoxCredits As System.Web.UI.WebControls.TextBox
	Protected WithEvents ImageButtonCredits As System.Web.UI.WebControls.ImageButton
	Protected WithEvents DataGridTransmit As System.Web.UI.WebControls.DataGrid
	Protected WithEvents TextBoxTotalAmount As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxTotalReceipts As System.Web.UI.WebControls.TextBox
	Protected WithEvents TextBoxTotalCredits As System.Web.UI.WebControls.TextBox

	Protected WithEvents TextBoxTotalBalance As System.Web.UI.WebControls.TextBox
	Protected WithEvents DataGridReceipts As System.Web.UI.WebControls.DataGrid
	Protected WithEvents ImageButtonReceipts As System.Web.UI.WebControls.ImageButton


	Protected WithEvents ButtonPayItem As System.Web.UI.WebControls.Button
	Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
	Protected WithEvents DateusercontrolFundedDate As YMCAUI.DateUserControl
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            'Put user code to initialize the page here
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            Me.TextBoxYmcaNo.Attributes.Add("onBlur", "javascript:FormatYMCANo();")
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("CashApplication" + "Page_Load", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
        End Try
    End Sub

    Private Sub TabStripCashApplication_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripCashApplication.SelectedIndexChange
        Try
            If Me.DataGridYmca.SelectedIndex <> -1 Then
                Me.MultiPageCashApplication.SelectedIndex = Me.TabStripCashApplication.SelectedIndex
            Else
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select a Ymca.", MessageBoxButtons.Stop)
                Me.TabStripCashApplication.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "TabStripCashApplication_SelectedIndexChange", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            Dim l_string_YmcaNo As String
            Me.DataGridYmca.SelectedIndex = -1
            LabelHdr.Text = ""
            l_string_YmcaNo = Me.TextBoxYmcaNo.Text.Trim
            If Not l_string_YmcaNo = "" Then
                If l_string_YmcaNo.Length < 6 Then
                    Dim i As Integer
                    For i = 0 To (5 - l_string_YmcaNo.Length)
                        l_string_YmcaNo = "0" + l_string_YmcaNo
                    Next
                End If
                Me.TextBoxYmcaNo.Text = l_string_YmcaNo.Trim
            End If

            Dim l_dataset_Ymcas As DataSet
            l_dataset_Ymcas = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmca(l_string_YmcaNo, TextBoxYmcaName.Text.Trim(), TextBoxCity.Text.Trim, TextBoxState.Text.ToUpper.Trim)
            If Not l_dataset_Ymcas Is Nothing Then
                If Not l_dataset_Ymcas.Tables("Ymcas") Is Nothing Then
                    If l_dataset_Ymcas.Tables("Ymcas").Rows.Count > 0 Then
                        Me.DataGridYmca.DataSource = l_dataset_Ymcas.Tables("Ymcas")
                        Me.DataGridYmca.DataBind()
                        Me.DataGridYmca.Visible = True
                        Me.LabelRecordNotFound.Visible = False
                    Else
                        Me.DataGridYmca.Visible = False
                        Me.LabelRecordNotFound.Visible = True
                    End If
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "ButtonFind_Click", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            TextBoxCity.Text = ""
            TextBoxState.Text = ""
            TextBoxYmcaNo.Text = ""
            TextBoxYmcaName.Text = ""
            Me.DataGridYmca.Visible = False
            Me.LabelRecordNotFound.Visible = False
        Catch ex As Exception
            HelperFunctions.LogException("CashApplication" + "ButtonClear_Click", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
        End Try
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Response.Redirect("MainWebForm.aspx")
    End Sub

    Private Sub DataGridYmca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridYmca.SelectedIndexChanged
        Try
            Dim l_string_YmcaId As String
            'Dim l_dataset_YmcaTransmittals As DataSet
            'Dim l_dataset_YmcaReceipts As DataSet
            'Dim l_dataset_YmcaInterests As DataSet

            l_string_YmcaId = Me.DataGridYmca.SelectedItem.Cells(1).Text.Trim
            LabelHdr.Text = "--" + Me.DataGridYmca.SelectedItem.Cells(2).Text.Trim
            Session("YmcaId") = l_string_YmcaId
            Dim i As Integer
            For i = 0 To Me.DataGridYmca.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridYmca.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridYmca.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            Next

            PopulateValues()

            ''Calling BO Classes to get the data for Transmittals,Receipts,Interests

            'l_dataset_YmcaTransmittals = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaTransmittals(l_string_YmcaId)
            'l_dataset_YmcaReceipts = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaReceipts(l_string_YmcaId)
            'l_dataset_YmcaInterests = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaInterest(l_string_YmcaId)



            'If Not l_dataset_YmcaTransmittals Is Nothing Then
            '    If l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows.Count > 0 Then
            '        'Creating the table Transmittals from the YmcaTransmittls datatable
            '        Dim l_dt_Transmittals As New DataTable
            '        l_dt_Transmittals.Columns.Add("Slctd")
            '        l_dt_Transmittals.Columns.Add("UniqueId")
            '        l_dt_Transmittals.Columns.Add("YmcaId")
            '        l_dt_Transmittals.Columns.Add("TransmittalNo")
            '        l_dt_Transmittals.Columns.Add("TransmittalSourceCode")
            '        l_dt_Transmittals.Columns.Add("TransmittalDate")
            '        l_dt_Transmittals.Columns.Add("AmtDue")
            '        l_dt_Transmittals.Columns.Add("AmtPaid")
            '        l_dt_Transmittals.Columns.Add("AmtCredit")
            '        l_dt_Transmittals.Columns.Add("TotAppliedRcpts")
            '        l_dt_Transmittals.Columns.Add("TotAppliedCredit")
            '        l_dt_Transmittals.Columns.Add("TotAppliedInterest")
            '        l_dt_Transmittals.Columns.Add("OrgAppliedCredit")
            '        l_dt_Transmittals.Columns.Add("OrgAppliedRcpts")
            '        l_dt_Transmittals.Columns.Add("OrgAppliedInterest")
            '        l_dt_Transmittals.Columns.Add("Balance")

            '        Dim i_counter As Integer
            '        For i_counter = 0 To l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows.Count - 1
            '            l_dt_Transmittals.ImportRow(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter))
            '        Next
            '        For i_counter = 0 To l_dt_Transmittals.Rows.Count - 1
            '            l_dt_Transmittals.Rows(i_counter).Item("AmtDue") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("AmtDue")), 0, l_dt_Transmittals.Rows(i_counter).Item("AmtDue"))
            '            l_dt_Transmittals.Rows(i_counter).Item("AmtPaid") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("AmtPaid")), 0, l_dt_Transmittals.Rows(i_counter).Item("AmtPaid"))
            '            l_dt_Transmittals.Rows(i_counter).Item("AmtCredit") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("AmtCredit")), 0, l_dt_Transmittals.Rows(i_counter).Item("AmtCredit"))
            '            l_dt_Transmittals.Rows(i_counter).Item("Balance") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("Balance")), 0, l_dt_Transmittals.Rows(i_counter).Item("Balance"))
            '            l_dt_Transmittals.Rows(i_counter).Item("TotAppliedCredit") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("TotAppliedCredit")), 0, l_dt_Transmittals.Rows(i_counter).Item("TotAppliedCredit"))
            '            l_dt_Transmittals.Rows(i_counter).Item("TotAppliedRcpts") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("TotAppliedRcpts")), 0, l_dt_Transmittals.Rows(i_counter).Item("TotAppliedRcpts"))
            '            l_dt_Transmittals.Rows(i_counter).Item("TotAppliedInterest") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("TotAppliedInterest")), 0, l_dt_Transmittals.Rows(i_counter).Item("TotAppliedInterest"))
            '            l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit")), 0, l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit"))
            '            l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts")), 0, l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts"))
            '            l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest")), 0, l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest"))
            '        Next
            '        Dim l_double_AmtDue As Double
            '        Dim l_double_AmtPaid As Double
            '        Dim l_double_AmtRcpts As Double
            '        Dim l_double_AmtCredit As Double
            '        Dim l_double_AmtBalance As Double
            '        Dim l_double_AmtInterest As Double
            '        For i_counter = 0 To l_dt_Transmittals.Rows.Count - 1
            '            l_dt_Transmittals.Rows(i_counter).Item("Slctd") = False
            '            l_double_AmtDue = l_double_AmtDue + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("AmtDue"))
            '            l_double_AmtPaid = l_double_AmtPaid + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts")) + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit")) + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest"))
            '            l_double_AmtRcpts = l_double_AmtRcpts + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts"))
            '            l_double_AmtCredit = l_double_AmtCredit + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit"))
            '            l_double_AmtBalance = l_double_AmtBalance + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("Balance"))
            '            l_double_AmtInterest = l_double_AmtInterest + Convert.ToDouble(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest"))
            '        Next


            '        TextBoxTotalAmount.Text = l_double_AmtDue
            '        TextBoxTotalReceipts.Text = l_double_AmtRcpts
            '        TextBoxTotalCredits.Text = l_double_AmtCredit
            '        TextBoxTotalInterest.Text = l_double_AmtInterest
            '        TextBoxTotalBalance.Text = l_double_AmtBalance
            '        Session("AmtPaid") = l_double_AmtPaid
            '        Session("RcptsSlctd") = False
            '        Session("CreditsSlctd") = False
            '        Session("InterestsSlctd") = False
            '        Me.ButtonPayItem.Enabled = False
            '        Me.DataGridTransmit.DataSource = l_dt_Transmittals
            '        Me.DataGridTransmit.DataBind()
            '        ViewState("Transmittals") = l_dt_Transmittals

            '        'Getting data into the credit Textbox
            '        Dim l_double_Credits As Double

            '        l_double_Credits = Convert.ToDouble(YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaCredit(l_string_YmcaId))
            '        TextBoxCredits.Text = l_double_Credits
            '        Session("amtStartCredit") = l_double_Credits
            '        Session("CreditBalance") = l_double_Credits

            '    End If
            'End If

            ''Binding The Receipts grid
            'If Not l_dataset_YmcaReceipts Is Nothing Then
            '    If l_dataset_YmcaReceipts.Tables("YmcaReceipts").Rows.Count > 0 Then
            '        Me.DataGridReceipts.DataSource = l_dataset_YmcaReceipts
            '        Me.DataGridReceipts.DataBind()
            '        ViewState("Receipts") = l_dataset_YmcaReceipts.Tables(0)
            '    End If
            'End If

            ''Binding the Interest Grid
            'If Not l_dataset_YmcaInterests Is Nothing Then
            '    If Not l_dataset_YmcaInterests.Tables("YmcaInterest").Rows.Count > 0 Then
            '        Me.DataGridInterest.DataSource = l_dataset_YmcaInterests
            '        Me.DataGridInterest.DataBind()
            '        ViewState("Interests") = l_dataset_YmcaInterests.Tables(0)
            '    End If
            'End If


            'TabStripCashApplication.SelectedIndex = 1
            'MultiPageCashApplication.SelectedIndex = 1
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "DataGridYmca_SelectedIndexChanged", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim ck1 As CheckBox = CType(sender, CheckBox)
            Dim dgItem As DataGridItem = CType(ck1.NamingContainer, DataGridItem)
            Dim i As Integer = dgItem.ItemIndex
            Dim l_double_TotalAmount As Double
            Dim l_double_TotalReceipts As Double
            Dim l_double_ReceiptDiff As Double
            Dim l_double_TotalCredits As Double
            Dim l_double_CreditdDiff As Double
            Dim l_double_TotalInterest As Double
            Dim l_double_InterestDiff As Double
            Dim l_double_TotalBalance As Double
            Dim l_double_AmtPaid As Double
            Dim l_double_StartCredit As Double
            Dim l_dt_transmittals As DataTable
            Dim l_dt_Orgtransmittals As DataTable
            ' Dim l_dt_Interests As DataTable
            Dim l_dt_Org As DataTable
            Dim l_dt_Receipts As DataTable
            l_double_TotalAmount = 0
            l_double_TotalReceipts = 0
            l_double_TotalCredits = 0
            l_double_TotalInterest = 0
            l_double_TotalBalance = 0
            If Not ViewState("Receipts") Is Nothing Then
                l_dt_Receipts = ViewState("Receipts")
            End If

            If Not Viewstate("Transmittals") Is Nothing Then
                l_dt_transmittals = Viewstate("Transmittals")
            End If
            'If Not Viewstate("Interests") Is Nothing Then
            '    l_dt_Interests = ViewState("Interests")
            'End If
            If ck1.Checked = True Then
                l_dt_transmittals.Rows(i).Item("Slctd") = True
            Else
                l_dt_transmittals.Rows(i).Item("Slctd") = False
            End If
            l_double_StartCredit = Convert.ToDecimal(Session("amtStartCredit1"))
            If ck1.Checked = False Then
                l_double_TotalAmount = Convert.ToDecimal(TextBoxTotalAmount.Text)
                l_double_TotalReceipts = Convert.ToDecimal(TextBoxTotalReceipts.Text)
                l_double_TotalCredits = Convert.ToDecimal(TextBoxTotalCredits.Text)
                'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                'l_double_TotalInterest = Convert.ToDecimal(TextBoxTotalInterest.Text)
                'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest ,End 
                l_double_TotalBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
                l_dt_Orgtransmittals = CType(Session("OriginalTramittals"), DataTable)

                If l_dt_transmittals.Rows(i).Item("TotAppliedCredit").ToString().Trim() <> l_dt_Orgtransmittals.Rows(i).Item("TotAppliedCredit").ToString().Trim() Then
                    l_double_CreditdDiff = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Orgtransmittals.Rows(i).Item("TotAppliedCredit"))
                    TextBoxTotalCredits.Text = Math.Round(l_double_TotalCredits, 2) - Math.Round(l_double_CreditdDiff, 2)

                    l_dt_transmittals.Rows(i).Item("TotAppliedCredit") = l_dt_Orgtransmittals.Rows(i).Item("TotAppliedCredit")
                    TextBoxCredits.Text = Convert.ToDouble(TextBoxCredits.Text) + l_double_CreditdDiff
                    TextBoxCredits.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxCredits.Text))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                    'l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotappliedInterest")))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                    'Added y Ashish ,Start
                    l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")))
                    'Added y Ashish ,End
                    ' TextBoxTotalCredits.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalCredits.Text))
                    TextBoxTotalCredits.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalCredits.Text))
                End If
                If l_dt_transmittals.Rows(i).Item("TotAppliedRcpts").ToString().Trim() <> l_dt_Orgtransmittals.Rows(i).Item("TotAppliedRcpts").ToString().Trim() Then
                    l_double_TotalBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
                    l_double_ReceiptDiff = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Orgtransmittals.Rows(i).Item("TotAppliedRcpts"))

                    TextBoxTotalReceipts.Text = Math.Round(l_double_TotalReceipts, 2) - Math.Round(l_double_ReceiptDiff, 2)
                    l_dt_transmittals.Rows(i).Item("TotAppliedRcpts") = l_dt_Orgtransmittals.Rows(i).Item("TotAppliedRcpts")
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                    'l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotappliedInterest")))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                    'Added y Ashish ,Start
                    l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")))
                    'Added y Ashish ,End
                    'TextBoxTotalBalance.Text = l_double_TotalBalance + l_double_ReceiptDiff
                    l_dt_Receipts.Rows(Me.DataGridReceipts.SelectedIndex).Item("Amount") = Convert.ToDecimal(l_dt_Receipts.Rows(Me.DataGridReceipts.SelectedIndex).Item("Amount")) + l_double_ReceiptDiff
                    'TextBoxTotalReceipts.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalReceipts.Text))
                    TextBoxTotalReceipts.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalReceipts.Text))
                End If
                'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest, Start
                'If l_dt_transmittals.Rows(i).Item("TotAppliedInterest").ToString().Trim() <> l_dt_Orgtransmittals.Rows(i).Item("TotAppliedInterest").ToString().Trim() Then
                '    l_double_TotalBalance = Convert.ToDecimal(TextBoxTotalBalance.Text) 'Imp for checking the total balance
                '    l_double_InterestDiff = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedInterest")) - Convert.ToDecimal(l_dt_Orgtransmittals.Rows(i).Item("TotAppliedInterest"))

                '    TextBoxTotalInterest.Text = Math.Round(l_double_TotalInterest, 2) - Math.Round(l_double_InterestDiff, 2)

                '    l_dt_transmittals.Rows(i).Item("TotAppliedInterest") = l_dt_Orgtransmittals.Rows(i).Item("TotAppliedInterest")
                '    l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotappliedInterest")))
                '    TextBoxTotalBalance.Text = l_double_TotalBalance + l_double_InterestDiff

                '    l_dt_Interests.Rows(Me.DataGridInterest.SelectedIndex).Item("Amount") = Convert.ToDecimal(l_dt_Interests.Rows(Me.DataGridInterest.SelectedIndex).Item("Amount")) + l_double_InterestDiff


                '    TextBoxTotalInterest.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalInterest.Text))

                '    TextBoxTotalInterest.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalInterest.Text))

                'End If


                'TextBoxTotalBalance.Text = l_double_TotalBalance + l_double_CreditdDiff + l_double_ReceiptDiff + l_double_InterestDiff
                'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                'Added by Ashish on 12 May,Start
                If TextBoxTotalReceipts.Text = "0.00" And TextBoxTotalCredits.Text = "0.00" Then
                    ButtonPayItem.Enabled = False
                Else
                    ButtonPayItem.Enabled = True
                End If
                'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                'If Not Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("Balance")) = 0 Then

                '    Dim dgItemTemp As DataGridItem = DataGridTransmit.Items(i)
                '    If Not dgItemTemp Is Nothing Then
                '        Dim dtUserControlFundedDate As DateUserControl
                '        dtUserControlFundedDate = CType(dgItemTemp.FindControl("DateusercontrolFundedDate"), DateUserControl)
                '        If Not dtUserControlFundedDate Is Nothing Then
                '            dtUserControlFundedDate.Text = String.Empty
                '            l_dt_transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                '            l_dt_transmittals.Rows(i).Item("dtmPaidDate") = String.Empty
                '            dtUserControlFundedDate.Enabled = False
                '        End If

                '    End If



                'End If

                'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                If Not Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("Balance")) = 0 Then

                    l_dt_transmittals.Rows(i).Item("FundedDate") = String.Empty
                    l_dt_transmittals.Rows(i).Item("dtmPaidDate") = String.Empty

                End If
                'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                'Added by Ashish on 12 May ,End



                TextBoxTotalBalance.Text = l_double_TotalBalance + l_double_CreditdDiff + l_double_ReceiptDiff
                DataGridTransmit.DataSource = l_dt_transmittals
                DataGridTransmit.DataBind()
                Viewstate("Transmittals") = l_dt_transmittals
                ViewState("Receipts") = l_dt_Receipts
                ' ViewState("Interests") = l_dt_Interests
            End If
            'TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalBalance.Text))
            TextBoxTotalBalance.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalBalance.Text))
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Protected Sub Check_Clicked1(ByVal sender As Object, ByVal e As EventArgs)
    '    Try

    '        Dim ck1 As CheckBox = CType(sender, CheckBox)
    '        Dim dgItem As DataGridItem = CType(ck1.NamingContainer, DataGridItem)
    '        Dim i As Integer = dgItem.ItemIndex
    '        Dim l_double_AmtDue As Double
    '        Dim l_double_AmtRcpts As Double
    '        Dim l_double_AmtCredit As Double
    '        Dim l_double_AmtInterest As Double
    '        Dim l_double_AmtBalance As Double
    '        Dim l_double_AmtPaid As Double
    '        Dim l_dt_transmittals As DataTable
    '        If Not Viewstate("Transmittals") Is Nothing Then
    '            l_dt_transmittals = Viewstate("Transmittals")
    '        End If

    '        l_double_AmtDue = Convert.ToDecimal(TextBoxTotalAmount.Text)
    '        l_double_AmtRcpts = Convert.ToDecimal(TextBoxTotalReceipts.Text)
    '        l_double_AmtCredit = Convert.ToDecimal(TextBoxTotalCredits.Text)
    '        l_double_AmtInterest = Convert.ToDecimal(TextBoxTotalInterest.Text)
    '        l_double_AmtBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
    '        l_double_AmtPaid = Session("AmtPaid")
    '        Session("RcptsSlctd") = False
    '        Session("CreditsSlctd") = False
    '        Session("InterestsSlctd") = False
    '        If ck1.Checked = True Then
    '            l_dt_transmittals.Rows(i).Item("Slctd") = True
    '        Else
    '            l_dt_transmittals.Rows(i).Item("Slctd") = False
    '        End If
    '        If ck1.Checked = False And (l_double_AmtCredit + l_double_AmtRcpts + l_double_AmtInterest = 0) Then
    '            ButtonPayItem.Enabled = False
    '            If Not l_dt_transmittals Is Nothing Then
    '                For i = 0 To l_dt_transmittals.Rows.Count - 1
    '                    l_dt_transmittals.Rows(i).Item("TotAppliedRcpts") = l_dt_transmittals.Rows(i).Item("OrgAppliedRcpts")
    '                    l_dt_transmittals.Rows(i).Item("TotAppliedCredit") = l_dt_transmittals.Rows(i).Item("OrgAppliedCredit")
    '                    l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedInterest"))
    '                    l_dt_transmittals.Rows(i).Item("TotAppliedInterest") = l_dt_transmittals.Rows(i).Item("OrgAppliedInterest")
    '                Next
    '            End If
    '            ' TextBoxCredits.Text = Session("amtStartCredit")
    '        Else
    '            If ck1.Checked = False Then
    '                Dim l_double_AmtStartCredit As Double
    '                l_double_AmtStartCredit = Convert.ToDecimal(Session("amtStartCredit"))
    '                If Not l_dt_transmittals Is Nothing Then
    '                    l_double_AmtStartCredit = l_double_AmtStartCredit + (Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("OrgAppliedCredit")))
    '                    l_dt_transmittals.Rows(i).Item("TotAppliedRcpts") = l_dt_transmittals.Rows(i).Item("OrgAppliedRcpts")
    '                    l_dt_transmittals.Rows(i).Item("TotAppliedCredit") = l_dt_transmittals.Rows(i).Item("OrgAppliedCredit")
    '                    l_dt_transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("AmtDue")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedInterest"))
    '                    l_dt_transmittals.Rows(i).Item("TotAppliedInterest") = l_dt_transmittals.Rows(i).Item("OrgAppliedInterest")
    '                End If
    '                Session("amtStartCredit") = l_double_AmtStartCredit
    '                TextBoxCredits.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtStartCredit))
    '            End If
    '        End If
    '        l_double_AmtRcpts = 0
    '        l_double_AmtCredit = 0
    '        l_double_AmtBalance = 0
    '        l_double_AmtInterest = 0
    '        For i = 0 To l_dt_transmittals.Rows.Count - 1
    '            l_double_AmtRcpts = l_double_AmtRcpts + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedRcpts"))
    '            l_double_AmtCredit = l_double_AmtCredit + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedCredit"))
    '            l_double_AmtBalance = l_double_AmtBalance + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("Balance"))
    '            l_double_AmtInterest = l_double_AmtInterest + Convert.ToDecimal(l_dt_transmittals.Rows(i).Item("TotAppliedInterest"))
    '        Next
    '        TextBoxTotalReceipts.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtRcpts))
    '        TextBoxTotalCredits.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtCredit))
    '        TextBoxTotalInterest.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtInterest))
    '        TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtBalance))
    '        DataGridTransmit.DataSource = l_dt_transmittals
    '        DataGridTransmit.DataBind()
    '        Viewstate("Transmittals") = l_dt_transmittals
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub

    Private Sub ImageButtonCredits_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonCredits.Click
        Try
            Dim l_dt_Transmittals As DataTable
            l_dt_Transmittals = viewstate("Transmittals")
            Dim i As Integer
            'Dim l_double_AppliedCredit As Double
            'Dim l_double_AmtStartCredit As Double
            'Dim l_double_CreditAvail As Double
            Dim l_double_TextBoxCredits As Double
            Dim l_double_TextBoxTotCredits As Double
            Dim l_double_StartCredits As Double
            Dim l_double_TempBalance As Double
            Dim l_double_Balance As Double
            Dim l_double_TextBoxTotBalance As Double
            'Dim l_double_CurrentBalance As Double
            Dim l_double_TempAppliedCredit As Double
            Dim l_double_TotAppliedCredit As Double

            l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))
            l_double_TotAppliedCredit = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit"))
            l_double_TextBoxCredits = Convert.ToDecimal(TextBoxCredits.Text)
            l_double_TextBoxTotCredits = Convert.ToDecimal(TextBoxTotalCredits.Text)
            l_double_TextBoxTotBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
            l_double_StartCredits = Convert.ToDecimal(Session("amtStartCredit"))
            Dim l_dt_Orgtransmittals As DataTable
            l_dt_Orgtransmittals = CType(Session("OriginalTramittals"), DataTable)
            If l_double_StartCredits <> l_double_TextBoxCredits Then
                l_double_TextBoxTotCredits = 0
                l_double_TextBoxTotBalance = 0
                For i = 0 To l_dt_Transmittals.Rows.Count - 1

                    l_double_TempAppliedCredit = l_dt_Orgtransmittals.Rows(i).Item("TotAppliedCredit")
                    l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = l_double_TempAppliedCredit
                    l_double_TextBoxTotCredits = l_double_TextBoxTotCredits + l_double_TempAppliedCredit
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                    'l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                    'Added by Ashish ,Start
                    l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")))
                    'Added by Ashish ,End
                    l_dt_Transmittals.Rows(i).Item("Balance") = l_double_Balance
                    l_double_TextBoxTotBalance = l_double_TextBoxTotBalance + l_double_Balance
                    'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                    'If Not Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) = 0 Then
                    '    Dim dgItem As DataGridItem = DataGridTransmit.Items(i)
                    '    If Not dgItem Is Nothing Then
                    '        Dim dtUserControlFundedDate As DateUserControl
                    '        dtUserControlFundedDate = CType(dgItem.FindControl("DateusercontrolFundedDate"), DateUserControl)
                    '        If Not dtUserControlFundedDate Is Nothing Then
                    '            dtUserControlFundedDate.Text = String.Empty
                    '            l_dt_Transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                    '            l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = String.Empty
                    '            dtUserControlFundedDate.Enabled = False
                    '        End If

                    '    End If
                    'End If
                    'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                    'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                    If Not Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")), 2) = 0 Then
                        l_dt_Transmittals.Rows(i).Item("FundedDate") = String.Empty
                        l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = String.Empty

                    End If
                    'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                Next
                'TextBoxTotalCredits.Text = FormatCurrency(l_double_TextBoxTotCredits)
                'TextBoxTotalBalance.Text = FormatCurrency(l_double_TextBoxTotBalance)
                'TextBoxCredits.Text = FormatCurrency(l_double_StartCredits)
                DataGridTransmit.DataSource = l_dt_Transmittals
                DataGridTransmit.DataBind()
                TextBoxTotalCredits.Text = String.Format("{0:N}", l_double_TextBoxTotCredits)
                TextBoxTotalBalance.Text = String.Format("{0:N}", l_double_TextBoxTotBalance)
                TextBoxCredits.Text = String.Format("{0:N}", l_double_StartCredits)
                ViewState("Transmittals") = l_dt_Transmittals
                If TextBoxTotalReceipts.Text = "0.00" And TextBoxTotalCredits.Text = "0.00" Then
                    ButtonPayItem.Enabled = False
                Else
                    ButtonPayItem.Enabled = True
                End If
                Exit Sub

            End If

            'If l_double_TextBoxCredits > 0 Then
            '    ButtonPayItem.Enabled = True
            'End If

            For i = 0 To l_dt_Transmittals.Rows.Count - 1
                l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))
                If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_double_Balance > 0 Then

                    If l_double_TextBoxCredits > 0 Then
                        l_double_TotAppliedCredit = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit"))
                        If l_double_TextBoxCredits >= l_double_Balance Then

                            l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = l_double_TotAppliedCredit + l_double_Balance
                            l_double_TextBoxTotCredits = l_double_TextBoxTotCredits + l_double_Balance
                            TextBoxTotalCredits.Text = l_double_TextBoxTotCredits
                            l_double_TextBoxCredits = l_double_TextBoxCredits - l_double_Balance
                            TextBoxCredits.Text = l_double_TextBoxCredits
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                            'l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                            'Added By Ashish ,Start
                            l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")))
                            'Added By Ashish ,End
                            l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_Balance
                            TextBoxTotalBalance.Text = l_double_TextBoxTotBalance
                            ' l_dt_Transmittals.Rows(i).Item("Slctd") = False
                            l_double_Balance = 0
                        End If
                        If l_double_Balance > l_double_TextBoxCredits Then

                            ' l_double_TempAppliedCredit = l_double_TextBoxCredits

                            l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = l_double_TotAppliedCredit + l_double_TextBoxCredits
                            l_double_TextBoxTotCredits = l_double_TextBoxTotCredits + l_double_TextBoxCredits
                            TextBoxTotalCredits.Text = l_double_TextBoxTotCredits
                            TextBoxCredits.Text = 0
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                            'l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                            'Added By Ashish ,Start
                            l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")))
                            'Added By Ashish ,End
                            l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_TextBoxCredits
                            TextBoxTotalBalance.Text = l_double_TextBoxTotBalance
                            l_double_TextBoxCredits = 0
                            ' l_dt_Transmittals.Rows(i).Item("Slctd") = True
                        End If
                        'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                        'Added by on 12 May Ashish enable funded date and set with defalut value, Start

                        'If l_double_Balance = 0 Then


                        '    Dim dgItem As DataGridItem = DataGridTransmit.Items(i)
                        '    If Not dgItem Is Nothing Then
                        '        Dim dtUserControlFundedDate As DateUserControl

                        '        dtUserControlFundedDate = CType(dgItem.FindControl("DateusercontrolFundedDate"), DateUserControl)
                        '        If Not dtUserControlFundedDate Is Nothing Then
                        '            If Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) = l_double_Balance Then

                        '                If dtUserControlFundedDate.Text = String.Empty Then

                        '                    dtUserControlFundedDate.Text = Convert.ToDateTime(Date.Today).ToString("MM/dd/yyyy")
                        '                    l_dt_Transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                        '                    l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = dtUserControlFundedDate.Text
                        '                End If
                        '                dtUserControlFundedDate.Enabled = True

                        '            Else

                        '                dtUserControlFundedDate.Text = String.Empty
                        '                l_dt_Transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                        '                l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = dtUserControlFundedDate.Text
                        '                dtUserControlFundedDate.Enabled = False
                        '            End If
                        '        End If
                        '    End If
                        'End If
                        'Added by on 12 May Ashish enable funded date and set with defalut value, End
                        'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                        'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                        If l_double_Balance = 0 Then
                            If Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")), 2) = l_double_Balance Then
                                l_dt_Transmittals.Rows(i).Item("FundedDate") = Me.DateusercontrolFundedDate.Text
                                l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = Me.DateusercontrolFundedDate.Text
                            Else
                                l_dt_Transmittals.Rows(i).Item("FundedDate") = String.Empty
                                l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = String.Empty
                            End If
                        End If
                        'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                    End If

                End If



            Next
            'TextBoxCredits.Text = FormatCurrency(Convert.ToDecimal(TextBoxCredits.Text))
            'TextBoxTotalCredits.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalCredits.Text))
            'TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalBalance.Text))
            If TextBoxTotalReceipts.Text = "0.00" And TextBoxTotalCredits.Text = "0.00" Then
                ButtonPayItem.Enabled = False
            Else
                ButtonPayItem.Enabled = True
            End If
            TextBoxTotalCredits.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalCredits.Text))
            TextBoxTotalBalance.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalBalance.Text))
            TextBoxCredits.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxCredits.Text))
            DataGridTransmit.DataSource = l_dt_Transmittals
            DataGridTransmit.DataBind()
            RebindCheckBox_Transmittals()
            ViewState("Transmittals") = l_dt_Transmittals

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "ImageButtonCredits_Click", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Sub ApplyReceipts()
        Dim l_dt_Receipts As DataTable
        Dim l_dt_Transmittals As DataTable

        Dim i As Integer

        Dim l_double_TempAppliedRcpts As Double
        Dim l_double_Balance As Double
        Dim l_double_TotAppliedRcpts As Double

        Dim l_double_ReceiptAmount As Double
        Dim l_double_TextBoxTotReceipts As Double
        Dim l_double_TextBoxTotBalance As Double
        Dim l_double_ReceiptAmount1 As Double

        Dim l_index As Integer
        If Not ViewState("Receipts") Is Nothing Then
            l_dt_Receipts = ViewState("Receipts")
        End If

        l_dt_Transmittals = ViewState("Transmittals")
        Dim l_dt_Orgtransmittals As DataTable
        l_dt_Orgtransmittals = CType(Session("OriginalTramittals"), DataTable)
        If Me.DataGridReceipts.SelectedIndex <> -1 Then
            l_index = Me.DataGridReceipts.SelectedIndex
        End If
        Dim ss As String
        ss = DataGridReceipts.Items(l_index).Cells(4).Text()
        l_double_ReceiptAmount1 = Convert.ToDouble(ss)


        Try

            If Not l_dt_Receipts Is Nothing Then
                If l_dt_Receipts.Rows.Count > 0 Then

                    l_double_ReceiptAmount = Convert.ToDecimal(l_dt_Receipts.Rows(l_index).Item("Amount"))
                    l_double_TextBoxTotBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
                    l_double_TextBoxTotReceipts = Convert.ToDecimal(TextBoxTotalReceipts.Text)
                    If l_double_ReceiptAmount <> l_double_ReceiptAmount1 Then

                        l_double_TextBoxTotReceipts = 0
                        l_double_TextBoxTotBalance = 0
                        For i = 0 To l_dt_Transmittals.Rows.Count - 1

                            l_double_TotAppliedRcpts = l_dt_Orgtransmittals.Rows(i).Item("TotAppliedRcpts")
                            l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = l_double_TotAppliedRcpts
                            l_double_TextBoxTotReceipts = l_double_TextBoxTotReceipts + l_double_TotAppliedRcpts
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                            'l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                            'code added by Ashish  changes according Phase IV-2 Remove Interest ,Start
                            l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")))
                            'code added by Ashish  changes according Phase IV-2 Remove Interest ,End
                            l_dt_Transmittals.Rows(i).Item("Balance") = l_double_Balance
                            l_double_TextBoxTotBalance = l_double_TextBoxTotBalance + l_double_Balance
                            'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                            'If Not Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) = 0 Then
                            '    Dim dgItem As DataGridItem = DataGridTransmit.Items(i)
                            '    If Not dgItem Is Nothing Then
                            '        Dim dtUserControlFundedDate As DateUserControl
                            '        dtUserControlFundedDate = CType(dgItem.FindControl("DateusercontrolFundedDate"), DateUserControl)
                            '        If Not dtUserControlFundedDate Is Nothing Then
                            '            dtUserControlFundedDate.Text = String.Empty
                            '            l_dt_Transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                            '            l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = dtUserControlFundedDate.Text
                            '            dtUserControlFundedDate.Enabled = False
                            '        End If
                            '    End If
                            'End If
                            'Commentd by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                            'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                            If Not Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")), 2) = 0 Then
                                l_dt_Transmittals.Rows(i).Item("FundedDate") = String.Empty
                                l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = String.Empty

                            End If

                            'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                        Next
                        l_dt_Receipts.Rows(l_index).Item("Amount") = l_double_ReceiptAmount1
                        'TextBoxTotalReceipts.Text = FormatCurrency(l_double_TextBoxTotReceipts)
                        'TextBoxTotalBalance.Text = FormatCurrency(l_double_TextBoxTotBalance)

                        TextBoxTotalReceipts.Text = String.Format("{0:N}", l_double_TextBoxTotReceipts)
                        TextBoxTotalBalance.Text = String.Format("{0:N}", l_double_TextBoxTotBalance)
                        DataGridTransmit.DataSource = l_dt_Transmittals
                        DataGridTransmit.DataBind()

                        ViewState("Transmittals") = l_dt_Transmittals
                        'Added by Ashish on 15 May 2008 ,Start
                        If TextBoxTotalReceipts.Text = "0.00" And TextBoxTotalCredits.Text = "0.00" Then
                            ButtonPayItem.Enabled = False
                        Else
                            ButtonPayItem.Enabled = True
                        End If
                        'Added by Ashish on 15 May 2008 ,End
                        Exit Sub

                    End If
                    'l_double_ReceiptAmount = Convert.ToDecimal(l_dt_Receipts.Rows(l_index).Item("Amount"))
                    'l_double_TextBoxTotBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
                    'l_double_TextBoxTotReceipts = Convert.ToDecimal(TextBoxTotalReceipts.Text)
                    For i = 0 To l_dt_Transmittals.Rows.Count - 1

                        l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))
                        If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_double_Balance > 0 Then
                            l_double_TotAppliedRcpts = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts"))
                            If l_double_ReceiptAmount > 0 Then
                                If l_double_ReceiptAmount >= l_double_Balance Then

                                    ' l_double_TempAppliedRcpts=
                                    l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = l_double_TotAppliedRcpts + l_double_Balance
                                    l_double_TextBoxTotReceipts = l_double_TextBoxTotReceipts + l_double_Balance
                                    TextBoxTotalReceipts.Text = l_double_TextBoxTotReceipts
                                    l_double_ReceiptAmount = Convert.ToDecimal(l_double_ReceiptAmount - l_double_Balance)
                                    l_dt_Receipts.Rows(l_index).Item("Amount") = l_double_ReceiptAmount
                                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                                    'l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
                                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                                    'code added by Ashish ,Start
                                    l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")))
                                    'code added by Ashish ,End
                                    l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_Balance
                                    TextBoxTotalBalance.Text = l_double_TextBoxTotBalance
                                    l_double_Balance = 0

                                End If
                                If l_double_Balance > l_double_ReceiptAmount Then

                                    l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = l_double_TotAppliedRcpts + l_double_ReceiptAmount
                                    l_double_TextBoxTotReceipts = l_double_TextBoxTotReceipts + l_double_ReceiptAmount
                                    TextBoxTotalReceipts.Text = l_double_TextBoxTotReceipts
                                    l_dt_Receipts.Rows(l_index).Item("Amount") = 0
                                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                                    'l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
                                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                                    'code added by Ashish ,Start
                                    l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")))
                                    'code added by Ashish ,End
                                    l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_ReceiptAmount
                                    TextBoxTotalBalance.Text = l_double_TextBoxTotBalance
                                    l_double_ReceiptAmount = 0

                                End If
                                'Commented By Ashish on 25-Sep-2008 remove date control fron grid,Start
                                'Added by on 12 May Ashish enable funded date and set with defalut value ,Start
                                'If l_double_Balance = 0 Then
                                '    Dim dgItem As DataGridItem = DataGridTransmit.Items(i)
                                '    If Not dgItem Is Nothing Then
                                '        Dim dtUserControlFundedDate As DateUserControl
                                '        dtUserControlFundedDate = CType(dgItem.FindControl("DateusercontrolFundedDate"), DateUserControl)
                                '        If Not dtUserControlFundedDate Is Nothing Then
                                '            If Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) = l_double_Balance Then

                                '                If dtUserControlFundedDate.Text = String.Empty Then
                                '                    dtUserControlFundedDate.Text = Convert.ToDateTime(Date.Today).ToString("MM/dd/yyyy")
                                '                    l_dt_Transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                                '                    l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = dtUserControlFundedDate.Text
                                '                End If
                                '                dtUserControlFundedDate.Enabled = True
                                '            Else
                                '                dtUserControlFundedDate.Text = String.Empty
                                '                l_dt_Transmittals.Rows(i).Item("FundedDate") = dtUserControlFundedDate.Text
                                '                l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = dtUserControlFundedDate.Text
                                '                dtUserControlFundedDate.Enabled = False
                                '            End If
                                '        End If

                                '    End If
                                'End If
                                'Added by on 12 May Ashish enable funded date and set with defalut value ,End
                                'Commented By Ashish on 25-Sep-2008 remove date control fron grid,Start
                                'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                                If l_double_Balance = 0 Then
                                    If Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")), 2) = l_double_Balance Then
                                        l_dt_Transmittals.Rows(i).Item("FundedDate") = Me.DateusercontrolFundedDate.Text
                                        l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = Me.DateusercontrolFundedDate.Text
                                    Else
                                        l_dt_Transmittals.Rows(i).Item("FundedDate") = String.Empty
                                        l_dt_Transmittals.Rows(i).Item("dtmPaidDate") = String.Empty
                                    End If
                                End If
                                'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                            End If


                        End If
                    Next

                    'For Each dgRow As DataGridItem In DataGridTransmit.Items
                    'TextBoxTotalReceipts.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalReceipts.Text))
                    'TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(TextBoxTotalBalance.Text))
                    TextBoxTotalReceipts.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalReceipts.Text))
                    TextBoxTotalBalance.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalBalance.Text))
                    DataGridTransmit.DataSource = l_dt_Transmittals
                    DataGridTransmit.DataBind()
                    RebindCheckBox_Transmittals()
                    ViewState("Transmittals") = l_dt_Transmittals
                    ViewState("Receipts") = l_dt_Receipts
                End If
            End If
            If TextBoxTotalReceipts.Text = "0.00" And TextBoxTotalCredits.Text = "0.00" Then
                ButtonPayItem.Enabled = False
            Else
                ButtonPayItem.Enabled = True
            End If
        Catch
            Throw
        End Try


    End Sub
    'Public Sub ApplyReceipts1()
    '    Try

    '        Dim l_dt_Receipts As DataTable
    '        Dim l_dt_Transmittals As DataTable

    '        Dim i As Integer
    '        Dim l_index As Integer

    '        l_dt_Receipts = ViewState("Receipts")
    '        l_dt_Transmittals = ViewState("Transmittals")

    '        If Me.DataGridReceipts.SelectedIndex <> -1 Then
    '            l_index = Me.DataGridReceipts.SelectedIndex
    '        Else
    '            If l_dt_Receipts.Rows.Count > 0 Then
    '                l_index = 0
    '                Me.DataGridReceipts.SelectedIndex = 0
    '            Else
    '                l_index = -1
    '            End If
    '        End If

    '        If Not l_dt_Receipts Is Nothing Then
    '            If l_dt_Receipts.Rows.Count > 0 Then
    '                'Checking if the amount in the receipts table is 0
    '                If l_dt_Receipts.Rows.Count = 0 Or l_dt_Receipts.Rows(l_index).Item("Amount") = 0 Then
    '                    ButtonPayItem.Enabled = False
    '                    Exit Sub
    '                End If

    '                If Session("RcptsSlctd") = False Then
    '                    If Not l_dt_Transmittals Is Nothing Then
    '                        For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                            If l_dt_Transmittals.Rows(i).Item("Slctd") = True And Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) > 0 Then
    '                                l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")
    '                            End If
    '                        Next
    '                    End If
    '                End If

    '                Dim l_double_OldRcpts As Double
    '                Dim l_double_RcptsAvail As Double
    '                If Not l_dt_Transmittals Is Nothing Then
    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1

    '                        l_double_OldRcpts += Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts"))

    '                    Next
    '                End If
    '                Dim flg As Boolean
    '                flg = False
    '                If Session("RcptsSlctd") = False Then

    '                    l_double_RcptsAvail = Convert.ToDecimal(l_dt_Receipts.Rows(l_index).Item("Amount")) - l_double_OldRcpts
    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        If l_dt_Transmittals.Rows(i).Item("Slctd") = True Then
    '                            flg = True
    '                            Exit For
    '                        End If
    '                    Next

    '                    If flg = False Then  'If none of the Records is selected then select all
    '                        For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = True
    '                        Next
    '                    End If

    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        If l_dt_Transmittals.Rows(i).Item("Balance") > 0 And l_dt_Transmittals.Rows(i).Item("Slctd") = True Then
    '                            l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest"))

    '                            If l_double_RcptsAvail >= Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) Then
    '                                l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")), 2)
    '                            Else
    '                                l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = Math.Round(l_double_RcptsAvail + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")), 2)
    '                            End If
    '                            l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")))

    '                            l_double_RcptsAvail = l_double_RcptsAvail - Math.Round((Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts"))), 2)
    '                        End If

    '                        If (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")) = 0) And (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")) = 0) Then
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = False
    '                        End If
    '                    Next
    '                    Session("RcptsSlctd") = True
    '                    ButtonPayItem.Enabled = True
    '                Else

    '                    Session("RcptsSlctd") = False

    '                    If Session("CreditsSlctd") = False And Session("InterestsSlctd") = False Then
    '                        ButtonPayItem.Enabled = False
    '                    End If

    '                    l_double_RcptsAvail = Convert.ToDecimal(l_dt_Receipts.Rows(l_index).Item("Amount"))

    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) + (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")))
    '                        If (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")) = 0) And (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")) = 0) Then
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = False
    '                        Else
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = True
    '                        End If
    '                        l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") = l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")
    '                    Next
    '                End If 'For Session("RcptsSlctd") = False


    '                Dim l_double_AmtRcpts As Double
    '                Dim l_double_AmtBalance As Double
    '                For i = 0 To l_dt_Transmittals.Rows.Count - 1

    '                    l_double_AmtRcpts = l_double_AmtRcpts + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts"))
    '                    l_double_AmtBalance = l_double_AmtBalance + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))

    '                Next
    '                If l_double_AmtRcpts <> 0 Then
    '                    TextBoxTotalReceipts.Text = l_double_AmtRcpts
    '                Else
    '                    TextBoxTotalReceipts.Text = 0
    '                End If
    '                If l_double_AmtBalance <> 0 Then
    '                    TextBoxTotalBalance.Text = l_double_AmtBalance
    '                Else
    '                    TextBoxTotalBalance.Text = 0
    '                End If

    '                DataGridTransmit.DataSource = l_dt_Transmittals
    '                DataGridTransmit.DataBind()

    '                ViewState("Transmittals") = l_dt_Transmittals

    '            End If
    '        Else
    '            ButtonPayItem.Enabled = False
    '        End If 'For l_dt_Receipts

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'Private Sub ImageButtonCredits1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonCredits.Click
    '    Try
    '        Dim l_dt_Transmittals As DataTable
    '        l_dt_Transmittals = viewstate("Transmittals")
    '        Dim i As Integer
    '        Dim l_double_AppliedCredit As Double
    '        Dim l_double_AmtStartCredit As Double
    '        Dim l_double_CreditAvail As Double
    '        l_double_AmtStartCredit = Convert.ToDecimal(Session("amtStartCredit"))
    '        If Session("CreditsSlctd") = False Then
    '            If Not l_dt_Transmittals Is Nothing Then
    '                For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                    If l_dt_Transmittals.Rows(i).Item("Slctd") = True And Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) > 0 Then
    '                        l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")
    '                    End If
    '                Next
    '            End If
    '        End If
    '        If Not l_dt_Transmittals Is Nothing Then
    '            For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_dt_Transmittals.Rows(i).Item("Balance") > 0 Then
    '                    l_double_AppliedCredit += Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit"))
    '                End If
    '            Next
    '        End If
    '        'l_double_AmtStartCredit = Convert.ToDecimal(Session("CreditBalance")) - l_double_AppliedCredit
    '        l_double_CreditAvail = l_double_AmtStartCredit
    '        Session("amtStartCredit") = l_double_AmtStartCredit


    '        If l_double_CreditAvail = 0 Then
    '            ButtonPayItem.Enabled = False
    '            Session("CreditsSlctd") = True
    '        End If

    '        Dim _flg As Boolean
    '        _flg = False
    '        If Session("CreditsSlctd") = False Then
    '            For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                If l_dt_Transmittals.Rows(i).Item("Slctd") = True Then
    '                    _flg = True
    '                    Exit For
    '                End If
    '            Next


    '            If _flg = False Then  'If none of the Records is selected then select all
    '                For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                    l_dt_Transmittals.Rows(i).Item("Slctd") = True
    '                Next
    '            End If

    '            For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_dt_Transmittals.Rows(i).Item("Balance") > 0 Then
    '                    l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest"))

    '                    If l_double_CreditAvail >= l_dt_Transmittals.Rows(i).Item("Balance") Then
    '                        l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit"))
    '                    Else
    '                        l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = Convert.ToDecimal(l_double_CreditAvail) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit"))
    '                    End If
    '                    'l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit"))
    '                    l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")))
    '                    'l_double_CreditAvail = Convert.ToDecimal(l_double_CreditAvail) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit"))
    '                    l_double_CreditAvail = Convert.ToDecimal(l_double_CreditAvail) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")))
    '                End If

    '                If Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")) = 0 And Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")) = 0 And Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")) = 0 Then
    '                    l_dt_Transmittals.Rows(i).Item("Slctd") = False
    '                End If
    '            Next

    '            Session("CreditsSlctd") = True
    '            ButtonPayItem.Enabled = True
    '        Else
    '            Session("CreditsSlctd") = False
    '            l_double_CreditAvail = Session("CreditBalance")
    '            If Session("RcptsSlctd") = False Then
    '                ButtonPayItem.Enabled = False
    '            End If

    '            For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                l_dt_Transmittals.Rows(i).Item("Balance") = l_dt_Transmittals.Rows(i).Item("Balance") + (l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit"))
    '                If (l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts") = 0) And (l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest") = 0) Then
    '                    l_dt_Transmittals.Rows(i).Item("Slctd") = False
    '                Else
    '                    l_dt_Transmittals.Rows(i).Item("Slctd") = True
    '                End If
    '                l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") = l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")
    '            Next
    '        End If
    '        Dim l_double_AmtCredit As Double
    '        Dim l_double_AmtBalance As Double
    '        For i = 0 To l_dt_Transmittals.Rows.Count - 1

    '            l_double_AmtCredit = l_double_AmtCredit + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit"))
    '            l_double_AmtBalance = l_double_AmtBalance + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))

    '        Next
    '        If l_double_AmtCredit <> 0 Then
    '            TextBoxTotalCredits.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtCredit))
    '        Else
    '            TextBoxTotalCredits.Text = 0
    '        End If
    '        If l_double_AmtBalance <> 0 Then
    '            TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtBalance))
    '        Else
    '            TextBoxTotalBalance.Text = 0
    '        End If
    '        Session("amtStartCredit") = l_double_CreditAvail
    '        TextBoxCredits.Text = Session("amtStartCredit")
    '        TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtBalance))
    '        DataGridTransmit.DataSource = l_dt_Transmittals
    '        DataGridTransmit.DataBind()

    '        ViewState("Transmittals") = l_dt_Transmittals


    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub

    Private Sub ImageButtonReceipts_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonReceipts.Click
        Try
            If Me.DataGridReceipts.Items.Count = 0 Then
                Exit Sub
            End If
            ApplyReceipts()
            '  ApplyReceipts1()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "ImageButtonReceipts_Click", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub
    'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
    'commented By ashish Start
    ''Added by Shubhrata Dec 27th 2006.Its similar to ApplyInterest2,the only change made is the placing of few lines of the code
    ''This was done because while doing YREN 3006-to maintain transactions,it was noticed that the image button interest was 
    ''not toggling correctly.Upon cancelling a interest the balance was not reset.
    'Sub ApplyInterest()
    '    Dim l_dt_Orgtransmittals As DataTable
    '    Dim l_dt_Interests As DataTable
    '    Dim l_dt_Transmittals As DataTable
    '    Dim l_double_TempAppliedInterest As Double
    '    Dim l_double_Balance As Double
    '    Dim l_double_TotAppliedInterest As Double

    '    Dim l_double_InterestAmount As Double = 0
    '    Dim l_double_TextBoxTotInterest As Double = 0
    '    Dim l_double_TextBoxTotBalance As Double = 0
    '    Dim l_double_InterestAmount1 As Double = 0

    '    Dim l_index As Integer
    '    Dim i As Integer
    '    l_dt_Interests = ViewState("Interests")
    '    l_dt_Transmittals = ViewState("Transmittals")
    '    l_dt_Orgtransmittals = CType(Session("OriginalTramittals"), DataTable)
    '    'If Not l_dt_Interests Is Nothing Then

    '    '    l_double_InterestAmount = Convert.ToDecimal(l_dt_Interests.Rows(l_index).Item("Amount"))

    '    'End If
    '    'If l_double_InterestAmount > 0 Then



    '    If Me.DataGridInterest.SelectedIndex <> -1 Then
    '        l_index = Me.DataGridInterest.SelectedIndex
    '    End If
    '    Dim sTemp As String
    '    ' DataGridInterest.
    '    sTemp = DataGridInterest.Items(l_index).Cells(3).Text

    '    l_double_InterestAmount1 = Convert.ToDouble(sTemp)

    '    Try

    '        If Not l_dt_Interests Is Nothing Then
    '            If l_dt_Interests.Rows.Count > 0 Then
    '                l_double_InterestAmount = Convert.ToDecimal(l_dt_Interests.Rows(l_index).Item("Amount"))
    '                l_double_TextBoxTotBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
    '                l_double_TextBoxTotInterest = Convert.ToDecimal(TextBoxTotalInterest.Text)
    '                If l_double_InterestAmount <> l_double_InterestAmount1 Then
    '                    l_double_TextBoxTotInterest = 0
    '                    l_double_TextBoxTotBalance = 0
    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1

    '                        l_double_TotAppliedInterest = l_dt_Orgtransmittals.Rows(i).Item("TotappliedInterest")
    '                        l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_double_TotAppliedInterest
    '                        l_double_TextBoxTotInterest = l_double_TextBoxTotInterest + l_double_TotAppliedInterest
    '                        l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
    '                        l_dt_Transmittals.Rows(i).Item("Balance") = l_double_Balance
    '                        l_double_TextBoxTotBalance = l_double_TextBoxTotBalance + l_double_Balance
    '                    Next

    '                    TextBoxTotalInterest.Text = String.Format("{0:N}", l_double_TextBoxTotInterest)
    '                    TextBoxTotalBalance.Text = String.Format("{0:N}", l_double_TextBoxTotBalance)
    '                    l_dt_Interests.Rows(l_index).Item("Amount") = l_double_InterestAmount1
    '                    DataGridTransmit.DataSource = l_dt_Transmittals
    '                    DataGridTransmit.DataBind()

    '                    ViewState("Transmittals") = l_dt_Transmittals
    '                    Exit Sub
    '                End If
    '                For i = 0 To l_dt_Transmittals.Rows.Count - 1

    '                    l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))
    '                    If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_double_Balance > 0 Then

    '                        l_double_TotAppliedInterest = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest"))

    '                        If l_double_InterestAmount > 0 Then
    '                            If l_double_InterestAmount >= l_double_Balance Then

    '                                ' l_double_TempAppliedRcpts=
    '                                l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_double_TotAppliedInterest + l_double_Balance
    '                                l_double_TextBoxTotInterest = l_double_TextBoxTotInterest + l_double_Balance
    '                                TextBoxTotalInterest.Text = l_double_TextBoxTotInterest
    '                                l_double_InterestAmount = l_double_InterestAmount - l_double_Balance
    '                                l_dt_Interests.Rows(l_index).Item("Amount") = l_double_InterestAmount
    '                                l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
    '                                l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_Balance
    '                                TextBoxTotalBalance.Text = l_double_TextBoxTotBalance

    '                                l_double_Balance = 0
    '                            End If
    '                            If l_double_Balance > l_double_InterestAmount Then

    '                                l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_double_TotAppliedInterest + l_double_InterestAmount
    '                                l_double_TextBoxTotInterest = l_double_TextBoxTotInterest + l_double_InterestAmount
    '                                TextBoxTotalInterest.Text = l_double_TextBoxTotInterest
    '                                l_dt_Interests.Rows(l_index).Item("Amount") = 0
    '                                l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
    '                                l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_InterestAmount
    '                                TextBoxTotalBalance.Text = l_double_TextBoxTotBalance
    '                                l_double_InterestAmount = 0

    '                            End If

    '                        End If
    '                    End If
    '                Next
    '                TextBoxTotalInterest.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalInterest.Text))
    '                TextBoxTotalBalance.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalBalance.Text))


    '                DataGridTransmit.DataSource = l_dt_Transmittals
    '                DataGridTransmit.DataBind()
    '                RebindCheckBox_Transmittals()
    '                ViewState("Transmittals") = l_dt_Transmittals
    '                ViewState("Interests") = l_dt_Interests
    '                ButtonPayItem.Enabled = True
    '            End If
    '        End If

    '    Catch
    '        Throw
    '    End Try
    '    'End If
    'End Sub

    ''Commented by Shubhrata Dec 27th 2006
    ''Sub ApplyInterest2()




    ''    Dim l_dt_Orgtransmittals As DataTable
    ''    Dim l_dt_Interests As DataTable
    ''    Dim l_dt_Transmittals As DataTable
    ''    Dim l_double_TempAppliedInterest As Double
    ''    Dim l_double_Balance As Double
    ''    Dim l_double_TotAppliedInterest As Double

    ''    Dim l_double_InterestAmount As Double = 0
    ''    Dim l_double_TextBoxTotInterest As Double = 0
    ''    Dim l_double_TextBoxTotBalance As Double = 0
    ''    Dim l_double_InterestAmount1 As Double = 0

    ''    Dim l_index As Integer
    ''    Dim i As Integer
    ''    l_dt_Interests = ViewState("Interests")
    ''    l_dt_Transmittals = ViewState("Transmittals")
    ''    l_dt_Orgtransmittals = CType(Session("OriginalTramittals"), DataTable)
    ''    If Not l_dt_Interests Is Nothing Then

    ''        l_double_InterestAmount = Convert.ToDecimal(l_dt_Interests.Rows(l_index).Item("Amount"))

    ''    End If
    ''    If l_double_InterestAmount > 0 Then



    ''        If Me.DataGridInterest.SelectedIndex <> -1 Then
    ''            l_index = Me.DataGridInterest.SelectedIndex
    ''        End If
    ''        Dim sTemp As String
    ''        ' DataGridInterest.
    ''        sTemp = DataGridInterest.Items(l_index).Cells(3).Text

    ''        l_double_InterestAmount1 = Convert.ToDouble(sTemp)

    ''        Try
    ''            If l_double_InterestAmount <> l_double_InterestAmount1 Then
    ''                l_double_TextBoxTotInterest = 0
    ''                l_double_TextBoxTotBalance = 0
    ''                For i = 0 To l_dt_Transmittals.Rows.Count - 1

    ''                    l_double_TotAppliedInterest = l_dt_Orgtransmittals.Rows(i).Item("TotappliedInterest")
    ''                    l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_double_TotAppliedInterest
    ''                    l_double_TextBoxTotInterest = l_double_TextBoxTotInterest + l_double_TotAppliedInterest
    ''                    l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
    ''                    l_dt_Transmittals.Rows(i).Item("Balance") = l_double_Balance
    ''                    l_double_TextBoxTotBalance = l_double_TextBoxTotBalance + l_double_Balance
    ''                Next

    ''                TextBoxTotalInterest.Text = String.Format("{0:N}", l_double_TextBoxTotInterest)
    ''                TextBoxTotalBalance.Text = String.Format("{0:N}", l_double_TextBoxTotBalance)
    ''                l_dt_Interests.Rows(l_index).Item("Amount") = l_double_InterestAmount1
    ''                DataGridTransmit.DataSource = l_dt_Transmittals
    ''                DataGridTransmit.DataBind()

    ''                ViewState("Transmittals") = l_dt_Transmittals
    ''                Exit Sub
    ''            End If
    ''            If Not l_dt_Interests Is Nothing Then
    ''                If l_dt_Interests.Rows.Count > 0 Then
    ''                    l_double_InterestAmount = Convert.ToDecimal(l_dt_Interests.Rows(l_index).Item("Amount"))
    ''                    l_double_TextBoxTotBalance = Convert.ToDecimal(TextBoxTotalBalance.Text)
    ''                    l_double_TextBoxTotInterest = Convert.ToDecimal(TextBoxTotalInterest.Text)
    ''                    For i = 0 To l_dt_Transmittals.Rows.Count - 1

    ''                        l_double_Balance = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))
    ''                        If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_double_Balance > 0 Then

    ''                            l_double_TotAppliedInterest = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest"))

    ''                            If l_double_InterestAmount > 0 Then
    ''                                If l_double_InterestAmount >= l_double_Balance Then

    ''                                    ' l_double_TempAppliedRcpts=
    ''                                    l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_double_TotAppliedInterest + l_double_Balance
    ''                                    l_double_TextBoxTotInterest = l_double_TextBoxTotInterest + l_double_Balance
    ''                                    TextBoxTotalInterest.Text = l_double_TextBoxTotInterest
    ''                                    l_double_InterestAmount = l_double_InterestAmount - l_double_Balance
    ''                                    l_dt_Interests.Rows(l_index).Item("Amount") = l_double_InterestAmount
    ''                                    l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
    ''                                    l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_Balance
    ''                                    TextBoxTotalBalance.Text = l_double_TextBoxTotBalance

    ''                                    l_double_Balance = 0
    ''                                End If
    ''                                If l_double_Balance > l_double_InterestAmount Then

    ''                                    l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_double_TotAppliedInterest + l_double_InterestAmount
    ''                                    l_double_TextBoxTotInterest = l_double_TextBoxTotInterest + l_double_InterestAmount
    ''                                    TextBoxTotalInterest.Text = l_double_TextBoxTotInterest
    ''                                    l_dt_Interests.Rows(l_index).Item("Amount") = 0
    ''                                    l_dt_Transmittals.Rows(i).Item("Balance") = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("AmtDue")) - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotappliedInterest")))
    ''                                    l_double_TextBoxTotBalance = l_double_TextBoxTotBalance - l_double_InterestAmount
    ''                                    TextBoxTotalBalance.Text = l_double_TextBoxTotBalance
    ''                                    l_double_InterestAmount = 0

    ''                                End If

    ''                            End If
    ''                        End If
    ''                    Next
    ''                    TextBoxTotalInterest.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalInterest.Text))
    ''                    TextBoxTotalBalance.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalBalance.Text))


    ''                    DataGridTransmit.DataSource = l_dt_Transmittals
    ''                    DataGridTransmit.DataBind()
    ''                    RebindCheckBox_Transmittals()
    ''                    ViewState("Transmittals") = l_dt_Transmittals
    ''                    ViewState("Interests") = l_dt_Interests
    ''                    ButtonPayItem.Enabled = True
    ''                End If
    ''            End If

    ''        Catch
    ''            Throw
    ''        End Try
    ''    End If
    ''End Sub
    ''Commented by Shubhrata Dec 27th 2006
    'Private Sub ImageButtonInterest_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonInterest.Click
    '    'Added By Ashutosh Patil as on 20-Feb-2007
    '    'If no. of records in the datagrid are zero then it will not Apply Interest i.e it will not enter into the function ApplyInterest
    '    If Me.DataGridInterest.Items.Count = 0 Then
    '        Exit Sub
    '    End If
    '    ApplyInterest()
    'End Sub
    'commented by Ashish End
    'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 

    'Private Sub ImageButtonInterest_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonInterest.Click
    '    Try
    '        Dim l_dt_Interests As DataTable
    '        Dim l_dt_Transmittals As DataTable

    '        Dim i As Integer
    '        Dim l_index As Integer

    '        If Me.DataGridInterest.Items.Count = 0 Then
    '            Exit Sub
    '        End If

    '        l_dt_Interests = ViewState("Interests")
    '        l_dt_Transmittals = ViewState("Transmittals")

    '        If Me.DataGridInterest.SelectedIndex <> -1 Then
    '            l_index = Me.DataGridInterest.SelectedIndex
    '        Else
    '            If l_dt_Interests.Rows.Count > 0 Then
    '                l_index = 0
    '                Me.DataGridInterest.SelectedIndex = 0
    '            Else
    '                l_index = -1
    '            End If
    '        End If

    '        If Not l_dt_Interests Is Nothing Then
    '            If l_dt_Interests.Rows.Count > 0 Then
    '                If l_dt_Interests.Rows.Count = 0 Or l_dt_Interests.Rows(l_index).Item("Amount") = 0 Then
    '                    ButtonPayItem.Enabled = False
    '                    Exit Sub
    '                End If

    '                If Session("InterestsSlctd") = False Then
    '                    If Not l_dt_Transmittals Is Nothing Then
    '                        For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                            If l_dt_Transmittals.Rows(i).Item("Slctd") = True And l_dt_Transmittals.Rows(i).Item("Balance") > 0 Then
    '                                l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")
    '                            End If
    '                        Next
    '                    End If
    '                End If

    '                Dim l_double_OldInterest As Double
    '                Dim l_double_InterestAvail As Double
    '                If Not l_dt_Transmittals Is Nothing Then
    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        l_double_OldInterest += l_double_OldInterest + (l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest"))
    '                    Next
    '                End If
    '                Dim flg As Boolean
    '                flg = False
    '                If Session("InterestsSlctd") = False Then
    '                    l_double_InterestAvail = l_dt_Interests.Rows(l_index).Item("Amount") - l_double_OldInterest

    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        If l_dt_Transmittals.Rows(i).Item("Slctd") = True Then
    '                            flg = True
    '                            Exit For
    '                        End If
    '                    Next


    '                    If flg = False Then  'If none of the Records is selected then select all
    '                        For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = True
    '                        Next
    '                    End If


    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        If l_dt_Transmittals.Rows(i).Item("Slctd") = True Then 'SCAN
    '                            If l_dt_Transmittals.Rows(i).Item("Balance") > 0 Then
    '                                l_dt_Transmittals.Rows(i).Item("Balance") = l_dt_Transmittals.Rows(i).Item("AmtDue") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("TotAppliedInterest")

    '                                If l_double_InterestAvail >= l_dt_Transmittals.Rows(i).Item("Balance") Then
    '                                    l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")), 2)
    '                                Else
    '                                    l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = Math.Round(l_double_InterestAvail + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")), 2)
    '                                End If

    '                                l_dt_Transmittals.Rows(i).Item("Balance") = Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance")) - Convert.ToDecimal((l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest"))), 2)

    '                                l_double_InterestAvail = l_double_InterestAvail - (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")))

    '                            End If
    '                            If (l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest") = 0) And (l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit") = 0) And (l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts") = 0) Then
    '                                l_dt_Transmittals.Rows(i).Item("Slctd") = False
    '                            End If

    '                        End If
    '                    Next 'END SCAN
    '                    Session("InterestsSlctd") = True
    '                    ButtonPayItem.Enabled = True
    '                Else
    '                    Session("InterestsSlctd") = False

    '                    If Session("RcptsSlctd") = False And Session("CreditsSlctd") = False Then
    '                        ButtonPayItem.Enabled = False
    '                    End If
    '                    l_double_InterestAvail = l_dt_Interests.Rows(l_index).Item("Amount")
    '                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
    '                        l_dt_Transmittals.Rows(i).Item("Balance") = l_dt_Transmittals.Rows(i).Item("Balance") + (l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest"))

    '                        If (l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit") = 0) And (l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts") = 0) Then
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = False
    '                        Else
    '                            l_dt_Transmittals.Rows(i).Item("Slctd") = True
    '                        End If
    '                        l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") = l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")
    '                    Next
    '                End If

    '                Dim l_double_AmtRcpts As Double
    '                Dim l_double_AmtBalance As Double
    '                Dim l_double_AmtInterest As Double
    '                For i = 0 To l_dt_Transmittals.Rows.Count - 1

    '                    l_double_AmtRcpts = l_double_AmtRcpts + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts"))
    '                    l_double_AmtBalance = l_double_AmtBalance + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("Balance"))
    '                    l_double_AmtInterest = l_double_AmtInterest + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest"))

    '                Next
    '                If l_double_AmtRcpts <> 0 Then
    '                    TextBoxTotalReceipts.Text = l_double_AmtRcpts
    '                Else
    '                    TextBoxTotalReceipts.Text = 0
    '                End If
    '                If l_double_AmtBalance <> 0 Then
    '                    TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtBalance))
    '                Else
    '                    TextBoxTotalBalance.Text = 0
    '                End If

    '                If l_double_AmtInterest <> 0 Then
    '                    TextBoxTotalInterest.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtInterest))
    '                Else
    '                    TextBoxTotalInterest.Text = 0
    '                End If

    '                TextBoxCredits.Text = Session("amtStartCredit")
    '                TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtBalance))
    '                DataGridTransmit.DataSource = l_dt_Transmittals
    '                DataGridTransmit.DataBind()

    '                ViewState("Transmittals") = l_dt_Transmittals

    '            End If
    '        Else
    '            ButtonPayItem.Enabled = False
    '        End If 'For l_dt_Interests Is Nothing 
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub

    Private Sub ButtonPayItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPayItem.Click
        Dim l_bool_ServiceUpdate As Boolean
        Dim l_LoggedBatchID As Int64
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If Page.IsValid Then


                'Added by Ashish on 26-Sep-2008 ,put validation funded date can not empty ,Start
                Dim minFundedDateRange As DateTime
                Dim maxFundedDateRange As DateTime
                Dim l_DateTimeFundedDate As DateTime
                'Added by Ashish on 02-Dec-2008 ,put validation funded date within business month ,Start
                Dim l_string_AcctDate As String
                ' Dim l_ds_AcctDate As DataSet
                Dim l_DateTime_AcctDate As DateTime
                Dim l_DateTime_CurrentDate As DateTime

                'Set Current Date
                l_DateTime_CurrentDate = System.DateTime.Now.Date
                'Get the accounting date 
                'l_ds_AcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
                'l_string_AcctDate = l_ds_AcctDate.Tables(0).Rows(0).Item(0).ToString()
                l_string_AcctDate = ViewState("CurrentAccountingDate")
                If Not l_string_AcctDate.Equals(String.Empty) Then
                    l_DateTime_AcctDate = Convert.ToDateTime(l_string_AcctDate).Date
                Else
                    l_DateTime_AcctDate = System.DateTime.Now.Date
                End If

                If (l_DateTime_AcctDate.Year = l_DateTime_CurrentDate.Year And l_DateTime_AcctDate.Month < l_DateTime_CurrentDate.Month) Or l_DateTime_AcctDate.Year < l_DateTime_CurrentDate.Year Then
                    minFundedDateRange = l_DateTime_AcctDate.AddDays(-(l_DateTime_AcctDate.Day - 1))
                    maxFundedDateRange = l_DateTime_AcctDate
                Else
                    minFundedDateRange = l_DateTime_CurrentDate.AddDays(-(l_DateTime_CurrentDate.Day - 1))
                    maxFundedDateRange = l_DateTime_CurrentDate

                End If



                'minFundedDateRange = System.DateTime.Now.Date.AddDays(-(System.DateTime.Now.Date.Day - 1))
                'maxFundedDateRange = System.DateTime.Now.Date
                'Added by Ashish on 02-Dec-2008 ,put validation funded date within business month ,End
                If Not DateusercontrolFundedDate.Text.Trim() = "" Then
                    l_DateTimeFundedDate = CType(DateusercontrolFundedDate.Text, DateTime).Date
                    If l_DateTimeFundedDate < minFundedDateRange Or l_DateTimeFundedDate > maxFundedDateRange Then
                        If minFundedDateRange.Month = l_DateTime_CurrentDate.Month Then
                            MessageBox.Show(Me.PlaceHolderMessageBox, "YMCA-YRS", "Invalid Funded date..Funded Date should be between first day of current business month to today's date.", MessageBoxButtons.Stop)
                        Else
                            MessageBox.Show(Me.PlaceHolderMessageBox, "YMCA-YRS", "Invalid Funded date..Funded Date should be within the current business month.", MessageBoxButtons.Stop)
                        End If

                        Exit Sub
                    End If
                Else
                    MessageBox.Show(Me.PlaceHolderMessageBox, "YMCA-YRS", "Funded Date can not be blank.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Added by Ashish on 26-Sep-2008 ,put validation funded date can not empty ,Start
                'Changed by Shubhrata Dec 22nd YREN 3006 To maintain transactions  

                Dim l_dt_Transmittals As DataTable
                Dim l_dt_Receipts As DataTable
                Dim l_dt_Interest As DataTable
                'Commented by Ashish on 02-Dec-2008, for puting validation, funded date within business month ,Start
                'Dim l_string_AcctDate As String
                'Dim l_ds_AcctDate As DataSet
                'Get the accounting date 
                ' l_ds_AcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
                'l_string_AcctDate = l_ds_AcctDate.Tables(0).Rows(0).Item(0).ToString()
                'Commented by Ashish on 02-Dec-2008, for puting validation, funded date within business month ,End
                Dim l_double_TotalPaid As Double
                Dim l_double_InterestPaid As Double
                Dim l_string_UniqueId As String
                'Post the payment to the Transmittals
                l_double_TotalPaid = 0
                'l_double_InterestPaid = 0

                l_dt_Transmittals = Viewstate("Transmittals")
                l_dt_Receipts = ViewState("Receipts")
                'l_dt_Interest = ViewState("Interests")

                Dim l_double_TotAppliedRcpts As Double
                Dim l_double_TotAvail As Double
                Dim l_double_TotCredit As Double
                'Dim l_double_TotInterest As Double
                Dim l_string_TransmittalsId As New StringBuilder



                Dim i As Integer

                'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                'commented by Ashish Start
                ''Start of Code Add by Hafiz on 27Jan06
                'Dim l_IntIndexInterest As Integer
                'l_IntIndexInterest = Me.DataGridInterest.SelectedIndex
                'commented by Ashish End
                'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest 
                'Added by Ashish 0n 25-Sep-2008
                Dim l_IntIndexReceipt As Integer = -1
                If Not DataGridReceipts Is Nothing Then
                    If DataGridReceipts.Items.Count > 0 Then
                        l_IntIndexReceipt = Me.DataGridReceipts.SelectedIndex
                    End If

                End If
                'End of Code Add by Hafiz on 27Jan06

                If Not l_dt_Transmittals Is Nothing Then
                    For i = 0 To l_dt_Transmittals.Rows.Count - 1
                        If Convert.ToBoolean(l_dt_Transmittals.Rows(i)("Slctd")) = True Then
                            l_double_TotAppliedRcpts += l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")

                            l_double_TotCredit += l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                            'l_double_TotInterest += l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")
                            'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                            'Added by Ashish 29-Sep-2008,Start
                            If Math.Round(Convert.ToDecimal(l_dt_Transmittals.Rows(i)("Balance")), 2) = 0 Then
                                l_dt_Transmittals.Rows(i)("FundedDate") = Me.DateusercontrolFundedDate.Text
                                l_dt_Transmittals.Rows(i)("dtmPaidDate") = Me.DateusercontrolFundedDate.Text

                            End If
                            'Added by Ashish 29-Sep-2008,End

                        End If
                    Next

                    If Not l_dt_Receipts Is Nothing Then
                        If l_dt_Receipts.Rows.Count > 0 And l_IntIndexReceipt > -1 Then
                            If l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount") > 0 Then
                                l_double_TotAvail = l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount")
                            Else
                                l_double_TotAvail = 0
                            End If
                        End If
                    End If

                    If l_double_TotAppliedRcpts = 0 And l_double_TotCredit = 0 Then
                        ButtonPayItem.Enabled = False
                        Exit Sub
                    End If

                    'Commented by Ashish 29-Jan-2009
                    'Dim l_Dataset As New DataSet
                    'Dim l_datatable As New DataTable
                    Dim l_dsLoanPersonalDetails As DataSet
                    Dim l_bool_flag As Boolean

                    Dim l_double_PayAmount As Double
                    Dim l_string_Output As String
                    l_bool_flag = True

                    Dim l_datarow_Receipts As DataRow
                    'Dim l_datarow_Interests As DataRow
                    If l_IntIndexReceipt >= 0 Then
                        l_datarow_Receipts = l_dt_Receipts.Rows(l_IntIndexReceipt)
                    Else
                        l_datarow_Receipts = Nothing
                    End If
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start 

                    'If l_IntIndexInterest >= 0 Then
                    '    l_datarow_Interests = l_dt_Interest.Rows(l_IntIndexInterest)
                    'Else
                    '    l_datarow_Interests = Nothing
                    'End If


                    l_dsLoanPersonalDetails = YMCARET.YmcaBusinessObject.CashApplicationBOClass.SaveTransmittals(l_dt_Transmittals, l_datarow_Receipts, l_double_TotAppliedRcpts, l_string_AcctDate, l_bool_ServiceUpdate, l_LoggedBatchID)

                    ' this part will do email sending. 
                    If Not l_dsLoanPersonalDetails Is Nothing Then
                        'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                        'Dim dtIDMFTList As New DataTable 'AA:04.27.2016 YRS-AT-2830:Added to copy the files in IDM
                        'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                        If l_dsLoanPersonalDetails.Tables.Count > 0 Then
                            ' email will be sent if the last remaining installment of the loan if paid i.e loan is re-paid in full
                            If l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails").Rows.Count > 0 Then
                                Try

                                    'start:AA:04.12.2016 YRS-AT-2830:Changed to call from common function where it will close loan and send email 
                                    'SendMail(l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"))

                                    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                                    'Call New LoanClass().SendLoanPaidClosedMail(l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"), dtIDMFTList)
                                    Call New LoanClass().SendLoanPaidClosedMail(l_dsLoanPersonalDetails.Tables("LoanpaidPersonDetails"))
                                    'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                                    'end:AA:04.12.2016 YRS-AT-2830:Changed to call from common function where it will close loan and send email 
                                Catch
                                    Dim AlertScript As String = "<script language='javascript'>" & _
                                             "alert('There was a problem encountered while sending mail');</script>"

                                    ' Add the JavaScript code to the page.
                                    Page.RegisterStartupScript("AlertScript1", AlertScript)
                                End Try

                            End If
                            'Email sent for defaulted loan
                            If l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails").Rows.Count > 0 Then
                                Try
                                    'Start: Bala: YRS-AT-2642: Remove ssno from sending email.
                                    'SendLoanDefaultedMail(l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"))
                                    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                                    'Call New LoanClass().SendLoanDefaultedClosedMail(l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"), dtIDMFTList) 'AA:04.12.2016 YRS-AT-2830:Changed to copy idm the file
                                    Call New LoanClass().SendLoanDefaultedClosedMail(l_dsLoanPersonalDetails.Tables("LoanDefaultedPersonDetails"))
                                    'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                                    'End: Bala: YRS-AT-2642: Remove ssno from sending email.
                                Catch
                                    Dim AlertScript As String = "<script language='javascript'>" & _
                                             "alert('There was a problem encountered while sending mail');</script>"

                                    ' Add the JavaScript code to the page.
                                    Page.RegisterStartupScript("AlertScript2", AlertScript)
                                End Try

                            End If

                            'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
                            ''start:AA:04.27.2016 YRS-AT-2830:Added below lines to copy the files in the IDM 
                            'If HelperFunctions.isNonEmpty(dtIDMFTList) Then
                            '    Session("FTFileList") = dtIDMFTList

                            '    'Call the ASPX to copy the file.
                            '    Dim popupScript As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp_5', " & _
                            '    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"

                            '    ClientScript.RegisterClientScriptBlock(GetType(Page), "PopupScript5", popupScript, True)

                            'End If
                            ''End:AA:04.27.2016 YRS-AT-2830:Added below lines to copy the files in the IDM 
                            'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.

                        End If
                    End If
                    'Email sent when service update failed
                    If Not l_bool_ServiceUpdate Then
                        Try
                            SendServiceFailedMail(l_LoggedBatchID)
                        Catch
                            Dim AlertScript As String = "<script language='javascript'>" & _
                                     "alert('There was a problem encountered while sending service update failed mail');</script>"

                            ' Add the JavaScript code to the page.
                            Page.RegisterStartupScript("AlertServiceMail", AlertScript)
                        End Try

                    End If

                    PopulateValues()

                End If  ' For l_dt_Transmittals is nothing

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "ButtonPayItem_Click", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Public Sub PopulateValues()
        Dim l_string_YmcaId As String
        Dim l_dataset_YmcaTransmittals As DataSet
        Dim l_dataset_YmcaReceipts As DataSet
        'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start 
        'Dim l_dataset_YmcaInterests As DataSet
        'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
        Dim i As Integer
        Try
            l_string_YmcaId = Session("YmcaId")
            'Added by Ashish on 02-Dec-2008 ,funded date should be within the current month, Start
            Dim l_string_AcctDate As String
            Dim l_ds_AcctDate As DataSet
            Dim l_DateTime_AcctDate As DateTime
            Dim l_DateTime_CurrentDate As DateTime

            'Set Current Date
            l_DateTime_CurrentDate = System.DateTime.Now.Date
            'Get the accounting date 
            l_ds_AcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()
            l_string_AcctDate = l_ds_AcctDate.Tables(0).Rows(0).Item(0).ToString()
            If Not l_string_AcctDate.Equals(String.Empty) Then
                l_DateTime_AcctDate = Convert.ToDateTime(l_string_AcctDate).Date
                ViewState("CurrentAccountingDate") = l_string_AcctDate
            Else
                l_DateTime_AcctDate = System.DateTime.Now.Date
                ViewState("CurrentAccountingDate") = System.DateTime.Now.Date.ToString()
            End If

            If (l_DateTime_AcctDate.Year = l_DateTime_CurrentDate.Year And l_DateTime_AcctDate.Month < l_DateTime_CurrentDate.Month) Or l_DateTime_AcctDate.Year < l_DateTime_CurrentDate.Year Then

                Me.DateusercontrolFundedDate.Text = l_DateTime_AcctDate.Date.ToString("MM/dd/yyyy")
            Else

                Me.DateusercontrolFundedDate.Text = l_DateTime_CurrentDate.Date.ToString("MM/dd/yyyy")

            End If
            'Added by Ashish on 02-Dec-2008 ,funded date should be within the current month, End

            'Added by Ashish on 25-Sep-2008 ,Remove date control from grid and put out side
            'Commented by Ashish on 02-Dec-2008
            ' Me.DateusercontrolFundedDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy")
            Me.DateusercontrolFundedDate.RequiredDate = True
            Me.DateusercontrolFundedDate.RequiredValidatorErrorMessage = "Please enter funded date."
            'Calling BO Classes to get the data for Transmittals,Receipts,Interests

            l_dataset_YmcaTransmittals = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaTransmittals(l_string_YmcaId)
            l_dataset_YmcaReceipts = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaReceipts(l_string_YmcaId)
            'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
            'l_dataset_YmcaInterests = YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaInterest(l_string_YmcaId)

            If Not l_dataset_YmcaTransmittals Is Nothing Then
                'If l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows.Count > 0 Then
                'Creating the table Transmittals from the YmcaTransmittls datatable
                Dim l_dt_Transmittals As New DataTable
                l_dt_Transmittals.Columns.Add("Slctd")
                l_dt_Transmittals.Columns.Add("UniqueId")
                l_dt_Transmittals.Columns.Add("YmcaId")
                l_dt_Transmittals.Columns.Add("TransmittalNo")
                l_dt_Transmittals.Columns.Add("TransmittalSourceCode")
                l_dt_Transmittals.Columns.Add("TransmittalDate")
                l_dt_Transmittals.Columns.Add("AmtDue", GetType(Double))
                l_dt_Transmittals.Columns.Add("AmtPaid", GetType(Double))
                l_dt_Transmittals.Columns.Add("AmtCredit", GetType(Double))
                l_dt_Transmittals.Columns.Add("TotAppliedRcpts", GetType(Double))
                l_dt_Transmittals.Columns.Add("TotAppliedCredit", GetType(Double))
                'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                'l_dt_Transmittals.Columns.Add("TotAppliedInterest", GetType(Double))
                l_dt_Transmittals.Columns.Add("OrgAppliedCredit", GetType(Double))
                l_dt_Transmittals.Columns.Add("OrgAppliedRcpts", GetType(Double))
                'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                'l_dt_Transmittals.Columns.Add("OrgAppliedInterest", GetType(Double))
                l_dt_Transmittals.Columns.Add("Balance", GetType(Double))
                l_dt_Transmittals.Columns.Add("FundedDate", GetType(String))
                l_dt_Transmittals.Columns("FundedDate").DefaultValue = String.Empty
                l_dt_Transmittals.Columns.Add("dtmPaidDate", GetType(String))
                l_dt_Transmittals.Columns("dtmPaidDate").DefaultValue = String.Empty

                Dim i_counter As Integer
                For i_counter = 0 To l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows.Count - 1
                    ' l_dt_Transmittals.ImportRow(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter))
                    Dim dr As DataRow
                    dr = l_dt_Transmittals.NewRow()
                    dr.Item("Slctd") = False
                    dr.Item("UniqueId") = l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("UniqueId")
                    dr.Item("YmcaId") = l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("YmcaId")
                    dr.Item("TransmittalNo") = l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("TransmittalNo")
                    dr.Item("TransmittalSourceCode") = l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("SourceCode")
                    dr.Item("TransmittalDate") = l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("TransmittalDate")
                    dr.Item("AmtDue") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AmtDue")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AmtDue"))
                    dr.Item("AmtPaid") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AmtPaid")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AmtPaid"))
                    dr.Item("AmtCredit") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AmtCredits")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AmtCredits"))
                    dr.Item("TotAppliedRcpts") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedReceipts")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedReceipts"))
                    dr.Item("TotAppliedCredit") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedCredit")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedCredit"))
                    'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                    ' dr.Item("TotAppliedInterest") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedInterest")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedInterest"))
                    dr.Item("OrgAppliedCredit") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedCredit")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedCredit"))
                    dr.Item("OrgAppliedRcpts") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedReceipts")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedReceipts"))
                    'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                    'dr.Item("OrgAppliedInterest") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedInterest")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("AppliedInterest"))
                    dr.Item("Balance") = IIf(IsDBNull(l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("Balance")), 0, l_dataset_YmcaTransmittals.Tables("YmcaTransmittals").Rows(i_counter).Item("Balance"))
                    l_dt_Transmittals.Rows.Add(dr)
                Next
                'For i_counter = 0 To l_dt_Transmittals.Rows.Count - 1
                '    l_dt_Transmittals.Rows(i_counter).Item("AmtDue") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("AmtDue")), 0, l_dt_Transmittals.Rows(i_counter).Item("AmtDue"))
                '    l_dt_Transmittals.Rows(i_counter).Item("AmtPaid") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("AmtPaid")), 0, l_dt_Transmittals.Rows(i_counter).Item("AmtPaid"))
                '    l_dt_Transmittals.Rows(i_counter).Item("AmtCredit") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("AmtCredit")), 0, l_dt_Transmittals.Rows(i_counter).Item("AmtCredit"))
                '    l_dt_Transmittals.Rows(i_counter).Item("Balance") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("Balance")), 0, l_dt_Transmittals.Rows(i_counter).Item("Balance"))
                '    l_dt_Transmittals.Rows(i_counter).Item("TotAppliedCredit") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("TotAppliedCredit")), 0, l_dt_Transmittals.Rows(i_counter).Item("TotAppliedCredit"))
                '    l_dt_Transmittals.Rows(i_counter).Item("TotAppliedRcpts") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("TotAppliedRcpts")), 0, l_dt_Transmittals.Rows(i_counter).Item("TotAppliedRcpts"))
                '    l_dt_Transmittals.Rows(i_counter).Item("TotAppliedInterest") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("TotAppliedInterest")), 0, l_dt_Transmittals.Rows(i_counter).Item("TotAppliedInterest"))
                '    l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit")), 0, l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit"))
                '    l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts")), 0, l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts"))
                '    l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest") = IIf(IsDBNull(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest")), 0, l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest"))
                'Next
                Dim l_double_AmtDue As Double
                Dim l_double_AmtPaid As Double
                Dim l_double_AmtRcpts As Double
                Dim l_double_AmtCredit As Double
                Dim l_double_AmtBalance As Double
                'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                'Dim l_double_AmtInterest As Double
                For i_counter = 0 To l_dt_Transmittals.Rows.Count - 1
                    l_dt_Transmittals.Rows(i_counter).Item("Slctd") = False
                    l_double_AmtDue = l_double_AmtDue + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("AmtDue"))
                    l_double_AmtPaid = l_double_AmtPaid + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit"))
                    l_double_AmtRcpts = l_double_AmtRcpts + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedRcpts"))
                    l_double_AmtCredit = l_double_AmtCredit + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedCredit"))
                    l_double_AmtBalance = l_double_AmtBalance + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("Balance"))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                    'l_double_AmtInterest = l_double_AmtInterest + Convert.ToDecimal(l_dt_Transmittals.Rows(i_counter).Item("OrgAppliedInterest"))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                Next
                'anita and shubhrata date 21 apr
                ' TextBoxTotalAmount.Text = l_double_AmtDue
                'TextBoxTotalAmount.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtDue))

                'TextBoxTotalReceipts.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtRcpts))
                'TextBoxTotalCredits.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtCredit))
                'TextBoxTotalInterest.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtInterest))
                'TextBoxTotalBalance.Text = FormatCurrency(Convert.ToDecimal(l_double_AmtBalance))

                TextBoxTotalAmount.Text = String.Format("{0:N}", l_double_AmtDue)
                TextBoxTotalReceipts.Text = String.Format("{0:N}", l_double_AmtRcpts)
                TextBoxTotalCredits.Text = String.Format("{0:N}", l_double_AmtCredit)
                'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
                'TextBoxTotalInterest.Text = String.Format("{0:N}", l_double_AmtInterest)
                TextBoxTotalBalance.Text = String.Format("{0:N}", l_double_AmtBalance)
                'anita and shubhrata date 21 apr
                Session("AmtPaid") = l_double_AmtPaid
                Session("RcptsSlctd") = False
                Session("CreditsSlctd") = False
                'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start 
                'Session("InterestsSlctd") = False
                'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End 
                Me.ButtonPayItem.Enabled = False

                Me.DataGridTransmit.DataSource = l_dt_Transmittals
                Me.DataGridTransmit.DataBind()

                'code add start by hafiz on 27Feb06
                If l_dt_Transmittals.Rows.Count > 0 Then
                    DataGridTransmit.SelectedIndex = 0

                    Dim l_CheckBox As CheckBox
                    Dim l_datarow As DataRow
                    l_CheckBox = DataGridTransmit.Items(0).FindControl("CheckBoxSelect")
                    l_datarow = l_dt_Transmittals.Rows(0)

                    If (Not l_CheckBox Is Nothing) And (Not l_datarow Is Nothing) Then
                        l_CheckBox.Checked = True
                        l_datarow("Slctd") = True
                    End If
                End If
                'code add end by hafiz on 27Feb06

                ViewState("Transmittals") = l_dt_Transmittals
                Session("OriginalTramittals") = l_dt_Transmittals

                'Getting data into the credit Textbox
                Dim l_double_Credits As Double

                l_double_Credits = Convert.ToDecimal(YMCARET.YmcaBusinessObject.CashApplicationBOClass.LookUpYmcaCredit(l_string_YmcaId))
                ' TextBoxCredits.Text = FormatCurrency(Convert.ToDecimal(l_double_Credits))
                TextBoxCredits.Text = String.Format("{0:N}", l_double_Credits)
                Session("amtStartCredit1") = l_double_Credits   'Ashutosh

                Session("amtStartCredit") = l_double_Credits
                Session("CreditBalance") = l_double_Credits
                'End If
            End If

            'Binding The Receipts grid
            If Not l_dataset_YmcaReceipts Is Nothing Then
                If l_dataset_YmcaReceipts.Tables("YmcaReceipts").Rows.Count > 0 Then
                    Me.DataGridReceipts.DataSource = l_dataset_YmcaReceipts
                    Me.DataGridReceipts.DataBind()
                    'code add start by hafiz on 27Feb06
                    If l_dataset_YmcaReceipts.Tables(0).Rows.Count > 0 Then
                        Me.DataGridReceipts.SelectedIndex = 0

                        For i = 0 To Me.DataGridReceipts.Items.Count - 1
                            Dim l_button_Select As ImageButton
                            l_button_Select = DataGridReceipts.Items(i).FindControl("Imagebutton1")
                            If Not l_button_Select Is Nothing Then
                                If i = DataGridReceipts.SelectedIndex Then
                                    l_button_Select.ImageUrl = "images\selected.gif"
                                Else
                                    l_button_Select.ImageUrl = "images\select.gif"
                                End If
                            End If
                        Next
                    End If
                    'code add end by hafiz on 27Feb06

                    ViewState("Receipts") = l_dataset_YmcaReceipts.Tables(0)
                Else

                    Me.DataGridReceipts.DataSource = l_dataset_YmcaReceipts
                    Me.DataGridReceipts.DataBind()
                End If

            End If
            'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 
            'Commented code Start
            'Binding the Interest Grid
            'If Not l_dataset_YmcaInterests Is Nothing Then
            '    If l_dataset_YmcaInterests.Tables("YmcaInterest").Rows.Count > 0 Then
            '        Me.DataGridInterest.DataSource = l_dataset_YmcaInterests
            '        Me.DataGridInterest.DataBind()
            '        'code add start by hafiz on 27Feb06
            '        If l_dataset_YmcaInterests.Tables(0).Rows.Count > 0 Then
            '            Me.DataGridInterest.SelectedIndex = 0

            '            For i = 0 To Me.DataGridInterest.Items.Count - 1
            '                Dim l_button_Select As ImageButton
            '                'Ashutosh Id changed by ashutosh of Image Button on 17-July-O6
            '                l_button_Select = DataGridInterest.Items(i).FindControl("ImageButtonInterest1")
            '                If Not l_button_Select Is Nothing Then
            '                    If i = DataGridInterest.SelectedIndex Then
            '                        l_button_Select.ImageUrl = "images\selected.gif"
            '                    Else
            '                        l_button_Select.ImageUrl = "images\select.gif"
            '                    End If
            '                End If
            '            Next
            '        End If
            '        'code add end by hafiz on 27Feb06
            '        ViewState("Interests") = l_dataset_YmcaInterests.Tables(0)
            '    Else
            '        Me.DataGridInterest.DataSource = l_dataset_YmcaInterests
            '        Me.DataGridInterest.DataBind()
            '    End If
            'End If
            'commented code End

            TabStripCashApplication.SelectedIndex = 1
            MultiPageCashApplication.SelectedIndex = 1

        Catch
            Throw
            '    Dim l_String_Exception_Message As String
            '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            '    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub DataGridYmca_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYmca.ItemDataBound
        Try
            e.Item.Cells(1).Visible = False
            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(2).Text = "YMCA No."
                e.Item.Cells(3).Text = "YMCA Name"
                e.Item.Cells(4).Text = "City"
                e.Item.Cells(5).Text = "State"
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "DataGridYmca_ItemDataBound", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub DataGridTransmit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridTransmit.ItemDataBound
        'Commented by ashutosh on 15-Jul-06 Reason:Now grid is binded through BoundColoumn
        'Try
        '    e.Item.Cells(1).Visible = False
        '    e.Item.Cells(2).Visible = False
        '    e.Item.Cells(3).Visible = False
        '    e.Item.Cells(5).Visible = False
        '    e.Item.Cells(8).Visible = False
        '    e.Item.Cells(9).Visible = False
        '    e.Item.Cells(13).Visible = False
        '    e.Item.Cells(14).Visible = False
        '    e.Item.Cells(15).Visible = False
        '    If e.Item.ItemType = ListItemType.Header Then
        '        e.Item.Cells(0).Text = "Slct"
        '        e.Item.Cells(4).Text = "Transmittal No."
        '        e.Item.Cells(6).Text = "Transmittal Date"
        '        e.Item.Cells(7).Text = "Total Amount"
        '        e.Item.Cells(10).Text = "Receipts Applied"
        '        e.Item.Cells(11).Text = "CR Applied"
        '        e.Item.Cells(12).Text = "Pymt Interest"
        '        e.Item.Cells(16).Text = "Balance"

        '    End If
        '    'anita and shubhrata date apr19
        '    If e.Item.ItemType <> ListItemType.Header Then

        '        If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Then

        '            Dim l_decimal_try As Decimal
        '            l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
        '            e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

        '            l_decimal_try = Convert.ToDecimal(e.Item.Cells(16).Text)
        '            e.Item.Cells(16).Text = FormatCurrency(l_decimal_try)

        '            l_decimal_try = Convert.ToDecimal(e.Item.Cells(12).Text)
        '            e.Item.Cells(12).Text = FormatCurrency(l_decimal_try)

        '            l_decimal_try = Convert.ToDecimal(e.Item.Cells(10).Text)
        '            e.Item.Cells(10).Text = FormatCurrency(l_decimal_try)

        '            l_decimal_try = Convert.ToDecimal(e.Item.Cells(11).Text)
        '            e.Item.Cells(11).Text = FormatCurrency(l_decimal_try)

        '        End If
        '    End If

        '    'anita and shubhrata date apr19
        '    If e.Item.ItemType <> ListItemType.Header Then
        '        e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
        '        e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
        '        e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
        '        e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
        '        e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Right
        '        e.Item.Cells(16).HorizontalAlign = HorizontalAlign.Right
        '    End If
        'Catch ex As Exception
        '    Throw
        '    'Dim l_String_Exception_Message As String
        '    'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        'End Try
        'Commented by Ashish on 25-Sep-2008  remove date control from grid ,Start
        'Start -Added By Ashish on 13 May 2008
        'Try
        '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
        '        Dim dateUserControl As DateUserControl
        '        dateUserControl = CType(e.Item.Cells(3).FindControl("DateusercontrolFundedDate"), DateUserControl)
        '        If Not dateUserControl Is Nothing Then
        '            If dateUserControl.Text = String.Empty Then
        '                dateUserControl.Enabled = False
        '                dateUserControl.EnabledDateRangeValidator = False
        '                dateUserControl.RequiredDate = False

        '            Else

        '                dateUserControl.RangeValidatorMinValue = System.DateTime.Now.Date.AddDays(-(System.DateTime.Now.Date.Day - 1)).ToString("MM/dd/yyyy")

        '                dateUserControl.RangeValidatorMaxValue = System.DateTime.Now.Date.ToString("MM/dd/yyyy")
        '                dateUserControl.RangeValidatorErrorMassege = "Funded Date should be between first day of current month to today date."
        '                dateUserControl.EnabledDateRangeValidator = True
        '                dateUserControl.RequiredDate = True
        '                dateUserControl.RequiredValidatorErrorMessage = "Please enter funded date."

        '            End If
        '        End If
        '    End If
        'Catch ex As Exception
        '    Dim l_String_Exception_Message As String
        '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        'End Try
        'End -Added By Ashish on 13 May 2008
        'Commented by Ashish on 25-Sep-2008  remove date control from grid ,End
    End Sub
    'anita and shubhrata 
    Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
        Try
            Dim n As String
            Dim m As String()
            Dim myNum As String
            'Changed by Ruchi on 7th March,2006
            Dim myDec As String
            'end of change
            Dim len As Integer
            Dim i As Integer
            Dim val As String
            If paramNumber = 0 Then
                val = 0
            Else
                n = paramNumber.ToString()
                m = (Math.Round(n * 100) / 100).ToString().Split(".")
                myNum = m(0).ToString()

                len = myNum.Length
                Dim fmat(len) As String
                For i = 0 To len - 1
                    fmat(i) = myNum.Chars(i)
                Next
                Array.Reverse(fmat)
                For i = 1 To len - 1
                    If i Mod 3 = 0 Then
                        fmat(i + 1) = fmat(i + 1) & ","
                    End If
                Next
                Array.Reverse(fmat)
                'start of change


                'end of change
                If m.Length = 1 Then
                    val = String.Join("", fmat) + ".00"
                Else
                    myDec = m(1).ToString
                    If myDec.Length = 1 Then
                        myDec = myDec + "0"
                    End If
                    val = String.Join("", fmat) + "." + myDec
                End If

            End If
            Dim tempBool As Boolean
            tempBool = val.StartsWith(",")
            If tempBool = True Then
                val = val.Substring(1)
            End If

            Return val

        Catch ex As Exception
            Return paramNumber
        End Try

    End Function


    ' anita and shubhrata



    Private Sub DataGridReceipts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridReceipts.ItemDataBound
        'Try
        '    e.Item.Cells(1).Visible = False
        '    e.Item.Cells(2).Visible = False
        '    e.Item.Cells(3).Visible = False
        '    e.Item.Cells(4).Visible = False
        '    ''e.Item.Cells(4).Visible = False
        '    e.Item.Cells(6).Visible = False
        '    e.Item.Cells(8).Visible = False
        '    e.Item.Cells(9).Visible = False
        '    e.Item.Cells(10).Visible = False
        '    e.Item.Cells(11).Visible = False

        '    If e.Item.ItemType = ListItemType.Header Then
        '        e.Item.Cells(5).Text = "Receipt Number"
        '        e.Item.Cells(7).Text = "Check Date"
        '        e.Item.Cells(12).Text = "Received Date"
        '        e.Item.Cells(13).Text = "Amount"
        '    End If

        '    'anita and shubhrata date apr19
        '    If e.Item.ItemType <> ListItemType.Header Then


        '        If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Then

        '            Dim l_decimal_try As Decimal
        '            l_decimal_try = Convert.ToDecimal(e.Item.Cells(13).Text)
        '            e.Item.Cells(13).Text = FormatCurrency(l_decimal_try)
        '        End If
        '    End If
        '    'anita and shubhrata date apr19


        '    If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
        '        e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
        '        Dim l_date As Date
        '        l_date = e.Item.Cells(7).Text
        '        e.Item.Cells(7).Text = l_date.ToShortDateString
        '        l_date = e.Item.Cells(12).Text
        '        e.Item.Cells(12).Text = l_date.ToShortDateString

        '    End If
        'Catch ex As Exception
        '    Dim l_String_Exception_Message As String
        '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        'End Try
    End Sub


    Private Sub DataGridReceipts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridReceipts.SelectedIndexChanged
        Dim l_dt_Transmittals As DataTable
        Dim l_index As Integer
        Dim l_bool_Flag As Boolean
        Dim l_dt_Receipts As DataTable
        Try
            l_dt_Receipts = ViewState("Receipts")

            viewstate("ReceiptAmount") = l_dt_Receipts.Rows(l_index).Item("Amount")
            Dim i As Integer
            For i = 0 To Me.DataGridReceipts.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridReceipts.Items(i).FindControl("Imagebutton1")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridReceipts.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            Next

            l_bool_Flag = False
            Dim l_AppliedRecipts As Double = 0
            Dim l_TextBoxTotalRecipts As Double = 0
            Dim l_ApliedBalance As Double = 0
            Dim l_TextBoxTotalBalance As Double = 0
            If Not Viewstate("Transmittals") Is Nothing Then
                l_dt_Transmittals = ViewState("Transmittals")

                For l_index = 0 To l_dt_Transmittals.Rows.Count - 1

                    l_AppliedRecipts = l_dt_Transmittals.Rows(l_index).Item("OrgAppliedRcpts")
                    l_dt_Transmittals.Rows(l_index).Item("TotAppliedRcpts") = l_AppliedRecipts
                    l_TextBoxTotalRecipts = l_TextBoxTotalRecipts + l_AppliedRecipts
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,Start
                    'l_ApliedBalance = Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("AmtDue")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedInterest"))
                    'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest ,End
                    'Added By Ashish ,Start
                    l_ApliedBalance = Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("AmtDue")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedCredit"))
                    'Added By Ashish ,End 
                    l_dt_Transmittals.Rows(l_index).Item("Balance") = l_ApliedBalance
                    l_TextBoxTotalBalance = l_TextBoxTotalBalance + l_ApliedBalance
                    'Commented on 25-Sep-2008 by Ashish remove date control from grid ,Start
                    'Added  on 12 May by Ashish ,Start
                    'If Not Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("Balance")) = 0 Then
                    '    Dim dgItemTemp As DataGridItem = DataGridTransmit.Items(l_index)
                    '    If Not dgItemTemp Is Nothing Then
                    '        Dim dtUserControlFundedDate As DateUserControl
                    '        dtUserControlFundedDate = CType(dgItemTemp.FindControl("DateusercontrolFundedDate"), DateUserControl)
                    '        If Not dtUserControlFundedDate Is Nothing Then
                    '            dtUserControlFundedDate.Text = String.Empty
                    '            l_dt_Transmittals.Rows(l_index).Item("FundedDate") = dtUserControlFundedDate.Text
                    '            l_dt_Transmittals.Rows(l_index).Item("dtmPaidDate") = dtUserControlFundedDate.Text

                    '            dtUserControlFundedDate.Enabled = False
                    '        End If
                    '    End If
                    'End If
                    'Added by on 12 May Ashish ,End
                    'Commented on 25-Sep-2008 by Ashish remove date control from grid ,End
                    'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,Start
                    If Not Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("Balance")) = 0 Then
                        l_dt_Transmittals.Rows(l_index).Item("FundedDate") = String.Empty
                        l_dt_Transmittals.Rows(l_index).Item("dtmPaidDate") = String.Empty
                    End If
                    'Added by Ashish on 25-Sep-2008 remove date controm from transmittal grid ,End
                Next


                TextBoxTotalReceipts.Text = String.Format("{0:N}", l_TextBoxTotalRecipts)
                TextBoxTotalBalance.Text = String.Format("{0:N}", Convert.ToDouble(TextBoxTotalBalance.Text))
                DataGridTransmit.DataSource = l_dt_Transmittals
                DataGridTransmit.DataBind()

                ViewState("Transmittals") = l_dt_Transmittals
                l_dt_Transmittals = Nothing
                'Added by Ashish ,Start
                If TextBoxTotalReceipts.Text = "0.00" And TextBoxTotalCredits.Text = "0.00" Then
                    ButtonPayItem.Enabled = False
                Else
                    ButtonPayItem.Enabled = True
                End If
                'Added by Ashish ,End

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("CashApplication" + "DataGridReceipts_SelectedIndexChanged", ex) 'AA :04.27.2016  YRS-AT-2830 : Added for logging the exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
	Sub RebindCheckBox_Transmittals()
		Dim l_dt_Transmittals As DataTable
		Dim i As Int16
		If Not Viewstate("Transmittals") Is Nothing Then
			l_dt_Transmittals = ViewState("Transmittals")
			Dim ck1 As CheckBox
			For i = 0 To l_dt_Transmittals.Rows.Count - 1
				If l_dt_Transmittals.Rows(i).Item("Slctd") = True Then
					ck1 = DataGridTransmit.Items(i).FindControl("CheckBoxSelect")
					ck1.Checked = True
				End If
			Next
		End If
		'ButtonPayItem.Enabled = True
		l_dt_Transmittals = Nothing

	End Sub

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub
	'Commented on 12 May by Ashish  changes according Phase IV-2 Remove Interest 
	'Commented Start
	'Private Sub DataGridInterest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridInterest.SelectedIndexChanged
	'    Dim l_dt_Transmittals As DataTable
	'    Dim l_index As Integer
	'    Dim l_bool_Flag As Boolean
	'    Try

	'        Dim i As Integer
	'        For i = 0 To Me.DataGridInterest.Items.Count - 1
	'            Dim l_button_Select As ImageButton
	'            'Ashutosh on 17-July-O6
	'            'Reason Chage the Id name of Image Button in Aspx Page Because Two buuton have same Id one in 
	'            'DataGridInterest image button Another in form imagebuttonInerest
	'            'Change DataGridInterest image button to ImageButtonInterest1 .
	'            l_button_Select = DataGridInterest.Items(i).FindControl("ImageButtonInterest1")
	'            If Not l_button_Select Is Nothing Then
	'                If i = DataGridInterest.SelectedIndex Then
	'                    l_button_Select.ImageUrl = "images\selected.gif"
	'                Else
	'                    l_button_Select.ImageUrl = "images\select.gif"
	'                End If
	'            End If
	'        Next

	'        'l_bool_Flag = False
	'        Dim l_AppliedInterest As Double = 0
	'        Dim l_TextBoxTotalInterest As Double = 0
	'        Dim l_ApliedBalance As Double = 0
	'        Dim l_TextBoxTotalBalance As Double = 0
	'        If Not Viewstate("Transmittals") Is Nothing Then
	'            l_dt_Transmittals = ViewState("Transmittals")

	'            For l_index = 0 To l_dt_Transmittals.Rows.Count - 1
	'                l_AppliedInterest = l_dt_Transmittals.Rows(l_index).Item("OrgAppliedInterest")
	'                l_dt_Transmittals.Rows(l_index).Item("TotAppliedInterest") = l_AppliedInterest
	'                l_TextBoxTotalInterest = l_TextBoxTotalInterest + l_AppliedInterest
	'                l_ApliedBalance = Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("AmtDue")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(l_index).Item("TotAppliedInterest"))
	'                l_dt_Transmittals.Rows(l_index).Item("Balance") = l_ApliedBalance
	'                l_TextBoxTotalBalance = l_TextBoxTotalBalance + l_ApliedBalance

	'            Next
	'            'TextBoxTotalInterest.Text = FormatCurrency(l_TextBoxTotalInterest)
	'            'TextBoxTotalBalance.Text = FormatCurrency(l_TextBoxTotalBalance)
	'            TextBoxTotalInterest.Text = String.Format("{0:N}", l_TextBoxTotalInterest)
	'            TextBoxTotalBalance.Text = String.Format("{0:N}", l_TextBoxTotalBalance)
	'            DataGridTransmit.DataSource = l_dt_Transmittals
	'            DataGridTransmit.DataBind()

	'            ViewState("Transmittals") = l_dt_Transmittals
	'            l_dt_Transmittals = Nothing
	'        End If
	'    Catch ex As Exception
	'        Dim l_String_Exception_Message As String
	'        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
	'        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
	'    End Try
	'End Sub
	'Commented End
	'Commented on 12 May by Ashish according changes Phase IV-2 Remove Interest 


	'Aparna Samala 05/09/2006


	Private Sub SendMail(ByVal l_Datatable As DataTable)
		Dim obj As MailUtil
		obj = New MailUtil
		Dim l_string_SSNo As String
		Dim l_string_Firstname As String
		Dim l_string_Middlename As String
		Dim l_string_LastName As String
		Dim l_string_FundIdNo As String
		Dim l_string_heading As String
        Dim l_string_details As StringBuilder 'Anudeep:24.12.2012 YRS 5.0-1717:Issue email when loan paid off 
		'By Ashutosh Patil as on 19-Apr-2007
		'Email will be used from the database instead of configuration file.
		'For Cash And Loans Category will be same i.e TDLoan
		Dim l_CashAppDataTable As New DataTable
		Dim l_DataRow As DataRow

		Dim drow As DataRow
		Dim l_datatable_persondetails As New DataTable
		l_datatable_persondetails = l_Datatable
		Try
            'Start: Anudeep:24.12.2012 YRS 5.0-1717:Issue email when loan paid off 
            l_string_details = New StringBuilder

            l_string_details.Append("You may now close the loan for following participants as the final payment of their TD Loan has been funded.")

            l_string_details.Append("<table width='50%' border='1' >")
            l_string_details.Append("<tr><td align='center'><B>Fund Id No.</B></td><td align='center'><B>First Name</B></td><td align='center'><B>Last Name</B></td></tr>")
            For Each drow In l_datatable_persondetails.Rows
                'l_string_details = l_string_details + drow("chrSSNo").ToString().PadRight(20, " ") + drow("chvLastName").ToString().PadRight(20, " ") + drow("chvFirstName").ToString().PadRight(20, " ") + drow("chvMiddleName").ToString().PadRight(20, " ") + drow("intFundIdNo").ToString() + ControlChars.CrLf
                'Changed by prasad 2012.02.28:YRS 5.0-1547 - loan payoff email not reporting multiple people

                l_string_details.Append("<tr>")
                l_string_details.Append("<td>" & drow("intFundIdNo").ToString() & "</td>")
                l_string_details.Append("<td>" & drow("chvFirstName").ToString() & "</td>")
                l_string_details.Append("<td>" & drow("chvLastName").ToString() & "</td>")
                l_string_details.Append("</tr>")

                'l_string_details += "Fund Id Number : " + drow("intFundIdNo").ToString() + ControlChars.CrLf + "First Name : " + drow("chvFirstName").ToString() + ControlChars.CrLf + "Last Name : " + drow("chvLastName").ToString() + ControlChars.CrLf + ControlChars.CrLf

                'Commented by Ashish
                'Exit For
                'l_string_SSNo = l_string_SSNo + drow("chrSSNo").ToString() + ControlChars.CrLf
                'l_string_LastName = l_string_LastName + drow("chvLastName").ToString() + ControlChars.CrLf
                'l_string_Firstname = l_string_Firstname + drow("chvFirstName").ToString() + ControlChars.CrLf
                'l_string_Middlename = l_string_Middlename + drow("chvMiddleName").ToString() + ControlChars.CrLf
                'l_string_FundIdNo = l_string_FundIdNo + drow("intFundIdNo").ToString() + ControlChars.CrLf

            Next
            l_string_details.Append("</table>")
            'END: Anudeep:24.12.2012 YRS 5.0-1717:Issue email when loan paid off 

            obj.MailCategory = "TDLoan"
            If obj.MailService = False Then Exit Sub

            'Anudeep:24.12.2012 YRS 5.0-1717:Issue email when loan paid off 
            obj.MailFormatType = Mail.MailFormat.Html

                ' Test Mode
                'Commentd by Ashish 29-Jan-2009
                'If l_str_msg.ToString = "Use_Default" Then
                '    obj.ToMail = obj.FromMail
                'End If

            obj.SendCc = ""

                'l_string_heading = "SSNo".PadRight(20, " ") + "First Name".PadRight(20, " ") + "Middle Name".PadRight(20, " ") + "Last Name".PadRight(20, " ") + "FundIDNo"
                '  obj.MailMessage = l_string_heading + ControlChars.CrLf + l_string_SSNo.PadRight(20, " ") + l_string_Firstname.PadRight(20, " ") + l_string_Middlename.PadRight(20, " ") + l_string_LastName.PadRight(20, " ") + l_string_FundIdNo

                'commented by Hafiz on 26/07/2007
                'obj.MailMessage = l_string_heading.PadRight(50, " ") + ControlChars.CrLf + ControlChars.Tab + ControlChars.Tab + l_string_details

            obj.MailMessage = l_string_details.ToString() ' added by Hafiz on 26/07/2007

                'obj.Subject = "" 'commented by hafiz on 26/07/2007

                'added by hafiz on 26/07/2007


            obj.Subject = "The Tax Deferred Loan for the following participant has been repaid in full."


                'obj.MailAttachments1 = Nothing
                'obj.MailAttachments2 = Nothing
                'obj.MailAttachments3 = Nothing

                'If Session("StringDestFilePath") <> "" Then
                '    obj.MailAttachments = Session("StringDestFilePath")
                'Else
                '    obj.MailAttachments = Nothing
                'End If

                obj.Send()
		Catch
            Throw
        End Try
    End Sub
    'Start: Bala: YRS-AT-2642: Following function is not required. Moved it to LoanClass()
	'Added By Ashish 29-Jan-2009 ,Start
    'Private Sub SendLoanDefaultedMail(ByVal l_Datatable As DataTable)

    '	' This method will be sent email if person defaluted for 
    '	Dim obj As MailUtil
    '	obj = New MailUtil
    '	Dim l_string_LoanDefaultedDetails As StringBuilder
    '	Dim drow As DataRow
    '	Dim l_datatable_persondetails As New DataTable
    '	l_datatable_persondetails = l_Datatable
    '	Try

    '		l_string_LoanDefaultedDetails = New StringBuilder
    '		For Each drow In l_datatable_persondetails.Rows
    '			l_string_LoanDefaultedDetails.Append("First Name : " + drow("chvFirstName").ToString() + ControlChars.CrLf + "Last Name : " + drow("chvLastName").ToString() + ControlChars.CrLf + "Ymca Name : " + drow("chvYmcaName").ToString() + ControlChars.CrLf + "Ymca Number : " + drow("chrYmcano").ToString() + ControlChars.CrLf + "Social Security No : " + drow("chrSSNo").ToString() + ControlChars.CrLf + "Fund Id Number : " + drow("intFundIdNo").ToString() + ControlChars.CrLf + ControlChars.CrLf)
    '		Next

    '		obj.MailCategory = "TDLoan_Defaulted"
    '		If obj.MailService = False Then Exit Sub

    '		obj.MailMessage = l_string_LoanDefaultedDetails.ToString()

    '		obj.Subject = "Tax Deferred Loan Defaulted."

    '		obj.Send()
    '	Catch
    '		Throw
    '	End Try
    'End Sub
    'End: Bala: YRS-AT-2642: Following function is not required. Moved it to LoanClass()
    'Added By Ashish 29-Jan-2009 ,End
    'Added By Ashish 17-July-2008 ,Start
	Private Sub SendServiceFailedMail(ByVal parameterLoggedId As Int64)
		Dim obj As MailUtil
		Try
			If parameterLoggedId > 0 Then

				obj = New MailUtil

				obj.MailCategory = "ADMIN"
				If obj.MailService = False Then Exit Sub

				obj.MailMessage = "ServiceTime and vesting update failed for BatchID :" + Convert.ToString(parameterLoggedId)

				obj.Subject = "ServiceTime and vesting update failed."

				obj.Send()
			End If
		Catch ex As Exception
			Throw ex
		End Try
	End Sub
	'Added By Ashish 17-July-2008 ,End

	'Shubhrata Dec 22nd Dec 06 YREN 3006--Have put the original code for ButtonpayClick in this method so that 
	'old code remains for reference.
	'Private Sub ButtonPayClick()
	'    Try
	'        Dim l_ds_AcctDate As DataSet
	'        Dim l_dt_Transmittals As DataTable
	'        Dim l_dt_Receipts As DataTable
	'        Dim l_dt_Interest As DataTable

	'        l_ds_AcctDate = YMCARET.YmcaBusinessObject.CashApplicationBOClass.GetAccountingDate()

	'        Dim l_string_AcctDate As String
	'        Dim l_string_Date As String
	'        l_string_AcctDate = l_ds_AcctDate.Tables(0).Rows(0).Item(0).ToString()
	'        l_string_Date = System.DateTime.Today()

	'        Dim l_double_TotalPaid As Double
	'        Dim l_double_InterestPaid As Double
	'        Dim l_string_UniqueId As String
	'        'Post the payment to the Transmittals
	'        l_double_TotalPaid = 0
	'        l_double_InterestPaid = 0

	'        l_dt_Transmittals = Viewstate("Transmittals")
	'        l_dt_Receipts = ViewState("Receipts")
	'        l_dt_Interest = ViewState("Interests")

	'        Dim l_double_TotAppliedRcpts As Double
	'        Dim l_double_TotAvail As Double
	'        Dim l_double_TotCredit As Double
	'        Dim l_double_TotInterest As Double
	'        Dim l_string_TransmittalsId As New StringBuilder



	'        Dim i As Integer

	'        'Start of Code Add by Hafiz on 27Jan06
	'        Dim l_IntIndexInterest As Integer
	'        l_IntIndexInterest = Me.DataGridInterest.SelectedIndex

	'        Dim l_IntIndexReceipt As Integer
	'        l_IntIndexReceipt = Me.DataGridReceipts.SelectedIndex
	'        'End of Code Add by Hafiz on 27Jan06

	'        If Not l_dt_Transmittals Is Nothing Then
	'            For i = 0 To l_dt_Transmittals.Rows.Count - 1
	'                l_double_TotAppliedRcpts += l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")

	'                l_double_TotCredit += l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")

	'                l_double_TotInterest += l_dt_Transmittals.Rows(i).Item("TotAppliedInterest") - l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")
	'            Next

	'            If Not l_dt_Receipts Is Nothing Then
	'                If l_dt_Receipts.Rows.Count > 0 And l_IntIndexReceipt > -1 Then
	'                    If l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount") > 0 Then
	'                        l_double_TotAvail = l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount")
	'                    Else
	'                        l_double_TotAvail = 0
	'                    End If
	'                End If
	'            End If

	'            If l_double_TotAppliedRcpts = 0 And l_double_TotCredit = 0 And l_double_TotInterest = 0 Then
	'                ButtonPayItem.Enabled = False
	'                Exit Sub
	'            End If


	'            Dim l_Dataset As New DataSet
	'            Dim l_datatable As New DataTable
	'            Dim l_bool_flag As Boolean

	'            Dim l_double_PayAmount As Double
	'            Dim l_string_Output As String
	'            l_bool_flag = True

	'            For i = 0 To l_dt_Transmittals.Rows.Count - 1

	'                If l_dt_Transmittals.Rows(i).Item("Slctd") = True Then
	'                    l_double_PayAmount = Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) + Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest"))

	'                    l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.UpdateYmcaTransmittals(l_dt_Transmittals.Rows(i).Item("UniqueId").ToString(), l_double_PayAmount)

	'                    If l_string_Output <> "0" Then
	'                        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                        Exit Sub
	'                    End If

	'                    'If credit is being applied create negative record for it!
	'                    If (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")) > 0) Then
	'                        l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.InsertYmcaCredit(l_dt_Transmittals.Rows(i).Item("YmcaId").ToString(), l_dt_Transmittals.Rows(i).Item("UniqueId").ToString(), -1 * Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedCredit") - l_dt_Transmittals.Rows(i).Item("OrgAppliedCredit")), l_string_Date, l_string_AcctDate)

	'                        If l_string_Output <> "0" Then
	'                            MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                            Exit Sub
	'                        End If
	'                    End If

	'                    If (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")) > 0) Then
	'                        l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.InsertYmcaAppliedRcpts(l_dt_Transmittals.Rows(i).Item("YmcaId").ToString(), l_dt_Receipts.Rows(l_IntIndexReceipt).Item("UniqueId").ToString(), l_dt_Transmittals.Rows(i).Item("UniqueId").ToString(), Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts") - l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")), l_string_Date)
	'                        If l_string_Output <> "0" Then
	'                            MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                            Exit Sub
	'                        End If
	'                    End If

	'                    l_double_TotalPaid = l_double_TotalPaid + (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedRcpts")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedRcpts")))
	'                    l_double_InterestPaid = l_double_InterestPaid + (Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("TotAppliedInterest")) - Convert.ToDecimal(l_dt_Transmittals.Rows(i).Item("OrgAppliedInterest")))


	'                    l_string_UniqueId = l_dt_Transmittals.Rows(i).Item("UniqueId").ToString()

	'                    'Aparna


	'                    l_Dataset = YMCARET.YmcaBusinessObject.CashApplicationBOClass.SelectPersonDetails(l_string_UniqueId)

	'                    If l_bool_flag = True Then
	'                        l_datatable = l_Dataset.Tables(0).Clone()
	'                        l_bool_flag = False
	'                    End If
	'                    If l_Dataset.Tables(0).Rows.Count > 0 Then
	'                        l_datatable.ImportRow(l_Dataset.Tables(0).Rows(0))
	'                        l_datatable.AcceptChanges()
	'                    End If

	'                    '    l_string_TransmittalsId.Append(l_string_UniqueId)
	'                    '  l_string_TransmittalsId.Append(",")
	'                    ' Session("TransmittalID") = l_string_TransmittalsId.ToString()

	'                End If ' Check Slctd = True
	'            Next


	'            If IsDBNull(l_string_UniqueId) Then
	'                l_string_UniqueId = ""
	'            End If

	'            'SELECT curTransmittals
	'            'LOCATE FOR UniqueId = l_string_UniqueId


	'            'Close out the Receipt if any used
	'            If l_double_TotAppliedRcpts > 0 Then
	'                l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.UpdateYmcaRcpts(l_dt_Receipts.Rows(l_IntIndexReceipt).Item("UniqueId").ToString(), l_string_UniqueId, l_string_Date, l_string_AcctDate)

	'                If l_string_Output <> "0" Then
	'                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                    Exit Sub
	'                End If
	'            End If

	'            'If total available payment from receipt is greater than the total used, create a credit 
	'            '  If ((l_double_TotAvail > l_double_TotalPaid) And l_double_TotAppliedRcpts <> 0) Then
	'            If Not l_dt_Receipts Is Nothing Then 'Code change by ashutosh on 03-Aug-06
	'                If l_dt_Receipts.Rows.Count > 0 Then
	'                    If Convert.ToDecimal(l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount")) > 0 And l_double_TotAppliedRcpts <> 0 Then
	'                        'l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.InsertYMCACreditsOvrPay(l_dt_Transmittals.Rows(0).Item("YmcaId").ToString(), l_string_UniqueId, (l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount") - l_double_TotalPaid), l_string_Date, l_string_AcctDate, l_dt_Receipts.Rows(l_IntIndexReceipt).Item("UniqueId").ToString())
	'                        l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.InsertYMCACreditsOvrPay(l_dt_Transmittals.Rows(0).Item("YmcaId").ToString(), l_string_UniqueId, (l_dt_Receipts.Rows(l_IntIndexReceipt).Item("Amount")), l_string_Date, l_string_AcctDate, l_dt_Receipts.Rows(l_IntIndexReceipt).Item("UniqueId").ToString())

	'                        If l_string_Output <> "0" Then
	'                            MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                            Exit Sub
	'                        End If
	'                    End If
	'                End If
	'            End If

	'            'Close out the PaymentInterest if any used!
	'            If l_double_TotInterest > 0 And l_IntIndexInterest > -1 Then
	'                If Not l_dt_Interest Is Nothing Then
	'                    If l_dt_Interest.Rows.Count > 0 Then
	'                        l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.UpdatePaymentInterest(l_string_UniqueId, l_string_Date, l_string_AcctDate, l_dt_Interest.Rows(l_IntIndexInterest).Item("UniqueId"))
	'                        If l_string_Output <> "0" Then
	'                            MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                            Exit Sub
	'                        End If
	'                    End If
	'                End If
	'            End If

	'            'If the total available payment from Payment Interest is greater than the total used, create
	'            ' a credit

	'            'Start of comment by hafiz on 27Jan06
	'            'Dim l_double_TotAppliedInterest As Double
	'            'l_double_TotAppliedInterest = Convert.ToDecimal(TextBoxTotalInterest.Text)
	'            'End of comment by hafiz on 27Jan06
	'            If Not l_dt_Interest Is Nothing And l_IntIndexInterest > -1 Then
	'                If l_dt_Interest.Rows.Count > 0 Then
	'                    'Start of comment by hafiz on 27Jan06
	'                    'If l_double_InterestPaid < Convert.ToDecimal(l_dt_Interest.Rows(l_IntIndexInterest).Item("Amount")) And l_double_TotAppliedInterest <> 0 Then
	'                    'End of comment by hafiz on 27Jan06

	'                    'Start of code add by hafiz on 27Jan06
	'                    ' If l_double_InterestPaid < Convert.ToDecimal(l_dt_Interest.Rows(l_IntIndexInterest).Item("Amount")) And l_double_TotInterest <> 0 Then
	'                    If Convert.ToDecimal(l_dt_Interest.Rows(l_IntIndexInterest).Item("Amount")) > 0 And l_double_TotInterest <> 0 Then
	'                        'End of code add by hafiz on 27Jan06
	'                        l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.InsertYmcaCreditsRcpts(l_dt_Transmittals.Rows(0).Item("YmcaId").ToString(), l_string_UniqueId, (l_dt_Interest.Rows(l_IntIndexInterest).Item("Amount")), l_string_Date, l_string_AcctDate, l_dt_Interest.Rows(l_IntIndexInterest).Item("UniqueId"))
	'                        '  l_string_Output = YMCARET.YmcaBusinessObject.CashApplicationBOClass.InsertYmcaCreditsRcpts(l_dt_Transmittals.Rows(0).Item("YmcaId").ToString(), l_string_UniqueId, (l_dt_Interest.Rows(l_IntIndexInterest).Item("Amount") - l_double_InterestPaid), l_string_Date, l_string_AcctDate, l_dt_Interest.Rows(l_IntIndexInterest).Item("UniqueId"))
	'                        If l_string_Output <> "0" Then
	'                            MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Error while saving the data.", MessageBoxButtons.Stop)
	'                            Exit Sub
	'                        End If
	'                    End If
	'                End If
	'            End If
	'            'Aparna Samala 05/09/2006
	'            ' Dim l_Dataset As New DataSet
	'            ' l_Dataset = YMCARET.YmcaBusinessObject.CashApplicationBOClass.SelectPersonDetails((Session("TransmittalID")))
	'            If l_datatable.Rows.Count > 0 Then
	'                Try
	'                    SendMail(l_datatable)
	'                Catch
	'                    Dim AlertScript As String = "<script language='javascript'>" & _
	'                             "alert('There was a problem encountered while sending mail');</script>"

	'                    ' Add the JavaScript code to the page.
	'                    Page.RegisterStartupScript("AlertScript", AlertScript)
	'                End Try

	'            End If



	'            PopulateValues()

	'        End If  ' For l_dt_Transmittals is nothing


	'    Catch ex As Exception

	'    End Try
	'End Sub
	'Shubhrata Dec 22nd Dec 06 YREN 3006--Have put the original code for ButtonpayClick in this method so that 
	'old code remains for reference.
End Class
