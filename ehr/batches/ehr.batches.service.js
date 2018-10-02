(function () {
    'use strict';

    BatchEditService.$inject = ['$stateParams', 'ApiService', 'AuthService'];
    function BatchEditService($stateParams, ApiService, AuthService) {
        var _this = this;
        var db = AuthService.getAccount();
        var batch = null;
        var claims = [];
        var claimsLoaded = false;
        var searchResults = {};
        var searchResultsLoaded = false;

        _this.resetSearchParams = function () {
            searchResults = {};
            //searchResultsLoaded = false;
        };

        _this.resetBatchParams = function () {
            console.log('resetBatchParams');
            claims = [];
            claimsLoaded = false;
            db = AuthService.getAccount();

            _this.resetSearchParams();
        };

        _this.getBatch = function () {
            return batch;
        };

        _this.setBatch = function (b) {
            console.log('setBatch');
            console.log(batch);

            if (batch) {
                if (batch.Id != b.Id) {
                    _this.resetBatchParams();
                }
                /*
                if (db != AuthService.getAccount()) {
                }
                else if (_this.batch.Id != b.Id) {
                    _this.resetBatchParams();
                }
                */
            } else {
                _this.resetBatchParams();
            }

            batch = b;
        };

        _this.getBatchClaims = function () {
            return claims;
            /*
            if (formatted) {
                return _this.formatClaimSearchResults(claims);
            }
            else {
            }
            */
        };

        _this.setBatchClaims = function (list) {
            claims = list;
            console.log('setBatchClaims');
            console.log(claims);
        };

        /*
        _this.getBatchClaims = function () {
            return ApiService.getBatchClaims(_this.batch.Id);
        };
        */

        /*
        _this.setClaims = function (batchClaims) {
            console.log('setClaims');
            console.log(batchClaims);
            claims = batchClaims;
            claimsLoaded = true;
        };

        _this.getClaims = function () {
            return claims;
        };
        */

        _this.getClaimsLoaded = function () {
            console.log('getClaimsLoaded: ' + claimsLoaded);
            return claimsLoaded;
        };

        _this.searchClaims = function (parms) {
            return ApiService.searchClaimsForBatch(parms);
        };

        // TO BE DELETED
        /*
        _this.formatClaimSearchResults = function (list) {
            var claims = [];

            for (var i = 0, n = list.length; i < n; i++) {
                var c = list[i];
                claims.push({
                    id: c.Id,
                    account: UIService.padLeft(c.Id, 5, '0'),
                    dateOfService: c.DateOfService,
                    subscriberName: c.SubscriberName,
                    payerName: c.PrimaryPayerName,
                    memberId: c.PrimaryMemberId,
                    diagnosis: c.Diagnosis,
                    amount: c.AmountTotal,
                    providerName: c.Provider,
                    groupName: c.Group,
                    facilityName: c.Facility
                });
            }
            //_this.setClaims(claims);

            return claims;
        };
        */

        _this.setSearchResults = function (results) {
            searchResults[db] = results;
            //searchResultsLoaded = true;
        };

        _this.getSearchResults = function () {
            return searchResults[db];
        };

        /*
        _this.getSearchResultsLoaded = function () {
            console.log('getSearchResultsLoaded');
            console.log('db: ' + db + ', udb: ' + AuthService.getCurrentUser().Database);
            return searchResultsLoaded;
        }
        */

        _this.formatClaimIDs = function () {
            var arr = [];

            for (var i = 0, n = claims.length; i < n; i++) {
                arr.push(claims[i].Id);
            }

            batch.ClaimIDs = arr.join(',');
            console.log(batch);
        };

        _this.addClaimToBatch = function (index) {
            var cl = searchResults[db][index];
            console.log(cl);
            console.log('BatchEditService.addClaimToBatch: ' + cl.Id);

            var ids = batch.ClaimIDs ?
                batch.ClaimIDs.split(',') : [];

            console.log(ids);

            if (ids.includes(cl.Id + "") == true) {
                alert('This claim is already inluded in the batch.');
            }
            else {
                ids.push(cl.Id);
                console.log(ids);
                //batch.ClaimIDs = ids.join(',');
                //console.log(batch.ClaimIDs);

                searchResults[db].splice(index, 1);
                claims.push(cl);

                _this.formatClaimIDs();

                console.log(searchResults[db]);
            }
        };

        _this.removeClaimFromBatch = function (index) {
            var cl = claims[index];
            console.log(cl);
            console.log('BatchEditService.removeClaimFromBatch: ' + cl.Id);

            claims.splice(index, 1);
            _this.formatClaimIDs();
        };

        // PENDING IMPLEM
        _this.hideBatchClaimsFromSearch = function () {

        }
    }

    angular.module('ehrApp').service('BatchEditService', BatchEditService);
})();

