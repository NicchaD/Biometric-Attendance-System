'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Anudeep                        07/11/2013          BT:2190-YRS 5.0-2199:YRS 5.0-2199:Create view mode for Power of Attorney display 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Vinayan C                      2018.07.19          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'Vinayan C                      2018.09.05          YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
'Shilpa N                       2019.03.08          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'Shilpa N                       2019.04.24          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) (Added End Trace)
'*******************************************************************************
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class SecurityCheck
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("SecurityCheck.aspx")
    'End issue id YRS 5.0-940
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents PlaceHolderSecurityCheck As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_String_Exception_Message As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not Me.IsPostBack Then
                Session("Page") = Nothing  'MMR | 2016.11.23 | YRS-AT-3145 | Clearing session values
                Check_Authorization()
            Else
                If Request.Form("OK") = "OK" Then
                    Response.Redirect("MainWebForm.aspx", False)
                End If
            End If
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message + "&FormType=Popup", False)
        End Try

    End Sub
    Public Sub Check_Authorization()
        Try
            Dim l_integer_AccPermission As Integer
            Dim l_String_FormName As String
            Dim l_integer_UserId As Integer
            Dim l_string_Query As String
            'Session("CurrentForm") = Request.Url.ToString()
            l_integer_UserId = Convert.ToInt32(Session("LoggedUserKey"))
            l_String_FormName = Convert.ToString(Request.QueryString("Form"))

            'START: VC | 2018.07.25 | YRS-AT-4017 | For Loan Admin pages tab id's will be passed as an querystring parameter
            Dim l_string_Tab As String = Convert.ToString(Request.QueryString("TabId"))
            'START: VC | 2018.09.05 | YRS-AT-4018 | Code to set query string value into variable
            Dim redirectedFrom As String
            redirectedFrom = Convert.ToString(Request.QueryString("From"))
            'END: VC | 2018.09.05 | YRS-AT-4018 | Code to set query string value into variable
            If l_string_Tab = "1" Then
                l_string_Tab = l_String_FormName + "?Id=1"
            ElseIf l_string_Tab = "2" Then
                l_string_Tab = "LACLoansProcessing.aspx"
            ElseIf l_string_Tab = "3" Then
                If Not String.IsNullOrEmpty(redirectedFrom) Then
                    l_string_Tab = "LACExceptions.aspx?From=" + redirectedFrom.Trim 'VC | 2018.09.05 | YRS-AT-4018 | Added code to go to LAC exception tab if it is redirected from person maintenance page
                Else
                    l_string_Tab = "LACExceptions.aspx"
                End If
            ElseIf l_string_Tab = "4" Then
                If Not String.IsNullOrEmpty(redirectedFrom) Then
                    l_string_Tab = l_String_FormName + "?Id=2&From=" + redirectedFrom 'VC | 2018.09.05 | YRS-AT-4018 | Added code to go to LAC search tab if it is redirected from person maintenance page
                Else
                    l_string_Tab = l_String_FormName + "?Id=2"
                End If
            ElseIf l_string_Tab = "5" Then
                l_string_Tab = "LACStatistics.aspx"
            ElseIf l_string_Tab = "6" Then
                l_string_Tab = "LACRate.aspx"
            End If
            'END: VC | 2018.07.25 | YRS-AT-4017 | For Loan Admin pages tab id's will be passed as an querystring parameter

            Session("FormName") = l_String_FormName
            l_integer_AccPermission = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(l_integer_UserId, l_String_FormName)
            If l_integer_AccPermission = 0 Then
                MessageBox.Show(PlaceHolderSecurityCheck, "YMCA-YRS", "You are not authorized to view this page.", MessageBoxButtons.Stop)
                'START: Shilpa N | 03/11/2019 | YRS-AT-4248 | Added new -4 code to block the access of some pages(E-category in approch doc) in read only mode
            ElseIf l_integer_AccPermission = -4 Then
                MessageBox.Show(PlaceHolderSecurityCheck, "YMCA-YRS", "You are not authorized to view this page.", MessageBoxButtons.Stop)
                'END: Shilpa N | 03/11/2019 | YRS-AT-4248 | Added new -4 code to block the access of some pages(E-category in approch doc) in read only mode
            ElseIf l_integer_AccPermission = 1 Then
                'START: VC | 2018.07.25 | YRS-AT-4017 | For Loan Admin pages navigation will happen based on tab url
                If Not String.IsNullOrEmpty(l_string_Tab) Then
                    Response.Redirect(l_string_Tab, False)
                Else
                    Response.Redirect(l_String_FormName, False)
                End If
                'Response.Redirect(l_String_FormName, False)
                'END: VC | 2018.07.25 | YRS-AT-4017 | For Loan Admin pages navigation will happen based on tab url
                Session("l_integer_AccPermission") = l_integer_AccPermission
            ElseIf l_integer_AccPermission = 2 Then
                'property also can be used
                'START: VC | 2018.07.25 | YRS-AT-4017 | For Loan Admin pages navigation will happen based on tab url
                If Not String.IsNullOrEmpty(l_string_Tab) Then
                    Response.Redirect(l_string_Tab, False)
                Else
                    Response.Redirect(l_String_FormName, False)
                End If
                'Response.Redirect(l_String_FormName, False)
                'END: VC | 2018.07.25 | YRS-AT-4017 | For Loan Admin pages navigation will happen based on tab url
                Session("l_integer_AccPermission") = l_integer_AccPermission
            ElseIf l_integer_AccPermission = -3 Then
                MessageBox.Show(PlaceHolderSecurityCheck, "YMCA-YRS", "There is no mapping for this item to the logged in User.", MessageBoxButtons.Stop)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Added optional parameter to give access to read-only mode
    Public Shared Function Check_Authorization(ByVal paramStrFormName As String, ByVal paramIntegerUserId As Integer, Optional ByVal bitAllowReadOnly As Boolean = False) As String
        Try
            '0 Then     None
            '1 Then     Read Only
            '2 Then     Full Access
            '-3 Then    no mapping 
            Dim l_integer_AccPermission As Integer

            l_integer_AccPermission = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(paramIntegerUserId, paramStrFormName)
            'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199:Added if condition to check whether readonly is allowed or not 
            'If Not allowed then it will be shown a stop message else it will returns as 'Read-only' string
            If bitAllowReadOnly = False Then
                If l_integer_AccPermission.Equals(0) Or l_integer_AccPermission.Equals(1) Then
                    Return "Sorry, You are not authorized to do this activity."
                Else
                    Return "True"
                End If
            Else
                If l_integer_AccPermission.Equals(0) Then
                    Return "Sorry, You are not authorized to do this activity."
                ElseIf l_integer_AccPermission.Equals(1) Then
                    Return "Read-Only"
                ElseIf l_integer_AccPermission.Equals(2) Then
                    Return "True"
                Else
                    Return "Sorry, You are not properly mapped to do this activity."
                End If
            End If

            'If Not l_integer_AccPermission.Equals(2) And l_integer_AccPermission.Equals(3) Then
            '    Return "Sorry, You are not authorized to do this activity."
            'Else
            '    Return "True"
            'End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Check the value of ReadOnly key  
    ''' <summary>
    ''' This function will return true only if value of 'APPLICATION_READ-ONLY_MODE' is true or 1. Default value will be false
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsApplicationInReadOnlyMode() As Boolean
        Try
            If HttpContext.Current.Session("LoggedUserGroup") = "Super Admin" Then
                Return False
            Else
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", "IsApplicationInReadOnlyMode START")
                Dim ReadOnlyFlag As String
                ReadOnlyFlag = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("APPLICATION_READ-ONLY_MODE").ToString().ToLower().Trim()
                If ReadOnlyFlag = "true" Or ReadOnlyFlag = "1" Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", String.Format("Value of the key APPLICATION_READ-ONLY_MODE : {0}", ReadOnlyFlag))
                    Return True
                Else
                    If Not ReadOnlyFlag = "false" Or Not ReadOnlyFlag = "0" Then
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", String.Format("Found Invalid Value for the key APPLICATION_SAFE_MODE {0}", ReadOnlyFlag))
                    End If
                    Return False
                End If
            End If
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", ex.ToString())
            Return False
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", "IsApplicationInReadOnlyMode END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace() 'Shilpa N | 04/24/2019 | YRS-AT-4248 | Added end trace to end the log tracing.
        End Try
    End Function
    'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Check the value of ReadOnly key  

    'START: Shilpa N | 03/13/2019 | YRS-AT-4248 | Check the value of ReadOnly Header key
    Public Shared Function GetApplicationReadonlyHeaderValue() As Boolean
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", "GetApplicationReadonlyHeaderValue START")
            Dim ReadOnlyFlag As String
            ReadOnlyFlag = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("APPLICATION_READ-ONLY_MODE").ToString().ToLower().Trim()
            If ReadOnlyFlag = "true" Or ReadOnlyFlag = "1" Then
                Return True
            Else
                If Not ReadOnlyFlag = "false" Or Not ReadOnlyFlag = "0" Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", String.Format("Found Invalid Value for the key APPLICATION_SAFE_MODE {0}", ReadOnlyFlag))
                End If
                Return False
            End If
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", ex.ToString())
            Return False
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("SecuritCheck", "GetApplicationReadonlyHeaderValue END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace() 'Shilpa N | 04/24/2019 | YRS-AT-4248 | Added end trace to end the log tracing.
        End Try
    End Function

    'END: Shilpa N | 03/13/2019 | YRS-AT-4248 | Check the value of ReadOnly key
End Class
