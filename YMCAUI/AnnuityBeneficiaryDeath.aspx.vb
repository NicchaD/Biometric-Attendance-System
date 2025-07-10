'****************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AnnuityBeneficiaryDeath.aspx.vb
' Author Name		:	Vipul Patel 
' Employee ID		:	32900 
' Email				:	vipul.patel@3i-infotech.com
' Contact No		:	55928738
' Creation Time		:	10/11/2005 
' Program Specification Name	:	YMCA_PS_Annuity_Beneficiary_Death_Settlement.doc
' Unit Test Plan Name			:	
' Description					:	This form is used to Annuity Benficiary Settlement   
'****************************************************************************************
'Cache-Session      :    Vipul 03Feb06 
'****************************************************************************************
'Changed By:preeti On:10thFeb06 IssueId:YRST-2092
'Changed By: Nikunj Patel On:3rdOct2007 IssueId: BugTracker-170 - Death Date cannot be in the future.
'Changed By: Nikunj Patel On:9thOct2007 IssueId: YRPS-3868 - Updating code to allow proper selection of records from the search screen.
'Changed By: Nikunj Patel On:22ndOct2007 Issue Id: YRPS-3868 - Refreshing data after update of death date for one JointAnnuity survivor.
'Changed By: Nikunj Patel On:23rdOct2007 Issue Id: YRPS-3868 - Refreshing data after update of death date. We are splitting the procedure to search and fetch actual data into two.
'Changed By: Nikunj Patel On:31stOct2007 Issue Id: BugTracker-256 - Updated code to refresh data from the database when process is cancelled.
'Changed By: Nikunj Patel On:2009.04.20 Issue Id: Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Neeraj Singh                12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar              17/Feb/2010         Restrict Data Archived Participants To proceed in Find list Except Person 
'Sanjay Rawat                2010.06.09          Loadviewstate and Saveviewstate has changed
'Sanjay R.                   2010.07.12          Code Review changes.(Region,Commentes) 
'Dinesh Kanojia              2012.12.27          FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death - Add one out parameter to return status message from database.            
'B.Jagadeesh                 2015.04.29          FOR BT-2570,YRS 5.0-2380: Converted existed Annuity Benefit Death form integrating with master page and added more functionality to popup annuity type
'Manthan Rajguru             2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Bala			             2016.02.18          YRS-AT-2673 - Remove SSN number from page banner. Annuity Beneficiary screen.
'Santosh Bura                2017.06.14          YRS-AT-2675 -  Annuity beneficiary restrictions. 
'****************************************************************************************

Option Explicit On
Imports System
Imports System.Data
Imports YMCAUI.SessionManager
Imports YMCAObjects.MetaMessageList
Imports YMCARET.CommonUtilities
Imports YMCARET.YmcaBusinessObject


