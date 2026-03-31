$(document).on("click", "#btnUpdateStatus", function (e) {
    const form = $("#statusForm");
    const actionUrl = form.attr("action");
    const formData = form.serialize();

    $.post(actionUrl, formData, function (freshHtml) {
        $("#orderModalContent").html(freshHtml);

        console.log("Status updated and modal refreshed.");
    }).fail(function () {
        alert("Failed to update status.");
    });
});