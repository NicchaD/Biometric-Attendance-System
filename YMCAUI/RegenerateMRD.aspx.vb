'****************************P R E P A R E D  B Y*********************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RegenerateMRD.aspx.vb
'
' Changed Hisorty
' ------------   ----------------        ---------------------------------------------------
' Date           Author                  Description
' ------------   ----------------        ---------------------------------------------------
' 14-Oct-2011    Sanjeev Gupta [54882]   BT-925: YRS 5.0-1400: Need abiltiy to regenerate RMD records for individual 
' 30-Jan-2012    Sanjeev Gupta [54882]   BT-982: Error while Regenerating RMD records.
' 22-Feb-2012    Sanjeev Gupta [54882]   BT-1000: The 'regenerate RMD' process should be limited to a termination date of no earlier than the previous year.
' 2015.09.16     Manthan Rajguru         YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
' 2019.02.28     Pooja K                 YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'*********************************************************************************************************************************

Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient

Public Class RegenerateMRD
    Inherits System.Web.UI.Page
    Dim ds_MRDRecords As DataSet
    Dim ds_RegenMRDRecords As DataSet

#Region "Page Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

            If Not Me.IsPostBack Then
                YMCA_Header_WebUserControl1.PageTitle = "Regenerate RMD Details"
                YMCA_Header_WebUserControl1.FundNo = Session("FundNo").ToString()
                PopulateYearDrp()
                LoadGeneratedMRD()

                ButtonConfirm.Attributes.Add("OnClick", "javascript:return ConfirmProcess()")

            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#End Region

#Region "Populate Year DropDownList Control"
    Private Sub PopulateYearDrp()
        Dim iYear, iStartYear As Int32
        iYear = DateTime.Now.Year

        YMCARET.YmcaBusinessObject.MRDBO.GetRegenYearByTermDate(Session("FundId").ToString(), iStartYear)

        For i = iStartYear To iYear Step 1
            DropDownListRegenMRDYear.Items.Add(i)
        Next
    End Sub
#End Region

#Region "Load RMD Generated Grid"
    Private Sub LoadGeneratedMRD()

        ds_MRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetGeneratedMRDRecords(Session("FundId").ToString())
        Cache("dataset_MRDRecords") = ds_MRDRecords

        If ds_MRDRecords.Tables(0) IsNot Nothing AndAlso ds_MRDRecords.Tables(0).DefaultView.Count > 0 Then
            GridViewGeneratedMRD.DataSource = ds_MRDRecords.Tables(0)
        Else
            GridViewGeneratedMRD.DataSource = Nothing
            GridViewGeneratedMRD.EmptyDataText = "Participant does not have any existing RMD records."
        End If
        GridViewGeneratedMRD.DataBind()
    End Sub
#End Region

#Region "Regenerate RMD Button Click Event"
    Protected Sub ButtonRegenMRD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRegenMRD.Click
        Try
            Dim stMessage As String

            If (Cache("dataset_MRDRecords") IsNot Nothing) Then
                ds_MRDRecords = DirectCast(Cache("dataset_MRDRecords"), DataSet)
            Else
                ds_MRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetGeneratedMRDRecords(Session("FundId").ToString())
                Cache("dataset_MRDRecords") = ds_MRDRecords
            End If

            Dim dt_MRD As DataTable
            Dim dr_MRD As DataRow()

            If ds_MRDRecords.Tables(0) IsNot Nothing AndAlso ds_MRDRecords.Tables(0).Rows.Count > 0 Then
                'START: Added By Sanjeev on 30-Jan-2012 for BT-982
                'dt_MRD = ds_MRDRecords.Tables(0).Select("MRDYear >= " + DropDownListRegenMRDYear.SelectedValue.ToString()).CopyToDataTable()
                dr_MRD = ds_MRDRecords.Tables(0).Select("MRDYear >= " + DropDownListRegenMRDYear.SelectedValue.ToString())
                If dr_MRD.Length > 0 Then
                    dt_MRD = dr_MRD.CopyToDataTable()
                End If
                'END:
            End If

            If dt_MRD IsNot Nothing AndAlso dt_MRD.Rows.Count > 0 Then
                GridViewGeneratedMRD.DataSource = dt_MRD
            Else
                GridViewGeneratedMRD.DataSource = Nothing
                GridViewGeneratedMRD.EmptyDataText = "No Records Found."
            End If
            GridViewGeneratedMRD.DataBind()

            ds_RegenMRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetRegeneratedRMDRecords(Convert.ToInt32(DropDownListRegenMRDYear.SelectedValue.ToString()), Session("FundID").ToString(), stMessage)
            Cache("dataset_RegenMRDRecords") = ds_RegenMRDRecords

            trRegen.Style("display") = "block"
            trRegenData.Style("display") = "block"

            If ds_RegenMRDRecords IsNot Nothing AndAlso ds_RegenMRDRecords.Tables.Count > 0 AndAlso ds_RegenMRDRecords.Tables(0).Rows.Count > 0 Then
                GridViewRegenMRD.DataSource = ds_RegenMRDRecords
                GridViewRegenMRD.DataBind()

                ButtonConfirm.Visible = True
                ButtonCancel.Visible = True
                ButtonOk.Visible = False
            Else
                GridViewRegenMRD.DataSource = Nothing
                GridViewRegenMRD.EmptyDataText = "No Records Found."

                ButtonConfirm.Visible = False
                ButtonCancel.Visible = False
                ButtonOk.Visible = True
            End If
            GridViewRegenMRD.DataBind()

            If (String.IsNullOrEmpty(stMessage)) Then
                DropDownListRegenMRDYear.Enabled = False
                lblErrorMessage.Visible = False
            Else
                'Commented By Sanjeev on 22-02-2012 for BT-1000
                'lblErrorMessage.Visible = True
                'lblErrorMessage.Text = "*" + stMessage
                'Added By Sanjeev on 22-02-2012 for BT-1000
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", GetRegenRMDMessage(stMessage), MessageBoxButtons.OK, False)
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#End Region

