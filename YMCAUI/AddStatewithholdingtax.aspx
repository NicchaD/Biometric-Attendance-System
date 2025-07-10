<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddStatewithholdingtax.aspx.vb"  Inherits="YMCAUI.AddStatewithholdingtax" MasterPageFile="~/MasterPages/YRSPopUp.Master" %>
<%@ Register  TagPrefix="YRSCustomControls"  Namespace="CustomControls" Assembly="CustomControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="CSS/CustomStyleSheet.css" rel="stylesheet" type="text/css" />
   
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>

    <script  type="text/javascript">       
        function BindEvents() {
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                autoResize:true,
                width: 450,
                scrollable: false,
                maxHeight: 400,               
                title: "YMCA - YRS",
                open: function (type, data) {
                    $('#ConfirmDialog').css('overflow', 'hidden'); //this line does the actual hiding
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#divSuccess').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 450,
                height: 270,
                title: "YMCA - YRS",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            })

            $('#divNoChanges').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 450,
                height: 270,
                title: "YMCA - YRS",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            })
        }

        function ShowDialog(id) {
            $('#' + id).dialog('open');           
            return false;
        }
               
        function closeDialog(id) {
            $('#' + id).dialog('close');
        }

        // Redirect to Parent Page of control
        function redirectToParentPage() {
            window.opener.document.forms(0).submit();
            self.close();
        }
               
        // Check Min Max Value for FlatAmount and AdditionalAmount on Text Change
         $(function CheckMinMaxFunction() {
             $("#NTFlatAmount").change(function () {
                 var max = parseInt($(this).attr('max'));
                 var min = parseInt($(this).attr('min'));
                 if ($(this).val() > max) {
                     $(this).val(max.toFixed(2));
                 }
                 else if (($(this).val().length == 0) || ($(this).val() < min)) {
                     $(this).val(min.toFixed(2));
                 }
                 else {
                     var value = $("#ddlStateCode option:selected").val()
                     if ((value == 'NY') || (value == 'NJ')) {
                         var Amount = $(this).val();
                         Amount = Math.round(Amount);
                         $(this).val(Amount.toFixed(2));
                     }
                 }                 
             });

             $("#NtAdditionalAmount").change(function () {
                 var max = parseInt($(this).attr('max'));
                 var min = parseInt($(this).attr('min'));
                 if ($(this).val() > max) {
                     $(this).val(max.toFixed(2));
                 }
                 else if (($(this).val().length == 0) || ($(this).val() < min)) {
                     $(this).val(min.toFixed(2));
                 }
             });            

             $("#NTNoOfExemption").change(function () {
                 var max = parseInt($(this).attr('max'));
                 var min = parseInt($(this).attr('min'));
                 if ($(this).val() > max) {
                     $(this).val(max);                     
                 }                 
                 else if (($(this).val().length == 0) || ($(this).val() < min)) {
                     $(this).val(min);                   
                 }             
             });
             
             $("#NTNewyorkCityOrYonkers").change(function () {
                     var value = $("#ddlStateCode option:selected").val()
                     if (value == 'NY') {
                         var Amount = $(this).val();
                         Amount = Math.round(Amount);
                         $(this).val(Amount.toFixed(2));
                     }                 
             });             
         });                      
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Maincontent"   runat="server">
  <asp:ScriptManagerProxy ID="AddStateWithholdingScriptManager" runat="server">
        </asp:ScriptManagerProxy>
