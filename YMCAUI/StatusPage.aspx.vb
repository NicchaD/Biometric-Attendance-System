'Created by Neeraj Singh
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Neeraj                         31-Aug-2010          added below code to check GetServiceVersion method from YrswebService
'Manthan Rajguru                2015.09.16           YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Imports System.Configuration
Imports System.Collections.Specialized
Imports System.Web.Configuration
Imports System.Linq
Imports System.Data.Linq
Imports System.Collections.Generic
Imports YMCARET.YmcaBusinessObject
Imports System
Imports System.Linq.Expressions
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common

Partial Public Class StatusPage
    Inherits System.Web.UI.Page
    Public Enum permissionSet
        checkCreate
        checkRead
        checkWrite
        checkDelete
    End Enum

    Public Class configSettingList

        Public Sub New(ByVal paramkeyname As String, ByVal paramcheckFolder As Boolean)
            _keyName = paramkeyname
            _isFolderCheck = paramcheckFolder
        End Sub
        Public Sub New(ByVal paramkeyname As String, ByVal paramcheckFolder As Boolean, ByVal paramListOfPermissions As List(Of permissionSet))
            _keyName = paramkeyname
            _isFolderCheck = paramcheckFolder
            _ListofPermission = paramListOfPermissions
        End Sub
        Private _keyName As String
        Public Property keyName() As String
            Get
                Return _keyName
            End Get
            Set(ByVal value As String)
                _keyName = value
            End Set
        End Property

        Private _isFolderCheck As Boolean
        Public Property IsFolderCheck() As String
            Get
                Return _isFolderCheck
            End Get
            Set(ByVal value As String)
                _isFolderCheck = value
            End Set
        End Property

        Private _ListofPermission As List(Of permissionSet)
        Public Property ListofPermission() As List(Of permissionSet)
            Get
                Return _ListofPermission
            End Get
            Set(ByVal value As List(Of permissionSet))
                _ListofPermission = value
            End Set
        End Property


    End Class

    Dim dtStatus As DataTable
    Dim dtRow As DataRow
    Dim g_dbConn As Boolean
    Private expectedConfigKey As New List(Of configSettingList)


    Private Function createConfigList()
        expectedConfigKey.Add(New configSettingList("ErrorLog", True))
        expectedConfigKey.Add(New configSettingList("SmallConnectionTimeOut", False))
        expectedConfigKey.Add(New configSettingList("MediumConnectionTimeOut", False))
        expectedConfigKey.Add(New configSettingList("ExtraLargeConnectionTimeOut", False))
        expectedConfigKey.Add(New configSettingList("DataSource", False))
        expectedConfigKey.Add(New configSettingList("DatabaseName", False))
        expectedConfigKey.Add(New configSettingList("UserId", False))
        expectedConfigKey.Add(New configSettingList("Password", False))

        expectedConfigKey.Add(New configSettingList _
                                        ("ReportPath", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead})
                                       )
                                )

        expectedConfigKey.Add(New configSettingList("DBUserPassword", False))
        expectedConfigKey.Add(New configSettingList("LDAP_Path1", False))
        expectedConfigKey.Add(New configSettingList("LDAP_Path2", False))
        expectedConfigKey.Add(New configSettingList("LDAP_Path3", False))
        expectedConfigKey.Add(New configSettingList("LDAP_Path4", False))
        expectedConfigKey.Add(New configSettingList("LDAP_Domain", False))
        expectedConfigKey.Add(New configSettingList("Vignette_Path", False))
        expectedConfigKey.Add(New configSettingList("Vignette_Page_Participant", False))
        expectedConfigKey.Add(New configSettingList("Vignette_Page_YMCA", False))
        expectedConfigKey.Add(New configSettingList("EFT", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("PP", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("CHKSCU", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("CHKSCC", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("EFTPRE", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("SHIRA", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("EDI_US", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("IDMPath", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("DELINQ", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )

        expectedConfigKey.Add(New configSettingList("FTLIST", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("ARCHIVE", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("RUNFutureMonthEnd", False))
        expectedConfigKey.Add(New configSettingList("smtpServer", False))
        expectedConfigKey.Add(New configSettingList("LockBox", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("ACH", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("UpdateCashedcheckDate", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("SessionXml", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )
        expectedConfigKey.Add(New configSettingList("DEVMode", False))
        expectedConfigKey.Add(New configSettingList("DlyIntManagmentXml", True, _
                                            New List(Of permissionSet) _
                                            ({permissionSet.checkRead, permissionSet.checkCreate, permissionSet.checkWrite, permissionSet.checkDelete})
                                       )
                                )


    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'LoadConfigKey()
            trUserid.InnerText = "ASP.NET user identity : " + User.Identity.Name + " and Environment.UserName : " + Environment.UserName
            createConfigList()
            ReadAppsettingFromWebConfig()
        Catch ex As Exception
            HelperFunctions.LogException("", ex)
        End Try

    End Sub

    Private Function ReadAppsettingFromWebConfig()

        Try
            dtStatus = New DataTable
            dtStatus.Columns.Add(New DataColumn("KeyName", GetType(String)))
            dtStatus.Columns.Add(New DataColumn("Message", GetType(String)))
            dtStatus.Columns.Add(New DataColumn("IsFail", GetType(Boolean)))
            dtStatus.Columns("IsFail").DefaultValue = False

            Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration("~")

            Dim appsec As AppSettingsSection = CType(config.GetSection("appSettings"), AppSettingsSection)

            Dim strKeyValye As String = String.Empty
            Dim isKeyMatched As Boolean

            For Each str As configSettingList In expectedConfigKey
                strKeyValye = String.Empty
                isKeyMatched = False

                For Each key As String In appsec.Settings.AllKeys
                    'check for key 
                    If isKeyMatched = False Then
                        If str.keyName.ToUpper.Trim.Equals(key.ToUpper.Trim()) Then
                            'check for value in key
                            isKeyMatched = True
                            strKeyValye = appsec.Settings(key).Value
                            If strKeyValye Is Nothing Or strKeyValye = "" Then
                                AddToErrorMesageList(str.keyName, "Value for key is missing", True)
                            Else
                                If str.IsFolderCheck Then
                                    CheckForInnerFolder(str, strKeyValye)
                                End If
                            End If
                            Exit For
                        End If
                    End If
                Next
                If isKeyMatched = False Then
                    AddToErrorMesageList(str.keyName, "This key is missing", True)
                End If
            Next

            Dim xdoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            xdoc.Load(Server.MapPath("web.config"))

            'check for External config file
            CheckForExternalConfigFileSetting(xdoc)
            'Check DB Connection
            checkDataBaseConnection()

            '----------------------------------------------------
            'Shashi Shekhar: 2010-08-18: Getting EmailStatus
            If g_dbConn = True Then 'Database connection check
                'Check for Email status
                checkEmailStatus()
            End If
            '---------------------------------------------------

            'Check for YmcaWebService
            CheckYMCAWebService(xdoc)

            gdvKeyStatus.DataSource = dtStatus
            gdvKeyStatus.DataBind()



        Catch ex As Exception

        End Try
    End Function
    Private Function CheckForInnerFolder(ByVal paramstr As configSettingList, ByVal paramstrKeyValye As String) As String
        Dim newPath As String = paramstrKeyValye
        If paramstr.keyName.ToUpper() = "ACH" Then
            'Add export
            newPath = paramstrKeyValye + "\\Export"
            TraverseInFolder(paramstr, newPath)
            'Add Import
            newPath = paramstrKeyValye + "\\Import"
            TraverseInFolder(paramstr, newPath)
        ElseIf paramstr.keyName.ToUpper() = "UpdateCashedcheckDate".ToUpper() Then
            'Add export
            newPath = paramstrKeyValye + "\\Import"
            TraverseInFolder(paramstr, newPath)
        Else
            TraverseInFolder(paramstr, newPath)
        End If

    End Function

    Private Function TraverseInFolder(ByVal str As configSettingList, ByVal strKeyValye As String)
        If CheckForFolderPath(str.keyName, strKeyValye) Then
            AddToErrorMesageList(str.keyName, "Ok")
            If Not str.ListofPermission Is Nothing Then
                CheckPermissionOnFolder(str, strKeyValye)
            End If
        End If
    End Function
    Private Function CheckForFolderPath(ByVal strKeyname As String, ByVal strPath As String) As Boolean
        If System.IO.Directory.Exists(strPath) Then
            'lblDir1.Text = dir_name & " exists"
            Return True
        Else
            AddToErrorMesageList(strKeyname, strPath + " Folder does not exists", True)
            Return False
        End If

    End Function
    'CHECK permission on folders
    Sub CheckPermissionOnFolder(ByVal paramCnfg As configSettingList, ByVal strPath As String)
        Dim fs As System.IO.FileInfo
        Dim fstr As System.IO.StreamWriter
        Dim re As System.IO.StreamReader
        'Dim isCheckNext As Boolean = False

        If paramCnfg.ListofPermission.Contains(permissionSet.checkCreate) Then
            Try
                fs = New System.IO.FileInfo(strPath + "\\TempCheckPermission.txt")
                fstr = fs.CreateText()

                AddToErrorMesageList(paramCnfg.keyName, "Has CREATE permission at " + strPath)
            Catch ex As Exception
                AddToErrorMesageList(paramCnfg.keyName, "Doesn't have permission to CREATE file at " + strPath, True)
            End Try
        End If
        If paramCnfg.ListofPermission.Contains(permissionSet.checkWrite) Then
            Try
                fstr.WriteLine("Test write permission.")
                fstr.Close()
                AddToErrorMesageList(paramCnfg.keyName, "Has WRITE permission at " + strPath)
            Catch ex As Exception
                AddToErrorMesageList(paramCnfg.keyName, "Doesn't have permission to WRITE in file at " + strPath, True)
            End Try
        End If

        If paramCnfg.ListofPermission.Contains(permissionSet.checkRead) Then
            Try
                Dim fstr1 As String()
                'If System.IO.Directory.Exists(strPath) Then
                '    fstr1 = System.IO.Directory.GetFileSystemEntries(strPath)

                Dim filePath As String = String.Empty
                If strPath.EndsWith("Reports") Then
                    If System.IO.Directory.Exists(strPath) Then
                        fstr1 = System.IO.Directory.GetFileSystemEntries(strPath)
                    End If
                    filePath = fstr1(0)
                Else
                    filePath = strPath + "\\TempCheckPermission.txt"
                End If

                If System.IO.File.Exists(filePath) Then
                    re = System.IO.File.OpenText(filePath)
                    If Not re.ReadLine() Is Nothing Then
                        AddToErrorMesageList(paramCnfg.keyName, "Has Read permission at " + strPath)
                    End If
                    re.Close()
                Else
                    AddToErrorMesageList(paramCnfg.keyName, "No file found to read at " + strPath, True)
                End If
                'End If



            Catch ex As Exception
                AddToErrorMesageList(paramCnfg.keyName, "Doesn't have permission to Read file at " + strPath, True)
            End Try
        End If

        If paramCnfg.ListofPermission.Contains(permissionSet.checkDelete) Then
            If System.IO.File.Exists(strPath + "\\TempCheckPermission.txt") Then
                Try
                    System.IO.File.Delete(strPath + "\\TempCheckPermission.txt")
                    AddToErrorMesageList(paramCnfg.keyName, "Has DELETE permission at " + strPath)
                Catch ex As Exception
                    AddToErrorMesageList(paramCnfg.keyName, "Doesn't have permission to DELETE a file at " + strPath, True)
                End Try

            End If
        End If


    End Sub

    Sub AddToErrorMesageList(ByVal strKeyname As String, ByVal errorMessage As String, Optional ByVal isFail As Boolean = False)
        dtRow = dtStatus.NewRow()
        dtRow("KeyName") = strKeyname
        dtRow("Message") = errorMessage
        dtRow("IsFail") = isFail
        dtStatus.Rows.Add(dtRow)
    End Sub

    Sub checkDataBaseConnection()
        Try
            Dim dsGetServerDBVersion As DataSet = YMCARET.YmcaBusinessObject.Login.GetServerDBVersion()
            If Not dsGetServerDBVersion Is Nothing AndAlso dsGetServerDBVersion.Tables.Count > 0 AndAlso dsGetServerDBVersion.Tables(0).Rows.Count > 0 Then
                Dim dbstring As String = dsGetServerDBVersion.Tables(0).Rows(0)("ServerName").ToString() + " " +
                                        dsGetServerDBVersion.Tables(0).Rows(0)("DatabaseName").ToString() + " " +
                dsGetServerDBVersion.Tables(0).Rows(0)("DatabaseVersion").ToString()
                AddToErrorMesageList("DataBaseConnection", "Succeed with connection string " + dbstring)
                g_dbConn = True 'Shashi Shekhar:2010-08-19:set global variable for database connection
            Else
                AddToErrorMesageList("DataBaseConnection", "Database connection failed", True)
                g_dbConn = False 'Shashi Shekhar:2010-08-19:set global variable for database connection
            End If
        Catch ex As Exception

        End Try

    End Sub

    Sub CheckForExternalConfigFileSetting(ByVal paramXdoc As System.Xml.XmlDocument)
        'check for connectionstring 
        Dim strConnString = paramXdoc.GetElementsByTagName("connectionStrings").Item(0).Attributes(0).InnerText
        If strConnString Is Nothing Then
            AddToErrorMesageList("connectionStrings", "External Configuration file dataconfiguration.config is not specified.", True)
        Else
            'check file existence
            If Not System.IO.File.Exists(Server.MapPath(strConnString)) Then
                AddToErrorMesageList("connectionStrings", strConnString + "file does not exists.", True)
            Else
                AddToErrorMesageList("connectionStrings", strConnString + "file found.")
            End If
        End If
        'check for exceptionHandling 
        Dim strException = paramXdoc.GetElementsByTagName("exceptionHandling").Item(0).Attributes(0).InnerText
        If strException Is Nothing Then
            AddToErrorMesageList("exceptionhandlingconfiguration", "External Configuration file exceptionhandlingconfiguration.config is not specified.", True)
        Else
            'check file existence
            If Not System.IO.File.Exists(Server.MapPath(strException)) Then
                AddToErrorMesageList("exceptionhandlingconfiguration", strException + "file does not exists.")
            Else
                AddToErrorMesageList("exceptionhandlingconfiguration", strException + "file found.")
            End If
        End If
        'check for loggingConfiguration 
        Dim strLogging = paramXdoc.GetElementsByTagName("loggingConfiguration").Item(0).Attributes(0).InnerText
        If strLogging Is Nothing Then
            AddToErrorMesageList("loggingconfiguration", "External Configuration file loggingconfiguration.config is not specified.", True)
        Else
            'check file existence
            If Not System.IO.File.Exists(Server.MapPath(strLogging)) Then
                AddToErrorMesageList("loggingconfiguration", strLogging + "file does not exists.", True)
            Else
                AddToErrorMesageList("loggingconfiguration", strLogging + "file found.")
            End If
        End If

    End Sub

    Sub CheckYMCAWebService(ByVal paramXDoc As System.Xml.XmlDocument)
        Dim ymcaServiceUrl As String
        Try
            ymcaServiceUrl = paramXDoc.GetElementsByTagName("applicationSettings").Item(0).InnerText
        Catch ex As Exception
        End Try
        If ymcaServiceUrl Is Nothing Then
            AddToErrorMesageList("YMCAUI_YRSWebService_YRSWithdrawalService", "Ymca WebService key is not added in web.config", True)
        Else
            'Neeraj 31-Aug-2010 : added below code to check GetServiceVersion method from YrswebService
            Try
                Dim objService As New YRSWebService.YRSWithdrawalService
                'Dim objWebServiceReturn As New YRSWebService.WebServiceReturn

                Dim serviceVersion As String = objService.GetServiceVersion()
                objService.Dispose()
                AddToErrorMesageList("YRSWithdrawalService", "Service at " + ymcaServiceUrl + " is running.")

            Catch ex As Exception
                AddToErrorMesageList("YRSWithdrawalService", "Service at " + ymcaServiceUrl + " is not running.", True)
            End Try
        End If
    End Sub

    'Shashi Shekhar: 2010-08-18: Check Email Status
    Sub checkEmailStatus()

        Dim l_DataSet As New DataSet
        Dim StatusPageBO As StatusBOClass = Nothing
        Dim list As New ArrayList

        Try

            'Adding key values in list
            list.Add("USE_DEFAULT")
            list.Add("ADMIN_DEFAULT_TO_EMAILID")
            list.Add("MAIL_SERVICE")

            l_DataSet = StatusPageBO.GetEmailStatus(list)


            If Not (l_DataSet Is Nothing) Then

                If l_DataSet.Tables(0).Rows.Count > 0 Then

                    Dim EmailStatus As DataTable = l_DataSet.Tables(0)

                    'Get data from dataset through LINQ
                    Dim query = From es In EmailStatus.AsEnumerable() _
                                Select New With _
                                    { _
                                        .chvKey = es.Field(Of String)("chvKey"), _
                                        .chvValue = es.Field(Of String)("chvValue")}


                    Dim l_USeDefault As String = String.Empty
                    Dim l_AdminDefaultEmailId As String = String.Empty
                    Dim l_MailService As String = String.Empty

                    'set values in variable
                    For Each p In query
                        If p.chvKey.Trim.ToUpper = "MAIL_SERVICE" Then
                            l_MailService = p.chvValue.Trim
                        ElseIf p.chvKey.Trim.ToUpper = "USE_DEFAULT" Then
                            l_USeDefault = p.chvValue.Trim
                        ElseIf p.chvKey.Trim.ToUpper = "ADMIN_DEFAULT_TO_EMAILID" Then
                            l_AdminDefaultEmailId = p.chvValue.Trim
                        End If
                    Next


                    'set message based on value
                    If l_MailService.Trim = "0" Then
                        AddToErrorMesageList("Email Status", "Emails are not being sent from the system.")
                    ElseIf l_MailService.Trim = "1" And l_USeDefault.Trim = "0" Then
                        AddToErrorMesageList("Email Status", "Email system has been configured for production and all emails are being sent from the system to specified email addresses.")
                    ElseIf l_MailService.Trim = "1" And l_USeDefault.Trim = "1" Then
                        AddToErrorMesageList("Email Status", "Email system has been configured for testing and all emails are being sent from the system to default email address - key.")
                    ElseIf (l_MailService.Trim = "1") And (l_USeDefault.Trim = "0") And (l_AdminDefaultEmailId.Trim = "" Or l_AdminDefaultEmailId = vbNull) Then
                        AddToErrorMesageList("Email Status", " Email system has been configured for testing but the default Email id has not been specified.")
                    End If


                End If
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            StatusPageBO = Nothing
            list = Nothing
        End Try

    End Sub


    'Shashi Shekhar: Get images as per value of IsFail column
    Public Function GetImage(ByVal IsFailed As Boolean) As String

        If IsFailed Then
            Return "~/Images/warning.jpg"
        Else
            Return "~/Images/tick.jpg"
        End If

    End Function


End Class