<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VoidAndTransferAnnuity.aspx.vb" Inherits="YMCAUI.VoidAndTransferAnnuity" EnableEventValidation ="false"  MasterPageFile="~/MasterPages/YRSMain.Master" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<title>
    YMCA YRS
</title>
<script language="javascript" type="text/javascript" src="JS/YMCA_JScript.js"></script>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>

   <style type="text/css">       
        /*
        CSS added by Sanjay R. YRS 5.0-1328 : Void and Transfer Annuity
        */
        .BG_Color_DisbursementExist
        {
            background-color: #f08080; /*Red*/
        }        
    </style>



<script language="javascript" type="text/javascript">


    function ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector) {
        var totalCheckboxes = $(checkBoxSelector),
                    checkedCheckboxes = totalCheckboxes.filter(":checked"),
                    noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                    allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
        if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
        $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
    }

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (args.get_error() == undefined) {
                BindEvents();
            }
        }
        BindEvents();

        $('#ConfirmDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            width: 600, height: 200,
            title: "Void and Transfer Annuity",
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });

        $('#divMessage').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            width: 350, height: 250,
            title: "Void and Transfer Annuity",
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });

    });

    function showDialog(id, text, btnokvisibility) {
        $('#' + id).dialog({ modal: true });
        $('#lblMessage1').text(text)
        $('#' + id).dialog("open");
        $("#btnYes").show();
        $("#btnNo").show();      

    }


    function showDivMessage(id, text) {
        $('#' + id).dialog({ modal: true });
        $('#lblMessage2').text(text);
        $('#' + id).dialog("open");
        $("#btnOK").show();
        $('#lblMessage2').text(text);
    }

    function closeDialog(id) {

        $('#' + id).dialog('close');
    }

    function disableButton() {
        $('#lblMessage1').text('Processing your request...');
        $("#btnYes").hide();
        $("#btnNo").hide();    

    }
        

    function BindEvents() {
        //Code for checkbox Check all, start
        var allCheckBoxSelector = '#<%=gvDisbursement.ClientID%> input[id*="chkSelectAll"]:checkbox';
        var checkBoxSelector = '#<%=gvDisbursement.ClientID%> input[id*="chkSelect"]:checkbox';

        $(allCheckBoxSelector).bind('click', function () {
            $(checkBoxSelector).attr('checked', $(this).is(':checked'));
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
        });

//        $(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector
        ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
        $(checkBoxSelector).bind('click', function () {
            mark_dirty();
        });

    }


    function ValidateNumericSSNO(str) {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {

            event.returnValue = false;
        }
        if ((event.keyCode == 80) && str.length == 0) {
            event.returnValue = true;
        }
        if (event.keyCode == 45) {
            event.returnValue = true;
        }
    }

    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }

 

    <%--  'SP 2014.08.27   YRS 5.0-2279  - Start
    function isValidEmail(str) {
       
        
        var at = "@"
        var dot = "."
        var lat = str.indexOf(at)
        var lstr = str.length
        var ldot = str.indexOf(dot)
        if (str.length > 0) {
            if (str.indexOf(at) == -1) {
                alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID')%>")
                return false
            }
            if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
            alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID")%>')%>")
                return false
            }
            if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
            alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID")%>') %>")
                return false
            }
            if (str.indexOf(at, (lat + 1)) != -1) {
            alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID")%>') %>")
                return false
            }
            if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
            alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID")%>')%>")
                return false
            }
            if (str.indexOf(dot, (lat + 2)) == -1) {
            alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID")%>') %>")
                return false
            }
            if (str.indexOf(" ") != -1) {
            alert('<%= GetMessage("MESSAGE_VTA_INVALID_EMAILID")%>') %>")
                return false
            }
        }
        return true
    }
    'SP 2014.08.27   YRS 5.0-2279  - End --%>

    </script>

    <script language="javascript" type="text/javascript">

        function OpenPopUp(Formname) {
            var left = (screen.width / 2) - (750 / 2);
            var top = (screen.height / 2) - (450 / 2);

            window.open('VoidAndTransferAnnuity_SearchBeneficiary.aspx?form', 'YMCAYRS', 'width=800, status=yes, height=557, menubar=no, resizable=no,top=' + top + ',left=' + left + ', scrollbars=yes', '');
        }   
       
    </script>


