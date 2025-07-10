'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	ACHDebitExportForm.aspx.vb
' Author Name		:	Ashish Srivastava
' Employee ID		:	51821
' Email				:	 
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : 
'*******************************************************************************
'Changed By:        On:                  IssueId


'*************************************************************************************************************************************************************************************
'Modification History
'*************************************************************************************************************************************************************************************
'Modified By			         Date		                	    Description
'*************************************************************************************************************************************************************************************
'Ashish Srivastava               07-Dec-2009                        Changed error message and padding with zero in imported Ymca no
'Ashish Srivastava               13-Dec-2009                        Remove logic of comma seperated Ymca no 
'Neeraj Singh                    12/Nov/2009                         Added form name for security issue YRS 5.0-940
'Ashish Srivastava               2010.07.06                          Enhancements08 changes 
'Anudeep                         2013-12-16                          BT:2311-13.3.0 Observations
'Manthan Rajguru                 2015.09.16                          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*************************************************************************************************************************************************************************************
#Region "Import Namespace"
Imports System.IO.File
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.IO
Imports System.Text
Imports YMCARET.YmcaBusinessObject

#End Region

Public Class ACHDebitImportForm
    Inherits System.Web.UI.Page


    Dim strFormName As String = New String("ACHDebitImportForm.aspx")


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderMessage As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonImport As System.Web.UI.WebControls.Button
    Protected WithEvents FileField As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents DatagridACHDebImport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextBoxTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButonMatchRecceipts As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolderACHDebitImportProcess As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonPostReceipts As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDeSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRecalculate As System.Web.UI.WebControls.Button
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button
    Protected WithEvents btnOK As System.Web.UI.WebControls.Button
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Try
            If IsPostBack Then
                'AA:16.12.2013 - BT:2311 Commented below code and kept in buton yes click event
                'If (Session("IsDone") = True) Then
                '    Session("IsDone") = False
                '    Response.Redirect("MainWebForm.aspx", False)
                'End If

                'If Not Session("ProcessOk") = Nothing Then
                '    If Request.Form("YES") = "Yes" And Session("ProcessOk") = True Then
                '        Session("ProcessOk") = Nothing
                '        AchDebitPostReceipts()
                '    End If
                '    '***********

                'End If
                'If Not Session("MatchReceipts") = Nothing Then

                '    If Request.Form("YES") = "Yes" And Session("MatchReceipts") = True Then
                '        Response.Redirect("ACHDebitReceiptApplicationForm.aspx", False)
                '        Session("MatchReceipts") = Nothing


                '    End If

                'End If
            End If
            If Not IsPostBack Then
                Session("IsDone") = False
                If Not Session("MatchReceipts") = Nothing Then

                    If Not Session("BackMatchReceipts") = Nothing And Session("MatchReceipts") = True Then
                        If Session("BackMatchReceipts") = True And Not Session("l_dataset_ACHInsert") Is Nothing Then
                            Me.DatagridACHDebImport.DataSource = Session("l_dataset_ACHInsert")
                            Me.DatagridACHDebImport.DataBind()
                            SetSelected()
                            Me.ButtonImport.Enabled = False
                            FileField.Disabled = True
                            Me.ButtonPostReceipts.Enabled = True
                            Me.ButonMatchRecceipts.Enabled = True
                            Me.ButtonRecalculate.Enabled = True
                            'AA:16.12.2013 - BT:2311 to tnable if there exists a data in grid
                            Me.ButtonDeSelectAll.Enabled = True
                            Session("BackMatchReceipts") = Nothing
                            Session("MatchReceipts") = Nothing
                        End If
                    End If
                End If
            End If

            
        Catch ex As SqlException

            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Catch ex As Exception

            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub



