<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="DCToolsEditRemainingDeathBenefit.aspx.vb" Inherits="YMCAUI.DCToolsEditRemainingDeathBenefit" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>

<asp:Content ID="contentEditDeathBenefitHead" ContentPlaceHolderID="head" runat="server">
    <title>EditRemainingDeathBenefit</title>
    <script type="text/javascript" src="JS/YMCA_JScript.js"></script>

    <style type="text/css">
        .alignright {
            text-align:right;
        }
    </style>
</asp:Content>

<asp:Content ID="contentEditRemainingDeathBenefit" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Div_Center" style="width: 100%;">
        <asp:ScriptManagerProxy ID="EditRemainingDeathBenefitScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplEditRemainingDeathBenefit" runat="server">
            <ContentTemplate>
                <table class="td_withoutborder" style="width: 100%; vertical-align: top;">
                    <tr>
                        <td id="tdEditRDBAmount" runat="server" class="ActiveTab" style="width: 25%;">1. Remaining Death Benefit</td>
                        <td id="tdReviewAndSubmit" runat="server" class="InActiveDisabledTab" style="width: 20%;">2. Review & Submit</td>
                        <td style="width: 55%;" Align="right">
                           <div id="divRequiredFields" runat="server"> <span class="aestrik">*</span><span class="Label_Small">Required Fields</span></div>
                        </td>
                    </tr>
                    <tr style="height: 2px;">
                        <td colspan="3"></td>
                    </tr>
                </table>
                <table class="Table_WithBorder" style="width: 100%; height:550px;">
                    <tr style="height: 100%;">
                        <td style="vertical-align: top;">
                            <div id="divEditRemainingDeathBenefit" style="width: 100%;display:block;" runat="server">
                                <table style="width: 100%; height: 100%;">
                                    <%--Annuities Information section--%>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align:left;height:15pt" class="td_Text" >
                                            <asp:Label ID="lblAnnuitiesInformaion" runat="server">Select Annuity<span class="aestrik">*</span></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align:left;"> <br />
                                            <div style="overflow: auto; width: 78%; height: 140px; text-align: left">
                                                <asp:GridView ID="gvAnnuitiesList" AllowPaging="true" AllowSorting="true"  
                                                    runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle  CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="2%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
                                                          CommandName  ="Select" ToolTip="Select Row"></asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:BoundField HeaderText="Annuity Source" DataField="AnnuitySourceCode" HeaderStyle-Width="8%"/>
                                                    <asp:BoundField HeaderText="Plan Type" DataField="PlanType" HeaderStyle-Width="7%"  />
                                                    <asp:BoundField HeaderText="Purchase Date" DataField="PurchaseDate" HeaderStyle-Width="8%" />
                                                    <asp:BoundField HeaderText="Current Payment" DataField="CurrentPayment" ItemStyle-CssClass="alignright" HeaderStyle-Width="9%"/>
                                                    <asp:BoundField HeaderText="Death Benefit" DataField="DeathBenefit" ItemStyle-CssClass="alignright" HeaderStyle-Width="8%" />
                                                    <asp:BoundField HeaderText="Remaining Death Benefit" DataField="DeathBenefitRemaining" ItemStyle-CssClass="alignright" HeaderStyle-Width="13%"/>
                                                    <asp:BoundField HeaderText="AnnuityId" DataField="AnnuityId"  ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" HeaderStyle-Width="55%"/>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="pagination"  />
                                                </asp:GridView>
                                             </div>
                                            
                                        </td>
                                    </tr>
                                    <tr style="vertical-align:bottom">
                                        <td>
                                            <div id="divLegendInformation" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                              <label id="lblLegendNote" class="Label_Small">Note :- </label>
                                                        </td>
                                                        <td>
                                                              <label id="lblLegendNote1" class="Label_Small"> The above list only displays annuities purchased from retirement plan. </label>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                             
                                                        </td>
                                                        <td>
                                                              <label id="lblLegendNote2" class="Label_Small"> The current payment displayed in the above list against individual annuities is combined payment of annuity purchased from retirement plan and annuity purchased from death benefit (if any used). </label>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divEditAmount" runat="server" visible="false">
                                                <table style="width:100%;">
                                                     <%--Edit Remaining Death Benefit section--%>
                                                     <tr style="vertical-align: top;" >
                                                         <td style="text-align:left;height:15pt" class="td_Text">
                                                             <asp:Label ID="Label1" runat="server">Edit Remaining Death Benefit</asp:Label>
                                                         </td>
                                                    </tr>
                                                    <tr style="vertical-align: top;">
                                                        <td style="text-align:left;">
                                                           
                                                           <table style="width:100%; height:100%;text-align:left;" cellspacing="4" cellpadding="4" border="0">
                                                                    <tr style="vertical-align: top;">
                                                                        <td style="text-align:left;width:33%;">
                                                                            <label id="lblOldAmount" class="Label_Small">Current Remaining Death Benefit Amount</label>
                                                                        </td>
                                                                        <td style="text-align: left;width:67%;">
                                                                            <asp:label id="lblDispOldAmount" class="Label_Small" runat="server"></asp:label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="vertical-align: top;">
                                                                        <td style="text-align:left;width:33%;">
                                                                            <label id="lblNewAmount" class="Label_Small">Enter New Remaining Death Benefit Amount<span class="aestrik">*</span></label>
                                                                        </td>
                                                                        <td style="text-align: left;width:67%;">
                                                                            <asp:TextBox ID="txtNewAmount" runat="server" cssclass="TextBox_Normal Warn" Width="80px" style="text-align:right;" onkeypress="Javascript:return HandleAmountFiltering(this);" onchange="javascript:FormatAmtControl(this);"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="vertical-align: top;">
                                                                        <td style="text-align:left;width:33%;">
                                                                            <label id="lblTransactDate" class="Label_Small">Transact Date<span class="aestrik">*</span></label>
                                                                        </td>
                                                                        <td style="text-align: left;width:67%;">
                                                                            <YRSCustomControls:CalenderTextBox ID="txtTransactDate" runat="server" Width="80px" cssclass="TextBox_Normal Warn"/>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="vertical-align: top;">
                                                                        <td style="text-align:left;width:33%;">
                                                                            <label id="lblNotes" class="Label_Small">Notes<span class="aestrik">*</span></label>
                                                                        </td>
                                                                        <td style="text-align: left;width:67%;">
                                                                            <asp:TextBox ID="txtNotes" runat="server" Width="350px" TextMode="MultiLine" Height="60px" cssclass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                            </table>
                                        
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                 
                                </table>
                            </div>
                            <div id="divReviewAndSubmit" style="width: 100%; display:none;" runat="server">
                                <table style="width: 100%; height: 100%;">
                                    <tr style="vertical-align: top;">
                                        <td style="text-align:left;" class="td_Text">
                                            <asp:Label ID="lblReviewHeaderText" runat="server">Review - Updated Remaining Death Benefit Details</asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align:left;">
                                            <table style="width:100%; height:100%;text-align:left;" cellspacing="2" cellpadding="6">
                                                    <tr style="vertical-align: top;">
                                                        <td style="text-align:left;width:33%;">
                                                            <asp:Label ID="lblReviewOldAmount" runat="server" class="Label_Small">Old Remaining Death Benefit Amount</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;width:67%;">
                                                            <asp:Label ID="lblReviewDispOldAmount" runat="server" class="Label_Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="vertical-align: top;">
                                                        <td style="text-align:left;width:33%;">
                                                            <asp:Label ID="lblReviewNewAmount" runat="server" class="Label_Small">New Remaining Death Benefit Amount</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;width:67%;">
                                                            <asp:Label ID="lblReviewDispNewAmount" runat="server" class="Label_Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr style="vertical-align: top;">
                                                        <td style="text-align:left;width:33%;">
                                                            <asp:Label ID="lblReviewTransactDate" runat="server" class="Label_Small">Transact Date</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;width:67%;">
                                                            <asp:Label ID="lblReviewDispTransactDate" runat="server" class="Label_Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="vertical-align: top;">
                                                        <td style="text-align:left;width:33%;">
                                                            <asp:Label ID="lblReviewNotes" runat="server" class="Label_Small">Notes</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;width:67%;">
                                                            <asp:Label ID="lblReviewDispNotes" runat="server" class="Label_Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                     <tr>
                                         <td style="padding-left:5px;" align="left" >
                                         <asp:label id="lblTransactionEntry" runat="server"  class="Label_Small" > New entry will be added in Transact table : 
                                                     </asp:label>
                                          </td>
                                     </tr>
                                     <tr>
                                              <td style="padding-left:5px;">
                                                   <div style="overflow: auto; width: 35%; height: 140px; text-align: left">
                                                <asp:GridView ID="gvTransactView" AllowSorting="true"  
                                                    runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle  CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Transact Date" DataField="Transact Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                    <asp:BoundField HeaderText="Transact Type" DataField="Transact Type" />
                                                    <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-CssClass="alignright" />
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="pagination"  />
                                                </asp:GridView>
                                             </div>
                                              </td>
                                      </tr>
                                </table>
                            </div>
                    

                        </td>
                    </tr>
                </table>
                <table style="width:100%; text-align:center;">
                    <tr>
                        <td class="Td_ButtonContainer" style="width: 33%"  text-align: center;">
                            <asp:Button ID="btnPrevious" runat="server" class="Button_Normal" Text="Previous" style="display:none; width:30%;"/>
                        </td>
                        <td class="Td_ButtonContainer" style="width: 33%; text-align: center;">
                            <asp:Button ID="btnNext" runat="server" class="Button_Normal" Text="Next" style="width:30%;" Enabled="false" />
                            <asp:Button ID="btnSubmit" runat="server" class="Button_Normal" Text="Submit" style="width:30%;display:none;" />
                        </td>
                        <td class="Td_ButtonContainer" style="width: 34%; text-align: center;">
                            <asp:Button ID="btnClose" runat="server" class="Button_Normal Warn_Dirty" Text="Close" style="width:30%;"/>
                        </td>
                    </tr>
                </table>
                <div id="ConfirmDialog" title="Remaining Death Benefit" style="display: none;">
                    <div>
                        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align:left;">
                            <tr>
                                <td rowspan="2" style="width:10%;">
                                    <img src="images/help48.JPG" style="border-width:0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessage">
                                       Are you sure, you want to submit?
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                             </tr>
                             <tr id="trConfirmDialogYesNo">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button runat="server" ID="btnConfirmDialogYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;"  OnClientClick="closeDialog('ConfirmDialog');"/>&nbsp;
                                    <input type="button" ID="btnConfirmDialogNo" value="No" class="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" onclick="closeDialog('ConfirmDialog');" />
                                </td>
                            </tr>
                            </table>
                    </div>
                </div>

                <script type="text/javascript">

                    function BindEvents() {
                        $('#ConfirmDialog').dialog({
                            autoOpen: false,
                            draggable: true,
                            close: false,
                            modal: true,
                            width: 450, maxHeight: 420,
                            height: 260,
                            title: "Edit Remaining Death Benefit Amount ",
                            open: function (type, data) {
                                $(this).parent().appendTo("form");
                                $('a.ui-dialog-titlebar-close').remove();
                            }
                        });
                    }
                        function ShowDialog() {
                            $('#ConfirmDialog').dialog("open");
                            return false;
                        }

                        function closeDialog(id) {
                            $('#' + id).dialog('close');
                        }

                  function FormatAmtControl(ctl) {
                        var vMask;
                        var vDecimalAfterPeriod;
                        var ctlVal;
                        var iPeriodPos;
                        var sTemp;
                        var iMaxLen
                        var ctlVal;
                        var tempVal;
                        ctlVal = ctl.value;
                        vDecimalAfterPeriod = 2
                        iMaxLen = ctl.maxLength;

                        if (isNaN(ctlVal)) {
                           
                        }
                        else {
                            ctlVal = ctl.value;
                            iPeriodPos = ctlVal.indexOf(".");
                            if (iPeriodPos < 0) {
                                if (ctl.value.length > (iMaxLen - 3)) {
                                    sTemp = ctl.value
                                    tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                                }
                                else {
                                    if (ctlVal == "")
                                        ctlVal = "0";
                                    tempVal = ctlVal + ".00"
                                }
                            }
                            else {
                                if ((ctlVal.length - iPeriodPos - 1) == 1)
                                    tempVal = ctlVal + "0"
                                if ((ctlVal.length - iPeriodPos - 1) == 0)
                                    tempVal = ctlVal + "00"
                                if ((ctlVal.length - iPeriodPos - 1) == 2)
                                    tempVal = ctlVal;
                                if ((ctlVal.length - iPeriodPos - 1) > 2) {
                                    tempVal = ctlVal.substring(0, iPeriodPos + 3);
                                }

                            }
                            ctl.value = tempVal;
                        }
                    }

                    /*
                    This function is responsible for filtering the keys pressed and the maintain the amount format of the 
                    value in the Text box
                    */
                    function HandleAmountFiltering(ctl) {
                        var iKeyCode, objInput;
                        var iMaxLen
                        var reValidChars = /[0-9.-]/;
                        var strKey;
                        var sValue;
                        var event = window.event || arguments.callee.caller.arguments[0];
                        iMaxLen = ctl.maxLength;
                        sValue = ctl.value;
                        detectBrowser();

                        if (isIE) {
                            iKeyCode = event.keyCode;
                            objInput = event.srcElement;
                        } else {
                            iKeyCode = event.which;
                            objInput = event.target;
                        }

                        strKey = String.fromCharCode(iKeyCode);

                        if (reValidChars.test(strKey)) {
                            if (iKeyCode == 46) {
                                if (objInput.value.indexOf('.') != -1)
                                    if (isIE)
                                        event.keyCode = 0;
                                    else {
                                        if (event.which != 0 && event.which != 8)
                                            return false;
                                    }
                            }
                            else {
                                if (objInput.value.indexOf('.') == -1) {

                                    if (objInput.value.length >= (iMaxLen - 3)) {
                                        if (isIE)
                                            event.keyCode = 0;
                                        else {
                                            if (event.which != 0 && event.which != 8)
                                                return false;
                                        }

                                    }
                                }
                                if ((objInput.value.length == (iMaxLen - 3)) && (objInput.value.indexOf('.') == -1)) {
                                    objInput.value = objInput.value + '.';

                                }


                            }

                        }
                        else {
                            if (isIE)
                                event.keyCode = 0;
                            else {
                                if (event.which != 0 && event.which != 8)
                                    return false;
                            }
                        }

                    }

                    function EnableDirty() {
                        $('#HiddenFieldDirty').val('true');
                    }

                    function ClearDirty() {
                        $('#HiddenFieldDirty').val('false');
                    }
                </script>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPrevious" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNext" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" /> 
                <asp:AsyncPostBackTrigger ControlID="btnConfirmDialogYes" EventName="Click" />
            </Triggers>
    </asp:UpdatePanel>
</div>
</asp:Content>
