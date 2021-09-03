'use strict';


$(document).ready(function () {
    FillSelectedInvitation();
    function FillSelectedInvitation(ApplicationId) {
        var ApplicationId = 55;
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{ApplicationId: "' + ApplicationId + '"}',
            url: '/Admin/GetSelectedLeaseddeedAppDetail',
            success: function (data) {
                $('#ApplicantName').val(data.ApplicantName);
                $('#SectorName').val(data.SectorName);
                $('#Plotnumber').val(data.Plotnumber);
                $('#PlotArea').val(data.PlotArea);
                $('#Applicantadress').val(data.ApplicantAddress);
                $('#EarnestMoneyPaid').val(data.EarnestMoneyPaid);
                $('#EarnestMoneyChallanNumber').val(data.EarnestMoneyChallanNumber);
                $('#AllotmentMoneyPaid').val(data.AllotmentMoneyPaid);
                $('#VerifyTotalPremium').val(data.VerifyTotalPremium);
                $('#StampValue').val(data.StampValue);
                $('#BankgChallanNumber').val(data.BankgChallanNumber);
                $('#EntityName').val(data.EntityName);
                $('#SvalueBankGchallanNumber').val(data.SvalueBankGchallanNumber);


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