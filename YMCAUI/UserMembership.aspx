<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserMembership.aspx.vb" Inherits="YMCAUI.UserMembership"%>
<!--#include virtual="TopNew.htm"-->
<script type="text/javascript">
    //AA:2012.09.26: BT-1153:  User Membership not appearing in sorted order -start
	$(document).ready(function () {

		$("#ListBoxAvailableGroups").change(function () { $('#ButtonAddGroup').attr("disabled", false); $("#ListBoxAvailableGroups").attr("multiple", true); });
		$("#ListboxMemberOf").change(function () { $('#ButtonRemoveGroup').attr("disabled", false); $("#ListboxMemberOf").attr("multiple", true); });
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
					Group To Which User Belongs
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="700">
		<tr>
			<td>
				<asp:Label id="LabelAvailableGroups" runat="server" Width="152px" cssClass="Label_Medium">Available Groups</asp:Label></td>
			<td></td>
			<td>
				<asp:Label id="LabelMemberOf" runat="server" Width="160px" cssClass="Label_Medium">Member Of</asp:Label></td>
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
							<asp:Button id="ButtonAddGroup" runat="server" Text="Add >" Width="110px" Enabled="False" cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Button id="ButtonAddAllGroups" runat="server" Text="Add All >>" Width="110px" cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							<asp:Button id="ButtonRemoveGroup" runat="server" Text="< Remove" Width="110px" Enabled="False"
								cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Button id="ButtonRemoveAllGroups" runat="server" Text="<< Remove All" Width="110px" cssClass="Button_Normal"></asp:Button>
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
		<tr>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td align="right" colspan="2">
				<asp:Button id="ButtonOk" runat="server" Width="94px" Text="OK" cssClass="Button_Normal"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
			<td><asp:Button id="ButtonSave" runat="server" Width="94px" Text="Save" Enabled="False" cssClass="Button_Normal"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Button id="ButtonCancel" runat="server" Width="94px" Text="Cancel" Enabled="False" cssClass="Button_Normal"></asp:Button>
			</td>
		</tr>
	</table>
	<asp:PlaceHolder id="PlaceHolderUserMembership" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
