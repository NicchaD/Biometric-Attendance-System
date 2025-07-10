<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="AnnuityBeneficiaryDeath.aspx.vb" Inherits="YMCAUI.AnnuityBeneficiaryDeath" EnableEventValidation="true" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });
        //Start: 2015-04-29 BJ:2570 YRS 5.0:2380 Changed for multiple dialogs purpose
        function CloseWSMessage(close) {
            if (close == 'infDialog') {
                $("#InformationDialog").dialog('close');
            } else if (close == 'letterDialog') {
                $("#GenerateLetterDialog").dialog('close');
            }
            return false;
        }
        
        function openDialog(msg, type) {
            switch (type) {
                case 'infDialog':
                    $("#InformationDialog").dialog
                           ({
                               title: "YMCA YRS - Information",
                               buttons: [{ text: "OK", click: CloseInformationDialog }],
                               height: 300
                           });
                    $("#InformationDialog").dialog('open');
                    $("#lblMessage").html(msg);
                    break;
                case 'Over6MonthsYesNo' :
                    $("#InformationDialog").dialog
                           ({
                               title: "YMCA YRS - Confirmation",
                               buttons: [{ text: "Yes", click: doPostBackForOver6Months }, { text: "No", click: CloseInformationDialog }],
                               height: 100  // SR : 2018-04-07 | YRS-AT-3804 | increase the height of message box.

                           });
                    $("#InformationDialog").dialog('open');
                    $("#lblMessage").html(msg);
                    break;
                case 'SaveYesNo':
                    $("#InformationDialog").dialog({
                        title: "YMCA YRS - Confirmation",
                        buttons: [{ text: "Yes", click: doPostBackForSave }, { text: "No", click: CloseInformationDialog }]
                    });
                    $("#InformationDialog").dialog('open');
                    $("#lblMessage").html(msg);
                    break;
                case 'Process':
                    $("#InformationDialog").dialog({
                        title: "YMCA YRS - Processing request",
                        buttons: []
                    });
                    $("#InformationDialog").dialog('open');
                    $("#lblMessage").html(msg);
                    break;
                case 'letterDialog':
                    $('#chkLetterRecord').removeAttr('checked');
                    $('#chkIDMCopy').removeAttr('checked');
                    $("#GenerateLetterDialog").dialog('open');
            }
            return false;
        }
        //End: 2015-04-29 BJ:2570 YRS 5.0:2380 Changed for multiple dialogs purpose

        //Start: 2015-04-29 BJ:2570 YRS 5.0:2380 
        function recordCheckBoxChecked(_this) {
            var isChecked = $(_this).is(':checked') ? true : false;
            if (isChecked == true) {
                $(_this).attr('checked', 'checked');
                $('#chkIDMCopy').attr('checked', 'checked');
                $('#chkIDMCopy').removeAttr('disabled');
            } else {
                $('#chkIDMCopy').removeAttr('checked');
                $('#chkIDMCopy').attr('disabled', 'disabled');
                $(_this).removeAttr('checked');
            };
        }

        function onGenerateLetters() {
            var strJointSurvivorId = $('#<%=hdnJointSurvivorId.ClientID%>').val();
            var strPersId = $('#<%=hdnPersId.ClientID%>').val();
            var strRetireeFundId = $('#<%=hdnRetireeFundId.ClientID%>').val();
            var isRecordInitialComm = $('#chkLetterRecord').is(':checked') ? true : false;
            var isIDMCopy = $('#chkIDMCopy').is(':checked') ? true : false;
            
            CallGenerateLetters(strRetireeFundId, strPersId, strJointSurvivorId, isRecordInitialComm, isIDMCopy);
        }

        function doPostBackForOver6Months() {
            CloseInformationDialog();
            __doPostBack('ButtonYes', "");
        }

        function doPostBackForSave() {
            CloseInformationDialog();
            openDialog('Please wait until processing is complete', 'Process');
            __doPostBack('ButtonSave', "");
        }

        function CallGenerateLetters(fundId, persId, joinSurvivorId, isRecordInitialComm, isIDMCopy) {
            CloseWSMessage('letterDialog');
            openDialog('Please wait until letter is generated', 'Process');
            var parameters = '{"strRetireeFundId": "' + fundId + '","strPersId":"' + persId + '","strJointSurvivorId":"' + joinSurvivorId + '","bitRecordInitialComm":"' + isRecordInitialComm + '","bitIDMCopy":"' + isIDMCopy + '"}';
            //var jsonStringfy = JSON.stringify(parameters);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AnnuityBeneficiaryDeath.aspx/GenerateLetters",
                data: parameters,
                dataType: "json",
                success: function (data) {
                    //CloseWSMessage('infDialog');
                    __doPostBack('IDMCopy', isIDMCopy);
                },
                beforeSend: function () {
                    //openDialog('Please wait until letter is generated...', 'Process');
                }
            });
        }

        function CloseGenerateLetterDialog() {
            CloseWSMessage("letterDialog");
        }

        function CloseInformationDialog() {
            CloseWSMessage("infDialog");
        }
        function checkdate(input) {
            // This javascript method separately used for buttons enable and disable purpose.
            var validformat = /^\d{1,2}\/\d{1,2}\/\d{4}$/ //Basic check for format validity
            var returnval = false
            if (!validformat.test(input.value))
                returnval = false;
            else { //Detailed check for valid date ranges
                var monthfield = input.value.split("/")[0]
                var dayfield = input.value.split("/")[1]
                var yearfield = input.value.split("/")[2]
                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
                    returnval = false;
                else
                    returnval = true;
            }
            if (returnval == false) input.select()
            return returnval;
        }

        function DisableButtons() {
            var btnSave = $('#<%=btnSave.ClientID%>');
            var btnCancel = $('#<%=btnCancel.ClientID%>');

            $(btnSave).attr('disabled', 'disabled');
            $(btnCancel).attr('disabled', 'disabled');
        }

        //End: 2015-04-29 BJ:2570 YRS 5.0:2380 
        function BindEvents() {
            $("#InformationDialog").dialog
                      ({
                          modal: true,
                          close: false,
                          resizable: false,
                          open: function (event, ui) { $(this).parent('div').find('button:contains("OK")').focus(); },
                          autoOpen: false,
                          width: 400
                      });

            //Start: 2015-04-29 BJ:2570 YRS 5.0:2380 
            $("#GenerateLetterDialog").dialog
                      ({
                          modal: true,
                          resizable: false,
                          closeOnEscape: false,
                          open: function (event, ui) { $(this).parent('div').find('button:contains("Generate Letter")').focus(); },
                          buttons: [{ text: "Generate Letter", click: onGenerateLetters }, { text: "Cancel", click: CloseGenerateLetterDialog }],
                          autoOpen: false,
                          title: "Generate deceased survivor letter",
                          width: 600, height: 190
                      });

            $('#<%=txtDeathDate.ClientID%>').on('change', function () {
                var btnSave = $('#<%=btnSave.ClientID%>');
                var btnCancel = $('#<%=btnCancel.ClientID%>');
                var actualValue = $(this).val();
                if (actualValue != '') {
                    if (checkdate(this) == true) {
                        $(btnSave).removeAttr('disabled');
                        $(btnCancel).removeAttr('disabled');
                    } else {
                        DisableButtons();
                    }
                } else {
                    DisableButtons();
                }
            });

            $(document).ready(function () {
                var disabled = $('#<%=txtDeathDate.ClientID%>').attr('disabled');
                $('#<%=txtDeathDate.ClientID%>').datepicker({
                    minDate: new Date(1900, 10 - 1, 25),
                    maxDate: 0,
                    showOn: 'button',
                    buttonImage: 'images/calendar.gif',
                    buttonImageOnly: true,
                    changeMonth: true,
                    changeYear: true, buttonText: 'Click here to select date.'
                })
                if (disabled == 'disabled') {
                    $('#<%=txtDeathDate.ClientID%>').datepicker('disable');
                }

            });

            //End: 2015-04-29 BJ:2570 YRS 5.0:2380 
        }

    </script>
    <style type="text/css">
        .ui-datepicker select.ui-datepicker-month {
            width: 30%;
        }
        .ui-datepicker select.ui-datepicker-year {
            width: 29%;
        }
    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" ClientIDMode="Predictable">
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="Table_WithBorder" style="width: 100%; height: 450px">
                <tr style="vertical-align: top;">
                    <td align="left" class="td_Text">&nbsp;Participant Information
                 <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                 </asp:ScriptManagerProxy>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="left">
                        <table width="100%" align="left">
                            <tr style="vertical-align: top;">
                                <td style="width: 11%; text-align: left;">
                                    <label id="LabelRetireeName" for="lblRetireeName" class="Label_Small">Name:</label></td>
                                <td style="width: 25%; text-align: left;">
                                    <asp:Label ID="lblRetireeName" runat="server" CssClass="NormalMessageText"></asp:Label>
                                </td>

                                <td style="width: 7%; text-align: left;">
                                    <label id="LabelRetireeSSNo" for="lblRetireeSSNo" class="Label_Small">SS No:</label>
                                </td>
                                <td align="left" style="width: 23%">
                                    <asp:Label ID="lblRetireeSSNo" runat="server" CssClass="NormalMessageText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="vertical-align: top">
                    <td align="left" class="td_Text">&nbsp;Annuity Beneficiary Information
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="left">
                        <table width="100%" align="left">
                            <tr>
                                <td style="width: 11%; text-align: left;">
                                    <label id="LabelBeneficiaryName" for="lblBeneficiaryName" class="Label_Small">Name:</label></td>
                                <td style="width: 25%; text-align: left;">
                                    <asp:Label ID="lblBeneficiaryName" runat="server" CssClass="NormalMessageText"></asp:Label>
                                </td>

                                <td style="width: 7%; text-align: left;">
                                    <label id="LabelBeneficiarySSNo" for="lblBeneficiaryName" class="Label_Small">SS No:</label>
                                </td>
                                <td align="left" style="width: 23%">
                                    <asp:Label ID="lblBeneficiarySSNo" runat="server" CssClass="NormalMessageText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 11%; text-align: left;">&nbsp;</td>
                                <td style="width: 25%; text-align: left;">&nbsp;</td>

                                <td style="width: 7%; text-align: left;">&nbsp;</td>
                                <td align="left" style="width: 23%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 11%; text-align: left;">
                                    <label id="LabelDeathDate" for="txtDeathDate" class="Label_Small">Death Date:</label></td>
                                <td style="width: 25%; text-align: left;">
                                    <%--<uc1:DateUserControl ID="txtDeathDate" Width="150px" AutoPostBack="false"  cssInput="TextBoxDeathDate TextBox_Normal Warn "  runat="server"></uc1:DateUserControl>--%>
                                    <asp:TextBox id="txtDeathDate" ReadOnly="False" ValidationGroup="process" autocomplete="off" TabIndex="0" MaxLength="10"  Width="80px" CssClass="TextBox_Normal DateControl TextBoxDeathDate Warn" onpaste="return false;" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator id="rfDatevalidation" runat="server" Enabled="True" Display="Dynamic" ControlToValidate="txtDeathDate"
										ErrorMessage="Date cannot be blank" ValidationGroup="process" CssClass="Error_Message">*</asp:RequiredFieldValidator>
										<asp:RangeValidator id="RangeValidatorUCDate" runat="server" Enabled="true" Display="Dynamic" ControlToValidate="txtDeathDate" ValidationGroup="process" Type="Date" MinimumValue="01/01/1900" CssClass="Error_Message">Invalid Date</asp:RangeValidator>
                                </td>

                                <td style="width: 7%; text-align: left;">
                                    <asp:Button ID="btnLetter" CssClass="Button_Normal" Width="80" TabIndex="1" runat="server" Text="Letter" CausesValidation="False"></asp:Button>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="left">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblDueSinceDays" class="Label_Small" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="left">
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <label id="LabelLogList" for="txtLogList" class="Label_Small">Status Summary:</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtLogList" runat="server" Width="100%" TextMode="MultiLine"
                                        ReadOnly="True" Style="resize: none;" Height="154px" CssClass="TextBox_Normal" Rows="5"
                                        cols="4"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="left">
                        <table style="width: 100%;">
                            <tr>
                                <td align="right" class="td_Text">
                                    <asp:Button ID="btnSave" CssClass="Button_Normal" Width="80" TabIndex="2" runat="server" Text="Save" CausesValidation="False"></asp:Button>
                                    <asp:Button ID="btnCancel" CssClass="Button_Normal ButtonCancel" Width="80" TabIndex="3" runat="server" Text="Cancel" CausesValidation="False"></asp:Button>
                                    <asp:Button ID="btnClose" CssClass="Button_Normal Warn_Dirty" Width="80" runat="server" TabIndex="4" Text="Close" CausesValidation="False"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnJointSurvivorId" runat="server" />
    <asp:HiddenField ID="hdnRetireeId" runat="server" />
    <asp:HiddenField ID="hdnPurchaseDate" runat="server" />
    <asp:HiddenField ID="hdnDeathDocReceived" runat="server" />
    <asp:HiddenField ID="hdnRetireeFundId" runat="server" />
    <asp:HiddenField ID="hdnPersId" runat="server" />
    <asp:HiddenField ID="hdnDueSinceDays" runat="server" />    <%-- SB | 06/14/2017 | YRS-AT-2675 | Contains number of days since first letter sent to participant, It will help to display warning message--%>
       <%-- SR | 06/29/2018 | YRS-AT-3804 | Contains Joint Survivor Annuity suppress status--%>
    <asp:HiddenField ID="hdnJointSurvivorAnnuitySuppress" runat="server" /> 
    <asp:HiddenField ID="hdnOriginalParticipantDeceased" runat="server" /> 
    <asp:HiddenField ID="hdnJointSurvivorAnnuityPurchaseDate" runat="server" /> 
    <asp:HiddenField ID="hdnJointSurvivorAnnuityExists" runat="server" /> 
    <asp:HiddenField ID="hdnJointSurvivorBirthDate" runat="server" /> 
       <%-- SR | 06/29/2018 | YRS-AT-3804 | Contains Joint Survivor Annuity suppress status--%>
    <div id="InformationDialog" style="display: none;">
        <table width="100%" border="0">
            <tr>
                <td>
                    <div id="lblMessage" style="width:auto;" class="Label_Small"></div>
                </td>
            </tr>

        </table>
    </div>
    <div id="GenerateLetterDialog" style="display: none">
        <table width="100%" border="0">
            <tr>
                <td>
                    <div style="color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; width: 530px; height: 120px">
                        <div>
                            <div>
                                <input type="checkbox" id="chkLetterRecord" onclick="javascript: recordCheckBoxChecked(this);" />
                                <div style="color: #808080; display: inline; font-weight: bolder;">Record this letter as the first communication with annuitant.</div>
                                <br />
                                <div style="margin: 5px 25px;">
                                    Note: If above option is checked, future 60-day and 90-day follow-up letters will be
                                            <div>
                                                calculated from today's date, to be generated as needed.
                                            </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div style="color: #808080; font-weight: bolder;">
                                <input type="checkbox" id="chkIDMCopy" class="isPopupChecked chkIDMCopy" disabled="disabled" />
                                Send a copy of letter to IDM
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