#Region "Row Data Bound Event for Generated RMD Grid"
    Protected Sub GridViewGeneratedMRD_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewGeneratedMRD.RowDataBound

        Dim dt As DataTable
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim sthtml As StringBuilder
            Dim lblNotes As New Label()
            lblNotes = DirectCast(e.Row.FindControl("lblShowNotes"), Label)

            If (lblNotes IsNot Nothing) Then
                e.Row.Cells(0).ToolTip = (DataBinder.Eval(e.Row.DataItem, "PlanType")).ToString()
                If DataBinder.Eval(e.Row.DataItem, "MRDYear").ToString() <> "" Then
                    sthtml = DisbursementDetails(DataBinder.Eval(e.Row.DataItem, "MRDYear").ToString(), (DataBinder.Eval(e.Row.DataItem, "PlanType")).ToString())
                    If (sthtml IsNot Nothing) Then
                        lblNotes.Attributes.Add("onclick", String.Format("showNotes(""{0}"", event)", sthtml))
                    Else
                        lblNotes.Attributes.Add("onclick", String.Format("showNotes(""{0}"", event)", "No Disbursements"))
                    End If

                Else
                    lblNotes.Text = ""
                End If
            End If
        End If
    End Sub
#End Region

#Region "Confirmed regenerated RMD records and save in to database"
    Protected Sub ButtonConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonConfirm.Click
        Try
            Dim stMessage As String

            If (Cache("dataset_RegenMRDRecords") IsNot Nothing) Then
                ds_RegenMRDRecords = DirectCast(Cache("dataset_RegenMRDRecords"), DataSet)
            Else
                ds_RegenMRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetRegeneratedRMDRecords(Convert.ToInt32(DropDownListRegenMRDYear.SelectedValue.ToString()), Convert.ToInt32(Session("FundNo").ToString()), stMessage)
            End If

            If (String.IsNullOrEmpty(stMessage)) Then
                Dim xmlRegenMRDRecords As String

                If ds_RegenMRDRecords IsNot Nothing AndAlso ds_RegenMRDRecords.Tables.Count > 0 AndAlso ds_RegenMRDRecords.Tables("RegenerateMRD").Rows.Count > 0 Then
                    xmlRegenMRDRecords = ds_RegenMRDRecords.GetXml()
                End If

                If (xmlRegenMRDRecords.Length < 10) Then
                    lblErrorMessage.Text = GetRegenRMDMessage("MESSAGE_CHECK_REGEN_RMD_RECORDS")
                    lblErrorMessage.Visible = True
                    Return
                End If

                Dim iSuccess As Integer
                iSuccess = YMCARET.YmcaBusinessObject.MRDBO.SaveRegeneratedMRD(xmlRegenMRDRecords, Session("FundId").ToString(), stMessage)

                If (iSuccess <= 0) Then
                    lblErrorMessage.Visible = True
                    lblErrorMessage.Text = "*" + stMessage
                Else
                    LoadGeneratedMRD()

                    GridViewRegenMRD.DataSource = Nothing
                    GridViewRegenMRD.DataBind()

                    lblErrorMessage.Text = GetRegenRMDMessage("MESSAGE_REGEN_RMD_INSERTED")
                    trRegen.Style("display") = "none"
                    trRegenData.Style("display") = "none"

                    lblErrorMessage.Visible = True
                    DropDownListRegenMRDYear.Enabled = True
                    ButtonConfirm.Visible = False
                    ButtonCancel.Visible = False
                    ButtonOk.Visible = True
                End If
            Else
                lblErrorMessage.Visible = True
                lblErrorMessage.Text = "*" + stMessage
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#End Region

