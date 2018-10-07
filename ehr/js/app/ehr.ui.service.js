(function () {
    'use strict';

    UIService.$inject = ['$stateParams', 'AuthService'];
    function UIService($stateParams, AuthService) {
        var service = this;

        service.log = function (obj) {
            if (AppConfig.AllowLogging == true) {
                console.log(obj);
            }
        };

        // TO BE DELETED
        service.applyMasks = function () {
            console.log('applyMasks');
        };

        service.applyDatePickers = function () {
            console.log('applyDatePickers');
            jQuery('[data-toggle="datepicker"]').datepicker({
                autoHide: true
            });
        };

        service.calculateAge = function (dt) {
            return ~~((Date.now() - dt) / (31557600000));
        };

        service.getDateParam = function (dt) {
            var dt = new Date(dt);
            return service.padLeft(dt.getDate(), 2, '0') +
                service.padLeft((dt.getMonth() + 1), 2, '0') +
                dt.getFullYear();
        };

        service.parseDate = function (str) {
            var dt = '';
            var output = '';

            dt = str ? new Date(str) : new Date();

            output = (dt.getMonth() + 1) + '/' +
                dt.getDate() + '/' +
                dt.getFullYear();

            return output;
        };

        service.getGender = function (str) {
            if (!str) {
                return "";
            }

            return (str.toUpperCase() == 'M' ? 'Male' : 'Female');
        };

        service.padLeft = function (str, length, ch) {
            if (!str) {
                return "";
            }

            if (!length || length <= 0) {
                return "";
            }

            var s = '';
            for (var i = 0; i < length; i++) {
                s += ch;
            }

            return (s + str).slice(-length);
        };

        service.formatAcctNumber = function (id) {
            var str = '';

            if (id > 0) {
                str = AuthService.getAccountName().substring(3, 6).toUpperCase() +
                    '-' + service.padLeft(id, 6, '0');
            }
            else {
                str = '< New >';
            }

            return str;
        }

        service.getStatusText = function (status) {
            var str = '';
            switch (status) {
                case 'P': {
                    str = 'Processed';
                    break;
                }
                case 'S': {
                    str = 'Submitted';
                    break;
                }
                default: {
                    str = 'Unprocessed';
                    break;
                }
            }
            return str;
        };

        service.getEditPageTitle = function (type, id) {
            var title = '';

            if (id && id > 0) {
                title = "EDIT " + type.toUpperCase() + ' - ' + service.formatAcctNumber(id);
            }
            else {
                title = "CREATE " + type.toUpperCase();
            }

            return title;
        }

    }

    angular.module('ehrApp').service('UIService', UIService);

})();


