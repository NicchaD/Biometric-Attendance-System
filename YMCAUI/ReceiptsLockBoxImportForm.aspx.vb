'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ReceiptsLockBoxImportForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:18:26 PM
' Program Specification Name	:	YMCA PS 3.10.2.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	Vartika Jain,Ashutosh Goswami
' Changed on			:	10/15/2005
' Change Description	:	Coding
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N                       03/04/2019          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
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
Imports System.Security
Imports System.IO
Imports System.IO.File
Imports System.IO.FileAccess

Imports System.Text

Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class ReceiptsLockBoxImportForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("ReceiptsLockBoxImportForm.aspx")
    'End issue id YRS 5.0-940

    Private Enum DgLockBox
        YMCANo = 2
        Name
        Source
        RefInfo
        PayDate
        RecDate
        Amount
        Code
        YmcaId
        Comments
        ABANum
        AcctNum
    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridLockBoxReceipts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelYMCANo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSource As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListSource As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelRefInfo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRefInfo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRecDate As System.Web.UI.WebControls.Label
    '' Protected WithEvents TextBoxRecDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxRecDate As YMCAUI.DateUserControl
    Protected WithEvents LabelAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelComments As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxComments As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelABANum As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxABANum As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAcctNum As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAcctNum As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPost As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxGeneral As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    '' Protected WithEvents TextBoxPayDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPayDate As YMCAUI.DateUserControl
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonFileSelect As System.Web.UI.WebControls.Button
    Protected WithEvents FileField As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents PopcalendaPayDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents ReqPayDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PopcalendarRecDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents ReqRecDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqRefInfo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PlaceHolderLockBox As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ReqAmount As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqYMCANo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim _myFileName As String
    Dim g_Bool_Err_Flag As Boolean
    Dim dt_TempBind As New DataTable
    Dim g_String_Exception_Message As String
    Dim fs As FileStream

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            CheckReadOnlyMode() ' Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

            Me.TextBoxPayDate.RequiredDate = True
            Me.TextBoxRecDate.RequiredDate = True
            If Not Me.IsPostBack Then
                TextBoxRecDate.Enabled = False
                TextBoxPayDate.Enabled = False
                Me.TextBoxRecDate.Attributes.Add("onBlur", "return CompareDate('TextBoxRecDate','TextBoxPayDate');")
                Me.LabelAmount.AssociatedControlID = Me.TextBoxAmount.ID
                Me.LabelComments.AssociatedControlID = Me.TextBoxComments.ID
                ''Me.LabelPayDate.AssociatedControlID = Me.TextBoxPayDate.ID
                ''Me.LabelRecDate.AssociatedControlID = Me.TextBoxRecDate.ID
                Me.LabelRefInfo.AssociatedControlID = Me.TextBoxRefInfo.ID
                Me.LabelSource.AssociatedControlID = Me.DropDownListSource.ID
                Me.LabelYMCANo.AssociatedControlID = Me.TextBoxYMCANo.ID

                Me.TextBoxYMCANo.Attributes.Add("onkeypress", "Javascript:ValidateNumeric();")
                Me.TextBoxAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.TextBoxAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                Me.TextBoxYMCANo.Attributes.Add("onBlur", "javascript:FormatYMCANo();")
                Me.TextBoxPayDate.RequiredDate = True
                Me.TextBoxRecDate.RequiredDate = True
                Me.ButtonAdd.Enabled = False
                Me.ButtonCancel.Enabled = False
                Me.ButtonPost.Enabled = False
                Me.ButtonSave.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.DropDownListSource.Enabled = False
            End If
            If Request.Form("OK") = "OK" Then
                'Me.TextBoxPayDate.RequiredDate = False
                'Me.TextBoxRecDate.RequiredDate = False
                ReqAmount.Enabled = False
                ReqRefInfo.Enabled = False
                ReqYMCANo.Enabled = False
            Else
                ReqAmount.Enabled = True
                ReqRefInfo.Enabled = True
                ReqYMCANo.Enabled = True
            End If
           
            'If Not Session("FileName") Is Nothing Then
            '    setTotalPostingAmount()
            'End If

            'If Not Session("FileCopied") Is Nothing Then 'Session varible in Copy FiletoFileServer
            '    PostedFile()
            'End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

