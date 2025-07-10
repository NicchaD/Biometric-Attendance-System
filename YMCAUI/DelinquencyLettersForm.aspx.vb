'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		            :	YMCA_YRS
' FileName			            :	DelinquencyLettersForm.aspx.vb
' Author Name		            :	Aparna Samala
' Employee ID		            :	34773
' Email				            :	aparna.samala@icici-infotech.com    
' Contact No		            :	8609
' Creation Time		            :	7th Oct-2006
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	To send Emails to Delinquent Ymcas
' Cache-Session   
'*******************************************************************************
'Changed By:            On:             IssueId: 
'Aparna Samala          14/12/2006      YREN-2994
'******************************************************************************************************************************************************
'Modification History
'******************************************************************************************************************************************************
'Modified By                        Date            Desription
'******************************************************************************************************************************************************
'Ashutosh P and Aparna S            22-Mar-2007     DelinquencyLetters will be generated from new IDM class.
'Aparna Samala                      20-Apr-2007     Change in folder structure
'Ashutosh Patil                     22-May-2007     Change in Email Functionality
'Aparna Samala                      28-Jun-2007     Error occured when To Email id was missing,Included checks for existence of folders/files
'Aparna Samala                      07/07/07        check if the Emailcongiuration details are retireved or not...dataset is available or not
'Nikunj Patel                       2008.04.04      YRPS-4711 - Showing Metro and Urban YMCA's in a different color
'Shashi Shekhar                     2008.10.23      Label Text change
'Neeraj Singh                       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Ashish Srivastava                  2010.03.22      Log the exception in exception log file
'Priya                              2010-06-03      Changes made for enhancement in vs-2010 
'Imran                              25/Oct/2010     BT:669-Handle unexpected errors & Some sort of a message to indicate which YMCA's it failed.
'Imran                              26/Apr/2011     BT:821-YRS 5.0-1317: Changes to delinquescy letters 
'Ashish Srivastava                  2011.12.05      BT-752 : Session not getting clear.
'Sanjay R                           2012.08.29      BT-1064:YRS 5.0-1643-Changes to 11th business day letter.
'Anudeep                            2013-12-16      BT:2311-13.3.0 Observations
'Anudeep                            2015-02-10      BT:2738:YRS 5.0-2456:YERDI3I-2319: YERDI YMCA Officer Information Update function - YRS Changes
'Manthan Rajguru                    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'******************************************************************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text
Imports System.IO
Imports System.Net


Public Class DelinquencyLettersForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DelinquencyLettersForm.aspx")
    'End issue id YRS 5.0-940
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridDelinquency As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonProcess As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents dgLetterType As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblLetterType As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents btnSelect As System.Web.UI.WebControls.Button
    'AA:16.12.2013 - BT:2311 Removed for menu item because it is in master page
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonReport As System.Web.UI.WebControls.Button
    Protected WithEvents SqlDataAdapter1 As System.Data.SqlClient.SqlDataAdapter
    Protected WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    'Protected WithEvents datagridCheckBox As CustomControls.CheckBoxColumn ' Manthan Rajguru | 2015-09-24 | YRS-AT-2550: Commented as control not used anywhere in the page
    Protected WithEvents LabelYmcas As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSelectedYmcas As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonUpdateCounter As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxYmcaList As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYmcaSelected As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DataSet11 As YMCAUI.DataSet1

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'Commented for New IDM Class
    'Ashutosh Patil as on 22-Mar-2007
    'Dim IDM As New IDforIDM

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Global Declaration"
    Dim g_String_Exception_Message As String
    Dim g_dataset_YMCA As New DataSet
    Dim strYmcaMissingEmailList As String
    Dim IDM As New IDMforAll
    Dim g_datatable_FailYMCA As DataTable
    Dim strProcessYmcaNo As String
    Dim str_mySearchString As String
    Dim g_dataset_LetterType As New DataSet
#End Region

    Public Property SelectedYmcas()
        Get
            Return ViewState("int_Count")
        End Get
        Set(ByVal Value)
            ViewState("int_Count") = Value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Not IsPostBack Then
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                End If
                'If (Request.Form("OK") = "OK" And Session("IsDone") = True) Then
                '    Session("IsDone") = False
                '    Response.Redirect("MainWebForm.aspx", False)
                'End If
                ' To load Datagrid Delinquency
                Me.LabelSelectedYmcas.Visible = False
                Me.LabelYmcas.Visible = False
                Me.TextBoxYmcaList.Visible = False
                Me.TextBoxYmcaSelected.Visible = False
                Me.ButtonUpdateCounter.Enabled = False
                Session("16 days letter") = Nothing
                Session("LetterTypeNo") = Nothing
                PopulateLetterType()
            End If
            If Not Session("LetterTypeNo") Is Nothing Then
                Dim l_int_lettertype As String = Session("LetterTypeNo")
                Dim popupScript As String = "<script language='javascript'>" & _
                 " ChangeHeader(" + l_int_lettertype + ") </script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                    Page.RegisterStartupScript("PopupScript1", popupScript)
                End If
                strYmcaMissingEmailList = ""
                'NOT REQUIRED ANYMORE - Session("Letter Type") = Nothing
            End If
            
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub

