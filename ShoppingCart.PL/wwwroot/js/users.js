var dtble;

$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $("#UsersTable").DataTable({
        // Enable responsive features
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": "/Admin/Users/GetUsersData"
        },
        "columns": [
            {
                "data": "userName",
                "responsivePriority": 1, // Username should always be visible
            },
            {
                "data": "fullName",
                "responsivePriority": 2,
                "render": function (data) {
                    return data || '<span class="text-muted">Not Provided</span>';
                }
            },
            {
                "data": "email",
                "responsivePriority": 3,
                "render": function (data) {
                    return `<span class="text-primary">${data}</span>`;
                }
            },
            {
                "data": "phoneNumber",
                "responsivePriority": 4,
                "render": function (data) {
                    return data || '<span class="text-muted">No Phone Number</span>';
                }
            },
            {
                "data": "lockoutEnd",
                "responsivePriority": 2, // Keep lock status visible longer
                "className": "text-center",
                "render": function (data, type, row) {
                    const isLocked = data == null || new Date(data) < new Date();
                    const buttonClass = isLocked ? 'btn-success' : 'btn-danger';
                    const iconClass = isLocked ? 'fa-lock-open' : 'fa-lock';
                    const title = isLocked ? 'Account Active' : 'Account Locked';
                    const statusBadge = isLocked ?
                        '<span class="badge bg-success text-white ms-2">Active</span>' :
                        '<span class="badge bg-danger text-white ms-2">Locked</span>';

                    return `
                        <div class="d-flex align-items-center justify-content-center">
                            <a href="/Admin/Users/LockUnlock/${row.id}" 
                               class="btn ${buttonClass} btn-sm lock-toggle" 
                               data-user-id="${row.id}"
                               title="${title}">
                                <i class="fas ${iconClass}"></i>
                            </a>
                            ${statusBadge}
                        </div>`;
                },
                orderable: false
            },
        ],
        // Add DOM configuration for better layout
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',

        // Add language configuration
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search Users...",
            lengthMenu: "Show _MENU_ users per page",
            info: "Showing _START_ to _END_ of _TOTAL_ users",
            emptyTable: "No users found"
        },

        // Default sorting by username
        order: [[0, 'asc']]
    });
}

// Add window resize handler
$(window).on('resize', function () {
    if (dtble) {
        dtble.columns.adjust().responsive.recalc();
    }
});
