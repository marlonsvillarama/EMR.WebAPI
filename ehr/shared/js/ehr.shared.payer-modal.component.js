(function () {

    function PayerModalComponentController() {
        var payerModal = this;


    }

    angular.module('ehrApp')
        .component('payerModal', {
            templateUrl: "/ehr/shared/views/ehr.shared.payer-modal.component.html",
            controller: PayerModalComponentController,
            bindings: {
                items: '<',
                title: 'Payers',
                onRemove: '&'
            }
        });

})();
