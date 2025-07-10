'*************************************************************************************************************************************************************************************
'Modification History
'*************************************************************************************************************************************************************************************
'Modified By			         Date		        Description
'*************************************************************************************************************************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*************************************************************************************************************************************************************************************

Public Class InterestProcessingStart
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("InterestProcessingStart.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim closeWindow4 As String

        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Session("LoggedUserKey") Is Nothing Then
                Session("bool_Error") = True
                Session("String_ProcessMessage") = "Login information not found"

                closeWindow4 = "<script language='javascript'> self.close();window.opener.document.forms(0).submit(); </script>"

                If (Not Me.IsStartupScriptRegistered("CloseWindow4")) Then
                    Page.RegisterStartupScript("CloseWindow4", closeWindow4)
                End If
            Else
                InterestPostingProcess()
                Session("bool_Error") = False
            End If

        Catch ex As Exception
            Session("bool_Error") = True
            Session("String_ProcessMessage") = ex.Message.Trim
        End Try

        closeWindow4 = "<script language='javascript'> self.close();window.opener.document.forms(0).submit(); </script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow4")) Then
            Page.RegisterStartupScript("CloseWindow4", closeWindow4)
        End If
    End Sub

#Region "Private Methods"

    Private Sub InterestPostingProcess()
        Dim l_String_Message As String
        Dim l_String_Tmp_Message As String
        Try
            Dim string_InterestPostingOption As String
            Session("bool_Error") = False
            Dim datetime_InterestPostingDate As DateTime
            string_InterestPostingOption = CType(Session("string_InterestPostingOption"), String)
            datetime_InterestPostingDate = CType(Session("dateTime_InterestDate"), DateTime)

            Dim objInterestProcessing As New YMCARET.YmcaBusinessObject.InterestProcessingBOClass
            objInterestProcessing.GetAccountDate()
            objInterestProcessing.InterestProcessingDate = datetime_InterestPostingDate

            Try
                objInterestProcessing.InterestProcess(string_InterestPostingOption)

                l_String_Tmp_Message = objInterestProcessing.String_ProcessMessage()

            Catch ex As Exception
                l_String_Tmp_Message = "Interest processing: Process not completed, Please Contact Support."


            End Try

            If l_String_Tmp_Message.Trim <> "" Then
                l_String_Message += vbCrLf + l_String_Tmp_Message
            End If
            l_String_Tmp_Message = ""
            objInterestProcessing.String_ProcessMessage = ""
            Session("String_ProcessMessage") = l_String_Message.Trim
            Exit Sub

        Catch ex As Exception
            Session("bool_Error") = True
            Session("String_ProcessMessage") = ex.Message.Trim
        End Try
    End Sub
#End Region
#Region "Private Porperty"
    'Private Property InterestPostingOption() As String
    '    Get
    '        If Not Session("string_InterestPostingOption") Is Nothing Then
    '            Return (CType(Session("string_InterestPostingOption"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("string_InterestPostingOption") = Value
    '    End Set
    'End Property

#End Region

End Class
