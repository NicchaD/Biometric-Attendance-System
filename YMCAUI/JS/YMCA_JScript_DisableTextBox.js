/**************************************************************************************************/
// Created JS file to add overrride style for disabled TextBoxes.
/**************************************************************************************************/
// Shiny C.      2019.05.13      YRS-AT-2601 - YRS- modification to color scheme for disabled Fields 
/**************************************************************************************************/
// This code is compatible with jquery 1.7 and above.
$(document).ready(function () {
    $(this).find("input:text").each(function () {
        //check if input textbox is disabled
        //2019.22.05 : SC : YRS-AT-2601 Starts - added check for exclude textbox which have to be excluded from application of new style
        if (($(this).is(':disabled')) && (!$(this).hasClass("Reject_TextBox_Disabled"))) {
        //2019.22.05 : SC : YRS-AT-2601 Ends
                $(this).attr('disabled', false);
                $(this).attr('readonly', true);
                $(this).removeClass().addClass("TextBox_Disabled");
                $(this).on('focus', function (e) {
                    $(this).blur();
                });
            }
            //display readonly controls in Loans tab as disabled
        if (($('#WEBLoanDetailsDiv').length > 0) || ($('#LoanDetailsDiv').length > 0)) {
                if ($(this).is('[readonly]')) {
                    $(this).removeClass().addClass("TextBox_Disabled");
                    $(this).on('focus', function (e) {
                        $(this).blur();
                    });
                }
            }
        });
});
/* 2019.15.04 : SC : YRS-AT-2601 Ends : Added script for disable controls styling*/