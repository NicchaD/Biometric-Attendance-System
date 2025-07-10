'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(YYYY/MM/DD)       Issue ID                Description  
'**********************************************************************************************************************
'Manthan Rajguru        2016.05.05          YRS-AT-2909 -  Support Request: printing crystal report com exception LoanDefaultLetter.rpt
'**********************************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Public Class ReportViewer_5
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim strReportName_5 As String
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

    Dim strReportName_5 As String
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        strReportName_5 = CType(Session("strReportName_5"), String)
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

            sReportName = strReportName_5

            Select Case sReportName


                Case "ReleaseBlankLess1K", "ReleaseBlank1kto5k", "ReleaseBlankOver5k", "RL_2_FullorPartialRefund", "HardshipWithDrawalofTDForm", "RL_3_FullPIAOver15000", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_3_FullPIAOver15000", "DeferredAssociationAnnuityAgreementForm", "RL_6a_Over70PIAOver15000", "RL_6c_Over70PIAUnder5000", "RL_6b_Over70PIA5000-15000", "HWL_4_TDOnly", "HWL_5_TDAndOtherAcc"
                    populateReportsValues()


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

            sReportName = strReportName_5

            Select Case sReportName

                Case "ReleaseBlankLess1K", "ReleaseBlank1kto5k", "RL_2_FullorPartialRefund", "HardshipWithDrawalofTDForm", "RL_3_FullPIAOver15000", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_3_FullPIAOver15000", "DeferredAssociationAnnuityAgreementForm", "RL_6a_Over70PIAOver15000", "RL_6c_Over70PIAUnder5000", "RL_6b_Over70PIA5000-15000", "HWL_4_TDOnly", "HWL_5_TDAndOtherAcc"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RefRequestsID_5"), String).ToString.Trim)
                Case "ReleaseBlankOver5k"

                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RefRequestsID_5"), String).ToString.Trim)
                    ArrListParamValues.Add(CType(Session("R_minTax_5"), String).ToString.Trim)

                  

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
