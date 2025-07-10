'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	ACHDebitExportForm.aspx.vb
' Author Name		:	Shubhrata T
' Employee ID		:	34774
' Email				:	 
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : Hafiz 03Feb06
'*******************************************************************************
'Changed By:        On:                  IssueId
'Aparna Samala      20/04/2007          Change in folder structure
'*******************************************************************************************************************************************************************************************************************
'Modification History
'*******************************************************************************************************************************************************************************************************************
'Modified By                  Date                              Description
'Ashutosh Patil               28-May-2007                       Confirmation MessageBox will be asked to the user on click of yes button on Yes-Record will be exported if found or present in datagrid
'                                                               on No - Nothing will happen or No action For avoiding the double click error YREN-3364 
'Ashutosh Patil               09-Jul-2007                       Deselect button's property enabled=false changed to true when records found.     
'Ashish Srivastava            22-Jan-2009                       Issue YRS 5.0-651 ,for exporting two files one for Bank other for YRS 
'Ashish Srivastava            16-Feb-2009                       Issue YRS 5.0-651 
'Neeraj Singh                   12/Nov/2009                     Added form name for security issue YRS 5.0-940 
'Ashish Srivastava           2010.06.24                         Enhancements08 changes 
'Priya                       19-August-2010                     YRS 5.0-1098:the export process requires a recalcualtion before funcitoning
'Anudeep                    2013-12-16                          BT:2311-13.3.0 Observations
'Manthan Rajguru            2015.09.16                          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja K                    2019.28.02                          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'*******************************************************************************************************************************************************************************************************************
Imports System.IO
Public Class ACHDebitExportForm
    Inherits System.Web.UI.Page
    Dim strFormName As String = New String("ACHDebitExportForm.aspx")


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    'Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonDeSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonExport As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents DatagridACHDebitExport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCount As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCount As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonPendingReport As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonReCalculate As System.Web.UI.WebControls.Button
    Protected WithEvents DataGrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Shared ReadOnly Property connectioninfo() As String
        Get
            'By Aparna -Change in folder structure
            'Return ConfigurationSettings.AppSettings("ACH")
            Return ConfigurationSettings.AppSettings("ACH") & "\\EXPORT"
        End Get

    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        'Menu1.DataBind()
        '  Response.Buffer = False

        'to reflect the changes in datagrid after editing

        Try

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)

            End If

            If IsPostBack = False Then
                Session("l_counter") = 0
            End If

            If Session("NewAmount") Then
                PopulateDataGridACHDebits()
                Session("NewAmount") = 0

            End If
            'may 25th.. to uncheck the check box on cancel button click
            If Session("cancel") = True Then
                SessionCancel()
            End If

            If Not Session("DataSetACHDebits") Is Nothing Then

                PopulateDataGridACHDebits()

                Session("DataSetACHDebits") = Nothing

            End If
            'for delete confirmation of rows from grid
            'Modified By Ashutosh Patil as on 28-May-2007
            'AA:16.12.2013 - BT:2311 Commented below code and kept in buton yes click event
            'If Session("ExportRecords") = False Then
            '    If Request.Form("Yes") = "Yes" Then
            '        DeleteFromACHDebit()
            '    End If
            'End If
            'for deleting a row if the date changes to future date

            Dim l_output As Integer
            l_output = Session("l_output")
            ' If Not Session("l_output") Is Nothing Then
            If l_output = 1 Then
                Dim l_guiuniqueid As String
                Dim l_dataset As DataSet
                Dim l_datarow As DataRow

                l_dataset = Session("DataSetACHDebitExport")
                l_guiuniqueid = Session("guiuniqueid")
                ' If l_output = 1 Then
                For Each l_datarow In l_dataset.Tables("ACHDebits").Rows

                    If l_datarow("UniqueId").ToString.ToUpper = l_guiuniqueid Then
                        l_datarow.Delete()
                        Exit For
                    End If
                Next
            End If

            'Added By Ashutosh Patil as on 28-May-2007
            'YREN-3364 
            'AA:16.12.2013 - BT:2311 Commented below code and kept in buton yes click event
            'If Session("ExportRecords") = True And Request.Form("Yes") = "Yes" Then
            '    Session("ExportRecords") = False
            '    Call ExportData()
            'End If
            If Not Me.IsPostBack Then
                PopulateDataGridACHDebits()
            End If
            CheckReadOnlyMode() 'PK | 02-28-2019 | YRS-AT-4248 | Check security method called here
        Catch ex As Exception
            'Added By Ashutosh Patil as on 28-May-2007
            'YREN-3364 
            Session("ExportRecords") = Nothing
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Dim l_counter As Integer
#Region "SessionCancelTrue"
    Private Sub SessionCancel()

        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem
        Try
            If DatagridACHDebitExport.Items.Count > 0 Then
                For Each dgi In DatagridACHDebitExport.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    CheckBoxSelect.Checked = False
                Next
                TextBoxTotal.Text = 0
                TextBoxCount.Text = 0
            End If

            DatagridACHDebitExport.SelectedIndex = -1
            Session("cancel") = Nothing

        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "FillDataGrid"
    Public Sub PopulateDataGridACHDebits()
        Dim l_DataSet_ACHDebits As DataSet
        Dim l_double_TotalAmount As Decimal
        Dim l_dv_AchExport As DataView
        Dim i_counter As Integer
        Try
            l_DataSet_ACHDebits = YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.GetPendingACHDebits()
            'Code added by ashutosh on 14 sep 2006
            ''Dim l_DataColoum As DataColumn
            Dim l_Arr_DataRow As DataRow()
            Dim l_TotalAmount As Decimal
            Dim i As Integer = 0
            Session("DataSetACHDebitExport") = l_DataSet_ACHDebits
            'Ashish:2010.06.24 ,maintain sort order 
            l_dv_AchExport = l_DataSet_ACHDebits.Tables(0).DefaultView
            If Not ViewState("DisbursementsListSort") Is Nothing Then
                l_dv_AchExport.Sort = ViewState("DisbursementsListSort")
            End If
            'DatagridACHDebitExport.DataSource = l_DataSet_ACHDebits.Tables(0).DefaultView
            DatagridACHDebitExport.DataSource = l_dv_AchExport
            DatagridACHDebitExport.DataBind()
            'DataGrid1.DataSource = l_DataSet_ACHDebits.Tables(0).DefaultView
            'DataGrid1.DataBind()

            'Code added by ashutosh on 15 sep 2006
            l_Arr_DataRow = l_DataSet_ACHDebits.Tables(0).Select("Selected = 1")
            For i = 0 To l_Arr_DataRow.Length - 1
                l_TotalAmount = l_TotalAmount + l_Arr_DataRow(i)("Amount")
            Next
            If l_TotalAmount > 0 Then
                ButtonExport.Enabled = True
            End If

            'Enable Delselect button if records found
            'Start Ashutosh Patil as on 09-Jul-2007
            If Not l_DataSet_ACHDebits Is Nothing Then
                'AA:16.12.2013 - BT:2311 Changed to use Select none and select all for single button
                'If l_DataSet_ACHDebits.Tables(0).Rows.Count > 0 Then
                '    Me.ButtonDeSelectAll.Enabled = True
                'End If
                If l_Arr_DataRow IsNot Nothing Then
                    If l_DataSet_ACHDebits.Tables(0).Rows.Count = l_Arr_DataRow.Length Then
                        ButtonSelectAll.Text = "Select None"
                    End If
                End If
            End If
            'End Ashutosh Patil as on 09-Jul-2007
            'commented & modified by hafiz on 15-Dec-2006
            'TextBoxTotal.Text = l_TotalAmount.ToString()
            TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()

            TextBoxCount.Text = i.ToString()
            'ashu End**********
            'Code added by ashutosh on 14 sep 2006
            'DataGrid1.DataSource = l_DataSet_ACHDebits.Tables(1)
            ' DataGrid1.DataBind()
            'ashu End**********
            'to get total amount in text box

            'If Not l_DataSet_ACHDebits Is Nothing Then
            '    For i_counter = 0 To l_DataSet_ACHDebits.Tables("ACHDebits").Rows.Count - 1
            '        l_double_TotalAmount = l_double_TotalAmount + Convert.ToDecimal(l_DataSet_ACHDebits.Tables("ACHDebits").Rows(i_counter).Item("Amount"))
            '    Next
            'End If
            'TextBoxTotal.Text = l_double_TotalAmount
            'TextBoxCount.Text = l_DataSet_ACHDebits.Tables("ACHDebits").Rows.Count
            'TextBoxTotal.Text = 0


        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "ButtonClicks"


    'AA:16.12.2013 - BT:2311 Commented for not to use button edit it is moved to grid row update
    'Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    '    Dim l_integer_RowIndex As Integer
    '    Dim l_counter As Integer
    '    Try

    '        'l_counter = Session("l_counter")
    '        'Code Commented by Ashutosh on 18 sep 
    '        'Dim new_counter As Integer
    '        ' new_counter = Session("l_counter")
    '        'If DatagridACHDebitExport.SelectedIndex <> -1 Then
    '        'If l_counter > 1 Then
    '        '    MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please Select A Single Record To Update.", MessageBoxButtons.Stop)
    '        'ElseIf l_counter = 0 Then

    '        '    MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please Select A Record To Edit.", MessageBoxButtons.Stop)
    '        'Else
    '        '**********************

    '        l_integer_RowIndex = GetSelectedDataGridRowIndex(l_counter, 0)
    '        DatagridACHDebitExport.SelectedIndex = l_integer_RowIndex

    '        If DatagridACHDebitExport.SelectedIndex > -1 Then
    '            Session("YmcaNo") = DatagridACHDebitExport.SelectedItem.Cells(2).Text
    '            Session("YmcaName") = DatagridACHDebitExport.SelectedItem.Cells(3).Text
    '            Session("UniqueId") = DatagridACHDebitExport.SelectedItem.Cells(4).Text
    '            Session("Amount") = DatagridACHDebitExport.SelectedItem.Cells(6).Text
    '            Session("PaymentDate") = DatagridACHDebitExport.SelectedItem.Cells(7).Text

    '            Dim popupScript As String
    '            popupScript = "<script language='javascript'>" & _
    '                                          "window.open('ACHDebitExportUpdateForm.aspx', 'CustomPopUp', " & _
    '                                          "'width=800, height=450, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
    '                                          "</script>"

    '            Page.RegisterStartupScript("PopupScript2", popupScript)
    '            'Else

    '            ' MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select an account to update.", MessageBoxButtons.Stop)
    '        End If

    '        'End If

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try

    'End Sub
    Private Function GetSelectedDataGridRowIndex(ByRef p_integer_SelectedRowCount As Integer, ByVal p_integer_Mode As Integer) As Integer
        Dim dgItem As DataGridItem
        Dim chkbox As System.Web.UI.WebControls.CheckBox
        Dim l_integer_RowIndex As Integer = 0
        Dim l_integer_RowCounter As Integer = 0
        'p_integer_Mode = 0 Means Edit Record, 1 Means Delete Record.
        Try
            Dim stringmessage As String
            For Each dgItem In DatagridACHDebitExport.Items
                chkbox = dgItem.FindControl("CheckBoxSelect")
                If chkbox.Checked Then
                    l_integer_RowIndex = dgItem.ItemIndex
                    l_integer_RowCounter = l_integer_RowCounter + 1
                    If l_integer_RowCounter > 1 Then
                        Exit For
                    End If
                End If
            Next

            If l_integer_RowCounter = 0 Then
                'AA:16.12.2013 - BT:2311 Commented below code because now button edit is not in use  
                'this condition added by Anita.
                'If p_integer_Mode = 0 Then
                '    stringmessage = "Please Select A Record To Update."
                'Else
                stringmessage = "Please select a record to delete."

                'End If
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", stringmessage, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(stringmessage, EnumMessageTypes.Error)
                l_integer_RowIndex = -1
                'ElseIf l_integer_RowCounter > 1 Then
                '    If p_integer_Mode = 0 Then
                '        stringmessage = "Please Select A Single Record To Update."
                '        l_integer_RowIndex = -1
                '        'Else
                '        '    stringmessage = "Please Select A Single Record To Delete."
                '    End If
                '    'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", stringmessage, MessageBoxButtons.Stop)
            End If

            p_integer_SelectedRowCount = l_integer_RowCounter

            Return l_integer_RowIndex
        Catch
            Throw
        End Try
    End Function
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Response.Redirect("MainWebForm.aspx", False)
    End Sub
    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        ' Dim dr As DataRow
        '  dr = l_DataSet_ACHDebits.Tables("atsYmcaACHDebit").Rows(0)
        ' dr.Delete()
        Dim l_counter As Integer
        Dim l_integer_RowIndex As Integer
        Try

            'l_counter = Session("l_counter")

            l_integer_RowIndex = GetSelectedDataGridRowIndex(l_counter, 1)
            'DatagridACHDebitExport.SelectedIndex = l_integer_RowIndex

            'If Me.DatagridACHDebitExport.SelectedIndex <> -1 Then
            'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
            If l_counter > 0 Then
                'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Are you sure you want to Delete ?", MessageBoxButtons.YesNo, False)
                lblMessage.Text = "Are you sure you want to delete ?"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "');", True)
                Session("ExportRecords") = False
                'Else
                'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please Select A Record To Delete", MessageBoxButtons.Stop)
            End If

            'YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.DeleteACHDebits(l_guiuniqueid)
            ' PopulateDataGridACHDebits()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    Private Sub ButtonExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExport.Click

        'Commented and Modified By Ashutosh Patil as on 28-May-2007
        'Confirmation MessageBox will be asked to the user on click of yes button on Yes-Record will be exported if found or present in datagrid
        'on No - Nothing will happen or No action
        'For avoiding the double click error 

        'Added by Shubhrata Oct3rd 2006
        Dim l_dataset As DataSet
        Dim l_CheckBox As CheckBox
        Dim l_flag As Boolean
        'Dim l_TotalAmount As Decimal

        Dim l_Arr_DataRow As DataRow()
        Dim l_String_Search As String

        'l_TotalAmount = 0
        l_flag = False

        Try
            'Added by neeraj on 23-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If Not Session("DataSetACHDebitExport") Is Nothing Then
                l_dataset = DirectCast(Session("DataSetACHDebitExport"), DataSet)
                'Priya 19-August-2010:YRS 5.0-1098:the export process requires a recalcualtion before funcitoning
                For Each l_DataGridItem As DataGridItem In Me.DatagridACHDebitExport.Items
                    l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                    l_String_Search = l_DataGridItem.Cells(4).Text()
                    l_Arr_DataRow = l_dataset.Tables(0).Select("uniqueid = '" & l_String_Search & "' ")
                    ' l_Arr_DataRow = l_dataset.Tables(0).Select(l_String_Search)

                    If (Not l_CheckBox Is Nothing) Then
                        If l_CheckBox.Checked = True Then
                            l_Arr_DataRow(0)("Selected") = 1
                            ' l_TotalAmount = l_TotalAmount + IIf(IsNumeric(l_DataGridItem.Cells(5).Text.Trim()), CType(l_DataGridItem.Cells(5).Text.Trim(), Decimal), 0)
                            l_flag = True
                        Else
                            l_Arr_DataRow(0)("Selected") = 0
                        End If
                    Else
                        If l_flag <> True Then
                            l_flag = False
                        End If
                    End If
                Next
                'End 19-August-2010:YRS 5.0-1098
                'For Each l_DataGridItem As DataGridItem In Me.DatagridACHDebitExport.Items
                '    l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                '    If (Not l_CheckBox Is Nothing) Then
                '        If l_CheckBox.Checked = True Then
                '            l_TotalAmount = l_TotalAmount + CType(l_DataGridItem.Cells(5).Text.Trim(), Decimal)
                '            l_count = l_count + 1
                '            l_flag = True
                '        ElseIf l_CheckBox.Checked = False Then
                '            If l_flag <> True Then
                '                l_flag = False
                '            End If
                '        End If
                '    End If
                'Next
            End If

            If l_flag = False Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select atleast one record to export.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Please select atleast one record to export.", EnumMessageTypes.Error)
                Exit Sub
            End If


            If DatagridACHDebitExport.Items.Count > 0 Then
                'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
                'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Are you sure you want to Export the records ?", MessageBoxButtons.YesNo)
                lblMessage.Text = "Are you sure you want to export the records ?"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "');", True)
                Session("ExportRecords") = True
            Else
                Exit Sub
            End If
            'If l_flag = True Then
            '    If Me.TextBoxTotal.Text = Math.Round(l_TotalAmount, 2) Then
            '        'Added by Shubhrata Oct3rd 2006
            '        'Aparna
            '        Session("batchid") = Nothing
            '        If ExportToPopUp() = True Then
            '            MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "File Exported Successfully.", MessageBoxButtons.OK)
            '            DatagridACHDebitExport.SelectedIndex = -1
            '            ButtonExport.Enabled = False
            '            'Aparna Nov-7th-2006
            '            'Calling Report to Show Selected Ymcas
            '            Dim arrListParaColl As ArrayList
            '            arrListParaColl = New ArrayList
            '            arrListParaColl.Add(Session("batchid"))
            '            ' CallReport("YMCA ACH Report", arrListParaColl)
            '            Session("strReportName") = "YMCA ACH Report"
            '            Session("arrListParaColl") = arrListParaColl
            '            Dim popupScript As String = "<script language='javascript'>" & _
            '             "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            '             "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            '             "</script>"
            '            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
            '                Page.RegisterStartupScript("PopupScript2", popupScript)
            '            End If
            '            'Aparna Nov-7th-2006
            '            '  MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Report is pending", MessageBoxButtons.OK)
            '            ' Exit Sub
            '        Else
            '            ButtonExport.Enabled = True
            '        End If
            '    Else
            '        MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please recalculate the amount.", MessageBoxButtons.OK)
            '        Exit Sub
            '    End If

            'ElseIf l_flag = False Then
            '    MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select atleast one record to export.", MessageBoxButtons.OK)
            '    Exit Sub
            'End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        Try
            'AA:16.12.2013 - BT:2311 changed to show select all and select none for only one button.
            If ButtonSelectAll.Text = "Select All" Then
                SelectAll()
                ButtonSelectAll.Text = "Select None"
            ElseIf ButtonSelectAll.Text = "Select None" Then
                DeSelectAll()
                ButtonSelectAll.Text = "Select All"
            End If
            'ButtonDeSelectAll.Enabled = True
            ButtonExport.Enabled = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'AA:16.12.2013 - BT:2311 changed to show select all and select none for only one button.
    'Private Sub ButtonDeSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeSelectAll.Click
    '    If Session("l_counter") Then
    '        Session("l_counter") = 0
    '    End If
    '    Try

    '        DeSelectAll()
    '        ButtonDeSelectAll.Enabled = False
    '        ButtonSelectAll.Enabled = True
    '        'Commented by Shubhrata Oct 3rd 2006 
    '        'ButtonExport.Enabled = False
    '        'Commented by Shubhrata Oct 3rd 2006 
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
#End Region

