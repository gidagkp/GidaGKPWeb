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

    $('#btnSearchFormSubmitted').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Admin/GetFormSubmittedDetail',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    /*dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));*/
                    rowHtml += '<tr>' +
                        '<th scope="row">' + index + '</th>' +
                        '<td>' + entry.UserName + '</td>' +
                        '<td>' + entry.FullName + '</td>' +
                        '<td>' + entry.ApplicationNumber + '</td>' +
                        '<td>' + entry.PlotArea + '</td>' +
                        '<td>' + entry.FatherName + '</td>' +
                        '<td>' + entry.ContactNo + '</td>' +
                        '<td>' + entry.Email + '</td>' +
                        '<td>' + entry.AadharNumber + '</td>' +
                        '<td>' + entry.UserType + '</td>' +
                        '<td>' + entry.DOB + '</td>' +
                        '<td>' + entry.CAddress + '</td>' +
                        '<td>' + entry.PAddress + '</td>' +
                        '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="13">No Record Found.</td>' +
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

    $('#btnSearchTransactionCompleted').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Admin/GetTransactionCompletedDetail',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<th scope="row">' + index + '</th>' +
                        '<td>' + entry.UserName + '</td>' +
                        '<td>' + entry.FullName + '</td>' +
                        '<td>' + entry.ApplicationNumber + '</td>' +
                        '<td>' + entry.PlotArea + '</td>' +
                        '<td>' + entry.PaidAmount + '</td>' +
                        '<td>' + entry.FatherName + '</td>' +
                        '<td>' + entry.ContactNo + '</td>' +
                        '<td>' + entry.Email + '</td>' +
                        '<td>' + entry.AadharNumber + '</td>' +
                        '<td>' + entry.UserType + '</td>' +
                        '<td>' + entry.DOB + '</td>' +
                        '<td>' + entry.UnitName + '</td>' +
                        '<td>' + entry.TotalInvestment + '</td>' +
                        '<td>' + entry.Skilled + '</td>' +
                        '<td>' + entry.CAddress + '</td>' +
                        '<td>' + entry.PAddress + '</td>' +
                        '<td><a target="_blank" href="/Admin/DownloadAttachment?applicationId=' + entry.ApplicationId + '"><strong>Download</strong></a>' +
                        '</td>' +
                        '<td><a class="previous action-button-previous pull-right" target="_blank" href="/Applicant/PaymentReciept?applicationId=' + entry.ApplicationId + '">Print Reciept</a></td>' +
                        '<td><a class="previous action-button-previous pull-right" target="_blank" href="/Applicant/PaymentAcknowledgement?applicationId=' + entry.ApplicationId + '">Print Acknowlegement</a></td>' +
                        '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="20">No Record Found.</td>' +
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

    $('#btnSearchFinancialDetail').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Admin/GetApplicantFinancialDetails',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<th scope="row">' + index + '</th>' +
                        '<td>' + entry.FullName + '</td>' +
                        '<td>' + entry.ApplicationNumber + '</td>' +
                        '<td>' + entry.PlotArea + '</td>' +
                        '<td>' + entry.PaidAmount + '</td>' +
                        '<td>' + entry.FatherName + '</td>' +
                        '<td>' + entry.ContactNo + '</td>' +
                        '<td>' + entry.AccoutnNumber + '</td>' +
                        '<td>' + entry.BankName + '</td>' +
                        '<td>' + entry.BranchName + '</td>' +
                        '<td>' + entry.IFSCCode + '</td>' +
                        '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="11">No Record Found.</td>' +
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

    FillDepartment();
    function FillDepartment(selectedDepartmentId = null) {
        let dropdown = $('#ddlDepartment');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "Department" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedDepartmentId != null) {
                    dropdown.val(selectedDepartmentId);
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

    FillDesignation();
    function FillDesignation(selectedDesignationId = null) {
        let dropdown = $('#ddlDesignation');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "Designation" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedDesignationId != null) {
                    dropdown.val(selectedDesignationId);
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

    FillRole();
    function FillRole() {
        let dropdown = $('#ddlRole');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "UserRole" }',
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



