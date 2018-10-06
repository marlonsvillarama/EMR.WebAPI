(function () {
    'use strict';

    setupSubtabConfig.$inject = ['$stateProvider'];
    function setupSubtabConfig($stateProvider) {
        $stateProvider.state('setup.providers', {
            controller: 'ehrSetupProvidersController as sProvCtrl'
        });

        $stateProvider.state('setup.providers.list', {
            templateUrl: '/ehr/setup/setup-providers.html',
            controller: 'ehrSetupProvidersController as sProvCtrl'
        });

        $stateProvider.state('setup.providers.edit', {
            templateUrl: '/ehr/setup/setup-provider-edit.html',
            controller: 'ehrSetupEditProviderController as sEditProvCtrl',
            resolve: {
                entity: ['$stateParams', 'ApiService', 'SetupService', function ($stateParams, ApiService, SetupService) {
                    var resp = ApiService.getEntity('Provider', SetupService.getCurrentId('provider'));
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });

        $stateProvider.state('setup.billing', {
            templateUrl: '/ehr/setup/setup-billing.html',
            controller: 'ehrSetupBillingController as sBillCtrl'
        });

        $stateProvider.state('setup.facilities', {
            controller: 'ehrSetupFacilitiesController as sFacCtrl'
        });

        $stateProvider.state('setup.facilities.list', {
            templateUrl: '/ehr/setup/setup-facilities.html',
            controller: 'ehrSetupFacilitiesController as sFacCtrl'
        });

        $stateProvider.state('setup.facilities.edit', {
            templateUrl: '/ehr/setup/setup-facility-edit.html',
            controller: 'ehrSetupEditFacilityController as sEditFacCtrl',
            resolve: {
                entity: ['$stateParams', 'ApiService', 'SetupService', function ($stateParams, ApiService, SetupService) {
                    var resp = ApiService.getEntity('Facility', SetupService.getCurrentId('facility'));
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });

        $stateProvider.state('setup.groups', {
            controller: 'ehrSetupGroupsController as sGrpCtrl'
        });

        $stateProvider.state('setup.groups.list', {
            templateUrl: '/ehr/setup/setup-groups.html',
            controller: 'ehrSetupGroupsController as sGrpCtrl'
        });

        $stateProvider.state('setup.groups.edit', {
            templateUrl: '/ehr/setup/setup-group-edit.html',
            controller: 'ehrSetupEditGroupController as sEditGrpCtrl',
            resolve: {
                entity: ['$stateParams', 'ApiService', 'SetupService', function ($stateParams, ApiService, SetupService) {
                    var resp = ApiService.getEntity('Group', SetupService.getCurrentId('group'));
                    return resp.then(function (response) {
                        return ApiService.prepareResponse(response);
                    });
                }]
            }
        });
    }

    // Setup page main controller
    ehrSetupController.$inject = ['$state', 'UIService'];
    function ehrSetupController($state, UIService) {
        var _this = this;

        _this.init = function () {
            UIService.log('fire ehrSetupController');

            // Go to the first subtab
            $state.go('setup.providers');
        };

        _this.init();
    }

    ehrSetupProvidersController.$inject = ['$state', 'SetupService', 'UIService'];
    function ehrSetupProvidersController($state, SetupService, UIService) {
        var _this = this;

        _this.editProv = function (id) {
            UIService.log('editProv: ' + id);
            SetupService.setCurrentId('provider', id);
            $state.go('setup.providers.edit');
        };

        _this.search = function () {
            SetupService.search('Providers', 'provider').then(function (response) {
                var list = [];

                UIService.log(response);
                if (response.data.results.IsSuccess == true) {
                    list = response.data.results.Data;//SetupService.formatProviderList(response.data.result.Data);
                }
                UIService.log('done providers.search');

                SetupService.setList('Providers', list);
                UIService.log(list);
                _this.list = list;
            });
        }

        _this.init = function () {
            UIService.log('fire ehrSetupProvidersController');

            _this.search();
            $state.go('setup.providers.list');
        };

        _this.init();
    }

    ehrSetupEditProviderController.$inject = ['$state', 'ngDialog', 'entity', 'ApiService', 'SetupService', 'UIService'];
    function ehrSetupEditProviderController($state, ngDialog, entity, ApiService, SetupService, UIService) {
        var _this = this;

        _this.validateForm = function () {
            UIService.log('validateForm');
            _this.errors = [];

            if (!_this.entity.FirstName) {
                _this.errors.push('Provider First Name is required');
            }

            if (!_this.entity.LastName) {
                _this.errors.push('Provider Last Name is required');
            }

            if (!_this.entity.NPI) {
                _this.errors.push('Provider NPI is required');
            }

            if (!_this.entity.TaxonomyCodeId) {
                _this.errors.push('Taxonomy Code is required');
            }

            if (_this.errors.length > 0) {
                var str = 'Please correct the following errors first:\n\n';
                for (var i = 0, n = _this.errors.length; i < n; i++) {
                    str += '- ' + _this.errors[i] + '\n';
                }
                alert(str);

                return false;
            }

            return true;
        };

        _this.submitForm = function () {
            UIService.log('submitForm');
            UIService.log(_this.entity);

            var actionType = '';
            actionType = (_this.entity.Id != null &&
                _this.entity.Id != undefined &&
                _this.entity.Id != '') == true ?
                'update' : 'create';

            if (!_this.validateForm()) {
                return false;
            }

            UIService.log('form is valid');
            _this.savingForm = true;

            SetupService.updateEntity({
                type: 'Provider',
                entity: _this.entity
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;

                if (resSuccess == true) {
                    _this.savingForm = false;
                    if (actionType == 'update') {
                        alert('Changes successfully saved!');
                        $state.go('setup.providers.list');
                    }
                    else if (actionType == 'create') {
                        alert('Provider successfully created!');
                        SetupService.setCurrentId('provider', resData);
                        $state.go('setup.providers.edit');
                    }
                }
            });
        };

        _this.cancelEdit = function () {
            if (confirm('Any unsaved changes will be lost. Are you sure?')) {
                $state.go('setup.providers.list');
            }
        };

        _this.showTaxonomy = function (tc) {
            _this.taxonomyName = "";
            _this.entity.TaxonomyCode = tc;

            if (tc) {
                _this.entity.TaxonomyCodeId = tc.Id;
                _this.taxonomyName = tc.Code + " - " +
                    tc.Classification + " : " +
                    tc.Specialization;
            }
        };

        _this.openModalTaxonomy = function () {
            ngDialog.openConfirm({
                template: '/ehr/shared/views/ehr.shared.taxonomy-modal.component.html',
                controller: 'TaxonomyModalController',
                controllerAs: 'taxoModalCtrl',
                disableAnimation: true
            }).then(
                function (value) {
                    UIService.log(value);
                    _this.showTaxonomy(value);
                },
                function (value) {
                }
            );
        };

        _this.removeTaxonomyCode = function () {
            if (window.confirm('Are you sure you want to remove the taxonomy code?') == true) {
                _this.showTaxonomy(null);
                alert('WARNING: You need to set a Taxonomy Code for this Provider.')
            }
        };

        _this.init = function () {
            UIService.log('fire ehrSetupEditProviderController');

            _this.entity = entity.entityList;
            _this.showTaxonomy(_this.entity.TaxonomyCode);
            _this.states = ApiService.getList('usstates').options;
            _this.savingForm = false;
        };

        _this.init();
    }

    ehrSetupBillingController.$inject = ['$q', '$state', 'SetupService'];
    function ehrSetupBillingController($q, $state, SetupService) {
        var _this = this;

        _this.init = function () {
            _this.searching = true;
            var billProvs = [], renProvs = [], reqs = [];
            var map = {};

            reqs.push(
                SetupService.search('Providers', 'provider').then(function (response) {
                    if (response.data.results.IsSuccess == true) {
                        renProvs = response.data.result.Data;
                    }
                })
            );

            reqs.push(
                SetupService.search('Groups', 'group').then(function (response) {
                    if (response.data.results.IsSuccess == true) {
                        billProvs = response.data.result.Data;
                    }
                })
            );

            $q.all(reqs).then(function (response) {
                for (var i = 0, n = billProvs.length; i < n; i++) {
                    for (var j = 0, o = renProvs.length; j < o; j++) {

                    }
                }

                _this.searching == false;
            });
        };

        _this.init();
    }

    ehrSetupFacilitiesController.$inject = ['$state', 'SetupService', 'UIService'];
    function ehrSetupFacilitiesController($state, SetupService, UIService) {
        var _this = this;

        _this.editFac = function (id) {
            UIService.log('editFac: ' + id);
            SetupService.setCurrentId('facility', id);
            $state.go('setup.facilities.edit');
        };

        _this.search = function () {
            SetupService.search('Facilities', 'facility').then(function (response) {
                var list = [];

                UIService.log(response);
                if (response.data.results.IsSuccess == true) {
                    list = response.data.results.Data;
                }

                UIService.log(list);
                UIService.log('done facilities.search');

                SetupService.setList('Facilities', list);
                _this.list = list;
            });
        }

        _this.init = function () {
            UIService.log('fire ehrSetupFacilitiesController');

            _this.search();
            $state.go('setup.facilities.list');
        };

        _this.init();
    }

    ehrSetupEditFacilityController.$inject = ['$state', 'entity', 'ApiService', 'SetupService', 'UIService'];
    function ehrSetupEditFacilityController($state, entity, ApiService, SetupService, UIService) {
        var _this = this;

        _this.validateForm = function () {
            UIService.log('validateForm');
            _this.errors = [];

            if (!_this.entity.Name) {
                _this.errors.push('Facility Name is required');
            }

            if (!_this.entity.NPI) {
                _this.errors.push('Facility NPI is required');
            }

            if (_this.errors.length > 0) {
                var str = 'Please correct the following errors first:\n\n';
                for (var i = 0, n = _this.errors.length; i < n; i++) {
                    str += '- ' + _this.errors[i] + '\n';
                }
                alert(str);

                return false;
            }

            return true;
        };

        _this.submitForm = function () {
            UIService.log('submitForm');
            UIService.log(_this.entity);

            var actionType = '';
            actionType = (_this.entity.Id != null &&
                _this.entity.Id != undefined &&
                _this.entity.Id != '') == true ?
                'update' : 'create';

            if (!_this.validateForm()) {
                return false;
            }

            UIService.log('form is valid');
            _this.savingForm = true;

            SetupService.updateEntity({
                type: 'Facility',
                entity: _this.entity
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;

                if (resSuccess == true) {
                    _this.savingForm = false;
                    if (actionType == 'update') {
                        alert('Changes successfully saved!');
                        $state.go('setup.facilities.list');
                    }
                    else if (actionType == 'create') {
                        alert('Facility successfully created!');
                        SetupService.setCurrentId('facility', resData);
                        $state.go('setup.facilities.edit');
                    }
                }
            });
        };

        _this.cancelEdit = function () {
            if (confirm('Any unsaved changes will be lost. Are you sure?')) {
                $state.go('setup.facilities.list');
            }
        };

        _this.init = function () {
            UIService.log('fire ehrSetupEditFacilitiesController');

            _this.entity = entity.entityList;
            _this.states = ApiService.getList('usstates').options;
            _this.savingForm = false;

            UIService.log(this.entity);
        };

        _this.init();
    }

    ehrSetupGroupsController.$inject = ['$state', 'SetupService', 'UIService'];
    function ehrSetupGroupsController($state, SetupService, UIService) {
        var _this = this;

        _this.editGrp = function (id) {
            UIService.log('editGrp: ' + id);
            SetupService.setCurrentId('group', id);
            $state.go('setup.groups.edit');
        };

        _this.search = function () {
            SetupService.search('Groups', 'group').then(function (response) {
                var list = [];

                UIService.log(response);
                if (response.data.results.IsSuccess == true) {
                    list = response.data.results.Data;
                }
                UIService.log('done groups.search');

                SetupService.setList('Groups', list);
                _this.list = list;
            });
        }

        _this.init = function () {
            UIService.log('fire ehrSetupGroupsController');

            _this.search();
            $state.go('setup.groups.list');
        };

        _this.init();
    }

    ehrSetupEditGroupController.$inject = ['$state', 'entity', 'ApiService', 'SetupService', 'UIService'];
    function ehrSetupEditGroupController($state, entity, ApiService, SetupService, UIService) {
        var _this = this;

        _this.validateForm = function () {
            UIService.log('validateForm');
            _this.errors = [];

            if (!_this.entity.LastName) {
                _this.errors.push('Group Name is required');
            }

            if (!_this.entity.NPI) {
                _this.errors.push('Group NPI is required');
            }

            if (!_this.entity.EIN) {
                _this.errors.push('Group EIN is required');
            }

            if (_this.errors.length > 0) {
                var str = 'Please correct the following errors first:\n\n';
                for (var i = 0, n = _this.errors.length; i < n; i++) {
                    str += '- ' + _this.errors[i] + '\n';
                }
                alert(str);

                return false;
            }

            return true;
        };

        _this.submitForm = function () {
            UIService.log('submitForm');
            UIService.log(_this.entity);

            var actionType = '';
            actionType = (_this.entity.Id != null &&
                _this.entity.Id != undefined &&
                _this.entity.Id != '') == true ?
                'update' : 'create';

            if (!_this.validateForm()) {
                return false;
            }

            UIService.log('form is valid');
            _this.savingForm = true;

            SetupService.updateEntity({
                type: 'Provider',
                entity: _this.entity
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;

                if (resSuccess == true) {
                    _this.savingForm = false;
                    if (actionType == 'update') {
                        alert('Changes successfully saved!');
                        $state.go('setup.groups.list');
                    }
                    else if (actionType == 'create') {
                        alert('Group successfully created!');
                        SetupService.setCurrentId('group', resData);
                        $state.go('setup.groups.edit', { id: resData });
                    }
                }
            });
        };

        _this.cancelEdit = function () {
            if (confirm('Any unsaved changes will be lost. Are you sure?')) {
                $state.go('setup.groups.list');
            }
        };

        _this.init = function () {
            UIService.log('fire ehrSetupEditGroupController');

            _this.entity = entity.entityList;
            _this.states = ApiService.getList('usstates').options;
            _this.savingForm = false;

            UIService.log(this.entity);
        };

        _this.init();
    }

    angular.module('ehrApp')
        .config(setupSubtabConfig)
        .controller('ehrSetupController', ehrSetupController)
        .controller('ehrSetupProvidersController', ehrSetupProvidersController)
        .controller('ehrSetupEditProviderController', ehrSetupEditProviderController)
        .controller('ehrSetupBillingController', ehrSetupBillingController)
        .controller('ehrSetupFacilitiesController', ehrSetupFacilitiesController)
        .controller('ehrSetupEditFacilityController', ehrSetupEditFacilityController)
        .controller('ehrSetupGroupsController', ehrSetupGroupsController)
        .controller('ehrSetupEditGroupController', ehrSetupEditGroupController);

})();

