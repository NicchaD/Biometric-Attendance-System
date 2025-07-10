'/**************************************************************************************************************/
' Author: Manthan Milan Rajguru
' Created on: 12/13/2016
' Summary of Functionality: Allow to save PDF
' Declared in Version: 19.0.0 | YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380) 
'
'/**************************************************************************************************************/
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' Manthan Rajguru             | 2017.05.16   | 20.2.0          | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186)
'------------------------------------------------------------------------------------------------------
'/**************************************************************************************************************/
Imports System
Imports System.Data
Imports System.Web
Imports Microsoft.Reporting.WebForms
Imports Microsoft.ReportingServices
Imports System.Web.UI
Imports System.IO

Public Class PDFDownloader
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tempFilePath As String
        Dim fileArray As Byte()
        Try
            'START: MMR | 2017.05.16 | YRS-AT-3356 | Redirect user to login page if session is timed out
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            'END: MMR | 2017.05.16 | YRS-AT-3356 | Redirect user to login page if session is timed out

            tempFilePath = String.Empty
            If Not Session("PDFFilePathForPrint") Is Nothing Then
                tempFilePath = Server.MapPath(Session("PDFFilePathForPrint"))
            End If

            If File.Exists(tempFilePath) Then
                fileArray = File.ReadAllBytes(tempFilePath)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", (New FileInfo(Server.MapPath(Session("PDFFilePathForPrint"))).Name)))
                Response.AddHeader("content-length", fileArray.Length.ToString())
                Response.BinaryWrite(fileArray)
                'START: MMR | 2017.05.16 | YRS-AT-3356 | Added trace log which user trying to access files
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PDFDownloader", String.Format("User ID:{0} downloaded file: {1}", Session("LoggedUserKey"), tempFilePath))
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                'END: MMR | 2017.05.16 | YRS-AT-3356 | Added trace log which user trying to access files
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("PDFDownloader_Page_Load", ex)
        Finally
            tempFilePath = Nothing
            fileArray = Nothing
        End Try
    End Sub
End Class