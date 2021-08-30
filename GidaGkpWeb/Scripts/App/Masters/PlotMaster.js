/// <reference path="../Global/Utility.js" />
'use strict';


$(document).ready(function () {

    FillSchemeType();
    function FillSchemeType(selectedSchemeTypeId = null) {
        let dropdown = $('[id*=SchemeType]');
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
        let dropdown = $('[id*=SchemeName]');
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
        FillSector(valueSelected);
    });

    function FillSector(SchemeNameId) {
        let dropdown = $('[id*=SectorName]');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: "' + SchemeNameId + '",lookupType: "SectorName" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }

    $('#SectorName').on('change', function (e) {
        var valueSelected = this.value;
        $("#tableBodyPlot tr:not('[id*=baseRow]')").each(function () {
            $(this).remove();
        });
        var firstRow = $('#tableBodyPlot tr').eq(0);
        $('#tableBodyPlot').append("<tr>" + firstRow.html() + "</tr>");
        FillPlotMasterDetail(valueSelected);
    });

    function FillPlotMasterDetail(selectedSectorId) {
        var schemeTypeId = $('#SchemeType').val();
        var schemeNameId = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{SchemeType: ' + schemeTypeId + ',SchemeName: ' + schemeNameId + ',SectorId: ' + selectedSectorId + ' }',
            url: '/Admin/GetPlotMasterDetail',
            success: function (data) {
                for (var i = 0; i < data.length - 1; i++) {
                    var firstRow = $('#tableBodyPlot tr').eq(0);
                    $('#tableBodyPlot').append("<tr>" + firstRow.html() + "</tr>");
                }
                //$.each(data, function (key, entry) {
                var index = 0;
                $("#tableBodyPlot tr:not('[id*=baseRow]')").each(function () {
                    $(this).find('td').eq(0).find('input').val(data[index].PlotNumber);
                    $(this).find('td').eq(1).find('input').val(data[index].PlotArea);
                    $(this).find('td').eq(2).find('select').val(data[index].PlotRange);
                    $(this).find('td').eq(3).find('input').val(data[index].PlotRate);
                    if (data[index].PlotSideCorner)
                        $(this).find('td').eq(4).find('input[type="checkbox"]').eq(0).prop('checked', true)
                    if (data[index].PlotSideWideRoad)
                        $(this).find('td').eq(4).find('input[type="checkbox"]').eq(1).prop('checked', true)
                    if (data[index].PlotSideParkFacing)
                        $(this).find('td').eq(4).find('input[type="checkbox"]').eq(2).prop('checked', true)
                    $(this).find('td').eq(5).find('input').val(data[index].PercentageRate);
                    $(this).find('td').eq(6).find('select').val(data[index].PlotCategory);
                    $(this).find('td').eq(7).find('input').val(data[index].PlotCost);
                    $(this).find('td').eq(8).find('input').val(data[index].ExtraCharge);
                    $(this).find('td').eq(9).find('input').val(data[index].GrandTotalCost);
                    index++;
                });
                //});
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }

    FillPlotRange();
    function FillPlotRange(selectedPlotRange = null) {
        let dropdown = $('[id*=PlotRange]');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "PlotRange" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                data = data.sort(function (a, b) {
                    var x = a["LookupId"]; var y = b["LookupId"];
                    return ((x < y) ? -1 : ((x > y) ? 1 : 0));
                });
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedPlotRange != null) {
                    dropdown.val(selectedPlotRange);
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

    FillPlotCategory();
    function FillPlotCategory(selectedPlotCategory = null) {
        let dropdown = $('[id*=PlotCategory]');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "PlotCategory" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                data = data.sort(function (a, b) {
                    var x = a["LookupId"]; var y = b["LookupId"];
                    return ((x < y) ? -1 : ((x > y) ? 1 : 0));
                });
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedPlotCategory != null) {
                    dropdown.val(selectedPlotCategory);
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

    $('#btnAddNewRow').click(function () {
        var firstRow = $('#tableBodyPlot tr').eq(0);
        $('#tableBodyPlot').append("<tr>" + firstRow.html() + "</tr>");
    });

    $('#tableBodyPlot').on('change', '#PercentageRate', function () {
        var totalPlotRate = $(this).parent().parent().find('td').eq(3).find('input').val();
        var percentageRate = (parseFloat(totalPlotRate) * parseFloat($(this).val())) / 100;
        $(this).parent().parent().find('td').eq(7).find('input').val(parseFloat(totalPlotRate) + percentageRate);
    });

    $('#tableBodyPlot').on('change', '#ExtraCharge', function () {
        var totalPlotCost = $(this).parent().parent().find('td').eq(7).find('input').val();
        var grandTotal = parseFloat(totalPlotCost) + parseFloat($(this).val());
        $(this).parent().parent().find('td').eq(9).find('input').val(grandTotal);
    });

    $('#tableBodyPlot').on('change', '#plotArea', function () {
        //change logic here
        var plotArea = parseFloat($(this).val());

        if (plotArea > 0 && plotArea <= 4000) {
            $(this).parent().parent().find('td').eq(2).find('select').find('option').map(function () {
                if ($(this).text() == '0-4000') return this;
            }).attr('selected', 'selected');
        }
        else if (plotArea > 4000 && plotArea <= 20000) {
            $(this).parent().parent().find('td').eq(2).find('select').find('option').map(function () {
                if ($(this).text() == '4001-20000') return this;
            }).attr('selected', 'selected');
        }
        else if (plotArea > 20000 && plotArea <= 80000) {
            $(this).parent().parent().find('td').eq(2).find('select').find('option').map(function () {
                if ($(this).text() == '20001-80000') return this;
            }).attr('selected', 'selected');
        }
        else if (plotArea > 80000) {
            $(this).parent().parent().find('td').eq(2).find('select').find('option').map(function () {
                if ($(this).text() == '80001-Above') return this;
            }).attr('selected', 'selected');
        }

        var calculation = 0, index = 0;
        while (plotArea > 0) {
            if (index == 0) {
                if (plotArea >= 4000) {
                    calculation += 4000 * 6000;
                    plotArea -= 4000;
                }
                else {
                    calculation += plotArea * 6000;
                    plotArea -= plotArea;
                }

                index++;
            }
            else if (index == 1) {
                if (plotArea >= 16000) {
                    calculation += 16000 * 5600;
                    plotArea -= 16000;
                }
                else {
                    calculation += plotArea * 5600;
                    plotArea -= plotArea;
                }

                index++;
            }
            else if (index == 2) {
                if (plotArea >= 60000) {
                    calculation += 60000 * 5200;
                    plotArea -= 60000;
                }
                else {
                    calculation += plotArea * 5200;
                    plotArea -= plotArea;
                }
                index++;
            }
            else if (plotArea >= 0 && index == 3) {
                calculation += plotArea * 4800;
                plotArea -= plotArea;
                index++;
            }
        }
        $(this).parent().parent().find('td').eq(3).find('input').val(calculation);
    });

    $('#btnSavePlotMaster').click(function () {
        var dataInput = [];
        $("#tableBodyPlot tr:not('[id*=baseRow]')").each(function () {
            dataInput.push({
                'SchemeType': $('#SchemeType').val(),
                'SchemeName': $('#SchemeName').val(),
                'SectorName': $('#SectorName').val(),
                'PlotNumber': $(this).find('td').eq(0).find('input').val(),
                'PlotArea': $(this).find('td').eq(1).find('input').val(),
                'PlotRange': $(this).find('td').eq(2).find('select').val(),
                'PlotRate': $(this).find('td').eq(3).find('input').val(),
                'PlotSideCorner': $(this).find('td').eq(4).find('input[type="checkbox"]').eq(0).prop('checked'),
                'PlotSideWideRoad': $(this).find('td').eq(4).find('input[type="checkbox"]').eq(1).prop('checked'),
                'PlotSideParkFacing': $(this).find('td').eq(4).find('input[type="checkbox"]').eq(2).prop('checked'),
                'PercentageRate': $(this).find('td').eq(5).find('input').val(),
                'PlotCategory': $(this).find('td').eq(6).find('select').val(),
                'PlotCost': $(this).find('td').eq(7).find('input').val(),
                'ExtraCharge': $(this).find('td').eq(8).find('input').val(),
                'GrandTotalCost': $(this).find('td').eq(9).find('input').val()
            });
        });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: JSON.stringify(dataInput),
            url: '/Admin/SavePlotMaster',
            success: function (data) {
                utility.alert.setAlert(utility.alert.alertType.success, "Plot master is saved");
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });

    $('#btnCloseModal').click(function () {
        $('#myModal').css('display', 'none');
        $('#myModal').removeClass('in');
        $('body').removeClass('modal-open')
        $('.modal-backdrop').remove();
    });

    $('#btnAddNewUser').click(function () {
        $('[name*=userId]').val("");
        $('[name*=username]').val("");
        $('[name*=name]').val("");
        $('[name*=email]').val("");
        $('[name*=mobileNumber]').val("");
        $('[name*=Designation]').val("");
        $('[name*=Department]').val("");
        $('[name*=active]').prop('checked', true);
        $('#lblModalTitle').text('Add New User');

        $('#myModal').css('display', 'block');
        $('#myModal').addClass('in');
        $('body').addClass('modal-open')
        $('body').append('<div class="modal-backdrop fade in"></div>');
    });

    $('#btnAddPage').click(function () {
        $('[name*=PageId]').val("");
        $('[name*=pagename]').val("");
        $('[name*=active]').prop('checked', true);
        $('#lblModalTitle').text('Add New Page');

        $('#myModal').css('display', 'block');
        $('#myModal').addClass('in');
        $('body').addClass('modal-open')
        $('body').append('<div class="modal-backdrop fade in"></div>');
    });

    FillPlotNumber();
    function FillPlotNumber() {
        let dropdown = $('[id*=PlotNumber]');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{}',
            url: '/Admin/GetPlotNumber',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.PlotId).text(entry.PlotNumber));
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

});

function editUser(id) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{userId: ' + id + ' }',
        url: '/Admin/GetUserDetail',
        success: function (data) {
            if (data.UserModel != null) {
                $('[name*=userId]').val(data.UserModel.Id);
                $('[name*=username]').val(data.UserModel.UserName);
                $('[name*=name]').val(data.UserModel.Name);
                $('[name*=email]').val(data.UserModel.Email);
                $('[name*=mobileNumber]').val(data.UserModel.MobileNo);
                $('[name*=Designation]').val(data.UserModel.DesignationId);
                $('[name*=Department]').val(data.UserModel.DepartmentId);
                $('[name*=Role]').val(data.UserModel.UserRoleId);
                $('[name*=active]').prop('checked', data.UserModel.IsActive);
                $('#lblModalTitle').text('Update User');
            }
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
    $('#myModal').css('display', 'block');
    $('#myModal').addClass('in');
    $('body').addClass('modal-open')
    $('body').append('<div class="modal-backdrop fade in"></div>');
}

function editPageMaster(id) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{PageId: ' + id + ' }',
        url: '/Admin/GetPageDetail',
        success: function (data) {
            if (data != null) {
                $('[name*=PageId]').val(data.Id);
                $('[name*=pagename]').val(data.PageName);
                $('[name*=active]').prop('checked', data.IsActive);
                $('#lblModalTitle').text('Update Page');
            }
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
    $('#myModal').css('display', 'block');
    $('#myModal').addClass('in');
    $('body').addClass('modal-open')
    $('body').append('<div class="modal-backdrop fade in"></div>');
}


function sendEmailForAllotment(userid) {
    var status = $('*[data-Status="' + userid + '"]').val();
    var plotId = $('*[data-Status="' + userid + '"]').parent().parent().find('td').eq(3).find('select').val()
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{userId: "' + userid + '",status: "' + status + '",plotId:"' + plotId + '" }',
        url: '/Admin/SendMailtoApplicantForInterviewResult',
        success: function (data) {
            location.reload();

        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}


