<%@ Control Language="vb" AutoEventWireup="false" Codebehind="DataGridPager.ascx.vb" Inherits="YMCAUI.DataGridPager" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div align="center">
	<asp:linkbutton id="lnkPrevPageList" onclick="ChangePage" Text="Prev 20" CommandArgument="..." runat="server"
		Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkFirstbutton" onclick="ChangePage" Text="<<" CommandArgument="0" runat="server"></asp:linkbutton>
	<asp:linkbutton id="lnkprevbutton" onclick="ChangePage" Text="<" CommandArgument="prev" runat="server"></asp:linkbutton>
	<asp:linkbutton id="lnkpg1" onclick="ChangePage" Text="" CommandArgument="0" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg2" onclick="ChangePage" Text="" CommandArgument="1" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg3" onclick="ChangePage" Text="" CommandArgument="2" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg4" onclick="ChangePage" Text="" CommandArgument="3" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg5" onclick="ChangePage" Text="" CommandArgument="4" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg6" onclick="ChangePage" Text="" CommandArgument="5" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg7" onclick="ChangePage" Text="" CommandArgument="6" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg8" onclick="ChangePage" Text="" CommandArgument="7" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg9" onclick="ChangePage" Text="" CommandArgument="8" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg10" onclick="ChangePage" Text="" CommandArgument="9" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg11" onclick="ChangePage" Text="" CommandArgument="10" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg12" onclick="ChangePage" Text="" CommandArgument="11" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg13" onclick="ChangePage" Text="" CommandArgument="12" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg14" onclick="ChangePage" Text="" CommandArgument="13" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg15" onclick="ChangePage" Text="" CommandArgument="14" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg16" onclick="ChangePage" Text="" CommandArgument="15" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg17" onclick="ChangePage" Text="" CommandArgument="16" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg18" onclick="ChangePage" Text="" CommandArgument="17" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg19" onclick="ChangePage" Text="" CommandArgument="18" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnkpg20" onclick="ChangePage" Text="" CommandArgument="19" runat="server" Visible="false"></asp:linkbutton>
	<asp:linkbutton id="lnknextbutton" onclick="ChangePage" Text=">" CommandArgument="next" runat="server"></asp:linkbutton>
	<asp:linkbutton id="lnklastbutton" onclick="ChangePage" Text=">>" CommandArgument="last" runat="server"></asp:linkbutton>
	<asp:linkbutton id="lnkNewPageList" onclick="ChangePage" Text="Next 20" CommandArgument=".." runat="server"
		Visible="false"></asp:linkbutton>
</div>
