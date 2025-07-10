'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	VoidDisbursementVRManager.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:19:42 PM
' Program Specification Name	:	YMCA PS 3.12.3.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			Changed on      Change Description
'**  DATE               AUTHOR          REASONS
'**  30/10/2009         imran           For BT 1003
'**  30/10/2009         Imran           Save and PHR button in single line
'**  05/11/2009         Priya J         changes made to view datagrid after no records found message.
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'**  12/11/2009         Imran           Remove validation on require Request id for disbursement.     
'**  13/11/2009         Imran           For BT 1014- Previous person details shows in disbursement tab.     
'**  20/11/2009         Imran           Trimming of search fields 
'**  19/12/2009         Imran           For Gemini Issue  968 -When disbursement is cash check then checkbox disable
'    15/Feb/2010        Shashi Shekhar  Restrict Data Archived Participants To proceed in Find list.
'**  16/June/2010       Imran           Enhancement changes
'    30/July/2010       Imran           YRS 5.0-1140/ BT-564  -Add validation on require Request id for disbursement.
' Shashi Shekhar:   2010-12-09:         For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'**  18-Feb-2011	Priya			If request is MRD then Block void reissue
'****************************************************
'Modification History
'****************************************************
'Modified by        Date               Description
'****************************************************
'Shashi Shekhar     28 Feb 2011          Replacing Header formating with user control (YRS 5.0-450 )
'Imran              27-Apr-2011          BT:825- Security issue in void reverse
'Imran              30-May-2011          BT:836 Getting Error while Void-Reissue   
'Sanjay             2014.08.13           BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
'Manthan Rajguru    2015.09.16           YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
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

Public Class VoidDisbursementVRManager_1
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("VoidDisbursementVRManager.aspx?Activity=Reissue") 'Shashi shekhar:2009-12-17:Modify the strFormName value to resolve security issue
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridVRManager As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridDisbursements As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripVRManager As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageVRManager As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelRecordNotFound As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeSSN As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox



    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelCheckNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCheckNo As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelAddressCheckSent As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress As System.Web.UI.WebControls.TextBox

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
    Protected WithEvents hdnDisbId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdnListDisbId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ButtonCloseVR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
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

