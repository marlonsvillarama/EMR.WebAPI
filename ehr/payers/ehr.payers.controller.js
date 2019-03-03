(function () {
    'use strict';

    ehrPayerListController.$inject = ['ApiService'];
    function ehrPayerListController(ApiService) {
        var _this = this;

        _this.init = function () {
            _this.loading = true;
            ApiService.getEntityList('Payers', 'payer')
                .then(function (response) {
                    var resp = ApiService.prepareResponse(response);
                    _this.entityList = resp.entityList;
                    _this.loading = false;
                }
            );
        };

        _this.init();
    }

})();