#Region "DataGridEvents"
    'AA:16.12.2013 - BT:2311 Added to edit the current row details
    Private Sub DatagridACHDebitExport_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DatagridACHDebitExport.EditCommand
        Try
            Session("YmcaNo") = e.Item.Cells(2).Text
            Session("YmcaName") = e.Item.Cells(3).Text
            Session("UniqueId") = e.Item.Cells(4).Text
            Session("Amount") = e.Item.Cells(6).Text
            Session("PaymentDate") = e.Item.Cells(7).Text

            Dim popupScript5 As String
            popupScript5 = "<script language='javascript'>" & _
                                          "window.open('ACHDebitExportUpdateForm.aspx', 'CustomPopUp', " & _
                                          "'width=800, height=450, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                                          "</script>"

            'Page.RegisterStartupScript("PopupScript5", popupScript5)
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript5", popupScript5, False)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DatagridACHDebitExport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridACHDebitExport.ItemDataBound
        ''Dim dataset_local As DataSet
        ''Dim l_checkbox As CheckBox
        ''Dim arrDataRows As DataRow()
        ''If e.Item.ItemType.Item = ListItemType.Item Then



        ''    dataset_local = Session("DataSetACHDebitExport")
        ''    arrDataRows = dataset_local.Tables(0).Select("uniqueid='" & e.Item.Cells(3).Text & "'")
        ''    If arrDataRows.Length > 0 Then

        ''        If arrDataRows(0)("Selected") = 1 Then
        ''            l_checkbox = DatagridACHDebitExport.Items(e.Item.ItemIndex).FindControl("CheckBoxSelect")
        ''            l_checkbox.Checked = True
        ''        End If


        ''End If
        ''End If
        'Dim l_button_Select As ImageButton

        'Try
        '    l_button_Select = e.Item.FindControl("ImageButtonSelect")

        '    If (Not l_button_Select Is Nothing) Then
        '        l_button_Select.ImageUrl = "images\select.gif"
        '    End If

        'Catch ex As Exception
        '    Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        'End Try

    End Sub
    Private Sub DatagridACHDebitExport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DatagridACHDebitExport.SelectedIndexChanged
        'PopulatetTextBoxTotal()
        Dim l_guiuniqueid As String
        Dim l_button_Select As ImageButton
        Dim l_intPreselectedIndex As Int32 = -1
        Try
            If Not ViewState("PreselectedIndex") Is Nothing Then
                l_intPreselectedIndex = CType(ViewState("PreselectedIndex"), Int32)
            End If

            l_guiuniqueid = DatagridACHDebitExport.SelectedItem.Cells(4).Text
            Session("guiuniqueid") = l_guiuniqueid
            l_button_Select = DatagridACHDebitExport.SelectedItem.FindControl("ImageButtonSelect")

            If (Not l_button_Select Is Nothing) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            If l_intPreselectedIndex <> -1 Then
                l_button_Select = DatagridACHDebitExport.Items(l_intPreselectedIndex).FindControl("ImageButtonSelect")
                If (Not l_button_Select Is Nothing) Then
                    l_button_Select.ImageUrl = "images\select.gif"
                End If
            End If
            ViewState("PreselectedIndex") = DatagridACHDebitExport.SelectedIndex
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

