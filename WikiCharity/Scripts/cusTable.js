$(document).ready(function () {
    var tableInit = $('#example ').DataTable({
        rowReorder: {
            selector: 'td:nth-child(2)'
        },
        responsive: true,
        
        "ajax": {
            "url": "/Home/AjaxGetJsonData",
            "type": "POST",
            "dataType": "json"
        },
        "columns": [
            
            {"data": "Name", "name": "Name"},
            {"data": "Description", "name": "Description" },
            {
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
    
    $('example tbody').on('dblclick', 'tr', function () {
        var currentRowData = tableInit.row(this).data();
        alert(currentRowData[0]);
        //var table = $('#example ').DataTable();
        //console.log('TD Cell textContent: ', this.textContent)
        //console.log('value by API: ', table.cell({ row: this.ParentNode, column: this.cellIndex }).data());
    });
    
});