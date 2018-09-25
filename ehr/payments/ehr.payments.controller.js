(function () {
    'use strict';

    ehrPaymentListController.$inject = ['ApiService'];
    function ehrPaymentListController(ApiService) {
        var _this = this;

        _this.init = function () {
            _this.loading = true;
            ApiService.getEntityList('Payments', 'payment')
                .then(function (response) {
                    var resp = ApiService.prepareResponse(response);
                    _this.entityList = resp.entityList;
                    _this.loading = false;
                }
            );
        };

        _this.init();
    }

    ehrPaymentEditController.$inject = ['$state', 'claim', 'history', 'ApiService', 'PaymentEditService', 'UIService'];
    function ehrPaymentEditController($state, claim, history, ApiService, PaymentEditService, UIService) {
        var _this = this;

        _this.switchClaim = function (id) {
            UIService.log('switchClaim: ' + id + ', ' + _this.claim.Id);
            if (id != _this.claim.Id) {
                $state.go('payment-edit', {
                    subscriberId: _this.claim.PrimarySubscriber.Id,
                    claimId: id
                });
            }
        };

        _this.validateForm = function () {
            UIService.log('_this.validateForm');
            _this.errors = [];

            var obj = PaymentEditService.calculateTotals();

            if (obj.payment <= 0) {
                _this.errors.push('Total Payment Amount must be greater than zero (0).');
            }

            if (obj.payment > obj.charges) {
                _this.errors.push('Total Payment Amount is greater than the Total Charges');
            }
        };

        _this.savePayment = function (claim) {
            UIService.log('savePayment');
            UIService.log(claim);

            _this.savingForm = true;

            ApiService.updateEntity({
                type: 'Payment',
                entity: claim.Payment,
                refId: claim.Id
            }).then(function (response) {
                var resSuccess = response.data.result.IsSuccess;
                var resData = response.data.result.Data;
                UIService.log(resData);

                _this.savingForm = false;

                if (resSuccess == true) {
                    if (claim.Payment.Id > 0) {
                        alert('Changes saved!');
                    }
                    else {
                        alert('Payment successfully created!');
                    }

                    if (resData != claim.Payment.Id) {
                        UIService.log('redirecting - subscriberId: ' + claim.PrimarySubscriber.Id + ', claimId: ' + claim.Id);

                        _this.payment.Id = resData;
                        _this.initUI();
                    }

                    //_this.init();
                }
                else {
                    UIService.log('exception: ' + resData.toString());
                }
            });
        };

        // Submit claim form
        _this.submitForm = function (copy) {
            UIService.log('_this.submitForm');

            _this.validateForm();
            if (_this.errors.length > 0) {
                var str = 'Please correct the following errors first:\n\n';
                for (var i = 0, n = _this.errors.length; i < n; i++) {
                    str += '- ' + _this.errors[i] + '\n';
                }
                alert(str);

                _this.savingForm = false;
                return;
            }

            if (_this.totalPayment <= 0) {
            }

            _this.savePayment(_this.claim);
        }

        _this.updateErrorCode = function () {
            if (_this.errorId) {
                _this.payment.ErrorCode = PaymentEditService.getErrorCode(_this.errorId);
            }

            UIService.log('updateErrorCode');
            UIService.log(_this.payment);
        }

        _this.cancelEdit = function () {
            if (confirm('Are you sure you want to leave this page?')) {
                $state.go('claim-edit', {
                    subscriberId: _this.claim.PrimarySubscriber.Id,
                    claimId: _this.claim.Id
                });
            }
        };

        _this.initUI = function () {
            _this.date = UIService.parseDate();
            _this.acctNum = UIService.formatAcctNumber(_this.payment.Id);
            _this.claimStatus = UIService.getStatusText(_this.claim.FilingStatus);
            _this.page_title = UIService.getEditPageTitle('payment', _this.payment.Id);
            _this.savingForm = false;
        };

        _this.init = function () {
            UIService.log('fire ehrPaymentEditController');
            //UIService.log(payment);
            UIService.log(claim);
            UIService.log(history);

            PaymentEditService.setClaim(claim.entityList);
            _this.claim = PaymentEditService.getClaim();

            PaymentEditService.setPayment(_this.claim.Payment);
            _this.payment = PaymentEditService.getPayment();
            UIService.log(_this.payment);

            _this.errorCodes = ApiService.getList('errorcodes');

            if (_this.payment.ErrorCode && _this.payment.ErrorCode.Id > 0) {
                _this.errorId = _this.payment.ErrorCode.Id;
                UIService.log('_this.errorId: ' + _this.errorId);
            }

            _this.initUI();
            // Render the first subtab
            //$state.go('payment-edit.subscriber');
        };

        _this.init();
    }

    ehrPaymentLinesController.$inject = ['ApiService', 'PaymentEditService', 'UIService'];
    function ehrPaymentLinesController(ApiService, PaymentEditService, UIService) {
        var _this = this;

        _this.calculateTotals = function () {
            var obj = PaymentEditService.calculateTotals();
            _this.totalPayment = obj.payment;
            _this.totalCopay = obj.copay;
            _this.totalDeductible = obj.deductible;
        };

        _this.getPaymentLine = function (lineId) {
            var line = {};
            for (var i = 0, n = _this.payment.PaymentLines.length; i < n; i++) {
                var l = _this.payment.PaymentLines[i];
                if (l.ClaimLineId == lineId) {
                    line = l;
                }
            }

            return line;
        };

        _this.init = function () {
            UIService.log('fire ehrPaymentLinesController');

            _this.payment = PaymentEditService.getPayment();
            _this.claim = PaymentEditService.getClaim();

            var claimLines = _this.claim.ClaimLines;

            if (_this.payment.Id <= 0) {
                for (var i = 0, n = claimLines.length; i < n; i++) {
                    var line = claimLines[i];

                    _this.payment.PaymentLines.push({
                        ClaimLineId: line.Id,
                        AmountPayment: 0,
                        AmountCopay: 0,
                        AmountDeductible: 0
                    });
                }
            }

            _this.calculateTotals();
            UIService.log(_this.payment);
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrPaymentListController', ehrPaymentListController)
        .controller('ehrPaymentEditController', ehrPaymentEditController)
        .controller('ehrPaymentLinesController', ehrPaymentLinesController);
        //.config(paymentSubtabConfig);

})();