#End Region

#Region "FunctionsForCreatingExportFile"
    'Private Function GenerateTextFile() As String
    '    Try


    '        Dim l_String_FileCreateDate As String
    '        Dim l_string_Tmp As String
    '        Dim l_Date_FileName As Date
    '        l_Date_FileName = Now

    '        l_String_FileCreateDate = "_"
    '        l_string_Tmp = CType(l_Date_FileName.Year, String).Trim()
    '        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
    '        l_string_Tmp = CType(l_Date_FileName.Month, String).Trim()
    '        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
    '        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
    '        l_string_Tmp = CType(l_Date_FileName.Day, String).Trim()
    '        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
    '        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp + "_"

    '        l_string_Tmp = CType(l_Date_FileName.Hour, String).Trim()
    '        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
    '        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

    '        l_string_Tmp = CType(l_Date_FileName.Minute, String).Trim()
    '        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
    '        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

    '        l_string_Tmp = CType(l_Date_FileName.Second, String).Trim()
    '        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
    '        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

    '        Return l_String_FileCreateDate

    '    Catch ex As Exception
    '        Throw

    '    End Try
    'End Function
    Private Function CreateCSVFile(ByVal Parameter_String_Filename As String) As Boolean
        Dim l_String_YMCANo As String
        Dim l_String_blank As String
        Dim l_String_Amount As String
        Dim l_String_Output As String
        Dim l_DataRow As DataRow
        'Shubhrata
        Dim dt_groupedexport As DataTable
        'SHubhrata
        Dim dtExport As DataTable
        Try
            'Shubhrata 
            'If Not Session("dtExport") Is Nothing Then
            '    dtExport = CType(Session("dtExport"), DataTable)
            'End If
            If Not Session("dt_groupedexport") Is Nothing Then
                dt_groupedexport = CType(Session("dt_groupedexport"), DataTable)
            End If
            'Shubhrata 

            Dim l_StreamWriter_File As StreamWriter = File.CreateText(Parameter_String_Filename.ToString)
            l_String_Output = ""
            'Shubhrata
            'For Each l_DataRow In dtExport.Rows
            For Each l_DataRow In dt_groupedexport.Rows
                'Shubhrata
                l_String_Output += l_DataRow("YmcaNo").ToString().Trim + ", ,"
                l_String_Output += l_DataRow("Amount").ToString().Trim

                l_StreamWriter_File.WriteLine(l_String_Output)
                l_String_Output = ""
            Next

            l_StreamWriter_File.Close()
            'Shubhrata
            Session("dt_groupedexport") = False
            'Shubhrata
            Session("dtExport") = False
            CreateCSVFile = True
        Catch ex As Exception
            Throw

        End Try


    End Function
    'Created by Ashish on 23-Jan-2009 ,Start
    Private Function CreateCSVFileForBank(ByVal Parameter_String_Filename As String, ByVal paraGroupedExortListForBank As DataTable) As Boolean

        Dim l_String_Output As String
        Dim l_StreamWriter_File As StreamWriter = Nothing
        Dim dv_GroupedExportListForBank As DataView = Nothing
        Try

            l_StreamWriter_File = File.CreateText(Parameter_String_Filename.ToString)
            l_String_Output = ""
            If Not paraGroupedExortListForBank Is Nothing Then
                If paraGroupedExortListForBank.Rows.Count > 0 Then
                    dv_GroupedExportListForBank = New DataView
                    dv_GroupedExportListForBank.Table = paraGroupedExortListForBank
                    dv_GroupedExportListForBank.Sort = "YmcaNo"
                    Dim j As Int32
                    For j = 0 To dv_GroupedExportListForBank.Count - 1 Step 1

                        l_String_Output += dv_GroupedExportListForBank.Item(j)("YmcaNo").ToString().Trim.PadLeft(4, "0") + ", ,"
                        l_String_Output += dv_GroupedExportListForBank.Item(j)("Amount").ToString().Trim

                        l_StreamWriter_File.WriteLine(l_String_Output)
                        l_StreamWriter_File.Flush()
                        l_String_Output = ""

                    Next

                End If
            End If

            CreateCSVFileForBank = True
        Catch ex As Exception
            Throw
        Finally
            If Not l_StreamWriter_File Is Nothing Then
                l_StreamWriter_File.Close()
            End If
            dv_GroupedExportListForBank = Nothing

        End Try


    End Function
    'Created by Ashish on 23-Jan-2009 ,End
