'****************************************************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved.
' Project Name		:	YMCA-YRS
' FileName			:	
' Author Name		:	
' Employee ID		:	
' Email			    :	
' Creation Date	    :	
' Program Specification Name	: 
' Unit Test Plan Name			:	
' Description					: 
'****************************************************************************************************************************
' Change History 
'****************************************************************************************************************************
'Modified By			 Date					 Desription
'****************************************************************************************************************************
'Shashank Patel		     10-Sep-2013		     BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
'Anudeep  Adusunilli     03-feb-2014             BT:2292:YRS 5.0-2248 - YRS Pin number    
'Shashank                18-Aug-2014             BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
'Manthan Rajguru         2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru         2015.09.21              YRS-AT-3631 - YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
'Santosh Bura            2018.08.09              YRS-AT-4017 - YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'******************************************************************************************************************************
Imports YMCARET.YmcaBusinessObject

Public Class MainMaster
	Inherits System.Web.UI.MasterPage

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			If Session("LoggedUserKey") Is Nothing Then
				Response.Redirect("Login.aspx", False)
				Exit Sub
			End If
			Menu1.DataSource = Server.MapPath("SimpleXML.xml")
			Menu1.DataBind()
			Dim dt As DataTable
			Dim strURL As String
			Dim strDisplayURL As String
			Dim ctx As HttpContext = HttpContext.Current
			dt = Session("dtapplicationcache")
			If dt Is Nothing Then
				dt = DirectCast(ctx.Application("SimpleXmlCache"), DataTable)
			End If
			strURL = ctx.Request.RawUrl
			For Each dr As DataRow In dt.Rows
                strDisplayURL = dr("Url").ToString()
                strDisplayURL = strDisplayURL.Replace("SecurityCheck.aspx?Form=", "/")
                'Added to get the Module name in the header label
                If strDisplayURL.Length > 0 AndAlso strDisplayURL.Chars(0) <> "/" Then
                    strDisplayURL = "/" + strDisplayURL
                End If
                strDisplayURL = strDisplayURL.Replace("%26", "&")
                If strDisplayURL IsNot Nothing And strDisplayURL IsNot String.Empty And strDisplayURL.Contains("/") Then
                    If (strURL.Substring(strURL.LastIndexOf("/"), (strURL.Length - strURL.LastIndexOf("/"))) = strDisplayURL) Then
                        LabelModuleName.Text = dr("Display").ToString()
                        Exit For
                    End If
                End If
            Next

            'START: SB | 08/09/2018 | YRS-AT-4017 | LAC pages are different but menu url is same for them, so to display module name properly following code needs to be written
            If LabelModuleName.Text = "" Then
                If strURL.IndexOf("LACWebLoans.aspx") >= 0 Or strURL.IndexOf("LACLoansProcessing.aspx") >= 0 Or strURL.IndexOf("LACExceptions.aspx") >= 0 Or strURL.IndexOf("LACStatistics.aspx") >= 0 Or strURL.IndexOf("LACRate.aspx") >= 0 Then
                    Dim row As System.Data.DataRow() = dt.Select("Url='SecurityCheck.aspx?Form=LACWebLoans.aspx'")
                    If Not row Is Nothing AndAlso row.Length > 0 Then
                        LabelModuleName.Text = row(0)("Display").ToString()
                    End If
                End If
            End If
            'END: SB | 08/09/2018 | YRS-AT-4017 | LAC pages are different but menu url is same for them, so to display module name properly following code needs to be written
        Catch
            Throw
		End Try
	End Sub

    'SP : 2013.09.25 : BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
    'SP 2014.08.18  BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site -Chnage the anum type in switch case
	Private Shared Sub AddMessageToDiv(ByVal messageType As EnumMessageTypes, ByVal htDivTemp As HtmlGenericControl, ByVal strMessage As String)
		Dim htDiv As HtmlGenericControl
		Try
			If Not String.IsNullOrEmpty(strMessage) Then


                Select Case messageType
                    Case YMCAObjects.EnumMessageTypes.Information
                        htDiv = DirectCast(htDivTemp.FindControl("info-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "info-msg"
                            htDiv.Attributes.Add("class", "info-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case YMCAObjects.EnumMessageTypes.Error
                        htDiv = DirectCast(htDivTemp.FindControl("error-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "error-msg"
                            htDiv.Attributes.Add("class", "error-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case YMCAObjects.EnumMessageTypes.Success
                        htDiv = DirectCast(htDivTemp.FindControl("success-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "success-msg"
                            htDiv.Attributes.Add("class", "success-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case YMCAObjects.EnumMessageTypes.Warning
                        htDiv = DirectCast(htDivTemp.FindControl("warning-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "warning-msg"
                            htDiv.Attributes.Add("class", "warning-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case Else
                        Exit Select
                End Select
			End If
		Catch ex As Exception

			Throw
		Finally
			htDiv = Nothing

		End Try
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Dim lsObjList As List(Of Dictionary(Of String, Object))
        Dim htDiv As HtmlGenericControl
        Dim strMessage As String
        Dim messageType As EnumMessageTypes
        lsObjList = DirectCast(System.Web.HttpContext.Current.Items("ErrorMessages"), List(Of Dictionary(Of String, Object)))
        If lsObjList IsNot Nothing Then
            For Each dict As Dictionary(Of String, Object) In lsObjList
                strMessage = Convert.ToString(dict("message"))
                htDiv = DirectCast(dict("div"), HtmlGenericControl)
                messageType = DirectCast(dict("messageType"), EnumMessageTypes)
                If Not String.IsNullOrEmpty(strMessage) Then
                    AddMessageToDiv(messageType, If(htDiv Is Nothing, DivMainMessage, htDiv), strMessage)
                End If
            Next
        End If

        'START: MMR |  2017.09.21 | YRS-AT-3665 | Display success messsage on next screen after succcessfull activity on existing screen
        If Not System.Web.HttpContext.Current.Session("NextPageMessages") Is Nothing Then
            lsObjList = DirectCast(System.Web.HttpContext.Current.Session("NextPageMessages"), List(Of Dictionary(Of String, Object)))
            If lsObjList IsNot Nothing Then
                For Each dict As Dictionary(Of String, Object) In lsObjList
                    strMessage = Convert.ToString(dict("message"))
                    htDiv = DirectCast(dict("div"), HtmlGenericControl)
                    messageType = DirectCast(dict("messageType"), EnumMessageTypes)
                    If Not String.IsNullOrEmpty(strMessage) Then
                        AddMessageToDiv(messageType, If(htDiv Is Nothing, DivMainMessage, htDiv), strMessage)
                    End If
                Next
            End If
            System.Web.HttpContext.Current.Session("NextPageMessages") = Nothing
        End If
        'END: MMR |  2017.09.21 | YRS-AT-3665 | Display success messsage on next screen after succcessfull activity on existing screen
    End Sub

	'SP : 2013.09.25 : BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
End Class