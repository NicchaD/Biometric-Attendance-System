<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CopyFilestoFileServer.aspx.vb" Inherits="YMCAUI.CopyFilestoFileServer" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<form id="Form1" method="post" runat="server">
	<table align="center" valign="middle" border="0" class="Table_WithoutBorder" cellSpacing="0"
		cellPadding="0" width="100%">
		<tr>
			<td align=center>
				<div align=center>
					<asp:Label id="LabelProgress" runat="server" CssClass="Label_Small" Visible="False">File is getting copied, Please Wait ...</asp:Label>
				</div>
			</td>
		</tr>
	</table>
</form>
