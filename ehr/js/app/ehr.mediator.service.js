(function () {
    'use strict';

    MediatorService.$inject = [];
    function MediatorService() {
        var _this = this;

        _this.defaults = {};
    }

    angular.module('ehrApp').service('MediatorService', MediatorService);

})();


