'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
'   Manthan Rajguru			    | 2017.05.16    |	20.2.0	    | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************
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
Public Class PDFViewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim deviceInfo As String = String.Empty
        Dim mimeType As String = String.Empty, encoding As String = String.Empty, extension As String = String.Empty
        Dim tempFilePath As String
        Dim dictJS As PdfDictionary
        Dim dict As PdfDictionary
        Dim array As PdfArray
        Dim group As PdfDictionary
        Try
            'START: MMR | 2017.05.16 | YRS-AT-3356 | Redirect user to login page if session is timed out
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            'END: MMR | 2017.05.16 | YRS-AT-3356 | Redirect user to login page if session is timed out

            tempFilePath = String.Format("{0}\{1}.pdf", Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("MergePDFPath")), Session.SessionID)

            Using mainPdf As PdfDocument = PdfReader.Open(Server.MapPath(Session("PDFFilePathForPrint")), PdfDocumentOpenMode.Modify)
                dictJS = New PdfDictionary()
                dictJS.Elements("/S") = New PdfName("/JavaScript")
                dictJS.Elements("/JS") = New PdfStringObject(mainPdf, "print(true);")

                mainPdf.Internals.AddObject(dictJS)

                dict = New PdfDictionary()
                array = New PdfArray()
                dict.Elements("/Names") = array
                array.Elements.Add(New PdfString("EmbeddedJS"))
                array.Elements.Add(PdfInternals.GetReference(dictJS))
                mainPdf.Internals.AddObject(dict)

                group = New PdfDictionary()
                group.Elements("/JavaScript") = PdfInternals.GetReference(dict)

                mainPdf.Internals.Catalog.Elements("/Names") = group

                mainPdf.Save(tempFilePath)
            End Using

            If File.Exists(tempFilePath) Then
                Dim fileArray As Byte() = File.ReadAllBytes(tempFilePath)
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", fileArray.Length.ToString())
                Response.BinaryWrite(fileArray)

                If File.Exists(tempFilePath) Then
                    File.Delete(tempFilePath)
                End If
                'START: MMR | 2017.05.16 | YRS-AT-3356 | Added trace log which user trying to access files
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PDFViewer", String.Format("User ID:{0} printed file: {1}", Session("LoggedUserKey"), tempFilePath))
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                'END: MMR | 2017.05.16 | YRS-AT-3356 | Added trace log which user trying to access files
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("PDFViewer_Page_Load", ex)
        End Try
    End Sub
End Class