#Region "Private Methods"
   
    Private Sub FillDatagridACHImportProcess()
        Dim dtFileData As DataTable
        Dim strImportFilePath As String
        Try

            'Save file into Imported Folder
            strImportFilePath = SaveFileIntoImportFolder()

            If strImportFilePath <> String.Empty Then

                dtFileData = getDataTableFromXLS(strImportFilePath)

                If verifyDataIndataTable(dtFileData) = False Then
                    Exit Sub
                End If

                If dtFileData.Rows.Count > 0 Then

                    Dim d_totalFileAmount As Decimal = 0.0

                    For Each dtRow As DataRow In dtFileData.Rows
                        d_totalFileAmount = d_totalFileAmount + Convert.ToDecimal(dtRow("AMOUNT"))
                    Next

                    'd_totalFileAmount = dtFileData.Compute("Sum(AMOUNT)", "")

                    'Session("d_totalFileAmount") = d_totalFileAmount

                    If ValidateTotalACHAmount(d_totalFileAmount) = False Then
                        Exit Sub
                    End If
                    'Validate Individual YMCA Total Amount

                    Dim l_dataset_ACHInsert As DataSet
                    If ValidateYmcaACHAmount(dtFileData, l_dataset_ACHInsert) = False Then
                        Exit Sub
                    End If


                    '**************
                    'Dim strYmcaNo As String = String.Empty
                    'strYmcaNo = GetYMCANos(dtFileData)
                    'If strYmcaNo = "" Then
                    '    MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Please select a BatchNo :", MessageBoxButtons.OK)
                    '    Exit Sub
                    'End If

                    'CreateAchDebitTableStructure(dtFileData)
                    'FillACHDebitDataFromDB(strYmcaNo, dtFileData)

                    ''Dim dtACHInsert As New DataTable
                    ''Dim l_dataset_ACHInsert As New DataSet
                    ''dtACHInsert = CType(Session("DtAchImportProcess"), DataTable)
                    'l_dataset_ACHInsert.Tables.Add(dtFileData)
                    'l_dataset_ACHInsert.Tables(0).TableName = "ACHDebitsImport"
                    '**************************************************************
                    Session("l_dataset_ACHInsert") = CType(l_dataset_ACHInsert, DataSet)


                    Me.DatagridACHDebImport.DataSource = l_dataset_ACHInsert
                    Me.DatagridACHDebImport.DataBind()
                    If Me.DatagridACHDebImport.Items.Count > 0 Then
                        ButtonImport.Enabled = False
                        Me.ButtonDeSelectAll.Enabled = True
                        Me.ButtonPostReceipts.Enabled = True
                        Me.ButonMatchRecceipts.Enabled = True
                        Me.ButtonRecalculate.Enabled = True
                        FileField.Disabled = True
                        TextBoxTotal.Text = 0
                        SetSelected()
                    End If

                    'Session("strImportFilePath") = Nothing
                End If

            End If
        Catch
            Throw
        End Try
    End Sub
    'Private Function GetYMCANos(ByVal dtTemp As DataTable) As String

    '    Dim string_YMCANos As New StringBuilder
    '    Dim dt As DataTable = New DataTable
    '    dt = dtTemp

    '    Try

    '        For Each dtrow As DataRow In dt.Rows
    '            If dtrow.RowState <> DataRowState.Deleted Then
    '                If dtrow(0).GetType.ToString <> "System.DBNull" Then
    '                    dtrow(0) = dtrow(0).ToString().PadLeft(6, "0")
    '                    string_YMCANos.Append(dtrow(0))
    '                    string_YMCANos.Append(",")
    '                End If
    '            End If
    '        Next
    '        Return string_YMCANos.ToString()
    '    Catch
    '        Throw
    '    End Try
    'End Function
   
    Private Function verifyDataIndataTable(ByVal dtCSV As DataTable) As Boolean
        
        Dim l_bool_flag As Boolean = False
        Try
            For Each dtRow As DataRow In dtCSV.Rows
                If dtRow(0).ToString().Trim().Equals(String.Empty) Then
                    l_bool_flag = False
                    'commented by Ashish 07-Dec-2009
                    'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Invalid YMCA No in Imported Excel Sheet.", MessageBoxButtons.Stop)
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "One or more YMCA Nos have not been specified in the imported file.", MessageBoxButtons.Stop)
                    HelperFunctions.ShowMessageToUser("One or more YMCA Nos have not been specified in the imported file.", EnumMessageTypes.Error)
                    Exit For
                Else
                    If dtRow(2).ToString().Trim().Equals(String.Empty) Then
                        l_bool_flag = False
                        'commented by Ashish 07-Dec-2009
                        'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Amount Column is blank for YMCA No " & dtRow(0).ToString().Trim() & " in Imported Excel Sheet.", MessageBoxButtons.Stop)
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Amount has not been specified for one or more YMCAs in the imported file.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Amount has not been specified for one or more YMCAs in the imported file.", EnumMessageTypes.Error)
                        Exit For
                    Else
                        If IsNumeric(dtRow(2).ToString().Trim()) Then
                            If CType(dtRow(2), Decimal) = 0 Then
                                l_bool_flag = False
                                'commented by Ashish 07-Dec-2009
                                'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Amount Column Value is Zero for YMCA No " & dtRow(0).ToString().Trim() & " in Imported Excel Sheet.", MessageBoxButtons.Stop)
                                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                                'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Amount has not been specified for one or more YMCAs in the imported file.", MessageBoxButtons.Stop)
                                HelperFunctions.ShowMessageToUser("Amount has not been specified for one or more YMCAs in the imported file.", EnumMessageTypes.Error)

                                Exit For
                            End If
                            dtRow(0) = dtRow(0).ToString().Trim().PadLeft(6, "0")
                            l_bool_flag = True
                        Else
                            l_bool_flag = False
                            'commented by Ashish 07-Dec-2009
                            'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Data Type Mismatch in Amount Column of Imported Excel Sheet.", MessageBoxButtons.Stop)
                            'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                            'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Invalid amount has been specified for one or more YMCAs in the imported file.", MessageBoxButtons.Stop)
                            HelperFunctions.ShowMessageToUser("Invalid amount has been specified for one or more YMCAs in the imported file.", EnumMessageTypes.Error)
                            Exit For
                        End If
                    End If
                End If

            Next
            Return l_bool_flag
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function getDataTableFromXLS(ByVal strFilePath As String) As DataTable
        Try
            Dim l_StreamReader_FileRead As StreamReader = File.OpenText(strFilePath)
            Dim strline As String = l_StreamReader_FileRead.ReadLine
            Dim dtCSV As New DataTable
            Dim dr1 As DataRow
            Dim l_chr As Char
            Dim flag As Integer = 0

            l_chr = Convert.ToChar(34)
            While strline <> Nothing
                strline = strline.Replace(l_chr, "")
                Dim strarray() As String = Split(strline, ",")
                If strarray.Length > 0 Then
                    If flag = 0 Then
                        For i As Integer = 0 To strarray.Length - 1
                            dtCSV.Columns.Add("col" & i, GetType(String))
                        Next

                        flag = 1
                    End If

                    dr1 = dtCSV.NewRow

                    For i As Integer = 0 To strarray.Length - 1
                        dr1(i) = strarray(i)
                    Next
                    If dr1(0).ToString().Trim() = "" And dr1(2).ToString().Trim() = "" Then
                    Else
                        dtCSV.Rows.Add(dr1)
                    End If
                End If
                strline = l_StreamReader_FileRead.ReadLine
            End While
            l_StreamReader_FileRead.Close()

            If dtCSV.Columns.Count >= 3 Then
                dtCSV.Columns(0).ColumnName = "YMCANO"
                dtCSV.Columns(1).ColumnName = "REFNO"
                dtCSV.Columns(1).DataType = System.Type.GetType("System.String")
                dtCSV.Columns(2).ColumnName = "AMOUNT"
                'dtCSV.Columns(2).DataType = System.Type.GetType("System.Decimal")
            Else
                Throw New Exception("Invalid batch file.")
            End If

            getDataTableFromXLS = dtCSV
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub SetSelected()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        'Dim dgi As DataGridItem
        Dim l_TotalAmount As Decimal
        l_TotalAmount = 0.0
        Dim ds As DataSet
        Try
            'Dim i As Integer = dgi.ItemIndex
            'If DatagridACHDebImport.Items.Count > 0 Then
            '    TextBoxTotal.Text = 0
            '    For Each dgi In DatagridACHDebImport.Items
            '        CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
            '        CheckBoxSelect.Checked = True
            '        l_TotalAmount = CType(Me.TextBoxTotal.Text, Double) + CType(dgi.Cells(7).Text.Trim(), Double)
            '        TextBoxTotal.Text = l_TotalAmount
            '    Next
            '    If Not Session("l_dataset_ACHInsert") Is Nothing Then
            '        ds = CType(Session("l_dataset_ACHInsert"), DataSet)
            '        If ds.Tables(0).Rows.Count > 0 Then
            '            For Each drow As DataRow In ds.Tables(0).Rows
            '                drow("Selected") = 1
            '            Next
            '        End If
            '    End If
            'End If
            'Session("l_dataset_ACHInsert") = ds
            '***********
            If Not Session("l_dataset_ACHInsert") Is Nothing Then

                ds = CType(Session("l_dataset_ACHInsert"), DataSet)
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim intCounter As Int16
                    For intCounter = 0 To ds.Tables(0).Rows.Count - 1 Step 1
                        CheckBoxSelect = CType(DatagridACHDebImport.Items(intCounter).Cells(0).FindControl("CheckBoxSelect"), CheckBox)
                        If Not CheckBoxSelect Is Nothing Then
                            If ds.Tables(0).Rows(intCounter)("Selected").ToString() = "1" Then
                                CheckBoxSelect.Checked = True
                                l_TotalAmount = l_TotalAmount + CType(ds.Tables(0).Rows(intCounter)("AMOUNT"), Decimal)
                            End If

                        End If


                    Next


                End If
                TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub DeSelectAll(ByVal blnSelected As Boolean)
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem
        Dim l_TotalAmount As Decimal
        l_TotalAmount = 0.0
        Dim ds As New DataSet
        Try
            If DatagridACHDebImport.Items.Count > 0 Then
                For Each dgi In DatagridACHDebImport.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    CheckBoxSelect.Checked = blnSelected
            l_TotalAmount = CType(Me.TextBoxTotal.Text, Decimal) - CType(dgi.Cells(7).Text.Trim(), Decimal)
                Next
                TextBoxTotal.Text = 0
                If Not Session("l_dataset_ACHInsert") Is Nothing Then
                    ds = CType(Session("l_dataset_ACHInsert"), DataSet)
                    If ds.Tables(0).Rows.Count > 0 Then
                        For Each drow As DataRow In ds.Tables(0).Rows
                            drow("Selected") = IIf(blnSelected, 1, 0)
                        Next
                    End If
                End If
            End If
            Session("l_dataset_ACHInsert") = ds
            'Me.ButtonDeSelectAll.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function SaveFileIntoImportFolder() As String
        Dim uploadFile As HttpPostedFile
        Dim strImportFolerPath As String = String.Empty
        Dim strArrayFileName() As String
        Dim strFileName As String = String.Empty
        Try
            uploadFile = FileField.PostedFile
            strImportFolerPath = ConfigurationSettings.AppSettings("ACH") + "\\" + "IMPORT\\"
            strArrayFileName = uploadFile.FileName.Split("\")
            strFileName = strArrayFileName.GetValue(strArrayFileName.Length - 1)
            strImportFolerPath = strImportFolerPath & strFileName
            'Session("strImportFilePath") = strImportFolerPath

            If Not File.Exists(strImportFolerPath) Then
                uploadFile.SaveAs(strImportFolerPath)
            Else
                File.Delete(strImportFolerPath)
                uploadFile.SaveAs(strImportFolerPath)
                'Dim fs As FileStream
                'Dim fileData() As Byte
                'Try
                '    Dim intFileLength As Int64
                '    intFileLength = uploadFile.ContentLength
                '    ReDim fileData(intFileLength)
                '    uploadFile.InputStream.Read(fileData, 0, intFileLength)
                '    fs = File.OpenWrite(strImportFolerPath)
                '    fs.Write(fileData, 0, intFileLength)
                '    fs.Close()
                ' Finally
                ' fs = Nothing
                '   End Try
            End If
        Catch ex As Exception
            Throw ex
        Finally
            uploadFile = Nothing
        End Try
        Return strImportFolerPath
    End Function
    'Private Function CreateAchDebitTableStructure(ByRef dt As DataTable)
    '    Try
    '        'Dim dtACH As New DataTable
    '        Dim dtColumn As DataColumn
    '        dtColumn = New DataColumn("YMCANAME", System.Type.GetType("System.String"))
    '        dt.Columns.Add(dtColumn)
    '        dtColumn = New DataColumn("UNIQUEID", System.Type.GetType("System.String"))
    '        dt.Columns.Add(dtColumn)
    '        dtColumn = New DataColumn("PAYMENT DATE", System.Type.GetType("System.DateTime"))
    '        dtColumn.DefaultValue = System.DateTime.Today.ToShortDateString
    '        dt.Columns.Add(dtColumn)
    '        dtColumn = New DataColumn("REC DATE", System.Type.GetType("System.DateTime"))
    '        dtColumn.DefaultValue = System.DateTime.Now.Date.ToShortDateString()
    '        dt.Columns.Add(dtColumn)
    '        dtColumn = New DataColumn("SOURCE", System.Type.GetType("System.String"))
    '        dtColumn.DefaultValue = "ACH De"
    '        dt.Columns.Add(dtColumn)
    '        dtColumn = New DataColumn("BatchId", System.Type.GetType("System.String"))
    '        dtColumn.DefaultValue = Session("strBAtchId")
    '        dt.Columns.Add(dtColumn)
    '        'datacolumn to maintain selected or deselected state
    '        dtColumn = New DataColumn("Selected", System.Type.GetType("System.Boolean"))
    '        dtColumn.DefaultValue = 0
    '        dt.Columns.Add(dtColumn)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    Private Function ValidateTotalACHAmount(ByVal l_FileTotalAmount As Decimal)
        Dim l_bool_flag As Boolean = False
        Dim achDebitImport As ACHDebitImportBOClass
        Try
            achDebitImport = New ACHDebitImportBOClass
            Dim l_DbTotalAmount As Decimal = 0.0
            If Not Session("strBatchId") = Nothing Then

                l_DbTotalAmount = achDebitImport.GetACHAmount(Session("strBatchId"))
                If l_DbTotalAmount = l_FileTotalAmount Then
                    l_bool_flag = True
                Else
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", "No valid data to import for the batch.", MessageBoxButtons.Stop)
                    HelperFunctions.ShowMessageToUser("No valid data to import for the batch.", EnumMessageTypes.Error)
                    'Session("l_totalamount") = Nothing
                    l_bool_flag = False
                End If
            End If

            Return l_bool_flag

        Catch
            Throw
        Finally
            achDebitImport = Nothing
        End Try
    End Function
    Private Function ValidateYmcaACHAmount(ByVal dtmport As DataTable, ByRef dsTemp As DataSet) As Boolean
        Dim achDebitImportBOClass As ACHDebitImportBOClass
        Dim l_bool_flag As Boolean = False
        Try
            achDebitImportBOClass = New ACHDebitImportBOClass
            Dim strMessage As String
            l_bool_flag = achDebitImportBOClass.GetACHDataAndValidateYmcaAmt(dtmport, Session("strBatchId"), strMessage, dsTemp)
            If l_bool_flag = False Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", strMessage, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(strMessage, EnumMessageTypes.Error)
            End If
            Return l_bool_flag
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Private Function FillACHDebitDataFromDB(ByVal strYmcasNo As String, ByRef dtTemp As DataTable)
    '    Dim achDebitImportBOClass As ACHDebitImportBOClass
    '    Dim l_ds_ACHData As DataSet
    '    Dim dtACHYMCAName As DataTable
    '    Dim dtACHRefNo As DataTable
    '    Dim dr_findrow As DataRow()
    '    Dim dr_ACHRefNo As DataRow()
    '    Try
    '        achDebitImportBOClass = New ACHDebitImportBOClass
    '        l_ds_ACHData = New DataSet

    '        dtACHYMCAName = New DataTable
    '        dtACHRefNo = New DataTable

    '        l_ds_ACHData = achDebitImportBOClass.GetYMCANameandNos(strYmcasNo, Session("strBatchId"))

    '        dtACHYMCAName = l_ds_ACHData.Tables(0)
    '        dtACHRefNo = l_ds_ACHData.Tables(1)

    '        For Each drow As DataRow In dtTemp.Rows
    '            If drow.RowState <> DataRowState.Deleted Then

    '                If drow(0).GetType.ToString() <> "System.DBNull" Then
    '                    drow(0) = drow(0).ToString().PadLeft(6, "0")
    '                End If

    '                dr_findrow = dtACHYMCAName.Select("YMCANO = '" & drow(0) & "'")
    '                If dr_findrow.Length > 0 Then
    '                    drow("YMCAName") = dr_findrow(0)("YMCANAME")
    '                    drow("UNIQUEID") = dr_findrow(0)("UNIQUEID")
    '                End If
    '                dr_ACHRefNo = dtACHRefNo.Select(" YMCAID = '" & drow("UNIQUEID").ToString() & "'")
    '                If dr_ACHRefNo.Length > 0 Then
    '                    drow("REFNO") = dr_ACHRefNo(0)("REFNO")
    '                    drow("PAYMENT DATE") = dr_ACHRefNo(0)("PAYMENT DATE")
    '                End If

    '            End If
    '        Next




    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Private Function ValidateCheckBoxSelection(ByRef intTotalRecord As Int32, ByRef intRecorsSelected As Int32, ByRef totalSelectedAmout As Decimal) As Boolean
        Try
            Dim boolSelected As Boolean = False
            Dim ctr As Integer
            Dim totalRec As Integer
            ctr = 0
            Session("ProcessOk") = Nothing
            totalRec = DatagridACHDebImport.Items.Count
            For Each item As DataGridItem In DatagridACHDebImport.Items
                Dim chkBox As CheckBox
                chkBox = CType(item.FindControl("CheckBoxSelect"), CheckBox)
                If chkBox.Checked = True And item.Cells(7).Text.Trim().ToUpper() <> "&NBSP;" Then
                    boolSelected = True
                    ctr += 1
                    totalSelectedAmout += CType(item.Cells(7).Text.Trim(), Decimal)
                End If
            Next


            intTotalRecord = totalRec
            intRecorsSelected = ctr
            Return boolSelected
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub AchDebitPostReceipts()
        Dim achDebitImportBOClass As ACHDebitImportBOClass
        Try
            achDebitImportBOClass = New ACHDebitImportBOClass

            If Not Session("l_dataset_ACHInsert") Is Nothing Then
                achDebitImportBOClass.SaveImportedRecords(Session("l_dataset_ACHInsert"))
                'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
                'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Processed Successfully", MessageBoxButtons.OK)
                lblMessage.Text = "Processed Successfully"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','OK');", True)
                Session("strBatchId") = Nothing
                Session("l_dataset_ACHInsert") = Nothing
                Session("IsDone") = True
                Session("BackMatchReceipts") = Nothing
                Session("MatchReceipts") = Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
        '********************************************
    End Sub

#End Region
#Region "Events"


    Private Sub ButtonImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImport.Click
        Dim achDebitImportBO As ACHDebitImportBOClass = Nothing
        Try

            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If Not FileField.PostedFile.FileName.Equals(String.Empty) Then
                achDebitImportBO = New ACHDebitImportBOClass
                Dim strMessage As String = String.Empty
                Dim strBatchID As String = String.Empty
                If achDebitImportBO.ValidateFileNameAndExt(FileField.PostedFile.FileName, strMessage) Then
                    strBatchID = strMessage
                    If achDebitImportBO.ValidateBatchID(strBatchID) > 0 Then
                        Session("strBatchId") = strMessage
                        FillDatagridACHImportProcess()
                    Else
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Invalid Batch Number.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Invalid Batch Number.", EnumMessageTypes.Error)
                    End If


                Else
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", strMessage, MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser(strMessage, EnumMessageTypes.Error)
                End If

            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Please Select the File to Import.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Please Select the File to Import.", EnumMessageTypes.Error)

            End If



        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            achDebitImportBO = Nothing


        End Try
    End Sub



    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            Session("strBatchId") = Nothing
            Session("BackMatchReceipts") = Nothing
            Session("MatchReceipts") = Nothing
            Session("l_dataset_ACHInsert") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception

            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonPostReceipts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPostReceipts.Click
        Try
            Dim intTotalRecord As Int32
            Dim intRecordsSelected As Int32
            Dim l_totalSelectedAmount As Decimal = 0
            If ValidateCheckBoxSelection(intTotalRecord, intRecordsSelected, l_totalSelectedAmount) Then
                If TextBoxTotal.Text <> String.Empty Then

                    If CType(TextBoxTotal.Text, Decimal) = Math.Round(l_totalSelectedAmount, 2) Then

                        'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
                        'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "You have selected " & intRecordsSelected & "/" & intTotalRecord & " records.Do you wish to proceed?", MessageBoxButtons.YesNo)
                        lblMessage.Text = "You have selected " & intRecordsSelected & "/" & intTotalRecord & " records.Do you wish to proceed?"
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YESNO');", True)

                        Session("ProcessOk") = True
                    Else
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Please recalculate amount.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Please recalculate amount.", EnumMessageTypes.Error)
                        Session("ProcessOk") = False
                    End If
                End If

            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Select at least one record.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Select atleast one record.", EnumMessageTypes.Error)
                Session("ProcessOk") = False
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButonMatchRecceipts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButonMatchRecceipts.Click
        Try

            Dim intTotalRecord As Int32
            Dim intRecordsSelected As Int32
            Dim l_totalSelectedAmount As Decimal = 0
            If Not Session("MatchReceipts") = Nothing Then

                Session("MatchReceipts") = Nothing
            End If
            If ValidateCheckBoxSelection(intTotalRecord, intRecordsSelected, l_totalSelectedAmount) Then
                
                If TextBoxTotal.Text <> String.Empty Then

                    If CType(TextBoxTotal.Text, Decimal) = Math.Round(l_totalSelectedAmount, 2) Then
                        If ValidateAnyMatchedTransmittals() Then
                            'AA:16.12.2013 - BT:2311 changed from messge box to jquery dialog box
                            'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "You have selected " & intRecordsSelected & "/" & intTotalRecord & " records.Do you wish to proceed post and apply receipts?", MessageBoxButtons.YesNo)
                            lblMessage.Text = "You have selected " & intRecordsSelected & "/" & intTotalRecord & " records.Do you wish to proceed post and apply receipts?"
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YESNO');", True)
                            Session("MatchReceipts") = True

                        Else
                            'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                            'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "No matches found.", MessageBoxButtons.Stop)
                            HelperFunctions.ShowMessageToUser("No matches found.", EnumMessageTypes.Error)
                            Session("MatchReceipts") = False
                        End If
                    Else
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Please recalculate amount.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Please recalculate amount.", EnumMessageTypes.Error)
                        Session("ProcessOk") = False

                    End If

                    End If

            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Select at least one record.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Select at least one record.", EnumMessageTypes.Error)
            Session("MatchReceipts") = False
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub ButtonDeSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDeSelectAll.Click
        Try
            If ButtonDeSelectAll.Text = "Select None" Then
                DeSelectAll(0)
                ButtonDeSelectAll.Text = "Select All"
            ElseIf ButtonDeSelectAll.Text = "Select All" Then
                DeSelectAll(1)
                ButtonDeSelectAll.Text = "Select None"
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Sub ButtonRecalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRecalculate.Click
        Dim l_DataSetACHImport As DataSet
        Dim l_String_Search As String
        Dim l_Arr_DataRow As DataRow()
        Dim l_TotalAmount As Decimal = 0
        Try
            If Not Session("l_dataset_ACHInsert") Is Nothing Then


                l_DataSetACHImport = CType(Session("l_dataset_ACHInsert"), DataSet)
                For Each dgItem As DataGridItem In DatagridACHDebImport.Items
                    Dim chkSelect As CheckBox
                    chkSelect = CType(dgItem.FindControl("CheckBoxSelect"), CheckBox)
                    l_String_Search = dgItem.Cells(8).Text.Trim()
                    l_Arr_DataRow = l_DataSetACHImport.Tables(0).Select("uniqueid = '" & l_String_Search & "' " & " and PaymentDate='" & dgItem.Cells(5).Text.Trim() & "'")
                    If Not chkSelect Is Nothing And l_Arr_DataRow.Length > 0 Then
                        If chkSelect.Checked Then
                            l_Arr_DataRow(0)("Selected") = 1
                            l_TotalAmount = l_TotalAmount + CType(dgItem.Cells(7).Text.Trim(), Decimal)
                        Else
                            l_Arr_DataRow(0)("Selected") = 0
                        End If
                    End If

                Next
                TextBoxTotal.Text = Math.Round(l_TotalAmount, 2).ToString()
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Commented by Ashish 2-Sep-2008 for adding recalculate button logic ,start
    'Public Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)

    '    Try
    '        Dim chkFlag As CheckBox = CType(sender, CheckBox)
    '        Dim dbl_Total As Double
    '        Dim dt As DataTable
    '        Dim ds As New DataSet

    '        ds = CType(Session("l_dataset_ACHInsert"), DataSet)
    '        Dim dgItem As DataGridItem = CType(chkFlag.NamingContainer, DataGridItem)
    '        Dim i As Integer = dgItem.ItemIndex
    '        If chkFlag.Checked = True And dgItem.Cells(7).Text().ToUpper <> "&NBSP;" Then
    '            ds.Tables(0).Rows(i).Item("Selected") = 1
    '            dbl_Total = CType(Me.TextBoxTotal.Text, Double) + CType(dgItem.Cells(7).Text.Trim(), Double)
    '            Me.TextBoxTotal.Text = dbl_Total

    '        Else
    '            ds.Tables(0).Rows(i).Item("Selected") = 0
    '            If dgItem.Cells(7).Text().ToUpper <> "&NBSP;" Then
    '                dbl_Total = CType(Me.TextBoxTotal.Text, Double) - CType(dgItem.Cells(7).Text.Trim(), Double)
    '                Me.TextBoxTotal.Text = dbl_Total
    '            End If
    '        End If

    '        Session("l_dataset_ACHInsert") = ds
    '        If TextBoxTotal.Text.Equals("0") Then
    '            Me.ButtonDeSelectAll.Enabled = False
    '        Else
    '            Me.ButtonDeSelectAll.Enabled = True
    '        End If

    '    Catch ex As SqlException

    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    Catch ex As Exception

    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub

    'Commented by Ashish 2-Sep-2008 for adding recalculate button logic ,End
    Private Function ValidateAnyMatchedTransmittals() As Boolean
        Dim achDebitImportBOClass As ACHDebitImportBOClass
        Dim dsAchDebitImport As DataSet
        Dim strBatchId As String
        Dim strYmcaNos As New StringBuilder
        Dim l_dsAchDebitMatchTransmittal As DataSet
        Dim l_boolFoundMatched = False
        Try
            'Session("l_dataset_ACHInsert")


            If Not Session("l_dataset_ACHInsert") Is Nothing Then

                dsAchDebitImport = New DataSet
                dsAchDebitImport = CType(Session("l_dataset_ACHInsert"), DataSet)
                If dsAchDebitImport.Tables(0).Rows.Count > 0 Then
                    strBatchId = dsAchDebitImport.Tables(0).Rows(0)("REFNO")

                    'commented by Ashish on 13-Jan-2009, Start
                    'For Each dtRow As DataRow In dsAchDebitImport.Tables(0).Rows
                    '    If dtRow("Selected").ToString().Trim() = "1" Then
                    '        strYmcaNos.Append(dtRow("YMCANO").ToString().Trim().PadLeft(6, "0") & ",")
                    '    End If
                    'Next

                    'If strYmcaNos.Length > 0 And strBatchId <> String.Empty Then
                    '    l_dsAchDebitMatchTransmittal = New DataSet
                    '    achDebitImportBOClass = New ACHDebitImportBOClass
                    '    l_dsAchDebitMatchTransmittal = achDebitImportBOClass.GetAchDebitMatchedTransmittals(strBatchId)

                    'End If
                    'commented by Ashish on 13-Jan-2009, End
                    If strBatchId <> String.Empty Then
                        l_dsAchDebitMatchTransmittal = New DataSet
                        achDebitImportBOClass = New ACHDebitImportBOClass
                        l_dsAchDebitMatchTransmittal = achDebitImportBOClass.GetAchDebitMatchedTransmittals(strBatchId)

                    End If
                    If Not l_dsAchDebitMatchTransmittal Is Nothing Then

                        If l_dsAchDebitMatchTransmittal.Tables(0).Rows.Count > 0 Then
                            For Each dtRowAchImportData As DataRow In dsAchDebitImport.Tables(0).Rows
                                If dtRowAchImportData("Selected") = 1 Then
                                    Dim dtRowFindMatchedTransmittal As DataRow()

                                    dtRowFindMatchedTransmittal = l_dsAchDebitMatchTransmittal.Tables(0).Select("YMCANo=" & dtRowAchImportData("YMCANo") & " and AchPaymentDate='" & Convert.ToDateTime(dtRowAchImportData("PaymentDate")).Date.ToString("MM/dd/yyyy") & "'")
                                    If dtRowFindMatchedTransmittal.Length > 0 Then
                                        Dim i As Int16
                                        For i = 0 To dtRowFindMatchedTransmittal.Length - 1 Step 1
                                            If dtRowFindMatchedTransmittal(i)("VisibleStatus") = 1 Then
                                                l_boolFoundMatched = True
                                                Exit For
                                            End If
                                        Next
                                        'dtRowFindMatchedTransmittal()
                                    End If
                                End If
                                If l_boolFoundMatched = True Then
                                    Exit For
                                End If

                            Next
                            'commented by Ashish on 13-Jan-2009
                            'For Each dtMatchedRow As DataRow In l_dsAchDebitMatchTransmittal.Tables(0).Rows
                            '    If dtMatchedRow("VisibleStatus") = 1 Then
                            '        l_boolFoundMatched = True
                            '        Exit For
                            '    End If
                            'Next
                        End If


                    End If


                End If

            End If
            dsAchDebitImport = Nothing
            l_dsAchDebitMatchTransmittal = Nothing
            achDebitImportBOClass = Nothing
            Return l_boolFoundMatched
        Catch ex As Exception
            Throw


        End Try
    End Function
