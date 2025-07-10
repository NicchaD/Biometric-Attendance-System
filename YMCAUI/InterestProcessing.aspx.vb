'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	DailyInterestProcessing.aspx.vb
' Author Name		:	Ashish Srivastava
' Employee ID		:	51821
' Email				:	 
' Contact No		:	
' Creation Time		:   20-Aug-2008	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : 
'*******************************************************************************
'Changed By:        On:                  IssueId
'*************************************************************************************************************************************************************************************
'Modification History
'*************************************************************************************************************************************************************************************
'Modified By			        Date		        Description
'*************************************************************************************************************************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*************************************************************************************************************************************************************************************
#Region "Import Namespace"
Imports System.IO.File
Imports System
Imports System.Data
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.IO
Imports System.Text
Imports YMCARET.YmcaBusinessObject

#End Region
Public Class InterestProcessing
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("InterestProcessing.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelMonthEnd As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonPost As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents MessagePlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RadioButtonProcessingOption As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DateusercontrolInterestDate As DateUserControl
    Protected WithEvents LabelProcessType As System.Web.UI.WebControls.Label
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button
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
        Try

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            'Me.Menu1.DataBind()

            If Not IsPostBack Then
                Session("string_InterestPostingOption") = Nothing
                Session("bool_Error") = Nothing
                Session("dateTime_InterestDate") = Nothing
                LoadDefaultValue()
            Else
                If Not (Session("bool_Error")) Is Nothing Then
                    If CType(Session("bool_Error"), Boolean) = True Then

                        Throw New Exception(CType(Session("String_ProcessMessage"), String).Trim)
                    Else
                        Response.Redirect("StatusPageForm.aspx?Message=" + Server.UrlEncode(CType(Session("String_ProcessMessage"), String).Trim) + "&ProcessType=" + Server.UrlEncode(CType(Session("string_InterestPostingOption"), String)).Trim, False)
                    End If
                End If
            End If
            'If Not Session("InterestProcessOk") = Nothing Then
            '    If Request.Form("YES") = "Yes" And Session("InterestProcessOk") = True Then
            '        Session("InterestProcessOk") = Nothing
            '        ProcessInterest()
            '    Else
            '        Session("InterestProcessOk") = Nothing
            '    End If
            'End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("InterestProcessing_Page_Load", ex)
        End Try

    End Sub

#Region "Private Methods"
    Private Sub LoadDefaultValue()
        Dim objDailyInterestProcessing As YMCARET.YmcaBusinessObject.InterestProcessingBOClass

        Try
            Dim dtCurrentDate As DateTime
            dtCurrentDate = System.DateTime.Today
            objDailyInterestProcessing = New YMCARET.YmcaBusinessObject.InterestProcessingBOClass
            objDailyInterestProcessing.GetAccountDate()
            If Not Session("CurrentAccDate") Is Nothing Then
                Session("CurrentAccDate") = Nothing

            End If
            Session("CurrentAccDate") = objDailyInterestProcessing.DateTime_EndingDate
            If dtCurrentDate > objDailyInterestProcessing.DateTime_EndingDate Then
                RadioButtonProcessingOption.Items(1).Selected = True
                LabelMonthEnd.Text = "Month End Closing Date"
                DateusercontrolInterestDate.Text = objDailyInterestProcessing.DateTime_EndingDate.ToString("MM/dd/yyyy")
            Else
                RadioButtonProcessingOption.Items(0).Selected = True
                LabelMonthEnd.Text = "Daily Interest Date"
                DateusercontrolInterestDate.Text = dtCurrentDate.AddDays(-1).ToString("MM/dd/yyyy")
            End If

        Catch 
            Throw
        End Try
    End Sub

    Private Function StatusBarScript() As String
        Dim popupScript As String = String.Empty
        Try
            popupScript = "var dots = 0;var dotmax = 99;var output = 1;"
            popupScript += " function ShowWait()"
            popupScript += "{var  width; if (output != 1) { width = new String(prgTD.style.width); output = parseInt(width.substring(0,width.length-1));}"
            popupScript += "dots++; if(dots>=dotmax) dots=1; output++; width = output + '%'; if(typeof(prgTD) != 'undefined') prgTD.style.width = output;"
            popupScript += "if(typeof(prgTD) != 'undefined') prgTD.style.backgroundColor = 'blue';if(typeof(prgSt) != 'undefined') prgSt.innerText = 'Processing of Monthly Closing: ';   if (output == 450) output = 1; }"
            popupScript += " function StartShowWait(){ window.setInterval('ShowWait()',100);}"
            popupScript += " function HideWait(){window.clearInterval();} "
            popupScript += " StartShowWait();"

            Return popupScript

        Catch
            Throw
        End Try
    End Function


    Private Function ProcessInterest()
        Dim l_dateTime As DateTime
        Dim objInterestProcessing As YMCARET.YmcaBusinessObject.InterestProcessingBOClass
        Try
            l_dateTime = Convert.ToDateTime(Me.DateusercontrolInterestDate.Text)
            objInterestProcessing = New YMCARET.YmcaBusinessObject.InterestProcessingBOClass
            objInterestProcessing.GetAccountDate()

            If RadioButtonProcessingOption.SelectedValue.Equals("1") Then
                If l_dateTime.Date() = objInterestProcessing.DateTime_EndingDate.Date() Then

                    objInterestProcessing.UpdateAccountingDate(l_dateTime)
                    objInterestProcessing.GetAccountDate()

                End If
                Session("string_InterestPostingOption") = "MonthEndInterest"
                Session("dateTime_InterestDate") = l_dateTime
            Else
                If RadioButtonProcessingOption.SelectedValue.Equals("0") Then
                    Session("string_InterestPostingOption") = "DailyInterest"
                    Session("dateTime_InterestDate") = l_dateTime
                End If
            End If
            Dim popupScript As String = StatusBarScript()
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "RunStatusBar", popupScript, True)

            Dim popupScript1 As String = "window.open('InterestProcessingStart.aspx', 'InterestProcessingStart', " & _
                            "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript1, True)

            Me.ButtonPost.Enabled = False
        Catch
            Throw
        End Try
    End Function

