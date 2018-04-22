$(document).ready(function () {
    var tableInit = $('#example ').DataTable({     
        "ajax": {
            "url": "/Home/AjaxGetJsonData",
            "type": "POST",
            "dataType": "json"
        },
        "columns": [
            
            {"data": "Name", "name": "Name"},
            {
                //only show fisrt 300 chars in the datatable row of description
                "data": "Description", "name": "Description", "render": function (data, type, row) {
                    return type == 'display' && data.length > 300 ?
                    data.substr(0, 300) + '......' :
                        data;
                }
            },
            {
                //use id column as detail button
                "data": "Id", "name": "Id", "render": function (data, type, full) {
                    return '<a class="btn btn-info btn-sm" href=/Home/Detail/' + data + '>' + 'Detail' + '</a>';
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