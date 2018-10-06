(function () {
    'use strict';

    var url;

    // Declare: ehrClaimListController
    //ehrClaimListController.$inject = ['entityList'];
    //function ehrClaimListController(entityList) {

    ehrClaimListController.$inject = ['ApiService', 'UIService'];
    function ehrClaimListController(ApiService, UIService) {
        var _this = this;

        _this.searchList = function () {
            var arrParms = [
                _this.lastName ? _this.lastName : "",
                _this.dateOfBirth ? UIService.getDateParam(_this.dateOfBirth) : ""
            ];

            var parms = arrParms.join('|');
            console.log('searchClms: ' + parms);
            _this.searching = true;

            ApiService.searchEntities('Claims', parms).then(function (response) {
                _this.entities = (response.data.result.IsSuccess == true) ?
                    response.data.result.Data : [];

                _this.searching = false;
            });

        };

        _this.init = function () {
            _this.searching = false;
            UIService.applyDatePickers();
        };

        _this.init();
    }

    // Register: ehrClaimListController
    angular.module('ehrApp').controller('ehrClaimListController', ehrClaimListController);

    // Main Claim Edit controller
    //ehrClaimEditController.$inject = ['$state', 'subscriber', 'claim', 'groups', 'facilities', 'providers', 'ClaimEditService'];
    //function ehrClaimEditController($state, subscriber, claim, groups, facilities, providers, ClaimEditService) {
    ehrClaimEditController.$inject = ['$state', 'ngDialog', 'subscriber', 'claim', 'history', 'ApiService', 'ClaimEditService', 'UIService'];
    function ehrClaimEditController($state, ngDialog, subscriber, claim, history, ApiService, ClaimEditService, UIService) {
        var _this = this;
        var list;

        // Validate claim info before submitting
        _this.validateForm = function () {
            console.log('_this.validateForm');
            _this.errors = [];

            if (!_this.claim.BillingProviderId) {
                _this.errors.push('No Billing Group selected');
            }

            if (!_this.claim.RenderingProviderId) {
                _this.errors.push('No Rendering Provider selected');
            }

            if (!_this.claim.FacilityId) {
                _this.errors.push('No Service Facility selected');
            }

            if (!_this.claim.PlaceOfServiceId) {
                _this.errors.push('No Place of Service selected');
            }

            if (!_this.claim.DateOfService) {
                _this.errors.push('Enter the Date of Service');
            }

            if (!_this.claim.DiagnosisCodes) {
                _this.errors.push('Enter at least one diagnosis code');
            }

            if (_this.claim.ClaimLines.length <= 0) {
                _this.errors.push('Enter at least one claim line');
            }

        };

        _this.saveClaim = function (claim) {
            _this.savingForm = true;

            ApiService.updateEntity({
                type: 'Claim',
                entity: claim
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;
                console.log(resData);

                _this.savingForm = false;
                _this.copyingClaim = false;
                _this.canCopyForm = true;

                if (resSuccess == true) {
                    if (claim.Id > 0) {
                        alert('Changes saved!');
                    }
                    else {
                        alert('Claim successfully created!');
                    }

                    if (resData != claim.Id) {
                        $state.go('claim-edit', {
                            subscriberId: _this.subscriber.Id,
                            claimId: resData
                        });
                    }

                    //_this.init();
                }
                else {
                    console.log('exception: ' + resData.toString());
                }
            });
        };

        // Submit claim form
        _this.submitForm = function (copy) {
            console.log('_this.submitForm');

            _this.validateForm();
            if (_this.errors.length > 0) {
                var str = 'Please correct the following errors first:\n\n';
                for (var i = 0, n = _this.errors.length; i < n; i++) {
                    str += '- ' + _this.errors[i] + '\n';
                }
                alert(str);

                _this.savingForm = false;
                return;
            }

            _this.saveClaim(_this.claim);
        }

        _this.initUI = function () {
            _this.page_title = UIService.getEditPageTitle('claim', _this.claim.Id);

            // Render the first subtab
            $state.go('claim-edit.diagnosis');
        };

        // Go to another claim from the claim history list
        _this.switchClaim = function (id) {
            console.log('switchClaim ' + _this.subscriber.Id + ', ' + id);

            //_this.initUI();

            if (id != _this.claim.Id) {
                $state.go('claim-edit', {
                    subscriberId: _this.subscriber.Id,
                    claimId: id
                });
            }
        };

        _this.init = function () {
            console.log('fire ehrClaimEditController');

            ClaimEditService.setSubscriber(subscriber.entityList);
            ClaimEditService.setClaim(claim.entityList);
            ClaimEditService.setClaimHistory(history.entityList);

            _this.claim = ClaimEditService.getClaim();
            /*
            if (isCopy) {
                _this.claim.Id = 0;
            }
            */
            _this.subscriber = ClaimEditService.getSubscriber();
            _this.savingForm = false;
            _this.canCopyClaim = _this.claim && _this.claim.Id > 0;
            _this.copyingClaim = false;

            // Page Title
            _this.initUI();
        };

        _this.copyClaim = function () {
            var c = _this.claim;
            c.Id = 0;

            for (var i = 0, n = c.ClaimLines.length; i < n; i++) {
                c.ClaimLines[i].Id = 0;
                //c.ClaimLines[i].ClaimId = 0;
            }

            console.log('copyClaim');
            console.log(c);
            //return;

            _this.copyingClaim = true;
            _this.savingForm = true;
            _this.saveClaim(c);
        };

        _this.cancelEdit = function () {
            if (confirm('Any unsaved changes will be lost. Are you sure?')) {
                $state.go('claims');
            }
        }

        _this.printClaim = function () {
            ApiService.printReport('CMS', null, _this.claim.Id);
        }

        _this.init();

    }

    // Controller for Claim main body fields
    ehrClaimHeaderController.$inject = ['$state', 'ngDialog', 'ApiService', 'UIService', 'ClaimEditService'];
    function ehrClaimHeaderController($state, ngDialog, ApiService, UIService, ClaimEditService) {
        var _this = this;
        var clm
        var sub;
        var history;
        
        // Format the claim history for display
        _this.formatClaimHistory = function () {
            var clmHist = [];

            for (var i = 0, n = history.length; i < n; i++) {
                var ch = history[i];
                console.log(ch);
                clmHist.push({
                    id: ch.Id,
                    acct: UIService.padLeft(ch.Id, 5, '0'),
                    date: UIService.parseDate(ch.DateOfService),
                    amt: ch.AmountTotal
                });
            }

            return clmHist;
        };
        
        // Go back to the Edit Subscriber page
        _this.editSubscriber = function () {
            if (confirm('This will take you back to the Edit Subscriber page. Any unsaved changes to this claim will be lost.\n\nAre you sure?')) {
                $state.go('subscriber-edit', { id: sub.Id });
            }
        };

        // Payer modal window
        _this.openPayerModal = function () {
            ngDialog.openConfirm({
                template: '/ehr/shared/views/ehr.shared.payer-modal.component.html',
                controller: 'PayerModalController',
                controllerAs: 'payerModalCtrl',
                disableAnimation: true,
                resolve: {
                    payers: ['ApiService', function (ApiService) {
                        var resp = ApiService.getEntityList('Payers', 'payer');
                        return resp.then(function (response) {
                            return ApiService.prepareResponse(response);
                        });
                    }]
                }
            }).then(
                function (value) {
                    console.log(value);
                    _this.claim.PrimaryPayerId = value.Id;
                    _this.claim.PrimaryPayer = { Name: value.Name };
                },
                function (value) {
                }
            );
        };

        // Function: Display current dependent in Patient Info section
        _this.displayPatient = function (patient) {
            _this.PatientFullName = patient.LastName + ", " + patient.FirstName;
            _this.PatientDateOfBirth = UIService.parseDate(patient.DateOfBirth);
            _this.PatientGender = UIService.getGender(patient.Gender);
            _this.PatientRelationship = ApiService.getRelationshipByCode(patient.Relationship).Name;
        };

        // Function: Update client-side Patient object
        _this.setPatient = function (patient) {
            _this.claim.PatientId = patient.PatientId;
            _this.claim.Patient.Id = patient.PatientId;
            _this.claim.Patient.FirstName = patient.FirstName;
            _this.claim.Patient.LastName = patient.LastName;
            _this.claim.Patient.DateOfBirth = UIService.parseDate(patient.DateOfBirth);
            _this.claim.Patient.Gender = patient.Gender;
            _this.claim.Relationship = patient.Relationship;

            console.log('_this.setPatient');
            console.log(ClaimEditService.getClaim());

            _this.displayPatient(patient);
        };

        // Open list of dependents
        _this.openDependentsModal = function () {
            ngDialog.openConfirm({
                template: '/ehr/shared/views/ehr.shared.dependent-modal.component.html',
                controller: 'DependentsModalController',
                controllerAs: 'depModalCtrl',
                disableAnimation: true,
                resolve: {
                    entities: ['ApiService', function (ApiService) {
                        var resp = ApiService.getDependentsList(ClaimEditService.getSubscriber().Id);
                        return resp.then(function (response) {
                            return ApiService.prepareResponse(response);
                        });
                    }]
                }
            }).then(
                function (value) {
                    _this.setPatient({
                        PatientId: value.id,
                        FirstName: value.fname,
                        LastName: value.lname,
                        DateOfBirth: value.dob,
                        Gender: value.gender,
                        Relationship: value.rel
                    });
                },
                function (value) {
                }
            );
        };

        // Switch to New Patient form
        _this.showNewDepForm = function (show, edit) {
            if (edit) {
                _this.newPat = {
                    PatientId: _this.claim.PatientId,
                    FirstName: _this.claim.Patient.FirstName,
                    LastName: _this.claim.Patient.LastName,
                    DateOfBirth: _this.claim.Patient.DateOfBirth,
                    Gender: _this.claim.Patient.Gender,
                    Relationship: _this.claim.Patient.Relationship
                };
            }
            else {
                _this.newPat = {};
            }

            _this.showNewDep = show;
        };

        // Save new dependent data
        _this.saveNewDepForm = function () {
            if (_this.newPat.Relationship == "18") {
                alert('ERROR: The dependent relationship must not be Self.');
                return;
            }

            if (!_this.newPat.FirstName ||
                !_this.newPat.LastName ||
                !_this.newPat.Relationship ||
                !_this.newPat.Gender ||
                !_this.newPat.DateOfBirth) {
                alert('ERROR: Please fill out all required dependent fields.');
                return;
            }

            var goSave = false;

            if (_this.newPat.PatientId) {
                goSave = confirm('Save changes?');
            }
            else {
                goSave = confirm('Create this dependent?');
            }

            if (goSave) {
                _this.savingNewDepForm = true;

                ApiService.updateSubDependent({
                    subId: _this.subscriber.Id,
                    patient: _this.newPat
                }).then(function (response) {
                    var resSuccess = response.data.result.IsSuccess;
                    var resData = response.data.result.Data;

                    if (resSuccess == true) {
                        _this.savingNewDepForm = false;
                        _this.setPatient({
                            PatientId: resData,
                            FirstName: _this.newPat.FirstName,
                            LastName: _this.newPat.LastName,
                            DateOfBirth: _this.newPat.DateOfBirth,
                            Gender: _this.newPat.Gender,
                            Relationship: _this.newPat.Relationship
                        });

                        _this.showNewDepForm(false);
                    }
                });
            }
        };

        // Header Initialization
        _this.init = function () {
            clm = ClaimEditService.getClaim();
            sub = clm.Id > 0 ? clm.PrimarySubscriber : ClaimEditService.getSubscriber();

            //initFieldMasks();
            UIService.applyDatePickers();
            UIService.applyMasks();

            _this.claim = clm;
            _this.subscriber = sub;
            _this.subscriber.FullName = sub.LastName + ", " + sub.FirstName;
            _this.subscriber.DateOfBirth = UIService.parseDate(sub.DateOfBirth);

            _this.date = UIService.parseDate();
            _this.acctNum = UIService.formatAcctNumber(_this.claim.Id);
            _this.claimStatus = UIService.getStatusText(_this.claim.FilingStatus);

            // Claim history
            history = ClaimEditService.getClaimHistory();
            console.log(history);
            _this.claimHistory = _this.formatClaimHistory();

            // Initialize claim if new
            if (_this.claim.Id <= 0) {
                _this.claim.PrimarySubscriberId = _this.subscriber.Id;
                _this.claim.PrimaryPayerId = _this.subscriber.PrimaryPayer.Id;
                _this.claim.PrimaryPayerMemberID = _this.subscriber.PrimaryMemberID;

                ClaimEditService.updateDateOfService(_this.date);
                _this.claim.PrimaryPayer = sub.PrimaryPayer;
            }

            _this.relationships = ApiService.getList('patrels').options;
            _this.newPat = {};
            _this.showNewDep = false;
            _this.savingNewDepForm = false;

            // Initialize Patient object if 'New Claim'
            var pat = _this.claim.Patient;
            var isSelf = clm.Relationship == "18";
            _this.setPatient({
                PatientId: _this.claim.PatientId,
                FirstName: isSelf ? sub.FirstName : pat.FirstName,
                LastName: isSelf ? sub.LastName : pat.LastName,
                DateOfBirth: isSelf ? sub.DateOfBirth : pat.DateOfBirth,
                Gender: isSelf ? sub.Gender : pat.Gender,
                Relationship: _this.claim.Relationship
            });

            _this.depRelationship = ApiService.getRelationshipByCode(clm.Relationship);
        }

        _this.init();

    }

    // Controller for provider selectors
    ehrClaimProvController.$inject = ['ngDialog', 'ClaimEditService', 'ModalService', 'AuthService'];
    function ehrClaimProvController(ngDialog, ClaimEditService, ModalService, AuthService) {
        var _this = this;
        var clm;

        _this.openListModal = function (p, s) {
            console.log('openListModal');
            ngDialog.openConfirm({
                template: '/ehr/shared/views/ehr.shared.' + s + '-modal.component.html',
                controller: (s[0].toUpperCase() + s.substring(1)) + 'ModalController',
                controllerAs: s + 'ModalCtrl',
                disableAnimation: true,
                resolve: {
                    entities: ['ApiService', function (ApiService) {
                        var resp = ApiService.getEntityList(p);
                        return resp.then(function (response) {
                            return ApiService.prepareResponse(response);
                        });
                    }]
                }
            }).then(
                function (value) {
                    console.log(value);
                    switch (p) {
                        case 'Groups': {
                            clm.BillingProviderId = value.Id;
                            _this.billingProvName = value.LastName;
                            break;
                        }
                        case 'Providers': {
                            clm.RenderingProviderId = value.Id;
                            _this.renderingProvFullName = value.LastName + ", " + value.FirstName;
                            clm.RenderingProvider = value;
                            ClaimEditService.updateRenderingNPI(value.NPI);
                            break;
                        }
                        case 'Facilities': {
                            clm.FacilityId = value.Id;
                            _this.facilityName = value.Name;
                            break;
                        }
                        case 'PlacesOfService': {
                            clm.PlaceOfService = value;
                            clm.PlaceOfServiceId = value.Id;
                            _this.posName = value.Name;
                            break;
                        }
                    }
                },
                function (value) {
                }
            );
        };

        _this.initDefaults = function () {
            var cd, pref;

            if (_this.claim.Id <= 0) {
                pref = AuthService.getPreference('BillingProvider');
                if (pref) {
                    _this.claim.BillingProvider = pref;
                    _this.claim.BillingProviderId = pref.Id;
                }

                pref = AuthService.getPreference('RenderingProvider');
                if (pref) {
                    _this.claim.RenderingProvider = pref;
                    _this.claim.RenderingProviderId = pref.Id;
                }

                pref = AuthService.getPreference('Facility');
                if (pref) {
                    _this.claim.Facility = pref;
                    _this.claim.FacilityId = pref.Id;
                }

                pref = AuthService.getPreference('PlaceOfService');
                if (pref) {
                    _this.claim.PlaceOfService = pref;
                    _this.claim.PlaceOfServiceId = pref.Id;
                }
            }
        };

        _this.init = function () {
            clm = ClaimEditService.getClaim();
            _this.claim = clm;

            _this.initDefaults();

            /* Providers section of the claim */
            _this.billingProv = _this.claim.BillingProvider;
            _this.billingProvName = _this.claim.BillingProvider ?
                _this.claim.BillingProvider.LastName : "";

            _this.renderingProv = _this.claim.RenderingProvider;
            _this.renderingProvFullName = _this.renderingProv ?
                (_this.renderingProv.LastName + ", " + _this.renderingProv.FirstName) : "";

            _this.facilityName = _this.claim.Facility ?
                _this.claim.Facility.Name : "";

            _this.posName = _this.claim.PlaceOfService ?
                _this.claim.PlaceOfService.Name : "";
        };

        _this.init();
    }

    // Configuration for claim edit subtabs
    claimSubtabConfig.$inject = ['$stateProvider'];
    function claimSubtabConfig($stateProvider) {
        var arrClaimStates = [];
        var arrClaimSubtabs = [
            'diagnosis',
            'subscriber',
            'provider',
            'other'
        ];

        for (var i = 0, n = arrClaimSubtabs.length; i < n; i++) {
            var cap = arrClaimSubtabs[i].charAt(0).toUpperCase() + arrClaimSubtabs[i].substr(1);
            $stateProvider.state('claim-edit.' + arrClaimSubtabs[i], {
                templateUrl: '/ehr/claims/claim-edit-' + arrClaimSubtabs[i] + '.html',
                controller: 'ehrClaim' + cap + 'Controller as clm' + cap
            });
        }
    }

    // Controller for Diagnosis subtab
    ehrClaimDiagnosisController.$inject = ['UIService', 'ClaimEditService'];
    function ehrClaimDiagnosisController(UIService, ClaimEditService) {
        var _this = this;
        var keys = 'ABCDEFGHIJKL';
        var clm;

        _this.updateDateOfService = function () {
            ClaimEditService.updateDateOfService(_this.claim.DateOfService);
        };

        _this.updateDiagnosis = function () {
            var keys = 'ABCDEFGHIJKL';
            var diags = [];

            for (var i = 0, n = keys.length; i < n; i++) {
                if (_this['diag_' + keys.charAt(i)]) {
                    diags.push(_this['diag_' + keys.charAt(i)]);
                }
            }
            _this.claim.DiagnosisCodes = diags.join(',');
        }

        _this.init = function () {
            //initFieldMasks();
            UIService.applyDatePickers();
            clm = ClaimEditService.getClaim();
            _this.claim = clm;
            _this.claim.DateOfService = UIService.parseDate(_this.claim.DateOfService);

            var diags = {};

            if (_this.claim.DiagnosisCodes) {
                var values = _this.claim.DiagnosisCodes.split(',');

                for (var i = 0, n = values.length; i < n; i++) {
                    console.log('diag_' + keys.charAt(i) + ': ' + values[i]);
                    _this['diag_' + keys.charAt(i)] = values[i];
                }
            }
        };

        _this.init();
    }

    // Controller for Subscriber subtab
    ehrClaimSubscriberController.$inject = ['UIService', 'ClaimEditService'];
    function ehrClaimSubscriberController(UIService, ClaimEditService) {
        var _this = this;
        var sub;
        var clm;

        _this.init = function () {
            clm = ClaimEditService.getClaim();
            _this.claim = clm;

            sub = _this.claim.Id > 0 ?
                _this.claim.PrimarySubscriber : ClaimEditService.getSubscriber();

            _this.subscriber = {
                name: sub.LastName + ', ' + sub.FirstName,
                dob: UIService.parseDate(sub.DateOfBirth),
                gender: sub.Gender == 'M' ? 'Male' : 'Female',
                phone: sub.Phone_1,
                ssn: sub.SSN,
                address_1: sub.Address_1,
                hasAddress_2: sub.Address_2 != null && sub.Address_2 != undefined &&
                    sub.Address_2 != '',
                address_2: sub.Address_2,
                city: sub.City,
                state: sub.State,
                zip: sub.Zip,

                primaryPayer: sub.PrimaryPayer,
                primaryMemberId: sub.PrimaryMemberID,
                secondaryPayer: sub.SecondaryPayer,
                secondaryMemberId: sub.SecondaryMemberID
            };
        }

        _this.init();
    }

    // Controller for Provider subtab
    ehrClaimProviderController.$inject = ['ClaimEditService'];
    function ehrClaimProviderController(ClaimEditService) {
        var _this = this;
        var clm = ClaimEditService.getClaim();

        _this.claim = clm;

        _this.billingProv = clm.BillingProvider;
        _this.billingProvName = clm.BillingProvider.LastName;

        _this.renderingProv = clm.RenderingProvider;
        _this.renderingProvFullName = _this.renderingProv ?
            (_this.renderingProv.LastName + ", " + _this.renderingProv.FirstName) : "";

        _this.facilityProv = clm.Facility;
    }

    // Controller for Other subtab
    // IN PROGRESS
    ehrClaimOtherController.$inject = ['ClaimEditService', 'UIService'];
    function ehrClaimOtherController(ClaimEditService, UIService) {
        var _this = this;

        _this.formatDates = function () {
            if (_this.claim.Dates) {
                _this.claim.Dates = {
                    OnsetOfCurrent: UIService.parseDate(_this.claim.Dates.OnsetOfCurrent),
                    InitialTreatment: UIService.parseDate(_this.claim.Dates.InitialTreatment),
                    LastSeen: UIService.parseDate(_this.claim.Dates.LastSeen),
                    Acute: UIService.parseDate(_this.claim.Dates.Acute),
                    LastMenstrual: UIService.parseDate(_this.claim.Dates.LastMenstrual),
                    LastXRay: UIService.parseDate(_this.claim.Dates.LastXRay),
                    HearingVision: UIService.parseDate(_this.claim.Dates.HearingVision),
                    LastWorked: UIService.parseDate(_this.claim.Dates.LastWorked),
                    ReturnToWork: UIService.parseDate(_this.claim.Dates.ReturnToWork),
                    Admission: UIService.parseDate(_this.claim.Dates.Admission),
                    Discharge: UIService.parseDate(_this.claim.Dates.Discharge),
                    Assumed: UIService.parseDate(_this.claim.Dates.Assumed),
                    Property: UIService.parseDate(_this.claim.Dates.Property),
                    Repricer: UIService.parseDate(_this.claim.Dates.Repricer),
                    Other: UIService.parseDate(_this.claim.Dates.Other),
                    DisabilityStart: UIService.parseDate(_this.claim.Dates.DisabilityStart),
                    DisabilityEnd: UIService.parseDate(_this.claim.Dates.DisabilityEnd)
                };
            }
        };

        _this.init = function () {
            _this.claim = ClaimEditService.getClaim();
            console.log(_this.claim);

            _this.formatDates();
            _this.acceptAssignment = (_this.claim.AcceptAssignment != 'C');
            _this.outsideLab = (_this.claim.AcceptAssignment != null);
        };

        _this.init();
    }

    // Controller for Claim Lines sublist
    // IN PROGRESS
    ehrClaimLinesController.$inject = ['ClaimEditService', 'ApiService'];
    function ehrClaimLinesController(ClaimEditService, ApiService) {
        console.log('fire ehrClaimLinesController');
        var _this = this;
        var clm;

        _this.validateClaimLine = function () {
            var errors = [];

            if (!_this.newLine.CPT) {
                errors.push('-CPT Code\n');
            }

            if (!_this.newLine.Amount) {
                errors.push('-Charges\n');
            }

            if (!_this.newLine.Unit) {
                errors.push('-Unit\n');
            }

            if (!_this.newLine.Quantity) {
                errors.push('-Quantity\n');
            }

            if (errors.length > 0) {
                var str = 'The following claim line fields are missing:\n\n';
                for (var i = 0, n = errors.length; i < n; i++) {
                    str += errors[i];
                }

                alert(str);
                return false;
            }

            return true;
        };

        _this.calcTotalAmt = function () {
            ClaimEditService.recalcAmtTotal();
        };

        _this.addClaimLine = function () {
            if (_this.validateClaimLine() == false) {
                return;
            }

            ClaimEditService.addClaimLine();
            _this.newLine = ClaimEditService.getNewLine();
            document.getElementById('claimLineFocus').focus();
        };

        _this.copyClaimLine = function (index) {
            ClaimEditService.copyClaimLine(index);
        };

        _this.removeClaimLine = function (index) {
            console.log('removeClaimLine: ' + index);
            ClaimEditService.removeClaimLine(index);
        };

        _this.getDateOfService = function () {
            return ClaimEditService.getDateOfService();
        };

        _this.canEditLines = function () {
            return ClaimEditService.canEditLines();
        };

        _this.init = function () {
            clm = ClaimEditService.getClaim();
            _this.claim = clm;

            ClaimEditService.formatLineDates();
            console.log(_this.claim);

            _this.newLine = ClaimEditService.getNewLine();
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrClaimEditController', ehrClaimEditController)
        .controller('ehrClaimHeaderController', ehrClaimHeaderController)
        .controller('ehrClaimProvController', ehrClaimProvController)
        .controller('ehrClaimDiagnosisController', ehrClaimDiagnosisController)
        .controller('ehrClaimSubscriberController', ehrClaimSubscriberController)
        .controller('ehrClaimProviderController', ehrClaimProviderController)
        .controller('ehrClaimOtherController', ehrClaimOtherController)
        .controller('ehrClaimLinesController', ehrClaimLinesController)
        .config(claimSubtabConfig);
})();

