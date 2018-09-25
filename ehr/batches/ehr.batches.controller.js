(function () {
    'use strict';

    ehrBatchListController.$inject = ['entityList', 'UIService'];
    function ehrBatchListController(entityList, UIService) {
        var _this = this;

        _this.init = function () {
            var entities = entityList.entityList;
            var list = [];
            console.log('fire ehrBatchListController');

            for (var i = 0, n = entities.length; i < n; i++) {
                var ids = entities[i].ClaimIds.split(',');
                var status = '';

                switch (entities[i].Status) {
                    case 'S': {
                        status: 'Submitted';
                        break;
                    }
                    default: {
                        status = 'Unprocessed';
                        break;
                    }
                }

                list.push({
                    id: entities[i].Id,
                    identifier: entities[i].Identifier,
                    dateCreated: entities[i].DateCreated,
                    timeCreated: entities[i].TimeCreated,
                    createdBy: entities[i].CreatedBy,
                    numClaims: ids.length,
                    status: status
                })
            }

            _this.batches = list;
            console.log(_this.batches);
        }

        _this.init();
    }

    angular.module('ehrApp').controller('ehrBatchListController', ehrBatchListController);

    ehrBatchEditController.$inject = ['$state', 'entity', 'claims', 'ApiService', 'BatchEditService', 'AuthService', 'UIService'];
    function ehrBatchEditController($state, entity, claims, ApiService, BatchEditService, AuthService, UIService) {
        var _this = this;
        _this.errors = [];

        _this.validateForm = function () {
            console.log('validateForm');
            _this.errors = [];

            if (!_this.batch.ClaimIDs) {
                _this.errors.push('You need to select at least one claim for this batch');
            }

        };

        _this.saveBatch = function (batch) {
            console.log('saveBatch');
            console.log(batch);
            _this.savingForm = true;

            batch.CreatedById = AuthService.getCurrentUser().Id;

            ApiService.updateEntity({
                type: 'Batch',
                entity: batch
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;
                console.log(resData);

                _this.savingForm = false;

                if (resSuccess == true) {
                    if (batch.Id > 0) {
                        alert('Changes saved!');
                    }
                    else {
                        alert('Batch successfully created!');
                    }

                    if (resData != batch.Id) {
                        $state.go('batch-edit', {
                            id: resData
                        });
                    }
                }
                else {
                    console.log('exception: ' + resData.toString());
                }
            });
        };

        _this.submitForm = function () {
            console.log('submitForm');
            if (_this.errors.length > 0) {
                var str = 'Please correct the following errors first:\n\n';
                for (var i = 0, n = _this.errors.length; i < n; i++) {
                    str += '- ' + _this.errors[i] + '\n';
                }
                alert(str);

                _this.savingForm = false;
                return;
            }

            _this.saveBatch(_this.batch);
        };

        _this.cancelEdit = function () {
            if (confirm('Any unsaved changes will be lost. Are you sure?')) {
                $state.go('batches');
            }
        };

        _this.printReport = function (type) {
            ApiService.printReport(type, _this.batch.Id).then(function (response) {
                console.log(response);
            });
        };

        _this.init = function () {
            console.log('fire ehrBatchEditController');

            BatchEditService.setBatch(entity.entityList);
            _this.batch = BatchEditService.getBatch();
            console.log(_this.batch);

            BatchEditService.setBatchClaims(claims.entityList);

            _this.page_title = UIService.getEditPageTitle('batch', _this.batch.Id);
            _this.savingForm = false;

            // Render the first subtab
            $state.go('batch-edit.lines');
        };

        _this.init();
    }

    ehrBatchHeaderController.$inject = ['$state', 'BatchEditService', 'UIService', 'AuthService'];
    function ehrBatchHeaderController($state, BatchEditService, UIService, AuthService) {
        var _this = this;

        _this.init = function () {
            _this.batch = BatchEditService.getBatch();
            console.log('fire ehrBatchHeaderController');
            console.log(_this.batch);

            if (_this.batch.Id > 0) {
                if (_this.batch.CreatedBy) {
                    _this.createdBy = _this.batch.CreatedBy.LastName + ", " +
                        _this.batch.CreatedBy.FirstName;
                }
                _this.batchIdentifier = _this.batch.Identifier;
            }
            else {
                var user = AuthService.getCurrentUser();
                _this.createdBy = user.LastName + ", " + user.FirstName;
                _this.batchIdentifier = "< New >";
            }

            _this.dateCreated = UIService.parseDate(_this.batch.DateCreated);
            _this.batchStatus = UIService.getStatusText(_this.batch.Status);
        };

        _this.init();
    }

    batchSubtabConfig.$inject = ['$stateProvider'];
    function batchSubtabConfig($stateProvider) {
        console.log('fire batchSubtabConfig');

        $stateProvider.state('batch-edit.lines', {
            templateUrl: '/ehr/batches/batch-edit-lines.html',
            controller: 'ehrBatchLinesController as bLineCtrl'
        });

        $stateProvider.state('batch-edit.search', {
            templateUrl: '/ehr/batches/batch-edit-search.html',
            controller: 'ehrBatchSearchController as bSearchCtrl'
        });
    
    }

    ehrBatchLinesController.$inject = ['BatchEditService', 'UIService'];
    function ehrBatchLinesController(BatchEditService, UIService) {
        var _this = this;

        _this.removeClaim = function (index) {
            console.log('removeClaim: ' + index);
            BatchEditService.removeClaimFromBatch(index);
        };

        _this.batchIsProcessed = function () {
            return (_this.batch.Status == 'P' || _this.batch.Status == 'S');
        };

        _this.getAcctNum = function (id) {
            return UIService.formatAcctNumber(id);
        };

        _this.init = function () {
            console.log('fire ehrBatchLinesController');
            _this.batch = BatchEditService.getBatch();
            _this.claims = BatchEditService.getBatchClaims();
        };

        _this.init();
    }

    ehrBatchSearchController.$inject = ['ApiService', 'BatchEditService', 'UIService'];
    function ehrBatchSearchController(ApiService, BatchEditService, UIService) {
        var _this = this;

        _this.getAcctNum = function (id) {
            return UIService.formatAcctNumber(id);
        };

        _this.searchClaims = function () {
            var parms = (_this.firstName ? _this.firstName : "") + '|' +
                (_this.lastName ? _this.lastName : "") + '|';// +

            console.log('searchClaims: ' + parms);
            _this.searching = true;

            BatchEditService.searchClaims(parms).then(function (response) {
                var claims = [];

                if (response.data.result.IsSuccess == true) {
                    var list = response.data.result.Data;
                    var batchClaims = BatchEditService.getBatchClaims();

                    for (var i = (list.length - 1), n = 0; i >= n; i--) {
                        for (var k = 0, o = batchClaims.length; k < o; k++) {
                            if (list[i].Id == batchClaims[k].Id) {
                                list.splice(i, 1);
                            }
                        }
                    }

                    claims = list;
                }

                BatchEditService.setSearchResults(claims);
                console.log('done searchClaims');

                _this.searchResults = claims;
                console.log(_this.searchResults);

                _this.searching = false;
            });
        }

        _this.addClaimToBatch = function (index) {
            console.log('addClaimToBatch: ' + index);
            BatchEditService.addClaimToBatch(index);
        };

        _this.hideBatchClaims = function () {
            BatchEditService.hideBatchClaimsFromSearch();
        };

        _this.init = function () {
            console.log('fire ehrBatchSearchController');
            _this.searching = false;

            _this.searchResults = BatchEditService.getSearchResults();
            console.log(_this.searchResults);
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrBatchEditController', ehrBatchEditController)
        .controller('ehrBatchHeaderController', ehrBatchHeaderController)
        .controller('ehrBatchLinesController', ehrBatchLinesController)
        .controller('ehrBatchSearchController', ehrBatchSearchController)
        .config(batchSubtabConfig);

})();
