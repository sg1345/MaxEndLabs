function loadOrderPage(pageNum) {
	let baseUrl = window.location.pathname;

	const term = $('#searchTerm').val() || "";
	const type = $('#searchType').val() || "OrderNumber";

	const url = `${baseUrl}?page=${pageNum}&searchType=${type}&searchTerm=${encodeURIComponent(term)}`;

	console.log(url);

	$.get(url, function (data) {
		$('#orderManagerBoxFullWrapper').html(data);
	});
}