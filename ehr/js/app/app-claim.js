/*
 * JS File for Claims views
 */

(function () {
    'use strict';

    var url;
    var claimsCtrl = angular.module('ehrApp')
        .controller('ehrClaimsController', ehrClaimsController)
        .config(claimStateConfig)
        .service('ApiService', ApiService);
    //.config(RouterConfig)

    ehrClaimsController.$inject = ['$scope', 'ApiService'];
    function ehrClaimsController($scope, $http, ApiService) {
        var claimsCtrl = this;
        var list;
        /*ApiService.getList('usstates').then(function (response) {
            appCtrl.usStates = response.data.results;
            UIService.log(appCtrl.usStates);
        });
        ApiService.getList('patrels').then(function (response) {
            UIService.log(response.data.results);
            var list = response.data.results;
            //UIService.log(prepareList(list.Data));
            appCtrl.patientRels = prepareList(list.Data);
            //UIService.log(appCtrl.patientRels);
        });*/
    }

    function prepareList(list) {
        var arr = [];
        for (var k in list) {
            if (list.hasOwnProperty(k)) {
                arr.push({ "Code": k, "Name": list[k] });
            }
        }

        return arr;
    }

    claimStateConfig.$inject = ['$stateProvider'];
    function claimStateConfig($stateProvider) {
        var arrClaimStates = [];
        var arrClaimSubtabs = [
            'diagnosis',
            'subscriber',
            'payers',
            'provider',
            'facility',
            'other'
        ];

        for (var i = 0, n = arrClaimSubtabs.length; i < n; i++) {
            $stateProvider.state('claims-edit.' + arrClaimSubtabs[i], {
                //url: '/' + arrClaimSubtabs[i] + '/{claimId}',
                templateUrl: '/ehr/claims/claims-edit-' + arrClaimSubtabs[i] + '.html'
            });
        }
    }

})();

