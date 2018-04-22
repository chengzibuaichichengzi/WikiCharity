
var chartData; // globar variable for hold chart data
google.load("visualization", "1", { packages: ["corechart"] });

// Here We will fill chartData

$(document).ready(function () {

    var url = window.location.pathname;
    var currid = url.substr(url.lastIndexOf('/') + 1);
    var data1 = JSON.stringify({
        'Id': currid
    });

    $.ajax({
        url: "/Home/GetbarData",
        data: data1,
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            chartData = data;
        },
        error: function () {
            alert("Error loading data! Please try again.");
        }
    }).done(function () {
        // after complete loading data
        google.setOnLoadCallback(drawChart);
        drawChart();
    });
});


function drawChart() {
    var data = google.visualization.arrayToDataTable(chartData);

    var options = {
        title: "Financial Chart",
        pointSize: 5
    };

    var barChart = new google.visualization.BarChart(document.getElementById('chart_div2'));
    barChart.draw(data, options);

}

