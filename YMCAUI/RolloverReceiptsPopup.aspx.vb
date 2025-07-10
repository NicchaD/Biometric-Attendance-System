' Cache-Session     : Hafiz 04Feb06
'Change History::
'****************************************************
'Modification History
'*******************************************************************************
'Modified by              Date              Description
'*******************************************************************************
' Amit Kumar Nigam       Dec 11th 2008      Added the Validations on check Recd Date and check date                                             
' Priya                 24-April-2009       YRS 5.0-738 Please use Account date instead of Calender date for roll in checks validation.
' Dilip Yadav           13-Aug-2009         Changed the spelling of 'recieve' to 'receive'
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya                  19-Oct-2010         BT:657,YRS 5.0-1185 : Allow input into nonTaxable field - mnyPersPostTax (Made nontaxable textbox readonly="False")
'Priya					17-05-2012			BT-1028,YRS 5.0-1577: Add new field to atsRollovers
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Public Class RolloverReceiptsPopup
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("Rollovers.aspx?Name=Receipts")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFirst As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLast As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInstitutionName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDateReceived As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCheckReceivedDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCheckNumber As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCheckDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxableAmount As System.Web.UI.WebControls.Label
    Protected WithEvents Label1NonTaxableAmount As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCheckTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFirst As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxlast As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxInstitution As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxDateReceived As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxheckReceivedDate As YMCAUI.DateUserControl
    Protected WithEvents TextboxCheckNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxCheckDate As YMCAUI.DateUserControl
    Protected WithEvents TextboxTaxableAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxableAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxCheckTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button

    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator6 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
	'BT-1028,YRS 5.0-1577: Add new field to atsRollovers
	Protected WithEvents TextboxInfoSource As System.Web.UI.WebControls.TextBox
	Protected WithEvents LabelInfoSource As System.Web.UI.WebControls.Label
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
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.TextboxNonTaxableAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        Me.TextboxNonTaxableAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        Me.TextboxNonTaxableAmount.Attributes.Add("onblur", "Javascript:return  _OnBlur_NonTaxableAmount();")
        ''Me.TextboxNonTaxableAmount.Attributes.Add("onfocus", "Javascript:return  _OnFocus_NonTaxableAmount();")
        ''Me.TextboxTaxableAmount.Attributes.Add("onfocus", "Javascript:return  _OnFocus_TaxableAmount();")
        ''Me.TextboxheckReceivedDate.Attributes.Add("onblur", "Javascript:return  _OnBlur_TextboxheckReceivedDate();")

        Me.TextboxTaxableAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        Me.TextboxTaxableAmount.Attributes.Add("onblur", "Javascript:return  _OnBlur_TaxableAmount();")
        Me.TextboxTaxableAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        Me.TextboxCheckTotal.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        Me.TextboxCheckNo.Attributes.Add("onblur", "Javascript:return  _OnBlur_TextboxCheckNo();")
        Me.TextboxheckReceivedDate.RequiredDate = True
        Me.TextboxCheckDate.RequiredDate = True

        Dim l_string_ymcapk As String
        Dim l_string_empeventpk As String
        Dim l_string_fundeventpk As String
        Dim l_string_RolloverId As String
        Dim l_string_PersId As String
        Try
            l_string_ymcapk = Request.QueryString.Get("YmcaId")
            l_string_fundeventpk = Request.QueryString.Get("FundEventId")
            l_string_empeventpk = Request.QueryString.Get("EmpEvent")
            l_string_RolloverId = Request.QueryString.Get("RolloverId")
            l_string_PersId = Session("PersId")
            Dim l_dataset_RollOverInfo As DataSet
            If Not IsPostBack Then
                TextboxTaxableAmount.Text = "0.00"
                TextboxNonTaxableAmount.Text = "0.00"
                TextboxCheckTotal.Text = "0.00"
            Else
                'TextboxCheckTotal.Text = Math.Round(Convert.ToDecimal(TextboxTaxableAmount.Text)) + Math.Round(Convert.ToDecimal(TextboxNonTaxableAmount.Text))
            End If
            l_dataset_RollOverInfo = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.GetRolloverData(l_string_PersId, l_string_RolloverId)
            If l_dataset_RollOverInfo.Tables("RolloverInfo").Rows.Count > 0 Then
                'If l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("SSNo").ToString() <> "System.DBNull" And l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("SSNo").ToString() <> "" Then
                TextBoxSSNo.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("SSNo").ToString()
                ' End If


                TextBoxlast.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("LastName").ToString()
                TextBoxFirst.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("FirstName").ToString()
				TextboxInstitution.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("InstitutionName").ToString().Trim()
				'BT-1028,YRS 5.0-1577: Add new field to atsRollovers
				TextboxInfoSource.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("InfoSource").ToString().Trim()
				TextboxDateReceived.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("ReceivedDate").ToString()
                TextboxStatus.Text = l_dataset_RollOverInfo.Tables("RolloverInfo").Rows(0).Item("Status").ToString()

            End If

            If TextboxStatus.Text = "CLOSED" Then
                Dim l_dataset_RolloverRcpts As DataSet
                l_dataset_RolloverRcpts = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.GetRolloverRcptsData(l_string_RolloverId)
                If l_dataset_RolloverRcpts.Tables("RolloverRcptsInfo").Rows.Count > 0 Then
                    TextboxCheckDate.Text = l_dataset_RolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("CheckDate").ToString()
                    TextboxCheckNo.Text = l_dataset_RolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("CheckNum").ToString()
                    TextboxheckReceivedDate.Text = l_dataset_RolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("ReceivedDate").ToString()
                    TextboxTaxableAmount.Text = l_dataset_RolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("Taxable").ToString()
                    TextboxNonTaxableAmount.Text = l_dataset_RolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("NonTaxable").ToString()
                    TextboxCheckTotal.Text = Convert.ToDecimal(TextboxTaxableAmount.Text) + Convert.ToDecimal(TextboxNonTaxableAmount.Text)
                    TextboxCheckDate.Enabled = False
                    TextboxCheckNo.Enabled = False
                    TextboxheckReceivedDate.Enabled = False
                    TextboxTaxableAmount.Enabled = False
                    TextboxNonTaxableAmount.Enabled = False
                    TextboxCheckTotal.Enabled = False
                    TextboxCheckDate.Enabled = False
                    ButtonSave.Enabled = False
                    ButtonCancel.Enabled = False
                    ButtonOk.Enabled = True
                End If


            End If
            ButtonAdd.Enabled = False
            'ButtonCancel.Visible = False

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try




    End Sub

    Private Sub TextboxTaxableAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextboxTaxableAmount.TextChanged
        'Try
        '    If Not TextboxNonTaxableAmount.Text = "" Then
        '        TextboxCheckTotal.Text = Convert.ToDecimal(TextboxTaxableAmount.Text) + Convert.ToDecimal(TextboxNonTaxableAmount.Text)
        '    Else
        '        TextboxCheckTotal.Text = Convert.ToDecimal(TextboxTaxableAmount.Text)
        '    End If
        'Catch ex As Exception
        '    Dim l_String_Exception_Message As String
        '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        'End Try


    End Sub

    Private Sub TextboxNonTaxableAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextboxNonTaxableAmount.TextChanged
        'Try
        '    If Not TextboxTaxableAmount.Text = "" Then
        '        TextboxCheckTotal.Text = Convert.ToDecimal(TextboxTaxableAmount.Text) + Convert.ToDecimal(TextboxNonTaxableAmount.Text)
        '    Else
        '        TextboxCheckTotal.Text = Convert.ToDecimal(TextboxNonTaxableAmount.Text)
        '    End If
        'Catch ex As Exception
        '    Dim l_String_Exception_Message As String
        '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        'End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Dim l_string_ymcapk As String
        Dim l_string_empeventpk As String
        Dim l_string_fundeventpk As String
        Dim l_string_RolloverId As String
        Dim l_string_PersId As String
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940 

            'Dim l_string_input As String
            'Dim l_bool_flg As Boolean
            'l_bool_flg = False
            'l_string_input = Me.TextboxCheckNo.Text
            'Dim i As Integer
            'While i < Len(l_string_input)
            '    If l_string_input.Substring(i, 1) <> l_string_input.Substring(i + 1, 1) Then
            '        l_bool_flg = True
            '    End If
            'End While
            'If Not (l_bool_flg) Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "All the characters are same", MessageBoxButtons.OK, True)
            'End If

            l_string_ymcapk = Request.QueryString.Get("YmcaId")
            l_string_fundeventpk = Request.QueryString.Get("FundEventId")
            l_string_empeventpk = Request.QueryString.Get("EmpEvent")
            l_string_RolloverId = Request.QueryString.Get("RolloverId")
            l_string_PersId = Session("PersId")
            Dim l_datetime_CompareDate1 As DateTime
            l_datetime_CompareDate1 = System.DateTime.Today.AddDays(-182)
            '*******************************************Validations Start - Amit Kumar Nigam
            'Priya 24-April-2009 YRS 5.0-738
            'Dim l_datetime_firstdate As Date = DateAdd(DateInterval.Month, DateDiff(DateInterval.Month, Date.MinValue, Today()), Date.MinValue)
            Dim dateAccountDate As Date
            dateAccountDate = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.GetAccountDate().ToString().Trim()


            Dim l_datetime_firstdate As Date = DateAdd(DateInterval.Month, DateDiff(DateInterval.Month, Date.MinValue, dateAccountDate), Date.MinValue)
            'End 24-April-2009
            '*******************************************Validations End - Amit Kumar Nigam
            Dim l_flag_DateErr As Boolean
            Dim l_string_Msg As String
            l_flag_DateErr = False

            If (System.DateTime.Compare(Convert.ToDateTime(TextboxCheckDate.Text), System.DateTime.Today()) = 1) Then
                l_flag_DateErr = True
                l_string_Msg = "Check date cannot be greater than Today's date."
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(TextboxheckReceivedDate.Text), System.DateTime.Today()) = 1) Then
                l_flag_DateErr = True
                l_string_Msg = "Check received date cannot be greater than Today's date."
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(TextboxheckReceivedDate.Text), l_datetime_CompareDate1) <> 1) Then
                l_flag_DateErr = True
                l_string_Msg = "Check received date is atleast six months old."
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(TextboxCheckDate.Text), l_datetime_CompareDate1) <> 1) Then
                l_flag_DateErr = True
                l_string_Msg = "Check date is atleast six months old."
                '*******************************************Validations start - Amit Kumar Nigam
            ElseIf (Convert.ToDateTime(TextboxheckReceivedDate.Text) < Convert.ToDateTime(TextboxDateReceived.Text)) Then
                l_flag_DateErr = True
                l_string_Msg = "Check Received Date is earlier than Received Date."
            ElseIf (Convert.ToDateTime(TextboxheckReceivedDate.Text) < Convert.ToDateTime(l_datetime_firstdate)) Then
                l_flag_DateErr = True
                'Priya 24-April-2009 YRS 5.0-738 Cahnge spelling recieved to received
                'l_string_Msg = "Check Recieved Date must not be earlier than First Day of the current Month."
                l_string_Msg = "Check Received Date must not be earlier than First Day of the current Month."
                'End YRS 5.0-738
            ElseIf (Convert.ToDateTime(TextboxCheckDate.Text) > Convert.ToDateTime(TextboxheckReceivedDate.Text)) Then
                l_flag_DateErr = True
                l_string_Msg = "Check  Date must be less than or equal to Check Received Date."
                'ElseIf (Convert.ToDateTime(TextboxCheckDate.Text) < Convert.ToDateTime(l_datetime_firstdate)) Then
                '    l_flag_DateErr = True
                '    l_string_Msg = "Check  Date must not be earlier than  First Day of the current Month."
            End If
            '*******************************************************************************Validations End  - Amit Kumar Nigam


            If (l_flag_DateErr) Then
                'last parameter changed to false by Anita on 01-06-2007
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Msg, MessageBoxButtons.OK, False)

            Else



                Dim l_string_output As String
                l_string_output = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.AddRolloverData(l_string_PersId, l_string_fundeventpk, l_string_ymcapk, l_string_RolloverId, TextboxCheckNo.Text.Trim(), TextboxCheckDate.Text.Trim(), TextboxheckReceivedDate.Text.Trim(), TextboxTaxableAmount.Text.Trim(), TextboxNonTaxableAmount.Text.Trim())
                If l_string_output = "0" Then
                    ButtonOk.Enabled = True
                    TextboxStatus.Text = "CLOSED"
                    TextboxCheckDate.Enabled = False
                    TextboxCheckNo.Enabled = False
                    TextboxheckReceivedDate.Enabled = False
                    TextboxTaxableAmount.Enabled = False
                    TextboxNonTaxableAmount.Enabled = False
                    TextboxCheckTotal.Enabled = False
                    ButtonSave.Enabled = False
                    ButtonCancel.Enabled = False
                    Session("Closed") = True

                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try


    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        ''' Code added by Vartika on 17th Nov
        Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                                "window.opener.document.forms(0).submit();self.close();" & _
                                                                "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        End If

        ''''Dim closeWindow5 As String = "<script language='javascript'>" & _
        ''''                                      "window.close();window.opener.location.reload();" & _
        ''''                                      "</script>"

        ''''If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
        ''''    Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        ''''End If
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        ''' Code added by Vartika on 17th Nov
        Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                                "window.opener.document.forms(0).submit();self.close();" & _
                                                                "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        End If
        '''Dim closeWindow5 As String = "<script language='javascript'>" & _
        '''                                     "window.close();window.opener.location.reload();" & _
        '''                                     "</script>"

        '''If (Not Me.IsStartupScriptRegistered("CloseWindow1")) Then
        '''    Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        '''End If
    End Sub
End Class
