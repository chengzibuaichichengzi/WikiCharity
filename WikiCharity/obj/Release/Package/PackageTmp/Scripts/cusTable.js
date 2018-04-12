$(document).ready(function () {
    $('#example').DataTable({
        
        "ajax": {
            "url": "/Home/AjaxGetJsonData",
            "type": "POST",
            "dataType": "json"
        },
        "columns": [
            { "data": "Name", "name": "Name"},
            { "data": "Beneficiaries", "name": "Beneficiaries" },
            { "data": "Tax", "name": "Tax" },
            { "data": "Size", "name": "Size" },
            { "data": "State", "name": "State" },
        ],
        "serverSide": true,
        "order": [0, "asc"],
        "processing": true,
        "language": {
            "processing": "Loading data.........Please wait"
        }
    });
    
});