var dataTable;



$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Order/GetAll' },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phonenumber', "width": "20%" },
            { data: 'applicationuser.email', "width": "15%" },
            { data: 'orderstatus', "width": "10%" },
            { data: 'status', "width": "10%" },
            { data: 'status', "width": "10%" },
            {
                data: 'productID',
                "render": function (data) {
                    return `<div class="w-75 btn-group rounded" role="group">
                    <a href="/Admin/order/details?orderid=${data}" class="btn btn-primary rounded mx-2"> <i class="bi bi-pencil-square"></i>  </a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}