Public Class AnnuityBeneficiaryDeath
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = "AnnuityBeneficiaryDeath.aspx"
    'End issue id YRS 5.0-940

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", True)
            Exit Sub
        End If
        If Not IsPostBack Then
            Try
                WebPerformanceTracer.LogInfoTrace("Annuity Beneficiary Death Form page load", "Page Load Call.")
                WebPerformanceTracer.EndTrace()

                If String.IsNullOrEmpty(SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityId) Then
                    Response.Redirect("FindInfo.aspx?Name=Ann_Ben_Death", True)
                    Exit Sub
                End If
                'Initially the Save/Cancel will not be available
                btnSave.Attributes.Add("disabled", "disabled")
                btnCancel.Attributes.Add("disabled", "disabled")
                RangeValidatorUCDate.MaximumValue = Date.Now.ToShortDateString()
                'Call LoadData to display the selected retiree and beneficiary information on the screen
                LoadData()
                'Start: Bala: 02/18/2016: YRS-AT-2673: Moved to LoadData()
                'Display title in the header section
                'Dim lblModuleName As Label

                'lblModuleName = Master.FindControl("LabelModuleName")
                'If lblModuleName IsNot Nothing Then
                '    lblModuleName.Text = "Activities > Death > Annuity Beneficiary > SSN " & lblBeneficiarySSNo.Text & " - " & lblBeneficiaryName.Text
                'End If
                'End: Bala: 02/18/2016: YRS-AT-2673: Moved to LoadData()
            Catch ex As Exception
                HelperFunctions.LogException("Annuity Beneficiary Death --> Page Load", ex)
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            End Try
        Else
            Dim eventTarget As String = Convert.ToString(Request.Params.Get("__EVENTTARGET"))
            If (eventTarget = "ButtonYes") Then
                'Retreiving event values from javascript __doPostBack method
                Process_BeneficiaryAnnuitySettlement()
            ElseIf (eventTarget = "ButtonSave") Then
                Process_BeneficiaryAnnuitySettlement()
            ElseIf (eventTarget = "IDMCopy") Then

                WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Javascript postback method calling.")
                Dim eventArgument As Boolean = Convert.ToString(Request.Params.Get("__EVENTARGUMENT"))
                Dim intIDMTrackingId As Integer = OpenReportViewer(hdnRetireeFundId.Value, lblBeneficiarySSNo.Text.Trim, hdnPersId.Value, hdnJointSurvivorId.Value, eventArgument)
                'Call update method for updating generated IDMTrackingId into atsPrintLetters

                WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Javascript postback method calling.")
                WebPerformanceTracer.EndTrace()
                If SessionAnnuityBeneficiaryDeath.AnnBeneDeathPrintLettersId IsNot Nothing And intIDMTrackingId <> 0 Then
                    Dim intPrintLettersId As String = SessionAnnuityBeneficiaryDeath.AnnBeneDeathPrintLettersId
                    If Val(intPrintLettersId) <> 0 Then
                        WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Javascript postback method calling.")

                        AnnuityBeneficiaryDeathBOClass.UpdatePrintLetters(intPrintLettersId, intIDMTrackingId)
                        WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Javascript postback method calling.")
                        WebPerformanceTracer.EndTrace()
                    End If
                    SessionAnnuityBeneficiaryDeath.AnnBeneDeathPrintLettersId = Nothing
                End If
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_INITIAL_COMM_LETTER_PRINTED_SUCCESSFULLY)
            End If
        End If
    End Sub
    Private Sub LoadData()
        Dim dsData As DataSet
        Dim dataRow As DataRow 'Bala: 02/18/2016: YRS-AT-2673: Proper declaration.
        Dim dictParam As Dictionary(Of String, String)
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Load data calling.")

            dsData = AnnuityBeneficiaryDeathBOClass.LookUp_AnnuityJointSurvivor(SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityId.ToString.Trim)

            dataRow = dsData.Tables("r_MemberListForDeath").Rows(0)
            'Start: Bala: 02/18/2016: YRS-AT-2673: Assigning datarow values to variables and checking if the variable is empty or null accordingly assigning values to controls.
            Dim strJointSurvivorId As String = String.Empty
            Dim strRetireeId As String = String.Empty
            Dim strPurchaseDate As String = String.Empty
            Dim strDeathDocReceived As String = String.Empty
            Dim strhdnPersId As String = String.Empty
            Dim strRetireeFundId As String = String.Empty
            Dim strDueSinceDays As String = String.Empty
            Dim strRetireeFirstName As String = String.Empty
            Dim strRetireeMiddleName As String = String.Empty
            Dim strRetireeLastName As String = String.Empty
            Dim strBeneficiaryFirstName As String = String.Empty
            Dim strBeneficiaryMiddleName As String = String.Empty
            Dim strBeneficiaryLastName As String = String.Empty
            Dim strSSNo As String = String.Empty
            Dim strBeneficiarySSNo As String = String.Empty
            Dim strDeathDate As String = String.Empty
            Dim strBeneficiaryFundNo As String = String.Empty
            ' START | SR | 2018.06.29 | YRS-AT-3804 | defined variable to store JointSurvivor Annuity details.
            Dim isJointSurvivorAnnuityPayrollSuppress As Boolean
            Dim isOriginalParticipantDeceased As Boolean
            Dim jointSurvivorAnnuityPurchaseDate As String = String.Empty
            Dim jointSurvivorAnnuityExists As Boolean
            Dim jointSurvivorBirthDate As String = String.Empty
            ' END | SR | 2018.06.29 | YRS-AT-3804 | defined variable to store JointSurvivor Annuity details.
            If Not IsDBNull(dataRow) Then
                strJointSurvivorId = dataRow("guiAnnuityJointSurvivorsID").ToString
                strRetireeId = dataRow("guiRetireeID").ToString
                strPurchaseDate = dataRow("dtmPurchaseDate").ToString
                strDeathDocReceived = dataRow("dtmDeathDocReceived").ToString
                strhdnPersId = dataRow("guiPersId").ToString
                strRetireeFundId = dataRow("intFundIdNo").ToString
                strDueSinceDays = dataRow("DueSinceDays").ToString
                strRetireeFirstName = dataRow("First Name").ToString
                strRetireeMiddleName = dataRow("chvRetireeMiddleName").ToString
                strRetireeLastName = dataRow("Last Name").ToString
                strBeneficiaryFirstName = dataRow("chvFirstName").ToString
                strBeneficiaryMiddleName = dataRow("chvMiddleName").ToString
                strBeneficiaryLastName = dataRow("chvLastName").ToString
                strSSNo = dataRow("SS No.").ToString
                strBeneficiarySSNo = dataRow("chrSSNo").ToString
                strDeathDate = dataRow("dtmDeathDate").ToString
                strBeneficiaryFundNo = dataRow("BeneficiaryFundNo").ToString
                ' START | SR | 2018.06.29 | YRS-AT-3804 | Store JointSurvivor Annuity Payroll Suppress status.
                isJointSurvivorAnnuityPayrollSuppress = Convert.ToBoolean(dataRow("JointSurvivorAnnuitySuppress"))
                isOriginalParticipantDeceased = Convert.ToBoolean(dataRow("OriginalParticipantDeceased"))
                jointSurvivorAnnuityPurchaseDate = dataRow("JointSurvivorAnnuityPurchaseDate").ToString
                jointSurvivorAnnuityExists = Convert.ToBoolean(dataRow("JointSurvivorAnnuityExists"))
                jointSurvivorBirthDate = dataRow("JointSurvivorBirthDate").ToString
                ' END | SR | 2018.06.29 | YRS-AT-3804 | Store JointSurvivor Annuity Payroll Suppress status.

            End If
            'Display Retiree's Information
            'lblRetireeName.Text = dataRow("First Name") & " " & dataRow("chvRetireeMiddleName") & " " & dataRow("Last Name")
            'lblRetireeSSNo.Text = dataRow("SS No.")
            lblRetireeName.Text = String.Format("{0} {1}{2}", strRetireeFirstName, IIf(strRetireeMiddleName = "", "", strRetireeMiddleName & " "), strRetireeLastName)
            lblRetireeSSNo.Text = strSSNo
            'Display Beneficiaries Information
            'lblBeneficiaryName.Text = dataRow("chvFirstName") & " " & dataRow("chvMiddleName") & " " & dataRow("chvLastName")
            'lblBeneficiarySSNo.Text = dataRow("chrSSNo")
            'txtDeathDate.Text = dataRow("dtmDeathDate").ToString
            lblBeneficiaryName.Text = String.Format("{0} {1}{2}", strBeneficiaryFirstName, IIf(strBeneficiaryMiddleName = "", "", strBeneficiaryMiddleName & " "), strBeneficiaryLastName)
            lblBeneficiarySSNo.Text = strBeneficiarySSNo
            txtDeathDate.Text = strDeathDate
            If Not String.IsNullOrEmpty(txtDeathDate.Text) Then
                txtDeathDate.Enabled = False
                btnLetter.Enabled = False
            End If

            'Storing Id's in page local variables
            'hdnJointSurvivorId.Value = dataRow("guiAnnuityJointSurvivorsID").ToString
            'hdnRetireeId.Value = dataRow("guiRetireeID").ToString
            'hdnPurchaseDate.Value = dataRow("dtmPurchaseDate").ToString
            'hdnDeathDocReceived.Value = dataRow("dtmDeathDocReceived").ToString
            'hdnPersId.Value = dataRow("guiPersId").ToString
            'hdnRetireeFundId.Value = dataRow("intFundIdNo").ToString
            hdnJointSurvivorId.Value = strJointSurvivorId
            hdnRetireeId.Value = strRetireeId
            hdnPurchaseDate.Value = strPurchaseDate
            hdnDeathDocReceived.Value = strDeathDocReceived
            hdnPersId.Value = strhdnPersId
            hdnRetireeFundId.Value = strRetireeFundId
            'End: Bala: 02/18/2016: YRS-AT-2673: Assigning datarow values to variables and checking if the variable is empty or null accordingly assigning values to controls.
            hdnDueSinceDays.Value = strDueSinceDays     ' SB | 06/14/2017 | YRS-AT-2675 | Contains number of days since first letter sent to participant, It will help to display warning message  

            ' START | SR | 2018.06.29 | YRS-AT-3804 | Store JointSurvivor Annuity Payroll Suppress status.
            hdnJointSurvivorAnnuitySuppress.Value = isJointSurvivorAnnuityPayrollSuppress
            hdnOriginalParticipantDeceased.Value = isOriginalParticipantDeceased
            hdnJointSurvivorAnnuityPurchaseDate.Value = jointSurvivorAnnuityPurchaseDate
            hdnJointSurvivorAnnuityExists.Value = jointSurvivorAnnuityExists
            hdnJointSurvivorBirthDate.Value = jointSurvivorBirthDate
            ' END | SR | 2018.06.29 | YRS-AT-3804 | Store JointSurvivor Annuity Payroll Suppress status.


            If (SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityType IsNot Nothing) Then
                'If annuity type is popup then only Letter Button visible other than invisible
                If (Not SessionAnnuityBeneficiaryDeath.AnnBeneDeathAnnuityType.Contains("P")) Then
                    btnLetter.Visible = False
                Else
                    If String.IsNullOrEmpty(txtDeathDate.Text) Then
                        'If String.IsNullOrEmpty(dataRow("dtmDeathDocReceived").ToString) Then 'Bala: 02/18/2016: YRS-AT-2673: Commented because we are using the variables to assign values
                        If String.IsNullOrEmpty(strDeathDocReceived) Then
                            'If Val(dataRow("DueSinceDays").ToString) > 0 Then 'Bala: 02/18/2016: YRS-AT-2673: Commented because we are using the variables to assign values
                            If Val(strDueSinceDays) > 0 Then
                                dictParam = New Dictionary(Of String, String)
                                'dictParam.Add("DAYS", dataRow("DueSinceDays").ToString) 'Bala: 02/18/2016: YRS-AT-2673: Commented because we are using the variables to assign values
                                dictParam.Add("DAYS", strDueSinceDays)
                                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DUE_SINCE, dictParam)
                            End If
                        Else
                            btnLetter.Enabled = False
                            HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATH_CERTIFICATE_RECEIVED)
                        End If
                    End If
                End If
            End If
            txtLogList.Text = ""
            'Start: Bala: 02/18/2016: YRS-AT-2673: Showing Fund No in the breadcrumb link instead of SSNO
            Dim lblModuleName As Label

            lblModuleName = Master.FindControl("LabelModuleName")
            If lblModuleName IsNot Nothing Then
                If String.IsNullOrEmpty(strBeneficiaryFundNo) = False Then
                    lblModuleName.Text = String.Format("Activities > Death > Annuity Beneficiary > Fund Id {0} - {1}, {2}", strBeneficiaryFundNo, strBeneficiaryLastName, strBeneficiaryFirstName)
                Else
                    lblModuleName.Text = String.Format("Activities > Death > Annuity Beneficiary > {0}, {1}", strBeneficiaryLastName, strBeneficiaryFirstName)
                End If
            End If
            'End: Bala: 02/18/2016: YRS-AT-2673: Showing Fund No in the breadcrumb link instead of SSNO
        Catch ex As Exception
            HelperFunctions.LogException("Annuity Beneficiary Death --> LoadData", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Load data calling.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    Public Function GetMessage(ByVal intMessageKey As Integer, Optional ByRef strParam As String = Nothing, Optional ByRef strParamKey As String = Nothing) As String
        Dim strMessage As String
        Dim dictParam As Dictionary(Of String, String)
        Try
            If strParam Is Nothing Then
                strMessage = MetaMessageBO.GetMessageByTextMessageNo(intMessageKey)
            ElseIf strParam IsNot Nothing Then
                dictParam = New Dictionary(Of String, String)
                dictParam.Add(strParamKey, strParam)
                strMessage = MetaMessageBO.GetMessageByTextMessageNo(intMessageKey, dictParam)
            End If

            Return strMessage
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBenefitDeathFollowup --> GetMessage", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Function
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Session("Page") = "Ann_Ben_Death"
            Response.Redirect("FindInfo.aspx?Name=Ann_Ben_Death", False)
        Catch ex As Exception
            HelperFunctions.LogException("Annuity Beneficiary Death --> ButtonCloseForm_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Button save click.")
            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If

            Dim checkSecurity As String = SecurityCheck.Check_Authorization("DeathAnnuityBeneficiarySave", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If

            btnSave.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")

            If ValidateDeathDate() = True Then
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)
                Dim confirmationMessage As String = MetaMessageBO.GetMessageByTextMessageNo(MESSAGE_ANN_BENE_DEATH_CONFIRM_SAVE)
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "Ann_Bene_Death_Process", "openDialog('" & confirmationMessage & "','SaveYesNo');", True)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Annuity Beneficiary Death --> ButtonSave_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Button save click.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Sub

