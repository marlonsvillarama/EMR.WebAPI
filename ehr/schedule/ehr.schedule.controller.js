(function () {
    'use strict';
    
    ehrScheduleController.$inject = [];
    function ehrScheduleController() {
        var _this = this;

        _this.init = function () {
            var options = {
                zoom: 7,
                center: new google.maps.LatLng(45.50867, -73.553992),
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                disableDefaultUI: true
            };

            var map = new google.maps.Map(jQuery('#ehr-map')[0]);
            _this.map = map;
        };

        _this.init();
    }

    angular.module('ehrApp')
        .controller('ehrScheduleController', ehrScheduleController);
})();