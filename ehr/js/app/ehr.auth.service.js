(function () {
    'use strict';

    AuthService.$inject = ['$http', '$state'];
    function AuthService($http, $state) {
        var _this = this;

        _this.logout = function () {
            _this.ready = false;
            _this.currentUser = null;
            _this.setAccount(null);
        }

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

        _this.getAccountByName = function (name) {
            var account = null;
            var dbname = '';

            if (name) {
                dbname = name.indexOf(AppConfig.DBPrefix) == 0 ? name.substring(3) : name;

                for (var i = 0, n = _this.currentUser.Accounts.length; i < n; i++) {
                    var a = _this.currentUser.Accounts[i];

                    if (a.Name == dbname) {
                        account = a;
                    }
                }
            }

            console.log('geAccountByName: ' + name);
            console.log(account);
            return account;
        };

        _this.setAccount = function (db) {
            if (db) {
                _this.account = _this.getAccountByName(db);
            }
            else {
                if (_this.currentUser && _this.currentUser.DefaultAccount) {
                    _this.account = _this.currentUser.DefaultAccount;
                }
                else {
                    _this.account = null;
                }
            }
        };

        _this.getAccount = function () {
            return _this.account;
        };

        _this.getAccountName = function () {
            if (_this.account) {
                return AppConfig.DBPrefix + _this.account.Name;
            }
            else {
                return '';
            }
        };

        _this.saveAccount = function () {
            var url = AppConfig.ApiBase + 'updateAccount';
            console.log('AuthService saveAccount: ' + url);

            return $http.post(url, _this.account);
        };

        _this.setCurrentUser = function (user) {
            _this.currentUser = user;
            _this.setAccount();
        };

        _this.getCurrentUser = function () {
            return _this.currentUser;
        };

        _this.getUserPreferencesFromDb = function () {
            var url = AppConfig.ApiBase + 'getUserPrefs/' + _this.account.Id + '/' + _this.currentUser.Id;

            console.log('getUserPreferencesFromDb: ' + url);
            return $http.get(url);
        };

        _this.saveUserPreferences = function () {
            var url = AppConfig.ApiBase + 'updateUserPrefs/' + _this.currentUser.Id;

            console.log('saveUserPreferences: ' + url);
            return $http.post(url, _this.userPreferences);
        };

        _this.getUserPreferences = function () {
            return _this.userPreferences;
        };

        _this.setUserPreferences = function (pref) {
            if (pref) {
                if (pref.Id <= 0) {
                    pref.AccountId = _this.account.Id;
                    pref.UserId = _this.currentUser.Id;
                }
                _this.userPreferences = pref;
                console.log('AuthService.setUserPreferences');
                console.log(_this.userPreferences);
            }
        };

        _this.getPreference = function (key) {
            var pref = '';
            if (_this.userPreferences) {
                pref = _this.userPreferences[key];
            }

            return pref;
        };

        _this.setPreference = function (key, val) {
            if (_this.userPreferences) {
                _this.userPreferences[key] = val;
            }
        };

        _this.isLoggedIn = function () {
            return (_this.currentUser != null && _this.currentUser.Id > 0); 
        };

        _this.setIsReady = function () {
            _this.ready = true;
            console.log('_this.ready = ' + _this.ready);
        };

        _this.isReady = function () {
            console.log('isReady: ' + _this.ready);
            return _this.ready;
        };

        _this.isConnected = function () {
            if (_this.account && _this.account.Id > 0) {
                return true;
            }
            else {
                return false;
            }
        };

        _this.getCurrentUser = function () {
            return _this.currentUser;
        };

        _this.setDB = function (db) {
            _this.currentUser.Account = db;
            console.log('AuthService.setDB');
            console.log(_this.getCurrentUser());
        };

        _this.loginUser = function (un, pw) {
            var url = AppConfig.ApiBase + 'loginUser';
            console.log('AuthService login: ' + un + ', ' + pw);

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


        /*
        _this.setAccountPreference = function (key, val) {
            account.AccountPreference[key] = val;
            var account = _this.account;
            if (account) {
                account.AccountPreference[key] = val;
                switch (key) {
                    case 'BillingProvider':
                        account.AccountPreference.BillingProviderId = val.Id;
                        break;
                    case 'RenderingProvider':
                        account.AccountPreference.RenderingProviderId = val.Id;
                        break;
                    case 'Facility':
                        account.AccountPreference.FacilityId = val.Id;
                        break;
                    case 'PlaceOfService':
                        account.AccountPreference.PlaceOfServiceId = val.Id;
                        break;
                }
            }

            console.log('setAccountPreference: ' + key);
            console.log(_this.account);
        };

        _this.getAccountPreference = function (key) {
            var account = _this.account;
            var val = '';

            if (account) {
                val = account.AccountPreference[key];
            }

            console.log('getAccountPreference: ' + key);
            console.log(account);
            console.log(val);

            return val;
        };
        */
