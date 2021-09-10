﻿/// <reference path="../Global/Utility.js" />
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

    //function FillSector(SchemeNameId) {
    //    let dropdown = $('#SectorName');
    //    dropdown.empty();
    //    dropdown.append('<option value="">Select</option>');
    //    dropdown.prop('selectedIndex', 0);

    //    $.ajax({
    //        contentType: 'application/json; charset=utf-8',
    //        dataType: 'json',
    //        type: 'POST',
    //        data: '{lookupTypeId: "' + SchemeNameId + '",lookupType: "SectorName" }',
    //        url: '/Masters/GetLookupDetail',
    //        success: function (data) {
    //            $.each(data, function (key, entry) {
    //                dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
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
                        '<td class="text-center">' + index + '</td>' +
                        '<td class="text-center">' + entry.ApplicationNumber + '</td>' +
                        '<td class="text-center">' + entry.FullName + '</td>' +
                        '<td class="text-center">' + entry.ContactNo + '</td>';
                    if ($('#hiddenHasPermission').val() == "true") {
                        rowHtml += '<td class="text-center"><a target="_blank" href="/Admin/ApplicationStatusPA?applicationId=' + entry.ApplicationId + '&schemeName=' + selectedSchemeName + '"><strong>Application Status</strong></a></td>';
                    }
                    else {
                        rowHtml += '<td class="text-center"></td>';
                    }
                    rowHtml += '</tr>';
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
    $('#btnApprovePayment').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#paymentStatusComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Approved",comment: "' + comment + '" }',
                url: '/Admin/ApprovePayment',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'payment is Approved.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to approved the payment.');
        }
    })
    $('#btnRejectedPayment').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#paymentStatusComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Not Approved",comment: "' + comment + '" }',
                url: '/Admin/RejectPayment',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'payment is Not Approved.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to reject the payment.');
        }
    })
    $('#btnApproveDocument').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#documentRejectedComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Approved",comment: "' + comment + '" }',
                url: '/Admin/ApproveDocument',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'document is Not Approved.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to approve the document.');
        }
    })
    $('#btnRejectedDocument').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#documentRejectedComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Not Approved",comment: "' + comment + '" }',
                url: '/Admin/RejectDocument',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'document is Approved.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to reject the document.');
        }
    })

    $('#btnAdminGMApprove').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#GMComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Approved",comment: "' + comment + '" }',
                url: '/Admin/AdminApproveRejectApplication',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'application submitted successfully.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to approve the approve/not approve.');
        }
    })

    $('#btnAdminGMReject').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#GMComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Not Approved",comment: "' + comment + '" }',
                url: '/Admin/AdminApproveRejectApplication',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'application submitted successfully.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to approve the approve/not approve.');
        }
    })

    $('#btnAdminCEOApprove').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#ceoComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Approved",comment: "' + comment + '" }',
                url: '/Admin/AdminApproveRejectApplication',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'application submitted successfully.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to approve the approve/not approve.');
        }
    })

    $('#btnAdminCEOReject').click(function () {
        var applicationId = $('#hdnApplicationId').val(), schemeName = $('#hdnSchemeNameId').val(), comment = $('#ceoComment').val();
        if (comment != null && comment != "" && comment != undefined && comment.trim() != "") {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                data: '{applicationId: "' + applicationId + '",schemeName: "' + schemeName + '",status:"Not Approved",comment: "' + comment + '" }',
                url: '/Admin/AdminApproveRejectApplication',
                success: function (data) {
                    utility.alert.setAlert(utility.alert.alertType.success, 'application submitted successfully.');
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
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'comment is mendatory to approve the approve/not approve.');
        }
    })

    $('#btnSearchApprovedList').click(function () {
        /*var sectorName = $('#SectorName').val();*/
        var schemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + schemeName + '"}',
            url: '/Admin/GetApprovedApplicantDetail',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<td class="text-center">' + index + '</td>' +
                        '<td class="text-center">' + entry.ApplicationNumber + '</td>' +
                        '<td class="text-center">' + entry.FullName + '</td>' +
                        '<td class="text-center">' + entry.ContactNo + '</td>' +
                        '<td class="text-center">' + entry.PlotArea + '</td>';
                    if (entry.InterviewLetterStatus == 'InvitationSentForInterview')
                        rowHtml += '<td class="text-center" style="color:#ea9a06">Invitation Sent</td>';
                    else if (entry.InterviewLetterStatus == 'Selected')
                        rowHtml += '<td class="text-center" style="color:#08bf0b">Selected in Interview</td>';
                    else
                        rowHtml += '<td class="text-center">' + entry.InterviewLetterStatus + '/td>';
                    if (entry.InterviewLetterStatus != null && entry.InterviewLetterStatus != "" && entry.InterviewLetterStatus != undefined && entry.InterviewLetterStatus == 'Selected') {
                        rowHtml += '<td class="text-center" id="actionColumn"></td>';
                    }
                    else if (entry.InterviewLetterStatus != null && entry.InterviewLetterStatus != "" && entry.InterviewLetterStatus != undefined && entry.InterviewLetterStatus != 'Selected') {
                        rowHtml += '<td class="text-center" id="actionColumn"><a target="_blank" href="/Admin/Invitation?applicationId=' + entry.ApplicationId + '&schemeName=' + schemeName + '"><strong style="color:#08bf0b">Update Invitation</strong></a></td>';
                    }
                    else {
                        rowHtml += '<td class="text-center" id="actionColumn"><a target="_blank" href="/Admin/Invitation?applicationId=' + entry.ApplicationId + '&schemeName=' + schemeName + '"><strong  style="color:#ea9a06">Send Invitation</strong></a></td>';
                    }
                    rowHtml += '</tr>';
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

    $('#btnSearchAlloteeListLeaseDeed').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Admin/GetAllotteeListForLeasedeed',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<td class="text-center">' + index + '</td>' +
                        '<td class="text-center">' + entry.FullName + '</td>' +
                        '<td class="text-center">' + entry.ApplicationNumber + '</td>' +
                        '<td class="text-center">' + entry.AllotmentNumber + '</td>' +
                        '<td class="text-center"><a target="_blank" href="/Admin/LeasedeedStatus?applicationId=' + entry.ApplicationId + '"><strong>Leasedeed Status</strong></a></td>';
                    rowHtml += '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="5">No Record Found.</td>' +
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

    $('#btnSchemeWiseAllotmentList').click(function () {
        var selectedSchemeName = $('#SchemeName').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{schemeName: "' + selectedSchemeName + '" }',
            url: '/Admin/GetSchemeWiseAllotmentList',
            success: function (data) {
                var rowHtml = "";
                var index = 1;
                $('#tbodyData tr').remove();
                $.each(data, function (key, entry) {
                    rowHtml += '<tr>' +
                        '<td class="text-center">' + index + '</td>' +
                        '<td class="text-center">' + entry.FullName + '</td>' +
                        '<td class="text-center">' + entry.ApplicationNumber + '</td>' +
                        '<td class="text-center">' + entry.PlotNumber + '</td>' +
                        '<td class="text-center"><a target="_blank" href="/Admin/AllotmentStatus?applicationId=' + entry.ApplicationId + '"><strong>Allotment Status</strong></a></td>';
                    rowHtml += '</tr>';
                    index++;
                });
                if (data.length == 0) {
                    rowHtml += '<tr>' +
                        '<td colspan="5">No Record Found.</td>' +
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



