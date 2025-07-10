Imports System.IO
Imports System.Data
Imports System.Configuration


'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	DailyInterest.aspx.vb
' Author Name		:	
' Employee ID		:	
' Email			    :	
' Contact No		:	
' Creation Time	    :	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'************************************************************************************
'Modficiation History
'************************************************************************************
'Modified By		Date	            Description
'************************************************************************************
'Swopna             14-July-08
'Ashish srivastava  07-Oct-08           Add logic for write changes into InterestManagment.xml
'Mohammed Hafiz     20-Nov-08           Made necessary changes during the review of Amit's code.
'Mohammed Hafiz     05-Dec-08			HR : 2008.12.05 Added a new parameter for storing the flag to send email notification.
'Neeraj Singh       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Anudeep A          24-sep-2014         BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'Manthan Rajguru    2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'************************************************************************************

Public Class DailyInterest
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DailyInterest.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelLastRunDateTime As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastRunDateTime As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCurrentMode As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistCurrentMode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelDescription As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStartTime As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonReports As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents tbStartTime As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents TextboxScheduler As System.Web.UI.WebControls.TextBox
    Protected WithEvents img_StartTime As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents calendar As System.Web.UI.HtmlControls.HtmlInputHidden
    '****************************************************Added by Amit 19-Nov-2008
    Protected WithEvents LabelDailyInterestprocess As System.Web.UI.WebControls.Label
    '****************************************************Added by Amit 19-Nov-2008


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    '****************************************************Added by Amit 19-Nov-2008
    Protected WithEvents chkMailValue As System.Web.UI.WebControls.CheckBox
    
    '****************************************************Added by Amit 19-Nov-2008

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Event Handlers"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        'commented by Swopna 14-July-08 start
        'Dim dsDailyInterests As New DataSet
        'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        'Menu1.DataBind()
        'If IsPostBack Then

        'Else
        '    dsDailyInterests = YMCARET.YmcaBusinessObject.DailyInterestBOClass.LookUpDailyInterest()
        '    If dsDailyInterests.Tables(0).Rows.Count > 0 Then
        '        Session("UniqueID") = dsDailyInterests.Tables(0).Rows(0)("NumUniqueID")
        '        DropdownlistCurrentMode.SelectedValue = Convert.ToString(dsDailyInterests.Tables(0).Rows(0)("ChvSchedulerMode"))
        '        TextboxDescription.Text = Convert.ToString(dsDailyInterests.Tables(0).Rows(0)("ChvDescription"))
        '        TextboxStartTime.Text = Convert.ToDateTime(dsDailyInterests.Tables(0).Rows(0)("DtmScheduledDate"))
        '        TextboxScheduler.Text = "Daily"
        '        TextBoxStatus.Text = "Completed"
        '        TextboxDescription.Enabled = True
        '    End If
        '    If DropdownlistCurrentMode.SelectedValue.Equals("Suspended") Then
        '        TextboxStartTime.Enabled = False
        '    End If
        'End If
        'commented by Swopna 14-July-08 end 
        Try
            'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            'Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            If Not Me.IsPostBack Then
                PopulateValues()
            Else
                'In suspended mode,user cannot change execution time.
                If DropdownlistCurrentMode.SelectedValue = "Suspended" Then
                    tbStartTime.Disabled = True
                    img_StartTime.Disabled = True
                ElseIf DropdownlistCurrentMode.SelectedValue = "Active" Then
                    tbStartTime.Disabled = False
                    img_StartTime.Disabled = False
                End If
            End If
            tbStartTime.Attributes.Add("onkeydown", "disablectrl(this);") 'Added to make Start Time ReadOnly
        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DailyInterest_Page_Load", ex)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        'commented by Swopna 14-July-08 Start
        'Dim UniqueID As Integer
        'UniqueID = Convert.ToInt32(Session("UniqueID"))
        'YMCARET.YmcaBusinessObject.DailyInterestBOClass.UpdateDailyInterest(DropdownlistCurrentMode.SelectedValue.ToString(), TextboxDescription.Text.Trim(), TextboxStartTime.Text.Trim(), UniqueID)
        'commented by Swopna 14-July-08 end
       
        'Added by Ashish on 07-Oct-2008
        'Dim l_DataSetDlyIntManagmentXml As DataSet = Nothing
        'Dim l_DlyIntManagmentXmlPath As String = String.Empty
        'Dim l_bool_DlyIntManagmentXml As Boolean = False
        Dim StartTime As DateTime
        Dim EndTime As DateTime
        Dim l_string_StartTime As String
        Dim l_string_EndTime As String
        Dim l_string_Message As String
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            '****************************************************Added by Amit 19-Nov-2008

            l_string_StartTime = YMCARET.YmcaBusinessObject.DailyInterestBOClass.getStartTime
            If l_string_StartTime = String.Empty Then
                l_string_Message = "Please define the Business Start Time in configuration table under the key BUSSINESS_START_TIME for e.g.: 08:15:00.000"
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error)
                Exit Sub
            End If

            StartTime = l_string_StartTime

            l_string_EndTime = YMCARET.YmcaBusinessObject.DailyInterestBOClass.getEndTime()
            If l_string_EndTime = String.Empty Then
                l_string_Message = "Please define the Business End Time in configuration table under the key BUSSINESS_END_TIME for e.g.: 17:30:00.000"
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error)
                Exit Sub
            End If

            EndTime = l_string_EndTime

            If CDate(Me.tbStartTime.Value) >= StartTime And CDate(Me.tbStartTime.Value) <= EndTime Then
                LabelDailyInterestprocess.Visible = False
                l_string_Message = "Daily Interest process cannot be scheduled during the business hours i.e. between " + l_string_StartTime + " and " + l_string_EndTime

                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error)
                Exit Sub
            ElseIf CDate(Me.tbStartTime.Value) < StartTime Then
                LabelDailyInterestprocess.Visible = True
                LabelDailyInterestprocess.Text = "Note: Daily Interest process will be run for Previous Day."
                SaveDailyInterest()
            ElseIf CDate(Me.tbStartTime.Value) > EndTime Then
                LabelDailyInterestprocess.Visible = True
                LabelDailyInterestprocess.Text = "Note: Daily Interest process will be run for Current Day."
                SaveDailyInterest()
            End If
            '****************************************************Added by Amit 19-Nov-2008
        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DailyInterest_ButtonSave_Click", ex)
        End Try
    End Sub

    '****************************************************Added by Amit 19-Nov-2008
    Private Sub SaveDailyInterest()
        Try
            Dim l_StartTime As DateTime
            Dim l_LastRunDate As DateTime
            Dim l_DataSetDlyIntManagmentXml As DataSet = Nothing
            Dim l_DlyIntManagmentXmlPath As String = String.Empty
            Dim l_bool_DlyIntManagmentXml As Boolean = False
            Dim MailValue As Integer

            l_DlyIntManagmentXmlPath = ConfigurationSettings.AppSettings.Item("DlyIntManagmentXml").ToString()
            If l_DlyIntManagmentXmlPath = String.Empty Then
                Throw New Exception("DlyIntManagment file path not define in config file")
            End If
            If DropdownlistCurrentMode.SelectedValue = "Select" Then
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select current mode.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Select Current Mode.", EnumMessageTypes.Error)
                Exit Sub
            End If
            If TextBoxLastRunDateTime.Text <> String.Empty Then
                l_LastRunDate = System.Convert.ToDateTime(Me.TextBoxLastRunDateTime.Text.Trim)
            Else
                'case before first execution of interest program
                l_LastRunDate = System.Convert.ToDateTime("01/01/1900")
            End If
            If Me.tbStartTime.Value <> String.Empty Then
                l_StartTime = System.Convert.ToDateTime(Me.tbStartTime.Value.Trim)
            End If
            'This process inserts new record into table and set bitActive of previous record on changing value(s).        
            YMCARET.YmcaBusinessObject.DailyInterestBOClass.InsertOnModification(CType(Session("UniqueID"), String), DropdownlistCurrentMode.SelectedValue.ToString(), l_StartTime, l_LastRunDate, TextboxDescription.Text.Trim(), chkMailValue.Checked)
            'Added by Ashish on 07-Oct-2008
            l_DlyIntManagmentXmlPath = l_DlyIntManagmentXmlPath & "\\" & "DlyIntManagment.xml"
            If File.Exists(l_DlyIntManagmentXmlPath) Then
                l_DataSetDlyIntManagmentXml = ReadDlyIntManagmentXml(l_DlyIntManagmentXmlPath)
                If Not l_DataSetDlyIntManagmentXml Is Nothing Then
                    If l_DataSetDlyIntManagmentXml.Tables.Count > 0 Then
                        If l_DataSetDlyIntManagmentXml.Tables(0).Rows.Count > 0 Then
                            l_DataSetDlyIntManagmentXml.Tables(0).Rows(0)(0) = DropdownlistCurrentMode.SelectedValue.ToString()
                            l_DataSetDlyIntManagmentXml.Tables(0).Rows(0)(1) = l_StartTime.ToString("HH:mm")
                            l_DataSetDlyIntManagmentXml.Tables(0).Rows(0)(2) = chkMailValue.Checked
                            l_bool_DlyIntManagmentXml = True
                        End If
                    End If

                End If
                If l_bool_DlyIntManagmentXml Then
                    WriteDlyIntManagmentXml(l_DlyIntManagmentXmlPath, l_DataSetDlyIntManagmentXml)
                End If
            Else
                CreateIntManagmentXml(DropdownlistCurrentMode.SelectedValue.ToString(), l_StartTime.ToString("HH:mm"), chkMailValue.Checked, l_DlyIntManagmentXmlPath)
            End If

            PopulateValues()
            HelperFunctions.ShowMessageToUser("Saved Succesfully.", EnumMessageTypes.Success)
        Catch
            Throw
        End Try
    End Sub
    '****************************************************Added by Amit 19-Nov-2008

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        'Redirects the page to main form
        Try
            Clear_Session()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DailyInterest_ButtonOK_Click", ex)
        End Try
    End Sub
    Private Sub ButtonReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReports.Click
        Try
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Report is missing.", MessageBoxButtons.Stop)
            HelperFunctions.ShowMessageToUser("Report is missing.", EnumMessageTypes.Error)
            Exit Sub
        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("DailyInterest_ButtonReports_Click", ex)
        End Try
    End Sub

    'Private Sub DropdownlistCurrentMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropdownlistCurrentMode.SelectedIndexChanged
    '    If DropdownlistCurrentMode.SelectedValue.Equals("Suspended") Then
    '        'TextboxStartTime.Enabled = False
    '    End If
    'End Sub
