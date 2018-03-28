$(document).ready(function () {
    $('#example').DataTable({
    });

    $('select').change(function () {
        var state1 = $('#state').val();
        var bene1 = $("#bene").val();
        var size1 = $("#size").val();
        var tax1 = $("#tax").val();

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
                var text = "Show " + data.countNum + " charities we find for you"
                $('#button1').text(text);
            },
            error: function () {
                alert(ErrorEvent);
            }
        });
    });
});

