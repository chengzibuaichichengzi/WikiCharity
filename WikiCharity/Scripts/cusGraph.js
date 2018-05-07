// globar variable for hold chart data
var chartData1; 
var chartData2;
var chartData3;
google.load("visualization", "1", { packages: ["corechart"] });

// Here We will fill chartData
$(document).ready(function () {

    //get the current charity Id through the url
    var url = window.location.pathname;
    var currid = url.substr(url.lastIndexOf('/') + 1);
    var d1 = JSON.stringify({
        'Id': currid
    });

    //get the data for line chart
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

    //get the data for revenue and expense bar chart
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
        //google.setOnLoadCallback(drawChart);
        //drawChart();
    });

    //get the data for total assets and liabilities bar chart
    $.ajax({
        url: "/Home/GetBar2Data",
        data: d1,
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            chartData3 = data;
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
    var data3 = google.visualization.arrayToDataTable(chartData3);

    var options = {
        title: "Financial Chart",
        pointSize: 5
    };

    var options2 = {
        title: "Financial Chart",
        pointSize: 5,
        colors: ['orange', 'gray']
    };

    var lineChart = new google.visualization.LineChart(document.getElementById('chart_div1'));
    lineChart.draw(data1, options);
    var barChart = new google.visualization.BarChart(document.getElementById('chart_div2'));
    barChart.draw(data2, options);
    var barChart2 = new google.visualization.ColumnChart(document.getElementById('chart_div3'));
    barChart2.draw(data3, options2);
}


//$(window).resize(function () {
 //   drawChart();
//});

function showHideCode() {
    $("#showdiv").toggle();
}

function showHideCode1() {
    $("#showdiv1").toggle();
}

function showHideDesc() {
    $("#showDesc").toggle();
    $("#lessDesc").toggle();

}