(function () {
    'use strict';

    ApiService.$inject = ['$http', 'AuthService'];
    function ApiService($http, AuthService) {
        var service = this;
        var url;
        var result = {};
        var lists = {};
        var exList = ['Payers', 'PlacesOfService', 'Users'];
        var exEntity = ['Payer', 'PlaceOfService', 'User'];

        service.doneInitializing = function () {
            service.initialized = true;
        };

        service.isInitialized = function () {
            return service.initialized;
        };

        service.getCurrentUser = function () {
            return AuthService.getCurrentUser();
        };

        service.searchEntities = function (p, parms) {
            url = AppConfig.ApiBase + 'search' + p + '/' +
                AuthService.getAccountName() + '/' + parms;

            console.log('ApiService searchEntities' + url);
            return $http.get(url);
        };

        service.getEntityList = function (p, s) {
            url = AppConfig.ApiBase + 'get' + p;
            if (exList.indexOf(p) < 0)  {
                url += '/' + AuthService.getAccountName();
            }

            console.log('ApiService getEntityList: ' + url);
            return $http.get(url);
        };

        service.getEntity = function (s, id) {
            if (id) {
                url = AppConfig.ApiBase + 'get' + s + 'ById/';
                if (exEntity.indexOf(s) < 0) {
                    url += AuthService.getAccountName() + '/';
                }
                url += id;
            }
            else {
                url = AppConfig.ApiBase + 'getNew' + s;
            }

            console.log('ApiService getEntity: ' + url);
            return $http.get(url);
        };

        service.getPayment = function (claimId) {
            url = AppConfig.ApiBase + 'getPaymentByClaimId/' +
                AuthService.getAccountName() + '/' + claimId;

            console.log('ApiService getPayment: ' + url);
            return $http.get(url);
        };

        service.updateEntity = function (parms) {
            var url = AppConfig.ApiBase + 'update' + parms.type + '/' + AuthService.getAccountName();
            var req = null;

            if (parms.type == 'Payment') {
                url += '/' + parms.refId;
            }
            console.log('updateEntity: ' + url);

            return $http.post(url, parms.entity,
                {
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        };

        service.getApiList = function (type) {
            url = AppConfig.ListBase + type;
            var response = $http({
                method: "GET",
                url: url
            });
            return response;
        };

        service.setList = function (type, obj) {
            lists[type] = obj;
            console.log('ApiService.setList(' + type + ')');
            console.log(lists[type]);
        };

        service.getList = function (type) {
            return lists[type];
        };

        service.getRelationshipByCode = function (code) {
            var relFil = lists['patrels'].options.filter(x => x.Code == code);
            var rel = {};

            if (relFil.length > 0) {
                rel = relFil[0];
            }
            else {
                rel = { Code: "18", Name: "Self" };
            }

            return rel;
        };

        service.getClaimHistory = function (id) {
            url = AppConfig.ApiBase + 'getClaimsBySubscriberId/' + AuthService.getAccountName() + '/' + id;
            console.log('ApiService getClaimHistory: ' + url);
            return $http.get(url);
        };

        service.getDependentsList = function (id) {
            url = AppConfig.ApiBase + 'getPatientsBySubscriberId/' + AuthService.getAccountName() + '/' + id;
            console.log('ApiService getDependentsList: ' + url);
            return $http.get(url);
        };

        service.updateSubDependent = function (parms) {
            url = AppConfig.ApiBase + 'updateSubscriberDependent/' + AuthService.getAccountName() + '/' + parms.subId;

            var pat = {
                SubscriberId: parms.subId,
                PatientId: parms.patient.PatientId,
                FirstName: parms.patient.FirstName,
                LastName: parms.patient.LastName,
                DateOfBirth: parms.patient.DateOfBirth,
                Gender: parms.patient.Gender,
                Relationship: parms.patient.Relationship
            };

            return $http.post(url,
                pat,
                {
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        };

        service.prepareResponse = function (response) {
            var res = response.data.results;
            var output;

            if (response.data.results != null) {
                res = response.data.results;
            }
            else if (response.data.result != null) {
                res = response.data.result;
            }
            else {
                return output = {
                    success: false,
                    message: 'Unparseable object'
                };
            }

            if (res.IsSuccess == true) {
                output = {
                    entityList: res.Data,
                    success: true
                };
            }
            else {
                output = {
                    success: false,
                    message: res.Data.toString()
                };
            }

            console.log(res.Data);
            return output;
        }

        service.printReport = function (type, batchId, claimId) {
            url = AppConfig.ApiBase + 'report/' + AuthService.getAccountName() + '/' +
                type + '|' +
                (batchId ? batchId : "") + '|' +
                (claimId ? claimId : "");

            console.log('printCMS: ' + url);
            window.open(url);
        };

        service.getBatchClaims = function (id) {
            url = AppConfig.ApiBase + 'getBatchClaims/' + AuthService.getAccountName() + '/' + id;
            console.log('ApiService getBatchClaims: ' + url);
            return $http.get(url);
        };

        /*
        service.searchClaimsForBatch = function (parms) {
            url = AppConfig.ApiBase + 'searchClaims/' + AuthService.getAccountName() + '/' + parms;
            console.log('ApiService searchClaimsForBatch: ' + url);
            return $http.get(url);
        };
        */
    }

    angular.module('ehrApp').service('ApiService', ApiService);
})();

