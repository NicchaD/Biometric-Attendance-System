'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	AddEmployment.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	6/13/2005 12:37:46 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	Pop Up for Participants Information WebForm
'
' Changed by			:	Vartika Jain    
' Changed on			:	12/01/2005
' Change Description	:	Bug Fixing: the message for date validation should be just a warning (#1751)
'*******************************************************************************
' Cache-Session     :   Vipul 03Feb06
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By			Date				Description
'********************************************************************************************************************************
'Ashutosh Patil			30-Mar-2007			 YREN-3203,YREN-3205
'Ashutosh Patil         25-May-2007          Changes related to IE7 Messagebox issue.
'Priya Jawale           08-Dec-2008          YRS 5.0-586:Do not allow enrollment date to be removed,added RequiredFiledValidator control.
'Priya Jawale           08-Dec-2008          Bug ID:  655 Change property MessageAlignment to RightCalendarControl of PopCaldar3
'Dilip Yadav            10-Nov-2009          YRS 5.0.941 : Enable fields in employment update screen
'Imran                  03-June-2010         Changes for Enhancements 
'Shashi Shekhar         26-Oct-2010          For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Shashi Shekhar Singh   2011-01-07           For BT-635, YRS 5.0-706 : Employment status when updating Terminatin date
'Shashi                 04 Mar. 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Sanjay R.              2011.05.26           YRS 5.0-1307 - Handling the null values in date fields like Hire date,Termination date and Enrollment date.
'Manthan Rajguru        2015.09.16           YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Santosh Bura			2017.03.23 			 YRS-AT-2606 -  YRS bug- modification of enrollment date should also update participation date (TrackIT 23896)   
'Santosh Bura			2017.06.13 			 YRS-AT-3410 -  YRS bug: Unable to correct DOH for a pre-eligible. Error: String was not recognized as a valid Date Time. (TrackIT 29717)
'********************************************************************************************************************************
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Public Class AddEmployment
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelHireDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxHireDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTermDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEligibilityDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEligibilityDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEnrollmentDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEnrollmentdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPriorService As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPriorService As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStatusType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListStatusType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelStatusDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStatusDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTitle As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonTitles As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxProfessional As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxExempt As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxFullTime As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelYMCA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonYMCA As System.Web.UI.WebControls.Button
    Protected WithEvents LabelYMCABranch As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYMCABranch As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonYMCABranch As System.Web.UI.WebControls.Button
    Protected WithEvents PopcalendarDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents Popcalendar1 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents Popcalendar2 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents Popcalendar3 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents Popcalendar4 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents CompareValidator3 As System.Web.UI.WebControls.CompareValidator

    Protected WithEvents valCustomDOB As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Customvalidator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Customvalidator2 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Customvalidator3 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents valRequiredErollmentDate As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim _icounter As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.LabelEligibilityDate.AssociatedControlID = Me.TextBoxEligibilityDate.ID
        Me.LabelEnrollmentDate.AssociatedControlID = Me.TextBoxEnrollmentdate.ID
        Me.LabelHireDate.AssociatedControlID = Me.TextBoxHireDate.ID
        Me.LabelPriorService.AssociatedControlID = Me.TextBoxPriorService.ID
        Me.LabelStatusDate.AssociatedControlID = Me.TextBoxStatusDate.ID
        Me.LabelStatusType.AssociatedControlID = Me.DropDownListStatusType.ID
        Me.LabelTermDate.AssociatedControlID = Me.TextBoxTermDate.ID
        Me.LabelTitle.AssociatedControlID = Me.TextBoxTitle.ID
        Me.LabelYMCA.AssociatedControlID = Me.TextBoxYMCA.ID
        Me.LabelYMCABranch.AssociatedControlID = Me.TextBoxYMCABranch.ID


        Try
            If Not IsPostBack Then
                TextBoxYMCABranch.Enabled = False
                ButtonYMCABranch.Enabled = False
                TextBoxYMCABranch.Text = ""
            End If
            TextBox1.Text = System.DateTime.Today.AddDays(31)
            If Session("Flag") = "AddEmployment" Then

                '----------------------------------------------------------------------------------------------------------------
                'Shashi: 04 Mar. 2011:Replacing Header formating with user control (YRS 5.0-450 )
                If Not Session("FundNo") Is Nothing Then
                    Headercontrol.PageTitle = "Participant Information - Add Employment"
                    Headercontrol.FundNo = Session("FundNo").ToString().Trim()
                End If
                '-----------------------------------------------------------------------------------------------------------------------------

                Session("Flag") = "AddEmployment1"
                TextBoxTermDate.Enabled = False
                TextBoxStatusDate.Text = System.DateTime.Today()
                TextBoxPriorService.Text = "0"
                Me.CompareValidator1.Enabled = True

                TextBoxEnrollmentdate.Enabled = False
                'Priya 08-Dec-2008 :  YRS 5.0-586:Do not allow enrollment date to be removed,added RequiredFiledValidator control.
                valRequiredErollmentDate.Enabled = False
                'Commented & Added by Dilip yadav : YRS BT 1024 & yrs 5.0.941
                'ElseIf Session("Flag") = "EditEmployment" Then
            Else

                '----------------------------------------------------------------------------------------------------------------
                'Shashi:04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )
                If Not Session("FundNo") Is Nothing Then
                    Headercontrol.PageTitle = "Participant Information - Update Employment"
                    Headercontrol.FundNo = Session("FundNo").ToString().Trim()
                End If
                '-------------------------------------------------------------------------------------------------------------

                Session("Flag") = "Edited"
                TextBox1.Text = System.DateTime.Today.AddDays(31)
                'Priya 08-Dec-2008 :  YRS 5.0-586:Do not allow enrollment date to be removed,added RequiredFiledValidator control.
                valRequiredErollmentDate.Enabled = TextBoxEnrollmentdate.Enabled
                If Not Session("HireDateP") Is Nothing Then
                    TextBoxHireDate.Text = Session("HireDateP")
                End If

                Me.CompareValidator1.Enabled = False

                If Not Session("TermDateP") Is Nothing Then
                    TextBoxTermDate.Text = Session("TermDateP")
                End If
                If Not Session("EligibilityDateP") Is Nothing Then
                    TextBoxEligibilityDate.Text = Session("EligibilityDateP")
                End If

                If Not Session("ProfessionalP") Is Nothing Then
                    CheckBoxProfessional.Checked = Session("ProfessionalP")
                End If
                If Not Session("ExemptP") Is Nothing Then
                    CheckBoxExempt.Checked = Session("ExemptP")
                End If
                If Not Session("FullTimeP") Is Nothing Then
                    CheckBoxFullTime.Checked = Session("FullTimeP")
                End If
            End If

            If Not Session("PriorServiceP") Is Nothing Then
                TextBoxPriorService.Text = Session("PriorServiceP")
            End If

            If Not Session("StatusTypeP") Is Nothing Then
                '--------------------------------------------------------------------------------------------------
                'Shashi Shekhar Singh: 2011-01-07: For BT-635, YRS 5.0-706 : Employment status when updating Terminatin date
                'DropDownListStatusType.SelectedItem.Value = Session("StatusTypeP")
                DropDownListStatusType.SelectedValue = Session("StatusTypeP")
                '-----------------------------------------------------------------------------------------------------
            End If
            If Not Session("StatusDateP") Is Nothing Then
                TextBoxStatusDate.Text = Session("StatusDateP")
            End If
            If Not Session("YmcaNameP") Is Nothing Then
                TextBoxYMCA.Text = Session("YmcaNameP")
            End If

            If Not Session("PositionTypeP") Is Nothing Then
                Session("TitleType") = Session("PositionTypeP")
            End If

            If Not Session("PositionDescP") Is Nothing Then
                TextBoxTitle.Text = Session("PositionDescP")
            End If
            If Not Session("BasicPaymentDateP") Is Nothing Then
                TextBoxEnrollmentdate.Text = Session("BasicPaymentDateP")
            End If
            If TextBoxTermDate.Text = "" Then
                TextBoxTermDate.Enabled = False
            Else
                TextBoxTermDate.Enabled = True
            End If
            TextBoxEligibilityDate.Enabled = False
            If TextBoxEnrollmentdate.Text = "" Then
                TextBoxEnrollmentdate.Enabled = False
                valRequiredErollmentDate.Enabled = TextBoxEnrollmentdate.Enabled
            Else
                TextBoxEnrollmentdate.Enabled = True
                valRequiredErollmentDate.Enabled = TextBoxEnrollmentdate.Enabled
            End If
            'commented by hafiz on 15-Nov-2006 for YREN-2853
            'TextBoxPriorService.Enabled = False

            DropDownListStatusType.Enabled = False
            '--- START : Below Lines are commented & Added by Dilip yadav : YRS 5.0.941 : 
            'CheckBoxExempt.Enabled = False
            'CheckBoxFullTime.Enabled = False
            'CheckBoxProfessional.Enabled = False
            CheckBoxExempt.Enabled = True
            CheckBoxFullTime.Enabled = True
            CheckBoxProfessional.Enabled = True
            '--- END : Above Lines are commented & Added by Dilip yadav : YRS 5.0.941 : 
            TextBoxStatusDate.Enabled = False
            TextBoxYMCA.Enabled = False
            TextBoxYMCABranch.Enabled = False
            ButtonYMCA.Enabled = False
            ButtonYMCABranch.Enabled = False
            '--- START : Below Lines are commented & Added by Dilip yadav : YRS 5.0.941 : 
            'TextBoxTitle.Enabled = False
            'ButtonTitles.Enabled = False
            TextBoxTitle.Enabled = True
            TextBoxTitle.ReadOnly = True
            ButtonTitles.Enabled = True
            ClearSession()
            '--- END : Above Lines are commented & Added by Dilip yadav : YRS 5.0.941 : 

            If Session("CallFlag") = "False" Then
                Session("CallFlag") = "True"
                TextBoxHireDate.Text = Session("HireDate")

                'Start : Added by DY : 13-Nov-09 : YRS 5.0.941
                If (Not Session("TermDate") Is Nothing AndAlso (Session("TermDate")) <> "") Then
                    TextBoxTermDate.Text = Session("TermDate")
                    TextBoxTermDate.Enabled = True
                End If
                'End : Added by DY : 13-Nov-09 : YRS 5.0.941

                TextBoxEligibilityDate.Text = Session("EligibilityDate")

                'Start : Added by DY : 13-Nov-09 : YRS 5.0.941
                If (Not Session("EnrollmentDate") Is Nothing AndAlso (Session("EnrollmentDate")) <> "") Then
                    TextBoxEnrollmentdate.Text = Session("EnrollmentDate")
                    TextBoxEnrollmentdate.Enabled = True
                End If
                'End : Added by DY : 13-Nov-09 : YRS 5.0.941

                TextBoxPriorService.Text = Session("PriorService")
                If Not Session("StatusType") Is Nothing Then
                    '--- START : Below Lines are commented & Added by Dilip yadav : YRS 5.0.941 :
                    'DropDownListStatusType.SelectedValue = Session("StatusType")
                    DropDownListStatusType.SelectedValue = Trim(Session("StatusType"))
                    '--- END : Above Lines are commented & Added by Dilip yadav : YRS 5.0.941 : 
                End If

                TextBoxStatusDate.Text = Session("StatusDate")
                TextBoxTitle.Text = Session("TitleR")
                If Not Session("Professional") Is Nothing Then
                    CheckBoxProfessional.Checked = Session("Professional")
                End If
                If Not Session("Exempt") Is Nothing Then
                    CheckBoxExempt.Checked = Session("Exempt")
                End If
                If Not Session("FullTime") Is Nothing Then
                    CheckBoxFullTime.Checked = Session("FullTime")
                End If

                TextBoxYMCA.Text = Session("YmcaDesc")
                TextBoxYMCABranch.Text = Session("YmcaDescB")
                Dim l_String_Status As String
                l_String_Status = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SearchYMCAStatus(Session("UniqueIdM"))
                If l_String_Status = "0" Or l_String_Status = "" Then
                    TextBoxYMCABranch.Enabled = False
                    ButtonYMCABranch.Enabled = False
                    TextBoxYMCABranch.Text = ""
                Else
                    TextBoxYMCABranch.Enabled = True
                    ButtonYMCABranch.Enabled = True
                End If
            End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonTitles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTitles.Click
        Try
            Session("HireDate") = TextBoxHireDate.Text
            Session("TermDate") = TextBoxTermDate.Text
            Session("EligibilityDate") = TextBoxEligibilityDate.Text
            Session("EnrollmentDate") = TextBoxEnrollmentdate.Text
            Session("PriorService") = TextBoxPriorService.Text
            Session("StatusType") = DropDownListStatusType.SelectedValue
            Session("StatusDate") = TextBoxStatusDate.Text
            Session("TitleR") = TextBoxTitle.Text
            Session("Professional") = CheckBoxProfessional.Checked
            Session("Exempt") = CheckBoxExempt.Checked
            Session("FullTime") = CheckBoxFullTime.Checked
            Session("YmcaDesc") = TextBoxYMCA.Text
            Session("YmcaDescB") = TextBoxYMCABranch.Text
            Session("CallFlag") = "True"
            Dim msg1 As String = "<script language='javascript'>" & _
            "window.open('SelectTitle.aspx?Page=AddEmployment','CustomPopUp1', " & _
            "'width=800, height=400, menubar=no, resizable=no,top=120,left=120, scrollbars=yes')" & _
             "</script>"

            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", msg1)
            End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub
    Private Sub ButtonYMCA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonYMCA.Click
        Try
            '_icounter = Session("icounter")
            '_icounter = _icounter + 1
            'Session("icounter") = _icounter
            'If (_icounter = 1) Then

            Session("HireDate") = TextBoxHireDate.Text
            Session("TermDate") = TextBoxTermDate.Text
            Session("EligibilityDate") = TextBoxEligibilityDate.Text
            Session("EnrollmentDate") = TextBoxEnrollmentdate.Text
            Session("PriorService") = TextBoxPriorService.Text
            Session("StatusType") = DropDownListStatusType.SelectedValue
            Session("StatusDate") = TextBoxStatusDate.Text
            Session("TitleR") = TextBoxTitle.Text
            Session("Professional") = CheckBoxProfessional.Checked
            Session("Exempt") = CheckBoxExempt.Checked
            Session("FullTime") = CheckBoxFullTime.Checked
            Session("YmcaDesc") = TextBoxYMCA.Text
            Session("YmcaDescB") = TextBoxYMCABranch.Text
            Session("CallFlag") = "True"
            Dim msg1 As String = "<script language='javascript'>" & _
            "window.open('SelectYmca.aspx','CustomPopUp2', " & _
            "'width=750, height=450, menubar=no, resizable=yes,top=120,left=120, scrollbars=yes')" & _
             "</script>"

            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", msg1)
            End If

            'Else
            '    _icounter = 0
            '    Session("icounter") = _icounter
            '    TextBoxYMCA.Text = Session("YmcaDesc")
            '    Dim l_String_Status As String
            '    l_String_Status = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SearchYMCAStatus(Session("UniqueId"))
            '    If l_String_Status = "0" Or l_String_Status = "" Then
            '        TextBoxYMCABranch.Enabled = False
            '        ButtonYMCABranch.Enabled = False
            '    Else
            '        TextBoxYMCABranch.Enabled = True
            '        ButtonYMCABranch.Enabled = True
            '    End If

            'End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonYMCABranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonYMCABranch.Click
        Try
            Session("Ymca") = "Metro"
            Session("HireDate") = TextBoxHireDate.Text
            Session("TermDate") = TextBoxTermDate.Text
            Session("EligibilityDate") = TextBoxEligibilityDate.Text
            Session("EnrollmentDate") = TextBoxEnrollmentdate.Text
            Session("PriorService") = TextBoxPriorService.Text
            Session("StatusType") = DropDownListStatusType.SelectedValue
            Session("StatusDate") = TextBoxStatusDate.Text
            Session("TitleR") = TextBoxTitle.Text
            Session("Professional") = CheckBoxProfessional.Checked
            Session("Exempt") = CheckBoxExempt.Checked
            Session("FullTime") = CheckBoxFullTime.Checked
            Session("YmcaDesc") = TextBoxYMCA.Text
            Session("YmcaDescB") = TextBoxYMCABranch.Text
            Session("CallFlag") = True
            '_icounter = Session("icounter")
            '_icounter = _icounter + 1
            'Session("icounter") = _icounter
            'If (_icounter = 1) Then
            Session("Ymca") = "Branch"
            Dim msg1 As String = "<script language='javascript'>" & _
            "window.open('SelectYmcaBranch.aspx','CustomPopUp3', " & _
            "'width=750, height=450, menubar=no, resizable=yes,top=120,left=120, scrollbars=yes')" & _
             "</script>"

            Response.Write(msg1)
            'Else
            '    _icounter = 0
            '    Session("icounter") = _icounter
            '    TextBoxYMCABranch.Text = Session("YmcaDescB")
            'End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub
    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim l_emp_datatable As DataTable
        Dim l_emp_datarow As DataRow
        Dim l_str_FundStatus As String
        Dim l_str_msg As String
        Dim l_str_YMCANo As String
        Dim l_bool_flag As Boolean
        Dim l_Term_date As Date

        Try
            Dim flag As Boolean
            flag = True
            If TextBoxEnrollmentdate.Text = "" Then
                flag = True
            Else
                Dim l_date As DateTime
                l_date = Session("l_orghiredate")
                'l_date = Convert.ToDateTime(TextBoxHireDate.Text).ToString("MM/dd/yyyy")
                'l_date = l_date.AddYears(1)
                Dim l_enrol_date As DateTime
                l_enrol_date = Convert.ToDateTime(TextBoxEnrollmentdate.Text).ToString("MM/dd/yyyy")
                If l_enrol_date < l_date Then
                    flag = False
                End If
            End If
            'Shubhrata Validations for Hire date and term date
            Dim l_tempdate As DateTime
            Dim l_tempdate1 As DateTime
            Dim l_validhiredate As DateTime
            Dim l_validtermdate As DateTime
            Dim l_temptermdate As DateTime
            l_tempdate = Convert.ToDateTime(TextBoxHireDate.Text).ToString("MM/dd/yyyy")
            l_tempdate1 = Now.ToString("MM/dd/yyyy")
            l_validhiredate = "01/01/1922"

            'Modified and Commented By Ashutosh Patil as on 25-May-2007 For IE7 Messagebox realted issue
            If TextBoxHireDate.Text <> "" Then
                If l_tempdate < l_validhiredate Then
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Hire Date", MessageBoxButtons.Stop, True)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Hire Date", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                If TextBoxTermDate.Text <> "" Then
                    If l_tempdate > Convert.ToDateTime(TextBoxTermDate.Text).ToString("MM/dd/yyyy") Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Hire date must be earlier than Termination date", MessageBoxButtons.Stop, True)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Hire date must be earlier than Termination date", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'Dim l_tempdate As DateTime
                'Dim l_tempdate1 As DateTime
                'l_tempdate = Convert.ToDateTime(TextBoxHireDate.Text).ToString("MM/dd/yyyy")
                'l_tempdate1 = Now.ToString("MM/dd/yyyy")

                If DateDiff(DateInterval.Day, l_tempdate1, l_tempdate) > 31 Then
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Hire Date is over 30 Days in Advance", MessageBoxButtons.Stop, True)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Hire Date is over 30 Days in Advance", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            If TextBoxTermDate.Text <> "" Then
                l_temptermdate = Convert.ToDateTime(TextBoxTermDate.Text).ToString("MM/dd/yyyy")
                If l_temptermdate < l_validhiredate Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Termination Date", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            'Ashutosh Patil as on 10-Apr-2007
            'YREN-3203,YREN-3205

            If Session("Flag") = "AddEmployment" Then
                l_bool_flag = True
                l_str_FundStatus = Session("FundStatus")
                If l_str_FundStatus.ToUpper() = "PRE-ELIGIBLE" Then      ''updated by sanjay on 24 Aug 09 for YRS 5.0-874
                    l_emp_datatable = CType(Session("EmploymentDataTable"), DataTable)
                    l_str_YMCANo = Session("YmcaNo")
                    If l_emp_datatable.Rows.Count > 0 Then
                        For Each l_emp_datarow In l_emp_datatable.Rows
                            If l_emp_datarow("YmcaNo") = l_str_YMCANo Then
                                If IsDBNull(l_emp_datarow("TermDate")) = True Then
                                    l_bool_flag = False
                                    l_str_msg = "No person can have simultaneous multiple active employments at the same YMCA."
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            'Shubhrata Validations for Hire date and term date
            Dim l_string_Output As String
            Dim l_int_Professional As Integer
            Dim l_int_Exempt As Integer
            Dim l_int_FullTime As Integer

            'Vipul 03Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()

            Dim dr As DataRow

            Dim l_dataset_Employment As New DataSet

            'Vipul 03Feb06 Cache-Session
            'l_dataset_Employment = CType(Cache("l_dataset_Employment"), DataSet)
            l_dataset_Employment = DirectCast(Session("l_dataset_Employment"), DataSet)
            'Vipul 03Feb06 Cache-Session
            If Session("Flag") = "AddEmployment" Then
                If l_bool_flag = False Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_str_msg, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            If CheckBoxProfessional.Checked Then
                l_int_Professional = 1
            Else
                l_int_Professional = 0
            End If
            If CheckBoxExempt.Checked Then
                l_int_Exempt = 1
            Else
                l_int_Exempt = 0
            End If
            If CheckBoxFullTime.Checked Then
                l_int_FullTime = 1
            Else
                l_int_FullTime = 0
            End If
            ' START: SB | 03/23/2017 | YRS-AT-2606 | After Checking Enrolment should not be less than Display message 
            If flag = False Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment Date must be atleast 1 year greater than Hire Date.", MessageBoxButtons.Stop)
                Exit Sub
            End If
            ' END: SB | 03/23/2017 | YRS-AT-2606 | After Checking Enrolment should not be less than Display message

            ' START: SB | 03/23/2017 | YRS-AT-2606 | Check if the Entered enrolment date should notoverlapwith other employment records and display proper message if overlaps
            If (ValidateEnrolmentDate() = False) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment date you have entered overlaps with another employment record at the same Y. Enter a different enrollment date or modify the prior employment.", MessageBoxButtons.Stop)
                Exit Sub
            End If
            ' END: SB | 03/23/2017 | YRS-AT-2606 | Check if the Entered enrolment date should notoverlapwith other employment records and display proper message if overlaps

            If Session("Flag") = "AddEmployment1" Then

                'Code for adding row to datatable


                'If flag = True Then      'SB | 03/23/2017 | YRS-AT-2606 | No need to check again this condition as it is already checked 
                Session("Flag") = "AddEmployment"
                dr = l_dataset_Employment.Tables(0).NewRow
                dr("PersonId") = Session("PersId")
                dr("YmcaId") = Session("UniqueIdM")
                dr("FundEventId") = Session("FundId")
                dr("BranchId") = IIf(IsNothing(Session("UniqueIdB")), System.DBNull.Value, Session("UniqueIdB"))

                'YRS 5.0-1307 - Handling the null values in Hire date.
                'dr("HireDate") = TextBoxHireDate.Text
                If TextBoxHireDate.Text = "" Then
                    dr("HireDate") = DBNull.Value
                Else
                    dr("HireDate") = TextBoxHireDate.Text
                End If

                'YRS 5.0-1307 - Handling the null values in Termination date.
                'dr("Termdate") = TextBoxTermDate.Text
                If TextBoxTermDate.Text = "" Then
                    dr("Termdate") = DBNull.Value
                Else
                    dr("Termdate") = TextBoxTermDate.Text
                End If

                dr("EligibilityDate") = TextBoxEligibilityDate.Text
                dr("Professional") = l_int_Professional
                dr("Salaried") = l_int_Exempt
                dr("FullTime") = l_int_FullTime
                dr("PriorService") = Convert.ToInt32(TextBoxPriorService.Text)
                dr("StatusType") = DropDownListStatusType.SelectedValue
                dr("Status") = DropDownListStatusType.SelectedItem.Text
                dr("StatusDate") = TextBoxStatusDate.Text
                dr("YmcaName") = TextBoxYMCA.Text
                dr("PositionType") = Session("TitleType")
                dr("YmcaNo") = Session("YmcaNo")
                dr("PositionDesc") = Session("TitleR")

                'YRS 5.0-1307 - Handling the null values in Enrollment date.
                'dr("BasicPaymentDate") = TextBoxEnrollmentdate.Text
                If TextBoxEnrollmentdate.Text = "" Then
                    dr("BasicPaymentDate") = DBNull.Value
                Else
                    dr("BasicPaymentDate") = TextBoxEnrollmentdate.Text
                End If

                If dr("Status") = "Active" Then
                    dr("Active") = 1
                Else
                    dr("Active") = 0

                End If

                'Vipul 03Feb06 Cache-Session
                'Cache.Add("l_dataset_Employment", l_dataset_Employment)
                Session("l_dataset_Employment") = l_dataset_Employment
                'Vipul 03Feb06 Cache-Session

                'l_string_Output = YMCARET.YmcaBusinessObject.AddEmploymentBOClass.AddEmployment(Session("PersId"), Session("UniqueIdM"), Session("FundId"), Session("UniqueIdB"), TextBoxHireDate.Text, TextBoxTermDate.Text, TextBoxEligibilityDate.Text, Session("TitleType"), l_int_Professional, l_int_Exempt, l_int_FullTime, Convert.ToInt32(TextBoxPriorService.Text), DropDownListStatusType.SelectedValue, TextBoxStatusDate.Text, TextBoxEnrollmentdate.Text, 1)

                ' START: SB | 03/23/2017 | YRS-AT-2606 | Following code handled at the begining of the function 
                'Else
                '    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment Date must be atleast 1 year greater than Hire Date.", MessageBoxButtons.Stop, True)
                '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment Date must be atleast 1 year greater than Hire Date.", MessageBoxButtons.Stop)
                '    'Session("DateValidation") = "date"
                '    Exit Sub
                'End If
                ' END: SB | 03/23/2017 | YRS-AT-2606 | Following code handled at the begining of the function 

            ElseIf Session("Flag") = "Edited" Then

                'If flag = False And Session("DateValidation") <> "date" Then
                '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment Date must be atleast 1 year greater than Hire Date.", MessageBoxButtons.Stop, True)
                '    Session("DateValidation") = "date"
                '    Exit Sub
                ' START: SB | 03/23/2017 | YRS-AT-2606 | Following code handled at the begining of the function 
                'If flag = False Then
                '    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment Date must be atleast 1 year greater than Hire Date.", MessageBoxButtons.Stop, True)
                '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enrollment Date must be atleast 1 year greater than Hire Date.", MessageBoxButtons.Stop)
                '    Exit Sub
                'Else
                ' END: SB | 03/23/2017 | YRS-AT-2606 | Following code handled at the begining of the function 

                'Session("DateValidation") = ""
                Dim drRows() As DataRow

                If Not Request.QueryString("UniqueIdP") Is Nothing Then
                    drRows = l_dataset_Employment.Tables(0).Select("UniqueId='" & Request.QueryString("UniqueIdP") & "'")
                    dr = drRows(0)

                    'YRS 5.0-1307 - Handling the null values in Hire date.
                    'dr("HireDate") = TextBoxHireDate.Text
                    If TextBoxHireDate.Text = "" Then
                        dr("HireDate") = DBNull.Value
                    Else
                        dr("HireDate") = TextBoxHireDate.Text
                    End If

                    'YRS 5.0-1307 - Handling the null values in Termdate.
                    'dr("Termdate") = TextBoxTermDate.Text
                    If TextBoxTermDate.Text = "" Then
                        dr("Termdate") = DBNull.Value
                    Else
                        dr("Termdate") = TextBoxTermDate.Text
                    End If


                    'added by hafiz on 15-Nov-2006 for YREN-2853
                    dr("PriorService") = Convert.ToInt32(TextBoxPriorService.Text)

                    'YRS 5.0-1307 - Handling the null values in Enrollment date.
                    'dr("BasicPaymentDate") = TextBoxEnrollmentdate.Text
                    If TextBoxEnrollmentdate.Text = "" Then
                        dr("BasicPaymentDate") = DBNull.Value
                    Else
                        dr("BasicPaymentDate") = TextBoxEnrollmentdate.Text
                    End If

                    If TextBoxTermDate.Text.Trim <> "" Then
                        dr("StatusType") = DropDownListStatusType.SelectedValue
                        dr("Status") = DropDownListStatusType.SelectedItem.Text
                        dr("StatusDate") = TextBoxStatusDate.Text
                    End If
                    'START : Added by Dilip yadav : 11-Nov-2009 : YRS 5.0.941
                    dr("Professional") = CheckBoxProfessional.Checked
                    dr("Salaried") = CheckBoxExempt.Checked
                    dr("FullTime") = CheckBoxFullTime.Checked
                    dr("PositionDesc") = TextBoxTitle.Text
                    dr("PositionType") = Session("TitleType")
                    'END : Added by Dilip yadav : 11-Nov-2009 : YRS 5.0.941

                    'Vipul 03Feb06 Cache-Session
                    'Cache.Add("l_dataset_Employment", l_dataset_Employment)
                    Session("l_dataset_Employment") = l_dataset_Employment
                    'Vipul 03Feb06 Cache-Session
                End If

                If Not Request.QueryString("Index") Is Nothing Then
                    dr = l_dataset_Employment.Tables(0).Rows(Request.QueryString("Index"))

                    'YRS 5.0-1307 - Handling the null values in Hiredate.
                    'dr("HireDate") = TextBoxHireDate.Text
                    If TextBoxHireDate.Text = "" Then
                        dr("HireDate") = DBNull.Value
                    Else
                        dr("HireDate") = TextBoxHireDate.Text
                    End If

                    'YRS 5.0-1307 - Handling the null values in Termdate.
                    'dr("Termdate") = TextBoxTermDate.Text
                    If TextBoxTermDate.Text = "" Then
                        dr("Termdate") = DBNull.Value
                    Else
                        dr("Termdate") = TextBoxTermDate.Text
                    End If

                    'YRS 5.0-1307 - Handling the null values in Enrollment date.
                    'dr("BasicPaymentDate") = TextBoxEnrollmentdate.Text
                    If TextBoxEnrollmentdate.Text = "" Then
                        dr("BasicPaymentDate") = DBNull.Value
                    Else
                        dr("BasicPaymentDate") = TextBoxEnrollmentdate.Text
                    End If


                    If TextBoxTermDate.Text.Trim <> "" Then
                        dr("StatusType") = DropDownListStatusType.SelectedValue
                        dr("Status") = DropDownListStatusType.SelectedItem.Text
                        dr("StatusDate") = TextBoxStatusDate.Text
                    End If
                    'Vipul 03Feb06 Cache-Session
                    'Cache.Add("l_dataset_Employment", l_dataset_Employment)
                    Session("l_dataset_Employment") = l_dataset_Employment
                    'Vipul 03Feb06 Cache-Session
                End If
            End If
            'End If      'SB | 03/23/2017 | YRS-AT-2606 | If condition is replaced as validation is done by other function for enrolment date 

            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.document.forms(0).submit();"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)

            'If flag = True Or Session("DateValidation") = "date" Then


            'End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            'Dim closeWindow As String = "<script language='javascript'>" & _
            '                                              "self.close();" & _
            '                                              "</script>"



            'Response.Write(closeWindow)


            Session("Flag") = ""

            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"
            'msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub Popcalendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Popcalendar1.SelectionChanged
        Try
            Dim l_date As Date
            l_date = Popcalendar1.SelectedDate
            If l_date.Day = 1 Then
                TextBoxTermDate.Text = l_date
            ElseIf l_date.Day <> 1 Then
                l_date.AddMonths(1)
                l_date = Convert.ToString(l_date.Month()) + "/" + "1/" + Convert.ToString(l_date.Year())
                TextBoxTermDate.Text = l_date
            End If
            ''Me.DropDownListStatusType.Enabled = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub Popcalendar3_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Popcalendar3.SelectionChanged
        Try
            Dim l_date As Date
            l_date = Popcalendar3.SelectedDate
            If l_date.Day <> 1 Then
                l_date.AddMonths(1)
                l_date = Convert.ToString(l_date.Month()) + "/" + "1/" + Convert.ToString(l_date.Year())
                TextBoxEnrollmentdate.Text = l_date
            Else
                TextBoxEnrollmentdate.Text = l_date
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub
    'start : Added by Dilip  yadav : YRS 5.0.941
    Private Sub ClearSession()
        Session("HireDateP") = Nothing
        Session("TermDateP") = Nothing
        Session("EligibilityDateP") = Nothing
        Session("ProfessionalP") = Nothing
        Session("ExemptP") = Nothing
        Session("FullTimeP") = Nothing
        Session("PriorServiceP") = Nothing
        Session("StatusTypeP") = Nothing
        Session("StatusDateP") = Nothing
        Session("YmcaNameP") = Nothing
        Session("PositionTypeP") = Nothing
        Session("PositionDescP") = Nothing
        Session("BasicPaymentDateP") = Nothing
    End Sub
    'End : Added by Dilip  yadav : YRS 5.0.941

    ' START: SB | 03/23/2017 | YRS-AT-2606 | Check Valid Enrolment date that doesnot overlap wih other employments for same YMCA
    Private Function ValidateEnrolmentDate() As Boolean
        Dim ValidEnrolmentDate As Boolean = False
        ' START: SB | 06/13/2017 | YRS-AT-3410 | Check if the enrollment date is blank return true as these validation is not required if enrollment date is not present
        If (TextBoxEnrollmentdate.Text.Trim() = "") Then
            Return True
        End If
        ' END: SB | 06/13/2017 | YRS-AT-3410 | Check if the enrollment date is blank return true as these validation is not required if enrollment date is not present
        Dim l_enrol_date As DateTime = Convert.ToDateTime(TextBoxEnrollmentdate.Text).ToString("MM/dd/yyyy")
        Dim dtrows As DataRow()
        Dim dsEmployments As DataSet
        Dim dtEmploymentsUnderSameY As DataTable
        Dim drowsEmploymentsUnderSameY As DataRow()
        Dim strSelectedYMCAID As String
        Dim strEmpEventId As String

        strEmpEventId = Request.QueryString("UniqueIdP")
        If HelperFunctions.isEmpty(strEmpEventId) And Not Session("UniqueIdM") Is Nothing Then     'add employment mode
            strSelectedYMCAID = Session("UniqueIdM")
        Else                                        'edit employment mode
            strSelectedYMCAID = Convert.ToString(Session("SelectedYMCAID"))
        End If

        'Getting all the employment of  YMCA excludning selecting Selected YMCA 
        If Not Session("SelectedYMCAID") Is Nothing And Not Session("l_dataset_Employment") Is Nothing Then
            dsEmployments = Session("l_dataset_Employment")

            If HelperFunctions.isEmpty(strEmpEventId) Then
                drowsEmploymentsUnderSameY = dsEmployments.Tables(0).Select(String.Format("YmcaId='{0}'", strSelectedYMCAID))  '.DefaultView.RowFilter = "YmcaId='"& SelectedYCMAIdEmploymentRecords &"'")
            Else
                drowsEmploymentsUnderSameY = dsEmployments.Tables(0).Select(String.Format("YmcaId='{0}' AND UniqueId <> '{1}'", strSelectedYMCAID, strEmpEventId))  '.DefaultView.RowFilter = "YmcaId='"& SelectedYCMAIdEmploymentRecords &"'")
            End If

            dtEmploymentsUnderSameY = dsEmployments.Tables(0).Clone()
            For Each Row As DataRow In drowsEmploymentsUnderSameY '
                dtEmploymentsUnderSameY.ImportRow(Row)
            Next
        End If
        'Checking if the enrolment date is not overlaoing with other employments for the same Y
        dtrows = dtEmploymentsUnderSameY.Select(String.Format("HireDate <= #{0}# AND ISNULL(Termdate, #{2}#) >= #{1}# ", l_enrol_date, l_enrol_date, System.DateTime.Now().ToString("MM/dd/yyyy")))
        If (dtrows.Length = 0) Then
            ValidEnrolmentDate = True
        End If

        Return ValidEnrolmentDate
    End Function
    ' END: SB | 03/23/2017 | YRS-AT-2606 | Check Valid Enrolment date that doesnot overlap wih other employments for same YMCA
End Class


