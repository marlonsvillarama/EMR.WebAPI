(function () {
    'use strict';

    SetupService.$inject = ['$state', 'ApiService'];
    function SetupService($state, ApiService) {
        var _this = this;
        _this.currentIds = {};
        _this.lists = {};

        _this.updateEntity = function (parms) {
            return ApiService.updateEntity({
                type: parms.type,
                entity: parms.entity
            });
        }

        _this.search = function (p, s) {
            return ApiService.getEntityList(p, s);
        };

        _this.setList = function (type, list) {
            _this.lists[type] = list;
        };

        _this.getList = function (type) {
            return _this.lists[type];
        };

        _this.setCurrentId = function (type, id) {
            _this.currentIds[type] = id;
        };

        _this.getCurrentId = function (type) {
            return _this.currentIds[type];
        };

    }

    angular.module('ehrApp').service('SetupService', SetupService);
})();
