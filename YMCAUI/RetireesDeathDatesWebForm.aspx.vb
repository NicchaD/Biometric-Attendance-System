'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RetireesDeathDatesWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 5:06:40 PM
' Program Specification Name	: Doc 3.1.2
' Unit Test Plan Name			:	
' Description					:	This is a Date Death Select pop up window of Beneficiaries Tab.
'*******************************************************************************
'******************************************************************************************************
'Modification History
'********************************************************************************************************
'Modified By        Date                        Description
'********************************************************************************************************
'Aparna Samala      12 Sep 2007                 New validations for checking Death Date.Check for the value in the AtsMetacinfitable
'Neeraj Singh       12/Nov/2009                 Added form name for security issue YRS 5.0-940 
'BhavnS             2012.03.16                  For BT-1015,YRS 5.0-1557: Death Notification Button disappears
'Anudeep            28.02.2013                  Bt-1303-YRS 5.0-1707:New Death Benefit Application form
'Manthan Rajguru    2015.09.16                  YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Santosh Bura       2017.07.31                  YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Santosh Bura       2018.01.11                  YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'********************************************************************************************************

Public Class RetireesDeathDatesWebForm
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("RetireesDeathDatesWebForm.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBoxDeathDate As YMCAUI.DateUserControl
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelEnterDeathDate As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    'Anudeep:27.02.2013 Added tbForms,tbHeader for Bt:1303 - YRS 5.0-1707:New Death Benefit Application form 
    'Commented by anudeep 08.05.2013-to not to show report in participant maintanance
    'Protected WithEvents tbForms As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents tbHeader As System.Web.UI.HtmlControls.HtmlTable
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl  'SB | 31/07/2017 | YRS-AT-3324 | A Div control placed on the top of the screen box to contain dynamic message related to notificatiom status.  


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.TextBoxDeathDate.RequiredDate = True
            'Me.LabelEnterDeathDate.AssociatedControlID = Me.TextBoxDeathDate.ID
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            If Not Me.IsPostBack Then
                'START: SB | 2017.07.28 | YRS-AT-3324 | Version - 20.4.3 |  Following validation code has been shfited on Ok click button
                'START: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate withdrawal and death notification restricitions for RMD eligbile participants
                'Dim DeathNotification As Boolean
                'Dim ReasonForRestriction As String
                'DeathNotification = Validation.IsRMDExist(YMCAObjects.Module.Death_Notification, Session("FundId"), ReasonForRestriction)
                'If Not DeathNotification Then
                '    DivMainMessage.InnerHtml = ReasonForRestriction
                '    DivMainMessage.Visible = True
                '    ButtonOk.Enabled = False
                '    Exit Sub
                'End If
                'END: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate withdrawal and death notification restricitions for RMD eligbile participants
                'END: SB | 2017.07.28 | YRS-AT-3324 | Version - 20.4.3 |  Following validation code has been shfited on Ok click button

                If Not Session("Date_Deceased") Is Nothing Then
                    Me.TextBoxDeathDate.Text = Session("Date_Deceased")
                    'BS:2012.03.16:BT-1015,YRS 5.0-1557:-here enable TextboxDeathDate for Death Notification if date of death exist
                    'Me.TextBoxDeathDate.Enabled = False
                    'Me.ButtonOk.Enabled = False
                Else
                    Me.TextBoxDeathDate.Text = ""
                    Me.TextBoxDeathDate.Enabled = True
                    'Anudeep:28.02.2013 Added below line For Bt-1303:YRS 5.0 1707:New Death Benefit Application form
                    'Checking whether particiapant has surviour who is not primary beneficiary
                    'Commented by anudeep 08.05.2013-to not to show report in participant maintanance
                    'Check_SurviourIsNotBeneficiary()
                End If
            Else
                Dim l_date_PreJuly As String
                Dim l_string_Message As String = String.Empty
                If Request.Form("Yes") = "Yes" Then
                    If Not Session("FundStatusType") Is Nothing Then
                        If Session("FundStatusType").ToString.ToUpper.Trim() = "RT" Then
                            l_date_PreJuly = "7/1/2006"
                            If (Date.Compare(Convert.ToDateTime(TextBoxDeathDate.Text), Convert.ToDateTime(l_date_PreJuly)) < 0) Then
                                l_string_Message = "Cannot process a death dated prior to 7/1/2006 for a fund status of RT.  Please refer to System Administrator."
                                MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop, False)
                                Exit Sub
                            Else
                                Session("DeathDate") = Me.TextBoxDeathDate.Text.Trim()
                                Session("DeathNotification") = True
                                CloseForm()
                            End If
                        Else
                            Session("DeathDate") = Me.TextBoxDeathDate.Text.Trim()
                            Session("DeathNotification") = True
                            CloseForm()
                        End If
                    End If

                ElseIf Request.Form("No") = "No" Then
                    ResetSessionWhenCancelled()
                    CloseForm()

                ElseIf Request.Form("Ok") = "OK" Then
                    CloseForm()
                End If

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            ' VP 13Sept07 Using Functions instead of code
            'Session("Flag") = ""
            'Session("DeathNotification") = False
            'Dim closeWindow As String = "<script language='javascript'>" & _
            '                                              "self.close();" & _
            '                                              "</script>"

            'Response.Write(closeWindow)
            ResetSessionWhenCancelled()
            CloseForm()
            ' VP 13Sept07 Using Functions instead of code

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Dim l_dataset_DthNotify As DataSet
        Dim l_Date_MetaConfigMonths As Integer
        Dim l_date As Date
        Dim l_date_PreJuly As String
        Dim l_date_NumberofMonths As Integer
        Dim l_string_Message As String = String.Empty
        Dim l_string_MetaConfigKey As String = String.Empty
        Dim l_boolflag As Boolean = True
        Try
            l_date = System.DateTime.Today

            'check if date is greater than today
            If TextBoxDeathDate.Text <> "" Then
                If (Date.Compare(Convert.ToDateTime(TextBoxDeathDate.Text), l_date) > 0) Then
                    l_string_Message = "The Death Notification date cannot be greater than today's date."
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop, False)
                    l_boolflag = False
                    Exit Sub
                End If
            Else
                'Anudeep:28.02.2013 :Added below code for Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
                'On ok click if death date is not entered then all the tbforms table will be destroyed on page load 
                'so on ok click if session variable has been set to yes then table has to rebinded
                'Commented by anudeep 08.05.2013-to not to show report in participant maintanance
                'If Not Session("ShowDeathBenefitForm") Is Nothing Then
                '    AdditionalForms()
                '    Session("Formlist") = Nothing
                'End If
                Exit Sub
            End If

            'validate the date of death upon input
            ' get the nmber of months from the atsMetaConfiguration table
            'and if the date of date is the earlier than the current date minus the number of months in that record,

            'START: SB | 2018.01.05 | YRS-AT-3324 | Version - 20.4.3 | Added method to validate death notification restricitions for RMD eligbile participants till entered death date
            Dim IsDeathNotificationAllowed As Boolean
            Dim ReasonForRestriction As String
            IsDeathNotificationAllowed = Validation.IsRMDExist(YMCAObjects.Module.Death_Notification, Session("FundId"), ReasonForRestriction, TextBoxDeathDate.Text.Trim())
            If Not IsDeathNotificationAllowed Then
                'ReasonForRestriction = "RMD’s for one or more years through the death year must be generated or regenerated. Check RMD records and use generate/regenerate RMD functions as necessary. Then try again."
                Dim index As Integer = ReasonForRestriction.IndexOf("<br />")
                'For multiple reasons for restricition display message with exteneded height and width
                If index >= 0 Then
                    MessageBox.Show(170, 300, 560, 175, PlaceHolder1, "YMCA-YRS", ReasonForRestriction, MessageBoxButtons.Stop, False)
                    Exit Sub
                End If
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", ReasonForRestriction, MessageBoxButtons.Stop, False)
                Exit Sub
            End If
            'END: SB | 2018.01.05 | YRS-AT-3324 | Version - 20.4.3 | Added method to validate death notification restricitions for RMD eligbile participants till entered death date

            l_string_MetaConfigKey = "DTH_NOTIFY_VALIDATE_MONTHS"
            l_dataset_DthNotify = YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetDeathNotificationConfig(l_string_MetaConfigKey.Trim())

            If Not l_dataset_DthNotify Is Nothing Then
                If l_dataset_DthNotify.Tables(0).Rows.Count > 0 Then
                    l_Date_MetaConfigMonths = l_dataset_DthNotify.Tables(0).Rows(0)("Value")
                    'get the date = the current date minus the number of months in the config record
                    l_date = System.DateTime.Today.AddMonths(-l_Date_MetaConfigMonths)
                    'l_date_NumberofMonths = DateDiff(DateInterval.Month, Convert.ToDateTime(TextBoxDeathDate.Text), l_date)
                    'If l_date_NumberofMonths > 0 Then
                    If Convert.ToDateTime(TextBoxDeathDate.Text) <= l_date Then
                        l_string_Message = "Date of death is greater than " + l_Date_MetaConfigMonths.ToString() + " months ago.  Do you wish to continue?"
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.YesNo, False)
                        l_boolflag = False
                        Exit Sub
                    End If
                Else
                    l_string_Message = "The value for the key DTH_NOTIFY_VALIDATE_MONTHS needs to be defined in the Configuration table"
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.OK, False)
                    l_boolflag = False
                    Exit Sub
                End If
            End If

            'check if the fundevent status is 'RT' and the date of death < Jul-1-2006
            If Not Session("FundStatusType") Is Nothing Then
                If Session("FundStatusType").ToString.Trim() = "RT" Then
                    l_date_PreJuly = "7/1/2006"
                    If (Date.Compare(Convert.ToDateTime(TextBoxDeathDate.Text), Convert.ToDateTime(l_date_PreJuly)) < 0) Then
                        l_string_Message = "Cannot process a death dated prior to 7/1/2006 for a fund stats of RT.  Please refer to System Administrator."
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.Stop, False)
                        l_boolflag = False
                        Exit Sub
                    End If
                End If
            End If

            'by Aparna 12/09/2007
            'If l_string_Message <> String.Empty Then
            If l_boolflag Then

                Session("DeathDate") = Me.TextBoxDeathDate.Text.Trim()
                Session("DeathNotification") = True

                'VP 13sept07 Use CloseForm() instead 
                'Dim msg As String
                'msg = msg + "<Script Language='JavaScript'>"
                'msg = msg + "window.opener.document.forms(0).submit();"
                'msg = msg + "self.close();"
                'msg = msg + "</Script>"
                'Response.Write(msg)
                CloseForm()
                'VP 13sept07 Use CloseForm() instead 

            End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub
    Public Sub CloseForm()
        Try

            'VP 13Sept07 Restoring the flag 
            Session("Flag") = ""
            'VP 13Sept07 Restoring the flag 

            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"

            Response.Write(msg)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ResetSessionWhenCancelled()
        Try
            'VP 13Sept07 Restoring the flag 
            Session("Flag") = ""
            Session("DeathNotification") = False
            'VP 13Sept07 Restoring the flag 

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    'Start: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
    'Commented by anudeep 08.05.2013-to not to show report in participant maintanance
