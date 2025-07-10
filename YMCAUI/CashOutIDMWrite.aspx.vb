'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	CashOutIDMWrite.aspx.vb
' Author Name		:	
' Creation Date		:	
' Description		:	This form is used to copy PDF files to IDM server
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Public Class CashOutIDMWrite
    Inherits System.Web.UI.Page

    Protected timeout As Int32
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        timeout = 10000
        If Not Page.IsPostBack Then

        End If

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Session("pageStatus") = Nothing
        BindDataGrid()
    End Sub

    Private Function GetUnProcessdata() As DataSet
        Dim dsUnProcessIDM As New DataSet
        Try
            If Not String.IsNullOrEmpty(txtBatchId.Text.Trim) Then

                Dim objCashOutBOClass As YMCARET.YmcaBusinessObject.CashOutBOClass
                objCashOutBOClass = New YMCARET.YmcaBusinessObject.CashOutBOClass
                dsUnProcessIDM = objCashOutBOClass.GetCashOutUnprocessIDM(txtBatchId.Text.Trim)
            Else
                lblMessage.Text = "Please enter batchid"
                dsUnProcessIDM = Nothing
            End If
        Catch ex As Exception
            Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Unprocess IDM"), False)
        End Try
        Return dsUnProcessIDM
    End Function
    Private Sub BindDataGrid()
        Dim drUnProcessData As New DataSet
        Try
            drUnProcessData = GetUnProcessdata()
            If Not drUnProcessData Is Nothing Then
                gvUnprocessIDM.DataSource = drUnProcessData
                gvUnprocessIDM.DataBind()
            End If
        Catch ex As Exception
            Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Unprocess IDM"), False)
        End Try
    End Sub
    Private Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim IDM As IDMforAll
        Dim dsUnProcessIDM As New DataSet
        Dim dsWithdrawalReportData As New DataSet
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_double_totalamtforreleaseblnk As Double = 0.0
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String
        Try
            Session("pageStatus") = Nothing
            dsUnProcessIDM = GetUnProcessdata()
            If Not dsUnProcessIDM Is Nothing Then
                IDM = New IDMforAll
                'create the Datatable -Filelist
                If IDM.DatatableFileList(False) Then
                    Session("FTFileList") = IDM.SetdtFileList
                Else
                    Throw New Exception("Unable to generate Release Blanks, Could not create dependent table")
                End If
                If dsUnProcessIDM.Tables(0).Rows.Count > 0 Then
                    Dim iCount As Integer = 0
                    For Each drUnprocess In dsUnProcessIDM.Tables(0).Rows
                        Try
                            If drUnprocess("RefRequestID").ToString <> String.Empty OrElse drUnprocess("RefRequestID").ToString <> "" Then
                                dsWithdrawalReportData = YMCARET.YmcaBusinessObject.RefundRequest.GetWithdrawalReportData(drUnprocess("RefRequestID").ToString().Trim())

                                If Not dsWithdrawalReportData.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
                                    Session("PersonID") = dsWithdrawalReportData.Tables("atsRefundRequest").Rows(0)("PersID").ToString()
                                End If

                                l_stringDocType = "REFREQST"
                                l_StringReportName = "Withdrawals_New.rpt"

                                l_ArrListParamValues.Add(drUnprocess("RefRequestID").ToString().Trim())
                                l_ArrListParamValues.Add("BIE")

                                l_string_OutputFileType = "Withdrawal_" & l_stringDocType
                                iCount += 1
                                l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, drUnprocess("RefRequestID").ToString().Trim(), iCount)

                                l_ArrListParamValues.Clear()
                                Session("pageStatus") = Nothing
                            End If
                        Catch ex As Exception
                            Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Unprocess IDM"), False)
                            Session("pageStatus") = Nothing
                        End Try
                    Next
                End If
            End If
        Catch ex As Exception
            Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Unprocess IDM"), False)
            Session("pageStatus") = Nothing
        End Try
    End Sub

    Private Function SetPropertiesForIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String, ByVal l_RefRequestsID As String, ByVal iCount As Integer) As String
        Dim l_StringErrorMessage As String = String.Empty

        Try

            Threading.Thread.Sleep(1000)

            ''lblMessage.Text = "No of records Process is : " + iCount
            Session("pageStatus") = "No of records Process is : " + iCount

            'Dim IDM As New IDMforAll

            'IDM.PreviewReport = True
            'IDM.LogonToDb = True
            'IDM.CreatePDF = True
            'IDM.CreateIDX = True
            'IDM.CopyFilesToIDM = True
            'IDM.AppType = "P"

            'If Not Session("PersonID") Is Nothing Then
            '    IDM.PersId = DirectCast(Session("PersonID"), String)
            'End If

            'If Not Session("FTFileList") Is Nothing Then
            '    IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            'End If

            'IDM.DocTypeCode = l_StringDocType
            'IDM.OutputFileType = l_string_OutputFileType
            'IDM.ReportName = l_StringReportName
            'IDM.ReportParameters = l_ArrListParamValues
            'IDM.RefRequestsID = l_RefRequestsID

            'l_StringErrorMessage = IDM.ExportToPDF()

            'l_ArrListParamValues.Clear()

            'Session("FTFileList") = IDM.SetdtFileList

        Catch
            Throw
        End Try
        Return l_StringErrorMessage
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function getpageStatus() As String
        Return Convert.ToString(HttpContext.Current.Session("pageStatus"))
    End Function

End Class