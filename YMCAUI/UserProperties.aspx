<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserProperties.aspx.vb" Inherits="YMCAUI.UserProperties" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="y" Namespace ="YMCAUI" Assembly="YMCAUI" %>  

<!--#include virtual="TopNew.htm"-->
<script language=javascript>
function ValidateNumeric()
{
	if ((event.keyCode < 48)||(event.keyCode > 57))
	{
		event.returnValue = false;
	}
}
function ValidateAlphabet()
{
	if((event.keyCode < 65)||(event.keycode >90 && event.keycode<97)||(event.keycode>122))
	{
		event.returnValue = false;
	}
}
</script>
<script type="text/javascript">
	function selectedChangeAvailableGroups() {
		if ($('#CheckBoxActive').is(':checked'))
		{ $('#ButtonAddGroup').attr("disabled", false); }
		else
		{ $('#ButtonAddGroup').attr("disabled", true); }
	}

	function selectedChangeMemberOf() {
		if ($('#CheckBoxActive').is(':checked') )
		{ $('#ButtonRemoveGroup').attr("disabled", false); }
		else
		{ $('#ButtonRemoveGroup').attr("disabled", true); }
	}

	$(document).ready(function () {
	    //AA:2012.09.26: BT-1153:  User Membership not appearing in sorted order -start
		$("#ListBoxAvailableGroups").change(function () { selectedChangeAvailableGroups(); $("#ListBoxAvailableGroups").attr("multiple", true); });
		$("#ListboxMemberOf").change(function () { selectedChangeMemberOf(); $("#ListboxMemberOf").attr("multiple", true); });
		$('#ButtonAddGroup').click(
                function (e) {
                    var li = $("#ListBoxAvailableGroups").prop("selectedIndex");
                    $('#ListBoxAvailableGroups > option:selected').appendTo('#ListboxMemberOf');
                    $("#ListBoxAvailableGroups").prop("selectedIndex", li);
                    $("#ListBoxAvailableGroups").attr('selected', 'selected');
                    $("#ListboxMemberOf").prop("selectedIndex", -1);
                    var sortedList = $.makeArray($("#ListboxMemberOf option"));
                    sortedList.sort(function (a, b) {
                        return $(a).text().toUpperCase() < $(b).text().toUpperCase() ? -1 : 1;
                    });
                    // Clear the options and add the sorted ones
                    $("#ListboxMemberOf").empty().html(sortedList);
                    //                 
                    $("#sortedList").val("");
                    $("#sortedList").val($("#ListboxMemberOf > option").map(function () { return this.value }).get());
                    e.preventDefault();
                    $('#ButtonSave').attr("disabled", false)
                    $('#ButtonCancel').attr("disabled", false)

                });
		$('#ButtonRemoveGroup').click(
                function (e) {
                    var li = $("#ListboxMemberOf").prop("selectedIndex");
                    $('#ListboxMemberOf > option:selected').appendTo('#ListBoxAvailableGroups');
                    $("#ListboxMemberOf").prop("selectedIndex", li);
                    $("#ListboxMemberOf").attr('selected', 'selected');
                    $("#ListBoxAvailableGroups").prop("selectedIndex", -1);
                    var sortedList = $.makeArray($("#ListBoxAvailableGroups option"));
                    sortedList.sort(function (a, b) {
                        return $(a).text().toUpperCase() < $(b).text().toUpperCase() ? -1 : 1;
                    });
                    // Clear the options and add the sorted ones
                    $("#ListBoxAvailableGroups").empty().html(sortedList);

                    $("#sortedList").val("");
                    $("#sortedList").val($("#ListboxMemberOf > option").map(function () { return this.value }).get());
                    e.preventDefault();
                    $('#ButtonSave').attr("disabled", false)
                    $('#ButtonCancel').attr("disabled", false)

                });
                //AA:2012.09.26: BT-1153:  User Membership not appearing in sorted order -End
});

