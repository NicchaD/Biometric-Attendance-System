
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	VoidDisbursementVRManager.aspx.vb
' Author Name		:	Imran
' Employee ID		:	51494
' Email				:	
' Contact No		:	
' Creation Time		:	08/18/2009 
'** DATE            AUTHOR      REASONS
'** 30/10/2009      imran       For BT 1003
'** 30/10/2009      Imran       Save and PHR button in single line
'** 05-11-09        Priya J     changes made to view datagrid after no records found message.
'** 12/11/2009      Imran       For BT 1014
'** 13/11/2009      Imran       For BT  1015
'** 17/11/2009      Imran       Check Disbursement Date to Issued Date in sending mail
'Neeraj Singh                   19/Nov/2009         Added form name for security issue YRS 5.0-940 
'** 20/11/2009      Imran       Trimming of search fields 
'** 19/12/2009      Imran       For Gemini Issue  968 -When disbursement is cash check then checkbox disable
'   15/Feb/2010     Shashi Shekhar   Restrict Data Archived Participants To proceed in Find list.
' 18/Mar/2010       Shashi Shekhar   Allow the user to access certain functions only for Retired participants (status RD) even if they are archived (Ref:Handling usability issue of Data Archive)
' 21/Apr/2010       Shashi Shekhar   Changes For YRS 5.0-1049 
' 29-April-2010     Priya       YRS 5.0-1060-Voiding a Check
'10-May-2010        Priya       YRS 5.0-1060-Voiding a Check
'12-May-2010        Priya       YRS 5.0-1060-Voiding a Check
' Shashi Shekhar: 2010-12-09: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'31-Jan-2011		priya		YRS 5.0-1217 : MRD Part-II changes
'****************************************************
'Modification History
'****************************************************
'Modified by       Date              Description
'****************************************************
'Shashi Shekhar    28 Feb 2011       Replacing Header formating with user control (YRS 5.0-450 )
'Imran             27-Apr-2011       BT:825- Security issue in void reverse                          
'Imran             12-Nov-2011       BT: 964- Problem in Withdrawal->Void-Reverse
'Manthan Rajguru   2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
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
Imports System.Text
Imports System.IO

Public Class VoidDisbursementReverseManager
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'Dim strFormName As String = New String("VoidDisbursementReverseManager.aspx")'Commented by Shashi Shekhar:2009-12-17:To Resolve the security Issue (Ref:Security Review excel sheet)
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridVRManager As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridDisbursements As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripVRManager As Microsoft.Web.UI.WebControls.TabStrip

    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeSSN As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents MultiPageVRManager As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAccNo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAccountNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBankInfo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxBankInfo As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEntityType As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxEntityType As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEntityAddress As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxEntityAddress As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLegalEntity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLegalEntity As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownListStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelReason As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownListReason As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents chkChangeStatus As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDeductionCashOutsCharges As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelNotes As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents TestCheckBox As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents PlaceHolderMessageBox As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents litPersID As System.Web.UI.WebControls.Literal
    Protected WithEvents litDisbID As System.Web.UI.WebControls.Literal
    Protected WithEvents litFundID As System.Web.UI.WebControls.Literal
    Protected WithEvents litFlag As System.Web.UI.WebControls.Literal
    Protected WithEvents litWHAmount As System.Web.UI.WebControls.Literal
    Protected WithEvents litGross As System.Web.UI.WebControls.Literal
    Protected WithEvents litPayeeId As System.Web.UI.WebControls.Literal
    Protected WithEvents litAddressID As System.Web.UI.WebControls.Literal
    Protected WithEvents litDisbNbr As System.Web.UI.WebControls.Literal
    Protected WithEvents LabelRecordNotFound As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPHR As System.Web.UI.WebControls.Button
    Protected WithEvents Datalist1 As System.Web.UI.WebControls.DataList
    Protected WithEvents DataList2 As System.Web.UI.WebControls.DataList
    Protected WithEvents Datagrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgItemDetails As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datalist3 As System.Web.UI.WebControls.DataList
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents VoidUserControl1 As YMCAUI.VoidUserControl
    Protected WithEvents frmVRManager As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents TextBoxAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAccountNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxBankInfo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxEntityType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxEntityAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLegalEntity As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCheckNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCheckNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelAddressCheckSent As System.Web.UI.WebControls.Label
    Protected WithEvents hdnDisbId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdnListDisbId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdnRequestID As System.Web.UI.HtmlControls.HtmlInputHidden

    'Added by imran on  29/10/2009 for UnGrouping Disbursement list
    Protected WithEvents VoidDisbursement1 As YMCAUI.VoidDisbursement
    Protected WithEvents ButtonCloseVR As System.Web.UI.WebControls.Button
    'Added by imran on  20/11/2009 for fund events Status list
    Protected WithEvents DropDownStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelFundStatus As System.Web.UI.WebControls.Label
    Protected WithEvents hdnCurrentFundStatus As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdnRecommendedFundStatus As System.Web.UI.HtmlControls.HtmlInputHidden
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
    'Global variable declaration
    Dim g_dataset_dsDisbursements As New DataSet
    Dim g_dataset_dsDisbursementsByPersId As New DataSet
    Dim g_dataset_dsWithholdingInfo As New DataSet
    Dim g_dataset_dsRelatedDisbursement As New DataSet
    Dim g_boolean_flag As Boolean
    Dim g_search_flag As Boolean
    Dim _iCounter As Integer = 0

    Dim objRefund As RefundClass
    Dim objLoan As LoanClass
    Dim objAnnuity As AnnuityClass

