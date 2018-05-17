
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
                        //return '<button id="' + data + '">Added</button>';
                    //return '<button class="btn btn-success" id="' + data + '" name="allBtn">Add</button>';
                    return '<input type="image", src="../Uploads/heart.png" name="addtolist" class="btTxt submit" id="' + data + '" />';
                }
            },
        ],
        
        "serverSide": true,
        "order": [0, "asc"],
        "processing": true,
        "language": {
            "processing": "Loading data.........Please wait"
        },

        //after the table finishing loading, change buttons of charities which are already in favorite list
        "initComplete": function (settings, json) {
            
            
            $.ajax({
                type: 'POST',
                url: "/Home/CheckList",
                dataType: "json",
                data: null,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                        var buttons = document.getElementsByName("addtolist");
                        for (var i = 0; i < buttons.length; i++) {
                            for (var x = 0; x < data.length; x++) {
                                var obj = data[x];
                                var text = obj.id;
                                if (obj.id == buttons[i].id) {
                                    buttons[i].src = "../Uploads/hearted.png";
                                }
                            }
                        }
                },
                error: function () {

                }
            });
        }
    });

    $('#example').on('draw.dt', function () {
        $.ajax({
            type: 'POST',
            url: "/Home/CheckList",
            dataType: "json",
            data: null,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var buttons = document.getElementsByName("allBtn");
                for (var i = 0; i < buttons.length; i++) {
                    for (var x = 0; x < data.length; x++) {
                        var obj = data[x];
                        var text = obj.id;
                        if (obj.id == buttons[i].id) {
                            buttons[i].src = "../Uploads/hearted.png";
                        }
                    }
                }
            },
            error: function () {
            }
        });
    });

    $('#example').on('click', 'input', function () {

        var x = tableInit.row(this.closest('tr')).data();
        //full screen
        //
        if (x == null) {
            //small screen
            var data1 = tableInit.row(this).data().Id;
        }
        else {
            var data1 = x.Id;
        }
        
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

                    //var text = "Added";
                    //this.value = text;
                    var a = document.getElementById(data1.toString());
                    a.src = "../Uploads/hearted.png";
                }
                else {
                    //var text = "Add";
                    //this.value = text;
                    var a = document.getElementById(data1.toString());
                    a.src = "../Uploads/heart.png";
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



