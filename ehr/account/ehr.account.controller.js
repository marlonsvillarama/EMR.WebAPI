(function () {
    'use strict';

    ehrAccountController.$inject = ['$state', 'AuthService'];
    function ehrAccountController($state, AuthService) {
        var _this = this;
        console.log('*** ehrAccountController ***');

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
                            //clm.BillingProviderId = value.Id;
                            _this.ctx.Preferences.BillingProvider = value;
                            _this.billingProvName = value.LastName;
                            break;
                        }
                        case 'Providers': {
                            //clm.RenderingProviderId = value.Id;
                            _this.ctx.Preferences.RenderingProvider = value;
                            _this.renderingProvFullName = value.LastName + ", " + value.FirstName;
                            //clm.RenderingProvider = value;
                            //ClaimEditService.updateRenderingNPI(value.NPI);
                            break;
                        }
                        case 'Facilities': {
                            //clm.FacilityId = value.Id;
                            _this.ctx.Preferences.Facility = value;
                            _this.facilityName = value.Name;
                            break;
                        }
                        case 'PlacesOfService': {
                            //clm.PlaceOfService = value;
                            //clm.PlaceOfServiceId = value.Id;
                            _this.ctx.Preferences.PlaceOfService = value;
                            _this.posName = value.Name;
                            break;
                        }
                    }
                },
                function (value) {
                }
            );
        };

        _this.switchDB = function (init) {
            _this.connecting = true;
            console.log('ehrAccountController.switchDB');

            var acc;
            console.log(_this.accounts);
            console.log(_this.db);

            for (var i = 0, n = _this.accounts.length; i < n; i++) {
                var a = _this.accounts[i];
                if (a.Code == _this.db) {
                    acc = a;
                }
            }

            console.log(acc);
            AuthService.setAccount(acc.Name);
            _this.currentdb = acc.Name;

            $state.go('home');
            //$state.go('initialize');
        };

        _this.initAccounts = function () {
            var accounts = [];
            for (var i = 0, n = _this.ctx.Accounts.length; i < n; i++) {
                var a = _this.ctx.Accounts[i];
                accounts.push({
                    Id: a.Id,
                    Code: "HK_" + a.Name,
                    Name: a.Name
                });
            }

            _this.accounts = accounts;
            _this.currentdb = AuthService.getAccount();
            _this.db = AuthService.getAccount();

            console.log('_this.currentdb: ' + _this.currentdb);
            console.log('_this.db: ' + _this.db);
        };

        _this.initClaimDefaults = function () {

        };

        _this.init = function () {
            console.log('ehrAccountController.init');

            //_this.ctx = ctx.entityList;
            _this.ctx = AuthService.getCurrentUser();
            console.log(_this.ctx);

            _this.firstName = _this.ctx.FirstName;
            _this.initAccounts();
            _this.initClaimDefaults();
        };

        _this.init();
    }

    ehrInitializeController.$inject = ['$q', '$state', 'ApiService', 'AuthService'];
    function ehrInitializeController($q, $state, ApiService, AuthService) {
        var _this = this;

        _this.init = function () {
            console.log('*** ehrInitializeController ***');

            _this.currentUser = AuthService.getCurrentUser();

            // US States
            var allReqs = [];

            allReqs.push(
                ApiService.getApiList('usstates').then(function (response) {
                    var list = response.data.results;
                    ApiService.setList('usstates', {
                        selected: -1,
                        options: prepareList(list.Data)
                    })
                })
            );

            // Patient Relationships
            allReqs.push(
                ApiService.getApiList('patrels').then(function (response) {
                    var list = response.data.results;
                    ApiService.setList('patrels', {
                        selected: -1,
                        options: prepareList(list.Data)
                    });
                })
            );
            //appCtrl.self = false;

            // ICD Codes
            /*
            ApiService.getApiList('icdcodes').then(function (response) {
                var list = response.data.results;
                ApiService.setList('icd', {
                    selected: -1,
                    options: prepareList(list.Data)
                });
            });
            */

            // CPT Codes
            /*
            ApiService.getApiList('cptcodes').then(function (response) {
                var list = response.data.results;
                ApiService.setList('cpt', {
                    selected: -1,
                    options: prepareList(list.Data)
                });
            });
            */

            // Taxonomy Codes
            allReqs.push(
                ApiService.getApiList('taxonomy').then(function (response) {
                    var list = response.data.results.Data;
                    var codes = [];

                    for (var k in list) {
                        if (list.hasOwnProperty(k)) {
                            var l = list[k].split('|');
                            codes.push({
                                Id: k,
                                Code: l[0],
                                Classification: l[1],
                                Specialization: l[2]
                            });
                        }
                    }

                    ApiService.setList('taxonomy', codes);
                })
            );


            // Error Codes
            allReqs.push(
                ApiService.getApiList('errorcodes').then(function (response) {
                    var list = response.data.results.Data;
                    var codes = [];

                    for (var k in list) {
                        if (list.hasOwnProperty(k)) {
                            codes.push({
                                Id: k,
                                Description: list[k]
                            });
                        }
                    }

                    ApiService.setList('errorcodes', codes);
                })
            );

            // Payers
            //ApiService.getEntityList('Payers', 'payer').then(function (response) {

            //});

            // NDC?

            $q.all(allReqs).then(function () {
                ApiService.doneInitializing();

                //$state.go('home');
                if (AuthService.isConnected() == true) {
                    $state.go('home');
                }
                else {
                    $state.go('changedb');
                }
            });
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrAccountController', ehrAccountController)
        .controller('ehrInitializeController', ehrInitializeController);

    function prepareList(list) {
        var arr = [];
        for (var k in list) {
            if (list.hasOwnProperty(k)) {
                arr.push({ "Code": k, "Name": list[k] });
            }
        }

        return arr;
    }

})();

