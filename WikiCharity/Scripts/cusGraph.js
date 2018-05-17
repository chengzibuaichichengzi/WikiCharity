window.onbeforeprint = beforePrint;
window.onafterprint = afterPrint;

// globar variable for hold chart data
var chartData1;
var chartData2;
var chartData3;
google.load("visualization", "1", { packages: ["corechart"] });

// Here We will fill chartData
$(document).ready(function () {
    //$("#myprint").click(function(){
    //    window.print();
    //})

    //enable tooltip on mobile
    $(document).tooltip();

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

    });
});

$(document).ajaxStop(function () {

    google.setOnLoadCallback(drawChart);
    drawChart();
});

function drawChart() {
    var data1 = google.visualization.arrayToDataTable(chartData1);
    var data2 = google.visualization.arrayToDataTable(chartData2);
    var data3 = google.visualization.arrayToDataTable(chartData3);

    var options = {
        //title: "Financial Chart",
        pointSize: 5
    };

    var options2 = {
        //title: "Financial Chart",
        pointSize: 5,
        colors: ['orange', 'gray']
    };
    var options3 = {
        //title: "Financial Chart",
        pointSize: 5,
        colors: ['skyblue', '#5574A6']
    };

    var lineChart = new google.visualization.LineChart(document.getElementById('chart_div1'));
    lineChart.draw(data1, options);
    var barChart = new google.visualization.BarChart(document.getElementById('chart_div2'));
    barChart.draw(data2, options3);
    var barChart2 = new google.visualization.ColumnChart(document.getElementById('chart_div3'));
    barChart2.draw(data3, options2);
}

//auto resize
$(window).resize(function () {
    drawChart();
});

function showTable() {
    $("#showdiv").toggle();
    $("#showTable").toggle();
    $("#hideTable").toggle();
}

function showExpDiv() {
    $("#showdiv1").show(0);
    //$("#showExp").toggle();
    $("#hideExp").show(0);
    $("#showExp").hide(0);
}
function HideExpDiv() {
    $("#showdiv1").toggle();
    //$("#showExp").toggle();
    $("#hideExp").hide(0);
    $("#showExp").show(0);
}


function showHideDesc() {
    $("#showDesc").show(0);
    $("#lessDesc").hide(0);
    $("#showlessbtn").show(0);
    $("#showmorebtn").hide(0);
}

function hideDesc() {
    $("#showDesc").toggle();
    $("#lessDesc").toggle();
    $("#showlessbtn").toggle();
    $("#showmorebtn").toggle();
}


function beforePrint() {
    $("#showdiv1").show(0);
    $("#showdiv").show(0);
    $("#showExp").hide(0);
    $("#hideExp").hide(0);
    $("#showTable").hide(0);
    $("#hideTable").hide(0);
    $("#showDesc").show(0);
    $("#lessDesc").hide(0);
    $("#showlessbtn").hide(0);
    $("#showmorebtn").hide(0);
}



function afterPrint() {
    $("#showdiv1").hide(0);
    $("#showdiv").hide(0);
    $("#showExp").show(0);
    $("#hideExp").hide(0);
    $("#showTable").show(0);
    $("#hideTable").hide(0);

    $("#showDesc").hide(0);
    $("#lessDesc").show(0);
    $("#showlessbtn").hide(0);
    $("#showmorebtn").show(0);

}