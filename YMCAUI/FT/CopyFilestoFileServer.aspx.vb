' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
''********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Aparna Samala    17-05-2007     YREN-3197 
'Ashutosh Patil   22-May-2007     Change in Email Functionality
'Sanjay Rawat     8-June-2009     To Keep track of all IDX and PDF file generated.
'Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
'Dinesh Kanojia   06-Jan-2015     BT-2735:Add activities logging functionality in RMD and RollIn batch creation.
'Manthan Rajguru  2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Sanjay S         2015.10.20      YRS-AT-2614 - YRS: files for IDM - .idx filename needs to match .pdf filename
'********************************************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports System.IO
Imports System.Collections
Imports System
Imports System.Text
Imports System.Net
Imports YMCAObjects
Imports YMCARET.CommonUtilities
Public Class CopyFilestoFileServer
    Inherits System.Web.UI.Page
    Protected WithEvents LabelProgress As System.Web.UI.WebControls.Label
    Dim FileName As String
    Dim l_str_ExceptionMode As String = "IE"
    Dim l_str_Exception As String

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub




#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtListFile As DataTable
        Dim i As Integer
        Dim strSourceFolder As String
        Dim strSourceFile As String
        Dim strDestFolder As String
        Dim strDestFile As String
        Dim blnResult As Boolean
        Dim popupScript As String = String.Empty
        Dim boolOverwrite As Boolean
        Dim strOverwrite As String
        Dim strDelete As String
        Dim boolDelete As Boolean
        Dim l_datatable As New DataTable
        Dim msg As String
        Dim l_intCounter As Integer = 0
        Dim l_string_error As String = String.Empty
        Dim l_stringErrorMessage As String = String.Empty
        ''Added by sanjay
        Dim bool_idxCopyinitiated As Boolean
        Dim bool_idxCopied As Boolean
        Dim bool_idxDeletedAfterCopy As Boolean
        Dim bool_pdfCopyinitiated As Boolean
        Dim bool_pdfCopied As Boolean
        Dim bool_pdfDeletedAfterCopy As Boolean
        Dim str_idxfilename As Boolean
        Dim str_pdffilename As Boolean
        ''Ends here
        Try
            'by Aparna YREN-3196 12/04/2007
            ' l_stringtomailAdd = ConfigurationSettings.AppSettings("DefaultMailTo").ToString.Trim()
            'setting up the error message incase error message is not trapped
            'l_str_ExceptionMode = ConfigurationSettings.AppSettings("ExceptionMode")
            l_string_error = "there was some problem"
            Session("FileCopied") = False
            'Put user code to initialize the page here
            If Session("LoggedUserKey") Is Nothing Then
                'Response.Redirect("Login.aspx", False)
                Page.Dispose()
            End If
            'Check for overwriting
            If Request.Form("OK") = "OK" Then
                Page.Dispose()
            End If
            If Request.QueryString("OR") Is Nothing Then
                strOverwrite = ""
            Else
                strOverwrite = Request.QueryString("OR")
            End If
            strOverwrite = strOverwrite.Trim()
            If strOverwrite = "1" Then
                boolOverwrite = True
            Else
                boolOverwrite = False
            End If
            'Check for deleting
            If Request.QueryString("DEL") Is Nothing Then
                strDelete = ""
            Else
                strDelete = Request.QueryString("DEL")
            End If
            strDelete = strDelete.Trim()
            If strDelete = "0" Then
                boolDelete = False
            Else
                boolDelete = True
            End If
            If Not Session("FTFileList") Is Nothing Then
                dtListFile = Session("FTFileList")
                If Not dtListFile Is Nothing Then
                    'by aparna YREN-3197 25/05/2007
                    'creating a datatable of all those files which are not copied to the destination folder
                    l_datatable = dtListFile.Clone
                    'by aparna YREN-3197 25/05/2007
                    For i = 0 To dtListFile.Rows.Count - 1
                        'by Aparna YREN-3196 11/04/2007
                        l_intCounter = i
                        'by Aparna YREN-3196 11/04/2007
                        strSourceFolder = dtListFile.Rows(i)(0)
                        If strSourceFolder = "" Then
                            Exit For
                        End If
                        strSourceFolder = CType(dtListFile.Rows(i)(0), String)
                        strSourceFile = CType(dtListFile.Rows(i)(1), String)
                        strDestFolder = CType(dtListFile.Rows(i)(2), String)
                        strDestFile = CType(dtListFile.Rows(i)(3), String)
                        ''Added by sanjay
                        FileName = strSourceFile.Substring(strSourceFile.LastIndexOf("\") + 1)
                        If strSourceFile.Substring(strSourceFile.Length - 3, 3) = "idx" Then
                            Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", "")
                        ElseIf strSourceFile.Substring(strSourceFile.Length - 3, 3) = "pdf" Then
                            Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", "")
                        End If
                        ''Ends here
                        If Directory.Exists(strDestFolder) Then
                            Try
                                CopyFile(strSourceFolder, strSourceFile, strDestFolder, strDestFile, boolOverwrite, boolDelete)
                                Session("FileCopied") = True
                            Catch ex As Exception
                                HelperFunctions.LogException("CopyFilestoFileServer_Page_Load", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                            End Try
                        Else
                            'YREN-3197 Aparna 27/04/2007
                            Try
                                Dim l_boolfolder As Boolean = False
                                If dtListFile.Columns.Contains("ArchiveFolder") Then
                                    l_string_error = "Directory does not exist for output file"
                                    If l_stringErrorMessage = String.Empty Then
                                        l_stringErrorMessage = "Directory does not exist for the output file.Please find the missing files in the Archive Folder."
                                    End If
                                    If strSourceFolder = "" Then
                                        Exit For
                                    End If
                                    'l_datatable = dtListFile.Clone
                                    'Create the datatable to generate tehe execption file as files are 
                                    'not copied to destination folder
                                    l_datatable.ImportRow(dtListFile.Rows(i))
                                    '  l_datatable.AcceptChanges()
                                    strSourceFolder = CType(dtListFile.Rows(i)(0), String)
                                    strSourceFile = CType(dtListFile.Rows(i)(1), String)
                                    strDestFolder = CType(dtListFile.Rows(i)(4), String)
                                    strDestFile = CType(dtListFile.Rows(i)(5), String)
                                    If Directory.Exists(strDestFolder) Then
                                        Try
                                            CopyFile(strSourceFolder, strSourceFile, strDestFolder, strDestFile, boolOverwrite, boolDelete)
                                            Session("FileCopied") = True
                                        Catch ex As Exception
                                            HelperFunctions.LogException("CopyFilestoFileServer_Page_Load", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                                            l_str_Exception = ""
                                            If l_str_ExceptionMode = "IE" Then
                                                l_str_Exception = ex.Message
                                                If Not ex.InnerException Is Nothing Then
                                                    l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                                End If
                                            ElseIf l_str_ExceptionMode = "ST" Then
                                                l_str_Exception = ex.StackTrace
                                            End If

                                            If strSourceFile.Substring(strSourceFile.Length - 3, 3) = "idx" Then
                                                Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", l_str_Exception)
                                            ElseIf strSourceFile.Substring(strSourceFile.Length - 3, 3) = "pdf" Then
                                                Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", l_str_Exception)
                                            End If
                                            Throw
                                        End Try
                                    Else
                                        'if folder doesnot exist create the folder runtime:
                                        l_boolfolder = Me.CreateFolders(strDestFolder)
                                        If l_boolfolder Then
                                            Try
                                                CopyFile(strSourceFolder, strSourceFile, strDestFolder, strDestFile, boolOverwrite, boolDelete)
                                                Session("FileCopied") = True
                                            Catch ex As Exception
                                                HelperFunctions.LogException("CopyFilestoFileServer_Page_Load", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                                                l_str_Exception = ""
                                                If l_str_ExceptionMode = "IE" Then
                                                    l_str_Exception = ex.Message
                                                    If Not ex.InnerException Is Nothing Then
                                                        l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                                    End If
                                                ElseIf l_str_ExceptionMode = "ST" Then
                                                    l_str_Exception = ex.StackTrace
                                                End If

                                                If strSourceFile.Substring(strSourceFile.Length - 3, 3) = "idx" Then
                                                    Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", l_str_Exception)
                                                ElseIf strSourceFile.Substring(strSourceFile.Length - 3, 3) = "pdf" Then
                                                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", l_str_Exception)
                                                End If
                                                Throw
                                            End Try
                                        Else
                                            popupScript = "<script language='javascript'>" & _
                                                          "alert('Directory could not be created in Archive folder for output file.');self.close();" & _
                                                          "</script> "
                                            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                                                Page.RegisterStartupScript("PopupScript1", popupScript)
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Else
                                    'YREN-3197 Aparna 27/04/2007
                                    'by Aparna YREN-3196 12/04/2007
                                    l_string_error = "Directory does not exist for output file"
                                    '  SendMail(l_string_error, l_intCounter)
                                    Me.GenerateCSVFile(l_string_error, l_intCounter)
                                    Me.UpdateTrackingStatus(l_string_error)
                                    'Throw New Exception("Directory does not exist for EFT Prenote output file.")
                                    popupScript = "<script language='javascript'>" & _
                                                    "alert('Directory does not exist for output file.');self.close();" & _
                                                    "</script> "
                                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                                        Page.RegisterStartupScript("PopupScript1", popupScript)
                                    End If
                                    Exit For
                                End If
                            Catch ex As Exception
                                HelperFunctions.LogException("CopyFilestoFileServer_Page_Load", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                                Throw
                            End Try
                        End If
                    Next
                    l_datatable.AcceptChanges()
                End If
            End If
            'attach the list of files which were not copied to Server
            'yren-3197 Aparna 27/04/2007
            If l_datatable.Rows.Count > 0 Then
                Me.GenerateCSVFile(l_string_error, l_datatable)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_Page_Load", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            'by Aparna YREN-3196 12/04/2007
            l_string_error = ex.Message.ToString()
            'SendMail(l_string_error, l_intCounter)
            Me.GenerateCSVFile(l_string_error, l_intCounter)
            Session("FTFileList") = Nothing
            popupScript = "<script language='javascript'>" & _
                          "alert('Error occured while copying the file, please verify.');self.close();" & _
                          "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
            'Throw
        Finally
            Session("FTFileList") = Nothing
        End Try
        ' If popupScript = String.Empty Then
        msg = msg + "<Script Language='JavaScript'>"
        If Not Session("FromPage") Is Nothing Then
            'by Aparna YREN-3197 17/05/2007
            If Session("FromPage") = "EDI" Or Session("FromPage") = "CopyUtility" Then
                If Session("FromPage") = "CopyUtility" Then
                    If popupScript = String.Empty Then
                        If Session("Message") = Nothing Then
                            If l_stringErrorMessage = String.Empty Then
                                Session("ErrorMessage") = "Files Copied Successfully"
                            Else
                                Session("ErrorMessage") = l_stringErrorMessage
                            End If
                        Else
                            Session("ErrorMessage") = CType(Session("Message"), String).Trim() + vbCrLf + l_stringErrorMessage
                        End If
                    End If
                End If
                Session("FromPage") = Nothing
                msg = msg + "window.opener.document.forms(0).submit();"
            End If
        End If
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)
        '  End If
    End Sub
    'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
    ''' <summary>
    ''' New function added to copy file and update in idm tracking table.
    ''' </summary>
    ''' <param name="pstrSourceFolder"></param>
    ''' <param name="pstrSourceFile"></param>
    ''' <param name="pstrDestFolder"></param>
    ''' <param name="pstrDestFile"></param>
    ''' <param name="pboolOverwrite"></param>
    ''' <param name="pboolDelete"></param>
    ''' <param name="pIdxFileName"></param>
    ''' <param name="pPdfFileName"></param>
    ''' <param name="pIdxFileinitiated"></param>
    ''' <param name="pIdxfileCopied"></param>
    ''' <param name="pIdxFileDeleted"></param>
    ''' <param name="pPdfFileinitiated"></param>
    ''' <param name="pPdffileCopied"></param>
    ''' <param name="pPdfFileDeleted"></param>
    ''' <param name="pFileType"></param>
    ''' <param name="pErrorMsg"></param>
    ''' <param name="strError"></param>
    ''' <param name="strStatus"></param>
    ''' <param name="ArrErrorDataList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function NewCopyFile(ByVal pstrSourceFolder As String, ByVal pstrSourceFile As String, ByVal pstrDestFolder As String, ByVal pstrDestFile As String, ByVal pboolOverwrite As Boolean, ByVal pboolDelete As Boolean,
                                 ByRef pIdxFileName As String, ByRef pPdfFileName As String, ByRef pIdxFileinitiated As Boolean, ByRef pIdxfileCopied As Boolean, ByRef pIdxFileDeleted As Boolean,
                                 ByRef pPdfFileinitiated As Boolean, ByRef pPdffileCopied As Boolean, ByRef pPdfFileDeleted As Boolean, ByRef pFileType As Char, ByRef pErrorMsg As String,
                                 Optional ByRef strError As String = "", Optional strStatus As String = "", Optional ByRef ArrErrorDataList As List(Of ExceptionLog) = Nothing)
        Dim pIdxFileCreated As Boolean = True
        Dim pPdfFilecreated As Boolean = True
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Start: Copying files initiated")
            pErrorMsg = ""
            If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                pFileType = "I"
                pIdxFileinitiated = True
                pIdxFileName = FileName
            ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                pPdfFileinitiated = True
                pFileType = "P"
                pPdfFileName = FileName
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Finish: Copying files initiated")
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Start: Copying files from source to destination")
            File.Copy(pstrSourceFile.ToString, pstrDestFile.ToString, pboolOverwrite)

            If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                pIdxfileCopied = True
            ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                pPdffileCopied = True
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Finish: Copying files from source to destination")

            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Start: Delete files from temp location")
            If pboolDelete = True Then
                File.Delete(pstrSourceFile.ToString)
                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                    pIdxFileDeleted = True
                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                    pPdfFileDeleted = True
                End If
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Finish: Delete files from temp location")
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Start: Exception raise while copying files to IDM server.")
            HelperFunctions.LogException("CopyFilestoFileServer_NewCopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            l_str_Exception = ""
            If l_str_ExceptionMode = "IE" Then
                l_str_Exception = ex.Message
                If Not ex.InnerException Is Nothing Then
                    l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                End If
            End If
            pErrorMsg = l_str_Exception
            strError = l_str_Exception
            strStatus = "error"
            ArrErrorDataList.Add(New ExceptionLog("Exception", "Copy Files to IDM Server", l_str_Exception))
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Finish: Exception raise while copying files to IDM server.")
            'Throw
        End Try
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Start: Final update to IDMTracking table in database")
            FilesTracking(0, pIdxFileCreated, pPdfFilecreated, pIdxFileName, pPdfFileName, pIdxFileinitiated, pIdxfileCopied, pIdxFileDeleted, pPdfFileinitiated, pPdffileCopied, pPdfFileDeleted, pFileType, pErrorMsg)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Finish: Final update to IDMTracking table in database")
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_NewCopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Start: Exception raise while updating IDMTracking table")
            ArrErrorDataList.Add(New ExceptionLog("Exception", "Unable to Update file tracking ", ex.Message))
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("NewCopyFile Method", "Finish: Exception raise while updating IDMTracking table")
            Throw
        End Try

    End Function
    'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
    Private Function CopyFile(ByVal pstrSourceFolder As String, ByVal pstrSourceFile As String, ByVal pstrDestFolder As String, ByVal pstrDestFile As String, ByVal pboolOverwrite As Boolean, ByVal pboolDelete As Boolean, Optional ByRef strError As String = "", Optional strStatus As String = "", Optional ByRef ArrErrorDataList As List(Of ExceptionLog) = Nothing)
        Dim l_string_FileName As String
        Dim l_string_FullFileName As String
        Dim l_string_FileNameBak As String
        Dim l_string_FileNameRen As String
        Dim l_string_Tmp As String
        Dim l_String_FileCreateDate As String
        Dim l_Date_Prenote As DateTime = System.DateTime.Now()
        Dim l_Integer_Index As Integer
        Dim l_String_extn As String
        Try
            If File.Exists(pstrDestFile) Then
                If pboolOverwrite = False Then
                    l_string_FullFileName = pstrDestFile
                    l_string_Tmp = Right(CType(System.DateTime.Now().Year, String).Trim(), 2)
                    l_String_FileCreateDate = l_string_Tmp
                    l_string_Tmp = CType(System.DateTime.Now().Month, String).Trim()
                    l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                    l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                    l_string_Tmp = CType(System.DateTime.Now().Day, String).Trim()
                    l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                    l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                    l_string_FileNameRen = l_String_FileCreateDate
                    l_Integer_Index = l_string_FullFileName.IndexOfAny(".")
                    l_String_extn = Right(l_string_FullFileName, l_string_FullFileName.Length - l_Integer_Index)
                    If l_Integer_Index > 0 Then
                        l_string_FileNameRen = Left(l_string_FullFileName, l_Integer_Index) + "_" + l_string_FileNameRen + l_String_extn
                    Else
                        l_string_FileNameRen = l_string_FullFileName + "_" + l_string_FileNameRen + l_String_extn
                    End If
                    If File.Exists(l_string_FileNameRen.ToString) Then
                        l_string_Tmp = CType(l_Date_Prenote.Hour, String).Trim()
                        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                        l_string_Tmp = CType(l_Date_Prenote.Minute, String).Trim()
                        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                        l_string_Tmp = CType(l_Date_Prenote.Second, String).Trim()
                        l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                        l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                        l_Integer_Index = l_string_FileNameRen.IndexOfAny(".")
                        If l_Integer_Index > 0 Then
                            l_string_FileNameRen = Left(l_string_FileNameRen, l_Integer_Index) + l_String_FileCreateDate + l_String_extn
                        Else
                            l_string_FileNameRen = l_string_FileNameRen + l_String_FileCreateDate + l_String_extn
                        End If

                        Try
                            File.Copy(l_string_FullFileName.ToString, l_string_FileNameRen.ToString)
                            ''''Added by sanjay
                            If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                Me.FilesTracking(0, True, True, FileName, "", True, True, False, False, False, False, "I", "")
                            ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, False, "P", "")
                            End If
                        Catch ex As Exception
                            HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                            l_str_Exception = ""
                            If l_str_ExceptionMode = "IE" Then
                                l_str_Exception = ex.Message
                                If Not ex.InnerException Is Nothing Then
                                    l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                End If
                            ElseIf l_str_ExceptionMode = "ST" Then
                                l_str_Exception = ex.StackTrace
                            End If
                            If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", l_str_Exception)
                            ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", l_str_Exception)
                            End If
                            strError = l_str_Exception
                            strStatus = "error"
                            ArrErrorDataList.Add(New ExceptionLog("Exception", "Copy Files to IDM Server", l_str_Exception))
                            Throw
                        End Try
                        ''Added by Sanjay Rawat Ends here
                        'do code--Ashut
                        If pboolDelete = True Then
                            Try
                                File.Delete(l_string_FullFileName.ToString)
                                '''''''''Added by sanjay
                                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                    Me.FilesTracking(0, True, True, FileName, "", True, True, True, False, False, False, "I", "")
                                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, True, "P", "")
                                End If
                            Catch ex As Exception
                                HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                                l_str_Exception = ""
                                If l_str_ExceptionMode = "IE" Then
                                    l_str_Exception = ex.Message
                                    If Not ex.InnerException Is Nothing Then
                                        l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                    End If
                                ElseIf l_str_ExceptionMode = "ST" Then
                                    l_str_Exception = ex.StackTrace
                                End If

                                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                    Me.FilesTracking(0, True, True, FileName, "", True, True, False, False, False, False, "I", l_str_Exception)
                                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, False, "P", l_str_Exception)
                                End If
                                strError = l_str_Exception
                                strStatus = "error"
                                ArrErrorDataList.Add(New ExceptionLog("Exception", "Deleting Files from temp locations.", l_str_Exception))
                                Throw
                            End Try
                            '''''''''Ends here
                        End If
                        'File has been renamed it, now the file can be overwrite it. 
                        pboolOverwrite = True
                        'Throw New Exception("File " + l_string_FileNameRen.ToString.Replace("\\", "\") + " already exists. Please delete or rename file.")
                    Else
                        Try
                            File.Copy(l_string_FullFileName.ToString, l_string_FileNameRen.ToString)
                            ''Added by sanjay
                            If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                Me.FilesTracking(0, True, True, FileName, "", True, True, False, False, False, False, "I", "")
                            ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, False, "P", "")
                            End If

                        Catch ex As Exception
                            HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                            l_str_Exception = ""
                            If l_str_ExceptionMode = "IE" Then
                                l_str_Exception = ex.Message
                                If Not ex.InnerException Is Nothing Then
                                    l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                End If
                            ElseIf l_str_ExceptionMode = "ST" Then
                                l_str_Exception = ex.StackTrace
                            End If

                            If pstrSourceFile.Substring(pstrSourceFolder.Length - 3, 3) = "idx" Then
                                Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", l_str_Exception)
                            ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", l_str_Exception)
                            End If

                            strError = l_str_Exception
                            strStatus = "error"

                            ArrErrorDataList.Add(New ExceptionLog("Exception", "Copy Files to IDM Server", l_str_Exception))
                            Throw
                        End Try
                        ''Added by Sanjay Rawat Ends here
                        'do code--Ashut
                        'File has been renamed it, now the file can be overwrite it. 
                        pboolOverwrite = True
                        If pboolDelete = True Then

                            Try
                                File.Delete(l_string_FullFileName.ToString)
                                '''''''''Added by sanjay
                                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                    Me.FilesTracking(0, True, True, FileName, "", True, True, True, False, False, False, "I", "")
                                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, True, "P", "")
                                End If

                            Catch ex As Exception
                                HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                                l_str_Exception = ""
                                If l_str_ExceptionMode = "IE" Then
                                    l_str_Exception = ex.Message
                                    If Not ex.InnerException Is Nothing Then
                                        l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                                    End If
                                ElseIf l_str_ExceptionMode = "ST" Then
                                    l_str_Exception = ex.StackTrace
                                End If

                                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                                    Me.FilesTracking(0, True, True, FileName, "", True, True, False, False, False, False, "I", l_str_Exception)
                                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, False, "P", l_str_Exception)
                                End If

                                strError = l_str_Exception
                                strStatus = "error"
                                ArrErrorDataList.Add(New ExceptionLog("Exception", "Deleting Files from temp locations.", l_str_Exception))
                                Throw
                            End Try
                            '''''''''Ends here
                        End If
                    End If
                End If
            End If
            Try
                File.Copy(pstrSourceFile.ToString, pstrDestFile.ToString, pboolOverwrite)
                ''Added by sanjay
                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                    Me.FilesTracking(0, True, True, FileName, "", True, True, False, False, False, False, "I", "")
                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, False, "P", "")
                End If

            Catch ex As Exception
                HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                l_str_Exception = ""
                If l_str_ExceptionMode = "IE" Then
                    l_str_Exception = ex.Message
                    If Not ex.InnerException Is Nothing Then
                        l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                    End If
                ElseIf l_str_ExceptionMode = "ST" Then
                    l_str_Exception = ex.StackTrace
                End If

                If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                    Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", l_str_Exception)
                ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                    Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", l_str_Exception)
                End If

                strError = l_str_Exception
                strStatus = "error"
                ArrErrorDataList.Add(New ExceptionLog("Exception", "Copy Files to IDM Server", l_str_Exception))
                Throw
            End Try
            ''Ends here

            If pboolDelete = True Then
                Try
                    File.Delete(pstrSourceFile.ToString)
                    '''''''''''''''''Added by sanjay
                    If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                        Me.FilesTracking(0, True, True, FileName, "", True, True, True, False, False, False, "I", "")
                    ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                        Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, True, "P", "")
                    End If

                Catch ex As Exception
                    HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                    l_str_Exception = ""
                    If l_str_ExceptionMode = "IE" Then
                        l_str_Exception = ex.Message
                        If Not ex.InnerException Is Nothing Then
                            l_str_Exception = "Error : " & ex.Message & vbCrLf & "Inner Exception : " & ex.InnerException.Message
                        End If
                    ElseIf l_str_ExceptionMode = "ST" Then
                        l_str_Exception = ex.StackTrace
                    End If

                    If pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "idx" Then
                        Me.FilesTracking(0, True, True, FileName, "", True, True, False, False, False, False, "I", l_str_Exception)
                    ElseIf pstrSourceFile.Substring(pstrSourceFile.Length - 3, 3) = "pdf" Then
                        Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, True, False, "P", l_str_Exception)
                    End If
                    strError = l_str_Exception
                    strStatus = "error"
                    ArrErrorDataList.Add(New ExceptionLog("Exception", "Deleting Files from temp locations.", l_str_Exception))
                    Throw
                End Try
                '''''''''''''''''Ends here
            End If
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_CopyFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            Throw
        End Try
    End Function
    Private Sub SendMail(ByVal l_string_error As String)
        Try
            Dim obj As MailUtil
            Dim l_strEmailCC As String = ""
            Dim l_Attachments As String = ""
            Dim l_stringDocName As String = String.Empty
            Dim dtListFile As New DataTable
            Dim i As Integer = 0
            Dim strSourceFolder As String
            Dim strSourceFile As String
            Dim strDestFolder As String
            Dim strDestFile As String
            Dim l_DataTable As New DataTable
            Dim l_DataRow As DataRow
            Dim l_stringDefaultId As String = ""
            Dim l_str_msg As String
            'creating the  body of the letter
            Dim l_stringMessage As String
            l_stringMessage = "The attached document contains a list of file(s) that were not copied to the destination, because of the following error: " + l_string_error + ControlChars.CrLf
            l_stringMessage += "Please take immediate action to resolve this situation and copy these files to the respective destination folder(s)" + ControlChars.CrLf
            'Done with the body of the letter.
            obj = New MailUtil
            obj.MailCategory = "ADMIN"
            If obj.MailService = False Then Exit Sub
            obj.MailMessage = l_stringMessage
            obj.Subject = "List of Files which couldn't be copied to IDM Server."
            If Not Session("StringDestFilePath") Is Nothing Then
                'obj.MailAttachments = Session("StringDestFilePath")
                obj.MailAttachments.Add(Session("StringDestFilePath"))
            End If
            obj.Send()
        Catch 
            Throw
        End Try
    End Sub
    Private Function GenerateCSVFile(ByVal l_stringError As String, ByVal l_intCounter As Integer) As Boolean
        Try
            Dim dr As DataRow
            Dim dtexport As New DataTable
            Dim l_dsExport As DataSet
            Dim batchid As String
            Dim struniqueid As String
            Dim l_filenameprefix As String
            Dim l_filename As String
            Dim l_filenamesuffix As String
            Dim l_severfilename As String
            Dim l_fileDel As String
            'new
            'Get the folder name from the Web config.
            'create the 
            l_filenameprefix = ConfigurationSettings.AppSettings("FTLIST")
            l_filenamesuffix = "_" & Format(Now, "ddMMMyyyy")
            l_fileDel = "Exceptions" + l_filenamesuffix + ".csv"
            l_filename = l_filenameprefix.Trim() + "\" + l_fileDel
            Session("StringDestFilePath") = l_filename
            If CreateCSVFile(l_filename, l_intCounter) Then
                Me.SendMail(l_stringError)
                GenerateCSVFile = True
            Else
                GenerateCSVFile = False
            End If
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_GenerateCSVFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            Throw
        End Try
    End Function
    Private Function GenerateCSVFile(ByVal l_stringError As String, ByVal p_datatable As DataTable) As Boolean
        Try
            Dim dr As DataRow
            Dim dtexport As New DataTable
            Dim l_dsExport As DataSet
            Dim batchid As String
            Dim struniqueid As String
            Dim l_filenameprefix As String
            Dim l_filename As String
            Dim l_filenamesuffix As String
            Dim l_severfilename As String
            Dim l_fileDel As String
            'new
            'Get the folder name from the Web config.
            'create the 
            l_filenameprefix = ConfigurationSettings.AppSettings("FTLIST")
            l_filenamesuffix = "_" & Format(Now, "ddMMMyyyy")
            l_fileDel = "Exceptions" + l_filenamesuffix + ".csv"
            l_filename = l_filenameprefix.Trim() + "\" + l_fileDel
            Session("StringDestFilePath") = l_filename
            If CreateCSVFile(l_filename, p_datatable) Then
                Me.SendMail(l_stringError)
                GenerateCSVFile = True
            Else
                GenerateCSVFile = False
            End If
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_GenerateCSVFile()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            Throw
        End Try
    End Function
    Private Overloads Function CreateCSVFile(ByVal Parameter_String_Filename As String, ByVal l_intCounter As Integer) As Boolean
        Dim l_stringbuilder As New StringBuilder
        Dim l_DataRow As DataRow
        Dim l_DatatableCSV As DataTable
        Dim dtListFile As New DataTable
        Dim i As Integer = 0
        Dim strSourceFolder As String
        Dim strSourceFile As String
        Dim strDestFolder As String
        Dim strDestFile As String
        Try
            If Not Session("FTFileList") Is Nothing Then
                dtListFile = Session("FTFileList")
                If Not dtListFile.Rows.Count = 0 Then
                    Dim l_StreamWriter_File As StreamWriter = File.CreateText(Parameter_String_Filename)
                    l_stringbuilder.Append("Source File" & "," & "Destination File")
                    l_stringbuilder.Append(ControlChars.CrLf)
                    For i = l_intCounter To dtListFile.Rows.Count - 1
                        strSourceFolder = CType(dtListFile.Rows(i)(0), String)
                        strSourceFile = CType(dtListFile.Rows(i)(1), String)
                        strDestFolder = CType(dtListFile.Rows(i)(2), String)
                        strDestFile = CType(dtListFile.Rows(i)(3), String)
                        l_stringbuilder.Append(strSourceFile & ",")
                        l_stringbuilder.Append(strDestFile & ",")
                        l_stringbuilder.Append(ControlChars.CrLf)
                    Next
                    l_StreamWriter_File.WriteLine(l_stringbuilder)
                    l_StreamWriter_File.Close()
                    CreateCSVFile = True
                End If
            Else
                CreateCSVFile = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    'for the utility YREN-3197 APARNA 27/04/2007
    Private Overloads Function CreateCSVFile(ByVal Parameter_String_Filename As String, ByVal p_datatable As DataTable) As Boolean
        Dim l_stringbuilder As New StringBuilder
        Dim l_DataRow As DataRow
        Dim l_DatatableCSV As DataTable
        Dim dtListFile As New DataTable
        Dim i As Integer = 0
        Dim strSourceFolder As String
        Dim strSourceFile As String
        Dim strDestFolder As String
        Dim strDestFile As String
        Try
            If Not p_datatable Is Nothing Then
                dtListFile = p_datatable
                If Not dtListFile.Rows.Count = 0 Then
                    Dim l_StreamWriter_File As StreamWriter = File.CreateText(Parameter_String_Filename)
                    l_stringbuilder.Append("Source File" & "," & "Destination File")
                    l_stringbuilder.Append(ControlChars.CrLf)
                    For i = 0 To dtListFile.Rows.Count - 1
                        strSourceFolder = CType(dtListFile.Rows(i)(0), String)
                        strSourceFile = CType(dtListFile.Rows(i)(1), String)
                        strDestFolder = CType(dtListFile.Rows(i)(2), String)
                        strDestFile = CType(dtListFile.Rows(i)(3), String)
                        l_stringbuilder.Append(strSourceFile & ",")
                        l_stringbuilder.Append(strDestFile & ",")
                        l_stringbuilder.Append(ControlChars.CrLf)
                    Next
                    l_StreamWriter_File.WriteLine(l_stringbuilder)
                    l_StreamWriter_File.Close()
                    CreateCSVFile = True
                End If
            Else
                CreateCSVFile = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    'by Aparna
    Public Function CreateFolders(ByVal p_stringFolderPath As String) As Boolean
        Try
            Dim strProjectName As String
            If Not Directory.Exists(p_stringFolderPath) Then
                Directory.CreateDirectory(p_stringFolderPath)
                CreateFolders = True
            End If
        Catch
            CreateFolders = False
        End Try
    End Function
    Public Sub FilesTracking(ByVal pTrackingId As Integer, ByVal pIdxFileCreated As Boolean, ByVal pPdfFilecreated As Boolean, ByVal pIdxFileName As String, ByVal pPdfFileName As String, ByVal pIdxFileinitiated As Boolean, ByVal pIdxfileCopied As Boolean, ByVal pIdxFileDeleted As Boolean, ByVal pPdfFileinitiated As Boolean, ByVal pPdffileCopied As Boolean, ByVal pPdfFileDeleted As Boolean, ByVal pFileType As Char, ByVal pErrorMsg As String)
        Try
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateYMCAOutputFileIDMTrackingLogs(pTrackingId, pIdxFileCreated, pPdfFilecreated, pIdxFileName, pIdxFileinitiated, pIdxfileCopied, pIdxFileDeleted, pPdfFileName, pPdfFileinitiated, pPdffileCopied, pPdfFileDeleted, pFileType, pErrorMsg)
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_FilesTracking()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            Throw
        End Try
    End Sub
    Private Sub UpdateTrackingStatus(ByVal P_str_ExceptionMsg As String)
        Dim strSourceFolder As String
        Dim strSourceFile As String
        Dim strDestFolder As String
        Dim strDestFile As String
        Dim dtListFile As New DataTable
        Dim i As Integer = 0
        Try
            If Not Session("FTFileList") Is Nothing Then
                dtListFile = Session("FTFileList")
                If Not dtListFile.Rows.Count = 0 Then
                    For i = 0 To dtListFile.Rows.Count - 1
                        strSourceFolder = CType(dtListFile.Rows(i)(0), String)
                        strSourceFile = CType(dtListFile.Rows(i)(1), String)
                        strDestFolder = CType(dtListFile.Rows(i)(2), String)
                        strDestFile = CType(dtListFile.Rows(i)(3), String)
                        FileName = strSourceFile.Substring(strSourceFile.LastIndexOf("\") + 1)
                        If strSourceFile.Substring(strSourceFile.Length - 3, 3) = "idx" Then
                            Me.FilesTracking(0, True, True, FileName, "", True, False, False, False, False, False, "I", P_str_ExceptionMsg)
                        ElseIf strSourceFile.Substring(strSourceFile.Length - 3, 3) = "pdf" Then
                            Me.FilesTracking(0, True, True, "", FileName, False, False, False, True, False, False, "P", P_str_ExceptionMsg)
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function ArrErrorDataList(ByVal strBatchId As String, ByVal strModule As String) As String
        Dim objStrHtml As New StringBuilder()
        Dim strHtml As String
        Dim obj As New CopyFilestoFileServer
        Dim gv As New GridView
        Dim dstemp As DataSet
        Dim strRetvalue As String = String.Empty
        Try
            dstemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule, True) 'AA: 10.21.2015 Changed to use sql authentication because it cant use windows authentication
        If HelperFunctions.isNonEmpty(dstemp) Then
            If dstemp.Tables.Contains("ArrErrorDataList") Then
                Dim objStrWriter As New StringWriter(objStrHtml)
                Dim objHtmlTextWriter As New HtmlTextWriter(objStrWriter)
                gv.DataSource = dstemp.Tables("ArrErrorDataList")
                gv.CssClass = "DataGrid_Grid"
                gv.RowStyle.CssClass = "DataGrid_NormalStyle"
                gv.SelectedRowStyle.CssClass = "DataGrid_SelectedStyle"
                gv.HeaderStyle.CssClass = "DataGrid_HeaderStyle"
                gv.Width = Unit.Percentage(99)
                gv.DataBind()
                gv.RenderControl(objHtmlTextWriter)
                strHtml = WebUtility.HtmlEncode(objStrHtml.ToString)
                strRetvalue = WebUtility.HtmlDecode(strHtml)
            Else
                strRetvalue = "ArrErrorDataList table not found in database."
            End If
        Else
            strRetvalue = "Records not found for BatchId:" + strBatchId
        End If
            Return strRetvalue
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_ArrErrorDataList()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function CopytoFileServer(ByVal strOR As String, ByVal strDEL As String, ByVal strBatchId As String, ByVal strModule As String) As RetValues
        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: CopytoFileServer process is started")
        Dim dtListFile As New DataTable
        Dim dstemp As New DataSet
        Dim objCopyFilesToFileServer As New CopyFilestoFileServer
        Dim strOverwrite As String = String.Empty
        Dim strDelete As String = String.Empty
        Dim bOverWrite As Boolean
        Dim bDelete As Boolean
        Dim objRetValues As New RetValues
        Dim strArchiveFolderDest As String = String.Empty
        Dim dtTemp As New DataTable
        Dim iCounter As Integer = 0
        Dim ArrErrorDataList = New List(Of ExceptionLog)

        'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
        Dim pIdxFileCreated As Boolean = False
        Dim pPdfFilecreated As Boolean = False
        Dim pIdxFileName As String = String.Empty
        Dim pPdfFileName As String = String.Empty
        Dim pIdxFileinitiated As Boolean = False
        Dim pIdxfileCopied As Boolean = False
        Dim pIdxFileDeleted As Boolean = False
        Dim pPdfFileinitiated As Boolean = False
        Dim pPdffileCopied As Boolean = False
        Dim pPdfFileDeleted As Boolean = False
        Dim pFileType As String = String.Empty
        Dim pErrorMsg As String = String.Empty
        'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Get XMl data to copy generated IDX and PDF files to IDM server")
            dstemp = YMCARET.YmcaBusinessObject.MRDBO.GetAtsTemp(strBatchId, strModule, True) 'AA: 10.21.2015 Changed to use sql authentication because it cant use windows authentication
            If String.IsNullOrEmpty(strOR) Then
                strOverwrite = ""
            Else
                strOverwrite = strOR
            End If
            strOverwrite = strOverwrite.Trim()

            If strOverwrite = "1" Then
                bOverWrite = True
            Else
                bOverWrite = False
            End If
            If String.IsNullOrEmpty(strDEL) Then
                strDelete = ""
            Else
                strDelete = strDEL
            End If
            strDelete = strDelete.Trim()
            If strDelete = "0" Then
                bDelete = False
            Else
                bDelete = True
            End If

            If HelperFunctions.isNonEmpty(dstemp) Then
                If (dstemp.Tables.Contains("dtFileList")) Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Get source and destination from XMl data to copy generated IDX and PDF files to IDM server")
                    dtListFile = dstemp.Tables("dtFileList")

                    If HelperFunctions.isNonEmpty(dtListFile) Then
                        Dim strSourceFolder As String = String.Empty
                        Dim strSourceFile As String = String.Empty
                        Dim strDestFolder As String = String.Empty
                        Dim strDestFile As String = String.Empty
                        For iCount As Integer = 0 To dtListFile.Rows.Count - 1

                            Dim dr As DataRow = dtListFile.Rows(iCount)

                            strSourceFolder = dr("SourceFolder").ToString()
                            strSourceFile = dr("SourceFile").ToString()
                            strDestFolder = dr("DestFolder").ToString()
                            strDestFile = dr("DestFile").ToString()

                            If String.IsNullOrEmpty(strSourceFile) Then
                                Exit For
                            End If

                            objCopyFilesToFileServer.FileName = strSourceFile.Substring(strSourceFile.LastIndexOf("\") + 1)

                            'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                            'If strSourceFile.Substring(strSourceFile.Length - 3, 3) = "idx" Then
                            '    objCopyFilesToFileServer.FilesTracking(0, True, True, objCopyFilesToFileServer.FileName, "", True, False, False, False, False, False, "I", "")
                            'ElseIf strSourceFile.Substring(strSourceFile.Length - 3, 3) = "pdf" Then
                            '    objCopyFilesToFileServer.FilesTracking(0, True, True, "", objCopyFilesToFileServer.FileName, False, False, False, True, False, False, "P", "")
                            'End If
                            'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Check if destination folder exists or not")
                            If Directory.Exists(strDestFolder) Then
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Finish: Check if destination folder exists or not")
                                Try
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Finish: Get source and destination from XMl data to copy generated IDX and PDF files to IDM server")
                                    'Start:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)   
                                    'Copy IDX and PDF generated files to IDM server and update records in IDMtracking table.
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Copy IDX and PDF files from source to IDM server")
                                    objCopyFilesToFileServer.NewCopyFile(strSourceFolder, strSourceFile, strDestFolder, strDestFile, bOverWrite, bDelete, pIdxFileName, pPdfFileName, pIdxFileinitiated, pIdxfileCopied, pIdxFileDeleted, pPdfFileinitiated, pPdffileCopied, pPdfFileDeleted, pFileType, pErrorMsg,
                                                                         objRetValues.strError, objRetValues.strStatus, ArrErrorDataList)
                                    'End:Dinesh Kanojia   09-Dec-2014     BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                                    If strSourceFile.Substring(strSourceFile.Length - 3, 3) = "idx" Then
                                        objRetValues.iIdxCopied += 1
                                    ElseIf strSourceFile.Substring(strSourceFile.Length - 3, 3) = "pdf" Then
                                        objRetValues.iPdfCopied += 1
                                    End If
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Finish: Copy IDX and PDF files from source to IDM server")
                                Catch ex As Exception
                                    HelperFunctions.LogException("CopyFilestoFileServer_CopytoFileServer()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Exception raise by copying files to server")
                                    objRetValues.strError += "Error occured while copying the file, please verify." + ex.Message
                                    ArrErrorDataList.Add(New ExceptionLog("Final Exception", "Copy files to IDM server.", "Error occured while copying the file, please verify. " + ex.Message))
                                    objRetValues.strStatus = "error"
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Finish: Exception raise by copying files to server")
                                    Throw
                                End Try

                            Else
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Check if destination folder does not exists")
                                Dim strError As String
                                strError = "Directory does not exist for output file."
                                objRetValues.strError = strError
                                objRetValues.strStatus = "error"
                                ArrErrorDataList.Add(New ExceptionLog("Copy Exception", "Copy files to IDM server.", strError))
                                objCopyFilesToFileServer.GenerateCSVFile(strError, iCounter)
                                objCopyFilesToFileServer.UpdateTrackingStatus(strError)
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Start: Check if destination folder does not exists")
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Finish: Get XMl data to copy generated IDX and PDF files to IDM server")
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_CopytoFileServer()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
            objCopyFilesToFileServer.GenerateCSVFile(ex.Message, iCounter)
            objRetValues.strError += "Error occured while copying the file, please verify." + ex.Message
            objRetValues.strStatus = "error"
            ArrErrorDataList.Add(New ExceptionLog("Final Exception", "Copy files to IDM server.", "Error occured while copying the file, please verify. " + ex.Message))
        Finally
            Dim dtArrErrorDataList As New DataTable("ArrErrorDataList")
            dtArrErrorDataList.Columns.Add("FundNo")
            dtArrErrorDataList.Columns.Add("Errors")
            dtArrErrorDataList.Columns.Add("Description")
            Dim dr As DataRow
            For Each exlog As ExceptionLog In ArrErrorDataList
                dr = dtArrErrorDataList.NewRow()
                dr("FundNo") = exlog.FundNo
                dr("Errors") = exlog.Errors
                dr("Description") = exlog.Decription
                dtArrErrorDataList.Rows.Add(dr)
            Next

            If dstemp.Tables.Contains("ArrErrorDataList") Then
                dstemp.Tables("ArrErrorDataList").Merge(dtArrErrorDataList)
            ElseIf Not dstemp.Tables.Contains("ArrErrorDataList") Then
                dtArrErrorDataList.TableName = "ArrErrorDataList"
                dstemp.Tables.Add(dtArrErrorDataList)
            End If

        End Try

        Try

            YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(strBatchId, strModule, dstemp, True) 'AA: 10.21.2015 Changed to use sql authentication because it cant use windows authentication
            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopytoFileServer Method", "Finish: CopytoFileServer process completed")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            Return objRetValues
        Catch ex As Exception
            HelperFunctions.LogException("CopyFilestoFileServer_CopytoFileServer()", ex)  'SR | 2015.10.20 | Added error log to track any error occured while Processing
        End Try
    End Function
End Class

Public Class RetValues
    Public iPdfCopied As Integer
    Public iIdxCopied As Integer
    Public strStatus As String
    Public strError As String
End Class
