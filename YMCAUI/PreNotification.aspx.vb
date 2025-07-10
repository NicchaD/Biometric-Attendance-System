'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	PreNotification.aspx.vb

' Author Name		:	Ragesh .V.P
' Employee ID		:	34231
' Email				:	ragesh.vp@3i-infotech.com
' Contact No		:	55928736

'History
'***********
' Author Name		:	Janhavi Shetye
' Employee ID		:	32659
' Email				:	janhavi.shetye@3i-infotech.com
' Contact No		:	5592835
' Creation Time		:	5/18/05 2:03:48 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	Vartika Jain    
' Changed on			:	05/23/2005
' Change Description	:	Applying Style Sheet
' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                Date                Description
'*******************************************************************************
'Neeraj Singh               12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar         :   2009-12-14          'Resolve the issue no-BT 1067
'Shashi Shekhar         :   2010-06-16          'Migration related changes.
'Shashi Shekhar         :   2010-07-07          'Code review changes, Resolve the issue BT - 555.
'prasad Jadhav          :   2011-10-04          for YRS 5.0-632 : Test database output files need word "test" in them.
'Manthan Rajguru            2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru            2016.11.25          YRS enh: Primary bank needs change to PreNotification output (TrackIT 27951)
'Shilpa N                   03/04/2019          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'Megha Lad                  07/26/2019          YRS-AT-3352 - YRS enh: HOT FIX NEEDED:file format generation for Disbursements->Payroll ->Pre Notification - additional adjustments (TrackIT 29142)
'*******************************************************************************
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
Imports System.Security.Permissions
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class PreNotification
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("PreNotification.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLastName As System.Web.UI.WebControls.Label
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSsNo As System.Web.UI.WebControls.Label
    Protected WithEvents txtSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblActNo As System.Web.UI.WebControls.Label
    Protected WithEvents txtActNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblActType As System.Web.UI.WebControls.Label
    Protected WithEvents txtActType As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblBankName As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblBankABA As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankABA As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblEPT As System.Web.UI.WebControls.Label
    Protected WithEvents drpPTStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLookFor As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents txtAreaMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents labelLookFor As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridPreNotification As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSsNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelActNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxActNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelActType As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxActType As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelBankName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelBankABA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBankABA As System.Web.UI.WebControls.TextBox
    Protected WithEvents DropDownListPTStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelMessage As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAreaMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents Menu1 As skmMenu.Menu
    'Protected WithEvents datagridCheckBox As CustomControls.CheckBoxColumn


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Dim g_dataset_dsPreNotificationList As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Dim g_String_State As String



    Protected WithEvents buttonPreNote As System.Web.UI.WebControls.Button
    Protected WithEvents buttonGeneratePreNote As System.Web.UI.WebControls.Button
    Protected WithEvents buttonUpdateStatus As System.Web.UI.WebControls.Button
    Protected WithEvents buttonSave As System.Web.UI.WebControls.Button
    Protected WithEvents buttonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents buttonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonGo As System.Web.UI.WebControls.Button
    Protected WithEvents LabelEfT As System.Web.UI.WebControls.Label


    Dim g_bool_DeleteFlag As New Boolean

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Enum FormMode
        INNew
        INProcess
    End Enum
    Private Enum EnableMode
        UPDATE_EFT_STATUS
        GENERATE_PRE_NOTE
        PRINT_PRE_NOTE
    End Enum
    Private Enum LoadDatasetMode
        Table
        Session
    End Enum

    '** This property is used to handle the session for Transaction Mode.

    Private Property SessionFormMode() As FormMode
        Get
            If Not (Session("FormMode")) Is Nothing Then
                Return (CType(Session("FormMode"), FormMode))
            Else
                Return FormMode.INNew
            End If
        End Get

        Set(ByVal Value As FormMode)
            Session("FormMode") = Value
        End Set
    End Property
    Private Property SessionEftStatus() As String
        Get
            If Not (Session("String_EftStatus")) Is Nothing Then
                Return (DirectCast(Session("String_EftStatus"), String))
            Else
                Return String.Empty
            End If
        End Get

        Set(ByVal Value As String)
            Session("String_EftStatus") = Value
        End Set
    End Property

    Private Property SessionSelectedUniqueID() As String
        Get
            If Not (Session("String_SelectedUniqueID")) Is Nothing Then
                Return (CType(Session("String_SelectedUniqueID"), String))
            Else
                Return String.Empty
            End If
        End Get

        Set(ByVal Value As String)
            Session("String_SelectedUniqueID") = Value
        End Set
    End Property

    Private Property SessionDataSetPreNotificationList() As DataSet
        Get
            If Not (Session("dsPreNotificationList")) Is Nothing Then

                Return (DirectCast(Session("dsPreNotificationList"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsPreNotificationList") = Value
        End Set
    End Property

    Private Property SessionOutputDirectory() As String
        Get
            If Not (Session("String_OutputDirectory")) Is Nothing Then
                Return (DirectCast(Session("String_OutputDirectory"), String))
            Else
                Return String.Empty
            End If
        End Get

        Set(ByVal Value As String)
            Session("String_OutputDirectory") = Value
        End Set
    End Property
    Private Property SessionOutputFileName() As String
        Get
            If Not (Session("String_OutputFileName")) Is Nothing Then
                Return (DirectCast(Session("String_OutputFileName"), String))
            Else
                Return String.Empty
            End If
        End Get

        Set(ByVal Value As String)
            Session("String_OutputFileName") = Value
        End Set
    End Property
    'prasad Jadhav:4-oct-2011:for YRS 5.0-632 : Test database output files need word "test" in them.
    Dim test_production As String = String.Empty
    '** This property is used to handle session for Process date

    Private Property SessionProcessDate() As Date
        Get
            If Not (Session("Date_idprocessdate")) Is Nothing Then
                Return (CType(Session("Date_idprocessdate"), System.DateTime).Date())
            Else
                Return Convert.ToDateTime("01/01/1800")
            End If
        End Get
        Set(ByVal Value As Date)
            Session("Date_idprocessdate") = Value
        End Set
    End Property

    Private Property SessionSelectedIndex() As Long
        Get
            If Not (Session("Long_SelectedIndex")) Is Nothing Then
                Return (CType(Session("Long_SelectedIndex"), Long))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Long)
            Session("Long_SelectedIndex") = Value
        End Set
    End Property
    Private Property SessionEditMode() As Boolean
        Get
            If Not (Session("Bool_EditMode")) Is Nothing Then
                Return (CType(Session("Bool_EditMode"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("Bool_EditMode") = Value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try

            Me.labelLookFor.AssociatedControlID = Me.TextBoxLookFor.ID
            Me.LabelMessage.AssociatedControlID = Me.TextBoxMessage.ID
            Me.LabelSsNo.AssociatedControlID = Me.TextBoxSSNo.ID
            Me.LabelLastName.AssociatedControlID = Me.TextBoxLastName.ID
            Me.LabelFirstName.AssociatedControlID = Me.TextBoxFirstName.ID
            Me.LabelActNo.AssociatedControlID = Me.TextBoxActNo.ID
            Me.LabelActType.AssociatedControlID = Me.TextBoxActType.ID
            Me.LabelBankABA.AssociatedControlID = Me.TextBoxBankABA.ID
            Me.LabelBankName.AssociatedControlID = Me.TextBoxBankName.ID
            Me.LabelEfT.AssociatedControlID = Me.DropDownListPTStatus.ID

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            'Response.Redirect("NetCP/CopyFilestoFileServer.aspx", False)

            If Not Me.IsPostBack Then

                ' Identify the State Transaction.

                Me.SessionDataSetPreNotificationList = Nothing
                IdentiyfyState()

                PopulateData(LoadDatasetMode.Table)
            Else

                If Request.Form("OK") = "OK" Then

                    PopulateData(LoadDatasetMode.Session)
                End If

            End If
            CheckReadOnlyMode() ' Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub PopulateData(ByVal parameter_load As LoadDatasetMode)

        'Dim l_Cache As CacheManager
        Dim l_CheckBox As CheckBox
        Dim l_dataset As DataSet
        Dim l_datarow As DataRow
        Dim l_button_Select As ImageButton
        Dim l_String_Search As String
        Dim l_SelDataGridItem As DataGridItem

        Dim l_Find_Datatow As DataRow()
        Dim i As Long

        Try

            'Ragesh 34231 02/02/06 Cache to Session
            'l_Cache = CacheFactory.GetCacheManager()

            'Load Main Dataset Pre Notification to process.
            If parameter_load = LoadDatasetMode.Table Then
                g_dataset_dsPreNotificationList = YMCARET.YmcaBusinessObject.Prenotification.getPrenotificationList()

                Me.SessionEditMode = False

            Else
                'Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsPreNotificationList = CType(l_Cache.GetData("PreNotificationLists"), DataSet)
                g_dataset_dsPreNotificationList = Me.SessionDataSetPreNotificationList

                i = 0
                If g_bool_AddFlag = False Then
                    For Each l_DataGridItem As DataGridItem In Me.DataGridPreNotification.Items

                        l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")


                        'Find the Curresponding records in the Dataset.
                        l_String_Search = "UniqueID = '" + l_DataGridItem.Cells(6).Text + "'"
                        l_Find_Datatow = g_dataset_dsPreNotificationList.Tables(0).Select(l_String_Search)

                        If (Not l_CheckBox Is Nothing) And (Not l_Find_Datatow Is Nothing) Then

                            If Not l_Find_Datatow(0) Is Nothing Then
                                If l_CheckBox.Checked = True Then
                                    l_Find_Datatow(0)("Selected") = True
                                Else
                                    l_Find_Datatow(0)("Selected") = False
                                End If
                            End If
                            'l_Find_Datatow(0).AcceptChanges()

                        End If


                    Next



                End If
            End If

            g_bool_AddFlag = False


            If HelperFunctions.isEmpty(g_dataset_dsPreNotificationList) Then 'Added by shashi:2009-12-14: To resolve the issue no-BT 1067
                If g_dataset_dsPreNotificationList.Tables(0).Rows.Count = 0 Then
                    Me.DataGridPreNotification.AllowSorting = False 'Added by shashi:2009-12-14: To resolve the issue no-BT 1067
                    Me.LabelNoRecordFound.Visible = True
                    'Me.DataGridPreNotification.DataSource = g_dataset_dsPreNotificationList
                    Me.DataGridPreNotification.DataSource = g_dataset_dsPreNotificationList.Tables(0).DefaultView()

                    Me.DataGridPreNotification.DataBind()



                    PopulateControlsInToEmpty()

                    'CommonModule.HideColumnsinDataGrid(g_dataset_dsPreNotificationList, Me.DataGridPreNotification, "UniqueID, EftProcessDate, EftText, EftTypeCode, BankName, BankAbaNumber, BankAcctNumber, ActType ,Selected")
                    Exit Sub
                End If

            Else

                'Me.DataGridPreNotification.DataSource = g_dataset_dsPreNotificationList
                Me.DataGridPreNotification.AllowSorting = True   'Added by shashi:2009-12-14: To resolve the issue no-BT 1067

                Me.DataGridPreNotification.DataSource = g_dataset_dsPreNotificationList.Tables(0).DefaultView()
                Me.DataGridPreNotification.DataBind()

                ButtonSelectAll.Enabled = True
                Me.buttonEdit.Enabled = True
                Me.ButtonGo.Enabled = True

                'CommonModule.HideColumnsinDataGrid(g_dataset_dsPreNotificationList, Me.DataGridPreNotification, "UniqueID, EftProcessDate, EftText, EftTypeCode, BankName, BankAbaNumber, BankAcctNumber, ActType ,Selected")

                i = 0

                For Each l_DataGridItem As DataGridItem In Me.DataGridPreNotification.Items
                    If l_DataGridItem.Cells(6).Text.Trim = Me.SessionSelectedUniqueID.Trim Then
                        Me.SessionSelectedIndex = i
                    ElseIf Me.SessionSelectedUniqueID.Trim = "" Then
                        Me.SessionSelectedIndex = 0
                        Me.SessionSelectedUniqueID = l_DataGridItem.Cells(6).Text.Trim
                    End If

                    i = i + 1

                Next

                If Me.DataGridPreNotification.Items.Count > Me.SessionSelectedIndex Then
                    l_SelDataGridItem = Me.DataGridPreNotification.Items(Me.SessionSelectedIndex)
                    l_button_Select = l_SelDataGridItem.FindControl("ImageButtonSelect")
                    If (Not l_button_Select Is Nothing) Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    End If
                End If
                'Ragesh 34231 02/02/06 Cache to Session
                'l_Cache.Add("PreNotificationLists", g_dataset_dsPreNotificationList)
                Me.SessionDataSetPreNotificationList = g_dataset_dsPreNotificationList

            End If

            PopulateControles()

            ' Fill the status values in the Combo based on the State.
            LoadDropDownListPTStatus()

            If Me.SessionSelectedIndex <= g_dataset_dsPreNotificationList.Tables(0).Rows.Count - 1 Then
                Me.DataGridPreNotification.SelectedIndex = Me.SessionSelectedIndex

                'SHASHI SHEKHAR SINGH : 2010-07-09 : FOR BT - 555 .
                If Me.SessionSelectedIndex = 0 Then
                    Me.SessionSelectedUniqueID = DataGridPreNotification.SelectedItem.Cells(6).Text.Trim()
                End If
                '---------------------------------------------------------------------------------------------


                PopulateDataIntoControls(Me.SessionSelectedUniqueID)
            Else
                Me.DataGridPreNotification.SelectedIndex = 0
                PopulateDataIntoControls("")
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    Private Sub IdentiyfyState()

        Dim l_dataset_dsPreNotificationLogs As New DataSet

        Try

            l_dataset_dsPreNotificationLogs = YMCARET.YmcaBusinessObject.Prenotification.getPrenotificationLogs()

            ' Identify the State of Transaction. 
            If Not (l_dataset_dsPreNotificationLogs Is Nothing) Then

                If l_dataset_dsPreNotificationLogs.Tables(0).Rows.Count > 0 Then

                    Me.SessionFormMode = FormMode.INProcess

                    Me.SessionProcessDate = CType(l_dataset_dsPreNotificationLogs.Tables(0).Rows(0)("began"), System.DateTime).Date()

                Else
                    Me.SessionFormMode = FormMode.INNew

                End If

                l_dataset_dsPreNotificationLogs = Nothing
            End If

        Catch
            Throw
        End Try

    End Sub

    Private Sub LoadDropDownListPTStatus()

        'Populate the Status Combo box based on the State.
        Try

            If Me.SessionFormMode = FormMode.INProcess Then
                Me.DropDownListPTStatus.Items.Clear()
                Me.DropDownListPTStatus.Items.Add("Failed")
                Me.DropDownListPTStatus.Items.Add("Pending")
                Me.DropDownListPTStatus.Items.Add("")
            Else
                Me.DropDownListPTStatus.Items.Clear()
                Me.DropDownListPTStatus.Items.Add("Changed")
                Me.DropDownListPTStatus.Items.Add("Failed")
                Me.DropDownListPTStatus.Items.Add("OK")
                Me.DropDownListPTStatus.Items.Add("Pending")
                Me.DropDownListPTStatus.Items.Add("Verify")

            End If
        Catch
            Throw

        End Try

    End Sub

    Private Function PopulateControles()

        Try

            Me.TextBoxFirstName.ReadOnly = True
            Me.TextBoxFirstName.ReadOnly = True
            Me.TextBoxLastName.ReadOnly = True
            Me.TextBoxSSNo.ReadOnly = True
            Me.TextBoxActNo.ReadOnly = True
            Me.TextBoxActType.ReadOnly = True
            Me.TextBoxBankName.ReadOnly = True
            Me.TextBoxBankABA.ReadOnly = True

            If Me.SessionEditMode Then

                Me.DropDownListPTStatus.Enabled = True
                Me.TextBoxMessage.ReadOnly = False
                Me.buttonGeneratePreNote.Enabled = False
                Me.buttonPreNote.Enabled = False

                Me.buttonSave.Enabled = True
                Me.buttonCancel.Enabled = True
                Me.ButtonOK.Enabled = False
                Me.buttonEdit.Enabled = False
                Me.ButtonGo.Enabled = False
                Me.ButtonSelectAll.Enabled = False

                'If VerifyCurrentValues(False, EnableMode.UPDATE_EFT_STATUS) Then
                Me.buttonUpdateStatus.Enabled = False
                'Else
                'Me.buttonUpdateStatus.Enabled = False
                'End If
                Me.TextBoxLookFor.ReadOnly = True

            Else

                Me.DropDownListPTStatus.Enabled = False
                Me.TextBoxMessage.ReadOnly = True
                Me.buttonSave.Enabled = False
                Me.buttonCancel.Enabled = False
                Me.ButtonOK.Enabled = True
                Me.ButtonGo.Enabled = True
                Me.ButtonSelectAll.Enabled = True

                Me.TextBoxLookFor.ReadOnly = False

                If VerifyCurrentValues(False, EnableMode.UPDATE_EFT_STATUS) Then
                    Me.buttonUpdateStatus.Enabled = True
                Else
                    Me.buttonUpdateStatus.Enabled = False
                End If

                If VerifyCurrentValues(False, EnableMode.GENERATE_PRE_NOTE) Then
                    Me.buttonGeneratePreNote.Enabled = True
                Else
                    Me.buttonGeneratePreNote.Enabled = False
                End If

                If VerifyCurrentValues(False, EnableMode.PRINT_PRE_NOTE) Then
                    Me.buttonPreNote.Enabled = True
                Else
                    Me.buttonPreNote.Enabled = False
                End If

            End If

        Catch

            Throw

        End Try
    End Function


    Private Function VerifyCurrentValues(ByVal Paramter_bool_check As Boolean, ByVal Paramter_process As EnableMode) As Boolean
        Dim l_DataSet As DataSet
        Dim i As Long
        Dim l_bool_flag As Boolean
        Dim l_string_Status As String

        l_bool_flag = False

        Try

            l_DataSet = g_dataset_dsPreNotificationList

            Select Case Paramter_process

                Case EnableMode.UPDATE_EFT_STATUS
                    'Store "P" to find the Status value. 
                    l_string_Status = "P"

                Case EnableMode.GENERATE_PRE_NOTE
                    'Store "V" to find the Status value. 
                    l_string_Status = "V"

                Case EnableMode.PRINT_PRE_NOTE
                    ' Validate the record count to enable or disable the Print Prenote Button

                    If Not (l_DataSet Is Nothing) Then
                        If l_DataSet.Tables(0).Rows.Count() > 0 Then
                            VerifyCurrentValues = True
                            Exit Function
                        End If

                        VerifyCurrentValues = False
                    End If

            End Select

            If Not (l_DataSet Is Nothing) Then

                If Paramter_bool_check Then

                    For i = 0 To l_DataSet.Tables(0).Rows.Count - 1
                        If l_DataSet.Tables(0).Rows(i)("EFT Status").ToString = l_string_Status.ToString() And CType(l_DataSet.Tables(0).Rows(i)("Selected"), Boolean) = True Then
                            l_bool_flag = True
                            Exit For
                        End If
                    Next

                    l_DataSet = Nothing

                    VerifyCurrentValues = l_bool_flag
                Else

                    For i = 0 To l_DataSet.Tables(0).Rows.Count - 1
                        If l_DataSet.Tables(0).Rows(i)("EFT Status").ToString() = l_string_Status.ToString() Then
                            l_bool_flag = True
                            Exit For
                        End If
                    Next i

                    l_DataSet = Nothing

                    VerifyCurrentValues = l_bool_flag

                End If
            Else
                VerifyCurrentValues = False
            End If

        Catch

            Throw

        End Try

    End Function
    Private Function GenerateTextFile(ByVal parameter_date_Prenote As DateTime, ByRef l_datatable_FileList As DataTable) As Boolean

        'Define dataset
        Dim l_dataset As DataSet
        Dim l_DataRow As DataRow

        Dim l_string_FileName As String
        Dim l_string_FullFileName As String
        Dim l_string_FileNameBak As String
        Dim l_string_FileNameRen As String
        Dim l_string_Tmp As String
        Dim l_String_FileCreateDate As String
        Dim l_Date_Prenote As DateTime
        Dim l_Integer_Index As Integer
        Dim l_stringOutputDirectory As String

        Try
            'CHECK atleast one row with status 'V' to generate Pre-Note file.
            If VerifyCurrentValues(True, EnableMode.GENERATE_PRE_NOTE) = False Then

                GenerateTextFile = False
                MessageBox.Show(PlaceHolder1, "Pre-Notification File", "Please Check atleast one row with status 'V' to generate Pre-Note file.", MessageBoxButtons.Stop)
                GenerateTextFile = False
                Exit Function

            End If

            l_dataset = YMCARET.YmcaBusinessObject.Prenotification.MetaOutputFileType()

            If l_dataset Is Nothing Then
                MessageBox.Show(PlaceHolder1, "Pre-Notification File", "Unable to find EFT Prenote Configuration Values in the MetaOutput file.", MessageBoxButtons.Stop)
                GenerateTextFile = False
                Exit Function
            End If

            l_DataRow = l_dataset.Tables(0).Rows(0)

            If l_DataRow Is Nothing Then
                MessageBox.Show(PlaceHolder1, "Pre-Notification File", "Unable to find EFT Prenote Configuration Values in the MetaOutput file.", MessageBoxButtons.Stop)
                GenerateTextFile = False
                Exit Function
            End If


            If (l_DataRow("OutputDirectory").GetType.ToString = "System.DBNull") Or CType(l_DataRow("OutputDirectory"), String) = "" Then
                MessageBox.Show(PlaceHolder1, "Pre-Notification File", "Output Directory Value not found in the Table.", MessageBoxButtons.Stop)

            Else

                l_stringOutputDirectory = System.Configuration.ConfigurationSettings.AppSettings("EFTPRE").ToString.Trim()

                'If Not Directory.Exists(CType(l_DataRow("OutputDirectory"), String).Trim()) Then
                If Not Directory.Exists(l_stringOutputDirectory) Then
                    MessageBox.Show(PlaceHolder1, "Pre-Notification File", "Directory does not exist for EFT Prenote output file. " + l_stringOutputDirectory, MessageBoxButtons.Stop)
                    GenerateTextFile = False
                    Exit Function
                End If

                If (l_DataRow("FilenamePrefix").GetType.ToString = "System.DBNull") Or CType(l_DataRow("FilenamePrefix"), String) = "" Then
                    l_string_FileName = String.Empty
                Else
                    l_string_FileName = CType(l_DataRow("FilenamePrefix"), String).Trim()
                End If
                'Added by prasad YRS 5.0-632 : Test database output files need word "test" in them.
                test_production = YMCARET.YmcaBusinessObject.Prenotification.GetPreNoteMode()
                If test_production = "TEST" Then
                    l_string_FileName = l_string_FileName + "_TEST"
                ElseIf test_production = "PRODUCTION" Then
                    l_string_FileName = l_string_FileName
                End If




                l_Date_Prenote = parameter_date_Prenote

                If CType(l_DataRow("DateSuffix"), Boolean) = True Then

                    l_string_Tmp = Right(CType(l_Date_Prenote.Year, String).Trim(), 2)
                    l_String_FileCreateDate = l_string_Tmp
                    l_string_Tmp = CType(l_Date_Prenote.Month, String).Trim()
                    l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                    l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                    l_string_Tmp = CType(l_Date_Prenote.Day, String).Trim()
                    l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                    l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                    l_string_FileName = l_String_FileCreateDate

                End If

                If CType(l_DataRow("FilenameExtension"), String) = "" Then
                    l_string_Tmp = String.Empty
                Else
                    l_string_Tmp = CType(l_DataRow("FilenameExtension"), String)
                    l_string_Tmp = "." + l_string_Tmp

                End If

                Me.SessionOutputFileName = l_string_FileName

                l_string_FileNameBak = l_string_FileName
                l_string_FileName = l_string_FileName + l_string_Tmp

                '34231 on 6 April 2006 to write in the webserver and copied to the File server. 
                'l_string_FullFileName = CType(l_DataRow("OutputDirectory"), String).Trim + "\" + l_string_FileName
                l_string_FullFileName = l_stringOutputDirectory + "\" + l_string_FileName
                ''commented by ruchi
                ''''arrayFileList(0, 0) = l_stringOutputDirectory
                ''''arrayFileList(0, 1) = l_stringOutputDirectory + "\" + l_string_FileName

                ''''arrayFileList(0, 2) = CType(l_DataRow("OutputDirectory"), String).Trim
                ''''arrayFileList(0, 3) = CType(l_DataRow("OutputDirectory"), String).Trim + "\" + l_string_FileName
                'end of comments
                'start of change by ruchi
                Dim l_temprow As DataRow
                l_temprow = l_datatable_FileList.NewRow

                l_temprow("SourceFolder") = l_stringOutputDirectory
                l_temprow("SourceFile") = l_stringOutputDirectory + "\" + l_string_FileName

                l_temprow("DestFolder") = Convert.ToString(l_DataRow("OutputDirectory"))
                l_temprow("DestFile") = Convert.ToString(l_DataRow("OutputDirectory")) + "\" + l_string_FileName
                l_datatable_FileList.Rows.Add(l_temprow)
                'end of change


                l_String_FileCreateDate = "_"
                l_string_Tmp = CType(l_Date_Prenote.Year, String).Trim()
                l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                l_string_Tmp = CType(l_Date_Prenote.Month, String).Trim()
                l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                l_string_Tmp = CType(l_Date_Prenote.Day, String).Trim()
                l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp + "_"

                l_string_Tmp = CType(l_Date_Prenote.Hour, String).Trim()
                l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

                l_string_Tmp = CType(l_Date_Prenote.Minute, String).Trim()
                l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

                l_string_Tmp = CType(l_Date_Prenote.Second, String).Trim()
                l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                l_string_FileNameBak = l_string_FileNameBak + l_String_FileCreateDate

                If CType(l_DataRow("FilenameExtension"), String) = "" Then
                    l_string_Tmp = String.Empty
                Else
                    l_string_Tmp = CType(l_DataRow("FilenameExtension"), String)
                    l_string_Tmp = "." + l_string_Tmp

                End If


                'Added by prasad YRS 5.0-632 : Test database output files need word "test" in them.
                If test_production = "TEST" Then
                    l_string_FileNameBak = l_string_FileNameBak.Remove(7, 5)
                    l_string_FileNameBak = l_string_FileNameBak + "_TEST"
                End If


                l_string_FileNameBak = l_string_FileNameBak + l_string_Tmp



                '34231 on 6 April 2006 to write in the webserver and copied to the File server. 
                'l_string_FileNameBak = CType(l_DataRow("OutputDirectory"), String).Trim + "\" + l_string_FileNameBak
                'commented by ruchi to implement datatable
                ''''arrayFileList(1, 0) = l_stringOutputDirectory
                ''''arrayFileList(1, 1) = l_stringOutputDirectory + "\" + l_string_FileNameBak

                ''''arrayFileList(1, 2) = CType(l_DataRow("OutputDirectory"), String).Trim
                ''''arrayFileList(1, 3) = CType(l_DataRow("OutputDirectory"), String).Trim + "\" + l_string_FileNameBak
                'end of comments
                'start of change by ruchi
                Dim l_temprow1 As DataRow
                l_temprow1 = l_datatable_FileList.NewRow

                l_temprow1("SourceFolder") = l_stringOutputDirectory
                l_temprow1("SourceFile") = l_stringOutputDirectory + "\" + l_string_FileNameBak

                l_temprow1("DestFolder") = CType(l_DataRow("OutputDirectory"), String).Trim
                l_temprow1("DestFile") = CType(l_DataRow("OutputDirectory"), String).Trim + "\" + l_string_FileNameBak
                l_datatable_FileList.Rows.Add(l_temprow1)
                'end of change

                l_string_FileNameBak = l_stringOutputDirectory + "\" + l_string_FileNameBak



                'If File.Exists(l_string_FullFileName.ToString) Then
                '    l_string_Tmp = Right(CType(System.DateTime.Now().Year, String).Trim(), 2)
                '    l_String_FileCreateDate = l_string_Tmp
                '    l_string_Tmp = CType(System.DateTime.Now().Month, String).Trim()
                '    l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                '    l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                '    l_string_Tmp = CType(System.DateTime.Now().Day, String).Trim()
                '    l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
                '    l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
                '    l_string_FileNameRen = l_String_FileCreateDate

                '    l_Integer_Index = l_string_FullFileName.IndexOfAny(".")

                '    If l_Integer_Index > 0 Then
                '        l_string_FileNameRen = Left(l_string_FullFileName, l_Integer_Index) + "_" + l_string_FileNameRen
                '    Else
                '        l_string_FileNameRen = l_string_FullFileName + "_" + l_string_FileNameRen
                '    End If

                '    If File.Exists(l_string_FileNameRen.ToString) Then
                '        MessageBox.Show(PlaceHolder1, "Pre-Notification File", "File " + l_string_FileNameRen.ToString + " already exists. Please delete or rename file.", MessageBoxButtons.Stop)
                '        Exit Function

                '    Else
                '        File.Copy(l_string_FullFileName.ToString, l_string_FileNameRen.ToString)
                '        Me.SessionOutputFileName = ""

                '    End If

                'End If

                CreateEFTFile(l_Date_Prenote, l_string_FullFileName, l_string_FileNameBak)

                Me.SessionOutputDirectory = CType(l_DataRow("OutputDirectory"), String).Trim

                GenerateTextFile = True

            End If

        Catch

            Throw

        End Try

    End Function
    Private Function PopulateControlsInToEmpty()
        Try
            Me.TextBoxFirstName.Text = String.Empty
            Me.TextBoxLastName.Text = String.Empty
            Me.TextBoxSSNo.Text = String.Empty
            Me.TextBoxActNo.Text = String.Empty
            Me.TextBoxActType.Text = String.Empty
            Me.TextBoxBankName.Text = String.Empty
            Me.TextBoxBankABA.Text = String.Empty
            Me.TextBoxMessage.Text = String.Empty

            Me.buttonEdit.Enabled = False
            Me.ButtonSelectAll.Enabled = False
            Me.buttonUpdateStatus.Enabled = False
            Me.buttonGeneratePreNote.Enabled = False
            Me.buttonPreNote.Enabled = False
            Me.buttonSave.Enabled = False
            Me.buttonCancel.Enabled = False
            Me.ButtonGo.Enabled = False

            Me.TextBoxFirstName.ReadOnly = True
            Me.TextBoxFirstName.ReadOnly = True
            Me.TextBoxLastName.ReadOnly = True
            Me.TextBoxSSNo.ReadOnly = True
            Me.TextBoxActNo.ReadOnly = True
            Me.TextBoxActType.ReadOnly = True
            Me.TextBoxBankName.ReadOnly = True
            Me.TextBoxBankABA.ReadOnly = True
            Me.DropDownListPTStatus.Enabled = True
            Me.TextBoxMessage.ReadOnly = True
            Me.TextBoxLookFor.ReadOnly = True

        Catch ex As Exception

        End Try
    End Function
    Private Function PopulateDataIntoControls(ByVal stringUnqueID As String)
        Dim l_DataSet As DataSet

        Dim l_Find_Datatrow As DataRow()
        Dim l_String_Search As String

        Try

            l_DataSet = g_dataset_dsPreNotificationList

            l_String_Search = "UniqueID = '" + stringUnqueID.Trim + "'"
            l_Find_Datatrow = g_dataset_dsPreNotificationList.Tables(0).Select(l_String_Search)

            For Each l_DataRow As DataRow In l_Find_Datatrow

                l_DataRow = l_Find_Datatrow(0)

                If Not l_DataRow Is Nothing Then

                    If (l_DataRow("First Name").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxFirstName.Text = String.Empty
                    Else
                        Me.TextBoxFirstName.Text = CType(l_DataRow("First Name"), String).Trim()
                    End If

                    If (l_DataRow("Last Name").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxLastName.Text = String.Empty
                    Else
                        Me.TextBoxLastName.Text = CType(l_DataRow("Last Name"), String).Trim()
                    End If

                    If (l_DataRow("ss no.").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxSSNo.Text = String.Empty
                    Else
                        Me.TextBoxSSNo.Text = CType(l_DataRow("ss no."), String).Trim()
                    End If

                    If (l_DataRow("BankAcctNumber").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxActNo.Text = String.Empty
                    Else
                        Me.TextBoxActNo.Text = CType(l_DataRow("BankAcctNumber"), String).Trim()
                    End If

                    If (l_DataRow("ActType").GetType.ToString = "System.DBNull") Then
                        TextBoxActType.Text = String.Empty
                    Else
                        TextBoxActType.Text = CType(l_DataRow("ActType"), String).Trim()
                    End If

                    If (l_DataRow("BankName").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxBankName.Text = String.Empty
                    Else
                        Me.TextBoxBankName.Text = CType(l_DataRow("BankName"), String).Trim()
                    End If

                    If (l_DataRow("BankAbaNumber").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxBankABA.Text = String.Empty
                    Else
                        Me.TextBoxBankABA.Text = CType(l_DataRow("BankAbaNumber"), String).Trim()
                    End If

                    If (l_DataRow("EftText").GetType.ToString = "System.DBNull") Then
                        Me.TextBoxMessage.Text = String.Empty
                    Else
                        Me.TextBoxMessage.Text = CType(l_DataRow("EftText"), String).Trim()
                    End If

                    If Me.SessionFormMode = FormMode.INNew Then

                        Select Case CType(l_DataRow("Eft Status"), String).Trim()
                            Case "D"
                                Me.DropDownListPTStatus.SelectedValue = "Changed"
                            Case "F"
                                Me.DropDownListPTStatus.SelectedValue = "Failed"
                            Case "O"
                                Me.DropDownListPTStatus.SelectedValue = "OK"
                            Case "P"
                                Me.DropDownListPTStatus.SelectedValue = "Pending"
                            Case "V"
                                Me.DropDownListPTStatus.SelectedValue = "Verify"
                        End Select
                    Else

                        Select Case CType(l_DataRow("Eft Status"), String).Trim()

                            Case "F"
                                Me.DropDownListPTStatus.SelectedValue = "Failed"

                            Case "P"
                                Me.DropDownListPTStatus.SelectedValue = "Pending"
                            Case Else

                                Me.DropDownListPTStatus.SelectedValue = ""

                        End Select

                    End If
                End If

            Next

        Catch ex As Exception
            Throw
        End Try


    End Function
    Private Function CreateEFTFile(ByVal parameter_Date_Prenote As DateTime, ByVal Paramater_String_Filename As String, ByVal Paramater_String_FilenameBak As String) As Boolean

        Const C_BATCHCOUNT As String = "000001"
        Const C_BATCHNUMBER As String = "0000001"
        Const C_BLOCKINGFACTOR As String = "10"
        Const C_COMPANYENTRYDESC As String = "ANNUITY_PY"
        Const C_COMPANYID As String = "1135562401"
        Const C_COMPANYNAME As String = "YMCA Retirement Fund             "
        Const C_FILEFORMATCODE As String = "1"
        Const C_FILEIDMODIFIER As String = "A"
        Const C_IMMEDIATEDEST As String = " 071000152"
        Const C_IMMEDIATEDESTNAME As String = "Northern Trust Company "
        Const C_IMMEDIATEORIGIN As String = "1135562401"
        Const C_IMMEDORIGINNAME As String = "YMCA Retirement Fund   "
        Const C_ORIGINATINGDFIID As String = "07100015"
        Const C_ORIGINATORSTATUSCODE As String = "1"
        Const C_PRIORITYCODE As String = "01"
        Const C_RECORDSIZE As String = "094"
        Const C_ROWCODE1 As String = "1"
        Const C_ROWCODE5 As String = "5"
        Const C_ROWCODE8 As String = "8"
        Const C_ROWCODE9 As String = "9"
        Const C_ROWCODEH As String = "H"
        Const C_SERVICECLASSCODE As String = "200"
        Const C_STANDARDECCODE As String = "PPD"
        Const C_COMPANYNAMESHORT As String = "YMCA Retire Fund"
        Const C_ADDENDAINDICATOR As String = "0"
        Const C_ROWCODE6 As String = "6"
        Const C_ROWCODEA As String = "A"
        Const C_ROWCODEB As String = "B"
        Const C_ROWCODEC As String = "C"
        Const C_ROWCODER As String = "R"
        Const C_ROWCODET As String = "T"
        Const C_TRACENUMBER As String = "000000000000000"
        Const C_TRANSACTIONCODE As String = "SA"
        Const C_YMCABANKACCOUNT As String = "0030362081"
        Const C_ROWCODE7_ADDENDA As String = "7"
        Const C_ADDENDATYPE_ADDENDA As String = "5"
        Const C_DEDTEXT As String = "Ded       "
        Const C_EXPDIVTEXT As String = "Experience Dividend - Taxable      "
        Const C_GROSSTEXT As String = "Gross     "
        Const C_NETTEXT As String = "Net       "
        Const C_NONTAXTEXT As String = "Non-Taxable     "
        Const C_REGALLOWTAXTEXT As String = "Regular Allowance: Taxable         "
        Const C_ROWCODED As String = "D"
        Const C_ROWCODEE As String = "E"
        Const C_YTDDEDTEXT As String = "YTD Ded   "
        Const C_YTDGROSSTEXT As String = "YTD Gross "
        Const C_YTDNETTEXT As String = "YTD Net   "

        Const C_ROWCODEF As String = "F"
        Const C_ROWCODEG As String = "G"
        'START: MMR | 2016.11.25 | YRS-AT-3213 | Commented as not used anywhere
        'Const C_POSPAY_EFT_LINE1 As String = "$$ADD ID=YMCRE1B BID='9902827 A" 
        'Const C_POSPAY_LINE1_SUFFIX As String = "RPI'"
        'Const C_EFT_LINE1_SUFFIX As String = "CHI'"
        'END: MMR | 2016.11.25 | YRS-AT-3213 | Commented as not used anywhere

        Dim l_DataRow As DataRow
        Dim l_DataSet As DataSet

        Dim l_String_EFTFile As String
        'Dim l_String_CustId As String 'MMR | 2016.11.25 | YRS-AT-3213 | Commented as not required
        Dim l_String_RecTypeCode As String
        Dim l_String_PriorityCode As String
        Dim l_String_ImmedDest As String
        Dim l_String_ImmedOrig As String
        Dim l_String_FileCreateDate As String
        Dim l_String_FileCreateTime As String
        Dim l_String_FileID As String
        Dim l_String_RecSize As String
        Dim l_String_BlockFactor As String
        Dim l_String_FormatCode As String
        Dim l_String_DestName As String
        Dim l_String_OrigName As String

        Dim l_string_Output As String
        Dim l_string_Tmp As String

        ' File Section
        Dim lc_filename As String


        Dim l_StreamWriter_File As StreamWriter = File.CreateText(Paramater_String_Filename.ToString)
        Dim l_StreamWriter_FileBak As StreamWriter = File.CreateText(Paramater_String_FilenameBak.ToString)

        Dim l_String_ServiceClass As String
        Dim l_String_CompanyName As String
        Dim l_String_Filler As String
        Dim l_String_CompanyID As String
        Dim l_String_EntryClass As String
        Dim l_String_EntryDesc As String
        Dim l_String_DescrDate As String
        Dim l_String_EntryDate As String
        Dim l_String_Filler2 As String
        Dim l_String_OrigStatus As String
        Dim l_String_OrigDFI As String
        Dim l_String_BatchID As String

        Dim l_String_TransCode As String
        Dim l_String_DFIID As String
        Dim l_String_DFIAccount As String
        Dim l_String_Amount As String
        Dim l_String_IndIdNum As String
        Dim l_String_CheckDigit As String

        Dim l_String_IndName As String
        Dim l_String_Filler3 As String
        Dim l_String_Filler4 As String
        Dim l_Long_HashSum As Long

        Dim l_Long_RecordCount As Long
        Dim i As Long

        Dim l_String_Service As String
        Dim l_String_EntryCount As String
        Dim l_String_EntryHash As String
        Dim l_String_DebitDollar As String
        Dim l_String_CreditDollar As String
        Dim l_String_Filler5 As String
        Dim l_String_BatchNum As String

        Dim l_string_BatchCount As String
        Dim l_Long_BlockCount As Long
        Dim l_string_BlockCount As String
        Dim l_long_RecCount As Long
        Dim l_string_AddendaCount As String
        Dim l_string_Reserved As String
        Dim l_long_Tmp As Decimal

        Try
            'l_String_CustId = "$$ADD ID=YMCRE1B BID='9902827 ACHI'" 'MMR | 2016.11.25 | YRS-AT-3213 | Commented as it should not be displayed in output
            l_String_RecTypeCode = C_ROWCODE1           '1'
            l_String_PriorityCode = C_PRIORITYCODE     '01'
            l_String_ImmedDest = C_IMMEDIATEDEST      ' 071000152'
            l_String_ImmedOrig = C_IMMEDIATEORIGIN    '1135562401'

            'START : ML |YRS-AT-3352 | Get Date time value in string to resolve 2 digit padding issue
            'l_string_Tmp = Right(CType(parameter_Date_Prenote.Year, String).Trim(), 2)
            'l_String_FileCreateDate = l_string_Tmp
            'l_string_Tmp = CType(parameter_Date_Prenote.Month, String).Trim()
            'l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            'l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp
            'l_string_Tmp = CType(parameter_Date_Prenote.Day, String).Trim()
            'l_string_Tmp = l_string_Tmp.PadLeft(2, "0")
            'l_String_FileCreateDate = l_String_FileCreateDate + l_string_Tmp

            'l_string_Tmp = CType(parameter_Date_Prenote.Hour, String).Trim()
            'l_String_FileCreateTime = l_string_Tmp
            'l_string_Tmp = CType(parameter_Date_Prenote.Minute, String).Trim()
            'l_String_FileCreateTime = l_String_FileCreateTime + l_string_Tmp


            l_String_FileCreateDate = parameter_Date_Prenote.ToString("yyMMdd")
            l_String_FileCreateTime = parameter_Date_Prenote.ToString("hhmm")
            'END : ML |YRS-AT-3352 | Get Date time value in string to resolve 2 digit padding issue

            l_String_FileID = C_ROWCODEA       'A'
            l_String_RecSize = C_RECORDSIZE      '094'
            l_String_BlockFactor = C_BLOCKINGFACTOR     '10'
            l_String_FormatCode = C_FILEFORMATCODE     '1'
            l_String_DestName = "Northern Trust Company"
            l_String_DestName = l_String_DestName.PadRight(23)

            l_String_OrigName = "YMCA Retirement Fund"
            l_String_OrigName = l_String_OrigName.PadRight(23)

            'START: MMR | 2016.11.25 | YRS-AT-3213 | Commented as it should not be displayed in output
            'l_StreamWriter_File.WriteLine(l_String_CustId)
            'l_StreamWriter_FileBak.WriteLine(l_String_CustId)
            'END: MMR | 2016.11.25 | YRS-AT-3213 | Commented as it should not be displayed in output

            l_string_Output = l_String_RecTypeCode + _
                              l_String_PriorityCode + _
                              l_String_ImmedDest + _
                              l_String_ImmedOrig + _
                              l_String_FileCreateDate + _
                              l_String_FileCreateTime + _
                              l_String_FileID + _
                              l_String_RecSize + _
                              l_String_BlockFactor + _
                              l_String_FormatCode + _
                              l_String_DestName + _
                              l_String_OrigName + Space(8)

            l_StreamWriter_File.WriteLine(l_string_Output)
            l_StreamWriter_FileBak.WriteLine(l_string_Output)

            l_String_RecTypeCode = C_ROWCODE5      ' '5'
            l_String_ServiceClass = C_SERVICECLASSCODE      '200'
            l_String_CompanyName = "YMCA Retire Fund"


            l_String_CompanyName = l_String_CompanyName.PadRight(16)

            l_String_Filler = Space(20)
            l_String_CompanyID = C_IMMEDIATEORIGIN        '1135562401'
            l_String_EntryClass = C_STANDARDECCODE      'PPD'
            l_String_EntryDesc = C_COMPANYENTRYDESC       'ANNUITY_PY'
            l_String_DescrDate = l_String_FileCreateDate
            l_String_EntryDate = l_String_FileCreateDate
            l_String_Filler2 = Space(3)
            l_String_OrigStatus = C_ROWCODE1       '1'
            l_String_OrigDFI = C_ORIGINATINGDFIID     '07100015'
            l_String_BatchID = C_BATCHNUMBER      '0000001' 

            l_string_Output = l_String_RecTypeCode + _
                              l_String_ServiceClass + _
                              l_String_CompanyName + _
                              l_String_Filler + _
                              l_String_CompanyID + _
                              l_String_EntryClass + _
                              l_String_EntryDesc + _
                              l_String_DescrDate + _
                              l_String_EntryDate + _
                              l_String_Filler2 + _
                              l_String_OrigStatus + _
                              l_String_OrigDFI + _
                              l_String_BatchID


            l_StreamWriter_File.WriteLine(l_string_Output)
            l_StreamWriter_FileBak.WriteLine(l_string_Output)

            l_DataSet = g_dataset_dsPreNotificationList

            If l_DataSet Is Nothing Then
                CreateEFTFile = False
            End If

            For i = 0 To l_DataSet.Tables(0).Rows.Count - 1

                l_DataRow = l_DataSet.Tables(0).Rows(i)

                If Not l_DataRow Is Nothing Then

                    If CType(l_DataRow("Eft Status"), String) = "V" And CType(l_DataRow("Selected"), Boolean) = True Then

                        l_String_RecTypeCode = Space(0)
                        l_String_TransCode = Space(0)
                        l_String_DFIID = Space(0)
                        l_String_DFIAccount = Space(0)
                        l_String_Amount = Space(0)
                        l_String_IndIdNum = Space(0)
                        l_String_CheckDigit = Space(0)

                        ' set values
                        l_String_RecTypeCode = C_ROWCODE6    '&& '6'

                        If (l_DataRow("EftTypeCode").GetType.ToString = "System.DBNull") Then
                            l_String_TransCode = String.Empty
                        Else
                            l_String_TransCode = CType(l_DataRow("EftTypeCode"), String).Trim()
                        End If

                        l_String_TransCode = IIf(l_String_TransCode.Trim() = "22", "23", "33")

                        If (l_DataRow("BankAbaNumber").GetType.ToString = "System.DBNull") Then
                            l_String_DFIID = String.Empty
                        Else
                            l_String_DFIID = CType(l_DataRow("BankAbaNumber"), String).Trim()
                            l_String_DFIID = l_String_DFIID.Trim()
                            l_String_DFIID = Left(l_String_DFIID, 8)
                            l_String_DFIID = l_String_DFIID.PadRight(8)
                        End If

                        If (l_DataRow("BankAbaNumber").GetType.ToString = "System.DBNull") Then
                            l_String_CheckDigit = String.Empty
                        Else
                            l_String_CheckDigit = CType(l_DataRow("BankAbaNumber"), String).Trim()
                            l_String_CheckDigit = l_String_CheckDigit.Trim()
                            l_String_CheckDigit = Right(l_String_CheckDigit, 1)
                        End If

                        If (l_DataRow("BankAcctNumber").GetType.ToString = "System.DBNull") Then
                            l_String_DFIAccount = String.Empty
                        Else
                            l_String_DFIAccount = CType(l_DataRow("BankAcctNumber"), String).Trim()
                            l_String_DFIAccount = l_String_DFIAccount.Trim()
                            l_String_DFIAccount = l_String_DFIAccount.PadRight(17)
                        End If

                        l_String_Amount = String.Empty

                        l_String_Amount = l_String_Amount.PadRight(10, "0")

                        If (l_DataRow("SS No.").GetType.ToString = "System.DBNull") Then
                            l_String_IndIdNum = String.Empty
                        Else
                            'FundIdNo
                            'by Aparna -YREN-3075 To replace SSNO With Fundo No
                            ' l_String_IndIdNum = CType(l_DataRow("SS No."), String).Trim()
                            l_String_IndIdNum = CType(l_DataRow("FundIdNo"), String).Trim()
                            l_String_IndIdNum = l_String_IndIdNum.Trim()
                            l_String_IndIdNum = l_String_IndIdNum.PadRight(15)
                        End If

                        If (l_DataRow("First Name").GetType.ToString = "System.DBNull") Then
                            l_String_IndName = String.Empty
                        Else
                            l_string_Tmp = CType(l_DataRow("First Name"), String).Trim()
                            l_string_Tmp = l_string_Tmp.Trim()
                            l_string_Tmp = Left(l_string_Tmp, 1)
                            l_string_Tmp = l_string_Tmp.PadRight(2)
                            l_String_IndName = l_string_Tmp
                        End If
                        If (l_DataRow("Last Name").GetType.ToString = "System.DBNull") Then
                            l_String_IndName = l_String_IndName
                        Else
                            l_string_Tmp = CType(l_DataRow("Last Name"), String).Trim()
                            l_string_Tmp = l_string_Tmp.Trim()
                            l_String_IndName = l_String_IndName.ToString() + l_string_Tmp.ToString()
                            l_String_IndName = l_String_IndName.PadRight(22)
                        End If

                        l_String_IndName = l_String_IndName.ToString()

                        l_String_Filler3 = Space(2)
                        l_String_Filler4 = "0"
                        l_String_Filler4 = l_String_Filler4.PadRight(16, "0")
                        l_String_Filler4 = l_String_Filler4.ToString()

                        If (l_DataRow("BankAbaNumber").GetType.ToString = "System.DBNull") Then
                            l_string_Tmp = String.Empty
                        Else
                            l_string_Tmp = CType(l_DataRow("BankAbaNumber"), String).Trim()
                            l_string_Tmp = Left(l_string_Tmp, 8)

                        End If

                        l_Long_HashSum = l_Long_HashSum + Val(l_string_Tmp)

                        l_string_Output = l_String_RecTypeCode + _
                                      l_String_TransCode + _
                                      l_String_DFIID + _
                                      l_String_CheckDigit + _
                                      l_String_DFIAccount + _
                                      l_String_Amount + _
                                      l_String_IndIdNum + _
                                      l_String_IndName + _
                                      l_String_Filler3 + _
                                      l_String_Filler4

                        l_StreamWriter_File.WriteLine(l_string_Output)
                        l_StreamWriter_FileBak.WriteLine(l_string_Output)

                        l_Long_RecordCount = l_Long_RecordCount + 1
                    End If
                End If

            Next i

            ' set values
            l_String_RecTypeCode = C_ROWCODE8    ' '8'
            l_String_ServiceClass = C_SERVICECLASSCODE  ' '200'
            l_String_EntryCount = l_Long_RecordCount.ToString()
            l_String_EntryCount = l_String_EntryCount.Trim()
            l_String_EntryCount = l_String_EntryCount.PadLeft(6, "0")
            l_String_EntryHash = l_Long_HashSum.ToString()
            l_String_EntryHash = Right(l_String_EntryHash.Trim(), 10)
            l_String_EntryHash = l_String_EntryHash.PadLeft(10, "0")

            '&& Ignore the higher order bits for hash

            l_String_DebitDollar = ""
            l_String_DebitDollar = l_String_DebitDollar.PadRight(12, "0")
            l_String_CreditDollar = ""
            l_String_CreditDollar = l_String_CreditDollar.PadRight(12, "0")

            l_String_CompanyID = C_COMPANYID    '&& '1135562401'
            l_String_Filler5 = Space(25)
            l_String_OrigDFI = C_ORIGINATINGDFIID   '&& '07100015'
            l_String_BatchNum = C_BATCHNUMBER   '&& '0000001'

            '* output file
            l_string_Output = l_String_RecTypeCode + _
                              l_String_ServiceClass + _
                              l_String_EntryCount + _
                              l_String_EntryHash + _
                              l_String_DebitDollar + _
                              l_String_CreditDollar + _
                              l_String_CompanyID + _
                              l_String_Filler5 + _
                              l_String_OrigDFI + _
                              l_String_BatchNum

            l_StreamWriter_File.WriteLine(l_string_Output)
            l_StreamWriter_FileBak.WriteLine(l_string_Output)

            '* file control record (9)

            '* initailize
            l_String_RecTypeCode = Space(0)
            l_string_BatchCount = Space(0)
            l_string_BlockCount = Space(0)
            l_string_AddendaCount = Space(0)
            l_string_Reserved = Space(39)

            '* set values
            l_String_RecTypeCode = C_ROWCODE9  '&& '9'
            l_string_BatchCount = C_BATCHCOUNT  '&& '000001'
            l_long_RecCount = l_Long_RecordCount + 4

            l_long_Tmp = (l_long_RecCount Mod 10)

            l_Long_BlockCount = IIf(l_long_Tmp = 0, l_long_RecCount, l_long_RecCount + 1)

            l_string_BlockCount = l_Long_BlockCount.ToString()
            l_string_BlockCount = l_string_BlockCount.Trim()
            l_string_BlockCount = l_string_BlockCount.PadLeft(6, "0")

            l_string_AddendaCount = l_Long_RecordCount.ToString()
            l_string_AddendaCount = l_string_AddendaCount.Trim()
            l_string_AddendaCount = l_string_AddendaCount.PadLeft(8, "0")

            '* output file
            l_string_Output = l_String_RecTypeCode + _
                       l_string_BatchCount + _
                       l_string_BlockCount + _
                       l_string_AddendaCount + _
                       l_String_EntryHash + _
                       l_String_DebitDollar + _
                       l_String_CreditDollar + _
                       l_string_Reserved

            l_StreamWriter_File.WriteLine(l_string_Output)
            l_StreamWriter_FileBak.WriteLine(l_string_Output)

            l_StreamWriter_File.Close()
            l_StreamWriter_FileBak.Close()

        Catch

            Throw

        End Try

    End Function

    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        Dim l_DataRow As DataRow
        Dim l_DataSet As DataSet
        Dim i As Long
        Dim l_bool_val As Boolean
        'Dim l_Cache As CacheManager
        Dim l_CheckBox As CheckBox
        Dim l_button_Select As ImageButton

        Try
            'Ragesh 34231 02/02/06 Cache to Session
            'l_Cache = CacheFactory.GetCacheManager()
            'Ragesh 34231 02/02/06 Cache to Session

            If Me.ButtonSelectAll.Text.Trim() = "Select All" Then

                Me.ButtonSelectAll.Text = "Select None"
                l_bool_val = True

            Else
                Me.ButtonSelectAll.Text = "Select All"
                l_bool_val = False

            End If


            'Ragesh 34231 02/02/06 Cache to Session
            'g_dataset_dsPreNotificationList = CType(l_Cache.GetData("PreNotificationLists"), DataSet)
            g_dataset_dsPreNotificationList = Me.SessionDataSetPreNotificationList
            'Ragesh 34231 02/02/06 Cache to Session

            l_DataSet = g_dataset_dsPreNotificationList

            If Not l_DataSet Is Nothing Then

                For i = 0 To l_DataSet.Tables(0).Rows.Count - 1

                    l_DataRow = l_DataSet.Tables(0).Rows(i)
                    l_DataRow("Selected") = l_bool_val

                Next i

                Me.SessionDataSetPreNotificationList = g_dataset_dsPreNotificationList


                'Ragesh 34231 02/02/06 Cache to Session
                g_bool_AddFlag = True
                PopulateData(LoadDatasetMode.Session)



            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)

        End Try

    End Sub

    Private Sub buttonPreNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPreNote.Click

        Try

            PopulateData(LoadDatasetMode.Session)
            If YMCARET.YmcaBusinessObject.Prenotification.SaveEFTStatus(g_dataset_dsPreNotificationList) Then
                PopulateData(LoadDatasetMode.Table)

            End If
            'Call ReportViewer.aspx 
            Session("strReportName") = "Pre-Notification"
            Dim popupScript As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
            CheckReadOnlyMode() 'Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub buttonGeneratePreNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonGeneratePreNote.Click
        Dim l_dataSet As DataSet
        Dim l_dataRow As DataRow
        Dim l_date_PreNoteBegan As DateTime
        'Dim l_Cache As CacheManager
        Dim i As Long

        ''Dim arrayFileList(4, 4) As String
        Dim l_datatable_FileList As New DataTable
        Dim popupScript As String
        Try
            ''Adding columns to DataTable
            Dim SourceFolder As DataColumn = New DataColumn("SourceFolder", System.Type.GetType("System.String"))
            Dim SourceFile As DataColumn = New DataColumn("SourceFile", System.Type.GetType("System.String"))
            Dim DestFolder As DataColumn = New DataColumn("DestFolder", System.Type.GetType("System.String"))
            Dim DestFile As DataColumn = New DataColumn("DestFile", System.Type.GetType("System.String"))

            l_datatable_FileList.Columns.Add(SourceFolder)
            l_datatable_FileList.Columns.Add(SourceFile)
            l_datatable_FileList.Columns.Add(DestFolder)
            l_datatable_FileList.Columns.Add(DestFile)

            PopulateData(LoadDatasetMode.Session)

            l_date_PreNoteBegan = System.DateTime.Now

            If GenerateTextFile(l_date_PreNoteBegan, l_datatable_FileList) = True Then

                l_dataSet = g_dataset_dsPreNotificationList
                Session("FTFileList") = Nothing
                Session("FTFileList") = l_datatable_FileList

                ' Call the calling of the ASPX to copy the file.
                Try
                    'Server.Execute("FT\CopyFilestoFileServer.aspx")
                    'Response.Redirect("FT\CopyFilestoFileServer.aspx", False)

                    'Session("FTProgress") = True
                    popupScript = "<script language='javascript'>" & _
                    "var a = window.open('FT\\CopyFilestoFileServer.aspx', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                    "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                        Page.RegisterStartupScript("PopupScript1", popupScript)
                    End If

                    'Session("FTProgress") = False
                    'popupScript = "<script language='javascript'>" & _
                    '"a.location.href='FT\\CopyFilestoFileServer.aspx';" & _
                    '"</script>"
                    'If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    '    Page.RegisterStartupScript("PopupScript2", popupScript)
                    'End If
                    'Session("FTProgress") = Nothing
                Catch ex As Exception
                    MessageBox.Show(PlaceHolder1, "Error While Writing File", ex.Message, MessageBoxButtons.OK)
                End Try

                If Not l_dataSet Is Nothing Then

                    For i = 0 To l_dataSet.Tables(0).Rows.Count - 1

                        l_dataRow = l_dataSet.Tables(0).Rows(i)

                        If CType(l_dataRow("Eft Status"), String).Trim() = "V" And CType(l_dataRow("Selected"), Boolean) = True Then
                            l_dataRow("Eft Status") = "P"
                            l_dataRow("EftProcessDate") = l_date_PreNoteBegan

                        End If

                    Next i

                    If YMCARET.YmcaBusinessObject.Prenotification.SaveAndInsertEFTStatusValues(l_dataSet, l_date_PreNoteBegan, Me.SessionOutputDirectory, Me.SessionOutputFileName) Then
                        IdentiyfyState()
                        PopulateData(LoadDatasetMode.Table)

                    End If

                    MessageBox.Show(PlaceHolder1, "Pre-Notification File", "Successfully generated the Prenote file", MessageBoxButtons.OK)

                End If
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub buttonUpdateStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonUpdateStatus.Click
        'Dim l_Cache As CacheManager
        Dim l_dataset As DataSet
        Dim l_datarow As DataRow
        Dim i As Long
        Dim l_processlog As Boolean

        Try
            PopulateData(LoadDatasetMode.Session)

            If VerifyCurrentValues(True, EnableMode.UPDATE_EFT_STATUS) Then

                l_dataset = g_dataset_dsPreNotificationList

                If Not l_dataset Is Nothing Then

                    For i = 0 To l_dataset.Tables(0).Rows.Count - 1

                        l_datarow = l_dataset.Tables(0).Rows(i)

                        If Not l_datarow Is Nothing Then

                            If CType(l_datarow("Eft Status"), String).Trim() = "P" And CType(l_datarow("Selected"), Boolean) = True Then
                                l_datarow("Eft Status") = "O"
                                l_datarow("EftProcessDate") = System.DateTime.Now()

                            End If
                        End If

                    Next i

                    If CType(Me.SessionProcessDate, String).Trim = "1/1/1800" Then
                        l_processlog = False
                    Else
                        l_processlog = True
                    End If


                    If YMCARET.YmcaBusinessObject.Prenotification.UpdateEFTStatus(l_dataset, Me.SessionProcessDate, l_processlog) Then
                        'Me.SessionSelectedIndex = 0
                        'g_bool_AddFlag = True
                        IdentiyfyState()
                        Me.SessionSelectedIndex = 0    'Shashi Shekhar Singh:09-07-2010: BT - 555
                        PopulateData(LoadDatasetMode.Table)


                    End If
                End If

            Else
                MessageBox.Show(PlaceHolder1, "Update EFT status", "Please Check atleast one row with status 'P' to update EFT status.", MessageBoxButtons.Stop)

            End If
        Catch ex As Exception

            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub


    Private Sub DataGridPreNotification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridPreNotification.SelectedIndexChanged
        Try

            'If Me.SessionEditMode = True And DataGridPreNotification.SelectedIndex <> Me.SessionSelectedIndex Then
            If Me.SessionEditMode = True And DataGridPreNotification.SelectedItem.Cells(6).Text.Trim() <> Me.SessionSelectedUniqueID Then
                PopulateData(LoadDatasetMode.Session)
                'PopulateDataIntoControls(Me.SessionSelectedIndex)
                MessageBox.Show(PlaceHolder1, "Please Note", "Please Save or Cancel the Edit operation before moving to new item.", MessageBoxButtons.Stop)
            Else
                Me.SessionSelectedUniqueID = DataGridPreNotification.SelectedItem.Cells(6).Text.Trim()
                'Me.SessionSelectedIndex = DataGridPreNotification.SelectedIndex
                PopulateData(LoadDatasetMode.Session)
                'PopulateDataIntoControls(Me.SessionSelectedIndex)
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub buttonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSave.Click

        'Dim l_Cache As CacheManager
        Dim l_dataset As DataSet
        Dim l_String_EFTStatus As String
        Dim l_String_EftText As String

        Dim l_Find_Datatrow As DataRow()
        Dim l_String_Search As String

        Dim i As Long

        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Select Case Me.DropDownListPTStatus.SelectedValue

                Case "Changed"
                    l_String_EFTStatus = "D"
                Case "Failed"
                    l_String_EFTStatus = "F"
                Case "OK"
                    l_String_EFTStatus = "O"
                Case "Pending"
                    l_String_EFTStatus = "P"
                Case "Verify"
                    l_String_EFTStatus = "V"

            End Select

            l_String_EftText = Me.TextBoxMessage.Text.Trim()

            PopulateData(LoadDatasetMode.Session)

            l_dataset = g_dataset_dsPreNotificationList

            If Not l_dataset Is Nothing Then

                l_String_Search = "UniqueID = '" + Me.SessionSelectedUniqueID.Trim + "'"
                l_Find_Datatrow = g_dataset_dsPreNotificationList.Tables(0).Select(l_String_Search)

                For Each l_DataRow As DataRow In l_Find_Datatrow

                    If Not l_DataRow Is Nothing Then

                        l_DataRow("EftProcessDate") = DBNull.Value
                        l_DataRow("Eft Status") = l_String_EFTStatus.ToString()
                        l_DataRow("EftText") = l_String_EftText.ToString()

                        Me.SessionEditMode = False

                        If YMCARET.YmcaBusinessObject.Prenotification.SaveEFTStatus(l_dataset) Then
                            PopulateData(LoadDatasetMode.Table)

                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub buttonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonEdit.Click
        Try
            Me.SessionEditMode = True
            PopulateData(LoadDatasetMode.Session)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click
        Try
            Me.SessionEditMode = False
            PopulateData(LoadDatasetMode.Session)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        'Dim l_Cache As CacheManager
        'Dim l_String_CloseWindowPreNotification As String = "<script language='javascript'>" & _
        '                                            "window.close()" & _
        '                                            "</script>"

        Try
            'l_Cache = CacheFactory.GetCacheManager()
            'l_Cache.Remove("PreNotificationLists")
            Response.Redirect("MainWebForm.aspx", False)
            Me.SessionSelectedIndex = Nothing
            Me.SessionSelectedUniqueID = Nothing

            'If (Not Me.IsStartupScriptRegistered("CloseWindowPreNotification")) Then
            '    Page.RegisterStartupScript("CloseWindowPreNotification", l_String_CloseWindowPreNotification)
            'End If

        Catch

        End Try

    End Sub

    Private Sub ButtonGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGo.Click
        Dim l_dataset As DataSet
        Dim l_DataRow As DataRow
        Dim i As Long
        Dim len As Integer

        Try

            PopulateData(LoadDatasetMode.Session)

            If Not Me.TextBoxLookFor.Text.Trim() = "" Then

                l_dataset = g_dataset_dsPreNotificationList

                If Not l_dataset Is Nothing Then

                    For i = 0 To l_dataset.Tables(0).Rows.Count - 1

                        l_DataRow = l_dataset.Tables(0).Rows(i)

                        If Not l_DataRow Is Nothing Then

                            len = Me.TextBoxLookFor.Text.Trim().Length()

                            If Left(l_DataRow("SS No."), len) = Left(Me.TextBoxLookFor.Text.Trim(), len) Then

                                Me.SessionSelectedUniqueID = CType(l_DataRow("UniqueID"), Guid).ToString.Trim

                                PopulateData(LoadDatasetMode.Session)

                                Exit For

                            End If

                        End If

                    Next i

                End If
            End If


        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridPreNotification_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridPreNotification.SortCommand
        Try
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression


            If Me.SessionEditMode = True Then
                PopulateData(LoadDatasetMode.Session)
                MessageBox.Show(PlaceHolder1, "Please Note", "Please Save or Cancel the Edit operation before moving to any operation.", MessageBoxButtons.Stop)
            Else
                Me.PopulateData(LoadDatasetMode.Session)

                If Not HelperFunctions.isEmpty(g_dataset_dsPreNotificationList) Then 'Added by shashi:2009-12-14: To resolve the issue no-BT 1067
                    dv = g_dataset_dsPreNotificationList.Tables(0).DefaultView

                    Me.SessionDataSetPreNotificationList = g_dataset_dsPreNotificationList

                    dv.Sort = SortExpression
                    If Not Session("PreNotificationListSort") Is Nothing Then
                        If SortExpression + " ASC" = Session("PreNotificationListSort").ToString.Trim Then
                            dv.Sort = SortExpression + " DESC"
                        Else
                            dv.Sort = SortExpression + " ASC"
                        End If

                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If

                    Session("PreNotificationListSort") = dv.Sort
                    'Me.SessionSelectedUniqueID = ""
                    Me.PopulateData(LoadDatasetMode.Session)
                End If
            End If


        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub DataGridPreNotification_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridPreNotification.ItemDataBound
        Dim l_button_Select As ImageButton

        Try
            l_button_Select = e.Item.FindControl("ImageButtonSelect")

            If (Not l_button_Select Is Nothing) Then
                l_button_Select.ImageUrl = "images\select.gif"
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub


    'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            DataGridPreNotification.Enabled = False
            DataGridPreNotification.ToolTip = tooltip
            buttonGeneratePreNote.Enabled = False
            buttonGeneratePreNote.ToolTip = tooltip
            buttonUpdateStatus.Enabled = False
            buttonUpdateStatus.ToolTip = tooltip
            ButtonSelectAll.ToolTip = tooltip
            ButtonSelectAll.Enabled = False
            ButtonGo.Enabled = False
            ButtonGo.ToolTip = tooltip
            buttonCancel.Enabled = False
            buttonCancel.ToolTip = tooltip
            buttonEdit.Enabled = False
            buttonEdit.ToolTip = tooltip

        End If
    End Sub
    'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

End Class
