
$(document).ready(function () {

    $("#size").chosen({
        width: "100%",
        //max_selected_options: 3,
        placeholder_text_multiple: "Any Charity Size"
    });



    $("#bene").chosen({
        width: "100%",
        max_selected_options: 5,
        placeholder_text_multiple: "Any Beneficiary"
    });

    $("#acti").chosen({
        width: "100%",
        max_selected_options: 3,
        placeholder_text_multiple: "Any Purpose"
    });

    var tableInit = $('#example').DataTable({
        fixedHeader: true,
        responsive:true,
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
                    id = data;
                    
                    return '<a class="btn btn-info btn-sm" href=/Home/Detail/' + data + '>' + 'More Details >>' + '</a>';
                }
            },
            {
                //use id column as detail button
                "data": "Id", "name": "Id", "render": function (data, type, full) {
                    if (full.isSelected == 'Y') {
                        return '<button id="' + data + '">Stored in list</button>';
                    }
                    else {
                        return '<button id="' + data + '">Add to list</button>';
                    }
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


    $('#example').on('click', 'button', function () {

        //full screen
        var data1 = tableInit.row(this.closest('tr')).data().Id;
        //small screen
        //var data1 = tableInit.row(this).data().Id;
        var data = JSON.stringify({
            'id': data1
        });
        $.ajax({
            type: 'POST',
            url: "/Home/AddToList",
            dataType: "json",
            data: data,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.isSelected == true) {

                    var text = "Stored in list";
                    //this.value = text;
                    var a = document.getElementById(data1.toString());
                    a.innerText = text;
                }
                else {
                    var text = "Add to list";
                    //this.value = text;
                    var a = document.getElementById(data1.toString());
                    a.innerText = text;
                }
            },
            error: function () {

            }
        });
    });
    
    
});

/*$('#example').on('click', '#btn1', function () {
    //var $btn = $(this);
    //var $tr = $btn.closest('tr');
    var $tr = $(this).closest('tr');
    //var dataTableRow = tableInit.row($tr[0]);
    var dataTableRow = tableInit.row($tr);
    var data = dataTableRow.data();
    console.log(data[0] + "size is" + data[1]);
});*/



