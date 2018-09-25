RoutesConfig.$inject = ['$stateProvider', '$urlRouterProvider'];
function RoutesConfig($stateProvider, $urlRouterProvider){
    // TO BE IMPLEMENTED
    /*$urlRouterProvider
        .otherwise('/master-view');
    */

    $stateProvider
        .state('faciliites-list', {
            url: '/facilities',
            templateUrl: '/ehr/facilities/facilities-list.html',
            controller: function ($scope, $http) {
                
                url = apiBase + 'get' + p;
                $http.get(url).then(function (response) {
                    var res = response.data.results;
                    if (res.IsSuccess == true) {
                        $scope.entityList = res.Data;
                    }
                    else {
                        alert(res.Data.toString());
                    }
                });
            }
        })

        .state('facilities-get', {
            url: '/facilities/get/:id',
            templateUrl: '/ehr/facilities/facilities-get.html',
            controller: function ($scope, $http, $stateParams) {
                url = apiBase + 'get' + s + 'ById/' + $stateParams.id;
                $http.get(url).then(function (response) {
                    var res = response.data.result;
                    if (res.IsSuccess == true) {
                        $scope.entity = res.Data;
                    }
                    else {
                        alert(res.Data.toString());
                    }
                });
            }
        })

        .state('facilities-edit', {
            url: '/' + p + '/edit/:id',
            templateUrl: '/ehr/' + p + '/' + p + '-edit.html',
            controller: function ($scope, $http, $state, $stateParams) {
                if (parseInt($stateParams.id) >= 0) {
                    url = apiBase + 'get' + s + 'ById/' + $stateParams.id;
                    $scope.page_title = 'EDIT ' + s.toUpperCase();
                    $http.get(url).then(function (response) {
                        var res = response.data.result;
                        if (res.IsSuccess == true) {
                            $scope.entity = response.data.result.Data;
                        }
                        else {
                            alert(res.Data.toString());
                        }
                    });
                }
                else {
                    $scope.page_title = 'CREATE NEW ' + s.toUpperCase();
                }
            }
        })
}

