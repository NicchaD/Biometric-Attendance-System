<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RolloverInstitution.ascx.vb" Inherits="YMCAUI.RolloverInstitution" %>
<script>
    $(document).ready(function () {        
        $('#<%=TextBoxInstitution.ClientID%>').autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: "Helper.aspx/GetInstitutions",
                    data: "{'name':'" + request.term + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (record) {
                        var instdata = new String();
                        instdata = $.parseJSON(record.d);
                        response($.map(instdata, function (item) {
                            return {
                                label: item.instName,
                                value: item.instName                                
                            }
                        }));

                    }
                });
            },
            minLength: 3,
            autoFocus: true,
            select: function (event, ui) {
                <%= OnSelectEvent%>
            }
         });

        $.ui.autocomplete.prototype._renderItem = function (ul, item) {
            item.label = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
            return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + item.label + "</a>")
                    .appendTo(ul);
        };
    });    
</script>

<asp:textbox id="TextBoxInstitution" runat="server" CssClass="Modalpopup TextBox_Normal" 
						                MaxLength="60" Width="100%"></asp:textbox>