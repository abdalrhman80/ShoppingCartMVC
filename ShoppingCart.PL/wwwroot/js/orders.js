var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#OrderTable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetOrdersData"
        },
        "columns": [
            { "data": "id" },
            { "data": "userName" },
            { "data": "applicationUser.email" },
            {
                "data": "phoneNumber",
                "render": function (data) {
                    return data || '<span class="text-muted">No Phone Number</span>';
                }
            },
            {
                "data": "orderStatus",
                "render": function (data) {
                    let statusClass = "";

                    switch (data) {
                        case "Approve":
                            statusClass = "text-primary"
                            break;

                        case "Processing":
                            statusClass = "text-warning"
                            break;

                        case "Shipped":
                            statusClass = "text-success"
                            break;

                        case "Canceled" || "Refund":
                            statusClass = "text-danger"
                            break;

                        //case "Refund":
                        //    statusClass = "text-danger"
                        //    break;

                        default:
                            statusClass = "text-black";
                            break;
                    }

                    return `<td> <span class="${statusClass}">${data}</span> </td>`
                }
            },
            {
                "data": "totalPrice",
                "render": function (data) {
                    return data.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-info">Details</a>`;
                },
                orderable: false
            },
        ],
        "responsive": true
    });
}