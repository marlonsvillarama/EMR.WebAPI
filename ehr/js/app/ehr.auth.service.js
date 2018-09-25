(function () {
    'use strict';

    AuthService.$inject = ['$http', '$state'];
    function AuthService($http, $state) {
        var _this = this;

        _this.logout = function () {
            _this.currentUser = null;
            _this.setAccount(null);
        }

        _this.setAccount = function (db) {
            if (db) {
                if (db.indexOf('HK_') < 0) {
                    db = 'HK_' + db;
                }
                _this.db = db;
            }
            else {
                if (_this.currentUser && _this.currentUser.DefaultAccount) {
                    _this.db = _this.currentUser.DefaultAccount.Name;
                }
                else {
                    _this.db = null;
                }
            }
            console.log('setAccount: ' + _this.db);
        };

        _this.getAccount = function () {
            return _this.db;
        };

        // DELETE
        _this.getAccountById = function (id) {
            var account = null;

            for (var i = 0, n = _this.currentUser.Accounts.length; i < n; i++) {
                var a = _this.currentUser.Accounts[i];
                if (a.Id == id) {
                    account = a;
                }
            }

            console.log('geAccountById: ' + id);
            console.log(account);
            return account;
        };

        _this.setCurrentUser = function (user) {
            _this.currentUser = user;
            _this.setAccount();
        };

        _this.getCurrentUser = function () {
            return _this.currentUser;
        };

        _this.isLoggedIn = function () {
            return (_this.currentUser != null && _this.currentUser.Id > 0); 
        };

        _this.isConnected = function () {
            console.log('isConnected: ' + _this.db);
            if (_this.db) {
                return true;
            }
            else {
                return false;
            }
        };

        /*
        _this.currentUser = {
            Id: 1,
            FirstName: "Louis",
            LastName: "Yson",
            Database: "HK_Kogan",
            DefaultAccount: {
                Id: 2,
                Name: "HK_Kogan"
            }
        };
        */

        _this.getCurrentUser = function () {
            return _this.currentUser;
        };

        _this.setDB = function (db) {
            _this.currentUser.Account = db;
            console.log('AuthService.setDB');
            console.log(_this.getCurrentUser());
            //$state.go('home');
        };

        _this.loginUser = function (un, pw) {
            var url = AppConfig.ApiBase + 'loginUser';
            console.log('ApiService login: ' + un + ', ' + pw);

            return $http.post(url,
                {
                    UserName: un,
                    Password: pw
                },
                {
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        };

    }

    angular.module('ehrApp').service('AuthService', AuthService);
})();

