/// <reference path="../Global/Utility.js" />
'use strict';


$(document).ready(function () {
    var current_fs, next_fs, previous_fs; //fieldsets
    var opacity;

    var applicationId = getUrlParameter('applicationId');
    if (applicationId != null && applicationId != undefined && applicationId > 0) {
        getPlotRegistrationDetail(applicationId);
    }
    else {
        FillSchemeType();
        FillAppliedFor();
        FillPlotRange();
        FillRelationshipStatus();
        FillIndustryOwnershipType();
        FillPaymemtSchedule();
    }

    function getPlotRegistrationDetail(applicationId) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{applicationId: ' + applicationId + '}',
            url: '/Applicant/GetPlotRegistrationDetail',
            success: function (data) {
                if (data != null && data != undefined) {
                    FillAppliedFor(data.AppliedFor);
                    FillSchemeType(data.SchemeType)
                    FillSchemeName(data.SchemeType, data.SchemeName);
                    FillSector(data.SchemeName, data.SectorName);

                    FillPlotRange(data.PlotRange);
                    FillRelationshipStatus(data.RelationshipStatus);
                    FillIndustryOwnershipType(data.IndustryOwnership);
                    FillPaymemtSchedule(data.PaymentSchedule);

                    $('#plotArea').val(data.PlotArea);
                    $('#EstimatedRate').val(data.EstimatedRate);
                    $('#TotalInvestment').val(data.TotalInvestment);
                    $('#ApplicationFee').val(data.ApplicationFee);
                    $('#EarnestMoneyDeposite').val(data.EarnestMoney);
                    $('#GST').val(data.GST);
                    $('#NetAmount').val(data.NetAmount);
                    $('#TotalAmount').val(data.TotalAmount);
                    $('#UnitName').val(data.UnitName);
                    $('#Name').val(data.SignatryName);
                    $('#PresentAddress').val(data.SignatryPresentAddress);
                    var milli = data.SignatryDateOfBirth.replace(/\/Date\((-?\d+)\)\//, '$1');
                    var now = new Date(parseInt(milli));

                    var day = ("0" + now.getDate()).slice(-2);
                    var month = ("0" + (now.getMonth() + 1)).slice(-2);

                    var today = now.getFullYear() + "-" + (month) + "-" + (day);

                    $('#dob').val(today);
                    $('#PermanentAddress').val(data.SignatryPermanentAddress);
                    $('#btnStep1Skip').removeClass('hidden');
                    $('#spanApplicationNumber').html(data.ApplicationNumber);
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

    function getApplicantPersonalDetail() {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '',
            url: '/Applicant/GetApplicantPersonalDetail',
            success: function (data) {
                if (data != null && data != undefined) {
                    //IdentiyProof
                    if (data.IdentiyProof == 'Passport') {
                        $('#IDPassport').prop('checked', true);
                    }
                    else if (data.IdentiyProof == 'PAN') {
                        $('#IDPAN').prop('checked', true);
                    }
                    else if (data.IdentiyProof == 'Voter ID Card') {
                        $('#IDvoterIDCard').prop('checked', true);
                    }
                    else if (data.IdentiyProof == 'Driving Liecence') {
                        $('#IDDrivingLiecence').prop('checked', true);
                    }
                    else if (data.IdentiyProof == 'AdhaarCard') {
                        $('#IDAdhaarCard').prop('checked', true);
                    }
                    else if (data.IdentiyProof == 'Company ID Card') {
                        $('#IDCompanyIDCard').prop('checked', true);
                    }

                    //ResidentialProof
                    if (data.ResidentialProof == 'Electric Bill') {
                        $('#RPElectricBill').prop('checked', true);
                    }
                    else if (data.ResidentialProof == 'ITR') {
                        $('#RPITR').prop('checked', true);
                    }
                    else if (data.ResidentialProof == 'Telephone Bill') {
                        $('#RPTelephoneBill').prop('checked', true);
                    }

                    else if (data.ResidentialProof == 'Bank Passbook Account Detail') {
                        $('#RPBankPassbook').prop('checked', true);
                    }
                    else if (data.ResidentialProof == 'Passport') {
                        $('#RPPassport').prop('checked', true);
                    }
                    else if (data.ResidentialProof == 'Voter ID Card') {
                        $('#RPVoterIDCard').prop('checked', true);
                    }

                    else if (data.ResidentialProof == 'HR Bill') {
                        $('#RPHRBill').prop('checked', true);
                    }
                    else if (data.ResidentialProof == 'Driving Liecence') {
                        $('#RPDrivingLiecence').prop('checked', true);
                    }

                    $('#FullName').val(data.FullApplicantName);
                    $('#FName').val(data.FName);
                    $('#MName').val(data.MName);
                    $('#SName').val(data.SName);
                    $('#Gender').val(data.Gender);
                    $('#Category').val(data.Category);
                    $('#Nationality').val(data.Nationality);
                    $('#AdhaarNo').val(data.AdhaarNumber);
                    $('#PAN').val(data.PAN);
                    $('#MobileNo').val(data.Mobile);
                    $('#Phone').val(data.Phone);
                    $('#Email').val(data.EmailId);
                    $('#Religion').val(data.Religion);
                    $('#SubCategory').val(data.SubCategory);
                    $('#CAddress').val(data.CAddress);
                    $('#PAddress').val(data.PAddress);

                    var milli = data.ApplicantDOB.replace(/\/Date\((-?\d+)\)\//, '$1');
                    var now = new Date(parseInt(milli));

                    var day = ("0" + now.getDate()).slice(-2);
                    var month = ("0" + (now.getMonth() + 1)).slice(-2);

                    var today = now.getFullYear() + "-" + (month) + "-" + (day);
                    $('#DOB').val(today);

                    $('#btnStep2Skip').removeClass('hidden');
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

    function FillAppliedFor(selectedAppliedForID = null) {
        let dropdown = $('#AppliedFor');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "AppliedFor" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedAppliedForID != null) {
                    dropdown.val(selectedAppliedForID);
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

    function FillSchemeName(SchemeTypeId, selectedSchemeNameId = null) {
        let dropdown = $('#SchemeName');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: "' + SchemeTypeId + '",lookupType: "SchemeName" }',
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

    $('#SectorName').on('change', function (e) {
        $('#SectorDescription').val($("#SectorName option:selected").text());
    });

    function FillSector(SchemeNameId, selectedSectorId = null) {
        let dropdown = $('#SectorName');
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
                if (selectedSectorId != null) {
                    dropdown.val(selectedSectorId);
                    $('#SectorDescription').val($("#SectorName option:selected").text());
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

    function FillPlotRange(selectedPlotRange = null) {
        let dropdown = $('#PlotRange');
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

    function FillPaymemtSchedule(selectedPaymemtSchedule = null) {
        let dropdown = $('#PaymemtSchedule');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "PaymemtSchedule" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option selected="true"></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedPaymemtSchedule != null) {
                    dropdown.val(selectedPaymemtSchedule);
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

    function FillIndustryOwnershipType(selectedIndustryOwnershipType = null) {
        let dropdown = $('#IndustryOwnershipType');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "IndustryOwnershipType" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedIndustryOwnershipType != null) {
                    dropdown.val(selectedIndustryOwnershipType);
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

    function FillRelationshipStatus(selectedRelationshipStatus = null) {
        let dropdown = $('#RelationshipStatus');
        dropdown.empty();
        dropdown.append('<option value="0">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "RelationshipStatus" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedRelationshipStatus != null) {
                    dropdown.val(selectedRelationshipStatus)
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

    $('#btnPlotDetailSave').on('click', function (e) {
        if ($('#ApplicationFee').val() != '' && $('#AppliedFor').val() != '' && $('#EarnestMoneyDeposite').val() != '' &&
            $('#GST').val() != '' && $('#IndustryOwnershipType').val() != '' &&
            $('#NetAmount').val() != '' && $('#PaymemtSchedule').val() != '' && $('#plotArea').val() != '' &&
            $('#PlotRange').val() != '' && $('#SchemeName').val() != '' &&
            $('#SchemeType').val() != '' && $('#SectorName').val() != '' && $('#dob').val() != '' &&
            $('#Name').val() != '' && $('#PermanentAddress').val() != '' && $('#PresentAddress').val() != '' &&
            $('#TotalAmount').val() != '' && $('#TotalInvestment').val() != '' && $('#UnitName').val() != '' &&
            $('#SectorDescription').val() != '') {
            var url = '/Applicant/SavePlotDetail';
            var inputData = {
                ApplicationFee: $('#ApplicationFee').val(),
                AppliedFor: $('#AppliedFor').val(),
                EarnestMoneyDeposite: $('#EarnestMoneyDeposite').val(),
                EstimatedRate: $('#EstimatedRate').val(),
                GST: $('#GST').val(),
                IndustryOwnershipType: $('#IndustryOwnershipType').val(),
                NetAmount: $('#NetAmount').val(),
                PaymemtSchedule: $('#PaymemtSchedule').val(),
                PlotArea: $('#plotArea').val(),
                PlotRange: $('#PlotRange').val(),
                RelationshipStatus: $('#RelationshipStatus').val(),
                SchemeName: $('#SchemeName').val(),
                SchemeType: $('#SchemeType').val(),
                SectorName: $('#SectorName').val(),
                dob: $('#dob').val(),
                Name: $('#Name').val(),
                PermanentAddress: $('#PermanentAddress').val(),
                PresentAddress: $('#PresentAddress').val(),
                TotalAmount: $('#TotalAmount').val(),
                TotalInvestment: $('#TotalInvestment').val(),
                UnitName: $('#UnitName').val(),
                SectorDescription: $('#SectorDescription').val()
            };
            utility.ajax.helperWithData(url, inputData, function (data) {
                if (data != 'Error') {
                    //$('#progressbar li').removeClass('active');
                    //$('#ApplicantDetail').addClass('active');
                    NextStep($('#btnPlotDetailSave'));
                    getApplicantPersonalDetail();
                    $('#spanApplicationNumber').html(data);
                    $('#divApplicationNumber').css('display', 'block');

                    utility.alert.setAlert(utility.alert.alertType.success, 'Plot Detail has been Saved, Your Application Number is ' + data);
                }
            });
        }
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please fill required input.');
        }

    });

    $('#Step2PreviousButton').on('click', function (e) {
        //$('#progressbar li').removeClass('active');
        //$('#PlotDetail').addClass('active');
        PreviousStep($('#Step2PreviousButton'));
    });

    $('#Step2NextButton').on('click', function (e) {
        var url = '/Applicant/SaveApplicantDetails';
        var IdentityProof = '';
        if ($('#IDPassport').prop('checked')) {
            IdentityProof = $('#IDPassport').val();
        }
        else if ($('#IDPAN').prop('checked')) {
            IdentityProof = $('#IDPAN').val();
        }
        else if ($('#IDvoterIDCard').prop('checked')) {
            IdentityProof = $('#IDvoterIDCard').val();
        }
        else if ($('#IDDrivingLiecence').prop('checked')) {
            IdentityProof = $('#IDDrivingLiecence').val();
        }
        else if ($('#IDAdhaarCard').prop('checked')) {
            IdentityProof = $('#IDAdhaarCard').val();
        }
        else if ($('#IDCompanyIDCard').prop('checked')) {
            IdentityProof = $('#IDCompanyIDCard').val();
        }

        var ResidentialProof = '';
        if ($('#RPElectricBill').prop('checked')) {
            ResidentialProof = $('#RPElectricBill').val();
        }
        else if ($('#RPITR').prop('checked')) {
            ResidentialProof = $('#RPITR').val();
        }
        else if ($('#RPTelephoneBill').prop('checked')) {
            ResidentialProof = $('#RPTelephoneBill').val();
        }
        else if ($('#RPBankPassbook').prop('checked')) {
            ResidentialProof = $('#RPBankPassbook').val();
        }
        else if ($('#RPPassport').prop('checked')) {
            ResidentialProof = $('#RPPassport').val();
        }
        else if ($('#RPVoterIDCard').prop('checked')) {
            ResidentialProof = $('#RPVoterIDCard').val();
        }
        else if ($('#RPHRBill').prop('checked')) {
            ResidentialProof = $('#RPHRBill').val();
        }
        else if ($('#RPDrivingLiecence').prop('checked')) {
            ResidentialProof = $('#RPDrivingLiecence').val();
        }

        if ($('#AdhaarNo').val().trim().length != 12) {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please enter 12 digit Aadhar Number.');
            return false;
        }

        if ($('#FullName').val() != '' && $('#FName').val() != '' && $('#MName').val() != '' &&
            $('#DOB').val() != '' && $('#Gender').val() != '' && $('#Category').val() != '' &&
            $('#Nationality').val() != '' && $('#PAN').val() != '' && $('#MobileNo').val() != '' &&
            $('#Email').val() != '' && $('#CAddress').val() != '' && $('#AdhaarNo').val() != '' &&
            $('#PAddress').val() != '' && IdentityProof != '' && ResidentialProof != '') {
            var inputData = {
                FullName: $('#FullName').val(),
                FName: $('#FName').val(),
                MName: $('#MName').val(),
                SName: $('#SName').val(),
                DOB: $('#DOB').val(),
                Gender: $('#Gender').val(),
                Category: $('#Category').val(),
                Nationality: $('#Nationality').val(),
                AdhaarNo: $('#AdhaarNo').val(),
                PAN: $('#PAN').val(),
                MobileNo: $('#MobileNo').val(),
                Phone: $('#Phone').val(),
                Email: $('#Email').val(),
                Religion: $('#Religion').val(),
                SubCategory: $('#SubCategory').val(),
                CAddress: $('#CAddress').val(),
                PAddress: $('#PAddress').val(),
                IdentityProof: IdentityProof,
                ResidentialProof: ResidentialProof,
            };

            utility.ajax.helperWithData(url, inputData, function (data) {
                if (data == 'Data has been saved') {
                    //$('#progressbar li').removeClass('active');
                    //$('#ProjectDetail').addClass('active');
                    NextStep($('#Step2NextButton'));
                    getApplicantProjectDetail();
                    utility.alert.setAlert(utility.alert.alertType.success, 'Applicant Detail has been Saved');
                }
            });
        }
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please fill required input.');
        }

    });


    $('#Step3PreviousButton').on('click', function (e) {
        //$('#progressbar li').removeClass('active');
        //$('#ApplicantDetail').addClass('active');
        PreviousStep($('#Step3PreviousButton'));
    });

    $('#Step3NextButton').on('click', function (e) {
        if ($('#ProposedIndustryType').val() != '' && $('#ProjectEstimatedCost').val() != '' && $('#CoveredArea').val() != '' &&
            $('#OpenArea').val() != '' && $('#Purposeforopenarea').val() != '' && $('#Investmentland').val() != '' &&
            $('#InvestmentBuilding').val() != '' && $('#InvestmentPlant').val() != '' && $('#processofmanufacture').val() != '') {
            var url = '/Applicant/SaveProjectDetails';
            var inputData = {
                ProposedIndustryType: $('#ProposedIndustryType').val(),
                ProjectEstimatedCost: $('#ProjectEstimatedCost').val(),
                ProposedCoveredArea: $('#CoveredArea').val(),
                ProposedOpenArea: $('#OpenArea').val(),
                PurpuseOpenArea: $('#Purposeforopenarea').val(),
                ProposedInvestmentLand: $('#Investmentland').val(),
                ProposedInvestmentBuilding: $('#InvestmentBuilding').val(),
                ProposedInvestmentPlant: $('#InvestmentPlant').val(),
                FumesNatureQuantity: $('#processofmanufacture').val(),
                LiquidQuantity: $('#LiquidQuantity').val(),
                LiquidChemicalComposition: $('#LiquidChemicalComposition').val(),
                SolidQuantity: $('#SolidQuantity').val(),
                SolidChemicalComposition: $('#SolidChemicalComposition').val(),
                GasQuantity: $('#GasQuantity').val(),
                GasChemicalComposition: $('#GasChemicalComposition').val(),
                //EffluentTreatmentMeasures: $('#GasChemicalComposition').val(),
                PowerRequirement: $('#PowerRequirement').val(),
                FirstYearNoOfTelephone: $('#FirstYearTelephonicConnection').val(),
                FirstYearNoOfFax: $('#FirstYearFaxConnection').val(),
                UltimateNoOfTelephone: $('#UltimateRequirementTelephonicConnection').val(),
                UltimateNoOfFax: $('#UltimateRequirementFaxConnection').val(),
                Skilled: $('#Skilled').val(),
                UnSkilled: $('#Unskilled').val(),
            };

            utility.ajax.helperWithData(url, inputData, function (data) {
                if (data == 'Data has been saved') {
                    //$('#progressbar li').removeClass('active');
                    //$('#BankDetail').addClass('active');
                    utility.alert.setAlert(utility.alert.alertType.success, 'Project Detail has been Saved');
                    getApplicantBankDetail();
                    NextStep($('#Step3NextButton'));
                }
            });
        }
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please fill required input.');
        }

    });

    $('#Step4PreviousButton').on('click', function (e) {
        //$('#progressbar li').removeClass('active');
        //$('#ProjectDetail').addClass('active');
        PreviousStep($('#Step4PreviousButton'));
    });

    $('#Step4NextButton').on('click', function (e) {
        if ($('#BankAccountName').val() != '' && $('#BankAccountNo').val() != '' && $('#BankName').val() != '' &&
            $('#BranchName').val() != '' && $('#BranchAddress').val() != '' && $('#IFSCCode').val() != '') {
            var url = '/Applicant/SaveBankDetail';
            var inputData = {
                BankAccountName: $('#BankAccountName').val(),
                BankAccountNo: $('#BankAccountNo').val(),
                BankName: $('#BankName').val(),
                BranchName: $('#BranchName').val(),
                BranchAddress: $('#BranchAddress').val(),
                IFSCCode: $('#IFSCCode').val(),
            };

            utility.ajax.helperWithData(url, inputData, function (data) {
                if (data == 'Data has been saved') {
                    //$('#progressbar li').removeClass('active');
                    //$('#AttachDocument').addClass('active');
                    utility.alert.setAlert(utility.alert.alertType.success, 'Bank Detail has been Saved');
                    NextStep($('#Step4NextButton'));
                    getApplicantDocumentDetail();
                    //$('#btnStep5Skip').removeClass('hidden');
                }
            });
        }
        else {
            utility.alert.setAlert(utility.alert.alertType.error, 'Please fill required input.');
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

