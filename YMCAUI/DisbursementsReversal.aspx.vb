'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	AddEditQDRO.aspx.vb
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Public Class DisbursementsReversal
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
    Protected WithEvents ButtonSelectNone As System.Web.UI.WebControls.Button
    Protected WithEvents LabelFactorGroup As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridDisbursementReversal As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DropDownStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ButtonNone As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_dataset_dsStatusList As New DataSet
    Dim g_dataset_Disbursements As New DataSet


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        '   Me.DataGridDisbursementReversal.DataSource = CommonModule.CreateDataSource
        '    Me.DataGridDisbursementReversal.DataBind()
        Dim l_string_PersId As String
        Dim l_string_Disbid As String
        Dim l_dataset_Status As DataSet
        Dim l_boolean_Vested As Boolean
        Dim l_boolean_Terminated As Boolean
        Dim l_boolean_Status As Boolean
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If (Request.QueryString.Get("PersId").ToString() <> "") Then
                l_string_PersId = Request.QueryString.Get("PersId").ToString()
            End If
            If (Request.QueryString.Get("DisbId").ToString() <> "") Then
                l_string_Disbid = Request.QueryString.Get("DisbId").ToString()
            End If


            '**************************Hard Coded Values****************

            'l_string_PersId = "8C2D88E0-609D-47E6-A022-B51116933FD4"
            'l_string_Disbid = "E9371383-39A3-41A5-A48E-DD365006177C"
            g_dataset_dsStatusList = YMCARET.YmcaBusinessObject.DisbursementReversalBOClass.GetStatusList()
            If (g_dataset_dsStatusList.Tables("StatusList").Rows.Count > 0) Then
                Me.DropDownStatus.DataSource = g_dataset_dsStatusList
                Me.DropDownStatus.DataTextField = g_dataset_dsStatusList.Tables("StatusList").Columns("Description").ColumnName
                Me.DropDownStatus.DataValueField = g_dataset_dsStatusList.Tables("StatusList").Columns("FundStatus").ColumnName
                Me.DropDownStatus.DataBind()
            End If

            g_dataset_Disbursements = YMCARET.YmcaBusinessObject.DisbursementReversalBOClass.GetDisbursementsByPersId(l_string_PersId, "0")

            l_dataset_Status = YMCARET.YmcaBusinessObject.DisbursementReversalBOClass.GetStatusByPersId(l_string_PersId)
            If l_dataset_Status.Tables("Status").Rows.Count > 0 Then
                l_boolean_Status = l_dataset_Status.Tables("Status").Rows(0).Item("Status").ToString()
                l_boolean_Terminated = l_dataset_Status.Tables("Status").Rows(0).Item("Terminated").ToString()
                l_boolean_Vested = l_dataset_Status.Tables("Status").Rows(0).Item("Vested").ToString()
            End If

            If l_boolean_Status And Not l_boolean_Terminated Then
                Me.DropDownStatus.SelectedValue = "TM"
                Me.DropDownStatus.Enabled = False
            ElseIf l_boolean_Status And l_boolean_Terminated And l_boolean_Vested Then
                Me.DropDownStatus.SelectedValue = "DF"
                Me.DropDownStatus.Enabled = False
            ElseIf Not l_boolean_Vested And l_boolean_Status And l_boolean_Terminated Then
                Me.DropDownStatus.SelectedValue = "XM"
                Me.DropDownStatus.Enabled = False

            End If
            If Not IsPostBack Then


                Dim l_datarow_disb As DataRow
                Dim l_string_IssueDate As String
                For Each l_datarow_disb In g_dataset_Disbursements.Tables("DisbursementsByPersId").Rows
                    If l_datarow_disb.Item("DisbursementID").ToString() = l_string_Disbid Then
                        l_string_IssueDate = l_datarow_disb.Item("IssuedDate").ToString()
                        Exit For
                    End If
                Next
                g_dataset_Disbursements.Tables.Add("Disb")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("DisbursementID")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("Type")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("Number")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("AccountDate")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("IssuedDate")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("Amount")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("PayMethod")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("CurrencyCode")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("Voided")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("Reversed")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("LegalEntityType")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("LegalEntityName")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("LegalEntityAddress")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("PayeeAddress")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("EFTType")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("AcctNumber")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("BankInfo")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("ActionCode")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("ActionNotes")
                g_dataset_Disbursements.Tables("Disb").Columns.Add("FinalAction")

                For Each l_datarow_disb In g_dataset_Disbursements.Tables("DisbursementsByPersId").Rows
                    If (l_datarow_disb.Item("IssuedDate").ToString() = l_string_IssueDate) And (l_datarow_disb.Item("Reversed").ToString() = "No" Or l_datarow_disb.Item("IssuedDate").ToString() = "") Then
                        g_dataset_Disbursements.Tables("Disb").ImportRow(l_datarow_disb)
                    End If
                Next
                Me.DataGridDisbursementReversal.DataSource = g_dataset_Disbursements.Tables("Disb")
                Me.DataGridDisbursementReversal.DataBind()
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)


        End Try

    End Sub

    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        Try
            ButtonNone.Visible = True
            ButtonSelectAll.Visible = False
            ButtonSave.Enabled = True

            Dim objItem As DataGridItem
            For Each objItem In Me.DataGridDisbursementReversal.Items
                ' Ignore invalid items
                If objItem.ItemType <> ListItemType.Header And _
                  objItem.ItemType <> ListItemType.Footer And _
                objItem.ItemType <> ListItemType.Pager Then
                    ' Retrieve the value of the check box
                    Dim l_CheckBox_Select As New System.Web.UI.WebControls.CheckBox
                    l_CheckBox_Select = objItem.Cells(0).FindControl("CheckBoxSelect")
                    l_CheckBox_Select.Checked = True

                End If
            Next


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try

    End Sub

    Private Sub DataGridDisbursementReversal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridDisbursementReversal.ItemDataBound
        Try
            e.Item.Cells(1).Visible = False
            e.Item.Cells(11).Visible = False
            e.Item.Cells(12).Visible = False
            e.Item.Cells(13).Visible = False
            e.Item.Cells(14).Visible = False
            e.Item.Cells(15).Visible = False
            e.Item.Cells(16).Visible = False
            e.Item.Cells(17).Visible = False
            e.Item.Cells(18).Visible = False
            e.Item.Cells(19).Visible = False
            e.Item.Cells(20).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try


    End Sub

    Private Sub ButtonNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNone.Click
        Try
            ButtonSelectAll.Visible = True
            ButtonNone.Visible = False
            Dim objItem As DataGridItem
            For Each objItem In Me.DataGridDisbursementReversal.Items
                ' Ignore invalid items
                If objItem.ItemType <> ListItemType.Header And _
                  objItem.ItemType <> ListItemType.Footer And _
                objItem.ItemType <> ListItemType.Pager Then
                    ' Retrieve the value of the check box
                    Dim l_CheckBox_Select As New System.Web.UI.WebControls.CheckBox
                    l_CheckBox_Select = objItem.Cells(0).FindControl("CheckBoxSelect")
                    l_CheckBox_Select.Checked = False

                End If
            Next

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try

    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
            Dim objItem As DataGridItem
            Dim l_string_disbId As String
            Dim l_string_PartNewStatus As String
            Dim l_dataset_WithholdingInfoForRev As New DataSet
            Dim l_string_FundEventId As String
            Dim l_string_WHAmount As String
            Dim l_string_Gross As String
            Dim l_string_DisId As String
            Dim l_string_PersId As String
            If (Request.QueryString.Get("PersId").ToString() <> "") Then
                l_string_PersId = Request.QueryString.Get("PersId").ToString()
            End If
            If (Request.QueryString.Get("DisbId").ToString() <> "") Then
                l_string_DisId = Request.QueryString.Get("DisbId").ToString()
            End If


            For Each objItem In Me.DataGridDisbursementReversal.Items
                ' Ignore invalid items
                If objItem.ItemType <> ListItemType.Header And _
                  objItem.ItemType <> ListItemType.Footer And _
                objItem.ItemType <> ListItemType.Pager Then
                    ' Retrieve the value of the check box
                    Dim l_CheckBox_Select As New System.Web.UI.WebControls.CheckBox
                    Dim l_Label_Id As New System.Web.UI.WebControls.Label
                    l_CheckBox_Select = objItem.Cells(0).FindControl("CheckBoxSelect")
                    l_Label_Id = objItem.Cells(0).FindControl("LabelDisbId")
                    If l_CheckBox_Select.Checked = True Then
                    Else
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select all disbursements for reversal.", MessageBoxButtons.OK, True)
                        ButtonSelectAll.Visible = True
                        ButtonNone.Visible = False

                        Exit Sub
                    End If
                    If l_string_disbId <> "" Then
                        l_string_disbId = l_string_disbId + ",'" + l_Label_Id.Text + "'"
                    Else
                        l_string_disbId = "'" + l_Label_Id.Text + "'"
                        'l_string_disbId = l_Label_Id.Text.Trim()
                    End If
                End If
            Next
            '  l_string_disbId = "'" + l_string_disbId + "'"
            Session("IdForReversal") = l_string_disbId
            If DropDownStatus.SelectedValue = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select new status for participant/retiree.", MessageBoxButtons.OK)
            Else

                l_string_PartNewStatus = DropDownStatus.SelectedValue
                l_dataset_WithholdingInfoForRev = YMCARET.YmcaBusinessObject.DisbursementReversalBOClass.GetWithholdingInfoForRev(l_string_disbId)
                If (l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows.Count > 0) Then
                    If (l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("FundEventID").ToString() <> "System.DBNull" And l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("FundEventID").ToString() <> "") Then
                        l_string_FundEventId = CType(l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("FundEventID"), String)
                    Else
                        l_string_FundEventId = ""
                    End If
                    If (l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("GrossAmount").ToString() <> "System.DBNull" And l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("GrossAmount").ToString() <> "") Then
                        l_string_Gross = CType(l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("GrossAmount"), String)
                    Else
                        l_string_Gross = ""
                    End If
                    If (l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("WithHoldingAmount").ToString() <> "System.DBNull" And l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("WithHoldingAmount").ToString() <> "") Then
                        l_string_WHAmount = CType(l_dataset_WithholdingInfoForRev.Tables("WithholdingInfoForRev").Rows(0).Item("WithHoldingAmount"), String)
                    Else
                        l_string_WHAmount = ""
                    End If
                    Dim popupScript As String = "<script language='javascript'>" & _
                                                          "window.open('DisbursementsBreakDown.aspx?PersId=" + l_string_PersId + "&DisbId=" + l_string_DisId + "&FundId=" + l_string_FundEventId + "&WHAmount=" + l_string_WHAmount + "&Gross=" + l_string_Gross + "&Status=REVERSE" + "&PStatus=" + l_string_PartNewStatus + "', 'CustomPopUp', " & _
                                                          "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                                          "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript00")) Then
                        Page.RegisterStartupScript("PopupScript00", popupScript)
                    End If

                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)


        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim closeWindow1 As String = "<script language='javascript'>" & _
                                                 "window.close()" & _
                                                 "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow1")) Then
            Page.RegisterStartupScript("CloseWindow1", closeWindow1)
        End If
    End Sub
End Class