#Region "Button Events"
    Private Sub ButtonFileSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFileSelect.Click
        'Dim errMess As String
        'errMess = "Verify CSV File Format-"
        Try

            If FileField.PostedFile.FileName = "" Or FileField.PostedFile.FileName.IndexOf(".csv") < 0 Then
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Please Select a valid Lock Box File.", MessageBoxButtons.Stop)
            Else

                Dim myfile As HttpPostedFile
                Dim nFileLen As Integer
                Dim l_string_FilePath As String

                Dim _str_FileName() As String
                'Code added by ashutosh----
                l_string_FilePath = ConfigurationSettings.AppSettings("LockBox")
                '*************************************
                myfile = FileField.PostedFile
                nFileLen = myfile.ContentLength
                'Code added by ashutosh*************
                If nFileLen = 0 Then
                    MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Lock Box file does not exists.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                '**********************************
                Dim myData(nFileLen) As Byte
                myfile.InputStream.Read(myData, 0, nFileLen)
                _str_FileName = Split(myfile.FileName, "\", -1)
                _myFileName = _str_FileName(_str_FileName.Length - 1)
                Session("FileName") = _myFileName
                ''ASHUTOSH 20-jULY-06
                '
                If _myFileName.Length > 25 Then
                    MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "File name cannot exceed 25 characters including extension. Cannot Post", MessageBoxButtons.Stop)
                    '  Exit Sub
                End If
                ''''System.IO.File.OpenWrite(Server.MapPath("Reports") + "\" + Session("FileName")).BeginWrite(myData, 0, nFileLen, , )
                ' Open the stream and read it back.
                ' Delete the file if it exists.
                If File.Exists(l_string_FilePath + "\" + Session("FileName")) = False Then
                    ' Create the file.
                    fs = File.Create(l_string_FilePath + "\" + Session("FileName"))
                    fs.Close()
                End If
                fs = File.OpenWrite(l_string_FilePath + "\" + Session("FileName"))
                ' Add some information to the file.
                fs.Write(myData, 0, nFileLen)
                fs.Close()


                'WriteToFile("D:\UploadedFile\" + Session("FileName"), myData)
                ''' WriteToFile(Server.MapPath("Reports") + "\" + Session("FileName"), myData)


                CheckFileImported()
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            Me.DataGridLockBoxReceipts.SelectedIndex = -1

            Me.DropDownListSource.Enabled = True
            Me.TextBoxYMCANo.Enabled = True
            Me.TextBoxPayDate.Enabled = True
            Me.TextBoxAmount.Enabled = True
            Me.TextBoxRecDate.Enabled = True
            Me.TextBoxComments.Enabled = True
            Me.TextBoxRefInfo.Enabled = True
            'Start- Added by Pankaj on 12th April 2006
            Me.TextBoxABANum.Enabled = True
            Me.TextBoxAcctNum.Enabled = True
            'End- Added by Pankaj on 12th April 2006
            Me.TextBoxAmount.Text = ""
            Me.TextBoxComments.Text = ""
            Me.TextBoxPayDate.Text = ""
            Me.TextBoxRecDate.Text = ""
            Me.TextBoxRefInfo.Text = ""
            Me.TextBoxYMCANo.Text = ""
            'Start- Added by Pankaj on 12th April 2006
            Me.TextBoxABANum.Text = ""
            Me.TextBoxAcctNum.Text = ""
            'End- Added by Pankaj on 12th April 2006

            PopulatePaymentTypes()

            Me.ButtonAdd.Enabled = False
            Me.ButtonCancel.Enabled = True
            Me.ButtonPost.Enabled = False
            Me.ButtonSave.Enabled = True

            Session("State") = False

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
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_dataset_dsLookupYmca As DataSet
            Dim l_ds_checkYmcaNameAndNum As DataSet
            Dim l_str_ymcaNo As String = ""
            Dim l_string_checkYmca As String = ""
            Dim l_str_ABANum As String
            Dim l_Boolean_Flag As Boolean = False
            Dim l_Boolean_Caution_Flag As Boolean = False
            Dim l_date_PayDate As Date
            Dim l_date_RecDate As Date
            Dim drow As DataRow
            Dim l_temp_amount As Double
            Dim j As Integer
            dt_TempBind = viewstate("dt_TempBind")

            l_dataset_dsLookupYmca = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.LookUpYMCA(Me.TextBoxYMCANo.Text.Trim())
            ' System.AppDomain.CurrentDomain.SetData("DataSetYmca", l_dataset_dsLookupYmca)
            Session("DataSetYmca") = l_dataset_dsLookupYmca

            If l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows.Count = 0 Then
                'message ---ymca does not exist
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Ymca " + Me.TextBoxYMCANo.Text.Trim() + " not found.", MessageBoxButtons.Stop)
                l_Boolean_Flag = True
            End If

            'Start- Added by Pankaj on 18th April 2006
            l_str_ABANum = Me.TextBoxABANum.Text.Trim
            If l_str_ABANum.Length < 9 Then
                Dim i As Integer
                For i = 0 To (9 - (l_str_ABANum.Length + 1))
                    l_str_ABANum = 0 & l_str_ABANum
                Next
            End If
            l_ds_checkYmcaNameAndNum = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.LookUpABANumAcctNum(l_str_ABANum, Me.TextBoxAcctNum.Text.Trim)
            If l_ds_checkYmcaNameAndNum.Tables(0).Rows.Count <> 0 Then
                l_string_checkYmca = l_ds_checkYmcaNameAndNum.Tables(0).Rows(0)("chrYmcaNo").ToString().Trim()
            End If

            l_str_ymcaNo = Me.TextBoxYMCANo.Text.Trim
            If l_str_ymcaNo.Length < 6 Then
                Dim i As Integer
                For i = 0 To (6 - (l_str_ymcaNo.Length + 1))
                    l_str_ymcaNo = 0 & l_str_ymcaNo
                Next
            End If

            If l_Boolean_Flag = False Then
                If l_string_checkYmca <> l_str_ymcaNo Then
                    MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Ymca No:" + Me.TextBoxYMCANo.Text.Trim() + " not found for the specified ABANo. and AcctNo.", MessageBoxButtons.Stop)
                    l_Boolean_Flag = True
                End If
            End If
            'End- Added by Pankaj on 18th April 2006

            l_date_PayDate = Me.TextBoxPayDate.Text
            l_date_RecDate = Me.TextBoxRecDate.Text
            'Doubt in this Ashutosh on 24/05/2006 **********************
            If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_PayDate > Date.Today.ToShortDateString Or l_date_RecDate > Date.Today.ToShortDateString) Then
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Date can not be Greater than Today's Date.", MessageBoxButtons.Stop)
                l_Boolean_Flag = True
            End If
            '*****************************************************
            If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And Convert.ToDecimal(Me.TextBoxAmount.Text) < 0 Then
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Please Enter a Positve Amount or Zero.", MessageBoxButtons.Stop)
                l_Boolean_Flag = True
            End If
            'If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_PayDate < l_date_RecDate) Then
            If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_PayDate > l_date_RecDate) Then
                'MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Recieved Date Can Not Be Greater Than Payment Date", MessageBoxButtons.Stop)
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Payment Date can not be greater than the Received Date", MessageBoxButtons.Stop)
                l_Boolean_Flag = True
            End If

            ' caution if the date is older than 6 months

            If l_Boolean_Flag = False And l_Boolean_Caution_Flag = False And (l_date_PayDate.AddMonths(6) < Date.Today.ToShortDateString Or l_date_RecDate.AddMonths(6) < Date.Today.ToShortDateString) Then
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Caution! The Date entered is at least six months old.", MessageBoxButtons.Stop)
                l_Boolean_Caution_Flag = True
            End If

            If l_Boolean_Flag = False Then
                If Session("State") = False Then
                    If Me.DataGridLockBoxReceipts.SelectedIndex = -1 Then
                        drow = dt_TempBind.NewRow()

                        drow("YMCANo") = Me.TextBoxYMCANo.Text.Trim
                        drow("Name") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chvYmcaName").ToString().Trim()
                        drow("Source") = Me.DropDownListSource.SelectedItem.Text.Trim()
                        drow("RefInfo") = Me.TextBoxRefInfo.Text.Trim
                        drow("PayDate") = Me.TextBoxPayDate.Text.Trim
                        drow("RecDate") = Me.TextBoxRecDate.Text.Trim
                        drow("Amount") = Me.TextBoxAmount.Text.Trim
                        drow("Code") = Me.DropDownListSource.SelectedValue.Trim
                        drow("YmcaId") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                        drow("Comments") = Me.TextBoxComments.Text.Trim
                        'Start-Added by Pankaj on 13th April 2006
                        drow("ABANum") = Me.TextBoxABANum.Text.Trim
                        drow("AcctNum") = Me.TextBoxAcctNum.Text.Trim
                        'End-Added by Pankaj on 13th April 2006

                        Dim i As Integer = dt_TempBind.Rows.Count
                        dt_TempBind.Rows.InsertAt(drow, i)
                    Else
                        drow = dt_TempBind.Rows(Me.DataGridLockBoxReceipts.SelectedIndex)


                        drow("YMCANo") = Me.TextBoxYMCANo.Text.Trim
                        drow("Name") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("chvYmcaName").ToString().Trim()
                        drow("Source") = Me.DropDownListSource.SelectedItem.Text.Trim()
                        drow("RefInfo") = Me.TextBoxRefInfo.Text.Trim
                        drow("PayDate") = Me.TextBoxPayDate.Text.Trim
                        drow("RecDate") = Me.TextBoxRecDate.Text.Trim
                        drow("Amount") = Me.TextBoxAmount.Text.Trim
                        drow("Code") = Me.DropDownListSource.SelectedValue.Trim
                        drow("YmcaId") = l_dataset_dsLookupYmca.Tables("ay_selectYmca").Rows(0)("guiUniqueId").ToString().Trim()
                        drow("Comments") = Me.TextBoxComments.Text.Trim
                        'Start-Added by Pankaj on 13th April 2006
                        drow("ABANum") = Me.TextBoxABANum.Text.Trim
                        drow("AcctNum") = Me.TextBoxAcctNum.Text.Trim
                        'End-Added by Pankaj on 13th April 2006
                    End If
                End If

                Me.DataGridLockBoxReceipts.DataSource = Nothing
                Me.DataGridLockBoxReceipts.DataBind()
                Me.DataGridLockBoxReceipts.DataSource = dt_TempBind
                Me.DataGridLockBoxReceipts.DataBind()

                Me.DropDownListSource.Enabled = False
                Me.TextBoxYMCANo.Enabled = False
                Me.TextBoxPayDate.Enabled = False
                Me.TextBoxAmount.Enabled = False
                Me.TextBoxRecDate.Enabled = False
                Me.TextBoxComments.Enabled = False
                Me.TextBoxRefInfo.Enabled = False
                'Start-Added by Pankaj on 13th April 2006
                Me.TextBoxABANum.Enabled = False
                Me.TextBoxAcctNum.Enabled = False

                Me.TextBoxAmount.Text = ""
                Me.TextBoxComments.Text = ""
                Me.TextBoxPayDate.Text = ""
                Me.TextBoxRecDate.Text = ""
                Me.TextBoxRefInfo.Text = ""
                Me.TextBoxYMCANo.Text = ""
                Me.TextBoxABANum.Text = ""
                Me.TextBoxAcctNum.Text = ""

                'End-Added by Pankaj on 13th April 2006
                Me.ButtonSave.Enabled = False
                Me.ButtonCancel.Enabled = False
                Me.ButtonPost.Enabled = True
                Me.ButtonAdd.Enabled = True

                Me.DataGridLockBoxReceipts.SelectedIndex = -1

                'For j = 0 To Me.DataGridLockBoxReceipts.Items.Count - 1
                '    l_temp_amount = l_temp_amount + Convert.ToDecimal(Me.DataGridLockBoxReceipts.Items(j).Cells(DgLockBox.Amount).Text.Trim())
                'Next
                'Me.TextBoxGeneral.Text = String.Format("{0:N}", l_temp_amount)

                SetTotalPostingAmount()

                Session("State") = True

            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        Try

            If DataGridLockBoxReceipts.Items.Count = 0 Then
                MessageBox.Show(PlaceHolderLockBox, "LOCK BOX", "There are no records in the Grid", MessageBoxButtons.Stop)
                Exit Sub
            End If
            SelectAll()
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Me.DropDownListSource.Enabled = False
            Me.TextBoxYMCANo.Enabled = False
            Me.TextBoxPayDate.Enabled = False
            Me.TextBoxAmount.Enabled = False
            Me.TextBoxRecDate.Enabled = False
            Me.TextBoxComments.Enabled = False
            Me.TextBoxRefInfo.Enabled = False
            Me.TextBoxABANum.Enabled = False
            Me.TextBoxAcctNum.Enabled = False

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonPost.Enabled = True
            Me.ButtonAdd.Enabled = True
            Dim l_button_select As ImageButton
            Dim i As Integer
            Me.DataGridLockBoxReceipts.SelectedIndex = -1

            For i = 0 To Me.DataGridLockBoxReceipts.Items.Count - 1
                l_button_select = Me.DataGridLockBoxReceipts.Items(i).FindControl("ImageButtonSelect")
                l_button_select.ImageUrl = "images\select.gif"
            Next

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPost.Click
        Try
            Dim i As Integer
            Dim l_boolean As Boolean = False


            Dim int_t As String
            If DataGridLockBoxReceipts.Items.Count > 0 Then


                int_t = Me.DataGridLockBoxReceipts.Items(0).Cells(DgLockBox.Amount).Text()
                Dim dtLockBoxReceipts As DataTable
                dtLockBoxReceipts = viewstate("dt_TempBind")
                Dim dtPostData As DataTable
                Dim dtNotPostedData As DataTable

                dtNotPostedData = CreateTableForExecl()
                dtPostData = dtLockBoxReceipts.Clone()

                Dim chkFlag As System.Web.UI.WebControls.CheckBox
                Dim dgItem As DataGridItem
                Dim drNewRow As DataRow

                Dim l_string_YMCAId As String
                Dim l_string_Code As String
                Dim l_decimal_Amount As Decimal
                Dim l_string_PayDate As String
                Dim l_string_RecvdDate As String
                Dim l_string_RefInfo As String
                Dim l_string_Comment As String

                i = 0

                For Each dgItem In DataGridLockBoxReceipts.Items

                    chkFlag = dgItem.FindControl("chkFlag")
                    l_string_YMCAId = Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text().Trim()
                    'l_string_Code = Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Code).Text().Trim()
                    'l_decimal_Amount = Convert.ToDecimal(Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text())
                    'l_string_PayDate = Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.PayDate).Text().Trim()
                    'l_string_RefInfo = Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RefInfo).Text().Trim()
                    'l_string_RecvdDate = Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RecDate).Text().Trim()
                    'l_string_Comment = Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Comments).Text().Trim().Replace("&nbsp;", "")

                    'If chkFlag.Checked = True And Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text().ToUpper <> "&NBSP;" Then

                    If chkFlag.Checked = True And l_string_YMCAId.ToUpper <> "&NBSP;" Then
                        'YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.InsertRecieptLockBox(Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Code).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.PayDate).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RefInfo).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RecDate).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Comments).Text())
                        'YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.InsertRecieptLockBox(l_string_YMCAId, _
                        '                                                                        l_string_Code, _
                        '                                                                        l_decimal_Amount, _
                        '                                                                        l_string_RecvdDate, _
                        '                                                                        l_string_RefInfo, _
                        '                                                                        l_string_PayDate, _
                        '   
                        '                                          l_string_Comment)

                        drNewRow = dtPostData.NewRow()
                        drNewRow("YmcaId") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text()
                        drNewRow("Code") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Code).Text()
                        drNewRow("Amount") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text()
                        drNewRow("RecDate") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RecDate).Text()
                        drNewRow("RefInfo") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RefInfo).Text()
                        drNewRow("PayDate") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.PayDate).Text()
                        drNewRow("Comments") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Comments).Text().Replace("&nbsp;", "")
                        'drNewRow("YMCANo") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YMCANo).Text()
                        'drNewRow("Name") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Name).Text()
                        'drNewRow("Source") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Source).Text()
                        'drNewRow("ABANum") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.ABANum).Text()
                        'drNewRow("AcctNum") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.AcctNum).Text()
                        dtPostData.Rows.Add(drNewRow)

                    End If

                    If chkFlag.Checked = False Or l_string_YMCAId.ToUpper = "&NBSP;" Then
                        Dim temp As String
                        Dim ii As Integer
                        drNewRow = dtNotPostedData.NewRow()

                        drNewRow("RefInfo") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RefInfo).Text()
                        temp = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RecDate).Text() + "/" + DateTime.Now.ToShortTimeString()
                        drNewRow("RecDate") = temp.Substring(0, temp.Length - 2)
                        temp = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.PayDate).Text()
                        ii = temp.IndexOf("/", 4)
                        temp = temp.Remove(ii + 1, 2)
                        temp = temp.Replace("/", "")
                        drNewRow("PayDate") = temp
                        drNewRow("Amount") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text().Replace(",", "")
                        drNewRow("ABANum") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.ABANum).Text()
                        drNewRow("AcctNum") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.AcctNum).Text()
                        dtNotPostedData.Rows.Add(drNewRow)
                    End If

                    i = i + 1
                Next
                If dtPostData.Rows.Count > 0 Then
                    '  YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.UpdateProcessLog(Session("FileName"))

                    'Session("dtLockBoxExportClone") = dtNotPostedData.Clone()
                    'DataGridLockBoxReceipts.DataSource = Nothing
                    'DataGridLockBoxReceipts.DataBind()
                    'DataGridLockBoxReceipts.DataSource = dtNotPostedData.Clone()
                    'DataGridLockBoxReceipts.DataBind()
                    Me.TextBoxGeneral.Text = 0 'Ashutosh


                    Dim l_string_File_Path As String
                    Dim l_string_File_Name As String
                    Dim l_string_FILE_PRIFIX As String
                    Dim l_int_Length As Int32
                    Dim l_string_File_Name_Path As String
                    l_string_File_Path = ConfigurationSettings.AppSettings("LockBox")
                    'Ashutosh on 25-July-06

                    l_string_FILE_PRIFIX = Session("FileName")

                    If l_string_FILE_PRIFIX.Length > 25 Then
                        MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "File name cannot exceed 25 characters including extension. Cannot Post", MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'Changed by ashutosh Reason:Functionality has been changed now insert Filename only when posted.
                    l_string_FILE_PRIFIX = l_string_FILE_PRIFIX.Replace(".csv", "")

                    l_string_File_Name = l_string_FILE_PRIFIX + GenerateTextFile() + ".csv"
                    l_string_File_Name_Path = l_string_File_Path + "\" + l_string_File_Name

                    Call CreateCSVFile(dtNotPostedData, l_string_File_Name_Path, False)

                    If PostedFile(dtPostData) = True Then
                        If CopyToServer(l_string_File_Path, l_string_File_Name_Path, l_string_File_Name) Then
                            Dim l_string_Message As String = "Posted Sucessfully."
                            If dtNotPostedData.Rows.Count > 0 Then
                                l_string_Message = l_string_Message + vbCrLf + "Exception file:" + viewstate("DestFile").ToString()
                            End If

                            MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", l_string_Message, MessageBoxButtons.OK, True)
                            viewstate("DestFile") = Nothing
                            ' Session("FromPage") = "EDI" 
                            'Session("dtPostData") = dtPostData
                        End If
                    End If
                Else
                    MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "Please select valid record(s) to post", MessageBoxButtons.Stop)
                End If

                Session("State") = False

                Me.TextBoxAmount.Text = ""
                Me.TextBoxComments.Text = ""
                Me.TextBoxGeneral.Text = 0
                Me.TextBoxPayDate.Text = ""
                Me.TextBoxRecDate.Text = ""
                Me.TextBoxRefInfo.Text = ""
                Me.TextBoxYMCANo.Text = ""
                Me.TextBoxABANum.Text = ""
                Me.TextBoxAcctNum.Text = ""


                Me.DropDownListSource.Enabled = False
                Me.TextBoxYMCANo.Enabled = False
                Me.TextBoxPayDate.Enabled = False
                Me.TextBoxAmount.Enabled = False
                Me.TextBoxRecDate.Enabled = False
                Me.TextBoxComments.Enabled = False
                Me.TextBoxRefInfo.Enabled = False
                Me.TextBoxABANum.Enabled = False
                Me.TextBoxAcctNum.Enabled = False
                'Code Commented by ashutosh 0n 29-June-06
                'Me.DataGridLockBoxReceipts.DataSource = Nothing
                'Me.DataGridLockBoxReceipts.DataBind()
                '***************
                Me.ButtonSave.Enabled = False
                Me.ButtonCancel.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.ButtonAdd.Enabled = False
                'Me.ButtonPost.Enabled = False
                Me.ButtonFileSelect.Enabled = True

                'Session("State") = Nothing
                'Session("session_tempTable") = Nothing
                'Session("FileName") = Nothing
            Else
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "There is no record to post.", MessageBoxButtons.Stop)
            End If
            'Code edited by ashutosh on 30-05-06******************
            '  Session("blnLockBoxReceiptEntryPosted") = True
            'Code commented by ashutosh on 29-05-06
            ' Response.Redirect("MainWebForm.aspx", False)
            '***********************************
            '*************
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Dim drow As DataRow
        Try
            If Not viewstate("dt_TempBind") Is Nothing Then
                dt_TempBind = viewstate("dt_TempBind")
            End If
            If Not Me.DataGridLockBoxReceipts.SelectedIndex = -1 Then
                drow = dt_TempBind.Rows(Me.DataGridLockBoxReceipts.SelectedIndex)
            End If
            dt_TempBind.Rows.Remove(drow)
            dt_TempBind.AcceptChanges()

            Session("session_tempTable") = dt_TempBind

            If Me.DataGridLockBoxReceipts.Items.Count = 1 Then
                Me.DataGridLockBoxReceipts.DataSource = Nothing
                Me.DataGridLockBoxReceipts.DataBind()
                Me.ButtonPost.Enabled = False
                Me.TextBoxGeneral.Text = ""
            Else

                Me.DataGridLockBoxReceipts.DataSource = Nothing
                Me.DataGridLockBoxReceipts.DataBind()

                Me.DataGridLockBoxReceipts.DataSource = dt_TempBind
                Me.DataGridLockBoxReceipts.DataBind()
                Me.ButtonPost.Enabled = True
                Dim j As Integer
                Dim l_temp_amount As Double
                For j = 0 To Me.DataGridLockBoxReceipts.Items.Count - 1
                    l_temp_amount = l_temp_amount + Convert.ToDecimal(Me.DataGridLockBoxReceipts.Items(j).Cells(DgLockBox.Amount).Text.Trim())
                Next
                Me.TextBoxGeneral.Text = String.Format("{0:N}", l_temp_amount)
            End If

            Me.TextBoxYMCANo.Text = ""
            Me.TextBoxPayDate.Text = ""
            Me.TextBoxAmount.Text = ""
            Me.TextBoxRecDate.Text = ""
            Me.TextBoxComments.Text = ""
            Me.TextBoxRefInfo.Text = ""
            'Start - Added by Pankaj on 13th April 2006
            Me.TextBoxABANum.Text = ""
            Me.TextBoxAcctNum.Text = ""
            'End - Added by Pankaj on 13th April 2006
            Me.DropDownListSource.SelectedIndex = 0

            Me.DataGridLockBoxReceipts.SelectedIndex = -1

            Me.ButtonAdd.Enabled = True
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonDelete.Enabled = False

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        'Session("State") = Nothing
        'Session("session_tempTable") = Nothing
        'Session("FileName") = Nothing
        ClearSessionMemory()
        Response.Redirect("MainWebForm.aspx")
    End Sub
    Public Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        'Dim chkFlag As CheckBox = CType(sender, CheckBox)
        'Dim dbl_Total As Double
        'Dim dgItem As DataGridItem = CType(chkFlag.NamingContainer, DataGridItem)
        'If chkFlag.Checked = True Then

        '    dbl_Total = CType(Me.TextBoxGeneral.Text, Double) + CType(dgItem.Cells(DgLockBox.Amount).Text.Trim(), Double)
        'Else
        '    dbl_Total = CType(Me.TextBoxGeneral.Text, Double) - CType(dgItem.Cells(DgLockBox.Amount).Text.Trim(), Double)
        'End If
        'Me.TextBoxGeneral.Text = String.Format("{0:N}", dbl_Total)
        Try

            SetTotalPostingAmount()

        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub
