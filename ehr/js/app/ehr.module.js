/*
 * EHR Main JS File
 */

(function () {
    'use strict';

    var ehrApp = angular.module('ehrApp', ['ui.router', 'ngDialog']);

    ehrApp.config(["ngDialogProvider", function (ngDialogProvider) {
        ngDialogProvider.setDefaults({
            className: "ngdialog-theme-default",
            plain: false,
            showClose: true,
            closeByDocument: true,
            closeByEscape: true,
            appendTo: false,
            preCloseCallback: function () {
                console.log("default pre-close callback");
            }
        });
    }]);

    /*
    angular.module('ehrApp')
        .controller('ehrAppController', ehrAppController)
        .config(appStateConfig)
        .service('ApiService', ApiService);
        //.config(RouterConfig)
    */

    function changeSelected() {
        console.log('changed!');
    }

    // PENDING IMPLEM
    GetListCtrl.$inject = ['ApiService'];
    function GetListCtrl(ApiService) {
        var result = ApiService.getList()
    }

    GetEntityCtrl.$inject = ['ApiService'];
    function GetEntityCtrl(ApiService) {

    }

    UpdateEntityCtrl.$inject = ['ApiService'];
    function UpdateEntityCtrl(ApiService) {

    }

    // CURRENTLY USED BUT PENDING DEPRECATION
    function buildStates(p, s) {
        var states = [];
        states.push({
            name: p + '-list',
            url: '/' + p,
            templateUrl: '/ehr/' + p + '/' + p + '-list.html',
            controller: function ($scope, $http) {
                url = AppConfig.ApiBase + 'get' + p;
                $http.get(url).then(function (response) {
                    var res = response.data.results;
                    if (res.IsSuccess == true) {
                        $scope.entityList = res.Data;
                    }
                    else {
                        alert(res.Data.toString());
                    }
                });
            }
        });
        /*
        */
        /*
        states.push({
            name: p + '-get',
            url: '/' + p + '/get/:id',
            templateUrl: '/ehr/' + p + '/' + p + '-get.html',
            controller: function ($scope, $http, $stateParams) {
                url = AppConfig.ApiBase + 'get' + s + 'ById/' + $stateParams.id;
                $http.get(url).then(function (response) {
                    var res = response.data.result;
                    if (res.IsSuccess == true) {
                        $scope.entity = res.Data;
                    }
                    else {
                        alert(res.Data.toString());
                    }
                });
            }
        });
        */
        states.push({
            name: p + '-edit',
            url: '/' + p + '/edit/:id',
            templateUrl: '/ehr/' + p + '/' + p + '-edit.html',
            controller: 'ehrClaimEditController as claimEditCtrl'
        })
        return states;
    }
})();