#End Region
#Region "Events"

    Private Sub RadioButtonProcessingOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonProcessingOption.SelectedIndexChanged
        Try
            If RadioButtonProcessingOption.SelectedValue.Equals("1") Then
                If Not Session("CurrentAccDate") Is Nothing Then
                    DateusercontrolInterestDate.Text = DirectCast(Session("CurrentAccDate"), DateTime).ToString("MM/dd/yyyy")
                End If
                LabelMonthEnd.Text = "Month End Closing Date"
            Else
                LabelMonthEnd.Text = "Daily Interest Date"
                DateusercontrolInterestDate.Text = System.DateTime.Today.AddDays(-1)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("InterestProcessing_RadioButtonProcessingOption_SelectedIndexChanged", ex)
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch
            Throw
        End Try
    End Sub

    Private Sub ButtonPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPost.Click
        Dim l_dateTime As DateTime
        Dim l_dateTime_PrevMonthDate As DateTime
        Dim l_string_ProcessType As String = String.Empty

        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'MessageBox.Show(MessagePlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940 
            'Validate date 
            Dim popupScript As String = String.Empty

            l_dateTime = Convert.ToDateTime("01/01/1900")

            Try
                l_dateTime = Convert.ToDateTime(Me.DateusercontrolInterestDate.Text)
            Catch ex As Exception
                'MessageBox.Show(MessagePlaceHolder, "Month End Closing", "Invalid " & LabelMonthEnd.Text & " Entered.", MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser("Invalid " & LabelMonthEnd.Text & " Entered.", EnumMessageTypes.Error)
                Exit Sub
            End Try

            'If Month End RaioButton Selected( 0 for DailyInterest and 1 for MonthEnd Interest
            If RadioButtonProcessingOption.SelectedValue.Equals("1") Then

                ''Added tag in "RUNFutureMonthEnd" Month End Process to allow / disallow the Month end Process. /b
                If Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("RUNFutureMonthEnd")).ToUpper() <> "ON" Then

                    If l_dateTime > System.DateTime.Now Then
                        'MessageBox.Show(MessagePlaceHolder, "Month End Closing", "Invalid Month Ending Date Entered.. The Date Entered is in the future", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Invalid Month Ending Date Entered.. The Date Entered is in the future", EnumMessageTypes.Error)
                        Exit Sub
                    End If
                End If

                Dim objInterestProcessing As New YMCARET.YmcaBusinessObject.InterestProcessingBOClass
                objInterestProcessing.GetAccountDate()

                If l_dateTime > objInterestProcessing.DateTime_EndingDate Then
                    'MessageBox.Show(MessagePlaceHolder, "Month End Closing", "Invalid Month Ending Date Entered.", MessageBoxButtons.Stop)
                    HelperFunctions.ShowMessageToUser("Invalid Month Ending Date Entered.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                l_dateTime_PrevMonthDate = objInterestProcessing.DateTime_StartingDate.AddDays(-1)
                'If l_dateTime < objInterestProcessing.DateTime_StartingDate.AddDays(-1) Then
                '    MessageBox.Show(MessagePlaceHolder, "Month End Closing", "Invalid Month Ending Date Entered.", MessageBoxButtons.Stop)
                '    Exit Sub
                'End If


                If l_dateTime < objInterestProcessing.DateTime_EndingDate And l_dateTime <> l_dateTime_PrevMonthDate Then
                    'MessageBox.Show(MessagePlaceHolder, "Month End Closing", "Invalid Month Ending Date Entered.", MessageBoxButtons.Stop)
                    HelperFunctions.ShowMessageToUser("Invalid Month Ending Date Entered.", EnumMessageTypes.Error)
                    Exit Sub
                End If
                l_string_ProcessType = "month end closing"


            Else 'if Daily Interest Radio Button Selected
                If RadioButtonProcessingOption.SelectedValue.Equals("0") Then
                    'Validation for Daily Interest
                    ''Added tag in "RUNFutureMonthEnd" Month End and DailyInterest Process to allow / disallow Run Interest for future date. /b
                    If Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("RUNFutureMonthEnd")).ToUpper() <> "ON" Then

                        If l_dateTime.Date > System.DateTime.Now.Date Then

                            'MessageBox.Show(MessagePlaceHolder, "Daily Interest", "Invalid Daily Interest Date Entered.. Daily Interest Date can not be future date", MessageBoxButtons.Stop)
                            HelperFunctions.ShowMessageToUser("Invalid Daily Interest Date Entered.. Daily Interest Date can not be future date", EnumMessageTypes.Error)
                            Exit Sub

                        End If

                        If l_dateTime.Date = System.DateTime.Now.Date And (System.DateTime.Now.Hour < 17) Then
                            'MessageBox.Show(MessagePlaceHolder, "Daily Interest", "Daily Interest for today date can not be Run until 5 PM.", MessageBoxButtons.Stop)
                            HelperFunctions.ShowMessageToUser("Daily Interest for today date can not be Run until 5 PM.", EnumMessageTypes.Error)
                            Exit Sub
                        End If
                    End If
                    Dim objInterestProcessing As New YMCARET.YmcaBusinessObject.InterestProcessingBOClass
                    objInterestProcessing.GetAccountDate()

                    If l_dateTime > objInterestProcessing.DateTime_EndingDate Then
                        'MessageBox.Show(MessagePlaceHolder, "Daily Interest", "Invalid Daily Interest Date Entered.. Please first run MonthEnd Interest for date " & objInterestProcessing.DateTime_EndingDate.ToString("MM/dd/yyyy") & ".", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Invalid Daily Interest Date Entered.. Please first run MonthEnd Interest for date.", EnumMessageTypes.Error)
                        Exit Sub
                    End If


                    If (l_dateTime < objInterestProcessing.DateTime_StartingDate) Or (l_dateTime >= objInterestProcessing.DateTime_EndingDate) Then
                        'MessageBox.Show(MessagePlaceHolder, "Daily Interest", "Invalid Daily Interest Date Entered.", MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser("Invalid Daily Interest Date Entered.", EnumMessageTypes.Error)
                        Exit Sub
                    End If
                    l_string_ProcessType = "daily interest"

                End If

            End If

            'MessageBox.Show(Me.MessagePlaceHolder, "YMCA-YRS", "Are you sure you want to process the " & l_string_ProcessType & " interest?", MessageBoxButtons.YesNo)
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','Are you sure you want to process the " & l_string_ProcessType & " ?','YES');", True)
            Session("InterestProcessOk") = True

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("InterestProcessing_ButtonPost_Click", ex)
        End Try
    End Sub

    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog');", True)
        Catch
            Throw
        End Try
    End Sub

    Protected Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog');", True)
            ProcessInterest()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("InterestProcessing_btnYes_Click", ex)
        End Try

    End Sub
#End Region

 
End Class
