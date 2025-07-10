'********************************************************************************************************************************
'Modified By                  Date                Description
'********************************************************************************************************************************
'Neeraj Singh                 12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Sanjay R.                    2010.06.29          Enhancement changes(CType to DirectCast)
'Manthan Rajguru              2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N                     02/22/2019          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)   
'********************************************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient

Public Class EDIOutsourceCheckPrintingForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'START: Shilpa N | 02/22/2019 | YRS-AT-4248 |Commented the existing code. Wrong Form name was passing changed with the right one.
    'Dim strFormName As String = New String("EDIOutsourceCheckPrintingForm.aspx")
    Dim strFormName
    Dim paramQuerystring
    'END: Shilpa N | 02/22/2019 | YRS-AT-4248 |Commented the existing code. Wrong Form name was passing changed with the right one.
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelLatestPayroll As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPayrollprocesson As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonStartProcess As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFundId As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLatestPayroll As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPayrollDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridEDIList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelRecordsFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonReport As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_String_Exception_Message As String
#Region "Properties"
    Public Property DatasetEDIList() As DataSet
        Get
            If Not Session("DatasetEDIList") Is Nothing Then
                Return Session("DatasetEDIList")
            Else
                Return Session("DatasetEDIList") = String.Empty
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("DatasetEDIList") = Value
        End Set
    End Property
    Public Property DataTableEDIList() As DataTable
        Get
            If Not Session("DataTableEDIList") Is Nothing Then
                Return Session("DataTableEDIList")
            Else
                Return Session("DataTableEDIList") = String.Empty
            End If
        End Get
        Set(ByVal Value As DataTable)
            Session("DataTableEDIList") = Value
        End Set
    End Property

    Public Property DisbursementType() As String
        Get
            If Not Session("DisbursementType") Is Nothing Then
                Return Session("DisbursementType")
            Else
                Return Session("DisbursementType") = String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("DisbursementType") = Value
        End Set
    End Property

    Public Property ProcessId() As String
        Get
            If Not Session("ProcessId") Is Nothing Then
                Return Session("ProcessId")
            Else
                Return Session("ProcessId") = String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ProcessId") = Value
        End Set
    End Property
    Public Property CheckDate() As String
        Get
            If Not Session("CheckDate") Is Nothing Then
                Return Session("CheckDate")
            Else
                Return Session("CheckDate") = String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("CheckDate") = Value
        End Set
    End Property
    Public Property BatchNo() As Integer
        Get
            If Not Session("BatchNo") Is Nothing Then
                Return Session("BatchNo")
            Else
                Return Session("BatchNo") = 0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("BatchNo") = Value
        End Set
    End Property
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_String_message As String = String.Empty
        Try
            Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Me.Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            'START : Shilpa N | 03/19/2019 | YRS-AT-4248 | Passing strFormName depends on the querystring.  
            If Not Page.Request.QueryString("Name") Is Nothing Then
                paramQuerystring = CType(Page.Request.QueryString("Name"), String)
                If paramQuerystring = "ANN" Then
                    strFormName = ("EDIOutsourceCheckPrintingForm.aspx?Name=ANN")
                ElseIf paramQuerystring = "EXP" Then
                    strFormName = ("EDIOutsourceCheckPrintingForm.aspx?Name=EXP")
                End If
            End If
            'END : Shilpa N | 03/19/2019 | YRS-AT-4248 |  Passing strFormName depends on the querystring.
            If Not IsPostBack Then
                'Get the Type of Disburement -Annuity or Special dividends
                'Based on the disbursement Type load the datagrid for the current Payroll ran.
                DisbursementType = Request.QueryString.Get("Name").ToString().Trim
                If Not DisbursementType = String.Empty Then
                    l_String_message = LoadDataIntoControls(DisbursementType)
                    If Not l_String_message.ToString().Trim() = String.Empty Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_String_message.Trim(), MessageBoxButtons.OK)
                    End If
                Else
                    Throw New Exception("Unable to find the disbursement type.")
                End If
            Else
                If Request.Form("Yes") = "Yes" Then
                    'Start the process of Check Generation
                    l_String_message = EDI_USProcessPayroll()
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_String_message.Trim(), MessageBoxButtons.OK)
                ElseIf Request.Form("No") = "No" Then

                ElseIf Request.Form("OK") = "OK" Then
                    If CType(Session("ProcessOk"), Boolean) = True Then
                        Session("ProcessOk") = Nothing
                        Response.Redirect("MainWebForm.aspx", False)
                    End If
                End If
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Public Function LoadDataIntoControls(ByVal parameterDisbursementType As String) As String
        Dim l_dataset As New DataSet
        Dim l_datarow As DataRow
        Dim l_stringFileName As String = String.Empty
        Dim l_string_Message As String = String.Empty
        Try
            l_dataset = YMCARET.YmcaBusinessObject.EDIBOClass.GetEDIOutSourceList(parameterDisbursementType)

            If Not l_dataset Is Nothing Then
                If l_dataset.Tables(1).Rows.Count > 0 Then
                    l_datarow = l_dataset.Tables(1).Rows(0)
                    If l_datarow(0).ToString().Trim = "EDI has already generated for the Last Payroll" Then
                        Session("ProcessOk") = True
                        If parameterDisbursementType.ToUpper.Trim() = "ANN" Then
                            l_string_Message = "EDI already processed for the latest payroll"
                        Else
                            l_string_Message = "EDI already processed for the latest special dividends."
                        End If

                        Return l_string_Message
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "EDI Process completed for the current Payroll", MessageBoxButtons.OK)
                        'Exit Function

                    End If
                End If

                Me.DatasetEDIList = l_dataset
                If l_dataset.Tables(1).Rows.Count > 0 Then
                    l_datarow = l_dataset.Tables(0).Rows(0)
                    'commented by Aparna 3rd Oct 2007 -not needed 
                    ''check whether the Payroll belongs to month before current
                    ''if so dont run the process
                    'If l_datarow("intyear") < Date.Now.Year Or l_datarow("intmonth") < Date.Now.Month Then
                    '    Session("ProcessOk") = True
                    '    If parameterDisbursementType.ToUpper.Trim() = "ANN" Then
                    '        l_string_Message = "EDI already processed for the latest payroll"
                    '    Else
                    '        l_string_Message = "EDI already processed for the latest special dividends."
                    '    End If
                    '    Return l_string_Message
                    '    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "EDI Process completed for the Last Payroll", MessageBoxButtons.OK)
                    '    'Exit Function
                    'End If

                    Me.TextBoxLatestPayroll.Text = l_datarow("intmonth").ToString().Trim() + "/" + l_datarow("intyear").ToString().Trim()
                    Me.TextBoxPayrollDate.Text = l_datarow("dtmpayrolldate").ToString().Trim()
                    Session("ProcessId") = l_datarow("guiProcessId").ToString().Trim()
                    'get the check date from the Atspayroll
                    Me.CheckDate = l_datarow("CheckDate").ToString().Trim()



                    If Me.CheckDate.ToString.Trim() = String.Empty Then
                        Session("ProcessOk") = True
                        If parameterDisbursementType.ToUpper.Trim() = "ANN" Then
                            l_string_Message = "Kindly update the payment date in dtmCheckdate field in the AtsPayroll table to proceed further"
                            Return l_string_Message
                        Else
                            l_string_Message = "Kindly update the payment date in dtmCheckdate field in the AtsEXPPayroll table to proceed further"
                            Return l_string_Message
                        End If



                    End If


                    If l_dataset.Tables.Count > 1 Then
                        If l_dataset.Tables(1).Rows.Count > 0 Then

                            'get the incremented Batch number of the checkseries 
                            Me.BatchNo = l_datarow("BatchNo")

                            Me.DataTableEDIList = l_dataset.Tables(1)
                            Me.DataGridEDIList.DataSource = l_dataset.Tables(1)
                            Me.DataGridEDIList.DataBind()
                            LabelRecordsFound.Visible = False
                            Me.ButtonStartProcess.Enabled = True
                            Me.ButtonReport.Enabled = True
                            Me.ButtonFind.Enabled = True

                        Else
                            Session("ProcessOk") = True
                            l_string_Message = "No Records found"
                            Return l_string_Message
                            'LabelRecordsFound.Visible = True
                            'LabelRecordsFound.Text = "No Records found"
                            'Me.ButtonStartProcess.Enabled = False
                            'Me.ButtonReport.Enabled = False
                            'Me.ButtonFind.Enabled = False
                        End If
                    Else
                        Session("ProcessOk") = True
                        l_string_Message = "EDI Already processed"
                        '  MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Payroll Already processed", MessageBoxButtons.OK)
                        Return l_string_Message

                    End If
                End If
            End If
            Return l_string_Message

        Catch ex As Exception
            Throw ex
        Finally
            l_dataset.Dispose()
        End Try
    End Function

    Private Sub GetSelectedRows(ByVal boolforprocess As Boolean)
        'Get only the rows which are selected for the check creation
        Dim l_datagriditem As DataGridItem
        Dim l_checkbox As CheckBox
        Dim l_datatable As New DataTable
        Dim l_dataset As New DataSet
        Dim l_stringFundIdNo As String = String.Empty
        Dim l_datarow As DataRow()
        Dim l_string_SearchExp As String = String.Empty
        Dim l_stringDisbursmentID As String = String.Empty

        Try
            If Not Me.DatasetEDIList Is Nothing Then
                l_dataset = DirectCast(Me.DatasetEDIList, DataSet) 'Directcast(for Ctype) implemented by SR:2010.06.29 for migration
                l_datatable = l_dataset.Tables(1).Clone
            End If
            'If the check box is checked then tht row should not be included for the further process
            If Me.DataGridEDIList.Items.Count > 0 Then
                For Each l_datagriditem In Me.DataGridEDIList.Items
                    l_checkbox = l_datagriditem.FindControl("checkboxSelect")
                    l_stringDisbursmentID = l_datagriditem.Cells(10).Text
                    l_string_SearchExp = "DisbursmentID = '" + l_stringDisbursmentID + "'"
                    l_datarow = l_dataset.Tables(1).Select(l_string_SearchExp)
                    If l_datarow.Length > 0 Then
                        If l_checkbox.Checked = True Then
                            'It means tht the row is selected to be excluded from the check printing action
                            l_datarow(0)("bitExcluded") = 1
                            l_datarow(0)("bitProcessed") = 0
                        Else
                            l_datarow(0)("bitExcluded") = 0
                            If boolforprocess Then
                                l_datarow(0)("bitProcessed") = 1
                            Else
                                l_datarow(0)("bitProcessed") = 0
                            End If
                            l_datatable.ImportRow(l_datarow(0))
                        End If
                    End If
                Next
            End If
            Me.DataTableEDIList = l_datatable
            Me.DatasetEDIList = l_dataset

        Catch ex As Exception
            Throw ex
        Finally
            l_dataset.Dispose()
            l_datatable.Dispose()
        End Try
    End Sub

    Public Function EDI_USProcessPayroll() As String

        Dim bool_Proof As Boolean
        Dim datetime_CheckDate As DateTime
        Dim datatime_PayrollMonthDate As DateTime
        Dim l_String_message As String
        Dim bool_Result As Boolean

        Try
            'Get only those rows which are to be processed.
            'If check box is checked then that record is to be exclude from the list
            Me.GetSelectedRows(True)
            datetime_CheckDate = Convert.ToDateTime(Me.TextBoxPayrollDate.Text.Trim())

            'If Me.CheckDate.ToString.Trim() = String.Empty Then
            '    Me.CheckDate = datetime_CheckDate
            'End If
            'Start Processing of Payroll data.
            Session("ProcessOk") = Nothing
            Dim objEDIProcess As New YMCARET.YmcaBusinessObject.EDIBOClass

            Dim datatable_FileList As DataTable
            bool_Result = objEDIProcess.ProcessEDIChecks(Me.DataTableEDIList, Me.CheckDate, Me.DisbursementType, Me.BatchNo, DatasetEDIList, Me.ProcessId)
            If bool_Result = True Then
                'insert data into AtsEDILogs to show report
                'Update the BitProcessed value in the AtsPayroll/AtsEXPPayroll table 
                'So that the process is not repeated again
                '   YMCARET.YmcaBusinessObject.EDIBOClass.UpdateEDIProcessList(Me.DisbursementType, Me.ProcessId, DatasetEDIList, 1, Me.BatchNo)
                'Set a session to indicate the success of the process
                'so as to refresh the page.
                Session("ProcessOk") = True

                datatable_FileList = objEDIProcess.datatable_FileList
                Session("FTFileList") = Nothing
                Session("FTFileList") = datatable_FileList
                ' Call the calling of the ASPX to copy the file.

                Try
                    Dim popupScript As String = "<script language='javascript'>" & _
                    "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                    "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                        Page.RegisterStartupScript("PopupScript1", popupScript)
                    End If
                Catch ex As Exception
                    'MessageBox.Show(PlaceHolder1, "Error While Writing File", ex.Message, MessageBoxButtons.OK)
                    Throw ex
                End Try
                l_String_message = "Process completed successfully."
                Return l_String_message

            Else
                l_String_message = "Process couldnot be completed"
                Return l_String_message
            End If
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
           
        End Try
    End Function

    Private Sub ButtonStartProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStartProcess.Click
        Dim checkSecurity As String
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim string_Message As String = String.Empty
            string_Message = "Are you sure you want to continue?"
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", string_Message.Trim(), MessageBoxButtons.YesNo)
            Exit Sub
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Dim l_string_FundId As String = Nothing
        Dim l_String_LastName As String = String.Empty
        Dim l_datarow_CurrentRow As DataRow()
        Dim l_DataSet As New DataSet
        Dim l_string_SearchExp As String = String.Empty
        Dim l_string_FundNumber As String = String.Empty
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try
            If Not Me.DataGridEDIList Is Nothing Then

                For j = 0 To Me.DataGridEDIList.Items.Count - 1
                    Me.DataGridEDIList.Items(j).BackColor = System.Drawing.Color.White
                Next

                If Me.TextboxFundId.Text.Trim() = "" And Me.TextboxLastName.Text.Trim() = "" Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter the search criteria.", MessageBoxButtons.OK)
                    Exit Sub
                End If

                If Not Me.TextboxFundId.Text.Trim() = "" Then
                    l_string_FundId = DirectCast(Me.TextboxFundId.Text, String) 'Directcast(for Ctype) implemented by SR:2010.06.29 for migration
                End If
                If Not Me.TextboxLastName.Text = "" Then
                    l_String_LastName = DirectCast(Me.TextboxLastName.Text, String) 'Directcast(for Ctype) implemented by SR:2010.06.29 for migration
                End If


                If Not Me.DatasetEDIList Is Nothing Then
                    l_DataSet = DirectCast(Me.DatasetEDIList, DataSet) 'Directcast(for Ctype) implemented by SR:2010.06.29 for migration
                    If l_DataSet.Tables(1).Rows.Count > 0 Then
                        If Not l_string_FundId = String.Empty Then
                            l_string_SearchExp = "Convert(FundIDNo,'System.String') LIKE '" + l_string_FundId + "%'"
                        End If
                        If Not l_String_LastName = String.Empty And l_string_SearchExp <> "" Then
                            l_string_SearchExp += " and LastName LIKE '" + l_String_LastName + "%'"
                        ElseIf l_string_SearchExp = String.Empty Then
                            l_string_SearchExp = "LastName LIKE '" + l_String_LastName + "%'"
                        End If

                        l_datarow_CurrentRow = l_DataSet.Tables(1).Select(l_string_SearchExp)



                        If l_datarow_CurrentRow.Length > 0 Then
                            'Highlight the rows selected through search
                            For i = 0 To l_datarow_CurrentRow.Length - 1
                                l_string_FundNumber = l_datarow_CurrentRow(i)(1).ToString()
                                For j = 0 To Me.DataGridEDIList.Items.Count - 1
                                    If Me.DataGridEDIList.Items(j).Cells(1).Text.Trim() = l_string_FundNumber Then
                                        Me.DataGridEDIList.Items(j).BackColor = System.Drawing.Color.Orange
                                    End If
                                Next
                            Next
                        Else
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "No records found for the search criteria", MessageBoxButtons.OK)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub

    'Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        ' SelectAll()
    '    Catch ex As SqlException
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

    '    End Try
    'End Sub



    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        End Try
    End Sub


    Private Sub ButtonReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReport.Click
        Dim int_ParameterPayrollProcessed As Integer = 0
        'Dim checkSecurity As String   Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
        Try
            'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            'checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            'If Not checkSecurity.Equals("True") Then
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            'Exit Sub
            ' End If
            'End : YRS 5.0-940
            'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Commented existing code to remove readonly access.

            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Report is pending", MessageBoxButtons.OK)
            'Session("guiProcessId") = Me.ProcessId.ToString.Trim()
            'Me.GetSelectedRows(False)
            ''insert data into AtsEDIProcessinglist to show report
            'YMCARET.YmcaBusinessObject.EDIBOClass.UpdateEDIProcessList(Me.DisbursementType, Me.ProcessId, DatasetEDIList, int_ParameterPayrollProcessed)

            'Report to be called
            '    Session("strReportName") = "EDILogReport"

            '    Dim popupScript As String = "<script language='javascript'>" & _
            '"window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            '"'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            '"</script>"
            '    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
            '        Page.RegisterStartupScript("PopupScript2", popupScript)
            '    End If
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub DataGridEDIList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridEDIList.SortCommand
        Dim dv As New DataView
        Dim SortExpression As String
        Try

            SortExpression = e.SortExpression
            dv = DirectCast(Me.DataTableEDIList, DataTable).DefaultView 'Directcast(for Ctype) implemented by SR:2010.06.29 for migration
            dv.Sort = SortExpression

            If Not Session("EDIListSort") Is Nothing Then

                If SortExpression + " ASC" = Session("EDIListSort").ToString.Trim Then
                    dv.Sort = SortExpression + " DESC"
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
            Else
                dv.Sort = SortExpression + " ASC"
            End If

            Session("EDIListSort") = dv.Sort
            Me.DataGridEDIList.DataSource = Nothing
            Me.DataGridEDIList.DataSource = dv
            Me.DataGridEDIList.DataBind()


        Catch ex As Exception
            ' Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            Throw ex
        End Try
    End Sub
End Class
