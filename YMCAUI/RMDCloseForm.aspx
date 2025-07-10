
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RMDCloseForm.aspx.vb" 
MasterPageFile="~/MasterPages/YRSMain.Master"  Inherits="YMCAUI.RMDCloseForm" %>


<asp:Content ContentPlaceHolderID="head" runat="server">
<title>
    YMCA YRS
</title>
<script language="javascript" type="text/javascript" src="JS/YMCA_JScript.js"></script>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">   </script>

<script language="javascript" type="text/javascript">
   $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (args.get_error() == undefined) {   
            }
        }  

       $('#ConfirmDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            width:450, height: 200,
            title: "Close Current MRD",
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
      

    });

    function showDialog(id, text, btnokvisibility) {
        $('#' + id).dialog({ modal: true });
        $('#lblMessage').text(text)
        $('#' + id).dialog("open");
        $("#btnYes").show();
        $("#btnNo").show();

    }    
    function closeDialog(id) {

        $('#' + id).dialog('close');
    }

    function disableButton() {
        $('#lblMessage').text('Processing your request...');
        $("#btnYes").hide();
        $("#btnNo").hide();

    }


</script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" >
<div class="Div_Center" style="width:100%; " >
<asp:ScriptManagerProxy   id="dbScriptManagerProxy" runat="server"> 
</asp:ScriptManagerProxy> 
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="loading">
                <img id="loadingimage" runat="server" src="images/ajax-loader.gif" alt="Loading..." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <div class="Div_Center" style="width:100%">
        <table align="center" class="Table_WithBorder" border ="0" width="100%">
            <tbody>
                <tr>
                    <td valign="top" nowrap="nowrap" colspan="2" >
                        <asp:label id="lblMRDDate" cssclass="Label_Small" text="RMD Process Date : " runat="server" font-names="Arial" font-size="X-Small" ></asp:label> 
                        <asp:label id="lblDate" cssclass="Label_Small" text="" runat="server" font-names="Arial" font-size="X-Small" ></asp:label>       
                    </td>
                </tr>
                 <tr valign="top">
                    <td valign="top" nowrap="nowrap" width="70%">
                        <table height="400px" width="100%" class="Table_WithBorder">
                            <tr valign="top"" >
                                <td>  
                                     <asp:GridView ID="gvRMDProcessLog" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                    EditRowStyle-Wrap="true" EditRowStyle-Width="1000px" EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true"
                                                    HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true" SelectedRowStyle-Wrap="true"
                                                     SortedAscendingCellStyle-Wrap="true" SortedDescendingCellStyle-Wrap="true"
                                                    AllowSorting="True" AllowPaging="True" PageSize="10" 
                                                    AutoGenerateColumns="False" PagerStyle-Font-Names="Arial"
                                                    PagerStyle-Font-Size="5px" Width ="100%">
                                                     <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField HeaderText="RMD Year" HeaderStyle-HorizontalAlign="Center"  DataField="RMDYEAR" />
                                                    <asp:BoundField HeaderText="Generated On"  HeaderStyle-HorizontalAlign="Center" DataField="Generated_On" ItemStyle-HorizontalAlign ="left"   />
                                                    <asp:BoundField HeaderText="Closed On"  HeaderStyle-HorizontalAlign="Center" DataField="Dec_Closed_On" ItemStyle-HorizontalAlign ="left" />  
                                                    <asp:BoundField HeaderText="Closed By"  HeaderStyle-HorizontalAlign="Center"  DataField="Dec_Closed_By" />
                                                    <asp:BoundField HeaderText="Closed On"  HeaderStyle-HorizontalAlign="Center" DataField="Mar_Closed_On" ItemStyle-HorizontalAlign ="left" />
                                                    <asp:BoundField HeaderText="Closed By"  HeaderStyle-HorizontalAlign="Center" DataField="Mar_Closed_By" />                                                   
                                                </Columns>
                                            </asp:GridView>                                               
                                        </td> 
                                    </tr>
                            </table>
                     </td>                    
                     <td valign="top" width="30%" align="center" class="Table_WithBorder" height="400px">
                          <table   width="100%" >                              
                              <tr align="center">
                                  <br/>
                                <td> <asp:Button ID="btnCloseCurrentRMD" runat="server" CssClass="Button_Normal" Text="Close Current RMD" Width="190px"></asp:Button> </td>     
                              </tr>                                        
                               <tr align="left" >
                                <td>
                                    <br/>   
                                    <asp:label id="lblTotalRMDs" cssclass="Label_Small" runat="server" font-names="Arial" font-size="X-Small" text="Total RMDs : " ></asp:label>
                                    <asp:label id="lblTotalRMDsCnt" cssclass="Label_Small" text="" runat="server" font-names="Arial" font-size="X-Small" ></asp:label>
                                </td>
                              </tr>     
                              <tr align="left" >
                                <td>
                                    <asp:label id="lblUnsatisfiedRMD" cssclass="Label_Small" runat="server" font-names="Arial" font-size="X-Small" text="Unsatisfied RMDs : " ></asp:label>
                                    <asp:label id="lblUnsatisfiedRMDCnt" cssclass="Label_Small" text="" runat="server" font-names="Arial" font-size="X-Small" ></asp:label>
                                </td>     
                              </tr>    
                              <tr align="left" >
                                <td>
                                    <asp:label id="lblProcessedRMD" cssclass="Label_Small" runat="server" font-names="Arial" font-size="X-Small" text="Satisfied RMDs : " ></asp:label>
                                    <asp:label id="lblProcessedRMDCnt" cssclass="Label_Small" text="" runat="server" font-names="Arial" font-size="X-Small" ></asp:label>
                                </td>     
                              </tr>    
                        </table>                 
                     </td> 
                </tr>  
                <tr>    
                   <td align="right" class="Td_ButtonContainer" colspan = "2">
                       <asp:button id="btnCloseForm" runat="server" text="Close" width="80" CssClass="Button_Normal"  CausesValidation="False"></asp:button>
                   </td>  
              </tr>                  
            </tbody>
        </table>        
    </div>          
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>    
</div>

<div id="ConfirmDialog" style="overflow:visible;">   
       <asp:UpdatePanel ID="upConfirmationDiv1" runat="server" UpdateMode="Conditional">
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
                            <hr/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="bottom">
                            <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" OnClientClick="javascript: disableButton();" />&nbsp;
                            <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" 
                                Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt;
                                font-weight: bold; height: 16pt;" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>            
            <asp:AsyncPostBackTrigger ControlID="btnCloseCurrentRMD" EventName="Click" />  
             <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />  
              <asp:PostBackTrigger ControlID="btnYes"  />         
        </Triggers>
    </asp:UpdatePanel>        
    </div>
</asp:Content>