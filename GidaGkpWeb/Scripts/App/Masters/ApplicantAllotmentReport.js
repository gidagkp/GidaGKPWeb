/// <reference path="../Global/Utility.js" />
'use strict';


$(document).ready(function () {

    FillSchemeType();
    function FillSchemeType(selectedSchemeTypeId = null) {
        let dropdown = $('#SchemeType');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "SchemeType" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedSchemeTypeId != null) {
                    dropdown.val(selectedSchemeTypeId);
                }
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }

    $('#SchemeType').on('change', function (e) {
        var valueSelected = this.value;
        FillSchemeName(valueSelected);
    });

    function FillSchemeName(SchemeTypeId, selectedSchemeNameId = null) {
        let dropdown = $('#SchemeName');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: "' + SchemeTypeId + '",lookupType: "SchemeName",active:"false" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedSchemeNameId != null) {
                    dropdown.val(selectedSchemeNameId);
                }
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }
    $('#SchemeName').on('change', function (e) {
        var valueSelected = this.value;
        //FillSector(valueSelected);
    });

    $('#btnSearch').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Report/GetSelectedApplicant',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<td class="text-center">' + index + '</td>' +
                        '<td class="text-center">' + entry.FullName + '</td>' +
                        '<td class="text-center">' + entry.ApplicationNumber + '</td>' +
                        '<td class="text-center">' + entry.PlotNumber + '</td>';
                    rowHtml += '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="4">No Record Found.</td>' +
                        '</tr>';
                }
                $('#tbodyData').append(rowHtml);
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });

    $('#btnSearchRejected').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Report/GetRejectedApplicant',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<td class="text-center">' + index + '</td>' +
                        '<td class="text-center">' + entry.FullName + '</td>' +
                        '<td class="text-center">' + entry.ApplicationNumber + '</td>' +
                        '<td class="text-center">' + entry.PlotNumber + '</td>';
                    rowHtml += '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="4">No Record Found.</td>' +
                        '</tr>';
                }
                $('#tbodyData').append(rowHtml);
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });
    
});



