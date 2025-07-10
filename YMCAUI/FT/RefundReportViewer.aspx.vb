
'***************
'Changed by - Imran
'Changed on - 28 Jan,2010

'***************
'**********************************************************************************************************************
'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(MM/DD/YYYY)       Issue ID                Description  
'**********************************************************************************************************************
'Imran                  06/Oct/2010          YRS 5.0-1181 : Generate harship report with passing parameter Ymca id 
'Imran                  25/May/2011          BT:802-YRS 5.0-1300 : letter and form for mrd-only requests 
'Dinesh.k               30/Oct/2013          BT:2140: YRS 5.0-2164:Replace RMD Message with Letter
'Sanjay R.              2013.12.06           BT:2140/YRS 5.0-2164:Replace RMD Message with Letter
'Sanjay R.              2014.07.03           BT 2590/YRS 5.0-2385 -  Modify MRD_Withdrawal_Letter_SeventyAndHalf Report 
'Dinesh k               2015.02.15           BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.
'Manthan Rajguru        2016.05.05           YRS-AT-2909 -  Support Request: printing crystal report com exception LoanDefaultLetter.rpt 
'Sanjay GS Rawat        2017.06.30           YRS-AT-3526 - YRS REPORTING: MRD_Withdrawal_Letter.rpt inputs should be FundID and guiUniqueID
'Sanjay GS Rawat        2018.03.22           YRS-AT-3761 - YRS REPORTING: MRD_Withdrawal_Letter.rpt revert to correct letter and update with 'special tax notice' section 
'Pooja Kumkar           2020.04.27           YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688)    
'**********************************************************************************************************************

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Public Class RefundReportViewer
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim ParameterFieldsCollection As New CrystalDecisions.Shared.ParameterFields

    Protected WithEvents btnLast As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrevious As System.Web.UI.WebControls.Button
    Protected WithEvents btnNext As System.Web.UI.WebControls.Button
    Protected WithEvents btnFirst As System.Web.UI.WebControls.Button
    Protected WithEvents btnExport As System.Web.UI.WebControls.Button
    Protected WithEvents CrystalReportViewer1 As CrystalDecisions.Web.CrystalReportViewer
    Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Dim sReportName As String
    Dim sRefRequestId As String
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'If Session("LoggedUserKey") Is Nothing Then
        '    Response.Redirect("Login.aspx", False)
        '    Exit Sub
        'End If
        'by aparna

        'by aparna
        Dim bBoolReport As Boolean
        Dim LstBox As New System.Web.UI.WebControls.ListBox
        Try
            If Not Session("ManualTransacts") = "ManualTransacts" Then
                btnExport.Attributes.Add("onclick", "JavaScript:showFileDialogBox()")
            End If
            If Not Session("VRManager") = "VRManager" Then
                btnExport.Attributes.Add("onclick", "JavaScript:showFileDialogBox()")
            End If


            If Not Request.QueryString("ReportNo") Is Nothing Then

                sReportName = Request.QueryString("ReportName").ToString()


                'Dim dt As DataTable
                'Dim ds As DataSet
                'Dim dr As DataRow()
                'Dim reportno As Integer
                'reportno = Request.QueryString("ReportNo")
                'ds = CType(Session("ServiceRequestReport"), DataSet)

                'dr = ds.Tables("ListOfParameter").Select("ReportNo=" + reportno.ToString())
                'For Each l_row As DataRow In dr
                '    If Not l_row("ParaName") Is Nothing Then
                '        If UCase(l_row("ParaName")) = UCase("RefRequestID") Then
                '            sRefRequestId = l_row("Paravalue")
                '        End If
                '    End If
                'Next


            End If
            Select Case sReportName
                '//IB Added on 26/05/2011-BT:802-YRS 5.0-1300 : letter and form for mrd-only requests,Add Mrd report Name
                'Dinesh k               2015.02.15           BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.
                Case "ReleaseBlankLess1K.rpt", "ReleaseBlank1kto5k.rpt", "ReleaseBlankOver5k.rpt", "RL_2_FullorPartialRefund.rpt", "HardshipWithDrawalofTDForm.rpt", "RL_3_FullPIAOver15000.rpt", "RL_2_FullRefundQDRO.rpt", "RL_2_FullorPartialRefund.rpt", "RL_2_FullorPartialRefund.rpt", "RL_3_FullPIAOver15000.rpt", "DeferredAssociationAnnuityAgreementForm.rpt", "RL_6a_Over70PIAOver15000.rpt", "RL_6c_Over70PIAUnder5000.rpt", "RL_6b_Over70PIA5000-15000.rpt", "HWL_4_TDOnly.rpt", "HWL_5_TDAndOtherAcc.rpt", "MRD_Withdrawal_Letter.rpt", "MRD_Withdrawl_Form.rpt", "MRD_Withdrawal_Letter_SeventyAndHalf.rpt" 'SR:2013.12.06 - BT:2140/YRS 5.0-2164:Renamed the report 
                    populateReportsValues()
                    'IB:YRS 5.0-1181 : Generate harship report with passing  new parameter Ymca id 
                Case "birefltr.rpt"
                    populateHardshipReportsValues()

                    'START: PK | 04.27.2020 | YRS-AT-4854 | Load report for COVID 19.
                Case "ReleaseBlankLess1K_C19.rpt", "ReleaseBlank1kto5k_C19.rpt", "ReleaseBlankOver5k_C19.rpt", "RL_2_FullorPartialRefund_C19.rpt", "MRD_Withdrawal_Letter_SeventyAndHalf_C19.rpt"
                    populateReportsValues_C19()
                    'END: PK | 04.27.2020 | YRS-AT-4854 | Load report for COVID 19.
            End Select

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Function populateReportsValues()
        Dim ArrListParamValues As New ArrayList
        Dim sReportName As String
        Dim boollogontoDB As Boolean
        'Added By Aparna on 17th April
        Dim l_dataSet As DataSet
        Dim l_datatable As DataTable
        Dim l_datarow As DataRow
        Dim l_String_tmp As String
        Dim l_string_Persid As String
        Dim ListBoxSelectedItems As New System.Web.UI.WebControls.ListBox
        Dim i As Integer

        If Not Session("ListBoxSelectedItems") Is Nothing Then
            ListBoxSelectedItems = Session("ListBoxSelectedItems")
        End If

        'end code Added By Aparna on 17th April

        Try
            sReportName = Request.QueryString("ReportName").ToString()
            'sRefRequestId = Request.QueryString("RefRequestId").ToString()



            Dim dt As DataTable
            Dim ds As DataSet
            Dim dr As DataRow()
            Dim reportno As Integer
            Dim l_minTax_2 As String
            Dim l_YearText As String
            Dim strReqRequestGuid As String  'SR:2014.07.03 - BT 2590/YRS 5.0-2385 -  Replace report parameter SSNo with  ReqRequestGuid

            ' START | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt. 
            ' Dim FundIDNo As String  'SR:2017.07.03 - YRS-AT-3526 -  Defined variable for parameter FundIdNo(MRD_Withdrawal_letter.rpt)
            ' END | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.

            reportno = Request.QueryString("ReportNo")
            ds = CType(Session("ServiceRequestReport"), DataSet)

            'dr = ds.Tables("ListOfParameter").Rows(reportno - 1)
            'sRefRequestId = dr("Paravalue")

            dr = ds.Tables("ListOfParameter").Select("ReportNo=" + reportno.ToString())
            For Each l_row As DataRow In dr
                If Not l_row("ParaName") Is Nothing Then
                    If UCase(l_row("ParaName")) = UCase("RefRequestID") Then
                        sRefRequestId = l_row("Paravalue")
                    End If

                    If UCase(l_row("ParaName")) = UCase("Tax") Then
                        l_minTax_2 = l_row("Paravalue")
                    End If

                    'IB Added on 26/05/2011-BT:802-YRS 5.0-1300 : letter and form for mrd-only requests
                    If UCase(l_row("ParaName")) = UCase("GUID") Then
                        sRefRequestId = l_row("Paravalue")
                    End If

                    If UCase(l_row("ParaName")) = UCase("YearText") Then
                        l_YearText = l_row("Paravalue")
                    End If
                    'Dinesh.k               30/Oct/2013          BT:2140: YRS 5.0-2164:Replace RMD Message with Letter
                    If UCase(l_row("ParaName")) = UCase("ReqRequestGuid") Then 'SR:2014.07.03 - BT 2590/YRS 5.0-2385 -  Replace report parameter SSNo with  ReqRequestGuid
                        strReqRequestGuid = l_row("Paravalue")
                    End If

                    ' START | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.
                    '' START | SR | 2017.06.30 | YRS-AT-3526 | look for parameter as FundIdNo for MRD withdrawal letter
                    ''If UCase(l_row("ParaName")) = UCase("FundIdNo") Then
                    ''    FundIDNo = l_row("Paravalue")
                    ''End If
                    '' END | SR | 2017.06.30 | YRS-AT-3526 | look for parameter as FundIdNo for MRD withdrawal letter
                    ' END | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.
                End If




            Next

            Select Case sReportName
                'Dinesh k               2015.02.15           BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.
                Case "ReleaseBlankLess1K.rpt", "ReleaseBlank1kto5k.rpt", "RL_2_FullorPartialRefund.rpt", "HardshipWithDrawalofTDForm.rpt", "RL_3_FullPIAOver15000.rpt", "RL_2_FullRefundQDRO.rpt", "RL_2_FullorPartialRefund.rpt", "RL_2_FullorPartialRefund.rpt", "RL_3_FullPIAOver15000.rpt", "DeferredAssociationAnnuityAgreementForm.rpt", "RL_6a_Over70PIAOver15000.rpt", "RL_6c_Over70PIAUnder5000.rpt", "RL_6b_Over70PIA5000-15000.rpt", "HWL_4_TDOnly.rpt", "HWL_5_TDAndOtherAcc.rpt"
                    boollogontoDB = True
                    ArrListParamValues.Add(sRefRequestId).ToString.Trim()
                Case "ReleaseBlankOver5k.rpt"

                    boollogontoDB = True
                    ArrListParamValues.Add(sRefRequestId.ToString.Trim())
                    ArrListParamValues.Add(l_minTax_2.ToString.Trim)

                    'Case "ReleaseBlankLess1K"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "ReleaseBlank1kto5k"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "ReleaseBlankOver5k"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "HardshipWithDrawalofTDForm"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_3_FullPIAOver15000"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_2_FullorPartialRefund"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_3_FullPIAOver15000"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " DeferredAssociationAnnuityAgreementForm"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_6a_Over70PIAOver15000"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_6c_Over70PIAUnder5000"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)

                    'Case "RL_6b_Over70PIA5000"
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)



                    'By Aparna done on 13th april


                    'IB Added on 26/05/2011-BT:802-YRS 5.0-1300 : letter and form for mrd-only requests
                    'Dinesh.k               30/Oct/2013          BT:2140: YRS 5.0-2164:Replace RMD Message with Letter
                    ' START | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.
                    '' START | SR | 2017.06.30 | YRS-AT-3526 | Replace report paramter from GuirRefrquestId to FundNo for MRD_Withdrawal_Letter.rpt
                    ''Case "MRD_Withdrawal_Letter.rpt", "MRD_Withdrawl_Form.rpt"
                    'Case "MRD_Withdrawal_Letter.rpt"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(FundIDNo.ToString.Trim())
                    ''    'ArrListParamValues.Add(l_YearText.ToString.Trim)
                    'Case "MRD_Withdrawl_Form.rpt"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(sRefRequestId.ToString.Trim())
                    '    ArrListParamValues.Add(l_YearText.ToString.Trim)
                    '' END | SR | 2017.06.30 | YRS-AT-3526 | Replace report paramter from GuirRefrquestId to FundNo for MRD_Withdrawal_Letter.rpt
                Case "MRD_Withdrawal_Letter.rpt", "MRD_Withdrawl_Form.rpt"
                    boollogontoDB = True
                    ArrListParamValues.Add(sRefRequestId.ToString.Trim())
                    ArrListParamValues.Add(l_YearText.ToString.Trim)
                    ' END | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.

                    'Dinesh.k               30/Oct/2013          BT:2140: YRS 5.0-2164:Replace RMD Message with Letter
                Case "MRD_Withdrawal_Letter_SeventyAndHalf.rpt"   'SR:2013.12.06 - BT:2140/YRS 5.0-2164:Renamed the report 
                    boollogontoDB = True
                    'ArrListParamValues.Add(l_SSNo.ToString.Trim())
                    ArrListParamValues.Add(strReqRequestGuid.ToString.Trim)

                Case Else
                    Return False
            End Select

            If sReportName.Trim <> String.Empty Then
                LoadReport(ArrListParamValues, sReportName, boollogontoDB)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Function
    Function populateHardshipReportsValues()
        Dim ArrListParamValues As New ArrayList
        Dim sReportName As String
        Dim boollogontoDB As Boolean
        Dim l_datatable As DataTable
        Dim l_datarow As DataRow
        Dim l_string_Persid As String
        Dim l_string_YMCAID As String
        Dim ListBoxSelectedItems As New System.Web.UI.WebControls.ListBox
        Dim l_datarow_CurrentRow As DataRow()
        Dim reportno As Integer
        Dim l_String_StatusType As String = "A"
        If Not Session("ListBoxSelectedItems") Is Nothing Then
            ListBoxSelectedItems = Session("ListBoxSelectedItems")
        End If

        Try
            sReportName = Request.QueryString("ReportName").ToString()
            reportno = Request.QueryString("ReportNo")
            'IB:on 06/Oct/2010 YRS 5.0-1181 Get Ymca ID
            If Not Session("Member Employment") Is Nothing Then
                l_datatable = CType(Session("Member Employment"), DataTable)
                l_datarow_CurrentRow = l_datatable.Select("FundEventID = '" & Session("FundID") & " ' and  (TermDate IS NULL OR StatusType =  '" & l_String_StatusType & "')")
                'l_datarow_CurrentRow = l_datatable.Rows(reportno)
                l_string_YMCAID = l_datarow_CurrentRow(reportno)("YMCAID")
            End If

            boollogontoDB = True
            l_string_Persid = CType(Session("PersonID"), String)
            If l_string_Persid.Trim = String.Empty Then
                Throw New Exception("Error: Person Id Not Found")
            End If
            If l_string_YMCAID.Trim = String.Empty Then
                Throw New Exception("Error: YMCA Id Not Found")
            End If

            ArrListParamValues.Add(CType(l_string_Persid, String).Trim)
            'IB:on 06/Oct/2010 YRS 5.0-1181  Add new parameter in BIREFLTR.rpt
            ArrListParamValues.Add(CType(l_string_YMCAID.Trim, String).Trim)

            If sReportName.Trim <> String.Empty Then
                LoadReport(ArrListParamValues, sReportName, boollogontoDB)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Function
    Function LoadReport(ByVal ArrListParamValues As ArrayList, ByVal sReportName As String, ByVal logontodb As Boolean) As Boolean
        'Dim objRpt As New cryst  
        Dim crCon As New ConnectionInfo
        Dim CrTableLogonInfo As New TableLogOnInfo
        Dim CrTables As Tables
        Dim CrTable As Table
        Dim TableCounter As Integer
        Dim DataSource As String
        Dim DatabaseName As String
        Dim UserID As String
        Dim Password As String
        Dim strXml As String
        Dim sReportPath As String
        Dim paramItem As String
        Try
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            sReportPath = ConfigurationSettings.AppSettings("ReportPath")
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim

            objRpt.Load(sReportPath)
            'Start Code - Added by hafiz on 10Apr2006
            objRpt.Refresh()
            'End Code - Added by hafiz on 10Apr2006
            CrystalReportViewer1.ReportSource = objRpt
            If ArrListParamValues.Count > 0 Then
                Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
                Dim paramField As CrystalDecisions.Shared.ParameterField
                Dim curValues As New CrystalDecisions.Shared.ParameterValues
                Dim discreteValue As New CrystalDecisions.Shared.ParameterDiscreteValue

                Dim i As Integer
                i = 0

                For i = 0 To ArrListParamValues.Count - 1

                    curValues = New CrystalDecisions.Shared.ParameterValues
                    discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    paramItem = ArrListParamValues.Item(i)
                    paramField = paramFieldsCollection(i)
                    curValues = paramField.CurrentValues
                    discreteValue.Value = paramItem.ToString
                    curValues.Add(discreteValue)
                    'paramField.CurrentValues = curValues
                    objRpt.ParameterFields(i).CurrentValues = curValues
                Next

                'CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            End If
            'Start Code - Added by Aparna on 17th Apr 2006
            'objRpt.Refresh()
            'End Code - Added by Aparna on 17th Apr 2006

            If logontodb Then
                CrTables = objRpt.Database.Tables

                crCon.ServerName = DataSource
                crCon.DatabaseName = DatabaseName
                crCon.UserID = UserID
                crCon.Password = Password

                For Each CrTable In CrTables
                    CrTableLogonInfo = CrTable.LogOnInfo
                    CrTableLogonInfo.ConnectionInfo = crCon
                    CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                    CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                Next
                'start - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
                For iSubCount As Integer = 0 To objRpt.Subreports.Count - 1
                    CrTables = objRpt.Subreports(iSubCount).Database.Tables                  
                    For Each CrTable In CrTables
                        CrTableLogonInfo = CrTable.LogOnInfo
                        CrTableLogonInfo.ConnectionInfo = crCon
                        CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                        CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                    Next
                Next
                'End - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            End If

            CrystalReportViewer1.DataBind()
            'CrystalReportViewer1.RefreshReport()

        Catch
            Throw
        End Try
    End Function
    'START: PK | 04.27.2020 | YRS-AT-4854 | Load report for COVID 19.
    Function populateReportsValues_C19()
        Dim ArrListParamValues As New ArrayList
        Dim sReportName As String
        Dim boollogontoDB As Boolean
        'Added By Aparna on 17th April
        Dim l_dataSet As DataSet
        Dim l_datatable As DataTable
        Dim l_datarow As DataRow
        Dim l_String_tmp As String
        Dim l_string_Persid As String
        Dim ListBoxSelectedItems As New System.Web.UI.WebControls.ListBox
        Dim i As Integer

        If Not Session("ListBoxSelectedItems_C19") Is Nothing Then
            ListBoxSelectedItems = Session("ListBoxSelectedItems_C19")
        End If

        'end code Added By Aparna on 17th April

        Try
            sReportName = Request.QueryString("ReportName").ToString()
            'sRefRequestId = Request.QueryString("RefRequestId").ToString()



            Dim dt As DataTable
            Dim ds As DataSet
            Dim dr As DataRow()
            Dim reportno As Integer
            Dim l_minTax_2 As String
            Dim l_YearText As String
            Dim strReqRequestGuid As String  'SR:2014.07.03 - BT 2590/YRS 5.0-2385 -  Replace report parameter SSNo with  ReqRequestGuid

            ' START | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt. 
            ' Dim FundIDNo As String  'SR:2017.07.03 - YRS-AT-3526 -  Defined variable for parameter FundIdNo(MRD_Withdrawal_letter.rpt)
            ' END | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.

            reportno = Request.QueryString("ReportNo")
            ds = CType(Session("ServiceRequestReport_C19"), DataSet)

            'dr = ds.Tables("ListOfParameter").Rows(reportno - 1)
            'sRefRequestId = dr("Paravalue")

            dr = ds.Tables("ListOfParameter").Select("ReportNo=" + reportno.ToString())
            For Each l_row As DataRow In dr
                If Not l_row("ParaName") Is Nothing Then
                    If UCase(l_row("ParaName")) = UCase("RefRequestID") Then
                        sRefRequestId = l_row("Paravalue")
                    End If

                    If UCase(l_row("ParaName")) = UCase("Tax") Then
                        l_minTax_2 = l_row("Paravalue")
                    End If

                    'IB Added on 26/05/2011-BT:802-YRS 5.0-1300 : letter and form for mrd-only requests
                    If UCase(l_row("ParaName")) = UCase("GUID") Then
                        sRefRequestId = l_row("Paravalue")
                    End If

                    If UCase(l_row("ParaName")) = UCase("YearText") Then
                        l_YearText = l_row("Paravalue")
                    End If
                    'Dinesh.k               30/Oct/2013          BT:2140: YRS 5.0-2164:Replace RMD Message with Letter
                    If UCase(l_row("ParaName")) = UCase("ReqRequestGuid") Then 'SR:2014.07.03 - BT 2590/YRS 5.0-2385 -  Replace report parameter SSNo with  ReqRequestGuid
                        strReqRequestGuid = l_row("Paravalue")
                    End If

                    ' START | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.
                    '' START | SR | 2017.06.30 | YRS-AT-3526 | look for parameter as FundIdNo for MRD withdrawal letter
                    ''If UCase(l_row("ParaName")) = UCase("FundIdNo") Then
                    ''    FundIDNo = l_row("Paravalue")
                    ''End If
                    '' END | SR | 2017.06.30 | YRS-AT-3526 | look for parameter as FundIdNo for MRD withdrawal letter
                    ' END | SR | 2018.03.22 | YRS-AT-3761 | commented below line of code to revert parameter changes made for MRD_Withdrawal_Letter.rpt.
                End If




            Next

            Select Case sReportName

                Case "ReleaseBlankLess1K_C19.rpt", "ReleaseBlank1kto5k_C19.rpt", "RL_2_FullorPartialRefund_C19.rpt"
                    boollogontoDB = True
                    ArrListParamValues.Add(sRefRequestId).ToString.Trim()

                Case "ReleaseBlankOver5k_C19.rpt"
                    boollogontoDB = True
                    ArrListParamValues.Add(sRefRequestId.ToString.Trim())
                    ArrListParamValues.Add(l_minTax_2.ToString.Trim)

                Case "MRD_Withdrawal_Letter_SeventyAndHalf_C19.rpt"
                    boollogontoDB = True
                    ArrListParamValues.Add(strReqRequestGuid.ToString.Trim)

                Case Else
                    Return False
            End Select

            If sReportName.Trim <> String.Empty Then
                LoadReport(ArrListParamValues, sReportName, boollogontoDB)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Function
    'END: PK | 04.27.2020 | YRS-AT-4854 | Load report for COVID 19.
    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        CrystalReportViewer1.ShowNextPage()
    End Sub
    Private Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        CrystalReportViewer1.ShowPreviousPage()
    End Sub

    Private Sub btnLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLast.Click
        CrystalReportViewer1.ShowLastPage()
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'CrystalReportViewer1.ShowLastPage() 
    End Sub

    Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        CrystalReportViewer1.ShowFirstPage()
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        objRpt.Close()
        objRpt.Dispose()
        objRpt = Nothing
        CrystalReportViewer1.Dispose()
        CrystalReportViewer1 = Nothing
        GC.Collect()
    End Sub

End Class
