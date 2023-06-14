var dataTable;

$(document).ready(function () {
    var url = window.location.href;
    if (url.includes("latest")) {
        loadDataTable("latest");
    }
    else {
        if (url.includes("mostViewed")) {
            loadDataTable("mostViewed");
        }
        else {
            if (url.includes("mostInteresting")) {
                loadDataTable("mostInteresting");
            }
            else {
                if (url.includes("published")) {
                    loadDataTable("published");
                }
                else {
                    if (url.includes("unPublished")) {
                        loadDataTable("unPublished");
                    }
                    else {
                        loadDataTable("all");
                    }
                }
            }
        }
    }
  
});
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Posts/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "title", "width": "15%" },
            { "data": "shortDescription", "width": "25%" },
            { "data": "postedOn", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Posts/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/Posts/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}