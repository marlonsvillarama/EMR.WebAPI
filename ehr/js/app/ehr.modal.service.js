(function () {

    ModalService.$inject = ['ngDialog', 'ApiService'];
    function ModalService(ngDialog, ApiService) {
        var service = this;

        service.PAGE_SIZE = 10;

        service.openModalPayers = function () {
            ApiService.getEntityList('payers', 'Payer')
                .then(function (d) {
                    return ngDialog.openConfirm({
                        template: '/ehr/shared/views/ehr.shared.payer-modal.component.html'
                    });
                });
        };
    }

    angular.module('ehrApp').service('ModalService', ModalService);

})();

