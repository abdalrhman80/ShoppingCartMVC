var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#UsersTable").DataTable({
        "ajax": {
            "url": "/Admin/Users/GetUsersData"
        },
        "columns": [
            { "data": "userName" },
            { "data": "fullName" },
            { "data": "email" },
            {
                "data": "phoneNumber",
                "render": function (data) {
                    return data || '<span class="text-muted">No Phone Number</span>';
                }
            },
            {
                "data": "lockoutEnd",
                "render": function (data, type, row) {
                    const isLocked = data == null || new Date(data) < new Date();
                    return isLocked ?
                        `<a href="/Admin/Users/LockUnlock/${row.id}"  class="btn btn-success lock-user"  title="Account Active">
                            <i class="fas fa-lock-open"></i>
                         </a>` : 
                        `<a href="/Admin/Users/LockUnlock/${row.id}" class="btn btn-danger unlock-user" asp-action="LockUnlock" title="Account Locked">
                            <i class="fas fa-lock"></i>
                         </a>`;
                },
                orderable: false
            },
        ],
        "responsive": true
    });
}