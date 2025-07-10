<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StateWithholdingListingControl.ascx.vb" Inherits="YMCAUI.StateWithholdingListingControl" %>
<script>
   
    $(document).ready(function () {
        $('#divViewStateWithholdingData').dialog({
            autoOpen: false,
            draggable: true,
            resizable: false,
            close: true,
            modal: true,
            width: 480,
            height: "auto",
            title: "State Withholding Information",
            open: function (type, data) {                
                $(this).dialog('open');
                $(this).css('overflow', 'hidden');
            }
        })

    });


    function ViewStateWithodingDataOpen() {
        $('#divViewStateWithholdingData').dialog("open");
        return false;
    }

    function ViewStateWithodingDataClose() {
        $('#divViewStateWithholdingData').dialog("close");
        return false;
    }
  
   
</script>
  <div>
        <tr>
            <td  style="text-align:left" class="td_Text">State Withholding
            </td>
            <td style="text-align:right" class="Td_ButtonContainer">
                  <asp:Button ID="btnStateWithholdAdd" runat="server" Width="90px" Text="Add..."  CssClass="Button_Normal" CausesValidation="False" ></asp:Button>                
                
            </td>
        </tr>
        <tr style="vertical-align:top" >
            <td style="text-align:center"  colspan="2">
                <div style="overflow: auto; width: 100%; height: 140px; text-align: left">
                    <asp:DataGrid ID="DataGridStateWithholding" runat="server" Width="98%" CssClass="DataGrid_Grid"
                        AutoGenerateColumns="false"  ViewStateMode="Enabled"  >
                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                        <Columns>
                             <asp:TemplateColumn HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnStateWithholdView" runat="server" ImageUrl="..\images\view.gif"
                                        CausesValidation="False" CommandName="Select" ToolTip="View"  OnClientClick ="javascript:ViewStateWithodingDataOpen(); return false;"></asp:ImageButton>                                    
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-Width="2%">                                
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnStateWithholdEdit" runat="server" ImageUrl="..\images\edits.gif"
                                        CausesValidation="False" OnClick="image_OnClick" CommandName="Edit" CommandArgument="intUniqueID" ToolTip="Edit" ></asp:ImageButton>                                               
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                            <asp:BoundColumn HeaderText="State" DataField="chvStateCode" />
                            <asp:BoundColumn HeaderText="Disbursement Type" DataField="chvDisbursementType" />                           
                             <asp:TemplateColumn HeaderText="Election not to have income tax withheld">
                                 <ItemTemplate>
                                      <%#bitStateTaxNotElected(Boolean.Parse(DataBinder.Eval(Container.DataItem, "bitStateTaxNotElected").ToString()))%>
                                 </ItemTemplate>
                                 </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="Tax Withheld Amount" DataField="numComputedTaxAmount" />
                        </Columns>
                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                    </asp:DataGrid>
                </div>
                 <div id="divViewStateWithholdingData" title="State Withholding Data" style="display: none;">
                     <table style="width:100%" >
                          <tr> <td>
                                   <table style="width:100%;padding:0; border-collapse: collapse; border-spacing: 0;text-align: left;" border="1" Class="DataGrid_Grid" >
                                        <tr id="trStateName" style="height:20px;" runat="server" ><td><span class ="Label_Small">Name of State </span></td>
                                            <td><asp:Label ID="lblstateName" runat="server"></asp:Label></td></tr>

                                        <tr id="trDisbursement" style="height:20px;" runat="server" ><td><span class ="Label_Small">Type of Disbursement</span></td>
                                            <td><asp:Label ID="lblDisbursement" runat="server"></asp:Label></td></tr>

                                        <tr id="trElected" style="height:20px;" runat="server" ><td><span class ="Label_Small">Election not to have income tax withheld</span></td>
                                            <td><asp:Label ID="lblElected" runat="server"></asp:Label></td></tr>

                                        <tr id="trFlatamt" style="height:20px;" runat="server" visible="false"><td><span class ="Label_Small">Flat Amount</span></td>
                                            <td><asp:Label ID="lblFlatamt" runat="server"></asp:Label></td></tr>

                                        <tr id="trNewYourkCity" style="height:20px;" runat="server" visible="false"  ><td><span class ="Label_Small">New York City</span></td>
                                            <td><asp:Label ID="lblNewYourkCity" runat="server"></asp:Label></td></tr>

                                         <tr id="trYonkers" style="height:20px;" runat="server" visible="false"><td><span class ="Label_Small">Yonkers</span></td>
                                            <td><asp:Label ID="lblYonkers" runat="server"></asp:Label></td></tr>

                                        <tr id="trFederalWithhold" style="height:20px;" runat="server" visible="false"><td><asp:Label ID="lblpercentage" cssClass ="Label_Small" runat="server" Text="10% of Federal Withholding"></asp:Label></td>
                                            <td><asp:Label ID="lblFederalWithhold" runat="server"></asp:Label></td></tr>

                                        <tr id="trAdditionalAmt" style="height:20px;" runat="server" visible="false"><td><span class ="Label_Small">Additional Amount to be withheld</span></td>
                                            <td><asp:Label ID="lblAdditionalAmt" runat="server"></asp:Label></td></tr>

                                        <tr id="trMaritalStatus" style="height:20px;" runat="server" visible="false"><td><span class ="Label_Small">Marital Status</span></td>
                                            <td><asp:Label ID="lblMaritalStatus" runat="server"></asp:Label></td></tr>

                                        <tr id="trExemptions" style="height:20px;" runat="server" visible="false"><td><span class ="Label_Small">Exemptions</span></td>
                                            <td><asp:Label ID="lblExemptions" runat="server"></asp:Label></td></tr>
                                         </table>                             
                                </td></tr>
                          <tr>
                            <td >
                                <hr />
                            </td>
                        </tr>
                          <tr id="tr1" style="height:20px;">
                            <td style="text-align:Center;display:inline"  colspan="2" >
                                 <input type="button" ID="Button1" value="OK" class="Button_Normal" Style="width: 80px;
                                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                                    height: 16pt;" OnClick ="javascript: ViewStateWithodingDataClose(); ">
                                &nbsp;
                    
                            </td>
                        </tr>
                    </table>
      
             </div> 
            </td>
        </tr>
   </div>


  