#Region "******************Public Methods**********************"

    Private Sub FillDatagrid()

        Dim l_dataset As New DataSet
        Dim l_intCount As Integer
        Const EmployeeContribution As Integer = 9

        Try

            l_intCount = 0
            'BT:821-YRS 5.0-1317: Changes to delinquescy letters
            'If lblLetterType.Text = "9th Business Day Letter" Then
            If CType(Session("LetterTypeNo"), Integer) = 1 Then
                l_dataset = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetDelinquentYmcasFor9thBusDay()
                Session("ds_DelinquentYmcas") = l_dataset
            Else
                l_dataset = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetDelinquentYmcas()
                Session("ds_DelinquentYmcas") = l_dataset
            End If

            If HelperFunctions.isNonEmpty(l_dataset) Then
                'BT:821-YRS 5.0-1317: Changes to delinquescy letters
                'If lblLetterType.Text = "16th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 4 Then
                    DataGridDelinquency.Columns(EmployeeContribution).Visible = True
                End If


                DataGridDelinquency.DataSource = l_dataset
                DataGridDelinquency.DataBind()
                l_intCount = l_dataset.Tables(0).Rows.Count
                'Me.ButtonProcess.Text = "Print " & lblLetterType.Text

            End If
            Me.LabelSelectedYmcas.Visible = True
            LabelYmcas.Visible = True
            Me.TextBoxYmcaList.Visible = True
            Me.TextBoxYmcaSelected.Visible = True
            TextBoxYmcaList.Text = l_intCount.ToString
            Me.TextBoxYmcaSelected.Text = 0
        Catch ex As Exception
            Throw

        End Try
    End Sub
    Private Sub PopulateLetterType()
        Try
            Dim l_drow As DataRow
            Dim l_dataset As New DataSet
            'To get the letter types
            'g_dataset_YMCA = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetLetter()
            g_dataset_LetterType = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetLetter()

            'To get 15th  business day 
            l_dataset = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.Get15thBusinessDay()

            If HelperFunctions.isNonEmpty(l_dataset) Then
                If HelperFunctions.isNonEmpty(g_dataset_LetterType) Then
                    dgLetterType.DataSource = g_dataset_LetterType
                    dgLetterType.DataBind()
                End If
                'check for 15th businessday

                If Date.Today = (CType(l_dataset.Tables(0).Rows(0)("15th BusinessDay"), Date)).ToShortDateString() Then
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "No Letters To be Mailed", MessageBoxButtons.Stop)

                    'Shashi Shekhar: 23-Oct-2009 :Change Label text as per discussion with Nikunj and Hafiz
                    lblLetterType.Text = "No Business Letter Due - Today is 15th Business Day"
                    Me.btnSelect.Enabled = False
                    Exit Sub
                End If
            End If

            'Set the current letter type into the label
            If HelperFunctions.isNonEmpty(g_dataset_LetterType) = True Then
                'NOT REQUIRED - This sessions variable is not being accessed anywhere
                'Session("dsLetterType") = g_dataset_YMCA
                For Each l_drow In g_dataset_LetterType.Tables(0).Rows
                    If l_drow("currentBDate") = "1" Then
                        lblLetterType.Text = l_drow("Letter Type")
                        'NOT REQUIRED ANYMORE - Session("Letter Type") = l_drow("Letter Type")
                        Session("LetterTypeNo") = l_drow("SNo")
                        Session("strReportName1") = l_drow("ReportName")
                        Me.btnSelect.Enabled = True
                        Exit For
                    Else
                        lblLetterType.Text = "No Business Letter Due"
                        btnSelect.Enabled = False
                    End If
                Next
                dgLetterType.DataSource = g_dataset_LetterType
                dgLetterType.DataBind()
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub OpenReportViewer()
        Try
            'Call ReportViewer.aspx 
            Dim popupScript As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If


        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function GetYMCANos() As String
        Dim chkFlag As System.Web.UI.WebControls.CheckBox
        Dim dgItem As DataGridItem
        Dim iCount As Int32
        Dim string_YMCANos As New StringBuilder
        Try
            If DataGridDelinquency.Items.Count > 0 Then

                For Each dgItem In DataGridDelinquency.Items
                    chkFlag = dgItem.FindControl("chkFlag")
                    If chkFlag.Checked Then
                        string_YMCANos.Append(dgItem.Cells(1).Text.Trim)
                        string_YMCANos.Append(",")
                        iCount = iCount + 1
                    End If
                Next
                ViewState("iCount") = iCount
                Return string_YMCANos.ToString()
            End If
        Catch
            Throw
        End Try

    End Function
    ' START: Manthan Rajguru | 2015-09-24 | YRS-AT-2550: Commented as control not used anywhere in the page
    ''Aparna samala 30/10/2006
    'Private Sub OnCheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles datagridCheckBox.CheckedChanged

    '    Dim l_CheckBox As CheckBox
    '    Dim l_DataGridItem As DataGridItem
    '    Dim l_Counter As Integer

    '    Try
    '        l_Counter = 0

    '        For Each l_DataGridItem In Me.DataGridDelinquency.Items
    '            l_CheckBox = l_DataGridItem.FindControl("Select")
    '            If Not l_CheckBox Is Nothing Then
    '                If l_CheckBox.Checked = True Then
    '                    l_Counter += 1
    '                End If
    '            End If
    '        Next

    '    Catch ex As Exception

    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

    '    End Try

    'End Sub
    ' END: Manthan Rajguru | 2015-09-24 | YRS-AT-2550: Commented as control not used anywhere in the page

    Private Sub SelectAll()
        Try
            Dim chkFlag As System.Web.UI.WebControls.CheckBox
            Dim dgItem As DataGridItem

            If DataGridDelinquency.Items.Count > 0 Then
                If ButtonSelectAll.Text = "Select All" Then
                    For Each dgItem In DataGridDelinquency.Items
                        chkFlag = dgItem.FindControl("chkFlag")
                        If chkFlag.Enabled Then
                            chkFlag.Checked = True
                        End If
                    Next
                    Me.SelectedYmcas = DataGridDelinquency.Items.Count
                    ButtonSelectAll.Text = "Select None"
                Else
                    For Each dgItem In DataGridDelinquency.Items
                        chkFlag = dgItem.FindControl("chkFlag")
                        If chkFlag.Enabled Then
                            chkFlag.Checked = False
                        End If
                    Next
                    Me.SelectedYmcas = 0
                    ButtonSelectAll.Text = "Select All"

                End If
                'aparna
                Me.LabelSelectedYmcas.Visible = True
                Me.TextBoxYmcaSelected.Enabled = True
                Me.TextBoxYmcaSelected.Text = SelectedYmcas.ToString
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Function ProcessForEmail(ByVal p_dataset_YMCA As DataSet) As String
        Dim dtContact As DataTable
        Dim dtOfficer As DataTable
        Dim drLetterType As DataRow
        Dim strLetterType As String
        Dim strEmail As String
        Dim strEmailAddr As String
        Dim strMessage As String
        Dim strYmcaNo As String


        Dim iCount As Integer
        'Dim strYmcaMissingEmailList As String
        Dim i As Integer
        Dim strCEOEmail As String
        Dim strCVOEmail As String
        Dim l_boolCEO As Boolean
        Dim l_boolCVO As Boolean
        l_boolCEO = True
        l_boolCVO = True
        Dim l_boolContinue As Boolean
        Dim l_string_Message As String = String.Empty
        Dim strLoggingText As StringBuilder
        'SR:2012.08.29- BT-1064:YRS 5.0-1643 : Changes to 11th business day letter.
        Dim l_boolCHRO As Boolean
        l_boolCHRO = True
        Dim strCHROMail As String
        'SR:2012.08.29:  BT-1064:YRS 5.0-1643 : END
        Try
            'Dim g_Dataset_YmcaPayrolls As New DataSet
            'Added by Ashish 2010.03.22, Log the exception into logfile
            strLoggingText = New StringBuilder
            strLoggingText.Append("LetterType=" + lblLetterType.Text + vbCrLf)
            strCEOEmail = ""
            strCVOEmail = ""
            strEmailAddr = ""
            strCHROMail = ""      'SR:2012.08.29:  BT-1064:YRS 5.0-1643 : Changes to 11th business day letter.
            g_dataset_YMCA = p_dataset_YMCA
            dtContact = g_dataset_YMCA.Tables(0)
            l_boolContinue = True
            For i = 0 To dtContact.Rows.Count - 1
                'If lblLetterType.Text = "9th Business Day Letter" Then
                IDM.DocTypeCode = "DELINQ" + GetSelectedBusinessDay()
                If CType(Session("LetterTypeNo"), Integer) = 1 Then
                    'Get the DocCOde -22/03/2007 
                    'Ashutosh Patil as on 22-Mar-2007
                    'IDM.DocTypeCode = "DELINQ09"
                    If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "TRANSM" Then
                        strEmailAddr = dtContact.Rows(i)("EmailAddr").ToString()
                    End If
                End If

                'If lblLetterType.Text = "12th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 2 Then
                    'Get the DocCOde -22/03/2007 
                    'Ashutosh Patil as on 22-Mar-2007
                    ' IDM.DocTypeCode = "DELINQ12"
                    If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "CFO" Then
                        strEmailAddr = dtContact.Rows(i)("EmailAddr").ToString()
                    End If
                    'SR:2012.08.29:  BT-1064:YRS 5.0-1643 : Changes to 11th business day letter.
                    If l_boolCHRO = True Then
                        If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "CHRO" Then
                            strCHROMail = dtContact.Rows(i)("EmailAddr").ToString()
                            l_boolCHRO = False
                        Else
                            strCHROMail = ""

                        End If		
                        'SR:2012.08.29:  BT-1064:YRS 5.0-1643 : End
                    End If
                End If
                'If lblLetterType.Text = "14th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 3 Then
                    'Get the DocCOde -22/03/2007 
                    'Ashutosh Patil as on 22-Mar-2007
                    'IDM.DocTypeCode = "DELINQ14"
                    If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "CEO" Then
                        strEmailAddr = dtContact.Rows(i)("EmailAddr").ToString()
                    End If
                End If

                ' have to look more in detaild VPR
                'If lblLetterType.Text = "16th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 4 Then
                    'Get the DocCOde -22/03/2007 
                    'Ashutosh Patil as on 22-Mar-2007
                    'IDM.DocTypeCode = "DELINQ16"
                    If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "CFO" Then
                        strEmailAddr = dtContact.Rows(i)("EmailAddr").ToString()
                    End If
                    If l_boolCEO = True Then
                        If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "CEO" Then
                            strCEOEmail = dtContact.Rows(i)("EmailAddr").ToString()
                            l_boolCEO = False
                        Else
                            strCEOEmail = ""

                        End If
                    End If
                    If l_boolCVO = True Then
                        'Start:AA:02/10/2015 BT:2738 :YRS 5.0-2456:YERDI3I-2319: Added to verify the cvo mail first if not exists check for the VOLDIR
                        If dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "CVO" Then
                            strCVOEmail = dtContact.Rows(i)("EmailAddr").ToString()
                            l_boolCVO = False
                            'End:AA:02/10/2015 BT:2738 :YRS 5.0-2456:YERDI3I-2319: Added to verify the cvo mail first if not exists check for the VOLDIR
                        ElseIf dtContact.Rows(i)("OfficerTitle").ToString().Trim().ToUpper() = "VOLDIR" Then
                            strCVOEmail = dtContact.Rows(i)("EmailAddr").ToString()
                            l_boolCVO = False
                        Else
                            strCVOEmail = ""

                        End If

                    End If
                End If

                strYmcaNo = dtContact.Rows(i)("YmcaNo").ToString()
                'Ashutosh Patil as on 22-Mar-2007
                Session("YmcaNo") = strYmcaNo

            Next

            'Added by Ashish 2010.03.22, Log the exception into logfile

            strLoggingText.Append("YMCA NO=" + strYmcaNo + vbCrLf + "EmailAddress=" + strEmailAddr + vbCrLf + "CEOEmail=" + strCEOEmail + vbCrLf + "CVOEmail=" + strCVOEmail)

            Try
                'by aparna 8/12/2006
                l_string_Message = ProcessReport(strYmcaNo)

                If l_string_Message = String.Empty Then
                    'To check for Ymcas who dont have Email IDs
                    'Mail Sent to the sender itself
                    'Change in the web config -removing the keys and placing them in the Atsmetaconfiguration table
                    'this check for email id will be done in the SendMail function itself
                    If strEmailAddr.Trim = Nothing Then
                        'commented by Aparna 18/04/2007
                        ' strEmailAddr = ConfigurationSettings.AppSettings("FromMail")
                        strEmailAddr = String.Empty
                        'by aparna -to get the actual error msg
                        '  l_boolContinue = False
                        l_string_Message = "Email ID missing for YMCA :"
                    End If

                    'SR:2012.08.29: YRS 5.0-1643 : Added new parameter to add CHRO in CC for 11th buisness report .                    
                    SendMail(strEmailAddr, strCEOEmail, strCVOEmail, strCHROMail)



                Else
                    'ProcessForEmail = l_string_Message
                    'l_boolContinue = False
                    '      l_string_Message = "Error while generating the report"
                End If
            Catch ex As Exception
                'l_string_Message = "Error while processing Email"
                '   l_boolContinue = False
                'Added by Ashish 2010.03.22, Log the exception into logfile
                HelperFunctions.LogException(strLoggingText.ToString(), ex)

                'IB: Added on 25/Oct/2010- BT:669-Handle unexpected errors & Some sort of a message to indicate which YMCA's it failed.
                'Throw
                AddDatatable_FailYMCA(strYmcaNo, "Error while process report/SendMail", ex.ToString())
                l_string_Message = "Error while generating the report"
            End Try
            'by aparna -to get the actual error msg
            ProcessForEmail = l_string_Message
            ' ProcessForEmail = l_boolContinue
        Catch ex As Exception
            'IB: Added on 25/Oct/2010- BT:669-Handle unexpected errors & Some sort of a message to indicate which YMCA's it failed.
            'Throw
            HelperFunctions.LogException(strLoggingText.ToString(), ex)
            AddDatatable_FailYMCA(strYmcaNo, "Error while fail ProcessForEmail method", ex.ToString())
            ProcessForEmail = "Error while fail ProcessForEmail method"
        End Try

    End Function
    Private Sub SendMail(ByVal ToEmailAdd As String, ByVal CEOEmail As String, ByVal CVOEmail As String, ByVal strCHROMail As String)
        Try

            Dim obj As MailUtil
            Dim l_strEmailCC As String = ""
            Dim l_Attachments As String = ""
            Dim l_Attachments1 As String = ""
            Dim l_Attachments2 As String = ""
            Dim l_Attachments3 As String = ""
            Dim l_str_msg As String
            Dim l_DataTable As New DataTable
            Dim l_DataRow As DataRow
            obj = New MailUtil

            obj.MailCategory = "DELINQ"
            If obj.MailService = False Then Exit Sub

            If ToEmailAdd.ToString().Trim() = String.Empty Then
                'If the "To" Email is missing, use the "From" Email to inform the sender 
                obj.ToMail = obj.FromMail
                'commented by Aparna 26/06/2007-Error occured as To Email id was not able.
                'Throw New Exception("Unable to send email, Email Id not available.")
            Else
                obj.ToMail = ToEmailAdd.ToString().Trim()
            End If

            If CEOEmail <> "" Then
                l_strEmailCC += "; " + CEOEmail.Trim
            End If

            If CVOEmail <> "" Then
                l_strEmailCC += "; " + CVOEmail.Trim
            End If
            'SR:2012.08.29: BT-1064:YRS 5.0-1643 : Added new parameter to add CHRO in CC for 11th buissness report .                    
            If strCHROMail <> "" Then
                l_strEmailCC += "; " + strCHROMail.Trim
            End If

            obj.SendCc += l_strEmailCC

            obj.MailMessage = "The attached documents contain a list of transmittals and/or payments that have not been received by The YMCA Retirement Fund." & ControlChars.CrLf & ControlChars.CrLf & " Please take immediate action to resolve this situation." & ControlChars.CrLf & ControlChars.CrLf & ControlChars.CrLf & ControlChars.CrLf & " Sincerely," & ControlChars.CrLf & ControlChars.CrLf & ControlChars.CrLf & _
                                          " The YMCA Retirement Fund Finance Department."

            obj.Subject = "We have not received either your transmittal or remittance-Please read attached documents."

            'Aparna 26/09/2006 To include the pdf files along with the reports
            If Session("StringDestFilePath") <> "" Then
                ' obj.MailAttachments = Session("StringDestFilePath")
                l_Attachments = Session("StringDestFilePath")

                ' If lblLetterType.Text = "12th Business Day Letter" Or lblLetterType.Text = "14th Business Day Letter" Or lblLetterType.Text = "16th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 2 Or CType(Session("LetterTypeNo"), Integer) = 3 Or CType(Session("LetterTypeNo"), Integer) = 4 Then
                    'by Aparna YREN-3197 16/04/2007 
                    'Changed the Key in the WebConfig to DELINQ From Del 
                    'The attachments which should go along with this Email are available in this folder
                    'C:\Inetpub\wwwroot\YRS\APPS\DELINQ\PDF
                    'l_Attachments1 = ConfigurationSettings.AppSettings("Del") + "\" + "remit.pdf"
                    l_Attachments1 = ConfigurationSettings.AppSettings("DELINQ") + "\\" + "PDF" + "\\" + "remit.pdf"
                    'obj.MailAttachments1 = l_Attachments1
                    obj.MailAttachments.Add(l_Attachments1)

                    If Session("strReportName1") = "16th business day letter - Employee" Then
                        'by Aparna YREN-3197 16/04/2007 
                        'Changed the Key in the WebConfig to DELINQ From Del 
                        'The attachments which should go along with this Email are available in this folder
                        'C:\Inetpub\wwwroot\YRS\APPS\DELINQ\PDF
                        'l_Attachments2 = ConfigurationSettings.AppSettings("Del") + "\" + "fm5330 4.0.pdf"
                        'l_Attachments3 = ConfigurationSettings.AppSettings("Del") + "\" + "5330instrc 4.0.pdf"

                        l_Attachments2 = ConfigurationSettings.AppSettings("DELINQ") + "\\" + "PDF" + "\\" + "fm5330 4.0.pdf"
                        l_Attachments3 = ConfigurationSettings.AppSettings("DELINQ") + "\\" + "PDF" + "\\" + "5330instrc 4.0.pdf"
                        'by Aparna YREN-3197 16/04/2007 

                        'obj.MailAttachments2 = l_Attachments2
                        'obj.MailAttachments3 = l_Attachments3
                        obj.MailAttachments.Add(l_Attachments2)
                        obj.MailAttachments.Add(l_Attachments3)
                    End If

                End If
                'obj.MailAttachments = l_Attachments
                obj.MailAttachments.Add(l_Attachments)
            Else
                'obj.MailAttachments = Nothing

            End If

            obj.Send()
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function ProcessReport(ByVal YmcaNo As String) As String
        Dim l_StringMessage As String
        Dim g_Dataset_YmcaPayrolls As New DataSet
        Dim strYmcaNo As String
        Dim strPayrolldates As String
        Dim l_drow As DataRow
        Dim str_lettertype As String
        Dim str_YmcaID As String
        Dim str_ContributionData As String
        Dim str_MissingData As String
        Dim str_TransmittalNo As String
        l_StringMessage = String.Empty
        Try
            str_lettertype = lblLetterType.Text.ToString()
            strPayrolldates = ""
            str_MissingData = ""
            str_ContributionData = ""
            str_TransmittalNo = ""
            If YmcaNo <> "" Then
                strYmcaNo = YmcaNo
                'If lblLetterType.Text = "9th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 1 Then
                    g_Dataset_YmcaPayrolls = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetPayRollDatesFor9thBusDay(strYmcaNo)
                Else
                    g_Dataset_YmcaPayrolls = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetYmcasPayRollDates(strYmcaNo)
                End If

            End If

            If HelperFunctions.isNonEmpty(g_Dataset_YmcaPayrolls) = True Then
                For Each l_drow In g_Dataset_YmcaPayrolls.Tables(0).Rows
                    str_YmcaID = l_drow("guiUniqueID").ToString
                    str_ContributionData = str_ContributionData + l_drow("MissingContributionData").ToString + ControlChars.CrLf
                    str_MissingData = str_MissingData + l_drow("MissingPayments").ToString + ControlChars.CrLf
                    strPayrolldates = strPayrolldates + l_drow("payrollDate") + ControlChars.CrLf
                    '    "." & ControlChars.CrLf
                    str_TransmittalNo = str_TransmittalNo + l_drow("TransmittalNo").ToString() + ControlChars.CrLf

                Next

                'To selecr rhe report based on employee Contribution
                'Aparna -26/09/2006
                'If lblLetterType.Text = "16th Business Day Letter" Then
                If CType(Session("LetterTypeNo"), Integer) = 4 Then
                    For Each l_drow In g_Dataset_YmcaPayrolls.Tables(0).Rows
                        If l_drow("EmployeeContribution") = "E" Then
                            'Session("strReportName1") = "16th business day letter - Employee"
                            Session("strReportName1") = GetSelectedReportName().Trim() + " - Employee"
                            Exit For
                        Else
                            'Session("strReportName1") = "16th business day letter - Employer"
                            Session("strReportName1") = GetSelectedReportName() + " - Employer"
                        End If
                    Next
                End If
            End If

            Session("FormYMCAId") = str_YmcaID
            Session("PayRollDates") = strPayrolldates
            Session("Contribution Data") = str_ContributionData
            Session("Missing Payrolls") = str_MissingData
            Session("TransmittalNo") = str_TransmittalNo
            'ASHISH:2011.12.05 BT-752 : Session not getting clear.
            'Session("OpenReport") = True

            Call Me.SetPropertiesForIDM()
            l_StringMessage = IDM.ExportToPDF()


            Return l_StringMessage
        Catch ex As Exception
            Throw

            l_StringMessage = "Error while generating the report"
            AddDatatable_FailYMCA(YmcaNo, "Error while generating the report", ex.ToString())
            Return l_StringMessage
        End Try

    End Function

    Private Function ExportToPopUp(ByVal p_dataTable As DataTable) As Boolean
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
            Const FORMAT As String = "yyyyMMddHHmmss" 'Example format
            Dim l_date As String


            Dim String_SourceFolder As String
            Dim String_SourceFile As String
            Dim String_DestFile As String

            l_date = System.Xml.XmlConvert.ToString(Now(), FORMAT)
            'new
            'by Aparna YREN-3197 16/04/2007
            'New Fodler created for the CSV Files with path as
            'C:\Inetpub\wwwroot\YRS\APPS\DELINQ\CSV
            'l_filenameprefix = ConfigurationSettings.AppSettings("Del")
            l_filenameprefix = ConfigurationSettings.AppSettings("DELINQ") & "\\" & "CSV"
            'by Aparna YREN-3197 16/04/2007
            ' l_filenamesuffix = GenerateTextFile()
            l_filenamesuffix = "_" + DirectCast(l_date, String)
            l_fileDel = "Delinquency" + l_filenamesuffix + ".csv"
            l_filename = l_filenameprefix.Trim() + "\" + l_fileDel

            If CreateCSVFile(p_dataTable, l_filename) Then

                String_SourceFolder = l_filenameprefix
                String_SourceFile = l_fileDel
                String_DestFile = l_fileDel
                CopyToServer(String_SourceFolder, String_SourceFile, String_DestFile)
                ExportToPopUp = True
            Else
                ExportToPopUp = False
            End If

        Catch
            Throw
        End Try

    End Function
    Private Function CopyToServer(ByVal p_String_SourceFolder As String, ByVal p_String_SourceFile As String, ByVal p_String_DestFile As String)
        Try
            Dim l_dataset As DataSet
            Dim l_DataRow As DataRow

            l_dataset = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.MetaOutputFileType()
            If l_dataset Is Nothing Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Unable to find Delinquency Letters Values in the MetaOutput file.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Unable to find Delinquency Letters Values in the MetaOutput file.", EnumMessageTypes.Error)
                Exit Function
            End If

            If l_dataset.Tables(0).Rows.Count > 0 Then
                l_DataRow = l_dataset.Tables(0).Rows(0)
            Else
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Unable to find Delinquency Letters Configuration Values in the MetaOutputFileTypes table.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Unable to find Delinquency Letters Configuration Values in the MetaOutputFileTypes table.", EnumMessageTypes.Error)
                Exit Function
            End If

            IDM.AddFileListRow(p_String_SourceFolder, p_String_SourceFile, Convert.ToString(l_DataRow("OutputDirectory")), p_String_DestFile)
            'After adding this Record it will automatically copied by the copytoserver.aspx file which is called from the parent Main button click.

        Catch
            Throw
        End Try

    End Function

    Private Function CreateCSVFile(ByVal p_dataTable As DataTable, ByVal Parameter_String_Filename As String) As Boolean
        Dim l_String_YMCANo As String
        Dim l_String_blank As String
        Dim l_String_Amount As String
        Dim l_String_Output As String
        Dim l_stringbuilder As New StringBuilder
        Dim l_DataRow As DataRow
        Dim l_DatatableCSV As DataTable
        Try

            l_DatatableCSV = p_dataTable

            If l_DatatableCSV.Rows.Count > 0 Then
                Dim l_StreamWriter_File As StreamWriter = File.CreateText(Parameter_String_Filename)
                ' l_stringbuilder = ""
                '"C:\Inetpub\wwwroot\YRS\APPS\Del\abc.csv"
                l_stringbuilder.Append("Ymca No" & "," & "Ymca Name" & "," & "Contact Name" & "," & "Fax Number")
                l_stringbuilder.Append(ControlChars.CrLf)

                For Each l_DataRow In l_DatatableCSV.Rows
                    ' Dim l_stringbuilder As New StringBuilder
                    l_stringbuilder.Append(l_DataRow("YmcaNo").ToString().Trim() & ",")
                    l_stringbuilder.Append(l_DataRow("YmcaName").ToString().Trim() & ",")
                    l_stringbuilder.Append(l_DataRow("OfficerName").ToString().Trim() & ",")
                    l_stringbuilder.Append(l_DataRow("FaxNumber").ToString().Trim())
                    l_stringbuilder.Append(ControlChars.CrLf)

                Next
                l_StreamWriter_File.WriteLine(l_stringbuilder)
                l_StreamWriter_File.Close()
                CreateCSVFile = True
            Else
                CreateCSVFile = False
            End If

        Catch ex As Exception
            Throw
        End Try


    End Function


