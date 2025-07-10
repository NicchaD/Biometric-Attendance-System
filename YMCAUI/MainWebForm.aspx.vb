'******************************************************************************
'Modification History
'******************************************************************************
'Modified by                    Date                Description
'******************************************************************************
'Hafiz                          04Feb06              Cache-Session

'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Deven                          23 Aug. 2010        Added code to show not authorized message box
'Bhavna Shrivastava             2011.08.19          - for keep the track of history/record of  visited pages in the past
'Bhavna Shrivastava             2011.08.26          - delete history/record of  visited pages in the past
'Bhavna Shrivastava             2011.08.30          - change in style of usertracking history module,change in Fetching and clearing history
'Shashank Patel					2013.12.13			- BT:2326 -Show all the message into New message box div.
'Anudeep A                      2014.08.26          - BT:2628 :YRS 5.0-2405-Consistent screen header sections 
'Anudeep A                      2015.06.03          - BT:2628 :YRS 5.0-2405-Consistent screen header sections
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'******************************************************************************
Public Class MainWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MainWebForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents PlaceHolderMessage As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents YMCA_Footer_WebUserControl As YMCAUI.YMCA_Footer_WebUserControl
	Protected WithEvents rptMenuHistory As System.Web.UI.WebControls.Repeater
	Protected WithEvents linkClearHistory As System.Web.UI.WebControls.LinkButton

    Protected WithEvents lbl As System.Web.UI.WebControls.Label
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
        Dim alertWindow As String

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
		End If
        'SP 2013.12.27 --Added Try catch
        Dim dtFTList As DataTable
        Try


            'Srart:Added by Deven to remove SecurityCheck.aspx functionality, 23 Aug. 2010
            If Not Session("NoAccess") Is Nothing Then
                MessageBox.Show(PlaceHolderMessage, "YMCA-YRS", "You are not authorized to view this page.", MessageBoxButtons.Stop)
                Session("NoAccess") = Nothing
            End If
            'End:
            'Session("VignettePath1") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsResultsForm.aspx?fundid="
            'Session("VignettePath") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsResultsForm.aspx?fundid="
            'Commented By Aparna Samala -02/01/2007 - not used anymore
            ' Session("VignettePath1") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsYmcaresultsForm.aspx?fundid="
            ' Session("VignettePath") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsYmcaresultsForm.aspx?fundid="
            'Commented By Aparna Samala -02/01/2007 - not used anymore

            If Not Session("LoggedUserKey") = 0 Then
                Fetch_History()
            End If

            If Session("blnSuccessful") = True Then
                'Code added by ashutosh on 27-05-06*********************
                'alertWindow = "<script language='javascript'>" & _
                '                         "alert('PURCHASE SUCCESSFUL!');" & _
                '                         "</script>"
                'If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                '    Page.RegisterStartupScript("alertWindow", alertWindow)
                'End If
                '***************************************
                'MessageBox.Show(PlaceHolderMessage, "YMCA-YRS", "Purchase Successful! ", MessageBoxButtons.OK)
                Session("blnSuccessful") = False
                If Session("RetirementProcessPersonDetails") IsNot Nothing Then
                    Dim strParam() As String = CType(Session("RetirementProcessPersonDetails"), String).Split(CChar("|"))
                    Session("RetirementProcessPersonDetails") = Nothing
                    HelperFunctions.ShowMessageToUser(String.Format(GetMessage("RetirementMessages", "MESSAGE_RETIREMENT_PROCESSING_PURCHASE_SUCCESFULL"), strParam), EnumMessageTypes.Success, Nothing) 'SP 2013.12.13 BT 2326
                End If

                'AA:26.08.2013 BT:2628 YRS 5.0-2405 -  Added for loan maintenance actions
                If Session("LoanMaintenanceAction") IsNot Nothing Then
                    Dim strString As String
                    strString = Session("LoanMaintenanceAction")
                    Session("LoanMaintenanceAction") = Nothing
                    HelperFunctions.ShowMessageToUser(strString, EnumMessageTypes.Success)
                    'Start :AA:06.03.2015 BT:2628 YRS 5.0-2405 -Added to open report and copy pdf to IDM
                    Dim popupScript As String
                    If Session("strReportName") IsNot Nothing Then
                        popupScript = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp_1', " & _
                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "PopupScript1", popupScript, True)
                    End If
                    If Session("strReportName_1") IsNot Nothing Then
                        popupScript = "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_2', " & _
                   "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "PopupScript2", popupScript, True)
                    End If
                    If Session("FTFileList") IsNot Nothing Then
                        dtFTList = Session("FTFileList")
                        If HelperFunctions.isNonEmpty(dtFTList) Then
                            popupScript = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp_5', " & _
                            "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript5", popupScript, True)
                        End If
                    End If
                    'End:AA:06.03.2015 BT:2628 YRS 5.0-2405 -Added to open report and copy pdf to IDM
                End If

            End If
            'Code added by ashutosh on 27-05-06*********************
            If Session("blnCashReceiptEntryPosted") = True Or Session("blnLockBoxReceiptEntryPosted") = True Then
                'alertWindow = "<script language='javascript'>" & _
                '            "alert('Posted Successfully');" & _
                '            "</script>"

                'If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                '    Page.RegisterStartupScript("alertWindow", alertWindow)
                'End If
                'MessageBox.Show(PlaceHolderMessage, "YMCA-YRS", " Posted Successfully  ", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser("Posted Successfully", EnumMessageTypes.Success, Nothing) 'SP 2013.12.13 BT 2326
                Session("blnCashReceiptEntryPosted") = Nothing
                Session("blnLockBoxReceiptEntryPosted") = Nothing
                '***************************************
            End If


            '***********************************

        Catch ex As Exception
            HelperFunctions.LogException("Page_Load-MainWebForm Page", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
	End Sub
	'BS:2011.08.19 - for keep the track of history/record of  visited pages in the past
	Private Sub Fetch_History()
		Dim ds As DataSet
		Try
			If Not Session("LoggedUserKey") = 0 Or Session("LoggedUserKey") = Nothing Then
				ds = YMCARET.YmcaBusinessObject.YMCAMenuHistoryBOClass.Fetch_MenuHistory(Session("LoggedUserKey"))
			End If
		Catch ex As Exception
			HelperFunctions.LogMessage(String.Format("Error occured while fetching history for {1}: {0}", ex.Message, Session("LoggedUserKey")))
		End Try
		BindHistory(ds)
	End Sub
	Private Sub BindHistory(ByVal ds As DataSet)
		If HelperFunctions.isNonEmpty(ds) Then
			rptMenuHistory.Visible = True
			rptMenuHistory.DataSource = ds
			rptMenuHistory.DataBind()
		Else
			rptMenuHistory.DataSource = Nothing
			rptMenuHistory.DataBind()
			rptMenuHistory.Visible = False
		End If
	End Sub
	'SP 2013.12.30 BT:2326 -Added for handle exception if key not found
	Public Function GetMessage(ByVal resourceFileName As String, ByVal resourcemessage As String)

		Dim strMessage As String
		Try
			strMessage = GetGlobalResourceObject(resourceFileName, resourcemessage).ToString()
		Catch ex As System.Resources.MissingManifestResourceException
			Throw New System.Resources.MissingManifestResourceException(resourcemessage + " Key not found in resource file.", ex)
		Catch ex As Exception
			Throw New Exception(resourcemessage + " Key not found in resource file.", ex)
		End Try

		Return strMessage

	End Function

	'BS:2011.08.26 - clear history/record of  visited pages in the past
	Protected Sub LinkButton_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
		Try
			If e.CommandName <> "ClearHistory" Then Exit Sub
			If Not Session("LoggedUserKey") = 0 Or Session("LoggedUserKey") = Nothing Then
				YMCARET.YmcaBusinessObject.YMCAMenuHistoryBOClass.Clear_MenuHistory(Session("LoggedUserKey"))
				BindHistory(Nothing)
			End If
		Catch Ex As Exception
			HelperFunctions.LogMessage(String.Format("Error occured while clearing history: {0}", Ex.Message))
		End Try
	End Sub

End Class
