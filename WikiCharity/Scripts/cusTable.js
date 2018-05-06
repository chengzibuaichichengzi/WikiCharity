$(document).ready(function () {
    var tableInit = $('#example ').DataTable({     
        "ajax": {
            "url": "/Home/AjaxGetJsonData",
            "type": "POST",
            "dataType": "json"
        },
        "columns": [
            
            { "data": "Name", "name": "Name" },
            { "data": "Size", "name": "Size" },
            { "data": "conActivity", "name": "conActivity" },
            { "data": "MainActivity", "name": "MainActivity" },
            { "data": "selectedBenes", "name": "selectedBenes" },
            /*{
                //only show fisrt 300 chars in the datatable row of description
                "data": "Description", "name": "Description", "render": function (data, type, row) {
                    return type == 'display' && data.length > 100 ?
                    data.substr(0, 300) + '......' :
                        data;
                }
            },*/
            {
                //use id column as detail button
                "data": "Id", "name": "Id", "render": function (data, type, full) {
                    return '<a class="btn btn-info btn-sm" href=/Home/Detail/' + data + '>' + 'More Details >>' + '</a>';
                }
            },
        ],
        "serverSide": true,
        "order": [0, "asc"],
        "processing": true,
        "language": {
            "processing": "Loading data.........Please wait"
        }
    });
    
});