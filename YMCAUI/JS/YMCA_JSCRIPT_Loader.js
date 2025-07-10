/***********************/
// Created JS file to add functionality to prevent double click.
/***********************/
// Megha Lad      2019.04.16      YRS-AT-4388 - YRS enh: Login button - prevent doubleclick
/***********************/


//To ShowLoader
function Showloader() {    
    $("#divloader").dialog('open');
    return true;
}


//Initialisation of Loader Dialogbox
function OpenLoaderDialog() {
    $('#divloader').dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        open: function (type, data) {
            $(this).parent().appendTo("form");
            $(".ui-dialog-titlebar").hide();
            $(".ui-widget-content").css("background", "none");
            $(".ui-widget-content").css("border", "none");
        }
    });
}

$(document).ready(function () {
    // Loader div appends automatically to current loaded page
    var loaderDiv = "<div id='divloader' style='overflow: visible;display:none;width:100%;height:100%; text-align:center;'><img src='images/ajax-loader.gif'  id='loadingimage' alt='Loading...'  title='Loading...' style='position:relative; top:25%;left:00%' /></div>";
     $("form").append(loaderDiv);    
     OpenLoaderDialog();

    // On click of all buttons, where "PreventDblClick" css is applied , the system shall dislay the loader dialogue and shall disable the background to prevent "Double-Click"
     $(".PreventDoubleClick").each(function () {
        var func = $(this)[0].onclick;
        if (func != null) {
            $(this)[0].onclick = null;
        }
        $(this).click(function (event) {
            return Showloader();
        });
    });



});