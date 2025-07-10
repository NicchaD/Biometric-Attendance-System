'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	UpdateCheckCashDate.aspx.vb
' Author Name		:	Shashi Shekhar 
' Employee ID		:	51426
' Email				:	shashi.singh@3i-infotech.com
' Contact No		:
' Creation Time		:	
' Program Specification Name	:	YMCA_PS_Updating _check_ cashed_dates.doc
' Unit Test Plan Name			:	Updating_check_cashed_dates-Test Cases.xls
' Description					:	This form is used to Import Csv file and update the cashed check in database
'*******************************************************************************
'****************************************************
'Modification History
'****************************************************
'Modified by          Date          Description
'****************************************************
'Nikunj Patel       2009.04.20          Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Shashi Shekhar     2009.10.13          Changing code to resolve BT 1018,BT 1019 and BT 1020
'Shashi Shekhar     2009.11.13          Changing code to resolve BT 1018,BT 1019 and BT 1020
'Neeraj Singh       19/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar     2009.11.23          Changing code to resolve BT 1027
'Shashi Shekhar     2010.01.04          To Implement Gemini issue YRS 5.0-970
'Shashi Shekhar     2010.02.02          To Implement Changes in message shown for exception file 
'Shashi Shekhar     2010.06.03          Migration related changes.
'Shashi Shekhar     2010-07-07          Code review changes.
'Shashi Shekhar     2010-07-16          To Resolve BT - 559 (Deleted Dispose from finally block)
'Manthan Rajguru    2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N           2019.03.19          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'****************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCARET.YmcaBusinessObject





Public Class UpdateCheckCashDate
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateCheckCashDate.aspx")
    Dim strFileName As String = String.Empty

    'End issue id YRS 5.0-940


#Region "Local Variables"
    Dim l_DisbursementDataSet As DataSet            'To store the data fetched from database
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Div1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Dim l_FileDataSet As DataSet
    'To store the data read from CSV file

    Dim IDM As New IDMforAll
#End Region

#Region " Web Form Designer Generated Code "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents lbl_Search_MoreItems As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid_Check As System.Web.UI.WebControls.DataGrid
    Protected WithEvents MultiPageDeathCalc As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoDataFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSelectMatching As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSelectNone As System.Web.UI.WebControls.Button
    Protected WithEvents FileField As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents ButtonImport As System.Web.UI.WebControls.Button
    Protected WithEvents err_lbl As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNote As System.Web.UI.WebControls.Label
    Protected WithEvents first_row As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trNote As System.Web.UI.HtmlControls.HtmlTableRow



    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Properties"
    'Shashi Shekhar:2010-01-04: Used as flag whether exception file needed to write or not
    Public Property ExceptionFileNeeded() As String
        Get
            If Not Session("ExceptionFileNeeded") Is Nothing Then
                Return Session("ExceptionFileNeeded")
            Else
                Return Session("ExceptionFileNeeded") = String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ExceptionFileNeeded") = Value
        End Set
    End Property

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            'Load menu items
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            CheckReadOnlyMode() 'Shilpa N | 03/19/2019 | YRS-AT-4248 | Called the method to check read only mode.

            If IsPostBack AndAlso HelperFunctions.isNonEmpty(l_FileDataSet) Then
                DataGrid_Check.DataSource = (l_FileDataSet.Tables(0).DefaultView)
                DataGrid_Check.DataBind()
                CompareRecord()
            End If

            If Not Me.IsPostBack Then

                Session("DC_l_DisbursementDataSet") = Nothing
                Session("DC_l_FileDataSet") = Nothing
                ButtonSelectAll.Visible = False
                ButtonSelectMatching.Visible = False
                ButtonSelectNone.Visible = False
                ButtonSave.Visible = False
                trNote.Visible = False
            End If

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

    Function ConvertToDecimal(ByVal strNumber As String) As Decimal
        'This function is written to handle empty string conversion to decimal
        Dim strNumberToReturn As Decimal
        Try
            strNumberToReturn = Convert.ToDecimal(strNumber.Trim)
        Catch ex As Exception
            strNumberToReturn = 0
        End Try

        Return strNumberToReturn
    End Function

#Region "General Utility Functions"

    Private Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = ds.Tables(0)
            dg.DataBind()
            dg.Visible = True

        End If

    End Sub
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        If dv Is Nothing OrElse dv.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = dv
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
#End Region


#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        Dim al As New ArrayList
        al.Add(StoreLocalVariablesToCache())
        al.Add(MyBase.SaveViewState())
        Return al
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        Dim al As ArrayList = DirectCast(savedState, ArrayList)
        InitializeLocalVariablesFromCache(al.Item(0))
        MyBase.LoadViewState(al.Item(1))
    End Sub


    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
        'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
        l_DisbursementDataSet = Session("DC_l_DisbursementDataSet")
        l_FileDataSet = Session("DC_l_FileDataSet")
    End Sub
    Private Function StoreLocalVariablesToCache() As Object
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("DC_l_DisbursementDataSet") = l_DisbursementDataSet
        Session("DC_l_FileDataSet") = l_FileDataSet
    End Function
