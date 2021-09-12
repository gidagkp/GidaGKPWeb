/// <reference path="../Global/Utility.js" />
'use strict';
$(document).ready(function () {
    $("#txtApplicationNumber").change(function () {
        var ApplicationId = parseInt($("#txtApplicationNumber").val());
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{ApplicationId: "' + ApplicationId + '"}',
            url: '/Applicant/GetmortgageDetail',
            success: function (data) {

                $('#txtApplicationAddress').val(data.AddressOfApplicant);
                $('#txtSectorNumber').val(data.SectorName);
                $('#txtPlotNumber').val(data.PlotNo);
                $('#txtAllotmentNumber').val(data.AllotmentNumber);
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });

    });
    var i = 0;
    var j = 0;
    var num = 8;
    $('#btnAddNewRow').show();
    $('#btnAddNewRow1').show();
    $('#btnAddNewRow').click(function () {
        if (i < num) {
            var firstRow = $('#tableBodyPlot tr').eq(0);
            $('#tableBodyPlot').append("<tr>" + firstRow.html() + "</tr>");
            i++;
        }
        else {
            $('#btnAddNewRow').hide();
        }
    });
    $('#btnAddNewRow1').click(function () {
        if (j < num) {
            var firstRow = $('#tableBodyPlot1 tr').eq(0);
            $('#tableBodyPlot1').append("<tr>" + firstRow.html() + "</tr>");
            j++;
        }
        else {
            $('#btnAddNewRow1').hide();
        }
    });

});