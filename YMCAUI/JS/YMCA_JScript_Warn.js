//Prasad Jadhav      2011.08.26      For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
function mark_dirty() {
    $('#HiddenFieldDirty').val(true);
}

function clear_dirty() {
    $('#HiddenFieldDirty').val(false);
}


function my_confirm() {
    return confirm('You have unsaved changes. Do you want to leave this page and lose your changes?')
}


function CheckDirty(event,thisevent,func) {
    var isDirty = $('#HiddenFieldDirty').val();
    if (isDirty == "true") {
        if (document.getElementById("divChangesExist") == null) {
            var result = new Boolean();
            result = my_confirm();
            if (!result) {
                event.preventDefault();
                return false;
            } else {
                if (func != null) return func();
            }

        }
        else {
            var inputevent = thisevent;
            $('#divChangesExist').dialog
                    ({
                        modal: true,
                        close: false,
                        width: 400, height: 200,
                        title: "Warning",
                        resizable: false,
                        buttons: [{ text: "Ok", click: function () {

                            $(this).dialog("close");
                            $('#HiddenFieldDirty').val(false);
                            return inputevent.click();
                        }

                        },
                               { text: "Cancel", click: function () {

                                   $(this).dialog("close");
                               }

                               }],
                        open: function (type, data) {
                            $(this).parent().appendTo("form");
                            $('a.ui-dialog-titlebar-close').remove();
                        }
                    });

            event.preventDefault();
        }

    } else { //isDirty == "false"
        if (func != null) return func();
    }

}

function CheckingWarning() {
    $(".Warn").change(mark_dirty);
    $(".DateControl").bind("onDateChanged", mark_dirty);
    //Prasad Jadhav 2011.09.26 For BT-934,Age not calculating automatically when we change the DOB through keyboard
    $(".DateControl").keypress(mark_dirty);
    $(".menuitem[onclick*='location.href'], .Warn_Dirty").each(function () {
        var func = $(this)[0].onclick;
        if (func != null) {
            $(this)[0].onclick = null;
        }
        $(this).click(function (event) {
            return CheckDirty(event, this, func);

        });
    });
}

$(document).ready(function () {
    if (typeof (Sys) != 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (args.get_error() == undefined) {
                CheckingWarning();
            }
        }
    }
    CheckingWarning();    
});