#End Region


#Region "Old logic To read CSV and convert  into DataTable"
    'To get the datatable from CSV file
    'Private Function GetDataTableFromCSV(ByVal strFilePath As String) As DataTable
    '    Try
    '        Dim l_StreamReader_FileRead As StreamReader = File.OpenText(strFilePath)
    '        Dim strline As String = l_StreamReader_FileRead.ReadLine
    '        Dim dtCSV As New DataTable
    '        Dim dr1 As DataRow
    '        Dim l_chr As Char
    '        Dim flag As Integer = 0
    '        Dim flagHeader As Integer = 0
    '        Dim [error] As Boolean = False
    '        Dim row_num As Integer = 0

    '        Dim colCount As Integer = 0

    '        Dim err_str As New System.Text.StringBuilder

    '        Dim l_BoolCommaFlag As Boolean = False


    '        While strline <> Nothing


    '            If InStr(1, strline, l_chr) > 0 Then
    '                l_BoolCommaFlag = True
    '            Else
    '                l_BoolCommaFlag = False
    '            End If


    '            l_chr = Convert.ToChar(34)
    '            strline = strline.Replace(l_chr, "")
    '            Dim strarray() As String = Split(strline, ",")

    '            'For i As Integer = 0 To strarray.Length - 1
    '            '    If strarray(i).StartsWith(l_chr) Then
    '            '        strarray(i) = strarray(i).Replace(l_chr, "")
    '            '    End If
    '            'Next





    '            If flagHeader = 0 Then
    '                Dim header As String() = Split(strline, ",")
    '                If colCount = 0 Then
    '                    colCount = header.Length
    '                End If
    '                flagHeader = 1
    '            End If

    '            ''do some error checking 
    '            'If colCount <> strarray.Length Then

    '            '    err_str.Append(String.Format("error found on line {0} expected {1} columns found {2}<br>", row_num.ToString(), colCount.ToString(), strarray.Length.ToString()))
    '            '    [error] = True
    '            'Else

    '            If strarray.Length > 0 Then
    '                If flag = 0 Then
    '                    For i As Integer = 0 To strarray.Length - 1
    '                        ' dtCSV.Columns.Add("col" & i)
    '                        If i = 0 Then
    '                            dtCSV.Columns.Add("FundID", GetType(Long))
    '                        ElseIf i = 1 Then
    '                            dtCSV.Columns.Add("ParticipantName", GetType(String))
    '                        ElseIf i = 2 Then
    '                            dtCSV.Columns.Add("CheckDate", GetType(Date))
    '                        ElseIf i = 3 Then
    '                            dtCSV.Columns.Add("NetAmount", GetType(Decimal))

    '                        Else
    '                            dtCSV.Columns.Add("CheckNumber", GetType(String))

    '                        End If

    '                    Next

    '                    flag = 1
    '                Else

    '                    dr1 = dtCSV.NewRow



    '                    For i As Integer = 0 To strarray.Length - 1

    '                        If strarray.Length > 5 And l_BoolCommaFlag = True Then
    '                            If i <> 2 Then

    '                                If i = 0 Then
    '                                    dr1(i) = strarray(i)
    '                                End If

    '                                If i = 1 Then
    '                                    strarray(i) = strarray(i) + "," + strarray(i + 1)
    '                                    dr1(i) = strarray(i)
    '                                End If


    '                                If i = 3 Then
    '                                    dr1(i - 1) = strarray(i)
    '                                End If

    '                                If i = 4 Then
    '                                    If strarray(i) = "" Then
    '                                        strarray(i) = 0
    '                                        dr1(i - 1) = strarray(i)
    '                                    Else
    '                                        dr1(i - 1) = strarray(i)
    '                                    End If
    '                                End If


    '                                If i = 5 Then
    '                                    dr1(i - 1) = strarray(i)
    '                                End If
    '                                'dr1(i) = strarray(i)
    '                            End If

    '                        Else
    '                            If i = 3 And strarray(i) = "" Then
    '                                strarray(i) = 0
    '                            End If
    '                            dr1(i) = strarray(i)
    '                        End If

    '                    Next
    '                    If dr1(0).ToString().Trim() = "" And dr1(2).ToString().Trim() = "" Then
    '                    Else
    '                        dtCSV.Rows.Add(dr1)
    '                    End If


    '                End If

    '            End If

    '            ' End If
    '            strline = l_StreamReader_FileRead.ReadLine
    '            row_num += 1
    '        End While
    '        'If [error] Then
    '        '    err_lbl.Text = err_str.ToString()
    '        'End If
    '        l_StreamReader_FileRead.Close()

    '        If dtCSV.Columns.Count >= 5 Then
    '            dtCSV.Columns(0).ColumnName = "FundID"
    '            dtCSV.Columns(1).ColumnName = "ParticipantName"
    '            dtCSV.Columns(2).ColumnName = "CheckDate"
    '            dtCSV.Columns(3).ColumnName = "NetAmount"
    '            dtCSV.Columns(4).ColumnName = "CheckNumber"

    '            'Adding one more col for guiuniqueID which will be used to update the record
    '            dtCSV.Columns.Add("guiUniqueIds", GetType(String))
    '            ' dtCSV.Columns(5).ColumnName = "guiUniqueIds"

    '        Else
    '            Throw New Exception("Invalid  file.")
    '        End If

    '        'dtCSV.Rows.RemoveAt(0)
    '        GetDataTableFromCSV = dtCSV
    '    Catch ex As Exception
    '        '  err_lbl.Text = ex.Message.ToString()
    '        Throw
    '    End Try
    'End Function