#End Region

#Region "DataGrid Events"
    Private Sub DataGridLockBoxReceipts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridLockBoxReceipts.ItemDataBound
        Try
            e.Item.Cells(DgLockBox.Code).Visible = False
            e.Item.Cells(DgLockBox.YmcaId).Visible = False
            e.Item.Cells(DgLockBox.Comments).Visible = False
            If e.Item.Cells(DgLockBox.YMCANo).Text.ToUpper = "&NBSP;" Then
                e.Item.Cells(DgLockBox.YMCANo).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.Name).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.Source).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.RefInfo).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.PayDate).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.RecDate).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.Amount).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.ABANum).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(DgLockBox.AcctNum).ForeColor = System.Drawing.Color.Red
            End If
            'If e.Item.Cells(DgLockBox.Amount).Text.ToUpper <> "&NBSP;" And e.Item.Cells(DgLockBox.Amount).Text.ToUpper <> "AMOUNT" Then
            '    Dim l_decimal_try As Decimal
            '    l_decimal_try = Convert.ToDecimal(e.Item.Cells(DgLockBox.Amount).Text)
            '    e.Item.Cells(DgLockBox.Amount).Text = String.Format("{0:N}", l_decimal_try)

            'End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DataGridLockBoxReceipts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridLockBoxReceipts.SelectedIndexChanged
        Try
            Dim l_button_select As ImageButton
            Dim i As Integer
            For i = 0 To Me.DataGridLockBoxReceipts.Items.Count - 1
                If i = Me.DataGridLockBoxReceipts.SelectedIndex Then
                    l_button_select = Me.DataGridLockBoxReceipts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select = Me.DataGridLockBoxReceipts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\select.gif"
                End If
            Next

            Me.DropDownListSource.Enabled = True
            Me.TextBoxYMCANo.Enabled = True
            Me.TextBoxPayDate.Enabled = True
            Me.TextBoxAmount.Enabled = True
            Me.TextBoxRecDate.Enabled = True
            Me.TextBoxComments.Enabled = True
            Me.TextBoxRefInfo.Enabled = True
            Me.TextBoxABANum.Enabled = True
            Me.TextBoxAcctNum.Enabled = True

            Me.ButtonSave.Enabled = True
            Me.ButtonDelete.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.ButtonPost.Enabled = False
            Me.ButtonAdd.Enabled = False

            Dim l_str_ymcaNo As String
            Dim counter As Integer
            l_str_ymcaNo = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.YMCANo).Text.Trim()
            If l_str_ymcaNo.ToUpper() = "&NBSP;" Then
                l_str_ymcaNo = ""
            End If
            If l_str_ymcaNo.Length < 6 And l_str_ymcaNo.Length > 0 Then
                For counter = 0 To (6 - (l_str_ymcaNo.Length + 1))
                    l_str_ymcaNo = 0 & l_str_ymcaNo
                Next
            End If

            Me.TextBoxYMCANo.Text = l_str_ymcaNo

            Me.TextBoxPayDate.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.PayDate).Text.Trim()
            Me.TextBoxAmount.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.Amount).Text.Trim()
            Me.TextBoxRecDate.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.RecDate).Text.Trim()

            If Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.Comments).Text = "" Or Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.Comments).Text = "&nbsp;" Then
                Me.TextBoxComments.Text = ""
            Else
                Me.TextBoxComments.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.Comments).Text.Trim()
            End If

            Me.TextBoxRefInfo.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.RefInfo).Text.Trim()
            Me.DropDownListSource.SelectedValue = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.Code).Text.Trim()

            Me.TextBoxABANum.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.ABANum).Text.Trim()
            Me.TextBoxAcctNum.Text = Me.DataGridLockBoxReceipts.SelectedItem.Cells(DgLockBox.AcctNum).Text.Trim()

            Session("State") = False

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

