﻿var dataTable;



$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Company/GetAll' },
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'city', "width": "15%" },
            { data: 'postalCode', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'streetAddress', "width": "10%" },
            {
                data: 'companyId',
                "render": function (data) {
                    return `<div class="w-75 btn-group rounded" role="group">
                    <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-primary rounded mx-2"> <i class="bi bi-pencil-square"></i> Edit </a>
                    <a onClick=Delete('/Admin/Company/delete/${data}') class="btn btn-danger rounded mx-2"> <i class="bi bi-trash-fill"></i> Delete </a>
                    </div>`
                },
                "width":"25%"
            }
        ]
    });
}

function Delete(url)
{
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
                url: url,
                type: 'DELETE',
                success: function (data) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: data.message,
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                    toastr.success(data.message);
                    dataTable.ajax.reload();                    
                }
            })

        }
    })

}





