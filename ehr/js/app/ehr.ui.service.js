(function () {
    'use strict';

    UIService.$inject = ['$stateParams', 'AuthService'];
    function UIService($stateParams, AuthService) {
        var service = this;

        service.months = ['January', 'February', 'March', 'April', 'May', 'June',
                            'July', 'August', 'September', 'October', 'November', 'December'];

        service.extractDigit = function (a, bitMask, shiftRightAmt) {
            var digit = (a & bitMask) >>> shiftRightAmt;
            return digit;
        };

        service.sortList = function (obj) {
            var bins = 256;
            var radix = 8;
            var arrOutput = new Array(obj.length);
            var count = new Array(bins);
            var result = false;
            var i, curr;

            var bitMask = 255;
            var shiftRightAmt = 0;

            var binStart = new Array(bins);
            var binEnd = new Array(bins);

            while (bitMask != 0) {
                for (i = 0; i < bins; i++) {
                    count[i] = 0;
                }

                for (curr = 0; curr < obj.length; curr++) {
                    count[extractDigit(obj[curr], bitMask, shiftRightAmt)]++;
                }

                binStart[0] = binEnd[0] = 0;
                for (i = 1; i < bins; i++) {
                    binStart[i] = binEnd[i] = binStart[i - 1] + count[i - 1];
                }

                for (curr = 0; curr < obj.length; curr++) {
                    arrOutput[binEnd[extractDigit(obj[curr], bitMask, shiftRightAmt)]++] = obj[curr];
                }

                bitMask <<= radix;
                shiftRightAmt += radix;
                result = !result;

                var tmp = obj, obj = arrOutput, arrOutput = tmp;
            }

            if (result) {
                for (curr = 0; curr < obj.length; curr++) {
                    obj[curr] = arrOutput[curr];
                }
            }

            return obj;
        };

        service.getPage = function (currPage, inc) {
            if (inc == '' || inc == null || inc == undefined) {
                currPage = 1;
            }
            else {
                currPage += inc;

                if (currPage < 1) {
                    currPage = 1;
                }
            }

            return currPage;
        };

        service.log = function (obj) {
            if (AppConfig.AllowLogging == true) {
                console.log(obj);
            }
        };

        // TO BE DELETED
        service.applyMasks = function () {
            console.log('applyMasks');
        };

        service.applyDatePickers = function () {
            console.log('applyDatePickers');
            jQuery('[data-toggle="datepicker"]').datepicker({
                autoHide: true
            });
        };

        service.calculateAge = function (dt) {
            return ~~((Date.now() - dt) / (31557600000));
        };

        service.getDateParam = function (dtIn) {
            var dt = dtIn ? new Date(dtIn) : new Date();
            return service.padLeft(dt.getDate(), 2, '0') +
                service.padLeft((dt.getMonth() + 1), 2, '0') +
                dt.getFullYear();
        };

        service.parseDate = function (str) {
            var dt = '';
            var output = '';

            dt = str ? new Date(str) : new Date();

            output = (dt.getMonth() + 1) + '/' +
                dt.getDate() + '/' +
                dt.getFullYear();

            return output;
        };

        service.getGender = function (str) {
            if (!str) {
                return "";
            }

            return (str.toUpperCase() == 'M' ? 'Male' : 'Female');
        };

        service.padLeft = function (str, length, ch) {
            if (!str) {
                return "";
            }

            if (!length || length <= 0) {
                return "";
            }

            var s = '';
            for (var i = 0; i < length; i++) {
                s += ch;
            }

            return (s + str).slice(-length);
        };

        service.formatAcctNumber = function (id) {
            var str = '';

            if (id > 0) {
                str = AuthService.getAccountName().substring(3, 6).toUpperCase() +
                    '-' + service.padLeft(id, 6, '0');
            }
            else {
                str = '< New >';
            }

            return str;
        }

        service.getStatusText = function (status) {
            var str = '';
            switch (status) {
                case 'P': {
                    str = 'Processed';
                    break;
                }
                case 'S': {
                    str = 'Submitted';
                    break;
                }
                default: {
                    str = 'Unprocessed';
                    break;
                }
            }
            return str;
        };

        service.getEditPageTitle = function (type, id) {
            var title = '';

            if (id && id > 0) {
                title = "EDIT " + type.toUpperCase() + ' - ' + service.formatAcctNumber(id);
            }
            else {
                title = "CREATE " + type.toUpperCase();
            }

            return title;
        }

        // Format the claim history for display
        service.formatClaimHistory = function (history) {
            var clmHist = [];

            for (var i = 0, n = history.length; i < n; i++) {
                var ch = history[i];
                console.log(ch);
                clmHist.push({
                    id: ch.Id,
                    acct: service.padLeft(ch.Id, 5, '0'),
                    date: service.parseDate(ch.DateOfService),
                    amt: ch.AmountTotal,
                    pmt: ch.AmountPayment,
                    balance: ch.AmountBalance
                });
            }

            return clmHist;
        };

        service.parseDiagCodes = function (codes, n) {
            var arr = [], output = [];
            var str = '';

            arr = codes.split(',');
            if (n) {
                for (var i = 0; i < n; i++) {
                    output.push(arr[i]);
                    str = arr.join('             |            ');
                }
            }
            else {
                output = arr;
                str = arr.join('             |            ');
            }

            //return output;
            return str;
        };

        service.getDateFromRange = function (range) {

        };

    }

    angular.module('ehrApp').service('UIService', UIService);

})();


