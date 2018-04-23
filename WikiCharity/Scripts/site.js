$(document).ready(function () {

    $(".chosen-select-multiple").chosen({
        width: "100%",
        max_selected_options: 5
    });

    $('.select-state, .select-bene, .select-size, .select-tax').change(function () {
        var state1 = $('#state').val();
        //var bene1 = $("#bene").val();
        var size1 = $("#size").val();
        var tax1 = $("#tax").val();
        var beneAll = document.getElementById("bene");
        var bene1 = "";
        var len = beneAll.options.length;
        for (var i = 0; i < len; i++) {
            if (beneAll.options[i].selected) {
                bene1 = bene1 + beneAll.options[i].text + ",";
            }
        }
        var data = JSON.stringify({
            'state': state1,
            'bene': bene1,
            'size': size1,
            'tax': tax1
        });
        
        $.ajax({
            type: 'POST',
            url: "/Home/CountResult",
            dataType: "json",
            data: data,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var text = "Click to show the " + data.countNum + " charities"
                $('#button1').text(text);
            },
            error: function () {
                
            }
        });
    });

   // $('#button2').click(function () {
     //   alert("Your message has been sent");
    //});

    $("#size").tooltip({
        
        content: function(callback){
            callback($(this).prop('title').replace('|','<br/>'));
    }
    });
});

