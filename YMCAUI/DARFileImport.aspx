<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DARFileImport.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.DARFileImport" %>

<asp:Content ID="contentDARFileImportHead" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/jquery-bPopup.js"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    <style type="text/css"> 
     .BG_ColourCheckStatusRETURNED {
            background-color: #00BFFF; /*DodgerBlue*/
        }
        .alignright {
            text-align:right ;
        }
        
        </style>
    <script type="text/javascript">
         function ShowErrorDetailsDialog(fosender) {
             //get the ImportBaseFundId/Process id
             var ImportBaseId = $(fosender).parent().find("[id$=hndImportBaseId]").val();
             //get the Fund id
             var ImportBaseFundId = $(fosender).parent().find("[id$=hdnFundNo]").val();
             //get the ImportBaseDetailID
             var ImportBaseDetailID = $(fosender).parent().find("[id$=hndImportBaseDetailId]").val();
                     
             GetErrorDetailsForEachRecord(ImportBaseId, ImportBaseDetailID);
             $('#divErrorDetails').dialog("open");
             return false;
         }

         function GetErrorDetailsForEachRecord(ImportBaseId, ImportBaseDetailID) {
             $.ajax({
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 url: "DARFileImport.aspx/GetErrorDetailsForEachRecord",
                 data: "{ 'requestedImportBaseId': '" + ImportBaseId + "','requestedImportDetailId':'" + ImportBaseDetailID + "'}",
                 datatype: "json",
                 success: function (data) {
                     $("#tblErrorDetailsEachRecord tbody").html("");
                     if ((data.d == null) ||(data.d.length <= 0)) {
                    $("#tblErrorDetailsEachRecord").append("<tr class='DataGrid_NormalStyle'><td colspan='2'>No records found.</td></tr>");
                        }
                    
                     else {
                       
                         for (var i = 0; i < data.d.length; i++) {                             
                                 $("#tblErrorDetailsEachRecord").append("<tr class='DataGrid_NormalStyle'><td colspan='0' style='width:20%;'>" + data.d[i].FundID + "</td> <td style='width:80%;'>" + data.d[i].ErrrorMessage + "</td> </tr>");
                                                     }
                       }
                      },
                error: function (result) {
                    $("#tblErrorDetailsEachRecord tbody").html("");
                    $("#tblErrorDetailsEachRecord").append("<tr class='DataGrid_NormalStyle'><td colspan='2'>" + result.responseText + "</td></tr>");
                    }
                });

           }

         function ClossErrorDetailsDialog() {
             $('#divErrorDetails').dialog("close");
             return false;
                }
         function BindEvents() {

             $('#divErrorDetails').dialog({
                 autoOpen: false,
                 resizable: false,
                 dialogClass: 'no-close',
                 draggable: true,
                 width: 450, minHeight: 420,
                 height: 380,
                 closeOnEscape: false,
                 title: "Record Error Details",
                 modal: true,
                 open: function (type, data) {
                     $(this).parent().appendTo("form");
                 }
             });

             $('#ConfirmDialogRun').dialog({
                 autoOpen: false,
                 draggable: true,
                 close: false,
                 modal: true,
                 width: 450, maxHeight: 420,
                 height: 260,
                 title: "DAR Import File",
                 open: function (type, data) {
                     $(this).parent().appendTo("form");
                     $('a.ui-dialog-titlebar-close').remove();
                 }
             });

             $('#ConfirmDialogDiscard').dialog({
                 autoOpen: false,
                 draggable: true,
                 close: false,
                 modal: true,
                 width: 450, maxHeight: 420,
                 height: 260,
                 title: "DAR Import File",
                 open: function (type, data) {
                     $(this).parent().appendTo("form");
                     $('a.ui-dialog-titlebar-close').remove();
                 }
             });
         }

         function ShowDialogRun(id, text) {
             var isOpen = false;
             $('#ConfirmDialogRun').dialog("open");
             return isOpen;
         }

         function closeDialogRun(id) {
             $('#' + id).dialog('close');
         }

         function ShowDialogDiscard(id, text) {
             var isOpen = false;
             $('#ConfirmDialogDiscard').dialog("open");
             return isOpen;
         }
         function CallLetter() {
             window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
         }
         function closeDialogDiscard(id) {
             $('#' + id).dialog('close');
            
         }

