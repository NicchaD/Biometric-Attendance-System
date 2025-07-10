'********************************************************************************************************************** 
' Copyright 3i Infotech Ltd. All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	MRDRecords.aspx.vb
' Author Name		:	
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Designed by			:	
' Designed on			:	
'
'**********************************************************************************************************************  
'   Modification History    
'**********************************************************************************************************************
'   Modified By     Date(YYYY/MM/DD)        Description  
'   ------------    ----------------        ---------------------------------------------------------------------------
'   Sanjeev(SG)     2012.03.15              BT-1011: YRS 5.0-1553 - enhancements to RMD processing pages.
'   DInesh.k        2013.10.03              BT:2139 : YRS 5.0-2165:RMD enhancements
'*********************************************************************************************************************/ 

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class MRDRecords
    Inherits System.Web.UI.Page
    Dim ds_MRDRecords As DataSet
    Dim dsYears As DataSet
    Dim dtYear As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            'populateMRDDetails()
            If Not Me.IsPostBack Then
                populateYear()
                populateMRDDetails()
            End If

            If Request.Form("Yes") = "Yes" Then
                GenerateMRDRecords()
                populateYear()
                populateMRDDetails()
            ElseIf Request.Form("No") = "No" Then

            ElseIf Request.Form("OK") = "OK" Then

            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#Region "Private Functions"
    Private Sub populateMRDDetails()
        Try
            ds_MRDRecords = Infotech.YmcaBusinessObject.MRDBO.GetMRDRecords()
            Session("dsMRD") = ds_MRDRecords
            If isNonEmpty(ds_MRDRecords) Then
                'If ds_MRDRecords.Tables.Count > 0 And ds_MRDRecords.Tables(0).Rows.Count > 0 Then
                ds_MRDRecords.Tables(0).DefaultView.RowFilter = "MRDYear='" & ddlYear.SelectedItem.Value & "'"
                ds_MRDRecords.Tables(0).DefaultView.Sort = "FundIdNo ASC"
                dgMRD.DataSource = ds_MRDRecords.Tables(0).DefaultView
                If ds_MRDRecords.Tables(0).DefaultView.Count > 0 Then dgMRD.SelectedIndex = -1
                dgMRD.DataBind()
                dgMRD.Visible = True
                'START: SG: 2012.03.14: BT-1011
                If ds_MRDRecords.Tables(1).DefaultView.Count > 0 Then
                    lblDate.Text = ds_MRDRecords.Tables(1).Rows(0).Item("LastProcessedGroup").ToString() & " group"
                    LblMRDDate.Visible = True
                Else
                    LblMRDDate.Visible = False
                End If

                btnGenerate.Enabled = Convert.ToBoolean(ds_MRDRecords.Tables(2).Rows(0).Item("IsAllowToGenerate"))

        

                'END: SG: 2012.03.14: BT-1011
            Else
                MessageBox.Show(PlaceHolder1, "MRD", GetRMDMessage("MESSAGE_NO_RECORDS"), MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Sub populateYear()
        Try
            Dim dv As New DataView
            ds_MRDRecords = Infotech.YmcaBusinessObject.MRDBO.GetMRDRecords()
            If isNonEmpty(ds_MRDRecords) Then
                'If ds_MRDRecords.Tables.Count > 0 And ds_MRDRecords.Tables(0).Rows.Count > 0 Then
                dv = ds_MRDRecords.Tables(0).DefaultView
                ds_MRDRecords.Tables(0).DefaultView.Sort = "MRDYear DESC"
                ddlYear.DataSource = dv.ToTable(True, "MRDYear")
                ddlYear.DataTextField = "MRDYear"
                ddlYear.DataValueField = "MRDYear"
                ddlYear.DataBind()
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Try
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            Dim dg As DataGrid = DirectCast(Source, DataGrid)
            Dim ds As DataSet
            Select Case dg.ID
                Case "dgMRD"
                    ds = Session("dsMRD")
                Case Else
                    Throw New Exception("Sorting is not handling in " & dg.ID)
            End Select

            dv = ds.Tables(0).DefaultView

            If Not ViewState(dg.ID) Is Nothing Then
                If ViewState(dg.ID).ToString.Trim.EndsWith("ASC") Then
                    dv.Sort = "[" + SortExpression + "] DESC"
                Else
                    dv.Sort = "[" + SortExpression + "] ASC"
                End If
            Else
                dv.Sort = "[" + SortExpression + "] ASC"
            End If

            dg.DataSource = Nothing
            dg.DataSource = dv
            dg.DataBind()
            ViewState(dg.ID) = dv.Sort
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function
    Private Function isNonEmpty(ByRef dv As DataView) As Boolean
        If dv Is Nothing Then Return False
        If dv.Count = 0 Then Return False
        Return True
    End Function

#End Region

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlYear.SelectedIndexChanged
        Try
            populateMRDDetails()
            If dgMRD.Items.Count <= 0 Then
                Dim ErrMessage As String
                ErrMessage = GetRMDMessage("MESSAGE_NO_RECORDS_FOR_YEAR").Replace("@Year", ddlYear.SelectedItem.Text)
                MessageBox.Show(PlaceHolder1, "MRD", ErrMessage, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Session("dsMRD") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerate.Click
        MessageBox.Show(PlaceHolder1, "MRD", GetRMDMessage("MESSAGE_CONFIRM_GENERATE"), MessageBoxButtons.YesNo)
    End Sub

    Public Sub GenerateMRDRecords()
        Dim strMsg As String
        Dim dv As New DataView
        Dim dtMRDGenerate As Date
        Dim count As Integer
        Try
            ds_MRDRecords = Session("dsMRD")

            If isNonEmpty(ds_MRDRecords) Then
                'If ds_MRDRecords.Tables.Count > 0 And ds_MRDRecords.Tables(0).Rows.Count > 0 Then
                dv = ds_MRDRecords.Tables(0).DefaultView
                dv.Sort = "MRDYear DESC"
                dtMRDGenerate = Convert.ToDateTime("01/01/" & dv.Item(0).Item(5).ToString())
                dtMRDGenerate = dtMRDGenerate.AddYears(1)
            Else
                dtMRDGenerate = Convert.ToDateTime("01/01/" & Year(Now))
            End If
            strMsg = Infotech.YmcaBusinessObject.MRDBO.GenerateMRDRecords(dtMRDGenerate)
            If strMsg = "" Or strMsg = String.Empty Then
                '   DInesh.k        2013.10.03              BT:2139 : YRS 5.0-2165:RMD enhancements
                ' Line 
                'InsertPersonMetaConfiguration()
                strMsg = GetRMDMessage("MESSAGE_GENERATE_SUCCESS")
            End If
            MessageBox.Show(PlaceHolder1, "MRD Batch Process", strMsg, MessageBoxButtons.OK)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

#Region "SG: 2012.03.16: Function to get Display Message from message code."
    Private Function GetRMDMessage(ByVal stMessageCode As String) As String

        Dim l_Message As String
        l_Message = String.Empty

        If stMessageCode = "MESSAGE_NO_RECORDS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_NO_RECORDS
        End If

        If stMessageCode = "MESSAGE_NO_RECORDS_FOR_YEAR" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_NO_RECORDS_FOR_YEAR
        End If

        If stMessageCode = "MESSAGE_CONFIRM_GENERATE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CONFIRM_GENERATE
        End If

        If stMessageCode = "MESSAGE_GENERATE_SUCCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_GENERATE_SUCCESS
        End If

        Return l_Message

    End Function
#End Region

    '   DInesh.k        2013.10.03              BT:2139 : YRS 5.0-2165:RMD enhancements
    '   DInesh.k        2014.02.18              BT:2139 : YRS 5.0-2165:RMD enhancements - remove unwanted code please refer SVN for same. 

End Class