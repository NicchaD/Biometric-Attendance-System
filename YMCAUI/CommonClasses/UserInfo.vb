Imports System
Imports System.Web
Imports System.Data




' <summary>
' Summary description for UserInfo.
' </summary>
Public Class UserInfo

    Private strHostAddress As String

    Private strHostName As String
    Private strUser As String

    Public Sub New()
        MyBase.New()
        Dim obj As HttpRequest = HttpContext.Current.Request
        Me.strHostName = obj.UserHostName
        Me.strHostAddress = obj.UserHostAddress
        Me.strUser = HttpContext.Current.User.Identity.Name
    End Sub

    Public ReadOnly Property HostName() As String
        Get
            Return Me.strHostName
        End Get
    End Property

    Public ReadOnly Property User() As String
        Get
            Return Me.strUser
        End Get
    End Property
    Public ReadOnly Property HostAddress() As String
        Get
            Return Me.strHostAddress
        End Get
    End Property

    Public Shared Function CreateDataTable() As DataTable
        Dim dtSessions As DataTable = New DataTable
        Dim dc As DataColumn = New DataColumn("chvSessionId", GetType(System.String))
        dtSessions.Columns.Add(dc)
        dc = New DataColumn("intUserId", GetType(System.Int32))
        dtSessions.Columns.Add(dc)
        dc = New DataColumn("HostName", GetType(System.String))
        dtSessions.Columns.Add(dc)
        dc = New DataColumn("chvIpAddress", GetType(System.String))
        dtSessions.Columns.Add(dc)
        dc = New DataColumn("KillSession", GetType(System.Boolean))
        dtSessions.Columns.Add(dc)
        dc = New DataColumn("dtmLoggedOn", GetType(System.String))
        dtSessions.Columns.Add(dc)
        Return dtSessions
    End Function
End Class



