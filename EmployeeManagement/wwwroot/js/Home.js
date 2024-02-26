
$(document).on("click", "#editEmployeeButton", function () {
    var employeeId = $(this).data("employee-id");
    $.get("/Home/Edit", { Id: employeeId }, function (data) {
        $("#showPartialViewModal .modal-content").html(data);
        $("#showPartialViewModal").modal("show");
    });
});
//Remove button
$(document).on("click", "#removeEmployeeButton", function () {
    var employeeId = $(this).data("employee-id");
    $.get("/Home/Delete", { Id: employeeId }, function (data) {
        $("#showPartialViewModal .modal-content").html(data);
        $("#showPartialViewModal").modal("show");
    });
});
//Delete button (after password verification)
$(document).on("click", "#deleteEmployeeButton", function () {
    var employeeId = $(this).data("employee-id");
    $(".form-control").removeClass("border border-danger");
    var password = $("#password").val();
    var confirmPassword = $("#confirmpassword").val();
    // Check for empty fields
    var isValid = true;
    $(".form-control[required]").each(function () {
        if ($(this).val() === "") {
            isValid = false;
            $(this).addClass("border border-danger");
        }
    });

    // If any field is empty, prevent form submission
    if (!isValid) {
        return false;
    }
    $.ajax({
        url: '/Home/Remove',
        type: 'POST',
        data: { Id: employeeId, password: password, confirmPassword: confirmPassword },
        dataType: 'json',  // Specify the expected data type
        success: function (result) {
            if (result.success) {
                // Employee deleted successfully, redirect to home page
                window.location.href = '/';
            } else {
                // Display an error message
                showToastEdit(result.message, 'danger');
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 400) {
                // Bad Request - Passwords do not match
                showToastEdit(xhr.responseJSON.message, 'danger');
            } else if (xhr.status === 401) {
                // Unauthorized - Incorrect Password
                showToastEdit(xhr.responseJSON.message, 'danger');
            } else {
                // Handle other errors if needed
                showToastEdit('An error occurred', 'danger');
            }
        }
    });
;
});

//add employee (Opens Edit page with null as id)
$(document).on("click", "#addEmployeeButton", function () {
    $.get("/Home/Edit", { Id: null }, function (data) {
        $("#showPartialViewModal .modal-content").html(data);
        $("#showPartialViewModal").modal("show");
    });
});

//show log button
$(document).on("click", "#showLogs", function () {
    $.get("/Home/GetLogData", { Id: null }, function (data) {
        $("#showPartialViewModal .modal-content").html(data);
        $("#showPartialViewModal").modal("show");

    });
});

$(document).on('click', '.btnConfirm', function () {
    $(".form-control").removeClass("border border-danger");
    // Check for empty fields
    var isValid = true;

    $(".form-control[required]").each(function () {
        if ($(this).val() === "") {
            isValid = false;
            $(this).addClass("border border-danger");
        }
    });

    // If any field is empty, prevent form submission
    if (!isValid) {
        return false;
    }
    var formData = $('#editForm').serialize();
    $.ajax({
        url: '/Home/SaveDetails',
        type: 'POST',
        data: formData,
        success: function (result) {
            window.location.href = '/';
            showToastLayout(result, 'success');
        },
        error: function (error) {
            showToastEdit('Incorrect Password', 'danger');
        }
    });
});
$(document).on('click', '.btnDelete', function () {
    $(".form-control").removeClass("border border-danger");
    // Check for empty fields
    var isValid = true;

    $(".form-control[required]").each(function () {
        if ($(this).val() === "") {
            isValid = false;
            $(this).addClass("border border-danger");
        }
    });

    // If any field is empty, prevent form submission
    if (!isValid) {
        return false;
    }
    var formData = $('#editForm').serialize();
    $.ajax({
        url: '/Home/SaveDetails',
        type: 'POST',
        data: formData,
        success: function (result) {
            window.location.href = '/';
            showToastLayout(result, 'success');
        },
        error: function (error) {
            showToastEdit('Incorrect Password', 'danger');
        }
    });
});

//jquery toaster alert
function showToastLayout(message, type = 'success') {
    var toastContainer = $('.layout_toast');
    var toastHtml = `
        <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>                
            </div>
        </div>`;

    toastContainer.append(toastHtml);

    var toast = toastContainer.find('.toast:last');
    var toastInstance = new bootstrap.Toast(toast[0]);
    toastInstance.show();
}
function showToastEdit(message, type = 'success') {
    var toastContainer = $('.edit_toast');
    var toastHtml = `
        <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>                
            </div>
        </div>`;

    toastContainer.append(toastHtml);

    var toast = toastContainer.find('.toast:last');
    var toastInstance = new bootstrap.Toast(toast[0]);
    toastInstance.show();
}