</script>
</asp:Content>

<asp:Content ID="contentDARFileImport" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

     <%--<asp:ScriptManagerProxy ID="DARFileImportScriptManager" runat="server">
        </asp:ScriptManagerProxy>--%>
      <div class="Div_Center">
            <table class="td_withoutborder" cellpadding="0" cellspacing="0" style="width: 100%; vertical-align: top;" border ="0" >           
           <tr>
            <td>
                <div  id ="divDARImportFileSection" runat ="server" >
				<table class="Table_WithBorder" width="100%" border="0">
                   <tr>
                        <td colspan="3" > &nbsp;</td>
                    </tr>
                    
                    <tr>
						<td width="35%" class="Label_Small" align="right"> Import Bank Acknowledgement File :  
                        </td>
                        <td align="right" width="25%">
                            <input id="FileField" type="file" name="FileField" runat="server" enableviewstate="true" />
                        </td>
						<td align="left" width="40%">
                        	<asp:button id="btnImportDARBankResponseFile" runat="server" CssClass="Button_Normal" Width="80px" Text="Import"
						    causesValidation="false" Height="21px"></asp:button>
                            <%--START Megha --%>
                           <%-- <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btnUpload" runat="server" Text="Upload"
            OnClick="btnUpload_Click" />
                            <br />

<asp:Label ID="Label1" runat="server" Text="Has Header ?" />--%>

