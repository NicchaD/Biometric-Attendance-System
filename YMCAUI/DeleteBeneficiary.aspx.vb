'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AddBeneficiary.aspx.vb
'*******************************************************************************

'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Anudeep            2013.04.02      YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
'DInesh.k           2013.06.10      BUG ID : 2076: Getting yellow screen while deleting beneficiary in retiree information page.
'Anudeep            2013.06.13      BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
'Anudeep            2013.06.14      BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
'Anudeep            2013.10.23      BT-2264:YRS 5.0-2232:Notes entry in person maintanance screen.
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Santosh Bura 		2016.09.21		YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233) 
'Manthan Rajguru 	2017.12.04		YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'****************************************************************************************************************************************************
Public Class DeleteBeneficiary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Dim msg As String = String.Empty
        Try
            If Not IsPostBack Then
                Session("strReason") = Nothing
            End If


            If Not Session("FundNo") Is Nothing Then
                HeaderControl.PageTitle = "Participant Information - Delete Beneficiary"
                HeaderControl.FundNo = Session("FundNo").ToString().Trim()

                If Not IsPostBack Then
                    Dim dsActiveBeneficiaries As New DataSet
                    Dim dsRetireBeneficiaries As New DataSet

                    'InitializeDeleteReasonDropDownList()
                    If Not Session("BeneficiariesRetired") Is Nothing Then
                        dsRetireBeneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)
                    End If

                    If Not Session("BeneficiariesActive") Is Nothing Then
                        dsActiveBeneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
                    End If

                    If Not Request.QueryString("UniqueID") Is Nothing Then
                        If Session("Flag") = "DeleteBeneficiaries" Then

                            'START: Code Added by DInesh.k on 10/06/2013 : BugID 2074: Improper validation while doing death notification.

                            If dsActiveBeneficiaries.Tables.Count > 0 Then
                                For iCount As Integer = 0 To dsActiveBeneficiaries.Tables(0).Rows.Count - 1
                                    If Not dsActiveBeneficiaries.Tables(0).Rows(iCount).RowState = DataRowState.Deleted Then
                                        If Not dsActiveBeneficiaries.Tables(0).Rows(iCount)("UniqueID") Is Nothing Then
                                            If dsActiveBeneficiaries.Tables(0).Rows(iCount)("UniqueID") = Request.QueryString("UniqueID") Then
                                                lblName.Text = dsActiveBeneficiaries.Tables(0).Rows(iCount)("Name") + "  " + dsActiveBeneficiaries.Tables(0).Rows(iCount)("Name2")
                                                InitializeDeleteReasonDropDownList(dsActiveBeneficiaries.Tables(0).Rows(iCount)("Rel").ToString())
                                                'Start:Anudeep:13.06.2013 BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                                                ViewState("BENE_SSN") = dsActiveBeneficiaries.Tables(0).Rows(iCount)("TAXID").ToString()
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next iCount
                            End If

                            If dsRetireBeneficiaries.Tables.Count > 0 Then
                                For iCount As Integer = 0 To dsRetireBeneficiaries.Tables(0).Rows.Count - 1
                                    If Not dsRetireBeneficiaries.Tables(0).Rows(iCount).RowState = DataRowState.Deleted Then
                                        If Not dsRetireBeneficiaries.Tables(0).Rows(iCount)("UniqueID") Is Nothing Then
                                            If dsRetireBeneficiaries.Tables(0).Rows(iCount)("UniqueID") = Request.QueryString("UniqueID") Then
                                                lblName.Text = dsRetireBeneficiaries.Tables(0).Rows(iCount)("Name") + "  " + dsRetireBeneficiaries.Tables(0).Rows(iCount)("Name2")
                                                InitializeDeleteReasonDropDownList(dsRetireBeneficiaries.Tables(0).Rows(iCount)("Rel").ToString())
                                                'Start:Anudeep:13.06.2013 BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                                                ViewState("BENE_SSN") = dsRetireBeneficiaries.Tables(0).Rows(iCount)("TAXID").ToString()
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next iCount
                            End If


                            'END: Code Added by DInesh.k on 10/06/2013 : BugID 2074: Improper validation while doing death notification.
                        End If
                    End If
                End If
            End If
            'Start:Anudeep:13.06.2013 BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
            If Not ViewState("SSN_EXISTS") Is Nothing Then
                Session("Reason") = "Yes"
                msg = msg + "<Script Language='JavaScript'>"
                'AA:2013.10.23 - Bt-2264:Changes made not to get main page refreshed and lose the changes
                'msg = msg + "window.opener.location.href = window.opener.location.href;self.close();"
                msg = msg + "window.opener.document.forms(0).submit();self.close();"
                msg = msg + "</Script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "CLOSE", msg)

                ViewState("SSN_EXISTS") = Nothing
            End If
            'End:Anudeep:13.06.2013 BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub


    Private Sub InitializeDeleteReasonDropDownList(ByRef strRelationship As String)
        Dim l_dataset_BenificiaryDeleteReasons As DataSet
        Dim l_dataset_Relationshipcode As DataSet
        l_dataset_Relationshipcode = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.SearchLookups("chvRelationshipCode")
        l_dataset_BenificiaryDeleteReasons = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.BenificiaryDeleteReasons()
        If HelperFunctions.isNonEmpty(l_dataset_BenificiaryDeleteReasons) Then
            For Each drRow As DataRow In l_dataset_Relationshipcode.Tables(0).Rows
                If Not IsDBNull(drRow("Code Value")) And Not IsDBNull(drRow("SubGroup1")) Then
                    If drRow("Code Value").ToString().Trim() = strRelationship.Trim() And drRow("SubGroup1").ToString().Trim() = "HBENE" Then
                        ddlReason.DataSource = l_dataset_BenificiaryDeleteReasons.Tables(0)
                        ddlReason.DataTextField = "Description"
                        ddlReason.DataValueField = "Code"
                        ddlReason.DataBind()
                        ddlReason.Items.Insert(0, "-- Select --")
                    ElseIf drRow("Code Value").ToString().Trim() = strRelationship.Trim() And drRow("SubGroup1").ToString().Trim() = "NHBENE" Then
                        ddlReason.DataSource = l_dataset_BenificiaryDeleteReasons.Tables(0).Select("Code = 'BEN01'").CopyToDataTable()
                        ddlReason.DataTextField = "Description"
                        ddlReason.DataValueField = "Code"
                        ddlReason.DataBind()
                        ddlReason.Items.Insert(0, "-- Select --")
                    End If
                End If
            Next


        End If
        'End :  by Dilip yadav on 2009.09.08 for YRS 5.0-852
    End Sub


    Private Function SaveBenificiaryDeleteReason() As Boolean

        Try
            Dim msg As String = String.Empty
            Dim drUpdated As DataRow
            Dim drRows As DataRow()
            Dim dsActiveBeneficiaries As New DataSet
            Dim dsRetireBeneficiaries As New DataSet
            Dim dtReason As DataTable
            Dim drReason As DataRow
            If Not Session("BeneficiariesRetired") Is Nothing Then
                dsRetireBeneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)
            End If


            If Not Session("BeneficiariesActive") Is Nothing Then
                dsActiveBeneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
            End If

            If txtDelBenfDOD.Text.Trim() <> String.Empty And ddlReason.SelectedValue = "BEN02" Then
                If HelperFunctions.isNonEmpty(dsActiveBeneficiaries) Then
                    For iCount As Integer = 0 To dsActiveBeneficiaries.Tables(0).Rows.Count - 1
                        If Not dsActiveBeneficiaries.Tables(0).Rows(iCount).RowState = DataRowState.Deleted Then
                            If dsActiveBeneficiaries.Tables(0).Rows(iCount)("UniqueID") = Request.QueryString("UniqueID") Then
                                If String.IsNullOrEmpty(dsActiveBeneficiaries.Tables(0).Rows(iCount)("Birthdate").ToString()) Then 'Anudeep:14.06.2013 Tostring added for occuring error For BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Birth date is not exists for beneficiary", MessageBoxButtons.Stop)
                                    Return False
                                Else
                                    If Convert.ToDateTime(dsActiveBeneficiaries.Tables(0).Rows(iCount)("Birthdate").ToString().Trim()) > Convert.ToDateTime(txtDelBenfDOD.Text.Trim()) Then
                                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Death date cannot be earlier than birth date", MessageBoxButtons.Stop)
                                        Return False
                                    End If
                                End If
                                'Anudeep:14.06.2013 Exit For has been placed inside If to check the next record also For BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                                Exit For
                            End If
                        End If
                    Next iCount
                End If

                'Start:Anudeep:13.06.2013 - BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                If HelperFunctions.isNonEmpty(dsRetireBeneficiaries) Then
                    For iCount As Integer = 0 To dsRetireBeneficiaries.Tables(0).Rows.Count - 1
                        If Not dsRetireBeneficiaries.Tables(0).Rows(iCount).RowState = DataRowState.Deleted Then
                            If dsRetireBeneficiaries.Tables(0).Rows(iCount)("UniqueID") = Request.QueryString("UniqueID") Then
                                If String.IsNullOrEmpty(dsRetireBeneficiaries.Tables(0).Rows(iCount)("Birthdate").ToString()) Then 'Anudeep:14.06.2013 Tostring added for occuring error For BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Birth date is not exists for beneficiary", MessageBoxButtons.Stop)
                                    Return False
                                Else
                                    If Convert.ToDateTime(dsRetireBeneficiaries.Tables(0).Rows(iCount)("Birthdate").ToString().Trim()) > Convert.ToDateTime(txtDelBenfDOD.Text.Trim()) Then
                                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Death date cannot be earlier than birth date", MessageBoxButtons.Stop)
                                        Return False
                                    End If
                                End If
                                'Anudeep:14.06.2013 Exit For has been placed inside If to check the next record also For BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                                Exit For
                            End If
                        End If
                    Next iCount
                End If
                'End:Anudeep:13.06.2013 - BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion

                'Anudeep:13.06.2013 - BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                If Not ViewState("BENE_SSN") Is Nothing Then
                    'Add Parameter
                    CheckBeneficiaryExistenceInOtherModules(ViewState("BENE_SSN"), Session("Person_Info").ToString())
                End If
            End If
            If Not Request.QueryString("UniqueID") Is Nothing Then
                If Session("Flag") = "DeleteBeneficiaries" Then
                    'Start:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                    dtReason = Session("dtReason")
                    If dtReason Is Nothing Then
                        dtReason = New DataTable
                        dtReason.Columns.Add("strBeneUniqueID")
                        dtReason.Columns.Add("strBeneficiaryName")
                        dtReason.Columns.Add("strReason")
                        dtReason.Columns.Add("strDOD")
                        dtReason.Columns.Add("strComments")
                        dtReason.Columns.Add("bImp")
                        dtReason.Columns.Add("strBeneficiarySSNo") ' SB | 2016.09.19 | Add deceased beneficary SSN in notes table.
                        dtReason.Columns.Add("ReasonCode") 'MMR | YRS-AT-3756 | Added Column to for reason code
                    End If
                    drReason = dtReason.NewRow()
                    drReason("strBeneUniqueID") = Request.QueryString("UniqueID")
                    drReason("strBeneficiaryName") = lblName.Text.Trim()
                    drReason("strReason") = ddlReason.SelectedItem.Text.Trim
                    drReason("strDOD") = txtDelBenfDOD.Text.Trim
                    drReason("strComments") = txtComments.Text.Trim
                    drReason("bImp") = chkImp.Checked
                    drReason("strBeneficiarySSNo") = ViewState("BENE_SSN").ToString.Trim ' SB | 2016.09.19 | Add deceased beneficary SSN in notes table.
                    drReason("ReasonCode") = ddlReason.SelectedValue 'MMR | YRS-AT-3756 | Added selected reason code value 
                    dtReason.Rows.Add(drReason)
                    Session("dtReason") = dtReason

                    'End:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                    Return True
                End If

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    Protected Sub btnSaveReason_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveReason.Click
        Dim msg As String = String.Empty
        Dim blnConfirm As Boolean = True
        Try
            blnConfirm = SaveBenificiaryDeleteReason()
            If blnConfirm And ViewState("SSN_EXISTS") Is Nothing Then
                Session("Reason") = "Yes"

                msg = msg + "<Script Language='JavaScript'>"
                'AA:2013.10.23 - Bt-2264:Changes made not to get main page refreshed and lose the changes
                'msg = msg + "window.opener.location.href = window.opener.location.href;self.close();"
                msg = msg + "window.opener.document.forms(0).submit();self.close();"
                msg = msg + "</Script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "CLOSE", msg)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Protected Sub btnCancelReason_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelReason.Click
        Dim msg As String = String.Empty

        Session("Reason") = "No"

        msg = msg + "<Script Language='JavaScript'>"
        'AA:2013.10.23 - Bt-2264:Changes made not to get main page refreshed and lose the changes
        'msg = msg + "window.opener.location.href = window.opener.location.href;self.close();"
        msg = msg + "window.opener.document.forms(0).submit();self.close();"
        msg = msg + "</Script>"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "CLOSE", msg)
        'Response.Write(msg)

    End Sub

    Protected Sub ddlReason_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlReason.SelectedIndexChanged
        If ddlReason.SelectedItem.Value.ToLower = "ben02" Then
            trDOD.Visible = True
            tdDOD.Visible = True
            txtDelBenfDOD.Visible = True
            calDeleteBenfDOD.Visible = True
        Else
            trDOD.Visible = False
            tdDOD.Visible = False
            txtDelBenfDOD.Visible = False
            calDeleteBenfDOD.Visible = False
        End If
    End Sub
    'Start:Anudeep:13.06.2013 - BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
    'Checks in Beneficiary ssn in Person and jointsurviour tables
    Public Function CheckBeneficiaryExistenceInOtherModules(ByRef strSSNO As String, ByVal strParticipantSSNo As String)
        Dim strMessage As String
        Try
            'Session("FundNo").ToString().Trim()
            strMessage = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.CheckBeneficiaryExistenceInOtherModules(strSSNO, strParticipantSSNo)
            If Not String.IsNullOrEmpty(strMessage) Then
                'MessageBox.Show(PlaceHolder1, "YMCA YRS", strMessage, MessageBoxButtons.Stop)
                lblMessage1.InnerHtml = strMessage
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "YMCA YRS", "openDialogDeathNotify('" + strMessage + "');", True)
                'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "')", True)
                ViewState("SSN_EXISTS") = True
            End If
        Catch
            Throw
        End Try
    End Function


    'End:Anudeep:13.06.2013 - BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "YMCA YRS", "CloseWSMessageDeathNotify();", True)
        ViewState("SSN_EXISTS") = True
        'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
    End Sub
End Class
