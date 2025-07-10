'/**************************************************************************************************************/
' Author: Sanjay Singh Rawat
' Created on: 10/19/2016
' Summary of Functionality: Displays provided document and also provides Print and Save options
' Declared in Version: 18.3.0 | YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380) 
'
'/**************************************************************************************************************/
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' Santosh Bura		          | 2016.10.19 	 | 18.3.0          | YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380) 
' Santosh Bura                | 2016.11.21   | 19.0.0          | YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224) 
' Manthan Rajguru             | 2016.12.13   | 19.0.0          | YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380) 
' Pramod Prakash Pokale       | 2017.05.02   | 20.2.0          | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
' Manthan  Rajguru            | 2017.05.16   | 20.2.0          | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
'------------------------------------------------------------------------------------------------------
'/**************************************************************************************************************/
Imports System
Imports System.Data
Imports System.Web
Imports Microsoft.Reporting.WebForms
Imports Microsoft.ReportingServices
Imports System.Web.UI
Imports System.IO
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.Advanced
Imports PdfSharp.Pdf.IO

Public Class DocumentViewer
    Inherits System.Web.UI.Page

    Public Property PDFFilePathForPrint() As String   ' Setting the path of the PDF file for printing the Document
        Get
            If Not Session("PDFFilePathForPrint") Is Nothing Then
                Return (CType(Session("PDFFilePathForPrint"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("PDFFilePathForPrint") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim encryptLink As EncryptedQueryString 'PPP | 05/02/2017 | YRS-AT-3356 | Helps to decrypt links
        'Dim reportType As String 'PPP | 05/02/2017 | YRS-AT-3356 | We have stopped passing report type to the page
        Dim filePath As String
        Try
            'START: MMR | 2017.05.16 | YRS-AT-3356 | Redirect user to login page if session is timed out
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            'END: MMR | 2017.05.16 | YRS-AT-3356 | Redirect user to login page if session is timed out
            If Not Page.IsPostBack Then
                'START: PPP | 05/02/2017 | YRS-AT-3356 | Accessing encrypted parameters
                If Not Request.QueryString("p") Is Nothing Then
                    encryptLink = New EncryptedQueryString(Convert.ToString(Request.QueryString("p")))
                    If Not encryptLink Is Nothing AndAlso encryptLink.Count > 0 Then
                        filePath = encryptLink("link")
                        'START: MMR | 2017.05.16 | YRS-AT-3356 | Added trace log which user trying to access files
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("DocumentViewer", String.Format("User ID:{0} trying to access file: {1}", Session("LoggedUserKey"), filePath))
                        YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                        'END: MMR | 2017.05.16 | YRS-AT-3356 | Added trace log which user trying to access files
                    End If
                End If
                'END: PPP | 05/02/2017 | YRS-AT-3356 | Accessing encrypted parameters

                'START: PPP | 05/02/2017 | YRS-AT-3356 | Passing encrypted documents relative path to the page hence not required to access additional sessions
                'If Not Request.QueryString("ReportType") Is Nothing Then
                '    reportType = Request.QueryString("ReportType")
                '    Select Case reportType
                '        Case "1" ' 1 - RMD Letter
                '            filePath = Session("RMDRePrintInitialLetterFileNameWithPath").ToString()
                '        Case "2" ' 2 - Cash out Letter
                '            filePath = Session("RMDRePrintCashoutLetterFileNameWithFullPath").ToString()
                '        Case "3" ' 3 - Follow-up Letter
                '            filePath = Session("RMDRePrintFollowupLetterFileNameWithFullPath").ToString()
                '            'START :  SB | 2016.11.22 | YRS-AT-3203 |  To set the pdf file path for cashout followup letter 
                '        Case "4" ' 4 - CashOut Follow-up Letter
                '            filePath = Session("RMDRePrintCashOutFollowupLetterFileNameWithFullPath").ToString()
                '            'END :  SB | 2016.11.22 | YRS-AT-3203 |  To set the pdf file path for cashout followup letter 
                '    End Select
                'End If
                'END: PPP | 05/02/2017 | YRS-AT-3356 | Passing encrypted documents relative path to the page hence not required to access additional sessions

                If Not filePath Is Nothing AndAlso File.Exists(Server.MapPath(filePath)) Then
                    PDFFilePathForPrint = filePath
                    frmRMDPrtLtr.Attributes("src") = filePath          'SB | 2016.11.22 | YRS-AT-3203 |  To display the letter in from 
                End If
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Page_Load", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message)))
            ex = Nothing
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            frmRMDPrtLtr.Attributes("src") = "PDFViewer.aspx"
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("PDFViewer_Page_Load", ex)
        End Try
    End Sub
    'START: MMR | 12/13/2016 | YRS-AT-2685 | Commented existing code as client side function called for saving PDF
    'Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    '    Try
    '        Response.ContentType = "Application/pdf"
    '        Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", (New FileInfo(Server.MapPath(PDFFilePathForPrint)).Name)))
    '        Response.TransmitFile(Server.MapPath(PDFFilePathForPrint))
    '        Response.[End]()
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("PDFViewer_Page_Load", ex)
    '    End Try
    'End Sub
    'END: MMR | 12/13/2016 | YRS-AT-2685 | Commented existing code as client side function called for saving PDF
End Class