function inquiryPostSuccess(data) {
    alert("Inquiry submitting successfully")
}
function inquiryPostFailure(data) {
    alert("Problem in submitting inquiry. Not submited!")
}

function deleteProperty(id) {
    var confrm = confirm("Are you sure to delete this property! it will be deleted permenently. press OK to confirm deletion");
    if (confrm == true) {
        $.ajax({
            type: 'POST',
            url: "/Property/Delete?id=" + id,
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    $("#" + id).hide();
                }
                else {
                    alert("data not deleted due to some reason!");
                }
            },
            error: function (ex) {
                var r = jQuery.parseJSON(response.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
            }
        });
    }
}
function MarkAsUnavailable(id) {
    var confrm = confirm("Are you sure to make this property as unavailable?  press OK to mark it as unavailable");
    if (confrm == true) {
        $.ajax({
            type: 'POST',
            url: "/Property/MarkAsUnAvailable?id=" + id,
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    $("#" + id + "btn").text("Mark as Available");
                    $("#" + id + "btn").attr("onclick", "MarkAsAvailable(" + id + ")");
                    $("#" + id + "availableSpan").text("Not Available");
                }
                else {
                    alert("OOps!!something went wrong");
                }
            },
            error: function (ex) {
                var r = jQuery.parseJSON(response.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
            }
        });
    }
}
function MarkAsAvailable(id) {
    var confrm = confirm("Are you sure to make this property as available?  press OK to mark it as available");
    if (confrm == true) {
        $.ajax({
            type: 'POST',
            url: "/Property/MarkAsAvailable?id=" + id,
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    $("#" + id + "btn").text("Mark as Unvailable");
                    $("#" + id + "btn").attr("onclick", "MarkAsUnavailable(" + id + ")");
                    $("#" + id + "availableSpan").text("Available");
                }
                else {
                    alert("OOps!!something went wrong");
                }
            },
            error: function (ex) {
                var r = jQuery.parseJSON(response.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
            }
        });
    }
}
function MarkAsSelected(id, dealerId = "") {
    
    var confrm = confirm("Are you sure to make this property as selected?  press OK to mark it as available");
    if (confrm == true) {
        $.ajax({
            type: 'POST',
            url: "/Property/MarkAsSelected?id=" + id + "&dealerId=" + dealerId,
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    $("#" + id + "btn").text("Mark as UnSelected");
                    $("#" + id + "btn").attr("onclick", "MarkAsUnSelected(" + id + ")");
                    $("#" + id + "selectedSpan").text("Selected");
                    $("#dropDown").hide();
                }
                else {
                    alert("OOps!!something went wrong");
                }
            },
            error: function (ex) {
                var r = jQuery.parseJSON(response.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
            }
        });
    }
}
function MarkAsUnSelected(id) {
    var confrm = confirm("Are you sure to make this property as unSelected?  press OK to mark it as available");
    if (confrm == true) {
        $.ajax({
            type: 'POST',
            url: "/Property/MarkAsUnSelectd?id=" + id,
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    $("#" + id + "btn").text("Mark as Selected");
                    $("#" + id + "btn").attr("onclick", "MarkAsSelected(" + id + ")");
                    $("#" + id + "selectedSpan").text("Not Selected");
                }
                else {
                    alert("OOps!!something went wrong");
                }
            },
            error: function (ex) {
                var r = jQuery.parseJSON(response.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
            }
        });
    }
}