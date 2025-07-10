<%@ Page Language="vb" %>

<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.IO" %>
<script language="vb" runat="server">
	'**********************************************************************
	'IMPORTANT: Make sure that the below URL points to the right location.
	Dim ProWebURL As String = "http://yaddrverify.ymcaret.org/qas/prowebproxy.aspx"
	'**********************************************************************
	Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim requestPostData As String
		Try

			'This page access a proxy to the proweb page 
			'It accepts an incoming request and passes it on to the page defined in ProWebURL variable.
			'The response received from the Proweb server is then sent back to the requesting client.
			'
			'This is required since most browsers prompt the users when a javascript function tries 
			'to access data from another domain than the one from where it was loaded.
			'
			'You can trace calls to the Proweb service by configuring the Enterprise Library settings.
			
			Dim request As WebRequest = WebRequest.Create(ProWebURL & HttpContext.Current.Request.Url.Query)
			request.Credentials = CredentialCache.DefaultCredentials
			request.Method = HttpContext.Current.Request.HttpMethod
			request.ContentType = HttpContext.Current.Request.ContentType
			request.ContentLength = HttpContext.Current.Request.ContentLength
			
			If request.Method = "POST" Then
				Dim buf(request.ContentLength) As Byte
				Dim dStream As Stream = HttpContext.Current.Request.InputStream()
				Dim i As Integer = dStream.Read(buf, 0, buf.Length)
				dStream.Close()
				dStream.Dispose()
				If (i > 0) Then
					requestPostData = New String(Encoding.UTF8.GetChars(buf))
					dStream = request.GetRequestStream()
					dStream.Write(buf, 0, i)
					dStream.Close()
					dStream.Dispose()
				End If
			End If

			Dim resp As WebResponse = request.GetResponse()
			Dim dataStream As Stream = resp.GetResponseStream()
			Dim reader As New StreamReader(dataStream)
			Dim responseFromServer As String = reader.ReadToEnd()
			Response.ContentType = resp.ContentType
			Response.Write(responseFromServer)
			reader.Close()
			dataStream.Close()
			resp.Close()
			WriteLog(String.Format("<data><input><QueryString>{0}</QueryString><PostData>{1}</PostData></input><output>{1}</output></data>", HttpContext.Current.Request.Url.Query, requestPostData, responseFromServer))
		Catch ex As Exception
			WriteLog(String.Format("<data><input>{0}</input><error>{1}</error></data>", HttpContext.Current.Request.Url.Query, ex.Message & "-" & ex.StackTrace))
			SendErrorResponse(ex.Message)
		End Try
	End Sub
	Shared Sub WriteLog(ByVal msg As String)
		Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(msg, "Proweb")
	End Sub
	Sub SendErrorResponse(ByVal msg As String)
		Response.ContentType = "text/xml"
		Response.Write(String.Format("<proweb><verifylevel>Error</verifylevel><message>{0}</message></proweb>", msg))
	End Sub
</script>
