'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(MM/DD/YYYY)       Issue ID                Description  
'**********************************************************************************************************************
'Sanjeev Gupta(SG)      2012.01.04          BT-1511: view cashout form\letters report
'Sanjay R               2013.06.26          BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
'Anudeep                2013.08.22          Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
'Dinesh kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
'Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
'Manthan Rajguru        2016.05.05          YRS-AT-2909 -  Support Request: printing crystal report com exception LoanDefaultLetter.rpt
'Shiny C.               2020.04.10          YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693)
'**********************************************************************************************************************

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Public Class ReportViewer_1
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
    Protected WithEvents ddlReportOption As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tableReportOption As System.Web.UI.HtmlControls.HtmlTable

    Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim strReportName_1 As String
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        strReportName_1 = CType(Session("strReportName_1"), String)
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'If Session("LoggedUserKey") Is Nothing Then
        '    Response.Redirect("Login.aspx", False)
        '    Exit Sub
        'End If
        'by aparna
        Dim sReportName As String
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

            'Added By Aparna on 13th April

            sReportName = strReportName_1

            Select Case sReportName

                Case "ReleaseBlankLess1K", "ReleaseBlank1kto5k", "ReleaseBlankOver5k", "RL_2_FullorPartialRefund", "HardshipWithDrawalofTDForm", "RL_3_FullPIAOver15000", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_3_FullPIAOver15000", "DeferredAssociationAnnuityAgreementForm", "RL_6a_Over70PIAOver15000", "RL_6c_Over70PIAUnder5000", "RL_6b_Over70PIA5000-15000", "HWL_4_TDOnly", "HWL_5_TDAndOtherAcc"
                    populateReportsValues()
                Case "Loan Letter to Participant"
                    populateReportsValues()
                    'Added By Ashutosh Patil as on 20-07-2007 for YREN - 3023
                Case "Military-PA"
                    populateReportsValues()
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan_Suspension_Ltr_to_LPA-COVID-19"
                    populateReportsValues()
                    ' End | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan Satisfied-PA"
                    populateReportsValues()
                    'Aparna Samala 18/12/2006 -Cashouts
                Case "Cash Out"
                    'Added By SG: 2012.01.04: BT-1511
                    tableReportOption.Visible = True
                    populateReportsValues()

                    'Added by Ashish 18-Mar-2009 fro Issue YRS5.0 679
                Case "Loan Reamortization Letters"
                    populateReportsValues()
                    'SR:2012.12.20 - YRS 5.0-1707- New death benefit application report
                    'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
                Case "Death Benefit Application", "Death Letter for all beneficiaries", "Death Letter for all beneficiaries 60 day followup"
                    populateReportsValues()
                Case "List of Uncashed Checks and Payments Issued After the Date of Death"
                    populateReportsValues()
                    'Start:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                Case "Withdrawals_PortableProject"
                    tableReportOption.Visible = True
                    populateReportsValues()
                    'End:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
            End Select
            'Added By Aparna on 13th April


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

            sReportName = strReportName_1

            Select Case sReportName
                Case "DeathBenefitOptions"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("PersID"), String).ToString.Trim)

                Case "ReleaseBlankLess1K", "ReleaseBlank1kto5k", "RL_2_FullorPartialRefund", "HardshipWithDrawalofTDForm", "RL_3_FullPIAOver15000", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_3_FullPIAOver15000", "DeferredAssociationAnnuityAgreementForm", "RL_6a_Over70PIAOver15000", "RL_6c_Over70PIAUnder5000", "RL_6b_Over70PIA5000-15000", "HWL_4_TDOnly", "HWL_5_TDAndOtherAcc"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)

                    'Case "ReleaseBlankOver5k"

                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    '    ArrListParamValues.Add(CType(Session("R_minTax_1"), String).ToString.Trim)

                    'Case "ReleaseBlankLess1K"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "ReleaseBlank1kto5k"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "ReleaseBlankOver5k"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "HardshipWithDrawalofTDForm"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_3_FullPIAOver15000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)

                    'Case "RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_3_FullPIAOver15000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " DeferredAssociationAnnuityAgreementForm"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case " RL_6a_Over70PIAOver15000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)
                    'Case "RL_6c_Over70PIAUnder5000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)

                    'Case "RL_6b_Over70PIA5000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID_1"), String).ToString.Trim)

                    'By Aparna done on 13th april

                    'By Aparna done on 13th april
                Case "Loan Letter to Participant"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("PersID"))
                    'by Aparna  -18/12/2006
                Case "Cash Out"
                    boollogontoDB = True
                    If Not Session("CashoutBatchId") Is Nothing Then
                        ArrListParamValues.Add(CType(Session("CashoutBatchId"), String).ToString.Trim)
                    End If

                    If Not Session("strSpecialBatchId") Is Nothing Then
                        ArrListParamValues.Add(CType(Session("strSpecialBatchId"), String).ToString.Trim)
                    End If


                    'Added By SG: 2012.01.04: BT-1511
                    ArrListParamValues.Add(ddlReportOption.SelectedValue)
                    'Added By Ashutosh Patil as on 20-07-2007 for YREN - 3023
                    'Dinesh kanojia         2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
                    'Session("CashoutBatchId") = Nothing
                Case "Military-PA"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("perseID"), String).ToString.Trim)
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan_Suspension_Ltr_to_LPA-COVID-19"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("perseID"), String).ToString.Trim)
                    ' End | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan Satisfied-PA"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("perseID"), String).ToString.Trim)
                    'Added by Ashish 17-Mar-2009 for Issue YRS 5.0 679
                Case "Loan Reamortization Letters"
                    Dim l_ArrListReamortizedPara As ArrayList
                    boollogontoDB = True
                    If Not Session("ReAmortizeRptParameterForYmca") Is Nothing Then
                        l_ArrListReamortizedPara = CType(Session("ReAmortizeRptParameterForYmca"), ArrayList)
                        If l_ArrListReamortizedPara.Count = 2 Then
                            ArrListParamValues.Add(Convert.ToInt32(l_ArrListReamortizedPara.Item(0)))
                            ArrListParamValues.Add(Convert.ToInt16(l_ArrListReamortizedPara.Item(1)))
                        End If

                    End If
                    'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
                Case "Death Benefit Application", "Death Letter for all beneficiaries", "Death Letter for all beneficiaries 60 day followup"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("intDBAppFormID"), String).ToString.Trim)
                    'BS:2012.03.03:BT-941,YRS 5.0-1432:- Add Report Name and parameter Session("FundNo"),boollogontoDB  into Case Section             
                Case "List of Uncashed Checks and Payments Issued After the Date of Death"
                    boollogontoDB = True
                    ArrListParamValues.Add(Convert.ToInt32(Session("FundNo")))
                    'Start:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                Case "Withdrawals_PortableProject"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("strBatchId"), String).ToString.Trim)
                    ArrListParamValues.Add(ddlReportOption.SelectedValue)
                    'End:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
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
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"

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
                    'Start:Anudeep:22.08.2013-Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                    'discreteValue.Value = paramItem.ToString
                    'Handle passing of multiple discrete values here
                    Dim multipleParams As String() = paramItem.Split("|")
                    For j = 0 To multipleParams.Count - 1
                        Dim discrete As New ParameterDiscreteValue()
                        discrete.Value = multipleParams(j)
                        curValues.Add(discrete)
                    Next
                    'curValues.Add(discreteValue)
                    'End:Anudeep:22.08.2013-Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
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
                'start - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            End If
            CrystalReportViewer1.DataBind()
            'CrystalReportViewer1.RefreshReport()
        Catch
            Throw
        End Try
    End Function
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

    'Added By SG: 2012.01.04: BT-1511
    Private Sub ddlReportOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReportOption.SelectedIndexChanged
        populateReportsValues()
    End Sub
End Class
