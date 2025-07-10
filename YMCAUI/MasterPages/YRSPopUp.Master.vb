'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	YRSPopUp.Master.vb
' Author Name		:	
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	
' Changed by			:	
' Changed on			:	
' Change Description	:	

'*******************************************************************************
'Changed By         Changes on              Description
'Shashank           2014.02.04              BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
'*******************************************************************************
Public Class YRSPopUp
	Inherits System.Web.UI.MasterPage

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim msg As String
		Try
			If Session("LoggedUserName") Is Nothing Then
				msg = "<Script Language='JavaScript'>"
				msg = msg + "window.opener.location.forms[0].submit();"
				msg = msg + "window.close();"
				msg = msg + "</Script>"
				Response.Write(msg)
				Exit Sub
			End If
		Catch
			Throw
		End Try
	End Sub
	'Start:SP:2014.02.04-BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN 
	Private Shared Sub AddMessageToDiv(ByVal messageType As EnumMessageTypes, ByVal htDivTemp As HtmlGenericControl, ByVal strMessage As String)
		Dim htDiv As HtmlGenericControl
		Try
			If Not String.IsNullOrEmpty(strMessage) Then


				Select Case messageType
					Case EnumMessageTypes.Information
						htDiv = DirectCast(htDivTemp.FindControl("info-msg"), HtmlGenericControl)
						If htDiv Is Nothing Then
							htDiv = New HtmlGenericControl("div")
							htDiv.ID = "info-msg"
							htDiv.Attributes.Add("class", "info-msg")
							htDivTemp.Controls.Add(htDiv)
						End If
						htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
						Exit Select
					Case EnumMessageTypes.[Error]
						htDiv = DirectCast(htDivTemp.FindControl("error-msg"), HtmlGenericControl)
						If htDiv Is Nothing Then
							htDiv = New HtmlGenericControl("div")
							htDiv.ID = "error-msg"
							htDiv.Attributes.Add("class", "error-msg")
							htDivTemp.Controls.Add(htDiv)
						End If
						htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
						Exit Select
					Case EnumMessageTypes.Success
						htDiv = DirectCast(htDivTemp.FindControl("success-msg"), HtmlGenericControl)
						If htDiv Is Nothing Then
							htDiv = New HtmlGenericControl("div")
							htDiv.ID = "success-msg"
							htDiv.Attributes.Add("class", "success-msg")
							htDivTemp.Controls.Add(htDiv)
						End If
						htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
						Exit Select
					Case EnumMessageTypes.Warning
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

	Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
		Dim lsObjList As List(Of Dictionary(Of String, Object))
		Dim htDiv As HtmlGenericControl
		Dim strMessage As String
		Dim messageType As EnumMessageTypes
		Try
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
		Catch ex As Exception
			Throw ex
		End Try
	End Sub
	'End:SP:2014.02.04-BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN 


End Class