/// <reference path="../Global/Utility.js" />
'use strict';


$(document).ready(function () {
    setTimeout(function () {
        FillAllotedDetail();
    }, 1000);

    function FillAllotedDetail() {
        var applicationId = $('#applicationId').val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{applicationId: ' + applicationId + '}',
            url: '/Admin/GetApplicantUserDetailByApplicationId',
            success: function (data) {
                if (data != null && data != undefined) {
                    $('[name*=AllotmentNumber]').val(data.AllotmentNumber);
                    if (data.AllotmentDate != null) {
                        $('[name*=Allotmentdate]').val(formatDate(data.AllotmentDate));
                    }
                    if (data.StartingDateofInterview != null) {
                        $('[name*=InterviewStartDate]').val(formatDate(data.StartingDateofInterview));
                    }
                    if (data.EndDateofInterview != null) {
                        $('[name*=InterviewEndDate]').val(formatDate(data.EndDateofInterview));
                    }
                    if (data.DateofAllotmentLetter != null) {
                        $('[name*=AllotmentLetterDate]').val(formatDate(data.DateofAllotmentLetter));
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

    function formatDate(currentDate) {
        var milli = currentDate.replace(/\/Date\((-?\d+)\)\//, '$1');
        var now = new Date(parseInt(milli));
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);
        return today;
    }
})