#End Region

#Region "Functions"
    'Ashish:2010.06.24 This function is not in use.
    'Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)

    '    Dim ck1 As CheckBox = CType(sender, CheckBox)
    '    Dim dgItem As DataGridItem = CType(ck1.NamingContainer, DataGridItem)
    '    Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
    '    Dim dgi As DataGridItem
    '    Dim l_TotalAmount As Decimal
    '    Dim l_string As String

    '    'july 7
    '    Try

    '        'Code added by ashutosh on 15 sep 2006
    '        Dim l_DataSet_ACHDebits As DataSet
    '        Dim l_Arr_DataRow As DataRow()
    '        l_DataSet_ACHDebits = Session("DataSetACHDebitExport")

    '        l_string = dgItem.Cells(3).Text()
    '        l_Arr_DataRow = l_DataSet_ACHDebits.Tables(0).Select("uniqueid = '" & l_string & "' ")


    '        'Ashu 15 Sep
    '        ButtonDeSelectAll.Enabled = True
    '        If ck1.Checked = True Then
    '            l_TotalAmount = CType(Me.TextBoxTotal.Text, Decimal) + CType(dgItem.Cells(5).Text.Trim(), Decimal)

    '            'commented & modified by hafiz on 15-Dec-2006
    '            'TextBoxTotal.Text = l_TotalAmount
    '            TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()

    '            If l_Arr_DataRow.Length > 0 Then
    '                l_Arr_DataRow(0)("Selected") = 1
    '            End If
    '        Else
    '            l_TotalAmount = CType(Me.TextBoxTotal.Text, Decimal) - CType(dgItem.Cells(5).Text.Trim(), Decimal)

    '            TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()

    '            If l_Arr_DataRow.Length > 0 Then
    '                l_Arr_DataRow(0)("Selected") = 0
    '            End If
    '        End If
    '        Session("DataSetACHDebitExport") = l_DataSet_ACHDebits
    '        'july 7
    '        Dim i As Integer = dgItem.ItemIndex
    '        'For Each dgItem In DatagridACHDebitExport.Items
    '        CheckBoxSelect = dgItem.FindControl("CheckBoxSelect")
    '        If CheckBoxSelect.Checked Then

    '            DatagridACHDebitExport.SelectedIndex = i
    '            Session("index") = i
    '            ' l_counter = l_counter + 1
    '            'MAY 25
    '            Session("l_counter") = (CType(Session("l_counter"), Int32) + 1)
    '            TextBoxCount.Text = CType(Session("l_counter"), Int32)
    '        Else
    '            Session("l_counter") = (CType(Session("l_counter"), Int32) - 1)
    '            TextBoxCount.Text = CType(Session("l_counter"), Int32)
    '            ' l_counter = l_counter - 1
    '            If Session("l_counter") = -1 Then
    '                Session("l_counter") = 0
    '            End If
    '        End If
    '        ' Session("l_counter") = (CType(Session("l_counter"), Int32) + l_counter)
    '        If Session("l_counter") > 0 Then
    '            ButtonDeSelectAll.Enabled = True

    '            ButtonExport.Enabled = True
    '        Else
    '            ButtonDeSelectAll.Enabled = False
    '            'ButtonExport.Enabled = False
    '        End If

    '        'Session("l_counter") = l_counter

    '        'End If
    '        'Dim i As Integer = dgItem.ItemIndex

    '        'For Each dgItem In DatagridACHDebitExport.Items
    '        ' If ck1.Checked Then
    '        ' dgItem.CssClass = "DataGrid_SelectedStyle"
    '        ' DatagridACHDebitExport.SelectedIndex = i
    '        ' Session("index") = i
    '        'l_counter = l_counter + 1
    '        ' Else
    '        '   dgItem.CssClass = "DataGrid_NormalStyle"
    '        ' End If
    '        ' Session("l_counter") = l_counter
    '        ' Next
    '    Catch
    '        Throw
    '    End Try

    'End Sub

    Private Function ExportToPopUp() As Boolean
        'Added by Ashish 22-Jan-2009,Start
        Dim dtexportForBank As New DataTable
        Dim dataRowSelected As DataRow()
        Dim l_DataSetACHDebits As DataSet
        'Added by Ashish 22-Jan-2009,End
        Try
            'Ashish:2010.06.24 :Check session is nothing
            If Session("DataSetACHDebitExport") Is Nothing Then
                Return False
            End If
            l_DataSetACHDebits = Session("DataSetACHDebitExport")
            Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
            Dim dgi As DataGridItem
            Dim dr As DataRow
            Dim dtexport As New DataTable
            Dim l_dsExport As DataSet
            Dim batchid As String
            'new
            Dim dt As DataTable
            Dim darray As DataRow()
            Dim struniqueid As String
            Dim l_filenameprefix As String
            Dim l_filename As String
            Dim l_filenamesuffix As String
            Dim l_severfilename As String
            Dim l_fileACH As String


            'Added by Ashish 22-Jan-2009,End
            'new
            dtexport = l_DataSetACHDebits.Tables("ACHDebits").Clone()

            dt = l_DataSetACHDebits.Tables("ACHDebits")
            'Commented by ashish 23-Jan-2009 :no need to update amount again,Start
            'For Each dgi In DatagridACHDebitExport.Items
            '    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
            '    If CheckBoxSelect.Checked Then
            '        dr = dtexport.NewRow()
            '        dr("YmcaNo") = dgi.Cells(1).Text
            '        dr("YMCAName") = dgi.Cells(2).Text
            '        dr("uniqueid") = dgi.Cells(3).Text
            '        dr("Amount") = dgi.Cells(5).Text
            '        dr("PaymentDate") = dgi.Cells(6).Text
            '        dtexport.Rows.Add(dr)
            '        struniqueid = dgi.Cells(3).Text
            '        
            '        darray = dt.Select("uniqueid='" & struniqueid & "'")
            '        darray(0)("Amount") = dgi.Cells(5).Text
            '        
            '    End If

            'Next

            'Commented by ashish 23-Jan-2009 :no need to update amount again,End
            'Added by Ashish 23-Jan-2009 ,Start
            Dim counter As Integer
            dataRowSelected = dt.Select("Selected=1")
            If dataRowSelected.Length > 0 Then
                For counter = 0 To dataRowSelected.Length - 1 Step 1
                    dr = dtexport.NewRow()
                    dr("YmcaNo") = dataRowSelected(counter)("YmcaNo")
                    dr("YMCAName") = dataRowSelected(counter)("YMCAName")
                    dr("uniqueid") = dataRowSelected(counter)("uniqueid")
                    dr("Amount") = dataRowSelected(counter)("Amount")
                    dr("PaymentDate") = dataRowSelected(counter)("PaymentDate")
                    dr("AssociationStatus") = dataRowSelected(counter)("AssociationStatus")
                    dtexport.Rows.Add(dr)
                Next
            End If
            'Added by Ashish 23-Jan-2009 ,End

            'Shubhrata
            Dim dr_combinedrow As DataRow()
            Dim l_ymcano As String = ""
            Dim i As Integer
            Dim j As Integer
            Dim l_amount As Decimal = 0.0
            Dim l_arramount As Decimal = 0.0
            Dim dt_groupedexport As New DataTable
            Dim dr_groupedexport As DataRow
            dt_groupedexport = dtexport.Clone()
            'Shubhrata
            Dim dv_groupedexport As New DataView  '= dtexport.DefaultView
            dv_groupedexport.Table = dtexport
            dv_groupedexport.Sort = "YmcaNo"
            Dim dt_sortedexport As New DataTable
            For j = 0 To dv_groupedexport.Count - 1
                dr_combinedrow = dv_groupedexport.Table.Select("YmcaNo = '" & dv_groupedexport.Item(j)(0) & "'")
                If dr_combinedrow.Length > 0 Then
                    If l_ymcano <> dv_groupedexport.Item(j)(0) Then
                        l_ymcano = dv_groupedexport.Item(j)(0)
                        For i = 0 To dr_combinedrow.Length - 1
                            l_arramount = Convert.ToDecimal(dr_combinedrow(i)("Amount"))
                            l_amount = l_amount + l_arramount
                        Next
                        dr_groupedexport = dt_groupedexport.NewRow()
                        dr_groupedexport("YmcaNo") = dv_groupedexport.Item(j)("YmcaNo")
                        dr_groupedexport("YMCAName") = dv_groupedexport.Item(j)("YMCAName")
                        ' dr_groupedexport("uniqueid") = drow("uniqueid")
                        dr_groupedexport("Amount") = l_amount
                        dr_groupedexport("PaymentDate") = dv_groupedexport.Item(j)("PaymentDate")
                        'Added By Ashish 23-Jan-2009,start
                        dr_groupedexport("AssociationStatus") = dv_groupedexport.Item(j)("AssociationStatus")
                        'Added By Ashish 23-Jan-2009,End
                        dt_groupedexport.Rows.Add(dr_groupedexport)
                        l_amount = 0.0
                    End If
                End If
            Next
            'For Each drow As DataRow In dt_sortedexport.Rows
            '    If drow.RowState <> DataRowState.Deleted Then
            '        'Shubhrata

            '        dr_combinedrow = dt_sortedexport.Select("YmcaNo = '" & drow(0) & "'")
            '        If dr_combinedrow.Length > 0 Then
            '            If l_ymcano <> drow("YmcaNo") Then
            '                l_ymcano = drow("YmcaNo")

            '                For i = 0 To dr_combinedrow.Length - 1
            '                    l_arramount = Convert.ToDouble(dr_combinedrow(i)("Amount"))
            '                    l_amount = l_amount + l_arramount
            '                Next
            '                'drow("Amount") = l_amount
            '                dr_groupedexport = dt_groupedexport.NewRow()
            '                dr_groupedexport("YmcaNo") = drow("YmcaNo")
            '                dr_groupedexport("YMCAName") = drow("YMCAName")
            '                ' dr_groupedexport("uniqueid") = drow("uniqueid")
            '                dr_groupedexport("Amount") = l_amount
            '                dr_groupedexport("PaymentDate") = drow("PaymentDate")
            '                dt_groupedexport.Rows.Add(dr_groupedexport)
            '                l_amount = 0.0
            '            End If
            '        End If

            '    End If

            'Next
            'Shubhrata
            Session("DataSetACHDebits") = l_DataSetACHDebits
            'new
            'Shubhrata
            'If dtexport.Rows.Count > 0 Then
            '    Session("dtExport") = dtexport
            If dt_groupedexport.Rows.Count > 0 Then

                'Added by Ashish 23-Jan-2009 ,Start
                'Grouped Associated Ymca's with 9572 Ymca
                Dim dt_TempGroupedExportForBank As New DataTable

                dt_TempGroupedExportForBank = dt_groupedexport
                dtexportForBank = GetGroupedExportListForBank(dt_TempGroupedExportForBank)
                dt_TempGroupedExportForBank = Nothing
                'Added by Ashish 23-Jan-2009 ,End

                Session("dt_groupedexport") = dt_groupedexport
                'Shubhrata
                'Response.Redirect("ACHDebitExportToExcel.aspx")
                'Ashu Code on 15- Sep
                l_DataSetACHDebits = Session("DataSetACHDebitExport")
                l_dsExport = l_DataSetACHDebits.Clone()
                For k As Integer = 0 To l_DataSetACHDebits.Tables(0).Rows.Count - 1
                    If l_DataSetACHDebits.Tables(0).Rows(k)("Selected") = 1 Then
                        l_dsExport.Tables(0).ImportRow(l_DataSetACHDebits.Tables(0).Rows(k))
                    End If
                Next
                'Ashut Code End of Code 15- Sep
                '   l_dsExport = Session("DataSetACHDebits")
                batchid = YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.GetBatchId()
                Session("batchid") = batchid

                YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.UpdateOnExport(l_dsExport, batchid)
                l_dsExport.AcceptChanges()

                'Dim arrListParaColl As ArrayList 'Code added by Ashutosh for Report on 18 sep
                'arrListParaColl = New ArrayList
                'arrListParaColl(0) = batchid
                'CallReport("Report Export Debits", arrListParaColl)
                Session("l_counter") = 0
                Session("DataSetACHDebits") = l_dsExport
                PopulateDataGridACHDebits()
                l_filenameprefix = ACHDebitExportForm.connectioninfo
                ' l_filenamesuffix = GenerateTextFile()
                l_filenamesuffix = "_" + batchid
                l_fileACH = "ACHDebit" + l_filenamesuffix + ".csv"
                l_filename = l_filenameprefix + "\\" + l_fileACH
                'l_severfilename = ConfigurationSettings.AppSettings("ACH")

                'Commented By Ashish on 23-Jan-2009,Start
                'If CreateCSVFile(l_filename) Then
                '    CopyToServer(l_filenameprefix, l_filename, l_fileACH)
                'End If
                'Commented By Ashish on 23-Jan-2009,End
                'Added By Ashish 23-Jan-2009 YRS5.0-651,Start 
                Dim l_dtCopyToServer As New DataTable
                Dim l_dtCopyToserverRow As DataRow
                Dim l_strFileNameForBank As String
                Dim l_strFileNameForYrs As String
                Dim l_strFileNameWithPathForBank As String
                Dim l_strFileNameWithPathForYrs As String
                l_strFileNameForBank = "ACHDebit_" + batchid + "_BANK.csv"
                l_strFileNameForYrs = "ACHDebit_" + batchid + "_YRS.csv"
                l_strFileNameWithPathForBank = l_filenameprefix + "\\" + l_strFileNameForBank
                l_strFileNameWithPathForYrs = l_filenameprefix + "\\" + l_strFileNameForYrs

                l_dtCopyToServer.Columns.Add(New DataColumn("AchConfigPath", System.Type.GetType("System.String")))
                l_dtCopyToServer.Columns.Add(New DataColumn("FileNameWithPath", System.Type.GetType("System.String")))
                l_dtCopyToServer.Columns.Add(New DataColumn("FileName", System.Type.GetType("System.String")))
                'Add Row for Bank
                l_dtCopyToserverRow = l_dtCopyToServer.NewRow()
                l_dtCopyToserverRow("AchConfigPath") = l_filenameprefix
                l_dtCopyToserverRow("FileNameWithPath") = l_strFileNameWithPathForBank
                l_dtCopyToserverRow("FileName") = l_strFileNameForBank
                l_dtCopyToServer.Rows.Add(l_dtCopyToserverRow)
                'Add Row for YRS
                l_dtCopyToserverRow = l_dtCopyToServer.NewRow()
                l_dtCopyToserverRow("AchConfigPath") = l_filenameprefix
                l_dtCopyToserverRow("FileNameWithPath") = l_strFileNameWithPathForYrs
                l_dtCopyToserverRow("FileName") = l_strFileNameForYrs
                l_dtCopyToServer.Rows.Add(l_dtCopyToserverRow)

                If CreateCSVFileForBank(l_strFileNameWithPathForBank, dtexportForBank) And CreateCSVFile(l_strFileNameWithPathForYrs) Then
                    CopyToServer(l_dtCopyToServer)
                End If

                'Added By Ashish 23-Jan-2009 YRS5.0-651,End 


                '  Dim popupScript As String
                ' popupScript = "<script language='javascript'>" & _
                ' "window.open('ACHDebitExportToExcel.aspx', 'CustomPopUp', " & _
                ' "'width=800, height=450, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                ' "</script>"




                ' Page.RegisterStartupScript("PopupScript2", popupScript)
                'MAY 25TH
                ExportToPopUp = True
            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select a record to export.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Please select atleast one record to export.", EnumMessageTypes.Error)
                ExportToPopUp = False
            End If

        Catch
            Throw
            'Added by Ashish 23-Jan-2009
        Finally
            dtexportForBank = Nothing

        End Try



    End Function
    'Commented by Ashish on 23-Jan-2009 
    ' Private Function CopyToServer(ByVal l_filenameprefix As String, ByVal l_filename As String, ByVal l_fileACH As String)
    Private Function CopyToServer(ByVal paraDataTableCopyToServer As DataTable)
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

            l_dataset = YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.MetaOutputFileType()

            If l_dataset Is Nothing Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(PlaceHolder1, "ACHDebitExport File", "Unable to find ACHDebitExport  Values in the MetaOutput file.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Unable to find ACHDebitExport  Values in the MetaOutput file.", EnumMessageTypes.Error)

                Exit Function
            End If

            l_DataRow = l_dataset.Tables(0).Rows(0)

            If l_DataRow Is Nothing Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(PlaceHolder1, "ACHDebitExport  File", "Unable to find ACHDebitExport  Configuration Values in the MetaOutput file.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Unable to find ACHDebitExport  Configuration Values in the MetaOutput file.", EnumMessageTypes.Error)
                Exit Function
            End If

            l_datatable_FileList.Columns.Add(SourceFolder)
            l_datatable_FileList.Columns.Add(SourceFile)
            l_datatable_FileList.Columns.Add(DestFolder)
            l_datatable_FileList.Columns.Add(DestFile)
            'commented by Ashish on 23-Jan2009 ,start

            'l_temprow = l_datatable_FileList.NewRow
            'l_temprow("SourceFolder") = l_filenameprefix
            'l_temprow("SourceFile") = l_filename

            'l_temprow("DestFolder") = Convert.ToString(l_DataRow("OutputDirectory"))
            'l_temprow("DestFile") = Convert.ToString(l_DataRow("OutputDirectory")) + "\" + l_fileACH
            'l_datatable_FileList.Rows.Add(l_temprow)
            'commented by Ashish on 23-Jan2009 ,End
            If paraDataTableCopyToServer.Rows.Count > 0 Then

                For Each dtFilePathRow As DataRow In paraDataTableCopyToServer.Rows
                    l_temprow = l_datatable_FileList.NewRow
                    l_temprow("SourceFolder") = dtFilePathRow("AchConfigPath")
                    l_temprow("SourceFile") = dtFilePathRow("FileNameWithPath")
                    l_temprow("DestFolder") = Convert.ToString(l_DataRow("OutputDirectory"))
                    l_temprow("DestFile") = Convert.ToString(l_DataRow("OutputDirectory")) + "\" + dtFilePathRow("FileName")
                    l_datatable_FileList.Rows.Add(l_temprow)
                Next

                Session("FTFileList") = l_datatable_FileList

                popupscript = "<script language='javascript'>" & _
                  "var a = window.open('FT\\CopyFilestoFileServer.aspx', 'FileCopyPopUp', " & _
                  "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                  "</script>"
                'If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                '    Page.RegisterStartupScript("PopupScript1", popupscript)
                'End If
                'AA:16.12.2013 - BT:2311 Added to open the copy to server pop-up in ajax postback page
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupscript, False)

            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub DeleteFromACHDebit()

        Dim l_DataSetfordelete As DataSet
        Dim l_delete As DataSet

        Dim dgi As DataGridItem
        Dim darray As DataRow()
        Dim dr As DataRow
        Dim dt As DataTable
        Dim l_dataset_delete As DataSet
        Dim struniqueid As String
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox

        Try
            l_DataSetfordelete = Session("DataSetACHDebitExport")
            dt = l_DataSetfordelete.Tables("ACHDebits")
            For Each dgi In DatagridACHDebitExport.Items
                CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                If CheckBoxSelect.Checked Then

                    struniqueid = dgi.Cells(4).Text
                    YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.DeleteACHDebits(struniqueid)
                    'Code commented by ashutosh 15-sep-2006
                    ' darray = dt.Select("uniqueid='" & struniqueid & "'")
                    'darray(0)("Amount") = dgi.Cells(4).Text

                    ' darray(0).Delete()
                    'Code commented by ashutosh 15-sep-2006

                End If
            Next

            ' Dim l_guiuniqueid As String
            ' l_guiuniqueid = Session("guiuniqueid")
            'Code commented by ashutosh 15-sep-2006
            'YMCARET.YmcaBusinessObject.ACHDebitExportBOClass.DeleteACHDebits(l_DataSetfordelete)
            'Code commented by ashutosh 

            Session("l_counter") = 0
            PopulateDataGridACHDebits()

        Catch
            Throw
        End Try
    End Sub
    Private Sub SelectAll()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem
        Dim l_TotalAmount As Decimal
        Try
            'Code added by ashutosh on 15 sep 2006
            Dim l_DataSet_ACHDebits As DataSet
            'Dim l_Arr_DataRow As DataRow()
            l_DataSet_ACHDebits = Session("DataSetACHDebitExport")
            ' l_DataSet_ACHDebits.Tables(0).Columns("Selected").Expression = 1
            For i As Integer = 0 To l_DataSet_ACHDebits.Tables(0).Rows.Count - 1
                l_DataSet_ACHDebits.Tables(0).Rows(i)("Selected") = 1
            Next
            'End Ashutosh Code 
            If DatagridACHDebitExport.Items.Count > 0 Then
                TextBoxTotal.Text = 0
                For Each dgi In DatagridACHDebitExport.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    CheckBoxSelect.Checked = True
                    l_TotalAmount = CType(Me.TextBoxTotal.Text, Decimal) + CType(dgi.Cells(6).Text.Trim(), Decimal)

                    'commented & modified by hafiz on 15-Dec-2006
                    'TextBoxTotal.Text = l_TotalAmount
                    TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()
                Next

                Session("l_counter") = DatagridACHDebitExport.Items.Count
                TextBoxCount.Text = CType(DatagridACHDebitExport.Items.Count, Int32)
            End If
        Catch
            Throw
        End Try

    End Sub
    Private Sub DeSelectAll()

        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem
        Dim l_TotalAmount As Decimal
        'Code added by ashutosh on 15 sep 2006
        Dim l_DataSet_ACHDebits As DataSet
        l_DataSet_ACHDebits = Session("DataSetACHDebitExport")
        For i As Integer = 0 To l_DataSet_ACHDebits.Tables(0).Rows.Count - 1
            l_DataSet_ACHDebits.Tables(0).Rows(i)("Selected") = 0
        Next
        Try
            If DatagridACHDebitExport.Items.Count > 0 Then
                For Each dgi In DatagridACHDebitExport.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    CheckBoxSelect.Checked = False
                    l_TotalAmount = CType(Me.TextBoxTotal.Text, Decimal) - CType(dgi.Cells(6).Text.Trim(), Decimal)
                Next
                TextBoxTotal.Text = 0
                Session("l_counter") = 0
                TextBoxCount.Text = 0
            End If
        Catch
            Throw
        End Try

    End Sub
    Private Function ExportData() As String
        'Added By Ashutosh Patil as on 28-May-2007
        'Purpose-Confirmation messagebox will be thrown on click of Export button so regarding that this function 
        'will be called on clicking of Confirmation of Yes button. For Avoiding double click error.
        'YREN-3364 

        Dim l_dataset As DataSet
        Dim l_CheckBox As CheckBox
        Dim l_flag As Boolean
        Dim l_TotalAmount As Decimal
        l_TotalAmount = 0
        l_flag = False
        Try

            If Not Session("DataSetACHDebitExport") Is Nothing Then
                l_dataset = CType(Session("DataSetACHDebitExport"), DataSet)
                For Each l_DataGridItem As DataGridItem In Me.DatagridACHDebitExport.Items
                    l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                    If (Not l_CheckBox Is Nothing) Then
                        If l_CheckBox.Checked = True Then
                            l_TotalAmount = l_TotalAmount + CType(l_DataGridItem.Cells(6).Text.Trim(), Decimal)
                            l_flag = True
                        ElseIf l_CheckBox.Checked = False Then
                            If l_flag <> True Then
                                l_flag = False
                            End If
                        End If
                    End If
                Next
            End If


            If l_flag = True Then
                If Me.TextBoxTotal.Text = Math.Round(l_TotalAmount, 2) Then
                    'Added by Shubhrata Oct3rd 2006
                    'Aparna
                    Session("batchid") = Nothing
                    If ExportToPopUp() = True Then
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "File Exported Successfully.", MessageBoxButtons.OK)
                        HelperFunctions.ShowMessageToUser("File Exported Successfully.", EnumMessageTypes.Success)
                        DatagridACHDebitExport.SelectedIndex = -1
                        ButtonExport.Enabled = False
                        'Session("ExportRecords") = False
                        'Aparna Nov-7th-2006
                        'Calling Report to Show Selected Ymcas
                        Dim arrListParaColl As ArrayList
                        arrListParaColl = New ArrayList
                        arrListParaColl.Add(Session("batchid"))
                        ' CallReport("YMCA ACH Report", arrListParaColl)
                        Session("strReportName") = "YMCA ACH Report"
                        Session("arrListParaColl") = arrListParaColl
                        Dim popupScript As String = "<script language='javascript'>" & _
                         "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
                         "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                         "</script>"
                        'If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        '    Page.RegisterStartupScript("PopupScript2", popupScript)
                        'End If
                        'AA:16.12.2013 - BT:2311 Added to open the copy to server pop-up in ajax postback page
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript2", popupScript, False)
                        'Aparna Nov-7th-2006
                        '  MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Report is pending", MessageBoxButtons.OK)
                        ' Exit Sub
                    Else
                        ButtonExport.Enabled = True
                    End If
                Else
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please recalculate the amount.", MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser("Please recalculate the amount.", EnumMessageTypes.Error)
                    Exit Function
                End If

            ElseIf l_flag = False Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select atleast one record to export.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Please select atleast one record to export.", EnumMessageTypes.Error)
                Exit Function
            End If
        Catch ex As Exception
            Session("ExportRecords") = Nothing
            Throw
        End Try
    End Function
