
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DCToolsReverseAnnuity.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master"  Inherits="YMCAUI.DCToolsReverseAnnuity" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>



<asp:Content ID="contentReverseAnnuityHead" ContentPlaceHolderID="head" runat="server">
    <title>Reverse Annuity</title>
    <script type="text/javascript" src="JS/YMCA_JScript.js"></script>
</asp:Content>

<asp:Content ID="contentReverseAnnuity" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Div_Center" style="width: 100%;">
        <asp:ScriptManagerProxy ID="ReverseAnnuityScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplReverseAnnuity" runat="server">
            <ContentTemplate>        
        <table class="td_withoutborder" cellpadding="0" cellspacing="0" style="width: 100%; vertical-align: top;" border ="0" >           
            <tr>
                <td id="tdReverseAnnuity" class="ActiveTab" style="width: 20%;" runat ="server" >1. Reverse Annuity</td>
                <td id="tdReviewAnnuity" class="InActiveDisabledTab" style="width: 20%;" runat="server" >2. Review & Submit</td>
                <td style="width: 60%;" Align="right">
                    <div id="divRequiredFields" runat ="server">                                               
                        <span class="aestrik">*</span><span class="Label_Small">Required Fields</span>                                                                                                         
                     </div>
                </td> 
            </tr>
            <tr style="height: 2px;">
                <td colspan="3"></td>
            </tr>
        </table>

        <table class="Table_WithBorder" width="100%" style="height: 550px" border ="0">
            <tr style="height: 100%;">
                <td style="vertical-align: top;">
                    <div id="TAB1" runat ="server" >
                         <table width="100%" style="height: auto" border ="0">                             
                             <tr valign="top">
                                 <td class="Td_ButtonContainer" style="width: 100%; text-align: left;" valign="top">
                                   <asp:Label ID="Label1" class="Td_ButtonContainer" runat="server" Height="20" Text="Select Annuity" ></asp:Label><span class="aestrik" style="vertical-align :top">*</span>  
                                 </td> 
                             </tr>
                             <tr style="height: 200px">
                                  <td valign="top">
                                       <div style="overflow: auto; width: 100%; height: auto; text-align: left">
                                            <asp:GridView runat="server" ID="gvAnnuities" CssClass="DataGrid_Grid"
                                                AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top" 
                                                Width="100%" EmptyDataText="Participant does not have any existing Annuity.">
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="20px">                                                        
                                                        <ItemTemplate>
                                                             <asp:ImageButton id="imgbtn" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																				CommandName="Select" ToolTip="Select" EnableViewState="false"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>     
                                                    <asp:BoundField HeaderText="Current Payment" DataField="CurrentPayment" />    
                                                    <asp:BoundField HeaderText="Purchase Date" DataField="PurchaseDate" />                                           
                                                    <asp:BoundField HeaderText="Plan Type" DataField="PlanType" />                                                         
                                                    <asp:BoundField HeaderText="ID" DataField="DisbursementId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide"/>
                                                   
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                             </tr>

                             
                              <tr>
                                  <td>
                                      <div id="divFundStatus" style="overflow: auto; width: auto ; height: auto; text-align: left" runat ="server" valign="top">
                                      <table  width="100%" style="height: auto" border ="0">
                                          <tr>
                                               <td class="Td_ButtonContainer" style="width: 100%; text-align: left;" colspan ="2">FundStatus</td> 
                                          </tr>
                                          <tr>
                                              <td class="Label_Small" valign="top" style="width: 25%; text-align: left;">Recommended Fund Status</td> 
                                              <td class="Label_Small" valign="top" style="width: 75%; text-align: left;">
                                                  <asp:DropDownList ID="ddlFundStatus" runat="server" Width="200px" CssClass="DropDown_Normal Warn"></asp:DropDownList>
                                              </td> 
                                          </tr>
                                      </table> 
                                      </div> 
                                  </td>                                 
                             </tr>

                             <tr>
                                 
                                 <td class="Label_Small" valign="top">
                                     <div id="divNotes" style="overflow: auto; width: auto ; height: auto; text-align: left" runat ="server" valign="top">   
                                         <table width="100%" style="height: auto" border ="0">                             
                                            <tr valign="top">
                                                <td class="Td_ButtonContainer" style="width: 100%; text-align: left;" valign="top">
                                                        <asp:Label ID="lblNotes" class="Td_ButtonContainer" runat="server" Height="20" Text="Notes" ></asp:Label><span class="aestrik" style="vertical-align :top">*</span>  
                                                </td> 
                                            </tr>
                                             <tr valign="top">
                                                <td class="Label_Small" style="width: 100%; text-align: left;" valign="top">
                                                        <asp:TextBox ID="txtNotes" CssClass="TextBox_Normal Warn" runat="server" Width="100%" TextMode ="MultiLine" Height ="150px"></asp:TextBox>
                                                </td> 
                                            </tr>
                                         </table> 
                                     </div> 
                                 </td>
                             </tr>
                         </table>
                    </div>
                    <div id="TAB2" runat ="server" >
                         <table class="Table_WithBorder" width="100%" border ="0">
                            <tr valign="top">
                                 <td colspan="4" class="Td_ButtonContainer" style="width: 100%; text-align: left;" valign="top">Fund Status</td> 
                            </tr>
                            <tr style="height: 50px">
                                 <%--<td valign="top" class="Label_Small" style="height: 100px"> Existing FundStatus :</td> 
                                <td valign="top" class="Label_Small"> <asp:Label ID="lblExistingFundStatus" runat="server" Height="20" Text="RD" Width="200"></asp:Label></td> --%>
                                <td valign="top" class="Label_Small" colspan ="2" style="width: 20%"> New Fund Status :</td> 
                                <td valign="top" class="Label_Small" colspan ="2" style="width: 80%"> <asp:Label ID="lblNewFundStatus" runat="server" Height="20" Text="TM"></asp:Label></td> 
                             </tr> 
                             <tr>
                                <td valign="top" colspan ="4" class="Td_ButtonContainer" style="width: 100%; text-align: left;">Following Annuity will be deleted</td>                                
                             </tr> 
                             <tr style="height: 200px">
                                 <td valign="top" class="Label_Small" colspan ="4">
                                       <div style="overflow: auto; width: 100%; height: auto ; text-align: left">
                                            <asp:GridView runat="server" ID="gvReviewAnnuity" CssClass="DataGrid_Grid"
                                                AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top" 
                                                Width="100%" EmptyDataText="No records found.">
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <Columns>                                                  
                                                    <asp:BoundField HeaderText="Annuity Source" DataField="AnnuitySourceCode"/>
                                                    <asp:BoundField HeaderText="Annuity Type" DataField="AnnuityType"/>  
                                                    <asp:BoundField HeaderText="Current Payment" DataField="CurrentPayment" />
                                                    <asp:BoundField HeaderText="Purchase Date" DataField="PurchaseDate" />
                                                    <asp:BoundField HeaderText="Social Security Adj" DataField="SocialSecurityAdj" />
                                                    <asp:BoundField HeaderText="Death Benefit" DataField="DeathBenefitRemaining" />
                                                    <asp:BoundField HeaderText="Plan Type" DataField="PlanType" />                                                  
                                                   <asp:BoundField HeaderText="ID" DataField="DisbursementId" Visible="false" />
                                                   <%--<asp:BoundField HeaderText="AnnuityJointSurvivorsID" DataField="AnnuityJointSurvivorsID"  Visible="False" />                                                    --%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>                            
                             </tr>
                              <tr style="height: 200px">
                               <td colspan="4" class="Td_ButtonContainer" style="width: 100%; text-align: left;" valign="top">Notes</td>                                   
                             </tr> 
                             <tr style="height: 200px">
                               <td colspan="4" style="width: 100%; text-align: left;" valign="top">
                                   <asp:Label ID="lblReviewNotes" runat="server" class="Label_Small"></asp:Label> 
                               </td>                                     
                             </tr> 
                         </table>


                    </div>


                </td>
            </tr>
        </table>

        <div id="Tooltip" style="z-index: 1000; width: auto; border-left: 1px solid silver; border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc; padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black; display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana; margin: 0; overflow: visible;">
            <span id="lblComments" style="display: block; width: auto; overflow: visible; font-size: x-small;"></span>
        </div>

        <table width="100%" align="center">
            <tr>                
                <td class="Td_ButtonContainer" style="width: 33%; text-align: center;">
                    <asp:Button runat="server" ID="btnPrevious" Text="Previous" CssClass="Button_Normal"  style="width: 80px;"/>
                </td>
                <td class="Td_ButtonContainer" style="width: 33%; text-align: center;">
                    <asp:Button runat="server" ID="btnNext" Text="Next" CssClass="Button_Normal"  style="width: 80px;"/>
                    <asp:Button runat="server" ID="btnSave" Text="Submit" CssClass="Button_Normal" style="width: 80px;"/>
                </td>
                <td class="Td_ButtonContainer" style="width: 33%; text-align: center;">
                    <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal Warn_Dirty"  style="width: 80px;"/>
                </td>
            </tr>
        </table>

        <div id="ConfirmDialog" title="Reverse Annuity" style="display: none;" height="600px"  >
                    <div>
                        <table width="100%" height="600px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
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
                                    <asp:Button runat="server" ID="btnConfirmDialogYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        CausesValidation="False" OnClientClick="closeDialog('ConfirmDialog')"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialog('ConfirmDialog');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

    </div>
    
   <script type="text/javascript">
        function BindEvents(){
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, height:250, maxHeight: 250,
                title: "Reverse Annuities",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function ShowDialog() {
            var isOpen = false;
            $('#ConfirmDialog').dialog("open");
            return isOpen;
        }

        function closeDialog(id) {
            $('#' + id).dialog('close');
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
       <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
       <asp:AsyncPostBackTrigger ControlID="btnConfirmDialogYes" EventName="Click" />
       <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>