</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" >


        
<div class="Div_Center" style="height: 500px;">

<asp:ScriptManagerProxy   id="dbScriptManagerProxy" runat="server"> 
</asp:ScriptManagerProxy> 

<table cellspacing="0" cellpadding="0" width="100%">   
         <tr>
            <td>
                <iewc:TabStrip ID="tabStripVoidandTransferAnnuity" runat="server" Width="100%" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                    TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                    AutoPostBack="True" Height="30px">
                    <iewc:Tab Text="List" ></iewc:Tab>
                    <iewc:Tab Text="Disbursement"></iewc:Tab>
                </iewc:TabStrip>
            </td>
        </tr>        
        <tr>
            <td>
                <iewc:MultiPage ID="multiPageVoidandTransferAnnuity" runat="server">
                 <iewc:PageView>
                      <asp:UpdatePanel ID="upRetireeGrid" runat="server" UpdateMode="Conditional">
                      <ContentTemplate>     
                       <div class="Div_Center" >

                            <table class="Table_WithBorder"  width="100%" style="height: 500px;" >
                                <tr valign="top" >
                                    <td>
                                        <table width="100%">
                                            <tr valign="top" >
                                                <td align="left">
                                                    <asp:label id="lblNoDataFound" runat="server" visible="False" CssClass="Label_Large">No Matching Records</asp:label>
                                                      <div style="overflow: auto; width: 600px; height: 500px; text-align: left">
                                                                                          
                                                          <asp:GridView ID="gvSearchRetiree" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                              EditRowStyle-Wrap="true" EditRowStyle-Width="100px" EmptyDataRowStyle-Wrap="true"
                                                              FooterStyle-Wrap="true" HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true"
                                                              SelectedRowStyle-Wrap="true" Width="100%" SortedAscendingCellStyle-Wrap="true"
                                                              SortedDescendingCellStyle-Wrap="true" AllowSorting="True" AllowPaging="True"
                                                              PageSize="100" AutoGenerateColumns="False" DataKeyNames="intFundIdNo" PagerStyle-Font-Names="Arial"
                                                              PagerStyle-Font-Size="5px">
                                                              <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                              <RowStyle CssClass="DataGrid_NormalStyle" />
                                                              <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                              <AlternatingRowStyle CssClass = "DataGrid_AlternateStyle"/> 
                                                              <Columns>
                                                                  <asp:CommandField HeaderStyle-Width="20px" ButtonType="Image" Visible="true" ShowSelectButton="true"
                                                                      SelectImageUrl="images\select.gif" SelectText="Select" />
                                                                  <asp:BoundField HeaderText="SSN" DataField="SSN" ItemStyle-Width="70" SortExpression="SSN" />
                                                                  <asp:BoundField HeaderText="Last Name" DataField="LastName" SortExpression="LastName" />
                                                                  <asp:BoundField HeaderText="First Name" DataField="FirstName" SortExpression="FirstName" />
                                                                  <asp:BoundField HeaderText="Middle Name" DataField="MiddleName"  SortExpression="MiddleName" />
                                                                  <asp:BoundField HeaderText="Fund Status" DataField="FundStatus" ItemStyle-Width="115" SortExpression="FundStatus" />
                                                                  <asp:BoundField HeaderText="FundIDNo" DataField="FundIDNo" HeaderStyle-CssClass="hideGridColumn"
                                                                      ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                  <asp:BoundField HeaderText="chrStatusType" DataField="chrStatusType" HeaderStyle-CssClass="hideGridColumn"
                                                                      ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                  <asp:BoundField HeaderText="IsArchived" DataField="IsArchived" HeaderStyle-CssClass="hideGridColumn"
                                                                      ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                  <asp:BoundField HeaderText="Fund No" DataField="intFundIdNo" ItemStyle-Width="80" SortExpression="intFundIdNo"></asp:BoundField>
                                                                  <asp:BoundField HeaderText="PersId" DataField="PersId" HeaderStyle-CssClass="hideGridColumn"
                                                                      ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                  <asp:BoundField HeaderText="DeathDate" DataField="DeathDate" DataFormatString="{0:MM/dd/yyyy}"
                                                                      HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                              </Columns>
                                                              <PagerSettings PageButtonCount="500" FirstPageText="First" LastPageText="Last" />
                                                              <PagerStyle Font-Names="Arial" Font-Size="Small" />
                                                          </asp:GridView>

                                                          <asp:label id="lbl_Search_MoreItems" runat="server" cssclass="Message" visible="False"
                                                            enableviewstate="False" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="Label_Small">Fund No.</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="txtRetireeFundNo" runat="server" width="150" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="Label_Small">SS No.</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="txtRetireeSSNo" runat="server" width="150" cssclass="TextBox_Normal" MaxLength ="9"></asp:textbox>
                                                                <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" errormessage="Invalid SSN"
                                                                    controltovalidate="TextBoxSSNo" validationexpression="\d{3}-\d{2}-\d{4}|\d{9}|[A-Z]\d{8}"
                                                                    enabled="False" visible="false"></asp:regularexpressionvalidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span Class="Label_Small">Last Name</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="txtRetireeLastName" runat="server" width="150" MaxLength ="30" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span Class="Label_Small">First Name</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="txtRetireeFirstName" runat="server" width="150" MaxLength ="20" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                               <asp:button id="btnFind" runat="server" text="Find" width="80" cssclass="Button_Normal" CausesValidation ="false"></asp:button>                                                                
                                                            </td>
                                                            <td align="right">
                                                                <asp:button id="btnClear" runat="server" text="Clear" width="80" cssclass="Button_Normal"></asp:button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>                                          
                                                 <td align="right" class="Td_ButtonContainer">
                                                    <asp:button id="btnCloseList" runat="server" text="Close" width="80" cssclass="Button_Normal"></asp:button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="gvSearchRetiree"  />                                       
                           <asp:AsyncPostBackTrigger ControlID="btnFind" EventName="Click" /> 
                            </Triggers>
                        </asp:UpdatePanel>
                    </iewc:PageView>                
                    <iewc:PageView>
                    <div class="Div_Center" >                         
                          <table class="Table_WithBorder" border="0" width="100%" cellpadding="0" cellspacing="1"  >
                          <tr> <td colspan="4" valign="top"  class="Td_ButtonContainer"> Select check(s) to Transfer </td></tr>
						    <tr>
							    <td colspan="4" valign="top">
                                <table class="Table_WithBorder" width="100%">
                                    <tr> 
                                        <td >
                                          <asp:label id="lblNoDisbursement" runat="server" visible="False" CssClass="Label_Large">No Records found.</asp:label>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td>                                        
                                       <asp:UpdatePanel ID="upDisbursementgrid" runat="server" UpdateMode="Conditional">
                                         <ContentTemplate>                                  
								            <div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 250px; TEXT-ALIGN: left; valign: top">							
								
                                            <asp:GridView ID="gvDisbursement" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                        EditRowStyle-Wrap="true" EditRowStyle-Width="100px" EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true"
                                                        HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true" SelectedRowStyle-Wrap="true"
                                                        Width="100%" SortedAscendingCellStyle-Wrap="true" SortedDescendingCellStyle-Wrap="true"
                                                        AllowSorting="True" AllowPaging="True" PageSize="8" 
                                                        AutoGenerateColumns="False" DataKeyNames="DisbursementID" PagerStyle-Font-Names="Arial"
                                                        PagerStyle-Font-Size="5px">
                                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingRowStyle CssClass = "DataGrid_AlternateStyle"/> 
                                                        <Columns>
                                                           <asp:TemplateField HeaderStyle-Width="20px" >
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server"  />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="20px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Number" DataField="Number" ItemStyle-Width="120" SortExpression="Number"/>
                                                            <asp:BoundField HeaderText="Account Date" DataField="AccountDate" ItemStyle-Width="180" DataFormatString="{0:MM/dd/yyyy}" SortExpression="AccountDate" ReadOnly="true" ShowHeader="true" Visible="true" />
                                                            <asp:BoundField HeaderText="Issued Date" ItemStyle-Width="100" DataField="IssuedDate" DataFormatString="{0:MM/dd/yyyy}" SortExpression="IssuedDate" />
                                                            <asp:BoundField HeaderText="Amount" ItemStyle-Width="100" DataField= "Amount"  SortExpression="Amount" />                                                       
                                                            <asp:BoundField HeaderText="Pay Method" ItemStyle-Width="100" DataField= "PayMethod"  SortExpression="PayMethod" />                                                       
                                                        </Columns>
                                                        <PagerSettings PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:GridView>     
								            </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvDisbursement" EventName="PageIndexChanging" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDisbursement" EventName="Sorting" />                                               
                                                <%-- <asp:PostBackTrigger ControlID="btnYes" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDisbursement" EventName="PageIndexChanging" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDisbursement" EventName="Sorting" />
                                                <asp:PostBackTrigger ControlID="btnClose" />--%>
                        
                                            </Triggers>
                                         </asp:UpdatePanel>
                                             
                                        </td>
                                    </tr>  
                                    <tr >
                                        <td >
                                            <asp:Label ID="lblcolorcode" Runat="server" CssClass="Label_Small" Width = "30px"  visible ="false"></asp:Label>
                                            &nbsp;&nbsp;<asp:Label ID="lblDisbAfterDeath" Runat="server" CssClass="Label_Small" Width = "500px" visible ="false" ></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRequiedFieldIcon"  CssClass="Label_Small" Visible="true" Width = "15px" runat="server" Text = "*" ForeColor="#FF0000" ></asp:Label>                                                            
                                             <asp:Label ID="lblRequiedField"  CssClass="Label_Small" Visible="true" Width = "200px" runat="server" Text = "Required Fields"></asp:Label>
                                        </td>
                                    </tr>                          
                                </table>								
							    </td>
						    </tr>
                            <tr>
                                <td colspan="2" valign="middle"   class="Td_ButtonContainer" width ="50%">                               
                                      Add, Edit or Search - Add existing payee
                                </td>
                                <td colspan="2" align ="right" class="Td_ButtonContainer"  width ="50%">
                                     <asp:button id="btnSearchPayee" runat="server" text="Search Payee..." width="160" cssclass="Button_Normal"></asp:button>
                                     <asp:button id="btnAddPayee" runat="server" text="Add Payee" width="80" cssclass="Button_Normal"></asp:button>
                                     <asp:button id="btnCancel" runat="server" text="Cancel" width="80" cssclass="Button_Normal"></asp:button>
                                </td>                           
                            </tr>                            
                            <tr>
                                <td colspan ="4" >
                                <table class="Table_WithBorder" width="100%">
                                        <tr>
                                            <td width="50%">
                                                <table  width="100%">
                                                    
                                                    <tr>
                                                        <td width="10%">
                                                              <asp:Label ID="lblSSNo" CssClass="Label_Small" Width="50px" runat="server">SS No.</asp:Label> 
                                                              <asp:Label ID="lblSSNo_validation" CssClass="Label_Small" ForeColor="#FF0000" runat="server">*</asp:Label>                                                          
                                                        </td>
                                                        <td align="left" height="26" width="90%">
                                                            <asp:TextBox ID="txtSSNo" runat="server" CssClass="TextBox_Normal Warn" Width="180px" MaxLength="9" autopostback="true"></asp:TextBox>                                                                                                                
                                                        </td>                                                 
                                                    </tr>
                                                    <tr>
                                                        <td align="left" height="23px" width="10%">
                                                            <asp:label ID="lblSalutation" cssclass="Label_Small" runat="server" text="Sal."></asp:label>
                                                        </td>
                                                        <td align="left" height="23px" width="90%">
                                                                <asp:dropdownlist id="ddlSalutaion" cssclass="DropDown_Normal Warn" runat="server" width="180px">
												                    <asp:ListItem Value="" selected="true"></asp:ListItem>
												                    <asp:ListItem Value="Dr.">Dr.</asp:ListItem>
												                    <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
												                    <asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
												                    <asp:ListItem Value="Ms.">Ms.</asp:ListItem>
											                    </asp:dropdownlist>
                                                        </td>
                                                   </tr>
                                                   <tr>
                                                        <td align="left" height="23px" width="20%">
                                                            <asp:label cssclass="Label_Small" id="lblFirstName" runat="server" text="First Name"></asp:label>
                                                             <asp:Label ID="lblFName_validation" CssClass="Label_Small" ForeColor="#FF0000" runat="server">*</asp:Label>                                          
                                                        </td>
                                                        <td align="left" height="23px" width="80%">
                                                            <asp:textbox cssclass="TextBox_Normal Warn" id="txtFirstName" runat="server" MaxLength ="20" width="180px"></asp:textbox> 
                                                        
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" height="23px" width="20%">
                                                            <asp:label cssclass="Label_Small" id="lblMiddleName" runat="server" text="Middle Name"></asp:label>
                                                        </td>
                                                        <td align="left" height="23px" width="80%">
                                                            <asp:textbox cssclass="TextBox_Normal Warn" id="txtMiddleName" runat="server" width="180px" MaxLength ="20"></asp:textbox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" height="23px" width="20%">
                                                            <asp:label cssclass="Label_Small" id="lblLastName" runat="server" text="Last Name"></asp:label>
                                                            <asp:Label ID="lblLName_validation" CssClass="Label_Small" ForeColor="#FF0000" runat="server">*</asp:Label>
                                                        </td>
                                                        <td align="left" height="23px" width="80%">
                                                            <asp:textbox cssclass="TextBox_Normal Warn" id="txtLastName" runat="server" width="180px" MaxLength ="30"></asp:textbox> 
                                                        </td>                                                
                                                     </tr> 
                                                     <tr>
                                                        <td align="left" height="23px" width="20%">
                                                            <asp:label cssclass="Label_Small" id="lblSuffix" runat="server" text="Suffix"></asp:label>
                                                        </td>
                                                        <td align="left" height="23px" width="80%">
                                                            <asp:textbox cssclass="TextBox_Normal Warn" id="txtSuffix" runat="server" width="100px" maxlength="6"></asp:textbox>
                                                        </td>    
                                                      </tr> 
                                                      <tr>
                                                        <td align="left" nowrap width="20%">
                                                            <asp:label cssclass="Label_Small" id="lblBirthDate" runat="server" text="Birth Date"></asp:label>
                                                             <asp:Label ID="lblBithdate_validation" CssClass="Label_Small" ForeColor="#FF0000" runat="server">*</asp:Label>
                                                        </td>
                                                        <td align="left" nowrap width="80%">                                                       
                                                           <YRSControls:DateUserControl ID="txtBirthDate" runat="server" CssClass="TextBox_Normal DateControl"></YRSControls:DateUserControl>                                                           
                                                        </td>
                                                    </tr>
                                                </table>
                                              </td> 
                                              <td width="50%" valign ="top">
                                                <table width="100%" valign ="top">
                                                    <tr>
                                                        <td valign ="top" width="5px">
                                                        <asp:Label ID="lblAddressRequiredField" CssClass="Label_Small" ForeColor="#FF0000" runat="server">*</asp:Label>
                                                       </td>
                                                        <td valign ="top" >                                                        
                                                            <NewYRSControls:New_AddressWebUserControl runat="server" ID="ucPayeeAddress" AllowNote="true" AllowEffDate="false" PopupHeight="930" IsPrimary= "1" AddrCode = "HOME" EntityCode = "PERSON" ClientIDMode ="Predictable"   />        
                                                        </td>
                                                    </tr>
                                                    
                                                  
                                                    
                                                   <%-- <tr>    
                                                         <td  align="left">
                                                            <table align="left">
                                                               <tr>
                                                                    <td align="left" width="105px">
                                                                        <asp:label cssclass="Label_Small" runat="server" id="lblEmail" text="E-mail Addr"></asp:label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:textbox cssclass="TextBox_Normal" id="txtEmail" runat="server" width="150px"></asp:textbox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="60">
                                                                        <asp:label cssclass="Label_Small" id="lblTelephone" runat="server" 
                                                                            text="Telephone"></asp:label>
                                                                    </td>
                                                                    <td align="left">
                                                                            <asp:textbox cssclass="TextBox_Normal" id="txtTelephone" runat="server" width="150px"
                                                                                maxLength="15" ></asp:textbox>
                                                                    </td>
                                                                </tr>
                                                              </table>
                                                          </td>
                                                      </tr>--%>
                                                </table>
                                                <table width="100%" valign ="top">
                                                <tr>
                                                        <td valign="top">
                                                               <asp:label cssclass="Label_Small" runat="server" id="lblEmail" text="E-mail Addr"></asp:label>
                                                        </td>
                                                        <td valign="top">
                                                               <asp:textbox cssclass="TextBox_Normal Warn" id="txtEmail" runat="server" width="190px"></asp:textbox>
                                                        </td>
                                                     </tr>
                                                     <tr>
                                                         <td valign="top" >
                                                            <asp:label cssclass="Label_Small" id="lblTelephone" runat="server" text="Telephone"></asp:label>
                                                         </td>
                                                         <td valign="top">
                                                             <asp:textbox cssclass="TextBox_Normal Warn" id="txtTelephone" runat="server" width="150px" maxLength="25" ></asp:textbox>
                                                         </td>
                                                      </tr>
                                                
                                                </table>
                                          
                                              </td>
                                                                       
                                        </tr>
                          
                                  </table> 
                               </td>
                            <tr>    
                                     
                                <td align="center" class="Td_ButtonContainer" colspan = "2">
                                    <asp:button id="btnTransfer" runat="server" text="Save & Transfer" width="160"  cssclass="Button_Normal" ></asp:button>
                                </td>
                                <td align="center" class="Td_ButtonContainer" colspan = "2">
                                    <asp:button id="btnClose" runat="server" text="Close" width="80" cssclass="Button_Normal Warn_Dirty"  CausesValidation="False"></asp:button>
                                </td>  
                                                                                                                       
                            </tr>  
                          </table>                         
                      </div>                    
                      </iewc:PageView>  
                    </iewc:MultiPage>                 
                    </td>        
        </tr>
                        
   </table>
 
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>


<div id="ConfirmDialog" style="overflow: visible;">
    <asp:UpdatePanel ID="upConfirmationDiv1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage1" CssClass="Label_Small" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
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
            <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnClose" />
            <asp:PostBackTrigger ControlID="btnYes" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
 

    <div id="divMessage" style="overflow: visible;">
        <asp:UpdatePanel ID="upConfirmationDiv2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage2" CssClass="Label_Small" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">
                                <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />&nbsp;
                            </td>
                        </tr>
                        
                    </table>
                </div>
                
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
       
    </div>
</asp:Content>



