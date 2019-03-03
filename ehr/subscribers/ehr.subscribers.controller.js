(function () {
    'use strict';

    //ehrSubscriberListController.$inject = ['entityList'];
    //function ehrSubscriberListController(entityList) {

    ehrSubscriberListController.$inject = ['ApiService', 'UIService'];
    function ehrSubscriberListController(ApiService, UIService) {
        var _this = this;
        _this.pageNum = 1;
        _this.pageSize = 20;

        _this.searchList = function (page) {
            if (page == '' || page == null || page == undefined) {
                _this.pageNum = 1;
            }
            else {
                _this.pageNum += page;

                if (_this.pageNum < 1) {
                    _this.pageNum = 1;
                }
            }

            var arrParms = [
                "subscribers",
                _this.pageNum, _this.pageSize,
                _this.lastName ? _this.lastName : "",
                _this.dateOfBirth ? UIService.getDateParam(_this.dateOfBirth) : ""
            ];

            var parms = arrParms.join('|');
            console.log('searchSubs: ' + parms);
            _this.searching = true;

            ApiService.searchEntities('Subscribers', parms).then(function (response) {
                var subs = [];

                _this.searched = true;
                _this.entities = (response.data.result.IsSuccess == true) ?
                    response.data.result.Data : [];

                _this.resultsCount = response.data.result.Count;
                _this.startIndex = ((_this.pageNum - 1) * _this.pageSize) + 1;
                _this.endIndex = _this.resultsCount > (_this.pageNum * _this.pageSize) ?
                    (_this.pageNum * _this.pageSize) : _this.resultsCount;
                _this.searching = false;
            });

        };
        //_this.entities = entityList.entityList;

        _this.init = function () {
            _this.searching = false;
            UIService.applyDatePickers();
        };

        _this.init();
    }

    angular.module('ehrApp').controller('ehrSubscriberListController', ehrSubscriberListController);

    ehrSubscriberEditController.$inject = ['$state', 'ngDialog', 'entity', 'ApiService', 'UIService', 'ModalService'];
    function ehrSubscriberEditController($state, ngDialog, entity, ApiService, UIService, ModalService) {
        var _this = this;
        var actionType = '';

        _this.getAge = function () {
            UIService.log(_this.entity.DateOfBirth);
            _this.age = UIService.calculateAge(new Date(_this.entity.DateOfBirth));
            return _this.age;
        };

        _this.isEdit = function () {
            if (_this.entity.Id != null &&
                _this.entity.Id != undefined &&
                _this.entity.Id != '')
            {
                return true;
            }
            return false;
        };

        _this.openAddDepModal = function () {
            ngDialog.openConfirm({
                template: '/ehr/shared/views/ehr.shared.addpatient-modal.component.html',
                controller: 'AddDependentModalController',
                controllerAs: 'addDepModalCtrl',
                disableAnimation: true,
                resolve: {
                    /*
                    payers: ['ApiService', function (ApiService) {
                        var resp = ApiService.getEntityList('Payers', 'payer');
                        return resp.then(function (response) {
                            return ApiService.prepareResponse(response);
                        });
                    }]
                    */
                }
            }).then(
                function (value) {
                    UIService.log('depModal OK');
                    UIService.log(value);
                },
                function (value) {
                    UIService.log('depModal cancel');
                }
            );
        };

        _this.openModalPayers = function (id) {
            ngDialog.openConfirm({
                template: '/ehr/shared/views/ehr.shared.payer-modal.component.html',
                controller: 'PayerModalController',
                controllerAs: 'payerModalCtrl',
                disableAnimation: true,
                resolve: {
                    payers: ['ApiService', function (ApiService) {
                        var resp = ApiService.getEntityList('Payers', 'payer');
                        return resp.then(function (response) {
                            return ApiService.prepareResponse(response);
                        });
                    }]
                }
            }).then(
                function (value) {
                    UIService.log('inside confirm');
                    UIService.log(value);
                    if (id == 1) {
                        _this.entity.PrimaryPayerId = value.Id;
                        _this.entity.PrimaryPayer = { Name: value.Name };
                    }
                    else if (id == 2) {
                        _this.entity.SecondaryPayerId = value.Id;
                        _this.entity.SecondaryPayer = { Name: value.Name };
                    }
                },
                function (value) {
                }
            );
        };

        _this.removePayer = function (id) {
            UIService.log('removePayer');
            if (window.confirm('Are you sure you want to remove this payer?') == true) {
                if (id == 1) {
                    _this.entity.PrimaryPayerId = null;
                    _this.entity.PrimaryPayer = null;
                    _this.entity.PrimaryMemberID = null;
                    alert('WARNING: You need to set a Primary Payer for this Subscriber.')
                }
                else if (id == 2) {
                    _this.entity.SecondaryPayerId = null;
                    _this.entity.SecondaryPayer = null;
                    _this.entity.SecondaryMemberID = null;
                }
            }
        }

        _this.validateForm = function () {
            if (!_this.entity.PrimaryPayerId) {
                alert('Please select a Primary Payer');
                return false;
            }

            if (_this.entity.SecondaryPayerId) {
                if (!_this.entity.SecondaryMemberID) {
                    alert('ERROR: The Secondary Member ID is missing.');
                    return false;
                }
            }
            else {
                if (_this.entity.SecondaryMemberID) {
                    alert('ERROR: The Secondary Payer is missing.');
                    return false;
                }
            }

            return true;
        };

        _this.submitForm = function () {
            var actionType = '';
            actionType = (_this.entity.Id != null &&
                _this.entity.Id != undefined &&
                _this.entity.Id != '') == true ?
                'update' : 'create';

            if (!_this.validateForm()) {
                UIService.log('form is invalid');
                return false;
            }

            _this.savingForm = true;

            ApiService.updateEntity({
                type: 'Subscriber',
                entity: _this.entity
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;

                if (resSuccess == true) {
                    if (actionType == 'update') {
                        _this.savingForm = false;
                        alert('Changes successfully saved!')
                    }
                    else if (actionType == 'create') {
                        alert('Subscriber successfully created!')
                        $state.go('subscriber-edit', { id: resData });
                    }
                }
            });
        };

        _this.deleteEntity = function (type) {
            if (confirm('Are you sure you want to delete this ' + type + '?') == false) {
                alert('no go');
                return false;
            }
        };

        _this.init = function () {
            _this.savingForm = false;
            _this.showOK = false;
            _this.showWarning = false;
            _this.showError = false;

            UIService.log('fire subscriberEditController');
            UIService.log(entity.entityList);
            UIService.log(ApiService.getList('usstates'));
            UIService.log(ApiService.getList('patrels'));

            _this.states = ApiService.getList('usstates').options;
            _this.relationships = ApiService.getList('patrels').options;

            var ent = entity.entityList;
            var id = ent.id;
            var title;

            if (ent.Id) {
                title = "EDIT ";
            }
            else {
                title = "CREATE NEW ";
            }
            _this.page_title = title + 'SUBSCRIBER';

            ent.DateOfBirth = UIService.parseDate(ent.DateOfBirth);

            // Expose sub model to view
            UIService.applyDatePickers();
            //UIService.applyMasks();

            _this.entity = ent;
            _this.age = _this.getAge();
        };

        _this.init();
    }

    angular.module('ehrApp').controller('ehrSubscriberEditController', ehrSubscriberEditController);
})();
