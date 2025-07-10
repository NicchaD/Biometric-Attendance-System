<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master"
    CodeBehind="FindInfo.aspx.vb" Inherits="YMCAUI.FindInfo" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function _OnBlur_TextBox() {
            document.Form1.all.ButtonFind.focus();
        }
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    InitializePINDialogBox();
                }
            }
            InitializePINDialogBox();
        });

        function InitializePINDialogBox() {
            $("#dvPINVerify").dialog
					({
					    modal: true,
					    open: function (type, data) {
					                                    $(this).parent().appendTo("form");
                                                     $('a.ui-dialog-titlebar-close').remove(); },
					    autoOpen: false,
					    resizable: false,
					    draggable: false,
					    closeOnEscape: false,
					    width: 380, height: 200
					});
        }
		function VerifyPIN(firstname, lastname, SSN, PIN) {
		    $("#lblFirstName").text(firstname);
		    $("#lblLastName").text(lastname);
		    $("#lblSSN").text(SSN);
		    $("#lblPIN").text(PIN);
            $("#dvPINVerify").dialog('open');
        }
        function ClosePINdialog() {
            $("#dvPINVerify").dialog('close');
        }

        <%--Start - Manthan Rajguru:2016.01.12 - YRS 2485:Function to dispaly and hide tooltip --%>
        function showToolTip(tooltip) {
            var divId, linkId;
            divId = "<%=Tooltip.ClientID%>";
            linkId = "lblComments";
            
            if (null != divId) {
                var elementRef = document.getElementById(divId);
                if (elementRef != null) {
                    elementRef.style.position = 'absolute';
                    elementRef.style.left = event.clientX - 373 + document.body.scrollLeft;
                    elementRef.style.top = event.clientY + 0 + document.body.scrollTop;
                    elementRef.style.width = '380';
                    elementRef.style.visibility = 'visible';
                    elementRef.style.display = 'block';
                }

                if (null != linkId) {


                    var lblNote = document.getElementById("<%=lblComments.ClientID%>");
                    lblNote.innerText = ' ' + tooltip;

					    }
                    }
                }

                //to hide tool tip when mouse is removed
        function hideToolTip() {
            var divId, linkId;
            divId = "<%=Tooltip.ClientID%>";
            linkId = "lblComments";
                    if (null != divId) {
                        var elementRef = document.getElementById(divId);
                        if (elementRef != null) {
                            elementRef.style.visibility = 'hidden';
                        }
                    }
                    if (null != linkId) {
                        var elementBak = document.getElementById(linkId);
                        if (elementBak != null) {

                        }
                    }
        }
        <%--End - Manthan Rajguru:2016.01.12 - YRS 2485:Function to dispaly and hide tooltip --%>
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<asp:ScriptManagerProxy   id="dbScriptManagerProxy" runat="server"> 
</asp:ScriptManagerProxy> 
    <div class="Div_Center" style="width:100%;height:550">
        <table class="Table_WithBorder" width="100%" style="height:550px">
            <tr style="height:100%;">
                <td align="center">
                <asp:UpdatePanel ID="upRetireeGrid" runat="server" >
                    <ContentTemplate>     
                        <table width="100%" style="height:520px" >
                            <tr>
                                <td align="left" width="60%" valign="top">
                                    <asp:Label ID="LabelNoDataFound" runat="server" Visible="False" CssClass="Label_Small">No Matching Records</asp:Label>
                                    <div style="overflow: auto; width: 100%;height:100%; border-top-style: none; border-right-style: none;
                                        border-left-style: none; position: static; border-bottom-style: none">
                                        <asp:GridView ID="gvFindInfo" AllowPaging="true" AllowSorting="true" PageSize="25" 
                                        runat="server" CssClass="DataGrid_Grid" Width="100%">
                                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                            <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                            <RowStyle CssClass="DataGrid_NormalStyle" />
                                            <SortedAscendingHeaderStyle  CssClass="sortasc" />
                                            <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                            <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Select" ImageUrl="images\select.gif" 
                                                        CausesValidation="false" />
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="pagination"  />
                                        </asp:GridView>
                                    
                                    </div>
                                </td>
                                <td align="right" valign="top" width="35%">
                                    <table class="Table_WithOutBorder" width="100%" align="right">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelFundNo" CssClass="Label_Small" Width="100" runat="server">Fund No.</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="TextboxFundNo" runat="server" CssClass="TextBox_Normal" Width="180"
                                                    MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                                <td align="left" height="24">
                                                    <asp:Label ID="LabelSSNo" CssClass="Label_Small" Width="100" runat="server">SS No.</asp:Label>
                                                </td>
                                                <td align="center" height="26">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="TextBoxSSNo" runat="server" CssClass="TextBox_Normal" Width="180"
                                                        MaxLength="11"></asp:TextBox> 
                                                    <asp:Label ID="lblSearchSSN" CssClass="Label_Small" Width="10" runat="server"> <%--SR:2016.07.04 - YRS 2484 | Added label to hide image--%>
                                                    <img id="imgSearchSSN" src="images/icon-info.jpg"  style="height:17px;width:17px"  onmouseover="showToolTip('To search by last digits only, enter an asterisk first. Example: *1234 or *12');" onmouseout="hideToolTip();"  /> <%--Manthan Rajguru:2016.01.12 - YRS 2485:Image control to display information --%>
                                                    </asp:Label>                                                  
                                                </td>
                                            </tr>                                        
                                </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelLastName" CssClass="Label_Small" Width="100" runat="server">Last Name</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBox_Normal" Width="180"
                                                    MaxLength="30"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelFirstName" CssClass="Label_Small" Width="100" runat="server">First Name</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBox_Normal" Width="180"
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelCity" runat="server" CssClass="Label_Small" Width="100">City</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="TextBoxCity" runat="server" CssClass="TextBox_Normal" Width="180"
                                                    MaxLength="29"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelState" runat="server" CssClass="Label_Small" Width="100">State</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="TextBoxState" runat="server" CssClass="TextBox_Normal" Width="180"
                                                    MaxLength="29"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblPhone" runat="server" CssClass="Label_Small" Width="100">Phone</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtPhone" runat="server" CssClass="TextBox_Normal" Width="180" MaxLength="29"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblEmail" runat="server" CssClass="Label_Small" Width="100">Email</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBox_Normal" Width="180" MaxLength="29"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="ButtonFind" runat="server" CssClass="Button_Normal" Width="80" Text="Find">
                                                </asp:Button>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="ButtonClear" runat="server" CssClass="Button_Normal" Width="80" Text="Clear">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                        <!-- START | SR | 2016.06.28 | YRS-AT-2484 | Add checkbox to get participant whose fundstatus changed during Year-->
                                        <tr>
                                            <td align="right" colspan ="2">                                                
                                                <br/><br/><asp:LinkButton ID="lnkFundStatusChanged" runat="server">List All RMD Eligible Terminated Persons</asp:LinkButton><br />
                                            </td>                                            
                                        </tr>
                                        <!-- END | SR | 2016.06.28 | YRS-AT-2484 | Add checkbox to get participant whose fundstatus changed during Year-->
                                    </table>
                                </td>
                            </tr>
                        </table>
                            <%--Start - Manthan Rajguru:2016.01.12 - YRS 2485:Div and label control to display information --%>
                         <div id="Tooltip" clientidmode="Static" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver;
                        border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc;
                        padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black;
                        display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana;
                        margin: 0; overflow: visible;">
                        <asp:Label runat="server" ID="lblComments" ClientIDMode="Static" Style="display: block; width: auto; overflow: visible;
                            font-size: x-small;"></asp:Label>
                    </div>
                            <%--End - Manthan Rajguru:2016.01.12 - YRS 2485:Div and label control to display information --%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table width="100%" align="center">
                        <tr>
                            <td class="Td_ButtonContainer" align="right" colspan="3">
                                <asp:Button ID="ButtonCancel" runat="server" CssClass="Button_Normal" Width="80"
                                    Text="Close"></asp:Button>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div id="dvPINVerify" title="PIN Verification" style="display:none;" >
    <asp:UpdatePanel ID="updatepanel2" runat="server"  >
        <ContentTemplate>
                <div>
                    <table>
                        <tr>
                            <td class="Label_Small" align="left" width="100px">
                                First Name
                            </td>
                            <td class="Label_Small" align="left" width="30px">
                                :
                            </td>
                            <td align="left">
                                <label id="lblFirstName" class="Label_Small"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small" align="left">
                                Last Name
                            </td>
                            <td class="Label_Small" align="left">
                                :
                            </td>
                            <td align="left">
                                <label id="lblLastName" class="Label_Small"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small" align="left">
                                SSN
                            </td>
                            <td class="Label_Small" align="left">
                                :
                            </td>
                            <td align="left">
                                <label id="lblSSN" class="Label_Small"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small" align="left">
                                PIN
                            </td>
                            <td class="Label_Small" align="left">
                                :
                            </td>
                            <td align="left">
                                <label id="lblPIN" class="Label_Small"></label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small" align="left">
                                Did participant confirm the PIN?
                            </td>
                        </tr>
                        <tr>
                            <td class="NormalMessageText" style="color:Red;" align="left">
                                Note: If you are not on a call with participant, please click on "Cancel" button to proceed.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">
                                <asp:Button runat="server" OnClientClick="ClosePINdialog();" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;"  />&nbsp;
                                <asp:Button runat="server" OnClientClick="ClosePINdialog();" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />&nbsp;
                                <asp:Button runat="server" OnClientClick="ClosePINdialog();" ID="btnCancel" Text="Cancel" CssClass="Button_Normal"
                                    Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt;
                                    font-weight: bold; height: 16pt;" />
                            </td>
                        </tr>
                    </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   </div>      

    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</asp:Content>
