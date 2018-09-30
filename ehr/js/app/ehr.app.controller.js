(function () {
	'use strict';

	ehrAppController.$inject = ['$http', '$state', 'ApiService', 'AuthService'];
    function ehrAppController($http, $state, ApiService, AuthService) {
		var _this = this;
        var list;
        console.log('*** ehrAppController ***')

        _this.isLoggedIn = function () {
            return AuthService.isLoggedIn();
        };

        _this.isConnected = function () {
            return AuthService.isConnected();
        };

        _this.isReady = function () {
            return AuthService.isReady();
        };

        _this.isInitialized = function () {
            return ApiService.isInitialized();
        };

        _this.getUserName = function () {
            var user = AuthService.getCurrentUser();
            if (user) {
                return user.FirstName + " " + user.LastName;
            }
        };

        _this.logout = function () {
            AuthService.setCurrentUser(null);
        };

        _this.init = function () {
            console.log('ehrAppController.init')

            if (AuthService.isLoggedIn() == false) {
                $state.go('login');
            }
            else {
                var db = '';
                try {
                    db = AuthService.getDatabase();
                }
                catch (ex) {
                    console.log(ex);
                }

                if (!db) {
                    $state.go('changedb');
                }
                else {
                    $state.go('initialize');
                }
            }
        };

        /*
        appCtrl.switchDB = function (db) {
            AuthService.setDB(db);
        };
        */
        _this.init();
	}

    ehrHomeController.$inject = ['$state', 'ApiService', 'AuthService'];
    function ehrHomeController($state, ApiService, AuthService) {
        var _this = this;

        _this.init = function () {
        };

        _this.init();
    };

    angular.module('ehrApp')
        .controller('ehrAppController', ehrAppController)
        .controller('ehrHomeController', ehrHomeController);

})();