'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	DeathBenefitFollowupForm.aspx.vb
' Author Name		:	Anudeep  
' Contact No		:	55928738
' Creation Date		:	02/08/2013
' Description		:	This form is used to View & store Death follow status
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
' Anudeep           22.08.2013      Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
' Anudeep           28.08.2013      Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
' Anudeep           07.11.2013      BT-2265:Need to display a message on individual tabs to indicate no records available for 60 or 90 days followup.
' Anudeep           04.12.2013      BT:2311:Disabling print letters button if no records found
' Anudeep           14.08.2014      BT:2575:YRS 5.0-2381 -  Cannot check Response Received if partic has 2 fund events 
' Anudeep           02.09.2014      BT:2575:YRS 5.0-2381 -  Cannot check Response Received if partic has 2 fund events 
' Shashank          02.12.2014      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
' Manthan Rajguru   2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
' Manthan Rajguru   2016.07.13      YRS-AT-2674 -  Additional chnages to Death benifit and Annuity beneficiary death cert. follow up screens 
'*******************************************************************************
Public Class DeathBenefitFollowupForm
    Inherits System.Web.UI.Page
    Protected Property dsFollowupDetails As DataSet
        Get
            Return ViewState("dsFollowupDetails")
        End Get
        Set(ByVal value As DataSet)
            ViewState("dsFollowupDetails") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        If Not IsPostBack Then
            GetFollowupDays()
            lnkAllApllicant_Click(sender, e)
            Bindgrids()
        End If
    End Sub

    Public Sub GetFollowupDays()
        Try
            dsFollowupDetails = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetFollowupDays()
        Catch
            Throw
        End Try
    End Sub
    Private Sub lnk60Days_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk60Days.Click
        ViewState("Sort") = Nothing
        ViewState("PageIndex") = Nothing
        td60Days.Style.Add("background-color", "#93BEEE")
        td60Days.Style.Add("color", "#000000")
        tdAllApplicant.Style.Add("background-color", "#4172A9")
        tdAllApplicant.Style.Add("color", "#ffffff")
        td90Days.Style.Add("background-color", "#4172A9")
        td90Days.Style.Add("color", "#ffffff")
        lnk90Days.Visible = True
        lbl90Days.Visible = False
        lnk60Days.Visible = False
        lbl60Days.Visible = True

        lnkAllApllicant.Visible = True
        lbllnkAllApplicant.Visible = False

        gv60DaysFollowUplist.Visible = True
        gv90DaysFollowUplist.Visible = False
        gvPendingFollowUplist.Visible = False

        btnSaveResponse.Visible = False

        btnPrint.Visible = True

        Bindgrids()

        tblLengend.Visible = True 'SP 2014.12.02 BT-2310\YRS 5.0-2255 
    End Sub
    Private Sub lnk90Days_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk90Days.Click
        ViewState("Sort") = Nothing
        ViewState("PageIndex") = Nothing
        td90Days.Style.Add("background-color", "#93BEEE")
        td90Days.Style.Add("color", "#000000")
        td60Days.Style.Add("background-color", "#4172A9")
        td60Days.Style.Add("color", "#ffffff")
        tdAllApplicant.Style.Add("background-color", "#4172A9")
        tdAllApplicant.Style.Add("color", "#ffffff")
        lnk90Days.Visible = False
        lbl90Days.Visible = True
        lnk60Days.Visible = True
        lbl60Days.Visible = False

        lnkAllApllicant.Visible = True
        lbllnkAllApplicant.Visible = False

        gv60DaysFollowUplist.Visible = False
        gv90DaysFollowUplist.Visible = True
        gvPendingFollowUplist.Visible = False

        btnSaveResponse.Visible = False

        btnPrint.Visible = True
        Bindgrids()
        tblLengend.Visible = True 'SP 2014.12.02 BT-2310\YRS 5.0-2255 
    End Sub
    Private Sub lnkAllApllicant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAllApllicant.Click
        ViewState("Sort") = Nothing
        ViewState("PageIndex") = Nothing
        tdAllApplicant.Style.Add("background-color", "#93BEEE")
        tdAllApplicant.Style.Add("color", "#000000")
        td60Days.Style.Add("background-color", "#4172A9")
        td60Days.Style.Add("color", "#ffffff")
        td90Days.Style.Add("background-color", "#4172A9")
        td90Days.Style.Add("color", "#ffffff")
        lnkAllApllicant.Visible = False
        lbllnkAllApplicant.Visible = True
        lnk90Days.Visible = True
        lbl90Days.Visible = False
        lnk60Days.Visible = True
        lbl60Days.Visible = False

        gv60DaysFollowUplist.Visible = False
        gv90DaysFollowUplist.Visible = False
        gvPendingFollowUplist.Visible = True

        btnSaveResponse.Visible = True

        btnPrint.Visible = False
        Bindgrids()
        tblLengend.Visible = False 'SP 2014.12.02 BT-2310\YRS 5.0-2255 
    End Sub
    Public Sub Bindgrids()
        Dim dtPending As DataTable
        Dim dt60Days As DataTable
        Dim dt90Days As DataTable
        Dim dv As DataView
        Try
            If HelperFunctions.isNonEmpty(dsFollowupDetails) Then
                dtPending = dsFollowupDetails.Tables(0)
            End If
            dt60Days = get60Days()
            dt90Days = get90Days()
            If lbllnkAllApplicant.Visible = True Then
                If HelperFunctions.isNonEmpty(dtPending) Then
                    dv = dtPending.DefaultView
                    If Not ViewState("Sort") Is Nothing Then
                        dv.Sort() = ViewState("Sort")
                    End If
                    If Not ViewState("PageIndex") Is Nothing Then
                        Me.gvPendingFollowUplist.PageIndex = ViewState("PageIndex")
                    End If
                    ' Anudeep:07.11.2013 BT-2265-Added to hide the no records exist label
                    LabelStatusNoDataFound.Visible = False
                Else
                    ' Anudeep:07.11.2013 BT-2265-Added to show the no records exist label
                    LabelStatusNoDataFound.Text = messageFromResourceFile("MESSAGE_DTH_FOLLOWUP_NO_PENDING_RECORDS_FOUND")
                    LabelStatusNoDataFound.Visible = True
                End If

                gvPendingFollowUplist.DataSource = dv
                gvPendingFollowUplist.DataBind()
                dv = Nothing
            End If
            If lbl60Days.Visible = True Then
                If HelperFunctions.isNonEmpty(dt60Days) Then
                    dv = dt60Days.DefaultView
                    If Not ViewState("Sort") Is Nothing Then
                        dv.Sort() = ViewState("Sort")
                    End If
                    If Not ViewState("PageIndex") Is Nothing Then
                        Me.gv60DaysFollowUplist.PageIndex = ViewState("PageIndex")
                    End If
                    ' Anudeep:07.11.2013 BT-2265-Added to hide the no records exist label
                    LabelStatusNoDataFound.Visible = False
                    'AA:BT:2311-04.12.2013 - Enabling print letters button if no records found
                    btnPrint.Enabled = True
                Else
                    ' Anudeep:07.11.2013 BT-2265-Added to show the no records exist label
                    LabelStatusNoDataFound.Text = messageFromResourceFile("MESSAGE_DTH_FOLLOWUP_NO_RECORDS_FOUND_60")
                    LabelStatusNoDataFound.Visible = True
                    'AA:BT:2311-04.12.2013 - Disabling print letters button if no records found
                    btnPrint.Enabled = False
                End If
                gv60DaysFollowUplist.DataSource = dv
                gv60DaysFollowUplist.DataBind()
                dv = Nothing
            End If
            If lbl90Days.Visible = True Then
                If HelperFunctions.isNonEmpty(dt90Days) Then
                    dv = dt90Days.DefaultView
                    If Not ViewState("Sort") Is Nothing Then
                        dv.Sort() = ViewState("Sort")
                    End If
                    If Not ViewState("PageIndex") Is Nothing Then
                        Me.gv90DaysFollowUplist.PageIndex = ViewState("PageIndex")
                    End If
                    ' Anudeep:07.11.2013 BT-2265-Added to hide the no records exist label
                    LabelStatusNoDataFound.Visible = False
                    'AA:BT:2311-04.12.2013 - Enabling print letters button if no records found
                    btnPrint.Enabled = True
                Else
                    ' Anudeep:07.11.2013 BT-2265-Added to show the no records exist label
                    LabelStatusNoDataFound.Text = messageFromResourceFile("MESSAGE_DTH_FOLLOWUP_NO_RECORDS_FOUND_90")
                    LabelStatusNoDataFound.Visible = True
                    'AA:BT:2311-04.12.2013 - Disabling print letters button if no records found
                    btnPrint.Enabled = False
                End If
                gv90DaysFollowUplist.DataSource = dv
                gv90DaysFollowUplist.DataBind()
            End If
        Catch
            Throw
        Finally
            dv = Nothing
        End Try
    End Sub
    Public Function get60Days() As DataTable
        Dim dt60Days As DataTable
        Try
            dt60Days = dsFollowupDetails.Tables(0).Clone()
            For Each dr As DataRow In dsFollowupDetails.Tables(0).Rows
                'Anudeep:27.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                If IsDBNull(dr("dtm60dayFollowup")) And IsDBNull(dr("dtm90dayFollowup")) And Convert.ToDateTime(dr("dtmOriginalDocument").ToString()).AddDays(60).Date < DateTime.Today.Date And dr("bitResponseReceived") = False Then
                    dt60Days.ImportRow(dr)
                End If
            Next
            Return dt60Days
        Catch
            Throw
        End Try
    End Function

    Public Function get90Days() As DataTable
        Dim dt90Days As DataTable
        Try
            dt90Days = dsFollowupDetails.Tables(0).Clone()
            For Each dr As DataRow In dsFollowupDetails.Tables(0).Rows
                'Anudeep:27.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                If IsDBNull(dr("dtm90dayFollowup")) And Convert.ToDateTime(dr("dtmOriginalDocument").ToString()).AddDays(90).Date < DateTime.Today.Date And dr("bitResponseReceived") = False Then
                    dt90Days.ImportRow(dr)
                End If
            Next
            Return dt90Days
        Catch
            Throw
        End Try
    End Function

    Private Sub btnSaveResponse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveResponse.Click
        'Anudeep:27.08.2013 Bt-1512:YRS 5.0-1751: Added security check for resopnse save button
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnSaveResponse", Convert.ToInt32(Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
                Exit Sub
            End If
            ViewState("Flag") = "Response_Process"
            lblMessage.Text = messageFromResourceFile("MESSAGE_DTH_PROCESS_CONFIRMATION")
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True)
            'AA:10.10.2013 :YRS 5.0-1751:added below line to clear the dirty flag
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)
        Catch
            Throw
        End Try
    End Sub

    Private Function getSelectedRecords(ByVal gv As GridView, ByVal chkName As String) As List(Of Dictionary(Of String, String))
        Dim LTerminationIds As New List(Of Dictionary(Of String, String))
        Dim chk As New CheckBox
        Dim dt As DataTable
        Dim dr As DataRow()
        Try
            Dim i As Integer = 0
            For i = 0 To gv.Rows.Count - 1
                chk = gv.Rows(i).FindControl(chkName)

                If lbllnkAllApplicant.Visible = True Then
                    dt = dsFollowupDetails.Tables(0)
                    If Not IsNothing(chk) And HelperFunctions.isNonEmpty(dt) Then
                        dr = dt.Select("intDBAFId =" + gv.DataKeys(i).Value.ToString())
                        If dr.Length = 1 AndAlso dr(0)("bitResponseReceived") <> chk.Checked Then
                            LTerminationIds.Add(New Dictionary(Of String, String)() From { _
                            {"Id", gv.DataKeys(i).Value.ToString()}, {"boolvalue", chk.Checked.ToString()}})
                        End If
                    End If
                ElseIf lbl60Days.Visible = True Then
                    dt = get60Days()
                    If Not IsNothing(chk) And HelperFunctions.isNonEmpty(dt) Then
                        dr = dt.Select("intDBAFId =" + gv.DataKeys(i).Value.ToString())
                        If dr.Length = 1 And chk.Checked AndAlso IsDBNull(dr(0)("dtm60dayFollowup")) Then
                            LTerminationIds.Add(New Dictionary(Of String, String)() From { _
                            {"Id", gv.DataKeys(i).Value.ToString()}, {"value", "60Days"}})
                        End If
                    End If
                ElseIf lbl90Days.Visible = True Then
                    dt = get90Days()
                    If Not IsNothing(chk) And HelperFunctions.isNonEmpty(dt) Then
                        dr = dt.Select("intDBAFId =" + gv.DataKeys(i).Value.ToString())
                        If dr.Length = 1 And chk.Checked AndAlso IsDBNull(dr(0)("dtm90dayFollowup")) Then
                            LTerminationIds.Add(New Dictionary(Of String, String)() From { _
                            {"Id", gv.DataKeys(i).Value.ToString()}, {"value", "90Days"}})
                        End If
                    End If
                End If
            Next
            Return LTerminationIds
        Catch
            Throw
        End Try
    End Function

    Private Function messageFromResourceFile(ByVal strMessage As String) As String
        Dim strReturnMessage As String = String.Empty
        Try
            strReturnMessage = GetGlobalResourceObject("DeathMessages", strMessage)
            Return strReturnMessage
        Catch
            Throw
        End Try

    End Function

    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            If ViewState("Flag") = "Response_Process" Then
                SaveResponse()
            ElseIf ViewState("Flag") = "Response_Print" Then
                PrintLetters()
            End If
            ViewState("Flag") = ""
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
            GetFollowupDays()
            Bindgrids()
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
            Bindgrids()
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            ViewState("Flag") = ""
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
            Bindgrids()
        Catch
            Throw
        End Try
    End Sub

    Private Sub gvPendingFollowUplist_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPendingFollowUplist.PageIndexChanging
        Try
            Me.gvPendingFollowUplist.PageIndex = e.NewPageIndex
            ViewState("PageIndex") = e.NewPageIndex
            Bindgrids()
        Catch
            Throw
        End Try
    End Sub

    Private Sub gvPendingFollowUplist_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPendingFollowUplist.RowDataBound
        Dim addToolTipDiv As HtmlContainerControl
        Dim strName, strDeathStatus As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim chk As CheckBox

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                chk = DirectCast(e.Row.FindControl("chkSelect"), CheckBox)
                strName = ""
                strDeathStatus = ""
                If HelperFunctions.isNonEmpty(drv) Then
                    chk.Checked = drv("bitResponseReceived")
                    strName = "Name: " + drv("chvFirstName").ToString() + " " + drv("chvLastName").ToString()
                    strName = strName.Replace("\", "\\").Replace("'", "\'") 'AA:09.02.2014    BT:2575:YRS 5.0-2381 - Added code for not occuring javascript error if any apostrophe(') is there in name.
                    'Start:AA:08.14.2014    BT:2575:YRS 5.0-2381 - Added code if the death fund event status has multiple
                    strDeathStatus = drv("chrStatusType")
                    If strDeathStatus.Trim.Contains(",") Then
                        strDeathStatus = "Death Status (Multiple): " + strDeathStatus
                    Else
                        strDeathStatus = "Death Status: " + strDeathStatus
                    End If
                    'End:AA:08.14.2014    BT:2575:YRS 5.0-2381 - Added code if the death fund event status has multiple
                    'Start - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                    'e.Row.Cells(4).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(61).ToShortDateString
                    e.Row.Cells(5).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(61).ToShortDateString
                    'End - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid

                    If String.IsNullOrEmpty(drv("dtm60dayFollowup").ToString()) Then
                        'Start - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                        'e.Row.Cells(5).Text = "No"
                        e.Row.Cells(6).Text = "No"
                        'End - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                    End If

                    'Start - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                    'e.Row.Cells(6).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(91).ToShortDateString()
                    e.Row.Cells(7).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(91).ToShortDateString()
                    'End - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid

                    If String.IsNullOrEmpty(drv("dtm90dayFollowup").ToString()) Then
                        'Start - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                        'e.Row.Cells(7).Text = "No"
                        e.Row.Cells(8).Text = "No"
                        'End - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                    End If
                    addToolTipDiv = DirectCast(UpdatePanel1.FindControl("Tooltip"), HtmlContainerControl)
                    If strName <> "" And strDeathStatus <> "" Then
                        e.Row.Cells(1).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + strName + "','" + strDeathStatus + "');")
                        e.Row.Cells(1).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                    End If

                End If
            End If
        Catch
            Throw
        End Try
    End Sub


    Private Sub gvPendingFollowUplist_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPendingFollowUplist.Sorting
        Dim dtPendingList As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsFollowupDetails) Then
                dtPendingList = dsFollowupDetails.Tables(0)
                Sortgrid(e)
                Bindgrids()
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub gv60DaysFollowUplist_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv60DaysFollowUplist.PageIndexChanging
        Try
            Me.gvPendingFollowUplist.PageIndex = e.NewPageIndex
            ViewState("PageIndex") = e.NewPageIndex
            Bindgrids()
        Catch
            Throw
        End Try
    End Sub

    Private Sub gv60DaysFollowUplist_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv60DaysFollowUplist.RowDataBound
        Dim addToolTipDiv As HtmlContainerControl
        Dim strName, strDeathStatus As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim chkBox As CheckBox
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strName = ""
                strDeathStatus = ""


                If HelperFunctions.isNonEmpty(drv) Then
                    strName = "Name: " + drv("chvFirstName").ToString() + " " + drv("chvLastName").ToString()
                    'Start:AA:08.14.2014    BT:2575:YRS 5.0-2381 - Added code if the death fund event status has multiple
                    strDeathStatus = drv("chrStatusType")
                    If strDeathStatus.Trim.Contains(",") Then
                        strDeathStatus = "Death Status (Multiple): " + strDeathStatus
                    Else
                        strDeathStatus = "Death Status: " + strDeathStatus
                    End If
                    'End:AA:08.14.2014    BT:2575:YRS 5.0-2381 - Added code if the death fund event status has multiple

                    'SP 2014.12.02 BT-2310\YRS 5.0-2255 - Start
                    'check if representative details is missing  then disable the checkbox & applying CSS
                    If (drv("IsRepMissed").ToString() = "1" OrElse drv("IsPrcMisMatch").ToString() = "1") Then
                        chkBox = e.Row.Cells(0).FindControl("chkSelect60Days")
                        If (chkBox IsNot Nothing) Then
                            chkBox.ID = "chk60RepMissing"
                            chkBox.Checked = False
                            chkBox.Enabled = False
                        End If
                        If drv("IsRepMissed").ToString() = "1" And drv("IsPrcMisMatch").ToString() = "1" Then
                            e.Row.CssClass = "BG_ColorBothRepMissedPercentageMisMatched"
                        ElseIf drv("IsRepMissed").ToString() = "1" Then
                            e.Row.CssClass = "BG_ColorRepMissed"
                        ElseIf drv("IsPrcMisMatch").ToString() = "1" Then
                            e.Row.CssClass = "BG_ColorPercentageMisMatched"
                        End If
                    End If
                    'SP 2014.12.02 BT-2310\YRS 5.0-2255 - End

                End If

                'Anudeep:27.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                'Start - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                'e.Row.Cells(4).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(61).ToShortDateString()
                e.Row.Cells(5).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(61).ToShortDateString()
                'End - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                addToolTipDiv = DirectCast(UpdatePanel1.FindControl("Tooltip"), HtmlContainerControl)
                If strName <> "" And strDeathStatus <> "" Then
                    e.Row.Cells(1).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + strName + "','" + strDeathStatus + "');")
                    e.Row.Cells(1).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                End If

            End If

        Catch
            Throw
        End Try

    End Sub


    Private Sub gv60DaysFollowUplist_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv60DaysFollowUplist.Sorting
        Dim dt60Days As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsFollowupDetails) Then
                dt60Days = get60Days()
                Sortgrid(e)
                Bindgrids()
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub gv90DaysFollowUplist_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv90DaysFollowUplist.PageIndexChanging
        Try
            Me.gvPendingFollowUplist.PageIndex = e.NewPageIndex
            ViewState("PageIndex") = e.NewPageIndex
            Bindgrids()
        Catch
            Throw
        End Try
    End Sub

    Private Sub gv90DaysFollowUplist_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv90DaysFollowUplist.RowDataBound
        Dim addToolTipDiv As HtmlContainerControl
        Dim strName, strDeathStatus As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim chkBox As CheckBox
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strName = ""
                strDeathStatus = ""
                If HelperFunctions.isNonEmpty(drv) Then
                    strName = "Name: " + drv("chvFirstName").ToString() + " " + drv("chvLastName").ToString()
                    'Start:AA:08.14.2014    BT:2575:YRS 5.0-2381 - Added code if the death fund event status has multiple
                    strDeathStatus = drv("chrStatusType")
                    If strDeathStatus.Trim.Contains(",") Then
                        strDeathStatus = "Death Status (Multiple): " + strDeathStatus
                    Else
                        strDeathStatus = "Death Status: " + strDeathStatus
                    End If
                    'End:AA:08.14.2014    BT:2575:YRS 5.0-2381 - Added code if the death fund event status has multiple
                    'SP 2014.12.02 BT-2310\YRS 5.0-2255 - Start
                    If (drv("IsRepMissed").ToString() = "1" OrElse drv("IsPrcMisMatch").ToString() = "1") Then
                        chkBox = e.Row.Cells(0).FindControl("chkSelect90Days")
                        If (chkBox IsNot Nothing) Then
                            chkBox.ID = "chk90RepMissing"
                            chkBox.Checked = False
                            chkBox.Enabled = False
                        End If
                        If drv("IsRepMissed").ToString() = "1" And drv("IsPrcMisMatch").ToString() = "1" Then
                            e.Row.CssClass = "BG_ColorBothRepMissedPercentageMisMatched"
                        ElseIf drv("IsRepMissed").ToString() = "1" Then
                            e.Row.CssClass = "BG_ColorRepMissed"
                        ElseIf drv("IsPrcMisMatch").ToString() = "1" Then
                            e.Row.CssClass = "BG_ColorPercentageMisMatched"
                        End If
                    End If
                    'SP 2014.12.02 BT-2310\YRS 5.0-2255 - End
                End If
             

                'Anudeep:27.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                'Start - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                'e.Row.Cells(4).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(91).ToShortDateString()
                e.Row.Cells(5).Text = Convert.ToDateTime(drv("dtmOriginalDocument")).AddDays(91).ToShortDateString()
                'End - Manthan Rajguru | 2016.07.13 | YRS-AT-2674 | Commented existing code and changed row cell index as added one more column in grid
                addToolTipDiv = DirectCast(UpdatePanel1.FindControl("Tooltip"), HtmlContainerControl)
                If strName <> "" And strDeathStatus <> "" Then
                    e.Row.Cells(1).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + strName + "','" + strDeathStatus + "');")
                    e.Row.Cells(1).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                End If

            End If
        Catch
            Throw
        End Try

    End Sub

    Private Sub gv90DaysFollowUplist_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv90DaysFollowUplist.Sorting
        Dim dt90Days As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsFollowupDetails) Then
                dt90Days = get90Days()
                Sortgrid(e)
                Bindgrids()
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub Sortgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)

        Dim SortExpression As String
        Try
            SortExpression = e.SortExpression
            If Not ViewState("Sort") Is Nothing Then
                If ViewState("Sort").ToString.Trim.EndsWith(" DESC") Then
                    ViewState("Sort") = SortExpression + " ASC"
                Else
                    ViewState("Sort") = SortExpression + " DESC"
                End If
            Else
                ViewState("Sort") = SortExpression + " DESC"
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Function SaveResponse()
        Dim LTerminationIds As List(Of Dictionary(Of String, String))
        Try
            LTerminationIds = getSelectedRecords(gvPendingFollowUplist, "chkSelect")
            If LTerminationIds.Count > 0 Then
                YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveResponse(LTerminationIds)
            End If
        Catch
            Throw
        End Try
    End Function

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim LTerminationIds As List(Of Dictionary(Of String, String))
        'Anudeep:27.08.2013 Bt-1512:YRS 5.0-1751: Added security check for print button
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnPrintLetter", Convert.ToInt32(Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                lblMessage.Text = checkSecurity
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
                Exit Sub
            End If

            If lbl60Days.Visible Then
                LTerminationIds = getSelectedRecords(gv60DaysFollowUplist, "chkSelect60Days")
            ElseIf lbl90Days.Visible Then
                LTerminationIds = getSelectedRecords(gv90DaysFollowUplist, "chkSelect90Days")
            End If
            If LTerminationIds.Count = 0 Then
                lblMessage.Text = messageFromResourceFile("MESSAGE_DTH_SELECT_RECORD")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YES');", True)
                Exit Sub
            Else
                ViewState("Flag") = "Response_Print"
                lblMessage.Text = messageFromResourceFile("MESSAGE_DTH_PRINT_CONFIRMATION")
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True)
            End If

            'AA:10.10.2013 :YRS 5.0-1751:added below line to clear the dirty flag
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)
        Catch
            Throw
        End Try
    End Sub
    Public Function PrintLetters()
        Dim LTerminationIds As List(Of Dictionary(Of String, String))
        Try
            If lbl60Days.Visible Then
                LTerminationIds = getSelectedRecords(gv60DaysFollowUplist, "chkSelect60Days")
                If LTerminationIds.Count > 0 Then
                    YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.UpdateFollowupStatus(LTerminationIds)
                    CreateAndCopyDBAForm(LTerminationIds, "Death Letter for all beneficiaries 60 day followup", "DTHBEN60")
                End If

            ElseIf lbl90Days.Visible Then
                LTerminationIds = getSelectedRecords(gv90DaysFollowUplist, "chkSelect90Days")
                If LTerminationIds.Count > 0 Then
                    YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.UpdateFollowupStatus(LTerminationIds)
                    CreateAndCopyDBAForm(LTerminationIds, "Death Letter for all beneficiaries 90 day followup", "DTHBEN90")
                End If
            End If
        Catch
            Throw
        End Try
    End Function


    Public Sub CreateAndCopyDBAForm(ByVal LTerminationIds As List(Of Dictionary(Of String, String)), ByVal strReportname As String, ByVal strDoccode As String)
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String = String.Empty
        Dim l_StringPersId As String
        Dim intDBAppFormID As Integer
        Dim popupScript As String
        Try
            'Anudeep:22.08.2013-Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
            Session("strReportName_1") = "Death Benefit Application"
            popupScript = " newwindow2 = window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp', " +
                          "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');" +
                          " if (window.focus) {newwindow2.focus()}" +
                          " if (!newwindow2.closed) {newwindow2.focus()}"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open2", popupScript, True)

            Session("strReportName") = strReportname
            popupScript = " newwindow = window.open('FT\\ReportViewer.aspx', 'ReportPopUps', " +
                          "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');" +
                          " if (window.focus) {newwindow.focus()}" +
                          " if (!newwindow.closed) {newwindow.focus()}"
            'Anudeep:22.08.2013-Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
            Session("intDBAppFormID") = GetPipeSeperated(LTerminationIds)

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", popupScript, True)

            For Each item As Dictionary(Of String, String) In LTerminationIds
                intDBAppFormID = item("Id")
                If intDBAppFormID <> Nothing Then
                    l_ArrListParamValues.Add(intDBAppFormID)
                    l_stringDocType = strDoccode
                    l_StringReportName = strReportname
                    l_string_OutputFileType = "DeathBenefit_" & l_stringDocType

                    If Not dsFollowupDetails Is Nothing Then
                        l_StringPersId = dsFollowupDetails.Tables(0).Select("intDBAFId = '" + intDBAppFormID.ToString + "'")(0)("guiUniqueID").ToString()
                    End If

                    l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, l_StringPersId)

                    If Not String.IsNullOrEmpty(l_StringErrorMessage) Then
                        MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If
                    'Anudeep:22.08.2013:Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                    l_ArrListParamValues.Add(intDBAppFormID)
                    l_stringDocType = "DTHBENAP"
                    l_StringReportName = "Death Benefit Application"
                    l_string_OutputFileType = "DeathBenefit_" & l_stringDocType
                    l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, l_StringPersId)

                    If Not String.IsNullOrEmpty(l_StringErrorMessage) Then
                        MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If

                End If
            Next


        Catch
            Throw
        End Try
    End Sub
    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String, ByVal l_StringPersId As String) As String
        Dim l_StringErrorMessage As String
        Dim IDM As New IDMforAll
        Try
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

            If Not l_StringPersId Is Nothing Then
                IDM.PersId = l_StringPersId
            End If

            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList


            If Not Session("FTFileList") Is Nothing Then
                Try
                    Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                Catch
                    Throw
                End Try
            End If
            Return l_StringErrorMessage

        Catch
            Throw
        Finally
            IDM = Nothing
        End Try
    End Function
    'Start:Anudeep:22.08.2013-Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
    Private Function GetPipeSeperated(ByVal lstGuids As List(Of Dictionary(Of String, String))) As String
        Dim sbuilder As New StringBuilder()

        For Each item As Dictionary(Of String, String) In lstGuids
            sbuilder.Append("|" & item("Id"))
        Next
        If (sbuilder.Length > 1) Then
            sbuilder.Remove(0, 1)
        End If
        Return sbuilder.ToString()
    End Function
    'End:Anudeep:22.08.2013-Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch
            Throw
        End Try
    End Sub
End Class