#Region "Cancel Button Click Event"
    Protected Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCancel.Click
        LoadGeneratedMRD()

        DropDownListRegenMRDYear.SelectedIndex = 0

        trRegen.Style("display") = "none"
        trRegenData.Style("display") = "none"

        'Added By Sanjeev on 30-Jan-2012 for BT-982
        lblErrorMessage.Text = String.Empty
        'Added By Sanjeev on 30-Jan-2012 for BT-982

        DropDownListRegenMRDYear.Enabled = True
        ButtonConfirm.Visible = False
        ButtonCancel.Visible = False
        ButtonOk.Visible = True
    End Sub
#End Region

#Region "Ok Buttong Click Event"
    Protected Sub ButtonOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonOk.Click
        Response.Redirect("FindInfo.aspx?Name=RegenerateMRD", False)
    End Sub
#End Region

#Region "Load Dibursement Details"
    Private Function DisbursementDetails(ByVal iYear As Integer, ByVal stPlanType As String) As StringBuilder
        Dim strHTML As StringBuilder
        Dim ds_DisbDetails As DataSet
        ds_DisbDetails = YMCARET.YmcaBusinessObject.MRDBO.GetDisbursementDetails(Session("FundId").ToString(), iYear, stPlanType)

        If (ds_DisbDetails.Tables(0).Rows.Count > 0) Then
            strHTML = New StringBuilder()
            Dim dr As DataRow()
            strHTML.Append("<Html>")
            strHTML.Append("<table class =label;>")
            strHTML.Append("<tr><td class=labelbold>Date &nbsp;</td><td>Disbursed Amount &nbsp;</td><TD>Withhold Amount &nbsp;</td></tr>")

            For i As Integer = 0 To ds_DisbDetails.Tables(0).Rows.Count - 1
                strHTML.Append("<tr><td class=label>" & ds_DisbDetails.Tables(0).Rows(i)("DisbursementDate").ToString() & "</td>")

                If (String.IsNullOrEmpty(ds_DisbDetails.Tables(0).Rows(i)("DisbursementAmount").ToString())) Then
                    strHTML.Append("<td>&nbsp;</td>")
                Else
                    strHTML.Append("<td align='Right'>$ " & ds_DisbDetails.Tables(0).Rows(i)("DisbursementAmount").ToString() & "</td>")
                End If

                If (String.IsNullOrEmpty(ds_DisbDetails.Tables(0).Rows(i)("WithHoldAmount").ToString())) Then
                    strHTML.Append("<td>&nbsp;</td> </tr>")
                Else
                    strHTML.Append("<td align='Right'>$ " & ds_DisbDetails.Tables(0).Rows(i)("WithHoldAmount").ToString() & "</td> </tr>")
                End If
            Next

            strHTML.Append("</table>")
            strHTML.Append("</Html>")
        End If

        Return strHTML
    End Function
#End Region

#Region "Function to get Display Message from message code. Added By Sanjeev on 23-02-2012 for BT-1000"
    Private Function GetRegenRMDMessage(ByVal stMessageCode As String) As String

        Dim l_Message As String
        l_Message = String.Empty

        If stMessageCode = "MESSAGE_FIRST_YEAR_RMD" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_FIRST_YEAR_RMD
        End If

        If stMessageCode = "MESSAGE_MISSING_RMD_FACTOR" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_MISSING_RMD_FACTOR
        End If

        If stMessageCode = "MESSAGE_MULTIPLE_RMD_PAYMENTS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_MULTIPLE_RMD_PAYMENTS
        End If

        If stMessageCode = "MESSAGE_NOT_ELIGIBLE_RMD" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_NOT_ELIGIBLE_RMD
        End If

        If stMessageCode = "MESSAGE_REGEN_RMD_CURRENT_YEAR" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_REGEN_RMD_CURRENT_YEAR
        End If

        If stMessageCode = "MESSAGE_VERIFY_MRD_GRACE_CONFIGURATION" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_VERIFY_MRD_GRACE_CONFIGURATION
        End If

        If stMessageCode = "MESSAGE_CHECK_REGEN_RMD_RECORDS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CHECK_REGEN_RMD_RECORDS
        End If

        If stMessageCode = "MESSAGE_REGEN_RMD_INSERTED" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_REGEN_RMD_INSERTED
        End If

        Return l_Message

    End Function
#End Region
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonRegenMRD.Enabled = False
            ButtonRegenMRD.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 

End Class