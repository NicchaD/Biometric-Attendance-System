<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ACHDebitImportForm.aspx.vb" Inherits="YMCAUI.ACHDebitImportForm" MasterPageFile="~/MasterPages/YRSMain.Master" %>

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
    
            <div class="Div_Center">
				<table class="Table_WithoutBorder" width="100%">
                   <tr>
						<td width="70%">
                        </td>
                        <td align="right" width="15%">
                            <input id="FileField" type="file" name="FileField" runat="server" enableviewstate="true" />
                        </td>
						<td align="right" width="15%">
                        	<asp:button id="ButtonImport" runat="server" CssClass="Button_Normal" Width="80px" Text="Import"
						    causesValidation="false" Height="21px"></asp:button>
				        </td>
					</tr>
				</table>
			</div>
	<asp:UpdatePanel ID="UpdatePanelAchDebit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    		<div class="Div_Center">
				<table class="Table_WithBorder" width="100%">
					<tbody>
						<tr>
							<td align="center" colspan="4">
								<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 370px; TEXT-ALIGN: left">
                                <asp:datagrid id="DatagridACHDebImport" CssClass="DataGrid_Grid" Width="100%" Runat="server" allowsorting="true"
										AutoGenerateColumns="False">
										<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:CheckBox id="CheckBoxSelect" runat="server" autopostback="false" CssClass="CheckBox_Normal"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="YMCANo" HeaderText="YMCA No"></asp:BoundColumn>
											<asp:BoundColumn DataField="YMCAName" HeaderText="YMCA Name"></asp:BoundColumn>
											<asp:BoundColumn DataField="SOURCE" HeaderText="Source"></asp:BoundColumn>
											<asp:BoundColumn DataField="REFNO" HeaderText="Ref No"></asp:BoundColumn>
											<asp:BoundColumn DataField="PaymentDate" HeaderText="Pay Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="Recdate" HeaderText="Rec Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
											<asp:BoundColumn DataField="uniqueid" HeaderText="UniqueId" Visible="False"></asp:BoundColumn>
											<asp:BoundColumn DataField="Selected" HeaderText="Selected" Visible="False"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
                                    </div>
							</td>
						</tr>
					</tbody>
                    </table>
				<table class="td_Text" width="100%">
					<tr>
						<td align="left" width="23%" colspan="2">&nbsp;
                        <asp:button class="Button_Normal" id="ButtonDeSelectAll" Width="95" Text="Select None" causesValidation="true"
								Runat="server" enabled="False"></asp:button>
                                </td>
						<td  align="center" width="45%"><asp:button id="ButtonPostReceipts" runat="server" CssClass="Button_Normal" Width="100px" Text="Post Receipts"
								Enabled="False"></asp:button>&nbsp;&nbsp;<asp:button id="ButonMatchRecceipts" runat="server" CssClass="Button_Normal" Width="110px" Text="Match Receipts"
								Enabled="False"></asp:button>&nbsp;
							<asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="80" Text="Close"></asp:button>
						</td>
						<td align="right" width="32%">
                        <asp:button id="ButtonRecalculate" runat="server" CssClass="Button_Normal" Width="90" Text="Re-calculate"
								Enabled="False"></asp:button>&nbsp;&nbsp;&nbsp;<asp:textbox id="TextBoxTotal" runat="server" CssClass="TextBox_Normal_Amount" Width="100" Height="20px"
								readonly="true"></asp:textbox>&nbsp;&nbsp;&nbsp;</td>
					</tr>
				</table>
			</div>
      </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonReCalculate" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonPostReceipts" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButonMatchRecceipts" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDeSelectAll" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnOK" />
        </Triggers>
    </asp:UpdatePanel>
			<asp:placeholder id="PlaceHolderACHDebitImportProcess" runat="server"></asp:placeholder>
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
                <asp:AsyncPostBackTrigger ControlID="ButtonReCalculate" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonPostReceipts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButonMatchRecceipts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonDeSelectAll" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnOK" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
	</asp:Content>