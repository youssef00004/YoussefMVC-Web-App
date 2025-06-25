﻿$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group rounded" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary rounded mx-2"> <i class="bi bi-pencil-square"></i> Edit </a>
                    <a class="btn btn-danger rounded mx-2"> <i class="bi bi-trash-fill"></i> Delete </a>
                    </div>`
                },
                "width":"25%"
            }
        ]
    });
}