#Region "DataGrid Events"
    'Private Sub DatagridACHDebImport_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DatagridACHDebImport.ItemCommand
    '    If e.Item.DataItem = ListItemType.Item And e.Item.DataItem = ListItemType.AlternatingItem Then
    '        If e.Item.Cells(8).Text.Equals("1") Then
    '            Dim chk As CheckBox
    '            chk = CType(e.Item.Cells(0).FindControl("CheckBoxSelect"), CheckBox)
    '            If Not chk Is Nothing Then
    '                If e.Item.Cells(8).Text.Equals("1") Then
    '                    chk.Checked = True
    '                Else
    '                    chk.Checked = False
    '                End If
    '            End If

    '        End If
    '    End If
    'End Sub
#End Region
#End Region
    'AA:16.12.2013 - BT:2311 Added Button yes and no button click event functionality.
    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)

            If Not Session("ProcessOk") = Nothing Then
                If Session("ProcessOk") = True Then
                    Session("ProcessOk") = Nothing
                    AchDebitPostReceipts()
                End If
            End If

            If Not Session("MatchReceipts") = Nothing Then
                If Session("MatchReceipts") = True Then
                    Response.Redirect("ACHDebitReceiptApplicationForm.aspx", False)
                    Session("MatchReceipts") = Nothing
                End If
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
   
   
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            If (Session("IsDone") = True) Then
                Session("IsDone") = False
                Response.Redirect("MainWebForm.aspx", False)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
