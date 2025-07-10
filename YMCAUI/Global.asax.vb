Imports System.Web
Imports System.Web.SessionState
Imports YMCARET.YmcaBusinessObject
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Application.Clear()
            Application("dtAllSessionId") = Nothing
            Application("appOldSessionKill") = Nothing
            Application("appNewSessionKill") = Nothing
            Application("dtAllSessionId") = UserInfo.CreateDataTable()
        Catch ex As Exception
            Response.Redirect("~/ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        Application("appCounter") = Nothing
        Dim CookieHeaders As String = HttpContext.Current.Request.Headers("Cookie")
        Try
            'START Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 
            'If Not HttpContext.Current.Session Is Nothing Then
            '    If (HttpContext.Current.Session.IsNewSession) Then
            '        If Not String.IsNullOrEmpty(CookieHeaders) Then
            '            If IsNothing(CookieHeaders) And CookieHeaders.IndexOf("ASP.NET_SessionId") >= 0 Then
            '                ' It is existing visitor, but ASP.NET session is expired
            '                Response.Redirect("Login.aspx?msg=2")
            '            Else
            '                ' It is a new visitor, session was not created before
            '            End If
            '        End If
            '    End If
            'End If



            Dim dtSessions, dtNew As DataTable
            Dim i As Integer
            Dim strDate As String
            Dim drArray As DataRow()
            If Application("dtAllSessionId") Is Nothing Then
                Application("dtAllSessionId") = UserInfo.CreateDataTable()
            Else
                strDate = System.DateTime.Now.Date.AddDays(-2).ToShortDateString()
                dtSessions = CType(Application("dtAllSessionId"), DataTable)
                'Application("dtAllSessionId") = Nothing
                drArray = dtSessions.Select("dtmLoggedOn='" & strDate & "'")
                'END Dinesh Kanojia         03/02/2013 BT: 1860:Maintaining user login history/sessions information 
                If drArray.Length > 0 Then
                    For i = 0 To drArray.Length - 1
                        drArray(i).Delete()
                    Next
                End If
                dtSessions.AcceptChanges()
            End If
        Catch ex As Exception
            Response.Redirect("~/ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Sub IsNewSessionKill()
        If Session.IsNewSession() = True Then
            Response.Redirect("~/ErrorPageForm.aspx?Message=" + +"Prevent Login", False)
            'Server.Transfer("ErrorForSession.aspx?Message=" + "Prevent Login")
        End If
    End Sub

    Sub Application_OnPostRequestHandlerExecute(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Application("appOldSessionKill") = "True" Then
                If Session.SessionID <> "" Then
                    Dim dtAllSessionId As DataTable
                    Dim strSessionId As String
                    Dim drArray As DataRow()
                    Dim i As Integer
                    If Not Application("dtAllSessionId") Is Nothing Then
                        dtAllSessionId = CType(Application("dtAllSessionId"), DataTable)
                        'Application("dtAllSessionId") = Nothing
                        drArray = dtAllSessionId.Select("KillSession='True'")
                        strSessionId = Session.SessionID.ToString()
                        For i = 0 To drArray.Length - 1
                            If strSessionId = drArray(i)(0) Then
                                drArray(i).Delete()
                                Response.Redirect("~/ErrorForSession.aspx", False)
                                'Server.Transfer("ErrorForSession.aspx", False)
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Redirect("~/ErrorForSession.aspx?Message=" + ex.Message.Trim.ToString(), False)
        End Try
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
        'Application("dtAllSessionId") = Nothing
        'Application("appOldSessionKill") = Nothing
        'Application("appNewSessionKill") = Nothing

        'Session("YRSErrorObject") = Server.GetLastError.GetBaseException()
        'Dim ex As System.Exception
        'ex = Server.GetLastError.GetBaseException()

        'Catch ex As System.Threading.ThreadAbortException

        'Catch ex As Exception

        'Code Commented by Dinesh kanojia on 15/02/2014.
        'Dim message As New StringBuilder()
        If Not Server Is Nothing Then

            Dim exc As Exception = Server.GetLastError().GetBaseException()
            If Not (exc.GetType Is GetType(HttpException)) And Not (exc.GetType Is GetType(System.Threading.ThreadAbortException)) Then

                'For Each SessionVariable As String In Session.Keys
                '    ' Find Session variable name
                '    Dim SessionVariableName As String = SessionVariable
                '    ' Find Session variable value
                '    If Not Session(SessionVariableName) Is Nothing Then
                '        Dim SessionVariableValue As String = GetStringFromObject(Session(SessionVariableName))
                '        Logger.Write(SessionVariableName + ": " + SessionVariableValue, "Exception", 0, 0, TraceEventType.Critical)

                '    End If
                'Next
                'Logger.Write(exc.Message, "Exception", 0, 0, TraceEventType.Critical)

                'Added by HR on 10/23/2012 : storing the exception object in session to display the details of error 
                'to user on the error page
                Session("ExceptionObject") = exc



                Dim errorMessage As String = "Application Exception: " + exc.Message

                ' If Not (ex.GetType Is GetType(HttpException)) And Not (ex.GetType Is GetType(System.Threading.ThreadAbortException)) Then

                If exc.InnerException IsNot Nothing Then
                    errorMessage += Environment.NewLine + "Inner Exception: " + exc.InnerException.Message
                End If

                If Context IsNot Nothing AndAlso Context.Request IsNot Nothing Then
                    errorMessage += Environment.NewLine + "Absolute Url: " + Context.Request.Url.AbsolutePath
                End If
                HelperFunctions.LogException("Application Exception Occured: ", exc)
                ExceptionPolicy.HandleException(exc, "Exception Policy")
                Server.ClearError()
                Server.Transfer("~/ErrorPageForm.aspx")
                'Server.Transfer("~/ErrorPageForm.aspx")
            End If
            'Server.ClearError()
            'Do While (Not exc Is Nothing)
            '    message.AppendFormat("{0}: {1}{2}", e.GetType().FullName, exc.Message, exc.StackTrace)
            '    message.Append(Environment.NewLine)
            '    exc = exc.InnerException
            'Loop
            'Logger.Write(message.ToString(), "Exception", 0, 0, System.Diagnostics.TraceEventType.Verbose)            
            'Response.Redirect("~/ErrorPageForm.aspx", True)

        End If


        

        '  End If

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends

        Dim dtAllSessionId As DataTable
        Dim strSessionId As String
        Dim drArray As DataRow()
        Dim i As Integer
        dtAllSessionId = CType(Application("dtAllSessionId"), DataTable)
        strSessionId = Session.SessionID.ToString()
        drArray = dtAllSessionId.Select("chvSessionId='" & strSessionId & "'")
        If drArray.Length > 0 Then
            drArray(0).Delete()
        End If
        Session.Clear()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
        Application("dtAllSessionId") = Nothing
        Application("appOldSessionKill") = Nothing
        Application("appNewSessionKill") = Nothing
    End Sub

    Private Function GetStringFromObject(ByVal p1 As Object) As String

        If TypeOf p1 Is String Then
            Return p1
        ElseIf TypeOf p1 Is Integer Then
            Return DirectCast(p1, Integer).ToString()
        ElseIf TypeOf p1 Is DataSet Then
            Return DirectCast(p1, DataSet).GetXml()
        ElseIf TypeOf p1 Is DataTable Then
            Dim dt As DataTable = DirectCast(p1, DataTable)

            If dt.DataSet Is Nothing Then
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Return ds.GetXml()
            Else
                Return dt.DataSet.GetXml()
            End If

        Else
            p1.ToString()
        End If
    End Function

End Class
