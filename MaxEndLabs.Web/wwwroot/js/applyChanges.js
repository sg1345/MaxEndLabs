$(document).on("click", "#btnUpdateStatus", function (e) {
    const form = $("#statusForm");
    const actionUrl = form.attr("action");
    const formData = form.serialize();

    $.post(actionUrl, formData, function (freshHtml) {
        // The 'freshHtml' is the result of 'return await Details(orderId)'
        // We just overwrite the modal content with the updated version!
        $("#orderModalContent").html(freshHtml);

        console.log("Status updated and modal refreshed.");
    }).fail(function () {
        alert("Failed to update status.");
    });
});