// globar variable for hold chart data
var chartData1;
google.load("visualization", "1", { packages: ["corechart"] });

$(document).ready(function () {

    //get the data for line chart
    $.ajax({
        url: "/Compare/GetBarData",
        data: null,
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
        google.setOnLoadCallback(drawChart);
        drawChart();
    });

    //enable tooltip on mobile
    $(document).tooltip();
});

/*$(document).ajaxStop(function () {
    google.setOnLoadCallback(drawChart);
    drawChart();
});*/

function drawChart() {
    var data1 = google.visualization.arrayToDataTable(chartData1);
    var options = {
        pointSize: 5,
        colors: ['skyblue', '#5574A6']

    };

    var barChart = new google.visualization.BarChart(document.getElementById('chart_div_new'));
    barChart.draw(data1, options);
}

$(window).resize(function () {
    drawChart();
});