<%@ Control Language="vb" AutoEventWireup="false" Codebehind="YMCA_Toolbar_WebUserControl.ascx.vb" Inherits="YMCAUI.YMCA_Toolbar_WebUserControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="100%">
	<TBODY>
		<tr align="left" height="48">
			
				<!--<IMG title="logo" height="48" alt="YMCA Retirement Fund Logo" src="images/ymca_logo.gif" width="244">-->
				<!--START: Shilpa N | 03/15/2019 | YRS-AT-4248 | Change CSS of Header on Read Only Access-->
               <% If (YMCAUI.SecurityCheck.GetApplicationReadonlyHeaderValue()) Then%>                                                        
					    <td  class="Td_HeaderColorInReadOnlyMode" runat="server" colspan="2">
                            <div style="float:left">
                                <img  title="logo" alt="YMCA Retirement Fund Logo" src="~/images/ymca_logo.gif" runat="server"/>
                            </div>
                    <div style="float:right;text-align:left;PADDING-TOP: 16px">
                            <%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.READONLY_APPLICATION_HEADER_MESSAGE).DisplayText%>&nbsp;&nbsp;
                        </div></td>
                <%Else%>
                        <td colspan="2" class="Td_BackGroundColorYMCA">
                            <img title="logo" alt="YMCA Retirement Fund Logo" src="~/images/ymca_logo.gif" runat="server"/>  
                        </td>
                <%End If%>  
                    <!--END: Shilpa N | 03/15/2019 | YRS-AT-4248 | Change CSS of Header on Read Only Access  -->      
                    
		</tr>
		<tr>
			<td class="Td_BackGroundColorWhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="Td_HeadingLines" colSpan="2"><!--<IMG title="Image" height="1" alt="image" src="images/spacer.gif" width="1">--><asp:image class="img_border0" id="ImageSpacer1" runat="server" ImageUrl="../images/spacer.gif"
					width="1" Height="1"></asp:image></td>
		</tr>
		<tr align="left">
			<td class="Td_WelcomeNoteContainer">
				<asp:image class="img_border0" id="ImageSpacer2" runat="server" ImageUrl="../images/spacer.gif"
					Height="1"></asp:image>
				<!--<IMG title="Image" height="10" alt="image"  src="images/spacer.gif">-->
				<asp:label id="LabelUserName" runat="server"></asp:label><b>@</b><asp:label id="LabelDBInfo" runat="server"></asp:label>				
                <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <cc1:HoverMenuExtender ID="HoverMenuShowVersion" runat="server" PopupControlID="dvShowVersion"
                    TargetControlID="imgVersion" PopupPosition="Bottom" OffsetX="0" OffsetY="0" PopDelay="60">
                </cc1:HoverMenuExtender>--%>
                <div runat="server" id="dvShowVersion" style="border:1px solid black;width:29%">
                </div>
			</td>
			<td class="Td_Home" vAlign="middle" align="right">
            <a href="javascript:void();" id="version" style="text-decoration:none">
                    <asp:Image alt="version" runat="server" ImageUrl="~/images/release_version.JPG" ID="imgVersion" />
                    
                </a>
				<A href="MainWebForm.aspx">
					<asp:image class="img_border0" AlternateText="Home" CausesValidation="false" UseSubmitBehavior="false" id="ImageHome" runat="server" ImageUrl="../images/homeCopy.gif"
						width="45"></asp:image><!--<IMG title="Image" id="ImageHome" alt="image" height="14" src="images/homeCopy.gif" width="45" class="img_border0">--></A><asp:image class="img_border0" id="ImageLine2" runat="server" ImageUrl="../images/line.gif" width="1"
					height="14"></asp:image>
				<!--<IMG title="Image" height="14" alt="image" src="images/line.gif" width="1">--> 
				&nbsp; <A href="#"></A>
				<asp:image class="img_border0" id="ImageLine1" Visible="false" runat="server" ImageUrl="../images/line.gif" width="1"
					Height="14"></asp:image>
				<!--<IMG title="Image" height="14" alt="image" src="images/line.gif" width="1">-->  <!--<A href="#"></A><A href="CrossButtonForSession.Aspx?Mess="+"logOut"><IMG title="Image" alt="image" height="14" src="images/logoutCopy.gif" width="50" class="img_border0">-->
				<A href="Logout.aspx">
                <%--<asp:ImageButton AlternateText="Logout" ID="ImgLogOut" CausesValidation="false" UseSubmitBehavior="false" runat="server" ImageUrl="../images/logoutCopy.gif" Width="50" height="14" />--%>
					<asp:image class="img_border0" id="ImageLogout" runat="server" ImageUrl="../images/logoutCopy.gif"
						width="50" height="14"></asp:image><!--<IMG title="Image" id="ImageLogout" alt="image" height="14" src="images/logoutCopy.gif"	width="50" class="img_border0">--></A>
			</td>
		</tr>
		<tr>
			<td class="Td_HeadingLines" colSpan="2"><asp:image class="img_border0" id="ImageSpacer3" runat="server" ImageUrl="../images/spacer.gif"
					Height="1" Width="1"></asp:image><!--<IMG title="Image" alt="image" height="1" src="images/spacer.gif" width="1">--></td>
		</tr>
	</TBODY>
</table>