<%--END Megha --%>
				        </td>
					</tr>
                    <tr>
                        <td colspan="3" style="text-align:left;padding-left:10px;padding-top:10px" class="Label_Small"> </td>
                    </tr>
                     <tr>
                        <td colspan="3" > &nbsp;</td>
                    </tr>
				</table>
               </div> 
			
            </td>
        </tr>
            
        </table>
      
      </div>
    <%-- <asp:UpdatePanel ID="uplDARFileImport" runat="server">
       <ContentTemplate>--%>
         
           <div class="Div_Center" style="width: 100%;height: 100%;" >
                <table class="Table_WithBorder" style="width: 100%; height:450px;" border="0" >
                        <tr style="vertical-align:top">
                            <td style="vertical-align:top">
                                <div id="divDisplayImportedResultsSection" style="width: 100%;display:block;" runat="server">
                                    <table style="width: 100%;border-color:green " border="0">
                                        <%--Imported Records Information section--%>
                                        <tr style="vertical-align: top;">
                                            <td style="text-align:left;height:15pt" class="td_Text" >
                                                <asp:Label ID="lblDARImportRecordsInformaion" runat="server">Imported Bank Records</asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top;">
                                            <td style="text-align:left;"> <br />
                                                <div style="overflow: auto; width: 100%; height: 240px; text-align: left" >
                                                    <asp:GridView ID="gvDARImportedRecordsList" AllowSorting="true"  
                                                        runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found."  >
                                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                       <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <SortedAscendingHeaderStyle  CssClass="sortasc" />
                                                    <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                    <Columns>
                                                    
                                                        <%--<asp:BoundField HeaderText="#" DataField="ID" HeaderStyle-Width="5%"/>--%>
                                                        <asp:BoundField HeaderText="ImportBaseHeaderId" DataField="ImportBaseHeaderId" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                        <asp:BoundField HeaderText="Fund No." DataField="FundNo" HeaderStyle-Width="9%"  />
                                                        <asp:BoundField HeaderText="Last Name" DataField="LastName" HeaderStyle-Width="12%" />
                                                        <asp:BoundField HeaderText="First Name" DataField="FirstName" HeaderStyle-Width="12%" />
                                                        <asp:BoundField HeaderText="Gross Amount" DataField="GrossAmount" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%"  DataFormatString="{0:N}" HtmlEncode="false" />
                                                        <asp:BoundField HeaderText="State Tax Withheld" DataField="StateAmount" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%" DataFormatString="{0:N}" HtmlEncode ="false" />
                                                        <asp:BoundField HeaderText="Fed Tax Withheld" DataField="TotalFederalAmount" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%" DataFormatString="{0:N}"/>
                                                        <asp:BoundField HeaderText="NYC or Yonkers Tax Withheld" DataField="LocalFlatAmount" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%" DataFormatString="{0:N}"/>
                                                        <asp:BoundField HeaderText="Other Withheld" DataField="OtherWithholdings" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%" DataFormatString="{0:N}"/>
                                                        <asp:BoundField HeaderText="Net Amount" DataField="NetAmount" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%" DataFormatString="{0:N}"/>
                                                        <asp:BoundField HeaderText="Check#" DataField="CheckNumber" ItemStyle-CssClass="alignright" HeaderStyle-Width="10%"/>
                                                        <asp:BoundField HeaderText="IsErrorRecordExits" DataField="ERROR_EXIST" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                               
                                                         <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
		                                                <ItemTemplate> 
                                                           <asp:ImageButton ID="ImgErrorDetails" runat="server" CommandName="ViewErrorRecords" 
                                                                CausesValidation="False" ImageUrl="images\error.gif"  ToolTip="click here to view error(s)"
                                                               style="cursor: pointer;height:11px;" OnClientClick="javascript: ShowErrorDetailsDialog(this); return false;"
                                                               > </asp:ImageButton>
                                                            <asp:Label ID="lblErrorDetails" runat="server" Text="" Visible="false"></asp:Label>
                                                             <asp:HiddenField ID ="hndImportBaseId" runat="server" Value='<%#Eval("ImportBaseHeaderId")%>'></asp:HiddenField> 
                                                            <asp:HiddenField ID ="hdnFundNo" runat="server" Value='<%#Eval("FundNo")%>'></asp:HiddenField>
                                                            <asp:HiddenField ID ="hndCheckStatus" runat="server" Value='<%#Eval("CheckStatus")%>'></asp:HiddenField>
                                                            <asp:HiddenField ID ="hndImportBaseDetailId" runat="server" Value='<%#Eval("ImportBaseDetailID")%>'></asp:HiddenField>

                                                        </ItemTemplate>
		                                                </asp:TemplateField>

                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle CssClass="pagination"  />
                                                    </asp:GridView>
                                                 </div>
                                            
                                            </td>
                                        </tr>                                
                                        <tr>
                                          <div id ="divTotalRecords" runat ="server" style="border:dashed">
                                            <td style="text-align: right; width: 100%; ">
                                            <label class="Label_Small">No of record(s) : </label>
                                            <span id="lblRecords" class="Label_Small" runat="server"></span>
                                            </td>
                                          </div>
                                        </tr>
                                         <tr>
                                                    <td align="left" colspan="2" Class="Label_Small">
                                                        <span class="BG_ColourCheckStatusRETURNED">&nbsp;&nbsp;&nbsp;</span> <span class="Label_Small">No records will be updated for returned items. Please review and handle them manually. </span>
                                                        
                                                  </td>
                                     </tr>
                                    </table>
                                </div>
                           
                            </td>
                        </tr>
                        <tr style="vertical-align:top;width: 100%;">
                            <td>
                                 <div id="divDARImportSummary" style="width: 100%;display:block;" runat="server">
                                    <table border ="0" style="width: 100%;">
                                        <tr style="vertical-align: bottom;">
                                            <td class="td_Text" >
                                                Summary (DAR Report)
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: bottom;">
                                        <td>
                                            <table style="width: 100%; " border="0">
                                                 <tr>
                                                    <td align="left" width="17%">
                                                        <asp:Label ID="lblTotalGrossAmount" runat="server" CssClass="Label_Small">Total Gross Amount:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                        <span id="lblTotalGrossAmountValue" runat="server" Class="Label_Small" ></span>
                                                    </td>
                                                    <td align="left" width="17%" >
                                                        <asp:Label ID="lblTotalStateTaxWithheld" runat="server" CssClass="Label_Small">Total State Tax Withheld:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">  
                                                        <span ID="lblTotalStateTaxWithheldValue" runat="server" Class="Label_Small"></span>
                                                    </td>
                                                    <td align="left" width="18%">
                                                        <asp:Label ID="lblTotalFedTaxWithheld" runat="server" CssClass="Label_Small">Total Fed Tax Withheld:</asp:Label>
                                                    </td>
                                                    <td align="right" width="18%" style="padding-right:20px;">
                                                        <span ID="lblTotalFedTaxWithheldValue" runat="server" Class="Label_Small"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="18%">
                                                        <asp:Label ID="lblTotalLocalTaxWithheld" runat="server" CssClass="Label_Small">Total Local and Other Withheld:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                        <span ID="lblTotalLocalTaxandOtherWithheldValue" runat="server" Class="Label_Small"></span>
                                                    </td>
                                                    <td align="left" width="18%">
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                    </td>
                                                    <td align="left" width="18%">
                                                        <asp:Label ID="lblTotalNetAmount" runat="server" CssClass="Label_Small">Total Net Amt.:</asp:Label>
                                                    </td>
                                                    <td align="right" width="18%" style="padding-right:20px;">
                                                       <span ID="lblTotalNetAmountValue" runat="server" Class="Label_Small"></span>
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

                <table style="width:100%; text-align:center;">
                        <tr>
                            <td style="text-align:right;width:100%;padding-right: 7px" class="td_Text">
                                <asp:Button ID="btnPrintDARImportFileList" runat="server" Text="Print List" class="Button_Normal"/>&nbsp;
                                <asp:Button ID="btnRunDARImportFile"  runat="server" Text="Run" class="Button_Normal"  OnClientClick="javascript:return ShowDialogRun();" Width ="65px"/>&nbsp;
                                <asp:Button ID="btnDiscardDARImportFile" runat="server" Text="Discard" class="Button_Normal" OnClientClick="javascript:return ShowDialogDiscard();"/>&nbsp;
                                <asp:Button ID="btnClose" runat="server" Text="Close" class="Button_Normal" CssClass="Button_Normal Warn_Dirty" Width ="75px"   />
                            </td>
                        </tr>
                  </table>
            </div>

            <div id="divErrorDetails" runat="server" style="display: none; overflow: auto;">
                <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                                <table id="tblErrorDetailsEachRecord" style="width:100%; height:100%; BORDER-COLLAPSE: collapse" cellSpacing=0  rules=all border=1   class ="DataGrid_Grid">
                                    <thead class="DataGrid_HeaderStyle" >
                                        <th>Fund No.</th>
                                        <th>Reason</th>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /> <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button class="Button_Normal" ID="Button2" runat="server" Width="80" Text="OK" OnClientClick="javascript:return ClossErrorDetailsDialog();"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div> 
            <div id="ConfirmDialogRun" title="DAR Import File" style="display: none;">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessageRun">
                                        Are you sure you want to process the imported DAR file?
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
                                    <asp:Button ID="btnConfirmDialogYesRun" runat="server"  Text="Yes" cssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="closeDialogRun('ConfirmDialogRun');"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNoRun" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialogRun('ConfirmDialogRun');" />
                                </td>
                            </tr>
                        </table>
                    </div>
            </div> 


           <div id="ConfirmDialogDiscard" title="DAR Import File" style="display: none;">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img1" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessageDiscard">
                                        Are you sure you want to discard the imported DAR file?
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="tr1">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button ID="btnConfirmDialogYesDiscard" runat="server"  Text="Yes" cssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="closeDialogDiscard('ConfirmDialogDiscard');"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNoDiscards" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialogDiscard('ConfirmDialogDiscard');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div> 

      <%-- </ContentTemplate>   
         <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnImportDARBankResponseFile" EventName="Click" />--%>
         <%--</Triggers>
    </asp:UpdatePanel>--%>--%>


</asp:Content>