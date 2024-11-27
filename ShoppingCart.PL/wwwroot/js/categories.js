var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#CategoriesTable").DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Admin/Category/GetCategoriesData"
        },
        "columns": [
            {
                "data": "name",
                "responsivePriority": 1
            },
            {
                "data": "description",
                "render": function (data) {
                    const maxLength = 50;
                    if (data.length > maxLength) {
                        return data.slice(0, maxLength) + '...';
                    }
                    return data;
                },
                "responsivePriority": 3
            },
            {
                "data": "createdTime",
                "render": function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString(undefined, {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    });
                },
                "responsivePriority": 4
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<a href="/Admin/Category/Edit/${data}" class="btn btn-success"> Edit </a>`
                },
                "responsivePriority": 2,
                orderable: false
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<a onClick=DeleteItem("/Admin/Category/DeleteCategory/${data}") class="btn btn-danger">Delete</a>`
                },
                "responsivePriority": 2,
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
                    tableClass: 'table'
                })
            }
        }
    });
}

// Add window resize handler to adjust table
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
        }
    });
}