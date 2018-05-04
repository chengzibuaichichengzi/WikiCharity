var pageSize = 10;
function pageData(e) {
    var skip = e == 1 ? 0 : (e * pageSzie) - pageSize;
    $ajax({
        type: "POST",
        url: "/Home/FetchCharities",
        data: "{skip:" + skip + ",take:" + pageSize + "}",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (msg) {
            printCharity(msg);
        }
    });
    return false;
}

$(document).ready(function () {
    $("#btnSearch").click(function () {
        $.ajax({
            type: "POST",
            url: "/Home/FetchCharities",
            data: "{skip:0 ,take:" + pageSize + "}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                var total = msg.d.TotalRecords;
                if (total > 0) {
                    printCharity(msg);
                    $("#paging").text("");
                    var pageTotal = Math.ceil(total / pageSize);
                    for (i = 0; i < pageSize; i++) {
                        $("#paging").append("<a href=\"#\" onClick=\"pageData(" + (i + 1) + ")\">" + (i + 1) + "</a>&nbsp;");
                    }
                }
                else {
                    $("paging").text("No Records");
                }
                $("#totalRecords").text("Total Records: " + total);
            }
        });
    });
});

function printCharity(msg) {
    $("#result").text("");
    var charities = msg.d.Charities;
    for (var i = 0; i < charities.length; i++) {
        $("#result").append(charities[i].ABN + ", ");
        $("#result").append(charities[i].Name + ", ");
        $("#result").append(charities[i].State + ", ");
        $("#result").append(charities[i].Size + ", ");
        $("#result").append(charities[i].Tax + ", ");
        $("#result").append(charities[i].Description + ", ");
        $("#result").append(charities[i].Beneficiaries + "<br />");
    }
}