var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#OrderTable").DataTable({
        // Enable responsive features with configuration
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Admin/Order/GetOrdersData"
        },
        "columns": [
            {
                "data": "id",
                "responsivePriority": 1 // Order ID should always be visible
            },
            {
                "data": "userName",
                "responsivePriority": 2
            },
            {
                "data": "applicationUser.email",
                "responsivePriority": 4
            },
            {
                "data": "phoneNumber",
                "responsivePriority": 5,
                "render": function (data) {
                    return data || '<span class="text-muted">No Phone Number</span>';
                }
            },
            {
                "data": "orderStatus",
                "responsivePriority": 3, // Status is important, keep it visible longer
                "render": function (data) {
                    let statusClass = "";
                    let badgeClass = "";

                    switch (data) {
                        case "Approve":
                            statusClass = "text-primary";
                            badgeClass = "bg-primary";
                            break;
                        case "Processing":
                            statusClass = "text-warning";
                            badgeClass = "bg-warning";
                            break;
                        case "Shipped":
                            statusClass = "text-success";
                            badgeClass = "bg-success";
                            break;
                        case "Canceled":
                        case "Refund":
                            statusClass = "text-danger";
                            badgeClass = "bg-danger";
                            break;
                        default:
                            statusClass = "text-black";
                            badgeClass = "bg-secondary";
                            break;
                    }

                    // Return badge style for better visibility
                    return `<span class="badge ${badgeClass} text-white">${data}</span>`;
                }
            },
            {
                "data": "totalPrice",
                "responsivePriority": 2, // Price should stay visible longer
                "render": function (data) {
                    return data.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                }
            },
            {
                "data": "id",
                "responsivePriority": 6,
                "render": function (data) {
                    return `
                    <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-info">Details</a>`;
                },
                orderable: false
            },
        ],
        // Customize responsive details display
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.modal({
                    header: function (row) {
                        return 'Order Details #' + row.data().id;
                    }
                }),
                renderer: function (api, rowIdx, columns) {
                    const data = $.map(columns, function (col, i) {
                        return col.hidden ?
                            '<tr data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' +
                            '<td class="font-weight-bold">' + col.title + ':</td> ' +
                            '<td>' + col.data + '</td>' +
                            '</tr>' :
                            '';
                    }).join('');

                    return data ?
                        $('<table class="table table-striped dtr-details" width="100%"/>').append(data) :
                        false;
                }
            }
        },

        // Add DOM configuration for better layout
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',

        // Add language configuration
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search Orders...",
            lengthMenu: "Show _MENU_ orders per page",
            info: "Showing _START_ to _END_ of _TOTAL_ orders"
        },

        // Order by most recent first (assuming ID is chronological)
        order: [[0, 'desc']]
    });
}

// Add window resize handler
$(window).on('resize', function () {
    if (dtble) {
        dtble.columns.adjust().responsive.recalc();
    }
});