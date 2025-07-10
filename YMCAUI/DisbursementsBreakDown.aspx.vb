'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	DisbursementsBreakDown.aspx.vb
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Imports System.Math
Imports System.Text.RegularExpressions

Public Class DisbursementsBreakDown
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DisbursementsBreakDown.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridTransacts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridNewTransacts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridDisbursementDetails As System.Web.UI.WebControls.DataGrid
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



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim l_string_DisbId As String
        Dim l_string_PersId As String
        Dim l_string_FundEventId As String
        Dim l_string_WithholdingAmount As String
        Dim l_string_GrossAmount As String
        Dim l_string_NewStatus As String
        Dim l_double_NetAmount As Double
        Dim l_string_DisbIds As String
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not IsPostBack Then
                TextBoxFactor.Text = "20"
                If Not Request.QueryString.Get("DisbId") = "" Then
                    l_string_DisbId = Request.QueryString.Get("DisbId").ToString()
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

                ''************************************HARD CODED VALUES ***************************************
                'l_string_DisbId = Session("DisbId")
                'l_string_PersId = Session("PersId")
                'l_string_FundEventId = Session("FundId")
                'l_string_WithholdingAmount = Session("WHAmount")
                'l_string_GrossAmount = Session("Gross")
                'l_string_NewStatus = Session("NewStatus")
                'l_double_NetAmount = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                '******************************************************************************************

                Me.TabStripBreakDown.Items(1).Enabled = False
                g_dataset_Breakdown = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.LookUpTransacts(l_string_PersId, l_string_FundEventId)
                If (g_dataset_Breakdown.Tables("Transacts").Rows.Count > 0) Then
                    Me.DataGridTransacts.DataSource = g_dataset_Breakdown.Tables("Transacts")
                    Session("dsTransacts") = g_dataset_Breakdown
                    Me.DataGridTransacts.DataBind()
                    PopulateNewTransacts()
                    Me.TextBoxGross.Text = l_string_GrossAmount
                    Me.TextBoxWithheld.Text = l_string_WithholdingAmount
                    Me.TextBoxNet.Text = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                Else
                    ButtonSave.Enabled = False
                    Me.ButtonUpdateDisbursements.Enabled = False
                    Me.ButtonUpdate.Enabled = False
                    Me.TextBoxGross.Text = l_string_GrossAmount
                    Me.TextBoxWithheld.Text = l_string_WithholdingAmount
                    Me.TextBoxNet.Text = Convert.ToDouble(l_string_GrossAmount) - Convert.ToDouble(l_string_WithholdingAmount)
                End If
            End If
            If (Session("CurIndex") = -1) Then
                Me.TextBoxPostTax.Text = "0.00"
                Me.TextBoxPreTax.Text = "0.00"
                Me.TextBoxYMCAPreTax.Text = "0.00"
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
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Sub


    Public Sub PopulateNewTransacts()
        Dim l_integer_index As Integer
        Try
            g_dataset_Breakdown = Session("dsTransacts")
            g_dataset_Breakdown.Tables.Add("NewTransacts")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("FundEventId")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("AcctType")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("TType")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("TDate")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("PPreTax")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("PPostTax")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("YPreTax")
            g_dataset_Breakdown.Tables("NewTransacts").Columns.Add("Basis")

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
            Me.DataGridNewTransacts.DataSource = g_dataset_Breakdown.Tables("NewTransacts")
            Me.DataGridNewTransacts.DataBind()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try


    End Sub

    Private Sub DataGridNewTransacts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridNewTransacts.ItemDataBound
        Try
            'Dim l_button_Select As ImageButton
            'l_button_Select = e.Item.FindControl("ImageButtonSelect")
            'If (e.Item.ItemIndex = Me.DataGridNewTransacts.SelectedIndex And Me.DataGridNewTransacts.SelectedIndex >= 0) Then
            '    l_button_Select.ImageUrl = "images\selected.gif"
            'End If
            e.Item.Cells(1).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Sub



    Private Sub DataGridNewTransacts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridNewTransacts.SelectedIndexChanged
        Try
            Session("CurIndex") = Me.DataGridNewTransacts.SelectedIndex
            Session("TDate") = Me.DataGridNewTransacts.Items(Me.DataGridNewTransacts.SelectedIndex).Cells(4).Text
            Me.DataGridTransacts.SelectedIndex = Me.DataGridNewTransacts.SelectedIndex

            FillTextBoxes()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub


    Private Sub TextBoxPreTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxPreTax.TextChanged
        'Dim l_integer_GridIndex As Integer
        'Try
        '    l_integer_GridIndex = Session("CurIndex")
        '    g_dataset_Breakdown = System.AppDomain.CurrentDomain.GetData("dsTransacts")
        '    If Not g_dataset_Breakdown Is Nothing Then
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax") = TextBoxPreTax.Text
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax") = TextBoxPostTax.Text
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax") = TextBoxYMCAPreTax.Text
        '        Me.DataGridNewTransacts.DataSource = g_dataset_Breakdown.Tables("NewTransacts")
        '        Me.DataGridNewTransacts.DataBind()
        '        EnableDisbursements()
        '        UpdateGross()
        '    End If

        'Catch ex As Exception
        '    Throw
        'End Try
    End Sub

    Private Sub TextBoxPostTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxPostTax.TextChanged
        'Dim l_integer_GridIndex As Integer
        'Try
        '    l_integer_GridIndex = Session("CurIndex")
        '    g_dataset_Breakdown = System.AppDomain.CurrentDomain.GetData("dsTransacts")
        '    If Not g_dataset_Breakdown Is Nothing Then
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax") = TextBoxPreTax.Text
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax") = TextBoxPostTax.Text
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax") = TextBoxYMCAPreTax.Text
        '        Me.DataGridNewTransacts.DataSource = g_dataset_Breakdown.Tables("NewTransacts")
        '        Me.DataGridNewTransacts.DataBind()
        '        EnableDisbursements()
        '        UpdateGross()
        '    End If

        'Catch ex As Exception
        '    Throw
        'End Try
    End Sub

    Private Sub TextBoxYMCAPreTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxYMCAPreTax.TextChanged
        'Dim l_integer_GridIndex As Integer
        'Try
        '    l_integer_GridIndex = Session("CurIndex")
        '    g_dataset_Breakdown = System.AppDomain.CurrentDomain.GetData("dsTransacts")
        '    If Not g_dataset_Breakdown Is Nothing Then
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax") = TextBoxPreTax.Text
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax") = TextBoxPostTax.Text
        '        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax") = TextBoxYMCAPreTax.Text
        '        Me.DataGridNewTransacts.DataSource = g_dataset_Breakdown.Tables("NewTransacts")
        '        Me.DataGridNewTransacts.DataBind()
        '        EnableDisbursements()
        '        UpdateGross()
        '    End If

        'Catch ex As Exception
        '    Throw
        'End Try
    End Sub

    Private Sub FillTextBoxes()
        Dim l_integer_GridIndex As Integer
        Try
            l_integer_GridIndex = Session("CurIndex")
            g_dataset_Breakdown = Session("dsTransacts")
            If Not g_dataset_Breakdown Is Nothing Then
                TextBoxPreTax.Text = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax")
                TextBoxPostTax.Text = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax")
                TextBoxYMCAPreTax.Text = g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax")
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
            If TextBoxGross.Text = "0" Or TextBoxGross.Text = "0.00" Then
                Me.TabStripBreakDown.Items(1).Enabled = True
                LoadDisbursements()
            End If

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
                If Request.QueryString.Get("Status") = "REPLACE" Then
                    EnableDisbursements()
                End If
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
                TextBoxTaxablePrinciple.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(0).Item("TaxPrinciple")
                TextBoxNonTaxablePrinciple.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(0).Item("NonTaxPrinciple")
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
                TextBoxTaxablePrinciple.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxPrinciple")
                TextBoxNonTaxablePrinciple.Text = g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("NonTaxPrinciple")
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
            'New Status = "REPLACE"
            '****************************************************************
            If l_string_NewStatus = "REPLACE" Then
                l_string_Output = "1"
                If (Me.TextBoxGross.Text <> "0" Or Me.TextBoxNet.Text <> "0" Or Me.TextBoxWithheld.Text <> "0") Then
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Either of Withholding, Gross or Net amounts are not equal to disbursements. Please update transactions/withholding detail amounts.", MessageBoxButtons.OK, True)
                Else
                    g_dataset_DisbursementDetails = Session("dsDetails")
                    l_string_Xml = g_dataset_DisbursementDetails.GetXml()
                    l_string_Output = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.UpdateDisbursement(l_string_Xml)
                    Dim popupScript As String = "<script language='javascript'>" & _
                                                          "window.open('VDReplaceDisbursements.aspx?PersId=" + l_string_PersId + "&DisbId=" + l_string_DisbId + "&AddId=" + l_string_AddId + "&PayId=" + l_string_Id + "', 'CustomPopUp', " & _
                                                          "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                                          "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                        Page.RegisterStartupScript("PopupScript1", popupScript)
                    End If
                End If
            End If



            '****************************************************************
            'New Status = "REISSUE"
            '****************************************************************
            If l_string_NewStatus = "REISSUE" Then
                l_string_Output = "1"
                If (Me.TextBoxGross.Text <> "0") Then
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Gross amount is not equal to disbursements. Please update transactions/withholding detail amounts.", MessageBoxButtons.OK, True)
                Else
                    g_dataset_Breakdown = Session("dsTransacts")
                    l_dataset_NewTransacts.Tables.Add("NewTransacts")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("FundEventId")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("AcctType")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("TType")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("TDate")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("PPreTax")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("PPostTax")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("YPreTax")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("Basis")
                    l_integer_Count = 0
                    While l_integer_Count < g_dataset_Breakdown.Tables("NewTransacts").Rows.Count
                        l_dataset_NewTransacts.Tables("NewTransacts").ImportRow(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count))
                        l_integer_Count = l_integer_Count + 1
                    End While
                    l_string_Xml = l_dataset_NewTransacts.GetXml()
                    l_string_Output = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.AddTransactsForReissue(l_string_Xml, l_string_DisbId, l_string_PersId)
                    ' l_string_Output = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.ChangeDisbursementStatus(l_string_DisbId, "VOID", "Voided for Reissue.")
                End If
            End If
            '****************************************************************
            'New Status = "REVERSE"
            '****************************************************************
            If l_string_NewStatus = "REVERSE" Then
                l_string_Output = "1"
                If (Me.TextBoxGross.Text <> "0") Then
                    MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Gross amount is not equal to disbursements. Please update transactions/withholding detail amounts.", MessageBoxButtons.OK, True)
                Else
                    Dim l_string_PartNewStatus As String
                    Dim l_string_OutputStatus As String

                    l_string_PartNewStatus = Request.QueryString.Get("PStatus")
                    g_dataset_Breakdown = Session("dsTransacts")
                    l_dataset_NewTransacts.Tables.Add("NewTransacts")

                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("FundEventId")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("AcctType")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("TType")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("TDate")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("PPreTax")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("PPostTax")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("YPreTax")
                    l_dataset_NewTransacts.Tables("NewTransacts").Columns.Add("Basis")
                    l_integer_Count = 0
                    If Not Session("IdForReversal") = "" Then
                        l_string_IdsForReversal = Session("IdForReversal")
                    End If
                    While l_integer_Count < g_dataset_Breakdown.Tables("NewTransacts").Rows.Count
                        l_dataset_NewTransacts.Tables("NewTransacts").ImportRow(g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_Count))
                        l_integer_Count = l_integer_Count + 1
                    End While
                    l_string_Xml = l_dataset_NewTransacts.GetXml()
                    l_string_Output = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.AddTransactsForReversal(l_string_Xml, l_string_IdsForReversal, l_string_PersId, l_string_PartNewStatus)
                    ' l_string_OutputStatus = YMCARET.YmcaBusinessObject.DisbursementBreakdownBOClass.UpdateDisbursementsForReversal(l_string_IdsForReversal)
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

    Private Sub ButtonUpdateDisbursements_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdateDisbursements.Click
        Dim l_integer_GridIndex As Integer
        Dim l_string_NewStatus As String
        l_string_NewStatus = Convert.ToString(Request.QueryString.Get("Status"))
        Try
            l_integer_GridIndex = Session("CurIndex")
            g_dataset_Breakdown = Session("dsTransacts")
            If Not g_dataset_Breakdown Is Nothing Then
                If TextBoxPreTax.Text <> "" Then
                    If g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("PPretax") * -1 >= Convert.ToDecimal(TextBoxPreTax.Text) Then
                        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPretax") = TextBoxPreTax.Text
                    Else
                        MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Interest Personal PreTax can not be greater than " + Convert.ToString(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("PPretax") * -1), MessageBoxButtons.OK, True)

                        Exit Sub

                    End If

                End If
                If TextBoxPostTax.Text <> "" Then
                    If g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("PPosttax") * -1 >= Convert.ToDecimal(TextBoxPostTax.Text) Then
                        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("PPosttax") = TextBoxPostTax.Text
                    Else
                        MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Interest Personal PostTax can not be greater than " + Convert.ToString(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("PPosttax") * -1), MessageBoxButtons.OK, True)
                        Exit Sub
                    End If

                End If
                If TextBoxYMCAPreTax.Text <> "" Then
                    If g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("YPretax") * -1 >= Convert.ToDecimal(TextBoxYMCAPreTax.Text) Then
                        g_dataset_Breakdown.Tables("NewTransacts").Rows(l_integer_GridIndex).Item("YPretax") = TextBoxYMCAPreTax.Text
                    Else
                        MessageBox.Show(PlaceHolderMsg, "YMCA-YRS", "Principle Ymca PreTax can not be greater than " + Convert.ToString(g_dataset_Breakdown.Tables("Transacts").Rows(l_integer_GridIndex).Item("YPretax") * -1), MessageBoxButtons.OK, True)
                        Exit Sub
                    End If

                End If

                Me.DataGridNewTransacts.DataSource = g_dataset_Breakdown.Tables("NewTransacts")
                Me.DataGridNewTransacts.DataBind()
                If (l_string_NewStatus = "REPLACE") Then
                    EnableDisbursements()
                End If

                UpdateGross()
                TextBoxPreTax.Text = ""
                TextBoxPostTax.Text = ""
                TextBoxYMCAPreTax.Text = ""
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
                If TextBoxTaxablePrinciple.Text <> "" Then
                    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxPrinciple") = TextBoxTaxablePrinciple.Text
                End If
                If TextBoxNonTaxablePrinciple.Text <> "" Then
                    g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("NonTaxPrinciple") = TextBoxNonTaxablePrinciple.Text
                End If
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
                TextBoxTaxablePrinciple.Text = ""
                TextBoxNonTaxablePrinciple.Text = ""
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
                TextBoxTaxWithheldPrinciple.Text = Round(Convert.ToDouble(TextBoxTaxablePrinciple.Text * TextBoxFactor.Text / 100), 2)
                TextBoxWithheldInterest.Text = Round(Convert.ToDouble(TextBoxTaxableInterest.Text * TextBoxFactor.Text / 100), 2)
                ' TextBoxTaxWithheldPrinciple.Text = TextBoxTaxWithheldPrinciple.Text.Substring(0, TextBoxTaxWithheldPrinciple.Text.Length - 1)
                '  TextBoxWithheldInterest.Text = TextBoxWithheldInterest.Text.Substring(0, TextBoxWithheldInterest.Text.Length - 1)
                g_dataset_DisbursementDetails = Session("dsDetails")
                If Not g_dataset_DisbursementDetails Is Nothing Then
                    If TextBoxTaxablePrinciple.Text <> "" Then
                        g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("TaxPrinciple") = Convert.ToDecimal(TextBoxTaxablePrinciple.Text)
                    End If
                    If TextBoxNonTaxablePrinciple.Text <> "" Then
                        g_dataset_DisbursementDetails.Tables("DisbDetails").Rows(l_integer_GridIndex).Item("NonTaxPrinciple") = Convert.ToDecimal(TextBoxNonTaxablePrinciple.Text)
                    End If
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
        Try

            Me.DataGridNewTransacts.SelectedIndex = Me.DataGridTransacts.SelectedIndex
            Session("CurIndex") = Me.DataGridNewTransacts.SelectedIndex
            Session("TDate") = Me.DataGridNewTransacts.Items(Me.DataGridNewTransacts.SelectedIndex).Cells(4).Text
            Me.DataGridTransacts.SelectedIndex = Me.DataGridNewTransacts.SelectedIndex

            FillTextBoxes()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)


        End Try


    End Sub

    Private Sub DataGridDisbursementDetails_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridDisbursementDetails.ItemCommand
        Try
            If (e.CommandName = "Select") Then
                Dim l_button_select As ImageButton
                l_button_select = e.Item.FindControl("ImageButtonSelect2")
                ' If e.Item.ItemIndex = Me.DataGridDisbursementDetails.SelectedIndex And Me.DataGridDisbursementDetails.SelectedIndex >= 0 Then
                l_button_select.ImageUrl = "images\selected.gif"
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try

    End Sub

    Private Sub DataGridNewTransacts_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridNewTransacts.ItemCommand
        Try
            Dim i As Integer
            Dim l_button_select As ImageButton
            Dim l_button_select2 As ImageButton
            If (e.CommandName = "Select") Then

                l_button_select = e.Item.FindControl("ImageButtonSelect")
                ' If e.Item.ItemIndex = Me.DataGridTransacts.SelectedIndex And Me.DataGridTransacts.SelectedIndex >= 0 Then
                l_button_select.ImageUrl = "images\selected.gif"
                'End If
            End If
            While i < Me.DataGridNewTransacts.Items.Count
                l_button_select = New ImageButton
                l_button_select2 = New ImageButton
                If i <> e.Item.ItemIndex Then
                    l_button_select = Me.DataGridNewTransacts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select2 = Me.DataGridTransacts.Items(i).FindControl("ImageButtonSel")
                    l_button_select.ImageUrl = "images\select.gif"
                    l_button_select2.ImageUrl = "images\select.gif"
                Else
                    l_button_select2 = Me.DataGridTransacts.Items(i).FindControl("ImageButtonSel")
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
                    l_button_select2 = Me.DataGridNewTransacts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select.ImageUrl = "images\select.gif"
                    l_button_select2.ImageUrl = "images\select.gif"
                Else
                    l_button_select2 = Me.DataGridNewTransacts.Items(i).FindControl("ImageButtonSelect")
                    l_button_select2.ImageUrl = "images\selected.gif"
                End If
                i = i + 1
            End While

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message,False)

        End Try
    End Sub
End Class
