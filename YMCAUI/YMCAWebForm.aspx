<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="YMCAWebForm.aspx.vb" Inherits="YMCAUI.YMCAWebForm" %>

<%-- START: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%-- END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>

<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>
    <%-- Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 --%>
<%@ Import Namespace ="YMCARET.YmcaBusinessObject.MetaMessageBO" %> 
<%@ Import Namespace="YMCAObjects.MetaMessageList" %>
    <%-- End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 --%>
<html>
<head>
    <title>YMCA YRS
    </title>
    <!--Included by Shashi Shekhar:2009-12-23:To use the common js function -->
    <script language="javascript" src="JS/YMCA_JScript.js"></script>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <!-- START : SC | 2019.05.13 | YRS-AT-2601 | Adding .js file for disabled controls and upgrading .js library-->
    <!--<script src="JS/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>-->    
    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>  
    <script type="text/javascript" src="JS/YMCA_JScript_DisableTextBox.js"></script>
    <!-- END : SC | 2019.05.13 | YRS-AT-2601 | Adding .js file for disabled controls and upgrading .js library-->
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    <!--Included by Prasad Jadhav:2011-08-26:For BT-895,YRS 5.0-1364 : prompt user to if changes not saved -->
    <script language="javascript" src="JS/YMCA_JScript_Warn.js"></script>
    <script>
        /* Shashi Shekhar:2009-12-31: comment and shift ValidateNumeric(controlname) in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */
        //Anudeep A         2012.09.26       BT:1050 :YRS 5.0-1621: new field and YRS display for when a Y started using Who's Where
        function FormatYMCANo() {
            var diff;
            var flg = false;
            var str = String(document.Form1.all.TextBoxYMCANo.value);
            var len;
            len = str.length;
            if (len == 0) {
                return false;
            }
            else if (len < 6) {
                diff = (6 - len);
                for (i = 0; i < diff; i++) {
                    str = "0" + str

                }
            }
            document.Form1.all.TextBoxYMCANo.value = str
        }
        //Added by Swopna in response to YREN-4125 on 3 Jan,2008
        //********
        function FormatYMCANoGeneral() {
            var diff;
            var flg = false;
            var str = String(document.Form1.all.TextBoxBoxYMCANo.value);
            var len;
            len = str.length;
            if (len == 0) {
                return false;
            }
            else if (len < 6) {
                diff = (6 - len);
                for (i = 0; i < diff; i++) {
                    str = "0" + str

                }
            }
            document.Form1.all.TextBoxBoxYMCANo.value = str
        }
        /* Shashi Shekhar:2009-12-31: comment and shift CheckAccess(controlname) in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */
        function CheckAccessEMail(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");


                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                window.open('YMCANetWebForm.aspx', 'YMCAYRS', 'width=750,height=550,menubar=no,Resizable=Yes,top=120,left=120,scrollbars=yes')

                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessOfficersAdd(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);

            //alert(controlname)
            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");

                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //    window.open('AddOfficerWebForm.aspx','CustomPopUp','width=750,height=550,menubar=no,Resizable=Yes,top=120,left=120,scrollbars = yes') 


                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessOfficersUpdate(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //  window.open('AddOfficerWebForm.aspx','CustomPopUp','width=750,height=550,menubar=no,Resizable=Yes,top=120,left=120,scrollbars = yes')



                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessResoAdd(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //  window.open('AddResolutionWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')




                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessResoUpdate(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //    window.open('AddResolutionWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')





                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessContactsAdd(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //window.open('AddContactWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')


                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessContactsUpdate(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //window.open('AddContactWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')

                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessBankInfoAdd(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //  window.open('UpdateBankInformationWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')

                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessBankInfoUpdate(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //   window.open('UpdateBankInformationWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')

                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }


        function CheckAccessAddRelationManager(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);


            if (str.match(controlname) != null) {

                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {
                //  window.open('UpdateBankInformationWebForm.aspx','CustomPopUp','width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes')
                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }



        function EnableSaveCancel() {

            document.Form1.all.ButtonSave.disabled = false;
            document.Form1.all.ButtonCancel.disabled = false;
            document.Form1.all.ButtonAdd.disabled = true;
            document.Form1.all.ButtonOk.disabled = true;
        }
        /*Start: Anudeep A 2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */
        $(document).ready(function () {
            OpenProgressDialog();
            LoadWaivedParticipantsList();  <%-- Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>
        });
        function OpenProgressDialog() {
            $('#divProgress').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 150,
                title: "Saving Information",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }
        function ShowProcessingDialog() {
            $("#divProgress").dialog('open');
            $('#labelMessage').text('Please wait while information is being saved.');
        }
        /*End: Anudeep A 2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */
        <%-- START: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>
        function LoadWaivedParticipantsList() {
            $('#divWaivedParicipant').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 700, height: 500,
                title: "List of Waived Participants",
                open: function (type, data)
                {
                    $(this).parent().appendTo("form");
                }
            });
            // $('#divWaivedParicipant').dialog('open');
        }
        function CancelWaivedParticipantsListDialog() {
            $("#divWaivedParicipant").dialog('close');
        }
        function OpenWaivedParticipantslistDialog()
        {
            $(document).ready(function() {
                $("#divWaivedParicipant").dialog('open');
            });
        }
        <%-- END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>
    </script>
    <script type="text/javascript">
        //Set up the Modal dialog box
        $(document).ready (function() { 
            InitializeYRelationDialogBox();
            $("#gvRelationManagers input:checkbox").click(RelationManagerSelectionChanged);
            $("#<%=btnAddRelationManager.ClientID%>").click(YRelationEditButtonClick);
        });

        function RelationManagerSelectionChanged(){
            var name=GetConcatenatedRelationManagersName();
            $("#<%=lblRelationManagerName.ClientID%>").text(name.toString());
           
            }

            function GetConcatenatedRelationManagersName(){
                var name = '';
                $("#gvRelationManagers tr:has(:checkbox:checked) td:nth-child(2)").each(function () {
                    name += ($(this).text() + "/");
                });
                name = name.toString().slice(0, name.toString().length - 1);
                return name;
            }

            function InitializeYRelationDialogBox (){
                $("#<%=dvname.ClientID%>").dialog
            ({
                modal: true,
                autoOpen: false,
                title: "Select Y-Relation Manager",
                width: 350, height: 510,
                buttons: [{ text: "OK", click: AddRelationManagerName },
                          { text: "Cancel", click: CloseDialogRelationManager}]
            });
        }

        function CloseDialogRelationManager() {
            $.each($("#<%= gvRelationManagers.ClientID %> input[type='checkbox']"), function () { this.checked = false; });
            $("#<%=lblRelationManagerName.ClientID%>").text('');
            $("#<%=dvname.ClientID%>").dialog('close');
        }

        function AddRelationManagerName() {
            var hdRelationManagerselectionLimit = <%=intRelationManagerselectionLimit%>; 
            if ($("#<%=gvRelationManagers.ClientID %> input[type=checkbox]:checked").length > hdRelationManagerselectionLimit) {
                alert("You can select maximum " + hdRelationManagerselectionLimit + " names.");
                return false;
            }
            var name = GetConcatenatedRelationManagersName();
            if ($("#<%=txtRelationManager.ClientID%>").val() != name) {
                EnableSaveCancel()
            }
            $("#<%=txtRelationManager.ClientID%>").val(name.toString());
            $("#<%=hdrelationManagerName.ClientID%>").val(name.toString());
            CloseDialogRelationManager();
        }

        function YRelationEditButtonClick (){
            var chkSecurity = CheckAccessAddRelationManager( ('<%=btnAddRelationManager.ClientID%>'));
            if (chkSecurity==false){
                return false;
            }
            $("#<%=dvname.ClientID%>").dialog('open');
            return false;
        }
   
        <%-- Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 --%>

        function TabDisplay(hash) {
            if (hash != ''){
                $('.WTSM_tabContent').css('display', 'none');
                $(hash).css('display', 'block');    
                LoadWTSMTabs($("input[href='"+hash+"']")[0]);
            }
            
        }
        var confirmation = {shown:false};
        function ShowSaveConfirmDialog() {   
            $('#divSaveConfirmDialog').dialog({              
                title: "YMCA YRS - Confirmation",
                buttons: {
                    "Yes": function () {
                        window.confirmation.shown = true;
                        $(this).dialog("close");
                        ResetValues($('#<%=hdnOperationPerformed.ClientID%>'));
                        LoadWTSMTabs($("input[href='"+$('#<%=hdnSelectedTab.ClientID%>').val()+"']")[0]);                          
                    },
                    "No": function () {
                        window.confirmation.shown = false;
                        $(this).dialog("close");
                        LoadWTSMTabs($("input[href='"+$('#<%=hdnOperationPerformed.ClientID%>').val()+"']")[0]);                          
                        }
                }
            });
                $('#divSaveConfirmDialog').dialog('open');               
                $('#lblSaveMessage').text('<%=GetMessageByTextMessageNo(MESSAGE_YMCA_CONFIRM_DIALOG_MESSAGE_TABCHANGE)%>');          
            }

            function SaveFollowupStatus() {
           
                $('#divSaveConfirmDialog').dialog('close');
                return true;
            }

            function CloseSaveConfirmDialog() {
           
                $('#divSaveConfirmDialog').dialog('close');

                return false;
            
            }
            function ResetValues(hdnOperation){  
                var DateUserControl, DropdownPayrollDate, DropdownYerdiAccess, ValDropDownYMCANo; 
                if (hdnOperation.val() != ""){
                    switch(hdnOperation.val())
                    {
                        case '#tabContent_Withdraw':
                            DateUserControl = $('#<%=DateUserControlWithdrawDate.ClientID%>');
                            DropdownPayrollDate = $('#<%=DropdownPayrollDate_W.ClientID%>');
                            DropdownYerdiAccess = $('#<%=DropdownYerdiAccess_W.ClientID%>');
                          
                            DateUserControl.val("");
                            DropdownPayrollDate.val("");
                            DropdownYerdiAccess.val("0");                          
                            break;
                        case '#tabContent_Terminate':                           
                            DropdownPayrollDate = $('#<%=DropdownPayrollDate_T.ClientID%>');
                        DropdownYerdiAccess = $('#<%=DropdownYerdiAccess_T.ClientID%>');

                        DropdownPayrollDate.val("");
                        DropdownYerdiAccess.val("0");
                        break;
                    case '#tabContent_Merge':
                        DateUserControl = $('#<%=DateUserControlMergeDate.ClientID%>');
                        DropdownPayrollDate = $('#<%=DropdownPayrolldate_M.ClientID%>');
                        DropdownYerdiAccess = $('#<%=DropdownYerdiAccess_M.ClientID%>');
                        ValDropDownYMCANo = $('#<%=DropDownYMCANo.ClientID%>');

                        DateUserControl.val("");
                        DropdownPayrollDate.val("");
                        DropdownYerdiAccess.val("0");
                        ValDropDownYMCANo.val("0")
                        break;
                    case '#tabContent_Suspend':
                        DateUserControl = $('#<%=DateUserControlSuspensionDate.ClientID%>');
                        DropdownPayrollDate = $('#<%=DropdownPayrollDate_S.ClientID%>');
                        DropdownYerdiAccess = $('#<%=DropdownYerdiAccess_S.ClientID%>');

                        DateUserControl.val("");
                        DropdownPayrollDate.val("");
                        DropdownYerdiAccess.val("0");
                        break;
                    case '#tabContent_Reactivate':
                        DateUserControl = $('#<%=DateUserControlReactivateDate.ClientID%>');
                        DateUserControl.val("");
                        break;
                    default:
                }
                hdnOperation.val("");                                        
            }
        }
        function TabChange(_this) {            
            var hdnOperation = $('#<%=hdnOperationPerformed.ClientID%>');                     
            if (hdnOperation.val() != ""){
                ShowSaveConfirmDialog();                
                var _href = $(_this).attr('href'); 
                var hrefLen = _href.length;
                var hdd = $('#<%=hdnSelectedTab.ClientID%>');
                _href = _href.substring(0, hrefLen);
                $(hdd).val(_href);                  
            }
            else{
                LoadWTSMTabs(_this);
            }
          
        }
        function LoadWTSMTabs(_this){
            $('#tab_btnWithdrawClick').removeClass('tabSelected1');
            $('#tab_btnWithdrawClick').addClass('tabNotSelected1');

            $('#tab_btnTerminateClick').removeClass('tabSelected1');
            $('#tab_btnTerminateClick').addClass('tabNotSelected1');

            $('#tab_btnSuspendClick').removeClass('tabSelected1');
            $('#tab_btnSuspendClick').addClass('tabNotSelected1');

            $('#tab_btnMergeClick').removeClass('tabSelected1');
            $('#tab_btnMergeClick').addClass('tabNotSelected1');

            $('#tab_btnReactivateClick').removeClass('tabSelected1');
            $('#tab_btnReactivateClick').addClass('tabNotSelected1');

            $(_this).addClass('tabSelected1');   
            
            var hdnSel = $('#<%=hdnSelectedTab.ClientID%>');           
            

            hdnSel.val($(_this).attr('href'));
            $('.WTSM_tabContent').css('display', 'none');          
            var _href = $(_this).attr('href');          
            $(_href).css('display', 'block');           
            var hrefLen = _href.length;
            var hddLoadTabSelected = $('#<%=hdnSelectedTab.ClientID%>');
            _href = _href.substring(0, hrefLen);
            $(hddLoadTabSelected).val(_href);          
        }
        function SetOperation(_this)
        {            
            var hdnOperation = $('#<%=hdnOperationPerformed.ClientID%>');            
            var hdd = $('#<%=hdnSelectedTab.ClientID%>');
            hdnOperation.val(hdd.val());
            EnableSaveCancel();
        }
        function BindEvents() {
            $(".tabNotSelected1").mouseover(function () {               
                //$(this).css("background-color", "#ffcc33");            
                $(this).removeClass('tabNotSelected1');
                $(this).addClass('tabSelected1');
            }).mouseout(function () {
                //$(this).css("background-color", "#FFFFFF");
                var obj = $('#<%=hdnSelectedTab.ClientID%>')
                if (obj.val() != $(this).attr('href'))
                {
                    $(this).addClass('tabNotSelected1');
                    $(this).removeClass('tabSelected1');
                }
            });              

        }

      
        $(document).ready(function () {          
            BindEvents();
            //BindEvents1();
           
        });
        <%-- End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 --%>
        
         </script>
</head>
<body>
    <%--<asp:ScriptManager ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManager>--%>
    <form id="Form1" method="post" runat="server">
        <div class="Div_Center">
            <table class="Table_WithoutBorder" cellspacing="0" width="700">
                <tr>
                    <td style="text-align: center">
                        <YRSControls:YMCA_Toolbar_WebUserControl ID="YMCA_Toolbar_WebUserControl1" runat="server"
                            ShowLogoutLinkButton="true" ShowHomeLinkButton="true" ShowReleaseLinkButton="false"></YRSControls:YMCA_Toolbar_WebUserControl>
                    </td>
                </tr>
                <tr>
                    <td class="Td_BackGroundColorMenu" align="left">
                        <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False"
                            Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                            DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                            mouseovercssclass="MouseOver">
                            <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                        </cc1:Menu>
                    </td>
                </tr>
                <tr>
                    <td class="Td_HeadingFormContainer" valign="top" align="left">
                        <img title="image" height="10" alt="image" src="images/spacer.gif" width="10">
                        YMCA Information<asp:Label ID="LabelHdr" Height="50%" runat="server" CssClass="Td_HeadingFormContainer"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" bgcolor="red">
                        <asp:Label ID="LabelPriorityHdr" runat="server" Font-Bold="True" Font-Size="12px"
                            Visible="false" ForeColor="White">Priority Handling</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummaryYMCA" runat="server" CssClass="Error_Message"></asp:ValidationSummary>
                    </td>
                </tr>
            </table>

            <table class="td_withoutborder" width="700">
                <tbody>
                    <tr>
                        <td>
                            <iewc:TabStrip ID="YMCATabStrip" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                                TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:center;" TabSelectedStyle="background-color:#93BEEE;color:#000000;text-align:center;"
                                Width="700px" Height="30px">
                                <iewc:Tab Text="List"></iewc:Tab>
                                <iewc:Tab Text="General"></iewc:Tab>
                                <iewc:Tab Text="Officers"></iewc:Tab>
                                <iewc:Tab Text="Contacts"></iewc:Tab>
                                <iewc:Tab Text="Branches" Enabled="False"></iewc:Tab>
                                <iewc:Tab Text="Resolutions"></iewc:Tab>
                                <iewc:Tab Text="Bank&amp;nbsp;Info."></iewc:Tab>
                                <iewc:Tab Text="Notes"></iewc:Tab>
                                <iewc:Tab Text="Documents"></iewc:Tab>
                                <%--<iewc:Tab Text="W/T/M"></iewc:Tab>--%>
                                <iewc:Tab Text="W/T/S/M"></iewc:Tab>
                                <%--Manthan YRS-AT-2334 --%>
                            </iewc:TabStrip>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <iewc:MultiPage ID="MultiPageYMCA" runat="server">
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="700" height="350">
                                        <tr valign="top">
                                            <td valign="top">
                                                <div style="overflow: scroll; width: 490px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCA" runat="server" Width="470" CellPadding="1" CellSpacing="0"
                                                        CssClass="DataGrid_Grid" AllowSorting="True" OnSortCommand="SortCommand_OnClick"
                                                        SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
                                                                        CommandName="Select" ToolTip="Select"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="Label_Small">YMCA No:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="100" runat="server" ID="TextBoxYMCANo" CssClass="TextBox_Normal"
                                                                MaxLength="6"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_Small">Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="100" runat="server" ID="TextBoxName" CssClass="TextBox_Normal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_Small">City:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="100" runat="server" ID="TextBoxCity" CssClass="TextBox_Normal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_Small">State:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="100" runat="server" ID="TextBoxState" CssClass="TextBox_Normal"></asp:TextBox>
                                                        </td>
                                                        <tr>
                                                        </tr>
                                                        <td>
                                                            <asp:Button Width="80" runat="server" ID="ButtonFind" Text="Find" CssClass="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button Width="80" runat="server" ID="ButtonClear" Text="Clear" CssClass="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="700" align="center" border="0" cellspacing="0">
                                        <%--<tr valign="top">
									<td align="left" valign="top" colspan="4">
										<table width="100%" cellspacing="0">--%>
                                        <tr>
                                            <td class="td_Text">General
                                            </td>
                                            <td class="td_Text" align="left">
                                                <asp:Label ID="LabelCurrentStatus" runat="server" CssClass="Label_Small" Width="200px"
                                                    Visible="true" ForeColor="red" BackColor="#ffcc33"></asp:Label>
                                            </td>
                                        </tr>
                                        <%--</table>
										
									</td>
								</tr>--%>
                                        <!--General-->
                                    </table>
                                    <table class="Table_WithBorder" width="100%" style="height: 25%" cellspacing="0" border="0" align="center">
                                        <tr>
                                            <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                            <td align="left" class="Label_Small">YMCA Name:
                                            </td>
                                            <td align="left" colspan="3" class="Label_Small">
                                                <asp:TextBox ID="TextBoxYMCAName" Enabled="false" runat="server" CssClass="TextBox_Normal Warn" Width="290"></asp:TextBox>
                                                <asp:Label ID="LabelYMCANameErrMsg" runat="server" CssClass="Label_Small" Width="180px"
                                                    Visible="false" ForeColor="red">YMCA Name cannot be blank</asp:Label>
                                                <asp:RequiredFieldValidator ID="requiredfieldvalidator2" runat="server" ControlToValidate="TextBoxYMCAName" ErrorMessage="YMCA Name cannot be blank">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="ButtonEditFedTaxId" runat="server" Width="70px" Text="Edit" CssClass="Button_Normal"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small">YMCA No:
                                            </td>
                                            <td width="246" colspan="4">

                                                <asp:TextBox ID="TextBoxBoxYMCANo" runat="server" Enabled="false" Width="100px" CssClass="TextBox_Normal Warn"
                                                    ReadOnly="true" MaxLength="6"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="requiredfieldvalidator10" runat="server" ControlToValidate="TextBoxBoxYMCANo" ErrorMessage="YMCA No cannot be blank" Display="Static">*</asp:RequiredFieldValidator>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small" width="130">Enrollment Date:
                                            </td>
                                            <td align="left" colspan="4">
                                                <uc1:DateUserControl ID="TextBoxEnrollmentDate" RequiredValidatorErrorMessage="Enrollment Date cannot be blank" runat="server"></uc1:DateUserControl>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small">Fed. Tax Id:
                                            </td>
                                            <td align="left" colspan="4">
                                                <asp:TextBox ID="TextBoxFedTaxId" runat="server" Width="100px" CssClass="TextBox_Normal Warn"
                                                    MaxLength="9" Enabled="false"></asp:TextBox>

                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxFedTaxId"
                                                    ErrorMessage="Fed. Tax Id should be numeric" Display="Dynamic" CssClass="Error_Message" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator>

                                            </td>
                                        </tr>
                                    </table>
                                    <table class="Table_WithBorder" width="100%" style="height: 70%; border-top: none;" align="center" cellspacing="0">
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelPriority" runat="server" CssClass="Label_Small">Priority Handling</asp:Label>
                                                <td align="left">
                                                    <asp:CheckBox ID="CheckboxPriority" CssClass="Warn" runat="server"></asp:CheckBox>
                                                </td>
                                            </td>
                                            <td align="left" width="150" class="Label_Small">State Tax Id:
                                            </td>
                                            <td align="left">
                                                <%-- added by prasad Warn class--%>
                                                <asp:TextBox ID="TextBoxStateTaxId" runat="server" Width="150px" CssClass="TextBox_Normal Warn"
                                                    MaxLength="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                <%-- Start: Bala: 19/01/2016: YRS-AT-2398: YNAN, MidMajor, Eligible Tracking User check box control added--%>
                                <td align="left">
									<asp:label id="LabelYNAN" runat="server" cssclass="Label_Small">YNAN</asp:label>
								</td>
                                <td align="left">
								    <asp:checkbox id="CheckboxYNAN" cssclass="Warn" runat="server"></asp:checkbox>
                                </td>
                                <td align="left">
									<asp:label id="LabelEligibleTrackingUser" runat="server" cssclass="Label_Small">Eligible Tracking User:</asp:label>
								</td>
                                <td align="left">
								    <asp:checkbox id="CheckboxEligibleTrackingUser" cssclass="Warn" runat="server"></asp:checkbox>
                                </td>
							</tr>
                            <tr>
                                <td align="left">
									<asp:label id="LabelMidMajor" runat="server" cssclass="Label_Small">Mid-Major</asp:label>
								</td>
                                <td align="left">
								    <asp:checkbox id="checkboxMidMajors" cssclass="Warn" runat="server"></asp:checkbox>
                                </td>	
                                <%-- End: Bala: 19/01/2016: YRS-AT-2398: YNAN, MidMajor, Eligible Tracking User check box control added--%>
							<!-- Added by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                            <td align="left" class="Label_Small" width="128px" height="35px">Eligibility Tracker Enrollment Date:
                                            </td>
                                            <td align="left" colspan="3">
                                                <asp:Label ID="lblEligibilityTrackerEnrollmentDate" runat="server" Width="60px" CssClass="Label_Small"
                                                    readonly="true"></asp:Label>
                                            </td>
                                            <!-- Added by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small">Payment Method:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DropDownPaymentMethod" runat="server" Width="200" CssClass="DropDown_Normal Warn"></asp:DropDownList>
                                            </td>
                                            <td align="left" class="Label_Small">Billing Method:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DropDownBillingMethod" runat="server" Width="150px" CssClass="DropDown_Normal Warn"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small">Hub Type:
                                            </td>
                                            <td align="left" colspan="3">
                                                <asp:DropDownList ID="DropDownHubType" runat="server" Width="200" CssClass="DropDown_Normal Warn"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="">---Select---</asp:ListItem>
                                                    <asp:ListItem Value="I">Independent</asp:ListItem>
                                                    <asp:ListItem Value="M">Metro</asp:ListItem>
                                                    <asp:ListItem Value="B">Branch</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small">Metro Name:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextBoxMetroName" runat="server" Width="200px" CssClass="TextBox_Normal"
                                                    ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:Button ID="ButtonMetro" runat="server" Width="96px" Text="Metro" CssClass="Button_Normal"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <NewYRSControls:New_AddressWebUserControl EnableControls="false" ID="AddressWebUserControlYMCA" runat="server" PopupHeight="530"
                                                    AllowNote="true" AllowEffDate="true" />
                                                <asp:Label ID="LabelAddressErrMsg" runat="server" Width="200px" CssClass="Label_Small"
                                                    Visible="false" ForeColor="red">Address cannot be blank</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="ButtonAdress" runat="server" Width="96px" Text="Address" CssClass="Button_Normal"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <%--<tr>
									<td align="left" class="Label_Small">
										Address1:
									</td>
									<td align="left">
										<asp:label id="TextBoxAddress1" runat="server" width="250px" cssclass="Label_Small"
											readonly="true"></asp:label>
                                        <asp:label id="LabelAddressErrMsg" runat="server" width="200px" cssclass="Label_Small"
											visible="false" forecolor="red">Address cannot be blank</asp:label>

									</td>
									<td align="left" colspan="2">
										<asp:button id="ButtonAdress" runat="server" width="96px" text="Address" cssclass="Button_Normal"
											causesvalidation="False"></asp:button>
									</td>
								</tr>
								<tr>
									<td align="left" class="Label_Small">
										Address2:
									</td>
									<td align="left" colspan="3">
										<asp:label id="TextBoxAdress2" runat="server" width="250px" cssclass="Label_Small"
											readonly="true"></asp:label>
									</td>
								</tr>
								<tr>
									<td align="left" class="Label_Small">
										Address3:
									</td>
									<td align="left" colspan="3">
										<asp:label id="TextBoxAdress3" runat="server" width="250px" cssclass="Label_Small"
											readonly="true"></asp:label>
									</td>
								</tr>
								<tr>
									<td align="left" class="Label_Small">
										City:
									</td>
									<td align="left">
										<asp:label id="TextBoxGeneralCity" runat="server" width="250px" cssclass="Label_Small"
											readonly="true"></asp:label>
									</td>
                               </tr>
                                <tr>
									<td align="left" class="Label_Small">
										<asp:label id="TextBoxGeneralState" runat="server" width="250px" cssclass="Label_Small"
											readonly="true" visible="False"></asp:label>
										Zip Code:
									</td>
									<td align="left">
										<asp:label id="TextBoxZip" runat="server" width="150px" cssclass="Label_Small"
											readonly="true"></asp:label>
									</td>
								</tr>
								<tr>
									<td align="left" class="Label_Small">
										State:
									</td>
									<td align="left" colspan="3">
										<asp:label width="100px" runat="server" id="TextBoxStateName" cssclass="Label_Small"
											readonly="true"></asp:label>
									</td>
								</tr>
								<tr>
									<td align="left" class="Label_Small">
										Country:
									</td>
									<td align="left" colspan="3">
										<asp:label id="TextBoxCountry" runat="server" width="100px" cssclass="Label_Small"
											readonly="true"></asp:label>
									</td>
								</tr>--%>

                                        <tr>
                                            <td align="left" class="Label_Small">Telephone:
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="TextBoxTelephone" runat="server" Width="200px" CssClass="Label_Small"
                                                    readonly="true"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3">
                                                <asp:Button ID="ButtonTelephone" runat="server" Width="96px" Text="Telephone" CssClass="Button_Normal"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small">Email:
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="TextBoxEmail" runat="server" Width="200px" CssClass="Label_Small"
                                                    readonly="true"></asp:Label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:Button ID="ButtonEmail" runat="server" Width="96px" Text="Email" CssClass="Button_Normal"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="LabelHubTypeErrMsg" runat="server" Width="500px" CssClass="Label_Small"
                                                    Visible="false" ForeColor="red">HubType not Selected</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="LabelResolutionErrMsg" runat="server" Width="500px" CssClass="Label_Small"
                                                    Visible="false" ForeColor="red">Resolution not Entered</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="LabelMetroErrMsg" runat="server" Width="500px" CssClass="Label_Small"
                                                    Visible="false" ForeColor="red">Metro cannot be blank</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small" colspan="2">Y-Relation Managers:
									
										<asp:TextBox ID="txtRelationManager" runat="server" Width="200px" CssClass="TextBox_Normal"
                                            ReadOnly="True"> </asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnAddRelationManager" runat="server" Text="Edit" CssClass="Button_Normal"
                                                    Width="96px" CausesValidation="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="Label_Small"></td>
                                            <td align="left"></td>
                                            <td align="left" colspan="2">
                                                <div id="dvname" runat="server" style="display: none" title="Select Y-relation Manager">
                                                    <asp:GridView ID="gvRelationManagers" runat="server" AutoGenerateColumns="false"
                                                        DataKeyNames="ID" AllowSorting="true" CssClass="DataGrid_Grid" Width="100%" hight="100%">

                                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                                        <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSel" runat="server" CssClass="Warn" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ID" HeaderText="EmployeeID" SortExpression="EmployeeID" Visible="False" />
                                                            <asp:BoundField DataField="FullName" HeaderText="Full Name" />

                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                    <asp:Label ID="lblRelationManagerName" runat="server" Width="100%" CssClass="TextBox_Normal"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="700" height="350">
                                        <tr valign="top" class="Table_WithBorder">
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td align="left" class="td_Text">Officers
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                            <asp:Button ID="ButtonOfficersAdd" runat="server" Text="Add..." Width="96px" CssClass="Button_Normal"
                                                                CausesValidation="false"></asp:Button>
                                                            <%--<asp:button id="ButtonOfficersUpdate12" runat="server" text="Update Item" width="96px"
														cssclass="Button_Normal" visible="false" causesvalidation="false"></asp:button>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Officer-->
                                            </td>
                                        </tr>
                                        <!--<tr>
										<td>
											<table width="688">
												<tr>-->
                                        <!--
												<td class="Td_ButtonContainer">
													<asp:button id="ButtonOfficersDelete" runat="server" Text="Delete Item " Width="96px" Enabled="False" CssClass="Button_Normal" CausesValidation="false"></asp:button></td>
													-->
                                        <!--<td class="Td_ButtonContainer" align="right">-->
                                        <!--<asp:button id="ButtonOfficersAdd1" runat="server" Text="Add Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="false"></asp:button>-->
                                        <!--Rahul 01,Mar06-->
                                        <!--<td class="Td_ButtonContainer" align="right">-->
                                        <!--<asp:button id="ButtonOfficersUpdate1" runat="server" Text="Update Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="false"></asp:button>-->
                                        <!--Rahul 01,Mar06-->
                                        <!--</tr>
											</table>
										</td>
									</tr>-->

                                        <tr>
                                            <td valign="top">
                                                <div style="overflow: scroll; width: 680px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCAOfficer" AutoGenerateColumns="False" runat="server"
                                                        Width="660" CellPadding="1" CellSpacing="0" CssClass="DataGrid_Grid" AllowSorting="True"
                                                        OnSortCommand="SortCommand_OnClick" SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="guiUniqueId" Visible="False" />
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                                    <asp:ImageButton ID="ButtonOfficersUpdate" runat="server" ImageUrl="images\edits.gif" CausesValidation="False"
                                                                        CommandName="Select" OnClientClick="javascript: return CheckAccessOfficersUpdate('ButtonOfficersUpdate');" ToolTip="Update Officer"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                                    <asp:ImageButton ID="Imagebutton6" runat="server" ImageUrl="images\delete.gif" CausesValidation="False"
                                                                        CommandName="DeleteSelect" ToolTip="Delete Officer" OnClientClick="javascript: return CheckAccessOfficersUpdate('ButtonOfficersDelete');"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Fund No" SortExpression="Fund No">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkbtnFundNo" CommandName="FundNo" SortExpression="Fund No" CommandArgument='<%# Eval("Fund No") %>' runat="server" CssClass="Warn_Dirty"><%# Eval("Fund No")%> </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:BoundColumn HeaderText="Title" DataField="Title" SortExpression="Title" />
                                                            <asp:BoundColumn HeaderText="Name" DataField="Name" SortExpression="Name" ItemStyle-Width="100px" />
                                                            <asp:BoundColumn HeaderText="Phone No" DataField="Phone No" SortExpression="Phone No" />
                                                            <asp:BoundColumn HeaderText="Extn No" DataField="Extn No" SortExpression="Extn No" />
                                                            <asp:BoundColumn HeaderText="Email" DataField="Email" SortExpression="Email" />
                                                            <asp:BoundColumn HeaderText="Effective Date" DataField="Effective Date" SortExpression="Effective Date"
                                                                DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="80px" />
                                                        </Columns>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="700" class="Table_WithBorder" height="350">
                                        <tr valign="top">
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text">Contacts
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                            <asp:Button ID="ButtonContactAdd" runat="server" Text="Add..." Width="96px" CssClass="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="ButtonContactUpdate" runat="server" Text="Update Item" Width="96px"
                                                                CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Contacts-->
                                                <!--<asp:label id="LabelContact" runat="server" Width="70px" cssclass="Label_Large">Contacts</asp:label>-->
                                            </td>
                                        </tr>
                                        <!--<tr>
										<td>
											<table class="Table_WithoutBorder" width="688">
												<tr>-->
                                        <!--
												<td class="Td_ButtonContainer">
													<asp:button id="ButtonContactDelete" runat="server" Text="Delete Item " Width="96px" Enabled="False"
														CssClass="Button_Normal" CausesValidation="False"></asp:button>
												</td>-->
                                        <!--<td class="Td_ButtonContainer" align="right">-->
                                        <!--<asp:button id="ButtonContactAdd1" runat="server" Text="Add Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="False"></asp:button>-->
                                        <!--</td>-->
                                        <!--Rahul-->
                                        <!--<td class="Td_ButtonContainer" align="right">-->
                                        <!--<asp:button id="ButtonContactUpdate1" runat="server" Text="Update Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="False"></asp:button>-->
                                        <!--Rahul-->
                                        <!--</td>
												</tr>
											</table>
										</td>
									</tr>-->

                                        <tr>
                                            <td align="center" valign="top">
                                                <div style="overflow: scroll; width: 680px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCAContact" AutoGenerateColumns="False" Width="660" runat="server"
                                                        CssClass="DataGrid_Grid" AllowSorting="True" OnSortCommand="SortCommand_OnClick"
                                                        SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="guiUniqueId" Visible="False" />
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                                    <asp:ImageButton ID="Imagebutton2" runat="server" ImageUrl="images\edits.gif" CausesValidation="False"
                                                                        CommandName="Select" ToolTip="Update Contact" OnClientClick="javascript: return CheckAccessContactsUpdate('ButtonContactUpdate');"></asp:ImageButton>
                                                                    <!--Rahul-->
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Imagebutton7" runat="server" ImageUrl="images\delete.gif" CausesValidation="False"
                                                                        CommandName="DeleteSelect" ToolTip="Delete Contact"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:TemplateColumn HeaderText="Fund No" SortExpression="Fund No">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkbtnFundNo" CommandName="FundNo" SortExpression="Fund No" CommandArgument='<%# Eval("Fund No") %>' runat="server" CssClass="Warn_Dirty"><%# Eval("Fund No")%> </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn HeaderText="Type" DataField="Type" SortExpression="Type" />
                                                            <asp:BoundColumn HeaderText="Contact Name" DataField="Contact Name" SortExpression="Contact Name" />
                                                            <asp:BoundColumn HeaderText="Phone No" DataField="Phone No" SortExpression="Phone No" />
                                                            <asp:BoundColumn HeaderText="Extn No" DataField="Extn No" SortExpression="Extn No" />
                                                            <asp:BoundColumn HeaderText="Email" DataField="Email" SortExpression="Email" />
                                                            <asp:BoundColumn HeaderText="Effective Date" DataField="Effective Date" SortExpression="Effective Date"
                                                                DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="80px" />
                                                            <asp:BoundColumn HeaderText="Title" DataField="TitleDescription" SortExpression="Title" />
                                                        </Columns>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="700" class="Table_WithBorder" height="350">
                                        <tr valign="top">
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text">Branches
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Branches-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <div style="overflow: scroll; width: 680px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCABranches" runat="server" Width="660" CellPadding="1"
                                                        CellSpacing="0" CssClass="DataGrid_Grid" AllowSorting="True" OnSortCommand="SortCommand_OnClick"
                                                        SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="700" class="Table_WithBorder" height="350">
                                        <tr valign="top">
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text">Resolutions
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                            <asp:Button ID="ButtonResoAdd" runat="server" Text="Add..." Width="96px" CssClass="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="ButtonResoUpdate" runat="server" Text="Update Item" Width="96px"
                                                                Enabled="False" CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Resolutions-->
                                            </td>
                                        </tr>
                                        <!--<tr>
										<td>
											<table width="688">
												<tr>
													<td class="Td_ButtonContainer" align="right">
														<asp:button id="ButtonResoAdd1" runat="server" Text="Add Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="False"></asp:button></td>-->
                                        <!--Rahul-->
                                        <!--<td class="Td_ButtonContainer" align="right">-->
                                        <!--Swopna -ButtonResoUpdate visibility made false;Phase IV changes-->
                                        <!--<asp:button id="ButtonResoUpdate1" runat="server" Text="Update Item" Width="96px" Enabled="False"
															CssClass="Button_Normal" CausesValidation="False" visible="false"></asp:button></td>-->
                                        <!--Rahul-->
                                        <!--</tr>
											</table>
										</td>
									</tr>-->
                                        <tr>
                                            <td valign="top">
                                                <div style="overflow: scroll; width: 680px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCAResolutions" AutoGenerateColumns="False" runat="server"
                                                        Width="660" CellPadding="1" CellSpacing="0" CssClass="DataGrid_Grid" AllowSorting="True"
                                                        OnSortCommand="SortCommand_OnClick" SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                                    <asp:ImageButton ID="Imagebutton3" runat="server" ImageUrl="images\edits.gif" CausesValidation="False"
                                                                        CommandName="Select" ToolTip="Update Resolution" OnClientClick="javascript: return CheckAccessResoUpdate('ButtonResoUpdate');"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn HeaderText="Eff. Date" DataField="Eff. Date" SortExpression="Eff. Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                            <asp:BoundColumn HeaderText="Term. Date" DataField="Term. Date" SortExpression="Term. Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                            <asp:BoundColumn HeaderText="Vesting Desc" DataField="Vesting Desc" SortExpression="Vesting Desc" />
                                                            <asp:BoundColumn HeaderText="Resolution Desc" DataField="Resolution Desc" SortExpression="Resolution Desc" />
                                                            <asp:BoundColumn HeaderText="Part.%" DataField="Part.%" SortExpression="Part.%" />
                                                            <asp:BoundColumn HeaderText="YMCA%" DataField="YMCA%" SortExpression="YMCA%" />
                                                            <asp:BoundColumn HeaderText="S.Scale%" DataField="S.Scale%" SortExpression="S.Scale%" />
                                                            <asp:BoundColumn HeaderText="Add'l YMCA%" DataField="Add'l YMCA%" SortExpression="Add'l YMCA%" />
                                                        </Columns>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="700" class="Table_WithBorder" height="350">
                                        <tr valign="top">
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text">Bank Information
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                            <asp:Button ID="ButtonBankInfoAdd" runat="server" Text="Add..." Width="96px" CssClass="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="ButtonBankInfoUpdate" runat="server" Text="Update Item" Width="96px"
                                                                CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Bank Information-->
                                            </td>
                                        </tr>
                                        <!--<tr>
										<td>
											<table width="688">
												<tr>
													<td class="Td_ButtonContainer" align="right">-->
                                        <!--<asp:button id="ButtonBankInfoAdd1" runat="server" Text="Add Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="False"></asp:button>-->
                                        <!--</td>-->
                                        <!--Rahul-->
                                        <!--<td class="Td_ButtonContainer" align="right">-->
                                        <!--<asp:button id="ButtonBankInfoUpdate1" runat="server" Text="Update Item" Width="96px" CssClass="Button_Normal"
															CausesValidation="False"></asp:button>-->
                                        <!--</td>-->
                                        <!--Rahul-->
                                        <!--</tr>
											</table>
										</td>
									</tr>-->
                                        <tr>
                                            <td valign="top">
                                                <div style="overflow: scroll; width: 680px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCABankInfo" runat="server" Width="660" CellPadding="1"
                                                        CellSpacing="0" AutoGenerateColumns="False" CssClass="DataGrid_Grid" AllowSorting="True"
                                                        OnSortCommand="SortCommand_OnClick" SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="guiUniqueId" Visible="False" />
                                                            <asp:TemplateColumn ItemStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                                    <asp:ImageButton ID="ImageButtonSelect" runat="server" ImageUrl="images\edits.gif" CausesValidation="False"
                                                                        CommandName="Select" ToolTip="Update Bank Info" OnClientClick="javascript: return CheckAccessBankInfoUpdate('ButtonBankInfoUpdate');"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                            <asp:BoundColumn DataField="Bank ABA#" HeaderText="Bank ABA#" SortExpression="Bank ABA#" />
                                                            <asp:BoundColumn DataField="Effective Date" HeaderText="Effective Date" SortExpression="Effective Date"
                                                                DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="80px" />
                                                        </Columns>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="700" class="Table_WithBorder" height="350">
                                        <tr valign="top">
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text">Notes History
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                            <asp:Button ID="ButtonNotesView" runat="server" Text="View Item" Width="96px" CssClass="Button_Normal"
                                                                CausesValidation="False" Visible="false"></asp:Button>
                                                            <asp:Button ID="ButtonYMCANotesAdd" runat="server" Text="Add..." Width="96px" CssClass="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Notes History-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <div style="overflow: scroll; width: 680px; height: 330px; text-align: left">
                                                    <asp:DataGrid ID="DataGridYMCANotesHistory" runat="server" Width="660" CellPadding="1"
                                                        CellSpacing="0" CssClass="DataGrid_Grid" AllowSorting="True" OnSortCommand="SortCommand_OnClick"
                                                        AutoGenerateColumns="false" SelectedItemStyle-VerticalAlign="Top">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        <Columns>
                                                            <asp:BoundColumn HeaderText="UniqueId" DataField="guiUniqueID" Visible="false" />
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <!-- Edited by Anudeep for YRS 5.0-1621 on 2012.09.26-->
                                                                    <asp:ImageButton ID="Imagebutton5" runat="server" ImageUrl="images\view.gif" CausesValidation="False"
                                                                        CommandName="View" ToolTip="View Note"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Creator" HeaderText="Creator" SortExpression="Creator"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="First Line of Notes" HeaderText="First Line of Notes" SortExpression="First Line of Notes"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Mark As Important" SortExpression="bitImportant" ItemStyle-Width="120">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBoxImportant" runat="server" AutoPostBack="True" OnCheckedChanged="Check_Clicked" Enabled="true" Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        <%--Start: Bala: 12/01/2016: YRS-AT-1718: Added Delete button--%>
                                                        <asp:TemplateColumn HeaderText ="Delete" ItemStyle-Width="10%" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="DeleteNotes" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <%-- End: Bala: 12/01/2016: YRS-AT-1718: Added Delete button --%>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="700" class="Table_WithBorder" cellspacing="0" border="0" height="350">
                                        <tr>
                                            <td align="left" colspan="4" valign="top">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="td_Text">Documents
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Documents
											<asp:Label id="LabelDocs" runat="server" Width="120px" CssClass="Label_Large">Documents</asp:Label>-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%" align="left" valign="top" height="200px">
                                                <asp:LinkButton ID="LinkButtonIDM" runat="server">View IDM</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>

                                <%-- Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Design for W/T/S/M Tab--%>
                                <iewc:PageView>
                                    <div style="overflow:auto;">
                                        <div class="Table_WithBorder" style="text-align: center; width: 100px; height: 250px; float: left;">
                                            <div tabindex="0" id="tab_Withdraw" runat="server">
                                                <input type="button" href="#tabContent_Withdraw" tabindex="2" id="tab_btnWithdrawClick" runat="server" onclick="TabChange(this);" value="Withdraw" class="tabNotSelected1" />
                                            </div>
                                            <div tabindex="1" id="tab_Terminate" runat="server">
                                                <div style="padding-top: 2px">
                                                    <input type="button" href="#tabContent_Terminate" id="tab_btnTerminateClick" tabindex="3" runat="server" onclick="TabChange(this);" value="Terminate" class="tabNotSelected1" />
                                                </div>
                                            </div>
                                            <div tabindex="2" id="tab_Suspend" runat="server">
                                                <div style="padding-top: 2px">
                                                    <input type="button" href="#tabContent_Suspend" id="tab_btnSuspendClick" tabindex="4" runat="server" onclick="TabChange(this);" value="Suspend" class="tabNotSelected1" />
                                                </div>
                                            </div>
                                            <div tabindex="3" id="tab_Merge" runat="server">
                                                <div style="padding-top: 2px">
                                                    <input type="button" href="#tabContent_Merge" id="tab_btnMergeClick" tabindex="5" runat="server" onclick="TabChange(this);" value="Merge" class="tabNotSelected1" />
                                                </div>
                                            </div>
                                            <div tabindex="4" id="tab_Reactivate" runat="server">
                                                <div style="padding-top: 2px">
                                                    <input type="button" href="#tabContent_Reactivate" id="tab_btnReactivateClick" tabindex="6" runat="server" onclick="TabChange(this);" value="Reactivate" class="tabNotSelected1" />
                                                </div>
                                            </div>
                                        </div>
                                        <%-- start --Manthan YRS-AT-2334 -- withdraw --%>
                                        <div id="tabContent_Withdraw"  runat="server" class="WTSM_tabContent" style="text-align: left; float: left; height: 250px; width: 595px;display:none;">
                                            <table width="595px" class="Table_WithBorder" height="250px">
                                                <tr>
                                                    <td align="left" class="td_Text" colspan="3">&nbsp;Withdraw
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelWithdrawDate" runat="server" Width="200px" CssClass="Label_Small">Enter withdrawal date</asp:Label>
                                                                </td>
                                                                <td align="left" width="400" style="height: 30px;">                                                                   
                                                                    <YRSCustomControls:CalenderTextBox ID="DateUserControlWithdrawDate" MaxLength="10" runat="server"  Width="90"  CssClass="TextBox_Normal" EnableCustomValidator="true" IsFocusRequired="false" 
                                                                        TrackDateChange="true" yearRange="1900:c+100" onchange="javascript:SetOperation(this);"></YRSCustomControls:CalenderTextBox>
                                                                    <asp:TextBox ID ="TxtDateWithdraw" width="95" cssclass="TextBox_Normal" runat="server" visible="false" Enabled="false"></asp:TextBox>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelPayrollDate_W" runat="server" Width="200px" CssClass="Label_Small">Enter final payroll date</asp:Label></td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <asp:DropDownList ID="DropdownPayrollDate_W" runat="server" Width="95" CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="">---Select---</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelYerdiAccess_W" runat="server" Width="200px" CssClass="Label_Small">Allow YERDI access?</asp:Label>
                                                                </td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <asp:DropDownList ID="DropdownYerdiAccess_W" runat="server" Width="95"
                                                                        CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;" colspan="2">
                                                                    <asp:Label ID="LabelSummaryReport" runat="server" CssClass="Label_Small" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="LabelBeforeWithdrawal" runat="server" CssClass="Label_Small" Visible="true">Before Withdrawal</asp:Label></td>
                                                                <td align="left">
                                                                    <asp:Label ID="LabelAfterWithdrawal" runat="server" CssClass="Label_Small" Visible="true">After Withdrawal</asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:DataGrid ID="DataGridSummaryReport" CssClass="DataGrid_Grid" runat="server" Visible="false">
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                                <td align="left" width="400">
                                                                    <asp:DataGrid ID="DatagridSummaryReportOnWithdrawal" CssClass="DataGrid_Grid" runat="server" Visible="false">
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="4">
                                                                    <asp:Label ID="LabelEmpRecord" runat="server" Width="300px" CssClass="Label_Small"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%-- End --Manthan YRS-AT-2334 -- withdraw --%>

                                        <%-- start --Manthan YRS-AT-2334 -- Terminate --%>
                                        <div id="tabContent_Terminate" runat="server" class="WTSM_tabContent" style="text-align: left; float: left; height: 250px; width: 595px;display:none;">
                                            <table width="595px" class="Table_WithBorder" height="250px">
                                                <tr>
                                                    <td align="left" class="td_Text" colspan="3">&nbsp;Terminate (YMCA not operational)
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelPayrollDate_T" runat="server" Width="200px" CssClass="Label_Small">Enter final payroll date</asp:Label></td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <asp:DropDownList ID="DropdownPayrollDate_T" runat="server" Width="95" CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="">---Select---</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelYerdiAccess_T" runat="server" Width="200px" CssClass="Label_Small">Allow YERDI access?</asp:Label>
                                                                </td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <asp:DropDownList ID="DropdownYerdiAccess_T" runat="server" Width="95"
                                                                        CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DataGrid ID="DataGridSummaryReportBeforeTermination" CssClass="DataGrid_Grid" runat="server" Visible="false">
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                                <td>
                                                                    <asp:DataGrid ID="DatagridSummaryReportOnTerminate" CssClass="DataGrid_Grid" runat="server" Visible="false">
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td align="left" colspan="5">
                                                                    <asp:Label ID="LabelEmpRecordTerminate" runat="server" Width="300px" CssClass="Label_Small"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%-- start --Manthan YRS-AT-2334 -- Terminate --%>

                                        <%-- start --Manthan YRS-AT-2334 -- Suspend --%>
                                        <div id="tabContent_Suspend"  runat="server" class="WTSM_tabContent" style="text-align: left; float: left; height: 250px; width: 595px;display:none;">
                                            <table width="593px" class="Table_WithBorder" height="250px">
                                                <tr>
                                                    <td align="left" class="td_Text">&nbsp;Suspend
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelSuspensionDate" runat="server" Width="200px" CssClass="Label_Small">Enter Suspension date</asp:Label>
                                                                </td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <YRSCustomControls:CalenderTextBox ID="DateUserControlSuspensionDate" MaxLength="10" runat="server"  Width="90"  CssClass="TextBox_Normal" EnableCustomValidator="true" IsFocusRequired="false" 
                                                                        TrackDateChange="true" yearRange="1900:c+100" onchange="javascript:SetOperation(this);"></YRSCustomControls:CalenderTextBox>
                                                                    <asp:TextBox ID ="TxtDateSuspend"  width="95"  cssclass="TextBox_Normal" runat="server" visible="false" Enabled ="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelPayrollDate_S" runat="server" Width="200px" CssClass="Label_Small">Enter final payroll date</asp:Label></td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <asp:DropDownList ID="DropdownPayrollDate_S" runat="server" Width="95" CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="">---Select---</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelYerdiAccess_S" runat="server" Width="200px" CssClass="Label_Small">Allow YERDI access?</asp:Label>
                                                                </td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <asp:DropDownList ID="DropdownYerdiAccess_S" runat="server" Width="95"
                                                                        CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>                                                         
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="LabelBeforeSuspend" runat="server" CssClass="Label_Small" Visible="false">Before Suspend</asp:Label></td>
                                                                <td align="left">
                                                                    <asp:Label ID="LabelAfterSuspend" runat="server" CssClass="Label_Small" Visible="false">After Suspend</asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:DataGrid ID="DataGridSummaryReportBeforeSuspend" CssClass="DataGrid_Grid" runat="server" Visible="false">
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DataGrid ID="DatagridSummaryReportOnSuspend" CssClass="DataGrid_Grid" runat="server" Visible="false">
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="4">
                                                                    <asp:Label ID="LabelEmpRecordSuspend" runat="server" Width="300px" CssClass="Label_Small"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%-- End --Manthan YRS-AT-2334 -- Suspend --%>

                                        <%-- start --Manthan YRS-AT-2334 -- Merge --%>
                                        <div id="tabContent_Merge"  runat="server" class="WTSM_tabContent" style="text-align: left; float: left; height: 250px; width: 595px;display:none;">
                                            <table width="595px" class="Table_WithBorder" height="250px">
                                                <tr>
                                                    <td align="left" class="td_Text" colspan="3">&nbsp;Merge
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" style="height: 30px;" nowrap="true">
                                                                    <asp:Label ID="LabelMetroDetails" runat="server" Width="200px" CssClass="Label_Small">Select Y that this Y is merging into</asp:Label>
                                                                </td>
                                                                <td align="left" style="height: 30px; width:400px">
                                                                    <asp:DropDownList ID="DropDownYMCANo" runat="server" Width="250px" style="POSITION: relative" CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);"></asp:DropDownList>&nbsp;&nbsp;                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelMergeDate" runat="server" Width="200px" CssClass="Label_Small">Enter date of merge</asp:Label>
                                                                </td>
                                                                <td align="left" style="height: 30px; width:400px" id="tdDateMerge" runat="server">
                                                                    <YRSCustomControls:CalenderTextBox ID="DateUserControlMergeDate" MaxLength="10" runat="server"  Width="90"  CssClass="TextBox_Normal" EnableCustomValidator="true" 
                                                                        IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100" onchange="javascript:SetOperation(this);"></YRSCustomControls:CalenderTextBox>
                                                                    <asp:TextBox ID ="TxtDateMerge"  width="95" cssclass="TextBox_Normal" runat="server" visible="false" Enabled ="false"></asp:TextBox>                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelPayrolldate_M" runat="server" Width="200px" CssClass="Label_Small">Enter final payroll date</asp:Label></td>
                                                                <td align="left" style="height: 30px; width:400px">
                                                                    <asp:DropDownList ID="DropdownPayrolldate_M" runat="server" Width="95" CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="">---Select---</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelYerdiAccess_M" runat="server" Width="200px" CssClass="Label_Small">Allow YERDI access?</asp:Label>
                                                                </td>
                                                                <td align="left" style="height: 30px; width:400px">
                                                                    <asp:DropDownList ID="DropdownYerdiAccess_M" runat="server" Width="95"
                                                                        CssClass="DropDown_Normal Warn" onchange="javascript:SetOperation(this);">
                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%-- End --Manthan YRS-AT-2334 -- Merge --%>

                                        <%-- start --Manthan YRS-AT-2334 -- Reactivate --%>
                                        <div id="tabContent_Reactivate"  runat="server" class="WTSM_tabContent" style="text-align: left; float: left; height: 250px; width: 595px;display:none;">
                                            <table width="595px" class="Table_WithBorder" height="250px">
                                                <tr>
                                                    <td align="left" class="td_Text" colspan="3">&nbsp;Reactivate
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" style="height: 30px;">
                                                                    <asp:Label ID="LabelReactivateDate" runat="server" Width="200px" CssClass="Label_Small">Enter Reactivate date</asp:Label>
                                                                </td>
                                                                <td align="left" width="400" style="height: 30px;">
                                                                    <YRSCustomControls:CalenderTextBox ID="DateUserControlReactivateDate" MaxLength="10" runat="server"  Width="90"  CssClass="TextBox_Normal" EnableCustomValidator="true" 
                                                                        IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100" onchange="javascript:SetOperation(this);"></YRSCustomControls:CalenderTextBox>                                                                
                                                                    <asp:TextBox ID ="TxtDateReactivate"  width="95" runat="server" cssclass="TextBox_Normal" visible="false" Enabled="false"></asp:TextBox>                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelResoNotEntered" runat="server" Width="200px" CssClass="Label_Small"
                                                                        ForeColor="red">Resolution not Entered</asp:Label></td>
                                                                <td>
                                                            </tr>
                                                        </table>
                                            </table>
                                        </div>
                                        <%-- End --Manthan YRS-AT-2334 -- Reactivate --%>
                                        <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnOperationPerformed" runat="server" Value="" />
                                    </div>
                                </iewc:PageView>
                               <%-- End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Design for W/T/S/M Tab--%>


                                <%--<iewc:PageView>
                           
							<table class="Table_WithBorder" width="700" cellspacing="0" border="0" height="350">
								<tr>
									<td align="left" colspan="4" valign="top">
										<table width="100%">
											<tr>
												<td class="td_Text">
													W/T/M
												</td>
											</tr>
										</table>--%>
                                <%--W/T/M--%>
                                <%--</td>
								</tr>
								<tr>
									<td align="center" width="100">
										<asp:button id="ButtonWithdraw" runat="server" text="Withdraw" width="96px" cssclass="Button_Normal"
											causesvalidation="False"></asp:button>
									</td>
									<td align="left">
										<asp:label id="LabelWithdrawDate" runat="server" width="200px" cssclass="Label_Small"
											enabled="false">Enter withdrawal date</asp:label>
									</td>
									<td align="left" width="400">
										<asp:dropdownlist id="DropdownWithdrawDate" runat="server" width="95" cssclass="DropDown_Normal Warn"
											enabled="false">
												<asp:ListItem Value="">---Select---</asp:ListItem>
											</asp:dropdownlist>
									</td>
								</tr>--%>
                                <%--<tr>--%>
                                <%--<td>
										</td>
										<td align="left">
											<asp:Label id="LabelYerdiAccess_" runat="server" Width="200px" cssclass="Label_Small" enabled="false">Yerdi access required?</asp:Label>
										</td>
										<td align="left">
											<asp:dropdownlist id="DropdownYerdiAccess_" runat="server" Width="95" CssClass="DropDown_Normal"
												enabled="false">
												<asp:ListItem Value="">---Select---</asp:ListItem>
												<asp:ListItem Value="1">Yes</asp:ListItem>
												<asp:ListItem Value="0">No</asp:ListItem>
											</asp:dropdownlist>
											<br />
											<br />
										</td>
									</tr>--%>
                                <%--<tr>
									<td align="center" width="100">
										<asp:button id="ButtonTerminate" runat="server" text="Terminate (YMCA not operational)"
											width="250px" cssclass="Button_Normal" causesvalidation="False"></asp:button>
									</td>
									<td align="left">
										<asp:label id="LabelTerminationDate" runat="server" width="200px" cssclass="Label_Small"
											enabled="false">Enter termination date</asp:label>
									</td>
									<td align="left">--%>
                                <%-- added by prasad Warn class--%>
                                <%--<asp:dropdownlist id="DropdownlistTermination" runat="server" width="95" cssclass="DropDown_Normal Warn"
											enabled="false">
												<asp:ListItem Value="">---Select---</asp:ListItem>
											</asp:dropdownlist>
									</td>
								</tr>
								<tr>
									<td>
									</td>
									<td align="left">
										<asp:label id="LabelYerdiAccess_T" runat="server" width="200px" cssclass="Label_Small"
											enabled="false">Allow YERDI access?</asp:label>
									</td>
									<td align="left">
										<asp:dropdownlist id="DropdownYerdiAccess_T" runat="server" autopostback="true" width="95"
											cssclass="DropDown_Normal Warn">
												<asp:ListItem Value="1">Yes</asp:ListItem>
												<asp:ListItem Value="0" Selected="True">No</asp:ListItem>
											</asp:dropdownlist>
									</td>
								</tr>
								<tr>
								</tr>
								<tr>
									<td align="center" width="100">
										<asp:button id="ButtonReactivate" runat="server" text="Reactivate" width="96px" cssclass="Button_Normal"
											enabled="false" causesvalidation="False"></asp:button>
									</td>
									<td align="left">
										<asp:label id="LabelReactivate" runat="server" width="200px" cssclass="Label_Small"
											enabled="false">Enter Reactivate date</asp:label>
									</td>
									<td align="left" width="400">
										<asp:textbox id="TextboxReactivate" runat="server" width="120px" cssclass="TextBox_Normal DateControl"
											enabled="false" readonly="True"></asp:textbox>
										<rjs:PopCalendar ID="PopcalendarReactivate" runat="server" Separator="/" Control="TextboxReactivate"
											Format="mm dd yyyy" Enabled="false"></rjs:PopCalendar>
									</td>
								</tr>
								<tr>
								</tr>
								<tr>
								</tr>
								<tr>
									<td align="center" width="100">
										<asp:button id="ButtonMerge" runat="server" text="Merge" width="96px" cssclass="Button_Normal"
											causesvalidation="False"></asp:button>
									</td>
									<td align="left" width="180">
										<asp:label id="LabelMetroDetails" runat="server" width="200px" cssclass="Label_Small"
											enabled="false">Enter Metro YMCA Details</asp:label>
									</td>
									<td align="left" width="200">
										<asp:dropdownlist id="DropDownYMCANo" runat="server" width="120" cssclass="DropDown_Normal Warn"
											enabled="false" autopostback="true"></asp:dropdownlist>
										<asp:textbox id="TextboxMetroDetails" runat="server" width="144px" cssclass="TextBox_Normal Warn"
											enabled="false" readonly="true"></asp:textbox>
									</td>
									<td>
									</td>
								</tr>
								<tr>
									<td>
									</td>
									<td align="left" width="180">
										<asp:label id="LabelMergeDate" runat="server" width="200px" cssclass="Label_Small"
											enabled="false">Enter date of merge</asp:label>
									</td>
									<td align="left">
										<asp:dropdownlist id="DropdownlistMergeDate" runat="server" width="95" cssclass="DropDown_Normal Warn"
											enabled="false">
												<asp:ListItem Value="">---Select---</asp:ListItem>
											</asp:dropdownlist>
										<br />
										<br />
									</td>
								</tr>
								<tr>
									<td colspan="3" align="center">
										<asp:label id="LabelSummaryReport" runat="server" cssclass="Label_Small" visible="false">
												<H4>Summary Report</H4>
											</asp:label>
									</td>
								</tr>
								<tr>
									<td>
									</td>
									<td>
										<asp:label id="LabelBeforeWithdrawal" runat="server" cssclass="Label_Small" visible="false">Before Withdrawal</asp:label>
									</td>
									<td>
										<asp:label id="LabelAfterWithdrawal" runat="server" cssclass="Label_Small" visible="false">After Withdrawal</asp:label>
									</td>
								</tr>
								<tr>
									<td>
									</td>
									<td>
										<asp:datagrid id="DataGridSummaryReport" cssclass="DataGrid_Grid" runat="server"
											visible="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
											</asp:datagrid>
									</td>
									<td>
										<asp:datagrid id="DatagridSummaryReportOnWithdrawal" cssclass="DataGrid_Grid" runat="server"
											visible="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
											</asp:datagrid>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:label id="LabelResoNotEntered" runat="server" width="500px" cssclass="Label_Small"
											forecolor="red">Resolution not Entered</asp:label>
									</td>
								</tr>
								<tr>
									<td colspan="3" align="center">
										<asp:label id="LabelEmpRecord" runat="server" width="500px" cssclass="Label_Small"></asp:label>
									</td>
								</tr>
							</table>
						</iewc:PageView>--%>
                            </iewc:MultiPage>
                        </td>
                    </tr>
                    <tr>
                        <td height="39">
                            <div class="Div_Center">
                                <table width="700">
                                    <tr>
                                        <td class="Td_ButtonContainer" align="center">
                                            <asp:Button ID="ButtonSave" CssClass="Button_Normal" Width="80" name="ButtonSave"
                                                Enabled="false" Text="Save" runat="server" CausesValidation="True"></asp:Button>
                                        </td>
                                        <td class="Td_ButtonContainer" align="center">
                                            <asp:Button ID="ButtonCancel" CssClass="Button_Normal" Width="78px" Enabled="false"
                                                Text="Cancel" runat="server" CausesValidation="False"></asp:Button>
                                        </td>
                                        <!-- <td class="Td_ButtonContainer" align="center" colSpan="2">&nbsp;</td> -->
                                        <td class="Td_ButtonContainer" align="center">
                                            <!--Edited by Nudeep for YRS:5.0-1621 -->
                                            <asp:Button ID="ButtonAdd" CssClass="Button_Normal" Width="80" Text="Add YMCA" runat="server"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                        <td class="Td_ButtonContainer" align="center">
                                            <asp:Button ID="ButtonOk" CssClass="Button_Normal" Width="80" Text="OK" runat="server"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="divProgress" style="overflow: visible;">
            <div>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <asp:Label ID="labelMessage" CssClass="Label_Small" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <input id="HiddenText" type="hidden" name="HiddenText" runat="server">
        <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server">
        <asp:HiddenField ID="hdrelationManagerName" runat="server"></asp:HiddenField>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <asp:HiddenField ID="HiddenFieldDirty" Value="false" runat="server" />
        <div id="divSaveConfirmDialog" style="overflow: visible;">   
             <%-- Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Design for displaying message on switching tab without saving--%>
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                <tr>
                    <td>
                        <label id="lblSaveMessage" class="Label_Small"></label>

                    </td>
                </tr>
                <tr>
                    <td>
                        <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                    </td>
                </tr>
            </table>
             <%-- End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Design for displaying message on switching tab without saving--%>
            </div>
            <%-- Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div id="divWaivedParicipant" style="overflow: visible;display:none;">
                <div id ="divLoadReportWaivedParicipant" style="overflow-y : scroll;HEIGHT:500px;">
                        <rsweb:ReportViewer ID="ReportViewerWaivedParticipantList" runat="server" Font-Names="Verdana"
                            BorderColor="Blue" BackColor="AliceBlue" Width="65%" Height="100%" Font-Size="8pt"
                                InteractiveDeviceInfos="(Collection)" PageCountMode="Actual" EnableTheming="True"
                            ZoomMode="PageWidth" ViewStateMode="Enabled" SizeToReportContent="True" AsyncRendering="False"
                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"></rsweb:ReportViewer>
                </div> 
           
                <hr>
                <br />
                <div style="position:relative; float:right; right: 10px; bottom: 5px;">
                <asp:Button ID="ButtonWaivedProceed" CssClass="Button_Normal" Width="80" Text="Proceed" runat="server"
                                                CausesValidation="False"></asp:Button>
                    <asp:Button ID="ButtonWaivedCancel" CssClass="Button_Normal" Width="80" Text="Cancel" runat="server"
                                                CausesValidation="False"></asp:Button>
                </div> 
                <br />
            </div>
            <%-- END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI) --%>
    </form>
</body>
</html>


