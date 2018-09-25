(function () {
    'use strict';

    angular.module('ehrApp')
        .component('loading', {
            template: '<img src="/ehr/images/loading_1.gif" ng-if="$ctrl.show">',
            controller: loadingController
        });

    loadingController.$inject = ['$rootScope'];
    function loadingController($rootScope) {
        var $ctrl = this;
        var listener;

        $ctrl.onInit = function () {
            $ctrl.show();
            listener = $rootScope.$on('spinner:active', onSpinnerActivate);
        };

        $ctrl.$onDestroy = function () {
            listener();
        };

        function onSpinnerActivate(event, data) {
            $ctrl.show = data.on;
        }
    }
})();