#Region "ValidateDeathDate"
    Private Function ValidateDeathDate() As Boolean
        Try

            If String.IsNullOrEmpty(txtDeathDate.Text.Trim) Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATHDATE_BLANK)
                Return False
            End If
            If btnLetter.Visible = True Then ' If annuity type is popup then only validate for Death Document Received or Not
                'START: SB | 06/14/2017 | YRS-AT-2675 | Display warning messages only for those participants whose letter is already generated and response is awaiting, previously system was allowing to perform death notification for whom death certificate was received. 
                'If String.IsNullOrEmpty(hdnDeathDocReceived.Value) = True Then 
                If String.IsNullOrEmpty(hdnDeathDocReceived.Value) AndAlso (Not String.IsNullOrEmpty(hdnDueSinceDays.Value)) Then
                    'END: SB | 06/14/2017 | YRS-AT-2675 | Display warning messages only for those participants whose letter is already generated and response is awaiting, previously system was allowing to perform death notification for whom death certificate was received. 
                    HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATHDATE_BEFORE_DOC_RECEIVED)
                    Return False
                End If
            End If

            ' START | SR | 2018.06.29 | YRS-AT-3804 | If annuity beneficiary’s annuity remains suppressed (no payments issued) the allow user to enter annuity beneficiary’s death date before original participant annuity purchased date.
            If Convert.ToBoolean(hdnJointSurvivorAnnuityExists.Value) Then
                If CDate(txtDeathDate.Text) <= CDate(hdnJointSurvivorAnnuityPurchaseDate.Value) AndAlso (Convert.ToBoolean(hdnJointSurvivorAnnuitySuppress.Value) = False) Then
                    ' If CDate(txtDeathDate.Text) < CDate(hdnPurchaseDate.Value) 
                    HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATHDATE_LATER_THAN_ANN_PURCHASE_DATE)
                    'Exit Sub
                    Return False
                End If
            End If

            If CDate(txtDeathDate.Text) <= CDate(hdnJointSurvivorBirthDate.Value) Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATHDATE_LATER_THAN_BIRTH_DATE)
                Return False
            End If
            ' END | SR | 2018.06.29 | YRS-AT-3804 | If annuity beneficiary’s annuity remains suppressed (no payments issued) the allow user to enter annuity beneficiary’s death date before original participant annuity purchased date.

            'NP:PS:2007.10.03 - Adding check to see that the date is not in the future.
            If CDate(txtDeathDate.Text) > CDate(Now.Date) Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATHDATE_NOT_FUTURE)
                Return False
            End If

            If CDate(txtDeathDate.Text) < CDate(Now.Date.AddMonths(-6)) Then
                Dim confirmationMessage As String = MetaMessageBO.GetMessageByTextMessageNo(MESSAGE_ANN_BENE_DEATH_DEATHDATE_OVER_6MONTHS_AGO_CONFIRMATION)
                'If user clicks "Yes" button then Process_BeneficiaryAnnuitySettlement should call here
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "Ann_Bene_Death_Validation", "openDialog('" + confirmationMessage.Trim + "','Over6MonthsYesNo');", True)
                Return False
            End If

            Return True

        Catch ex As Exception
            'Since This is a child control we will just throw back o calling sub
            HelperFunctions.LogException("Annuity Beneficiary Death --> ValidateDeathDate", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Function
#End Region

#Region "Process_BeneficiaryAnnuitySettlement"

    Private Sub Process_BeneficiaryAnnuitySettlement()
        'Define a Status string variable for screen display 
        Dim strDisplayStatusMessage As String
        Dim strErrorMessage As String = String.Empty
        Dim strDeathNotify As String = String.Empty
        Dim strMessage As String = String.Empty
        Try
            'Finally if everything is ok, call the processing method
            'Cross check SSN when entering a date of death             
            'Add Parameter for participant.
            AnnuityBeneficiaryDeathBOClass.Process_BeneficiaryAnnuitySettlement(hdnJointSurvivorId.Value, txtDeathDate.Text, lblRetireeSSNo.Text, strDisplayStatusMessage, strErrorMessage, strDeathNotify)
            txtLogList.Text = strDisplayStatusMessage.Trim

            'DineshK: 2012.12.27: FOR BT-1266, YRS 5.0-1698: Cross check SSN when entering a date of death             
            'If (str_DeathNotify.Trim.Length > 0) Then
            '    Dim l_MessageBox As New MessageBoxClass
            '    'DineshK:2012.12.27: FOR BT-1266, YRS 5.0-1698: Cross check SSN when entering a date of death             
            '    Dim strMessage As String = String.Empty
            '    strMessage = "<b>Death notification succeeded.</b>" + "<br/>" + str_DeathNotify.Trim
            '    Page.RegisterStartupScript("Annuity Beneficiary Death Settlement", "<script language='javascript'> openDialog('" + strMessage + "');</script>")
            '    'MessageBox.Show(PlaceHolder1, "Annuity Beneficiary Death Settlement", strMessage, MessageBoxButtons.Stop)
            '    'MessageBox.Show(170, 300, 350, 150, PlaceHolder1, "Annuity Beneficiary Death Settlement", str_DeathNotify.Trim, MessageBoxButtons.Stop, True)
            '    ''MessageBox.Show(PlaceHolder1, "YMCA YRS", strMessage, MessageBoxButtons.Stop)
            '    GoTo A
            'End If
            'If str_ErrorMessage.Trim.Length > 0 Then
            '    MessageBox.Show(PlaceHolder1, "Annuity Beneficiary Death Settlement", str_ErrorMessage.Trim, MessageBoxButtons.Stop)
            'End If

            If ((Not String.IsNullOrEmpty(strErrorMessage.Trim)) And (Not String.IsNullOrEmpty(strDeathNotify.Trim))) Then
                strMessage = "Death notification succeeded. <br/> " & strErrorMessage + " " + strDeathNotify.Trim
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "Annuity Beneficiary Death Settlement", "openDialog('" + strMessage + "','infDialog');", True)
            ElseIf (Not String.IsNullOrEmpty(strDeathNotify.Trim)) Then
                strMessage = "Death notification succeeded." & "<br/>" & strDeathNotify.Trim
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "Annuity Beneficiary Death Settlement", "openDialog('" + strMessage + "','infDialog');", True)
            ElseIf strErrorMessage.Trim.Length > 0 Then
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "Annuity Beneficiary Death Settlement", "openDialog('" + strErrorMessage.Trim + "','infDialog');", True)
            End If

            'START: SB | 06/14/2017 | YRS-AT-2675 | Display "Death notification Successful" message if no error have occured else display proper warning message to the user when death notification is unsuccessful
            If strDisplayStatusMessage.Trim() = "2" Then
                txtLogList.Text = ""
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATHDATE_BEFORE_DOC_RECEIVED)
            Else
                'END: SB | 06/14/2017 | YRS-AT-2675 | Display "Death notification Successful" message if no error have occured else display proper warning message to the user when death notification is unsuccessful
                HelperFunctions.ShowMessageToUser(MESSAGE_ANN_BENE_DEATH_DEATH_NOTIFICATION_COMPLETED)
                txtDeathDate.Enabled = False
                btnLetter.Enabled = False
                btnSave.Enabled = False
                btnCancel.Enabled = False
            End If 'SB | 06/14/2017 | YRS-AT-2675 | Display "Death notification Successful" message if no error have occured else display proper warning message to the user when death notification is unsuccessful
            'End DineshK: 2012.12.27: FOR BT-1266, YRS 5.0-1698: Cross check SSN when entering a date of death             

            '''Refresh the Search Data 
            'Process_LookupData()
            'Me.DataGridAnnuityBeneficiaryDeathList.SelectedIndex = 1

            'Dim l_dataRowMemberList As DataRow
            'l_dataRowMemberList = l_dataset_SearchResults.Tables(0).DefaultView.Item(Me.DataGridAnnuityBeneficiaryDeathList.SelectedIndex).Row
            'If l_dataRowMemberList Is Nothing Then
            '    'If selection not made exit from here........this is Highly unlikely but still play safe
            '    Exit Sub
            'End If
            'l_dataRowMemberList("dtmDeathDate") = Me.TextBoxBeneficiaryDeathDate.Text
            'l_dataRowMemberList.AcceptChanges()
        Catch ex As Exception
            HelperFunctions.LogException("Annuity Beneficiary Death --> Process_BeneficiaryAnnuitySettlement", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

#End Region
#Region "Open Report Viewer"
    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function GenerateLetters(ByVal strRetireeFundId As String, ByVal strPersId As String, ByVal strJointSurvivorId As String, ByVal bitRecordInitialComm As Boolean, ByVal bitIDMCopy As Boolean) As Integer
        Dim strFundId As String = strRetireeFundId
        Dim strPersonId As String = strPersId
        Dim strSurvivorId As String = strJointSurvivorId
        Dim intPrintLettersId As Integer
        Dim strLettersCode As String = ""
        Dim bitRecInitialComm As Boolean = bitRecordInitialComm
        Dim bitIDM As Boolean = bitIDMCopy

        Try

            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Ajax Calling for Generate Letters.")

            strLettersCode = YMCAObjects.IDMDocumentCodes.Ann_Bene_Death_InitialLetter '"ABDCINIT"

            'First of all we inserting record into atsPrintLetters without IDMTracking id
            If bitRecInitialComm = True Then
                intPrintLettersId = AnnuityBeneficiaryDeathBOClass.InsertPrintLetters(strSurvivorId, strPersonId, strLettersCode)
                SessionAnnuityBeneficiaryDeath.AnnBeneDeathPrintLettersId = intPrintLettersId
            End If

            'Generate and copy a IDMTracking record retreive the IDMTracking Id
            'l_intIDMTrackingId = OpenReportViewer(l_stringRetireeFundId, l_stringPersId, l_stringJointSurvivorId, l_StringReportName, "ABDCINIT", l_bitIDMCopy)
            ''Call update method for updating generated IDMTrackingId into atsPrintLetters
            'If l_bitRecordInitialComm = True Then
            '    Call YMCARET.YmcaBusinessObject.AnnuityBeneficiaryDeathBOClass.UpdatePrintLetters(l_intPrintLettersId, l_intIDMTrackingId)
            'End If
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBeneficiaryDeathForm--> GenerateLetters", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Ajax Calling for Generate Letters.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function
    Private Function OpenReportViewer(ByVal strPersFundId As String, ByVal strBeneSSN As String, ByVal strPersId As String, strRefRequestId As String, bitIDMCopy As Boolean) As Integer
        Dim strDocType As String = String.Empty
        Dim strReportName As String = String.Empty
        Dim arrlstParamValues As New ArrayList
        Dim strParams(1) As String
        Dim strOutputFileType As String = String.Empty
        Dim strErrorMessage As String = String.Empty
        Dim strPersonId As String
        Dim intIDMTrackingId As Integer
        Dim strRefReqId As String
        Dim popupScript As String

        Try

            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Calling open report viewer method.")

            strReportName = "DB_Initial PopUp Letter"
            strDocType = YMCAObjects.IDMDocumentCodes.Ann_Bene_Death_InitialLetter '"ABDCINIT"
            Session("strReportName") = strReportName

            strParams(0) = strPersFundId
            strParams(1) = strBeneSSN
            SessionAnnuityBeneficiaryDeath.AnnBeneDeathInitialLetterParams = strParams

            arrlstParamValues.Add(strPersFundId)
            arrlstParamValues.Add(strBeneSSN)


            'Call ReportViewer.aspx 
            popupScript = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"

            ScriptManager.RegisterStartupScript(Me, GetType(Page), "PopupScriptReport", popupScript, True)

            If bitIDMCopy = True Then
                strOutputFileType = "AnnuityDeathBenefit_" & strDocType
                strPersonId = strPersId
                strRefReqId = strRefRequestId
                strErrorMessage = CopyToIDM(strDocType, strReportName, arrlstParamValues, strOutputFileType, strPersonId, strRefReqId, intIDMTrackingId)

                If Not String.IsNullOrEmpty(strErrorMessage) Then
                    HelperFunctions.ShowMessageToUser(strErrorMessage)
                    Exit Function
                End If

                Return intIDMTrackingId
            End If
        Catch ex As Exception
            HelperFunctions.LogException("AnnuityBeneficiaryDeathForm--> OpenReportViewer", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Calling open report viewer method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function
    Private Function CopyToIDM(ByVal StringDocType As String, ByVal strReportName As String, ByVal arrlstParamValues As ArrayList, ByVal strOutputFileType As String, ByVal strPersId As String, ByVal strRefRequestsId As String, ByRef intIDMTrackingId As Integer) As String
        Dim strErrorMessage As String
        Dim IDM As New IDMforAll
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: Calling CopyToIDM method.")

            'gets the columns for idm and stored in session varilable 
            If Session("FTFileList") Is Nothing Then
                If IDM.DatatableFileList(False) Then
                    Session("FTFileList") = IDM.SetdtFileList
                End If
            End If
            If Not Session("FTFileList") Is Nothing Then
                IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"
            If Not strPersId Is Nothing Then
                IDM.PersId = strPersId
            End If
            If Not strRefRequestsId Is Nothing Then
                IDM.RefRequestsID = strRefRequestsId
            End If
            IDM.DocTypeCode = StringDocType
            IDM.OutputFileType = strOutputFileType
            IDM.ReportName = strReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = arrlstParamValues

            strErrorMessage = IDM.ExportToPDF()

            If IDM.IDMTrackingId <> 0 Then
                intIDMTrackingId = IDM.IDMTrackingId
            End If

            arrlstParamValues.Clear()

            HttpContext.Current.Session("FTFileList") = IDM.SetdtFileList

            If Not HttpContext.Current.Session("FTFileList") Is Nothing Then
                Try
                    Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterStartupScript(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                Catch
                    Throw
                End Try
            End If
            Return strErrorMessage
        Catch
            Throw
        Finally
            IDM = Nothing
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: Calling CopyToIDM method.")
            WebPerformanceTracer.EndTrace()
        End Try
    End Function
#End Region

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtDeathDate.Text = ""
        txtLogList.Text = ""
        btnSave.Attributes.Add("disabled", "disabled")
        btnCancel.Attributes.Add("disabled", "disabled")
        Try
            SessionAnnuityBeneficiaryDeath.AnnBeneDeathPrintLettersId = Nothing
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)
            LoadData()
        Catch ex As Exception
            HelperFunctions.LogException("Annuity Beneficiary Death --> ButtonCancel_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
       
    End Sub

    Private Sub btnLetter_Click(sender As Object, e As EventArgs) Handles btnLetter.Click
        Try
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Start: btnLetter click.")

            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If

            Dim checkSecurity As String = SecurityCheck.Check_Authorization("DeathAnnuityBeneficiaryLetter", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "Ann_Bene_Death_Process", "openDialog('','letterDialog');", True)

        Catch ex As Exception
            HelperFunctions.LogException("Annuity Beneficiary Death --> ButtonSave_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            WebPerformanceTracer.LogPerformanceTrace("AnnuityBeneficiaryDeath", "Finish: btnLetter click.")
            WebPerformanceTracer.EndTrace()
        End Try

    End Sub
End Class