#Region "EVENTS"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            'BT:825- Security issue in void reverse
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            If Not (IsPostBack) Then
                g_boolean_flag = False
                Clear_Session()

                If Not IsNothing(DataGridDisbursements) Then
                    Me.DataGridDisbursements.SelectedIndex = 0
                End If
            End If

            If Not Session("IsZeroFunding") Is Nothing Then
                If Session("IsZeroFunding") = True Then
                    Session("IsZeroFunding") = Nothing
                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "No amount due. Net amount after withholding = $0.00", MessageBoxButtons.OK)
                End If
            End If

            Me.LabelSSNo.AssociatedControlID = Me.TextBoxSSNo.ID

            Me.LabelFundNo.AssociatedControlID = Me.TextBoxFundNo.ID

            Me.LabelLastName.AssociatedControlID = Me.TextBoxLastName.ID

            Me.LabelFirstName.AssociatedControlID = Me.TextBoxFirstName.ID

            Me.LabelCheckNo.AssociatedControlID = Me.TextBoxCheckNo.ID

            Me.LabelRecordNotFound.Visible = False

            If Not IsPostBack Then
                Me.TabStripVRManager.Items(1).Enabled = False
                'Added on 23/10/2009 for Header label
                LoadGeneraltab("")

            End If

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            If Request.Form("OK") = "OK" Then
                FinalStatus()
            End If


            AddHandler VoidUserControl1.DisbursementbyPersIdEvent, AddressOf PopulateDisbursementTextBoxes
            AddHandler VoidUserControl1.ClearDisbursementGridEvent, AddressOf ClearDisbursementTextBoxes
            'Added by imran on  29/10/2009 for UnGrouping Disbursement list
            AddHandler VoidDisbursement1.DisbursementbyPersIdEvent, AddressOf PopulateDisbursementTextBoxes
            AddHandler VoidDisbursement1.ClearDisbursementGridEvent, AddressOf ClearDisbursementTextBoxes

            AddHandler VoidUserControl1.GetFundStatusbyRequestIdEvent, AddressOf GetFundStatusByRequestId



			If Request.Form("Yes") = "Yes" Then
				

				'Added by imran on 21/10/2009 for Interest Process 
				If Session("flag") = "YInterest" Then
					Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
					savevoidReverse()
				End If

				'31-Jan-2011	 priya YRS 5.0-1217 : MRD Part-II changes
				If Session("PromptInActiveMRDReversal") = "1" Then
					ValidateVoidReverse()
					Session("PromptInActiveMRDReversal") = "YES"
				End If
				'End 31-Jan-2011	 YRS 5.0-1217 : MRD Part-II changes

				'10-May-2010:Priya: YRS 5.0-1060-Voiding a Check
				If Not IsNothing(Session("ANNPaid")) Then
					If Session("ANNPaid") = "Yes" Then
						Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
						Session("ANNPaid") = "No"
						'savevoidReverse()
						'12-May-2010        Priya       YRS 5.0-1060-Voiding a Check
						ValidateVoidReverse()
						If Session("flag") = "Pending" Then
							Exit Sub
						End If
						'12-May-2010        Priya       YRS 5.0-1060-Voiding a Check

					End If
				End If
				'10-May-2010:Priya: YRS 5.0-1060-Voiding a Check
				If Session("flag") = "Pending" Then
					Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
					Session("flag") = "notpending"
					' hdnListDisbId.Value = hdnListDisbId.Value.Replace(hdnDisbId.Value, "")
					'ValidateVoidReverse()

					'Added Imran on 22/10/2009
					Dim Strarr As Array
					Dim StrListDisbId As String

					StrListDisbId = hdnListDisbId.Value
					If StrListDisbId.Length > 0 Then
						Strarr = StrListDisbId.Split(",")
						Dim i As Integer
						If Strarr.Length >= 2 Then
							Session("_iCounter") = 0
							If Strarr.GetValue(1) <> "" Then
								ChangeStatus(Strarr.GetValue(1))
							End If
						End If
					End If

				End If


			End If

            If Request.Form("continue") = "Continue" Then
                If Session("flag") = "FundStatus" Then
                    ValidateVoidReverse()
                Else
                    SearchAndPopulateData(True)
                    Session("_iCounter") = 1
                    ChangeStatus(Session("DisbursementId"))
                End If
            End If

            If Request.Form("No") = "No" Then

                'Added by imran on 21/10/2009 for Interest Process 
                If Session("flag") = "YInterest" Then
                    Session("flag") = "NInterest"
                    Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                    savevoidReverse()
                End If
                SearchAndPopulateData(True)
                If Session("flag") = "Pending" Then
                    Session("flag") = "notpending"
                End If
            End If

            If Request.Form("MessageBoxIDNo") = "Cancel" Then
                SearchAndPopulateData(True)
            End If
            'If Request.Form("OK") = "OK" Then
            '    'Added by imran on 29/10/2009 
            '    ClearDisbursementTextBoxes()
            '    If VoidUserControl1.Action = "REFUND" Then
            '        VoidUserControl1.PopulateDisbursementGrid()
            '    Else

            '        VoidDisbursement1.PopulateDisbursementGrid()
            '    End If
            'End If


            If Session("flag") <> "FundStatus" And Request.Form("OK") = "OK" Then

                'Added by imran on 24/11/2009 
                LabelFundStatus.Text = ""
                DropDownStatus.Enabled = False
                DropDownStatus.SelectedValue = "0"
                hdnCurrentFundStatus.Value = ""
                hdnRecommendedFundStatus.Value = ""
                'Added by imran on 29/10/2009 
                ClearDisbursementTextBoxes()
                If VoidUserControl1.Action = "REFUND" Then
                    VoidUserControl1.PopulateDisbursementGrid()
                Else

                    VoidDisbursement1.PopulateDisbursementGrid()
                End If
            End If

            If Session("Called") = True Then
                '    'Priya 11/04/2008 send mail if accounting date is less than todays date nad if status selected value is Reverse
                '    'If DropDownListStatus.SelectedValue = "REVERSE" Then

                savevoidReverse()
                SearchAndPopulateData(True)
                Session("Called") = False
            End If


            'Added by imran on 30/10/2009 For BT 1003
        Catch ex As SqlException
            Session("Called") = False
            Dim l_string_Type As String
            l_string_Type = Request.QueryString.Get("Type")
            If ex.Number = 60006 Then
                ' Response.Redirect("VoidDisbursementVRManager.aspx?Type=" & l_string_Type & "")
                LoadGeneraltab("")
                If Not IsNothing(DataGridVRManager) Then
                    Me.DataGridVRManager.SelectedIndex = -1
                End If
                RepopulatePopulateData()
                Me.TabStripVRManager.Items(1).Enabled = False
                Me.MultiPageVRManager.SelectedIndex = 0
                Me.TabStripVRManager.SelectedIndex = 0
            End If
        Catch ex As Exception
            Session("Called") = False
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub TabStripVRManager_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripVRManager.SelectedIndexChange
        Try
            If (TabStripVRManager.SelectedIndex <> 0) Then
                Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                ButtonSave.Enabled = True
                Session("DisbursementByPersIdIndex") = 0
                Session("DisbursementId") = Nothing
                SearchAndPopulateData(True)
            Else
                Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                ButtonSave.Enabled = True
                Session("DisbursementByPersIdIndex") = 0
                Session("DisbursementId") = Nothing
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
		Try
			'31-Jan-2011		priya		YRS 5.0-1217 : MRD Part-II changes
			Clear_Session()
			'End 31-Jan-2011		priya		YRS 5.0-1217 : MRD Part-II changes
			TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "") 'by Aparna 19/09/2007
			'Call this method to get the list of participant
			'Added by imran on 20/11/2009 For Triming search fields
			TextBoxFundNo.Text = TextBoxFundNo.Text.Trim()
			TextBoxSSNo.Text = TextBoxSSNo.Text.Trim()
			TextBoxCheckNo.Text = TextBoxCheckNo.Text.Trim()
			TextBoxFirstName.Text = TextBoxFirstName.Text.Trim()
			TextBoxLastName.Text = TextBoxLastName.Text.Trim()


			If Not (TextBoxCheckNo.Text = "" And TextBoxFirstName.Text = "" And TextBoxLastName.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNo.Text = "") Then

				Session("DisbursementIndex") = 0
				Session("DisbursementByPersIdIndex") = 0
				Session("DisbursementId") = Nothing
				'Added by imran on 13/11/2009 For BT-1014
				Me.TabStripVRManager.Items(1).Enabled = False
				SearchAndPopulateData(False)


			Else
				Session("g_search_flag") = True
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "This search will take a long time, so please define a search criteria.", MessageBoxButtons.OK)
			End If

		Catch ex As System.Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
		End Try
    End Sub
    Private Sub DataGridVRManager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridVRManager.SelectedIndexChanged
        Try
           
            '----Shashi Shekhar:2010-02-15: Code to handle Archived Participants from list--------------
            'Shashi Shekhar:2010-03-18:Handling usability issue of Data Archive
            Dim l_string_Type As String
            Dim l_PersId As String
            Dim dr As DataRow()
            l_PersId = Me.DataGridVRManager.SelectedItem.Cells(1).Text.Trim
            If Not Session("dsDisbursements") Is Nothing Then
                g_dataset_dsDisbursements = Session("dsDisbursements")
            End If
            dr = g_dataset_dsDisbursements.Tables("Disbursements").Select("[PersId]= '" & l_PersId & "'")


            l_string_Type = Request.QueryString.Get("Type")

			If l_string_Type = "REFUND" Then
				

				'If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
				If (dr(0).Item("IsArchived") = True) Then
					MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.pageTitle = "Void and Reverse Disbursement / Withdrawals"
                    Exit Sub
				End If

			ElseIf l_string_Type = "TDLOAN" Then

				' If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
				If (dr(0).Item("IsArchived") = True) Then
					MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.pageTitle = "Void and Reverse Disbursement / Loan"
                    Exit Sub
				End If

			ElseIf l_string_Type = "ANNUITY" Then
				'Priya 29-April-2010 : YRS 5.0-1060-Voiding a Check
				Session("StatusType") = dr(0).Item("StatusType").trim
				'End YRS 5.0-1060
				'If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
				If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "RD")) Then
					'Shashi Shekhar:2010-04-21:For YRS 5.0-1049 Adding Stauts "DR" also with "RD" for Handling usability issue of Data Archive
					If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "DR")) Then
						'Shashi Shekhar:2010-04-21:For YRS 5.0-1049 Adding Stauts "DR" also with "DD" for Handling usability issue of Data Archive
						If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "DD")) Then
							MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
							Me.TabStripVRManager.Items(1).Enabled = False
                            Headercontrol.pageTitle = "Void and Reverse Disbursement / Annuity"
							Exit Sub
						End If
					End If
				End If

			End If


			'-----------------------------------------------------------------------------------------------

			'Method added by priya to get selected index change of inactive employee but active MRD according to user selected of message
			'commentd by priya 31-Jan-2011 MRD Part-II changes, YRS 5.0-1217
			'SelectedIndexChangeDataGridVRManager()
			Session("DisbursementIndex") = DataGridVRManager.SelectedIndex
			Session("_iCounter") = 0
			Session("SSNoVRManager") = Me.DataGridVRManager.SelectedItem.Cells(2).Text.Trim

			Session("DisbursementByPersIdIndex") = 0
			ClearDisbursementTextBoxes()
			If Me.DataGridVRManager.Items.Count > 0 Then
				Me.TabStripVRManager.Items(1).Enabled = True

			End If
			'Added by imran on  29/10/2009 for UnGrouping Disbursement list
			If Request.QueryString.Get("Type") = "REFUND" Then
				Me.VoidUserControl1.PopulateDisbursementGrid()
				Me.VoidDisbursement1.Visible = False
			Else
				Session("VRManager_Sort") = Nothing
				Me.VoidDisbursement1.PopulateDisbursementGrid()
				Me.VoidUserControl1.Visible = False
			End If
			Me.MultiPageVRManager.SelectedIndex = 1
			Me.TabStripVRManager.SelectedIndex = 1
			'Added on 22/10/2009 For Header text
			LoadGeneraltab(Me.DataGridVRManager.SelectedItem.Cells(1).Text.Trim)
			If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
				If g_dataset_dsDisbursementsByPersId.Tables.Count > 0 Then
					If (g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows.Count > 0) Then
						PopulateDisbursementTextBoxes()
					End If
				End If
			End If

		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
		End Try
    End Sub
    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click


        Try
            ''Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            'If Not checkSecurity.Equals("True") Then
            '    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            ''End : YRS 5.0-940



            '---------------------------------------------------------------------------------------
            'Added by Shashi Shekhar:2009-12-17: To resolve the security Issue(Reg:Security Review excel sheet)
            Dim checkSecurity As String
            Dim strFormName As String

            If VoidUserControl1.Action = "REFUND" Then
                strFormName = "VoidDisbursementReverseManager.aspx?Type=REFUND"
                checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            ElseIf VoidUserControl1.Action = "ANNUITY" Then
                strFormName = "VoidDisbursementReverseManager.aspx?Type=ANNUITY"
                checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            ElseIf VoidUserControl1.Action = "TDLOAN" Then
                strFormName = "VoidDisbursementReverseManager.aspx?Type=TDLOAN"
                checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            End If

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            '--------------------------------------------------------------------------------------




            Dim datagrid As DataGrid
            Dim dglst As DataList
            Dim cnt As Integer
            hdnDisbId.Value = ""
            Session("flag") = Nothing
            hdnListDisbId.Value = ""
            If VoidUserControl1.Action = "REFUND" Then
                dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)

                cnt = 0
                For Each item As DataListItem In dglst.Items
                    Dim img As ImageButton



                    img = item.Controls(0).FindControl("Imagebutton1")
                    If img.ImageUrl = "../images/selected.gif" Then
                        datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
                        hdnRequestID.Value = dglst.DataKeys().Item(cnt).ToString()
                        If Not IsNothing(datagrid) Then
                            Dim i As Integer

                            For i = 0 To datagrid.Items.Count - 1
                                Dim chk As CheckBox
                                Dim LabelDisbId As Label
                                chk = datagrid.Items(i).FindControl("chkRefund")
                                LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
                                If Not IsNothing(chk) Then
                                    If chk.Checked Then
                                        If LabelDisbId.Text.ToUpper = dglst.DataKeys(cnt).ToString().ToUpper Then
                                            MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Necessary data is missing. Unable to complete the void process. Please refer this to the IT dept for a manual correction.", MessageBoxButtons.Stop)
                                            Exit Sub
                                        End If
                                        hdnListDisbId.Value = hdnListDisbId.Value + "," + LabelDisbId.Text.Trim()
                                    End If
                                End If
                            Next
                        End If

                    End If
                    cnt += 1
                Next

                '    'Commented by imran on 29/10/2009 For Grouping Disbursements
                'ElseIf VoidUserControl1.Action = "TDLOAN" Then

                '    dglst = CType(VoidUserControl1.FindControl("datalistTDLoan"), DataList)

                '    For Each item As DataListItem In dglst.Items
                '        Dim img As ImageButton
                '        img = item.Controls(0).FindControl("imgTDLoan")
                '        If img.ImageUrl = "../images/selected.gif" Then
                '            datagrid = CType(item.FindControl("dgLoan"), DataGrid)
                '            If Not IsNothing(datagrid) Then
                '                Dim i As Integer

                '                For i = 0 To datagrid.Items.Count - 1
                '                    Dim chk As CheckBox
                '                    chk = datagrid.Items(i).FindControl("chkLoan")
                '                    If Not IsNothing(chk) Then
                '                        If chk.Checked Then
                '                            'If datagrid.Items(i).Cells(1).Text.ToUpper = dglst.DataKeys(cnt).ToString().ToUpper Then
                '                            '    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Request is not exist for Disbursement.", MessageBoxButtons.OK)
                '                            '    Exit Sub
                '                            'End If
                '                            hdnListDisbId.Value = hdnListDisbId.Value + "," + datagrid.Items(i).Cells(1).Text
                '                        End If
                '                    End If
                '                Next
                '            End If

                '        End If
                '        cnt += 1
                '    Next


                'ElseIf VoidUserControl1.Action = "ANNUITY" Then

                '    dglst = CType(VoidUserControl1.FindControl("datalistAnnuity"), DataList)

                '    For Each item As DataListItem In dglst.Items
                '        Dim img As ImageButton
                '        img = item.Controls(0).FindControl("imgAnnuity")
                '        If img.ImageUrl = "../images/selected.gif" Then
                '            datagrid = CType(item.FindControl("dgAnnuity"), DataGrid)
                '            If Not IsNothing(datagrid) Then
                '                Dim i As Integer

                '                For i = 0 To datagrid.Items.Count - 1
                '                    Dim chk As CheckBox
                '                    chk = datagrid.Items(i).FindControl("chkAnnuity")
                '                    If Not IsNothing(chk) Then
                '                        If chk.Checked Then
                '                            'If datagrid.Items(i).Cells(1).Text.ToUpper = dglst.DataKeys(cnt).ToString().ToUpper Then
                '                            '    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Request is not exist for Disbursement.", MessageBoxButtons.OK)
                '                            '    Exit Sub
                '                            'End If
                '                            hdnListDisbId.Value = hdnListDisbId.Value + "," + datagrid.Items(i).Cells(1).Text
                '                        End If
                '                    End If
                '                Next
                '            End If

                '        End If
                '        cnt += 1
                '    Next
            Else
                Dim i As Integer
                Dim dg As DataGrid
                dg = CType(VoidDisbursement1.FindControl("Datagrid1"), DataGrid)
                For i = 0 To dg.Items.Count - 1
                    Dim btnImage As ImageButton
                    btnImage = dg.Items(i).FindControl("ImagebtnAddress")
                    If Not IsNothing(btnImage) Then
                        If btnImage.ImageUrl = "../images/selected.gif" Then
                            'If btnImage.ImageUrl = "../images/details.gif" Then
                            hdnListDisbId.Value = hdnListDisbId.Value + "," + dg.Items(i).Cells(1).Text
                        End If
                    End If
                Next

            End If


            If hdnListDisbId.Value.Length = 0 Then
                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select a Disbursement.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'This validation only for Withdrawal Reverse
            If VoidUserControl1.Action = "REFUND" And DropDownStatus.Enabled Then
                If DropDownStatus.SelectedValue.Trim() = "0" Then
                    Session("flag") = "FundStatus"
                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select fund status for participant.", MessageBoxButtons.Stop)
                    Exit Sub
                End If

                If DropDownStatus.SelectedValue.Trim() <> hdnCurrentFundStatus.Value Then
                    If DropDownStatus.SelectedValue.Trim() <> hdnRecommendedFundStatus.Value And hdnRecommendedFundStatus.Value <> "" Then
                        Session("flag") = "FundStatus"
                        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "The selected fund status does not match with the recommended fund status. Do you wish to continue?", MessageBoxButtons.ContinueCancel)
                        Exit Sub
                    End If
                End If


                'hdnCurrentFundStatus.Value
            End If



            'Added by imran on 16/12/2009  for Gemini Issue  968 -When disbursement is cash check then checkbox disable
            g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
            If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                Dim parameterListDisbId As Array
                Dim strOutPut As String
                Dim i As Integer
                Dim l_searchExpr As String
                Dim l_datarow_CurrentRow As DataRow


                If hdnListDisbId.Value.Length > 0 Then
                    parameterListDisbId = hdnListDisbId.Value.Split(",")
                End If
                ''**If Not Session("DisbursementId") Is Nothing Then
                Dim l_dr_CurrentRow As DataRow()

                If parameterListDisbId.Length >= 2 Then
                    For i = 1 To parameterListDisbId.Length - 1
                        strOutPut += "','" + parameterListDisbId.GetValue(i)

                    Next
                End If
                strOutPut = strOutPut.Substring(3, strOutPut.Length - 3)

                l_searchExpr = "DisbursementID in  ('" + strOutPut + "') and bitpaid=1"
                l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                If l_dr_CurrentRow.Length > 0 Then
                    '29-April-2010:Priya: YRS 5.0-1060-Voiding a Check
                    If VoidUserControl1.Action = "ANNUITY" Then
                        If Not Session("StatusType") Is Nothing OrElse Convert.ToString(Session("StatusType")).Trim = "" Then
                            If Convert.ToString(Session("StatusType")).Trim = "DR" OrElse Convert.ToString(Session("StatusType")).Trim = "DD" OrElse Convert.ToString(Session("StatusType")).Trim = "DF" OrElse Convert.ToString(Session("StatusType")).Trim = "DQ" Then
                                Session("ANNPaid") = "Yes"
                                MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "The payment has been cashed. Do you wish to continue voiding this payment?", MessageBoxButtons.YesNo)
                                Exit Sub
                            End If
                        End If
                    End If
                    'End YRS 5.0-1060-Voiding a Check

                    'l_datarow_CurrentRow = l_dr_CurrentRow(0)
                    'If CType(l_datarow_CurrentRow("bitpaid"), String).ToUpper = "TRUE" Then
                    'End If
                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement(s) cashed. Cannot reverse it.", MessageBoxButtons.Stop)
                    Exit Sub

                End If
            End If

            ValidateVoidReverse()


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click, ButtonCloseVR.Click

        Session("VRManager_Sort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)

    End Sub
    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFundNo.Text = ""
            Me.TextBoxLastName.Text = ""
            Me.TextBoxFirstName.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.TextBoxCheckNo.Text = ""
            Me.DataGridVRManager.Visible = False
            Me.TabStripVRManager.Items(1).Enabled = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub DataGridVRManager_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridVRManager.SortCommand
        Try
            Me.DataGridVRManager.SelectedIndex = -1
            If Not Session("DS_Sort_VRManager") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression

                g_dataset_dsDisbursements = Session("DS_Sort_VRManager")
                dv = g_dataset_dsDisbursements.Tables(0).DefaultView
                dv.Sort = SortExpression

                If Not Session("VRManager_Sort") Is Nothing Then
                    If Session("VRManager_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If

                Me.DataGridVRManager.DataSource = Nothing
                Me.DataGridVRManager.DataSource = dv
                Me.DataGridVRManager.DataBind()
                Session("VRManager_Sort") = dv.Sort
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonPHR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPHR.Click
        Try

            Session("VRManager") = "VRManager"
            Session("SSNo") = Session("SSNoVRManager")
            Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('RetireesFrmMoverWebForm.aspx', 'ReportPopUp', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
            SearchAndPopulateData(True)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region

#Region "METHODS"
	
	Public Sub SearchAndPopulateData(ByVal _postback As Boolean)
		Dim l_integer_AnnuityOnly As Integer
		Dim l_string_Type As String
		Try
			l_string_Type = Request.QueryString.Get("Type")
			If l_string_Type = "REFUND" Then
				VoidUserControl1.Action = "REFUND"
				l_integer_AnnuityOnly = 0
				VoidUserControl1.Activity = "REFUNDREVERSE"
			ElseIf (l_string_Type = "TDLOAN") Then
				VoidUserControl1.Action = "TDLOAN"
				VoidUserControl1.Activity = "LOANREVERSE"

				'Added by Imran on 29/10/2009
				VoidDisbursement1.Action = "TDLOAN"
				VoidDisbursement1.Activity = "LOANREVERSE"
				l_integer_AnnuityOnly = 21
			ElseIf (l_string_Type = "ANNUITY") Then

				VoidUserControl1.Action = "ANNUITY"
				VoidUserControl1.Activity = "VREVERSE"
				'Added by Imran on 29/10/2009
				VoidDisbursement1.Action = "ANNUITY"
				VoidDisbursement1.Activity = "VREVERSE"

				l_integer_AnnuityOnly = 1
			End If

			If Not _postback Then
				g_dataset_dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.LookUpDisbursements(Me.TextBoxFundNo.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxSSNo.Text.Trim(), Me.TextBoxCheckNo.Text.Trim(), l_integer_AnnuityOnly)


				If Not g_dataset_dsDisbursements Is Nothing Then
					If g_dataset_dsDisbursements.Tables.Count = 0 Then
						MessageBox.Show(PlaceHolderMessageBox, " YMCA - YRS", "No Record Found.", MessageBoxButtons.OK, False)
					End If
				End If

				Session("dsDisbursements") = g_dataset_dsDisbursements
			Else
				If Not Session("dsDisbursements") Is Nothing Then
					g_dataset_dsDisbursements = Session("dsDisbursements")
				End If

			End If
			If Not g_dataset_dsDisbursements Is Nothing Then

				If g_dataset_dsDisbursements.Tables.Count > 0 Then


					If g_dataset_dsDisbursements.Tables("Disbursements").Rows.Count = 0 Then
						Me.LabelRecordNotFound.Visible = True
						Me.DataGridVRManager.Visible = False
					Else
						Me.DataGridVRManager.DataSource = g_dataset_dsDisbursements.Tables(0).DefaultView

						Session("DS_Sort_VRManager") = g_dataset_dsDisbursements
						Me.DataGridVRManager.DataBind()

						If Not (Session("DisbursementIndex") > 0) Then
							Session("DisbursementIndex") = 0
						End If

						Me.LabelRecordNotFound.Visible = False
						Me.DataGridVRManager.Visible = True

					End If
				End If
			End If
		Catch ex As SqlException
			'Throw (ex)
			If Not Session("dsDisbursements") Is Nothing Then
				Session("dsDisbursements") = Nothing
			End If
			'Added on 22/10/2009 by imran
			Me.TabStripVRManager.Items(1).Enabled = False
			Session("g_search_flag") = True
			'MessageBox.Show(PlaceHolderMessageBox, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
			LabelRecordNotFound.Visible = True
			'05-11-09 : changes made to view datagrid after no records found message.
			Me.DataGridVRManager.Visible = False
		End Try
	End Sub
	'Added by imran on 30/10/2009
	'Method to Repopulate the datagrid with the list of employees.
	Public Sub RepopulatePopulateData()
		Dim l_integer_AnnuityOnly As Integer
		Dim l_string_Type As String
		Try
			l_string_Type = Request.QueryString.Get("Type")
			If l_string_Type = "REFUND" Then
				VoidUserControl1.Action = "REFUND"
				l_integer_AnnuityOnly = 0
				VoidUserControl1.Activity = "REFUNDREPLACE"
			ElseIf (l_string_Type = "TDLOAN") Then
				VoidUserControl1.Action = "TDLOAN"
				VoidUserControl1.Activity = "LOANREPLACE"

				'Added by Imran on 29/10/2009
				VoidDisbursement1.Action = "TDLOAN"
				VoidDisbursement1.Activity = "LOANREPLACE"
				l_integer_AnnuityOnly = 21
			ElseIf (l_string_Type = "ANNUITY") Then

				VoidUserControl1.Action = "ANNUITY"
				VoidUserControl1.Activity = "VReplace"

				'Added by Imran on 29/10/2009
				VoidDisbursement1.Action = "ANNUITY"
				VoidDisbursement1.Activity = "VReplace"
				l_integer_AnnuityOnly = 1
			End If


			g_dataset_dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.LookUpDisbursements(Me.TextBoxFundNo.Text, Me.TextBoxFirstName.Text, Me.TextBoxLastName.Text, Me.TextBoxSSNo.Text, Me.TextBoxCheckNo.Text, l_integer_AnnuityOnly)
			Session("dsDisbursements") = g_dataset_dsDisbursements



			If g_dataset_dsDisbursements.Tables("Disbursements").Rows.Count = 0 Then
				Me.LabelRecordNotFound.Visible = True
				Me.DataGridVRManager.Visible = False
			Else
				Me.DataGridVRManager.DataSource = g_dataset_dsDisbursements.Tables(0).DefaultView

				' Session("DS_Sort_VRManager") = g_dataset_dsDisbursements
				Me.DataGridVRManager.DataBind()

				If Not (Session("DisbursementIndex") > 0) Then
					Session("DisbursementIndex") = 0
				End If

				Me.LabelRecordNotFound.Visible = False
				Me.DataGridVRManager.Visible = True
				'Call the following method to load the Disbursements Grid.


			End If
		Catch ex As SqlException
			' Throw (ex)
			If Not Session("dsDisbursements") Is Nothing Then
				Session("dsDisbursements") = Nothing
			End If
			'Added on 22/10/2009 by imran
			Session("g_search_flag") = True
			Me.TabStripVRManager.Items(1).Enabled = False
			' MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
			Me.LabelRecordNotFound.Visible = True
			Me.DataGridVRManager.Visible = False
		End Try
	End Sub
	Public Sub PopulateDisbursementTextBoxes()
		Dim l_integer_CurrentIndex As Integer
		Dim l_datarow_CurrentRow As DataRow

		Try
			l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
			g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

			If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
				Dim l_dr_CurrentRow As DataRow()
				Dim l_searchExpr As String
				If Not Session("DisbursementId") Is Nothing Then
					l_searchExpr = "DisbursementID = '" + Session("DisbursementId") + "'"
					l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
					If l_dr_CurrentRow.Length > 0 Then
						l_datarow_CurrentRow = l_dr_CurrentRow(0)
					Else
						MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data could not be retrieved.", MessageBoxButtons.OK)
						Exit Sub
					End If
				Else
					l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
				End If

			End If
		Catch
			Throw
		End Try
	End Sub
	Public Sub ClearDisbursementTextBoxes()

		Try
			Dim l_CheckBox As CheckBox

			Me.TextBoxAddress.Text = ""
			Me.TextBoxAccountNo.Text = ""
			Me.TextBoxEntityType.Text = ""
			Me.TextBoxBankInfo.Text = ""
			Me.TextBoxEntityAddress.Text = ""
			Me.TextBoxLegalEntity.Text = ""

		Catch
			Throw
		End Try
	End Sub
	Public Sub FinalStatus()
		Try
			'Added on 23/10/2009 if record is not then hide grid
			If (Session("g_search_flag") = True) Then
				Session("g_search_flag") = True
				Me.LabelRecordNotFound.Visible = False
				Me.DataGridVRManager.Visible = False

			End If

		Catch
			Throw
		End Try
	End Sub
	Private Sub SendMail()


		Dim obj As YMCAUI.MailUtil
		obj = New YMCAUI.MailUtil
		Dim l_string_details As String
		Dim l_stringCategory As String = "REFUND"

		Dim l_str_msg As String
		Dim l_CashAppDataTable As New DataTable
		Dim drow As DataRow

		Dim datagrid As DataGrid
		Dim dglst As DataList
		Dim cnt As Integer
		Dim l_string_FundIdNo As String
		Dim l_disbursement_type As String
		Dim l_payment_type As String
		Dim l_check_number As String
		Dim l_disbursement_date As DateTime
		Dim l_voided_date As DateTime
		Dim dsDisbursements As DataSet
		Dim strHTML As StringBuilder
		Dim l_Gross As Decimal


		l_string_FundIdNo = Convert.ToString(Session("dsDisbursements").Tables(0).Rows(0)("FundIDNo")).Trim	'g_dataset_dsDisbursements.Tables(0).Rows(0)("FundIDNo")
		If l_string_FundIdNo = "" Then
			Exit Sub
		End If
		Try
			'=================================================
			If VoidUserControl1.Action = "REFUND" Then
				dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)

				cnt = 0
				For Each item As DataListItem In dglst.Items
					Dim img As ImageButton

					img = item.Controls(0).FindControl("Imagebutton1")
					If img.ImageUrl = "../images/selected.gif" Then
						datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
						hdnRequestID.Value = dglst.DataKeys().Item(cnt).ToString()
						If Not IsNothing(datagrid) Then
							Dim i As Integer
							strHTML = New StringBuilder
							For i = 0 To datagrid.Items.Count - 1
								Dim chk As CheckBox
								Dim LabelDisbId As Label
								chk = datagrid.Items(i).FindControl("chkRefund")
								LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
								If Not IsNothing(chk) Then
									If chk.Checked Then
										dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(LabelDisbId.Text)

										If Not dsDisbursements Is Nothing Then
											If dsDisbursements.Tables(0).Rows.Count > 0 Then
												If i = 0 Then

													strHTML.Append("<Html>")
													'-------------CSS------------------------
													strHTML.Append("<style type=text/css>")
													strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
													strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
													strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
													strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

													strHTML.Append("</style>")
													'-------------------------------------

													strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
													strHTML.Append("<tr><td class=labelbold>Disbursement Type:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
													strHTML.Append("<tr><td class=labelbold>Voided Date:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")
													strHTML.Append("<tr><td class=labelbold colspan=4>")
													strHTML.Append("<table class =label border='1' bordercolor='Black' cellSpacing='0' cellPadding='0' align='Center' width=300px>")
													strHTML.Append("<tr><td class=labelbold align='Center'>Number</td><td class=labelbold align='Center'>Payment Type</td><td class=labelbold align='Center'>Date</td><td class=labelbold align='Center'>Gross Amt</td></tr>")

												End If

												If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
													strHTML.Append("<tr><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td><td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</td></tr>")
												Else
													strHTML.Append("<tr><td align='Center'> - </td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td><td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</td></tr>")
												End If

												'If dsDisbursements.Tables(1).Rows.Count > 0 Then
												'    Dim counter As Integer
												'    strHTML.Append("<tr><td colspan=4>")
												'    strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
												'    strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
												'    For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
												'        strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
												'        l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

												'    Next
												'    strHTML.Append("</table>")
												'    strHTML.Append("</td></tr>")

												'End If

											End If


										End If


									End If
								End If
							Next
							'If l_Gross <> 0 Then
							'    strHTML.Append("<tr><td class=labelbold>Gross Fees: </td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
							'End If

							strHTML.Append("</table></td></tr></table></html>")
						End If

					End If
					cnt += 1
				Next
				'Added by Imran on 29/10/2009 For Disbursement Ungrouping
			Else
				Dim i As Integer
				Dim dg As DataGrid
				dg = CType(VoidDisbursement1.FindControl("Datagrid1"), DataGrid)
				For i = 0 To dg.Items.Count - 1
					Dim btnImage As ImageButton
					btnImage = dg.Items(i).FindControl("ImagebtnAddress")
					If Not IsNothing(btnImage) Then
						If btnImage.ImageUrl = "../images/selected.gif" Then
							'If btnImage.ImageUrl = "../images/details.gif" Then
							'hdnListDisbId.Value = hdnListDisbId.Value + "," + dg.Items(i).Cells(1).Text


							dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(dg.Items(i).Cells(1).Text)

							If Not dsDisbursements Is Nothing Then
								If dsDisbursements.Tables(0).Rows.Count > 0 Then
									strHTML = New StringBuilder
									' If i = 0 Then

									strHTML.Append("<Html>")
									'-------------CSS------------------------
									strHTML.Append("<style type=text/css>")
									strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
									strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
									strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
									strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

									strHTML.Append("</style>")
									'-------------------------------------

									strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
									strHTML.Append("<tr><td class=labelbold>Disbursement Type:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
									strHTML.Append("<tr><td class=labelbold>Voided Date:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

									strHTML.Append("<tr><td class=labelbold colspan=4>")
									strHTML.Append("<table class =label border='1' bordercolor='Black' cellSpacing='0' cellPadding='0' align='Center' width=300px>")
									strHTML.Append("<tr><td class=labelbold align='Center'>Number</td><td class=labelbold align='Center'>Payment Type</td><td class=labelbold align='Center'>Date</td><td class=labelbold align='Center'>Gross Amt</td></tr>")


									'End If

									If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
										strHTML.Append("<tr><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td><td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</td></tr>")
									Else
										strHTML.Append("<tr><td align='Center'> - </td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td><td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</td></tr>")
									End If
									strHTML.Append("</table></td></tr></table></html>")
								End If
							End If
						End If

					End If
				Next



				'ElseIf VoidUserControl1.Action = "TDLOAN" Then

				'    dglst = CType(VoidUserControl1.FindControl("datalistTDLoan"), DataList)

				'    For Each item As DataListItem In dglst.Items
				'        Dim img As ImageButton
				'        img = item.Controls(0).FindControl("imgTDLoan")
				'        If img.ImageUrl = "../images/selected.gif" Then
				'            datagrid = CType(item.FindControl("dgLoan"), DataGrid)
				'            If Not IsNothing(datagrid) Then
				'                Dim i As Integer
				'                strHTML = New StringBuilder
				'                For i = 0 To datagrid.Items.Count - 1
				'                    Dim chk As CheckBox
				'                    chk = datagrid.Items(i).FindControl("chkLoan")
				'                    If Not IsNothing(chk) Then
				'                        If chk.Checked Then
				'                            ' hdnListDisbId.Value = hdnListDisbId.Value + "," + DataGrid.Items(i).Cells(1).Text
				'                            dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(datagrid.Items(i).Cells(1).Text)

				'                            If Not dsDisbursements Is Nothing Then
				'                                If dsDisbursements.Tables(0).Rows.Count > 0 Then
				'                                    If i = 0 Then

				'                                        strHTML.Append("<Html>")
				'                                        '-------------CSS------------------------
				'                                        strHTML.Append("<style type=text/css>")
				'                                        strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

				'                                        strHTML.Append("</style>")
				'                                        '-------------------------------------

				'                                        strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Disbursement Type:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Voided Date:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

				'                                        strHTML.Append("<tr><td class=labelbold colspan=4>")
				'                                        strHTML.Append("<table class =label border='1' bordercolor='Black' cellSpacing='0' cellPadding='0' align='Center' width=300px>")
				'                                        strHTML.Append("<tr><td class=labelbold align='Center'>Number</td><td class=labelbold align='Center'>Payment Type</td><td class=labelbold align='Center'>Date</td></tr>")


				'                                    End If

				'                                    If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
				'                                        strHTML.Append("<tr><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td></tr>")
				'                                    Else
				'                                        strHTML.Append("<tr><td align='Center'> - </td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td></tr>")
				'                                    End If



				'                                End If


				'                            End If

				'                            ''--------Chek end if                         
				'                        End If
				'                    End If
				'                Next

				'                strHTML.Append("</table></td></tr></table></html>")
				'            End If

				'        End If
				'    Next


				'ElseIf VoidUserControl1.Action = "ANNUITY" Then

				'    dglst = CType(VoidUserControl1.FindControl("datalistAnnuity"), DataList)

				'    For Each item As DataListItem In dglst.Items
				'        Dim img As ImageButton
				'        img = item.Controls(0).FindControl("imgAnnuity")
				'        If img.ImageUrl = "../images/selected.gif" Then
				'            datagrid = CType(item.FindControl("dgAnnuity"), DataGrid)
				'            If Not IsNothing(datagrid) Then
				'                Dim i As Integer
				'                strHTML = New StringBuilder
				'                For i = 0 To datagrid.Items.Count - 1
				'                    Dim chk As CheckBox
				'                    chk = datagrid.Items(i).FindControl("chkAnnuity")
				'                    If Not IsNothing(chk) Then
				'                        If chk.Checked Then
				'                            ' hdnListDisbId.Value = hdnListDisbId.Value + "," + DataGrid.Items(i).Cells(1).Text
				'                            dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(datagrid.Items(i).Cells(1).Text)

				'                            If Not dsDisbursements Is Nothing Then
				'                                If dsDisbursements.Tables(0).Rows.Count > 0 Then
				'                                    If i = 0 Then

				'                                        strHTML.Append("<Html>")
				'                                        '-------------CSS------------------------
				'                                        strHTML.Append("<style type=text/css>")
				'                                        strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

				'                                        strHTML.Append("</style>")
				'                                        '-------------------------------------

				'                                        strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Disbursement Type:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Voided Date:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

				'                                        strHTML.Append("<tr><td class=labelbold colspan=4>")
				'                                        strHTML.Append("<table class =label border='1' bordercolor='Black' cellSpacing='0' cellPadding='0' align='Center' width=300px>")
				'                                        strHTML.Append("<tr><td class=labelbold align='Center'>Number</td><td class=labelbold align='Center'>Payment Type</td><td class=labelbold align='Center'>Date</td></tr>")


				'                                    End If

				'                                    If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
				'                                        strHTML.Append("<tr><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td></tr>")
				'                                    Else
				'                                        strHTML.Append("<tr><td align='Center'> - </td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td></tr>")
				'                                    End If


				'                                End If


				'                            End If

				'                        End If
				'                    End If
				'                Next
				'                strHTML.Append("</table></td></tr></table></html>")
				'            End If

				'        End If
				'    Next

			End If
			'=================================================

			If obj.MailService = False Then
				Exit Sub
			End If
			Dim l_string_Activity As String
			l_string_Activity = ""
			l_string_Activity = Request.QueryString.Get("Type")

			obj.SendCc = ""
			obj.MailMessage = strHTML.ToString() '"A withdrawal for fund Id " & l_string_FundIdNo & " which was processed in a prior month has been reversed.  Please review and make necessary interest adjustments as soon as possible."
			'obj.Subject = "Withdrawal from prior month has been reversed."

			If l_string_Activity = "REFUND" Then
				obj.Subject = "Withdrawal from prior month has been reversed."
			Else
				obj.Subject = "Disbursement from prior month has been reversed."
			End If

			obj.MailCategory = "VOID"
			obj.MailFormatType = Mail.MailFormat.Html
			obj.Send()
		Catch
			Throw
		End Try
	End Sub
	Private Sub SendMailForFees()


		Dim obj As YMCAUI.MailUtil
		obj = New YMCAUI.MailUtil
		Dim l_string_details As String
		Dim l_stringCategory As String = "REFUND"

		Dim l_str_msg As String
		Dim l_CashAppDataTable As New DataTable
		Dim drow As DataRow

		Dim datagrid As DataGrid
		Dim dglst As DataList
		Dim cnt As Integer
		Dim l_string_FundIdNo As String
		Dim l_disbursement_type As String
		Dim l_payment_type As String
		Dim l_check_number As String
		Dim l_disbursement_date As DateTime
		Dim l_voided_date As DateTime
		Dim dsDisbursements As DataSet
		Dim strHTML As StringBuilder
		Dim l_Gross As Decimal
		Dim IsFeesExist As Boolean
		IsFeesExist = False

		l_string_FundIdNo = Convert.ToString(Session("dsDisbursements").Tables(0).Rows(0)("FundIDNo")).Trim	'g_dataset_dsDisbursements.Tables(0).Rows(0)("FundIDNo")
		If l_string_FundIdNo = "" Then
			Exit Sub
		End If
		Try
			'=================================================
			If VoidUserControl1.Action = "REFUND" Then
				dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)

				cnt = 0
				For Each item As DataListItem In dglst.Items
					Dim img As ImageButton

					img = item.Controls(0).FindControl("Imagebutton1")
					If img.ImageUrl = "../images/selected.gif" Then
						datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
						hdnRequestID.Value = dglst.DataKeys().Item(cnt).ToString()
						If Not IsNothing(datagrid) Then
							Dim i As Integer
							strHTML = New StringBuilder

							For i = 0 To datagrid.Items.Count - 1
								Dim chk As CheckBox
								Dim LabelDisbId As Label
								chk = datagrid.Items(i).FindControl("chkRefund")
								LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
								If Not IsNothing(chk) Then
									If chk.Checked Then
										dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(LabelDisbId.Text)

										If Not dsDisbursements Is Nothing Then
											'If dsDisbursements.Tables(0).Rows.Count > 0 And dsDisbursements.Tables(1).Rows.Count > 0 Then
											If dsDisbursements.Tables(0).Rows.Count > 0 Then

												If i = 0 Then

													strHTML.Append("<Html>")
													'-------------CSS------------------------
													strHTML.Append("<style type=text/css>")
													strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
													strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
													strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
													strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

													strHTML.Append("</style>")
													'-------------------------------------

													strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id: </td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
													strHTML.Append("<tr><td class=labelbold>Disbursement Type: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
													strHTML.Append("<tr><td class=labelbold>Voided Date: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")



													'strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id:</td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
													'strHTML.Append("<tr><td class=labelbold>Disbursement Type:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
													'strHTML.Append("<tr><td class=labelbold>Voided Date:</td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")
													'strHTML.Append("<tr><td class=labelbold colspan=4>")
													'strHTML.Append("<table class =label border='1' bordercolor='Black' cellSpacing='0' cellPadding='0' align='Center' width=300px>")
													'strHTML.Append("<tr><td class=labelbold align='Center'>Number</td><td class=labelbold align='Center'>Payment Type</td><td class=labelbold align='Center'>Date</td></tr>")


												End If

												If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
													strHTML.Append("<tr><td class=labelbold>Disbursement Number: </td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Issued Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</sapn></td><td class=labelbold>  Gross Amt: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</sapn></td></tr>")
												Else
													strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Issued Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td><td class=labelbold>  Gross Amt: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</sapn></td></tr>")
												End If

												'If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
												'    strHTML.Append("<tr><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'> " & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td></tr>")
												'Else
												'    strHTML.Append("<tr><td align='Center'> - </td><td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</td> <td align='Center'>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</td></tr>")
												'End If

												If dsDisbursements.Tables(1).Rows.Count > 0 Then
													IsFeesExist = True
													Dim counter As Integer
													strHTML.Append("<tr><td colspan=5>")
													strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
													strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
													For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
														strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
														l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

													Next
													strHTML.Append("</table>")
													strHTML.Append("</td></tr>")

												End If

											End If


										End If


									End If
								End If
							Next

							If l_Gross <> 0 Then
								strHTML.Append("<tr><td class=labelbold>Gross Fees: </td><td colspan=4>" & l_Gross.ToString() & "</td></tr>")

								strHTML.Append("<tr><td colspan=5></td></tr>")
								strHTML.Append("<tr><td colspan=5></td></tr>")
								strHTML.Append("<tr><td colspan=5 class=label>Please adjust the fees through manual transacts if required.</td></tr>")

							End If

							strHTML.Append("</table></html>")
						End If

					End If
					cnt += 1
				Next
				'Added by Imran on 29/10/2009 For Disbursement Ungrouping
			Else
				Dim i As Integer
				Dim dg As DataGrid
				dg = CType(VoidDisbursement1.FindControl("Datagrid1"), DataGrid)
				For i = 0 To dg.Items.Count - 1
					Dim btnImage As ImageButton
					btnImage = dg.Items(i).FindControl("ImagebtnAddress")
					If Not IsNothing(btnImage) Then
						If btnImage.ImageUrl = "../images/selected.gif" Then
							'    If btnImage.ImageUrl = "../images/details.gif" Then
							'hdnListDisbId.Value = hdnListDisbId.Value + "," + dg.Items(i).Cells(1).Text


							dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(dg.Items(i).Cells(1).Text)
							strHTML = New StringBuilder
							If Not dsDisbursements Is Nothing Then
								' If dsDisbursements.Tables(0).Rows.Count > 0 And dsDisbursements.Tables(1).Rows.Count > 0 Then
								If dsDisbursements.Tables(0).Rows.Count > 0 Then

									If i = 0 Then


										strHTML.Append("<Html>")
										'-------------CSS------------------------
										strHTML.Append("<style type=text/css>")
										strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
										strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
										strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
										strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

										strHTML.Append("</style>")
										'-------------------------------------

										strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id: </td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
										strHTML.Append("<tr><td class=labelbold>Disbursement Type: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
										strHTML.Append("<tr><td class=labelbold>Voided Date: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

									End If

									If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
										strHTML.Append("<tr><td class=labelbold>Disbursement Number: </td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Issued Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td><td class=labelbold>  Gross Amt: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</sapn></td></tr>")
									Else
										strHTML.Append("<tr><td class=labelbold> Disbursement: </td><td></td><td class=labelbold>Payment Type - <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Issued Date - <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td><td class=labelbold>  Gross Amt: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("GrossAmount").ToString() & "</sapn></td></tr>")
									End If

									If dsDisbursements.Tables(1).Rows.Count > 0 Then
										Dim counter As Integer
										IsFeesExist = True
										strHTML.Append("<tr><td colspan=5>")
										strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
										strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
										For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
											strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
											l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

										Next
										strHTML.Append("</table>")
										strHTML.Append("</td></tr>")

									End If

									' End If

									If l_Gross <> 0 Then
										strHTML.Append("<tr><td class=labelbold>Gross Fees: </td><td colspan=4>" & l_Gross.ToString() & "</td></tr>")

										strHTML.Append("<tr><td colspan=5></td></tr>")
										strHTML.Append("<tr><td colspan=5></td></tr>")
										strHTML.Append("<tr><td colspan=5 class=label>Please adjust the fees through manual transacts if required.</td></tr>")

									End If


									strHTML.Append("</table></html>")
								End If
							End If
							''--------Chek end if                         
						End If
					End If
				Next






				'ElseIf VoidUserControl1.Action = "TDLOAN" Then

				'    dglst = CType(VoidUserControl1.FindControl("datalistTDLoan"), DataList)

				'    For Each item As DataListItem In dglst.Items
				'        Dim img As ImageButton
				'        img = item.Controls(0).FindControl("imgTDLoan")
				'        If img.ImageUrl = "../images/selected.gif" Then
				'            datagrid = CType(item.FindControl("dgLoan"), DataGrid)
				'            If Not IsNothing(datagrid) Then
				'                Dim i As Integer
				'                strHTML = New StringBuilder
				'                For i = 0 To datagrid.Items.Count - 1
				'                    Dim chk As CheckBox
				'                    chk = datagrid.Items(i).FindControl("chkLoan")
				'                    If Not IsNothing(chk) Then
				'                        If chk.Checked Then
				'                            ' hdnListDisbId.Value = hdnListDisbId.Value + "," + DataGrid.Items(i).Cells(1).Text
				'                            dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(datagrid.Items(i).Cells(1).Text)

				'                            If Not dsDisbursements Is Nothing Then
				'                                If dsDisbursements.Tables(0).Rows.Count > 0 And dsDisbursements.Tables(1).Rows.Count > 0 Then
				'                                    If i = 0 Then
				'                                        IsFeesExist = True

				'                                        strHTML.Append("<Html>")
				'                                        '-------------CSS------------------------
				'                                        strHTML.Append("<style type=text/css>")
				'                                        strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

				'                                        strHTML.Append("</style>")
				'                                        '-------------------------------------

				'                                        strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id: </td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Disbursement Type: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Voided Date: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

				'                                    End If

				'                                    If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
				'                                        strHTML.Append("<tr><td class=labelbold>Disbursement Number: </td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Issued Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</sapn></td></tr>")
				'                                    Else
				'                                        strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Issued Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td></tr>")
				'                                    End If

				'                                    If dsDisbursements.Tables(1).Rows.Count > 0 Then
				'                                        Dim counter As Integer
				'                                        strHTML.Append("<tr><td colspan=4>")
				'                                        strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
				'                                        For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
				'                                            strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
				'                                            l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

				'                                        Next
				'                                        strHTML.Append("</table>")
				'                                        strHTML.Append("</td></tr>")

				'                                    End If

				'                                End If


				'                            End If

				'                            ''--------Chek end if                         
				'                        End If
				'                    End If
				'                Next
				'                If l_Gross <> 0 Then
				'                    strHTML.Append("<tr><td class=labelbold>Gross Fees: </td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
				'                End If

				'                strHTML.Append("</table></html>")
				'            End If

				'        End If
				'    Next


				'ElseIf VoidUserControl1.Action = "ANNUITY" Then

				'    dglst = CType(VoidUserControl1.FindControl("datalistAnnuity"), DataList)

				'    For Each item As DataListItem In dglst.Items
				'        Dim img As ImageButton
				'        img = item.Controls(0).FindControl("imgAnnuity")
				'        If img.ImageUrl = "../images/selected.gif" Then
				'            datagrid = CType(item.FindControl("dgAnnuity"), DataGrid)
				'            If Not IsNothing(datagrid) Then
				'                Dim i As Integer
				'                strHTML = New StringBuilder
				'                For i = 0 To datagrid.Items.Count - 1
				'                    Dim chk As CheckBox
				'                    chk = datagrid.Items(i).FindControl("chkAnnuity")
				'                    If Not IsNothing(chk) Then
				'                        If chk.Checked Then
				'                            ' hdnListDisbId.Value = hdnListDisbId.Value + "," + DataGrid.Items(i).Cells(1).Text
				'                            dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementInfo(datagrid.Items(i).Cells(1).Text)

				'                            If Not dsDisbursements Is Nothing Then
				'                                If dsDisbursements.Tables(0).Rows.Count > 0 And dsDisbursements.Tables(1).Rows.Count > 0 Then
				'                                    If i = 0 Then
				'                                        IsFeesExist = True

				'                                        strHTML.Append("<Html>")
				'                                        '-------------CSS------------------------
				'                                        strHTML.Append("<style type=text/css>")
				'                                        strHTML.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
				'                                        strHTML.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
				'                                        strHTML.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

				'                                        strHTML.Append("</style>")
				'                                        '-------------------------------------

				'                                        strHTML.Append("<table class =label><tr><td class=labelbold>Fund Id: </td><td colspan=3 >" & dsDisbursements.Tables(0).Rows(0)("FUNDID").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Disbursement Type: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("Type").ToString() & "</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Voided Date: </td><td colspan=3>" & dsDisbursements.Tables(0).Rows(0)("VoidedDate").ToString() & "</td></tr>")

				'                                    End If

				'                                    If dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() <> "" Then
				'                                        strHTML.Append("<tr><td class=labelbold>Disbursement Number: </td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>  Issued Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</sapn></td></tr>")
				'                                    Else
				'                                        strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Issued Date:-<span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td></tr>")
				'                                    End If

				'                                    If dsDisbursements.Tables(1).Rows.Count > 0 Then
				'                                        Dim counter As Integer
				'                                        strHTML.Append("<tr><td colspan=4>")
				'                                        strHTML.Append("<table border=1 class=label width=55%><tr><td colspan=2 class=labelbold> Fees</td></tr>")
				'                                        strHTML.Append("<tr><td class=labelbold>Type</td><td class=labelbold>Amount</td></tr>")
				'                                        For counter = 0 To dsDisbursements.Tables(1).Rows.Count - 1
				'                                            strHTML.Append("<tr><td>" & dsDisbursements.Tables(1).Rows(counter)("chvWithholdingTypeCode").ToString() & "</td><td>" & dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString() & "</td></tr>")
				'                                            l_Gross += Convert.ToDecimal(dsDisbursements.Tables(1).Rows(counter)("mnyAmount").ToString())

				'                                        Next
				'                                        strHTML.Append("</table>")
				'                                        strHTML.Append("</td></tr>")

				'                                    End If

				'                                End If


				'                            End If

				'                        End If
				'                    End If
				'                Next
				'                If l_Gross.ToString() <> "0" Then
				'                    strHTML.Append("<tr><td class=labelbold>Gross Fees:-</td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
				'                End If

				'                strHTML.Append("</table></html>")
				'            End If

				'        End If
				'    Next

			End If
			'=================================================




			If IsFeesExist = False Then

				Dim l_datetime_CompareDate, l_ds_AcctDate As DateTime

				l_ds_AcctDate = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.getAccountingDate()
				l_datetime_CompareDate = DateAdd(DateInterval.Month, 1, Convert.ToDateTime(Session("dsDisbursementsByPersId").Tables(0).Rows(Session("DisbursementByPersIdIndex"))("AccountDate")))
				l_datetime_CompareDate = DateAdd(DateInterval.Minute, 1, l_datetime_CompareDate) 'Handle Boundary condition whther disbursement in previous month
				If (System.DateTime.Compare(l_ds_AcctDate, l_datetime_CompareDate) = 1) Then
					SendMail()
				End If

				Exit Sub
			End If

			If obj.MailService = False Then
				Exit Sub
			End If

			Dim l_string_Activity As String
			l_string_Activity = ""
			l_string_Activity = Request.QueryString.Get("Type")
			obj.SendCc = ""
			obj.MailMessage = strHTML.ToString() '"A withdrawal for fund Id " & l_string_FundIdNo & " which was processed in a prior month has been reversed.  Please review and make necessary interest adjustments as soon as possible."

			If l_string_Activity = "REFUND" Then
				obj.Subject = "Withdrawal from prior month has been reversed."
			Else
				obj.Subject = "Disbursement from prior month has been reversed."
			End If
			obj.MailCategory = "VOID"
			obj.MailFormatType = Mail.MailFormat.Html
			obj.Send()
		Catch
			Throw
		End Try
	End Sub
	Public Sub ChangeStatus(ByVal parameterDisbId As String)
		Dim l_string_NewStatus As String
		Dim l_datarow_CurrentRow As DataRow
		Dim l_integer_CurrentIndex As Integer
		Dim l_string_DisbId As String
		Dim l_string_DisbType As String
		Dim l_datarow_WithholdingInfo As DataRow
		Dim l_boolean_GoForBreakdown As Boolean = False
		Dim l_string_FundEventId As String
		Dim l_double_WHAmount As Double
		Dim l_double_Gross As Double
		Dim l_string_ReplaceId As String
		Dim l_string_PersId As String
		Dim l_integer_GridIndex As Integer
		Dim l_datarow_CurrentRow_PersId As DataRow
		Dim Strarr As Array

		Try
			'l_string_NewStatus = Me.DropDownListStatus.SelectedValue.Trim()

			'l_string_NewStatus = Me.DropDownListStatus.SelectedValue.Trim()
			l_string_NewStatus = "REVERSE"
			l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
			g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

			'Added on 29/09/2009 by imran
			If Not Session("dsDisbursements") Is Nothing Then
				g_dataset_dsDisbursements = Session("dsDisbursements")
				l_integer_GridIndex = Session("DisbursementIndex") 'commented by priya on 12/15/2008 for YRS 5.0-626
				l_datarow_CurrentRow_PersId = g_dataset_dsDisbursements.Tables("Disbursements").Rows(l_integer_GridIndex)
				l_string_PersId = CType(l_datarow_CurrentRow_PersId("PersId"), String)
				litPersID.Text = l_string_PersId
			End If

			If Not g_dataset_dsDisbursementsByPersId Is Nothing Then


				Dim l_dr_CurrentRow As DataRow()
				Dim l_searchExpr As String

				'If Not Session("DisbursementId") Is Nothing Then
				If parameterDisbId <> "" Then

					l_searchExpr = "DisbursementID = '" + parameterDisbId + "'"
					'l_searchExpr = "DisbursementID = '" + Session("DisbursementId") + "'"
					l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
					'l_datarow_CurrentRow = l_dr_CurrentRow(0)
					If l_dr_CurrentRow.Length > 0 Then
						l_datarow_CurrentRow = l_dr_CurrentRow(0)
					Else
						MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data couldnot be retrieved.", MessageBoxButtons.OK)
						Exit Sub
					End If
				Else
					l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
				End If

				If ((l_datarow_CurrentRow("DisbursementID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("DisbursementID").ToString <> "")) Then
					l_string_DisbId = CType(l_datarow_CurrentRow.Item("DisbursementID"), String)
				End If

				If ((l_datarow_CurrentRow("Type").ToString <> "System.DBNull") And (l_datarow_CurrentRow("Type").ToString <> "")) Then
					l_string_DisbType = CType(l_datarow_CurrentRow.Item("Type"), String)
					litDisbID.Text = l_string_DisbId
				End If

				If ((l_datarow_CurrentRow("PayeeEntityID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeEntityID").ToString <> "")) Then
					litPayeeId.Text = l_datarow_CurrentRow.Item("PayeeEntityID").ToString
				End If


				Dim l_string_Activity As String
				l_string_Activity = ""
				l_string_Activity = Request.QueryString.Get("Type")

				If l_string_Activity <> "TDLOAN" And l_string_Activity <> "ANNUITY" Then
					If l_string_NewStatus = "REVERSE" Then	''Void Withrawal


						g_dataset_dsWithholdingInfo = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetWithholdingReverseInfo(hdnRequestID.Value)
						If g_dataset_dsWithholdingInfo.Tables("WithholdingInfo").Rows.Count > 0 Then

							l_datarow_WithholdingInfo = g_dataset_dsWithholdingInfo.Tables("WithholdingInfo").Rows(0)


							If ((l_datarow_WithholdingInfo("FundEventID").ToString <> "System.DBNull") And (l_datarow_WithholdingInfo("FundEventID").ToString <> "")) Then
								l_string_FundEventId = Convert.ToString(l_datarow_WithholdingInfo("FundEventID"))
								litFundID.Text = l_string_FundEventId
							Else
								litFundID.Text = ""
							End If

							If ((l_datarow_WithholdingInfo("WithHoldingAmount").ToString <> "System.DBNull") And (l_datarow_WithholdingInfo("WithHoldingAmount").ToString <> "")) Then
								l_double_WHAmount = CType(l_datarow_WithholdingInfo("WithHoldingAmount"), Double)
								litWHAmount.Text = l_double_WHAmount
							End If

							If ((l_datarow_WithholdingInfo("GrossAmount").ToString <> "System.DBNull") And (l_datarow_WithholdingInfo("GrossAmount").ToString <> "")) Then
								l_double_Gross = CType(l_datarow_WithholdingInfo("GrossAmount"), Double)
								litGross.Text = l_double_Gross
							End If
							'Commented by imran on 1/11/2009 Only Check From Refund Table 
							'If l_string_NewStatus = "REVERSE" And (CType(l_datarow_WithholdingInfo("DetailRows"), Integer) = 0 Or CType(l_datarow_WithholdingInfo("RefundRefRows"), Integer) = 0) Then
							If l_string_NewStatus = "REVERSE" And (CType(l_datarow_WithholdingInfo("RefundRefRows"), Integer) = 0) Then
								l_boolean_GoForBreakdown = True
							End If

						End If


						If l_boolean_GoForBreakdown Then

							Dim popupScript As String = "<script language='javascript'>" & _
							 "window.open('DisbursementsRevese.aspx?PersId=" + litPersID.Text.Trim() + "&DisbId=" + hdnListDisbId.Value + "&RequestId=" + hdnRequestID.Value + "&FundId=" + litFundID.Text.Trim() + "&WHAmount=" + litWHAmount.Text.Trim() + "&Gross=" + litGross.Text.Trim() + "&Status=" + l_string_NewStatus + "&DisbNo=" + litDisbNbr.Text.Trim() + "&AddId=" + litAddressID.Text.Trim() + "', 'CustomPopUp', " & _
							 "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
							 "</script>"

							If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
								Page.RegisterStartupScript("PopupScript2", popupScript)
							End If
						Else
							savevoidReverse()
						End If

					End If
				ElseIf l_string_Activity = "TDLOAN" Or l_string_Activity = "ANNUITY" Then ''Annuity
					savevoidReverse()
				End If
			End If



		Catch
			Throw
		End Try

	End Sub
	Public Sub PopulateDisbursementTextBoxes(ByVal DisbursementID As String)
		Dim l_integer_CurrentIndex As Integer
		Dim l_datarow_CurrentRow As DataRow

		Try

			g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

			If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
				Dim l_dr_CurrentRow As DataRow()
				Dim l_searchExpr As String
				'If Not Session("DisbursementId") Is Nothing Then
				If Not DisbursementID Is Nothing Then
					l_searchExpr = "DisbursementID = '" & DisbursementID & " '"	'+ Session("DisbursementId") + "'"
					l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
					If l_dr_CurrentRow.Length > 0 Then
						l_datarow_CurrentRow = l_dr_CurrentRow(0)
						'Else

						'    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data couldnot be retrieved.", MessageBoxButtons.OK)
						'    Exit Sub
					End If
					'Else
					'    Throw New Exception("DisbusrementId not found.")
					'l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
				End If

				If Not l_datarow_CurrentRow Is Nothing Then
					If ((l_datarow_CurrentRow("PayeeAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeAddress").ToString <> "")) Then
						Me.TextBoxAddress.Text = CType(l_datarow_CurrentRow.Item("PayeeAddress"), String)
					Else
						Me.TextBoxAddress.Text = "No address found"
					End If

					If ((l_datarow_CurrentRow("AcctNumber").ToString <> "System.DBNull") And (l_datarow_CurrentRow("AcctNumber").ToString <> "")) Then
						Me.TextBoxAccountNo.Text = CType(l_datarow_CurrentRow.Item("AcctNumber"), String)
					Else
						Me.TextBoxAccountNo.Text = ""
					End If

					If ((l_datarow_CurrentRow("LegalEntityType").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityType").ToString <> "")) Then
						Me.TextBoxEntityType.Text = CType(l_datarow_CurrentRow.Item("LegalEntityType"), String)
					Else
						Me.TextBoxEntityType.Text = ""
					End If

					If ((l_datarow_CurrentRow("BankInfo").ToString <> "System.DBNull") And (l_datarow_CurrentRow("BankInfo").ToString <> "")) Then
						Me.TextBoxBankInfo.Text = CType(l_datarow_CurrentRow.Item("BankInfo"), String)
					Else
						Me.TextBoxBankInfo.Text = ""
					End If

					If ((l_datarow_CurrentRow("LegalEntityAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityAddress").ToString <> "")) Then
						Me.TextBoxEntityAddress.Text = CType(l_datarow_CurrentRow.Item("LegalEntityAddress"), String)
					Else
						Me.TextBoxEntityAddress.Text = ""
					End If


					If ((l_datarow_CurrentRow("LegalEntityName").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityName").ToString <> "")) Then
						Me.TextBoxLegalEntity.Text = CType(l_datarow_CurrentRow.Item("LegalEntityName"), String)
					Else
						Me.TextBoxLegalEntity.Text = ""
					End If

					'If ((l_datarow_CurrentRow("ActionNotes").ToString <> "System.DBNull") And (l_datarow_CurrentRow("ActionNotes").ToString <> "")) Then
					'    Me.TextBoxNotes.Text = CType(l_datarow_CurrentRow.Item("ActionNotes"), String)
					'Else
					'    Me.TextBoxNotes.Text = ""
					'End If
				Else
					ClearDisbursementTextBoxes()
				End If
			End If
		Catch
			Throw
		End Try
	End Sub
	Private Sub Clear_Session()
		Session("flag") = Nothing
		Session("LoanReplace") = Nothing
		Session("CashOutsReplace") = Nothing
		Session("ReplaceFees") = Nothing
		Session("VRManager_Sort") = Nothing
		Session("ShowDisbursement") = Nothing
		Session("DisbursementByPersIdIndex") = Nothing
		'Priya 29-April-2010 : YRS 5.0-1060-Voiding a Check
		Session("StatusType") = Nothing
		Session("ANNPaid") = Nothing
		'End YRS 5.0-1060
		Session("g_search_flag") = Nothing
		'31-Jan-2011		priya		YRS 5.0-1217 : MRD Part-II changes
		Session("PromptInActiveMRDReversal") = Nothing
		'END 31-Jan-2011	 YRS 5.0-1217 : MRD Part-II changes
	End Sub
	Private Sub ValidateVoidReverse()
		Dim l_datarow_CurrentRow As DataRow
		Dim l_integer_CurrentIndex As Integer
		Dim l_datetime_CompareDate As DateTime
		Dim Strarr As Array
		Dim StrListDisbId As String
		Dim parameterDisbId As String
		Dim l_string_RelDisbs As String
		Try


			StrListDisbId = hdnListDisbId.Value
			If hdnDisbId.Value.Length > 0 Then
				StrListDisbId = StrListDisbId.Replace(hdnDisbId.Value, "")
			End If

			If StrListDisbId.Length > 0 Then
				'hdnDisbId.Value = hdnDisbId.Value.Substring(1, hdnDisbId.Value.Length - 1)
				Strarr = StrListDisbId.Split(",")
				Dim i As Integer
				If Strarr.Length >= 2 Then
					For i = 1 To Strarr.Length - 1
						hdnDisbId.Value = Strarr.GetValue(i)
						parameterDisbId = hdnDisbId.Value

						g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")


						If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
							''**If Not Session("DisbursementId") Is Nothing Then
							Dim l_dr_CurrentRow As DataRow()
							Dim l_searchExpr As String
							l_searchExpr = "DisbursementID = '" + parameterDisbId + "'"
							l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
							If l_dr_CurrentRow.Length > 0 Then
								l_datarow_CurrentRow = l_dr_CurrentRow(0)

								'1. Validate Disbursement alredy voided    
								If CType(l_datarow_CurrentRow("Voided"), String).ToUpper = "YES" And (CType(l_datarow_CurrentRow("Reversed"), String)).ToUpper = "YES" Then
									MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement(s) are already voided and reversed. Cannot Reverse.", MessageBoxButtons.Stop)
									Exit Sub
								End If

								'2.validate date of issue older 

								l_datetime_CompareDate = DateAdd(DateInterval.Year, 1, Convert.ToDateTime(l_datarow_CurrentRow("Issueddate")))
								If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then
									l_string_RelDisbs = l_string_RelDisbs + "," + l_datarow_CurrentRow("Number")
								End If


								'31-Jan-2011	 priya YRS 5.0-1217 : MRD Part-II changes
								'3.MRDSatisfy for previous financial year
								'31-Jan-2011	 priya YRS 5.0-1217 : MRD Part-II changes
								'If dr(0).Item("MRDSatisfy") = 1 Then
								'	MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Disbursements satisfy MRD amount for the previous financial year. Cannot reverse disbursement.", MessageBoxButtons.Stop, False)
								'	Me.TabStripVRManager.Items(1).Enabled = False
                                '	Exit Sub
								'End If
								'If dr(0).Item("PromptInActiveMRDReversal") = 1 Then
								'	MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "The disbursement you wish to reverse satisfies the participant’s MRD obligation. Do you wish to continue?", MessageBoxButtons.YesNo, False)
								'	Me.TabStripVRManager.Items(1).Enabled = False

								'	Session("PromptInActiveMRDReversal") = "1"
								'	Exit Sub
								'End If


								If l_datarow_CurrentRow("StopMRDReversal") = 1 Then
									MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursements satisfy MRD amount for the previous financial year. Cannot reverse disbursement.", MessageBoxButtons.Stop)
									Exit Sub
								End If
								'4.Inactive participant but MRS is satify 
                                'BT: 964- Problem in Withdrawal->Void-Reverse
                                If Session("PromptInActiveMRDReversal") <> "YES" AndAlso Session("PromptInActiveMRDReversal") <> "1" Then
                                    If l_datarow_CurrentRow("PromptInActiveMRDReversal") = 1 Then
                                        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "The disbursement you wish to reverse satisfies the participant’s MRD obligation. Do you wish to continue?", MessageBoxButtons.YesNo)
                                        Session("PromptInActiveMRDReversal") = "1"
                                        Exit Sub
                                    End If
                                End If
                                'End 31-Jan-2011	 priya YRS 5.0-1217 : MRD Part-II changes

                                'l_datetime_CompareDate = DateAdd(DateInterval.Year, 1, Convert.ToDateTime(l_datarow_CurrentRow("Issueddate")))
                                'If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then
                                '    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Issue date for disbursement #" & l_datarow_CurrentRow("Number") & " is older than a year. Do you wish to continue?", MessageBoxButtons.YesNo)
                                '    Session("flag") = "Pending"
                                '    Exit Sub
                                'End If


                            End If
                        End If

                        '------
                    Next
					If l_string_RelDisbs <> "" Then
						l_string_RelDisbs = l_string_RelDisbs.Substring(1, l_string_RelDisbs.Length - 1)
						MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Issue date for disbursement #" & l_string_RelDisbs & " is older than a year. Do you wish to continue?", MessageBoxButtons.YesNo)
						Session("flag") = "Pending"
						Exit Sub
					End If
				End If
			End If

			'Added Imran on 05/10/2009

			StrListDisbId = hdnListDisbId.Value
			If StrListDisbId.Length > 0 Then
				Strarr = StrListDisbId.Split(",")
				Dim i As Integer
				If Strarr.Length >= 2 Then
					Session("_iCounter") = 0
					'For i = 1 To Strarr.Length - 1
					If Strarr.GetValue(1) <> "" Then
						ChangeStatus(Strarr.GetValue(1))
					End If
					'Next
				End If
			End If

		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Throw (ex)
		End Try
	End Sub
	'Added on 09/10/2009
	Private Sub savevoidReverse()
		Dim l_dataset_RefundDetails As New DataSet
		Dim strOutPut As String
		Dim parameterListDisbId As Array
		Dim l_string_Type As String
		Dim Interestsatus As Integer
		Interestsatus = 0
		Try
			l_string_Type = Request.QueryString.Get("Type")


			If Session("flag") <> "YInterest" And Session("flag") <> "NInterest" And l_string_Type <> "ANNUITY" Then
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Do you wish to create Interest adjustment records?", MessageBoxButtons.YesNo)
				Session("flag") = "YInterest"
				Exit Sub
			End If


			If hdnListDisbId.Value.Length > 0 Then
				parameterListDisbId = hdnListDisbId.Value.Split(",")
			End If
			If Not Session("RefundDetails") Is Nothing Then
				l_dataset_RefundDetails = Session("RefundDetails")
				Session("RefundDetails") = Nothing
			End If
			ButtonSave.Enabled = True
			If Session("flag") = "YInterest" Then
				Interestsatus = 1
			End If

			strOutPut = ""
			strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoVoidReverse(parameterListDisbId, l_dataset_RefundDetails, l_string_Type, "", Interestsatus, DropDownStatus.SelectedValue)

			If (strOutPut <> "") Then
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", strOutPut, MessageBoxButtons.Stop)
				Exit Sub
			Else
				'MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Request has been reversed successfully.", MessageBoxButtons.OK)
				'Added by imran on 14/12/2009  change save popup messages according to type 
				If l_string_Type = "REFUND" Then
					MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Request has been reversed successfully.", MessageBoxButtons.OK)
				Else
					MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement has been reversed successfully.", MessageBoxButtons.OK)
				End If


				'ButtonSave.Enabled = False
				Session("g_search_flag") = False

				'Added by imran For BT 1015 
				SendMailForFees()
				Exit Sub
			End If
		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Throw (ex)
		End Try





	End Sub
	'Added on 22/10/2009 For add Header label
	Public Sub LoadGeneraltab(ByVal l_string_PersId As String)
		Try

			Dim g_dataset_GeneralInfo As DataSet
			Dim FName As String
			Dim MName As String
			Dim LName As String
			Dim SSNO As String
			Dim l_string_Type As String
			Dim g_dataset_dsStatusList As New DataSet
			Dim l_dataset_Status As New DataSet
			Dim l_integer_GridIndex As Integer
			Dim l_datarow_CurrentRow_PersId As DataRow



			LabelFundStatus.Text = ""
			If Not Request.QueryString.Get("Type") Is Nothing Then
				l_string_Type = Request.QueryString.Get("Type")
                If l_string_Type = "REFUND" Then
                    Headercontrol.pageTitle = "Void and Reverse Disbursement / Withdrawals"
                ElseIf (l_string_Type = "TDLOAN") Then
                    Headercontrol.pageTitle = "Void and Reverse Disbursement / Loan"
                ElseIf (l_string_Type = "ANNUITY") Then
                    Headercontrol.pageTitle = "Void and Reverse Disbursement / Annuity"
                Else
                    Headercontrol.pageTitle = "Void Disbursement"
                End If

			End If


			If l_string_PersId <> "" Then
				g_dataset_GeneralInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(l_string_PersId)
				If Not g_dataset_GeneralInfo Is Nothing Then
					If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "") Then
						FName = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString().Trim
					End If

					If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "") Then
						LName = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString().Trim
					End If
					If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "") Then
						MName = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString().Trim
					End If

					If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "") Then
						SSNO = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString().Trim
					End If

					Session("Person_Info") = Me.TextBoxSSNo.Text
					Dim strSSN As String = SSNO.Insert(3, "-")
					strSSN = strSSN.Insert(6, "-")

                    'Shashi Shekhar     28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                        Headercontrol.FundNo = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim
                    End If

				End If
			Else
				g_dataset_dsStatusList = YMCARET.YmcaBusinessObject.DisbursementReversalBOClass.GetStatusList()
				If (g_dataset_dsStatusList.Tables("StatusList").Rows.Count > 0 And DropDownStatus.Items.Count <= 0) Then
					'If (g_dataset_dsStatusList.Tables("StatusList").Rows.Count > 0) Then
					Me.DropDownStatus.ClearSelection()
					Me.DropDownStatus.Items.Clear()
					Me.DropDownStatus.DataSource = g_dataset_dsStatusList
					Me.DropDownStatus.DataTextField = g_dataset_dsStatusList.Tables("StatusList").Columns("Description").ColumnName
					Me.DropDownStatus.DataValueField = g_dataset_dsStatusList.Tables("StatusList").Columns("FundStatus").ColumnName
					Me.DropDownStatus.DataBind()
					DropDownStatus.Enabled = False

				End If
				If (DropDownStatus.Items.Contains(New ListItem("Select One", "0")) = False) Then
					DropDownStatus.Items.Add(New ListItem("Select One", "0"))
					Me.DropDownStatus.SelectedValue = "0"
				End If



			End If


		Catch ex As Exception
			Throw
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
		End Try

	End Sub

	Public Sub GetFundStatusByRequestId(ByVal RequestID As String)
		Dim l_dataset_Status As New DataSet
		Dim l_Current_Status As String
		Dim l_Recommended_Status As String

		Try

			hdnCurrentFundStatus.Value = ""
			hdnRecommendedFundStatus.Value = ""


			If RequestID = "" Then
				LabelFundStatus.Text = ""
				DropDownStatus.Enabled = False
				DropDownStatus.SelectedValue = "0"
				hdnCurrentFundStatus.Value = ""
				hdnRecommendedFundStatus.Value = ""
				Exit Sub
			End If

			l_dataset_Status = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetFundStatusByPersId(Me.DataGridVRManager.SelectedItem.Cells(1).Text.Trim, RequestID)

			If Not l_dataset_Status Is Nothing Then

				If l_dataset_Status.Tables.Count > 0 Then
					DropDownStatus.Enabled = True
					l_Current_Status = l_dataset_Status.Tables("Status").Rows(0).Item("Currentstatus").ToString()
					l_Recommended_Status = l_dataset_Status.Tables("Status").Rows(0).Item("RecommendedStatus").ToString()
					'LabelFundStatus.Text = "Current Fund Status is  <b>" + l_dataset_Status.Tables("Status").Rows(0).Item("CurrentstatusDescription").ToString() + "  </b> and recommended Fund Status  is " + IIf(l_Recommended_Status = "", "Unable to determine", l_dataset_Status.Tables("Status").Rows(0).Item("RecommendedStatusDescription").ToString()) + ". Please change if necessary."
					If l_Recommended_Status = "" Then
						LabelFundStatus.Text = "Current Fund Status is  <b>" + l_dataset_Status.Tables("Status").Rows(0).Item("CurrentstatusDescription").ToString() + "  </b> but the system is unable to recommend a new status for update. Please select the new fund event status form the dropdown list."
					Else
						'LabelFundStatus.Text = "Current Fund Status is  <b>" + l_dataset_Status.Tables("Status").Rows(0).Item("CurrentstatusDescription").ToString() + "  </b>. Recommended Fund Status is  " + l_dataset_Status.Tables("Status").Rows(0).Item("RecommendedStatusDescription").ToString() + ". Current status has been pre-selected, please change if necessary."
						LabelFundStatus.Text = "Recommended Fund Status has been pre-selected. Please change if necessary."
					End If
					'DropDownStatus.SelectedValue = IIf(l_Recommended_Status = "", "0", l_Current_Status)
					DropDownStatus.SelectedValue = IIf(l_Recommended_Status = "", l_Current_Status, l_Recommended_Status)
					hdnCurrentFundStatus.Value = l_Current_Status
					hdnRecommendedFundStatus.Value = l_Recommended_Status
				Else
					LabelFundStatus.Text = "Unable to determine current fund event status and recommended status for this request."
					DropDownStatus.Enabled = False
					DropDownStatus.SelectedValue = "0"
					hdnCurrentFundStatus.Value = ""
					hdnRecommendedFundStatus.Value = ""
				End If
			Else
				LabelFundStatus.Text = "Unable to determine current fund event status and recommended status for this request."
				DropDownStatus.Enabled = False
				DropDownStatus.SelectedValue = "0"
				hdnCurrentFundStatus.Value = ""
				hdnRecommendedFundStatus.Value = ""
			End If




		Catch
			Throw
		End Try
	End Sub
#End Region


End Class