#End Region




    Private Sub DatagridACHDebitExport_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DatagridACHDebitExport.SortCommand
        'Code added by ashutosh on
        Dim l_dataset As DataSet
        Dim l_DataView As DataView
        Dim SortExpression As String

        SortExpression = e.SortExpression
        l_dataset = CType(Session("DataSetACHDebitExport"), DataSet)
        l_DataView = l_dataset.Tables(0).DefaultView
        l_DataView.Sort = SortExpression
        If Not ViewState("DisbursementsListSort") Is Nothing Then

            If SortExpression + " ASC" = ViewState("DisbursementsListSort").ToString.Trim Then
                l_DataView.Sort = SortExpression + " DESC"
            Else
                l_DataView.Sort = SortExpression + " ASC"
            End If

        Else
            l_DataView.Sort = SortExpression + " ASC"
        End If
        ViewState("DisbursementsListSort") = l_DataView.Sort
        DatagridACHDebitExport.DataSource = l_DataView
        DatagridACHDebitExport.DataBind()
    End Sub



    Private Sub ButtonPendingReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPendingReport.Click
        Try
            'Aparna To include the Report for Pending Records
            ' MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Report is missing", MessageBoxButtons.OK)
            ' Exit Sub
            Dim arrListParaColl As ArrayList
            arrListParaColl = New ArrayList
            arrListParaColl.Add("yes")
            CallReport("YMCA ACH Report", arrListParaColl)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    Private Function CallReport(ByVal strReportName As String, ByVal arrListParaColl As ArrayList)
        Try
            Session("strReportName") = strReportName
            Session("arrListParaColl") = arrListParaColl
            Dim popupScript As String = "<script language='javascript'>" & _
             "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
             "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
             "</script>"
            'If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
            '    Page.RegisterStartupScript("PopupScript1", popupScript)
            'End If
            'AA:16.12.2013 - BT:2311 Added to open the copy to server pop-up in ajax postback page
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript, False)
        Catch
            Throw
        End Try

    End Function


    Private Sub ButtonReCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReCalculate.Click
        Dim i As Long
        Dim l_dataset As DataSet
        Dim l_Arr_DataRow As DataRow()
        Dim l_String_Search As String
        Dim l_CheckBox As CheckBox
        Dim l_TotalAmount As Decimal = 0
        Dim l_count As Integer = 0
        Try

            'ButtonExport.Enabled = True
            If Not Session("DataSetACHDebitExport") Is Nothing Then
                l_dataset = CType(Session("DataSetACHDebitExport"), DataSet)

                For Each l_DataGridItem As DataGridItem In Me.DatagridACHDebitExport.Items
                    l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                    l_String_Search = l_DataGridItem.Cells(4).Text()
                    l_Arr_DataRow = l_dataset.Tables(0).Select("uniqueid = '" & l_String_Search & "' ")
                    ' l_Arr_DataRow = l_dataset.Tables(0).Select(l_String_Search)

                    If (Not l_CheckBox Is Nothing) And (l_Arr_DataRow.Length > 0) Then
                        If l_CheckBox.Checked = True Then
                            l_Arr_DataRow(0)("Selected") = 1
                            l_TotalAmount = l_TotalAmount + IIf(IsNumeric(l_DataGridItem.Cells(6).Text.Trim()), CType(l_DataGridItem.Cells(6).Text.Trim(), Decimal), 0)
                            'Added by Shubhrata Oct3rd 2006
                            l_count = l_count + 1
                            'Added by Shubhrata Oct3rd 2006
                        Else
                            l_Arr_DataRow(0)("Selected") = 0
                        End If
                    End If
                Next

                'commented & modified by hafiz on 15-Dec-2006
                'TextBoxTotal.Text = l_TotalAmount
                TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()

                'Added by Shubhrata Oct3rd 2006
                Me.TextBoxCount.Text = l_count
                'Added by Shubhrata Oct3rd 2006
                Session("DataSetACHDebitExport") = l_dataset
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Created by Ashish 23-Jan-2009,Start
    Private Function GetGroupedExportListForBank(ByVal paraGroupExportedList As DataTable) As DataTable
        Dim l_dtGroupedExportForBank As DataTable
        Dim dtRowsFilter As DataRow()
        Dim l_sumAmount As Decimal
        Dim l_dtNewRow As DataRow
        Dim l_dtFindArmedYmca As DataRow()
        Try
            If paraGroupExportedList.Rows.Count > 0 Then
                l_dtGroupedExportForBank = New DataTable("ExportListForBank")
                l_dtGroupedExportForBank.Columns.Add(New DataColumn("YmcaNo", System.Type.GetType("System.Int32")))
                l_dtGroupedExportForBank.Columns.Add(New DataColumn("Amount", System.Type.GetType("System.Decimal")))


                'Add non associated Ymca's

                dtRowsFilter = paraGroupExportedList.Select("AssociationStatus <> 'F'")
                If dtRowsFilter.Length > 0 Then
                    Dim i As Integer
                    l_sumAmount = 0
                    For i = 0 To dtRowsFilter.Length - 1 Step 1
                        l_dtNewRow = l_dtGroupedExportForBank.NewRow()

                        l_dtNewRow("YmcaNo") = Convert.ToInt32(dtRowsFilter(i)("YmcaNo"))
                        l_dtNewRow("Amount") = Convert.ToDecimal(dtRowsFilter(i)("Amount"))
                        l_dtGroupedExportForBank.Rows.Add(l_dtNewRow)

                    Next
                End If

                'Merge Ymca's with 9572
                dtRowsFilter = paraGroupExportedList.Select("AssociationStatus='F'")
                If dtRowsFilter.Length > 0 Then
                    l_dtFindArmedYmca = l_dtGroupedExportForBank.Select("YmcaNo=9572")
                    If l_dtFindArmedYmca.Length > 0 Then
                        Dim i As Integer
                        l_sumAmount = 0
                        For i = 0 To dtRowsFilter.Length - 1 Step 1
                            l_sumAmount += Convert.ToDecimal(dtRowsFilter(i)("Amount"))
                        Next
                        l_dtFindArmedYmca(0)("Amount") = Convert.ToDecimal(l_dtFindArmedYmca(0)("Amount")) + l_sumAmount
                    Else

                        l_dtNewRow = l_dtGroupedExportForBank.NewRow()
                        l_dtNewRow("YmcaNo") = 9572
                        Dim i As Integer
                        l_sumAmount = 0
                        For i = 0 To dtRowsFilter.Length - 1 Step 1
                            l_sumAmount += Convert.ToDecimal(dtRowsFilter(i)("Amount"))
                        Next
                        l_dtNewRow("Amount") = l_sumAmount
                        l_dtGroupedExportForBank.Rows.Add(l_dtNewRow)
                    End If


                End If


            End If
            Return l_dtGroupedExportForBank

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Created by Ashish 23-Jan-2009,End
    'AA:16.12.2013 - BT:2311 Added Button yes and no button click event functionality.
    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
            If Session("ExportRecords") = False Then
                Session("ExportRecords") = Nothing
                DeleteFromACHDebit()
            End If
            If Session("ExportRecords") = True Then
                Session("ExportRecords") = Nothing
                Call ExportData()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonDelete.Enabled = False
            ButtonDelete.ToolTip = tooltip
            For Each row In DatagridACHDebitExport.Items
                Dim imgUpdate As ImageButton = (TryCast((TryCast(row, TableRow)).Cells(1).Controls(1), ImageButton))
                imgUpdate.Enabled = False
                imgUpdate.ToolTip = toolTip
            Next
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 
End Class
