'********************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	CashReceiptsEntryForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:13:39 PM
' Program Specification Name	:	YMCA PS 3.10.1.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Changed by			:	Vartika Jain
' Changed on			:	09/29/2005
' Change Description	:	Coding.
'********************************************************************************
'Cache-Session     :   Vipul 03Feb06 
'****************************************************************************************
'Modification History
'********************************************************************************************
'Modified By                    Date                        Desription
'********************************************************************************************
'Aparna Samala                  03/09/2007                  Modified the enabling and disabling of the buttons
'Swopna                         29-Jan-2008                 In order to avoid entering &nbsp; into table(Reference-Hafiz's email,Subject-YMCA QA Testing status of 4.0.6 patch,Date-26 Jan,2008)
'Neeraj Singh                   12/Nov/2009                 Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16                  YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N                       03/04/2019                  YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'********************************************************************************************
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
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class CashReceiptsEntryForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("CashReceiptsEntryForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridCashReceipt As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelYMCANo As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListSource As System.Web.UI.WebControls.DropDownList
    ''Protected WithEvents TextBoxRecDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxRecDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxComments As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSource As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRefInfo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRefInfo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCheckDate As System.Web.UI.WebControls.Label
    ''Protected WithEvents TextBoxCheckDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCheckDate As YMCAUI.DateUserControl
    Protected WithEvents LabelRecDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAmount As System.Web.UI.WebControls.Label
    Protected WithEvents LabelComments As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents PopcalendarRecDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPost As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents TextboxTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolderCashEntry As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ReqAmt As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqYMCANo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqCheckDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqRecDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_dt_tempTable As New DataTable
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            CheckReadOnlyMode() ' Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            Me.LabelAmount.AssociatedControlID = Me.TextBoxAmount.ID
            ''Me.LabelCheckDate.AssociatedControlID = Me.TextBoxCheckDate.ID
            ''Me.LabelRecDate.AssociatedControlID = Me.TextBoxRecDate.ID
            Me.LabelSource.AssociatedControlID = Me.DropDownListSource.ID
            Me.LabelYMCANo.AssociatedControlID = Me.TextBoxYMCANo.ID
            Me.LabelRefInfo.AssociatedControlID = Me.TextBoxRefInfo.ID
            Me.LabelComments.AssociatedControlID = Me.TextBoxComments.ID
            'Me.LabelGeneral.AssociatedControlID = Me.TextBoxGeneral.ID
            Me.TextBoxCheckDate.RequiredDate = True
            Me.TextBoxAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            Me.TextBoxYMCANo.Attributes.Add("onblur", "javascript:FormatYMCANo();")
            ''Rahul()

            Me.TextBoxYMCANo.Attributes.Add("onkeypress", "Javascript:DropDownListSourceFocus();")
            'Me.DropDownListSource.Attributes.Add("onchange", "Javascript:TextBoxRefInfoFocus();")
            Me.TextBoxRefInfo.Attributes.Add("onkeypress", "Javascript:TextBoxRefInfoFocus();")
            'Me.TextBoxCheckDate.Attributes.Add("onkeypress", "Javascript:DropDownListSourceFocus(3);")
            'Me.TextBoxRecDate.Attributes.Add("onkeypress", "Javascript:TextBoxAmountFocus(4);")
            'onkeypress changed to onkeyup by Anita on 04-jun-2007, HandleAmountFiltering was not getting executed as both functions were called on same event
            Me.TextBoxAmount.Attributes.Add("onkeyup", "Javascript:TextBoxCommentsFocus();")
            'onkeypress changed to onkeyup by Anita on 04-jun-2007, HandleAmountFiltering was not getting executed as both functions were called on same event

            'Me.TextBoxComments.Attributes.Add("onkeydown", "Javascript:TextBoxComments_onkeydown();")

            Dim strJS As String
            strJS = "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {document.forms[0].elements['ButtonAdd'].click();return false;} else return true; "
            Me.TextBoxComments.Attributes.Add("onkeydown", strJS)

            ''Rahul()
            Me.TextBoxCheckDate.RequiredDate = True
            Me.TextBoxRecDate.RequiredDate = True

           


            If Not Me.IsPostBack Then
                PopulatePaymentTypes()
                Me.TextBoxRecDate.Text = Convert.ToDateTime(Date.Today).ToString("MM/dd/yyyy")

                'added by Hafiz on 6-Mar-2009
                ButtonSave.Enabled = False
                ButtonCancel.Enabled = False
            End If
            If Request.Form("OK") = "OK" Then
                If Session("Control_Focus") = "YMCA_No" Then
                    SetFocus(TextBoxYMCANo)
                ElseIf Session("Control_Focus") = "CheckDate" Then
                    SetFocus(TextBoxCheckDate)
                ElseIf Session("Control_Focus") = "RecDate" Then
                    SetFocus(TextBoxRecDate)
                ElseIf Session("Control_Focus") = "Amount" Then
                    SetFocus(TextBoxAmount)
                End If
                Session("Control_Focus") = Nothing
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim j As Integer
            Dim l_dataset_dsLookupYmca As DataSet
            Dim l_string_YmcaGuId As String
            Dim l_string_YmcaMetroId As String
            Dim l_string_hubInd As String
            Dim l_date_checkdate As Date
            Dim l_Date_MinDate As Date
            Dim l_date_RecDate As Date
            Dim l_Boolean_Flag As Boolean = False
            Dim l_Boolean_Caution_Flag As Boolean = False
            Dim l_temp_amount As Decimal = 0
            Dim drow As DataRow


            l_dataset_dsLookupYmca = YMCARET.YmcaBusinessObject.CashRecieptsEntry.LookUpYmca(Me.TextBoxYMCANo.Text.Trim())
            Session("DataSetYmca") = l_dataset_dsLookupYmca
            'System.AppDomain.CurrentDomain.SetData("DataSetYmca", l_dataset_dsLookupYmca)
            If l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows.Count = 0 Then
                'message ---ymca does not exist
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Ymca " + Me.TextBoxYMCANo.Text.Trim() + " not found.", MessageBoxButtons.OK)
                Session("Control_Focus") = "YMCA_No"

                l_Boolean_Flag = True
            Else
                l_string_hubInd = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chrHubInd").ToString().Trim()
                l_string_YmcaGuId = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                l_string_YmcaMetroId = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiYMCAMetroId").ToString().Trim()

                If l_string_hubInd = "B" Then
                    If l_string_YmcaGuId = l_string_YmcaMetroId Then
                        'message .. 'branch of ymca
                        'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                        MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Ymca " + Me.TextBoxYMCANo.Text.Trim() + " is a branch of " + Me.TextBoxYMCANo.Text.Trim(), MessageBoxButtons.OK)
                        Session("Control_Focus") = "YMCA_No"

                        l_Boolean_Flag = True
                    End If
                End If
            End If

            ' the Ymca No is now correct

            l_date_checkdate = Me.TextBoxCheckDate.Text
            l_date_RecDate = Me.TextBoxRecDate.Text
            ''viewstate("Recieved_Date") = Me.TextBoxRecDate.Text
            l_Date_MinDate = Convert.ToDateTime("07/01/1922")
            If l_Boolean_Flag = False And (l_date_checkdate > Date.Today.ToShortDateString Or l_date_checkdate < l_Date_MinDate) Then
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Check Date entered is outside of an acceptable range.", MessageBoxButtons.OK)
                Session("Control_Focus") = "CheckDate"

                l_Boolean_Flag = True
            End If

            If l_Boolean_Flag = False And (l_date_RecDate > Date.Today.ToShortDateString Or l_date_RecDate < l_Date_MinDate) Then
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Recieved Date entered is outside of an acceptable range.", MessageBoxButtons.OK)
                Session("Control_Focus") = "RecDate"

                l_Boolean_Flag = True
            End If

            If l_Boolean_Flag = False And Convert.ToDecimal(Me.TextBoxAmount.Text) = 0 Then
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Please Enter Positive Amount.", MessageBoxButtons.OK)
                Session("Control_Focus") = "Amount"

                l_Boolean_Flag = True
            End If
            If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_checkdate > l_date_RecDate) Then
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Payment Date Can Not Be Greater Than Recieved Date", MessageBoxButtons.OK)
                l_Boolean_Flag = True
            End If
            ' caution if the date is older than 6 months

            If l_Boolean_Flag = False And (l_date_checkdate.AddMonths(6) < Date.Today.ToShortDateString) Then
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Caution! The Date entered is at least six months old.", MessageBoxButtons.OK)
                Session("Control_Focus") = "CheckDate"

                l_Boolean_Caution_Flag = True
            End If

            If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_RecDate.AddMonths(6) < Date.Today.ToShortDateString) Then
                'TOP=250 and LEFT=200 removed by anita on 01-Jun-2007
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Caution! The Date entered is at least six months old.", MessageBoxButtons.OK)
                Session("Control_Focus") = "RecDate"

                l_Boolean_Caution_Flag = True
            End If
            If l_Boolean_Flag = False Then

                If g_dt_tempTable.Columns.Count = 0 Then
                    CreateTempTable()
                End If
                If Not Session("session_tempTable") Is Nothing Then
                    g_dt_tempTable = Session("session_tempTable")
                End If


                If Session("State") = False Then
                    If Me.DataGridCashReceipt.SelectedIndex = -1 Then

                        drow = g_dt_tempTable.NewRow()

                        drow("YmcaNo.") = Me.TextBoxYMCANo.Text
                        drow("Name") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chvYmcaName").ToString().Trim()
                        drow("Source") = Me.DropDownListSource.SelectedItem.Text.Trim()
                        drow("RefInfo") = Me.TextBoxRefInfo.Text
                        drow("CheckDate") = Me.TextBoxCheckDate.Text
                        drow("Rec.Date") = Me.TextBoxRecDate.Text
                        drow("Amount") = Me.TextBoxAmount.Text
                        drow("Comments") = Me.TextBoxComments.Text
                        drow("Code") = Me.DropDownListSource.SelectedValue
                        drow("YmcaId") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                        Dim i As Integer = g_dt_tempTable.Rows.Count
                        g_dt_tempTable.Rows.InsertAt(drow, i)
                    Else
                        drow = g_dt_tempTable.Rows(Me.DataGridCashReceipt.SelectedIndex)

                        drow("YmcaNo.") = Me.TextBoxYMCANo.Text
                        drow("Name") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chvYmcaName").ToString().Trim()
                        drow("Source") = Me.DropDownListSource.SelectedItem.Text.Trim()
                        drow("RefInfo") = Me.TextBoxRefInfo.Text
                        drow("CheckDate") = Me.TextBoxCheckDate.Text
                        drow("Rec.Date") = Me.TextBoxRecDate.Text
                        drow("Amount") = Me.TextBoxAmount.Text
                        drow("Comments") = Me.TextBoxComments.Text
                        drow("Code") = Me.DropDownListSource.SelectedValue
                        drow("YmcaId") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                    End If

                End If

                Session("session_tempTable") = g_dt_tempTable
                Me.DataGridCashReceipt.DataSource = g_dt_tempTable
                Me.DataGridCashReceipt.DataBind()


                'Me.DataGridCashReceipt.SelectedIndex = 0
                'Me.DropDownListSource.Enabled = False
                'Me.TextBoxYMCANo.Enabled = False
                'Me.TextBoxCheckDate.Enabled = False
                'Me.TextBoxAmount.Enabled = False
                'Me.TextBoxRecDate.Enabled = False
                'Me.TextBoxComments.Enabled = False
                'Me.TextBoxRefInfo.Enabled = False
                'Me.ButtonSave.Enabled = False
                'Me.ButtonCancel.Enabled = False
                'Me.ButtonPost.Enabled = True
                'Me.ButtonAdd.Enabled = True
                'Me.ButtonDelete.Enabled = False

                Me.TextBoxYMCANo.Enabled = True
                Me.TextBoxCheckDate.Enabled = True
                Me.TextBoxAmount.Enabled = True
                Me.TextBoxRecDate.Enabled = True
                Me.TextBoxComments.Enabled = True
                Me.TextBoxRefInfo.Enabled = True
                Me.DropDownListSource.Enabled = True

                Me.TextBoxYMCANo.Text = ""
                Me.TextBoxCheckDate.Text = ""
                Me.TextBoxAmount.Text = ""
                ''Me.TextBoxRecDate.Text = ""
                Me.TextBoxComments.Text = ""
                Me.TextBoxRefInfo.Text = ""
                Me.DropDownListSource.SelectedIndex = 0

                'commented by Hafiz on 6-Mar-2009
                'Me.ButtonCancel.Enabled = True

                'added by Hafiz on 6-Mar-2009
                Me.ButtonCancel.Enabled = False
                Me.ButtonSave.Enabled = False

                Me.ButtonPost.Enabled = True 'Rahul
                Me.ButtonDelete.Enabled = False

                Me.DataGridCashReceipt.SelectedIndex = -1

                'added by Hafiz on 6-Mar-2009
                Me.ButtonAdd.Enabled = True

                For j = 0 To Me.DataGridCashReceipt.Items.Count - 1
                    l_temp_amount = l_temp_amount + Convert.ToDecimal(Me.DataGridCashReceipt.Items(j).Cells(7).Text.Trim())
                Next
                Me.TextboxTotal.Text = FormatCurrency(l_temp_amount)

                Session("State") = True
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Public Sub CreateTempTable()
        Try
            Dim dcol_YmcaNo As New DataColumn("YmcaNo.")
            Dim dcol_ymcaName As New DataColumn("Name")
            Dim dcol_Source As New DataColumn("Source")

            Dim dcol_Reference As New DataColumn("RefInfo")
            Dim dcol_Check As New DataColumn("CheckDate")
            Dim dcol_Recieved As New DataColumn("Rec.Date")
            Dim dcol_Amount As New DataColumn("Amount")
            Dim dcol_Comments As New DataColumn("Comments")
            Dim dcol_Code As New DataColumn("Code")
            Dim dcol_YMCAId As New DataColumn("YmcaId")

            g_dt_tempTable.Columns.Add(dcol_YmcaNo)

            g_dt_tempTable.Columns.Add(dcol_ymcaName)
            g_dt_tempTable.Columns.Add(dcol_Source)
            g_dt_tempTable.Columns.Add(dcol_Reference)
            g_dt_tempTable.Columns.Add(dcol_Check)
            g_dt_tempTable.Columns.Add(dcol_Recieved)
            g_dt_tempTable.Columns.Add(dcol_Amount)
            g_dt_tempTable.Columns.Add(dcol_Comments)
            g_dt_tempTable.Columns.Add(dcol_Code)
            g_dt_tempTable.Columns.Add(dcol_YMCAId)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Public Sub PopulatePaymentTypes()
        Try
            Dim l_dataset_PaymentTypes As DataSet
            l_dataset_PaymentTypes = YMCARET.YmcaBusinessObject.CashRecieptsEntry.LookUpPaymentTypes()
            Me.DropDownListSource.DataSource = l_dataset_PaymentTypes.Tables("PaymentTypes")
            Me.DropDownListSource.DataTextField = l_dataset_PaymentTypes.Tables("PaymentTypes").Columns("Description").ToString().Trim()
            Me.DropDownListSource.DataValueField = l_dataset_PaymentTypes.Tables("PaymentTypes").Columns("CodeValue").ToString().Trim()
            Me.DropDownListSource.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Dim j As Integer
        Dim l_dataset_dsLookupYmca As DataSet
        Dim l_string_YmcaGuId As String
        Dim l_string_YmcaMetroId As String
        Dim l_string_hubInd As String
        Dim l_date_checkdate As Date
        Dim l_Date_MinDate As Date
        Dim l_date_RecDate As Date
        Dim l_Boolean_Flag As Boolean = False
        Dim l_Boolean_Caution_Flag As Boolean = False
        Dim l_temp_amount As Decimal = 0
        Dim drow As DataRow
        Try
            If Me.TextBoxAmount.Text <> "" And Me.TextBoxCheckDate.Text <> "" And Me.TextBoxRecDate.Text <> "" And Me.TextBoxYMCANo.Text <> "" Then


                l_dataset_dsLookupYmca = YMCARET.YmcaBusinessObject.CashRecieptsEntry.LookUpYmca(Me.TextBoxYMCANo.Text.Trim())
                Session("DataSetYmca") = l_dataset_dsLookupYmca
                ' System.AppDomain.CurrentDomain.SetData("DataSetYmca", l_dataset_dsLookupYmca)
                If l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows.Count = 0 Then
                    'message ---ymca does not exist
                    'anita-IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Ymca " + Me.TextBoxYMCANo.Text.Trim() + " not found.", MessageBoxButtons.OK)
                    Session("Control_Focus") = "YMCA_No"

                    l_Boolean_Flag = True
                Else
                    l_string_hubInd = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chrHubInd").ToString().Trim()
                    l_string_YmcaGuId = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                    l_string_YmcaMetroId = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiYMCAMetroId").ToString().Trim()

                    If l_string_hubInd = "B" Then
                        If l_string_YmcaGuId = l_string_YmcaMetroId Then
                            'message .. 'branch of ymca
                            'anita-IE7
                            MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Ymca " + Me.TextBoxYMCANo.Text.Trim() + " is a branch of " + Me.TextBoxYMCANo.Text.Trim(), MessageBoxButtons.OK)
                            Session("Control_Focus") = "YMCA_No"

                            l_Boolean_Flag = True
                        End If
                    End If
                End If

                ' the Ymca No is now correct

                l_date_checkdate = Me.TextBoxCheckDate.Text
                l_date_RecDate = Me.TextBoxRecDate.Text
                ''viewstate("Recieved_Date") = Me.TextBoxRecDate.Text
                l_Date_MinDate = Convert.ToDateTime("07/01/1922")
                If l_Boolean_Flag = False And (l_date_checkdate > Date.Today.ToShortDateString Or l_date_checkdate < l_Date_MinDate) Then
                    'ANITA -IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Check Date entered is outside of an acceptable range.", MessageBoxButtons.OK)
                    Session("Control_Focus") = "CheckDate"

                    l_Boolean_Flag = True
                End If

                If l_Boolean_Flag = False And (l_date_RecDate > Date.Today.ToShortDateString Or l_date_RecDate < l_Date_MinDate) Then
                    'ANITA -IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Recieved Date entered is outside of an acceptable range.", MessageBoxButtons.OK)
                    Session("Control_Focus") = "RecDate"

                    l_Boolean_Flag = True
                End If

                If l_Boolean_Flag = False And Convert.ToDecimal(Me.TextBoxAmount.Text) = 0 Then
                    'ANITA -IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Please Enter Positive Amount.", MessageBoxButtons.OK)
                    Session("Control_Focus") = "Amount"

                    l_Boolean_Flag = True
                End If
                If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_checkdate > l_date_RecDate) Then
                    'ANITA -IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Payment Date Can Not Be Greater Than Recieved Date", MessageBoxButtons.OK)
                    l_Boolean_Flag = True
                End If
                ' caution if the date is older than 6 months

                If l_Boolean_Flag = False And (l_date_checkdate.AddMonths(6) < Date.Today.ToShortDateString) Then
                    'ANITA -IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Caution! The Date entered is at least six months old.", MessageBoxButtons.OK)
                    Session("Control_Focus") = "CheckDate"

                    l_Boolean_Caution_Flag = True
                End If

                If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_RecDate.AddMonths(6) < Date.Today.ToShortDateString) Then
                    'ANITA -IE7
                    MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Caution! The Date entered is at least six months old.", MessageBoxButtons.OK)
                    Session("Control_Focus") = "RecDate"

                    l_Boolean_Caution_Flag = True
                End If
                If l_Boolean_Flag = False Then

                    If g_dt_tempTable.Columns.Count = 0 Then
                        CreateTempTable()
                    End If
                    If Not Session("session_tempTable") Is Nothing Then
                        g_dt_tempTable = Session("session_tempTable")
                    End If


                    If Session("State") = False Then
                        If Me.DataGridCashReceipt.SelectedIndex = -1 Then

                            drow = g_dt_tempTable.NewRow()

                            drow("YmcaNo.") = Me.TextBoxYMCANo.Text
                            drow("Name") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chvYmcaName").ToString().Trim()
                            drow("Source") = Me.DropDownListSource.SelectedItem.Text.Trim()
                            drow("RefInfo") = Me.TextBoxRefInfo.Text
                            drow("CheckDate") = Me.TextBoxCheckDate.Text
                            drow("Rec.Date") = Me.TextBoxRecDate.Text
                            drow("Amount") = Me.TextBoxAmount.Text
                            drow("Comments") = Me.TextBoxComments.Text
                            drow("Code") = Me.DropDownListSource.SelectedValue
                            drow("YmcaId") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                            Dim i As Integer = g_dt_tempTable.Rows.Count
                            g_dt_tempTable.Rows.InsertAt(drow, i)
                        Else
                            drow = g_dt_tempTable.Rows(Me.DataGridCashReceipt.SelectedIndex)

                            drow("YmcaNo.") = Me.TextBoxYMCANo.Text
                            drow("Name") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chvYmcaName").ToString().Trim()
                            drow("Source") = Me.DropDownListSource.SelectedItem.Text.Trim()
                            drow("RefInfo") = Me.TextBoxRefInfo.Text
                            drow("CheckDate") = Me.TextBoxCheckDate.Text
                            drow("Rec.Date") = Me.TextBoxRecDate.Text
                            drow("Amount") = Me.TextBoxAmount.Text
                            drow("Comments") = Me.TextBoxComments.Text
                            drow("Code") = Me.DropDownListSource.SelectedValue
                            drow("YmcaId") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                        End If

                    End If

                    Session("session_tempTable") = g_dt_tempTable
                    Me.DataGridCashReceipt.DataSource = g_dt_tempTable
                    Me.DataGridCashReceipt.DataBind()


                    Me.DataGridCashReceipt.SelectedIndex = 0
                    Me.DropDownListSource.Enabled = False
                    Me.TextBoxYMCANo.Enabled = False
                    Me.TextBoxCheckDate.Enabled = False
                    Me.TextBoxAmount.Enabled = False
                    Me.TextBoxRecDate.Enabled = False
                    Me.TextBoxComments.Enabled = False
                    Me.TextBoxRefInfo.Enabled = False
                    Me.ButtonSave.Enabled = False
                    Me.ButtonCancel.Enabled = False
                    Me.ButtonPost.Enabled = True
                    Me.ButtonAdd.Enabled = True
                    '     Me.ButtonDelete.Enabled = False



                    Me.DataGridCashReceipt.SelectedIndex = -1

                    For j = 0 To Me.DataGridCashReceipt.Items.Count - 1
                        l_temp_amount = l_temp_amount + Convert.ToDecimal(Me.DataGridCashReceipt.Items(j).Cells(7).Text.Trim())
                    Next
                    Me.TextboxTotal.Text = FormatCurrency(l_temp_amount)

                    Session("State") = True
                End If

            End If

            'Rahul 02 Mar,06
            ''''' Code for Add....
            Me.TextBoxYMCANo.Enabled = True
            Me.TextBoxCheckDate.Enabled = True
            Me.TextBoxAmount.Enabled = True
            Me.TextBoxRecDate.Enabled = True
            Me.TextBoxComments.Enabled = True
            Me.TextBoxRefInfo.Enabled = True
            Me.DropDownListSource.Enabled = True

            Me.TextBoxYMCANo.Text = ""
            Me.TextBoxCheckDate.Text = ""
            Me.TextBoxAmount.Text = ""
            ''Me.TextBoxRecDate.Text = ""
            Me.TextBoxComments.Text = ""
            Me.TextBoxRefInfo.Text = ""
            Me.DropDownListSource.SelectedIndex = 0

            ''Me.ButtonAdd.Enabled = False
            Me.ButtonSave.Enabled = False 'Rahul

            'commented by Hafiz on 6-Mar-2009
            'Me.ButtonCancel.Enabled = True
            'Added by Hafiz on 6-Mar-2009
            Me.ButtonCancel.Enabled = False

            Me.ButtonPost.Enabled = True 'Rahul
            Me.ButtonDelete.Enabled = False


            Session("State") = False

        Catch ex As SqlException
            g_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Me.TextBoxYMCANo.Text = String.Empty
            Me.TextBoxAmount.Text = String.Empty
            Me.TextBoxCheckDate.Text = String.Empty
            Me.TextBoxComments.Text = String.Empty
            Me.TextBoxRefInfo.Text = String.Empty
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonAdd.Enabled = True


            Dim i As Integer
            For i = 0 To Me.DataGridCashReceipt.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridCashReceipt.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridCashReceipt.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If

            Next

            Me.DataGridCashReceipt.SelectedIndex = -1

            If Me.DataGridCashReceipt.Items.Count > 0 Then
                Me.ButtonPost.Enabled = True
            Else
                Me.ButtonPost.Enabled = False
            End If

            'Me.DropDownListSource.Enabled = False
            'Me.TextBoxYMCANo.Enabled = False
            'Me.TextBoxCheckDate.Enabled = False
            'Me.TextBoxAmount.Enabled = False
            'Me.TextBoxRecDate.Enabled = False
            'Me.TextBoxComments.Enabled = False
            'Me.TextBoxRefInfo.Enabled = False

            'Me.ButtonSave.Enabled = False
            'Me.ButtonCancel.Enabled = False
            'Me.ButtonAdd.Enabled = True
            'Me.DataGridCashReceipt.SelectedIndex = -1

            'If Me.DataGridCashReceipt.Items.Count > 0 Then
            '    Me.ButtonPost.Enabled = True
            'Else
            '    Me.ButtonPost.Enabled = False
            'End If
        Catch ex As Exception
            g_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPost.Click
        Try
            Dim i As Integer
            Dim l_DataSet_Ymca_Name As DataSet
            Dim l_string_YmcaId As String
            Dim l_decimal_amount As Decimal
            Dim l_string_SourceCode As String
            Dim l_date_checkDate As DateTime
            Dim l_date_RecieveDate As DateTime
            Dim l_string_comments As String
            Dim l_string_refInfo As String

            l_DataSet_Ymca_Name = Session("DataSetYmca")
            For i = 0 To Me.DataGridCashReceipt.Items.Count - 1
                l_string_YmcaId = Me.DataGridCashReceipt.Items(i).Cells(10).Text.Trim()

                l_string_SourceCode = Me.DataGridCashReceipt.Items(i).Cells(9).Text.Trim()

                l_decimal_amount = Me.DataGridCashReceipt.Items(i).Cells(7).Text.Trim()
                l_date_checkDate = Convert.ToDateTime(Me.DataGridCashReceipt.Items(i).Cells(5).Text.Trim())

                l_date_RecieveDate = Convert.ToDateTime(Me.DataGridCashReceipt.Items(i).Cells(6).Text.Trim())
                'Added by Swopna on 29 Jan,2008(Reference-Hafiz's email,Subject-YMCA QA Testing status of 4.0.6 patch,Date-26 Jan,2008)
                '**************
                If Me.DataGridCashReceipt.Items(i).Cells(4).Text.Trim() = "&nbsp;" Then
                    l_string_refInfo = ""
                Else
                    l_string_refInfo = Me.DataGridCashReceipt.Items(i).Cells(4).Text.Trim()
                End If
                '***********
                'commented by Swopna
                'l_string_refInfo = Me.DataGridCashReceipt.Items(i).Cells(4).Text.Trim()

                'Added by Swopna on 29 Jan,2008(Reference-Hafiz's email,Subject-YMCA QA Testing status of 4.0.6 patch,Date-26 Jan,2008)
                '*********
                If Me.DataGridCashReceipt.Items(i).Cells(8).Text.Trim() = "&nbsp;" Then
                    l_string_comments = ""
                Else
                    l_string_comments = Me.DataGridCashReceipt.Items(i).Cells(8).Text.Trim()
                End If
                '***********
                'commented by swopna
                'l_string_comments = Me.DataGridCashReceipt.Items(i).Cells(8).Text.Trim()

                YMCARET.YmcaBusinessObject.CashRecieptsEntry.InsertCashReciept(l_string_YmcaId, l_string_SourceCode, l_decimal_amount, l_date_RecieveDate, l_string_refInfo, l_date_checkDate, l_string_comments)
            Next
            Session("State") = False
            Session("session_tempTable") = Nothing

            Session("blnCashReceiptEntryPosted") = True
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
        Response.Redirect("MainWebForm.aspx", False)
    End Sub
    Private Sub DataGridCashReceipt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridCashReceipt.ItemDataBound
        Try
            'Dim l_button_Select As ImageButton
            'l_button_Select = e.Item.FindControl("ImageButtonSelect")
            'If (e.Item.ItemIndex = Me.DataGridCashReceipt.SelectedIndex And Me.DataGridCashReceipt.SelectedIndex >= 0) Then
            '    l_button_Select.ImageUrl = "images\selected.gif"
            'End If

            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            'anita and shubhrata apr25th
            If e.Item.ItemType <> ListItemType.Header Then

                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Then

                    Dim l_decimal_try As Decimal

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                    e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)
                End If
            End If


            'anita and shubhrata apr25th

            If Not e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If

        Catch ex As SqlException
            g_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DataGridCashReceipt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridCashReceipt.SelectedIndexChanged
        Try
            Dim i As Integer
            For i = 0 To Me.DataGridCashReceipt.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridCashReceipt.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridCashReceipt.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If

            Next
            Me.DropDownListSource.Enabled = True
            Me.TextBoxYMCANo.Enabled = True
            Me.TextBoxCheckDate.Enabled = True
            Me.TextBoxAmount.Enabled = True
            Me.TextBoxRecDate.Enabled = True
            Me.TextBoxComments.Enabled = True
            Me.TextBoxRefInfo.Enabled = True

            Me.TextBoxYMCANo.Text = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(1).Text.Trim()
            Me.TextBoxCheckDate.Text = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(5).Text.Trim()
            Me.TextBoxAmount.Text = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(7).Text.Trim()
            Me.TextBoxRecDate.Text = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(6).Text.Trim()
            If Not Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(8).Text = "" And Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(8).Text.Trim() <> "&nbsp;" Then
                Me.TextBoxComments.Text = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(8).Text.Trim()
            Else
                Me.TextBoxComments.Text = ""
            End If
            If Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(4).Text.Trim() <> "&nbsp;" And Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(4).Text <> "" Then
                Me.TextBoxRefInfo.Text = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(4).Text.Trim()
            Else
                Me.TextBoxRefInfo.Text = ""
            End If

            Me.DropDownListSource.SelectedValue = Me.DataGridCashReceipt.Items(Me.DataGridCashReceipt.SelectedIndex).Cells(9).Text.Trim()
            'me.DataGridCashReceipt.Items(me.DataGridCashReceipt.SelectedIndex)

            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.ButtonAdd.Enabled = False
            Me.ButtonDelete.Enabled = True

            Dim strJS As String
            strJS = "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {document.forms[0].elements['ButtonSave'].click();return false;} else return true; "
            Me.TextBoxComments.Attributes.Add("onkeydown", strJS)

            Session("State") = False
            SetFocus(TextBoxYMCANo)
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Session("State") = False
        Session("session_tempTable") = Nothing
        Response.Redirect("MainWebForm.aspx", False)

    End Sub
    Private Sub DataGridCashReceipt_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridCashReceipt.ItemCommand
        Try
            If (e.CommandName = "Select") Then
                Dim l_button_select As ImageButton
                l_button_select = e.Item.FindControl("ImageButtonSelect")
                ' If e.Item.ItemIndex =Me.DataGridDisbursementDetails.SelectedIndex And Me.DataGridDisbursementDetails.SelectedIndex >= 0 Then
                l_button_select.ImageUrl = "images\selected.gif"
            End If
        Catch ex As Exception
            g_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message)
        End Try


    End Sub
    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Dim drow As DataRow
        Try
            If Not Session("session_tempTable") Is Nothing Then
                g_dt_tempTable = Session("session_tempTable")
            End If
            If Not Me.DataGridCashReceipt.SelectedIndex = -1 Then

                drow = g_dt_tempTable.Rows(Me.DataGridCashReceipt.SelectedIndex)
                g_dt_tempTable.Rows.Remove(drow)
                g_dt_tempTable.AcceptChanges()
                Session("session_tempTable") = g_dt_tempTable
                'Commented by Aparna 03/09/2007
                'Need to act only if the record in datagrid is selected for deletion.
                'End If

                'If Me.DataGridCashReceipt.Items.Count = 1 Then
                '    Me.DataGridCashReceipt.DataSource = Nothing
                '    Me.DataGridCashReceipt.DataBind()
                '    Me.ButtonPost.Enabled = False
                '    Me.TextboxTotal.Text = ""
                'Else
                If g_dt_tempTable.Rows.Count > 0 Then
                    Me.DataGridCashReceipt.DataSource = g_dt_tempTable
                    Me.DataGridCashReceipt.DataBind()
                    Me.ButtonPost.Enabled = True
                Else
                    Me.ButtonPost.Enabled = False
                    Me.DataGridCashReceipt.DataSource = Nothing
                    Me.DataGridCashReceipt.DataBind()
                End If

                Dim j As Integer
                Dim l_temp_amount As Decimal
                For j = 0 To Me.DataGridCashReceipt.Items.Count - 1
                    l_temp_amount = l_temp_amount + Convert.ToDecimal(Me.DataGridCashReceipt.Items(j).Cells(7).Text.Trim())
                Next
                Me.TextboxTotal.Text = FormatCurrency(l_temp_amount)

                Me.TextBoxYMCANo.Text = ""
                Me.TextBoxCheckDate.Text = ""
                Me.TextBoxAmount.Text = ""
                Me.TextBoxComments.Text = ""
                Me.TextBoxRefInfo.Text = ""
                Me.DropDownListSource.SelectedIndex = 0

                Me.DataGridCashReceipt.SelectedIndex = -1

                Me.ButtonAdd.Enabled = True
                Me.ButtonSave.Enabled = False
                Me.ButtonCancel.Enabled = False
                Me.ButtonDelete.Enabled = False
            Else
                MessageBox.Show(PlaceHolderCashEntry, "YMCA-YRS", "Please select a record to delete.", MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message)
        End Try
    End Sub
    Private Sub SetFocus(ByVal ctrl As Control)
        ' Define the JavaScript function for the specified control.
        Dim focusScript As String = "<script language='javascript'>" & _
          "document.getElementById('" + ctrl.ClientID & _
          "').focus();</script>"

        ' Add the JavaScript code to the page.
        Page.RegisterStartupScript("FocusScript", focusScript)
    End Sub
    Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
        Try
            Dim n As String
            Dim m As String()
            Dim myNum As String

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

                If m.Length = 1 Then
                    val = String.Join("", fmat) + ".00"
                Else
                    val = String.Join("", fmat) + "." + m(1)
                End If

            End If

            Return val

        Catch ex As Exception
            Return paramNumber
        End Try

    End Function
    'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonAdd.Enabled = False
            ButtonAdd.ToolTip = toolTip
            ButtonPost.Enabled = False
            ButtonPost.ToolTip = toolTip
        End If
    End Sub
    'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

End Class
