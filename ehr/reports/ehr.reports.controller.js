(function () {
    'use strict';

    ehrReportsStateConfig.$inject = ['$stateProvider', '$urlRouteProvider'];
    function ehrReportsStateConfig($stateProvider, $urlRouteProvider) {
        console.log('fire ehrReportsStateConfig');
    }

    ehrReportsController.$inject = ['$state', 'ngDialog', 'ApiService', 'UIService'];
    function ehrReportsController($state, ngDialog, ApiService, UIService) {
        var _this = this;
        _this.pageNum = 1;
        _this.pageSize = 20;
        _this.repType = "";
        _this.repParams = [];
        _this.execReport = false;
        //_this.repChanged = false;

        _this.openDateModal = function () {
            console.log('openDateModal');
            ngDialog.openConfirm({
                template: 'ehr/shared/views/ehr.shared.date-modal.component.html',
                //controller: 'DateModalController',
                //controllerAs: 'dateModalCtrl',
                disableAnimation: true
            }).then(
                function(value) {
                    console.log(value);
                    _this.dateRange = value.Id;
                    _this.dateRangeName = value.Name;
                }
            );
        };

        // Show dialog window for providers/facilities
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
                        case 'Providers': {
                            _this.providerId = value.Id;
                            _this.providerName = value.LastName + ", " + value.FirstName;
                            //ClaimEditService.updateRenderingNPI(value.NPI);
                            break;
                        }
                        case 'Facilities': {
                            _this.facilityId = value.Id;
                            _this.facilityName = value.Name;
                            break;
                        }
                    }
                },
                function (value) {
                }
            );
        };

        // Clear provider/facility filters
        _this.clearField = function (type) {
            switch (type.toUpperCase()) {
                case 'PROV': {
                    _this.providerId = 0;
                    _this.providerName = "";
                    break;
                }
                case 'FAC': {
                    _this.facilityId = 0;
                    _this.facilityName = "";
                    break;
                }
                case 'DATE': {
                    _this.dateRange = null;
                    _this.dateRangeName = "";
                    break;
                }
                default: break;
            }
        };

        _this.showMonthlySummary = function () {
            _this.viewDetail = false;
            _this.repType = "CLAIMS_BYMONTH_SUMMARY";
            _this.pageNum = 1;
            //_this.repChanged = true;
            _this.getReport();
        };

        // This will be the generic report summary function
        // Will replace showMonthlySummary
        _this.showReportSummary = function () {
            _this.viewDetail = false;
            _this.pageNum = 1;
            _this.getReport();
        };

        _this.showMonthlyDetail = function (year, month, page) {
            _this.detailTitle = UIService.months[month - 1] + ' ' + year;
            _this.viewDetail = true;
            //_this.repType = "CLAIMS_BYMONTH_DETAIL";
            //_this.repChanged = true;
            _this.detailYear = year;
            _this.detailMonth = month;

            /*
            if (page == '' || page == null || page == undefined) {
                _this.pageNum = 1;
            }
            else {
                _this.pageNum += page;

                if (_this.pageNum < 1) {
                    _this.pageNum = 1;
                }
            }
            */

            if (_this.repParams.length >= 6) {
                _this.repParams[6] = UIService.padLeft(month, 2, '0') + year;
            }
            else if (_this.repParms.length == 5) {
                _this.repParams.push(UIService.padLeft(month, 2, '0') + year);
            }

            //_this.getClaimReport(_this.repType, ["reports", _this.pageNum, _this.pageSize, "", "", "", UIService.padLeft(month, 2, '0') + year]);
            /*
            _this.pageNum = UIService.getPage(_this.pageNum, page);
            _this.repParams[1] = _this.pageNum;
            _this.getClaimReport();
            */
            _this.nextPage(page);
        };

        _this.showAgingDetail = function (age, page) {
            _this.detailTitle = age + ' Days';
            _this.viewDetail = true;
            //_this.repType = "CLAIMS_BYMONTH_DETAIL";
            //_this.repChanged = true;

            _this.agingPeriod = age;

            /*
            if (page == '' || page == null || page == undefined) {
                _this.pageNum = 1;
            }
            else {
                _this.pageNum += page;

                if (_this.pageNum < 1) {
                    _this.pageNum = 1;
                }
            }
            */

            if (_this.repParams.length >= 6) {
                _this.repParams[6] = age;
            }
            else if (_this.repParms.length == 5) {
                _this.repParams.push(age);
            }

            //_this.getClaimReport(_this.repType, ["reports", _this.pageNum, _this.pageSize, "", "", "", UIService.padLeft(month, 2, '0') + year]);
            /*
            _this.pageNum = UIService.getPage(_this.pageNum, page);
            _this.repParams[1] = _this.pageNum;
            _this.getClaimReport();
            */
            _this.nextPage(page);
        }

        // Send reports async call
        _this.getClaimReport = function () {
            var summaryReports = [
                'CLAIMS_BYMONTH_SUMMARY',
                'CLAIMS_AGING_SUMMARY'
            ];

            _this.generating = true;
            console.log('getClaimReport: ' + _this.repType);

            if (summaryReports.indexOf(_this.repType) >= 0 && _this.viewDetail == false) {
                ApiService.getClaimReport(_this.repType).then(function (response) {
                    var arr = [];
                    arr = (response.data.result.IsSuccess == true) ?
                        response.data.result.Data : [];

                    if (arr.length > 0) {
                        _this.entities = arr;
                    }

                    if (_this.execReport == false) {
                        _this.pageNum = 1;
                    }

                    _this.generating = false;
                    _this.viewDetail = false;
                    _this.execReport = true;

                    _this.showReport();
                });
            }
            else if (_this.repType) {
                var parms = _this.repParams.join('|');
                console.log('params: ' + parms);

                ApiService.searchEntities('Claims', parms).then(function (response) {
                    _this.searched = true;
                    _this.entities = [];

                    var arr = [];
                    arr = (response.data.result.IsSuccess == true) ?
                        response.data.result.Data : [];

                    if (arr.length > 0) {
                        for (var i = 0, n = arr.length; i < n; i++) {
                            arr[i].Diagnosis = UIService.parseDiagCodes(arr[i].Diagnosis);
                        }

                        _this.entities = arr;
                    }

                    if (_this.execReport == false) {
                        _this.pageNum = 1;
                    }

                    _this.resultsCount = response.data.result.Count;
                    _this.startIndex = ((_this.pageNum - 1) * _this.pageSize) + 1;
                    _this.endIndex = _this.resultsCount > (_this.pageNum * _this.pageSize) ?
                        (_this.pageNum * _this.pageSize) : _this.resultsCount;
                    _this.generating = false;
                    _this.execReport = true;

                    _this.showReport();
                });
            }
        };

        _this.refreshReport = function () {
            //_this.repChanged = false;
            _this.getClaimReport();
        }

        // Build report filters for async call
        _this.getReport = function () {
            console.log('getReport: ' + _this.repType);

            if (!_this.repType) {
                return;
            }

            if (_this.repType) {
                $state.go('reports.' + _this.repType);

                _this.generating = true;
            }

            //if (_this.repChanged == true) {
            _this.pageNum = 1;
            _this.resultsCount = 0;
            //}

            _this.today = UIService.parseDate();

            var bRun = false;
            var arrParms = [
                _this.repType,
                _this.pageNum,
                _this.pageSize,
                "", ""
            ];
            var arr = [];
            
            switch (_this.repType) {
                case "CLAIMS_ENTERED_TODAY": {
                    arrParms.push(UIService.getDateParam());
                    bRun = true;
                    break;
                }
                case "CLAIMS_BYMONTH_SUMMARY": {
                    _this.viewDetail = false;
                    arrParms.push("");
                    bRun = true;
                    break;
                }
                /*
                case "CLAIMS_BYMONTH_DETAIL": {
                    _this.viewDetail = true;
                    arrParms.push("");
                    bRun = true;
                    break;
                }
                */
                case "CLAIMS_BILLING": {
                    console.log('in CLAIMS_BILLING');
                    _this.showFilters = true;
                    arrParms.push(_this.dateEntered ? UIService.getDateParam(_this.dateEntered) : "");
                    arrParms.push("");
                    arrParms.push([
                        _this.facilityId,
                        _this.providerId
                    ].join(","));

                    bRun = true;
                    break;
                }
                case "CLAIMS_PAYMENT": {
                    _this.showFilters = true;
                    arrParms.push("");
                    arrParms.push("");
                    arrParms.push([
                        _this.facilityId,
                        _this.providerId,
                        _this.cptCode,
                        _this.deductible,
                        _this.copay
                    ].join(","));
                    bRun = true;
                    break;
                }
                case "CLAIMS_AGING_SUMMARY": {
                    _this.showFilters = true;
                    _this.viewDetail = false;
                    arrParms.push("");
                    //arrParms.push(_this.agingPeriod);
                    bRun = true;
                    break;
                }
                /*
                case "CLAIMS_AGING_DETAIL": {
                    _this.viewDetail = true;
                    arrParms.push("");
                    bRun = true;
                    break;
                }
                */
                default: break;
            }

            console.log(arrParms);
            if (bRun) {
                _this.repParams = arrParms;
                _this.getClaimReport();
            }
        };

        // Show filters section on change of report type
        _this.changeReportType = function () {
            console.log('changeReportType: ' + _this.repType);

            UIService.applyDatePickers();
            switch (_this.repType) {    
                case "CLAIMS_BILLING": {
                    _this.showFilters = true;
                    break;
                }
                case "CLAIMS_PAYMENT": {
                    _this.showFilters = true;
                    break;
                }
                case "CLAIMS_AGING_SUMMARY": {
                    _this.showFilters = true;
                    break;
                }
                default: {
                    _this.showFilters = false;
                };
            }

            //_this.repChanged = true;
            _this.execReport = false;
            _this.viewDetail = false;
            _this.showReport();
        };

        // Results page navigator
        _this.nextPage = function (page) {
            /*
            if (page == '' || page == null || page == undefined) {
                _this.pageNum = 1;
            }
            else {
                _this.pageNum += page;

                if (_this.pageNum < 1) {
                    _this.pageNum = 1;
                }
            }
            */

            _this.pageNum = UIService.getPage(_this.pageNum, page);
            _this.repParams[1] = _this.pageNum;
            _this.getClaimReport();
        };

        _this.showReport = function () {
            console.log('execReport: ' + _this.execReport);
            console.log('generating: ' + _this.generating);
            console.log('viewDetail: ' + _this.viewDetail);

            _this.showSummary = _this.execReport == true &&
                _this.generating == false &&
                _this.viewDetail == false;

            _this.showDetail = _this.execReport == true &&
                _this.generating == false &&
                _this.viewDetail == true;

            console.log('showSummary: ' + _this.showSummary);
            console.log('showDetail: ' + _this.showDetail);
        };

        // Reports page initializer
        _this.init = function () {
            _this.generating = false;
            _this.providerId = 0;
            _this.providerName = "";
            _this.facilityId = 0;
            _this.facilityName = "";
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrReportsController', ehrReportsController);
        //.config(ehrReportsStateConfig);
})();
