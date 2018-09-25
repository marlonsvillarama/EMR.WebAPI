(function () {
    'use strict';

    ehrLoginController.$inject = ['$state', 'AuthService'];
    function ehrLoginController($state, AuthService) {
        var _this = this;
        
        _this.submitForm = function () {
            _this.loggingIn = true;

            AuthService.loginUser(_this.username, _this.password).then(function (response) {
                console.log('AuthService.loginUser');
                console.log(response);
                var resSuccess = response.data.results.IsSuccess;
                var resData = response.data.results.Data;

                if (resSuccess == true) {
                    _this.loginError = false;

                    AuthService.setCurrentUser(resData);
                    console.log(AuthService.getCurrentUser());

                    $state.go('initialize');
                }
                else {
                    _this.loggingIn = false;
                    _this.loginError = true;
                }

                console.log(resData);
            });
        };

        _this.init = function () {
            console.log('fire loginController');
        };

        _this.init();
    }

    ehrLogoutController.$inject = ['$state', 'AuthService'];
    function ehrLogoutController($state, AuthService) {
        var _this = this;

        _this.init = function () {
            AuthService.logout();
            $state.go('login');
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrLoginController', ehrLoginController)
        .controller('ehrLogoutController', ehrLogoutController);
})();


