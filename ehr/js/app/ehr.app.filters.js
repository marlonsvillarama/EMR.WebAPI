(function () {
    'use strict';

    angular.module('ehrApp').filter('tel', function () {
        return function (str) {
            var value = parseNumbers(str);
            var area, num;

            if (value.length == 10) {
                area = value.slice(0, 3);
                num = value.slice(3);
                return '(' + area + ') ' + num.slice(0, 3) + '-' + num.slice(3);
            }
            else {
                return str;
            }
        }
    });

    angular.module('ehrApp').filter('zip', function () {
        return function (str) {
            var value = parseNumbers(str);
            var zip, ext;

            if (value.length == 5) {
                return value;
            }
            else if (value.length == 9) {
                return value.slice(0, 5) + '-' + value.slice(5);
            }
            else {
                return str;
            }
        }
    });

    angular.module('ehrApp').filter('startFrom', function () {
        return function (input, start) {
            if (!input || !input.length) return;
            //start += start;
            return input.slice(start);
        }
    });

    function parseNumbers(str) {
        if (!str) {
            return '';
        }

        var value = str.toString().trim().replace(/^\+/, '');
        if (value.match(/[^0-9]/)) {
            return str;
        }

        return value;
    }

})();

