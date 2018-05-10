$(document).ready(function () {
    document.getElementById("compareBtn").disabled = true;
    document.getElementById("compareBtn2").disabled = true;
    document.getElementById("compareBtn").style.visibility = 'hidden';
    document.getElementById("compareBtn2").style.visibility = 'visible';
    var tableInit = $('#favTable').DataTable({
        fixedHeader: true,
        responsive: true,
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "columnDefs": [{
            "targets": [6],
            "visible": false
        }]
    });

    $('#favTable').on('click', 'button', function () {
        //full screen
        var data1 = tableInit.row(this.closest('tr')).data()[6];
        //small screen
        //var data1 = tableInit.row(this).data().Id;
        var data = JSON.stringify({
            'id': data1
        });
        $.ajax({
            type: 'POST',
            url: "/Compare/AddToCompare",
            dataType: "json",
            data: data,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.isSelected == true) {
                   
                    var text = "Prepare to compare";
                    //this.value = text;
                    var a = document.getElementById(data1.toString());
                    //get all buttons
                    var buttons = document.getElementsByName("all");
                    a.innerText = text;
                    if (data.compareNum == 2) {
                        document.getElementById("compareBtn").style.visibility = 'hidden';
                        document.getElementById("compareBtn").action = "/Compare/compare";
                        document.getElementById("compareBtn").disabled = false;
                        document.getElementById("compareBtn2").style.visibility = 'visible';
                        document.getElementById("compareBtn2").action = "/Compare/compare";
                        document.getElementById("compareBtn2").disabled = false;
                    }
                    if (data.compareNum == 3) {
                        for (var i = 0; i < buttons.length; i++) {
                            if (buttons[i].innerText != "Prepare to compare") {
                                buttons[i].disabled = true;
                            }
                        }
                        //go to 3 charities compare page
                        document.getElementById("compareBtn").action = "/Compare/compares";
                        document.getElementById("compareBtn").disabled = false;
                        document.getElementById("compareBtn").style.visibility = 'visible';
                        document.getElementById("compareBtn2").action = "/Compare/compares";
                        document.getElementById("compareBtn2").disabled = false;
                        document.getElementById("compareBtn2").style.visibility = 'hidden';
                    }
                }
                else {
                    document.getElementById("compareBtn").style.visibility = 'hidden';
                    document.getElementById("compareBtn2").style.visibility = 'visible';
                    var text = "Compare it";
                    //this.value = text;
                    var a = document.getElementById(data1.toString());
                    //get all buttons
                    var buttons = document.getElementsByName("all");
                    a.innerText = text;
                    for (var i = 0; i < buttons.length; i++) {                       
                        buttons[i].disabled = false;
                    }
                    if (data.compareNum < 2) {
                        //document.getElementById("compareBtn").formaction = "/Compare/compare";
                        // document.getElementById("compareBtn").disabled = false;
                        document.getElementById("compareBtn").disabled = true;
                        document.getElementById("compareBtn2").disabled = true;
                    }
                    
                    //go to 2 charities compare page
                    //document.getElementById("compareBtn").formaction = "/Compare/compare";
                    
                }
            },
            error: function () {

            }
        });
    });
});