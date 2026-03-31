$(document).on("click", ".view-details-btn", function () {
    const id = $(this).data("id");


    $("#orderModalContent").html('<div class="text-center p-5"><div class="spinner-border text-primary"></div></div>');
    $("#orderModal").modal("show");

    $.get(`/Order/Details?orderId=${id}`, function (htmlData) {
        $("#orderModalContent").html(htmlData);
    });
});