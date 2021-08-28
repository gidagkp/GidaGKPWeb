/// <reference path="../Global/Utility.js" />
'use strict';


$(document).ready(function () {

    //FillGidaUser();
    //function FillGidaUser() {
    //    let dropdown = $('#ddlGidaUser');
    //    dropdown.empty();
    //    dropdown.append('<option value="">Select</option>');
    //    dropdown.prop('selectedIndex', 0);
    //    $.ajax({
    //        contentType: 'application/json; charset=utf-8',
    //        dataType: 'json',
    //        type: 'POST',
    //        url: '/Admin/GetGidaUserList',
    //        success: function (data) {
    //            $.each(data, function (key, entry) {
    //                dropdown.append($('<option></option>').attr('value', entry.Id).text(entry.Name + '(' + entry.UserName + ')'));
    //            });
    //        },
    //        failure: function (response) {
    //            console.log(response);
    //        },
    //        error: function (response) {
    //            console.log(response.responseText);
    //        }
    //    });
    //}

    //$('#ddlGidaUser').on('change', function (e) {
    //    var valueSelected = this.value;
    //    FillSelectedUser(valueSelected);
    //});

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

    $('#ddlDepartment').on('change', function (e) {
        var valueSelected = this.value;
        var designationId = $('#ddlDesignation').val();
        var roleId = $('#ddlRole').val();
        if (valueSelected != "" && departmentId != "" && roleId != "") {
            FillRoleWisePermission(valueSelected, designationId, roleId);
        }
    });

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

    $('#ddlDesignation').on('change', function (e) {
        var valueSelected = this.value;
        var departmentId = $('#ddlDepartment').val();
        var roleId = $('#ddlRole').val();
        if (valueSelected != "" && departmentId != "" && roleId != "") {
            FillRoleWisePermission(departmentId, valueSelected, roleId);
        }
    });

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

    $('#ddlRole').on('change', function (e) {
        var valueSelected = this.value;
        var departmentId = $('#ddlDepartment').val();
        var designationId = $('#ddlDesignation').val();
        if (valueSelected != "" && departmentId != "" && designationId != "") {
            FillRoleWisePermission(departmentId, designationId, valueSelected);
        }
    });

    function FillRoleWisePermission(departmentId, DesignationId, roleId) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{departmentId: "' + departmentId + '",designationId: "' + DesignationId + '",roleId: "' + roleId + '"}',
            url: '/Admin/GetRoleWisePermission',
            success: function (data) {
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: 'POST',
                    data: '{lookupTypeId: 0,lookupType: "PermissionType" }',
                    url: '/Admin/GetLookupDetail',
                    success: function (dataPermissionType) {
                        var rowHtml = "";
                        $('#roleUserTBody tr').remove();
                        if (data != null && data != undefined) {
                            if (data.PermissionModel != null && data.PermissionModel != undefined && data.PermissionModel.length > 0) {
                                var index = 1;
                                rowHtml = "";
                                $('#rolePermissionTBody tr').remove();
                                $.each(data.PermissionModel, function (key, entry) {
                                    rowHtml += '<tr>' +
                                        '<td class="text-center">' + index + '</td>' +
                                        '<td class="text-center">' + entry.PageId + '</td>' +
                                        '<td class="text-center">' + entry.PageName + '</td>';
                                    $.each(dataPermissionType, function (key2, entry2) {
                                        if (entry.PermissionTypeIdList != null && entry.PermissionTypeIdList != undefined && entry.PermissionTypeIdList.indexOf(entry2.LookupId) > -1)
                                            rowHtml += '<td class="text-center"><input type="checkbox" checked></td>';
                                        else
                                            rowHtml += '<td class="text-center"><input type="checkbox"></td>';
                                    });
                                    rowHtml += '</tr>';
                                    index++;
                                });
                                $('#rolePermissionTBody').append(rowHtml);
                            }
                        }
                    }
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

    $('#btnSaveUserPermission').click(function () {
        var departmentId = $('#ddlDepartment').val();
        var designationId = $('#ddlDesignation').val();
        var roleId = $('#ddlRole').val();
        var dataInput = [];
        $("#rolePermissionTBody tr").each(function () {
            for (var i = 0; i < $(this).find('td').length; i++) {
                if ($(this).find('td').eq(3 + i).find('input[type="checkbox"]').prop('checked') == true) {
                    dataInput.push({ 'DepartmentId': departmentId, 'DesignationId': designationId, 'RoleId': roleId, 'PageId': $(this).find('td').eq(1).html(), 'PermissionId': $("#rolePermissionTHead tr").find('th').eq(3 + i).attr('data-permission-id') });
                }
            }
        });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: JSON.stringify(dataInput),
            url: '/Admin/SaveUserPermission',
            success: function (data) {
                utility.alert.setAlert(utility.alert.alertType.success, "Permission for user is saved");
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