#Region "PROPERTY"
    Private Property FundNo() As String
        Get
            If Not Session("FundNo") Is Nothing Then
                Return (CType(Session("FundNo"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("FundNo") = Value
        End Set
    End Property
    Private Property PersonName() As String
        Get
            If Not Session("PersonName") Is Nothing Then
                Return (CType(Session("PersonName"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonName") = Value
        End Set
    End Property
    Private Property IssuedDate() As String
        Get
            If Not Session("IssuedDate") Is Nothing Then
                Return (CType(Session("IssuedDate"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("IssuedDate") = Value
        End Set
    End Property
    Private Property PaymentMethod() As String
        Get
            If Not Session("PaymentMethod") Is Nothing Then
                Return (CType(Session("PaymentMethod"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PaymentMethod") = Value
        End Set
    End Property
    Private Property CheckNumber() As String
        Get
            If Not Session("CheckNumber") Is Nothing Then
                Return (CType(Session("CheckNumber"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("CheckNumber") = Value
        End Set
    End Property
    Private Property DisbursementType() As String
        Get
            If Not Session("DisbursementType") Is Nothing Then
                Return (CType(Session("DisbursementType"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("DisbursementType") = Value
        End Set
    End Property
#End Region

#Region "EVENTS"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        ''Shubhrata 05-Sep-2006 TD Loans Phase 2
        'If Request.QueryString.Get("Activity") Is Nothing Then
        '    Response.Redirect("MainWebForm.aspx", False)
        'End If
        'Shubhrata 05-Sep-2006 TD Loans Phase 2

        Try

            If Not (IsPostBack) Then

                Clear_Session()
                g_boolean_flag = False

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

            'Me.LabelAddressCheckSent.AssociatedControlID = Me.TextBoxAddress.ID

            'Me.LabelAccNo.AssociatedControlID = Me.TextBoxAccountNo.ID

            'Me.LabelBankInfo.AssociatedControlID = Me.TextBoxBankInfo.ID

            'Me.LabelEntityType.AssociatedControlID = Me.TextBoxEntityType.ID

            'Me.LabelEntityAddress.AssociatedControlID = Me.TextBoxEntityAddress.ID
            '
            'Me.LabelLegalEntity.AssociatedControlID = Me.TextBoxLegalEntity.ID

            'Me.LabelStatus.AssociatedControlID = Me.DropDownListStatus.ID

            'Me.LabelReason.AssociatedControlID = Me.DropDownListReason.ID

            'Me.LabelNotes.AssociatedControlID = Me.TextBoxNotes.ID
            Me.LabelRecordNotFound.Visible = False

            'Me.DropDownListReason.Attributes.Add("onchange", "Javascript:_OnBlur_DropdownReason();")

            If Not IsPostBack Then
                Me.TabStripVRManager.Items(1).Enabled = False
                Dim l_string_Activity As String
                LoadGeneraltab("")

            End If

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            'ButtonSave.Attributes.Add("onclick", "javascript:return OpenBreakdownWindow('" + litPersID.Text + "','" + litDisbID.Text + "','" + litFundID.Text + "','" + litFlag.Text + "');")
            If Request.Form("OK") = "OK" Then
                FinalStatus()
            End If


            AddHandler VoidUserControl1.DisbursementbyPersIdEvent, AddressOf PopulateDisbursementTextBoxes
            AddHandler VoidUserControl1.ClearDisbursementGridEvent, AddressOf ClearDisbursementTextBoxes


            If Request.Form("Yes") = "Yes" Then
                If Not Session("flag") Is Nothing Then
                    If Session("flag") = "Pending" Then
                        Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                        Session("flag") = "YesContinue"
                        hdnDisbId.Value = hdnListDisbId.Value
                        ValidateVoidReissue()
                        SendReissueMail()

                    End If

                End If


            End If

            If Request.Form("No") = "No" Then

                If Session("flag") = "Pending" Then
                    Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                    Session("flag") = "notpending"
                    'BT:836 Getting Error while Void-Reissue   
                    'hdnListDisbId.Value = hdnListDisbId.Value.Replace(hdnDisbId.Value, "")
                    hdnListDisbId.Value = hdnListDisbId.Value.Replace("," + hdnDisbId.Value, "")
                    ValidateVoidReissue()
                End If
            End If

            If Request.Form("MessageBoxIDNo") = "Cancel" Then
                SearchAndPopulateData(True)
            End If

            If Request.Form("OK") = "OK" Then
                'Added by imran on 29/10/2009 
                ClearDisbursementTextBoxes()
                VoidUserControl1.PopulateDisbursementGrid()
            End If
			'Added by imran on 30/10/2009 For BT 1003
		Catch ex As SqlException
			Session("Called") = False
			Dim l_string_Type As String
			l_string_Type = Request.QueryString.Get("Type")
			If ex.Number = 60006 Then
				' Response.Redirect("VoidDisbursementVRManager.aspx?Type=" & l_string_Type & "")
				LoadGeneraltab("")
				'SearchAndPopulateData(True)
				If Not IsNothing(DataGridVRManager) Then
					Me.DataGridVRManager.SelectedIndex = -1
				End If
				RepopulatePopulateData()
				Me.TabStripVRManager.Items(1).Enabled = False
				Me.MultiPageVRManager.SelectedIndex = 0
				Me.TabStripVRManager.SelectedIndex = 0
			End If
		Catch ex As Exception
			'Added by imran on 20/10/2009 
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
                '** '** ButtonSave.Enabled = False
                Session("DisbursementByPersIdIndex") = 0
                'aparna yren-2646
                Session("DisbursementId") = Nothing
                'aparna
                SearchAndPopulateData(True)
            Else
                Me.MultiPageVRManager.SelectedIndex = Me.TabStripVRManager.SelectedIndex
                '**  '** ButtonSave.Enabled = False
                Session("DisbursementByPersIdIndex") = 0
                'aparna yren-2646
                Session("DisbursementId") = Nothing
                'aparna
                'SearchAndPopulateData(True)

            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "") 'by Aparna 19/09/2007
            'Added by imran on 20/11/2009 For Triming search fields
            TextBoxFundNo.Text = TextBoxFundNo.Text.Trim()
            TextBoxSSNo.Text = TextBoxSSNo.Text.Trim()
            TextBoxCheckNo.Text = TextBoxCheckNo.Text.Trim()
            TextBoxFirstName.Text = TextBoxFirstName.Text.Trim()
            TextBoxLastName.Text = TextBoxLastName.Text.Trim()



            'Call this method to get the list of participant
            If Not (TextBoxCheckNo.Text = "" And TextBoxFirstName.Text = "" And TextBoxLastName.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNo.Text = "") Then

                Session("DisbursementIndex") = 0
                Session("DisbursementByPersIdIndex") = 0
                Session("DisbursementId") = Nothing
                Clear_Session()
                'Added by imran on 13/11/2009 For BT-1014
                Me.TabStripVRManager.Items(1).Enabled = False
                SearchAndPopulateData(False)
                'Added no 22/10/2009 For Label Header
                LoadGeneraltab("")
                'If Me.DataGridVRManager.Items.Count > 0 Then
                '    Me.TabStripVRManager.Items(1).Enabled = True

                'End If
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
            Dim l_string_Activity As String

            If Not Request.QueryString.Get("Activity") Is Nothing Then
                l_string_Activity = Request.QueryString.Get("Activity")
            End If


            If l_string_Activity = "Void" Then

                If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                    MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.PageTitle = "Void Disbursement"
                    Exit Sub
                End If

            ElseIf l_string_Activity = "Reissue" Then

                If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                    MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.PageTitle = "Void and Reissue Disbursement / Withdrawals"
                    Exit Sub
                End If

            Else

                If Me.DataGridVRManager.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
                    MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripVRManager.Items(1).Enabled = False
                    Headercontrol.PageTitle = "Void Disbursement"
                    Exit Sub
                End If

            End If


            '-----------------------------------------------------------------------------------------------


            Session("DisbursementIndex") = DataGridVRManager.SelectedIndex
            Session("_iCounter") = 0
            Session("SSNoVRManager") = Me.DataGridVRManager.SelectedItem.Cells(2).Text.Trim
            FundNo = DataGridVRManager.SelectedItem.Cells(6).Text.Trim
            PersonName = DataGridVRManager.SelectedItem.Cells(3).Text.Trim + DataGridVRManager.SelectedItem.Cells(4).Text.Trim + DataGridVRManager.SelectedItem.Cells(5).Text.Trim


            'aparna 10/01/2007
            'Me.DataGridDisbursements.SelectedIndex = 0
            Session("DisbursementByPersIdIndex") = 0 'added by priya on 12/15/2008 for YRS 5.0-626
            'aparna 10/01/2007
            ' PopulateDisbursementGrid()

            ClearDisbursementTextBoxes()

            If Me.DataGridVRManager.Items.Count > 0 Then
                Me.TabStripVRManager.Items(1).Enabled = True

            End If
			Me.VoidUserControl1.PopulateDisbursementGrid()
			Me.MultiPageVRManager.SelectedIndex = 1
			Me.TabStripVRManager.SelectedIndex = 1
			ButtonSave.Enabled = True
			'If LabelNoRecordFound.Visible = False Then
			'End If
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
			'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
			Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

			If Not checkSecurity.Equals("True") Then
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
				Exit Sub
			End If
			'End : YRS 5.0-940
			Dim datagrid As DataGrid
			Dim dglst As DataList
			Dim cnt As Integer
			cnt = 0
			hdnDisbId.Value = ""
			Session("flag") = Nothing
			hdnListDisbId.Value = ""
			dglst = CType(VoidUserControl1.FindControl("datalistRefund"), DataList)

			PaymentMethod = ""
			CheckNumber = ""
			DisbursementType = ""
			IssuedDate = ""

			For Each item As DataListItem In dglst.Items
				Dim img As ImageButton
				img = item.Controls(0).FindControl("Imagebutton1")
				If img.ImageUrl = "../images/selected.gif" Then
					datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
					If Not IsNothing(datagrid) Then
						Dim i As Integer

						i = Session("DisbursementByPersIdIndex")
						For i = 0 To datagrid.Items.Count - 1
							Dim chk As CheckBox
							Dim LabelDisbId As Label
							chk = datagrid.Items(i).FindControl("chkRefund")
							LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
							If Not IsNothing(chk) Then
								If chk.Checked Then
									'PopulateDisbursementTextBoxes(datagrid.Items(i).Cells(1).Text.Trim())
									'Added by imran 12/11/2009 Removing validation    
									'If LabelDisbId.Text.ToUpper = dglst.DataKeys(cnt).ToString().ToUpper Then
									'    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Request does not exist for selected Disbursement(s).", MessageBoxButtons.Stop)
									'    Exit Sub
									'End If
									' Added by Imran as on 30/July/2010 :YRS 5.0-1140 /  BT-564 -Add validation on require Request id for disbursement. 
									If LabelDisbId.Text.ToUpper = dglst.DataKeys(cnt).ToString().ToUpper Then
										MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Necessary data is missing. Unable to complete the void process. Please refer this to the IT dept for a manual correction.", MessageBoxButtons.Stop)
										Exit Sub
									End If
									hdnListDisbId.Value = hdnListDisbId.Value + "," + LabelDisbId.Text.Trim()
									If PaymentMethod.Trim <> "" Then
										PaymentMethod = PaymentMethod & "," & datagrid.Items(i).Cells(8).Text.Trim
									Else
										PaymentMethod = datagrid.Items(i).Cells(8).Text.Trim

									End If

									If CheckNumber.Trim <> "" Then
										CheckNumber = CheckNumber.Trim & "," & datagrid.Items(i).Cells(3).Text.Trim

									Else
										CheckNumber = datagrid.Items(i).Cells(3).Text.Trim
									End If
									If DisbursementType.Trim <> "" Then
										DisbursementType = DisbursementType.Trim & "," & datagrid.Items(i).Cells(2).Text.Trim

									Else
										DisbursementType = datagrid.Items(i).Cells(2).Text.Trim

									End If

									If IssuedDate.Trim <> "" Then
										IssuedDate = IssuedDate.Trim & "," & datagrid.Items(i).Cells(5).Text.Trim

									Else
										IssuedDate = datagrid.Items(i).Cells(5).Text.Trim

									End If

								End If
							End If

						Next
					End If

				End If
				cnt += 1
			Next

			If hdnListDisbId.Value.Length = 0 Then
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select a Disbursement.", MessageBoxButtons.Stop)
				Exit Sub
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

					'l_datarow_CurrentRow = l_dr_CurrentRow(0)
					'If CType(l_datarow_CurrentRow("bitpaid"), String).ToUpper = "TRUE" Then
					'End If
					MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement(s) are cashed. Cannot void.", MessageBoxButtons.Stop)
					Exit Sub

				End If
			End If

			ValidateVoidReissue()
			'SaveVoid()
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
				'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
				'g_dataset_dsDisbursements = viewstate("DS_Sort_VRManager")
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
	'Method to populate the datagrid with the list of employees.
	Public Sub SearchAndPopulateData(ByVal _postback As Boolean)
		Dim l_integer_AnnuityOnly As Integer
		Dim l_string_Activity As String
		Try
			'l_string_Activity = Request.QueryString.Get("Activity")
			'VoidUserControl1.Activity = Request.QueryString.Get("Activity")
			VoidUserControl1.Action = "REFUND"
			VoidUserControl1.Activity = "REFUNDRISSUE"


			l_string_Activity = VoidUserControl1.Activity

			If (l_string_Activity = "Void") Or (l_string_Activity = "VReplace") Then
				l_integer_AnnuityOnly = 1
				'added by hafiz on 31-Aug-2006 - TD Loans Phase 2
				'Shubhrata May5th,2007 YREN-3276 the if condition is being modified to set a diff parameter value for Void and Replace of Loan 
				'to fetch even those records for which there are entries in atsLoanAmortization.
			ElseIf (l_string_Activity = "LOANREVERSE") Then
				l_integer_AnnuityOnly = 2
				'added by hafiz on 31-Aug-2006 - TD Loans Phase 2
				'Added by Shubhrata on 1st Dec 2006- to accomodate CashOuts
				'ElseIf (l_string_Activity = "CashOuts") Then
				'    l_integer_AnnuityOnly = 3
			ElseIf (l_string_Activity = "LOANREPLACE") Then
				l_integer_AnnuityOnly = 21
			Else
				l_integer_AnnuityOnly = 0
			End If



			If Not _postback Then
				g_dataset_dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.LookUpDisbursements(Me.TextBoxFundNo.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxSSNo.Text.Trim(), Me.TextBoxCheckNo.Text.Trim(), l_integer_AnnuityOnly)
				'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
				'ViewState("dsDisbursement") = g_dataset_dsDisbursements

				'g_dataset_dsDisbursements.Tables(0).DefaultView.Table().Select("order by Number desc")
				Session("dsDisbursements") = g_dataset_dsDisbursements

			Else
				'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
				'If Not ViewState("dsDisbursement") Is Nothing Then
				'    g_dataset_dsDisbursements = ViewState("dsDisbursement")
				'End If

				If Not Session("dsDisbursements") Is Nothing Then
					g_dataset_dsDisbursements = Session("dsDisbursements")
				End If

			End If

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
		Catch ex As SqlException
			'Throw (ex)
			'Added on 22/10/2009 by imran
			Session("g_search_flag") = True
			Me.TabStripVRManager.Items(1).Enabled = False
			'MessageBox.Show(Me.PlaceHolderMessageBox, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
			LabelRecordNotFound.Visible = True
			'05-11-09 : changes made to bind datagrid after no records found message.
			Me.DataGridVRManager.Visible = False
		End Try
	End Sub
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


				l_integer_AnnuityOnly = 21
			ElseIf (l_string_Type = "ANNUITY") Then

				VoidUserControl1.Action = "ANNUITY"
				VoidUserControl1.Activity = "VReplace"

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

				If Not l_datarow_CurrentRow Is Nothing Then
					'If ((l_datarow_CurrentRow("PayeeAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeAddress").ToString <> "")) Then
					'    Me.TextBoxAddress.Text = CType(l_datarow_CurrentRow.Item("PayeeAddress"), String)
					'Else
					'    Me.TextBoxAddress.Text = "No address found"
					'End If

					'If ((l_datarow_CurrentRow("AcctNumber").ToString <> "System.DBNull") And (l_datarow_CurrentRow("AcctNumber").ToString <> "")) Then
					'    Me.TextBoxAccountNo.Text = CType(l_datarow_CurrentRow.Item("AcctNumber"), String)
					'Else
					'    Me.TextBoxAccountNo.Text = ""
					'End If

					'If ((l_datarow_CurrentRow("LegalEntityType").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityType").ToString <> "")) Then
					'    Me.TextBoxEntityType.Text = CType(l_datarow_CurrentRow.Item("LegalEntityType"), String)
					'Else
					'    Me.TextBoxEntityType.Text = ""
					'End If

					'If ((l_datarow_CurrentRow("BankInfo").ToString <> "System.DBNull") And (l_datarow_CurrentRow("BankInfo").ToString <> "")) Then
					'    Me.TextBoxBankInfo.Text = CType(l_datarow_CurrentRow.Item("BankInfo"), String)
					'Else
					'    Me.TextBoxBankInfo.Text = ""
					'End If

					'If ((l_datarow_CurrentRow("LegalEntityAddress").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityAddress").ToString <> "")) Then
					'    Me.TextBoxEntityAddress.Text = CType(l_datarow_CurrentRow.Item("LegalEntityAddress"), String)
					'Else
					'    Me.TextBoxEntityAddress.Text = ""
					'End If


					'If ((l_datarow_CurrentRow("LegalEntityName").ToString <> "System.DBNull") And (l_datarow_CurrentRow("LegalEntityName").ToString <> "")) Then
					'    Me.TextBoxLegalEntity.Text = CType(l_datarow_CurrentRow.Item("LegalEntityName"), String)
					'Else
					'    Me.TextBoxLegalEntity.Text = ""
					'End If

					'If ((l_datarow_CurrentRow("ActionNotes").ToString <> "System.DBNull") And (l_datarow_CurrentRow("ActionNotes").ToString <> "")) Then
					'    Me.TextBoxNotes.Text = CType(l_datarow_CurrentRow.Item("ActionNotes"), String)
					'Else
					'    Me.TextBoxNotes.Text = ""
					'End If

				End If
			End If
		Catch
			Throw
		End Try
	End Sub
	Private Sub ValidationOnCheckChange()
		Dim l_datarow_CurrentRow As DataRow
		Dim l_integer_CurrentIndex As Integer

		Try
			'Shubhrata Dec 21st Cash Out chNges
			'If Viewstate("DisbSelIndex") <> "Selected" Then
			'    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select a record.", MessageBoxButtons.OK)
			'    Exit Sub
			'End If
			'Shubhrata Dec 21st Cash Out chNges
			l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
			g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
			If Not g_dataset_dsDisbursementsByPersId Is Nothing Then

				'Aparna YREN 2646 15/09/2006
				Dim l_dr_CurrentRow As DataRow()
				Dim l_searchExpr As String
				If Not Session("DisbursementId") Is Nothing Then
					l_searchExpr = "DisbursementID = '" + Session("DisbursementId") + "'"
					l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
					' l_datarow_CurrentRow = l_dr_CurrentRow(0)
					If l_dr_CurrentRow.Length > 0 Then
						l_datarow_CurrentRow = l_dr_CurrentRow(0)
					Else
						MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data couldnot be retrieved.", MessageBoxButtons.OK)
						Exit Sub
					End If
				Else
					l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)

				End If

				'Aparna YREN 2646 15/09/2006
				'Commented  by aparna

				' l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)

				If (l_datarow_CurrentRow("FinalAction").ToString = "False") Then


					'If (chkChangeStatus.Checked) Then


					'    Dim l_dataset_DisbursementStatusTypes As DataSet
					'    'Activity type for the time being is hardcoded has to be passed in session
					'    Dim l_string_ActivityType As String
					'    l_string_ActivityType = Request.QueryString.Get("Activity")
					'    If (l_string_ActivityType = "VoidReplace") Then
					'        l_string_ActivityType = "Replace"
					'    End If

					'    ButtonSave.Enabled = True
					'    Session("_iCounter") = 0


					'Else
					'    'Me.DropDownListReason.Enabled = False
					'    'Me.DropDownListStatus.Enabled = False
					'    'Me.TextBoxNotes.Enabled = False
					'    'Me.DropDownListStatus.BackColor = System.Drawing.Color.LightGray
					'    'Me.DropDownListReason.BackColor = System.Drawing.Color.LightGray
					'    'Me.TextBoxNotes.BackColor = System.Drawing.Color.LightGray
					'     '** ButtonSave.Enabled = False

					'End If
				Else

					MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement Status is final and cannot be changed.", MessageBoxButtons.OK)

				End If
			End If
			SearchAndPopulateData(True)

		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Throw (ex)
		End Try
	End Sub
	Public Sub FinalStatus()
		Try
			'If (Session("g_search_flag") = False) Then
			'    If (g_boolean_flag = False) Then
			'        'chkChangeStatus.Checked = False
			'        'Me.DropDownListStatus.BackColor = System.Drawing.Color.LightGray
			'        'Me.DropDownListReason.BackColor = System.Drawing.Color.LightGray
			'        'Me.TextBoxNotes.BackColor = System.Drawing.Color.LightGray
			'        'Me.DropDownListReason.Enabled = False
			'        'Me.DropDownListStatus.Enabled = False
			'        'Me.TextBoxNotes.Enabled = False
			'        SearchAndPopulateData(True)
			'        g_boolean_flag = False
			'        '** ButtonSave.Enabled = False
			'    Else
			'        'chkChangeStatus.Checked = True
			'        'Me.DropDownListStatus.BackColor = System.Drawing.White
			'        'Me.DropDownListReason.BackColor = System.Drawing.Color.White
			'        'Me.TextBoxNotes.BackColor = System.Drawing.Color.White
			'        'Me.DropDownListReason.Enabled = True
			'        'Me.DropDownListStatus.Enabled = True
			'        'Me.TextBoxNotes.Enabled = True
			'        SearchAndPopulateData(True)
			'        g_boolean_flag = False
			'        ButtonSave.Enabled = True
			'    End If
			'End If
			If (Session("g_search_flag") = True) Then
				Session("g_search_flag") = True
				Me.LabelRecordNotFound.Visible = False
				Me.DataGridVRManager.Visible = False

			End If



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
        'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
        Dim dblDisbTaxableAmt As Double = 0.0
        Dim dblDisbNonTaxableAmt As Double = 0.0
        Dim dblProRateFactor As Double = 0.0
        'End:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.

		Try
			'l_string_NewStatus = Me.DropDownListStatus.SelectedValue.Trim()
			l_string_NewStatus = "REISSUE"
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

				'Aparna YREN 2646 15/09/2006
				Dim l_dr_CurrentRow As DataRow()
				Dim l_searchExpr As String

				'If Not Session("DisbursementId") Is Nothing Then
				If parameterDisbId <> "" Then

					l_searchExpr = "DisbursementID = '" + parameterDisbId + "'"
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

				'If ((l_datarow_CurrentRow("PersId").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PersId").ToString <> "")) Then
				'    litPersID.Text = CType(l_datarow_CurrentRow.Item("PersId"), String)
				'End If


				If ((l_datarow_CurrentRow("PayeeEntityID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("PayeeEntityID").ToString <> "")) Then
					litPayeeId.Text = l_datarow_CurrentRow.Item("PayeeEntityID").ToString
				End If

				'If ((l_datarow_CurrentRow("AddressID").ToString <> "System.DBNull") And (l_datarow_CurrentRow("AddressID").ToString <> "")) Then
				'    litAddressID.Text = l_datarow_CurrentRow.Item("AddressID").ToString
				'End If

				'If ((l_datarow_CurrentRow("Number").ToString <> "System.DBNull") And (l_datarow_CurrentRow("Number").ToString <> "")) Then
				'    litDisbNbr.Text = l_datarow_CurrentRow.Item("Number").ToString
                'End If

                'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
                If Not (String.IsNullOrEmpty(l_datarow_CurrentRow("DisbTaxableAmount").ToString)) Then
                    dblDisbTaxableAmt = l_datarow_CurrentRow.Item("DisbTaxableAmount").ToString
                End If

                If Not (String.IsNullOrEmpty(l_datarow_CurrentRow("DisbNonTaxableAmount").ToString)) Then
                    dblDisbNonTaxableAmt = l_datarow_CurrentRow.Item("DisbNonTaxableAmount").ToString
                End If
                'End:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.

				If l_string_NewStatus = "REPLACE" Or l_string_NewStatus = "REISSUE" Then
					g_dataset_dsWithholdingInfo = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetWithholdingInfo(l_string_DisbId)

					If g_dataset_dsWithholdingInfo.Tables("WithholdingInfo").Rows.Count > 0 Then

						l_datarow_WithholdingInfo = g_dataset_dsWithholdingInfo.Tables("WithholdingInfo").Rows(0)

						If (l_string_DisbType.Trim() = "REF" OrElse l_string_DisbType.Trim() = "ADT" OrElse l_string_DisbType.Trim() = "RDT") _
								And l_string_NewStatus = "REPLACE" And CType(l_datarow_WithholdingInfo("DetailRows"), Integer) = 0 Then
							l_boolean_GoForBreakdown = True
						End If

						'Comment on 1/10/2009 do work for AtsRefunds record
						'If l_string_NewStatus = "REISSUE" And (CType(l_datarow_WithholdingInfo("DetailRows"), Integer) = 0 Or CType(l_datarow_WithholdingInfo("RefundRefRows"), Integer) = 0) Then
						If l_string_NewStatus = "REISSUE" And (CType(l_datarow_WithholdingInfo("DetailRows"), Integer) = 0) Then
							l_boolean_GoForBreakdown = True
						End If

						If l_boolean_GoForBreakdown Then

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
                            'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
                            If l_double_Gross > 0 Then
                                dblProRateFactor = l_double_WHAmount / l_double_Gross
                            End If
                            CreateDisbursementDetails(l_string_DisbId, dblDisbTaxableAmt, dblDisbNonTaxableAmt, dblProRateFactor)
                            SaveListVoidReissue()
                            'End:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.   
						End If
					End If
				End If

				If l_string_NewStatus = "REISSUE" And l_boolean_GoForBreakdown = False Then
					Dim l_string_Output As String
					SaveListVoidReissue()
				End If

			End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & Reissue Withdrawal --> ChangeStatus", ex)
            Throw
		End Try

	End Sub
	Public Sub PopulateDisbursementTextBoxes(ByVal DisbursementID As String)
		Dim l_integer_CurrentIndex As Integer
		Dim l_datarow_CurrentRow As DataRow

		Try

			l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
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
				Else
					Throw New Exception("DisbusrementId not found.")
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
	Public Sub ClearDisbursementTextBoxes()

		Try


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
	Private Sub SaveVoidReissue(ByVal parameterDisbId As String)
		Dim l_datarow_CurrentRow As DataRow
		Dim l_integer_CurrentIndex As Integer
		Dim l_datetime_CompareDate As DateTime

		Try
			ValidationOnCheckChange()
			Session("ReplaceFees") = 0

			l_integer_CurrentIndex = Session("DisbursementByPersIdIndex")
			g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")


			If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
				''**If Not Session("DisbursementId") Is Nothing Then
				Dim l_dr_CurrentRow As DataRow()
				Dim l_searchExpr As String
				l_searchExpr = "DisbursementID = '" + parameterDisbId + "'"
				l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
				'**l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
				If l_dr_CurrentRow.Length > 0 Then
					l_datarow_CurrentRow = l_dr_CurrentRow(0)
				Else
					MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Please select the item as data couldnot be retrieved.", MessageBoxButtons.OK)
					Exit Sub
				End If
				'Else
				'**l_datarow_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows(l_integer_CurrentIndex)
			End If

			If CType(l_datarow_CurrentRow("Voided"), String).ToUpper = "YES" And (CType(l_datarow_CurrentRow("Reversed"), String)).ToUpper = "YES" Then
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement(s) are already voided and reversed. Cannot void.", MessageBoxButtons.Stop)
				Exit Sub
			End If



			If Session("flag") Is Nothing Then
				l_datetime_CompareDate = DateAdd(DateInterval.Year, 1, Convert.ToDateTime(l_datarow_CurrentRow("Issueddate")))
				If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then
					MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Issue date for disbursement #" & l_datarow_CurrentRow("Number") & " is older than a year. Do you wish to continue?", MessageBoxButtons.YesNo)
					Session("flag") = "Pending"
					Exit Sub
				Else
					Session("_iCounter") = Session("_iCounter") + 1
					'Comment by imran
					ChangeStatus(parameterDisbId)
					Dim strOutPut As String
					strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoVoidWithdrawalReissue(l_datarow_CurrentRow("DisbursementID"))
					If (strOutPut <> "") Then
						MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", strOutPut, MessageBoxButtons.YesNo)
						Exit Sub
					End If
				End If
			Else
				If Session("flag") = "YesContinue" Then

					Session("_iCounter") = Session("_iCounter") + 1
					'Comment by imran
					'** ChangeStatus()
					Dim strOutPut As String
					strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoVoidWithdrawalReissue(l_datarow_CurrentRow("DisbursementID"))
					If (strOutPut <> "") Then
						MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", strOutPut, MessageBoxButtons.YesNo)
						Exit Sub
					Else
						SendReissueMail()
					End If
				End If



			End If


			SearchAndPopulateData(True)
			'** End If
			'** ButtonSave.Enabled = False

		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Throw (ex)
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
		Session("g_search_flag") = Nothing
		Session("R_MRDAcctBalance") = Nothing
		Session("MRDRate") = Nothing
		Session("MRDRequest") = Nothing
	End Sub
	Private Sub SaveVoid()
		Dim datagrid As DataGrid
		Dim dglst As DataList
		dglst = CType(VoidUserControl1.FindControl("Datalist1"), DataList)
		For Each item As DataListItem In dglst.Items
			Dim img As ImageButton
			img = item.Controls(0).FindControl("Imagebutton1")
			If img.ImageUrl = "../images/selected.gif" Then
				datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)
				If Not IsNothing(datagrid) Then
					Dim i As Integer

					'i = Session("DisbursementByPersIdIndex")
					For i = Session("DisbursementByPersIdIndex") To datagrid.Items.Count - 1
						Dim chk As CheckBox
						Dim LabelDisbId As Label
						chk = datagrid.Items(i).FindControl("chkRefund")
						LabelDisbId = datagrid.Items(i).FindControl("LabelDisbId")
						If Not IsNothing(chk) Then
							If chk.Checked Then
								Session("DisbursementByPersIdIndex") = i
								'PopulateDisbursementTextBoxes(datagrid.Items(i).Cells(1).Text.Trim())
								hdnDisbId.Value = LabelDisbId.Text.Trim()
								SaveVoidReissue(hdnDisbId.Value)
							End If
						End If
					Next
				End If

			End If



		Next

	End Sub
	Private Sub ValidateVoidReissue()
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

                        If Strarr.GetValue(i) <> "" Then
                            hdnDisbId.Value = Strarr.GetValue(i)
                            parameterDisbId = hdnDisbId.Value
                            g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                        End If



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
                                    MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement is already voided and reversed.", MessageBoxButtons.Stop)
                                    Exit Sub
                                End If


                                '2.validate date of issue older 

                                l_datetime_CompareDate = DateAdd(DateInterval.Year, 1, Convert.ToDateTime(l_datarow_CurrentRow("Issueddate")))
                                If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then

                                    l_string_RelDisbs = l_string_RelDisbs + "," + l_datarow_CurrentRow("Number")
                                End If


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


            StrListDisbId = hdnListDisbId.Value
            If StrListDisbId.Length > 0 Then
                Strarr = StrListDisbId.Split(",")
                Dim i As Integer
                If Strarr.Length >= 2 Then
                    'For i = 1 To Strarr.Length - 1
                    If Strarr.GetValue(1) <> "" Then
                        ChangeStatus(Strarr.GetValue(1))
                    End If
                    'Next
                End If
            End If


            If StrListDisbId.IndexOf(",") = 0 And hdnListDisbId.Value.Length > 10 Then
                Strarr = hdnListDisbId.Value.Split(",")
                ' SaveListVoidReissue(Strarr)
            End If



        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Throw (ex)
        End Try
	End Sub
	Private Sub SaveListVoidReissue()

		Try
			Dim parameterListDisbId As Array

			If hdnListDisbId.Value.Length > 0 Then
				parameterListDisbId = hdnListDisbId.Value.Split(",")
			End If

			Dim strOutPut As String
			Dim i As Integer
			'If parameterListDisbId.Length >= 2 Then
			'    For i = 1 To parameterListDisbId.Length - 1
			'        ' strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoVoidWithdrawalReissue(parameterListDisbId.GetValue(i))
			'    Next

			'    If (strOutPut <> "") Then
			'        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", strOutPut, MessageBoxButtons.YesNo)
			'        Exit Sub
			'    Else
			'        MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Record save successfully.", MessageBoxButtons.OK)
			'        Exit Sub
			'    End If

			'End If


			SaveVoidReissue()

			'SearchAndPopulateData(True)
			'  '** ButtonSave.Enabled = False

		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Throw (ex)
		End Try
	End Sub
	'Added on 1/10/2009
	Private Sub savevoidReissue()
		Dim l_dataset_disbDetail As New DataSet
		Dim strOutPut As String
		Dim parameterListDisbId As Array
		Try

			If hdnListDisbId.Value.Length > 0 Then
				parameterListDisbId = hdnListDisbId.Value.Split(",")
			End If
			If Not Session("DisbursementDetails") Is Nothing Then
				l_dataset_disbDetail = Session("DisbursementDetails")
				Session("DisbursementDetails") = Nothing
			End If
			ButtonSave.Enabled = True
			strOutPut = ""
			strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoVoidWithdrawalRissue(parameterListDisbId, l_dataset_disbDetail)

			If (strOutPut <> "") Then
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", strOutPut, MessageBoxButtons.Stop)
				Exit Sub
			Else
				'MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Record saved successfully.", MessageBoxButtons.OK)
				MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement has been void-reissued successfully.", MessageBoxButtons.OK)
				Session("g_search_flag") = False

				hdnDisbId.Value = ""
				hdnListDisbId.Value = ""

				'ButtonSave.Enabled = False
				'Dim l_datetime_CompareDate As DateTime
				''commented by priya on 12/15/2008 for YRS 5.0-626
				''l_datetime_CompareDate = DateAdd(DateInterval.Month, 1, Convert.ToDateTime(Session("dsDisbursementsByPersId").Tables(0).Rows(Session("DisbursementIndex"))("AccountDate")))
				'''added by priya on 12/15/2008 for YRS 5.0-626
				'l_datetime_CompareDate = DateAdd(DateInterval.Month, 1, Convert.ToDateTime(Session("dsDisbursementsByPersId").Tables(0).Rows(Session("DisbursementByPersIdIndex"))("AccountDate")))
				'If (System.DateTime.Compare(System.DateTime.Today(), l_datetime_CompareDate) = 1) Then
				'    SendMail()
				'End If
				Exit Sub
			End If
		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
			Throw (ex)
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
						If Not IsNothing(datagrid) Then
							Dim i As Integer

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
													strHTML = New StringBuilder
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
													strHTML.Append("<tr><td class=labelbold>Disbursement Number: </td><td>" & dsDisbursements.Tables(0).Rows(0)("Number").ToString().Trim() & "</td><td class=labelbold> Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold> Issued Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</sapn></td></tr>")
												Else
													strHTML.Append("<tr><td class=labelbold> Disbursement- </td><td></td><td class=labelbold>Payment Type: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("PayMethod").ToString() & "</span></td> <td class=labelbold>Disbursement Date: <span class=label>" & dsDisbursements.Tables(0).Rows(0)("IssuedDate").ToString() & "</span></td></tr>")
												End If

												If dsDisbursements.Tables(1).Rows.Count > 0 Then
													Dim counter As Integer
													strHTML.Append("<tr><td colspan=4>")
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
								strHTML.Append("<tr><td class=labelbold>Gross Fees: </td><td colspan=3>" & l_Gross.ToString() & "</td></tr>")
							End If

							strHTML.Append("</table></html>")
						End If

					End If
					cnt += 1
				Next

			End If
			'=================================================






			If obj.MailService = False Then
				Exit Sub
			End If
			obj.SendCc = ""
			obj.MailMessage = strHTML.ToString() '"A withdrawal for fund Id " & l_string_FundIdNo & " which was processed in a prior month has been reversed.  Please review and make necessary interest adjustments as soon as possible."
			obj.Subject = "Withdrawal from prior month has been Voided."
			obj.MailCategory = "VOID"
			obj.MailFormatType = Mail.MailFormat.Html
			obj.Send()
		Catch
			Throw
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
            Dim l_string_Activity As String

            If Not Request.QueryString.Get("Activity") Is Nothing Then
                l_string_Activity = Request.QueryString.Get("Activity")
                If l_string_Activity = "Void" Then
                    Headercontrol.PageTitle = "Void Disbursement"
                ElseIf l_string_Activity = "Reissue" Then
                    Headercontrol.PageTitle = "Void and Reissue Disbursement / Withdrawals"
                Else
                    Headercontrol.PageTitle = "Void Disbursement"
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

                    'Session("Person_Info") = Me.TextBoxSSNo.Text
                    Dim strSSN As String = SSNO.Insert(3, "-")
                    strSSN = strSSN.Insert(6, "-")

                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    'Start: Shashi Shekhar: 2010-12-09: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
                    'LabelHdr.Text = LabelHdr.Text + "--" + LName + ", " + FName + " " + MName + ",SS#: " + strSSN

                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                        Headercontrol.FundNo = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim
                    End If
                    'End of YRS 5.0-450, BT-643
                    '--------------------------------------------------------------------------------------------------------------------------------------------------------
                End If
            End If
        Catch ex As Exception
            Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

	End Sub
#End Region

#Region "FUNCTION"
	Private Function SendReissueMail() As String
		Dim obj As New MailUtil
		Dim l_string_MailMessage As String
		Dim VoidedDate As String
		Dim l_PaymentMethod(PaymentMethod.Split(",").Length), l_FundNo(FundNo.Split(",").Length), l_CheckNumber(CheckNumber.Split(",").Length), l_IssuedDate(IssuedDate.Split(",").Length), l_DisbursementType(DisbursementType.Split(",").Length) As String

		obj.MailCategory = "VOID"

		If obj.MailService = False Then Exit Function
		obj.SendCc = ""

		VoidedDate = System.DateTime.Today.ToShortDateString()

		l_PaymentMethod = PaymentMethod.Split(",")
		l_FundNo = FundNo.Split(",")
		l_CheckNumber = CheckNumber.Split(",")
		l_IssuedDate = IssuedDate.Split(",")
		l_DisbursementType = DisbursementType.Split(",")


		Dim strTable As New System.Text.StringBuilder
		strTable.Append("<Html>")
		'-------------CSS------------------------
		strTable.Append("<style type=text/css>")
		strTable.Append(".label {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: normal;color: #000;}")
		strTable.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")
		strTable.Append(".labelbold {font-family: Arial, Helvetica, sans-serif;font-size: 12px;font-weight: bold;color: #000;}")
		strTable.Append(".smalllabel {font-family: Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #000;}")

		strTable.Append("</style>")
		'-------------------------------------

		strTable.Append("<table class =label><tr><td class=labelbold>Fund Id:</td><td colspan=3 >" & l_FundNo(0).Trim & "</td></tr>")
		strTable.Append("<tr><td class=labelbold>Disbursement Type:</td><td colspan=3>" & l_DisbursementType(0).Trim & "</td></tr>")
		strTable.Append("<tr><td class=labelbold>Voided Date:</td><td colspan=3>" & VoidedDate.Trim() & "</td></tr>")
		strTable.Append("<tr><td class=labelbold colspan=4>")
		strTable.Append("<table class =label border='1' bordercolor='Black' cellSpacing='0' cellPadding='0' align='Center' width=300px>")
		strTable.Append("<tr><td class=labelbold align='Center'>Number</td><td class=labelbold align='Center'>Payment Type</td><td class=labelbold align='Center'>Date</td></tr>")


		Dim i As Integer
		For i = 0 To l_DisbursementType.Length - 1
			If (Not IsNothing(l_CheckNumber(i))) Then
				strTable.Append("<td align='Center'>" & l_CheckNumber(i).Trim & "</td>")
			End If
			If (Not IsNothing(l_PaymentMethod(i))) Then
				strTable.Append("<td align='Center'>" & l_PaymentMethod(i).Trim & "</td>")
			End If

			If (Not IsNothing(l_IssuedDate(i))) Then
				strTable.Append("<td align='Center'>" & l_IssuedDate(i).Trim & "</td>")
			End If

		Next
		strTable.Append("</table></td></tr></table></html>")
		l_string_MailMessage = strTable.ToString()
		'l_string_MailMessage = l_string_MailMessage & ControlChars.CrLf & ControlChars.CrLf & "Above fundid number(s), which was processed in prior month has been void re-issued.  Please review and make necessary  changes to 1099 tax information as soon as possible."


		'        l_string_MailMessage = "Fund Id Number: " & FundNo & ", disbursement type: " & DisbursementType & ", payment type: " & PaymentMethod & ", check number: " & CheckNumber & ",Issued Date: " & IsuedDate & ", voided date: " & VoidedDate & ControlChars.CrLf & " which was processed in prior month has been void re-issued.  Please review and make necessary  changes to 1099 tax information as soon as possible."
		obj.MailFormatType = Mail.MailFormat.Html

		obj.MailMessage = l_string_MailMessage
		obj.Subject = "Withdrawal from prior month has been void reissued."

		obj.Send()


	End Function
#End Region

	'Priya 18-Feb-2011 If request is MRD then Block void reissue
	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		If Session("MRDRequest") = "YES" Then
			Session("MRDRequest") = Nothing
			MessageBox.Show(PlaceHolderMessageBox, "YMCA-YRS", "Disbursement is of type MRD. Cannot Void and Reissue.", MessageBoxButtons.OK)
			ButtonSave.Enabled = False
			Exit Sub
		End If
    End Sub

    'Start:SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.
    Public Sub CreateDisbursementDetails(ByVal DisbursementId As String, ByVal dblDisbTaxableAmt As Double, ByVal dblDisbNonTaxableAmt As Double, ByVal dblProRateFactor As Double)
        Dim dsDisbDetail As New DataSet
        Dim temRow As DataRow
        Try
            dsDisbDetail.Tables.Add("DisbursementDetails")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Clear()
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("DisbursementID")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("DisbursementNumber")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("chrAcctType")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("chrAcctBreakDownType")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("TaxablePrincipal")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("TaxableInterest")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("NonTaxablePrincipal")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("WithheldPrincipal")
            dsDisbDetail.Tables("DisbursementDetails").Columns.Add("WithheldInterest")
            temRow = dsDisbDetail.Tables("DisbursementDetails").NewRow

            temRow("DisbursementID") = DisbursementId
            temRow("DisbursementNumber") = litDisbNbr.Text.Trim()
            temRow("chrAcctType") = "BA"
            temRow("chrAcctBreakDownType") = "18"
            temRow("TaxablePrincipal") = dblDisbTaxableAmt
            temRow("TaxableInterest") = 0
            temRow("NonTaxablePrincipal") = dblDisbNonTaxableAmt
            temRow("WithheldPrincipal") = Math.Round(dblDisbTaxableAmt * dblProRateFactor, 2)
            temRow("WithheldInterest") = Math.Round(dblDisbNonTaxableAmt * dblProRateFactor, 2)
            dsDisbDetail.Tables("DisbursementDetails").Rows.Add(temRow)
            Session("DisbursementDetails") = dsDisbDetail
        Catch ex As Exception
            Session("DisbursementDetails") = Nothing
            HelperFunctions.LogException("Void & Reissue Withdrawal --> CreateDisbursementDetails", ex)
            Throw ex
        End Try
    End Sub
    'End : SR:2014.08.26: BT 2588/YRS 5.0-2371 - Payment Manager does not display void replaced disbursements for old disbursements.

End Class
