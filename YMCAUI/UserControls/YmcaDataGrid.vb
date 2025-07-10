Public Class YmcaDataGrid
	Inherits DataGrid
	Dim hid As New System.Web.UI.WebControls.HiddenField
	Dim scrollPos As String
	Public Property AnimateScroll As Boolean = False

	Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
		MyBase.Render(writer)
		Dim script As String = String.Empty
		
		script = "function initializeGrid(id) { var offset; if ($('table#' + id + ' .DataGrid_SelectedStyle').offset() ==  null) {offset = 0;} else { offset = $('table#' + id + ' .DataGrid_SelectedStyle').offset().top - $('table#' + id + ' .DataGrid_SelectedStyle').parent().parent().parent().offset().top;}" & _
		 "var h = $('table#' + id + ' .DataGrid_SelectedStyle').parent().parent().parent().height();" & _
		 "offset = (offset - h) + (h / 2); "
		If AnimateScroll Then
			script &= "$('table#' + id + ' .DataGrid_SelectedStyle').parent().parent().parent().animate({scrollTop: offset}, 1000);}"
		Else
			'script &= "$('table#' + id + ' .DataGrid_SelectedStyle').parent().parent().parent().animate({scrollTop: offset}, 1000);}"
			script &= "$('table#' + id + ' .DataGrid_SelectedStyle').parent().parent().parent().scrollTop(offset);}"
		End If
		script &= "$(document).ready( function() { initializeGrid( '" & Me.ClientID & "'); }); "

		writer.RenderBeginTag(HtmlTextWriterTag.Script)
		writer.Write(script)
		writer.RenderEndTag()

	End Sub



End Class
