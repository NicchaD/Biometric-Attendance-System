<%@ Page Language="vb" AutoEventWireup="false" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="YMCAUI" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Common" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Data" %>
<%@ Import Namespace="System.Data.Common" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="CrystalDecisions.CrystalReports.Engine" %>
<%@ Import Namespace="CrystalDecisions.Shared" %>
<%@ Import Namespace="CrystalDecisions.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Process Started</title>
    <script type="text/javascript" language="javascript">
        function windowrefreshPage(i) {

            window.location.href = "ProcessCashOut.aspx?count=" + i;
        }
    </script>
    <script runat="server">
        
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim idmas As New IDMforAll
            Dim dsUnProcessIDM As New DataSet
            Dim IDM As IDMforAll
            Dim dsWithdrawalReportData As New DataSet
            Dim l_stringDocType As String = String.Empty
            Dim l_StringReportName As String = String.Empty
            Dim l_ArrListParamValues As New ArrayList
            Dim l_double_totalamtforreleaseblnk As Double = 0.0
            Dim l_string_OutputFileType As String = String.Empty
            Dim l_StringErrorMessage As String
            dsUnProcessIDM = Session("UnprocessData")
            If (HelperFunctions.isEmpty(dsUnProcessIDM)) Then
                Response.Write("Session not set")
                Exit Sub
            End If
            
            Try
                If Not Page.IsPostBack Then
                    If Not Session("strBatchId") Is Nothing Then
                        UpdateJunkUnprocessIDMRecords(Session("strBatchId").ToString)
                    Else
                        Response.Write("Session Expired")
                        HelperFunctions.LogMessage("Session('strBatchId') Expired")
                        Exit Sub
                    End If
                End If
                
                If Not dsUnProcessIDM Is Nothing Then
                    IDM = New IDMforAll
                    If IDM.DatatableFileList(False) Then
                        Session("FTFileList") = IDM.SetdtFileList
                    Else
                        Throw New Exception("Unable to generate Release Blanks, Could not create dependent table")
                        HelperFunctions.LogMessage("Unable to generate Release Blanks, Could not create dependent table")
                    End If
                    If dsUnProcessIDM.Tables(0).Rows.Count > 0 Then
                        Dim iCount As Integer = 0
                        If Not String.IsNullOrEmpty(Request.QueryString("count")) Then
                            iCount = Convert.ToInt32(Request.QueryString("count"))
                        End If
                        
                        Dim ProcessCount As Integer = 0
                        For ProcessCount = iCount To dsUnProcessIDM.Tables(0).Rows.Count - 1
                            If ProcessCount - iCount >= 10 Then
                                Exit For
                            End If
                            Try
                                Dim drUnprocess As DataRow
                                drUnprocess = dsUnProcessIDM.Tables(0).Rows(ProcessCount)
                                If drUnprocess("RefRequestID").ToString <> String.Empty OrElse drUnprocess("RefRequestID").ToString <> "" Then
                                    dsWithdrawalReportData = YMCARET.YmcaBusinessObject.RefundRequest.GetWithdrawalReportData(drUnprocess("RefRequestID").ToString.Trim)
                                    If Not dsWithdrawalReportData.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
                                        Session("PersonID") = dsWithdrawalReportData.Tables("atsRefundRequest").Rows(0)("PersID").ToString()
                                    End If

                                    l_stringDocType = "REFREQST"
                                    l_StringReportName = "Withdrawals_New.rpt"

                                    l_ArrListParamValues.Add(drUnprocess("RefRequestID").ToString().Trim())
                                    l_ArrListParamValues.Add("BIE")

                                    l_string_OutputFileType = "Withdrawal_" & l_stringDocType
                                    
                                    'HelperFunctions.CreateDSTrackingIDMProcess("SetPropertiesForIDM", Session("PersonID").ToString, Date.Now.ToString, "", "start")
                                    
                                    l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, drUnprocess("RefRequestID").ToString().Trim(), ProcessCount)

                                    'HelperFunctions.CreateDSTrackingIDMProcess("SetPropertiesForIDM", Session("PersonID").ToString, , Date.Now.ToString, "End")
                                    l_ArrListParamValues.Clear()
                                End If
                            Catch
                                Throw
                            End Try
                        Next
                        If ProcessCount < dsUnProcessIDM.Tables(0).Rows.Count Then
                            Response.Write("No. of records processed " + ProcessCount.ToString() + " out of " + Session("RowCount").ToString)
                            Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                                        "windowrefreshPage(" + ProcessCount.ToString + ")</" & "script" & ">"
                            
                            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                                Page.RegisterStartupScript("PopupScript2", popupScript3)
                            End If
                        Else
                            Response.Write("Process Completed. No. of documents processed: " + ProcessCount.ToString)
                        End If
                    End If
                End If
            Catch ex As Exception
                Response.Write(ex.StackTrace)
                Response.Write("<h1>" + ex.Message + "</h1>")
            End Try
            
        End Sub
        
        
        Private Function SetPropertiesForIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String, ByVal l_RefRequestsID As String, ByVal iCount As Integer) As String
            Dim l_StringErrorMessage As String = String.Empty

            Try

                Threading.Thread.Sleep(1000)

                ''lblMessage.Text = "No of records Process is : " + iCount
                'If (iCount < 10) Then
                Dim IDM As New IDMforAll
                IDM.PreviewReport = True
                IDM.LogonToDb = True
                IDM.CreatePDF = True
                IDM.CreateIDX = True
                IDM.CopyFilesToIDM = True
                IDM.AppType = "P"

                If Not Session("PersonID") Is Nothing Then
                    IDM.PersId = DirectCast(Session("PersonID"), String)
                End If

                If Not Session("FTFileList") Is Nothing Then
                    IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
                End If

                IDM.DocTypeCode = l_StringDocType
                IDM.OutputFileType = l_string_OutputFileType
                IDM.ReportName = l_StringReportName
                IDM.ReportParameters = l_ArrListParamValues
                IDM.RefRequestsID = l_RefRequestsID

                'HelperFunctions.CreateDSTrackingIDMProcess("ExportToPDF()", Session("PersonID").ToString, Date.Now.ToString, "", "Start")
                l_StringErrorMessage = IDM.ExportToPDF()
                'HelperFunctions.CreateDSTrackingIDMProcess("ExportToPDF()", Session("PersonID").ToString, "", Date.Now.ToString, "End")
                l_ArrListParamValues.Clear()

                Session("FTFileList") = IDM.SetdtFileList
                    
                If Not Session("FTFileList") Is Nothing Then
                    Try
                        ' HelperFunctions.CreateDSTrackingIDMProcess("Calling CopyFilestoFileServer.aspx to copy files", Session("PersonID").ToString, Date.Now.ToString, Date.Now.ToString, "Start")
                        
                        Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                        
                        'HelperFunctions.CreateDSTrackingIDMProcess("Calling CopyFilestoFileServer.aspx to copy files", Session("PersonID").ToString, Date.Now.ToString, Date.Now.ToString, "End")
                    Catch
                        Throw
                    End Try
                End If
                    
                ' End If
                Session("pageStatus") = "No. of records Process is : " + iCount.ToString()
            Catch
                Throw
            End Try
            Return l_StringErrorMessage
        End Function
        
        Private Sub UpdateJunkUnprocessIDMRecords(ByVal strBatchID As String)
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = Nothing
            Dim getCommandWrapper As DbCommand = Nothing
            Try
                db = DatabaseFactory.CreateDatabase("YRS")
                If db Is Nothing Then
                    Exit Try
                End If
                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateJunkUnprocessIDMRecords")
                getCommandWrapper.CommandTimeout = ConfigurationManager.AppSettings("LargeConnectionTimeOut").ToString()
                db.AddInParameter(getCommandWrapper, "@chvBatchId", DbType.String, strBatchID)
                db.ExecuteNonQuery(getCommandWrapper)
                If getCommandWrapper Is Nothing Then
                    Exit Try
                End If
            Catch ex As Exception
                Throw
            End Try
        End Sub
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
