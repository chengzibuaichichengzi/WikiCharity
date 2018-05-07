$(document).ready(function () {
    //.chosen-select-multiple
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

    var textInput = document.getElementById('name');
    var timeout = null;
    textInput.onkeyup = function (e) {
        clearTimeout(timeout);
        var state1 = $('#state').val();
        //var bene1 = $("#bene").val();
        //var size1 = $("#size").val();
        var tax1 = $("#tax").val();
        var beneAll = document.getElementById("bene");
        var sizeAll = document.getElementById("size");
        //var stateAll = document.getElementById("state");
        var actiAll = document.getElementById("acti");
        var acti1 = "";
        var len = actiAll.options.length;
        for (var i = 0; i < len; i++) {
            if (actiAll.options[i].selected) {
                acti1 = acti1 + actiAll.options[i].text + ",";
            }
        }
        var bene1 = "";
        var len = beneAll.options.length;
        for (var i = 0; i < len; i++) {
            if (beneAll.options[i].selected) {
                bene1 = bene1 + beneAll.options[i].text + ",";
            }
        }
        var size1 = "";
        var len = sizeAll.options.length;
        for (var i = 0; i < len; i++) {
            if (sizeAll.options[i].selected) {
                size1 = size1 + sizeAll.options[i].text + ",";
            }
        }
        

        
        timeout = setTimeout(function () {
            var data = JSON.stringify({
                'state': state1,
                'bene': bene1,
                'size': size1,
                'tax': tax1,
                'name': textInput.value,
                'acti': acti1
            });

            $.ajax({
                type: 'POST',
                url: "/Home/CountResult",
                dataType: "json",
                data: data,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.countNum != 0) {

                        var text = "Click to show the " + data.countNum + " charities";
                        $('#button1').text(text);
                        document.getElementById("button1").disabled = false;
                    }
                    else {
                        var text = "No charities available";
                        $('#button1').text(text);
                        document.getElementById("button1").disabled = true;
                    }
                    
                },
                error: function (e) {
                    console.log(e);
                }
            });

            console.log(textInput.value);
        }, 500);
    };

    $('.select-state, .select-bene, .select-size, .select-tax, .select-act').change(function () {
        var state1 = $('#state').val();
        //var bene1 = $("#bene").val();
        //var size1 = $("#size").val();
        var tax1 = $("#tax").val();
        var beneAll = document.getElementById("bene");
        var sizeAll = document.getElementById("size");
        //var stateAll = document.getElementById("state");
        var actiAll = document.getElementById("acti");
        var acti1 = "";
        var actiLen = actiAll.options.length;
        for (var i = 0; i < actiLen; i++) {
            if (actiAll.options[i].selected) {
                acti1 = acti1 + actiAll.options[i].text + ",";
            }
        }
        var bene1 = "";
        var benelen = beneAll.options.length;
        for (var i = 0; i < benelen; i++) {
            if (beneAll.options[i].selected) {
                bene1 = bene1 + beneAll.options[i].text + ",";
            }
        }
        var size1 = "";
        var sizelen = sizeAll.options.length;
        for (var i = 0; i < sizelen; i++) {
            if (sizeAll.options[i].selected) {
                size1 = size1 + sizeAll.options[i].text + ",";
            }
        }
        
        var data = JSON.stringify({
            'state': state1,
            'bene': bene1,
            'size': size1,
            'tax': tax1,
            'name': "",
            'acti': acti1
        });
        
        $.ajax({
            type: 'POST',
            url: "/Home/CountResult",
            dataType: "json",
            data: data,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.countNum != 0) {

                    var text = "Click to show the " + data.countNum + " charities";
                    $('#button1').text(text);
                    document.getElementById("button1").disabled = false;
                }
                else {
                    var text = "No charities available";
                    $('#button1').text(text);
                    document.getElementById("button1").disabled = true;
                }
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

