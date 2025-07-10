<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RefundRequestCancel.aspx.vb"
    Inherits="YMCAUI.RefundRequestCancel" %>

<head>

<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">

    <style type="text/css">
        .style1
        {
            width: 40%;
        }
    </style>
</head>
<form id="Form1" method="post" runat="server">
			<DIV class="Div_Center">
				<TABLE width="742">
					<TR>
						<TD align="center">
							<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowHomeLinkButton="false" ShowLogoutLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></TD>
					</TR>
					<TR>
						<TD class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
							Cancel Refund Request
						</TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
					<TR>
						<TD></TD>
					</TR>
				</TABLE>
			</DIV>
			<DIV class="Div_Center">
				<TABLE class="Table_WithBorder" width="742" border="0">
					
					<TR vAlign="top">
						<TD align="left" class="style1">
							<asp:label id="LabelType" runat="server" CssClass="Label_Small">Reason to cancel the Refund Request: </asp:label></TD>
						<TD align="left" width="85%">
							<asp:dropdownlist id="ddlReasonCode" runat="server" CssClass="DropDown_Normal" Width="70%"></asp:dropdownlist>
							
								</TD>
					</TR> 
				
				
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>

                    <TR>
						<TD>&nbsp;
						</TD>
					</TR>
                                        <TR>
						<TD>&nbsp;
						</TD>
					</TR>
                                        <TR>
						<TD>&nbsp;
						</TD>
					</TR>
					
					
					<TR>
						<TD class="Td_ButtonContainer" align="right" colSpan="2">
							<asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:button>&nbsp;&nbsp;&nbsp;
							<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="87" Text="Cancel"></asp:button></TD>
					</TR>
				</TABLE>
			</DIV>
			<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
	
</FORM>
<!--#include virtual="bottom.html"-->
