<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GroupMembers.aspx.vb" Inherits="YMCAUI.GroupMembers"%>

<!--#include virtual="TopNew.htm"-->
<script type="text/javascript">
//AA:2012.08.13: BT-1057:YRS 5.0 1630:  Group members not appearing in sorted order -start
    $(document).ready(function () {

        $("#ListBoxAllUsers").change(function () { $('#ButtonAddUser').attr("disabled", false); $("#ListBoxAllUsers").attr("multiple", true); });
        $("#ListboxGroupMembers").change(function () { $('#ButtonRemoveUser').attr("disabled", false); $("#ListboxGroupMembers").attr("multiple", true); });
        $("#sortedList").val("");
        $('#ButtonAddUser').click(
                function (e) {
                    var li = $("#ListBoxAllUsers").prop("selectedIndex");
                    $('#ListBoxAllUsers > option:selected').appendTo('#ListboxGroupMembers');
                    $("#ListBoxAllUsers").prop("selectedIndex", li);
                    $("#ListBoxAllUsers").attr('selected', 'selected');
                    $("#ListboxGroupMembers").prop("selectedIndex", -1);
                    var sortedList = $.makeArray($("#ListboxGroupMembers option"));
                    sortedList.sort(function (a, b) {
                        return $(a).text().toUpperCase() < $(b).text().toUpperCase() ? -1 : 1;
                    });
                    // Clear the options and add the sorted ones
                    $("#ListboxGroupMembers").empty().html(sortedList);
                    //                 
                    $("#sortedList").val("");  
                    $("#sortedList").val($("#ListboxGroupMembers > option").map(function () { return this.value }).get());
                    e.preventDefault();
                    $('#ButtonSave').attr("disabled", false)
                    $('#ButtonCancel').attr("disabled", false)

                });
        $('#ButtonRemoveUser').click(
                function (e) {
                    var li = $("#ListboxGroupMembers").prop("selectedIndex");
                    $('#ListboxGroupMembers > option:selected').appendTo('#ListBoxAllUsers');
                    $("#ListboxGroupMembers").prop("selectedIndex", li);
                    $("#ListboxGroupMembers").attr('selected', 'selected');
                    $("#ListBoxAllUsers").prop("selectedIndex", -1);
                    var sortedList = $.makeArray($("#ListBoxAllUsers option"));
                    sortedList.sort(function (a, b) {
                        return $(a).text().toUpperCase() < $(b).text().toUpperCase() ? -1 : 1;
                    });
                    // Clear the options and add the sorted ones
                    $("#ListBoxAllUsers").empty().html(sortedList);
                    
                    $("#sortedList").val("");
                    $("#sortedList").val($("#ListboxGroupMembers > option").map(function () { return this.value }).get());
                    e.preventDefault();
                    $('#ButtonSave').attr("disabled", false)
                    $('#ButtonCancel').attr("disabled", false)

                });

    });



//End, AA:2012.08.13-BT-1057:YRS 5.0 1630:  Group members not appearing in sorted order


</script>
<form id="Form1" method="post" runat="server">
<input type="hidden" name="sortedList" id="sortedList" />
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Group Members
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
				<asp:Label id="LabelAllUsers" runat="server" Width="152px" cssClass="Label_Medium">All Users</asp:Label></td>
			<td></td>
			<td>
				<asp:Label id="LabelGroupMembers" runat="server" Width="160px" cssClass="Label_Medium">Group Members</asp:Label></td>
		</tr>
		<tr>
			<td>
            <!--AA:2012.08.13: YRS 5.0 1630:  Group members not appearing in sorted order -->
			                            <select id="ListBoxAllUsers" runat="server" multiple="true" size="10" style="width:230px; height:168px"></select>
           <!-- AA:2012.08.13: YRS 5.0 1630:  Group members not appearing in sorted order -->
             </td>
			<td><table>
					<tr>
						<td>
							<asp:Button id="ButtonAddUser" runat="server" Text="Add >" Width="110px" Enabled="False" cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Button id="ButtonAddAllUsers" runat="server" Text="Add All >>" Width="110px" cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							<asp:Button id="ButtonRemoveUser" runat="server" Text="< Remove" Width="110px" Enabled="False" cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Button id="ButtonRemoveAllUsers" runat="server" Text="<< Remove All" Width="110px" cssClass="Button_Normal"></asp:Button>
						</td>
					</tr>
				</table>
			</td>
			<td>
                 <!--AA:2012.08.13: YRS 5.0 1630:  Group members not appearing in sorted order -->
                   <select id="ListboxGroupMembers" multiple="true" runat="server" size="10" style="width:230px; height:168px">
                   </select>
                <!-- End, AA:2012.08.13: YRS 5.0 1630:  Group members not appearing in sorted order -->
            </td>
		</tr>
		<tr>
			<td colspan="3">&nbsp;</td>
		</tr>
		<tr>
			<td align="right" colspan="2">
				<asp:Button id="ButtonOk" runat="server" Width="94px" Text="OK" cssClass="Button_Normal"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
			<td><asp:Button id="ButtonSave" runat="server" Width="94px" Text="Save" Enabled="False" cssClass="Button_Normal"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Button id="ButtonCancel" runat="server" Width="94px" Text="Cancel" Enabled="False" cssClass="Button_Normal"></asp:Button>
			</td>
		</tr>
	</table>
  	<asp:PlaceHolder id="PlaceHolderUserMembers" runat="server"></asp:PlaceHolder>	
</form>
<!--#include virtual="bottom.html"-->