'/**************************************************************************************************************/
'// Copyright YMCA Retirement Fund All Rights Reserved. 
'//
'// Project Name		:	YMCa-YRS
'// FileName			:	DARFileImport.aspx.vb
'// Author: Santosh Bura
'// Created on: 11/08/2019
'// Summary of Functionality: New Screen to Import DAR File for First annuity payments
'// Declared in Version: 20.7.0.1 | YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File) 
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'// 			                | 	           |		         | 
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/

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
Imports System.Data.DataRow
Imports System.IO
Imports System.IO.File
Imports System.Text
Imports YMCAObjects
Imports YMCARET.YmcaBusinessObject
Imports System.Data.OleDb
#End Region

Public Class DARFileImport
    Inherits System.Web.UI.Page

    ''This call is required by the Web Form Designer.
    '<System.Diagnostics.DebuggerStepThrough()>
    'Private Sub InitializeComponent()

    'End Sub

    Protected WithEvents gvDARImportedRecordsList As System.Web.UI.WebControls.GridView
    Protected WithEvents FileField As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents divTotalRecords As System.Web.UI.HtmlControls.HtmlGenericControl

#Region "Properties"
    Public Property ReadOnlyWarningMessage() As String
        Get
            If Not ViewState("ReadOnlyWarningMessage") Is Nothing Then
                Return (CType(ViewState("ReadOnlyWarningMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ReadOnlyWarningMessage") = value
        End Set
    End Property

    Public Property IsReadOnlyAccess() As Boolean
        Get
            If Not ViewState("IsReadOnlyAccess") Is Nothing Then
                Return (CType(ViewState("IsReadOnlyAccess"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsReadOnlyAccess") = value
        End Set
    End Property

    Public Property FolderPath() As String
        Get
            If Not ViewState("FolderPath") Is Nothing Then
                Return CType(ViewState("FolderPath"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("FolderPath") = value
        End Set
    End Property

    Public Property ImportedFileName() As String
        Get
            If Not ViewState("ImportedFileName") Is Nothing Then
                Return CType(ViewState("ImportedFileName"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ImportedFileName") = value
        End Set
    End Property

    Public Property ImportedBaseHeaderID() As Integer
        Get
            If Not ViewState("ImportedBaseHeaderID") Is Nothing Then
                Return CType(ViewState("ImportedBaseHeaderID"), Integer)
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("ImportedBaseHeaderID") = value
        End Set
    End Property

    Public Property ImportedFileRecordsDataSet() As DataTable
        Get
            If Not ViewState("ImportBaseRecords") Is Nothing Then
                Return TryCast(ViewState("ImportBaseRecords"), DataTable)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As DataTable)
            ViewState("ImportBaseRecords") = value
        End Set
    End Property

    Public Property ParticipantErrorCountforError() As Integer
        Get
            If Not ViewState("ParticipantErrorCount") Is Nothing Then
                Return CType(ViewState("ParticipantErrorCount"), Integer)
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("ParticipantErrorCount") = value
        End Set
    End Property
#End Region

#Region " Index variables "     'Index varibles to be used in case of the datagrid columns are required to change 

    Dim IsErrorExistsField As Integer = 11

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport page load", "START: Page Load.")
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)

            If Not IsPostBack Then
                Dim dsPendingImportBaseData As DataSet

                'Clear session variables
                ClearSessionValues()

                'Set Readonly access right and message value
                CheckAccessRights()

                'Check if any DAR file is already imported with status as PENDING and get the results
                dsPendingImportBaseData = YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.GetPendingImportFilePresent()

                'If any pending records are present then load the contents of previously loaded file else allow to import new file
                If HelperFunctions.isNonEmpty(dsPendingImportBaseData.Tables("ImportBaseRecords")) Then
                    If dsPendingImportBaseData.Tables("ImportBaseRecords").Rows.Count > 0 Then
                        Me.ImportedFileRecordsDataSet = dsPendingImportBaseData.Tables("ImportBaseRecords")
                        Me.ImportedBaseHeaderID = dsPendingImportBaseData.Tables("ImportBaseRecords").Rows(0)("ImportBaseHeaderId").ToString()
                        Me.gvDARImportedRecordsList.DataSource = dsPendingImportBaseData.Tables("ImportBaseRecords").DefaultView
                        Me.gvDARImportedRecordsList.DataBind()

                        'lblRecords.InnerText = gvDARImportedRecordsList.Rows.Count
                        SetPendingFileControlsOnPage()
                        SetTotalValuesinSummarySection(dsPendingImportBaseData.Tables("ImportBaseHeaderRecords"))
                        Me.ParticipantErrorCountforError = dsPendingImportBaseData.Tables("ImportBaseRecords").Select(" ERROR_EXIST = 1 ").Count
                        If Me.ParticipantErrorCountforError > 0 Then
                            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_MISSMATCH_RECORDS).DisplayText, EnumMessageTypes.Error)
                            btnRunDARImportFile.Enabled = False
                        End If


                    End If
                Else
                    SetDefaultPageLoadControls()
                End If
            End If
            
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport page load", "END: Page Load.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.LogException("DARFileImport --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


#Region "Validation-CheckRights-Supportive Functions"
    ' This method will check read-only access rights
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("DARFileImport.aspx", Convert.ToInt32(Session("LoggedUserKey")))
            If Not String.IsNullOrEmpty(readOnlyWarningMessage) AndAlso Not readOnlyWarningMessage.ToUpper().Contains("TRUE") Then
                Me.IsReadOnlyAccess = True
                Me.ReadOnlyWarningMessage = readOnlyWarningMessage
            End If
        Catch
            Throw
        Finally
            readOnlyWarningMessage = Nothing
        End Try
    End Sub

    Public Shared Function SplitCsv(ByVal sLine As String, ByVal cDelim As Char) As String()
        Dim sVal As String() = Nothing

        Try

            If sLine IsNot Nothing Then
                sLine = sLine.Trim()
                Dim al As System.Collections.ArrayList = New System.Collections.ArrayList()
                Dim bInQ As Boolean = False
                Dim sCurString As StringBuilder = New StringBuilder()

                For i As Integer = 0 To sLine.Length - 1

                    If sLine(i) = """"c Then

                        If bInQ Then
                            bInQ = False
                        Else
                            bInQ = True
                        End If
                    Else

                        If Not bInQ AndAlso sLine(i) = cDelim Then
                            al.Add(sCurString.ToString())
                            sCurString.Remove(0, sCurString.Length)
                        Else
                            sCurString.Append(sLine(i))
                        End If
                    End If
                Next

                al.Add(sCurString.ToString())
                sVal = New String(al.Count - 1) {}

                For i As Integer = 0 To al.Count - 1
                    sVal(i) = CStr(al(i))
                Next
            End If
            Return sVal
        Catch
            Throw
        End Try
    End Function

    Public Function ValidateFileExtension(ByVal strFileName As String) As Boolean
        Dim bolFlag As Boolean = False
        Dim strFullFileName As String = String.Empty
        Dim strSecondPartFName As String = String.Empty
        Try
            Dim strArrayFilePath As String() = strFileName.Split("\"c)
            strFullFileName = strArrayFilePath(strArrayFilePath.Length - 1)

            If strFullFileName.ToUpper().EndsWith(".CSV") Then
                bolFlag = True
            End If
            Return bolFlag
        Catch
            Throw
        End Try
    End Function

    ' This method will load/set the default controls on page load. Also once the file gets processed or discareded successfully this method is called
    Private Sub SetDefaultPageLoadControls()
        Try
            divDisplayImportedResultsSection.Visible = False
            divDARImportSummary.Visible = False
            btnPrintDARImportFileList.Enabled = False
            btnRunDARImportFile.Enabled = False
            btnDiscardDARImportFile.Enabled = False
            btnImportDARBankResponseFile.Enabled = True
            gvDARImportedRecordsList.Style("Display") = "none"
            divTotalRecords.Visible = False
            btnImportDARBankResponseFile.ToolTip = Nothing

        Catch
            Throw
        End Try
    End Sub

    ' This method will load/set when there is a pending file is present 
    Private Sub SetPendingFileControlsOnPage()
        Try
            divDisplayImportedResultsSection.Visible = True
            divDARImportSummary.Visible = True
            btnPrintDARImportFileList.Enabled = True
            btnRunDARImportFile.Enabled = True
            btnDiscardDARImportFile.Enabled = True
            btnImportDARBankResponseFile.Enabled = False
            gvDARImportedRecordsList.Style("Display") = "block"
            divTotalRecords.Visible = True
            btnImportDARBankResponseFile.ToolTip = "To import new file please Run / Discard existing pending file"
          
        Catch
            Throw
        End Try
    End Sub
#End Region

    Private Sub btnImportDARBankResponseFile_Click(sender As Object, e As EventArgs) Handles btnImportDARBankResponseFile.Click
        'Dim bankAckImportBO As BankAcknowledgementImportBO = Nothing
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "btnImportDARBankResponseFile_Click START")
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                'If no file is selected and import button clicked  then through validation error on screen
                If Not FileField.PostedFile.FileName.Equals(String.Empty) Then
                    ' bankAckImportBO = New BankAcknowledgementImportBO
                    Dim strMessage As String = String.Empty
                    ' If bankAckImportBO.ValidateFileExtension(FileField.PostedFile.FileName, strMessage) Then
                    If ValidateFileExtension(FileField.PostedFile.FileName) Then
                        SaveDARImportFileData()
                    Else
                        HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_INVALID_FILEEXTENSION).DisplayText, EnumMessageTypes.Error)
                    End If
                Else
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_INVALID_FILEEXTENSION).DisplayText, EnumMessageTypes.Error)
                End If
            End If

        Catch ex As SqlException
            HelperFunctions.LogException("btnImportDARBankResponseFile_Click", ex)
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_SQL_EXCEPTION).DisplayText, EnumMessageTypes.Error, Nothing)
        Catch ex As Exception
            HelperFunctions.LogException("btnImportDARBankResponseFile_Click", ex)
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DOTNET_EXCEPTION).DisplayText, EnumMessageTypes.Error, Nothing)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "btnImportDARBankResponseFile_Click END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Private Function getDataTableFromCSVFile(ByVal strFilePath As String) As DataTable

        Dim l_StreamReader_FileRead As StreamReader = File.OpenText(strFilePath)
        Dim strline As String = l_StreamReader_FileRead.ReadLine
        Dim dtCSV As New DataTable
        Dim dr As DataRow
        Dim l_chr As Char
        Dim flag As Integer = 0
        Dim isColumnExists As Integer = 0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "getDataTableFromCSVFile START")

            l_chr = Convert.ToChar(34)

            While strline <> Nothing
                Dim strarray() As String = SplitCsv(strline, ",")
                If strarray.Length > 0 Then
                    If flag = 0 Then
                        For i As Integer = 0 To strarray.Length - 1
                            dtCSV.Columns.Add(strarray(i), GetType(String))
                        Next

                        flag = 1
                    End If

                    dr = dtCSV.NewRow

                    For i As Integer = 0 To strarray.Length - 1
                        dr(i) = strarray(i)
                    Next

                    'Check the file contents in this place 
                    If dr(0).ToString().Trim() = "" And dr(2).ToString().Trim() = "" Then
                    Else
                        dtCSV.Rows.Add(dr)
                    End If
                End If
                strline = l_StreamReader_FileRead.ReadLine
            End While

            l_StreamReader_FileRead.Close()
            For Each c As DataColumn In dtCSV.Columns
                c.ColumnName = String.Join("", c.ColumnName.Split())
            Next

            'Check the column count 
            If dtCSV.Columns.Count >= 3 Then

                dtCSV = ValidateColumnExists(dtCSV)

                If (Not dtCSV Is Nothing) Then
                    dtCSV.Rows.RemoveAt(0)  ' Remove Header ROW

                    dtCSV = AddRequiredColumn(dtCSV)

                    dtCSV = SetDataInDataTable(dtCSV)
                End If
            Else
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DARFILE_INVALID_COLUMNHEADERS).DisplayText, EnumMessageTypes.Error)
                Exit Function
            End If
            getDataTableFromCSVFile = dtCSV
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "getDataTableFromCSVFile END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Function ValidateColumnExists(ByVal dtCSV As DataTable) As DataTable
        Dim isColumnExists As Integer = 0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "ValidateColumnExists START")
            If (dtCSV.Columns.Contains("SSN")) Then
                dtCSV.Columns("SSN").ColumnName = "SSNo"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("PARTID")) Then
                dtCSV.Columns("PARTID").ColumnName = "FundNo"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("FIRSTNAME")) Then
                dtCSV.Columns("FIRSTNAME").ColumnName = "FirstName"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("LASTNAME")) Then
                dtCSV.Columns("LASTNAME").ColumnName = "LastName"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("DISTRMETHOD")) Then
                dtCSV.Columns("DISTRMETHOD").ColumnName = "PaymentMethodCode"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("PAYABLEDATE")) Then
                dtCSV.Columns("PAYABLEDATE").ColumnName = "PayableDate"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("PAYMTREF#")) Then
                dtCSV.Columns("PAYMTREF#").ColumnName = "CheckNumber"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("CHECKSTATUS")) Then
                dtCSV.Columns("CHECKSTATUS").ColumnName = "CheckStatus"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("GROSSAMT")) Then
                dtCSV.Columns("GROSSAMT").ColumnName = "GrossAmount"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("NETPAYAMT")) Then
                dtCSV.Columns("NETPAYAMT").ColumnName = "NetAmount"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("TAXABLEAMOUNT")) Then
                dtCSV.Columns("TAXABLEAMOUNT").ColumnName = "TaxableAmount"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("NONTAXABLEAMOUNT")) Then
                dtCSV.Columns("NONTAXABLEAMOUNT").ColumnName = "NonTaxableAmount"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("FEDAMT")) Then
                dtCSV.Columns("FEDAMT").ColumnName = "FedTaxWithheld"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("STATE/PROVTAXAUTHORITY")) Then
                dtCSV.Columns("STATE/PROVTAXAUTHORITY").ColumnName = "StateTaxCode"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("STATE/PROVDEDAMT")) Then
                dtCSV.Columns("STATE/PROVDEDAMT").ColumnName = "StateTaxWithheld"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("LOCALTAXAUTHORITY")) Then 'NYC or Yonkers tax Withheld
                dtCSV.Columns("LOCALTAXAUTHORITY").ColumnName = "LocalTaxCode"
            Else
                isColumnExists = 1
            End If

            If (dtCSV.Columns.Contains("LOCALTAXAMT")) Then 'NYC or Yonkers tax Withheld
                dtCSV.Columns("LOCALTAXAMT").ColumnName = "LocalTaxWithheld"
            Else
                isColumnExists = 1
            End If

            If (isColumnExists = 1) Then
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DARFILE_INVALID_COLUMNHEADERS).DisplayText, EnumMessageTypes.Error)
                Exit Function
            End If

            Return dtCSV
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "ValidateColumnExists END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    Private Function AddRequiredColumn(ByVal dtCSV As DataTable) As DataTable

        If (dtCSV.Columns.Contains("001-MEDICAL")) Then
            dtCSV.Columns("001-MEDICAL").ColumnName = "MEDINSDeductions" 'Other deductions MEDINS
        Else
            dtCSV.Columns.Add("MEDINSDeductions", System.Type.GetType("System.String"))
        End If

        If (dtCSV.Columns.Contains("002-BANKFEE")) Then
            dtCSV.Columns("002-BANKFEE").ColumnName = "PRCSTSFeeDeductions" 'Other deductions PRCSTS
        Else
            dtCSV.Columns.Add("PRCSTSFeeDeductions", System.Type.GetType("System.String"))
        End If

        If (dtCSV.Columns.Contains("003-OTHER")) Then
            dtCSV.Columns("003-OTHER").ColumnName = "NAFYRDeductions" 'Other deductions NAFYR
        Else
            dtCSV.Columns.Add("NAFYRDeductions", System.Type.GetType("System.String"))
        End If

        If (dtCSV.Columns.Contains("004-LV-YERRI")) Then
            dtCSV.Columns("004-LV-YERRI").ColumnName = "IRSLVYDeductions" 'Other deductions IRSLVY
        Else
            dtCSV.Columns.Add("IRSLVYDeductions", System.Type.GetType("System.String"))
        End If

        If (dtCSV.Columns.Contains("005-LV-LOVEL")) Then
            dtCSV.Columns("005-LV-LOVEL").ColumnName = "LVLOVELDeductions" 'Other deductions LOVEL
        Else
            dtCSV.Columns.Add("LVLOVELDeductions", System.Type.GetType("System.String"))
        End If

        dtCSV.Columns.Add("DeductionsCode1", System.Type.GetType("System.String"))
        dtCSV.Columns.Add("DeductionsCode2", System.Type.GetType("System.String"))
        dtCSV.Columns.Add("DeductionsCode3", System.Type.GetType("System.String"))
        dtCSV.Columns.Add("DeductionsCode4", System.Type.GetType("System.String"))
        dtCSV.Columns.Add("DeductionsCode5", System.Type.GetType("System.String"))
        dtCSV.Columns.Add("OtherWithholdings", System.Type.GetType("System.String")) ' This is the sum of  MEDINSDeductions + PRCSTSFeeDeductions + NAFYRDeductions +  IRSLVYDeductions
        Return dtCSV
    End Function

    Private Function SetDataInDataTable(ByVal dtCSV As DataTable) As DataTable
        Dim deductioncode1 As String
        Dim deductioncode2 As String
        Dim deductioncode3 As String
        Dim deductioncode4 As String
        Dim deductioncode5 As String
        deductioncode1 = "001-MEDICAL"
        deductioncode2 = "002-BANKFEE"
        deductioncode3 = "003-OTHER"
        deductioncode4 = "004-LV-YERRI"
        deductioncode5 = "005-LV-LOVEL"

        For i = 0 To dtCSV.Rows.Count - 1

            If (String.IsNullOrEmpty(dtCSV.Rows(i)("MEDINSDeductions").ToString)) Then
                dtCSV.Rows(i)("DeductionsCode1") = Nothing
                dtCSV.Rows(i)("MEDINSDeductions") = 0
            Else
                dtCSV.Rows(i)("DeductionsCode1") = deductioncode1.Substring(4)

            End If

            If (String.IsNullOrEmpty(dtCSV.Rows(i)("PRCSTSFeeDeductions").ToString)) Then
                dtCSV.Rows(i)("DeductionsCode2") = Nothing
                dtCSV.Rows(i)("PRCSTSFeeDeductions") = 0
            Else
                dtCSV.Rows(i)("DeductionsCode2") = deductioncode2.Substring(4)
            End If

            If (String.IsNullOrEmpty(dtCSV.Rows(i)("NAFYRDeductions").ToString)) Then
                dtCSV.Rows(i)("DeductionsCode3") = Nothing
                dtCSV.Rows(i)("NAFYRDeductions") = 0
            Else
                dtCSV.Rows(i)("DeductionsCode3") = deductioncode3.Substring(4)
            End If

            If (String.IsNullOrEmpty(dtCSV.Rows(i)("IRSLVYDeductions").ToString)) Then
                dtCSV.Rows(i)("DeductionsCode4") = Nothing
                dtCSV.Rows(i)("IRSLVYDeductions") = 0
            Else
                dtCSV.Rows(i)("DeductionsCode4") = deductioncode4.Substring(4)
            End If

            If (String.IsNullOrEmpty(dtCSV.Rows(i)("LVLOVELDeductions").ToString)) Then
                dtCSV.Rows(i)("DeductionsCode5") = Nothing
                dtCSV.Rows(i)("LVLOVELDeductions") = 0
            Else
                dtCSV.Rows(i)("DeductionsCode5") = deductioncode5.Substring(4)
            End If

            dtCSV.Rows(i)("OtherWithholdings") = Convert.ToDecimal(IIf(String.IsNullOrEmpty(dtCSV.Rows(i)("PRCSTSFeeDeductions").ToString), "0", dtCSV.Rows(i)("PRCSTSFeeDeductions").ToString)) + _
             Convert.ToDecimal(IIf(String.IsNullOrEmpty(dtCSV.Rows(i)("NAFYRDeductions").ToString), "0", dtCSV.Rows(i)("NAFYRDeductions").ToString)) + _
             Convert.ToDecimal(IIf(String.IsNullOrEmpty(dtCSV.Rows(i)("IRSLVYDeductions").ToString), "0", dtCSV.Rows(i)("IRSLVYDeductions").ToString)) + _
             Convert.ToDecimal(IIf(String.IsNullOrEmpty(dtCSV.Rows(i)("MEDINSDeductions").ToString), "0", dtCSV.Rows(i)("MEDINSDeductions").ToString)) + _
             Convert.ToDecimal(IIf(String.IsNullOrEmpty(dtCSV.Rows(i)("LVLOVELDeductions").ToString), "0", dtCSV.Rows(i)("LVLOVELDeductions").ToString))

        Next
        Return dtCSV
    End Function


    Private Sub SaveDARImportFileData()
        Dim dtFileData As DataTable
        Dim strDARFileImportFilePath As String
        Dim AtsSTWImportBaseHeaderDetails As YMCAObjects.BankAcknowledgementHeaderImport
        Dim dsFileImportBaseData As DataSet = New DataSet()
        Dim dsFileImportBaseDataWithErrorList As DataSet
        'Dim dtImportFileData As DataTable
        Dim HeaderID As String
        Dim ParticipantErrorCount As Integer = 0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "SaveDARImportFileData START")
            'Save file into Imported Folder
            strDARFileImportFilePath = SaveFileIntoImportFolder()

            If strDARFileImportFilePath <> String.Empty Then

                dtFileData = getDataTableFromCSVFile(strDARFileImportFilePath)

                If (dtFileData Is Nothing) Then
                    Exit Sub
                End If

                If (HelperFunctions.isEmpty(dtFileData)) Then
                    HelperFunctions.ShowMessageToUser("DAR File can not be blank", EnumMessageTypes.Error)
                    Exit Sub
                End If

                If (HelperFunctions.isNonEmpty(dtFileData)) Then

                    dsFileImportBaseData.Tables.Add(dtFileData)

                    Dim d_totalGrossAmount As Decimal = 0.0
                    Dim d_totalStateTaxWithheldAmount As Decimal = 0.0
                    Dim d_totalFedTaxWithheldAmount As Decimal = 0.0
                    Dim d_totalLocalTaxWithheldAmount As Decimal = 0.0  'NYC or Yonkers tax Withheld
                    Dim d_totalOtherWithheldAmount As Decimal = 0.0
                    Dim d_totalNetAmount As Decimal = 0.0

                    For Each dtRow As DataRow In dtFileData.Rows
                        Dim grossAmount As Decimal = 0.0
                        Dim netAmount As Decimal = 0.0
                        Dim stateTaxAmount As Decimal = 0.0
                        Dim fedTaxAmount As Decimal = 0.0
                        Dim localTaxAmount As Decimal = 0.0
                        Dim otherWithheld As Decimal = 0.0

                        Dim valueGrossAmount As String = dtRow("GrossAmount")
                        Dim valueNetAmount As String = dtRow("NetAmount")
                        Dim valueStateTaxAmount As String = dtRow("StateTaxWithheld")
                        Dim valueFedTaxAmount As String = dtRow("FedTaxWithheld")
                        Dim valueLocalTaxAmount As String = dtRow("LocalTaxWithheld")
                        Dim valueOtherWithheld As String = dtRow("OtherWithholdings")

                        If (Not dtRow("CheckStatus") = "RETURNED") Then

                            If Not String.IsNullOrEmpty(valueGrossAmount) Then
                                grossAmount = Convert.ToDecimal(valueGrossAmount)
                                d_totalGrossAmount = d_totalGrossAmount + grossAmount
                            End If

                            If Not String.IsNullOrEmpty(valueNetAmount) Then
                                netAmount = Convert.ToDecimal(valueNetAmount)
                                d_totalNetAmount = d_totalNetAmount + netAmount
                            End If

                            If Not String.IsNullOrEmpty(valueStateTaxAmount) Then
                                stateTaxAmount = Convert.ToDecimal(valueStateTaxAmount)
                                d_totalStateTaxWithheldAmount = d_totalStateTaxWithheldAmount + stateTaxAmount
                            End If

                            If Not String.IsNullOrEmpty(valueFedTaxAmount) Then
                                fedTaxAmount = Convert.ToDecimal(valueFedTaxAmount)
                                d_totalFedTaxWithheldAmount = d_totalFedTaxWithheldAmount + fedTaxAmount
                            End If

                            If Not String.IsNullOrEmpty(valueLocalTaxAmount) Then
                                localTaxAmount = Convert.ToDecimal(valueLocalTaxAmount)
                                d_totalLocalTaxWithheldAmount = d_totalLocalTaxWithheldAmount + localTaxAmount
                            End If

                            If Not String.IsNullOrEmpty(valueOtherWithheld) Then
                                otherWithheld = Convert.ToDecimal(valueOtherWithheld)
                                d_totalOtherWithheldAmount = d_totalOtherWithheldAmount + otherWithheld
                            End If
                        End If
                    Next


                    AtsSTWImportBaseHeaderDetails = New YMCAObjects.BankAcknowledgementHeaderImport()
                    AtsSTWImportBaseHeaderDetails.DARFilePath = Me.FolderPath
                    AtsSTWImportBaseHeaderDetails.DisbursementType = "ANN"
                    AtsSTWImportBaseHeaderDetails.Source = "FSTANN"
                    AtsSTWImportBaseHeaderDetails.Status = "PENDING"
                    AtsSTWImportBaseHeaderDetails.TotalRecords = dtFileData.Rows.Count
                    AtsSTWImportBaseHeaderDetails.TotalGrossAmount = d_totalGrossAmount
                    AtsSTWImportBaseHeaderDetails.TotalStateTaxAmount = d_totalStateTaxWithheldAmount
                    AtsSTWImportBaseHeaderDetails.TotalLocalFlatAmount = d_totalLocalTaxWithheldAmount
                    AtsSTWImportBaseHeaderDetails.TotalFedTaxAmount = d_totalFedTaxWithheldAmount
                    AtsSTWImportBaseHeaderDetails.TotalDedcutionAmount = d_totalOtherWithheldAmount
                    AtsSTWImportBaseHeaderDetails.TotalNetAmount = d_totalNetAmount

                    'AtsSTWImportBaseHeaderDetails.TotalGrossAmount = dtFileData.AsEnumerable().Sum(Function(x) Convert.ToDouble(x("GrossAmount"))) 'dtRow("GrossAmount")
                    'AtsSTWImportBaseHeaderDetails.TotalNetAmount = dtFileData.AsEnumerable().Sum(Function(x) Convert.ToDouble(x("NetAmount"))) 'dtRow("NetAmount")
                    'AtsSTWImportBaseHeaderDetails.TotalStateTaxAmount = dtFileData.AsEnumerable().Sum(Function(x) Convert.ToDouble(x("StateTaxWithheld"))) 'dtRow("StateTaxWithheld")
                    'AtsSTWImportBaseHeaderDetails.TotalFedTaxAmount = dtFileData.AsEnumerable().Sum(Function(x) Convert.ToDouble(x("FedTaxWithheld"))) 'dtRow("FedTaxWithheld")
                    'AtsSTWImportBaseHeaderDetails.TotalLocalFlatAmount = dtFileData.AsEnumerable().Sum(Function(x) Convert.ToDouble(x("LocalTaxWithheld"))) 'dtRow("LocalTaxWithheld")
                    'AtsSTWImportBaseHeaderDetails.TotalDedcutionAmount = dtFileData.AsEnumerable().Sum(Function(x) Convert.ToDouble(x("OtherWithheld"))) 'dtRow("OtherWithheld")

                    'Save the Import Base header data
                    HeaderID = YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.PrepareImportBaseHeaderRecord(AtsSTWImportBaseHeaderDetails)


                    If Not HeaderID Is Nothing Then
                        Me.ImportedBaseHeaderID = HeaderID
                        'Save the import base records
                        dsFileImportBaseDataWithErrorList = YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.InsertImportBaseRecordsWithErrors(dsFileImportBaseData, Convert.ToInt32(HeaderID))
                    End If
                    'Else
                    '    Throw New Exception("Invalid batch file.")
                End If
                Me.gvDARImportedRecordsList.DataSource = Nothing

                If HelperFunctions.isNonEmpty(dsFileImportBaseDataWithErrorList.Tables("ImportBaseRecords")) And dsFileImportBaseDataWithErrorList.Tables("ImportBaseRecords").Rows.Count > 0 Then
                    Me.gvDARImportedRecordsList.DataSource = dsFileImportBaseDataWithErrorList.Tables("ImportBaseRecords").DefaultView
                    SetTotalValuesinSummarySection(dsFileImportBaseDataWithErrorList.Tables("ImportBaseHeaderRecords"))
                    'ViewState("ImportBaseRecords") = dsFileImportBaseDataWithErrorList.Tables("ImportBaseRecords")
                    Me.gvDARImportedRecordsList.DataBind()
                    Me.ParticipantErrorCountforError = dsFileImportBaseDataWithErrorList.Tables("ImportBaseRecords").Select(" ERROR_EXIST = 1 ").Count
                    SetPendingFileControlsOnPage()
                    If Me.ParticipantErrorCountforError > 0 Then
                        HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_MISSMATCH_RECORDS).DisplayText, EnumMessageTypes.Error)
                        btnRunDARImportFile.Enabled = False
                    End If
                Else
                    Me.gvDARImportedRecordsList.DataBind()

                    btnRunDARImportFile.Enabled = False
                    divTotalRecords.Visible = True
                End If

            End If
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "SaveDARImportFileData END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Private Function SaveFileIntoImportFolder() As String
        Dim uploadFile As HttpPostedFile
        Dim importFolderPath As String
        Dim arrayFileName() As String
        Dim fileName As String
        Dim timeStamp As String
        Dim idx As Integer
        Dim newfilename As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DAR FileImport", "SaveFileIntoImportFolder() START")
            uploadFile = FileField.PostedFile
            importFolderPath = ConfigurationSettings.AppSettings("DAR") + "\\"
            ' Create directory if it doesnot exist
            If Not Directory.Exists(importFolderPath) Then
                'Directory.CreateDirectory(importFolderPath)
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_FOLDERNOTEXISTS).DisplayText, EnumMessageTypes.Error)
                Exit Function
            End If
            ' Get filename 
            arrayFileName = uploadFile.FileName.Split("\")
            timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff")
            fileName = arrayFileName.GetValue(arrayFileName.Length - 1)
            idx = fileName.Trim().Length - 4
            newfilename = fileName.Trim().Remove(idx, 4).ToString + "_" + timeStamp + ".csv"

            importFolderPath = importFolderPath & newfilename
            'if DAR file does not exist in ACH folder then upload, otherwise delete & Upload again.
            If Not File.Exists(importFolderPath) Then
                uploadFile.SaveAs(importFolderPath)
            Else
                File.Delete(importFolderPath)
                uploadFile.SaveAs(importFolderPath)
            End If
            Me.FolderPath = importFolderPath
            Me.ImportedFileName = newfilename

        Catch ex As Exception
            HelperFunctions.LogException("SaveFileIntoImportFolder", ex)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DAR FileImport", "SaveFileIntoImportFolder END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Finally
            uploadFile = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DAR FileImport", "SaveFileIntoImportFolder END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
        Return importFolderPath
    End Function

    Private Sub gvDARImportedRecordsList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDARImportedRecordsList.RowDataBound
        ' Dim records As New DataTable
        ' Dim row As DataRow()
        Dim isErrorRecordExists As Boolean

        Dim ImgErrorRecordsExists As ImageButton
        Dim lblNoErrorRecordExists As Label
        Dim hndCheckStatus As HiddenField
        Dim index As Integer
        Dim columnValue As String
        Try
            isErrorRecordExists = False
            If e.Row.RowType = DataControlRowType.Header Then
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then

                ImgErrorRecordsExists = e.Row.FindControl("ImgErrorDetails")
                lblNoErrorRecordExists = e.Row.FindControl("lblErrorDetails")
                If ImgErrorRecordsExists IsNot Nothing AndAlso lblNoErrorRecordExists IsNot Nothing Then
                    ImgErrorRecordsExists.Visible = False
                    lblNoErrorRecordExists.Visible = False
                    If (e.Row.Cells(IsErrorExistsField).Text.Trim = "0") Then
                        lblNoErrorRecordExists.Visible = True
                    Else
                        ImgErrorRecordsExists.Visible = True

                    End If
                End If
                hndCheckStatus = e.Row.FindControl("hndCheckStatus")
                If (hndCheckStatus.Value = "RETURNED") Then
                    e.Row.CssClass = "BG_ColourCheckStatusRETURNED"
                End If


                index = GetColumnIndexByName(e.Row, "GrossAmount")
                If ((e.Row.Cells(index).Text.Length = 0) Or e.Row.Cells(index).Text.Equals("&nbsp;") Or e.Row.Cells(index).Text.Equals("") Or e.Row.Cells(index).Text.Equals(String.Empty)) Then
                    e.Row.Cells(index).Text = String.Empty
                Else
                    columnValue = Convert.ToDecimal(e.Row.Cells(index).Text).ToString("#,##0.00").ToString
                    e.Row.Cells(index).Text = columnValue
                End If

                index = GetColumnIndexByName(e.Row, "NetAmount")
                If ((e.Row.Cells(index).Text.Length = 0) Or e.Row.Cells(index).Text.Equals("&nbsp;") Or e.Row.Cells(index).Text.Equals("") Or e.Row.Cells(index).Text.Equals(String.Empty)) Then
                    e.Row.Cells(index).Text = String.Empty
                Else
                    columnValue = Convert.ToDecimal(e.Row.Cells(index).Text).ToString("#,##0.00").ToString
                    e.Row.Cells(index).Text = columnValue
                End If

                index = GetColumnIndexByName(e.Row, "TotalFederalAmount")
                If ((e.Row.Cells(index).Text.Length = 0) Or e.Row.Cells(index).Text.Equals("&nbsp;") Or e.Row.Cells(index).Text.Equals("") Or e.Row.Cells(index).Text.Equals(String.Empty)) Then
                    e.Row.Cells(index).Text = String.Empty
                Else
                    columnValue = Convert.ToDecimal(e.Row.Cells(index).Text).ToString("#,##0.00").ToString
                    e.Row.Cells(index).Text = columnValue
                End If

                index = GetColumnIndexByName(e.Row, "StateAmount")
                If ((e.Row.Cells(index).Text.Length = 0) Or e.Row.Cells(index).Text.Equals("&nbsp;") Or e.Row.Cells(index).Text.Equals("") Or e.Row.Cells(index).Text.Equals(String.Empty)) Then
                    e.Row.Cells(index).Text = String.Empty
                Else
                    columnValue = Convert.ToDecimal(e.Row.Cells(index).Text).ToString("#,##0.00").ToString
                    e.Row.Cells(index).Text = columnValue
                End If

                index = GetColumnIndexByName(e.Row, "LocalFlatAmount")
                If ((e.Row.Cells(index).Text.Length = 0) Or e.Row.Cells(index).Text.Equals("&nbsp;") Or e.Row.Cells(index).Text.Equals("") Or e.Row.Cells(index).Text.Equals(String.Empty)) Then
                    e.Row.Cells(index).Text = String.Empty
                Else
                    columnValue = Convert.ToDecimal(e.Row.Cells(index).Text).ToString("#,##0.00").ToString
                    e.Row.Cells(index).Text = columnValue
                End If

                index = GetColumnIndexByName(e.Row, "OtherWithholdings")
                If ((e.Row.Cells(index).Text.Length = 0) Or e.Row.Cells(index).Text.Equals("&nbsp;") Or e.Row.Cells(index).Text.Equals("") Or e.Row.Cells(index).Text.Equals(String.Empty)) Then
                    e.Row.Cells(index).Text = String.Empty
                Else
                    columnValue = Convert.ToDecimal(e.Row.Cells(index).Text).ToString("#,##0.00").ToString
                    e.Row.Cells(index).Text = columnValue
                End If

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DARImportFile_DARImportedRecordsList_RowDataBound", ex)
        Finally
            lblNoErrorRecordExists = Nothing
            ImgErrorRecordsExists = Nothing

        End Try
    End Sub
    Private Function GetColumnIndexByName(ByVal row As GridViewRow, ByVal columnName As String) As Integer
        Dim columnIndex As Integer = 0

        For Each cell As DataControlFieldCell In row.Cells

            If TypeOf cell.ContainingField Is BoundField Then
                If (CType(cell.ContainingField, BoundField)).DataField.Equals(columnName) Then Exit For
            End If

            columnIndex += 1
        Next

        Return columnIndex
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetErrorDetailsForEachRecord(ByVal requestedImportBaseId As String, ByVal requestedImportDetailId As String) As ImportFileErrorDetails()
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "GetErrorDetailsForEachRecord START")

            Dim details As New List(Of ImportFileErrorDetails)()
            Dim g_dataset_dsMemberInfo As New DataSet
            Dim errorDetailsRecords As New ImportFileErrorDetails()
            If Not String.IsNullOrEmpty(requestedImportBaseId) And Not String.IsNullOrEmpty(requestedImportDetailId) Then
                g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.GetErrorDetailsForEachRecord(requestedImportBaseId, requestedImportDetailId)

                If g_dataset_dsMemberInfo.Tables(0).Rows.Count > 0 Then
                    For Each dtrow As DataRow In g_dataset_dsMemberInfo.Tables(0).Rows
                        errorDetailsRecords = New ImportFileErrorDetails()
                        If (dtrow("intFundNo").ToString() = "0") Then
                            errorDetailsRecords.FundID = String.Empty
                        Else
                            errorDetailsRecords.FundID = dtrow("intFundNo").ToString()
                        End If

                        errorDetailsRecords.ErrrorMessage = dtrow("chvErrorDesc").ToString()
                        details.Add(errorDetailsRecords)
                    Next
                    Return details.ToArray()
                End If
            End If
            Return Nothing
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "GetErrorDetailsForEachRecord END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try

    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessionValues()

            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnConfirmDialogYesDiscard_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYesDiscard.Click
        Dim ymcaNo As Dictionary(Of String, String)
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "Discard Yes Click START")
            'if user has read-only or none rights, message will be displayed and further process will not happen.
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else

                If Me.ImportedBaseHeaderID > 0 Then
                    'Throw New Exception
                    'Below method will be called to discard DAR file 
                    If (YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.DiscardImportedDARFile(ImportedBaseHeaderID)) Then
                        ClearSessionValues()
                        'After sucessfull discard, acknowledgment message will be shown to user on the screen
                        HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_DISCARDED).DisplayText, EnumMessageTypes.Success)
                    End If
                    SetDefaultPageLoadControls()
                End If
            End If
        Catch ex As SqlException
            HelperFunctions.LogException("btnConfirmDialogYesDiscard_Click", ex)
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_SQL_EXCEPTION).DisplayText, EnumMessageTypes.Error, Nothing)
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogYesDiscard_Click", ex)
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DOTNET_EXCEPTION).DisplayText, EnumMessageTypes.Error, Nothing)
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "Discard Yes Click END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    ''' <summary>
    ''' Clear session values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSessionValues()
        Try
            Me.ImportedFileRecordsDataSet = Nothing
            Me.ImportedBaseHeaderID = 0
            Me.ImportedFileName = Nothing
            Me.FolderPath = Nothing
        Catch
            Throw
        End Try

    End Sub

    Private Sub SetTotalValuesinSummarySection(ByVal dtFileData As DataTable)

        Dim d_totalGrossAmount As Decimal = 0.0
        Dim d_totalStateTaxWithheldAmount As Decimal = 0.0
        Dim d_totalFedTaxWithheldAmount As Decimal = 0.0
        Dim d_totalLocalTaxWithheldAmount As Decimal = 0.0  'NYC or Yonkers tax Withheld
        Dim d_totalOtherWithheldAmount As Decimal = 0.0
        Dim d_totalNetAmount As Decimal = 0.0
        Dim int_totalRecords As Integer = 0

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "SetTotalValuesinSummarySection START")

            If (Not IsDBNull(dtFileData.Rows(0)("numGrossAmt"))) Then
                d_totalGrossAmount = Convert.ToDecimal(dtFileData.Rows(0)("numGrossAmt").ToString())
                lblTotalGrossAmountValue.InnerText = "$" + d_totalGrossAmount.ToString("#,##0.00").ToString()
            Else
                lblTotalGrossAmountValue.InnerText = "$ 0.00"
            End If
            If (Not IsDBNull(dtFileData.Rows(0)("numTotalStateTaxAmount"))) Then
                d_totalStateTaxWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numTotalStateTaxAmount").ToString())
                lblTotalStateTaxWithheldValue.InnerText = "$" + d_totalStateTaxWithheldAmount.ToString("#,##0.00").ToString()
            Else
                lblTotalStateTaxWithheldValue.InnerText = "$ 0.00"
            End If
            If (Not IsDBNull(dtFileData.Rows(0)("numTotalFederalAmount"))) Then
                d_totalFedTaxWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numTotalFederalAmount").ToString())
                lblTotalFedTaxWithheldValue.InnerText = "$" + d_totalFedTaxWithheldAmount.ToString("#,##0.00").ToString()
            Else
                lblTotalFedTaxWithheldValue.InnerText = "$ 0.00"
            End If
            If (Not IsDBNull(dtFileData.Rows(0)("numLocalFlatAmount"))) Then
                d_totalLocalTaxWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numLocalFlatAmount").ToString())
            End If
            If (Not IsDBNull(dtFileData.Rows(0)("numTotalDeductionAmt"))) Then
                d_totalOtherWithheldAmount = Convert.ToDecimal(dtFileData.Rows(0)("numTotalDeductionAmt").ToString())
            End If
            If (Not IsDBNull(dtFileData.Rows(0)("numNetAmount"))) Then
                d_totalNetAmount = Convert.ToDecimal(dtFileData.Rows(0)("numNetAmount").ToString())
                lblTotalNetAmountValue.InnerText = "$" + d_totalNetAmount.ToString("#,##0.00").ToString()
            Else
                lblTotalNetAmountValue.InnerText = "$ 0.00"
            End If
            If (Not IsDBNull(dtFileData.Rows(0)("intTotalRecords"))) Then
                int_totalRecords = Convert.ToDecimal(dtFileData.Rows(0)("intTotalRecords").ToString())
                lblRecords.InnerText = int_totalRecords
            Else
                lblRecords.InnerText = 0
            End If
            lblTotalLocalTaxandOtherWithheldValue.InnerText = "$" + (d_totalLocalTaxWithheldAmount + d_totalOtherWithheldAmount).ToString("#,##0.00").ToString()
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "SetTotalValuesinSummarySection END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Private Sub btnConfirmDialogYesRun_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYesRun.Click
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "ConfirmDialogYesRun Click START")
            'if user has read-only or none rights, message will be displayed and further process will not happen.
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else

                If Me.ImportedBaseHeaderID > 0 Then
                    'Below method will be called to save DAR import file records
                    If (YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.ProcessImportedDARFile(ImportedBaseHeaderID)) Then
                        ClearSessionValues()
                        'After sucessfull processing, acknowledgment message will be shown to user on the screen
                        HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_SUCCESS).DisplayText, EnumMessageTypes.Success)
                    End If

                    SetDefaultPageLoadControls()
                End If

            End If
      
        Catch ex As SqlException
            HelperFunctions.LogException("btnConfirmDialogYesRun_Click", ex)
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_SQL_EXCEPTION).DisplayText, EnumMessageTypes.Error, Nothing)
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogYesRun_Click", ex)
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DOTNET_EXCEPTION).DisplayText, EnumMessageTypes.Error, Nothing)
        Finally
            btnRunDARImportFile.Enabled = False
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport", "ConfirmDialogYesRun Click END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub

    Protected Sub btnPrintDARImportFileList_Click(sender As Object, e As EventArgs) Handles btnPrintDARImportFileList.Click
        Dim strSessionName As String = String.Empty
        Dim strReportName As String = String.Empty
        Dim dsSelectedDARFileRecords As New DataSet
        Dim dtRecords As New DataSet

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DAR FileImport", "btnPrintDARImportFileList_Click() START")
            If Me.ParticipantErrorCountforError > 0 Then
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_IMPORT_DAR_MISSMATCH_RECORDS).DisplayText, EnumMessageTypes.Error)
                btnRunDARImportFile.Enabled = False
            End If
            dsSelectedDARFileRecords = YMCARET.YmcaBusinessObject.BankAcknowledgementImportBO.GetDARFileImportReport(Me.ImportedBaseHeaderID)
            dtRecords = dsSelectedDARFileRecords
            strSessionName = "dsSelectedDARFileRecords"
            strReportName = "DAR_File_Import"

            If HelperFunctions.isEmpty(dsSelectedDARFileRecords) Then
                HelperFunctions.ShowMessageToUser("No record(s) exist to print.", EnumMessageTypes.Error)
                Exit Sub
            End If
            Session(strSessionName) = dsSelectedDARFileRecords
            Session("ReportName") = strReportName
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "CALLREPORT", "CallLetter();", True)

            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DARFileImport page load", "btnPrintDARImportFileList_Click() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.LogException("btnPrintDARImportFileList_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)

        End Try
    End Sub
End Class

Public Class ImportFileErrorDetails
    Public Property Type() As String
        Get
            Return m_Type
        End Get
        Set(ByVal value As String)
            m_Type = value
        End Set
    End Property
    Private m_Type As String

    Public Property ErrrorMessage() As String
        Get
            Return m_errormsg
        End Get
        Set(ByVal value As String)
            m_errormsg = value
        End Set
    End Property
    Private m_errormsg As String

    Public Property FundID() As String
        Get
            Return m_FundID
        End Get
        Set(ByVal value As String)
            m_FundID = value
        End Set
    End Property
    Private m_FundID As String

End Class