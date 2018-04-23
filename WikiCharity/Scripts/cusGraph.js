
var chartData1; // globar variable for hold chart data
var chartData2;
google.load("visualization", "1", { packages: ["corechart"] });
google.setOnLoadCallback(drawChart);
// Here We will fill chartData

$(document).ready(function () {

    var url = window.location.pathname;
    var currid = url.substr(url.lastIndexOf('/') + 1);
    var d1 = JSON.stringify({
        'Id': currid
    });

    $.ajax({
        url: "/Home/GetChartData",
        data: d1,
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            chartData1 = data;
        },
        error: function () {
            alert("Error loading data! Please try again.");
        }
    }).done(function () {
        // after complete loading data
        //google.setOnLoadCallback(drawChart);
        //drawChart();
    });
    $.ajax({
        url: "/Home/GetBarData",
        data: d1,
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            chartData2 = data;
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
    var data1 = google.visualization.arrayToDataTable(chartData1);
    var data2 = google.visualization.arrayToDataTable(chartData2);

    var options = {
        title: "Financial Chart",
        pointSize: 5
    };

    var lineChart = new google.visualization.LineChart(document.getElementById('chart_div1'));
    lineChart.draw(data1, options);
    var barChart = new google.visualization.BarChart(document.getElementById('chart_div2'));
    barChart.draw(data2, options);

}

$(window).resize(function () {
    drawChart();
});

function showHideCode() {
    $("#showdiv").toggle();
}

function showHideCode() {
    $("#showdiv1").toggle();
}