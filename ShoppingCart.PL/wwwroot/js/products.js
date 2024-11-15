var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#ProductTable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetProductsData"
        },
        "columns": [
            {
                "data": "name", "render": function (data) {
                    const maxLength = 50;
                    if (data.length > maxLength) {
                        return data.slice(0, maxLength) + '...';
                    }
                    return data; // Return original data if it doesn't exceed maxLength
                }
            },
            {
                "data": "description",
                "render": function (data) {
                    const maxLength = 50;
                    if (data.length > maxLength) {
                        return data.slice(0, maxLength) + '...';
                    }
                    return data; // Return original data if it doesn't exceed maxLength
                }
            },
            {
                "data": "price",
                "render": function (data) {
                    return data.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                }
            },
            { "data": "category.name" },
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