#End Region


    Private Sub ButtonImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonImport.Click

        Dim strMessage As String = String.Empty
        Try
            ViewState("previousCheckGridSortExpression") = ""
            If Not FileField.PostedFile.FileName.Equals(String.Empty) Then


                'File Extension check
                If Path.GetExtension(FileField.PostedFile.FileName).ToLower() <> ".csv" Then
                    strMessage = "Please select only CSV file to Import."
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMessage, MessageBoxButtons.Stop)
                    Exit Sub
                End If

                If FillDatagridCashedCheck() = False Then
                    ButtonSelectAll.Visible = False
                    trNote.Visible = False
                    ButtonSelectMatching.Visible = False
                    ButtonSelectNone.Visible = False
                    ButtonSave.Visible = False
                    Exit Sub
                End If
                GetCheckDetailsDataSet()
                Me.ExceptionFileNeeded = "yes" 'Shashi Shekhar:2010-01-04:Used as flag
                CompareRecord()

                '----------------------------------------------------------------------------
                ' Copy all the documents into respective folders.
                Session("FTFileList") = IDM.SetdtFileList
                If IDM.SetdtFileList.Rows.Count > 0 Then
                    Try
                        ' Call the calling of the ASPX to copy the file.
                        Dim popupScript As String = "<script language='javascript'>" & _
                        "var a =  window.open('FT\\CopyFilestoFileServer.aspx?OR=1&DEL=0&CO=1', 'FileCopyPopUp', " & _
                        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                        "</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
                            Page.RegisterStartupScript("PopupScript3", popupScript)
                        End If

                    Catch ex As Exception
                        MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
                        Exit Sub
                    End Try
                End If
                '--------------------------------------------------------------------------------------


            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select the file to Import.", MessageBoxButtons.Stop)
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            strMessage = Nothing
        End Try


    End Sub

    Private Sub ButtonSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        'CompareRecord()
        SelectAll()

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        'To come on main form
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_blnFlag As Boolean = True
        Try
            'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            l_blnFlag = CheckSelection()
            'If user click on save button, without selecting any record from grid, then show alert message
            If (l_blnFlag = True) Then
                MessageBox.Show(PlaceHolder1, "Alert", "Please select record. ", MessageBoxButtons.Stop)
                Exit Sub
            End If

            FillguiUniqueIdColumn()
            UpdateCashedCheck()
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Selected disbursement records have been updated successfully.", MessageBoxButtons.OK)
            UncheckAll()
            GetCheckDetailsDataSet()
            CompareRecord()
            UncheckAll()


        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            l_blnFlag = Nothing
        End Try

    End Sub

    Private Sub ButtonSelectNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectNone.Click
        UncheckAll()
    End Sub

    Private Sub ButtonSelectMatching_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectMatching.Click
        CompareRecord()
    End Sub

    'For Datagrid Sorting
    Private Sub DataGrid_Check_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid_Check.SortCommand
        Dim l_CheckedList As ArrayList = New ArrayList
        Try

            '--------------------------------------------------------------------------------------------
            'Keeping Checked record in arraylist to maintain the checkbox selection after sorting

            For Each itm As DataGridItem In DataGrid_Check.Items
                Dim l_blnChecked As Boolean
                l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                If l_blnChecked Then
                    l_CheckedList.Add(itm.Cells(5).Text.ToString())
                End If
            Next

            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGrid_Check.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(l_FileDataSet) Then
                If Not ViewState("previousCheckGridSortExpression") Is Nothing AndAlso ViewState("previousCheckGridSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousCheckGridSortExpression")) Then
                        ViewState("previousCheckGridSortExpression") = e.SortExpression
                        l_FileDataSet.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        l_FileDataSet.Tables(0).DefaultView.Sort = IIf(l_FileDataSet.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    l_FileDataSet.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousCheckGridSortExpression") = e.SortExpression
                End If
                BindGrid(DataGrid_Check, l_FileDataSet.Tables(0).DefaultView)
                CompareRecord()

                '-------------------------------------------------------------------------
                'Checking all check box by getting id from arraylist in which checked id is stored before sorting
                Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
                Dim dgi As DataGridItem
                If DataGrid_Check.Items.Count > 0 Then
                    For Each dgi In DataGrid_Check.Items
                        CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                        If l_CheckedList.Contains(dgi.Cells(5).Text.ToString()) Then
                            CheckBoxSelect.Checked = True
                        Else
                            CheckBoxSelect.Checked = False
                        End If
                    Next
                End If
                '------------------------------------------------------------------------

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_CheckedList = Nothing
        End Try
    End Sub

#Region "Private Functions "

    Private Function VerifyDataInDataTable(ByVal dtCSV As DataTable) As Boolean
        Dim l_bool_flag As Boolean = True
        Dim l_RowId As String
        Dim l_RowNo As Long

        Try
            l_RowNo = 0
            For Each dtRow As DataRow In dtCSV.Rows
                l_RowNo = l_RowNo + 1
                'If Check number is empty then set flag false
                If dtRow(4).ToString().Trim().Equals(String.Empty) Then
                    l_RowId = l_RowId + l_RowNo.ToString() + ", "
                    l_bool_flag = False

                    'If Check Amount is empty then set flag false
                ElseIf dtRow(3).ToString().Trim().Equals(String.Empty) Or dtRow(3).ToString().Trim() = "0" Then
                    l_RowId = l_RowId + l_RowNo.ToString() + ", "
                    l_bool_flag = False

                    'If Issue Date is empty then set flag false
                ElseIf dtRow(2).ToString().Trim().Equals(String.Empty) Or dtRow(2).ToString().Trim() = "" Then
                    l_RowId = l_RowId + l_RowNo.ToString() + ", "
                    l_bool_flag = False

                    'ElseIf Not (Date.TryParseExact(dtRow(2).ToString().Trim(), dateFormats, culture, DateTimeStyles.None, dateString)) Then
                    '    l_RowId = l_RowId + l_RowNo.ToString() + ", "
                    '    l_bool_flag = False

                    'If fund Id is empty then set flag false
                ElseIf dtRow(0).ToString().Trim().Equals(String.Empty) Or dtRow(0).ToString().Trim() = "" Then
                    l_RowId = l_RowId + l_RowNo.ToString() + ", "
                    l_bool_flag = False

                End If

            Next

            'Show message
            If l_bool_flag = False Then
                l_RowId = l_RowId.Remove(l_RowId.LastIndexOf(","c), 1)
                MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Some data is missing for the following record Nos: " & l_RowId, MessageBoxButtons.Stop)
            End If

            Return l_bool_flag

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)

        Finally
            l_bool_flag = Nothing
            l_RowId = Nothing

        End Try
    End Function

    'To fill the Data grid with CSV file Records
    Private Function FillDatagridCashedCheck() As Boolean
        Dim dtFileData As DataTable = New DataTable
        Dim dtFileDataClone As DataTable = New DataTable
        l_FileDataSet = New DataSet
        Dim strImportFilePath As String
        Dim l_hTable As Hashtable = New Hashtable


        Try
            'Save file into Imported Folder
            strImportFilePath = SaveFileIntoImportFolder()

            'If key not avilable in web .confog file for folder path to import the CSV file then show the message
            If strImportFilePath.ToLower() = "no path entry" Then
                MessageBox.Show(PlaceHolder1, "Alert", "Folder Path not defined in settings.", MessageBoxButtons.Stop)
                Return False
            End If

            'If Folder doesn't exist to import the CSV file then show the message
            If strImportFilePath.ToLower() = "no folder" Then
                MessageBox.Show(PlaceHolder1, "Alert", "Defined folder path doesn't exist.", MessageBoxButtons.Stop)
                Return False
            End If



            If strImportFilePath <> String.Empty Then

                'Calling function to parse the CSV file in data table
                dtFileData = HelperFunctions.ParseCSVFile(strImportFilePath)

                'Check If file is empty
                If dtFileData.Columns(0).Caption = "NoData" Then
                    MessageBox.Show(PlaceHolder1, "Alert", "File is empty. ", MessageBoxButtons.Stop)
                    Return False
                End If

                'Check if File has invalid format
                If dtFileData.Columns.Count <> 5 Then
                    MessageBox.Show(PlaceHolder1, "Alert", "Imported file is not in valid CSV format. Please correct the file and try importing again.", MessageBoxButtons.Stop)
                    Return False
                End If


                'Check if First row is header, then remove first row before binding in grid. 
                If (dtFileData.Rows(0).Item(0).ToString().Trim.ToLower = "fundid") Or (dtFileData.Rows(0).Item(0).ToString().Trim.ToLower = "fund id") Then
                    dtFileData.Rows.RemoveAt(0)
                End If


                'Check if Check number,Fund Id,issue date and Amout is missing then return false (Check Mandatory fields)
                If VerifyDataInDataTable(dtFileData) = False Then
                    Return False
                End If



                'Adding one more col for guiuniqueID, which we will use to update the record
                dtFileData.Columns.Add("guiUniqueIds", GetType(String))
                dtFileData.Columns.Add("FundID", GetType(Long))

                'Removing comma from amount column
                For Each dr As DataRow In dtFileData.Rows
                    dr("col003") = RemoveComma(dr("col003").ToString().Trim)
                Next

                dtFileDataClone = dtFileData.Clone 'cloning datatable

                'Changing the datatype for sorting purpose, because ParseCSV class return all field as string
                dtFileDataClone.Columns(0).DataType = GetType(Long)
                dtFileDataClone.Columns(0).AllowDBNull = True
                dtFileDataClone.Columns(2).DataType = GetType(Date)
                dtFileDataClone.Columns(2).AllowDBNull = True
                dtFileDataClone.Columns(3).DataType = GetType(Decimal)
                dtFileDataClone.Columns(3).AllowDBNull = True
                dtFileDataClone.Columns(4).AllowDBNull = True


                'Importing row in datatable
                For Each dr As DataRow In dtFileData.Rows

                    If dr.Item(0).ToString = "" Then
                        dr.Item(0) = Nothing
                    End If

                    If dr.Item(2).ToString = "" Then
                        dr.Item(2) = Nothing
                    End If

                    If dr.Item(3).ToString = "" Then
                        dr.Item(3) = Nothing
                    End If

                    If dr.Item(4).ToString = "" Then
                        dr.Item(4) = Nothing
                    End If

                    dtFileDataClone.ImportRow(dr)

                Next

                l_FileDataSet.Tables.Add(dtFileDataClone)
                BindGrid(DataGrid_Check, l_FileDataSet)

                ButtonSelectAll.Visible = True
                trNote.Visible = True
                ButtonSelectMatching.Visible = True
                ButtonSelectNone.Visible = True
                ButtonSave.Visible = True

                Return True
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            strImportFilePath = Nothing


        End Try
    End Function

    'To get the record from database in reference with CSV file checkno
    Private Function GetCheckDetailsDataSet() As DataSet
        Dim UpdateCheckCashDateBO As UpdateCheckCashDateBOClass = Nothing
        Dim list As New ArrayList

        Try
            For Each itm As DataGridItem In DataGrid_Check.Items
                list.Add(itm.Cells(5).Text.ToString().Trim())
            Next
            l_DisbursementDataSet = UpdateCheckCashDateBO.GetCheckDetailsDataSet(list)
            Return l_DisbursementDataSet
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            UpdateCheckCashDateBO = Nothing
            list = Nothing
        End Try

    End Function

    'To Compare the CSV record with our database record and do required formating in grid accordingly
    Private Function CompareRecord()
        Dim l_builderCheckNo As StringBuilder = New StringBuilder
        Dim expression As StringBuilder = New StringBuilder
        Dim l_BooleanISCheckExist As Boolean = False
        Dim l_BooleanISCheckPaid As Boolean = False
        Dim l_BooleanISamountMatched As Boolean = False
        Dim l_BooleanISFundIdMatched As Boolean = False
        Dim l_BooleanISIssuedDateMatched As Boolean = False
        Dim l_BooleanISDuplicateCheckNo As Boolean = False
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim l_Amount As Decimal
        Dim l_FundId As Long
        Dim l_IssuedDate As Date
        Dim l_RowCounter As Int16
        Dim expressionForPaid As StringBuilder = New StringBuilder
        ''Dim dtInvalidCheck As DataTable = New DataTable
        Dim dtException As New System.Data.DataTable

        Dim String_SourceFolder As String
        Dim String_SourceFile As String
        Dim String_DestFile As String

        Try
            'Create columns For dtException Datatable
            dtException.Columns.Add("Fund ID")
            dtException.Columns.Add("Participant Name")
            dtException.Columns.Add("Check Date")
            dtException.Columns.Add("Net Amount")
            dtException.Columns.Add("Check Number")



            If DataGrid_Check.Items.Count > 0 Then
                For Each itm As DataGridItem In DataGrid_Check.Items
                    CheckBoxSelect = itm.FindControl("CheckBoxSelect")
                    l_builderCheckNo = l_builderCheckNo.Append(itm.Cells(5).Text.Trim)

                    Decimal.TryParse(itm.Cells(4).Text.Trim, l_Amount)
                    Long.TryParse(itm.Cells(1).Text.Trim, l_FundId)
                    Date.TryParse(itm.Cells(3).Text.Trim, l_IssuedDate)

                    expression = expression.Append("chvDisbursementNumber ='" + l_builderCheckNo.ToString().Trim + "' ")

                    '----------------------------------------------------------------------------------

                    'To set flag for Amount,Check Date,Fund No,Already Cashed,and duplicate check no

                    Dim dr() As DataRow = l_DisbursementDataSet.Tables(0).Select(expression.ToString())

                    If dr.Length = 0 Then

                    ElseIf dr.Length > 1 Then
                        l_BooleanISCheckExist = True  'If check exist in the system
                        l_BooleanISDuplicateCheckNo = True
                    Else
                        l_BooleanISCheckExist = True  'If check exist in the system
                        'Amount matched
                        If l_Amount = CType(dr(0)(2).ToString().Trim, Decimal) Then
                            l_BooleanISamountMatched = True
                        End If

                        'Fund No matched
                        If l_FundId = CType(dr(0)(5).ToString().Trim, Long) Then
                            l_BooleanISFundIdMatched = True
                        End If

                        'Check Date matched
                        If l_IssuedDate = CType(dr(0)(3).ToString().Trim, Date) Then
                            l_BooleanISIssuedDateMatched = True
                        End If

                        'Check is already paid in system
                        If CType(dr(0)(4), Boolean) = True Then
                            l_BooleanISCheckPaid = True
                        End If

                    End If

                    '----------------------------------------------------------------------------------

                    'According to flag, formating of grid start here
                    If l_BooleanISCheckExist Then
                        itm.Cells(0).Enabled = True 'Check box is enabled in grid
                        If l_BooleanISDuplicateCheckNo = True Then
                            itm.BackColor = itm.BackColor.LightSkyBlue
                            itm.Cells(0).Enabled = False 'Check box is disabled in grid
                            dtException = AddRowInDataTable(dtException, itm) 'Shashi Shekhar:2010-01-05:Adding row (in which wxception occurs) in exception datatable
                        Else
                            If l_BooleanISCheckPaid = True Then
                                Dim indx As Int32 = itm.ItemIndex()
                                DataGrid_Check.Items(indx).Visible = False
                            ElseIf (l_BooleanISCheckPaid = False) And ((l_BooleanISamountMatched = False) Or (l_BooleanISFundIdMatched = False) Or (l_BooleanISIssuedDateMatched = False)) Then
                                itm.BackColor = itm.BackColor.LightSalmon
                                CheckBoxSelect.Checked = False
                                dtException = AddRowInDataTable(dtException, itm) 'Shashi Shekhar:2010-01-05:Adding row (in which wxception occurs) in exception datatable
                            Else
                                CheckBoxSelect.Checked = True
                            End If
                        End If
                    Else
                        dtException = AddRowInDataTable(dtException, itm) 'Shashi Shekhar:2010-01-05:Adding row (in which wxception occurs) in exception datatable
                        itm.BackColor = itm.BackColor.LightYellow
                        itm.Cells(0).Enabled = False 'Check box is disabled in grid

                    End If
                    '----------------------------------------------------------------------------------------

                    'Reinitialization of variables
                    expression.Length = 0
                    l_builderCheckNo.Length = 0
                    l_BooleanISCheckExist = False
                    l_BooleanISamountMatched = False
                    l_BooleanISCheckPaid = False
                    l_BooleanISFundIdMatched = False
                    l_BooleanISIssuedDateMatched = False
                    l_BooleanISDuplicateCheckNo = False
                    l_Amount = 0
                    l_FundId = 0
                    l_RowCounter = 0
                    expressionForPaid.Length = 0

                Next

                '-------------------------------------------------------------------------------------------------------------------
                'Shashi Shekhar:2010-01-04:Check whether there is need to write exception file or not. For YRS 5.0-970
                If dtException.Rows.Count > 0 And Me.ExceptionFileNeeded = "yes" Then

                    Dim folderpath As String = ConfigurationSettings.AppSettings("UpdateCashedcheckDate") + "\\" + "IMPORT\\"

                    String_SourceFolder = folderpath
                    'Check whether folder exist or not 
                    If Not System.IO.Directory.Exists(folderpath) Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Destination folder not exist to save exception file.", MessageBoxButtons.Stop)
                    Else
                        Try
                            'Using same file name which is Imported by appending the text "_exceptions"
                            strFileName = strFileName.Substring(0, strFileName.Length - 4)
                            strFileName = strFileName + "_exceptions"
                            strFileName = strFileName + ".CSV"
                            folderpath = folderpath + strFileName
                            DataTable2CSV(dtException, folderpath, ",")

                            folderpath = folderpath.Substring(folderpath.LastIndexOf("/") + 1, folderpath.Length - (folderpath.LastIndexOf("/")) - 1)
                            Dim strMessage As String = "An exception file named """ + strFileName + """ has been written to the source directory."
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMessage, MessageBoxButtons.Stop)

                            String_SourceFile = strFileName
                            String_DestFile = strFileName
                            CopyToServer(String_SourceFolder, String_SourceFile, String_DestFile)

                        Catch ex As Exception
                            'Show message if user has no right on folder
                            Dim l_String_Exception_Message As String
                            l_String_Exception_Message = "Logged-In user has no permission to write exception file in destination folder."
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_String_Exception_Message, MessageBoxButtons.Stop)
                            Exit Try
                        End Try

                    End If

                End If
                '---------------------------------------------------------------------------------------------------------------------------------------

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_BooleanISCheckExist = Nothing
            l_BooleanISamountMatched = Nothing
            l_BooleanISCheckPaid = Nothing
            l_builderCheckNo = Nothing
            expression = Nothing
            l_Amount = Nothing
            l_BooleanISIssuedDateMatched = Nothing
            l_BooleanISFundIdMatched = Nothing
            l_RowCounter = Nothing
            l_BooleanISDuplicateCheckNo = Nothing
            'Shashi Shekhar:2010-01-04:Set nothing

            Me.ExceptionFileNeeded = String.Empty
            strFileName = String.Empty



        End Try

    End Function

    'To check all enabled records in grid
    Private Function SelectAll()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem
        Try
            If DataGrid_Check.Items.Count > 0 Then
                For Each dgi In DataGrid_Check.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    If dgi.Cells(0).Enabled = True Then 'If Check box is enabled in grid
                        CheckBoxSelect.Checked = True
                    End If
                Next
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            CheckBoxSelect = Nothing
            dgi = Nothing
        End Try

    End Function

    'To save the Imported CSV file 
    Private Function SaveFileIntoImportFolder() As String
        Dim uploadFile As HttpPostedFile
        Dim strImportFolderPath As String = String.Empty
        Dim strArrayFileName() As String

        Dim String_SourceFolder As String
        Dim String_SourceFile As String
        Dim String_DestFile As String

        'Shashi Shekhar Singh:2010-01-19: For YRS 5.0-970
        Dim l_dataset As DataSet

        Try
            uploadFile = FileField.PostedFile

            'check folder path key
            strImportFolderPath = ConfigurationSettings.AppSettings("UpdateCashedcheckDate")
            If strImportFolderPath = Nothing Then
                strImportFolderPath = "No path entry"
                Return strImportFolderPath
            End If

            'Check whether folder exist or not 
            strImportFolderPath = ConfigurationSettings.AppSettings("UpdateCashedcheckDate") + "\\" + "IMPORT\\"
            String_SourceFolder = strImportFolderPath
            If Not System.IO.Directory.Exists(strImportFolderPath) Then
                strImportFolderPath = "no folder"
                Return strImportFolderPath
            End If

            strArrayFileName = uploadFile.FileName.Split("\")
            strFileName = strArrayFileName.GetValue(strArrayFileName.Length - 1)
            strFileName = strFileName.Remove(strFileName.Length - 4, 4)
            strFileName = strFileName + "-" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")
            strFileName = strFileName.Replace(":"c, "."c)
            strFileName = strFileName + ".csv"
            strImportFolderPath = strImportFolderPath & strFileName

            If Not File.Exists(strImportFolderPath) Then
                uploadFile.SaveAs(strImportFolderPath)

                'Shashi Shekhar Singh:2010-01-21: For YRS 5.0-970
                If IDM.DatatableFileList(False) Then

                Else
                    Throw New Exception("Unable to Process Update cashed Check Date, Could not create dependent table")
                End If


                String_SourceFile = strFileName
                String_DestFile = strFileName
                CopyToServer(String_SourceFolder, String_SourceFile, String_DestFile)


                '---------------------------------------------------------------------------


            End If

        Catch ex As Exception
            Throw ex

        Finally
            uploadFile = Nothing
        End Try
        Return strImportFolderPath
    End Function

    'To fill the guiUniqueID column
    Private Function FillguiUniqueIdColumn()
        Dim l_builderCheckNo As StringBuilder = New StringBuilder
        Dim expression As StringBuilder = New StringBuilder

        Try
            If DataGrid_Check.Items.Count > 0 Then
                For Each itm As DataGridItem In DataGrid_Check.Items
                    If itm.Cells(0).Enabled = True Then 'If check box is enabled in grid
                        l_builderCheckNo = l_builderCheckNo.Append(itm.Cells(5).Text.Trim)
                        expression = expression.Append("chvDisbursementNumber ='" + l_builderCheckNo.ToString() + "' ")

                        For Each dr As DataRow In l_DisbursementDataSet.Tables(0).Select(expression.ToString())
                            itm.Cells(6).Text = dr.Item("guiUniqueID").ToString()
                        Next
                        expression.Length = 0
                        l_builderCheckNo.Length = 0
                    End If
                Next
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_builderCheckNo = Nothing
            expression = Nothing

        End Try

    End Function

    'To Update the Cashed check record in database
    Private Function UpdateCashedCheck()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim UpdateCheckCashDateBO As UpdateCheckCashDateBOClass = Nothing
        Dim list As New ArrayList
        Try
            If DataGrid_Check.Items.Count > 0 Then
                For Each itm As DataGridItem In DataGrid_Check.Items
                    Dim l_blnChecked As Boolean
                    l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                    If l_blnChecked Then
                        list.Add(itm.Cells(6).Text.ToString().Trim())
                    End If
                Next

                UpdateCheckCashDateBO.UpdateCashedChecks(list)
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            UpdateCheckCashDateBO = Nothing
            list = Nothing

        End Try

    End Function

    'To Uncheck all records in grid
    Private Function UncheckAll()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Try
            If DataGrid_Check.Items.Count > 0 Then
                For Each itm As DataGridItem In DataGrid_Check.Items
                    Dim l_blnChecked As Boolean
                    CheckBoxSelect = itm.FindControl("CheckBoxSelect")
                    l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                    If l_blnChecked Then
                        CheckBoxSelect.Checked = False
                    End If
                Next
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        

        End Try

    End Function

    'To check whether any record in grid is selected or not.
    Private Function CheckSelection()
        Dim l_blnChecked As Boolean
        Try
            If DataGrid_Check.Items.Count > 0 Then
                For Each itm As DataGridItem In DataGrid_Check.Items
                    l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                    If l_blnChecked Then
                        Return False
                    End If
                Next
                Return True
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            l_blnChecked = Nothing
        End Try

    End Function

    'To remove comma from string
    Private Function RemoveComma(ByVal amount As String) As String
        Dim tChar As Char = Nothing
        Dim tResult As StringBuilder = New StringBuilder(amount.Length)
        Dim tLevel As Integer = 0
        Try
            For Each tChar In amount
                Select Case tChar
                    Case ","
                        If 0 < tLevel Then
                            tResult.Append(tChar)
                        End If
                    Case Else
                        tResult.Append(tChar)
                End Select
            Next
            Return amount
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            tChar = Nothing
            tResult = Nothing
            tLevel = Nothing
        End Try
    End Function


    'Shashi Shekhar:04-01-2010: Function to add the row in datatable
    Private Function AddRowInDataTable(ByVal objDataTable As DataTable, ByVal itm As DataGridItem) As DataTable
        Try
            'Adding some data in the rows of this DataTable
            Dim dr As DataRow = objDataTable.NewRow()
            dr.Item(0) = itm.Cells(1).Text
            dr.Item(1) = itm.Cells(2).Text
            dr.Item(2) = itm.Cells(3).Text
            dr.Item(3) = itm.Cells(4).Text
            dr.Item(4) = itm.Cells(5).Text

            objDataTable.Rows.Add(dr)
            Return objDataTable
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Function

#End Region

    'Shashi Shekhar Singh:2010-01-19: For YRS 5.0-970
    Private Function CopyToServer(ByVal p_String_SourceFolder As String, ByVal p_String_SourceFile As String, ByVal p_String_DestFile As String)

        Dim l_dataset As DataSet
        Dim l_DataRow As DataRow
        Try


            l_dataset = YMCARET.YmcaBusinessObject.UpdateCheckCashDateBOClass.GetExceptionMetaOutputFileType()
            If l_dataset Is Nothing Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Unable to find Update Cashed Check Date Values in the MetaOutput file.", MessageBoxButtons.Stop)
                Exit Function
            End If

            If l_dataset.Tables(0).Rows.Count > 0 Then
                l_DataRow = l_dataset.Tables(0).Rows(0)
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Unable to find Update Cashed Check Date  Configuration Values in the MetaOutputFileTypes table.", MessageBoxButtons.Stop)
                Exit Function
            End If



            IDM.AddFileListRow(p_String_SourceFolder, p_String_SourceFile, Convert.ToString(l_DataRow("OutputDirectory")), p_String_DestFile)
            'After adding this Record it will automatically copied by the copytoserver.aspx file. 

        Catch
            Throw

        End Try

    End Function

    'START: Shilpa N | 03/19/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonImport.Enabled = False
            ButtonImport.ToolTip = toolTip
        End If
    End Sub
    'END: Shilpa N | 03/19/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

#Region "Public Functions "
    'To remove duplicate row from datatable
    Public Function RemoveDuplicateRows(ByVal dTable As DataTable, ByVal colName As String) As DataTable
        Dim l_hTable As Hashtable
        Dim l_duplicateList As ArrayList

        Try
            l_hTable = New Hashtable
            l_duplicateList = New ArrayList
            Dim drow As DataRow
            For Each drow In dTable.Rows
                If l_hTable.Contains(drow(colName)) Then
                    l_duplicateList.Add(drow)
                Else
                    l_hTable.Add(drow(colName), String.Empty)
                End If
            Next
            Dim dRow1 As DataRow
            For Each dRow1 In l_duplicateList
                dTable.Rows.Remove(dRow1)
            Next
            Return dTable


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_duplicateList = Nothing
            l_hTable = Nothing
        End Try

    End Function




    'Shashi Shekhar:04-01-2010:Function To write datatable in csv file
    Public Shared Sub DataTable2CSV(ByVal table As DataTable, ByVal filename As String, ByVal seperateChar As String)

        Dim sr As StreamWriter = Nothing

        Try

            sr = New StreamWriter(filename)
            Dim seperator As String = ""
            Dim builder As New StringBuilder
            For Each col As DataColumn In table.Columns

                builder.Append(seperator).Append(col.ColumnName)

                seperator = seperateChar
            Next

            sr.WriteLine(builder.ToString())

            For Each row As DataRow In table.Rows

                seperator = ""
                builder = New StringBuilder
                For Each col As DataColumn In table.Columns

                    builder.Append(seperator).Append(row(col.ColumnName))

                    seperator = seperateChar
                Next


                sr.WriteLine(builder.ToString())

            Next
        Finally


            If Not sr Is Nothing Then
                sr.Close()
            End If

        End Try
    End Sub


#End Region



    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click

    End Sub
End Class

