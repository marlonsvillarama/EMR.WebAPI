(function () {

    PayerModalController.$inject = ['payers', 'ModalService', 'UIService'];
    function PayerModalController(payers, ModalService, UIService) {
        var payerModalCtrl = this;

        payerModalCtrl.payers = payers.entityList;
        payerModalCtrl.filteredPayers = payers.entityList;
        payerModalCtrl.currentPage = 1;
        payerModalCtrl.pageSize = ModalService.PAGE_SIZE;
        payerModalCtrl.pageLength = Math.ceil(payerModalCtrl.filteredPayers.length / ModalService.PAGE_SIZE);

        payerModalCtrl.updatePageIndexes = function () {
            payerModalCtrl.startIndex = Math.min(
                ((payerModalCtrl.currentPage - 1) * payerModalCtrl.pageSize) + 1,
                payerModalCtrl.filteredPayers.length
            );
            payerModalCtrl.endIndex = Math.min(
                (payerModalCtrl.startIndex + ModalService.PAGE_SIZE) - 1,
                payerModalCtrl.filteredPayers.length
            );
        };
        payerModalCtrl.updatePageIndexes();

        payerModalCtrl.filterText = "";
        payerModalCtrl.filterResults = function () {
            payerModalCtrl.filteredPayers = payerModalCtrl.payers.filter(p => p.Name.toUpperCase().startsWith(payerModalCtrl.filterText.toUpperCase()));
            payerModalCtrl.pageLength = Math.ceil(payerModalCtrl.filteredPayers.length / ModalService.PAGE_SIZE);
            payerModalCtrl.currentPage = 1;
            payerModalCtrl.updatePageIndexes();
        };

        payerModalCtrl.selectPayer = function (payerIndex) {
            var pIdx = ((payerModalCtrl.currentPage - 1) * payerModalCtrl.pageSize) + payerIndex;
            return payerModalCtrl.filteredPayers[pIdx];
        };

        payerModalCtrl.isFirstPage = function () {
            return payerModalCtrl.currentPage == 1;
        };

        payerModalCtrl.isLastPage = function () {
            return payerModalCtrl.currentPage == payerModalCtrl.pageLength;
        };

        payerModalCtrl.getEndIndex = function () {

        };

        payerModalCtrl.nextPage = function () {
            payerModalCtrl.currentPage += 1;
            payerModalCtrl.updatePageIndexes();
            UIService.log('nextPage: ' + payerModalCtrl.currentPage);
        }

        payerModalCtrl.prevPage = function () {
            payerModalCtrl.currentPage -= 1;
            payerModalCtrl.updatePageIndexes();
            UIService.log('prevPage: ' + payerModalCtrl.currentPage);
        }
    }

    GroupModalController.$inject = ['entities', 'ModalService', 'UIService'];
    function GroupModalController(entities, ModalService, UIService) {
        var groupModalCtrl = this;

        groupModalCtrl.list = entities.entityList;
        groupModalCtrl.filteredList = entities.entityList;
        groupModalCtrl.currentPage = 1;
        groupModalCtrl.pageSize = ModalService.PAGE_SIZE;
        groupModalCtrl.pageLength = Math.ceil(groupModalCtrl.filteredList.length / ModalService.PAGE_SIZE);

        groupModalCtrl.updatePageIndexes = function () {
            groupModalCtrl.startIndex = Math.min(
                ((groupModalCtrl.currentPage - 1) * groupModalCtrl.pageSize) + 1,
                groupModalCtrl.filteredList.length
            );
            groupModalCtrl.endIndex = Math.min(
                (groupModalCtrl.startIndex + ModalService.PAGE_SIZE) - 1,
                groupModalCtrl.filteredList.length
            );
        };
        groupModalCtrl.updatePageIndexes();

        groupModalCtrl.filterText = "";
        groupModalCtrl.filterResults = function () {
            groupModalCtrl.filteredList = groupModalCtrl.list.filter(x => x.LastName.toUpperCase().startsWith(groupModalCtrl.filterText.toUpperCase()));
            groupModalCtrl.pageLength = Math.ceil(groupModalCtrl.filteredList.length / ModalService.PAGE_SIZE);
            groupModalCtrl.currentPage = 1;
            groupModalCtrl.updatePageIndexes();
        };

        groupModalCtrl.selectEntity = function (index) {
            var idx = ((groupModalCtrl.currentPage - 1) * groupModalCtrl.pageSize) + index;
            return groupModalCtrl.filteredList[idx];
        };

        groupModalCtrl.isFirstPage = function () {
            return groupModalCtrl.currentPage == 1;
        };

        groupModalCtrl.isLastPage = function () {
            return groupModalCtrl.currentPage == groupModalCtrl.pageLength;
        };

        groupModalCtrl.nextPage = function () {
            groupModalCtrl.currentPage += 1;
            groupModalCtrl.updatePageIndexes();
            UIService.log('nextPage: ' + groupModalCtrl.currentPage);
        }

        groupModalCtrl.prevPage = function () {
            groupModalCtrl.currentPage -= 1;
            groupModalCtrl.updatePageIndexes();
            UIService.log('prevPage: ' + groupModalCtrl.currentPage);
        }
    }

    ProviderModalController.$inject = ['entities', 'ModalService', 'UIService'];
    function ProviderModalController(entities, ModalService, UIService) {
        var providerModalCtrl = this;

        providerModalCtrl.list = entities.entityList;
        providerModalCtrl.filteredList = entities.entityList;
        providerModalCtrl.currentPage = 1;
        providerModalCtrl.pageSize = ModalService.PAGE_SIZE;
        providerModalCtrl.pageLength = Math.ceil(providerModalCtrl.filteredList.length / ModalService.PAGE_SIZE);

        providerModalCtrl.updatePageIndexes = function () {
            providerModalCtrl.startIndex = Math.min(
                ((providerModalCtrl.currentPage - 1) * providerModalCtrl.pageSize) + 1,
                providerModalCtrl.filteredList.length
            );
            providerModalCtrl.endIndex = Math.min(
                (providerModalCtrl.startIndex + ModalService.PAGE_SIZE) - 1,
                providerModalCtrl.filteredList.length
            );
        };
        providerModalCtrl.updatePageIndexes();

        providerModalCtrl.filterText = "";
        providerModalCtrl.filterResults = function () {
            providerModalCtrl.filteredList = providerModalCtrl.list.filter(x => x.NameFormatted.toUpperCase().startsWith(providerModalCtrl.filterText.toUpperCase()));
            providerModalCtrl.pageLength = Math.ceil(providerModalCtrl.filteredList.length / ModalService.PAGE_SIZE);
            providerModalCtrl.currentPage = 1;
            providerModalCtrl.updatePageIndexes();
        };

        providerModalCtrl.selectEntity = function (index) {
            var idx = ((providerModalCtrl.currentPage - 1) * providerModalCtrl.pageSize) + index;
            return providerModalCtrl.filteredList[idx];
        };

        providerModalCtrl.isFirstPage = function () {
            return providerModalCtrl.currentPage == 1;
        };

        providerModalCtrl.isLastPage = function () {
            return providerModalCtrl.currentPage == providerModalCtrl.pageLength;
        };

        providerModalCtrl.nextPage = function () {
            providerModalCtrl.currentPage += 1;
            providerModalCtrl.updatePageIndexes();
            UIService.log('nextPage: ' + providerModalCtrl.currentPage);
        }

        providerModalCtrl.prevPage = function () {
            providerModalCtrl.currentPage -= 1;
            providerModalCtrl.updatePageIndexes();
            UIService.log('prevPage: ' + providerModalCtrl.currentPage);
        }
    }

    FacilityModalController.$inject = ['entities', 'ModalService', 'UIService'];
    function FacilityModalController(entities, ModalService, UIService) {
        var facilityModalCtrl = this;

        facilityModalCtrl.list = entities.entityList;
        facilityModalCtrl.filteredList = entities.entityList;
        facilityModalCtrl.currentPage = 1;
        facilityModalCtrl.pageSize = ModalService.PAGE_SIZE;
        facilityModalCtrl.pageLength = Math.ceil(facilityModalCtrl.filteredList.length / ModalService.PAGE_SIZE);

        facilityModalCtrl.updatePageIndexes = function () {
            facilityModalCtrl.startIndex = Math.min(
                ((facilityModalCtrl.currentPage - 1) * facilityModalCtrl.pageSize) + 1,
                facilityModalCtrl.filteredList.length
            );
            facilityModalCtrl.endIndex = Math.min(
                (facilityModalCtrl.startIndex + ModalService.PAGE_SIZE) - 1,
                facilityModalCtrl.filteredList.length
            );
        };
        facilityModalCtrl.updatePageIndexes();

        facilityModalCtrl.filterText = "";
        facilityModalCtrl.filterResults = function () {
            facilityModalCtrl.filteredList = facilityModalCtrl.list.filter(x => x.Name.toUpperCase().startsWith(facilityModalCtrl.filterText.toUpperCase()));
            facilityModalCtrl.pageLength = Math.ceil(facilityModalCtrl.filteredList.length / ModalService.PAGE_SIZE);
            facilityModalCtrl.currentPage = 1;
            facilityModalCtrl.updatePageIndexes();
        };

        facilityModalCtrl.selectEntity = function (index) {
            var idx = ((facilityModalCtrl.currentPage - 1) * facilityModalCtrl.pageSize) + index;
            return facilityModalCtrl.filteredList[idx];
        };

        facilityModalCtrl.isFirstPage = function () {
            return facilityModalCtrl.currentPage == 1;
        };

        facilityModalCtrl.isLastPage = function () {
            return facilityModalCtrl.currentPage == facilityModalCtrl.pageLength;
        };

        facilityModalCtrl.nextPage = function () {
            facilityModalCtrl.currentPage += 1;
            facilityModalCtrl.updatePageIndexes();
            UIService.log('nextPage: ' + facilityModalCtrl.currentPage);
        }

        facilityModalCtrl.prevPage = function () {
            facilityModalCtrl.currentPage -= 1;
            facilityModalCtrl.updatePageIndexes();
            UIService.log('prevPage: ' + facilityModalCtrl.currentPage);
        }
    }

    PosModalController.$inject = ['entities', 'ModalService', 'UIService'];
    function PosModalController(entities, ModalService, UIService) {
        var posModalCtrl = this;
        var PAGE_SIZE = 5;

        posModalCtrl.list = entities.entityList;
        posModalCtrl.filteredList = entities.entityList;
        posModalCtrl.currentPage = 1;
        posModalCtrl.pageSize = PAGE_SIZE;
        posModalCtrl.pageLength = Math.ceil(posModalCtrl.filteredList.length / PAGE_SIZE);

        posModalCtrl.updatePageIndexes = function () {
            posModalCtrl.startIndex = Math.min(
                ((posModalCtrl.currentPage - 1) * posModalCtrl.pageSize) + 1,
                posModalCtrl.filteredList.length
            );
            posModalCtrl.endIndex = Math.min(
                (posModalCtrl.startIndex + PAGE_SIZE) - 1,
                posModalCtrl.filteredList.length
            );
        };
        posModalCtrl.updatePageIndexes();

        posModalCtrl.filterText = "";
        posModalCtrl.filterResults = function () {
            posModalCtrl.filteredList = posModalCtrl.list.filter(x => x.Name.toUpperCase().startsWith(posModalCtrl.filterText.toUpperCase()));
            posModalCtrl.pageLength = Math.ceil(posModalCtrl.filteredList.length / PAGE_SIZE);
            posModalCtrl.currentPage = 1;
            posModalCtrl.updatePageIndexes();
        };

        posModalCtrl.selectEntity = function (index) {
            var idx = ((posModalCtrl.currentPage - 1) * PAGE_SIZE) + index;
            return posModalCtrl.filteredList[idx];
        };

        posModalCtrl.isFirstPage = function () {
            return posModalCtrl.currentPage == 1;
        };

        posModalCtrl.isLastPage = function () {
            return posModalCtrl.currentPage == posModalCtrl.pageLength;
        };

        posModalCtrl.nextPage = function () {
            posModalCtrl.currentPage += 1;
            posModalCtrl.updatePageIndexes();
            UIService.log('nextPage: ' + posModalCtrl.currentPage);
        }

        posModalCtrl.prevPage = function () {
            posModalCtrl.currentPage -= 1;
            posModalCtrl.updatePageIndexes();
            UIService.log('prevPage: ' + posModalCtrl.currentPage);
        }
    }

    DependentsModalController.$inject = ['entities', 'ApiService', 'ModalService', 'UIService'];
    function DependentsModalController(entities, ApiService, ModalService, UIService) {
        var depModalCtrl = this;

        UIService.log('fire DependentsModalController');
        UIService.log(entities);
        UIService.log(ApiService.getList('patrels'));

        var entList = [];
        for (var i = 0, n = entities.entityList.length; i < n; i++) {
            var ent = entities.entityList[i];
            var rel = ApiService.getRelationshipByCode(ent.Relationship);

            entList.push({
                id: ent.PatientId,
                fname: ent.FirstName,
                lname: ent.LastName,
                name: ent.LastName + ", " + ent.FirstName,
                dob: ent.DateOfBirth,
                gender: ent.Gender,
                genderName: UIService.getGender(ent.Gender),
                rel: rel.Code,
                relName: rel.Name
            });
        }
        UIService.log(entList);

        depModalCtrl.list = entList;
        depModalCtrl.filteredList = entList;
        depModalCtrl.currentPage = 1;
        depModalCtrl.pageSize = ModalService.PAGE_SIZE;
        depModalCtrl.pageLength = Math.ceil(depModalCtrl.filteredList.length / ModalService.PAGE_SIZE);

        depModalCtrl.updatePageIndexes = function () {
            depModalCtrl.startIndex = Math.min(
                ((depModalCtrl.currentPage - 1) * depModalCtrl.pageSize) + 1,
                depModalCtrl.filteredList.length
            );
            depModalCtrl.endIndex = Math.min(
                (depModalCtrl.startIndex + ModalService.PAGE_SIZE) - 1,
                depModalCtrl.filteredList.length
            );
        };
        depModalCtrl.updatePageIndexes();

        depModalCtrl.filterText = "";
        depModalCtrl.filterResults = function () {
            depModalCtrl.filteredList = depModalCtrl.list.filter(x => x.name.toUpperCase().startsWith(depModalCtrl.filterText.toUpperCase()));
            depModalCtrl.pageLength = Math.ceil(depModalCtrl.filteredList.length / ModalService.PAGE_SIZE);
            depModalCtrl.currentPage = 1;
            depModalCtrl.updatePageIndexes();
        };

        depModalCtrl.selectEntity = function (index) {
            var idx = ((depModalCtrl.currentPage - 1) * ModalService.PAGE_SIZE) + index;
            return depModalCtrl.filteredList[idx];
        };

        depModalCtrl.isFirstPage = function () {
            return depModalCtrl.currentPage == 1;
        };

        depModalCtrl.isLastPage = function () {
            return depModalCtrl.currentPage == depModalCtrl.pageLength;
        };

        depModalCtrl.nextPage = function () {
            depModalCtrl.currentPage += 1;
            depModalCtrl.updatePageIndexes();
            UIService.log('nextPage: ' + depModalCtrl.currentPage);
        }

        depModalCtrl.prevPage = function () {
            depModalCtrl.currentPage -= 1;
            depModalCtrl.updatePageIndexes();
            UIService.log('prevPage: ' + depModalCtrl.currentPage);
        }
    }

    AddDependentModalController.$inject = ['UIService'];
    function AddDependentModalController(UIService) {
        var addDepModalCtrl = this;

        addDepModalCtrl.savingForm = false;

        addDepModalCtrl.submitForm = function () {
            addDepModalCtrl.savingForm = true;
            UIService.log('submitting modal');
        }
    };

    TaxonomyModalController.$inject = ['ApiService', 'ModalService', 'UIService'];
    function TaxonomyModalController(ApiService, ModalService, UIService) {
        var _this = this;
        var PAGE_SIZE = 10;

        _this.updatePageIndexes = function () {
            _this.startIndex = Math.min(
                ((_this.currentPage - 1) * _this.pageSize) + 1,
                _this.filteredList.length
            );
            _this.endIndex = Math.min(
                (_this.startIndex + PAGE_SIZE) - 1,
                _this.filteredList.length
            );
        };

        _this.filterResults = function () {
            _this.filteredList = _this.list.filter(x => x.Classification.toUpperCase().startsWith(_this.filterText.toUpperCase()));
            _this.pageLength = Math.ceil(_this.filteredList.length / PAGE_SIZE);
            _this.currentPage = 1;
            _this.updatePageIndexes();
        };

        _this.selectEntity = function (index) {
            var idx = ((_this.currentPage - 1) * PAGE_SIZE) + index;
            return _this.filteredList[idx];
        };

        _this.isFirstPage = function () {
            return _this.currentPage == 1;
        };

        _this.isLastPage = function () {
            return _this.currentPage == _this.pageLength;
        };

        _this.nextPage = function () {
            _this.currentPage += 1;
            _this.updatePageIndexes();
            UIService.log('nextPage: ' + _this.currentPage);
        };

        _this.prevPage = function () {
            _this.currentPage -= 1;
            _this.updatePageIndexes();
            UIService.log('prevPage: ' + _this.currentPage);
        };

        _this.init = function () {
            UIService.log('editProv.init');
            _this.list = ApiService.getList('taxonomy');
            _this.filteredList = _this.list;
            _this.currentPage = 1;
            _this.pageSize = PAGE_SIZE;
            _this.pageLength = Math.ceil(_this.filteredList.length / PAGE_SIZE);

            _this.updatePageIndexes();

            _this.filterText = "";
            UIService.log(_this.filteredList);
            UIService.log('currentPage: ' + _this.currentPage);
            UIService.log('pageLength: ' + _this.filteredList.length);
            UIService.log('PAGESIZE: ' + _this.pageSize);
        };

        _this.init();
    }

    DateModalController.$inject = ['ApiService', 'ModalService', 'UIService'];
    function DateModalController(ApiService, ModalService, UIService) {
        var _this = this;
    }

    angular.module('ehrApp')
        .controller('PayerModalController', PayerModalController)
        .controller('GroupModalController', GroupModalController)
        .controller('ProviderModalController', ProviderModalController)
        .controller('FacilityModalController', FacilityModalController)
        .controller('PosModalController', PosModalController)
        .controller('DependentsModalController', DependentsModalController)
        .controller('AddDependentModalController', AddDependentModalController)
        .controller('TaxonomyModalController', TaxonomyModalController);

})();