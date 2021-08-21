$(document).ready(function () {

    var NoticeId = getUrlParameter('NoticeId');
    if (NoticeId != null && NoticeId != undefined && NoticeId > 0) {
        GetNoticeDetail(NoticeId);
    }
    else {
        FillNoticeType();
        FillDepartmentNotice();
    }

    function GetNoticeDetail(NoticeId) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{NoticeId: ' + NoticeId + '}',
            url: '/Admin/GetNoticeDetail',
            success: function (data) {
                if (data != null && data != undefined) {
                    FillNoticeType(data.NoticeTypeId);
                    FillDepartmentNotice(data.Department);

                    $('#Title').val(data.Notice_title);
                    $('#EstimatedRate').val(data.EstimatedRate);
                    $('#TotalInvestment').val(data.TotalInvestment);
                    $('#ApplicationFee').val(data.ApplicationFee);
                    $('#NoticeId').val(data.Id);
                    if (data.Notice_Date != null && data.Notice_Date != undefined) {
                        var milli = data.Notice_Date.replace(/\/Date\((-?\d+)\)\//, '$1');
                        var now = new Date(parseInt(milli));
                        var day = ("0" + now.getDate()).slice(-2);
                        var month = ("0" + (now.getMonth() + 1)).slice(-2);
                        var today = now.getFullYear() + "-" + (month) + "-" + (day);
                        $('#NoticeDate').val(today);
                    }

                    $('#Documentfilename').val(data.NoticeDocumentName);
                    if (data.NoticeNewTag == true) {
                        $('#NewTag').prop('checked', 'checked');
                    }
                    else {
                        $('#NewTag').prop('checked', '');
                    }
                    if (data.IsActive == true) {
                        $('#Publish').prop('checked', 'checked');
                    }
                    else {
                        $('#Publish').prop('checked', '');
                    }

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

    function FillNoticeType(selectedNoticeTypeID = null) {
        let dropdown = $('#NoticeType');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "NoticeType" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedNoticeTypeID != null) {
                    dropdown.val(selectedNoticeTypeID);
                    $('#NoticeType').change();
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

    $('#NoticeType').on('change', function (e) {
        var selectedText = $("#NoticeType option:selected").text();
        if (selectedText == 'Highlight Notice') {
            $('#divDepartment').hide();
            $('#divNoticeDate').hide();
        }
        else if (selectedText == 'Latest Notice') {
            $('#divDepartment').hide();
            $('#divNoticeDate').hide();
            $('#divNewTag').hide();
        }
        else {
            $('#divDepartment').show();
            $('#divNoticeDate').show();
            $('#divNewTag').show();
        }
    });

    function FillDepartmentNotice(selectedDepartmentNoticeID = null) {
        let dropdown = $('#DepartmentNotice');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "DepartmentNotice" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedDepartmentNoticeID != null) {
                    dropdown.val(selectedDepartmentNoticeID);
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

    function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    }
})