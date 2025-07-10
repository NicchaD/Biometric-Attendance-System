'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		            :	YMCA_YRS
' FileName			            :	CopyFilesUtilityForm.aspx.vb
' Author Name		            :	Aparna Samala
' Employee ID		            :	34773
' Email				            :	aparna.samala@icici-infotech.com    
' Contact No		            :	8609
' Creation Time		            :	14th May 2007
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	To Copy all the files to the respective folders
' Cache-Session   
'*******************************************************************************
'Modification History
'****************************************************************************************
'Modified By                    Date                Desription
'****************************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar Singh           29/June/2010        Migration related changes.
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Anudeep A                      2016.27.06          BT-1156:Add a precautionary measure while IDM creation
'Pooja K                        2019.28.02          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'****************************************************************************************
Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Data.SqlClient
Public Class CopyFilesUtilityForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("CopyFilesUtilityForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents MessagePlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelIDM As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxIDM As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelDelinquency As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxDELINQ As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelACH As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxACH As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelPayroll As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonStartProcess As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxPayroll As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Global Declarations"
    Dim g_String_Exception_Message As String
    '  Dim IDM As New IDMforAll
    Dim m_dtFileList As New DataTable
#End Region
#Region "Properpties"
    Public Property SetdtFileList() As DataTable
        Get
            If Not m_dtFileList Is Nothing Then
                Return (DirectCast(m_dtFileList, DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            m_dtFileList = Value
        End Set
    End Property
    Private Property SessionPersonID() As String
        Get
            If Not (Session("PersonID")) Is Nothing Then
                Return (CType(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property
    Public Property OutputFileType() As String
        Get
            If Not (Session("OutputFileType")) Is Nothing Then
                Return (DirectCast(Session("OutputFileType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("OutputFileType") = Value
        End Set
    End Property

    Public Property SourceFolderPath() As String
        Get
            If Not (Session("SourceFolderPath")) Is Nothing Then
                Return (DirectCast(Session("SourceFolderPath"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("SourceFolderPath") = Value
        End Set
    End Property

    Public Property DestFolderPath() As String
        Get
            If Not (Session("DestFolderPath")) Is Nothing Then
                Return (DirectCast(Session("DestFolderPath"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("DestFolderPath") = Value
        End Set
    End Property
    Public Property ArchiveFolderPath() As String
        Get
            If Not (Session("ArchiveFolderPath")) Is Nothing Then
                Return (DirectCast(Session("ArchiveFolderPath"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ArchiveFolderPath") = Value
        End Set
    End Property

    Public Property FileExtension() As String
        Get
            If Not (Session("FileExtension")) Is Nothing Then
                Return (DirectCast(Session("FileExtension"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FileExtension") = Value
        End Set
    End Property
#End Region
    
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_stringMessage As String
        Try
            Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Me.Menu1.DataBind()
            If Not IsPostBack Then
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                End If
                Session("Message") = Nothing
                'CHeck whether Files exist in the folders to  copy
                SetCheckBoxStatus()
            End If
            If Not Session("ErrorMessage") Is Nothing Then
                If Session("ErrorMessage") = "Files Copied Successfully" Then
                    l_stringMessage = Session("ErrorMessage").ToString.Trim()
                    MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringMessage, MessageBoxButtons.OK)
                    '  SetCheckBoxStatus()
                Else
                    l_stringMessage = Session("ErrorMessage").ToString.Trim()
                    Response.Redirect("StatusPageForm.aspx?CopyFile=1 &Message=" + Server.UrlEncode(l_stringMessage.Trim.ToString()), False)

                End If
                Session("ErrorMessage") = Nothing
                Session("Message") = Nothing
                'CHeck whether Files exist in the folders to  copy

            End If
            CheckReadOnlyMode() 'PK| 02/28/2019 | YRS-AT-4248 | Check security method called here
            If Request.Form("Yes") = "Yes" Then
                Me.CopyFiles()
            ElseIf Request.Form("No") = "No" Then
                'Response.Redirect("MainWebForm.aspx", False)
            ElseIf Request.Form("Ok") = "OK" Then
                SetCheckBoxStatus()
            End If

        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonStartProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStartProcess.Click
        Try

            ' Dim l_stringDelinqPath As String
            Dim l_stringfolderpath As String
            Dim l_String_SourceFolder As String
            Dim l_String_SourceFile As String
            Dim l_StringDestFolder As String
            Dim l_String_DestFile As String
            Dim l_bool As Boolean
            Dim l_stringFileExtension As String
            Dim l_boolPayroll As Boolean = False
            Dim l_stringOutputfiletype As String = String.Empty
            Dim l_stringArchivefolderpath As String = String.Empty
            Dim l_stringerrormsg As String = String.Empty
            Dim l_stringErrMessage As String = String.Empty
            Dim string_Message As String

            Dim popupScript1 As String = "<script language='javascript'> "

            Dim arrFiles As FileInfo()

            If Me.CheckBoxACH.Checked = False And Me.CheckBoxDELINQ.Checked = False And Me.CheckBoxIDM.Checked = False And Me.CheckBoxPayroll.Checked = False Then
                MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "Please select an item to proceed", MessageBoxButtons.OK)
                Exit Sub
            End If
            'to find whether to copy the files or not
            string_Message = "Are you sure you want to copy files?"
            MessageBox.Show(MessagePlaceHolder, "YMCA-YRS", string_Message.Trim(), MessageBoxButtons.YesNo)
            Exit Sub

            'CHECK WHETHER ARCHIVE FOLDER EXISTS
            l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE")
            If l_stringArchivefolderpath <> Nothing Then
                If Me.CheckFolderExists(l_stringArchivefolderpath) = False Then
                    l_stringerrormsg = "Archive Folder doesn't exist.Please create it to continue."
                    MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringerrormsg, MessageBoxButtons.OK)
                    Exit Sub
                End If
            End If

            ''Concatenate the error Message.
            'If Not Session("Message") Is Nothing Then
            '    l_stringErrMessage = CType(Session("Message"), String)
            'End If

            ''create the Datatable -Filelist
            'If Me.DatatableFileList(False) Then
            'Else
            '    Throw New Exception("Unable to Process, Could not create dependent table")
            'End If

            ''Showing the progress bar..to indicate the running of the process
            'popupScript1 += "var dots = 0;var dotmax = 99;var output = 1;"
            'popupScript1 += " function ShowWait()"
            'popupScript1 += "{var  width; if (output != 1) { width = new String(prgTD.style.width); output = parseInt(width.substring(0,width.length-1));}"
            'popupScript1 += "dots++; if(dots>=dotmax) dots=1; output++; width = output + '%'; prgTD.style.width = output;"
            'popupScript1 += "prgTD.style.backgroundColor = 'blue'; prgSt.innerText = 'Copying of files in progess: ';   if (output == 450) output = 1; }"
            'popupScript1 += " function StartShowWait(){ window.setInterval('ShowWait()',100);}"
            'popupScript1 += " function HideWait(){window.clearInterval();} "
            'popupScript1 += " StartShowWait(); </script>"
            'Page.RegisterStartupScript("RunStatusBar", popupScript1)

            ''Copy ACH Export files 
            'If Me.CheckBoxACH.Checked = True Then
            '    'Copy ACH Export files 
            '    Me.OutputFileType = "ACHDEB"
            '    Me.SourceFolderPath = ConfigurationSettings.AppSettings("ACH") + "\\" + "EXPORT"
            '    Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "ACH" + "\\" + "EXPORT"
            '    Me.FileExtension = "*.csv"
            '    l_stringerrormsg = Me.CopyFiles(False)
            '    If Not l_stringerrormsg = String.Empty Then
            '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '        l_stringerrormsg = ""
            '    End If
            'End If


            ''Copy Delinquency Files

            'If Me.CheckBoxDELINQ.Checked = True Then
            '    Me.OutputFileType = "DLTTR"
            '    Me.SourceFolderPath = ConfigurationSettings.AppSettings("DELINQ") + "\\" + "CSV"
            '    Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "DELINQ" + "\\" + "CSV"
            '    Me.FileExtension = "*.csv"
            '    l_stringerrormsg = Me.CopyFiles(False)
            '    If Not l_stringerrormsg = String.Empty Then
            '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '        l_stringerrormsg = ""
            '    End If
            'End If

            'If Me.CheckBoxIDM.Checked = True Then
            '    Dim l_stringPDFfolderpath As String
            '    Dim l_stringIDXfolderpath As String
            '    Dim l_stringPDFFileExtension As String
            '    Dim l_stringIDXFileExtension As String
            '    Dim l_boolparticipant As Boolean

            '    'YMCAFILES
            '    l_stringPDFfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "YMCA" + "\\" + "PDF"
            '    l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "IDM" + "\\" + "YMCA" + "\\"
            '    l_stringIDXfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "YMCA" + "\\" + "IDX"
            '    l_boolparticipant = False
            '    l_stringPDFFileExtension = "*.pdf"
            '    l_stringIDXFileExtension = "*.idx"

            '    If Me.CheckFolderExists(l_stringPDFfolderpath) Then
            '        Dim myDirInfo As New DirectoryInfo(l_stringPDFfolderpath)
            '        arrFiles = myDirInfo.GetFiles(l_stringPDFFileExtension)
            '        If arrFiles.Length > 0 Then
            '            l_stringerrormsg = Me.RetrieveIDMFilesFromFolder(l_stringPDFfolderpath, l_stringPDFFileExtension, l_stringIDXfolderpath, l_stringIDXFileExtension, l_boolparticipant, l_stringArchivefolderpath)
            '            If Not l_stringerrormsg = String.Empty Then
            '                l_stringErrMessage += l_stringerrormsg + vbCrLf
            '                l_stringerrormsg = ""
            '            End If
            '            'Else
            '            '    l_stringerrormsg = "No files exist to copy from the path :" + l_stringPDFfolderpath.ToString().Trim()
            '            '    If Not l_stringerrormsg = String.Empty Then
            '            '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '            '        l_stringerrormsg = ""
            '            '    End If
            '        End If

            '        'Else
            '        '    l_stringerrormsg = "Following Folder doesnot exist : " + l_stringPDFfolderpath
            '        '    If Not l_stringerrormsg = String.Empty Then
            '        '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '        '        l_stringerrormsg = ""
            '        '    End If

            '    End If


            '    'PARTICIPANT FILES
            '    l_stringPDFfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "PARTICIPANT" + "\\" + "PDF"
            '    l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "IDM" + "\\" + "PARTICIPANT" + "\\"
            '    l_stringIDXfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "PARTICIPANT" + "\\" + "IDX"
            '    l_stringPDFFileExtension = "*.pdf"
            '    l_stringIDXFileExtension = "*.idx"
            '    l_boolparticipant = True
            '    If Me.CheckFolderExists(l_stringPDFfolderpath) Then
            '        Dim myDirInfo As New DirectoryInfo(l_stringPDFfolderpath)
            '        arrFiles = myDirInfo.GetFiles(l_stringPDFFileExtension)
            '        If arrFiles.Length > 0 Then

            '            l_stringerrormsg = Me.RetrieveIDMFilesFromFolder(l_stringPDFfolderpath, l_stringPDFFileExtension, l_stringIDXfolderpath, l_stringIDXFileExtension, l_boolparticipant, l_stringArchivefolderpath)
            '            If Not l_stringerrormsg = String.Empty Then
            '                l_stringErrMessage += l_stringerrormsg + vbCrLf
            '                l_stringerrormsg = ""
            '            End If
            '            'Else
            '            '    l_stringerrormsg = "No files exist to copy from the path :" + l_stringPDFfolderpath.ToString().Trim()
            '            '    If Not l_stringerrormsg = String.Empty Then
            '            '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '            '        l_stringerrormsg = ""
            '            '    End If
            '        End If
            '        'Else
            '        '    l_stringerrormsg = "Following Folder doesnot exist :" + l_stringPDFfolderpath
            '        '    If Not l_stringerrormsg = String.Empty Then
            '        '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '        '        l_stringerrormsg = ""
            '        '    End If
            '    End If
            '    '  Me.RetrieveIDMFilesFromFolder(l_stringPDFfolderpath, l_stringPDFFileExtension, l_stringIDXfolderpath, l_stringIDXFileExtension, l_boolparticipant, l_stringArchivefolderpath)

            'End If

            'If Me.CheckBoxPayroll.Checked = True Then
            '    l_stringerrormsg = Me.CopyPayrollFiles()
            '    If Not l_stringerrormsg = String.Empty Then
            '        l_stringErrMessage += l_stringerrormsg + vbCrLf
            '        l_stringerrormsg = ""
            '    End If
            'End If

            'Session("Message") = l_stringErrMessage

            ''  If l_stringErrMessage = String.Empty Then
            'If Me.SetdtFileList.Rows.Count > 0 Then
            '    Try
            '        Session("FromPage") = "CopyUtility"
            '        ' Call the calling of the ASPX to copy the file.
            '        Dim popupScript As String = "<script language='javascript'>" & _
            '        "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
            '        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
            '        "</script>"
            '        If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
            '            Page.RegisterStartupScript("PopupScript3", popupScript)
            '        End If
            '        'MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "Files Copied successfully", MessageBoxButtons.OK)
            '    Catch ex As Exception
            '        MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
            '        Exit Sub
            '    End Try
            'End If
            ''Else
            ''Session("ErrorMessage") = l_stringErrMessage
            '''MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringErrMessage, MessageBoxButtons.OK)
            ''Response.Redirect("StatusPageForm.aspx?CopyFile=1 &Message=" + Server.UrlEncode(l_stringErrMessage.Trim.ToString()), False)

            ''End If


        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub
    Private Function RetrieveFilesFromFolder(ByVal p_stringfolderpath As String, ByVal p_stringfileextension As String, ByVal p_stringdestPath As String, ByVal p_ArchiveFolderpath As String) As String
        Dim I As Integer
        Dim l_stringerrormsg As String = String.Empty
        Dim l_String_SourceFolder As String
        Dim l_String_SourceFile As String
        Dim l_String_DestFolder As String
        Dim l_String_DestFile As String
        Dim l_stringArchivefile As String = String.Empty
        Dim l_StringArchiveFolderpath As String
        Dim arrFiles() As FileInfo
        Dim l_bool As Boolean
      
        Dim l_datasetMetaOutputfiletypes As New DataSet
        Dim l_datarow As DataRow()

        Try

            Dim myDirInfo As New DirectoryInfo(p_stringfolderpath)
            arrFiles = myDirInfo.GetFiles(p_stringfileextension)
            'Temporary folder path
            l_String_SourceFolder = p_stringfolderpath.Trim()
            'Get the destination folder path
            l_String_DestFolder = p_stringdestPath.Trim()
            'get the archive folder path -to dump the files incase error occurs while copying files to destination
            l_StringArchiveFolderpath = p_ArchiveFolderpath.Trim()


            For I = LBound(arrFiles) To UBound(arrFiles)
                'source folder
                If Right(l_String_SourceFolder, 1) = "\" Then
                    l_String_SourceFile = l_String_SourceFolder.Trim() + arrFiles(I).Name.ToString.Trim()
                Else
                    l_String_SourceFile = l_String_SourceFolder.Trim() + "\" + arrFiles(I).Name.ToString.Trim()
                End If
                'destination folder
                If Right(l_String_DestFolder, 1) = "\" Then
                    l_String_DestFile = l_String_DestFolder.Trim() + arrFiles(I).Name.ToString.Trim()
                Else
                    l_String_DestFile = l_String_DestFolder.Trim() + "\" + arrFiles(I).Name.ToString.Trim()
                End If

                'l_StringArchiveFolderpath
                If Right(l_StringArchiveFolderpath, 1) = "\" Then
                    l_stringArchivefile = l_StringArchiveFolderpath.Trim() + arrFiles(I).Name.ToString.Trim()
                Else
                    l_stringArchivefile = l_StringArchiveFolderpath.Trim() + "\" + arrFiles(I).Name.ToString.Trim()
                End If

                If Not l_String_DestFolder.Trim Is Nothing Then
                    l_bool = Me.AddFileListRow(l_String_SourceFolder, l_String_SourceFile, l_String_DestFolder, l_String_DestFile, l_StringArchiveFolderpath, l_stringArchivefile)
                    If l_bool = False Then
                        l_stringerrormsg = "Error occured while creating datatable"
                    End If
                End If
            Next I
            Session("FTFileList") = Me.SetdtFileList
            RetrieveFilesFromFolder = l_stringerrormsg
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function DatatableFileList(ByVal l_bool_Participant As Boolean) As Boolean
        Try
            m_dtFileList.Columns.Add("SourceFolder", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("SourceFile", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("DestFolder", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("DestFile", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("ArchiveFolder", System.Type.GetType("System.String"))
            m_dtFileList.Columns.Add("ArchiveFile", System.Type.GetType("System.String"))
            DatatableFileList = True
        Catch ex As Exception
            DatatableFileList = False
        End Try
    End Function
    '    Public Function FolderExistsUsingDir(ByVal strFolder As String) As Boolean
    '        On Error Resume Next
    '        FolderExistsUsingDir = (Dir$(strFolder, vbDirectory) <> vbNullString)
    '    End Function
    '    Public Function FolderExistsUsingAttrib(ByVal sFolder As String) As Boolean
    '        Dim lRetVal As Long
    '        On Error GoTo FileDoesntExist
    '        lRetVal = GetAttr(sFolder)
    '        FolderExistsUsingAttrib = True
    '        Exit Function

    'FileDoesntExist:
    '        FolderExistsUsingAttrib = False

    '    End Function
    Public Function CopyFiles()
        Dim l_stringfolderpath As String
        Dim l_String_SourceFolder As String
        Dim l_String_SourceFile As String
        Dim l_StringDestFolder As String
        Dim l_String_DestFile As String
        Dim l_bool As Boolean
        Dim l_stringFileExtension As String
        Dim l_boolPayroll As Boolean = False
        Dim l_stringOutputfiletype As String = String.Empty
        Dim l_stringArchivefolderpath As String = String.Empty
        Dim l_stringerrormsg As String = String.Empty
        Dim l_stringErrMessage As String = String.Empty

        Dim popupScript1 As String = "<script language='javascript'> "

        Dim arrFiles As FileInfo()
        Try
            'CHECK WHETHER ARCHIVE FOLDER EXISTS
            l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE")
            If l_stringArchivefolderpath <> Nothing Then
                If Me.CheckFolderExists(l_stringArchivefolderpath) = False Then
                    l_stringerrormsg = "Archive Folder doesn't exist.Please create it to continue"
                    MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringerrormsg, MessageBoxButtons.OK)
                    Exit Function
                End If
            End If

            'Concatenate the error Message.
            If Not Session("Message") Is Nothing Then
                l_stringErrMessage = CType(Session("Message"), String)
            End If

            'create the Datatable -Filelist
            If Me.DatatableFileList(False) Then
            Else
                Throw New Exception("Unable to Process, Could not create dependent table")
            End If

            'Showing the progress bar..to indicate the running of the process
            popupScript1 += "var dots = 0;var dotmax = 99;var output = 1;"
            popupScript1 += " function ShowWait()"
            popupScript1 += "{var  width; if (output != 1) { width = new String(prgTD.style.width); output = parseInt(width.substring(0,width.length-1));}"
            popupScript1 += "dots++; if(dots>=dotmax) dots=1; output++; width = output + '%'; prgTD.style.width = output;"
            popupScript1 += "prgTD.style.backgroundColor = 'blue'; prgSt.innerText = 'Copying of files in progess: ';   if (output == 450) output = 1; }"
            popupScript1 += " function StartShowWait(){ window.setInterval('ShowWait()',100);}"
            popupScript1 += " function HideWait(){window.clearInterval();} "
            popupScript1 += " StartShowWait(); </script>"
            Page.RegisterStartupScript("RunStatusBar", popupScript1)

            'Copy ACH Export files 
            If Me.CheckBoxACH.Checked = True Then
                'Copy ACH Export files 
                Me.OutputFileType = "ACHDEB"
                Me.SourceFolderPath = ConfigurationSettings.AppSettings("ACH") + "\\" + "EXPORT"
                Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "ACH" + "\\" + "EXPORT"
                Me.FileExtension = "*.csv"
                l_stringerrormsg = Me.CopyFiles(False)
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If


            'Copy Delinquency Files

            If Me.CheckBoxDELINQ.Checked = True Then
                Me.OutputFileType = "DLTTR"
                Me.SourceFolderPath = ConfigurationSettings.AppSettings("DELINQ") + "\\" + "CSV"
                Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "DELINQ" + "\\" + "CSV"
                Me.FileExtension = "*.csv"
                l_stringerrormsg = Me.CopyFiles(False)
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If

            If Me.CheckBoxIDM.Checked = True Then
                Dim l_stringPDFfolderpath As String
                Dim l_stringIDXfolderpath As String
                Dim l_stringPDFFileExtension As String
                Dim l_stringIDXFileExtension As String
                Dim l_boolparticipant As Boolean

                'YMCAFILES
                l_stringPDFfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "YMCA" + "\\" + "PDF"
                l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "IDM" + "\\" + "YMCA" + "\\"
                l_stringIDXfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "YMCA" + "\\" + "IDX"
                l_boolparticipant = False
                l_stringPDFFileExtension = "*.pdf"
                l_stringIDXFileExtension = "*.idx"

                If Me.CheckFolderExists(l_stringPDFfolderpath) Then
                    Dim myDirInfo As New DirectoryInfo(l_stringPDFfolderpath)
                    arrFiles = myDirInfo.GetFiles(l_stringPDFFileExtension)
                    If arrFiles.Length > 0 Then
                        l_stringerrormsg = Me.RetrieveIDMFilesFromFolder(l_stringPDFfolderpath, l_stringPDFFileExtension, l_stringIDXfolderpath, l_stringIDXFileExtension, l_boolparticipant, l_stringArchivefolderpath)
                        If Not l_stringerrormsg = String.Empty Then
                            l_stringErrMessage += l_stringerrormsg + vbCrLf
                            l_stringerrormsg = ""
                        End If
                        'Else
                        '    l_stringerrormsg = "No files exist to copy from the path :" + l_stringPDFfolderpath.ToString().Trim()
                        '    If Not l_stringerrormsg = String.Empty Then
                        '        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        '        l_stringerrormsg = ""
                        '    End If
                    End If

                    'Else
                    '    l_stringerrormsg = "Following Folder doesnot exist : " + l_stringPDFfolderpath
                    '    If Not l_stringerrormsg = String.Empty Then
                    '        l_stringErrMessage += l_stringerrormsg + vbCrLf
                    '        l_stringerrormsg = ""
                    '    End If

                End If


                'PARTICIPANT FILES
                l_stringPDFfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "PARTICIPANT" + "\\" + "PDF"
                l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "IDM" + "\\" + "PARTICIPANT" + "\\"
                l_stringIDXfolderpath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "PARTICIPANT" + "\\" + "IDX"
                l_stringPDFFileExtension = "*.pdf"
                l_stringIDXFileExtension = "*.idx"
                l_boolparticipant = True
                If Me.CheckFolderExists(l_stringPDFfolderpath) Then
                    Dim myDirInfo As New DirectoryInfo(l_stringPDFfolderpath)
                    arrFiles = myDirInfo.GetFiles(l_stringPDFFileExtension)
                    If arrFiles.Length > 0 Then

                        l_stringerrormsg = Me.RetrieveIDMFilesFromFolder(l_stringPDFfolderpath, l_stringPDFFileExtension, l_stringIDXfolderpath, l_stringIDXFileExtension, l_boolparticipant, l_stringArchivefolderpath)
                        If Not l_stringerrormsg = String.Empty Then
                            l_stringErrMessage += l_stringerrormsg + vbCrLf
                            l_stringerrormsg = ""
                        End If
                        'Else
                        '    l_stringerrormsg = "No files exist to copy from the path :" + l_stringPDFfolderpath.ToString().Trim()
                        '    If Not l_stringerrormsg = String.Empty Then
                        '        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        '        l_stringerrormsg = ""
                        '    End If
                    End If
                    'Else
                    '    l_stringerrormsg = "Following Folder doesnot exist :" + l_stringPDFfolderpath
                    '    If Not l_stringerrormsg = String.Empty Then
                    '        l_stringErrMessage += l_stringerrormsg + vbCrLf
                    '        l_stringerrormsg = ""
                    '    End If
                End If
                '  Me.RetrieveIDMFilesFromFolder(l_stringPDFfolderpath, l_stringPDFFileExtension, l_stringIDXfolderpath, l_stringIDXFileExtension, l_boolparticipant, l_stringArchivefolderpath)

            End If

            If Me.CheckBoxPayroll.Checked = True Then
                l_stringerrormsg = Me.CopyPayrollFiles()
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If

            Session("Message") = l_stringErrMessage

            '  If l_stringErrMessage = String.Empty Then
            If Me.SetdtFileList.Rows.Count > 0 Then
                Try
                    Session("FromPage") = "CopyUtility"
                    ' Call the calling of the ASPX to copy the file.
                    Dim popupScript As String = "<script language='javascript'>" & _
                    "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                    "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
                        Page.RegisterStartupScript("PopupScript3", popupScript)
                    End If
                    'MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "Files Copied successfully", MessageBoxButtons.OK)
                Catch ex As Exception
                    MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "Error while transfering documents", MessageBoxButtons.OK)
                    Exit Function
                End Try
            End If
        Catch
            Throw
        End Try
    End Function

    Public Function CheckFolderExists(ByVal stringFolderName As String) As Boolean
        'create instance of Filesystemobject
        Try
            Dim FileSystemObject
            FileSystemObject = Server.CreateObject("Scripting.FileSystemObject")
            If (FileSystemObject.FolderExists(stringFolderName)) Then
                CheckFolderExists = True
            Else
                CheckFolderExists = False
            End If
        Catch
            Throw
        End Try
    End Function


    Private Function RetrieveIDMFilesFromFolder(ByVal p_stringPDFfolderpath As String, ByVal p_stringPDFfileextension As String, ByVal p_stringIDXFolderPath As String, ByVal p_stringIDXfileextension As String, ByVal p_boolparticipant As Boolean, ByVal p_stringArchivefolder As String) As String
        Dim I As Integer
        Dim l_String_PDF_SourceFolder As String
        Dim l_String_IDX_SourceFolder As String
        Dim l_String_PDF_DestFolder As String
        Dim l_String_IDX_DestFolder As String

        Dim l_string_Archivefolder As String
        Dim l_string_ArchiveFile As String = String.Empty

        Dim arrPDFFiles() As FileInfo
        Dim arrIDXFiles() As FileInfo

        Dim l_bool As Boolean
        Dim l_stringErrormsg As String
        Dim l_String_SourceFile As String
        Dim l_String_DestFolder As String
        Dim l_String_DestFile As String



        Dim l_stringIDXFilename As String = String.Empty
        Dim l_stringPDFFilename As String
        Dim l_intIndex As Integer
        Dim l_index As Integer

        Dim l_stringFilename As String
        Try

            Dim PDFDirInfo As New DirectoryInfo(p_stringPDFfolderpath)
            arrPDFFiles = PDFDirInfo.GetFiles(p_stringPDFfileextension)

            Dim IDXDirInfo As New DirectoryInfo(p_stringIDXFolderPath)
            arrIDXFiles = IDXDirInfo.GetFiles(p_stringIDXfileextension)

            'if the pdf or idx files dont have the corresponding files then they are sent to error 
            'folder in the Archive
            Dim l_stringErrorfolderpath As String
            l_stringErrorfolderpath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "ERROR"

            'if folder doesnot exist create the folder runtime:
            If Me.CheckFolderExists(l_stringErrorfolderpath) = False Then
                Me.CreateFolders(l_stringErrorfolderpath)
            End If

            'set source folder path
            l_String_PDF_SourceFolder = p_stringPDFfolderpath.Trim()
            l_String_IDX_SourceFolder = p_stringIDXFolderPath.Trim()
            l_string_Archivefolder = p_stringArchivefolder.Trim()
            'get destination folder path
            l_String_DestFolder = Me.GetDestinationPath(p_boolparticipant)

            For I = LBound(arrPDFFiles) To UBound(arrPDFFiles)

                'source folder
                If Right(l_String_PDF_SourceFolder, 1) = "\" Then
                    l_String_SourceFile = l_String_PDF_SourceFolder.Trim() + arrPDFFiles(I).Name.ToString.Trim()
                Else
                    l_String_SourceFile = l_String_PDF_SourceFolder.Trim() + "\\" + arrPDFFiles(I).Name.ToString.Trim()
                End If
                'destination folder
                If Right(l_String_DestFolder, 1) = "\" Then
                    l_String_DestFile = l_String_DestFolder.Trim() + arrPDFFiles(I).Name.ToString.Trim()
                Else
                    l_String_DestFile = l_String_DestFolder.Trim() + "\\" + arrPDFFiles(I).Name.ToString.Trim()
                End If

                'l_StringArchiveFolderpath
                If Right(l_string_Archivefolder, 1) = "\" Then
                    l_string_ArchiveFile = l_string_Archivefolder.Trim() + arrPDFFiles(I).Name.ToString.Trim()
                Else
                    l_string_ArchiveFile = l_string_Archivefolder.Trim() + "\\" + arrPDFFiles(I).Name.ToString.Trim()
                End If


                'get the name of pdf file and find if corresponding idx exists then write to datatable
                l_intIndex = arrPDFFiles(I).Name.ToString.IndexOf(".")
                l_stringPDFFilename = arrPDFFiles(I).Name.ToString.Substring(0, l_intIndex).Trim()

                If Right(l_String_IDX_SourceFolder, 1) = "\" Then
                    l_stringIDXFilename = l_String_IDX_SourceFolder.Trim() + l_stringPDFFilename + ".idx"
                Else
                    l_stringIDXFilename = l_String_IDX_SourceFolder.Trim() + "\\" + l_stringPDFFilename + ".idx"
                End If

                If File.Exists(l_stringIDXFilename) Then
                    ' l_index = arrIDXFiles.BinarySearch(arrIDXFiles, l_stringPDFFilename.ToString())
                    l_bool = Me.AddFileListRow(l_String_PDF_SourceFolder, l_String_SourceFile, l_String_DestFolder, l_String_DestFile, l_string_Archivefolder, l_string_ArchiveFile)
                    If l_bool Then
                        l_String_SourceFile = l_stringIDXFilename
                        l_String_DestFile = l_String_DestFolder.Trim() + l_stringPDFFilename + ".idx"
                        l_string_ArchiveFile = l_string_Archivefolder + l_stringPDFFilename + ".idx"

                        l_bool = Me.AddFileListRow(l_String_IDX_SourceFolder, l_String_SourceFile, l_String_DestFolder, l_String_DestFile, l_string_Archivefolder, l_string_ArchiveFile)
                    End If
                Else
                    'send the pdf file to error folder
                    l_String_DestFile = l_stringErrorfolderpath + "\\" + arrPDFFiles(I).Name.ToString.Trim()
                    l_string_ArchiveFile = l_String_DestFile
                    l_bool = Me.AddFileListRow(l_String_PDF_SourceFolder, l_String_SourceFile, l_stringErrorfolderpath, l_String_DestFile, l_stringErrorfolderpath, l_string_ArchiveFile)
                End If
            Next I

            'check if any idx files are left out 
            'send them to error folder.
            arrIDXFiles = IDXDirInfo.GetFiles(p_stringIDXfileextension)
            If arrIDXFiles.Length > 0 Then
                For I = LBound(arrIDXFiles) To UBound(arrIDXFiles)
                    If Right(l_String_IDX_SourceFolder, 1) = "\" Then
                        l_stringIDXFilename = l_String_IDX_SourceFolder.Trim() + arrIDXFiles(I).Name.ToString.Trim()
                    Else
                        l_stringIDXFilename = l_String_IDX_SourceFolder.Trim() + "\\" + arrIDXFiles(I).Name.ToString.Trim()
                    End If

                    'get the name of idx file and find if corresponding pdf exists then write to datatable
                    l_intIndex = arrIDXFiles(I).Name.ToString.IndexOf(".")
                    l_stringPDFFilename = arrIDXFiles(I).Name.ToString.Substring(0, l_intIndex).Trim()

                    If Right(l_String_IDX_SourceFolder, 1) = "\" Then
                        l_stringPDFFilename = l_String_PDF_SourceFolder.Trim() + l_stringPDFFilename + ".pdf"
                    Else
                        l_stringPDFFilename = l_String_PDF_SourceFolder.Trim() + "\\" + l_stringPDFFilename + ".pdf"
                    End If

                    If Not File.Exists(l_stringPDFFilename) Then
                        l_String_DestFile = l_stringErrorfolderpath + "\\" + arrIDXFiles(I).Name.ToString.Trim()
                        l_String_DestFolder = l_stringErrorfolderpath.Trim()
                        l_string_Archivefolder = l_String_DestFolder
                        l_string_ArchiveFile = l_String_DestFile
                        l_bool = Me.AddFileListRow(l_String_IDX_SourceFolder, l_stringIDXFilename, l_String_DestFolder, l_String_DestFile, l_string_Archivefolder, l_string_ArchiveFile)
                    End If
                Next
            End If

            Session("FTFileList") = Me.SetdtFileList

        Catch
            Throw
        End Try

    End Function

    Public Function CopyPayrollFiles() As String
        Try
            Dim l_datasetMetaOutputfiletypes As New DataSet
            Dim l_StringDestFolder As String
            Dim arrFiles() As FileInfo
            Dim l_datarow As DataRow()
            Dim l_stringerrormsg As String = String.Empty
            Dim l_stringErrMessage As String = String.Empty


            'get the destination folder paths for the different file types of Payroll
            l_datasetMetaOutputfiletypes = YMCARET.YmcaBusinessObject.YMCACommonBOClass.MetaOutputChkFileType()


            'EFT
            Me.OutputFileType = "EFT"
            Me.FileExtension = "*.*"
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("EFT")
            Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "EFT"

            If l_datasetMetaOutputfiletypes.Tables(0).Rows.Count > 0 Then
                l_datarow = l_datasetMetaOutputfiletypes.Tables(0).Select("OutputFileType = '" + Me.OutputFileType.Trim() + "'")
                If l_datarow.Length > 0 Then
                    'get the output directory
                    l_StringDestFolder = l_datarow(0)(5).ToString().Trim()
                    Me.DestFolderPath = l_StringDestFolder
                Else
                    l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                    If Not l_stringerrormsg = String.Empty Then
                        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        l_stringerrormsg = ""
                    End If
                End If
            Else
                l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
                'CopyPayrollFiles = l_stringerrormsg
                'Exit Function
            End If
            l_stringerrormsg = Me.CopyFiles(True)

            If Not l_stringerrormsg = String.Empty Then
                l_stringErrMessage += l_stringerrormsg + vbCrLf
                l_stringerrormsg = ""
            End If

            'PP FOLDER COPYING
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("PP")
            Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "PP"
            Me.OutputFileType = "PP"
            Me.FileExtension = "*.*"


            If l_datasetMetaOutputfiletypes.Tables(0).Rows.Count > 0 Then
                l_datarow = l_datasetMetaOutputfiletypes.Tables(0).Select("OutputFileType = '" + Me.OutputFileType.Trim() + "'")
                If l_datarow.Length > 0 Then
                    'get the output directory
                    l_StringDestFolder = l_datarow(0)(5).ToString().Trim()
                    Me.DestFolderPath = l_StringDestFolder
                Else
                    l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                    If Not l_stringerrormsg = String.Empty Then
                        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        l_stringerrormsg = ""
                    End If
                End If
            Else
                l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If
            l_stringerrormsg = Me.CopyFiles(True)
            If Not l_stringerrormsg = String.Empty Then

                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If



            'CHKSCU
            'l_stringOutputfiletype = "CHKSCU"

            Me.OutputFileType = "CHKSCU"
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("CHKSCU")
            Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "CHKSCU"
            Me.FileExtension = "*.*"
            If l_datasetMetaOutputfiletypes.Tables(0).Rows.Count > 0 Then
                l_datarow = l_datasetMetaOutputfiletypes.Tables(0).Select("OutputFileType = '" + Me.OutputFileType.Trim() + "'")
                If l_datarow.Length > 0 Then
                    'get the output directory
                    l_StringDestFolder = l_datarow(0)(5).ToString().Trim()
                    Me.DestFolderPath = l_StringDestFolder
                Else
                    l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                    If Not l_stringerrormsg = String.Empty Then
                        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        l_stringerrormsg = ""
                    End If
                End If
            Else
                l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If
            l_stringerrormsg = Me.CopyFiles(True)
            If Not l_stringerrormsg = String.Empty Then
                l_stringErrMessage += l_stringerrormsg + vbCrLf
                l_stringerrormsg = ""
            End If



            'CHKSCC
            'l_stringOutputfiletype = "CHKSCC"
            Me.OutputFileType = "CHKSCC"
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("CHKSCC")
            Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "CHKSCC"
            Me.FileExtension = "*.*"


            If l_datasetMetaOutputfiletypes.Tables(0).Rows.Count > 0 Then
                l_datarow = l_datasetMetaOutputfiletypes.Tables(0).Select("OutputFileType = '" + Me.OutputFileType.Trim() + "'")
                If l_datarow.Length > 0 Then
                    'get the output directory
                    l_StringDestFolder = l_datarow(0)(5).ToString().Trim()
                    Me.DestFolderPath = l_StringDestFolder
                Else
                    l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                    If Not l_stringerrormsg = String.Empty Then
                        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        l_stringerrormsg = ""
                    End If
                End If
            Else
                l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If
            l_stringerrormsg = Me.CopyFiles(True)

            If Not l_stringerrormsg = String.Empty Then
                l_stringErrMessage += l_stringerrormsg + vbCrLf
                l_stringerrormsg = ""
            End If

            'EFTPRE

            Me.OutputFileType = "EFTPRE"
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("EFTPRE")
            Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "EFTPRE"
            Me.FileExtension = "*.*"


            l_stringerrormsg = Me.CopyFiles(False)

            If Not l_stringerrormsg = String.Empty Then
                l_stringErrMessage += l_stringerrormsg + vbCrLf
                l_stringerrormsg = ""
            End If

            'EDI
            Me.OutputFileType = "EDI_US"
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("EDI")
            Me.ArchiveFolderPath = ConfigurationSettings.AppSettings("ARCHIVE") + "\\" + "EDI"
            Me.FileExtension = "*.*"

            'l_stringOutputfiletype = "EDI_US"
            If l_datasetMetaOutputfiletypes.Tables(0).Rows.Count > 0 Then
                l_datarow = l_datasetMetaOutputfiletypes.Tables(0).Select("OutputFileType = '" + Me.OutputFileType.Trim() + "'")
                If l_datarow.Length > 0 Then
                    'get the output directory
                    l_StringDestFolder = l_datarow(0)(5).ToString().Trim()
                    Me.DestFolderPath = l_StringDestFolder
                Else
                    l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                    If Not l_stringerrormsg = String.Empty Then
                        l_stringErrMessage += l_stringerrormsg + vbCrLf
                        l_stringerrormsg = ""
                    End If
                End If
            Else
                l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file for Outfiletype:" + Me.OutputFileType
                If Not l_stringerrormsg = String.Empty Then
                    l_stringErrMessage += l_stringerrormsg + vbCrLf
                    l_stringerrormsg = ""
                End If
            End If
            l_stringerrormsg = Me.CopyFiles(True)

            If Not l_stringerrormsg = String.Empty Then
                l_stringErrMessage += l_stringerrormsg + vbCrLf
                l_stringerrormsg = ""
            End If

            CopyPayrollFiles = l_stringErrMessage
        Catch
            Throw
        End Try
    End Function

    Public Function CopyFiles(ByVal boolPayrollfiles As Boolean) As String
        Dim l_stringerrormsg As String = String.Empty
        Dim arrFiles As FileInfo()
        Dim l_dataset As New DataSet
        Dim l_DataRow As DataRow
        Try
            'CHECK IF SOURCE FOLDER EXISTS
            If Not Me.SourceFolderPath Is Nothing Then
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    'GET THE DESTINATION FOLDER PATH FOR ACH,DELINQUENCY FILES
                    'Destination path already set for Payroll files
                    If boolPayrollfiles = False Then
                        l_dataset = YMCARET.YmcaBusinessObject.YMCACommonBOClass.MetaOutputFileType(Me.OutputFileType)

                        If l_dataset.Tables(0).Rows.Count > 0 Then
                            l_DataRow = l_dataset.Tables(0).Rows(0)
                        Else
                            l_stringerrormsg = "Unable to find Values in the MetaOutput file."
                            CopyFiles = l_stringerrormsg
                            Exit Function
                        End If
                        'GET THE DESTINATION FOLDER 
                        If l_DataRow Is Nothing Then
                            l_stringerrormsg = "Unable to find Configuration Values in the MetaOutput file."
                        Else
                            Me.DestFolderPath = Convert.ToString(l_DataRow("OutputDirectory")).Trim()
                        End If
                    End If
                    Dim myDirInfo As New DirectoryInfo(Me.SourceFolderPath)
                    arrFiles = myDirInfo.GetFiles(Me.FileExtension)
                    If arrFiles.Length > 0 Then
                        If l_stringerrormsg = String.Empty Then
                            l_stringerrormsg = RetrieveFilesFromFolder(Me.SourceFolderPath, Me.FileExtension, Me.DestFolderPath, Me.ArchiveFolderPath)
                        Else
                            ' MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringerrormsg, MessageBoxButtons.OK)
                            CopyFiles = l_stringerrormsg
                            Exit Function
                        End If
                        'Else
                        '    l_stringerrormsg = "No files exist to copy from the path :" + Me.SourceFolderPath.ToString().Trim()
                        '    'MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringerrormsg, MessageBoxButtons.OK)
                        '    CopyFiles = l_stringerrormsg
                        '    Exit Function
                    End If
                    'Else
                    '    l_stringerrormsg = "Following Folder doesnot exist:" + Me.SourceFolderPath
                    '    '  MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringerrormsg, MessageBoxButtons.OK)
                    '    CopyFiles = l_stringerrormsg
                    '    Exit Function
                End If
            End If
            Me.SourceFolderPath = String.Empty
            Me.DestFolderPath = String.Empty
            Me.ArchiveFolderPath = String.Empty
            Me.OutputFileType = String.Empty
            Me.FileExtension = String.Empty
            CopyFiles = l_stringerrormsg
        Catch
            Throw
        End Try
    End Function



    Public Function AddFileListRow(ByVal p_String_SourceFolder As String, ByVal p_String_SourceFile As String, ByVal p_String_DestFolder As String, ByVal p_String_DestFile As String, ByVal p_String_ArchiveFolderpath As String, ByVal p_String_ArchiveFile As String) As Boolean
        Dim l_curValues As DataRow
        Dim l_stringArchivefolderpath As String

        Try
            If p_String_SourceFolder <> "" And p_String_SourceFile <> "" And p_String_DestFolder <> "" And p_String_DestFile <> "" Then
                l_curValues = m_dtFileList.NewRow()
                l_curValues("SourceFolder") = p_String_SourceFolder
                If Right(p_String_SourceFolder, 1) = "\" Then
                    l_curValues("SourceFile") = p_String_SourceFile
                Else
                    l_curValues("SourceFile") = p_String_SourceFile
                End If

                l_curValues("DestFolder") = p_String_DestFolder
                If Right(p_String_DestFolder, 1) = "\" Then
                    l_curValues("DestFile") = p_String_DestFile
                Else
                    l_curValues("DestFile") = p_String_DestFile
                End If
                'get the archive folder path
                l_stringArchivefolderpath = p_String_ArchiveFolderpath
                l_curValues("ArchiveFolder") = l_stringArchivefolderpath
                If Right(l_stringArchivefolderpath, 1) = "\" Then
                    l_curValues("ArchiveFile") = p_String_ArchiveFile
                Else
                    l_curValues("ArchiveFile") = p_String_ArchiveFile
                End If

                m_dtFileList.Rows.Add(l_curValues)

                AddFileListRow = True
            End If
        Catch
            Throw
        End Try
    End Function

    Public Function CheckKeyFolderExistence()

        Dim l_stringErrMsg As String = String.Empty
        Try


        Catch
            Throw
        End Try

    End Function
    'sets the check box status
    Public Function SetCheckBoxStatus()
        Dim l_boolExists As Boolean = False
        Dim l_boolFilesExist As Boolean = False
        Dim l_stringErrMsg As String = String.Empty
        Dim l_stringMessage As String = String.Empty
        Dim l_stringArchivefolderpath As String
        Try

            'CHECK WHETHER ARCHIVE FOLDER EXISTS
            l_stringArchivefolderpath = ConfigurationSettings.AppSettings("ARCHIVE")
            If l_stringArchivefolderpath <> Nothing Then
                If Me.CheckFolderExists(l_stringArchivefolderpath) = False Then
                    ' l_stringErrMsg = "Archive Folder doesn't exist.Please create it to continue"
                    'MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", l_stringErrMsg, MessageBoxButtons.OK)
                    'Exit Function
                    Throw New Exception("Archive Folder doesn't exist.Please create it to continue")
                End If
            Else
                Throw New Exception("ARCHIVE key not found in Configuration file.")
            End If


            'ACH
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("ACH")
            If Me.SourceFolderPath <> Nothing Then
                Me.SourceFolderPath = Me.SourceFolderPath.ToString.Trim() + "\\" + "EXPORT"
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = False Then
                        Me.CheckBoxACH.Enabled = False
                        Me.CheckBoxACH.Checked = False
                    Else
                        Me.CheckBoxACH.Enabled = True
                        Me.CheckBoxACH.Checked = True
                    End If
                Else
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                    'Throw New Exception(l_stringErrMsg)
                    '      Me.LabelErrorMsg3.Text = l_stringErrMsg.ToString()
                    Me.CheckBoxACH.Enabled = False
                    Me.CheckBoxACH.Checked = False
                End If
            Else
                Throw New Exception("ACH key not found in Configuration file.")
            End If


            'DELINQ
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("DELINQ")
            If Me.SourceFolderPath <> Nothing Then
                Me.SourceFolderPath = Me.SourceFolderPath + "\\" + "CSV"
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = False Then
                        Me.CheckBoxDELINQ.Enabled = False
                        Me.CheckBoxDELINQ.Checked = False
                    Else
                        Me.CheckBoxDELINQ.Enabled = True
                        Me.CheckBoxDELINQ.Checked = True
                    End If
                Else
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                    '       Me.LabelErrorMsg2.Text = l_stringErrMsg.ToString()
                    Me.CheckBoxDELINQ.Enabled = False
                    Me.CheckBoxDELINQ.Checked = False
                End If
            Else
                Throw New Exception("DELINQ key not found in Configuration file.")
            End If

            'IDMPath
            l_boolFilesExist = False
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("IDMPath")
            If Me.SourceFolderPath <> Nothing Then
                Me.SourceFolderPath = Me.SourceFolderPath + "\\" + "YMCA" + "\\" + "PDF"
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = True Then
                        l_boolFilesExist = True
                    End If
                Else
                    ' l_boolFilesExist = False
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                End If

                Me.SourceFolderPath = ConfigurationSettings.AppSettings("IDMPath") + "\\" + "PARTICIPANT" + "\\" + "PDF"
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = True Then
                        l_boolFilesExist = True
                    End If
                Else
                    ' l_boolFilesExist = False
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                    'Me.LabelErrorMsg1.Text = l_stringErrMsg.ToString()
                End If

                If l_boolFilesExist = False Then
                    Me.CheckBoxIDM.Enabled = False
                    Me.CheckBoxIDM.Checked = False
                Else
                    Me.CheckBoxIDM.Enabled = True
                    Me.CheckBoxIDM.Checked = True
                End If
            Else
                Throw New Exception("IDMPath key not found in Configuration file.")
            End If



            'PAYROLL
            'EFT
            l_boolFilesExist = False
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("EFT")
            If Me.SourceFolderPath <> Nothing Then
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = True Then
                        l_boolFilesExist = True
                    End If
                Else
                    '  l_boolFilesExist = False
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                End If
            Else
                Throw New Exception("EFT key not found in Configuration file.")
            End If


            'PP
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("PP")
            If Me.SourceFolderPath <> Nothing Then
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = True Then
                        l_boolFilesExist = True
                    End If
                Else
                    ' l_boolFilesExist = False
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                End If
            Else
                Throw New Exception("PP key not found in Configuration file.")
            End If

            'CHKSCU
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("CHKSCU")
            If Me.SourceFolderPath <> Nothing Then
                If Me.CheckFolderExists(Me.SourceFolderPath) Then
                    l_boolExists = Me.CheckFilesExists()
                    If l_boolExists = True Then
                        l_boolFilesExist = True
                    End If
                Else
                    ' l_boolFilesExist = False
                    l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                    l_stringMessage += l_stringErrMsg + vbCrLf
                End If
            Else
                Throw New Exception("CHKSCU key not found in Configuration file.")
            End If

            'CHKSCC
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("CHKSCC")
            If Me.CheckFolderExists(Me.SourceFolderPath) Then
                l_boolExists = Me.CheckFilesExists()
                If l_boolExists = True Then
                    l_boolFilesExist = True
                End If
            Else
                'l_boolFilesExist = False
                l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                l_stringMessage += l_stringErrMsg + vbCrLf
            End If
            'EFTPRE
            Me.SourceFolderPath = ConfigurationSettings.AppSettings("EFTPRE")
            If Me.CheckFolderExists(Me.SourceFolderPath) Then
                l_boolExists = Me.CheckFilesExists()
                If l_boolExists = True Then
                    l_boolFilesExist = True
                End If
            Else
                'l_boolFilesExist = False
                l_stringErrMsg = "Folder doesnot exist at the path: " + SourceFolderPath
                l_stringMessage += l_stringErrMsg + vbCrLf
                'Me.LabelErrorMsg3.Text = l_stringErrMsg.ToString()
            End If

            If l_boolFilesExist = False Then
                Me.CheckBoxPayroll.Enabled = False
                Me.CheckBoxPayroll.Checked = False
            Else
                Me.CheckBoxPayroll.Enabled = True
                Me.CheckBoxPayroll.Checked = True
            End If

            If l_stringMessage.ToString.Trim = String.Empty Then
                Session("Message") = Nothing
            Else
                Session("Message") = l_stringMessage
            End If

            'enable or disable the Process button
            If Me.CheckBoxACH.Enabled = False And Me.CheckBoxDELINQ.Enabled = False And Me.CheckBoxIDM.Enabled = False And Me.CheckBoxPayroll.Enabled = False Then
                Me.ButtonStartProcess.Enabled = False
                ' MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "No Files Exist to Copy", MessageBoxButtons.OK)
            Else
                Me.ButtonStartProcess.Enabled = True
            End If

        Catch
            Throw
        End Try
    End Function
    'check for existence of files
    Public Function CheckFilesExists() As Boolean
        Dim arrFiles As FileInfo()
        Try
            Dim myDirInfo As New DirectoryInfo(Me.SourceFolderPath)
            arrFiles = myDirInfo.GetFiles("*.*")
            Me.SourceFolderPath = String.Empty
            If arrFiles.Length > 0 Then
                Return True
            Else
                Return False

            End If
        Catch
            Throw
        End Try
    End Function

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub
    Public Sub GetCatch(ByVal ex As Exception)
        Dim l_String_Exception_Message As String
        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    End Sub
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
    Public Function GetDestinationPath(ByVal p_boolParticipant As Boolean) As String
        Dim l_string_ServerName As String
        Dim l_string_FilePath As String
        Try
            l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
            l_string_FilePath = YMCARET.YmcaBusinessObject.RefundRequest.GetVignettePath(l_string_ServerName)
            'Start:AA:06.27.2016 BT-1156 Add a precautionary measure while IDM creation 
            'If the destination path was not set in the serverlookup then the exception wil be raised
            If String.IsNullOrEmpty(l_string_FilePath) Then
                Dim ex As New Exception("IDM Destination path is not configured in serverlookup table. Please contact database administrator.")
                HelperFunctions.LogException("CopyFileUtil_IDM_GetDestinationPath", ex)
                Throw ex
            End If
            'End:AA:06.27.2016 BT-1156 Add a precautionary measure while IDM creation 
            'Modified By Ashutosh Patil as on 18-Apr-2007
            If p_boolParticipant = True Then
                If Right(l_string_FilePath, 1) = "\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "PARTICIPANT\"
                Else
                    l_string_FilePath = l_string_FilePath.Trim() & "\PARTICIPANT\"
                End If
            Else
                If Right(l_string_FilePath, 1) = "\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "YMCA\"
                Else
                    l_string_FilePath = l_string_FilePath.Trim() & "\YMCA\"
                End If
            End If
            GetDestinationPath = l_string_FilePath
        Catch
            Throw
        End Try
    End Function
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonStartProcess.Enabled = False
            ButtonStartProcess.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
