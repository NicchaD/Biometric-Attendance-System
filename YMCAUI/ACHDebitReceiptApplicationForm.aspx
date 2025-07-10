<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ACHDebitReceiptApplicationForm.aspx.vb" Inherits="YMCAUI.ACHDebitReceiptApplicationForm" MasterPageFile="~/MasterPages/YRSMain.Master"  %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });

        function BindEvents() {
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 570, height: 330,
                title: "ACH Debit",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function showDialog(id, text, type) {

            $('#' + id).dialog({ modal: true });
            $('#lblMessage').text(text);
            $('#' + id).dialog("open");
            if (type == 'YESNO') {
                $("#btnYes").show();
                $("#btnNo").show();
                $("#btnOK").hide();
            }
            else if (type == 'OK') {
                $("#btnYes").hide();
                $("#btnNo").hide();
                $("#btnOK").show();
            }
        }

        function disableButton() {
            $('#lblMessage').text('Processing your request...');
            $("#btnYes").hide();
            $("#btnNo").hide();
            $("#btnOK").hide();

        }

        function closeDialog(id) {

            $('#' + id).dialog('close');
        }
    
    </script>
</asp:Content>
<asp:Content ID="ACHDebitImportContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<asp:ScriptManagerProxy ID="Achdebitscriptmanger" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanelAchDebit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
			<div class="Div_Center">
				<table class="Table_WithBorder" width="100%">
					<tr>
						<td align="left"></td>
					</tr>
					<tr>
						<td align="center">
							<table>
								<tr>
									<td align="center" width="50%"><asp:label id="LabelHRefNo" CssClass="Label_Small" Runat="server">Reference No: </asp:label></td>
									<td><asp:label id="LabelRefNo" CssClass="NormalMessageText" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="center" width="50%"><asp:label id="LabelHFundingDate" CssClass="Label_Small" Runat="server"> Funding Date: </asp:label></td>
									<td><YRSCONTROLS:DATEUSERCONTROL onpaste="return true" id="DateusercontrolFundedDate" runat="server"></YRSCONTROLS:DATEUSERCONTROL></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td align="center" colspan="4">
							<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 350px; TEXT-ALIGN: center">
                            <asp:datagrid id="DatagridACHDebitMatchReceipt" CssClass="DataGrid_Grid" Runat="server" AutoGenerateColumns="False"
									allowsorting="True">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:BoundColumn Visible="False" DataField="AchUniqueId" HeaderText="AchUniqueId"></asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="guiTransmittalId" HeaderText="TransmittalUniqueId"></asp:BoundColumn>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:CheckBox id="CheckBoxSelect" runat="server" autopostback="true" CssClass="CheckBox_Normal"
													Checked="True" OnCheckedChanged="Check_Clicked"></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="YMCANo" HeaderText="YMCA No">
											<HeaderStyle Wrap="False"></HeaderStyle>
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="ReceiptAmount" HeaderText="Receipt Amount" DataFormatString="{0:N}">
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TransmittalNo" HeaderText="Transmittal No">
											<HeaderStyle Width="110px"></HeaderStyle>
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TransmittalDate" HeaderText="Transmittal Date" DataFormatString="{0:MM/dd/yyyy}">
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TransmittalAmt" HeaderText="Transmittal Amount" DataFormatString="{0:N}">
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TotAppliedCredit" HeaderText="CR Applied" DataFormatString="{0:N}">
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="ReceiptApplied" HeaderText="Amount Due/ Receipt Applied" DataFormatString="{0:N}">
											<ItemStyle HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="Selected" HeaderText="Selected"></asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="VisibleStatus" HeaderText="Visible Status"></asp:BoundColumn>
									</Columns>
								</asp:datagrid>
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<table class="Td_ButtonContainer" width="100%">
								<tr>
									<td align="center" width="25%"><asp:button class="Button_Normal" id="ButtonBack" Runat="server" Width="100" causesValidation="true"
											Text="Back"></asp:button></td>
									<td align="center">
										<!--<asp:Textbox id="TextBoxTotal" runat="server" CssClass="TextBox_Normal_Amount" readonly="true"
															width="100"></asp:Textbox> --></td>
									<td align="center" width="25%"><asp:button id="ButtonPostAndApply" runat="server" CssClass="Button_Normal" Width="150px" Text="Post &amp; Apply Receipts"></asp:button>&nbsp;</td>
									<td align="center" width="25%"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="80" Text="Close"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
					</td></tr>
                    </table>
				
			</div>
			<asp:placeholder id="PlaceHolderPostAndApply" runat="server"></asp:placeholder>
 </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonBack" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonPostAndApply" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnOK" />
            </Triggers>
        </asp:UpdatePanel>             
<div id="ConfirmDialog" title="Ach Debit">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">
                                <asp:Button runat="server" ID="btnYes" Text="Yes" OnClientClick="javascript: disableButton();" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />&nbsp;
                                <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />
                                    <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;"  />&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonBack" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonPostAndApply" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnOK" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
	</asp:Content>