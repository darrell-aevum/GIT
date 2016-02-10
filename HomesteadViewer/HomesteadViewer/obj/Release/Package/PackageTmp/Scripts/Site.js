$(document).ready(function () {
    StyleTables();

    $('#result-table tr:not(:first)').click(function () {
        $.ajaxSetup({ cache: false });
        var url = $(this).attr('data-url');
        if (url != '') {
            $.get(url, function (data) {
                $('#dialog').html(data);
                StyleTables();
                $("#dialog").dialog({ autoSize: true, width: 'auto', height: 'auto' });
            });
        }
    });
    
    $("#add-business").bind("click", function (e) {
        e.preventDefault();
        CreateWindow('Add New Business', '/Business/EditBusiness');
        return false;
    });

    $("#add-contact").bind("click", function (e) {
        e.preventDefault();
        CreateWindow('Add New Contact', '/Business/EditContact');
        return false;
    });
    
    $("#edit-business").bind("click", function (e) {
        e.preventDefault();
        var id = $("#edit-business").attr("data-id");
        CreateWindow('Edit Business', '/Business/EditBusiness/' + id);
        return false;
    });

    $("#edit-contact").bind("click", function (e) {
        e.preventDefault();
        var id = $("#edit-contact").attr("data-id");
        CreateWindow('Edit Contact', '/Business/EditContact/' + id)
        return false;
    });
    
    $("#add-coment").bind("click", function (e) {
        e.preventDefault();
        var id = $("#add-coment").attr("data-id");
        CreateWindow('Add Comment', '/Business/SaveComment/' + id)
        return false;
    });
});

function CreateWindow(title, url) {
    var window;
    if (!$("#window").data("kendoWindow")) {
        $("#window").kendoWindow({
            actions: ["Maximize", "Close"],
            title: title,
            modal: true,
            content: url,
            refresh: function () {
                $("#window").data("kendoWindow").center();
            },
        });
        window = $("#window").data("kendoWindow");
    } else {
        window = $("#window").data("kendoWindow");
        window.title(title);
        window.refresh(url);
    }
    
    window.open().center().delay(1000).center();
}

function CloseWindow() {
    $("#window").data("kendoWindow").close();
}

function submitBusinessComment(elm) {
    $.ajax({
        async: false,
        url: elm.action,
        type: elm.method,
        data: $('#' + elm.id).serialize(),
        success: function (result) {
            CloseWindow();
            var list = $('#comment-list').data('kendoListView');
            list.dataSource.read();
            list.refresh();
            $('#comment-list').kendoAnimate({ effects: "fade:in", duration: 1000, show: true });
        }
    });
    return false;
}

function StyleTables() {
    $('#result-table span').each(function () {
        if ($(this).attr('data-valid') == 'True') {
            $(this).addClass('label label-block label-success center');
        } else if ($(this).attr('data-valid') == 'False') {
            $(this).addClass('label label-block label-important center');
        }
        
        if ($(this).attr('data-status') == '1') {
            $(this).addClass('label label-block label-success center');
        } else if ($(this).attr('data-status') == '2') {
            $(this).addClass('label label-block label-important center');
        } else if ($(this).attr('data-status') == '3') {
            $(this).addClass('label label-block label-warning center');
        }
    });
}

function validateNewTicket() {
    var errors = [];
    if ($('#NewTicketTitle').val() == '') { errors.push('Title'); }
    if ($('#NewForClientBusiness').data("kendoComboBox").value() == '') { errors.push('Client'); }
    if ($('#NewCategory').data("kendoComboBox").value() == '') { errors.push('Category'); }
    if ($('#NewAction').data("kendoComboBox").value() == '') { errors.push('Action'); }

    if (errors.length == 0 && $('#save-new-ticket').hasClass('btn-danger')) {
        $('#save-new-ticket').removeClass('btn-danger').addClass('btn-success');
        $('#new-ticket-errors').hide();
    } else if (errors.length > 0 && $('#save-new-ticket').hasClass('btn-success')) {
        $('#save-new-ticket').removeClass('btn-danger btn-success').addClass('btn-danger');
        $('#new-ticket-errors').show();
    }

    if (errors.length == 1) {
        $('#new-ticket-errors').html(errors[0] + ' is a required field!');
    } else if (errors.length > 1) {
        $('#new-ticket-errors').html(errors.join(", ") + ' are required fields!');
    }
}

function addHoverHighlight(e) {
    var gridId = "#" + this.table.context.id;
    $(gridId + " table.k-selectable tbody tr").hover(function () {
        $(this).toggleClass("k-state-hover");
    });
};

function selectRowNavigate(e) {
    var selectedRows = this.select();
    for (var i = 0; i < selectedRows.length; i++) {
        var dataItem = this.dataItem(selectedRows[i]);
        document.location = dataItem.SelectUrl;
    }
}

function updateDTO(data, actionUrl, refreshPage) { 
    var request = $.ajax({
        url: actionUrl,
        method: "POST",
        data: data,
        dataType: "json"
    }); 

    request.success(function (d) {
        if(refreshPage != false)
            window.location = window.location;
    });

    request.fail(function (jqXHR, textStatus) {
        console.dir(jqXHR);
        console.dir(textStatus);
    });
}
function formatDateString(d) {
    if (Object.prototype.toString.call(d) === "[object Date]") {
        // it is a date
        if (isNaN(d.getTime())) {  // d.valueOf() could also work
            return "";
        }
        else {
            var date = new Date(d);
            return (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
        }
    }
    else {
        return "";
    }

    (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear()
}