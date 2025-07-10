

'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	DisbursementsReissue.aspx.vb
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
' Imran                         2009                Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'**Imran                        13/11/2009          If transaction does not exist then close window
'**Imran                        16/06/2010          Enhancement changes(CType to DirectCast)
' Manthan Rajguru               2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Imports System.Math
Imports System.Text.RegularExpressions
Public Class DisbursementsRevese
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DisbursementsReversal.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridTransacts As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents DataGridNewTransacts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridDisbursementDetails As System.Web.UI.WebControls.DataGrid
    ' Protected WithEvents dgDisbursements As System.Web.UI.WebControls.DataGrid

    Protected WithEvents LabelTaxWithheldPrinciple As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxableInterest As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxWithheldInterest As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFactor As System.Web.UI.WebControls.Label
    
    Protected WithEvents TextBoxTaxWithheldPrinciple As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxableInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxWithheldInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFactor As System.Web.UI.WebControls.TextBox

    Protected WithEvents TabStripBreakDown As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageBreakDown As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelGross As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGross As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelWithheld As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxWithheld As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUpdateDisbursements As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderMsg As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Regularexpressionvalidator6 As System.Web.UI.WebControls.RegularExpressionValidator

    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Regularexpressionvalidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Regularexpressionvalidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Regularexpressionvalidator4 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Regularexpressionvalidator5 As System.Web.UI.WebControls.RegularExpressionValidator
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
    Public Shared l_RequestId As String
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
                    selected_Disbursement = Strarr.GetValue(1).ToString()
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
                If Not Request.QueryString.Get("RequestId") = "" Then
                    l_RequestId = Request.QueryString.Get("RequestId").ToString()
                End If

                ''************************************HARD CODED VALUES ***************************************
                'l_string_DisbId = Session("DisbId")
                'l_string_PersId = Session("PersId")
                'l_string_FundEventId = Session("FundId")
                'l_string_WithholdingAmount = Session("WHAmount")
                'l_string_GrossAmount = Session("Gross")
                'l_string_NewStatus = Session("NewStatus")
                'l_double_NetAmount = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                '******************************************************************************************

                'Me.TabStripBreakDown.Items(1).Enabled = False
                g_dataset_Breakdown = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.LookUpTransacts(l_string_PersId, l_string_FundEventId)
                If (g_dataset_Breakdown.Tables("Transacts").Rows.Count > 0) Then
                    Me.DataGridTransacts.DataSource = g_dataset_Breakdown.Tables("Transacts")
                    Session("dsTransacts") = g_dataset_Breakdown
                    Me.DataGridTransacts.DataBind()
                    'PopulateNewTransacts()
                    ' Me.TextBoxGross.Text = l_string_GrossAmount
                    Me.TextBoxWithheld.Text = l_string_WithholdingAmount
                    Me.TextBoxNet.Text = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                Else
                    ButtonSave.Enabled = False
                    ' Me.ButtonUpdateDisbursements.Enabled = False
                    Me.ButtonUpdate.Enabled = False
                    ' Me.TextBoxGross.Text = l_string_GrossAmount
                    ' Me.TextBoxWithheld.Text = l_string_WithholdingAmount
                    ' Me.TextBoxNet.Text = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                    'Added by imran on 13/11/2009
                    Session("Called") = True
                    Dim closeWindow4 As String = "<script language='javascript'>" & _
                            "self.close();window.opener.document.forms(0).submit();" & _
                            "</script>"

                    If (Not Me.IsStartupScriptRegistered("CloseWindow4")) Then
                        Page.RegisterStartupScript("CloseWindow4", closeWindow4)
                    End If


            End If
            End If

            If Not Request.QueryString.Get("Status") = "" Then
                l_string_NewStatus = Request.QueryString.Get("Status").ToString()
            End If
            If (l_string_NewStatus <> "REPLACE") Then
                TextBoxNet.Visible = False
                TextBoxWithheld.Visible = False
                LabelNet.Visible = False
                LabelWithheld.Visible = False
            End If



        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try



    End Sub

    Private Sub TabStripBreakDown_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripBreakDown.SelectedIndexChange
        Try
            MultiPageBreakDown.SelectedIndex = TabStripBreakDown.SelectedIndex

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try

    End Sub

    Private Sub DataGridTransacts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridTransacts.ItemDataBound
        Try
            'Dim l_button_Select As ImageButton
            'l_button_Select = e.Item.FindControl("ImageButtonSel")
            'If (e.Item.ItemIndex = Me.DataGridTransacts.SelectedIndex And Me.DataGridTransacts.SelectedIndex >= 0) Then
            '    l_button_Select.ImageUrl = "images\selected.gif"
            'End If
            e.Item.Cells(1).Visible = False
            e.Item.Cells(2).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Public Sub PopulateDisbursementGrid(ByVal parameterListDisbId As Array)
        Dim l_string_PersId As String
        
        Dim l_datarow_CurrentRow As DataRow

        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        Dim g_dataset_dsDisbursementsByPersId As DataSet
        Dim i As Integer
        Dim strOutPut As String
        Dim l_GrossAmount As Decimal
        Try
            If Not Session("dsDisbursementsByPersId") Is Nothing Then

                g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                If parameterListDisbId.Length >= 2 Then
                    For i = 1 To parameterListDisbId.Length - 1
                        strOutPut += "','" + parameterListDisbId.GetValue(i)

                    Next
                End If
                strOutPut = strOutPut.Substring(3, strOutPut.Length - 3)

                l_searchExpr = "DisbursementID in  ('" + strOutPut + "')"
                l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                If l_dr_CurrentRow.Length > 0 Then

                    'Dim l_disbursementLoanHeading As DataTable
                    'l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Tables(0).DefaultView.RowFilter = l_searchExpr
                    'l_disbursementLoanHeading.TableName = "l_disbursementHeading"
                    'l_disbursementLoanHeading.Rows.Clear()

                    'For Each dr As DataRow In l_dr_CurrentRow
                    '    l_disbursementLoanHeading.Rows.Add(dr)
                    'Next

                   
                    For Each dr As DataRow In l_dr_CurrentRow
                        l_GrossAmount += dr("GrossAmount")
                    Next

                    'For i = 0 To dgDisbursements.Items.Count - 1
                    '    l_GrossAmount += dgDisbursements.Items(i).Cells(3).Text

                    'Next
                    Me.TextBoxGross.Text = l_GrossAmount.ToString()
                End If




            End If
        Catch ex As Exception
            Throw ex
        End Try




    End Sub
    Public Sub PopulateNewTransacts()
        Dim l_integer_index As Integer
        Try
            g_dataset_Breakdown = Session("dsTransacts")
            g_dataset_Breakdown.Tables.Add("NewTransacts")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("TransactID")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("FundEventId")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("AcctType")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("TType")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("TDate")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("PPreTax")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("PPostTax")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("YPreTax")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("Basis")
            'Added on 1/10/2009
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("DisbursementID")


            While l_integer_index < g_dataset_Breakdown.Tables("Transacts").Rows.Count
                g_dataset_Breakdown.Tables("NewTransacts").ImportRow(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_index))
                l_integer_index = l_integer_index + 1

            End While

            l_integer_index = 0
            While l_integer_index < g_dataset_Breakdown.Tables("NewTransacts").Rows.Count
                g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_index).Item("PPreTax") = 0
                g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_index).Item("PPostTax") = 0
                g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_index).Item("YPreTax") = 0
                l_integer_index = l_integer_index + 1
            End While


            ' System.AppDomain.CurrentDomain.SetData("dsTransacts", g_dataset_Breakdown)
            Session("dsTransacts") = g_dataset_Breakdown
            'Me.DataGridNewTransacts.DataSource = g_dataset_Breakdown.Tables("NewTransacts")
            'Me.DataGridNewTransacts.DataBind()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try


    End Sub

    Private Sub FillTextBoxes()
        Dim l_integer_GridIndex As Integer
        Try
            l_integer_GridIndex = Session("CurIndex")
            g_dataset_Breakdown = Session("dsTransacts")
            If Not g_dataset_Breakdown Is Nothing Then
                'TextBoxPreTax.Text = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax")
                'TextBoxPostTax.Text = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax")
                ' TextBoxYMCAPreTax.Text = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax")
                UpdateGross()

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub EnableDisbursements()
        Dim l_integer_GridIndex As Integer
        Dim l_boolean_status As Boolean
        Try
            'g_dataset_Breakdown = System.AppDomain.CurrentDomain.GetData("dsTransacts")
            'If Not g_dataset_Breakdown Is Nothing Then
            '    l_integer_GridIndex = 0
            '    l_boolean_status = True
            '    While l_integer_GridIndex < g_dataset_Breakdown.Tables("Transacts").Rows.Count
            '        If Convert.ToDouble(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax")) * -1 <> Convert.ToDouble(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("PPretax")) Then
            '            l_boolean_status = False
            '            Exit While
            '        End If
            '        If Convert.ToDouble(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax")) * -1 <> Convert.ToDouble(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("PPosttax")) Then
            '            l_boolean_status = False
            '            Exit While
            '        End If
            '        If Convert.ToDouble(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax")) * -1 <> Convert.ToDouble(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("YPretax")) Then
            '            l_boolean_status = False
            '            Exit While
            '        End If
            '        l_integer_GridIndex = l_integer_GridIndex + 1

            '    End While
            '    If l_boolean_status = True Then
            '        Me.TabStripBreakDown.Items(1).Enabled = True
            '        LoadDisbursements()
            '    End If
            'End If
            'If TextBoxGross.Text = "0" Or TextBoxGross.Text = "0.00" Then
            '    Me.TabStripBreakDown.Items(1).Enabled = True
            '    LoadDisbursements()
            'End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub UpdateGross()
        Dim l_integer_GridIndex As Integer

        Dim l_decimal_WH As Decimal
        Try
            g_dataset_Breakdown = Session("dsTransacts")
            If Not g_dataset_Breakdown Is Nothing Then
                l_integer_GridIndex = 0
                While l_integer_GridIndex < g_dataset_Breakdown.Tables("NewTransacts").Rows.Count
                    l_decimal_WH = l_decimal_WH + Convert.ToDecimal(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax")) + Convert.ToDecimal(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax")) + Convert.ToDecimal(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax"))
                    l_integer_GridIndex = l_integer_GridIndex + 1
                End While
                TextBoxGross.Text = Convert.ToDecimal(Request.QueryString.Get("Gross").ToString()) - l_decimal_WH


            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub UpdateWH()
        Dim l_integer_GridIndex As Integer

        Dim l_decimal_WH As Decimal
        Try
            g_dataset_DisbursementDetails = Session("dsDetails")
            If Not g_dataset_DisbursementDetails Is Nothing Then
                l_integer_GridIndex = 0
                While l_integer_GridIndex < g_dataset_DisbursementDetails.Tables("DisbDetails").Rows.Count
                    'If (g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TDate") = Session("TDate")) Then
                    l_decimal_WH = l_decimal_WH + Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxInterest")) + Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxPrinciple"))
                    l_integer_GridIndex = l_integer_GridIndex + 1
                    ' End If


                End While
                TextBoxWithheld.Text = Convert.ToDecimal(Request.QueryString.Get("WHAmount").ToString()) - l_decimal_WH
                If TextBoxGross.Text = "0" Or TextBoxGross.Text = "0.00" Then
                    TextBoxNet.Text = Convert.ToDecimal(TextBoxGross.Text) - Convert.ToDecimal(TextBoxWithheld.Text)
                Else
                    UpdateNet()
                End If


            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub UpdateNet()
        Dim l_integer_GridIndex As Integer

        Dim l_decimal_Net As Decimal
        Try
            g_dataset_DisbursementDetails = Session("dsDetails")
            If Not g_dataset_DisbursementDetails Is Nothing Then
                l_integer_GridIndex = 0
                While l_integer_GridIndex < g_dataset_DisbursementDetails.Tables("DisbDetails").Rows.Count
                    l_decimal_Net = l_decimal_Net + Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxPrinciple")) + Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxInterest")) + Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("NonTaxPrinciple")) - Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxInterest")) - Convert.ToDecimal(g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxPrinciple"))
                    l_integer_GridIndex = l_integer_GridIndex + 1
                End While
                TextBoxNet.Text = Convert.ToDecimal(Request.QueryString.Get("Gross")) - Convert.ToDecimal(Request.QueryString.Get("WHAmount")) - l_decimal_Net
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub LoadDisbursements()
        'Dim l_string_PersId As String
        Dim l_string_TDate As String

        Try
            l_string_TDate = Session("TDate")
            g_dataset_DisbursementDetails = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.LookUpDisbursementDetails(Request.QueryString.Get("PersId").ToString(), Request.QueryString.Get("DisbId").ToString(), Request.QueryString.Get("FundId").ToString(), l_string_TDate)
            Session("v") = g_dataset_DisbursementDetails

            If g_dataset_DisbursementDetails.Tables("DisbDetails").Rows.Count > 0 Then
                Me.DataGridDisbursementDetails.DataSource = g_dataset_DisbursementDetails
                Me.DataGridDisbursementDetails.DataBind()

                TextBoxTaxWithheldPrinciple.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(0).Item("WHTaxPrinciple")
                TextBoxTaxableInterest.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(0).Item("TaxInterest")
                TextBoxWithheldInterest.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(0).Item("WHTaxInterest")

                UpdateWH()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridDisbursementDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridDisbursementDetails.SelectedIndexChanged
        Try
            Session("Disbursement_Index") = Me.DataGridDisbursementDetails.SelectedIndex
            LoadDisbursements()
            FillDisbursementTextBoxes()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub FillDisbursementTextBoxes()
        Dim l_integer_GridIndex As Integer
        Try
            l_integer_GridIndex = Session("Disbursement_Index")
            g_dataset_DisbursementDetails = Session("dsDetails")
            If Not g_dataset_DisbursementDetails Is Nothing Then

                TextBoxTaxWithheldPrinciple.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxPrinciple")
                TextBoxTaxableInterest.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxInterest")
                TextBoxWithheldInterest.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxInterest")

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

        Dim closeWindow5 As String = "<script language='javascript'>" & _
                                                     "window.close()" & _
                                                     "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        End If
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_string_NewStatus As String
        Dim l_string_Output As String
        Dim l_string_Xml As String
        Dim l_dataset_NewTransacts As New DataSet
        Dim l_integer_Count As Integer
        Dim l_string_PersId As String
        Dim l_string_DisbId As String
        Dim l_string_AddId As String
        Dim l_string_Id As String
        Dim l_string_IdsForReversal As String

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

        Try
            l_string_NewStatus = Convert.ToString(Request.QueryString.Get("Status"))
            l_string_DisbId = Request.QueryString.Get("DisbId").ToString()
            l_string_PersId = Request.QueryString.Get("PersId").ToString()
            If Not Request.QueryString.Get("AddId") Is Nothing Then
                If Request.QueryString.Get("AddId").ToString() <> "" Then
                    l_string_AddId = Request.QueryString.Get("AddId").ToString()
                Else
                    l_string_AddId = ""
                End If
            End If


            If Not Request.QueryString.Get("PayId") Is Nothing Then
                If Request.QueryString.Get("PayId") <> "" Then
                    l_string_Id = Request.QueryString.Get("PayId").ToString()
                Else
                    l_string_Id = Request.QueryString.Get("PersId").ToString()
                End If
            End If


            '****************************************************************
            'New Status = "REVERSE"
            '****************************************************************
            If l_string_NewStatus = "REVERSE" Then
                l_string_Output = "0"
                If (Me.TextBoxGross.Text <> "0.00") Then
                    l_string_Output = 1
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Gross amount is not equal to disbursements. Please update transactions/withholding detail amounts.", MessageBoxButtons.OK, True)
                Else
                    Dim l_string_PartNewStatus As String
                    Dim l_string_OutputStatus As String

                    l_string_PartNewStatus = Request.QueryString.Get("PStatus")
                    g_dataset_Breakdown = Session("dsTransacts")
                    l_dataset_NewTransacts.Tables.Add("RefundDetails")



                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Clear()
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("TransactID")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("FundEventId")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("chrAcctType")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("TType")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("TDate")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("TaxablePrincipal")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("TaxableInterest")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("NonTaxablePrincipal")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("intTaxRate")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("chvPayee")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("chrRequestType")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("chrAnnuityBasisType")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("DisbursementID")
                    l_dataset_NewTransacts.Tables("RefundDetails").Columns.Add("RefRequestsID")

                    l_integer_Count = 0

                    For l_integer_Count = 0 To DataGridTransacts.Items.Count - 1
                        Dim chktrans As CheckBox
                        chktrans = DataGridTransacts.Items(l_integer_Count).FindControl("chkTrans")
                        If chktrans.Checked Then
                            TaxablePrincipal = 0.0
                            TaxableInterest = 0.0
                            NonTaxablePrincipal = 0.0
                            strchrAcctBreakDownType = "00"


                            l_temprow = l_dataset_NewTransacts.Tables("RefundDetails").NewRow

                            If DataGridTransacts.Items(l_integer_Count).Cells(4).Text = "RFPR" Then
                                TaxablePrincipal = Convert.ToDecimal(IIf(DataGridTransacts.Items(l_integer_Count).Cells(6).Text = "", "0.0", DataGridTransacts.Items(l_integer_Count).Cells(6).Text)) + Convert.ToDecimal(IIf(DataGridTransacts.Items(l_integer_Count).Cells(7).Text = "", "0.0", DataGridTransacts.Items(l_integer_Count).Cells(7).Text))
                            End If
                            If DataGridTransacts.Items(l_integer_Count).Cells(4).Text = "RFIN" Then
                                TaxableInterest = Convert.ToDecimal(IIf(DataGridTransacts.Items(l_integer_Count).Cells(6).Text = "", "0.0", DataGridTransacts.Items(l_integer_Count).Cells(6).Text)) + Convert.ToDecimal(IIf(DataGridTransacts.Items(l_integer_Count).Cells(7).Text = "", "0.0", DataGridTransacts.Items(l_integer_Count).Cells(7).Text))
                            End If
                            NonTaxablePrincipal = Convert.ToDecimal(IIf(DataGridTransacts.Items(l_integer_Count).Cells(8).Text = "", "0.0", DataGridTransacts.Items(l_integer_Count).Cells(8).Text))

                            l_temprow("TransactID") = DataGridTransacts.Items(l_integer_Count).Cells(1).Text
                            l_temprow("DisbursementID") = selected_Disbursement
                            l_temprow("chrAcctType") = DataGridTransacts.Items(l_integer_Count).Cells(3).Text
                            'l_temprow("chrAcctBreakDownType") = strchrAcctBreakDownType
                            l_temprow("TaxablePrincipal") = TaxablePrincipal
                            l_temprow("TaxableInterest") = TaxableInterest
                            l_temprow("NonTaxablePrincipal") = NonTaxablePrincipal
                            l_temprow("RefRequestsID") = l_RequestId
                            l_dataset_NewTransacts.Tables("RefundDetails").Rows.Add(l_temprow)

                        End If
                    Next

                    If l_dataset_NewTransacts.Tables("RefundDetails").Rows.Count > 0 Then
                        Session("RefundDetails") = l_dataset_NewTransacts
                    End If

                End If
            End If

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
    Private Sub ButtonUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdate.Click
        Dim l_integer_GridIndex As Integer
        Try
            l_integer_GridIndex = Session("Disbursement_Index")
            g_dataset_DisbursementDetails = Session("dsDetails")
            If Not g_dataset_DisbursementDetails Is Nothing Then
                'If TextBoxTaxablePrinciple.Text <> "" Then
                '    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxPrinciple") = TextBoxTaxablePrinciple.Text
                'End If
                'If TextBoxNonTaxablePrinciple.Text <> "" Then
                '    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("NonTaxPrinciple") = TextBoxNonTaxablePrinciple.Text
                'End If
                If TextBoxTaxWithheldPrinciple.Text <> "" Then
                    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxPrinciple") = TextBoxTaxWithheldPrinciple.Text
                End If
                If TextBoxTaxableInterest.Text <> "" Then
                    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxInterest") = TextBoxTaxableInterest.Text
                End If
                If TextBoxWithheldInterest.Text <> "" Then
                    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxInterest") = TextBoxWithheldInterest.Text
                End If


                Me.DataGridDisbursementDetails.DataSource = g_dataset_DisbursementDetails
                Me.DataGridDisbursementDetails.DataBind()
                UpdateWH()
                'TextBoxTaxablePrinciple.Text = ""
                'TextBoxNonTaxablePrinciple.Text = ""
                TextBoxTaxWithheldPrinciple.Text = ""
                TextBoxTaxableInterest.Text = ""
                TextBoxWithheldInterest.Text = ""
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub DataGridDisbursementDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridDisbursementDetails.ItemDataBound
        Try
            'Dim l_button_select As ImageButton
            'l_button_select = e.Item.FindControl("ImageButtonSelect2")
            'If (e.Item.ItemIndex = Me.DataGridDisbursementDetails.SelectedIndex And Me.DataGridDisbursementDetails.SelectedIndex >= 0) Then
            '    l_button_select.ImageUrl = "images\selected.gif"
            'End If
            e.Item.Cells(9).Visible = False
            e.Item.Cells(2).Visible = False
            e.Item.Cells(10).Visible = False

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

                If Session("Disbursement_Index") Is Nothing Then
                    l_integer_GridIndex = 0
                Else
                    l_integer_GridIndex = Session("Disbursement_Index")
                End If
                'TextBoxTaxWithheldPrinciple.Text = Round(Convert.ToDouble(TextBoxTaxablePrinciple.Text * TextBoxFactor.Text / 100), 2)
                TextBoxWithheldInterest.Text = Round(Convert.ToDouble(TextBoxTaxableInterest.Text * TextBoxFactor.Text / 100), 2)
                ' TextBoxTaxWithheldPrinciple.Text = TextBoxTaxWithheldPrinciple.Text.Substring(0, TextBoxTaxWithheldPrinciple.Text.Length - 1)
                '  TextBoxWithheldInterest.Text = TextBoxWithheldInterest.Text.Substring(0, TextBoxWithheldInterest.Text.Length - 1)
                g_dataset_DisbursementDetails = Session("dsDetails")
                If Not g_dataset_DisbursementDetails Is Nothing Then
                    'If TextBoxTaxablePrinciple.Text <> "" Then
                    '    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxPrinciple") = Convert.ToDecimal(TextBoxTaxablePrinciple.Text)
                    'End If
                    'If TextBoxNonTaxablePrinciple.Text <> "" Then
                    '    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("NonTaxPrinciple") = Convert.ToDecimal(TextBoxNonTaxablePrinciple.Text)
                    'End If
                    If TextBoxTaxWithheldPrinciple.Text <> "" Then
                        g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxPrinciple") = Convert.ToDecimal(TextBoxTaxWithheldPrinciple.Text)
                    End If
                    If TextBoxTaxableInterest.Text <> "" Then
                        g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxInterest") = Convert.ToDecimal(TextBoxTaxableInterest.Text)
                    End If
                    If TextBoxWithheldInterest.Text <> "" Then
                        g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("WHTaxInterest") = Convert.ToDecimal(TextBoxWithheldInterest.Text)
                    End If



                    Me.DataGridDisbursementDetails.DataSource = g_dataset_DisbursementDetails
                    Me.DataGridDisbursementDetails.DataBind()
                    UpdateWH()
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try

    End Sub

    Private Sub DataGridTransacts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridTransacts.SelectedIndexChanged
        Dim PreTax As Decimal
        Dim PostTax As Decimal
        Dim PreYMCATax As Decimal
        Dim Gross As Decimal

        Try
            Gross = Convert.ToDecimal(IIf(TextBoxGross.Text = "", "0", TextBoxGross.Text))
            If Gross <> 0 Then

                PreTax = Convert.ToDecimal(IIf(Me.DataGridTransacts.SelectedItem.Cells(6).Text = "", "0", Me.DataGridTransacts.SelectedItem.Cells(6).Text))
                PostTax = Convert.ToDecimal(IIf(Me.DataGridTransacts.SelectedItem.Cells(7).Text = "", "0", Me.DataGridTransacts.SelectedItem.Cells(7).Text))
                PreYMCATax = Convert.ToDecimal(IIf(Me.DataGridTransacts.SelectedItem.Cells(8).Text = "", "0", Me.DataGridTransacts.SelectedItem.Cells(8).Text))


                TextBoxGross.Text = Convert.ToDecimal(TextBoxGross.Text) - (PreTax + PostTax + PreYMCATax)
            End If
            'FillTextBoxes()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)


        End Try


    End Sub
    Private Sub DataGridTransacts_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridTransacts.ItemCommand
        Try
            Dim i As Integer
            Dim l_button_select As ImageButton
            Dim l_button_select2 As ImageButton
            If (e.CommandName = "Select") Then

                l_button_select = e.Item.FindControl("ImageButtonSel")
                ' If e.Item.ItemIndex = Me.DataGridTransacts.SelectedIndex And Me.DataGridTransacts.SelectedIndex >= 0 Then
                l_button_select.ImageUrl = "images\selected.gif"
                'End If
            End If
            While i < Me.DataGridTransacts.Items.Count
                l_button_select = New ImageButton
                l_button_select2 = New ImageButton
                If i <> e.Item.ItemIndex Then
                    l_button_select = Me.DataGridTransacts.Items(i).FindControl("ImageButtonSel")
                    ' l_button_select2 = Me.DataGridNewTransacts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\select.gif"
                    l_button_select2.ImageUrl = "images\select.gif"
                Else
                    ' l_button_select2 = Me.DataGridNewTransacts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select2.ImageUrl = "images\selected.gif"
                End If
                i = i + 1
            End While

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

        Try

            l_integer_GridIndex = Session("CurIndex")
            g_dataset_Breakdown = Session("dsTransacts")
            TaxablePrincipal = 0.0
            TaxableInterest = 0.0
            NonTaxablePrincipal = 0.0
            strchrAcctBreakDownType = "00"

            If Not Session("DisbursementDetails") Is Nothing Then
                l_dataset_disbDetail = Session("DisbursementDetails")
                For i = 0 To i < l_dataset_disbDetail.Tables("DisbursementDetails").Rows.Count
                    If selected_Disbursement = l_dataset_disbDetail.Tables("DisbursementDetails").Rows(i)("DisbursementID") Then
                        c_rowIndex = i
                    End If

                Next

                l_searchExpr = "DisbursementID =  '" + selected_Disbursement + "' and chrAcctType='" + g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("AcctType") + "' and chrAcctBreakDownType='" + strchrAcctBreakDownType + "'"
                l_dr_CurrentRow = l_dataset_disbDetail.Tables("DisbursementDetails").Select(l_searchExpr)

                If l_dr_CurrentRow.Length > 0 Then
                    drSelectedRow = l_dr_CurrentRow(0)

                    TaxablePrincipal = drSelectedRow("TaxablePrincipal")
                    TaxableInterest = drSelectedRow("TaxableInterest")
                    NonTaxablePrincipal = drSelectedRow("NonTaxablePrincipal")

                    'If g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex)("TType") = "RFPR" Then
                    '    TaxablePrincipal = TaxablePrincipal + Convert.ToDecimal(IIf(TextBoxPreTax.Text = "", "0.0", TextBoxPreTax.Text)) + Convert.ToDecimal(IIf(TextBoxYMCAPreTax.Text = "", "0.0", TextBoxYMCAPreTax.Text))
                    'End If
                    'If g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex)("TType") = "RFIN" Then
                    '    TaxableInterest = TaxableInterest + Convert.ToDecimal(IIf(TextBoxPreTax.Text = "", "0.0", TextBoxPreTax.Text)) + Convert.ToDecimal(IIf(TextBoxYMCAPreTax.Text = "", "0.0", TextBoxYMCAPreTax.Text))
                    'End If
                    'NonTaxablePrincipal = NonTaxablePrincipal + Convert.ToDecimal(TextBoxPostTax.Text)

                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).AcceptChanges()
                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).BeginEdit()

                    drSelectedRow.Item("DisbursementID") = selected_Disbursement
                    drSelectedRow.Item("chrAcctType") = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex)("AcctType")
                    drSelectedRow.Item("chrAcctBreakDownType") = strchrAcctBreakDownType
                    drSelectedRow.Item("TaxablePrincipal") = TaxablePrincipal
                    drSelectedRow.Item("TaxableInterest") = TaxableInterest
                    drSelectedRow.Item("NonTaxablePrincipal") = NonTaxablePrincipal


                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows(c_rowIndex).EndEdit()

                    Session("DisbursementDetails") = l_dataset_disbDetail
                Else
                    l_temprow = l_dataset_disbDetail.Tables("DisbursementDetails").NewRow

                    'If g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("TType") = "RFPR" Then
                    '    TaxablePrincipal = Convert.ToDecimal(IIf(TextBoxPreTax.Text = "", "0.0", TextBoxPreTax.Text)) + Convert.ToDecimal(TextBoxYMCAPreTax.Text)
                    'End If
                    'If g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("TType") = "RFIN" Then
                    '    TaxableInterest = Convert.ToDecimal(IIf(TextBoxPreTax.Text = "", "0.0", TextBoxPreTax.Text)) + Convert.ToDecimal(IIf(TextBoxYMCAPreTax.Text = "", "0.0", TextBoxYMCAPreTax.Text))
                    'End If
                    'NonTaxablePrincipal = Convert.ToDecimal(IIf(TextBoxPostTax.Text = "", "0.0", TextBoxPostTax.Text))

                    l_temprow("DisbursementID") = selected_Disbursement
                    l_temprow("chrAcctType") = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("AcctType")
                    l_temprow("chrAcctBreakDownType") = strchrAcctBreakDownType
                    l_temprow("TaxablePrincipal") = TaxablePrincipal
                    l_temprow("TaxableInterest") = TaxableInterest
                    l_temprow("NonTaxablePrincipal") = NonTaxablePrincipal

                    l_dataset_disbDetail.Tables("DisbursementDetails").Rows.Add(l_temprow)

                    Session("DisbursementDetails") = l_dataset_disbDetail


                End If
            Else
                l_dataset_disbDetail.Tables.Add("DisbursementDetails")
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Clear()
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Add("DisbursementID")
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Add("chrAcctType")
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Add("chrAcctBreakDownType")
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Add("TaxablePrincipal")
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Add("TaxableInterest")
                l_dataset_disbDetail.Tables("DisbursementDetails").Columns.Add("NonTaxablePrincipal")

                l_temprow = l_dataset_disbDetail.Tables("DisbursementDetails").NewRow

                'If g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("TType") = "RFPR" Then
                '    TaxablePrincipal = Convert.ToDecimal(IIf(TextBoxPreTax.Text = "", "0.0", TextBoxPreTax.Text)) + Convert.ToDecimal(IIf(TextBoxYMCAPreTax.Text = "", "0.0", TextBoxYMCAPreTax.Text))
                'End If
                'If g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("TType") = "RFIN" Then
                '    TaxableInterest = Convert.ToDecimal(IIf(TextBoxPreTax.Text = "", "0.0", TextBoxPreTax.Text)) + Convert.ToDecimal(IIf(TextBoxYMCAPreTax.Text = "", "0.0", TextBoxYMCAPreTax.Text))
                'End If
                'NonTaxablePrincipal = Convert.ToDecimal(IIf(TextBoxPostTax.Text = "", "0.0", TextBoxPostTax.Text))

                l_temprow("DisbursementID") = selected_Disbursement
                l_temprow("chrAcctType") = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count)("AcctType")
                l_temprow("chrAcctBreakDownType") = strchrAcctBreakDownType
                l_temprow("TaxablePrincipal") = TaxablePrincipal
                l_temprow("TaxableInterest") = TaxableInterest
                l_temprow("NonTaxablePrincipal") = NonTaxablePrincipal

                l_dataset_disbDetail.Tables("DisbursementDetails").Rows.Add(l_temprow)

                Session("DisbursementDetails") = l_dataset_disbDetail

            End If

        Catch ex As Exception
            Session("DisbursementDetails") = Nothing

            Throw ex
        End Try
    End Sub

    Sub CheckBox_CheckedChanged(ByVal Source As Object, ByVal e As System.EventArgs)
        Try

            Dim PreTax As Decimal
            Dim PostTax As Decimal
            Dim PreYMCATax As Decimal
            Dim Gross As Decimal

            Dim strID As String
            Dim chk As CheckBox
            chk = CType(Source, CheckBox)

            strID = chk.ClientID
            If Not IsNothing(chk) Then

                Dim i As Integer
                For i = 0 To DataGridTransacts.Items.Count - 1
                    Dim chkRefund As CheckBox
                    chkRefund = DataGridTransacts.Items(i).FindControl("chkTrans")
                    If Not IsNothing(chkRefund) Then

                        If chkRefund.ClientID = strID Then


                            Gross = Convert.ToDecimal(IIf(TextBoxGross.Text = "", "0", TextBoxGross.Text))
                            PreTax = Convert.ToDecimal(IIf(Me.DataGridTransacts.Items(i).Cells(6).Text = "", "0", Me.DataGridTransacts.Items(i).Cells(6).Text))
                            PostTax = Convert.ToDecimal(IIf(Me.DataGridTransacts.Items(i).Cells(7).Text = "", "0", Me.DataGridTransacts.Items(i).Cells(7).Text))
                            PreYMCATax = Convert.ToDecimal(IIf(Me.DataGridTransacts.Items(i).Cells(8).Text = "", "0", Me.DataGridTransacts.Items(i).Cells(8).Text))

                            If chkRefund.Checked Then

                                If Gross <> 0 And Gross > 0 Then
                                    'TextBoxGross.Text = Convert.ToDecimal(TextBoxGross.Text) - (PreTax + PostTax + PreYMCATax)
                                    TextBoxGross.Text = Convert.ToDecimal(TextBoxGross.Text) + (PreTax + PostTax + PreYMCATax)
                                Else
                                    chkRefund.Checked = False
                                End If

                            Else
                                'If Gross <> 0 Then
                                '    TextBoxGross.Text = Convert.ToDecimal(TextBoxGross.Text) + (PreTax + PostTax + PreYMCATax)
                                'End If
                                'TextBoxGross.Text = Convert.ToDecimal(TextBoxGross.Text) + (PreTax + PostTax + PreYMCATax)
                                TextBoxGross.Text = Convert.ToDecimal(TextBoxGross.Text) - (PreTax + PostTax + PreYMCATax)
                                chkRefund.Checked = False
                            End If
                        End If



                    End If
                Next
            End If







        Catch ex As Exception
            '  Throw (ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
End Class
