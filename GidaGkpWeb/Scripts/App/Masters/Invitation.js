'use strict';


$(document).ready(function () {

    FillApplicantdata();
    function FillApplicantdata() {
        let dropdown = $('#ddlApplicant');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            url: '/Admin/GetInvitationList',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.Id).text(entry.FullName));
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

    $('#ddlApplicant').on('change', function (e) {
        var valueSelected = this.value;
        FillSelectedInvitation(valueSelected);
    });

    function FillSelectedInvitation(UserId) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{UserId: "' + UserId + '"}',
            url: '/Admin/GetSelectedInvitationDetail',
            success: function (data) {
                $('#Address').val(data.Address);
                $('#SectorName').val(data.SectorName);
                $('#PlotRange').val(data.PlotRange);
                $('#ApplicationNo').val(data.ApplicationNumber);

            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }

})