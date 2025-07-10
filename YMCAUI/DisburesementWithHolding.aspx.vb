'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	DisbursementsReissue.aspx.vb
'*******************************************************************************
' Cache-Session     :   Imran 2009
'*******************************************************************************
'** DATE                AUTHOR            DESCRIPTION           
'*******************************************************************************
'12/Nov/2009            Neeraj Singh      Added form name for security issue YRS 5.0-940 
'** 03/12/2009          Imran             Validation for Adjusted amount is less than the amount to be adjusted
'**15/12/2009           Imran             For BT 1068
'**18/Feb/2010          Imran             BT-1068 For showing error  with saving blank values
'**28-June-2010         Imran             Chenges for Enhancements 
'**2015.09.16           Manthan Rajguru   YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Imports System.Math
Imports System.Text.RegularExpressions

Public Class DisburesementWithHolding
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DisburesementWithHolding.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TabStripBreakDown As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageBreakDown As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelGross As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGross As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelWithheld As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxWithheld As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderMsg As System.Web.UI.WebControls.PlaceHolder


    Protected WithEvents LabelPreTax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPostTax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYMCAPreTax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxablePrinciple As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxablePrinciple As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxWithheldPrinciple As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxableInterest As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxWithheldInterest As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFactor As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPreTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPostTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYMCAPreTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxablePrinciple As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxNonTaxablePrinciple As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxWithheldPrinciple As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxableInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxWithheldInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFactor As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUpdateDisbursements As System.Web.UI.WebControls.Button

    Protected WithEvents DataGridTransacts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridNewTransacts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridDisbursementDetails As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgDisbursements As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridWithHoldTransacts As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_dataset_Breakdown As New DataSet
    Dim g_dataset_DisbursementDetails As New DataSet
    Public Shared selected_Disbursement As String
    Public Shared selected_Disbursement_Number As String
    Public Shared selected_Disbursement_AccType As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim l_string_DisbId As String
        Dim l_string_PersId As String
        Dim l_string_FundEventId As String
        Dim l_string_WithholdingAmount As String
        Dim l_string_GrossAmount As String
        Dim l_string_NewStatus As String
        Dim l_double_NetAmount As Double
        Dim l_string_DisbIds As String
        Dim Strarr As Array
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not IsPostBack Then
                TextBoxFactor.Text = "20"
                'If Not Request.QueryString.Get("DisbId") = "" Then
                '    l_string_DisbId = Request.QueryString.Get("DisbId").ToString()
                'End If

                If Not Request.QueryString.Get("DisbId") = "" Then
                    Strarr = Request.QueryString.Get("DisbId").ToString().Split(",")
                    PopulateDisbursementGrid(Strarr)
                End If
                If Not Session("IdForReversal") = "" Then
                    l_string_DisbIds = Session("IdForReversal")
                End If
                If Not Request.QueryString.Get("PersId") = "" Then
                    l_string_PersId = Request.QueryString.Get("PersId").ToString()
                End If
                If Not Request.QueryString.Get("FundId") = "" Then
                    l_string_FundEventId = Request.QueryString.Get("FundId").ToString()
                Else
                    l_string_FundEventId = ""
                End If
                If Not Request.QueryString.Get("WHAmount") = "" Then
                    l_string_WithholdingAmount = Request.QueryString.Get("WHAmount").ToString()
                End If
                If Not Request.QueryString.Get("Gross") = "" Then
                    l_string_GrossAmount = Request.QueryString.Get("Gross").ToString()
                End If
                If Not Request.QueryString.Get("Status") = "" Then
                    l_string_NewStatus = Request.QueryString.Get("Status").ToString()
                End If

               
                ''Me.TabStripBreakDown.Items(1).Enabled = False
                'g_dataset_Breakdown = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.LookUpTransacts(l_string_PersId, l_string_FundEventId)
                'If (g_dataset_Breakdown.Tables("Transacts").Rows.Count > 0) Then
                '    Me.DataGridTransacts.DataSource = g_dataset_Breakdown.Tables("Transacts")
                '    Session("dsTransacts") = g_dataset_Breakdown
                '    Me.DataGridTransacts.DataBind()
                '    'PopulateNewTransacts()
                '    'Me.TextBoxGross.Text = l_string_GrossAmount
                '    Me.TextBoxWithheld.Text = l_string_WithholdingAmount
                '    Me.TextBoxNet.Text = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                'Else
                '    ButtonSave.Enabled = False
                '    Me.ButtonUpdateDisbursements.Enabled = False
                '    Me.ButtonUpdate.Enabled = False
                '    ' Me.TextBoxGross.Text = l_string_GrossAmount
                '    Me.TextBoxWithheld.Text = l_string_WithholdingAmount
                '    Me.TextBoxNet.Text = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                'End If
            End If

            'If (Session("CurIndex") = -1) Then
            '    Me.TextBoxPostTax.Text = "0.00"
            '    Me.TextBoxPreTax.Text = "0.00"
            '    Me.TextBoxYMCAPreTax.Text = "0.00"
            'End If
            'If Not Request.QueryString.Get("Status") = "" Then
            '    l_string_NewStatus = Request.QueryString.Get("Status").ToString()
            'End If
            'If (l_string_NewStatus <> "REPLACE") Then
            '    TextBoxNet.Visible = False
            '    TextBoxWithheld.Visible = False
            '    LabelNet.Visible = False
            '    LabelWithheld.Visible = False
            'End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Public Sub PopulateDisbursementGrid(ByVal parameterListDisbId As Array)
        Dim l_string_PersId As String
        Dim l_integer_GridIndex As Integer
        Dim l_string_Activity As String
        Dim l_integer_AnnuityOnly As Integer
        Dim l_datarow_CurrentRow As DataRow
        'Aparna Samala -22/09/2006
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        Dim g_dataset_dsDisbursementsByPersId As DataSet
        Dim i As Integer
        Dim strOutPut As String
        Dim l_GrossAmount As Decimal
        Dim dsDisbursements As DataSet
        Try
            If Not Session("dsDisbursementsByPersId") Is Nothing Then

                g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                'If parameterListDisbId.Length >= 2 Then
                '    For i = 1 To parameterListDisbId.Length - 1
                '        strOutPut += "','" + parameterListDisbId.GetValue(i)

                '    Next
                'End If
                'strOutPut = strOutPut.Substring(3, strOutPut.Length - 3)

                If parameterListDisbId.Length >= 2 Then
                    For i = 1 To parameterListDisbId.Length - 1
                        strOutPut += "," + parameterListDisbId.GetValue(i)

                    Next
                End If

                strOutPut = strOutPut.Substring(1, strOutPut.Length - 1)

                l_searchExpr = "DisbursementID in  ('" + strOutPut + "')"
                'l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)

                dsDisbursements = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementWithHoldingInfo(strOutPut)
                If Not dsDisbursements Is Nothing Then
                    dgDisbursements.DataSource = dsDisbursements
                    dgDisbursements.DataBind()
                End If

                If dgDisbursements.Items.Count = 0 Then
                    Session("Called") = True
                    Dim closeWindow4 As String = "<script language='javascript'>" & _
                            "self.close();window.opener.document.forms(0).submit();" & _
                            "</script>"

                    If (Not Me.IsStartupScriptRegistered("CloseWindow4")) Then
                        Page.RegisterStartupScript("CloseWindow4", closeWindow4)
                    End If
                End If


                'If l_dr_CurrentRow.Length > 0 Then

                '    'Dim l_disbursementLoanHeading As DataTable
                '    'l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Tables(0).DefaultView.RowFilter = l_searchExpr
                '    'l_disbursementLoanHeading.TableName = "l_disbursementHeading"
                '    'l_disbursementLoanHeading.Rows.Clear()

                '    'For Each dr As DataRow In l_dr_CurrentRow
                '    '    l_disbursementLoanHeading.Rows.Add(dr)
                '    'Next

                '    g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").DefaultView.RowFilter = l_searchExpr
                '    dgDisbursements.DataSource = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").DefaultView
                '    dgDisbursements.DataBind()



                '    'dgDisbursements.DataSource = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").DefaultView.Table().Select(l_searchExpr)
                '    'dgDisbursements.DataBind()
                '    For i = 0 To dgDisbursements.Items.Count - 1
                '        l_GrossAmount += dgDisbursements.Items(i).Cells(3).Text

                '    Next
                '    Me.TextBoxGross.Text = l_GrossAmount.ToString()
                'End If




            End If
        Catch ex As Exception
            Throw ex
        End Try




    End Sub
    Public Sub PopulateNewTransacts(ByVal DisbId As String)
        Dim l_integer_index As Integer
        Dim l_dataset_disbDetail As New DataSet
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        Try
            l_dataset_disbDetail = Session("DisbursementDetails")

            'Me.DataGridNewTransacts.DataSource = l_dataset_disbDetail
            'Me.DataGridNewTransacts.DataBind()

            l_searchExpr = "DisbursementID ='" + DisbId + "'"
            l_dr_CurrentRow = l_dataset_disbDetail.Tables("DisbursementDetails").Select(l_searchExpr)
            If l_dr_CurrentRow.Length > 0 Then


                l_dataset_disbDetail.Tables("DisbursementDetails").DefaultView.RowFilter = l_searchExpr
                DataGridNewTransacts.DataSource = l_dataset_disbDetail.Tables("DisbursementDetails").DefaultView
                DataGridNewTransacts.DataBind()

                'For i = 0 To dgDisbursements.Items.Count - 1
                '    l_GrossAmount += dgDisbursements.Items(i).Cells(3).Text

                'Next
                'Me.TextBoxGross.Text = l_GrossAmount.ToString()
            End If


           

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try


    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        selected_Disbursement = ""
        selected_Disbursement_Number = ""
        selected_Disbursement_AccType = ""
        Session("DisbursementDetails") = Nothing
        Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                     "window.close()" & _
                                                     "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        End If
    End Sub

    Private Sub DataGridTransacts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridTransacts.ItemDataBound
        Try
            e.Item.Cells(1).Visible = False
            e.Item.Cells(2).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridNewTransacts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridNewTransacts.ItemDataBound
        Try
            e.Item.Cells(1).Visible = False
            'Added on 1/10/2009
            e.Item.Cells(4).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridWithHoldTransacts_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridWithHoldTransacts.ItemCommand


        Try
            TextBoxTaxablePrinciple.Text = e.Item.Cells(5).Text
            TextBoxNonTaxablePrinciple.Text = e.Item.Cells(7).Text
            TextBoxTaxWithheldPrinciple.Text = e.Item.Cells(8).Text
            TextBoxTaxableInterest.Text = e.Item.Cells(6).Text
            TextBoxWithheldInterest.Text = e.Item.Cells(9).Text
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub DataGridNewTransacts_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridNewTransacts.ItemCommand
        Dim l_button_select As ImageButton
        Dim l_button_select2 As ImageButton
        Dim i As Integer
        Try

            TextBoxTaxablePrinciple.Text = e.Item.Cells(5).Text
            TextBoxNonTaxablePrinciple.Text = e.Item.Cells(7).Text
            TextBoxTaxWithheldPrinciple.Text = e.Item.Cells(8).Text
            TextBoxTaxableInterest.Text = e.Item.Cells(6).Text
            TextBoxWithheldInterest.Text = e.Item.Cells(9).Text

            If (e.CommandName = "Select") Then

                l_button_select = e.Item.FindControl("ImageButtonSelect")
                l_button_select.ImageUrl = "images\selected.gif"
                'Added by imran on 1/11/2009
                selected_Disbursement_AccType = e.Item.Cells(3).Text
               
            End If
            While i < Me.DataGridNewTransacts.Items.Count
                l_button_select = New ImageButton
                l_button_select2 = New ImageButton
                If i <> e.Item.ItemIndex Then
                    l_button_select = Me.DataGridNewTransacts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\select.gif"
                End If
                i = i + 1
            End While
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub dgDisbursements_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDisbursements.ItemCommand
        Dim i As Integer
        Dim l_button_select As ImageButton
        Dim l_button_select2 As ImageButton
        selected_Disbursement = ""
        selected_Disbursement_Number = ""
        selected_Disbursement_AccType = ""
        Try


            If (e.CommandName = "Select") Then

                l_button_select = e.Item.FindControl("imgdisbursements")
                ' If e.Item.ItemIndex = Me.DataGridTransacts.SelectedIndex And Me.DataGridTransacts.SelectedIndex >= 0 Then
                l_button_select.ImageUrl = "images\selected.gif"
                selected_Disbursement = e.Item.Cells(0).Text
                selected_Disbursement_Number = e.Item.Cells(2).Text
                selected_Disbursement_AccType = ""
                'selected_Disbursement_AccType = e.Item.Cells(3).Text
                'End If
            End If
            While i < Me.dgDisbursements.Items.Count
                l_button_select = New ImageButton
                l_button_select2 = New ImageButton
                If i <> e.Item.ItemIndex Then
                    l_button_select = Me.dgDisbursements.Items(i).FindControl("imgdisbursements")
                    l_button_select.ImageUrl = "images\select.gif"
                End If
                i = i + 1
            End While
            If selected_Disbursement <> "" Then
                PopulateNewTransacts(selected_Disbursement)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonUpdateDisbursements_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdateDisbursements.Click
        Dim l_integer_GridIndex As Integer
        Dim l_string_NewStatus As String
        l_string_NewStatus = Convert.ToString(Request.QueryString.Get("Status"))
        Dim i As Integer
        Dim l_dr_CurrentRow As DataRow()
        Dim l_datarow_CurrentRow As DataRow
        Dim AdjAmount As Decimal
        Dim ActualAmount As Decimal

        Dim l_dataset_disbDetail As New DataSet
        Dim l_temprow As DataRow
        Dim TaxablePrincipal As Decimal
        Dim TaxableInterest As Decimal
        Dim NonTaxablePrincipal As Decimal
        Dim l_searchExpr As String
        Dim drSelectedRow As DataRow
        Dim c_rowIndex As Integer
        Dim strchrAcctBreakDownType As String
        Dim l_WithheldPrincipal As Decimal
        Dim l_WithheldInterest As Decimal
        Try

            l_integer_GridIndex = Session("CurIndex")
            g_dataset_Breakdown = Session("DisbursementDetails")

            'Added by imran on 20/10/2009 
            If selected_Disbursement Is Nothing Then
                MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Please select a Disbursement for adjustment.", MessageBoxButtons.OK, True)
                Exit Sub
            Else

                If selected_Disbursement.ToString() = "" Then
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Please select a Disbursement ", MessageBoxButtons.OK, True)
                    Exit Sub

                End If
            End If
            'Added on 15/12/2009 for BT 1068
            If selected_Disbursement_AccType = "" Then
                MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Please select a Disbursement details for adjustments.", MessageBoxButtons.OK, True)
                Exit Sub
            End If

            If TextBoxTaxWithheldPrinciple.Text.Trim() <> "" Then
                If Convert.ToDecimal(IIf(TextBoxTaxWithheldPrinciple.Text.Trim() = "", "0.0", TextBoxTaxWithheldPrinciple.Text.Trim())) > Convert.ToDecimal(IIf(TextBoxTaxablePrinciple.Text.Trim() = "", "0.0", TextBoxTaxablePrinciple.Text.Trim())) Then
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Withheld Principal cannot be greater than Taxable Principle ", MessageBoxButtons.OK, True)
                    Exit Sub
                End If

            End If

            If TextBoxWithheldInterest.Text.Trim() <> "" Then
                If Convert.ToDecimal(IIf(TextBoxWithheldInterest.Text.Trim() = "", "0.0", TextBoxWithheldInterest.Text.Trim())) > Convert.ToDecimal(IIf(TextBoxTaxableInterest.Text.Trim() = "", "0.0", TextBoxTaxableInterest.Text.Trim())) Then
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Withheld Interest cannot be greater than Taxable Interest ", MessageBoxButtons.OK, True)
                    Exit Sub
                End If

            End If

            'Added by imran on 30/10/2009 
            'If Session("CurIndex") = "-1" Then
            'Added on 15/12/2009 for BT 1068
            If selected_Disbursement_AccType = "" Then
                MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Please select a Disbursement details for adjustments.", MessageBoxButtons.OK, True)
                Exit Sub
            End If

            AdjAmount = 0
            'For i = 0 To DataGridNewTransacts.Items.Count - 1
            '    AdjAmount += Convert.ToDecimal(IIf(DataGridNewTransacts.Items(i).Cells(8).Text = "", "0.00", (DataGridNewTransacts.Items(i).Cells(8).Text))) + Convert.ToDecimal(IIf(DataGridNewTransacts.Items(i).Cells(9).Text = "", "0.00", (DataGridNewTransacts.Items(i).Cells(9).Text)))
            'Next
            'AdjAmount += Convert.ToDouble(TextBoxWithheldInterest.Text) + Convert.ToDouble(TextBoxTaxWithheldPrinciple.Text)
            '============================================
            'Added by imran on 3/12/2009


            If Not Session("DisbursementDetails") Is Nothing Then
                l_dataset_disbDetail = Session("DisbursementDetails")
                For i = 0 To l_dataset_disbDetail.Tables("DisbursementDetails").Rows.Count - 1
                    If UCase(selected_Disbursement) = UCase(l_dataset_disbDetail.Tables("DisbursementDetails").Rows(i)("DisbursementID")) And UCase(selected_Disbursement_AccType) = UCase(l_dataset_disbDetail.Tables("DisbursementDetails").Rows(i)("chrAcctType")) Then
                        c_rowIndex = i
                    End If

                Next


                l_searchExpr = "DisbursementID =  '" + selected_Disbursement + "'and chrAcctType='" + selected_Disbursement_AccType + "' "
                l_dr_CurrentRow = l_dataset_disbDetail.Tables("DisbursementDetails").Select(l_searchExpr)

                If l_dr_CurrentRow.Length > 0 Then
                    drSelectedRow = l_dr_CurrentRow(0)

                    'Added on 15/12/2009 for BT 1068
                    l_WithheldPrincipal = IIf(TextBoxTaxWithheldPrinciple.Text.Trim() = "", 0, TextBoxTaxWithheldPrinciple.Text.Trim())
                    l_WithheldInterest = IIf(TextBoxWithheldInterest.Text.Trim() = "", 0, TextBoxWithheldInterest.Text.Trim())


                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).AcceptChanges()
                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).BeginEdit()

                    drSelectedRow.Item("WithheldPrincipal") = l_WithheldPrincipal
                    drSelectedRow.Item("WithheldInterest") = l_WithheldInterest

                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).EndEdit()


                End If



                '=============================================

                l_dr_CurrentRow = l_dataset_disbDetail.Tables("DisbursementDetails").Select("DisbursementID = '" + selected_Disbursement + "'")
                If l_dr_CurrentRow.Length > 0 Then
                    For Each l_datarow_CurrentRow In l_dr_CurrentRow
                        AdjAmount += Convert.ToDouble(IIf(l_datarow_CurrentRow("WithheldPrincipal").ToString() = "", "0", l_datarow_CurrentRow("WithheldPrincipal"))) + Convert.ToDouble(IIf(l_datarow_CurrentRow("WithheldInterest").ToString() = "", "0", l_datarow_CurrentRow("WithheldInterest")))
                    Next

                End If

            End If

            If Not g_dataset_Breakdown Is Nothing Then

                For i = 0 To dgDisbursements.Items.Count - 1

                    If dgDisbursements.Items(i).Cells(0).Text = selected_Disbursement Then


                        ActualAmount = Convert.ToDecimal(IIf(dgDisbursements.Items(i).Cells(3).Text = "", "0.00", (dgDisbursements.Items(i).Cells(3).Text)))

                        'Commented by imran for BT 987
                        If ActualAmount < AdjAmount Then
                            MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Adjusted amount is greater than the amount to be adjusted for selected Disbursement.", MessageBoxButtons.OK, True)
                            Exit Sub
                        Else
                            'dgDisbursements.Items(i).Cells(4).Text = Convert.ToDecimal(IIf(dgDisbursements.Items(i).Cells(4).Text = "", "0.00", (dgDisbursements.Items(i).Cells(4).Text))) + Convert.ToDouble(TextBoxWithheldInterest.Text) + Convert.ToDouble(TextBoxTaxWithheldPrinciple.Text)
                            CreateDisbDetail()
                            ClearTextBox()
                            If selected_Disbursement <> "" Then
                                PopulateNewTransacts(selected_Disbursement)

                                Dim j As Integer
                                AdjAmount = 0
                                For j = 0 To DataGridNewTransacts.Items.Count - 1
                                    AdjAmount += Convert.ToDecimal(IIf(DataGridNewTransacts.Items(j).Cells(8).Text = "", "0.00", (DataGridNewTransacts.Items(j).Cells(8).Text))) + Convert.ToDecimal(IIf(DataGridNewTransacts.Items(j).Cells(9).Text = "", "0.00", (DataGridNewTransacts.Items(j).Cells(9).Text)))
                                Next
                                dgDisbursements.Items(i).Cells(4).Text = AdjAmount
                                dgDisbursements.Items(i).Cells(5).Text = ActualAmount - AdjAmount
                            End If


                        End If

                        'dgDisbursements.Items(i).Cells(4).Text = AdjAmount

                    End If

                Next




                'Added by imran on 30/10/2009 For Unselect Grid Index
                DataGridNewTransacts.SelectedIndex = -1
                ' DataGridTransacts.SelectedIndex = -1
                Session("CurIndex") = -1

                Dim l_button_select2 As ImageButton
                l_button_select2 = CType(DataGridNewTransacts.Items(l_integer_GridIndex).FindControl("ImageButtonSel"), ImageButton)
                If Not l_button_select2 Is Nothing Then
                    l_button_select2.ImageUrl = "images\select.gif"
                End If



            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub DataGridNewTransacts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridNewTransacts.SelectedIndexChanged
        Try
            Session("CurIndex") = Me.DataGridNewTransacts.SelectedIndex
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)


        End Try
    End Sub
    Private Sub CreateDisbDetail()
        Dim l_dataset_disbDetail As New DataSet
        Dim g_dataset_Breakdown As New DataSet
        Dim l_integer_Count As Integer
        Dim l_temprow As DataRow
        Dim TaxablePrincipal As Decimal
        Dim TaxableInterest As Decimal
        Dim NonTaxablePrincipal As Decimal
        Dim l_integer_GridIndex As Integer
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        Dim drSelectedRow As DataRow
        Dim i As Integer
        Dim c_rowIndex As Integer
        Dim strchrAcctBreakDownType As String
        Dim l_WithheldPrincipal As Decimal
        Dim l_WithheldInterest As Decimal

        Try

            TaxablePrincipal = 0.0
            TaxableInterest = 0.0
            NonTaxablePrincipal = 0.0
            strchrAcctBreakDownType = "00"

            If Not Session("DisbursementDetails") Is Nothing Then
                l_dataset_disbDetail = Session("DisbursementDetails")
                For i = 0 To l_dataset_disbDetail.Tables("DisbursementDetails").Rows.Count - 1
                    'chrAcctType
                    '  If UCase(selected_Disbursement) = UCase(l_dataset_disbDetail.Tables("DisbursementDetails").Rows(i)("DisbursementID")) Then
                    If UCase(selected_Disbursement) = UCase(l_dataset_disbDetail.Tables("DisbursementDetails").Rows(i)("DisbursementID")) And UCase(selected_Disbursement_AccType) = UCase(l_dataset_disbDetail.Tables("DisbursementDetails").Rows(i)("chrAcctType")) Then
                        c_rowIndex = i
                    End If

                Next

                'l_searchExpr = "DisbursementID =  '" + selected_Disbursement + "' and chrAcctType='" + g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("AcctType") + "' and chrAcctBreakDownType='" + strchrAcctBreakDownType + "'"
                l_searchExpr = "DisbursementID =  '" + selected_Disbursement + "'and chrAcctType='" + selected_Disbursement_AccType + "' "
                l_dr_CurrentRow = l_dataset_disbDetail.Tables("DisbursementDetails").Select(l_searchExpr)

                If l_dr_CurrentRow.Length > 0 Then
                    drSelectedRow = l_dr_CurrentRow(0)


                    l_WithheldPrincipal = IIf(TextBoxTaxWithheldPrinciple.Text.Trim() = "", 0, TextBoxTaxWithheldPrinciple.Text.Trim())
                    l_WithheldInterest = IIf(TextBoxWithheldInterest.Text.Trim() = "", 0, TextBoxWithheldInterest.Text.Trim())


                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).AcceptChanges()
                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).BeginEdit()

                    '  drSelectedRow.Item("DisbursementID") = selected_Disbursement
                    ' drSelectedRow("DisbursementNumber") = selected_Disbursement_Number
                    ' drSelectedRow.Item("chrAcctType") = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex)("AcctType")
                    ' drSelectedRow.Item("chrAcctBreakDownType") = strchrAcctBreakDownType
                    ' drSelectedRow.Item("TaxablePrincipal") = TaxablePrincipal
                    ' drSelectedRow.Item("TaxableInterest") = TaxableInterest
                    'drSelectedRow.Item("NonTaxablePrincipal") = NonTaxablePrincipal
                    'Added on 31/10/2009
                    drSelectedRow.Item("WithheldPrincipal") = l_WithheldPrincipal
                    drSelectedRow.Item("WithheldInterest") = l_WithheldInterest

                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).EndEdit()

                    Session("DisbursementDetails") = l_dataset_disbDetail


                    Session("DisbursementDetails") = l_dataset_disbDetail
                End If
            End If

        Catch ex As Exception
            Session("DisbursementDetails") = Nothing

            Throw ex
        End Try
    End Sub
    Private Sub ClearTextBox()
        TextBoxTaxablePrinciple.Text = ""
        TextBoxNonTaxablePrinciple.Text = ""
        TextBoxTaxWithheldPrinciple.Text = ""
        TextBoxTaxableInterest.Text = ""
        TextBoxWithheldInterest.Text = ""
        selected_Disbursement_AccType = ""
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Dim l_string_Output As String
        Dim i As Integer
        Dim AdjAmount As Decimal
        Dim ActualAmount As Decimal

        Try


            For i = 0 To dgDisbursements.Items.Count - 1
                AdjAmount = 0
                ActualAmount = 0
                AdjAmount = Convert.ToDecimal(IIf(dgDisbursements.Items(i).Cells(4).Text = "", "0.00", (dgDisbursements.Items(i).Cells(4).Text)))
                ActualAmount = Convert.ToDecimal(IIf(dgDisbursements.Items(i).Cells(3).Text = "", "0.00", (dgDisbursements.Items(i).Cells(3).Text)))
                If ActualAmount <> AdjAmount Then
                    ' MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "One or more Disbursement's amount has not been adjusted. Please update amount by adjusting details.", MessageBoxButtons.OK, True)
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "One or more Disbursement's amount has not been adjusted. Please update amount by adjusting Withheld Principal and Withheld Interest in details.", MessageBoxButtons.OK, True)
                    Exit Sub
                End If

            Next
            '-----------------------------------------------------------------


            If (l_string_Output = 0) Then
                Session("Called") = True
                Dim closeWindow4 As String = "<script language='javascript'>" & _
                        "self.close();window.opener.document.forms(0).submit();" & _
                        "</script>"

                If (Not Me.IsStartupScriptRegistered("CloseWindow4")) Then
                    Page.RegisterStartupScript("CloseWindow4", closeWindow4)
                End If
            End If




        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub TextBoxFactor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxFactor.TextChanged
        Dim l_integer_GridIndex As Integer

        Try
            Dim regEx As Regex
            regEx = New Regex("^\d*$")

            If (regEx.IsMatch(TextBoxFactor.Text, "^\d*$")) Then
                TextBoxTaxWithheldPrinciple.Text = Round(Convert.ToDouble(IIf(TextBoxTaxablePrinciple.Text.Trim() = "", "0", TextBoxTaxablePrinciple.Text.Trim()) * (IIf(TextBoxFactor.Text.Trim() = "", "0", TextBoxFactor.Text.Trim())) / 100), 2)
                TextBoxWithheldInterest.Text = Round(Convert.ToDouble(IIf(TextBoxTaxableInterest.Text.Trim() = "", "0", TextBoxTaxableInterest.Text.Trim()) * (IIf(TextBoxFactor.Text.Trim() = "", "0", TextBoxFactor.Text.Trim())) / 100), 2)

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
End Class
