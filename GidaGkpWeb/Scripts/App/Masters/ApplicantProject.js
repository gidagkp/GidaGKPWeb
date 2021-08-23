/// <reference path="../Global/Utility.js" />
'use strict';


$(document).ready(function () {
    var current_fs, next_fs, previous_fs; //fieldsets
    var opacity;    

    function getProjectChangeRequestDetail() {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'GET',
            url: '/Applicant/GetProjectChangeRequest',
            success: function (data) {
                if (data != null && data != undefined) {
                    $('#applicantName').val(data.ApplicantName);
                    $('#applicantNumber').val(data.ApplicationId);
                    $('#allotmentNumber').val(data.AllotmentNumber);
                    $('#areaToBeUsedForProject').val(data.AreaToBeUsedForProject);
                    $('#detailOfProject').val(data.DetailOfProjectFileName);
                    $('#isEffluentMore').val(data.IsEffluentMore);
                    $('#nocOfProject').val(data.NOCOfProjectFileName);
                    $('#ssiRegistration').val(data.SSIRegistrationNo);
                    $('#isEmissionToSurrounding').val(data.IsEmissionToSurrounding);
                    $('#notarizedPoint').val(data.NotarizedFileName);
                    $('#signature').val(data.SignatureFileName);
                    $('#uploadPhotograph').val(data.PhotographFileName);
                    $('#hdnChangeProjectId').val(data.Id);
                    
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

    getProjectChangeRequestDetail();

    $('#btnProjectChangeRequestSave').on('click', function (e) {
        var hdnChangeProjectId = $('#hdnChangeProjectId').val();
        var applicantName = $('#applicantName').val();
        var applicantNumber = $('#applicantNumber').val();
        var allotmentNumber = $('#allotmentNumber').val();
        var areaToBeUsedForProject = $('#areaToBeUsedForProject').val();
        var detailOfProjectFileName = $('[name*=detailOfProjectFileName]').val();
        var isEffluentMore = $('#isEffluentMore').val();
        var nocOfProjectFileName = $('[name*=nocOfProjectFileName]').val();
        var ssiRegistration = $('#ssiRegistration').val();
        var isEmissionToSurrounding = $('#isEmissionToSurrounding').val();
        var notarizedFileName = $('[name*=notarizedFileName]').val();
        var signatureFileName = $('[name*=signatureFileName]').val();
        var photographFileName = $('[name*=photographFileName]').val();
        if (applicantName != '' && applicantNumber != '' && allotmentNumber != '' &&
            areaToBeUsedForProject != '' && detailOfProjectFileName != '' &&
            isEffluentMore != '' && nocOfProjectFileName != '' && ssiRegistration != '' &&
            isEmissionToSurrounding != '' && notarizedFileName != '' && signatureFileName != '' && photographFileName != '') {
            $('#formSaveProjectChangeRequest').submit();            
        } else if (hdnChangeProjectId != '') {
            $('#formSaveProjectChangeRequest').submit();
        }
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please select required inputs.');
            return false;
        }

    });


    $('#Step5NextButton').click(function () {
        //var ProjectReports = document.getElementById('ProjectReports');
        //var Proposedplan = document.getElementById('Proposedplan');
        //var PartnershipDeed = document.getElementById('PartnershipDeed');
        //var PanCard = document.getElementById('PanCard');
        //var AddressProof = document.getElementById('AddressProof');
        //var IncomeTaxreturn = document.getElementById('IncomeTaxreturn');
        //var Experienceproof = document.getElementById('Experienceproof');
        //var educationalqualification = document.getElementById('educationalqualification');
        //var electricitybill = document.getElementById('electricitybill');
        //var financialdetails = document.getElementById('financialdetails');
        //var IdentityProof = document.getElementById('IdentityProof');
        //var BankVerifiedSignature = document.getElementById('BankVerifiedSignature');
        //var ApplicantPhoto = document.getElementById('ApplicantPhoto');
        //var ApplicantSignature = document.getElementById('ApplicantSignature');
        //ProjectReports.files.length === 0 || Proposedplan.files.length === 0 || PartnershipDeed.files.length === 0 ||
        //PanCard.files.length === 0 || AddressProof.files.length === 0 || IncomeTaxreturn.files.length === 0 ||
        //Experienceproof.files.length === 0 || ApplicantSignature.files.length === 0 || educationalqualification.files.length === 0 ||
        //electricitybill.files.length === 0 || financialdetails.files.length === 0 || IdentityProof.files.length === 0 ||
        //BankVerifiedSignature.files.length === 0 || ApplicantPhoto.files.length === 0

        var ProjectReportsfilename = $('[name*=ProjectReportsfilename]').val();
        var Proposedplanfilename = $('[name*=Proposedplanfilename]').val();
        var PartnershipDeedfilename = $('[name*=PartnershipDeedfilename]').val();
        var PanCardfilename = $('[name*=PanCardfilename]').val();
        var AddressProoffilename = $('[name*=AddressProoffilename]').val();
        var IncomeTaxreturnfilename = $('[name*=IncomeTaxreturnfilename]').val();
        var Experienceprooffilename = $('[name*=Experienceprooffilename]').val();
        var educationalqualificationfilename = $('[name*=educationalqualificationfilename]').val();
        var electricitybillfilename = $('[name*=electricitybillfilename]').val();
        var financialdetailsfilename = $('[name*=financialdetailsfilename]').val();
        var IdentityProoffilename = $('[name*=IdentityProoffilename]').val();
        var BankVerifiedSignaturefilename = $('[name*=BankVerifiedSignaturefilename]').val();
        var ApplicantPhotofilename = $('[name*=ApplicantPhotofilename]').val();
        var ApplicantSignaturefilename = $('[name*=ApplicantSignaturefilename]').val();
        if (ProjectReportsfilename == "" || Proposedplanfilename == "" || PanCardfilename == "" ||
            AddressProoffilename == "" || IncomeTaxreturnfilename == "" || educationalqualificationfilename == "" ||
            IdentityProoffilename == "" || BankVerifiedSignaturefilename == "" ||
            ApplicantPhotofilename == "" || ApplicantSignaturefilename == "") {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please select required file.');
            return false;
        }
        else {
            $('#formSaveDocument').submit();
        }

    });

    $('#Step5PreviousButton').on('click', function (e) {
        //$('#progressbar li').removeClass('active');
        //$('#BankDetail').addClass('active');
        PreviousStep($('#Step5PreviousButton'));
    });

    $("#PlotRange").change(function () {
        //reset dependent input
        if ($("#PlotRange option:selected").text() == "0-4000") {
            $('#labelPlotArea').html('Plot Area Not less than 500.00 (Square Meter) <span class="required-field"></span>');
        }
        else {
            $('#labelPlotArea').html('Plot Area (Square Meter) <span class="required-field"></span>');
        }
        $('#plotArea').val('');
        $('#TotalInvestment').val('');
        $('#EarnestMoneyDeposite').val('');
        $('#NetAmount').val('');
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: "' + $(this).val() + '",lookupType: "EstimatedRate" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $('#EstimatedRate').val(data[0].LookupName);
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });


    $("#CoveredArea").change(function () {
        if (parseFloat($(this).val()) > parseFloat($("#plotArea").val())) {
            $(this).val('');
            $('#OpenArea').val('');
            utility.alert.setAlert(utility.alert.alertType.info, 'Covered Area must less than or equal to plot area');
        }
        if ($("#plotArea").val() != '') {
            $('#OpenArea').val(parseFloat($("#plotArea").val()) - parseFloat($(this).val()));
        }
    });

    $("#plotArea").change(function () {
        var plotRangeSelected = $("#PlotRange option:selected").text();
        if (plotRangeSelected == "0-4000") {
            if ($(this).val() < 500.00) {
                $(this).val('');
                utility.alert.setAlert(utility.alert.alertType.info, 'Plot Area must greater than 500.00');
                return false;
            }
        }
        if (plotRangeSelected.indexOf('Select') > -1) {
            var rangeArray = plotRangeSelected.split('-');
            if (parseInt($(this).val()) < parseInt(rangeArray[0])) {
                $(this).val('')
                utility.alert.setAlert(utility.alert.alertType.info, 'Plot Area must in selected plot range');
                return false;
            }
        }
        else {
            var rangeArray = plotRangeSelected.split('-');
            if (parseInt($(this).val()) < parseInt(rangeArray[0]) || parseInt($(this).val()) > parseInt(rangeArray[1])) {
                $(this).val('')
                utility.alert.setAlert(utility.alert.alertType.info, 'Plot Area must in selected plot range');
                return false;
            }
        }

        //change logic here
        var plotArea = parseFloat($(this).val());
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

        $('#TotalInvestment').val(calculation);

        //var previousRange = $("#PlotRange option:selected").prev();
        //if (previousRange.text() != 'Select') {
        //    $.ajax({
        //        contentType: 'application/json; charset=utf-8',
        //        dataType: 'json',
        //        type: 'POST',
        //        data: '{lookupTypeId: "' + previousRange.val() + '",lookupType: "EstimatedRate" }',
        //        url: '/Masters/GetLookupDetail',
        //        success: function (data) {
        //            var lastEstimatedCast = data[0].LookupName;
        //            var rangeArray = previousRange.text().split('-');
        //            var firstCalculation = parseInt(rangeArray[1]) * parseInt(lastEstimatedCast);
        //            var secondCalculation = parseInt(plotArea - parseInt(rangeArray[1])) * parseInt($('#EstimatedRate').val());
        //            $('#TotalInvestment').val(firstCalculation + secondCalculation);
        //        },
        //        failure: function (response) {
        //            console.log(response);
        //        },
        //        error: function (response) {
        //            console.log(response.responseText);
        //        }
        //    });
        //}
        //else {
        //    if ($(this).val() != '' && $('#EstimatedRate').val() != '') {
        //        $('#TotalInvestment').val($(this).val() * $('#EstimatedRate').val());
        //    }
        //}

        var auxValue = (parseInt($(this).val()) + 1000).toString().slice($(this).val().length - 3, $(this).val().length);
        var EMD = "";
        if (auxValue == "000") {
            EMD = (parseInt($(this).val())).toString().slice(0, -3) + "0000";
        }
        else {
            EMD = (parseInt($(this).val()) + 1000).toString().slice(0, -3) + "0000";
        }
        $('#EarnestMoneyDeposite').val(EMD);
        if ($('#EarnestMoneyDeposite').val() != '') {
            $('#NetAmount').val(parseInt($('#EarnestMoneyDeposite').val()) + parseInt($('#TotalAmount').val()));
        }
    });


    $('#btnStep1Skip').click(function () {
        NextStep($('#btnStep1Skip'));
        getApplicantPersonalDetail();
    });

    $('#btnStep2Skip').click(function () {
        NextStep($('#btnStep2Skip'));
        getApplicantProjectDetail();
    });

    $('#btnStep3Skip').click(function () {
        NextStep($('#btnStep3Skip'));
        getApplicantBankDetail();
    });

    $('#btnStep4Skip').click(function () {
        NextStep($('#btnStep4Skip'));
        getApplicantDocumentDetail();
        //$('#btnStep5Skip').removeClass('hidden');
    });

    $('#btnStep5Skip').click(function () {
        window.location.href = "/Applicant/PaymentRequest";
    });

    function getApplicantDocumentDetail() {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '',
            url: '/Applicant/GetApplicantDocumentDetail',
            success: function (data) {
                if (data != null && data != undefined) {
                    $('[name*=ProjectReportsfilename]').val(data.ProjectReportFileName);
                    $('[name*=Proposedplanfilename]').val(data.ProposedPlanLandUsesFileName);
                    $('[name*=PartnershipDeedfilename]').val(data.MemorendumFileName);
                    $('[name*=PanCardfilename]').val(data.ScanPANFileName);
                    $('[name*=AddressProoffilename]').val(data.ScanAddressProofFileName);
                    $('[name*=IncomeTaxreturnfilename]').val(data.ITReturnFileName);
                    $('[name*=Experienceprooffilename]').val(data.ExperienceProofFileName);
                    $('[name*=educationalqualificationfilename]').val(data.ApplicantEduTechQualificationFileName);
                    $('[name*=electricitybillfilename]').val(data.PreEstablishedIndustriesDocFileName);
                    $('[name*=financialdetailsfilename]').val(data.FinDetailsEstablishedIndustriesFileName);
                    $('[name*=IdentityProoffilename]').val(data.ScanIDFileName);
                    $('[name*=BankVerifiedSignaturefilename]').val(data.BankVerifiedSignatureFileName);
                    $('[name*=ApplicantPhotofilename]').val(data.ApplicantPhotoFileName);
                    $('[name*=ApplicantSignaturefilename]').val(data.ApplicantSignatureFileName);

                    $('[name*=BalanceSheetfilename]').val(data.BalanceSheetFileName);
                    $('[name*=Otherproposedindustryfilename]').val(data.OtherDocForProposedIndustryFileName);
                    $('[name*=CasteCertificatefilename]').val(data.ScanCastCertFileName);
                    $('[name*=outsideGIDAElectricitybillfilename]').val(data.DocProofForIndustrialEstablishedOutsideGidaFileName);
                    if (data.ApplicantPhotoFileName != '' && data.ApplicantPhotoFileName != null) {
                        $('#btnStep5Skip').removeClass('hidden');
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

    function getApplicantBankDetail() {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '',
            url: '/Applicant/GetApplicantBankDetail',
            success: function (data) {
                if (data != null && data != undefined) {
                    $('#BankAccountName').val(data.AccountHolderName);
                    $('#BankAccountNo').val(data.BankAccountNo);
                    $('#BankName').val(data.BankName);
                    $('#BranchName').val(data.BBName);
                    $('#BranchAddress').val(data.BBAddress);
                    $('#IFSCCode').val(data.IFSC_Code);
                    $('#btnStep4Skip').removeClass('hidden');
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

    function getApplicantProjectDetail() {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '',
            url: '/Applicant/GetApplicantProjectDetail',
            success: function (data) {
                if (data != null && data != undefined) {
                    $('#ProposedIndustryType').val(data.ProposedIndustryType);
                    $('#ProjectEstimatedCost').val(data.ProjectEstimatedCost);
                    $('#CoveredArea').val(data.ProposedCoveredArea);
                    $('#OpenArea').val(data.ProposedOpenArea);
                    $('#Purposeforopenarea').val(data.PurpuseOpenArea);
                    $('#Investmentland').val(data.ProposedInvestmentLand);
                    $('#InvestmentBuilding').val(data.ProposedInvestmentBuilding);
                    $('#InvestmentPlant').val(data.ProposedInvestmentPlant);
                    $('#processofmanufacture').val(data.FumesNatureQuantity);
                    $('#LiquidQuantity').val(data.LiquidQuantity);
                    $('#LiquidChemicalComposition').val(data.LiquidChemicalComposition);
                    $('#SolidQuantity').val(data.SolidQuantity);
                    $('#SolidChemicalComposition').val(data.SolidChemicalComposition);
                    $('#GasQuantity').val(data.GasQuantity);
                    $('#GasChemicalComposition').val(data.GasChemicalComposition);
                    $('#PowerRequirement').val(data.PowerRequirement);
                    $('#FirstYearTelephonicConnection').val(data.FirstYearNoOfTelephone);
                    $('#FirstYearFaxConnection').val(data.FirstYearNoOfFax);
                    $('#UltimateRequirementTelephonicConnection').val(data.UltimateNoOfTelephone);
                    $('#UltimateRequirementFaxConnection').val(data.UltimateNoOfFax);
                    $('#Skilled').val(data.Skilled);
                    $('#Unskilled').val(data.UnSkilled);
                    $('#btnStep3Skip').removeClass('hidden');
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

    $('#processofmanufacture').change(function () {
        if ($(this).val().toLowerCase() == 'n.a.' || $(this).val().toLowerCase() == 'na' || $(this).val().toLowerCase() == 'no') {
            $('#tableIndustrialEfflunets').addClass('disabledAnchor');
        }
        else if ($(this).val().toLowerCase() == 'yes') {
            $('#tableIndustrialEfflunets').removeClass('disabledAnchor');
        }
        else {
            $('#tableIndustrialEfflunets').addClass('disabledAnchor');
        }
    });

    //binds to onchange event of your input field
    $('[type="file"]').bind('change', function () {
        if (this.files[0].size > 6291456) {  //allowed 6 MB Currently
            utility.alert.setAlert(utility.alert.alertType.error, 'Please select file less than 6 MB');
            $(this).val('');
            $('[name*=' + $(this).attr('id') + 'filename]').val('');
        }
    });

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

    function NextStep(nextButton) {

        current_fs = $(nextButton).parent().parent().parent();
        next_fs = $(nextButton).parent().parent().parent().next();

        //Add Class Active
        $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

        //show the next fieldset
        next_fs.show();
        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making fielset appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                next_fs.css({ 'opacity': opacity });
            },
            duration: 600
        });
        window.scrollTo(0, 200);
    }

    function PreviousStep(previousButton) {

        current_fs = $(previousButton).parent().parent().parent();
        previous_fs = $(previousButton).parent().parent().parent().prev();

        //Remove class active
        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

        //show the previous fieldset
        previous_fs.show();

        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making fielset appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                previous_fs.css({ 'opacity': opacity });
            },
            duration: 600
        });
        window.scrollTo(0, 0);
    }

});