#End Region
#Region "Methods"
    Public Sub Clear_Session()
        'Try
        Session("UniqueID") = Nothing
        'Catch
        '    Throw
        'End Try
    End Sub
    Private Sub PopulateValues()
        Dim l_ds_PopulateControls As DataSet
        Try
            'To collect value and display in respective controls.
            l_ds_PopulateControls = YMCARET.YmcaBusinessObject.DailyInterestBOClass.LookUpDailyInterest()

            If Not l_ds_PopulateControls Is Nothing Then
                TextboxScheduler.Text = "Daily"
                If Not l_ds_PopulateControls.Tables.Count = 0 Then
                    Dim l_dt_PopulateControls As DataTable

                    'Added/Commented by Swopna,17 July 08 Start
                    'l_dt_PopulateControls = l_ds_PopulateControls.Tables(0)
                    Dim l_dt_PopulateStatus As DataTable
                    l_dt_PopulateControls = l_ds_PopulateControls.Tables("DlyIntController")
                    l_dt_PopulateStatus = l_ds_PopulateControls.Tables("DlyIntLog")
                    'Added/Commented by Swopna,17 July 08 End

                    'case when there is/are record(s) in table
                    If l_dt_PopulateControls.Rows.Count > 0 Then

                        Session("UniqueID") = l_dt_PopulateControls.Rows(0)("NumUniqueID")

                        'If l_dt_PopulateControls.Rows(0)("DtmLastRunDate").GetType.ToString() <> "System.DBNull" Then
                        If Not l_dt_PopulateStatus Is Nothing Then
                            If l_dt_PopulateStatus.Rows.Count > 0 Then
                                TextBoxStatus.Text = Convert.ToString(l_dt_PopulateStatus.Rows(0)("ChrStatus"))
                                TextBoxLastRunDateTime.Text = String.Format("{0:MM/dd/yyyy HH:mm:ss.fff}", l_dt_PopulateStatus.Rows(0)("DtmLastRunDate"))
                            End If
                        End If

                        If TextBoxLastRunDateTime.Text.Trim() = "" Then
                            If Not l_dt_PopulateControls Is Nothing Then
                                If l_dt_PopulateControls.Rows.Count > 0 Then
                                    If l_dt_PopulateControls.Rows(0)("DtmLastRunDate").GetType.ToString() <> "System.DBNull" Then
                                        TextBoxLastRunDateTime.Text = String.Format("{0:MM/dd/yyyy HH:mm:ss.fff}", l_dt_PopulateControls.Rows(0)("DtmLastRunDate"))
                                    End If
                                End If
                            End If
                        End If

                        'TextBoxStatus.Text = Convert.ToString(l_dt_PopulateControls.Rows(0)("ChrStatus"))
                        'Added/Commented by Swopna,17 July 08 End
                        'End If

                        DropdownlistCurrentMode.SelectedValue = Convert.ToString(l_dt_PopulateControls.Rows(0)("chvSchedulerMode"))
                        TextboxDescription.Text = Convert.ToString(l_dt_PopulateControls.Rows(0)("chvDescription"))
                        'TextboxScheduler.Text = "Daily"
                        tbStartTime.Value = String.Format("{0:HH:mm}", l_dt_PopulateControls.Rows(0)("dtmScheduledDate"))
                        'In suspended mode,user cannot change execution time.
                        If DropdownlistCurrentMode.SelectedValue = "Suspended" Then
                            tbStartTime.Disabled = True
                            img_StartTime.Disabled = True
                        End If

                        'Dim l_string_SendMailFlag As String = YMCARET.YmcaBusinessObject.DailyInterestBOClass.getSendMailFlag()
                        Dim l_bool_SendSuccessNotification As Boolean = False

                        l_bool_SendSuccessNotification = Convert.ToBoolean(l_dt_PopulateControls.Rows(0)("bitSendSuccessNotification"))

                        chkMailValue.Checked = l_bool_SendSuccessNotification

                        Dim StartTime As DateTime
                        Dim EndTime As DateTime
                        Dim l_string_StartTime As String
                        Dim l_string_EndTime As String

                        '****************************************************Added by Amit 19-Nov-2008

                        l_string_StartTime = YMCARET.YmcaBusinessObject.DailyInterestBOClass.getStartTime
                        If l_string_StartTime = String.Empty Then
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please define the Business Start Time in configuration table under the key BUSSINESS_START_TIME for e.g.: 08:15:00.000", MessageBoxButtons.Stop)
                            HelperFunctions.ShowMessageToUser("Please define the Business Start Time in configuration table under the key BUSSINESS_START_TIME for e.g.: 08:15:00.000", EnumMessageTypes.Error)
                            Exit Sub
                        End If

                        StartTime = l_string_StartTime

                        l_string_EndTime = YMCARET.YmcaBusinessObject.DailyInterestBOClass.getEndTime()
                        If l_string_EndTime = String.Empty Then
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please define the Business End Time in configuration table under the key BUSSINESS_END_TIME for e.g.: 17:30:00.000", MessageBoxButtons.Stop)
                            HelperFunctions.ShowMessageToUser("Please define the Business End Time in configuration table under the key BUSSINESS_END_TIME for e.g.: 17:30:00.000", EnumMessageTypes.Error)
                            Exit Sub
                        End If

                        EndTime = l_string_EndTime

                        If CDate(Me.tbStartTime.Value) < StartTime Then
                            LabelDailyInterestprocess.Visible = True
                            LabelDailyInterestprocess.Text = "Note: Daily Interest process will be run for Previous Day."
                        ElseIf CDate(Me.tbStartTime.Value) > EndTime Then
                            LabelDailyInterestprocess.Visible = True
                            LabelDailyInterestprocess.Text = "Note: Daily Interest process will be run for Current Day."
                        End If
                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub CreateIntManagmentXml(ByVal parameterSchedulerMode As String, ByVal parameterStartTime As DateTime, ByVal parameterDlyIntSendMail As Boolean, ByVal parameterDlyIntManagmentXmlPath As String)

        Dim l_dtDlyIntManagment As DataTable = Nothing
        Dim l_dsDlyIntManagment As DataSet = Nothing
        Dim l_strXml As String = String.Empty
        Dim objStreamWriter As StreamWriter = Nothing
        Dim dtSchedulerRow As DataRow
        Try
            l_dtDlyIntManagment = New DataTable("DlyIntManagment")
            l_dtDlyIntManagment.Columns.Add(New DataColumn("DlyIntSchedulerMode", GetType(System.String)))
            l_dtDlyIntManagment.Columns.Add(New DataColumn("DlyIntStartTime", GetType(System.String)))
            l_dtDlyIntManagment.Columns.Add(New DataColumn("DlyIntSendMail", GetType(System.Boolean)))

            dtSchedulerRow = l_dtDlyIntManagment.NewRow()

            dtSchedulerRow("DlyIntSchedulerMode") = parameterSchedulerMode
            dtSchedulerRow("DlyIntStartTime") = parameterStartTime.ToString("HH:mm")
            dtSchedulerRow("DlyIntSendMail") = parameterDlyIntSendMail

            l_dtDlyIntManagment.Rows.Add(dtSchedulerRow)

            l_dsDlyIntManagment = New DataSet("DlyIntManagmentScheduler")
            l_dsDlyIntManagment.Tables.Add(l_dtDlyIntManagment)

            l_strXml = l_dsDlyIntManagment.GetXml()
            objStreamWriter = New StreamWriter(parameterDlyIntManagmentXmlPath)
            objStreamWriter.Flush()
            objStreamWriter.Write(l_strXml)
            objStreamWriter.Close()

        Catch
            Throw

        Finally
            objStreamWriter = Nothing
            l_dtDlyIntManagment = Nothing
            l_dsDlyIntManagment = Nothing
        End Try
    End Sub
    Private Function ReadDlyIntManagmentXml(ByVal parameterDlyIntManagmentXmlPath As String) As DataSet
        Dim l_dsDlyIntManagment As DataSet = Nothing
        Try
            l_dsDlyIntManagment = New DataSet
            l_dsDlyIntManagment.ReadXml(parameterDlyIntManagmentXmlPath)
            Return l_dsDlyIntManagment

        Catch ex As Exception
            Throw ex

        End Try
    End Function
    Private Sub WriteDlyIntManagmentXml(ByVal parameterDlyIntManagmentXmlPath As String, ByVal parameterDataSetDlyIntManagmentXml As DataSet)

        Dim l_strXml As String = String.Empty
        Dim objStreamWriter As StreamWriter = Nothing
        Try
            If Not parameterDataSetDlyIntManagmentXml Is Nothing Then
                l_strXml = parameterDataSetDlyIntManagmentXml.GetXml()
                objStreamWriter = New StreamWriter(parameterDlyIntManagmentXmlPath)
                objStreamWriter.Flush()
                objStreamWriter.Write(l_strXml)
                objStreamWriter.Close()
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objStreamWriter = Nothing
        End Try
    End Sub

#End Region
End Class