<div class="Div_Center" style="width:100%;">
    <asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
                
    <table class="Table_WithBorder" style="width:100%" >    
  
            <tr>
            <td colspan="3">
                <table style="width:100%;padding:0; border-collapse: collapse; border-spacing: 0;" border="0" >
                  <tr>
                       
                      <td style="text-align : left;" class="td_Text">
                           FundNo : <asp:Label ID="lblFundNo" runat="server"/>
                      </td>
                      <td style="text-align : left;" class="td_Text">
                          Name : <asp:Label ID="lblName" runat="server" />
                      </td>
                      <td style="text-align : left;" class="td_Text">
                          Current Residing State : <asp:Label ID="lblCurrentState" runat="server" />
                      </td>
                    </tr>
                    <tr style="height:2px;">
                           <td colspan="3" class="Label_Small" style="text-align:right;"><span class="aestrik" style="font-size:10px;">*</span> <span style="font-size:9px;">Required Fields</span> </td>
                       </tr>
                </table>
            </td>
            </tr>
            <tr>
              <td colspan="3">           
                  <div style="width:100%;">                
                                <table id="Table2"  style="border-collapse:separate; border-spacing:0 15px;width:100%; border-collapse: collapse; border-spacing: 0;"   border="0">         
                                         <tr style="height:25px;">
                                                <td  style="text-align:left;width:25%;white-space: nowrap" >
                                                 <span class="aestrik">*</span><span class="Label_Small">Name of State</span> 
                                                </td>
                                                 <td style="text-align:left;width:25%" >                                
                                                     <asp:DropDownList ID="ddlStateCode" runat="server" Width="80px" AutoPostBack ="true" OnSelectedIndexChanged="ddlStateCode_SelectedIndexChanged" CssClass="DropDown_Normal Warn">
                                                     </asp:DropDownList>                                
                                                 </td>
                                                <td  style="text-align:left;width:25%;white-space: nowrap" class="bitDisbursementType" runat="server">
                                                   <span class="aestrik">*</span><asp:Label ID="lblWithHoldingType" runat="server" CssClass="Label_Small"> Type of Disbursement</asp:Label>
                                                </td>
                                                <td style="text-align:left;width:25%" class="bitDisbursementType" >                                     
                                                     <asp:DropDownList ID="ddlWithHoldingType" runat="server" Width="90px"  CssClass="DropDown_Normal Warn" AutoPostBack="true" OnSelectedIndexChanged="ddlWithHoldingType_SelectedIndexChanged">
                                                         <asp:ListItem Value="ANN">Annuity</asp:ListItem>
                                                         <asp:ListItem Value="REF">Lump sum</asp:ListItem>
                                                     </asp:DropDownList>                         
                                                </td>                           
                                            </tr>
                                         <tr><td colspan="4"> <br /> </td> </tr>
                                         <tr id="trStateTaxInfo">
                                            <td colspan="4">
                                                <table  style="border:solid 1px;border-color:lightgray ;width:100%"">                               
                                                    <tr><td style="text-align:left" class="Label_Medium"> <asp:label id="lblStateTitle" runat="server">  </asp:label></td>  </tr>
                                                    <tr  style="height:25px;">                               
                                                    <td  style="text-align:left;" >
                                                        <div style="font: 11px/14px Verdana,Arial,Helvetica;line-height:17px;">                                        
                                                           <asp:Label ID="lblStateTaxInfo" Text="" Width="100%" runat="server" Style="word-wrap: normal;"   />
                                                       </div>
                                                    </td>
                                                    </tr>                               
                                                </table>
                                            </td> </tr>
                                         <tr> <td colspan="4"><br /></td>   </tr>
                                          <tr id="trStateTaxNotElected" runat="server" style="height:25px;" >
                                                <td  style="text-align:left;width:25%" class="bitStateTaxNotElected"  colspan ="2" >
                                                    <span class="aestrik">*</span><asp:label ID="lblElected" runat="server" CssClass="Label_Small" style="word-wrap:normal">Election not to have income tax withheld</asp:label>                             </td>
                                                <td style="text-align:left;width:25%" class="bitStateTaxNotElected" >
                                                    <asp:DropDownList ID="ddlElected" runat="server" Width="80px"  CssClass="DropDown_Normal Warn" AutoPostBack="true" OnSelectedIndexChanged="ddlElected_SelectedIndexChanged">
                                                         <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                         <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                     </asp:DropDownList>                                                                                        
                                                </td>
                                                 <td  style="text-align:left;width:25%;">                                 
                                                </td>                           
                                          </tr>                     
                                         <tr id="trFlatAmount" runat="server" style="height:25px;">
                                             <td  style="text-align:left;width:25%;white-space: nowrap"  colspan="2" >
                                                     <span class="aestrik">*</span><asp:label ID="lblFlatAmount" runat="server" CssClass="Label_Small" style="word-wrap:normal">Flat Amount</asp:label>
                                                                            <asp:RadioButton ID="rblFlatAmtYes"   AutoPostBack="true" runat="server" CssClass ="Label_Small Warn" GroupName ="amount" Text ="Yes" />
                                                 <asp:RadioButton ID="rblFlatAmtNo" runat="server"  AutoPostBack="true" CssClass ="Label_Small Warn" GroupName ="amount" Text ="No" />
                                            </td>
                                               <td style="text-align:left;width:25%"  >
                                                      <YRSCustomControls:NumberTextBox ID="NTFlatAmount"    AllowDecimal ="true"  AllowNegative ="false"  min="0" max="10000" 
                                                       style="width:80px" runat ="server" CssClass="TextBox_Normal Warn"  ></YRSCustomControls:NumberTextBox>                                                               
                                                </td>
                                             </tr>

                                         <tr id="trNewyork" runat="server" style="height:25px;">                         
                                            <td colspan ="2">
                                                 <asp:RadioButton ID="rblNYCT" runat="server" CssClass ="Label_Small Warn" GroupName ="NEWYORK"  Text ="New York City" />
                                                 <asp:RadioButton ID="rblYonkers" runat="server" CssClass ="Label_Small Warn" GroupName ="NEWYORK" Text ="Yonkers" />
                                            </td>
                                               <td style="text-align:left;width:25%"  >
                                                      <YRSCustomControls:NumberTextBox ID="NTNewyorkCityOrYonkers"     AllowDecimal ="true"  AllowNegative ="false" 
                                                       style="width:80px" runat ="server" CssClass="TextBox_Normal Warn"  ></YRSCustomControls:NumberTextBox>                                                                
                                                </td>
                                             </tr>

                                          <tr id="trpercentageoffedwithhold" runat="server" style="height:25px;">
                                              <td id="Td1"  class="bitPercentageofFederalWithholding"  colspan="2" runat="server" style="width:25%;  ">
                                                 <span class="aestrik">*</span><asp:label ID="lblPercentageoffederalwithholding" runat="server" CssClass="Label_Small" >10% of Federal Withholding</asp:label>
                                            </td>
                                            <td id="Td2" class="bitPercentageofFederalWithholding" runat="server"  style="width:25%;">
                                                  <asp:DropDownList ID="ddlPercentageofFedralwithholding" runat="server" Width="80px"  CssClass="DropDown_Normal Warn" AutoPostBack="true" OnSelectedIndexChanged="ddlPercentageofFedralwithholding_SelectedIndexChanged" >
                                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                    </asp:DropDownList>  
                                                      <asp:HiddenField ID="hdnMetaFederalWithholdingValue" runat="server" />                
                                             </td>
                                          </tr>

                                         <tr id="trAdditionalAmount" runat="server" style="height:25px;">
                                             <td  style="text-align:left;width:25%;white-space: nowrap"   colspan="2" >
                                                     <span id ="aestrikAdditionalAmt" runat="server" class="aestrik">*</span><asp:label ID="lblAdditionalAmount" runat="server" CssClass="Label_Small" style="word-wrap:normal">Additional Amount to be withheld</asp:label>
                                               </td>                        
                                               <td style="text-align:left;width:25%"  >
                                                      <YRSCustomControls:NumberTextBox ID="NtAdditionalAmount"    AllowDecimal ="true"  AllowNegative ="false"  min="0" max="10000" 
                                                       style="width:80px" runat ="server" CssClass="TextBox_Normal Warn"  ></YRSCustomControls:NumberTextBox>                                                                
                                                </td>
                                             </tr>

                                          <tr id="trMaritalStatusCode" runat="server" style="height:25px;">
                                             <td  style="text-align:left;width:25%;white-space: nowrap" class="bitMaritalStatusCode"  colspan="2" >
                                                     <span class="aestrik">*</span><asp:label ID="lblMaritalStatus" runat="server" CssClass="Label_Small" style="word-wrap:normal">Marital Status</asp:label>
                                                </td>
                                                <td style="text-align:left;width:25%" class="bitMaritalStatusCode" >
                                                    <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="80px"  CssClass="DropDown_Normal Warn">
                                                        <asp:ListItem Value="" Selected="True">Select</asp:ListItem>
                                                         <asp:ListItem Value="S">S</asp:ListItem>
                                                         <asp:ListItem Value="H">H</asp:ListItem>
                                                         <asp:ListItem Value="M">M</asp:ListItem>
                                                     </asp:DropDownList>                                                                 
                                                </td>
                                             </tr>
         
                                           <tr id="trNoOfExemption" runat="server"  style="height:25px;">
                                             <td  style="text-align:left;width:25%;white-space: nowrap" class="bitNoOfExemption" colspan="2" >
                                                     <span class="aestrik">*</span><asp:label ID="lblNoOfExemption" runat="server" CssClass="Label_Small" style="word-wrap:normal">Exemptions</asp:label> 
                                                </td>
                                             <td style="text-align:left;width:25%" class="bitNoOfExemption" >
                                                  <YRSCustomControls:NumberTextBox ID="NTNoOfExemption"    AllowDecimal ="false"  AllowNegative ="false"  min="0" max="99"
                                                       style="width:80px" runat ="server" CssClass="TextBox_Normal Warn"></YRSCustomControls:NumberTextBox>
                                                </td>
                                            </tr>

                                       <tr><td colspan="4"> <br /> </td> </tr>
                                    <tr>
                          <td style="text-align: right; width: 100%;" class="td_Text"  colspan="4">                           
                                <asp:Button ID="btnSave" runat="server" Text="OK " CssClass="Button_Normal  PreventDoubleClick"   style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"  />&nbsp;                             
                                <asp:Button ID="btnClose" CssClass="Button_Normal Warn_Dirty" runat="server" Text="Cancel" CausesValidation="false" />                     
                            </td>
                        </tr>                    
                        </table> 
                        </div> 
                     
              </td> 
                
            </tr>
           
        
        </table> 

     <!-------------------------------------------------Confirm Dialogbox START--------------------------------------------------------->            
     <div id="ConfirmDialog" title="YMCA - YRS" style="display: none; ">
        <table  border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;width:100%">
            <tr>
                <td rowspan="2" style="width: 10%;">
                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divWarnningMessage" runat="server" style="height:auto;" >
                        <asp:Label ID="lblFederalWitholdingMessage" runat="server" Text=""></asp:Label>
                    </div>
                    <br />
                    <div id="divConfirmDialogMessage">                        
                        Please ensure that all the details updated in the state withholding section match the paper form. To verify, click on “Go back”. To proceed, click on “Confirm”.
                        
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr id="trConfirmDialogOK">
                <td style="text-align:right;vertical-align:bottom"  colspan="2">
                    <asp:button runat="server" id="btnConfirmDialog" text="Confirm" cssclass="Button_Normal" 
                        style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                    &nbsp;
                    <input type="button" ID="btnGoBack" value="Go back" class="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" onclick="closeDialog('ConfirmDialog');" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <br />
                </td>
            </tr>
        </table>
    </div>
    <!-------------------------------------------------Confirm Dialogbox END--------------------------------------------------------->            
      
    <!-------------------------------------------------Success Dialogbox START--------------------------------------------------------->            
    <div id="divSuccess" title="YMCA - YRS" style="display: none;">
        <table  border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;width:100%"">
            <tr>
                <td rowspan="2" style="width: 10%;">
                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img1" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div2">
                        Saved Successfully
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr id="tr1">
                <td   style="vertical-align:bottom;text-align:right;" colspan="2">
                     <input type="button" ID="Button1" value="OK" class="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" onclick="redirectToParentPage();" />
                    &nbsp;
                    
                </td>
            </tr>
        </table>
    </div>
    <!-------------------------------------------------Success Dialogbox END--------------------------------------------------------->  
    
     <!-------------------------------------------------NoChanges to Save Dialogbox START--------------------------------------------------------->            
    <div id="divNoChanges" title="YMCA - YRS" style="display: none;">
        <table  border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;width:100%"">
            <tr>
                <td rowspan="2" style="width: 10%;">
                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img2" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="div3">
                       <asp:Label ID="lblNochanges" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr id="tr2">
                <td   style="vertical-align:bottom;text-align:right;" colspan="2">
                     <input type="button" ID="Button2" value="OK" class="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" onclick="closeDialog('divNoChanges');" />
                    &nbsp;
                    
                </td>
            </tr>
        </table>
    </div>
    <!-------------------------------------------------NoChanges to Save Dialogbox END--------------------------------------------------------->  
    
    </div> 
</asp:Content>


