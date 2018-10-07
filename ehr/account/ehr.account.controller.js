(function () {
    'use strict';

    ehrAccountController.$inject = ['$state', '$q', 'ngDialog', 'ApiService', 'AuthService'];
    function ehrAccountController($state, $q, ngDialog, ApiService, AuthService) {
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
                            AuthService.setPreference('BillingProvider', value);
                            AuthService.setPreference('BillingProviderId', value.Id);
                            _this.billingProvName = value.LastName;
                            break;
                        }
                        case 'Providers': {
                            AuthService.setPreference('RenderingProvider', value);
                            AuthService.setPreference('RenderingProviderId', value.Id);
                            _this.renderingProvFullName = value.LastName + ", " + value.FirstName;
                            break;
                        }
                        case 'Facilities': {
                            AuthService.setPreference('Facility', value);
                            AuthService.setPreference('FacilityId', value.Id);
                            _this.facilityName = value.Name;
                            break;
                        }
                        case 'PlacesOfService': {
                            AuthService.setPreference('PlaceOfService', value);
                            AuthService.setPreference('PlaceOfServiceId', value.Id);
                            _this.posName = value.Name;
                            break;
                        }
                    }
                },
                function (value) {
                }
            );
        };

        _this.saveClaimDefaults = function () {
            _this.savingPrefs = true;
            AuthService.saveUserPreferences().then(function (response) {
                console.log(response);
                var res = response.data.results;
                if (res.IsSuccess) {
                    AuthService.setUserPreferences(res.Data);
                    _this.initPreferences();
                    _this.savingPrefs = false;
                }
            });
            _this.savingPrefs = false;
        };

        _this.switchDB = function (init) {
            _this.connecting = true;
            _this.savingPrefs = false;

            console.log('ehrAccountController.switchDB');
            console.log(_this.accounts);
            console.log(_this.db);

            //acc = AuthService.getAccountByName(_this.db);
            //console.log(acc);

            AuthService.setAccount(_this.db);
            console.log(AuthService.getAccount());

            _this.initPreferences(init);

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
            _this.db = AuthService.getAccountName();

            console.log('_this.currentdb: ' + _this.currentdb);
            console.log('_this.db: ' + _this.db);
        };

        _this.showResponse = function (res, type, fld) {
            var result = res;
            
            if (res) {
                if (res.data) {
                    result = res.data.result.Data;
                    AuthService.setPreference(type, result);
                }
            }

            switch (fld) {
                case 'billingProvName':
                    _this[fld] = result ? result.LastName : "";
                    break;  
                case 'renderingProvFullName':
                    _this[fld] = result ? result.LastName + ', ' + result.FirstName : "";
                    break;
                default: 
                    _this[fld] = result ? result.Name : "";
                    break;
            }
        };

        _this.buildClaimDefaultReq = function (key) {
            var pref;
            var res;

            pref = AuthService.getPreference(key + 'Id');

            if (pref) {
                console.log('getClaimDefault: key=' + key + ', pref=' + pref);
                switch (key) {
                    case 'BillingProvider':
                    case 'RenderingProvider': {
                        res = ApiService.getEntity('Provider', pref);
                        break;
                    }
                    default: {
                        res = ApiService.getEntity(key, pref);
                        break;
                    }
                }

                return res;
            }
            else {
                return null;
            }
        };

        _this.getClaimDefault = function (arr, type, fld) {
            var r, pref;
            console.log('getClaimDefault(' + type + ', ' + fld + ')');

            pref = AuthService.getPreference(type);

            if (pref) {
                _this.showResponse(pref, type, fld);
            }
            else {
                r = _this.buildClaimDefaultReq(type);
                if (r) {
                    arr.push(
                        r.then(function (response) {
                            _this.showResponse(response, type, fld);
                        })
                    );
                }
                else {
                    _this.showResponse(null, type, fld);
                }
            }
        };

        _this.buildClaimDefaults = function () {
            var reqs = [];

            _this.getClaimDefault(reqs, 'BillingProvider', 'billingProvName');
            _this.getClaimDefault(reqs, 'RenderingProvider', 'renderingProvFullName');
            _this.getClaimDefault(reqs, 'Facility', 'facilityName');
            _this.getClaimDefault(reqs, 'PlaceOfService', 'posName');

            $q.all(reqs).then(function (response) {
                console.log('done buildClaimDefaults');
                console.log(AuthService.getUserPreferences());
                _this.connecting = false;
                _this.savingPrefs = false;
            });
        };

        // DELETE
        _this.showUserPreferences = function () {
        };

        _this.initPreferences = function (init) {
            var reqs = [];
            var r, pref, acc;
            _this.connecting = true;
            console.log('initPreferences(' + init + ')');

            pref = AuthService.getUserPreferences();
            console.log(pref);

            acc = AuthService.getAccount();
            console.log(acc);

            if (init || !pref || pref.AccountId != acc.Id) {
                AuthService.getUserPreferencesFromDb().then(function (response) {
                    var isSuccess = response.data.result.IsSuccess;

                    if (isSuccess) {
                        console.log(response.data.result.Data);

                        AuthService.setUserPreferences(response.data.result.Data);
                        _this.buildClaimDefaults();

                        if (init) {
                            AuthService.setIsReady();
                            $state.go('home');
                        }
                    }
                });
            }
            else {
                _this.buildClaimDefaults();
            }
        };

        _this.init = function () {
            console.log('ehrAccountController.init');

            //_this.ctx = ctx.entityList;
            _this.ctx = AuthService.getCurrentUser();
            console.log(_this.ctx);

            _this.firstName = _this.ctx.FirstName;
            _this.initAccounts();

            if (AuthService.isConnected() == true) {
                _this.initPreferences();
            }

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

