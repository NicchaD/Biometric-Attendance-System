'*******************************************************************************
' Copyright 3i Infotech Ltd. All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	CallReport.aspx.vb
' Author Name		:	Dinesh Kanojia  
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
'
' Designed by			:	
' Designed on			:	
'
'*******************************************************************************

'**********************************************************************************************************************  
'** Modification History    
'**********************************************************************************************************************    
'** Modified By         Date(MM/DD/YYYY)    Description  
'**********************************************************************************************************************  

'*********************************************************************************************************************/ 
Imports System
Imports System.Data
Imports System.Web
Imports Microsoft.Reporting.WebForms
Imports System.Web.UI

Public Class CallReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rptDataSource As ReportDataSource
        Dim rptParameter As New ReportParameter()
        Dim strReportName As String
        Dim dtReport As New DataTable
        Try
            'Start  Dinesh Kanojia      2013.10.03          YRS 5.0-2165:RMD enhancements 
            'Print SSRS report from batch process screen
            If Not Session("ReportName") Is Nothing Then
                strReportName = Session("ReportName").ToString()
                Select Case strReportName
                    'Print RMD reports.
                    Case "RMD Report"
                        dtReport = SSRSDataset.GetSelectedMRD().Tables(0)
                        If (Not dtReport Is Nothing) Then
                            If dtReport.Rows.Count > 0 Then
                                rptDataSource = New ReportDataSource("dsSelectedRMDRecords", dtReport)
                                RptViewRMD.LocalReport.ReportPath = "RDLCReport\" + Session("ReportName").ToString.Trim + ".rdlc"
                            End If
                        End If
                        'Print Processed Disbursement Report
                    Case "Processed Disbursement Report"
                        dtReport = SSRSDataset.GetMRDDisburseRecord().Tables(0)
                        If (Not dtReport Is Nothing) Then
                            If dtReport.Rows.Count > 0 Then
                                rptDataSource = New ReportDataSource("dsProcessedRMDRecords", dtReport)
                                RptViewRMD.LocalReport.ReportPath = "RDLCReport\" + Session("ReportName").ToString.Trim + ".rdlc"
                            End If
                        End If
                End Select
                If (Not dtReport Is Nothing) Then
                    If dtReport.Rows.Count > 0 Then
                        RptViewRMD.LocalReport.DataSources.Clear()
                        RptViewRMD.LocalReport.DataSources.Add(rptDataSource)
                        RptViewRMD.LocalReport.Refresh()
                    End If
                End If
            End If

        Catch ex As Exception

        End Try
        'END  Dinesh Kanojia      2013.10.03          YRS 5.0-2165:RMD enhancements 
    End Sub

End Class