(function () {
    'use strict';

    ClaimEditService.$inject = ['$stateParams', 'ApiService', 'UIService'];
    function ClaimEditService($stateParams, ApiService, UIService) {
        const MAX_CLAIM_LINES = 6;

        var lineProps = ['pos', 'emg', 'cpt', 'mod', 'ptr', 'charges', 'unit', 'epsdt', 'provider']; //'from', 'to',
        var service = this;
        var dos = '';

        var clm = null;
        var history = null;
        var sub = null;
        var lists = {};
        var groups = null;
        var facilities = null;
        var providers = null;
        var billingProv = null;
        var renderingProv = null;
        var facilityProv = null;
        var lines = [];
        var claimLine = {};
        var newLine = {}

        service.MAX_CLAIM_LINES = MAX_CLAIM_LINES;

        service.getNewLine = function () {
            newLine = {
                StartDate: UIService.parseDate(clm.DateOfService),
                EndDate: UIService.parseDate(clm.DateOfService),
                IsEmergency: "",
                CPT: "",
                Modifier: "",
                Pointer: "",
                Amount: "",
                Quantity: "",
                EPSDT: false
            }
            return newLine;
        };

        service.getClaim = function () {
            return clm;
        };

        service.setClaim = function (claim) {
            clm = claim;
            newLine = service.getNewLine()
        };

        service.getClaimHistory = function () {
            return history;
        };

        service.setClaimHistory = function (hst) {
            history = hst;
        };

        service.getList = function (type) {
            return lists[type];
        };

        service.setList = function (type, obj) {
            lists[type] = obj;
        };

        service.getSubscriber = function () {
            return sub;
        };

        service.setSubscriber = function (subscriber) {
            sub = subscriber;
        };

        service.formatLineDates = function () {
            for (var i = 0, n = clm.ClaimLines.length; i < n; i++) {
                var str = clm.ClaimLines[i].StartDate;
                var dt = new Date(str);
                clm.ClaimLines[i].StartDate = (dt.getMonth() + 1) + "/" +
                    dt.getDate() + "/" + dt.getFullYear();

                str = clm.ClaimLines[i].EndDate;
                dt = new Date(str);
                clm.ClaimLines[i].EndDate = (dt.getMonth() + 1) + "/" +
                    dt.getDate() + "/" + dt.getFullYear();
            }
        };

        service.updateDateOfService = function () {
            try {
                if (clm.DateOfService) {
                    dos = new Date(clm.DateOfService);
                }
                else {
                    dos = new Date();
                }
            }
            catch (e) {
                UIService.log(e);
                return;
            }

            dos = (dos.getMonth() + 1) + "/" +
                dos.getDate() + "/" +
                dos.getFullYear();
            UIService.log('dos: ' + dos);
            
            for (var i = 0, n = clm.ClaimLines.length; i < n; i++) {
                clm.ClaimLines[i].StartDate = dos;
                clm.ClaimLines[i].EndDate = dos;
            }

            newLine.StartDate = dos;
            newLine.EndDate = dos;
        };

        // TO BE DELETED
        service.updateRenderingNPI = function (npi) {
            for (var i = 0, n = clm.ClaimLines.length; i < n; i++) {
                clm.ClaimLines[i].NPI = npi;
            }
        }

        // TO BE DELETED
        service.setBillingProvider = function () {
            ApiService.getEntity('Provider', clm.BillingProviderId)
                .then(function (response) {
                    var r = ApiService.prepareResponse(response);
                    billingProv = r.success == true ? r.entityList : null;
                });
        };

        // TO BE DELETED
        service.getBillingProvider = function () {
            return billingProv;
        };

        // TO BE DELETED
        service.setRenderingProvider = function () {
            ApiService.getEntity('Provider', clm.RenderingProviderId)
                .then(function (response) {
                    var r = ApiService.prepareResponse(response);
                    renderingProv = r.success == true ? r.entityList : null;
                });
        };

        // TO BE DELETED
        service.getRenderingProvider = function () {
            return renderingProv;
        };

        // TO BE DELETED
        service.setFacilityProvider = function () {
            ApiService.getEntity('Facility', clm.FacilityId)
                .then(function (response) {
                    var r = ApiService.prepareResponse(response);
                    facilityProv = r.success == true ? r.entityList : null;
                });
        };

        // TO BE DELETED
        service.getFacilityProvider = function () {
            return facilityProv;
        };

        service.getDateOfService = function () {
            UIService.log('ClaimEditService.getDateOfService = ' + dos);
            return dos;
        };

        // TO BE DELETED
        service.addClaimLine1 = function (obj) {
            var i = 0;
            claimLine = {};

            if (lines.length < MAX_CLAIM_LINES) {
                for (var k in obj) {
                    if (obj.hasOwnProperty(k) == true &&
                        lineProps.indexOf(k) >= 0 &&
                        obj[k] != '' &&
                        obj[k] != null &&
                        obj[k] != undefined) {

                        claimLine[k] = obj[k];
                        i++;
                    }
                }

                //if (i == lineProps.length) {
                //}
                claimLine.from = dos;
                claimLine.to = dos;
                lines.push(claimLine);

                for (var i = 0, n = lines.length; i < n; i++) {
                    lines[i].order = i;
                }
            }
        }

        service.recalcAmtTotal = function () {
            var sum = clm.ClaimLines.reduce(function (t, v) {
                return (+t) + (+v.Amount);
            }, 0);
            clm.AmountTotal = sum;
        };

        service.getClaimLines = function () {
            return lines;
        };

        service.canEditLines = function () {
            if (lines.length >= MAX_CLAIM_LINES ||
                clm.Payment.Id > 0) {
                UIService.log('canEditLines: false, lines: ' + lines.length + ', payment: ' + clm.Payment.Id);
                return false;
            }

            return true;
        };

        service.addLineToClaim = function (line) {
            clm.ClaimLines.push({
                Id: -1,
                StartDate: clm.DateOfService,
                EndDate: clm.DateOfService,
                IsEmergency: (line.IsEmergency ? true : false),
                CPT: line.CPT,
                Modifier: line.Modifier,
                Pointer: line.Pointer,
                Amount: line.Amount,
                Quantity: line.Quantity,
                Unit: line.Unit,
                EPSDT: (line.EPSDT ? true : false)
            });
            service.setClaimLineOrders();
            service.recalcAmtTotal();
        };

        service.addClaimLine = function () {
            if (!newLine.CPT || !newLine.Amount || !newLine.Quantity || !newLine.Unit) {
                return false;
            }

            service.addLineToClaim(newLine);
        };

        service.copyClaimLine = function (index) {
            UIService.log('service.copyClaimLine');
            var line = clm.ClaimLines[index];
            UIService.log(line);
            service.addLineToClaim(line);
        }

        service.removeClaimLine = function (index) {
            clm.ClaimLines.splice(index, 1);
            service.setClaimLineOrders();
            service.recalcAmtTotal();
        }

        service.setClaimLineOrders = function () {
            for (var i = 0, n = clm.ClaimLines.length; i < n; i++) {
                clm.ClaimLines[i].OrderLine = (i + 1);
            }
            UIService.log(clm);
        };

    }

    angular.module('ehrApp').service('ClaimEditService', ClaimEditService);
})();


