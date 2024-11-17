var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#ProductTable").DataTable({
        // Enable responsive features
        responsive: true,
        // Add scrollX for horizontal scrolling
        scrollX: true,
        "ajax": {
            "url": "/Admin/Product/GetProductsData"
        },
        "columns": [
            {
                "data": "name",
                "responsivePriority": 1, // Highest priority - always visible
                "render": function (data) {
                    const maxLength = 50;
                    if (data.length > maxLength) {
                        return data.slice(0, maxLength) + '...';
                    }
                    return data;
                }
            },
            {
                "data": "description",
                "responsivePriority": 4,
                "render": function (data) {
                    const maxLength = 50;
                    if (data.length > maxLength) {
                        return data.slice(0, maxLength) + '...';
                    }
                    return data;
                }
            },
            {
                "data": "price",
                "responsivePriority": 2, // High priority for price
                "render": function (data) {
                    return data.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                }
            },
            {
                "data": "category.name",
                "responsivePriority": 3
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<a href="/Admin/Product/Details/${data}" class="btn btn-info">Details</a>`
                },
                orderable: false
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<a href="/Admin/Product/Edit/${data}" class="btn btn-success"> Edit </a>`
                },
                orderable: false
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a onClick=DeleteItem("/Admin/Product/DeleteProduct/${data}") class="btn btn-danger">Delete</a>`
                },
                orderable: false
            }
        ],
        // Customize responsive behavior
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.modal({
                    header: function (row) {
                        return 'Details for ' + row.data().name;
                    }
                }),
                renderer: $.fn.dataTable.Responsive.renderer.tableAll({
                    tableClass: 'table table-striped'
                })
            }
        },

        // Add DOM configuration for better responsive layout
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',

        // Add language configuration for better UX
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search Products..."
        }
    });
}

// Add window resize handler
$(window).on('resize', function () {
    if (dtble) {
        dtble.columns.adjust().responsive.recalc();
    }
});


function DeleteItem(urlPath) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: urlPath,
                type: "Delete",
                success: function (data) {
                    if (data.success == true) {
                        dtble.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
            //Swal.fire(
            //    'Deleted!',
            //    'The product has been deleted.',
            //    'success'
            //);
        }
    });
}