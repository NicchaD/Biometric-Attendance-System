'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	UpdateSSN.aspx.vb
' Author Name		:	Shashank Patel
' Employee ID		:	55381
' Email				:	
' Contact No		:	
' Creation Time		:	01/31/2014
' Program Specification Name	:	YRS 5.0-2198 - Approach document.doc
' Unit Test Plan Name			:	
' Description					:	<<BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN >>
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Santosh Bura       2016.07.07      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annty bene SSN
'Manthan Rajguru    2016.07.27      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru    2016.07.29      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annty bene SSN (TrackIT 19856)
'Manthan Rajguru    2016.08.02      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiaries SSN
'Manthan Rajguru    2018.05.03      YRS-AT-3941 - YRS enh: change "phony SSN" beneficiary label to "placeholder SSN" (TrackIT 33287)
'********************************************************************************************************************************
Imports System.Data.SqlClient 'MMR | 05/10/2018 | YRS-AT-3941 | Added namespace to handle sql exception
Public Class UpdateSSN
    Inherits System.Web.UI.Page
    Private Property SessionPersonID() As String
        Get
            If Not (Session("PersId")) Is Nothing Then
                Return (DirectCast(Session("PersId"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersId") = Value
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim lblPopupmodulename As Label
            lblPopupmodulename = Master.FindControl("lblPopupmodulename")

            If Session("SessionTempSSNTable") Is Nothing Then
                Throw New Exception("Session has expired.")
            End If

            If Not Page.IsPostBack Then
                LoadDetails(lblPopupmodulename)
                Session("IsAnnuityBeneficiariesSSNChanged") = Nothing ' // SB | 07/07/2016 | YRS-AT-2382 | To check is Annuity Beneficiary SSN changed or not
                Session("strOldSSN") = Nothing  '// SB | 07/07/2016 | YRS-AT-2382 | Clearing the Old SSN 
                Session("EnableEditJSBeneficiaryButton") = Nothing 'Manthan Rajguru | 2016.08.02 |YRS-AT-2382 | Setting session variable to nothing
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Page_Load-UpdateSSN", ex)
            Throw ex
        End Try
    End Sub
    Private Sub LoadDetails(ByVal lblPopupmodulename As Label)
        Dim strFundNo, strSSN, strMode, strPageName, strAnnuityJointSurvivorsID As String ' // SB | 07/07/2016 | YRS-AT-2382 | Declared 2 variables strPageName, strAnnuityJointSurvivorsID 
        Try
            If (Not (String.IsNullOrEmpty(Request.QueryString("Name"))) And Not (String.IsNullOrEmpty(Request.QueryString("Mode")))) Then
                strPageName = Request.QueryString("Name").ToLower()   ' // SB | 07/07/2016 | YRS-AT-2382 | To identify which page has requested "Change SSN"
                strFundNo = GetColumnValueFromSession("FundNo", strPageName)
                If (Request.QueryString("Mode") = "ViewSSN") Then
                    strMode = "View SSN Updates"
                ElseIf (Request.QueryString("Mode") = "EditSSN") Then
                    strMode = "Edit SSN"
                End If

                If Request.QueryString("Name").ToLower() = "person" Then
                    If lblPopupmodulename IsNot Nothing Then
                        lblPopupmodulename.Text = "Participant Information - " + strMode + "  - Fund Id - " + strFundNo
                    End If
                ElseIf Request.QueryString("Name").ToLower() = "retire" Then
                    If lblPopupmodulename IsNot Nothing Then
                        lblPopupmodulename.Text = "Retiree Information - " + strMode + " - Fund Id - " + strFundNo
                    End If
                    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Set label for Annuity Beneficiary Update Click
                ElseIf Request.QueryString("Name").ToLower() = "retireandannuitybeneficiary" Then
                    If lblPopupmodulename IsNot Nothing Then
                        lblPopupmodulename.Text = String.Format("Annuity Beneficiary - {0}", strMode)
                    End If
                ElseIf Request.QueryString("Name").ToLower() = "personandbeneficiary" Or Request.QueryString("Name").ToLower() = "retireandbeneficiary" Then
                    If lblPopupmodulename IsNot Nothing Then
                        lblPopupmodulename.Text = String.Format("Death Beneficiary - {0}", strMode)
                    End If
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Set label for Annuity Beneficiary Update Click
                End If

                ' strSSN = GetColumnValueFromSession("OldSSN")     ' // SB | 07/07/2016 | YRS-AT-2382 | Method is now sending two parameters
                strSSN = GetColumnValueFromSession("OldSSN", strPageName)  ' // SB | 07/07/2016 | YRS-AT-2382 | Parameter is added in the current function
                Dim dtAuditBeneficiaryTable As DataTable = CType(Session("AuditBeneficiariesTable"), DataTable)         ' // SB | 07/07/2016 | YRS-AT-2382 | To get unique beneficiary id 
                Dim strBeneficiaryUniqueId As String = String.Empty
                ' // START : SB | 07/07/2016 | YRS-AT-2382 | keeping old SSN in session else blank value
                If strSSN = Nothing Then
                    Session("strOldSSN") = ""
                Else
                    Session("strOldSSN") = strSSN
                End If
                ' // END : SB | 07/07/2016 | YRS-AT-2382 | keeping old SSN in session else blank value
                If Request.QueryString("Mode") = "EditSSN" Then
                    pnlEditSSN.Visible = True
                    labelOldSSN.Text = strSSN
                    BindReasonDropDown()
                    'Start - Manthan Rajguru | 2016.07.29 |YRS-AT-2560| setting Comparevalidator value to compare and enabling client script
                    CompareOldSSNvalidator.ValueToCompare = strSSN
                    CompareOldSSNvalidator.EnableClientScript = True
                    'End - Manthan Rajguru | 2016.07.29 |YRS-AT-2560| setting Comparevalidator value to compare and enabling client script
                ElseIf Request.QueryString("Mode") = "ViewSSN" Then
                    pnlViewSSN.Visible = True
                    btnSSNOk.Visible = False
                    btnSSNCancel.Text = "Close"
                    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Finding from which page it is called and finding unique beneficiary id 
                    If strPageName = "retireandannuitybeneficiary" Then
                        If HelperFunctions.isNonEmpty(dtAuditBeneficiaryTable) Then
                            strAnnuityJointSurvivorsID = Convert.ToString(dtAuditBeneficiaryTable.Rows(0)("UniqueId"))
                            BindGrid(Convert.ToString(Session("AnnuityJointSurvivorsID")), EnumAuditLogColumn.chrSSNo)
                        End If
                    ElseIf strPageName = "personandbeneficiary" Then
                        strBeneficiaryUniqueId = Request.QueryString("BenefID").ToString()
                        If HelperFunctions.isNonEmpty(dtAuditBeneficiaryTable) Then
                            BindGrid(strBeneficiaryUniqueId, EnumAuditLogColumn.chrSSNo)
                        End If
                    ElseIf strPageName = "retireandbeneficiary" Then
                        strBeneficiaryUniqueId = Request.QueryString("BenefID").ToString()
                        If HelperFunctions.isNonEmpty(dtAuditBeneficiaryTable) Then
                            BindGrid(strBeneficiaryUniqueId, EnumAuditLogColumn.chrSSNo)
                        End If
                        ' // END : SB | 07/07/2016 | YRS-AT-2382 | Finding from which page it is called and finding unique beneficiary id 
                    Else
                        BindGrid(SessionPersonID, EnumAuditLogColumn.chrSSNo)
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub BindReasonDropDown()
        Dim dsReasons As DataSet
        Try
            dsReasons = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.SearchLookups("ChangeSSNReason")
            ddlReason.DataSource = dsReasons
            ddlReason.DataTextField = "Desc"
            ddlReason.DataValueField = "Code Value"
            ddlReason.DataBind()
            ddlReason.Items.Insert(0, New ListItem("-Select-", "-Select-"))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Previous function commented and replaced by new function 
    'Private Function GetColumnValueFromSession(ByVal strColumnName As String) As String

    '    Dim strColumnValue As String
    '    Try
    '        strColumnValue = String.Empty
    '        Dim dtTempTable As New DataTable()
    '        If Not (Session("SessionTempSSNTable") Is Nothing) Then
    '            dtTempTable = CType(Session("SessionTempSSNTable"), DataTable)
    '            strColumnValue = (dtTempTable.Rows(0)(strColumnName)).ToString()
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    '    Return strColumnValue
    'End Function

    Private Function GetColumnValueFromSession(ByVal strColumnName As String, ByVal strPageName As String) As String
        Dim strColumnValue As String
        Dim dtTempTable As New DataTable()
        Dim drTempTable As DataRow()

        Try
            strColumnValue = String.Empty

            If strPageName = "person" Or strPageName = "retire" Then
                If Not (Session("SessionTempSSNTable") Is Nothing) Then
                    dtTempTable = CType(Session("SessionTempSSNTable"), DataTable)
                    drTempTable = dtTempTable.Select()
                End If
            ElseIf strPageName = "retireandannuitybeneficiary" Then
                If Not (Session("AuditBeneficiariesTable") Is Nothing) Then
                    dtTempTable = CType(Session("AuditBeneficiariesTable"), DataTable)
                    drTempTable = dtTempTable.Select(String.Format("UniqueId = '{0}'", Convert.ToString(Session("AnnuityJointSurvivorsID"))))
                End If
            End If
            If Not drTempTable Is Nothing AndAlso drTempTable.Length > 0 AndAlso (drTempTable.CopyToDataTable().Columns.Contains(strColumnName)) Then
                strColumnValue = (drTempTable(0)(strColumnName)).ToString()
            End If
            Return strColumnValue
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Previous function commented and replaced by new function

    Private Sub BindGrid(ByVal strGuiPersID As String, ByVal enumColumnname As EnumAuditLogColumn)
        Dim dsAuditLogDetails As DataSet
        Try
            dsAuditLogDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetAtsYRSAuditLogDeatilsByGuiEntityID(strGuiPersID, enumColumnname.ToString())
            gvSSNHistory.DataSource = dsAuditLogDetails
            gvSSNHistory.DataBind()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub CloseWindow()
        Dim msg As String
        Try
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
            Session("EnableEditJSBeneficiaryButton") = True 'Manthan Rajguru | 2016.08.02 | YRS-AT-2382 | Setting session variable
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function Validate() As Boolean
        Dim strMessage As String
        Dim strGuiPersid As String
        Dim bIsValid As Boolean
        Dim strCheckSSNMessage As String
        Dim strAnnuityJointSurvivorsID As String
        Dim bIsValidPhonySSN As Boolean
        Try
            bIsValid = True
            strGuiPersid = SessionPersonID 'Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Storing persId in variable
            If (ddlReason.SelectedValue = "-Select-") Then
                HelperFunctions.ShowMessageToUser("Please select Reason.", EnumMessageTypes.Error)
                bIsValid = False
                Exit Function
            End If

            If (String.IsNullOrEmpty(txtNewSSN.Text)) Then
                HelperFunctions.ShowMessageToUser("Please enter New SSN.", EnumMessageTypes.Error)
                bIsValid = False
                Exit Function
            End If
            ' // START : SB | 07/07/2016 | YRS-AT-2382 | To check wheather new SSN is same as old SSN
            If (Convert.ToString(Session("strOldSSN")) = txtNewSSN.Text.Trim) Then
                HelperFunctions.ShowMessageToUser("New SSN matches old SSN. No update required", EnumMessageTypes.Error)
                bIsValid = False
                Exit Function
            End If
            ' // END : SB | 07/07/2016 | YRS-AT-2382 | To check wheather new SSN is same as old SSN
            'Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Validating existing phony SSN
            strAnnuityJointSurvivorsID = CType(Session("AnnuityJointSurvivorsID"), String)
            bIsValidPhonySSN = Validation.IsValidPhonySSN(txtNewSSN.Text.Trim(), "INSERT", strAnnuityJointSurvivorsID, "")
            If Not bIsValidPhonySSN Then
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'HelperFunctions.ShowMessageToUser("Phony SSN already exists", EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser("Placeholder SSN already exists", EnumMessageTypes.Error)
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                bIsValid = False
                Exit Function
            End If
            'End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Validating existing phony SSN
            strMessage = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(txtNewSSN.Text.Trim())
            If strMessage.ToString().Trim() = "Not_Phony_SSNo" Then
                Session("PhonySSNo") = "Not_Phony_SSNo"
                HelperFunctions.ShowMessageToUser("Invalid SSNo entered, Please enter a Valid SSNo.", EnumMessageTypes.Error)
                bIsValid = False
                Exit Function
            ElseIf strMessage.ToString().Trim() = "No_Configuration_Key" Then
                bIsValid = False
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'HelperFunctions.ShowMessageToUser("No Key defined for Phony SSNo in AtsMetaConfiguration.", EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser("No Key defined for Placeholder SSNo in AtsMetaConfiguration.", EnumMessageTypes.Error)
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                Exit Function
            End If

            'strGuiPersid = SessionPersonID 'Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Commneted existing code
            strCheckSSNMessage = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.CheckSSNExistToOtherPerson(strGuiPersid, txtNewSSN.Text.Trim())
            If Not (String.IsNullOrEmpty(strCheckSSNMessage)) Then
                bIsValid = False
                HelperFunctions.ShowMessageToUser("SSN is already in use for participant - " + strCheckSSNMessage, EnumMessageTypes.Error)
                Exit Function
            End If
            Return bIsValid
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Previous function commented and replaced by new function with added  parameter to set the values in Audit Table for SSN  
    '    Dim dtTempTable As New DataTable()
    '    Try
    '        If Not (Session("SessionTempSSNTable") Is Nothing) Then
    '            dtTempTable = CType(Session("SessionTempSSNTable"), DataTable)
    '            dtTempTable.Rows(0)("NewSSN") = txtNewSSN.Text.Trim()
    '            dtTempTable.Rows(0)("Reason") = ddlReason.SelectedItem.Text()
    '            dtTempTable.AcceptChanges()
    '            Session("SessionTempSSNTable") = dtTempTable
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    Private Sub SetSessionForFinalUpdate(strPageName As String)
        Dim dtTempTable As New DataTable()
        Dim strBeneficiaryUniqueId As String
        Dim drChangedSSNo As DataRow()
        Try

            If strPageName = "person" Or strPageName = "retire" Then
                If Not (Session("SessionTempSSNTable") Is Nothing) Then
                    dtTempTable = CType(Session("SessionTempSSNTable"), DataTable)
                    dtTempTable.Rows(0)("NewSSN") = txtNewSSN.Text.Trim()
                    dtTempTable.Rows(0)("Reason") = ddlReason.SelectedItem.Text()
                    dtTempTable.AcceptChanges()
                    Session("SessionTempSSNTable") = dtTempTable
                End If
            ElseIf strPageName = "retireandannuitybeneficiary" Then
                If Not (Session("AuditBeneficiariesTable") Is Nothing) Then
                    dtTempTable = CType(Session("AuditBeneficiariesTable"), DataTable)

                    strBeneficiaryUniqueId = CType(Session("AnnuityJointSurvivorsID"), String)
                    drChangedSSNo = dtTempTable.Select("UniqueId='" + strBeneficiaryUniqueId + "'")

                    drChangedSSNo(0)("NewSSN") = txtNewSSN.Text.Trim()
                    drChangedSSNo(0)("Reason") = ddlReason.SelectedItem.Text()
                    drChangedSSNo(0)("IsEdited") = "True"

                    Session("AuditBeneficiariesTable") = dtTempTable
                    Session("IsAnnuityBeneficiariesSSNChanged") = True
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Previous function commented and replaced by new function with added  parameter to set the values in Audit Table for SSN  
    Private Sub btnSSNOk_Click(sender As Object, e As System.EventArgs) Handles btnSSNOk.Click
        Dim strPageName As String = Request.QueryString("Name").ToLower()    ' // SB | 07/07/2016 | YRS-AT-2382 
        Try
            If Validate() Then
                SetSessionForFinalUpdate(strPageName) ' // SB | 07/07/2016 | YRS-AT-2382 | Passing Page name 
                CloseWindow()
            End If
            'START : MMR | 05/10/2018 | YRS-AT-3941 | Handling exception error and redirecting it to error page
        Catch ex As SqlException
            Dim errorMessage As String
            If ex.Number = 60006 And ex.Procedure.Trim.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                errorMessage = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
            Else
                errorMessage = Server.UrlEncode(ex.Message.Trim.ToString())
            End If
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + errorMessage, False)
            'END : MMR | 05/10/2018 | YRS-AT-3941 | Handling exception error and redirecting it to error page
        Catch ex As Exception
            HelperFunctions.LogException("btnSSNOk_Click-UpdateSSN", ex)
            Throw ex
        End Try
    End Sub
    'Start - Manthan Rajguru | 2016.08.02 |YRS-AT-2382 | closing pop-up window on click of cancel button
    Private Sub btnSSNCancel_Click(sender As Object, e As EventArgs) Handles btnSSNCancel.Click
        CloseWindow()
    End Sub
    'End - Manthan Rajguru | 2016.08.02 |YRS-AT-2382 | closing pop-up window on click of cancel button
End Class