var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#CategoriesTable").DataTable({
        "ajax": {
            "url": "/Admin/Category/GetCategoriesData"
        },
        "columns": [
            { "data": "name" },
            {
                "data": "description",
                "render": function (data) {
                    const maxLength = 50;
                    if (data.length > maxLength) {
                        return data.slice(0, maxLength) + '...';
                    }
                    return data;
                }
            },
            {
                "data": "createdTime",
                "render": function (data) {
                    return new Date(data).toLocaleDateString();
                },
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<a href="/Admin/Category/Edit/${data}" class="btn btn-success"> Edit </a>`
                },
                orderable: false
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<a onClick=DeleteItem("/Admin/Category/DeleteCategory/${data}") class="btn btn-danger">Delete</a>`
                },
                orderable: false
            }
        ]
    });
}

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