#End Region '********************End Public Methods*****************'
#Region "*************Private General Functions*********"
    'private enum string

    'Private Function HasRows(ByVal ds As DataSet) As Boolean
    '    If Not ds Is Nothing Then
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Else
    '        Return False
    '    End If
    'End Function
    'Private Function HasRows(ByVal dt As DataTable) As Boolean

    '    If dt.Rows.Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If


    'End Function
    Public Sub GetCatch(ByVal ex As Exception)
        Dim l_String_Exception_Message As String
        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    End Sub

    '**************************************************************
#End Region

    Private Sub ButtonSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        Try
            If DataGridDelinquency.Items.Count > 0 Then
                SelectAll()
            Else
                ' MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Please  Select the Letter Type :", MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub
    Private Sub dgLetterType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLetterType.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If e.Item.DataItem("currentBDate").ToString() = "1" Then
                    e.Item.BackColor = System.Drawing.Color.PaleGreen
                    e.Item.Font.Bold = True
                End If
            End If
        Catch ex As Exception
            GetCatch(ex)
            'Throw
        End Try
    End Sub
    Private Sub ButtonProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonProcess.Click
        Dim string_YMCANo As String
        Dim int_Sno As Integer
        Dim str_lettertype As String
        Dim chkFlag As System.Web.UI.WebControls.CheckBox
        Dim dgItem As DataGridItem
        Dim l_DataTableCSV As New DataTable
        Dim l_bool_flag As Boolean
        Dim l_DatasetCSV As New DataSet
        Dim l_drow As DataRow
        Dim l_strYmcaNo As New StringBuilder
        Dim strYmcaEmailNotSent As String
        Dim l_bool_EmailSentNotificatiion As Boolean
        l_bool_EmailSentNotificatiion = False
        Dim l_string_Message As String
        Dim l_stringLetterType As String
        Dim l_bool_ImportRow As Boolean
        Dim l_string_searchExpr As String
        Dim l_datarow_CurrentRow As DataRow()
        Dim l_intCount As Integer


        Try

            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            l_bool_flag = True
            Me.SelectedYmcas = 0
            'To get the Sno-UniqueId of the Business Letter 
            int_Sno = CType(Session("LetterTypeNo"), Integer)
            strYmcaEmailNotSent = String.Empty
            If DataGridDelinquency.Items.Count = 0 Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "No Delinquent YMCAs", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("No Delinquent YMCAs", EnumMessageTypes.Error)
                Exit Sub
            End If
            'BY APARNA yren-3197 17/04/2007 - changed the filelist name from intdtFileList to DatatableFileList
            If IDM.DatatableFileList(False) Then
                'IDM.CreateDatatableReportTracking()
            Else
                Throw New Exception("Unable to Process delinquency Letters, Could not create dependent table")
            End If
            'by aparna 26/06/2007
            'Check for Existence of Keys in Web Config
            'Check for Folders.
            'CHeck for Keys in Atsmetaconfiguration table
            l_string_Message = Me.PerformChecks()
            If Not l_string_Message = "" Then
                Response.Redirect("StatusPageForm.aspx?CopyFile=2 &Message=" + Server.UrlEncode(l_string_Message.Trim.ToString()), False)
                ' Response.Redirect("ErrorPageForm.aspx?Message=" + l_string_Message, False)
                Exit Sub
            End If

            'IB: Added on 25/Oct/2010- BT:669-Handle unexpected errors & Some sort of a message to indicate which YMCA's it failed.
            'Create datatable for captured failed YMCA's error exception
            CreateDatatable_FailYMCA()
            'Looping through all the Ymcas-get the selected YMCA
            For Each dgItem In DataGridDelinquency.Items
                chkFlag = dgItem.FindControl("chkFlag")
                If chkFlag.Checked Then
                    Me.SelectedYmcas += 1
                    string_YMCANo = (dgItem.Cells(1).Text.Trim)
                    Session("l_strYmcaNo") = string_YMCANo
                    'Getting the list of the contacts to whom email should be sent
                    g_dataset_YMCA = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(string_YMCANo, int_Sno)

                    If HelperFunctions.isNonEmpty(g_dataset_YMCA) = True Then
                        'Creating the datatable for csv file
                        If l_bool_flag = True Then
                            l_DataTableCSV = g_dataset_YMCA.Tables(0).Clone()
                            l_bool_flag = False
                        End If

                        'l_stringLetterType = (lblLetterType.Text).Trim

                        l_bool_ImportRow = True


                        ' For 9th, 12th & 14th Business Day letter, we have to sent to one person ONLY, processing is same.
                        Select Case CType(Session("LetterTypeNo"), Integer)
                            ' For 16th Business Day letter, we have to sent Email to 3 person but write to CSV only Once.
                            'for missing Email information, Write ONE record in csv in the following order
                            ' 1. CFO is missing, then mention the CFO, 
                            ' 2  if CFO is present, then mention CEO
                            ' 3. if CFO & CEO are present, mention CVO / VOLDIR in CSV
                            'Case "16th Business Day Letter"
                            Case 4
                                If g_dataset_YMCA.Tables(0).Rows.Count > 0 Then
                                    Dim bitInValidEmail As Boolean
                                    bitInValidEmail = False
                                    ' Process Case 1 first , CFO is missing
                                    l_datarow_CurrentRow = g_dataset_YMCA.Tables(0).Select("Officer = '" + "CFO" + "'")

                                    If l_bool_ImportRow = True AndAlso l_datarow_CurrentRow.Length > 0 Then
                                        l_drow = l_datarow_CurrentRow(0)
                                        If ((l_drow("EmailAddr")).GetType.ToString = "System.DBNull" Or (l_drow("EmailAddr")).GetType.ToString = "") Then
                                            l_DataTableCSV.ImportRow(l_drow)
                                            l_DataTableCSV.AcceptChanges()

                                            l_bool_ImportRow = False
                                            Exit Select
                                        End If
                                    End If

                                    ' Process Case 2, CEO is missing
                                    l_datarow_CurrentRow = g_dataset_YMCA.Tables(0).Select("Officer = '" + "CEO" + "'")

                                    If l_bool_ImportRow = True AndAlso l_datarow_CurrentRow.Length > 0 Then
                                        l_drow = l_datarow_CurrentRow(0)
                                        If ((l_drow("EmailAddr")).GetType.ToString = "System.DBNull" Or (l_drow("EmailAddr")).GetType.ToString = "") Then
                                            l_DataTableCSV.ImportRow(l_drow)
                                            l_DataTableCSV.AcceptChanges()

                                            l_bool_ImportRow = False
                                            Exit Select
                                        End If
                                    End If

                                    'AA:02.11.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Changed for checking if both cvo and voldir are not exists then write in CSV
                                    ' Process Case 3, CVO is missing
                                    l_datarow_CurrentRow = g_dataset_YMCA.Tables(0).Select("Officer IN ('CVO','VOLDIR') AND ISNULL(EmailAddr,'') = ''")
                                    If l_bool_ImportRow = True AndAlso l_datarow_CurrentRow.Length > 1 Then
                                        For i As Integer = 0 To l_datarow_CurrentRow.Length - 1
                                            l_drow = l_datarow_CurrentRow(i)
                                            If ((l_drow("EmailAddr")).GetType.ToString = "System.DBNull" Or (l_drow("EmailAddr")).GetType.ToString = "") Then
                                                l_DataTableCSV.ImportRow(l_drow)
                                                l_DataTableCSV.AcceptChanges()
                                                l_bool_ImportRow = False
                                            End If
                                        Next
                                        Exit Select
                                    End If
                                    
                                End If  ' End Case

                                'Case ("9th Business Day Letter" Or "12th Business Day Letter" Or "14th Business Day Letter")
                            Case Else

                                If g_dataset_YMCA.Tables(0).Rows.Count > 0 Then
                                    If ((g_dataset_YMCA.Tables(0).Rows(0)("EmailAddr")).GetType.ToString = "System.DBNull" Or (g_dataset_YMCA.Tables(0).Rows(0)("EmailAddr")).GetType.ToString = "") Then
                                        l_DataTableCSV.ImportRow(g_dataset_YMCA.Tables(0).Rows(0))
                                        l_DataTableCSV.AcceptChanges()

                                    End If
                                End If


                        End Select



                        'Check for email Ids ,Generate the pdf and send emails

                        'by aparna 8/12/2006
                        l_string_Message = Me.ProcessForEmail(g_dataset_YMCA)
                        If l_string_Message <> String.Empty Then
                            '  If Me.ProcessForEmail(g_dataset_YMCA) = False Then
                            'Populate Message 
                            'l_string_Message can retrieve any of the two msgs
                            ' 1.There is error generating report
                            ' 2. Email ID missing for YMCA
                            If l_string_Message = "Email ID missing for YMCA :" Then
                                If strYmcaEmailNotSent = String.Empty Then
                                    strYmcaEmailNotSent = CType(CType(string_YMCANo, Int32), String)
                                Else
                                    strYmcaEmailNotSent += "," + CType(CType(string_YMCANo, Int32), String)
                                End If
                            End If

                        Else
                            l_bool_EmailSentNotificatiion = True
                        End If
                    End If
                End If
            Next

            If string_YMCANo = "" Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Please select a YMCA to Process", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Please select a YMCA to Process", EnumMessageTypes.Error)
                Exit Sub
            End If

            If strYmcaEmailNotSent <> String.Empty Then
                Dim l_bool As Boolean
                'Check whther CSV file is to be generated and do the same
                If HelperFunctions.isNonEmpty(l_DataTableCSV) = True Then
                    If ExportToPopUp(l_DataTableCSV) = False Then
                        l_string_Message = "Error Writing CSV File"
                    ElseIf l_bool_EmailSentNotificatiion = True Then
                        l_string_Message = "Email sent successfully and Email ID missing for YMCA : "
                    Else
                        l_string_Message = "Email ID missing for YMCA : "
                    End If
                Else

                    'by aparna -8/12/2006
                    If l_string_Message <> String.Empty Then
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error)
                        Exit Sub
                    Else
                        l_string_Message = "Process completed with errors, Contact Tech. Support"
                        'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                        'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error)
                        Exit Sub
                    End If
                End If
            Else
                If l_string_Message <> String.Empty Then
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop)
                    HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error)
                    Exit Sub
                End If

                If l_bool_EmailSentNotificatiion = True Then
                    l_string_Message = "Email sent successfully"
                Else
                    l_string_Message = "Email not sent"
                End If
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_Message, MessageBoxButtons.OK)
                'Exit Sub
            End If
            ' Copy all the documents into respective folders.
            Session("FTFileList") = IDM.SetdtFileList
            If IDM.SetdtFileList.Rows.Count > 0 Then
                Try
                    ' Call the calling of the ASPX to copy the file.
                    Dim popupScript As String = "<script language='javascript'>" & _
                    "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                    "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
                        Page.RegisterStartupScript("PopupScript3", popupScript)
                    End If

                Catch ex As Exception
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser("Error while transfering documents to IDM", EnumMessageTypes.Error)
                    Exit Sub
                End Try
            End If

            'IB: Added on 25/Oct/2010- BT:669-Handle unexpected errors & Some sort of a message to indicate which YMCA's it failed.
            Dim strFailYmcaEmailNotSent As String
            If HelperFunctions.isNonEmpty(g_datatable_FailYMCA) = True Then
                strFailYmcaEmailNotSent = GetFailYMCAListAndLog()
                If strFailYmcaEmailNotSent <> String.Empty Then
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Process completed with errors for failed YMCA's:-" + strFailYmcaEmailNotSent + ".Contact Tech. Support", MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser("Process completed with errors for failed YMCA's:-" + strFailYmcaEmailNotSent + ".Contact Tech. Support", EnumMessageTypes.Error)
                    Exit Sub
                End If
            End If
            'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
            'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_Message + strYmcaEmailNotSent, MessageBoxButtons.OK)
            HelperFunctions.ShowMessageToUser(l_string_Message + strYmcaEmailNotSent, EnumMessageTypes.Success)
            Exit Sub
        Catch ex As Exception
            'IB: Added on 25/Oct/2010- BT:669-Handle unexpected errors & Some sort of a message to indicate which YMCA's it failed.
            Dim strFailYmcaEmailNotSent As String
            If HelperFunctions.isNonEmpty(g_datatable_FailYMCA) = True Then
                strFailYmcaEmailNotSent = GetFailYMCAListAndLog()
                If strFailYmcaEmailNotSent <> String.Empty Then
                    'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Process completed with errors for failed YMCA's:-" + strFailYmcaEmailNotSent + ".Contact Tech. Support", MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser("Process completed with errors for failed YMCA's:-" + strFailYmcaEmailNotSent + ".Contact Tech. Support", EnumMessageTypes.Error)
                    Exit Sub
                End If
            End If
            GetCatch(ex)
            'Throw
        End Try
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Try

            FillDatagrid()
            Me.LabelSelectedYmcas.Visible = True
            Me.LabelYmcas.Visible = True
            Me.ButtonUpdateCounter.Enabled = True
            Me.ButtonProcess.Enabled = True
            Me.ButtonSelectAll.Enabled = True
            Me.ButtonReport.Enabled = True

        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Try
            '  Session("Letter Type") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub

    Private Sub ButtonReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReport.Click
        Dim dgitem As DataGridItem
        Dim l_chkflag As New CheckBox
        Dim l_ymcano As String
        Dim l_datatable As New DataTable
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow As DataRow()
        Dim l_dsDelinquency As New DataSet
        Dim l_strHostName As String
        Dim drow As DataRow
        Dim l_drow As DataRow

        'aparna -yren -2994 -Indexing the grid columns
        Const intchrYmcaNoIndex As Integer = 1
        Const intchvYmcaNameIndex As Integer = 2
        Const intchvActiveResnIndex As Integer = 3
        Const intCountsIndex As Integer = 4
        Const intMinPayRollDateIndex As Integer = 5
        Const intAddlAcctsIndex As Integer = 6
        Const intValidEmailIndex As Integer = 7
        Const intNoOfEmpIndex As Integer = 8
        Const intEmpContIndex As Integer = 9
        'aparna -yren -2994

        Dim l_datasetReport As New DataSet
        Dim l_stringLetterType As String
        'by Aparna 28/06/2007
        Dim l_string_ReportPath As String = String.Empty
        Dim l_String_Message As String = String.Empty

        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            l_stringLetterType = (lblLetterType.Text).Trim
            l_strHostName = Dns.GetHostName()
            Session("HostName") = l_strHostName

            'Report to be called
            Session("strReportName") = "DelinquencyReport"

            'To Check if the Report Exists -Aparna 28/06/2007
            l_string_ReportPath = ConfigurationSettings.AppSettings("ReportPath")
            If l_string_ReportPath = Nothing Then
                l_String_Message = "ReportPath Key missing in Web Config file"
            Else
                If Directory.Exists(l_string_ReportPath) Then
                    l_string_ReportPath = l_string_ReportPath + "\\" + DirectCast(Session("strReportName"), String).Trim() + ".rpt"
                    If Not File.Exists(l_string_ReportPath) Then
                        l_String_Message = Session("strReportName").ToString().Trim + " is missing at path : " + "\Reports"
                    End If
                End If

            End If
            If Not l_String_Message = "" Then
                'AA:16.12.2013 - BT:2311 Removed showing messsage through message and displaying in message in div
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_String_Message, MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser(l_String_Message, EnumMessageTypes.Error)
                l_String_Message = ""
                Exit Sub
            End If
            'End Aparna 28/06/2007

            'aparna - to include new parameter -for report 19/12/2006
            'If l_stringLetterType.Trim = "16th Business Day Letter" Then
            If CType(Session("LetterTypeNo"), Integer) = 4 Then
                Session("16 days letter") = "Y"
            Else
                Session("16 days letter") = "N"
            End If

            'aparna - to include new parameter -for report 19/12/2006
            'aparna -to get schema of report table
            l_datasetReport = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetSchemaAtsDelinquencyCRData()



            Me.SelectedYmcas = 0

            If DataGridDelinquency.Items.Count > 0 Then
                If Not l_datasetReport.Tables("AtsDelinquencyCRData") Is Nothing Then
                    For Each dgitem In DataGridDelinquency.Items
                        l_chkflag = dgitem.FindControl("chkFlag")
                        'Aparna -14/12/2006
                        'l_drow = l_datatable.NewRow()
                        'Aparna -14/12/2006

                        l_drow = l_datasetReport.Tables("AtsDelinquencyCRData").NewRow()
                        l_drow("chrYmcaNO") = dgitem.Cells(intchrYmcaNoIndex).Text.Trim()
                        l_drow("chvYmcaName") = dgitem.Cells(intchvYmcaNameIndex).Text.Trim()
                        If dgitem.Cells(intchvActiveResnIndex).Text.ToString.ToUpper.Trim() = "&NBSP;" Then
                            l_drow("chvActiveResolution") = ""
                        Else
                            l_drow("chvActiveResolution") = dgitem.Cells(intchvActiveResnIndex).Text.Trim()
                        End If

                        l_drow("intNoofPayrolls") = CType(dgitem.Cells(intCountsIndex).Text, Int32)
                        l_drow("dtmEarliestPayRollDate") = dgitem.Cells(intMinPayRollDateIndex).Text.Trim()

                        '   l_drow("chvActiveResolution") = dgitem.Cells(6).Text
                        l_drow("chvAdditionalAccounts") = dgitem.Cells(intAddlAcctsIndex).Text.Trim()
                        'aparna - to user logged id

                        'l_drow("chvUserId") = Session("LoggedUserKey")
                        l_drow("chvUserId") = Session("LoginId")
                        l_drow("chvIpAddress") = l_strHostName
                        l_drow("intNoOfEmployees") = dgitem.Cells(intNoOfEmpIndex).Text.Trim()
                        l_drow("chvMissingContType") = dgitem.Cells(intEmpContIndex).Text.Trim()

                        'l_datatable.Rows.Add(l_dr_CurrentRow(0))
                        '      l_datatable.AcceptChanges()
                        If l_chkflag.Checked Then
                            l_drow("chvSelected") = "Yes"
                            SelectedYmcas += 1
                        Else
                            l_drow("chvSelected") = "No"
                        End If
                        l_datasetReport.Tables("AtsDelinquencyCRData").Rows.Add(l_drow)
                    Next
                End If
            End If

            'Deleting the data in the table
            YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.DeleteReportData(Session("LoginId"), l_strHostName)
            'Insert the Data to be showed on the report in the Report table
            YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.InsertReportData(l_datasetReport)

            Dim popupScript As String = "<script language='javascript'>" & _
        "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
        "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", popupScript)
            End If


        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub

    Private Sub DataGridDelinquency_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridDelinquency.SortCommand
        Dim l_DataSet As New DataSet
        Me.DataGridDelinquency.SelectedIndex = -1

        Try
            If Not Session("ds_DelinquentYmcas") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_DataSet = Session("ds_DelinquentYmcas")
                dv = l_DataSet.Tables(0).DefaultView
                dv.Sort = SortExpression

                If Not Session("Delinquency_Sort") Is Nothing Then
                    If Session("Delinquency_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If

                DataGridDelinquency.DataSource = dv
                DataGridDelinquency.DataBind()
                Session("Delinquency_Sort") = dv.Sort

            End If

        Catch ex As Exception
            GetCatch(ex)
        End Try


    End Sub

    Private Sub DataGridDelinquency_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridDelinquency.ItemDataBound
        Try
            Dim l_intCount As Integer
            l_intCount = 0
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                l_intCount += 1
                'NP:Comment - Cells(7) = bit for ValidEmail
                'If e.Item.Cells(7).Text = "0" Then	'InValid Email Id then red else default
                If e.Item.DataItem("ValidEmail").ToString() = "0" Then    'InValid Email Id then red else default
                    e.Item.ForeColor = System.Drawing.Color.Red
                End If
            End If
        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub



    Private Sub ButtonUpdateCounter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdateCounter.Click
        Try
            Dim dgItem As DataGridItem
            Dim chkFlag As New CheckBox
            Me.SelectedYmcas = 0
            For Each dgItem In DataGridDelinquency.Items
                chkFlag = dgItem.FindControl("chkFlag")
                If chkFlag.Checked Then
                    Me.SelectedYmcas += 1
                End If
            Next
            Me.TextBoxYmcaSelected.Text = Me.SelectedYmcas.ToString

        Catch ex As Exception
            GetCatch(ex)
        End Try
    End Sub
    Private Sub SetPropertiesForIDM()
        'Ashutosh Patil as on 22-Mar-2007
        Try
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "A"
            IDM.YMCAID = Session("FormYMCAId")
            IDM.GetYMCANO = Session("YmcaNo")
            IDM.ReportName = Session("strReportName1").ToString().Trim & ".rpt"
            IDM.OutputFileType = "DLTTR"
            Call AssignParametersToReport()
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub AssignParametersToReport()
        'Ashutosh Patil as on 22-Mar-2007
        Dim l_ArrListParamValues As New ArrayList
        Try
            'If CType(Session("LetterTypeNo"), Integer) = 1 Then
            'Select Case lblLetterType.Text
            Select Case CType(Session("LetterTypeNo"), Integer)
                Case 1 '"9th Business Day Letter"

                    l_ArrListParamValues.Add(DirectCast(Session("YmcaNo"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("PayRollDates"), String).ToString.Trim)

                Case 2 '"12th Business Day Letter"

                    l_ArrListParamValues.Add(DirectCast(Session("YmcaNo"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("PayRollDates"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("Contribution Data"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("Missing Payrolls"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("TransmittalNo"), String).ToString.Trim)


                Case 3 '"14th Business Day Letter"

                    l_ArrListParamValues.Add(DirectCast(Session("YmcaNo"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("PayRollDates"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("Contribution Data"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("Missing Payrolls"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("TransmittalNo"), String).ToString.Trim)



                Case 4 '"16th Business Day Letter"

                    l_ArrListParamValues.Add(DirectCast(Session("YmcaNo"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("PayRollDates"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("Contribution Data"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("Missing Payrolls"), String).ToString.Trim)
                    l_ArrListParamValues.Add(DirectCast(Session("TransmittalNo"), String).ToString.Trim)

            End Select
            IDM.ReportParameters = l_ArrListParamValues

        Catch ex As Exception
            Throw
        End Try

    End Sub
    'by Aparna -To check the existence of a  folder -26/06/2007
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

    Public Function PerformChecks() As String
        Dim l_string_FilePath As String = String.Empty
        Dim l_String_Message As String = String.Empty
        Dim l_string_Error_Message As String = String.Empty
        Dim l_stringFileName As String = String.Empty
        Dim l_stringReportName As String = String.Empty
        Dim l_stringReportPath As String = String.Empty
        Try
            'Check for the Reports Folder
            l_string_FilePath = ConfigurationSettings.AppSettings("ReportPath")
            If l_string_FilePath = Nothing Then
                l_String_Message = "ReportPath Key missing in Web Config file"
                l_string_Error_Message += l_String_Message + vbCrLf
                'PerformChecks = l_String_Message
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_String_Message, MessageBoxButtons.Stop)
                ' Exit Function
            Else
                'check if the current business day letter is existing.
                If Not Session("strReportName1") Is Nothing Then
                    'check if its 16th day letter
                    'Two letters for the 16th day letter
                    'If lblLetterType.Text = "16th Business Day Letter" Then
                    If CType(Session("LetterTypeNo"), Integer) = 4 Then
                        l_stringReportName = "16th business day letter - Employee"
                        l_stringReportPath = l_string_FilePath + "\\" + DirectCast(l_stringReportName, String).Trim() + ".rpt"
                        If Not File.Exists(l_stringReportPath) Then
                            l_String_Message = l_stringReportName.ToString().Trim + " report is missing at path : " + "\Reports"
                            l_string_Error_Message += l_String_Message + vbCrLf
                            l_String_Message = ""
                        End If
                        l_stringReportName = "16th business day letter - Employer"
                        l_stringReportPath = l_string_FilePath + "\\" + DirectCast(l_stringReportName, String).Trim() + ".rpt"
                        If Not File.Exists(l_stringReportPath) Then
                            l_String_Message = l_stringReportName.ToString().Trim + " report is missing at path : " + "\Reports"
                            l_string_Error_Message += l_String_Message + vbCrLf
                            l_String_Message = ""
                        End If
                        'check if the current business day letter is existing.
                    Else
                        l_stringReportName = CType(Session("strReportName1"), String).Trim()
                        l_stringReportPath = l_string_FilePath + "\\" + DirectCast(l_stringReportName, String).Trim() + ".rpt"
                        If Not File.Exists(l_stringReportPath) Then
                            l_String_Message = Session("strReportName1").ToString().Trim + " report is missing at path : " + "\Reports"
                            l_string_Error_Message += l_String_Message + vbCrLf
                            l_String_Message = ""
                        End If
                    End If

                End If
            End If

            'Check for the DELINQ Folder and Key in Web config
            l_string_FilePath = ConfigurationSettings.AppSettings("DELINQ")
            If l_string_FilePath <> Nothing Then
                If Not Directory.Exists(l_string_FilePath) Then
                    l_String_Message = "DELINQ Folder doesn't exist. Please create it to continue "
                    l_string_Error_Message += l_String_Message + vbCrLf
                    l_String_Message = ""
                Else
                    'Check for the CSV Folder at the path mentioned in DELINQ Key
                    l_string_FilePath = ConfigurationSettings.AppSettings("DELINQ") & "\\" & "CSV"
                    If Not Directory.Exists(l_string_FilePath) Then
                        l_String_Message = "DELINQ\CSV Folder doesn't exist."
                        l_string_Error_Message += l_String_Message + vbCrLf
                        l_String_Message = ""
                    End If

                    l_string_FilePath = ConfigurationSettings.AppSettings("DELINQ")
                    'Check for the PDF Folder at the path mentioned in DELINQ Key
                    l_string_FilePath = l_string_FilePath + "\\" + "PDF"
                    If Not Directory.Exists(l_string_FilePath) Then

                        l_String_Message = "DELINQ\PDF Folder doesn't exist."
                        l_string_Error_Message += l_String_Message + vbCrLf
                        l_String_Message = ""
                        'Check for the PDF FILES "remit.pdf","fm5330 4.0.pdf","5330instrc 4.0.pdf"
                    Else

                        l_stringFileName = l_string_FilePath + "\\" + "remit.pdf"
                        If Not File.Exists(l_stringFileName) Then
                            l_String_Message = "remit.pdf" + " File doesn't exist at the path : " + "DELINQ\PDF"
                            l_string_Error_Message += l_String_Message + vbCrLf
                            l_String_Message = ""
                        End If
                        l_stringFileName = l_string_FilePath + "\\" + "fm5330 4.0.pdf"
                        If Not File.Exists(l_stringFileName) Then
                            l_String_Message = "fm5330 4.0.pdf" + " File doesn't exist at the path : " + "DELINQ\PDF"
                            l_string_Error_Message += l_String_Message + vbCrLf
                            l_String_Message = ""
                        End If
                        l_stringFileName = l_string_FilePath + "\\" + "5330instrc 4.0.pdf"
                        If Not File.Exists(l_stringFileName) Then
                            l_String_Message = "5330instrc 4.0.pdf" + " File doesn't exist at the path : " + "DELINQ\PDF"
                            l_string_Error_Message += l_String_Message + vbCrLf
                            l_String_Message = ""
                        End If
                    End If
                End If
            Else
                l_String_Message = "DELINQ Key missing in Web Config file"
                l_string_Error_Message += l_String_Message + vbCrLf
                'PerformChecks = l_String_Message
                'Exit Function
            End If

            'Check fo r IDM Path
            l_string_FilePath = ConfigurationSettings.AppSettings("IDMPath")
            If l_string_FilePath <> Nothing Then
                l_string_FilePath = l_string_FilePath + "\\" + "YMCA" + "\\" + "IDX"
                If Not Directory.Exists(l_string_FilePath) Then
                    l_String_Message = "IDM\YMCA\IDX Folder doesn't exist."
                    l_string_Error_Message += l_String_Message + vbCrLf
                    l_String_Message = ""
                End If
                l_string_FilePath = ConfigurationSettings.AppSettings("IDMPath")
                l_string_FilePath = l_string_FilePath + "\\" + "YMCA" + "\\" + "PDF"
                If Not Directory.Exists(l_string_FilePath) Then
                    l_String_Message = "IDM\YMCA\PDF Folder doesnot exist."
                    l_string_Error_Message += l_String_Message + vbCrLf
                    l_String_Message = ""
                End If
            Else
                l_String_Message = "IDMPath Key missing in Web Config file"
                l_string_Error_Message += l_String_Message + vbCrLf
                ' PerformChecks = l_String_Message
                'Exit Function
            End If

            l_string_FilePath = ConfigurationSettings.AppSettings("FTLIST")
            If l_string_FilePath <> Nothing Then
                If Not Directory.Exists(l_string_FilePath) Then
                    l_String_Message = "FTLIST Folder doesn't exist."
                    l_string_Error_Message += l_String_Message + vbCrLf
                    l_String_Message = ""
                End If

            Else
                l_String_Message = "FTLIST Key missing in Web Config file."
                l_string_Error_Message += l_String_Message + vbCrLf
                l_String_Message = ""
                'PerformChecks = l_String_Message
                'Exit Function
            End If
            'check if the Delinq Ids are there in Config table
            l_String_Message = GetEmailConfigurationDetails("DELINQ")

            If Not l_String_Message = "" Then
                l_string_Error_Message += l_String_Message
            End If
            'Check if Admin Email ids are available
            l_String_Message = GetEmailConfigurationDetails("ADMIN")

            If Not l_String_Message = "" Then
                l_string_Error_Message += l_String_Message + vbCrLf
            End If
            PerformChecks = l_string_Error_Message
        Catch
            Throw
        End Try
    End Function
    'check for existence of files
    Public Function CheckFilesExists(ByVal StringFileName As String) As Boolean
        Dim arrFiles As FileInfo()
        Try
            'Dim myDirInfo As New DirectoryInfo(StringFileName)
            'arrFiles = myDirInfo.GetFiles("*.pdf")
            '' Me.SourceFolderPath = String.Empty
            'If arrFiles.Length > 0 Then
            '    Return True
            'Else
            '    Return False

            'End If
            Dim FileSystemObject
            Dim PermissionChecker
            FileSystemObject = Server.CreateObject("Scripting.FileSystemObject")
            If (FileSystemObject.FileExists(StringFileName)) Then
                PermissionChecker = Server.CreateObject("MSWC.PermissionChecker")
                If PermissionChecker.HasAccess(StringFileName) Then
                    CheckFilesExists = True
                End If


            Else
                CheckFilesExists = False
            End If
        Catch
            Throw
        End Try
    End Function

    Public Function GetEmailConfigurationDetails(ByVal paramstrConfigCategoryCode As String) As String
        Dim l_Mail_dataset As DataSet
        Dim l_DataTable As DataTable
        Dim l_string_ErrorMessage As String = String.Empty
        Dim l_datarow_CurrentRow As DataRow()
        Dim l_string_SearchExp As String = String.Empty
        Dim objMail As New MailUtil

        Try
            l_Mail_dataset = YMCARET.YmcaBusinessObject.MailBOClass.GetMailConfigurationDetails(paramstrConfigCategoryCode)

            If objMail.MailService = False Then Return String.Empty

            If l_Mail_dataset Is Nothing _
                    OrElse l_Mail_dataset.Tables.Count = 0 _
                    OrElse l_Mail_dataset.Tables(0).Rows.Count = 0 Then
                Return String.Empty
            End If
            If Not l_Mail_dataset Is Nothing _
                    AndAlso l_Mail_dataset.Tables("EmailDetails").Rows.Count > 0 Then
                'by aparna -07/07/07 check if the table exists then continue
                l_DataTable = l_Mail_dataset.Tables(0)
                Select Case paramstrConfigCategoryCode

                    Case "DELINQ"

                        If l_DataTable Is Nothing Then Exit Function

                        l_string_SearchExp = "chvKey ='" + "DELINQ_FROM_EMAILID" + "'"
                        l_datarow_CurrentRow = l_Mail_dataset.Tables("EmailDetails").Select(l_string_SearchExp)
                        If l_datarow_CurrentRow.Length > 0 Then
                            If l_datarow_CurrentRow(0)("chvValue") = "" Then
                                l_string_ErrorMessage += "DELINQ_FROM_EMAILID value is missing in the Configuration table" + vbCrLf
                            End If
                        Else
                            l_string_ErrorMessage += "DELINQ_FROM_EMAILID" + " key missing in the Configuration table" + vbCrLf
                        End If

                        l_string_SearchExp = "chvKey ='" + "DELINQ_DEFAULT_TO_EMAILID" + "'"
                        l_datarow_CurrentRow = l_Mail_dataset.Tables("EmailDetails").Select(l_string_SearchExp)
                        If l_datarow_CurrentRow.Length > 0 Then
                            If l_datarow_CurrentRow(0)("chvValue") = "" Then
                                l_string_ErrorMessage += "DELINQ_DEFAULT_TO_EMAILID value is missing in the Configuration table" + vbCrLf
                            End If
                        Else
                            l_string_ErrorMessage += "DELINQ_DEFAULT_TO_EMAILID" + " key missing in the Configuration table" + vbCrLf
                        End If

                        l_string_SearchExp = "chvKey ='" + "DELINQ_CC_EMAILID" + "'"
                        l_datarow_CurrentRow = l_Mail_dataset.Tables("EmailDetails").Select(l_string_SearchExp)
                        If l_datarow_CurrentRow.Length > 0 Then
                            If l_datarow_CurrentRow(0)("chvValue") = "" Then
                                l_string_ErrorMessage += "DELINQ_CC_EMAILID value is missing in the Configuration table" + vbCrLf
                            End If
                        Else
                            l_string_ErrorMessage += "DELINQ_CC_EMAILID" + " key missing in the Configuration table" + vbCrLf
                        End If

                    Case "ADMIN"

                        If l_DataTable Is Nothing Then Exit Function

                        l_string_SearchExp = "chvKey ='" + "ADMIN_FROM_EMAILID" + "'"
                        l_datarow_CurrentRow = l_Mail_dataset.Tables("EmailDetails").Select(l_string_SearchExp)
                        If l_datarow_CurrentRow.Length > 0 Then
                            If l_datarow_CurrentRow(0)("chvValue") = "" Then
                                l_string_ErrorMessage += "ADMIN_FROM_EMAILID value is missing in the Configuration table" + vbCrLf
                            End If
                        Else
                            l_string_ErrorMessage += "ADMIN_FROM_EMAILID" + " key missing in the Configuration table" + vbCrLf
                        End If

                        l_string_SearchExp = "chvKey ='" + "ADMIN_DEFAULT_TO_EMAILID" + "'"
                        l_datarow_CurrentRow = l_Mail_dataset.Tables("EmailDetails").Select(l_string_SearchExp)
                        If l_datarow_CurrentRow.Length > 0 Then
                            If l_datarow_CurrentRow(0)("chvValue") = "" Then
                                l_string_ErrorMessage += "ADMIN_DEFAULT_TO_EMAILID value is missing in the Configuration table" + vbCrLf
                            End If
                        Else
                            l_string_ErrorMessage += "ADMIN_DEFAULT_TO_EMAILID" + " key missing in the Configuration table" + vbCrLf
                        End If

                        l_string_SearchExp = "chvKey ='" + "ADMIN_TO_EMAILID" + "'"
                        l_datarow_CurrentRow = l_Mail_dataset.Tables("EmailDetails").Select(l_string_SearchExp)
                        If l_datarow_CurrentRow.Length > 0 Then
                            If l_datarow_CurrentRow(0)("chvValue") = "" Then
                                l_string_ErrorMessage += "ADMIN_TO_EMAILID value is missing in the Configuration table" + vbCrLf
                            End If
                        Else
                            l_string_ErrorMessage += "ADMIN_TO_EMAILID" + " key missing in the Configuration table" + vbCrLf
                        End If

                End Select
            End If
            Return l_string_ErrorMessage
        Catch ex As Exception
            Throw
        End Try
    End Function

    'IB: BT:669-Create datatable ,captured error,get failed YMCA list which fail during process  
    Public Sub CreateDatatable_FailYMCA()

        Try
            g_datatable_FailYMCA = New DataTable
            g_datatable_FailYMCA.Columns.Add("SrNo")
            g_datatable_FailYMCA.Columns.Add("YMCANO")
            g_datatable_FailYMCA.Columns.Add("ErrorMessage")
            g_datatable_FailYMCA.Columns.Add("Exception")
        Catch ex As Exception
            Throw
        End Try


    End Sub
    Public Sub AddDatatable_FailYMCA(ByVal strYMCANO As String, ByVal p_ErrorMessage As String, ByVal strException As String)
        Dim dr As DataRow
        Try
            dr = g_datatable_FailYMCA.NewRow()
            dr("SrNo") = g_datatable_FailYMCA.Rows.Count + 1
            dr("YMCANO") = strYMCANO
            dr("ErrorMessage") = p_ErrorMessage
            dr("Exception") = strException
            g_datatable_FailYMCA.Rows.Add(dr)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Function GetFailYMCAListAndLog() As String
        Dim strFailYMCAList As String
        Dim strExceptionLog As New StringBuilder
        Try

            strExceptionLog.Append("--Exception Policy--")
            strExceptionLog.Append(Environment.NewLine)
            For Each l_drow In g_datatable_FailYMCA.Rows

                If strFailYMCAList = String.Empty Then
                    strFailYMCAList = CType(CType(l_drow("YMCANO"), Int32), String)
                Else
                    strFailYMCAList += "," + CType(CType(l_drow("YMCANO"), Int32), String)
                End If
                strExceptionLog.Append("YMCA NO: " + CType(CType(l_drow("YMCANO"), Int32), String))
                strExceptionLog.Append(Environment.NewLine)
                strExceptionLog.Append("Exception:-")
                strExceptionLog.Append(Environment.NewLine)
                strExceptionLog.Append(l_drow("Exception").Trim)

            Next
            strExceptionLog.Append(Environment.NewLine)
            strExceptionLog.Append("--End Exception Policy--")
            Try
                Throw New ArgumentException("Exception generated for failed YMCA")
            Catch ex As Exception
                HelperFunctions.LogException(strExceptionLog.ToString(), ex)
            End Try
            Return strFailYMCAList
        Catch ex As Exception
            Throw
        End Try

    End Function

#Region "Persistence Mechanism"
    Protected Overrides Function SaveViewState() As Object
        ViewState("g_dataset_LetterType") = g_dataset_LetterType
        Return MyBase.SaveViewState()
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        g_dataset_LetterType = DirectCast(ViewState("g_dataset_LetterType"), DataSet)

    End Sub
#End Region

    Private Function GetSelectedBusinessDay() As String
        Dim dr As DataRow
        dr = GetSelectedLetterTypeRow()
        If Not dr Is Nothing Then
            Return String.Format("{0:00}", dr("BusinessDay"))
        End If
        Return ""
    End Function

    Private Function GetSelectedReportName() As String
        Dim dr As DataRow
        dr = GetSelectedLetterTypeRow()
        If Not dr Is Nothing Then
            Return CType(dr("ReportName"), String)
        End If
        Return ""

    End Function
    Private Function GetSelectedLetterTypeRow() As DataRow
        If HelperFunctions.isNonEmpty(g_dataset_YMCA) = True Then
            Return g_dataset_LetterType.Tables(0).Select("SNo=" & CType(Session("LetterTypeNo"), Integer) & "")(0)
        End If

    End Function
End Class
