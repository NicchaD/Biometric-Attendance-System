'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	DeferredPayment.aspx.vb
' Author Name		:	Neeraj Singh
' Employee ID		:	53212
' Email				:	neeraj.singh@3i-infotech.com
' Contact No		:	
' Creation Time		:	22-Dec-2009
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*****************************************************
'Modification History
'*****************************************************
'Name               Date        Comment
'*****************************************************
'Ashish Srivastava  2010.02.02  Resolve bug reclaculate total
'Ashish Srivastava  2010.02.9   Resolve BT-1110
'Manthan Rajguru    2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N           2019.03.27  YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'*******************************************************************************
#Region "Imports"
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
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Globalization

#End Region

Public Class DeferredPayment
    Inherits System.Web.UI.Page

#Region "Web Form Designer Generated Code "


    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonSelectNone As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPay As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxFundIdNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents txtRecalculateTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonRecalculate As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridDeferredPayment As System.Web.UI.WebControls.DataGrid
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
#End Region

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DeferredPayment.aspx")


    Enum DataGridDeferredPaymentColumns
        CheckBoxSelect = 0
        intFundIdNo = 1
        dtmScheduledPaymentDate = 2
        chvRolloverOptions = 3
        intInstPercentage = 4
        chvLastName = 5
        chvFirstName = 6
        mnyParticipantInsAmt = 7
        chvRolloverInstitutionName = 8
        mnyRollOverInsAmt = 9
        Edit = 10
        guiRefRequestID = 11
        guiFundEventID = 12
        intInstallmentID = 13
    End Enum

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Private Property SessionDeferred_PaymentList() As DataSet
        Get
            If Not Session("Deferred_PaymentList") Is Nothing Then
                Return (CType(Session("Deferred_PaymentList"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("Deferred_PaymentList") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtDef_PaymentList As DataTable
        Dim dsDef_PaymentList As DataSet
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()


            If Not Session("DP_PopUpClose") Is Nothing AndAlso CType(Session("DP_PopUpClose"), Boolean) = True Then
                Session("DP_PopUpClose") = Nothing
                LoadDeferredPaymentData(True)
            End If
            If Not Page.IsPostBack Then
                ClearSessions()
                LoadDeferredPaymentData(False)

            End If

            If Not Request.Form("Yes") Is Nothing Then
                If Request.Form("Yes") = "Yes" Then
                    If Session("Def_Processing") = True Then
                        Session("Def_Processing") = Nothing
                        DoDeferredPaymentProcess()
                    End If

                End If
            ElseIf Not Request.Form("No") Is Nothing Then
                ButtonPay.Enabled = True
            End If
            CheckReadOnlyMode() 'Shilpa N | 03/27/2019 | Calling the method

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Function GetDeferredPaymentList() As DataSet
        Try
            Return YMCARET.YmcaBusinessObject.DeferredPayment.GetDeferredPaymentList()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function LoadDeferredPaymentData(ByVal paramisPopUpClose As Boolean)
        Dim dtDef_PaymentList As DataTable
        Dim dsDef_PaymentList As DataSet
        Dim l_dtWithHoldingDeduction As DataTable
        Try
            If Not paramisPopUpClose Then
                dsDef_PaymentList = GetDeferredPaymentList()
            Else
                If SessionDeferred_PaymentList Is Nothing Then Exit Function
                dsDef_PaymentList = SessionDeferred_PaymentList
            End If

            If Session("dtWithHoldingDeduction") Is Nothing Then
                l_dtWithHoldingDeduction = CratetTableWithHoldingDeduction()
                Session("dtWithHoldingDeduction") = l_dtWithHoldingDeduction
            End If
            'Load datagrid
            'dtDef_PaymentList.Columns.Add("Selected")
            If Not dsDef_PaymentList Is Nothing Then


                BindDataGridDeferredPayment(dsDef_PaymentList, paramisPopUpClose)

                'Dim l_recalculate As Decimal
                'If (Not dsDef_PaymentList.Tables("dtDefPaymentList") Is Nothing) AndAlso (dsDef_PaymentList.Tables("dtDefPaymentList").Select("Selected=1").Length) > 0 Then
                '    l_recalculate = Math.Round(Convert.ToDecimal(dsDef_PaymentList.Tables("dtDefPaymentList").Compute("SUM(mnyParticipantInsAmt)+SUM(mnyRollOverInsAmt)", "Selected=1")), 2)
                'End If
                DisplayRecalculateTotal()
                'Dim l_double_sumTotal As Double
                'Dim nfi As New CultureInfo("en-US", False)
                'nfi.NumberFormat.CurrencyGroupSeparator = ","
                'nfi.NumberFormat.CurrencySymbol = "$"
                'nfi.NumberFormat.CurrencyDecimalDigits = 2
                'Me.txtRecalculateTotal.Text = System.Convert.ToString(l_recalculate.ToString("C", nfi)).Trim()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Sorting in Datagrid
    Private Sub DataGridDeferredPayment_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridDeferredPayment.SortCommand
        Try
            If Not SessionDeferred_PaymentList Is Nothing Then
                Dim l_dtDef_PaymentList As DataTable = SessionDeferred_PaymentList.Tables("dtDefPaymentList")
                '--------------------------------------------------------------------------------------------
                'Keeping Checked record in arraylist to maintain the checkbox selection after sorting
                Dim l_CheckedList As ArrayList = New ArrayList
                For Each itm As DataGridItem In DataGridDeferredPayment.Items
                    Dim l_blnChecked As Boolean
                    l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                    If l_blnChecked Then
                        l_CheckedList.Add(itm.Cells(DataGridDeferredPaymentColumns.guiFundEventID).Text.ToString())
                    End If
                Next
                DataGridDeferredPayment.SelectedIndex = -1


                If HelperFunctions.isNonEmpty(l_dtDef_PaymentList) Then
                    If Not ViewState("previousCheckGridSortExpression") Is Nothing AndAlso ViewState("previousCheckGridSortExpression") <> "" Then
                        'If SortExpression is not the same as the previous one then initialize new one
                        If (e.SortExpression <> ViewState("previousCheckGridSortExpression")) Then
                            ViewState("previousCheckGridSortExpression") = e.SortExpression
                            l_dtDef_PaymentList.DefaultView.Sort = e.SortExpression + " ASC"
                        Else
                            'else toggle existing sort expression
                            l_dtDef_PaymentList.DefaultView.Sort = IIf(l_dtDef_PaymentList.DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                        End If
                    Else
                        'First time in the sort function
                        l_dtDef_PaymentList.DefaultView.Sort = e.SortExpression + " ASC"
                        ViewState("previousCheckGridSortExpression") = e.SortExpression
                    End If

                    Me.DataGridDeferredPayment.DataSource = Nothing
                    Me.DataGridDeferredPayment.DataSource = l_dtDef_PaymentList.DefaultView
                    Me.DataGridDeferredPayment.DataBind()

                End If

                'Checking all check box by getting id from arraylist in which checked id is stored before sorting
                Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
                Dim dgi As DataGridItem
                If DataGridDeferredPayment.Items.Count > 0 Then
                    For Each dgi In DataGridDeferredPayment.Items
                        CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                        If l_CheckedList.Contains(dgi.Cells(DataGridDeferredPaymentColumns.guiFundEventID).Text.ToString()) Then
                            CheckBoxSelect.Checked = True
                        Else
                            CheckBoxSelect.Checked = False
                        End If
                    Next
                End If
            End If

            '------------------------------------------------------------------------

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub


    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Dim dtDef_PaymentList As DataTable
        Dim l_drFoundRow As DataRow()
        Dim l_CheckBox As CheckBox
        Dim l_strFilter As String
        Try
            'bind datagrid
            If Not SessionDeferred_PaymentList Is Nothing Then

                dtDef_PaymentList = SessionDeferred_PaymentList.Tables("dtDefPaymentList")
                If Not Me.DataGridDeferredPayment Is Nothing AndAlso Me.DataGridDeferredPayment.Items.Count > 0 Then
                    For Each l_DataGridItem As DataGridItem In Me.DataGridDeferredPayment.Items
                        l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                        If Not l_CheckBox Is Nothing Then
                            l_strFilter = "guiRefRequestID='" + l_DataGridItem.Cells(DataGridDeferredPaymentColumns.guiRefRequestID).Text.ToString() + "' AND intInstallmentID ='" + l_DataGridItem.Cells(DataGridDeferredPaymentColumns.intInstallmentID).Text.ToString() + "'"
                            l_drFoundRow = dtDef_PaymentList.Select(l_strFilter)
                            If l_drFoundRow.Length > 0 Then
                                l_drFoundRow(0).Item("Selected") = l_CheckBox.Checked
                            End If

                        End If
                    Next

                End If

                Me.DataGridDeferredPayment.DataSource = Nothing
                Me.DataGridDeferredPayment.DataSource = dtDef_PaymentList
                Me.DataGridDeferredPayment.DataBind()

                If Me.DataGridDeferredPayment.Items.Count > 0 Then
                    'Dim l_dataTable As DataTable
                    Dim l_double_sumTotal As Decimal

                    Dim l_FundIdValid As Integer = 0
                    Dim nfi As New CultureInfo("en-US", False)

                    If TextBoxFundIdNo.Text = String.Empty Then
                        DataGridDeferredPayment.DataSource = dtDef_PaymentList
                        DataGridDeferredPayment.DataBind()
                    Else

                        For l_DataRow As Integer = 0 To DataGridDeferredPayment.Items.Count - 1
                            If DataGridDeferredPayment.Items(l_DataRow).Cells(DataGridDeferredPaymentColumns.intFundIdNo).Text.Trim = TextBoxFundIdNo.Text.Trim Then
                                DataGridDeferredPayment.Items(l_DataRow).BackColor = System.Drawing.Color.LightBlue
                                l_FundIdValid = 1
                            End If
                        Next
                        If l_FundIdValid = 1 Then
                            For Each l_DataGridItem As DataGridItem In Me.DataGridDeferredPayment.Items
                                l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                                If l_DataGridItem.Cells(DataGridDeferredPaymentColumns.intFundIdNo).Text.Trim = TextBoxFundIdNo.Text.Trim Then
                                    l_CheckBox.Checked = True
                                    'Added to Register the Javascript for setting the focus in the grid
                                    SetFocus(l_CheckBox)
                                    'Added to Register the Javascript for setting the focus in the grid
                                End If
                                If l_CheckBox.Checked = True Then
                                    'check below line ?? which column value to be summed up
                                    'l_double_sumTotal += Convert.ToDecimal(IIf(l_DataGridItem.Cells(DataGridDeferredPaymentColumns.mnyRollOverInsAmt).Text = String.Empty, 0, l_DataGridItem.Cells(DataGridDeferredPaymentColumns.mnyRollOverInsAmt).Text))
                                    'l_double_sumTotal += Convert.ToDecimal(IIf(l_DataGridItem.Cells(DataGridDeferredPaymentColumns.mnyRollOverInsAmt).Text = String.Empty, 0, l_DataGridItem.Cells(DataGridDeferredPaymentColumns.mnyParticipantInsAmt).Text))

                                    l_strFilter = "guiRefRequestID='" + l_DataGridItem.Cells(DataGridDeferredPaymentColumns.guiRefRequestID).Text.ToString() + "' AND intInstallmentID ='" + l_DataGridItem.Cells(DataGridDeferredPaymentColumns.intInstallmentID).Text.ToString() + "'"
                                    l_drFoundRow = dtDef_PaymentList.Select(l_strFilter)
                                    If l_drFoundRow.Length > 0 Then
                                        l_drFoundRow(0).Item("Selected") = l_CheckBox.Checked
                                    End If


                                End If
                            Next
                            'check below lines are required or not
                            'Me.TextBoxboxNetAmount.Text = System.Convert.ToString(l_double_sumTotal.ToString("C", nfi)).Trim()
                            'Me.SessionDoubleTotalNetAmount = l_double_sumTotal
                        Else
                            MessageBox.Show(MessageBoxPlaceHolder, "Invalid Fund Id", "Fund Id " + TextBoxFundIdNo.Text + " not found ", MessageBoxButtons.OK)
                            TextBoxFundIdNo.Text = String.Empty
                        End If

                    End If
                    DisplayRecalculateTotal()
                End If 'DataGridDeferredPayment.Items.Count >0

            End If 'SessionDeferred_PaymentList is not nothing
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Added to Register the Javascript for setting the focus in the grid
    Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)
        Try
            Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ClientID & "').focus() </SCRIPT>"
            Page.RegisterStartupScript("focus", s)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ButtonRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRecalculate.Click
        Dim l_CheckBox As CheckBox

        Dim l_recalculate As Decimal = 0
        Try
            If Not SessionDeferred_PaymentList Is Nothing Then
                Dim dtDef_PaymentList As DataTable = SessionDeferred_PaymentList.Tables("dtDefPaymentList")
                Dim l_CheckedList As ArrayList = New ArrayList
                Dim l_strChecked As String

                For Each itm As DataGridItem In DataGridDeferredPayment.Items
                    Dim l_blnChecked As Boolean
                    l_CheckBox = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox)
                    If Not l_CheckBox Is Nothing Then
                        'l_CheckedList.Add(itm.Cells(DataGridDeferredPaymentColumns.guiRefRequestID).Text.ToString())
                        Dim l_strFilter As String = "guiRefRequestID='" + itm.Cells(DataGridDeferredPaymentColumns.guiRefRequestID).Text.ToString() + "' AND intInstallmentID ='" + itm.Cells(DataGridDeferredPaymentColumns.intInstallmentID).Text.ToString() + "'"
                        Dim l_FoundDataRow As DataRow() = dtDef_PaymentList.Select(l_strFilter)
                        If l_FoundDataRow.Length > 0 Then
                            l_FoundDataRow(0).Item("Selected") = l_CheckBox.Checked
                            'l_strChecked = "'" + itm.Cells(DataGridDeferredPaymentColumns.guiRefRequestID).Text.ToString() + "',"
                        End If
                    End If
                Next
                'If dtDef_PaymentList.Select("Selected=1").Length > 0 Then
                '    l_recalculate = Math.Round(Convert.ToDecimal(dtDef_PaymentList.Compute("SUM(mnyParticipantInsAmt)+SUM(mnyRollOverInsAmt)", "Selected=1")), 2)
                'End If


                DisplayRecalculateTotal()
                'Dim l_double_sumTotal As Double
                'Dim nfi As New CultureInfo("en-US", False)
                'nfi.NumberFormat.CurrencyGroupSeparator = ","
                'nfi.NumberFormat.CurrencySymbol = "$"
                'nfi.NumberFormat.CurrencyDecimalDigits = 2

                'Me.txtRecalculateTotal.Text = System.Convert.ToString(l_recalculate.ToString("C", nfi)).Trim()
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Function BindDataGridDeferredPayment(ByRef paramDsDefPaymentList As DataSet, ByVal paraIsPopupClose As Boolean)
        Try
            If Not paramDsDefPaymentList Is Nothing AndAlso paramDsDefPaymentList.Tables.Count = 2 Then

                Dim l_dtDefPaymentList As DataTable = paramDsDefPaymentList.Tables("dtDefPaymentList")
                Dim l_dtInstallmentTransactdetail As DataTable = paramDsDefPaymentList.Tables("dtInstallmentTransactdetail")

                'Add columns to datatable
                l_dtInstallmentTransactdetail = AddColumnsToInstallmentTranDtlDataTable(l_dtInstallmentTransactdetail)

                If Not l_dtDefPaymentList Is Nothing AndAlso l_dtDefPaymentList.Rows.Count > 0 AndAlso (Not l_dtInstallmentTransactdetail Is Nothing) Then
                    For Each dr As DataRow In l_dtDefPaymentList.Rows
                        'Dim drDetailArray As DataRow() = l_dtInstallmentTransactdetail.Select("guiRefRequestID='" + l_strrefReuestid + "'" + "and  intInstallmentID ='" + l_intInstallmentID + "'")
                        'GetCalculatedRolloverInstallment(dr, drDetailArray)
                        If Not paraIsPopupClose Then
                            ComputeRolloverInstallment(dr, l_dtInstallmentTransactdetail)
                        Else
                            UpdateTaxDeduction(dr, l_dtInstallmentTransactdetail)
                        End If
                        'withholding deduction
                        DeductWithHoldingDeduction(dr, l_dtInstallmentTransactdetail)
                    Next
                End If



                Me.DataGridDeferredPayment.DataSource = Nothing
                Me.DataGridDeferredPayment.DataSource = l_dtDefPaymentList
                Me.DataGridDeferredPayment.DataBind()
                If l_dtDefPaymentList.Rows.Count > 0 Then
                    ButtonFind.Enabled = True
                    ButtonRecalculate.Enabled = True
                    ButtonPay.Enabled = True
                    ButtonSelectNone.Enabled = True
                Else
                    ButtonFind.Enabled = False
                    ButtonRecalculate.Enabled = False
                    ButtonPay.Enabled = False
                    ButtonSelectNone.Enabled = False
                End If
                If Not SessionDeferred_PaymentList Is Nothing Then
                    SessionDeferred_PaymentList = Nothing
                End If
                SessionDeferred_PaymentList = paramDsDefPaymentList
            End If



        Catch ex As Exception
            Throw
        End Try
    End Function
    'Add columns to InstallmentTransactDetail Data table
    Private Function AddColumnsToInstallmentTranDtlDataTable(ByVal paramdtInstallmentTransactdetail As DataTable) As DataTable
        Dim l_dcColumn As DataColumn
        Try
            l_dcColumn = New DataColumn("mnyPersonalAfterTaxDedn", GetType(System.Decimal))
            l_dcColumn.DefaultValue = 0
            If Not paramdtInstallmentTransactdetail.Columns.Contains("mnyYmcaPreAfterTaxDedn") Then
                paramdtInstallmentTransactdetail.Columns.Add(l_dcColumn)
            End If
            l_dcColumn = Nothing

            l_dcColumn = New DataColumn("mnyYmcaPreAfterTaxDedn", GetType(System.Decimal))
            l_dcColumn.DefaultValue = 0
            If Not paramdtInstallmentTransactdetail.Columns.Contains("mnyYmcaPreAfterTaxDedn") Then
                paramdtInstallmentTransactdetail.Columns.Add(l_dcColumn)
            End If
            paramdtInstallmentTransactdetail.AcceptChanges()
            Return paramdtInstallmentTransactdetail
        Catch ex As Exception
            Throw ex
        Finally
            paramdtInstallmentTransactdetail = Nothing
        End Try
    End Function
    Private Function ComputeRolloverInstallment(ByRef paramDrDeferredPaymentList As DataRow, ByRef paramdtInstallmentTransactdetail As DataTable)
        Dim drInstallmentTranDtlArray As DataRow()
        Try
            Dim l_RollOverInsAmt As Decimal = 0
            Dim l_ParticipantInsAmt As Decimal = 0
            Dim l_strrefReuestid As String = Convert.ToString(paramDrDeferredPaymentList("guiRefRequestID"))
            Dim l_intInstallmentID As Int64 = Convert.ToInt32(paramDrDeferredPaymentList("intInstallmentID"))
            'Find the transaction detail rows
            Dim qstring As String = "guiTransactionRefID='" + l_strrefReuestid + "' AND intInstallmentID ='" + l_intInstallmentID.ToString() + "'"

            drInstallmentTranDtlArray = paramdtInstallmentTransactdetail.Select(qstring)
            Dim l_totalTaxableInstallmentTranDtl As Decimal = 0
            Dim l_totalNonTaxableInstallmentTranDtl As Decimal = 0

            For Each drInstallmentTranDtl As DataRow In drInstallmentTranDtlArray
                l_totalTaxableInstallmentTranDtl += Convert.ToDecimal(drInstallmentTranDtl.Item("mnyPersonalPreTax")) + Convert.ToDecimal(drInstallmentTranDtl.Item("mnyYmcaPreTax"))
                l_totalNonTaxableInstallmentTranDtl += Convert.ToDecimal(drInstallmentTranDtl.Item("mnyPersonalPostTax"))
            Next


            For Each drInstallmentTranDtl As DataRow In drInstallmentTranDtlArray
                Dim l_datarow As DataRow = CreateInstallmentRollOverTransactDetailRow(drInstallmentTranDtl, paramdtInstallmentTransactdetail)
                If Not l_datarow Is Nothing Then
                    If UpdateInstallMentTransactDetailRow(l_totalTaxableInstallmentTranDtl, l_totalNonTaxableInstallmentTranDtl, paramDrDeferredPaymentList, drInstallmentTranDtl, l_datarow) Then
                        'Add row in case of PARTIAL,ROLLOVERALL and TAXABle only
                        paramdtInstallmentTransactdetail.Rows.Add(l_datarow)
                    End If
                    'Add new row with payeeEntitytype code as Rollover to datatable InstallmentTransactdetail 
                    l_RollOverInsAmt += Convert.ToDecimal(l_datarow.Item("mnyPersonalPreTax")) + Convert.ToDecimal(l_datarow.Item("mnyYmcaPretax")) + Convert.ToDecimal(l_datarow.Item("mnyPersonalPostTax"))
                    'l_ParticipantInsAmt += Convert.ToDecimal(drInstallmentTranDtl.Item("mnyPersonalAfterTaxDedn")) + Convert.ToDecimal(drInstallmentTranDtl.Item("mnyYmcaPreAfterTaxDedn")) + Convert.ToDecimal(drInstallmentTranDtl.Item("mnyPersonalPostTax"))
                    l_ParticipantInsAmt += Convert.ToDecimal(drInstallmentTranDtl.Item("mnyPersonalPreTax")) + Convert.ToDecimal(drInstallmentTranDtl.Item("mnyYmcaPretax")) + Convert.ToDecimal(drInstallmentTranDtl.Item("mnyPersonalPostTax"))
                    l_datarow = Nothing
                    drInstallmentTranDtl = Nothing
                End If
            Next

            paramdtInstallmentTransactdetail.AcceptChanges()

            paramDrDeferredPaymentList.Item("mnyRollOverInsAmt") = Math.Round(l_RollOverInsAmt, 2)
            paramDrDeferredPaymentList.Item("mnyParticipantInsAmt") = Math.Round(l_ParticipantInsAmt, 2)

        Catch ex As Exception
            Throw ex
        Finally
            drInstallmentTranDtlArray = Nothing
        End Try
    End Function

    Private Function CreateInstallmentRollOverTransactDetailRow(ByVal paramdrInstallmentTranDtl As DataRow, ByVal paramdtInstallmentTransactdetail As DataTable) As DataRow
        Dim l_drInstallmentTranDtl As DataRow
        Try
            l_drInstallmentTranDtl = paramdtInstallmentTransactdetail.NewRow()
            'copy data into new data row
            For i As Int16 = 0 To paramdtInstallmentTransactdetail.Columns.Count - 1
                l_drInstallmentTranDtl.Item(i) = paramdrInstallmentTranDtl.Item(i)
            Next
            l_drInstallmentTranDtl.Item("chvPayeeEntityTypeCode") = "ROLINS"
            Return l_drInstallmentTranDtl
        Catch ex As Exception
            Throw ex
        Finally
            l_drInstallmentTranDtl = Nothing
        End Try
    End Function

    Private Function UpdateInstallMentTransactDetailRow(ByVal paramtotalTaxableInstallmentTranDtl As Decimal, ByVal paramtotalNonTaxableInstallmentTranDtl As Decimal, ByVal paramDrDeferredPaymentList As DataRow, ByRef paramdrOldRow As DataRow, ByRef paramdrNewRow As DataRow) As Boolean
        Try
            Dim l_RollOverOption As String = Convert.ToString(paramDrDeferredPaymentList("chvRolloverOptions"))

            Select Case l_RollOverOption.ToUpper()
                Case "PARTIAL"
                    Dim l_mnyPersonalPreTax As Decimal = Convert.ToDecimal(paramdrOldRow.Item("mnyPersonalPreTax"))
                    Dim l_mnyYmcaPretax As Decimal = Convert.ToDecimal(paramdrOldRow.Item("mnyYmcaPretax"))
                    Dim l_mnyPersonalPostTax As Decimal = Convert.ToDecimal(paramdrOldRow.Item("mnyPersonalPostTax"))

                    Dim l_RollOverTaxablePect As Decimal = Convert.ToDecimal(paramDrDeferredPaymentList("mnyRolloverTaxablePect"))
                    Dim l_RollOverNonTaxablePect As Decimal = Convert.ToDecimal(paramDrDeferredPaymentList("mnyRolloverNonTaxablePect"))
                    Dim l_DefTotalRollOverAmount As Decimal = Convert.ToDecimal(paramDrDeferredPaymentList("mnyDefRolloverAmt"))

                    Dim l_DefNonTaxableRollOverAmount As Decimal = l_DefTotalRollOverAmount * l_RollOverNonTaxablePect / 100
                    Dim l_DefTaxableRollOverAmount As Decimal = l_DefTotalRollOverAmount * l_RollOverTaxablePect / 100

                    Dim l_DefRolloverNonTaxable As Decimal = 0
                    Dim l_DefRolloverYmcaPreTax As Decimal = 0
                    Dim l_DefRolloverPersonalPreTax As Decimal = 0
                    Dim l_DefParticipantNonTaxable As Decimal = 0
                    Dim l_DefParticipantTaxable As Decimal = 0

                    If paramtotalTaxableInstallmentTranDtl <> 0 Then

                        paramdrNewRow.Item("mnyPersonalPreTax") = l_mnyPersonalPreTax * l_DefTaxableRollOverAmount / paramtotalTaxableInstallmentTranDtl
                        paramdrNewRow.Item("mnyYmcaPretax") = l_mnyYmcaPretax * l_DefTaxableRollOverAmount / paramtotalTaxableInstallmentTranDtl
                    Else
                        paramdrNewRow.Item("mnyPersonalPreTax") = 0
                        paramdrNewRow.Item("mnyYmcaPretax") = 0
                    End If
                    If paramtotalNonTaxableInstallmentTranDtl <> 0 Then
                        paramdrNewRow.Item("mnyPersonalPostTax") = l_mnyPersonalPostTax * l_DefNonTaxableRollOverAmount / paramtotalNonTaxableInstallmentTranDtl
                    Else
                        paramdrNewRow.Item("mnyPersonalPostTax") = 0
                    End If


                Case "ROLLOVERALL"
                Case "ALLTAXABLE"
                    paramdrNewRow.Item("mnyPersonalPostTax") = 0
                Case "NONE"
                    paramdrNewRow.Item("mnyPersonalPreTax") = 0
                    paramdrNewRow.Item("mnyYmcaPretax") = 0
                    paramdrNewRow.Item("mnyPersonalPostTax") = 0

            End Select

            paramdrOldRow.Item("mnyPersonalPreTax") = Convert.ToDecimal(paramdrOldRow.Item("mnyPersonalPreTax")) - Convert.ToDecimal(paramdrNewRow.Item("mnyPersonalPreTax"))
            paramdrOldRow.Item("mnyYmcaPretax") = Convert.ToDecimal(paramdrOldRow.Item("mnyYmcaPretax")) - Convert.ToDecimal(paramdrNewRow.Item("mnyYmcaPretax"))
            paramdrOldRow.Item("mnyPersonalPostTax") = Convert.ToDecimal(paramdrOldRow.Item("mnyPersonalPostTax")) - Convert.ToDecimal(paramdrNewRow.Item("mnyPersonalPostTax"))

            'to do :get tax rate from datarow
            computeTaxDeduction(paramdrOldRow, Convert.ToDecimal(paramDrDeferredPaymentList.Item("mnyTaxRatePct")))

            If l_RollOverOption.Equals("NONE") Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function computeTaxDeduction(ByRef paramDrInstallMentTranDtl As DataRow, ByVal paramTaxRate As Decimal)
        Try
            Dim l_personalTaxDeduction As Decimal = Convert.ToDecimal(paramDrInstallMentTranDtl.Item("mnyPersonalPreTax")) * paramTaxRate / 100
            Dim l_ymcaTaxDeduction As Decimal = Convert.ToDecimal(paramDrInstallMentTranDtl.Item("mnyYmcaPretax")) * paramTaxRate / 100

            'paramDrInstallMentTranDtl.Item("mnyPersonalPreTax") = 
            'paramDrInstallMentTranDtl.Item("mnyYmcaPretax") = 

            'paramDrInstallMentTranDtl.Item("mnyPersonalAfterTaxDedn") = Math.Round(Convert.ToDecimal(paramDrInstallMentTranDtl.Item("mnyPersonalPreTax")) - l_personalTaxDeduction, 2)
            'paramDrInstallMentTranDtl.Item("mnyYmcaPreAfterTaxDedn") = Math.Round(Convert.ToDecimal(paramDrInstallMentTranDtl.Item("mnyYmcaPretax")) - l_ymcaTaxDeduction, 2)
            'paramDrInstallMentTranDtl.Item("mnyTaxDeduction") = Math.Round(l_personalTaxDeduction + l_ymcaTaxDeduction, 2)

            paramDrInstallMentTranDtl.Item("mnyPersonalAfterTaxDedn") = Convert.ToDecimal(paramDrInstallMentTranDtl.Item("mnyPersonalPreTax")) - l_personalTaxDeduction
            paramDrInstallMentTranDtl.Item("mnyYmcaPreAfterTaxDedn") = Convert.ToDecimal(paramDrInstallMentTranDtl.Item("mnyYmcaPretax")) - l_ymcaTaxDeduction

            paramDrInstallMentTranDtl.Item("mnyTaxDeduction") = l_personalTaxDeduction + l_ymcaTaxDeduction


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub ButtonPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPay.Click

        Dim l_SelectedRecAmtTotal As Decimal = 0
        Dim l_RecalculateTotal As Decimal = 0
        Dim l_RecCount As Int32 = 0
        Dim l_ParticipantInstAmt As Decimal = 0
        Dim l_RolloverInstAmt As Decimal = 0
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Sub



            Dim dtDef_PaymentList As DataTable = SessionDeferred_PaymentList.Tables("dtDefPaymentList")
            If Not Me.DataGridDeferredPayment Is Nothing AndAlso Me.DataGridDeferredPayment.Items.Count > 0 Then
                Dim l_CheckBox As CheckBox
                For Each dgItem As DataGridItem In Me.DataGridDeferredPayment.Items

                    l_ParticipantInstAmt = 0
                    l_RolloverInstAmt = 0
                    l_CheckBox = CType(dgItem.FindControl("CheckBoxSelect"), CheckBox)
                    If Not l_CheckBox Is Nothing Then
                        If l_CheckBox.Checked Then
                            l_RecCount += 1
                            If Not dgItem.Cells(DataGridDeferredPaymentColumns.mnyParticipantInsAmt).Text.Equals(String.Empty) Then
                                l_ParticipantInstAmt = Convert.ToDecimal(dgItem.Cells(DataGridDeferredPaymentColumns.mnyParticipantInsAmt).Text)
                            End If
                            If Not dgItem.Cells(DataGridDeferredPaymentColumns.mnyRollOverInsAmt).Text.Equals(String.Empty) Then
                                l_RolloverInstAmt = Convert.ToDecimal(dgItem.Cells(DataGridDeferredPaymentColumns.mnyRollOverInsAmt).Text)

                            End If
                            l_SelectedRecAmtTotal += Math.Round(l_ParticipantInstAmt + l_RolloverInstAmt, 2)

                        End If
                    End If

                Next

            End If


            'Validation for select atleast one record
            If l_RecCount = 0 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Please select atleast one record for processing.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'If Not dtDef_PaymentList Is Nothing AndAlso dtDef_PaymentList.Select("Selected=1").Length > 0 Then
            '    l_SelectedRecAmtTotal = Math.Round(Convert.ToDecimal(dtDef_PaymentList.Compute("SUM(mnyParticipantInsAmt)+ SUM(mnyRollOverInsAmt)", "Selected=1")), 2)
            'End If
            If Not txtRecalculateTotal.Text.Equals(String.Empty) Then

                l_RecalculateTotal = Math.Round(Convert.ToDecimal(txtRecalculateTotal.Text.Replace("$", "")), 2)
            End If
            'Recalculate validation
            If Not l_SelectedRecAmtTotal = l_RecalculateTotal Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "There is a mismatch on the Total Net Amount, Please verify. Click Recalculate button to avoid this mismatch before the PAY.", MessageBoxButtons.Stop)
                Exit Sub
            End If


            If dtDef_PaymentList.Select("Selected=1").Length > 0 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Are you sure you want to process selected deferred payment installment?", MessageBoxButtons.YesNo)
                Session("Def_Processing") = True
                ButtonPay.Enabled = False
                Exit Sub
            End If






        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Function CreateRefRolloverInstitutionDataTable(ByVal paramDrDpPaymentList As DataRow, ByVal paramRefRolloverInstitutionsID As String)
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function
            If Not Session("DP_DataSet") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                l_DataRow = l_DataSet.Tables("RefRollOverInstitution").NewRow()

                l_DataRow.Item("guiRefRequestsID") = paramDrDpPaymentList.Item("guiRefRequestID")
                l_DataRow.Item("guiRolloverinstitutionId") = paramRefRolloverInstitutionsID
                l_DataRow.Item("mnyRollOverAmount") = paramDrDpPaymentList.Item("mnyDefRolloverAmt")
                l_DataRow.Item("bitIsActive") = 1
                l_DataRow.Item("mnyTaxablePercentage") = paramDrDpPaymentList.Item("mnyRolloverTaxablePect")
                l_DataRow.Item("mnyNonTaxablePercentage") = paramDrDpPaymentList.Item("mnyRolloverNonTaxablePect")

                l_DataSet.Tables("RefRollOverInstitution").Rows.Add(l_DataRow)
                l_DataSet.Tables("RefRollOverInstitution").AcceptChanges()
                Session("DP_DataSet") = l_DataSet
            End If
        Catch ex As Exception
            Throw ex
        Finally
            l_DataSet = Nothing
            l_DataRow = Nothing
        End Try
    End Function

    Private Function CreateDeferredPaymentTableSchema()
        Dim l_DataSetSchema As DataSet
        Try
            l_DataSetSchema = YMCARET.YmcaBusinessObject.DeferredPayment.GetDeferredPaymentTableSchemas()
            If l_DataSetSchema Is Nothing Then Exit Function

            Session("DP_DataSet") = l_DataSetSchema
            'Session("DP_RefundTransaction") = l_DataSetSchema
            'Session("DP_Transactions") = l_DataSetSchema.Tables("Transactions")
            'Session("DP_DataSet") = l_DataSetSchema.Tables("Disbursements")
            'Session("DP_DisbursementDetails") = l_DataSetSchema.Tables("DisbursementDetails")
            'Session("DP_DisbursementWithholding") = l_DataSetSchema.Tables("DisbursementWithholding")
            'Session("DP_DisbursementRefunds") = l_DataSetSchema.Tables("DisbursementRefunds")
        Catch ex As Exception
            Throw ex
        Finally
            l_DataSetSchema = Nothing
        End Try
    End Function

    Private Function MakeDataTableForDPDisbursement(ByRef paramdrDPListRow As DataRow)
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Dim l_strPayeeDisbursementID As String
        Dim l_strRolinsDisbursementID As String
        Dim l_guiTransactID As String = String.Empty
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function
            If Not Session("DP_DataSet") Is Nothing Then

                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                Dim dtDP_installmentTranDtl As DataTable = SessionDeferred_PaymentList.Tables("dtInstallmentTransactdetail")

                Dim drFoundPersonTranDtlRows As DataRow() = dtDP_installmentTranDtl.Select("guiTransactionRefID='" + Convert.ToString(paramdrDPListRow.Item("guiRefRequestID")) + "' AND chvPayeeEntityTypeCode ='PERSON' AND intInstallmentID='" + Convert.ToString(paramdrDPListRow.Item("intInstallmentID")) + "'")
                Dim drFoundRolINSTranDtlRows As DataRow() = dtDP_installmentTranDtl.Select("guiTransactionRefID='" + Convert.ToString(paramdrDPListRow.Item("guiRefRequestID")) + "' AND chvPayeeEntityTypeCode ='ROLINS' AND intInstallmentID='" + Convert.ToString(paramdrDPListRow.Item("intInstallmentID")) + "'")

                'Create Person Disbursement Record
                l_strPayeeDisbursementID = System.Guid.NewGuid.ToString()
                Dim l_isPersDisbursementCreated As Boolean = CreateDisbursement("PERSON", l_strPayeeDisbursementID, paramdrDPListRow, dtDP_installmentTranDtl)

                If l_isPersDisbursementCreated Then
                    'Create Disbusement Detail record
                    CreateDisbursementDetail(l_strPayeeDisbursementID, drFoundPersonTranDtlRows, paramdrDPListRow)
                    'Create Refund record
                    CreateDisdusmentRefunds(l_strPayeeDisbursementID, paramdrDPListRow("guiRefRequestID").ToString(), Convert.ToInt64(paramdrDPListRow("intInstallmentID")))
                    'Create applicable Federal Withhoding record
                    CreateFedTaxWithHodings(l_strPayeeDisbursementID, drFoundPersonTranDtlRows)
                    'create withholdings deductions if deducted from Payee1
                    If Convert.ToDecimal(paramdrDPListRow("mnyPayeeWHDeduction")) > 0 Then
                        CreateDiductionWithHodings(l_strPayeeDisbursementID, paramdrDPListRow)
                    End If

                    If drFoundPersonTranDtlRows.Length > 0 Then

                        For Each drInstTrnsactDlt As DataRow In drFoundPersonTranDtlRows
                            l_guiTransactID = String.Empty
                            l_guiTransactID = CreateTransactions(drInstTrnsactDlt)
                            If Not l_guiTransactID Is Nothing AndAlso Not l_guiTransactID.Equals(String.Empty) Then
                                CreateAtsRefundRecord(l_guiTransactID, l_strPayeeDisbursementID, paramdrDPListRow, drInstTrnsactDlt)
                            End If
                        Next
                    End If
                End If

                'Create RollOver Disbursement Record
                l_strRolinsDisbursementID = System.Guid.NewGuid.ToString()
                Dim l_isRolinsDisbursementCreated As Boolean = CreateDisbursement("ROLINS", l_strRolinsDisbursementID, paramdrDPListRow, dtDP_installmentTranDtl)

                If l_isRolinsDisbursementCreated Then
                    CreateDisbursementDetail(l_strRolinsDisbursementID, drFoundRolINSTranDtlRows, paramdrDPListRow)
                    CreateDisdusmentRefunds(l_strRolinsDisbursementID, paramdrDPListRow("guiRefRequestID").ToString(), Convert.ToInt64(paramdrDPListRow("intInstallmentID")))
                    'create withholdings deductions if deducted from Payee1
                    If Convert.ToDecimal(paramdrDPListRow("mnyRolloverWHDeduction")) > 0 Then
                        CreateDiductionWithHodings(l_strRolinsDisbursementID, paramdrDPListRow)
                    End If
                    'to do create atsrefunds
                    If drFoundRolINSTranDtlRows.Length > 0 Then

                        For Each drInstTrnsactDlt As DataRow In drFoundRolINSTranDtlRows
                            l_guiTransactID = String.Empty
                            l_guiTransactID = CreateTransactions(drInstTrnsactDlt)
                            If Not l_guiTransactID Is Nothing AndAlso Not l_guiTransactID.Equals(String.Empty) Then
                                CreateAtsRefundRecord(l_guiTransactID, l_strRolinsDisbursementID, paramdrDPListRow, drInstTrnsactDlt)
                            End If
                        Next
                    End If
                End If

                l_DataSet.AcceptChanges()
                Session("DP_DataSet") = l_DataSet
            End If

        Catch ex As Exception
            Throw ex
        Finally
            l_DataSet = Nothing
            ''l_DataTable = Nothing
        End Try
    End Function
    Private Function CreateDisbursement(ByVal paramPayeeEntityTypeCode As String, ByVal paramstrDisbursementID As String, ByVal paramdrDPListRow As DataRow, ByVal paramdtDP_installmentTranDtl As DataTable) As Boolean
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Try
            If Not Session("DP_DataSet") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                Dim drFoundTranDtlRows As DataRow() = paramdtDP_installmentTranDtl.Select("guiTransactionRefID='" + Convert.ToString(paramdrDPListRow.Item("guiRefRequestID")) + "' AND chvPayeeEntityTypeCode ='" + paramPayeeEntityTypeCode + "' AND intInstallmentID='" + Convert.ToString(paramdrDPListRow.Item("intInstallmentID")) + "'")
                If drFoundTranDtlRows.Length > 0 Then
                    l_DataRow = CreateDataRowForDisbursement(paramstrDisbursementID, paramdrDPListRow, drFoundTranDtlRows, paramPayeeEntityTypeCode)
                    If Not l_DataRow Is Nothing Then
                        l_DataSet.Tables("Disbursements").Rows.Add(l_DataRow)
                        l_DataSet.Tables("Disbursements").AcceptChanges()
                        Session("DP_DataSet") = l_DataSet
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If

        Catch ex As Exception
            Throw ex
        Finally
            l_DataSet = Nothing
            l_DataRow = Nothing
        End Try
    End Function
    Private Function CreateDataRowForDisbursement(ByVal paramstrDisbursementID As String, ByRef paramdrDPListRow As DataRow, ByVal paramdrDPTranDtl As DataRow(), ByVal paramEntityType As String) As DataRow
        Dim l_datarow As DataRow
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function


            Dim l_TotalTaxable As Decimal = 0
            Dim l_NonTotalTaxable As Decimal = 0
            For Each dr1 As DataRow In paramdrDPTranDtl
                l_TotalTaxable += Convert.ToDecimal(dr1.Item("mnyPersonalPreTax")) + Convert.ToDecimal(dr1.Item("mnyYmcaPreTax"))
                l_NonTotalTaxable += Convert.ToDecimal(dr1.Item("mnyPersonalPostTax"))
            Next
            'for disbursement total money should be >=0
            If l_TotalTaxable + l_NonTotalTaxable <= 0 Then Return Nothing

            Dim l_DataSet As DataSet = CType(Session("DP_DataSet"), DataSet)
            If Not l_DataSet Is Nothing Then

                l_datarow = l_DataSet.Tables("Disbursements").NewRow()
                l_datarow.Item("PayeeEntityID") = IIf(paramEntityType.Equals("PERSON"), Convert.ToString(paramdrDPListRow.Item("guiPersID")), Convert.ToString(paramdrDPListRow.Item("guiRolloverinstitutionId")))
                l_datarow.Item("PayeeAddrID") = paramdrDPListRow.Item("guiAddressID")
                l_datarow.Item("PayeeEntityTypeCode") = paramEntityType
                l_datarow.Item("DisbursementType") = "REF"
                l_datarow.Item("IrsTaxTypeCode") = ""
                l_datarow.Item("TaxableAmount") = l_TotalTaxable
                l_datarow.Item("NonTaxableAmount") = l_NonTotalTaxable
                l_datarow.Item("PaymentMethodCode") = paramdrDPListRow.Item("chvPaymentMethodCode")
                l_datarow.Item("CurrencyCode") = paramdrDPListRow.Item("chrCurrencyCode")
                l_datarow.Item("PersID") = paramdrDPListRow.Item("guiPersID")
                l_datarow.Item("Rollover") = ""
                l_datarow.Item("UniqueID") = paramstrDisbursementID
            End If
            Return l_datarow
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function CreateDisbursementDetail(ByVal paramstrDisbursementID As String, ByVal paramdrFoundPersonTranDtlRows As DataRow(), ByVal paramdrDPListRow As DataRow) As Boolean
        Dim l_datarow As DataRow
        Dim l_ClubbedDataTable As DataTable
        Dim targetColumns As String()
        Dim compareColumns As String()
        Dim clubbedColumns As String()
        Dim mnyTaxablePrincipal As Decimal = 0
        Dim mnyTaxableInterest As Decimal = 0
        Dim mnyNonTaxablePrincipal As Decimal = 0
        Dim mnyTaxWithheldPrincipal As Decimal = 0
        Dim mnyTaxWithheldInterest As Decimal = 0
        Dim mnyPersonalPreTax As Decimal = 0
        Dim mnyPersonalPostTax As Decimal = 0
        Dim mnyYmcaPreTax As Decimal = 0
        Dim mnyTaxDeduction As Decimal = 0




        Dim drFoundTransactDtl As DataRow()
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function
            If paramdrFoundPersonTranDtlRows.Length > 0 Then

                Dim dtTransactDtl As DataTable = SessionDeferred_PaymentList.Tables("dtInstallmentTransactdetail")
                Dim dtTmpTransactDtl As DataTable = New DataTable
                dtTmpTransactDtl = dtTransactDtl.Clone()

                Dim l_DataSet As DataSet = CType(Session("DP_DataSet"), DataSet)
                If Not l_DataSet Is Nothing Then

                    For Each dr As DataRow In paramdrFoundPersonTranDtlRows
                        dtTmpTransactDtl.ImportRow(dr)
                    Next

                    targetColumns = Nothing
                    compareColumns = New String() {"chrAcctType", "guiTransactionRefID", "intInstallmentID", "chrTransactType", "chvPayeeEntityTypeCode", "chrAcctBreakdownType", "intSortOrder"}
                    clubbedColumns = New String() {"mnyPersonalPreTax", "mnyPersonalPostTax", "mnyYmcaPreTax", "mnyTaxDeduction", "mnyDeduction"}
                    l_ClubbedDataTable = YMCARET.YmcaBusinessObject.DeferredPayment.SelectDistinct(dtTmpTransactDtl, targetColumns, compareColumns, clubbedColumns)
                    Dim l_dtDintinctBreakDownCode As DataTable

                    compareColumns = New String() {"chrAcctType", "chrAcctBreakdownType", "intSortOrder"}
                    clubbedColumns = Nothing
                    clubbedColumns = Nothing
                    l_dtDintinctBreakDownCode = YMCARET.YmcaBusinessObject.DeferredPayment.SelectDistinct(l_ClubbedDataTable, targetColumns, compareColumns, clubbedColumns)
                    For Each drDistictBCode As DataRow In l_dtDintinctBreakDownCode.Rows
                        Dim strFilter As String = String.Empty
                        mnyTaxablePrincipal = 0
                        mnyTaxableInterest = 0
                        mnyNonTaxablePrincipal = 0
                        mnyTaxWithheldPrincipal = 0
                        mnyTaxWithheldInterest = 0
                        mnyPersonalPreTax = 0
                        mnyPersonalPostTax = 0
                        mnyYmcaPreTax = 0
                        mnyTaxDeduction = 0

                        strFilter = "chrAcctType='" + drDistictBCode.Item("chrAcctType").ToString() + "' AND chrAcctBreakdownType='" + drDistictBCode.Item("chrAcctBreakdownType").ToString() + "' AND intSortOrder='" + drDistictBCode.Item("chrAcctBreakdownType").ToString() + "'"

                        If l_ClubbedDataTable.Select(strFilter + " AND chrTransactType='MWPR'").Length > 0 Then
                            mnyPersonalPreTax = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyPersonalPreTax)", strFilter + " AND chrTransactType='MWPR'"))
                            mnyYmcaPreTax = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyYmcaPreTax)", strFilter + " AND chrTransactType='MWPR'"))
                            mnyTaxablePrincipal = Math.Round((mnyPersonalPreTax + mnyYmcaPreTax), 2)

                            mnyNonTaxablePrincipal = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyPersonalPostTax)", strFilter + " AND chrTransactType='MWPR'"))
                            mnyTaxDeduction = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyTaxDeduction)", strFilter + " AND chrTransactType='MWPR'"))
                            mnyTaxWithheldPrincipal = Math.Round(mnyTaxDeduction, 2)

                        End If
                        If l_ClubbedDataTable.Select(strFilter + " AND chrTransactType='MWIN'").Length > 0 Then
                            mnyPersonalPreTax = 0
                            mnyYmcaPreTax = 0
                            mnyTaxDeduction = 0
                            mnyPersonalPreTax = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyPersonalPreTax)", strFilter + " AND chrTransactType='MWIN'"))
                            mnyYmcaPreTax = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyYmcaPreTax)", strFilter + " AND chrTransactType='MWIN'"))
                            mnyTaxableInterest = Math.Round(mnyPersonalPreTax + mnyYmcaPreTax, 2)
                            mnyTaxDeduction = Convert.ToDecimal(l_ClubbedDataTable.Compute("SUM(mnyTaxDeduction)", strFilter + " AND chrTransactType='MWIN'"))
                            mnyTaxWithheldInterest = Math.Round(mnyTaxDeduction, 2)
                        End If
                        l_datarow = l_DataSet.Tables("DisbursementDetails").NewRow()
                        l_datarow.Item("AcctType") = drDistictBCode.Item("chrAcctType")
                        l_datarow.Item("AcctBreakdownType") = drDistictBCode.Item("chrAcctBreakdownType")
                        l_datarow.Item("SortOrder") = drDistictBCode.Item("intSortOrder")
                        l_datarow.Item("TaxablePrincipal") = mnyTaxablePrincipal
                        l_datarow.Item("TaxableInterest") = mnyTaxableInterest
                        l_datarow.Item("NonTaxablePrincipal") = mnyNonTaxablePrincipal
                        l_datarow.Item("TaxWithheldPrincipal") = mnyTaxWithheldPrincipal
                        l_datarow.Item("TaxWithheldInterest") = mnyTaxWithheldInterest
                        l_datarow.Item("DisbursementID") = paramstrDisbursementID

                        l_DataSet.Tables("DisbursementDetails").Rows.Add(l_datarow)
                        l_DataSet.Tables("DisbursementDetails").AcceptChanges()
                    Next
                    Session("DP_DataSet") = l_DataSet
                End If
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function CreateTransactions(ByVal paramDrDPInstallmentTranDtl As DataRow) As String
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Dim l_dtTransact As DataTable
        Dim strTransactID As String = String.Empty
        Try
            If Not Session("DP_DataSet") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                l_dtTransact = l_DataSet.Tables("Transactions")

                If Not paramDrDPInstallmentTranDtl Is Nothing Then
                    strTransactID = System.Guid.NewGuid.ToString()
                    l_DataRow = l_dtTransact.NewRow()
                    l_DataRow("UniqueID") = strTransactID
                    l_DataRow("PersID") = paramDrDPInstallmentTranDtl("guiPersID")
                    l_DataRow("FundEventID") = paramDrDPInstallmentTranDtl("guiFundEventID")
                    l_DataRow("YmcaID") = System.DBNull.Value           '' We can add in store Proc Itself
                    l_DataRow("AcctType") = paramDrDPInstallmentTranDtl("chrAcctType")
                    l_DataRow("TransactType") = paramDrDPInstallmentTranDtl("chrTransactType")
                    l_DataRow("AnnuityBasisType") = paramDrDPInstallmentTranDtl("chrAnnuityBasisType")
                    l_DataRow("MonthlyComp") = "0.00"
                    l_DataRow("PersonalPreTax") = Convert.ToDecimal(paramDrDPInstallmentTranDtl("mnyPersonalPreTax")) * -1
                    l_DataRow("PersonalPostTax") = Convert.ToDecimal(paramDrDPInstallmentTranDtl("mnyPersonalPostTax")) * -1
                    l_DataRow("YmcaPreTax") = Convert.ToDecimal(paramDrDPInstallmentTranDtl("mnyYmcaPreTax")) * -1
                    l_DataRow("ReceivedDate") = Date.Now
                    l_DataRow("AccountingDate") = Date.Now          '' Needs to Get Account date. in SP we can do.
                    l_DataRow("TransactDate") = Date.Now
                    l_DataRow("FundedDate") = Date.Now              '' Needs to Get Account date. in SP we can do.
                    l_DataRow("TransmittalID") = System.DBNull.Value
                    l_DataRow("TransactionRefID") = paramDrDPInstallmentTranDtl("guiTransactionRefID")
                    l_dtTransact.Rows.Add(l_DataRow)
                    l_dtTransact.AcceptChanges()

                End If

            End If
            Return strTransactID
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function CreateAtsRefundRecord(ByVal paraTransactID As String, ByVal paraDisbursmentID As String, ByVal paraDpInstListRow As DataRow, ByVal paraTransactDtlRow As DataRow)
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Dim l_dtAtsRefunds As DataTable
        Try
            If Not Session("DP_DataSet") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                l_dtAtsRefunds = l_DataSet.Tables("AtsRefunds")
                If Not paraDpInstListRow Is Nothing AndAlso Not paraTransactDtlRow Is Nothing Then
                    Dim payeeName As String
                    l_DataRow = l_dtAtsRefunds.NewRow

                    l_DataRow("Uniqueid") = System.DBNull.Value                     '&& UniqueID
                    l_DataRow("RefRequestsID") = paraDpInstListRow.Item("guiRefRequestID")
                    l_DataRow("AcctType") = paraTransactDtlRow.Item("chrAcctType")                    '&& Account Type
                    l_DataRow("Taxable") = (Convert.ToDecimal(paraTransactDtlRow("mnyPersonalPreTax")) + Convert.ToDecimal(paraTransactDtlRow("mnyYmcaPreTax"))) * -1
                    l_DataRow("NonTaxable") = Convert.ToDecimal(paraTransactDtlRow("mnyPersonalPostTax")) * -1
                    l_DataRow("Tax") = Convert.ToDecimal(paraTransactDtlRow("mnyTaxDeduction")) * -1
                    l_DataRow("TaxRate") = paraDpInstListRow.Item("mnyTaxRatePct")
                    'Need to verify payee name for all 
                    ' If paraTransactDtlRow("chvPayeeEntityTypeCode").ToString().Equals("PERSON") Then
                    payeeName = paraDpInstListRow("chvLastName").ToString().Trim()
                    payeeName += IIf(paraDpInstListRow("chvMiddleName").ToString() = "", String.Empty, " " + paraDpInstListRow("chvMiddleName").ToString().Trim())
                    payeeName += IIf(paraDpInstListRow("chvFirstName").ToString() = "", String.Empty, " " + paraDpInstListRow("chvFirstName").ToString().Trim())
                    'Else
                    ' payeeName = paraDpInstListRow("chvRolloverInstitutionName").ToString().Trim()
                    'End If
                    l_DataRow("Payee") = payeeName
                    l_DataRow("FundedDate") = System.DBNull.Value

                    l_DataRow("RequestType") = paraTransactDtlRow("chrTransactType")
                    l_DataRow("TransactID") = paraTransactID
                    l_DataRow("AnnuityBasisType") = paraTransactDtlRow("chrAnnuityBasisType")
                    l_DataRow("DisbursementID") = paraDisbursmentID
                    l_DataRow("DeferredInstallmentNo") = paraDpInstListRow("intInstallmentNo")
                    l_dtAtsRefunds.Rows.Add(l_DataRow)
                    l_dtAtsRefunds.AcceptChanges()
                    Session("DP_DataSet") = l_DataSet
                End If

            End If

        Catch ex As Exception
            Throw ex

        End Try
    End Function
    '' DisbursementRefunds
    Private Function CreateDisdusmentRefunds(ByVal paraDisbursementID As String, ByVal paraRefRequestID As String, ByVal paramInstallmentID As Int64)
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Dim l_dtDisburmentRefunds As DataTable
        Try
            If Not Session("DP_DataSet") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                l_dtDisburmentRefunds = l_DataSet.Tables("DisbursementRefunds")
                l_DataRow = l_dtDisburmentRefunds.NewRow
                l_DataRow("guiRefRequestID") = paraRefRequestID
                l_DataRow("guiDisbursementID") = paraDisbursementID
                l_DataRow("intRefDeferredPaymentID") = paramInstallmentID

                l_dtDisburmentRefunds.Rows.Add(l_DataRow)
                l_dtDisburmentRefunds.AcceptChanges()
                Session("DP_DataSet") = l_DataSet
            End If

        Catch ex As Exception
            Throw (ex)
        End Try
    End Function
    Private Function CreateFedTaxWithHodings(ByVal paramPayeeDisbursementID As String, ByVal drFoundPersonTranDtlRows As DataRow())
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Dim l_dtDisbursementWithholding As DataTable
        Try
            If Not Session("DP_DataSet") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                l_dtDisbursementWithholding = l_DataSet.Tables("DisbursementWithholding")
                l_DataRow = l_dtDisbursementWithholding.NewRow
                l_DataRow("DisbursementID") = paramPayeeDisbursementID
                l_DataRow("WithholdingTypeCode") = "FEDTAX"
                Dim l_amount As Decimal = 0
                For Each dr As DataRow In drFoundPersonTranDtlRows
                    l_amount += Convert.ToDecimal(dr("mnyTaxDeduction"))
                Next
                l_DataRow("Amount") = l_amount
                l_dtDisbursementWithholding.Rows.Add(l_DataRow)
                l_dtDisbursementWithholding.AcceptChanges()

                Session("DP_DataSet") = l_DataSet
            End If

        Catch ex As Exception
            Throw (ex)
        End Try
    End Function
    Private Function CreateDiductionWithHodings(ByVal paramDisbursementID As String, ByVal paraDiferredListRow As DataRow)
        Dim l_DataSet As DataSet
        Dim l_DataRow As DataRow
        Dim l_dtDisbursementWithholding As DataTable
        Dim l_dtDeductionWHoldings As DataTable
        Dim l_drDeductionFoundRows As DataRow()
        Dim l_RefRequestId As String = String.Empty
        Dim l_InstallmentID As Int64 = 0
        Dim l_filter As String = String.Empty
        Try
            If Not Session("DP_DataSet") Is Nothing AndAlso Not Session("dtWithHoldingDeduction") Is Nothing Then
                l_DataSet = CType(Session("DP_DataSet"), DataSet)
                l_dtDisbursementWithholding = l_DataSet.Tables("DisbursementWithholding")
                l_dtDeductionWHoldings = CType(Session("dtWithHoldingDeduction"), DataTable)
                If l_dtDeductionWHoldings.Rows.Count > 0 Then
                    l_RefRequestId = paraDiferredListRow("guiRefRequestID").ToString()
                    l_InstallmentID = Convert.ToInt64(paraDiferredListRow("intInstallmentID"))
                    l_filter = "guiRefRequestID='" + l_RefRequestId + "'AND intInstallmentID='" + l_InstallmentID.ToString() + "'"

                    l_drDeductionFoundRows = l_dtDeductionWHoldings.Select(l_filter)
                    If l_drDeductionFoundRows.Length > 0 Then

                        For Each drDeductionRow As DataRow In l_drDeductionFoundRows
                            l_DataRow = l_dtDisbursementWithholding.NewRow
                            l_DataRow("DisbursementID") = paramDisbursementID
                            l_DataRow("WithholdingTypeCode") = drDeductionRow("chvWithholdingTypeCode").Trim()
                            l_DataRow("Amount") = drDeductionRow("mnyAmount")
                            l_dtDisbursementWithholding.Rows.Add(l_DataRow)
                            l_dtDisbursementWithholding.AcceptChanges()
                        Next
                        Session("DP_DataSet") = l_DataSet

                    End If

                End If
            End If



        Catch ex As Exception
            Throw (ex)
        End Try
    End Function
    Private Function ClearSessions()
        Session("DP_DataSet") = Nothing
        SessionDeferred_PaymentList = Nothing
        Session("Def_Processing") = Nothing
        Session("dtWithHoldingDeduction") = Nothing

    End Function
    Private Function DoDeferredPaymentProcess()
        Dim l_Found_Datatows As DataRow()
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function

            Dim dtDef_PaymentList As DataTable = SessionDeferred_PaymentList.Tables("dtDefPaymentList")

            If Me.DataGridDeferredPayment.Items.Count > 0 Then
                Dim l_CheckBox As CheckBox
                Dim l_CheckedList As ArrayList = New ArrayList
                Dim l_strChecked As String


                'Çreate datatable schema to save data
                CreateDeferredPaymentTableSchema()


                For Each itm As DataGridItem In DataGridDeferredPayment.Items
                    Dim l_blnChecked As Boolean
                    l_CheckBox = CType(itm.FindControl("CheckBoxSelect"), CheckBox)
                    If Not l_CheckBox Is Nothing Then
                        If l_CheckBox.Checked Then
                            Dim strRefID As String = itm.Cells(DataGridDeferredPaymentColumns.guiRefRequestID).Text.ToString()
                            Dim strInstallmetID As String = itm.Cells(DataGridDeferredPaymentColumns.intInstallmentID).Text.ToString()


                            l_Found_Datatows = dtDef_PaymentList.Select("guiRefRequestID='" + strRefID + "'AND intInstallmentID='" + strInstallmetID + "'")
                            If l_Found_Datatows.Length > 0 Then
                                If Not l_Found_Datatows(0).Item("chvRolloverInstitutionName") Is System.DBNull.Value AndAlso l_Found_Datatows(0).Item("chvRolloverInstitutionName").ToString() <> String.Empty Then

                                    Dim l_strRolloverInstitutionName As String = l_Found_Datatows(0).Item("chvRolloverInstitutionName").ToString().Trim()
                                    'GetInstitutionId to check whether institution Already exists or not
                                    Dim guiInstitutionIDToValidate = Convert.ToString(YMCARET.YmcaBusinessObject.DeferredPayment.GetRolloverInstitutionID(l_strRolloverInstitutionName)).Trim().ToUpper

                                    If Not Convert.ToString(l_Found_Datatows(0).Item("guiRolloverinstitutionId")).Trim().ToUpper.Equals(guiInstitutionIDToValidate) Then
                                        CreateRefRolloverInstitutionDataTable(l_Found_Datatows(0), guiInstitutionIDToValidate)
                                        l_Found_Datatows(0).Item("guiRolloverinstitutionId") = guiInstitutionIDToValidate
                                        dtDef_PaymentList.AcceptChanges()
                                    End If

                                End If
                                '***********************************************************************************************
                                '* Create Rows for Disbursements, Disbursement Details, Transacts and Refunds
                                '***********************************************************************************************
                                '***********************************************************************************************
                                '* Loop Through the Possible Annuity Basis Types
                                '***********************************************************************************************
                                'Me.CreateRowsForDisbursements(l_dec_TDAmount, l_dec_TDRequestedAmount)
                                MakeDataTableForDPDisbursement(l_Found_Datatows(0))
                                'to do add other data table to save

                            End If
                        End If
                    End If
                Next


                Dim l_DataSet As DataSet = CType(Session("DP_DataSet"), DataSet)
                If l_DataSet Is Nothing Then Exit Function
                Dim l_boolSuccess As Boolean = False
                l_boolSuccess = YMCARET.YmcaBusinessObject.DeferredPayment.SaveDeferredPayment(l_DataSet)
                If l_boolSuccess = True Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Deferred payment process completed successfully.", MessageBoxButtons.OK)
                    ClearSessions()
                    LoadDeferredPaymentData(False)
                    TextBoxFundIdNo.Text = String.Empty
                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            ClearSessions()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub ButtonSelectNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectNone.Click
        Dim l_Found_Datatows As DataRow()
        Dim l_bool_flag As Boolean
        Dim intSelected As Int32 = 0
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Sub

            Dim dtDef_PaymentList As DataTable = SessionDeferred_PaymentList.Tables("dtDefPaymentList")


            If Not Me.DataGridDeferredPayment Is Nothing AndAlso Me.DataGridDeferredPayment.Items.Count > 0 Then
                If ButtonSelectNone.Text = "Select None" Then
                    l_bool_flag = False
                    intSelected = 0
                    ButtonSelectNone.Text = "Select All"
                Else
                    l_bool_flag = True
                    intSelected = 1
                    ButtonSelectNone.Text = "Select None"
                End If
                'PopulateData()
                For Each l_DataRow As DataRow In dtDef_PaymentList.Rows
                    If Not (l_DataRow Is Nothing) Then
                        l_DataRow("Selected") = intSelected
                    End If
                Next
                DataGridDeferredPayment.DataSource = dtDef_PaymentList
                DataGridDeferredPayment.DataBind()
                DisplayRecalculateTotal()

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Function CratetTableWithHoldingDeduction() As DataTable
        Dim l_dtWitholdingDeduction As DataTable
        Dim l_Column As DataColumn
        Try
            l_dtWitholdingDeduction = New DataTable
            l_Column = New DataColumn("guiRefRequestID", GetType(System.String))
            l_dtWitholdingDeduction.Columns.Add(l_Column)
            l_Column = New DataColumn("intInstallmentID", GetType(System.Int64))
            l_dtWitholdingDeduction.Columns.Add(l_Column)
            l_Column = New DataColumn("chvWithholdingTypeCode", GetType(System.String))
            l_dtWitholdingDeduction.Columns.Add(l_Column)
            l_Column = New DataColumn("mnyAmount", GetType(System.Decimal))
            l_dtWitholdingDeduction.Columns.Add(l_Column)

            Return l_dtWitholdingDeduction
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Private Function DeductWithHoldingDeduction(ByRef paraDeferredListRow As DataRow, ByVal paradtTransactDtl As DataTable)
        Dim l_dtWithHoldingList As DataTable
        Dim l_filter As String = String.Empty
        Dim l_RefRequestId As String = String.Empty
        Dim l_InstallmentID As Int64 = 0
        Dim l_drWithHoldingFound As DataRow()
        Dim l_WithHoldingTotalAmt As Decimal = 0
        Dim l_netPersonBalance As Decimal = 0
        Dim l_netRollinsBalance As Decimal = 0
        Try

            If Session("dtWithHoldingDeduction") Is Nothing Then Exit Function
            l_RefRequestId = paraDeferredListRow("guiRefRequestID").ToString()
            l_InstallmentID = Convert.ToInt64(paraDeferredListRow("intInstallmentID"))
            l_dtWithHoldingList = CType(Session("dtWithHoldingDeduction"), DataTable)
            l_filter = "guiRefRequestID='" + l_RefRequestId + "'AND intInstallmentID='" + l_InstallmentID.ToString() + "'"

            If l_dtWithHoldingList.Select(l_filter).Length > 0 Then
                l_WithHoldingTotalAmt = Convert.ToDecimal(l_dtWithHoldingList.Compute("SUM(mnyAmount)", l_filter))
            End If
            l_filter = "guiTransactionRefID='" + l_RefRequestId + "'AND intInstallmentID='" + l_InstallmentID.ToString() + "'"

            If paradtTransactDtl.Select(l_filter + " AND chvPayeeEntityTypeCode='PERSON'").Length > 0 Then
                l_netPersonBalance = paradtTransactDtl.Compute("SUM(mnyPersonalAfterTaxDedn)+ SUM(mnyYmcaPreAfterTaxDedn)+ SUM(mnyPersonalPostTax) ", l_filter + " AND chvPayeeEntityTypeCode='PERSON'")
            End If

            If paradtTransactDtl.Select(l_filter + " AND chvPayeeEntityTypeCode='ROLINS'").Length > 0 Then
                l_netRollinsBalance = paradtTransactDtl.Compute("SUM(mnyPersonalPreTax)+ SUM(mnyYmcaPreTax) +SUM(mnyPersonalPostTax)", l_filter + " AND chvPayeeEntityTypeCode='ROLINS'")
            End If
            If l_WithHoldingTotalAmt > 0 Then
                If l_WithHoldingTotalAmt < l_netPersonBalance Then
                    paraDeferredListRow.Item("mnyPayeeWHDeduction") = l_WithHoldingTotalAmt
                    paraDeferredListRow.Item("mnyRolloverWHDeduction") = 0
                ElseIf l_WithHoldingTotalAmt < l_netRollinsBalance Then
                    paraDeferredListRow.Item("mnyPayeeWHDeduction") = 0
                    paraDeferredListRow.Item("mnyRolloverWHDeduction") = l_WithHoldingTotalAmt
                End If
            Else
                paraDeferredListRow.Item("mnyPayeeWHDeduction") = 0
                paraDeferredListRow.Item("mnyRolloverWHDeduction") = 0
            End If

        Catch ex As Exception
            Throw ex

        End Try
    End Function
    Private Function UpdateTaxDeduction(ByVal paraDeferredListRow As DataRow, ByVal paraDtTransactDtl As DataTable)
        Dim l_filter As String = String.Empty
        Dim l_RefRequestId As String = String.Empty
        Dim l_InstallmentID As Int64 = 0
        Dim l_drTransactFoundRows As DataRow()
        Dim l_TaxRate As Decimal = 0
        Try

            If paraDtTransactDtl Is Nothing Then Exit Function
            l_RefRequestId = paraDeferredListRow("guiRefRequestID").ToString()
            l_InstallmentID = Convert.ToInt64(paraDeferredListRow("intInstallmentID"))
            l_TaxRate = Convert.ToDecimal(paraDeferredListRow.Item("mnyTaxRatePct"))
            l_filter = "guiTransactionRefID='" + l_RefRequestId + "'AND intInstallmentID='" + l_InstallmentID.ToString() + "' AND chvPayeeEntityTypeCode='PERSON'"
            l_drTransactFoundRows = paraDtTransactDtl.Select(l_filter)

            If l_drTransactFoundRows.Length > 0 Then
                For Each drTransact As DataRow In l_drTransactFoundRows
                    computeTaxDeduction(drTransact, l_TaxRate)
                Next

            End If


        Catch ex As Exception

        End Try
    End Function
    Private Function DisplayRecalculateTotal()
        Dim l_recalculate As Decimal = 0
        Dim dsDef_PaymentList As DataSet
        Try
            If SessionDeferred_PaymentList Is Nothing Then Exit Function

            dsDef_PaymentList = SessionDeferred_PaymentList

            If (Not dsDef_PaymentList.Tables("dtDefPaymentList") Is Nothing) AndAlso (dsDef_PaymentList.Tables("dtDefPaymentList").Select("Selected=1").Length) > 0 Then
                l_recalculate = Math.Round(Convert.ToDecimal(dsDef_PaymentList.Tables("dtDefPaymentList").Compute("SUM(mnyParticipantInsAmt)+SUM(mnyRollOverInsAmt)", "Selected=1")), 2)
            End If
            Dim nfi As New CultureInfo("en-US", False)
            nfi.NumberFormat.CurrencyGroupSeparator = ","
            nfi.NumberFormat.CurrencySymbol = "$"
            nfi.NumberFormat.CurrencyDecimalDigits = 2
            Me.txtRecalculateTotal.Text = System.Convert.ToString(l_recalculate.ToString("C", nfi)).Trim()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'START: Shilpa N | 03/27/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonPay.Enabled = False
            ButtonPay.ToolTip = tooltip
        End If
    End Sub
    'END: Shilpa N | 03/27/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
