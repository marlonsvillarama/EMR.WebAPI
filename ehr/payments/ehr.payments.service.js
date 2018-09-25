(function () {
    'use strict';

    PaymentEditService.$inject = ['$stateParams', 'ApiService', 'UIService'];
    function PaymentEditService($stateParams, ApiService, UIService) {
        var _this = this;

        _this.getAcctNum = function () {
            return UIService.formatAcctNumber(_this.claim.Id);
        };

        _this.setClaim = function (claim) {
            _this.claim = claim;
        };

        _this.getClaim = function () {
            return _this.claim;
        };

        _this.setPayment = function (payment) {
            _this.payment = payment;
        };

        _this.getPayment = function () {
            return _this.payment;
        };

        _this.setPaymentLines = function (lines) {
            _this.payment.PaymentLines = lines;
        };

        _this.getPaymentLines = function () {
            return _this.payment.PaymentLines;
        };

        _this.calculateTotals = function () {
            var totalCharges = 0;
            var totalPayment = 0;
            var totalCopay = 0;
            var totalDeductible = 0;
            var i, n, line;

            for (i = 0, n = _this.claim.ClaimLines.length; i < n; i++) {
                line = _this.claim.ClaimLines[i];
                totalCharges += +line.Amount;
            }

            for (i = 0, n = _this.payment.PaymentLines.length; i < n; i++) {
                line = _this.payment.PaymentLines[i];
                totalPayment += +line.AmountPayment;
                totalCopay += +line.AmountCopay;
                totalDeductible += +line.AmountDeductible;
            }

            return {
                charges: totalCharges,
                payment: totalPayment,
                copay: totalCopay,
                deductible: totalDeductible
            };
        };

        _this.getErrorCode = function (errorId) {
            var err = null;
            var list = ApiService.getList('errorcodes');

            for (var i = 0, n = list.length; i < n; i++) {
                var ec = list[i];
                if (ec.Id == errorId) {
                    err = ec;
                }
            }

            return err;
        };
    }

    angular.module('ehrApp').service('PaymentEditService', PaymentEditService);
})();