</script>
<form id="Form1" method="post" runat="server">
<input type="hidden" name="sortedList" id="sortedList" />
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					User Properties<asp:label id="LabelUserDetails" runat="server"></asp:label>
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="690">
		<tr>
			<td><iewc:tabstrip id="TabStripUserProperties" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
					TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
					Width="690px" height="30px">
					<iewc:Tab Text=" Properties "></iewc:Tab>
					<iewc:Tab Text=" Permission Exceptions "></iewc:Tab>
				</iewc:tabstrip></td>
		</tr>
		<tr>
			<td><iewc:multipage id="MultiPageUserProperties" runat="server">
					<iewc:PageView>
						<table class="Table_WithOutBorder" width="680">
							<tr>
								<td align="right">
									<asp:label id="LabelFirstName" runat="server" cssClass="Label_Small">First Name</asp:label></td>
								<td colSpan="4" align="left">
									<asp:TextBox id="TextBoxFirstName" runat="server" cssClass="TextBox_Normal" maxlength=15></asp:TextBox>
									<asp:RequiredFieldValidator id="ReqFirstName" runat="server" ErrorMessage="*" ControlToValidate="TextBoxFirstName"></asp:RequiredFieldValidator>
								</td>
								<td align="left">
									<asp:CheckBox id="CheckBoxActive" runat="server" Text="Active" autopostback="true" cssClass="CheckBox_Normal"></asp:CheckBox></td>
							</tr>
							<tr>
								<td align="right">
									<asp:label id="LabelMiddleInitial" runat="server" cssClass="Label_Small">Middle Initial</asp:label></td>
								<td colspan="3" align="left">
									<asp:TextBox id="TextboxMiddleInitial" runat="server" width="30" MaxLength="1" cssClass="TextBox_Normal"></asp:TextBox></td>
								<td align="right">
									<asp:label id="LabelUsername" runat="server" cssClass="Label_Small">Username</asp:label></td>
								<td align="left">
									<asp:TextBox id="TextboxUserName" runat="server" width="90" cssClass="TextBox_Normal"></asp:TextBox>
									<asp:RequiredFieldValidator id="ReqUserName" runat="server" ErrorMessage="*" ControlToValidate="TextboxUserName"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:label id="LabelLastName" runat="server" cssClass="Label_Small">Last Name</asp:label></td>
								<td colspan="3" align="left">
									<asp:TextBox id="TextboxLastName" runat="server" width="180" maxlength=20 autopostback="true" cssClass="TextBox_Normal"></asp:TextBox>
								<asp:RequiredFieldValidator id="ReqLastName" runat="server" ErrorMessage="*" ControlToValidate="TextboxLastName"></asp:RequiredFieldValidator>
								</td>
								<td align="right">
									<asp:label id="LabelPassword" runat="server" cssClass="Label_Small">Password</asp:label></td>
								<td align="left">
									<asp:TextBox id="TextboxPassword" runat="server" width="110" cssClass="TextBox_Normal" TextMode="Password"
										maxlength="8"></asp:TextBox>
								<asp:RequiredFieldValidator id="ReqPassword" runat="server" ErrorMessage="*" ControlToValidate="TextboxPassword"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:label id="LabelPhone" runat="server" cssClass="Label_Small">Phone</asp:label></td>
								<td align="left">
									<asp:TextBox id="TextboxPhone" runat="server" width="110" cssClass="TextBox_Normal"  maxlength=12></asp:TextBox></td>
								<td align="right">
									<asp:TextBox id="TextboxExtn" runat="server" width="30" cssClass="TextBox_Normal" maxlength=4></asp:TextBox></td>
								<td align="left">
									<asp:label id="LabelExtn" runat="server" cssClass="Label_Small">Extn</asp:label></td>
								<td align="right">
									<asp:label id="LabelLastLogin" runat="server" cssClass="Label_Small">Last Login</asp:label></td>
								<td align="left">
									<asp:TextBox id="TextboxLastLogin" runat="server" width="140" readonly cssClass="TextBox_Normal"></asp:TextBox></td>
							</tr>
							<tr>
								<td align="right">
									<asp:label id="LabelFax" runat="server" cssClass="Label_Small">Fax</asp:label></td>
								<td colspan="5" align="left">
									<asp:TextBox id="TextboxFax" runat="server" width="110" cssClass="TextBox_Normal" maxlength=12></asp:TextBox></td>
							</tr>
						</table>
						<table class="Table_WithoutBorder" width="680">
						<tr>
						<td align=left>&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="LabelAvlGroups" runat="server" cssClass="Label_Small">Available Groups</asp:label></td>
						<td></td>
						<td align=left>&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="LabelMemof" runat="server" cssClass="Label_Small">Member Of</asp:label></td>
						</tr>
							<tr>
								<td>
                                <%-- commented by Anudeep for bt-1153 --%>
									<%--<asp:ListBox id="ListBoxAvailableGroups" runat="server" Width="230px" Height="120px"
					SelectionMode="Multiple"></asp:ListBox></td>--%>
                    <select id="ListBoxAvailableGroups" runat="server" multiple="true" size="10" style="width:230px; height:168px"> </select>
								<td><table>
										<tr>
											<td>
												<asp:Button id="ButtonAddGroup" runat="server" Text="Add>" Enabled="False" Width="110px" AutoPostBack="True"
													cssClass="Button_Normal" causesvalidation=false></asp:Button>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Button id="ButtonAddAllGroups" runat="server" Text="Add All >>" Width="110px" cssClass="Button_Normal" causesvalidation=false></asp:Button>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Button id="ButtonRemoveGroup" runat="server" Text="< Remove" Width="110px" Enabled="False"
													cssClass="Button_Normal" causesvalidation=false></asp:Button>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Button id="ButtonRemoveAllGroups" runat="server" Text="<< Remove All" Width="110px" cssClass="Button_Normal" causesvalidation=false></asp:Button>
											</td>
										</tr>
									</table>
								</td>
								<td>
                                <%-- commented by Anudeep for bt-1153 --%>
									<%--<asp:ListBox id="ListboxMemberOf" runat="server" Width="230px" Height="120px"
					SelectionMode="Multiple"></asp:ListBox></td>--%>
                     <select id="ListboxMemberOf" runat="server" multiple="true" size="10" style="width:230px; height:168px"></select>
							</tr>
						</table>
					</iewc:PageView>
					<iewc:PageView>
						<table>
							<tr>
								<td>
									<asp:Button id="ButtonAddItem" runat="server" Text="Add Item" Width="110px" cssClass="Button_Normal" causesvalidation=false></asp:Button></td>
								<td rowspan="2" align="left">
                                    <%--START | ML | 2019.08.19 |YRS-AT-4546 - Change Label height to display whole paragraph--%>
                                    <asp:Label id="LabelDesc" runat="server" Width="504px" Height="60px" align="left" cssClass="Label_Small">
									<%--<asp:Label id="LabelDesc" runat="server" Width="504px" Height="33px" align="left" cssClass="Label_Small">--%>
                                        <%--END | ML | 2019.08.19 |YRS-AT-4546 - Change Label height to display whole paragraph--%>
                                        Specify the Access Permission for specific Secured Items, which will
				override any permissions granted based on the groups to which this user belongs.Note that any 
				changes made here are not reflected until the next time this user logs into the system.</asp:Label></td>
							</tr>
							<tr>
								<td>
									<asp:Button id="ButtonDeleteItem" runat="server" Text="Delete Item" Width="110px" cssClass="Button_Normal" causesvalidation=false enabled=false></asp:Button></td>
							</tr>
							<tr>
								<td colspan="2">
									<div style="OVERFLOW: auto; WIDTH: 670px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none">
										<y:YmcaDataGrid id="DataGridUserProps" runat="server" Width="655px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
											<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
											<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
											<Columns>
												<asp:TemplateColumn>
													<ItemTemplate>
														<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
															CommandName="Select" ToolTip="Select"></asp:ImageButton>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn Visible="False" HeaderText="Secured Item Code">
													<ItemTemplate>
														<asp:Label runat="server" Width="141px" ID="lblItemCode" Text='<%#Container.DataItem("Secured Item Code")%>'>
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Secured Item">
													<ItemTemplate>
														<asp:Label id="lblItemDesc" runat="server" Width="141px" Text='<%#Container.DataItem("Secured Item")%>'>
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Type">
													<ItemTemplate>
														<asp:Label id="lblType" runat="server" Width="141px" Text='<%#Container.DataItem("Type")%>'>
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Access">
													<ItemTemplate>
														<asp:Label id="lblAccess" runat="server" Visible="False" Text='<%#Container.DataItem("Access")%>'>
														</asp:Label>
														<asp:DropDownList id="DrpAccess" runat="server" AutoPostBack="true" cssClass="DropDown_Normal" Width="141px"
															OnSelectedIndexChanged="enableSave"></asp:DropDownList>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</y:YmcaDataGrid>
									</div>
								</td>
							</tr>
						</table>
					</iewc:PageView>
				</iewc:multipage></td>
		</tr>
		<tr>
			<td>
				<table>
					<tr>
						<td colspan="5">
							<table width="150">
								<tr>
									<td>&nbsp;</td>
								</tr>
							</table>
						</td>
						<td><asp:button id="ButtonPrint" runat="server" Text="Print.." width="60" cssClass="Button_Normal" enabled=false causesvalidation=false></asp:button></td>
						<td><asp:button id="ButtonSave" runat="server" Text="Save" width="80" cssClass="Button_Normal"></asp:button></td>
						<td><asp:button id="ButtonCancel" runat="server" Text="Cancel" width="80" cssClass="Button_Normal" causesvalidation=false></asp:button></td>
						<td><asp:button id="ButtonDelete" runat="server" Text="Delete" width="90" cssClass="Button_Normal" causesvalidation=false></asp:button></td>
						<td><asp:button id="ButtonAdd" runat="server" Text="Add" width="90" cssClass="Button_Normal" causesvalidation=false></asp:button></td>
						<td><asp:button id="ButtonOk" runat="server" Text="OK" width="90" cssClass="Button_Normal" causesvalidation=false></asp:button></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<asp:PlaceHolder id="PlaceHolderUserProps" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