#Region "DeathNotification "
    'on click of Show form this event fill fire this will get the selected data and stores into session variable
    '<System.Web.Services.WebMethod()> _
    'Public Shared Function ShowFormclick(ByVal Formlist As String) As String
    '    Dim DeathBenefit As New DeathBenefitsCalculatorForm
    '    Try
    '        If HttpContext.Current.Session("Formlist") Is Nothing Then
    '            HttpContext.Current.Session("Formlist") = Formlist
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Function

    ''Gets Forms list from database and Displays in the screen
    'Public Sub AdditionalForms()
    '    Dim row As HtmlTableRow
    '    Dim cell1 As HtmlTableCell
    '    Dim cell2 As HtmlTableCell
    '    Dim Chk As CheckBox
    '    Dim txt As TextBox
    '    Dim img As Image
    '    Dim strInnerhtml As String
    '    Dim l_dataset_Additonal_Forms As DataSet

    '    Try

    '        If tbForms Is Nothing Then
    '            Exit Sub
    '        End If

    '        If tbForms.Rows.Count <> 0 Then
    '            tbForms.Rows.Clear()
    '        End If

    '        ' get the form details and store in dataset
    '        l_dataset_Additonal_Forms = Session("DataSet_AdditionalForms")

    '        If l_dataset_Additonal_Forms Is Nothing Then
    '            l_dataset_Additonal_Forms = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetMetaAdditionalForms()
    '            Session("DataSet_AdditionalForms") = l_dataset_Additonal_Forms
    '        End If

    '        'If there is no forms defined then show only that no forms found in atsMetaAdditionalForms
    '        If HelperFunctions.isEmpty(l_dataset_Additonal_Forms) Then
    '            cell1 = New HtmlTableCell
    '            cell1.InnerHtml = "No forms available in database"
    '            Exit Sub
    '        End If

    '        For i As Integer = tbForms.Rows.Count To l_dataset_Additonal_Forms.Tables(0).Rows.Count - 1
    '            row = New HtmlTableRow
    '            cell1 = New HtmlTableCell
    '            cell2 = New HtmlTableCell
    '            Chk = New CheckBox

    '            ' checks if the person has more than $5000 balace then it will show forms which has bitvalidateBalance is set true
    '            If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("bitValidateBalance") Then

    '                Try
    '                    If Not Session("FundId") Is Nothing Then
    '                        If YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetBasicAccountBalance(Session("FundId")) <= 5000 Then
    '                            Continue For
    '                        End If
    '                    End If
    '                Catch
    '                    'if any error returns while caluculating the balance then that form will not be shown
    '                    Continue For
    '                End Try
    '            End If

    '            Chk.ID = "chk" + l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("intUniqueID").ToString()
    '            Chk.Checked = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("bitDefaultSelected")
    '            Chk.CssClass = "checkbox"
    '            cell1.Controls.Add(Chk)
    '            row.Cells.Insert(0, cell1)
    '            cell2.Attributes.Add("class", "table_cell")

    '            'For the form "A copy of the death certificate for" the name is concatinated to the form label
    '            If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString().Trim() = "A copy of the death certificate for" And Not Session("FirstName") Is Nothing And Not Session("LastName") Is Nothing Then
    '                strInnerhtml = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString() + " " + Session("FirstName") + " " + Session("LastName") + " "

    '            Else
    '                strInnerHtml = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString() + " "
    '            End If
    '            cell2.InnerHtml = strInnerhtml
    '            ' If bitadditionalInfo is set to true then it should it adds the textbox
    '            If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("bitAdditionalInfo") Then
    '                txt = New TextBox
    '                img = New Image
    '                txt.ID = "txt" + l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("intUniqueID").ToString()
    '                txt.CssClass = "textbox_center"
    '                If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText") = "Other" Then
    '                    txt.CssClass = "textbox_center_widebox"
    '                End If
    '                'img.Style.Value = "vertical-align:middle;"
    '                img.ToolTip = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvAdditonalHelpText")
    '                'img.Width = 20
    '                'img.Height = 20
    '                img.ImageUrl = "~/images/help.jpg"
    '                img.CssClass = "img_help_small"
    '                cell2.Controls.Add(txt)
    '                cell2.Controls.Add(img)
    '            End If
    '            row.Cells.Insert(1, cell2)
    '            tbForms.Rows.Insert(tbForms.Rows.Count, row)
    '        Next
    '        Session("ShowDeathBenefitForm") = "Yes" 'Setting the session variable for showing deathbenefit form after death notification completed
    '    Catch
    '        Throw
    '    End Try
    'End Sub

    ''Checking in database with respect to persid that person has surviour who is not primary deathbeneficiary
    'Public Sub Check_SurviourIsNotBeneficiary()
    '    Dim l_bool_verify As Boolean
    '    Try
    '        If Not Session("PersId") Is Nothing Then
    '            l_bool_verify = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.GetJSBeneficiaries(Session("PersId").ToString())
    '        End If
    '        Session("Formlist") = Nothing
    '        'If the person has only surviour who is not death beneficiary then only additional forms and documents are displayed
    '        If l_bool_verify Then
    '            AdditionalForms()
    '        Else
    '            If tbForms Is Nothing Then
    '                Exit Sub
    '            End If
    '            tbForms = FindControl("tbForms")
    '            tbForms.Visible = False
    '            tbHeader.Visible = False
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub

#End Region
    'End: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
End Class
