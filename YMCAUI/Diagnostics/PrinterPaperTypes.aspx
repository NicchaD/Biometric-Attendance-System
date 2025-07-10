<%@ Page Language="vb" AutoEventWireup="false"  %>

<%@ Import Namespace="System.Drawing.Printing" %>

<%@ Import Namespace="System.Security.Principal" %>
<%	'CodeBehind="PrinterPaperTypes.aspx.vb" Inherits="YMCAUI.PrinterPaperTypes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Test printer types</title>
	<script runat="server">
		Sub LoadPaperTypes_Click(ByVal sender As Object, ByVal e As EventArgs)
			Dim wic As WindowsImpersonationContext = WindowsIdentity.Impersonate(IntPtr.Zero)
			Try
				Dim PrintPermissions As New PrintingPermission(System.Security.Permissions.PermissionState.Unrestricted)
				PrintPermissions.Level = PrintingPermissionLevel.AllPrinting

				Response.Write(String.Format("<h1>Paper Types for printer {0}</h1>", printerName.Text))
				
				Dim printerSettings As System.Drawing.Printing.PrinterSettings = New System.Drawing.Printing.PrinterSettings
				printerSettings.PrinterName = printerName.Text
				Dim paperSource As System.Drawing.Printing.PaperSource
				Response.Write("<ul>")
				For Each paperSource In printerSettings.PaperSources
					Response.Write(String.Format("<li>{0}</li>", paperSource.SourceName.ToString()))
				Next
			Catch ex As Exception
				Response.Write(String.Format("<h1>{0}</h1><h3>{1}</h3>", ex.Message, ex.StackTrace))
			Finally
				wic.Undo()
			End Try
		End Sub
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
	<asp:TextBox ID="printerName" runat="server" />
	<asp:Button ID="LoadPaperTypes" runat="server" onclick="LoadPaperTypes_Click" Text="Show Paper Types" />
	</div>
	</form>
</body>
</html>