#End Region

#Region "Function"
    Public Sub PopulatePaymentTypes()
        Try
            Dim l_dataset_PaymentTypes As DataSet
            l_dataset_PaymentTypes = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.LookUpPaymentTypes()
            Me.DropDownListSource.DataSource = l_dataset_PaymentTypes.Tables("PaymentTypes")
            Me.DropDownListSource.DataTextField = l_dataset_PaymentTypes.Tables("PaymentTypes").Columns("Description").ToString().Trim()
            Me.DropDownListSource.DataValueField = l_dataset_PaymentTypes.Tables("PaymentTypes").Columns("CodeValue").ToString().Trim()
            Me.DropDownListSource.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CheckFileImported()
        Try

            Dim l_ds_CheckImportedFile As DataSet
            Dim l_string_FilePath As String
            l_ds_CheckImportedFile = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.CheckFileImported(Session("FileName"))
            l_string_FilePath = ConfigurationSettings.AppSettings("LockBox")
            If l_ds_CheckImportedFile.Tables(0).Rows.Count <> 0 Then
                MessageBox.Show(PlaceHolderLockBox, "YMCA-YRS", "This file has already been Imported.", MessageBoxButtons.Stop)
                'Code changed by ashutosh on 22-06-06***************

                ' If File.Exists(Server.MapPath("Reports") + "\" + Session("FileName")) = True Then
                If File.Exists(l_string_FilePath + "\" + Session("FileName")) = True Then
                    '***************
                    ' Create the file.
                    File.Delete(l_string_FilePath + "\" + Session("FileName"))
                End If
            Else
                'Ashutosh 25-06-06
                'YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.InsertImportedFile(Session("FileName"))
                bindGrid()
                If File.Exists(l_string_FilePath + "\" + Session("FileName")) = True Then
                    ' Create the file.
                    File.Delete(l_string_FilePath + "\" + Session("FileName"))
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub bindGrid()
        Dim Conn As System.Data.Odbc.OdbcConnection
        Dim ds As New DataSet
        Dim dbl_Total As Double
        Dim da As System.Data.Odbc.OdbcDataAdapter
        Dim strConnstr, strImportfolder, strFilename As String
        Dim dr_TempBindRow As DataRow

        ' Me.ButtonFileSelect.Enabled = False
        ' dt_TempBind = Nothing
        dt_TempBind = New DataTable
        dt_TempBind.Columns.Add("YMCANo")
        dt_TempBind.Columns.Add("Name")
        dt_TempBind.Columns.Add("Source")
        dt_TempBind.Columns.Add("RefInfo")
        dt_TempBind.Columns.Add("PayDate")

        dt_TempBind.Columns.Add("RecDate")
        dt_TempBind.Columns.Add("Amount")

        '''hidden columns

        dt_TempBind.Columns.Add("Code")
        dt_TempBind.Columns.Add("YmcaId")
        dt_TempBind.Columns.Add("Comments")
        'Start - Added by Pankaj on 12th April 2006 
        dt_TempBind.Columns.Add("ABANum")
        dt_TempBind.Columns.Add("AcctNum")
        'End - Added by Pankaj on 12th April 2006 

        dt_TempBind.AcceptChanges()


        Try
            strImportfolder = ConfigurationSettings.AppSettings("LockBox")  '''this is the folder in which the file resides
            strFilename = Session("FileName")

            'Start -Commented by Pankaj on 8th May,2006
            '''this is the csv file to be imported

            'strConnstr = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + strImportfolder + ";"
            'Conn = New Odbc.OdbcConnection(strConnstr)
            'Conn.Open()
            'da = New System.data.Odbc.OdbcDataAdapter("select * from [" + strFilename + "]", Conn)
            'da.Fill(ds)

            'Conn.Close()
            'End -Commented by Pankaj on 8th May,2006

            Dim l_StreamReader_FileRead As StreamReader = File.OpenText(strImportfolder + "\" + strFilename)
            Dim strline As String = l_StreamReader_FileRead.ReadLine
            Dim dt As New DataTable
            Dim dr1 As DataRow
            Dim l_chr As Char
            ds.Tables.Add(dt)
            Dim flag As Integer = 0
            'Code added by ashutosh 0n 22-05-06
            l_chr = Convert.ToChar(34)
            While strline <> Nothing
                strline = strline.Replace(l_chr, "")
                Dim strarray() As String = Split(strline, ",")
                If strarray.Length > 0 Then

                    If flag = 0 Then
                        For i As Integer = 0 To strarray.Length - 1
                            dt.Columns.Add("col" & i, GetType(String))
                        Next
                        flag = 1
                    End If
                    dr1 = dt.NewRow
                    For i As Integer = 0 To strarray.Length - 1

                        dr1(i) = strarray(i)

                    Next
                    dt.Rows.Add(dr1)
                End If

                strline = l_StreamReader_FileRead.ReadLine
            End While
            l_StreamReader_FileRead.Close()

            'commented by Pankaj on 12th April 2006
            'CheckAllYMCANo(ds)
            'Added by Pankaj on 12th April 2006
            g_Bool_Err_Flag = False

            If g_Bool_Err_Flag = False Then
                Dim strTemp As String
                For Each dr As DataRow In ds.Tables(0).Rows
                    If Not IsDBNull(dr(0)) Then
                        'Start-Added by Pankaj on 12th April
                        Dim l_ds_tempYmcaNameAndNum As DataSet
                        Dim l_str_ABANum As String
                        Dim l_str_AcctNum As String
                        l_str_ABANum = dr(4).ToString().Trim()
                        l_str_AcctNum = dr(5).ToString().Trim()
                        If l_str_ABANum.Length < 9 Then
                            Dim i As Integer
                            For i = 0 To (9 - (l_str_ABANum.Length + 1))
                                l_str_ABANum = 0 & l_str_ABANum
                            Next
                        End If
                        l_ds_tempYmcaNameAndNum = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.LookUpABANumAcctNum(l_str_ABANum, l_str_AcctNum)

                        'End- Added by Pankaj on 12th April
                        Dim l_ds_tempYmcaCheck As DataSet
                        Dim l_str_ymcaNo As String
                        'commented by Pankaj on 12th april 2006
                        ' l_str_ymcaNo = dr(0).ToString().Trim()
                        If l_ds_tempYmcaNameAndNum.Tables(0).Rows.Count <> 0 Then
                            l_str_ymcaNo = l_ds_tempYmcaNameAndNum.Tables(0).Rows(0)("chrYmcaNo").ToString().Trim()
                            If l_str_ymcaNo.Length < 6 Then
                                Dim i As Integer
                                For i = 0 To (6 - (l_str_ymcaNo.Length + 1))
                                    l_str_ymcaNo = 0 & l_str_ymcaNo
                                Next
                            End If

                            l_ds_tempYmcaCheck = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.LookUpYMCA(l_str_ymcaNo)
                        End If
                        dr_TempBindRow = dt_TempBind.NewRow

                        'dr_TempBindRow("YMCANo") = dr(0)
                        If l_ds_tempYmcaNameAndNum.Tables(0).Rows.Count <> 0 Then
                            dr_TempBindRow("YMCANo") = l_ds_tempYmcaNameAndNum.Tables(0).Rows(0)("chrYmcaNo").ToString().Trim()
                            dr_TempBindRow("Name") = l_ds_tempYmcaCheck.Tables(0).Rows(0)("chvYmcaName").ToString().Trim()
                        Else
                            dr_TempBindRow("YMCANo") = ""
                            dr_TempBindRow("Name") = ""
                        End If


                        'dr_TempBindRow("Source") = dr(1)
                        dr_TempBindRow("Source") = "CHECK"

                        If Not IsDBNull(dr(2)) Then
                            dr_TempBindRow("RefInfo") = dr(2)
                        Else
                            dr_TempBindRow("RefInfo") = ""
                        End If


                        'If Not IsDBNull(dr(3)) Then
                        '    dr_TempBindRow("Pay.Date") = Convert.ToDateTime(dr(3)).ToShortDateString
                        'Else
                        '    dr_TempBindRow("Pay.Date") = dr(3)
                        'End If
                        If Not IsDBNull(dr(1)) Then
                            Dim l_str_date As String
                            Dim l_str_checkDate As String
                            l_str_checkDate = dr(1)
                            'Code Edited by ashutosh
                            'l_str_checkDate = String.Format("{0:d}", l_str_checkDate)
                            ' Dim da As Date

                            If l_str_checkDate.Length < 6 Then
                                l_str_checkDate = "0" + l_str_checkDate
                            End If
                            l_str_date = l_str_checkDate.Substring(0, 2)
                            l_str_date = l_str_date & "/" & l_str_checkDate.Substring(2, 2)
                            l_str_date = l_str_date & "/" & l_str_checkDate.Substring(4, 2)
                            dr_TempBindRow("PayDate") = Convert.ToDateTime(l_str_date).ToShortDateString()
                        End If
                        '**********************************ashutosh
                        'If Not IsDBNull(dr(4)) Then
                        '    dr_TempBindRow("Rec.Date") = Convert.ToDateTime(dr(4)).ToShortDateString
                        'Else
                        '    dr_TempBindRow("Rec.Date") = dr(4)
                        'End If

                        If Not IsDBNull(dr(0)) Then
                            Dim l_str_date As String
                            Dim l_str_RecDate As String
                            Dim strArr() As String
                            Dim i1 As Integer
                            l_str_RecDate = dr(0)
                            If l_str_RecDate.Length > 0 Then
                                strArr = l_str_RecDate.Split("/")
                                l_str_RecDate = String.Empty
                                For i As Integer = 0 To 2
                                    l_str_date = strArr(i)
                                    If i = 2 Then
                                        If l_str_date.Length > 4 Then
                                            l_str_date = l_str_date.Substring(0, 4)
                                        End If
                                    End If
                                    l_str_RecDate = l_str_RecDate + l_str_date + "/"
                                Next
                                l_str_RecDate = l_str_RecDate.Remove(l_str_RecDate.Length - 1, 1)
                                dr_TempBindRow("RecDate") = Convert.ToDateTime(l_str_RecDate).ToShortDateString
                            End If
                        End If



                        If Not IsDBNull(dr(3)) Then
                            dr_TempBindRow("Amount") = dr(3)
                        Else
                            dr_TempBindRow("Amount") = 0
                        End If



                        'If dr(1).ToString.ToUpper.Trim = "ACH Debit".ToUpper Then
                        '    dr_TempBindRow("Code") = "ACH_DR"
                        'ElseIf dr(1).ToString.ToUpper.Trim = "Check".ToUpper Then
                        '    dr_TempBindRow("Code") = "CHECK"
                        'ElseIf dr(1).ToString.ToUpper.Trim = "Funds Transfer".ToUpper Then
                        '    dr_TempBindRow("Code") = "TRNSFR"
                        'End If

                        dr_TempBindRow("Code") = "CHECK"
                        If l_ds_tempYmcaNameAndNum.Tables(0).Rows.Count <> 0 Then
                            dr_TempBindRow("YmcaId") = l_ds_tempYmcaCheck.Tables(0).Rows(0)("guiUniqueId").ToString().Trim()
                        Else
                            dr_TempBindRow("YmcaId") = ""
                        End If

                        dr_TempBindRow("Comments") = ""

                        dr_TempBindRow("ABANum") = dr(4)
                        dr_TempBindRow("AcctNum") = dr(5)

                        dt_TempBind.Rows.Add(dr_TempBindRow)
                        dt_TempBind.GetChanges(DataRowState.Added)
                        dt_TempBind.AcceptChanges()

                    End If
                Next
                viewstate("dt_TempBind") = dt_TempBind
                PopulatePaymentTypes()

                DataGridLockBoxReceipts.DataSource = Nothing
                DataGridLockBoxReceipts.DataBind()


                DataGridLockBoxReceipts.DataSource = dt_TempBind
                DataGridLockBoxReceipts.DataBind()

                Me.ButtonAdd.Enabled = True
                Me.ButtonCancel.Enabled = False
                Me.ButtonPost.Enabled = True
                Me.ButtonSave.Enabled = False

            End If
            'Code Commented by ashutosh on 30-June
            'Dim j As Integer
            'For j = 0 To Me.DataGridLockBoxReceipts.Items.Count - 1
            '    dbl_Total = dbl_Total + CType(Me.DataGridLockBoxReceipts.Items(j).Cells(DgLockBox.Amount).Text, Double)
            'Next
            Me.TextBoxGeneral.Text = 0
            'Me.TextBoxGeneral.Text = CType(Me.TextBoxGeneral.Text, Double)
            '****************
        Catch ex As Exception
            MessageBox.Show(PlaceHolderLockBox, "LockBox File", "Verify the CSV File Format.", MessageBoxButtons.Stop)
            DataGridLockBoxReceipts.DataSource = Nothing
            DataGridLockBoxReceipts.DataBind()
            ' Throw ex
        End Try

    End Sub
    Public Sub WriteToFile(ByVal strPath As String, ByRef Buffer() As Byte)
        Try
            'Create a file
            Dim newFile As New FileStream(strPath, FileMode.Create)

            'Write data to the file
            newFile.Write(Buffer, 0, Buffer.Length)

            newFile.Close()


        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Public Sub CheckAllYMCANo(ByVal paramds As DataSet)
        Try
            Dim l_ds_tempYmcaCheck As DataSet
            Dim dtTemp As New DataTable
            dtTemp = paramds.Tables(0)


            ''' creating a temporary data table for keeping the rows having error
            Dim dt_errLog As New DataTable
            Dim dr_err As DataRow

            'dt_errLog = paramds.Tables(0).Clone

            dt_errLog.Columns.Add("YMCANo")
            dt_errLog.Columns.Add("ReferenceInfo")
            dt_errLog.Columns.Add("Amount")
            dt_errLog.Columns.Add("ErrMsg")
            dt_errLog.AcceptChanges()

            ''' scanning the imported dataset for any error in  YMCA no.

            For Each dr As DataRow In paramds.Tables(0).Rows
                Dim l_str_ymcaNo As String
                l_str_ymcaNo = dr(0).ToString().Trim()
                If l_str_ymcaNo.Length < 6 Then
                    Dim i As Integer
                    For i = 0 To (6 - (l_str_ymcaNo.Length + 1))
                        l_str_ymcaNo = 0 & l_str_ymcaNo
                    Next
                End If
                l_ds_tempYmcaCheck = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.LookUpYMCA(l_str_ymcaNo)
                If l_ds_tempYmcaCheck.Tables(0).Rows.Count = 0 Then
                    Dim l_str_amt As String

                    dr_err = dt_errLog.NewRow()
                    dr_err("YMCANo") = dr(0)
                    dr_err("ReferenceInfo") = dr(2)
                    l_str_amt = "$" & dr(5)
                    dr_err("Amount") = l_str_amt
                    dr_err("ErrMsg") = "YMCA Not Found"

                    g_Bool_Err_Flag = True

                    dt_errLog.Rows.Add(dr_err)
                    dt_errLog.GetChanges(DataRowState.Added)

                    dt_errLog.AcceptChanges()

                End If
            Next
            If g_Bool_Err_Flag = True Then
                Session("Error_Table") = dt_errLog
                Response.Redirect("LockBoxErrorForm.aspx", False)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub SelectAll()
        'Function added by ashutosh on 28-June-06
        Dim chkFlag As System.Web.UI.WebControls.CheckBox
        Dim dgItem As DataGridItem
        Dim strSessionId As String
        Dim strCurrentSessionId As String
        Dim i As Integer = 0
        Dim _decimal_TotalAmount As Decimal = 0
        Dim _decimal_Amount As Decimal = 0
        Try

        If DataGridLockBoxReceipts.Items.Count > 0 Then
            If ButtonSelectAll.Text = "Select All" Then
                For Each dgItem In DataGridLockBoxReceipts.Items
                    chkFlag = dgItem.FindControl("chkFlag")
                    If chkFlag.Enabled Then
                        chkFlag.Checked = True
                    End If

                    _decimal_Amount = Convert.ToDecimal(Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text())
                    _decimal_TotalAmount = _decimal_TotalAmount + _decimal_Amount
                    i = i + 1
                Next
                TextBoxGeneral.Text = String.Format("{0:N}", _decimal_TotalAmount)
                ButtonSelectAll.Text = "Select None"
            Else
                For Each dgItem In DataGridLockBoxReceipts.Items
                    chkFlag = dgItem.FindControl("chkFlag")
                    If chkFlag.Enabled Then
                        chkFlag.Checked = False
                    End If
                Next
                TextBoxGeneral.Text = 0.0
                ButtonSelectAll.Text = "Select All"
            End If

            End If

        Catch
            Throw
        End Try
    End Sub
    Function SetTotalPostingAmount()
        Dim dgItem As DataGridItem
        Dim _decimal_Amount As Decimal
        Dim _decimal_TotalAmount As Decimal

        Dim i As Integer

        Dim chkFlag As System.Web.UI.WebControls.CheckBox
        Try
            _decimal_TotalAmount = 0
            For Each dgItem In DataGridLockBoxReceipts.Items
                chkFlag = dgItem.FindControl("chkFlag")
                If chkFlag.Checked Then
                    _decimal_Amount = Convert.ToDecimal(Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text())
                    _decimal_TotalAmount = _decimal_TotalAmount + _decimal_Amount
                End If
                i = i + 1
            Next
            TextBoxGeneral.Text = String.Format("{0:N}", _decimal_TotalAmount)

        Catch
            Throw
        End Try
    End Function
    Function CreateTableForExecl() As DataTable
        Dim dtNew As New DataTable
        Dim dc As DataColumn
        dc = New DataColumn("RecDate")

        dtNew.Columns.Add(dc)
        dc = New DataColumn("PayDate")

        dtNew.Columns.Add(dc)
        dc = New DataColumn("RefInfo")

        dtNew.Columns.Add(dc)
        dc = New DataColumn("Amount")

        dtNew.Columns.Add(dc)
        dc = New DataColumn("ABANum")

        dtNew.Columns.Add(dc)
        dc = New DataColumn("AcctNum")

        dtNew.Columns.Add(dc)

        Return dtNew

    End Function
    Sub ClearSessionMemory()
        Session("State") = Nothing
        Session("session_tempTable") = Nothing
        Session("FileName") = Nothing
    End Sub
    Function CreateCSVFile(ByVal dTableCsv As DataTable, ByVal strFileNamePath As String, ByVal columnHeadings As Boolean) As Boolean
        Dim sb As StringBuilder
        Dim sw As StreamWriter
        Try
            If dTableCsv.Rows.Count > 0 Then
                sb = New StringBuilder
                sw = File.CreateText(strFileNamePath)
                If columnHeadings Then
                    For Each col As DataColumn In dTableCsv.Columns

                        sb.Append(Chr(34))
                        sb.Append(col.ColumnName)
                        sb.Append(Chr(34))
                        sb.Append(",")
                    Next

                    sb.Replace(",", "", sb.Length - 1, 1)
                    sw.WriteLine(sb.ToString())
                End If
                sb.Length = 0
                For Each row As DataRow In dTableCsv.Rows
                    Dim arr() As Object = row.ItemArray()
                    For i As Integer = 0 To arr.Length - 1
                        sb.Append(arr(i).ToString)
                        sb.Append(",")
                    Next
                    sb.Replace(",", "", sb.Length - 1, 1)
                    sw.WriteLine(sb.ToString())
                    sb.Length = 0
                Next
                sw.Flush()
                sw.Close()
            End If

            sw = Nothing
            sb = Nothing

            Return True
        Catch
            sw.Flush()
            sw.Close()

            sw = Nothing
            sb = Nothing

            Throw
        End Try
    End Function
    Private Function GenerateTextFile() As String
        Try
            Dim l_String_FileCreateDate As String
            Dim l_string_Tmp As String
            Dim l_Date_FileName As Date
            l_Date_FileName = Now

            l_String_FileCreateDate = "_"
            l_string_Tmp = CType(l_Date_FileName.Year, String).Trim()
            l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
            l_string_Tmp = CType(l_Date_FileName.Month, String).Trim()
            l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
            l_string_Tmp = CType(l_Date_FileName.Day, String).Trim()
            l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp + "_"

            l_string_Tmp = CType(l_Date_FileName.Hour, String).Trim()
            l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

            l_string_Tmp = CType(l_Date_FileName.Minute, String).Trim()
            l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

            'l_string_Tmp = CType(l_Date_FileName.Second, String).Trim()
            'l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            'l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

            Return l_String_FileCreateDate

        Catch ex As Exception
            Throw

        End Try
    End Function
    Private Function CopyToServer(ByVal l_SourceFolder As String, ByVal l_sourceFile As String, ByVal l_file_LBOX As String) As Boolean
        Try
            Dim SourceFolder As DataColumn = New DataColumn("SourceFolder", System.Type.GetType("System.String"))
            Dim SourceFile As DataColumn = New DataColumn("SourceFile", System.Type.GetType("System.String"))
            Dim DestFolder As DataColumn = New DataColumn("DestFolder", System.Type.GetType("System.String"))
            Dim DestFile As DataColumn = New DataColumn("DestFile", System.Type.GetType("System.String"))
            Dim l_temprow As DataRow
            Dim l_dataset As DataSet
            Dim l_DataRow As DataRow

            Dim l_datatable_FileList As New DataTable
            Dim popupscript As String


            l_dataset = YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.getMetaOutputFileType()

            If l_dataset Is Nothing Then
                MessageBox.Show(PlaceHolderLockBox, "LockBox File", "Unable to find LockBox  Values in the MetaOutput file.", MessageBoxButtons.Stop)

                Exit Function
            End If

            l_DataRow = l_dataset.Tables(0).Rows(0)

            If l_DataRow Is Nothing Then
                MessageBox.Show(PlaceHolderLockBox, "LockBox  File", "Unable to find LockBox  Configuration Values in the MetaOutputfile.", MessageBoxButtons.Stop)

                Exit Function
            End If

            l_datatable_FileList.Columns.Add(SourceFolder)
            l_datatable_FileList.Columns.Add(SourceFile)
            l_datatable_FileList.Columns.Add(DestFolder)
            l_datatable_FileList.Columns.Add(DestFile)

            l_temprow = l_datatable_FileList.NewRow
            l_temprow("SourceFolder") = l_SourceFolder
            l_temprow("SourceFile") = l_sourceFile

            l_temprow("DestFolder") = Convert.ToString(l_DataRow("OutputDirectory"))
            l_temprow("DestFile") = Convert.ToString(l_DataRow("OutputDirectory")) + "\" + l_file_LBOX
            l_datatable_FileList.Rows.Add(l_temprow)
            viewstate("DestFile") = l_temprow("DestFile").ToString()
            Session("FTFileList") = l_datatable_FileList

            popupscript = "<script language='javascript'>" & _
            "window.open('FT\\CopyFilestoFileServer.aspx', 'ReceiptPopUp', " & _
            "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupscript)
            End If
        Catch ex As Exception
            Throw
        End Try
        Return True
    End Function
    Private Function PostedFile(ByVal dtPostData As DataTable) As Boolean
        Try
            'Dim dtTemp As DataTable
            Dim intCount As Integer
            ' Dim dtPostData As DataTable
            YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.UpdateProcessLog(Session("FileName"))
            'dtTemp = Session("dtLockBoxExportClone")
            ' dtPostData = CType(Session("dtPostData"), DataTable)
            For i As Integer = 0 To dtPostData.Rows.Count - 1
                YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.InsertRecieptLockBox(dtPostData.Rows(i)("YmcaId"), _
                dtPostData.Rows(i)("Code"), dtPostData.Rows(i)("Amount"), dtPostData.Rows(i)("RecDate"), _
                dtPostData.Rows(i)("RefInfo"), dtPostData.Rows(i)("PayDate"), dtPostData.Rows(i)("Comments"))
                ' i = i + 1
            Next
            DataGridLockBoxReceipts.DataSource = Nothing
            DataGridLockBoxReceipts.DataBind()
            DataGridLockBoxReceipts.DataSource = dtPostData.Clone()
            DataGridLockBoxReceipts.DataBind()

            ClearSessionMemory()
            ' Session("FileCopied") = Nothing
            'Session("dtPostData") = Nothing
            DataGridLockBoxReceipts.DataSource = Nothing
            ' Session("dtLockBoxExportClone") = Nothing
            ' viewstate("DestFile") = Nothing
            'dtTemp = Nothing
            dtPostData = Nothing

            Return True
        Catch
            Throw
        End Try
    End Function

    'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonFileSelect.Enabled = False
            ButtonFileSelect.ToolTip = toolTip
            ButtonPost.Enabled = False
            ButtonPost.ToolTip = toolTip
        End If
    End Sub
    'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

#End Region

#Region "Garbage"
    'Public Function FormatCurrency(ByVal paramNumber As Double) As String
    '    Try
    '        Dim n As String
    '        Dim m As String()
    '        Dim myNum As String

    '        Dim len As Integer
    '        Dim i As Integer
    '        Dim val As String
    '        If paramNumber = 0 Then
    '            val = 0
    '        Else
    '            n = paramNumber.ToString()
    '            m = (Math.Round(n * 100) / 100).ToString().Split(".")
    '            myNum = m(0).ToString()
    '            len = myNum.Length
    '            Dim fmat(len) As String
    '            For i = 0 To len - 1
    '                fmat(i) = myNum.Chars(i)
    '            Next
    '            Array.Reverse(fmat)
    '            For i = 1 To len - 1
    '                If i Mod 3 = 0 Then
    '                    fmat(i + 1) = fmat(i + 1) & ","
    '                End If
    '            Next
    '            Array.Reverse(fmat)

    '            If m.Length = 1 Then
    '                val = String.Join("", fmat) + ".00"
    '            Else
    '                val = String.Join("", fmat) + "." + m(1)
    '            End If

    '        End If

    '        Return val

    '    Catch ex As Exception
    '        Return paramNumber
    '    End Try

    'End Function
    'ashu Dump
    Sub Dump()
        'Public Shared Sub exportToExcel(ByVal source As DataSet, ByVal fileName As String)
        '    Dim excelDoc As System.IO.StreamWriter
        '    excelDoc = New System.IO.StreamWriter(fileName)
        'Const startExcelXML As String = ("<xml version><Workbook " + ("xmlns=\""urn:schemas-microsoft-com:office:spreadsheet\"""& vbCrLf + (" xmlns:o=\""urn:schemas-microsoft-com:office:office\"""& vbCrLf&" " + ("xmlns:x=\""urn:schemas-    microsoft-com:office:" + ("excel\"""& vbCrLf&" xmlns:ss=\""urn:schemas-microsoft-com:" + ("office:spreadsheet\"">"& vbCrLf&" <Styles>"& vbCrLf&" " + ("<Style ss:ID=\""Default\"" ss:Name=\""Normal\"">"& vbCrLf&" " + ("<Alignment ss:Vertical=\""Bottom\""/>"& vbCrLf&" <Borders/>" + (""& vbCrLf&" <Font/>"& vbCrLf&" <Interior/>"& vbCrLf&" <NumberFormat/>" + (""& vbCrLf&" <Protection/>"& vbCrLf&" </Style>"& vbCrLf&" " + ("<Style ss:ID=\""BoldColumn\"">"& vbCrLf&" <Font " + ("x:Family=\""Swiss\"" ss:Bold=\""1\""/>"& vbCrLf&" </Style>"& vbCrLf&" " + ("<Style     ss:ID=\""StringLiteral\"">"& vbCrLf&" <NumberFormat" + (" ss:Format=\""@\""/>"& vbCrLf&" </Style>"& vbCrLf&" <Style " + ("ss:ID=\""Decimal\"">"& vbCrLf&" <NumberFormat " + ("ss:Format=\""0.0000\""/>"& vbCrLf&" </Style>"& vbCrLf&" " + ("<Style ss:ID=\""Integer\"">"& vbCrLf&" <NumberFormat " + ("ss:Format=\""0\""/>"& vbCrLf&" </Style>"& vbCrLf&" <Style " + ("ss:ID=\""DateLiteral\"">"& vbCrLf&" <NumberFormat " + ("ss:Format=\""mm/dd/yyyy;@\""/>"& vbCrLf&" </Style>"& vbCrLf&" " + "</Styles>"& vbCrLf&" "))))))))))))))))))))
        '    Const endExcelXML As String = "</Workbook>"
        '    Dim rowCount As Integer = 0
        '    Dim sheetCount As Integer = 1
        '    excelDoc.Write(startExcelXML)
        '    excelDoc.Write(("<Worksheet ss:Name=\""Sheet" _
        '                    + (sheetCount + "\"">")))
        '    excelDoc.Write("<Table>")
        '    excelDoc.Write("<Row>")
        '    Dim x As Integer = 0
        '    Do While (x < source.Tables(0).Columns.Count)
        '        excelDoc.Write("<Cell ss:StyleID=\""BoldColumn\""><Data ss:Type=\""String\"">")
        '        excelDoc.Write(source.Tables(0).Columns(x).ColumnName)
        '        excelDoc.Write("</Data></Cell>")
        '        x = (x + 1)
        '    Loop
        '    excelDoc.Write("</Row>")
        '    For Each x As DataRow In source.Tables(0).Rows
        '        rowCount = (rowCount + 1)
        '        'if the number of rows is > 64000 create a new page to continue output
        '        If (rowCount = 64000) Then
        '            rowCount = 0
        '            sheetCount = (sheetCount + 1)
        '            excelDoc.Write("</Table>")
        '            excelDoc.Write(" </Worksheet>")
        '            excelDoc.Write(("<Worksheet ss:Name=\""Sheet" _
        '                            + (sheetCount + "\"">")))
        '            excelDoc.Write("<Table>")
        '        End If
        '        excelDoc.Write("<Row>")
        '        'ID=" + rowCount + "
        '        Dim y As Integer = 0
        '        Do While (y < source.Tables(0).Columns.Count)
        '            Dim rowType As System.Type
        '            rowType = x(y).GetType
        '            Select Case (rowType.ToString)
        '                Case "System.String"
        '                    Dim XMLstring As String = x(y).ToString
        '                    XMLstring = XMLstring.Trim
        '                    XMLstring = XMLstring.Replace("&", "&")
        '                    XMLstring = XMLstring.Replace(">", ">")
        '                    XMLstring = XMLstring.Replace("<", "<")
        '                    excelDoc.Write(("<Cell ss:StyleID=\""StringLiteral\"">" + "<Data ss:Type=\""String\"">"))
        '                    excelDoc.Write(XMLstring)
        '                    excelDoc.Write("</Data></Cell>")
        '                Case "System.DateTime"
        '                    'Excel has a specific Date Format of YYYY-MM-DD followed by  
        '                    'the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
        '                    'The Following Code puts the date stored in XMLDate 
        '                    'to the format above
        '                    Dim XMLDate As DateTime = CType(x(y), DateTime)
        '                    Dim XMLDatetoString As String = ""
        '                    'Excel Converted Date
        '                    XMLDatetoString = (XMLDate.Year.ToString + ("-" _
        '                                + (XMLDate.Month < 10)))
        '                    excelDoc.Write(("<Cell ss:StyleID=\""DateLiteral\"">" + "<Data ss:Type=\""DateTime\"">"))
        '                    excelDoc.Write(XMLDatetoString)
        '                    excelDoc.Write("</Data></Cell>")
        '                Case "System.Boolean"
        '                    excelDoc.Write(("<Cell ss:StyleID=\""StringLiteral\"">" + "<Data ss:Type=\""String\"">"))
        '                    excelDoc.Write(x(y).ToString)
        '                    excelDoc.Write("</Data></Cell>")
        '                Case "System.Int16", "System.Int32", "System.Int64", "System.Byte"
        '                    excelDoc.Write(("<Cell ss:StyleID=\""Integer\"">" + "<Data ss:Type=\""Number\"">"))
        '                    excelDoc.Write(x(y).ToString)
        '                    excelDoc.Write("</Data></Cell>")
        '                Case "System.Decimal", "System.Double"
        '                    excelDoc.Write(("<Cell ss:StyleID=\""Decimal\"">" + "<Data ss:Type=\""Number\"">"))
        '                    excelDoc.Write(x(y).ToString)
        '                    excelDoc.Write("</Data></Cell>")
        '                Case "System.DBNull"
        '                    excelDoc.Write(("<Cell ss:StyleID=\""StringLiteral\"">" + "<Data ss:Type=\""String\"">"))
        '                    excelDoc.Write("")
        '                    excelDoc.Write("</Data></Cell>")
        '                Case Else
        '                    Throw New Exception((rowType.ToString + " not handled."))
        '            End Select
        '            'TODO: Warning!!!, inline IF is not supported ?
        '            y = (y + 1)
        '        Loop
        '        excelDoc.Write("</Row>")
        '    Next
        '    excelDoc.Write("</Table>")
        '    excelDoc.Write(" </Worksheet>")
        '    excelDoc.Write(endExcelXML)
        '    excelDoc.Close()

        'If Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text().ToUpper = "&NBSP;" Then
        '    ' YMCARET.YmcaBusinessObject.RecieptsLockBoxImport.InsertRecieptLockBox(Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Code).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.PayDate).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RefInfo).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RecDate).Text(), Me.DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Comments).Text())
        '    drNewRow = dtNotValidData.NewRow()
        '    drNewRow("YMCANo") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YMCANo).Text()
        '    drNewRow("Name") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Name).Text()
        '    drNewRow("Source") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Source).Text()
        '    drNewRow("RefInfo") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RefInfo).Text()
        '    drNewRow("PayDate") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.PayDate).Text()
        '    drNewRow("RecDate") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.RecDate).Text()
        '    drNewRow("Amount") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Amount).Text()
        '    drNewRow("Code") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Code).Text()
        '    drNewRow("YmcaId") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.YmcaId).Text()
        '    drNewRow("Comments") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.Comments).Text()
        '    drNewRow("ABANum") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.ABANum).Text()
        '    drNewRow("AcctNum") = DataGridLockBoxReceipts.Items(i).Cells(DgLockBox.AcctNum).Text()
        '    dtNotValidData.Rows.Add(drNewRow)

        'End If
        'End Sub
    End Sub
#End Region


End Class
